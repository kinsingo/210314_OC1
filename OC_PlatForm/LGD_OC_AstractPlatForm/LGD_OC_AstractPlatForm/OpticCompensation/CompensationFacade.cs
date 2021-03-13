using System;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.Enums;

namespace LGD_OC_AstractPlatForm.OpticCompensation
{
    public class CompensationFacade
    {
        IBusinessAPI API;
        OCFactory factory;
        public CompensationFacade(IBusinessAPI _API)
        {
            API = _API;
            factory = new OCFactory(API);
        }

        public void OpticCompensation(Model model)
        {
            Sub_Module_Compensation(model, Compensation.AOD);
            Sub_Module_Compensation(model, Compensation.Black);
            Sub_Module_Compensation(model, Compensation.White);
            Sub_Module_Compensation(model, Compensation.GrayLowRef);
            Sub_Module_Compensation(model, Compensation.ELVSS);
            Sub_Module_Compensation(model, Compensation.Main);
        }

        private void Sub_Module_Compensation(Model model, Compensation comp)
        {
            try
            {
                ICompensation compensation = factory.GetCompensationModule(comp, model);
                compensation.Compensation();
            }
            catch(Exception ex)
            {
                API.WriteLine(ex.Message);
            }
        }


    }
}
