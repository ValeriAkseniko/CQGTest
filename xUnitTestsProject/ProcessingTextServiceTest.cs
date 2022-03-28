using CQGTest;
using System.Collections.Generic;
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
                SourceWord = "rain",
                IsCorrect = true,
                CorrectWords = new List<string>()
                {
                    "arin",
                    "lrain",
                    "ain"
                }
            };
            Word wordNull = null;

            ProcessingTextService services = new ProcessingTextService();
            try
            {
                services.HasCorrect(word);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }
            try
            {
                services.HasCorrect(wordNull);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }
        }

        [Fact]
        public void BuildCorrectWordsTest()
        {
            Word word = new Word()
            {
                SourceWord = "arin",
                IsCorrect = true
            };
            Word wordNull = null;
            string dictionary = "rain";
            string dictionaryNull = null;

            ProcessingTextService services = new ProcessingTextService();

            try
            {
                services.BuildCorrectWords(word, dictionary);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }

            try
            {
                services.BuildCorrectWords(word, dictionaryNull);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }

            try
            {
                services.BuildCorrectWords(wordNull, dictionary);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }
        }
    }
}
