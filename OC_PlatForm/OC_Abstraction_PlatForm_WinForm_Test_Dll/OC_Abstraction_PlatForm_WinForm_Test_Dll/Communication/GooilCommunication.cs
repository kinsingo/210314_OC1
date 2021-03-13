using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication
{
    class GooilCommunication : ICommunication
    {
        RichTextBox richtextbox;
        public GooilCommunication(RichTextBox _richtextbox)
        {
            richtextbox = _richtextbox;
        }

        public void DisplayBoxPattern(byte[] Box_RGB, byte[] Background_RGB, int[] Pos_BoxLeftTop, int[] Pos_BoxRightBottom, int channel_num)
        {
            throw new NotImplementedException();
        }

        public void DisplayMonoPattern(byte[] RGB, int channel_num)
        {
            throw new NotImplementedException();
        }
        public byte[] ReadData(byte address, int amount, int offset, int channel_num)
        {
            throw new NotImplementedException();
        }

        public void WriteData(byte address, byte[] parameters, int channel_num)
        {
            throw new NotImplementedException();
        }
    }
}
