using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void but_first_cycle_Click(object sender, EventArgs e)
        {
            CodeChecker ch = new CodeChecker();
            ch.first_cycle(richTextBox_code.Text.Split('\n'));
            ch.show_code();
        }
    }
}
