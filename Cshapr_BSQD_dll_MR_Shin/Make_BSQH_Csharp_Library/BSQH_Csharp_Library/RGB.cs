using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSQH_Csharp_Library
{
    public struct RGB
    {
        public string R;
        public string G;
        public string B;
        public string Vreg1;//190527

        public int int_R;
        public int int_G;
        public int int_B;
        public int int_Vreg1;//190527

        public string Binary_R;
        public string Binary_G;
        public string Binary_B;
        public string Binary_Vreg1;//190527

        public RGB(int value = 0, string str = "")
        {
            R = G = B = Vreg1 = str;
            Binary_R = Binary_G = Binary_B = Binary_Vreg1 = str;
            int_R = int_G = int_B = int_Vreg1 = value;
        }

        public bool Is_RGB_Equal(RGB Compared)
        {
            if ((this.int_R == Compared.int_R)
                && (this.int_G == Compared.int_G)
                && (this.int_B == Compared.int_B))
                return true;
            else
                return false;
        }

        public static RGB operator -(RGB A, RGB B)
        {
            RGB C = new RGB();

            // C = A - B
            C.int_R = A.int_R - B.int_R;
            C.int_G = A.int_G - B.int_G;
            C.int_B = A.int_B - B.int_B;
            C.int_Vreg1 = A.int_Vreg1 - B.int_Vreg1;

            //Update String
            C.String_Update_From_int();

            return C;
        }

        public static RGB operator +(RGB A, RGB B)
        {
            RGB C = new RGB();

            // C = A + B
            C.int_R = A.int_R + B.int_R;
            C.int_G = A.int_G + B.int_G;
            C.int_B = A.int_B + B.int_B;
            C.int_Vreg1 = A.int_Vreg1 + B.int_Vreg1;

            //Update String
            C.String_Update_From_int();

            return C;
        }


        public static RGB GetAverage(RGB A, RGB B)
        {
            RGB C = new RGB();

            // C = A + B
            C.int_R = (A.int_R + B.int_R)/2;
            C.int_G = (A.int_G + B.int_G)/2;
            C.int_B = (A.int_B + B.int_B)/2;
            C.int_Vreg1 = (A.int_Vreg1 + B.int_Vreg1)/2;

            //Update String
            C.String_Update_From_int();

            return C;
        }

        public static bool operator >(RGB A, RGB B)
        {
            if ((A.int_R > B.int_R) && (A.int_G > B.int_G) && (A.int_B > B.int_B))
                return true;
            else
                return false;
        }

        public static bool operator <(RGB A, RGB B)
        {
            if ((A.int_R < B.int_R) && (A.int_G < B.int_G) && (A.int_B < B.int_B))
                return true;
            else
                return false;
        }

        public static bool operator >=(RGB A, RGB B)
        {
            if ((A.int_R >= B.int_R) && (A.int_G >= B.int_G) && (A.int_B >= B.int_B))
                return true;
            else
                return false;
        }

        public static bool operator <=(RGB A, RGB B)
        {
            if ((A.int_R <= B.int_R) && (A.int_G <= B.int_G) && (A.int_B <= B.int_B))
                return true;
            else
                return false;
        }

        public RGB Set_Value(int R, int G, int B, int Vreg1 = 1000)
        {
            //This = A
            this.int_R = R;
            this.int_G = G;
            this.int_B = B;
            this.int_Vreg1 = Vreg1;

            //Update String
            this.String_Update_From_int();

            return this;
        }

        public RGB Equal_Value(RGB A)
        {
            //This = A
            this.int_R = A.int_R;
            this.int_G = A.int_G;
            this.int_B = A.int_B;
            this.int_Vreg1 = A.int_Vreg1;

            //Update String
            this.String_Update_From_int();

            return this;
        }

        public void String_Update_From_int()
        {
            R = int_R.ToString();
            G = int_G.ToString();
            B = int_B.ToString();
            Vreg1 = int_Vreg1.ToString();
        }

        public void Binary_String_Update_From_Int()
        {
            Binary_R = Convert.ToString(int_R, 2);
            Binary_G = Convert.ToString(int_G, 2);
            Binary_B = Convert.ToString(int_B, 2);
            Binary_Vreg1 = Convert.ToString(int_Vreg1, 2);
        }

        public void Hex_String_Update_From_Int()
        {
            Binary_R = Convert.ToString(int_R, 16);
            Binary_G = Convert.ToString(int_G, 16);
            Binary_B = Convert.ToString(int_B, 16);
            Binary_Vreg1 = Convert.ToString(int_Vreg1, 16);
        }

        public void Hex_String_Update_From_Int(int Left_Zero_Padding_amount = 2)
        {
            Binary_R = int_R.ToString("X" + Left_Zero_Padding_amount.ToString());
            Binary_G = int_G.ToString("X" + Left_Zero_Padding_amount.ToString());
            Binary_B = int_B.ToString("X" + Left_Zero_Padding_amount.ToString());
            Binary_Vreg1 = int_Vreg1.ToString("X" + Left_Zero_Padding_amount.ToString());
        }

        public void Binary_String_Update_From_Int(int padleft_amount, char padding_char)
        {
            Binary_R = Convert.ToString(int_R, 2).PadLeft(padleft_amount, padding_char);
            Binary_G = Convert.ToString(int_G, 2).PadLeft(padleft_amount, padding_char);
            Binary_B = Convert.ToString(int_B, 2).PadLeft(padleft_amount, padding_char);
            Binary_Vreg1 = Convert.ToString(int_Vreg1, 2).PadLeft(padleft_amount, padding_char);
        }

        public void Int_Update_From_String()
        {
            int_R = Convert.ToInt16(R);
            int_G = Convert.ToInt16(G);
            int_B = Convert.ToInt16(B);
            int_Vreg1 = Convert.ToInt16(Vreg1);
        }
    }
}
