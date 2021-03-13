using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSQH_Csharp_Library
{
    

    public struct XYLv
    {
        public string X;
        public string Y;
        public string Lv;

        public double double_X;
        public double double_Y;
        public double double_Lv;

        
        public XYLv(double _double_X = 0.0, double _double_Y = 0.0, double _double_Lv = 0.0)
        {
            double_X = _double_X;
            double_Y = _double_Y;
            double_Lv = _double_Lv;
            X = double_X.ToString();
            Y = double_Y.ToString();
            Lv = double_Lv.ToString();
        }


        public XYLv Change_To_Absolute_Values()
        {
            double_X = Math.Abs(double_X);
            double_Y = Math.Abs(double_Y);
            double_Lv = Math.Abs(double_Lv);
            String_Update_From_Double();

            return this;
        }


        public static XYLv operator -(XYLv A, XYLv B)
        {
            XYLv C = new XYLv();

            // C = A - B
            C.double_X = A.double_X - B.double_X;
            C.double_Y = A.double_Y - B.double_Y;
            C.double_Lv = A.double_Lv - B.double_Lv;

            //Update String
            C.String_Update_From_Double();

            return C;
        }

        public static XYLv operator *(XYLv A, double ratio)
        {
            XYLv C = new XYLv();

            // C = A - B
            C.double_X = A.double_X * ratio;
            C.double_Y = A.double_Y * ratio;
            C.double_Lv = A.double_Lv * ratio;

            //Update String
            C.String_Update_From_Double();

            return C;
        }


        public XYLv Equal_Value(XYLv A)
        {
            //This = A
            this.double_X = A.double_X;
            this.double_Y = A.double_Y;
            this.double_Lv = A.double_Lv;

            //Update String
            this.String_Update_From_Double();

            return this;
        }

        public XYLv Set_Value(double X, double Y, double Lv)
        {
            //This = A
            this.double_X = X;
            this.double_Y = Y;
            this.double_Lv = Lv;
            //Update String
            this.String_Update_From_Double();

            return this;
        }

        public void String_Update_From_Double()
        {
            X = double_X.ToString();
            Y = double_Y.ToString();
            Lv = double_Lv.ToString();
        }

        public void Double_Update_From_String()
        {
            double_X = Convert.ToDouble(X);
            double_Y = Convert.ToDouble(Y);
            double_Lv = Convert.ToDouble(Lv);
        }
    }
}
