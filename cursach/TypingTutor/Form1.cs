using Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
namespace TypingTutor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            Button[] numbersButtons  = new Button[] { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };
            Button[] lettersButtons  = new Button[] { btnQ, btnW, btnE, btnR, btnT, btnY, btnU, btnI, btnO, btnP,
                    btnA, btnS, btnD, btnF, btnG, btnH, btnJ, btnK, btnL, btnZ, btnX, btnC, btnV, btnB, btnN, btnM};
            Button btnSpace, btnAlt, btnCtrl, btnLShift, btnRShift, btnCapsLock, btnBackspace, btnTab;

            foreach (var b in numberButtons)
                b.Click += new System.EventHandler(this.Number_Click);

            foreach (var b in operationButtons)
                b.Click += new System.EventHandler(this.Operation_Click);

            // etc

            Button[][] allButtons =
            {
                new Button[] {btnSqrt, btnExp, btn10x, btnPow,btnMultInverse, btnCHS, null, null, null, null},
                new Button[] {btnN, btnInterest, btnPMT, btnPV, btnFV, null, btn7, btn8, btn9, btnDiv},
                new Button[] {btnLn, btnLog, btnSine, btnCosine, btnTangent, btnPi, btn4, btn5, btn6, btnMult},
                new Button[] {btnRoll, btnSwap, btnCLRfin, btnCLX, btnCLR, btnEnter, btn1, btn2, btn3, btnSubtract},
                new Button[] {btnInt, btnFrac, btnFix, btnStore, btnRecall, null, btn0, btnDecimalPt, btnNotUsed, btnAdd}
            };

            // programmatically set the location
            int col, row;
            Font font1 = new Font("Arial", 14.0f);
            for (row = 0; row < allButtons.Length; row++)
            {
                Button[] ButtonCol = allButtons[row];
                for (col = 0; col < ButtonCol.Length; col++)
                {
                    if (ButtonCol[col] != null)
                    {
                        ButtonCol[col].TabIndex = col + (row * allButtons.Length) + 1;
                        ButtonCol[col].Font = font1;
                        ButtonCol[col].BackColor = System.Drawing.SystemColors.ControlDark;
                        ButtonCol[col].Size = new System.Drawing.Size(stdButtonWidth, stdButtonHeight);
                        ButtonCol[col].Location = new Point(startX + (col * stdButtonWidth),
                                                          startY + (row * stdButtonHeight));
                    }
                }
            }
        }
    }
}
