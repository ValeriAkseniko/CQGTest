using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQGTest
{
    public class Services
    {
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
        }

        public void Execute(List<Word> words)
        {
            ConnectionServices connectionServices = new ConnectionServices();
            string path = connectionServices.GetFilePath();
            string temp;
            List<string> linesDictionary = new List<string>();
            List<string> linesText = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((temp = sr.ReadLine()) != null)
                    {
                        if (temp == "===")
                        {
                            break;
                        }
                        linesDictionary.Add(temp);
                    }
                    while ((temp = sr.ReadLine()) != null)
                    {
                        if (temp == "===")
                        {
                            break;
                        }
                        linesText.Add(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            path = connectionServices.GetDirectoryPath();
            StringBuilder sb = connectionServices.Print(words, temp); //TODO temp
            connectionServices.RecordTxtFile(sb, $@"{path}\result.txt");
        }
    }
}
