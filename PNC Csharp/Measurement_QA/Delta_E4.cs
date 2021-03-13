using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PNC_Csharp.Measurement_QA
{
    class Delta_E4 : BaseMeasure, I_Measure
    {
        I_Channel channel_obj;

        CheckBox checkBox_IR_Drop_DBV1;
        CheckBox checkBox_IR_Drop_DBV2;

        RadioButton radioButton_E4_50ea_PTNs;
        RadioButton radioButton_E4_94ea_PTNs;

        RadioButton radioButton_1st_Condition_Measure_E4;
        RadioButton radioButton_2nd_Condition_Measure_E4;
        RadioButton radioButton_3rd_Condition_Measure_E4;

        TextBox textBox_IR_Drop_DBV1;
        TextBox textBox_IR_Drop_DBV2;

        TextBox textBox_delay_time_Delat_E4;

        ProgressBar progressBar_E4;
        DataGridView dataGridView14;
        AvgMeasMode avgMeasMode;

        public Delta_E4(TextBox _textBox_Show_Compared_Mipi_Data, TextBox _textBox_Show_Compared_Mipi_Data2, TextBox _textBox_Show_Compared_Mipi_Data3,
                        TextBox _textBox_delay_After_Condition_1, TextBox _textBox_delay_After_Condition_2, TextBox _textBox_delay_After_Condition_3,
        CheckBox _checkBox_IR_Drop_DBV1,
        CheckBox _checkBox_IR_Drop_DBV2,
        RadioButton _radioButton_E4_50ea_PTNs,
        RadioButton _radioButton_E4_94ea_PTNs,
        RadioButton _radioButton_1st_Condition_Measure_E4,
        RadioButton _radioButton_2nd_Condition_Measure_E4,
        RadioButton _radioButton_3rd_Condition_Measure_E4,
        TextBox _textBox_IR_Drop_DBV1,
        TextBox _textBox_IR_Drop_DBV2,
        TextBox _textBox_delay_time_Delat_E4,
        ProgressBar _progressBar_E4,
            DataGridView _dataGridView14,
            AvgMeasMode _avgMeasMode)
    : base(_textBox_Show_Compared_Mipi_Data, _textBox_Show_Compared_Mipi_Data2, _textBox_Show_Compared_Mipi_Data3, _textBox_delay_After_Condition_1, _textBox_delay_After_Condition_2, _textBox_delay_After_Condition_3)
        {
            checkBox_IR_Drop_DBV1 = _checkBox_IR_Drop_DBV1;
            checkBox_IR_Drop_DBV2 = _checkBox_IR_Drop_DBV2;

            radioButton_E4_50ea_PTNs = _radioButton_E4_50ea_PTNs;
            radioButton_E4_94ea_PTNs = _radioButton_E4_94ea_PTNs;

            radioButton_1st_Condition_Measure_E4 = _radioButton_1st_Condition_Measure_E4;
            radioButton_2nd_Condition_Measure_E4 = _radioButton_2nd_Condition_Measure_E4;
            radioButton_3rd_Condition_Measure_E4 = _radioButton_3rd_Condition_Measure_E4;
            textBox_IR_Drop_DBV1 = _textBox_IR_Drop_DBV1;
            textBox_IR_Drop_DBV2 = _textBox_IR_Drop_DBV2;

            textBox_delay_time_Delat_E4 = _textBox_delay_time_Delat_E4;

            progressBar_E4 = _progressBar_E4;
            dataGridView14 = _dataGridView14;
            avgMeasMode = _avgMeasMode;
        }


        public void MeasureAll(I_Channel _channel_obj)
        {
            channel_obj = _channel_obj;
            if (channel_obj.IsMultiChannel()) MultiChannelCheckBoxEnable(able: false);
            Measure();
            if (channel_obj.IsMultiChannel()) MultiChannelCheckBoxEnable(able: true);   
        }

        private void Measure()
        {
            if(Availability)
            {
                IRC_Drop_Delta_E_Initialize();
                int dgv_startindex = 0;

                if (checkBox_IR_Drop_DBV1.Checked && Availability)
                {
                    string DBV = textBox_IR_Drop_DBV1.Text.PadLeft(3, '0');//dex to hex (as a string form)
                    f1().DBV_Setting(DBV); Thread.Sleep(20);
                    dataGridView14.Rows.Add("DBV1", DBV, "-", "-");
                    dgv_startindex++;
                    Sub_Conditon_Measure(ref dgv_startindex);
                }

                if (checkBox_IR_Drop_DBV2.Checked && Availability)
                {
                    string DBV = textBox_IR_Drop_DBV2.Text.PadLeft(3, '0');//dex to hex (as a string form)
                    f1().DBV_Setting(DBV); Thread.Sleep(20);
                    dataGridView14.Rows.Add("DBV2", DBV, "-", "-");
                    dgv_startindex++;
                    Sub_Conditon_Measure(ref dgv_startindex);
                }

                IRC_Drop_Delta_E_Finalize();
            }

            
        }


        private void Sub_Conditon_Measure(ref int dgv_startindex)
        {
            if (radioButton_1st_Condition_Measure_E4.Checked && Availability)
            {
                Script_Apply_For_Condition1();
                Calculate_IRC_Drop_Delta_E_PTNs_And_Set_ProgressBar_Max(ref dgv_startindex);
            }
            else if (radioButton_2nd_Condition_Measure_E4.Checked && Availability)
            {
                Script_Apply_For_Condition2();
                Calculate_IRC_Drop_Delta_E_PTNs_And_Set_ProgressBar_Max(ref dgv_startindex);
            }
            else if (radioButton_3rd_Condition_Measure_E4.Checked && Availability)
            {
                Script_Apply_For_Condition3();
                Calculate_IRC_Drop_Delta_E_PTNs_And_Set_ProgressBar_Max(ref dgv_startindex);
            }
        }


        private void Calculate_IRC_Drop_Delta_E_PTNs_And_Set_ProgressBar_Max(ref int dgv_startindex)
        {
            int delay_time_between_measurement = Convert.ToInt16(textBox_delay_time_Delat_E4.Text);
            if (radioButton_E4_50ea_PTNs.Checked && Availability)
                IRC_Drop_Delta_E_For_50ea_PTNs_And_Set_ProgressBar_Max(ref dgv_startindex, delay_time_between_measurement);
            else if (radioButton_E4_94ea_PTNs.Checked && Availability)
                IRC_Drop_Delta_E_For_94ea_PTNs_And_Set_ProgressBar_Max(ref dgv_startindex, delay_time_between_measurement);
        }


        private void IRC_Drop_Delta_E_For_50ea_PTNs_And_Set_ProgressBar_Max(ref int dgv_startindex,int delay_time_between_measurement)
        {
            int pattern_max_index = 49;//0~49 (50ea)
            progressBar_E4.Maximum = pattern_max_index;
            SH_IR_Drop_Delta_E_Measure_For_50ea_PTNs(delay_time_between_measurement);
            
            if (Availability)
            {
                channel_obj.Calculate_Delta_E4_From_x_y_Lv(ref dgv_startindex, pattern_max_index, dataGridView14);
                dgv_startindex += 50;
            }
                
        }

        private void IRC_Drop_Delta_E_For_94ea_PTNs_And_Set_ProgressBar_Max(ref int dgv_startindex,int delay_time_between_measurement)
        {
            int pattern_max_index = 93;//0~93 (94ea)
            progressBar_E4.Maximum = pattern_max_index;
            SH_IR_Drop_Delta_E_Measure_For_94ea_PTNs(delay_time_between_measurement);
           
            if (Availability)
            {
                channel_obj.Calculate_Delta_E4_From_x_y_Lv(ref dgv_startindex, pattern_max_index, dataGridView14);
                dgv_startindex += 94;
            }
                
        }



        public void SH_IR_Drop_Delta_E_Measure_For_50ea_PTNs(int delay_time_between_measurement)
        {
            //Index 0~49 에서 X/Y/Lv 먼저 찍음
            for (int index = 0; index <= 49 & Availability; index++)
            {
                IRC_Drop_Full_and_Square_50ea_PTN_List(index);
                Thread.Sleep(delay_time_between_measurement);
                Sub_SH_IR_Drop_Delta_E_Measure(index);
                progressBar_E4.PerformStep();
            }
        }

        public void SH_IR_Drop_Delta_E_Measure_For_94ea_PTNs(int delay_time_between_measurement)
        {
            //Index 0~93 에서 X/Y/Lv 먼저 찍음
            for (int index = 0; index <= 93 & Availability; index++)
            {
                IRC_Drop_Full_and_Square_94ea_PTN_List(index);
                Thread.Sleep(delay_time_between_measurement);
                progressBar_E4.PerformStep();
                Sub_SH_IR_Drop_Delta_E_Measure(index);
            }
        }


        private void Sub_SH_IR_Drop_Delta_E_Measure(int index)
        {
            try
            {
                channel_obj.Measure_and_Update_Datagridview(dataGridView14, index, IsCalculateDeltaE: true, avgMeasMode);
            }
            catch (Exception er)
            {
                f1().DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }



        private void IRC_Drop_Full_and_Square_94ea_PTN_List(int index)
        {
            switch (index)
            {
                case 0:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 230, 230));
                    break;
                case 1:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 230, 230), true);
                    break;

                case 2:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(209, 209, 209));
                    break;
                case 3:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(209, 209, 209), true);
                    break;

                case 4:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 186, 186));
                    break;
                case 5:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 186, 186), true);
                    break;

                case 6:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 158, 158));
                    break;
                case 7:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 158, 158), true);
                    break;

                case 8:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 0));
                    break;
                case 9:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 0), true);
                    break;

                case 10:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66));
                    break;
                case 11:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66), true);
                    break;

                case 12:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130));
                    break;
                case 13:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130), true);
                    break;

                case 14:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156));
                    break;
                case 15:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156), true);
                    break;

                case 16:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66));
                    break;
                case 17:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66), true);
                    break;

                case 18:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176));
                    break;
                case 19:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176), true);
                    break;

                case 20:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168));
                    break;
                case 21:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168), true);
                    break;

                case 22:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41));
                    break;
                case 23:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41), true);
                    break;

                case 24:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163));
                    break;
                case 25:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163), true);
                    break;

                case 26:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97));
                    break;
                case 27:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97), true);
                    break;

                case 28:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107));
                    break;
                case 29:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107), true);
                    break;

                case 30:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64));
                    break;
                case 31:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64), true);
                    break;

                case 32:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46));
                    break;
                case 33:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46), true);
                    break;

                case 34:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150));
                    break;
                case 35:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150), true);
                    break;

                case 36:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71));
                    break;
                case 37:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71), true);
                    break;

                case 38:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59));
                    break;
                case 39:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59), true);
                    break;

                case 40:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33));
                    break;
                case 41:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33), true);
                    break;

                case 42:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145));
                    break;
                case 43:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145), true);
                    break;

                case 44:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163));
                    break;
                case 45:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163), true);
                    break;

                case 46:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0));
                    break;
                case 47:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0), true);
                    break;

                case 48:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0));
                    break;
                case 49:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0), true);
                    break;

                case 50:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255));
                    break;
                case 51:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255), true);
                    break;

                case 52:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255));
                    break;
                case 53:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255), true);
                    break;

                case 54:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255));
                    break;
                case 55:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255), true);
                    break;

                case 56:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0));
                    break;
                case 57:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0), true);
                    break;

                case 58:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(112, 64, 38));
                    break;
                case 59:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(112, 64, 38), true);
                    break;

                case 60:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(204, 138, 102));
                    break;
                case 61:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(204, 138, 102), true);
                    break;

                case 62:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 199, 153));
                    break;
                case 63:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 199, 153), true);
                    break;

                case 64:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 199, 171));
                    break;
                case 65:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 199, 171), true);
                    break;

                case 66:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(247, 171, 125));
                    break;
                case 67:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(247, 171, 125), true);
                    break;

                case 68:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(199, 140, 92));
                    break;
                case 69:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(199, 140, 92), true);
                    break;

                case 70:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(143, 92, 51));
                    break;
                case 71:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(143, 92, 51), true);
                    break;

                case 72:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(207, 150, 115));
                    break;
                case 73:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(207, 150, 115), true);
                    break;

                case 74:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(161, 87, 33));
                    break;
                case 75:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(161, 87, 33), true);
                    break;

                case 76:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(214, 133, 92));
                    break;
                case 77:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(214, 133, 92), true);
                    break;

                case 78:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(209, 138, 105));
                    break;
                case 79:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(209, 138, 105), true);
                    break;

                case 80:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(250, 153, 115));
                    break;
                case 81:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(250, 153, 115), true);
                    break;

                case 82:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(199, 143, 107));
                    break;
                case 83:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(199, 143, 107), true);
                    break;

                case 84:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(201, 140, 107));
                    break;
                case 85:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(201, 140, 107), true);
                    break;

                case 86:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(204, 143, 105));
                    break;
                case 87:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(204, 143, 105), true);
                    break;

                case 88:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(122, 74, 38));
                    break;
                case 89:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(122, 74, 38), true);
                    break;

                case 90:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 140, 94));
                    break;
                case 91:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 140, 94), true);
                    break;

                case 92:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255));
                    break;
                case 93:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255), true);
                    break;

                default:
                    MessageBox.Show("Index is out of limit(min : 0 , max : 93)");
                    break;
            }
        }

        private void IRC_Drop_Delta_E_Pattern(Color Inner_color, bool Full_PTN = false)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Color Outer_color = Color.White;

            double X = 0;
            double Y = 0;

            f1.Intialize_XY(ref X, ref Y);

            string one_side = Convert.ToInt16(Math.Sqrt(X * Y * 0.3)).ToString();

            if (Full_PTN)
            {
                f1.PTN_update(Inner_color.R, Inner_color.G, Inner_color.B);

                //Just Mornitoring
                f1.GB_Status_AppendText_Nextline("Full PTN : " + Inner_color.R.ToString() + "/" + Inner_color.G.ToString()
                    + "/" + Inner_color.B.ToString(), Color.Black);
            }
            else
            {
                f1.IPC_Quick_Send("image.crosstalk " + one_side + " " + one_side
                    + " " + Outer_color.R.ToString() + " " + Outer_color.G.ToString() + " " + Outer_color.B.ToString()
                    + " " + Inner_color.R.ToString() + " " + Inner_color.G.ToString() + " " + Inner_color.B.ToString());

                //Just Mornitoring
                f1.GB_Status_AppendText_Nextline("Small PTN : " + Inner_color.R.ToString() + "/" + Inner_color.G.ToString()
                    + "/" + Inner_color.B.ToString(), Color.Black);
            }
        }




        private void IRC_Drop_Full_and_Square_50ea_PTN_List(int index)
        {
            switch (index)
            {
                case 0:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0));
                    break;
                case 1:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0), true);
                    break;
                case 2:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0));
                    break;
                case 3:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0), true);
                    break;
                case 4:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255));
                    break;
                case 5:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255), true);
                    break;
                case 6:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0));
                    break;
                case 7:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0), true);
                    break;
                case 8:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255));
                    break;
                case 9:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255), true);
                    break;
                case 10:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255));
                    break;
                case 11:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255), true);
                    break;
                case 12:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66));
                    break;
                case 13:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66), true);
                    break;
                case 14:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130));
                    break;
                case 15:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130), true);
                    break;
                case 16:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156));
                    break;
                case 17:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156), true);
                    break;
                case 18:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66));
                    break;
                case 19:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66), true);
                    break;
                case 20:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176));
                    break;
                case 21:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176), true);
                    break;
                case 22:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168));
                    break;
                case 23:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168), true);
                    break;
                case 24:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41));
                    break;
                case 25:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41), true);
                    break;
                case 26:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163));
                    break;
                case 27:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163), true);
                    break;
                case 28:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97));
                    break;
                case 29:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97), true);
                    break;
                case 30:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107));
                    break;
                case 31:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107), true);
                    break;
                case 32:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64));
                    break;
                case 33:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64), true);
                    break;
                case 34:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46));
                    break;
                case 35:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46), true);
                    break;
                case 36:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150));
                    break;
                case 37:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150), true);
                    break;
                case 38:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71));
                    break;
                case 39:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71), true);
                    break;
                case 40:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59));
                    break;
                case 41:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59), true);
                    break;
                case 42:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33));
                    break;
                case 43:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33), true);
                    break;
                case 44:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145));
                    break;
                case 45:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145), true);
                    break;
                case 46:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163));
                    break;
                case 47:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163), true);
                    break;
                case 48:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255));
                    break;
                case 49:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255), true);
                    break;
                default:
                    MessageBox.Show("Index is out of limit(min : 0 , max : 49)");
                    break;
            }
        }

        private void IRC_Drop_Delta_E_Initialize()
        {
            progressBar_E4.Value = 0;
            progressBar_E4.Step = 1;
            dataGridView14.Rows.Clear();

            checkBox_IR_Drop_DBV1.Enabled = false;
            checkBox_IR_Drop_DBV2.Enabled = false;
            radioButton_E4_50ea_PTNs.Enabled = false;
            radioButton_E4_94ea_PTNs.Enabled = false;
            radioButton_1st_Condition_Measure_E4.Enabled = false;
            radioButton_2nd_Condition_Measure_E4.Enabled = false;
            radioButton_3rd_Condition_Measure_E4.Enabled = false;  
        }

        private void IRC_Drop_Delta_E_Finalize()
        {
            checkBox_IR_Drop_DBV1.Enabled = true;
            checkBox_IR_Drop_DBV2.Enabled = true;
            radioButton_E4_50ea_PTNs.Enabled = true;
            radioButton_E4_94ea_PTNs.Enabled = true;
            radioButton_1st_Condition_Measure_E4.Enabled = true;
            radioButton_2nd_Condition_Measure_E4.Enabled = true;
            radioButton_3rd_Condition_Measure_E4.Enabled = true;
        }
    }
}
