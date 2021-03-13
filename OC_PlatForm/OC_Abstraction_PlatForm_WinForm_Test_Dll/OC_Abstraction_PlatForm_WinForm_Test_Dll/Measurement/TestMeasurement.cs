using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement
{
    class TestMeasurement : Imeasurement
    {
        RichTextBox richtextbox;
        public TestMeasurement(RichTextBox _richtextbox)
        {
            richtextbox = _richtextbox;
        }

        public double Get_Frequency(int channel_num)
        {
            throw new NotImplementedException();
        }

        public double[] measure_UVL(int channel_num)
        {
            throw new NotImplementedException();
        }

        public double[] measure_XYL(int channel_num)
        {
            Random rand = new Random();
            double x = rand.NextDouble();
            double y = rand.NextDouble();
            double Lv = rand.Next(1, 1000);
            richtextbox.AppendText(String.Format("Test Measure X/Y/Lv : {0:0.0000}/{1:0.0000}/{2:0.00} \n", x, y, Lv));

            return new double[3] { x, y, Lv };
        }
    }
}
