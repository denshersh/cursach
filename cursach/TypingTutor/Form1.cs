using Classes;
namespace TypingTutor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Class1 myClass = new Class1();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 a = new Class1();
            a.a = 1;
            Console.WriteLine($"{a.a}");
        }
    }
}
