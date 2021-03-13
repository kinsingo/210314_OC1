using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.DiaLog;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation;
using LGD_OC_AstractPlatForm.Enums;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement;
using System.Windows.Forms;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Factory
{
    class ChannelWinformAPIFactory
    {
        WhichVendor vendor;
        public ChannelWinformAPIFactory(WhichVendor _vendor)
        {
            vendor = _vendor;
        }

        public IBusinessAPI GetIBusinessAPI(RichTextBox richtextbox)
        {
            if (vendor == WhichVendor.Gooil)
                return new WinFormAPI(new GooilCommunication(richtextbox), new GooilMeasurement(richtextbox), richtextbox);
            else if (vendor == WhichVendor.LGD)
                return new WinFormAPI(new LGDCommunication(richtextbox), new LGDMeasurement(richtextbox), richtextbox);
            else if (vendor == WhichVendor.Test)
                return new WinFormAPI(new TestCommunication(richtextbox), new TestMeasurement(richtextbox), richtextbox);

            return null;
        }
    }
}
