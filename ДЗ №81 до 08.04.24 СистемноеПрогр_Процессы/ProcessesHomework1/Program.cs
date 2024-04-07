using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        var processes = Process.GetProcesses();
        var svchostProcesses = processes.Count(p => p.ProcessName.Contains("svchost"));
        Console.WriteLine($"Количество процессов 'svchost': {svchostProcesses}");
        Console.ReadKey();
    }
}