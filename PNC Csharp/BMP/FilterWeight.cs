using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNC_Csharp
{
    static class EdgeFilter
    {



        public static double[][] Edge_Horizontal_Detection_Weight()
        {
            double[][] Weight = new double[3][];
            Weight[0] = new double[3] { -1, -2, -1 };
            Weight[1] = new double[3] { 0, 0, 0 };
            Weight[2] = new double[3] { 1, 2, 1 };
            return Weight;
        }

        public static double[][] Edge_Vertical_Detection_Weight()
        {
            double[][] Weight = new double[3][];
            Weight[0] = new double[3] { -1, 0, 1 };
            Weight[1] = new double[3] { -2, 0, 2 };
            Weight[2] = new double[3] { -1, 0, 1 };
            return Weight;
        }

        public static double[][] Edge_45degree_Detection_Weight()
        {
            double[][] Weight = new double[3][];
            Weight[0] = new double[3] { 0, 1, 2 };
            Weight[1] = new double[3] { -1, 0, 1 };
            Weight[2] = new double[3] { -2, -1, 0 };
            return Weight;
        }

        public static double[][] Edge_225degree_Detection_Weight()
        {
            double[][] Weight = new double[3][];
            Weight[0] = new double[3] { -2, -1, 0 };
            Weight[1] = new double[3] { -1, 0, 1 };
            Weight[2] = new double[3] { 0, 1, 2 };
            return Weight;
        }

    }
}
