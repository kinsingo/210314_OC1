using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public partial class Dual_Engineer_Monitoring_Mode : Form
    {
        //First Value
        Point First_Box2_Location = new Point();
        Point First_Box9_Location = new Point();
        int Form_Height;
        int Form_Width;

        bool Dual_Gamma_Apply_and_Measure_Stop = false;
        XYLv Measure;
        private XYLv[,] Limit_Condition1 = new XYLv[12, 10]; //12ea Bands , 10ea Gray-points
        private XYLv[,] Limit_Condition2 = new XYLv[12, 10]; //12ea Bands , 10ea Gray-points

        public RGB[,] All_band_gray_Gamma = new RGB[12, 10]; //12ea Bands , 10ea Gray-points

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

        private static Dual_Engineer_Monitoring_Mode Instance;
        public static Dual_Engineer_Monitoring_Mode getInstance()
        {
            if (Instance == null)
                Instance = new Dual_Engineer_Monitoring_Mode();

            return Instance;
        }


        private Dual_Engineer_Monitoring_Mode()
        {
            InitializeComponent();
        }
        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        private void Dual_Engineer_Monitoring_Mode_Load(object sender, EventArgs e)
        {
            BackColor = f1().current_model.Get_Back_Ground_Color();
            //Get First Location and Size
            First_Box2_Location = groupBox2.Location;
            First_Box9_Location = groupBox9.Location;
            Form_Height = this.Size.Height;
            Form_Width = this.Size.Width;

            radioButton_Gamma_Offset_Hide_CheckedChanged_1(sender, e);

            button_Read_OC_Param_From_Excel_File.PerformClick();
            button_Script_Load.PerformClick();
            button_Script_Transform.PerformClick();
            button_Script_Transform_2.PerformClick();

            Engineer_Mode_Grid_Tema_Change(dataGridView_OC_param_1, dataGridView_Band_OC_Viewer_1);
            Engineer_Mode_Grid_Tema_Change(dataGridView_OC_param_2, dataGridView_Band_OC_Viewer_2);
            Gamma_Offset_Tema_Change();

            OC_DataGridView_Not_Sortable();

            //Get First Limit Values
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    Limit_Condition1[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[10].Value);
                    Limit_Condition1[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[11].Value);
                    Limit_Condition1[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[12].Value);

                    Limit_Condition2[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[10].Value);
                    Limit_Condition2[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[11].Value);
                    Limit_Condition2[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[12].Value);
                }
            }
        }

        public void Dual_RadioButton_All_Enable(bool enable)
        {
            //Condition1
            radiobutton_Band0.Enabled = enable;
            radiobutton_Band1.Enabled = enable;
            radiobutton_Band2.Enabled = enable;
            radiobutton_Band3.Enabled = enable;
            radiobutton_Band4.Enabled = enable;
            radiobutton_Band5.Enabled = enable;
            radiobutton_Band6.Enabled = enable;
            radiobutton_Band7.Enabled = enable;
            radiobutton_Band8.Enabled = enable;

            radiobutton_AOD0.Enabled = enable;
            radiobutton_AOD1.Enabled = enable;
            radiobutton_AOD2.Enabled = enable;

            //Condition2
            radiobutton_2_Band0.Enabled = enable;
            radiobutton_2_Band1.Enabled = enable;
            radiobutton_2_Band2.Enabled = enable;
            radiobutton_2_Band3.Enabled = enable;
            radiobutton_2_Band4.Enabled = enable;
            radiobutton_2_Band5.Enabled = enable;
            radiobutton_2_Band6.Enabled = enable;
            radiobutton_2_Band7.Enabled = enable;
            radiobutton_2_Band8.Enabled = enable;

            radiobutton_2_AOD0.Enabled = enable;
            radiobutton_2_AOD1.Enabled = enable;
            radiobutton_2_AOD2.Enabled = enable;
        }




       

        private void OC_DataGridView_Not_Sortable()
        {
            foreach (DataGridViewColumn column in this.dataGridView_OC_param_1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_Band_OC_Viewer_1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_2.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_Band_OC_Viewer_2.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        //Normal
        private void first_Condition_Mipi_Script_Send()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("First Condition Script Applied", Color.Blue);

            //Send "mipi.write" of "delay" command
            for (int i = 0; i < textBox_Show_Compared_Mipi_Data.Lines.Length - 1; i++)
            {
                System.Windows.Forms.Application.DoEvents();

                if (textBox_Show_Compared_Mipi_Data.Lines[i].Length >= 10
                    && textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 10) == "mipi.write")
                {
                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data.Lines[i]);
                }
                else if (textBox_Show_Compared_Mipi_Data.Lines[i].Length >= 5 && (
                    textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5) == "delay"
                    ||textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5) == "image"))
                {
                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data.Lines[i]);
                }
                else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 14) == "gpio.i2c.write")
                {
                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data.Lines[i]);
                }
                else
                {
                    // It's not a "mipi.write" command , do nothing 
                }
            }
        }



        private void first_Condition_Mipi_Script_Change()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string temp_Mipi_Data_String = string.Empty;
            int count_mipi_cmd = 0;
            int count_one_mipi_cmd_length = 0;
            bool Flag = false;

            textBox_Show_Compared_Mipi_Data.Clear();

            //Delete others except for Mipi CMDs and Write on the 2nd Textbox
            for (int i = 0; i < textBox_Mipi_Script_Condition1.Lines.Length; i++)
            {
                if (textBox_Mipi_Script_Condition1.Lines[i].Length >= 20) // mipi.write 0xXX 0xXX <-- 20ea Character
                {
                    if (textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 10) == "mipi.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 10; k < textBox_Mipi_Script_Condition1.Lines[i].Length; k++)
                        {
                            if (textBox_Mipi_Script_Condition1.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && textBox_Mipi_Script_Condition1.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }
                    else if (textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 14) == "gpio.i2c.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 14; k < textBox_Mipi_Script_Condition1.Lines[i].Length; k++)
                        {
                            if (textBox_Mipi_Script_Condition1.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && textBox_Mipi_Script_Condition1.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }
                    else
                    {
                        // It's not a "mipi.write" of "delay" command , do nothing 
                    }
                }

                //Delay
                else if(textBox_Mipi_Script_Condition1.Lines[i].Length >= 5 
                    && textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5) != "     ")
                {
                    if (textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5) == "delay")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < textBox_Mipi_Script_Condition1.Lines[i].Length; k++)
                        {
                            if (textBox_Mipi_Script_Condition1.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && textBox_Mipi_Script_Condition1.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else if (textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5) == "image")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < textBox_Mipi_Script_Condition1.Lines[i].Length; k++)
                        {
                            if (textBox_Mipi_Script_Condition1.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && textBox_Mipi_Script_Condition1.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition1.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }

                }
            }
        }
        private void Second_Condition_Mipi_Script_Send()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            f1.GB_Status_AppendText_Nextline("Second Condition Script Applied", Color.Red);

            //Send "mipi.write" of "delay" command
            for (int i = 0; i < textBox_Show_Compared_Mipi_Data_2.Lines.Length - 1; i++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (textBox_Show_Compared_Mipi_Data_2.Lines[i].Length >= 10 &&
                    textBox_Show_Compared_Mipi_Data_2.Lines[i].Substring(0, 10) == "mipi.write")
                {
                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data_2.Lines[i]);
                }
                else if (textBox_Show_Compared_Mipi_Data_2.Lines[i].Length >= 5 &&
                    (textBox_Show_Compared_Mipi_Data_2.Lines[i].Substring(0, 5) == "delay"
                    || textBox_Show_Compared_Mipi_Data_2.Lines[i].Substring(0, 5) == "image"))
                {
                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data_2.Lines[i]);
                }
                else if (textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 14) == "gpio.i2c.write")
                {
                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data_2.Lines[i]);
                }
                else
                {
                    // It's not a "mipi.write" command , do nothing 
                }
            }
        }
        private void Second_Condition_Mipi_Script_Change()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string temp_Mipi_Data_String = string.Empty;
            int count_mipi_cmd = 0;
            int count_one_mipi_cmd_length = 0;
            bool Flag = false;

            textBox_Show_Compared_Mipi_Data_2.Clear();

            //Delete others except for Mipi CMDs and Write on the 2nd Textbox
            for (int i = 0; i < textBox_Mipi_Script_Condition2.Lines.Length; i++)
            {
                if (textBox_Mipi_Script_Condition2.Lines[i].Length >= 20) // mipi.write 0xXX 0xXX <-- 20ea Character
                {
                    if (textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 10) == "mipi.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 10; k < textBox_Mipi_Script_Condition2.Lines[i].Length; k++)
                        {
                            if (textBox_Mipi_Script_Condition2.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && textBox_Mipi_Script_Condition2.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data_2.Text += temp_Mipi_Data_String + "\r\n";
                    }
                    else if (textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 14) == "gpio.i2c.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 14; k < textBox_Mipi_Script_Condition2.Lines[i].Length; k++)
                        {
                            if (textBox_Mipi_Script_Condition2.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && textBox_Mipi_Script_Condition2.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data_2.Text += temp_Mipi_Data_String + "\r\n";
                    }
                    else
                    {
                        // It's not a "mipi.write" of "delay" command , do nothing 
                    }
                }
                

                //Delay
                else if (textBox_Mipi_Script_Condition2.Lines[i].Length >= 5
                    && textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5) != "     ")
                {
                    if (textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5) == "delay")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < textBox_Mipi_Script_Condition2.Lines[i].Length; k++)
                        {
                            if (textBox_Mipi_Script_Condition2.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && textBox_Mipi_Script_Condition2.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data_2.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else if (textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5) == "image")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < textBox_Mipi_Script_Condition2.Lines[i].Length; k++)
                        {
                            if (textBox_Mipi_Script_Condition2.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && textBox_Mipi_Script_Condition2.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = textBox_Mipi_Script_Condition2.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data_2.Text += temp_Mipi_Data_String + "\r\n";
                    }

                }
            }
        }

        public void Dual_Script_Apply(bool Condition)
        {
            if(Condition)
            {
                button_Script_Apply.PerformClick();
            }
            else
            {
                button_Script_Apply_2.PerformClick();
            }
        }


        //Button
        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        private void button_Script_Transform_Click(object sender, EventArgs e)
        {
            first_Condition_Mipi_Script_Change();
        }

        public void Conditon_1_Script_Apply()
        {
            button_Script_Apply.PerformClick();
            //first_Condition_Mipi_Script_Send();
        }

        public void Conditon_2_Script_Apply()
        {
            button_Script_Apply_2.PerformClick();
            //Second_Condition_Mipi_Script_Send();
        }

        private void button_Script_Apply_Click(object sender, EventArgs e)
        {
            first_Condition_Mipi_Script_Send();
        }
        private void button_Script_Clear_Click(object sender, EventArgs e)
        {
            textBox_Mipi_Script_Condition1.Clear();
        }
        private void button_Script_Transform_2_Click(object sender, EventArgs e)
        {
            Second_Condition_Mipi_Script_Change();
        }
        private void button_Script_Apply_2_Click(object sender, EventArgs e)
        {
            Second_Condition_Mipi_Script_Send();
        }
        private void button_Script_Clear_2_Click(object sender, EventArgs e)
        {
            textBox_Mipi_Script_Condition2.Clear();
        }
        private void button_Script_Load_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string Path_1 = string.Empty;
            string Path_2 = string.Empty;
            f1.Get_Dual_Mode_TXT_Path(ref Path_1, ref Path_2);

            this.textBox_Mipi_Script_Condition1.Text = File.ReadAllText(Path_1);
            this.textBox_Mipi_Script_Condition2.Text = File.ReadAllText(Path_2);

            textBox_Show_Compared_Mipi_Data.Clear();
            textBox_Show_Compared_Mipi_Data_2.Clear();
        }

        private void button_Read_OC_Param_From_Excel_File_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.current_model.Read_OC_Param_From_Excel_For_Dual_Mode();
            Band_Radiobuttion_Select(0, true); //Select Band as 0
            Band_Radiobuttion_Select(0, false); //Select Band as 0
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {
            
        }

        public string Dual_Get_BX_GXXX_By_Gray_DP116(int gray)
        {
            return dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[0].Value.ToString();
        }



        private void Gamma_Offset_Tema_Change()
        {
            dataGridView_Gamma_Offset.Columns[0].Width = 80;

            for (int i = 1; i <= 3; i++) //Offset Gamma R,G,B
            {
                dataGridView_Gamma_Offset.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                dataGridView_Gamma_Offset.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                dataGridView_Gamma_Offset.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Gamma_Offset.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Gamma_Offset.Columns[i].Width = 40;
            }

            for (int i = 4; i <= 6; i++) //Diff Gamma (L-R) R,G,B
            {
                dataGridView_Gamma_Offset.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_Gamma_Offset.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                dataGridView_Gamma_Offset.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Gamma_Offset.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Gamma_Offset.Columns[i].Width = 40;
            }
        }

        private void Engineer_Mode_Grid_Tema_Change(DataGridView dataGridView_OC_param, DataGridView dataGridView_Band_OC_Viewer)
        {
            dataGridView_OC_param.Columns[0].Width = 80;
            dataGridView_Band_OC_Viewer.Columns[0].Width = 80;

            for (int i = 1; i <= 3; i++) //Gamma R,G,B
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 40;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 40;

            }

            for (int i = 4; i <= 6; i++) //Measure X,Y,Lv
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 55;
            }

            for (int i = 7; i <= 9; i++) //Target X,Y,Lv
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 55;
            }

            for (int i = 10; i <= 12; i++) //Limit X,Y,Lv
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 55;
            }

            for (int i = 13; i <= 15; i++) //Extension X,Y,Applied
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 55;
            }

            //Loop
            dataGridView_OC_param.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param.Columns[16].Width = 40;

            dataGridView_Band_OC_Viewer.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_Band_OC_Viewer.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_Band_OC_Viewer.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_Band_OC_Viewer.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_Band_OC_Viewer.Columns[16].Width = 40;
        }
        private void dataGridView_OC_param_1_KeyDown(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            if (e.KeyData == (Keys.Control | Keys.V)) PasteInData(ref this.dataGridView_OC_param_1);
        }
        private void dataGridView_OC_param_2_KeyDown(object sender, KeyEventArgs e)
        {
            
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            if (e.KeyData == (Keys.Control | Keys.V)) PasteInData(ref this.dataGridView_OC_param_2);
        }
        private void PasteInData(ref DataGridView dgv)
        {
            char[] rowSplitter = { '\n', '\r' };  // Cr and Lf.
            char columnSplitter = '\t';         // Tab.

            IDataObject dataInClipboard = Clipboard.GetDataObject();

            string stringInClipboard =
                dataInClipboard.GetData(DataFormats.Text).ToString();

            string[] rowsInClipboard = stringInClipboard.Split(rowSplitter,
                StringSplitOptions.RemoveEmptyEntries);

            int r = dgv.SelectedCells[0].RowIndex;
            int c = dgv.SelectedCells[0].ColumnIndex;

            if (dgv.Rows.Count < (r + rowsInClipboard.Length))
                dgv.Rows.Add(r + rowsInClipboard.Length - dgv.Rows.Count);

            // Loop through lines:

            int iRow = 0;
            while (iRow < rowsInClipboard.Length)
            {
                // Split up rows to get individual cells:

                string[] valuesInRow =
                    rowsInClipboard[iRow].Split(columnSplitter);

                // Cycle through cells.
                // Assign cell value only if within columns of grid:

                int jCol = 0;
                while (jCol < valuesInRow.Length)
                {
                    if ((dgv.ColumnCount - 1) >= (c + jCol))
                        dgv.Rows[r + iRow].Cells[c + jCol].Value =
                        valuesInRow[jCol];

                    jCol += 1;
                } // end while

                iRow += 1;
            } // end while
        }

        private void Copy_Data_Grid_View(int Offset_Row , bool Condition)
        {
            if (Condition)
            {
                for (int j = 0; j < dataGridView_Band_OC_Viewer_1.ColumnCount; j++)
                {
                    for (int i = 2; i < dataGridView_Band_OC_Viewer_1.RowCount; i++)
                    {
                        dataGridView_Band_OC_Viewer_1.Rows[i].Cells[j].Value = dataGridView_OC_param_1.Rows[i + Offset_Row].Cells[j].Value;
                    }
                }
            }
            else
            {
                for (int j = 0; j < dataGridView_Band_OC_Viewer_2.ColumnCount; j++)
                {
                    for (int i = 2; i < dataGridView_Band_OC_Viewer_2.RowCount; i++)
                    {
                        dataGridView_Band_OC_Viewer_2.Rows[i].Cells[j].Value = dataGridView_OC_param_2.Rows[i + Offset_Row].Cells[j].Value;
                    }
                }
            }
        }
        private void Get_OC_Param_By_Gray_DP116(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
    , ref double Limit_Y, ref double Limit_Lv, ref double Extension_X, ref double Extension_Y,bool Condition)
        {
            if (Condition)
            {
                Gamma_R = Convert.ToInt16(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[1].Value.ToString());
                Gamma_G = Convert.ToInt16(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[2].Value.ToString());
                Gamma_B = Convert.ToInt16(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[3].Value.ToString());

                Target_X = Convert.ToDouble(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[7].Value.ToString());
                Target_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[8].Value.ToString());
                Target_Lv = Convert.ToDouble(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[9].Value.ToString());

                Limit_X = Convert.ToDouble(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[10].Value.ToString());
                Limit_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[11].Value.ToString());
                Limit_Lv = Convert.ToDouble(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[12].Value.ToString());

                Extension_X = Convert.ToDouble(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[13].Value.ToString());
                Extension_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[14].Value.ToString());
            }
            else
            {
                Gamma_R = Convert.ToInt16(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[1].Value.ToString());
                Gamma_G = Convert.ToInt16(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[2].Value.ToString());
                Gamma_B = Convert.ToInt16(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[3].Value.ToString());

                Target_X = Convert.ToDouble(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[7].Value.ToString());
                Target_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[8].Value.ToString());
                Target_Lv = Convert.ToDouble(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[9].Value.ToString());

                Limit_X = Convert.ToDouble(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[10].Value.ToString());
                Limit_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[11].Value.ToString());
                Limit_Lv = Convert.ToDouble(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[12].Value.ToString());

                Extension_X = Convert.ToDouble(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[13].Value.ToString());
                Extension_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[14].Value.ToString());
            } 
        }
        private void Set_OC_Param_By_Gray_DP116(int gray, int Gamma_R, int Gamma_G, int Gamma_B, 
            double Measure_X, double Measure_Y, double Measure_Lv, double loop_count, string Extension_Applied, bool Condition)
        {
            if (Condition)
            {
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[1].Value = Gamma_R;
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[2].Value = Gamma_G;
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[3].Value = Gamma_B;

                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[4].Value = Measure_X;
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[5].Value = Measure_Y;
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[6].Value = Measure_Lv;

                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[15].Value = Extension_Applied;
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[16].Value = loop_count;
            }
            else
            {
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[1].Value = Gamma_R;
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[2].Value = Gamma_G;
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[3].Value = Gamma_B;

                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[4].Value = Measure_X;
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[5].Value = Measure_Y;
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[6].Value = Measure_Lv;

                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[15].Value = Extension_Applied;
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[16].Value = loop_count;
            }
        }

        public void DP116_Get_All_Band_Gray_Gamma(RGB[,] Gamma,bool Condition)
        {
            if (Condition)
            {
                for (int band = 0; band < 12; band++)
                {
                    for (int gray = 0; gray < 10; gray++)
                    {
                        Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[1].Value.ToString());
                        Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[2].Value.ToString());
                        Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[3].Value.ToString());
                        Gamma[band, gray].String_Update_From_int();

                    }
                }
            }
            else
            {
                for (int band = 0; band < 12; band++)
                {
                    for (int gray = 0; gray < 10; gray++)
                    {
                        Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[1].Value.ToString());
                        Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[2].Value.ToString());
                        Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[3].Value.ToString());
                        Gamma[band, gray].String_Update_From_int();

                    }
                }
            }
            
        }
        public void Updata_Sub_To_Main_GridView(int band, int gray,bool Condition)
        {
            if (Condition)
            {
                int Offset_Row = 10 * band;
                for (int j = 0; j < dataGridView_Band_OC_Viewer_1.ColumnCount; j++)
                {
                    dataGridView_OC_param_1.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[j].Value;
                }
            }
            else
            {
                int Offset_Row = 10 * band;
                for (int j = 0; j < dataGridView_Band_OC_Viewer_2.ColumnCount; j++)
                {
                    dataGridView_OC_param_2.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[j].Value;
                }
            }
        }

        public void Set_OC_Param_DP116(int gray, int Gamma_R, int Gamma_G, int Gamma_B, 
            double Measure_X, double Measure_Y, double Measure_Lv, int loop_count, string Extension_Applied,bool Condition)
        {
            //Set Param according to gray
            Set_OC_Param_By_Gray_DP116(gray, Gamma_R, Gamma_G, Gamma_B,
                Measure_X, Measure_Y, Measure_Lv, loop_count, Extension_Applied, Condition);
        }
       
        public void Get_OC_Param_DP116(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
      , ref double Limit_Y, ref double Limit_Lv, ref double Extension_X, ref double Extension_Y,bool Condition)
        {
            //Get Param according to gray
            Get_OC_Param_By_Gray_DP116(gray, ref Gamma_R, ref Gamma_G, ref Gamma_B,
                ref Target_X, ref Target_Y, ref Target_Lv, ref Limit_X, ref Limit_Y, ref Limit_Lv, ref Extension_X, ref Extension_Y, Condition);
        }

        public void Dual_Engineering_Mode_DataGridview_ReadOnly(bool ReadOnly)
        {
            this.dataGridView_OC_param_1.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_2.ReadOnly = ReadOnly;
        }

        public void Dual_Mode_Gamma_Offset_Apply(int band,int gray)
        {
                    dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[1].Value);
                      /*  +  Convert.ToInt16(dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[1].Value);*/
                    dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[2].Value);
                      /*  + Convert.ToInt16(dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[2].Value);*/
                    dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[3].Value);
                      /*  + Convert.ToInt16(dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[3].Value);*/
        }

        private void Dual_Mode_Cal_Gamma_Diff()
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[4].Value = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[1].Value)
                        - Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[1].Value);
                    dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[5].Value = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[2].Value)
                        - Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[2].Value);
                    dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[6].Value = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[3].Value)
                        - Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[3].Value);
                    
                }
            }
        }

        public void Clear_Dual_Mode_Cal_Gamma_Diff()
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_Gamma_Offset.Rows[band * 10 + (gray + 2)].Cells[6].Value = string.Empty;

                }
            }
        }


        public void Dual_Mode_GridView_Measure_Extension_LoopCound_Area_Data_Clear()
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    //Measure Condition 1 Clear
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[4].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[5].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[6].Value = string.Empty;
                    dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[6].Value = string.Empty;

                    //Extension Loopcount Condition 1 Clear
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[15].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[16].Value = string.Empty;
                    dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[16].Value = string.Empty;


                    //Measure Condition 2 Clear
                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[4].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[5].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[6].Value = string.Empty;
                    dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[6].Value = string.Empty;

                    //Extension Loopcount Condition 2 Clear
                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[15].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[16].Value = string.Empty;
                    dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[16].Value = string.Empty;
                }
            }

        }
        public void Band_Radiobuttion_Select(int band, bool Condition)
        {
            if (Condition)
            {
                switch (band)
                {
                    case 0:
                        radiobutton_Band0.Checked = true;
                        break;
                    case 1:
                        radiobutton_Band1.Checked = true;
                        break;
                    case 2:
                        radiobutton_Band2.Checked = true;
                        break;
                    case 3:
                        radiobutton_Band3.Checked = true;
                        break;
                    case 4:
                        radiobutton_Band4.Checked = true;
                        break;
                    case 5:
                        radiobutton_Band5.Checked = true;
                        break;
                    case 6:
                        radiobutton_Band6.Checked = true;
                        break;
                    case 7:
                        radiobutton_Band7.Checked = true;
                        break;
                    case 8:
                        radiobutton_Band8.Checked = true;
                        break;
                    case 9:
                        radiobutton_AOD0.Checked = true;
                        break;
                    case 10:
                        radiobutton_AOD1.Checked = true;
                        break;
                    case 11:
                        radiobutton_AOD2.Checked = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (band)
                {
                    case 0:
                        radiobutton_2_Band0.Checked = true;
                        break;
                    case 1:
                        radiobutton_2_Band1.Checked = true;
                        break;
                    case 2:
                        radiobutton_2_Band2.Checked = true;
                        break;
                    case 3:
                        radiobutton_2_Band3.Checked = true;
                        break;
                    case 4:
                        radiobutton_2_Band4.Checked = true;
                        break;
                    case 5:
                        radiobutton_2_Band5.Checked = true;
                        break;
                    case 6:
                        radiobutton_2_Band6.Checked = true;
                        break;
                    case 7:
                        radiobutton_2_Band7.Checked = true;
                        break;
                    case 8:
                        radiobutton_2_Band8.Checked = true;
                        break;
                    case 9:
                        radiobutton_2_AOD0.Checked = true;
                        break;
                    case 10:
                        radiobutton_2_AOD1.Checked = true;
                        break;
                    case 11:
                        radiobutton_2_AOD2.Checked = true;
                        break;
                    default:
                        break;
                }
            }
        }
        public void Get_Gamma_Only_DP116(int band, int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool Condition)
        {
            if (Condition)
            {
                Gamma_R = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[1].Value.ToString());
                Gamma_G = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[2].Value.ToString());
                Gamma_B = Convert.ToInt16(dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[3].Value.ToString());
            }
            else
            {
                Gamma_R = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[1].Value.ToString());
                Gamma_G = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[2].Value.ToString());
                Gamma_B = Convert.ToInt16(dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[3].Value.ToString());
            }
        }


        private void radiobutton_Band0_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 0;
            if (radiobutton_Band0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_Band1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 1;
            if (radiobutton_Band1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, true);
            }
        }
        private void radiobutton_Band2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 2;
            if (radiobutton_Band2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_Band3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 3;
            if (radiobutton_Band3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_Band4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 4;
            if (radiobutton_Band4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_Band5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 5;
            if (radiobutton_Band5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_Band6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 6;
            if (radiobutton_Band6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_Band7_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 7;
            if (radiobutton_Band7.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_Band8_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 8;
            if (radiobutton_Band8.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_AOD0_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 9;
            if (radiobutton_AOD0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_AOD1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 10;
            if (radiobutton_AOD1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }
        private void radiobutton_AOD2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 11;
            if (radiobutton_AOD2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row,true);
            }
        }

        private void radiobutton_2_Band0_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 0;
            if (radiobutton_2_Band0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_Band1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 1;
            if (radiobutton_2_Band1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_Band2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 2;
            if (radiobutton_2_Band2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_Band3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 3;
            if (radiobutton_2_Band3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_Band4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 4;
            if (radiobutton_2_Band4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_Band5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 5;
            if (radiobutton_2_Band5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_Band6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 6;
            if (radiobutton_2_Band6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_Band7_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 7;
            if (radiobutton_2_Band7.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_Band8_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 8;
            if (radiobutton_2_Band8.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_AOD0_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 9;
            if (radiobutton_2_AOD0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_AOD1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 10;
            if (radiobutton_2_AOD1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }
        private void radiobutton_2_AOD2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 11;
            if (radiobutton_2_AOD2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        public void Band_Gray_Gamma_Copy_L_to_R(int band,int gray)
        {
            dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[1].Value = dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[1].Value;
            dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[2].Value = dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[2].Value;
            dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[3].Value = dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[3].Value;
        }

        public void Dual_Copy_C1Measure_To_C2Target(int band, int gray)
        {
            dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[7].Value = dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[8].Value = dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[9].Value = dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_All_Gamma_Copy()
        {
            button_Gamma_Copy.PerformClick();
        }

        public void Dual_Band_Gamma_Copy(int band)
        {
            for (int gray = 0; gray < 10; gray++)
            {
                Band_Gray_Gamma_Copy_L_to_R(band, gray);
            }
        }

        public void Dual_Update_Viewer_Sheet_form_OC_Sheet(int band,bool condition)
        {
            int Offset_Row = 10 * band;
            Copy_Data_Grid_View(Offset_Row, condition);
        }

        private void button_Gamma_Copy_Click(object sender, EventArgs e)
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    Band_Gray_Gamma_Copy_L_to_R(band, gray);
                }
            }
        }

        private int Get_Current_Band(bool Condition)
        {
            int band=0;

            if (Condition)
            {
                if (radiobutton_Band0.Checked)
                    band = 0;
                else if (radiobutton_Band1.Checked)
                    band = 1;
                else if (radiobutton_Band2.Checked)
                    band = 2;
                else if (radiobutton_Band3.Checked)
                    band = 3;
                else if (radiobutton_Band4.Checked)
                    band = 4;
                else if (radiobutton_Band5.Checked)
                    band = 5;
                else if (radiobutton_Band6.Checked)
                    band = 6;
                else if (radiobutton_Band7.Checked)
                    band = 7;
                else if (radiobutton_Band8.Checked)
                    band = 8;
                else if (radiobutton_AOD0.Checked)
                    band = 9;
                else if (radiobutton_AOD1.Checked)
                    band = 10;
                else if (radiobutton_AOD2.Checked)
                    band = 11;
                else
                { 
                    //Do nothing 
                }
                
            }
            else
            {
               if(radiobutton_2_Band0.Checked)
                    band = 0;
                else if (radiobutton_2_Band1.Checked)
                    band = 1;
                else if (radiobutton_2_Band2.Checked)
                    band = 2;
                else if (radiobutton_2_Band3.Checked)
                    band = 3;
                else if (radiobutton_2_Band4.Checked)
                    band = 4;
                else if (radiobutton_2_Band5.Checked)
                    band = 5;
                else if (radiobutton_2_Band6.Checked)
                    band = 6;
                else if (radiobutton_2_Band7.Checked)
                    band = 7;
                else if (radiobutton_2_Band8.Checked)
                    band = 8;
                else if (radiobutton_2_AOD0.Checked)
                    band = 9;
                else if (radiobutton_2_AOD1.Checked)
                    band = 10;
                else if (radiobutton_2_AOD2.Checked)
                    band = 11;
                else
                { 
                    //Do nothing 
                }
            }
            return band;
        }

        public void Dual_All_Add_Offset_Gamma()
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    Dual_Mode_Gamma_Offset_Apply(band, gray);
                }
            }
        }

        public void Dual_Band_Add_Offset_Gamma(int band)
        {
            for (int gray = 0; gray < 10; gray++)
            {
                Dual_Mode_Gamma_Offset_Apply(band, gray);
            }
        }




        public void Dual_Cal_Diff_R_L_Gamma()
        {
            button_Cal_Diff_Gamma.PerformClick();
        }

        private void button_Add_Offset_Gamma_Click(object sender, EventArgs e)
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    Dual_Mode_Gamma_Offset_Apply(band,gray);
                }
            }
            Dual_Update_Viewer_Sheet_form_OC_Sheet(Get_Current_Band(false), false);
        }





        private void button_Cal_Diff_Gamma_Click(object sender, EventArgs e)
        {
            Dual_Mode_Cal_Gamma_Diff();
        }


        private void deltaEMeasureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dual_SH_Delta_E obj_dual_sh_delta_e = Dual_SH_Delta_E.getInstance();

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (f1.label_CA_remote_status.Text == "CA Remote : On")
            {

                obj_dual_sh_delta_e.Visible = false;
                obj_dual_sh_delta_e.Show();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Connect CA310 First");
            }
        }


        private void radioButton_Gamma_Offset_Show_CheckedChanged_1(object sender, EventArgs e)
        {
            this.AutoScrollPosition = new Point(0, 0);

            if (radioButton_Gamma_Offset_Hide.Checked)
            {
                groupBox9.Hide();
                groupBox2.Location = First_Box9_Location;
                this.Size = new Size(Form_Width * 1362 / 1752, Form_Height * 940 / 940);
            }

            else if (radioButton_Gamma_Offset_Show.Checked)
            {
                groupBox9.Show();
                groupBox2.Location = First_Box2_Location;
                this.Size = new Size(Form_Width, Form_Height);
            }
        }

        private void radioButton_Gamma_Offset_Hide_CheckedChanged_1(object sender, EventArgs e)
        {
            this.AutoScrollPosition = new Point(0, 0);

            /*
            if (radioButton_Gamma_Offset_Hide.Checked)
            {
                groupBox9.Hide();
                groupBox2.Location = new Point(678,32);
                this.Size = new Size(1362, 940);
            }

            else if (radioButton_Gamma_Offset_Show.Checked)
            {
                groupBox9.Show();
                groupBox2.Location = new Point(1075,32);
                this.Size = new Size(1752, 940);
            }
             * */
            if (radioButton_Gamma_Offset_Hide.Checked)
            {
                groupBox9.Hide();
                groupBox2.Location = First_Box9_Location;
                this.Size = new Size(Form_Width * 1362 / 1752, Form_Height * 940 / 940);
            }

            else if (radioButton_Gamma_Offset_Show.Checked)
            {
                groupBox9.Show();
                groupBox2.Location = First_Box2_Location;
                this.Size = new Size(Form_Width, Form_Height);
            }
        }

        private void button_Gamma_Down_Condition_1_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            First_Model_Option_Form.getInstance().Show();
            System.Windows.Forms.Application.DoEvents();
            First_Model_Option_Form First_Model = (First_Model_Option_Form)Application.OpenForms["First_Model_Option_Form"];

            //VREF1,2 / AM0 / Vreg1
            f1.DP116_Read_Vreg1_VREF_AM0_and_Send(true, false, ref First_Model.Current_Page_Address);

            //IRC
            f1.GB_Status_AppendText_Nextline("#IRC Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(16, "4A", true);
            f1.Read_DP116_Quantity_Register_to_Send(16, "4B", true);
            f1.Read_DP116_Quantity_Register_to_Send(12, "4C", true);

            //Gamma 255
            f1.GB_Status_AppendText_Nextline("#Condition 1 Gamma Send from OC_DataGrid", System.Drawing.Color.Blue);
            this.DP116_Get_All_Band_Gray_Gamma(All_band_gray_Gamma,true);
            f1.DP116_CMD2_Page_Selection_And_Show(11, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            First_Model.Send_Band_Gray_Gamma(All_band_gray_Gamma,0,0,true);

            //Gamma Gray (<255)
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            for (int band = 0; band < 12; band++)
                First_Model.Send_Band_Gray_Gamma(All_band_gray_Gamma,band, 1, true);

            //ELVSS
            f1.GB_Status_AppendText_Nextline("#ELVSS Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(11, "BD", true);
            f1.Read_DP116_Quantity_Register_to_Send(11, "BE", true);

            //Vinit
            f1.GB_Status_AppendText_Nextline("#Vinit Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "9F", true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "A2", true);

            //ELVSS Set
            f1.GB_Status_AppendText_Nextline("#ELVSS Set Read and Send", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x10 #CMD1 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD1_Page_Selection(false, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "66", true);
        }

        private void button_Gamma_Down_Condition_2_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            First_Model_Option_Form.getInstance().Show();
            System.Windows.Forms.Application.DoEvents();
            First_Model_Option_Form First_Model = (First_Model_Option_Form)Application.OpenForms["First_Model_Option_Form"];

            //VREF1,2 / AM0 / Vreg1
            f1.DP116_Read_Vreg1_VREF_AM0_and_Send(false, false, ref First_Model.Current_Page_Address);

            //IRC
            f1.GB_Status_AppendText_Nextline("#IRC Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(16, "4A", true);
            f1.Read_DP116_Quantity_Register_to_Send(16, "4B", true);
            f1.Read_DP116_Quantity_Register_to_Send(12, "4C", true);

            //Gamma 255
            f1.GB_Status_AppendText_Nextline("#Condition 2 Gamma Send from OC_DataGrid", System.Drawing.Color.Blue);
            this.DP116_Get_All_Band_Gray_Gamma(All_band_gray_Gamma, false);
            f1.DP116_CMD2_Page_Selection_And_Show(11, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            First_Model.Send_Band_Gray_Gamma(All_band_gray_Gamma,0, 0, false);

            //Gamma Gray (<255)
            f1.DP116_CMD2_Page_Selection_And_Show(1, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            for (int band = 0; band < 12; band++)
                First_Model.Send_Band_Gray_Gamma(All_band_gray_Gamma,band, 1, false);

            //ELVSS
            f1.GB_Status_AppendText_Nextline("#ELVSS Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(11, "BD", true);
            f1.Read_DP116_Quantity_Register_to_Send(11, "BE", true);

            //Vinit
            f1.GB_Status_AppendText_Nextline("#Vinit Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.DarkGreen, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "9F", true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "A2", true);

            //ELVSS Set
            f1.GB_Status_AppendText_Nextline("#ELVSS Set Read and Send", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x10 #CMD1 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD1_Page_Selection(false, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "66", true);

        }

        private void button_Gamma_Read_Condition1_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            First_Model_Option_Form First_Model = (First_Model_Option_Form)Application.OpenForms["First_Model_Option_Form"];
            f1.GB_Status_AppendText_Nextline("#--- VREF1 , VREF2 , AM0 ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AD", "VREF1"); //VREF1
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AE", "VREF2"); //VREF2
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AF", "AM0"); //AM0

            f1.GB_Status_AppendText_Nextline("#--- Condition1 Vreg1 ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "DD", "Vreg1(1)"); //Vreg1 +
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "DE", "Vreg2(2)"); //Vreg1 + 2

            f1.GB_Status_AppendText_Nextline("#--- IRC Setting ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "4A");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "4B");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(12, "4C");

            f1.GB_Status_AppendText_Nextline("#--- Condition1 Gamma(Gray) ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.DP116_Read_Gamma_Gray();

            f1.GB_Status_AppendText_Nextline("#---Condition1 Gamma(White) ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(11, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "EA");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "EB");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "EC");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "ED");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "EE");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "EF");

            f1.GB_Status_AppendText_Nextline("#--- ELVSS  ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x2C #CMD2_P12 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection(12, false, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(11, "BD");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(11, "BE");

            f1.GB_Status_AppendText_Nextline("#--- Vinit ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x20 #CMD2_P0 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection(0, false, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "9F");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "A2");

            f1.GB_Status_AppendText_Nextline("#--- ELVSS Set ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x10 #CMD1 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD1_Page_Selection(false, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "66");
        }

        private void button_Gamma_Read_Condition2_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            First_Model_Option_Form First_Model = (First_Model_Option_Form)Application.OpenForms["First_Model_Option_Form"];
            f1.GB_Status_AppendText_Nextline("#--- VREF1 , VREF2 , AM0 ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AD", "VREF1"); //VREF1
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AE", "VREF2"); //VREF2
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AF", "AM0"); //AM0

            f1.GB_Status_AppendText_Nextline("#--- Condition2 Vreg1 ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(1, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "DD", "Vreg1(1)"); //Vreg1 +
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "DE", "Vreg2(2)"); //Vreg1 + 2

            f1.GB_Status_AppendText_Nextline("#--- IRC Setting ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "4A");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "4B");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(12, "4C");

            f1.GB_Status_AppendText_Nextline("#--- Condition2 Gamma(Gray) ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(1, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.DP116_Read_Gamma_Gray();

            f1.GB_Status_AppendText_Nextline("#---Condition2 Gamma(White) ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(11, false, Color.Blue, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "F0");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "F1");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "F2");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "F3");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "F4");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "F5");

            f1.GB_Status_AppendText_Nextline("#--- ELVSS  ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x2C #CMD2_P12 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection(12, false, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(11, "BD");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(11, "BE");

            f1.GB_Status_AppendText_Nextline("#--- Vinit ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x20 #CMD2_P0 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection(0, false, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "9F");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "A2");

            f1.GB_Status_AppendText_Nextline("#--- ELVSS Set ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x10 #CMD1 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD1_Page_Selection(false, false, ref First_Model.Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "66");
        }

        private void Limit_Update(double Ratio, bool XY,bool Condition1)
        {
            if (Condition1)
            {
                for (int band = 0; band < 12; band++)
                {
                    for (int gray = 0; gray < 10; gray++)
                    {
                        if (XY)
                        {

                            if ((Limit_Condition1[band, gray].double_X * Ratio) >= 0.99 || Limit_Condition1[band, gray].double_X >= 0.99) dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[10].Value = 1;
                            else dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[10].Value = Limit_Condition1[band, gray].double_X * Ratio;

                            if ((Limit_Condition1[band, gray].double_Y * Ratio) >= 0.99 || Limit_Condition1[band, gray].double_Y >= 0.99) dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[11].Value = 1;
                            else dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[11].Value = Limit_Condition1[band, gray].double_Y * Ratio;
                        }
                        else //Lv
                        {
                            dataGridView_OC_param_1.Rows[band * 10 + (gray + 2)].Cells[12].Value = Limit_Condition1[band, gray].double_Lv * Ratio;
                        }
                    }
                }

                //update main_Grid_view from sub_Grid_View
                int Current_Band = this.Get_Current_Band(true);
                int Offset_Row = 10 * Current_Band;
                Copy_Data_Grid_View(Offset_Row,true);
            }
            else //Condition2
            {
                for (int band = 0; band < 12; band++)
                {
                    for (int gray = 0; gray < 10; gray++)
                    {
                        if (XY)
                        {
                            if ((Limit_Condition2[band, gray].double_X * Ratio) >= 0.99 || Limit_Condition2[band, gray].double_X >= 0.99) dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[10].Value = 1;
                            else dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[10].Value = Limit_Condition2[band, gray].double_X * Ratio;

                            if ((Limit_Condition2[band, gray].double_Y * Ratio) >= 0.99 || Limit_Condition2[band, gray].double_Y >= 0.99) dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[11].Value = 1;
                            else dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[11].Value = Limit_Condition2[band, gray].double_Y * Ratio;
                        }
                        else //Lv
                        {
                            dataGridView_OC_param_2.Rows[band * 10 + (gray + 2)].Cells[12].Value = Limit_Condition2[band, gray].double_Lv * Ratio;
                        }
                    }
                }

                //update main_Grid_view from sub_Grid_View
                int Current_Band = this.Get_Current_Band(false);
                int Offset_Row = 10 * Current_Band;
                Copy_Data_Grid_View(Offset_Row, false);
            }
        }


        private void radioButton_Limit_Ratio_150_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150.Checked)
            {
                Limit_Update(1.5, true,true);
            }
        }

        private void radioButton_Limit_Ratio_100_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100.Checked)
            {
                Limit_Update(1, true, true);
            }
        }

        private void radioButton_Limit_Ratio_80_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80.Checked)
            {
                Limit_Update(0.8, true, true);
            }
        }

        private void radioButton_Limit_Ratio_60_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60.Checked)
            {
                Limit_Update(0.6, true, true);
            }
        }

        private void radioButton_Limit_Lv_Ratio_150_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150.Checked)
            {
                Limit_Update(1.5, false, true);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100.Checked)
            {
                Limit_Update(1, false, true);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80.Checked)
            {
                Limit_Update(0.8, false, true);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60.Checked)
            {
                Limit_Update(0.6, false, true);
            }
        }

        private void radioButton_Limit_Ratio_150_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150_2.Checked)
            {
                Limit_Update(1.5, true,false);
            }
        }

        private void radioButton_Limit_Ratio_100_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100_2.Checked)
            {
                Limit_Update(1, true, false);
            }
        }

        private void radioButton_Limit_Ratio_80_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80_2.Checked)
            {
                Limit_Update(0.8, true, false);
            }
        }

        private void radioButton_Limit_Ratio_60_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60_2.Checked)
            {
                Limit_Update(0.6, true, false);
            }
        }

        private void radioButton_Limit_Lv_Ratio_150_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150_2.Checked)
            {
                Limit_Update(1.5, false, false);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100_2.Checked)
            {
                Limit_Update(1, false, false);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80_2.Checked)
            {
                Limit_Update(0.8, false, false);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60_2.Checked)
            {
                Limit_Update(0.6, false, false);
            }
        }


        private bool Dual_Band_BSQH_Selection(ref int band)
        {
            if (checkBox_Band1.Checked == false && band == 0)
                return false;
            else if (checkBox_Band2.Checked == false && band == 1)
                return false;
            else if (checkBox_Band3.Checked == false && band == 2)
                return false;
            else if (checkBox_Band4.Checked == false && band == 3)
                return false;
            else if (checkBox_Band5.Checked == false && band == 4)
                return false;
            else if (checkBox_Band6.Checked == false && band == 5)
                return false;
            else if (checkBox_Band7.Checked == false && band == 6)
                return false;
            else if (checkBox_Band8.Checked == false && band == 7)
                return false;
            else if (checkBox_Band9.Checked == false && band == 8)
                return false;
            else if (checkBox_Band10.Checked == false && band == 9)
                return false;
            else if (checkBox_Band11.Checked == false && band == 10)
                return false;
            else if (checkBox_Band12.Checked == false && band == 11)
                return false;
            else
                return true;
        }




        private void button_Dual_Gamma_Apply_and_Measure_Click(object sender, EventArgs e)
        {
            Dual_Gamma_Apply_and_Measure_Stop = false;
            bool Condition = true;
            int Measure_delay = Convert.ToInt16(textBox_Dual_Measure_Delay.Text);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Option_Setting_Tool_Click();

            First_Model_Option_Form First_Model = (First_Model_Option_Form)Application.OpenForms["First_Model_Option_Form"];
            First_Model.Read_DP116_DBV_Setting();
            int gray = 0;
            int band = 0;

            //Measure Clear
            Dual_Mode_GridView_Measure_Extension_LoopCound_Area_Data_Clear();

            //Gamma Apply
            if (checkBox_Gamma_Down.Checked)
            {
                f1.GB_Status_AppendText_Nextline("Gamma Down Before Dual Measure", Color.Blue);
                button_Gamma_Down_Condition_1_Click(sender, e);
                button_Gamma_Down_Condition_2_Click(sender, e);
            }
            else
            {
                f1.GB_Status_AppendText_Nextline("Dual Measure without Gamma Down",Color.Blue);
            }

            //Measure
            for (band = 0; band < 12; band++)
            {
                if (Dual_Band_BSQH_Selection(ref band))  //If this band is not selected , move on to the next band
                {
                    if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                    {
                        f1.AOD_On();
                        f1.GB_Status_AppendText_Nextline("AOD" + (band - 9).ToString(), Color.Green);
                    }
                    else
                    {
                        f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                    }
                    First_Model.DP116_DBV_Setting(band);  //DBV Setting

                    for (gray = 0; gray < 10; gray++)
                    {
                        if (Dual_Gamma_Apply_and_Measure_Stop) break;

                        //Gray Pattern Setting
                        First_Model.DP116_Pattern_Setting(gray, band, false);

                        //Condition 1 Measure 
                        Condition = true; //Condition 1
                        Band_Radiobuttion_Select(band, Condition);//Select Band
                        Dual_Script_Apply(Condition);
                        Thread.Sleep(Measure_delay);
                        f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);

                        dataGridView_Band_OC_Viewer_1.Rows[(gray + 2)].Cells[4].Value = Measure.double_X;
                        dataGridView_Band_OC_Viewer_1.Rows[(gray + 2)].Cells[5].Value = Measure.double_Y;
                        dataGridView_Band_OC_Viewer_1.Rows[(gray + 2)].Cells[6].Value = Measure.double_Lv;
                        Updata_Sub_To_Main_GridView(band, gray, Condition);

                        //Condition 2 Measure
                        Condition = false; //Condition 2
                        Band_Radiobuttion_Select(band, Condition);//Select Band
                        Dual_Script_Apply(Condition);
                        Thread.Sleep(Measure_delay);
                        f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);
                        dataGridView_Band_OC_Viewer_2.Rows[(gray + 2)].Cells[4].Value = Measure.double_X;
                        dataGridView_Band_OC_Viewer_2.Rows[(gray + 2)].Cells[5].Value = Measure.double_Y;
                        dataGridView_Band_OC_Viewer_2.Rows[(gray + 2)].Cells[6].Value = Measure.double_Lv;
                        Updata_Sub_To_Main_GridView(band, gray, Condition);
                    }
                }
                else
                {
                    //next band
                }
            }
            System.Windows.MessageBox.Show("Dual Mode Gamma Apply and Measure finished");
        }

        private void button_Dual_Gamma_Apply_and_Measure_Stop_Click(object sender, EventArgs e)
        {
            Dual_Gamma_Apply_and_Measure_Stop = true;
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
