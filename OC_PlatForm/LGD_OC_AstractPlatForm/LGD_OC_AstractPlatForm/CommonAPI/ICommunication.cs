

namespace LGD_OC_AstractPlatForm.CommonAPI
{
    public interface ICommunication
    {
        void WriteData(byte address, byte[] parameters, int channel_num);
        byte[] ReadData(byte address, int amount,int offset, int channel_num);

        void DisplayMonoPattern(byte[] RGB, int channel_num);
        void DisplayBoxPattern(byte[] Box_RGB, byte[] Background_RGB,int[] Pos_BoxLeftTop, int[] Pos_BoxRightBottom, int channel_num);
    }
}
