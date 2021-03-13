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
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public class DP213_OC_Current_Variables_Structure : DP213_forms_accessor
    {
        DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
        DP213_OC_Values_Storage storage = DP213_OC_Values_Storage.getInstance();

        //Singleton
        static private DP213_OC_Current_Variables_Structure instance = null;

        static public DP213_OC_Current_Variables_Structure getInstance()
        {
            if (instance == null) instance = new DP213_OC_Current_Variables_Structure();
            return instance;
        }

        //GB Status/Result
        public bool Optic_Compensation_Stop = false;
        public bool Optic_Compensation_Succeed = false;

        //OC Related
        public int loop_count;
        public int Initial_Vreg1;
        public RGB Prev_Band_Gray255_Gamma;

        //Compensation related(RGB)
        public RGB Gamma;
        public XYLv Measure;
        public XYLv First_Measure;
        public XYLv Target;
        public XYLv Limit;
        public XYLv Extension;
        public string Extension_Applied;
        public RGB Prev_Gamma;

        //Vreg1 related
        public bool Vreg1_Need_To_Be_Updated;
        public int Vreg1;
        public int Diff_Vreg1;
        public int Prev_Vreg1;
        public int Vreg1_First_Gamma_Red;
        public int Vreg1_First_Gamma_Blue;

        //Compensation related(Gray255 RGB)
        public RGB cur_G255_First_Gamma;

        //RGB Infinite_Loop_Detect
        public bool Infinite;
        public int Infinite_Count;
        public RGB[] Temp_Gamma; //A0,A1,A2,A3,A4,A5
        public RGB[] Diif_Gamma; //(A1-A0),(A2-A1),(A3-A2),(A4-A3),(A5-A4)
        public RGB Temp;

        //RB Vreg1_Infinite_Loop_Detect
        public bool Vreg1_Infinite;
        public int Vreg1_Infinite_Count;
        public int[] Vreg1_Value;
        public int Vreg1_Value_Temp;
        public RGB[] Vreg1_Temp_Gamma; //A0,A1,A2,A3
        public RGB[] Vreg1_Diif_Gamma; //(A1-A0),(A2-A1),(A3-A2) 
        public RGB Vreg1_Temp;

        //dll-related variables
        public bool Gamma_Out_Of_Register_Limit;
        public bool Within_Spec_Limit;

        //210113
        public bool Is_HBM_Gray255_Compensated;
        public RGB_Double HBM_Gray255_Voltage;

        private DP213_OC_Current_Variables_Structure()
        {
            //Struct RGB has constructor,so it needs to be initalized
            //Struct XYLv doesn't have constructor,so it doesn't need to be initalized
            Prev_Band_Gray255_Gamma = new RGB();
            Gamma = new RGB();
            Prev_Gamma = new RGB();
            cur_G255_First_Gamma = new RGB();
            Temp_Gamma = new RGB[6]; //A0,A1,A2,A3,A4,A5
            Diif_Gamma = new RGB[5]; //(A1-A0),(A2-A1),(A3-A2),(A4-A3),(A5-A4)
            Temp = new RGB();
            Vreg1_Value = new int[3];
            Vreg1_Temp_Gamma = new RGB[4]; //A0,A1,A2,A3
            Vreg1_Diif_Gamma = new RGB[3]; //(A1-A0),(A2-A1),(A3-A2) 
            Vreg1_Temp = new RGB();


            //210113
            Is_HBM_Gray255_Compensated = false;
            HBM_Gray255_Voltage = new RGB_Double();
        }

        public void Update_HBM_Gray255_Gamma(Gamma_Set set)
        {
            if (Is_HBM_Gray255_Compensated)
            {
                Gamma.int_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(set, band: 0), HBM_Gray255_Voltage.double_R);
                Gamma.int_G = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(set, band: 0), HBM_Gray255_Voltage.double_G);
                Gamma.int_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(set, band: 0), HBM_Gray255_Voltage.double_B);
            }
        }

        public void Update_HBM_Gray255_Gamma_and_Send(Gamma_Set set)
        {
            if(Is_HBM_Gray255_Compensated)
            {
                Gamma.int_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(set, band: 0), HBM_Gray255_Voltage.double_R);
                Gamma.int_G = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(set, band: 0), HBM_Gray255_Voltage.double_G);
                Gamma.int_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Dec(storage.Get_Voltage_VREF4095(), storage.Get_Normal_Voltage_Vreg1(set, band: 0), HBM_Gray255_Voltage.double_B);
                
                RGB AM0 = storage.Get_Band_Set_AM0(set, band: 0);
                RGB AM1 = storage.Get_Band_Set_AM1(set, band: 0);
                cmds.Set_and_Send_RGB_CMD(set, band: 0, gray: 0, Gamma, AM0, AM1);

                f1().GB_Status_AppendText_Nextline("storage.Get_Voltage_VREF4095() / storage.Get_Normal_Voltage_Vreg1(set, band: 0) :" + storage.Get_Voltage_VREF4095() + storage.Get_Normal_Voltage_Vreg1(set, band: 0), Color.Blue);
                f1().GB_Status_AppendText_Nextline("HBM_Gray255_Voltage R/G/B " + HBM_Gray255_Voltage.double_R + "/" + HBM_Gray255_Voltage.double_G + "/" + HBM_Gray255_Voltage.double_B, Color.Blue);
                f1().GB_Status_AppendText_Nextline("Gamma R/G/B : " + Gamma.int_R + "/" + Gamma.int_G + "/" + Gamma.int_B, Color.Blue);
            }
        }
    }

    public interface DP213_OC_Variables_Update_Algorithm_Interface
    {
        void Initailize();
        void Showing_Diff_and_Current_Gamma();
        void Showing_Diff_and_Current_Vreg1_and_Gamma();

        void Check_Vreg1_InfiniteLoop();
        void Check_InfiniteLoop();

        void Check_InfiniteLoop_and_Update_ExtensionApplied();
        void Check_Vreg1_InfiniteLoop_and_Update_ExtensionApplied();
        
        bool Is_Sub_OC_Should_Be_Conducted();
        void Update_Gamma_Vreg1_Target_Limit_Extension_And_All_Band_Gray_Gamma(Gamma_Set Set, OC_Mode Mode, int band, int gray);

        void Update_Target_Lv(int band, int gray, OC_Mode Mode);
    }

        

    public class DP213_OC_Variables_Update_Algorithm : DP213_forms_accessor, DP213_OC_Variables_Update_Algorithm_Interface
    {
        DP213_OC_Current_Variables_Structure vars = DP213_OC_Current_Variables_Structure.getInstance();
        DP213_OC_Values_Storage storage = DP213_OC_Values_Storage.getInstance();

        //SingleTon
        private DP213_OC_Variables_Update_Algorithm(){}
        public static DP213_OC_Variables_Update_Algorithm instance;
        public static DP213_OC_Variables_Update_Algorithm getInstance()
        {
            if (instance == null)
                instance = new DP213_OC_Variables_Update_Algorithm();

            return instance;
        }


        public void Initailize()
        {
            vars.Optic_Compensation_Succeed = false;
            vars.loop_count = 0;
            
            vars.Infinite_Count = 0;
            vars.Infinite = false;

            vars.Vreg1_Infinite_Count = 0;
            vars.Vreg1_Infinite = false;

            vars.Extension_Applied = "X";
            vars.Within_Spec_Limit = false;
            vars.Gamma_Out_Of_Register_Limit = false;
        }

        public void Showing_Diff_and_Current_Gamma()
        {
            f1().Showing_Diff_and_Current_Gamma(vars.Prev_Gamma, vars.Gamma);
        }

        public void Showing_Diff_and_Current_Vreg1_and_Gamma()
        {
            f1().Showing_Diff_and_Current_Vreg1_and_Gamma(vars.Prev_Gamma, vars.Gamma, vars.Prev_Vreg1, vars.Vreg1);
        }

        public void Check_InfiniteLoop_and_Update_ExtensionApplied()
        {
            Check_InfiniteLoop();
            RGB_OC_Extension_Applied_Update();
        }

        public void Check_Vreg1_InfiniteLoop_and_Update_ExtensionApplied()
        {
            Check_Vreg1_InfiniteLoop();
            RVregB_OC_Extension_Applied_Update();
        }

        public void Check_InfiniteLoop()
        {
            if (vars.loop_count == 0) vars.Temp_Gamma[0].Equal_Value(vars.Gamma);
            else if (vars.loop_count == 1) vars.Temp_Gamma[1].Equal_Value(vars.Gamma);
            else if (vars.loop_count == 2) vars.Temp_Gamma[2].Equal_Value(vars.Gamma);
            else if (vars.loop_count == 3) vars.Temp_Gamma[3].Equal_Value(vars.Gamma);
            else if (vars.loop_count == 4) vars.Temp_Gamma[4].Equal_Value(vars.Gamma);//Added On 200218
            else if (vars.loop_count == 5) vars.Temp_Gamma[5].Equal_Value(vars.Gamma);//Added On 200218
            else Infinite_Loop_Check_When_Loopcount_Is_Bigger_Than_5();
        }

        public void Check_Vreg1_InfiniteLoop()
        {
            if (vars.loop_count == 0) vars.Vreg1_Temp_Gamma[0].Equal_Value(vars.Gamma);
            else if (vars.loop_count == 1)
            {
                vars.Vreg1_Value[0] = vars.Vreg1;
                vars.Vreg1_Temp_Gamma[1].Equal_Value(vars.Gamma);
            }
            else if (vars.loop_count == 2)
            {
                vars.Vreg1_Value[1] = vars.Vreg1;
                vars.Vreg1_Temp_Gamma[2].Equal_Value(vars.Gamma);
            }
            else if (vars.loop_count == 3)
            {
                vars.Vreg1_Value[2] = vars.Vreg1;
                vars.Vreg1_Temp_Gamma[3].Equal_Value(vars.Gamma);
            }
            else
            {
                Vreg1_Ininite_Loop_Check_Initialize();

         
                if (Is_Vreg1_Infinite())
                {
                    vars.Vreg1_Infinite = true;
                    vars.Vreg1_Infinite_Count++;
                }
                else
                {
                    vars.Vreg1_Infinite = false;
                }

                Show_Vreg1_Infinite_and_InfiniteCount();
            }
        }

        private void Vreg1_Ininite_Loop_Check_Initialize()
        {
            vars.Vreg1_Value_Temp = vars.Vreg1_Value[1];
            vars.Vreg1_Value[0] = vars.Vreg1_Value_Temp;
            vars.Vreg1_Value_Temp = vars.Vreg1_Value[2];
            vars.Vreg1_Value[1] = vars.Vreg1_Value_Temp;
            vars.Vreg1_Value[2] = vars.Vreg1;

            vars.Vreg1_Temp.Equal_Value(vars.Vreg1_Temp_Gamma[1]);
            vars.Vreg1_Temp_Gamma[0].Equal_Value(vars.Vreg1_Temp);
            vars.Vreg1_Temp.Equal_Value(vars.Vreg1_Temp_Gamma[2]);
            vars.Vreg1_Temp_Gamma[1].Equal_Value(vars.Vreg1_Temp);
            vars.Vreg1_Temp.Equal_Value(vars.Vreg1_Temp_Gamma[3]);
            vars.Vreg1_Temp_Gamma[2].Equal_Value(vars.Vreg1_Temp);
            vars.Vreg1_Temp_Gamma[3].Equal_Value(vars.Gamma);

            vars.Vreg1_Diif_Gamma[0] = vars.Vreg1_Temp_Gamma[1] - vars.Vreg1_Temp_Gamma[0];
            vars.Vreg1_Diif_Gamma[1] = vars.Vreg1_Temp_Gamma[2] - vars.Vreg1_Temp_Gamma[1];
            vars.Vreg1_Diif_Gamma[2] = vars.Vreg1_Temp_Gamma[3] - vars.Vreg1_Temp_Gamma[2];

        }

        public void Update_Target_Lv(int band, int gray, OC_Mode Mode)
        {
            dp213_mornitoring().Update_Target_Lv( band,  gray,  Mode);
        }

        private bool Is_Vreg1_Infinite()
        {
            return ((vars.Vreg1_Value[2] == vars.Vreg1_Value[1] && vars.Vreg1_Value[1] == vars.Vreg1_Value[0]) &&
                    ((vars.Vreg1_Diif_Gamma[0].R == vars.Vreg1_Diif_Gamma[2].R && vars.Vreg1_Diif_Gamma[0].B == vars.Vreg1_Diif_Gamma[2].B) && (vars.Vreg1_Diif_Gamma[0].R != vars.Vreg1_Diif_Gamma[1].R || vars.Vreg1_Diif_Gamma[0].B != vars.Vreg1_Diif_Gamma[1].B)));
        }

        private void Show_Vreg1_Infinite_and_InfiniteCount()
        {
            if (vars.Vreg1_Infinite)
                f1().GB_Status_AppendText_Nextline("Vreg1_Infinite : " + vars.Vreg1_Infinite.ToString(), Color.Red);
            else
                f1().GB_Status_AppendText_Nextline("Vreg1_Infinite : " + vars.Vreg1_Infinite.ToString(), Color.Green);

            if (vars.Vreg1_Infinite_Count >= 3)
                f1().GB_Status_AppendText_Nextline("Vreg1_Infinite_Count = " + vars.Vreg1_Infinite_Count.ToString(), Color.Red);
            else
                f1().GB_Status_AppendText_Nextline("Vreg1_Infinite_Count = " + vars.Vreg1_Infinite_Count.ToString(), Color.Green);
        }

        private void RVregB_OC_Extension_Applied_Update()
        {
            if (vars.Vreg1_Infinite_Count >= 3)
                vars.Extension_Applied = "O";
            else
                vars.Extension_Applied = "X";
        }

        
        
        private void Infinite_Loop_Check_When_Loopcount_Is_Bigger_Than_5()
        {
            RGB_Ininite_Loop_Check_Initialize();
            
            if (Is_Infinite_True_Case1())//Added On 200218
            {
                f1().GB_Status_AppendText_Nextline("Infinite Loop Case 1", Color.Purple);
                vars.Infinite = true;
                vars.Infinite_Count++;
            }
            else if (Is_Infinite_True_Case2())
            {
                f1().GB_Status_AppendText_Nextline("Infinite Loop Case 2", Color.Blue);
                vars.Infinite = true;
                vars.Infinite_Count++;
            }
            else
            {
                vars.Infinite = false;
            }

            Show_Infinite_and_InfiniteCount();
        }

        private void RGB_Ininite_Loop_Check_Initialize()
        {
            vars.Temp.Equal_Value(vars.Temp_Gamma[1]);
            vars.Temp_Gamma[0].Equal_Value(vars.Temp);

            vars.Temp.Equal_Value(vars.Temp_Gamma[2]);
            vars.Temp_Gamma[1].Equal_Value(vars.Temp);

            vars.Temp.Equal_Value(vars.Temp_Gamma[3]);//Added On 200218
            vars.Temp_Gamma[2].Equal_Value(vars.Temp);//Added On 200218

            vars.Temp.Equal_Value(vars.Temp_Gamma[4]);//Added On 200218
            vars.Temp_Gamma[3].Equal_Value(vars.Temp);//Added On 200218

            vars.Temp.Equal_Value(vars.Temp_Gamma[5]);//Added On 200218
            vars.Temp_Gamma[4].Equal_Value(vars.Temp);//Added On 200218

            vars.Temp_Gamma[5].Equal_Value(vars.Gamma);//Added On 200218

            vars.Diif_Gamma[0] = vars.Temp_Gamma[1] - vars.Temp_Gamma[0];
            vars.Diif_Gamma[1] = vars.Temp_Gamma[2] - vars.Temp_Gamma[1];
            vars.Diif_Gamma[2] = vars.Temp_Gamma[3] - vars.Temp_Gamma[2];
            vars.Diif_Gamma[3] = vars.Temp_Gamma[4] - vars.Temp_Gamma[3];
            vars.Diif_Gamma[4] = vars.Temp_Gamma[5] - vars.Temp_Gamma[4];
        }

        private bool Is_Infinite_True_Case1()
        {
            return (vars.Diif_Gamma[2].R == vars.Diif_Gamma[4].R)
                && (vars.Diif_Gamma[2].G == vars.Diif_Gamma[4].G)
                && (vars.Diif_Gamma[2].B == vars.Diif_Gamma[4].B)
                && ((vars.Diif_Gamma[2].R != vars.Diif_Gamma[3].R) || (vars.Diif_Gamma[2].G != vars.Diif_Gamma[3].G) || (vars.Diif_Gamma[2].B != vars.Diif_Gamma[3].B))
                && (((vars.Diif_Gamma[3].int_R >= 0) && (vars.Diif_Gamma[4].int_R < 0))//Added On 200218
                    || ((vars.Diif_Gamma[3].int_R < 0) && (vars.Diif_Gamma[4].int_R >= 0))//Added On 200218
                    || ((vars.Diif_Gamma[3].int_G >= 0) && (vars.Diif_Gamma[4].int_G < 0))//Added On 200218
                    || ((vars.Diif_Gamma[3].int_G < 0) && (vars.Diif_Gamma[4].int_G >= 0))//Added On 200218
                    || ((vars.Diif_Gamma[3].int_B >= 0) && (vars.Diif_Gamma[4].int_B < 0))//Added On 200218
                    || ((vars.Diif_Gamma[3].int_B < 0) && (vars.Diif_Gamma[4].int_B >= 0)));
        }

        private bool Is_Infinite_True_Case2()
        {
            return (vars.Temp_Gamma[0].Is_RGB_Equal(vars.Temp_Gamma[3])
                && vars.Temp_Gamma[1].Is_RGB_Equal(vars.Temp_Gamma[4])
                && vars.Temp_Gamma[2].Is_RGB_Equal(vars.Temp_Gamma[5]));
        }

        private void Show_Infinite_and_InfiniteCount()
        {
            if (vars.Infinite) f1().GB_Status_AppendText_Nextline("Infinite : " + vars.Infinite.ToString(), Color.Red);
            else f1().GB_Status_AppendText_Nextline("Infinite : " + vars.Infinite.ToString(), Color.Green);

            if (vars.Infinite_Count >= 3)
                f1().GB_Status_AppendText_Nextline("Infinite_Count = " + vars.Infinite_Count.ToString(), Color.Red);
            else
                f1().GB_Status_AppendText_Nextline("Infinite_Count = " + vars.Infinite_Count.ToString(), Color.Green);
        }

        private void RGB_OC_Extension_Applied_Update()
        {
            if (vars.Infinite_Count >= 3)
                vars.Extension_Applied = "O";
            else
                vars.Extension_Applied = "X";
        }

        public bool Is_Sub_OC_Should_Be_Conducted()
        {
            return (vars.Optic_Compensation_Succeed == false && vars.Optic_Compensation_Stop == false);
        }

        public void Update_Gamma_Vreg1_Target_Limit_Extension_And_All_Band_Gray_Gamma(Gamma_Set Set, OC_Mode Mode, int band, int gray)
        {
            dp213_mornitoring().Get_Gamma_Target_Limit_Extention_and_Update_Set_All_band_gray_Gamma(band, gray, Set, Mode);
            vars.Vreg1 = storage.Get_Normal_Dec_Vreg1(Set, band);
        }
    }



    public class DP213_CMDS_Write_Read_Update_Variables : DP213_forms_accessor
    {
        //Singleton
        static private DP213_CMDS_Write_Read_Update_Variables instance = null;
        private DP213_CMDS_Write_Read_Update_Variables()
        {
            Update_Addresses_of_ELVSS_OC_Related_Textboxes();
            Update_Addresses_of_Vreg1_Textboxes();
            Update_Addresses_of_VREF0_VREF4095_Textboxes();
            crcs = DP213_CRC_Check.getInstance(dp213_form().Get_OC_Mode_Set(OC_Mode.Mode1));
            storage = DP213_OC_Values_Storage.getInstance();
        }
        static public DP213_CMDS_Write_Read_Update_Variables getInstance()
        {
            if (instance == null) 
                instance = new DP213_CMDS_Write_Read_Update_Variables();

            return instance;
        }

        protected DP213_OC_Values_Storage storage;
        private DP213_CRC_Check crcs;
        
        //REF0/REF4095 
        private TextBox textBox_VREF4095_Voltage = new TextBox();
        private TextBox textBox_VREF0_Voltage = new TextBox();

        //Vreg1
        private TextBox[,] Vreg1_Textbox_Normal_Decimal = new TextBox[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        
        //ELVSS 
        private TextBox[,] ELVSS_Textbox_Normal_Voltage = new TextBox[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        private TextBox[,] ELVSS_Offset_Textbox_Normal_Voltage = new TextBox[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        private TextBox[,] Vinit2_Textbox_Normal_Voltage = new TextBox[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        private TextBox[,] Vinit2_Offset_Textbox_Normal_Voltage = new TextBox[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];

        //----------public Functions---------------
        //Set and Send
        public void Set_and_Send_RGB_CMD(Gamma_Set Set, int band, int gray, RGB New_Gamma, RGB New_AM0, RGB New_AM1)
        {
            storage.Set_All_band_gray_Gamma(Set, band, gray, New_Gamma);
            storage.Set_Band_Set_AM0(Set, band, New_AM0);
            storage.Set_Band_Set_AM1(Set, band, New_AM1);
            Send_Gamma_AM1_AM0(Set, band);

            f1().GB_Status_AppendText_Nextline("(Set and Send) set/band/gray Gamma : (" + Set.ToString() + "/" + band.ToString() + "/" + gray.ToString() + ")"
                + New_Gamma.int_R.ToString() + "/" + New_Gamma.int_G.ToString() + "/" + New_Gamma.int_B.ToString(), Color.Black);
            f1().GB_Status_AppendText_Nextline("(Set and Send) set/band/gray AM1 : (" + Set.ToString() + "/" + band.ToString() + "/" + gray.ToString() + ")"
                + New_AM1.int_R.ToString() + "/" + New_AM1.int_G.ToString() + "/" + New_AM1.int_B.ToString(), Color.Black);
            f1().GB_Status_AppendText_Nextline("(Set and Send) set/band/gray AM0 : (" + Set.ToString() + "/" + band.ToString() + "/" + gray.ToString() + ")"
                + New_AM0.int_R.ToString() + "/" + New_AM0.int_G.ToString() + "/" + New_AM0.int_B.ToString(), Color.Black);
        }

        public void Set_and_Send_AM0(Gamma_Set Set, int band, RGB New_AM0)
        {
            storage.Set_Band_Set_AM0(Set, band, New_AM0);
            Send_Gamma_AM1_AM0(Set, band);
            f1().GB_Status_AppendText_Nextline("(Set and Send) set/band AM0 : (" + Set.ToString() + "/" + band.ToString() + ")"
                + New_AM0.int_R.ToString() + "/" + New_AM0.int_G.ToString() + "/" + New_AM0.int_B.ToString(), Color.Black);
        }

        public void Set_and_Send_AM1(Gamma_Set Set, int band, RGB New_AM1)
        {
            storage.Set_Band_Set_AM1(Set, band, New_AM1);
            Send_Gamma_AM1_AM0(Set, band);
            f1().GB_Status_AppendText_Nextline("(Set and Send) set/band AM1 : (" + Set.ToString() + "/" + band.ToString() + ")" 
                + New_AM1.int_R.ToString() + "/" + New_AM1.int_G.ToString() + "/" + New_AM1.int_B.ToString(), Color.Black);
        }

        // Set and Send and update textboxes
        public void Set_and_Send_VREF4095_and_update_Textbox(byte New_Byte_VREF4095)
        {
            Set_Dec_VREF4095_and_Update_Textboxes(New_Byte_VREF4095);
            Send_VREF0_VREF4095();
            f1().GB_Status_AppendText_Nextline("(Set and Send and update_textboxes) Dec_VREF4095 :" + New_Byte_VREF4095.ToString(), Color.Black);
        }

        public void Set_and_Send_VREF0_and_update_Textbox(byte New_Byte_VREF0)
        {
            Set_Dec_VREF0_and_Update_Textboxes(New_Byte_VREF0);
            Send_VREF0_VREF4095();
            f1().GB_Status_AppendText_Nextline("(Set and Send and update_textboxes) Dec_VREF0 :" + New_Byte_VREF0.ToString(), Color.Black);
        }

        public void Set_and_Send_Vreg1_and_update_Textbox(Gamma_Set Set, int band, int New_Vreg1)
        {
            if (New_Vreg1 > DP213_Static.Vreg1_Register_Max) New_Vreg1 = DP213_Static.Vreg1_Register_Max;
            if (New_Vreg1 < 0) New_Vreg1 = 0;

            Set_Normal_Dec_Vreg1_and_Update_Textboxes(Set, band, New_Vreg1);
            Send_Vreg1(Set);
            f1().GB_Status_AppendText_Nextline("(Set and Send and update_textboxes)  set/band Vreg1 : (" + Set.ToString() + "/" + band.ToString() + ")" + New_Vreg1.ToString(), Color.Black);
        }

        public void Set_and_Send_ELVSS_CMD_and_update_Textbox(Gamma_Set Set, int band, int New_Dec_ELVSS)
        {
            Set_Dec_ELVSS_and_and_Update_Textboxes(Set, band, New_Dec_ELVSS);
            Send_ELVSS_CMD(Set);
            f1().GB_Status_AppendText_Nextline("(Set and Send and update_textboxes) set/band Dec_ELVSS : (" + Set.ToString() + "/" + band.ToString() + ")" + New_Dec_ELVSS.ToString(), Color.Blue);
        }

        public void Set_and_Send_ELVSS_CMD_and_update_Textbox(Gamma_Set Set, int band, double Voltage_ELVSS)
        {
            Set_Voltage_ELVSS_and_and_Update_Textboxes(Set, band, Voltage_ELVSS);
            Send_ELVSS_CMD(Set);
            f1().GB_Status_AppendText_Nextline("(Set and Send and update_textboxes)  set/band Voltage_ELVSS : (" + Set.ToString() + "/" + band.ToString() + ")" + Voltage_ELVSS.ToString(), Color.Blue);
        }

        public void Set_and_Send_Vinit2_CMD_and_update_Textbox(Gamma_Set Set, int band, int New_Dec_Vinit2)
        {
            Set_Dec_Vinit2_and_and_Update_Textboxes(Set, band, New_Dec_Vinit2);
            Send_Vinit2_CMD(Set);
            f1().GB_Status_AppendText_Nextline("(Set and Send and update_textboxes) set/band Dec_Vinit2 : (" + Set.ToString() + "/" + band.ToString() + ")" + New_Dec_Vinit2.ToString(), Color.Blue);

        }
        public void Set_and_Send_Vinit2_CMD_and_update_Textbox(Gamma_Set Set, int band, double Voltage_Vinit2)
        {
            Set_Voltage_Vinit2_and_and_Update_Textboxes(Set, band, Voltage_Vinit2);
            Send_Vinit2_CMD(Set);
            f1().GB_Status_AppendText_Nextline("(Set and Send and update_textboxes) set/band Voltage_Vinit2 : (" + Set.ToString() + "/" + band.ToString() + ")" + Voltage_Vinit2.ToString(), Color.Blue);
        }

        //RGB & AM1 & AM0
        //Set and update Textboxes
        public void Set_Dec_VREF0_and_Update_Textboxes(byte New_VREF0)
        {
            storage.Set_Dec_VREF0(New_VREF0);
            Set_VREF0_Voltage_Textbox();
        }

        public void Set_Dec_VREF4095_and_Update_Textboxes(byte New_VREF4095)
        {
            storage.Set_Dec_VREF4095(New_VREF4095);
            Set_VREF4095_Voltage_Textbox();
        }

        public void Set_Normal_Dec_Vreg1_and_Update_Textboxes(Gamma_Set Set, int band, int New_Vreg1)
        {
            storage.Set_Normal_Dec_Vreg1(Set, band, New_Vreg1);
            Set_Vreg1_Dec_Textbox(Set, band, New_Vreg1);
        }

        public void Set_Dec_ELVSS_and_and_Update_Textboxes(Gamma_Set Set, int band, int Dec_ELVSS)
        {
            Set_Normal_ELVSS(Set, band, Dec_ELVSS);
            Set_ELVSS_Voltage_Textbox(Dec_ELVSS, Set, band); // int --> voltage --> string
        }

        public void Set_Voltage_ELVSS_and_and_Update_Textboxes(Gamma_Set Set, int band, double Voltage_ELVSS)
        {
            Set_Normal_ELVSS(Set, band, Voltage_ELVSS);
            Set_ELVSS_Voltage_Textbox(Voltage_ELVSS, Set, band); // int --> voltage --> string
        }

        public void Set_Dec_Vinit2_and_and_Update_Textboxes(Gamma_Set Set, int band, int VINI2_Dec)
        {
            Set_Normal_Vinit2(Set, band, VINI2_Dec);
            Set_Vinit2_Voltage_Textbox(VINI2_Dec, Set, band);
            
        }

        public void Set_Voltage_Vinit2_and_and_Update_Textboxes(Gamma_Set Set, int band, double VINI2_Voltage)
        {
            Set_Normal_Vinit2(Set, band, VINI2_Voltage);
            Set_Vinit2_Voltage_Textbox(VINI2_Voltage, Set, band);
        }
            
        
        //Read and update Textboxes
        public void Read_and_Update_REF0_REF4095_and_Textboxes()
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Normal_Read_REF0_REF4095_CMD();
            f1().MX_OTP_Read(output);
            Thread.Sleep(20);
            byte[] parameters = f1().Get_Read_Byte_Array(output[2]);

            Set_Dec_VREF4095_and_Update_Textboxes(ModelFactory.Get_DP213_Instance().Get_Normal_REF4095(parameters));
            Set_Dec_VREF0_and_Update_Textboxes(ModelFactory.Get_DP213_Instance().Get_Normal_REF0(parameters));
            Application.DoEvents();
        }

        public void Read_and_Update_AOD_REF0_REF4095()
        {
            f1().MX_OTP_Read(21, 2, "B1");
            byte[] Dec_Read_Params = f1().Get_Read_Byte_Array(2);
           
            storage.Set_AOD_Dec_VREF4095((byte)(Dec_Read_Params[0] & 0x7F));
            storage.Set_AOD_Dec_VREF0((byte)(Dec_Read_Params[1] & 0x7F));
        }


        public void Read_and_Update_Set_Vreg1_and_Textboxes(Gamma_Set Set)
        {
            byte[] parameters = Read_Vreg1(Set);

            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                int vreg1 = ModelFactory.Get_DP213_Instance().Get_Normal_Vreg1s(parameters, band);
                Set_Normal_Dec_Vreg1_and_Update_Textboxes(Set, band, vreg1);
            }
            
            Application.DoEvents();
        }

        public void Read_and_Update_AOD_Vreg1_and_Textboxes()
        {
            f1().MX_OTP_Read(4, 5, "B2");
            storage.Set_AOD_Dec_Vreg1(0, Convert.ToInt32((f1().dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1().dataGridView1.Rows[2].Cells[1].Value.ToString()), 16));
            storage.Set_AOD_Dec_Vreg1(1, Convert.ToInt32((f1().dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1().dataGridView1.Rows[3].Cells[1].Value.ToString()), 16));
            storage.Set_AOD_Dec_Vreg1(2, Convert.ToInt32((f1().dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1().dataGridView1.Rows[4].Cells[1].Value.ToString()), 16));

            dp213_form().textBox_Vreg1_A0.Text = storage.Get_AOD_Dec_Vreg1(0).ToString();
            dp213_form().textBox_Vreg1_A1.Text = storage.Get_AOD_Dec_Vreg1(1).ToString();
            dp213_form().textBox_Vreg1_A2.Text = storage.Get_AOD_Dec_Vreg1(2).ToString();
            Application.DoEvents();
        }

        public void Read_and_Update_Set_Band_AM1_AM0_Gamma(Gamma_Set Set, int band)
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Read_Gamma_AM1_AM0_CMD(Set, band);
            f1().MX_OTP_Read(output);
            byte[] parameters = f1().Get_Read_Byte_Array(output[2]);

            RGB AM1 = ModelFactory.Get_DP213_Instance().Get_AM1(parameters);
            storage.Set_Band_Set_AM1(Set, band, AM1);

            RGB AM0 = ModelFactory.Get_DP213_Instance().Get_AM0(parameters);
            storage.Set_Band_Set_AM0(Set, band, AM0);

            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
            {
                RGB Gamma = ModelFactory.Get_DP213_Instance().Get_Gamma(gray, parameters);
                storage.Set_All_band_gray_Gamma(Set, band, gray, Gamma);
            }
        }


        //Send
        public void Send_Gamma_AM1_AM0(Gamma_Set Set, int band)
        {
            RGB[] Band_Set_Gamma = storage.Get_Band_Set_Gamma(Set, band);
            RGB AM0 = storage.Get_Band_Set_AM0(Set, band);
            RGB AM1 = storage.Get_Band_Set_AM1(Set, band);

            string[] String_Hex_Gamma = Get_Hex_String_Gamma_Param(Band_Set_Gamma, AM1, AM0);
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
                crcs.Set_GammaSet_Normal_RGB_Hex_Param(Set, band, String_Hex_Gamma);
            else
                crcs.Set_AOD_RGB_Hex_Param((band - DP213_Static.Max_HBM_and_Normal_Band_Amount), String_Hex_Gamma);

            byte[][] output_cmds = ModelFactory.Get_DP213_Instance().Get_Gamma_AM1_AM0_CMD(Set, band, Band_Set_Gamma, AM1, AM0);
            f1().DP173_Long_Packet_CMD_Send(output_cmds);
           
            Thread.Sleep(20);
        }

        public void Send_Gamma_AM1_AM0_As_0x00s(Gamma_Set Set, int band)
        {
            byte[][] output_cmds = ModelFactory.Get_DP213_Instance().Get_Gamma_AM1_AM0_0x00_CMD(Set, band);
            f1().DP173_Long_Packet_CMD_Send(output_cmds);

            Thread.Sleep(20);
        }

        public void Send_VREF0_VREF4095()
        {
            crcs.Set_VREF0_VREF4095_Hex_Param(storage.Get_Hex_VREF0(), storage.Get_Hex_VREF4095());

            byte[][] Output_CMD = ModelFactory.Get_DP213_Instance().Get_REF4095_REF0_CMD(Convert.ToByte(storage.Get_Hex_VREF4095(), 16), Convert.ToByte(storage.Get_Hex_VREF0(), 16));
            f1().DP173_Long_Packet_CMD_Send(Output_CMD);
        }

        public void Send_0x00s_VREF0_VREF4095()
        {
            byte[][] Output_CMD = ModelFactory.Get_DP213_Instance().Get_REF4095_REF0_0x00_CMD();
            f1().DP173_Long_Packet_CMD_Send(Output_CMD);
        }

        public void Send_Vreg1(Gamma_Set Set)
        {
            string[] Hex_Vreg1_Params = storage.Get_Set_Hex_Normal_Vreg1(Set);
            crcs.Set_Normal_GammaSet_Vreg1_Hex_Params(Set,Hex_Vreg1_Params);

            int[] Dec_Vreg1s = storage.Get_Normal_Dec_Vreg1s(Set);
            byte[][] Output_CMD = ModelFactory.Get_DP213_Instance().Get_Vreg1_CMD(Set,Dec_Vreg1s);
            f1().DP173_Long_Packet_CMD_Send(Output_CMD);
        }

        public void Send_0x00s_Vreg1(Gamma_Set Set)
        {
            byte[][] Output_CMD = ModelFactory.Get_DP213_Instance().Get_Vreg1_0x00_CMD(Set);
            f1().DP173_Long_Packet_CMD_Send(Output_CMD);
        }

        public void Set_and_Send_AOD_Dec_Vreg1_and_update_textboxes(int band,int vreg1)
        {
            storage.Set_AOD_Dec_Vreg1(band - 12,vreg1);
           
            if (band == 12) dp213_form().textBox_Vreg1_A0.Text = storage.Get_AOD_Dec_Vreg1(0).ToString();
            else if (band == 13) dp213_form().textBox_Vreg1_A1.Text = storage.Get_AOD_Dec_Vreg1(1).ToString();
            else if (band == 14) dp213_form().textBox_Vreg1_A2.Text = storage.Get_AOD_Dec_Vreg1(2).ToString();

            int[] Band_Dec_Vreg1 = new int[3];
            string[] Hex_Vreg1_Array = new string[5];
            
            Band_Dec_Vreg1[0] = storage.Get_AOD_Dec_Vreg1(0);
            Band_Dec_Vreg1[1] = storage.Get_AOD_Dec_Vreg1(1);
            Band_Dec_Vreg1[2] = storage.Get_AOD_Dec_Vreg1(2);
           
            Hex_Vreg1_Array[0] = (((Band_Dec_Vreg1[0] & 0xF00) >> 4) + ((Band_Dec_Vreg1[1] & 0xF00) >> 8)).ToString("X2");
            Hex_Vreg1_Array[1] = ((Band_Dec_Vreg1[2] & 0xF00) >> 8).ToString("X2");            
            Hex_Vreg1_Array[2] = (Band_Dec_Vreg1[0] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[3] = (Band_Dec_Vreg1[1] & 0xFF).ToString("X2");
            Hex_Vreg1_Array[4] = (Band_Dec_Vreg1[2] & 0xFF).ToString("X2");

            f1().DP173_Long_Packet_CMD_Send(4, Hex_Vreg1_Array.Length, "B2", Hex_Vreg1_Array);
        }

        public void Set_and_Send_AOD_REF4095_REF0(byte AOD_VREF4095,int AOD_VREF0)
        {
            storage.Set_AOD_Dec_VREF4095(AOD_VREF4095);
            storage.Set_AOD_Dec_VREF0(AOD_VREF0);

            string[] Hex_Params = new string[2];
            Hex_Params[0] = AOD_VREF4095.ToString("X2");
            Hex_Params[1] = AOD_VREF0.ToString("X2");

            f1().DP173_Long_Packet_CMD_Send(21, Hex_Params.Length, "B1", Hex_Params);
        }


        //Clear
        public void ELVSS_Vinit2_Text_Clear()
        {
            foreach (TextBox element in ELVSS_Textbox_Normal_Voltage) element.Clear();
            foreach (TextBox element in Vinit2_Textbox_Normal_Voltage) element.Clear();
            Application.DoEvents();
        }

        public void VREF4095_VREF0_Clear()
        {
            textBox_VREF4095_Voltage.Clear();
            textBox_VREF0_Voltage.Clear();
        }

        public void Vreg1_Text_Clear()
        {
            foreach (TextBox element in Vreg1_Textbox_Normal_Decimal) element.Clear();
            Application.DoEvents();
        }

        public void Read_ELVSS_and_Vinit2_Voltage_and_Save_to_Textbox()
        {
            Read_And_Update_ELVSS();
            Read_and_Update_Vinit2();
        }

        public void Read_Cold_ELVSS_and_Vinit2_Voltage_and_Save_to_Textbox()
        {
            Read_Cold_And_Update_ELVSS();
            Read_Cold_and_Update_Vinit2();
        }

        public void Read_VREF0_VREF4095_Voltage_and_Save_to_Textbox()
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Normal_Read_REF0_REF4095_CMD();
            f1().MX_OTP_Read(output);
            Thread.Sleep(20);
            byte[] parameters = f1().Get_Read_Byte_Array(output[2]);

            storage.Set_Dec_VREF0(ModelFactory.Get_DP213_Instance().Get_Normal_REF0(parameters));
            storage.Set_Dec_VREF4095(ModelFactory.Get_DP213_Instance().Get_Normal_REF4095(parameters));
            Set_VREF0_Voltage_Textbox();
            Set_VREF4095_Voltage_Textbox();
            Application.DoEvents();
        }

        public void Read_Dec_Vreg1_and_Save_to_Textbox()
        {
            //Set1 ~ Set6
            Read_and_Update_Set_Vreg1_and_Textboxes(Gamma_Set.Set1);
            Read_and_Update_Set_Vreg1_and_Textboxes(Gamma_Set.Set2);
            Read_and_Update_Set_Vreg1_and_Textboxes(Gamma_Set.Set3);
            Read_and_Update_Set_Vreg1_and_Textboxes(Gamma_Set.Set4);
            Read_and_Update_Set_Vreg1_and_Textboxes(Gamma_Set.Set5);
            Read_and_Update_Set_Vreg1_and_Textboxes(Gamma_Set.Set6);
            Read_and_Update_AOD_Vreg1_and_Textboxes();
            Application.DoEvents();
        }



        

        //---------------private Functions-----------------------
        //Get Hex String[] Gamma/AM1/AM0 Params()
        private string[] Get_Hex_String_Gamma_Param(RGB[] Band_Set_Gamma, RGB AM1, RGB AM0)
        {
            int[] Dec_Gamma_Param = new int[45];

            //Red(1~15)
            Dec_Gamma_Param[0] = ((Band_Set_Gamma[0].int_R & 0x100) >> 6) + ((Band_Set_Gamma[1].int_R & 0x100) >> 7) + ((Band_Set_Gamma[2].int_R & 0x100) >> 8);
            Dec_Gamma_Param[1] = ((Band_Set_Gamma[3].int_R & 0x100) >> 1) + ((Band_Set_Gamma[4].int_R & 0x100) >> 2) + ((Band_Set_Gamma[5].int_R & 0x100) >> 3)
                + ((Band_Set_Gamma[6].int_R & 0x100) >> 4) + ((Band_Set_Gamma[7].int_R & 0x100) >> 5) + ((Band_Set_Gamma[8].int_R & 0x100) >> 6)
                + ((Band_Set_Gamma[9].int_R & 0x100) >> 7) + ((Band_Set_Gamma[10].int_R & 0x100) >> 8);
            Dec_Gamma_Param[2] = (Band_Set_Gamma[0].int_R & 0xFF);
            Dec_Gamma_Param[3] = AM1.int_R;
            Dec_Gamma_Param[4] = AM0.int_R;
            Dec_Gamma_Param[5] = (Band_Set_Gamma[1].int_R & 0xFF);
            Dec_Gamma_Param[6] = (Band_Set_Gamma[2].int_R & 0xFF);
            Dec_Gamma_Param[7] = (Band_Set_Gamma[3].int_R & 0xFF);
            Dec_Gamma_Param[8] = (Band_Set_Gamma[4].int_R & 0xFF);
            Dec_Gamma_Param[9] = (Band_Set_Gamma[5].int_R & 0xFF);
            Dec_Gamma_Param[10] = (Band_Set_Gamma[6].int_R & 0xFF);
            Dec_Gamma_Param[11] = (Band_Set_Gamma[7].int_R & 0xFF);
            Dec_Gamma_Param[12] = (Band_Set_Gamma[8].int_R & 0xFF);
            Dec_Gamma_Param[13] = (Band_Set_Gamma[9].int_R & 0xFF);
            Dec_Gamma_Param[14] = (Band_Set_Gamma[10].int_R & 0xFF);

            //Green(16~30)
            Dec_Gamma_Param[15] = ((Band_Set_Gamma[0].int_G & 0x100) >> 6) + ((Band_Set_Gamma[1].int_G & 0x100) >> 7) + ((Band_Set_Gamma[2].int_G & 0x100) >> 8);
            Dec_Gamma_Param[16] = ((Band_Set_Gamma[3].int_G & 0x100) >> 1) + ((Band_Set_Gamma[4].int_G & 0x100) >> 2) + ((Band_Set_Gamma[5].int_G & 0x100) >> 3)
                + ((Band_Set_Gamma[6].int_G & 0x100) >> 4) + ((Band_Set_Gamma[7].int_G & 0x100) >> 5) + ((Band_Set_Gamma[8].int_G & 0x100) >> 6)
                + ((Band_Set_Gamma[9].int_G & 0x100) >> 7) + ((Band_Set_Gamma[10].int_G & 0x100) >> 8);
            Dec_Gamma_Param[17] = (Band_Set_Gamma[0].int_G & 0xFF);
            Dec_Gamma_Param[18] = AM1.int_G;
            Dec_Gamma_Param[19] = AM0.int_G;
            Dec_Gamma_Param[20] = (Band_Set_Gamma[1].int_G & 0xFF);
            Dec_Gamma_Param[21] = (Band_Set_Gamma[2].int_G & 0xFF);
            Dec_Gamma_Param[22] = (Band_Set_Gamma[3].int_G & 0xFF);
            Dec_Gamma_Param[23] = (Band_Set_Gamma[4].int_G & 0xFF);
            Dec_Gamma_Param[24] = (Band_Set_Gamma[5].int_G & 0xFF);
            Dec_Gamma_Param[25] = (Band_Set_Gamma[6].int_G & 0xFF);
            Dec_Gamma_Param[26] = (Band_Set_Gamma[7].int_G & 0xFF);
            Dec_Gamma_Param[27] = (Band_Set_Gamma[8].int_G & 0xFF);
            Dec_Gamma_Param[28] = (Band_Set_Gamma[9].int_G & 0xFF);
            Dec_Gamma_Param[29] = (Band_Set_Gamma[10].int_G & 0xFF);

            //Blue(31~45)
            Dec_Gamma_Param[30] = ((Band_Set_Gamma[0].int_B & 0x100) >> 6) + ((Band_Set_Gamma[1].int_B & 0x100) >> 7) + ((Band_Set_Gamma[2].int_B & 0x100) >> 8);
            Dec_Gamma_Param[31] = ((Band_Set_Gamma[3].int_B & 0x100) >> 1) + ((Band_Set_Gamma[4].int_B & 0x100) >> 2) + ((Band_Set_Gamma[5].int_B & 0x100) >> 3)
                + ((Band_Set_Gamma[6].int_B & 0x100) >> 4) + ((Band_Set_Gamma[7].int_B & 0x100) >> 5) + ((Band_Set_Gamma[8].int_B & 0x100) >> 6)
                + ((Band_Set_Gamma[9].int_B & 0x100) >> 7) + ((Band_Set_Gamma[10].int_B & 0x100) >> 8);
            Dec_Gamma_Param[32] = (Band_Set_Gamma[0].int_B & 0xFF);
            Dec_Gamma_Param[33] = AM1.int_B;
            Dec_Gamma_Param[34] = AM0.int_B;
            Dec_Gamma_Param[35] = (Band_Set_Gamma[1].int_B & 0xFF);
            Dec_Gamma_Param[36] = (Band_Set_Gamma[2].int_B & 0xFF);
            Dec_Gamma_Param[37] = (Band_Set_Gamma[3].int_B & 0xFF);
            Dec_Gamma_Param[38] = (Band_Set_Gamma[4].int_B & 0xFF);
            Dec_Gamma_Param[39] = (Band_Set_Gamma[5].int_B & 0xFF);
            Dec_Gamma_Param[40] = (Band_Set_Gamma[6].int_B & 0xFF);
            Dec_Gamma_Param[41] = (Band_Set_Gamma[7].int_B & 0xFF);
            Dec_Gamma_Param[42] = (Band_Set_Gamma[8].int_B & 0xFF);
            Dec_Gamma_Param[43] = (Band_Set_Gamma[9].int_B & 0xFF);
            Dec_Gamma_Param[44] = (Band_Set_Gamma[10].int_B & 0xFF);

            string[] String_Hex_Gamma = new string[45];
            for (int i = 0; i < 45; i++) String_Hex_Gamma[i] = Dec_Gamma_Param[i].ToString("X2");

            return String_Hex_Gamma;
        }

        //Setter and Getter for VREF4095 Voltage
        private void Set_VREF4095_Voltage_Textbox()
        {
            textBox_VREF4095_Voltage.Text = storage.Get_Voltage_VREF4095().ToString();
            Application.DoEvents();
        }


        //Setter and Getter for ELVSS_Normal Voltage
        private void Set_ELVSS_Voltage_Textbox(double ELVSS_Voltage, Gamma_Set Set, int band)// voltage --> string
        {
            int Set_Index = Convert.ToInt16(Set);
            ELVSS_Textbox_Normal_Voltage[Set_Index, band].Text = ELVSS_Voltage.ToString();
            Application.DoEvents();

        }
        private void Set_ELVSS_Voltage_Textbox(int ELVSS_Dec, Gamma_Set Set, int band) // int --> voltage --> string
        {
            double ELVSS_Voltage = Imported_my_cpp_dll.DP213_ELVSS_Dec_to_Voltage(ELVSS_Dec);
            Set_ELVSS_Voltage_Textbox(ELVSS_Voltage, Set, band);
            Application.DoEvents();
        }
        private double Get_ELVSS_Voltage_From_Textbox(Gamma_Set Set, int band) //string --> voltage
        {
            int Set_Index = Convert.ToInt16(Set);
            double ELVSS_Voltage = Convert.ToDouble(ELVSS_Textbox_Normal_Voltage[Set_Index, band].Text);
            return ELVSS_Voltage;
        }
        public void Set_Normal_ELVSS(Gamma_Set Set, int band, byte New_Byte_ELVSS)
        {
            storage.Set_Normal_Dec_ELVSS(Set, band, New_Byte_ELVSS);
        }
        public void Set_Normal_ELVSS(Gamma_Set Set, int band, double ELVSS_Voltage)
        {
            storage.Set_Normal_Voltage_ELVSS(Set, band, ELVSS_Voltage);
        }
        public void Set_Cold_ELVSS(Gamma_Set Set, int band, byte New_Byte_ELVSS)
        {
            storage.Set_Cold_Dec_ELVSS(Set, band, New_Byte_ELVSS);
        }
        public void Set_Cold_ELVSS(Gamma_Set Set, int band, double ELVSS_Voltage)
        {
            storage.Set_Cold_Voltage_ELVSS(Set, band, ELVSS_Voltage);
        }

        public void Send_ELVSS_CMD(Gamma_Set Set)
        {
            string[] Hex_Set_ELVSS = storage.Get_Normal_Hex_String_ELVSS(Set);
            crcs.Set_GammaSet_ELVSS_Hex_Param(Set, Hex_Set_ELVSS);

            double[] ELVSS_Voltages = storage.Get_Normal_ELVSS_Voltages(Set);
            Send_ELVSS(Set, ELVSS_Voltages);
        }

        public void Send_ELVSS_0x00s_CMD(Gamma_Set Set)
        {
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_ELVSS_0x00_CMD(Set);
            f1().DP173_Long_Packet_CMD_Send(output_cmd);
        }


         private void Send_ELVSS(Gamma_Set Set, double[] ELVSS_Voltages)
         {
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_ELVSS_CMD(Set, ELVSS_Voltages);
            f1().DP173_Long_Packet_CMD_Send(output_cmd);
         }


        public void Send_Cold_ELVSS_CMD(Gamma_Set Set)
        {
            string[] Hex_Set_ELVSS = storage.Get_Cold_Hex_String_ELVSS(Set);
            crcs.Set_GammaSet_Cold_ELVSS_Hex_Param(Set, Hex_Set_ELVSS);

            double[] Cold_ELVSS_Voltages = storage.Get_Cold_ELVSS_Voltages(Set);
            Send_Cold_ELVSS(Set, Cold_ELVSS_Voltages);
        }

        public void Send_Cold_ELVSS_0x00s_CMD(Gamma_Set Set)
        {
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Cold_ELVSS_0x00_CMD(Set);
            f1().DP173_Long_Packet_CMD_Send(output_cmd);
        }

        private void Send_Cold_ELVSS(Gamma_Set Set, double[] Cold_ELVSS_Voltages)
        {
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Cold_ELVSS_CMD(Set, Cold_ELVSS_Voltages);
            f1().DP173_Long_Packet_CMD_Send(output_cmd);
        }


        private string[] Get_Hex_Set_ELVSS_or_Vinit2_0x00()
        {
            string[] Hex_Set_ELVSS = new string[12];
            for (int b = 0; b < DP213_Static.Max_HBM_and_Normal_Band_Amount; b++)
                Hex_Set_ELVSS[b] = "00";
            return Hex_Set_ELVSS;
        }



        //Setter and Getter for VINIT2_Normal Voltage
        private void Set_Vinit2_Voltage_Textbox(double VINI2_Voltage, Gamma_Set Set, int band)// voltage --> string
        {
            int Set_Index = Convert.ToInt16(Set);
            Vinit2_Textbox_Normal_Voltage[Set_Index, band].Text = VINI2_Voltage.ToString();
            Application.DoEvents();
        }

        private void Set_Vinit2_Voltage_Textbox(int VINI2_Dec, Gamma_Set Set, int band) // int --> voltage --> string
        {
            double Vinit2_Voltage = Imported_my_cpp_dll.DP213_VINI2_Dec_to_Voltage(VINI2_Dec);
            Set_Vinit2_Voltage_Textbox(Vinit2_Voltage, Set, band);
            Application.DoEvents();
        }

        private double Get_Vinit2_Voltage_From_Textbox(Gamma_Set Set, int band) //string --> voltage
        {
            int Set_Index = Convert.ToInt16(Set);
            double Vinit2_Voltage = Convert.ToDouble(Vinit2_Textbox_Normal_Voltage[Set_Index, band].Text);
            return Vinit2_Voltage;
        }

        public void Set_Normal_Vinit2(Gamma_Set Set, int band, byte New_Byte_Vinit2)
        {
            storage.Set_Normal_Dec_Vinit2(Set, band, New_Byte_Vinit2);
        }

        public void Set_Normal_Vinit2(Gamma_Set Set, int band, double Voltage_Vinit2)
        {
            storage.Set_Normal_Voltage_Vinit2(Set, band, Voltage_Vinit2);
        }

        public void Set_Cold_Vinit2(Gamma_Set Set, int band, byte New_Byte_Vinit2)
        {
            storage.Set_Cold_Dec_Vinit2(Set, band, New_Byte_Vinit2);
        }
        public void Set_Cold_Vinit2(Gamma_Set Set, int band, double Voltage_Vinit2)
        {
            storage.Set_Cold_Voltage_Vinit2(Set, band, Voltage_Vinit2);
        }

        public void Send_Vinit2_CMD(Gamma_Set Set)
        {
            string[] Hex_Set_Vinit2 = storage.Get_Normal_Hex_String_Vinit2(Set);
            crcs.Set_GammaSet_Vinit2_Hex_Param(Set, Hex_Set_Vinit2);

            double[] Vinit2_Voltages = storage.Get_Normal_Vinit2_Voltages(Set);
            Send_Vinit2(Set, Vinit2_Voltages);
        }

        public void Send_Vinit2_0x00s_CMD(Gamma_Set Set)
        {
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Vinit2_0x00_CMD(Set);
            f1().DP173_Long_Packet_CMD_Send(output_cmd);
        }

        private void Send_Vinit2(Gamma_Set Set, double[] Vinit2_Voltages)
        {
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Vinit2_CMD(Set, Vinit2_Voltages);
            f1().DP173_Long_Packet_CMD_Send(output_cmd);
        }

        public void Send_Cold_Vinit2_CMD(Gamma_Set Set)
        {
            string[] Hex_Set_Vinit2 = storage.Get_Cold_Hex_String_Vinit2(Set);
            crcs.Set_GammaSet_Cold_Vinit2_Hex_Param(Set, Hex_Set_Vinit2);

            double[] Vinit2_Voltages = storage.Get_Cold_Vinit2_Voltages(Set);
            Send_Cold_Vinit2(Set, Vinit2_Voltages);
        }

        public void Send_Cold_Vinit2_0x00s_CMD(Gamma_Set Set)
        {
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Cold_Vinit2_0x00_CMD(Set);
            f1().DP173_Long_Packet_CMD_Send(output_cmd);
        }

        private void Send_Cold_Vinit2(Gamma_Set Set, double[] Cold_Vinit2_Voltages)
        {
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Cold_Vinit2_CMD(Set, Cold_Vinit2_Voltages);
            f1().DP173_Long_Packet_CMD_Send(output_cmd);
        }

        //Setter and Getter for VREF0 Voltage
        private void Set_VREF0_Voltage_Textbox()
        {
            textBox_VREF0_Voltage.Text = storage.Get_Voltage_VREF0().ToString();
            Application.DoEvents();
        }
        private double Get_VREF0_Voltage_From_Textbox()
        {
            return Convert.ToDouble(textBox_VREF0_Voltage.Text);
        }

        private byte[] Read_ELVSS(Gamma_Set Set)
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Read_ELVSS_CMD(Set);
            f1().MX_OTP_Read(output);
            Thread.Sleep(200);
            return f1().Get_Read_Byte_Array(output[2]);
        }


        private byte[] Read_Cold_ELVSS(Gamma_Set Set)
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Read_Cold_ELVSS_CMD(Set);
            f1().MX_OTP_Read(output);
            Thread.Sleep(200);
            return f1().Get_Read_Byte_Array(output[2]);
        }

        private void update_ELVSS_from_read_data(Gamma_Set Set)
        {
            byte[] parameters = Read_ELVSS(Set);
            
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                byte ELVSS = ModelFactory.Get_DP213_Instance().Get_Byte_ELVSS(parameters, band);
                storage.Set_Normal_Dec_ELVSS(Set, band, ELVSS);
                Set_ELVSS_Voltage_Textbox(storage.Get_Normal_Dec_ELVSS(Set, band), Set, band); // int --> voltage --> string
            }
        }

        private void update_Cold_ELVSS_from_read_data(Gamma_Set Set)
        {
            byte[] parameters = Read_Cold_ELVSS(Set);

            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                byte Cold_ELVSS = ModelFactory.Get_DP213_Instance().Get_Byte_Cold_ELVSS(parameters, band);
                storage.Set_Cold_Dec_ELVSS(Set, band, Cold_ELVSS);
                Set_ELVSS_Voltage_Textbox(storage.Get_Cold_Dec_ELVSS(Set, band), Set, band); // int --> voltage --> string
            }
        }


        private void Read_And_Update_ELVSS()
        {
            update_ELVSS_from_read_data(Gamma_Set.Set1);
            update_ELVSS_from_read_data(Gamma_Set.Set2);
            update_ELVSS_from_read_data(Gamma_Set.Set3);
            update_ELVSS_from_read_data(Gamma_Set.Set4);
            update_ELVSS_from_read_data(Gamma_Set.Set5);
            update_ELVSS_from_read_data(Gamma_Set.Set6);
        }

        private void Read_Cold_And_Update_ELVSS()
        {
            update_Cold_ELVSS_from_read_data(Gamma_Set.Set1);
            update_Cold_ELVSS_from_read_data(Gamma_Set.Set2);
            update_Cold_ELVSS_from_read_data(Gamma_Set.Set3);
            update_Cold_ELVSS_from_read_data(Gamma_Set.Set4);
            update_Cold_ELVSS_from_read_data(Gamma_Set.Set5);
            update_Cold_ELVSS_from_read_data(Gamma_Set.Set6);
        }

        private byte[] Read_Vinit2(Gamma_Set Set)
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Read_Vinit2_CMD(Set);
            f1().MX_OTP_Read(output);
            Thread.Sleep(200);
            return f1().Get_Read_Byte_Array(output[2]);  
        }

        private byte[] Read_Cold_Vinit2(Gamma_Set Set)
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Read_Cold_Vinit2_CMD(Set);
            f1().MX_OTP_Read(output);
            Thread.Sleep(200);
            return f1().Get_Read_Byte_Array(output[2]);
        }

        private void update_VInit2_from_read_data(Gamma_Set Set)
        {
            byte[] parameters = Read_Vinit2(Set);

            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                byte Vinit2 = ModelFactory.Get_DP213_Instance().Get_Byte_Vinit2(parameters, band);
                storage.Set_Normal_Dec_Vinit2(Set, band, Vinit2);
                Set_Vinit2_Voltage_Textbox(storage.Get_Normal_Dec_Vinit2(Set, band), Set, band); // int --> voltage --> string
            }
        }

        private void update_Cold_VInit2_from_read_data(Gamma_Set Set)
        {
            byte[] parameters = Read_Cold_Vinit2(Set);
           
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                byte Cold_Vinit2 = ModelFactory.Get_DP213_Instance().Get_Byte_Cold_Vinit2(parameters, band);
                storage.Set_Cold_Dec_Vinit2(Set, band, Cold_Vinit2);
                Set_Vinit2_Voltage_Textbox(storage.Get_Cold_Dec_Vinit2(Set, band), Set, band); // int --> voltage --> string
            }
        }

        private void Read_and_Update_Vinit2()
        {
            update_VInit2_from_read_data(Gamma_Set.Set1);
            update_VInit2_from_read_data(Gamma_Set.Set2);
            update_VInit2_from_read_data(Gamma_Set.Set3);
            update_VInit2_from_read_data(Gamma_Set.Set4);
            update_VInit2_from_read_data(Gamma_Set.Set5);
            update_VInit2_from_read_data(Gamma_Set.Set6);
        }

        private void Read_Cold_and_Update_Vinit2()
        {
            update_Cold_VInit2_from_read_data(Gamma_Set.Set1);
            update_Cold_VInit2_from_read_data(Gamma_Set.Set2);
            update_Cold_VInit2_from_read_data(Gamma_Set.Set3);
            update_Cold_VInit2_from_read_data(Gamma_Set.Set4);
            update_Cold_VInit2_from_read_data(Gamma_Set.Set5);
            update_Cold_VInit2_from_read_data(Gamma_Set.Set6);

        }

        private void Update_Addresses_of_VREF0_VREF4095_Textboxes()
        {
            textBox_VREF4095_Voltage = dp213_form().textBox_VREF4095;
            textBox_VREF0_Voltage = dp213_form().textBox_VREF0;
        }

        private void Set_Vreg1_Dec_Textbox(Gamma_Set Set, int band,int Dec_Vreg1)
        {
            int Set_Index = Convert.ToInt16(Set);
            Vreg1_Textbox_Normal_Decimal[Set_Index, band].Text = Dec_Vreg1.ToString();
            Application.DoEvents();
        }

        private void Update_Addresses_of_Vreg1_Textboxes()
        {
            //Vreg1 Set1
            Vreg1_Textbox_Normal_Decimal[0, 0] = dp213_form().textBox_Vreg1_B0_1;
            Vreg1_Textbox_Normal_Decimal[0, 1] = dp213_form().textBox_Vreg1_B1_1;
            Vreg1_Textbox_Normal_Decimal[0, 2] = dp213_form().textBox_Vreg1_B2_1;
            Vreg1_Textbox_Normal_Decimal[0, 3] = dp213_form().textBox_Vreg1_B3_1;
            Vreg1_Textbox_Normal_Decimal[0, 4] = dp213_form().textBox_Vreg1_B4_1;
            Vreg1_Textbox_Normal_Decimal[0, 5] = dp213_form().textBox_Vreg1_B5_1;
            Vreg1_Textbox_Normal_Decimal[0, 6] = dp213_form().textBox_Vreg1_B6_1;
            Vreg1_Textbox_Normal_Decimal[0, 7] = dp213_form().textBox_Vreg1_B7_1;
            Vreg1_Textbox_Normal_Decimal[0, 8] = dp213_form().textBox_Vreg1_B8_1;
            Vreg1_Textbox_Normal_Decimal[0, 9] = dp213_form().textBox_Vreg1_B9_1;
            Vreg1_Textbox_Normal_Decimal[0, 10] = dp213_form().textBox_Vreg1_B10_1;
            Vreg1_Textbox_Normal_Decimal[0, 11] = dp213_form().textBox_Vreg1_B11_1;

            //Vreg1 Set2
            Vreg1_Textbox_Normal_Decimal[1, 0] = dp213_form().textBox_Vreg1_B0_2;
            Vreg1_Textbox_Normal_Decimal[1, 1] = dp213_form().textBox_Vreg1_B1_2;
            Vreg1_Textbox_Normal_Decimal[1, 2] = dp213_form().textBox_Vreg1_B2_2;
            Vreg1_Textbox_Normal_Decimal[1, 3] = dp213_form().textBox_Vreg1_B3_2;
            Vreg1_Textbox_Normal_Decimal[1, 4] = dp213_form().textBox_Vreg1_B4_2;
            Vreg1_Textbox_Normal_Decimal[1, 5] = dp213_form().textBox_Vreg1_B5_2;
            Vreg1_Textbox_Normal_Decimal[1, 6] = dp213_form().textBox_Vreg1_B6_2;
            Vreg1_Textbox_Normal_Decimal[1, 7] = dp213_form().textBox_Vreg1_B7_2;
            Vreg1_Textbox_Normal_Decimal[1, 8] = dp213_form().textBox_Vreg1_B8_2;
            Vreg1_Textbox_Normal_Decimal[1, 9] = dp213_form().textBox_Vreg1_B9_2;
            Vreg1_Textbox_Normal_Decimal[1, 10] = dp213_form().textBox_Vreg1_B10_2;
            Vreg1_Textbox_Normal_Decimal[1, 11] = dp213_form().textBox_Vreg1_B11_2;

            //Vreg1 Set3
            Vreg1_Textbox_Normal_Decimal[2, 0] = dp213_form().textBox_Vreg1_B0_3;
            Vreg1_Textbox_Normal_Decimal[2, 1] = dp213_form().textBox_Vreg1_B1_3;
            Vreg1_Textbox_Normal_Decimal[2, 2] = dp213_form().textBox_Vreg1_B2_3;
            Vreg1_Textbox_Normal_Decimal[2, 3] = dp213_form().textBox_Vreg1_B3_3;
            Vreg1_Textbox_Normal_Decimal[2, 4] = dp213_form().textBox_Vreg1_B4_3;
            Vreg1_Textbox_Normal_Decimal[2, 5] = dp213_form().textBox_Vreg1_B5_3;
            Vreg1_Textbox_Normal_Decimal[2, 6] = dp213_form().textBox_Vreg1_B6_3;
            Vreg1_Textbox_Normal_Decimal[2, 7] = dp213_form().textBox_Vreg1_B7_3;
            Vreg1_Textbox_Normal_Decimal[2, 8] = dp213_form().textBox_Vreg1_B8_3;
            Vreg1_Textbox_Normal_Decimal[2, 9] = dp213_form().textBox_Vreg1_B9_3;
            Vreg1_Textbox_Normal_Decimal[2, 10] = dp213_form().textBox_Vreg1_B10_3;
            Vreg1_Textbox_Normal_Decimal[2, 11] = dp213_form().textBox_Vreg1_B11_3;

            //Vreg1 Set4
            Vreg1_Textbox_Normal_Decimal[3, 0] = dp213_form().textBox_Vreg1_B0_4;
            Vreg1_Textbox_Normal_Decimal[3, 1] = dp213_form().textBox_Vreg1_B1_4;
            Vreg1_Textbox_Normal_Decimal[3, 2] = dp213_form().textBox_Vreg1_B2_4;
            Vreg1_Textbox_Normal_Decimal[3, 3] = dp213_form().textBox_Vreg1_B3_4;
            Vreg1_Textbox_Normal_Decimal[3, 4] = dp213_form().textBox_Vreg1_B4_4;
            Vreg1_Textbox_Normal_Decimal[3, 5] = dp213_form().textBox_Vreg1_B5_4;
            Vreg1_Textbox_Normal_Decimal[3, 6] = dp213_form().textBox_Vreg1_B6_4;
            Vreg1_Textbox_Normal_Decimal[3, 7] = dp213_form().textBox_Vreg1_B7_4;
            Vreg1_Textbox_Normal_Decimal[3, 8] = dp213_form().textBox_Vreg1_B8_4;
            Vreg1_Textbox_Normal_Decimal[3, 9] = dp213_form().textBox_Vreg1_B9_4;
            Vreg1_Textbox_Normal_Decimal[3, 10] = dp213_form().textBox_Vreg1_B10_4;
            Vreg1_Textbox_Normal_Decimal[3, 11] = dp213_form().textBox_Vreg1_B11_4;

            //Vreg1 Set5
            Vreg1_Textbox_Normal_Decimal[4, 0] = dp213_form().textBox_Vreg1_B0_5;
            Vreg1_Textbox_Normal_Decimal[4, 1] = dp213_form().textBox_Vreg1_B1_5;
            Vreg1_Textbox_Normal_Decimal[4, 2] = dp213_form().textBox_Vreg1_B2_5;
            Vreg1_Textbox_Normal_Decimal[4, 3] = dp213_form().textBox_Vreg1_B3_5;
            Vreg1_Textbox_Normal_Decimal[4, 4] = dp213_form().textBox_Vreg1_B4_5;
            Vreg1_Textbox_Normal_Decimal[4, 5] = dp213_form().textBox_Vreg1_B5_5;
            Vreg1_Textbox_Normal_Decimal[4, 6] = dp213_form().textBox_Vreg1_B6_5;
            Vreg1_Textbox_Normal_Decimal[4, 7] = dp213_form().textBox_Vreg1_B7_5;
            Vreg1_Textbox_Normal_Decimal[4, 8] = dp213_form().textBox_Vreg1_B8_5;
            Vreg1_Textbox_Normal_Decimal[4, 9] = dp213_form().textBox_Vreg1_B9_5;
            Vreg1_Textbox_Normal_Decimal[4, 10] = dp213_form().textBox_Vreg1_B10_5;
            Vreg1_Textbox_Normal_Decimal[4, 11] = dp213_form().textBox_Vreg1_B11_5;

            //Vreg1 Set6
            Vreg1_Textbox_Normal_Decimal[5, 0] = dp213_form().textBox_Vreg1_B0_6;
            Vreg1_Textbox_Normal_Decimal[5, 1] = dp213_form().textBox_Vreg1_B1_6;
            Vreg1_Textbox_Normal_Decimal[5, 2] = dp213_form().textBox_Vreg1_B2_6;
            Vreg1_Textbox_Normal_Decimal[5, 3] = dp213_form().textBox_Vreg1_B3_6;
            Vreg1_Textbox_Normal_Decimal[5, 4] = dp213_form().textBox_Vreg1_B4_6;
            Vreg1_Textbox_Normal_Decimal[5, 5] = dp213_form().textBox_Vreg1_B5_6;
            Vreg1_Textbox_Normal_Decimal[5, 6] = dp213_form().textBox_Vreg1_B6_6;
            Vreg1_Textbox_Normal_Decimal[5, 7] = dp213_form().textBox_Vreg1_B7_6;
            Vreg1_Textbox_Normal_Decimal[5, 8] = dp213_form().textBox_Vreg1_B8_6;
            Vreg1_Textbox_Normal_Decimal[5, 9] = dp213_form().textBox_Vreg1_B9_6;
            Vreg1_Textbox_Normal_Decimal[5, 10] = dp213_form().textBox_Vreg1_B10_6;
            Vreg1_Textbox_Normal_Decimal[5, 11] = dp213_form().textBox_Vreg1_B11_6;
        }

       

        private byte[] Read_Vreg1(Gamma_Set Set)
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Normal_Read_Vreg1_CMD(Set);
            f1().MX_OTP_Read(output);
            Thread.Sleep(200);

            return f1().Get_Read_Byte_Array(output[2]);
        }

        private void Read_AOD_Vreg1()
        {
            f1().MX_OTP_Read(4, 5, "B1");
        }


        private void Update_Addresses_of_ELVSS_OC_Related_Textboxes()
        {
            //ELVSS Set1
            ELVSS_Textbox_Normal_Voltage[0, 0] = dp213_form().textBox_ELVSS_B0_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 1] = dp213_form().textBox_ELVSS_B1_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 2] = dp213_form().textBox_ELVSS_B2_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 3] = dp213_form().textBox_ELVSS_B3_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 4] = dp213_form().textBox_ELVSS_B4_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 5] = dp213_form().textBox_ELVSS_B5_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 6] = dp213_form().textBox_ELVSS_B6_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 7] = dp213_form().textBox_ELVSS_B7_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 8] = dp213_form().textBox_ELVSS_B8_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 9] = dp213_form().textBox_ELVSS_B9_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 10] = dp213_form().textBox_ELVSS_B10_Set1;
            ELVSS_Textbox_Normal_Voltage[0, 11] = dp213_form().textBox_ELVSS_B11_Set1;

            //ELVSS Set2
            ELVSS_Textbox_Normal_Voltage[1, 0] = dp213_form().textBox_ELVSS_B0_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 1] = dp213_form().textBox_ELVSS_B1_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 2] = dp213_form().textBox_ELVSS_B2_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 3] = dp213_form().textBox_ELVSS_B3_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 4] = dp213_form().textBox_ELVSS_B4_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 5] = dp213_form().textBox_ELVSS_B5_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 6] = dp213_form().textBox_ELVSS_B6_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 7] = dp213_form().textBox_ELVSS_B7_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 8] = dp213_form().textBox_ELVSS_B8_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 9] = dp213_form().textBox_ELVSS_B9_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 10] = dp213_form().textBox_ELVSS_B10_Set2;
            ELVSS_Textbox_Normal_Voltage[1, 11] = dp213_form().textBox_ELVSS_B11_Set2;

            //ELVSS Set3
            ELVSS_Textbox_Normal_Voltage[2, 0] = dp213_form().textBox_ELVSS_B0_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 1] = dp213_form().textBox_ELVSS_B1_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 2] = dp213_form().textBox_ELVSS_B2_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 3] = dp213_form().textBox_ELVSS_B3_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 4] = dp213_form().textBox_ELVSS_B4_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 5] = dp213_form().textBox_ELVSS_B5_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 6] = dp213_form().textBox_ELVSS_B6_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 7] = dp213_form().textBox_ELVSS_B7_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 8] = dp213_form().textBox_ELVSS_B8_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 9] = dp213_form().textBox_ELVSS_B9_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 10] = dp213_form().textBox_ELVSS_B10_Set3;
            ELVSS_Textbox_Normal_Voltage[2, 11] = dp213_form().textBox_ELVSS_B11_Set3;

            //ELVSS Set4
            ELVSS_Textbox_Normal_Voltage[3, 0] = dp213_form().textBox_ELVSS_B0_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 1] = dp213_form().textBox_ELVSS_B1_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 2] = dp213_form().textBox_ELVSS_B2_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 3] = dp213_form().textBox_ELVSS_B3_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 4] = dp213_form().textBox_ELVSS_B4_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 5] = dp213_form().textBox_ELVSS_B5_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 6] = dp213_form().textBox_ELVSS_B6_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 7] = dp213_form().textBox_ELVSS_B7_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 8] = dp213_form().textBox_ELVSS_B8_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 9] = dp213_form().textBox_ELVSS_B9_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 10] = dp213_form().textBox_ELVSS_B10_Set4;
            ELVSS_Textbox_Normal_Voltage[3, 11] = dp213_form().textBox_ELVSS_B11_Set4;

            //ELVSS Set5
            ELVSS_Textbox_Normal_Voltage[4, 0] = dp213_form().textBox_ELVSS_B0_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 1] = dp213_form().textBox_ELVSS_B1_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 2] = dp213_form().textBox_ELVSS_B2_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 3] = dp213_form().textBox_ELVSS_B3_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 4] = dp213_form().textBox_ELVSS_B4_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 5] = dp213_form().textBox_ELVSS_B5_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 6] = dp213_form().textBox_ELVSS_B6_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 7] = dp213_form().textBox_ELVSS_B7_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 8] = dp213_form().textBox_ELVSS_B8_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 9] = dp213_form().textBox_ELVSS_B9_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 10] = dp213_form().textBox_ELVSS_B10_Set5;
            ELVSS_Textbox_Normal_Voltage[4, 11] = dp213_form().textBox_ELVSS_B11_Set5;

            //ELVSS Set6
            ELVSS_Textbox_Normal_Voltage[5, 0] = dp213_form().textBox_ELVSS_B0_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 1] = dp213_form().textBox_ELVSS_B1_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 2] = dp213_form().textBox_ELVSS_B2_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 3] = dp213_form().textBox_ELVSS_B3_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 4] = dp213_form().textBox_ELVSS_B4_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 5] = dp213_form().textBox_ELVSS_B5_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 6] = dp213_form().textBox_ELVSS_B6_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 7] = dp213_form().textBox_ELVSS_B7_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 8] = dp213_form().textBox_ELVSS_B8_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 9] = dp213_form().textBox_ELVSS_B9_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 10] = dp213_form().textBox_ELVSS_B10_Set6;
            ELVSS_Textbox_Normal_Voltage[5, 11] = dp213_form().textBox_ELVSS_B11_Set6;




            //ELVSS Set1 Offset
            ELVSS_Offset_Textbox_Normal_Voltage[0, 0] = dp213_form().ELVSS_B0_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 1] = dp213_form().ELVSS_B1_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 2] = dp213_form().ELVSS_B2_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 3] = dp213_form().ELVSS_B3_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 4] = dp213_form().ELVSS_B4_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 5] = dp213_form().ELVSS_B5_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 6] = dp213_form().ELVSS_B6_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 7] = dp213_form().ELVSS_B7_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 8] = dp213_form().ELVSS_B8_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 9] = dp213_form().ELVSS_B9_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 10] = dp213_form().ELVSS_B10_Offset_Set1;
            ELVSS_Offset_Textbox_Normal_Voltage[0, 11] = dp213_form().ELVSS_B11_Offset_Set1;

            //ELVSS Set2 Offset
            ELVSS_Offset_Textbox_Normal_Voltage[1, 0] = dp213_form().ELVSS_B0_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 1] = dp213_form().ELVSS_B1_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 2] = dp213_form().ELVSS_B2_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 3] = dp213_form().ELVSS_B3_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 4] = dp213_form().ELVSS_B4_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 5] = dp213_form().ELVSS_B5_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 6] = dp213_form().ELVSS_B6_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 7] = dp213_form().ELVSS_B7_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 8] = dp213_form().ELVSS_B8_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 9] = dp213_form().ELVSS_B9_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 10] = dp213_form().ELVSS_B10_Offset_Set2;
            ELVSS_Offset_Textbox_Normal_Voltage[1, 11] = dp213_form().ELVSS_B11_Offset_Set2;

            //ELVSS Set3 Offset
            ELVSS_Offset_Textbox_Normal_Voltage[2, 0] = dp213_form().ELVSS_B0_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 1] = dp213_form().ELVSS_B1_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 2] = dp213_form().ELVSS_B2_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 3] = dp213_form().ELVSS_B3_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 4] = dp213_form().ELVSS_B4_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 5] = dp213_form().ELVSS_B5_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 6] = dp213_form().ELVSS_B6_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 7] = dp213_form().ELVSS_B7_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 8] = dp213_form().ELVSS_B8_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 9] = dp213_form().ELVSS_B9_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 10] = dp213_form().ELVSS_B10_Offset_Set3;
            ELVSS_Offset_Textbox_Normal_Voltage[2, 11] = dp213_form().ELVSS_B11_Offset_Set3;

            //ELVSS Set4 Offset
            ELVSS_Offset_Textbox_Normal_Voltage[3, 0] = dp213_form().ELVSS_B0_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 1] = dp213_form().ELVSS_B1_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 2] = dp213_form().ELVSS_B2_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 3] = dp213_form().ELVSS_B3_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 4] = dp213_form().ELVSS_B4_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 5] = dp213_form().ELVSS_B5_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 6] = dp213_form().ELVSS_B6_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 7] = dp213_form().ELVSS_B7_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 8] = dp213_form().ELVSS_B8_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 9] = dp213_form().ELVSS_B9_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 10] = dp213_form().ELVSS_B10_Offset_Set4;
            ELVSS_Offset_Textbox_Normal_Voltage[3, 11] = dp213_form().ELVSS_B11_Offset_Set4;

            //ELVSS Set5 Offset
            ELVSS_Offset_Textbox_Normal_Voltage[4, 0] = dp213_form().ELVSS_B0_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 1] = dp213_form().ELVSS_B1_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 2] = dp213_form().ELVSS_B2_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 3] = dp213_form().ELVSS_B3_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 4] = dp213_form().ELVSS_B4_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 5] = dp213_form().ELVSS_B5_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 6] = dp213_form().ELVSS_B6_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 7] = dp213_form().ELVSS_B7_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 8] = dp213_form().ELVSS_B8_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 9] = dp213_form().ELVSS_B9_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 10] = dp213_form().ELVSS_B10_Offset_Set5;
            ELVSS_Offset_Textbox_Normal_Voltage[4, 11] = dp213_form().ELVSS_B11_Offset_Set5;

            //ELVSS Set6 Offset
            ELVSS_Offset_Textbox_Normal_Voltage[5, 0] = dp213_form().ELVSS_B0_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 1] = dp213_form().ELVSS_B1_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 2] = dp213_form().ELVSS_B2_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 3] = dp213_form().ELVSS_B3_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 4] = dp213_form().ELVSS_B4_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 5] = dp213_form().ELVSS_B5_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 6] = dp213_form().ELVSS_B6_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 7] = dp213_form().ELVSS_B7_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 8] = dp213_form().ELVSS_B8_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 9] = dp213_form().ELVSS_B9_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 10] = dp213_form().ELVSS_B10_Offset_Set6;
            ELVSS_Offset_Textbox_Normal_Voltage[5, 11] = dp213_form().ELVSS_B11_Offset_Set6;

            //Vinit Set1
            Vinit2_Textbox_Normal_Voltage[0, 0] = dp213_form().textBox_Vinit_B0_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 1] = dp213_form().textBox_Vinit_B1_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 2] = dp213_form().textBox_Vinit_B2_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 3] = dp213_form().textBox_Vinit_B3_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 4] = dp213_form().textBox_Vinit_B4_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 5] = dp213_form().textBox_Vinit_B5_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 6] = dp213_form().textBox_Vinit_B6_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 7] = dp213_form().textBox_Vinit_B7_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 8] = dp213_form().textBox_Vinit_B8_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 9] = dp213_form().textBox_Vinit_B9_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 10] = dp213_form().textBox_Vinit_B10_Set1;
            Vinit2_Textbox_Normal_Voltage[0, 11] = dp213_form().textBox_Vinit_B11_Set1;

            //Vinit Set2
            Vinit2_Textbox_Normal_Voltage[1, 0] = dp213_form().textBox_Vinit_B0_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 1] = dp213_form().textBox_Vinit_B1_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 2] = dp213_form().textBox_Vinit_B2_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 3] = dp213_form().textBox_Vinit_B3_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 4] = dp213_form().textBox_Vinit_B4_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 5] = dp213_form().textBox_Vinit_B5_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 6] = dp213_form().textBox_Vinit_B6_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 7] = dp213_form().textBox_Vinit_B7_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 8] = dp213_form().textBox_Vinit_B8_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 9] = dp213_form().textBox_Vinit_B9_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 10] = dp213_form().textBox_Vinit_B10_Set2;
            Vinit2_Textbox_Normal_Voltage[1, 11] = dp213_form().textBox_Vinit_B11_Set2;

            //Vinit Set3
            Vinit2_Textbox_Normal_Voltage[2, 0] = dp213_form().textBox_Vinit_B0_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 1] = dp213_form().textBox_Vinit_B1_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 2] = dp213_form().textBox_Vinit_B2_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 3] = dp213_form().textBox_Vinit_B3_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 4] = dp213_form().textBox_Vinit_B4_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 5] = dp213_form().textBox_Vinit_B5_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 6] = dp213_form().textBox_Vinit_B6_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 7] = dp213_form().textBox_Vinit_B7_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 8] = dp213_form().textBox_Vinit_B8_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 9] = dp213_form().textBox_Vinit_B9_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 10] = dp213_form().textBox_Vinit_B10_Set3;
            Vinit2_Textbox_Normal_Voltage[2, 11] = dp213_form().textBox_Vinit_B11_Set3;

            //Vinit Set4 
            Vinit2_Textbox_Normal_Voltage[3, 0] = dp213_form().textBox_Vinit_B0_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 1] = dp213_form().textBox_Vinit_B1_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 2] = dp213_form().textBox_Vinit_B2_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 3] = dp213_form().textBox_Vinit_B3_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 4] = dp213_form().textBox_Vinit_B4_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 5] = dp213_form().textBox_Vinit_B5_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 6] = dp213_form().textBox_Vinit_B6_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 7] = dp213_form().textBox_Vinit_B7_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 8] = dp213_form().textBox_Vinit_B8_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 9] = dp213_form().textBox_Vinit_B9_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 10] = dp213_form().textBox_Vinit_B10_Set4;
            Vinit2_Textbox_Normal_Voltage[3, 11] = dp213_form().textBox_Vinit_B11_Set4;


            //Vinit Set5
            Vinit2_Textbox_Normal_Voltage[4, 0] = dp213_form().textBox_Vinit_B0_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 1] = dp213_form().textBox_Vinit_B1_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 2] = dp213_form().textBox_Vinit_B2_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 3] = dp213_form().textBox_Vinit_B3_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 4] = dp213_form().textBox_Vinit_B4_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 5] = dp213_form().textBox_Vinit_B5_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 6] = dp213_form().textBox_Vinit_B6_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 7] = dp213_form().textBox_Vinit_B7_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 8] = dp213_form().textBox_Vinit_B8_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 9] = dp213_form().textBox_Vinit_B9_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 10] = dp213_form().textBox_Vinit_B10_Set5;
            Vinit2_Textbox_Normal_Voltage[4, 11] = dp213_form().textBox_Vinit_B11_Set5;

            //Vinit Set6
            Vinit2_Textbox_Normal_Voltage[5, 0] = dp213_form().textBox_Vinit_B0_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 1] = dp213_form().textBox_Vinit_B1_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 2] = dp213_form().textBox_Vinit_B2_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 3] = dp213_form().textBox_Vinit_B3_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 4] = dp213_form().textBox_Vinit_B4_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 5] = dp213_form().textBox_Vinit_B5_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 6] = dp213_form().textBox_Vinit_B6_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 7] = dp213_form().textBox_Vinit_B7_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 8] = dp213_form().textBox_Vinit_B8_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 9] = dp213_form().textBox_Vinit_B9_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 10] = dp213_form().textBox_Vinit_B10_Set6;
            Vinit2_Textbox_Normal_Voltage[5, 11] = dp213_form().textBox_Vinit_B11_Set6;



            //Vinit Offset Set1
            Vinit2_Offset_Textbox_Normal_Voltage[0, 0] = dp213_form().textBox_Vinit_B0_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 1] = dp213_form().textBox_Vinit_B1_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 2] = dp213_form().textBox_Vinit_B2_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 3] = dp213_form().textBox_Vinit_B3_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 4] = dp213_form().textBox_Vinit_B4_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 5] = dp213_form().textBox_Vinit_B5_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 6] = dp213_form().textBox_Vinit_B6_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 7] = dp213_form().textBox_Vinit_B7_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 8] = dp213_form().textBox_Vinit_B8_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 9] = dp213_form().textBox_Vinit_B9_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 10] = dp213_form().textBox_Vinit_B10_Set1;
            Vinit2_Offset_Textbox_Normal_Voltage[0, 11] = dp213_form().textBox_Vinit_B11_Set1;

            //Vinit Offset Set2
            Vinit2_Offset_Textbox_Normal_Voltage[1, 0] = dp213_form().textBox_Vinit_B0_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 1] = dp213_form().textBox_Vinit_B1_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 2] = dp213_form().textBox_Vinit_B2_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 3] = dp213_form().textBox_Vinit_B3_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 4] = dp213_form().textBox_Vinit_B4_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 5] = dp213_form().textBox_Vinit_B5_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 6] = dp213_form().textBox_Vinit_B6_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 7] = dp213_form().textBox_Vinit_B7_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 8] = dp213_form().textBox_Vinit_B8_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 9] = dp213_form().textBox_Vinit_B9_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 10] = dp213_form().textBox_Vinit_B10_Set2;
            Vinit2_Offset_Textbox_Normal_Voltage[1, 11] = dp213_form().textBox_Vinit_B11_Set2;

            //Vinit Offset Set3
            Vinit2_Offset_Textbox_Normal_Voltage[2, 0] = dp213_form().textBox_Vinit_B0_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 1] = dp213_form().textBox_Vinit_B1_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 2] = dp213_form().textBox_Vinit_B2_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 3] = dp213_form().textBox_Vinit_B3_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 4] = dp213_form().textBox_Vinit_B4_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 5] = dp213_form().textBox_Vinit_B5_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 6] = dp213_form().textBox_Vinit_B6_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 7] = dp213_form().textBox_Vinit_B7_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 8] = dp213_form().textBox_Vinit_B8_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 9] = dp213_form().textBox_Vinit_B9_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 10] = dp213_form().textBox_Vinit_B10_Set3;
            Vinit2_Offset_Textbox_Normal_Voltage[2, 11] = dp213_form().textBox_Vinit_B11_Set3;

            //Vinit Offset Set4
            Vinit2_Offset_Textbox_Normal_Voltage[3, 0] = dp213_form().textBox_Vinit_B0_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 1] = dp213_form().textBox_Vinit_B1_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 2] = dp213_form().textBox_Vinit_B2_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 3] = dp213_form().textBox_Vinit_B3_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 4] = dp213_form().textBox_Vinit_B4_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 5] = dp213_form().textBox_Vinit_B5_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 6] = dp213_form().textBox_Vinit_B6_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 7] = dp213_form().textBox_Vinit_B7_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 8] = dp213_form().textBox_Vinit_B8_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 9] = dp213_form().textBox_Vinit_B9_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 10] = dp213_form().textBox_Vinit_B10_Set4;
            Vinit2_Offset_Textbox_Normal_Voltage[3, 11] = dp213_form().textBox_Vinit_B11_Set4;

            //Vinit Offset Set5
            Vinit2_Offset_Textbox_Normal_Voltage[4, 0] = dp213_form().textBox_Vinit_B0_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 1] = dp213_form().textBox_Vinit_B1_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 2] = dp213_form().textBox_Vinit_B2_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 3] = dp213_form().textBox_Vinit_B3_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 4] = dp213_form().textBox_Vinit_B4_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 5] = dp213_form().textBox_Vinit_B5_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 6] = dp213_form().textBox_Vinit_B6_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 7] = dp213_form().textBox_Vinit_B7_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 8] = dp213_form().textBox_Vinit_B8_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 9] = dp213_form().textBox_Vinit_B9_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 10] = dp213_form().textBox_Vinit_B10_Set5;
            Vinit2_Offset_Textbox_Normal_Voltage[4, 11] = dp213_form().textBox_Vinit_B11_Set5;

            //Vinit Offset Set6
            Vinit2_Offset_Textbox_Normal_Voltage[5, 0] = dp213_form().textBox_Vinit_B0_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 1] = dp213_form().textBox_Vinit_B1_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 2] = dp213_form().textBox_Vinit_B2_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 3] = dp213_form().textBox_Vinit_B3_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 4] = dp213_form().textBox_Vinit_B4_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 5] = dp213_form().textBox_Vinit_B5_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 6] = dp213_form().textBox_Vinit_B6_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 7] = dp213_form().textBox_Vinit_B7_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 8] = dp213_form().textBox_Vinit_B8_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 9] = dp213_form().textBox_Vinit_B9_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 10] = dp213_form().textBox_Vinit_B10_Set6;
            Vinit2_Offset_Textbox_Normal_Voltage[5, 11] = dp213_form().textBox_Vinit_B11_Set6;
        }
    }
}
