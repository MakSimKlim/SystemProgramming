using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pools
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void StartProgressBars_Click(object sender, RoutedEventArgs e)
        {
            // Очистка stackPanel перед добавлением новых прогресс-баров
            // Удалите только прогресс-бары
            for (int i = stackPanel.Children.Count - 1; i >= 0; i--)
            {
                if (stackPanel.Children[i] is ProgressBar)
                {
                    stackPanel.Children.RemoveAt(i);
                }
            }

            int numberOfProgressBars = int.Parse(NumberOfProgressBars.Text);
            for (int i = 0; i < numberOfProgressBars; i++)
            {
                ProgressBar progressBar = new ProgressBar() { Height = 30, Margin = new Thickness(0, 10, 0, 0) };
                stackPanel.Children.Add(progressBar);
            }

            foreach (var child in stackPanel.Children)
            {
                if (child is ProgressBar progressBar)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(StartProgressBar), progressBar);
                }
            }
        }


        private void StartProgressBar(object state)
        {
            ProgressBar progressBar = state as ProgressBar;

            this.Dispatcher.Invoke(() =>
            {
            progressBar.Foreground = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
            });

            for (int i = 0; i <= 100; i++)
            {
                this.Dispatcher.Invoke(() =>
                {
                    progressBar.Value = i;
                });
                Thread.Sleep(random.Next(10, 100)); // Время заполнения определяется случайным образом
            }
        }
    }
}