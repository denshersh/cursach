using System.Globalization;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Classes
{
    public class TextInserter
    {
        private string[] ParsedText;
        private string Buffer;
        private string TextToInsert;
        const int StandardClientStringLength = 75;
        static int CurrentStartPos = 0;

        public TextInserter(string PathToFile, int ShuffleMode)
        {
            if (!File.Exists(PathToFile)) { throw new FileNotFoundException("Can't find file"); }
            else 
            {
                Encoding UTF8_Encoding = Encoding.UTF8;
                
                if (new FileInfo(PathToFile).Length == 0)
                { throw new TextInsertionException("Unable to copy text from an empty file"); }

                else if (UTF8_Encoding != GetEncoding(PathToFile))
                { throw new EncoderFallbackException("Unable to copy text from a file with non-UTF8 encoding"); }
                
                ParseTextFromFile(PathToFile, ShuffleMode);
            }
        }

        public TextInserter(string PathToFile): this(PathToFile, 0) { }

        private void ParseTextFromFile (string PathToFile, int ShuffleMode)
        {
            try { ParsedText = System.IO.File.ReadAllLines(PathToFile); }
            catch (Exception e) {
                Console.WriteLine("{0}: The copy operation could not be performed", e.GetType().Name);
            }
            if (ShuffleMode == 0) { ; }
            else if (ShuffleMode == 1) { ShuffleParsedTextBySentences(); }
            else { throw new ArgumentException("Invalid option for shuffling the text"); }
            Buffer = string.Join(" ", ParsedText);
        }

        private void ShuffleParsedTextBySentences ()
        {
            Random random = new Random();
            for (int i = 0; i < ParsedText.Length - 1; ++i)
            {
                int r = random.Next(i, ParsedText.Length);
                (ParsedText[r], ParsedText[i]) = (ParsedText[i], ParsedText[r]);
            }
        }

        private bool IsValidSeparator(string str, int pos)
        {
            if (pos >= str.Length || pos < 0) return false;

            return str[pos] == ' ' ||
                   str[pos] == ',' ||
                   str[pos] == '.' ||
                   str[pos] == '!' ||
                   str[pos] == '?';
        }

        public string InsertNextLine()
        {
            int CurrentEndPos = CurrentStartPos + StandardClientStringLength - 1;

            if (CurrentEndPos >= Buffer.Length)
            {
                CurrentEndPos = Buffer.Length - 1;
            }

            if (IsValidSeparator(Buffer, CurrentEndPos))
            {
                TextToInsert = Buffer.Substring(CurrentStartPos, CurrentEndPos - CurrentStartPos + 1);
                CurrentStartPos = CurrentEndPos + 1;
            }
            else
            {
                bool separatorFound = false;

                for (int i = CurrentEndPos; i > CurrentStartPos; --i)
                {
                    if (IsValidSeparator(Buffer, i))
                    {
                        TextToInsert = Buffer.Substring(CurrentStartPos, i - CurrentStartPos + 1);
                        CurrentStartPos = i + 1;
                        separatorFound = true;
                        break;
                    }
                }

                if (!separatorFound)
                {
                    TextToInsert = Buffer.Substring(CurrentStartPos, CurrentEndPos - CurrentStartPos + 1);
                    CurrentStartPos = CurrentEndPos + 1;
                }
            }

            return TextToInsert;
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
