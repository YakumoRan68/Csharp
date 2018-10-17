using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1017
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        int t = 1;
        bool delta = true;

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (t)
            {
                case 1: pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + "/../../Images/red.ico"); break;
                case 2: pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + "/../../Images/yellow.ico"); break;
                case 3: pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + "/../../Images/blue.ico"); break;
            }

            t = delta ? ++t : --t;
            delta = t == 3 || t == 1 ? !delta : delta;
        }
    }
}
