using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ProcessesHomework2_WPF_
{
    public partial class MainWindow : Window
    {
        private Process process;
        private DateTime startTime;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            process = new Process();
            process.StartInfo.FileName = "E:/Родители/Папа/Системное программирование/ДЗ №81 до 08.04.24 СистемноеПрогр_Процессы/ProcessesHomework1/ProcessesHomework1/bin/Debug/ProcessesHomework1.exe";
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            startTime = DateTime.Now;
            process.Start();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                TimeSpan runTime = DateTime.Now - startTime;
                textBlock.Text = $"Приложение было запущено {runTime.TotalSeconds} секунд.";
            });
        }
    }
}
