using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0905
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        int a = 0;

        private void textBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                a = Convert.ToInt32(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("자연수를 입력해주세요");
                textBox1.Text = "";
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                textBox2.Text = "";
                textBox3.Text = "";
                int i, Num = a, Sum1 = 0, Sum2 = 0;
                for (i = 1; i <= Num; i = i + 1)
                {
                    if (i % 2 == 1)
                    {
                        Sum1 = Sum1 + i;
                        if (Num == i || Num - 1 == i) textBox2.Text = textBox2.Text + i + " = " + Sum1;
                        else textBox2.Text = textBox2.Text + i + " + ";
                    }
                    else
                    {
                        Sum2 = Sum2 + i;
                        if (Num == i || Num - 1 == i) textBox3.Text = textBox3.Text + i + " = " + Sum2;
                        else textBox3.Text = textBox3.Text + i + " + ";
                    }
                }
            }
        }
    }
}
