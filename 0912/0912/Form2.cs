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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int formnum = 1; formnum <= 3; formnum++)
            {
                string name = "textBox" + Convert.ToInt32(formnum);
                string txt = "";
                for (int i = 1; i <= 9; i++)
                {
                    int index = formnum * 3 - 2;
                    txt = txt + index + " * " + i + " = " + index * i + "\t\t" + (index + 1) + " * " + i + " = " + (index + 1) * i + "\t\t" + (index + 2) + " * " + i + " = " + (index + 2) * i + "\r\n";
                }
                Control ctn = Controls[name];
                ctn.Text = Convert.ToString(txt);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int formnum = 1;
            while (formnum <= 3)
            {
                string name = "textBox" + Convert.ToInt32(formnum);
                string txt = "";
                int i = 1;
                while (i <= 9)
                {
                    int index = formnum * 3 - 2;
                    txt = txt + index + " * " + i + " = " + index * i + "\t\t" + (index + 1) + " * " + i + " = " + (index + 1) * i + "\t\t" + (index + 2) + " * " + i + " = " + (index + 2) * i + "\r\n";
                    i++;
                }
                Control ctn = Controls[name];
                ctn.Text = Convert.ToString(txt);
                formnum++;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int formnum = 1;
            while (true)
            {
                string name = "textBox" + Convert.ToInt32(formnum);
                string txt = "";
                int i = 1;
                while (true)
                {
                    int index = formnum * 3 - 2;
                    txt = txt + index + " * " + i + " = " + index * i + "\t\t" + (index + 1) + " * " + i + " = " + (index + 1) * i + "\t\t" + (index + 2) + " * " + i + " = " + (index + 2) * i + "\r\n";
                    i++;
                    if (i == 10) break; 
                }
                Control ctn = Controls[name];
                ctn.Text = Convert.ToString(txt);
                formnum++;
                if (formnum == 4) break;
            }
        }
    }
}
