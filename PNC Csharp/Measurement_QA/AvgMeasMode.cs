using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PNC_Csharp.Measurement_QA
{
    public class AvgMeasMode
    {
        RadioButton radioButton_AverageMeasure_Meas_1times;
        RadioButton radioButton_AverageMeasure_Meas_3times;
        RadioButton radioButton_AverageMeasure_Meas_5times;
        TextBox textBox_AverageMeasure_Apply_Max_Lv;
        TextBox textBox_AverageMeasure_Delay_Before_Measure;

        public AvgMeasMode(RadioButton _radioButton_AverageMeasure_Meas_1times, RadioButton _radioButton_AverageMeasure_Meas_3times, RadioButton _radioButton_AverageMeasure_Meas_5times, TextBox _textBox_AverageMeasure_Apply_Max_Lv, TextBox _textBox_AverageMeasure_Delay_Before_Measure)
        {
            radioButton_AverageMeasure_Meas_1times = _radioButton_AverageMeasure_Meas_1times;
            radioButton_AverageMeasure_Meas_3times = _radioButton_AverageMeasure_Meas_3times;
            radioButton_AverageMeasure_Meas_5times = _radioButton_AverageMeasure_Meas_5times;
            textBox_AverageMeasure_Apply_Max_Lv = _textBox_AverageMeasure_Apply_Max_Lv;
            textBox_AverageMeasure_Delay_Before_Measure = _textBox_AverageMeasure_Delay_Before_Measure;
        }

        public double Get_AverageMeasure_Apply_Max_Lv()
        {
            return Convert.ToDouble(textBox_AverageMeasure_Apply_Max_Lv.Text);
        }

        public int Get_AverageMeasure_Delay_Before_Measure_MS()
        {
            return Convert.ToInt32(textBox_AverageMeasure_Delay_Before_Measure.Text);
        }

        public int Get_AverageMeasure_Amount()
        {
            if (radioButton_AverageMeasure_Meas_1times.Checked)
                return 1;
            if (radioButton_AverageMeasure_Meas_3times.Checked)
                return 3;
            if (radioButton_AverageMeasure_Meas_5times.Checked)
                return 5;

            throw new Exception("AverageMeasurement Mode Should be selected(1,3 or 5)");
        }

    }
}
