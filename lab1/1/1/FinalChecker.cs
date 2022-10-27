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
        private List<string[]> code;
        private List<string[]> name_table;
        private List<string[]> add_table;
        private List<string[]> code_table;
        private List<string[]> final_table;
        private string[] registers = { "R1", "R2", "R3", "R4" };
        private string[] p_c = { "START", "END", "RESW", "WORD", "RESB", "BYTE" };
        private string name;
        private int code_length;
        private bool begin;

        public FinalChecker(List<string[]> code_t_send)
        {
            this.code = new List<string[]>();
            this.name_table = new List<string[]>();
            this.add_table = new List<string[]>();
            this.code_table = code_t_send;
            this.begin = false;
        }

        public List<string[]> get_name_t()
        {
            return this.name_table;
        }

        public List<string[]> get_add_t()
        {
            return this.add_table;
        }

        public List<string[]> get_final_t()
        {
            return this.final_table;
        }

        public void set_code(string[] code_t_send)
        {
            foreach (var str in code_t_send) // проверка на пустые строки + (< 2)
            {
                // проверка на пустые строки в самих передаваемых строках
                this.code.Add(str.Split());
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

        private void pc_filler(string[] line)
        {
            int ex, add = 0;
            switch (line[1])
            {
                case "START":
                    if (begin)
                    {
                        // ошибка, уже есть START
                    }
                    else
                    {
                        this.name = line[0];
                        // проверка адреса на переполнение и число
                        this.address_counter = Int32.Parse(line[2]); // hex
                        this.begin_address = Int32.Parse(line[2]);
                    }
                    break;
                case "RESB":
                    if (line[0] == "_")
                    {
                        // ошибка, директива без метки
                    }
                    else
                    {
                        // проверка на отсутствие такой же метки в таблице + число
                        string[] to_nt_resb = { line[0], this.address_counter.ToString("X6") };
                        this.name_table.Add(to_nt_resb);
                        string[] to_at_resb = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                        this.add_table.Add(to_at_resb);
                        this.address_counter += (Int32.Parse(line[2]));
                    }
                    break;
                case "RESW":
                    if (line[0] == "_")
                    {
                        // ошибка директивы
                    }
                    else
                    {
                        // проверка на отсутствие такой же метки в таблице + число
                        string[] to_nt_resw = { line[0], this.address_counter.ToString("X6") };
                        this.name_table.Add(to_nt_resw);
                        string[] to_at_resw = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                        this.add_table.Add(to_at_resw);
                        this.address_counter += (Int32.Parse(line[2]) * 3);
                    }
                    break;
                case "WORD":
                    if (line[0] == "_")
                    {
                        // ошибка директивы
                    }
                    else
                    {
                        // проверка на отсутствие такой же метки в таблице + надо решить с проверкой выделения слова
                        string[] to_nt_word = { line[0], this.address_counter.ToString("X6") };
                        this.name_table.Add(to_nt_word);
                        string[] to_at_word = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                        this.add_table.Add(to_at_word);
                        this.address_counter += 3;
                    }
                    break;
                case "BYTE":
                    if (line[0] == "_")
                    {
                        // ошибка директивы
                    }
                    else
                    {
                        // проверка на отсутствие такой же метки в таблице + если число - число
                        string[] to_nt_byte = { line[0], this.address_counter.ToString("X6") };
                        this.name_table.Add(to_nt_byte);
                        string[] to_at_byte = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                        this.add_table.Add(to_at_byte);

                        if (int.TryParse(line[2], out ex))
                        {
                            if (ex < 255)
                            {
                                add = 1;
                            }
                            else
                            {
                                add = 2;
                            }
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
                    }
                    break;
                case "END":
                    // если метка, то ошибка
                    // проверка на начальный адрес
                    string[] to_at_end = { this.address_counter.ToString("X6"), line[1], line[2], " " };
                    this.add_table.Add(to_at_end);
                    this.last_address = this.address_counter - this.begin_address;
                    // как-то кинуть основному циклу на конец
                    break;
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
                    int[] res2 = { Int32.Parse(st[1]) * 4 + 1, Int32.Parse(st[2]) };
                    return res2; // проверка таблицы в самом начале???
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
            }
            return ""; // ошибка
        }


        private void command_filler(string[] line)
        {
            if (line[0] != "_")
            {
                // проверка на наличие такой в таблице
                string[] to_nt = { line[0], this.address_counter.ToString("X6") };
                this.name_table.Add(to_nt);
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

        public void first_cycle()
        {
            foreach(string[] str in this.code)
            {
                if (check_pc(str[1]))
                {
                    pc_filler(str);
                }
                else if (check_command(str))
                {
                    command_filler(str);
                }
                else
                {
                    // ошибка
                }
                // если проверить begin, и там ничего не будет, то ошибка
            }
        }

        public void second_cycle()
        {
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
                        string[] last = { "E", begin_address.ToString("X6") };
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
                                at_st[3] = str[1];
                                at_st[4] = get_dir_address(str[2]);
                                at_st[2] = (at_st[3].Length + at_st[4].Length).ToString("X2");
                            }
                            this.final_table.Add(at_st);
                        }
                        break;
                }
            }
        }

    }
}
