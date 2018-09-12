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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txt = "";
            for (int num = 1; num <= 3; num++)
            {
                for (int i = 1; i <= 9; i++)
                {
                    int index = num*3-2;
                    txt = txt + index + " * " + i + " = " + index * i + "\t\t" + (index + 1) + " * " + i + " = " + (index + 1) * i + "\t\t" + (index + 2) + " * " + i + " = " + (index + 2) * i + "\r\n";
                }
                txt = txt + "\r\n";
                textBox1.Text = Convert.ToString(txt);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = "";
            int num = 1;
            while (num <= 3)
            {
                int i = 1;
                while (i <= 9)
                {
                    int index = num * 3 - 2;
                    txt = txt + index + " * " + i + " = " + index * i + "\t\t" + (index + 1) + " * " + i + " = " + (index + 1) * i + "\t\t" + (index + 2) + " * " + i + " = " + (index + 2) * i + "\r\n";
                    i++;
                }
                txt = txt + "\r\n";
                textBox1.Text = Convert.ToString(txt);
                num++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string txt = "";
            int num = 1;
            while (true)
            {
                int i = 1;
                while (true)
                {
                    int index = num * 3 - 2;
                    txt = txt + index + " * " + i + " = " + index * i + "\t\t" + (index + 1) + " * " + i + " = " + (index + 1) * i + "\t\t" + (index + 2) + " * " + i + " = " + (index + 2) * i + "\r\n";
                    i++;
                    if (i == 10) break;
                }
                txt = txt + "\r\n";
                textBox1.Text = Convert.ToString(txt);
                num++;
                if (num == 4) break;
            }
        }
    }
}
