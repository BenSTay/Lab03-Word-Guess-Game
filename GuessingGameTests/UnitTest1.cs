using System;
using Xunit;
using System.IO;
using System.Text;

namespace GuessingGameTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanReadLines()
        {
            if (File.Exists(GuessingGame.Program.filepath)) File.Delete(GuessingGame.Program.filepath);
            string[] words = new string[] { "Pink", "Flamingo" };
            File.WriteAllLines(GuessingGame.Program.filepath, words);
            string[] filewords = GuessingGame.Program.ReadWords();
            Assert.Equal("Pink", filewords[0]);
        }

        [Fact]
        public void ReadWordsCreatesDefaultFileIfNoneExists()
        {
            if (File.Exists(GuessingGame.Program.filepath)) File.Delete(GuessingGame.Program.filepath);
            string[] filewords = GuessingGame.Program.ReadWords();
            Assert.NotEmpty(filewords);
        }

        [Fact]
        public void CanWriteWord()
        {
            if (File.Exists(GuessingGame.Program.filepath)) File.Delete(GuessingGame.Program.filepath);
            GuessingGame.Program.WriteWord("contemporaneous");
            string[] filewords = File.ReadAllLines(GuessingGame.Program.filepath);
            Assert.Equal("contemporaneous", filewords[0]);
        }

        [Fact]
        public void CantWriteWordThatAlreadyExists()
        {
            if (File.Exists(GuessingGame.Program.filepath)) File.Delete(GuessingGame.Program.filepath);
            GuessingGame.Program.WriteWord("bacon");
            GuessingGame.Program.WriteWord("bacon");
            string[] filewords = File.ReadAllLines(GuessingGame.Program.filepath);
            Assert.Single(filewords);
        }

        [Fact]
        public void CanGetLetterPositions()
        {
            string word = "bacon";
            char guess = 'c';
            int[] positions = GuessingGame.Program.GetLetterPositions(word, guess);
            Assert.Equal(2, positions[0]);
        }

        [Fact]
        public void GetLetterPositionsReturnsEmptyArrayIfLetterNotFound()
        {
            string word = "bacon";
            char guess = 'z';
            int[] positions = GuessingGame.Program.GetLetterPositions(word, guess);
            Assert.Empty(positions);
        }

        [Fact]
        public void CanUpdateKnownLetters()
        {
            bool[] knownletters = new bool[5];
            int[] positions = new int[] { 1, 2 };
            GuessingGame.Program.UpdateKnownLetters(knownletters, positions);
            Assert.True(knownletters[1] && knownletters[2]);
        }


    }
}
