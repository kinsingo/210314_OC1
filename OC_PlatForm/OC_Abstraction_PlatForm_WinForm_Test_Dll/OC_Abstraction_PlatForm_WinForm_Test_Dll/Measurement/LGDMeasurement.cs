using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement
{
    class LGDMeasurement : Imeasurement
    {
        RichTextBox richtextbox;
        public LGDMeasurement(RichTextBox _richtextbox)
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
            throw new NotImplementedException();
        }
    }
}
