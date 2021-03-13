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
using PNC_Csharp.CA_Multi_Channels;

namespace PNC_Csharp.Measurement_QA
{
    interface I_Measure
    {
        void MeasureAll(I_Channel channel_obj);
    };


    class BaseMeasure
    {
        public bool Availability;

        protected void MultiChannelCheckBoxEnable(bool able)
        {
            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];
            ca_multi_ch_form.Update_checkBox_MultiCAChannel_Enabled(able);
        }

        public void Set_Availability(bool able)
        {
            Availability = able;
        }

        protected enum Condition
        {
            first,
            second,
            third,
        }

        protected TextBox textBox_Show_Compared_Mipi_Data;
        protected TextBox textBox_Show_Compared_Mipi_Data2;
        protected TextBox textBox_Show_Compared_Mipi_Data3;
        protected TextBox textBox_delay_After_Condition_1;
        protected TextBox textBox_delay_After_Condition_2;
        protected TextBox textBox_delay_After_Condition_3;


        protected BaseMeasure() { Availability = false; }

        protected BaseMeasure(TextBox _textBox_Show_Compared_Mipi_Data, TextBox _textBox_Show_Compared_Mipi_Data2, TextBox _textBox_Show_Compared_Mipi_Data3
            ,TextBox _textBox_delay_After_Condition_1, TextBox _textBox_delay_After_Condition_2, TextBox _textBox_delay_After_Condition_3)
        {
            textBox_Show_Compared_Mipi_Data = _textBox_Show_Compared_Mipi_Data;
            textBox_Show_Compared_Mipi_Data2 = _textBox_Show_Compared_Mipi_Data2;
            textBox_Show_Compared_Mipi_Data3 = _textBox_Show_Compared_Mipi_Data3;
            textBox_delay_After_Condition_1 = _textBox_delay_After_Condition_1;
            textBox_delay_After_Condition_2 = _textBox_delay_After_Condition_2;
            textBox_delay_After_Condition_3 = _textBox_delay_After_Condition_3;
            Availability = false;
        }

        protected Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        protected void Script_Apply_For_Condition1()
        {
            Script_Apply(Condition.first);
            int delay = Convert.ToInt16(textBox_delay_After_Condition_1.Text);
            Thread.Sleep(delay);
            f1().GB_Status_AppendText_Nextline("Thread delay " + delay.ToString() + " was applied", Color.Teal);
        }

        protected void Script_Apply_For_Condition2()
        {
            Script_Apply(Condition.second);
            int delay = Convert.ToInt16(textBox_delay_After_Condition_2.Text);
            Thread.Sleep(delay);
            f1().GB_Status_AppendText_Nextline("Thread delay " + delay.ToString() + " was applied", Color.Green);
        }

        protected void Script_Apply_For_Condition3()
        {
            Script_Apply(Condition.third);
            int delay = Convert.ToInt16(textBox_delay_After_Condition_3.Text);
            Thread.Sleep(delay);
            f1().GB_Status_AppendText_Nextline("Thread delay " + delay.ToString() + " was applied", Color.Olive);
        }

        private void Script_Apply(Condition condition)
        {
            TextBox TextBox_Show_Compared_Mipi_Data;
            if (condition == Condition.first)
            {
                TextBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data;
                f1().GB_Status_AppendText_Nextline("1st Condition Script Applied", Color.Teal);
            }
            else if (condition == Condition.second)
            {
                TextBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data2;
                f1().GB_Status_AppendText_Nextline("2nd Condition Script Applied", Color.Green);
            }
            else if (condition == Condition.third)
            {
                TextBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data3;
                f1().GB_Status_AppendText_Nextline("3rd Condition Script Applied", Color.Olive);
            }
            else TextBox_Show_Compared_Mipi_Data = null;

            //Send "mipi.write" of "delay" command
            for (int i = 0; i < TextBox_Show_Compared_Mipi_Data.Lines.Length - 1; i++)
            {
                System.Windows.Forms.Application.DoEvents();

                if (TextBox_Show_Compared_Mipi_Data.Lines[i].Length >= 10
                    && TextBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 10) == "mipi.write")
                {
                    f1().IPC_Quick_Send(TextBox_Show_Compared_Mipi_Data.Lines[i]);
                }
                else if (TextBox_Show_Compared_Mipi_Data.Lines[i].Length >= 5 && (
                    TextBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5) == "delay"
                    || TextBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5) == "image"))
                {
                    f1().IPC_Quick_Send(TextBox_Show_Compared_Mipi_Data.Lines[i]);
                }
                else if (TextBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 14) == "gpio.i2c.write")
                {
                    f1().IPC_Quick_Send(TextBox_Show_Compared_Mipi_Data.Lines[i]);
                }
                else
                {
                    // It's not a "mipi.write" command , do nothing 
                }
            }
        }
    }
}
