using System;
using System.Drawing;
using System.Windows.Forms;

namespace _1031
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        int tick = 1, type = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ChangeImg(tick);
            tick++;
        }

        public void ChangeImg(int a)
        {
            string Dir = "/../../Images/";
            /*switch (a) {
                case 1: pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + Dir + "1/1.jpg"); break;
                case 2: pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + Dir + "1/2.jpg"); break;
                case 3: pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + Dir + "1/3.jpg"); break;
                case 4: pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + Dir + "1/4.jpg"); break;
            }*/
            int frame = a;
            try
            {
                pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + Dir + Convert.ToString(type)+ "/" + Convert.ToString(frame) + ".jpg" );
            }
            catch(System.IO.FileNotFoundException)
            {
                frame = 1;
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            type = 3;
            tick = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            type = 2;
            tick = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            type = 1;
            tick = 0;
        }
    }
}
