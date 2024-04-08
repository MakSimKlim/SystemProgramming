using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)                        // Обработчик события нажатия на кнопку
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();     // Создаем диалоговое окно для выбора файла

            Nullable<bool> result = dlg.ShowDialog();                                      // Открываем диалоговое окно и получаем результат (true, если файл был выбран)

            if (result == true)                                                            // Если файл был выбран...
            {
                string filename = dlg.FileName;                                            // Получаем путь к выбранному файлу

                FileStream fs = new FileStream(                                            // Создаем поток для чтения файла
                    filename, 
                    FileMode.Open, 
                    FileAccess.Read, 
                    FileShare.Read, 
                    1024, 
                    FileOptions.Asynchronous);

                new AsyncCallbackReader(fs, delegate (byte[] data)                         // Создаем новый экземпляр AsyncCallbackReader для асинхронного чтения файла
                {
                                                                                           // Обновляем текстовый блок с содержимым файла
                                                                                           // Dispatcher.Invoke используется, чтобы обновить пользовательский интерфейс из другого потока
                    this.Dispatcher.Invoke(() =>
                    {
                        textBox.Text += $"============= {filename} =============\n{Encoding.UTF8.GetString(data)}\n";
                    });
                });
            }
        }
    }
    internal class AsyncCallbackReader                                                     // Определяем класс AsyncCallbackReader для асинхронного чтения файла
    {
        FileStream fs;                                                                     // Объявляем необходимые поля
        byte[] data;
        IAsyncResult aRes;
        AsyncBytesReadDel callbackMethod;
        public AsyncCallbackReader(FileStream fs, AsyncBytesReadDel cbM)                   // Конструктор класса AsyncCallbackReader
        {
            this.fs = fs;                                                                  // Инициализируем поля
            this.callbackMethod = cbM;
            data = new byte[fs.Length];                                                    // создаем новый буфер data, размер которого равен размеру файла fs.Length
                                                                                           // Запускается асинхронная операция чтения из файла fs в буфер data. 
            aRes = fs.BeginRead(data, 0, data.Length, ReadIsComplete, null);               // Параметры 0 и data.Length указывают, что чтение должно начаться с начала буфера data и продолжаться до его конца, то есть, считываются все данные из файла
        }
        public void ReadIsComplete(IAsyncResult res)                                       // Callback-метод, который вызывается, когда чтение файла завершено
        {
            int countByte = fs.EndRead(res);                                               // Завершаем чтение файла
            fs.Close();                                                                    // Закрываем поток
            Array.Resize(ref data, countByte);                                             // Изменяем размер массива данных, чтобы он соответствовал количеству прочитанных байт
            callbackMethod(data);                                                          // Вызываем callback-метод с данными из файла
        }
    }
    public delegate void AsyncBytesReadDel(byte[] streamData);                             // Определяем делегат для callback-метода
}
