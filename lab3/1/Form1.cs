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
        private FinalChecker ch;
        private int type;
        public Form1()
        {
            InitializeComponent();
        }

        private void fill_op_table()
        {
            string op = "";
            string n1 = "", n2 = "";
            foreach (string line in System.IO.File.ReadLines(path + "\\1\\res\\default_op.txt"))
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

        private void fill_final_code(List<string[]> st)
        {
            bool ret = false;
            foreach (string[] el in st)
            {
                if (!ret)
                {
                    richTextBox_bin_code.AppendText(string.Join(" ", el));
                    ret = true;
                }
                else
                    richTextBox_bin_code.AppendText(Environment.NewLine + string.Join(" ", el));
            }
        }

        private void fill_code(string ch)
        {
            bool ret = false;
            string st;
            richTextBox_code.Clear();
            if (ch == "Прямая адресация")
            {
                st = path + "\\1\\res\\default_code1.txt";
                this.type = 1;
            }
            else if (ch == "Относительная адресация")
            {
                st = path + "\\1\\res\\default_code2.txt";
                this.type = 2;
            }
            else
            {
                st = path + "\\1\\res\\default_code3.txt";
                this.type = 3;
            }
            foreach (string line in System.IO.File.ReadLines(st))
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
            richTextBox_first_err.Clear();
            if (String.IsNullOrEmpty(richTextBox_code.Text))
            {
                richTextBox_first_err.Text = "Ошибка: в поле кода пусто!";
                return;
            }

            if (dataGrid_oper.Rows.Count <= 1 || dataGrid_oper.Rows == null)
            {
                richTextBox_first_err.Text = "Ошибка: таблица операций пуста!";
                return;
            }
            this.ch = new FinalChecker(get_code_table());
            ch.set_code(richTextBox_code.Text.Split('\n'));
            ch.set_type(this.type);
            string err = ch.first_cycle();
            if (err != "")
            {
                richTextBox_first_err.Text = err;
            }
            else
            {
                foreach (string[] st in ch.get_name_t())
                {
                    dataGrid_name.Rows.Add(new object[] { st[0], st[1], st[2], st[3] });
                }
                foreach (string[] st in ch.get_add_t())
                {
                    dataGrid_add.Rows.Add(new object[] { st[0], st[1], st[2], st[3] });
                }
                // проверка 
                but_sec_cycle.Enabled = true;
                but_first_cycle.Enabled = false;
            }   
        }

        private void but_fill_default_Click(object sender, EventArgs e)
        {
            richTextBox_first_err.Clear();
            if (comboBox_choice.SelectedItem != null)
            {
                fill_code(comboBox_choice.SelectedItem.ToString());
                Console.WriteLine(dataGrid_oper.Rows.Count);
                if (dataGrid_oper.Rows.Count == 1)
                {
                    fill_op_table();
                }
                but_fill_default.Enabled = false;
            }
            else
            {
                richTextBox_first_err.Text = "Не выбран пример кода!";
            }
        }

        private void but_reset_Click(object sender, EventArgs e)
        {
            dataGrid_name.Rows.Clear();
            dataGrid_add.Rows.Clear();
            dataGridView1.Rows.Clear();
            richTextBox_bin_code.Clear();
            richTextBox_sec_err.Clear();
            but_sec_cycle.Enabled = false;
            but_first_cycle.Enabled = true;
            but_fill_default.Enabled = true;
        }

        private void but_sec_cycle_Click(object sender, EventArgs e)
        {
            string err = ch.second_cycle();
            if (err != "")
            {
                richTextBox_sec_err.Text = err;
            }
            else
            {
                this.fill_final_code(ch.get_final_t());
                foreach (string[] el in ch.get_m())
                {
                    string st = el[0] + " " + el[1] + " " + el[2];
                    dataGridView1.Rows.Add(new object[] { st });
                }
                but_sec_cycle.Enabled = false;
            }
        }
    }
}
