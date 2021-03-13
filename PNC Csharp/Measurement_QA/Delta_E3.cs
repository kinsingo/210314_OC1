using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;





namespace PNC_Csharp.Measurement_QA
{
    class Delta_E3 : BaseMeasure , I_Measure
    {
        ProgressBar progressBar_E3;
        RadioButton radioButton_Min_to_Max_E3;
        CheckBox checkBox_1st_Condition_Measure_E3;
        CheckBox checkBox_2nd_Condition_Measure_E3;
        CheckBox checkBox_3rd_Condition_Measure_E3;
        DataGridView dataGridView1;
        DataGridView dataGridView2;
        DataGridView dataGridView3;

        bool[] checkBox_Condition1;
        bool[] checkBox_Condition2;
        bool[] checkBox_Condition3;
        string[] textBox_Condition1;
        string[] textBox_Condition2;
        string[] textBox_Condition3;

        TextBox textBox_delay_time;
        TextBox textBox_Delta_E_End_Point;

        I_Channel channel_obj;
        AvgMeasMode avgMeasMode;
        public Delta_E3(ProgressBar _progressBar_E3,RadioButton _radioButton_Min_to_Max_E3,
            TextBox _textBox_Show_Compared_Mipi_Data, TextBox _textBox_Show_Compared_Mipi_Data2, TextBox _textBox_Show_Compared_Mipi_Data3,
            TextBox _textBox_delay_After_Condition_1, TextBox _textBox_delay_After_Condition_2, TextBox _textBox_delay_After_Condition_3,
            CheckBox _checkBox_1st_Condition_Measure_E3, CheckBox _checkBox_2nd_Condition_Measure_E3, CheckBox _checkBox_3rd_Condition_Measure_E3,
            DataGridView _dataGridView1, DataGridView _dataGridView2, DataGridView _dataGridView3,
            bool[] _checkBox_Condition1, bool[] _checkBox_Condition2, bool[] _checkBox_Condition3
            ,string[] _textBox_Condition1, string[] _textBox_Condition2,string[] _textBox_Condition3, TextBox _textBox_delay_time, TextBox _textBox_Delta_E_End_Point, AvgMeasMode _avgMeasMode)
            : base(_textBox_Show_Compared_Mipi_Data, _textBox_Show_Compared_Mipi_Data2, _textBox_Show_Compared_Mipi_Data3, _textBox_delay_After_Condition_1, _textBox_delay_After_Condition_2, _textBox_delay_After_Condition_3)
        {
            progressBar_E3 = _progressBar_E3;
            radioButton_Min_to_Max_E3 = _radioButton_Min_to_Max_E3;
            checkBox_1st_Condition_Measure_E3 = _checkBox_1st_Condition_Measure_E3;
            checkBox_2nd_Condition_Measure_E3 = _checkBox_2nd_Condition_Measure_E3;
            checkBox_3rd_Condition_Measure_E3 = _checkBox_3rd_Condition_Measure_E3;
            dataGridView1 = _dataGridView1;
            dataGridView2 = _dataGridView2;
            dataGridView3 = _dataGridView3;
            checkBox_Condition1 = _checkBox_Condition1;
            checkBox_Condition2 = _checkBox_Condition2;
            checkBox_Condition3 = _checkBox_Condition3;
            textBox_Condition1 = _textBox_Condition1;
            textBox_Condition2 = _textBox_Condition2;
            textBox_Condition3 = _textBox_Condition3;

            textBox_delay_time = _textBox_delay_time;
            textBox_Delta_E_End_Point = _textBox_Delta_E_End_Point;
            avgMeasMode = _avgMeasMode;

    }

        private void Update_ProgressBar()
        {
            //Set ProgressBar_E3 Max and Step and Value
            int Progress_Bar_Condion1_Max = 0;
            int Progress_Bar_Condion2_Max = 0;
            int Progress_Bar_Condion3_Max = 0;
            for (int i = 0; i < checkBox_Condition1.Length; i++)
            {
                if (checkBox_Condition1[i]) Progress_Bar_Condion1_Max++;
                if (checkBox_Condition2[i]) Progress_Bar_Condion2_Max++;
                if (checkBox_Condition3[i]) Progress_Bar_Condion3_Max++;
            }
            progressBar_E3.Value = 0;
            progressBar_E3.Step = 1;
            progressBar_E3.Maximum = 1;
            if (checkBox_1st_Condition_Measure_E3.Checked) progressBar_E3.Maximum += Progress_Bar_Condion1_Max;
            if (checkBox_2nd_Condition_Measure_E3.Checked) progressBar_E3.Maximum += Progress_Bar_Condion2_Max;
            if (checkBox_3rd_Condition_Measure_E3.Checked) progressBar_E3.Maximum += Progress_Bar_Condion3_Max;
            progressBar_E3.PerformStep();
        }

        private void Initialize()
        {
            if (checkBox_1st_Condition_Measure_E3.Checked) dataGridView1.Rows.Clear();
            if (checkBox_2nd_Condition_Measure_E3.Checked) dataGridView2.Rows.Clear();
            if (checkBox_3rd_Condition_Measure_E3.Checked) dataGridView3.Rows.Clear();
        }

        private void Measure_And_Calculate_Delta_E(int gray_end_Point, int delay_time_between_measurement, Condition condition)
        {
            if(Availability)
            {
                Optic_SH_Delta_E3_Measure(gray_end_Point, delay_time_between_measurement, condition);

                int Addtional_DeltaE_Rows = 0;
                if (condition == Condition.first) Addtional_DeltaE_Rows = (dataGridView1.Rows.Count - 1);
                else if (condition == Condition.second) Addtional_DeltaE_Rows = (dataGridView2.Rows.Count - 1);
                else if (condition == Condition.third) Addtional_DeltaE_Rows = (dataGridView3.Rows.Count - 1);

                Calculate_Delta_E3_From_x_y_Lv(gray_end_Point, Addtional_DeltaE_Rows, condition);
            }
        }


        private void Optic_SH_Delta_E3_Measure(int gray_end_Point, int delay_time_between_measurement, Condition Condition)
        {
            if (Availability)
            {
                DataGridView datagridview;
                if (Condition == Condition.first)
                    datagridview = dataGridView1;
                else if (Condition == Condition.second)
                    datagridview = dataGridView2;
                else if (Condition == Condition.third)
                    datagridview = dataGridView3;
                else
                    datagridview = null;

                //Gray 48~255 에서 X/Y/Lv 먼저 찍음
                if (radioButton_Min_to_Max_E3.Checked)
                {
                    for (int gray = gray_end_Point; gray <= 255 & Availability; gray++)
                    {
                        f1().PTN_update(gray, gray, gray);
                        Thread.Sleep(delay_time_between_measurement);
                        try
                        {
                            channel_obj.Measure_and_Update_Datagridview(datagridview, gray, IsCalculateDeltaE: true, avgMeasMode);
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
                        f1().PTN_update(gray, gray, gray);
                        Thread.Sleep(delay_time_between_measurement);
                        try
                        {
                            channel_obj.Measure_and_Update_Datagridview(datagridview, gray, IsCalculateDeltaE: true, avgMeasMode);
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


        private void Calculate_Delta_E3_From_x_y_Lv(int gray_end_Point, int Addtional_DeltaE_Rows, Condition condition)
        {
            if (Availability)
            {
                DataGridView datagridview;
                if (condition == Condition.first) datagridview = dataGridView1;
                else if (condition == Condition.second) datagridview = dataGridView2;
                else if (condition == Condition.third) datagridview = dataGridView3;
                else datagridview = null;

                channel_obj.Calculate_Delta_E3_From_x_y_Lv(radioButton_Min_to_Max_E3.Checked, gray_end_Point, Addtional_DeltaE_Rows, datagridview);
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
                Update_ProgressBar();
                Initialize();

                int gray_end_Point = Get_AOD_or_DeltaE3_gray_end_Point();
                int delay_time_between_measurement = Convert.ToInt16(textBox_delay_time.Text);

                if (checkBox_1st_Condition_Measure_E3.Checked && Availability)
                {
                    dataGridView1.Columns[0].HeaderText = "Gray";
                    Script_Apply_For_Condition1();

                    for (int i = 0; i < checkBox_Condition1.Length; i++)
                    {
                        if (checkBox_Condition1[i] && Availability)
                        {
                            string DBV = textBox_Condition1[i].PadLeft(3, '0');//dex to hex (as a string form)
                            try
                            {
                                f1().DBV_Setting(DBV);

                                dataGridView1.Rows.Add(i.ToString() + ")DBV", DBV, "-", "-");
                                f1().GB_Status_AppendText_Nextline(i.ToString() + ")1st Condition DBV[" + DBV + "] was applied", Color.Teal);
                            }
                            catch
                            {
                                f1().GB_Status_AppendText_Nextline("Sending 1st Condition DBV[" + DBV + "] was failed", Color.Red);
                            }
                            Measure_And_Calculate_Delta_E(gray_end_Point, delay_time_between_measurement, Condition.first);
                            progressBar_E3.PerformStep();
                        }
                        else
                        {
                            f1().GB_Status_AppendText_Nextline(i.ToString() + ")DBV Point Skip(1st Condition)", Color.Black);
                        }
                    }
                }

                if (checkBox_2nd_Condition_Measure_E3.Checked && Availability)
                {
                    dataGridView2.Columns[0].HeaderText = "Gray";
                    Script_Apply_For_Condition2();
                    for (int i = 0; i < checkBox_Condition1.Length; i++)
                    {
                        if (checkBox_Condition2[i] && Availability)
                        {
                            string DBV = textBox_Condition2[i].PadLeft(3, '0');//dex to hex (as a string form)
                            try
                            {
                                f1().DBV_Setting(DBV);

                                dataGridView2.Rows.Add(i.ToString() + ")DBV", DBV, "-", "-");
                                f1().GB_Status_AppendText_Nextline(i.ToString() + ")2nd Condition DBV[" + DBV + "] was applied", Color.Green);
                            }
                            catch
                            {
                                f1().GB_Status_AppendText_Nextline("Sending 2nd Condition DBV[" + DBV + "] was failed", Color.Red);
                            }
                            Measure_And_Calculate_Delta_E(gray_end_Point, delay_time_between_measurement, Condition.second);
                            progressBar_E3.PerformStep();
                        }
                        else
                        {
                            f1().GB_Status_AppendText_Nextline(i.ToString() + ")DBV Point Skip(2nd Condition)", Color.Black);
                        }
                    }
                }
                if (checkBox_3rd_Condition_Measure_E3.Checked && Availability)
                {
                    dataGridView3.Columns[0].HeaderText = "Gray";
                    Script_Apply_For_Condition3();
                    for (int i = 0; i < checkBox_Condition1.Length; i++)
                    {
                        if (checkBox_Condition3[i] && Availability)
                        {
                            string DBV = textBox_Condition3[i].PadLeft(3, '0');//dex to hex (as a string form)
                            try
                            {
                                f1().DBV_Setting(DBV);

                                dataGridView3.Rows.Add(i.ToString() + ")DBV", DBV, "-", "-");
                                f1().GB_Status_AppendText_Nextline(i.ToString() + ")3rd Condition DBV [" + DBV + "] was applied", Color.Olive);
                            }
                            catch
                            {
                                f1().GB_Status_AppendText_Nextline("Sending 3rd Condition DBV[" + DBV + "] was failed", Color.Red);
                            }
                            Measure_And_Calculate_Delta_E(gray_end_Point, delay_time_between_measurement, Condition.third);
                            progressBar_E3.PerformStep();
                        }
                        else
                        {
                            f1().GB_Status_AppendText_Nextline(i.ToString() + ")DBV Point Skip(3rd Condition)", Color.Black);
                        }
                    }
                }
            }
        }





    }
}
