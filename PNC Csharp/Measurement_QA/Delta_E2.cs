using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace PNC_Csharp.Measurement_QA
{
    class Delta_E2 : BaseMeasure , I_Measure
    {
        ProgressBar progressBar_E2;
        CheckBox checkBox_1st_Condition_Measure_E2;
        CheckBox checkBox_2nd_Condition_Measure_E2;
        CheckBox checkBox_3rd_Condition_Measure_E2;
        RadioButton Delta_E2_step_value_1;
        RadioButton Delta_E2_step_value_4;
        RadioButton Delta_E2_step_value_8;
        RadioButton Delta_E2_step_value_16;
        TextBox textBox_Delta_E2_Max_Point;
        TextBox textBox_Delta_E2_End_Point;
        DataGridView dataGridView4;
        DataGridView dataGridView5;
        DataGridView dataGridView6;
        CheckBox checkBox_White_PTN_Apply_E2;
        RadioButton radioButton_Min_to_Max_E2;
        TextBox textBox_delay_time_E2;

        I_Channel channel_obj;
        AvgMeasMode avgMeasMode;

        public Delta_E2(
            TextBox _textBox_Show_Compared_Mipi_Data,
            TextBox _textBox_Show_Compared_Mipi_Data2,
            TextBox _textBox_Show_Compared_Mipi_Data3,
            TextBox _textBox_delay_After_Condition_1,
            TextBox _textBox_delay_After_Condition_2,
            TextBox _textBox_delay_After_Condition_3,
            ProgressBar _progressBar_E2,
        CheckBox _checkBox_1st_Condition_Measure_E2,
        CheckBox _checkBox_2nd_Condition_Measure_E2,
        CheckBox _checkBox_3rd_Condition_Measure_E2,
        RadioButton _Delta_E2_step_value_1,
        RadioButton _Delta_E2_step_value_4,
        RadioButton _Delta_E2_step_value_8,
        RadioButton _Delta_E2_step_value_16,
        TextBox _textBox_Delta_E2_Max_Point,
        TextBox _textBox_Delta_E2_End_Point,
        DataGridView _dataGridView4,
        DataGridView _dataGridView5,
        DataGridView _dataGridView6,
        CheckBox _checkBox_White_PTN_Apply_E2,
        RadioButton _radioButton_Min_to_Max_E2,
        TextBox _textBox_delay_time_E2,
        AvgMeasMode _avgMeasMode) 
            : base(_textBox_Show_Compared_Mipi_Data, _textBox_Show_Compared_Mipi_Data2, _textBox_Show_Compared_Mipi_Data3, _textBox_delay_After_Condition_1, _textBox_delay_After_Condition_2, _textBox_delay_After_Condition_3)
        {
            progressBar_E2 = _progressBar_E2;
            checkBox_1st_Condition_Measure_E2 = _checkBox_1st_Condition_Measure_E2;
            checkBox_2nd_Condition_Measure_E2 = _checkBox_2nd_Condition_Measure_E2;
            checkBox_3rd_Condition_Measure_E2 = _checkBox_3rd_Condition_Measure_E2;
            Delta_E2_step_value_1 = _Delta_E2_step_value_1;
            Delta_E2_step_value_4 = _Delta_E2_step_value_4;
            Delta_E2_step_value_8 = _Delta_E2_step_value_8;
            Delta_E2_step_value_16 = _Delta_E2_step_value_16;
            textBox_Delta_E2_Max_Point = _textBox_Delta_E2_Max_Point;
            textBox_Delta_E2_End_Point = _textBox_Delta_E2_End_Point;
            dataGridView4 = _dataGridView4;
            dataGridView5 = _dataGridView5;
            dataGridView6 = _dataGridView6;
            checkBox_White_PTN_Apply_E2 = _checkBox_White_PTN_Apply_E2;
            radioButton_Min_to_Max_E2 = _radioButton_Min_to_Max_E2;
            textBox_delay_time_E2 = _textBox_delay_time_E2;
            avgMeasMode = _avgMeasMode;
        }

        private void Update_ProgressBar()
        {

                progressBar_E2.Value = 0;
                progressBar_E2.Step = 1;
                progressBar_E2.Maximum = 1;
                if (checkBox_1st_Condition_Measure_E2.Checked) progressBar_E2.Maximum++;
                if (checkBox_2nd_Condition_Measure_E2.Checked) progressBar_E2.Maximum++;
                if (checkBox_3rd_Condition_Measure_E2.Checked) progressBar_E2.Maximum++;
                progressBar_E2.PerformStep();
            
        }

        int Get_Step_Value()
        {
            int Step_Value = 0;
            if (Delta_E2_step_value_1.Checked) Step_Value = 1;
            else if (Delta_E2_step_value_4.Checked) Step_Value = 4;
            else if (Delta_E2_step_value_8.Checked) Step_Value = 8;
            else if (Delta_E2_step_value_16.Checked) Step_Value = 16;
            else System.Windows.Forms.MessageBox.Show("It's impossible(Delta E2)");

            return Step_Value;
        }

        private int Get_dbv_max_point()
        {
            int dbv_max_point = Convert.ToInt16(textBox_Delta_E2_Max_Point.Text);
            if (dbv_max_point >= f1().Get_DBV_TrackBar_Maximum()) dbv_max_point = f1().Get_DBV_TrackBar_Maximum();
            else if (dbv_max_point <= 0) dbv_max_point = 0;
            textBox_Delta_E2_Max_Point.Text = dbv_max_point.ToString();
           
            return dbv_max_point;
        }

        private int Get_dbv_end_Point(int dbv_max_point)
        {
            int dbv_end_Point = Convert.ToInt16(textBox_Delta_E2_End_Point.Text);
            if (dbv_end_Point >= (dbv_max_point - 1)) dbv_end_Point = (dbv_max_point - 1);
            else if (dbv_end_Point <= 0) dbv_end_Point = 0;
            textBox_Delta_E2_End_Point.Text = dbv_end_Point.ToString();

            return dbv_end_Point;
        }

        public void MeasureAll(I_Channel _channel_obj)
        {
            if (Availability)
            {
                channel_obj = _channel_obj;
                if (channel_obj.IsMultiChannel()) MultiChannelCheckBoxEnable(able: false);
                Measure();
                if (channel_obj.IsMultiChannel()) MultiChannelCheckBoxEnable(able: true);
            }
        }

        private void Measure()
        {
            if (Availability)
            {
                int delay_time_between_measurement = Convert.ToInt32(textBox_delay_time_E2.Text);

                Update_ProgressBar();
                int dbv_max_point = Get_dbv_max_point();
                int dbv_end_Point = Get_dbv_end_Point(dbv_max_point);
                int Step_Value = Get_Step_Value();

                if (checkBox_1st_Condition_Measure_E2.Checked) dataGridView4.Rows.Clear();
                if (checkBox_2nd_Condition_Measure_E2.Checked) dataGridView5.Rows.Clear();
                if (checkBox_3rd_Condition_Measure_E2.Checked) dataGridView6.Rows.Clear();

                if (checkBox_1st_Condition_Measure_E2.Checked && Availability)
                {
                    dataGridView4.Columns[0].HeaderText = "DBV";
                    Script_Apply_For_Condition1();
                    Measure_and_Calculate_E2(dbv_end_Point, dbv_max_point, delay_time_between_measurement, Step_Value, Condition.first);
                }

                if (checkBox_2nd_Condition_Measure_E2.Checked && Availability)
                {
                    dataGridView5.Columns[0].HeaderText = "DBV";
                    Script_Apply_For_Condition2();
                    Measure_and_Calculate_E2(dbv_end_Point, dbv_max_point, delay_time_between_measurement, Step_Value, Condition.second);
                }

                if (checkBox_3rd_Condition_Measure_E2.Checked && Availability)
                {
                    dataGridView6.Columns[0].HeaderText = "DBV";
                    Script_Apply_For_Condition3();
                    Measure_and_Calculate_E2(dbv_end_Point, dbv_max_point, delay_time_between_measurement, Step_Value, Condition.third);
                }
            }
        }


        private void Measure_and_Calculate_E2(int dbv_end_Point, int dbv_max_point, int delay_time_between_measurement, int Step_Value, Condition condition)
        {
            if(Availability)
            {
                if (checkBox_White_PTN_Apply_E2.Checked)
                {
                    f1().PTN_update(255, 255, 255);
                    Thread.Sleep(300);
                }
                Optic_SH_Delta_E2_Measure(dbv_end_Point, dbv_max_point, delay_time_between_measurement, Step_Value, condition);
                Calculate_Delta_E2_From_x_y_Lv(dbv_end_Point, dbv_max_point, Step_Value, condition);
                progressBar_E2.PerformStep();
            }
        }

        private void Optic_SH_Delta_E2_Measure(int dbv_end_Point, int dbv_max_point, int delay_time_between_measurement, int Step_Value, Condition Condition)
        {
            if(Availability)
            {
                DataGridView datagridview;
                if (Condition == Condition.first) datagridview = dataGridView4;
                else if (Condition == Condition.second) datagridview = dataGridView5;
                else if (Condition == Condition.third) datagridview = dataGridView6;
                else datagridview = null;

                int dbv = dbv_end_Point;

                //Min to Max
                if (radioButton_Min_to_Max_E2.Checked)
                {
                    for (int i = dbv_end_Point; i < (dbv_max_point + Step_Value) & Availability;)
                    {
                        i = i + Step_Value;
                        dbv = i - Step_Value;
                        if (dbv > dbv_max_point)
                            break;

                        f1().Set_BCS(dbv);
                        Thread.Sleep(delay_time_between_measurement);

                        try
                        {
                            channel_obj.Measure_and_Update_Datagridview(datagridview, dbv, IsCalculateDeltaE: true, avgMeasMode);
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
                    for (int i = (dbv_max_point); i > dbv_end_Point - Step_Value & Availability;)
                    {
                        i = i - Step_Value;
                        dbv = i + Step_Value;
                        if (dbv < dbv_end_Point)
                            break;

                        f1().Set_BCS(dbv);
                        Thread.Sleep(delay_time_between_measurement);

                        try
                        {
                            channel_obj.Measure_and_Update_Datagridview(datagridview, dbv, IsCalculateDeltaE: true, avgMeasMode);
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


        private void Calculate_Delta_E2_From_x_y_Lv(int dbv_end_Point, int dbv_max_point, int Step_Value, Condition condition)
        {
            if (Availability)
            {
                Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
                DataGridView datagridview;
                if (condition == Condition.first) datagridview = dataGridView4;
                else if (condition == Condition.second) datagridview = dataGridView5;
                else if (condition == Condition.third) datagridview = dataGridView6;
                else datagridview = null;

                channel_obj.Calculate_Delta_E2_From_x_y_Lv(radioButton_Min_to_Max_E2.Checked, dbv_end_Point, dbv_max_point, Step_Value, datagridview);
            }
        }
    }
}
