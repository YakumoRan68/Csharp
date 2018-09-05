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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
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
            textBox2.Text = "";
            textBox3.Text = "";
            int i, Num = a, Sum = 0;
            for (i = 1; i <= Num; i = i + 1) {
                Sum = Sum + i;
                if (i == Num) textBox2.Text = textBox2.Text + i;
                else textBox2.Text = textBox2.Text + i + " + ";
            }

            textBox3.Text = textBox3.Text + Sum;
        }
    }
}