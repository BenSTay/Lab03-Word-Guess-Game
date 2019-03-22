using System;
using System.IO;
using System.Text;

namespace GuessingGame
{
    public class Program
    {
        public static readonly string filepath = "../../../words.txt";
        static Random rng = new Random();

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
            StringBuilder positions = new StringBuilder();
            for (int i = 0; i < wordchars.Length; i++)
            {
                if (wordchars[i] == letter) positions.Append($"{(positions.Length == 0 ? "" : ",")}{i}");
            }
            int[] result;
            if (positions.Length != 0)
            {
                string[] splitpositions = positions.ToString().Split(',');
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

        public static bool HasLetter(string guesses, char letter)
        {
            return guesses.Contains(letter);
        }

        static void DrawMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to WordGuess!\n");
            Console.WriteLine("1) Play Game");
            Console.WriteLine("2) Add Word");
            Console.WriteLine("3) Reset Words");
            Console.WriteLine("4) Quit\n");
        }

        //TODO: Write test(s)
        public static bool WordOnlyContainsLetters(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (!Char.IsLetter(word, i)) return false;
            }
            return true;
        }

        public static char GetLetter()
        {
            string input;
            bool invalidinput = true;
            do
            {
                Console.Write("\nPick a letter: ");
                input = Console.ReadLine();
                if (input.Length != 1) Console.WriteLine("Please enter exactly one letter.");
                else if (!WordOnlyContainsLetters(input)) Console.WriteLine("Numbers and punctuation are not accepted. Please try again.");
                else invalidinput = false;

            } while (invalidinput);
            return input.ToCharArray()[0];
        }

        //TODO: Write test(s)
        public static bool AllLettersKnown(bool[] knownletters)
        {
            for (int i = 0; i < knownletters.Length; i++)
            {
                if (!knownletters[i]) return false;
            }
            return true;
        }

        //TODO: Write tests(s)
        public static string FormatWord(string word, bool[] knownletters)
        {
            char[] letters = word.ToCharArray();
            StringBuilder wordbuilder = new StringBuilder();
            for (int i = 0; i < word.Length; i++)
            {
                wordbuilder.Append($"{(knownletters[i] ? letters[i] : '_')} ");
            }
            return wordbuilder.ToString();
        }

        static void DisplayGame(string word, bool[] knownletters, string guesses)
        {
            Console.Clear();
            Console.WriteLine("Guess The Word!\n");
            Console.WriteLine(FormatWord(word, knownletters));
            Console.WriteLine($"\nYour guesses: {guesses}");
        }

        static void Play()
        {
            string[] words = ReadWords();
            string word = words[rng.Next(words.Length)];
            bool[] knownletters = new bool[word.Length];
            char letter;
            int[] positions;
            StringBuilder guesses = new StringBuilder();
            do
            {
                DisplayGame(word, knownletters, guesses.ToString());
                letter = GetLetter();
                if (!HasLetter(guesses.ToString(), letter))
                {
                    positions = GetLetterPositions(word, letter);
                    knownletters = UpdateKnownLetters(knownletters, positions);
                    guesses.Append(letter);
                }
            } while (!AllLettersKnown(knownletters));

            DisplayGame(word, knownletters, guesses.ToString());
            Console.WriteLine("\nYou got it!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static bool Game()
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Play();
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            do
            {
                DrawMenu();
            } while (Game());
        }
    }
}
