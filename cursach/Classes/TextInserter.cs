using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Classes
{
    public class TextInserter
    {
        public string[] ParsedText { get; set; }
        public string Buffer;
        public string TextToInsert;
        public string ErrorMsg;
        public uint StringLength;
        private bool InsertingStatus;
        const uint StandardClientStringLength = 75;

        public TextInserter(string PathToFile, uint StringLength)
        {
            ErrorMsg = "";
            if (!File.Exists(PathToFile)) { InsertingStatus = false; throw new FileNotFoundException("Can't find file"); }
            else 
            {
                Encoding UTF8_Encoding = Encoding.UTF8;
                if (new FileInfo(PathToFile).Length == 0)
                {
                    InsertingStatus = false;
                    ErrorMsg = "Unable to copy text from an empty file";
                }
                else if (UTF8_Encoding != GetEncoding(PathToFile))
                {
                    InsertingStatus = false; throw new EncoderFallbackException("Unable to copy text from a file with non-UTF8 encoding");
                }
                else if (StringLength == 0)
                {
                    InsertingStatus = false;
                    ErrorMsg = "\\'TextInserter.StringLength = 0\\'\nCan't insert text with zero-StringLength";
                }
                InsertingStatus = true;
                ParsedText = ParseTextFromFile(PathToFile);
            }
        }

        public TextInserter(string PathToFile) : this(PathToFile, StandardClientStringLength)
        { }
        
        /*  Crutch which works
        public string ConvertStringArrayToString ()
        {
            return string.Join("\n", ParsedText);
        }
        */

        private string[] ParseTextFromFile (string PathToFile)
        {
            if (InsertingStatus == false)
            {
                return null;
            }

            return System.IO.File.ReadAllLines(PathToFile);
        }

        public string InsertLine(uint StringLength) // Getting the length from user
        {
            return "";
        }

        public string InsertLine() // Standard Client String Length by default
        {
            return "";
        }
        private static Encoding GetEncoding(string filename)
        {
            // Read the BOM (byte order mark)
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.ASCII;
        }
    }
}
