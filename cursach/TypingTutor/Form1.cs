using Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Text;

namespace TypingTutor
{
    public partial class Form1 : Form
    {
        const int buttonWidth = 55, buttonHeight = 45;
        const int keyboardLayoutXPos = 51, keyboardLayoutYPos = 229;
        
        private string CurrentInputString;
        private string CurrentReferenceString;
        private uint NumberOfEnteredWords;
        private int NumberOfWrongEnteredChars;
        private bool CorrectCharFlag;
        private TextInserter textInserter;
        private TextValidator textValidator;
        private CharacterAccuracyStats CAS;
        static TypingSpeedStats TSS = new TypingSpeedStats();

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.MaximumSize = new Size(1920, 1080);
            this.MinimumSize = new Size(880, 495);
            this.ClientSize = new Size(944, 531);
            CAS = new CharacterAccuracyStats();
            NumberOfEnteredWords = 0;
            NumberOfWrongEnteredChars = 0;
            CorrectCharFlag = true;

            // used own txt file
            textInserter = new TextInserter("..\\..\\..\\..\\texts\\txttxt.txt", 1);
            CurrentReferenceString = textInserter.InsertNextLine();
            shownTextArea.Text = textInserter.GetCurrentLine();
            
            textValidator = new TextValidator(shownTextArea.Text);
            textInputArea.MaxLength = shownTextArea.Text.Length;
        }
        private Point DrawKeysRow(Button[] buttons, int row, Point pos)
        {
            int col;
            System.Drawing.Font font1 = new System.Drawing.Font("Consolas", 9f);
            for (col = 0; col < buttons.Length; col++)
            {
                buttons[col].Font = font1;
                buttons[col].BackColor = System.Drawing.SystemColors.ControlLight;
                buttons[col].Size = new System.Drawing.Size(buttonWidth, buttonHeight);
                buttons[col].Location = new Point(pos.X + (col * buttonWidth), pos.Y);
            }
            Point nextKeyPos = new Point(buttons[col - 1].Location.X + buttonWidth, buttons[col - 1].Location.Y);
            return nextKeyPos;
        }
        private Point DrawKey(Button button, int extendSizeX, Point pos)
        {
            System.Drawing.Font font1 = new System.Drawing.Font("Consolas", 9f);
            button.Font = font1;
            button.BackColor = System.Drawing.SystemColors.ControlLight;
            button.Size = new System.Drawing.Size(buttonWidth + extendSizeX, buttonHeight);
            button.Location = pos;
            Point nextKeyPos = new Point(button.Location.X + button.Size.Width, button.Location.Y);
            return nextKeyPos;
        }
        private void DrawKeyBoardLayout()
        {
            Button[] firstRow = new Button[] { btnTilda, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn0, btnMinus, btnPlus }; // + btnBackspace
            Button[] secondRow = new Button[] { btnQ, btnW, btnE, btnR, btnT, btnY, btnU, btnI, btnO, btnP, btnLBracket, btnRBracket }; // + btnTab (start) + btnBackSlash (end)
            Button[] thirdRow = new Button[] { btnA, btnS, btnD, btnF, btnG, btnH, btnJ, btnK, btnL, btnColon, btnDoubleQuotes }; // + btnCapsLock (start) + btnEnter (end)
            Button[] fourthRow = new Button[] { btnZ, btnX, btnC, btnV, btnB, btnN, btnM, btnLArrow, btnRArrow, btnQMark }; // + btnLShift (start) + btnRShift (end)

            Point endPos;
            Point startPos = new Point(keyboardLayoutXPos, keyboardLayoutYPos);
            int extendSize;

            // row 0
            endPos = DrawKeysRow(firstRow, 0, startPos);
            extendSize = 55;
            DrawKey(btnBackspace, extendSize, endPos);

            // row 1
            extendSize = 28;
            startPos.Y += buttonHeight;
            endPos = DrawKey(btnTab, extendSize, startPos);
            endPos = DrawKeysRow(secondRow, 1, endPos);
            extendSize = 27;
            DrawKey(btnBackSlash, extendSize, endPos);

            // row 2
            extendSize = 40;
            startPos.Y += buttonHeight;
            endPos = DrawKey(btnCapsLock, extendSize, startPos);
            endPos = DrawKeysRow(thirdRow, 2, endPos);
            extendSize = buttonWidth + 15;
            DrawKey(btnEnter, extendSize, endPos);

            // row 3
            extendSize = 15 + buttonWidth;
            startPos.Y += buttonHeight;
            endPos = DrawKey(btnLShift, extendSize, startPos);
            endPos = DrawKeysRow(fourthRow, 3, endPos);
            extendSize = 40 + buttonWidth;
            DrawKey(btnRShift, extendSize, endPos);

            // row 4
            extendSize = 28;
            startPos.Y += buttonHeight;
            endPos = DrawKey(btnLCtrl, extendSize, startPos);
            endPos = DrawKey(btnLAlt, extendSize, endPos);
            extendSize = 53 + 7 * buttonWidth;
            endPos = DrawKey(btnSpace, extendSize, endPos);
            extendSize = 28;
            endPos = DrawKey(btnRAlt, extendSize, endPos);
            DrawKey(btnRCtrl, extendSize, endPos);
        }

        private void DrawSplitter(PaintEventArgs e)
        {
            Pen lineColor = new Pen(Color.FromArgb(30, 60, 90), 4);
            int pointStartX = (this.ClientSize.Width - 825) / 2;
            int pointEndX = this.ClientSize.Width - pointStartX;
            int pointY = (this.ClientSize.Height - 125) / 2;
            Point pointStart = new Point(pointStartX, pointY);
            Point pointEnd = new Point(pointEndX, pointY);
            e.Graphics.DrawLine(lineColor, pointStart, pointEnd);
        }

        private void DrawTextInputArea()
        {
            int pointStartX = (this.ClientSize.Width - 825) / 2;
            int pointEndX = this.ClientSize.Width - pointStartX * 2;
            int pointY = (this.ClientSize.Height - 190) / 2;
            Point pointStart = new Point(pointStartX, pointY);
            textInputArea.Location = pointStart;
            textInputArea.Size = new Size(pointEndX, 1);
        }

        private void DrawShownTextArea()
        {
            int pointStartX = (this.ClientSize.Width - 825) / 2;
            int pointY = (this.ClientSize.Height - 115) / 2;
            Point pointStart = new Point(pointStartX, pointY);
            shownTextArea.Location = pointStart;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            DrawKeyBoardLayout();
            DrawTextInputArea();
            DrawShownTextArea();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawSplitter(e);
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // redrawing objects each time when window size changes
            this.Invalidate();
        }

        private void ButtonReady_Click(object sender, EventArgs e)
        {
            ButtonReady.Enabled = false;
            ButtonReady.Visible = false;
            textInputArea.Enabled = true;
            textInputArea.Focus();
            //! a crutch to fix the first symbol to highlight on keyboard
            CurrentInputString = string.Empty;
            KeyHighlight();
            //!
            TSS.StartMonitoringSpeed();
        }

        private void ButtonBackColorPaintBack()
        {
            btnQ.BackColor = System.Drawing.SystemColors.ControlLight;
            btnW.BackColor = System.Drawing.SystemColors.ControlLight;
            btnE.BackColor = System.Drawing.SystemColors.ControlLight;
            btnR.BackColor = System.Drawing.SystemColors.ControlLight;
            btnT.BackColor = System.Drawing.SystemColors.ControlLight;
            btnY.BackColor = System.Drawing.SystemColors.ControlLight;
            btnU.BackColor = System.Drawing.SystemColors.ControlLight;
            btnI.BackColor = System.Drawing.SystemColors.ControlLight;
            btnO.BackColor = System.Drawing.SystemColors.ControlLight;
            btnP.BackColor = System.Drawing.SystemColors.ControlLight;
            btnA.BackColor = System.Drawing.SystemColors.ControlLight;
            btnS.BackColor = System.Drawing.SystemColors.ControlLight;
            btnD.BackColor = System.Drawing.SystemColors.ControlLight;
            btnF.BackColor = System.Drawing.SystemColors.ControlLight;
            btnG.BackColor = System.Drawing.SystemColors.ControlLight;
            btnH.BackColor = System.Drawing.SystemColors.ControlLight;
            btnJ.BackColor = System.Drawing.SystemColors.ControlLight;
            btnK.BackColor = System.Drawing.SystemColors.ControlLight;
            btnL.BackColor = System.Drawing.SystemColors.ControlLight;
            btnZ.BackColor = System.Drawing.SystemColors.ControlLight;
            btnX.BackColor = System.Drawing.SystemColors.ControlLight;
            btnC.BackColor = System.Drawing.SystemColors.ControlLight;
            btnV.BackColor = System.Drawing.SystemColors.ControlLight;
            btnB.BackColor = System.Drawing.SystemColors.ControlLight;
            btnN.BackColor = System.Drawing.SystemColors.ControlLight;
            btnM.BackColor = System.Drawing.SystemColors.ControlLight;
            btnSpace.BackColor = System.Drawing.SystemColors.ControlLight;
            btnLShift.BackColor = System.Drawing.SystemColors.ControlLight;
            btnRShift.BackColor = System.Drawing.SystemColors.ControlLight;
            btnRArrow.BackColor =  System.Drawing.SystemColors.ControlLight;
            btnTilda.BackColor = System.Drawing.SystemColors.ControlLight;
            btnMinus.BackColor =  System.Drawing.SystemColors.ControlLight;  
            btnPlus.BackColor =  System.Drawing.SystemColors.ControlLight;  
            btnLBracket.BackColor = System.Drawing.SystemColors.ControlLight;  
            btnRBracket.BackColor = System.Drawing.SystemColors.ControlLight;  
            btnColon.BackColor = System.Drawing.SystemColors.ControlLight;  
            btnDoubleQuotes.BackColor = System.Drawing.SystemColors.ControlLight;  
            btnBackSlash.BackColor = System.Drawing.SystemColors.ControlLight;  
            btnLArrow.BackColor = System.Drawing.SystemColors.ControlLight;  
            btnRArrow.BackColor = System.Drawing.SystemColors.ControlLight;  
            btnQMark.BackColor = System.Drawing.SystemColors.ControlLight;
            btn0.BackColor = System.Drawing.SystemColors.ControlLight;
            btn1.BackColor = System.Drawing.SystemColors.ControlLight;
            btn2.BackColor = System.Drawing.SystemColors.ControlLight;
            btn3.BackColor = System.Drawing.SystemColors.ControlLight;
            btn4.BackColor = System.Drawing.SystemColors.ControlLight;
            btn5.BackColor = System.Drawing.SystemColors.ControlLight;
            btn6.BackColor = System.Drawing.SystemColors.ControlLight;
            btn7.BackColor = System.Drawing.SystemColors.ControlLight;
            btn8.BackColor = System.Drawing.SystemColors.ControlLight;
            btn9.BackColor = System.Drawing.SystemColors.ControlLight;
        }

        private void KeyHighlight()
        {
            List<char> charForRshift = new List<char> { 'q', 'w', 'r', 't', 'a', 's', 'd', 'f', 'g', 'z', 'x', 'c', 'v', 'b' };
            ButtonBackColorPaintBack();

            bool SubstringPassed;
            SubstringPassed = textValidator.ValidateSubString(CurrentInputString, 0);

            if (SubstringPassed)
            {
                btnBackspace.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            else
            {
                btnBackspace.BackColor = Color.Yellow;
                return;
            }

            char KeyToHighlight;

            if (CurrentInputString.Length < textInputArea.MaxLength ) { KeyToHighlight = CurrentReferenceString[CurrentInputString.Length]; }
            else { return; }
            
            bool IsUpperCase = char.IsUpper(KeyToHighlight);
            bool IsLetter = char.IsLetter(KeyToHighlight);
            bool IsDigit = char.IsDigit(KeyToHighlight);
            KeyToHighlight = char.ToLower(KeyToHighlight);

            if (IsUpperCase)
            {
                if (charForRshift.Contains(KeyToHighlight))
                {
                    btnRShift.BackColor = Color.FromArgb(255, 255, 153);
                }
                else
                {
                    btnLShift.BackColor = Color.FromArgb(255, 255, 153);
                }
                
            }
            if (IsLetter || IsDigit)
            {
                switch (KeyToHighlight)
                {
                    case 'q': btnQ.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'w': btnW.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'e': btnE.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'r': btnR.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 't': btnT.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'y': btnY.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'u': btnU.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'i': btnI.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'o': btnO.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'p': btnP.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'a': btnA.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 's': btnS.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'd': btnD.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'f': btnF.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'g': btnG.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'h': btnH.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'j': btnJ.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'k': btnK.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'l': btnL.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'z': btnZ.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'x': btnX.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'c': btnC.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'v': btnV.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'b': btnB.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'n': btnN.BackColor = Color.FromArgb(255, 255, 153); break;
                    case 'm': btnM.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '0': btn0.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '1': btn1.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '2': btn2.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '3': btn3.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '4': btn4.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '5': btn5.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '6': btn6.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '7': btn7.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '8': btn8.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '9': btn9.BackColor = Color.FromArgb(255, 255, 153); break;
                }
            }
            else
            {
                switch (KeyToHighlight)
                {
                    case ' ': btnSpace.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '`': btnTilda.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '-': btnMinus.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '=': btnPlus.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '[': btnLBracket.BackColor = Color.FromArgb(255, 255, 153); break;
                    case ']': btnRBracket.BackColor = Color.FromArgb(255, 255, 153); break;
                    case ';': btnColon.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '\'': btnDoubleQuotes.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '\\': btnBackSlash.BackColor = Color.FromArgb(255, 255, 153); break;
                    case ',': btnLArrow.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '.': btnRArrow.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '/': btnQMark.BackColor = Color.FromArgb(255, 255, 153); break;
                    case '~': btnRShift.BackColor = Color.FromArgb(255, 255, 153); btnTilda.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '!': btnRShift.BackColor = Color.FromArgb(255, 255, 153); btn1.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '@': btnRShift.BackColor = Color.FromArgb(255, 255, 153); btn2.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '#': btnRShift.BackColor = Color.FromArgb(255, 255, 153); btn3.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '$': btnRShift.BackColor = Color.FromArgb(255, 255, 153); btn4.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '%': btnRShift.BackColor = Color.FromArgb(255, 255, 153); btn5.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '^': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btn6.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '&': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btn7.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '*': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btn8.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '(': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btn9.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case ')': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btn0.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '_': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnMinus.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '+': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnPlus.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '{': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnLBracket.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '}': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnRBracket.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case ':': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnColon.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '"': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnDoubleQuotes.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '|': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnBackSlash.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '<': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnLArrow.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '>': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnRArrow.BackColor = Color.FromArgb(255, 255, 153);   break;
                    case '?': btnLShift.BackColor = Color.FromArgb(255, 255, 153); btnQMark.BackColor = Color.FromArgb(255, 255, 153);   break;
                }
            }
        }

        private void CountWords()
        {
            int index = 0;

            // skip whitespace until first word
            while (index < CurrentInputString.Length && char.IsWhiteSpace(CurrentInputString[index]))
                index++;

            while (index < CurrentInputString.Length)
            {
                // check if current char is part of a word
                while (index < CurrentInputString.Length && !char.IsWhiteSpace(CurrentInputString[index]))
                    index++;

                NumberOfEnteredWords++;

                // skip whitespace until next word
                while (index < CurrentInputString.Length && char.IsWhiteSpace(CurrentInputString[index]))
                    index++;
            }
            TotalWordsPrompt.Text = NumberOfEnteredWords.ToString();
        }

        // the main logic of program is here 
        private void textInputArea_TextChanged(object sender, EventArgs e)
        {
            CurrentInputString = textInputArea.Text;
            bool StringPassed;

            StringPassed = textValidator.ValidateString(CurrentInputString);
            if (StringPassed)
            {
                TSS.StopMonitoringSpeed();

                TSS.CalculateStat(CurrentInputString.Length);
                TTSPrompt.Text = TSS.GetTypingSpeed().ToString() + TSS.StatUnitName;
                CAS.CalculateStat(shownTextArea.Text.Length);
                CASPrompt.Text = CAS.GetUnitAccuracy().ToString() + CAS.StatUnitName;

                CountWords();

                CurrentReferenceString = textInserter.InsertNextLine();
                shownTextArea.Text = textInserter.GetCurrentLine();
                textValidator.SetReferenceString(CurrentReferenceString);
                textInputArea.MaxLength = shownTextArea.Text.Length;
                textInputArea.Text = string.Empty;

                TSS.StartMonitoringSpeed();
                return;
            }

            KeyHighlight();

            bool SubstringPassed;

            if (CurrentInputString.Length > 0)
            {
                SubstringPassed = textValidator.ValidateSubString(CurrentInputString, 0);    // current substring starts from 0 index
                if (SubstringPassed)
                {
                    textInputArea.BackColor = Color.Linen;
                    CorrectCharFlag = true;
                }
                else 
                {
                    //! preventing incorrect summing of wrong entered chars when the user
                    // types more after the substring being already invalidated
                    if (CorrectCharFlag)
                    {
                        CAS.SetWrongTypedUnits(++NumberOfWrongEnteredChars);
                    }
                    textInputArea.BackColor = Color.Yellow;
                    CorrectCharFlag = false;
                    //!
                }
            } 
        }
    }
}
