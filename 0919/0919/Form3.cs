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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int connum = 1;
            int sum = 0; 
            
            for (;connum <= 9; connum++)
            {
                int score = 0;
                string name = "textBox" + Convert.ToInt32(connum);
                string GP = "";
                Control ctn = Controls[name];

                if (ctn.Text == "") break;

                try
                {
                    score = Convert.ToInt32(ctn.Text);
                }
                catch
                {
                    MessageBox.Show("하나이상의 올바르지 않은 값이 있습니다. 빈칸의 값을 다시 입력해주세요", "오류");
                    ctn.Text = "";
                    ctn.Focus();
                    return;
                }

                if (score < 0 || score > 100)
                {
                    MessageBox.Show("0 ~ 100점의 점수를 입력해야 합니다. 빈칸의 값을 다시 입력해주세요", "오류");
                    ctn.Text = "";
                    ctn.Focus();
                    return;
                }
                else
                {
                    switch (score / 5)
                    {
                        case 19: GP = "A+"; break;
                        case 18: GP = "A0"; break;
                        case 17: GP = "B+"; break;
                        case 16: GP = "B0"; break;
                        case 15: GP = "C+"; break;
                        case 14: GP = "C0"; break;
                        case 13: GP = "D+"; break;
                        case 12: GP = "D"; break;
                        default: GP = "F"; break;
                    }
                }
                sum = sum + score;
                name = "GpBox" + Convert.ToInt32(connum);
                ctn = Controls[name];
                ctn.Text = Convert.ToString(GP);
            }

            textBox10.Text = Convert.ToString(sum / (connum-1));
        }
    }
}
