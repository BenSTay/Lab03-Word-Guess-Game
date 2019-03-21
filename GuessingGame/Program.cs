using System;
using System.IO;

namespace GuessingGame
{
    public class Program
    {
        public static readonly string filepath = "../../../../words.txt";

        public static string[] ReadWords()
        {
            if (!File.Exists(filepath))
            {
                string[] words = new string[]
                {
                    "pizza",
                    "party",
                    "animal",
                    "desire",
                    "feature",
                    "complete"
                };
                File.WriteAllLines(filepath, words);
            }
            return File.ReadAllLines(filepath);
        }

        public static bool WriteWord(string word)
        {
            if (File.Exists(filepath))
            {
                string[] words = ReadWords();
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i] == word)
                    {
                        return false;
                    }
                }
            }
            using (StreamWriter w = File.AppendText(filepath))
            {
                w.WriteLine(word);
            }
            return true;
        }

        public static void DeleteWords()
        {
            File.Delete(filepath);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
