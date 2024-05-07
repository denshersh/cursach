using System.Globalization;
using System.Text;

namespace Classes
{
    public class TextInserter
    {
        public required string ParsedText;
        public required string Buffer;
        public required string TextToInsert;
        public required string ErrorMsg;
        public uint StringLength;
        private bool InsertingStatus;
        const uint StandardClientStringLength = 75;

        public TextInserter(string PathToFile, uint StringLength)
        {
            if (!File.Exists(PathToFile)) { InsertingStatus = false; throw new FileNotFoundException("Can't find file"); }
            else 
            {
                Encoding encoding = Encoding.UTF8;
                if (new FileInfo(PathToFile).Length == 0)
                {
                    InsertingStatus = false;
                    ErrorMsg = "Unable to copy text from an empty file";
                }
                else if (encoding != GetEncoding(PathToFile))
                {
                    InsertingStatus = false;
                    ErrorMsg = "Unable to copy text from a file with non-UTF8 encoding";
                }
                else if (StringLength == 0)
                {
                    InsertingStatus = false;
                    ErrorMsg = "\\'TextInserter.StringLength = 0\\'\nCan't insert text with zero-StringLength";
                }
                ParsedText = ParseTextFromFile(PathToFile);
            }
        }

        public TextInserter(string PathToFile) : this(PathToFile, StandardClientStringLength)
        { }

        private string ParseTextFromFile (string PathToFile)
        {
            if (InsertingStatus == false)
            {
                return "";
            }
            return File.OpenText(PathToFile).ToString();  // can handle only 1 073 741 823 symbols (INT32_MAX / 2)
        }

        public string InsertLine(uint StringLength) // Getting the length from user
        {

        }

        public string InsertLine() // Standard Client String Length by default
        {

        }
        public static Encoding GetEncoding(string filename)
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
