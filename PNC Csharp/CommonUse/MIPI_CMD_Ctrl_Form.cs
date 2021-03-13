using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

namespace PNC_Csharp
{
    public partial class MIPI_CMD_Ctrl_Form : Form
    {
        bool stop = false;

        private static MIPI_CMD_Ctrl_Form Instance;
        public static MIPI_CMD_Ctrl_Form getInstance()
        {
            if (Instance == null)
                Instance = new MIPI_CMD_Ctrl_Form();

            return Instance;
        }

        public static bool IsIstanceNull()
        {
            if (Instance == null)
                return true;
            else
                return false;
        }

        public static void DeleteInstance()
        {
            Instance = null;
        }
        private MIPI_CMD_Ctrl_Form()
        {
            InitializeComponent();
        }

        private void MIPI_CMD_Ctrl_Form_Load(object sender, EventArgs e)
        {

        }
        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void radioButton_Min_to_Max_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Min_to_Max.Checked)
            {
                label_Direction.Text = "→";
            }
            else
            {
                label_Direction.Text = "←";
            }
        }

        private void radioButton_Max_to_Min_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_Min_to_Max_CheckedChanged(sender, e);
        }

        private void radioButton_Option_1ea_Param_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Option_1ea_Param.Checked)
            {
                textBox_Min_Second.Text = string.Empty;
                textBox_Min_Second.ReadOnly = true;

                textBox_Max_Second.Text = string.Empty;
                textBox_Max_Second.ReadOnly = true;

                textBox_Which_Param2.Text = string.Empty;
            }
            else
            {
                textBox_Min_Second.ReadOnly = false;
                textBox_Max_Second.ReadOnly = false;

                textBox_Min_Second.Text = "00";
                textBox_Max_Second.Text = "00";

                if (textBox_Which_Param1.Text == string.Empty)
                    textBox_Which_Param1.Text = "1";

                textBox_Which_Param2.Text = (Convert.ToInt16(textBox_Which_Param1.Text) + 1).ToString();
            }
        }

        private void radioButton_Option_2ea_Param_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_Option_1ea_Param_CheckedChanged(sender, e);
        }

        private void textBox_Which_Param1_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(textBox_Which_Param1.Text) <= 0)
            {
                textBox_Which_Param1.Text = "1";
            }

            if (radioButton_Option_2ea_Param.Checked)
            {
                textBox_Which_Param2.Text = (Convert.ToInt16(textBox_Which_Param1.Text) + 1).ToString();
            }
        }

        private void button_Change_Start_Click(object sender, EventArgs e)
        {
            stop = false;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string offset = "mipi.write 0x15 0xB0 0x" + textBox_Offset.Text;

                if (textBox_Mipi_CMD.Text.Length >= 20) // mipi.write 0xXX 0xXX 0xXX <-- 25ea Character
                {
                    int index_1st_Param = Convert.ToInt16(textBox_Which_Param1.Text);
                    int temp_index = 0;
                    string temp_string = string.Empty;
                    string To_Be_Changed = string.Empty;
                    string Remaining_string = string.Empty;
                    string Min = string.Empty;
                    string Max = string.Empty;
                    string step = textBox_Step.Text;
                    string delay = textBox_Delay.Text;
                    string mipi_cmd = string.Empty;
                    int count = 0;
                    textBox_count.Text = count.ToString();

                    //radioButton_Option_1ea_Param.Checked
                    if (radioButton_Option_1ea_Param.Checked)
                    {
                        //Set string temp_index,(To_Be_Changed),Remaining_string
                        temp_index = 20 + ((index_1st_Param - 1) * 5);
                        if (textBox_Mipi_CMD.Text.Length < temp_index + 5)
                        {
                            System.Windows.Forms.MessageBox.Show("Mipi CMD is too short(1)");
                            return;
                        }
                        temp_string = textBox_Mipi_CMD.Text.Substring(0, temp_index);
                        To_Be_Changed = textBox_Mipi_CMD.Text.Substring(temp_index, 5);
                        temp_index += 5;
                        Remaining_string = textBox_Mipi_CMD.Text.Substring(temp_index, textBox_Mipi_CMD.Text.Length - temp_index);


                        //Get Min , Max string
                        Min = this.textBox_Min_First.Text.PadLeft(2, '0');
                        Max = this.textBox_Max_First.Text.PadLeft(2, '0');
                        if (Convert.ToInt16(Min, 16) >= Convert.ToInt16(Max, 16))
                        {
                            System.Windows.Forms.MessageBox.Show("Min(" + Min + ") >= Max(" + Max + "), Please Set Min/Max Value again(1)");
                            return;
                        }

                        //Set string To_Be_Changed
                        if (radioButton_Min_to_Max.Checked) //Min to Max
                        {
                            //Min
                            To_Be_Changed = " 0x" + Min;
                            mipi_cmd = temp_string + To_Be_Changed + Remaining_string;

                        if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                        f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                            textBox_Mipi_CMD.Text = mipi_cmd;

                            textBox_count.Text = (++count).ToString();

                            int temp = Convert.ToInt16(Min,16);
                            while (true) //(Min,Max)
                            {
                                if (stop) return;
                                Thread.Sleep(Convert.ToInt16(delay));
                                if (checkBox_Measure_Between_CMD.Checked) f1.CA_Measure_BT_Click();
                                System.Windows.Forms.Application.DoEvents();
                          
                                temp += Convert.ToInt16(step,16);
                                if (temp >= Convert.ToInt16(Max, 16))
                                    break;
                                else
                                {
                                    To_Be_Changed = " 0x" + Convert.ToString(temp, 16).PadLeft(2,'0');
                                    mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                                if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                                f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                                    textBox_Mipi_CMD.Text = mipi_cmd;
                                    textBox_count.Text = (++count).ToString();
                                }
                                
                                
                            }
                            //Max
                            To_Be_Changed = " 0x" + Max;
                            mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                        if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                        f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                            textBox_Mipi_CMD.Text = mipi_cmd;
                            textBox_count.Text = (++count).ToString();
                            Thread.Sleep(Convert.ToInt16(delay));
                            if (checkBox_Measure_Between_CMD.Checked) f1.CA_Measure_BT_Click();
                            System.Windows.Forms.Application.DoEvents();
                        }
                        else  //Max to Min
                        {
                            //Max
                            To_Be_Changed = " 0x" + Max;
                            mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                        if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                        f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                            textBox_Mipi_CMD.Text = mipi_cmd;
                            textBox_count.Text = (++count).ToString();
                            int temp = Convert.ToInt16(Max, 16);
                            while (true) //(Max,Min)
                            {
                                if (stop) return;

                                Thread.Sleep(Convert.ToInt16(delay));
                                if (checkBox_Measure_Between_CMD.Checked) f1.CA_Measure_BT_Click();
                                System.Windows.Forms.Application.DoEvents();

                                temp -= Convert.ToInt16(step, 16);
                                if (temp <= Convert.ToInt16(Min, 16))
                                    break;
                                else
                                {
                                    To_Be_Changed = " 0x" + Convert.ToString(temp, 16).PadLeft(2, '0');
                                    mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                                if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                                f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                                    textBox_Mipi_CMD.Text = mipi_cmd;
                                    textBox_count.Text = (++count).ToString();
                                }
                            }
                            //Min
                            To_Be_Changed = " 0x" + Min;
                            mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                        if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                        f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                            textBox_Mipi_CMD.Text = mipi_cmd;
                            textBox_count.Text = (++count).ToString();
                            Thread.Sleep(Convert.ToInt16(delay));
                            if (checkBox_Measure_Between_CMD.Checked) f1.CA_Measure_BT_Click();
                            System.Windows.Forms.Application.DoEvents();
                        }
                    }

                    else //radioButton_Option_2ea_Param.Checked
                    {
                        temp_index = 20 + ((index_1st_Param - 1) * 5);
                        if (textBox_Mipi_CMD.Text.Length < temp_index + 10)
                        {
                            System.Windows.Forms.MessageBox.Show("Mipi CMD is too short(2)");
                            return;
                        }
                        temp_string = textBox_Mipi_CMD.Text.Substring(0, temp_index);
                        To_Be_Changed = textBox_Mipi_CMD.Text.Substring(temp_index, 10);
                        temp_index += 10;
                        Remaining_string = textBox_Mipi_CMD.Text.Substring(temp_index, textBox_Mipi_CMD.Text.Length - temp_index);

                        //Get Min , Max string
                        string Min_1st = this.textBox_Min_First.Text.PadLeft(2, '0');
                        string Min_2nd = this.textBox_Min_Second.Text.PadLeft(2, '0');
                        string Max_1st = this.textBox_Max_First.Text.PadLeft(2, '0');
                        string Max_2nd = this.textBox_Max_Second.Text.PadLeft(2, '0');

                        Min = Min_1st + Min_2nd;
                        Max = Max_1st + Max_2nd;

                        if (Convert.ToInt16(Min, 16) >= Convert.ToInt16(Max, 16))
                        {
                            System.Windows.Forms.MessageBox.Show("Min(" + Min + ") >= Max(" + Max + ") , Please Set Min/Max Value again(2)");
                            return;
                        }

                        if (radioButton_Min_to_Max.Checked) //Min to Max
                        {
                            //Min
                            To_Be_Changed = " 0x" + Min_1st + " 0x" + Min_2nd;
                            mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                        if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                        f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                            textBox_Mipi_CMD.Text = mipi_cmd;
                            textBox_count.Text = (++count).ToString();
                            int temp = Convert.ToInt16(Min, 16);
                            while (true) //(Min,Max)
                            {
                                if (stop) return;

                                Thread.Sleep(Convert.ToInt16(delay));
                                if (checkBox_Measure_Between_CMD.Checked) f1.CA_Measure_BT_Click();
                                System.Windows.Forms.Application.DoEvents();

                                temp += Convert.ToInt16(step, 16);
                                if (temp >= Convert.ToInt16(Max, 16))
                                    break;
                                else
                                {
                                    string Temp_String = Convert.ToString(temp, 16).PadLeft(4, '0');
                                    To_Be_Changed = " 0x" + Temp_String.Substring(0, 2) + " 0x" + Temp_String.Substring(2, 2);
                                    mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                                if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                                f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                                    textBox_Mipi_CMD.Text = mipi_cmd;
                                    textBox_count.Text = (++count).ToString();
                                }
                            }
                            //Max
                             To_Be_Changed = " 0x" + Max_1st + " 0x" + Max_2nd;
                             mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                        if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                        f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                             textBox_Mipi_CMD.Text = mipi_cmd;
                             textBox_count.Text = (++count).ToString();
                             Thread.Sleep(Convert.ToInt16(delay));
                             if (checkBox_Measure_Between_CMD.Checked) f1.CA_Measure_BT_Click();
                             System.Windows.Forms.Application.DoEvents();
                        }
                        else //Max to Min
                        {
                            //Max
                            To_Be_Changed = " 0x" + Max_1st + " 0x" + Max_2nd;
                            mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                        if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                        f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                            textBox_Mipi_CMD.Text = mipi_cmd;
                            textBox_count.Text = (++count).ToString();
                            int temp = Convert.ToInt16(Max, 16);
                            while (true) //(Max,Min)
                            {
                                if (stop) return;

                                Thread.Sleep(Convert.ToInt16(delay));
                                if (checkBox_Measure_Between_CMD.Checked) f1.CA_Measure_BT_Click();
                                System.Windows.Forms.Application.DoEvents();

                                temp -= Convert.ToInt16(step, 16);
                                if (temp <= Convert.ToInt16(Min, 16))
                                    break;
                                else
                                {
                                    string Temp_String = Convert.ToString(temp, 16).PadLeft(4, '0');
                                    To_Be_Changed = " 0x" + Temp_String.Substring(0, 2) + " 0x" + Temp_String.Substring(2, 2);
                                    mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                                if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                                f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                                    textBox_Mipi_CMD.Text = mipi_cmd;
                                    textBox_count.Text = (++count).ToString();
                                }
                            }
                            //Min
                            To_Be_Changed = " 0x" + Min_1st + " 0x" + Min_2nd;
                            mipi_cmd = temp_string + To_Be_Changed + Remaining_string;
                        if (checkBox_Offset_Apply.Checked) f1.IPC_Quick_Send_And_Show(offset, Color.Blue);
                        f1.IPC_Quick_Send_And_Show(mipi_cmd, Color.Black);
                            textBox_Mipi_CMD.Text = mipi_cmd;
                            textBox_count.Text = (++count).ToString();
                            Thread.Sleep(Convert.ToInt16(delay));
                            if (checkBox_Measure_Between_CMD.Checked) f1.CA_Measure_BT_Click();
                            System.Windows.Forms.Application.DoEvents();
                        }
                    }   
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Mipi CMD is too short..(< 20)");
                }
            
        }

 

        private void button_Change_Stop_Click(object sender, EventArgs e)
        {
            stop = true;
        }
    }
}
