using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PNC_Csharp.Measurement_QA
{
    class BCS_Single_or_Difference : BaseMeasure
    {
        DataGridView dataGridView10;
        DataGridView dataGridView11;
        DataGridView dataGridView12;
        RadioButton BCS_Diff_step_value_1_Range1;
        RadioButton BCS_Diff_step_value_4_Range1;
        RadioButton BCS_Diff_step_value_8_Range1;
        RadioButton BCS_Diff_step_value_16_Range1;
        RadioButton BCS_Diff_step_value_1_Range2;
        RadioButton BCS_Diff_step_value_4_Range2;
        RadioButton BCS_Diff_step_value_8_Range2;
        RadioButton BCS_Diff_step_value_16_Range2;
        RadioButton BCS_Diff_step_value_1_Range3;
        RadioButton BCS_Diff_step_value_4_Range3;
        RadioButton BCS_Diff_step_value_8_Range3;
        RadioButton BCS_Diff_step_value_16_Range3;
        bool[] checkBox_Diff_BCS_Gray;
        string[] textBox_Diff_BCS_DBV;
        ProgressBar progressBar_BCS_Diff;
        CheckBox checkBox_BCS_Diif_Range_1;
        CheckBox checkBox_BCS_Diif_Range_2;
        CheckBox checkBox_BCS_Diif_Range_3;
        RadioButton radioButton_BCS_Diff_Single;
        RadioButton radioButton_BCS_Diff_Dual;
        RadioButton radioButton_BCS_Diff_Triple;
        TextBox textBox_BCS_Diff_Sub_1_Max_Point;
        TextBox textBox_BCS_Diff_Sub_2_Max_Point;
        TextBox textBox_BCS_Diff_Sub_3_Max_Point;
        TextBox textBox_BCS_Diff_Sub_1_Min_Point;
        TextBox textBox_BCS_Diff_Sub_2_Min_Point;
        TextBox textBox_BCS_Diff_Sub_3_Min_Point;
        TextBox textBox_delay_time_Diff_BCS;

        I_Channel channel_obj;
        AvgMeasMode avgMeasMode;

        public BCS_Single_or_Difference(
            TextBox _textBox_Show_Compared_Mipi_Data,
            TextBox _textBox_Show_Compared_Mipi_Data2,
            TextBox _textBox_Show_Compared_Mipi_Data3,
            TextBox _textBox_delay_After_Condition_1,
            TextBox _textBox_delay_After_Condition_2,
            TextBox _textBox_delay_After_Condition_3,
            DataGridView _dataGridView10,
        DataGridView _dataGridView11,
        DataGridView _dataGridView12,
        RadioButton _BCS_Diff_step_value_1_Range1,
        RadioButton _BCS_Diff_step_value_4_Range1,
        RadioButton _BCS_Diff_step_value_8_Range1,
        RadioButton _BCS_Diff_step_value_16_Range1,
        RadioButton _BCS_Diff_step_value_1_Range2,
        RadioButton _BCS_Diff_step_value_4_Range2,
        RadioButton _BCS_Diff_step_value_8_Range2,
        RadioButton _BCS_Diff_step_value_16_Range2,
        RadioButton _BCS_Diff_step_value_1_Range3,
        RadioButton _BCS_Diff_step_value_4_Range3,
        RadioButton _BCS_Diff_step_value_8_Range3,
        RadioButton _BCS_Diff_step_value_16_Range3,
        bool[] _checkBox_Diff_BCS_Gray,
        string[] _textBox_Diff_BCS_DBV,
        ProgressBar _progressBar_BCS_Diff,
        CheckBox _checkBox_BCS_Diif_Range_1,
        CheckBox _checkBox_BCS_Diif_Range_2,
        CheckBox _checkBox_BCS_Diif_Range_3,
        RadioButton _radioButton_BCS_Diff_Single,
        RadioButton _radioButton_BCS_Diff_Dual,
        RadioButton _radioButton_BCS_Diff_Triple,
        TextBox _textBox_BCS_Diff_Sub_1_Max_Point,
        TextBox _textBox_BCS_Diff_Sub_2_Max_Point,
        TextBox _textBox_BCS_Diff_Sub_3_Max_Point,
        TextBox _textBox_BCS_Diff_Sub_1_Min_Point,
        TextBox _textBox_BCS_Diff_Sub_2_Min_Point,
        TextBox _textBox_BCS_Diff_Sub_3_Min_Point,
        TextBox _textBox_delay_time_Diff_BCS,
        AvgMeasMode _avgMeasMode
            ) : base(_textBox_Show_Compared_Mipi_Data, _textBox_Show_Compared_Mipi_Data2, _textBox_Show_Compared_Mipi_Data3, _textBox_delay_After_Condition_1, _textBox_delay_After_Condition_2, _textBox_delay_After_Condition_3)
        {
            textBox_Show_Compared_Mipi_Data = _textBox_Show_Compared_Mipi_Data;
             textBox_Show_Compared_Mipi_Data2 = _textBox_Show_Compared_Mipi_Data2;
            textBox_Show_Compared_Mipi_Data3 = _textBox_Show_Compared_Mipi_Data3;
            textBox_delay_After_Condition_1 = _textBox_delay_After_Condition_1;
            textBox_delay_After_Condition_2 = _textBox_delay_After_Condition_2;
            textBox_delay_After_Condition_3 = _textBox_delay_After_Condition_3;
            dataGridView10 = _dataGridView10;
            dataGridView11 = _dataGridView11;
            dataGridView12 = _dataGridView12;
            BCS_Diff_step_value_1_Range1 = _BCS_Diff_step_value_1_Range1;
            BCS_Diff_step_value_4_Range1 = _BCS_Diff_step_value_4_Range1;
            BCS_Diff_step_value_8_Range1 = _BCS_Diff_step_value_8_Range1;
            BCS_Diff_step_value_16_Range1 = _BCS_Diff_step_value_16_Range1;
            BCS_Diff_step_value_1_Range2 = _BCS_Diff_step_value_1_Range2;
            BCS_Diff_step_value_4_Range2 = _BCS_Diff_step_value_4_Range2;
            BCS_Diff_step_value_8_Range2 = _BCS_Diff_step_value_8_Range2;
            BCS_Diff_step_value_16_Range2 = _BCS_Diff_step_value_16_Range2;
            BCS_Diff_step_value_1_Range3 = _BCS_Diff_step_value_1_Range3;
            BCS_Diff_step_value_4_Range3 = _BCS_Diff_step_value_4_Range3;
            BCS_Diff_step_value_8_Range3 = _BCS_Diff_step_value_8_Range3;
            BCS_Diff_step_value_16_Range3 = _BCS_Diff_step_value_16_Range3;
            checkBox_Diff_BCS_Gray = _checkBox_Diff_BCS_Gray;
            textBox_Diff_BCS_DBV = _textBox_Diff_BCS_DBV;
            progressBar_BCS_Diff = _progressBar_BCS_Diff;
            checkBox_BCS_Diif_Range_1 = _checkBox_BCS_Diif_Range_1;
            checkBox_BCS_Diif_Range_2 = _checkBox_BCS_Diif_Range_2;
            checkBox_BCS_Diif_Range_3 = _checkBox_BCS_Diif_Range_3;
            radioButton_BCS_Diff_Single = _radioButton_BCS_Diff_Single;
            radioButton_BCS_Diff_Dual = _radioButton_BCS_Diff_Dual;
            radioButton_BCS_Diff_Triple = _radioButton_BCS_Diff_Triple;
            textBox_BCS_Diff_Sub_1_Max_Point = _textBox_BCS_Diff_Sub_1_Max_Point;
            textBox_BCS_Diff_Sub_2_Max_Point = _textBox_BCS_Diff_Sub_2_Max_Point;
            textBox_BCS_Diff_Sub_3_Max_Point = _textBox_BCS_Diff_Sub_3_Max_Point;
            textBox_BCS_Diff_Sub_1_Min_Point = _textBox_BCS_Diff_Sub_1_Min_Point;
            textBox_BCS_Diff_Sub_2_Min_Point = _textBox_BCS_Diff_Sub_2_Min_Point;
            textBox_BCS_Diff_Sub_3_Min_Point = _textBox_BCS_Diff_Sub_3_Min_Point;
            textBox_delay_time_Diff_BCS = _textBox_delay_time_Diff_BCS;
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
                int dbv_delay_time = Convert.ToInt32(textBox_delay_time_Diff_BCS.Text);

                dataGridView10.Rows.Clear();
                dataGridView11.Rows.Clear();
                dataGridView12.Rows.Clear();
                dataGridView10.Columns[0].HeaderText = "DBV";
                dataGridView11.Columns[0].HeaderText = "DBV";
                dataGridView12.Columns[0].HeaderText = "DBV";

                int Step_Value_Range1 = 0;
                if (BCS_Diff_step_value_1_Range1.Checked) Step_Value_Range1 = 1;
                else if (BCS_Diff_step_value_4_Range1.Checked) Step_Value_Range1 = 4;
                else if (BCS_Diff_step_value_8_Range1.Checked) Step_Value_Range1 = 8;
                else if (BCS_Diff_step_value_16_Range1.Checked) Step_Value_Range1 = 16;

                int Step_Value_Range2 = 0;
                if (BCS_Diff_step_value_1_Range2.Checked) Step_Value_Range2 = 1;
                else if (BCS_Diff_step_value_4_Range2.Checked) Step_Value_Range2 = 4;
                else if (BCS_Diff_step_value_8_Range2.Checked) Step_Value_Range2 = 8;
                else if (BCS_Diff_step_value_16_Range2.Checked) Step_Value_Range2 = 16;

                int Step_Value_Range3 = 0;
                if (BCS_Diff_step_value_1_Range3.Checked) Step_Value_Range3 = 1;
                else if (BCS_Diff_step_value_4_Range3.Checked) Step_Value_Range3 = 4;
                else if (BCS_Diff_step_value_8_Range3.Checked) Step_Value_Range3 = 8;
                else if (BCS_Diff_step_value_16_Range3.Checked) Step_Value_Range3 = 16;


                //Set ProgressBar_E3 Max and Step and Value
                int Progress_Bar_Diff_Max = 0;
                for (int i = 0; i < 11; i++) if (checkBox_Diff_BCS_Gray[i]) Progress_Bar_Diff_Max++;
                progressBar_BCS_Diff.Value = 0;
                progressBar_BCS_Diff.Step = 1;
                progressBar_BCS_Diff.Maximum = 1;
                progressBar_BCS_Diff.Maximum += Progress_Bar_Diff_Max;
                progressBar_BCS_Diff.PerformStep();

                for (int i = 0; i < 11; i++)
                {
                    if (checkBox_Diff_BCS_Gray[i] && Availability)
                    {
                        int gray = Convert.ToInt32(textBox_Diff_BCS_DBV[i]);
                        f1().PTN_update(gray, gray, gray);
                        Thread.Sleep(300);
                        BCS_Diff_Measure(gray, dbv_delay_time, Step_Value_Range1, Step_Value_Range2, Step_Value_Range3);
                        progressBar_BCS_Diff.PerformStep();
                        f1().GB_Status_AppendText_Nextline(i.ToString() + ")Gray" + gray.ToString() + " was applied", Color.Blue);
                    }
                    else
                    {
                        f1().GB_Status_AppendText_Nextline(i.ToString() + ")Gray point was skipped was Skipped", Color.Black);
                    }
                }
            }
        }


        private void BCS_Diff_Measure(int gray, int dbv_delay_time, int Step_Value_Range1, int Step_Value_Range2, int Step_Value_Range3)
        {
            int Max_DBV = f1().Get_DBV_TrackBar_Maximum();

            if (checkBox_BCS_Diif_Range_1.Checked && Availability)
            {
                //Set DBV_End_Point (Range1)
                int dbv_max_point_1 = Get_DBV_Max_Point_1(Max_DBV);
                int dbv_end_Point_1 = Get_DBV_Min_Point_1(dbv_max_point_1);
                Sub_BCS_Diff_Measure(gray, dbv_delay_time, Step_Value_Range1, dbv_end_Point_1, dbv_max_point_1);
            }

            if (checkBox_BCS_Diif_Range_2.Checked && Availability)
            {
                //Set DBV_End_Point (Range2)
                int dbv_max_point_2 = Get_DBV_Max_Point_2(Max_DBV);
                int dbv_end_Point_2 = Get_DBV_Min_Point_2(dbv_max_point_2);
                Sub_BCS_Diff_Measure(gray, dbv_delay_time, Step_Value_Range2, dbv_end_Point_2, dbv_max_point_2);
            }

            if (checkBox_BCS_Diif_Range_3.Checked && Availability)
            {
                //Set DBV_End_Point (Range3)
                int dbv_max_point_3 = Get_DBV_Max_Point_3(Max_DBV);
                int dbv_end_Point_3 = Get_DBV_Min_Point_3(dbv_max_point_3);
                Sub_BCS_Diff_Measure(gray, dbv_delay_time, Step_Value_Range3, dbv_end_Point_3, dbv_max_point_3);
            }
        }

        private void Sub_BCS_Diff_Measure(int gray, int dbv_delay_time, int Step_Value, int dbv_end_Point, int dbv_max_point)
        {
            if (radioButton_BCS_Diff_Single.Checked && Availability) Optic_SH_Diff_BCS_Measure(gray, dbv_end_Point, dbv_max_point, dbv_delay_time, Step_Value, OC_Single_Dual_Triple.Single);
            else if (radioButton_BCS_Diff_Dual.Checked && Availability) Optic_SH_Diff_BCS_Measure(gray, dbv_end_Point, dbv_max_point, dbv_delay_time, Step_Value, OC_Single_Dual_Triple.Dual);
            else if (radioButton_BCS_Diff_Triple.Checked && Availability) Optic_SH_Diff_BCS_Measure(gray, dbv_end_Point, dbv_max_point, dbv_delay_time, Step_Value, OC_Single_Dual_Triple.Triple);

        }

        private void Optic_SH_Diff_BCS_Measure(int gray, int dbv_end_Point, int dbv_max_point, int dbv_delay_time, int Step_Value, OC_Single_Dual_Triple oc_mode)
        {
            if(Availability)
            {
                DataGridView datagridview1 = dataGridView10;
                DataGridView datagridview2 = dataGridView11;
                DataGridView datagridview3 = dataGridView12;
                int dbv = dbv_max_point;

                if (oc_mode == OC_Single_Dual_Triple.Triple)
                {
                    datagridview1.Rows.Add("G" + gray.ToString(), "-", "-", "-");
                    datagridview2.Rows.Add("G" + gray.ToString(), "-", "-", "-");
                    datagridview3.Rows.Add("G" + gray.ToString(), "-", "-", "-");
                }
                else if (oc_mode == OC_Single_Dual_Triple.Dual)
                {
                    datagridview1.Rows.Add("G" + gray.ToString(), "-", "-", "-");
                    datagridview2.Rows.Add("G" + gray.ToString(), "-", "-", "-");
                }
                else if (oc_mode == OC_Single_Dual_Triple.Single)
                {
                    datagridview1.Rows.Add("G" + gray.ToString(), "-", "-", "-");
                }


                for (int i = (dbv_max_point); i > dbv_end_Point - Step_Value & Availability;)
                {
                    i = i - Step_Value;
                    dbv = i + Step_Value;
                    if (dbv < dbv_end_Point)
                        break;

                    f1().Set_BCS(dbv);
                    Thread.Sleep(dbv_delay_time);

                    //Condition1   
                    Script_Apply_For_Condition1();
                    try
                    {
                        channel_obj.Measure_and_Update_Datagridview(datagridview1, dbv, IsCalculateDeltaE: false, avgMeasMode);
                    }
                    catch (Exception er)
                    {
                        f1().DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }

                    if (oc_mode == OC_Single_Dual_Triple.Triple || oc_mode == OC_Single_Dual_Triple.Dual)
                    {
                        //Condition2
                        Script_Apply_For_Condition2();
                        try
                        {
                            channel_obj.Measure_and_Update_Datagridview(datagridview2, dbv, IsCalculateDeltaE: false, avgMeasMode);
                        }
                        catch (Exception er)
                        {
                            f1().DisplayError(er);
                            System.Windows.Forms.Application.Exit();
                        }
                    }

                    if (oc_mode == OC_Single_Dual_Triple.Triple)
                    {
                        //Condition3
                        Script_Apply_For_Condition3();
                        try
                        {
                            channel_obj.Measure_and_Update_Datagridview(datagridview3, dbv, IsCalculateDeltaE: false, avgMeasMode);
                        }
                        catch (Exception er)
                        {
                            f1().DisplayError(er);
                            System.Windows.Forms.Application.Exit();
                        }
                    }
                }
            }
        }


        private int Get_DBV_Max_Point_1(int Max_DBV)
        {
            int dbv_max_point_1 = Convert.ToInt32(textBox_BCS_Diff_Sub_1_Max_Point.Text);
            dbv_max_point_1 = DBV_Max_Boundary_Processing(dbv_max_point_1, Max_DBV);
            textBox_BCS_Diff_Sub_1_Max_Point.Text = dbv_max_point_1.ToString();

            return dbv_max_point_1;
        }

        private int Get_DBV_Max_Point_2(int Max_DBV)
        {
            int dbv_max_point_2 = Convert.ToInt32(textBox_BCS_Diff_Sub_2_Max_Point.Text);
            dbv_max_point_2 = DBV_Max_Boundary_Processing(dbv_max_point_2, Max_DBV);
            textBox_BCS_Diff_Sub_2_Max_Point.Text = dbv_max_point_2.ToString();

            return dbv_max_point_2;
        }

        private int Get_DBV_Max_Point_3(int Max_DBV)
        {
            int dbv_max_point_3 = Convert.ToInt32(textBox_BCS_Diff_Sub_3_Max_Point.Text);
            dbv_max_point_3 = DBV_Max_Boundary_Processing(dbv_max_point_3, Max_DBV);
            textBox_BCS_Diff_Sub_3_Max_Point.Text = dbv_max_point_3.ToString();

            return dbv_max_point_3;
        }

        private int Get_DBV_Min_Point_1(int dbv_max_point_1)
        {
            int dbv_end_Point_1 = Convert.ToInt32(textBox_BCS_Diff_Sub_1_Min_Point.Text);
            dbv_end_Point_1 = DBV_End_Boundary_Processing(dbv_end_Point_1, dbv_max_point_1);
            textBox_BCS_Diff_Sub_1_Min_Point.Text = dbv_end_Point_1.ToString();

            return dbv_end_Point_1;
        }

        private int Get_DBV_Min_Point_2(int dbv_max_point_2)
        {
            int dbv_end_Point_2 = Convert.ToInt32(textBox_BCS_Diff_Sub_2_Min_Point.Text);
            dbv_end_Point_2 = DBV_End_Boundary_Processing(dbv_end_Point_2, dbv_max_point_2);
            textBox_BCS_Diff_Sub_2_Min_Point.Text = dbv_end_Point_2.ToString();

            return dbv_end_Point_2;
        }

        private int Get_DBV_Min_Point_3(int dbv_max_point_3)
        {
            int dbv_end_Point_3 = Convert.ToInt32(textBox_BCS_Diff_Sub_3_Min_Point.Text);
            dbv_end_Point_3 = DBV_End_Boundary_Processing(dbv_end_Point_3, dbv_max_point_3);
            textBox_BCS_Diff_Sub_3_Min_Point.Text = dbv_end_Point_3.ToString();

            return dbv_end_Point_3;
        }

        private int DBV_Max_Boundary_Processing(int dbv_max_point, int Max_DBV)
        {
            if (dbv_max_point >= Max_DBV) dbv_max_point = Max_DBV;
            else if (dbv_max_point <= 0) dbv_max_point = 0;
            return dbv_max_point;
        }

        private int DBV_End_Boundary_Processing(int dbv_end_Point, int dbv_max_point)
        {
            if (dbv_end_Point >= (dbv_max_point - 1)) dbv_end_Point = (dbv_max_point - 1);
            else if (dbv_end_Point <= 0) dbv_end_Point = 0;
            return dbv_end_Point;
        }

    }
}
