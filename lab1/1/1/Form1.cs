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
        private string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public Form1()
        {
            InitializeComponent();
        }

        private void fill_op_table()
        {
            string op = "";
            string n1 = "", n2 = "";
            foreach (string line in System.IO.File.ReadLines(path + "\\" + path.Split('\\').Last() + "\\res\\default_op.txt"))
            {
                foreach (string el in line.Split(' '))
                {
                    if (!int.TryParse(el, out _))
                    {
                        op = op + el + " ";
                    }
                    else
                    {
                        if (n1.Length == 0)
                        {
                            n1 = el;
                        }
                        else
                        {
                            n2 = el;
                        }
                    }
                }
                dataGrid_oper.Rows.Add(new object[] { op, n1, n2 });
                op = "";
                n1 = "";
                n2 = "";
            }

        }

        private void fill_code()
        {
            bool ret = false;
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

        private List<string[]> get_code_table()
        {
            List<string[]> st = new List<string[]>();
            foreach (DataGridViewRow row in dataGrid_oper.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string[] res = { row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString() };
                    st.Add(res);
                }
            }
            return st;
        }

        private void but_first_cycle_Click(object sender, EventArgs e)
        {
            CodeChecker ch = new CodeChecker(get_code_table());
            ch.first_cycle(richTextBox_code.Text.Split('\n'));
            foreach (string[] st in ch.get_nt())
            {
                dataGrid_name.Rows.Add(new object[] { st[0], st[1] });
            }
            foreach (string[] st in ch.get_at())
            {
                dataGrid_add.Rows.Add(new object[] { st[0], st[1], st[2], st[3] });
            }
        }

        private void but_fill_default_Click(object sender, EventArgs e)
        {
            fill_code();
            fill_op_table();
        }
    }
}
