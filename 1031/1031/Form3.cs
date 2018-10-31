using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1031
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        int tick = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i <= 3; i++) {
                int count = Directory.GetFiles(@Environment.CurrentDirectory + "/../../Images/" + i.ToString() + "/", "*.jpg").Length; // 해당 이름을 가진 디렉토리의 파일의 리스트(자료형 list).갯수
                ((PictureBox)Controls["pictureBox" + i.ToString()]).Image = Image.FromFile(Environment.CurrentDirectory + "/../../Images/" + i.ToString() + "/" + Convert.ToString((tick % count) + 1) + ".jpg");
                //(PictureBox) : Controls[]로 컨트롤을 묶음으로 가져올 수 있고 PictureBox도 컨트롤이나, PictureBox의 Image 매서드를 호출할 경우에는 명시적인 형변환이 필요함.
                //문자열으로의 명시적인 형변환의 종류 : Convert.ToString(i), i.ToString
            }
            tick++;
        }
    }
}
