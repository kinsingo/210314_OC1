using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices; // (Dll 사용위해 필요)
using BSQH_Csharp_Library;


namespace PNC_Csharp
{
    public partial class Meta_Model_Option_Form : Form
    {
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Get_Dll_Information();

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Sub_Compensation(int loop_count, bool Infinite_Loop, int Infinite_Loop_Count, ref int R_Gamma, ref int G_Gamma, ref int B_Gamma, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, ref bool Gamma_Out_Of_Register_Limit, ref bool Within_Spec_Limit);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Vreg1_Compensation(int loop_count, bool Vreg1_Infinite_Loop, int Vreg1_Infinite_Loop_Count, ref int Gamma_R, ref int Vreg1, ref int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double Target_X, double Target_Y, double Target_Lv
        , double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y, int Gamma_Register_Limit, int Vreg1_Register_Limit, ref bool Gamma_Or_Vreg1_Out_Of_Register_Limit, ref bool Within_Spec_Limit);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ELVSS_Compensation(ref bool ELVSS_Find_Finish, double First_ELVSS, ref double ELVSS, ref double Vinit, ref double First_Slope, double Vinit_Margin, double ELVSS_Margin
    , double Slope_Margin, double First_Measure_X, double First_Measure_Y, double First_Measure_Lv, double Measure_X, double Measure_Y, double Measure_Lv);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Meta_WRGB_Mipi_Transfer(double Measured_Wx, double Measured_Wy, double Measured_Rx, double Measured_Ry, double Measured_Gx, double Measured_Gy, double Measured_Bx, double Measured_By
    , double Target_Wx, double Target_Wy, double Target_Rx, double Target_Ry, double Target_Gx, double Target_Gy, double Target_Bx, double Target_By
    , ref IntPtr Mipi_Wx, ref IntPtr Mipi_Wy, ref IntPtr Mipi_Rx, ref IntPtr Mipi_Ry, ref IntPtr Mipi_Gx, ref IntPtr Mipi_Gy, ref IntPtr Mipi_Bx, ref IntPtr Mipi_By);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_Meta_SW43417_AM0_Resolution(string FV1, string VCI1);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_E7 (double ELVDD,int dec_FV1);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_F7 (double ELVDD,int dec_VCI1);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_VREG1_REF818_volt (double E7,double F7,int Dec_VREG1_REF818);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_VREG1_REF614_volt (double E7,double F7,int Dec_VREG1_REF614);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_VREG1_REF409_volt(double E7, double F7, int Dec_VREG1_REF409);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_VREG1_REF205_volt(double E7, double F7, int Dec_VREG1_REF205);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Meta_Get_Normal_Gamma_Voltage(double L, double H, int Gamma_Dec);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Meta_Get_AM2_Voltage(double F7, double Vreg1_Voltage, int Gamma_Dec);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Meta_Get_Gamma_From_Normal_Voltage(double L, double H, double Vdata);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Meta_Get_Gamma_From_AM2_Voltage(double F7, double Vreg1_Voltage, double Vdata);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Meta_Get_Vreg1_Voltage(int Dec_Vreg1, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Dll_Meta_Get_Normal_Initial_Vreg1_Fx_HBM([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]bool[] Selected_Band, int[] HBM_Gamma_Green, int Vreg1_Dec_Init, int band, double band_Target_Lv, int Previous_Band_G255_Green_Gamma, int Previous_Band_Vreg1_Dec, double[] HBM_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt
                    , double VREG1_REF205_volt, double F7);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Dll_Meta_Get_Normal_Initial_Vreg1_R_B_Fx_Previous_Band(ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
           , double[] Previous_Band_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Dll_Meta_Get_Normal_Initial_Vreg1_Fx_Previous_Band([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]bool[] Selected_Band, int[] Previous_Band_Gamma_Green, int Vreg1_Dec_Init, int band, double band_Target_Lv, int Previous_Band_G255_Green_Gamma, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Dll_Meta_Get_First_Gamma_Fx_HBM(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]bool[] Selected_Band, int[] HBM_Gamma_R, int[] HBM_Gamma_G, int[] HBM_Gamma_B, double[] HBM_Target_Lv,
            int Current_Band_Dec_Vreg1, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);

        //Parameters for Gamma Read/Write
        string Current_Band_Gamma_Register = string.Empty;
        int Current_Gamma_Gray = 255;

        //OTP
        string label_OTP_Status;

        //Gamma control
        string[] Gamma_Binary_data = new string[33]; //Gamma 10ea 밴드는 33개의 parameter 만사용함으로 33 개 할당.
        string[] Gamma_Hex_data = new string[33];
        RGB[] Gamma_dex = new RGB[10]; //Band 는 총 10개 이므로 10개 할당
        RGB Gray255 = new RGB();
        RGB Gray191 = new RGB();
        RGB Gray127 = new RGB();
        RGB Gray63 = new RGB();
        RGB Gray31 = new RGB();
        RGB Gray15 = new RGB();
        RGB Gray7 = new RGB();
        RGB Gray4 = new RGB();
        RGB Gray1 = new RGB();
        public RGB[,] All_band_gray_Gamma = new RGB[12, 8]; //12ea Bands , 8ea Gray-points

        //GB Status/Result
        bool Optic_Compensation_Stop = false;
        bool Optic_Compensation_Succeed = false;
        bool Gamma_Out_Of_Register_Limit = false;
        bool Within_Spec_Limit = false;

        //Vreg1 Related
        string[] D1_Vreg1_Params = new string[16];
        bool Vreg1_Need_To_Be_Updated = false;
        int Vreg1;
        int Prev_Vreg1;
        int Diff_Vreg1;
        int Vreg1_First_Gamma_Red;
        int Vreg1_First_Gamma_Blue;

        //Compensation related
        RGB Gamma;
        XYLv Measure;
        XYLv Target;
        XYLv Limit;
        XYLv Extension;
        RGB Prev_Gamma;
        RGB Diff_Gamma;

        //RGB Infinite_Loop_Detect
        bool Infinite = false;
        int Infinite_Count = 0;
        RGB[] Temp_Gamma = new RGB[4]; //A0,A1,A2,A3
        RGB[] Diif_Gamma = new RGB[3]; //(A1-A0),(A2-A1),(A3-A2) 
        RGB Temp = new RGB();

        //RB Vreg1_Infinite_Loop_Detect
        bool Vreg1_Infinite = false;
        int Vreg1_Infinite_Count = 0;
        int[] Vreg1_Value = new int[3];
        int Vreg1_Value_Temp;
        RGB[] Vreg1_Temp_Gamma = new RGB[4]; //A0,A1,A2,A3
        RGB[] Vreg1_Diif_Gamma = new RGB[3]; //(A1-A0),(A2-A1),(A3-A2) 
        RGB Vreg1_Temp = new RGB();

        //Extension
        string Extension_Applied = "X";

        //dll-related variables
        int Normal_Gamma_Register_Limit = 511; //Gray7~Gray255
        int Vreg1_Register_Limit = 1023;

        //RGB Gamma Boolen
        bool[] RGB_Need_To_Change = new bool[3];

        // <summary : Gamma Test Param> 
        int Selected_Band_Index = 0;
        int Selected_Gray_Index = 0;

        // Vdata_RGB_Related (Fast OC)
        RGB_Double[,] Calculated_Vdata = new RGB_Double[10, 8];
        bool[] Selected_Band = new bool[10];

        private static Meta_Model_Option_Form Instance;
        public static Meta_Model_Option_Form getInstance()
        {
            if (Instance == null)
                Instance = new Meta_Model_Option_Form();
            return Instance;
        }

        private Meta_Model_Option_Form()
        {
            InitializeComponent();
        }

        public static bool IsIstanceNull()
        {
            if (Instance == null)
                return true;
            else
                return false;
        }

        public static void DeleteInstance()
        {
            Instance = null;
        }

        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        private void Meta_Model_Option_Form_Load(object sender, EventArgs e)
        {
            BackColor = f1().current_model.Get_Back_Ground_Color();
            radioButton_band0.Checked = true;//Band0 Select for Gamma Write/Read Mode
            radioButton_Gray255.Checked = true;//Gray255 Select for Gamma Write/Read Mode
        }

        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public bool Get_IS_G2G_On()
        {
            if (radioButton_G2G_On.Checked) return true;
            else return false;
        }

  

        private void Clear_Gamma_Read_Write_TextBox()
        {
            textBox_Gamma_Write_R.Text = textBox_Gamma_Write_G.Text = textBox_Gamma_Write_B.Text = string.Empty;
            textBox_Gamma_Read_R.Text = textBox_Gamma_Read_G.Text = textBox_Gamma_Read_B.Text = string.Empty;
        }

        private void radioButton_band0_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band0.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "D2";
                Selected_Band_Index = 0;
                
                f1.GB_Status_AppendText_Nextline("(Band0)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band1.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "D3";
                Selected_Band_Index = 1;

                f1.GB_Status_AppendText_Nextline("(Band1)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band2.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "D4";
                Selected_Band_Index = 2;

                f1.GB_Status_AppendText_Nextline("(Band2)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band3.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "D5";
                Selected_Band_Index = 3;

                f1.GB_Status_AppendText_Nextline("(Band3)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band4.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "D6";
                Selected_Band_Index = 4;

                f1.GB_Status_AppendText_Nextline("(Band4)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band5.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "D7";
                Selected_Band_Index = 5;

                f1.GB_Status_AppendText_Nextline("(Band5)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band6.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "D8";
                Selected_Band_Index = 6;

                f1.GB_Status_AppendText_Nextline("(Band6)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band7.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "D9";
                Selected_Band_Index = 7;

                f1.GB_Status_AppendText_Nextline("(Band7)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band8.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "DD";
                Selected_Band_Index = 8;

                f1.GB_Status_AppendText_Nextline("(Band8)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_band9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_band9.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Band_Gamma_Register = "DE";
                Selected_Band_Index = 9;

                f1.GB_Status_AppendText_Nextline("(Band9)Meta Current Gamma Register is " + Current_Band_Gamma_Register, Color.Blue);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray255_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray255.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 255;
                Selected_Gray_Index = 0;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray191_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray191.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 191;
                Selected_Gray_Index = 1;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray127_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray127.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 127;
                Selected_Gray_Index = 2;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray63_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray63.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 63;
                Selected_Gray_Index = 3;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray31_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray31.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 31;
                Selected_Gray_Index = 4;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray15_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray15.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 15;
                Selected_Gray_Index = 5;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray7.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 7;
                Selected_Gray_Index = 6;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray4.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 4;
                Selected_Gray_Index = 7;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void radioButton_Gray1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Gray1.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Current_Gamma_Gray = 1;
                Selected_Gray_Index = 8;

                f1.GB_Status_AppendText_Nextline("Current Gamma Gray is " + Current_Gamma_Gray.ToString(), Color.Black);
                Clear_Gamma_Read_Write_TextBox();
            }
        }

        private void Get_Gray_Gamma()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.OTP_Read(33, Current_Band_Gamma_Register);
            for (int i = 0; i < 33; i++)
            {
                Gamma_Hex_data[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Gamma_Binary_data[i] = (Convert.ToString(Convert.ToInt32(Gamma_Hex_data[i], 16), 2)).PadLeft(8, '0');
            }

            //Gray 별 Binary Values
            //R
            Gray255.int_R = Convert.ToInt16((Gamma_Binary_data[0][0] + Gamma_Binary_data[1]), 2); 
            Gray191.int_R = Convert.ToInt16((Gamma_Binary_data[2][0] + Gamma_Binary_data[10]), 2); 
            Gray127.int_R = Convert.ToInt16((Gamma_Binary_data[2][1] + Gamma_Binary_data[9]), 2);
            Gray63.int_R = Convert.ToInt16((Gamma_Binary_data[2][2] + Gamma_Binary_data[8]), 2);
            Gray31.int_R = Convert.ToInt16((Gamma_Binary_data[2][3] + Gamma_Binary_data[7]), 2);
            Gray15.int_R = Convert.ToInt16((Gamma_Binary_data[2][4] + Gamma_Binary_data[6]), 2);
            Gray7.int_R = Convert.ToInt16((Gamma_Binary_data[2][5] + Gamma_Binary_data[5]), 2);
            Gray4.int_R = Convert.ToInt16((Gamma_Binary_data[2][6] + Gamma_Binary_data[4]), 2);
            Gray1.int_R = Convert.ToInt16((Gamma_Binary_data[2][7] + Gamma_Binary_data[3]), 2);

            //G
            int Offset = 11;
            Gray255.int_G = Convert.ToInt16((Gamma_Binary_data[0 + Offset][0] + Gamma_Binary_data[1 + Offset]), 2);
            Gray191.int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][0] + Gamma_Binary_data[10 + Offset]), 2);
            Gray127.int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][1] + Gamma_Binary_data[9 + Offset]), 2);
            Gray63.int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][2] + Gamma_Binary_data[8 + Offset]), 2);
            Gray31.int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][3] + Gamma_Binary_data[7 + Offset]), 2);
            Gray15.int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][4] + Gamma_Binary_data[6 + Offset]), 2);
            Gray7.int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][5] + Gamma_Binary_data[5 + Offset]), 2);
            Gray4.int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][6] + Gamma_Binary_data[4 + Offset]), 2);
            Gray1.int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][7] + Gamma_Binary_data[3 + Offset]), 2);

            //B
            Offset = 22;
            Gray255.int_B = Convert.ToInt16((Gamma_Binary_data[0 + Offset][0] + Gamma_Binary_data[1 + Offset]), 2);
            Gray191.int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][0] + Gamma_Binary_data[10 + Offset]), 2);
            Gray127.int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][1] + Gamma_Binary_data[9 + Offset]), 2);
            Gray63.int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][2] + Gamma_Binary_data[8 + Offset]), 2);
            Gray31.int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][3] + Gamma_Binary_data[7 + Offset]), 2);
            Gray15.int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][4] + Gamma_Binary_data[6 + Offset]), 2);
            Gray7.int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][5] + Gamma_Binary_data[5 + Offset]), 2);
            Gray4.int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][6] + Gamma_Binary_data[4 + Offset]), 2);
            Gray1.int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][7] + Gamma_Binary_data[3 + Offset]), 2);

            //Update RGB String From Int
            Gray255.String_Update_From_int();
            Gray191.String_Update_From_int();
            Gray127.String_Update_From_int();
            Gray63.String_Update_From_int();
            Gray31.String_Update_From_int();
            Gray15.String_Update_From_int();
            Gray7.String_Update_From_int();
            Gray4.String_Update_From_int();
            Gray1.String_Update_From_int();
        }

        private void Gamma_read_btn_Click(object sender, EventArgs e)
        {
            textBox_Gamma_Write_R.Text = textBox_Gamma_Write_G.Text = textBox_Gamma_Write_B.Text = string.Empty;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            //Read Gray255 ~ Gray1 (for selected band) as string(Hex) and int(dex)
            //1) Gamma_Hex_data[33] 
            //2) Gamma_Binary_data[33] 
            //3) GrayXXX.int_R/G/B (Dex(int))
            //3) GrayXXX.R/G/B (Hex(string))
            Get_Gray_Gamma();

            //Gray 몇인지 확인
            if (this.radioButton_Gray255.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray255.R;
                this.textBox_Gamma_Read_G.Text = Gray255.G;
                this.textBox_Gamma_Read_B.Text = Gray255.B;
            }
            else if (this.radioButton_Gray191.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray191.R;
                this.textBox_Gamma_Read_G.Text = Gray191.G;
                this.textBox_Gamma_Read_B.Text = Gray191.B;
            }
            else if (this.radioButton_Gray127.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray127.R;
                this.textBox_Gamma_Read_G.Text = Gray127.G;
                this.textBox_Gamma_Read_B.Text = Gray127.B;
            }
            else if (this.radioButton_Gray63.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray63.R;
                this.textBox_Gamma_Read_G.Text = Gray63.G;
                this.textBox_Gamma_Read_B.Text = Gray63.B;
            }
            else if (this.radioButton_Gray31.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray31.R;
                this.textBox_Gamma_Read_G.Text = Gray31.G;
                this.textBox_Gamma_Read_B.Text = Gray31.B;
            }
            else if (this.radioButton_Gray15.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray15.R;
                this.textBox_Gamma_Read_G.Text = Gray15.G;
                this.textBox_Gamma_Read_B.Text = Gray15.B;
            }
            else if (this.radioButton_Gray7.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray7.R;
                this.textBox_Gamma_Read_G.Text = Gray7.G;
                this.textBox_Gamma_Read_B.Text = Gray7.B;
            }
            else if (this.radioButton_Gray4.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray4.R;
                this.textBox_Gamma_Read_G.Text = Gray4.G;
                this.textBox_Gamma_Read_B.Text = Gray4.B;
            }
            else if (this.radioButton_Gray1.Checked)
            {
                this.textBox_Gamma_Read_R.Text = Gray1.R;
                this.textBox_Gamma_Read_G.Text = Gray1.G;
                this.textBox_Gamma_Read_B.Text = Gray1.B;
            }
        }

        private void String_Replace_Char_By_Index(ref string STR,int INDEX,char CHAR)
        {
            StringBuilder SB = new StringBuilder(STR);
            SB[INDEX] = CHAR;
            STR = SB.ToString();
        }

        private void Update_Gamma_Binary_Data_From_Input_Dex(RGB Applied_Gamma)
        {
            //Gamma_Binary_data = new string[33];
            //Binary_R = X XXXX XXXX

            if (radioButton_Gray255.Checked)
            {
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[0], 0, Applied_Gamma.Binary_R[0]);//R_AM2[8]
                Gamma_Binary_data[1] = Applied_Gamma.Binary_R.Remove(0, 1);//R_AM2[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[0 + Offset], 0, Applied_Gamma.Binary_G[0]);//G_AM2[8]
                Gamma_Binary_data[1 + Offset] = Applied_Gamma.Binary_G.Remove(0, 1); //G_AM[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[0 + Offset], 0, Applied_Gamma.Binary_B[0]);//B_AM[8]
                Gamma_Binary_data[1 + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_AM[7:0]
                
            }
            else if (radioButton_Gray191.Checked)
            {
                int Bit_Position = 0;
                int Byte_Position = 10;
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2], Bit_Position, Applied_Gamma.Binary_R[0]);//R_GR5[8]
                Gamma_Binary_data[Byte_Position] = Applied_Gamma.Binary_R.Remove(0, 1);//R_GR5[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_G[0]);//G_GR5[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_G.Remove(0, 1);//G_GR5[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_B[0]);//B_GR5[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_GR5[7:0]

            }
            else if (radioButton_Gray127.Checked)
            {
                int Bit_Position = 1;
                int Byte_Position = 9;
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2], Bit_Position, Applied_Gamma.Binary_R[0]);//R_GR4[8]
                Gamma_Binary_data[Byte_Position] = Applied_Gamma.Binary_R.Remove(0, 1);//R_GR4[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_G[0]);//G_GR4[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_G.Remove(0, 1);//G_GR4[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_B[0]);//B_GR4[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_GR4[7:0]
            }
            else if (radioButton_Gray63.Checked)
            {
                int Bit_Position = 2;
                int Byte_Position = 8;
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2], Bit_Position, Applied_Gamma.Binary_R[0]);//R_GR3[8]
                Gamma_Binary_data[Byte_Position] = Applied_Gamma.Binary_R.Remove(0, 1);//R_GR3[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_G[0]);//G_GR3[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_G.Remove(0, 1);//G_GR3[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_B[0]);//B_GR3[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_GR3[7:0]
            }
            else if (radioButton_Gray31.Checked)
            {
                int Bit_Position = 3;
                int Byte_Position = 7;
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2], Bit_Position, Applied_Gamma.Binary_R[0]);//R_GR2[8]
                Gamma_Binary_data[Byte_Position] = Applied_Gamma.Binary_R.Remove(0, 1);//R_GR2[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_G[0]);//G_GR2[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_G.Remove(0, 1);//G_GR2[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_B[0]);//B_GR2[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_GR2[7:0]
            }
            else if (radioButton_Gray15.Checked)
            {
                int Bit_Position = 4;
                int Byte_Position = 6;
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2], Bit_Position, Applied_Gamma.Binary_R[0]);//R_GR1[8]
                Gamma_Binary_data[Byte_Position] = Applied_Gamma.Binary_R.Remove(0, 1);//R_GR1[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_G[0]);//G_GR1[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_G.Remove(0, 1);//G_GR1[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_B[0]);//B_GR1[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_GR1[7:0]
            }
            else if (radioButton_Gray7.Checked)
            {
                int Bit_Position = 5;
                int Byte_Position = 5;
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2], Bit_Position, Applied_Gamma.Binary_R[0]);//R_GR0[8]
                Gamma_Binary_data[Byte_Position] = Applied_Gamma.Binary_R.Remove(0, 1);//R_GR0[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_G[0]);//G_GR0[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_G.Remove(0, 1);//G_GR0[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_B[0]);//B_GR0[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_GR0[7:0]
            }
            else if (radioButton_Gray4.Checked)
            {
                int Bit_Position = 6;
                int Byte_Position = 4;
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2], Bit_Position, Applied_Gamma.Binary_R[0]);//R_GR0[8]
                Gamma_Binary_data[Byte_Position] = Applied_Gamma.Binary_R.Remove(0, 1);//R_GR0[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_G[0]);//G_GR0[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_G.Remove(0, 1);//G_GR0[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_B[0]);//B_GR0[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_GR0[7:0]
            }
            else if (radioButton_Gray1.Checked)
            {
                int Bit_Position = 7;
                int Byte_Position = 3;
                //R
                int Offset = 0;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2], Bit_Position, Applied_Gamma.Binary_R[0]);//R_GR0[8]
                Gamma_Binary_data[Byte_Position] = Applied_Gamma.Binary_R.Remove(0, 1);//R_GR0[7:0]

                //G
                Offset = 11;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_G[0]);//G_GR0[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_G.Remove(0, 1);//G_GR0[7:0]

                //B
                Offset = 22;
                String_Replace_Char_By_Index(ref Gamma_Binary_data[2 + Offset], Bit_Position, Applied_Gamma.Binary_B[0]);//B_GR0[8]
                Gamma_Binary_data[Byte_Position + Offset] = Applied_Gamma.Binary_B.Remove(0, 1);//B_GR0[7:0]
            }
        }

        private void Gamma_Write_btn_Click(object sender, EventArgs e)
        {
            textBox_Gamma_Read_R.Text = textBox_Gamma_Read_G.Text = textBox_Gamma_Read_B.Text = string.Empty;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            
            /*Get_Gray_Gamma : Read Gray255 ~ Gray1 (for selected band) as string(Hex) and int(dex)
            1) Gamma_Hex_data[33] 
            2) Gamma_Binary_data[33] 
            3) GrayXXX.int_R/G/B (Dex(int))
            3) GrayXXX.R/G/B (Hex(string))*/
            Get_Gray_Gamma();

            RGB Applied_Gamma = new RGB();

            Applied_Gamma.int_R = Convert.ToInt16(textBox_Gamma_Write_R.Text);
            if (Applied_Gamma.int_R < 0) Applied_Gamma.int_R = 0;
            else if (Applied_Gamma.int_R > 511) Applied_Gamma.int_R = 511;
            textBox_Gamma_Write_R.Text = Applied_Gamma.int_R.ToString();


            Applied_Gamma.int_G = Convert.ToInt16(textBox_Gamma_Write_G.Text);
            if (Applied_Gamma.int_G < 0) Applied_Gamma.int_G = 0;
            else if (Applied_Gamma.int_G > 511) Applied_Gamma.int_G = 511;
            textBox_Gamma_Write_G.Text = Applied_Gamma.int_G.ToString();


            Applied_Gamma.int_B = Convert.ToInt16(textBox_Gamma_Write_B.Text);
            if (Applied_Gamma.int_B < 0) Applied_Gamma.int_B = 0;
            else if (Applied_Gamma.int_B > 511) Applied_Gamma.int_B = 511;
            textBox_Gamma_Write_B.Text = Applied_Gamma.int_B.ToString();


            Applied_Gamma.Binary_String_Update_From_Int(9, '0'); //X XXXX XXXX

            //1)Update Binary Values (From the input dex R/G/B)
            Update_Gamma_Binary_Data_From_Input_Dex(Applied_Gamma);

            //2)Binary[33] --> (Dex) --> Hex[33]
            for (int i = 0; i < 33; i++) Gamma_Hex_data[i] = Convert.ToString(Convert.ToInt16(Gamma_Binary_data[i], 2), 16);

            f1.Long_Packet_CMD_Send(33, Current_Band_Gamma_Register, Gamma_Hex_data);  
        }

        private void button_Manufactuer_Access_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0xAC #Manufactuer Access",Color.Blue);
        }

        private void button_Manufactuer_Close_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0xCA #Manufactuer Close", Color.Red);
        }

        private void button_Select_All_Band_Click(object sender, EventArgs e)
        {
            checkBox_Band1.Checked = true;
            checkBox_Band2.Checked = true;
            checkBox_Band3.Checked = true;
            checkBox_Band4.Checked = true;
            checkBox_Band5.Checked = true;
            checkBox_Band6.Checked = true;
            checkBox_Band7.Checked = true;
            checkBox_Band8.Checked = true;
            checkBox_Band9.Checked = true;
            checkBox_Band10.Checked = true;
        }

        private void button_Deselect_All_Band_Click(object sender, EventArgs e)
        {
            checkBox_Band1.Checked = false;
            checkBox_Band2.Checked = false;
            checkBox_Band3.Checked = false;
            checkBox_Band4.Checked = false;
            checkBox_Band5.Checked = false;
            checkBox_Band6.Checked = false;
            checkBox_Band7.Checked = false;
            checkBox_Band8.Checked = false;
            checkBox_Band9.Checked = false;
            checkBox_Band10.Checked = false;
        }

        public void button_B1_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B1_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band0 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B2_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B2_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band1 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B3_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B3_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band2 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B4_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B4_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band3 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B5_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B5_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band4 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B6_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B6_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band5 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B7_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B7_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band6 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B8_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B8_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band7 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B9_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B9_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band8 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B10_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B10_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band9 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_Read_DBV_Setting_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                f1.OTP_Read(39, "D0");
                Thread.Sleep(200);
                textBox_B1_DBV_Setting.Text = "3FF";//Band0
                textBox_B2_DBV_Setting.Text =  (Convert.ToInt16(f1.dataGridView1.Rows[23].Cells[1].Value.ToString(), 16) & 0x03).ToString("X") + f1.dataGridView1.Rows[24].Cells[1].Value.ToString(); //Band1
                textBox_B3_DBV_Setting.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[23].Cells[1].Value.ToString(), 16) >> 2) & 0x03).ToString("X") + f1.dataGridView1.Rows[25].Cells[1].Value.ToString(); //Band2
                textBox_B4_DBV_Setting.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[23].Cells[1].Value.ToString(), 16) >> 4) & 0x03).ToString("X") + f1.dataGridView1.Rows[26].Cells[1].Value.ToString(); //Band3
                textBox_B5_DBV_Setting.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[23].Cells[1].Value.ToString(), 16) >> 6) & 0x03).ToString("X") + f1.dataGridView1.Rows[27].Cells[1].Value.ToString(); //Band4
                textBox_B6_DBV_Setting.Text = (Convert.ToInt16(f1.dataGridView1.Rows[28].Cells[1].Value.ToString(), 16) & 0x03).ToString("X") + f1.dataGridView1.Rows[29].Cells[1].Value.ToString(); //Band5
                textBox_B7_DBV_Setting.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[28].Cells[1].Value.ToString(), 16) >> 2) & 0x03).ToString("X") + f1.dataGridView1.Rows[30].Cells[1].Value.ToString(); //Band6
                textBox_B8_DBV_Setting.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[28].Cells[1].Value.ToString(), 16) >> 4) & 0x03).ToString("X") + f1.dataGridView1.Rows[31].Cells[1].Value.ToString(); //Band7
                textBox_B9_DBV_Setting.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[28].Cells[1].Value.ToString(), 16) >> 6) & 0x03).ToString("X") + f1.dataGridView1.Rows[32].Cells[1].Value.ToString(); //Band8
                textBox_B10_DBV_Setting.Text = (Convert.ToInt16(f1.dataGridView1.Rows[33].Cells[1].Value.ToString(), 16) & 0x03).ToString("X") + f1.dataGridView1.Rows[34].Cells[1].Value.ToString(); //Band9
                f1.GB_Status_AppendText_Nextline("DBV values were loaded from register", System.Drawing.Color.Black);
                Application.DoEvents();
            }
            catch
            {
                System.Windows.MessageBox.Show("DBV Value Read fail");
            }

        }

        private void TextBox_Vreg1_Clear()
        {
            textBox_Vreg1_B0.Text = string.Empty;
            textBox_Vreg1_B1.Text = string.Empty;
            textBox_Vreg1_B2.Text = string.Empty;
            textBox_Vreg1_B3.Text = string.Empty;
            textBox_Vreg1_B4.Text = string.Empty;
            textBox_Vreg1_B5.Text = string.Empty;
            textBox_Vreg1_B6.Text = string.Empty;
            textBox_Vreg1_B7.Text = string.Empty;
            textBox_Vreg1_B8.Text = string.Empty;
            textBox_Vreg1_B9.Text = string.Empty;
        }

        public void button_Vreg1_Read_Click()
        {
            button_Vreg1_Read.PerformClick();
        }

        private void button_Vreg1_Read_Click(object sender, EventArgs e)
        {
            try
            {
            TextBox_Vreg1_Clear();
           
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.OTP_Read(16, "D1");
            Thread.Sleep(200);
            for (int i = 0; i < 16; i++)
            {
                D1_Vreg1_Params[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                
            }
            int Offset = 15;
            textBox_Vreg1_B0.Text = "3FF";//Band0
            textBox_Vreg1_B1.Text = (Convert.ToInt16(f1.dataGridView1.Rows[15 - Offset].Cells[1].Value.ToString(), 16) & 0x03).ToString("X") + f1.dataGridView1.Rows[16 - Offset].Cells[1].Value.ToString(); //Band1
            textBox_Vreg1_B2.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[15 - Offset].Cells[1].Value.ToString(), 16) >> 2) & 0x03).ToString("X") + f1.dataGridView1.Rows[17 - Offset].Cells[1].Value.ToString(); //Band2
            textBox_Vreg1_B3.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[15 - Offset].Cells[1].Value.ToString(), 16) >> 4) & 0x03).ToString("X") + f1.dataGridView1.Rows[18 - Offset].Cells[1].Value.ToString(); //Band3
            textBox_Vreg1_B4.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[15 - Offset].Cells[1].Value.ToString(), 16) >> 6) & 0x03).ToString("X") + f1.dataGridView1.Rows[19 - Offset].Cells[1].Value.ToString(); //Band4
            textBox_Vreg1_B5.Text = (Convert.ToInt16(f1.dataGridView1.Rows[20 - Offset].Cells[1].Value.ToString(), 16) & 0x03).ToString("X") + f1.dataGridView1.Rows[21 - Offset].Cells[1].Value.ToString(); //Band5
            textBox_Vreg1_B6.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[20 - Offset].Cells[1].Value.ToString(), 16) >> 2) & 0x03).ToString("X") + f1.dataGridView1.Rows[22 - Offset].Cells[1].Value.ToString(); //Band6
            textBox_Vreg1_B7.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[20 - Offset].Cells[1].Value.ToString(), 16) >> 4) & 0x03).ToString("X") + f1.dataGridView1.Rows[23 - Offset].Cells[1].Value.ToString(); //Band7
            textBox_Vreg1_B8.Text = ((Convert.ToInt16(f1.dataGridView1.Rows[20 - Offset].Cells[1].Value.ToString(), 16) >> 6) & 0x03).ToString("X") + f1.dataGridView1.Rows[24 - Offset].Cells[1].Value.ToString(); //Band8
            textBox_Vreg1_B9.Text = (Convert.ToInt16(f1.dataGridView1.Rows[25 - Offset].Cells[1].Value.ToString(), 16) & 0x03).ToString("X") + f1.dataGridView1.Rows[26 - Offset].Cells[1].Value.ToString(); //Band9
            Display_Vreg1_As_Dex();
                
            f1.GB_Status_AppendText_Nextline("Vreg1 values were loaded from register", System.Drawing.Color.Black);
            Application.DoEvents();
            }
            catch
            {
                System.Windows.MessageBox.Show("Vreg1 Value Read fail");
            }
        }

        private void Display_Vreg1_As_Dex()
        {
            textBox_Vreg1_B0.Text = Convert.ToInt16(textBox_Vreg1_B0.Text, 16).ToString();
            textBox_Vreg1_B1.Text = Convert.ToInt16(textBox_Vreg1_B1.Text, 16).ToString();
            textBox_Vreg1_B2.Text = Convert.ToInt16(textBox_Vreg1_B2.Text, 16).ToString();
            textBox_Vreg1_B3.Text = Convert.ToInt16(textBox_Vreg1_B3.Text, 16).ToString();
            textBox_Vreg1_B4.Text = Convert.ToInt16(textBox_Vreg1_B4.Text, 16).ToString();
            textBox_Vreg1_B5.Text = Convert.ToInt16(textBox_Vreg1_B5.Text, 16).ToString();
            textBox_Vreg1_B6.Text = Convert.ToInt16(textBox_Vreg1_B6.Text, 16).ToString();
            textBox_Vreg1_B7.Text = Convert.ToInt16(textBox_Vreg1_B7.Text, 16).ToString();
            textBox_Vreg1_B8.Text = Convert.ToInt16(textBox_Vreg1_B8.Text, 16).ToString();
            textBox_Vreg1_B9.Text = Convert.ToInt16(textBox_Vreg1_B9.Text, 16).ToString();
        }

        private void Meta_Pattern_Setting(int gray)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Gray = 255;
            if (checkBox_Special_Gray_Compensation.Checked)
            {
                string Band_Gray = string.Empty;

                Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
                Band_Gray = Meta_form_engineer.Get_BX_GXXX_By_Gray_Meta(gray);
                Gray = Convert.ToInt16(Band_Gray.Remove(0, 4)); //ex) B0_G255 --> 255 , A2_G91 --> 91 , A0_G4 --> 4
                f1.GB_Status_AppendText_Nextline("Band_Gray : " + Band_Gray + " / Gray : " + Gray.ToString(), System.Drawing.Color.Red);
            }
            else
            {
                switch (gray)
                {
                    case 0:
                        Gray = 255;
                        break;
                    case 1:
                        Gray = 191;
                        break;
                    case 2:
                        Gray = 127;
                        break;
                    case 3:
                        Gray = 63;
                        break;
                    case 4:
                        Gray = 31;
                        break;
                    case 5:
                        Gray = 15;
                        break;
                    case 6:
                        Gray = 7;
                        break;
                    case 7:
                        Gray = 4;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Gray is out of boundary");
                        break;
                }
            }
            
            if (radioButton_OPR_100_Per.Checked)
            {
                if (checkBox_OC_APL_Change.Checked)
                {
                    double APL = Convert.ToDouble(numericUpDown_APL.Value) * 0.01;
                    f1.APL_PTN_update(Gray, Gray, Gray, APL);
                    f1.GB_Status_AppendText_Nextline("Gray" + Gray.ToString() + " Setting , APL(%) : " + APL.ToString() + " Apply", System.Drawing.Color.Black);
                }
                else
                {
                    f1.PTN_update(Gray, Gray, Gray);
                    f1.GB_Status_AppendText_Nextline("Gray" + Gray.ToString() + " Setting", System.Drawing.Color.Black);
                }
            }
        }


        private void Optic_compensation_Start_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            TextBox_Vreg1_Clear();
            textBox_Total_Average_Meas_Count.Text = "0";
            Meta_Engineer_Mornitoring_Mode.getInstance().Show();

            Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_form_engineer.Band_Radiobuttion_Select(0);
            Meta_form_engineer.RadioButton_All_Enable(false);
            Single_Mode_Optic_compensation();
            Meta_form_engineer.RadioButton_All_Enable(true);
        }

        private void button_BSQH_Stop_Click(object sender, EventArgs e)
        {
            Optic_Compensation_Stop = true;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.OC_Timer_Stop();
        }

        private void Get_Param(int gray, ref RGB Gamma, ref XYLv Target, ref XYLv Limit, ref XYLv Extension)
        {
            Meta_Engineer_Mornitoring_Mode Single_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Single_form_engineer.Get_OC_Param_Meta(gray, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B
                , ref Target.double_X, ref Target.double_Y, ref Target.double_Lv, ref Limit.double_X, ref Limit.double_Y, ref Limit.double_Lv, ref Extension.double_X, ref Extension.double_Y);
        }


        private void Get_First_Gamma_Fx_Previous_Band_Mode_C_Sharp_Dll_Test(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, int Previous_Band_Vreg1_Dec, int Current_Band_Dec_Vreg1, bool[] Selected_Band, int[] Previous_Band_Gamma_R, int[] Previous_Band_Gamma_G, int[] Previous_Band_Gamma_B, double[] Previous_Band_Target_Lv,
             int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            SJH_Matrix M = new SJH_Matrix();

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                double[] Previous_Band_Gamma_R_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_R, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Previous_Band_Gamma_G_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_G, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Previous_Band_Gamma_B_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_B, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[][] A_R = M.MatrixCreate(8, 8);
                double[][] A_G = M.MatrixCreate(8, 8);
                double[][] A_B = M.MatrixCreate(8, 8);
                double[][] Inv_A_R = M.MatrixCreate(8, 8);
                double[][] Inv_A_G = M.MatrixCreate(8, 8);
                double[][] Inv_A_B = M.MatrixCreate(8, 8);
                double[] C_R = new double[8];
                double[] C_G = new double[8];
                double[] C_B = new double[8];

                //Get A[i][count] = HBM_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= 7; i++)
                {
                    count = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        A_R[i][count] = Math.Pow(Previous_Band_Gamma_R_Voltage[i], j);
                        A_G[i][count] = Math.Pow(Previous_Band_Gamma_G_Voltage[i], j);
                        A_B[i][count] = Math.Pow(Previous_Band_Gamma_B_Voltage[i], j);
                        count++;
                    }
                }

                //Get C[8] = inv(A)[8,8] * HBM_Target_Lv[8]
                Inv_A_R = M.MatrixInverse(A_R);
                Inv_A_G = M.MatrixInverse(A_G);
                Inv_A_B = M.MatrixInverse(A_B);
                C_R = M.Matrix_Multiply(Inv_A_R, Previous_Band_Target_Lv);
                C_G = M.Matrix_Multiply(Inv_A_G, Previous_Band_Target_Lv);
                C_B = M.Matrix_Multiply(Inv_A_B, Previous_Band_Target_Lv);

                //Get Calculated Voltage_R/G/B
                double Calculated_Target_Lv;
                int iteration;
                double Calculated_R_Vdata = 0;
                double Calculated_G_Vdata = 0;
                double Calculated_B_Vdata = 0;
                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0) { Calculated_G_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Current_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Current_Band_AM1_RGB_Voltage = F7 + (Current_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata);
                Gamma_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata);
                Gamma_B = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata);
            }
        }

        private void Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_All_Band_3points(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[][] Band_Gray_Gamma_Red, int[][] Band_Gray_Gamma_Green, int[][] Band_Gray_Gamma_Blue, double[][] Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            SJH_Matrix M = new SJH_Matrix();

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                double[][] Band_Gray_Gamma_Red_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Green_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Blue_Voltage = M.MatrixCreate(band, 8);
                for (int i = 0; i < band; i++)
                {
                    //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
                    Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Red[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Green[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Blue[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                }

                int Points_Num = band * 8;
                double[] Gamma_Red_Voltage_Points = new double[Points_Num];
                double[] Gamma_Green_Voltage_Points = new double[Points_Num];
                double[] Gamma_Blue_Voltage_Points = new double[Points_Num];
                double[] Target_Lv_Points = new double[Points_Num];

                for (int b = 0; b < band; b++)
                {
                    for (int g = 0; g < 8; g++)
                    {
                        Gamma_Red_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Red_Voltage[b][g];
                        Gamma_Green_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Green_Voltage[b][g];
                        Gamma_Blue_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Blue_Voltage[b][g];
                        Target_Lv_Points[(8 * b) + g] = Band_Gray_Target_Lv[b][g];
                    }
                }

                Array.Sort<double>(Gamma_Red_Voltage_Points);
                Array.Sort<double>(Gamma_Green_Voltage_Points);
                Array.Sort<double>(Gamma_Blue_Voltage_Points);
                Array.Sort<double>(Target_Lv_Points);
                Array.Reverse(Target_Lv_Points);

                if (band == 4 && gray == 6)
                    for (int b = 0; b < band; b++)
                        for (int g = 0; g < 8; g++)
                            f1.GB_Status_AppendText_Nextline("Gamma_Green_Voltage_Points[(8 * b) + g]/Target_Lv_Points[(8 * b) + g]  : " + Gamma_Green_Voltage_Points[(8 * b) + g].ToString() + "/" + Target_Lv_Points[(8 * b) + g].ToString(), Color.Brown);
               

                int Formula_Num = Points_Num - 2; //((band * 8) - 2 = Points_Num - 2) 
                f1.GB_Status_AppendText_Nextline("Formula_Num : " + Formula_Num.ToString(), Color.Red, true);

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
                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                    {
                        //Get "Three_points_Target_Lv"
                        Three_points_Target_Lv[k][i] = Target_Lv_Points[i + k];
                        count = 0;
                        //Get "A"
                        for (int j = 2; j >= 0; j--)
                        {
                            Temp_A_R[i][count] = Math.Pow(Gamma_Red_Voltage_Points[i + k], j);
                            Temp_A_G[i][count] = Math.Pow(Gamma_Green_Voltage_Points[i + k], j);
                            Temp_A_B[i][count] = Math.Pow(Gamma_Blue_Voltage_Points[i + k], j);
                            count++;
                        }
                    }

                    //Show Temp A_G
                    f1.GB_Status_AppendText_Nextline("--Show Temp A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Temp_A_G[i][0].ToString() + "," + Temp_A_G[i][1].ToString() + "," + Temp_A_G[i][2].ToString(), Color.Black, true);




                    //f1.GB_Status_AppendText_Nextline("-----", Color.Black);
                    //f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Black);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[0 + k] : " + Band01_Gamma_Red_Voltage[0 + k].ToString(), Color.Red);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[1 + k] : " + Band01_Gamma_Red_Voltage[1 + k].ToString(), Color.Red);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[2 + k] : " + Band01_Gamma_Red_Voltage[2 + k].ToString(), Color.Red);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[0 + k] : " + Gamma_Green_Voltage_Points[0 + k].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[1 + k] : " + Gamma_Green_Voltage_Points[1 + k].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[2 + k] : " + Gamma_Green_Voltage_Points[2 + k].ToString(), Color.Green, true);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[0 + k] : " + Band01_Gamma_Blue_Voltage[0 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[1 + k] : " + Band01_Gamma_Blue_Voltage[1 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[2 + k] : " + Band01_Gamma_Blue_Voltage[2 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("-----", Color.Black);

                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][0] : " + Three_points_Target_Lv[k][0].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][1] : " + Three_points_Target_Lv[k][1].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][2] : " + Three_points_Target_Lv[k][2].ToString(), Color.Blue, true);



                    //Get Inv_A (by using "A")
                    Temp_Inv_A_R = M.MatrixInverse(Temp_A_R);
                    Temp_Inv_A_G = M.MatrixInverse(Temp_A_G);
                    Temp_Inv_A_B = M.MatrixInverse(Temp_A_B);


                    //Show Temp Inv_A
                    f1.GB_Status_AppendText_Nextline("--Show Temp_Inv_A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Temp_Inv_A_G[i][0].ToString() + "," + Temp_Inv_A_G[i][1].ToString() + "," + Temp_Inv_A_G[i][2].ToString(), Color.Black, true);


                    //Get C (by using "Inv_A" , "Three_points_Target_Lv")
                    Three_points_C_R[k] = M.Matrix_Multiply(Temp_Inv_A_R, Three_points_Target_Lv[k]);
                    Three_points_C_G[k] = M.Matrix_Multiply(Temp_Inv_A_G, Three_points_Target_Lv[k]);
                    Three_points_C_B[k] = M.Matrix_Multiply(Temp_Inv_A_B, Three_points_Target_Lv[k]);


                    //Show Three_points_C_R
                    f1.GB_Status_AppendText_Nextline("--Show Three_points_C_G---", Color.Black, true);
                    f1.GB_Status_AppendText_Nextline(Three_points_C_G[k][0].ToString() + "," + Three_points_C_G[k][1].ToString() + "," + Three_points_C_G[k][2].ToString(), Color.Black, true);


                    //Get C (Target belongs to [Three_points_Target_Lv[k][2],Three_points_Target_Lv[k][0]])
                    if (Three_points_Target_Lv[k][2] < Target_Lv && Target_Lv < Three_points_Target_Lv[k][0])
                    {
                        for (int i = 0; i <= 2; i++)
                        {
                            C_R[i] = Three_points_C_R[k][i];
                            C_G[i] = Three_points_C_G[k][i];
                            C_B[i] = Three_points_C_B[k][i];
                        }

                        f1.GB_Status_AppendText_Nextline("k : " + k.ToString() + " was selected for C", Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Target_LV : " + Target_Lv.ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][0] : " + Three_points_Target_Lv[k][0].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][1] : " + Three_points_Target_Lv[k][1].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][2] : " + Three_points_Target_Lv[k][2].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("--Show Final Selected C_G---", Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline(C_G[0].ToString() + "," + C_G[1].ToString() + "," + C_G[2].ToString(), Color.Blue, true);
                        break; //Break for k loop
                    }
                }

                //Get Calculated Voltage_R/G/B
                double Calculated_Target_Lv;
                int iteration;
                double Calculated_R_Vdata = 0;
                double Calculated_G_Vdata = 0;
                double Calculated_B_Vdata = 0;
                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0)
                    {
                        Calculated_G_Vdata = Vdata;

                        f1.GB_Status_AppendText_Nextline("When Calculated_Target_Lv < Target_Lv", Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv : " + Calculated_Target_Lv.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Target_Lv : " + Target_Lv.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Prvious_Gray_Gamma_G_Voltage : " + Prvious_Gray_Gamma_G_Voltage.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Calculated_G_Vdata : " + Calculated_G_Vdata.ToString(), Color.Green, true);

                        break;
                    }
                }

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Current_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Current_Band_AM1_RGB_Voltage = F7 + (Current_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata);
                Gamma_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata);
                Gamma_B = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata);
            }
        }


        private void Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_All_Band_3points_Combine_Points(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[][] Band_Gray_Gamma_Red, int[][] Band_Gray_Gamma_Green, int[][] Band_Gray_Gamma_Blue, double[][] Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7
            , double Fx_3points_Combine_LV_1, double Fx_3points_Combine_LV_2, double Fx_3points_Combine_LV_3, double Fx_3points_Combine_Lv_Distance)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            SJH_Matrix M = new SJH_Matrix();

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                double[][] Band_Gray_Gamma_Red_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Green_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Blue_Voltage = M.MatrixCreate(band, 8);
                for (int i = 0; i < band; i++)
                {
                    //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
                    Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Red[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Green[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Blue[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                }



                //Need to...
                //Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance
                int Points_Num = band * 8;
                double[] Gamma_Red_Voltage_Points = new double[Points_Num];
                double[] Gamma_Green_Voltage_Points = new double[Points_Num];
                double[] Gamma_Blue_Voltage_Points = new double[Points_Num];
                double[] Target_Lv_Points = new double[Points_Num];

                for (int b = 0; b < band; b++)
                {
                    for (int g = 0; g < 8; g++)
                    {
                        Gamma_Red_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Red_Voltage[b][g];
                        Gamma_Green_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Green_Voltage[b][g];
                        Gamma_Blue_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Blue_Voltage[b][g];
                        Target_Lv_Points[(8 * b) + g] = Band_Gray_Target_Lv[b][g];
                    }
                }
               
                Array.Sort<double>(Gamma_Red_Voltage_Points);
                Array.Sort<double>(Gamma_Green_Voltage_Points);
                Array.Sort<double>(Gamma_Blue_Voltage_Points);
                Array.Sort<double>(Target_Lv_Points);
                Array.Reverse(Target_Lv_Points);

                List<double> Gamma_Red_Voltage_Points_Rearrange = new List<double>();
                List<double> Temp_R_1 = new List<double>();
                List<double> Temp_R_2 = new List<double>();
                List<double> Temp_R_3 = new List<double>();

                List<double> Gamma_Green_Voltage_Points_Rearrange = new List<double>();
                List<double> Temp_G_1 = new List<double>();
                List<double> Temp_G_2 = new List<double>();
                List<double> Temp_G_3 = new List<double>();

                List<double> Gamma_Blue_Voltage_Points_Rearrange = new List<double>();
                List<double> Temp_B_1 = new List<double>();
                List<double> Temp_B_2 = new List<double>();
                List<double> Temp_B_3 = new List<double>();
                
                List<double> Target_Lv_Points_Rearrange = new List<double>();
                List<double> Temp_Target_Lv_1 = new List<double>();
                List<double> Temp_Target_Lv_2 = new List<double>();
                List<double> Temp_Target_Lv_3 = new List<double>();
                
                double Diff_Lv = 0 ;
                bool flag = false;
                for (int b = 0; b < band; b++)
                {
                    for (int g = 0; g < 8; g++)
                    {
                        if (flag)
                        {
                            g++;
                            flag = false;
                        }
                        //Lv
                        // X < A (Region 1)
                        if(Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_1)
                        {
                            Temp_R_1.Add(Gamma_Red_Voltage_Points[(8 * b) + g]);
                            Temp_G_1.Add(Gamma_Green_Voltage_Points[(8 * b) + g]);
                            Temp_B_1.Add(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                            Temp_Target_Lv_1.Add(Target_Lv_Points[(8 * b) + g]);
                        }
                        // A <= X < B (Region 2)
                        else if(Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_2)
                        {
                            Temp_R_2.Add(Gamma_Red_Voltage_Points[(8 * b) + g]);
                            Temp_G_2.Add(Gamma_Green_Voltage_Points[(8 * b) + g]);
                            Temp_B_2.Add(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                            Temp_Target_Lv_2.Add(Target_Lv_Points[(8 * b) + g]);
                        }
                        // B <= X < C (Region 3)
                        else if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_3)
                        {
                            Temp_R_3.Add(Gamma_Red_Voltage_Points[(8 * b) + g]);
                            Temp_G_3.Add(Gamma_Green_Voltage_Points[(8 * b) + g]);
                            Temp_B_3.Add(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                            Temp_Target_Lv_3.Add(Target_Lv_Points[(8 * b) + g]);
                            
                        }
                        // C <= X (Region 4)
                        else 
                        {
                            f1.GB_Status_AppendText_Nextline("Diff_Lv < Fx_3points_Combine_Lv_Distance", Color.Blue,true);
                            Gamma_Red_Voltage_Points_Rearrange.Add(Gamma_Red_Voltage_Points[(8 * b) + g]);
                            Gamma_Green_Voltage_Points_Rearrange.Add(Gamma_Green_Voltage_Points[(8 * b) + g]);
                            Gamma_Blue_Voltage_Points_Rearrange.Add(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                            Target_Lv_Points_Rearrange.Add(Target_Lv_Points[(8 * b) + g]);

                            if(((8 * b) + g) < ((8 * (band-1)) + (8-1))) //if it's not the last point
                            {
                                Diff_Lv = Math.Abs(Target_Lv_Points[(8 * b) + g] - Target_Lv_Points[(8 * b) + (g + 1)]);
                                f1.GB_Status_AppendText_Nextline("Diff_Lv/Fx_3points_Combine_Lv_Distance : " + Diff_Lv.ToString() + "/" + Fx_3points_Combine_Lv_Distance.ToString(), Color.Blue, true);
                                if(Diff_Lv < Fx_3points_Combine_Lv_Distance)
                                {    
                                    if (g == 7)
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        g++;
                                        flag = false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (Temp_Target_Lv_3.Count() != 0)
                {
                    Gamma_Red_Voltage_Points_Rearrange.Add(Temp_R_3.Sum() / Temp_R_3.Count());
                    Gamma_Green_Voltage_Points_Rearrange.Add(Temp_G_3.Sum() / Temp_G_3.Count());
                    Gamma_Blue_Voltage_Points_Rearrange.Add(Temp_B_3.Sum() / Temp_B_3.Count());
                    Target_Lv_Points_Rearrange.Add(Temp_Target_Lv_3.Sum() / Temp_Target_Lv_3.Count());
                }
                if (Temp_Target_Lv_2.Count() != 0)
                {
                    Gamma_Red_Voltage_Points_Rearrange.Add(Temp_R_2.Sum() / Temp_R_2.Count());
                    Gamma_Green_Voltage_Points_Rearrange.Add(Temp_G_2.Sum() / Temp_G_2.Count());
                    Gamma_Blue_Voltage_Points_Rearrange.Add(Temp_B_2.Sum() / Temp_B_2.Count());
                    Target_Lv_Points_Rearrange.Add(Temp_Target_Lv_2.Sum() / Temp_Target_Lv_2.Count());
                }
                if (Temp_Target_Lv_1.Count() != 0)
                {
                    Gamma_Red_Voltage_Points_Rearrange.Add(Temp_R_1.Sum() / Temp_R_1.Count());
                    Gamma_Green_Voltage_Points_Rearrange.Add(Temp_G_1.Sum() / Temp_G_1.Count());
                    Gamma_Blue_Voltage_Points_Rearrange.Add(Temp_B_1.Sum() / Temp_B_1.Count());
                    Target_Lv_Points_Rearrange.Add(Temp_Target_Lv_1.Sum() / Temp_Target_Lv_1.Count());
                }

                for (int b = 0; b < band; b++)
                   for (int g = 0; g < 8; g++)
                       f1.GB_Status_AppendText_Nextline("Gamma_Green_Voltage_Points[(8 * b) + g]/Target_Lv_Points[(8 * b) + g]  : " + Gamma_Green_Voltage_Points[(8 * b) + g].ToString() + "/" + Target_Lv_Points[(8 * b) + g].ToString(), Color.Brown, true);

                for (int i = 0; i < Gamma_Green_Voltage_Points_Rearrange.Count(); i++)
                    f1.GB_Status_AppendText_Nextline("Gamma_Green_Voltage_Points_Rearrange/Target_Lv_Points : " + Gamma_Green_Voltage_Points_Rearrange[i].ToString() + "/" + Target_Lv_Points_Rearrange[i].ToString(), Color.Black, true);

                //int Formula_Num = Points_Num - 2; //((band * 8) - 2 = Points_Num - 2) 
                int Formula_Num = Target_Lv_Points_Rearrange.Count() - 2; //Formula_Num = Points_Num - 2;
                f1.GB_Status_AppendText_Nextline("Formula_Num : " + Formula_Num.ToString(), Color.Red, true);

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
                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                    {
                        //Get "Three_points_Target_Lv"
                        Three_points_Target_Lv[k][i] = Target_Lv_Points_Rearrange[i + k];
                        count = 0;
                        //Get "A"
                        for (int j = 2; j >= 0; j--)
                        {
                            Temp_A_R[i][count] = Math.Pow(Gamma_Red_Voltage_Points_Rearrange[i + k], j);
                            Temp_A_G[i][count] = Math.Pow(Gamma_Green_Voltage_Points_Rearrange[i + k], j);
                            Temp_A_B[i][count] = Math.Pow(Gamma_Blue_Voltage_Points_Rearrange[i + k], j);
                            count++;
                        }
                    }

                    //Show Temp A_G
                    f1.GB_Status_AppendText_Nextline("--Show Temp A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Temp_A_G[i][0].ToString() + "," + Temp_A_G[i][1].ToString() + "," + Temp_A_G[i][2].ToString(), Color.Black, true);

                    /*
                    f1.GB_Status_AppendText_Nextline("Gamma_Green_Voltage_Points_Rearrange[0 + k] : " + Gamma_Green_Voltage_Points_Rearrange[0 + k].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("Gamma_Green_Voltage_Points_Rearrange[1 + k] : " + Gamma_Green_Voltage_Points_Rearrange[1 + k].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("Gamma_Green_Voltage_Points_Rearrange[2 + k] : " + Gamma_Green_Voltage_Points_Rearrange[2 + k].ToString(), Color.Green, true);
                    
                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][0] : " + Three_points_Target_Lv[k][0].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][1] : " + Three_points_Target_Lv[k][1].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][2] : " + Three_points_Target_Lv[k][2].ToString(), Color.Blue, true);
                    */

                    //Get Inv_A (by using "A")
                    Temp_Inv_A_R = M.MatrixInverse(Temp_A_R);
                    Temp_Inv_A_G = M.MatrixInverse(Temp_A_G);
                    Temp_Inv_A_B = M.MatrixInverse(Temp_A_B);

                    //Show Temp Inv_A
                    f1.GB_Status_AppendText_Nextline("--Show Temp_Inv_A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Temp_Inv_A_G[i][0].ToString() + "," + Temp_Inv_A_G[i][1].ToString() + "," + Temp_Inv_A_G[i][2].ToString(), Color.Black, true);


                    //Get C (by using "Inv_A" , "Three_points_Target_Lv")
                    Three_points_C_R[k] = M.Matrix_Multiply(Temp_Inv_A_R, Three_points_Target_Lv[k]);
                    Three_points_C_G[k] = M.Matrix_Multiply(Temp_Inv_A_G, Three_points_Target_Lv[k]);
                    Three_points_C_B[k] = M.Matrix_Multiply(Temp_Inv_A_B, Three_points_Target_Lv[k]);


                    //Show Three_points_C_R
                    f1.GB_Status_AppendText_Nextline("--Show Three_points_C_G---", Color.Black, true);
                    f1.GB_Status_AppendText_Nextline(Three_points_C_G[k][0].ToString() + "," + Three_points_C_G[k][1].ToString() + "," + Three_points_C_G[k][2].ToString(), Color.Black, true);


                    //Get C (Target belongs to [Three_points_Target_Lv[k][2],Three_points_Target_Lv[k][0]])
                    if (Three_points_Target_Lv[k][2] < Target_Lv && Target_Lv < Three_points_Target_Lv[k][0])
                    {
                        for (int i = 0; i <= 2; i++)
                        {
                            C_R[i] = Three_points_C_R[k][i];
                            C_G[i] = Three_points_C_G[k][i];
                            C_B[i] = Three_points_C_B[k][i];
                        }

                        f1.GB_Status_AppendText_Nextline("k : " + k.ToString() + " was selected for C", Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Target_LV : " + Target_Lv.ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][0] : " + Three_points_Target_Lv[k][0].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][1] : " + Three_points_Target_Lv[k][1].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][2] : " + Three_points_Target_Lv[k][2].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("--Show Final Selected C_G---", Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline(C_G[0].ToString() + "," + C_G[1].ToString() + "," + C_G[2].ToString(), Color.Blue, true);
                        break; //Break for k loop
                    }
                }

                //Get Calculated Voltage_R/G/B
                double Calculated_Target_Lv;
                int iteration;
                double Calculated_R_Vdata = 0;
                double Calculated_G_Vdata = 0;
                double Calculated_B_Vdata = 0;
                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0)
                    {
                        Calculated_G_Vdata = Vdata;

                        f1.GB_Status_AppendText_Nextline("When Calculated_Target_Lv < Target_Lv", Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv : " + Calculated_Target_Lv.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Target_Lv : " + Target_Lv.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Prvious_Gray_Gamma_G_Voltage : " + Prvious_Gray_Gamma_G_Voltage.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Calculated_G_Vdata : " + Calculated_G_Vdata.ToString(), Color.Green, true);

                        break;
                    }
                }

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Current_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Current_Band_AM1_RGB_Voltage = F7 + (Current_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata);
                Gamma_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata);
                Gamma_B = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata);
            }
        }




        private void Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_All_Band_3points_Fx_Input_Output_Reverse(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[][] Band_Gray_Gamma_Red, int[][] Band_Gray_Gamma_Green, int[][] Band_Gray_Gamma_Blue, double[][] Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            SJH_Matrix M = new SJH_Matrix();
            double ratio = 0.01;

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                double[][] Band_Gray_Gamma_Red_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Green_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Blue_Voltage = M.MatrixCreate(band, 8);
                for (int i = 0; i < band; i++)
                {
                    //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
                    Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Red[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Green[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Vreg1_Dec[i], Band_Gray_Gamma_Blue[i], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                }

                int Points_Num = band * 8;
                double[] Gamma_Red_Voltage_Points = new double[Points_Num];
                double[] Gamma_Green_Voltage_Points = new double[Points_Num];
                double[] Gamma_Blue_Voltage_Points = new double[Points_Num];
                double[] Target_Lv_Points = new double[Points_Num];

                for (int b = 0; b < band; b++)
                {
                    for (int g = 0; g < 8; g++)
                    {
                        Gamma_Red_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Red_Voltage[b][g] * ratio;
                        Gamma_Green_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Green_Voltage[b][g] * ratio;
                        Gamma_Blue_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Blue_Voltage[b][g] * ratio;
                        Target_Lv_Points[(8 * b) + g] = Band_Gray_Target_Lv[b][g] * ratio;
                    }
                }
                Target_Lv = (Target_Lv * ratio);

                Array.Sort<double>(Gamma_Red_Voltage_Points);
                Array.Sort<double>(Gamma_Green_Voltage_Points);
                Array.Sort<double>(Gamma_Blue_Voltage_Points);
                Array.Sort<double>(Target_Lv_Points);
                Array.Reverse(Target_Lv_Points);

                int Formula_Num = Points_Num - 2; //((band * 8) - 2 = Points_Num - 2) 
                f1.GB_Status_AppendText_Nextline("Formula_Num : " + Formula_Num.ToString(), Color.Red, true);

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
                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                    {
                        //Get "Three_points_Target_Lv"
                        Three_points_Gamma_Red_Voltage[k][i] = Gamma_Red_Voltage_Points[i + k];
                        Three_points_Gamma_Green_Voltage[k][i] = Gamma_Green_Voltage_Points[i + k];
                        Three_points_Gamma_Blue_Voltage[k][i] = Gamma_Blue_Voltage_Points[i + k];
                        Three_points_Target_Lv[k][i] = Target_Lv_Points[i + k];
                        count = 0;
                        //Get "A"
                        for (int j = 2; j >= 0; j--)
                        {
                            Temp_A_R[i][count] = Math.Pow(Target_Lv_Points[i + k], j);
                            Temp_A_G[i][count] = Math.Pow(Target_Lv_Points[i + k], j);
                            Temp_A_B[i][count] = Math.Pow(Target_Lv_Points[i + k], j);
                            count++;
                        }
                    }

                    //Show Temp A_G
                    f1.GB_Status_AppendText_Nextline("--Show Temp A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Temp_A_G[i][0].ToString() + "," + Temp_A_G[i][1].ToString() + "," + Temp_A_G[i][2].ToString(), Color.Black, true);

                    //Get Inv_A (by using "A")
                    Temp_Inv_A_R = M.MatrixInverse(Temp_A_R);
                    Temp_Inv_A_G = M.MatrixInverse(Temp_A_G);
                    Temp_Inv_A_B = M.MatrixInverse(Temp_A_B);

                    //Show Temp Inv_A
                    f1.GB_Status_AppendText_Nextline("--Show Temp_Inv_A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Temp_Inv_A_G[i][0].ToString() + "," + Temp_Inv_A_G[i][1].ToString() + "," + Temp_Inv_A_G[i][2].ToString(), Color.Black, true);

                    //Get C (by using "Inv_A" , "Three_points_Target_Lv")
                    Three_points_C_R[k] = M.Matrix_Multiply(Temp_Inv_A_R, Three_points_Gamma_Red_Voltage[k]);
                    Three_points_C_G[k] = M.Matrix_Multiply(Temp_Inv_A_G, Three_points_Gamma_Green_Voltage[k]);
                    Three_points_C_B[k] = M.Matrix_Multiply(Temp_Inv_A_B, Three_points_Gamma_Blue_Voltage[k]);

                    //Show Three_points_C_R
                    f1.GB_Status_AppendText_Nextline("--Show Three_points_C_G---", Color.Black, true);
                    f1.GB_Status_AppendText_Nextline(Three_points_C_G[k][0].ToString() + "," + Three_points_C_G[k][1].ToString() + "," + Three_points_C_G[k][2].ToString(), Color.Black, true);

                    //Get C (Target belongs to [Three_points_Target_Lv[k][2],Three_points_Target_Lv[k][0]])
                    if (Three_points_Target_Lv[k][2] < Target_Lv && Target_Lv < Three_points_Target_Lv[k][0])
                    {
                        for (int i = 0; i <= 2; i++)
                        {
                            C_R[i] = Three_points_C_R[k][i];
                            C_G[i] = Three_points_C_G[k][i];
                            C_B[i] = Three_points_C_B[k][i];
                        }

                        f1.GB_Status_AppendText_Nextline("k : " + k.ToString() + " was selected for C", Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Target_LV : " + Target_Lv.ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][0] : " + Three_points_Target_Lv[k][0].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][1] : " + Three_points_Target_Lv[k][1].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][2] : " + Three_points_Target_Lv[k][2].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("--Show Final Selected C_G---", Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline(C_G[0].ToString() + "," + C_G[1].ToString() + "," + C_G[2].ToString(), Color.Blue, true);
                        break; //Break for k loop
                    }
                }

                //Get Calculated Voltage_R/G/B
                int iteration;
                double Calculated_R_Vdata = 0;
                double Calculated_G_Vdata = 0;
                double Calculated_B_Vdata = 0;
                    
                iteration = 0;
                for (int j = 2; j >= 0; j--) Calculated_R_Vdata += (Math.Pow(Target_Lv, j) * C_R[iteration++]);
               
                iteration = 0;
                for (int j = 2; j >= 0; j--) Calculated_G_Vdata += (Math.Pow(Target_Lv, j) * C_G[iteration++]);

                iteration = 0;
                for (int j = 2; j >= 0; j--) Calculated_B_Vdata += (Math.Pow(Target_Lv, j) * C_B[iteration++]);

                f1.GB_Status_AppendText_Nextline("Calculated_G_Vdata : " + Calculated_G_Vdata.ToString(), Color.Blue, true);

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Current_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Current_Band_AM1_RGB_Voltage = F7 + (Current_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage * ratio, Prvious_Gray_Gamma_R_Voltage * ratio, Calculated_R_Vdata);
                Gamma_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage * ratio, Prvious_Gray_Gamma_G_Voltage * ratio, Calculated_G_Vdata);
                Gamma_B = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage * ratio, Prvious_Gray_Gamma_B_Voltage * ratio, Calculated_B_Vdata);
            }
        }



        private void Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_Band2_3points(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[] Band0_Gamma_Red, int[] Band0_Gamma_Green, int[] Band0_Gamma_Blue, int[] Band1_Gamma_Red, int[] Band1_Gamma_Green, int[] Band1_Gamma_Blue, double[] Band0_Target_Lv, double[] Band1_Target_Lv,
            int Current_Band_Dec_Vreg1, int band, int gray, double Target_Lv,int Band0_Vreg1_Dec,int Band1_Vreg1_Dec ,double Prvious_Gray_Gamma_R_Voltage,double Prvious_Gray_Gamma_G_Voltage,double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            SJH_Matrix M = new SJH_Matrix();

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                //Sorting
                double[] Band0_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band1_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band01_Gamma_Red_Voltage = new double[16];
                double[] Band01_Gamma_Green_Voltage = new double[16];
                double[] Band01_Gamma_Blue_Voltage = new double[16];

                double[] Band01_Target_Lv = new double[16];

                for (int i = 0; i < 8; i++)
                {
                    Band01_Gamma_Red_Voltage[i] = Band0_Gamma_Red_Voltage[i];
                    Band01_Gamma_Red_Voltage[i + 8] = Band1_Gamma_Red_Voltage[i];

                    Band01_Gamma_Green_Voltage[i] = Band0_Gamma_Green_Voltage[i];
                    Band01_Gamma_Green_Voltage[i + 8] = Band1_Gamma_Green_Voltage[i];

                    Band01_Gamma_Blue_Voltage[i] = Band0_Gamma_Blue_Voltage[i];
                    Band01_Gamma_Blue_Voltage[i + 8] = Band1_Gamma_Blue_Voltage[i];

                    Band01_Target_Lv[i] = Band0_Target_Lv[i];
                    Band01_Target_Lv[i + 8] = Band1_Target_Lv[i];
                }

                Array.Sort<double>(Band01_Gamma_Red_Voltage);
                Array.Sort<double>(Band01_Gamma_Green_Voltage);
                Array.Sort<double>(Band01_Gamma_Blue_Voltage);

                Array.Sort<double>(Band01_Target_Lv);
                Array.Reverse(Band01_Target_Lv);

               
                int Formula_Num = (band * 8) - 2; //(16 - 2 = 14) 
                f1.GB_Status_AppendText_Nextline("Formula_Num : " + Formula_Num.ToString(), Color.Red, true);

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
                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                    {
                        //Get "Three_points_Target_Lv"
                        Three_points_Target_Lv[k][i] = Band01_Target_Lv[i + k];
                        count = 0;
                        //Get "A"
                        for (int j = 2; j >= 0; j--)
                        {
                            Temp_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i + k], j);
                            Temp_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i + k], j);
                            Temp_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i + k], j);
                            count++;
                        }
                    }

                    //Show Temp A_G
                    f1.GB_Status_AppendText_Nextline("--Show Temp A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Temp_A_G[i][0].ToString() + "," + Temp_A_G[i][1].ToString() + "," + Temp_A_G[i][2].ToString(), Color.Black, true);
                        
                    

                    
                    //f1.GB_Status_AppendText_Nextline("-----", Color.Black);
                    //f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Black);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[0 + k] : " + Band01_Gamma_Red_Voltage[0 + k].ToString(), Color.Red);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[1 + k] : " + Band01_Gamma_Red_Voltage[1 + k].ToString(), Color.Red);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[2 + k] : " + Band01_Gamma_Red_Voltage[2 + k].ToString(), Color.Red);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[0 + k] : " + Band01_Gamma_Green_Voltage[0 + k].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[1 + k] : " + Band01_Gamma_Green_Voltage[1 + k].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[2 + k] : " + Band01_Gamma_Green_Voltage[2 + k].ToString(), Color.Green, true);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[0 + k] : " + Band01_Gamma_Blue_Voltage[0 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[1 + k] : " + Band01_Gamma_Blue_Voltage[1 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[2 + k] : " + Band01_Gamma_Blue_Voltage[2 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("-----", Color.Black);

                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][0] : " + Three_points_Target_Lv[k][0].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][1] : " + Three_points_Target_Lv[k][1].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][2] : " + Three_points_Target_Lv[k][2].ToString(), Color.Blue, true);



                    //Get Inv_A (by using "A")
                    Temp_Inv_A_R = M.MatrixInverse(Temp_A_R);
                    Temp_Inv_A_G = M.MatrixInverse(Temp_A_G);
                    Temp_Inv_A_B = M.MatrixInverse(Temp_A_B);


                    //Show Temp Inv_A
                    f1.GB_Status_AppendText_Nextline("--Show Temp_Inv_A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Temp_Inv_A_G[i][0].ToString() + "," + Temp_Inv_A_G[i][1].ToString() + "," + Temp_Inv_A_G[i][2].ToString(), Color.Black, true);
                    

                    //Get C (by using "Inv_A" , "Three_points_Target_Lv")
                    Three_points_C_R[k] = M.Matrix_Multiply(Temp_Inv_A_R, Three_points_Target_Lv[k]);
                    Three_points_C_G[k] = M.Matrix_Multiply(Temp_Inv_A_G, Three_points_Target_Lv[k]);
                    Three_points_C_B[k] = M.Matrix_Multiply(Temp_Inv_A_B, Three_points_Target_Lv[k]);


                    //Show Three_points_C_R
                    f1.GB_Status_AppendText_Nextline("--Show Three_points_C_G---", Color.Black, true);
                    f1.GB_Status_AppendText_Nextline(Three_points_C_G[k][0].ToString() + "," + Three_points_C_G[k][1].ToString() + "," + Three_points_C_G[k][2].ToString(), Color.Black, true);


                    //Get C (Target belongs to [Three_points_Target_Lv[k][2],Three_points_Target_Lv[k][0]])
                    if (Three_points_Target_Lv[k][2] < Target_Lv && Target_Lv < Three_points_Target_Lv[k][0])
                    {
                        for (int i = 0; i <= 2; i++)
                        {
                            C_R[i] = Three_points_C_R[k][i];
                            C_G[i] = Three_points_C_G[k][i];
                            C_B[i] = Three_points_C_B[k][i];
                        }

                        f1.GB_Status_AppendText_Nextline("k : " + k.ToString() + " was selected for C", Color.Blue,true);
                        f1.GB_Status_AppendText_Nextline("Target_LV : " + Target_Lv.ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][0] : " + Three_points_Target_Lv[k][0].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][1] : " + Three_points_Target_Lv[k][1].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("Three_points_Target_Lv[k][2] : " + Three_points_Target_Lv[k][2].ToString(), Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline("--Show Final Selected C_G---", Color.Blue, true);
                        f1.GB_Status_AppendText_Nextline(C_G[0].ToString() + "," + C_G[1].ToString() + "," + C_G[2].ToString(), Color.Blue, true);

                        break; //Break for k loop
                    }
                }

                //Get Calculated Voltage_R/G/B
                double Calculated_Target_Lv;
                int iteration;
                double Calculated_R_Vdata = 0;
                double Calculated_G_Vdata = 0;
                double Calculated_B_Vdata = 0;
                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0) 
                    {
                        Calculated_G_Vdata = Vdata;

                        f1.GB_Status_AppendText_Nextline("When Calculated_Target_Lv < Target_Lv", Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv : " + Calculated_Target_Lv.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Target_Lv : " + Target_Lv.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Prvious_Gray_Gamma_G_Voltage : " + Prvious_Gray_Gamma_G_Voltage.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Calculated_G_Vdata : " + Calculated_G_Vdata.ToString(), Color.Green, true);

                        break; 
                    }
                }

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Current_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Current_Band_AM1_RGB_Voltage = F7 + (Current_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata);
                Gamma_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata);
                Gamma_B = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata);
            }
        }


        private void Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_Band2_3points_Adjacent_Fx(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[] Band0_Gamma_Red, int[] Band0_Gamma_Green, int[] Band0_Gamma_Blue, int[] Band1_Gamma_Red, int[] Band1_Gamma_Green, int[] Band1_Gamma_Blue, double[] Band0_Target_Lv, double[] Band1_Target_Lv,
            int Current_Band_Dec_Vreg1, int band, int gray, double Target_Lv, int Band0_Vreg1_Dec, int Band1_Vreg1_Dec, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            SJH_Matrix M = new SJH_Matrix();

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                //Sorting
                double[] Band0_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band1_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band01_Gamma_Red_Voltage = new double[16];
                double[] Band01_Gamma_Green_Voltage = new double[16];
                double[] Band01_Gamma_Blue_Voltage = new double[16];

                double[] Band01_Target_Lv = new double[16];

                for (int i = 0; i < 8; i++)
                {
                    Band01_Gamma_Red_Voltage[i] = Band0_Gamma_Red_Voltage[i];
                    Band01_Gamma_Red_Voltage[i + 8] = Band1_Gamma_Red_Voltage[i];

                    Band01_Gamma_Green_Voltage[i] = Band0_Gamma_Green_Voltage[i];
                    Band01_Gamma_Green_Voltage[i + 8] = Band1_Gamma_Green_Voltage[i];

                    Band01_Gamma_Blue_Voltage[i] = Band0_Gamma_Blue_Voltage[i];
                    Band01_Gamma_Blue_Voltage[i + 8] = Band1_Gamma_Blue_Voltage[i];

                    Band01_Target_Lv[i] = Band0_Target_Lv[i];
                    Band01_Target_Lv[i + 8] = Band1_Target_Lv[i];
                }

                Array.Sort<double>(Band01_Gamma_Red_Voltage);
                Array.Sort<double>(Band01_Gamma_Green_Voltage);
                Array.Sort<double>(Band01_Gamma_Blue_Voltage);

                Array.Sort<double>(Band01_Target_Lv);
                Array.Reverse(Band01_Target_Lv);

                //First
                double[] First_3points_Target_Lv = new double[3];
                double[] First_3points_C_R = new double[3];
                double[] First_3points_C_G = new double[3];
                double[] First_3points_C_B = new double[3];

                double[][] First_3points_A_R = M.MatrixCreate(3, 3);
                double[][] First_3points_A_G = M.MatrixCreate(3, 3);
                double[][] First_3points_A_B = M.MatrixCreate(3, 3);
                double[][] First_3points_Inv_A_R = M.MatrixCreate(3, 3);
                double[][] First_3points_Inv_A_G = M.MatrixCreate(3, 3);
                double[][] First_3points_Inv_A_B = M.MatrixCreate(3, 3);

                //Second
                double[] Second_3points_Target_Lv = new double[3];
                double[] Second_3points_C_R = new double[3];
                double[] Second_3points_C_G = new double[3];
                double[] Second_3points_C_B = new double[3];

                double[][] Second_3points_A_R = M.MatrixCreate(3, 3);
                double[][] Second_3points_A_G = M.MatrixCreate(3, 3);
                double[][] Second_3points_A_B = M.MatrixCreate(3, 3);
                double[][] Second_3points_Inv_A_R = M.MatrixCreate(3, 3);
                double[][] Second_3points_Inv_A_G = M.MatrixCreate(3, 3);
                double[][] Second_3points_Inv_A_B = M.MatrixCreate(3, 3);


                int count = 0;
                int Formula_Num = (band * 8) - 2; //(16 - 2 = 14) 
                f1.GB_Status_AppendText_Nextline("Formula_Num : " + Formula_Num.ToString(), Color.Red, true);
                double[] C_R = new double[3];
                double[] C_G = new double[3];
                double[] C_B = new double[3];

                for (int k = 0; k < (Formula_Num - 1); k++)
                {
                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                    {
                        //Get "Three_points_Target_Lv"
                        First_3points_Target_Lv[i] = Band01_Target_Lv[i + k];
                        Second_3points_Target_Lv[i] = Band01_Target_Lv[i + (k + 1)];
                        count = 0;
                        //Get "A"
                        for (int j = 2; j >= 0; j--)
                        {
                            First_3points_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i + k], j);
                            First_3points_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i + k], j);
                            First_3points_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i + k], j);
                            
                            Second_3points_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i + (k + 1)], j);
                            Second_3points_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i + (k + 1)], j);
                            Second_3points_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i + (k + 1)], j);
                            
                            count++;
                        }
                    }

                    //Show Temp A_G
                    f1.GB_Status_AppendText_Nextline("--Show First_3points_A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(First_3points_A_G[i][0].ToString() + "," + First_3points_A_G[i][1].ToString() + "," + First_3points_A_G[i][2].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("--Show Second_3points_A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++)
                        f1.GB_Status_AppendText_Nextline(Second_3points_A_G[i][0].ToString() + "," + Second_3points_A_G[i][1].ToString() + "," + Second_3points_A_G[i][2].ToString(), Color.Red, true);




                    //f1.GB_Status_AppendText_Nextline("-----", Color.Black);
                    //f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Black);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[0 + k] : " + Band01_Gamma_Red_Voltage[0 + k].ToString(), Color.Red);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[1 + k] : " + Band01_Gamma_Red_Voltage[1 + k].ToString(), Color.Red);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Red_Voltage[2 + k] : " + Band01_Gamma_Red_Voltage[2 + k].ToString(), Color.Red);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[0 + k] : " + Band01_Gamma_Green_Voltage[0 + k].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[1 + k] : " + Band01_Gamma_Green_Voltage[1 + k].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("Band01_Gamma_Green_Voltage[2 + k] : " + Band01_Gamma_Green_Voltage[2 + k].ToString(), Color.Green, true);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[0 + k] : " + Band01_Gamma_Blue_Voltage[0 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[1 + k] : " + Band01_Gamma_Blue_Voltage[1 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("Band01_Gamma_Blue_Voltage[2 + k] : " + Band01_Gamma_Blue_Voltage[2 + k].ToString(), Color.Blue);
                    //f1.GB_Status_AppendText_Nextline("-----", Color.Black);

                    f1.GB_Status_AppendText_Nextline("k : " + k.ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("First_3points_points_Target_Lv[k][0] : " + First_3points_Target_Lv[0].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("First_3points_points_Target_Lv[k][1] : " + First_3points_Target_Lv[1].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("First_3points_points_Target_Lv[k][2] : " + First_3points_Target_Lv[2].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("Second_3points_points_Target_Lv[k][0] : " + Second_3points_Target_Lv[0].ToString(), Color.Red, true);
                    f1.GB_Status_AppendText_Nextline("Second_3points_points_Target_Lv[k][1] : " + Second_3points_Target_Lv[1].ToString(), Color.Red, true);
                    f1.GB_Status_AppendText_Nextline("Second_3points_points_Target_Lv[k][2] : " + Second_3points_Target_Lv[2].ToString(), Color.Red, true);


                    //Get Inv_A (by using "A")
                    First_3points_Inv_A_R = M.MatrixInverse(First_3points_A_R);
                    First_3points_Inv_A_G = M.MatrixInverse(First_3points_A_G);
                    First_3points_Inv_A_B = M.MatrixInverse(First_3points_A_B);
                    
                    Second_3points_Inv_A_R = M.MatrixInverse(Second_3points_A_R);
                    Second_3points_Inv_A_G = M.MatrixInverse(Second_3points_A_G);
                    Second_3points_Inv_A_B = M.MatrixInverse(Second_3points_A_B);


                    //Show Temp Inv_A
                    f1.GB_Status_AppendText_Nextline("--Show First_3points_Inv_A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++) f1.GB_Status_AppendText_Nextline(First_3points_Inv_A_G[i][0].ToString() + "," + First_3points_Inv_A_G[i][1].ToString() + "," + First_3points_Inv_A_G[i][2].ToString(), Color.Blue, true);
                    
                    f1.GB_Status_AppendText_Nextline("--Show Second_3points_Inv_A_G---", Color.Black, true);
                    for (int i = 0; i <= 2; i++) f1.GB_Status_AppendText_Nextline(Second_3points_Inv_A_G[i][0].ToString() + "," + Second_3points_Inv_A_G[i][1].ToString() + "," + Second_3points_Inv_A_G[i][2].ToString(), Color.Blue, true);


                    //Get C (by using "Inv_A" , "Three_points_Target_Lv")
                    First_3points_C_R = M.Matrix_Multiply(First_3points_Inv_A_R, First_3points_Target_Lv);
                    First_3points_C_G = M.Matrix_Multiply(First_3points_Inv_A_G, First_3points_Target_Lv);
                    First_3points_C_B = M.Matrix_Multiply(First_3points_Inv_A_B, First_3points_Target_Lv);

                    Second_3points_C_R = M.Matrix_Multiply(Second_3points_Inv_A_R, Second_3points_Target_Lv);
                    Second_3points_C_G = M.Matrix_Multiply(Second_3points_Inv_A_G, Second_3points_Target_Lv);
                    Second_3points_C_B = M.Matrix_Multiply(Second_3points_Inv_A_B, Second_3points_Target_Lv);

                    //Show Three_points_C_R
                    f1.GB_Status_AppendText_Nextline("--Show First_3points_C_G---", Color.Black, true);
                    f1.GB_Status_AppendText_Nextline(First_3points_C_G[0].ToString() + "," + First_3points_C_G[1].ToString() + "," + First_3points_C_G[2].ToString(), Color.Blue, true);

                    f1.GB_Status_AppendText_Nextline("--Show Second_3points_C_G---", Color.Black, true);
                    f1.GB_Status_AppendText_Nextline(Second_3points_C_G[0].ToString() + "," + Second_3points_C_G[1].ToString() + "," + Second_3points_C_G[2].ToString(), Color.Red, true);


                    //Get C (Target belongs to [Three_points_Target_Lv[k][2],Three_points_Target_Lv[k][0]])
                    //Region [b,a] in range(d~c~b~a)

                    f1.GB_Status_AppendText_Nextline("a = First_3points_Target_Lv[0] = " + First_3points_Target_Lv[0].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("b = First_3points_Target_Lv[1] = " + First_3points_Target_Lv[1].ToString(), Color.Blue, true);
                    f1.GB_Status_AppendText_Nextline("b = First_3points_Target_Lv[1] = " + First_3points_Target_Lv[1].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("c = Second_3points_Target_Lv[1] = " + Second_3points_Target_Lv[1].ToString(), Color.Green, true);
                    f1.GB_Status_AppendText_Nextline("c = Second_3points_Target_Lv[1] = " + Second_3points_Target_Lv[1].ToString(), Color.Red, true);
                    f1.GB_Status_AppendText_Nextline("d = Second_3points_Target_Lv[2] = " + Second_3points_Target_Lv[2].ToString(), Color.Red, true);

                    if (k == 0)
                    {
                        if ((First_3points_Target_Lv[1] <= Target_Lv) && (Target_Lv <= First_3points_Target_Lv[0]))
                        {
                            f1.GB_Status_AppendText_Nextline("In Region [b,a], select first f(x)", Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("a = First_3points_Target_Lv[0] = " + First_3points_Target_Lv[0].ToString(), Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("Target_Lv = " + Target_Lv.ToString(), Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("b = First_3points_Target_Lv[1] = " + First_3points_Target_Lv[1].ToString(), Color.Blue, true);
                            for (int i = 0; i <= 2; i++)
                            {
                                C_R[i] = First_3points_C_R[i];
                                C_G[i] = First_3points_C_G[i];
                                C_B[i] = First_3points_C_B[i];
                            }
                            break;
                        }
                        else
                        {
                            f1.GB_Status_AppendText_Nextline("k = 0", Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("In Region [b,a], select first f(x)", Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("a = First_3points_Target_Lv[0] = " + First_3points_Target_Lv[0].ToString(), Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("b = First_3points_Target_Lv[1] = " + First_3points_Target_Lv[1].ToString(), Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("Target_Lv = " + Target_Lv.ToString(), Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("Out of [b,a] Region, Continue to find F(x)1st/2nd (k++)", Color.Black, true);
                        }
                    }
                    else
                    {
                        //Region (c,b) in range(d~c~b~a)
                        if ((Second_3points_Target_Lv[1] < Target_Lv) && (Target_Lv < First_3points_Target_Lv[1]))
                        {
                            double Distance_1st = First_3points_Target_Lv[1] - Target_Lv;
                            double Distance_2nd = Target_Lv - Second_3points_Target_Lv[1];

                            f1.GB_Status_AppendText_Nextline("In Region (c,b),need to compare distance", Color.Green, true);
                            f1.GB_Status_AppendText_Nextline("b = First_3points_Target_Lv[1] = " + First_3points_Target_Lv[1].ToString(), Color.Green, true);
                            f1.GB_Status_AppendText_Nextline("Target_Lv = " + Target_Lv.ToString(), Color.Green, true);
                            f1.GB_Status_AppendText_Nextline("c = Second_3points_Target_Lv[1] = " + Second_3points_Target_Lv[1].ToString(), Color.Green, true);
                            f1.GB_Status_AppendText_Nextline("Distance_1st : " + Distance_1st.ToString(), Color.Blue, true);
                            f1.GB_Status_AppendText_Nextline("Distance_2nd : " + Distance_2nd.ToString(), Color.Red, true);

                            if (Distance_1st < Distance_2nd)
                            {
                                f1.GB_Status_AppendText_Nextline("Distance_1st < Distance_2nd , first f(x) was selected", Color.Blue, true);
                                for (int i = 0; i <= 2; i++)
                                {
                                    C_R[i] = First_3points_C_R[i];
                                    C_G[i] = First_3points_C_G[i];
                                    C_B[i] = First_3points_C_B[i];
                                }
                            }
                            else
                            {
                                f1.GB_Status_AppendText_Nextline("Distance_1st >= Distance_2nd , second f(x) was selected", Color.Red, true);
                                for (int i = 0; i <= 2; i++)
                                {
                                    C_R[i] = Second_3points_C_R[i];
                                    C_G[i] = Second_3points_C_G[i];
                                    C_B[i] = Second_3points_C_B[i];
                                }
                            }
                            break;
                        }
                        else
                        {
                            if (k == (Formula_Num - 2))
                            {
                                f1.GB_Status_AppendText_Nextline("Final k point(k = (Formula_Num - 2)", Color.Red, true);
                                f1.GB_Status_AppendText_Nextline("c = Second_3points_Target_Lv[1] = " + Second_3points_Target_Lv[1].ToString(), Color.Red, true);
                                f1.GB_Status_AppendText_Nextline("Target_Lv = " + Target_Lv.ToString(), Color.Red, true);
                                f1.GB_Status_AppendText_Nextline("d = Second_3points_Target_Lv[2] = " + Second_3points_Target_Lv[2].ToString(), Color.Red, true);
                                for (int i = 0; i <= 2; i++)
                                {
                                    C_R[i] = Second_3points_C_R[i];
                                    C_G[i] = Second_3points_C_G[i];
                                    C_B[i] = Second_3points_C_B[i];
                                }
                                break;
                            }
                            else
                            {
                                f1.GB_Status_AppendText_Nextline("k = " + k.ToString(), Color.Black, true);
                                f1.GB_Status_AppendText_Nextline("b = First_3points_Target_Lv[1] = " + First_3points_Target_Lv[1].ToString(), Color.Green, true);
                                f1.GB_Status_AppendText_Nextline("c = Second_3points_Target_Lv[1] = " + Second_3points_Target_Lv[1].ToString(), Color.Green, true);
                                f1.GB_Status_AppendText_Nextline("Target_Lv = " + Target_Lv.ToString(), Color.Green, true);
                                f1.GB_Status_AppendText_Nextline("Out of [c,b] Region, Continue to find F(x)1st/2nd (k++)", Color.Black, true);
                            }
                        }
                    }

                }

                //Get Calculated Voltage_R/G/B
                double Calculated_Target_Lv;
                int iteration;
                double Calculated_R_Vdata = 0;
                double Calculated_G_Vdata = 0;
                double Calculated_B_Vdata = 0;
                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0)
                    {
                        Calculated_G_Vdata = Vdata;

                        f1.GB_Status_AppendText_Nextline("When Calculated_Target_Lv < Target_Lv", Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv : " + Calculated_Target_Lv.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Target_Lv : " + Target_Lv.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Prvious_Gray_Gamma_G_Voltage : " + Prvious_Gray_Gamma_G_Voltage.ToString(), Color.Green, true);
                        f1.GB_Status_AppendText_Nextline("Calculated_G_Vdata : " + Calculated_G_Vdata.ToString(), Color.Green, true);

                        break;
                    }
                }

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Current_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Current_Band_AM1_RGB_Voltage = F7 + (Current_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata);
                Gamma_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata);
                Gamma_B = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata);
            }
        }


        private void Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[] HBM_Gamma_R, int[] HBM_Gamma_G, int[] HBM_Gamma_B, double[] HBM_Target_Lv,
            int Current_Band_Dec_Vreg1, int band, int gray, double Target_Lv,double Prvious_Gray_Gamma_R_Voltage,double Prvious_Gray_Gamma_G_Voltage,double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            SJH_Matrix M = new SJH_Matrix();

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                double[] HBM_Gamma_R_Voltage = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_R, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] HBM_Gamma_G_Voltage = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_G, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] HBM_Gamma_B_Voltage = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_B, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[][] A_R = M.MatrixCreate(8, 8);
                double[][] A_G = M.MatrixCreate(8, 8);
                double[][] A_B = M.MatrixCreate(8, 8);
                double[][] Inv_A_R = M.MatrixCreate(8, 8);
                double[][] Inv_A_G = M.MatrixCreate(8, 8);
                double[][] Inv_A_B = M.MatrixCreate(8, 8);
                double[] C_R = new double[8];
                double[] C_G = new double[8];
                double[] C_B = new double[8];

                //Get A[i][count] = HBM_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= 7; i++)
                {
                    count = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        A_R[i][count] = Math.Pow(HBM_Gamma_R_Voltage[i], j);
                        A_G[i][count] = Math.Pow(HBM_Gamma_G_Voltage[i], j);
                        A_B[i][count] = Math.Pow(HBM_Gamma_B_Voltage[i], j);
                        count++;
                    }
                }

                //Get C[8] = inv(A)[8,8] * HBM_Target_Lv[8]
                Inv_A_R = M.MatrixInverse(A_R);
                Inv_A_G = M.MatrixInverse(A_G);
                Inv_A_B = M.MatrixInverse(A_B);
                C_R = M.Matrix_Multiply(Inv_A_R, HBM_Target_Lv);
                C_G = M.Matrix_Multiply(Inv_A_G, HBM_Target_Lv);
                C_B = M.Matrix_Multiply(Inv_A_B, HBM_Target_Lv);

                //Get Calculated Voltage_R/G/B
                double Calculated_Target_Lv;
                int iteration;
                double Calculated_R_Vdata = 0;
                double Calculated_G_Vdata = 0;
                double Calculated_B_Vdata = 0;
                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0) { Calculated_G_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Current_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Current_Band_AM1_RGB_Voltage = F7 + (Current_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata);
                Gamma_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata);
                Gamma_B = Meta_Engineering.Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata);
            }
        }


        private void Get_First_Gamma_Fx_HBM_Mode(ref RGB Gamma, int band, int gray,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];

            SJH_Matrix M = new SJH_Matrix();
            RGB Rev_Gamma = new RGB();

            double[] Lv = new double[8];
            double[] Vdata_R = new double[8];
            double[] Vdata_G = new double[8];
            double[] Vdata_B = new double[8];
            double[][] A_R = M.MatrixCreate(8, 8);
            double[][] A_G = M.MatrixCreate(8, 8);
            double[][] A_B = M.MatrixCreate(8, 8);



            Meta_Form.button_Vreg1_Read_Click();
            int[] Dec_Vreg1 = new int[10];
            double[] Vreg1_Voltage = new double[10];
            Dec_Vreg1[0] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B0.Text);
            Dec_Vreg1[1] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B1.Text);
            Dec_Vreg1[2] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B2.Text);
            Dec_Vreg1[3] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B3.Text);
            Dec_Vreg1[4] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B4.Text);
            Dec_Vreg1[5] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B5.Text);
            Dec_Vreg1[6] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B6.Text);
            Dec_Vreg1[7] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B7.Text);
            Dec_Vreg1[8] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B8.Text);
            Dec_Vreg1[9] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B9.Text);
            double[] AM1_RGB_Voltage = new double[10];


            for (int b = 0; b < 10; b++)
            {
                Vreg1_Voltage[b] = Meta_Engineering.Get_Vreg1_Voltage(Dec_Vreg1[b], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                AM1_RGB_Voltage[b] = F7 + (Vreg1_Voltage[b] - F7) * (8.0 / 700.0);
                //f1.GB_Status_AppendText_Nextline(b.ToString() + ") Vreg1_Dec / Vreg1_Voltage / AM1_Voltage = " + Dec_Vreg1[b].ToString() + " / " + Vreg1_Voltage[b].ToString() + " / " + AM1_RGB_Voltage[b].ToString(), Color.Black);
            }

            RGB[,] Temp_Gamma = new RGB[10, 8];
            XYLv[,] Temp_Target = new XYLv[10, 8];
            RGB_Double[,] Gamma_Voltage = new RGB_Double[10, 8];

            int OC_row_length = Meta_Engineering.dataGridView_OC_param.RowCount;
            for (int i = 2; i < OC_row_length; i++)
            {
                int b = (i - 2) / 8;
                int g = (i - 2) % 8;
                Temp_Gamma[b, g].int_R = Convert.ToInt16(Meta_Engineering.dataGridView_OC_param.Rows[i].Cells[1].Value);
                Temp_Gamma[b, g].int_G = Convert.ToInt16(Meta_Engineering.dataGridView_OC_param.Rows[i].Cells[2].Value);
                Temp_Gamma[b, g].int_B = Convert.ToInt16(Meta_Engineering.dataGridView_OC_param.Rows[i].Cells[3].Value);
                Temp_Target[b, g].double_Lv = Convert.ToDouble(Meta_Engineering.dataGridView_OC_param.Rows[i].Cells[9].Value);
                //Read Gamma From DataGridView Test OK
                //f1.GB_Status_AppendText_Nextline("F [B/G] Gamma R/G/B= [" + b.ToString() + "/" + g.ToString() + "]" + Temp_Gamma[b, g].int_R.ToString() + "/" + Temp_Gamma[b, g].int_G.ToString() + "/" + Temp_Gamma[b, g].int_B.ToString(), Color.Blue);

                if (g == 0)//G255 (AM2)
                {
                    Gamma_Voltage[b, g].double_R = Meta_Engineering.Get_AM2_Voltage(F7, Vreg1_Voltage[b], Temp_Gamma[b, g].int_R);
                    Gamma_Voltage[b, g].double_G = Meta_Engineering.Get_AM2_Voltage(F7, Vreg1_Voltage[b], Temp_Gamma[b, g].int_G);
                    Gamma_Voltage[b, g].double_B = Meta_Engineering.Get_AM2_Voltage(F7, Vreg1_Voltage[b], Temp_Gamma[b, g].int_B);

                    //Reverse Mode (Vdata(AM2) -> Gamma) Test OK 
                    //Reverse_Gamma.int_R = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma_Voltage[band, gray].double_R);
                    //Reverse_Gamma.int_G = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma_Voltage[band, gray].double_G);
                    //Reverse_Gamma.int_B = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma_Voltage[band, gray].double_B);
                    //f1.GB_Status_AppendText_Nextline("R [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Reverse_Gamma.int_R.ToString() + "/" + Reverse_Gamma.int_G.ToString() + "/" + Reverse_Gamma.int_B.ToString(), Color.Red);

                }
                else
                {
                    Gamma_Voltage[b, g].double_R = Meta_Engineering.Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[b], Gamma_Voltage[b, g - 1].double_R, Temp_Gamma[b, g].int_R);
                    Gamma_Voltage[b, g].double_G = Meta_Engineering.Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[b], Gamma_Voltage[b, g - 1].double_G, Temp_Gamma[b, g].int_G);
                    Gamma_Voltage[b, g].double_B = Meta_Engineering.Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[b], Gamma_Voltage[b, g - 1].double_B, Temp_Gamma[b, g].int_B);

                    //Reverse Mode (Vdata(Normal) -> Gamma) Test OK
                    //Reverse_Gamma.int_R = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_R, Gamma_Voltage[band, gray].double_R);
                    //Reverse_Gamma.int_G = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_G, Gamma_Voltage[band, gray].double_G);
                    //Reverse_Gamma.int_B = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_B, Gamma_Voltage[band, gray].double_B);
                    //f1.GB_Status_AppendText_Nextline("R [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Reverse_Gamma.int_R.ToString() + "/" + Reverse_Gamma.int_G.ToString() + "/" + Reverse_Gamma.int_B.ToString(), Color.Red);

                }
            }


            //Get A and Lv
            int count = 0;
            for (int i = 0; i <= 7; i++)
            {
                Vdata_R[i] = Gamma_Voltage[0, i].double_R;
                Vdata_G[i] = Gamma_Voltage[0, i].double_G;
                Vdata_B[i] = Gamma_Voltage[0, i].double_B;
                Lv[i] = Temp_Target[0, i].double_Lv;
                count = 0;
                for (int j = 7; j >= 0; j--)
                {
                    A_R[i][count] = Math.Pow(Vdata_R[i], j);
                    A_G[i][count] = Math.Pow(Vdata_G[i], j);
                    A_B[i][count] = Math.Pow(Vdata_B[i], j);
                    count++;
                }
            }

            //Show A and Lv
            string[] Temp_R = new string[8];
            string[] Temp_G = new string[8];
            string[] Temp_B = new string[8];
            for (int i = 0; i <= 7; i++)
            {
                Temp_R[i] = (Lv[i].ToString() + "(nit):");
                Temp_G[i] = (Lv[i].ToString() + "(nit):");
                Temp_B[i] = (Lv[i].ToString() + "(nit):");
                for (int j = 0; j <= 7; j++)
                {
                    Temp_R[i] += (" " + A_R[i][j].ToString());
                    Temp_G[i] += (" " + A_G[i][j].ToString());
                    Temp_B[i] += (" " + A_B[i][j].ToString());
                }
                //f1.GB_Status_AppendText_Nextline(Temp_R[i], Color.Red);
                //f1.GB_Status_AppendText_Nextline(Temp_G[i], Color.Green);
                //f1.GB_Status_AppendText_Nextline(Temp_B[i], Color.Blue);
            }


            //Get C = inv(A) * Lv
            double[][] Inv_A_R = M.MatrixCreate(8, 8);
            double[][] Inv_A_G = M.MatrixCreate(8, 8);
            double[][] Inv_A_B = M.MatrixCreate(8, 8);

            double[] C_R = new double[8];
            double[] C_G = new double[8];
            double[] C_B = new double[8];
            Inv_A_R = M.MatrixInverse(A_R);
            Inv_A_G = M.MatrixInverse(A_G);
            Inv_A_B = M.MatrixInverse(A_B);
            C_R = M.Matrix_Multiply(Inv_A_R, Lv);
            C_G = M.Matrix_Multiply(Inv_A_G, Lv);
            C_B = M.Matrix_Multiply(Inv_A_B, Lv);

            //Show C
            string temp_C_R = "R :";
            string temp_C_G = "G :";
            string temp_C_B = "B :";
            for (int i = 0; i <= 7; i++)
            {
                temp_C_R += (" " + C_R[i].ToString());
                temp_C_G += (" " + C_G[i].ToString());
                temp_C_B += (" " + C_B[i].ToString());
            }
            //f1.GB_Status_AppendText_Nextline(temp_C_R, Color.Red);
            //f1.GB_Status_AppendText_Nextline(temp_C_G, Color.Green);
            //f1.GB_Status_AppendText_Nextline(temp_C_B, Color.Blue);

            //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
            
            double Calculated_Target_Lv;
            int iteration;
            f1.GB_Status_AppendText_Nextline("Target_Lv : " + Temp_Target[band,gray].double_Lv.ToString(), Color.Black);

            if (band == 1)
            {
                Calculated_Vdata[band - 1, gray].double_R = Gamma_Voltage[0, gray].double_R;
                Calculated_Vdata[band - 1, gray].double_G = Gamma_Voltage[0, gray].double_G;
                Calculated_Vdata[band - 1, gray].double_B = Gamma_Voltage[0, gray].double_B;
            }

            Calculated_Vdata[band, gray].double_R = 0;
            Calculated_Vdata[band, gray].double_G = 0;
            Calculated_Vdata[band, gray].double_B = 0;
            Calculated_Target_Lv = 0;

            //f1.GB_Status_AppendText_Nextline("Calculated_Vdata[band - 1, gray].double_R : " + Calculated_Vdata[band - 1, gray].double_R.ToString() , Color.Red);
            //f1.GB_Status_AppendText_Nextline("Calculated_Vdata[band - 1, gray].double_G : " + Calculated_Vdata[band - 1, gray].double_G.ToString(), Color.Green);
            //f1.GB_Status_AppendText_Nextline("Calculated_Vdata[band - 1, gray].double_B : " + Calculated_Vdata[band - 1, gray].double_B.ToString(), Color.Blue);

            for (double Vdata = Calculated_Vdata[band - 1, gray].double_R; Vdata <= F7; Vdata += 0.001)
            {
                if (Vdata > (F7 - 0.002))
                {
                    Calculated_Vdata[band, gray].double_R = Vdata;
                    break;
                }

                Calculated_Target_Lv = 0;
                iteration = 0;
                for (int j = 7; j >= 0; j--)
                {
                    Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_R[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Red);
                }

                //f1.GB_Status_AppendText_Nextline("R_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Red);
                if ((Calculated_Target_Lv < Temp_Target[band, gray].double_Lv) && Calculated_Vdata[band, gray].double_R == 0)
                {
                    Calculated_Vdata[band, gray].double_R = Vdata;
                    break;
                }
            }


            for (double Vdata = Calculated_Vdata[band - 1, gray].double_G; Vdata <= F7; Vdata += 0.001)
            {
                if (Vdata > (F7 - 0.002))
                {
                    Calculated_Vdata[band, gray].double_G = Vdata;
                    break;
                }
                Calculated_Target_Lv = 0;
                iteration = 0;
                for (int j = 7; j >= 0; j--)
                {
                    Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_G[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Green);
                }

                //f1.GB_Status_AppendText_Nextline("G_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Green);
                if ((Calculated_Target_Lv < Temp_Target[band, gray].double_Lv) && Calculated_Vdata[band, gray].double_G == 0)
                {
                    Calculated_Vdata[band, gray].double_G = Vdata;
                    break;
                }
            }



            for (double Vdata = Calculated_Vdata[band - 1, gray].double_B; Vdata <= F7; Vdata += 0.001)
            {
                if (Vdata > (F7 - 0.002))
                {
                    Calculated_Vdata[band, gray].double_B = Vdata;
                    break;
                }

                Calculated_Target_Lv = 0;
                iteration = 0;
                for (int j = 7; j >= 0; j--)
                {
                    Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_B[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Blue);
                }
                //f1.GB_Status_AppendText_Nextline("B_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Blue);
                if ((Calculated_Target_Lv < Temp_Target[band, gray].double_Lv) && Calculated_Vdata[band, gray].double_B == 0)
                {
                    Calculated_Vdata[band, gray].double_B = Vdata;
                    break;
                }
            }

            //f1.GB_Status_AppendText_Nextline("==========================================", Color.Black);
            //f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "/" + gray.ToString() + "]Calculated_Vdata_R = " + Calculated_Vdata[band, gray].double_R, Color.Red);
            //f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "/" + gray.ToString() + "]Calculated_Vdata_G = " + Calculated_Vdata[band, gray].double_G, Color.Green);
            //f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "/" + gray.ToString() + "]Calculated_Vdata_B = " + Calculated_Vdata[band, gray].double_B, Color.Blue);

            if (gray == 0)//G255 (AM2)
            {
                Rev_Gamma.int_R = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Calculated_Vdata[band, gray].double_R);
                Rev_Gamma.int_G = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Calculated_Vdata[band, gray].double_G);
                Rev_Gamma.int_B = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Calculated_Vdata[band, gray].double_B);
                f1.GB_Status_AppendText_Nextline("Rev [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Rev_Gamma.int_R.ToString() + "/" + Rev_Gamma.int_G.ToString() + "/" + Rev_Gamma.int_B.ToString(), Color.Red);

            }
            else
            {
                Rev_Gamma.int_R = Meta_Engineering.Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_R, Calculated_Vdata[band, gray].double_R);
                Rev_Gamma.int_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_G, Calculated_Vdata[band, gray].double_G);
                Rev_Gamma.int_B = Meta_Engineering.Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_B, Calculated_Vdata[band, gray].double_B);
                f1.GB_Status_AppendText_Nextline("Rev [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Rev_Gamma.int_R.ToString() + "/" + Rev_Gamma.int_G.ToString() + "/" + Rev_Gamma.int_B.ToString(), Color.Red);
                //f1.GB_Status_AppendText_Nextline("AM1_RGB_Voltage[band] : " + AM1_RGB_Voltage[band].ToString(), Color.Red);
                //f1.GB_Status_AppendText_Nextline("Calculated_Vdata[band, gray - 1].double_R : " + Gamma_Voltage[band, gray - 1].double_R.ToString(), Color.Red);
                //f1.GB_Status_AppendText_Nextline("Calculated_Vdata[band, gray].double_R" + Calculated_Vdata[band, gray].double_R.ToString(), Color.Red);
            }

            Gamma.int_R = Rev_Gamma.int_R;
            Gamma.int_G = Rev_Gamma.int_G;
            Gamma.int_B = Rev_Gamma.int_B;
        }


        private void HBM_Mode_Gray255_Compensation()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode.getInstance().Show();
            Meta_Engineer_Mornitoring_Mode form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_DBV_Setting(0);//HBM DBV Setting
            Meta_Pattern_Setting(0);//HBM Gray255 Pattern Setting
            Thread.Sleep(300); //Pattern 안정화 Time
            form_engineer.Band_Radiobuttion_Select(0);//Select HBM
            Get_Param(0, ref Gamma, ref Target, ref Limit, ref Extension); //Get (First) Gray255 Gamma,Target,Limit From OC-Param-Table 
            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

            Optic_Compensation_Succeed = false;
            Optic_Compensation_Stop = false;
            int loop_count = 0;
            Infinite_Count = 0;
            Infinite = false;

            RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
            Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma,Gamma, 0, 0); //Setting Gamma Values for HBM/Gray255
            Thread.Sleep(50);

            f1.GB_Status_AppendText_Nextline("HBM/Gray255 Compensation Start", Color.Green);

            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
            {
                f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure

                Infinite_Loop_Check(loop_count);
                Prev_Gamma.Equal_Value(Gamma);

                Sub_Compensation(loop_count, Infinite, Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Normal_Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                Diff_Gamma = Gamma - Prev_Gamma;
                f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                else RGB_Need_To_Change[0] = false;
                if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                else RGB_Need_To_Change[1] = false;
                if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                else RGB_Need_To_Change[2] = false;

                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0); //Setting Gamma Values for HBM/Gray255
                Thread.Sleep(50);

                Update_Engineering_Sheet(Gamma, Measure, 0, 0, loop_count, "X");

                if (Within_Spec_Limit)
                {
                    Optic_Compensation_Succeed = true;
                    break;
                }

                if (Gamma_Out_Of_Register_Limit)
                {
                    Optic_Compensation_Stop = true;
                    System.Windows.Forms.MessageBox.Show("Red/Vreg1/Blue is out of Limit");
                    break;
                }
                textBox_loop_count.Text = (++loop_count).ToString();

                if (loop_count == 300)
                {
                    Optic_Compensation_Succeed = false;
                    System.Windows.Forms.MessageBox.Show("HBM Gray255" + "Loop Count Over");
                    if (this.checkBox_Continue_After_Fail.Checked == false)
                        Optic_Compensation_Stop = true;
                    break;
                }
            }
            f1.GB_Status_AppendText_Nextline("HBM / Gray255 Compensation Finish", Color.Green);
        }


        private void Single_Mode_Optic_compensation()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            //datagridview-related
            Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_form_engineer.Engineering_Mode_DataGridview_ReadOnly(true);
            Meta_form_engineer.GridView_Measure_Applied_Loop_Area_Data_Clear();
            Meta_form_engineer.Gamma_Vreg1_Diff_Clear();
            Meta_form_engineer.Gamma_Vdata_Clear(); //Add on 191028
            Meta_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB();
            Application.DoEvents();

            //Initialize
            Optic_Compensation_Stop = false;
            Application.DoEvents();

            //CA channel Selection
            f1.objMemory.ChannelNO = Convert.ToInt32(f1.textBox_ch.Text);
            f1.CA_Channel_trackBar.Value = Convert.ToInt32(f1.textBox_ch.Text);
            
            f1.OC_Timer_Start();

            //dll-related variables
            Gamma_Out_Of_Register_Limit = false;
            Within_Spec_Limit = false;

            //Optic Compensation variables
            int band = 0;
            int gray = 0;
            Vreg1 = 0;
            int loop_count = 0;
            int Vreg1_loop_count = 0;
            int loop_count_max = Convert.ToInt16(textBox_Max_Loop.Text);
            int total_average_loop_count = 0;
            int Initial_Vreg1 = 0;
            double Skip_Lv = Convert.ToDouble(textBox_Subcompensation_GB_skip_Lv.Text);
            RGB Prev_Band_Gray255_Gamma = new RGB();
            Optic_Compensation_Succeed = false;
            RGB Gamma_Init = new RGB();

            //ProgressBar-related
            int step = 1;
            f1.Set_GB_ProgressBar_Value(0); //Progressbar Value's initializing
            bool Any_Band_is_Selected = ProgressBar_Max_Step_Setting(step); //Set Progressbar's Step and Max-Value

            if (checkBox_Send_Manual_Code.Checked)
            {
                f1.PNC_Manual_Button_Click();
                Thread.Sleep(3000); //Manual Code 안정화 Time
            }
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");//#MCAP - Manufactuer ACCESS

            //Add on 191028 (About RGB_Vdata)
            f1.GB_Status_AppendText_Nextline("*Assumption : ELVDD = 4.6v , DDVDH = 6.7v , GREF_SEL = 1 , GA_INV = 1,VG4_EN =1 , AVAMODE = 0", Color.Red);
            //Get ELVDD - FV1_LVL[5:0],ELVDD + VCI1_LVL[5:0]
            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            double ELVDD = 4.6;

            string FV1 = D0_Hex[18];
            int dec_FV1 = Convert.ToInt16(FV1, 16);
            if (dec_FV1 >= 42) dec_FV1 = 42;
            double E7 = ELVDD - (0.2 + (dec_FV1 * 0.1)); //ELVDD - FV1_LVL[5:0]

            string VCI1 = D0_Hex[16];
            int dec_VCI1 = Convert.ToInt16(VCI1, 16);
            if (dec_VCI1 >= 42) dec_VCI1 = 42;
            double F7 = ELVDD + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]

            //Get VREG1_REF818[5:0],VREG1_REF614[5:0],VREG1_REF409[5:0],VREG1_REF205[5:0]
            int Dec_VREG1_REF818 = Convert.ToInt16(D0_Hex[1], 16) & 0x3F;
            int Dec_VREG1_REF614 = Convert.ToInt16(D0_Hex[2], 16) & 0x3F;
            int Dec_VREG1_REF409 = Convert.ToInt16(D0_Hex[3], 16) & 0x3F;
            int Dec_VREG1_REF205 = Convert.ToInt16(D0_Hex[4], 16) & 0x3F;

            double VREG1_REF818_volt = F7 + (E7 - F7) * ((222.5 + 0.5 * Dec_VREG1_REF818) / 254);
            double VREG1_REF614_volt = F7 + (E7 - F7) * ((206.5 + 0.5 * Dec_VREG1_REF614) / 254);
            double VREG1_REF409_volt = F7 + (E7 - F7) * ((182.5 + 0.5 * Dec_VREG1_REF409) / 254);
            double VREG1_REF205_volt = F7 + (E7 - F7) * ((154.5 + 0.5 * Dec_VREG1_REF205) / 254);

            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) - FV1_LVL(v) = " + E7.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF818(v) = " + VREG1_REF818_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF614(v) = " + VREG1_REF614_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF409(v) = " + VREG1_REF409_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF205(v) = " + VREG1_REF205_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) + VCI1_LVL(v) = " + F7.ToString(), Color.Blue);

            double Vreg1_Voltage;
            double AM1_RGB_Voltage;
            RGB_Double[] Gamma_Voltage = new RGB_Double[8];//8ea Gray-points

            if (checkBox_Read_DBV_Values.Checked) button_Read_DBV_Setting.PerformClick();

            Get_All_Band_Gray_Gamma(All_band_gray_Gamma); //Get All_band_gray_Gamma[12,8]
            
            //POCB Off
            SW43410_POCB_Off_btn.PerformClick();

            // Black Compensation
            if (checkBox_Black_Compensation.Checked && Optic_Compensation_Stop == false)
            {
                if (Black_Compensation())
                {
                    f1.GB_Status_AppendText_Nextline("Black Compensation Ok", Color.Blue);
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("Black Compensation NG", Color.Red);
                    Optic_Compensation_Stop = true;
                }
            }

            // ELVSS Compensation
            if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false)
            {
                Get_Param(0, ref Gamma, ref Target, ref Limit, ref Extension);//Get (First)Gamma,Target,Limit From OC-Param-Table
                Gamma_Init.Equal_Value(Gamma);//190529
                HBM_Mode_Gray255_Compensation();
                ELVSS_Compensation();
            }

            if (checkBox_Apply_FX_Previous_Band_Vreg1.Checked || checkBox_Apply_FX_HBM_RGB.Checked)
            {
                Selected_Band[0] = checkBox_Fast_OC_B0.Checked;
                Selected_Band[1] = checkBox_Fast_OC_B1.Checked;
                Selected_Band[2] = checkBox_Fast_OC_B2.Checked;
                Selected_Band[3] = checkBox_Fast_OC_B3.Checked;
                Selected_Band[4] = checkBox_Fast_OC_B4.Checked;
                Selected_Band[5] = checkBox_Fast_OC_B5.Checked;
                Selected_Band[6] = checkBox_Fast_OC_B6.Checked;
                Selected_Band[7] = checkBox_Fast_OC_B7.Checked;
                Selected_Band[8] = checkBox_Fast_OC_B8.Checked;
                Selected_Band[9] = checkBox_Fast_OC_B9.Checked;
            }

            if (Any_Band_is_Selected)
            {
                //Read Vreg1s to Textbox (For Initial Value)
                //Update string[] D1_Vreg1_Params = new string[15];
                //if (checkBox_Vreg1_Compensation.Checked) button_Vreg1_Read.PerformClick();
                button_Vreg1_Read.PerformClick();     
                for (band = 0; band < 10; band++)
                {
                    f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                    if (Optic_Compensation_Stop) break;
                    Gamma_Out_Of_Register_Limit = false;
                    if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                    {
                        //if (checkBox_Vreg1_Compensation.Checked)
                        {
                            Vreg1_loop_count = 0; //Vreg1 loop countR
                            Vreg1_Infinite_Count = 0;

                            if (checkBox_Apply_FX_Previous_Band_Vreg1.Checked && band >= 1)
                            {
                                if (Selected_Band[band])
                                {

                                    double band_Target_Lv = Convert.ToDouble(Meta_form_engineer.dataGridView_OC_param.Rows[(band * 8) + (0 + 2)].Cells[9].Value);
                                    int[] Previous_Band_Gamma_Red = new int[8];
                                    int[] Previous_Band_Gamma_Green = new int[8];
                                    int[] Previous_Band_Gamma_Blue = new int[8];
                                    double[] Previous_Band_Target_Lv = new double[8];
                                    for (int i = 0; i < 8; i++)
                                    {
                                        Previous_Band_Gamma_Red[i] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[((band - 1) * 8) + (i + 2)].Cells[1].Value);
                                        Previous_Band_Gamma_Green[i] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[((band - 1) * 8) + (i + 2)].Cells[2].Value);
                                        Previous_Band_Gamma_Blue[i] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[((band - 1) * 8) + (i + 2)].Cells[3].Value);
                                        if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(Meta_form_engineer.dataGridView_OC_param.Rows[((band - 1) * 8) + (i + 2)].Cells[6].Value);
                                        else if(this.radioButton_Previous_Target_Lv.Checked)Previous_Band_Target_Lv[i] = Convert.ToDouble(Meta_form_engineer.dataGridView_OC_param.Rows[((band - 1) * 8) + (i + 2)].Cells[9].Value);
                                    }

                                    int Previous_Band_Vreg1_Dec = Meta_Get_Normal_Initial_Vreg1(band - 1);
                                    Vreg1 = Meta_Get_Normal_Initial_Vreg1(band);
                                    
                                    Dll_Meta_Get_Normal_Initial_Vreg1_R_B_Fx_Previous_Band(ref Vreg1, ref Vreg1_First_Gamma_Red, ref Vreg1_First_Gamma_Blue, Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, band, band_Target_Lv, Previous_Band_Vreg1_Dec, Previous_Band_Target_Lv, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                                    f1.GB_Status_AppendText_Nextline("(1)After(Dll C++,,Precision : 0.001) Vreg1_First_Gamma_Red/Vreg1/Vreg1_First_Gamma_Blue : " + Vreg1_First_Gamma_Red.ToString() + "/" + Vreg1.ToString() + "/" + Vreg1_First_Gamma_Blue.ToString(), Color.Red);
                                    
                                    //Copy "Previous Band Gamma to Current Band Gamma" and Set "All_band_gray_Gamma"
                                    Meta_form_engineer.Copy_Previous_Band_Gamma(band);
                                    System.Windows.Forms.Application.DoEvents();
                                    Meta_form_engineer.Meta_Get_Band_Gray_Gamma(All_band_gray_Gamma, band);

                                    //Set Calculated Vreg1_dec
                                    Meta_Update_and_Send_Vreg1(Vreg1, band);
                                    Thread.Sleep(20);
                                    Meta_Update_Vreg1_TextBox(Vreg1, band);
                                    //f1.GB_Status_AppendText_Nextline("C++ Dll Test Vreg1 Setting (Previous Band F(x)) : " + Vreg1.ToString(), Color.Red);
                                }
                                else
                                {
                                    f1.GB_Status_AppendText_Nextline("C++ Dll Test Vreg1 Setting (Previous Band F(x)) : " + Vreg1.ToString(), Color.Blue);
                                }
                            }
                            else if (checkBox_Apply_FLviX_HBM.Checked)
                            {
                                
                            }
                            else
                            {
                                Vreg1 = Meta_Get_Normal_Initial_Vreg1(band);
                            }

                            //Vreg Update
                            Initial_Vreg1 = Vreg1;
                            Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                            Meta_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(band, Initial_Vreg1);
                            Meta_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);

                            //Add on 191028
                            Vreg1_Voltage = Meta_form_engineer.Get_Vreg1_Voltage(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                            AM1_RGB_Voltage = F7 + (Vreg1_Voltage - F7) * (8.0 / 700.0);
                            f1.GB_Status_AppendText_Nextline("Vreg1_Dec / Vreg1_Voltage / AM1_Voltage = " + Vreg1.ToString() + " / " + Vreg1_Voltage.ToString() + " / " + AM1_RGB_Voltage.ToString(), Color.Black);
                        }

                        Meta_form_engineer.Band_Radiobuttion_Select(band);//Select Band
                        Meta_DBV_Setting(band);  //DBV Setting

                        for (gray = 0; gray < 8; gray++)
                        {
                            if (Optic_Compensation_Stop) break;
                            
                            Get_Param(gray, ref Gamma, ref Target, ref Limit, ref Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table  
                            if (Target.double_Lv > Convert.ToDouble(textBox_Fast_OC_RGB_Skip_Target.Text))
                            {
                                if (checkBox_Apply_FX_HBM_RGB.Checked && band >= 1 && gray >= 1)
                                {
                                    if (Selected_Band[band])
                                    {
                                        int[] HBM_Gamma_R = new int[8];
                                        int[] HBM_Gamma_G = new int[8];
                                        int[] HBM_Gamma_B = new int[8];
                                        double[] HBM_Target_Lv = new double[8];
                                        for (int i = 0; i < 8; i++)
                                        {
                                            HBM_Gamma_R[i] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[i + 2].Cells[1].Value);
                                            HBM_Gamma_G[i] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[i + 2].Cells[2].Value);
                                            HBM_Gamma_B[i] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[i + 2].Cells[3].Value);

                                            if (radioButton_Previous_Measure_Lv.Checked) HBM_Target_Lv[i] = Convert.ToDouble(Meta_form_engineer.dataGridView_OC_param.Rows[i + 2].Cells[6].Value);
                                            else if (this.radioButton_Previous_Target_Lv.Checked) HBM_Target_Lv[i] = Convert.ToDouble(Meta_form_engineer.dataGridView_OC_param.Rows[i + 2].Cells[9].Value);
                                        }

                                        double Target_Lv = Convert.ToDouble(Meta_form_engineer.dataGridView_OC_param.Rows[(band * 8) + (gray + 2)].Cells[9].Value);

                                        int Previous_Band_G255_Green_Gamma = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[((band - 1) * 8) + (0 + 2)].Cells[2].Value);
                                        int Previous_Band_Vreg1_Dec = Meta_Get_Normal_Initial_Vreg1(band - 1);

                                        int Current_Band_Dec_Vreg1 = Meta_Get_Normal_Initial_Vreg1(band);
                                        double Prvious_Gray_Gamma_R_Voltage = Convert.ToDouble(Meta_form_engineer.dataGridView_RGB_Vdata.Rows[(band * 8) + ((gray - 1) + 2)].Cells[1].Value);
                                        double Prvious_Gray_Gamma_G_Voltage = Convert.ToDouble(Meta_form_engineer.dataGridView_RGB_Vdata.Rows[(band * 8) + ((gray - 1) + 2)].Cells[2].Value);
                                        double Prvious_Gray_Gamma_B_Voltage = Convert.ToDouble(Meta_form_engineer.dataGridView_RGB_Vdata.Rows[(band * 8) + ((gray - 1) + 2)].Cells[3].Value);

                                        
                                        ///////////////////////////////////////////////////////////////////////////////////////////////
                                        SJH_Matrix M = new SJH_Matrix();
                                        int[][] Band_Gray_Gamma_Red = M.Int_MatrixCreate(band, 8);
                                        int[][] Band_Gray_Gamma_Green = M.Int_MatrixCreate(band, 8);
                                        int[][] Band_Gray_Gamma_Blue = M.Int_MatrixCreate(band, 8);
                                        double[][] Band_Gray_Target_Lv= M.MatrixCreate(band,8);
                                        int[] Band_Vreg1_Dec = new int[band];
                                        for (int b = 0; b < band; b++)
                                        {
                                            for (int g = 0; g < 8; g++)
                                            {
                                                Band_Gray_Gamma_Red[b][g] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[1].Value);
                                                Band_Gray_Gamma_Green[b][g] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[2].Value);
                                                Band_Gray_Gamma_Blue[b][g] = Convert.ToInt16(Meta_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[3].Value);

                                                if (radioButton_Previous_Measure_Lv.Checked) Band_Gray_Target_Lv[b][g] = Convert.ToDouble(Meta_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[6].Value);
                                                else if (this.radioButton_Previous_Target_Lv.Checked) Band_Gray_Target_Lv[b][g] = Convert.ToDouble(Meta_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[9].Value);  
                                            }
                                            Band_Vreg1_Dec[b] = Meta_Get_Normal_Initial_Vreg1(b);
                                        }

                                        
                                        Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_All_Band_3points(ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Selected_Band, Band_Gray_Gamma_Red, Band_Gray_Gamma_Green, Band_Gray_Gamma_Blue, Band_Gray_Target_Lv, Current_Band_Dec_Vreg1,
            Band_Vreg1_Dec, band,gray,Target_Lv,Prvious_Gray_Gamma_R_Voltage, Prvious_Gray_Gamma_G_Voltage, Prvious_Gray_Gamma_B_Voltage,
            E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                                        f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C# (All Band , 3points) Gamma_R/G/B : " + Gamma.int_R.ToString() + "/" + Gamma.int_G.ToString() + "/" + Gamma.int_B.ToString(), Color.Blue);

                                        double Fx_3points_Combine_LV_1 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_1.Text);
                                        double Fx_3points_Combine_LV_2 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_2.Text);
                                        double Fx_3points_Combine_LV_3 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_3.Text);
                                        double Fx_3points_Combine_Lv_Distance = Convert.ToDouble(textBox_Fx_3points_Lv_Distance_Combine.Text);

                                        Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_All_Band_3points_Combine_Points(ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Selected_Band, Band_Gray_Gamma_Red, Band_Gray_Gamma_Green, Band_Gray_Gamma_Blue, Band_Gray_Target_Lv, Current_Band_Dec_Vreg1,
            Band_Vreg1_Dec, band, gray, Target_Lv, Prvious_Gray_Gamma_R_Voltage, Prvious_Gray_Gamma_G_Voltage, Prvious_Gray_Gamma_B_Voltage,
            E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7
            , Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance);
                                        f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C# (All Band , 3points, Combine) Gamma_R/G/B : " + Gamma.int_R.ToString() + "/" + Gamma.int_G.ToString() + "/" + Gamma.int_B.ToString(), Color.Red);
                                        
                                        /*
                                        Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_All_Band_3points_Fx_Input_Output_Reverse(ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Selected_Band, Band_Gray_Gamma_Red, Band_Gray_Gamma_Green, Band_Gray_Gamma_Blue, Band_Gray_Target_Lv, Current_Band_Dec_Vreg1,
            Band_Vreg1_Dec, band, gray, Target_Lv, Prvious_Gray_Gamma_R_Voltage, Prvious_Gray_Gamma_G_Voltage, Prvious_Gray_Gamma_B_Voltage,
            E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                                        f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C# (All Band , 3points , Input_Output_Reverse) Gamma_R/G/B : " + Gamma.int_R.ToString() + "/" + Gamma.int_G.ToString() + "/" + Gamma.int_B.ToString(), Color.Blue);
                                        */
                                        ///////////////////////////////////////////////////////////////////////////////////////////////
                                        //Dll_Meta_Get_First_Gamma_Fx_HBM(ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Selected_Band, HBM_Gamma_R, HBM_Gamma_G, HBM_Gamma_B, HBM_Target_Lv, Current_Band_Dec_Vreg1, band, gray, Target_Lv, Prvious_Gray_Gamma_R_Voltage, Prvious_Gray_Gamma_G_Voltage, Prvious_Gray_Gamma_B_Voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7); 
                                        //f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ (F(X)_HBM) Gamma_R/G/B : " + Gamma.int_R.ToString() + "/" + Gamma.int_G.ToString() + "/" + Gamma.int_B.ToString(), Color.Red);
                                        ///////////////////////////////////////////////////////////////////////////////////////////////
                                    }
                                }
                                else if (checkBox_Apply_FLviX_HBM.Checked)
                                {

                                }
                            }
                       
                            Gamma_Init.Equal_Value(Gamma);

                            if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                            Meta_Pattern_Setting(gray);//Pattern Setting
                            if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                || band == 6 || band == 7 || band == 8 || band == 9) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                            {
                                Meta_form_engineer.Get_Gamma_Only_Meta(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                Gamma.Equal_Value(Prev_Band_Gray255_Gamma);

                                if (checkBox_Apply_FX_Previous_Band_Vreg1.Checked && Selected_Band[band] == true)
                                {
                                    f1.GB_Status_AppendText_Nextline("(1)Before R/Vreg1/B : " + Gamma.int_R.ToString() + "/" + Vreg1.ToString() + "/" + Gamma.int_B.ToString(), Color.Blue);
                                    Gamma.int_R = Vreg1_First_Gamma_Red;//Add on 191118
                                    Gamma.int_B = Vreg1_First_Gamma_Blue;//Add on 191118
                                    f1.GB_Status_AppendText_Nextline("(1)After R/Vreg1/B : " + Gamma.int_R.ToString() + "/" + Vreg1.ToString() + "/" + Gamma.int_B.ToString(), Color.Red);
                                }
                            }


                            Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray);

                            Thread.Sleep(300); //Pattern 안정화 Time
                            Meta_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                            loop_count = 0;
                            Infinite_Count = 0;
                            Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, "X");
                            Optic_Compensation_Succeed = false;
                            Within_Spec_Limit = false;

                            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                            {
                                if (Target.double_Lv < Skip_Lv)
                                {
                                    if (band >= 1)
                                    {
                                        Meta_form_engineer.Get_Gamma_Only_Meta(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                        Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                    }
                                    Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray); //Setting Gamma Values
                                    Measure.Set_Value(0, 0, 0);
                                    Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString() + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                    Optic_Compensation_Succeed = true;
                                    break;
                                }

                                //Vreg1 + Sub-Compensation (Change Gamma Value)
                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8 || band == 9) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                {
                                    Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                    if (Vreg1_loop_count < loop_count_max)
                                    {
                                        //f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                        Prev_Vreg1 = Vreg1;
                                        Prev_Gamma.Equal_Value(Gamma);

                                        Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Normal_Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                        Diff_Vreg1 = Vreg1 - Prev_Vreg1;

                                        Diff_Gamma = Gamma - Prev_Gamma;
                                        f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);

                                        if (Math.Abs(Diff_Vreg1) >= 1) Vreg1_Need_To_Be_Updated = true;
                                        else Vreg1_Need_To_Be_Updated = false;

                                        if (Vreg1_Need_To_Be_Updated)
                                        {
                                            //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                            Meta_Update_and_Send_Vreg1(Vreg1, band);
                                            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)(20 > 16.7ms (=60hz))
                                            Meta_Update_Vreg1_TextBox(Vreg1, band);
                                            Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                                            Meta_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
                                            //Add on 191028
                                            //Vreg1_Voltage = Meta_form_engineer.Get_Vreg1_Voltage(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                                            Vreg1_Voltage = Meta_Get_Vreg1_Voltage(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                                           
                                            AM1_RGB_Voltage = F7 + (Vreg1_Voltage - F7) * (8.0 / 700.0);
                                            f1.GB_Status_AppendText_Nextline("Vreg1_Dec / Vreg1_Voltage / AM1_Voltage = " + Vreg1.ToString() + " / " + Vreg1_Voltage.ToString() + " / " + AM1_RGB_Voltage.ToString(), Color.Black);
                                        }
                                        
                                    }

                                    Vreg1_loop_count++;
                                    loop_count++;
                                    if (Vreg1_Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else Extension_Applied = "X";

                                }
                                else
                                {
                                    Vreg1_Need_To_Be_Updated = false;

                                    Prev_Gamma.Equal_Value(Gamma);
                                    Infinite_Loop_Check(loop_count);

                                    Sub_Compensation(loop_count, Infinite, Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Normal_Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                    //Engineering Mode
                                    Diff_Gamma = Gamma - Prev_Gamma;
                                    f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                                    loop_count++;

                                    if (Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else Extension_Applied = "X";

                                }

                                if (Vreg1_Need_To_Be_Updated == false)
                                {
                                    //f1.GB_Status_AppendText_Nextline("Gamma Setting", Color.Blue);
                                    Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray); //Setting Gamma Values
                                    Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)(20 > 16.7ms (=60hz))

                                    int DIff_R = Gamma.int_R - Gamma_Init.int_R;
                                    int DIff_G = Gamma.int_G - Gamma_Init.int_G;
                                    int DIff_B = Gamma.int_B - Gamma_Init.int_B;
                                    Meta_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(band, gray, DIff_R, DIff_G, DIff_B);

                                    //Add on 191028
                                    if (gray == 0)//G255 (AM2)
                                    {
                                        //Gamma_Voltage[gray].double_R = Meta_form_engineer.Get_AM2_Voltage(F7, Vreg1_Voltage, Gamma.int_R);
                                        //Gamma_Voltage[gray].double_G = Meta_form_engineer.Get_AM2_Voltage(F7, Vreg1_Voltage, Gamma.int_G);
                                        //Gamma_Voltage[gray].double_B = Meta_form_engineer.Get_AM2_Voltage(F7, Vreg1_Voltage, Gamma.int_B);
                                        //f1.GB_Status_AppendText_Nextline("C# G255 Ver Get Gamma Voltage R/G/B : " + Gamma_Voltage[gray].double_R.ToString() + "/" + Gamma_Voltage[gray].double_G.ToString() + "/" + Gamma_Voltage[gray].double_B.ToString(), Color.Red);
                                  

                                        Gamma_Voltage[gray].double_R = Meta_Get_AM2_Voltage(F7, Vreg1_Voltage, Gamma.int_R);
                                        Gamma_Voltage[gray].double_G = Meta_Get_AM2_Voltage(F7, Vreg1_Voltage, Gamma.int_G);
                                        Gamma_Voltage[gray].double_B = Meta_Get_AM2_Voltage(F7, Vreg1_Voltage, Gamma.int_B);
                                        //f1.GB_Status_AppendText_Nextline("C++ Dll G255 Ver Get Gamma Voltage R/G/B : " + Gamma_Voltage[gray].double_R.ToString() + "/" + Gamma_Voltage[gray].double_G.ToString() + "/" + Gamma_Voltage[gray].double_B.ToString(), Color.Blue);
                                      
                                    }
                                    else
                                    {
                                       //Gamma_Voltage[gray].double_R = Meta_form_engineer.Get_Normal_Gamma_Voltage(AM1_RGB_Voltage, Gamma_Voltage[gray - 1].double_R, Gamma.int_R);
                                        //Gamma_Voltage[gray].double_G = Meta_form_engineer.Get_Normal_Gamma_Voltage(AM1_RGB_Voltage, Gamma_Voltage[gray - 1].double_G, Gamma.int_G);
                                        //Gamma_Voltage[gray].double_B = Meta_form_engineer.Get_Normal_Gamma_Voltage(AM1_RGB_Voltage, Gamma_Voltage[gray - 1].double_B, Gamma.int_B);
                                       //f1.GB_Status_AppendText_Nextline("C# Ver Get Gamma Voltage R/G/B : " + Gamma_Voltage[gray].double_R.ToString() + "/" + Gamma_Voltage[gray].double_G.ToString() + "/" + Gamma_Voltage[gray].double_B.ToString(), Color.Blue);

                                        Gamma_Voltage[gray].double_R = Meta_Get_Normal_Gamma_Voltage(AM1_RGB_Voltage, Gamma_Voltage[gray - 1].double_R, Gamma.int_R);
                                        Gamma_Voltage[gray].double_G = Meta_Get_Normal_Gamma_Voltage(AM1_RGB_Voltage, Gamma_Voltage[gray - 1].double_G, Gamma.int_G);
                                        Gamma_Voltage[gray].double_B = Meta_Get_Normal_Gamma_Voltage(AM1_RGB_Voltage, Gamma_Voltage[gray - 1].double_B, Gamma.int_B);
                                        //f1.GB_Status_AppendText_Nextline("C++ Dll G255 Ver Get Gamma Voltage R/G/B : " + Gamma_Voltage[gray].double_R.ToString() + "/" + Gamma_Voltage[gray].double_G.ToString() + "/" + Gamma_Voltage[gray].double_B.ToString(), Color.Blue);
                                      
                                    }
                                    Meta_form_engineer.dataGridView_RGB_Vdata.Rows[band * 8 + (gray + 2)].Cells[1].Value = Gamma_Voltage[gray].double_R;
                                    Meta_form_engineer.dataGridView_RGB_Vdata.Rows[band * 8 + (gray + 2)].Cells[2].Value = Gamma_Voltage[gray].double_G;
                                    Meta_form_engineer.dataGridView_RGB_Vdata.Rows[band * 8 + (gray + 2)].Cells[3].Value = Gamma_Voltage[gray].double_B;
                                }

                                if (Within_Spec_Limit)
                                {
                                    Optic_Compensation_Succeed = true;
                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    break;
                                }

                                if (Gamma_Out_Of_Register_Limit)
                                {
                                    Optic_Compensation_Succeed = false;
                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    Optic_Compensation_Stop = true;
                                    System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");
                                    break;
                                }

                                textBox_loop_count.Text = (loop_count).ToString();
                                if (loop_count == loop_count_max)
                                {
                                    Optic_Compensation_Succeed = false;
                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    System.Windows.Forms.MessageBox.Show("B" + band.ToString() + "/G" + gray.ToString() + " Loop Count Over");
                                    if (this.checkBox_Continue_After_Fail.Checked == false)
                                        Optic_Compensation_Stop = true;
                                    break;
                                }
                                Meta_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Measured Lv : " + Measure.double_Lv.ToString(), Color.Black);
                                Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, Extension_Applied); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();
                            }
                            f1.GB_ProgressBar_PerformStep();
                            if (checkBox_Only_255G.Checked)
                                gray = 8;
                        }
                    }
                }
                f1.OC_Timer_Stop();//GB Timer Stop
                Meta_form_engineer.Engineering_Mode_DataGridview_ReadOnly(false);
                //if Optic Compensation is finished , then....
                if (Optic_Compensation_Stop == false && Gamma_Out_Of_Register_Limit == false)
                {
                    if (checkBox_1st_Mode_OTP_AutoWirte.Checked)
                    {
                        button_Meta_OTP_Write.PerformClick();
                        f1.ADD_GB_ProgressBar_Value(5 * step);
                    }
                }
                else
                {
                    //if 
                }
            }
            f1.OC_Timer_Stop();
            //---------------------------------------------
            if (Optic_Compensation_Succeed) System.Windows.Forms.MessageBox.Show("Optic Compensation Finish !");
        }


        private void Meta_Measure_Average(ref double Measured_X, ref double Measured_Y, ref double Measured_Lv, ref int total_average_loop_count
           , double Target_X, double Target_Y, double Target_Lv, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];


            XYLv[] Measure = new XYLv[5];
            int Max_Index = 0; double Max_Value = 0;
            int Min_Index = 0; double Min_Value = 2000;

            double Ratio;
            double Diff_X;
            double Diff_Y;
            double Diff_Lv;
            double Tolerance_X;
            double Tolerance_Y;
            double Tolerance_Lv;
            if (checkBox_Ave_Apply_Ratio.Checked)
            {
                Ratio = Convert.ToDouble(textBox_Ave_Ratio.Text);
            }
            else
            {
                Ratio = 1000000;
            }

            try
            {
                f1.isMsr = true;
                f1.CA_Measure_button.Enabled = false;
                f1.objCa.Measure();
                Diff_X = Math.Abs(Target_X - f1.objCa.OutputProbes.get_ItemOfNumber(1).sx);
                Diff_Y = Math.Abs(Target_Y - f1.objCa.OutputProbes.get_ItemOfNumber(1).sy);
                Diff_Lv = Math.Abs(Target_Lv - f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv);

                Tolerance_X = Limit_X + Extension_X;
                Tolerance_Y = Limit_Y + Extension_Y;
                Tolerance_Lv = Limit_Lv;

                if (checkBox_Ave_Measure.Checked && f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(textBox_Ave_Lv_Limit.Text)
                    && ((Tolerance_X * Ratio) > Diff_X) && ((Tolerance_Y * Ratio) > Diff_Y) && ((Tolerance_Lv * Ratio) > Diff_Lv))
                {
                    f1.GB_Status_AppendText_Nextline(f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + textBox_Ave_Lv_Limit.Text, Color.Blue);
                    f1.GB_Status_AppendText_Nextline("Tolerance_X * Ratio : " + (Tolerance_X * Ratio).ToString() + " / Diff_X : " + Diff_X.ToString(), Color.Blue);
                    f1.GB_Status_AppendText_Nextline("Tolerance_Y * Ratio : " + (Tolerance_Y * Ratio).ToString() + " / Diff_X : " + Diff_Y.ToString(), Color.Blue);
                    f1.GB_Status_AppendText_Nextline("Tolerance_Lv * Ratio : " + (Tolerance_Lv * Ratio).ToString() + " / Diff_X : " + Diff_Lv.ToString(), Color.Blue);

                    for (int a = 0; a < 5; a++)
                    {
                        f1.objCa.Measure();
                        Measure[a].X = f1.objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Measure[a].Y = f1.objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Measure[a].Lv = f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

                        Measure[a].Double_Update_From_String();

                        if (Measure[a].double_Lv < Min_Value)
                        {
                            Min_Value = Measure[a].double_Lv;
                            Min_Index = a;
                        }

                        if (Measure[a].double_Lv > Max_Value)
                        {
                            Max_Value = Measure[a].double_Lv;
                            Max_Index = a;
                        }
                    }


                    int count = 0;
                    XYLv Sum_Measure = new XYLv();
                    XYLv Ave_Measure = new XYLv();
                    Sum_Measure.Set_Value(0, 0, 0);
                    Ave_Measure.Set_Value(0, 0, 0);

                    for (int a = 0; a < 5; a++)
                    {
                        if (a == Max_Index || a == Min_Index)
                        {
                        }
                        else
                        {
                            Sum_Measure.double_X += Measure[a].double_X;
                            Sum_Measure.double_Y += Measure[a].double_Y;
                            Sum_Measure.double_Lv += Measure[a].double_Lv;
                            count++;
                        }
                    }
                    Ave_Measure.double_X = Math.Round((Sum_Measure.double_X / count), 4);
                    Ave_Measure.double_Y = Math.Round((Sum_Measure.double_Y / count), 4);
                    Ave_Measure.double_Lv = Math.Round((Sum_Measure.double_Lv / count), 4);
                    Ave_Measure.String_Update_From_Double();
                    f1.GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                    f1.GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                    f1.GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                    f1.GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                    f1.GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                    f1.X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                    f1.Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                    f1.Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                    textBox_Total_Average_Meas_Count.Text = (++total_average_loop_count).ToString();
                }

                else
                {
                    f1.X_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    f1.Y_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    f1.Lv_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                }

                Measured_X = Convert.ToDouble(f1.X_Value_display.Text);
                Measured_Y = Convert.ToDouble(f1.Y_Value_display.Text);
                Measured_Lv = Convert.ToDouble(f1.Lv_Value_display.Text);

                //Data Grid setting//////////////////////
                f1.dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                f1.dataGridView2.Rows.Add("-", f1.X_Value_display.Text, f1.Y_Value_display.Text, f1.Lv_Value_display.Text);
                f1.dataGridView2.FirstDisplayedScrollingRowIndex = f1.dataGridView2.RowCount - 1;

                System.Windows.Forms.Application.DoEvents();
                f1.CA_Measure_button.Enabled = true;
            }
            catch (Exception er)
            {
                f1.DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }


        private void SET_ELVSS(int Band, double ELVSS, string[] Hex_ELVSS)
        {
            int Current_Dec = Convert.ToInt16(30 +  10 * (ELVSS + 3.5));
            Hex_ELVSS[Band] = Current_Dec.ToString("X2");
        }

        private void Normal_Vinit_Setting(double Vinit)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];         
            int Current_Dec = Convert.ToInt16(33 - 10 * (Vinit + 3.3));
            f1.OTP_Read(16, "C2");
            string[] temp = new string[16];
            for (int i = 0; i < 15; i++) temp[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            temp[15] = ((Convert.ToInt16(temp[15], 16) & 0xC0) + Current_Dec).ToString("X2");
            f1.Long_Packet_CMD_Send(16, "C2", temp);
            f1.GB_Status_AppendText_Nextline("Final Vinit : " + Vinit.ToString(), Color.Green);
        }


        private void ELVSS_Compensation()
        {
            Meta_DBV_Setting(0);//HBM DBV Setting
            Meta_Pattern_Setting(0);//HBM Gray255 Pattern Setting

            if (Optic_Compensation_Stop)
            {
            }
            else
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];

                bool ELVSS_Find_Finish = false;
                double First_ELVSS = -4.5;//Input
                double ELVSS = 0; //Input & Output (ref)
                double Vinit = 0; //Output (ref)
                double First_Slope = 0; //Input & Output (ref)

                double Vinit_Margin = Convert.ToDouble(textBox_Vinit_Margin.Text);
                double ELVSS_Margin = Convert.ToDouble(textBox_ELVSS_Margin.Text);
                double Slope_Margin = Convert.ToDouble(textBox_Slope_Margin.Text);
                XYLv First_Measure = new XYLv();
                XYLv Measure = new XYLv();

                string[] Hex_ELVSS = new string[10];
                for (int i = 0; i < 10; i++) Hex_ELVSS[i] = "00";

                string mipi_cmd = "mipi.write 0x39 0xCD 0x00 0x00 0x00";
                for (ELVSS = First_ELVSS; ELVSS < -1.4; ELVSS += 0.1)
                {
                    if (ELVSS == First_ELVSS)
                    {
                        SET_ELVSS(0, First_ELVSS, Hex_ELVSS);//Hex_ELVSS[band = 0] <-- Fisrst_ELVSS
                        f1.IPC_Quick_Send(mipi_cmd + " 0x" + Hex_ELVSS[0]); //send Hex_ELVSS[0]
                        Thread.Sleep(50);
                        f1.CA_Measure_button_Perform_Click(ref First_Measure.double_X, ref First_Measure.double_Y, ref First_Measure.double_Lv);
                    }
                    else
                    {
                        //TI_Normal_ELVSS_Setting(0, "BD", ELVSS);//HBM
                        SET_ELVSS(0, ELVSS, Hex_ELVSS);//Hex_ELVSS[band = 0] <-- ELVSS
                        f1.IPC_Quick_Send(mipi_cmd + " 0x" + Hex_ELVSS[0]); //send Hex_ELVSS[0]
                        Thread.Sleep(50);
                        f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);
                        f1.GB_Status_AppendText_Nextline("ELVSS/x/y/Lv : " + ELVSS.ToString() + "/" + Measure.double_X.ToString() + "/" + Measure.double_Y.ToString()
                            + "/" + Measure.double_Lv.ToString(),Color.Blue);

                        ELVSS_Compensation(ref ELVSS_Find_Finish, First_ELVSS, ref ELVSS, ref Vinit, ref First_Slope, Vinit_Margin, ELVSS_Margin, Slope_Margin, First_Measure.double_X, First_Measure.double_Y
                            , First_Measure.double_Lv, Measure.double_X, Measure.double_Y, Measure.double_Lv);

                        if (ELVSS_Find_Finish)
                        {
                            f1.GB_Status_AppendText_Nextline("ELVSS Find Finish", Color.Black);
                            break;
                        }
                    }
                }

                //Vinit 적용
                Normal_Vinit_Setting(Vinit);

                //ELVSS 적용
                f1.GB_Status_AppendText_Nextline("HBM ELVSS : " + ELVSS.ToString(), Color.Black);

                SET_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset.Text), Hex_ELVSS);
                SET_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset.Text), Hex_ELVSS);
                SET_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset.Text), Hex_ELVSS);
                SET_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset.Text), Hex_ELVSS);
                SET_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset.Text), Hex_ELVSS);
                SET_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset.Text), Hex_ELVSS);
                SET_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset.Text), Hex_ELVSS);
                SET_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset.Text), Hex_ELVSS);
                SET_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset.Text), Hex_ELVSS);
                SET_ELVSS(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset.Text), Hex_ELVSS);

                //Setting ELVSS
                for (int i = 0; i < 10; i++) mipi_cmd += (" 0x" + Hex_ELVSS[i]);
                f1.IPC_Quick_Send(mipi_cmd); //send Hex_ELVSS[0]
            }
            button_Read_ELVSS_Vinit.PerformClick();
           
        }


        private void Infinite_Loop_Check(int loop_count)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (loop_count == 0) Temp_Gamma[0].Equal_Value(Gamma);
            else if (loop_count == 1) Temp_Gamma[1].Equal_Value(Gamma);
            else if (loop_count == 2) Temp_Gamma[2].Equal_Value(Gamma);
            else if (loop_count == 3) Temp_Gamma[3].Equal_Value(Gamma);
            else
            {
                Temp.Equal_Value(Temp_Gamma[1]);
                Temp_Gamma[0].Equal_Value(Temp);
                Temp.Equal_Value(Temp_Gamma[2]);
                Temp_Gamma[1].Equal_Value(Temp);
                Temp.Equal_Value(Temp_Gamma[3]);
                Temp_Gamma[2].Equal_Value(Temp);
                Temp_Gamma[3].Equal_Value(Gamma);

                Diif_Gamma[0] = Temp_Gamma[1] - Temp_Gamma[0];
                Diif_Gamma[1] = Temp_Gamma[2] - Temp_Gamma[1];
                Diif_Gamma[2] = Temp_Gamma[3] - Temp_Gamma[2];

                //Ver5
                if ((Diif_Gamma[0].R == Diif_Gamma[2].R && Diif_Gamma[0].G == Diif_Gamma[2].G && Diif_Gamma[0].B == Diif_Gamma[2].B) && (Diif_Gamma[0].R != Diif_Gamma[1].R || Diif_Gamma[0].G != Diif_Gamma[1].G || Diif_Gamma[0].B != Diif_Gamma[1].B))
                {
                    Infinite = true;
                    Infinite_Count++;
                }

                else Infinite = false;

                if (Infinite) f1.GB_Status_AppendText_Nextline("Infinite : " + Infinite.ToString(), Color.Red);
                else f1.GB_Status_AppendText_Nextline("Infinite : " + Infinite.ToString(), Color.Green);

                if (Infinite_Count >= 3)
                    f1.GB_Status_AppendText_Nextline("Infinite_Count = " + Infinite_Count.ToString(), Color.Red);
                else
                    f1.GB_Status_AppendText_Nextline("Infinite_Count = " + Infinite_Count.ToString(), Color.Green);
            }
        }

        private void Meta_Update_Vreg1_TextBox(int Vreg1_int, int band)
        {
            if (band == 0)
                textBox_Vreg1_B0.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 1)
                textBox_Vreg1_B1.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 2)
                textBox_Vreg1_B2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 3)
                textBox_Vreg1_B3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 4)
                textBox_Vreg1_B4.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 5)
                textBox_Vreg1_B5.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 6)
                textBox_Vreg1_B6.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 7)
                textBox_Vreg1_B7.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 8)
                textBox_Vreg1_B8.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else if (band == 9)
                textBox_Vreg1_B9.Text = Vreg1_int.ToString(); //Read Vreg1 Value
            else
            {
                System.Windows.Forms.MessageBox.Show("Band out of Boundary");
            }   
            Application.DoEvents();
        }




        private void Meta_Update_and_Send_Vreg1(int Vreg1_int, int band)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Vreg1_High = Vreg1_int >> 8;//Vreg1_BD[9:8]
            int Vreg1_Low = Vreg1_int & 0xFF;//Vreg1_BD[7:0]
            int D1_Vreg1_Params_Int = 0;
            
            if (band == 1 || band == 2 || band == 3 || band == 4)D1_Vreg1_Params_Int = Convert.ToInt16(D1_Vreg1_Params[0], 16);
            else if (band == 5 || band == 6 || band == 7 || band == 8) D1_Vreg1_Params_Int = Convert.ToInt16(D1_Vreg1_Params[5], 16);
            else if (band == 9) D1_Vreg1_Params_Int = Convert.ToInt16(D1_Vreg1_Params[10], 16);

            if (band == 1)
            {
                D1_Vreg1_Params[0] = ((D1_Vreg1_Params_Int & 0xFC) + Vreg1_High).ToString("X2");
                D1_Vreg1_Params[1] = Vreg1_Low.ToString("X2");
            }
            else if (band == 2)
            {
                D1_Vreg1_Params[0] = ((D1_Vreg1_Params_Int & 0xF3) + (Vreg1_High << 2)).ToString("X2");
                D1_Vreg1_Params[2] = Vreg1_Low.ToString("X2");
            }
            else if (band == 3)
            {
                D1_Vreg1_Params[0] = ((D1_Vreg1_Params_Int & 0xCF) + (Vreg1_High << 4)).ToString("X2");
                D1_Vreg1_Params[3] = Vreg1_Low.ToString("X2");
            }
            else if (band == 4)
            {
                D1_Vreg1_Params[0] = ((D1_Vreg1_Params_Int & 0x3F) + (Vreg1_High << 6)).ToString("X2");
                D1_Vreg1_Params[4] = Vreg1_Low.ToString("X2");
            }


            else if (band == 5)
            {
                D1_Vreg1_Params[5] = ((D1_Vreg1_Params_Int & 0xFC) + Vreg1_High).ToString("X2");
                D1_Vreg1_Params[6] = Vreg1_Low.ToString("X2");
            }
            else if (band == 6)
            {
                D1_Vreg1_Params[5] = ((D1_Vreg1_Params_Int & 0xF3) + (Vreg1_High << 2)).ToString("X2");
                D1_Vreg1_Params[7] = Vreg1_Low.ToString("X2");
            }
            else if (band == 7)
            {
                D1_Vreg1_Params[5] = ((D1_Vreg1_Params_Int & 0xCF) + (Vreg1_High << 4)).ToString("X2");
                D1_Vreg1_Params[8] = Vreg1_Low.ToString("X2");
            }
            else if (band == 8)
            {
                D1_Vreg1_Params[5] = ((D1_Vreg1_Params_Int & 0x3F) + (Vreg1_High << 6)).ToString("X2");
                D1_Vreg1_Params[9] = Vreg1_Low.ToString("X2");
            }

            else if (band == 9)
            {
                D1_Vreg1_Params[10] = ((D1_Vreg1_Params_Int & 0xFC) + (Vreg1_High)).ToString("X2");
                D1_Vreg1_Params[11] = Vreg1_Low.ToString("X2");
            }

            else
            {
                System.Windows.Forms.MessageBox.Show("Band Out Of Boundary");
            }

            f1.Long_Packet_CMD_Send(15,"D1",D1_Vreg1_Params);
        }

        private void Update_Engineering_Sheet(RGB Gamma, XYLv Measure, int band, int gray, int loop_count, string Extension_Applied)
        {
            Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_form_engineer.Set_OC_Param_Meta(gray, Gamma.int_R, Gamma.int_G, Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, loop_count, Extension_Applied);
            Meta_form_engineer.Updata_Sub_To_Main_GridView(band, gray);
        }

        private void Vreg1_Infinite_Loop_Check(int Vreg1_loop_count)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (Vreg1_loop_count == 0) Vreg1_Temp_Gamma[0].Equal_Value(Gamma);
            else if (Vreg1_loop_count == 1)
            {
                Vreg1_Value[0] = Vreg1;
                Vreg1_Temp_Gamma[1].Equal_Value(Gamma);
            }
            else if (Vreg1_loop_count == 2)
            {
                Vreg1_Value[1] = Vreg1;
                Vreg1_Temp_Gamma[2].Equal_Value(Gamma);
            }
            else if (Vreg1_loop_count == 3)
            {
                Vreg1_Value[2] = Vreg1;
                Vreg1_Temp_Gamma[3].Equal_Value(Gamma);
            }
            else
            {

                Vreg1_Value_Temp = Vreg1_Value[1];
                Vreg1_Value[0] = Vreg1_Value_Temp;
                Vreg1_Value_Temp = Vreg1_Value[2];
                Vreg1_Value[1] = Vreg1_Value_Temp;
                Vreg1_Value[2] = Vreg1;

                Vreg1_Temp.Equal_Value(Vreg1_Temp_Gamma[1]);
                Vreg1_Temp_Gamma[0].Equal_Value(Vreg1_Temp);
                Vreg1_Temp.Equal_Value(Vreg1_Temp_Gamma[2]);
                Vreg1_Temp_Gamma[1].Equal_Value(Vreg1_Temp);
                Vreg1_Temp.Equal_Value(Vreg1_Temp_Gamma[3]);
                Vreg1_Temp_Gamma[2].Equal_Value(Vreg1_Temp);
                Vreg1_Temp_Gamma[3].Equal_Value(Gamma);

                Vreg1_Diif_Gamma[0] = Vreg1_Temp_Gamma[1] - Vreg1_Temp_Gamma[0];
                Vreg1_Diif_Gamma[1] = Vreg1_Temp_Gamma[2] - Vreg1_Temp_Gamma[1];
                Vreg1_Diif_Gamma[2] = Vreg1_Temp_Gamma[3] - Vreg1_Temp_Gamma[2];

                //Ver5
                if ((Vreg1_Value[2] == Vreg1_Value[1] && Vreg1_Value[1] == Vreg1_Value[0]) &&
                    ((Vreg1_Diif_Gamma[0].R == Vreg1_Diif_Gamma[2].R && Vreg1_Diif_Gamma[0].B == Vreg1_Diif_Gamma[2].B) && (Vreg1_Diif_Gamma[0].R != Vreg1_Diif_Gamma[1].R || Vreg1_Diif_Gamma[0].B != Vreg1_Diif_Gamma[1].B)))
                {
                    Vreg1_Infinite = true;
                    Vreg1_Infinite_Count++;
                }

                else Vreg1_Infinite = false;

                if (Vreg1_Infinite) f1.GB_Status_AppendText_Nextline("Vreg1_Infinite : " + Vreg1_Infinite.ToString(), Color.Red);
                else f1.GB_Status_AppendText_Nextline("Vreg1_Infinite : " + Vreg1_Infinite.ToString(), Color.Green);

                if (Vreg1_Infinite_Count >= 3)
                    f1.GB_Status_AppendText_Nextline("Vreg1_Infinite_Count = " + Vreg1_Infinite_Count.ToString(), Color.Red);
                else
                    f1.GB_Status_AppendText_Nextline("Vreg1_Infinite_Count = " + Vreg1_Infinite_Count.ToString(), Color.Green);
            }
        }

        private int Meta_Get_Normal_Initial_Vreg1(int band)
        {
            switch (band)
            {
                case 0:
                    return Convert.ToInt16(textBox_Vreg1_B0.Text);
                case 1:
                    return Convert.ToInt16(textBox_Vreg1_B1.Text);
                case 2:
                    return Convert.ToInt16(textBox_Vreg1_B2.Text);
                case 3:
                    return Convert.ToInt16(textBox_Vreg1_B3.Text);
                case 4:
                    return Convert.ToInt16(textBox_Vreg1_B4.Text);
                case 5:
                    return Convert.ToInt16(textBox_Vreg1_B5.Text);
                case 6:
                    return Convert.ToInt16(textBox_Vreg1_B6.Text);
                case 7:
                    return Convert.ToInt16(textBox_Vreg1_B7.Text);
                case 8:
                    return Convert.ToInt16(textBox_Vreg1_B8.Text);
                case 9:
                    return Convert.ToInt16(textBox_Vreg1_B9.Text);
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of boundary(Not a Normal Band)");
                    Optic_Compensation_Stop = true;
                    return 0;
            }
        }

        //To Be Tested
        private double[] Get_HBM_Gamma_Green_Voltage(int[] HBM_Gamma_Green, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];

            double[] HBM_Gamma_Green_Voltage = new double[8];

            int HBM_Dec_Vreg1 = 1023;
            //double HBM_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(HBM_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
            double HBM_Vreg1_Voltage = Meta_Get_Vreg1_Voltage(HBM_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
            double HBM_AM1_RGB_Voltage = F7 + (HBM_Vreg1_Voltage - F7) * (8.0 / 700.0);

            for (int gray = 0; gray < 8; gray++)
            {
                if (gray == 0) HBM_Gamma_Green_Voltage[gray] = Meta_Get_AM2_Voltage(F7, HBM_Vreg1_Voltage, HBM_Gamma_Green[gray]);
                else HBM_Gamma_Green_Voltage[gray] = Meta_Get_Normal_Gamma_Voltage(HBM_AM1_RGB_Voltage, HBM_Gamma_Green_Voltage[gray - 1], HBM_Gamma_Green[gray]);
            }

            return HBM_Gamma_Green_Voltage;
        }

        private double[] Get_Previous_Band_Gamma_Voltage(int Previous_Band_Dec_Vreg1, int[] Previous_Band_Gamma, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];

            double[] Previous_Band_Gamma_Voltage = new double[8];

            //double Previous_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
            double Previous_Band_Vreg1_Voltage = Meta_Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
            double Previous_Band_AM1_RGB_Voltage = F7 + (Previous_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);

            for (int gray = 0; gray < 8; gray++)
            {
                if (gray == 0) Previous_Band_Gamma_Voltage[gray] = Meta_Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma[gray]);
                else Previous_Band_Gamma_Voltage[gray] = Meta_Get_Normal_Gamma_Voltage(Previous_Band_AM1_RGB_Voltage, Previous_Band_Gamma_Voltage[gray - 1], Previous_Band_Gamma[gray]);
            }

            return Previous_Band_Gamma_Voltage;
        }









        //To Be Tested
        private int Dll_C_Sharp_Meta_Get_Normal_Initial_Vreg1_Fx_HBM(bool[] Selected_Band, int[] HBM_Gamma_Green, int Vreg1_Dec_Init, int band, double band_Target_Lv, int Previous_Band_G255_Green_Gamma, int Previous_Band_Vreg1_Dec, double[] HBM_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt
            , double VREG1_REF205_volt, double F7)
        {
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];

            /*if ((Selected_Band[1] == true && band == 1) || (Selected_Band[2] == true && band == 2) || (Selected_Band[3] == true && band == 3) || (Selected_Band[4] == true && band == 4)
                || (Selected_Band[5] == true && band == 5) || (Selected_Band[6] == true && band == 6) || (Selected_Band[7] == true && band == 7) || (Selected_Band[8] == true && band == 8)
                || (Selected_Band[9] == true && band == 9))*/

            if (band >= 1 && Selected_Band[band] == true)
            {
                double[] HBM_Gamma_Green_Voltage = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                SJH_Matrix M = new SJH_Matrix();
                double[][] A_G = M.MatrixCreate(8, 8);

                //Get A[i][count] = HBM_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= 7; i++)
                {
                    count = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        A_G[i][count++] = Math.Pow(HBM_Gamma_Green_Voltage[i], j);
                    }
                }

                //Get C[8] = inv(A)[8,8] * HBM_Target_Lv[8]
                double[][] Inv_A_G = M.MatrixCreate(8, 8);
                double[] C_G = new double[8];
                Inv_A_G = M.MatrixInverse(A_G);
                C_G = M.Matrix_Multiply(Inv_A_G,HBM_Target_Lv);

                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Green = 0;
                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
                double Previous_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Actual_Previous_Vdata_Green = Meta_Engineering.Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_G255_Green_Gamma);

                //Green
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
                }

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Previous_Band_G255_Green_Gamma + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                int Vreg1_Dec = Meta_Engineering.Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                return Vreg1_Dec;
            }
            else //Band0 + Other not selected Bands's
            {
                return Vreg1_Dec_Init;
            }
        }



        private void Dll_C_Sharp_Meta_Get_Normal_Initial_Vreg1_Several_Linear_Fx_Previous_Band(ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (band >= 1 && Selected_Band[band] == true)
            {
                double[] Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);



                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                //Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
                //f1.GB_Status_AppendText_Nextline("C_G[0] , C_G[7] = " + C_G[0].ToString() + " , " + C_G[7].ToString(), Color.Blue);//Just For Debug, it can be deleted later (191113)

                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;
                double Calculated_Target_Lv = 0;
                int index = 0;
                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
                double Previous_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double Actual_Previous_Vdata_Red = Meta_Engineering.Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Meta_Engineering.Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Meta_Engineering.Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);

                //Red
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.001)
                {
                    index = 0;
                    if ((Previous_Band_Target_Lv[1] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[0])) index = 0;
                    else if ((Previous_Band_Target_Lv[2] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[1])) index = 1;
                    else if ((Previous_Band_Target_Lv[3] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[2])) index = 2;
                    else if ((Previous_Band_Target_Lv[4] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[3])) index = 3;
                    else if ((Previous_Band_Target_Lv[5] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[4])) index = 4;
                    else if ((Previous_Band_Target_Lv[6] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[5])) index = 5;
                    else if ((Previous_Band_Target_Lv[7] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[6])) index = 6;
                    else index = 10;

                    
                    Calculated_Target_Lv = (Previous_Band_Target_Lv[index + 1] - Previous_Band_Target_Lv[index]) * (Vdata - Actual_Previous_Vdata_Red) + Previous_Band_Target_Lv[index];
                    f1.GB_Status_AppendText_Nextline("(" + (Previous_Band_Target_Lv[index + 1].ToString() + "-" + Previous_Band_Target_Lv[index].ToString()) + ")*" + (Vdata - Actual_Previous_Vdata_Red).ToString() + "+" + Previous_Band_Target_Lv[index].ToString() + " = " + Calculated_Target_Lv, Color.Red);


                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0)
                    {
                        Calculated_Vdata_Red = Vdata;
                        break;

                    }

                }

                //Green
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                {
                    index = 0;
                    if ((Previous_Band_Target_Lv[1] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[0])) index = 0;
                    else if ((Previous_Band_Target_Lv[2] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[1])) index = 1;
                    else if ((Previous_Band_Target_Lv[3] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[2])) index = 2;
                    else if ((Previous_Band_Target_Lv[4] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[3])) index = 3;
                    else if ((Previous_Band_Target_Lv[5] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[4])) index = 4;
                    else if ((Previous_Band_Target_Lv[6] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[5])) index = 5;
                    else if ((Previous_Band_Target_Lv[7] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[6])) index = 6;
                    else index = 10;
                    
                    Calculated_Target_Lv = (Previous_Band_Target_Lv[index + 1] - Previous_Band_Target_Lv[index]) * (Vdata - Actual_Previous_Vdata_Green) + Previous_Band_Target_Lv[index];
                    f1.GB_Status_AppendText_Nextline("(" + (Previous_Band_Target_Lv[index + 1].ToString() + "-" + Previous_Band_Target_Lv[index].ToString()) + ")*" + (Vdata - Actual_Previous_Vdata_Green).ToString() + "+" + Previous_Band_Target_Lv[index].ToString() + " = " + Calculated_Target_Lv, Color.Green);

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0)
                    {
                        Calculated_Vdata_Green = Vdata;
                        break;
                    }
                }

                //Blue
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.001)
                {
                    index = 0;
                    if ((Previous_Band_Target_Lv[1] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[0])) index = 0;
                    else if ((Previous_Band_Target_Lv[2] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[1])) index = 1;
                    else if ((Previous_Band_Target_Lv[3] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[2])) index = 2;
                    else if ((Previous_Band_Target_Lv[4] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[3])) index = 3;
                    else if ((Previous_Band_Target_Lv[5] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[4])) index = 4;
                    else if ((Previous_Band_Target_Lv[6] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[5])) index = 5;
                    else if ((Previous_Band_Target_Lv[7] < Target_Lv) && (Target_Lv <= Previous_Band_Target_Lv[6])) index = 6;
                    else index = 10;

                    Calculated_Target_Lv = (Previous_Band_Target_Lv[index + 1] - Previous_Band_Target_Lv[index]) * (Vdata - Actual_Previous_Vdata_Blue) + Previous_Band_Target_Lv[index];
                    f1.GB_Status_AppendText_Nextline("(" + (Previous_Band_Target_Lv[index + 1].ToString() + "-" + Previous_Band_Target_Lv[index].ToString()) + ")*" + (Vdata - Actual_Previous_Vdata_Blue).ToString() + "+" + Previous_Band_Target_Lv[index].ToString() + " = " + Calculated_Target_Lv, Color.Blue);

                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
                    {
                        Calculated_Vdata_Blue = Vdata;
                        break;
                    }
                          
                }

                
                f1.GB_Status_AppendText_Nextline("Calculated_Vdata_Red : " + Calculated_Vdata_Red, Color.Red);
                f1.GB_Status_AppendText_Nextline("Calculated_Vdata_Green : " + Calculated_Vdata_Green, Color.Green);
                f1.GB_Status_AppendText_Nextline("Calculated_Vdata_Blue : " + Calculated_Vdata_Blue, Color.Blue);

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Previous_Band_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                Vreg1_Dec_Init = Meta_Engineering.Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                //Got the Vreg1 
                //Need to get Gamma_R/B
                double Current_Band_AM1_RGB_Voltage = F7 + (Vreg1_voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_B = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Blue);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }




        //To Be Tested
        private void Dll_C_Sharp_Meta_Get_Normal_Initial_Vreg1_Fx_Previous_Band(ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
            
        {
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];

            /*if ((Selected_Band[1] == true && band == 1) || (Selected_Band[2] == true && band == 2) || (Selected_Band[3] == true && band == 3) || (Selected_Band[4] == true && band == 4)
                || (Selected_Band[5] == true && band == 5) || (Selected_Band[6] == true && band == 6) || (Selected_Band[7] == true && band == 7) || (Selected_Band[8] == true && band == 8)
                || (Selected_Band[9] == true && band == 9))*/

            if (band >= 1 && Selected_Band[band] == true)
            {
                double[] Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                
                SJH_Matrix M = new SJH_Matrix();
                double[][] A_R = M.MatrixCreate(8, 8);
                double[][] A_G = M.MatrixCreate(8, 8);
                double[][] A_B = M.MatrixCreate(8, 8);

                //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= 7; i++)
                {
                    count = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        A_R[i][count] = Math.Pow(Previous_Band_Gamma_Red_Voltage[i], j);
                        A_G[i][count] = Math.Pow(Previous_Band_Gamma_Green_Voltage[i], j);
                        A_B[i][count] = Math.Pow(Previous_Band_Gamma_Blue_Voltage[i], j);
                        count++;
                    }
                }

                //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
                double[][] Inv_A_R = M.MatrixCreate(8, 8);
                double[][] Inv_A_G = M.MatrixCreate(8, 8);
                double[][] Inv_A_B = M.MatrixCreate(8, 8);
                double[] C_R = new double[8];
                double[] C_G = new double[8];
                double[] C_B = new double[8];
                Inv_A_R = M.MatrixInverse(A_R);
                Inv_A_G = M.MatrixInverse(A_G);
                Inv_A_B = M.MatrixInverse(A_B);
                C_R = M.Matrix_Multiply(Inv_A_R, Previous_Band_Target_Lv);
                C_G = M.Matrix_Multiply(Inv_A_G, Previous_Band_Target_Lv);
                C_B = M.Matrix_Multiply(Inv_A_B, Previous_Band_Target_Lv);

                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
                string temp = "";
                for (int i = 0; i < 8; i++) temp += (C_G[i].ToString() + " ");
                f1.GB_Status_AppendText_Nextline("C_G[0] + C_G[1]+ ... + C_G[7] = " + temp, Color.Red);//Just For Debug, it can be deleted later (191113)


                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;
                
                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
                double Previous_Band_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double Actual_Previous_Vdata_Red = Meta_Engineering.Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Meta_Engineering.Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Meta_Engineering.Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);
                
                //Red
                //for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0)
                    {
                        Calculated_Vdata_Red = Vdata;
                        break;
                    }
                }

                //Green
                //for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0)
                    {
                        Calculated_Vdata_Green = Vdata;
                        break;
                    }
                }

                //Blue
                //for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
                    {
                        Calculated_Vdata_Blue = Vdata;
                        break;
                    }
                }

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Previous_Band_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                Vreg1_Dec_Init = Meta_Engineering.Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                
                //Got the Vreg1 
                //Need to get Gamma_R/B
                double Current_Band_AM1_RGB_Voltage = F7 + (Vreg1_voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_B = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Blue);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }








        //(C#)
        private int Meta_Get_Normal_Initial_Vreg1_Fx_HBM(int band, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt
            , double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            
            if (checkBox_Fast_OC_B0.Checked && band == 0)
            {
                return Meta_Get_Normal_Initial_Vreg1(band);
            }
            else if ((checkBox_Fast_OC_B1.Checked && band == 1) || (checkBox_Fast_OC_B2.Checked && band == 2) || (checkBox_Fast_OC_B3.Checked && band == 3) || (checkBox_Fast_OC_B4.Checked && band == 4)
                || ( checkBox_Fast_OC_B5.Checked && band == 5) || (checkBox_Fast_OC_B6.Checked && band == 6) || (checkBox_Fast_OC_B7.Checked && band == 7) || (checkBox_Fast_OC_B8.Checked && band == 8)
                || (checkBox_Fast_OC_B9.Checked && band == 9))
            {
                SJH_Matrix M = new SJH_Matrix();
                double[] Lv = new double[8];
                double[] Vdata_G = new double[8];
                double[][] A_G = M.MatrixCreate(8, 8);
            
                //Get HBM Gamma
                RGB[] HBM_Gamma = new RGB[8];//10ea Bands , 8ea Gray-points (Add on 191028)
                XYLv[] HBM_Target = new XYLv[8];//Add on 191105
                RGB_Double[] HBM_Gamma_Voltage = new RGB_Double[8];
                
                int HBM_Dec_Vreg1 = Meta_Get_Normal_Initial_Vreg1(0);
                double HBM_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(HBM_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double HBM_AM1_RGB_Voltage = F7 + (HBM_Vreg1_Voltage - F7) * (8.0 / 700.0);

                for (int gray = 0; gray < 8; gray++)
                {
                    HBM_Gamma[gray].int_G = Convert.ToInt16(Meta_Engineering.dataGridView_OC_param.Rows[gray + 2].Cells[2].Value);
                    HBM_Target[gray].double_Lv = Convert.ToDouble(Meta_Engineering.dataGridView_OC_param.Rows[gray + 2].Cells[9].Value);
                    
                    //f1.GB_Status_AppendText_Nextline("F [B/G] HBM Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + HBM_Gamma[gray].int_R.ToString() + "/" + HBM_Gamma[gray].int_G.ToString() + "/" + HBM_Gamma[gray].int_B.ToString(), Color.Blue);

                    if (gray == 0)HBM_Gamma_Voltage[gray].double_G = Meta_Engineering.Get_AM2_Voltage(F7, HBM_Vreg1_Voltage, HBM_Gamma[gray].int_G);
                    else HBM_Gamma_Voltage[gray].double_G = Meta_Engineering.Get_Normal_Gamma_Voltage(HBM_AM1_RGB_Voltage, HBM_Gamma_Voltage[gray - 1].double_G, HBM_Gamma[gray].int_G);
                }

                //double[][] inv = M.MatrixInverse(A);
                //Get A and Lv
                int count = 0;
                for (int i = 0; i <= 7; i++)
                {
                    Vdata_G[i] = HBM_Gamma_Voltage[i].double_G;
                    Lv[i] = HBM_Target[i].double_Lv;
                    count = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        A_G[i][count] = Math.Pow(Vdata_G[i], j);
                        count++;
                    }
                }

                //Show A and Lv
                string[] Temp_G = new string[8];
                for (int i = 0; i <= 7; i++)
                {
                    Temp_G[i] = (Lv[i].ToString() + "(nit):");
                    for (int j = 0; j <= 7; j++) Temp_G[i] += (" " + A_G[i][j].ToString());
                }

                //Get C = inv(A) * Lv
                double[][] Inv_A_G = M.MatrixCreate(8, 8);
                double[] C_G = new double[8];
                Inv_A_G = M.MatrixInverse(A_G);
                C_G = M.Matrix_Multiply(Inv_A_G, Lv);
                
                //Show C
                string temp_C_G = "G :";
                for (int i = 0; i <= 7; i++) temp_C_G += (" " + C_G[i].ToString());

                //f1.GB_Status_AppendText_Nextline(temp_C_G, Color.Green);
                
                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                double Target_Lv = Convert.ToDouble(Meta_Engineering.dataGridView_OC_param.Rows[(band * 8) + (0 + 2)].Cells[9].Value);
                double Calculated_Vdata_Green = 0;
                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                int Previous_Green_Gamma = Convert.ToInt16(Meta_Engineering.dataGridView_OC_param.Rows[((band - 1) * 8) + (0 + 2)].Cells[2].Value);
                int Previous_Dec_Vreg1 = Meta_Get_Normal_Initial_Vreg1(band - 1);
                double Previous_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Previous_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Actual_Previous_Vdata_Green = Meta_Engineering.Get_AM2_Voltage(F7, Previous_Vreg1_Voltage, Previous_Green_Gamma);

                //f1.GB_Status_AppendText_Nextline("Target_Lv : " + Target_Lv.ToString(), Color.Green);
                //f1.GB_Status_AppendText_Nextline("Previous_Green_Gamma : " + Previous_Green_Gamma.ToString(), Color.Green);
                //f1.GB_Status_AppendText_Nextline("Previous_Dec_Vreg1 : " + Previous_Dec_Vreg1.ToString(), Color.Green);
                //f1.GB_Status_AppendText_Nextline("Previous_Vreg1_Voltage : " + Previous_Vreg1_Voltage.ToString(), Color.Green);
                //f1.GB_Status_AppendText_Nextline("Actual_Previous_Vdata_Green : " + Actual_Previous_Vdata_Green.ToString(), Color.Green);

                //Green
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
                }
                //f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated_Vdata_G = " + Calculated_Vdata_Green, Color.Green);

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (HBM_Gamma[0].int_G + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                int Vreg1_dec = Meta_Engineering.Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated Band1 Vreg1(Get by Gamma G) : " + Vreg1_voltage.ToString() + "/" + Vreg1_dec.ToString(), Color.Green);

              

                //Copy "Previous Band Gamma to Current Band Gamma" and Set "All_band_gray_Gamma"
                Meta_Engineering.Copy_Previous_Band_Gamma(band);
                System.Windows.Forms.Application.DoEvents();
                Meta_Engineering.Meta_Get_Band_Gray_Gamma(All_band_gray_Gamma, band);
                
                //Set Calculated Vreg1_dec
                Meta_Update_and_Send_Vreg1(Vreg1_dec, band);
                Thread.Sleep(20);
                Meta_Update_Vreg1_TextBox(Vreg1_dec, band);
                
                //return Calculated Vreg1_dec
                return Vreg1_dec;
            }
            else
            {
                return Meta_Get_Normal_Initial_Vreg1(band);
            }
        }





        public string Update_and_Send_All_Band_Gray_Gamma(RGB[,] All_band_gray_Gamma, RGB Current_Gamma, int Current_Band, int Current_Gray)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            //Update All_band_gray_Gamma[Current_Band, Current_Gray] as current Gamma
            All_band_gray_Gamma[Current_Band, Current_Gray].Equal_Value(Current_Gamma);
           
            //Gray255 ~ Gray4
            RGB[] Gamma_9th_data = new RGB[9]; //Data[8]
            RGB[] Gamma_8ea_data = new RGB[9]; //Data[7:0]
            for (int gray = 0; gray < 8; gray++)
            {
                Gamma_9th_data[gray].Set_Value(All_band_gray_Gamma[Current_Band, gray].int_R >> 8, All_band_gray_Gamma[Current_Band, gray].int_G >> 8, All_band_gray_Gamma[Current_Band, gray].int_B >> 8);
                Gamma_8ea_data[gray].Set_Value(All_band_gray_Gamma[Current_Band, gray].int_R & 0xFF, All_band_gray_Gamma[Current_Band, gray].int_G & 0xFF, All_band_gray_Gamma[Current_Band, gray].int_B & 0xFF);
                Gamma_9th_data[gray].Binary_String_Update_From_Int(1,'0');
                Gamma_8ea_data[gray].Binary_String_Update_From_Int();
            }

            //Gray1 needs to be updated by All_band_gray_Gamma[Current_Band, 7]
            RGB Gray1 = new RGB();
            int Offset_R = Convert.ToInt16(textBox_G1_Offset_From_G4_R.Text);
            int Offset_G = Convert.ToInt16(textBox_G1_Offset_From_G4_G.Text);
            int Offset_B = Convert.ToInt16(textBox_G1_Offset_From_G4_B.Text);
            Gray1.Set_Value(All_band_gray_Gamma[Current_Band, 7].int_R + Offset_R, All_band_gray_Gamma[Current_Band, 7].int_G + Offset_G, All_band_gray_Gamma[Current_Band, 7].int_B + Offset_B);
            Gamma_9th_data[8].Set_Value(Gray1.int_R >> 8, Gray1.int_G >> 8, Gray1.int_B >> 8);
            Gamma_8ea_data[8].Set_Value(Gray1.int_R & 0xFF, Gray1.int_G & 0xFF, Gray1.int_B & 0xFF);
            Gamma_9th_data[8].Binary_String_Update_From_Int(1, '0');
            Gamma_8ea_data[8].Binary_String_Update_From_Int();

            //Setting Hex_Param[33]
            string[] Hex_Param = new string[33]; //Gamma Params 33ea for each register

            //R
            //Hex_Param[0] = Gamma_9th_data[0].Binary_R + Gamma_8ea_data[7].Binary_R.PadLeft(7, '0'); ... Mistake (SW43408 Way...)
            Hex_Param[0] = Gamma_9th_data[0].Binary_R.PadRight(8, '0');
            
            Hex_Param[1] = Gamma_8ea_data[0].Binary_R.PadLeft(8, '0');
            Hex_Param[2] = string.Empty; for (int i = 1; i <= 8; i++) Hex_Param[2] += Gamma_9th_data[i].Binary_R;
            //f1.GB_Status_AppendText_Nextline("R : " + Hex_Param[2], Color.Red);
            Hex_Param[3] = Gamma_8ea_data[8].Binary_R.PadLeft(8, '0');
            Hex_Param[4] = Gamma_8ea_data[7].Binary_R.PadLeft(8, '0');
            Hex_Param[5] = Gamma_8ea_data[6].Binary_R.PadLeft(8, '0');
            Hex_Param[6] = Gamma_8ea_data[5].Binary_R.PadLeft(8, '0');
            Hex_Param[7] = Gamma_8ea_data[4].Binary_R.PadLeft(8, '0');
            Hex_Param[8] = Gamma_8ea_data[3].Binary_R.PadLeft(8, '0');
            Hex_Param[9] = Gamma_8ea_data[2].Binary_R.PadLeft(8, '0');
            Hex_Param[10] = Gamma_8ea_data[1].Binary_R.PadLeft(8, '0');

            //G
            int Offset = 11;
            //Hex_Param[0 + Offset] = Gamma_9th_data[0].Binary_G + Gamma_8ea_data[7].Binary_G.PadLeft(7, '0'); ... Mistake (SW43408 Way...)
            Hex_Param[0 + Offset] = Gamma_9th_data[0].Binary_G.PadRight(8, '0');     
            Hex_Param[1 + Offset] = Gamma_8ea_data[0].Binary_G.PadLeft(8, '0');
            Hex_Param[2 + Offset] = string.Empty; for (int i = 1; i <= 8; i++) Hex_Param[2 + Offset] += Gamma_9th_data[i].Binary_G;
            //f1.GB_Status_AppendText_Nextline("G : " + Hex_Param[2 + Offset], Color.Green);
            Hex_Param[3 + Offset] = Gamma_8ea_data[8].Binary_G.PadLeft(8, '0');
            Hex_Param[4 + Offset] = Gamma_8ea_data[7].Binary_G.PadLeft(8, '0');
            Hex_Param[5 + Offset] = Gamma_8ea_data[6].Binary_G.PadLeft(8, '0');
            Hex_Param[6 + Offset] = Gamma_8ea_data[5].Binary_G.PadLeft(8, '0');
            Hex_Param[7 + Offset] = Gamma_8ea_data[4].Binary_G.PadLeft(8, '0');
            Hex_Param[8 + Offset] = Gamma_8ea_data[3].Binary_G.PadLeft(8, '0');
            Hex_Param[9 + Offset] = Gamma_8ea_data[2].Binary_G.PadLeft(8, '0');
            Hex_Param[10 + Offset] = Gamma_8ea_data[1].Binary_G.PadLeft(8, '0');

            //B
            Offset = 22;
            //Hex_Param[0 + Offset] = Gamma_9th_data[0].Binary_B + Gamma_8ea_data[7].Binary_B.PadLeft(7, '0'); ... Mistake (SW43408 Way...)
            Hex_Param[0 + Offset] = Gamma_9th_data[0].Binary_B.PadRight(8, '0');
            Hex_Param[1 + Offset] = Gamma_8ea_data[0].Binary_B.PadLeft(8, '0');
            Hex_Param[2 + Offset] = string.Empty; for (int i = 1; i <= 8; i++) Hex_Param[2 + Offset] += Gamma_9th_data[i].Binary_B;
            //f1.GB_Status_AppendText_Nextline("B : " + Hex_Param[2 + Offset], Color.Blue);
            Hex_Param[3 + Offset] = Gamma_8ea_data[8].Binary_B.PadLeft(8, '0');
            Hex_Param[4 + Offset] = Gamma_8ea_data[7].Binary_B.PadLeft(8, '0');
            Hex_Param[5 + Offset] = Gamma_8ea_data[6].Binary_B.PadLeft(8, '0');
            Hex_Param[6 + Offset] = Gamma_8ea_data[5].Binary_B.PadLeft(8, '0');
            Hex_Param[7 + Offset] = Gamma_8ea_data[4].Binary_B.PadLeft(8, '0');
            Hex_Param[8 + Offset] = Gamma_8ea_data[3].Binary_B.PadLeft(8, '0');
            Hex_Param[9 + Offset] = Gamma_8ea_data[2].Binary_B.PadLeft(8, '0');
            Hex_Param[10 + Offset] = Gamma_8ea_data[1].Binary_B.PadLeft(8, '0');

            //Convert Binary to Hex
            for (int i = 0; i < 33; i++) Hex_Param[i] = Convert.ToInt16(Hex_Param[i], 2).ToString("X2");

            //Sending Hexparam at current band register
            Meta_SW43408B Meta = Meta_SW43408B.getInstance();
            string Current_Band_Register = Meta.Get_Gamma_Register_Hex_String(Current_Band).Remove(0, 2); //0xXX --> XX
            return f1.Long_Packet_CMD_Send(33, Current_Band_Register, Hex_Param); //send and return mipi.cmd as string
        }








        public void Meta_DBV_Setting(int band)
        {
            switch (band)
            {
                case 0:
                    button_B1_DBV_Send.PerformClick();
                    break;
                case 1:
                    button_B2_DBV_Send.PerformClick();
                    break;
                case 2:
                    button_B3_DBV_Send.PerformClick();
                    break;
                case 3:
                    button_B4_DBV_Send.PerformClick();
                    break;
                case 4:
                    button_B5_DBV_Send.PerformClick();
                    break;
                case 5:
                    button_B6_DBV_Send.PerformClick();
                    break;
                case 6:
                    button_B7_DBV_Send.PerformClick();
                    break;
                case 7:
                    button_B8_DBV_Send.PerformClick();
                    break;
                case 8:
                    button_B9_DBV_Send.PerformClick();
                    break;
                case 9:
                    button_B10_DBV_Send.PerformClick();
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                    break;
            }
            Thread.Sleep(100);
        }



        private bool Band_BSQH_Selection(ref int band)
        {
            if (checkBox_Band1.Checked == false && band == 0)
                return false;
            else if (checkBox_Band2.Checked == false && band == 1)
                return false;
            else if (checkBox_Band3.Checked == false && band == 2)
                return false;
            else if (checkBox_Band4.Checked == false && band == 3)
                return false;
            else if (checkBox_Band5.Checked == false && band == 4)
                return false;
            else if (checkBox_Band6.Checked == false && band == 5)
                return false;
            else if (checkBox_Band7.Checked == false && band == 6)
                return false;
            else if (checkBox_Band8.Checked == false && band == 7)
                return false;
            else if (checkBox_Band9.Checked == false && band == 8)
                return false;
            else if (checkBox_Band10.Checked == false && band == 9)
                return false;
            else
                return true;
        }


        private void Get_All_Band_Gray_Gamma(RGB[,] All_band_gray_Gamma)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Get All Band/Gray Gamma from OC_Param", Color.Blue);

            Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_form_engineer.Meta_Get_All_Band_Gray_Gamma(All_band_gray_Gamma);
        }

        private bool ProgressBar_Max_Step_Setting(int step)
        {
            int ProgressBar_max = 0;

            //if (checkBox_VREF2_AM0_Compensation.Checked) ProgressBar_max += step;

            //How many BSQH Points are checked ? (graypoints = 10)
            if (checkBox_Band1.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band2.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band3.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band4.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band5.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band6.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band7.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band8.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band9.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band10.Checked) ProgressBar_max += (8 * step);
            
            bool If_Any_Band_Is_Selected;
            if (ProgressBar_max == 0)
                If_Any_Band_Is_Selected = false;
            else
                If_Any_Band_Is_Selected = true;


            //OTP Auto Write checked 
            if (checkBox_1st_Mode_OTP_AutoWirte.Checked) ProgressBar_max += (5 * step);

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Set_GB_ProgressBar_Maximum(ProgressBar_max);
            f1.Set_GB_ProgressBar_Step(step);

            return If_Any_Band_Is_Selected;
        }

        private void button_Gray_Points_Measure_Click(object sender, EventArgs e)
        {
            int measure_delay = Convert.ToInt16(textBox_Gray_Points_Measure_Delay.Text);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Gray 8 Points Measure Start", Color.Blue);

            for (int gray = 0; gray < 8; gray++)
            {
                Meta_Pattern_Setting(gray);
                Thread.Sleep(measure_delay);
                f1.CA_Measure_BT_Click();
            }
            f1.GB_Status_AppendText_Nextline("Gray 8 Points Measure End", Color.Blue);
        }

        private void SW43410_POCB_On_btn_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Meta POCB On(G2G On)", Color.Blue);
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0xAC",Color.Blue); //Access to MCS
            f1.IPC_Quick_Send_And_Show("mipi.write 0x39 0xEB 0x00 0x27", Color.Blue); //POCB on
            
        }

        private void SW43410_POCB_Off_btn_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Meta POCB Off(G2G Off)", Color.Red);
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0xAC", Color.Red); //Access to MCS
            f1.IPC_Quick_Send_And_Show("mipi.write 0x39 0xEB 0x00 0x00", Color.Red); //POCB off
        }




        private void VPP_On()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("power.vpp.level 6.0");
            f1.IPC_Quick_Send("power.on vpp");
        }
        private void Sleep_In()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x05 0x10");
        }
        private void Sleep_Out()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x05 0x11");
        }
        private void Display_On()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x05 0x29");
        }
        private void DIsplay_Off()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x05 0x28");
        }
        private void VPP_Off()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("power.off vpp");
        }
        private void Set_Still_Image()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("spi.write 0x3D 0x0086");
        }
        private void Set_Moving_image()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("spi.write 0x3D 0x0082");
        }
        private void Hs_off_LP_on()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.video.disable");
        }
        private void Hs_on_LP_off()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.video.enable");
        }

        private void Start_OTP_Write()
        {
            this.DIsplay_Off();
            Thread.Sleep(100);
            Set_Still_Image(); // 반드시 설정해야 OTP 정상 write 가능
            this.Hs_off_LP_on(); // 반드시 설정해야 OTP 정상 write 가능
            this.VPP_On();
            Thread.Sleep(200);
        }

        private void End_OTP_Write()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            VPP_Off();
            Set_Moving_image(); // 반드시 설정해야 OTP 정상 write 가능
            this.Hs_on_LP_off(); // 반드시 설정해야 OTP 정상 write 가능
            //OnBnClickedButtonPoweroff(); // 반드시 설정해야 OTP 정상 write 가능          
            Thread.Sleep(200);
            Sleep_Out();
            f1.IPC_Quick_Send("delay 150");
            this.Display_On();
        }



        private bool OTP_Write_fail_check()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.textBox2_cmd.Text = string.Empty;
            f1.OTP_Read(1, "FD");
            char[] mipi_data = f1.textBox2_cmd.Text.ToCharArray();
            //Hex
            string Param1 = new string(mipi_data, 19, 2);
            //binary
            string param1_bi = (Convert.ToString(Convert.ToInt32(Param1, 16), 2)).PadLeft(8, '0');

            //Binary OTP Fail check
            string OTP_Fail_Check = param1_bi.Substring(0, 1);
            //Dec OTP Fail check
            int OTP_Fail_Check_int = Convert.ToInt32(OTP_Fail_Check, 2);

            if (OTP_Fail_Check_int == 0)
            {
                f1.GB_Status_AppendText_Nextline("OTP Write Fail Check : OK",Color.Blue);
                return true;
            }
            else if (OTP_Fail_Check_int == 1)
            {
                f1.GB_Status_AppendText_Nextline("OTP Write Fail Check : NG", Color.Red);
                return false;
            }
            else
            {
                return false;
            }
        }


        private void OTP_Main1_Write()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            bool OTP_Write_Success_check1 = true;
            bool OTP_Write_Success_check2 = true;

            //OTP_check_button.PerformClick();
            
            if (this.label_M1.Text == "OTP 5")
            {
                label_OTP_Status = "Main1 OTP are fully written";
                f1.GB_Status_AppendText_Nextline(label_OTP_Status, Color.Red);
            }
            else
            {
                Start_OTP_Write();
                //Select Main1 Area
                f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x6A"); //Write and Margin Read main 1
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 100");

                //Margin1 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x61"); //Margin 1 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check1 = OTP_Write_fail_check();

                //Margin2 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x62"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check2 = OTP_Write_fail_check();

                //Margin read off
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x63");
                End_OTP_Write();

                if (OTP_Write_Success_check1 && OTP_Write_Success_check1)
                {
                    f1.GB_Status_AppendText_Nextline("M1 OTP writing was successful ! ",Color.Green);
                }
                else if (OTP_Write_Success_check1 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M1 OTP Margin 1 writing fail ", Color.Red);
                }
                else if (OTP_Write_Success_check2 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M1 OTP Margin 2 writing fail ", Color.Red);
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("M1 OTP Margin 1 and 2 writing fail ", Color.Red);
                }
            }
        }

        private void OTP_Main2_Write()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            bool OTP_Write_Success_check1 = true;
            bool OTP_Write_Success_check2 = true;

            //OTP_check_button.PerformClick();

            if (this.label_M2.Text == "OTP 5")
            {
                label_OTP_Status = "Main2 OTP are fully written";
                f1.GB_Status_AppendText_Nextline(label_OTP_Status, Color.Red);
                
            }
            else
            {
                Start_OTP_Write();
                //Select Main1 Area
                f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x7A"); //Write and Margin Read main 1
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 500");

                //Margin1 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x71"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check1 = OTP_Write_fail_check();

                //Margin2 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x72"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check2 = OTP_Write_fail_check();

                //Margin read off
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x73");
                End_OTP_Write();

                if (OTP_Write_Success_check1 && OTP_Write_Success_check1)
                {
                    f1.GB_Status_AppendText_Nextline("M2 OTP writing was successful ! ", Color.Green);
                }
                else if (OTP_Write_Success_check1 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M2 OTP Margin 1 writing fail ", Color.Red);
                }
                else if (OTP_Write_Success_check2 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M2 OTP Margin 2 writing fail ", Color.Red);
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("M2 OTP Margin 1 and 2 writing fail ", Color.Red);
                }
            }
        }



        private void OTP_Main3_Write()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            bool OTP_Write_Success_check1 = true;
            bool OTP_Write_Success_check2 = true;

            //OTP_check_button.PerformClick();

            if (this.label_M3.Text == "OTP 5")
            {
                label_OTP_Status = "Main3 OTP are fully written";
                f1.GB_Status_AppendText_Nextline(label_OTP_Status, Color.Red);
            }
            else
            {
                Start_OTP_Write();
                //Select Main1 Area
                f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x8A"); //Write and Margin Read main 1
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 800");

                //Margin1 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x81"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check1 = OTP_Write_fail_check();

                //Margin2 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x82"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check2 = OTP_Write_fail_check();

                //Margin read off
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x83");
                End_OTP_Write();

                if (OTP_Write_Success_check1 && OTP_Write_Success_check1)
                {
                    f1.GB_Status_AppendText_Nextline("M3 OTP writing was successful ! ", Color.Green);
                }
                else if (OTP_Write_Success_check1 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M3 OTP Margin 1 writing fail ", Color.Red);
                }
                else if (OTP_Write_Success_check2 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M3 OTP Margin 2 writing fail ", Color.Red);
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("M3 OTP Margin 1 and 2 writing fail ", Color.Red);
                }
            }
        }


        private void OTP_Main4_Write()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            bool OTP_Write_Success_check1 = true;
            bool OTP_Write_Success_check2 = true;

            //OTP_check_button.PerformClick();

            if (this.label_M4.Text == "OTP 5")
            {
                label_OTP_Status = "Main4 OTP are fully written";
                f1.GB_Status_AppendText_Nextline(label_OTP_Status, Color.Red);
            }
            else
            {
                Start_OTP_Write();
                //Select Main1 Area
                f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x9A"); //Write and Margin Read main 1
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 500");

                //Margin1 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x91"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check1 = OTP_Write_fail_check();

                //Margin2 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x92"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check2 = OTP_Write_fail_check();

                //Margin read off
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0x93");
                End_OTP_Write();

                if (OTP_Write_Success_check1 && OTP_Write_Success_check1)
                {
                    f1.GB_Status_AppendText_Nextline("M4 OTP writing was successful ! ", Color.Green);
                }
                else if (OTP_Write_Success_check1 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M4 OTP Margin 1 writing fail ", Color.Red);
                }
                else if (OTP_Write_Success_check2 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M4 OTP Margin 2 writing fail ", Color.Red);
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("M4 OTP Margin 1 and 2 writing fail ", Color.Red);
                }
            }
        }

        private void OTP_Main5_Write()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            bool OTP_Write_Success_check1 = true;
            bool OTP_Write_Success_check2 = true;

            //OTP_check_button.PerformClick();

            if (this.label_M5.Text == "OTP 4")
            {
                label_OTP_Status = "Main5 OTP are fully written";
                f1.GB_Status_AppendText_Nextline(label_OTP_Status, Color.Red);
            }
            else
            {
                Start_OTP_Write();
                //Select Main1 Area
                f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x55");
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xAA"); //Write and Margin Read main 1
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 1000");

                //Margin1 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xA1"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check1 = OTP_Write_fail_check();

                //Margin2 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xA2"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check2 = OTP_Write_fail_check();

                //Margin read off
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xA3");
                End_OTP_Write();

                if (OTP_Write_Success_check1 && OTP_Write_Success_check1)
                {
                    f1.GB_Status_AppendText_Nextline("M5 OTP writing was successful ! ", Color.Green);
                }
                else if (OTP_Write_Success_check1 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M5 OTP Margin 1 writing fail ", Color.Red);
                }
                else if (OTP_Write_Success_check2 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M5 OTP Margin 2 writing fail ", Color.Red);
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("M5 OTP Margin 1 and 2 writing fail ", Color.Red);
                }
            }
        }

        /*
        private void OTP_Main6_Write()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            bool OTP_Write_Success_check1 = true;
            bool OTP_Write_Success_check2 = true;

            //OTP_check_button.PerformClick();

            if (this.label_M6.Text == "OTP 4")
            {
                label_OTP_Status = "Main6 OTP are fully written";
                f1.GB_Status_AppendText_Nextline(label_OTP_Status, Color.Red);
            }
            else
            {
                Start_OTP_Write();
                //Select Main1 Area
                f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x55");
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBA"); //Write and Margin Read main 6
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 600");

                //Margin1 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xB1"); //Margin 1 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check1 = OTP_Write_fail_check();

                //Margin2 read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xB2"); //Margin 2 Read
                f1.IPC_Quick_Send("mipi.write 0x15 0xFC 0xBB"); //Confirm cmd
                f1.IPC_Quick_Send("delay 10");
                OTP_Write_Success_check2 = OTP_Write_fail_check();

                //Margin read off
                f1.IPC_Send("mipi.write 0x15 0xFC 0xB3");
                End_OTP_Write();

                if (OTP_Write_Success_check1 && OTP_Write_Success_check1)
                {
                    f1.GB_Status_AppendText_Nextline("M6 OTP writing was successful ! ", Color.Green);
                }
                else if (OTP_Write_Success_check1 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M6 OTP Margin 1 writing fail ", Color.Red);
                }
                else if (OTP_Write_Success_check2 == false)
                {
                    f1.GB_Status_AppendText_Nextline("M6 OTP Margin 2 writing fail ", Color.Red);
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("M6 OTP Margin 1 and 2 writing fail ", Color.Red);
                }
            }
        }
        */

        private void OTP_check_button_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            f1.textBox2_cmd.Text = string.Empty;
            f1.OTP_Read(6, "FD");
            char[] mipi_data = f1.textBox2_cmd.Text.ToCharArray();

            //Hex           
            string Param3 = new string(mipi_data, 29, 2);
            string Param4 = new string(mipi_data, 34, 2);
            string Param5 = new string(mipi_data, 39, 2);
            string Param6 = new string(mipi_data, 44, 2);

            //Binary           
            string param3_bi = (Convert.ToString(Convert.ToInt32(Param3, 16), 2)).PadLeft(8, '0');
            string param4_bi = (Convert.ToString(Convert.ToInt32(Param4, 16), 2)).PadLeft(8, '0');
            string param5_bi = (Convert.ToString(Convert.ToInt32(Param5, 16), 2)).PadLeft(8, '0');
            string param6_bi = (Convert.ToString(Convert.ToInt32(Param6, 16), 2)).PadLeft(8, '0');

            //Binary Main 1~6
            string PMain1 = param3_bi.Substring(5, 3);
            string PMain2 = param4_bi.Substring(1, 3);
            string PMain3 = param4_bi.Substring(5, 3);
            string PMain4 = param5_bi.Substring(1, 3);
            string PMain5 = param5_bi.Substring(5, 3);
            string PMain6 = param6_bi.Substring(5, 3);

            //Dec Main 1~6
            int PMain1_Int = Convert.ToInt32(PMain1, 2);
            int PMain2_Int = Convert.ToInt32(PMain2, 2);
            int PMain3_Int = Convert.ToInt32(PMain3, 2);
            int PMain4_Int = Convert.ToInt32(PMain4, 2);
            int PMain5_Int = Convert.ToInt32(PMain5, 2);
            int PMain6_Int = Convert.ToInt32(PMain6, 2);

            switch (PMain1_Int)
            {
                case 0:
                    this.label_M1.Text = "Empty";
                    break;
                case 1:
                    this.label_M1.Text = "OTP 1";
                    break;
                case 2:
                    this.label_M1.Text = "OTP 2";
                    break;
                case 3:
                    this.label_M1.Text = "OTP 3";
                    break;
                case 4:
                    this.label_M1.Text = "OTP 4";
                    break;
                case 5:
                    this.label_M1.Text = "OTP 5";
                    break;
                default:
                    break;
            }

            switch (PMain2_Int)
            {
                case 0:
                    this.label_M2.Text = "Empty";
                    break;
                case 1:
                    this.label_M2.Text = "OTP 1";
                    break;
                case 2:
                    this.label_M2.Text = "OTP 2";
                    break;
                case 3:
                    this.label_M2.Text = "OTP 3";
                    break;
                case 4:
                    this.label_M2.Text = "OTP 4";
                    break;
                case 5:
                    this.label_M2.Text = "OTP 5";
                    break;
                default:
                    break;
            }

            switch (PMain3_Int)
            {
                case 0:
                    this.label_M3.Text = "Empty";
                    break;
                case 1:
                    this.label_M3.Text = "OTP 1";
                    break;
                case 2:
                    this.label_M3.Text = "OTP 2";
                    break;
                case 3:
                    this.label_M3.Text = "OTP 3";
                    break;
                case 4:
                    this.label_M3.Text = "OTP 4";
                    break;
                case 5:
                    this.label_M3.Text = "OTP 5";
                    break;
                default:
                    break;
            }

            switch (PMain4_Int)
            {
                case 0:
                    this.label_M4.Text = "Empty";
                    break;
                case 1:
                    this.label_M4.Text = "OTP 1";
                    break;
                case 2:
                    this.label_M4.Text = "OTP 2";
                    break;
                case 3:
                    this.label_M4.Text = "OTP 3";
                    break;
                case 4:
                    this.label_M4.Text = "OTP 4";
                    break;
                case 5:
                    this.label_M4.Text = "OTP 5";
                    break;
                default:
                    break;
            }

            switch (PMain5_Int)
            {
                case 0:
                    this.label_M5.Text = "Empty";
                    break;
                case 1:
                    this.label_M5.Text = "OTP 1";
                    break;
                case 2:
                    this.label_M5.Text = "OTP 2";
                    break;
                case 3:
                    this.label_M5.Text = "OTP 3";
                    break;
                case 4:
                    this.label_M5.Text = "OTP 4";
                    break;
                default:
                    break;
            }
            
            /*
            switch (PMain6_Int)
            {
                case 0:
                    this.label_M6.Text = "Empty";
                    break;
                case 1:
                    this.label_M6.Text = "OTP 1";
                    break;
                case 2:
                    this.label_M6.Text = "OTP 2";
                    break;
                case 3:
                    this.label_M6.Text = "OTP 3";
                    break;
                case 4:
                    this.label_M6.Text = "OTP 4";
                    break;
                default:
                    break;
            }
            */


        }

        private void button_Meta_OTP_Write_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
            SW43410_POCB_On_btn.PerformClick();
            
            OTP_check_button.PerformClick();
            f1.GB_Status_AppendText_Nextline("Before OTP Write : " + label_M1.Text + "/" + label_M2.Text + "/"
                + label_M3.Text + "/" + label_M4.Text + "/" + label_M5.Text, Color.Black);

            if (checkBox_OTP_M1.Checked) OTP_Main1_Write();
            if (checkBox_OTP_M2.Checked) OTP_Main2_Write();
            if (checkBox_OTP_M3.Checked) OTP_Main3_Write();
            if (checkBox_OTP_M4.Checked) OTP_Main4_Write();
            if (checkBox_OTP_M5.Checked) OTP_Main5_Write();
            //if (checkBox_OTP_M6.Checked) OTP_Main6_Write();

            //OTP_check_button.PerformClick();
            //f1.GB_Status_AppendText_Nextline("After OTP Write : " + label_M1.Text + "/" + label_M2.Text + "/" + label_M3.Text + "/" + label_M4.Text + "/" + label_M5.Text, Color.Black);
    
        }

        private void button_Meta_Transform_PID_String_Click(object sender, EventArgs e)
        {
            string PID = textBox_Meta_PID_Input_String.Text;
            String After = Meta_PIC_Transfer_As_ACSII(PID);
            System.Windows.MessageBox.Show(PID + " became" + After);
        }

        private string Meta_PIC_Transfer_As_ACSII(string PID)
        {
            int length = PID.Length;
            string After = string.Empty;
            for (int i = 0; i < length; i++)
            {
               char Before = PID[i];
               After += Meta_PIC_Transfer_As_ACSII_Sub(Before);
            }

            return After;
        }

        private string Meta_PIC_Transfer_As_ACSII_Sub(char Before)
        {
            Before = char.ToUpper(Before);
            switch (Before)
            {
                case '0': return " 0x30";
                case '1': return " 0x31";
                case '2': return " 0x32";
                case '3': return " 0x33";
                case '4': return " 0x34";
                case '5': return " 0x35";
                case '6': return " 0x36";
                case '7': return " 0x37";
                case '8': return " 0x38";
                case '9': return " 0x39";
                case 'A': return " 0x41";
                case 'B': return " 0x42";
                case 'C': return " 0x43";
                case 'D': return " 0x44";
                case 'E': return " 0x45";
                case 'F': return " 0x46";
                case 'G': return " 0x47";
                case 'H': return " 0x48";
                case 'I': return " 0x49";
                case 'J': return " 0x4A";
                case 'K': return " 0x4B";
                case 'L': return " 0x4C";
                case 'M': return " 0x4D";
                case 'N': return " 0x4E";
                case 'O': return " 0x4F";
                case 'P': return " 0x50";
                case 'Q': return " 0x51";
                case 'R': return " 0x52";
                case 'S': return " 0x53";
                case 'T': return " 0x54";
                case 'U': return " 0x55";
                case 'V': return " 0x56";
                case 'W': return " 0x57";
                case 'X': return " 0x58";
                default: return "Invalid_Input";
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr ptr = Get_Dll_Information();
            string Message = Marshal.PtrToStringAnsi(ptr);
            System.Windows.MessageBox.Show(Message);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            IntPtr Wx = new IntPtr();
            IntPtr Wy = new IntPtr();
            IntPtr Rx = new IntPtr();
            IntPtr Ry = new IntPtr();
            IntPtr Gx = new IntPtr();
            IntPtr Gy = new IntPtr();
            IntPtr Bx = new IntPtr();
            IntPtr By = new IntPtr();

            Meta_WRGB_Mipi_Transfer(0.2987, 0.3125, 0.6837, 0.3201, 0.2357, 0.7250, 0.1388, 0.0599,0.3127,0.3290,0.6830,0.3220,0.2530,0.7100,0.1370,0.0550, ref Wx, ref Wy, ref Rx, ref Ry, ref Gx, ref Gy, ref Bx, ref By);

            f1.GB_Status_AppendText_Nextline("Wx : " + Marshal.PtrToStringAnsi(Wx), Color.Black);
            f1.GB_Status_AppendText_Nextline("Wy : " + Marshal.PtrToStringAnsi(Wy), Color.Black);
            f1.GB_Status_AppendText_Nextline("Rx : " + Marshal.PtrToStringAnsi(Rx), Color.Red);
            f1.GB_Status_AppendText_Nextline("Ry : " + Marshal.PtrToStringAnsi(Ry), Color.Red);
            f1.GB_Status_AppendText_Nextline("Gx : " + Marshal.PtrToStringAnsi(Gx), Color.Green);
            f1.GB_Status_AppendText_Nextline("Gy : " + Marshal.PtrToStringAnsi(Gy), Color.Green);
            f1.GB_Status_AppendText_Nextline("Bx : " + Marshal.PtrToStringAnsi(Bx), Color.Blue);
            f1.GB_Status_AppendText_Nextline("By : " + Marshal.PtrToStringAnsi(By), Color.Blue);
            
        }

        private void Red_Data_Range_Check_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode.getInstance().Show();
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");


            Get_All_Band_Gray_Gamma(All_band_gray_Gamma); //Get All_band_gray_Gamma[12,8] from Engineering Sheet
            Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_form_engineer.Form_Hide();

            RGB Temp_Gamma = new RGB();

            button_Read_DBV_Setting.PerformClick();//Read DBV for each Band
            Meta_DBV_Setting(Selected_Band_Index);//Band Setting
            Meta_Pattern_Setting(Selected_Gray_Index);//Pattern Setting

           
            for (int i = 0; i <= 511; i++)
            {
                Temp_Gamma.Set_Value(i, 0, 0);
                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Temp_Gamma, Selected_Band_Index, Selected_Gray_Index);
                Thread.Sleep(20);
                f1.Measure_Indicate_Gray(i);
                f1.GB_Status_AppendText_Nextline("Band/Gray Gamma : " + Selected_Band_Index.ToString() + "/" + Selected_Gray_Index.ToString() + " (" + i.ToString() + ",0,0)" , Color.Red);

                if (Optic_Compensation_Stop) break;
            }
        }

        private void button_MLPIS_Access_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0x55 #MLPIS Access", Color.Purple);
        }

        private void Green_Data_Range_Check_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode.getInstance().Show();
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");


            Get_All_Band_Gray_Gamma(All_band_gray_Gamma); //Get All_band_gray_Gamma[12,8] from Engineering Sheet
            Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_form_engineer.Form_Hide();

            RGB Temp_Gamma = new RGB();

            button_Read_DBV_Setting.PerformClick();//Read DBV for each Band
            Meta_DBV_Setting(Selected_Band_Index);//Band Setting
            Meta_Pattern_Setting(Selected_Gray_Index);//Pattern Setting


            for (int i = 0; i <= 511; i++)
            {
                Temp_Gamma.Set_Value(0, i, 0);
                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Temp_Gamma, Selected_Band_Index, Selected_Gray_Index);
                Thread.Sleep(20);
                f1.Measure_Indicate_Gray(i);
                f1.GB_Status_AppendText_Nextline("Band/Gray Gamma : " + Selected_Band_Index.ToString() + "/" + Selected_Gray_Index.ToString() + " (0," + i.ToString() + ",0)", Color.Red);

                if (Optic_Compensation_Stop) break;
            }
        }

        private void Blue_Data_Range_Check_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode.getInstance().Show();
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");


            Get_All_Band_Gray_Gamma(All_band_gray_Gamma); //Get All_band_gray_Gamma[12,8] from Engineering Sheet
            Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            Meta_form_engineer.Form_Hide();

            RGB Temp_Gamma = new RGB();

            button_Read_DBV_Setting.PerformClick();//Read DBV for each Band
            Meta_DBV_Setting(Selected_Band_Index);//Band Setting
            Meta_Pattern_Setting(Selected_Gray_Index);//Pattern Setting


            for (int i = 0; i <= 511; i++)
            {
                Temp_Gamma.Set_Value(0, 0, i);
                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Temp_Gamma, Selected_Band_Index, Selected_Gray_Index);
                Thread.Sleep(20);
                f1.Measure_Indicate_Gray(i);
                f1.GB_Status_AppendText_Nextline("Band/Gray Gamma : " + Selected_Band_Index.ToString() + "/" + Selected_Gray_Index.ToString() + " (0,0," + i.ToString() + ")", Color.Red);

                if (Optic_Compensation_Stop) break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double u = Convert.ToDouble(textBox1.Text);
            double v = Convert.ToDouble(textBox2.Text);

            double x = (9 * u) / (6 * u - 16 * v + 12);
            double y = (4 * v) / (6 * u - 16 * v + 12);

            textBox3.Text = x.ToString().Substring(0, 6);
            textBox4.Text = y.ToString().Substring(0, 6);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(textBox5.Text);
            double y = Convert.ToDouble(textBox6.Text);

            double u = (4 * x) / (-2 * x + 12 * y + 3);
            double v = (9 * y) / (-2 * x + 12 * y + 3);

            textBox7.Text = u.ToString().Substring(0, 6);
            textBox8.Text = v.ToString().Substring(0, 6);
        }

        private void button_Read_ELVSS_Vinit_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            //Normal ELVSS Read
            f1.OTP_Read(13, "CD");
            string[] hex_ELVSS = new string[10];
            double[] dec_ELVSS = new double[10];
            double[] ELVSS = new double[10];
            for (int i = 3; i <= 12; i++)
            {
                hex_ELVSS[i - 3] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS[i - 3] = Convert.ToDouble(Convert.ToInt16(hex_ELVSS[i - 3], 16));
                ELVSS[i - 3] = ((dec_ELVSS[i - 3] - 30) / 10.0) - 3.5;
            }
            textBox_ELVSS_B0.Text = ELVSS[0].ToString() + "v (" + hex_ELVSS[0] + "h)";
            textBox_ELVSS_B1.Text = ELVSS[1].ToString() + "v (" + hex_ELVSS[1] + "h)";
            textBox_ELVSS_B2.Text = ELVSS[2].ToString() + "v (" + hex_ELVSS[2] + "h)";
            textBox_ELVSS_B3.Text = ELVSS[3].ToString() + "v (" + hex_ELVSS[3] + "h)";
            textBox_ELVSS_B4.Text = ELVSS[4].ToString() + "v (" + hex_ELVSS[4] + "h)";
            textBox_ELVSS_B5.Text = ELVSS[5].ToString() + "v (" + hex_ELVSS[5] + "h)";
            textBox_ELVSS_B6.Text = ELVSS[6].ToString() + "v (" + hex_ELVSS[6] + "h)";
            textBox_ELVSS_B7.Text = ELVSS[7].ToString() + "v (" + hex_ELVSS[7] + "h)";
            textBox_ELVSS_B8.Text = ELVSS[8].ToString() + "v (" + hex_ELVSS[8] + "h)";
            textBox_ELVSS_B9.Text = ELVSS[9].ToString() + "v (" + hex_ELVSS[9] + "h)";

            //Vinit Read
            f1.OTP_Read(16, "C2");
            string hex_Vinit = f1.dataGridView1.Rows[15].Cells[1].Value.ToString();
            double dec_Vinit = Convert.ToDouble(Convert.ToInt16(hex_Vinit, 16)& 0x3F);
            double Vinit = (-dec_Vinit / 10.0);
            textBox_Vinit.Text = Vinit.ToString() + "v ";
        }

        private void button_Gamma_Down_Real_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            Meta_Engineer_Mornitoring_Mode.getInstance().Show();
            System.Windows.Forms.Application.DoEvents();
            Get_All_Band_Gray_Gamma(All_band_gray_Gamma);
            for (int band = 0; band < 10; band++)
            {
                Gamma.Set_Value(All_band_gray_Gamma[band, 0].int_R, All_band_gray_Gamma[band, 0].int_G, All_band_gray_Gamma[band, 0].int_B);
                string temp = Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, 0);
                f1.GB_Status_AppendText_Nextline(temp, Color.Blue);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
           //Anything want to test
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Meta_Engineer_Mornitoring_Mode Meta_form_engineer = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];

            /*
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");//#MCAP - Manufactuer ACCESS

            //Add on 191028 (About RGB_Vdata)
            f1.GB_Status_AppendText_Nextline("*Assumption : ELVDD = 4.6v , DDVDH = 6.7v , GREF_SEL = 1 , GA_INV = 1,VG4_EN =1 , AVAMODE = 0", Color.Red);
            //Get ELVDD - FV1_LVL[5:0],ELVDD + VCI1_LVL[5:0]
            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            double ELVDD = 4.6;

            string FV1 = D0_Hex[18];
            int dec_FV1 = Convert.ToInt16(FV1, 16);
            if (dec_FV1 >= 42) dec_FV1 = 42;
            double E7 = ELVDD - (0.2 + (dec_FV1 * 0.1)); //ELVDD - FV1_LVL[5:0]

            string VCI1 = D0_Hex[16];
            int dec_VCI1 = Convert.ToInt16(VCI1, 16);
            if (dec_VCI1 >= 42) dec_VCI1 = 42;
            double F7 = ELVDD + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]

            //Get VREG1_REF818[5:0],VREG1_REF614[5:0],VREG1_REF409[5:0],VREG1_REF205[5:0]
            //Vreg1_REF818 = F7+(E7-F7)*((222.5+0.5*HEX2DEC(VREG1_REF818_Hex))/254))
            //VREG1_REF614 = F7+(E7-F7)*((206.5+0.5*HEX2DEC(VREG1_REF614_Hex))/254))
            //VREG1_REF409 = F7+(E7-F7)*((182.5+0.5*HEX2DEC(VREG1_REF409_Hex))/254))
            //VREG1_REF205 = F7+(E7-F7)*((154.5+0.5*HEX2DEC(VREG1_REF205_Hex))/254))
            int Dec_VREG1_REF818 = Convert.ToInt16(D0_Hex[1], 16) & 0x3F;
            int Dec_VREG1_REF614 = Convert.ToInt16(D0_Hex[2], 16) & 0x3F;
            int Dec_VREG1_REF409 = Convert.ToInt16(D0_Hex[3], 16) & 0x3F;
            int Dec_VREG1_REF205 = Convert.ToInt16(D0_Hex[4], 16) & 0x3F;

            double VREG1_REF818_volt = F7 + (E7 - F7) * ((222.5 + 0.5 * Dec_VREG1_REF818) / 254);
            double VREG1_REF614_volt = F7 + (E7 - F7) * ((206.5 + 0.5 * Dec_VREG1_REF614) / 254);
            double VREG1_REF409_volt = F7 + (E7 - F7) * ((182.5 + 0.5 * Dec_VREG1_REF409) / 254);
            double VREG1_REF205_volt = F7 + (E7 - F7) * ((154.5 + 0.5 * Dec_VREG1_REF205) / 254);

            f1.GB_Status_AppendText_Nextline("-----------By C#----------", Color.Blue);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) - FV1_LVL(v) = " + E7.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF818(v) = " + VREG1_REF818_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF614(v) = " + VREG1_REF614_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF409(v) = " + VREG1_REF409_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF205(v) = " + VREG1_REF205_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) + VCI1_LVL(v) = " + F7.ToString(), Color.Blue);


            f1.GB_Status_AppendText_Nextline("-----------Inialize----------", Color.Black);
            E7 = 0;
            F7 = 0;
            VREG1_REF818_volt = 0;
            VREG1_REF614_volt = 0;
            VREG1_REF409_volt = 0;
            VREG1_REF205_volt = 0;
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) - FV1_LVL(v) = " + E7.ToString(), Color.Black);
            f1.GB_Status_AppendText_Nextline("VREG1_REF818(v) = " + VREG1_REF818_volt.ToString(), Color.Black);
            f1.GB_Status_AppendText_Nextline("VREG1_REF614(v) = " + VREG1_REF614_volt.ToString(), Color.Black);
            f1.GB_Status_AppendText_Nextline("VREG1_REF409(v) = " + VREG1_REF409_volt.ToString(), Color.Black);
            f1.GB_Status_AppendText_Nextline("VREG1_REF205(v) = " + VREG1_REF205_volt.ToString(), Color.Black);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) + VCI1_LVL(v) = " + F7.ToString(), Color.Black);


            f1.GB_Status_AppendText_Nextline("-----------By Dll----------", Color.Red);
            E7 = Get_E7(ELVDD,dec_FV1);
            F7 = Get_F7(ELVDD,dec_VCI1);
            VREG1_REF818_volt = Get_VREG1_REF818_volt(E7, F7, Dec_VREG1_REF818);
            VREG1_REF614_volt = Get_VREG1_REF614_volt(E7, F7, Dec_VREG1_REF614);
            VREG1_REF409_volt = Get_VREG1_REF409_volt(E7, F7, Dec_VREG1_REF409);
            VREG1_REF205_volt = Get_VREG1_REF205_volt(E7, F7, Dec_VREG1_REF205);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) - FV1_LVL(v) = " + E7.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("VREG1_REF818(v) = " + VREG1_REF818_volt.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("VREG1_REF614(v) = " + VREG1_REF614_volt.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("VREG1_REF409(v) = " + VREG1_REF409_volt.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("VREG1_REF205(v) = " + VREG1_REF205_volt.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) + VCI1_LVL(v) = " + F7.ToString(), Color.Red);
            */

            f1.GB_Status_AppendText_Nextline("-----------By C#----------", Color.Blue);
            //Gamma_Voltage[b, g].double_G = Meta_Engineering.Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[b], Gamma_Voltage[b, g - 1].double_G, Temp_Gamma[b, g].int_G);
            //Gamma_Voltage[b, g].double_G = Meta_Engineering.Get_AM2_Voltage(F7, Vreg1_Voltage[b], Temp_Gamma[b, g].int_G);
            //Rev_Gamma.int_G = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Calculated_Vdata[band, gray].double_G);
            //Rev_Gamma.int_G = Meta_Engineering.Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_G, Calculated_Vdata[band, gray].double_G);

            double A = Meta_form_engineer.Get_Normal_Gamma_Voltage(4.5123, 5.2123, 312);
            double B = Meta_form_engineer.Get_AM2_Voltage(6.0, 4.345,417);
            double C = Meta_form_engineer.Get_Gamma_From_AM2_Voltage(6.0, 4.543, 3.2);
            double D = Meta_form_engineer.Get_Gamma_From_Normal_Voltage(4.129,5.2653,4.5);


            f1.GB_Status_AppendText_Nextline("A = " + A.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("B = " + B.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("C = " + C.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("D = " + D.ToString(), Color.Blue);

            f1.GB_Status_AppendText_Nextline("-----------Inialize----------", Color.Black);
            A = 0;
            B = 0;
            C = 0;
            D = 0;
            f1.GB_Status_AppendText_Nextline("A = " + A.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("B = " + B.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("C = " + C.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("D = " + D.ToString(), Color.Blue);

            f1.GB_Status_AppendText_Nextline("-----------By Dll----------", Color.Red);
            A = Meta_Get_Normal_Gamma_Voltage(4.5123, 5.2123, 312);
            B = Meta_Get_AM2_Voltage(6.0, 4.345,417);
            C = Meta_Get_Gamma_From_AM2_Voltage(6.0, 4.543, 3.2);
            D = Meta_Get_Gamma_From_Normal_Voltage(4.129, 5.2653, 4.5);
            
            f1.GB_Status_AppendText_Nextline("A = " + A.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("B = " + B.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("C = " + C.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("D = " + D.ToString(), Color.Blue);
        }

        private void HBM_ELVSS_Margin_Test()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            string[] Hex_ELVSS = new string[2];
            string mipi_cmd = "mipi.write 0x39 0xCD 0x00 0x00 0x00";
            for (double ELVSS = -4.5; ELVSS < -1.4; ELVSS += 0.1)
            {
                SET_ELVSS(0, ELVSS, Hex_ELVSS);//Hex_ELVSS[band = 0] <-- ELVSS
                f1.IPC_Quick_Send(mipi_cmd + " 0x" + Hex_ELVSS[0]); //send Hex_ELVSS[0]            
                Thread.Sleep(50);
                f1.Measure_Indicate_Gray(ELVSS);
            }
        }

        private void groupBox17_Enter(object sender, EventArgs e)
        {

        }

        private void Clear_AM0_Read_Params()
        {
            textBox_FV1.Text = "";
            textBox_VCI1.Text = "";
            textBox_AM0_Resolution.Text = "";
            textBox_R_AM0.Text = "";
            textBox_G_AM0.Text = "";
            textBox_B_AM0.Text = "";
        }

        private void button_Read_AM0_VREF2_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");

            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            double ELVDD = 4.6;

            string FV1 = D0_Hex[18];
            int dec_FV1 = Convert.ToInt16(FV1,16);
            if (dec_FV1 >= 42) dec_FV1 = 42;
            double K12 = ELVDD - (0.2 + (dec_FV1 * 0.1)); //ELVDD - FV1_LVL[5:0]
            textBox_FV1.Text = K12.ToString() + "v (" + FV1 + "h)";

            string VCI1 = D0_Hex[16];
            int dec_VCI1 = Convert.ToInt16(VCI1, 16);
            if (dec_VCI1 >= 42) dec_VCI1 = 42;
            double F12 = ELVDD + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]
            textBox_VCI1.Text = F12.ToString() + "v (" + VCI1 + "h)";

            double AM0_Resolution = (F12 - K12) / 700;
            textBox_AM0_Resolution.Text = AM0_Resolution.ToString();

            string[] D1_Hex = new string[19];
            f1.OTP_Read(19, "D1");
            for (int i = 0; i < 19; i++) D1_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            string AM0_Hex_R = f1.dataGridView1.Rows[16].Cells[1].Value.ToString();
            string AM0_Hex_G = f1.dataGridView1.Rows[17].Cells[1].Value.ToString();
            string AM0_Hex_B = f1.dataGridView1.Rows[18].Cells[1].Value.ToString();

            double AM0_R = F12 - AM0_Resolution * Convert.ToInt16(AM0_Hex_R, 16);
            double AM0_G = F12 - AM0_Resolution * Convert.ToInt16(AM0_Hex_G, 16);
            double AM0_B = F12 - AM0_Resolution * Convert.ToInt16(AM0_Hex_B, 16);

            textBox_R_AM0.Text = AM0_R.ToString();
            textBox_G_AM0.Text = AM0_G.ToString();
            textBox_B_AM0.Text = AM0_B.ToString();

            //Use Dll
            double AM0_Dll_Resolution = Get_Meta_SW43417_AM0_Resolution(FV1, VCI1);

            f1.GB_Status_AppendText_Nextline("AM0_Resolution / AM0_Dll_Resolution : " + AM0_Resolution.ToString() + "/" + AM0_Dll_Resolution.ToString(), Color.Black);
        }

        private void button_HBM_ELVSS_Margin_Test_Click(object sender, EventArgs e)
        {
            HBM_ELVSS_Margin_Test();
        }

        private void button_R_AM0_Test_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            Meta_DBV_Setting(0);//HBM
            f1.IPC_Quick_Send("image.black");//Black Pattern

            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            string FV1 = D0_Hex[18];

            string VCI1 = D0_Hex[16];
            int dec_VCI1 = Convert.ToInt16(VCI1, 16);
            if (dec_VCI1 >= 42) dec_VCI1 = 42;
            double F12 = 4.6 + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]


            double AM0_Dll_Resolution = Get_Meta_SW43417_AM0_Resolution(FV1, VCI1);

            string[] D1_Hex = new string[19];
            f1.OTP_Read(19, "D1");
            for (int i = 0; i < 19; i++) D1_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            double AM0_R, AM0_G, AM0_B;

            for (int i = 0; i < 128; i++)
            {
                D1_Hex[16] = i.ToString("X2"); //R_AM0
                D1_Hex[17] = "00"; //G
                D1_Hex[18] = "00"; //B
                f1.Long_Packet_CMD_Send(19, "D1", D1_Hex);
                Thread.Sleep(50);
                f1.Measure_Indicate_Gray(i);

                //Indicate AM0_R/AM0_G/AM0_B
                AM0_R = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[16], 16);
                AM0_G = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[17], 16);
                AM0_B = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[18], 16);
                textBox_R_AM0.Text = AM0_R.ToString();
                textBox_G_AM0.Text = AM0_G.ToString();
                textBox_B_AM0.Text = AM0_B.ToString();
                f1.GB_Status_AppendText_Nextline("AM0_R(i) : " + textBox_R_AM0.Text + "(" + i.ToString() + ")",Color.Red);
            }

        }

        private void button_G_AM0_Test_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            Meta_DBV_Setting(0);//HBM
            f1.IPC_Quick_Send("image.black");//Black Pattern

            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            string FV1 = D0_Hex[18];

            string VCI1 = D0_Hex[16];
            int dec_VCI1 = Convert.ToInt16(VCI1, 16);
            if (dec_VCI1 >= 42) dec_VCI1 = 42;
            double F12 = 4.6 + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]


            double AM0_Dll_Resolution = Get_Meta_SW43417_AM0_Resolution(FV1, VCI1);

            string[] D1_Hex = new string[19];
            f1.OTP_Read(19, "D1");
            for (int i = 0; i < 19; i++) D1_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            double AM0_R, AM0_G, AM0_B;

            for (int i = 0; i < 128; i++)
            {
                D1_Hex[16] = "00"; //R_AM0
                D1_Hex[17] = i.ToString("X2"); //G
                D1_Hex[18] = "00"; //B
                f1.Long_Packet_CMD_Send(19, "D1", D1_Hex);
                Thread.Sleep(50);
                f1.Measure_Indicate_Gray(i);

                //Indicate AM0_R/AM0_G/AM0_B
                AM0_R = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[16], 16);
                AM0_G = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[17], 16);
                AM0_B = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[18], 16);
                textBox_R_AM0.Text = AM0_R.ToString();
                textBox_G_AM0.Text = AM0_G.ToString();
                textBox_B_AM0.Text = AM0_B.ToString();
                f1.GB_Status_AppendText_Nextline("AM0_G(i) : " + textBox_G_AM0.Text + "(" + i.ToString() + ")", Color.Green);
            }
        }

        private void button_B_AM0_Test_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            Meta_DBV_Setting(0);//HBM
            f1.IPC_Quick_Send("image.black");//Black Pattern

            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            string FV1 = D0_Hex[18];

            string VCI1 = D0_Hex[16];
            int dec_VCI1 = Convert.ToInt16(VCI1, 16);
            if (dec_VCI1 >= 42) dec_VCI1 = 42;
            double F12 = 4.6 + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]


            double AM0_Dll_Resolution = Get_Meta_SW43417_AM0_Resolution(FV1, VCI1);

            string[] D1_Hex = new string[19];
            f1.OTP_Read(19, "D1");
            for (int i = 0; i < 19; i++) D1_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            double AM0_R, AM0_G, AM0_B;

            for (int i = 0; i < 128; i++)
            {
                D1_Hex[16] = "00"; //R_AM0
                D1_Hex[17] = "00"; //G
                D1_Hex[18] = i.ToString("X2"); //B
                f1.Long_Packet_CMD_Send(19, "D1", D1_Hex);
                Thread.Sleep(50);
                f1.Measure_Indicate_Gray(i);

                //Indicate AM0_R/AM0_G/AM0_B
                AM0_R = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[16], 16);
                AM0_G = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[17], 16);
                AM0_B = F12 - AM0_Dll_Resolution * Convert.ToInt16(D1_Hex[18], 16);
                textBox_R_AM0.Text = AM0_R.ToString();
                textBox_G_AM0.Text = AM0_G.ToString();
                textBox_B_AM0.Text = AM0_B.ToString();
                f1.GB_Status_AppendText_Nextline("AM0_B(i) : " + textBox_B_AM0.Text + "(" + i.ToString() + ")", Color.Blue);
            }
        }

        private bool Black_Compensation()
        {
            Clear_AM0_Read_Params();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");
            Meta_DBV_Setting(0);//HBM
            f1.IPC_Quick_Send("image.black");//Black Pattern
            Thread.Sleep(200);

            //Get D0[19](FV1 , VCI1)
            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            string FV1 = D0_Hex[18];
            string VCI1 = D0_Hex[16];

            //Get D1[19](AM0_R, AM0_G, AM0_B)
            string[] D1_Hex = new string[19];
            f1.OTP_Read(19, "D1");
            for (int i = 0; i < 19; i++) D1_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            
            //Get Resolution (By using Dll)
            double AM0_Dll_Resolution = Get_Meta_SW43417_AM0_Resolution(FV1, VCI1);

            //Get Margin
            double Margin = Convert.ToDouble(textBox_AM0_Margin.Text);

            //int M_R = (int)Margin/Resolution
            int M_R = Convert.ToUInt16(Margin/AM0_Dll_Resolution);

            //Get Black Limit Lv
            double Black_Limit_Lv = Convert.ToDouble(textBox_Black_Limit_Lv.Text);

            //Red
            string Red_AM0_Hex = "XX";//TBD (Final Red AM0 Hex)
            //D1_Hex[16] = "00"; //Red AM0
            D1_Hex[17] = "00"; //Green AM0
            D1_Hex[18] = "00"; //Blue AM0
            for (int AM0 = 127; AM0 > 0; AM0--)
            {
                f1.GB_Status_AppendText_Nextline("Current_Red_AM0(dec) : " + AM0.ToString(), Color.Black);
                if ((AM0 - M_R) < 0) 
                {
                    f1.GB_Status_AppendText_Nextline("Red Black Margin NG , (AM0 - M_R) : " + (AM0 - M_R).ToString(), Color.Red);
                    return false;//Red Black Margin NG
                }
                else
                {
                    D1_Hex[16] = AM0.ToString("X2"); //Red AM0
                    f1.Long_Packet_CMD_Send(19, "D1", D1_Hex);//Set AM0
                    Thread.Sleep(50);
                    f1.Measure_Indicate(AM0, ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure

                    if (Measure.double_Lv < Black_Limit_Lv)
                    {
                        Red_AM0_Hex = (AM0 - M_R).ToString("X2");//Red Black Compensation Ok
                        f1.GB_Status_AppendText_Nextline("Final_Red_AM0(Hex) : " + Red_AM0_Hex + "h", Color.Red);
                        break;
                    }
                }
            }

            //Green
            string Green_AM0_Hex = "XX";//TBD (Final Green AM0 Hex)
            D1_Hex[16] = "00"; //Red AM0
            //D1_Hex[17] = "00"; //Green AM0
            D1_Hex[18] = "00"; //Blue AM0
            for (int AM0 = 127; AM0 > 0; AM0--)
            {
                f1.GB_Status_AppendText_Nextline("Current_Green_AM0(dec) : " + AM0.ToString(), Color.Black);
                if ((AM0 - M_R) < 0)
                {
                    f1.GB_Status_AppendText_Nextline("Green Black Margin NG , (AM0 - M_R) : " + (AM0 - M_R).ToString(), Color.Green);
                    return false;//Green Black Margin NG
                }
                else
                {
                    D1_Hex[17] = AM0.ToString("X2"); //Green AM0
                    f1.Long_Packet_CMD_Send(19, "D1", D1_Hex);//Set AM0
                    Thread.Sleep(50);
                    f1.Measure_Indicate(AM0, ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure

                    if (Measure.double_Lv < Black_Limit_Lv)
                    {  
                        Green_AM0_Hex = (AM0 - M_R).ToString("X2");//Red Black Compensation Ok
                        f1.GB_Status_AppendText_Nextline("Final_Green_AM0(Hex) : " + Green_AM0_Hex + "h", Color.Green);
                        break;
                    }
                }
            }

            //Blue
            string Blue_AM0_Hex = "XX";//TBD (Final Green AM0 Hex)
            D1_Hex[16] = "00"; //Red AM0
            D1_Hex[17] = "00"; //Green AM0
            //D1_Hex[18] = "00"; //Blue AM0
            for (int AM0 = 127; AM0 > 0; AM0--)
            {
                f1.GB_Status_AppendText_Nextline("Current_Blue_AM0(dec) : " + AM0.ToString(), Color.Black);
                if ((AM0 - M_R) < 0)
                {
                    f1.GB_Status_AppendText_Nextline("Blue Black Margin NG , (AM0 - M_R) : " + (AM0 - M_R).ToString(), Color.Blue);
                    return false;//Green Black Margin NG
                }
                else
                {
                    D1_Hex[18] = AM0.ToString("X2"); //Blue AM0
                    f1.Long_Packet_CMD_Send(19, "D1", D1_Hex);//Set AM0
                    Thread.Sleep(50);
                    f1.Measure_Indicate(AM0, ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure

                    if (Measure.double_Lv < Black_Limit_Lv)
                    {
                        Blue_AM0_Hex = (AM0 - M_R).ToString("X2");//Red Black Compensation Ok
                        f1.GB_Status_AppendText_Nextline("Final_Blue_AM0(Hex) : " + Blue_AM0_Hex + "h", Color.Blue);
                        break;
                    }
                }
            }

            D1_Hex[16] = Red_AM0_Hex; //Red AM0
            D1_Hex[17] = Green_AM0_Hex; //Green AM0
            D1_Hex[18] = Blue_AM0_Hex; //Blue AM0
            f1.Long_Packet_CMD_Send(19, "D1", D1_Hex);//Set AM0

            button_Read_AM0_VREF2.PerformClick();//Read Final Values
            return true;
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_Apply_FX_HBM_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Apply_FX_HBM_RGB.Checked)
            {
                //checkBox_Apply_FX_HBM.Checked = false;
                checkBox_Apply_FLviX_HBM.Checked = false;
                //checkBox_Apply_FX_Previous_Band_Vreg1.Checked = false;
            }
        }


        private void checkBox_Apply_FLviX_HBM_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Apply_FLviX_HBM.Checked)
            {
                checkBox_Apply_FX_HBM_RGB.Checked = false;
                //checkBox_Apply_FLviX_HBM.Checked = false;
                checkBox_Apply_FX_Previous_Band_Vreg1.Checked = false;
            }
        }

        private void checkBox_Apply_FX_Previous_Band_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Apply_FX_Previous_Band_Vreg1.Checked)
            {

                //checkBox_Apply_FX_HBM_RGB.Checked = false;
                checkBox_Apply_FLviX_HBM.Checked = false;
                //checkBox_Apply_FX_Previous_Band.Checked = false;
            }
        }


        private void Dll_C_Sharp_Meta_Get_Normal_Initial_Vreg1_For_Band2_4points(ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, bool[] Selected_Band, int[] Band0_Gamma_Red, int[] Band0_Gamma_Green, int[] Band0_Gamma_Blue, int[] Band1_Gamma_Red, int[] Band1_Gamma_Green, int[] Band1_Gamma_Blue, int band, double band_Target_Lv, int Band0_Vreg1_Dec, int Band1_Vreg1_Dec
            , double[] Band0_Target_Lv, double[] Band1_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            if (band >= 1 && Selected_Band[band] == true)
            {
                //Sorting
                double[] Band0_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band1_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band01_Gamma_Red_Voltage = new double[16];
                double[] Band01_Gamma_Green_Voltage = new double[16];
                double[] Band01_Gamma_Blue_Voltage = new double[16];

                double[] Band01_Target_Lv = new double[16];

                for (int i = 0; i < 8; i++)
                {
                    Band01_Gamma_Red_Voltage[i] = Band0_Gamma_Red_Voltage[i];
                    Band01_Gamma_Red_Voltage[i + 8] = Band1_Gamma_Red_Voltage[i];

                    Band01_Gamma_Green_Voltage[i] = Band0_Gamma_Green_Voltage[i];
                    Band01_Gamma_Green_Voltage[i + 8] = Band1_Gamma_Green_Voltage[i];

                    Band01_Gamma_Blue_Voltage[i] = Band0_Gamma_Blue_Voltage[i];
                    Band01_Gamma_Blue_Voltage[i + 8] = Band1_Gamma_Blue_Voltage[i];

                    Band01_Target_Lv[i] = Band0_Target_Lv[i];
                    Band01_Target_Lv[i + 8] = Band1_Target_Lv[i];
                }

                Array.Sort<double>(Band01_Gamma_Red_Voltage);
                Array.Sort<double>(Band01_Gamma_Green_Voltage);
                Array.Sort<double>(Band01_Gamma_Blue_Voltage);

                Array.Sort<double>(Band01_Target_Lv);
                Array.Reverse(Band01_Target_Lv);

                //Rearrange for 2ea 8points
                double[] First_4points_Gamma_Red_Voltage = new double[4];
                double[] First_4points_Gamma_Green_Voltage = new double[4];
                double[] First_4points_Gamma_Blue_Voltage = new double[4];
                double[] First_4points_Target_Lv = new double[4];



                SJH_Matrix M = new SJH_Matrix();
                double[][] First_4points_8points_A_R = M.MatrixCreate(4, 4);
                double[][] First_4points_8points_A_G = M.MatrixCreate(4, 4);
                double[][] First_4points_8points_A_B = M.MatrixCreate(4, 4);

                double[][] Second_4points_8points_A_R = M.MatrixCreate(4, 4);//Not used In Vreg1 Mode
                double[][] Second_4points_8points_A_G = M.MatrixCreate(4, 4);//Not used In Vreg1 Mode
                double[][] Second_4points_8points_A_B = M.MatrixCreate(4, 4);//Not used In Vreg1 Mode

                //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= 3; i++)
                {
                    First_4points_Target_Lv[i] = Band01_Target_Lv[i];
                    count = 0;
                    for (int j = 3; j >= 0; j--)
                    {
                        First_4points_8points_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i], j);
                        First_4points_8points_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i], j);
                        First_4points_8points_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i], j);
                        count++;
                    }
                }

                //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
                double[][] First_4points_8points_Inv_A_R = M.MatrixCreate(4, 4);
                double[][] First_4points_8points_Inv_A_G = M.MatrixCreate(4, 4);
                double[][] First_4points_8points_Inv_A_B = M.MatrixCreate(4, 4);
                double[] First_4points_8points_C_R = new double[4];
                double[] First_4points_8points_C_G = new double[4];
                double[] First_4points_8points_C_B = new double[4];
                First_4points_8points_Inv_A_R = M.MatrixInverse(First_4points_8points_A_R);
                First_4points_8points_Inv_A_G = M.MatrixInverse(First_4points_8points_A_G);
                First_4points_8points_Inv_A_B = M.MatrixInverse(First_4points_8points_A_B);
                First_4points_8points_C_R = M.Matrix_Multiply(First_4points_8points_Inv_A_R, First_4points_Target_Lv);
                First_4points_8points_C_G = M.Matrix_Multiply(First_4points_8points_Inv_A_G, First_4points_Target_Lv);
                First_4points_8points_C_B = M.Matrix_Multiply(First_4points_8points_Inv_A_B, First_4points_Target_Lv);

                for (int i = 0; i <= 3; i++) f1.GB_Status_AppendText_Nextline(i.ToString() + ")Band01_Gamma_Green_Voltage[i]/First_4points_Target_Lv[i] : " + Band01_Gamma_Green_Voltage[i].ToString() + "/" + First_4points_Target_Lv[i].ToString(), Color.Blue);

                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                string First_temp = "";
                for (int i = 0; i <= 3; i++) First_temp += (First_4points_8points_C_G[i].ToString() + " ");
                f1.GB_Status_AppendText_Nextline("First(high 4ea points) : C_G[0] C_G[1] C_G[2] C_G[3] = " + First_temp, Color.Red);//Just For Debug, it can be deleted later (191113)

                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;
                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                double Band1_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Band1_Vreg1_Dec, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Actual_Previous_Vdata_Red = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Blue[0]);

                //Red
                //for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 3; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_4points_8points_C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0)
                    {
                        Calculated_Vdata_Red = Vdata;
                        break;
                    }
                }

                //Green
                //for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 3; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_4points_8points_C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0)
                    {
                        Calculated_Vdata_Green = Vdata;
                        break;
                    }
                }

                //Blue
                //for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.001
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 3; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_4points_8points_C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
                    {
                        Calculated_Vdata_Blue = Vdata;
                        break;
                    }
                }

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Band1_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                Vreg1_Dec_Init = Meta_Engineering.Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                //Got the Vreg1 
                //Need to get Gamma_R/B
                double Current_Band_AM1_RGB_Voltage = F7 + (Vreg1_voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_B = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Blue);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }


        //To Be Tested
        private void Dll_C_Sharp_Meta_Get_Normal_Initial_Vreg1_For_Band2_3points_Select_Adjacent_Fx(ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, bool[] Selected_Band, int[] Band0_Gamma_Red, int[] Band0_Gamma_Green, int[] Band0_Gamma_Blue, int[] Band1_Gamma_Red, int[] Band1_Gamma_Green, int[] Band1_Gamma_Blue, int band, double band_Target_Lv, int Band0_Vreg1_Dec, int Band1_Vreg1_Dec
            , double[] Band0_Target_Lv, double[] Band1_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            SJH_Matrix M = new SJH_Matrix();
            if (band >= 1 && Selected_Band[band] == true)
            {
                //Sorting
                double[] Band0_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band1_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band01_Gamma_Red_Voltage = new double[16];
                double[] Band01_Gamma_Green_Voltage = new double[16];
                double[] Band01_Gamma_Blue_Voltage = new double[16];

                double[] Band01_Target_Lv = new double[16];

                for (int i = 0; i < 8; i++)
                {
                    Band01_Gamma_Red_Voltage[i] = Band0_Gamma_Red_Voltage[i];
                    Band01_Gamma_Red_Voltage[i + 8] = Band1_Gamma_Red_Voltage[i];

                    Band01_Gamma_Green_Voltage[i] = Band0_Gamma_Green_Voltage[i];
                    Band01_Gamma_Green_Voltage[i + 8] = Band1_Gamma_Green_Voltage[i];

                    Band01_Gamma_Blue_Voltage[i] = Band0_Gamma_Blue_Voltage[i];
                    Band01_Gamma_Blue_Voltage[i + 8] = Band1_Gamma_Blue_Voltage[i];

                    Band01_Target_Lv[i] = Band0_Target_Lv[i];
                    Band01_Target_Lv[i + 8] = Band1_Target_Lv[i];
                }

                Array.Sort<double>(Band01_Gamma_Red_Voltage);
                Array.Sort<double>(Band01_Gamma_Green_Voltage);
                Array.Sort<double>(Band01_Gamma_Blue_Voltage);

                Array.Sort<double>(Band01_Target_Lv);
                Array.Reverse(Band01_Target_Lv);

                //First 3points (1,2,3)
                double[] First_3points_Gamma_Red_Voltage = new double[3];
                double[] First_3points_Gamma_Green_Voltage = new double[3];
                double[] First_3points_Gamma_Blue_Voltage = new double[3];
                double[] First_3points_Target_Lv = new double[3];

                double[][] First_3points_8points_A_R = M.MatrixCreate(3, 3);
                double[][] First_3points_8points_A_G = M.MatrixCreate(3, 3);
                double[][] First_3points_8points_A_B = M.MatrixCreate(3, 3);

                //Second 3points (2,3,4)
                double[] Second_3points_Gamma_Red_Voltage = new double[3];
                double[] Second_3points_Gamma_Green_Voltage = new double[3];
                double[] Second_3points_Gamma_Blue_Voltage = new double[3];
                double[] Second_3points_Target_Lv = new double[3];

                double[][] Second_3points_8points_A_R = M.MatrixCreate(3, 3);//Not used In Vreg1 Mode
                double[][] Second_3points_8points_A_G = M.MatrixCreate(3, 3);//Not used In Vreg1 Mode
                double[][] Second_3points_8points_A_B = M.MatrixCreate(3, 3);//Not used In Vreg1 Mode


                //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
                int count = 0;

                //Get First/Second A_R/G/B and Target_LV
                for (int i = 0; i <= 2; i++)
                {
                    First_3points_Target_Lv[i] = Band01_Target_Lv[i];
                    Second_3points_Target_Lv[i] = Band01_Target_Lv[i + 1];
                    count = 0;
                    for (int j = 2; j >= 0; j--)
                    {
                        First_3points_8points_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i], j);
                        First_3points_8points_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i], j);
                        First_3points_8points_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i], j);

                        Second_3points_8points_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i + 1], j);
                        Second_3points_8points_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i + 1], j);
                        Second_3points_8points_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i + 1], j);

                        count++;
                    }
                }

                double[][] Temp_3points_8points_Inv_A_R = M.MatrixCreate(3, 3);
                double[][] Temp_3points_8points_Inv_A_G = M.MatrixCreate(3, 3);
                double[][] Temp_3points_8points_Inv_A_B = M.MatrixCreate(3, 3);

                //Get First C_R/G/B
                double[] First_3points_8points_C_R = new double[3];
                double[] First_3points_8points_C_G = new double[3];
                double[] First_3points_8points_C_B = new double[3];
                Temp_3points_8points_Inv_A_R = M.MatrixInverse(First_3points_8points_A_R);
                Temp_3points_8points_Inv_A_G = M.MatrixInverse(First_3points_8points_A_G);
                Temp_3points_8points_Inv_A_B = M.MatrixInverse(First_3points_8points_A_B);
                First_3points_8points_C_R = M.Matrix_Multiply(Temp_3points_8points_Inv_A_R, First_3points_Target_Lv);
                First_3points_8points_C_G = M.Matrix_Multiply(Temp_3points_8points_Inv_A_G, First_3points_Target_Lv);
                First_3points_8points_C_B = M.Matrix_Multiply(Temp_3points_8points_Inv_A_B, First_3points_Target_Lv);

                //Get Second C_R/G/B
                double[] Second_3points_8points_C_R = new double[3];
                double[] Second_3points_8points_C_G = new double[3];
                double[] Second_3points_8points_C_B = new double[3];
                Temp_3points_8points_Inv_A_R = M.MatrixInverse(Second_3points_8points_A_R);
                Temp_3points_8points_Inv_A_G = M.MatrixInverse(Second_3points_8points_A_G);
                Temp_3points_8points_Inv_A_B = M.MatrixInverse(Second_3points_8points_A_B);
                Second_3points_8points_C_R = M.Matrix_Multiply(Temp_3points_8points_Inv_A_R, Second_3points_Target_Lv);
                Second_3points_8points_C_G = M.Matrix_Multiply(Temp_3points_8points_Inv_A_G, Second_3points_Target_Lv);
                Second_3points_8points_C_B = M.Matrix_Multiply(Temp_3points_8points_Inv_A_B, Second_3points_Target_Lv);

                for (int i = 0; i <= 2; i++) f1.GB_Status_AppendText_Nextline(i.ToString() + ")Band01_Gamma_Green_Voltage[i]/First_3points_Target_Lv[i] : " + Band01_Gamma_Green_Voltage[i].ToString() + "/" + First_3points_Target_Lv[i].ToString(), Color.Blue);

                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                string First_temp = "";
                for (int i = 0; i <= 2; i++) First_temp += (First_3points_8points_C_G[i].ToString() + " ");
                f1.GB_Status_AppendText_Nextline("First(high 8ea points) : C_G[0] C_G[1] C_G[2] = " + First_temp, Color.Red);//Just For Debug, it can be deleted later (191113)

                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;
                double Calculated_Target_Lv = 0;
                int iteration;

                double[] C_R = new double[3];
                double[] C_G = new double[3];
                double[] C_B = new double[3];
                //Region [b,a] in range(d~c~b~a)
                if((First_3points_Target_Lv[1] <= Target_Lv) && (Target_Lv <= First_3points_Target_Lv[0]))
                {
                    f1.GB_Status_AppendText_Nextline("In Region [b,a], select first f(x)", Color.Blue);
                    for(int i=0;i<=2;i++)
                    {
                        C_R[i] = First_3points_8points_C_R[i];
                        C_G[i] = First_3points_8points_C_G[i];
                        C_B[i] = First_3points_8points_C_B[i];
                    }
                }
                //Region [d,c] in range(d~c~b~a)
                else if((Second_3points_Target_Lv[2] <= Target_Lv) && (Target_Lv <= Second_3points_Target_Lv[1]))
                {
                    f1.GB_Status_AppendText_Nextline("In Region [d,c], select second f(x)", Color.Red);
                    for(int i=0;i<=2;i++)
                    {
                        C_R[i] = Second_3points_8points_C_R[i];
                        C_G[i] = Second_3points_8points_C_G[i];
                        C_B[i] = Second_3points_8points_C_B[i];
                    }
                }
                //Region (c,b) in range(d~c~b~a)
                else if((Second_3points_Target_Lv[1] < Target_Lv) && (Target_Lv < First_3points_Target_Lv[1]))
                {
                    double Distance_1st = First_3points_Target_Lv[1] - Target_Lv;
                    double Distance_2nd = Target_Lv - Second_3points_Target_Lv[1];

                    f1.GB_Status_AppendText_Nextline("In Region (c,b),need to compare distance", Color.Green);
                    f1.GB_Status_AppendText_Nextline("Distance_1st : " + Distance_1st.ToString(), Color.Blue);
                    f1.GB_Status_AppendText_Nextline("Distance_2nd : " + Distance_2nd.ToString(), Color.Red);

                    if(Distance_1st < Distance_2nd)
                    {
                        f1.GB_Status_AppendText_Nextline("Distance_1st < Distance_2nd , first f(x) was selected", Color.Blue);
                        for (int i = 0; i <= 2; i++)
                        {
                            C_R[i] = First_3points_8points_C_R[i];
                            C_G[i] = First_3points_8points_C_G[i];
                            C_B[i] = First_3points_8points_C_B[i];
                        }
                    }
                    else
                    {
                        f1.GB_Status_AppendText_Nextline("Distance_1st >= Distance_2nd , second f(x) was selected", Color.Red);
                        for (int i = 0; i <= 2; i++)
                        {
                            C_R[i] = Second_3points_8points_C_R[i];
                            C_G[i] = Second_3points_8points_C_G[i];
                            C_B[i] = Second_3points_8points_C_B[i];
                        }
                    }
                }
                else
                {
                     f1.GB_Status_AppendText_Nextline("Out of A/B/C Region, It's Impossible to happen",Color.Red);
                }

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                double Band1_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Band1_Vreg1_Dec, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Actual_Previous_Vdata_Red = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Blue[0]);

                //Red
                //for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0)
                    {
                        Calculated_Vdata_Red = Vdata;
                        break;
                    }
                }

                //Green
                //for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0)
                    {
                        Calculated_Vdata_Green = Vdata;
                        break;
                    }
                }

                //Blue
                //for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.001
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
                    {
                        Calculated_Vdata_Blue = Vdata;
                        break;
                    }
                }

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Band1_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                Vreg1_Dec_Init = Meta_Engineering.Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                //Got the Vreg1 
                //Need to get Gamma_R/B
                double Current_Band_AM1_RGB_Voltage = F7 + (Vreg1_voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_B = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Blue);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }


        //To Be Tested
        private void Dll_C_Sharp_Meta_Get_Normal_Initial_Vreg1_For_Band2_3points(ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, bool[] Selected_Band, int[] Band0_Gamma_Red, int[] Band0_Gamma_Green, int[] Band0_Gamma_Blue, int[] Band1_Gamma_Red, int[] Band1_Gamma_Green, int[] Band1_Gamma_Blue, int band, double band_Target_Lv, int Band0_Vreg1_Dec,int Band1_Vreg1_Dec
            , double[] Band0_Target_Lv, double[] Band1_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            if (band >= 1 && Selected_Band[band] == true)
            {
                //Sorting
                double[] Band0_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band1_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band01_Gamma_Red_Voltage = new double[16];
                double[] Band01_Gamma_Green_Voltage = new double[16];
                double[] Band01_Gamma_Blue_Voltage = new double[16];

                double[] Band01_Target_Lv = new double[16];

                for (int i = 0; i < 8; i++)
                {
                    Band01_Gamma_Red_Voltage[i] = Band0_Gamma_Red_Voltage[i];
                    Band01_Gamma_Red_Voltage[i + 8] = Band1_Gamma_Red_Voltage[i];

                    Band01_Gamma_Green_Voltage[i] = Band0_Gamma_Green_Voltage[i];
                    Band01_Gamma_Green_Voltage[i + 8] = Band1_Gamma_Green_Voltage[i];

                    Band01_Gamma_Blue_Voltage[i] = Band0_Gamma_Blue_Voltage[i];
                    Band01_Gamma_Blue_Voltage[i + 8] = Band1_Gamma_Blue_Voltage[i];

                    Band01_Target_Lv[i] = Band0_Target_Lv[i];
                    Band01_Target_Lv[i + 8] = Band1_Target_Lv[i];
                }

                Array.Sort<double>(Band01_Gamma_Red_Voltage);
                Array.Sort<double>(Band01_Gamma_Green_Voltage);
                Array.Sort<double>(Band01_Gamma_Blue_Voltage);
           
                Array.Sort<double>(Band01_Target_Lv);
                Array.Reverse(Band01_Target_Lv);

                //Rearrange for 2ea 8points
                double[] First_3points_Gamma_Red_Voltage = new double[3];
                double[] First_3points_Gamma_Green_Voltage = new double[3];
                double[] First_3points_Gamma_Blue_Voltage = new double[3];
                double[] First_3points_Target_Lv = new double[3];



                SJH_Matrix M = new SJH_Matrix();
                double[][] First_3points_8points_A_R = M.MatrixCreate(3, 3);
                double[][] First_3points_8points_A_G = M.MatrixCreate(3, 3);
                double[][] First_3points_8points_A_B = M.MatrixCreate(3, 3);

                double[][] Second_3points_8points_A_R = M.MatrixCreate(3, 3);//Not used In Vreg1 Mode
                double[][] Second_3points_8points_A_G = M.MatrixCreate(3, 3);//Not used In Vreg1 Mode
                double[][] Second_3points_8points_A_B = M.MatrixCreate(3, 3);//Not used In Vreg1 Mode

                //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= 2 ; i++)
                {
                    First_3points_Target_Lv[i] = Band01_Target_Lv[i];
                    count = 0;
                    for (int j = 2; j >= 0; j--)
                    {
                        First_3points_8points_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i], j);
                        First_3points_8points_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i], j);
                        First_3points_8points_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i], j);
                        count++;
                    }
                }
                
                //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
                double[][] First_3points_8points_Inv_A_R = M.MatrixCreate(3, 3);
                double[][] First_3points_8points_Inv_A_G = M.MatrixCreate(3, 3);
                double[][] First_3points_8points_Inv_A_B = M.MatrixCreate(3, 3);
                double[] First_3points_8points_C_R = new double[3];
                double[] First_3points_8points_C_G = new double[3];
                double[] First_3points_8points_C_B = new double[3];
                First_3points_8points_Inv_A_R = M.MatrixInverse(First_3points_8points_A_R);
                First_3points_8points_Inv_A_G = M.MatrixInverse(First_3points_8points_A_G);
                First_3points_8points_Inv_A_B = M.MatrixInverse(First_3points_8points_A_B);
                First_3points_8points_C_R = M.Matrix_Multiply(First_3points_8points_Inv_A_R, First_3points_Target_Lv);
                First_3points_8points_C_G = M.Matrix_Multiply(First_3points_8points_Inv_A_G, First_3points_Target_Lv);
                First_3points_8points_C_B = M.Matrix_Multiply(First_3points_8points_Inv_A_B, First_3points_Target_Lv);

                for (int i = 0; i <= 2; i++) f1.GB_Status_AppendText_Nextline(i.ToString() + ")Band01_Gamma_Green_Voltage[i]/First_3points_Target_Lv[i] : " + Band01_Gamma_Green_Voltage[i].ToString() + "/" + First_3points_Target_Lv[i].ToString(), Color.Blue);
                    
                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                string First_temp = "";
                for (int i = 0; i <= 2; i++) First_temp += (First_3points_8points_C_G[i].ToString() + " ");
                f1.GB_Status_AppendText_Nextline("First(high 8ea points) : C_G[0] C_G[1] C_G[2] = " + First_temp, Color.Red);//Just For Debug, it can be deleted later (191113)
 
                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;
                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                double Band1_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Band1_Vreg1_Dec, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Actual_Previous_Vdata_Red = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Blue[0]);

                //Red
                //for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_3points_8points_C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0)
                    {
                        Calculated_Vdata_Red = Vdata;
                        break;
                    }
                }

                //Green
                //for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_3points_8points_C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0)
                    {
                        Calculated_Vdata_Green = Vdata;
                        break;
                    }
                }

                //Blue
                //for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.001
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_3points_8points_C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
                    {
                        Calculated_Vdata_Blue = Vdata;
                        break;
                    }
                }

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Band1_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                Vreg1_Dec_Init = Meta_Engineering.Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                //Got the Vreg1 
                //Need to get Gamma_R/B
                double Current_Band_AM1_RGB_Voltage = F7 + (Vreg1_voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_B = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Blue);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }

        //To Be Tested
        private void Dll_C_Sharp_Meta_Get_Normal_Initial_Vreg1_For_Band2(ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, bool[] Selected_Band, int[] Band0_Gamma_Red, int[] Band0_Gamma_Green, int[] Band0_Gamma_Blue, int[] Band1_Gamma_Red, int[] Band1_Gamma_Green, int[] Band1_Gamma_Blue, int band, double band_Target_Lv, int Band0_Vreg1_Dec,int Band1_Vreg1_Dec
            , double[] Band0_Target_Lv, double[] Band1_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
            Meta_Engineer_Mornitoring_Mode Meta_Engineering = (Meta_Engineer_Mornitoring_Mode)Application.OpenForms["Meta_Engineer_Mornitoring_Mode"];
            if (band >= 1 && Selected_Band[band] == true)
            {
                //Sorting
                double[] Band0_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band0_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band0_Vreg1_Dec, Band0_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band1_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double[] Band1_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Band1_Vreg1_Dec, Band1_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                double[] Band01_Gamma_Red_Voltage = new double[16];
                double[] Band01_Gamma_Green_Voltage = new double[16];
                double[] Band01_Gamma_Blue_Voltage = new double[16];

                double[] Band01_Target_Lv = new double[16];

                for (int i = 0; i < 8; i++)
                {
                    Band01_Gamma_Red_Voltage[i] = Band0_Gamma_Red_Voltage[i];
                    Band01_Gamma_Red_Voltage[i + 8] = Band1_Gamma_Red_Voltage[i];

                    Band01_Gamma_Green_Voltage[i] = Band0_Gamma_Green_Voltage[i];
                    Band01_Gamma_Green_Voltage[i + 8] = Band1_Gamma_Green_Voltage[i];

                    Band01_Gamma_Blue_Voltage[i] = Band0_Gamma_Blue_Voltage[i];
                    Band01_Gamma_Blue_Voltage[i + 8] = Band1_Gamma_Blue_Voltage[i];

                    Band01_Target_Lv[i] = Band0_Target_Lv[i];
                    Band01_Target_Lv[i + 8] = Band1_Target_Lv[i];
                }

                Array.Sort<double>(Band01_Gamma_Red_Voltage);
                Array.Sort<double>(Band01_Gamma_Green_Voltage);
                Array.Sort<double>(Band01_Gamma_Blue_Voltage);
           
                Array.Sort<double>(Band01_Target_Lv);
                Array.Reverse(Band01_Target_Lv);

                //Rearrange for 2ea 8points
                double[] First_8points_Gamma_Red_Voltage = new double[8];
                double[] First_8points_Gamma_Green_Voltage = new double[8];
                double[] First_8points_Gamma_Blue_Voltage = new double[8];
                double[] First_8points_Target_Lv = new double[8];

                double[] Second_8points_Gamma_Red_Voltage = new double[8]; //Not used In Vreg1 Mode
                double[] Second_8points_Gamma_Green_Voltage = new double[8];//Not used In Vreg1 Mode
                double[] Second_8points_Gamma_Blue_Voltage = new double[8];//Not used In Vreg1 Mode
                double[] Second_8points_Target_Lv = new double[8];//Not used In Vreg1 Mode



                SJH_Matrix M = new SJH_Matrix();
                double[][] First_8points_A_R = M.MatrixCreate(8, 8);
                double[][] First_8points_A_G = M.MatrixCreate(8, 8);
                double[][] First_8points_A_B = M.MatrixCreate(8, 8);

                double[][] Second_8points_A_R = M.MatrixCreate(8, 8);//Not used In Vreg1 Mode
                double[][] Second_8points_A_G = M.MatrixCreate(8, 8);//Not used In Vreg1 Mode
                double[][] Second_8points_A_B = M.MatrixCreate(8, 8);//Not used In Vreg1 Mode

                //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= 7 ; i++)
                {
                    First_8points_Target_Lv[i] = Band01_Target_Lv[i];
                    Second_8points_Target_Lv[i] = Band01_Target_Lv[i + 8];

                    count = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        First_8points_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i], j);
                        First_8points_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i], j);
                        First_8points_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i], j);

                        Second_8points_A_R[i][count] = Math.Pow(Band01_Gamma_Red_Voltage[i + 8], j);//Not used In Vreg1 Mode
                        Second_8points_A_G[i][count] = Math.Pow(Band01_Gamma_Green_Voltage[i + 8], j);//Not used In Vreg1 Mode
                        Second_8points_A_B[i][count] = Math.Pow(Band01_Gamma_Blue_Voltage[i + 8], j);//Not used In Vreg1 Mode

                        count++;
                    }
                }
                
                //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
                double[][] First_8points_Inv_A_R = M.MatrixCreate(8, 8);
                double[][] First_8points_Inv_A_G = M.MatrixCreate(8, 8);
                double[][] First_8points_Inv_A_B = M.MatrixCreate(8, 8);
                double[] First_8points_C_R = new double[8];
                double[] First_8points_C_G = new double[8];
                double[] First_8points_C_B = new double[8];
                First_8points_Inv_A_R = M.MatrixInverse(First_8points_A_R);
                First_8points_Inv_A_G = M.MatrixInverse(First_8points_A_G);
                First_8points_Inv_A_B = M.MatrixInverse(First_8points_A_B);
                First_8points_C_R = M.Matrix_Multiply(First_8points_Inv_A_R, First_8points_Target_Lv);
                First_8points_C_G = M.Matrix_Multiply(First_8points_Inv_A_G, First_8points_Target_Lv);
                First_8points_C_B = M.Matrix_Multiply(First_8points_Inv_A_B, First_8points_Target_Lv);

                double[][] Second_8points_Inv_A_R = M.MatrixCreate(8, 8);//Not used In Vreg1 Mode
                double[][] Second_8points_Inv_A_G = M.MatrixCreate(8, 8);//Not used In Vreg1 Mode
                double[][] Second_8points_Inv_A_B = M.MatrixCreate(8, 8);//Not used In Vreg1 Mode
                double[] Second_8points_C_R = new double[8];//Not used In Vreg1 Mode
                double[] Second_8points_C_G = new double[8];//Not used In Vreg1 Mode
                double[] Second_8points_C_B = new double[8];//Not used In Vreg1 Mode
                Second_8points_Inv_A_R = M.MatrixInverse(Second_8points_A_R);//Not used In Vreg1 Mode
                Second_8points_Inv_A_G = M.MatrixInverse(Second_8points_A_G);//Not used In Vreg1 Mode
                Second_8points_Inv_A_B = M.MatrixInverse(Second_8points_A_B);//Not used In Vreg1 Mode
                Second_8points_C_R = M.Matrix_Multiply(Second_8points_Inv_A_R, Second_8points_Target_Lv);//Not used In Vreg1 Mode
                Second_8points_C_G = M.Matrix_Multiply(Second_8points_Inv_A_G, Second_8points_Target_Lv);//Not used In Vreg1 Mode
                Second_8points_C_B = M.Matrix_Multiply(Second_8points_Inv_A_B, Second_8points_Target_Lv);//Not used In Vreg1 Mode

                for (int i = 0; i <= 7; i++) f1.GB_Status_AppendText_Nextline(i.ToString() + ")Band01_Gamma_Green_Voltage[i]/First_8points_Target_Lv[i] : " + Band01_Gamma_Green_Voltage[i].ToString() + "/" + First_8points_Target_Lv[i].ToString(), Color.Blue);
                for (int i = 0; i <= 7; i++) f1.GB_Status_AppendText_Nextline(i.ToString() + ")Band01_Gamma_Green_Voltage[i+8]/Second_8points_Target_Lv[i] : " + Band01_Gamma_Green_Voltage[i + 8].ToString() + "/" + Second_8points_Target_Lv[i].ToString(), Color.Green);
                    
                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                string First_temp = "";
                string Second_temp = "";
                for (int i = 0; i <= 7; i++)
                {
                    First_temp += (First_8points_C_G[i].ToString() + " ");
                    Second_temp += (Second_8points_C_G[i].ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("First(high 8ea points) : C_G[0] C_G[1] ... C_G[7] = " + First_temp, Color.Red);//Just For Debug, it can be deleted later (191113)
                f1.GB_Status_AppendText_Nextline("Second(low 8ea points) : C_G[0] C_G[1] ... C_G[7] = " + Second_temp, Color.Red);//Just For Debug, it can be deleted later (191113)



                   
                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;
                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                double Band1_Vreg1_Voltage = Meta_Engineering.Get_Vreg1_Voltage(Band1_Vreg1_Dec, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Actual_Previous_Vdata_Red = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Meta_Engineering.Get_AM2_Voltage(F7, Band1_Vreg1_Voltage, Band1_Gamma_Blue[0]);

                //Red
                //for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_8points_C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0)
                    {
                        Calculated_Vdata_Red = Vdata;
                        break;
                    }
                }

                //Green
                //for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_8points_C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0)
                    {
                        Calculated_Vdata_Green = Vdata;
                        break;
                    }
                }

                //Blue
                //for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.001
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.0001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * First_8points_C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
                    {
                        Calculated_Vdata_Blue = Vdata;
                        break;
                    }
                }

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Band1_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                Vreg1_Dec_Init = Meta_Engineering.Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

                //Got the Vreg1 
                //Need to get Gamma_R/B
                double Current_Band_AM1_RGB_Voltage = F7 + (Vreg1_voltage - F7) * (8.0 / 700.0);
                Gamma_R = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_B = Meta_Engineering.Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Blue);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }
    }
}
