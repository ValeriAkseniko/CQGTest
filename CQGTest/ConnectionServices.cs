using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CQGTest
{
    public class ConnectionServices
    {
        public string GetFilePath()
        {
            bool isCorrectPath = false;
            Console.WriteLine(Messages.InputPath);
            string path = Console.ReadLine();
            isCorrectPath = File.Exists(path);
            while (!isCorrectPath)
            {
                Console.WriteLine(Messages.InvalidInputPath);
                Console.WriteLine(Messages.InputPath);
                path = Console.ReadLine();
                isCorrectPath = File.Exists(path);
            }
            return path;
        }
        public string GetDirectoryPath()
        {
            Console.WriteLine(Messages.OutputDirectoryPath);
            string path = Console.ReadLine();
            bool isCorrecPath = Directory.Exists(path);
            while (!isCorrecPath)
            {
                Console.WriteLine(Messages.InvalidOutputPath);
                Console.WriteLine(Messages.OutputDirectoryPath);
                path = Console.ReadLine();
                isCorrecPath = Directory.Exists(path);
            }
            return path;
        }
        public void Execute()
        {
            Services service = new Services();
            string path = GetFilePath();
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
            string[] dictionaryWords = service.TextConverter(dictionatyLines).Split(' ');
            string[] textWords = service.TextConverter(textLines).Split(' ');
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
                    service.BuildCorrectWords(word, dictionaryWord);
                }
            }

            string text = String.Join("\n",textLines);

            StringBuilder sb = Print(listWords, text);
            path = GetDirectoryPath();
            RecordTxtFile(sb, $@"{path}\result.txt");
        }
        public void RecordTxtFile(StringBuilder txt, string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(txt);
                }
                Console.WriteLine(Messages.RecordingCompleted);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public StringBuilder Print(List<Word> words, string text)
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

            return sb;
        }
    }
}
