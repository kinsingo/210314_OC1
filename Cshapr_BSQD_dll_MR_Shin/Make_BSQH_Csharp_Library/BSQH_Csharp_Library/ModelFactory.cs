using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSQH_Csharp_Library
{
    public class ModelFactory
    {
        public static Model Get_DP213_Instance()
        {
            return DP213Model.getInstance();
        }

        public static Model Get_DP173_Instance()
        {
            return DP173Model.getInstance();
        }
    }
}
