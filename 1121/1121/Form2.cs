using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1121 {
    public partial class Form2 : Form {
        public Form2() {
            InitializeComponent();
        }
        private void 일반용계산기ToolStripMenuItem_Click(object sender, EventArgs e) {
            Owner.Show();
            Close();
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e) {
            Owner.Close();
            Close();
        }
    }
}
