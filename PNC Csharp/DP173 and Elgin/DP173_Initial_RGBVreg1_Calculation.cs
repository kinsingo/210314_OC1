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
    public class Initial_RGBVreg1_Calculation_EA9154
    {
        //Vdata_RGB_Related (Fast OC)
        public RGB_Double[,] Calculated_Vdata;
        private RGB_Double[,] Calculated_Vdata_Set1;
        private RGB_Double[,] Calculated_Vdata_Set2;
        private RGB_Double[,] Calculated_Vdata_Set3;
        private RGB_Double[,] Calculated_Vdata_Set4;
        private RGB_Double[,] Calculated_Vdata_Set5;
        private RGB_Double[,] Calculated_Vdata_Set6;

        public double[] Calculated_Vreg1_Voltage;
        private double[] Calculated_Vreg1_Voltage_Set1;
        private double[] Calculated_Vreg1_Voltage_Set2;
        private double[] Calculated_Vreg1_Voltage_Set3;
        private double[] Calculated_Vreg1_Voltage_Set4;
        private double[] Calculated_Vreg1_Voltage_Set5;
        private double[] Calculated_Vreg1_Voltage_Set6;
        public bool[] Selected_Band;

        public void Show_Calculated_Vdata()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            for (int i = 0; i < 11; i++)
            {
                f1.GB_Status_AppendText_Nextline("Calculated_Vreg1_Voltage[" + i.ToString() + "] : " + Calculated_Vreg1_Voltage[i].ToString(), Color.Black);
                for (int j = 0; j < 8; j++)
                {
                    f1.GB_Status_AppendText_Nextline("Calculated_Vdata_R/G/B : " + "[" + i.ToString() + "," + j.ToString() + "] : " + Calculated_Vdata[i, j].double_R.ToString() + "/" + Calculated_Vdata[i, j].double_G.ToString() + "/" + Calculated_Vdata[i, j].double_B.ToString(), Color.DarkGray);    
                }
            }
        }



        public void Show_Calculated_Vdata(Gamma_Set Set)
        {
            RGB_Double[,] Temp_Vdata = Get_Calculated_Vdata(Set);
            double[] Temp_Vreg1_Voltage = Get_Calculated_Vreg1_Voltage(Set);

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Set : " + Set.ToString(), Get_Color(Set));

            for (int i = 0; i < 11; i++)
            {
                f1.GB_Status_AppendText_Nextline("Calculated_Vreg1_Voltage[" + i.ToString() + "]_" + Set.ToString() + " : " + Temp_Vreg1_Voltage[i].ToString(), Get_Color(Set));
                for (int j = 0; j < 8; j++)
                {
                    f1.GB_Status_AppendText_Nextline("Calculated_Vdata_R/G/B : " + "[" + i.ToString() + "," + j.ToString() + "]_" + Set.ToString() + " : " + Temp_Vdata[i, j].double_R.ToString() + "/" + Temp_Vdata[i, j].double_G.ToString() + "/" + Temp_Vdata[i, j].double_B.ToString(), Color.DarkGray);
                }
            }
        }

        private Color Get_Color(Gamma_Set Set)
        {
            switch (Set)
            {
                case Gamma_Set.Set1:
                    return Color.FromArgb(155, 50, 50);
                case Gamma_Set.Set2:
                    return Color.FromArgb(155, 100, 50);
                case Gamma_Set.Set3:
                    return Color.FromArgb(75, 75, 155);
                case Gamma_Set.Set4:
                    return Color.FromArgb(50, 100, 155);
                case Gamma_Set.Set5:
                    return Color.FromArgb(100, 155, 100);
                case Gamma_Set.Set6:
                    return Color.FromArgb(50, 155, 50);
                default:
                    return Color.Black;
            }
        }



        public Initial_RGBVreg1_Calculation_EA9154()
        {
            Calculated_Vdata_Set1 = new RGB_Double[11, 8]; //for Dual or Triple
            Calculated_Vdata_Set2 = new RGB_Double[11, 8]; //for Dual or Triple
            Calculated_Vdata_Set3 = new RGB_Double[11, 8]; //for Dual or Triple
            Calculated_Vdata_Set4 = new RGB_Double[11, 8]; //for Dual or Triple
            Calculated_Vdata_Set5 = new RGB_Double[11, 8]; //for Dual or Triple
            Calculated_Vdata_Set6 = new RGB_Double[11, 8]; //for Dual or Triple

            Calculated_Vreg1_Voltage_Set1 = new double[11];//for Dual
            Calculated_Vreg1_Voltage_Set2 = new double[11];//for Dual
            Calculated_Vreg1_Voltage_Set3 = new double[11];//for Dual
            Calculated_Vreg1_Voltage_Set4 = new double[11];//for Dual
            Calculated_Vreg1_Voltage_Set5 = new double[11];//for Dual
            Calculated_Vreg1_Voltage_Set6 = new double[11];//for Dual
         
            Selected_Band = new bool[11];// Vdata_RGB_Related (Fast OC)
        }

        public void Set_Calculated_RGBVreg1_Vdata_Pointer(Gamma_Set Set)
        {
            Calculated_Vdata = Get_Calculated_Vdata(Set);
            Calculated_Vreg1_Voltage = Get_Calculated_Vreg1_Voltage(Set);
        }

        private RGB_Double[,] Get_Calculated_Vdata(Gamma_Set Set)
        {
            switch(Set)
            {
                case Gamma_Set.Set1:
                    return Calculated_Vdata_Set1;
                case Gamma_Set.Set2:
                    return Calculated_Vdata_Set2;
                case Gamma_Set.Set3:
                    return Calculated_Vdata_Set3;
                case Gamma_Set.Set4:
                    return Calculated_Vdata_Set4;
                case Gamma_Set.Set5:
                    return Calculated_Vdata_Set5;
                case Gamma_Set.Set6:
                    return Calculated_Vdata_Set6;
                default:
                    return Calculated_Vdata;
            }
        }

        private double[] Get_Calculated_Vreg1_Voltage(Gamma_Set Set)
        {
            switch (Set)
            {
                case Gamma_Set.Set1:
                    return Calculated_Vreg1_Voltage_Set1;
                case Gamma_Set.Set2:
                    return Calculated_Vreg1_Voltage_Set2;
                case Gamma_Set.Set3:
                    return Calculated_Vreg1_Voltage_Set3;
                case Gamma_Set.Set4:
                    return Calculated_Vreg1_Voltage_Set4;
                case Gamma_Set.Set5:
                    return Calculated_Vreg1_Voltage_Set5;
                case Gamma_Set.Set6:
                    return Calculated_Vreg1_Voltage_Set6;
                default:
                    return Calculated_Vreg1_Voltage;
            }
        }

        public void Update_Calculated_Vdata(DP173_or_Elgin model,Gamma_Set Set)
        {
            DP173_Model_Option_Form DP173 = (DP173_Model_Option_Form)Application.OpenForms["DP173_Model_Option_Form"];

            RGB_Double[,] Temp_Calculated_Vdata = Get_Calculated_Vdata(Set);
            double[] Temp_Calculated_Vreg1_Voltage = Get_Calculated_Vreg1_Voltage(Set);

            //-------------Added on 200316-----------
            if (model.band < 11)
            {
                //update Vreg1 , R/G/B AM2 Voltage
                if (model.gray == 0)
                {
                    int Dec_Vreg1 = DP173.DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                    Temp_Calculated_Vreg1_Voltage[model.band] = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Dec_Vreg1, DP173.Voltage_VREG1_REF2047, DP173.Voltage_VREG1_REF1635, DP173.Voltage_VREG1_REF1227, DP173.Voltage_VREG1_REF815, DP173.Voltage_VREG1_REF407, DP173.Voltage_VREG1_REF63, DP173.Voltage_VREG1_REF1);
                    Temp_Calculated_Vdata[model.band, model.gray].double_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(DP173.Voltage_VREG1_REF2047, Temp_Calculated_Vreg1_Voltage[model.band], model.Gamma.int_R);
                    Temp_Calculated_Vdata[model.band, model.gray].double_G = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(DP173.Voltage_VREG1_REF2047, Temp_Calculated_Vreg1_Voltage[model.band], model.Gamma.int_G);
                    Temp_Calculated_Vdata[model.band, model.gray].double_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(DP173.Voltage_VREG1_REF2047, Temp_Calculated_Vreg1_Voltage[model.band], model.Gamma.int_B);
                }
                //update R/G/B Normal Voltage
                else
                {
                    Temp_Calculated_Vdata[model.band, model.gray].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(DP173.Voltage_VREG1_REF2047, Temp_Calculated_Vreg1_Voltage[model.band], model.R_AM1_Dec, Temp_Calculated_Vdata[model.band, (model.gray - 1)].double_R, model.Gamma.int_R, model.gray);
                    Temp_Calculated_Vdata[model.band, model.gray].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(DP173.Voltage_VREG1_REF2047, Temp_Calculated_Vreg1_Voltage[model.band], model.G_AM1_Dec, Temp_Calculated_Vdata[model.band, (model.gray - 1)].double_G, model.Gamma.int_G, model.gray);
                    Temp_Calculated_Vdata[model.band, model.gray].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(DP173.Voltage_VREG1_REF2047, Temp_Calculated_Vreg1_Voltage[model.band], model.B_AM1_Dec, Temp_Calculated_Vdata[model.band, (model.gray - 1)].double_B, model.Gamma.int_B, model.gray);
                }
            }
            //---------------------------------------
        }

        



        public void Update_Data_Voltages_And_Skip_Band_Measures(Gamma_Set Set, DP173_or_Elgin model)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_Model_Option_Form DP173 = (DP173_Model_Option_Form)Application.OpenForms["DP173_Model_Option_Form"];

            if (model.oc_mode == OC_Single_Dual_Triple.Dual)
            {
                if (Set == Gamma_Set.Set3)
                {
                    f1.GB_Status_AppendText_Nextline("Set3 Band" + (model.band).ToString() + " Skip (Just Apply Gamma and Vreg1)", Color.Blue);
                    model.Vreg1 = DP173.DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set1);

                    for (int g = 0; g < 8; g++)
                    {
                        DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set3_Measure(model.band, g);
                        Calculated_Vdata_Set3[model.band, g] = Calculated_Vdata_Set1[model.band, g];//Set1 Vdata
                    }
                    Calculated_Vreg1_Voltage_Set3[model.band] = Calculated_Vreg1_Voltage_Set1[model.band];
                }
                else if (Set == Gamma_Set.Set4)
                {
                    f1.GB_Status_AppendText_Nextline("Set4 Band" + (model.band).ToString() + " Skip (Just Apply Gamma and Vreg1)", Color.Blue);
                    model.Vreg1 = DP173.DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set2);

                    for (int g = 0; g < 8; g++)
                    {
                        DP173_form_Dual_engineer.Dual_Copy_Set2_Measure_To_Set4_Measure(model.band, g);
                        Calculated_Vdata_Set4[model.band, g] = Calculated_Vdata_Set2[model.band, g];//Set2 Vdata
                    }
                    Calculated_Vreg1_Voltage_Set4[model.band] = Calculated_Vreg1_Voltage_Set2[model.band];
                }
                else if (Set == Gamma_Set.Set6)
                {
                    f1.GB_Status_AppendText_Nextline("Set6 Band" + (model.band).ToString() + " Skip (Just Apply Gamma and Vreg1)", Color.Blue);
                    model.Vreg1 = DP173.DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set5);

                    for (int g = 0; g < 8; g++)
                    {
                        DP173_form_Dual_engineer.Dual_Copy_Set5_Measure_To_Set6_Measure(model.band, g);
                        Calculated_Vdata_Set6[model.band, g] = Calculated_Vdata_Set5[model.band, g];//Set5 Vdata
                    }
                    Calculated_Vreg1_Voltage_Set6[model.band] = Calculated_Vreg1_Voltage_Set5[model.band];
                }
            }
            else if (model.oc_mode == OC_Single_Dual_Triple.Triple)
            {
                if (Set == Gamma_Set.Set4)
                {
                    f1.GB_Status_AppendText_Nextline("Set4 Band" + (model.band).ToString() + " Skip (Just Apply Gamma and Vreg1)", Color.Blue);
                    model.Vreg1 = DP173.DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set1);

                    for (int g = 0; g < 8; g++)
                    {
                        DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set4_Measure(model.band, g);
                        Calculated_Vdata_Set4[model.band, g] = Calculated_Vdata_Set1[model.band, g];//Set1 Vdata
                    }
                    Calculated_Vreg1_Voltage_Set4[model.band] = Calculated_Vreg1_Voltage_Set1[model.band];
                }
                else if (Set == Gamma_Set.Set5)
                {
                    f1.GB_Status_AppendText_Nextline("Set5 Band" + (model.band).ToString() + " Skip (Just Apply Gamma and Vreg1)", Color.Blue);
                    model.Vreg1 = DP173.DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set2);

                    for (int g = 0; g < 8; g++)
                    {
                        DP173_form_Dual_engineer.Dual_Copy_Set2_Measure_To_Set5_Measure(model.band, g);
                        Calculated_Vdata_Set5[model.band, g] = Calculated_Vdata_Set2[model.band, g];//Set2 Vdata
                    }
                    Calculated_Vreg1_Voltage_Set5[model.band] = Calculated_Vreg1_Voltage_Set2[model.band];
                }
                else if (Set == Gamma_Set.Set6)
                {
                    f1.GB_Status_AppendText_Nextline("Set6 Band" + (model.band).ToString() + " Skip (Just Apply Gamma and Vreg1)", Color.Blue);
                    model.Vreg1 = DP173.DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set3);

                    for (int g = 0; g < 8; g++)
                    {
                        DP173_form_Dual_engineer.Dual_Copy_Set3_Measure_To_Set6_Measure(model.band, g);
                        Calculated_Vdata_Set6[model.band, g] = Calculated_Vdata_Set3[model.band, g];//Set5 Vdata
                    }
                    Calculated_Vreg1_Voltage_Set6[model.band] = Calculated_Vreg1_Voltage_Set3[model.band];
                }
            }
        }
    }
}
