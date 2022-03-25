using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CQGTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string dictionary = "rain spain plain plaint pain main mainly the in on fall falls his was i";
            Console.WriteLine("=======");
            string text = "hte rame in pain fells mainy oon teh lain was hints pliant ai";
            Console.WriteLine("=======");
            string[] dictionaryWords = dictionary.Split(' ');
            string[] textWords = text.Split(' ');
            List<Word> listWords = new List<Word>();
            foreach (var word in textWords)
            {
                listWords.Add(new Word() { SourceWord = word, IsCorrect = false });
            }
            foreach (var word in listWords)
            {
                foreach (var dictionaryWord in dictionaryWords)
                {
                    if (word.SourceWord == dictionaryWord)
                    {
                        word.IsCorrect = true;
                        break;
                    }
                    BuildCorrectWords(word, dictionaryWord);
                }
            }

            Print(listWords, text);

        }

        private static bool HasError(string word, string keyWord)
        {
            int countError = 0;
            for (int i = 0; i < word.Length && countError < 2; i++)
            {
                if (word[i] != keyWord[i])
                {
                    if (i < word.Length - 1)
                    {
                        var firstChar = word[i];
                        var secondChar = word[i + 1];
                        var testWord = new StringBuilder(word);
                        testWord[i] = secondChar;
                        testWord[i + 1] = firstChar;
                        if (testWord.ToString() == keyWord)
                        {
                            return true;
                        }
                    }
                    countError++;
                }
            }
            if (countError < 2)
            {
                return true;
            }
            return false;
        }

        private static bool HasOneExcessLetter(string word, string word2)
        {
            for (int i = 0; i < word.Length; i++)
            {
                string testWord = word.Remove(i, 1);
                if (testWord == word2)
                {
                    return true;
                }
            }
            for (int i = 0; i < word2.Length; i++)
            {
                string testWord = word2.Remove(i, 1);
                if (testWord == word)
                {
                    return true;
                }
            }
            return false;
        }

        public static void BuildCorrectWords(Word word, string dictionaryWord)
        {
            if (word.SourceWord.Length == dictionaryWord.Length && HasError(word.SourceWord, dictionaryWord))
            {
                word.CorrectWords.Add(dictionaryWord);
            }
            if (HasOneExcessLetter(word.SourceWord, dictionaryWord))
            {
                word.CorrectWords.Add(dictionaryWord);
            }
        }

        public static void Print(List<Word> words, string text)
        {
            var sb = new StringBuilder(text);

            foreach (var word in words)
            {
                if (word.IsCorrect)
                {
                    continue;
                }

                var textWord = string.Empty;
                string pattern = @$"\b{word.SourceWord}\b";

                if (word.CorrectWords.Count == 0)
                {
                    textWord = $"{{{word.SourceWord}?}}";

                    sb = new StringBuilder(Regex.Replace(sb.ToString(), pattern, textWord));

                    continue;
                }
                if (word.CorrectWords.Count == 1)
                {
                    sb = new StringBuilder(Regex.Replace(sb.ToString(), pattern, word.CorrectWords.First()));
                    continue;
                }
                textWord = String.Join(' ', word.CorrectWords);
                textWord = $"{{{textWord}}}";

                sb = new StringBuilder(Regex.Replace(sb.ToString(), pattern, textWord));

            }

            Console.WriteLine(sb);
        }
    }
}
