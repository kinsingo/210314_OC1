using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Serialization;

////using References
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using BSQH_Csharp_Library;
using PNC_Csharp.CA_Multi_Channels;


namespace PNC_Csharp.Measurement_10ch
{
    public partial class Optic_Measurement_10ch : Form
    {
        XmlSerializer mySerializer = new XmlSerializer(typeof(Multi_CH_Measurement_Preferences));//Used For Saving and Loading Setting

        // PNC 관련 변수
        bool[] check_SET = new bool[6];
        int[] SET_sequence = new int[6];
        bool[] check_CA_CH;
        DataGridView[] CH_Grid_View = new DataGridView[10];

        // GCS 관련 변수
        bool[] check_GCS_DBV = new bool[20];
        string[] value_GCS_DBV = new string[20];
        int GCS_check_count = 0;

        // BCS 관련 변수
        bool[] check_BCS_Gray = new bool[20];
        string[] value_BCS_Gray = new string[20];
        bool[] check_BCS_Range = new bool[3];
        string[] value_BCS_Range_min_DBV = new string[3];
        string[] value_BCS_Range_max_DBV = new string[3];
        string[] value_BCS_Range_DBV_step = new string[3];
        int BCS_check_count = 0;
        int BCS_range_check_count = 0;

        // AoD GCS 관련 변수
        bool[] check_AOD_GCS_DBV = new bool[3];
        string[] value_AOD_GCS_DBV = new string[3];
        int AOD_GCS_check_count = 0;

        // Gamma Crush 관련 변수
        bool[] check_Gamma_Crush = new bool[10];
        bool[] check_Gamma_Crush_color = new bool[4];
        string[] value_Gamma_Crush_DBV = new string[10];
        string[] value_Gamma_Crush_Gray = new string[10];
        int Gamma_Crush_check_count = 0;
        int Gamma_Crush_Color_check_count = 0;

        // IR Drop DeltaE 관련 변수
        Color[] IR_Drop_PTN = new Color[25];

        // deltaE 및 gamma 산출 변수
        XYLv[][][] data_list = new XYLv[][][] {
        new XYLv[10][], new XYLv[10][], new XYLv[10][], new XYLv[10][], new XYLv[10][], new XYLv[10][], };
        int[] index_list;
        int index_max;
        int count_measure;
        int data_index = 0;

        int row_count = 1;
        object[] first_line = new object[19];

        Color Color_Set1 = Color.FromArgb(255, 150, 150);
        Color Color_Set2 = Color.FromArgb(255, 200, 150);
        Color Color_Set3 = Color.FromArgb(175, 175, 255);
        Color Color_Set4 = Color.FromArgb(150, 200, 255);
        Color Color_Set5 = Color.FromArgb(40, 170, 160);
        Color Color_Set6 = Color.FromArgb(200, 255, 200);

        TextBox Textbox_Script_Set1_Final;
        TextBox Textbox_Script_Set2_Final;
        TextBox Textbox_Script_Set3_Final;
        TextBox Textbox_Script_Set4_Final;
        TextBox Textbox_Script_Set5_Final;
        TextBox Textbox_Script_Set6_Final;

        DateTime Start_Time;

        public void UpdateModelInfo()
        {
            label_model.Text = "Model : " + f1().current_model.Get_Current_Model_Name().ToString();
            label_Max_DBV.Text = "Max DBV : " + f1().current_model.get_DBV_Max().ToString() + " (0x" + f1().current_model.get_DBV_Max().ToString("X3") + ")";
            label_Size.Text = "Size : " + f1().current_model.get_X().ToString() + " * " + f1().current_model.get_Y().ToString();
        }

        public struct MinMax
        {
            public int Min;
            public int Max;
        }

        public struct DeltaE
        {
            public double X;
            public double Y;
            public double Z;

            public double Xn;
            public double Yn;
            public double Zn;

            public double fx;
            public double fy;
            public double fz;

            public double L;
            public double a;
            public double b;

            public double delta_a;
            public double delta_b;
            public double delta_L;

            public double delta_C;
            public double delta_E;
        }

        public struct RGB
        {
            public double data_R;
            public double data_G;
            public double data_B;
        }

        public bool GCS_measure = false;
        public bool BCS_measure = false;
        public bool AOD_GCS_measure = false;
        public bool IR_Drop_DeltaE_measure = false;
        public bool Gamma_Crush_measure = false;
        public bool aging_flag = true;
        public bool stop_flag = false;

        private static Optic_Measurement_10ch Instance;
        public static Optic_Measurement_10ch getInstance()
        {
            if (Instance == null)
                Instance = new Optic_Measurement_10ch();

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

        private bool Is_CA_Connected()
        {
            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];
            return ca_multi_ch_form.Check_Is_CA_Connected();
        }

        private I_CA_Control Get_ca_ctrl_obj()
        {
            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];
            return ca_multi_ch_form.Get_ca_ctrl_obj();
        }

        private bool[] Get_Is_CA_connected_channels()
        {
            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];
            return ca_multi_ch_form.Get_Is_CA_connected_channels();
        }

        private Optic_Measurement_10ch()
        {
            InitializeComponent();

            try
            {
                for (int i = 0; i < 19; i++)
                {
                    if (i == 0) first_line[i] = "/ DBV";
                    else if (i % 3 == 0) first_line[i] = "Lv";
                    else if (i % 3 == 1) first_line[i] = "x";
                    else if (i % 3 == 2) first_line[i] = "y";
                }

                CH_Grid_View[0] = dataGridView_CH1;
                CH_Grid_View[1] = dataGridView_CH2;
                CH_Grid_View[2] = dataGridView_CH3;
                CH_Grid_View[3] = dataGridView_CH4;
                CH_Grid_View[4] = dataGridView_CH5;
                CH_Grid_View[5] = dataGridView_CH6;
                CH_Grid_View[6] = dataGridView_CH7;
                CH_Grid_View[7] = dataGridView_CH8;
                CH_Grid_View[8] = dataGridView_CH9;
                CH_Grid_View[9] = dataGridView_CH10;

                CH_Grid_View_all_initial_setting();
                Button_Click_Enable(true);
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void Uri_Load(object sender, EventArgs e)
        {

        }

        private void MultiChannelCheckBoxEnable(bool able)
        {
            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];
            ca_multi_ch_form.Update_checkBox_MultiCAChannel_Enabled(able);
        }


        private void CH_Grid_View_all_initial_setting()
        {
            for (int ch = 0; ch < 10; ch++)
            {
                CH_Grid_View[ch].EnableHeadersVisualStyles = false;
                CH_Grid_View[ch].ReadOnly = true;
                CH_Grid_View[ch].Columns.Add("Gray", "Gray");
                CH_Grid_View[ch].Columns.Add("SET1_x", "");
                CH_Grid_View[ch].Columns.Add("SET1_y", "SET1");
                CH_Grid_View[ch].Columns.Add("SET1_Lv", "");
                CH_Grid_View[ch].Columns.Add("SET2_x", "");
                CH_Grid_View[ch].Columns.Add("SET2_y", "SET2");
                CH_Grid_View[ch].Columns.Add("SET2_Lv", "");
                CH_Grid_View[ch].Columns.Add("SET3_x", "");
                CH_Grid_View[ch].Columns.Add("SET3_y", "SET3");
                CH_Grid_View[ch].Columns.Add("SET3_Lv", "");
                CH_Grid_View[ch].Columns.Add("SET4_x", "");
                CH_Grid_View[ch].Columns.Add("SET4_y", "SET4");
                CH_Grid_View[ch].Columns.Add("SET4_Lv", "");
                CH_Grid_View[ch].Columns.Add("SET5_x", "");
                CH_Grid_View[ch].Columns.Add("SET5_y", "SET5");
                CH_Grid_View[ch].Columns.Add("SET5_Lv", "");
                CH_Grid_View[ch].Columns.Add("SET6_x", "");
                CH_Grid_View[ch].Columns.Add("SET6_y", "SET6");
                CH_Grid_View[ch].Columns.Add("SET6_Lv", "");

                CH_Grid_View[ch].Rows.Add(first_line);

                CH_Grid_View[ch].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                for (int col = 0; col < CH_Grid_View[ch].ColumnCount; col++)
                {
                    CH_Grid_View[ch].Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    CH_Grid_View[ch].Columns[col].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    if (col == 0)
                    {
                        CH_Grid_View[ch].Columns[col].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                        CH_Grid_View[ch].Columns[col].HeaderCell.Style.BackColor = System.Drawing.Color.LightGray;
                    }
                    else if (col < 4)
                    {
                        CH_Grid_View[ch].Columns[col].DefaultCellStyle.BackColor = this.Color_Set1;
                        CH_Grid_View[ch].Columns[col].HeaderCell.Style.BackColor = this.Color_Set1;
                    }
                    else if (col < 7)
                    {
                        CH_Grid_View[ch].Columns[col].DefaultCellStyle.BackColor = this.Color_Set2;
                        CH_Grid_View[ch].Columns[col].HeaderCell.Style.BackColor = this.Color_Set2;
                    }
                    else if (col < 10)
                    {
                        CH_Grid_View[ch].Columns[col].DefaultCellStyle.BackColor = this.Color_Set3;
                        CH_Grid_View[ch].Columns[col].HeaderCell.Style.BackColor = this.Color_Set3;
                    }
                    else if (col < 13)
                    {
                        CH_Grid_View[ch].Columns[col].DefaultCellStyle.BackColor = this.Color_Set4;
                        CH_Grid_View[ch].Columns[col].HeaderCell.Style.BackColor = this.Color_Set4;
                    }
                    else if (col < 16)
                    {
                        CH_Grid_View[ch].Columns[col].DefaultCellStyle.BackColor = this.Color_Set5;
                        CH_Grid_View[ch].Columns[col].HeaderCell.Style.BackColor = this.Color_Set5;
                    }
                    else if (col < 19)
                    {
                        CH_Grid_View[ch].Columns[col].DefaultCellStyle.BackColor = this.Color_Set6;
                        CH_Grid_View[ch].Columns[col].HeaderCell.Style.BackColor = this.Color_Set6;
                    }

                    CH_Grid_View[ch].Columns[col].Width = 50;
                }
                foreach (DataGridViewColumn column in this.CH_Grid_View[ch].Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Button_Hide_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void radioButton_CA310_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton_CA410_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btn_CA_Connect_Click(object sender, EventArgs e)
        {

        }

  
        

        private void PNC_CH_check()
        {
            check_CA_CH = Get_Is_CA_connected_channels();
            PNC_CH_Label_update();
        }


        private void PNC_CH_Label_update()
        {
            for (int ch = 0; ch < 10; ch++)
            {
                if (check_CA_CH[ch])
                {
                    switch (ch)
                    {
                        case 0:
                            label_PNC_CH1.Text = "PNC CH1" + " - CA #" + ch;
                            label_PNC_CH1.ForeColor = Color.YellowGreen;
                            break;
                        case 1:
                            label_PNC_CH2.Text = "PNC CH2" + " - CA #" + ch;
                            label_PNC_CH2.ForeColor = Color.YellowGreen;
                            break;
                        case 2:
                            label_PNC_CH3.Text = "PNC CH3" + " - CA #" + ch;
                            label_PNC_CH3.ForeColor = Color.YellowGreen;
                            break;
                        case 3:
                            label_PNC_CH4.Text = "PNC CH4" + " - CA #" + ch;
                            label_PNC_CH4.ForeColor = Color.YellowGreen;
                            break;
                        case 4:
                            label_PNC_CH5.Text = "PNC CH5" + " - CA #" + ch;
                            label_PNC_CH5.ForeColor = Color.YellowGreen;
                            break;
                        case 5:
                            label_PNC_CH6.Text = "PNC CH6" + " - CA #" + ch;
                            label_PNC_CH6.ForeColor = Color.YellowGreen;
                            break;
                        case 6:
                            label_PNC_CH7.Text = "PNC CH7" + " - CA #" + ch;
                            label_PNC_CH7.ForeColor = Color.YellowGreen;
                            break;
                        case 7:
                            label_PNC_CH8.Text = "PNC CH8" + " - CA #" + ch;
                            label_PNC_CH8.ForeColor = Color.YellowGreen;
                            break;
                        case 8:
                            label_PNC_CH9.Text = "PNC CH9" + " - CA #" + ch;
                            label_PNC_CH9.ForeColor = Color.YellowGreen;
                            break;
                        case 9:
                            label_PNC_CH10.Text = "PNC CH10" + " - CA #" + ch;
                            label_PNC_CH10.ForeColor = Color.LimeGreen;
                            break;
                    }
                }
                else
                {
                    switch (ch)
                    {
                        case 0:
                            label_PNC_CH1.ForeColor = Color.DarkGray;
                            break;
                        case 1:
                            label_PNC_CH2.ForeColor = Color.DarkGray;
                            break;
                        case 2:
                            label_PNC_CH3.ForeColor = Color.DarkGray;
                            break;
                        case 3:
                            label_PNC_CH4.ForeColor = Color.DarkGray;
                            break;
                        case 4:
                            label_PNC_CH5.ForeColor = Color.DarkGray;
                            break;
                        case 5:
                            label_PNC_CH6.ForeColor = Color.DarkGray;
                            break;
                        case 6:
                            label_PNC_CH7.ForeColor = Color.DarkGray;
                            break;
                        case 7:
                            label_PNC_CH8.ForeColor = Color.DarkGray;
                            break;
                        case 8:
                            label_PNC_CH9.ForeColor = Color.DarkGray;
                            break;
                        case 9:
                            label_PNC_CH10.ForeColor = Color.DarkGray;
                            break;
                    }
                }
            }
        }

        private void PNC_SET_check()
        {
            int[] seq_temp = new int[6];
            
            //Textbox to String
            seq_temp[0] = Convert.ToInt32(SEQ_SET1.Text);
            seq_temp[1] = Convert.ToInt32(SEQ_SET2.Text);
            seq_temp[2] = Convert.ToInt32(SEQ_SET3.Text);
            seq_temp[3] = Convert.ToInt32(SEQ_SET4.Text);
            seq_temp[4] = Convert.ToInt32(SEQ_SET5.Text);
            seq_temp[5] = Convert.ToInt32(SEQ_SET6.Text);

            for (int seq = 0; seq < 6; seq++)
            {
                switch (seq_temp[seq]-1)
                {
                    case 0: SET_sequence[seq_temp[seq] - 1] = seq; break;
                    case 1: SET_sequence[seq_temp[seq] - 1] = seq; break;
                    case 2: SET_sequence[seq_temp[seq] - 1] = seq; break;
                    case 3: SET_sequence[seq_temp[seq] - 1] = seq; break;
                    case 4: SET_sequence[seq_temp[seq] - 1] = seq; break;
                    case 5: SET_sequence[seq_temp[seq] - 1] = seq; break;
                }
            }

            //CheckBox to Bool
            check_SET[0] = check_SET1.Checked;
            check_SET[1] = check_SET2.Checked;
            check_SET[2] = check_SET3.Checked;
            check_SET[3] = check_SET4.Checked;
            check_SET[4] = check_SET5.Checked;
            check_SET[5] = check_SET6.Checked;
        }

        private void Set_change_Measure()
        {

            if(Is_CA_Connected() == false)
            {
                MessageBox.Show("Multi channel CA is not connected yet, please check CA status first");
                return;
            }


            int SET_delay = Convert.ToInt16(Set_Change_Delay.Text);

            // Set 변환
            for (int seq = 0; seq < 6; seq++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_SET[SET_sequence[seq]])
                {
                    PNC_SET_Script_Send(SET_sequence[seq]);

                    Thread.Sleep(SET_delay);

                    XYLv[] measurement = Get_ca_ctrl_obj().Get_Multi_MeasuredData();
                    // Data update

                    for (int ch = 0; ch < 10; ch++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        if (check_CA_CH[ch])
                        {
                            PNC_CH_Data_Update(ch, SET_sequence[seq], ch, measurement);
                        }
                    }
                }
            }
        }

        private void Set_non_change_Measure()
        {
            if (Is_CA_Connected() == false)
            {
                MessageBox.Show("Multi channel CA is not connected yet, please check CA status first");
                return;
            }

            int SET_delay = Convert.ToInt16(Set_Change_Delay.Text);
            XYLv[] measurement = Get_ca_ctrl_obj().Get_Multi_MeasuredData();

            // Data update
            for (int ch = 0; ch < 10; ch++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_CA_CH[ch])
                    PNC_CH_Data_Update(ch, set : 0,CA_ch : ch, measurement);
                
            }
        }

        private void PNC_SET_Script_Send(int set)
        {
            TextBox TextBox_script_data = null;

            PNC_Script_Transform(set);

            switch (set)
            {
                case 0:
                    TextBox_script_data = Textbox_Script_Set1_Final;
                    break;
                case 1:
                    TextBox_script_data = Textbox_Script_Set2_Final;
                    break;
                case 2:
                    TextBox_script_data = Textbox_Script_Set3_Final;
                    break;
                case 3:
                    TextBox_script_data = Textbox_Script_Set4_Final;
                    break;
                case 4:
                    TextBox_script_data = Textbox_Script_Set5_Final;
                    break;
                case 5:
                    TextBox_script_data = Textbox_Script_Set6_Final;
                    break;
            }

            for (int i = 0; i < TextBox_script_data.Lines.Length - 1; i++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (TextBox_script_data.Lines[i].Length >= 10
                    && TextBox_script_data.Lines[i].Substring(0, 10) == "mipi.write")
                {
                    f1().IPC_Quick_Send(TextBox_script_data.Lines[i]);
                }
                else if (TextBox_script_data.Lines[i].Length >= 5 && (
                    TextBox_script_data.Lines[i].Substring(0, 5) == "delay"
                    || TextBox_script_data.Lines[i].Substring(0, 5) == "image"))
                {
                    f1().IPC_Quick_Send(TextBox_script_data.Lines[i]);
                }
                else if (TextBox_script_data.Lines[i].Substring(0, 14) == "gpio.i2c.write")
                {
                    f1().IPC_Quick_Send(TextBox_script_data.Lines[i]);
                }
            }
        }
        private void PNC_Script_Transform(int set)
        {
            string temp_Mipi_Data_String = string.Empty;
            int count_mipi_cmd = 0;
            int count_one_mipi_cmd_length = 0;
            bool Flag = false;
            TextBox Textbox_transform_before = textBox_script_copy_temp;
            TextBox Textbox_transform_after = textBox_script_copy_temp;

            Textbox_transform_before.Clear();
            Textbox_transform_after.Clear();

            switch (set)
            {
                case 0:
                    Textbox_transform_before = textBox_Script_SET1;
                    break;
                case 1:
                    Textbox_transform_before = textBox_Script_SET2;
                    break;
                case 2:
                    Textbox_transform_before = textBox_Script_SET3;
                    break;
                case 3:
                    Textbox_transform_before = textBox_Script_SET4;
                    break;
                case 4:
                    Textbox_transform_before = textBox_Script_SET5;
                    break;
                case 5:
                    Textbox_transform_before = textBox_Script_SET6;
                    break;
            }

            //Delete others except for Mipi CMDs and Write on the 2nd Textbox
            for (int i = 0; i < Textbox_transform_before.Lines.Length; i++)
            {
                if (Textbox_transform_before.Lines[i].Length >= 20) // mipi.write 0xXX 0xXX <-- 20ea Character
                {
                    if (Textbox_transform_before.Lines[i].Substring(0, 10) == "mipi.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 10; k < Textbox_transform_before.Lines[i].Length; k++)
                        {
                            if (Textbox_transform_before.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && Textbox_transform_before.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        Textbox_transform_after.Text += temp_Mipi_Data_String + "\r\n";
                    }
                    else if (Textbox_transform_before.Lines[i].Substring(0, 14) == "gpio.i2c.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 14; k < Textbox_transform_before.Lines[i].Length; k++)
                        {
                            if (Textbox_transform_before.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && Textbox_transform_before.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 14 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        Textbox_transform_after.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else
                    {
                        // It's not a "mipi.write" of "delay" command , do nothing 
                    }
                }

                //Delay
                else if (Textbox_transform_before.Lines[i].Length >= 5
                    && Textbox_transform_before.Lines[i].Substring(0, 5) != "     ")
                {
                    if (Textbox_transform_before.Lines[i].Substring(0, 5) == "delay")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < Textbox_transform_before.Lines[i].Length; k++)
                        {
                            if (Textbox_transform_before.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && Textbox_transform_before.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        Textbox_transform_after.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else if (Textbox_transform_before.Lines[i].Substring(0, 5) == "image")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < Textbox_transform_before.Lines[i].Length; k++)
                        {
                            if (Textbox_transform_before.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && Textbox_transform_before.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = Textbox_transform_before.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        Textbox_transform_after.Text += temp_Mipi_Data_String + "\r\n";
                    }
                }
            }

            switch (set)
            {
                case 0:
                    Textbox_Script_Set1_Final = Textbox_transform_after;
                    break;
                case 1:
                    Textbox_Script_Set2_Final = Textbox_transform_after;
                    break;
                case 2:
                    Textbox_Script_Set3_Final = Textbox_transform_after;
                    break;
                case 3:
                    Textbox_Script_Set4_Final = Textbox_transform_after;
                    break;
                case 4:
                    Textbox_Script_Set5_Final = Textbox_transform_after;
                    break;
                case 5:
                    Textbox_Script_Set6_Final = Textbox_transform_after;
                    break;
            }
        }
        private void PNC_CH_Data_Update(int PNC_ch, int set, int CA_ch, XYLv[] measurement)
        {
            CH_Grid_View[PNC_ch].Rows[row_count - 1].Cells[set * 3 + 1].Value = measurement[CA_ch].double_X.ToString("0.0000");
            CH_Grid_View[PNC_ch].Rows[row_count - 1].Cells[set * 3 + 2].Value = measurement[CA_ch].double_Y.ToString("0.0000");
            CH_Grid_View[PNC_ch].Rows[row_count - 1].Cells[set * 3 + 3].Value = measurement[CA_ch].double_Lv.ToString();
        }

        private void save_GCS_data(int number, string DBV, StreamWriter sw)
        {
            double gamma_data = 0;
            string log_data;

            DeltaE calc_data;
            DeltaE max_data;

            label_mornitoring.Text = "Calculate 'Gamma & DeltaE3' and save the data of GCS" + (number + 1).ToString() + "";
            label_mornitoring.Text = "Calculate 'Gamma & DeltaE3' and save the data of GCS" + (number + 1).ToString() + "";

            if (number != 0) sw.WriteLine("-");

            for (int index = 0; index < count_measure; index++)
            {
                log_data = "";
                log_data = log_data + index.ToString() + "\t" + DBV + "\t" + index_list[index] + "\t";
                for (int ch = 0; ch < 10; ch++)
                {
                    for (int set = 0; set < 6; set++)
                    {
                        if (check_CA_CH[ch] && check_SET[set])
                        {
                            log_data = log_data + data_list[set][ch][index].double_X.ToString() + "\t"; // x
                            log_data = log_data + data_list[set][ch][index].double_Y.ToString() + "\t"; // y
                            log_data = log_data + data_list[set][ch][index].double_Lv.ToString() + "\t";    // Lv

                            max_data = calculate_DeltaE_max(set, ch);
                            calc_data = calculate_DeltaE(set, ch, index, max_data);

                            log_data = log_data + calc_data.delta_C.ToString() + "\t";  // △E3

                            gamma_data = Math.Log(data_list[set][ch][index].double_Lv / data_list[set][ch][index_max].double_Lv) / Math.Log(Convert.ToDouble(index_list[index]) / Convert.ToDouble(255));

                            log_data = log_data + gamma_data.ToString() + "\t";  // Gamma

                            //if ((index_list[index] > 240) || (index_list[index] < 17))
                            //{
                            //    log_data = log_data + "-" + "\t";  // Gamma
                            //}
                            //else
                            //{
                            //    log_data = log_data + gamma_data.ToString() + "\t";  // Gamma
                            //}
                        }
                        else
                        {
                            log_data += "\t";
                            log_data += "\t";
                            log_data += "\t";
                            log_data += "\t";
                            log_data += "\t";
                        }
                            
                    }
                }
                sw.WriteLine(log_data);
            }
        }
        private void save_BCS_data(int ragne, int number, int gray, StreamWriter sw)
        {
            double accuracy = 0;
            string log_data;

            DeltaE calc_data;
            DeltaE max_data;

            label_mornitoring.Text = "Calculate 'delta E & accuracy' and save the data of BCS" + (number + 1).ToString() + "";
            label_mornitoring.Text = "Calculate 'delta E & accuracy' and save the data of BCS" + (number + 1).ToString() + "";

            if(number!=0)  sw.WriteLine("-");

            for (int index = 0; index < count_measure; index++)
            {
                log_data = "";
                log_data = log_data + "range" + ragne.ToString() + "\t" +  index.ToString() + "\t" + gray.ToString() + "\t" + index_list[index] + "\t";
                for (int ch = 0; ch < 10; ch++)
                {
                    for (int set = 0; set < 6; set++)
                    {
                        if (check_CA_CH[ch] && check_SET[set])
                        {
                            log_data = log_data + data_list[set][ch][index].double_X.ToString() + "\t"; // x
                            log_data = log_data + data_list[set][ch][index].double_Y.ToString() + "\t"; // y
                            log_data = log_data + data_list[set][ch][index].double_Lv.ToString() + "\t";    // Lv

                            max_data = calculate_DeltaE_max(set, ch);
                            calc_data = calculate_DeltaE(set, ch, index, max_data);

                            log_data = log_data + calc_data.delta_C.ToString() + "\t";  // △E2

                            if (index == index_max)
                            {
                                log_data = log_data + "-" + "\t";  // Accuracy
                            }
                            else
                            {
                                if (index_list[index] < index_list[index - 1]) accuracy = Math.Abs((data_list[set][ch][index].double_Lv - data_list[set][ch][index - 1].double_Lv) / data_list[set][ch][index].double_Lv) * 100;
                                else accuracy = Math.Abs((data_list[set][ch][index].double_Lv - data_list[set][ch][index - 1].double_Lv) / data_list[set][ch][index - 1].double_Lv) * 100;
                                log_data = log_data + accuracy.ToString() + "\t";  // Accuracy
                            }
                        }
                        else
                        {
                            log_data += "\t";
                            log_data += "\t";
                            log_data += "\t";
                            log_data += "\t";
                            log_data += "\t";
                        }

                    }
                }
                sw.WriteLine(log_data);
            }
        }
        private void save_AOD_GCS_data(int number, string DBV, StreamWriter sw)
        {
            double gamma_data = 0;
            string log_data;

            DeltaE calc_data;
            DeltaE max_data;

            label_mornitoring.Text = "Calculate 'Gamma & DeltaE3' and save the data of AOD_GCS" + (number + 1).ToString() + "";
            label_mornitoring.Text = "Calculate 'Gamma & DeltaE3' and save the data of AOD_GCS" + (number + 1).ToString() + "";

            if (number != 0) sw.WriteLine("-");

            for (int index = 0; index < count_measure; index++)
            {
                log_data = "";
                log_data = log_data + index.ToString() + "\t" + DBV + "\t" + index_list[index] + "\t";
                for (int ch = 0; ch < 10; ch++)
                {
                    if (check_CA_CH[ch])
                    {
                        log_data = log_data + data_list[0][ch][index].double_X.ToString() + "\t"; // x
                        log_data = log_data + data_list[0][ch][index].double_Y.ToString() + "\t"; // y
                        log_data = log_data + data_list[0][ch][index].double_Lv.ToString() + "\t";    // Lv

                        max_data = calculate_DeltaE_max(0, ch);
                        calc_data = calculate_DeltaE(0, ch, index, max_data);

                        log_data = log_data + calc_data.delta_C.ToString() + "\t";  // △E3

                        gamma_data = Math.Log(data_list[0][ch][index].double_Lv / data_list[0][ch][index_max].double_Lv) / Math.Log(Convert.ToDouble(index_list[index]) / Convert.ToDouble(255));

                        log_data = log_data + gamma_data.ToString() + "\t";  // Gamma

                        //if ((index_list[index] > 240) || (index_list[index] < 17))
                        //{
                        //    log_data = log_data + "-" + "\t";  // Gamma
                        //}
                        //else
                        //{
                        //    log_data = log_data + gamma_data.ToString() + "\t";  // Gamma
                        //}
                    }
                    else
                    {
                        log_data += "\t";
                        log_data += "\t";
                        log_data += "\t";
                        log_data += "\t";
                        log_data += "\t";
                    }
                }
                sw.WriteLine(log_data);
            }
        }
        private void save_Gamma_Crush(int number, string DBV, int gray, StreamWriter sw)
        {
            string log_data;

            RGB gamma_crush;

            label_mornitoring.Text = "Calculate 'Gamma Crush' and save the data of Setting" + (number + 1).ToString() + "";
            label_mornitoring.Text = "Calculate 'Gamma Crush' and save the data of Setting" + (number + 1).ToString() + "";

            for (int ch = 0; ch < 10; ch++)
            {
                if (check_CA_CH[ch])
                {
                    log_data = "";
                    log_data = log_data + "#" + (ch + 1).ToString() + "\t" + DBV + "\t" + gray.ToString() + "\t";
                    for (int set = 0; set < 6; set++)
                    {
                        if (check_SET[set])
                        {
                            for (int index = 0; index < count_measure; index++)
                            {
                                log_data = log_data + data_list[set][ch][index].double_X.ToString() + "\t"; // x
                                log_data = log_data + data_list[set][ch][index].double_Y.ToString() + "\t"; // y
                                log_data = log_data + data_list[set][ch][index].double_Lv.ToString() + "\t";    // Lv
                            }
                            gamma_crush = calcultae_Gamma_Crush(set, ch);

                            log_data = log_data + gamma_crush.data_R + "\t" + gamma_crush.data_G + "\t" + gamma_crush.data_B + "\t";
                        }
                        else
                        {
                            for (int index = 0; index < count_measure; index++)
                            {
                                log_data += "\t";
                                log_data += "\t";
                                log_data += "\t";
                            }
                            log_data += "\t";
                        }
                    }
                    sw.WriteLine(log_data);
                }
            }
        }
        private void save_IR_Drop_DeltaE(string set, string DBV, StreamWriter sw)
        {
            string log_data;

            DeltaE data_Ref;
            DeltaE data_full;
            DeltaE data_APL;

            double delta_L;
            double delta_a;
            double delta_b;
            double delta_E;
            double[] delta_E_sum = new double[10];
            double[] delta_E_avg = new double[10];

            label_mornitoring.Text = "Calculate 'IR Drop delta E' and save the data";
            label_mornitoring.Text = "Calculate 'IR Drop delta E' and save the data";

            for (int ch = 0; ch < 10; ch++)
            {
                delta_E_sum[ch] = 0;
            }

            for (int i = 0; i < 25; i++)
            {
                log_data = "";
                log_data = log_data + (i + 1).ToString() + "\t" + set.ToString() + "\t" + DBV + "\t" + IR_Drop_PTN[i].R.ToString() + "\t" + IR_Drop_PTN[i].G.ToString() + "\t" + IR_Drop_PTN[i].B.ToString() + "\t";
                for (int ch = 0; ch < 10; ch++)
                {
                    if (check_CA_CH[ch])
                    {
                        log_data = log_data + data_list[0][ch][2 * i].double_X + "\t";  // full PTN x
                        log_data = log_data + data_list[0][ch][2 * i].double_Y + "\t";  // full PTN y
                        log_data = log_data + data_list[0][ch][2 * i].double_Lv + "\t";  // full PTN Lv

                        log_data = log_data + data_list[0][ch][2 * i + 1].double_X + "\t";  // 30% PTN x
                        log_data = log_data + data_list[0][ch][2 * i + 1].double_Y + "\t";  // 30% PTN y
                        log_data = log_data + data_list[0][ch][2 * i + 1].double_Lv + "\t";  // 30% PTN Lv

                        data_Ref = calculate_DeltaE_max(0, ch);
                        data_full = calculate_DeltaE(0, ch, 2 * i, data_Ref);
                        data_APL = calculate_DeltaE(0, ch, 2 * i + 1, data_Ref);

                        delta_L = data_full.L - data_APL.L;
                        delta_a = data_full.a - data_APL.a;
                        delta_b = data_full.b - data_APL.b;

                        delta_E = Math.Sqrt(Math.Pow(delta_L, 2) + Math.Pow(delta_a, 2) + Math.Pow(delta_b, 2));

                        log_data = log_data + delta_E.ToString() + "\t";  // 30% PTN Lv

                        delta_E_sum[ch] += delta_E;
                        delta_E_avg[ch] = delta_E_sum[ch] / (i + 1);
                    }
                    else
                    {
                        for (int skip = 0; skip < 7; skip++)
                        {
                            log_data += "\t";
                        }
                    }
                }

                sw.WriteLine(log_data);
            }

            log_data = "\t" + "\t" + "\t" + "\t" + "\t" + "\t";

            for (int ch = 0; ch < 10; ch++)
            {
                log_data += "\t";
                log_data += "\t";
                log_data += "\t";
                log_data += "\t";
                log_data += "\t";
                log_data += "\t";
                if (check_CA_CH[ch]) log_data = log_data + delta_E_avg[ch] + "\t";
                else log_data += "\t";
            }
            sw.WriteLine(log_data);
        }

        private DeltaE calculate_DeltaE_max(int set, int ch)
        {
            DeltaE calc_data;

            calc_data.X = data_list[set][ch][index_max].double_X / data_list[set][ch][index_max].double_Y * data_list[set][ch][index_max].double_Lv;
            calc_data.Y = data_list[set][ch][index_max].double_Lv;
            calc_data.Z = (1 - data_list[set][ch][index_max].double_X - data_list[set][ch][index_max].double_Y) / data_list[set][ch][index_max].double_Y * data_list[set][ch][index_max].double_Lv;

            calc_data.Xn = calc_data.X / calc_data.X;
            calc_data.Yn = calc_data.Y / calc_data.Y;
            calc_data.Zn = calc_data.Z / calc_data.Z;

            if (calc_data.Xn > 0.008856) calc_data.fx = Math.Pow(calc_data.Xn, Convert.ToDouble(1) / Convert.ToDouble(3));
            else calc_data.fx = 7.787 * calc_data.Xn + (Convert.ToDouble(16) / Convert.ToDouble(116));
            if (calc_data.Yn > 0.008856) calc_data.fy = Math.Pow(calc_data.Yn, Convert.ToDouble(1) / Convert.ToDouble(3));
            else calc_data.fy = 7.787 * calc_data.Yn + (Convert.ToDouble(16) / Convert.ToDouble(116));
            if (calc_data.Zn > 0.008856) calc_data.fz = Math.Pow(calc_data.Zn, Convert.ToDouble(1) / Convert.ToDouble(3));
            else calc_data.fz = 7.787 * calc_data.Zn + (Convert.ToDouble(16) / Convert.ToDouble(116));

            if (calc_data.Yn > 0.008856) calc_data.L = 116 * Math.Pow(calc_data.Yn, Convert.ToDouble(1) / Convert.ToDouble(3)) - 16;
            else calc_data.L = 903.3 * calc_data.Yn;
            calc_data.a = 500 * (calc_data.fx - calc_data.fy);
            calc_data.b = 200 * (calc_data.fy - calc_data.fz);

            calc_data.delta_a = calc_data.a - calc_data.a;
            calc_data.delta_b = calc_data.b - calc_data.b;
            calc_data.delta_L = calc_data.L - calc_data.L;

            calc_data.delta_C = Math.Sqrt(Math.Pow(calc_data.delta_a,2) + Math.Pow(calc_data.delta_b,2));
            calc_data.delta_E = Math.Sqrt(Math.Pow(calc_data.delta_L, 2) + Math.Pow(calc_data.delta_C, 2));

            return calc_data;
        }
        private DeltaE calculate_DeltaE(int set, int ch, int index, DeltaE ref_data)
        {
            DeltaE calc_data;

            calc_data.X = data_list[set][ch][index].double_X / data_list[set][ch][index].double_Y * data_list[set][ch][index].double_Lv;
            calc_data.Y = data_list[set][ch][index].double_Lv;
            calc_data.Z = (1 - data_list[set][ch][index].double_X - data_list[set][ch][index].double_Y) / data_list[set][ch][index].double_Y * data_list[set][ch][index].double_Lv;

            calc_data.Xn = calc_data.X / ref_data.X;
            calc_data.Yn = calc_data.Y / ref_data.Y;
            calc_data.Zn = calc_data.Z / ref_data.Z;

            if (calc_data.Xn > 0.008856) calc_data.fx = Math.Pow(calc_data.Xn, Convert.ToDouble(1) / Convert.ToDouble(3));
            else calc_data.fx = 7.787 * calc_data.Xn + (Convert.ToDouble(16) / Convert.ToDouble(116));
            if (calc_data.Yn > 0.008856) calc_data.fy = Math.Pow(calc_data.Yn, Convert.ToDouble(1) / Convert.ToDouble(3));
            else calc_data.fy = 7.787 * calc_data.Yn + (Convert.ToDouble(16) / Convert.ToDouble(116));
            if (calc_data.Zn > 0.008856) calc_data.fz = Math.Pow(calc_data.Zn, Convert.ToDouble(1) / Convert.ToDouble(3));
            else calc_data.fz = 7.787 * calc_data.Zn + (Convert.ToDouble(16) / Convert.ToDouble(116));

            if (calc_data.Yn > 0.008856) calc_data.L = 116 * Math.Pow(calc_data.Yn, Convert.ToDouble(1) / Convert.ToDouble(3)) - 16;
            else calc_data.L = 903.3 * calc_data.Yn;
            calc_data.a = 500 * (calc_data.fx - calc_data.fy);
            calc_data.b = 200 * (calc_data.fy - calc_data.fz);

            calc_data.delta_a = calc_data.a - ref_data.a;
            calc_data.delta_b = calc_data.b - ref_data.b;
            calc_data.delta_L = calc_data.L - ref_data.L;

            calc_data.delta_C = Math.Sqrt(Math.Pow(calc_data.delta_a, 2) + Math.Pow(calc_data.delta_b, 2));
            calc_data.delta_E = Math.Sqrt(Math.Pow(calc_data.delta_L, 2) + Math.Pow(calc_data.delta_C, 2));

            return calc_data;
        }
        private RGB calcultae_Gamma_Crush(int set, int ch)
        {
            RGB ratio;
            RGB target_Lv;
            RGB gamma_crush;

            double Wx = data_list[set][ch][0].double_X;
            double Wy = data_list[set][ch][0].double_Y;
            double WLV = data_list[set][ch][0].double_Lv;

            double Rx = data_list[set][ch][1].double_X;
            double Ry = data_list[set][ch][1].double_Y;
            double RLV = data_list[set][ch][1].double_Lv;

            double Gx = data_list[set][ch][2].double_X;
            double Gy = data_list[set][ch][2].double_Y;
            double GLV = data_list[set][ch][2].double_Lv;

            double Bx = data_list[set][ch][3].double_X;
            double By = data_list[set][ch][3].double_Y;
            double BLV = data_list[set][ch][3].double_Lv;

            ratio.data_G = 1;
            ratio.data_B = (By * (Wy * (Gx - Rx) + Ry * (Wx - Gx) + Gy * (Rx - Wx))) / (Gy * (Wy * (Rx - Bx) + By * (Wx - Rx) + Ry * (Bx - Wx)));
            ratio.data_R = ((Wx - Bx) * Ry * ratio.data_B) / (By * (Rx - Wx)) + (Ry * (Wx - Gx)) / (Gy * (Rx - Wx));

            target_Lv.data_R = WLV * (ratio.data_R / (ratio.data_R + ratio.data_G + ratio.data_B));
            target_Lv.data_G = WLV * (ratio.data_G / (ratio.data_R + ratio.data_G + ratio.data_B));
            target_Lv.data_B = WLV * (ratio.data_B / (ratio.data_R + ratio.data_G + ratio.data_B));

            gamma_crush.data_R = RLV / target_Lv.data_R * 100;
            gamma_crush.data_G = GLV / target_Lv.data_G * 100;
            gamma_crush.data_B = BLV / target_Lv.data_B * 100;

            return gamma_crush;
        }

        private void GCS_DBV_check()
        {
            GCS_check_count = 0;

            //Textbox to String
            value_GCS_DBV[0] = GCS_DBV1.Text;
            value_GCS_DBV[1] = GCS_DBV2.Text;
            value_GCS_DBV[2] = GCS_DBV3.Text;
            value_GCS_DBV[3] = GCS_DBV4.Text;
            value_GCS_DBV[4] = GCS_DBV5.Text;
            value_GCS_DBV[5] = GCS_DBV6.Text;
            value_GCS_DBV[6] = GCS_DBV7.Text;
            value_GCS_DBV[7] = GCS_DBV8.Text;
            value_GCS_DBV[8] = GCS_DBV9.Text;
            value_GCS_DBV[9] = GCS_DBV10.Text;
            value_GCS_DBV[10] = GCS_DBV11.Text;
            value_GCS_DBV[11] = GCS_DBV12.Text;
            value_GCS_DBV[12] = GCS_DBV13.Text;
            value_GCS_DBV[13] = GCS_DBV14.Text;
            value_GCS_DBV[14] = GCS_DBV15.Text;
            value_GCS_DBV[15] = GCS_DBV16.Text;
            value_GCS_DBV[16] = GCS_DBV17.Text;
            value_GCS_DBV[17] = GCS_DBV18.Text;
            value_GCS_DBV[18] = GCS_DBV19.Text;
            value_GCS_DBV[19] = GCS_DBV20.Text;

            //CheckBox to Bool
            check_GCS_DBV[0] = check_GCS_DBV1.Checked;
            check_GCS_DBV[1] = check_GCS_DBV2.Checked;
            check_GCS_DBV[2] = check_GCS_DBV3.Checked;
            check_GCS_DBV[3] = check_GCS_DBV4.Checked;
            check_GCS_DBV[4] = check_GCS_DBV5.Checked;
            check_GCS_DBV[5] = check_GCS_DBV6.Checked;
            check_GCS_DBV[6] = check_GCS_DBV7.Checked;
            check_GCS_DBV[7] = check_GCS_DBV8.Checked;
            check_GCS_DBV[8] = check_GCS_DBV9.Checked;
            check_GCS_DBV[9] = check_GCS_DBV10.Checked;
            check_GCS_DBV[10] = check_GCS_DBV11.Checked;
            check_GCS_DBV[11] = check_GCS_DBV12.Checked;
            check_GCS_DBV[12] = check_GCS_DBV13.Checked;
            check_GCS_DBV[13] = check_GCS_DBV14.Checked;
            check_GCS_DBV[14] = check_GCS_DBV15.Checked;
            check_GCS_DBV[15] = check_GCS_DBV16.Checked;
            check_GCS_DBV[16] = check_GCS_DBV17.Checked;
            check_GCS_DBV[17] = check_GCS_DBV18.Checked;
            check_GCS_DBV[18] = check_GCS_DBV19.Checked;
            check_GCS_DBV[19] = check_GCS_DBV20.Checked;

            for (int gcs = 0; gcs < 10; gcs++)
            {
                if (check_GCS_DBV[gcs]) GCS_check_count++;
            }

            if (GCS_check_count == 0) GCS_measure = false;
        }
        private void GCS_All_DBV_Status(bool Checked)
        {
            check_GCS_DBV1.Checked = Checked;
            check_GCS_DBV2.Checked = Checked;
            check_GCS_DBV3.Checked = Checked;
            check_GCS_DBV4.Checked = Checked;
            check_GCS_DBV5.Checked = Checked;
            check_GCS_DBV6.Checked = Checked;
            check_GCS_DBV7.Checked = Checked;
            check_GCS_DBV8.Checked = Checked;
            check_GCS_DBV9.Checked = Checked;
            check_GCS_DBV10.Checked = Checked;
            check_GCS_DBV11.Checked = Checked;
            check_GCS_DBV12.Checked = Checked;
            check_GCS_DBV13.Checked = Checked;
            check_GCS_DBV14.Checked = Checked;
            check_GCS_DBV15.Checked = Checked;
            check_GCS_DBV16.Checked = Checked;
            check_GCS_DBV17.Checked = Checked;
            check_GCS_DBV18.Checked = Checked;
            check_GCS_DBV19.Checked = Checked;
            check_GCS_DBV20.Checked = Checked;
        }
        private void button_GCS_DBV_all_select_Click(object sender, EventArgs e)
        {
            GCS_All_DBV_Status(true);
        }
        private void button_GCS_DBV_all_deselect_Click(object sender, EventArgs e)
        {
            GCS_All_DBV_Status(false);
        }
        private MinMax GCS_Min_Max_check(int max_gray, int min_gray)
        {
            MinMax GCS_MinMax = new MinMax();

            GCS_MinMax.Max = max_gray;
            GCS_MinMax.Min = min_gray;

            if (max_gray > 255)
            {
                GCS_max_gray.Text = "255";
                max_gray = 255;
                GCS_MinMax.Max = 255;
            }
            if (min_gray < 0)
            {
                GCS_min_gray.Text = "0";
                min_gray = 0;
                GCS_MinMax.Min = 0;
            }
            return GCS_MinMax;
        }
        private void GCS_Measure()
        {
            label_mornitoring.Text = "Open file for saving GCS measurement data";
           
            ///////////////////// Log 저장을 위한 file setting
            string FileName_load = Directory.GetCurrentDirectory() + "\\Optic_Measurement_Data(10ch)\\format\\GCS_format.csv";
            string FileName_save = Folder_Control.Make_new_folder(Start_Time) + "\\GCS_" + Start_Time.ToString(@"yyyy_MM_dd_HH_mm", new CultureInfo("en-US")) + ".csv";

            StreamReader sr = new StreamReader(FileName_load);
            FileStream stream = new FileStream(FileName_save, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(Regex.Replace(line, ",", "\t", RegexOptions.IgnoreCase));
            }

            sr.Close();
            ///////////////////// Log 저장을 위한 file setting

            MinMax GCS_MinMax = new MinMax();
            int max_gray = Convert.ToInt32(GCS_max_gray.Text);
            int min_gray = Convert.ToInt32(GCS_min_gray.Text);
            int gray_step = Convert.ToInt32(GCS_step.Text);
            int PTN_delay = Convert.ToInt16(GCS_Delay.Text);

            GCS_MinMax = GCS_Min_Max_check(max_gray, min_gray);
            progressBar_GCS.PerformStep();

            // GCS Measurement
            for (int i = 0; i < 20 & GCS_measure; i++)
            {
                data_index = 0;

                System.Windows.Forms.Application.DoEvents();
                if (check_GCS_DBV[i] && GCS_measure)
                {
                    string DBV = value_GCS_DBV[i].PadLeft(3, '0');//dex to hex (as a string form)
                    try
                    {
                        f1().DBV_Setting(DBV);
                        Thread.Sleep(PTN_delay);

                        GCS_header_update(i+1, DBV);
                        row_count++;

                        f1().GB_Status_AppendText_Nextline("multi CH measurement GCS" + (i+1).ToString() + ") DBV[" + DBV + "] was applied", Color.Blue);
                    }
                    catch
                    {
                        f1().GB_Status_AppendText_Nextline("multi CH measurement GCS" + (i + 1).ToString() + ") DBV[" + DBV + "] was failed", Color.Red);
                    }

                    try
                    {
                        GCS_Measure_step(i + 1, DBV, GCS_MinMax.Min, GCS_MinMax.Max, gray_step, PTN_delay);

                        save_GCS_data(i + 1, DBV, sw);
                    }
                    catch
                    {
                        sw.Close();
                        stream.Close();

                        stop_flag = true;
                        measure_flahg_change(false);
                    }
                }
                else
                {
                    f1().GB_Status_AppendText_Nextline("multi CH measurement GCS" + (i + 1).ToString() + ") DBV Point Skip", Color.Black);
                }
            }
            
            if (!stop_flag)
            {
                label_mornitoring.Text = "Save the GCS measurement data to file";

                sw.Close();
                stream.Close();
            }
        }
        private void GCS_Measure_step(int num, string DBV, int min_gray, int max_gray, int gray_step, int PTN_delay)
        {
            int gray=0;
            int number_of_measure = (max_gray - min_gray) / gray_step + 1;
            double number_of_measure_remain = (max_gray - min_gray) % gray_step;
            if(number_of_measure_remain>=0.5) number_of_measure++;

            count_measure = number_of_measure;
            for (int set = 0; set < 6; set++)
            {
                index_list = new int[count_measure];
                for (int ch = 0; ch < 10; ch++)
                {
                    data_list[set][ch] = new XYLv[count_measure];
                }
            }

            if (GCS_Max_to_Min.Checked)
            {
                index_max = 0;
                gray = max_gray;
            }
            else
            {
                index_max = number_of_measure - 1;
                gray = min_gray;
            }

            for (int n = 0; n < number_of_measure & GCS_measure; n++)
            {
                System.Windows.Forms.Application.DoEvents();
                label_mornitoring.Text = "GCS Measure : GCS" + num.ToString() + ") DBV 0x" + DBV + " Gray " + gray.ToString();

                try
                {
                    GCS_row_update(gray);

                    row_count++;

                    f1().PTN_update(gray, gray, gray);

                    Thread.Sleep(PTN_delay);

                    Set_change_Measure();

                    GCS_measurement_data_Save(num, n, DBV, gray);

                    if (GCS_Max_to_Min.Checked)
                    {
                        gray = gray - gray_step;
                        if (gray < min_gray) gray = min_gray;
                    }
                    else
                    {
                        gray = gray + gray_step;
                        if (gray > max_gray) gray = max_gray;
                    }

                    progressBar_GCS.PerformStep();
                    progressBar_Measurement.PerformStep();
                }
                catch
                {
                    stop_flag = true;
                    measure_flahg_change(false);
                }
            }


        }
        private int GCS_Progress_Bar_Setting()
        {
            int max_gray = Convert.ToInt32(GCS_max_gray.Text);
            int min_gray = Convert.ToInt32(GCS_min_gray.Text);
            int gray_step = Convert.ToInt32(GCS_step.Text);
            int number_of_measure = (max_gray - min_gray) / gray_step + 1;
            double number_of_measure_remain = (max_gray - min_gray) % gray_step;
            if (number_of_measure_remain >= 0.5) number_of_measure++;

            progressBar_GCS.Value = 0;
            progressBar_GCS.Step = 1;
            progressBar_GCS.Minimum = 0;
            progressBar_GCS.Maximum = GCS_check_count * number_of_measure;

            return progressBar_GCS.Maximum;
        }
        private void GCS_header_update(int number, string DBV)
        {
            for (int ch = 0; ch < 10 & GCS_measure; ch++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add("GCS", number.ToString() + ")DBV", DBV, "", number.ToString() + ")DBV", DBV, "", number.ToString() + ")DBV", DBV, "", number.ToString() + ")DBV", DBV, "", number.ToString() + ")DBV", DBV, "", number.ToString() + ")DBV", DBV, "");
                }
            }
        }
        private void GCS_row_update(int gray)
        {
            for (int ch = 0; ch < 10 & GCS_measure; ch++)
            {
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add(gray.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }
        }
        private void GCS_measurement_data_Save(int number, int measure_num, string DBV, int gray)
        {
            Application.DoEvents();

            for (int ch = 0; ch < 10; ch++)
            {
                for (int set = 0; set < 6; set++) // Set & 시료별 data write
                {
                    if (check_SET[set] && check_CA_CH[ch])
                    {
                        index_list[data_index] = gray;

                        data_list[set][ch][data_index].double_X = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 1].Value);
                        data_list[set][ch][data_index].double_Y = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 2].Value);
                        data_list[set][ch][data_index].double_Lv = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 3].Value);
                    }
                }
            }
            data_index++;
        }

        private void BCS_Gray_check()
        {
            BCS_check_count = 0;
            BCS_range_check_count = 0;

            //Textbox to String
            value_BCS_Gray[0] = BCS_Gray1.Text;
            value_BCS_Gray[1] = BCS_Gray2.Text;
            value_BCS_Gray[2] = BCS_Gray3.Text;
            value_BCS_Gray[3] = BCS_Gray4.Text;
            value_BCS_Gray[4] = BCS_Gray5.Text;
            value_BCS_Gray[5] = BCS_Gray6.Text;
            value_BCS_Gray[6] = BCS_Gray7.Text;
            value_BCS_Gray[7] = BCS_Gray8.Text;
            value_BCS_Gray[8] = BCS_Gray9.Text;
            value_BCS_Gray[9] = BCS_Gray10.Text;
            value_BCS_Gray[10] = BCS_Gray11.Text;
            value_BCS_Gray[11] = BCS_Gray12.Text;
            value_BCS_Gray[12] = BCS_Gray13.Text;
            value_BCS_Gray[13] = BCS_Gray14.Text;
            value_BCS_Gray[14] = BCS_Gray15.Text;
            value_BCS_Gray[15] = BCS_Gray16.Text;
            value_BCS_Gray[16] = BCS_Gray17.Text;
            value_BCS_Gray[17] = BCS_Gray18.Text;
            value_BCS_Gray[18] = BCS_Gray19.Text;
            value_BCS_Gray[19] = BCS_Gray20.Text;

            //CheckBox to Bool
            check_BCS_Gray[0] = check_BCS_Gray1.Checked;
            check_BCS_Gray[1] = check_BCS_Gray2.Checked;
            check_BCS_Gray[2] = check_BCS_Gray3.Checked;
            check_BCS_Gray[3] = check_BCS_Gray4.Checked;
            check_BCS_Gray[4] = check_BCS_Gray5.Checked;
            check_BCS_Gray[5] = check_BCS_Gray6.Checked;
            check_BCS_Gray[6] = check_BCS_Gray7.Checked;
            check_BCS_Gray[7] = check_BCS_Gray8.Checked;
            check_BCS_Gray[8] = check_BCS_Gray9.Checked;
            check_BCS_Gray[9] = check_BCS_Gray10.Checked;
            check_BCS_Gray[10] = check_BCS_Gray11.Checked;
            check_BCS_Gray[11] = check_BCS_Gray12.Checked;
            check_BCS_Gray[12] = check_BCS_Gray13.Checked;
            check_BCS_Gray[13] = check_BCS_Gray14.Checked;
            check_BCS_Gray[14] = check_BCS_Gray15.Checked;
            check_BCS_Gray[15] = check_BCS_Gray16.Checked;
            check_BCS_Gray[16] = check_BCS_Gray17.Checked;
            check_BCS_Gray[17] = check_BCS_Gray18.Checked;
            check_BCS_Gray[18] = check_BCS_Gray19.Checked;
            check_BCS_Gray[19] = check_BCS_Gray20.Checked;

            check_BCS_Range[0] = BCS_Range1.Checked;
            check_BCS_Range[1] = BCS_Range2.Checked;
            check_BCS_Range[2] = BCS_Range3.Checked;

            value_BCS_Range_min_DBV[0] = BCS_Range1_min_DBV.Text;
            value_BCS_Range_min_DBV[1] = BCS_Range2_min_DBV.Text;
            value_BCS_Range_min_DBV[2] = BCS_Range3_min_DBV.Text;
            
            value_BCS_Range_max_DBV[0] = BCS_Range1_max_DBV.Text;
            value_BCS_Range_max_DBV[1] = BCS_Range2_max_DBV.Text;
            value_BCS_Range_max_DBV[2] = BCS_Range3_max_DBV.Text;

            value_BCS_Range_DBV_step[0] = BCS_Range1_step.Text;
            value_BCS_Range_DBV_step[1] = BCS_Range2_step.Text;
            value_BCS_Range_DBV_step[2] = BCS_Range3_step.Text;

            for (int bcs = 0; bcs < 10; bcs++)
            {
                if (check_BCS_Gray[bcs]) BCS_check_count++;
            }
            for (int range = 0; range < 3; range++)
            {
                if (check_BCS_Range[range]) BCS_range_check_count++;
            }

            if ((BCS_check_count == 0) || (BCS_range_check_count == 0)) BCS_measure = false;
        }
        private void BCS_All_Gray_Status(bool Checked)
        {
            check_BCS_Gray1.Checked = Checked;
            check_BCS_Gray2.Checked = Checked;
            check_BCS_Gray3.Checked = Checked;
            check_BCS_Gray4.Checked = Checked;
            check_BCS_Gray5.Checked = Checked;
            check_BCS_Gray6.Checked = Checked;
            check_BCS_Gray7.Checked = Checked;
            check_BCS_Gray8.Checked = Checked;
            check_BCS_Gray9.Checked = Checked;
            check_BCS_Gray10.Checked = Checked;
            check_BCS_Gray11.Checked = Checked;
            check_BCS_Gray12.Checked = Checked;
            check_BCS_Gray13.Checked = Checked;
            check_BCS_Gray14.Checked = Checked;
            check_BCS_Gray15.Checked = Checked;
            check_BCS_Gray16.Checked = Checked;
            check_BCS_Gray17.Checked = Checked;
            check_BCS_Gray18.Checked = Checked;
            check_BCS_Gray19.Checked = Checked;
            check_BCS_Gray20.Checked = Checked;
        }
        private void button_BCS_Gray_all_select_Click_1(object sender, EventArgs e)
        {
            BCS_All_Gray_Status(true);
        }
        private void button_BCS_Gray_all_deselect_Click_1(object sender, EventArgs e)
        {
            BCS_All_Gray_Status(false);
        }
        private MinMax[] BCS_Min_Max_check(int max_limit_DBV)
        {
            MinMax[] BCS_MinMax = new MinMax[3];
            
            for (int r = 0; r < 3 & BCS_measure; r++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_BCS_Range[r] && BCS_measure)
                {
                    BCS_MinMax[r].Max = Convert.ToInt32(value_BCS_Range_max_DBV[r]);
                    BCS_MinMax[r].Min = Convert.ToInt32(value_BCS_Range_min_DBV[r]);

                    if (BCS_MinMax[r].Max > max_limit_DBV)
                    {
                        switch (r)
                        {
                            case 0:
                                BCS_Range1_max_DBV.Text = max_limit_DBV.ToString();
                                value_BCS_Range_max_DBV[r] = max_limit_DBV.ToString();
                                break;
                            case 1:
                                BCS_Range2_max_DBV.Text = max_limit_DBV.ToString();
                                value_BCS_Range_max_DBV[r] = max_limit_DBV.ToString();
                                break;
                            case 2:
                                BCS_Range3_max_DBV.Text = max_limit_DBV.ToString();
                                value_BCS_Range_max_DBV[r] = max_limit_DBV.ToString();
                                break;
                        }
                        BCS_MinMax[r].Max = max_limit_DBV;
                    }
                    if (BCS_MinMax[r].Min < 1)
                    {
                        switch (r)
                        {
                            case 0:
                                BCS_Range1_min_DBV.Text = "1";
                                value_BCS_Range_min_DBV[r] = "1";
                                break;
                            case 1:
                                BCS_Range2_min_DBV.Text = "1";
                                value_BCS_Range_min_DBV[r] = "1";
                                break;
                            case 2:
                                BCS_Range3_min_DBV.Text = "1";
                                value_BCS_Range_min_DBV[r] = "1";
                                break;
                        }
                        BCS_MinMax[r].Min = 1;
                    }
                }
            }

            return BCS_MinMax;
        }
        private void BCS_Measure()
        {
            
            label_mornitoring.Text = "Open file for saving BCS measurement data";
            ///////////////////// Log 저장을 위한 file setting
            string FileName_load = Directory.GetCurrentDirectory() + "\\Optic_Measurement_Data(10ch)\\format\\BCS_format.csv";
            string FileName_save = Folder_Control.Make_new_folder(Start_Time) + "\\BCS_" + Start_Time.ToString(@"yyyy_MM_dd_HH_mm", new CultureInfo("en-US")) + ".csv";

            StreamReader sr = new StreamReader(FileName_load);
            FileStream stream = new FileStream(FileName_save, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(Regex.Replace(line, ",", "\t", RegexOptions.IgnoreCase));
            }

            sr.Close();
            ///////////////////// Log 저장을 위한 file setting

            int DBV_step=0;
            int DBV_delay = Convert.ToInt16(BCS_Delay.Text);
            MinMax[] BCS_MinMax = new MinMax[3];
            int max_limit_DBV = f1().current_model.get_DBV_Max();

            BCS_MinMax = BCS_Min_Max_check(max_limit_DBV);

            progressBar_BCS.PerformStep();
            
            for (int i = 0; i < 20 & BCS_measure; i++)
            {
                if (check_BCS_Gray[i] && BCS_measure)
                {
                    int gray = Convert.ToInt32(value_BCS_Gray[i]);
                    for (int r = 0; r < 3 & BCS_measure; r++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        if (check_BCS_Range[r] && BCS_measure)
                        {
                            data_index = 0;
                            DBV_step = Convert.ToInt32(value_BCS_Range_DBV_step[r]);
                        
                            System.Windows.Forms.Application.DoEvents();

                            try
                            {
                                f1().PTN_update(gray, gray, gray);
                                BCS_header_update(i+1, gray, r+1);
                                
                                row_count++;

                                f1().GB_Status_AppendText_Nextline("multi CH measurement BCS" + (i+1).ToString() + ")Gray" + gray.ToString() + " was applied", Color.Blue);
                            }
                            catch
                            {
                                f1().GB_Status_AppendText_Nextline("multi CH measurement BCS" + (i + 1).ToString() + ")Gray point was skipped was failed", Color.Black);
                            }

                            try
                            {
                                BCS_Measure_step(i + 1, r, gray, BCS_MinMax[r].Min, BCS_MinMax[r].Max, DBV_step, DBV_delay);

                                save_BCS_data(r+1,i+1,gray,sw);
                            }
                            catch
                            {
                                sw.Close();
                                stream.Close();

                                stop_flag = true;
                                measure_flahg_change(false);
                            }
                        }
                        else
                        {
                            f1().GB_Status_AppendText_Nextline("multi CH measurement BCS" + (i + 1).ToString() + ")Gray Point Skip", Color.Black);
                        }
                    }
                }
            }

            if (!stop_flag)
            {
                label_mornitoring.Text = "Save the BCS measurement data to file";

                sw.Close();
                stream.Close();
            }
        }
        private void BCS_Measure_step(int num, int range, int gray, int min_DBV, int max_DBV, int DBV_step, int DBV_delay)
        {
            int DBV = 0;
            int number_of_measure = (max_DBV - min_DBV) / DBV_step + 1;
            double number_of_measure_remain = (max_DBV - min_DBV) % DBV_step;
            if (number_of_measure_remain >= 0.5) number_of_measure++;

            count_measure = number_of_measure;
            for (int set = 0; set < 6; set++)
            {
                index_list = new int[count_measure];
                for (int ch = 0; ch < 10; ch++)
                {
                    data_list[set][ch] = new XYLv[count_measure];
                }
            }

            if (BCS_Max_to_Min.Checked)
            {
                index_max = 0;
                DBV = max_DBV;
            }
            else
            {
                index_max = number_of_measure - 1;
                DBV = min_DBV;
            }

            for (int n = 0; n < number_of_measure & BCS_measure; n++)
            {
                System.Windows.Forms.Application.DoEvents();
                label_mornitoring.Text = "BCS Measure : range" + (range + 1).ToString() + "-BCS" + num.ToString() + ") Gray" + gray.ToString() + " DBV 0x" + DBV.ToString("X3");
                BCS_row_update(DBV);
                row_count++;

                try
                {
                    f1().DBV_Setting(DBV.ToString("X3"));
                    Thread.Sleep(DBV_delay);

                    Set_change_Measure();

                    BCS_measurement_data_Save(range, n, DBV, gray);

                    // 측정 후 Gray 변경
                    if (BCS_Max_to_Min.Checked)
                    {
                        DBV = DBV - DBV_step;
                        if (DBV < min_DBV) DBV = min_DBV;
                    }
                    else
                    {
                        DBV = DBV + DBV_step;
                        if (DBV > max_DBV) DBV = max_DBV;
                    }
                    progressBar_BCS.PerformStep();
                    progressBar_Measurement.PerformStep();
                }
                catch
                {
                    stop_flag = true;
                    measure_flahg_change(false);
                }
            }
        }
        private int BCS_Progress_Bar_Setting()
        {
            //Set ProgressBar
            int multiple = 0;

            for (int r = 0; r < 3 & BCS_measure; r++)
            {
                int max_DBV = Convert.ToInt32(value_BCS_Range_max_DBV[r]);
                int min_DBV = Convert.ToInt32(value_BCS_Range_min_DBV[r]);
                int DBV_step = Convert.ToInt32(value_BCS_Range_DBV_step[r]);

                int number_of_measure = (max_DBV - min_DBV) / DBV_step + 1;
                double number_of_measure_remain = (max_DBV - min_DBV) % DBV_step;
                if (number_of_measure_remain >= 0.5) number_of_measure++;

                if (check_BCS_Range[r])
                {
                    multiple += number_of_measure;
                }
            }

            progressBar_BCS.Value = 0;
            progressBar_BCS.Step = 1;
            progressBar_BCS.Minimum = 0;
            progressBar_BCS.Maximum = BCS_check_count * multiple;

            return progressBar_BCS.Maximum;
        }
        private void BCS_header_update(int number, int gray, int range)
        {
            for (int ch = 0; ch < 10 & BCS_measure; ch++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add("BCS-" + range.ToString(), number.ToString() + ")Gray", gray, "", number.ToString() + ")Gray", gray, "", number.ToString() + ")Gray", gray, "", number.ToString() + ")Gray", gray, "", number.ToString() + ")Gray", gray, "", number.ToString() + ")Gray", gray, "");
                }
            }
        }
        private void BCS_row_update(int DBV)
        {
            for (int ch = 0; ch < 10 & BCS_measure; ch++)
            {
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add(DBV.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }
        }
        private void BCS_measurement_data_Save(int range, int measure_num, int DBV, int gray)
        {
            Application.DoEvents();

            for (int ch = 0; ch < 10; ch++)
            {
                for (int set = 0; set < 6; set++) // Set & 시료별 data write
                {
                    if (check_SET[set] && check_CA_CH[ch])
                    {
                        index_list[data_index] = DBV;

                        data_list[set][ch][data_index].double_X = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 1].Value);
                        data_list[set][ch][data_index].double_Y = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 2].Value);
                        data_list[set][ch][data_index].double_Lv = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 3].Value);
                    }
                }
            }

            data_index++;
        }

        private void Gamma_Crush_Setting_check()
        {
            Gamma_Crush_check_count = 0;
            Gamma_Crush_check_count = 0;

            //Textbox to String
            value_Gamma_Crush_DBV[0] = Gamma_Crush_DBV1.Text;
            value_Gamma_Crush_DBV[1] = Gamma_Crush_DBV2.Text;
            value_Gamma_Crush_DBV[2] = Gamma_Crush_DBV3.Text;
            value_Gamma_Crush_DBV[3] = Gamma_Crush_DBV4.Text;
            value_Gamma_Crush_DBV[4] = Gamma_Crush_DBV5.Text;
            value_Gamma_Crush_DBV[5] = Gamma_Crush_DBV6.Text;
            value_Gamma_Crush_DBV[6] = Gamma_Crush_DBV7.Text;
            value_Gamma_Crush_DBV[7] = Gamma_Crush_DBV8.Text;
            value_Gamma_Crush_DBV[8] = Gamma_Crush_DBV9.Text;
            value_Gamma_Crush_DBV[9] = Gamma_Crush_DBV10.Text;

            //Textbox to String
            value_Gamma_Crush_Gray[0] = Gamma_Crush_Gray1.Text;
            value_Gamma_Crush_Gray[1] = Gamma_Crush_Gray2.Text;
            value_Gamma_Crush_Gray[2] = Gamma_Crush_Gray3.Text;
            value_Gamma_Crush_Gray[3] = Gamma_Crush_Gray4.Text;
            value_Gamma_Crush_Gray[4] = Gamma_Crush_Gray5.Text;
            value_Gamma_Crush_Gray[5] = Gamma_Crush_Gray6.Text;
            value_Gamma_Crush_Gray[6] = Gamma_Crush_Gray7.Text;
            value_Gamma_Crush_Gray[7] = Gamma_Crush_Gray8.Text;
            value_Gamma_Crush_Gray[8] = Gamma_Crush_Gray9.Text;
            value_Gamma_Crush_Gray[9] = Gamma_Crush_Gray10.Text;

            //CheckBox to Bool
            check_Gamma_Crush[0] = check_Gamma_Crush1.Checked;
            check_Gamma_Crush[1] = check_Gamma_Crush2.Checked;
            check_Gamma_Crush[2] = check_Gamma_Crush3.Checked;
            check_Gamma_Crush[3] = check_Gamma_Crush4.Checked;
            check_Gamma_Crush[4] = check_Gamma_Crush5.Checked;
            check_Gamma_Crush[5] = check_Gamma_Crush6.Checked;
            check_Gamma_Crush[6] = check_Gamma_Crush7.Checked;
            check_Gamma_Crush[7] = check_Gamma_Crush8.Checked;
            check_Gamma_Crush[8] = check_Gamma_Crush9.Checked;
            check_Gamma_Crush[9] = check_Gamma_Crush10.Checked;

            //CheckBox to Bool
            check_Gamma_Crush_color[0] = checkBox_Gamma_Crush_W.Checked;
            check_Gamma_Crush_color[1] = checkBox_Gamma_Crush_R.Checked;
            check_Gamma_Crush_color[2] = checkBox_Gamma_Crush_G.Checked;
            check_Gamma_Crush_color[3] = checkBox_Gamma_Crush_B.Checked;

            for (int gc = 0; gc < 10; gc++)
            {
                if (check_Gamma_Crush[gc]) Gamma_Crush_check_count++;
            }
            for (int color = 0; color < 4; color++)
            {
                if (check_Gamma_Crush_color[color]) Gamma_Crush_Color_check_count++;
            }
            if ((Gamma_Crush_check_count == 0) || (Gamma_Crush_Color_check_count == 0)) Gamma_Crush_measure = false;
        }
        private void Gamma_Crush_Measure()
        {
            label_mornitoring.Text = "Open file for saving Gamma_Crush measurement data";
            ///////////////////// Log 저장을 위한 file setting
            string FileName_load = Directory.GetCurrentDirectory() + "\\Optic_Measurement_Data(10ch)\\format\\Gamma_Crush_format.csv";
            string FileName_save = Folder_Control.Make_new_folder(Start_Time) + "\\Gamma_Crush" + Start_Time.ToString(@"yyyy_MM_dd_HH_mm", new CultureInfo("en-US")) + ".csv";

            StreamReader sr = new StreamReader(FileName_load);
            FileStream stream = new FileStream(FileName_save, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(Regex.Replace(line, ",", "\t", RegexOptions.IgnoreCase));
            }

            sr.Close();
            ///////////////////// Log 저장을 위한 file setting

            int DBV_delay = Convert.ToInt16(Gamma_Crush_DBV_Delay.Text);
            int PTN_delay = Convert.ToInt16(Gamma_Crush_PTN_Delay.Text);

            progressBar_Gamma_Crush.PerformStep();

            for (int i = 0; i < 10 & Gamma_Crush_measure; i++)
            {
                data_index = 0;
                System.Windows.Forms.Application.DoEvents();
                if (check_Gamma_Crush[i] && Gamma_Crush_measure)
                {
                    int gray = Convert.ToInt16(value_Gamma_Crush_Gray[i]);
                    string DBV = value_Gamma_Crush_DBV[i].PadLeft(3, '0');

                    try
                    {
                        f1().DBV_Setting(DBV);
                        Thread.Sleep(DBV_delay);
                        Gamma_Crush_header_update(i+1);

                        row_count++;

                        f1().GB_Status_AppendText_Nextline("multi CH measurement Gamma Cursh" + (i + 1).ToString() + ") DBV[" + DBV + "] was applied", Color.Blue);
                    }
                    catch
                    {
                        f1().GB_Status_AppendText_Nextline("multi CH measurement Gamma Cursh" + (i + 1).ToString() + ") DBV[" + DBV + "] was failed", Color.Red);
                    }

                    try
                    {
                        Gamma_Crush_Measure_step(i + 1, gray, DBV, PTN_delay);

                        save_Gamma_Crush(i + 1, DBV, gray, sw);
                    }
                    catch
                    {
                        sw.Close();
                        stream.Close();

                        stop_flag = true;
                        measure_flahg_change(false);
                    }
                }
                else
                {
                    f1().GB_Status_AppendText_Nextline("multi CH measurement Gamma Cursh" + (i + 1).ToString() + ") Point Skip", Color.Black);
                }
            }

            if (!stop_flag)
            {
                label_mornitoring.Text = "Save the Gamma_Crush measurement data to file";

                sw.Close();
                stream.Close();
            }
        }
        private void Gamma_Crush_Measure_step(int num, int gray, string DBV, int PTN_delay)
        {
            count_measure = 4;
            
            index_list = new int[count_measure];

            for (int set = 0; set < 6; set++)
            {
                for (int ch = 0; ch < 10; ch++)
                {
                    data_list[set][ch] = new XYLv[count_measure];
                }
            }

            for (int color = 0; color < 4 & Gamma_Crush_measure; color++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_Gamma_Crush_color[color] && Gamma_Crush_measure)
                {
                    string PTN_Color = "";
                    switch (color)
                    {
                        case 0: PTN_Color = "W"; break;
                        case 1: PTN_Color = "R"; break;
                        case 2: PTN_Color = "G"; break;
                        case 3: PTN_Color = "B"; break;
                    }
                    label_mornitoring.Text = "Gamma Crush Measure : " + num.ToString() + ") DBV 0x" + DBV + " Gray " + gray.ToString() + PTN_Color;

                    Gamma_Crush_row_update(color);
                    row_count++;

                    Gamma_Crush_PTN_update(color, gray, PTN_delay);

                    Set_change_Measure();
                    
                    Gamma_Crush_measurement_data_Save(num-1, color, DBV, gray);
                }
                progressBar_Gamma_Crush.PerformStep();
                progressBar_Measurement.PerformStep();
            }
        }
        private void Gamma_Crush_header_update(int number)
        {
           for (int ch = 0; ch < 10 & GCS_measure; ch++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add("G.Crush", number.ToString() + ")", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }
        }
        private int Gamma_Crush_Progress_Bar_Setting()
        {
            ///Set ProgressBar
            progressBar_Gamma_Crush.Value = 0;
            progressBar_Gamma_Crush.Step = 1;
            progressBar_Gamma_Crush.Minimum = 0;
            progressBar_Gamma_Crush.Maximum = Gamma_Crush_check_count * Gamma_Crush_Color_check_count;

            return progressBar_Gamma_Crush.Maximum;
        }
        private void Gamma_Crush_row_update(int color)
        {
            string color_string = null;
            switch (color)
            {
                case 0:
                    color_string = "W";
                    break;
                case 1:
                    color_string = "R";
                    break;
                case 2:
                    color_string = "G";
                    break;
                case 3:
                    color_string = "B";
                    break;
            }
            for (int ch = 0; ch < 10 & GCS_measure; ch++)
            {
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add(color_string, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }
        }
        private void Gamma_Crush_PTN_update(int color, int gray, int PTN_delay)
        {
            switch (color)
            {
                case 0:
                    f1().PTN_update(gray, gray, gray);
                    Thread.Sleep(PTN_delay);
                    break;
                case 1:
                    f1().PTN_update(gray, 0, 0);
                    Thread.Sleep(PTN_delay);
                    break;
                case 2:
                    f1().PTN_update(0, gray, 0);
                    Thread.Sleep(PTN_delay);
                    break;
                case 3:
                    f1().PTN_update(0, 0, gray);
                    Thread.Sleep(PTN_delay);
                    break;
            }
        }
        private void Gamma_Crush_measurement_data_Save(int num, int color, string DBV, int gray)
        {
            Application.DoEvents();

            for (int ch = 0; ch < 10; ch++)
            {
                for (int set = 0; set < 6; set++) // Set & 시료별 data write
                {
                    if (check_SET[set] && check_CA_CH[ch])
                    {
                        data_list[set][ch][data_index].double_X = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 1].Value);
                        data_list[set][ch][data_index].double_Y = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 2].Value);
                        data_list[set][ch][data_index].double_Lv = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3 * set + 3].Value);
                    }
                }
            }
            data_index++;
        }

        private void AOD_GCS_DBV_check()
        {
            AOD_GCS_check_count = 0;
            //Textbox to String
            value_AOD_GCS_DBV[0] = AOD_GCS_DBV1.Text;
            value_AOD_GCS_DBV[1] = AOD_GCS_DBV2.Text;
            value_AOD_GCS_DBV[2] = AOD_GCS_DBV3.Text;

            //CheckBox to Bool
            check_AOD_GCS_DBV[0] = check_AOD_GCS_DBV1.Checked;
            check_AOD_GCS_DBV[1] = check_AOD_GCS_DBV2.Checked;
            check_AOD_GCS_DBV[2] = check_AOD_GCS_DBV3.Checked;

            for (int gcs = 0; gcs < 3; gcs++)
            {
                if (check_AOD_GCS_DBV[gcs]) AOD_GCS_check_count++;
            }
            if (GCS_check_count == 0) GCS_measure = false;
        }
        private void AOD_GCS_Measure()
        {
           
            label_mornitoring.Text = "Open file for saving GCS measurement data";
            ///////////////////// Log 저장을 위한 file setting
            string FileName_load = Directory.GetCurrentDirectory() + "\\Optic_Measurement_Data(10ch)\\format\\AOD_GCS_format.csv";
            string FileName_save = Folder_Control.Make_new_folder(Start_Time) + "\\AOD_GCS_" + Start_Time.ToString(@"yyyy_MM_dd_HH_mm", new CultureInfo("en-US")) + ".csv";

            StreamReader sr = new StreamReader(FileName_load);
            FileStream stream = new FileStream(FileName_save, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(Regex.Replace(line, ",", "\t", RegexOptions.IgnoreCase));
            }

            sr.Close();
            ///////////////////// Log 저장을 위한 file setting
            
            int max_gray = Convert.ToInt32(AOD_GCS_max_gray.Text);
            int min_gray = Convert.ToInt32(AOD_GCS_min_gray.Text);
            int gray_step = Convert.ToInt32(AOD_GCS_step.Text);
            int PTN_delay = Convert.ToInt16(AOD_GCS_Delay.Text);
            int Code_delay = Convert.ToInt16(AOD_CODE_Delay.Text);

            if (max_gray > 255)
            {
                AOD_GCS_measure = false;
                System.Windows.Forms.MessageBox.Show("Maximun value of Gray Max is 255");
                GCS_max_gray.Text = "255";
            }
            if (min_gray < 0)
            {
                AOD_GCS_measure = false;
                System.Windows.Forms.MessageBox.Show("Minimum value of Gray Min is 0");
                GCS_min_gray.Text = "0";
            }

            progressBar_AOD_GCS.PerformStep();

            f1().PTN_update(0, 0, 0);
            Thread.Sleep(PTN_delay);
            if(AOD_GCS_measure) f1().AOD_On();
            else f1().AOD_Off();
            Thread.Sleep(Code_delay);

            // GCS Measurement
            for (int i = 0; i < 3 & AOD_GCS_measure; i++)
            {
                data_index = 0;
                System.Windows.Forms.Application.DoEvents();
                if (check_AOD_GCS_DBV[i])
                {
                    string DBV = value_AOD_GCS_DBV[i].PadLeft(3, '0');//dex to hex (as a string form)
                    try
                    {
                        f1().DBV_Setting(DBV);
                        Thread.Sleep(PTN_delay);

                        AOD_GCS_header_update(i, DBV);
                        row_count++;

                        f1().GB_Status_AppendText_Nextline("multi CH measurement AOD GCS" + (i + 1).ToString() + ") DBV[" + DBV + "] was applied", Color.Blue);
                    }
                    catch
                    {
                        f1().GB_Status_AppendText_Nextline("multi CH measurement AOD GCS" + (i + 1).ToString() + ") DBV[" + DBV + "] was failed", Color.Red);
                    }

                    try
                    {
                        AOD_GCS_Measure_step(i+1, DBV, min_gray, max_gray, gray_step, PTN_delay);

                        save_AOD_GCS_data(i + 1, DBV, sw);
                    }
                    catch
                    {
                        sw.Close();
                        stream.Close();

                        stop_flag = true;
                        measure_flahg_change(false);
                    }
                }
                else
                {
                    f1().GB_Status_AppendText_Nextline("multi CH measurement AOD GCS" + (i + 1).ToString() + ") DBV Point Skip", Color.Black);
                }
            }

            if (!stop_flag)
            {
                label_mornitoring.Text = "Save the AOD_GCS measurement data to file";
                sw.Close();
                stream.Close();
            }

            f1().PTN_update(0, 0, 0);
            Thread.Sleep(PTN_delay);
            f1().AOD_Off();
            Thread.Sleep(Code_delay);
            f1().AOD_Off();
            Thread.Sleep(Code_delay);
            f1().AOD_Off();
            Thread.Sleep(Code_delay);
        }

        private void AOD_GCS_Measure_step(int num, string DBV, int min_gray, int max_gray, int gray_step, int PTN_delay)
        {
            int gray = 0;
            int number_of_measure = (max_gray - min_gray) / gray_step + 1;
            double number_of_measure_remain = (max_gray - min_gray) % gray_step;
            if (number_of_measure_remain >= 0.5) number_of_measure++;

            count_measure = number_of_measure;
            index_list = new int[count_measure];
            for (int ch = 0; ch < 10; ch++)
            {
                data_list[0][ch] = new XYLv[count_measure];
            }

            if (AOD_GCS_Max_to_Min.Checked)
            {
                index_max = 0;
                gray = max_gray;
            }
            else
            {
                index_max = number_of_measure - 1;
                gray = min_gray;
            }

            for (int n = 0; n < number_of_measure & AOD_GCS_measure; n++)
            {
                System.Windows.Forms.Application.DoEvents();
                label_mornitoring.Text = "AoD GCS Measure : AOD GCS" + num.ToString() + ") DBV 0x" + DBV + " Gray " + gray.ToString();
                AOD_GCS_row_update(gray);
                row_count++;

                try
                {
                    f1().AOD_Pattern_Setting(gray);

                    Thread.Sleep(PTN_delay);

                    Set_non_change_Measure();

                    AOD_GCS_measurement_data_Save(num, n, DBV, gray);

                    // 측정 후 Gray 변경
                    if (AOD_GCS_Max_to_Min.Checked)
                    {
                        gray = gray - gray_step;
                        if (gray < min_gray) gray = min_gray;
                    }
                    else
                    {
                        gray = gray + gray_step;
                        if (gray > max_gray) gray = max_gray;
                    }
                    progressBar_AOD_GCS.PerformStep();
                    progressBar_Measurement.PerformStep();
                }
                catch
                {
                    stop_flag = true;
                    measure_flahg_change(false);
                } 
            }
        }
        private int AOD_GCS_Progress_Bar_Setting()
        {
            ///Set ProgressBar
            int max_gray = Convert.ToInt32(AOD_GCS_max_gray.Text);
            int min_gray = Convert.ToInt32(AOD_GCS_min_gray.Text);
            int gray_step = Convert.ToInt32(AOD_GCS_step.Text);

            int number_of_measure = (max_gray - min_gray) / gray_step + 1;
            double number_of_measure_remain = (max_gray - min_gray) % gray_step;
            if (number_of_measure_remain >= 0.5) number_of_measure++;

            progressBar_AOD_GCS.Value = 0;
            progressBar_AOD_GCS.Step = 1;
            progressBar_AOD_GCS.Minimum = 0;
            progressBar_AOD_GCS.Maximum = AOD_GCS_check_count * number_of_measure;

            return progressBar_AOD_GCS.Maximum;
        }
        private void AOD_GCS_header_update(int number, string DBV)
        {
            for (int ch = 0; ch < 10 & GCS_measure; ch++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add("AOD GCS", number.ToString() + ")DBV", DBV, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }
        }
        private void AOD_GCS_row_update(int gray)
        {
            for (int ch = 0; ch < 10 & AOD_GCS_measure; ch++)
            {
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add(gray.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }
        }
        private void AOD_GCS_measurement_data_Save(int number, int measure_num, string DBV, int gray)
        {
            Application.DoEvents();
            
            for (int ch = 0; ch < 10; ch++)
            {
                for (int set = 0; set < 6; set++) // Set & 시료별 data write
                {
                    if ((set==0) && check_CA_CH[ch])
                    {
                        index_list[data_index] = gray;

                        data_list[0][ch][data_index].double_X = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[1].Value);
                        data_list[0][ch][data_index].double_Y = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[2].Value);
                        data_list[0][ch][data_index].double_Lv = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3].Value);
                    }
                }
            }

            data_index++;
        }

        private void IR_Drop_DeltaE_Measure()
        {
            Make_IR_Drop_DeltaE_PTN_List();
            double X = 0;
            double Y = 0;
            f1().Intialize_XY(ref X, ref Y);
            bool Full_PTN = false;
            int PTN_delay = Convert.ToInt16(IR_Drop_DeltaE_Delay.Text);

            label_mornitoring.Text = "Open file for saving IR_Drop_DeltaE measurement data";
            ///////////////////// Log 저장을 위한 file setting
            string FileName_load = Directory.GetCurrentDirectory() + "\\Optic_Measurement_Data(10ch)\\format\\IR_Drop_DeltaE_format.csv";
            string FileName_save = Folder_Control.Make_new_folder(Start_Time) + "\\IR_Drop_DeltaE_" + Start_Time.ToString(@"yyyy_MM_dd_HH_mm", new CultureInfo("en-US")) + ".csv";

            StreamReader sr = new StreamReader(FileName_load);
            FileStream stream = new FileStream(FileName_save, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(Regex.Replace(line, ",", "\t", RegexOptions.IgnoreCase));
            }

            sr.Close();
            ///////////////////// Log 저장을 위한 file setting

            PNC_SET_Script_Send((Convert.ToInt32(IR_Drop_DeltaE_Set.Text)-1));  // Set change for IR Drop DeltaE
            string DBV = IR_Drop_DeltaE_DBV.Text.PadLeft(3, '0');//dex to hex (as a string form)
            f1().DBV_Setting(DBV);
            Thread.Sleep(PTN_delay);

            IR_Drop_DeltaE_header_update();
            row_count++;

            progressBar_IR_Drop_DeltaE.PerformStep();

            data_index = 0;
            count_measure = 50;
            index_list = new int[count_measure];
            for (int ch = 0; ch < 10; ch++)
            {
                data_list[0][ch] = new XYLv[count_measure];
            }

            index_max = 48;
            for (int i = 0; i < 50 & IR_Drop_DeltaE_measure; i++)
            {
                int Color_index =  i/2;
                int remain = i - Color_index*2;
                if(remain==1) Full_PTN = false;
                else Full_PTN = true;
                string PTN_Sizs = "";

                switch (Full_PTN)
                {
                    case true:
                        PTN_Sizs = "Full PTN";
                        break;
                    case false:
                        PTN_Sizs = "APL 30% PTN";
                        break;
                }
                label_mornitoring.Text = "IR Drop DeltaE Measure : Color" + (Color_index + 1).ToString() + ") (" + IR_Drop_PTN[Color_index].R.ToString() + "/" + IR_Drop_PTN[Color_index].G.ToString() + "/" + IR_Drop_PTN[Color_index].B.ToString() + ") " + PTN_Sizs;
                
                IR_Drop_DeltaE_row_update(i+1);
                row_count++;

                try
                {
                    System.Windows.Forms.Application.DoEvents();
                    IR_Drop_DeltaE_PTN_update(X, Y, IR_Drop_PTN[Color_index], Full_PTN);
                    Thread.Sleep(PTN_delay);

                    f1().GB_Status_AppendText_Nextline("multi CH measurement IR Drop DeltaE" + (i + 1).ToString() + ") PTN was applied", Color.Blue);
                }
                catch
                {
                    f1().GB_Status_AppendText_Nextline("multi CH measurement IR Drop DeltaE" + (i + 1).ToString() + ") PTN was failed", Color.Red);
                }


                try
                {
                    Set_non_change_Measure();
                    IR_Drop_DeltaE_measurement_data_Save(i);
                    progressBar_IR_Drop_DeltaE.PerformStep();
                    progressBar_Measurement.PerformStep();
                }
                catch
                {
                    stop_flag = true;
                    measure_flahg_change(false);
                }
            }

            try
            {
                save_IR_Drop_DeltaE(IR_Drop_DeltaE_Set.Text,DBV,sw);
            }
            catch
            {
                sw.Close();
                stream.Close();

                stop_flag = true;
                measure_flahg_change(false);
            }

            if (!stop_flag)
            {
                label_mornitoring.Text = "Save the IR_Drop_DeltaE measurement data to file";

                sw.Close();
                stream.Close();
            }
        }
        private void IR_Drop_DeltaE_header_update()
        {
            for (int ch = 0; ch < 10 & IR_Drop_DeltaE_measure; ch++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add("IR", "Drop", "DeltaE", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }
        }
        private void IR_Drop_DeltaE_Progress_Bar_Setting()
        {
            ///Set ProgressBar
            progressBar_IR_Drop_DeltaE.Value = 0;
            progressBar_IR_Drop_DeltaE.Step = 1;
            progressBar_IR_Drop_DeltaE.Minimum = 0;
            progressBar_IR_Drop_DeltaE.Maximum = 50;
        }
        private void IR_Drop_DeltaE_row_update(int index)
        {
            for (int ch = 0; ch < 10 & IR_Drop_DeltaE_measure; ch++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (check_CA_CH[ch])
                {
                    CH_Grid_View[ch].Rows.Add(index.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }
        }
        private void IR_Drop_DeltaE_PTN_update(double X, double Y, Color Inner_color, bool Full_PTN = false)
        {
            Color Outer_color = Color.White;
            
            string one_side = Convert.ToInt16(Math.Sqrt(X * Y * 0.3)).ToString();

            if (Full_PTN)
            {
                f1().PTN_update(Inner_color.R, Inner_color.G, Inner_color.B);

                //Just Mornitoring
                f1().GB_Status_AppendText_Nextline("Full PTN : " + Inner_color.R.ToString() + "/" + Inner_color.G.ToString()
                    + "/" + Inner_color.B.ToString(), Color.Black);
            }
            else
            {
                f1().IPC_Quick_Send("image.crosstalk " + one_side + " " + one_side
                    + " " + Outer_color.R.ToString() + " " + Outer_color.G.ToString() + " " + Outer_color.B.ToString()
                    + " " + Inner_color.R.ToString() + " " + Inner_color.G.ToString() + " " + Inner_color.B.ToString());

                //Just Mornitoring
                f1().GB_Status_AppendText_Nextline("Small PTN : " + Inner_color.R.ToString() + "/" + Inner_color.G.ToString()
                    + "/" + Inner_color.B.ToString(), Color.Black);
            }
        }
        public void Make_IR_Drop_DeltaE_PTN_List()
        {
            for(int i=0;i<25;i++)
            {
                switch (i)
                {
                    case 0:
                        IR_Drop_PTN[i] = Color.FromArgb(255, 0, 0);
                        break;
                    case 1:
                        IR_Drop_PTN[i] = Color.FromArgb(0, 255, 0);
                        break;
                    case 2:
                        IR_Drop_PTN[i] = Color.FromArgb(0, 0, 255);
                        break;
                    case 3:
                        IR_Drop_PTN[i] = Color.FromArgb(255, 255, 0);
                        break;
                    case 4:
                        IR_Drop_PTN[i] = Color.FromArgb(0, 255, 255);
                        break;
                    case 5:
                        IR_Drop_PTN[i] = Color.FromArgb(255, 0, 255);
                        break;
                    case 6:
                        IR_Drop_PTN[i] = Color.FromArgb(115, 82, 66);
                        break;
                    case 7:
                        IR_Drop_PTN[i] = Color.FromArgb(194, 150, 130);
                        break;
                    case 8:
                        IR_Drop_PTN[i] = Color.FromArgb(94, 122, 156);
                        break;
                    case 9:
                        IR_Drop_PTN[i] = Color.FromArgb(89, 107, 66);
                        break;
                    case 10:
                        IR_Drop_PTN[i] = Color.FromArgb(130, 128, 176);
                        break;
                    case 11:
                        IR_Drop_PTN[i] = Color.FromArgb(99, 189, 168);
                        break;
                    case 12:
                        IR_Drop_PTN[i] = Color.FromArgb(217, 120, 41);
                        break;
                    case 13:
                        IR_Drop_PTN[i] = Color.FromArgb(74, 92, 163);
                        break;
                    case 14:
                        IR_Drop_PTN[i] = Color.FromArgb(194, 84, 97);
                        break;
                    case 15:
                        IR_Drop_PTN[i] = Color.FromArgb(92, 61, 107);
                        break;
                    case 16:
                        IR_Drop_PTN[i] = Color.FromArgb(158, 186, 64);
                        break;
                    case 17:
                        IR_Drop_PTN[i] = Color.FromArgb(230, 161, 46);
                        break;
                    case 18:
                        IR_Drop_PTN[i] = Color.FromArgb(51, 61, 150);
                        break;
                    case 19:
                        IR_Drop_PTN[i] = Color.FromArgb(71, 148, 71);
                        break;
                    case 20:
                        IR_Drop_PTN[i] = Color.FromArgb(176, 48, 59);
                        break;
                    case 21:
                        IR_Drop_PTN[i] = Color.FromArgb(237, 199, 33);
                        break;
                    case 22:
                        IR_Drop_PTN[i] = Color.FromArgb(186, 84, 145);
                        break;
                    case 23:
                        IR_Drop_PTN[i] = Color.FromArgb(0, 133, 163);
                        break;
                    case 24:
                        IR_Drop_PTN[i] = Color.FromArgb(255, 255, 255);
                        break;
                }
            }
        }
        private void IR_Drop_DeltaE_measurement_data_Save(int index)
        {
            Application.DoEvents();
            
            for (int ch = 0; ch < 10; ch++)
            {
                for (int set = 0; set < 6; set++) // Set & 시료별 data write
                {
                    if ((set == 0) && check_CA_CH[ch])
                    {
                        data_list[0][ch][data_index].double_X = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[1].Value);
                        data_list[0][ch][data_index].double_Y = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[2].Value);
                        data_list[0][ch][data_index].double_Lv = Convert.ToDouble(CH_Grid_View[ch].Rows[row_count - 1].Cells[3].Value);
                    }
                }
            }

            data_index++;
        }
        


        private void DisplayError(Exception er)
        {
            String msg;
            msg = "Error from" + er.Source + "\r\n";
            msg += er.Message + "\r\n";
            System.Windows.Forms.MessageBox.Show(msg);
        }

        private void Clear_all()
        {
            row_count = 1;

            for (int ch = 0; ch < 10; ch++)
            {
                CH_Grid_View[ch].Rows.Clear();
                CH_Grid_View[ch].Rows.Add(first_line);
            }

            progressBar_GCS.Value = 0;
            progressBar_BCS.Value = 0;
            progressBar_Gamma_Crush.Value = 0;
            progressBar_AOD_GCS.Value = 0;
            progressBar_IR_Drop_DeltaE.Value = 0;
            progressBar_Measurement.Value = 0;
        }
        private void button_clear_Click(object sender, EventArgs e)
        {
            Clear_all();
        }

        private void button_Setting_Load_Click(object sender, EventArgs e)
        {
            Button_Click_Enable(false);

            Application.DoEvents();

            //------Set Setting Here------
            //FileStream myFileStream = new FileStream(Directory.GetCurrentDirectory() + "\\prefs.xml", FileMode.Open);//Used For Loading Setting

            FileStream myFileStream; 
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory()+"\\Optic_Measurement(10ch)";
            openFileDialog1.Filter = "Default Extension (*.xml)|*.xml";
            openFileDialog1.DefaultExt = "xml";
            openFileDialog1.AddExtension = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myFileStream = new FileStream(openFileDialog1.FileName, FileMode.Open);
                Multi_CH_Measurement_Preferences up = (Multi_CH_Measurement_Preferences)mySerializer.Deserialize(myFileStream);
               

                Set_Change_Delay.Text = up.Set_Change_Delay;

                check_SET1.Checked = up.check_SET[0];
                check_SET2.Checked = up.check_SET[1];
                check_SET3.Checked = up.check_SET[2];
                check_SET4.Checked = up.check_SET[3];
                check_SET5.Checked = up.check_SET[4];
                check_SET6.Checked = up.check_SET[5];

                SEQ_SET1.Text = up.SEQ_SET[0];
                SEQ_SET2.Text = up.SEQ_SET[1];
                SEQ_SET3.Text = up.SEQ_SET[2];
                SEQ_SET4.Text = up.SEQ_SET[3];
                SEQ_SET5.Text = up.SEQ_SET[4];
                SEQ_SET6.Text = up.SEQ_SET[5];

                textBox_Script_SET1.Text = up.textBox_Script_SET[0];
                textBox_Script_SET2.Text = up.textBox_Script_SET[1];
                textBox_Script_SET3.Text = up.textBox_Script_SET[2];
                textBox_Script_SET4.Text = up.textBox_Script_SET[3];
                textBox_Script_SET5.Text = up.textBox_Script_SET[4];
                textBox_Script_SET6.Text = up.textBox_Script_SET[5];

                //---GCS Setting---
                check_GCS_DBV1.Checked = up.check_GCS_DBV[0];
                check_GCS_DBV2.Checked = up.check_GCS_DBV[1];
                check_GCS_DBV3.Checked = up.check_GCS_DBV[2];
                check_GCS_DBV4.Checked = up.check_GCS_DBV[3];
                check_GCS_DBV5.Checked = up.check_GCS_DBV[4];
                check_GCS_DBV6.Checked = up.check_GCS_DBV[5];
                check_GCS_DBV7.Checked = up.check_GCS_DBV[6];
                check_GCS_DBV8.Checked = up.check_GCS_DBV[7];
                check_GCS_DBV9.Checked = up.check_GCS_DBV[8];
                check_GCS_DBV10.Checked = up.check_GCS_DBV[9];
                check_GCS_DBV11.Checked = up.check_GCS_DBV[10];
                check_GCS_DBV12.Checked = up.check_GCS_DBV[11];
                check_GCS_DBV13.Checked = up.check_GCS_DBV[12];
                check_GCS_DBV14.Checked = up.check_GCS_DBV[13];
                check_GCS_DBV15.Checked = up.check_GCS_DBV[14];
                check_GCS_DBV16.Checked = up.check_GCS_DBV[15];
                check_GCS_DBV17.Checked = up.check_GCS_DBV[16];
                check_GCS_DBV18.Checked = up.check_GCS_DBV[17];
                check_GCS_DBV19.Checked = up.check_GCS_DBV[18];
                check_GCS_DBV20.Checked = up.check_GCS_DBV[19];

                GCS_DBV1.Text = up.GCS_DBV[0];
                GCS_DBV2.Text = up.GCS_DBV[1];
                GCS_DBV3.Text = up.GCS_DBV[2];
                GCS_DBV4.Text = up.GCS_DBV[3];
                GCS_DBV5.Text = up.GCS_DBV[4];
                GCS_DBV6.Text = up.GCS_DBV[5];
                GCS_DBV7.Text = up.GCS_DBV[6];
                GCS_DBV8.Text = up.GCS_DBV[7];
                GCS_DBV9.Text = up.GCS_DBV[8];
                GCS_DBV10.Text = up.GCS_DBV[9];
                GCS_DBV11.Text = up.GCS_DBV[10];
                GCS_DBV12.Text = up.GCS_DBV[11];
                GCS_DBV13.Text = up.GCS_DBV[12];
                GCS_DBV14.Text = up.GCS_DBV[13];
                GCS_DBV15.Text = up.GCS_DBV[14];
                GCS_DBV16.Text = up.GCS_DBV[15];
                GCS_DBV17.Text = up.GCS_DBV[16];
                GCS_DBV18.Text = up.GCS_DBV[17];
                GCS_DBV19.Text = up.GCS_DBV[18];
                GCS_DBV20.Text = up.GCS_DBV[19];

                GCS_Delay.Text = up.GCS_Delay;
                GCS_min_gray.Text = up.GCS_min_gray;
                GCS_max_gray.Text = up.GCS_max_gray;
                GCS_step.Text = up.GCS_step;

                GCS_Min_to_Max.Checked = up.GCS_Min_to_Max;
                GCS_Max_to_Min.Checked = up.GCS_Max_to_Min;

                //---BCS Setting---
                check_BCS_Gray1.Checked = up.check_BCS_Gray[0];
                check_BCS_Gray2.Checked = up.check_BCS_Gray[1];
                check_BCS_Gray3.Checked = up.check_BCS_Gray[2];
                check_BCS_Gray4.Checked = up.check_BCS_Gray[3];
                check_BCS_Gray5.Checked = up.check_BCS_Gray[4];
                check_BCS_Gray6.Checked = up.check_BCS_Gray[5];
                check_BCS_Gray7.Checked = up.check_BCS_Gray[6];
                check_BCS_Gray8.Checked = up.check_BCS_Gray[7];
                check_BCS_Gray9.Checked = up.check_BCS_Gray[8];
                check_BCS_Gray10.Checked = up.check_BCS_Gray[9];
                check_BCS_Gray11.Checked = up.check_BCS_Gray[10];
                check_BCS_Gray12.Checked = up.check_BCS_Gray[11];
                check_BCS_Gray13.Checked = up.check_BCS_Gray[12];
                check_BCS_Gray14.Checked = up.check_BCS_Gray[13];
                check_BCS_Gray15.Checked = up.check_BCS_Gray[14];
                check_BCS_Gray16.Checked = up.check_BCS_Gray[15];
                check_BCS_Gray17.Checked = up.check_BCS_Gray[16];
                check_BCS_Gray18.Checked = up.check_BCS_Gray[17];
                check_BCS_Gray19.Checked = up.check_BCS_Gray[18];
                check_BCS_Gray20.Checked = up.check_BCS_Gray[19];

                BCS_Gray1.Text = up.BCS_Gray[0];
                BCS_Gray2.Text = up.BCS_Gray[1];
                BCS_Gray3.Text = up.BCS_Gray[2];
                BCS_Gray4.Text = up.BCS_Gray[3];
                BCS_Gray5.Text = up.BCS_Gray[4];
                BCS_Gray6.Text = up.BCS_Gray[5];
                BCS_Gray7.Text = up.BCS_Gray[6];
                BCS_Gray8.Text = up.BCS_Gray[7];
                BCS_Gray9.Text = up.BCS_Gray[8];
                BCS_Gray10.Text = up.BCS_Gray[9];
                BCS_Gray11.Text = up.BCS_Gray[10];
                BCS_Gray12.Text = up.BCS_Gray[11];
                BCS_Gray13.Text = up.BCS_Gray[12];
                BCS_Gray14.Text = up.BCS_Gray[13];
                BCS_Gray15.Text = up.BCS_Gray[14];
                BCS_Gray16.Text = up.BCS_Gray[15];
                BCS_Gray17.Text = up.BCS_Gray[16];
                BCS_Gray18.Text = up.BCS_Gray[17];
                BCS_Gray19.Text = up.BCS_Gray[18];
                BCS_Gray20.Text = up.BCS_Gray[19];

                BCS_Range1.Checked = up.BCS_Range1;
                BCS_Range2.Checked = up.BCS_Range2;
                BCS_Range3.Checked = up.BCS_Range3;

                BCS_Delay.Text = up.BCS_Delay;
                BCS_Range1_min_DBV.Text = up.BCS_Range1_min_DBV;
                BCS_Range1_max_DBV.Text = up.BCS_Range1_max_DBV;
                BCS_Range1_step.Text = up.BCS_Range1_step;
                BCS_Range2_min_DBV.Text = up.BCS_Range2_min_DBV;
                BCS_Range2_max_DBV.Text = up.BCS_Range2_max_DBV;
                BCS_Range2_step.Text = up.BCS_Range2_step;
                BCS_Range3_min_DBV.Text = up.BCS_Range3_min_DBV;
                BCS_Range3_max_DBV.Text = up.BCS_Range3_max_DBV;
                BCS_Range3_step.Text = up.BCS_Range3_step;

                BCS_Min_to_Max.Checked = up.BCS_Min_to_Max;
                BCS_Max_to_Min.Checked = up.BCS_Max_to_Min;

                //---Gamma Crush Setting---
                check_Gamma_Crush1.Checked = up.check_Gamma_Crush[0];
                check_Gamma_Crush2.Checked = up.check_Gamma_Crush[1];
                check_Gamma_Crush3.Checked = up.check_Gamma_Crush[2];
                check_Gamma_Crush4.Checked = up.check_Gamma_Crush[3];
                check_Gamma_Crush5.Checked = up.check_Gamma_Crush[4];
                check_Gamma_Crush6.Checked = up.check_Gamma_Crush[5];
                check_Gamma_Crush7.Checked = up.check_Gamma_Crush[6];
                check_Gamma_Crush8.Checked = up.check_Gamma_Crush[7];
                check_Gamma_Crush9.Checked = up.check_Gamma_Crush[8];
                check_Gamma_Crush10.Checked = up.check_Gamma_Crush[9];

                Gamma_Crush_DBV1.Text = up.Gamma_Crush_DBV[0];
                Gamma_Crush_DBV2.Text = up.Gamma_Crush_DBV[1];
                Gamma_Crush_DBV3.Text = up.Gamma_Crush_DBV[2];
                Gamma_Crush_DBV4.Text = up.Gamma_Crush_DBV[3];
                Gamma_Crush_DBV5.Text = up.Gamma_Crush_DBV[4];
                Gamma_Crush_DBV6.Text = up.Gamma_Crush_DBV[5];
                Gamma_Crush_DBV7.Text = up.Gamma_Crush_DBV[6];
                Gamma_Crush_DBV8.Text = up.Gamma_Crush_DBV[7];
                Gamma_Crush_DBV9.Text = up.Gamma_Crush_DBV[8];
                Gamma_Crush_DBV10.Text = up.Gamma_Crush_DBV[9];

                Gamma_Crush_Gray1.Text = up.Gamma_Crush_Gray[0];
                Gamma_Crush_Gray2.Text = up.Gamma_Crush_Gray[1];
                Gamma_Crush_Gray3.Text = up.Gamma_Crush_Gray[2];
                Gamma_Crush_Gray4.Text = up.Gamma_Crush_Gray[3];
                Gamma_Crush_Gray5.Text = up.Gamma_Crush_Gray[4];
                Gamma_Crush_Gray6.Text = up.Gamma_Crush_Gray[5];
                Gamma_Crush_Gray7.Text = up.Gamma_Crush_Gray[6];
                Gamma_Crush_Gray8.Text = up.Gamma_Crush_Gray[7];
                Gamma_Crush_Gray9.Text = up.Gamma_Crush_Gray[8];
                Gamma_Crush_Gray10.Text = up.Gamma_Crush_Gray[9];

                Gamma_Crush_PTN_Delay.Text = up.Gamma_Crush_PTN_Delay;
                Gamma_Crush_DBV_Delay.Text = up.Gamma_Crush_DBV_Delay;

                checkBox_Gamma_Crush_W.Checked = up.checkBox_Gamma_Crush_W;
                checkBox_Gamma_Crush_R.Checked = up.checkBox_Gamma_Crush_R;
                checkBox_Gamma_Crush_G.Checked = up.checkBox_Gamma_Crush_G;
                checkBox_Gamma_Crush_B.Checked = up.checkBox_Gamma_Crush_B;

                //---AOD GCS Setting---
                check_AOD_GCS_DBV1.Checked = up.check_AOD_GCS_DBV[0];
                check_AOD_GCS_DBV2.Checked = up.check_AOD_GCS_DBV[1];
                check_AOD_GCS_DBV3.Checked = up.check_AOD_GCS_DBV[2];

                AOD_GCS_DBV1.Text = up.AOD_GCS_DBV[0];
                AOD_GCS_DBV2.Text = up.AOD_GCS_DBV[1];
                AOD_GCS_DBV3.Text = up.AOD_GCS_DBV[2];

                AOD_GCS_Delay.Text = up.AOD_GCS_Delay;
                AOD_CODE_Delay.Text = up.AOD_CODE_Delay;               
                AOD_GCS_min_gray.Text = up.AOD_GCS_min_gray;
                AOD_GCS_max_gray.Text = up.AOD_GCS_max_gray;
                AOD_GCS_step.Text = up.AOD_GCS_step;

                AOD_GCS_Min_to_Max.Checked = up.AOD_GCS_Min_to_Max;
                AOD_GCS_Max_to_Min.Checked = up.AOD_GCS_Max_to_Min;

                //---IR Drop DeltaE Setting---
                IR_Drop_DeltaE_DBV.Text = up.IR_Drop_DeltaE_DBV;
                IR_Drop_DeltaE_Delay.Text = up.IR_Drop_DeltaE_Delay;
                IR_Drop_DeltaE_Set.Text = up.IR_Drop_DeltaE_Set;

                //---Measurement Setting---
                textBox_Aging_Time.Text = up.textBox_Aging_Time;
                textBox_Aging_DBV.Text = up.textBox_Aging_DBV;

                check_GCS_Measure.Checked = up.check_GCS_Measure;
                check_BCS_Measure.Checked = up.check_BCS_Measure;
                check_AOD_GCS_Measure.Checked = up.check_AOD_GCS_Measure;
                check_IR_Drop_DeltaE_Measure.Checked = up.check_IR_Drop_DeltaE_Measure;
                check_Gamma_Crush_Measure.Checked = up.check_Gamma_Crush_Measure;

                myFileStream.Close();
                System.Windows.Forms.MessageBox.Show("Setting has been Loaded (File Date : " + up.Saved_Date + ")");
            }
            else
            {
                myFileStream = null;
                System.Windows.Forms.MessageBox.Show("Nothing has been Loaded");
            }
            Button_Click_Enable(true);
        }
        private void button_Setting_Save_Click(object sender, EventArgs e)
        {
            Button_Click_Enable(false);

            Application.DoEvents();

            //------Get Setting Here------
            Multi_CH_Measurement_Preferences up = new Multi_CH_Measurement_Preferences();

            //---Common---
            //Save Current Date
            DateTime localDate = DateTime.Now;
            up.Saved_Date = localDate.ToString(@"yyyy.MM.dd HH:mm:ss", new CultureInfo("en-US"));

            //---PNC Setting---
            up.Set_Change_Delay = Set_Change_Delay.Text;

            up.check_SET[0] = check_SET1.Checked;
            up.check_SET[1] = check_SET2.Checked;
            up.check_SET[2] = check_SET3.Checked;
            up.check_SET[3] = check_SET4.Checked;
            up.check_SET[4] = check_SET5.Checked;
            up.check_SET[5] = check_SET6.Checked;

            up.SEQ_SET[0] = SEQ_SET1.Text;
            up.SEQ_SET[1] = SEQ_SET2.Text;
            up.SEQ_SET[2] = SEQ_SET3.Text;
            up.SEQ_SET[3] = SEQ_SET4.Text;
            up.SEQ_SET[4] = SEQ_SET5.Text;
            up.SEQ_SET[5] = SEQ_SET6.Text;

            up.textBox_Script_SET[0] = textBox_Script_SET1.Text;
            up.textBox_Script_SET[1] = textBox_Script_SET2.Text;
            up.textBox_Script_SET[2] = textBox_Script_SET3.Text;
            up.textBox_Script_SET[3] = textBox_Script_SET4.Text;
            up.textBox_Script_SET[4] = textBox_Script_SET5.Text;
            up.textBox_Script_SET[5] = textBox_Script_SET6.Text;

            //---GCS Setting---
            up.check_GCS_DBV[0] = check_GCS_DBV1.Checked;
            up.check_GCS_DBV[1] = check_GCS_DBV2.Checked;
            up.check_GCS_DBV[2] = check_GCS_DBV3.Checked;
            up.check_GCS_DBV[3] = check_GCS_DBV4.Checked;
            up.check_GCS_DBV[4] = check_GCS_DBV5.Checked;
            up.check_GCS_DBV[5] = check_GCS_DBV6.Checked;
            up.check_GCS_DBV[6] = check_GCS_DBV7.Checked;
            up.check_GCS_DBV[7] = check_GCS_DBV8.Checked;
            up.check_GCS_DBV[8] = check_GCS_DBV9.Checked;
            up.check_GCS_DBV[9] = check_GCS_DBV10.Checked;
            up.check_GCS_DBV[10] = check_GCS_DBV11.Checked;
            up.check_GCS_DBV[11] = check_GCS_DBV12.Checked;
            up.check_GCS_DBV[12] = check_GCS_DBV13.Checked;
            up.check_GCS_DBV[13] = check_GCS_DBV14.Checked;
            up.check_GCS_DBV[14] = check_GCS_DBV15.Checked;
            up.check_GCS_DBV[15] = check_GCS_DBV16.Checked;
            up.check_GCS_DBV[16] = check_GCS_DBV17.Checked;
            up.check_GCS_DBV[17] = check_GCS_DBV18.Checked;
            up.check_GCS_DBV[18] = check_GCS_DBV19.Checked;
            up.check_GCS_DBV[19] = check_GCS_DBV20.Checked;

            up.GCS_DBV[0] = GCS_DBV1.Text;
            up.GCS_DBV[1] = GCS_DBV2.Text;
            up.GCS_DBV[2] = GCS_DBV3.Text;
            up.GCS_DBV[3] = GCS_DBV4.Text;
            up.GCS_DBV[4] = GCS_DBV5.Text;
            up.GCS_DBV[5] = GCS_DBV6.Text;
            up.GCS_DBV[6] = GCS_DBV7.Text;
            up.GCS_DBV[7] = GCS_DBV8.Text;
            up.GCS_DBV[8] = GCS_DBV9.Text;
            up.GCS_DBV[9] = GCS_DBV10.Text;
            up.GCS_DBV[10] = GCS_DBV11.Text;
            up.GCS_DBV[11] = GCS_DBV12.Text;
            up.GCS_DBV[12] = GCS_DBV13.Text;
            up.GCS_DBV[13] = GCS_DBV14.Text;
            up.GCS_DBV[14] = GCS_DBV15.Text;
            up.GCS_DBV[15] = GCS_DBV16.Text;
            up.GCS_DBV[16] = GCS_DBV17.Text;
            up.GCS_DBV[17] = GCS_DBV18.Text;
            up.GCS_DBV[18] = GCS_DBV19.Text;
            up.GCS_DBV[19] = GCS_DBV20.Text;

            up.GCS_Delay = GCS_Delay.Text;
            up.GCS_min_gray = GCS_min_gray.Text;
            up.GCS_max_gray = GCS_max_gray.Text;
            up.GCS_step = GCS_step.Text;

            up.GCS_Min_to_Max = GCS_Min_to_Max.Checked;
            up.GCS_Max_to_Min = GCS_Max_to_Min.Checked;

            //---BCS Setting---
            up.check_BCS_Gray[0] = check_BCS_Gray1.Checked;
            up.check_BCS_Gray[1] = check_BCS_Gray2.Checked;
            up.check_BCS_Gray[2] = check_BCS_Gray3.Checked;
            up.check_BCS_Gray[3] = check_BCS_Gray4.Checked;
            up.check_BCS_Gray[4] = check_BCS_Gray5.Checked;
            up.check_BCS_Gray[5] = check_BCS_Gray6.Checked;
            up.check_BCS_Gray[6] = check_BCS_Gray7.Checked;
            up.check_BCS_Gray[7] = check_BCS_Gray8.Checked;
            up.check_BCS_Gray[8] = check_BCS_Gray9.Checked;
            up.check_BCS_Gray[9] = check_BCS_Gray10.Checked;
            up.check_BCS_Gray[10] = check_BCS_Gray11.Checked;
            up.check_BCS_Gray[11] = check_BCS_Gray12.Checked;
            up.check_BCS_Gray[12] = check_BCS_Gray13.Checked;
            up.check_BCS_Gray[13] = check_BCS_Gray14.Checked;
            up.check_BCS_Gray[14] = check_BCS_Gray15.Checked;
            up.check_BCS_Gray[15] = check_BCS_Gray16.Checked;
            up.check_BCS_Gray[16] = check_BCS_Gray17.Checked;
            up.check_BCS_Gray[17] = check_BCS_Gray18.Checked;
            up.check_BCS_Gray[18] = check_BCS_Gray19.Checked;
            up.check_BCS_Gray[19] = check_BCS_Gray20.Checked;

            up.BCS_Gray[0] = BCS_Gray1.Text;
            up.BCS_Gray[1] = BCS_Gray2.Text;
            up.BCS_Gray[2] = BCS_Gray3.Text;
            up.BCS_Gray[3] = BCS_Gray4.Text;
            up.BCS_Gray[4] = BCS_Gray5.Text;
            up.BCS_Gray[5] = BCS_Gray6.Text;
            up.BCS_Gray[6] = BCS_Gray7.Text;
            up.BCS_Gray[7] = BCS_Gray8.Text;
            up.BCS_Gray[8] = BCS_Gray9.Text;
            up.BCS_Gray[9] = BCS_Gray10.Text;
            up.BCS_Gray[10] = BCS_Gray11.Text;
            up.BCS_Gray[11] = BCS_Gray12.Text;
            up.BCS_Gray[12] = BCS_Gray13.Text;
            up.BCS_Gray[13] = BCS_Gray14.Text;
            up.BCS_Gray[14] = BCS_Gray15.Text;
            up.BCS_Gray[15] = BCS_Gray16.Text;
            up.BCS_Gray[16] = BCS_Gray17.Text;
            up.BCS_Gray[17] = BCS_Gray18.Text;
            up.BCS_Gray[18] = BCS_Gray19.Text;
            up.BCS_Gray[19] = BCS_Gray20.Text;

            up.BCS_Range1 = BCS_Range1.Checked;
            up.BCS_Range2 = BCS_Range2.Checked;
            up.BCS_Range3 = BCS_Range3.Checked;

            up.BCS_Delay = BCS_Delay.Text;
            up.BCS_Range1_min_DBV = BCS_Range1_min_DBV.Text;
            up.BCS_Range1_max_DBV = BCS_Range1_max_DBV.Text;
            up.BCS_Range1_step = BCS_Range1_step.Text;
            up.BCS_Range2_min_DBV = BCS_Range2_min_DBV.Text;
            up.BCS_Range2_max_DBV = BCS_Range2_max_DBV.Text;
            up.BCS_Range2_step = BCS_Range2_step.Text;
            up.BCS_Range3_min_DBV = BCS_Range3_min_DBV.Text;
            up.BCS_Range3_max_DBV = BCS_Range3_max_DBV.Text;
            up.BCS_Range3_step = BCS_Range3_step.Text;

            up.BCS_Min_to_Max = BCS_Min_to_Max.Checked;
            up.BCS_Max_to_Min = BCS_Max_to_Min.Checked;

            //---Gamma Crush Setting---
            up.check_Gamma_Crush[0] = check_Gamma_Crush1.Checked;
            up.check_Gamma_Crush[1] = check_Gamma_Crush2.Checked;
            up.check_Gamma_Crush[2] = check_Gamma_Crush3.Checked;
            up.check_Gamma_Crush[3] = check_Gamma_Crush4.Checked;
            up.check_Gamma_Crush[4] = check_Gamma_Crush5.Checked;
            up.check_Gamma_Crush[5] = check_Gamma_Crush6.Checked;
            up.check_Gamma_Crush[6] = check_Gamma_Crush7.Checked;
            up.check_Gamma_Crush[7] = check_Gamma_Crush8.Checked;
            up.check_Gamma_Crush[8] = check_Gamma_Crush9.Checked;
            up.check_Gamma_Crush[9] = check_Gamma_Crush10.Checked;

            up.Gamma_Crush_DBV[0] = Gamma_Crush_DBV1.Text;
            up.Gamma_Crush_DBV[1] = Gamma_Crush_DBV2.Text;
            up.Gamma_Crush_DBV[2] = Gamma_Crush_DBV3.Text;
            up.Gamma_Crush_DBV[3] = Gamma_Crush_DBV4.Text;
            up.Gamma_Crush_DBV[4] = Gamma_Crush_DBV5.Text;
            up.Gamma_Crush_DBV[5] = Gamma_Crush_DBV6.Text;
            up.Gamma_Crush_DBV[6] = Gamma_Crush_DBV7.Text;
            up.Gamma_Crush_DBV[7] = Gamma_Crush_DBV8.Text;
            up.Gamma_Crush_DBV[8] = Gamma_Crush_DBV9.Text;
            up.Gamma_Crush_DBV[9] = Gamma_Crush_DBV10.Text;

            up.Gamma_Crush_Gray[0] = Gamma_Crush_Gray1.Text;
            up.Gamma_Crush_Gray[1] = Gamma_Crush_Gray2.Text;
            up.Gamma_Crush_Gray[2] = Gamma_Crush_Gray3.Text;
            up.Gamma_Crush_Gray[3] = Gamma_Crush_Gray4.Text;
            up.Gamma_Crush_Gray[4] = Gamma_Crush_Gray5.Text;
            up.Gamma_Crush_Gray[5] = Gamma_Crush_Gray6.Text;
            up.Gamma_Crush_Gray[6] = Gamma_Crush_Gray7.Text;
            up.Gamma_Crush_Gray[7] = Gamma_Crush_Gray8.Text;
            up.Gamma_Crush_Gray[8] = Gamma_Crush_Gray9.Text;
            up.Gamma_Crush_Gray[9] = Gamma_Crush_Gray10.Text;

            up.Gamma_Crush_PTN_Delay = Gamma_Crush_PTN_Delay.Text;
            up.Gamma_Crush_DBV_Delay = Gamma_Crush_DBV_Delay.Text;

            up.checkBox_Gamma_Crush_W = checkBox_Gamma_Crush_W.Checked;
            up.checkBox_Gamma_Crush_R = checkBox_Gamma_Crush_R.Checked;
            up.checkBox_Gamma_Crush_G = checkBox_Gamma_Crush_G.Checked;
            up.checkBox_Gamma_Crush_B = checkBox_Gamma_Crush_B.Checked;

            //---AOD GCS Setting---
            up.check_AOD_GCS_DBV[0] = check_AOD_GCS_DBV1.Checked;
            up.check_AOD_GCS_DBV[1] = check_AOD_GCS_DBV2.Checked;
            up.check_AOD_GCS_DBV[2] = check_AOD_GCS_DBV3.Checked;

            up.AOD_GCS_DBV[0] = AOD_GCS_DBV1.Text;
            up.AOD_GCS_DBV[1] = AOD_GCS_DBV2.Text;
            up.AOD_GCS_DBV[2] = AOD_GCS_DBV3.Text;

            up.AOD_GCS_Delay = AOD_GCS_Delay.Text;
            up.AOD_CODE_Delay = AOD_CODE_Delay.Text;
            up.AOD_GCS_min_gray = AOD_GCS_min_gray.Text;
            up.AOD_GCS_max_gray = AOD_GCS_max_gray.Text;
            up.AOD_GCS_step = AOD_GCS_step.Text;

            up.AOD_GCS_Min_to_Max = AOD_GCS_Min_to_Max.Checked;
            up.AOD_GCS_Max_to_Min = AOD_GCS_Max_to_Min.Checked;

            //---IR Drop DeltaE Setting---
            up.IR_Drop_DeltaE_DBV = IR_Drop_DeltaE_DBV.Text;
            up.IR_Drop_DeltaE_Delay = IR_Drop_DeltaE_Delay.Text;
            up.IR_Drop_DeltaE_Set = IR_Drop_DeltaE_Set.Text;

            //---Measurement Setting---
            up.textBox_Aging_Time = textBox_Aging_Time.Text;
            up.textBox_Aging_DBV = textBox_Aging_DBV.Text;

            up.check_GCS_Measure = check_GCS_Measure.Checked;
            up.check_BCS_Measure = check_BCS_Measure.Checked;
            up.check_AOD_GCS_Measure = check_AOD_GCS_Measure.Checked;
            up.check_IR_Drop_DeltaE_Measure = check_IR_Drop_DeltaE_Measure.Checked;
            up.check_Gamma_Crush_Measure = check_Gamma_Crush_Measure.Checked;

            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\Optic_Measurement(10ch)";
            saveFileDialog1.Filter = "Default Extension (*.xml)|*.xml";
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.AddExtension = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter myWriter = new StreamWriter(saveFileDialog1.FileName);
                mySerializer.Serialize(myWriter, up);
                myWriter.Close();
                System.Windows.Forms.MessageBox.Show("Setting has been saved (File Date : " + up.Saved_Date + ")");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing has benn Saved");
            }
            Button_Click_Enable(true);
        }
        private void Button_Click_Enable(bool Able)
        {
            button_Setting_Load.Enabled = Able;
            button_Setting_Save.Enabled = Able;
            button_Measure.Enabled = Able;

            button_GCS_DBV_all_select.Enabled = Able;
            button_GCS_DBV_all_deselect.Enabled = Able;
            button_BCS_Gray_all_select.Enabled = Able;
            button_BCS_Gray_all_deselect.Enabled = Able;
        }

        private void All_Setting_Check()
        {
            PNC_CH_check();
            PNC_SET_check();

            if (check_GCS_Measure.Checked)
                GCS_measure = true;
            
            if(check_BCS_Measure.Checked)
            {
                BCS_measure = true;
            }
            if(check_AOD_GCS_Measure.Checked)
            {
                AOD_GCS_measure = true;
            }
            if(check_IR_Drop_DeltaE_Measure.Checked)
            {
                IR_Drop_DeltaE_measure = true;
            }
            if (check_Gamma_Crush_Measure.Checked)
            {
                Gamma_Crush_measure = true;
            }
        }
        private void button_Measure_Click(object sender, EventArgs e)
        {
            if (Is_CA_Connected() == false)
            {
                MessageBox.Show("Multi channel CA is not connected yet, please check CA status first");
                return;
            }

            Components_Enable(able: false);
            MultiChannelCheckBoxEnable(able: false);
            Clear_all();
            stop_flag = false;
            All_Setting_Check();
            Start_Time = DateTime.Now;

            Aging();
            Measurement_Progress_Bar_Setting();

            if (GCS_measure)
            {
                GCS_Measure();
            }

            if(BCS_measure)
            {
                BCS_Measure();
            }

            if (Gamma_Crush_measure)
            {
                Gamma_Crush_Measure();
            }

            if(AOD_GCS_measure)
            {
                AOD_GCS_Measure();
            }

            if(IR_Drop_DeltaE_measure)
            {
                IR_Drop_DeltaE_Measure();
            }

            if (stop_flag)
            {
                System.Windows.Forms.MessageBox.Show("Measurement Stop");
            }
            else
            {
                label_mornitoring.Text = "Measurement Finish!";
                System.Windows.Forms.MessageBox.Show("Measurement Finish!");
            }

            f1().PTN_update(127, 127, 127);
            Components_Enable(able : true);
            MultiChannelCheckBoxEnable(able: true);
        }
        private void button_Stop_Click(object sender, EventArgs e)
        {
            stop_flag = true;
            measure_flahg_change(false);
        }
        private void Measurement_Progress_Bar_Setting()
        {
            int maximum_value = 0;

            if (GCS_measure)
            {
                GCS_DBV_check();
                maximum_value += GCS_Progress_Bar_Setting();
            }

            if(BCS_measure)
            {
                BCS_Gray_check();
                maximum_value += BCS_Progress_Bar_Setting();
            }

            if(AOD_GCS_measure)
            {
                AOD_GCS_DBV_check();
                maximum_value += AOD_GCS_Progress_Bar_Setting();
            }

            if(IR_Drop_DeltaE_measure)
            {
                IR_Drop_DeltaE_Progress_Bar_Setting();
                maximum_value += 50;
            }

            if (Gamma_Crush_measure)
            {
                Gamma_Crush_Setting_check();
                maximum_value += Gamma_Crush_Progress_Bar_Setting();
            }
            ///Set ProgressBar
            progressBar_Measurement.Value = 0;
            progressBar_Measurement.Step = 1;
            progressBar_Measurement.Minimum = 0;
            progressBar_Measurement.Maximum = maximum_value;
            progressBar_Measurement.PerformStep();
        }
        private void measure_flahg_change(bool flag)
        {
            GCS_measure = flag;
            BCS_measure = flag;
            AOD_GCS_measure = flag;
            IR_Drop_DeltaE_measure = flag;
            Gamma_Crush_measure = flag;
            aging_flag = flag;
        }
        private void Aging()
        {
            int Sec = Convert.ToInt16(textBox_Aging_Time.Text);
            int Sec_temp = Sec;

            string DBV = textBox_Aging_DBV.Text.PadLeft(3, '0');//dex to hex (as a string form)
            f1().DBV_Setting(DBV);
            f1().PTN_update(255, 255, 255);

            ///Set ProgressBar
            progressBar_Measurement.Value = 0;
            progressBar_Measurement.Step = 1;
            progressBar_Measurement.Minimum = 0;
            progressBar_Measurement.Maximum = Sec;
            progressBar_Measurement.PerformStep();

            aging_flag = true;

            Application.DoEvents();
            label_mornitoring.Text = "Aging-remain time : " + (Sec_temp).ToString();
            while (aging_flag)
            {
                if (!aging_flag) break;
                if (Sec_temp > 0)
                {
                    Thread.Sleep(1000);
                    Sec_temp--;
                    textBox_Aging_Time.Text = (Sec_temp).ToString();
                    label_mornitoring.Text = "Aging-remain time : " + (Sec_temp).ToString();
                    Application.DoEvents();
                    progressBar_Measurement.PerformStep();
                }
                else
                {
                    textBox_Aging_Time.Text = Sec_temp.ToString();
                    Application.DoEvents();
                    progressBar_Measurement.PerformStep();
                    break;
                }
            }

            label_mornitoring.Text = "Aging is finished";
            textBox_Aging_Time.Text = Sec.ToString();
        }       
    }
}