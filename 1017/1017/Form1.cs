using System;
using System.Drawing;
using System.Windows.Forms;

namespace _1017
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void ChangeSign(int a) {
            switch(a)
            {
                case 1: pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory + "/../../Images/red.ico"); break;
                case 2: pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory + "/../../Images/yellow.ico"); break;
                case 3: pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory + "/../../Images/blue.ico"); break;
                default: return;
            }
        }  

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeSign(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeSign(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeSign(3);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChangeSign(3);
        }
    }
}