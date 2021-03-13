using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication
{
    class TestCommunication : ICommunication
    {
        RichTextBox richtextbox;
        public TestCommunication(RichTextBox _richtextbox)
        {
            richtextbox = _richtextbox;
        }

        public void WriteData(byte address, byte[] parameters, int channel_num)
        {
            string HexAddress = "0x" + Convert.ToString(address, 16).PadLeft(2, '0').ToUpper();
            string str = $"{channel_num}ch Test WriteData Address : {HexAddress}, Param :";
            foreach (byte param in parameters)
                str += (" 0x" + Convert.ToString(param, 16).ToUpper());

            richtextbox.AppendText(str + "\n");
        }

        public byte[] ReadData(byte address, int amount, int offset, int channel_num)
        {
            string HexAddress = "0x" + Convert.ToString(address, 16).PadLeft(2, '0').ToUpper();
            string str = $"{channel_num}ch Test ReadData Address : {HexAddress}, Param :";

            Random rand = new Random();
            byte[] result = new byte[amount];
            for (int i = 0; i < amount; i++)
            {
                result[i] = (byte)rand.Next(0, 256);
                str += (" 0x" + Convert.ToString(result[i], 16).PadLeft(2, '0').ToUpper());
            }

            richtextbox.AppendText(str + "\n");

            return result;
        }

        public void DisplayMonoPattern(byte[] RGB, int channel_num)
        {
            string str = string.Format("Display Pattern R/G/B: {0}/{1}/{2}", RGB[0], RGB[1], RGB[2]);
            richtextbox.AppendText(str + "\n");
        }

        public void DisplayBoxPattern(byte[] Box_RGB, byte[] Background_RGB, int[] Pos_BoxLeftTop, int[] Pos_BoxRightBottom, int channel_num)
        {
            int boxWidth = Pos_BoxRightBottom[0] - Pos_BoxLeftTop[0];
            int boxHeight = Pos_BoxRightBottom[1] - Pos_BoxLeftTop[1];

            string box_image = string.Format("image.crosstalk {0} {1} {2} {3} {4} {5} {6} {7}", boxWidth, boxHeight, Background_RGB[0], Background_RGB[1], Background_RGB[2], Box_RGB[0], Box_RGB[1], Box_RGB[2]);
            richtextbox.AppendText(box_image + "\n");
        }
    }
}
