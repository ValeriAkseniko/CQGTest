using System;
using System.Collections.Generic;
using System.Linq;

namespace CQGTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string dictionary = "rain spain plain plaint pain main mainly the in on fall falls his was";
            Console.WriteLine("=======");
            string text = "hte rame in pain fells mainy oon teh lain was hints pliant";
            Console.WriteLine("=======");
            string[] dictionaryArray = dictionary.Split(' ');
            string[] textArray = text.Split(' ');
            List<string> correctText = new List<string>();
            for (int i = 0; i < textArray.Length; i++)
            {
                for (int j = 0; j < dictionaryArray.Length; j++)
                {
                    if (dictionaryArray.Contains(textArray[i]))
                    {
                        correctText.Add(textArray[i]);
                        break;
                    }
                    if (oneIteration(textArray[i], dictionaryArray[j]))
                    {
                        correctText.Add(dictionaryArray[j]);
                        break;
                    }
                    else if (j + 1 == dictionaryArray.Length)
                    {
                        correctText.Add($"{{{textArray[i]}}}?");
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            for (int i = 0; i < correctText.Count; i++)
            {
                Console.WriteLine(correctText[i]);
            }
        }

        public static bool IsAnagram(string word, string keyWord)
        {
            char[] firstArray = ((word.ToLower()).ToCharArray());
            char[] secondArray = ((keyWord.ToLower()).ToCharArray());
            Array.Sort(firstArray);
            Array.Sort(secondArray);
            string firstWord = new string(firstArray);
            string secondWord = new string(secondArray);
            if (firstWord == secondWord)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasOneError(string word, string keyWord)
        {
            int countError = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != keyWord[i])
                {
                    countError++;
                }
            }
            if (countError < 2)
            {
                return true;
            }
            return false;
        }

        public static bool Bolshe(string word, string keyWord)
        {
            if (word.Contains(keyWord) && (word.Length - keyWord.Length) == 1)
            {
                return true;
            }
            if (keyWord.Contains(word) && (keyWord.Length - word.Length) == 1)
            {
                return true;
            }
            return false;
        }

        public static bool oneIteration(string word, string keyWord)
        {
            if (IsAnagram(word, keyWord))
            {
                return true;
            }
            if (word.Length == keyWord.Length && HasOneError(word, keyWord))
            {
                return true;
            }
            if (Bolshe(word,keyWord))
            {
                return true;
            }
            return false;
        }
    }
}
