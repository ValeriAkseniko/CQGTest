using System.Collections.Generic;
using System.Text;

namespace CQGTest
{
    public class Services
    {
        private void HasCorrect(Word word)
        {
            List<string> One = new List<string>();
            List<string> Two = new List<string>();
            foreach (var item in word.CorrectWords)
            {
                if (word.SourceWord.Length == item.Length && HasError(word.SourceWord, item))
                {
                    Two.Add(item);
                }
                if (HasOneExcessLetter(word.SourceWord, item))
                {
                    One.Add(item);
                }
            }

            if (One.Count != 0 && Two.Count != 0)
            {
                foreach (var item in Two)
                {
                    word.CorrectWords.Remove(item);
                }
            }
        }

        private bool HasError(string word, string keyWord)
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

        private bool HasOneExcessLetter(string word, string word2)
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

        public void BuildCorrectWords(Word word, string dictionaryWord)
        {
            if (word.SourceWord.Length == dictionaryWord.Length && HasError(word.SourceWord, dictionaryWord))
            {
                word.CorrectWords.Add(dictionaryWord);
            }
            if (HasOneExcessLetter(word.SourceWord, dictionaryWord))
            {
                word.CorrectWords.Add(dictionaryWord);
            }
            if (word.CorrectWords.Count > 1)
            {
                HasCorrect(word);
            }
        }

        public string TextConverter(List<string> lines)
        {
            string result = string.Empty;
            foreach (var line in lines)
            {
                result = result + " " + line;
            }
            return result;
        }
    }
}
