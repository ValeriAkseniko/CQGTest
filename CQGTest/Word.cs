using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQGTest
{
    public class Word
    {
        public string SourceWord { get; set; }

        public IList<string> CorrectWords { get; set; }

        public bool IsCorrect { get; set; }

        public Word()
        {
            CorrectWords = new List<string>();
        }
    }
}
