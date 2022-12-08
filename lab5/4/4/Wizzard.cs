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
        private string error;
        private int type;
        public bool empty;

        // нужно ли поле имени программы?

        public Wizzard(List<string[]> sent_code, int ch)
        {
            this.name_table = new List<string[]>();
            this.final_table = new List<string[]>();
            this.code_table = sent_code;
            this.str_counter = 1;
            this.begin_address = 0;
            this.to_update = false;
            this.error = "";
            this.empty = false;
            this.type = ch;
        }

        private string get_str_counter()
        {
            return this.str_counter.ToString();
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
        public string get_error()
        {
            return this.error;
        }
        private bool check_hex(string num)
        {
            foreach (char ch in num)
            {
                if (!((ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F')))
                    return false;
            }
            return true;
        }

        private bool check_quotes(string for_ch)
        {
            int counter = 0;
            foreach (char ch in for_ch)
            {
                if (ch == '\'')
                    counter += 1;
            }
            if (counter > 1)
                return true;
            return false;
        }

        private bool check_str(string mark)
        {
            bool begin = true;
            foreach (char el in mark)
            {
                if (begin && (el >= '0' && el <= '9'))
                {
                    this.error = "Ошибка: некорректное имя метки/команды, первый символ - цифра!";
                    return false;
                }
                if (begin && el == '_')
                {
                    this.error = "Ошибка: некорректное имя метки/команды!";
                    return false;
                }
                begin = false;
            }
            return true;
        }

        private bool mark_in(string mark)
        {
            foreach (string[] str in this.name_table)
            {
                if (str[0] == mark)
                {
                    return true;
                }
            }
            return false;
        }

        private string check_name_table()
        {
            string res = "";
            foreach (string[] el in this.name_table)
            {
                if (el[1] == "")
                {
                    res = res + el[0] + " ";
                }
            }
            return res;
        }

   
        // пустые строки
        public void step(string[] line)
        {
            this.empty = false;
            print_arr(line);
            if (line[0] == "")
            {
                line[0] = "_";
                if (line.Length == 1)
                {
                    this.empty = true;
                    return;
                }
            }
            switch (line[1])
            {
                case "START":
                    //проверка имени программы 
                    if (this.begin_address != 0)
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: повторное использование директиввы START!";
                        break;
                    }
                    if (line.Length == 2)
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: отсутствует загрузочный адрес!";
                        break;
                    }
                    if (line[2] != "0")
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: загрузочный адрес должен быть равен 0!";
                        break;
                    }
                    if (!check_hex(line[2]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: неправильный формат адреса загрузки!";
                        break;
                    }
                    if (line[2].Length > 6)
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: переполнение адреса загрузки!";
                        break;
                    }
                    this.begin_address = Convert.ToInt32(line[2], 16);
                    this.address_counter = begin_address;
                    this.str_counter += 1;
                    this.final_table.Add(new string[] { "H", line[0], this.address_counter.ToString("X6"), "?" });
                    break;
                case "RESB":
                    if (line[0] == "_")
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: директива RESW без метки!";
                        break;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: некорректное имя метки!";
                        break;
                    }
                    if (this.registers.Contains(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: имя метки зарезирвированно!";
                        break;
                    }
                    if (mark_in(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: " + line[0] + " уже есть в таблице символических имен!";
                        break;
                    }
                    if (!int.TryParse(line[2], out _))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: Введено не число!";
                        break;
                    }
                    this.name_table.Add(new string[] { line[0], this.address_counter.ToString("X6"), "" });
                    this.str_counter += 1;
                    this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), Convert.ToInt32(line[2]).ToString("X")});
                    this.address_counter += (Int32.Parse(line[2]));
                    break;
                case "RESW":
                    if (line[0] == "_")
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: директива RESW без метки!";
                        break;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: некорректное имя метки!";
                        break;
                    }
                    if (this.registers.Contains(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: имя метки зарезирвированно!";
                        break;
                    }
                    if (mark_in(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: " + line[0] + " уже есть в таблице символических имен!";
                        break;
                    }
                    if (!int.TryParse(line[2], out _))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: Введено не число!";
                        break;
                    }
                    this.name_table.Add(new string[] { line[0], this.address_counter.ToString("X6"), "" });
                    this.str_counter += 1;
                    this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), (Convert.ToInt32(line[2])*3).ToString("X6") });
                    this.address_counter += (Int32.Parse(line[2]) * 3);
                    break;
                case "WORD":
                    int ch;
                    if (line[0] == "_")
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: директива RESW без метки!";
                        break;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: некорректное имя метки!";
                        break;
                    }
                    if (this.registers.Contains(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: имя метки зарезирвированно!";
                        break;
                    }
                    if (mark_in(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: " + line[0] + " уже есть в таблице символических имен!";
                        break;
                    }
                    if (!int.TryParse(line[2], out ch))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: Введено не число!";
                        break;
                    }
                    if (ch > 16777215)
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: введено число меньше 16777215!";
                        break;
                    }
                    this.name_table.Add(new string[] { line[0], this.address_counter.ToString("X6"), "" });
                    this.str_counter += 1;
                    this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "06", Convert.ToInt32(line[2]).ToString("X6") });
                    this.address_counter += 3;
                    break;
                case "BYTE":
                    if (line[0] == "_")
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: директива RESW без метки!";
                        break;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: некорректное имя метки!";
                        break;
                    }
                    if (this.registers.Contains(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: имя метки зарезирвированно!";
                        break;
                    }
                    if (mark_in(line[0]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: " + line[0] + " уже есть в таблице символических имен!";
                        break;
                    }
                    if (line[2][0] != 'c' && line[2][0] != 'x' && !int.TryParse(line[2], out _))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: неверный формат константы!";
                        break;
                    }

                    if (line[2][0] == 'x' && !check_hex(line[2].Substring(1)))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: неверный формат константы, введите 16-ричное число!";
                        break;
                    }
                    int add = 0;
                    this.name_table.Add(new string[] { line[0], this.address_counter.ToString("X6"), "" });
                    this.str_counter += 1;

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
                            if (!check_quotes(last))
                            {
                                this.error = "(" + get_str_counter() + ") Ошибка: неверный формат строки!";
                                break;
                            }
                            byte[] ba = Encoding.Default.GetBytes(last.Substring(2, last.Length - 3));
                            this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), BitConverter.ToString(ba).Replace("-", "").Length.ToString("X2"), BitConverter.ToString(ba).Replace("-", "") });
                        }
                        else
                        {
                            if (!check_quotes(line[2]))
                            {
                                this.error = "(" + get_str_counter() + ") Ошибка: неверный формат строки!";
                                break;
                            }
                            byte[] ba = Encoding.Default.GetBytes(line[2].Substring(2, line[2].Length - 3));
                            this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), BitConverter.ToString(ba).Replace("-", "").Length.ToString("X2"), BitConverter.ToString(ba).Replace("-", "") });
                        }
                        add = (line[2].Length - 3);
                    }
                    else
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: неверный формат директивы!";
                        break;
                    }
                    this.address_counter += add;
                    break;
                case "END":
                    if (check_name_table() != "")
                    {
                        this.error = "Ошибка: не определены следующие метки -> " + check_name_table();
                        break;
                    }
                    this.last_address = this.address_counter - this.begin_address;
                    if (line[0] != "_")
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: директиве END не нужна метка!";
                        break;
                    }
                    if (line.Length == 2)
                    {
                        this.final_table.Add(new string[] { "E", this.begin_address.ToString("X6") });
                        this.final_table[0][3] = this.last_address.ToString("X6");
                        break;
                    }
                    if (!check_hex(line[2]))
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: неправильный формат адреса в директиве END!";
                        break;
                    }
                    if (line[2].Length > 6)
                    {
                        this.error = "(" + get_str_counter() + ") Ошибка: переполнение адреса в директиве END!";
                        break;
                    }
                    if (Convert.ToInt32(line[2], 16) != this.begin_address)
                    {
                        if (Convert.ToInt32(line[2], 16) < this.begin_address)
                        {
                            this.error = "(" + get_str_counter() + ") Ошибка: неправильный адрес точки входа!";
                            break;
                        }
                        if (Convert.ToInt32(line[2], 16) > (this.last_address + this.begin_address))
                        {
                            this.error = "(" + get_str_counter() + ") Ошибка: неправильный адрес точки входа!";
                            break;
                        }
                    }
                    // лишние END

                    this.final_table[0][3] = this.last_address.ToString("X6");
                    this.final_table.Add(new string[] { "E", line[2] });
                    break;
                default:
                    // проблема с ДЛИНОЙ КОМАНД и NOPE

                    // сразу проверка на наличие команды
                    if (line[0] != "_")
                    {
                        if (!check_str(line[0]))
                        {
                            this.error = "(" + get_str_counter() + ") Ошибка: некорректное имя метки!";
                            break;
                        }

                        if (this.registers.Contains(line[0]))
                        {
                            this.error = "(" + get_str_counter() + ") Ошибка: имя метки зарезирвированно!";
                            break;
                        }

                        // улучшенная проверка метки, есть ли у нее адрес
                        set_mark_address(line[0]);
                        this.to_update = true;
                        if (line.Length == 3)
                        {
                            int[] helper1 = get_command_code(line);
                            string addr = get_mark_address(line[2]);
                            if (addr != "" && addr != "mfdoom")
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), (helper1[0].ToString("X2").Length + addr.Length).ToString("X2"), helper1[0].ToString("X2"), addr });
                                this.str_counter += 1;
                            }
                                
                            else if (addr == "mfdoom")
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "?", helper1[0].ToString("X2"), "?" });
                                this.name_table.Add(new string[] { line[2], "", this.address_counter.ToString("X6") });
                                this.str_counter += 1;
                            }
                            else
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "?", helper1[0].ToString("X2"), "?" });
                                add_waiter(line[2]);
                                this.str_counter += 1;
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
                                this.str_counter += 1;
                            }
                            else
                            {
                                this.error = "(" + get_str_counter() + ") Ошибка: неверный формат команды!";
                                break;
                            }
                        }
                        else
                        {
                            this.error = "(" + get_str_counter() + ") Ошибка: неверный формат команды!";
                            break;
                        }
                    }
                    else
                    {
                        if (line.Length == 3)
                        {
                            int[] helper1 = get_command_code(line);
                            string addr = get_mark_address(line[2]);
                            
                            if (addr != "" && addr != "mfdoom")
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), (helper1[0].ToString("X2").Length + addr.Length).ToString("X2"), helper1[0].ToString("X2"), addr });
                                this.str_counter += 1;
                            }
                                
                            else if (addr == "mfdoom")
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "?", helper1[0].ToString("X2"), "?" });
                                this.name_table.Add(new string[] { line[2], "", this.address_counter.ToString("X6") });
                                this.str_counter += 1;
                            }
                            else
                            {
                                this.final_table.Add(new string[] { "T", this.address_counter.ToString("X6"), "?", helper1[0].ToString("X2"), "?" });
                                add_waiter(line[2]);
                                this.str_counter += 1;
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
                                this.str_counter += 1;
                            }
                            else
                            {
                                this.error = "(" + get_str_counter() + ") Ошибка: неверный формат команды!";
                                break;
                            }
                        }
                        else
                        {
                            this.error = "(" + get_str_counter() + ") Ошибка: неверный формат команды!";
                            break;
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
                if (this.error != "")
                    break;
            }
        }
    }
}
