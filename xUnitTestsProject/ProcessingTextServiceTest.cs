using CQGTest;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTestsProject
{
    public class ProcessingTextServiceTest
    {
        [Fact]
        public void TextConvertorTest()
        {
            List<string> firstList = new List<string>()
            {
                "What",
                "is",
                "your",
                "name?"
            };
            string firstString = " What is your name?";

            List<string> secondList = new List<string>()
            {
                "How",
                "are",
                "you?"
            };
            string secondString = " How are you?";

            ProcessingTextService service = new ProcessingTextService();
            string firtsResult = service.TextConverter(firstList);
            string secondResult = service.TextConverter(secondList);

            Assert.Equal(firtsResult, firstString);
            Assert.Equal(secondResult, secondString);
        }

        [Theory]
        [InlineData("rain", "lrain")]
        [InlineData("you", "yoru")]
        [InlineData("blank", "blnk")]
        [InlineData("tropic", "troppic")]
        [InlineData("tropic", "tropi")]
        public void HasOneExcessLetterText(string firstWord, string secondWord)
        {
            ProcessingTextService services = new ProcessingTextService();

            Assert.True(services.HasOneExcessLetter(firstWord, secondWord));
        }

        [Theory]
        [InlineData("rain", "arin")]
        [InlineData("you", "yoe")]
        [InlineData("blank", "blonk")]
        [InlineData("tropic", "torpic")]
        public void HasErroTest(string firstWord, string secondWord)
        {
            ProcessingTextService services = new ProcessingTextService();

            Assert.True(services.HasError(firstWord, secondWord));
        }

        [Fact]
        public void HasCorrectTest()
        {
            Word word = new Word()
            {
                SourceWord = "lain",
                IsCorrect = true,
                CorrectWords = new List<string>()
                {
                    "rain",
                    "plain",
                    "pain",
                    "main"
                }
            };
            Word wordCorrect = new Word()
            {
                SourceWord = "lain",
                IsCorrect = true,
                CorrectWords = new List<string>()
                {
                    "plain"
                }
            };

            ProcessingTextService services = new ProcessingTextService();
            services.HasCorrect(word);
            Assert.Equal(wordCorrect.CorrectWords, word.CorrectWords);
        }

        [Fact]
        public void BuildCorrectWordsTest()
        {
            Word word = new Word()
            {
                SourceWord = "lain",
                IsCorrect = false
            };
            string dictionary = "plain";

            ProcessingTextService services = new ProcessingTextService();

            services.BuildCorrectWords(word, dictionary);

            Assert.True(word.CorrectWords.Contains(dictionary));
        }

        [Fact]
        public void ExecuteTest()
        {
            List<Word> listWord = new List<Word>()
            {
                new Word()
                {
                    SourceWord = "lain",
                    IsCorrect = false
                }
            };
            Word wordCorrect = new Word()
            {
                SourceWord = "lain",
                IsCorrect = true,
                CorrectWords = new List<string>()
                {
                    "plain"
                }
            };

            string[] dictionary = new string[]
            {
                    "rain",
                    "plain",
                    "pain",
                    "main",
                    "kate",
                    "false"
            };

            ProcessingTextService service = new ProcessingTextService();

            service.Execute(dictionary, listWord);

            Assert.Equal(wordCorrect.CorrectWords, listWord.First().CorrectWords);
        }
    }
}
