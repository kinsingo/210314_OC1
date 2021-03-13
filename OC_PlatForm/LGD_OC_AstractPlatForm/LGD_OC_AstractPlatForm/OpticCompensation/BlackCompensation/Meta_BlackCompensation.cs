
using LGD_OC_AstractPlatForm.CommonAPI;

namespace LGD_OC_AstractPlatForm.OpticCompensation.BlackCompensation
{
    internal class Meta_BlackCompensation : ICompensation
    {
        IBusinessAPI API;

        public Meta_BlackCompensation(IBusinessAPI _API)
        {
            API = _API;
        }

        public void Compensation()
        {
            API.WriteLine("Meta Black Compensation()");

            double[] XYLv = API.measure_XYL(0);
            API.WriteLine($"X / Y / Lv : {XYLv[0]} / {XYLv[1]} / {XYLv[2]}");

            byte[] read = API.ReadData(55, 5,0, 0);
            API.WriteData(55, read, 0);
        }
    }
}
