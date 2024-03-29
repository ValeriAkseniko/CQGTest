﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CQGTest
{
    public class TxtService
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

        public StringBuilder GetText(List<Word> words, List<string> textLines)
        {
            string text = String.Join("\n", textLines);
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
