
using LGD_OC_AstractPlatForm.CommonAPI;

namespace LGD_OC_AstractPlatForm.OpticCompensation.AODCompensation
{
    internal class DP213_AODCompensation : ICompensation
    {
        IBusinessAPI API;

        public DP213_AODCompensation(IBusinessAPI _API)
        {
            API = _API;
        }

        public void Compensation()
        {
            API.WriteLine("DP213 AOD Compensation()");
            
            double[] XYLv = API.measure_XYL(0);
            API.WriteLine($"X / Y / Lv : {XYLv[0]} / {XYLv[1]} / {XYLv[2]}");

            byte[] read = API.ReadData(55, 5,0,0);
            API.WriteData(55, read,0);
        }
    }
}
