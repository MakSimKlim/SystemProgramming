using System;
using System.Threading;

class Program
{
    static int upperLimit;
    static CancellationTokenSource cts = new CancellationTokenSource();

    static void Main()
    {
        Console.Write("Введите верхнюю границу: ");
        upperLimit = int.Parse(Console.ReadLine());

        Thread thread1 = new Thread(PrintEvenNumbers);
        Thread thread2 = new Thread(PrintFibonacci);
        Thread thread3 = new Thread(PrintPowersOfTwo);
        Thread thread4 = new Thread(PrintStars);

        thread1.Start();
        thread2.Start();
        thread3.Start();
        thread4.Start();

        Console.ReadKey();
        
        cts.Cancel();
       

    }

    static void PrintEvenNumbers()
    {
        for (int i = 2; i <= upperLimit; i += 2)
        {
            if (cts.Token.IsCancellationRequested)
                break;
            Console.WriteLine($"Чётное число: {i}");
            Thread.Sleep(500);
        }
    }

    static void PrintFibonacci()
    {
        int a = 0, b = 1;
        while (b <= upperLimit)
        {
            if (cts.Token.IsCancellationRequested)
                break;
            Console.WriteLine($"Число Фибоначчи: {b}");
            int temp = a;
            a = b;
            b = temp + b;
            Thread.Sleep(500);
        }
    }

    static void PrintPowersOfTwo()
    {
        int number = 1;
        while (number <= upperLimit)
        {
            if (cts.Token.IsCancellationRequested)
                break;
            Console.WriteLine($"Степень двойки: {number}");
            number *= 2;
            Thread.Sleep(500);
        }
    }

    static void PrintStars()
    {
        string stars = "*";
        while (true)
        {
            if (cts.Token.IsCancellationRequested)
                break;
            Console.WriteLine(stars);
            stars += "*";
            Thread.Sleep(500);
            
        }
    }

   
}
