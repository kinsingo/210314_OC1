using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSQH_Csharp_Library
{
    public struct RGB_Double
    {
        public string R;
        public string G;
        public string B;
        public string Vreg1;

        public double double_R;
        public double double_G;
        public double double_B;
        public double double_Vreg1;

        public static RGB_Double operator +(RGB_Double A, double B)
        {
            RGB_Double C = new RGB_Double();

            // C = A + B
            C.double_R = A.double_R + B;
            C.double_G = A.double_G + B;
            C.double_B = A.double_B + B;
            C.double_Vreg1 = A.double_Vreg1 + B;

            //Update String
            C.Update_String_From_Double();

            return C;
        }

        public RGB_Double Set_Value(double R, double G, double B, double Vreg1 = 1000)
        {
            //This = A
            this.double_R = R;
            this.double_G = G;
            this.double_B = B;
            this.double_Vreg1 = Vreg1;

            //Update String
            this.Update_String_From_Double();

            return this;
        }

        public static bool operator >(RGB_Double A, RGB_Double B)
        {
            if ((A.double_R > B.double_R) && (A.double_G > B.double_G) && (A.double_B > B.double_B))
                return true;
            else
                return false;
        }

        public static bool operator <(RGB_Double A, RGB_Double B)
        {
            if ((A.double_R < B.double_R) && (A.double_G < B.double_G) && (A.double_B < B.double_B))
                return true;
            else
                return false;
        }

        public static bool operator >=(RGB_Double A, RGB_Double B)
        {
            if ((A.double_R >= B.double_R) && (A.double_G >= B.double_G) && (A.double_B >= B.double_B))
                return true;
            else
                return false;
        }

        public static bool operator <=(RGB_Double A, RGB_Double B)
        {
            if ((A.double_R <= B.double_R) && (A.double_G <= B.double_G) && (A.double_B <= B.double_B))
                return true;
            else
                return false;
        }




        public RGB_Double(double value = 0, string str = "")
        {
            R = G = B = Vreg1 = str;
            double_R = double_G = double_B = double_Vreg1 = value;
        }

        public void Update_String_From_Double()
        {
            R = double_R.ToString();
            G = double_G.ToString();
            B = double_B.ToString();
            Vreg1 = double_Vreg1.ToString();
        }
    }
}
