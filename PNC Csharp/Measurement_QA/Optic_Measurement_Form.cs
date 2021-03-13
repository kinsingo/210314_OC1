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
using System.Threading;
using System.Xml.Serialization;

//using References
using SectionLib;
using System.IO.MemoryMappedFiles;
using System.IO;

using System.Threading.Tasks;
using System.Globalization;
using Microsoft.VisualBasic;
using BSQH_Csharp_Library;
using PNC_Csharp.Measurement_QA;

namespace PNC_Csharp
{
    public partial class Optic_Measurement_Form : Form
    {
        XmlSerializer mySerializer = new XmlSerializer(typeof(Measurement_UserPreferences));//Used For Saving and Loading Setting
        Delta_E3 delta_E3;
        AOD_Delta_E3 aod_delta_E3;
        Delta_E2 delta_E2;
        GCS_Difference gcs_difference;
        BCS_Single_or_Difference bcs_difference;
        All_At_Once all_at_once;
        Delta_E4 delta_E4;
        I_Channel channel_obj;

        //E3
        const int E3_condition_size = 12;
        bool[] checkBox_Condition1 = new bool[E3_condition_size];
        bool[] checkBox_Condition2 = new bool[E3_condition_size];
        bool[] checkBox_Condition3 = new bool[E3_condition_size];
        string[] textBox_Condition1 = new string[E3_condition_size];
        string[] textBox_Condition2 = new string[E3_condition_size];
        string[] textBox_Condition3 = new string[E3_condition_size];

        //GCS Difference
        const int GCS_Diff_condition_size = 20;
        bool[] checkBox_Diff_GCS_DBV = new bool[GCS_Diff_condition_size];
        string[] textBox_Diff_GCS_DBV = new string[GCS_Diff_condition_size];

        //BCS Difference
        const int BCS_Diff_condition_size = 11;
        bool[] checkBox_Diff_BCS_Gray = new bool[BCS_Diff_condition_size];
        string[] textBox_Diff_BCS_DBV = new string[BCS_Diff_condition_size];

        private static Optic_Measurement_Form Instance;
        public static Optic_Measurement_Form getInstance()
        {
            if (Instance == null)
                Instance = new Optic_Measurement_Form();

            return Instance;
        }

        public static bool IsIstanceNull()
        {
            if (Instance == null)
                return true;
            else
                return false;
        }

        public void Set_IChannel(I_Channel i_Channel)
        {
            channel_obj = i_Channel;
            if (channel_obj.IsMultiChannel())
                label_CA_Channel.Text = "Multi CA Channel";
            else
                label_CA_Channel.Text = "Single CA Channel";
        }


        public static void DeleteInstance()
        {
            Instance = null;
        }

        private void Initialize_Measurement_Objs()
        {
            AvgMeasMode avgMeasMode = new AvgMeasMode(radioButton_AverageMeasure_Meas_1times, radioButton_AverageMeasure_Meas_3times, radioButton_AverageMeasure_Meas_5times, textBox_AverageMeasure_Apply_Max_Lv, textBox_AverageMeasure_Delay_Before_Measure);

            delta_E4 = new Delta_E4(textBox_Show_Compared_Mipi_Data, textBox_Show_Compared_Mipi_Data2, textBox_Show_Compared_Mipi_Data3, textBox_delay_After_Condition_1, textBox_delay_After_Condition_2, textBox_delay_After_Condition_3, checkBox_IR_Drop_DBV1, checkBox_IR_Drop_DBV2, radioButton_E4_50ea_PTNs, radioButton_E4_94ea_PTNs, radioButton_1st_Condition_Measure_E4, radioButton_2nd_Condition_Measure_E4, radioButton_3rd_Condition_Measure_E4, textBox_IR_Drop_DBV1, textBox_IR_Drop_DBV2, textBox_delay_time_Delat_E4, progressBar_E4, dataGridView14, avgMeasMode);
            delta_E3 = new Delta_E3(progressBar_E3, radioButton_Min_to_Max_E3, textBox_Show_Compared_Mipi_Data, textBox_Show_Compared_Mipi_Data2, textBox_Show_Compared_Mipi_Data3, textBox_delay_After_Condition_1, textBox_delay_After_Condition_2, textBox_delay_After_Condition_3, checkBox_1st_Condition_Measure_E3, checkBox_2nd_Condition_Measure_E3, checkBox_3rd_Condition_Measure_E3, dataGridView1, dataGridView2, dataGridView3, checkBox_Condition1, checkBox_Condition2, checkBox_Condition3, textBox_Condition1, textBox_Condition2, textBox_Condition3, textBox_delay_time, textBox_Delta_E_End_Point, avgMeasMode);
            aod_delta_E3 = new AOD_Delta_E3(progressBar_E3, radioButton_Min_to_Max_E3, dataGridView13, checkBox_AOD_DBV1, checkBox_AOD_DBV2, checkBox_AOD_DBV3, checkBox_AOD_DBV4, checkBox_AOD_DBV5, checkBox_AOD_DBV6, textBox_AOD_DBV1, textBox_AOD_DBV2, textBox_AOD_DBV3, textBox_AOD_DBV4, textBox_AOD_DBV5, textBox_AOD_DBV6, textBox_delay_time, textBox_Delta_E_End_Point, avgMeasMode);
            delta_E2 = new Delta_E2(textBox_Show_Compared_Mipi_Data, textBox_Show_Compared_Mipi_Data2, textBox_Show_Compared_Mipi_Data3, textBox_delay_After_Condition_1, textBox_delay_After_Condition_2, textBox_delay_After_Condition_3, progressBar_E2, checkBox_1st_Condition_Measure_E2, checkBox_2nd_Condition_Measure_E2, checkBox_3rd_Condition_Measure_E2, Delta_E2_step_value_1, Delta_E2_step_value_4, Delta_E2_step_value_8, Delta_E2_step_value_16, textBox_Delta_E2_Max_Point, textBox_Delta_E2_End_Point, dataGridView4, dataGridView5, dataGridView6, checkBox_White_PTN_Apply_E2, radioButton_Min_to_Max_E2, textBox_delay_time_E2, avgMeasMode);
            gcs_difference = new GCS_Difference(textBox_Show_Compared_Mipi_Data, textBox_Show_Compared_Mipi_Data2, textBox_Show_Compared_Mipi_Data3, textBox_delay_After_Condition_1, textBox_delay_After_Condition_2, textBox_delay_After_Condition_3, progressBar_GCS_Diff, textBox_GCS_Diff_Max_Point, textBox_GCS_Diff_Min_Point, dataGridView7, dataGridView8, dataGridView9, radioButton_SH_Diff_Step_4, radioButton_SH_Diff_Step_8, radioButton_SH_Diff_Step_16, radioButton_Diff_2nd_and_3rd, radioButton_Diff_1st_and_3rd, radioButton_Diff_1st_and_2nd, checkBox_Diff_GCS_DBV, textBox_Diff_GCS_DBV, textBox_delay_time_Diff, avgMeasMode);
            bcs_difference = new BCS_Single_or_Difference(textBox_Show_Compared_Mipi_Data, textBox_Show_Compared_Mipi_Data2, textBox_Show_Compared_Mipi_Data3, textBox_delay_After_Condition_1, textBox_delay_After_Condition_2, textBox_delay_After_Condition_3, dataGridView10, dataGridView11, dataGridView12, BCS_Diff_step_value_1_Range1, BCS_Diff_step_value_4_Range1, BCS_Diff_step_value_8_Range1, BCS_Diff_step_value_16_Range1, BCS_Diff_step_value_1_Range2, BCS_Diff_step_value_4_Range2, BCS_Diff_step_value_8_Range2, BCS_Diff_step_value_16_Range2, BCS_Diff_step_value_1_Range3, BCS_Diff_step_value_4_Range3, BCS_Diff_step_value_8_Range3, BCS_Diff_step_value_16_Range3, checkBox_Diff_BCS_Gray, textBox_Diff_BCS_DBV, progressBar_BCS_Diff, checkBox_BCS_Diif_Range_1, checkBox_BCS_Diif_Range_2, checkBox_BCS_Diif_Range_3, radioButton_BCS_Diff_Single, radioButton_BCS_Diff_Dual, radioButton_BCS_Diff_Triple, textBox_BCS_Diff_Sub_1_Max_Point, textBox_BCS_Diff_Sub_2_Max_Point, textBox_BCS_Diff_Sub_3_Max_Point, textBox_BCS_Diff_Sub_1_Min_Point, textBox_BCS_Diff_Sub_2_Min_Point, textBox_BCS_Diff_Sub_3_Min_Point, textBox_delay_time_Diff_BCS, avgMeasMode);
            all_at_once = new All_At_Once(progressBar_All_At_Once, checkBox_All_At_Once_Delta_E4, checkBox_All_At_Once_E3, checkBox_All_At_Once_E2, checkBox_All_At_Once_Diff_GCS, checkBox_All_At_Once_Diff_BCS, checkBox_All_At_Once_AOD_GCS, textBox_Aging_Sec, textBox_Aging_Sec_Read);
        }

        private Optic_Measurement_Form()
        {
            InitializeComponent();
            dataGridView_Inialize(1, "Gray");
            dataGridView_Inialize(2, "Gray");
            dataGridView_Inialize(3, "Gray");
            dataGridView_Inialize(4, "DBV");
            dataGridView_Inialize(5, "DBV");
            dataGridView_Inialize(6, "DBV");
            dataGridView_Inialize(7, "Gray");
            dataGridView_Inialize(8, "Gray");
            dataGridView_Inialize(9, "Gray");
            dataGridView_Inialize(10, "DBV");
            dataGridView_Inialize(11, "DBV");
            dataGridView_Inialize(12, "DBV");
            dataGridView_Inialize(13, "Gray");
            dataGridView_Inialize(14, "PTN");
            dataGridView_Inialize(15, "TBD");

            button_Script_Transform.PerformClick();
            button_Script_Transform2.PerformClick();
            button_Script_Transform3.PerformClick();
        }

        private void Optic_Measurement_Form_Load(object sender, EventArgs e)
        {
            label_Model_Indicate.ForeColor = f1().current_model.Get_Back_Ground_Color();
            label_Model_DBV_Max_Indicate.ForeColor = f1().current_model.Get_Back_Ground_Color();
            label_Model_Indicate.Text = "Model:" + f1().current_model.Get_Current_Model_Name().ToString();
            label_Model_DBV_Max_Indicate.Text = "DBV Max:" + f1().current_model.get_DBV_Max().ToString();
            try
            {
                button_Load_Setting.PerformClick();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Loadindg Settings Failed('prefs.xml'needs to be updated)");
                Button_Click_Enable(true);
            }
            DBV_CheckBox_Status_Update(sender, e);
            Initialize_Measurement_Objs();
        }


        private void Update_Diff_DBV_Checkbox_And_Textbox()
        {
            //Textbox to String
            textBox_Diff_GCS_DBV[0] = textBox1_Diff.Text;
            textBox_Diff_GCS_DBV[1] = textBox2_Diff.Text;
            textBox_Diff_GCS_DBV[2] = textBox3_Diff.Text;
            textBox_Diff_GCS_DBV[3] = textBox4_Diff.Text;
            textBox_Diff_GCS_DBV[4] = textBox5_Diff.Text;
            textBox_Diff_GCS_DBV[5] = textBox6_Diff.Text;
            textBox_Diff_GCS_DBV[6] = textBox7_Diff.Text;
            textBox_Diff_GCS_DBV[7] = textBox8_Diff.Text;
            textBox_Diff_GCS_DBV[8] = textBox9_Diff.Text;
            textBox_Diff_GCS_DBV[9] = textBox10_Diff.Text;
            textBox_Diff_GCS_DBV[10] = textBox11_Diff.Text;
            textBox_Diff_GCS_DBV[11] = textBox12_Diff.Text;
            textBox_Diff_GCS_DBV[12] = textBox13_Diff.Text;
            textBox_Diff_GCS_DBV[13] = textBox14_Diff.Text;
            textBox_Diff_GCS_DBV[14] = textBox15_Diff.Text;
            textBox_Diff_GCS_DBV[15] = textBox16_Diff.Text;
            textBox_Diff_GCS_DBV[16] = textBox17_Diff.Text;
            textBox_Diff_GCS_DBV[17] = textBox18_Diff.Text;
            textBox_Diff_GCS_DBV[18] = textBox19_Diff.Text;
            textBox_Diff_GCS_DBV[19] = textBox20_Diff.Text;


            //CheckBox to Bool
            checkBox_Diff_GCS_DBV[0] = checkBox1_Diff.Checked;
            checkBox_Diff_GCS_DBV[1] = checkBox2_Diff.Checked;
            checkBox_Diff_GCS_DBV[2] = checkBox3_Diff.Checked;
            checkBox_Diff_GCS_DBV[3] = checkBox4_Diff.Checked;
            checkBox_Diff_GCS_DBV[4] = checkBox5_Diff.Checked;
            checkBox_Diff_GCS_DBV[5] = checkBox6_Diff.Checked;
            checkBox_Diff_GCS_DBV[6] = checkBox7_Diff.Checked;
            checkBox_Diff_GCS_DBV[7] = checkBox8_Diff.Checked;
            checkBox_Diff_GCS_DBV[8] = checkBox9_Diff.Checked;
            checkBox_Diff_GCS_DBV[9] = checkBox10_Diff.Checked;
            checkBox_Diff_GCS_DBV[10] = checkBox11_Diff.Checked;
            checkBox_Diff_GCS_DBV[11] = checkBox12_Diff.Checked;
            checkBox_Diff_GCS_DBV[12] = checkBox13_Diff.Checked;
            checkBox_Diff_GCS_DBV[13] = checkBox14_Diff.Checked;
            checkBox_Diff_GCS_DBV[14] = checkBox15_Diff.Checked;
            checkBox_Diff_GCS_DBV[15] = checkBox16_Diff.Checked;
            checkBox_Diff_GCS_DBV[16] = checkBox17_Diff.Checked;
            checkBox_Diff_GCS_DBV[17] = checkBox18_Diff.Checked;
            checkBox_Diff_GCS_DBV[18] = checkBox19_Diff.Checked;
            checkBox_Diff_GCS_DBV[19] = checkBox20_Diff.Checked;
        }


        private void Update_Diff_BCS_Gray_Checkbox_And_Textbox()
        {
            //Textbox to String
            textBox_Diff_BCS_DBV[0] = textBox_BCS_Diff_Gray_P1.Text;
            textBox_Diff_BCS_DBV[1] = textBox_BCS_Diff_Gray_P2.Text;
            textBox_Diff_BCS_DBV[2] = textBox_BCS_Diff_Gray_P3.Text;
            textBox_Diff_BCS_DBV[3] = textBox_BCS_Diff_Gray_P4.Text;
            textBox_Diff_BCS_DBV[4] = textBox_BCS_Diff_Gray_P5.Text;
            textBox_Diff_BCS_DBV[5] = textBox_BCS_Diff_Gray_P6.Text;
            textBox_Diff_BCS_DBV[6] = textBox_BCS_Diff_Gray_P7.Text;
            textBox_Diff_BCS_DBV[7] = textBox_BCS_Diff_Gray_P8.Text;
            textBox_Diff_BCS_DBV[8] = textBox_BCS_Diff_Gray_P9.Text;
            textBox_Diff_BCS_DBV[9] = textBox_BCS_Diff_Gray_P10.Text;
            textBox_Diff_BCS_DBV[10] = textBox_BCS_Diff_Gray_P11.Text;

            //CheckBox to Bool
            checkBox_Diff_BCS_Gray[0] = checkBox1_BCS_Diff_Gray_P1.Checked;
            checkBox_Diff_BCS_Gray[1] = checkBox1_BCS_Diff_Gray_P2.Checked;
            checkBox_Diff_BCS_Gray[2] = checkBox1_BCS_Diff_Gray_P3.Checked;
            checkBox_Diff_BCS_Gray[3] = checkBox1_BCS_Diff_Gray_P4.Checked;
            checkBox_Diff_BCS_Gray[4] = checkBox1_BCS_Diff_Gray_P5.Checked;
            checkBox_Diff_BCS_Gray[5] = checkBox1_BCS_Diff_Gray_P6.Checked;
            checkBox_Diff_BCS_Gray[6] = checkBox1_BCS_Diff_Gray_P7.Checked;
            checkBox_Diff_BCS_Gray[7] = checkBox1_BCS_Diff_Gray_P8.Checked;
            checkBox_Diff_BCS_Gray[8] = checkBox1_BCS_Diff_Gray_P9.Checked;
            checkBox_Diff_BCS_Gray[9] = checkBox1_BCS_Diff_Gray_P10.Checked;
            checkBox_Diff_BCS_Gray[10] = checkBox1_BCS_Diff_Gray_P11.Checked;
        }

        private void Update_E3_DBV_Checkbox_And_Textbox()
        {
            //---Condition---
            //Textbox to String
            textBox_Condition1[0] = textBox1_1.Text;
            textBox_Condition1[1] = textBox2_1.Text;
            textBox_Condition1[2] = textBox3_1.Text;
            textBox_Condition1[3] = textBox4_1.Text;
            textBox_Condition1[4] = textBox5_1.Text;
            textBox_Condition1[5] = textBox6_1.Text;
            textBox_Condition1[6] = textBox7_1.Text;
            textBox_Condition1[7] = textBox8_1.Text;
            textBox_Condition1[8] = textBox9_1.Text;
            textBox_Condition1[9] = textBox10_1.Text;
            textBox_Condition1[10] = textBox11_1.Text;
            textBox_Condition1[11] = textBox12_1.Text;

            //CheckBox to Bool
            checkBox_Condition1[0] = checkBox1_1.Checked;
            checkBox_Condition1[1] = checkBox2_1.Checked;
            checkBox_Condition1[2] = checkBox3_1.Checked;
            checkBox_Condition1[3] = checkBox4_1.Checked;
            checkBox_Condition1[4] = checkBox5_1.Checked;
            checkBox_Condition1[5] = checkBox6_1.Checked;
            checkBox_Condition1[6] = checkBox7_1.Checked;
            checkBox_Condition1[7] = checkBox8_1.Checked;
            checkBox_Condition1[8] = checkBox9_1.Checked;
            checkBox_Condition1[9] = checkBox10_1.Checked;
            checkBox_Condition1[10] = checkBox11_1.Checked;
            checkBox_Condition1[11] = checkBox12_1.Checked;


            //---Condition2---
            //Textbox to String
            textBox_Condition2[0] = textBox1_2.Text;
            textBox_Condition2[1] = textBox2_2.Text;
            textBox_Condition2[2] = textBox3_2.Text;
            textBox_Condition2[3] = textBox4_2.Text;
            textBox_Condition2[4] = textBox5_2.Text;
            textBox_Condition2[5] = textBox6_2.Text;
            textBox_Condition2[6] = textBox7_2.Text;
            textBox_Condition2[7] = textBox8_2.Text;
            textBox_Condition2[8] = textBox9_2.Text;
            textBox_Condition2[9] = textBox10_2.Text;
            textBox_Condition2[10] = textBox11_2.Text;
            textBox_Condition2[11] = textBox12_2.Text;

            //CheckBox to Bool
            checkBox_Condition2[0] = checkBox1_2.Checked;
            checkBox_Condition2[1] = checkBox2_2.Checked;
            checkBox_Condition2[2] = checkBox3_2.Checked;
            checkBox_Condition2[3] = checkBox4_2.Checked;
            checkBox_Condition2[4] = checkBox5_2.Checked;
            checkBox_Condition2[5] = checkBox6_2.Checked;
            checkBox_Condition2[6] = checkBox7_2.Checked;
            checkBox_Condition2[7] = checkBox8_2.Checked;
            checkBox_Condition2[8] = checkBox9_2.Checked;
            checkBox_Condition2[9] = checkBox10_2.Checked;
            checkBox_Condition2[10] = checkBox11_2.Checked;
            checkBox_Condition2[11] = checkBox12_2.Checked;


            //---Condition3---
            //Textbox to String
            textBox_Condition3[0] = textBox1_3.Text;
            textBox_Condition3[1] = textBox2_3.Text;
            textBox_Condition3[2] = textBox3_3.Text;
            textBox_Condition3[3] = textBox4_3.Text;
            textBox_Condition3[4] = textBox5_3.Text;
            textBox_Condition3[5] = textBox6_3.Text;
            textBox_Condition3[6] = textBox7_3.Text;
            textBox_Condition3[7] = textBox8_3.Text;
            textBox_Condition3[8] = textBox9_3.Text;
            textBox_Condition3[9] = textBox10_3.Text;
            textBox_Condition3[10] = textBox11_3.Text;
            textBox_Condition3[11] = textBox12_3.Text;

            //CheckBox to Bool
            checkBox_Condition3[0] = checkBox1_3.Checked;
            checkBox_Condition3[1] = checkBox2_3.Checked;
            checkBox_Condition3[2] = checkBox3_3.Checked;
            checkBox_Condition3[3] = checkBox4_3.Checked;
            checkBox_Condition3[4] = checkBox5_3.Checked;
            checkBox_Condition3[5] = checkBox6_3.Checked;
            checkBox_Condition3[6] = checkBox7_3.Checked;
            checkBox_Condition3[7] = checkBox8_3.Checked;
            checkBox_Condition3[8] = checkBox9_3.Checked;
            checkBox_Condition3[9] = checkBox10_3.Checked;
            checkBox_Condition3[10] = checkBox11_3.Checked;
            checkBox_Condition3[11] = checkBox12_3.Checked;

        }
        


        private void dataGridView_Inialize(int condition, string first)
        {
            DataGridView datagridview;
            if (condition == 1) datagridview = dataGridView1;
            else if (condition == 2) datagridview = dataGridView2;
            else if (condition == 3) datagridview = dataGridView3;
            else if (condition == 4) datagridview = dataGridView4;
            else if (condition == 5) datagridview = dataGridView5;
            else if (condition == 6) datagridview = dataGridView6;
            else if (condition == 7) datagridview = dataGridView7;
            else if (condition == 8) datagridview = dataGridView8;
            else if (condition == 9) datagridview = dataGridView9;
            else if (condition == 10) datagridview = dataGridView10;
            else if (condition == 11) datagridview = dataGridView11;
            else if (condition == 12) datagridview = dataGridView12;
            else if (condition == 13) datagridview = dataGridView13;//AOD
            else if (condition == 14) datagridview = dataGridView14;//Delta_E4
            else if (condition == 15) datagridview = dataGridView15;//TBD
            else datagridview = null;

            //Set the datagridview's EnableHeadersVisualStyles to false to get the header cell to accept the color change
            datagridview.EnableHeadersVisualStyles = false;
            datagridview.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            datagridview.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;

            //Delta E Data Grid view initialize


            for (int ch = 0; ch < 10; ch++)
            {
                datagridview.Columns.Add(first, first);
                datagridview.Columns.Add("x", "x");
                datagridview.Columns.Add("y", "y");
                datagridview.Columns.Add("Lv", "Lv");
                if (condition == 1 || condition == 2 || condition == 3 || condition == 13) datagridview.Columns.Add("Delta E*", "E3");
                else if (condition == 4 || condition == 5 || condition == 6) datagridview.Columns.Add("Delta E*", "E2");
                else if (condition == 14) datagridview.Columns.Add("Delta E*", "E4");
            }

            foreach (DataGridViewColumn column in datagridview.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Width = 60;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            //change color for X/Y/Lv Measured area  


            //----------
            for (int ch = 0; ch < 10; ch++)
            {
                int Each_Ch_column_size;
                if (condition == 1 || condition == 2 || condition == 3 || condition == 4 || condition == 5 || condition == 6 || condition == 13 || condition == 14)
                    Each_Ch_column_size = 5;
                else
                    Each_Ch_column_size = 4;

                datagridview.Columns[(Each_Ch_column_size * ch) + 0].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                datagridview.Columns[(Each_Ch_column_size * ch) + 0].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                datagridview.Columns[(Each_Ch_column_size * ch) + 0].HeaderCell.Style.BackColor = System.Drawing.Color.Gray;
                datagridview.Columns[(Each_Ch_column_size * ch) + 0].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                datagridview.Columns[(Each_Ch_column_size * ch) + 0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;


                for (int i = 1; i <= 3; i++)
                {
                    if (condition == 1 || condition == 2 || condition == 3)
                    {
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(255, 100, 100);
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 200, 200);
                    }
                    else if (condition == 4 || condition == 5 || condition == 6)
                    {
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(100, 255, 100);
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(200, 255, 200);
                    }
                    else if (condition == 7 || condition == 8 || condition == 9)
                    {
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(100, 100, 255);
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(200, 200, 255);
                    }
                    else if (condition == 10 || condition == 11 || condition == 12)
                    {
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(255, 100, 255);
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 200, 255);
                    }
                    else if (condition == 13)
                    {
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(100, 255, 255);
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(200, 255, 255);
                    }
                    else if (condition == 14)
                    {
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(255, 150, 150);
                        datagridview.Columns[(Each_Ch_column_size * ch) + i].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 230, 230);
                    }

                    datagridview.Columns[(Each_Ch_column_size * ch) + i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    datagridview.Columns[(Each_Ch_column_size * ch) + i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                    datagridview.Columns[(Each_Ch_column_size * ch) + i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                }

                if (condition == 1 || condition == 2 || condition == 3 || condition == 4 || condition == 5 || condition == 6 || condition == 13 || condition == 14)
                {
                    datagridview.Columns[(Each_Ch_column_size * ch) + 4].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                    datagridview.Columns[(Each_Ch_column_size * ch) + 4].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    datagridview.Columns[(Each_Ch_column_size * ch) + 4].HeaderCell.Style.BackColor = System.Drawing.Color.Coral;
                    datagridview.Columns[(Each_Ch_column_size * ch) + 4].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
                    datagridview.Columns[(Each_Ch_column_size * ch) + 4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                }
            }
        }





        private void button_Clear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            dataGridView5.Rows.Clear();
            dataGridView6.Rows.Clear();
            dataGridView7.Rows.Clear();
            dataGridView8.Rows.Clear();
            dataGridView9.Rows.Clear();
            dataGridView10.Rows.Clear();
            dataGridView11.Rows.Clear();
            dataGridView12.Rows.Clear();
            dataGridView13.Rows.Clear();
            dataGridView14.Rows.Clear();
            dataGridView15.Rows.Clear();
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            delta_E3.Set_Availability(able: false);
            aod_delta_E3.Set_Availability(able: false);
            delta_E2.Set_Availability(able: false);
            gcs_difference.Set_Availability(able: false);
            bcs_difference.Set_Availability(able: false);
            all_at_once.Set_Availability(able: false);
            delta_E4.Set_Availability(able: false);
        }

        private void button_Hide_Click_1(object sender, EventArgs e)
        {
            this.Visible = false;
        }


        private void button_Script_Transform_Click(object sender, EventArgs e)
        {
            PNC_Scirp.Script_Transform(textBox_Mipi_Script_Condition1, textBox_Show_Compared_Mipi_Data);
        }

        private void button_Script_Transform2_Click(object sender, EventArgs e)
        {
            PNC_Scirp.Script_Transform(textBox_Mipi_Script_Condition2, textBox_Show_Compared_Mipi_Data2);
        }

        private void button_Script_Transform3_Click(object sender, EventArgs e)
        {
            PNC_Scirp.Script_Transform(textBox_Mipi_Script_Condition3, textBox_Show_Compared_Mipi_Data3);
        }

        private void button_Script_Clear_Click(object sender, EventArgs e)
        {
            textBox_Mipi_Script_Condition1.Clear();
            textBox_Show_Compared_Mipi_Data.Clear();
        }

        private void button_Script_Clear2_Click(object sender, EventArgs e)
        {
            textBox_Mipi_Script_Condition2.Clear();
            textBox_Show_Compared_Mipi_Data2.Clear();
        }

        private void button_Script_Clear3_Click(object sender, EventArgs e)
        {
            textBox_Mipi_Script_Condition3.Clear();
            textBox_Show_Compared_Mipi_Data3.Clear();
        }

        private void Button_Click_Enable(bool Able)
        {
            //Button
            Delta_E_calculation_btn.Enabled = Able;
            Delta_E2_calculation_btn.Enabled = Able;
            button_Clear.Enabled = Able;
            button_Save_Setting.Enabled = Able;
            button_Load_Setting.Enabled = Able;
            button_SH_GCS_Difference_Measure.Enabled = Able;
            button_All_At_Once.Enabled = Able;
            button_AOD_GCS.Enabled = Able;
            IR_Drop_Delta_E_btn.Enabled = Able;
        }

        private void AOD_GCS_Textbox_CheckBox_Enable(bool Able)
        {
            textBox_AOD_DBV1.Enabled = Able;
            textBox_AOD_DBV2.Enabled = Able;
            textBox_AOD_DBV3.Enabled = Able;
            textBox_AOD_DBV4.Enabled = Able;
            textBox_AOD_DBV5.Enabled = Able;
            textBox_AOD_DBV6.Enabled = Able;

            checkBox_AOD_DBV1.Enabled = Able;
            checkBox_AOD_DBV2.Enabled = Able;
            checkBox_AOD_DBV3.Enabled = Able;
            checkBox_AOD_DBV4.Enabled = Able;
            checkBox_AOD_DBV5.Enabled = Able;
            checkBox_AOD_DBV6.Enabled = Able;
        }


        private void Delta_E3_Textbox_CheckBox_Radiobutton_Enable(bool Able)
        {
            //CheckBox
            checkBox_1st_Condition_Measure_E3.Enabled = Able;
            checkBox_2nd_Condition_Measure_E3.Enabled = Able;
            checkBox_3rd_Condition_Measure_E3.Enabled = Able;

            //TextBox
            textBox_delay_time.Enabled = Able;
            textBox_Delta_E_End_Point.Enabled = Able;

            //Radiobutton
            radioButton_Min_to_Max_E3.Enabled = Able;
            radioButton_Max_to_Min_E3.Enabled = Able;

            //---Condition1---
            textBox1_1.Enabled = Able;
            textBox2_1.Enabled = Able;
            textBox3_1.Enabled = Able;
            textBox4_1.Enabled = Able;
            textBox5_1.Enabled = Able;
            textBox6_1.Enabled = Able;
            textBox7_1.Enabled = Able;
            textBox8_1.Enabled = Able;
            textBox9_1.Enabled = Able;
            textBox10_1.Enabled = Able;
            textBox11_1.Enabled = Able;
            textBox12_1.Enabled = Able;

            //CheckBox to Bool
            checkBox1_1.Enabled = Able;
            checkBox2_1.Enabled = Able;
            checkBox3_1.Enabled = Able;
            checkBox4_1.Enabled = Able;
            checkBox5_1.Enabled = Able;
            checkBox6_1.Enabled = Able;
            checkBox7_1.Enabled = Able;
            checkBox8_1.Enabled = Able;
            checkBox9_1.Enabled = Able;
            checkBox10_1.Enabled = Able;
            checkBox11_1.Enabled = Able;
            checkBox12_1.Enabled = Able;


            //---Condition2---
            //Textbox to String
            textBox1_2.Enabled = Able;
            textBox2_2.Enabled = Able;
            textBox3_2.Enabled = Able;
            textBox4_2.Enabled = Able;
            textBox5_2.Enabled = Able;
            textBox6_2.Enabled = Able;
            textBox7_2.Enabled = Able;
            textBox8_2.Enabled = Able;
            textBox9_2.Enabled = Able;
            textBox10_2.Enabled = Able;
            textBox11_2.Enabled = Able;
            textBox12_2.Enabled = Able;

            //CheckBox to Bool
            checkBox1_2.Enabled = Able;
            checkBox2_2.Enabled = Able;
            checkBox3_2.Enabled = Able;
            checkBox4_2.Enabled = Able;
            checkBox5_2.Enabled = Able;
            checkBox6_2.Enabled = Able;
            checkBox7_2.Enabled = Able;
            checkBox8_2.Enabled = Able;
            checkBox9_2.Enabled = Able;
            checkBox10_2.Enabled = Able;
            checkBox11_2.Enabled = Able;
            checkBox12_2.Enabled = Able;


            //---Condition3---
            //Textbox to String
            textBox1_3.Enabled = Able;
            textBox2_3.Enabled = Able;
            textBox3_3.Enabled = Able;
            textBox4_3.Enabled = Able;
            textBox5_3.Enabled = Able;
            textBox6_3.Enabled = Able;
            textBox7_3.Enabled = Able;
            textBox8_3.Enabled = Able;
            textBox9_3.Enabled = Able;
            textBox10_3.Enabled = Able;
            textBox11_3.Enabled = Able;
            textBox12_3.Enabled = Able;

            //CheckBox to Bool
            checkBox1_3.Enabled = Able;
            checkBox2_3.Enabled = Able;
            checkBox3_3.Enabled = Able;
            checkBox4_3.Enabled = Able;
            checkBox5_3.Enabled = Able;
            checkBox6_3.Enabled = Able;
            checkBox7_3.Enabled = Able;
            checkBox8_3.Enabled = Able;
            checkBox9_3.Enabled = Able;
            checkBox10_3.Enabled = Able;
            checkBox11_3.Enabled = Able;
            checkBox12_3.Enabled = Able;

        }

        private void Delta_E3_calculation_btn_Click(object sender, EventArgs e)
        {
            Delta_E3_calculation();
        }

        public void Delta_E3_calculation()
        {
            if (channel_obj.IsCAConnected() == false)
            {
                System.Windows.Forms.MessageBox.Show("Please Connect CA310 or CA410 First");
            }
            else
            {
                Button_Click_Enable(false);
                Delta_E3_Textbox_CheckBox_Radiobutton_Enable(false);

                Update_E3_DBV_Checkbox_And_Textbox();
                delta_E3.Set_Availability(able: true);
                delta_E3.MeasureAll(channel_obj);

                Button_Click_Enable(true);
                Delta_E3_Textbox_CheckBox_Radiobutton_Enable(true);
            }
        }



        private void IR_Drop_Delta_E_btn_Click(object sender, EventArgs e)
        {
            IR_Drop_Delta_E();
        }

        public void IR_Drop_Delta_E()
        {
            if (channel_obj.IsCAConnected() == false)
            {
                System.Windows.Forms.MessageBox.Show("Please Connect CA310 or CA410 First");
            }
            else
            {
                Button_Click_Enable(false);

                delta_E4.Set_Availability(able: true);
                delta_E4.MeasureAll(channel_obj);

                Button_Click_Enable(true);
            }
        }

        private void button_Save_Setting_Click(object sender, EventArgs e)
        {
            //------Get Setting Here------
            Measurement_UserPreferences up = new Measurement_UserPreferences();

            //---Common---
            //Save Current Date
            DateTime localDate = DateTime.Now;
            up.Saved_Date = localDate.ToString(@"yyyy.MM.dd HH:mm:ss", new CultureInfo("en-US"));

            //Textbox to String
            up.textBox_Mipi_Script_Condition1 = textBox_Mipi_Script_Condition1.Text;
            up.textBox_Mipi_Script_Condition2 = textBox_Mipi_Script_Condition2.Text;
            up.textBox_Mipi_Script_Condition3 = textBox_Mipi_Script_Condition3.Text;

            up.textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data.Text;
            up.textBox_Show_Compared_Mipi_Data2 = textBox_Show_Compared_Mipi_Data2.Text;
            up.textBox_Show_Compared_Mipi_Data3 = textBox_Show_Compared_Mipi_Data3.Text;

            up.textBox_Delta_E_End_Point = textBox_Delta_E_End_Point.Text;
            up.textBox_delay_time = textBox_delay_time.Text;

            up.textBox_Delta_E2_End_Point = textBox_Delta_E2_End_Point.Text;
            up.textBox_Delta_E2_Max_Point = textBox_Delta_E2_Max_Point.Text;
            up.textBox_delay_time_E2 = textBox_delay_time_E2.Text;


            up.textBox_delay_After_Condition_1 = textBox_delay_After_Condition_1.Text;
            up.textBox_delay_After_Condition_2 = textBox_delay_After_Condition_2.Text;
            up.textBox_delay_After_Condition_3 = textBox_delay_After_Condition_3.Text;

            up.textBox_Aging_Sec = textBox_Aging_Sec.Text;

            //CheckBox to Bool
            up.checkBox_3rd_Condition_Measure = checkBox_3rd_Condition_Measure_E3.Checked;
            up.checkBox_2nd_Condition_Measure = checkBox_2nd_Condition_Measure_E3.Checked;
            up.checkBox_1st_Condition_Measure = checkBox_1st_Condition_Measure_E3.Checked;
            up.checkBox_1st_Condition_Measure_E2 = checkBox_1st_Condition_Measure_E2.Checked;
            up.checkBox_2nd_Condition_Measure_E2 = checkBox_2nd_Condition_Measure_E2.Checked;
            up.checkBox_3rd_Condition_Measure_E2 = checkBox_3rd_Condition_Measure_E2.Checked;
            up.checkBox_White_PTN_Apply_E2 = checkBox_White_PTN_Apply_E2.Checked;

            //RadioButton to Bool
            up.radioButton_Max_to_Min_E3 = radioButton_Max_to_Min_E3.Checked;
            up.radioButton_Min_to_Max_E3 = radioButton_Min_to_Max_E3.Checked;
            up.step_value_1 = Delta_E2_step_value_1.Checked;
            up.step_value_4 = Delta_E2_step_value_4.Checked;
            up.step_value_8 = Delta_E2_step_value_8.Checked;
            up.step_value_16 = Delta_E2_step_value_16.Checked;
            up.radioButton_Min_to_Max_E2 = radioButton_Min_to_Max_E2.Checked;
            up.radioButton_Max_to_Min_E2 = radioButton_Max_to_Min_E2.Checked;

            //---Condition---
            //Textbox to String
            up.textBox_Condition1[0] = textBox1_1.Text;
            up.textBox_Condition1[1] = textBox2_1.Text;
            up.textBox_Condition1[2] = textBox3_1.Text;
            up.textBox_Condition1[3] = textBox4_1.Text;
            up.textBox_Condition1[4] = textBox5_1.Text;
            up.textBox_Condition1[5] = textBox6_1.Text;
            up.textBox_Condition1[6] = textBox7_1.Text;
            up.textBox_Condition1[7] = textBox8_1.Text;
            up.textBox_Condition1[8] = textBox9_1.Text;
            up.textBox_Condition1[9] = textBox10_1.Text;
            up.textBox_Condition1[10] = textBox11_1.Text;
            up.textBox_Condition1[11] = textBox12_1.Text;

            //CheckBox to Bool
            up.checkBox_Condition1[0] = checkBox1_1.Checked;
            up.checkBox_Condition1[1] = checkBox2_1.Checked;
            up.checkBox_Condition1[2] = checkBox3_1.Checked;
            up.checkBox_Condition1[3] = checkBox4_1.Checked;
            up.checkBox_Condition1[4] = checkBox5_1.Checked;
            up.checkBox_Condition1[5] = checkBox6_1.Checked;
            up.checkBox_Condition1[6] = checkBox7_1.Checked;
            up.checkBox_Condition1[7] = checkBox8_1.Checked;
            up.checkBox_Condition1[8] = checkBox9_1.Checked;
            up.checkBox_Condition1[9] = checkBox10_1.Checked;
            up.checkBox_Condition1[10] = checkBox11_1.Checked;
            up.checkBox_Condition1[11] = checkBox12_1.Checked;


            //---Condition2---
            //Textbox to String
            up.textBox_Condition2[0] = textBox1_2.Text;
            up.textBox_Condition2[1] = textBox2_2.Text;
            up.textBox_Condition2[2] = textBox3_2.Text;
            up.textBox_Condition2[3] = textBox4_2.Text;
            up.textBox_Condition2[4] = textBox5_2.Text;
            up.textBox_Condition2[5] = textBox6_2.Text;
            up.textBox_Condition2[6] = textBox7_2.Text;
            up.textBox_Condition2[7] = textBox8_2.Text;
            up.textBox_Condition2[8] = textBox9_2.Text;
            up.textBox_Condition2[9] = textBox10_2.Text;
            up.textBox_Condition2[10] = textBox11_2.Text;
            up.textBox_Condition2[11] = textBox12_2.Text;

            //CheckBox to Bool
            up.checkBox_Condition2[0] = checkBox1_2.Checked;
            up.checkBox_Condition2[1] = checkBox2_2.Checked;
            up.checkBox_Condition2[2] = checkBox3_2.Checked;
            up.checkBox_Condition2[3] = checkBox4_2.Checked;
            up.checkBox_Condition2[4] = checkBox5_2.Checked;
            up.checkBox_Condition2[5] = checkBox6_2.Checked;
            up.checkBox_Condition2[6] = checkBox7_2.Checked;
            up.checkBox_Condition2[7] = checkBox8_2.Checked;
            up.checkBox_Condition2[8] = checkBox9_2.Checked;
            up.checkBox_Condition2[9] = checkBox10_2.Checked;
            up.checkBox_Condition2[10] = checkBox11_2.Checked;
            up.checkBox_Condition2[11] = checkBox12_2.Checked;


            //---Condition3---
            //Textbox to String
            up.textBox_Condition3[0] = textBox1_3.Text;
            up.textBox_Condition3[1] = textBox2_3.Text;
            up.textBox_Condition3[2] = textBox3_3.Text;
            up.textBox_Condition3[3] = textBox4_3.Text;
            up.textBox_Condition3[4] = textBox5_3.Text;
            up.textBox_Condition3[5] = textBox6_3.Text;
            up.textBox_Condition3[6] = textBox7_3.Text;
            up.textBox_Condition3[7] = textBox8_3.Text;
            up.textBox_Condition3[8] = textBox9_3.Text;
            up.textBox_Condition3[9] = textBox10_3.Text;
            up.textBox_Condition3[10] = textBox11_3.Text;
            up.textBox_Condition3[11] = textBox12_3.Text;

            //CheckBox to Bool
            up.checkBox_Condition3[0] = checkBox1_3.Checked;
            up.checkBox_Condition3[1] = checkBox2_3.Checked;
            up.checkBox_Condition3[2] = checkBox3_3.Checked;
            up.checkBox_Condition3[3] = checkBox4_3.Checked;
            up.checkBox_Condition3[4] = checkBox5_3.Checked;
            up.checkBox_Condition3[5] = checkBox6_3.Checked;
            up.checkBox_Condition3[6] = checkBox7_3.Checked;
            up.checkBox_Condition3[7] = checkBox8_3.Checked;
            up.checkBox_Condition3[8] = checkBox9_3.Checked;
            up.checkBox_Condition3[9] = checkBox10_3.Checked;
            up.checkBox_Condition3[10] = checkBox11_3.Checked;
            up.checkBox_Condition3[11] = checkBox12_3.Checked;

            //------------------------

            //-------GCS Diff Measure Related---------
            up.checkBox_Diff[0] = checkBox1_Diff.Checked;
            up.checkBox_Diff[1] = checkBox2_Diff.Checked;
            up.checkBox_Diff[2] = checkBox3_Diff.Checked;
            up.checkBox_Diff[3] = checkBox4_Diff.Checked;
            up.checkBox_Diff[4] = checkBox5_Diff.Checked;
            up.checkBox_Diff[5] = checkBox6_Diff.Checked;
            up.checkBox_Diff[6] = checkBox7_Diff.Checked;
            up.checkBox_Diff[7] = checkBox8_Diff.Checked;
            up.checkBox_Diff[8] = checkBox9_Diff.Checked;
            up.checkBox_Diff[9] = checkBox10_Diff.Checked;
            up.checkBox_Diff[10] = checkBox11_Diff.Checked;
            up.checkBox_Diff[11] = checkBox12_Diff.Checked;
            up.checkBox_Diff[12] = checkBox13_Diff.Checked;
            up.checkBox_Diff[13] = checkBox14_Diff.Checked;
            up.checkBox_Diff[14] = checkBox15_Diff.Checked;
            up.checkBox_Diff[15] = checkBox16_Diff.Checked;
            up.checkBox_Diff[16] = checkBox17_Diff.Checked;
            up.checkBox_Diff[17] = checkBox18_Diff.Checked;
            up.checkBox_Diff[18] = checkBox19_Diff.Checked;
            up.checkBox_Diff[19] = checkBox20_Diff.Checked;

            up.textBox_Diff[0] = textBox1_Diff.Text;
            up.textBox_Diff[1] = textBox2_Diff.Text;
            up.textBox_Diff[2] = textBox3_Diff.Text;
            up.textBox_Diff[3] = textBox4_Diff.Text;
            up.textBox_Diff[4] = textBox5_Diff.Text;
            up.textBox_Diff[5] = textBox6_Diff.Text;
            up.textBox_Diff[6] = textBox7_Diff.Text;
            up.textBox_Diff[7] = textBox8_Diff.Text;
            up.textBox_Diff[8] = textBox9_Diff.Text;
            up.textBox_Diff[9] = textBox10_Diff.Text;
            up.textBox_Diff[10] = textBox11_Diff.Text;
            up.textBox_Diff[11] = textBox12_Diff.Text;
            up.textBox_Diff[12] = textBox13_Diff.Text;
            up.textBox_Diff[13] = textBox14_Diff.Text;
            up.textBox_Diff[14] = textBox15_Diff.Text;
            up.textBox_Diff[15] = textBox16_Diff.Text;
            up.textBox_Diff[16] = textBox17_Diff.Text;
            up.textBox_Diff[17] = textBox18_Diff.Text;
            up.textBox_Diff[18] = textBox19_Diff.Text;
            up.textBox_Diff[19] = textBox20_Diff.Text;

            up.textBox_delay_time_Diff = textBox_delay_time_Diff.Text;

            up.textBox_GCS_Diff_Min_Point = textBox_GCS_Diff_Min_Point.Text;
            up.textBox_GCS_Diff_Max_Point = textBox_GCS_Diff_Max_Point.Text;

            up.radioButton_Diff_1st_and_3rd = radioButton_Diff_1st_and_3rd.Checked;
            up.radioButton_Diff_2nd_and_3rd = radioButton_Diff_2nd_and_3rd.Checked;
            up.radioButton_Diff_1st_and_2nd = radioButton_Diff_1st_and_2nd.Checked;
            up.radioButton_Diff_1st_2nd_3rd = radioButton_Diff_1st_2nd_3rd.Checked;

            up.radioButton_SH_Diff_Step_16 = radioButton_SH_Diff_Step_16.Checked;
            up.radioButton_SH_Diff_Step_8 = radioButton_SH_Diff_Step_8.Checked;
            up.radioButton_SH_Diff_Step_4 = radioButton_SH_Diff_Step_4.Checked;
            //------------------------------------

            //-------BCS Diff Measure Related--------
            up.textBox_delay_time_Diff_BCS = textBox_delay_time_Diff_BCS.Text;

            up.BCS_Diff_step_value_1_Range1 = BCS_Diff_step_value_1_Range1.Checked;
            up.BCS_Diff_step_value_4_Range1 = BCS_Diff_step_value_4_Range1.Checked;
            up.BCS_Diff_step_value_8_Range1 = BCS_Diff_step_value_8_Range1.Checked;
            up.BCS_Diff_step_value_16_Range1 = BCS_Diff_step_value_16_Range1.Checked;

            up.BCS_Diff_step_value_1_Range2 = BCS_Diff_step_value_1_Range2.Checked;
            up.BCS_Diff_step_value_4_Range2 = BCS_Diff_step_value_4_Range2.Checked;
            up.BCS_Diff_step_value_8_Range2 = BCS_Diff_step_value_8_Range2.Checked;
            up.BCS_Diff_step_value_16_Range2 = BCS_Diff_step_value_16_Range2.Checked;

            up.BCS_Diff_step_value_1_Range3 = BCS_Diff_step_value_1_Range3.Checked;
            up.BCS_Diff_step_value_4_Range3 = BCS_Diff_step_value_4_Range3.Checked;
            up.BCS_Diff_step_value_8_Range3 = BCS_Diff_step_value_8_Range3.Checked;
            up.BCS_Diff_step_value_16_Range3 = BCS_Diff_step_value_16_Range3.Checked;

            up.CheckBox_BCS_Diff_Gray_Points[0] = checkBox1_BCS_Diff_Gray_P1.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[1] = checkBox1_BCS_Diff_Gray_P2.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[2] = checkBox1_BCS_Diff_Gray_P3.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[3] = checkBox1_BCS_Diff_Gray_P4.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[4] = checkBox1_BCS_Diff_Gray_P5.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[5] = checkBox1_BCS_Diff_Gray_P6.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[6] = checkBox1_BCS_Diff_Gray_P7.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[7] = checkBox1_BCS_Diff_Gray_P8.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[8] = checkBox1_BCS_Diff_Gray_P9.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[9] = checkBox1_BCS_Diff_Gray_P10.Checked;
            up.CheckBox_BCS_Diff_Gray_Points[10] = checkBox1_BCS_Diff_Gray_P11.Checked;

            up.textBox_BCS_Diff_Gray_Points[0] = textBox_BCS_Diff_Gray_P1.Text;
            up.textBox_BCS_Diff_Gray_Points[1] = textBox_BCS_Diff_Gray_P2.Text;
            up.textBox_BCS_Diff_Gray_Points[2] = textBox_BCS_Diff_Gray_P3.Text;
            up.textBox_BCS_Diff_Gray_Points[3] = textBox_BCS_Diff_Gray_P4.Text;
            up.textBox_BCS_Diff_Gray_Points[4] = textBox_BCS_Diff_Gray_P5.Text;
            up.textBox_BCS_Diff_Gray_Points[5] = textBox_BCS_Diff_Gray_P6.Text;
            up.textBox_BCS_Diff_Gray_Points[6] = textBox_BCS_Diff_Gray_P7.Text;
            up.textBox_BCS_Diff_Gray_Points[7] = textBox_BCS_Diff_Gray_P8.Text;
            up.textBox_BCS_Diff_Gray_Points[8] = textBox_BCS_Diff_Gray_P9.Text;
            up.textBox_BCS_Diff_Gray_Points[9] = textBox_BCS_Diff_Gray_P10.Text;
            up.textBox_BCS_Diff_Gray_Points[10] = textBox_BCS_Diff_Gray_P11.Text;

            up.radioButton_BCS_Diff_Dual = radioButton_BCS_Diff_Dual.Checked;
            up.radioButton_BCS_Diff_Triple = radioButton_BCS_Diff_Triple.Checked;

            up.radioButton_BCS_Diff_Single = radioButton_BCS_Diff_Single.Checked;

            up.textBox_BCS_Diff_Sub_1_Min_Point = textBox_BCS_Diff_Sub_1_Min_Point.Text;
            up.textBox_BCS_Diff_Sub_2_Min_Point = textBox_BCS_Diff_Sub_2_Min_Point.Text;
            up.textBox_BCS_Diff_Sub_3_Min_Point = textBox_BCS_Diff_Sub_3_Min_Point.Text;
            up.textBox_BCS_Diff_Sub_1_Max_Point = textBox_BCS_Diff_Sub_1_Max_Point.Text;
            up.textBox_BCS_Diff_Sub_2_Max_Point = textBox_BCS_Diff_Sub_2_Max_Point.Text;
            up.textBox_BCS_Diff_Sub_3_Max_Point = textBox_BCS_Diff_Sub_3_Max_Point.Text;

            up.checkBox_BCS_Diif_Range_1 = checkBox_BCS_Diif_Range_1.Checked;
            up.checkBox_BCS_Diif_Range_2 = checkBox_BCS_Diif_Range_2.Checked;
            up.checkBox_BCS_Diif_Range_3 = checkBox_BCS_Diif_Range_3.Checked;
            //---------------------------------------

            //-------All_At_Once Measure Related---------
            up.checkBox_All_At_Once_E3 = checkBox_All_At_Once_E3.Checked;
            up.checkBox_All_At_Once_E2 = checkBox_All_At_Once_E2.Checked;
            up.checkBox_All_At_Once_Diff_BCS = checkBox_All_At_Once_Diff_BCS.Checked;
            up.checkBox_All_At_Once_AOD_GCS = checkBox_All_At_Once_AOD_GCS.Checked;
            up.checkBox_All_At_Once_Diff_GCS = checkBox_All_At_Once_Diff_GCS.Checked;
            up.checkBox_All_At_Once_Delta_E4 = checkBox_All_At_Once_Delta_E4.Checked;
            //-------------------------------------------



            //-----AOD GCS Measure Related------
            //Textbox to String
            up.textBox_AOD_DBV1 = textBox_AOD_DBV1.Text;
            up.textBox_AOD_DBV2 = textBox_AOD_DBV2.Text;
            up.textBox_AOD_DBV3 = textBox_AOD_DBV3.Text;
            up.textBox_AOD_DBV4 = textBox_AOD_DBV4.Text;
            up.textBox_AOD_DBV5 = textBox_AOD_DBV5.Text;
            up.textBox_AOD_DBV6 = textBox_AOD_DBV6.Text;


            //CheckBox to Bool
            up.checkBox_AOD_DBV1 = checkBox_AOD_DBV1.Checked;
            up.checkBox_AOD_DBV2 = checkBox_AOD_DBV2.Checked;
            up.checkBox_AOD_DBV3 = checkBox_AOD_DBV3.Checked;
            up.checkBox_AOD_DBV4 = checkBox_AOD_DBV4.Checked;
            up.checkBox_AOD_DBV5 = checkBox_AOD_DBV5.Checked;
            up.checkBox_AOD_DBV6 = checkBox_AOD_DBV6.Checked;

            //-------------------------------


            //----Delta E4 (IR-Drop Delta E)-----
            up.checkBox_IR_Drop_DBV1 = checkBox_IR_Drop_DBV1.Checked;
            up.checkBox_IR_Drop_DBV2 = checkBox_IR_Drop_DBV2.Checked;

            up.radioButton_E4_50ea_PTNs = radioButton_E4_50ea_PTNs.Checked;
            up.radioButton_E4_94ea_PTNs = radioButton_E4_94ea_PTNs.Checked;

            up.radioButton_1st_Condition_Measure_E4 = radioButton_1st_Condition_Measure_E4.Checked;
            up.radioButton_2nd_Condition_Measure_E4 = radioButton_2nd_Condition_Measure_E4.Checked;
            up.radioButton_3rd_Condition_Measure_E4 = radioButton_3rd_Condition_Measure_E4.Checked;

            up.textBox_IR_Drop_DBV1 = textBox_IR_Drop_DBV1.Text;
            up.textBox_IR_Drop_DBV2 = textBox_IR_Drop_DBV2.Text;
            up.textBox_delay_time_Delat_E4 = textBox_delay_time_Delat_E4.Text;
            //-----------------------------------

            //--------AverageMeasure----------
            up.radioButton_AverageMeasure_Meas_1times = radioButton_AverageMeasure_Meas_1times.Checked;
            up.radioButton_AverageMeasure_Meas_3times = radioButton_AverageMeasure_Meas_3times.Checked;
            up.radioButton_AverageMeasure_Meas_5times = radioButton_AverageMeasure_Meas_5times.Checked;
            up.textBox_AverageMeasure_Apply_Max_Lv = textBox_AverageMeasure_Apply_Max_Lv.Text;
            up.textBox_AverageMeasure_Delay_Before_Measure = textBox_AverageMeasure_Delay_Before_Measure.Text;
            //--------------------------------





            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\Optic_Measurement";
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
        }

        private void button_Load_Setting_Click(object sender, EventArgs e)
        {
            FileStream myFileStream;
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\Optic_Measurement";
            openFileDialog1.Filter = "Default Extension (*.xml)|*.xml";
            openFileDialog1.DefaultExt = "xml";
            openFileDialog1.AddExtension = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myFileStream = new FileStream(openFileDialog1.FileName, FileMode.Open);
                Measurement_UserPreferences up = (Measurement_UserPreferences)mySerializer.Deserialize(myFileStream);

                //---Common---
                //Textbox to String
                textBox_Mipi_Script_Condition1.Text = up.textBox_Mipi_Script_Condition1;
                textBox_Mipi_Script_Condition2.Text = up.textBox_Mipi_Script_Condition2;
                textBox_Mipi_Script_Condition3.Text = up.textBox_Mipi_Script_Condition3;

                textBox_Delta_E2_End_Point.Text = up.textBox_Delta_E2_End_Point;
                textBox_Delta_E2_Max_Point.Text = up.textBox_Delta_E2_Max_Point;
                textBox_delay_time_E2.Text = up.textBox_delay_time_E2;

                textBox_Show_Compared_Mipi_Data.Text = up.textBox_Show_Compared_Mipi_Data;
                textBox_Show_Compared_Mipi_Data2.Text = up.textBox_Show_Compared_Mipi_Data2;
                textBox_Show_Compared_Mipi_Data3.Text = up.textBox_Show_Compared_Mipi_Data3;

                textBox_Delta_E_End_Point.Text = up.textBox_Delta_E_End_Point;
                textBox_delay_time.Text = up.textBox_delay_time;

                textBox_delay_After_Condition_1.Text = up.textBox_delay_After_Condition_1;
                textBox_delay_After_Condition_2.Text = up.textBox_delay_After_Condition_2;
                textBox_delay_After_Condition_3.Text = up.textBox_delay_After_Condition_3;

                textBox_Aging_Sec.Text = up.textBox_Aging_Sec;

                //CheckBox to Bool
                checkBox_3rd_Condition_Measure_E3.Checked = up.checkBox_3rd_Condition_Measure;
                checkBox_2nd_Condition_Measure_E3.Checked = up.checkBox_2nd_Condition_Measure;
                checkBox_1st_Condition_Measure_E3.Checked = up.checkBox_1st_Condition_Measure;

                checkBox_1st_Condition_Measure_E2.Checked = up.checkBox_1st_Condition_Measure_E2;
                checkBox_2nd_Condition_Measure_E2.Checked = up.checkBox_2nd_Condition_Measure_E2;
                checkBox_3rd_Condition_Measure_E2.Checked = up.checkBox_3rd_Condition_Measure_E2;
                checkBox_White_PTN_Apply_E2.Checked = up.checkBox_White_PTN_Apply_E2;

                //RadioButton to Bool
                radioButton_Max_to_Min_E3.Checked = up.radioButton_Max_to_Min_E3;
                radioButton_Min_to_Max_E3.Checked = up.radioButton_Min_to_Max_E3;

                Delta_E2_step_value_1.Checked = up.step_value_1;
                Delta_E2_step_value_4.Checked = up.step_value_4;
                Delta_E2_step_value_8.Checked = up.step_value_8;
                Delta_E2_step_value_16.Checked = up.step_value_16;
                radioButton_Min_to_Max_E2.Checked = up.radioButton_Min_to_Max_E2;
                radioButton_Max_to_Min_E2.Checked = up.radioButton_Max_to_Min_E2;

                //---Condition---
                //Textbox to String
                textBox1_1.Text = up.textBox_Condition1[0];
                textBox2_1.Text = up.textBox_Condition1[1];
                textBox3_1.Text = up.textBox_Condition1[2];
                textBox4_1.Text = up.textBox_Condition1[3];
                textBox5_1.Text = up.textBox_Condition1[4];
                textBox6_1.Text = up.textBox_Condition1[5];
                textBox7_1.Text = up.textBox_Condition1[6];
                textBox8_1.Text = up.textBox_Condition1[7];
                textBox9_1.Text = up.textBox_Condition1[8];
                textBox10_1.Text = up.textBox_Condition1[9];
                textBox11_1.Text = up.textBox_Condition1[10];
                textBox12_1.Text = up.textBox_Condition1[11];

                //CheckBox to Bool
                checkBox1_1.Checked = up.checkBox_Condition1[0];
                checkBox2_1.Checked = up.checkBox_Condition1[1];
                checkBox3_1.Checked = up.checkBox_Condition1[2];
                checkBox4_1.Checked = up.checkBox_Condition1[3];
                checkBox5_1.Checked = up.checkBox_Condition1[4];
                checkBox6_1.Checked = up.checkBox_Condition1[5];
                checkBox7_1.Checked = up.checkBox_Condition1[6];
                checkBox8_1.Checked = up.checkBox_Condition1[7];
                checkBox9_1.Checked = up.checkBox_Condition1[8];
                checkBox10_1.Checked = up.checkBox_Condition1[9];
                checkBox11_1.Checked = up.checkBox_Condition1[10];
                checkBox12_1.Checked = up.checkBox_Condition1[11];


                //---Condition2---
                //Textbox to String
                textBox1_2.Text = up.textBox_Condition2[0];
                textBox2_2.Text = up.textBox_Condition2[1];
                textBox3_2.Text = up.textBox_Condition2[2];
                textBox4_2.Text = up.textBox_Condition2[3];
                textBox5_2.Text = up.textBox_Condition2[4];
                textBox6_2.Text = up.textBox_Condition2[5];
                textBox7_2.Text = up.textBox_Condition2[6];
                textBox8_2.Text = up.textBox_Condition2[7];
                textBox9_2.Text = up.textBox_Condition2[8];
                textBox10_2.Text = up.textBox_Condition2[9];
                textBox11_2.Text = up.textBox_Condition2[10];
                textBox12_2.Text = up.textBox_Condition2[11];

                //CheckBox to Bool
                checkBox1_2.Checked = up.checkBox_Condition2[0];
                checkBox2_2.Checked = up.checkBox_Condition2[1];
                checkBox3_2.Checked = up.checkBox_Condition2[2];
                checkBox4_2.Checked = up.checkBox_Condition2[3];
                checkBox5_2.Checked = up.checkBox_Condition2[4];
                checkBox6_2.Checked = up.checkBox_Condition2[5];
                checkBox7_2.Checked = up.checkBox_Condition2[6];
                checkBox8_2.Checked = up.checkBox_Condition2[7];
                checkBox9_2.Checked = up.checkBox_Condition2[8];
                checkBox10_2.Checked = up.checkBox_Condition2[9];
                checkBox11_2.Checked = up.checkBox_Condition2[10];
                checkBox12_2.Checked = up.checkBox_Condition2[11];


                //---Condition3---
                //Textbox to String
                textBox1_3.Text = up.textBox_Condition3[0];
                textBox2_3.Text = up.textBox_Condition3[1];
                textBox3_3.Text = up.textBox_Condition3[2];
                textBox4_3.Text = up.textBox_Condition3[3];
                textBox5_3.Text = up.textBox_Condition3[4];
                textBox6_3.Text = up.textBox_Condition3[5];
                textBox7_3.Text = up.textBox_Condition3[6];
                textBox8_3.Text = up.textBox_Condition3[7];
                textBox9_3.Text = up.textBox_Condition3[8];
                textBox10_3.Text = up.textBox_Condition3[9];
                textBox11_3.Text = up.textBox_Condition3[10];
                textBox12_3.Text = up.textBox_Condition3[11];

                //CheckBox to Bool
                checkBox1_3.Checked = up.checkBox_Condition3[0];
                checkBox2_3.Checked = up.checkBox_Condition3[1];
                checkBox3_3.Checked = up.checkBox_Condition3[2];
                checkBox4_3.Checked = up.checkBox_Condition3[3];
                checkBox5_3.Checked = up.checkBox_Condition3[4];
                checkBox6_3.Checked = up.checkBox_Condition3[5];
                checkBox7_3.Checked = up.checkBox_Condition3[6];
                checkBox8_3.Checked = up.checkBox_Condition3[7];
                checkBox9_3.Checked = up.checkBox_Condition3[8];
                checkBox10_3.Checked = up.checkBox_Condition3[9];
                checkBox11_3.Checked = up.checkBox_Condition3[10];
                checkBox12_3.Checked = up.checkBox_Condition3[11];


                //-------GCS Diff Measure Related---------
                checkBox1_Diff.Checked = up.checkBox_Diff[0];
                checkBox2_Diff.Checked = up.checkBox_Diff[1];
                checkBox3_Diff.Checked = up.checkBox_Diff[2];
                checkBox4_Diff.Checked = up.checkBox_Diff[3];
                checkBox5_Diff.Checked = up.checkBox_Diff[4];
                checkBox6_Diff.Checked = up.checkBox_Diff[5];
                checkBox7_Diff.Checked = up.checkBox_Diff[6];
                checkBox8_Diff.Checked = up.checkBox_Diff[7];
                checkBox9_Diff.Checked = up.checkBox_Diff[8];
                checkBox10_Diff.Checked = up.checkBox_Diff[9];
                checkBox11_Diff.Checked = up.checkBox_Diff[10];
                checkBox12_Diff.Checked = up.checkBox_Diff[11];
                checkBox13_Diff.Checked = up.checkBox_Diff[12];
                checkBox14_Diff.Checked = up.checkBox_Diff[13];
                checkBox15_Diff.Checked = up.checkBox_Diff[14];
                checkBox16_Diff.Checked = up.checkBox_Diff[15];
                checkBox17_Diff.Checked = up.checkBox_Diff[16];
                checkBox18_Diff.Checked = up.checkBox_Diff[17];
                checkBox19_Diff.Checked = up.checkBox_Diff[18];
                checkBox20_Diff.Checked = up.checkBox_Diff[19];

                textBox1_Diff.Text = up.textBox_Diff[0];
                textBox2_Diff.Text = up.textBox_Diff[1];
                textBox3_Diff.Text = up.textBox_Diff[2];
                textBox4_Diff.Text = up.textBox_Diff[3];
                textBox5_Diff.Text = up.textBox_Diff[4];
                textBox6_Diff.Text = up.textBox_Diff[5];
                textBox7_Diff.Text = up.textBox_Diff[6];
                textBox8_Diff.Text = up.textBox_Diff[7];
                textBox9_Diff.Text = up.textBox_Diff[8];
                textBox10_Diff.Text = up.textBox_Diff[9];
                textBox11_Diff.Text = up.textBox_Diff[10];
                textBox12_Diff.Text = up.textBox_Diff[11];
                textBox13_Diff.Text = up.textBox_Diff[12];
                textBox14_Diff.Text = up.textBox_Diff[13];
                textBox15_Diff.Text = up.textBox_Diff[14];
                textBox16_Diff.Text = up.textBox_Diff[15];
                textBox17_Diff.Text = up.textBox_Diff[16];
                textBox18_Diff.Text = up.textBox_Diff[17];
                textBox19_Diff.Text = up.textBox_Diff[18];
                textBox20_Diff.Text = up.textBox_Diff[19];

                textBox_delay_time_Diff.Text = up.textBox_delay_time_Diff;

                textBox_GCS_Diff_Min_Point.Text = up.textBox_GCS_Diff_Min_Point;
                textBox_GCS_Diff_Max_Point.Text = up.textBox_GCS_Diff_Max_Point;

                radioButton_Diff_1st_and_3rd.Checked = up.radioButton_Diff_1st_and_3rd;
                radioButton_Diff_2nd_and_3rd.Checked = up.radioButton_Diff_2nd_and_3rd;
                radioButton_Diff_1st_and_2nd.Checked = up.radioButton_Diff_1st_and_2nd;
                radioButton_Diff_1st_2nd_3rd.Checked = up.radioButton_Diff_1st_2nd_3rd;

                radioButton_SH_Diff_Step_16.Checked = up.radioButton_SH_Diff_Step_16;
                radioButton_SH_Diff_Step_8.Checked = up.radioButton_SH_Diff_Step_8;
                radioButton_SH_Diff_Step_4.Checked = up.radioButton_SH_Diff_Step_4;
                //------------------------------------

                //-------BCS Diff Measure Related--------
                textBox_delay_time_Diff_BCS.Text = up.textBox_delay_time_Diff_BCS;

                BCS_Diff_step_value_1_Range1.Checked = up.BCS_Diff_step_value_1_Range1;
                BCS_Diff_step_value_4_Range1.Checked = up.BCS_Diff_step_value_4_Range1;
                BCS_Diff_step_value_8_Range1.Checked = up.BCS_Diff_step_value_8_Range1;
                BCS_Diff_step_value_16_Range1.Checked = up.BCS_Diff_step_value_16_Range1;

                BCS_Diff_step_value_1_Range2.Checked = up.BCS_Diff_step_value_1_Range2;
                BCS_Diff_step_value_4_Range2.Checked = up.BCS_Diff_step_value_4_Range2;
                BCS_Diff_step_value_8_Range2.Checked = up.BCS_Diff_step_value_8_Range2;
                BCS_Diff_step_value_16_Range2.Checked = up.BCS_Diff_step_value_16_Range2;

                BCS_Diff_step_value_1_Range3.Checked = up.BCS_Diff_step_value_1_Range3;
                BCS_Diff_step_value_4_Range3.Checked = up.BCS_Diff_step_value_4_Range3;
                BCS_Diff_step_value_8_Range3.Checked = up.BCS_Diff_step_value_8_Range3;
                BCS_Diff_step_value_16_Range3.Checked = up.BCS_Diff_step_value_16_Range3;

                checkBox1_BCS_Diff_Gray_P1.Checked = up.CheckBox_BCS_Diff_Gray_Points[0];
                checkBox1_BCS_Diff_Gray_P2.Checked = up.CheckBox_BCS_Diff_Gray_Points[1];
                checkBox1_BCS_Diff_Gray_P3.Checked = up.CheckBox_BCS_Diff_Gray_Points[2];
                checkBox1_BCS_Diff_Gray_P4.Checked = up.CheckBox_BCS_Diff_Gray_Points[3];
                checkBox1_BCS_Diff_Gray_P5.Checked = up.CheckBox_BCS_Diff_Gray_Points[4];
                checkBox1_BCS_Diff_Gray_P6.Checked = up.CheckBox_BCS_Diff_Gray_Points[5];
                checkBox1_BCS_Diff_Gray_P7.Checked = up.CheckBox_BCS_Diff_Gray_Points[6];
                checkBox1_BCS_Diff_Gray_P8.Checked = up.CheckBox_BCS_Diff_Gray_Points[7];
                checkBox1_BCS_Diff_Gray_P9.Checked = up.CheckBox_BCS_Diff_Gray_Points[8];
                checkBox1_BCS_Diff_Gray_P10.Checked = up.CheckBox_BCS_Diff_Gray_Points[9];
                checkBox1_BCS_Diff_Gray_P11.Checked = up.CheckBox_BCS_Diff_Gray_Points[10];

                textBox_BCS_Diff_Gray_P1.Text = up.textBox_BCS_Diff_Gray_Points[0];
                textBox_BCS_Diff_Gray_P2.Text = up.textBox_BCS_Diff_Gray_Points[1];
                textBox_BCS_Diff_Gray_P3.Text = up.textBox_BCS_Diff_Gray_Points[2];
                textBox_BCS_Diff_Gray_P4.Text = up.textBox_BCS_Diff_Gray_Points[3];
                textBox_BCS_Diff_Gray_P5.Text = up.textBox_BCS_Diff_Gray_Points[4];
                textBox_BCS_Diff_Gray_P6.Text = up.textBox_BCS_Diff_Gray_Points[5];
                textBox_BCS_Diff_Gray_P7.Text = up.textBox_BCS_Diff_Gray_Points[6];
                textBox_BCS_Diff_Gray_P8.Text = up.textBox_BCS_Diff_Gray_Points[7];
                textBox_BCS_Diff_Gray_P9.Text = up.textBox_BCS_Diff_Gray_Points[8];
                textBox_BCS_Diff_Gray_P10.Text = up.textBox_BCS_Diff_Gray_Points[9];
                textBox_BCS_Diff_Gray_P11.Text = up.textBox_BCS_Diff_Gray_Points[10];

                radioButton_BCS_Diff_Dual.Checked = up.radioButton_BCS_Diff_Dual;
                radioButton_BCS_Diff_Triple.Checked = up.radioButton_BCS_Diff_Triple;

                radioButton_BCS_Diff_Single.Checked = up.radioButton_BCS_Diff_Single;

                textBox_BCS_Diff_Sub_1_Min_Point.Text = up.textBox_BCS_Diff_Sub_1_Min_Point;
                textBox_BCS_Diff_Sub_2_Min_Point.Text = up.textBox_BCS_Diff_Sub_2_Min_Point;
                textBox_BCS_Diff_Sub_3_Min_Point.Text = up.textBox_BCS_Diff_Sub_3_Min_Point;
                textBox_BCS_Diff_Sub_1_Max_Point.Text = up.textBox_BCS_Diff_Sub_1_Max_Point;
                textBox_BCS_Diff_Sub_2_Max_Point.Text = up.textBox_BCS_Diff_Sub_2_Max_Point;
                textBox_BCS_Diff_Sub_3_Max_Point.Text = up.textBox_BCS_Diff_Sub_3_Max_Point;

                checkBox_BCS_Diif_Range_1.Checked = up.checkBox_BCS_Diif_Range_1;
                checkBox_BCS_Diif_Range_2.Checked = up.checkBox_BCS_Diif_Range_2;
                checkBox_BCS_Diif_Range_3.Checked = up.checkBox_BCS_Diif_Range_3;
                //---------------------------------------


                //-------All_At_Once Measure Related---------
                checkBox_All_At_Once_E3.Checked = up.checkBox_All_At_Once_E3;
                checkBox_All_At_Once_E2.Checked = up.checkBox_All_At_Once_E2;
                checkBox_All_At_Once_Diff_BCS.Checked = up.checkBox_All_At_Once_Diff_BCS;
                checkBox_All_At_Once_AOD_GCS.Checked = up.checkBox_All_At_Once_AOD_GCS;
                checkBox_All_At_Once_Diff_GCS.Checked = up.checkBox_All_At_Once_Diff_GCS;
                checkBox_All_At_Once_Delta_E4.Checked = up.checkBox_All_At_Once_Delta_E4;
                //-------------------------------------------


                //-----AOD GCS Measure Related------
                //Textbox to String
                textBox_AOD_DBV1.Text = up.textBox_AOD_DBV1;
                textBox_AOD_DBV2.Text = up.textBox_AOD_DBV2;
                textBox_AOD_DBV3.Text = up.textBox_AOD_DBV3;
                textBox_AOD_DBV4.Text = up.textBox_AOD_DBV4;
                textBox_AOD_DBV5.Text = up.textBox_AOD_DBV5;
                textBox_AOD_DBV6.Text = up.textBox_AOD_DBV6;

                //CheckBox to Bool
                checkBox_AOD_DBV1.Checked = up.checkBox_AOD_DBV1;
                checkBox_AOD_DBV2.Checked = up.checkBox_AOD_DBV2;
                checkBox_AOD_DBV3.Checked = up.checkBox_AOD_DBV3;
                checkBox_AOD_DBV4.Checked = up.checkBox_AOD_DBV4;
                checkBox_AOD_DBV5.Checked = up.checkBox_AOD_DBV5;
                checkBox_AOD_DBV6.Checked = up.checkBox_AOD_DBV6;
                //-------------------------------

                //----Delta E4 (IR-Drop Delta E)-----
                checkBox_IR_Drop_DBV1.Checked = up.checkBox_IR_Drop_DBV1;
                checkBox_IR_Drop_DBV2.Checked = up.checkBox_IR_Drop_DBV2;

                radioButton_E4_50ea_PTNs.Checked = up.radioButton_E4_50ea_PTNs;
                radioButton_E4_94ea_PTNs.Checked = up.radioButton_E4_94ea_PTNs;

                radioButton_1st_Condition_Measure_E4.Checked = up.radioButton_1st_Condition_Measure_E4;
                radioButton_2nd_Condition_Measure_E4.Checked = up.radioButton_2nd_Condition_Measure_E4;
                radioButton_3rd_Condition_Measure_E4.Checked = up.radioButton_3rd_Condition_Measure_E4;

                textBox_IR_Drop_DBV1.Text = up.textBox_IR_Drop_DBV1;
                textBox_IR_Drop_DBV2.Text = up.textBox_IR_Drop_DBV2;
                textBox_delay_time_Delat_E4.Text = up.textBox_delay_time_Delat_E4;
                //-----------------------------------

                //--------AverageMeasure----------
                radioButton_AverageMeasure_Meas_1times.Checked = up.radioButton_AverageMeasure_Meas_1times;
                radioButton_AverageMeasure_Meas_3times.Checked = up.radioButton_AverageMeasure_Meas_3times;
                radioButton_AverageMeasure_Meas_5times.Checked = up.radioButton_AverageMeasure_Meas_5times;
                textBox_AverageMeasure_Apply_Max_Lv.Text = up.textBox_AverageMeasure_Apply_Max_Lv;
                textBox_AverageMeasure_Delay_Before_Measure.Text = up.textBox_AverageMeasure_Delay_Before_Measure;
                //--------------------------------

                myFileStream.Close();
                System.Windows.Forms.MessageBox.Show("Setting has been Loaded (File Date : " + up.Saved_Date + ")");
            }
            else
            {
                myFileStream = null;
                System.Windows.Forms.MessageBox.Show("Nothing has been Loaded");
            }
            //------------------------
            Button_Click_Enable(true);
        }

        private void DBV_CheckBox_Status_Update(object sender, EventArgs e)
        {
            //Condition1
            checkBox1_1_CheckedChanged(sender, e);
            checkBox2_1_CheckedChanged(sender, e);
            checkBox3_1_CheckedChanged(sender, e);
            checkBox4_1_CheckedChanged(sender, e);
            checkBox5_1_CheckedChanged(sender, e);
            checkBox6_1_CheckedChanged(sender, e);
            checkBox7_1_CheckedChanged(sender, e);
            checkBox8_1_CheckedChanged(sender, e);
            checkBox9_1_CheckedChanged(sender, e);
            checkBox10_1_CheckedChanged(sender, e);
            checkBox11_1_CheckedChanged(sender, e);
            checkBox12_1_CheckedChanged(sender, e);


            //Condition2
            checkBox1_2_CheckedChanged(sender, e);
            checkBox2_2_CheckedChanged(sender, e);
            checkBox3_2_CheckedChanged(sender, e);
            checkBox4_2_CheckedChanged(sender, e);
            checkBox5_2_CheckedChanged(sender, e);
            checkBox6_2_CheckedChanged(sender, e);
            checkBox7_2_CheckedChanged(sender, e);
            checkBox8_2_CheckedChanged(sender, e);
            checkBox9_2_CheckedChanged(sender, e);
            checkBox10_2_CheckedChanged(sender, e);
            checkBox11_2_CheckedChanged(sender, e);
            checkBox12_2_CheckedChanged(sender, e);

            //Condition3
            checkBox1_3_CheckedChanged(sender, e);
            checkBox2_3_CheckedChanged(sender, e);
            checkBox3_3_CheckedChanged(sender, e);
            checkBox4_3_CheckedChanged(sender, e);
            checkBox5_3_CheckedChanged(sender, e);
            checkBox6_3_CheckedChanged(sender, e);
            checkBox7_3_CheckedChanged(sender, e);
            checkBox8_3_CheckedChanged(sender, e);
            checkBox9_3_CheckedChanged(sender, e);
            checkBox10_3_CheckedChanged(sender, e);
            checkBox11_3_CheckedChanged(sender, e);
            checkBox12_3_CheckedChanged(sender, e);

            //Diff GCS
            checkBox1_Diff_CheckedChanged(sender, e);
            checkBox2_Diff_CheckedChanged(sender, e);
            checkBox3_Diff_CheckedChanged(sender, e);
            checkBox4_Diff_CheckedChanged(sender, e);
            checkBox5_Diff_CheckedChanged(sender, e);
            checkBox6_Diff_CheckedChanged(sender, e);
            checkBox7_Diff_CheckedChanged(sender, e);
            checkBox8_Diff_CheckedChanged(sender, e);
            checkBox9_Diff_CheckedChanged(sender, e);
            checkBox10_Diff_CheckedChanged(sender, e);
            checkBox11_Diff_CheckedChanged(sender, e);
            checkBox12_Diff_CheckedChanged(sender, e);
            checkBox13_Diff_CheckedChanged(sender, e);
            checkBox14_Diff_CheckedChanged(sender, e);
            checkBox15_Diff_CheckedChanged(sender, e);
            checkBox16_Diff_CheckedChanged(sender, e);
            checkBox17_Diff_CheckedChanged(sender, e);
            checkBox18_Diff_CheckedChanged(sender, e);
            checkBox19_Diff_CheckedChanged(sender, e);
            checkBox20_Diff_CheckedChanged(sender, e);

            //Diff BCS
            checkBox1_BCS_Diff_Gray_P1_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P2_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P3_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P4_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P5_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P6_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P7_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P8_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P9_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P10_CheckedChanged(sender, e);
            checkBox1_BCS_Diff_Gray_P11_CheckedChanged(sender, e);

            //AOD GCS
            checkBox_AOD_DBV1_CheckedChanged(sender, e);
            checkBox_AOD_DBV2_CheckedChanged(sender, e);
            checkBox_AOD_DBV3_CheckedChanged(sender, e);
            checkBox_AOD_DBV4_CheckedChanged(sender, e);
            checkBox_AOD_DBV5_CheckedChanged(sender, e);
            checkBox_AOD_DBV6_CheckedChanged(sender, e);

            //Delta E4
            checkBox_IR_Drop_DBV1_CheckedChanged(sender, e);
            checkBox_IR_Drop_DBV2_CheckedChanged(sender, e);
        }

        private void CheckBox_CheckedChanged(CheckBox checkbox, TextBox textbox)
        {
            if (checkbox.Checked == false) textbox.BackColor = Color.Black;
            else textbox.BackColor = Color.White;
        }

        private void CheckBox_CheckedChanged(CheckBox checkbox, TextBox textbox1, TextBox textbox2)
        {
            if (checkbox.Checked == false)
            {
                textbox1.BackColor = Color.Black;
                textbox2.BackColor = Color.Black;
            }
            else
            {
                textbox1.BackColor = Color.White;
                textbox2.BackColor = Color.White;
            }
        }

        private void checkBox1_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_1, textBox1_1);
        }

        private void checkBox2_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox2_1, textBox2_1);
        }

        private void checkBox3_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox3_1, textBox3_1);
        }

        private void checkBox4_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox4_1, textBox4_1);
        }

        private void checkBox5_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox5_1, textBox5_1);
        }

        private void checkBox6_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox6_1, textBox6_1);
        }

        private void checkBox7_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox7_1, textBox7_1);
        }

        private void checkBox8_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox8_1, textBox8_1);
        }

        private void checkBox9_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox9_1, textBox9_1);
        }

        private void checkBox10_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox10_1, textBox10_1);
        }

        private void checkBox11_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox11_1, textBox11_1);
        }

        private void checkBox12_1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox12_1, textBox12_1);
        }

        private void checkBox1_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_2, textBox1_2);
        }

        private void checkBox2_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox2_2, textBox2_2);
        }

        private void checkBox3_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox3_2, textBox3_2);
        }

        private void checkBox4_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox4_2, textBox4_2);
        }

        private void checkBox5_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox5_2, textBox5_2);
        }

        private void checkBox6_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox6_2, textBox6_2);
        }

        private void checkBox7_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox7_2, textBox7_2);
        }

        private void checkBox8_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox8_2, textBox8_2);
        }

        private void checkBox9_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox9_2, textBox9_2);
        }

        private void checkBox10_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox10_2, textBox10_2);
        }

        private void checkBox11_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox11_2, textBox11_2);
        }

        private void checkBox12_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox12_2, textBox12_2);
        }


        private void checkBox1_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_3, textBox1_3);
        }

        private void checkBox2_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox2_3, textBox2_3);
        }

        private void checkBox3_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox3_3, textBox3_3);
        }

        private void checkBox4_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox4_3, textBox4_3);
        }

        private void checkBox5_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox5_3, textBox5_3);
        }

        private void checkBox6_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox6_3, textBox6_3);
        }

        private void checkBox7_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox7_3, textBox7_3);
        }

        private void checkBox8_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox8_3, textBox8_3);
        }

        private void checkBox9_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox9_3, textBox9_3);
        }

        private void checkBox10_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox10_3, textBox10_3);
        }

        private void checkBox11_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox11_3, textBox11_3);
        }

        private void checkBox12_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox12_3, textBox12_3);
        }






        public void Delta_E2_calculation_btn_Click(object sender, EventArgs e)
        {
            Delta_E2_calculation();
        }

        public void Delta_E2_calculation()
        {
            if (channel_obj.IsCAConnected() == false)
            {
                System.Windows.Forms.MessageBox.Show("Please Connect CA310 or CA410 First");
            }
            else
            {
                groupBox_DeltaE2.Enabled = false;
                Button_Click_Enable(false);

                int delay_time_between_measurement = Convert.ToInt32(textBox_delay_time_E2.Text);
                delta_E2.Set_Availability(able: true);
                delta_E2.MeasureAll(channel_obj);

                groupBox_DeltaE2.Enabled = true;
                Button_Click_Enable(true);
            }
        }


        private void checkBox1_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_Diff, textBox1_Diff);
        }

        private void checkBox2_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox2_Diff, textBox2_Diff);
        }

        private void checkBox3_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox3_Diff, textBox3_Diff);
        }

        private void checkBox4_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox4_Diff, textBox4_Diff);
        }

        private void checkBox5_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox5_Diff, textBox5_Diff);
        }

        private void checkBox6_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox6_Diff, textBox6_Diff);
        }

        private void checkBox7_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox7_Diff, textBox7_Diff);
        }

        private void checkBox8_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox8_Diff, textBox8_Diff);
        }

        private void checkBox9_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox9_Diff, textBox9_Diff);
        }

        private void checkBox10_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox10_Diff, textBox10_Diff);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox_delay_After_Condition_1_TextChanged(object sender, EventArgs e)
        {

        }





        private void button_SH_GCS_Difference_Measure_Click(object sender, EventArgs e)
        {
            SH_GCS_Difference_Measure();
        }

        public void SH_GCS_Difference_Measure()
        {
            if (channel_obj.IsCAConnected() == false)
            {
                System.Windows.Forms.MessageBox.Show("Please Connect CA310 or CA410 First");
            }
            else
            {
                Button_Click_Enable(false);
                groupBox_GCS_Diff.Enabled = false;
                Update_Diff_DBV_Checkbox_And_Textbox();

                gcs_difference.Set_Availability(able: true);
                gcs_difference.MeasureAll(channel_obj);

                Button_Click_Enable(true);
                groupBox_GCS_Diff.Enabled = true;
            }
        }


        private void All_At_Once_CheckBox_Enable(bool Able)
        {
            checkBox_All_At_Once_E3.Enabled = Able;
            checkBox_All_At_Once_E2.Enabled = Able;
            checkBox_All_At_Once_Diff_GCS.Enabled = Able;
            checkBox_All_At_Once_Diff_BCS.Enabled = Able;
            checkBox_All_At_Once_AOD_GCS.Enabled = Able;
            checkBox_All_At_Once_Delta_E4.Enabled = Able;
        }


        private void button_All_At_Once_Click(object sender, EventArgs e)
        {
            All_At_Once_CheckBox_Enable(false);
            button_Clear.PerformClick();

            all_at_once.Set_Availability(able : true);
            all_at_once.MeasureAll(channel_obj);

            All_At_Once_CheckBox_Enable(true);
            MessageBox.Show("All-AtOnce Measure was finished !");
        }

        private void checkBox11_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox11_Diff, textBox11_Diff);
        }


        private void groupBox21_Enter(object sender, EventArgs e)
        {

        }


        private void button_SH_BCS_Difference_Measure_Click(object sender, EventArgs e)
        {
            SH_BCS_Difference_Measure();
        }

        public void SH_BCS_Difference_Measure()
        {
            if (channel_obj.IsCAConnected() == false)
            {
                System.Windows.Forms.MessageBox.Show("Please Connect CA310 or CA410 First");
            }
            else
            {
                Button_Click_Enable(false);
                groupBox_BCS_Diff.Enabled = false;
                Update_Diff_BCS_Gray_Checkbox_And_Textbox();

                bcs_difference.Set_Availability(able: true);
                bcs_difference.MeasureAll(channel_obj);

                Button_Click_Enable(true);
                groupBox_BCS_Diff.Enabled = true;
            }
        }

        private void textBox4_1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_AOD_DBV1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox_AOD_DBV1, textBox_AOD_DBV1);
        }

        private void checkBox_AOD_DBV2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox_AOD_DBV2, textBox_AOD_DBV2);
        }

        private void checkBox_AOD_DBV3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox_AOD_DBV3, textBox_AOD_DBV3);
        }

        private void button_AOD_GCS_Click(object sender, EventArgs e)
        {
            AOD_GCS();
        }

        public void AOD_GCS()
        {
            if (channel_obj.IsCAConnected() == false)
            {
                System.Windows.Forms.MessageBox.Show("Please Connect CA310 or CA410 First");
            }
            else
            {
                Button_Click_Enable(false);
                AOD_GCS_Textbox_CheckBox_Enable(false);

                aod_delta_E3.Set_Availability(able: true);
                aod_delta_E3.MeasureAll(channel_obj);

                Button_Click_Enable(true);
                AOD_GCS_Textbox_CheckBox_Enable(true);
            }
        }



        private void checkBox12_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox12_Diff, textBox12_Diff);
        }

        private void checkBox13_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox13_Diff, textBox13_Diff);
        }

        private void checkBox14_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox14_Diff, textBox14_Diff);
        }

        private void checkBox15_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox15_Diff, textBox15_Diff);
        }

        private void checkBox16_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox16_Diff, textBox16_Diff);
        }

        private void checkBox17_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox17_Diff, textBox17_Diff);
        }

        private void checkBox18_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox18_Diff, textBox18_Diff);
        }

        private void checkBox19_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox19_Diff, textBox19_Diff);
        }

        private void checkBox20_Diff_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox20_Diff, textBox20_Diff);
        }

        private void checkBox1_BCS_Diff_Gray_P1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P1, textBox_BCS_Diff_Gray_P1);
        }

        private void checkBox1_BCS_Diff_Gray_P2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P2, textBox_BCS_Diff_Gray_P2);
        }

        private void checkBox1_BCS_Diff_Gray_P3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P3, textBox_BCS_Diff_Gray_P3);
        }

        private void checkBox1_BCS_Diff_Gray_P4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P4, textBox_BCS_Diff_Gray_P4);
        }

        private void checkBox1_BCS_Diff_Gray_P5_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P5, textBox_BCS_Diff_Gray_P5);
        }

        private void checkBox1_BCS_Diff_Gray_P6_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P6, textBox_BCS_Diff_Gray_P6);
        }

        private void checkBox1_BCS_Diff_Gray_P7_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P7, textBox_BCS_Diff_Gray_P7);
        }

        private void checkBox1_BCS_Diff_Gray_P8_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P8, textBox_BCS_Diff_Gray_P8);
        }

        private void checkBox1_BCS_Diff_Gray_P9_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P9, textBox_BCS_Diff_Gray_P9);
        }

        private void checkBox1_BCS_Diff_Gray_P10_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P10, textBox_BCS_Diff_Gray_P10);
        }

        private void checkBox1_BCS_Diff_Gray_P11_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox1_BCS_Diff_Gray_P11, textBox_BCS_Diff_Gray_P11);
        }

        private void textBox_Aging_Sec_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox_Aging_Sec_Read_TextChanged(object sender, EventArgs e)
        {

        }

        private void Clear_Gamma_Crush_dataGridView()
        {
            dataGridView10.Rows.Clear();
            dataGridView11.Rows.Clear();
            dataGridView12.Rows.Clear();
            Application.DoEvents();
        }
        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }







        private void checkBox_AOD_DBV4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox_AOD_DBV4, textBox_AOD_DBV4);
        }

        private void checkBox_AOD_DBV5_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox_AOD_DBV5, textBox_AOD_DBV5);
        }

        private void checkBox_AOD_DBV6_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox_AOD_DBV6, textBox_AOD_DBV6);
        }


        private void checkBox_IR_Drop_DBV1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox_IR_Drop_DBV1, textBox_IR_Drop_DBV1);
        }

        private void checkBox_IR_Drop_DBV2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_CheckedChanged(checkBox_IR_Drop_DBV2, textBox_IR_Drop_DBV2);
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            SQLiteDB sqliteDB = new SQLiteDB(dataGridView1, dataGridView2, dataGridView3, dataGridView4, dataGridView5, dataGridView6, dataGridView7, dataGridView8, dataGridView9, dataGridView10, dataGridView11, dataGridView12, dataGridView13, dataGridView14, dataGridView15);
            sqliteDB.Create_New_DB("SJH_TEST_CREATE_DB_AND_TABLEs");

        }
    }
}
