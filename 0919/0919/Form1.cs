using System;
using System.Windows.Forms;

namespace _0919
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint score = 0;
            string GP = "";
            

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

                if (score < 0 || score > 100) {
                    MessageBox.Show("0 ~ 100사이의 점수를 입력하세요.", "경고");
                    textBox1.Text = "";
                    textBox1.Focus();
                }
                else
                {
                    if (score > 95) GP = "A+";
                    else if (score > 90) GP = "A0";
                    else if (score > 85) GP = "B+";
                    else if (score > 80) GP = "B0";
                    else if (score > 75) GP = "C+";
                    else if (score > 70) GP = "C0";
                    else if (score > 65) GP = "D+";
                    else if (score > 60) GP = "D0";
                    else GP = "F";
                }
            }
           textBox2.Text = Convert.ToString(GP); 
        }
    }
}
