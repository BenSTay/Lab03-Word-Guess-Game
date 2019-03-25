using System;
using System.IO;
using System.Text;

namespace GuessingGame
{
    public class Program
    {
        public static readonly string filepath = "../../../words.txt";
        static Random rng = new Random();

        // CRUD Methods

        public static string[] InitializeWords()
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
            return words;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] ReadWords()
        {
            if (!File.Exists(filepath)) return InitializeWords();
            string[] words = File.ReadAllLines(filepath);
            if (words.Length < 1) return InitializeWords();
            return words;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteWords()
        {
            File.Delete(filepath);
        }

        static void RemoveWordFromFile(int index)
        {
            string[] words = ReadWords();
            using (StreamWriter sw = File.CreateText(filepath))
            {
                for (int i = 0; i < words.Length; i++)
                {
                    if (i != index) sw.WriteLine(words[i]);
                }
            }
        }

        // Guessing Game Methods

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="knownletters"></param>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static bool[] UpdateKnownLetters(bool[] knownletters, int[] positions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                knownletters[positions[i]] = true;
            }
            return knownletters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guesses"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static bool HasLetter(string guesses, char letter)
        {
            return guesses.Contains(letter);
        }

        //TODO: Write test(s)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="knownletters"></param>
        /// <returns></returns>
        public static bool AllLettersKnown(bool[] knownletters)
        {
            for (int i = 0; i < knownletters.Length; i++)
            {
                if (!knownletters[i]) return false;
            }
            return true;
        }

        //TODO: Write test(s)
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


        //TODO: Write test(s)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool WordOnlyContainsLetters(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (!Char.IsLetter(word, i)) return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static char GetLetter()
        {
            string input;
            bool invalidinput = true;
            do
            {
                Console.Write("\nPick a letter: ");
                input = Console.ReadLine().ToLower();
                if (input.Length != 1) Console.WriteLine("Please enter exactly one letter.");
                else if (!WordOnlyContainsLetters(input)) Console.WriteLine("Numbers and punctuation are not accepted. Please try again.");
                else invalidinput = false;

            } while (invalidinput);
            return input.ToCharArray()[0];
        }

        // UI Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="knownletters"></param>
        /// <param name="guesses"></param>
        static void DisplayGame(string word, bool[] knownletters, string guesses)
        {
            Console.Clear();
            Console.WriteLine("Guess The Word!\n");
            Console.WriteLine(FormatWord(word, knownletters));
            Console.WriteLine($"\nYour guesses: {guesses}");
        }

        static void DrawAdmin()
        {
            Console.Clear();
            Console.WriteLine("Wordguess Admin\n");
            Console.WriteLine("1) View All Words");
            Console.WriteLine("2) Add A Word");
            Console.WriteLine("3) Remove A Word");
            Console.WriteLine("4) Reset Words");
            Console.WriteLine("5) Back To Main Menu\n");
        }

        /// <summary>
        /// 
        /// </summary>
        static void DrawMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to WordGuess!\n");
            Console.WriteLine("1) Play Game");
            Console.WriteLine("2) Admin");
            Console.WriteLine("3) Quit\n");
        }

        //TODO: warn player before deleting file, write confirmatory message after completion.
        static void ResetWords()
        {
            Console.Write("WARNING: Words will be reset to default. Are you sure? (y/n): ");
            string input = Console.ReadLine().ToLower();
            if (input == "y")
            {
                InitializeWords();
                Console.WriteLine("Words reset.");
            }
            else Console.WriteLine("Operation cancelled.");
            Console.WriteLine("\nPress any key to continue..");
            Console.ReadKey();
        }

        //TODO: get valid word (lowercase!) from user input, 
        static void AddWord()
        {
            Console.Write("\nEnter your new word: ");
            string input = Console.ReadLine();
            if (input.Length < 3) Console.WriteLine("Word must be 3 letters or longer.");
            else if (WordOnlyContainsLetters(input))
            {
                WriteWord(input.ToLower());
                Console.WriteLine("Operation Successful.");
            }
            else Console.WriteLine("Word can only contain letters.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void ListWords()
        {
            string[] words = ReadWords();
            for (int i = 0; i < words.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {words[i]}");
            }
        }

        static void ViewWords()
        {
            Console.Clear();
            Console.WriteLine("All words currently in WordGuess:\n");
            ListWords();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void RemoveWord()
        {
            Console.Clear();
            ListWords();
            Console.WriteLine("\nEnter the number next to the word you would like to remove");
            Console.Write("> ");
            string input = Console.ReadLine();
            if (Int32.TryParse(input, out int index))
            {
                RemoveWordFromFile(index - 1);
                Console.WriteLine("Removed.");
            }
            else Console.WriteLine("Invalid input.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static bool Admin()
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ViewWords();
                    break;
                case "2":
                    AddWord();
                    break;
                case "3":
                    RemoveWord();
                    break;
                case "4":
                    ResetWords();
                    break;
                case "5":
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                    do
                    {
                        DrawAdmin();
                    } while (Admin());
                    break;
                case "3":
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