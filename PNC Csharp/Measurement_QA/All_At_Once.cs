using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PNC_Csharp.Measurement_QA
{
    class All_At_Once : BaseMeasure,I_Measure
    {
        ProgressBar progressBar_All_At_Once;

        CheckBox checkBox_All_At_Once_E3;
        CheckBox checkBox_All_At_Once_E2;
        CheckBox checkBox_All_At_Once_Diff_GCS;
        CheckBox checkBox_All_At_Once_Diff_BCS;
        CheckBox checkBox_All_At_Once_AOD_GCS;
        CheckBox checkBox_All_At_Once_Delta_E4;

        TextBox textBox_Aging_Sec;
        TextBox textBox_Aging_Sec_Read;

        I_Channel channel_obj;
        Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        public All_At_Once(
        ProgressBar _progressBar_All_At_Once,
        CheckBox _checkBox_All_At_Once_Delta_E4,
        CheckBox _checkBox_All_At_Once_E3,
        CheckBox _checkBox_All_At_Once_E2,
        CheckBox _checkBox_All_At_Once_Diff_GCS,
        CheckBox _checkBox_All_At_Once_Diff_BCS,
        CheckBox _checkBox_All_At_Once_AOD_GCS,
        TextBox _textBox_Aging_Sec,
        TextBox _textBox_Aging_Sec_Read):base()
        {
            progressBar_All_At_Once = _progressBar_All_At_Once;

            checkBox_All_At_Once_Delta_E4 = _checkBox_All_At_Once_Delta_E4;
            checkBox_All_At_Once_E3 = _checkBox_All_At_Once_E3;
            checkBox_All_At_Once_E2 = _checkBox_All_At_Once_E2;
            checkBox_All_At_Once_Diff_GCS = _checkBox_All_At_Once_Diff_GCS;
            checkBox_All_At_Once_Diff_BCS = _checkBox_All_At_Once_Diff_BCS;
            checkBox_All_At_Once_AOD_GCS = _checkBox_All_At_Once_AOD_GCS;

            textBox_Aging_Sec = _textBox_Aging_Sec;
            textBox_Aging_Sec_Read = _textBox_Aging_Sec_Read;
        }



        public void MeasureAll(I_Channel _channel_obj)
        {
            if (Availability)
            {
                channel_obj = _channel_obj;
                All_At_Once_ProgressBar_Update();
                All_At_Once_Aging();
                AllAtOnce_Measure();
            }
        }

        private void All_At_Once_Aging()
        {
            if (Availability)
            {
                f1().PTN_update(255, 255, 255);
                progressBar_All_At_Once.Maximum++;
                int Sec = Convert.ToInt16(textBox_Aging_Sec.Text);
                textBox_Aging_Sec_Read.Text = Sec.ToString();
                Application.DoEvents();
                while (true)
                {
                    if (Sec > 0)
                    {
                        Thread.Sleep(1000);
                        textBox_Aging_Sec_Read.Text = (Sec--).ToString();
                        Application.DoEvents();

                    }
                    else
                    {
                        textBox_Aging_Sec_Read.Text = Sec.ToString();
                        Application.DoEvents();
                        break;
                    }
                }
                progressBar_All_At_Once.PerformStep();
            }
        }

        private void All_At_Once_ProgressBar_Update()
        {
            if(Availability)
            {
                //Set ProgressBar_E3 Max and Step and Value
                int Progress_Bar_All_At_Once_Max = 0;
                if (checkBox_All_At_Once_E3.Checked) Progress_Bar_All_At_Once_Max++;
                if (checkBox_All_At_Once_E2.Checked) Progress_Bar_All_At_Once_Max++;
                if (checkBox_All_At_Once_Diff_GCS.Checked) Progress_Bar_All_At_Once_Max++;
                if (checkBox_All_At_Once_Diff_BCS.Checked) Progress_Bar_All_At_Once_Max++;
                if (checkBox_All_At_Once_AOD_GCS.Checked) Progress_Bar_All_At_Once_Max++;
                if (checkBox_All_At_Once_Delta_E4.Checked) Progress_Bar_All_At_Once_Max++;

                progressBar_All_At_Once.Value = 0;
                progressBar_All_At_Once.Step = 1;
                progressBar_All_At_Once.Maximum = 1;
                progressBar_All_At_Once.Maximum += Progress_Bar_All_At_Once_Max;
                progressBar_All_At_Once.PerformStep();
            }
        }

        private void AllAtOnce_Measure()
        {
            Optic_Measurement_Form optic_meas_ui_form = (Optic_Measurement_Form)System.Windows.Forms.Application.OpenForms["Optic_Measurement_Form"];

            //Perform Button E3/E2/Diff
            if (checkBox_All_At_Once_E3.Checked && Availability)
            {
                optic_meas_ui_form.Delta_E3_calculation();
                progressBar_All_At_Once.PerformStep();
            }
            if (checkBox_All_At_Once_E2.Checked && Availability)
            {
                optic_meas_ui_form.Delta_E2_calculation();
                progressBar_All_At_Once.PerformStep();
            }
            if (checkBox_All_At_Once_Diff_GCS.Checked && Availability)
            {
                optic_meas_ui_form.SH_GCS_Difference_Measure();
                progressBar_All_At_Once.PerformStep();
            }
            if (checkBox_All_At_Once_Diff_BCS.Checked && Availability)
            {
                optic_meas_ui_form.SH_BCS_Difference_Measure();
                progressBar_All_At_Once.PerformStep();
            }
            if (checkBox_All_At_Once_AOD_GCS.Checked && Availability)
            {
                optic_meas_ui_form.AOD_GCS();
                progressBar_All_At_Once.PerformStep();
            }
            if (checkBox_All_At_Once_Delta_E4.Checked && Availability)
            {
                optic_meas_ui_form.IR_Drop_Delta_E();
                progressBar_All_At_Once.PerformStep();
            }
        }




    }
}
