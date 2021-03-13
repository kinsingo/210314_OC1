using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public class Show_DP213_Dual_Engineer_Read_Datas : DP213_Dual_Engineer_Read_Datas
    {
        Read_DP213_Datas read_data;

        public Show_DP213_Dual_Engineer_Read_Datas()
        {
            read_data = new Read_DP213_Datas();
            read_data.Read_and_Update_REF0_REF4095_Vreg1s();
            
            dp213_mornitoring().Set_ProgressBar_Max_and_Initialize_Value(DP213_Static.Max_HBM_and_Normal_Band_Amount);
        }

        public void Read_And_Update_Gridview()
        {
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                Read_And_Update_Gridview(band);
                Application.DoEvents();
            }
        }

        private void Read_And_Update_Gridview(int band)
        {
            read_data.Read_and_Update_Set_Band_AM1_AM0_Gamma(band);

            Gamma_Set Mode1_GammaSet = dp213_form().Get_OC_Mode_Set(OC_Mode.Mode1);
            Gamma_Set Mode2_GammaSet = dp213_form().Get_OC_Mode_Set(OC_Mode.Mode2);
            Gamma_Set Mode3_GammaSet = dp213_form().Get_OC_Mode_Set(OC_Mode.Mode3);
            Gamma_Set Mode4_GammaSet = dp213_form().Get_OC_Mode_Set(OC_Mode.Mode4);
            Gamma_Set Mode5_GammaSet = dp213_form().Get_OC_Mode_Set(OC_Mode.Mode5);
            Gamma_Set Mode6_GammaSet = dp213_form().Get_OC_Mode_Set(OC_Mode.Mode6);

            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
            {
                Set_Read_RGB(band, gray);
                Set_Read_RGB_Voltage(band, gray);
            }

            Set_Read_AM1(band);
            Set_Read_AM1_Voltages(band);

            Set_Read_AM0(band);
            Set_Read_AM0_Voltages(band);

            Set_Read_Vreg1(band);
            Set_Read_Vreg1_Voltage(band);

            dp213_mornitoring().Set_ProgressBar_Value(band + 1);
        }


    }

    class Read_DP213_Datas : DP213_forms_accessor
    {
        DP213_CMDS_Write_Read_Update_Variables cmds;
      
        public Read_DP213_Datas()
        {
            cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
        }

        public void Read_and_Update_REF0_REF4095_Vreg1s()
        {
            cmds.Read_and_Update_REF0_REF4095_and_Textboxes(); //(1.1) Read REF0/REF4095 For Vreg1
            cmds.Read_and_Update_AOD_REF0_REF4095();//(1.2) Read AOD REF0/REF4095
            cmds.Read_Dec_Vreg1_and_Save_to_Textbox();//(2) update Vreg1 for HBM/Normal/AOD
        }

        public void Read_and_Update_Set_Band_AM1_AM0_Gamma(int band)
        {
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(Gamma_Set.Set1, band);
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(Gamma_Set.Set2, band);
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(Gamma_Set.Set3, band);
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(Gamma_Set.Set4, band);
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(Gamma_Set.Set5, band);
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(Gamma_Set.Set6, band);
        }
    }

    public class DP213_Dual_Engineer_Read_Datas : DP213_forms_accessor
    {
        DP213_OC_Values_Storage storage;

        protected DP213_Dual_Engineer_Read_Datas()
        {
            Initalize_For_Reading();
            storage = DP213_OC_Values_Storage.getInstance(); 
        }

        private void Set_OC_Params_For_OCMode123456(int rows_index, int columns_index, string New_value)
        {
            dp213_mornitoring().Set_OC_Params_For_OCMode123456(rows_index, columns_index, New_value);
        }
        private void Set_OC_Params_For_Selected_OCMode(OC_Mode Mode, int rows_index, int columns_index, string New_value)
        {
            dp213_mornitoring().Set_OC_Params_For_Selected_OCMode(Mode, rows_index, columns_index, New_value);
        }

        private void Clear_OCMode123456()
        {
            for (int columns_index = 4; columns_index <= 16; columns_index++)
            {
                Set_OC_Params_For_OCMode123456(0, columns_index, string.Empty);
                Set_OC_Params_For_OCMode123456(1, columns_index, string.Empty);
            }

            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                {
                    for (int columns_index = 4; columns_index <= 16; columns_index++)
                    {
                        int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
                        Set_OC_Params_For_OCMode123456(rows_index, columns_index, string.Empty);

                    }
                }
            }
        }

        private void Set_Head_Rows_For_Reading()
        {
            Set_OC_Params_For_OCMode123456(0, 5, "RGB");
            Set_OC_Params_For_OCMode123456(1, 4, "R");
            Set_OC_Params_For_OCMode123456(1, 5, "G");
            Set_OC_Params_For_OCMode123456(1, 6, "B");

            Set_OC_Params_For_OCMode123456(0, 8, "Vdatas");
            Set_OC_Params_For_OCMode123456(1, 7, "R");
            Set_OC_Params_For_OCMode123456(1, 8, "G");
            Set_OC_Params_For_OCMode123456(1, 9, "B");

            Set_OC_Params_For_OCMode123456(0, 11, "AM1");
            Set_OC_Params_For_OCMode123456(1, 10, "R");
            Set_OC_Params_For_OCMode123456(1, 11, "G");
            Set_OC_Params_For_OCMode123456(1, 12, "B");

            Set_OC_Params_For_OCMode123456(0, 14, "AM0");
            Set_OC_Params_For_OCMode123456(1, 13, "R");
            Set_OC_Params_For_OCMode123456(1, 14, "G");
            Set_OC_Params_For_OCMode123456(1, 15, "B");

            Set_OC_Params_For_OCMode123456(0, 16, "Vreg1");
        }

        void Initalize_For_Reading()
        {
            Clear_OCMode123456();
            Set_Head_Rows_For_Reading();
        }

        //Set_Read_RGB
        void Set_Read_RGB(OC_Mode Mode,int band,int gray, RGB data)
        {
            data.String_Update_From_int();

            int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 4, data.R);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 5, data.G);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 6, data.B);
        }
        void Set_Read_RGB(OC_Mode Mode, int band, int gray)
        {
            Gamma_Set Set = dp213_form().Get_OC_Mode_Set(Mode);
            Set_Read_RGB(Mode, band, gray, storage.Get_All_band_gray_Gamma(Set, band, gray));
        }

        protected void Set_Read_RGB(int band, int gray)
        {
            Set_Read_RGB(OC_Mode.Mode1, band, gray);
            Set_Read_RGB(OC_Mode.Mode2, band, gray);
            Set_Read_RGB(OC_Mode.Mode3, band, gray);
            Set_Read_RGB(OC_Mode.Mode4, band, gray);
            Set_Read_RGB(OC_Mode.Mode5, band, gray);
            Set_Read_RGB(OC_Mode.Mode6, band, gray);
        }

        //Set_Read_RGB_Voltage
        void Set_Read_RGB_Voltage(OC_Mode Mode, int band, int gray, RGB_Double Vdata)
        {
            Vdata.Update_String_From_Double();

            int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 7, Vdata.R);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 8, Vdata.G);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 9, Vdata.B);
        }
        void Set_Read_RGB_Voltage(OC_Mode Mode, int band, int gray)
        {
            Gamma_Set Set = dp213_form().Get_OC_Mode_Set(Mode);
            Set_Read_RGB_Voltage(Mode, band, gray, storage.Get_Voltage_All_band_gray_Gamma(Set, band, gray));
        }
        protected void Set_Read_RGB_Voltage(int band, int gray)
        {
            Set_Read_RGB_Voltage(OC_Mode.Mode1, band, gray);
            Set_Read_RGB_Voltage(OC_Mode.Mode2, band, gray);
            Set_Read_RGB_Voltage(OC_Mode.Mode3, band, gray);
            Set_Read_RGB_Voltage(OC_Mode.Mode4, band, gray);
            Set_Read_RGB_Voltage(OC_Mode.Mode5, band, gray);
            Set_Read_RGB_Voltage(OC_Mode.Mode6, band, gray);
        }

        //Set_Read_AM1
        void Set_Read_AM1(OC_Mode Mode, int band, RGB data)
        {
            data.String_Update_From_int();

            int gray = 0;
            int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 10, data.R);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 11, data.G);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 12, data.B);
        }
        void Set_Read_AM1(OC_Mode Mode, int band)
        {
            Gamma_Set Set = dp213_form().Get_OC_Mode_Set(Mode);
            Set_Read_AM1(Mode, band, storage.Get_Band_Set_AM1(Set, band));
        }
        protected void Set_Read_AM1(int band)
        {
            Set_Read_AM1(OC_Mode.Mode1, band);
            Set_Read_AM1(OC_Mode.Mode2, band);
            Set_Read_AM1(OC_Mode.Mode3, band);
            Set_Read_AM1(OC_Mode.Mode4, band);
            Set_Read_AM1(OC_Mode.Mode5, band);
            Set_Read_AM1(OC_Mode.Mode6, band);
        }

        //Set_Read_AM1_Voltages
        void Set_Read_AM1_Voltages(OC_Mode Mode, int band, RGB_Double Vdata)
        {
            Vdata.Update_String_From_Double();

            int gray = 1;
            int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 10, Vdata.R);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 11, Vdata.G);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 12, Vdata.B);
        }
        void Set_Read_AM1_Voltages(OC_Mode Mode, int band)
        {
            Gamma_Set Set = dp213_form().Get_OC_Mode_Set(Mode);
            Set_Read_AM1_Voltages(Mode, band, storage.Get_Band_Set_Voltage_AM1(Set, band));
        }
        protected void Set_Read_AM1_Voltages(int band)
        {
            Set_Read_AM1_Voltages(OC_Mode.Mode1, band);
            Set_Read_AM1_Voltages(OC_Mode.Mode2, band);
            Set_Read_AM1_Voltages(OC_Mode.Mode3, band);
            Set_Read_AM1_Voltages(OC_Mode.Mode4, band);
            Set_Read_AM1_Voltages(OC_Mode.Mode5, band);
            Set_Read_AM1_Voltages(OC_Mode.Mode6, band);
        }



        //Set_Read_AM0
        void Set_Read_AM0(OC_Mode Mode, int band, RGB data)
        {
            data.String_Update_From_int();

            int gray = 0;
            int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 13, data.R);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 14, data.G);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 15, data.B);
        }
        void Set_Read_AM0(OC_Mode Mode, int band)
        {
            Gamma_Set Set = dp213_form().Get_OC_Mode_Set(Mode);
            Set_Read_AM0(Mode, band, storage.Get_Band_Set_AM0(Set, band));
        }
        protected void Set_Read_AM0(int band)
        {
            Set_Read_AM0(OC_Mode.Mode1, band);
            Set_Read_AM0(OC_Mode.Mode2, band);
            Set_Read_AM0(OC_Mode.Mode3, band);
            Set_Read_AM0(OC_Mode.Mode4, band);
            Set_Read_AM0(OC_Mode.Mode5, band);
            Set_Read_AM0(OC_Mode.Mode6, band);
        }

        //Set_Read_AM0_Voltages
        void Set_Read_AM0_Voltages(OC_Mode Mode, int band, RGB_Double Vdata)
        {
            Vdata.Update_String_From_Double();

            int gray = 1;
            int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
            Set_OC_Params_For_Selected_OCMode(Mode,rows_index, 13, Vdata.R);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 14, Vdata.G);
            Set_OC_Params_For_Selected_OCMode(Mode, rows_index, 15, Vdata.B);
        }
        void Set_Read_AM0_Voltages(OC_Mode Mode, int band)
        {
            Gamma_Set Set = dp213_form().Get_OC_Mode_Set(Mode);
            Set_Read_AM0_Voltages(Mode, band, storage.Get_Band_Set_Voltage_AM0(Set, band));
        }
        protected void Set_Read_AM0_Voltages(int band)
        {
            Set_Read_AM0_Voltages(OC_Mode.Mode1, band);
            Set_Read_AM0_Voltages(OC_Mode.Mode2, band);
            Set_Read_AM0_Voltages(OC_Mode.Mode3, band);
            Set_Read_AM0_Voltages(OC_Mode.Mode4, band);
            Set_Read_AM0_Voltages(OC_Mode.Mode5, band);
            Set_Read_AM0_Voltages(OC_Mode.Mode6, band);
        }

        //Set_Read_Vreg1
        void Set_Read_Vreg1(OC_Mode Mode, int band, int Vreg1)
        {
            int gray = 0;
            int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
            Set_OC_Params_For_Selected_OCMode(Mode,rows_index, 16, Vreg1.ToString());
        }
        void Set_Read_Vreg1(OC_Mode Mode, int band)
        {
            Gamma_Set Set = dp213_form().Get_OC_Mode_Set(Mode);
            Set_Read_Vreg1(Mode, band, storage.Get_Normal_Dec_Vreg1(Set, band));
        }
        protected void Set_Read_Vreg1(int band)
        {
            Set_Read_Vreg1(OC_Mode.Mode1, band);
            Set_Read_Vreg1(OC_Mode.Mode2, band);
            Set_Read_Vreg1(OC_Mode.Mode3, band);
            Set_Read_Vreg1(OC_Mode.Mode4, band);
            Set_Read_Vreg1(OC_Mode.Mode5, band);
            Set_Read_Vreg1(OC_Mode.Mode6, band);
        }

        //Set_Read_Vreg1_Voltage
        void Set_Read_Vreg1_Voltage(OC_Mode Mode, int band, double Vreg1_Voltage)
        {
            int gray = 1;
            int rows_index = (band * DP213_Static.Max_Gray_Amount) + (gray + 2);
            Set_OC_Params_For_Selected_OCMode(Mode,rows_index, 16, Vreg1_Voltage.ToString());
        }
        void Set_Read_Vreg1_Voltage(OC_Mode Mode, int band)
        {
           
            Gamma_Set Set = dp213_form().Get_OC_Mode_Set(Mode);
            Set_Read_Vreg1_Voltage(Mode, band, storage.Get_Normal_Voltage_Vreg1(Set, band));
        }
        protected void Set_Read_Vreg1_Voltage(int band)
        {
            Set_Read_Vreg1_Voltage(OC_Mode.Mode1, band);
            Set_Read_Vreg1_Voltage(OC_Mode.Mode2, band);
            Set_Read_Vreg1_Voltage(OC_Mode.Mode3, band);
            Set_Read_Vreg1_Voltage(OC_Mode.Mode4, band);
            Set_Read_Vreg1_Voltage(OC_Mode.Mode5, band);
            Set_Read_Vreg1_Voltage(OC_Mode.Mode6, band);
        }


       




        

       






    }
}
