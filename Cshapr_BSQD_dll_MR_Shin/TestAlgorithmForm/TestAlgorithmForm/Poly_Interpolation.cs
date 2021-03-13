using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BSQH_Csharp_Library;

namespace TestAlgorithmForm
{
    public class Poly_Interpolation
    {
        double[] C;
        public void Update_Function_Param(int[] grays,double[] Voltages)
        {

            SJH_Matrix M = new SJH_Matrix();

            double[][] A = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
            
            for (int i = 0; i <= (DP213_Static.Max_Gray_Amount - 1); i++)
            {
                int count = 0;
                for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                {
                    A[i][count++] = Math.Pow(Voltages[i], j);
                }
            }

            double[] DoubleGrays = new double[grays.Length]; 
            for(int i = 0;i < grays.Length;i++)
                DoubleGrays[i] = Convert.ToDouble(grays[i]);

            C = M.Matrix_Multiply(M.MatrixInverse(A), DoubleGrays);
        }

        public void Update_Function_Param2(int[] grays, double[] Voltages)
        {

            SJH_Matrix M = new SJH_Matrix();

            double[][] A = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);

            for (int i = 0; i <= (DP213_Static.Max_Gray_Amount - 1); i++)
            {
                int count = 0;
                for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                {
                    //A[i][count] = Math.Pow(Voltages[i], j);
                    A[i][count++] = Math.Pow(grays[i], j);
                }
            }

            //C = M.Matrix_Multiply(M.MatrixInverse(A), DoubleGrays);
            C = M.Matrix_Multiply(M.MatrixInverse(A), Voltages);
        }

        public int Polynorminal_Fuction(double Voltage)
        {
            if (C == null)
                throw new Exception("Please Update C[] first");

            double gray = 0;

            int iteration = 0;
            for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                gray += (C[iteration++] * Math.Pow(Voltage, j));
            

            //System.Windows.Forms.MessageBox.Show("gray/Voltage/iteration : " + gray + "/" + Voltage + "/" + iteration);

            return Convert.ToInt32(gray);
        }

        public double Polynorminal_Fuction2(int gray)
        {
            if (C == null)
                throw new Exception("Please Update C[] first");

            double volt = 0;

            int iteration = 0;
            for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                volt += (C[iteration++] * Math.Pow(gray, j));
            

           // System.Windows.Forms.MessageBox.Show("gray/Voltage/iteration : " + gray + "/" + volt + "/" + iteration);

            return volt;
        }


    }
}
