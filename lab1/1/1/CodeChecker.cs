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
        public CodeChecker()
        {
            this.code = new List<string[]>();
        }

        private void set_code(string[] b_code)
        {
            foreach (var str in b_code) // проверка на пустые строки
            {
                this.code.Add(str.Split());
            }
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
            if (this.code[0].Length == 3)
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

        }
    }
}
