using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0912
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        uint dan;
        private void button1_Click(object sender, EventArgs e)
        {
            int i;
            textBox1.Text = "";

            for (i = 1; i <= 9; i++)
            {
                textBox1.Text = textBox1.Text + dan + " * " + i + " = " + dan * i + "\r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = 1;
            textBox1.Text = "";

            while (i <= 9)
            {
                textBox1.Text = textBox1.Text + dan + " * " + i + " = " + dan * i + "\r\n";
                i++;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                dan = Convert.ToUInt32(textBox2.Text);
                if (dan == 0)
                {
                    MessageBox.Show("자연수를 입력해주세요");
                    textBox2.Text = "";
                    textBox2.Focus();
                }
            }
            catch
            {
                MessageBox.Show("자연수를 입력해주세요");
                textBox2.Text = "";
                textBox2.Focus();
               
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = 1, j = 1;
            /*for (i; i <= 9; i++)
            {
                for (j; j <= 9; j++) textBox1.Text = textBox1.Text + i + " * " + j + " = " + i * j + "\r\n";
                textBox1.Text = textBox1.Text + "\r\n";
            }*/
            while (i <= 9)
            {
                while (j <= 9)
                {
                    textBox1.Text = textBox1.Text + i + " * " + j + " = " + i * j + "\r\n";
                    j++;
                }
                textBox1.Text = textBox1.Text + "\r\n";
                j = 1;
                i++;
            }
        }
    }
}
