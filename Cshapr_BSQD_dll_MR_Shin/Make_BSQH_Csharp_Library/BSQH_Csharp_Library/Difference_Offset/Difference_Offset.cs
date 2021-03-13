using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSQH_Csharp_Library
{
    public class Difference_Offset
    {
        internal static bool IsCase1(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
           , double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode)
        {
            int PrevMode_RegisterNum = (Cur_Register_PrevMode - Prev_Register_PrevMode);
            int CurMode_RegisterNum = (Cur_Register_CurMode - Prev_Register_CurMode);

            if (Prev_Lv_CurMode > Prev_Lv_PrevMode && Cur_Lv_CurMode > Cur_Lv_PrevMode && CurMode_RegisterNum > PrevMode_RegisterNum)
                return true;
            else
                return false;
        }

        internal static bool IsCase2(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
            , double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode)
        {
            int PrevMode_RegisterNum = (Cur_Register_PrevMode - Prev_Register_PrevMode);
            int CurMode_RegisterNum = (Cur_Register_CurMode - Prev_Register_CurMode);

            if (Prev_Lv_CurMode > Prev_Lv_PrevMode && Cur_Lv_CurMode < Cur_Lv_PrevMode && CurMode_RegisterNum < PrevMode_RegisterNum)
                return true;
            else
                return false;
        }


        internal static bool IsCase3(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
    , double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode)
        {
            int PrevMode_RegisterNum = (Cur_Register_PrevMode - Prev_Register_PrevMode);
            int CurMode_RegisterNum = (Cur_Register_CurMode - Prev_Register_CurMode);

            if (Prev_Lv_CurMode < Prev_Lv_PrevMode && Cur_Lv_CurMode > Cur_Lv_PrevMode && CurMode_RegisterNum > PrevMode_RegisterNum)
                return true;
            else
                return false;
        }

        internal static bool IsCase4(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
    , double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode)
        {
            int PrevMode_RegisterNum = (Cur_Register_PrevMode - Prev_Register_PrevMode);
            int CurMode_RegisterNum = (Cur_Register_CurMode - Prev_Register_CurMode);

            if (Prev_Lv_CurMode < Prev_Lv_PrevMode && Cur_Lv_CurMode < Cur_Lv_PrevMode && CurMode_RegisterNum > PrevMode_RegisterNum)
                return true;
            else
                return false;
        }

        internal static bool IsCase5(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
, double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode)
        {
            int PrevMode_RegisterNum = (Cur_Register_PrevMode - Prev_Register_PrevMode);
            int CurMode_RegisterNum = (Cur_Register_CurMode - Prev_Register_CurMode);

            if (Prev_Lv_CurMode < Prev_Lv_PrevMode && Cur_Lv_CurMode < Cur_Lv_PrevMode && CurMode_RegisterNum < PrevMode_RegisterNum)
                return true;
            else
                return false;
        }

        internal static bool IsToBeApplied(double Prev_Lv_PrevMode, double Cur_Lv_PrevMode, int Prev_Register_PrevMode, int Cur_Register_PrevMode
, double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode)
        {
            return IsCase1(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode, Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode)
             || IsCase2(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode, Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode)
             || IsCase3(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode, Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode)
             || IsCase4(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode, Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode)
             || IsCase5(Prev_Lv_PrevMode, Cur_Lv_PrevMode, Prev_Register_PrevMode, Cur_Register_PrevMode, Prev_Lv_CurMode, Cur_Lv_CurMode, Prev_Register_CurMode, Cur_Register_CurMode);
        }


        internal static bool IsNotOffsetSkip(int Prev_Register_PrevMode, int Cur_Register_PrevMode, double Prev_Lv_CurMode, double Cur_Lv_CurMode, int Prev_Register_CurMode, int Cur_Register_CurMode, double Delta_L_Limit)
        {
            return (!(OffsetSkip_Case1(Prev_Lv_CurMode, Cur_Lv_CurMode, Delta_L_Limit) || OffsetSkip_Case2(Prev_Register_PrevMode, Cur_Register_PrevMode, Prev_Register_CurMode, Cur_Register_CurMode) || OffsetSkip_Case3(Prev_Register_PrevMode, Cur_Register_PrevMode, Prev_Register_CurMode, Cur_Register_CurMode)));
        }

        internal static bool OffsetSkip_Case1(double Prev_Lv_CurMode, double Cur_Lv_CurMode,double Delta_L_Limit)
        {
            double Delta_L = Math.Abs((Prev_Lv_CurMode - Cur_Lv_CurMode) / (Prev_Lv_CurMode));
            return (Delta_L < Delta_L_Limit);
        }

        internal static bool OffsetSkip_Case2(int Prev_Register_PrevMode, int Cur_Register_PrevMode, int Prev_Register_CurMode, int Cur_Register_CurMode)
        {
            int PrevMode_Diff = Cur_Register_PrevMode - Prev_Register_PrevMode;
            int CurMode_Diff = Cur_Register_CurMode - Prev_Register_CurMode;
            return (PrevMode_Diff == CurMode_Diff);
        }
        
        internal static bool OffsetSkip_Case3(int Prev_Register_PrevMode, int Cur_Register_PrevMode, int Prev_Register_CurMode, int Cur_Register_CurMode)
        {
            int PrevMode_Diff = Cur_Register_PrevMode - Prev_Register_PrevMode;
            int CurMode_Diff = Cur_Register_CurMode - Prev_Register_CurMode;
            return ((PrevMode_Diff * CurMode_Diff) < 0);
        }

        internal static RGB Get_CurMode_RGB_Offset(double PrevMode_X, double PrevMode_Y, double PrevMode_Lv, double CurMode_X, double CurMode_Y, double CurMode_Lv)
        {
            RGB CurMode_RGB_Offset = new RGB();

            //1
            if (CurMode_Lv > PrevMode_Lv && CurMode_X > PrevMode_X && CurMode_Y > PrevMode_Y)
            {
                CurMode_RGB_Offset.int_R = -1; CurMode_RGB_Offset.int_G = -1; CurMode_RGB_Offset.int_B = 0;
            }
            //2
            else if (CurMode_Lv > PrevMode_Lv && CurMode_X > PrevMode_X && CurMode_Y < PrevMode_Y)
            {
                CurMode_RGB_Offset.int_R = -1; CurMode_RGB_Offset.int_G = 0; CurMode_RGB_Offset.int_B = -1;
            }
            //3
            else if (CurMode_Lv > PrevMode_Lv && CurMode_X < PrevMode_X && CurMode_Y > PrevMode_Y)
            {
                CurMode_RGB_Offset.int_R = 0; CurMode_RGB_Offset.int_G = -1; CurMode_RGB_Offset.int_B = 0;
            }
            //4
            else if (CurMode_Lv > PrevMode_Lv && CurMode_X < PrevMode_X && CurMode_Y < PrevMode_Y)
            {
                CurMode_RGB_Offset.int_R = -1; CurMode_RGB_Offset.int_G = -1; CurMode_RGB_Offset.int_B = -2;
            }
            //5
            else if (CurMode_Lv < PrevMode_Lv && CurMode_X > PrevMode_X && CurMode_Y > PrevMode_Y)
            {
                CurMode_RGB_Offset.int_R = 1; CurMode_RGB_Offset.int_G = 1; CurMode_RGB_Offset.int_B = 2;
            }
            //6
            else if (CurMode_Lv < PrevMode_Lv && CurMode_X > PrevMode_X && CurMode_Y < PrevMode_Y)
            {
                CurMode_RGB_Offset.int_R = 0; CurMode_RGB_Offset.int_G = 1; CurMode_RGB_Offset.int_B = 0;
            }
            //7
            else if (CurMode_Lv < PrevMode_Lv && CurMode_X < PrevMode_X && CurMode_Y > PrevMode_Y)
            {
                CurMode_RGB_Offset.int_R = 1; CurMode_RGB_Offset.int_G = 0; CurMode_RGB_Offset.int_B = 1;
            }
            //8
            else if (CurMode_Lv < PrevMode_Lv && CurMode_X < PrevMode_X && CurMode_Y < PrevMode_Y)
            {
                CurMode_RGB_Offset.int_R = 1; CurMode_RGB_Offset.int_G = 1; CurMode_RGB_Offset.int_B = 0;
            }
            else
            {
                CurMode_RGB_Offset.int_R = 0; CurMode_RGB_Offset.int_G = 0; CurMode_RGB_Offset.int_B = 0;
            }
            return CurMode_RGB_Offset;
        }

        public static RGB Get_RGB_Difference_Offset(XYLv PrevBand_PrevMode_Measure, XYLv CurBand_PrevMode_Measure, RGB PrevBand_PrevMode_RGB, RGB CurBand_PrevMode_RGB
    , XYLv PrevBand_CurMode_Measure, XYLv CurBand_CurMode_Measure, RGB PrevBand_CurMode_RGB, RGB CurBand_CurMode_RGB,double Delta_L_Limit)
        {
            if (IsNotOffsetSkip(PrevBand_PrevMode_RGB.int_G, CurBand_PrevMode_RGB.int_G, PrevBand_CurMode_Measure.double_Lv, CurBand_CurMode_Measure.double_Lv, PrevBand_CurMode_RGB.int_G, CurBand_CurMode_RGB.int_G, Delta_L_Limit)
                && IsToBeApplied(PrevBand_PrevMode_Measure.double_Lv, CurBand_PrevMode_Measure.double_Lv, PrevBand_PrevMode_RGB.int_G, CurBand_PrevMode_RGB.int_G, PrevBand_CurMode_Measure.double_Lv, CurBand_CurMode_Measure.double_Lv, PrevBand_CurMode_RGB.int_G, CurBand_CurMode_RGB.int_G))
            {
                return Get_CurMode_RGB_Offset(CurBand_PrevMode_Measure.double_X, CurBand_PrevMode_Measure.double_Y, CurBand_PrevMode_Measure.double_Lv
                    , CurBand_CurMode_Measure.double_X, CurBand_CurMode_Measure.double_Y, CurBand_CurMode_Measure.double_Lv);
            }
            else
                return new RGB(0);
        }
    }
}
