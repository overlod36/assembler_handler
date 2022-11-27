using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class FinalChecker
    {
        private int begin_address;
        private int last_address;
        private int address_counter;
        private int check_ex;
        private string section_name;
        private List<string[]> code;
        private List<string[]> name_table;
        private List<string[]> add_table;
        private List<string[]> code_table;
        private List<string> m_table;
        private List<string[]> final_table;
        private string[] registers = { "R1", "R2", "R3", "R4", "R5", "R6", "R7", "R8", "R9", "R10", "R11", "R12", "R13", "R14", "R15" };
        private string[] p_c = { "START", "END", "RESW", "WORD", "RESB", "BYTE", "EXTREF", "EXTDEF", "CSECT" };
        private string name;
        private int code_length;
        private bool begin;
        private bool end;
        private int type;
        private string error;
        

        public FinalChecker(List<string[]> code_t_send)
        {
            this.code = new List<string[]>();
            this.name_table = new List<string[]>();
            this.add_table = new List<string[]>();
            this.m_table = new List<string>();
            this.code_table = code_t_send;
            this.error = "";
            this.begin = false;
            this.end = false;
            this.check_ex = 0;
        }

        public void set_type(int tp)
        {
            this.type = tp;
        }

        public List<string[]> get_name_t()
        {
            return this.name_table;
        }

        public List<string[]> get_add_t()
        {
            return this.add_table;
        }

        public List<string> get_m()
        {
            return this.m_table;
        }

        public List<string[]> get_final_t()
        {
            return this.final_table;
        }

        private bool check_spaces(string st)
        {
            int cnt = 0;
            foreach (char el in st)
            {
                if (el == '\'')
                {
                    cnt += 1;
                }
            }
            if (cnt >= 2)
            {
                return true;
            }
            return false;
        }

        private void print_arr(string[] array)
        {
            Console.WriteLine("[{0}]", string.Join(", ", array));
        }

        private string[] wizzard(string res)
        {
            int ct = 0, ct1 = 0;
            int index1 = 0, index2 = 0;
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] == '\'')
                {
                    ct += 1;
                }
            }

            for (int i=0; i < res.Length; i++)
            {
                if (res[i] == '\'' && ct1 == 0)
                {
                    index1 = i;
                    ct1 += 1;
                    continue;
                }
                if (res[i] == '\'' && ct1 == (ct - 1))
                {
                    index2 = i;
                    break;
                }
            }
            string[] rt = { res.Substring(0, index1), res.Substring(index1 + 1, res.Length - index1-2 ) };
            return rt;

        }


        public void set_code(string[] code_t_send)
        {
            string[] dg_nail, rest;
            string res;
            foreach (var str in code_t_send) // проверка на пустые строки + (< 2)
            {
                if (str.Length == 0)
                    continue;
                // проверка на пустые строки в самих передаваемых строках
                if (str[0] == ' ')
                {
                    string[] r1 = {"_"};
                    string[] r2 = str.Split().Skip(1).ToArray();
                    this.code.Add(r1.Concat(r2).ToArray());
                }
                else
                {
                    if (check_spaces(str))
                    {
                        dg_nail = wizzard(str);
                        rest = dg_nail[0].Split();
                        //print_arr(dg_nail);
                        rest[rest.Length - 1] = rest[rest.Length - 1] + "'" + dg_nail[dg_nail.Length - 1] + "'";
                        this.code.Add(rest);
                    }
                    else
                    {
                        this.code.Add(str.Split());
                    }
                    
                }
                
            }
        }

        private bool check_pc(string cmd)
        {
            if (this.p_c.Contains(cmd))
            {
                return true;
            }
            return false;
        }

        private bool check_command(string[] line)
        {
            //print_arr(line);
            if (!check_str(line[1]))
            {
                this.error = "Ошибка: некорректное имя команды/директивы!";
                return false;
            }
            /*if (line.Length == 3 && line[0] != "_" && mark_in(line[0]))
            {
                this.error = "Ошибка: метка " + line[0] + " уже есть в таблице символических имен!";
                return false;
            }*/
            if (line.Length == 3 && line[0] != "_" && !check_str(line[0]))
            {
                this.error = "Ошибка: некорректное имя метки!";
                return false;
            }
            if (this.registers.Contains(line[0]))
            {
                this.error = "Ошибка: имя метки зарезирвированно!";
                return false;
            }
            
            foreach (string[] st in this.code_table)
            {
                if (st[0] == (line[1] + " "))
                {
                    return true;
                }
                else if (st[0] == (line[1] + " " + line[2] + " "))
                {
                    return true;
                }
            }
            return false;
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

        private bool change_table_at(string mark, int addr, string name)
        {
            Console.WriteLine(mark);
            foreach (string[] str in this.name_table)
            {
                if (str[0] == mark && str[2] == name)
                {
                    if (str[3] == "ВС")
                    {
                        this.error = "Ошибка: определение внешней ссылки в секции!";
                        return false;
                    }
                    if (str[1] != " ")
                    {
                        this.error = "Ошибка: метка уже определена!";
                        return false;
                    }
                    str[1] = addr.ToString("X6");
                    return true;
                }
            }
            this.error = "Ошибка: внешнее имя не было определено!";
            return false;
        }

        private void pc_filler(string[] line)
        {
            int ex, add = 0;
            switch (line[1])
            {
                case "START":
                    if (begin)
                    {
                        this.error = "Ошибка: директива START уже имеется в коде!";
                        return;
                    }
                    else
                    {
                        this.name = line[0];

                        if (line.Length == 2)
                        {
                            this.error = "Ошибка: отсутствует загрузочный адрес!";
                            return;
                        }
                        
                        if (!check_hex(line[2]))
                        {
                            this.error = "Ошибка: неправильный формат адреса загрузки!";
                            return;
                        }

                        if (line[2] != "0")
                        {
                            this.error = "Ошибка: загрузочный адрес должен быть равен 0!";
                            return;
                        }

                        if (line[2].Length > 6)
                        {
                            this.error = "Ошибка: переполнение адреса загрузки!";
                            return;
                        }
                        this.check_ex = 1;
                        this.section_name = line[0];
                        this.address_counter = Convert.ToInt32(line[2], 16); // hex
                        this.begin_address = Convert.ToInt32(line[2], 16);
                        this.begin = true;
                        string[] to_at_st = { "START", line[1], address_counter.ToString("X6"), " " };
                        this.add_table.Add(to_at_st);
                    }
                    break;
                case "CSECT":
                    if (this.check_ex == 1)
                    {
                        this.error = "Ошибка: после объявления внешних имен и ссылок необходимо написать код!";
                        return;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "Ошибка: некорректное имя секции!";
                        return;
                    }
                    this.check_ex = 1;
                    this.section_name = line[0];
                    this.address_counter = Convert.ToInt32("0", 16); ; // hex
                    string[] to_at_ces = { this.address_counter.ToString("X6"), "CSECT", line[0], " " };
                    this.add_table.Add(to_at_ces);
                    break;
                case "EXTDEF":
                    if (this.check_ex == 0)
                    {
                        this.error = "Ошибка: объявление внешнего имени в коде!";
                        return;
                    }
                    if (!check_str(line[2]))
                    {
                        this.error = "Ошибка: некорректное имя метки!";
                        return;
                    }
                    string[] to_nt_def = { line[2], " ", this.section_name, "ВИ" };
                    this.name_table.Add(to_nt_def);
                    string[] to_at_def = { " ", "EXTDEF", line[2], " " };
                    this.add_table.Add(to_at_def);
                    break;
                case "EXTREF":
                    if (this.check_ex == 0)
                    {
                        this.error = "Ошибка: объявление внешней ссылки в коде!";
                        return;
                    }
                    // ???
                    string[] to_nt_ref = { line[2], " ", this.section_name, "ВС" };
                    this.name_table.Add(to_nt_ref);
                    string[] to_at_ref = { " ", "EXTREF", line[2], " " };
                    this.add_table.Add(to_at_ref);
                    break;
                case "RESB":
                    if (line[0] == "_")
                    {
                        this.error = "Ошибка: директива RESB без метки!";
                        return;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "Ошибка: некорректное имя метки!";
                        return;
                    }
                    else
                    {
                        if (!change_table_at(line[0], this.address_counter, this.section_name))
                        {
                            return;
                        }
                        if (!int.TryParse(line[2], out _))
                        {
                            this.error = "Ошибка: Введено не число!";
                            return;
                        }
                        string[] to_at_resb = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                        this.add_table.Add(to_at_resb);
                        this.address_counter += (Int32.Parse(line[2]));
                        if (this.check_ex == 1)
                        {
                            this.check_ex = 0;
                        }
                    }
                    break;
                case "RESW":
                    if (line[0] == "_")
                    {
                        this.error = "Ошибка: директива RESW без метки!";
                        return;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "Ошибка: некорректное имя метки!";
                        return;
                    }
                    else
                    {
                        if (!change_table_at(line[0], this.address_counter, this.section_name))
                        {
                            return;
                        }

                        if (!int.TryParse(line[2], out _))
                        {
                            this.error = "Ошибка: Введено не число!";
                            return;
                        }
                        string[] to_at_resw = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                        this.add_table.Add(to_at_resw);
                        this.address_counter += (Int32.Parse(line[2]) * 3);
                        if (this.check_ex == 1)
                        {
                            this.check_ex = 0;
                        }
                    }
                    break;
                case "WORD":
                    int ch;
                    if (line[0] == "_")
                    {
                        this.error = "Ошибка: директива WORD без метки!";
                        return;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "Ошибка: некорректное имя метки!";
                        return;
                    }
                    else
                    {
                        if (!change_table_at(line[0], this.address_counter, this.section_name))
                        {
                            return;
                        }
                        if (!int.TryParse(line[2], out ch))
                        {
                            this.error = "Ошибка: Введено не число!";
                            return;
                        }
                        else
                        {
                            if (ch > 16777215)
                            {
                                this.error = "Ошибка: введено число меньше 16777215!";
                                return;
                            }
                        }
                        string[] to_at_word = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                        this.add_table.Add(to_at_word);
                        this.address_counter += 3;
                        if (this.check_ex == 1)
                        {
                            this.check_ex = 0;
                        }
                    }
                    break;
                case "BYTE":
                    int tr;
                    if (line[0] == "_")
                    {
                        this.error = "Ошибка: директива BYTE без метки!";
                        return;
                    }
                    if (!check_str(line[0]))
                    {
                        this.error = "Ошибка: некорректное имя метки!";
                        return;
                    }
                    else
                    {
                        if (!change_table_at(line[0], this.address_counter, this.section_name))
                        {
                            return;
                        }
                        if (line[2][0] != 'c' && line[2][0] != 'x' && !int.TryParse(line[2], out _))
                        {
                            this.error = "Ошибка: неверный формат константы!";
                            return;
                        }

                        if (line[2][0] == 'c' && (line[2][1] != '\'' || line[2][line[2].Length-1] != '\''))
                        {
                            this.error = "Ошибка: неверный формат константы, нужны кавычки!";
                            return;
                        }

                        if (line[2][0] == 'x' && !check_hex(line[2].Substring(1)))
                        {
                            this.error = "Ошибка: неверный формат константы, введите 16-ричное число!";
                            return;
                        }

                        if (int.TryParse(line[2], out tr))
                        {
                            if (tr > 255)
                            {
                                this.error = "Ошибка: введите число меньше 256!";
                                return;
                            }
                        }

                        string[] to_at_byte = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                        this.add_table.Add(to_at_byte);

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
                        // else неправильная команда
                        this.address_counter += add;
                        if (this.check_ex == 1)
                        {
                            this.check_ex = 0;
                        }
                    }
                    break;
                case "END":
                    if (line[0] != "_")
                    {
                        this.error = "Ошибка: директиве END не нужна метка!";
                        return;
                    }

                    if(line.Length == 2)
                    {
                        string[] to_end = { this.address_counter.ToString("X6"), line[1], this.begin_address.ToString("X"), " " };
                        this.add_table.Add(to_end);
                        this.last_address = this.address_counter - this.begin_address;
                        this.end = true;
                        return;
                    }

                    if (!check_hex(line[2]))
                    {
                        this.error = "Ошибка: неправильный формат адреса в директиве END!";
                        return;
                    }

                    if (line[2].Length > 6)
                    {
                        this.error = "Ошибка: переполнение адреса в директиве END!";
                        return;
                    }


                    // проверка на начальный адрес
                    if (Convert.ToInt32(line[2], 16) != this.begin_address)
                    {
                        if (Convert.ToInt32(line[2], 16) < this.begin_address)
                        {
                            this.error = "Ошибка: неправильный адрес точки входа!";
                            return;
                        }
                    }


                    string[] to_at_end = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                    this.add_table.Add(to_at_end);
                    this.last_address = this.address_counter - this.begin_address;
                    this.end = true;
                    break;
                default:
                    this.error = "Ошибка: неправильный формат директивы!";
                    return;
            }
        }

        private bool check_for_address(string[] line)
        {
            if (this.registers.Contains(line[2]) && this.registers.Contains(line[3]))
            {
                return true;
            }
            return false;
        }

        private int[] get_command_code(string[] line)
        {
            int[] wr = { };
            foreach (string[] st in this.code_table)
            {
                if (st[0] == (line[1] + " "))
                {
                    if (line.Length == 4)
                    {
                        if (this.registers.Contains(line[2]) && this.registers.Contains(line[3])){
                            int[] res1 = { Int32.Parse(st[1]) * 4, Int32.Parse(st[2]) };
                            return res1;
                        }
                    }

                    //Console.WriteLine(line[2]);

                    

                    if (line[2][0] == '$' && this.type == 1 && !Int32.TryParse(line[2][1].ToString(), out _))
                    {
                        this.error = "Ошибка: относительная адресация при примере прямой адресации!";
                    }
                    if (line[2][0] != '$' && this.type == 2 && !Int32.TryParse(line[2][0].ToString(), out _))
                    {
                        this.error = "Ошибка: прямая адресация при примере относительной адресации!";
                    }
                    if (line[2][0] == '$')
                    {
                        int[] res2 = { Int32.Parse(st[1]) * 4 + 2, Int32.Parse(st[2]) };
                        return res2;
                    }
                    else
                    {
                        int[] res2 = { Int32.Parse(st[1]) * 4 + 1, Int32.Parse(st[2]) };
                        return res2;
                    }
                       
                }
                else if (st[0] == (line[1] + " " + line[2] + " "))
                {
                    int[] res3 = { Int32.Parse(st[1]) * 4 + 1, Int32.Parse(st[2]) };
                    return res3;
                }
            }
            return wr; // нет команды
        }

        private string get_dir_address(string dir)
        {
            foreach(string[] el in this.name_table)
            {
                if (el[0] == dir)
                {
                    return el[1];
                }
                if (dir[0] == '$' && dir.Substring(1) == el[0])
                {
                    return el[1];
                }
            }
            this.error = "Ошибка: " + dir + " отсутствует в таблице имен!";
            return ""; // ошибка
        }

        private void command_filler(string[] line)
        {
            if (line[0] != "_")
            {
                // проверка на наличие такой в таблице
                if (!change_table_at(line[0], this.address_counter, this.section_name))
                {
                    return;
                }
            }

            if (line.Length == 3)
            {
                int[] r1 = get_command_code(line);
                string[] to_at = { this.address_counter.ToString("X6"), r1[0].ToString("X2"), line[2], " " };
                this.add_table.Add(to_at);
                this.address_counter += (r1[1]);
            }
            else if (line.Length == 4 && check_for_address(line))
            {
                int[] r2 = get_command_code(line);
                string[] to_at = { this.address_counter.ToString("X6"), r2[0].ToString("X2"), line[2], line[3] };
                this.add_table.Add(to_at);
                this.address_counter += (r2[1]);
            }
            else
            {
                int[] r3 = get_command_code(line);
                string[] to_at = { this.address_counter.ToString("X6"), r3[0].ToString("X2"), line[3], " " };
                this.add_table.Add(to_at);
                this.address_counter += (r3[1]);
            }
            
        }

        public string first_cycle()
        {

            foreach (string[] el in this.code_table)
            {
                if (!check_str(el[0]) || !check_hex(el[1]) || !int.TryParse(el[2], out _))
                {
                    return "Ошибка: недопустимые значения в таблице кодов операций!";
                }
            }

            int i = 1;
            foreach(string[] str in this.code)
            {
                
                if (end)
                {
                    return "(" + i.ToString() + ") Ошибка: есть код после директивы END!";
                }

                if (str.Length > 4 || str.Length < 2)
                {
                    return "(" + i.ToString() + ") Ошибка: слишком большое количество команд/аргументов!";
                }

                if (check_pc(str[1]))
                {
                    pc_filler(str);
                }
                else if (check_command(str))
                {
                    if (this.check_ex == 1)
                        this.check_ex = 0;
                    command_filler(str);
                }
                //else
                //{
                  //  Console.WriteLine("BEE");
                  //  this.error = "Ошибка: неправильный формат команды/директивы!";
                //}
                
                // если проверить begin, и там ничего не будет, то ошибка
                if (this.error != "")
                {
                    return "(" + i.ToString() + ") " + this.error;
                }
                i += 1;
            }

            return "";
        }

        public string second_cycle()
        {
            int i = 1;
            this.final_table = new List<string[]>();
            string[] st = { "H", this.name, this.begin_address.ToString("X6"), this.last_address.ToString("X6") };
            this.final_table.Add(st);
            foreach (string[] str in this.add_table)
            {
                switch (str[1])
                {
                    case "RESB":
                        string[] at_resb = { "T", str[0], Convert.ToInt32(str[2]).ToString("X")};
                        this.final_table.Add(at_resb);
                        break;
                    case "RESW":
                        string[] at_resw = { "T", str[0], (Convert.ToInt32(str[2])*3).ToString("X") };
                        this.final_table.Add(at_resw);
                        break;
                    case "WORD":
                        string[] at_word = { "T", str[0], "06", (Convert.ToInt32(str[2])).ToString("X6")};
                        this.final_table.Add(at_word);
                        break;
                    case "END":
                        foreach (string el in this.m_table)
                        {
                            string[] m_stuff = { "M", el };
                            this.final_table.Add(m_stuff);
                        }
                        if (Convert.ToInt32(str[2], 16) > (this.last_address + this.begin_address))
                        {
                            //Console.WriteLine(code_length);
                            this.error = "Ошибка: неправильный адрес загрузки!";
                            break;
                        }
                        string[] last = { "E", str[2] };
                        this.final_table.Add(last);
                        break;
                    case "BYTE":
                        string[] at_byte = { "T", str[0], " | ", "BYTE"};
                        if (str[2][0] == 'c')
                        {
                            byte[] ba = Encoding.Default.GetBytes(str[2].Substring(2, str[2].Length - 3));
                            at_byte[3] = BitConverter.ToString(ba).Replace("-", "");
                            at_byte[2] = at_byte[3].Length.ToString("X2");
                        }
                        else if(str[2][0] == 'x')
                        {
                            at_byte[2] = str[2].Substring(1, str[2].Length - 1).Length.ToString("X2");
                            at_byte[3] = str[2].Substring(1, str[2].Length - 1);
                        }
                        else
                        {
                            at_byte[3] = Convert.ToInt32(str[2]).ToString("X");
                            at_byte[2] = at_byte[3].Length.ToString("X2");
                        }
                        this.final_table.Add(at_byte);
                        break;
                    default:
                        if (str.Length == 3)
                        {
                            string[] at_st = { "T", str[0], " | ", str[1], str[2] };
                            this.final_table.Add(at_st);
                        }
                        else
                        {
                            string[] at_st = { "T", str[0], " | ", str[1], str[2], str[3] };
                            if (this.registers.Contains(at_st[4]) && this.registers.Contains(at_st[5]))
                            {
                                at_st[4] = at_st[4][1].ToString();
                                at_st[5] = at_st[5][1].ToString();
                                at_st[2] = (at_st[3].Length + 2).ToString("X2");
                            }
                            else
                            {
                                if (str[2][0] == '$')
                                {
                                    if (Int32.TryParse(str[2][1].ToString(), out _)) {
                                        this.error = "Ошибка: неправильный формат метки!";
                                        break;
                                    }
                                    if (this.registers.Contains(str[2].Substring(1)))
                                    {
                                        this.error = "Ошибка: имя метки зарезервированно!";
                                        break;
                                    }
                                    if (get_dir_address(str[2]) == "")
                                    {
                                        this.error = "Ошибка: отсутствие метки в таблице символических имен!";
                                        break;
                                    }
                                    at_st[4] = (Convert.ToInt32(get_dir_address(str[2]), 16) - Convert.ToInt32(this.add_table.SkipWhile(x => x != str).Skip(1).DefaultIfEmpty(add_table[0]).FirstOrDefault()[0], 16)).ToString("X6");
                                    if (at_st[4].Length > 6)
                                    {
                                        at_st[4] = at_st[4].Substring(2);
                                    }
                                }
                                else
                                {
                                    if (Int32.TryParse(str[2][0].ToString(), out _)) {
                                        this.error = "Ошибка: неправильный формат метки!";
                                        break;
                                    }
                                    if (this.registers.Contains(str[2]))
                                    {
                                        this.error = "Ошибка: имя метки зарезервированно!";
                                        break;
                                    }
                                    at_st[4] = get_dir_address(str[2]);
                                    this.m_table.Add(str[0]);
                                }
                                at_st[3] = str[1];
                                at_st[2] = (at_st[3].Length + at_st[4].Length).ToString("X2");
                            }
                            this.final_table.Add(at_st);
                        }
                        break;
                }
                if (this.error != "")
                {
                    return "(" + i.ToString() + ") " + this.error;
                }
                i += 1;
            }
            return "";
        }

    }
}
