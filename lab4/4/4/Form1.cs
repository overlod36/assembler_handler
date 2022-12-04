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

namespace _4
{
    public partial class Form1 : Form
    {
        private string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private int str_counter = 0;
        private Wizzard wz;
        public Form1()
        {
            InitializeComponent();
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

        private void fill_code()
        {
            bool ret = false;
            if (richText_code.Text.Length == 0)
            {
                foreach (string line in System.IO.File.ReadLines(path + "\\" + path.Split('\\').Last() + "\\res\\default_code.txt"))
                {
                    if (!ret)
                    {
                        richText_code.AppendText(line);
                        ret = true;
                    }
                    else
                        richText_code.AppendText(Environment.NewLine + line);
                }
            }
            else
            {
                Console.WriteLine("Поле с кодом уже занято!");
            }
        }

        private void fill_op_table()
        {
            string op = "";
            string n1 = "", n2 = "";
            if (dataGrid_oper.Rows.Count == 1)
            {
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
        }

        private void update_code()
        {
            richText_bincode.Clear();
            bool ret = false;
            foreach (string[] el in wz.get_final_table())
            {
                if (!ret)
                {
                    richText_bincode.AppendText(string.Join(" ", el));
                    ret = true;
                }
                else
                    richText_bincode.AppendText(Environment.NewLine + string.Join(" ", el));
            }
        }

        private void default_button_Click(object sender, EventArgs e)
        {
            fill_code();
            fill_op_table();
        }

        private void reset_button_Click(object sender, EventArgs e)
        {
            dataGrid_names.Rows.Clear();
            richText_errors.Clear();
            richText_bincode.Clear();
            this.str_counter = 0;
            string_num.Clear();
        }

        private void by_step_button_Click(object sender, EventArgs e)
        {
            richText_errors.Clear();
            if (richText_code.TextLength == 0)
            {
                richText_errors.Text = "Ошибка: поле кода пустое!";
                return;
            }
            if (dataGrid_oper.Rows.Count <= 1 || dataGrid_oper.Rows == null)
            {
                richText_errors.Text = "Ошибка: таблица кодов операций пустая!";
                return;
            }
            // на каждой итерации проверка на наличие ошибки
            if (this.str_counter == 0)
            {
                richText_bincode.Clear();
                dataGrid_names.Rows.Clear();
                this.str_counter = 1;
                string_num.Text = this.str_counter.ToString();
                this.wz = new Wizzard(get_code_table());
                wz.step(richText_code.Text.Split('\n')[str_counter-1].Split());
                if (wz.get_error() != "")
                {
                    richText_errors.Text = wz.get_error();
                    this.str_counter = 0;
                    return;
                }
                richText_bincode.AppendText(string.Join(" ", wz.get_final_table()[str_counter-1]));
            }
            else
            {
                if (wz.get_error() != "")
                {
                    richText_errors.Text = wz.get_error();
                    return;
                }
                this.str_counter += 1;
                if (str_counter > richText_code.Text.Split('\n').Count())
                {
                    str_counter = 0;
                    update_code();
                    return;
                }
                string_num.Text = this.str_counter.ToString();
                wz.step(richText_code.Text.Split('\n')[str_counter - 1].Split());
                richText_bincode.AppendText(Environment.NewLine + string.Join(" ", wz.get_final_table()[str_counter - 1]));
                dataGrid_names.Rows.Clear();
                foreach (string[] st in wz.get_name_table())
                {
                    dataGrid_names.Rows.Add(new object[] { st[0], st[1], st[2] });
                }
                if (wz.to_update == true)
                {
                    wz.to_update = false;
                    update_code();
                }
            } 
        }

        private void full_button_Click(object sender, EventArgs e)
        {
            richText_errors.Clear();
            if (richText_code.TextLength == 0)
            {
                richText_errors.Text = "Ошибка: поле кода пустое!";
                return;
            }
            if (dataGrid_oper.Rows.Count <= 1 || dataGrid_oper.Rows == null)
            {
                richText_errors.Text = "Ошибка: таблица кодов операций пустая!";
                return;
            }
            this.str_counter = 0;
            string_num.Clear();
            this.wz = new Wizzard(get_code_table());
            wz.full_cycle(richText_code.Text.Split('\n'));
            if (wz.get_error() == "")
            {
                dataGrid_names.Rows.Clear();
                foreach (string[] st in wz.get_name_table())
                {
                    dataGrid_names.Rows.Add(new object[] { st[0], st[1], st[2] });
                }
                update_code();
            }
            else
            {
                richText_errors.Text = wz.get_error();
            }
        }
    }
}
