using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.Threading;


namespace ДЗ__86_до_20._04._24_СистемноеПрогр_ТаймерыМониторы
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer autosaveTimer;
        private object lockObject = new object();
        private int wordCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            //реализация команды Delete
            CommandBinding deleteCommandBinding = new CommandBinding(
            ApplicationCommands.Delete,
            ExecuteDeleteCommand,
            CanExecuteDeleteCommand);
            this.CommandBindings.Add(deleteCommandBinding);

            // Создание нового таймера
            autosaveTimer = new DispatcherTimer();
            // Установка интервала в 30 секунд
            autosaveTimer.Interval = TimeSpan.FromSeconds(30);
            // Добавление обработчика событий
            autosaveTimer.Tick += AutosaveTimer_Tick;
            // Запуск таймера
            autosaveTimer.Start();

        }

        // Автосохранение текста из textBox в файл autosave.txt
        private void AutosaveTimer_Tick(object sender, EventArgs e)
        {
            File.WriteAllText("autosave.txt", textBox.Text);
        }

        //обработчик события для пункта меню Поиск слова
        private void SearchWord_Click(object sender, RoutedEventArgs e)
        {
            // Запросите у пользователя слово для поиска
            var inputDialog = new InputBox("Введите слово для поиска:");
            if (inputDialog.ShowDialog() == true)
            {
                string wordToSearch = inputDialog.ResponseText;
                SearchWord(wordToSearch);
            }
        }

        //поиск количества вхождений слова в тексте в нескольких потоках с помощью механизма Монитор и Lock

        //Функция SearchWordInText ищет слово в тексте и увеличивает счетчик wordCount при каждом вхождении
        private void SearchWordInText(string text, string word)
        {
            int count = 0;
            int index = 0;

            // Поиск всех вхождений, включая перекрывающиеся
            while ((index = text.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index++; // Перемещаем индекс на один символ вперед для поиска перекрывающихся вхождений
            }

            // Безопасное обновление общего количества вхождений в многопоточной среде
            lock (lockObject)
            {
                wordCount += count;
            }
        }

        //Этот подход позволяет избежать проблемы разделения слов пополам.
       
        public void SearchWord(string word)
        {
            string text = textBox.Text;
            int textLength = text.Length;
            int partLength = textLength / 3;

            // Находим безопасные точки разделения текста
            int end1 = FindEndOfWord(text, partLength);
            int start2 = end1;
            int end2 = FindEndOfWord(text, start2 + partLength);
            int start3 = end2;

            Thread thread1 = new Thread(() => SearchWordInText(text.Substring(0, end1), word));
            Thread thread2 = new Thread(() => SearchWordInText(text.Substring(start2, end2 - start2), word));
            Thread thread3 = new Thread(() => SearchWordInText(text.Substring(start3), word));

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();

            MessageBox.Show($"Количество вхождений слова '{word}': {wordCount}");
            wordCount = 0;
        }
        //Метод FindEndOfWord ищет ближайший пробел или символ конца строки, начиная от предполагаемой точки разделения,
        //и возвращает позицию, которая гарантированно не разрезает слово. Это обеспечивает,
        //что каждый поток обрабатывает полные слова
        private int FindEndOfWord(string text, int approximateEnd)
        {
            if (approximateEnd >= text.Length) return text.Length;
            while (approximateEnd < text.Length && !char.IsWhiteSpace(text[approximateEnd]))
                approximateEnd++;
            return approximateEnd;
        }
       
        //реализация добавления даты и времени в конец
        private void AddDateTime_Click(object sender, RoutedEventArgs e)
        {
            textBox.AppendText($"{Environment.NewLine}{DateTime.Now}");
        }
        // реализация меню выхода
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //реализация команды Delete
        private void CanExecuteDeleteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = textBox != null && textBox.SelectedText.Length > 0;
        }

        private void ExecuteDeleteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
            e.Handled = true;
        }

        // для команд Copy, Cut, Redo, Undo и Paste уже опредлены встроенные привязки.
        // Поэтому в данном случае нам не надо вносить в файл кода C# никаких изменений.
    }
}
