
using System.Drawing;

namespace LGD_OC_AstractPlatForm.CommonAPI
{
    public interface IBusinessAPI : ICommunication, Imeasurement
    {
        void WriteLine(string str, Color color);
        void WriteLine(string str);
    }
}
