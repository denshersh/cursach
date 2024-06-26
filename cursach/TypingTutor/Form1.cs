using Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace TypingTutor
{
    public partial class Form1 : Form
    {
        const int buttonWidth = 55, buttonHeight = 45;
        const int keyboardLayoutXPos = 51, keyboardLayoutYPos = 229;
        //
        private uint NumberOfEnteredWords;
        private int NumberOfCorrectEnteredChars;
        private int NumberOfCharsInCurrString;
        private int NumberOfWrongEnteredChars;
        private bool CorrectCharFlag;
        private TextInserter textInserter;
        private TextValidator textValidator;
        private CharacterAccuracyStats CAS;
        private WordAccuracyStats WAS;
        static TypingSpeedStats TSS = new TypingSpeedStats();
        //
        [DllImport("user32.dll")]
        public static extern short GetKeyState(Keys key);
        public Form1()
        {
            InitializeComponent();
            CAS = new CharacterAccuracyStats();
            WAS = new WordAccuracyStats();
            NumberOfEnteredWords = 0;
            NumberOfCorrectEnteredChars = 0;
            NumberOfWrongEnteredChars = 0;

            textInserter = new TextInserter("..\\..\\..\\..\\texts\\txttxt.txt");
            shownTextArea.Text = textInserter.InsertNextLine();
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
            textInputArea.Size = new Size(pointEndX, 1);    // height is automatically set
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
            this.Invalidate();
        }

        private void ButtonReady_Click(object sender, EventArgs e)
        {
            ButtonReady.Enabled = false;
            ButtonReady.Visible = false;
            textInputArea.Enabled = true;
            textInputArea.Focus();
            TSS.StartMonitoringSpeed();
        }

        // Unnecessary (maybe)
        private void textInputArea_Enter(object sender, EventArgs e)
        {

        }

        private void textInputArea_TextChanged(object sender, EventArgs e)
        {
            string CurrentString = textInputArea.Text;
            bool StringPassed;

            StringPassed = textValidator.ValidateString(CurrentString);
            if (StringPassed)
            {
                TSS.StopMonitoringSpeed();
                TSS.CalculateStat(CurrentString.Length);
                TTSPrompt.Text = TSS.GetTypingSpeed().ToString();
                CAS.CalculateStat(NumberOfCorrectEnteredChars);
                CASPrompt.Text = CAS.GetUnitAccuracy().ToString();
                int index = 0;

                // refactor
                // skip whitespace until first word
                while (index < CurrentString.Length && char.IsWhiteSpace(CurrentString[index]))
                    index++;

                while (index < CurrentString.Length)
                {
                    // check if current char is part of a word
                    while (index < CurrentString.Length && !char.IsWhiteSpace(CurrentString[index]))
                        index++;

                    NumberOfEnteredWords++;

                    // skip whitespace until next word
                    while (index < CurrentString.Length && char.IsWhiteSpace(CurrentString[index]))
                        index++;
                }
                TotalWordsPrompt.Text = NumberOfEnteredWords.ToString();
                // refactor
                textInputArea.Text = string.Empty;
                shownTextArea.Text = textInserter.InsertNextLine();
                textValidator.SetReferenceString(shownTextArea.Text);
                textInputArea.MaxLength = shownTextArea.Text.Length;
                TSS.StartMonitoringSpeed();
                return;
            }

            bool SubstringPassed;

            if (CurrentString.Length > 0)
            {
                SubstringPassed = textValidator.ValidateSubString(CurrentString, 0);    // current substring starts from 0 index
                if (SubstringPassed)
                {
                    NumberOfCorrectEnteredChars++;
                    textInputArea.BackColor = Color.Linen;
                    CorrectCharFlag = true;
                }
                else 
                {
                    CAS.SetWrongTypedUnits(++NumberOfWrongEnteredChars);
                    if (CorrectCharFlag)
                    {
                        CAS.SetWrongTypedUnits(++NumberOfWrongEnteredChars);
                    }
                    textInputArea.BackColor = Color.Yellow;
                    CorrectCharFlag = false;
                }
            } 
        }

        private void textInputArea_KeyDown(object sender, KeyEventArgs e)
        {
            // Check key state for Ctrl, Shift and Alt
            bool leftCtrlPressed = (GetKeyState(Keys.LControlKey) & 0x8000) != 0;
            bool rightCtrlPressed = (GetKeyState(Keys.RControlKey) & 0x8000) != 0;
            bool leftShiftPressed = (GetKeyState(Keys.LShiftKey) & 0x8000) != 0;
            bool rightShiftPressed = (GetKeyState(Keys.RShiftKey) & 0x8000) != 0;
            bool leftAltPressed = (GetKeyState(Keys.LMenu) & 0x8000) != 0; // LMenu is left Alt
            bool rightAltPressed = (GetKeyState(Keys.RMenu) & 0x8000) != 0; // RMenu is right Alt

            if (leftCtrlPressed)
            {
                btnLCtrl.BackColor = Color.FromArgb(209, 207, 207); // Color for left Ctrl
                return;
            }
            if (rightCtrlPressed)
            {
                btnRCtrl.BackColor = Color.FromArgb(209, 207, 207); // Color for right Ctrl
                return;
            }
            if (leftShiftPressed)
            {
                btnLShift.BackColor = Color.FromArgb(209, 207, 207); // Color for left Shift
                return;
            }
            if (rightShiftPressed)
            {
                btnRShift.BackColor = Color.FromArgb(209, 207, 207); // Color for right Shift
                return;
            }
            if (leftAltPressed)
            {
                btnLAlt.BackColor = Color.FromArgb(209, 207, 207); // Color for left Alt
                return;
            }
            if (rightAltPressed)
            {
                btnRAlt.BackColor = Color.FromArgb(209, 207, 207); // Color for right Alt
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Q: btnQ.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.W: btnW.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.E: btnE.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.R: btnR.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.T: btnT.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.Y: btnY.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.U: btnU.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.I: btnI.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.O: btnO.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.P: btnP.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.A: btnA.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.S: btnS.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D: btnD.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.F: btnF.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.G: btnG.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.H: btnH.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.J: btnJ.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.K: btnK.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.L: btnL.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.Z: btnZ.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.X: btnX.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.C: btnC.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.V: btnV.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.B: btnB.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.N: btnN.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.M: btnM.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D0: btn0.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D1: btn1.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D2: btn2.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D3: btn3.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D4: btn4.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D5: btn5.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D6: btn6.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D7: btn7.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D8: btn8.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.D9: btn9.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.Space: btnSpace.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.CapsLock:
                    {
                        if (Control.IsKeyLocked(Keys.CapsLock))
                            btnCapsLock.BackColor = Color.FromArgb(209, 207, 207);
                        else
                            btnCapsLock.BackColor = System.Drawing.SystemColors.ControlLight; break;
                    }
                case Keys.Back: btnBackspace.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.Tab: btnTab.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.Enter: btnEnter.BackColor = Color.FromArgb(209, 207, 207); break;
                case Keys.OemMinus: btnMinus.BackColor = Color.FromArgb(209, 207, 207); break; // minus
                case Keys.Oemplus: btnPlus.BackColor = Color.FromArgb(209, 207, 207); break; // plus
                case Keys.OemOpenBrackets: btnLBracket.BackColor = Color.FromArgb(209, 207, 207); break; // left square bracket
                case Keys.OemCloseBrackets: btnRBracket.BackColor = Color.FromArgb(209, 207, 207); break; // right square bracket
                case Keys.OemPipe: btnBackSlash.BackColor = Color.FromArgb(209, 207, 207); break; // back slash
                case Keys.OemSemicolon: btnColon.BackColor = Color.FromArgb(209, 207, 207); break; // colon
                case Keys.OemQuotes: btnDoubleQuotes.BackColor = Color.FromArgb(209, 207, 207); break; // double qoutes
                case Keys.Oemtilde: btnTilda.BackColor = Color.FromArgb(209, 207, 207); break; // tilda
                case Keys.Oemcomma: btnLArrow.BackColor = Color.FromArgb(209, 207, 207); break; // comma (left arrow)
                case Keys.OemPeriod: btnRArrow.BackColor = Color.FromArgb(209, 207, 207); break; // period (right arrow)
                case Keys.OemQuestion: btnQMark.BackColor = Color.FromArgb(209, 207, 207); break; // question mark
            }
        }

        private void textInputArea_KeyUp(object sender, KeyEventArgs e)
        {
            // Check key state for Ctrl, Shift and Alt
            bool leftCtrlPressed = (GetKeyState(Keys.LControlKey) & 0x8000) == 0;
            bool rightCtrlPressed = (GetKeyState(Keys.RControlKey) & 0x8000) == 0;
            bool leftShiftPressed = (GetKeyState(Keys.LShiftKey) & 0x8000) == 0;
            bool rightShiftPressed = (GetKeyState(Keys.RShiftKey) & 0x8000) == 0;
            bool leftAltPressed = (GetKeyState(Keys.LMenu) & 0x8000) == 0; // LMenu is left Alt
            bool rightAltPressed = (GetKeyState(Keys.RMenu) & 0x8000) == 0; // RMenu is right Alt

            if (leftCtrlPressed)
                btnLCtrl.BackColor = System.Drawing.SystemColors.ControlLight; // Color for left Ctrl
            if (rightCtrlPressed)
                btnRCtrl.BackColor = System.Drawing.SystemColors.ControlLight; // Color for right Ctrl
            if (leftShiftPressed)
                btnLShift.BackColor = System.Drawing.SystemColors.ControlLight; // Color for left Shift
            if (rightShiftPressed)
                btnRShift.BackColor = System.Drawing.SystemColors.ControlLight; // Color for right Shift
            if (leftAltPressed)
                btnLAlt.BackColor = System.Drawing.SystemColors.ControlLight; // Color for left Alt
            if (rightAltPressed)
                btnRAlt.BackColor = System.Drawing.SystemColors.ControlLight; // Color for right Alt

            switch (e.KeyCode)
            {
                case Keys.Q: btnQ.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.W: btnW.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.E: btnE.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.R: btnR.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.T: btnT.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.Y: btnY.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.U: btnU.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.I: btnI.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.O: btnO.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.P: btnP.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.A: btnA.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.S: btnS.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D: btnD.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.F: btnF.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.G: btnG.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.H: btnH.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.J: btnJ.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.K: btnK.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.L: btnL.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.Z: btnZ.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.X: btnX.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.C: btnC.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.V: btnV.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.B: btnB.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.N: btnN.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.M: btnM.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D0: btn0.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D1: btn1.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D2: btn2.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D3: btn3.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D4: btn4.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D5: btn5.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D6: btn6.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D7: btn7.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D8: btn8.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.D9: btn9.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.Space: btnSpace.BackColor = System.Drawing.SystemColors.ControlLight; break;
                //case Keys.CapsLock: btnCapsLock.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.Back: btnBackspace.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.Tab: btnTab.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.Enter: btnEnter.BackColor = System.Drawing.SystemColors.ControlLight; break;
                case Keys.OemMinus: btnMinus.BackColor = System.Drawing.SystemColors.ControlLight; break; // minus
                case Keys.Oemplus: btnPlus.BackColor = System.Drawing.SystemColors.ControlLight; break; // plus
                case Keys.OemOpenBrackets: btnLBracket.BackColor = System.Drawing.SystemColors.ControlLight; break; // left square bracket
                case Keys.OemCloseBrackets: btnRBracket.BackColor = System.Drawing.SystemColors.ControlLight; break; // right square bracket
                case Keys.OemPipe: btnBackSlash.BackColor = System.Drawing.SystemColors.ControlLight; break; // back slash
                case Keys.OemSemicolon: btnColon.BackColor = System.Drawing.SystemColors.ControlLight; break; // colon
                case Keys.OemQuotes: btnDoubleQuotes.BackColor = System.Drawing.SystemColors.ControlLight; break; // double quotes
                case Keys.Oemtilde: btnTilda.BackColor = System.Drawing.SystemColors.ControlLight; break; // tilda
                case Keys.Oemcomma: btnLArrow.BackColor = System.Drawing.SystemColors.ControlLight; break; // comma (left arrow)
                case Keys.OemPeriod: btnRArrow.BackColor = System.Drawing.SystemColors.ControlLight; break; // period (right arrow)
                case Keys.OemQuestion: btnQMark.BackColor = System.Drawing.SystemColors.ControlLight; break; // question mark
            }
        }
    }
}
