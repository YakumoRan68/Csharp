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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox3.ReadOnly = true;
        }

        long x, y;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            x = long.Parse(textBox1.Text); // 바꿀 문자열이 Null 인경우 에러
            //Convert.ToInt64(textBox1.Text); Convert => Null인경우 무시
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            y = long.Parse(textBox2.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = (x + y).ToString(); // textBox에는 string 형식만 올 수 있음.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = (x - y).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = (x * y).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = (x / y).ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox3.Text = Math.Pow(x, y).ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox3.Text = (x % y).ToString();
        }
    }
}
