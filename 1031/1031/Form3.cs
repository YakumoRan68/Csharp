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
                var count = Directory.GetFiles(@Environment.CurrentDirectory + "/../../Images/" + i.ToString() + "/", "*.jpg").Length; // 해당 이름을 가진 디렉토리의 파일 갯수
                ((PictureBox)Controls["pictureBox" + i.ToString()]).Image = Image.FromFile(Environment.CurrentDirectory + "/../../Images/" + i.ToString() + "/" + Convert.ToString((tick % count) + 1) + ".jpg");
            }
            tick++;
        }
    }
}
