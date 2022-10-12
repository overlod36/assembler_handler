using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class CodeChecker
    {
        private int address_counter;
        private List<string[]> code;
        private List<string[]> name_table;
        private List<string[]> add_table;
        private int last_address;

        public CodeChecker()
        {
            this.code = new List<string[]>();
            this.name_table = new List<string[]>();
            this.add_table = new List<string[]>();
        }

        public List<string[]> get_nt()
        {
            return this.name_table;
        }

        public List<string[]> get_at()
        {
            return this.add_table;
        }

        private void set_code(string[] b_code)
        {
            foreach (var str in b_code) // проверка на пустые строки
            {
                // проверка на пустые строки в самих передаваемых строках
                this.code.Add(str.Split());
            }
        }

        private string calculate_cc()
        {
            return " ";
        }

        public void show_code()
        {
            foreach (var st in this.code)
            {
                foreach (var el in st)
                {
                    Console.WriteLine(el);
                }
            }
        }

        public bool check_begin_address()
        {
            if (this.code[0].Length == 3) // проверка на имя программы
            {
                if (int.TryParse(this.code[0][2], out _))
                {
                    return true;
                }
            }
            return false;
        }

        public void first_cycle(string[] st)
        {
            bool check_begin = false;
            this.set_code(st);
            if (check_begin_address()) // + проверка на переполнение
            {
                this.address_counter = Int32.Parse(code[0][2]);
                Console.WriteLine(this.address_counter);
            }
            else {
                Console.WriteLine("Неправильный начальный адрес!");
                return;
            }

            foreach (string[] str in this.code) // на каждой итерации проверять адреса на переполнение
            {
                if (!check_begin) // проверка первой строки
                {
                    check_begin = true;
                    string[] at1 = { str[0], str[1], str[2], " " };
                    add_table.Add(at1);
                    continue;
                }
                if (str[0] != "_") // проверка метки, еще добавить проверку на присутствие такого в таблице
                {
                    string[] tp = { str[0], " " };
                    string[] at2;
                    name_table.Add(tp);
                    if (str.Length == 4)
                    {
                        at2 = new string[] { "Адрес", str[1], str[2], str[3] };
                        add_table.Add(at2);
                    }
                    else if (str.Length == 3)
                    {
                        at2 = new string[] { "Адрес", str[1], str[2], " " };
                        add_table.Add(at2);
                    }
                }
                else
                {
                    string[] at3;
                    if (str.Length == 3)
                    {
                        at3 = new string[] { "Адрес", str[1], str[2], " " };
                        add_table.Add(at3);
                    }
                    else if (str.Length == 3)
                    {
                        at3 = new string[] { "Адрес", str[1], str[2], str[3] };
                        add_table.Add(at3);
                    }
                }
            }
        }
    }
}
