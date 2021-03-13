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
    public class DP173_or_Elgin
    {
        //Static public Variables
        static public int Max_Gray_Amount = 8;
        static public int Max_Normal_Band = 10;

        //Const Values
        public readonly int Gamma_Register_Limit;
        public readonly int Vreg1_Register_Limit;

        //RGBVreg1 OC Related
        public RGB[,] All_band_gray_Gamma;
        public int band;
        public int gray;
        public int loop_count;
        public int Vreg1_loop_count;
        public int loop_count_max;
        public int total_average_loop_count;
        public int Initial_Vreg1;
        public double Skip_Lv;
        public RGB Prev_Band_Gray255_Gamma;

        //AM0
        public string R_AM0_Hex;
        public string G_AM0_Hex;
        public string B_AM0_Hex;

        //AM1
        public string R_AM1_Hex;
        public string G_AM1_Hex;
        public string B_AM1_Hex;
        public int R_AM1_Dec = 0;
        public int G_AM1_Dec = 0;
        public int B_AM1_Dec = 0;


        //Onlt For Single Mode
        public RGB Gamma_Init;
        public RGB Cal_Gamma_Init;

        //----they were global variables before---
        //Compensation related(RGB)
        public RGB Gamma;
        public XYLv Measure;
        public XYLv Target;
        public XYLv Limit;
        public XYLv Extension;
        public RGB Prev_Gamma;

        //Vreg1 related
        public bool Vreg1_Need_To_Be_Updated;
        public int Vreg1;
        public int Diff_Vreg1;
        public int Prev_Vreg1;
        public int Vreg1_First_Gamma_Red;
        public int Vreg1_First_Gamma_Blue;

        //Compensation related(Gray255 RGB)
        public int G255_First_Gamma_Red;
        public int G255_First_Gamma_Green;
        public int G255_First_Gamma_Blue;

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

        public OC_Single_Dual_Triple oc_mode;

        //IRC_G255_Verify_OC Related
        public int IRC_G255_OC_Try_Count;
        public int IRC_G255_OC_Try_Max_Count;
        public double G255_IRC_OC_Verify_Limit_Ratio;

        public void IRC_G255_Verify_OC_Vars_Init()
        {
            DP173_Model_Option_Form model_option_form = (DP173_Model_Option_Form)Application.OpenForms["DP173_Model_Option_Form"];
            IRC_G255_OC_Try_Max_Count = Convert.ToInt16(model_option_form.textBox_G255_IRC_OC_Max_Try.Text);
            G255_IRC_OC_Verify_Limit_Ratio = Convert.ToDouble(model_option_form.textBox_G255_IRC_OC_Verify_Limit_Ratio.Text);
            IRC_G255_OC_Try_Count = 0;
        }

        public bool Is_AOD_Band()
        {
            return (band == 11 || band == 12 || band == 13);
        }

        public bool Is_Normal_Band()
        {
            return (band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                             || band == 6 || band == 7 || band == 8 || band == 9 || band == 10);
        }

        public bool Is_HBM_Band()
        {
            return (band == 0);
        }

        public DP173_or_Elgin(OC_Single_Dual_Triple mode)
        {
            DP173_Model_Option_Form model_option_form = (DP173_Model_Option_Form)Application.OpenForms["DP173_Model_Option_Form"];
            All_band_gray_Gamma = new RGB[14, 8]; //14ea Bands , 8ea Gray-points

            band = 0;
            gray = 0;
            loop_count = 0;
            Vreg1_loop_count = 0;
            this.loop_count_max = model_option_form.Get_Max_Loop_Count();
            total_average_loop_count = 0;
            Initial_Vreg1 = 0;
            this.Skip_Lv = model_option_form.Get_Skip_Lv();
            Prev_Band_Gray255_Gamma = new RGB();

            Update_SET1_HBM_AM0_Hex();
            Update_SET1_HBM_AM1_Hex();

            //Onlt For Single Mode
            Gamma_Init = new RGB();
            Cal_Gamma_Init = new RGB();

            //----they were global variables before---
            //Compensation related(RGB)
            Gamma = new RGB();
            Measure = new XYLv();
            Target = new XYLv();
            Limit = new XYLv();
            Extension = new XYLv();
            Prev_Gamma = new RGB();

            //Vreg1 related
            Vreg1_Need_To_Be_Updated = false;
            Vreg1 = 0;
            Diff_Vreg1 = 0;
            Prev_Vreg1 = 0;
            Vreg1_First_Gamma_Red = 0;
            Vreg1_First_Gamma_Blue = 0;

            G255_First_Gamma_Red = 0;
            G255_First_Gamma_Green = 0;
            G255_First_Gamma_Blue = 0;

            //RGB Infinite_Loop_Detect
            Infinite = false;
            Infinite_Count = 0;
            Temp_Gamma = new RGB[6]; //A0,A1,A2,A3,A4,A5
            Diif_Gamma = new RGB[5]; //(A1-A0),(A2-A1),(A3-A2),(A4-A3),(A5-A4)
            Temp = new RGB();

            //RB Vreg1_Infinite_Loop_Detect
            Vreg1_Infinite = false;
            Vreg1_Infinite_Count = 0;
            Vreg1_Value = new int[3];
            Vreg1_Value_Temp = 0;
            Vreg1_Temp_Gamma = new RGB[4]; //A0,A1,A2,A3
            Vreg1_Diif_Gamma = new RGB[3]; //(A1-A0),(A2-A1),(A3-A2) 
            Vreg1_Temp = new RGB();

            //dll-related variables
            Gamma_Out_Of_Register_Limit = false;
            Within_Spec_Limit = false;

            Gamma_Register_Limit = 511;
            Vreg1_Register_Limit = 2047;

            oc_mode = mode;

            IRC_G255_OC_Try_Count = 0;
            IRC_G255_OC_Try_Max_Count = 0;
            G255_IRC_OC_Verify_Limit_Ratio = 1.0;
        }

        public void Update_SET1_HBM_AM0_Hex()
        {
            DP173_Model_Option_Form model_option_form = (DP173_Model_Option_Form)Application.OpenForms["DP173_Model_Option_Form"];
            if (model_option_form.radioButton_AM0_0x00.Checked)
            {
                R_AM0_Hex = "00";
                G_AM0_Hex = "00";
                B_AM0_Hex = "00";
            }
            else
            {
                //Get Set1 HBM AM0
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                f1.MX_OTP_Read(0, 40, "B3"); Thread.Sleep(50);
                string[] Hex = new string[40];
                for (int i = 0; i < 40; i++) Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                R_AM0_Hex = f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
                G_AM0_Hex = f1.dataGridView1.Rows[18].Cells[1].Value.ToString();
                B_AM0_Hex = f1.dataGridView1.Rows[31].Cells[1].Value.ToString();
            }
        }

        public void Update_SET1_HBM_AM1_Hex()
        {
            DP173_Model_Option_Form model_option_form = (DP173_Model_Option_Form)Application.OpenForms["DP173_Model_Option_Form"];
            if (model_option_form.radioButton_AM1_Set_As_AM0_divided_by_2.Checked)
            {
                R_AM1_Hex = Convert.ToInt32(Math.Ceiling(Convert.ToInt32(R_AM0_Hex, 16) / 2.0)).ToString("X2");
                G_AM1_Hex = Convert.ToInt32(Math.Ceiling(Convert.ToInt32(G_AM0_Hex, 16) / 2.0)).ToString("X2");
                B_AM1_Hex = Convert.ToInt32(Math.Ceiling(Convert.ToInt32(B_AM0_Hex, 16) / 2.0)).ToString("X2");
            }
            else if (model_option_form.radioButton_AM1_Set_As_0x00.Checked)
            {
                R_AM1_Hex = "00";
                G_AM1_Hex = "00";
                B_AM1_Hex = "00";
            }
            else
            {
                //Get Set1 HBM AM1
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                f1.MX_OTP_Read(0, 40, "B3"); Thread.Sleep(50);
                string[] Hex = new string[40];
                for (int i = 0; i < 40; i++) Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                R_AM1_Hex = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();
                G_AM1_Hex = f1.dataGridView1.Rows[17].Cells[1].Value.ToString();
                B_AM1_Hex = f1.dataGridView1.Rows[30].Cells[1].Value.ToString();
            }
            Update_AM1_Dec_From_AM1_Hex();
        }

        public void Update_AM1_Dec_From_AM1_Hex()
        {
            R_AM1_Dec = Convert.ToInt32(R_AM1_Hex, 16);
            G_AM1_Dec = Convert.ToInt32(G_AM1_Hex, 16);
            B_AM1_Dec = Convert.ToInt32(B_AM1_Hex, 16);
        }


        public void Vreg1_Compensation()
        {
            Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                                , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);
        }

        public void Sub_Compensation()
        {
            Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                                    , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);
        }
        
    }
}
