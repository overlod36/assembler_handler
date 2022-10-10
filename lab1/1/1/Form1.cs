using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

        private void but_load_code_Click(object sender, EventArgs e)
        {
            bool ret = false;
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            if (richTextBox_code.Text.Length == 0)
            {
                foreach (string line in System.IO.File.ReadLines(path + "\\" + path.Split('\\').Last() + "\\res\\default_code.txt"))
                {
                    if (!ret)
                    {
                        richTextBox_code.AppendText(line);
                        ret = true;
                    }
                    else
                        richTextBox_code.AppendText(Environment.NewLine + line);
                }
            }
            else
            {
                Console.WriteLine("Поле с кодом уже занято!");
            }
        }
    }
}
