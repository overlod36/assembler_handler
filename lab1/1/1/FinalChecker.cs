using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class FinalChecker
    {
        private int address_counter;
        private List<string[]> code;
        private List<string[]> name_table;
        private List<string[]> add_table;
        private List<string[]> code_table;
        private string[] registers = { "R1", "R2", "R3", "R4" };
        private string[] p_c = { "START", "END", "RESW", "WORD", "RESB", "BYTE" };
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
                        // проверка адреса на переполнение и число
                        this.address_counter = Int32.Parse(line[2]); // hex
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

    }
}
