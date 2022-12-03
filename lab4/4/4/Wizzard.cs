using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4
{
    class Wizzard
    {
        private string[] registers = { "R1", "R2", "R3", "R4", "R5", "R6", "R7", "R8", "R9", "R10", "R11", "R12", "R13", "R14", "R15" };
        private string[] p_c = { "START", "END", "RESW", "WORD", "RESB", "BYTE" };
        private int str_counter;
        private int begin_address;
        private int last_address;
        private int address_counter;
        private List<string[]> name_table;
        private List<string[]> code_table;
        private List<string[]> final_table;
        private List<string[]> code;

        // нужно ли поле имени программы?

        public Wizzard(List<string[]> sent_code)
        {
            this.code = new List<string[]>();
            this.name_table = new List<string[]>();
            this.final_table = new List<string[]>();
            this.code_table = sent_code;
            this.str_counter = 1;
            this.begin_address = 0;
        }

        private void print_arr(string[] array)
        {
            Console.WriteLine("[{0}]", string.Join(", ", array));
        }

        public void print_final_table()
        {
            foreach (string[] el in this.final_table)
            {
                print_arr(el);
            }
            Console.WriteLine();
        }

        public List<string[]> get_final_table()
        {
            return this.final_table;
        }

        public List<string[]> get_name_table()
        {
            return this.name_table;
        }

        // ПУСТЫЕ МЕТКИ В _
        public void step(string[] line)
        {
            switch (line[1])
            {
                case "START":
                    //проверка имени программы и адреса загрузки на формат
                    if (this.begin_address != 0) // или же проверка на str_counter
                    {
                        //повторение директивы
                    }
                    this.begin_address = Convert.ToInt32(line[2], 16);
                    this.address_counter = begin_address;
                    this.str_counter += 1;
                    this.final_table.Add(new string[] { "H", line[0], this.address_counter.ToString("X6"), "?" });
                    break;
                    //return new string[][] { final_table[str_counter - 1], new string[] { } };
                case "RESB":
                    // проверка на метку (корректность и отсутствие похожей и пустоты)
                    this.name_table.Add(new string[] { line[0], this.address_counter.ToString("X6"), "" });
                    // проверка на число
                    this.str_counter += 1;
                    this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), Convert.ToInt32(line[2]).ToString("X")});
                    this.address_counter += (Int32.Parse(line[2]));
                    break;
                case "RESW":
                    // есть ли метка + корректность метки + есть ли такая
                    this.name_table.Add(new string[] { line[0], this.address_counter.ToString("X6"), "" });
                    // проверка на число
                    this.str_counter += 1;
                    this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), Convert.ToInt32(line[2]).ToString("X") });
                    Console.WriteLine(Convert.ToInt32(line[2]).ToString("X6")); // ?????
                    this.address_counter += (Int32.Parse(line[2]) * 3);
                    break;
                case "WORD":
                    // все тоже самое + проверка переполнения
                    this.name_table.Add(new string[] { line[0], this.address_counter.ToString("X6"), "" });
                    this.str_counter += 1;
                    this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "06", Convert.ToInt32(line[2]).ToString("X6") });
                    this.address_counter += 3;
                    break;
                case "BYTE":
                    int add = 0;
                    // все тоже самое с метками
                    this.name_table.Add(new string[] { line[0], this.address_counter.ToString("X6"), "" });
                    this.str_counter += 1;
                    // переполнение
                    // проверка на первый символ - если c - проверяем на кавычки, если x - проверяем на формат, если другое - на число
                    // до высчитывания адреса break, если неправильный формат команды
                    
                    if (int.TryParse(line[2], out _))
                    {
                        add = 1;
                    }
                    else if (line[2][0] == 'x')
                    {
                        add = (line[2].Length - 1) / 2;
                    }
                    else if (line[2][0] == 'c')
                    {
                        add = (line[2].Length - 3);
                    }
                    this.final_table.Add(new string[] { "T", "Пока заглушка!" });
                    this.address_counter += add;
                    break;
                case "END":
                    // если есть метка, то x
                    // проверка на попадание в область памяти
                    // переполнение + неправильный формат адреса + лишние END
                    this.last_address = this.address_counter - this.begin_address;
                    this.final_table.Add(new string[] { "E", Convert.ToInt32(line[2]).ToString("X") });
                    break;
            }
        }
    }
}
