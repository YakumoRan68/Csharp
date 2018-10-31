using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1031
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int pos = 0;
        bool delta = true;

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString();
            pos = delta ? ++pos : --pos;
            delta = pos == 5 || pos == 0 ? !delta : delta;
            hScrollBar1.Value = pos;
        }
    }
}
