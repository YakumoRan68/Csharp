using System;
using System.Windows.Forms;

namespace _1010
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txt = string.Empty;
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    txt = txt + i + " * " + j + " = " + i * j + "\r\n";
                }
                txt = txt + Environment.NewLine;
            }
            textBox1.Text = txt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = string.Empty;
            int i = 1, j;
            while (i <= 9)
            {
                j = 1;
                while (j <= 9)
                {
                    txt = txt + i + " * " + j + " = " + i * j + "\r\n";
                    j++;
                }
                txt = txt + Environment.NewLine;
                i++;
            }
            textBox2.Text = txt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string txt = string.Empty;
            int i = 1, j;
            do
            {
                j = 1;
                do
                {
                    txt = txt + i + " * " + j + " = " + i * j + "\r\n";
                    j++;
                } while (j <= 9);
                txt = txt + Environment.NewLine;
                i++;
            } while (i <= 9);
            textBox3.Text = txt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string txt = string.Empty;
            int i = 1, j;
            while (true)
            {
                j = 1;
                while (true)
                {
                    txt = txt + i + " * " + j + " = " + i * j + "\r\n";
                    j++;
                    if (j > 9) break;
                }
                txt = txt + Environment.NewLine;
                i++;
                if (i > 9) break;
            }
            textBox4.Text = txt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int[] num = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string txt = string.Empty;
            foreach (int i in num)
            {
                foreach (int j in num)
                {
                    txt = txt + i + " * " + j + " = " + i * j + "\r\n";
                }
                txt = txt + Environment.NewLine;
            }
            textBox5.Text = txt;
        }
    }
}
