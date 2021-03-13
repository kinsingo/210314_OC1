using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PNC_Csharp.Measurement_QA
{
    class PNC_Scirp
    {
        public static void Script_Transform(TextBox TextBox_Mipi_Script_Condition, TextBox TextBox_Show_Compared_Mipi_Data)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string temp_Mipi_Data_String = string.Empty;
            int count_mipi_cmd = 0;
            int count_one_mipi_cmd_length = 0;
            bool Flag = false;

            TextBox_Show_Compared_Mipi_Data.Clear();

            //Delete others except for Mipi CMDs and Write on the 2nd Textbox
            for (int i = 0; i < TextBox_Mipi_Script_Condition.Lines.Length; i++)
            {
                if (TextBox_Mipi_Script_Condition.Lines[i].Length >= 20) // mipi.write 0xXX 0xXX <-- 20ea Character
                {
                    if (TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 10) == "mipi.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 10; k < TextBox_Mipi_Script_Condition.Lines[i].Length; k++)
                        {
                            if (TextBox_Mipi_Script_Condition.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && TextBox_Mipi_Script_Condition.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        TextBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }
                    else if (TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 14) == "gpio.i2c.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 14; k < TextBox_Mipi_Script_Condition.Lines[i].Length; k++)
                        {
                            if (TextBox_Mipi_Script_Condition.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && TextBox_Mipi_Script_Condition.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        TextBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else
                    {
                        // It's not a "mipi.write" of "delay" command , do nothing 
                    }
                }

                //Delay
                else if (TextBox_Mipi_Script_Condition.Lines[i].Length >= 5
                    && TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5) != "     ")
                {
                    if (TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5) == "delay")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < TextBox_Mipi_Script_Condition.Lines[i].Length; k++)
                        {
                            if (TextBox_Mipi_Script_Condition.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && TextBox_Mipi_Script_Condition.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        TextBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else if (TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5) == "image")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < TextBox_Mipi_Script_Condition.Lines[i].Length; k++)
                        {
                            if (TextBox_Mipi_Script_Condition.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && TextBox_Mipi_Script_Condition.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = TextBox_Mipi_Script_Condition.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        TextBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }
                }
            }
        }
    }

    
}
