using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4
{
    class Wizzard
    {
        public bool to_update;
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
            this.to_update = false;
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

        private void set_mark_address(string mark)
        {
            bool ch = false;
            foreach (string[] el in this.name_table)
            {
                if (el[0] == mark)
                {
                    el[1] = this.address_counter.ToString("X6");
                    foreach (string[] sm in this.final_table)
                    {
                        if (el[2].Split().Contains(sm[1]))
                        {
                            sm[4] = this.address_counter.ToString("X6");
                            sm[2] = (sm[3].Length + sm[4].Length).ToString("X2");
                            this.to_update = true;
                        }
                    }
                    ch = true;
                }
            }
            if (!ch)
                this.name_table.Add(new string[] { mark, this.address_counter.ToString("X6"), "" });
        }

        private string get_mark_address(string mark)
        {
            foreach (string[] el in this.name_table)
            {
                if (el[0] == mark)
                {
                    return el[1];
                }
            }
            return "mfdoom";
        }

        private void add_waiter(string mark)
        {
            foreach (string[] el in this.name_table)
            {
                if (el[0] == mark)
                {
                    el[2] = el[2] + " " + this.address_counter.ToString("X6");
                }
            }
        }

        private int[] get_command_code(string[] line)
        {
            foreach (string[] st in this.code_table)
            {
                if (st[0] == (line[1] + " "))
                {
                    if (line.Length == 4)
                    {
                        if (this.registers.Contains(line[2]) && this.registers.Contains(line[3]))
                            return new int[] { Int32.Parse(st[1]) * 4, Int32.Parse(st[2]) };
                    }
                    return new int[] { Int32.Parse(st[1]) * 4 + 1, Int32.Parse(st[2]) }; // проверка таблицы в самом начале???
                }
                else if (st[0] == (line[1] + " " + line[2] + " "))
                    return new int[] { Int32.Parse(st[1]) * 4 + 1, Int32.Parse(st[2]) };
            }
            return new int[] { };
        }

        private string[] str_maker(string[] st)
        {
            string[] res = { st[0], st[1], ""};
            string smth = "";
            for (int i = 2; i < st.Length; i++)
            {
                smth = smth + st[i] + " ";
            }
            res[2] = smth.Remove(smth.Length-1);
            return res;
        }

        // пустые строки
        public void step(string[] line)
        {
            if (line[0] == "")
            {
                line[0] = "_";
            }
            print_arr(line);
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
                        this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), Convert.ToInt32(line[2]).ToString("X").Length.ToString("X2"), Convert.ToInt32(line[2]).ToString("X") });
                    }
                    else if (line[2][0] == 'x')
                    {
                        add = (line[2].Length - 1) / 2;
                        this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), line[2].Substring(1, line[2].Length - 1).Length.ToString("X2"), line[2].Substring(1, line[2].Length - 1) });
                    }
                    else if (line[2][0] == 'c') // проверка закрываемости кавычек при обычной и большой длине
                    {
                        string last;
                        if (line.Length > 3)
                        {
                            last = str_maker(line)[2];
                            byte[] ba = Encoding.Default.GetBytes(last.Substring(2, last.Length - 3));
                            this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), BitConverter.ToString(ba).Replace("-", "").Length.ToString("X2"), BitConverter.ToString(ba).Replace("-", "") });
                        }
                        else
                        {
                            byte[] ba = Encoding.Default.GetBytes(line[2].Substring(2, line[2].Length - 3));
                            this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), BitConverter.ToString(ba).Replace("-", "").Length.ToString("X2"), BitConverter.ToString(ba).Replace("-", "") });
                        }
                        add = (line[2].Length - 3);
                    }
                    else
                    {
                        // ошибка
                    }
                    this.address_counter += add;
                    break;
                case "END":
                    // если есть метка, то x
                    // проверка на попадание в область памяти
                    // переполнение + неправильный формат адреса + лишние END
                    this.last_address = this.address_counter - this.begin_address;
                    Console.WriteLine(this.last_address);
                    this.final_table[0][3] = this.last_address.ToString("X6");
                    this.final_table.Add(new string[] { "E", line[2] });
                    break;
                default:
                    // сразу проверка на наличие команды
                    if (line[0] != "_")
                    {
                        // проверка метки
                        set_mark_address(line[0]);
                        this.to_update = true;
                        if (line.Length == 3)
                        {
                            int[] helper1 = get_command_code(line);
                            string addr = get_mark_address(line[2]);
                            if (addr != "" && addr != "mfdoom")
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), (helper1[0].ToString("X2").Length + addr.Length).ToString("X2"), helper1[0].ToString("X2"), addr });
                            else if (addr == "mfdoom")
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "?", helper1[0].ToString("X2"), "?" });
                                this.name_table.Add(new string[] { line[2], "", this.address_counter.ToString("X6") });
                            }
                            else
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "?", helper1[0].ToString("X2"), "?" });
                                add_waiter(line[2]);
                            }
                                
                            this.address_counter += helper1[1];
                        }
                        else if (line.Length == 4)
                        {
                            if (this.registers.Contains(line[3]) && this.registers.Contains(line[4]))
                            {
                                int[] helper1 = get_command_code(line);
                                string addr = get_mark_address(line[2]);
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), (helper1[0].ToString("X2").Length + 2).ToString("X2"), helper1[0].ToString("X2"), line[3][1].ToString() , line[4][1].ToString() });
                                this.address_counter += helper1[1];
                            }
                            else
                            {
                                // ошибка
                            }
                        }
                        else
                        {
                            // ошибка
                        }
                    }
                    else
                    {
                        if (line.Length == 3)
                        {
                            int[] helper1 = get_command_code(line);
                            string addr = get_mark_address(line[2]);
                            if (addr != "" && addr != "mfdoom")
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), (helper1[0].ToString("X2").Length + addr.Length).ToString("X2"), helper1[0].ToString("X2"), addr });
                            else if (addr == "mfdoom")
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "?", helper1[0].ToString("X2"), "?" });
                                this.name_table.Add(new string[] { line[2], "", this.address_counter.ToString("X6") });
                            }
                            else
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "?", helper1[0].ToString("X2"), "?" });
                                add_waiter(line[2]);
                            }

                            this.address_counter += helper1[1];
                        }
                        else if (line.Length == 4)
                        {
                            if (this.registers.Contains(line[2]) && this.registers.Contains(line[3]))
                            {
                                int[] helper1 = get_command_code(line);
                                string addr = get_mark_address(line[2]);
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), (helper1[0].ToString("X2").Length + 2).ToString("X2"), helper1[0].ToString("X2"), line[2][1].ToString(), line[3][1].ToString() });
                                this.address_counter += helper1[1];
                            }
                            else
                            {
                                // ошибка
                            }
                        }
                        else
                        {
                            // ошибка
                        }
                    }
                    break;
            }
        }

        public void full_cycle(string[] code)
        {
            foreach (string str in code)
            {
                step(str.Split());
            }
        }
    }
}
