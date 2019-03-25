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

        /// <summary>
        /// Creates a text file with default words.
        /// </summary>
        /// <returns>The default words.</returns>
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
        /// Gets all the words in words.txt
        /// </summary>
        /// <returns>The words in words.txt</returns>
        public static string[] ReadWords()
        {
            if (!File.Exists(filepath)) return InitializeWords();
            string[] words = File.ReadAllLines(filepath);
            if (words.Length < 1) return InitializeWords();
            return words;
        }

        /// <summary>
        /// Adds a word to words.txt
        /// </summary>
        /// <param name="word">The word being added.</param>
        /// <returns>true if the word is added, else false.</returns>
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
        /// Removes the word at the given index from words.txt
        /// </summary>
        /// <param name="index">The index of the word being deleted.</param>
        static void RemoveWordFromFile(int index)
        {
            string[] words = ReadWords();
            if (index >= words.Length) return;
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
        /// Handles the guessing game.
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
                
                if (!guesses.ToString().Contains(letter))
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
        /// Finds the positions of letters in a word that match a given letter.
        /// </summary>
        /// <param name="word">The word being checked.</param>
        /// <param name="letter">The letter being searched for.</param>
        /// <returns>An array with all of the indexes where the letter appears in the word.</returns>
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
        /// Flags the given letter positions as correctly guessed.
        /// </summary>
        /// <param name="knownletters">The array of letter position flags.</param>
        /// <param name="positions">The letter positions being set to true.</param>
        /// <returns>The updated array of known positions.</returns>
        public static bool[] UpdateKnownLetters(bool[] knownletters, int[] positions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                knownletters[positions[i]] = true;
            }
            return knownletters;
        }

        /// <summary>
        /// Checks if all of the letters have been guessed.
        /// </summary>
        /// <param name="knownletters">The array of letter position flags.</param>
        /// <returns>true if all letter position flags are marked true, else false.</returns>
        public static bool AllLettersKnown(bool[] knownletters)
        {
            for (int i = 0; i < knownletters.Length; i++)
            {
                if (!knownletters[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Formats a word so that letters that haven't been guessed yet are blanked out.
        /// </summary>
        /// <param name="word">The word being formatted</param>
        /// <param name="knownletters">The array of letter position flags.</param>
        /// <returns>The formatted string.</returns>
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

        /// <summary>
        /// Checks to see if a word contains numbers or punctuation.
        /// </summary>
        /// <param name="word">The word being checked.</param>
        /// <returns>true if all characters in the word are letters, else false.</returns>
        public static bool WordOnlyContainsLetters(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (!Char.IsLetter(word, i)) return false;
            }
            return true;
        }

        /// <summary>
        /// Gets a single character from user input.
        /// </summary>
        /// <returns>The character entered by the user.</returns>
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
        /// Displays the game UI.
        /// </summary>
        /// <param name="word">The word being guessed.</param>
        /// <param name="knownletters">The array of letter position flags.</param>
        /// <param name="guesses">All of the users previous guesses.</param>
        static void DisplayGame(string word, bool[] knownletters, string guesses)
        {
            Console.Clear();
            Console.WriteLine("Guess The Word!\n");
            Console.WriteLine(FormatWord(word, knownletters));
            Console.WriteLine($"\nYour guesses: {guesses}");
        }

        /// <summary>
        /// Displays the admin menu.
        /// </summary>
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
        /// Displays the main menu.
        /// </summary>
        static void DrawMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to WordGuess!\n");
            Console.WriteLine("1) Play Game");
            Console.WriteLine("2) Admin");
            Console.WriteLine("3) Quit\n");
        }

        /// <summary>
        /// Makes sure the user really wants to reset words.txt, and resets words.txt if the user enters 'y'.
        /// </summary>
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

        /// <summary>
        /// Adds a word to words.txt from user input.
        /// </summary>
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

        /// <summary>
        /// Displays all of the words in words.txt.
        /// </summary>
        static void ListWords()
        {
            string[] words = ReadWords();
            for (int i = 0; i < words.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {words[i]}");
            }
        }

        /// <summary>
        /// Presents all of the words in words.txt to the user.
        /// </summary>
        static void ViewWords()
        {
            Console.Clear();
            Console.WriteLine("All words currently in WordGuess:\n");
            ListWords();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Lists all of the words in words.txt, and then asks the user which word to remove.
        /// The word at the chosen position in the list will be removed from words.txt.
        /// </summary>
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
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Handles the admin menu interaction.
        /// </summary>
        /// <returns>true until the user chooses to return to the main menu.</returns>
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
        /// Handles the main menu interaction.
        /// </summary>
        /// <returns>true until the user chooses to exit the game.</returns>
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