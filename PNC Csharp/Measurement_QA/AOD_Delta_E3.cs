using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PNC_Csharp.Measurement_QA
{
    class AOD_Delta_E3 : BaseMeasure, I_Measure
    {
        ProgressBar progressBar_E3;
        RadioButton radioButton_Min_to_Max_E3;
        DataGridView dataGridView13;
        CheckBox checkBox_AOD_DBV1;
        CheckBox checkBox_AOD_DBV2;
        CheckBox checkBox_AOD_DBV3;
        CheckBox checkBox_AOD_DBV4;
        CheckBox checkBox_AOD_DBV5;
        CheckBox checkBox_AOD_DBV6;
        TextBox textBox_AOD_DBV1;
        TextBox textBox_AOD_DBV2;
        TextBox textBox_AOD_DBV3;
        TextBox textBox_AOD_DBV4;
        TextBox textBox_AOD_DBV5;
        TextBox textBox_AOD_DBV6;

        TextBox textBox_delay_time;
        TextBox textBox_Delta_E_End_Point;

        I_Channel channel_obj;
        AvgMeasMode avgMeasMode;

        public AOD_Delta_E3(ProgressBar _progressBar_E3,
        RadioButton _radioButton_Min_to_Max_E3,
        DataGridView _dataGridView13,
        CheckBox _checkBox_AOD_DBV1,
        CheckBox _checkBox_AOD_DBV2,
        CheckBox _checkBox_AOD_DBV3,
        CheckBox _checkBox_AOD_DBV4,
        CheckBox _checkBox_AOD_DBV5,
        CheckBox _checkBox_AOD_DBV6,
        TextBox _textBox_AOD_DBV1,
        TextBox _textBox_AOD_DBV2,
        TextBox _textBox_AOD_DBV3,
        TextBox _textBox_AOD_DBV4,
        TextBox _textBox_AOD_DBV5,
        TextBox _textBox_AOD_DBV6,
        TextBox _textBox_delay_time,
        TextBox _textBox_Delta_E_End_Point,
        AvgMeasMode _avgMeasMode) : base()
        {
            progressBar_E3 = _progressBar_E3;
            radioButton_Min_to_Max_E3 = _radioButton_Min_to_Max_E3;
            dataGridView13 = _dataGridView13;
            checkBox_AOD_DBV1 = _checkBox_AOD_DBV1;
            checkBox_AOD_DBV2 = _checkBox_AOD_DBV2;
            checkBox_AOD_DBV3 = _checkBox_AOD_DBV3;
            checkBox_AOD_DBV4 = _checkBox_AOD_DBV4;
            checkBox_AOD_DBV5 = _checkBox_AOD_DBV5;
            checkBox_AOD_DBV6 = _checkBox_AOD_DBV6;
            textBox_AOD_DBV1 = _textBox_AOD_DBV1;
            textBox_AOD_DBV2 = _textBox_AOD_DBV2;
            textBox_AOD_DBV3 = _textBox_AOD_DBV3;
            textBox_AOD_DBV4 = _textBox_AOD_DBV4;
            textBox_AOD_DBV5 = _textBox_AOD_DBV5;
            textBox_AOD_DBV6 = _textBox_AOD_DBV6;
            textBox_delay_time = _textBox_delay_time;
            textBox_Delta_E_End_Point = _textBox_Delta_E_End_Point;
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
                int gray_end_Point = Get_AOD_or_DeltaE3_gray_end_Point();
                int delay_time_between_measurement = Convert.ToInt16(textBox_delay_time.Text);
                Initalize_For_AOD_GCS_Measure();
                AOD_GCS_Measure_and_Calculate_DeltaE3(gray_end_Point, delay_time_between_measurement);
            }
        }
            

        private int Get_AOD_or_DeltaE3_gray_end_Point()
        {
            int gray_end_Point = Convert.ToInt16(textBox_Delta_E_End_Point.Text);
            if (gray_end_Point >= 254) gray_end_Point = 254;
            else if (gray_end_Point <= 0) gray_end_Point = 0;
            else { }
            textBox_Delta_E_End_Point.Text = gray_end_Point.ToString();

            return gray_end_Point;
        }

        private void Initalize_For_AOD_GCS_Measure()
        {
            progressBar_E3.Value = 0;
            progressBar_E3.Step = 1;
            progressBar_E3.Maximum = 1;
            if (checkBox_AOD_DBV1.Checked) progressBar_E3.Maximum++;
            if (checkBox_AOD_DBV2.Checked) progressBar_E3.Maximum++;
            if (checkBox_AOD_DBV3.Checked) progressBar_E3.Maximum++;
            if (checkBox_AOD_DBV4.Checked) progressBar_E3.Maximum++;
            if (checkBox_AOD_DBV5.Checked) progressBar_E3.Maximum++;
            if (checkBox_AOD_DBV6.Checked) progressBar_E3.Maximum++;

            progressBar_E3.PerformStep();
            dataGridView13.Rows.Clear();
        }

        private void AOD_GCS_Measure_and_Calculate_DeltaE3(int gray_end_Point, int delay_time_between_measurement)
        {
            f1().AOD_On(); Thread.Sleep(50);
            f1().AOD_On(); Thread.Sleep(50);

            if (checkBox_AOD_DBV1.Checked && Availability) AOD_GCS_Measure_and_Calculate_DeltaE3(textBox_AOD_DBV1.Text.PadLeft(3, '0'), gray_end_Point, delay_time_between_measurement);
            if (checkBox_AOD_DBV2.Checked && Availability) AOD_GCS_Measure_and_Calculate_DeltaE3(textBox_AOD_DBV2.Text.PadLeft(3, '0'), gray_end_Point, delay_time_between_measurement);
            if (checkBox_AOD_DBV3.Checked && Availability) AOD_GCS_Measure_and_Calculate_DeltaE3(textBox_AOD_DBV3.Text.PadLeft(3, '0'), gray_end_Point, delay_time_between_measurement);
            if (checkBox_AOD_DBV4.Checked && Availability) AOD_GCS_Measure_and_Calculate_DeltaE3(textBox_AOD_DBV4.Text.PadLeft(3, '0'), gray_end_Point, delay_time_between_measurement);
            if (checkBox_AOD_DBV5.Checked && Availability) AOD_GCS_Measure_and_Calculate_DeltaE3(textBox_AOD_DBV5.Text.PadLeft(3, '0'), gray_end_Point, delay_time_between_measurement);
            if (checkBox_AOD_DBV6.Checked && Availability) AOD_GCS_Measure_and_Calculate_DeltaE3(textBox_AOD_DBV6.Text.PadLeft(3, '0'), gray_end_Point, delay_time_between_measurement);

            f1().AOD_Off(); Thread.Sleep(50);
            f1().AOD_Off(); Thread.Sleep(50);
        }

        private void AOD_GCS_Measure_and_Calculate_DeltaE3(string DBV, int gray_end_Point, int delay_time_between_measurement)
        {
            if(Availability)
            {
                f1().DBV_Setting(DBV);
                dataGridView13.Rows.Add("DBV", DBV, "-", "-");
                AOD_GCS_Measure(gray_end_Point, delay_time_between_measurement);
                Calculate_Delta_E_From_x_y_Lv(gray_end_Point, (dataGridView13.Rows.Count - 1), 4);
                progressBar_E3.PerformStep();
            }
        }


        private void AOD_GCS_Measure(int gray_end_Point, int delay_time_between_measurement)
        {
            //Gray 48~255 에서 X/Y/Lv 먼저 찍음
            if (radioButton_Min_to_Max_E3.Checked)
            {
                for (int gray = gray_end_Point; gray <= 255 & Availability; gray++)
                {
                    AOD_Pattern_Setting(gray);
                    Thread.Sleep(delay_time_between_measurement);

                    try
                    {
                        channel_obj.Measure_and_Update_Datagridview(dataGridView13, gray,IsCalculateDeltaE : true,avgMeasMode);
                    }
                    catch (Exception er)
                    {
                        f1().DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
            else
            {
                for (int gray = 255; gray >= gray_end_Point & Availability; gray--)
                {
                    AOD_Pattern_Setting(gray);
                    Thread.Sleep(delay_time_between_measurement);

                    try
                    {
                        channel_obj.Measure_and_Update_Datagridview(dataGridView13, gray, IsCalculateDeltaE: true, avgMeasMode);
                    }
                    catch (Exception er)
                    {
                        f1().DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
        }


        private void Calculate_Delta_E_From_x_y_Lv(int gray_end_Point, int Addtional_DeltaE_Rows, int Condition = 1)
        {
            if (Availability)
                channel_obj.Calculate_Delta_E3_From_x_y_Lv(radioButton_Min_to_Max_E3.Checked, gray_end_Point, Addtional_DeltaE_Rows, dataGridView13);
        }

        private void AOD_Pattern_Setting(int Gray)
        {
            f1().IPC_Quick_Send("image.crosstalk " + f1().current_model.get_AOD_X().ToString() + " " + f1().current_model.get_AOD_Y().ToString() + " 0 0 0 " + Gray.ToString() + " " + Gray.ToString() + " " + Gray.ToString());
        }


    }
}
