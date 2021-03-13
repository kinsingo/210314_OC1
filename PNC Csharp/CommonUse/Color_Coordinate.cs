using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNC_Csharp
{
    class Color_Coordinate
    {
        private static double Get_U_from_XY(double x, double y)
        {
            return (4 * x) / (-2 * x + 12 * y + 3);
        }

        private static double Get_V_from_XY(double x, double y)
        {
            return (9 * y) / (-2 * x + 12 * y + 3);
        }

        private static double Get_Distance(double a1, double b1, double a2, double b2)
        {
            return Math.Abs(Math.Sqrt(Math.Pow(a1-a2,2) + Math.Pow(b1-b2,2)));
        }

        public static double Get_UV_Distance_From_XY(double x1, double y1,double x2, double y2)
        { 
            double u1 = Get_U_from_XY(x1, y1);
            double v1 = Get_V_from_XY(x1, y1);

            double u2 = Get_U_from_XY(x2, y2);
            double v2 = Get_V_from_XY(x2, y2);

            return Get_Distance(u1,v1,u2,v2);
        }

        public static double Get_XY_Distance_From_XY(double x1, double y1, double x2, double y2)
        {
            return Get_Distance(x1, y1, x2, y2);
        }
    }
}
