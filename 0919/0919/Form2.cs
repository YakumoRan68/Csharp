using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0919
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint score = 0;
            string GP = "";
            textBox2.Text = "";

            if (textBox1.Text == "")
            {
                MessageBox.Show("점수를 입력하고 버튼을 눌러주세요.", "경고");
                textBox1.Focus();
            }
            else
            {
                try
                {
                    score = uint.Parse(textBox1.Text);
                }
                catch
                {
                    MessageBox.Show("올바르지 않은 값입니다", "오류");
                    textBox1.Text = "";
                    textBox1.Focus();
                    return;
                }

                if (score < 0 || score > 100)
                {
                    MessageBox.Show("0 ~ 100사이의 점수를 입력하세요.", "경고");
                    textBox1.Text = "";
                    textBox1.Focus();
                }
                else
                {
                    switch (score / 5)
                    {
                        case 20: 
                        case 19: GP = "A+"; break;
                        case 18: GP = "A0"; break;
                        case 17: GP = "B+"; break;
                        case 16: GP = "B0"; break;
                        case 15: GP = "C+"; break;
                        case 14: GP = "C0"; break;
                        case 13: GP = "D+"; break;
                        case 12: GP = "D"; break;
                        default: GP = "F"; break;
                    };
                }
            }
            textBox2.Text = Convert.ToString(GP);
        }
    }
}
