
using System.Windows.Forms;
using System.Drawing;

namespace LGD_OC_AstractPlatForm.CommonAPI
{
    public class WinFormAPI : IBusinessAPI
    {
        ICommunication communication_obj;
        Imeasurement measurement_obj;
        RichTextBox richtextbox;

        public WinFormAPI(ICommunication _communication_obj, Imeasurement _measurement_obj, RichTextBox _richTextBox)
        {
            communication_obj = _communication_obj;
            measurement_obj = _measurement_obj;
            richtextbox = _richTextBox;
        }

        public void WriteLine(string str, Color color)
        {
            //Color (Text 색 바꾸고,AppendText)
            richtextbox.SelectionColor = color;
            richtextbox.AppendText(str + "\r\n");

            //Black (Text 색 원복)
            richtextbox.SelectionColor = Color.Black;

            //scroll richbox to the end line
            richtextbox.SelectionStart = richtextbox.Text.Length;
            richtextbox.ScrollToCaret();
        }

        public void WriteLine(string str)
        {
            WriteLine(str, Color.Black);
        }

        public void WriteData(byte address, byte[] parameters, int channel_num)
        {
            communication_obj.WriteData(address, parameters, channel_num);
        }

        public byte[] ReadData(byte address, int amount,int offset, int channel_num)
        {
            return communication_obj.ReadData(address, amount,offset, channel_num);
        }

        public void DisplayMonoPattern(byte[] RGB, int channel_num)
        {
            communication_obj.DisplayMonoPattern(RGB, channel_num);
        }

        public void DisplayBoxPattern(byte[] Box_RGB, byte[] Background_RGB, int[] Pos_BoxLeftTop, int[] Pos_BoxRightBottom, int channel_num)
        {
            communication_obj.DisplayBoxPattern(Box_RGB, Background_RGB, Pos_BoxLeftTop, Pos_BoxRightBottom, channel_num);
        }

        public double[] measure_XYL(int channel_num)
        {
            return measurement_obj.measure_XYL(channel_num);
        }

        public double[] measure_UVL(int channel_num)
        {
            return measurement_obj.measure_UVL(channel_num);
        }

        public double Get_Frequency(int channel_num)
        {
            return measurement_obj.Get_Frequency(channel_num);
        }
    }
}
