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
        const uint StandardClientStringLength = 75;

        public TextInserter(string PathToFile, uint StringLength)
        {
            if (!File.Exists(PathToFile)) { throw new FileNotFoundException("Can't find file"); }
            else 
            {
                Encoding UTF8_Encoding = Encoding.UTF8;
                
                if (new FileInfo(PathToFile).Length == 0)
                { throw new TextInsertionException("Unable to copy text from an empty file"); }

                else if (UTF8_Encoding != GetEncoding(PathToFile))
                { throw new EncoderFallbackException("Unable to copy text from a file with non-UTF8 encoding"); }
                
                else if (StringLength == 0)
                { throw new TextInsertionException("Can't insert text with zero-StringLength"); }
                
                ParsedText = ParseTextFromFile(PathToFile);
            }
        }

        public TextInserter(string PathToFile) : this(PathToFile, StandardClientStringLength)
        { }
        
        /*
        public string ConvertStringArrayToString ()
        {
            return string.Join("\n", ParsedText);
        }
        */

        private string[] ParseTextFromFile (string PathToFile)
        {
            return System.IO.File.ReadAllLines(PathToFile);
        }

        public string InsertNextLine(uint StringLength) // Getting the length from user
        {
            throw new NotImplementedException();
        }

        public string InsertNextLine() // Standard Client String Length by default
        {
            throw new NotImplementedException();
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
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            return Encoding.UTF8;
        }
    }
}
