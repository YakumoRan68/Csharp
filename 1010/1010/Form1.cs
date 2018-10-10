using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1010
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txt = "";
            for (int i = 1; i <= 9; i++)
            {
                txt = txt + "3 * " + i + " = " + 3*i + "\r\n";
            }
            textBox1.Text = txt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = "";
            int i = 1;
            while (i <= 9)
            {
                txt = txt + "4 * " + i + " = " + 4 * i + "\r\n";
                i++;
            }
            textBox2.Text = txt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string txt = "";
            int i = 1;
            do
            {
                txt = txt + "5 * " + i + " = " + 5 * i + "\r\n";
                i++;
            } while (i <= 9);
            textBox3.Text = txt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string txt = "";
            int i = 1;
            while (true)
            {
                txt = txt + "6 * " + i + " = " + 6 * i + "\r\n";
                i++;
                if (i > 9) break;
            }
            textBox4.Text = txt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int[] num = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string txt = "";
            foreach (int i in num)
            {
                txt = txt + "7 * " + i + " = " + 7 * i + "\r\n";
            }
            textBox5.Text = txt;
        }
    }
}
