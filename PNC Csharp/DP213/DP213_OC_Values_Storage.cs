using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Xml.Serialization;
using BSQH_Csharp_Library;


namespace PNC_Csharp
{

    public class DP213_Intial_Algorithm_RGBVreg1_CPP : DP213_Intial_Algorithm_RGBVreg1
    {
        private static DP213_Intial_Algorithm_RGBVreg1_CPP instance;
        private DP213_Intial_Algorithm_RGBVreg1_CPP() : base() { }
        public static DP213_Intial_Algorithm_RGBVreg1_CPP getInstance()
        {
            if(instance == null)
                instance = new DP213_Intial_Algorithm_RGBVreg1_CPP();

            return instance;
        }
    }





    public class DP213_Intial_Algorithm_RGBVreg1 : DP213_forms_accessor
    {
        DP213_OC_Values_Storage storage = DP213_OC_Values_Storage.getInstance();
        protected DP213_Intial_Algorithm_RGBVreg1(){}

        public void Data_Initializing_As_Zeros()
        {
            for (int set = 0; set < DP213_Static.Max_Set_Amount; set++)
            {
                for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
                {
                    for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                    {
                        All_band_gray_Gamma[set, band, gray].Set_Value(0, 0, 0, 0);//Include AOD band
                        Voltage_All_band_gray_Gamma[set, band, gray].Set_Value(0, 0, 0, 0);
                    }
                }
            }
        }
        void Get_Extended_Log_Max_Line()
        {
            int Max_Line = Convert.ToInt32(f1().textBox_GB_Status_Max_Line.Text);
            Max_Line += 1000;
            f1().textBox_GB_Status_Max_Line.Text = Max_Line.ToString();
        }

        public void Show_RGB_Datas(Color color)
        {
            Get_Extended_Log_Max_Line();
            for (int set = 0; set < DP213_Static.Max_Set_Amount; set++)
            {
                for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
                {
                    for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                    {
                        StringBuilder temp_string = new StringBuilder("[Set,band,gray) : ");
                       
                        temp_string.Append(set).Append(",").Append(band).Append(",").Append(gray).Append("]/")
                            .Append(All_band_gray_Gamma[set, band, gray].int_R).Append("/")
                            .Append(All_band_gray_Gamma[set, band, gray].int_G).Append("/")
                            .Append(All_band_gray_Gamma[set, band, gray].int_B).Append("/")
                            .Append(Voltage_All_band_gray_Gamma[set, band, gray].double_R).Append("/")
                            .Append(Voltage_All_band_gray_Gamma[set, band, gray].double_G).Append("/")
                            .Append(Voltage_All_band_gray_Gamma[set, band, gray].double_B).Append("/");

                        f1().GB_Status_AppendText_Nextline(temp_string.ToString(), color);
                    }
                }
            }
        }

        //RGB Gamma
        private RGB[, ,] All_band_gray_Gamma = new RGB[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
        private RGB_Double[, ,] Voltage_All_band_gray_Gamma = new RGB_Double[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
        public RGB[] Get_Band_Set_Gamma(Gamma_Set Set, int band)
        {
            RGB[] Band_Set_Gamma = new RGB[DP213_Static.Max_Gray_Amount];
            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
            {
                Band_Set_Gamma[gray].Equal_Value(All_band_gray_Gamma[Convert.ToInt16(Set), band, gray]);
            }
            return Band_Set_Gamma;
        }
        public RGB Get_All_band_gray_Gamma(Gamma_Set Set, int band, int gray)
        {
            return All_band_gray_Gamma[Convert.ToInt16(Set), band, gray];
        }
        public RGB_Double Get_Voltage_All_band_gray_Gamma(Gamma_Set Set, int band, int gray)
        {
            Update_HBM_Normal_Gamma_Voltage(Set, band, gray, All_band_gray_Gamma[Convert.ToInt16(Set), band, gray]);
            return Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray];
        }

        public void Set_All_band_gray_Gamma(Gamma_Set Set, int band, int gray, RGB New_Gamma)
        {
            All_band_gray_Gamma[Convert.ToInt16(Set), band, gray] = New_Gamma;
            //Band (HBM + Normal) : update value and voltage together
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                Update_HBM_Normal_Gamma_Voltage(Set, band, gray, New_Gamma);
            }
        }

        private void Update_HBM_Normal_Gamma_Voltage(Gamma_Set Set, int band, int gray, RGB New_Gamma)
        {
            if (gray == 0)
            {
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(Set, band), New_Gamma.int_R);
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_G = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(Set, band), New_Gamma.int_G);
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(Set, band), New_Gamma.int_B);
            }
            else
            {
                RGB_Double Prev_GR_Gamma_Voltage = new RGB_Double();
                if (gray == 1 || gray == 2 || gray == 3 || gray == 5 || gray == 7 || gray == 9 || gray == 10)
                {
                    Prev_GR_Gamma_Voltage.double_R = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 1)].double_R;
                    Prev_GR_Gamma_Voltage.double_G = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 1)].double_G;
                    Prev_GR_Gamma_Voltage.double_B = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 1)].double_B;
                }

                else if (gray == 4 || gray == 6 || gray == 8)
                {
                    Prev_GR_Gamma_Voltage.double_R = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 2)].double_R;
                    Prev_GR_Gamma_Voltage.double_G = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 2)].double_G;
                    Prev_GR_Gamma_Voltage.double_B = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 2)].double_B;
                }
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_R = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(storage.Get_Band_Set_Voltage_AM1(Set, band).double_R, Prev_GR_Gamma_Voltage.double_R, New_Gamma.int_R, gray);
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_G = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(storage.Get_Band_Set_Voltage_AM1(Set, band).double_G, Prev_GR_Gamma_Voltage.double_G, New_Gamma.int_G, gray);
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_B = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(storage.Get_Band_Set_Voltage_AM1(Set, band).double_B, Prev_GR_Gamma_Voltage.double_B, New_Gamma.int_B, gray);
            }
        }
    }




    public class DP213_OC_Values_Storage : DP213_forms_accessor
    {
        ELVSS_Vinit2 elvss_vini2;
        ELVSS_Vinit2 Cold_elvss_vini2;

        //Singleton
        static private DP213_OC_Values_Storage instance = null;
        private DP213_OC_Values_Storage()
        {
            elvss_vini2 = new ELVSS_Vinit2();
            Cold_elvss_vini2 = new ELVSS_Vinit2();
        }
        static public DP213_OC_Values_Storage getInstance()
        {
            if (instance == null) instance = new DP213_OC_Values_Storage();
            return instance;
        }

        //Initialize
        public void Data_Initializing_As_Zeros()
        {
            Byte_VREF4095 = 0;
            Voltage_VREF4095 = 0;

            Byte_VREF0 = 0;
            Voltage_VREF0 = 0;

            AOD_Dec_VREF4095 = 0;

            AOD_Dec_VREF0 = 0;
            AOD_Dec_Vreg1[0] = AOD_Dec_Vreg1[1] = AOD_Dec_Vreg1[2] = 0;

            for (int set = 0; set < DP213_Static.Max_Set_Amount; set++)
            {
                for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
                {
                    if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
                    {
                        AM0[set, band].Set_Value(0, 0, 0, 0);//Include AOD band
                        AM1[set, band].Set_Value(0, 0, 0, 0);//Include AOD band

                        Voltage_AM0[set, band].Set_Value(0, 0, 0, 0);
                        Voltage_AM1[set, band].Set_Value(0, 0, 0, 0);

                        Dec_Normal_Vreg1[set, band] = 0;
                        Vreg1_Voltage[set, band] = 0;

                        elvss_vini2.Set_ELVSS_Dec_Voltage_AS_Zero(set, band);
                        elvss_vini2.Set_Vinit2_Dec_Voltage_AS_Zero(set, band);

                        Cold_elvss_vini2.Set_ELVSS_Dec_Voltage_AS_Zero(set, band);
                        Cold_elvss_vini2.Set_Vinit2_Dec_Voltage_AS_Zero(set, band);
                    }

                    for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                    {
                        All_band_gray_Gamma[set, band, gray].Set_Value(0, 0, 0, 0);//Include AOD band

                        if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
                        {
                            Voltage_All_band_gray_Gamma[set, band, gray].Set_Value(0, 0, 0, 0);
                        }
                    }
                }
            }
        }


        //RGB AM0
        private RGB[,] AM0 = new RGB[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        private RGB_Double[,] Voltage_AM0 = new RGB_Double[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        public RGB Get_Band_Set_AM0(Gamma_Set Set, int band)
        {
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                return AM0[Convert.ToInt16(Set), band];
            }
            else
            {
                return new RGB(0);
            }
            
        }

        public RGB_Double Get_Band_Set_Voltage_AM0(Gamma_Set Set, int band)
        {
            int Set_Index = Convert.ToInt16(Set);
            Voltage_AM0[Set_Index, band].double_R = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM0(Set, band).int_R);
            Voltage_AM0[Set_Index, band].double_G = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM0(Set, band).int_G);
            Voltage_AM0[Set_Index, band].double_B = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM0(Set, band).int_B);
            return Voltage_AM0[Set_Index, band];
        }

        public void Set_All_Band_Set_AM0_As_Same_Values(RGB New_AM0)
        {
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                Set_Band_Set_AM0(Gamma_Set.Set1, band, New_AM0);
                Set_Band_Set_AM0(Gamma_Set.Set2, band, New_AM0);
                Set_Band_Set_AM0(Gamma_Set.Set3, band, New_AM0);
                Set_Band_Set_AM0(Gamma_Set.Set4, band, New_AM0);
                Set_Band_Set_AM0(Gamma_Set.Set5, band, New_AM0);
                Set_Band_Set_AM0(Gamma_Set.Set6, band, New_AM0);
            }
        }

        public void Set_Band_Set_AM0(Gamma_Set Set, int band, RGB New_AM0)
        {
            if(band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                int Set_Index = Convert.ToInt16(Set);
                AM0[Set_Index, band] = New_AM0;

                Voltage_AM0[Set_Index, band].double_R = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM0(Set, band).int_R);
                Voltage_AM0[Set_Index, band].double_G = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM0(Set, band).int_G);
                Voltage_AM0[Set_Index, band].double_B = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM0(Set, band).int_B);
            }
        }

        //RGB AM1
        private RGB[,] AM1 = new RGB[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        private RGB_Double[,] Voltage_AM1 = new RGB_Double[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        public RGB Get_Band_Set_AM1(Gamma_Set Set, int band)
        {
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                return AM1[Convert.ToInt16(Set), band];
            }
            else
            {
                return new RGB(0);
            }
        }

        public RGB_Double Get_Band_Set_Voltage_AM1(Gamma_Set Set, int band)
        {
            int Set_Index = Convert.ToInt16(Set);

            Voltage_AM1[Set_Index, band].double_R = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM1(Set, band).int_R);
            Voltage_AM1[Set_Index, band].double_G = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM1(Set, band).int_G);
            Voltage_AM1[Set_Index, band].double_B = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM1(Set, band).int_B);

            return Voltage_AM1[Convert.ToInt16(Set), band];
        }

        public void Set_All_Band_Set_AM1_As_Same_Values(RGB New_AM1)
        {
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                Set_Band_Set_AM1(Gamma_Set.Set1, band, New_AM1);
                Set_Band_Set_AM1(Gamma_Set.Set2, band, New_AM1);
                Set_Band_Set_AM1(Gamma_Set.Set3, band, New_AM1);
                Set_Band_Set_AM1(Gamma_Set.Set4, band, New_AM1);
                Set_Band_Set_AM1(Gamma_Set.Set5, band, New_AM1);
                Set_Band_Set_AM1(Gamma_Set.Set6, band, New_AM1);
            }
        }

        public void Set_All_Band_Set_AM1_By_Applying_Offset(RGB New_AM1)
        {
            RGB[,] AM1_Offset = DP213_OC_Offset.getInstance().getAM1Offset();
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                Set_Band_Set_AM1(Gamma_Set.Set1, band, New_AM1 + AM1_Offset[band, 0]);
                Set_Band_Set_AM1(Gamma_Set.Set2, band, New_AM1 + AM1_Offset[band, 1]);
                Set_Band_Set_AM1(Gamma_Set.Set3, band, New_AM1 + AM1_Offset[band, 2]);
                Set_Band_Set_AM1(Gamma_Set.Set4, band, New_AM1 + AM1_Offset[band, 3]);
                Set_Band_Set_AM1(Gamma_Set.Set5, band, New_AM1 + AM1_Offset[band, 4]);
                Set_Band_Set_AM1(Gamma_Set.Set6, band, New_AM1 + AM1_Offset[band, 5]);
            }
        }

        public void Set_Band_Set_AM1(Gamma_Set Set, int band, RGB New_AM1)
        {
            if (New_AM1.int_R > DP213_Static.AM1_AM0_Max) New_AM1.int_R = DP213_Static.AM1_AM0_Max;
            if (New_AM1.int_G > DP213_Static.AM1_AM0_Max) New_AM1.int_G = DP213_Static.AM1_AM0_Max;
            if (New_AM1.int_B > DP213_Static.AM1_AM0_Max) New_AM1.int_B = DP213_Static.AM1_AM0_Max;
            if (New_AM1.int_R < 0) New_AM1.int_R = 0;
            if (New_AM1.int_G < 0) New_AM1.int_G = 0;
            if (New_AM1.int_B < 0) New_AM1.int_B = 0;
           
            if(band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                int Set_Index = Convert.ToInt16(Set);
                AM1[Set_Index, band] = New_AM1;

                Voltage_AM1[Set_Index, band].double_R = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM1(Set, band).int_R);
                Voltage_AM1[Set_Index, band].double_G = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM1(Set, band).int_G);
                Voltage_AM1[Set_Index, band].double_B = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), Get_Band_Set_AM1(Set, band).int_B);
            }
        }

        public RGB AM1_Convert_Voltgae_to_Dec(Gamma_Set Set, int band, RGB_Double AM1_Voltage)
        {
            RGB AM1_Dec = new RGB();

            AM1_Dec.int_R = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Dec(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), AM1_Voltage.double_R);
            AM1_Dec.int_G = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Dec(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), AM1_Voltage.double_G);
            AM1_Dec.int_B = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Dec(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), AM1_Voltage.double_B);

            if (AM1_Dec.int_R > DP213_Static.AM1_AM0_Max) AM1_Dec.int_R = DP213_Static.AM1_AM0_Max;
            if (AM1_Dec.int_G > DP213_Static.AM1_AM0_Max) AM1_Dec.int_G = DP213_Static.AM1_AM0_Max;
            if (AM1_Dec.int_B > DP213_Static.AM1_AM0_Max) AM1_Dec.int_B = DP213_Static.AM1_AM0_Max;
            if (AM1_Dec.int_R < 0) AM1_Dec.int_R = 0;
            if (AM1_Dec.int_G < 0) AM1_Dec.int_G = 0;
            if (AM1_Dec.int_B < 0) AM1_Dec.int_B = 0;

            return AM1_Dec;
        }

        public RGB AM0_Convert_Voltgae_to_Dec(Gamma_Set Set, int band, RGB_Double AM0_Voltage)
        {
            RGB AM0_Dec = new RGB();

            AM0_Dec.int_R = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Dec(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), AM0_Voltage.double_R);
            AM0_Dec.int_G = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Dec(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), AM0_Voltage.double_G);
            AM0_Dec.int_B = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Dec(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), AM0_Voltage.double_B);

            return AM0_Dec;
        }





        //RGB Gamma
        private RGB[, ,] All_band_gray_Gamma = new RGB[DP213_Static.Max_Set_Amount, DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        private RGB_Double[, ,] Voltage_All_band_gray_Gamma = new RGB_Double[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
        private RGB_Double[] HBM_Voltage_Right_After_AM1_OC = new RGB_Double[DP213_Static.Max_Gray_Amount];

        public RGB[] Get_Band_Set_Gamma(Gamma_Set Set, int band)
        {
            RGB[] Band_Set_Gamma = new RGB[DP213_Static.Max_Gray_Amount];
            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
            {
                Band_Set_Gamma[gray].Equal_Value(All_band_gray_Gamma[Convert.ToInt16(Set), band, gray]);
            }
            return Band_Set_Gamma;
        }

        public RGB_Double[] Get_Band_Set_Gamma_Voltages(Gamma_Set Set, int band)
        {
            RGB_Double[] Band_Set_Gamma_Voltage = new RGB_Double[DP213_Static.Max_Gray_Amount];
            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                Band_Set_Gamma_Voltage[gray] = Get_Voltage_All_band_gray_Gamma(Set, band, gray);
            return Band_Set_Gamma_Voltage;
        }

        public RGB Get_All_band_gray_Gamma(Gamma_Set Set, int band, int gray)
        {
            return All_band_gray_Gamma[Convert.ToInt16(Set), band, gray];
        }
        public RGB_Double Get_Voltage_All_band_gray_Gamma(Gamma_Set Set, int band, int gray)
        {
            Update_HBM_Normal_Gamma_Voltage(Set, band, gray, All_band_gray_Gamma[Convert.ToInt16(Set), band, gray]);
            return Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray];
        }

        public RGB_Double Get_HBM_Voltage_Right_After_AM1_OC(int gray)
        {
            return HBM_Voltage_Right_After_AM1_OC[gray];
        }

        public void Update_HBM_Voltages_Right_After_AM1_OC(Gamma_Set Set)
        {
            for (int g = 0; g < HBM_Voltage_Right_After_AM1_OC.Length; g++)
                HBM_Voltage_Right_After_AM1_OC[g] = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), 0, g];
        }

        public void Set_All_band_gray_Gamma(RGB[, ,] New_All_band_gray_Gamma)
        {
            //Set
            for (int Set_Index = 0; Set_Index < DP213_Static.Max_Set_Amount; Set_Index++)
            {
                //Gray
                for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                {
                    for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
                    {
                        Set_All_band_gray_Gamma(Set_Index, band, gray, New_All_band_gray_Gamma[Set_Index, band, gray]);
                    }
                }
            }
        }

        public Gamma_Set Convert_index_Set_to_Gamma_Set(int Set_Index)
        {
            if (Set_Index == 0) return Gamma_Set.Set1;
            else if (Set_Index == 1) return Gamma_Set.Set2;
            else if (Set_Index == 2) return Gamma_Set.Set3;
            else if (Set_Index == 3) return Gamma_Set.Set4;
            else if (Set_Index == 4) return Gamma_Set.Set5;
            else if (Set_Index == 5) return Gamma_Set.Set6;
            else
            {
                System.Windows.Forms.MessageBox.Show("Convert_index_Set_to_Gamma_Set) Set_Index should be within 1~5");
                return Gamma_Set.Set1;
            }
        }

        public void Set_All_band_gray_Gamma(int Set_Index, int band, int gray, RGB New_Gamma)
        {
            Gamma_Set Set = Convert_index_Set_to_Gamma_Set(Set_Index);
            Set_All_band_gray_Gamma(Set, band, gray, New_Gamma);
        }


        public void Set_All_band_gray_Gamma(Gamma_Set Set, int band, int gray, RGB New_Gamma)
        {
            if (Set == Gamma_Set.SetNull)
                Set = Gamma_Set.Set1;

            All_band_gray_Gamma[Convert.ToInt16(Set), band, gray] = New_Gamma;
            //Band (HBM + Normal) : update value and voltage together
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                Update_HBM_Normal_Gamma_Voltage(Set, band, gray, New_Gamma);
            }
        }

        private void Update_HBM_Normal_Gamma_Voltage(Gamma_Set Set, int band, int gray, RGB New_Gamma)
        {
            if (gray == 0)
            {
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), New_Gamma.int_R);
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_G = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), New_Gamma.int_G);
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(Get_Voltage_VREF4095(), Get_Normal_Voltage_Vreg1(Set, band), New_Gamma.int_B);
            }
            else
            {
                RGB_Double Prev_GR_Gamma_Voltage = new RGB_Double();
                if (gray == 1 || gray == 2 || gray == 3 || gray == 5 || gray == 7 || gray == 9 || gray == 10)
                {
                    Prev_GR_Gamma_Voltage.double_R = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 1)].double_R;
                    Prev_GR_Gamma_Voltage.double_G = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 1)].double_G;
                    Prev_GR_Gamma_Voltage.double_B = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 1)].double_B;
                }

                else if (gray == 4 || gray == 6 || gray == 8)
                {
                    Prev_GR_Gamma_Voltage.double_R = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 2)].double_R;
                    Prev_GR_Gamma_Voltage.double_G = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 2)].double_G;
                    Prev_GR_Gamma_Voltage.double_B = Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, (gray - 2)].double_B;
                }
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_R = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(Get_Band_Set_Voltage_AM1(Set, band).double_R, Prev_GR_Gamma_Voltage.double_R, New_Gamma.int_R, gray);
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_G = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(Get_Band_Set_Voltage_AM1(Set, band).double_G, Prev_GR_Gamma_Voltage.double_G, New_Gamma.int_G, gray);
                Voltage_All_band_gray_Gamma[Convert.ToInt16(Set), band, gray].double_B = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(Get_Band_Set_Voltage_AM1(Set, band).double_B, Prev_GR_Gamma_Voltage.double_B, New_Gamma.int_B, gray);
            }
        }

        //REF0/REF4095
        private byte Byte_VREF4095;
        private double Voltage_VREF4095;
        public void Set_Dec_VREF4095(byte New_Byte_VREF4095)
        {
            Byte_VREF4095 = New_Byte_VREF4095;
            Voltage_VREF4095 = Imported_my_cpp_dll.DP213_VREF4095_Dec_to_Voltage(New_Byte_VREF4095);
        }

        public byte Get_Dec_VREF4095()
        {
            return Byte_VREF4095;
        }

        public double Get_Voltage_VREF4095()
        {
            Voltage_VREF4095 = Imported_my_cpp_dll.DP213_VREF4095_Dec_to_Voltage(Byte_VREF4095);
            return Voltage_VREF4095;
        }
        public string Get_Hex_VREF4095()
        {
            return Byte_VREF4095.ToString("X2");
        }

        private byte Byte_VREF0;
        private double Voltage_VREF0;
        public void Set_Dec_VREF0(byte New_Byte_VREF0)
        {
            Byte_VREF0 = New_Byte_VREF0;
            Voltage_VREF0 = Imported_my_cpp_dll.DP213_VREF0_Dec_to_Voltage(Byte_VREF0);
        }
        public double Get_Voltage_VREF0()
        {
            Voltage_VREF0 = Imported_my_cpp_dll.DP213_VREF0_Dec_to_Voltage(Byte_VREF0);
            return Voltage_VREF0;
        }
        public string Get_Hex_VREF0()
        {
            return Byte_VREF0.ToString("X2");
        }
        public int Get_Dec_VREF0()
        {
            return Byte_VREF0;
        }

        //AOD REF0 REF4095
        private byte AOD_Dec_VREF4095;
        public void Set_AOD_Dec_VREF4095(byte New_Dec_VREF4095)
        {
            AOD_Dec_VREF4095 = New_Dec_VREF4095;
        }
        public byte Get_AOD_Dec_VREF4095()
        {
            return AOD_Dec_VREF4095;
        }

        private int AOD_Dec_VREF0;
        public void Set_AOD_Dec_VREF0(int New_Dec_VREF0)
        {
            AOD_Dec_VREF0 = New_Dec_VREF0;
        }
        public int Get_AOD_Dec_VREF0()
        {
            return AOD_Dec_VREF0;
        }


        //Vreg1
        private int[,] Dec_Normal_Vreg1 = new int[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[,] Vreg1_Voltage = new double[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        
        public string[] Get_Set_Hex_Normal_Vreg1(Gamma_Set Set)
        {
            int[] Band_Dec_Vreg1 = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            string[] Hex_Vreg1_Array = new string[DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount];

            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                Band_Dec_Vreg1[band] = Dec_Normal_Vreg1[Convert.ToInt16(Set), band];
            }

            Hex_Vreg1_Array[0] = (((Band_Dec_Vreg1[0] & 0xF00) >> 4) + ((Band_Dec_Vreg1[1] & 0xF00) >> 8)).ToString("X2");
            Hex_Vreg1_Array[1] = (((Band_Dec_Vreg1[2] & 0xF00) >> 4) + ((Band_Dec_Vreg1[3] & 0xF00) >> 8)).ToString("X2");
            Hex_Vreg1_Array[2] = (((Band_Dec_Vreg1[4] & 0xF00) >> 4) + ((Band_Dec_Vreg1[5] & 0xF00) >> 8)).ToString("X2");
            Hex_Vreg1_Array[3] = (((Band_Dec_Vreg1[6] & 0xF00) >> 4) + ((Band_Dec_Vreg1[7] & 0xF00) >> 8)).ToString("X2");
            Hex_Vreg1_Array[4] = (((Band_Dec_Vreg1[8] & 0xF00) >> 4) + ((Band_Dec_Vreg1[9] & 0xF00) >> 8)).ToString("X2");
            Hex_Vreg1_Array[5] = (((Band_Dec_Vreg1[10] & 0xF00) >> 4) + ((Band_Dec_Vreg1[11] & 0xF00) >> 8)).ToString("X2");

            Hex_Vreg1_Array[6] = (Band_Dec_Vreg1[0] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[7] = (Band_Dec_Vreg1[1] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[8] = (Band_Dec_Vreg1[2] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[9] = (Band_Dec_Vreg1[3] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[10] = (Band_Dec_Vreg1[4] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[11] = (Band_Dec_Vreg1[5] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[12] = (Band_Dec_Vreg1[6] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[13] = (Band_Dec_Vreg1[7] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[14] = (Band_Dec_Vreg1[8] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[15] = (Band_Dec_Vreg1[9] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[16] = (Band_Dec_Vreg1[10] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[17] = (Band_Dec_Vreg1[11] & 0xFF).ToString("X2");

            return Hex_Vreg1_Array;
        }

        public int[] Get_Normal_Dec_Vreg1s(Gamma_Set Set)
        {
            int[] Band_Dec_Vreg1 = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                Band_Dec_Vreg1[band] = Dec_Normal_Vreg1[Convert.ToInt16(Set), band];
            }
            return Band_Dec_Vreg1;
        }


        public int Get_Normal_Dec_Vreg1(Gamma_Set Set, int band)
        {
            return Dec_Normal_Vreg1[Convert.ToInt16(Set), band];
        }

        public double Get_Normal_Voltage_Vreg1(Gamma_Set Set, int band)
        {
            Vreg1_Voltage[Convert.ToInt16(Set), band] = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(Get_Voltage_VREF4095(), Get_Voltage_VREF0(), Get_Normal_Dec_Vreg1(Set, band));
            return Vreg1_Voltage[Convert.ToInt16(Set), band];
        }
        public void Set_Normal_Dec_Vreg1(Gamma_Set Set, int band, int New_Vreg1)
        {
            Dec_Normal_Vreg1[Convert.ToInt16(Set), band] = New_Vreg1;
            Vreg1_Voltage[Convert.ToInt16(Set), band] = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(Get_Voltage_VREF4095(), Get_Voltage_VREF0(), Get_Normal_Dec_Vreg1(Set, band));
        }

        //AOD Vreg1
        private int[] AOD_Dec_Vreg1 = new int[3];
        public int Get_AOD_Dec_Vreg1(int Aod_band_index)
        {
          return AOD_Dec_Vreg1[Aod_band_index];
        }
        public void Set_AOD_Dec_Vreg1(int Aod_band_index, int New_Vreg1)
        {
            AOD_Dec_Vreg1[Aod_band_index] = New_Vreg1;
        }

        //ELVSS & Vinit2
        public void Set_Normal_Dec_ELVSS(Gamma_Set Set, int band, byte New_Byte_ELVSS)
        {
            elvss_vini2.Set_Dec_ELVSS(Set, band, New_Byte_ELVSS);
        }
        public void Set_Cold_Dec_ELVSS(Gamma_Set Set, int band, byte New_Byte_ELVSS)
        {
            Cold_elvss_vini2.Set_Dec_ELVSS(Set, band, New_Byte_ELVSS);
        }


        public void Set_Normal_Voltage_ELVSS(Gamma_Set Set, int band, double New_ELVSS_Voltage)
        {
            elvss_vini2.Set_Voltage_ELVSS(Set, band, New_ELVSS_Voltage);
        }
        public void Set_Cold_Voltage_ELVSS(Gamma_Set Set, int band, double New_ELVSS_Voltage)
        {
            Cold_elvss_vini2.Set_Voltage_ELVSS(Set, band, New_ELVSS_Voltage);
        }


        public int Get_Normal_Dec_ELVSS(Gamma_Set Set, int band)
        {
            return elvss_vini2.Get_Dec_ELVSS(Set, band);
        }
        public int Get_Cold_Dec_ELVSS(Gamma_Set Set, int band)
        {
            return Cold_elvss_vini2.Get_Dec_ELVSS(Set, band);
        }


        public double Get_Normal_Voltage_ELVSS(Gamma_Set Set, int band)
        {
            return elvss_vini2.Get_Voltage_ELVSS(Set, band);
        }
        public double Get_Cold_Voltage_ELVSS(Gamma_Set Set, int band)
        {
            return Cold_elvss_vini2.Get_Voltage_ELVSS(Set, band);
        }

        public double[] Get_Normal_ELVSS_Voltages(Gamma_Set Set)
        {
            return elvss_vini2.Get_ELVSS_Voltages(Set);
        }
        public double[] Get_Cold_ELVSS_Voltages(Gamma_Set Set)
        {
            return Cold_elvss_vini2.Get_ELVSS_Voltages(Set);
        }


        public string[] Get_Normal_Hex_String_ELVSS(Gamma_Set Set)
        {
            return elvss_vini2.Get_Hex_String_ELVSS(Set);
        }
        public string[] Get_Cold_Hex_String_ELVSS(Gamma_Set Set)
        {
            return Cold_elvss_vini2.Get_Hex_String_ELVSS(Set);
        }


        public void Set_Normal_Dec_Vinit2(Gamma_Set Set, int band, byte New_Byte_Vinit2)
        {
            elvss_vini2.Set_Dec_Vinit2(Set, band, New_Byte_Vinit2);
        }
        public void Set_Cold_Dec_Vinit2(Gamma_Set Set, int band, byte New_Byte_Vinit2)
        {
            Cold_elvss_vini2.Set_Dec_Vinit2(Set, band, New_Byte_Vinit2);
        }


        public void Set_Normal_Voltage_Vinit2(Gamma_Set Set, int band, double New_Vinit2_Voltage)
        {
            elvss_vini2.Set_Voltage_Vinit2(Set, band, New_Vinit2_Voltage);
        }
        public void Set_Cold_Voltage_Vinit2(Gamma_Set Set, int band, double New_Vinit2_Voltage)
        {
            Cold_elvss_vini2.Set_Voltage_Vinit2(Set, band, New_Vinit2_Voltage);
        }


        public int Get_Normal_Dec_Vinit2(Gamma_Set Set, int band)
        {
            return elvss_vini2.Get_Dec_Vinit2(Set, band);
        }
        public int Get_Cold_Dec_Vinit2(Gamma_Set Set, int band)
        {
            return Cold_elvss_vini2.Get_Dec_Vinit2(Set, band);
        }


        public double Get_Normal_Voltage_Vinit2(Gamma_Set Set, int band)
        {
            return elvss_vini2.Get_Voltage_Vinit2(Set, band);
        }
        public double Get_Cold_Voltage_Vinit2(Gamma_Set Set, int band)
        {
            return Cold_elvss_vini2.Get_Voltage_Vinit2(Set, band);
        }

        public double[] Get_Normal_Vinit2_Voltages(Gamma_Set Set)
        {
            return elvss_vini2.Get_Vinit2_Voltages(Set);
        }
        public double[] Get_Cold_Vinit2_Voltages(Gamma_Set Set)
        {
            return Cold_elvss_vini2.Get_Vinit2_Voltages(Set);
        }


        public string[] Get_Normal_Hex_String_Vinit2(Gamma_Set Set)
        {
            return elvss_vini2.Get_Hex_String_Vinit2(Set);
        }

        public string[] Get_Cold_Hex_String_Vinit2(Gamma_Set Set)
        {
            return Cold_elvss_vini2.Get_Hex_String_Vinit2(Set);
        }
    }

    public class ELVSS_Vinit2
    {
        private byte[,] Dec_ELVSS = new byte[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        private double[,] Voltage_ELVSS = new double[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        public void Set_ELVSS_Dec_Voltage_AS_Zero(int Set_index, int band)
        {
            Dec_ELVSS[Set_index, band] = 0;
            Voltage_ELVSS[Set_index, band] = 0;
        }

        public void Set_Dec_ELVSS(Gamma_Set Set, int band, byte New_Dec_ELVSS)
        {
            Dec_ELVSS[Convert.ToInt16(Set), band] = New_Dec_ELVSS;
            Voltage_ELVSS[Convert.ToInt16(Set), band] = Imported_my_cpp_dll.DP213_ELVSS_Dec_to_Voltage(New_Dec_ELVSS);
        }
        public void Set_Voltage_ELVSS(Gamma_Set Set, int band, double New_ELVSS_Voltage)
        {
            Voltage_ELVSS[Convert.ToInt16(Set), band] = New_ELVSS_Voltage;
            Dec_ELVSS[Convert.ToInt16(Set), band] = Convert.ToByte(Imported_my_cpp_dll.DP213_ELVSS_Voltage_to_Dec(New_ELVSS_Voltage));
        }
        public int Get_Dec_ELVSS(Gamma_Set Set, int band)
        {
            return Dec_ELVSS[Convert.ToInt16(Set), band];
        }
        public double Get_Voltage_ELVSS(Gamma_Set Set, int band)
        {
            Voltage_ELVSS[Convert.ToInt16(Set), band] = Imported_my_cpp_dll.DP213_ELVSS_Dec_to_Voltage(Dec_ELVSS[Convert.ToInt16(Set), band]);
            return Voltage_ELVSS[Convert.ToInt16(Set), band];
        }

        public double[] Get_ELVSS_Voltages(Gamma_Set Set)
        {
            double[] ELVSS_Voltages = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int b = 0; b < DP213_Static.Max_HBM_and_Normal_Band_Amount; b++)
                ELVSS_Voltages[b] = Voltage_ELVSS[Convert.ToInt16(Set), b];

            return ELVSS_Voltages;
        }

        public string[] Get_Hex_String_ELVSS(Gamma_Set Set)
        {
            int Set_Index = Convert.ToInt16(Set);
            string[] Temp_Hex_ELVSS = new string[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int b = 0; b < DP213_Static.Max_HBM_and_Normal_Band_Amount; b++) Temp_Hex_ELVSS[b] = Dec_ELVSS[Set_Index, b].ToString("X2");
            return Temp_Hex_ELVSS;
        }

        private byte[,] Dec_Vinit2 = new byte[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        private double[,] Voltage_Vinit2 = new double[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        public void Set_Vinit2_Dec_Voltage_AS_Zero(int Set_index, int band)
        {
            Dec_Vinit2[Set_index, band] = 0;
            Voltage_Vinit2[Set_index, band] = 0;
        }

        public void Set_Dec_Vinit2(Gamma_Set Set, int band, byte New_Dec_Vinit2)
        {
            Dec_Vinit2[Convert.ToInt16(Set), band] = New_Dec_Vinit2;
            Voltage_Vinit2[Convert.ToInt16(Set), band] = Imported_my_cpp_dll.DP213_VINI2_Dec_to_Voltage(New_Dec_Vinit2);
        }
        public void Set_Voltage_Vinit2(Gamma_Set Set, int band, double New_Vinit2_Voltage)
        {
            Voltage_Vinit2[Convert.ToInt16(Set), band] = New_Vinit2_Voltage;

            int a = Imported_my_cpp_dll.DP213_VINI2_Voltage_to_Dec(New_Vinit2_Voltage);
            Dec_Vinit2[Convert.ToInt16(Set), band] = Convert.ToByte(Imported_my_cpp_dll.DP213_VINI2_Voltage_to_Dec(New_Vinit2_Voltage));

        }
        public int Get_Dec_Vinit2(Gamma_Set Set, int band)
        {
            return Dec_Vinit2[Convert.ToInt16(Set), band];
        }

        public double Get_Voltage_Vinit2(Gamma_Set Set, int band)
        {
            Voltage_Vinit2[Convert.ToInt16(Set), band] = Imported_my_cpp_dll.DP213_VINI2_Dec_to_Voltage(Dec_Vinit2[Convert.ToInt16(Set), band]);
            return Voltage_Vinit2[Convert.ToInt16(Set), band];
        }

        public double[] Get_Vinit2_Voltages(Gamma_Set Set)
        {
            double[] Vinit2_Voltages = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int b = 0; b < DP213_Static.Max_HBM_and_Normal_Band_Amount; b++)
                Vinit2_Voltages[b] = Voltage_Vinit2[Convert.ToInt16(Set), b];

            return Vinit2_Voltages;
        }

        public string[] Get_Hex_String_Vinit2(Gamma_Set Set)
        {
            int Set_Index = Convert.ToInt16(Set);
            string[] Temp_Dec_Vinit2 = new string[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int b = 0; b < DP213_Static.Max_HBM_and_Normal_Band_Amount; b++) Temp_Dec_Vinit2[b] = Dec_Vinit2[Set_Index, b].ToString("X2");
            return Temp_Dec_Vinit2;
        }
    }
}
