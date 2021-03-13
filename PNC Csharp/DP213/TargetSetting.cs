using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNC_Csharp
{
    public struct TargetSetting
    {
        public double Offset_Applied_Max_LV;
        public double Offset_X;
        public double Offset_Y;
        public TargetSetting(double _Copy_PrevMode_M_to_CurMode_Target_Offset_Applied_Max_LV, double _Copy_PrevMode_M_to_CurMode_Target_Offset_X
            , double _Copy_PrevMode_M_to_CurMode_Target_Offset_Y)
        {
            Offset_Applied_Max_LV = _Copy_PrevMode_M_to_CurMode_Target_Offset_Applied_Max_LV;
            Offset_X = _Copy_PrevMode_M_to_CurMode_Target_Offset_X;
            Offset_Y = _Copy_PrevMode_M_to_CurMode_Target_Offset_Y;
        }
    }
}
