using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PNC_Csharp.Measurement_QA
{
    class GCS_Difference : BaseMeasure , I_Measure
    {
        ProgressBar progressBar_GCS_Diff;
        TextBox textBox_GCS_Diff_Max_Point;
        TextBox textBox_GCS_Diff_Min_Point;
        DataGridView dataGridView7;
        DataGridView dataGridView8;
        DataGridView dataGridView9;
        RadioButton radioButton_SH_Diff_Step_4;
        RadioButton radioButton_SH_Diff_Step_8;
        RadioButton radioButton_SH_Diff_Step_16;
        RadioButton radioButton_Diff_2nd_and_3rd;
        RadioButton radioButton_Diff_1st_and_3rd;
        RadioButton radioButton_Diff_1st_and_2nd;
        bool[] checkBox_Diff_GCS_DBV;
        string[] textBox_Diff_GCS_DBV;
        TextBox textBox_delay_time_Diff;

        I_Channel channel_obj;
        AvgMeasMode avgMeasMode;
        public GCS_Difference(
            TextBox _textBox_Show_Compared_Mipi_Data,
            TextBox _textBox_Show_Compared_Mipi_Data2,
            TextBox _textBox_Show_Compared_Mipi_Data3,
            TextBox _textBox_delay_After_Condition_1,
            TextBox _textBox_delay_After_Condition_2,
            TextBox _textBox_delay_After_Condition_3,
            ProgressBar _progressBar_GCS_Diff,
        TextBox _textBox_GCS_Diff_Max_Point,
        TextBox _textBox_GCS_Diff_Min_Point,
        DataGridView _dataGridView7,
        DataGridView _dataGridView8,
        DataGridView _dataGridView9,
        RadioButton _radioButton_SH_Diff_Step_4,
        RadioButton _radioButton_SH_Diff_Step_8,
        RadioButton _radioButton_SH_Diff_Step_16,
        RadioButton _radioButton_Diff_2nd_and_3rd,
        RadioButton _radioButton_Diff_1st_and_3rd,
        RadioButton _radioButton_Diff_1st_and_2nd,
        bool[] _checkBox_Diff_GCS_DBV,
        string[] _textBox_Diff_GCS_DBV,
            TextBox _textBox_delay_time_Diff,
            AvgMeasMode _avgMeasMode)
            : base(_textBox_Show_Compared_Mipi_Data, _textBox_Show_Compared_Mipi_Data2, _textBox_Show_Compared_Mipi_Data3, _textBox_delay_After_Condition_1, _textBox_delay_After_Condition_2, _textBox_delay_After_Condition_3)
        {
            progressBar_GCS_Diff = _progressBar_GCS_Diff;
            textBox_GCS_Diff_Max_Point = _textBox_GCS_Diff_Max_Point;
            textBox_GCS_Diff_Min_Point = _textBox_GCS_Diff_Min_Point;
            dataGridView7 = _dataGridView7;
            dataGridView8 = _dataGridView8;
            dataGridView9 = _dataGridView9;
            radioButton_SH_Diff_Step_4 = _radioButton_SH_Diff_Step_4;
            radioButton_SH_Diff_Step_8 = _radioButton_SH_Diff_Step_8;
            radioButton_SH_Diff_Step_16 = _radioButton_SH_Diff_Step_16;
            radioButton_Diff_2nd_and_3rd = _radioButton_Diff_2nd_and_3rd;
            radioButton_Diff_1st_and_3rd = _radioButton_Diff_1st_and_3rd;
            radioButton_Diff_1st_and_2nd = _radioButton_Diff_1st_and_2nd;
            checkBox_Diff_GCS_DBV = _checkBox_Diff_GCS_DBV;
            textBox_Diff_GCS_DBV = _textBox_Diff_GCS_DBV;
            textBox_delay_time_Diff = _textBox_delay_time_Diff;
            avgMeasMode = _avgMeasMode;
        }



        private int Get_Step()
        {
            int step = 1;
            if (radioButton_SH_Diff_Step_4.Checked) step = 4;
            else if (radioButton_SH_Diff_Step_8.Checked) step = 8;
            else if (radioButton_SH_Diff_Step_16.Checked) step = 16;

            return step;
        }


        private int Get_Max_Gray(int gray)
        {
            gray = Math.Min(255, gray);
            textBox_GCS_Diff_Max_Point.Text = gray.ToString();

            return gray;
        }

        private int Get_Min_Gray(int gray)
        {
            gray = Math.Max(0, gray);
            textBox_GCS_Diff_Min_Point.Text = gray.ToString();

            return gray;
        }

        private void Update_ProgressBar()
        {
            int Progress_Bar_Diff_Max = 0;
            for (int i = 0; i < 20; i++) if (checkBox_Diff_GCS_DBV[i]) Progress_Bar_Diff_Max++;
            progressBar_GCS_Diff.Value = 0;
            progressBar_GCS_Diff.Step = 1;
            progressBar_GCS_Diff.Maximum = 1;
            progressBar_GCS_Diff.Maximum += Progress_Bar_Diff_Max;
            progressBar_GCS_Diff.PerformStep();
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
            if (Availability)
            {
                int delay_time_after_pattern = Convert.ToInt32(textBox_delay_time_Diff.Text);

                Update_ProgressBar();
                int Gray_Max = Get_Max_Gray(Convert.ToInt32(textBox_GCS_Diff_Max_Point.Text));
                int Gray_Min = Get_Min_Gray(Convert.ToInt32(textBox_GCS_Diff_Min_Point.Text));

                dataGridView7.Rows.Clear();
                dataGridView8.Rows.Clear();
                dataGridView9.Rows.Clear();

                int step = Get_Step();

                bool First_skip = false; if (radioButton_Diff_2nd_and_3rd.Checked) First_skip = true;
                bool Second_skip = false; if (radioButton_Diff_1st_and_3rd.Checked) Second_skip = true;
                bool Third_skip = false; if (radioButton_Diff_1st_and_2nd.Checked) Third_skip = true;

                for (int i = 0; i < checkBox_Diff_GCS_DBV.Length; i++)
                {
                    if (checkBox_Diff_GCS_DBV[i] && Availability)
                    {
                        string DBV = textBox_Diff_GCS_DBV[i].PadLeft(3, '0');//dex to hex (as a string form)
                        try
                        {
                            f1().DBV_Setting(DBV);

                            if (First_skip == false) dataGridView7.Rows.Add(i.ToString() + ")DBV", DBV, "-", "-");
                            if (Second_skip == false) dataGridView8.Rows.Add(i.ToString() + ")DBV", DBV, "-", "-");
                            if (Third_skip == false) dataGridView9.Rows.Add(i.ToString() + ")DBV", DBV, "-", "-");

                            f1().GB_Status_AppendText_Nextline(i.ToString() + ")Diff DBV[" + DBV + "] was applied", Color.Blue);
                        }
                        catch
                        {
                            f1().GB_Status_AppendText_Nextline(i.ToString() + ")Diff DBV[" + DBV + "] was failed", Color.Red);
                        }

                        Optic_Dual_SH_Difference_Measure_By_Step(Gray_Max, Gray_Min, delay_time_after_pattern, step, First_skip, Second_skip, Third_skip);
                        progressBar_GCS_Diff.PerformStep();
                    }
                    else
                    {
                        f1().GB_Status_AppendText_Nextline(i.ToString() + ")Diff DBV Point Skip", Color.Black);
                    }
                }
            }
        }

        private void Optic_Dual_SH_Difference_Measure_By_Step(int Gray_Max, int Gray_Min, int delay_time_after_pattern, int step, bool First_skip, bool Second_skip, bool Third_skip)
        {
            bool First_Step = true;
            for (int gray = Gray_Max; gray >= Gray_Min && Availability;)
            {
                if (Availability == false) break;
                f1().PTN_update(gray, gray, gray);

                Thread.Sleep(delay_time_after_pattern);
                try
                {
                    //Condition 1
                    if (First_skip == false)
                    {
                        Script_Apply_For_Condition1();
                        channel_obj.Measure_and_Update_Datagridview(dataGridView7, gray, IsCalculateDeltaE: false, avgMeasMode);
                    }

                    //Condition 2
                    if (Second_skip == false)
                    {
                        Script_Apply_For_Condition2();
                        channel_obj.Measure_and_Update_Datagridview(dataGridView8, gray, IsCalculateDeltaE: false, avgMeasMode);
                    }

                    //Condition 3
                    if (Third_skip == false)
                    {
                        Script_Apply_For_Condition3();
                        channel_obj.Measure_and_Update_Datagridview(dataGridView9, gray, IsCalculateDeltaE: false, avgMeasMode);
                    }
                }
                catch (Exception er)
                {
                    f1().DisplayError(er);
                    System.Windows.Forms.Application.Exit();
                }

                if ((First_Step) && (step != 1))
                {
                    gray -= (step - 1);
                    First_Step = false;
                }
                else
                {
                    gray -= step;
                }
            }
        }




     
    }
}
