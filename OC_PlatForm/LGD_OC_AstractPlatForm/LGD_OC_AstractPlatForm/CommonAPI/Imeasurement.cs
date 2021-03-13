

namespace LGD_OC_AstractPlatForm.CommonAPI
{
    public interface Imeasurement
    {
        double[] measure_XYL(int channel_num);
        double[] measure_UVL(int channel_num);
        double Get_Frequency(int channel_num);

    }
}
