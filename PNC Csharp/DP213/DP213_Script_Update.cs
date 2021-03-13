using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public class DP213_Script_Update : DP213_forms_accessor
    {
        private TextBox textBox_Mipi_Script_Set1;
        private TextBox textBox_Mipi_Script_Set2;
        private TextBox textBox_Mipi_Script_Set3;
        private TextBox textBox_Mipi_Script_Set4;
        private TextBox textBox_Mipi_Script_Set5;
        private TextBox textBox_Mipi_Script_Set6;

        private TextBox textBox_Mipi_Script_OD;



        //Singleton
        static private DP213_Script_Update instance = null;
        static public DP213_Script_Update getInstance()
        {
            if (instance == null) instance = new DP213_Script_Update();
            return instance;
        }
        private DP213_Script_Update()
        {
            Allocate_Memories_For_Member_Textboxes();
            Update_Set123456_Script();
            Update_OD_Script();
        }

        private void Allocate_Memories_For_Member_Textboxes()
        {
            textBox_Mipi_Script_Set1 = new TextBox();
            textBox_Mipi_Script_Set2 = new TextBox();
            textBox_Mipi_Script_Set3 = new TextBox();
            textBox_Mipi_Script_Set4 = new TextBox();
            textBox_Mipi_Script_Set5 = new TextBox();
            textBox_Mipi_Script_Set6 = new TextBox();

            textBox_Mipi_Script_OD = new TextBox();
        }

        private void Update_Set123456_Script()
        {
            string filepath_1 = string.Empty;
            string filepath_2 = string.Empty;
            string filepath_3 = string.Empty;
            string filepath_4 = string.Empty;
            string filepath_5 = string.Empty;
            string filepath_6 = string.Empty;
            f1().Get_Set123456_txt_Path(ref filepath_1, ref filepath_2, ref filepath_3, ref filepath_4, ref filepath_5, ref filepath_6);

            textBox_Mipi_Script_Set1.Text = File.ReadAllText(filepath_1);
            textBox_Mipi_Script_Set2.Text = File.ReadAllText(filepath_2);
            textBox_Mipi_Script_Set3.Text = File.ReadAllText(filepath_3);
            textBox_Mipi_Script_Set4.Text = File.ReadAllText(filepath_4);
            textBox_Mipi_Script_Set5.Text = File.ReadAllText(filepath_5);
            textBox_Mipi_Script_Set6.Text = File.ReadAllText(filepath_6);

            Set_Condition_Mipi_Script_Change(Gamma_Set.Set1);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set2);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set3);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set4);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set5);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set6);
        }


        private void Update_OD_Script()
        {
            string filepath_OD = Directory.GetCurrentDirectory() + "\\DP213\\Script_OD_LUT_Data.txt";

            textBox_Mipi_Script_OD.Text = File.ReadAllText(filepath_OD);

            dp213_form().textBox_OD_Script.Text = Get_To_Be_Adjusted_Mipi_Script(textBox_Mipi_Script_OD).Text;

        }

        private void Set_Condition_Mipi_Script_Change(Gamma_Set Set)
        {
            TextBox Before_Adjust_Textbox = Get_Current_Before_Adjust_textbox(Set);
            TextBox To_Be_Adjusted_Textbox = Get_To_Be_Adjusted_Mipi_Script(Before_Adjust_Textbox);
            Set_To_Be_Adjusted_textbox(Set, To_Be_Adjusted_Textbox);
        }

        private TextBox Get_Current_Before_Adjust_textbox(Gamma_Set Set)
        {
            if (Set == Gamma_Set.Set1)
                return textBox_Mipi_Script_Set1;
            else if (Set == Gamma_Set.Set2)
                return textBox_Mipi_Script_Set2;
            else if (Set == Gamma_Set.Set3)
                return textBox_Mipi_Script_Set3;
            else if (Set == Gamma_Set.Set4)
                return textBox_Mipi_Script_Set4;
            else if (Set == Gamma_Set.Set5)
                return textBox_Mipi_Script_Set5;
            else if (Set == Gamma_Set.Set6)
                return textBox_Mipi_Script_Set6;
            else
                return null;
        }

        private void Set_To_Be_Adjusted_textbox(Gamma_Set Set, TextBox To_Be_Adjust_textbox)
        {
            if (Set == Gamma_Set.Set1)
                dp213_form().textBox_Show_Compared_Mipi_Data_Set1.Text = To_Be_Adjust_textbox.Text;
            else if (Set == Gamma_Set.Set2)
                dp213_form().textBox_Show_Compared_Mipi_Data_Set2.Text = To_Be_Adjust_textbox.Text;
            else if (Set == Gamma_Set.Set3)
                dp213_form().textBox_Show_Compared_Mipi_Data_Set3.Text = To_Be_Adjust_textbox.Text;
            else if (Set == Gamma_Set.Set4)
                dp213_form().textBox_Show_Compared_Mipi_Data_Set4.Text = To_Be_Adjust_textbox.Text;
            else if (Set == Gamma_Set.Set5)
                dp213_form().textBox_Show_Compared_Mipi_Data_Set5.Text = To_Be_Adjust_textbox.Text;
            else if (Set == Gamma_Set.Set6)
                dp213_form().textBox_Show_Compared_Mipi_Data_Set6.Text = To_Be_Adjust_textbox.Text;
        }

        private TextBox Get_To_Be_Adjusted_Mipi_Script(TextBox Before_Adjust_Textbox)
        {
            TextBox To_Be_Adjusted_Textbox = new TextBox();
            
            string temp_Mipi_Data_String = string.Empty;
            int count_mipi_cmd = 0;
            int count_one_mipi_cmd_length = 0;
            bool Flag = false;

            //Delete others except for Mipi CMDs and Write on the 2nd Textbox
            for (int i = 0; i < Before_Adjust_Textbox.Lines.Length; i++)
            {
                if (Before_Adjust_Textbox.Lines[i].Length >= 20) // mipi.write 0xXX 0xXX <-- 20ea Character
                {
                    if (Before_Adjust_Textbox.Lines[i].Substring(0, 10) == "mipi.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 10; k < Before_Adjust_Textbox.Lines[i].Length; k++)
                        {
                            if (Before_Adjust_Textbox.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && Before_Adjust_Textbox.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        To_Be_Adjusted_Textbox.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else
                    {
                        // It's not a "mipi.write" of "delay" command , do nothing 
                    }
                }

                //Delay
                else if (Before_Adjust_Textbox.Lines[i].Length >= 5
                    && Before_Adjust_Textbox.Lines[i].Substring(0, 5) != "     ")
                {
                    if (Before_Adjust_Textbox.Lines[i].Substring(0, 5) == "delay")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < Before_Adjust_Textbox.Lines[i].Length; k++)
                        {
                            if (Before_Adjust_Textbox.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && Before_Adjust_Textbox.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        To_Be_Adjusted_Textbox.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else if (Before_Adjust_Textbox.Lines[i].Substring(0, 5) == "image")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < Before_Adjust_Textbox.Lines[i].Length; k++)
                        {
                            if (Before_Adjust_Textbox.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && Before_Adjust_Textbox.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = Before_Adjust_Textbox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        To_Be_Adjusted_Textbox.Text += temp_Mipi_Data_String + "\r\n";
                    }

                }
            }

            return To_Be_Adjusted_Textbox;
        }


    }
}
