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

        public static int[] GetLetterPositions(string word, char letter)
        {
            char[] wordchars = word.ToCharArray();
            string positions = "";
            for (int i = 0; i < wordchars.Length; i++)
            {
                if (wordchars[i] == letter) positions += $"{(positions.Length == 0 ? "" : ",")}{i}";
            }
            int[] result;
            if (positions != "")
            {
                string[] splitpositions = positions.Split(',');
                result = new int[splitpositions.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = Int32.Parse(splitpositions[i]);
                }
            }
            else result = new int[0];
            return result;
        }

        public static bool[] UpdateKnownLetters(bool[] knownletters, int[] positions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                knownletters[positions[i]] = true;
            }
            return knownletters;
        }

        public static string UpdateGuesses(string guesses, char letter)
        {
            if (!guesses.Contains(letter)) guesses += letter;
            return guesses;
        }

        static void Main(string[] args)
        {
            GetLetterPositions("bacon", 'z');
            Console.WriteLine("Hello World!");
        }
    }
}
