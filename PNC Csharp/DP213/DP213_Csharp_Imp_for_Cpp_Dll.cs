using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices; // (Dll 사용위해 필요)
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public class DP213_Csharp_Imp_for_Cpp_Dll 
    {

        public static void C_Sharp_DP213_Get_Intial_R_G_B_Using_3Points_Method(double Combine_Lv_Ratio, double[] Band_Voltage_AM1_R, double[] Band_Voltage_AM1_G, double[] Band_Voltage_AM1_B,
            ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[] Band_Gray_Gamma_Red, int[] Band_Gray_Gamma_Green, int[] Band_Gray_Gamma_Blue, double[] Band_Gray_Finally_Measured_Lv, int Current_Band_Dec_Vreg1,
            int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage, double Voltage_VREF4095, double Voltage_VREF0)
        {
            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                SJH_Matrix M = new SJH_Matrix();

                double[][] Band_Gray_Gamma_Red_Voltage = M.MatrixCreate(band, DP213_Static.Max_Gray_Amount);
                double[][] Band_Gray_Gamma_Green_Voltage = M.MatrixCreate(band, DP213_Static.Max_Gray_Amount);
                double[][] Band_Gray_Gamma_Blue_Voltage = M.MatrixCreate(band, DP213_Static.Max_Gray_Amount);

                for (int i = 0; i < band; i++)
                {
                    int[] Previous_Band_Gamma_R = new int[DP213_Static.Max_Gray_Amount];
                    int[] Previous_Band_Gamma_G = new int[DP213_Static.Max_Gray_Amount];
                    int[] Previous_Band_Gamma_B = new int[DP213_Static.Max_Gray_Amount];

                    for (int g = 0; g < DP213_Static.Max_Gray_Amount; g++)
                    {
                        Previous_Band_Gamma_R[g] = Band_Gray_Gamma_Red[(DP213_Static.Max_Gray_Amount * i) + g];
                        Previous_Band_Gamma_G[g] = Band_Gray_Gamma_Green[(DP213_Static.Max_Gray_Amount * i) + g];
                        Previous_Band_Gamma_B[g] = Band_Gray_Gamma_Blue[(DP213_Static.Max_Gray_Amount * i) + g];
                    }
                    //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
                    Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_R[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_R, Voltage_VREF4095, Voltage_VREF0);
                    Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_G[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_G, Voltage_VREF4095, Voltage_VREF0);
                    Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_B[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_B, Voltage_VREF4095, Voltage_VREF0);
                }


                //Need to...
                //Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance
                int Points_Num = band * DP213_Static.Max_Gray_Amount;
                double[] Gamma_Red_Voltage_Points = new double[Points_Num];
                double[] Gamma_Green_Voltage_Points = new double[Points_Num];
                double[] Gamma_Blue_Voltage_Points = new double[Points_Num];
                double[] Target_Lv_Points = new double[Points_Num];

                for (int b = 0; b < band; b++)
                {
                    for (int g = 0; g < DP213_Static.Max_Gray_Amount; g++)
                    {
                        Gamma_Red_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g] = Band_Gray_Gamma_Red_Voltage[b][g];
                        Gamma_Green_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g] = Band_Gray_Gamma_Green_Voltage[b][g];
                        Gamma_Blue_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g] = Band_Gray_Gamma_Blue_Voltage[b][g];
                        Target_Lv_Points[(DP213_Static.Max_Gray_Amount * b) + g] = Band_Gray_Finally_Measured_Lv[(DP213_Static.Max_Gray_Amount * b) + g];
                    }
                }

                Array.Sort(Gamma_Red_Voltage_Points);
                Array.Sort(Gamma_Green_Voltage_Points);
                Array.Sort(Gamma_Blue_Voltage_Points);
                Array.Sort(Target_Lv_Points);
                Array.Reverse(Target_Lv_Points);


                //-----------------Added On 200311-------------------
                List<double> Combinded_Gamma_Red_Voltage_Points = new List<double>();
                List<double> Combinded_Gamma_Green_Voltage_Points = new List<double>();
                List<double> Combinded_Gamma_Blue_Voltage_Points = new List<double>();
                List<double> Combinded_Target_Lv_Points = new List<double>();

                for (int b = 0; b < band; b++)
                {
                    for (int g = 0; g < DP213_Static.Max_Gray_Amount; g++)
                    {
                        if (b == 0 && g == 0)//First Point
                        {
                            Combinded_Gamma_Red_Voltage_Points.Add(Gamma_Red_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                            Combinded_Gamma_Green_Voltage_Points.Add(Gamma_Green_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                            Combinded_Gamma_Blue_Voltage_Points.Add(Gamma_Blue_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                            Combinded_Target_Lv_Points.Add(Target_Lv_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                        }
                        else
                        {
                            double Abs_Diff_Lv_Between_Two_Points = Math.Abs(Combinded_Target_Lv_Points[Combinded_Target_Lv_Points.Count - 1] - Target_Lv_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                            double Threshold_Lv = (Target_Lv_Points[(DP213_Static.Max_Gray_Amount * b) + g] * Combine_Lv_Ratio);

                            if (Abs_Diff_Lv_Between_Two_Points > Threshold_Lv)
                            {
                                Combinded_Gamma_Red_Voltage_Points.Add(Gamma_Red_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                                Combinded_Gamma_Green_Voltage_Points.Add(Gamma_Green_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                                Combinded_Gamma_Blue_Voltage_Points.Add(Gamma_Blue_Voltage_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                                Combinded_Target_Lv_Points.Add(Target_Lv_Points[(DP213_Static.Max_Gray_Amount * b) + g]);
                            }
                        }
                    }
                }
                //----------------------------------------------------
                //int Formula_Num = Points_Num - 2; //((band * 8) - 2 = Points_Num - 2) 
                int Formula_Num = Combinded_Target_Lv_Points.Count - 2; //Formula_Num = Points_Num - 2;

                double[][] Three_points_Gamma_Red_Voltage = M.MatrixCreate(Formula_Num, 3);
                double[][] Three_points_Gamma_Green_Voltage = M.MatrixCreate(Formula_Num, 3);
                double[][] Three_points_Gamma_Blue_Voltage = M.MatrixCreate(Formula_Num, 3);
                double[][] Three_points_Target_Lv = M.MatrixCreate(Formula_Num, 3);
                double[][] Three_points_C_R = M.MatrixCreate(Formula_Num, 3);
                double[][] Three_points_C_G = M.MatrixCreate(Formula_Num, 3);
                double[][] Three_points_C_B = M.MatrixCreate(Formula_Num, 3);

                double[][] Temp_A_R = M.MatrixCreate(3, 3);
                double[][] Temp_A_G = M.MatrixCreate(3, 3);
                double[][] Temp_A_B = M.MatrixCreate(3, 3);
                double[][] Temp_Inv_A_R = M.MatrixCreate(3, 3);
                double[][] Temp_Inv_A_G = M.MatrixCreate(3, 3);
                double[][] Temp_Inv_A_B = M.MatrixCreate(3, 3);

                double[] C_R = new double[3];
                double[] C_G = new double[3];
                double[] C_B = new double[3];

                int count = 0;
                for (int k = 0; k < Formula_Num; k++)
                {
                    for (int i = 0; i <= 2; i++)
                    {
                        //Get "Three_points_Target_Lv"
                        Three_points_Target_Lv[k][i] = Combinded_Target_Lv_Points[i + k];
                        count = 0;
                        //Get "A"
                        for (int j = 2; j >= 0; j--)
                        {
                            Temp_A_R[i][count] = Math.Pow(Combinded_Gamma_Red_Voltage_Points[i + k], j);
                            Temp_A_G[i][count] = Math.Pow(Combinded_Gamma_Green_Voltage_Points[i + k], j);
                            Temp_A_B[i][count] = Math.Pow(Combinded_Gamma_Blue_Voltage_Points[i + k], j);
                            count++;
                        }
                    }

                    //Get Inv_A (by using "A")
                    Temp_Inv_A_R = M.MatrixInverse(Temp_A_R);
                    Temp_Inv_A_G = M.MatrixInverse(Temp_A_G);
                    Temp_Inv_A_B = M.MatrixInverse(Temp_A_B);

                    //Get C (by using "Inv_A" , "Three_points_Target_Lv")
                    Three_points_C_R[k] = M.Matrix_Multiply(Temp_Inv_A_R, Three_points_Target_Lv[k]);
                    Three_points_C_G[k] = M.Matrix_Multiply(Temp_Inv_A_G, Three_points_Target_Lv[k]);
                    Three_points_C_B[k] = M.Matrix_Multiply(Temp_Inv_A_B, Three_points_Target_Lv[k]);

                    //Get C (Target belongs to [Three_points_Target_Lv[k][2],Three_points_Target_Lv[k][0]])
                    if (Three_points_Target_Lv[k][2] < Target_Lv && Target_Lv < Three_points_Target_Lv[k][0])
                    {
                        for (int i = 0; i <= 2; i++)
                        {
                            C_R[i] = Three_points_C_R[k][i];
                            C_G[i] = Three_points_C_G[k][i];
                            C_B[i] = Three_points_C_B[k][i];
                        }
                        break; //Break for k loop
                    }
                }

                //Get Calculated Voltage_R/G/B
                double Calculated_Target_Lv;
                int iteration;
                double Calculated_R_Vdata = 0;
                double Calculated_G_Vdata = 0;
                double Calculated_B_Vdata = 0;

                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;

                    for (int j = 2; j >= 0; j--)
                        Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0)
                    {
                        Calculated_R_Vdata = Vdata;
                        break;
                    }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;

                    for (int j = 2; j >= 0; j--)
                        Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0)
                    {
                        Calculated_G_Vdata = Vdata;
                        break;
                    }
                }

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;

                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0)
                    {
                        Calculated_B_Vdata = Vdata;
                        break;
                    }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(Voltage_VREF4095, Voltage_VREF0, Band_Vreg1_Dec[band]);
                Gamma_R = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_R[band], Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata, gray);
                Gamma_G = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_G[band], Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata, gray);
                Gamma_B = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_B[band], Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata, gray);

                int Prev_Band_Current_Gray_Gamma_R = Band_Gray_Gamma_Red[(DP213_Static.Max_Gray_Amount * (band - 1)) + gray];
                int Prev_Band_Current_Gray_Gamma_G = Band_Gray_Gamma_Green[(DP213_Static.Max_Gray_Amount * (band - 1)) + gray];
                int Prev_Band_Current_Gray_Gamma_B = Band_Gray_Gamma_Blue[(DP213_Static.Max_Gray_Amount * (band - 1)) + gray];
                DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(ref Gamma_R, ref Gamma_G, ref Gamma_B, Prev_Band_Current_Gray_Gamma_R, Prev_Band_Current_Gray_Gamma_G, Prev_Band_Current_Gray_Gamma_B);
            }
            else //Band0 + Other not selected Bands's
            {

            }
        }



        public static void C_Sharp_DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(int Vreg1, double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band,
            int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (band >= 1 && Selected_Band[band] == true)
            {
                SJH_Matrix M = new SJH_Matrix();

                double[] Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_R, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREF4095, Voltage_VREF0);
                double[] Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_G, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREF4095, Voltage_VREF0);
                double[] Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_B, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREF4095, Voltage_VREF0);

                double[][] A_R = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[][] A_G = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[][] A_B = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);

                //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= (DP213_Static.Max_Gray_Amount - 1); i++)
                {
                    count = 0;
                    for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                    {
                        A_R[i][count] = Math.Pow(Previous_Band_Gamma_Red_Voltage[i], j);
                        A_G[i][count] = Math.Pow(Previous_Band_Gamma_Green_Voltage[i], j);
                        A_B[i][count] = Math.Pow(Previous_Band_Gamma_Blue_Voltage[i], j);
                        count++;
                    }
                }

                //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
                double[][] Inv_A_R = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[][] Inv_A_G = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[][] Inv_A_B = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[] C_R = new double[DP213_Static.Max_Gray_Amount];
                double[] C_G = new double[DP213_Static.Max_Gray_Amount];
                double[] C_B = new double[DP213_Static.Max_Gray_Amount];
                Inv_A_R = M.MatrixInverse(A_R);
                Inv_A_G = M.MatrixInverse(A_G);
                Inv_A_B = M.MatrixInverse(A_B);
                C_R = M.Matrix_Multiply(Inv_A_R, Previous_Band_Finally_Measured_Lv);
                C_G = M.Matrix_Multiply(Inv_A_G, Previous_Band_Finally_Measured_Lv);
                C_B = M.Matrix_Multiply(Inv_A_B, Previous_Band_Finally_Measured_Lv);

                //Show "C10*(Vdata^10) + C9*(Vdata^9) + C8*(Vdata^8) + C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;

                double Calculated_Target_Lv = 0;
                int iteration = 0;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                double Previous_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(Voltage_VREF4095, Voltage_VREF0, Previous_Band_Vreg1_Dec);
                double Actual_Previous_Vdata_Red = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);

                //Red
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;

                    for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                        Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0)
                    {
                        Calculated_Vdata_Red = Vdata;
                        break;
                    }
                }

                //Green
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;

                    for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                        Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0)
                    {
                        Calculated_Vdata_Green = Vdata;
                        break;
                    }
                }

                //Blue
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;

                    for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                        Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
                    {
                        Calculated_Vdata_Blue = Vdata;
                        break;
                    }
                }

                double Vreg1_voltage = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(Voltage_VREF4095, Voltage_VREF0, Vreg1);
                //Got the Vreg1 
                //Need to get Gamma_R/B
                Gamma_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_G = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Green);
                Gamma_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Blue);

                DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(ref Gamma_R, ref Gamma_G, ref Gamma_B, Previous_Band_Gamma_Red[0], Previous_Band_Gamma_Green[0], Previous_Band_Gamma_Blue[0]);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }

        public static void C_Sharp_DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B,
        bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (band >= 1 && Selected_Band[band] == true)
            {
                SJH_Matrix M = new SJH_Matrix();
                double[] Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_R, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREF4095, Voltage_VREF0);
                double[] Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_G, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREF4095, Voltage_VREF0);
                double[] Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_B, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREF4095, Voltage_VREF0);
                
                double[][] A_R = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[][] A_G = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[][] A_B = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);

                //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= (DP213_Static.Max_Gray_Amount - 1); i++)
                {
                    count = 0;
                    for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                    {
                        A_R[i][count] = Math.Pow(Previous_Band_Gamma_Red_Voltage[i], j);
                        A_G[i][count] = Math.Pow(Previous_Band_Gamma_Green_Voltage[i], j);
                        A_B[i][count] = Math.Pow(Previous_Band_Gamma_Blue_Voltage[i], j);
                        count++;
                    }
                }

                //Get C[DP213_Static.Max_Gray_Amount] = inv(A)[DP213_Static.Max_Gray_Amount,DP213_Static.Max_Gray_Amount] * Previous_Band_Target_Lv[DP213_Static.Max_Gray_Amount]
                double[][] Inv_A_R = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[][] Inv_A_G = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[][] Inv_A_B = M.MatrixCreate(DP213_Static.Max_Gray_Amount, DP213_Static.Max_Gray_Amount);
                double[] C_R = new double[DP213_Static.Max_Gray_Amount];
                double[] C_G = new double[DP213_Static.Max_Gray_Amount];
                double[] C_B = new double[DP213_Static.Max_Gray_Amount];
                Inv_A_R = M.MatrixInverse(A_R);
                Inv_A_G = M.MatrixInverse(A_G);
                Inv_A_B = M.MatrixInverse(A_B);
                C_R = M.Matrix_Multiply(Inv_A_R, Previous_Band_Finally_Measured_Lv);
                C_G = M.Matrix_Multiply(Inv_A_G, Previous_Band_Finally_Measured_Lv);
                C_B = M.Matrix_Multiply(Inv_A_B, Previous_Band_Finally_Measured_Lv);

                //Show "C10*(Vdata^10) + C9*(Vdata^9) + C8*(Vdata^8) + C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;
                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                double Previous_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(Voltage_VREF4095, Voltage_VREF0, Previous_Band_Vreg1_Dec);
                double Actual_Previous_Vdata_Red = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);

                //Red
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                    {
                        Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    }
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0)
                    {
                        Calculated_Vdata_Red = Vdata;
                        break;
                    }
                }

                //Green
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                    {
                        Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    }

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0)
                    {
                        Calculated_Vdata_Green = Vdata;
                        break;
                    }
                }

                //Blue
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= Voltage_VREF4095; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = (DP213_Static.Max_Gray_Amount - 1); j >= 0; j--)
                    {
                        Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    }
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
                    {
                        Calculated_Vdata_Blue = Vdata;
                        break;
                    }
                }

                f1.GB_Status_AppendText_Nextline("RBVreg1 Algorithm)Calculated_Vdata_Red/Calculated_Vdata_Green/Calculated_Vdata_Blue : "
                    + Calculated_Vdata_Red.ToString() + "/" + Calculated_Vdata_Green.ToString() + "/" + Calculated_Vdata_Blue.ToString(), Color.Blue);

                double Calculated_Vreg1_voltage = Voltage_VREF4095 + ((Calculated_Vdata_Green - Voltage_VREF4095) * (900.0 / (Previous_Band_Gamma_Green[0] + 389.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                Vreg1_Dec_Init = Imported_my_cpp_dll.DP213_Get_Vreg1_Dec(Voltage_VREF4095, Voltage_VREF0, Calculated_Vreg1_voltage);

                f1.GB_Status_AppendText_Nextline("RBVreg1 Algorithm)Calculated_Vreg1_voltage : " + Calculated_Vreg1_voltage.ToString(), Color.Blue);
                
                //Got the Vreg1 
                //Need to get Gamma_R/B
                Gamma_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Calculated_Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Calculated_Vreg1_voltage, Calculated_Vdata_Blue);

                DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(ref Gamma_R, ref Previous_Band_Gamma_Blue[0], ref Gamma_B, Previous_Band_Gamma_Red[0], Previous_Band_Gamma_Green[0], Previous_Band_Gamma_Blue[0]);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }

        private static double[] Get_Previous_Band_Gamma_Voltage(double Prev_Band_AM1_Voltage, int Previous_Band_Vreg1_Dec, int[] Previous_Band_Gamma, double Voltage_VREF4095, double Voltage_VREF0)
        {
            double[] Previous_Band_Gamma_Voltage = new double[DP213_Static.Max_Gray_Amount];
            double Previous_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(Voltage_VREF4095, Voltage_VREF0, Previous_Band_Vreg1_Dec);

            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
            {
                if (gray == 0)
                    Previous_Band_Gamma_Voltage[gray] = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma[gray]);

                else if (gray == 1 || gray == 2 || gray == 3 || gray == 5 || gray == 7 || gray == 9 || gray == 10)
                    Previous_Band_Gamma_Voltage[gray] = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(Prev_Band_AM1_Voltage, Previous_Band_Gamma_Voltage[gray - 1], Previous_Band_Gamma[gray], gray);

                else if (gray == 4 || gray == 6 || gray == 8)
                    Previous_Band_Gamma_Voltage[gray] = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(Prev_Band_AM1_Voltage, Previous_Band_Gamma_Voltage[gray - 2], Previous_Band_Gamma[gray], gray);
            }
            return Previous_Band_Gamma_Voltage;
        }

        private static void DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, int Prev_Band_Gray255_Gamma_R, int Prev_Band_Gray255_Gamma_G, int Prev_Band_Gray255_Gamma_B)
        {
            if (Gamma_R >= 500 || Gamma_R <= 10 || Gamma_G >= 500 || Gamma_G <= 10 || Gamma_B >= 500 || Gamma_B <= 10)
            {
                Gamma_R = Prev_Band_Gray255_Gamma_R;
                Gamma_G = Prev_Band_Gray255_Gamma_G;
                Gamma_B = Prev_Band_Gray255_Gamma_B;
            }
        }
    }
}
