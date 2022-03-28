using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CQGTest
{
    public class Services
    {
        public void Execute()
        {
            ConnectionServices connectionServices = new ConnectionServices();
            string path = connectionServices.GetFilePath();
            string blank;
            List<string> dictionatyLines = new List<string>();
            List<string> textLines = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((blank = sr.ReadLine()) != "===")
                    {
                        dictionatyLines.Add(blank);
                    }
                    while ((blank = sr.ReadLine()) != "===")
                    {
                        textLines.Add(blank);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            string[] dictionaryWords = TextConverter(dictionatyLines).Split(' ');
            string[] textWords = TextConverter(textLines).Split(' ');
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

            string text = String.Join("\n", textLines);

            StringBuilder sb = connectionServices.Print(listWords, text);
            path = connectionServices.GetDirectoryPath();
            connectionServices.RecordTxtFile(sb, $@"{path}\result.txt");
        }

        public void HasCorrect(Word word)
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

        public bool HasError(string word, string keyWord)
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

        public bool HasOneExcessLetter(string firstWord, string secondWord)
        {
            for (int i = 0; i < firstWord.Length; i++)
            {
                string testWord = firstWord.Remove(i, 1);
                if (testWord == secondWord)
                {
                    return true;
                }
            }
            for (int i = 0; i < secondWord.Length; i++)
            {
                string testWord = secondWord.Remove(i, 1);
                if (testWord == firstWord)
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
