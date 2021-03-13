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
using System.DirectoryServices;

namespace PNC_Csharp
{
    public class DP213_Calculating_Init_RGBVreg1_Algorithm : DP213_forms_accessor
    {
        DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
        DP213_OC_Values_Storage storage = DP213_OC_Values_Storage.getInstance();
        DP213_OC_Current_Variables_Structure vars = DP213_OC_Current_Variables_Structure.getInstance();
        DP213_Intial_Algorithm_RGBVreg1_CPP init_algorithm_storage_cpp = DP213_Intial_Algorithm_RGBVreg1_CPP.getInstance();
        
        private double Combine_Lv_Ratio;
        private double Algorism_Upper_Skip_LV;
        private double Algorism_Lower_Skip_LV;
        private int Max_Applied_Band_Index;
        public DP213_Calculating_Init_RGBVreg1_Algorithm()
        {
            Combine_Lv_Ratio = Convert.ToDouble(dp213_form().textBox_Initial_RGB_Algorithm_Lv_Combine_Ratio.Text);
            Max_Applied_Band_Index = Convert.ToInt16(dp213_form().numericUpDown_Set456_Skip_Max_Band.Value);
            Algorism_Upper_Skip_LV = Convert.ToDouble(dp213_form().textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Upper_Limit.Text);
            Algorism_Lower_Skip_LV = Convert.ToDouble(dp213_form().textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Lower_Limit.Text);
        }

        private bool[] Get_Is_Selected_Mode1_Band()
        {
            bool[] Is_Selected_Mode1_Band = new bool[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int selected_band_index = 0; selected_band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; selected_band_index++)
            {
                if (selected_band_index <= Max_Applied_Band_Index)
                    Is_Selected_Mode1_Band[selected_band_index] = true;
                else
                    Is_Selected_Mode1_Band[selected_band_index] = false;
            }
            return Is_Selected_Mode1_Band;
        }

        private bool[] Get_Is_Selected_Mode456_Band()
        {
            bool[] Is_Selected_Mode456_Band = new bool[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int selected_band_index = 0; selected_band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; selected_band_index++)
            {
                if (selected_band_index <= Max_Applied_Band_Index)
                    Is_Selected_Mode456_Band[selected_band_index] = false;
                else
                    Is_Selected_Mode456_Band[selected_band_index] = true;
            }
            return Is_Selected_Mode456_Band;
        }

        private bool[] Get_Is_Selected_Bands(OC_Mode Mode)
        {
            if (Mode == OC_Mode.Mode1)
                return Get_Is_Selected_Mode1_Band();
            else
                return Get_Is_Selected_Mode456_Band();
        }



        private bool Is_Selected_Band(int band, OC_Mode Mode)
        {
            if (band <= Max_Applied_Band_Index)
            {
                if (Mode == OC_Mode.Mode1)
                    return true;
                else
                    return false;
            }
            else
            {
                if (Mode == OC_Mode.Mode1)
                    return false;
                else
                    return true;
            }
        }

        protected bool DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(Gamma_Set Set, int band, OC_Mode Mode)
        {
            if ((band >= 1) && Is_Selected_Band(band, Mode) && (vars.Target.double_Lv > Algorism_Lower_Skip_LV) && (vars.Target.double_Lv < Algorism_Upper_Skip_LV))
            {
                f1().GB_Status_AppendText_Nextline("Conduct)DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method()", Color.DarkBlue);
                Sub_DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(Set, band, Mode);
                return true;
            }
            else
            {
                f1().GB_Status_AppendText_Nextline("Skip)DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method()", Color.DarkRed);
                return false;
            }
        }



        private void Sub_DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(Gamma_Set Set, int band, OC_Mode Mode)
        {
            int Previous_Band_Vreg1_Dec = storage.Get_Normal_Dec_Vreg1(Set, (band - 1));
            double Voltage_VREF4095 = storage.Get_Voltage_VREF4095();
            double Voltage_VREF0 = storage.Get_Voltage_VREF0();

            RGB Prev_Band_AM1 = storage.Get_Band_Set_AM1(Set, (band - 1));
            storage.Set_Band_Set_AM1(Set, band, Prev_Band_AM1);//update AM1 voltage
            RGB_Double Prev_Band_Voltage_AM1 = storage.Get_Band_Set_Voltage_AM1(Set, (band - 1));

            int[] Previous_Band_Gamma_Red = new int[DP213_Static.Max_Gray_Amount];
            int[] Previous_Band_Gamma_Green = new int[DP213_Static.Max_Gray_Amount];
            int[] Previous_Band_Gamma_Blue = new int[DP213_Static.Max_Gray_Amount];
            double[] Previous_Band_Finally_Measured_Lv = new double[DP213_Static.Max_Gray_Amount];


            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
            {
                Previous_Band_Gamma_Red[gray] = storage.Get_All_band_gray_Gamma(Set, (band - 1), gray).int_R;
                Previous_Band_Gamma_Green[gray] = storage.Get_All_band_gray_Gamma(Set, (band - 1), gray).int_G;
                Previous_Band_Gamma_Blue[gray] = storage.Get_All_band_gray_Gamma(Set, (band - 1), gray).int_B;
                Previous_Band_Finally_Measured_Lv[gray] = dp213_mornitoring().Get_Mode_Measured_Values((band - 1), gray, Mode).double_Lv;
            }

            f1().GB_Status_AppendText_Nextline("Before) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Blue);


            Imported_my_cpp_dll.DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method_Through_MCI
 (Prev_Band_Voltage_AM1.double_R,
Prev_Band_Voltage_AM1.double_G,
Prev_Band_Voltage_AM1.double_B,
ref vars.Vreg1,
ref vars.Gamma.int_R,
ref vars.Gamma.int_B,
Get_Is_Selected_Bands(Mode),
Previous_Band_Gamma_Red,
Previous_Band_Gamma_Green,
Previous_Band_Gamma_Blue,
band,
vars.Target.double_Lv,
Previous_Band_Vreg1_Dec,
Previous_Band_Finally_Measured_Lv,
Voltage_VREF4095,
Voltage_VREF0);

            f1().GB_Status_AppendText_Nextline("After C++ Dll(MCI)) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Red);
            init_algorithm_storage_cpp.Set_All_band_gray_Gamma(Set, band, 0, vars.Gamma);

        }

        protected bool DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(Gamma_Set Set, int band, OC_Mode Mode)
        {
            if ((band >= 1) && Is_Selected_Band(band, Mode) && (vars.Target.double_Lv > Algorism_Lower_Skip_LV) && (vars.Target.double_Lv < Algorism_Upper_Skip_LV))
            {
                f1().GB_Status_AppendText_Nextline("Conduct)DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method()", Color.DarkBlue);
                Sub_DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(Set, band, Mode);
                return true;
            }
            else
            {
                f1().GB_Status_AppendText_Nextline("Skip)DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method()", Color.DarkRed);
                return false;
            }
        }

        private void Sub_DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(Gamma_Set Set, int band,OC_Mode Mode)
        {
            RGB_Double Prev_Band_Voltage_AM1 = storage.Get_Band_Set_Voltage_AM1(Set, (band - 1));
            int Previous_Band_Vreg1_Dec = storage.Get_Normal_Dec_Vreg1(Set, (band - 1));
            double Voltage_VREF4095 = storage.Get_Voltage_VREF4095();
            double Voltage_VREF0 = storage.Get_Voltage_VREF0();

            int[] Previous_Band_Gamma_Red = new int[DP213_Static.Max_Gray_Amount];
            int[] Previous_Band_Gamma_Green = new int[DP213_Static.Max_Gray_Amount];
            int[] Previous_Band_Gamma_Blue = new int[DP213_Static.Max_Gray_Amount];
            double[] Previous_Band_Finally_Measured_Lv = new double[DP213_Static.Max_Gray_Amount];
            
            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
            {
                Previous_Band_Gamma_Red[gray] = storage.Get_All_band_gray_Gamma(Set, (band - 1), gray).int_R;
                Previous_Band_Gamma_Green[gray] = storage.Get_All_band_gray_Gamma(Set, (band - 1), gray).int_G;
                Previous_Band_Gamma_Blue[gray] = storage.Get_All_band_gray_Gamma(Set, (band - 1), gray).int_B;
                Previous_Band_Finally_Measured_Lv[gray] = dp213_mornitoring().Get_Mode_Measured_Values((band - 1), gray, Mode).double_Lv;
            }

            
            f1().GB_Status_AppendText_Nextline("Before) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Blue);

    
                //Csharp Algorithm
                Imported_my_cpp_dll.DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method_Through_MCI
                    (vars.Vreg1,
                    Prev_Band_Voltage_AM1.double_R,
                    Prev_Band_Voltage_AM1.double_G,
                    Prev_Band_Voltage_AM1.double_B,
                    ref vars.Gamma.int_R,
                    ref vars.Gamma.int_G,
                    ref vars.Gamma.int_B,
                    Get_Is_Selected_Bands(Mode),
                    Previous_Band_Gamma_Red,
                    Previous_Band_Gamma_Green,
                    Previous_Band_Gamma_Blue,
                    band,
                    vars.Target.double_Lv,
                    Previous_Band_Vreg1_Dec,
                    Previous_Band_Finally_Measured_Lv,
                    Voltage_VREF4095,
                    Voltage_VREF0);

            f1().GB_Status_AppendText_Nextline("After C++(PolyI)) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Red);
            init_algorithm_storage_cpp.Set_All_band_gray_Gamma(Set, band, 0, vars.Gamma);
        }



        protected bool Get_Initial_RVreg1B_Using_LUT_MCI(Gamma_Set Set, int band, OC_Mode Mode)
        {
            if ((band >= 1) && (band <= DP213_Static.Max_HBM_and_Normal_Band_Amount) && (vars.Target.double_Lv > Algorism_Lower_Skip_LV) && (vars.Target.double_Lv < Algorism_Upper_Skip_LV))
            {
                f1().GB_Status_AppendText_Nextline("Before) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Blue);

                int gray = 0;
                RGB_Double Gray = DP213_Main_OC_Flow.init_gray_lut.Get_LUT_RGB(band, gray, Mode);

                RGB_Double Gamma_Voltage = new RGB_Double();
                Gamma_Voltage.double_R = DP213_Main_OC_Flow.interpolation_fomula_obj.GetPrevBand_Red_Volatge(Gray.double_R);
                Gamma_Voltage.double_G = DP213_Main_OC_Flow.interpolation_fomula_obj.GetPrevBand_Green_Volatge(Gray.double_G);
                Gamma_Voltage.double_B = DP213_Main_OC_Flow.interpolation_fomula_obj.GetPrevBand_Blue_Volatge(Gray.double_B);

                double Voltage_VREF0 = storage.Get_Voltage_VREF0();
                double Voltage_VREF4095 = storage.Get_Voltage_VREF4095();
                double Vreg1_voltage = Voltage_VREF4095 + ((Gamma_Voltage.double_G - Voltage_VREF4095) * (900.0 / (storage.Get_All_band_gray_Gamma(Set, (band - 1), gray).int_G + 389.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM

                vars.Vreg1 = Imported_my_cpp_dll.DP213_Get_Vreg1_Dec(Voltage_VREF4095, Voltage_VREF0, Vreg1_voltage);
                vars.Gamma.int_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Gamma_Voltage.double_R);
                vars.Gamma.int_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Gamma_Voltage.double_B);

                f1().GB_Status_AppendText_Nextline("After C++) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Red);
                init_algorithm_storage_cpp.Set_All_band_gray_Gamma(Set, band, gray, vars.Gamma);

                return true;
            }
            else
            {
                f1().GB_Status_AppendText_Nextline("Skip)Get_Initial_RVreg1B_Using_LUT_MCI()", Color.DarkRed);
                return false;
            }
        }

        protected bool Get_Initial_RGB_Using_LUT_MCI(Gamma_Set Set, int band, int gray, OC_Mode Mode)
        {
            if ((band >= 1) && (band <= DP213_Static.Max_HBM_and_Normal_Band_Amount) && (vars.Target.double_Lv > Algorism_Lower_Skip_LV) && (vars.Target.double_Lv < Algorism_Upper_Skip_LV))
            {
                RGB_Double Gray = DP213_Main_OC_Flow.init_gray_lut.Get_LUT_RGB(band, gray, Mode);

                RGB_Double Gamma_Voltage = new RGB_Double();
                Gamma_Voltage.double_R = DP213_Main_OC_Flow.interpolation_fomula_obj.GetPrevBand_Red_Volatge(Gray.double_R);
                Gamma_Voltage.double_G = DP213_Main_OC_Flow.interpolation_fomula_obj.GetPrevBand_Green_Volatge(Gray.double_G);
                Gamma_Voltage.double_B = DP213_Main_OC_Flow.interpolation_fomula_obj.GetPrevBand_Blue_Volatge(Gray.double_B);

                f1().GB_Status_AppendText_Nextline("Before) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Blue);

                if (gray == 0)
                {
                    double Voltage_VREF4095 = storage.Get_Voltage_VREF4095();
                    double Vreg1_Voltage = storage.Get_Normal_Voltage_Vreg1(Set, band);

                    vars.Gamma.int_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_Voltage, Gamma_Voltage.double_R);
                    vars.Gamma.int_G = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_Voltage, Gamma_Voltage.double_G);
                    vars.Gamma.int_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_Voltage, Gamma_Voltage.double_B);
                }
                else
                {
                    RGB_Double AM1_Voltage = storage.Get_Band_Set_Voltage_AM1(Set, band);

                    RGB_Double Prvious_Gray_Gamma_Voltage;
                    if (gray == 4 || gray == 6 || gray == 8)
                        Prvious_Gray_Gamma_Voltage = storage.Get_Voltage_All_band_gray_Gamma(Set, band, (gray - 2));
                    else
                        Prvious_Gray_Gamma_Voltage = storage.Get_Voltage_All_band_gray_Gamma(Set, band, (gray - 1));
                    
                    vars.Gamma.int_R = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Dec(AM1_Voltage.double_R, Prvious_Gray_Gamma_Voltage.double_R, Gamma_Voltage.double_R, gray);
                    vars.Gamma.int_G = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Dec(AM1_Voltage.double_G, Prvious_Gray_Gamma_Voltage.double_G, Gamma_Voltage.double_G, gray);
                    vars.Gamma.int_B = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Dec(AM1_Voltage.double_B, Prvious_Gray_Gamma_Voltage.double_B, Gamma_Voltage.double_B, gray);
                }

                f1().GB_Status_AppendText_Nextline("After C++) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Red);
                init_algorithm_storage_cpp.Set_All_band_gray_Gamma(Set, band, gray, vars.Gamma);
                return true;
            }
            else
            {
                f1().GB_Status_AppendText_Nextline("Skip)Get_Initial_RGB_Using_LUT_MCI()", Color.DarkRed);
                return false;
            }
        }

        protected bool DP213_Get_Intial_R_G_B_Using_3Points_Method(Gamma_Set Set, int band, int gray,OC_Mode Mode)
        {
            if ((band >= 1) && (gray >= 1) && Is_Selected_Band(band, Mode) && (vars.Target.double_Lv > Algorism_Lower_Skip_LV) && (vars.Target.double_Lv < Algorism_Upper_Skip_LV))
            {
                f1().GB_Status_AppendText_Nextline("Conduct)DP213_Get_Intial_R_G_B_Using_3Points_Method()", Color.DarkBlue);
                Sub_DP213_Get_Intial_R_G_B_Using_3Points_Method(Set, band, gray, Mode);
                return true;
            }
            else
            {
                f1().GB_Status_AppendText_Nextline("Skip)DP213_Get_Intial_R_G_B_Using_3Points_Method()", Color.DarkRed);
                return false;
            }
        }

        private void Sub_DP213_Get_Intial_R_G_B_Using_3Points_Method(Gamma_Set Set, int band, int gray, OC_Mode Mode)
        {
            RGB_Double Prvious_Gray_Gamma_Voltage;
            if (gray == 4 || gray == 6 || gray == 8)
            {
                Prvious_Gray_Gamma_Voltage = storage.Get_Voltage_All_band_gray_Gamma(Set, band, (gray - 2));
            }
            else
            {
                Prvious_Gray_Gamma_Voltage = storage.Get_Voltage_All_band_gray_Gamma(Set, band, (gray - 1));
            }

            double Voltage_VREF4095 = storage.Get_Voltage_VREF4095();
            double Voltage_VREF0 = storage.Get_Voltage_VREF0();

            int[] Band_Gray_Gamma_Red = new int[band * DP213_Static.Max_Gray_Amount];//Prev bands' grays
            int[] Band_Gray_Gamma_Green = new int[band * DP213_Static.Max_Gray_Amount];//Prev bands' grays
            int[] Band_Gray_Gamma_Blue = new int[band * DP213_Static.Max_Gray_Amount];//Prev bands' grays
            double[] Band_Gray_Finally_Measured_Lv = new double[band * DP213_Static.Max_Gray_Amount];//Prev bands' grays
            int[] Band_Vreg1_Dec = new int[band + 1];//Pre + Current bands

            double[] Band_Voltage_AM1_R = new double[band + 1];//Pre + Current bands
            double[] Band_Voltage_AM1_G = new double[band + 1];//Pre + Current bands
            double[] Band_Voltage_AM1_B = new double[band + 1];//Pre + Current bands

            for (int b = 0; b < band; b++)
            {
                for (int g = 0; g < DP213_Static.Max_Gray_Amount; g++)
                {
                    Band_Gray_Gamma_Red[(b * DP213_Static.Max_Gray_Amount) + g] = storage.Get_All_band_gray_Gamma(Set, b, g).int_R;
                    Band_Gray_Gamma_Green[(b * DP213_Static.Max_Gray_Amount) + g] = storage.Get_All_band_gray_Gamma(Set, b, g).int_G;
                    Band_Gray_Gamma_Blue[(b * DP213_Static.Max_Gray_Amount) + g] = storage.Get_All_band_gray_Gamma(Set, b, g).int_B;
                    Band_Gray_Finally_Measured_Lv[(b * DP213_Static.Max_Gray_Amount) + g] = dp213_mornitoring().Get_Mode_Measured_Values(b, g, Mode).double_Lv;
                }
                Band_Vreg1_Dec[b] = storage.Get_Normal_Dec_Vreg1(Set, b);
                Band_Voltage_AM1_R[b] = storage.Get_Band_Set_Voltage_AM1(Set, b).double_R;
                Band_Voltage_AM1_G[b] = storage.Get_Band_Set_Voltage_AM1(Set, b).double_G;
                Band_Voltage_AM1_B[b] = storage.Get_Band_Set_Voltage_AM1(Set, b).double_B;

            }
            Band_Vreg1_Dec[band] = vars.Vreg1;
            Band_Voltage_AM1_R[band] = storage.Get_Band_Set_Voltage_AM1(Set, band).double_R;
            Band_Voltage_AM1_G[band] = storage.Get_Band_Set_Voltage_AM1(Set, band).double_G;
            Band_Voltage_AM1_B[band] = storage.Get_Band_Set_Voltage_AM1(Set, band).double_B;

            for (int b = 0; b < band; b++)
            {

                Band_Vreg1_Dec[b] = storage.Get_Normal_Dec_Vreg1(Set, b);
            }

            f1().GB_Status_AppendText_Nextline("Before) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Blue);

            Imported_my_cpp_dll.DP213_Get_Intial_R_G_B_Using_3Points_Method
                (Combine_Lv_Ratio,
                Band_Voltage_AM1_R,
                Band_Voltage_AM1_G,
                Band_Voltage_AM1_B,
                ref vars.Gamma.int_R,
                ref vars.Gamma.int_G,
                ref vars.Gamma.int_B,
                Get_Is_Selected_Bands(Mode),
                Band_Gray_Gamma_Red,
                Band_Gray_Gamma_Green,
                Band_Gray_Gamma_Blue,
                Band_Gray_Finally_Measured_Lv,
                Band_Vreg1_Dec,
                band,
                gray,
                vars.Target.double_Lv,
                Prvious_Gray_Gamma_Voltage.double_R,
                Prvious_Gray_Gamma_Voltage.double_G,
                Prvious_Gray_Gamma_Voltage.double_B,
                Voltage_VREF4095,
                Voltage_VREF0);

            f1().GB_Status_AppendText_Nextline("After C++(PolyI)) R/G/B/Vreg1 = " + vars.Gamma.int_R.ToString() + "/" + vars.Gamma.int_G.ToString().ToString() + "/" + vars.Gamma.int_B.ToString() + "/" + vars.Vreg1, Color.Red);
            init_algorithm_storage_cpp.Set_All_band_gray_Gamma(Set, band, gray, vars.Gamma);
        }
    }
}
