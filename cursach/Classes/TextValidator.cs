using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class TextValidator
    {
        private string ReferenceString;

        public TextValidator(string referenceString)
        {
            ReferenceString = referenceString;
        }

        public void SetReferenceString(string referenceString) { ReferenceString = referenceString; }

        public bool ValidateChar(char inputChar, int charPosInString)
        {
            if (inputChar == ReferenceString[charPosInString])
            { return  true; }
            return false;
        }

        public bool ValidateSubString(string subString, int subStringPos)
        {
            if (ReferenceString.Substring(subStringPos, subString.Length) == subString )
            { return true; }
            return false;
        }

        public bool ValidateString(string inputString)
        {
            if (inputString == ReferenceString)
            { return true; }
            return false;
        }
    }
}
