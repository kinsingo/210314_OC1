using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices; // (Dll 사용위해 필요)

namespace BSQH_Csharp_Library
{
  
    public class Imported_my_cpp_dll
    {
        //---------------DP213-------------


        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Band0_REF0_Gray255_Compensation(int loop_count, ref int Gamma_R,
    ref int Gamma_G, ref int Gamma_B, ref int REF0, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
    , double Limit_X, double Limit_Y, double Limit_Lv, int Gamma_Register_Limit, int REF0_Register_Limit, ref bool Gamma_Or_REF0_Out_Of_Register_Limit, ref bool Within_Spec_Limit);

        //Calculating Initial RGBVreg1 Algorithm (Through Monotone-Cubic-Interpolation)
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method_Through_MCI(double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B,
[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
    , double[] Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method_Through_MCI(int Vreg1, double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DP213_Get_Intial_R_G_B_Using_3Points_Method_Through_MCI(double Combine_Lv_Ratio, double[] Band_Voltage_AM1_R, double[] Band_Voltage_AM1_G, double[] Band_Voltage_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Band_Gray_Gamma_Red, int[] Band_Gray_Gamma_Green, int[] Band_Gray_Gamma_Blue, double[] Band_Gray_Finally_Measured_Lv,
            int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage, double Voltage_VREF4095, double Voltage_VREF0);

        //Calculating Initial RGBVreg1 Algorithm (Through Poly-Interpolation)
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(int Vreg1, double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DP213_Get_Intial_R_G_B_Using_3Points_Method(double Combine_Lv_Ratio, double[] Band_Voltage_AM1_R, double[] Band_Voltage_AM1_G, double[] Band_Voltage_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Band_Gray_Gamma_Red, int[] Band_Gray_Gamma_Green, int[] Band_Gray_Gamma_Blue, double[] Band_Gray_Finally_Measured_Lv,
            int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage, double Voltage_VREF4095, double Voltage_VREF0);

        //ELVSS & Vinit2
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_ELVSS_Dec_to_Voltage(int ELVSS_Dec);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_ELVSS_Voltage_to_Dec(double ELVSS_Voltage);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_VINI2_Dec_to_Voltage(int VINIT2_Dec);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_VINI2_Voltage_to_Dec(double VINIT2_Voltage);

        //VREF0
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_VREF0_Dec_to_Voltage(int VREF0_Dec);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_VREF0_Voltage_to_Dec(double VREF0_Voltage);

        //VREF4095
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_VREF4095_Dec_to_Voltage(int VREF4095_Dec);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_VREF4095_Voltage_to_Dec(double VREF4095_Voltage);

        //Get Vreg1/GR/AM2/AM1/AM0 Voltage From Gamma
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_Get_Vreg1_Voltage(double Voltage_VREF4095, double Voltage_VREF0, int Dec_Vreg1);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_Get_GR_Gamma_Voltage(double AM1_Voltage, double Prev_GR_Gamma_Voltage, int Gamma_Dec, int gray);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_Get_AM2_Gamma_Voltage(double Voltage_VREF4095, double Vreg1_Voltage, int Dec_AM2);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_Get_AM1_RGB_Voltage(double Voltage_VREF4095, double Vreg1_voltage, int Dec_AM1);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP213_Get_AM0_RGB_Voltage(double Voltage_VREF4095, double Vreg1_voltage, int Dec_AM0);

        //Get Vreg1/GR/AM2/AM1/AM0 Gamma From Voltage
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_Get_Vreg1_Dec(double Voltage_VREF4095, double Voltage_VREF0, double Vreg1_Voltage);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_Get_GR_Gamma_Dec(double AM1_Voltage, double Prev_GR_Gamma_Voltage, double Gamma_Voltage, int gray);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_Get_AM2_Gamma_Dec(double Voltage_VREF4095, double Vreg1_voltage, double AM2_voltage);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_Get_AM1_RGB_Dec(double Voltage_VREF4095, double Vreg1_voltage, double AM1_Voltage);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP213_Get_AM0_RGB_Dec(double Voltage_VREF4095, double Vreg1_voltage, double AM0_Voltage);

        //Get AM0 Resolution
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_DP213_EA9155_AM0_Resolution(int Dec_REF0, int Dev_REF4095);


        //---- Common Function ----- 
        //Get Dll Info
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Get_Dll_Information();


        //Calculate and Get Initial R/G/B (Gray191~Gray4, Ver2)
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Get_Initial_Gamma_Fx_3points_Combine_Points_2(double Combine_Lv_Ratio, int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Band_Gray_Gamma_Red, int[] Band_Gray_Gamma_Green, int[] Band_Gray_Gamma_Blue, double[] Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
                 int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
                 double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1
                 , double Fx_3points_Combine_LV_1, double Fx_3points_Combine_LV_2, double Fx_3points_Combine_LV_3, double Fx_3points_Combine_Lv_Distance);



        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Sub_Compensation(int loop_count, bool Infinite_Loop, ref int Infinite_Loop_Count, ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);



        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Sub_Compensation_Upper_Lv_Limit(int loop_count, bool Infinite_Loop, ref int Infinite_Loop_Count, ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);


        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Sub_Compensation_Lower_Lv_Limit(int loop_count, bool Infinite_Loop, ref int Infinite_Loop_Count, ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
      , double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);












        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init_Algorithm_Applied_Sub_Compensation(int loop_count, bool Infinite_Loop, ref int Infinite_Loop_Count, ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init_Algorithm_Applied_Sub_Compensation_Upper_Lv_Limit(int loop_count, bool Infinite_Loop, ref int Infinite_Loop_Count, ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);


        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init_Algorithm_Applied_Sub_Compensation_Lower_Lv_Limit(int loop_count, bool Infinite_Loop, ref int Infinite_Loop_Count, ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
 , double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);










        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Vreg1_Compensation(int loop_count, bool Vreg1_Infinite_Loop, int Vreg1_Infinite_Loop_Count, ref int Gamma_R, ref int Vreg1, ref int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
        , double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, int Vreg1_Register_Limit, ref bool Gamma_Or_Vreg1_Out_Of_Register_Limit, ref bool Within_Spec_Limit);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Vreg1_Compensation_Upper_Lv_Limit(int loop_count, bool Vreg1_Infinite_Loop, int Vreg1_Infinite_Loop_Count, ref int Gamma_R, ref int Vreg1, ref int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, int Vreg1_Register_Limit, ref bool Gamma_Or_Vreg1_Out_Of_Register_Limit, ref bool Within_Spec_Limit);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Vreg1_Compensation_Lower_Lv_Limit(int loop_count, bool Vreg1_Infinite_Loop, int Vreg1_Infinite_Loop_Count, ref int Gamma_R, ref int Vreg1, ref int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
        , double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, int Vreg1_Register_Limit, ref bool Gamma_Or_Vreg1_Out_Of_Register_Limit, ref bool Within_Spec_Limit);







        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init_Algorithm_Applied_Vreg1_Compensation(int loop_count, bool Vreg1_Infinite_Loop, int Vreg1_Infinite_Loop_Count, ref int Gamma_R, ref int Vreg1, ref int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, int Vreg1_Register_Limit, ref bool Gamma_Or_Vreg1_Out_Of_Register_Limit, ref bool Within_Spec_Limit);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init_Algorithm_Applied_Vreg1_Compensation_Upper_Lv_Limit(int loop_count, bool Vreg1_Infinite_Loop, int Vreg1_Infinite_Loop_Count, ref int Gamma_R, ref int Vreg1, ref int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, int Vreg1_Register_Limit, ref bool Gamma_Or_Vreg1_Out_Of_Register_Limit, ref bool Within_Spec_Limit);
        
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init_Algorithm_Applied_Vreg1_Compensation_Lower_Lv_Limit(int loop_count, bool Vreg1_Infinite_Loop, int Vreg1_Infinite_Loop_Count, ref int Gamma_R, ref int Vreg1, ref int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, int Vreg1_Register_Limit, ref bool Gamma_Or_Vreg1_Out_Of_Register_Limit, ref bool Within_Spec_Limit);






        //----- DP086 & DP116 & DP150 -----
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ELVSS_Compensation(ref bool ELVSS_Find_Finish, double First_ELVSS, ref double ELVSS, ref double Vinit, ref double First_Slope, double Vinit_Margin, double ELVSS_Margin
    , double Slope_Margin, double First_Measure_X, double First_Measure_Y, double First_Measure_Lv, double Measure_X, double Measure_Y, double Measure_Lv);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RGB_Gamma_Initial_Values(int Index_Count, int Min_Value, int Max_Value
            , int[] Gamma_R, int[] Gamma_G, int[] Gamma_B, ref int Out_R, ref int Out_G, ref int Out_B);

        [DllImport("BSQH_dll_MR_Shin.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Vreg1_Initial_Values(int Index_Count, int[] Vreg1);

        //------ DP173 & Elgin -----
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ELVSS_Compensation_For_DP173(double ELVSS_Min, double ELVSS_Max, ref bool ELVSS_Find_Finish, double First_ELVSS, ref double ELVSS, ref double First_Slope, double ELVSS_Margin, double Slope_Margin, double First_Measure_X, double First_Measure_Y, double First_Measure_Lv, double Measure_X, double Measure_Y, double Measure_Lv);

        //Get AM0 Resolution
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_DP173_EA9154_AM0_Resolution(string Hex_VREG1_REF1, string Hex_VREG1_REF2047);

        //Get REF Voltages (Int --> Voltages)
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Get_REF_Voltages(int Dec_VREG1_REF2047, int Dec_VREG1_REF1635, int Dec_VREG1_REF1227, int Dec_VREG1_REF815, int Dec_VREG1_REF407, int Dec_VREG1_REF63, int Dec_VREG1_REF1, ref double Voltage_VREG1_REF2047, ref double Voltage_VREG1_REF1635, ref double Voltage_VREG1_REF1227, ref double Voltage_VREG1_REF815, ref double Voltage_VREG1_REF407, ref double Voltage_VREG1_REF63, ref double Voltage_VREG1_REF1);

        //Get R/G/B/Vreg1 Voltages (Int --> Voltages)
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP173_Get_Vreg1_Voltage(int Dec_Vreg1, double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP173_Get_GR_Gamma_Voltage(double Voltage_VREG1_REF2047, double Vreg1_voltage, int Dec_AM1, double Prev_GR_Gamma_Voltage, int Gamma_Dec, int gray);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP173_Get_AM2_Gamma_Voltage(double Voltage_VREG1_REF2047, double Vreg1_Voltage, int Gamma_Dec);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP173_Get_AM1_RGB_Voltage(double Voltage_VREG1_REF2047, double Vreg1_voltage, int Dec_AM1);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double DP173_Get_AM0_RGB_Voltage(double Voltage_VREG1_REF2047, double Vreg1_voltage, int Dec_AM0);

        //Get R/G/B/Vreg1 Decs (Voltages -- > Int)
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP173_Get_Vreg1_Dec(double Vreg1_Voltage, double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP173_Get_GR_Gamma_Dec(double Voltage_VREG1_REF2047, double Vreg1_voltage, int Dec_AM1, double Prev_GR_Gamma_Voltage, double Gamma_Voltage, int gray);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP173_Get_AM2_Gamma_Dec(double Voltage_VREG1_REF2047, double Vreg1_voltage, double AM2_Gamma_voltage);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP173_Get_AM1_RGB_Dec(double Voltage_VREG1_REF2047, double Vreg1_voltage, double AM1_Voltage);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DP173_Get_AM0_RGB_Dec(double Voltage_VREG1_REF2047, double Vreg1_voltage, double AM0_Voltage);

        //Calculate and Get Initial R/Vreg1/B (Gray255)
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DP173_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Target_Lv, double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1);

        //Calculate and Get Initial R/G/B (Gray255)
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DP173_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(int Vreg1, int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
                , double[] Previous_Band_Target_Lv, double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1);

        //Test Dll
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CCT_Sub_Compensation(int loop_count, bool Infinite_Loop, int Infinite_Loop_Count, ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);
        //Test Dll
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CCT_Vreg1_Compensation(int loop_count, bool Vreg1_Infinite_Loop, int Vreg1_Infinite_Loop_Count, ref int Gamma_R, ref int Vreg1, ref int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
        , double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, int Vreg1_Register_Limit, ref bool Gamma_Or_Vreg1_Out_Of_Register_Limit, ref bool Within_Spec_Limit);
    }
}
