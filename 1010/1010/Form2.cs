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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txt = "";
            for (int i = 1; i < 15; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    txt = txt + "*";
                }
                txt = txt + "\r\n";
            }
            textBox1.Text = txt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = "";
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (j < i) txt = txt + " ";
                    else txt = txt + "*";
                }
                txt = txt + "\r\n";
            }
            textBox2.Text = txt;
        }
    }
}
