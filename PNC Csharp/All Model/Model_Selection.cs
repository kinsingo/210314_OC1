using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    interface OC_Related_Get_Methods
    {
        void Get_File_Address();
        string Get_Single_OC_Param_Address();
        string Get_Single_OC_G2G_Param_Address();

        string Get_Dual_OC_Param_Address(Gamma_Set Set);
        string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set);

        string Get_Dual_OC_Param_Address(OC_Mode Mode);
        string Get_Dual_OC_G2G_Param_Address(OC_Mode Mode);

        string Get_Dual_Set2_Diff_Delta_L_Spec_Address();
        string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address();

        string Get_Single_OC_Verify_Address();
        Color Get_Back_Ground_Color();
    }

    public abstract class ModelInfo : OC_Related_Get_Methods
    {
        protected int X;
        protected int Y;
        protected int AOD_X;
        protected int AOD_Y;
        protected int DBV_Max;
       
        virtual public int get_X() { return X; }
        virtual public int get_Y() { return Y; }
        virtual public int get_AOD_X() { return AOD_X; }
        virtual public int get_AOD_Y() { return AOD_Y; }
        virtual public int get_DBV_Max() { return DBV_Max; }

        public Model_Name current_model_name;
        protected OC_Param_Load OC_parameter_loder;

        abstract public void Get_File_Address();
        abstract public string Get_Single_OC_Param_Address();
        abstract public string Get_Single_OC_G2G_Param_Address();
        abstract public string Get_Dual_OC_Param_Address(Gamma_Set Set);
        abstract public string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set);
        abstract public string Get_Dual_OC_Param_Address(OC_Mode Mode);
        abstract public string Get_Dual_OC_G2G_Param_Address(OC_Mode Mode);
        abstract public string Get_Dual_Set2_Diff_Delta_L_Spec_Address();
        abstract public string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address();
        abstract public string Get_Single_OC_Verify_Address();
        abstract public Color Get_Back_Ground_Color();

        public void OC_Param_load() { OC_parameter_loder.OC_Param_load(); }
        public void Read_OC_Param_From_Excel_File() { OC_parameter_loder.Read_OC_Param_From_Excel_File(); }
        public void Read_OC_Param_From_Excel_For_Dual_Mode() { OC_parameter_loder.Read_OC_Param_From_Excel_For_Dual_Mode(); }

    }

    //DP086
    class DP086_NT37280 : ModelInfo
    {
        private static DP086_NT37280 instance = null;
        protected DP086_NT37280()
        {
            current_model_name = Model_Name.DP086;
            OC_parameter_loder = new DP116_or_DP086_OC_Param();
            X = 1080;
            Y = 2340;
            AOD_X = Convert.ToInt16((X * Math.Sqrt(0.1)));
            AOD_Y = Convert.ToInt16((Y * Math.Sqrt(0.1)));
            DBV_Max = 4095;
        }
        public static DP086_NT37280 getInstance()
        {
            if (instance == null) instance = new DP086_NT37280();
            return instance;
        }
        public override void Get_File_Address()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.textBox_Auto_Address.Text = Directory.GetCurrentDirectory() + "\\DP086\\Script_AutoSequence.txt";
            f1.textBox_Manual_Address.Text = Directory.GetCurrentDirectory() + "\\DP086\\Script_ManuSequence.txt";
            f1.AOD_In_Path = Directory.GetCurrentDirectory() + "\\DP086\\Mode_AOD_In.txt";
            f1.AOD_Out_Path = Directory.GetCurrentDirectory() + "\\DP086\\Mode_AOD_Out.txt";
            f1.VR_In_Path = Directory.GetCurrentDirectory() + "\\DP086\\Mode_VR_In.txt";
            f1.VR_Out_Path = Directory.GetCurrentDirectory() + "\\DP086\\Mode_VR_Out.txt";
            f1.Turn_Off_Path = Directory.GetCurrentDirectory() + "\\DP086\\Script_Power_Off.txt";
        }
        public override Color Get_Back_Ground_Color()
        {
            return Color.LightGreen;
        }
        public override string Get_Single_OC_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP086\\OC_Parameter.csv";
        }
        public override string Get_Single_OC_Verify_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP086\\OC_Verify.csv";
        }

        public override string Get_Single_OC_G2G_Param_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_Param_Address(Gamma_Set Set)
        {
            return Directory.GetCurrentDirectory() + "\\DP086\\Dual_Mode\\OC_Parameter_" + Set.ToString() + ".csv";
        }

        public override string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_Set2_Diff_Delta_L_Spec_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address()
        {
            throw new NotImplementedException();
        }
    
        public override string  Get_Dual_OC_Param_Address(OC_Mode Mode)
        {
 	        throw new NotImplementedException();
        }

        public override string  Get_Dual_OC_G2G_Param_Address(OC_Mode Mode)
        {
 	        throw new NotImplementedException();
        }
    }

    //DP116
    class DP116_NT37280 : ModelInfo
    {
        private static DP116_NT37280 instance = null;
        protected DP116_NT37280()
        {
            current_model_name = Model_Name.DP116;
            OC_parameter_loder = new DP116_or_DP086_OC_Param();
            X = 1176;
            Y = 2400;
            AOD_X = Convert.ToInt16((X * Math.Sqrt(0.1)));
            AOD_Y = Convert.ToInt16((Y * Math.Sqrt(0.1)));
            DBV_Max = 100;
        }
        public static DP116_NT37280 getInstance()
        {
            if (instance == null) instance = new DP116_NT37280();
            return instance;
        }
        public string page_selection(int page_num)
        {
            string Page_Access_Param = "0x00";
            switch (page_num)
            {
                case 0:
                    Page_Access_Param = "0x20";	// CMD2_P0	: Power / Gate / Source / MIPI / MTP / Gamma1
                    break;
                case 1:
                    Page_Access_Param = "0x21";	// CMD2_P1	: Gamma2 / Gamma0
                    break;
                case 2:
                    Page_Access_Param = "0x22";
                    break;
                case 3:
                    Page_Access_Param = "0x23";	// CMD2_P3	: Orbit / Incell / FRM / Color Swap
                    break;
                case 4:
                    Page_Access_Param = "0x24";	// CMD2_P4	: GIP_EM timing
                    break;
                case 5:
                    Page_Access_Param = "0x25";	// CMD2_P5	: GIP_EM timing / Power Sequence
                    break;
                case 6:
                    Page_Access_Param = "0x26";	// CMD2_P6	: VRR / N2 / AOD
                    break;
                case 7:
                    Page_Access_Param = "0x27";	// CMD2_P7	: PWR Transition / Temp Sensor / RTC
                    break;
                case 8:
                    Page_Access_Param = "0x28";	// CMD2_P7	: PWR Transition / Temp Sensor / RTC
                    break;
                case 9:
                    Page_Access_Param = "0x29";	// CMD2_P7	: PWR Transition / Temp Sensor / RTC
                    break;
                case 10:
                    Page_Access_Param = "0x2A";	// CMD2_P7	: PWR Transition / Temp Sensor / RTC
                    break;
                case 11:
                    Page_Access_Param = "0x2B";	// CMD2_P11	: Flash Control
                    break;
                case 12:
                    Page_Access_Param = "0x2C";	// CMD2_P12	: IP1 / FPL / BC / FPL / PLC / IS
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Page_num cannot be this value : " + page_num.ToString());
                    break;
            }
            return Page_Access_Param;
        }
        public override void Get_File_Address()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.textBox_Auto_Address.Text = Directory.GetCurrentDirectory() + "\\DP116\\Script_AutoSequence.txt";
            f1.textBox_Manual_Address.Text = Directory.GetCurrentDirectory() + "\\DP116\\Script_ManuSequence.txt";
            f1.AOD_In_Path = Directory.GetCurrentDirectory() + "\\DP116\\Mode_AOD_In.txt";
            f1.AOD_Out_Path = Directory.GetCurrentDirectory() + "\\DP116\\Mode_AOD_Out.txt";
            f1.VR_In_Path = Directory.GetCurrentDirectory() + "\\DP116\\Mode_VR_In.txt";
            f1.VR_Out_Path = Directory.GetCurrentDirectory() + "\\DP116\\Mode_VR_Out.txt";
            f1.Turn_Off_Path = Directory.GetCurrentDirectory() + "\\DP116\\Script_Power_Off.txt";
        }
        public override Color Get_Back_Ground_Color()
        {
            return Color.LightSteelBlue;
        }
        public override string Get_Single_OC_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP116\\OC_Parameter.csv";
        }
        public override string Get_Single_OC_Verify_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP116\\OC_Verify.csv";
        }

        public override string Get_Single_OC_G2G_Param_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_Param_Address(Gamma_Set Set)
        {
            return Directory.GetCurrentDirectory() + "\\DP116\\Dual_Mode\\OC_Parameter_" + Set.ToString() + ".csv";
        }

        public override string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }
        public override string Get_Dual_Set2_Diff_Delta_L_Spec_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_Param_Address(OC_Mode Mode)
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_G2G_Param_Address(OC_Mode Mode)
        {
            throw new NotImplementedException();
        }
    }

    //DP150
    class DP150_EA9152 : ModelInfo
    {
        private static DP150_EA9152 instance = null;
        protected DP150_EA9152()
        {
            current_model_name = Model_Name.DP150;
            OC_parameter_loder = new DP150_OC_Param();
            X = 1200;
            Y = 2640;
            AOD_X = 563;
            AOD_Y = 563;
            DBV_Max = 2047;
        }
        public static DP150_EA9152 getInstance()
        {
            if (instance == null) instance = new DP150_EA9152();
            return instance;
        }
        public string Get_Gamma_Register_Hex_String(int band)
        {
            string hex_string = "0x00";
            switch (band)
            {
                //HBM (band 0) Normal(band 1~10) AOD(band 11~13)
                case 0:
                    hex_string = "0xB3";
                    break;
                case 1:
                    hex_string = "0xB4";
                    break;
                case 2:
                    hex_string = "0xB5";
                    break;
                case 3:
                    hex_string = "0xB6";
                    break;
                case 4:
                    hex_string = "0xB7";
                    break;
                case 5:
                    hex_string = "0xB8";
                    break;
                case 6:
                    hex_string = "0xB9";
                    break;
                case 7:
                    hex_string = "0xBA";
                    break;
                case 8:
                    hex_string = "0xBB";
                    break;
                case 9:
                    hex_string = "0xBC";
                    break;
                case 10:
                    hex_string = "0xBD";
                    break;
                case 11:
                    hex_string = "0xB2";
                    break;
                case 12:
                    hex_string = "0xB2";
                    break;
                case 13:
                    hex_string = "0xB2";
                    break;
                //Cannot take place
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                    break;
            }
            return hex_string;
        }
        public override void Get_File_Address()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.textBox_Auto_Address.Text = Directory.GetCurrentDirectory() + "\\DP150\\Script_AutoSequence.txt";
            f1.textBox_Manual_Address.Text = Directory.GetCurrentDirectory() + "\\DP150\\Script_ManuSequence.txt";
            f1.AOD_In_Path = Directory.GetCurrentDirectory() + "\\DP150\\Mode_AOD_In.txt";
            f1.AOD_Out_Path = Directory.GetCurrentDirectory() + "\\DP150\\Mode_AOD_Out.txt";
            f1.VR_In_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.Turn_Off_Path = Directory.GetCurrentDirectory() + "";
        }
        public override Color Get_Back_Ground_Color()
        {
            return Color.Ivory;
        }
        public override string Get_Single_OC_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP150\\OC_Parameter.csv";
        }
        public override string Get_Single_OC_Verify_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP150\\OC_Verify.csv";
        }

        public override string Get_Single_OC_G2G_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP150\\OC_Parameter_G2G.csv";
        }

        public override string Get_Dual_OC_Param_Address(Gamma_Set Set)
        {
            return Directory.GetCurrentDirectory() + "\\DP150\\Dual_Mode\\OC_Parameter_" + Set.ToString() + ".csv";
        }

        public override string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set)
        {
            return Directory.GetCurrentDirectory() + "\\DP150\\Dual_Mode\\OC_Parameter_G2G_" + Set.ToString() + ".csv";
        }
        public override string Get_Dual_Set2_Diff_Delta_L_Spec_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_Param_Address(OC_Mode Mode)
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_G2G_Param_Address(OC_Mode Mode)
        {
            throw new NotImplementedException();
        }
    }

    //Meta
    class Meta_SW43408B : ModelInfo
    {
        private static Meta_SW43408B instance = null;
        private Meta_SW43408B()
        {
            current_model_name = Model_Name.Meta;
            OC_parameter_loder = new Meta_OC_Param();
            X = 1350;
            Y = 1804;
            AOD_X = Convert.ToInt16((X * Math.Sqrt(0.1)));
            AOD_Y = Convert.ToInt16((Y * Math.Sqrt(0.1)));
            DBV_Max = 1023;
        }
        public static Meta_SW43408B getInstance()
        {
            if (instance == null) instance = new Meta_SW43408B();
            return instance;
        }
        public string Get_Gamma_Register_Hex_String(int band)
        {
            string hex_string = "0x00";
            switch (band)
            {
                //HBM (band 0) Normal(band 1~10) AOD(band 11~13)
                case 0:
                    hex_string = "0xD2";
                    break;
                case 1:
                    hex_string = "0xD3";
                    break;
                case 2:
                    hex_string = "0xD4";
                    break;
                case 3:
                    hex_string = "0xD5";
                    break;
                case 4:
                    hex_string = "0xD6";
                    break;
                case 5:
                    hex_string = "0xD7";
                    break;
                case 6:
                    hex_string = "0xD8";
                    break;
                case 7:
                    hex_string = "0xD9";
                    break;
                case 8:
                    hex_string = "0xDD";
                    break;
                case 9:
                    hex_string = "0xDE";
                    break;
                //Cannot take place
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                    break;
            }
            return hex_string;
        }
        public override void Get_File_Address()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.textBox_Auto_Address.Text = Directory.GetCurrentDirectory() + "";
            f1.textBox_Manual_Address.Text = Directory.GetCurrentDirectory() + "";
            f1.AOD_In_Path = Directory.GetCurrentDirectory() + "";
            f1.AOD_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_In_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.Turn_Off_Path = Directory.GetCurrentDirectory() + "";
        }
        public override Color Get_Back_Ground_Color()
        {
            return Color.LightPink;
        }
        public override string Get_Single_OC_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\Meta\\OC_Parameter.csv";
        }
        public override string Get_Single_OC_Verify_Address()
        {
            return Directory.GetCurrentDirectory() + "\\Meta\\OC_Verify.csv";
        }

        public override string Get_Single_OC_G2G_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\Meta\\OC_Parameter_G2G.csv";
        }

        public override string Get_Dual_OC_Param_Address(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }
        public override string Get_Dual_Set2_Diff_Delta_L_Spec_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_Param_Address(OC_Mode Mode)
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_G2G_Param_Address(OC_Mode Mode)
        {
            throw new NotImplementedException();
        }
    }

    //DP173 
    class DP173_EA9154 : DP150_EA9152
    {
        private static DP173_EA9154 instance = null;
        protected DP173_EA9154()
        {
            current_model_name = Model_Name.DP173;
            OC_parameter_loder = new DP173_or_Elgin_OC_Param();
            X = 1344;
            Y = 2772;
            AOD_X = 610;
            AOD_Y = 610;
            DBV_Max = 2047;
        }
        public static DP173_EA9154 getInstance()
        {
            if (instance == null) instance = new DP173_EA9154();
            return instance;
        }
        public override void Get_File_Address()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.textBox_Auto_Address.Text = Directory.GetCurrentDirectory() + "\\DP173\\Script_AutoSequence.txt";
            f1.textBox_Manual_Address.Text = Directory.GetCurrentDirectory() + "\\DP173\\Script_ManuSequence.txt";
            f1.AOD_In_Path = Directory.GetCurrentDirectory() + "";
            f1.AOD_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_In_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.Turn_Off_Path = Directory.GetCurrentDirectory() + "";
        }
        public override Color Get_Back_Ground_Color()
        {
            return Color.FromArgb(210, 190, 210);
        }
        public override string Get_Single_OC_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP173\\OC_Parameter.csv";
        }
        public override string Get_Single_OC_Verify_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP173\\OC_Verify.csv";
        }

        public override string Get_Single_OC_G2G_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP173\\OC_Parameter_G2G.csv";
        }

        public override string Get_Dual_OC_Param_Address(Gamma_Set Set)
        {
            return Directory.GetCurrentDirectory() + "\\DP173\\Dual_Mode\\OC_Parameter_" +Set.ToString() + ".csv";
        }

        public override string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set)
        {
            return Directory.GetCurrentDirectory() + "\\DP173\\Dual_Mode\\OC_Parameter_G2G_" + Set.ToString() + ".csv";
        }
        public override string Get_Dual_Set2_Diff_Delta_L_Spec_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP173\\Dual_Mode\\Set2_Delta_L_Diff_Spec.csv";
        }

        public override string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP173\\Dual_Mode\\Set2_Delta_L_Diff_Spec_G2G.csv";
        }
    }

    //Elgin 
    class Elgin_EA9154 : DP173_EA9154
    {
        private static Elgin_EA9154 instance = null;
        private Elgin_EA9154()
        {
            current_model_name = Model_Name.Elgin;
            X = 1896;
            Y = 1344;
            AOD_X = 505;
            AOD_Y = 505;
            DBV_Max = 2047;
        }
        public static Elgin_EA9154 getInstance()
        {
            if (instance == null) instance = new Elgin_EA9154();
            return instance;
        }
        public override void Get_File_Address()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.textBox_Auto_Address.Text = Directory.GetCurrentDirectory() + "\\Elgin\\Script_AutoSequence.txt";
            f1.textBox_Manual_Address.Text = Directory.GetCurrentDirectory() + "\\Elgin\\Script_ManuSequence.txt";
            f1.AOD_In_Path = Directory.GetCurrentDirectory() + "";
            f1.AOD_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_In_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.Turn_Off_Path = Directory.GetCurrentDirectory() + "";
        }
        public override Color Get_Back_Ground_Color()
        {
            return Color.FromArgb(170, 210, 210);
        }
        public override string Get_Single_OC_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\Elgin\\OC_Parameter.csv";
        }
        public override string Get_Single_OC_Verify_Address()
        {
            return Directory.GetCurrentDirectory() + "\\Elgin\\OC_Verify.csv";
        }

        public override string Get_Single_OC_G2G_Param_Address()
        {
            return Directory.GetCurrentDirectory() + "\\Elgin\\OC_Parameter_G2G.csv";
        }

        public override string Get_Dual_OC_Param_Address(Gamma_Set Set)
        {
            return Directory.GetCurrentDirectory() + "\\Elgin\\Dual_Mode\\OC_Parameter_" + Set.ToString() + ".csv";
        }

        public override string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set)
        {
            return Directory.GetCurrentDirectory() + "\\Elgin\\Dual_Mode\\OC_Parameter_G2G_" + Set.ToString() + ".csv";
        }
        public override string Get_Dual_Set2_Diff_Delta_L_Spec_Address()
        {
            return Directory.GetCurrentDirectory() + "\\Elgin\\Dual_Mode\\Set2_Delta_L_Diff_Spec.csv";
        }

        public override string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address()
        {
            return Directory.GetCurrentDirectory() + "\\Elgin\\Dual_Mode\\Set2_Delta_L_Diff_Spec_G2G.csv";
        }
    }

    //DP213 
    class DP213_EA9155 : DP173_EA9154
    {
        private static DP213_EA9155 instance = null;
        protected DP213_EA9155()
        {
            current_model_name = Model_Name.DP213;
            OC_parameter_loder = new DP213_OC_Param();
            X = 1228;
            Y = 2700;
            AOD_X = 576;
            AOD_Y = 576;
            DBV_Max = 4095;
        }
        public static DP213_EA9155 getInstance()
        {
            if (instance == null) instance = new DP213_EA9155();
            return instance;
        }
        public override void Get_File_Address()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.textBox_Auto_Address.Text = Directory.GetCurrentDirectory() + "\\DP213\\Script_AutoSequence.txt";
            f1.textBox_Manual_Address.Text = Directory.GetCurrentDirectory() + "\\DP213\\Script_ManuSequence.txt";
            f1.AOD_In_Path = Directory.GetCurrentDirectory() + "";
            f1.AOD_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_In_Path = Directory.GetCurrentDirectory() + "";
            f1.VR_Out_Path = Directory.GetCurrentDirectory() + "";
            f1.Turn_Off_Path = Directory.GetCurrentDirectory() + "";
        }
        public override Color Get_Back_Ground_Color()
        {
            return Color.FromArgb(255, 255, 160);
        }
        public override string Get_Single_OC_Param_Address()
        {
            throw new NotImplementedException();
        }
        public override string Get_Single_OC_Verify_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Single_OC_G2G_Param_Address()
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_Param_Address(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public override string Get_Dual_OC_Param_Address(OC_Mode Mode)
        {
            return Directory.GetCurrentDirectory() + "\\DP213\\Triple_Mode\\OC_Parameter_" + Mode.ToString() + ".csv";
        }

        public override string Get_Dual_OC_G2G_Param_Address(OC_Mode Mode)
        {
            return Directory.GetCurrentDirectory() + "\\DP213\\Triple_Mode\\OC_Parameter_G2G_" + Mode.ToString() + ".csv";
        }

        public override string Get_Dual_Set2_Diff_Delta_L_Spec_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP213\\Triple_Mode\\Mode2_Mode3_Delta_L_Diff_Spec.csv";
        }

        public override string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address()
        {
            return Directory.GetCurrentDirectory() + "\\DP213\\Triple_Mode\\Mode2_Mode3_Delta_L_Diff_Spec_G2G.csv";
        }
    }

    public class Selected_Model : OC_Related_Get_Methods
    {
        ModelInfo current_model;

        public void Set_Model_As_DP086() { current_model = DP086_NT37280.getInstance(); }
        public void Set_Model_As_DP116() { current_model = DP116_NT37280.getInstance(); }
        public void Set_Model_As_DP150() { current_model = DP150_EA9152.getInstance(); }
        public void Set_Model_As_Meta() { current_model = Meta_SW43408B.getInstance(); }
        public void Set_Model_As_DP173() { current_model = DP173_EA9154.getInstance(); }
        public void Set_Model_As_Elgin() { current_model = Elgin_EA9154.getInstance(); }
        public void Set_Model_As_DP213() { current_model = DP213_EA9155.getInstance(); }

        private static Selected_Model instance = null;
        private Selected_Model()
        {
            Set_Model_As_DP086();
        }

        //public
        public static Selected_Model getInstance()
        {
            if (instance == null) instance = new Selected_Model();
            return instance;
        }

        public void OC_Param_load() { current_model.OC_Param_load(); }
        public void Read_OC_Param_From_Excel_File() { current_model.Read_OC_Param_From_Excel_File(); }
        public void Read_OC_Param_From_Excel_For_Dual_Mode() { current_model.Read_OC_Param_From_Excel_For_Dual_Mode(); }

        public Model_Name Get_Current_Model_Name() { return current_model.current_model_name; }
        public int get_X() { return current_model.get_X(); }
        public int get_Y() { return current_model.get_Y(); }
        public int get_AOD_X() { return current_model.get_AOD_X(); }
        public int get_AOD_Y() { return current_model.get_AOD_Y(); }

        public int get_DBV_Max() { return current_model.get_DBV_Max(); }
        public void Get_File_Address() { current_model.Get_File_Address(); }
        public Color Get_Back_Ground_Color() { return current_model.Get_Back_Ground_Color(); }
        public string Get_Single_OC_Param_Address() { return current_model.Get_Single_OC_Param_Address(); }
        public string Get_Single_OC_Verify_Address() { return current_model.Get_Single_OC_Verify_Address(); }
        public string Get_Single_OC_G2G_Param_Address() { return current_model.Get_Single_OC_G2G_Param_Address(); }
        public string Get_Dual_OC_Param_Address(Gamma_Set Set) { return current_model.Get_Dual_OC_Param_Address(Set); }
        public string Get_Dual_OC_G2G_Param_Address(Gamma_Set Set) { return current_model.Get_Dual_OC_G2G_Param_Address(Set); }
        public string Get_Dual_OC_Param_Address(OC_Mode Mode) { return current_model.Get_Dual_OC_Param_Address(Mode); }
        public string Get_Dual_OC_G2G_Param_Address(OC_Mode Mode) { return current_model.Get_Dual_OC_G2G_Param_Address(Mode); }
        public string Get_Dual_Set2_Diff_Delta_L_Spec_Address() { return current_model.Get_Dual_Set2_Diff_Delta_L_Spec_Address(); }
        public string Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address() { return current_model.Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address(); }
    }


}