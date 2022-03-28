using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CQGTest
{
    public class ProcessingTextService
    {
        private readonly TxtService connectionServices;

        public ProcessingTextService()
        {
            connectionServices = new TxtService();
        }
        public void Execute()
        {
            string path = connectionServices.GetFilePath();
            List<string> dictionaryLines = new List<string>();
            List<string> textLines = new List<string>();
            BuildSorceData(path, dictionaryLines, textLines);
            string[] dictionaryWords = TextConverter(dictionaryLines).Split(' ');
            string[] textWords = TextConverter(textLines).Split(' ');
            List<Word> listWords = textWords.Select(x => new Word
            {
                SourceWord = x,
                IsCorrect = false
            })
                .ToList();

            Execute(dictionaryWords, listWords);

            StringBuilder sb = connectionServices.GetText(listWords, textLines);
            path = connectionServices.GetDirectoryPath();
            connectionServices.RecordTxtFile(sb, $@"{path}\result.txt");
        }

        public void Execute(string[] dictionaryWords, List<Word> listWords)
        {
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
        }

        private void BuildSorceData(string path, List<string> dictionaryLines, List<string> textLines)
        {            
            try
            {
                string blank;
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((blank = sr.ReadLine()) != "===")
                    {
                        dictionaryLines.Add(blank);
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
