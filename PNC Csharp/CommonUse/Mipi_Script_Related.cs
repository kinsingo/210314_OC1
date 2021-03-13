using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace PNC_Csharp
{
    enum D_IC
    {
        Normal,
        SIW,
        Magna,
        Novatec,
    }

    enum Vendor
    {
        Guil,
        Doowon,
    }

    interface Mipi_Script
    {
        void Transfrom_Mipi_Cript(TextBox Original_Script, TextBox Changed_Script,bool Include_Delay = false);
        void Read_and_Compare(ref bool OTP_Read_And_Compare_Stop, TextBox Changed_Script, RichTextBox Compared_Result_Script, D_IC d_ic);
        void Convert_mipi_Script_For_OC_or_FI_Machine(TextBox Changed_Script, RichTextBox Converted_Script, Vendor vendor);
    }

    
    class Mipi_Script_Related : Mipi_Script
    {
        public void Transfrom_Mipi_Cript(TextBox Original_Script,TextBox Changed_Script, bool Include_Delay = false)
        {
            TextBox textBox_Mipi_Data_To_Be_Compared = Original_Script;
            TextBox textBox_Show_Compared_Mipi_Data = Changed_Script;
            List<string> Temp = new List<string>();

            for (int i = 0; i < textBox_Mipi_Data_To_Be_Compared.Lines.Length; i++)
            {
                string temp = textBox_Mipi_Data_To_Be_Compared.Lines[i];
                Temp.Add(temp.Trim());
            }
            textBox_Mipi_Data_To_Be_Compared.Clear();
            foreach (string temp in Temp) textBox_Mipi_Data_To_Be_Compared.Text += (temp + "\r\n");

            //Step1 (1st to 2nd (Transform))
            string temp_Mipi_Data_String = string.Empty;
            int count_one_mipi_cmd_length = 0;
            for (int i = 0; i < textBox_Mipi_Data_To_Be_Compared.Lines.Length; i++)
            {
                string Current_Line = textBox_Mipi_Data_To_Be_Compared.Lines[i];
                bool Is_delay_CMD = false;

                if ((Current_Line.Length >= 6)
               && (Current_Line.Substring(0, 5).ToLower() == "delay")
               && (textBox_Show_Compared_Mipi_Data.Lines.Length > 0))
                {
                    if (Include_Delay)
                    {
                        Is_delay_CMD = true;
                        textBox_Show_Compared_Mipi_Data.Text += "\r\n";
                    }
                    else
                    {
                        continue;
                    }
                }
               
                if (Is_delay_CMD == false)
                {
                    if ((Current_Line.Length >= 11)
                        && (Current_Line.Substring(0, 10).ToLower() == "mipi.write")
                        && (textBox_Show_Compared_Mipi_Data.Lines.Length > 0)) textBox_Show_Compared_Mipi_Data.Text += "\r\n";
                }

                count_one_mipi_cmd_length = 0;
                for (int k = 0; k < Current_Line.Length; k++)
                {
                    if (Current_Line[k] != '#') count_one_mipi_cmd_length++;
                    else break;
                }

                temp_Mipi_Data_String = Current_Line.Substring(0, count_one_mipi_cmd_length);
                temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\\", "");
                temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");

                textBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String.ToLower();
                textBox_Show_Compared_Mipi_Data.Text = System.Text.RegularExpressions.Regex.Replace(textBox_Show_Compared_Mipi_Data.Text, @"  ", " ");
            }
        }

        private void SetrichTextBox_Mipi_Finally_Compared(String A, Color color, RichTextBox Compared_Result_Script)
        {
            RichTextBox richTextBox_Mipi_Finally_Compared = Compared_Result_Script;
            richTextBox_Mipi_Finally_Compared.SelectionColor = color;
            richTextBox_Mipi_Finally_Compared.AppendText(A + "\r\n");
            richTextBox_Mipi_Finally_Compared.SelectionStart = richTextBox_Mipi_Finally_Compared.Text.Length;
            richTextBox_Mipi_Finally_Compared.ScrollToCaret();
        }


        private bool Is_D_IC_Specific_Process_Performed(string textBox_Show_Compared_Mipi_Data_Line, RichTextBox Compared_Result_Script, D_IC d_ic,ref string annotation)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            RichTextBox richTextBox_Mipi_Finally_Compared = Compared_Result_Script;
            
            if (d_ic == D_IC.Normal) 
            {
                return false;
            }
            else if (d_ic == D_IC.Magna)
            {
                if (textBox_Show_Compared_Mipi_Data_Line.Substring(0, 20).ToLower() == "mipi.write 0x15 0xb0"
                    || textBox_Show_Compared_Mipi_Data_Line.Substring(0, 20).ToLower() == "mipi.write 0x39 0xb0")
                {
                    f1.IPC_Quick_Send_And_Show(textBox_Show_Compared_Mipi_Data_Line, Color.Blue);
                    SetrichTextBox_Mipi_Finally_Compared(textBox_Show_Compared_Mipi_Data_Line.ToLower(), Color.Blue, richTextBox_Mipi_Finally_Compared);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (d_ic == D_IC.Novatec)
            {
                string CMD_Page_String = string.Empty;
                if (textBox_Show_Compared_Mipi_Data_Line.Substring(0, 20).ToLower() == "mipi.write 0x15 0xff"
                    || textBox_Show_Compared_Mipi_Data_Line.Substring(0, 20).ToLower() == "mipi.write 0x39 0xff")
                {
                    if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x10") CMD_Page_String = "(CMD1)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x20") CMD_Page_String = "(CMD2_P0)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x21") CMD_Page_String = "(CMD2_P1)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x22") CMD_Page_String = "(CMD2_P2)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x23") CMD_Page_String = "(CMD2_P3)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x24") CMD_Page_String = "(CMD2_P4)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x25") CMD_Page_String = "(CMD2_P5)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x26") CMD_Page_String = "(CMD2_P6)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x27") CMD_Page_String = "(CMD2_P7)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x28") CMD_Page_String = "(CMD2_P8)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x29") CMD_Page_String = "(CMD2_P9)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x2a") CMD_Page_String = "(CMD2_P10)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x2b") CMD_Page_String = "(CMD2_P11)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x2c") CMD_Page_String = "(CMD2_P12)";
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0xe0") CMD_Page_String = "(CMD2_PE0)";
                    else { }

                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data_Line.Substring(0, 25).ToLower());
                    SetrichTextBox_Mipi_Finally_Compared(textBox_Show_Compared_Mipi_Data_Line.Substring(0, 25).ToLower(), Color.Blue, richTextBox_Mipi_Finally_Compared);

                    annotation = CMD_Page_String;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (d_ic == D_IC.SIW)
            {
                if (textBox_Show_Compared_Mipi_Data_Line.Substring(0, 20).ToLower() == "mipi.write 0x15 0xb0"
                    || textBox_Show_Compared_Mipi_Data_Line.Substring(0, 20).ToLower() == "mipi.write 0x39 0xb0")
                {
                    if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0x55")
                    {
                        f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0x55", Color.Blue);
                        SetrichTextBox_Mipi_Finally_Compared("mipi.write 0x15 0xB0 0x55".ToLower(), Color.Blue, richTextBox_Mipi_Finally_Compared);
                    }
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0xac")
                    {
                        f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0xAC", Color.Blue);
                        SetrichTextBox_Mipi_Finally_Compared("mipi.write 0x15 0xB0 0xAC".ToLower(), Color.Blue, richTextBox_Mipi_Finally_Compared);
                    }
                    else if (textBox_Show_Compared_Mipi_Data_Line.Substring(21, 4).ToLower() == "0xca")
                    {
                        f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0xCA", Color.Blue);
                        SetrichTextBox_Mipi_Finally_Compared("mipi.write 0x15 0xB0 0xCA".ToLower(), Color.Blue, richTextBox_Mipi_Finally_Compared);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("D_IC Selection is not made");
                return false;
            }
        }

        public void Read_and_Compare(ref bool OTP_Read_And_Compare_Stop, TextBox Changed_Script, RichTextBox Compared_Result_Script,D_IC d_ic)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            TextBox textBox_Show_Compared_Mipi_Data = Changed_Script;
            RichTextBox richTextBox_Mipi_Finally_Compared = Compared_Result_Script;

            f1.Set_GB_ProgressBar_Maximum(textBox_Show_Compared_Mipi_Data.Lines.Length - 1);
            f1.Set_GB_ProgressBar_Step(1);
            f1.Set_GB_ProgressBar_Value(0);

            int count_mipi_parameter = 0;
            string Register_Address = string.Empty;
            string All_NG_Register = string.Empty;
            string annotation = string.Empty;

            //Comperare 2nd Textbox and Mipi Data read from OTP
            for (int i = 0; i < textBox_Show_Compared_Mipi_Data.Lines.Length; i++)
            {
                if (OTP_Read_And_Compare_Stop) break;

                f1.GB_ProgressBar_PerformStep();
                System.Windows.Forms.Application.DoEvents();
                
                if (Is_D_IC_Specific_Process_Performed(textBox_Show_Compared_Mipi_Data.Lines[i], richTextBox_Mipi_Finally_Compared, d_ic, ref annotation))
                {
                    //if(Is_D_IC_Specific_Process_Performed(...) do sth then return true, otherwise return false
                }
                else
                {
                    if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 10).ToLower() == "mipi.write")
                    {
                        //Get Address
                        Register_Address = textBox_Show_Compared_Mipi_Data.Lines[i].Substring(18, 2);

                        //Get Parameter Amount
                        if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x15")
                        {
                            count_mipi_parameter = 1;
                        }
                        else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x39")
                        {
                            count_mipi_parameter = (textBox_Show_Compared_Mipi_Data.Lines[i].Length - 20) / 5;
                        }
                        else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x05")
                        {
                            count_mipi_parameter = 0;
                        }
                        else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x0a") //PPA
                        {
                            count_mipi_parameter = 0; //PPA 는 원래 Read 불가한거 , 어짜피 다른값 읽힘 
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Error , it should be 0x05 , 0x15 or 0x39");
                        }

                        //Read Data From OTP
                        try
                        {
                            f1.OTP_Read(count_mipi_parameter, Register_Address);

                            string temp_string = string.Empty;

                            if (count_mipi_parameter == 0)
                            {
                                if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x0a") //if PPA
                                {
                                    f1.GB_Status_AppendText_Nextline("Cannot Read PPA(mipi.write 0x0A..) Setting , Skip", Color.Red);
                                }

                                else
                                    temp_string = "mipi.write 0x05 0x" + Register_Address;

                            }
                            else if (count_mipi_parameter == 1) // <-- 1 일때는 0x39 일수도 있음 !!
                            {
                                if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x15")
                                    temp_string = "mipi.write 0x15 0x" + Register_Address + f1.Get_Textbox_cmd2().Substring(16, 5 + 1);
                                else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x39")
                                    temp_string = "mipi.write 0x39 0x" + Register_Address + f1.Get_Textbox_cmd2().Substring(16, 5 + 1);
                            }
                            else if (count_mipi_parameter < 10)
                            {
                                temp_string = "mipi.write 0x39 0x" + Register_Address + f1.Get_Textbox_cmd2().Substring(16, 5 * count_mipi_parameter + 1);
                            }
                            else if (count_mipi_parameter < 100)
                            {
                                temp_string = "mipi.write 0x39 0x" + Register_Address + f1.Get_Textbox_cmd2().Substring(17, 5 * count_mipi_parameter + 1);
                            }
                            else
                            {
                                temp_string = "mipi.write 0x39 0x" + Register_Address + f1.Get_Textbox_cmd2().Substring(18, 5 * count_mipi_parameter + 1);
                            }

                            richTextBox_Mipi_Finally_Compared.Select(richTextBox_Mipi_Finally_Compared.TextLength, 0);

                            if (textBox_Show_Compared_Mipi_Data.Lines[i].Length > temp_string.Length)// 주석처리가 어디에 되어있냐에 따라 length 다르기에
                            {
                                temp_string = temp_string.PadRight(textBox_Show_Compared_Mipi_Data.Lines[i].Length, ' ');
                            }
                            else if ((textBox_Show_Compared_Mipi_Data.Lines[i].Length < temp_string.Length))
                            {
                                int temp = temp_string.Length - textBox_Show_Compared_Mipi_Data.Lines[i].Length;
                                temp_string = temp_string.Remove(textBox_Show_Compared_Mipi_Data.Lines[i].Length, temp);
                            }

                            if (textBox_Show_Compared_Mipi_Data.Lines[i].ToLower() == temp_string.ToLower())
                            {
                                SetrichTextBox_Mipi_Finally_Compared(temp_string.ToLower(), Color.Green, richTextBox_Mipi_Finally_Compared);
                                f1.GB_Status_AppendText_Nextline(" " + Register_Address + annotation + " OK", Color.Green);
                            }
                            else
                            {
                                SetrichTextBox_Mipi_Finally_Compared(temp_string.ToLower(), Color.Red, richTextBox_Mipi_Finally_Compared);
                                f1.GB_Status_AppendText_Nextline(" " + Register_Address + annotation + " NG", Color.Red);
                                All_NG_Register += (Register_Address + annotation + "\r\n");
                            }
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("OTP Read fail !\nPlease check the Sample or System-connection status");
                        }
                    }
                    else
                    {
                        // It's not a "mipi.write" command , do nothing 
                    }
                }
            }

            if (All_NG_Register != string.Empty)
            {
                f1.GB_Status_AppendText_Nextline("==========OTP Read NG List==========", Color.Red);
                f1.GB_Status_AppendText_Nextline(All_NG_Register, Color.Red);
            }
            else f1.GB_Status_AppendText_Nextline("OTP Read and Compare OK", Color.Green);
        }

        public void Convert_mipi_Script_For_OC_or_FI_Machine(TextBox Changed_Script, RichTextBox Converted_Script,Vendor vendor)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            TextBox textBox_Show_Compared_Mipi_Data = Changed_Script;
            RichTextBox richTextBox_Mipi_Converted = Converted_Script;
            int count_mipi_parameter = 0;
            
            //Comperare 2nd Textbox and Mipi Data read from OTP
            for (int i = 0; i < textBox_Show_Compared_Mipi_Data.Lines.Length; i++)
            {
                f1.GB_ProgressBar_PerformStep();
                System.Windows.Forms.Application.DoEvents();
                SetrichTextBox_Mipi_Finally_Compared("//" + textBox_Show_Compared_Mipi_Data.Lines[i], Color.Gray, richTextBox_Mipi_Converted);

                if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5).ToLower() == "delay")
                {
                    string CMD1 = "f_DELAY_MS->(" + textBox_Show_Compared_Mipi_Data.Lines[i].Substring(6, 2) + ");";
                    SetrichTextBox_Mipi_Finally_Compared(CMD1, Color.White, richTextBox_Mipi_Converted);
                }
                else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 10).ToLower() == "mipi.write")
                {
                    //Get Parameter Amount
                    if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x15")
                    {
                        count_mipi_parameter = 2;
                    }
                    else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x39" || textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x0a")
                    {
                        count_mipi_parameter = (textBox_Show_Compared_Mipi_Data.Lines[i].Length - 15) / 5;
                    }
                    else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(11, 4).ToLower() == "0x05")
                    {
                        count_mipi_parameter = 1;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Error , it should be 0x05 , 0x15 ,0x39 or 0x0A");
                    }

                    if (vendor == Vendor.Doowon)
                    {
                        string CMD1 = "f_CPU_WRITE->(ALL,1,0x50," + count_mipi_parameter.ToString() + ",0x00);";
                        SetrichTextBox_Mipi_Finally_Compared(CMD1, Color.White, richTextBox_Mipi_Converted);

                        string CMD2 = "f_CPU_WRITE->(ALL,1,0x53" + textBox_Show_Compared_Mipi_Data.Lines[i].ToLower().Substring(15).Replace(" ", ",") + ");";
                        SetrichTextBox_Mipi_Finally_Compared(CMD2, Color.White, richTextBox_Mipi_Converted);
                    }
                    else if (vendor == Vendor.Guil)
                    {
                        //--------Process--------
                        string CMD1 = "f_REG_CMDW->(ALL,1,0x50,0x" + count_mipi_parameter.ToString("X2") + ",0x00);";
                        SetrichTextBox_Mipi_Finally_Compared(CMD1, Color.White, richTextBox_Mipi_Converted);

                        string CMD2 = "f_REG_CMDW->(ALL,1,0x53" + textBox_Show_Compared_Mipi_Data.Lines[i].ToLower().Substring(15).Replace(" ", ",") + ");";
                        SetrichTextBox_Mipi_Finally_Compared(CMD2, Color.White, richTextBox_Mipi_Converted);
                        //------------------------
                    }
                }
                else
                {
                    // It's not a "mipi.write" command , do nothing 
                }
            }
        }
    }
}
