using System;
using Xunit;
using System.IO;

namespace GuessingGameTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanReadLines()
        {
            string[] words = new string[] { "Pink", "Flamingo" };
            File.WriteAllLines(GuessingGame.Program.filepath, words);
            string[] filewords = GuessingGame.Program.ReadWords();
            File.Delete(GuessingGame.Program.filepath);
            Assert.Equal("Pink", filewords[0]);
        }

        [Fact]
        public void ReadWordsCreatesDefaultFileIfNoneExists()
        {
            if (File.Exists(GuessingGame.Program.filepath)) File.Delete(GuessingGame.Program.filepath);
            string[] filewords = GuessingGame.Program.ReadWords();
            Assert.NotEmpty(filewords);

        }
    }
}
