﻿using System;
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

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
