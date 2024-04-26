using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextLibrary
{
    public static class StringExtensions
    {
        // проверка на палиндром
        public static bool IsPalindrome(this string str)
        {
            var cleaned = new string(str.Where(char.IsLetter).ToArray()).ToLower();
            return cleaned.SequenceEqual(cleaned.Reverse());
        }
        // подсчет количества слов в строке
        public static int WordCount(this string str)
        {
            return str.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
        //подсчет количества предложений в строке
        public static int SentenceCount(this string str)
        {
            return Regex.Matches(str, @"[.!?]").Count;
        }
        //подсчет количества определенных символов в строке
        public static int CharCount(this string str, char c)
        {
            return str.Count(ch => ch == c);
        }
        //подсчет количества всех символов в строке
        public static int CharCountAll(this string str)
        {
            return str.Length;
        }
    }
}
