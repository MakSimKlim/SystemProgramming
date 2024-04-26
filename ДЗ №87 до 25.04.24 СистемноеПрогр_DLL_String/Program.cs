using System;
using TextLibrary;

class Program
{
    static void Main()
    {
        //string text = "Madam Arora teaches malayalam. She was wearing a red colored dress. She felt Ill and was seeing blurred lines.";
        string text = "А роза упала на лапу Азора.";

        Console.WriteLine($"Text: {text}");                                  //текст палиндрома
        Console.WriteLine($"Is Palindrome?: {text.IsPalindrome()}");         //проверка на палиндром
        Console.WriteLine($"Word Count: {text.WordCount()}");                //подсчет количества слов
        Console.WriteLine($"Sentence Count: {text.SentenceCount()}");        //подсчет количества предложений
        Console.WriteLine($"Count of character 'а': {text.CharCount('а')}"); //подсчет количества опрееленных символов
        Console.WriteLine($"Total character count: {text.CharCountAll()}");  //подсчет количества всех символов
        Console.ReadKey(); 
    }
}