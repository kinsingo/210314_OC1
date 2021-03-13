using System;
using LGD_OC_AstractPlatForm.Enums;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.AODCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.BlackCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.ELVSSCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.GrayLowReferenceCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.MainCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.WhiteCompensation;

namespace LGD_OC_AstractPlatForm.OpticCompensation
{
    internal class OCFactory
    {
        IBusinessAPI API;

        public OCFactory(IBusinessAPI _API)
        {
            API = _API;
        }

        public ICompensation GetCompensationModule(Compensation OCmode,Model model)
        {
            if(OCmode == Compensation.AOD)
            {
                if (model == Model.DP213)
                    return new DP213_AODCompensation(API);
                else if (model == Model.Meta)
                    return new Meta_AODCompensation(API);
                else if (model == Model.DP253)
                    return new DP253_AODCompensation(API);
            }

            if(OCmode == Compensation.ELVSS)
            {
                if (model == Model.DP213)
                    return new DP213_ELVSSCompensation(API);
                else if (model == Model.Meta)
                    return new Meta_ELVSSCompensation(API);
                else if (model == Model.DP253)
                    return new DP253_ELVSSCompensation(API);
            }

            if(OCmode == Compensation.Black)
            {
                if (model == Model.DP213)
                    return new DP213_BlackCompensation(API);
                else if (model == Model.Meta)
                    return new Meta_BlackCompensation(API);
                else if (model == Model.DP253)
                    return new DP253_BlackCompensation(API);
            }

            if (OCmode == Compensation.White)
            {
                if (model == Model.DP213)
                    return new DP213_WhiteCompensation(API);
                else if (model == Model.Meta)
                    return new Meta_WhiteCompensation(API);
                else if (model == Model.DP253)
                    return new DP253_WhiteCompensation(API);
            }

            if (OCmode == Compensation.GrayLowRef)
            {
                if (model == Model.DP213)
                    return new DP213_GrayLowRefCompensation(API);
                else if (model == Model.Meta)
                    return new Meta_GrayLowRefCompensation(API);
                else if (model == Model.DP253)
                    return new DP253_GrayLowRefCompensation(API);
            }

            if(OCmode == Compensation.Main)
            {
                if (model == Model.DP213)
                    return new DP213_MainCompensation(API);
                else if (model == Model.Meta)
                    return new Meta_MainCompensation(API);
                else if (model == Model.DP253)
                    return new DP253_MainCompensation(API);
            }

            throw new Exception("Not a valid input : (" + OCmode.ToString() + "," + model.ToString() + ")");
        }
    }
}
