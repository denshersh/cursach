using Classes;
namespace TypingTutor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Class1 myClass = new Class1();
            // 1
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 b = new Class1();
            b.a = 1;
            Console.WriteLine($"{b.a}");
            Class1 c = new Class1();
            c.a = 1;
            Console.WriteLine($"{c.a}");
        }
    }
}
