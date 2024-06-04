using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class TextInsertionException : Exception
    {
        public TextInsertionException() { }

        public TextInsertionException(string message) : base(message) { }

        public TextInsertionException(string message, Exception inner) : base(message, inner) { }
    }
}
