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

//using References
using SectionLib;
using System.IO.MemoryMappedFiles;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Xml.Serialization;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public partial class DP173_Model_Option_Form : Form
    {
        DP173_EA9154 DP173 = DP173_EA9154.getInstance();

        Color Status_Color_Set1 = Color.FromArgb(155, 50, 50);
        Color Status_Color_Set2 = Color.FromArgb(155, 100, 50);
        Color Status_Color_Set3 = Color.FromArgb(75, 75, 155);
        Color Status_Color_Set4 = Color.FromArgb(50, 100, 155);
        Color Status_Color_Set5 = Color.FromArgb(100, 155, 100);
        Color Status_Color_Set6 = Color.FromArgb(50, 155, 50);

        //GB Status/Result
        public bool Optic_Compensation_Stop = false;
        public bool Optic_Compensation_Succeed = false;

        //Compensation related(Vreg1)
        string[] B1_Vreg1_Gamma_Set1 = new string[17];
        string[] B1_Vreg1_Gamma_Set2 = new string[17];
        string[] B1_Vreg1_Gamma_Set3 = new string[17];
        string[] B1_Vreg1_Gamma_Set4 = new string[17];
        string[] B1_Vreg1_Gamma_Set5 = new string[17];
        string[] B1_Vreg1_Gamma_Set6 = new string[17];

        //Extension
        string Extension_Applied = "X";
        double Vreg1_voltage;

        //REF Voltage 
        public double Voltage_VREG1_REF1;
        public double Voltage_VREG1_REF2047;
        public double Voltage_VREG1_REF1635;
        public double Voltage_VREG1_REF1227;
        public double Voltage_VREG1_REF815;
        public double Voltage_VREG1_REF407;
        public double Voltage_VREG1_REF63;

        public double[,] Set2_Diff_Delta_L_Spec = new double[11, 8];
        public double[,] Set2_Diff_Delta_UV_Spec = new double[11, 8];
        //------------------------------------------------------------
        XmlSerializer mySerializer = new XmlSerializer(typeof(EA9154_Preferences));//Used For Saving and Loading Setting
   
        private void Show_AM0_AM1_GR_Voltage(int band, RGB_Double[,] GR_Voltage, int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, int Dec_AM0_R, int Dec_AM0_G, int Dec_AM0_B, double[] Calculated_Vreg1_Voltage, double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1)
        {
            double Vreg1_Voltage = Calculated_Vreg1_Voltage[band];

            double AM1_R_Voltage = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_Voltage, Dec_AM1_R);
            double AM1_G_Voltage = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_Voltage, Dec_AM1_G);
            double AM1_B_Voltage = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_Voltage, Dec_AM1_B);

            double AM0_R_Voltage = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_Voltage, Dec_AM0_R);
            double AM0_G_Voltage = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_Voltage, Dec_AM0_G);
            double AM0_B_Voltage = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_Voltage, Dec_AM0_B);

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("AM2 R/G/B Voltage : " + Math.Round(GR_Voltage[band, 0].double_R, 4) + "/" + Math.Round(GR_Voltage[band, 0].double_G, 4) + "/" + Math.Round(GR_Voltage[band, 0].double_B, 4), Color.Black);
            f1.GB_Status_AppendText_Nextline("GR7 R/G/B Voltage : " + Math.Round(GR_Voltage[band, 1].double_R, 4) + "/" + Math.Round(GR_Voltage[band, 1].double_G, 4) + "/" + Math.Round(GR_Voltage[band, 1].double_B, 4), Color.Black);
            f1.GB_Status_AppendText_Nextline("GR6 R/G/B Voltage : " + Math.Round(GR_Voltage[band, 2].double_R, 4) + "/" + Math.Round(GR_Voltage[band, 2].double_G, 4) + "/" + Math.Round(GR_Voltage[band, 2].double_B, 4), Color.Black);
            f1.GB_Status_AppendText_Nextline("GR5 R/G/B Voltage : " + Math.Round(GR_Voltage[band, 3].double_R, 4) + "/" + Math.Round(GR_Voltage[band, 3].double_G, 4) + "/" + Math.Round(GR_Voltage[band, 3].double_B, 4), Color.Black);
            f1.GB_Status_AppendText_Nextline("GR4 R/G/B Voltage : " + Math.Round(GR_Voltage[band, 4].double_R, 4) + "/" + Math.Round(GR_Voltage[band, 4].double_G, 4) + "/" + Math.Round(GR_Voltage[band, 4].double_B, 4), Color.Black);
            f1.GB_Status_AppendText_Nextline("GR3 R/G/B Voltage : " + Math.Round(GR_Voltage[band, 5].double_R, 4) + "/" + Math.Round(GR_Voltage[band, 5].double_G, 4) + "/" + Math.Round(GR_Voltage[band, 5].double_B, 4), Color.Black);
            f1.GB_Status_AppendText_Nextline("GR2 R/G/B Voltage : " + Math.Round(GR_Voltage[band, 6].double_R, 4) + "/" + Math.Round(GR_Voltage[band, 6].double_G, 4) + "/" + Math.Round(GR_Voltage[band, 6].double_B, 4), Color.Black);
            f1.GB_Status_AppendText_Nextline("GR1 or GR0 R/G/B Voltage : " + Math.Round(GR_Voltage[band, 7].double_R, 4) + "/" + Math.Round(GR_Voltage[band, 7].double_G, 4) + "/" + Math.Round(GR_Voltage[band, 7].double_B, 4), Color.Red);
            f1.GB_Status_AppendText_Nextline("AM1 R/G/B Voltage : " + Math.Round(AM1_R_Voltage, 4) + "/" + Math.Round(AM1_G_Voltage, 4) + "/" + Math.Round(AM1_B_Voltage, 4), Color.Green);
            f1.GB_Status_AppendText_Nextline("AM0 R/G/B Voltage : " + Math.Round(AM0_R_Voltage, 4) + "/" + Math.Round(AM0_G_Voltage, 4) + "/" + Math.Round(AM0_B_Voltage, 4), Color.Blue);
        }



        private static DP173_Model_Option_Form Instance;
        public static DP173_Model_Option_Form getInstance()
        {
            if (Instance == null)
                Instance = new DP173_Model_Option_Form();
            return Instance;
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

        private DP173_Model_Option_Form()
        {
            InitializeComponent();
        }

        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        private void DP173_Model_Option_Form_Load_1(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            BackColor = f1.current_model.Get_Back_Ground_Color();
            RGB_Vdata_Grid_Initalize();
            textBox_B0_DBV_Setting.Text = "7FF";

            string filepath_1 = string.Empty;
            string filepath_2 = string.Empty;
            string filepath_3 = string.Empty;
            string filepath_4 = string.Empty;
            string filepath_5 = string.Empty;
            string filepath_6 = string.Empty;
            f1.Get_Set123456_txt_Path(ref filepath_1, ref filepath_2, ref filepath_3, ref filepath_4, ref filepath_5, ref filepath_6);

            textBox_Mipi_Script_Set1.Text = File.ReadAllText(filepath_1);
            textBox_Mipi_Script_Set2.Text = File.ReadAllText(filepath_2);
            textBox_Mipi_Script_Set3.Text = File.ReadAllText(filepath_3);
            textBox_Mipi_Script_Set4.Text = File.ReadAllText(filepath_4);
            textBox_Mipi_Script_Set5.Text = File.ReadAllText(filepath_5);
            textBox_Mipi_Script_Set6.Text = File.ReadAllText(filepath_6);

            Set_Condition_Mipi_Script_Change(Gamma_Set.Set1);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set2);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set3);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set4);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set5);
            Set_Condition_Mipi_Script_Change(Gamma_Set.Set6);
        }

        private void groupBox15_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Send_AOD_Vinit2_Setting(string[] hex_Vinit2)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP173_Long_Packet_CMD_Send(189, 3, "E3", hex_Vinit2);
        }

        private void Send_Vinit2_Setting(Gamma_Set Set, string[] hex_Vinit2)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Set1
            if (Set == Gamma_Set.Set1)
            {
                f1.DP173_Long_Packet_CMD_Send(87, 11, "E2", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set2)
            {
                f1.DP173_Long_Packet_CMD_Send(146, 11, "E2", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set3)
            {
                f1.DP173_Long_Packet_CMD_Send(205, 11, "E2", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set4)
            {
                f1.DP173_Long_Packet_CMD_Send(24, 11, "E3", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set5)
            {
                f1.DP173_Long_Packet_CMD_Send(83, 11, "E3", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set6)
            {
                f1.DP173_Long_Packet_CMD_Send(142, 11, "E3", hex_Vinit2);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
            }
        }

        private void Send_Lowtemperature_Vinit2_Setting(Gamma_Set Set, string[] hex_Vinit2)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Set1
            if (Set == Gamma_Set.Set1)
            {
                f1.DP173_Long_Packet_CMD_Send(5, 11, "E4", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set2)
            {
                f1.DP173_Long_Packet_CMD_Send(27, 11, "E4", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set3)
            {
                f1.DP173_Long_Packet_CMD_Send(49, 11, "E4", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set4)
            {
                f1.DP173_Long_Packet_CMD_Send(71, 11, "E4", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set5)
            {
                f1.DP173_Long_Packet_CMD_Send(93, 11, "E4", hex_Vinit2);
            }
            else if (Set == Gamma_Set.Set6)
            {
                f1.DP173_Long_Packet_CMD_Send(115, 11, "E4", hex_Vinit2);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
            }
        }

        private void Send_AOD_ELVSS_Setting(string[] hex_ELVSS)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP173_Long_Packet_CMD_Send(195, 3, "E0", hex_ELVSS);
        }

        private void Send_ELVSS_Setting(Gamma_Set Set, string[] hex_ELVSS)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Set1
            if (Set == Gamma_Set.Set1)
            {
                f1.DP173_Long_Packet_CMD_Send(129, 11, "E0", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set2)
            {
                f1.DP173_Long_Packet_CMD_Send(140, 11, "E0", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set3)
            {
                f1.DP173_Long_Packet_CMD_Send(151, 11, "E0", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set4)
            {
                f1.DP173_Long_Packet_CMD_Send(162, 11, "E0", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set5)
            {
                f1.DP173_Long_Packet_CMD_Send(173, 11, "E0", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set6)
            {
                f1.DP173_Long_Packet_CMD_Send(184, 11, "E0", hex_ELVSS);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
            }
        }


        private void Send_Lowtemperature_ELVSS_Setting(Gamma_Set Set, string[] hex_ELVSS)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Set1
            if (Set == Gamma_Set.Set1)
            {
                f1.DP173_Long_Packet_CMD_Send(150, 11, "E4", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set2)
            {
                f1.DP173_Long_Packet_CMD_Send(161, 11, "E4", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set3)
            {
                f1.DP173_Long_Packet_CMD_Send(172, 11, "E4", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set4)
            {
                f1.DP173_Long_Packet_CMD_Send(183, 11, "E4", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set5)
            {
                f1.DP173_Long_Packet_CMD_Send(194, 11, "E4", hex_ELVSS);
            }
            else if (Set == Gamma_Set.Set6)
            {
                f1.DP173_Long_Packet_CMD_Send(205, 11, "E4", hex_ELVSS);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
            }
        }

        private void button_Read_ELVSS_Vinit_Click(object sender, EventArgs e)
        {
            ELVSS_Text_Clear();

            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            f1.MX_OTP_Read(129, 11, "E0"); //OK
            string[] hex_ELVSS_Set1 = new string[11];
            double[] dec_ELVSS_Set1 = new double[11];
            double[] ELVSS_Set1 = new double[11];
            for (int i = 0; i < 11; i++)
            {
                hex_ELVSS_Set1[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS_Set1[i] = Convert.ToDouble(Convert.ToInt32(hex_ELVSS_Set1[i], 16));
                ELVSS_Set1[i] = ((dec_ELVSS_Set1[i] - 30) / 10.0) - 3.1;
            }

            f1.MX_OTP_Read(140, 11, "E0"); //OK
            string[] hex_ELVSS_Set2 = new string[11];
            double[] dec_ELVSS_Set2 = new double[11];
            double[] ELVSS_Set2 = new double[11];
            for (int i = 0; i < 11; i++)
            {
                hex_ELVSS_Set2[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS_Set2[i] = Convert.ToDouble(Convert.ToInt32(hex_ELVSS_Set2[i], 16));
                ELVSS_Set2[i] = ((dec_ELVSS_Set2[i] - 30) / 10.0) - 3.1;
            }

            f1.MX_OTP_Read(151, 11, "E0"); //OK
            string[] hex_ELVSS_Set3 = new string[11];
            double[] dec_ELVSS_Set3 = new double[11];
            double[] ELVSS_Set3 = new double[11];
            for (int i = 0; i < 11; i++)
            {
                hex_ELVSS_Set3[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS_Set3[i] = Convert.ToDouble(Convert.ToInt32(hex_ELVSS_Set3[i], 16));
                ELVSS_Set3[i] = ((dec_ELVSS_Set3[i] - 30) / 10.0) - 3.1;
            }

            f1.MX_OTP_Read(162, 11, "E0"); //Ok
            string[] hex_ELVSS_Set4 = new string[11];
            double[] dec_ELVSS_Set4 = new double[11];
            double[] ELVSS_Set4 = new double[11];
            for (int i = 0; i < 11; i++)
            {
                hex_ELVSS_Set4[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS_Set4[i] = Convert.ToDouble(Convert.ToInt32(hex_ELVSS_Set4[i], 16));
                ELVSS_Set4[i] = ((dec_ELVSS_Set4[i] - 30) / 10.0) - 3.1;
            }

            f1.MX_OTP_Read(173, 11, "E0"); //Ok
            string[] hex_ELVSS_Set5 = new string[11];
            double[] dec_ELVSS_Set5 = new double[11];
            double[] ELVSS_Set5 = new double[11];
            for (int i = 0; i < 11; i++)
            {
                hex_ELVSS_Set5[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS_Set5[i] = Convert.ToDouble(Convert.ToInt32(hex_ELVSS_Set5[i], 16));
                ELVSS_Set5[i] = ((dec_ELVSS_Set5[i] - 30) / 10.0) - 3.1;
            }

            f1.MX_OTP_Read(184, 11, "E0"); //Ok
            string[] hex_ELVSS_Set6 = new string[11];
            double[] dec_ELVSS_Set6 = new double[11];
            double[] ELVSS_Set6 = new double[11];
            for (int i = 0; i < 11; i++)
            {
                hex_ELVSS_Set6[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS_Set6[i] = Convert.ToDouble(Convert.ToInt32(hex_ELVSS_Set6[i], 16));
                ELVSS_Set6[i] = ((dec_ELVSS_Set6[i] - 30) / 10.0) - 3.1;
            }

            textBox_ELVSS_B0_Set1.Text = ELVSS_Set1[0].ToString();
            textBox_ELVSS_B1_Set1.Text = ELVSS_Set1[1].ToString();
            textBox_ELVSS_B2_Set1.Text = ELVSS_Set1[2].ToString();
            textBox_ELVSS_B3_Set1.Text = ELVSS_Set1[3].ToString();
            textBox_ELVSS_B4_Set1.Text = ELVSS_Set1[4].ToString();
            textBox_ELVSS_B5_Set1.Text = ELVSS_Set1[5].ToString();
            textBox_ELVSS_B6_Set1.Text = ELVSS_Set1[6].ToString();
            textBox_ELVSS_B7_Set1.Text = ELVSS_Set1[7].ToString();
            textBox_ELVSS_B8_Set1.Text = ELVSS_Set1[8].ToString();
            textBox_ELVSS_B9_Set1.Text = ELVSS_Set1[9].ToString();
            textBox_ELVSS_B10_Set1.Text = ELVSS_Set1[10].ToString();

            textBox_ELVSS_B0_Set2.Text = ELVSS_Set2[0].ToString();
            textBox_ELVSS_B1_Set2.Text = ELVSS_Set2[1].ToString();
            textBox_ELVSS_B2_Set2.Text = ELVSS_Set2[2].ToString();
            textBox_ELVSS_B3_Set2.Text = ELVSS_Set2[3].ToString();
            textBox_ELVSS_B4_Set2.Text = ELVSS_Set2[4].ToString();
            textBox_ELVSS_B5_Set2.Text = ELVSS_Set2[5].ToString();
            textBox_ELVSS_B6_Set2.Text = ELVSS_Set2[6].ToString();
            textBox_ELVSS_B7_Set2.Text = ELVSS_Set2[7].ToString();
            textBox_ELVSS_B8_Set2.Text = ELVSS_Set2[8].ToString();
            textBox_ELVSS_B9_Set2.Text = ELVSS_Set2[9].ToString();
            textBox_ELVSS_B10_Set2.Text = ELVSS_Set2[10].ToString();

            textBox_ELVSS_B0_Set3.Text = ELVSS_Set3[0].ToString();
            textBox_ELVSS_B1_Set3.Text = ELVSS_Set3[1].ToString();
            textBox_ELVSS_B2_Set3.Text = ELVSS_Set3[2].ToString();
            textBox_ELVSS_B3_Set3.Text = ELVSS_Set3[3].ToString();
            textBox_ELVSS_B4_Set3.Text = ELVSS_Set3[4].ToString();
            textBox_ELVSS_B5_Set3.Text = ELVSS_Set3[5].ToString();
            textBox_ELVSS_B6_Set3.Text = ELVSS_Set3[6].ToString();
            textBox_ELVSS_B7_Set3.Text = ELVSS_Set3[7].ToString();
            textBox_ELVSS_B8_Set3.Text = ELVSS_Set3[8].ToString();
            textBox_ELVSS_B9_Set3.Text = ELVSS_Set3[9].ToString();
            textBox_ELVSS_B10_Set3.Text = ELVSS_Set3[10].ToString();

            textBox_ELVSS_B0_Set4.Text = ELVSS_Set4[0].ToString();
            textBox_ELVSS_B1_Set4.Text = ELVSS_Set4[1].ToString();
            textBox_ELVSS_B2_Set4.Text = ELVSS_Set4[2].ToString();
            textBox_ELVSS_B3_Set4.Text = ELVSS_Set4[3].ToString();
            textBox_ELVSS_B4_Set4.Text = ELVSS_Set4[4].ToString();
            textBox_ELVSS_B5_Set4.Text = ELVSS_Set4[5].ToString();
            textBox_ELVSS_B6_Set4.Text = ELVSS_Set4[6].ToString();
            textBox_ELVSS_B7_Set4.Text = ELVSS_Set4[7].ToString();
            textBox_ELVSS_B8_Set4.Text = ELVSS_Set4[8].ToString();
            textBox_ELVSS_B9_Set4.Text = ELVSS_Set4[9].ToString();
            textBox_ELVSS_B10_Set4.Text = ELVSS_Set4[10].ToString();

            textBox_ELVSS_B0_Set5.Text = ELVSS_Set5[0].ToString();
            textBox_ELVSS_B1_Set5.Text = ELVSS_Set5[1].ToString();
            textBox_ELVSS_B2_Set5.Text = ELVSS_Set5[2].ToString();
            textBox_ELVSS_B3_Set5.Text = ELVSS_Set5[3].ToString();
            textBox_ELVSS_B4_Set5.Text = ELVSS_Set5[4].ToString();
            textBox_ELVSS_B5_Set5.Text = ELVSS_Set5[5].ToString();
            textBox_ELVSS_B6_Set5.Text = ELVSS_Set5[6].ToString();
            textBox_ELVSS_B7_Set5.Text = ELVSS_Set5[7].ToString();
            textBox_ELVSS_B8_Set5.Text = ELVSS_Set5[8].ToString();
            textBox_ELVSS_B9_Set5.Text = ELVSS_Set5[9].ToString();
            textBox_ELVSS_B10_Set5.Text = ELVSS_Set5[10].ToString();

            textBox_ELVSS_B0_Set6.Text = ELVSS_Set6[0].ToString();
            textBox_ELVSS_B1_Set6.Text = ELVSS_Set6[1].ToString();
            textBox_ELVSS_B2_Set6.Text = ELVSS_Set6[2].ToString();
            textBox_ELVSS_B3_Set6.Text = ELVSS_Set6[3].ToString();
            textBox_ELVSS_B4_Set6.Text = ELVSS_Set6[4].ToString();
            textBox_ELVSS_B5_Set6.Text = ELVSS_Set6[5].ToString();
            textBox_ELVSS_B6_Set6.Text = ELVSS_Set6[6].ToString();
            textBox_ELVSS_B7_Set6.Text = ELVSS_Set6[7].ToString();
            textBox_ELVSS_B8_Set6.Text = ELVSS_Set6[8].ToString();
            textBox_ELVSS_B9_Set6.Text = ELVSS_Set6[9].ToString();
            textBox_ELVSS_B10_Set6.Text = ELVSS_Set6[10].ToString();

            //AOD ELVSS Read
            f1.MX_OTP_Read(195, 3, "E0");
            string[] hex_ELVSS = new string[3];
            double[] dec_ELVSS = new double[3];
            double[] ELVSS = new double[3];
            for (int i = 0; i < 3; i++)
            {
                hex_ELVSS[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS[i] = Convert.ToDouble(Convert.ToInt32(hex_ELVSS[i], 16));
                ELVSS[i] = ((dec_ELVSS[i] - 30) / 10.0) - 3.1;
            }
            textBox_ELVSS_A0.Text = ELVSS[0].ToString();
            textBox_ELVSS_A1.Text = ELVSS[1].ToString();
            textBox_ELVSS_A2.Text = ELVSS[2].ToString();

            //Vinit2 Read Set1
            f1.MX_OTP_Read(87, 11, "E2");
            string[] hex_Vinit2_Set1 = new string[11];
            double[] dec_Vinit2_Set1 = new double[11];
            double[] Vinit2_Set1 = new double[11];
            int VINIT2_SEL_Set1 = ((Convert.ToInt32(hex_Vinit2_Set1[0], 16) & 0x40) >> 6);
            for (int i = 0; i < 11; i++)
            {
                hex_Vinit2_Set1[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_Vinit2_Set1[i] = Convert.ToDouble(Convert.ToInt32(hex_Vinit2_Set1[i], 16) & 0x3F);
                if (VINIT2_SEL_Set1 == 1) Vinit2_Set1[i] = (dec_Vinit2_Set1[i] - 2) / 10.0;
                else Vinit2_Set1[i] = (-(dec_Vinit2_Set1[i] - 2) / 10.0);
            }

            //Vinit2 Read Set2
            f1.MX_OTP_Read(146, 11, "E2");
            string[] hex_Vinit2_Set2 = new string[11];
            double[] dec_Vinit2_Set2 = new double[11];
            double[] Vinit2_Set2 = new double[11];
            int VINIT2_SEL_Set2 = ((Convert.ToInt32(hex_Vinit2_Set2[0], 16) & 0x40) >> 6);
            for (int i = 0; i < 11; i++)
            {
                hex_Vinit2_Set2[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_Vinit2_Set2[i] = Convert.ToDouble(Convert.ToInt32(hex_Vinit2_Set2[i], 16) & 0x3F);
                if (VINIT2_SEL_Set2 == 1) Vinit2_Set2[i] = (dec_Vinit2_Set2[i] - 2) / 10.0;
                else Vinit2_Set2[i] = (-(dec_Vinit2_Set2[i] - 2) / 10.0);
            }

            //Vinit2 Read Set3
            f1.MX_OTP_Read(205, 11, "E2");
            string[] hex_Vinit2_Set3 = new string[11];
            double[] dec_Vinit2_Set3 = new double[11];
            double[] Vinit2_Set3 = new double[11];
            int VINIT2_SEL_Set3 = ((Convert.ToInt32(hex_Vinit2_Set3[0], 16) & 0x40) >> 6);
            for (int i = 0; i < 11; i++)
            {
                hex_Vinit2_Set3[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_Vinit2_Set3[i] = Convert.ToDouble(Convert.ToInt32(hex_Vinit2_Set3[i], 16) & 0x3F);
                if (VINIT2_SEL_Set3 == 1) Vinit2_Set3[i] = (dec_Vinit2_Set3[i] - 2) / 10.0;
                else Vinit2_Set3[i] = (-(dec_Vinit2_Set3[i] - 2) / 10.0);
            }

            //Vinit2 Read Set4
            f1.MX_OTP_Read(24, 11, "E3");
            string[] hex_Vinit2_Set4 = new string[11];
            double[] dec_Vinit2_Set4 = new double[11];
            double[] Vinit2_Set4 = new double[11];
            int VINIT2_SEL_Set4 = ((Convert.ToInt32(hex_Vinit2_Set4[0], 16) & 0x40) >> 6);
            for (int i = 0; i < 11; i++)
            {
                hex_Vinit2_Set4[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_Vinit2_Set4[i] = Convert.ToDouble(Convert.ToInt32(hex_Vinit2_Set4[i], 16) & 0x3F);
                if (VINIT2_SEL_Set4 == 1) Vinit2_Set4[i] = (dec_Vinit2_Set4[i] - 2) / 10.0;
                else Vinit2_Set4[i] = (-(dec_Vinit2_Set4[i] - 2) / 10.0);
            }

            //Vinit2 Read Set5
            f1.MX_OTP_Read(83, 11, "E3");
            string[] hex_Vinit2_Set5 = new string[11];
            double[] dec_Vinit2_Set5 = new double[11];
            double[] Vinit2_Set5 = new double[11];
            int VINIT2_SEL_Set5 = ((Convert.ToInt32(hex_Vinit2_Set5[0], 16) & 0x40) >> 6);
            for (int i = 0; i < 11; i++)
            {
                hex_Vinit2_Set5[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_Vinit2_Set5[i] = Convert.ToDouble(Convert.ToInt32(hex_Vinit2_Set5[i], 16) & 0x3F);
                if (VINIT2_SEL_Set5 == 1) Vinit2_Set5[i] = (dec_Vinit2_Set5[i] - 2) / 10.0;
                else Vinit2_Set5[i] = (-(dec_Vinit2_Set5[i] - 2) / 10.0);
            }

            //Vinit2 Read Set6
            f1.MX_OTP_Read(142, 11, "E3");
            string[] hex_Vinit2_Set6 = new string[11];
            double[] dec_Vinit2_Set6 = new double[11];
            double[] Vinit2_Set6 = new double[11];
            int VINIT2_SEL_Set6 = ((Convert.ToInt32(hex_Vinit2_Set6[0], 16) & 0x40) >> 6);
            for (int i = 0; i < 11; i++)
            {
                hex_Vinit2_Set6[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_Vinit2_Set6[i] = Convert.ToDouble(Convert.ToInt32(hex_Vinit2_Set6[i], 16) & 0x3F);
                if (VINIT2_SEL_Set6 == 1) Vinit2_Set6[i] = (dec_Vinit2_Set6[i] - 2) / 10.0;
                else Vinit2_Set6[i] = (-(dec_Vinit2_Set6[i] - 2) / 10.0);
            }

            textBox_Vinit_B0_Set1.Text = Vinit2_Set1[0].ToString();
            textBox_Vinit_B1_Set1.Text = Vinit2_Set1[1].ToString();
            textBox_Vinit_B2_Set1.Text = Vinit2_Set1[2].ToString();
            textBox_Vinit_B3_Set1.Text = Vinit2_Set1[3].ToString();
            textBox_Vinit_B4_Set1.Text = Vinit2_Set1[4].ToString();
            textBox_Vinit_B5_Set1.Text = Vinit2_Set1[5].ToString();
            textBox_Vinit_B6_Set1.Text = Vinit2_Set1[6].ToString();
            textBox_Vinit_B7_Set1.Text = Vinit2_Set1[7].ToString();
            textBox_Vinit_B8_Set1.Text = Vinit2_Set1[8].ToString();
            textBox_Vinit_B9_Set1.Text = Vinit2_Set1[9].ToString();
            textBox_Vinit_B10_Set1.Text = Vinit2_Set1[10].ToString();

            textBox_Vinit_B0_Set2.Text = Vinit2_Set2[0].ToString();
            textBox_Vinit_B1_Set2.Text = Vinit2_Set2[1].ToString();
            textBox_Vinit_B2_Set2.Text = Vinit2_Set2[2].ToString();
            textBox_Vinit_B3_Set2.Text = Vinit2_Set2[3].ToString();
            textBox_Vinit_B4_Set2.Text = Vinit2_Set2[4].ToString();
            textBox_Vinit_B5_Set2.Text = Vinit2_Set2[5].ToString();
            textBox_Vinit_B6_Set2.Text = Vinit2_Set2[6].ToString();
            textBox_Vinit_B7_Set2.Text = Vinit2_Set2[7].ToString();
            textBox_Vinit_B8_Set2.Text = Vinit2_Set2[8].ToString();
            textBox_Vinit_B9_Set2.Text = Vinit2_Set2[9].ToString();
            textBox_Vinit_B10_Set2.Text = Vinit2_Set2[10].ToString();

            textBox_Vinit_B0_Set3.Text = Vinit2_Set3[0].ToString();
            textBox_Vinit_B1_Set3.Text = Vinit2_Set3[1].ToString();
            textBox_Vinit_B2_Set3.Text = Vinit2_Set3[2].ToString();
            textBox_Vinit_B3_Set3.Text = Vinit2_Set3[3].ToString();
            textBox_Vinit_B4_Set3.Text = Vinit2_Set3[4].ToString();
            textBox_Vinit_B5_Set3.Text = Vinit2_Set3[5].ToString();
            textBox_Vinit_B6_Set3.Text = Vinit2_Set3[6].ToString();
            textBox_Vinit_B7_Set3.Text = Vinit2_Set3[7].ToString();
            textBox_Vinit_B8_Set3.Text = Vinit2_Set3[8].ToString();
            textBox_Vinit_B9_Set3.Text = Vinit2_Set3[9].ToString();
            textBox_Vinit_B10_Set3.Text = Vinit2_Set3[10].ToString();

            textBox_Vinit_B0_Set4.Text = Vinit2_Set4[0].ToString();
            textBox_Vinit_B1_Set4.Text = Vinit2_Set4[1].ToString();
            textBox_Vinit_B2_Set4.Text = Vinit2_Set4[2].ToString();
            textBox_Vinit_B3_Set4.Text = Vinit2_Set4[3].ToString();
            textBox_Vinit_B4_Set4.Text = Vinit2_Set4[4].ToString();
            textBox_Vinit_B5_Set4.Text = Vinit2_Set4[5].ToString();
            textBox_Vinit_B6_Set4.Text = Vinit2_Set4[6].ToString();
            textBox_Vinit_B7_Set4.Text = Vinit2_Set4[7].ToString();
            textBox_Vinit_B8_Set4.Text = Vinit2_Set4[8].ToString();
            textBox_Vinit_B9_Set4.Text = Vinit2_Set4[9].ToString();
            textBox_Vinit_B10_Set4.Text = Vinit2_Set4[10].ToString();

            textBox_Vinit_B0_Set5.Text = Vinit2_Set5[0].ToString();
            textBox_Vinit_B1_Set5.Text = Vinit2_Set5[1].ToString();
            textBox_Vinit_B2_Set5.Text = Vinit2_Set5[2].ToString();
            textBox_Vinit_B3_Set5.Text = Vinit2_Set5[3].ToString();
            textBox_Vinit_B4_Set5.Text = Vinit2_Set5[4].ToString();
            textBox_Vinit_B5_Set5.Text = Vinit2_Set5[5].ToString();
            textBox_Vinit_B6_Set5.Text = Vinit2_Set5[6].ToString();
            textBox_Vinit_B7_Set5.Text = Vinit2_Set5[7].ToString();
            textBox_Vinit_B8_Set5.Text = Vinit2_Set5[8].ToString();
            textBox_Vinit_B9_Set5.Text = Vinit2_Set5[9].ToString();
            textBox_Vinit_B10_Set5.Text = Vinit2_Set5[10].ToString();

            textBox_Vinit_B0_Set6.Text = Vinit2_Set6[0].ToString();
            textBox_Vinit_B1_Set6.Text = Vinit2_Set6[1].ToString();
            textBox_Vinit_B2_Set6.Text = Vinit2_Set6[2].ToString();
            textBox_Vinit_B3_Set6.Text = Vinit2_Set6[3].ToString();
            textBox_Vinit_B4_Set6.Text = Vinit2_Set6[4].ToString();
            textBox_Vinit_B5_Set6.Text = Vinit2_Set6[5].ToString();
            textBox_Vinit_B6_Set6.Text = Vinit2_Set6[6].ToString();
            textBox_Vinit_B7_Set6.Text = Vinit2_Set6[7].ToString();
            textBox_Vinit_B8_Set6.Text = Vinit2_Set6[8].ToString();
            textBox_Vinit_B9_Set6.Text = Vinit2_Set6[9].ToString();
            textBox_Vinit_B10_Set6.Text = Vinit2_Set6[10].ToString();

            //AOD VINIT2 Read
            f1.MX_OTP_Read(189, 3, "E3");
            string[] hex_Vinit2 = new string[3];
            double[] dec_Vinit2 = new double[3];
            double[] Vinit2 = new double[3];
            int VINIT2_SEL = ((Convert.ToInt32(hex_Vinit2[0], 16) & 0x40) >> 6);
            for (int i = 0; i < 3; i++)
            {
                hex_Vinit2[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_Vinit2[i] = Convert.ToDouble(Convert.ToInt32(hex_Vinit2[i], 16) & 0x3F);
                if (VINIT2_SEL == 1) Vinit2[i] = (dec_Vinit2[i] - 2) / 10.0;
                else Vinit2[i] = (-(dec_Vinit2[i] - 2) / 10.0);
            }
            textBox_Vinit_A0.Text = Vinit2[0].ToString();
            textBox_Vinit_A1.Text = Vinit2[1].ToString();
            textBox_Vinit_A2.Text = Vinit2[2].ToString();

            //---Low Temperature Read (only when "radioButton_Debug_Status_Mode" is checked)---
            if (f1.radioButton_Debug_Status_Mode.Checked)
            {
                f1.GB_Status_AppendText_Nextline("-----Low Temperature ELVSS----", Color.DarkGreen);
                string Original_ELVSS_Set1 = string.Empty;
                string Low_Temperature_ELVSS_Set1 = string.Empty; f1.MX_OTP_Read(150, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_ELVSS_Set1 += (hex_ELVSS_Set1[i] + " ");
                    Low_Temperature_ELVSS_Set1 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set1)" + Original_ELVSS_Set1, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set1)" + Low_Temperature_ELVSS_Set1, Color.Red);

                string Original_ELVSS_Set2 = string.Empty;
                string Low_Temperature_ELVSS_Set2 = string.Empty; f1.MX_OTP_Read(161, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_ELVSS_Set2 += (hex_ELVSS_Set2[i] + " ");
                    Low_Temperature_ELVSS_Set2 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set2)" + Original_ELVSS_Set2, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set2)" + Low_Temperature_ELVSS_Set2, Color.Red);

                string Original_ELVSS_Set3 = string.Empty;
                string Low_Temperature_ELVSS_Set3 = string.Empty; f1.MX_OTP_Read(172, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_ELVSS_Set3 += (hex_ELVSS_Set3[i] + " ");
                    Low_Temperature_ELVSS_Set3 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set3)" + Original_ELVSS_Set3, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set3)" + Low_Temperature_ELVSS_Set3, Color.Red);

                string Original_ELVSS_Set4 = string.Empty;
                string Low_Temperature_ELVSS_Set4 = string.Empty; f1.MX_OTP_Read(183, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_ELVSS_Set4 += (hex_ELVSS_Set4[i] + " ");
                    Low_Temperature_ELVSS_Set4 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set4)" + Original_ELVSS_Set4, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set4)" + Low_Temperature_ELVSS_Set4, Color.Red);


                string Original_ELVSS_Set5 = string.Empty;
                string Low_Temperature_ELVSS_Set5 = string.Empty; f1.MX_OTP_Read(194, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_ELVSS_Set5 += (hex_ELVSS_Set5[i] + " ");
                    Low_Temperature_ELVSS_Set5 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set5)" + Original_ELVSS_Set5, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set5)" + Low_Temperature_ELVSS_Set5, Color.Red);

                string Original_ELVSS_Set6 = string.Empty;
                string Low_Temperature_ELVSS_Set6 = string.Empty; f1.MX_OTP_Read(205, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_ELVSS_Set6 += (hex_ELVSS_Set6[i] + " ");
                    Low_Temperature_ELVSS_Set6 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set6)" + Original_ELVSS_Set6, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set6)" + Low_Temperature_ELVSS_Set6, Color.Red);


                f1.GB_Status_AppendText_Nextline("-----Low Temperature VINIT2----", Color.DarkGreen);
                string Original_VINIT2_Set1 = string.Empty;
                string Low_Temperature_Vinit2_Set1 = string.Empty; f1.MX_OTP_Read(5, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_VINIT2_Set1 += (hex_Vinit2_Set1[i] + " ");
                    Low_Temperature_Vinit2_Set1 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set1)" + Original_VINIT2_Set1, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set1)" + Low_Temperature_Vinit2_Set1, Color.Red);

                string Original_VINIT2_Set2 = string.Empty;
                string Low_Temperature_Vinit2_Set2 = string.Empty; f1.MX_OTP_Read(27, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_VINIT2_Set2 += (hex_Vinit2_Set2[i] + " ");
                    Low_Temperature_Vinit2_Set2 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set2)" + Original_VINIT2_Set2, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set2)" + Low_Temperature_Vinit2_Set2, Color.Red);

                string Original_VINIT2_Set3 = string.Empty;
                string Low_Temperature_Vinit2_Set3 = string.Empty; f1.MX_OTP_Read(49, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_VINIT2_Set3 += (hex_Vinit2_Set3[i] + " ");
                    Low_Temperature_Vinit2_Set3 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set3)" + Original_VINIT2_Set3, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set3)" + Low_Temperature_Vinit2_Set3, Color.Red);

                string Original_VINIT2_Set4 = string.Empty;
                string Low_Temperature_Vinit2_Set4 = string.Empty; f1.MX_OTP_Read(71, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_VINIT2_Set4 += (hex_Vinit2_Set4[i] + " ");
                    Low_Temperature_Vinit2_Set4 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set4)" + Original_VINIT2_Set4, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set4)" + Low_Temperature_Vinit2_Set4, Color.Red);


                string Original_VINIT2_Set5 = string.Empty;
                string Low_Temperature_Vinit2_Set5 = string.Empty; f1.MX_OTP_Read(93, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_VINIT2_Set5 += (hex_Vinit2_Set5[i] + " ");
                    Low_Temperature_Vinit2_Set5 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set5)" + Original_VINIT2_Set5, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set5)" + Low_Temperature_Vinit2_Set5, Color.Red);

                string Original_VINIT2_Set6 = string.Empty;
                string Low_Temperature_Vinit2_Set6 = string.Empty; f1.MX_OTP_Read(115, 11, "E4");
                for (int i = 0; i < 11; i++)
                {
                    Original_VINIT2_Set6 += (hex_Vinit2_Set6[i] + " ");
                    Low_Temperature_Vinit2_Set6 += (f1.dataGridView1.Rows[i].Cells[1].Value.ToString() + " ");
                }
                f1.GB_Status_AppendText_Nextline("Original Set6)" + Original_VINIT2_Set6, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Lowtemp Set6)" + Low_Temperature_Vinit2_Set6, Color.Red);
            }
        }

        private void Show_Band_Set_RGB_AM0_Voltage(int band, Gamma_Set set, double Vreg1_voltage, double VGEG1_REF2047)
        {
            double AM0_Resolution = (VGEG1_REF2047 - Vreg1_voltage) / 700;
            string Band_Register = DP173.Get_Gamma_Register_Hex_String(band).Remove(0, 2);
            int Offset = 0;
            Color color = Color.Black;
            if (set == Gamma_Set.Set1)
            {
                Offset = 0;
                color = Color.Red;
            }
            else if (set == Gamma_Set.Set2)
            {
                Offset = 40;
                color = Color.Green;
            }
            else if (set == Gamma_Set.Set3)
            {
                Offset = 80;
                color = Color.Blue;
            }
            else if (set == Gamma_Set.Set4)
            {
                Offset = 120;
                color = Color.Purple;
            }
            else if (set == Gamma_Set.Set5)
            {
                Offset = 160;
                color = Color.Black;
            }
            else if (set == Gamma_Set.Set6)
            {
                Offset = 200;
                color = Color.Black;
            }
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(Offset, 40, Band_Register); ;
            string[] Hex = new string[40];
            for (int i = 0; i < 40; i++) Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            string HBM_AM0_Hex_R = f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
            string HBM_AM0_Hex_G = f1.dataGridView1.Rows[18].Cells[1].Value.ToString();
            string HBM_AM0_Hex_B = f1.dataGridView1.Rows[31].Cells[1].Value.ToString();
            int Dec_AM_R = Convert.ToInt32(HBM_AM0_Hex_R, 16); if (Dec_AM_R > 127) Dec_AM_R = 127;
            int Dec_AM_G = Convert.ToInt32(HBM_AM0_Hex_G, 16); if (Dec_AM_G > 127) Dec_AM_G = 127;
            int Dec_AM_B = Convert.ToInt32(HBM_AM0_Hex_B, 16); if (Dec_AM_B > 127) Dec_AM_B = 127;
            double Voltage_AM0_R = VGEG1_REF2047 - AM0_Resolution * Dec_AM_R;
            double Voltage_AM0_G = VGEG1_REF2047 - AM0_Resolution * Dec_AM_G;
            double Voltage_AM0_B = VGEG1_REF2047 - AM0_Resolution * Dec_AM_B;
            f1.GB_Status_AppendText_Nextline("Band" + band.ToString() + " Set" + set.ToString() + " Vreg1_voltage/VGEG1_REF2047/AM0_Resolution : " + Vreg1_voltage.ToString() + "/" + VGEG1_REF2047.ToString() + "/" + AM0_Resolution.ToString(), color);
            f1.GB_Status_AppendText_Nextline("Band" + band.ToString() + " Set" + set.ToString() + " R/G/B_AM0 Hex : 0x" + HBM_AM0_Hex_R + "/0x" + HBM_AM0_Hex_G + "/0x" + HBM_AM0_Hex_B, color);
            f1.GB_Status_AppendText_Nextline("Band" + band.ToString() + " Set" + set.ToString() + " R/G/B_AM0 voltage : " + Voltage_AM0_R.ToString() + "/" + Voltage_AM0_G.ToString() + "/" + Voltage_AM0_B.ToString(), color);
        }





        private void Vreg1_Text_Clear()
        {
            //REF
            VREG1_REF1.Text = String.Empty;
            VREG1_REF63.Text = String.Empty;
            VREG1_REF407.Text = String.Empty;
            VREG1_REF815.Text = String.Empty;
            VREG1_REF1227.Text = String.Empty;
            VREG1_REF1635.Text = String.Empty;
            VREG1_REF2047.Text = String.Empty;

            //Set1
            textBox_Vreg1_B0_1.Text = String.Empty;
            textBox_Vreg1_B1_1.Text = String.Empty;
            textBox_Vreg1_B2_1.Text = String.Empty;
            textBox_Vreg1_B3_1.Text = String.Empty;
            textBox_Vreg1_B4_1.Text = String.Empty;
            textBox_Vreg1_B5_1.Text = String.Empty;
            textBox_Vreg1_B6_1.Text = String.Empty;
            textBox_Vreg1_B7_1.Text = String.Empty;
            textBox_Vreg1_B8_1.Text = String.Empty;
            textBox_Vreg1_B9_1.Text = String.Empty;
            textBox_Vreg1_B10_1.Text = String.Empty;
            textBox_Vreg1_B0_1_volt.Text = String.Empty;
            textBox_Vreg1_B1_1_volt.Text = String.Empty;
            textBox_Vreg1_B2_1_volt.Text = String.Empty;
            textBox_Vreg1_B3_1_volt.Text = String.Empty;
            textBox_Vreg1_B4_1_volt.Text = String.Empty;
            textBox_Vreg1_B5_1_volt.Text = String.Empty;
            textBox_Vreg1_B6_1_volt.Text = String.Empty;
            textBox_Vreg1_B7_1_volt.Text = String.Empty;
            textBox_Vreg1_B8_1_volt.Text = String.Empty;
            textBox_Vreg1_B9_1_volt.Text = String.Empty;
            textBox_Vreg1_B10_1_volt.Text = String.Empty;

            //Set2
            textBox_Vreg1_B0_2.Text = String.Empty;
            textBox_Vreg1_B1_2.Text = String.Empty;
            textBox_Vreg1_B2_2.Text = String.Empty;
            textBox_Vreg1_B3_2.Text = String.Empty;
            textBox_Vreg1_B4_2.Text = String.Empty;
            textBox_Vreg1_B5_2.Text = String.Empty;
            textBox_Vreg1_B6_2.Text = String.Empty;
            textBox_Vreg1_B7_2.Text = String.Empty;
            textBox_Vreg1_B8_2.Text = String.Empty;
            textBox_Vreg1_B9_2.Text = String.Empty;
            textBox_Vreg1_B10_2.Text = String.Empty;
            textBox_Vreg1_B0_2_volt.Text = String.Empty;
            textBox_Vreg1_B1_2_volt.Text = String.Empty;
            textBox_Vreg1_B2_2_volt.Text = String.Empty;
            textBox_Vreg1_B3_2_volt.Text = String.Empty;
            textBox_Vreg1_B4_2_volt.Text = String.Empty;
            textBox_Vreg1_B5_2_volt.Text = String.Empty;
            textBox_Vreg1_B6_2_volt.Text = String.Empty;
            textBox_Vreg1_B7_2_volt.Text = String.Empty;
            textBox_Vreg1_B8_2_volt.Text = String.Empty;
            textBox_Vreg1_B9_2_volt.Text = String.Empty;
            textBox_Vreg1_B10_2_volt.Text = String.Empty;

            //Set3
            textBox_Vreg1_B0_3.Text = String.Empty;
            textBox_Vreg1_B1_3.Text = String.Empty;
            textBox_Vreg1_B2_3.Text = String.Empty;
            textBox_Vreg1_B3_3.Text = String.Empty;
            textBox_Vreg1_B4_3.Text = String.Empty;
            textBox_Vreg1_B5_3.Text = String.Empty;
            textBox_Vreg1_B6_3.Text = String.Empty;
            textBox_Vreg1_B7_3.Text = String.Empty;
            textBox_Vreg1_B8_3.Text = String.Empty;
            textBox_Vreg1_B9_3.Text = String.Empty;
            textBox_Vreg1_B10_3.Text = String.Empty;
            textBox_Vreg1_B0_3_volt.Text = String.Empty;
            textBox_Vreg1_B1_3_volt.Text = String.Empty;
            textBox_Vreg1_B2_3_volt.Text = String.Empty;
            textBox_Vreg1_B3_3_volt.Text = String.Empty;
            textBox_Vreg1_B4_3_volt.Text = String.Empty;
            textBox_Vreg1_B5_3_volt.Text = String.Empty;
            textBox_Vreg1_B6_3_volt.Text = String.Empty;
            textBox_Vreg1_B7_3_volt.Text = String.Empty;
            textBox_Vreg1_B8_3_volt.Text = String.Empty;
            textBox_Vreg1_B9_3_volt.Text = String.Empty;
            textBox_Vreg1_B10_3_volt.Text = String.Empty;

            //Set4
            textBox_Vreg1_B0_4.Text = String.Empty;
            textBox_Vreg1_B1_4.Text = String.Empty;
            textBox_Vreg1_B2_4.Text = String.Empty;
            textBox_Vreg1_B3_4.Text = String.Empty;
            textBox_Vreg1_B4_4.Text = String.Empty;
            textBox_Vreg1_B5_4.Text = String.Empty;
            textBox_Vreg1_B6_4.Text = String.Empty;
            textBox_Vreg1_B7_4.Text = String.Empty;
            textBox_Vreg1_B8_4.Text = String.Empty;
            textBox_Vreg1_B9_4.Text = String.Empty;
            textBox_Vreg1_B10_4.Text = String.Empty;
            textBox_Vreg1_B0_4_volt.Text = String.Empty;
            textBox_Vreg1_B1_4_volt.Text = String.Empty;
            textBox_Vreg1_B2_4_volt.Text = String.Empty;
            textBox_Vreg1_B3_4_volt.Text = String.Empty;
            textBox_Vreg1_B4_4_volt.Text = String.Empty;
            textBox_Vreg1_B5_4_volt.Text = String.Empty;
            textBox_Vreg1_B6_4_volt.Text = String.Empty;
            textBox_Vreg1_B7_4_volt.Text = String.Empty;
            textBox_Vreg1_B8_4_volt.Text = String.Empty;
            textBox_Vreg1_B9_4_volt.Text = String.Empty;
            textBox_Vreg1_B10_4_volt.Text = String.Empty;

            //Set5
            textBox_Vreg1_B0_5.Text = String.Empty;
            textBox_Vreg1_B1_5.Text = String.Empty;
            textBox_Vreg1_B2_5.Text = String.Empty;
            textBox_Vreg1_B3_5.Text = String.Empty;
            textBox_Vreg1_B4_5.Text = String.Empty;
            textBox_Vreg1_B5_5.Text = String.Empty;
            textBox_Vreg1_B6_5.Text = String.Empty;
            textBox_Vreg1_B7_5.Text = String.Empty;
            textBox_Vreg1_B8_5.Text = String.Empty;
            textBox_Vreg1_B9_5.Text = String.Empty;
            textBox_Vreg1_B10_5.Text = String.Empty;

            //Set6
            textBox_Vreg1_B0_6.Text = String.Empty;
            textBox_Vreg1_B1_6.Text = String.Empty;
            textBox_Vreg1_B2_6.Text = String.Empty;
            textBox_Vreg1_B3_6.Text = String.Empty;
            textBox_Vreg1_B4_6.Text = String.Empty;
            textBox_Vreg1_B5_6.Text = String.Empty;
            textBox_Vreg1_B6_6.Text = String.Empty;
            textBox_Vreg1_B7_6.Text = String.Empty;
            textBox_Vreg1_B8_6.Text = String.Empty;
            textBox_Vreg1_B9_6.Text = String.Empty;
            textBox_Vreg1_B10_6.Text = String.Empty;

            //AOD
            textBox_Vreg1_A0.Text = String.Empty;
            textBox_Vreg1_A1.Text = String.Empty;
            textBox_Vreg1_A2.Text = String.Empty;
            textBox_Vreg1_A0_volt.Text = String.Empty;
            textBox_Vreg1_A1_volt.Text = String.Empty;
            textBox_Vreg1_A2_volt.Text = String.Empty;

            Application.DoEvents();
        }

        private void button_Vreg1_Read_Click(object sender, EventArgs e)
        {
            Vreg1_Text_Clear();

            //VREG1_REF1~2047 
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(21, 7, "B1"); ;
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF63 = f1.dataGridView1.Rows[5].Cells[1].Value.ToString(); //[5:0] (6bit)
            string Hex_VREG1_REF407 = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF815 = f1.dataGridView1.Rows[3].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF1227 = f1.dataGridView1.Rows[2].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF1635 = f1.dataGridView1.Rows[1].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)

            int Dec_VREG1_REF1 = Convert.ToInt32(Hex_VREG1_REF1, 16) & 0x7F;
            int Dec_VREG1_REF63 = Convert.ToInt32(Hex_VREG1_REF63, 16) & 0x3F;
            int Dec_VREG1_REF407 = Convert.ToInt32(Hex_VREG1_REF407, 16) & 0x3F;
            int Dec_VREG1_REF815 = Convert.ToInt32(Hex_VREG1_REF815, 16) & 0x3F;
            int Dec_VREG1_REF1227 = Convert.ToInt32(Hex_VREG1_REF1227, 16) & 0x3F;
            int Dec_VREG1_REF1635 = Convert.ToInt32(Hex_VREG1_REF1635, 16) & 0x3F;
            int Dec_VREG1_REF2047 = Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F;

            Imported_my_cpp_dll.Get_REF_Voltages(Dec_VREG1_REF2047, Dec_VREG1_REF1635, Dec_VREG1_REF1227, Dec_VREG1_REF815, Dec_VREG1_REF407, Dec_VREG1_REF63, Dec_VREG1_REF1, ref Voltage_VREG1_REF2047, ref  Voltage_VREG1_REF1635, ref  Voltage_VREG1_REF1227, ref  Voltage_VREG1_REF815, ref  Voltage_VREG1_REF407, ref  Voltage_VREG1_REF63, ref  Voltage_VREG1_REF1);
            f1.GB_Status_AppendText_Nextline("-----update REF1 Voltage-----" + Voltage_VREG1_REF1.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Voltage_VREG1_REF1 : " + Voltage_VREG1_REF1.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Voltage_VREG1_REF63 : " + Voltage_VREG1_REF63.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Voltage_VREG1_REF407 : " + Voltage_VREG1_REF407.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Voltage_VREG1_REF815 : " + Voltage_VREG1_REF815.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Voltage_VREG1_REF1227 : " + Voltage_VREG1_REF1227.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Voltage_VREG1_REF1635 : " + Voltage_VREG1_REF1635.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Voltage_VREG1_REF2047 : " + Voltage_VREG1_REF2047.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("-----------------------------" + Voltage_VREG1_REF1.ToString(), Color.Red);

            VREG1_REF1.Text = Voltage_VREG1_REF1.ToString();
            VREG1_REF63.Text = Voltage_VREG1_REF63.ToString();
            VREG1_REF407.Text = Voltage_VREG1_REF407.ToString();
            VREG1_REF815.Text = Voltage_VREG1_REF815.ToString();
            VREG1_REF1227.Text = Voltage_VREG1_REF1227.ToString();
            VREG1_REF1635.Text = Voltage_VREG1_REF1635.ToString();
            VREG1_REF2047.Text = Voltage_VREG1_REF2047.ToString();

            //Get Dec Vreg1 (Set1)
            f1.MX_OTP_Read(43, 17, "B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set1[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B1_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B2_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B3_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B4_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B5_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B6_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B7_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B8_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B9_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10_1.Text = Convert.ToInt32((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();

            //Get Dec Vreg1 (Set2)
            f1.MX_OTP_Read(60, 17, "B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set2[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B1_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B2_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B3_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B4_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B5_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B6_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B7_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B8_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B9_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10_2.Text = Convert.ToInt32((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();

            //Get Dec Vreg1 (Set3)
            f1.MX_OTP_Read(77, 17, "B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set3[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B1_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B2_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B3_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B4_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B5_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B6_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B7_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B8_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B9_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10_3.Text = Convert.ToInt32((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();

            //Get Dec Vreg1 (Set4)
            f1.MX_OTP_Read(94, 17, "B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set4[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B1_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B2_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B3_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B4_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B5_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B6_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B7_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B8_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B9_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10_4.Text = Convert.ToInt32((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();



            //Get Dec Vreg1 (Set5)
            f1.MX_OTP_Read(111, 17, "B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set5[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B1_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B2_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B3_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B4_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B5_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B6_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B7_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B8_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B9_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10_5.Text = Convert.ToInt32((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();


            //Get Dec Vreg1 (Set6)
            f1.MX_OTP_Read(128, 17, "B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set6[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B1_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B2_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B3_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B4_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B5_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B6_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B7_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B8_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B9_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10_6.Text = Convert.ToInt32((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();


            //Get Dec Vreg1 ( AOD)
            f1.MX_OTP_Read(2, 7, "B2");
            Thread.Sleep(100);
            textBox_Vreg1_A0.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[4].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_A1.Text = Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[5].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_A2.Text = Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16).ToString();


            //Get Voltage (Set1)
            textBox_Vreg1_B0_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B0_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B1_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B1_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B2_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B2_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B3_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B3_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B4_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B4_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B5_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B5_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B6_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B6_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B7_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B7_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B8_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B8_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B9_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B9_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B10_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B10_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();

            //Get Voltage (Set2)
            textBox_Vreg1_B0_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B0_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B1_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B1_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B2_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B2_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B3_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B3_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B4_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B4_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B5_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B5_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B6_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B6_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B7_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B7_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B8_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B8_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B9_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B9_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B10_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B10_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();


            //Get Voltage (Set3)
            textBox_Vreg1_B0_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B0_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B1_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B1_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B2_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B2_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B3_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B3_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B4_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B4_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B5_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B5_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B6_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B6_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B7_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B7_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B8_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B8_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B9_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B9_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B10_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B10_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();

            //Get Voltage (Set4)
            textBox_Vreg1_B0_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B0_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B1_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B1_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B2_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B2_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B3_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B3_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B4_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B4_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B5_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B5_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B6_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B6_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B7_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B7_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B8_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B8_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B9_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B9_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_B10_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B10_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();

            //Get Voltage ( AOD)
            textBox_Vreg1_A0_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_A0.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_A1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_A1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
            textBox_Vreg1_A2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_A2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
        }

        private int Get_Normal_Vreg1(Gamma_Set Set, int band)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (Set == Gamma_Set.Set1) f1.MX_OTP_Read(43, 17, "B1");
            else if (Set == Gamma_Set.Set2) f1.MX_OTP_Read(60, 17, "B1");
            else if (Set == Gamma_Set.Set3) f1.MX_OTP_Read(77, 17, "B1");
            else if (Set == Gamma_Set.Set4) f1.MX_OTP_Read(94, 17, "B1");
            else if (Set == Gamma_Set.Set5) f1.MX_OTP_Read(111, 17, "B1");
            else if (Set == Gamma_Set.Set6) f1.MX_OTP_Read(128, 17, "B1");
            Thread.Sleep(200);

            string[] B1_Vreg1_Gamma = new string[17];

            switch (band)
            {
                case 0:
                    return Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16);
                case 1:
                    return Convert.ToInt32((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()), 16);
                case 2:
                    return Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()), 16);
                case 3:
                    return Convert.ToInt32((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()), 16);
                case 4:
                    return Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()), 16);
                case 5:
                    return Convert.ToInt32((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()), 16);
                case 6:
                    return Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()), 16);
                case 7:
                    return Convert.ToInt32((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()), 16);
                case 8:
                    return Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()), 16);
                case 9:
                    return Convert.ToInt32((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16);
                case 10:
                    return Convert.ToInt32((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16);
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of (Normal)boundary");
                    return 0;
            }
        }

        private void DP173_Model_Option_Form_Load(object sender, EventArgs e)
        {

        }

        public void DP173_DBV_Setting(int band)
        {
            switch (band)
            {
                case 0:
                    button_B0_DBV_Send.PerformClick();
                    break;
                case 1:
                    button_B1_DBV_Send.PerformClick();
                    break;
                case 2:
                    button_B2_DBV_Send.PerformClick();
                    break;
                case 3:
                    button_B3_DBV_Send.PerformClick();
                    break;
                case 4:
                    button_B4_DBV_Send.PerformClick();
                    break;
                case 5:
                    button_B5_DBV_Send.PerformClick();
                    break;
                case 6:
                    button_B6_DBV_Send.PerformClick();
                    break;
                case 7:
                    button_B7_DBV_Send.PerformClick();
                    break;
                case 8:
                    button_B8_DBV_Send.PerformClick();
                    break;
                case 9:
                    button_B9_DBV_Send.PerformClick();
                    break;
                case 10:
                    button_B10_DBV_Send.PerformClick();
                    break;
                case 11:
                    button_A0_DBV_Send.PerformClick();
                    break;
                case 12:
                    button_A1_DBV_Send.PerformClick();
                    break;
                case 13:
                    button_A2_DBV_Send.PerformClick();
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                    break;
            }
            Thread.Sleep(100);
        }

        private void ELVSS_Text_Clear()
        {

            //ELVSS Set1
            textBox_ELVSS_B0_Set1.Text = string.Empty;
            textBox_ELVSS_B1_Set1.Text = string.Empty;
            textBox_ELVSS_B2_Set1.Text = string.Empty;
            textBox_ELVSS_B3_Set1.Text = string.Empty;
            textBox_ELVSS_B4_Set1.Text = string.Empty;
            textBox_ELVSS_B5_Set1.Text = string.Empty;
            textBox_ELVSS_B6_Set1.Text = string.Empty;
            textBox_ELVSS_B7_Set1.Text = string.Empty;
            textBox_ELVSS_B8_Set1.Text = string.Empty;
            textBox_ELVSS_B9_Set1.Text = string.Empty;
            textBox_ELVSS_B10_Set1.Text = string.Empty;
            //ELVSS Set2
            textBox_ELVSS_B0_Set2.Text = string.Empty;
            textBox_ELVSS_B1_Set2.Text = string.Empty;
            textBox_ELVSS_B2_Set2.Text = string.Empty;
            textBox_ELVSS_B3_Set2.Text = string.Empty;
            textBox_ELVSS_B4_Set2.Text = string.Empty;
            textBox_ELVSS_B5_Set2.Text = string.Empty;
            textBox_ELVSS_B6_Set2.Text = string.Empty;
            textBox_ELVSS_B7_Set2.Text = string.Empty;
            textBox_ELVSS_B8_Set2.Text = string.Empty;
            textBox_ELVSS_B9_Set2.Text = string.Empty;
            textBox_ELVSS_B10_Set2.Text = string.Empty;
            //ELVSS Set3
            textBox_ELVSS_B0_Set3.Text = string.Empty;
            textBox_ELVSS_B1_Set3.Text = string.Empty;
            textBox_ELVSS_B2_Set3.Text = string.Empty;
            textBox_ELVSS_B3_Set3.Text = string.Empty;
            textBox_ELVSS_B4_Set3.Text = string.Empty;
            textBox_ELVSS_B5_Set3.Text = string.Empty;
            textBox_ELVSS_B6_Set3.Text = string.Empty;
            textBox_ELVSS_B7_Set3.Text = string.Empty;
            textBox_ELVSS_B8_Set3.Text = string.Empty;
            textBox_ELVSS_B9_Set3.Text = string.Empty;
            textBox_ELVSS_B10_Set3.Text = string.Empty;
            //ELVSS Set4
            textBox_ELVSS_B0_Set4.Text = string.Empty;
            textBox_ELVSS_B1_Set4.Text = string.Empty;
            textBox_ELVSS_B2_Set4.Text = string.Empty;
            textBox_ELVSS_B3_Set4.Text = string.Empty;
            textBox_ELVSS_B4_Set4.Text = string.Empty;
            textBox_ELVSS_B5_Set4.Text = string.Empty;
            textBox_ELVSS_B6_Set4.Text = string.Empty;
            textBox_ELVSS_B7_Set4.Text = string.Empty;
            textBox_ELVSS_B8_Set4.Text = string.Empty;
            textBox_ELVSS_B9_Set4.Text = string.Empty;
            textBox_ELVSS_B10_Set4.Text = string.Empty;
            //ELVSS Set5
            textBox_ELVSS_B0_Set5.Text = string.Empty;
            textBox_ELVSS_B1_Set5.Text = string.Empty;
            textBox_ELVSS_B2_Set5.Text = string.Empty;
            textBox_ELVSS_B3_Set5.Text = string.Empty;
            textBox_ELVSS_B4_Set5.Text = string.Empty;
            textBox_ELVSS_B5_Set5.Text = string.Empty;
            textBox_ELVSS_B6_Set5.Text = string.Empty;
            textBox_ELVSS_B7_Set5.Text = string.Empty;
            textBox_ELVSS_B8_Set5.Text = string.Empty;
            textBox_ELVSS_B9_Set5.Text = string.Empty;
            textBox_ELVSS_B10_Set5.Text = string.Empty;
            //ELVSS Set6
            textBox_ELVSS_B0_Set6.Text = string.Empty;
            textBox_ELVSS_B1_Set6.Text = string.Empty;
            textBox_ELVSS_B2_Set6.Text = string.Empty;
            textBox_ELVSS_B3_Set6.Text = string.Empty;
            textBox_ELVSS_B4_Set6.Text = string.Empty;
            textBox_ELVSS_B5_Set6.Text = string.Empty;
            textBox_ELVSS_B6_Set6.Text = string.Empty;
            textBox_ELVSS_B7_Set6.Text = string.Empty;
            textBox_ELVSS_B8_Set6.Text = string.Empty;
            textBox_ELVSS_B9_Set6.Text = string.Empty;
            textBox_ELVSS_B10_Set6.Text = string.Empty;
            //ELVSS AOD
            textBox_ELVSS_A0.Text = string.Empty;
            textBox_ELVSS_A1.Text = string.Empty;
            textBox_ELVSS_A2.Text = string.Empty;

            //Vinit Set1
            textBox_Vinit_B0_Set1.Text = string.Empty;
            textBox_Vinit_B1_Set1.Text = string.Empty;
            textBox_Vinit_B2_Set1.Text = string.Empty;
            textBox_Vinit_B3_Set1.Text = string.Empty;
            textBox_Vinit_B4_Set1.Text = string.Empty;
            textBox_Vinit_B5_Set1.Text = string.Empty;
            textBox_Vinit_B6_Set1.Text = string.Empty;
            textBox_Vinit_B7_Set1.Text = string.Empty;
            textBox_Vinit_B8_Set1.Text = string.Empty;
            textBox_Vinit_B9_Set1.Text = string.Empty;
            textBox_Vinit_B10_Set1.Text = string.Empty;
            //Vinit Set2
            textBox_Vinit_B0_Set2.Text = string.Empty;
            textBox_Vinit_B1_Set2.Text = string.Empty;
            textBox_Vinit_B2_Set2.Text = string.Empty;
            textBox_Vinit_B3_Set2.Text = string.Empty;
            textBox_Vinit_B4_Set2.Text = string.Empty;
            textBox_Vinit_B5_Set2.Text = string.Empty;
            textBox_Vinit_B6_Set2.Text = string.Empty;
            textBox_Vinit_B7_Set2.Text = string.Empty;
            textBox_Vinit_B8_Set2.Text = string.Empty;
            textBox_Vinit_B9_Set2.Text = string.Empty;
            textBox_Vinit_B10_Set2.Text = string.Empty;
            //Vinit Set3
            textBox_Vinit_B0_Set3.Text = string.Empty;
            textBox_Vinit_B1_Set3.Text = string.Empty;
            textBox_Vinit_B2_Set3.Text = string.Empty;
            textBox_Vinit_B3_Set3.Text = string.Empty;
            textBox_Vinit_B4_Set3.Text = string.Empty;
            textBox_Vinit_B5_Set3.Text = string.Empty;
            textBox_Vinit_B6_Set3.Text = string.Empty;
            textBox_Vinit_B7_Set3.Text = string.Empty;
            textBox_Vinit_B8_Set3.Text = string.Empty;
            textBox_Vinit_B9_Set3.Text = string.Empty;
            textBox_Vinit_B10_Set3.Text = string.Empty;
            //Vinit Set4
            textBox_Vinit_B0_Set4.Text = string.Empty;
            textBox_Vinit_B1_Set4.Text = string.Empty;
            textBox_Vinit_B2_Set4.Text = string.Empty;
            textBox_Vinit_B3_Set4.Text = string.Empty;
            textBox_Vinit_B4_Set4.Text = string.Empty;
            textBox_Vinit_B5_Set4.Text = string.Empty;
            textBox_Vinit_B6_Set4.Text = string.Empty;
            textBox_Vinit_B7_Set4.Text = string.Empty;
            textBox_Vinit_B8_Set4.Text = string.Empty;
            textBox_Vinit_B9_Set4.Text = string.Empty;
            textBox_Vinit_B10_Set4.Text = string.Empty;
            //Vinit Set5
            textBox_Vinit_B0_Set5.Text = string.Empty;
            textBox_Vinit_B1_Set5.Text = string.Empty;
            textBox_Vinit_B2_Set5.Text = string.Empty;
            textBox_Vinit_B3_Set5.Text = string.Empty;
            textBox_Vinit_B4_Set5.Text = string.Empty;
            textBox_Vinit_B5_Set5.Text = string.Empty;
            textBox_Vinit_B6_Set5.Text = string.Empty;
            textBox_Vinit_B7_Set5.Text = string.Empty;
            textBox_Vinit_B8_Set5.Text = string.Empty;
            textBox_Vinit_B9_Set5.Text = string.Empty;
            textBox_Vinit_B10_Set5.Text = string.Empty;
            //Vinit Set6
            textBox_Vinit_B0_Set6.Text = string.Empty;
            textBox_Vinit_B1_Set6.Text = string.Empty;
            textBox_Vinit_B2_Set6.Text = string.Empty;
            textBox_Vinit_B3_Set6.Text = string.Empty;
            textBox_Vinit_B4_Set6.Text = string.Empty;
            textBox_Vinit_B5_Set6.Text = string.Empty;
            textBox_Vinit_B6_Set6.Text = string.Empty;
            textBox_Vinit_B7_Set6.Text = string.Empty;
            textBox_Vinit_B8_Set6.Text = string.Empty;
            textBox_Vinit_B9_Set6.Text = string.Empty;
            textBox_Vinit_B10_Set6.Text = string.Empty;
            //Vinit AOD
            textBox_Vinit_A0.Text = string.Empty;
            textBox_Vinit_A1.Text = string.Empty;
            textBox_Vinit_A2.Text = string.Empty;
        }

        private void Elgin_UI_Setting_For_Dual_Compensation()
        {
            checkBox_Dual_Mode_Gamma_Copy_Set1_to_Set3.Checked = true;
            checkBox_Dual_Mode_Gamma_Copy_Set2_to_Set4.Checked = true;
            checkBox_Dual_Mode_Gamma_Copy_Set1_to_Set3.Enabled = false;
            checkBox_Dual_Mode_Gamma_Copy_Set2_to_Set4.Enabled = false;

            checkBox_Dual_Mode_Set3_OC_Skip.Checked = false;
            checkBox_Dual_Mode_Set4_OC_Skip.Checked = false;
            checkBox_Dual_Mode_Set3_OC_Skip.Enabled = false;
            checkBox_Dual_Mode_Set4_OC_Skip.Enabled = false;

            numericUpDown_Dual_Set346_OC_Skip_Last_Band.Value = 10;
            numericUpDown_Dual_Set346_OC_Skip_Last_Band.Enabled = false;
        }

        private void DP173_UI_Setting_For_Dual_Compensation()
        {
            checkBox_Dual_Mode_Gamma_Copy_Set1_to_Set3.Enabled = true;
            checkBox_Dual_Mode_Gamma_Copy_Set2_to_Set4.Enabled = true;

            checkBox_Dual_Mode_Set3_OC_Skip.Enabled = true;
            checkBox_Dual_Mode_Set4_OC_Skip.Enabled = true;

            numericUpDown_Dual_Set346_OC_Skip_Last_Band.Value = 3;
            numericUpDown_Dual_Set346_OC_Skip_Last_Band.Enabled = true;
        }

        private void GR1_Enable_Option_Setting()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (radioButton_GR1_EN_False.Checked)
            {
                f1.DP173_One_Param_CMD_Send(17, "E2", "30");//GR1_EN = false
            }
            else if (radioButton_GR1_EN_False_GR1_Calculation.Checked)
            {
                f1.DP173_One_Param_CMD_Send(17, "E2", "30");//GR1_EN = false
                button_Vreg1_Read.PerformClick();
            }
        }

        private void OC_Initialize()
        {
            Vreg1_Text_Clear();
            ELVSS_Text_Clear();
            textBox_Total_Average_Meas_Count.Text = "0";
            GR1_Enable_Option_Setting();
        }

        private void Optic_compensation_Start_Click(object sender, EventArgs e)
        {
            OC_Initialize();
            
            if (Is_Single_Mode())
                Single_Mode_Optic_compensation();

            else if (radioButton_Dual_Mode.Checked)
                Dual_Mode_Optic_Compensation();

            else if (radioButton_Triple_Mode.Checked)
                Triple_Mode_Optic_Compensation();
        }

        private bool Is_Single_Mode()
        {
            return (radioButton_Single_Mode_Set1.Checked || radioButton_Single_Mode_Set2.Checked
                || radioButton_Single_Mode_Set3.Checked || radioButton_Single_Mode_Set4.Checked
                || radioButton_Single_Mode_Set5.Checked || radioButton_Single_Mode_Set6.Checked);
        }


        private void Set3_Optic_Compensation(bool[] Set3_Skip_Band, OC_Single_Dual_Triple mode, Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal)
        {
            Gamma_Set Set = Gamma_Set.Set3;
            Set_Condition_Mipi_Script_Change(Set);
            button_Gamma_Set3_Apply.PerformClick();

            Dual_Or_Triple_Single_Compensation(Set, Set3_Skip_Band, mode, initial_RGBVreg1_cal);
        }

        private void Set4_Optic_Compensation(bool[] Set4_Skip_Band, OC_Single_Dual_Triple mode, Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal)
        {
            Gamma_Set Set = Gamma_Set.Set4;
            Set_Condition_Mipi_Script_Change(Set);
            button_Gamma_Set4_Apply.PerformClick();

            Dual_Or_Triple_Single_Compensation(Set, Set4_Skip_Band, mode, initial_RGBVreg1_cal);
        }

        private void Dual_Set5_Optic_Compensation(bool[] Set6_Skip_Band)
        {
            Gamma_Set Set = Gamma_Set.Set5;
            Set_Condition_Mipi_Script_Change(Set);
            button_Gamma_Set5_Apply.PerformClick();

            Dual_Set5_Compensation(Set6_Skip_Band);
        }

        private void Set5_Optic_Compensation(bool[] Set5_Skip_Band, OC_Single_Dual_Triple mode, Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal)
        {
            Gamma_Set Set = Gamma_Set.Set5;
            Set_Condition_Mipi_Script_Change(Set);
            button_Gamma_Set5_Apply.PerformClick();

            Dual_Or_Triple_Single_Compensation(Set, Set5_Skip_Band, mode, initial_RGBVreg1_cal);
        }

        private void Set6_Optic_Compensation(bool[] Set6_Skip_Band, OC_Single_Dual_Triple mode, Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal)
        {
            Gamma_Set Set = Gamma_Set.Set6;
            Set_Condition_Mipi_Script_Change(Set);
            button_Gamma_Set6_Apply.PerformClick();

            Dual_Or_Triple_Single_Compensation(Set, Set6_Skip_Band, mode, initial_RGBVreg1_cal);
        }

       

        public int Get_Max_Loop_Count()
        {
            return Convert.ToInt32(textBox_Max_Loop.Text);
        }

        public double Get_Skip_Lv()
        {
            return Convert.ToDouble(textBox_Subcompensation_GB_skip_Lv.Text);
        }



        private void Dual_Set5_Compensation(bool[] Set_Skip_Band)
        {
            Gamma_Set Set = Gamma_Set.Set5;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_or_Elgin model = new DP173_or_Elgin(OC_Single_Dual_Triple.Dual);
            Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal = new Initial_RGBVreg1_Calculation_EA9154();
            initial_RGBVreg1_cal.Set_Calculated_RGBVreg1_Vdata_Pointer(Set);

            //Get All_band_gray_Gamma
            DP173_Dual_Get_All_Band_Gray_Gamma(Set, model.All_band_gray_Gamma); //update "All_band_gray_Gamma[11,8]

            //dll-related variables
            Optic_Compensation_Succeed = false;

            for (model.band = 0; model.band < 11 && Optic_Compensation_Stop == false; model.band++)
            {
                if (Optic_Compensation_Stop) break;

                if (Band_BSQH_Selection(ref model.band)) //If this band is not selected , move on to the next band
                {
                    if (checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked)
                    {
                        model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set1);
                        DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);

                    }

                    DataGridView dataGridView_OC_param = DP173_form_Dual_engineer.dataGridView_OC_param_Set5;
                    model.Gamma_Out_Of_Register_Limit = false;
                    DP173_form_Dual_engineer.Band_Radiobuttion_Select(model.band, Set);
                    DP173_form_Dual_engineer.Force_Triger_Band_Set_CheckedChanged_Function(model.band, Set);
                    DP173_DBV_Setting(model.band);

                    //------Added on 200317-----------
                    double Before_Calculated_Init_Vreg1 = 0;
                    bool Gray255_Calculated = false;

                    if (checkBox_Vreg1_Compensation.Checked && checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                    {
                        model.Vreg1_loop_count = 0; //Vreg1 loop countR
                        model.Vreg1_Infinite_Count = 0;
                        model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                        Before_Calculated_Init_Vreg1 = model.Vreg1;

                        if ((checkBox_Apply_FX_Previous_Band_Vreg1.Checked) && (model.band >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]) && (model.band < 11) && Set_Skip_Band[model.band])
                        {
                            Gray255_Calculated = true;

                            double band_Target_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[(model.band * 8) + (0 + 2)].Cells[9].Value);
                            int[] Previous_Band_Gamma_Red = new int[8];
                            int[] Previous_Band_Gamma_Green = new int[8];
                            int[] Previous_Band_Gamma_Blue = new int[8];
                            double[] Previous_Band_Target_Lv = new double[8];
                            for (int i = 0; i < 8; i++)
                            {
                                Previous_Band_Gamma_Red[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[1].Value);
                                Previous_Band_Gamma_Green[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[2].Value);
                                Previous_Band_Gamma_Blue[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[3].Value);
                                if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[6].Value);
                                else if (this.radioButton_Previous_Target_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[9].Value);
                            }

                            int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1(model.band - 1, Set);//Prev_Band Vreg1

                            //C++ Dll Initial Calculate R/Vreg1/B Verify OK (= C# Result)
                            Imported_my_cpp_dll.DP173_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.Vreg1, ref model.Vreg1_First_Gamma_Red, ref model.Vreg1_First_Gamma_Blue, initial_RGBVreg1_cal.Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, model.band, band_Target_Lv, Previous_Band_Vreg1_Dec
    , Previous_Band_Target_Lv, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                            f1.GB_Status_AppendText_Nextline("(1)After(Dll C++,Precision : 0.001) Vreg1_First_Gamma_Red/Vreg1/Vreg1_First_Gamma_Blue : " + model.Vreg1_First_Gamma_Red.ToString() + "/" + model.Vreg1.ToString() + "/" + model.Vreg1_First_Gamma_Blue.ToString(), Color.Red);

                            //Set Calculated Vreg1_dec
                            DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);
                        }
                        else
                        {
                            Gray255_Calculated = false;
                        }


                        model.Initial_Vreg1 = model.Vreg1;
                        model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1;
                    }

                    else
                    {
                        if ((checkBox_Apply_FX_Previous_Band_Vreg1.Checked) && (model.band >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]) && (model.band < 11) && Set_Skip_Band[model.band])
                        {
                            model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                            Gray255_Calculated = true;

                            double band_Target_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[(model.band * 8) + (0 + 2)].Cells[9].Value);
                            int[] Previous_Band_Gamma_Red = new int[8];
                            int[] Previous_Band_Gamma_Green = new int[8];
                            int[] Previous_Band_Gamma_Blue = new int[8];
                            double[] Previous_Band_Target_Lv = new double[8];
                            for (int i = 0; i < 8; i++)
                            {
                                Previous_Band_Gamma_Red[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[1].Value);
                                Previous_Band_Gamma_Green[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[2].Value);
                                Previous_Band_Gamma_Blue[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[3].Value);
                                if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[6].Value);
                                else if (this.radioButton_Previous_Target_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[9].Value);
                            }

                            int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1(model.band - 1, Set);//Prev_Band Vreg1

                            Imported_my_cpp_dll.DP173_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(model.Vreg1, model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.G255_First_Gamma_Red, ref model.G255_First_Gamma_Green, ref model.G255_First_Gamma_Blue, initial_RGBVreg1_cal.Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, model.band, band_Target_Lv, Previous_Band_Vreg1_Dec
                                                                                              , Previous_Band_Target_Lv, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                            f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ (Gray255) After Gamma_R/G/B : " + model.G255_First_Gamma_Red.ToString() + "/" + model.G255_First_Gamma_Green.ToString() + "/" + model.G255_First_Gamma_Blue.ToString(), Color.Blue);

                            //Set Calculated Vreg1_dec
                            DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);

                            model.Initial_Vreg1 = model.Vreg1;
                            model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1;
                        }
                        else
                        {
                            Gray255_Calculated = false;
                        }
                    }
                    //--------------------------------


                    for (model.gray = 0; model.gray < 8 && Optic_Compensation_Stop == false; model.gray++)
                    {
                        if (Optic_Compensation_Stop) break;

                        DP173_Dual_Mode_Get_Param(model.gray, Set, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table 

                        //------Added on 200317-----------
                        if ((model.Target.double_Lv > Convert.ToDouble(textBox_Fast_OC_RGB_Skip_Target.Text)) && (checkBox_Apply_FX_3points_RGB.Checked) && (model.band >= 1) && (model.gray >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]) && Set_Skip_Band[model.band])
                        {

                            f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C# (All Band , 3points, Combine) Before Gamma_R/G/B : " + model.Gamma.int_R.ToString() + "/" + model.Gamma.int_G.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);

                            int Previous_Band_G255_Green_Gamma = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (0 + 2)].Cells[2].Value);
                            int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1((model.band - 1), Set);
                            int Current_Band_Dec_Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);
                            double Prvious_Gray_Gamma_R_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_R;
                            double Prvious_Gray_Gamma_G_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_G;
                            double Prvious_Gray_Gamma_B_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_B;

                            SJH_Matrix M = new SJH_Matrix();

                            int[] Band_Gray_Gamma_Red = new int[(model.band * 8)]; //Previous Bands
                            int[] Band_Gray_Gamma_Green = new int[(model.band * 8)];
                            int[] Band_Gray_Gamma_Blue = new int[(model.band * 8)];
                            double[] Band_Gray_Target_Lv = new double[(model.band * 8)];
                            int[] Band_Vreg1_Dec = new int[model.band + 1];//Previous Bands + Current Band
                            Band_Vreg1_Dec[model.band] = DP173_Get_Normal_Initial_Vreg1(model.band, Set);
                            //-----------Added On 200312---------------------
                            int second_band_end = Convert.ToInt32(numericUpDown2.Value);
                            
                            if (model.band <= second_band_end) //(band > first_band_end && band <= second_band_end), ex)B4,B5,B6,..,B10 (it means : min(second_band_start) > first_band_end)
                            {
                                for (int b = 0; b < model.band; b++)
                                {
                                    for (int g = 0; g < 8; g++)
                                    {
                                        
                                        Band_Gray_Gamma_Red[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[1].Value);
                                        Band_Gray_Gamma_Green[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[2].Value);
                                        Band_Gray_Gamma_Blue[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[3].Value);
                                        if (radioButton_Previous_Measure_Lv.Checked) Band_Gray_Target_Lv[(b * 8) + g] = Convert.ToDouble(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[6].Value);
                                        else if (this.radioButton_Previous_Target_Lv.Checked) Band_Gray_Target_Lv[(b * 8) + g] = Convert.ToDouble(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[9].Value);
                                        
                                    }
                                    Band_Vreg1_Dec[b] = DP173_Get_Normal_Initial_Vreg1(b, Set);
                                }
                            }
                            //-----------------------------------------------

                            double Fx_3points_Combine_LV_1 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_1.Text);
                            double Fx_3points_Combine_LV_2 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_2.Text);
                            double Fx_3points_Combine_LV_3 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_3.Text);
                            double Fx_3points_Combine_Lv_Distance = Convert.ToDouble(textBox_Fx_3points_Lv_Distance_Combine.Text);

                            double Combine_Lv_Ratio = Convert.ToDouble(textBox_Fast_OC_3Points_Ver2_LV_Combine_Ratio.Text);

                            Imported_my_cpp_dll.Get_Initial_Gamma_Fx_3points_Combine_Points_2(Combine_Lv_Ratio, model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.Gamma.int_R, ref model.Gamma.int_G, ref model.Gamma.int_B, initial_RGBVreg1_cal.Selected_Band, Band_Gray_Gamma_Red, Band_Gray_Gamma_Green, Band_Gray_Gamma_Blue, Band_Gray_Target_Lv, Current_Band_Dec_Vreg1,
                                    Band_Vreg1_Dec, model.band, model.gray, model.Target.double_Lv, Prvious_Gray_Gamma_R_Voltage, Prvious_Gray_Gamma_G_Voltage, Prvious_Gray_Gamma_B_Voltage,
                                    Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1
                                    , Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance);
                            f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ Ver2 (All Band , 3points, Combine) After Gamma_R/G/B : " + model.Gamma.int_R.ToString() + "/" + model.Gamma.int_G.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);
                        }
                        else if (checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked && Gray255_Calculated == true && model.gray == 0)
                        {
                            model.Gamma.int_R = model.G255_First_Gamma_Red;
                            model.Gamma.int_G = model.G255_First_Gamma_Green;
                            model.Gamma.int_B = model.G255_First_Gamma_Blue;
                        }
                        else
                        {

                        }
                        //--------------------------------

                        //---Added On 200317---
                        if (checkBox_Copy_Apply_Band_From_Upper_To_Lower.Checked && (model.band > 0) && (model.band < 11) && (model.gray == 0) && initial_RGBVreg1_cal.Selected_Band[model.band] && Set_Skip_Band[model.band])
                        {
                            DP173_form_Dual_engineer.Copy_Previous_Band_Gamma(model.band, Set); Application.DoEvents();
                            DP173_form_Dual_engineer.Get_Band_Gray_Gamma(model.All_band_gray_Gamma, model.band, Set); Application.DoEvents();
                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                        }
                        //---------------------

                        f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

                        if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && model.Vreg1_loop_count == 0 && checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                        {
                            DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, 0, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Set);
                            model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                        }

                        DP173_Pattern_Setting(Set, model.gray, model.band, OC_Single_Dual_Triple.Dual);//Pattern Setting
                        Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                        Thread.Sleep(300); //Pattern 안정화 Time
                        DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                        f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);

                        model.loop_count = 0;
                        model.Infinite_Count = 0;
                        DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Set); //Add on 190614
                        Optic_Compensation_Succeed = false;
                        model.Within_Spec_Limit = false;
                        while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                        {
                            if (model.Target.double_Lv < model.Skip_Lv)
                            {
                                if (model.band >= 1)
                                {
                                    DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Set);
                                    model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                }
                                Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

                                initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Set);

                                model.Measure.Set_Value(0, 0, 0);
                                DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Set); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                f1.GB_Status_AppendText_Nextline("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Compensation Skip (Target Lv : " + model.Target.double_Lv.ToString() + ") < " + model.Skip_Lv.ToString(), System.Drawing.Color.Blue);

                                Optic_Compensation_Succeed = true;
                                break;
                            }

                            //Vreg1 + Sub-Compensation (Change Gamma Value)
                            if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                            {
                                Vreg1_Infinite_Loop_Check(model.Vreg1_loop_count, model);

                                if (model.Vreg1_loop_count < model.loop_count_max)
                                {
                                    f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + model.Vreg1_loop_count.ToString(), Color.Blue);
                                    model.Prev_Vreg1 = model.Vreg1;
                                    model.Prev_Gamma.Equal_Value(model.Gamma);

                                    model.Vreg1_Compensation();

                                    f1.Showing_Diff_and_Current_Vreg1_and_Gamma(model.Prev_Gamma, model.Gamma, model.Prev_Vreg1, model.Vreg1);

                                    if (Math.Abs(model.Vreg1 - model.Prev_Vreg1) >= 1) model.Vreg1_Need_To_Be_Updated = true;
                                    else model.Vreg1_Need_To_Be_Updated = false;

                                    if (model.Vreg1_Need_To_Be_Updated)
                                    {
                                        DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);
                                    }
                                }
                                model.Vreg1_loop_count++;
                                model.loop_count++;

                                if (model.Vreg1_Infinite_Count >= 3)
                                {
                                    Extension_Applied = "O";
                                    f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                }
                                else
                                    Extension_Applied = "X";
                            }

                            else
                            {
                                model.Vreg1_Need_To_Be_Updated = false;//Add on 190603

                                model.Prev_Gamma.Equal_Value(model.Gamma);
                                Infinite_Loop_Check(model.loop_count, model);

                                model.Sub_Compensation();

                                f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);
                                model.loop_count++;

                                if (model.Infinite_Count >= 3)
                                {
                                    Extension_Applied = "O";
                                    f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                }
                                else Extension_Applied = "X";
                            }
                            if (model.Vreg1_Need_To_Be_Updated == false)
                            {
                                Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                            }

                            if (model.Within_Spec_Limit)
                            {
                                initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Set);

                                Optic_Compensation_Succeed = true;
                                textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                break;
                            }

                            if (model.Gamma_Out_Of_Register_Limit)
                            {
                                if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                    Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Set); //Added on 200519
                                else
                                    Optic_Compensation_Succeed = false;

                                textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");

                                if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                break;
                            }

                            textBox_loop_count.Text = (model.loop_count).ToString();

                            if (model.loop_count == model.loop_count_max)
                            {
                                if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                    Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Set); //Added on 200519
                                else
                                    Optic_Compensation_Succeed = false;

                                textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                System.Windows.Forms.MessageBox.Show("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Loop Count Over");

                                if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                break;
                            }

                            DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Set); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                        }//while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false) Loop End
                        if (checkBox_Only_255G.Checked) model.gray = 8;

                    }//Gray Loop End
                    f1.GB_ProgressBar_PerformStep();

                }
            }//Band Loop End   
        }





        private void Dual_Or_Triple_Single_Compensation(Gamma_Set Set, bool[] Set_Skip_Band, OC_Single_Dual_Triple mode, Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_or_Elgin model = new DP173_or_Elgin(mode);
            initial_RGBVreg1_cal.Set_Calculated_RGBVreg1_Vdata_Pointer(Set);

            //Get All_band_gray_Gamma
            DP173_Dual_Get_All_Band_Gray_Gamma(Set, model.All_band_gray_Gamma); //update "All_band_gray_Gamma[11,8]

            //dll-related variables
            model.Gamma_Out_Of_Register_Limit = false;
            model.Within_Spec_Limit = false;
            Optic_Compensation_Succeed = false;

            for (model.band = 0; model.band < 11 && Optic_Compensation_Stop == false; model.band++)
            {
                if (Optic_Compensation_Stop) break;

                if (Band_BSQH_Selection(ref model.band)) //If this band is not selected , move on to the next band
                {
                    if (Set_Skip_Band[model.band])
                    {
                        //Get Calculated_Vdata and Calculated_Vreg1_Voltage
                        initial_RGBVreg1_cal.Update_Data_Voltages_And_Skip_Band_Measures(Set, model);

                        //Apply Vreg1
                        DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);

                        //Apply All_band_gray_Gamma
                        DP173_DBV_Setting(model.band);
                        DP173_form_Dual_engineer.Band_Radiobuttion_Select(model.band, Set);
                        DP173_form_Dual_engineer.Force_Triger_Band_Set_CheckedChanged_Function(model.band, Set);
                        model.gray = 0;
                        DP173_Dual_Mode_Get_Param(model.gray, Set, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension);
                        Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                        Thread.Sleep(20);
                        f1.GB_ProgressBar_PerformStep();
                    }
                    else
                    {
                        if (checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked)
                        {
                            model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set1);
                            DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);
                        }

                        DataGridView dataGridView_OC_param = DP173_form_Dual_engineer.Get_OC_Param_DataGridView(Set);
                        model.Gamma_Out_Of_Register_Limit = false;
                        DP173_form_Dual_engineer.Band_Radiobuttion_Select(model.band, Set);
                        DP173_form_Dual_engineer.Force_Triger_Band_Set_CheckedChanged_Function(model.band, Set);
                        DP173_DBV_Setting(model.band);


                        //------Added on 200317-----------
                        double Before_Calculated_Init_Vreg1 = 0;
                        bool Gray255_Calculated = false;

                        if (checkBox_Vreg1_Compensation.Checked && checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                        {
                            model.Vreg1_loop_count = 0; //Vreg1 loop countR
                            model.Vreg1_Infinite_Count = 0;
                            model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                            Before_Calculated_Init_Vreg1 = model.Vreg1;

                            if ((checkBox_Apply_FX_Previous_Band_Vreg1.Checked) && (model.band >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]) && (model.band < 11))
                            {
                                Gray255_Calculated = true;

                                double band_Target_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[(model.band * 8) + (0 + 2)].Cells[9].Value);
                                int[] Previous_Band_Gamma_Red = new int[8];
                                int[] Previous_Band_Gamma_Green = new int[8];
                                int[] Previous_Band_Gamma_Blue = new int[8];
                                double[] Previous_Band_Target_Lv = new double[8];
                                for (int i = 0; i < 8; i++)
                                {
                                    Previous_Band_Gamma_Red[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[1].Value);
                                    Previous_Band_Gamma_Green[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[2].Value);
                                    Previous_Band_Gamma_Blue[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[3].Value);
                                    if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[6].Value);
                                    else if (this.radioButton_Previous_Target_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[9].Value);
                                }

                                int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1(model.band - 1, Set);//Prev_Band Vreg1

                                //C++ Dll Initial Calculate R/Vreg1/B Verify OK (= C# Result)
                                Imported_my_cpp_dll.DP173_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.Vreg1, ref model.Vreg1_First_Gamma_Red, ref model.Vreg1_First_Gamma_Blue, initial_RGBVreg1_cal.Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, model.band, band_Target_Lv, Previous_Band_Vreg1_Dec
        , Previous_Band_Target_Lv, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                                f1.GB_Status_AppendText_Nextline("(1)After(Dll C++,Precision : 0.001) Vreg1_First_Gamma_Red/Vreg1/Vreg1_First_Gamma_Blue : " + model.Vreg1_First_Gamma_Red.ToString() + "/" + model.Vreg1.ToString() + "/" + model.Vreg1_First_Gamma_Blue.ToString(), Color.Red);

                                //Set Calculated Vreg1_dec
                                DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);
                            }
                            else
                            {
                                Gray255_Calculated = false;
                            }

                            model.Initial_Vreg1 = model.Vreg1;
                            model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1;
                        }

                        else
                        {
                            if ((checkBox_Apply_FX_Previous_Band_Vreg1.Checked) && (model.band >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]) && (model.band < 11))
                            {
                                model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                                Gray255_Calculated = true;

                                double band_Target_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[(model.band * 8) + (0 + 2)].Cells[9].Value);
                                int[] Previous_Band_Gamma_Red = new int[8];
                                int[] Previous_Band_Gamma_Green = new int[8];
                                int[] Previous_Band_Gamma_Blue = new int[8];
                                double[] Previous_Band_Target_Lv = new double[8];
                                for (int i = 0; i < 8; i++)
                                {
                                    Previous_Band_Gamma_Red[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[1].Value);
                                    Previous_Band_Gamma_Green[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[2].Value);
                                    Previous_Band_Gamma_Blue[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[3].Value);
                                    if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[6].Value);
                                    else if (this.radioButton_Previous_Target_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[9].Value);
                                }

                                int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1(model.band - 1, Set);//Prev_Band Vreg1

                                Imported_my_cpp_dll.DP173_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(model.Vreg1, model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.G255_First_Gamma_Red, ref model.G255_First_Gamma_Green, ref model.G255_First_Gamma_Blue, initial_RGBVreg1_cal.Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, model.band, band_Target_Lv, Previous_Band_Vreg1_Dec
                                                                                                  , Previous_Band_Target_Lv, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                                f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ (Gray255) After Gamma_R/G/B : " + model.G255_First_Gamma_Red.ToString() + "/" + model.G255_First_Gamma_Green.ToString() + "/" + model.G255_First_Gamma_Blue.ToString(), Color.Blue);

                                //Set Calculated Vreg1_dec
                                DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);

                                model.Initial_Vreg1 = model.Vreg1;
                                model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1;
                            }
                            else
                            {
                                Gray255_Calculated = false;
                            }
                        }
                        //--------------------------------


                        for (model.gray = 0; model.gray < 8 && Optic_Compensation_Stop == false; model.gray++)
                        {
                            if (Optic_Compensation_Stop) break;

                            DP173_Dual_Mode_Get_Param(model.gray, Set, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table

                            //------Added on 200317-----------
                            if ((model.Target.double_Lv > Convert.ToDouble(textBox_Fast_OC_RGB_Skip_Target.Text)) && (checkBox_Apply_FX_3points_RGB.Checked) && (model.band >= 1) && (model.gray >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]))
                            {

                                f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C# (All Band , 3points, Combine) Before Gamma_R/G/B : " + model.Gamma.int_R.ToString() + "/" + model.Gamma.int_G.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);

                                int Previous_Band_G255_Green_Gamma = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (0 + 2)].Cells[2].Value);
                                int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1((model.band - 1), Set);
                                int Current_Band_Dec_Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);
                                double Prvious_Gray_Gamma_R_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_R;
                                double Prvious_Gray_Gamma_G_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_G;
                                double Prvious_Gray_Gamma_B_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_B;

                                SJH_Matrix M = new SJH_Matrix();

                                int[] Band_Gray_Gamma_Red = new int[(model.band * 8)]; //Previous Bands
                                int[] Band_Gray_Gamma_Green = new int[(model.band * 8)];
                                int[] Band_Gray_Gamma_Blue = new int[(model.band * 8)];
                                double[] Band_Gray_Target_Lv = new double[(model.band * 8)];
                                int[] Band_Vreg1_Dec = new int[model.band + 1];//Previous Bands + Current Band
                                Band_Vreg1_Dec[model.band] = DP173_Get_Normal_Initial_Vreg1(model.band, Set);
                                //-----------Added On 200312---------------------
                                int second_band_end = Convert.ToInt32(numericUpDown2.Value);
                                if (model.band <= second_band_end) //(band > first_band_end && band <= second_band_end), ex)B4,B5,B6,..,B10 (it means : min(second_band_start) > first_band_end)
                                {
                                    for (int b = 0; b < model.band; b++)
                                    {
                                        for (int g = 0; g < 8; g++)
                                        {
                                            Band_Gray_Gamma_Red[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[1].Value);
                                            Band_Gray_Gamma_Green[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[2].Value);
                                            Band_Gray_Gamma_Blue[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[3].Value);
                                            if (radioButton_Previous_Measure_Lv.Checked) Band_Gray_Target_Lv[(b * 8) + g] = Convert.ToDouble(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[6].Value);
                                            else if (this.radioButton_Previous_Target_Lv.Checked) Band_Gray_Target_Lv[(b * 8) + g] = Convert.ToDouble(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[9].Value);   
                                        }
                                        Band_Vreg1_Dec[b] = DP173_Get_Normal_Initial_Vreg1(b, Set);
                                    }
                                }
                                //-----------------------------------------------

                                double Fx_3points_Combine_LV_1 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_1.Text);
                                double Fx_3points_Combine_LV_2 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_2.Text);
                                double Fx_3points_Combine_LV_3 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_3.Text);
                                double Fx_3points_Combine_Lv_Distance = Convert.ToDouble(textBox_Fx_3points_Lv_Distance_Combine.Text);

                                double Combine_Lv_Ratio = Convert.ToDouble(textBox_Fast_OC_3Points_Ver2_LV_Combine_Ratio.Text);

                                Imported_my_cpp_dll.Get_Initial_Gamma_Fx_3points_Combine_Points_2(Combine_Lv_Ratio, model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.Gamma.int_R, ref model.Gamma.int_G, ref model.Gamma.int_B, initial_RGBVreg1_cal.Selected_Band, Band_Gray_Gamma_Red, Band_Gray_Gamma_Green, Band_Gray_Gamma_Blue, Band_Gray_Target_Lv, Current_Band_Dec_Vreg1,
                                        Band_Vreg1_Dec, model.band, model.gray, model.Target.double_Lv, Prvious_Gray_Gamma_R_Voltage, Prvious_Gray_Gamma_G_Voltage, Prvious_Gray_Gamma_B_Voltage,
                                        Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1
                                        , Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance);
                                f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ Ver2 (All Band , 3points, Combine) After Gamma_R/G/B : " + model.Gamma.int_R.ToString() + "/" + model.Gamma.int_G.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);

                            }
                            else if (checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked && Gray255_Calculated == true && model.gray == 0)
                            {
                                model.Gamma.int_R = model.G255_First_Gamma_Red;
                                model.Gamma.int_G = model.G255_First_Gamma_Green;
                                model.Gamma.int_B = model.G255_First_Gamma_Blue;
                            }

                            //--------------------------------

                            //---Added On 200317---
                            if (checkBox_Copy_Apply_Band_From_Upper_To_Lower.Checked && (model.band > 0) && (model.band < 11) && (model.gray == 0) && initial_RGBVreg1_cal.Selected_Band[model.band])
                            {
                                DP173_form_Dual_engineer.Copy_Previous_Band_Gamma(model.band, Set); Application.DoEvents();
                                DP173_form_Dual_engineer.Get_Band_Gray_Gamma(model.All_band_gray_Gamma, model.band, Set); Application.DoEvents();
                                Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                            }
                            //---------------------


                            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

                            if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && model.Vreg1_loop_count == 0 && checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                            {
                                DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, 0, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Set);
                                model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);

                                //---Added On 200317---
                                if (checkBox_Apply_FX_Previous_Band_Vreg1.Checked && initial_RGBVreg1_cal.Selected_Band[model.band] == true)
                                {
                                    f1.GB_Status_AppendText_Nextline("(1)Before R/Vreg1/B : " + model.Gamma.int_R.ToString() + "/" + Before_Calculated_Init_Vreg1.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);
                                    model.Gamma.int_R = model.Vreg1_First_Gamma_Red;
                                    model.Gamma.int_B = model.Vreg1_First_Gamma_Blue;
                                    f1.GB_Status_AppendText_Nextline("(1)After R/Vreg1/B : " + model.Gamma.int_R.ToString() + "/" + model.Vreg1.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Red);
                                }
                                //---------------------
                            }

                            DP173_Pattern_Setting(Set, model.gray, model.band, OC_Single_Dual_Triple.Dual);//Pattern Setting
                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                            Thread.Sleep(300); //Pattern 안정화 Time
                            DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);

                            model.loop_count = 0;
                            model.Infinite_Count = 0;
                            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Set); //Add on 190614
                            Optic_Compensation_Succeed = false;
                            model.Within_Spec_Limit = false;
                            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                            {
                                if (model.Target.double_Lv < model.Skip_Lv)
                                {
                                    if (model.band >= 1)
                                    {
                                        DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Set);
                                        model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                    }
                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

                                    initial_RGBVreg1_cal.Update_Calculated_Vdata(model,Set);

                                    model.Measure.Set_Value(0, 0, 0);
                                    DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Set); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.GB_Status_AppendText_Nextline("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Compensation Skip (Target Lv : " + model.Target.double_Lv.ToString() + ") < " + model.Skip_Lv.ToString(), System.Drawing.Color.Blue);

                                    Optic_Compensation_Succeed = true;
                                    break;
                                }

                                //Vreg1 + Sub-Compensation (Change Gamma Value)
                                if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                                {
                                    Vreg1_Infinite_Loop_Check(model.Vreg1_loop_count, model);

                                    if (model.Vreg1_loop_count < model.loop_count_max)
                                    {
                                        f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + model.Vreg1_loop_count.ToString(), Color.Blue);
                                        model.Prev_Vreg1 = model.Vreg1;
                                        model.Prev_Gamma.Equal_Value(model.Gamma);

                                        model.Vreg1_Compensation();

                                        f1.Showing_Diff_and_Current_Vreg1_and_Gamma(model.Prev_Gamma, model.Gamma, model.Prev_Vreg1, model.Vreg1);

                                        if (Math.Abs(model.Vreg1 - model.Prev_Vreg1) >= 1) model.Vreg1_Need_To_Be_Updated = true;
                                        else model.Vreg1_Need_To_Be_Updated = false;

                                        if (model.Vreg1_Need_To_Be_Updated)
                                        {
                                            DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);
                                        }
                                    }
                                    model.Vreg1_loop_count++;
                                    model.loop_count++;

                                    if (model.Vreg1_Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }

                                else
                                {
                                    model.Vreg1_Need_To_Be_Updated = false;//Add on 190603

                                    model.Prev_Gamma.Equal_Value(model.Gamma);
                                    Infinite_Loop_Check(model.loop_count, model);

                                    model.Sub_Compensation();

                                    f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);
                                    model.loop_count++;

                                    if (model.Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else Extension_Applied = "X";
                                }
                                if (model.Vreg1_Need_To_Be_Updated == false)
                                {
                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                    Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                }

                                if (model.Within_Spec_Limit)
                                {
                                    initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Set);

                                    Optic_Compensation_Succeed = true;
                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    break;
                                }

                                if (model.Gamma_Out_Of_Register_Limit)
                                {
                                    if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                        Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Set); //Added on 200519
                                    else
                                        Optic_Compensation_Succeed = false;

                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");

                                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                    break;
                                }

                                textBox_loop_count.Text = (model.loop_count).ToString();

                                if (model.loop_count == model.loop_count_max)
                                {
                                    if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                        Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Set); //Added on 200519
                                    else
                                        Optic_Compensation_Succeed = false;

                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    System.Windows.Forms.MessageBox.Show("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Loop Count Over");

                                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                    break;
                                }

                                DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Set); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                            }//while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false) Loop End
                            if (checkBox_Only_255G.Checked) model.gray = 8;

                        }//Gray Loop End
                        f1.GB_ProgressBar_PerformStep();
                    }//else (-->Set_Skip_Band[band] == false)End
                }
            }//Band Loop End       
        }



        private void Copy_All_Band_Gamma_From_Set1_To_Set4()
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            for (int band = 0; band <= 10; band++)
                for (int gray = 0; gray < 8; gray++)
                    DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set1_to_Set4(band, gray); //Copy Gamma (Set1 to Set4)   
        }

        private void Copy_All_Band_Gamma_From_Set2_To_Set5()
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            for (int band = 0; band <= 10; band++)
                for (int gray = 0; gray < 8; gray++)
                    DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set2_to_Set5(band, gray); //Copy Gamma (Set2 to Set5)   
        }

        private void Copy_All_Band_Gamma_From_Set3_To_Set6()
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            for (int band = 0; band <= 10; band++)
                for (int gray = 0; gray < 8; gray++)
                    DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set3_to_Set6(band, gray); //Copy Gamma (Set3 to Set6)   
        }


        private void Copy_All_Band_Gamma_From_Set1_To_Set3()
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            for (int band = 0; band <= 10; band++)
                for (int gray = 0; gray < 8; gray++)
                    DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set1_to_Set3(band, gray); //Copy Gamma (Set1 to Set3)   
        }

        private void Copy_All_Band_Gamma_From_Set2_To_Set4()
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            for (int band = 0; band <= 10; band++)
                for (int gray = 0; gray < 8; gray++)
                    DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set2_to_Set4(band, gray); //Copy Gamma (Set2 to Set4) 
        }


        private void Copy_All_Band_Gamma_From_Set5_To_Set6()
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            for (int band = 0; band <= 10; band++)
                for (int gray = 0; gray < 8; gray++)
                    DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set5_to_Set6(band, gray); //Copy Gamma (Set2 to Set4) 
        }


        private void DP173_Dual_Get_All_Band_Gray_Gamma(Gamma_Set Set, RGB[,] All_band_gray_Gamma)
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_form_Dual_engineer.DP173_Get_All_Band_Gray_Gamma(All_band_gray_Gamma, Set);
        }


        private void DP173_Dual_Mode_Get_Param(int gray, Gamma_Set Set, ref RGB Gamma, ref XYLv Target, ref XYLv Limit, ref XYLv Extension)
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_form_dual_engineer.Get_OC_Param_DP173(gray, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B
                , ref Target.double_X, ref Target.double_Y, ref Target.double_Lv, ref Limit.double_X, ref Limit.double_Y, ref Limit.double_Lv, ref Extension.double_X, ref Extension.double_Y, Set);
            Gamma.String_Update_From_int();
        }

        private void Triple_Mode_Set23_Target_XYLv_Setting_From_Set1_Measure(DP173_or_Elgin model)
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            //if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set234Target(band, gray);
            if (radioButton_Target_Setting_Triple_Default.Checked)
            {
                //Do nothing
            }
            else if (radioButton_Triple_Copy_Set1_M_to_Set23_T.Checked)
            {
                DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set2Target(model.band, model.gray);//Set1 M to Set2 T
                DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set3Target(model.band, model.gray);//Set1 M to Set3 T
            }
            else if (radioButton_Triple_Copy_Set1_M_to_Set2_T_Copy_Set2_M_to_Set3_T.Checked)
            {
                DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set2Target(model.band, model.gray);//Set1 M to Set2 T
                //DP173_form_Dual_engineer.Dual_Copy_Set2_Measure_To_Set3Target(band, gray);//Set2 M to Set3 T
            }
            else if (radioButton_Triple_Copy_Set1_M_to_Set2_T_Copy_Set12_Ave_M_to_Set3_T.Checked)
            {
                DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set2Target(model.band, model.gray);//Set1 M to Set2 T
                //DP173_form_Dual_engineer.Dual_Copy_Set1_Set2_Ave_Measure_To_Set3Target(band, gray);// Set1/2 Ave M to Set3 T
            }
        }


        private void Triple_Mode_Set3_Target_XYLv_Setting_From_Set12_Measure(DP173_or_Elgin model)
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];

            //if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set234Target(band, gray);
            if (radioButton_Target_Setting_Triple_Default.Checked)
            {
                //Do nothing
            }
            else if (radioButton_Triple_Copy_Set1_M_to_Set23_T.Checked)
            {
                //DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set2Target(band, gray);//Set1 M to Set2 T
                //DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set3Target(band, gray);//Set1 M to Set3 T
            }
            else if (radioButton_Triple_Copy_Set1_M_to_Set2_T_Copy_Set2_M_to_Set3_T.Checked)
            {
                //DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set2Target(band, gray);//Set1 M to Set2 T
                DP173_form_Dual_engineer.Dual_Copy_Set2_Measure_To_Set3Target(model.band, model.gray);//Set2 M to Set3 T
            }
            else if (radioButton_Triple_Copy_Set1_M_to_Set2_T_Copy_Set12_Ave_M_to_Set3_T.Checked || radioButton_Triple_Copy_Set12_Ave_M_to_Set3_T.Checked)
            {
                //DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set2Target(band, gray);//Set1 M to Set2 T
                DP173_form_Dual_engineer.Dual_Copy_Set1_Set2_Ave_Measure_To_Set3Target(model.band, model.gray);// Set1/2 Ave M to Set3 T
            }
        }



        private void Triple_Mode_Optic_Compensation()
        {
            Dual_Or_Triple_Mode_Initialize();

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_or_Elgin model = new DP173_or_Elgin(OC_Single_Dual_Triple.Triple);
            Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal = new Initial_RGBVreg1_Calculation_EA9154();

            //Update All_band_gray_Gamma
            DP173_Dual_Get_All_Band_Gray_Gamma(Gamma_Set.Set1, model.All_band_gray_Gamma); //Get All_band_gray_Gamma[14,8]

            //Dual ELVSS Compensation
            Dual_ELVSS_VREF1_Compensation(model, model.All_band_gray_Gamma);

            // Black Compensation
            Black_Compensation(model);
            button_Vreg1_Read.PerformClick();//Update vreg1 and REF_voltage(global variables)
            button_Read_AM0_VREF2_HBM_Set1_Only.PerformClick();//Added On 200219 (VREF1/VREF2047/AM0 Read)

            bool[] Set4_Skip_Band = new bool[11];//default bool value is false
            bool[] Set5_Skip_Band = new bool[11];//default bool value is false
            bool[] Set6_Skip_Band = new bool[11];//default bool value is false

            int Last_Band = Convert.ToInt32(numericUpDown2.Value);
            for (int b = 0; b <= Last_Band; b++) initial_RGBVreg1_cal.Selected_Band[b] = true;
            if (Last_Band < 10) for (int b = (Last_Band + 1); b <= 10; b++) initial_RGBVreg1_cal.Selected_Band[b] = false;

            int Set456_OC_Skip_Last_Band = Convert.ToInt32(numericUpDown_Triple_Set456_OC_Skip_Last_Band.Value);
            for (int b = 0; b <= Set456_OC_Skip_Last_Band; b++)
            {
                Set4_Skip_Band[b] = true;
                Set5_Skip_Band[b] = true;
                Set6_Skip_Band[b] = true;
            }

            if (Set456_OC_Skip_Last_Band < 10)
            {
                for (int b = (Set456_OC_Skip_Last_Band + 1); b <= 10; b++)
                {
                    Set4_Skip_Band[b] = false;
                    Set5_Skip_Band[b] = false;
                    Set6_Skip_Band[b] = false;
                }
            }

            if (Optic_Compensation_Stop) return;

            bool AM1_OC = radioButton_HBM_AM1_OC.Checked;
            bool AM1_OC_Finished = false;


            for (model.band = 0; model.band < 14 && Optic_Compensation_Stop == false; model.band++)
            {
                if (Is_AM1_HBM_OC(model.band, AM1_OC, AM1_OC_Finished))
                {
                    Optic_Compensation_Stop = Set1_HBM_AM1_Compensation(ref model.R_AM1_Hex, ref model.G_AM1_Hex, ref  model.B_AM1_Hex, OC_Single_Dual_Triple.Dual, Gamma_Set.Set1);
                    model.Update_AM1_Dec_From_AM1_Hex();
                    AM1_OC_Finished = true;
                    model.band = -1;
                    continue;
                }

                f1.GB_Status_AppendText_Nextline("Band" + (model.band).ToString(), Color.Green);

                if (Optic_Compensation_Stop) break;

                model.Gamma_Out_Of_Register_Limit = false;

                if (Band_BSQH_Selection(ref model.band)) //If this band is not selected , move on to the next band
                {
                    model.Vreg1_loop_count = 0; //Add on 190820
                    model.Vreg1_Infinite_Count = 0; //Add on 190820

                    if (model.Is_AOD_Band()) f1.AOD_On();
                    
                    DP173_DBV_Setting(model.band);
                    for (model.gray = 0; model.gray < 8 && Optic_Compensation_Stop == false; model.gray++)
                    {
                        bool Current_Gray_GB_Skip = false;//Added on 200221
                        //Set1
                        {
                            DP173_form_Dual_engineer.Band_Radiobuttion_Select(model.band, Gamma_Set.Set1);
                            DP173_form_Dual_engineer.Force_Triger_Band_Set_CheckedChanged_Function(model.band, Gamma_Set.Set1); //In triple mode it must be needed !!! (200525)
                            
                            button_Gamma_Set1_Apply.PerformClick();//Set1 Apply
                            DP173_Pattern_Setting(Gamma_Set.Set1, model.gray, model.band, OC_Single_Dual_Triple.Dual);
                            Thread.Sleep(300);
                            DP173_DBV_Setting(model.band);  //DBV Settin

                            Dual_Triple_Calculate_Init_RGBVreg1_And_Apply_And_Measure(model, Set4_Skip_Band, Gamma_Set.Set1, initial_RGBVreg1_cal); //Added on 200521
                            
                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set1, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                            Thread.Sleep(20);

                            DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count
                                    , model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                            Application.DoEvents();

                            model.loop_count = 0;
                            model.Infinite_Count = 0;

                            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set1); //Add on 190614
                            Triple_Mode_Set23_Target_XYLv_Setting_From_Set1_Measure(model);

                            Optic_Compensation_Succeed = false;
                            model.Within_Spec_Limit = false;

                            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                            {
                                if (model.Target.double_Lv < model.Skip_Lv)
                                {
                                    Current_Gray_GB_Skip = true;//Added on 200221

                                    if (model.band >= 1)
                                    {
                                        DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Gamma_Set.Set1);
                                        model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                    }
                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set1, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                    model.Measure.Set_Value(0, 0, 0);
                                    DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set1); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.GB_Status_AppendText_Nextline("B" + model.band.ToString() + "/G" + model.gray.ToString()
                                        + " Compensation Skip (Target Lv : " + model.Target.double_Lv.ToString() + ") < " + model.Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                    Optic_Compensation_Succeed = true;
                                    break;
                                }

                                if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                {
                                    Vreg1_Infinite_Loop_Check(model.Vreg1_loop_count, model);

                                    if (model.Vreg1_loop_count < model.loop_count_max)
                                    {
                                        f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + model.Vreg1_loop_count.ToString(), Color.Blue);
                                        model.Prev_Vreg1 = model.Vreg1;
                                        model.Prev_Gamma.Equal_Value(model.Gamma);

                                        model.Vreg1_Compensation();

                                        f1.Showing_Diff_and_Current_Vreg1_and_Gamma(model.Prev_Gamma, model.Gamma, model.Prev_Vreg1, model.Vreg1);

                                        if (Math.Abs(model.Vreg1 - model.Prev_Vreg1) >= 1) model.Vreg1_Need_To_Be_Updated = true;
                                        else model.Vreg1_Need_To_Be_Updated = false;

                                        if (model.Vreg1_Need_To_Be_Updated)
                                        {
                                            DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Gamma_Set.Set1); ;
                                        }
                                    }
                                    model.Vreg1_loop_count++;
                                    model.loop_count++;

                                    if (model.Vreg1_Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }
                                else
                                {
                                    model.Vreg1_Need_To_Be_Updated = false;//Add on 190603

                                    model.Prev_Gamma.Equal_Value(model.Gamma);
                                    Infinite_Loop_Check(model.loop_count, model);

                                    model.Sub_Compensation();

                                    f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);

                                    model.loop_count++;

                                    if (model.Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }
                                if (model.Vreg1_Need_To_Be_Updated == false)
                                {
                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set1, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                    Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                }

                                if (model.Within_Spec_Limit)
                                {
                                    initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set1);//Added on 200521

                                    Optic_Compensation_Succeed = true;
                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    break;
                                }

                                if (model.Gamma_Out_Of_Register_Limit)
                                {
                                    if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                        Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set1); //Added on 200519
                                    else
                                        Optic_Compensation_Succeed = false;

                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");

                                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                    break;
                                }

                                textBox_loop_count.Text = (model.loop_count).ToString();

                                if (model.loop_count == model.loop_count_max)
                                {
                                    if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                        Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set1); //Added on 200519
                                    else
                                        Optic_Compensation_Succeed = false;

                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    System.Windows.Forms.MessageBox.Show("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Loop Count Over");

                                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                    break;
                                }
                                DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Gamma_Set.Set1); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                Application.DoEvents();
                            }
                            f1.GB_ProgressBar_PerformStep();
                            ///////////////////////////Condition 1 Over
                        }

                        if (!(AM1_OC == true && AM1_OC_Finished == false))
                        {
                            Triple_Mode_Set23_Target_XYLv_Setting_From_Set1_Measure(model);
                            if (model.Is_HBM_Band() || model.Is_Normal_Band())
                            {
                                DP173_form_Dual_engineer.Band_Radiobuttion_Select(model.band, Gamma_Set.Set2);
                                DP173_form_Dual_engineer.Force_Triger_Band_Set_CheckedChanged_Function(model.band, Gamma_Set.Set2); //In triple mode it must be needed !!! (200525)
                                Application.DoEvents();

                                ///////////////////////////Set 2 Start
                                button_Gamma_Set2_Apply.PerformClick();//Set2 Apply
                                model.Vreg1_loop_count = 0; //Add on 191031
                                model.Vreg1_Infinite_Count = 0; //Add on 191031

                                //Set2
                                {
                                    double OC_Skip_Special_Process_Start_Lv = Convert.ToDouble(textBox_OC_Skip_Special_Process_Start_Lv_triple.Text);
                                    double OC_Skip_Special_Process_Last_Lv = Convert.ToDouble(textBox_OC_Skip_Special_Process_Last_Lv_triple.Text);
                                    if (checkBox_Triple_Gamma_Copy_Set1_to_Set2.Checked) 
                                    {
                                        DP173_form_Dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(model.band, Gamma_Set.Set2);
                                        DP173_Dual_Mode_Get_Param(model.gray, Gamma_Set.Set2, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table
                                        if (checkBox_Set2_Set3_OC_Skip_If_XY_and_deltaL_Are_within_Specs_Triple_Mode.Checked && model.Target.double_Lv <= OC_Skip_Special_Process_Start_Lv && model.Target.double_Lv >= OC_Skip_Special_Process_Last_Lv)
                                        {
                                            int Green_Offset = Convert.ToInt32(textBox_OC_Skip_Special_Process_G_Offset_triple_Set12.Text);
                                            f1.GB_Status_AppendText_Nextline("OC_Skip_Special_Process_Start_Lv >= Target.double_Lv >= OC_Skip_Special_Process_Last_Lv : " + OC_Skip_Special_Process_Start_Lv.ToString() + ">=" + model.Target.double_Lv.ToString() + ">=" + OC_Skip_Special_Process_Last_Lv.ToString(), Color.Green);
                                            f1.GB_Status_AppendText_Nextline("Green_Offset " + Green_Offset.ToString() + " has been applied", Color.Green);
                                            DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set1_to_Set2(model.band, model.gray, Green_Offset); //Copy Gamma (Set1 to Set2)
                                        }
                                        else
                                        {
                                            DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set1_to_Set2(model.band, model.gray); //Copy Gamma (Set1 to Set2)
                                        }
                                    }

                                    DP173_form_Dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(model.band, Gamma_Set.Set2);
                                    DP173_Dual_Get_All_Band_Gray_Gamma(Gamma_Set.Set2, model.All_band_gray_Gamma); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet

                                    if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                    {
                                        if (checkBox_Triple_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked) model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set1);
                                        else model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set2); //Read Vreg1 Value (Gamma_Set 2's Vreg1 Value = Gamma_Set 1's Vreg1 Value);
                                        DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Gamma_Set.Set2); //Add on 190702 (Condition 1 꺼를 읽은거기 때문에 먼저 Condition2 꺼에 초기 Vreg1 세팅 필요)
                                        model.Initial_Vreg1 = model.Vreg1;//Add on 190603 
                                        model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1; //Add on 190603
                                    }

                                    if (Optic_Compensation_Stop) break;
                                    DP173_Dual_Mode_Get_Param(model.gray, Gamma_Set.Set2, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table   
                                    f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

                                    if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && model.Vreg1_loop_count == 0
                                        && checkBox_Triple_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                                    {
                                        DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, 0, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Gamma_Set.Set2);
                                        model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                    }

                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set2, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                    Thread.Sleep(20);
                                    DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count
                                        , model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                    model.loop_count = 0;
                                    model.Infinite_Count = 0;
                                    DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set2); //Add on 190614
                                    f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                    Application.DoEvents();

                                    Optic_Compensation_Succeed = false;
                                    model.Within_Spec_Limit = false;


                                    bool Skip_Set2_OC = Is_Triple_Mode_Set2_OC_Skip(model); //Added on 200521
                                    if (Skip_Set2_OC)
                                    {
                                        Triple_Set2_Band_Green_Offset_1(model, OC_Skip_Special_Process_Last_Lv);
                                        DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                        DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Gamma_Set.Set2); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                        Application.DoEvents();

                                        initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set2);
                                    }


                                    while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false && Skip_Set2_OC == false)
                                    {
                                        //if (Target.double_Lv < Skip_Lv)//Deleted on 200221
                                        if (Current_Gray_GB_Skip) //Added on 200221
                                        {
                                            if (model.band >= 1)
                                            {
                                                DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Gamma_Set.Set2);
                                                model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                            }
                                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set2, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

                                            initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set2);

                                            model.Measure.Set_Value(0, 0, 0);
                                            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set2); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                            f1.GB_Status_AppendText_Nextline("B" + model.band.ToString() + "/G" + model.gray.ToString()
                                                + " Compensation Skip (Target Lv : " + model.Target.double_Lv.ToString() + ") < " + model.Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                            Optic_Compensation_Succeed = true;
                                            break;
                                        }
                                        //Vreg1 + Sub-Compensation (Change Gamma Value)
                                        if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked
                                             && (checkBox_Triple_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false))
                                        {
                                            Vreg1_Infinite_Loop_Check(model.Vreg1_loop_count, model);

                                            if (model.Vreg1_loop_count < model.loop_count_max)
                                            {
                                                f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + model.Vreg1_loop_count.ToString(), Color.Blue);
                                                model.Prev_Vreg1 = model.Vreg1;
                                                model.Prev_Gamma.Equal_Value(model.Gamma);

                                                model.Vreg1_Compensation();

                                                f1.Showing_Diff_and_Current_Vreg1_and_Gamma(model.Prev_Gamma, model.Gamma, model.Prev_Vreg1, model.Vreg1);

                                                if (Math.Abs(model.Vreg1 - model.Prev_Vreg1) >= 1) model.Vreg1_Need_To_Be_Updated = true;
                                                else model.Vreg1_Need_To_Be_Updated = false;


                                                if (model.Vreg1_Need_To_Be_Updated)
                                                {
                                                    DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Gamma_Set.Set2);
                                                }
                                            }
                                            model.Vreg1_loop_count++;
                                            model.loop_count++;
                                            if (model.Vreg1_Infinite_Count >= 3)
                                            {
                                                Extension_Applied = "O";
                                                f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                            }
                                            else
                                                Extension_Applied = "X";
                                        }
                                        else
                                        {
                                            model.Vreg1_Need_To_Be_Updated = false;

                                            model.Prev_Gamma.Equal_Value(model.Gamma);
                                            Infinite_Loop_Check(model.loop_count, model);

                                            model.Sub_Compensation();

                                            //Engineering Mode
                                            f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);
                                            model.loop_count++;

                                            if (model.Infinite_Count >= 3)
                                            {
                                                Extension_Applied = "O";
                                                f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                            }
                                            else
                                                Extension_Applied = "X";
                                        }
                                        if (model.Vreg1_Need_To_Be_Updated == false)
                                        {
                                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set2, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                        }

                                        if (model.Within_Spec_Limit)
                                        {
                                            initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set2);


                                             //------Added on 200423-----------
                                            Triple_Set2_Band_Green_Offset_1(model, OC_Skip_Special_Process_Last_Lv);
                                            //--------------------------------


                                            Optic_Compensation_Succeed = true;
                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            break;
                                        }

                                        if (model.Gamma_Out_Of_Register_Limit)
                                        {
                                            if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                                Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set2); //Added on 200519
                                            else
                                                Optic_Compensation_Succeed = false;

                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");

                                            if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                            break;
                                        }

                                        textBox_loop_count.Text = (model.loop_count).ToString();

                                        if (model.loop_count == model.loop_count_max)
                                        {
                                            if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                                Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set2); //Added on 200519
                                            else
                                                Optic_Compensation_Succeed = false;

                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            System.Windows.Forms.MessageBox.Show("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Loop Count Over");

                                            if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                            break;
                                        }

                                        DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                        DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Gamma_Set.Set2); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                        Application.DoEvents();
                                    }
                                    f1.GB_ProgressBar_PerformStep();
                                }
                            }


                            if (model.Is_HBM_Band() || model.Is_Normal_Band())
                            {
                                DP173_form_Dual_engineer.Band_Radiobuttion_Select(model.band, Gamma_Set.Set3);
                                DP173_form_Dual_engineer.Force_Triger_Band_Set_CheckedChanged_Function(model.band, Gamma_Set.Set3); //In triple mode it must be needed !!! (200525)
                                Application.DoEvents();

                                Triple_Mode_Set3_Target_XYLv_Setting_From_Set12_Measure(model);
                                button_Gamma_Set3_Apply.PerformClick();//Set3 Apply
                                model.Vreg1_loop_count = 0; //Add on 191031
                                model.Vreg1_Infinite_Count = 0; //Add on 191031

                                //Set3
                                {
                                    double OC_Skip_Special_Process_Start_Lv = Convert.ToDouble(textBox_OC_Skip_Special_Process_Start_Lv_triple.Text);
                                    double OC_Skip_Special_Process_Last_Lv = Convert.ToDouble(textBox_OC_Skip_Special_Process_Last_Lv_triple.Text);
                                        
                                    if (checkBox_Triple_Gamma_Copy_Set2_to_Set3.Checked) DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set2_to_Set3(model.band, model.gray); //Copy Gamma (Set2 to Set3)
                                    else if(checkBox_Triple_Gamma_Copy_Set1_to_Set3.Checked)
                                    {
                                        DP173_form_Dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(model.band, Gamma_Set.Set3);
                                        DP173_Dual_Mode_Get_Param(model.gray, Gamma_Set.Set3, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table
                                         if (checkBox_Set2_Set3_OC_Skip_If_XY_and_deltaL_Are_within_Specs_Triple_Mode.Checked && model.Target.double_Lv <= OC_Skip_Special_Process_Start_Lv && model.Target.double_Lv >= OC_Skip_Special_Process_Last_Lv)
                                         {
                                            int Green_Offset = Convert.ToInt32(textBox_OC_Skip_Special_Process_G_Offset_triple_Set13.Text);
                                            f1.GB_Status_AppendText_Nextline("OC_Skip_Special_Process_Start_Lv >= Target.double_Lv >= OC_Skip_Special_Process_Last_Lv : " + OC_Skip_Special_Process_Start_Lv.ToString() + ">=" + model.Target.double_Lv.ToString() + ">=" + OC_Skip_Special_Process_Last_Lv.ToString(), Color.Green);
                                            f1.GB_Status_AppendText_Nextline("Green_Offset " + Green_Offset.ToString() + " has been applied", Color.Green);
                                            DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set1_to_Set3(model.band, model.gray, Green_Offset); //Copy Gamma (Set1 to Set3)
                                         }
                                         else
                                         {
                                             DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set1_to_Set3(model.band, model.gray);
                                         }
                                    }
                                    
                                    DP173_form_Dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(model.band, Gamma_Set.Set3);
                                    DP173_Dual_Get_All_Band_Gray_Gamma(Gamma_Set.Set3, model.All_band_gray_Gamma); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet

                                    if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                    {

                                        if (checkBox_Triple_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked) model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set1);
                                        else model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set3); //Read Vreg1 Value (Gamma_Set 2's Vreg1 Value = Gamma_Set 1's Vreg1 Value);

                                        DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Gamma_Set.Set3); //Add on 190702 (Condition 1 꺼를 읽은거기 때문에 먼저 Condition2 꺼에 초기 Vreg1 세팅 필요)
                                        model.Initial_Vreg1 = model.Vreg1; //Add on 190603
                                        model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1; //Add on 190603
                                    }

                                    if (Optic_Compensation_Stop) break;
                                    DP173_Dual_Mode_Get_Param(model.gray, Gamma_Set.Set3, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table   
                                    f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

                                    if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && model.Vreg1_loop_count == 0
                                         && checkBox_Triple_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                                    {
                                        DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, 0, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Gamma_Set.Set3);
                                        model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                    }

                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set3, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                    Thread.Sleep(20);
                                    DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count
                                        , model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                    model.loop_count = 0;
                                    model.Infinite_Count = 0;
                                    DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set3); //Add on 190614
                                    f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                    Application.DoEvents();

                                    Optic_Compensation_Succeed = false;
                                    model.Within_Spec_Limit = false;

                                    bool Skip_Set3_OC = Is_Triple_Mode_Set3_OC_Skip(model);//Added On 200521
                                    if (Skip_Set3_OC)
                                    {
                                        Triple_Set3_Band_Green_Offset_1(model, OC_Skip_Special_Process_Last_Lv);
                                        DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                        DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Gamma_Set.Set3); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                        Application.DoEvents();

                                        initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set3);
                                    }

                                    while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false && Skip_Set3_OC == false)
                                    {
                                        //if (Target.double_Lv < Skip_Lv)//Deleted on 200221
                                        if (Current_Gray_GB_Skip) //Added on 200221
                                        {
                                            if (model.band >= 1)
                                            {
                                                DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Gamma_Set.Set3);
                                                model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                            }
                                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set3, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

                                            initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set3);
                                            
                                            model.Measure.Set_Value(0, 0, 0);
                                            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set3); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                            f1.GB_Status_AppendText_Nextline("B" + model.band.ToString() + "/G" + model.gray.ToString()
                                                + " Compensation Skip (Target Lv : " + model.Target.double_Lv.ToString() + ") < " + model.Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                            Optic_Compensation_Succeed = true;
                                            break;
                                        }
                                        //Vreg1 + Sub-Compensation (Change Gamma Value)
                                        if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked
                                             && (checkBox_Triple_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false))
                                        {


                                            Vreg1_Infinite_Loop_Check(model.Vreg1_loop_count, model);

                                            if (model.Vreg1_loop_count < model.loop_count_max)
                                            {
                                                f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + model.Vreg1_loop_count.ToString(), Color.Blue);
                                                model.Prev_Vreg1 = model.Vreg1;
                                                model.Prev_Gamma.Equal_Value(model.Gamma);

                                                model.Vreg1_Compensation();

                                                f1.Showing_Diff_and_Current_Vreg1_and_Gamma(model.Prev_Gamma, model.Gamma, model.Prev_Vreg1, model.Vreg1);

                                                if (Math.Abs(model.Vreg1 - model.Prev_Vreg1) >= 1) model.Vreg1_Need_To_Be_Updated = true;
                                                else model.Vreg1_Need_To_Be_Updated = false;


                                                if (model.Vreg1_Need_To_Be_Updated)
                                                {
                                                    DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Gamma_Set.Set3);
                                                }
                                            }
                                            model.Vreg1_loop_count++;
                                            model.loop_count++;
                                            if (model.Vreg1_Infinite_Count >= 3)
                                            {
                                                Extension_Applied = "O";
                                                f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                            }
                                            else
                                                Extension_Applied = "X";
                                        }
                                        else
                                        {
                                            model.Vreg1_Need_To_Be_Updated = false;

                                            model.Prev_Gamma.Equal_Value(model.Gamma);
                                            Infinite_Loop_Check(model.loop_count, model);

                                            model.Sub_Compensation();

                                            //Engineering Mode
                                            f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);
                                            model.loop_count++;

                                            if (model.Infinite_Count >= 3)
                                            {
                                                Extension_Applied = "O";
                                                f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                            }
                                            else
                                                Extension_Applied = "X";
                                        }
                                        if (model.Vreg1_Need_To_Be_Updated == false)
                                        {
                                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set3, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                        }

                                        if (model.Within_Spec_Limit)
                                        {
                                            initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set3);

                                            //------Added on 200423-----------
                                            Triple_Set3_Band_Green_Offset_1(model, OC_Skip_Special_Process_Last_Lv);
                                            //--------------------------------


                                            Optic_Compensation_Succeed = true;
                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            break;
                                        }

                                        if (model.Gamma_Out_Of_Register_Limit)
                                        {
                                            if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                                Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set3); //Added on 200519
                                            else
                                                Optic_Compensation_Succeed = false;

                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");

                                            if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                            break;
                                        }

                                        textBox_loop_count.Text = (model.loop_count).ToString();

                                        if (model.loop_count == model.loop_count_max)
                                        {
                                            if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                                Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set3); //Added on 200519
                                            else
                                                Optic_Compensation_Succeed = false;

                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            System.Windows.Forms.MessageBox.Show("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Loop Count Over");

                                            if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                            break;
                                        }

                                        DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                        DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Gamma_Set.Set3); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                        Application.DoEvents();
                                    }
                                    f1.GB_ProgressBar_PerformStep();
                                }
                            }
                        }
                        if (checkBox_Only_255G.Checked) model.gray = 8;
                    }//Gray Loop End
                    if (model.Is_AOD_Band()) f1.AOD_Off();
                }
            }//Band Loop End

            //---Set4----
            if (checkBox_Triple_Gamma_Copy_Set1_to_Set4.Checked && Optic_Compensation_Stop == false) Copy_All_Band_Gamma_From_Set1_To_Set4();
            if (checkBox_Triple_Mode_Set4_OC_Skip.Checked == false && Optic_Compensation_Stop == false) Set4_Optic_Compensation(Set4_Skip_Band, model.oc_mode, initial_RGBVreg1_cal);//Single Set4 Optic Compensation

            //---Set5----
            if (checkBox_Triple_Gamma_Copy_Set2_to_Set5.Checked && Optic_Compensation_Stop == false) Copy_All_Band_Gamma_From_Set2_To_Set5();
            if (checkBox_Triple_Mode_Set5_OC_Skip.Checked == false && Optic_Compensation_Stop == false) Set5_Optic_Compensation(Set5_Skip_Band, model.oc_mode, initial_RGBVreg1_cal);//Single Set5 Optic Compensation

            //---Set6----
            if (checkBox_Triple_Gamma_Copy_Set3_to_Set6.Checked && Optic_Compensation_Stop == false) Copy_All_Band_Gamma_From_Set3_To_Set6();
            if (checkBox_Triple_Mode_Set6_OC_Skip.Checked == false && Optic_Compensation_Stop == false) Set6_Optic_Compensation(Set6_Skip_Band, model.oc_mode, initial_RGBVreg1_cal);//Single Set6 Optic Compensation

            f1.OC_Timer_Stop();
            DP173_DBV_Setting(1);  //DBV Setting    
            f1.PTN_update(255, 255, 255);

            System.Windows.Forms.MessageBox.Show("Optic Compensation Finished !");

            //--After Dual Compensation Finished--
            DP173_form_Dual_engineer.Dual_RadioButton_All_Enable(true);
            DP173_form_Dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(false);

            /*
            initial_RGBVreg1_cal.Show_Calculated_Vdata();
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set1);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set2);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set3);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set4);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set5);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set6);
            */
        }


        private bool Is_Dual_Mode_Set2_OC_Skip(DP173_or_Elgin model, double OC_Skip_Special_Process_Last_Lv)
        {
           
            bool X_Spec_In = false;
            bool Y_Spec_In = false;
            bool Delta_L_Spec_In = false;
            bool Delta_UV_Spec_In = false;

            if (checkBox_Set2_OC_Skip_If_XY_and_deltaL_Are_within_Specs.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];

                XYLv Set1_Measured = DP173_form_Dual_engineer.Get_Set_Measured_Values(Gamma_Set.Set1, model.band, model.gray);
                XYLv Set2_Measured = DP173_form_Dual_engineer.Get_Set_Measured_Values(Gamma_Set.Set2, model.band, model.gray);
                f1.GB_Status_AppendText_Nextline("Set1 Measured B" + model.band.ToString() + "/" + model.gray.ToString() + " X/Y/LV : " + Set1_Measured.X + "/" + Set1_Measured.Y + "/" + Set1_Measured.Lv, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Set2 Measured B" + model.band.ToString() + "/" + model.gray.ToString() + " X/Y/LV : " + Set2_Measured.X + "/" + Set2_Measured.Y + "/" + Set2_Measured.Lv, Color.Blue);
                double Diff_Delta_L_Spec = Set2_Diff_Delta_L_Spec[model.band, model.gray];
                double UV_Distance_Limit = Set2_Diff_Delta_UV_Spec[model.band, model.gray];

                if (radioButton_OC_Skip_XY_Dual_Mode.Checked)
                {
                    X_Spec_In = Compare_X(Set1_Measured, Set2_Measured, model);
                    Y_Spec_In = Compare_Y(Set1_Measured, Set2_Measured, model);
                }
                else if (radioButton_OC_Skip_UV_Dual_Mode.Checked)
                {
                    Delta_UV_Spec_In = Compare_Delta_UV(Set1_Measured, Set2_Measured, UV_Distance_Limit);
                }
                Delta_L_Spec_In = Compare_Delta_L(Set1_Measured, Set2_Measured, Diff_Delta_L_Spec);
            }
            return Get_Skip_Set2_OC(X_Spec_In, Y_Spec_In, Delta_L_Spec_In, Delta_UV_Spec_In, OC_Skip_Special_Process_Last_Lv, model.Target.double_Lv);
        }


        private bool Is_Triple_Mode_Set2_OC_Skip(DP173_or_Elgin model)
        {
            bool X_Spec_In = false;
            bool Y_Spec_In = false;
            bool Delta_L_Spec_In = false;
            bool Delta_UV_Spec_In = false;

            if (checkBox_Set2_Set3_OC_Skip_If_XY_and_deltaL_Are_within_Specs_Triple_Mode.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];

                XYLv Set1_Measured = DP173_form_Dual_engineer.Get_Set_Measured_Values(Gamma_Set.Set1, model.band, model.gray);
                XYLv Set2_Measured = DP173_form_Dual_engineer.Get_Set_Measured_Values(Gamma_Set.Set2, model.band, model.gray);
                f1.GB_Status_AppendText_Nextline("Set1 Measured B" + model.band.ToString() + "/" + model.gray.ToString() + " X/Y/LV : " + Set1_Measured.X + "/" + Set1_Measured.Y + "/" + Set1_Measured.Lv, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Set2 Measured B" + model.band.ToString() + "/" + model.gray.ToString() + " X/Y/LV : " + Set2_Measured.X + "/" + Set2_Measured.Y + "/" + Set2_Measured.Lv, Color.Blue);
                double Diff_Delta_L_Spec = Set2_Diff_Delta_L_Spec[model.band, model.gray];
                double UV_Distance_Limit = Set2_Diff_Delta_UV_Spec[model.band, model.gray];

                if (radioButton_OC_Skip_XY_Triple_Mode.Checked)
                {
                    X_Spec_In = Compare_X(Set1_Measured, Set2_Measured, model);
                    Y_Spec_In = Compare_Y(Set1_Measured, Set2_Measured, model);
                }
                else if (radioButton_OC_Skip_UV_Triple_Mode.Checked)
                {
                    Delta_UV_Spec_In = Compare_Delta_UV(Set1_Measured, Set2_Measured, UV_Distance_Limit);
                }
                Delta_L_Spec_In = Compare_Delta_L(Set1_Measured, Set2_Measured, Diff_Delta_L_Spec);
            }
            return Triple_Model_Get_Skip_Set23_OC(X_Spec_In, Y_Spec_In, Delta_L_Spec_In, Delta_UV_Spec_In, model.Target.double_Lv);
        }


        private bool Is_Triple_Mode_Set3_OC_Skip(DP173_or_Elgin model)
        {
            bool X_Spec_In = false;
            bool Y_Spec_In = false;
            bool Delta_L_Spec_In = false;
            bool Delta_UV_Spec_In = false;

            if (checkBox_Set2_Set3_OC_Skip_If_XY_and_deltaL_Are_within_Specs_Triple_Mode.Checked)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
               
                XYLv Set1_Measured = DP173_form_Dual_engineer.Get_Set_Measured_Values(Gamma_Set.Set1, model.band, model.gray);
                XYLv Set3_Measured = DP173_form_Dual_engineer.Get_Set_Measured_Values(Gamma_Set.Set3, model.band, model.gray);
                f1.GB_Status_AppendText_Nextline("Set1 Measured B" + model.band.ToString() + "/" + model.gray.ToString() + " X/Y/LV : " + Set1_Measured.X + "/" + Set1_Measured.Y + "/" + Set1_Measured.Lv, Color.Blue);
                f1.GB_Status_AppendText_Nextline("Set3 Measured B" + model.band.ToString() + "/" + model.gray.ToString() + " X/Y/LV : " + Set3_Measured.X + "/" + Set3_Measured.Y + "/" + Set3_Measured.Lv, Color.Blue);
                double Diff_Delta_L_Spec = Set2_Diff_Delta_L_Spec[model.band, model.gray];
                double UV_Distance_Limit = Set2_Diff_Delta_UV_Spec[model.band, model.gray];

                if (radioButton_OC_Skip_XY_Triple_Mode.Checked)
                {
                    X_Spec_In = Compare_X(Set1_Measured, Set3_Measured, model);
                    Y_Spec_In = Compare_Y(Set1_Measured, Set3_Measured, model);
                }
                else if (radioButton_OC_Skip_UV_Triple_Mode.Checked)
                {
                    Delta_UV_Spec_In = Compare_Delta_UV(Set1_Measured, Set3_Measured, UV_Distance_Limit);
                }
                Delta_L_Spec_In = Compare_Delta_L(Set1_Measured, Set3_Measured, Diff_Delta_L_Spec);
            }
            return Triple_Model_Get_Skip_Set23_OC(X_Spec_In, Y_Spec_In, Delta_L_Spec_In, Delta_UV_Spec_In, model.Target.double_Lv);
        }




        private bool Compare_X(XYLv Set1_Measured, XYLv Set2_Measured, DP173_or_Elgin model)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            XYLv Set1_Minus_Set2 = (Set1_Measured - Set2_Measured);
            bool X_Spec_In;

            if (Math.Abs(Set1_Minus_Set2.double_X) < model.Limit.double_X)
            {
                X_Spec_In = true;
                f1.GB_Status_AppendText_Nextline("Math.Abs(Set1_Minus_Set2.double_X)(" + Math.Abs(Set1_Minus_Set2.double_X) + ") < Limit.double_X(" + model.Limit.double_X.ToString() + "), X_Spec_In = true)", Color.Green);
            }
            else
            {
                X_Spec_In = false;
                f1.GB_Status_AppendText_Nextline("Math.Abs(Set1_Minus_Set2.double_X)(" + Math.Abs(Set1_Minus_Set2.double_X) + ") >= Limit.double_X(" + model.Limit.double_X.ToString() + "), X_Spec_In = false)", Color.Red);
            }
            return X_Spec_In;
        }

        private bool Compare_Y(XYLv Set1_Measured, XYLv Set2_Measured, DP173_or_Elgin model)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            XYLv Set1_Minus_Set2 = (Set1_Measured - Set2_Measured);
            bool Y_Spec_In;

            if (Math.Abs(Set1_Minus_Set2.double_Y) < model.Limit.double_Y)
            {
                Y_Spec_In = true;
                f1.GB_Status_AppendText_Nextline("Math.Abs(Set1_Minus_Set2.double_Y)(" + Math.Abs(Set1_Minus_Set2.double_Y) + ") < Limit.double_Y(" + model.Limit.double_Y.ToString() + "), Y_Spec_In = true)", Color.Green);
            }
            else
            {
                Y_Spec_In = false;
                f1.GB_Status_AppendText_Nextline("Math.Abs(Set1_Minus_Set2.double_Y)(" + Math.Abs(Set1_Minus_Set2.double_Y) + ") >= Limit.double_Y(" + model.Limit.double_Y.ToString() + "), Y_Spec_In = false)", Color.Red);
            }

            return Y_Spec_In;
        }

        private bool Compare_Delta_UV(XYLv Set1_Measured, XYLv Set2_Measured, double UV_Distance_Limit)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            double UV_Distance = Color_Coordinate.Get_UV_Distance_From_XY(Set1_Measured.double_X, Set1_Measured.double_Y, Set2_Measured.double_X, Set2_Measured.double_Y);
            bool UV_Spec_In;

            if (UV_Distance < UV_Distance_Limit)
            {
                UV_Spec_In = true;
                f1.GB_Status_AppendText_Nextline("UV_Distance(" + UV_Distance.ToString() + ") < UV_Distance_Limit(" + UV_Distance_Limit.ToString() + "), UV_Spec_In = true)", Color.Green);
            }
            else
            {
                UV_Spec_In = false;
                f1.GB_Status_AppendText_Nextline("UV_Distance(" + UV_Distance.ToString() + ") >= UV_Distance_Limit(" + UV_Distance_Limit.ToString() + "), UV_Spec_In = false)", Color.Red);
            }

            return UV_Spec_In;
        }

        private bool Compare_Delta_L(XYLv Set1_Measured, XYLv Set2_Measured, double Diff_Delta_L_Spec)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            XYLv Set1_Minus_Set2 = (Set1_Measured - Set2_Measured);
            double Delta_L = Math.Abs(Set1_Minus_Set2.double_Lv / (Set1_Measured.double_Lv));
            bool Delta_L_Spec_In;

            if (Delta_L < Diff_Delta_L_Spec)
            {
                Delta_L_Spec_In = true;
                f1.GB_Status_AppendText_Nextline("Delta_L(" + Delta_L.ToString() + ") < Diff_Delta_L_Spec(" + Diff_Delta_L_Spec.ToString() + "), Delta_L_Spec_In = true)", Color.Green);
            }
            else
            {
                Delta_L_Spec_In = false;
                f1.GB_Status_AppendText_Nextline("Delta_L(" + Delta_L.ToString() + ") >= Diff_Delta_L_Spec(" + Diff_Delta_L_Spec.ToString() + "), Delta_L_Spec_In = false)", Color.Red);
            }

            return Delta_L_Spec_In;
        }



        bool Triple_Model_Get_Skip_Set23_OC(bool X_Spec_In, bool Y_Spec_In, bool Delta_L_Spec_In, bool Delta_UV_Spec_In, double Target_Lv)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            bool Skip_Set23_OC;
            if (X_Spec_In && Y_Spec_In && Delta_L_Spec_In)
            {
                Skip_Set23_OC = true;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_or_Set3_OC(XY) = true", Color.Green);
            }
            else if (Delta_UV_Spec_In && Delta_L_Spec_In)
            {
                Skip_Set23_OC = true;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_or_Set3_OC(UV) = true", Color.Green);
            }
            else
            {
                Skip_Set23_OC = false;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_or_Set3_OC = false", Color.Red);
            }
            return Skip_Set23_OC;
        }

        bool Get_Skip_Set2_OC(bool X_Spec_In, bool Y_Spec_In, bool Delta_L_Spec_In, bool Delta_UV_Spec_In, double OC_Skip_Special_Process_Last_Lv, double Target_Lv)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            bool Skip_Set2_OC;

            if (Target_Lv < OC_Skip_Special_Process_Last_Lv)
            {
                Skip_Set2_OC = false;
                f1.GB_Status_AppendText_Nextline("Target_Lv < OC_Skip_Special_Process_Last_Lv(" + Target_Lv.ToString() + "<" + "OC_Skip_Special_Process_Last_Lv)", Color.Red);
                f1.GB_Status_AppendText_Nextline("Skip_Set2_OC = false", Color.Red);
            }

            else if (X_Spec_In && Y_Spec_In && Delta_L_Spec_In)
            {
                Skip_Set2_OC = true;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_OC(XY) = true", Color.Green);
            }
            else if (Delta_UV_Spec_In && Delta_L_Spec_In)
            {
                Skip_Set2_OC = true;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_OC(UV) = true", Color.Green);
            }
            else
            {
                Skip_Set2_OC = false;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_OC = false", Color.Red);
            }
            return Skip_Set2_OC;
        }



        private void Dual_Or_Triple_Mode_Initialize()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Dual_Engineering_Mornitoring.getInstance().Show();

            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_form_Dual_engineer.button_Read_OC_Param_From_Excel_File_Perform_Click();//added on 200317
            DP173_form_Dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(true);
            DP173_form_Dual_engineer.Dual_Mode_GridView_Measure_Applied_Loop_Area_Data_Clear();
            DP173_form_Dual_engineer.Dual_RadioButton_All_Enable(false);

            DP173_form_Dual_engineer.Band_Radiobuttion_Select(0, Gamma_Set.Set1); //Select Band as 0 (Set 1)
            DP173_form_Dual_engineer.Force_Triger_Band_Set_CheckedChanged_Function(0, Gamma_Set.Set1); //Make sure to update sub_grid as main_grid's band0 
            DP173_form_Dual_engineer.Band_Radiobuttion_Select(0, Gamma_Set.Set2); //Select Band as 0 (Set 2)
            DP173_form_Dual_engineer.Force_Triger_Band_Set_CheckedChanged_Function(0, Gamma_Set.Set2);//Make sure to update sub_grid as main_grid's band0

            button_Gamma_Set1_Apply.PerformClick();//Set1 Apply

            Optic_Compensation_Stop = false;

            //CA channel Selection
            f1.objMemory.ChannelNO = Convert.ToInt32(f1.textBox_ch.Text);
            f1.CA_Channel_trackBar.Value = Convert.ToInt32(f1.textBox_ch.Text);

            //Timer Start
            f1.OC_Timer_Start();

            //ProgressBar-related
            int step = 1;
            f1.Set_GB_ProgressBar_Value(0); //Progressbar Value's initializing
            ProgressBar_Max_Step_Setting(step * 2); //Set Progressbar's Step and Max-Value
            f1.Add_GB_ProgressBar_Maximum(22);//Add Set3 & Se4 Compensation Band (11ea * 2ea = 20ea)

            //Global Value Initializing
            Optic_Compensation_Succeed = false;

            if (checkBox_Read_DBV_Values.Checked) button_Read_DP173_DBV_Setting.PerformClick();
        }

        private void Dual_ELVSS_VREF1_Compensation(DP173_or_Elgin model, RGB[,] All_band_gray_Gamma)
        {
            //ELVSS OC
            if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false)
            {
                if (radioButton_ELVSS_Start_From_Band0_First_ELVSS_60.Checked)
                {
                    int start_band = 0;
                    double First_ELVSS = -6.0;
                    double Last_ELVSS = -2.0;
                    DP173_Dual_Band_Gray255_Compensation(start_band, All_band_gray_Gamma, Gamma_Set.Set1, model);
                    DP173_ELVSS_Compensation(start_band, First_ELVSS, Last_ELVSS);
                }
                else if (radioButton_ELVSS_Start_From_Band1_First_ELVSS_45.Checked)
                {
                    int start_band = 1;
                    double First_ELVSS = -4.5;
                    double Last_ELVSS = -2.0;
                    DP173_Dual_Band_Gray255_Compensation(start_band, All_band_gray_Gamma, Gamma_Set.Set1, model);
                    DP173_ELVSS_Compensation(start_band, First_ELVSS, Last_ELVSS);
                }
            }

            //REF1 Compensation
            if (checkBox_VREF1_Comp.Checked && Optic_Compensation_Stop == false)
            {
                int band = 0;
                int gray = 0;
                DP173_Dual_Band_Gray255_Compensation(band, All_band_gray_Gamma, Gamma_Set.Set1, model);

                RGB HBM_White_Gamma = new RGB();
                HBM_White_Gamma.Equal_Value(All_band_gray_Gamma[band, gray]);
                double Set1_HBM_RGB_Min_White = Get_Set1_HBM_RGB_Min_White(HBM_White_Gamma);
                DP173_VREF1_Compensation(Set1_HBM_RGB_Min_White);
            }
        }

        private void Black_Compensation(DP173_or_Elgin model)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (radioButton_Black_Compensation.Checked && Optic_Compensation_Stop == false)
            {
                if (Sub_Black_Compensation(model))
                {
                    f1.GB_Status_AppendText_Nextline("Black Compensation Ok", Color.Blue);
                    button_Read_AM0_VREF2_HBM_Set1_Only.PerformClick();//Added On 200219 (VREF1/VREF2047/AM0 Read)
                    model.Update_SET1_HBM_AM0_Hex();
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("Black Compensation NG", Color.Red);
                    Optic_Compensation_Stop = true;
                }
            }
        }


        private void Dual_Triple_Calculate_Init_RGBVreg1_And_Apply_And_Measure(DP173_or_Elgin model, bool[] Applied_Band, Gamma_Set Set, Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DataGridView dataGridView_OC_param = DP173_form_Dual_engineer.Get_OC_Param_DataGridView(Set);
            initial_RGBVreg1_cal.Set_Calculated_RGBVreg1_Vdata_Pointer(Set);

            double Before_Calculated_Init_Vreg1 = 0;
            bool Gray255_Calculated = false;

            if (checkBox_Vreg1_Compensation.Checked && model.gray == 0)
            {
                model.Vreg1_loop_count = 0; //Vreg1 loop countR
                model.Vreg1_Infinite_Count = 0;
                model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                Before_Calculated_Init_Vreg1 = model.Vreg1;

                if ((checkBox_Apply_FX_Previous_Band_Vreg1.Checked) && (model.band >= 1) && (model.band < 11) && (initial_RGBVreg1_cal.Selected_Band[model.band]) && (model.gray == 0) && Applied_Band[model.band])
                {
                    Gray255_Calculated = true;

                    double band_Target_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[(model.band * 8) + (0 + 2)].Cells[9].Value);
                    int[] Previous_Band_Gamma_Red = new int[8];
                    int[] Previous_Band_Gamma_Green = new int[8];
                    int[] Previous_Band_Gamma_Blue = new int[8];
                    double[] Previous_Band_Target_Lv = new double[8];
                    for (int i = 0; i < 8; i++)
                    {
                        Previous_Band_Gamma_Red[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[1].Value);
                        Previous_Band_Gamma_Green[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[2].Value);
                        Previous_Band_Gamma_Blue[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[3].Value);
                        if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[6].Value);
                        else if (this.radioButton_Previous_Target_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[9].Value);
                    }

                    int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1(model.band - 1, Set);//Prev_Band Vreg1

                    //C++ Dll Initial Calculate R/Vreg1/B Verify OK (= C# Result)
                    Imported_my_cpp_dll.DP173_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.Vreg1, ref model.Vreg1_First_Gamma_Red, ref model.Vreg1_First_Gamma_Blue, initial_RGBVreg1_cal.Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, model.band, band_Target_Lv, Previous_Band_Vreg1_Dec
, Previous_Band_Target_Lv, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                    f1.GB_Status_AppendText_Nextline("(1)After(Dll C++,Precision : 0.001) Vreg1_First_Gamma_Red/Vreg1/Vreg1_First_Gamma_Blue : " + model.Vreg1_First_Gamma_Red.ToString() + "/" + model.Vreg1.ToString() + "/" + model.Vreg1_First_Gamma_Blue.ToString(), Color.Red);

                    //Set Calculated Vreg1_dec
                    DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);
                }
                else
                {
                    Gray255_Calculated = false;
                }
            }

            else
            {
                if ((checkBox_Apply_FX_Previous_Band_Vreg1.Checked) && (model.band >= 1) && (model.band < 11) && (initial_RGBVreg1_cal.Selected_Band[model.band]) && (model.gray == 0) && Applied_Band[model.band])
                {
                    model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                    Gray255_Calculated = true;

                    double band_Target_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[(model.band * 8) + (0 + 2)].Cells[9].Value);
                    int[] Previous_Band_Gamma_Red = new int[8];
                    int[] Previous_Band_Gamma_Green = new int[8];
                    int[] Previous_Band_Gamma_Blue = new int[8];
                    double[] Previous_Band_Target_Lv = new double[8];
                    for (int i = 0; i < 8; i++)
                    {
                        Previous_Band_Gamma_Red[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[1].Value);
                        Previous_Band_Gamma_Green[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[2].Value);
                        Previous_Band_Gamma_Blue[i] = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[3].Value);
                        if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[6].Value);
                        else if (this.radioButton_Previous_Target_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[9].Value);
                    }

                    int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1(model.band - 1, Set);//Prev_Band Vreg1

                    Imported_my_cpp_dll.DP173_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(model.Vreg1, model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.G255_First_Gamma_Red, ref model.G255_First_Gamma_Green, ref model.G255_First_Gamma_Blue, initial_RGBVreg1_cal.Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, model.band, band_Target_Lv, Previous_Band_Vreg1_Dec
                                                                                        , Previous_Band_Target_Lv, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                    f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ (Gray255) After Gamma_R/G/B : " + model.G255_First_Gamma_Red.ToString() + "/" + model.G255_First_Gamma_Green.ToString() + "/" + model.G255_First_Gamma_Blue.ToString(), Color.Blue);

                    //Set Calculated Vreg1_dec
                    DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);

                }
                else
                {
                    Gray255_Calculated = false;
                }
            }
            //---------------------------------




            DP173_Dual_Get_All_Band_Gray_Gamma(Set, model.All_band_gray_Gamma); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet

            if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked)
            {
                model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set); //Read Vreg1 Value
                DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);
                model.Initial_Vreg1 = model.Vreg1; //Add on 190603
                model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1; //Add on 190603
            }

            DP173_Dual_Mode_Get_Param(model.gray, Set, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table
            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

            //----------Add on 200316----------
            if ((model.Target.double_Lv > Convert.ToDouble(textBox_Fast_OC_RGB_Skip_Target.Text)) && (checkBox_Apply_FX_3points_RGB.Checked) && (model.band >= 1) && (model.band < 11) && (model.gray >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]) && Applied_Band[model.band])
            {
                f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C# (All Band , 3points, Combine) Before Gamma_R/G/B : " + model.Gamma.int_R.ToString() + "/" + model.Gamma.int_G.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);

                int Previous_Band_G255_Green_Gamma = Convert.ToInt32(dataGridView_OC_param.Rows[((model.band - 1) * 8) + (0 + 2)].Cells[2].Value);
                int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1((model.band - 1), Set);
                int Current_Band_Dec_Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);
                double Prvious_Gray_Gamma_R_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_R;
                double Prvious_Gray_Gamma_G_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_G;
                double Prvious_Gray_Gamma_B_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_B;

                SJH_Matrix M = new SJH_Matrix();

                int[] Band_Gray_Gamma_Red = new int[(model.band * 8)]; //Previous Bands
                int[] Band_Gray_Gamma_Green = new int[(model.band * 8)];
                int[] Band_Gray_Gamma_Blue = new int[(model.band * 8)];
                double[] Band_Gray_Target_Lv = new double[(model.band * 8)];
                int[] Band_Vreg1_Dec = new int[model.band + 1];//Previous Bands + Current Band
                Band_Vreg1_Dec[model.band] = DP173_Get_Normal_Initial_Vreg1(model.band, Set);
                //-----------Added On 200312---------------------
                int second_band_end = Convert.ToInt32(numericUpDown2.Value);
                if (model.band <= second_band_end) //(band > first_band_end && band <= second_band_end), ex)B4,B5,B6,..,B10 (it means : min(second_band_start) > first_band_end)
                {
                    for (int b = 0; b < model.band; b++)
                    {
                        for (int g = 0; g < 8; g++)
                        {
                            Band_Gray_Gamma_Red[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[1].Value);
                            Band_Gray_Gamma_Green[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[2].Value);
                            Band_Gray_Gamma_Blue[(b * 8) + g] = Convert.ToInt32(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[3].Value);
                            if (radioButton_Previous_Measure_Lv.Checked) Band_Gray_Target_Lv[(b * 8) + g] = Convert.ToDouble(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[6].Value);
                            else if (this.radioButton_Previous_Target_Lv.Checked) Band_Gray_Target_Lv[(b * 8) + g] = Convert.ToDouble(dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[9].Value);
                        }
                        Band_Vreg1_Dec[b] = DP173_Get_Normal_Initial_Vreg1(b, Set);
                    }
                }
                double Fx_3points_Combine_LV_1 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_1.Text);
                double Fx_3points_Combine_LV_2 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_2.Text);
                double Fx_3points_Combine_LV_3 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_3.Text);
                double Fx_3points_Combine_Lv_Distance = Convert.ToDouble(textBox_Fx_3points_Lv_Distance_Combine.Text);


                double Combine_Lv_Ratio = Convert.ToDouble(textBox_Fast_OC_3Points_Ver2_LV_Combine_Ratio.Text);

                Imported_my_cpp_dll.Get_Initial_Gamma_Fx_3points_Combine_Points_2(Combine_Lv_Ratio, model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.Gamma.int_R, ref model.Gamma.int_G, ref model.Gamma.int_B, initial_RGBVreg1_cal.Selected_Band, Band_Gray_Gamma_Red, Band_Gray_Gamma_Green, Band_Gray_Gamma_Blue, Band_Gray_Target_Lv, Current_Band_Dec_Vreg1,
                        Band_Vreg1_Dec, model.band, model.gray, model.Target.double_Lv, Prvious_Gray_Gamma_R_Voltage, Prvious_Gray_Gamma_G_Voltage, Prvious_Gray_Gamma_B_Voltage,
                        Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1
                        , Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance);
                f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ Ver2 (All Band , 3points, Combine) After Gamma_R/G/B : " + model.Gamma.int_R.ToString() + "/" + model.Gamma.int_G.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);
            }
            else if (checkBox_Vreg1_Compensation.Checked == false && Gray255_Calculated == true && model.gray == 0)
            {
                model.Gamma.int_R = model.G255_First_Gamma_Red;
                model.Gamma.int_G = model.G255_First_Gamma_Green;
                model.Gamma.int_B = model.G255_First_Gamma_Blue;
            }
            else
            {

            }
            //---------------------------------

            //-------------Added on 200316-----------
            if (checkBox_Copy_Apply_Band_From_Upper_To_Lower.Checked && (model.band > 0) && (model.band < 11) && (model.gray == 0) && initial_RGBVreg1_cal.Selected_Band[model.band] && Applied_Band[model.band])
            {
                DP173_form_Dual_engineer.Copy_Previous_Band_Gamma(model.band, Set); Application.DoEvents();
                DP173_form_Dual_engineer.Get_Band_Gray_Gamma(model.All_band_gray_Gamma, model.band, Set); Application.DoEvents();
                Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
            }
            //---------------------------------------

            if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && model.Vreg1_loop_count == 0)
            {
                DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, 0, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Set);
                model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                //-------------Added on 200316-----------
                if (checkBox_Apply_FX_Previous_Band_Vreg1.Checked && initial_RGBVreg1_cal.Selected_Band[model.band] && Applied_Band[model.band])
                {
                    f1.GB_Status_AppendText_Nextline("(1)Before R/Vreg1/B : " + model.Gamma.int_R.ToString() + "/" + Before_Calculated_Init_Vreg1.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);
                    model.Gamma.int_R = model.Vreg1_First_Gamma_Red;
                    model.Gamma.int_B = model.Vreg1_First_Gamma_Blue;
                    f1.GB_Status_AppendText_Nextline("(1)After R/Vreg1/B : " + model.Gamma.int_R.ToString() + "/" + model.Vreg1.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Red);
                }
                //---------------------------------------
            }
        }

        private void Triple_Set3_Band_Green_Offset_1(DP173_or_Elgin model, double OC_Skip_Special_Process_Last_Lv)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
            //------Added on 200423-----------
            if (model.band >= 1
                && model.Gamma.int_G <= 510
                && (checkBox_Set2_Set3_OC_Skip_If_XY_and_deltaL_Are_within_Specs_Triple_Mode.Checked)
                && model.Target.double_Lv < OC_Skip_Special_Process_Last_Lv)
            {
                int Prev_Band_Green = Get_Set3_Band_Gray_Green_Gamma(model.band - 1, model.gray);

                f1.GB_Status_AppendText_Nextline(" Target.double_Lv (< OC_Skip_Special_Process_Last_Lv) = " + model.Target.double_Lv.ToString() + "(<" + OC_Skip_Special_Process_Last_Lv.ToString() + ")", Color.Blue);
                f1.GB_Status_AppendText_Nextline("(Gamma.int_G - Prev_Band_Green) = " + (model.Gamma.int_G - Prev_Band_Green).ToString(), Color.Blue);
                f1.GB_Status_AppendText_Nextline("[Before]Gamma.G" + model.Gamma.int_G.ToString(), Color.Green);
                if ((model.Gamma.int_G - Prev_Band_Green) == 1)
                {
                    model.Gamma.int_G++;
                    Send_Set_Gamma_And_Measure_And_Update_GridView(model.All_band_gray_Gamma, model, Gamma_Set.Set3);
                    f1.GB_Status_AppendText_Nextline("[Process1]Gamma.G" + model.Gamma.int_G.ToString(), Color.Black);
                    if (Get_Is_Skip_Set2_OC(model.band, model.gray, model) == false)
                    {
                        model.Gamma.int_G--;
                        Send_Set_Gamma_And_Measure_And_Update_GridView(model.All_band_gray_Gamma, model, Gamma_Set.Set3);
                        f1.GB_Status_AppendText_Nextline("[Process2]Gamma.G" + model.Gamma.int_G.ToString(), Color.Black);
                    }
                }
                f1.GB_Status_AppendText_Nextline("[After]Gamma.G" + model.Gamma.int_G.ToString(), Color.Red);
            }
            //--------------------------------
        }


        private void Triple_Set2_Band_Green_Offset_1(DP173_or_Elgin model, double OC_Skip_Special_Process_Last_Lv)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //------Added on 200423-----------
            if (model.band >= 1
                && model.Gamma.int_G <= 510
                && (checkBox_Set2_Set3_OC_Skip_If_XY_and_deltaL_Are_within_Specs_Triple_Mode.Checked)
                && model.Target.double_Lv < OC_Skip_Special_Process_Last_Lv)
            {
                int Prev_Band_Green = Get_Set2_Band_Gray_Green_Gamma(model.band - 1, model.gray);

                f1.GB_Status_AppendText_Nextline(" Target.double_Lv (< OC_Skip_Special_Process_Last_Lv) = " + model.Target.double_Lv.ToString() + "(<" + OC_Skip_Special_Process_Last_Lv.ToString() + ")", Color.Blue);
                f1.GB_Status_AppendText_Nextline("(Gamma.int_G - Prev_Band_Green) = " + (model.Gamma.int_G - Prev_Band_Green).ToString(), Color.Blue);
                f1.GB_Status_AppendText_Nextline("[Before]Gamma.G" + model.Gamma.int_G.ToString(), Color.Green);
                if ((model.Gamma.int_G - Prev_Band_Green) == 1)
                {
                    model.Gamma.int_G++;
                    Send_Set_Gamma_And_Measure_And_Update_GridView(model.All_band_gray_Gamma, model, Gamma_Set.Set2);
                    f1.GB_Status_AppendText_Nextline("[Process1]Gamma.G" + model.Gamma.int_G.ToString(), Color.Black);
                    if (Get_Is_Skip_Set2_OC(model.band, model.gray, model) == false)
                    {
                        model.Gamma.int_G--;
                        Send_Set_Gamma_And_Measure_And_Update_GridView(model.All_band_gray_Gamma, model, Gamma_Set.Set2);
                        f1.GB_Status_AppendText_Nextline("[Process2]Gamma.G" + model.Gamma.int_G.ToString(), Color.Black);
                    }
                }
                f1.GB_Status_AppendText_Nextline("[After]Gamma.G" + model.Gamma.int_G.ToString(), Color.Red);
            }
            //--------------------------------
        }


        private void Dual_Set2_Band_Green_Offset_1(DP173_or_Elgin model, double OC_Skip_Special_Process_Last_Lv)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //------Added on 200423-----------
            if (model.band >= 1
                && model.Gamma.int_G <= 510
                && checkBox_Set2_OC_Skip_If_XY_and_deltaL_Are_within_Specs.Checked
                && model.Target.double_Lv < OC_Skip_Special_Process_Last_Lv)
            {
                int Prev_Band_Green = Get_Set2_Band_Gray_Green_Gamma(model.band - 1, model.gray);

                f1.GB_Status_AppendText_Nextline(" Target.double_Lv (< OC_Skip_Special_Process_Last_Lv) = " + model.Target.double_Lv.ToString() + "(<" + OC_Skip_Special_Process_Last_Lv.ToString() + ")", Color.Blue);
                f1.GB_Status_AppendText_Nextline("(Gamma.int_G - Prev_Band_Green) = " + (model.Gamma.int_G - Prev_Band_Green).ToString(), Color.Blue);
                f1.GB_Status_AppendText_Nextline("[Before]Gamma.G" + model.Gamma.int_G.ToString(), Color.Green);
                if ((model.Gamma.int_G - Prev_Band_Green) == 1)
                {
                    model.Gamma.int_G++;
                    Send_Set_Gamma_And_Measure_And_Update_GridView(model.All_band_gray_Gamma, model, Gamma_Set.Set2);
                    f1.GB_Status_AppendText_Nextline("[Process1]Gamma.G" + model.Gamma.int_G.ToString(), Color.Black);
                    if (Get_Is_Skip_Set2_OC(model.band, model.gray, model) == false)
                    {
                        model.Gamma.int_G--;
                        Send_Set_Gamma_And_Measure_And_Update_GridView(model.All_band_gray_Gamma, model, Gamma_Set.Set2);
                        f1.GB_Status_AppendText_Nextline("[Process2]Gamma.G" + model.Gamma.int_G.ToString(), Color.Black);
                    }
                }
                f1.GB_Status_AppendText_Nextline("[After]Gamma.G" + model.Gamma.int_G.ToString(), Color.Red);
            }
            //--------------------------------
        }



        private void Dual_Mode_Optic_Compensation()
        {
            Dual_Or_Triple_Mode_Initialize();

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_or_Elgin model = new DP173_or_Elgin(OC_Single_Dual_Triple.Dual);
            Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal = new Initial_RGBVreg1_Calculation_EA9154();

            //Update All_band_gray_Gamma
            DP173_Dual_Get_All_Band_Gray_Gamma(Gamma_Set.Set1, model.All_band_gray_Gamma); //Get All_band_gray_Gamma[14,8]

            //Dual ELVSS Compensation
            Dual_ELVSS_VREF1_Compensation(model, model.All_band_gray_Gamma);

            // Black Compensation
            Black_Compensation(model);
            button_Vreg1_Read.PerformClick();//Update vreg1 and REF_voltage(global variables)
            button_Read_AM0_VREF2_HBM_Set1_Only.PerformClick();//Added On 200219 (VREF1/VREF2047/AM0 Read)

            bool[] Set3_Skip_Band = new bool[11];//default bool value is false
            bool[] Set4_Skip_Band = new bool[11];//default bool value is false
            bool[] Set6_Skip_Band = new bool[11];//default bool value is false

            int Last_Band = Convert.ToInt32(numericUpDown2.Value);
            for (int b = 0; b <= Last_Band; b++) initial_RGBVreg1_cal.Selected_Band[b] = true;
            if (Last_Band < 10) for (int b = (Last_Band + 1); b <= 10; b++) initial_RGBVreg1_cal.Selected_Band[b] = false;

            int Set346_OC_Skip_Last_Band = Convert.ToInt32(numericUpDown_Dual_Set346_OC_Skip_Last_Band.Value);
            for (int b = 0; b <= Set346_OC_Skip_Last_Band; b++)
            {
                Set3_Skip_Band[b] = true;
                Set4_Skip_Band[b] = true;
                Set6_Skip_Band[b] = true;
            }

            if (Set346_OC_Skip_Last_Band < 10)
            {
                for (int b = (Set346_OC_Skip_Last_Band + 1); b <= 10; b++)
                {
                    Set3_Skip_Band[b] = false;
                    Set4_Skip_Band[b] = false;
                    Set6_Skip_Band[b] = false;
                }
            }

            if (Optic_Compensation_Stop) return;

            bool AM1_OC = radioButton_HBM_AM1_OC.Checked;
            bool AM1_OC_Finished = false;

            for (model.band = 0; model.band < 14 && Optic_Compensation_Stop == false; model.band++)
            {
                if (Is_AM1_HBM_OC(model.band, AM1_OC, AM1_OC_Finished))
                {
                    Optic_Compensation_Stop = Set1_HBM_AM1_Compensation(ref model.R_AM1_Hex, ref model.G_AM1_Hex, ref model.B_AM1_Hex, OC_Single_Dual_Triple.Dual, Gamma_Set.Set1);
                    model.Update_AM1_Dec_From_AM1_Hex();
                    AM1_OC_Finished = true;
                    model.band = -1;
                    continue;
                }

                f1.GB_Status_AppendText_Nextline("Band" + (model.band).ToString(), Color.Green);

                if (Optic_Compensation_Stop) break;

                model.Gamma_Out_Of_Register_Limit = false;

                if (Band_BSQH_Selection(ref model.band)) //If this band is not selected , move on to the next band
                {
                    model.Vreg1_loop_count = 0; //Add on 190820
                    model.Vreg1_Infinite_Count = 0; //Add on 190820

                    DP173_form_Dual_engineer.Band_Radiobuttion_Select(model.band, Gamma_Set.Set1);
                    DP173_form_Dual_engineer.Band_Radiobuttion_Select(model.band, Gamma_Set.Set2);

                    if (model.Is_AOD_Band()) //AOD0,1,2
                    {
                        DP173_Pattern_Setting(Gamma_Set.Set1, 0, model.band, OC_Single_Dual_Triple.Dual);
                        Thread.Sleep(300);
                        f1.AOD_On();
                        DP173_DBV_Setting(model.band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                    }
                    DP173_DBV_Setting(model.band);

                    for (model.gray = 0; model.gray < 8 && Optic_Compensation_Stop == false; model.gray++)
                    {
                        bool Current_Gray_GB_Skip = false;

                        DP173_Pattern_Setting(Gamma_Set.Set1, model.gray, model.band, OC_Single_Dual_Triple.Dual);
                        Thread.Sleep(300);

                        DP173_DBV_Setting(model.band);  //DBV Settin
                        button_Gamma_Set1_Apply.PerformClick();//Set1 Apply

                        //Set1
                        {
                            Dual_Triple_Calculate_Init_RGBVreg1_And_Apply_And_Measure(model, Set3_Skip_Band, Gamma_Set.Set1,initial_RGBVreg1_cal); //Added on 200521

                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set1, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                            Thread.Sleep(20);

                            DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count
                                    , model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                            Application.DoEvents();

                            model.loop_count = 0;
                            model.Infinite_Count = 0;

                            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set1); //Add on 190614
                            if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set23456Target(model.band, model.gray);
                            
                            Optic_Compensation_Succeed = false;
                            model.Within_Spec_Limit = false;

                            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                            {
                                if (model.Target.double_Lv < model.Skip_Lv)
                                {
                                    Current_Gray_GB_Skip = true;

                                    if (model.band >= 1)
                                    {
                                        DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Gamma_Set.Set1);
                                        model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                    }
                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set1, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

                                    initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set1);
                                   
                                    model.Measure.Set_Value(0, 0, 0);
                                    DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set1); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.GB_Status_AppendText_Nextline("B" + model.band.ToString() + "/G" + model.gray.ToString()
                                        + " Compensation Skip (Target Lv : " + model.Target.double_Lv.ToString() + ") < " + model.Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                    Optic_Compensation_Succeed = true;
                                    break;
                                }


                                if ((model.Is_Normal_Band()) && model.gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                {
                                    Vreg1_Infinite_Loop_Check(model.Vreg1_loop_count, model);

                                    if (model.Vreg1_loop_count < model.loop_count_max)
                                    {
                                        f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + model.Vreg1_loop_count.ToString(), Color.Blue);
                                        model.Prev_Vreg1 = model.Vreg1;
                                        model.Prev_Gamma.Equal_Value(model.Gamma);

                                        model.Vreg1_Compensation();

                                        f1.Showing_Diff_and_Current_Vreg1_and_Gamma(model.Prev_Gamma, model.Gamma, model.Prev_Vreg1, model.Vreg1);

                                        if (Math.Abs(model.Vreg1 - model.Prev_Vreg1) >= 1) model.Vreg1_Need_To_Be_Updated = true;
                                        else model.Vreg1_Need_To_Be_Updated = false;

                                        if (model.Vreg1_Need_To_Be_Updated)
                                        {
                                            DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Gamma_Set.Set1);
                                        }
                                    }
                                    model.Vreg1_loop_count++;
                                    model.loop_count++;

                                    if (model.Vreg1_Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";

                                }
                                else
                                {
                                    model.Vreg1_Need_To_Be_Updated = false;//Add on 190603

                                    model.Prev_Gamma.Equal_Value(model.Gamma);
                                    Infinite_Loop_Check(model.loop_count, model);

                                    model.Sub_Compensation();

                                    f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);
                                    model.loop_count++;

                                    if (model.Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }
                                if (model.Vreg1_Need_To_Be_Updated == false)
                                {
                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set1, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                    Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                }

                                if (model.Within_Spec_Limit)
                                {
                                    initial_RGBVreg1_cal.Update_Calculated_Vdata(model,Gamma_Set.Set1);//Added on 200521

                                    if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set23456Target(model.band, model.gray);//Add on 

                                    Optic_Compensation_Succeed = true;
                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    break;
                                }

                                if (model.Gamma_Out_Of_Register_Limit)
                                {
                                    if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                    {
                                        Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set1); //Added on 200519
                                        if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set23456Target(model.band, model.gray);
                                    }
                                    else
                                        Optic_Compensation_Succeed = false;

                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");

                                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;

                                    break;
                                }

                                textBox_loop_count.Text = (model.loop_count).ToString();

                                if (model.loop_count == model.loop_count_max)
                                {
                                    if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                    {
                                        Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set1); //Added on 200519
                                        if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set23456Target(model.band, model.gray);
                                    }
                                    else
                                        Optic_Compensation_Succeed = false;

                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    System.Windows.Forms.MessageBox.Show("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Loop Count Over");

                                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                    break;
                                }

                                DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Gamma_Set.Set1); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) DP173_form_Dual_engineer.Dual_Copy_Set1_Measure_To_Set23456Target(model.band, model.gray);
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                Application.DoEvents();
                            }

                            f1.GB_ProgressBar_PerformStep();
                            ///////////////////////////Condition 1 Over
                        }

                        if (!(AM1_OC == true && AM1_OC_Finished == false))
                        {
                            if (model.Is_HBM_Band() || model.Is_Normal_Band())
                            {
                                ///////////////////////////Set 2 Start
                                button_Gamma_Set2_Apply.PerformClick();//Set2 Apply
                                model.Vreg1_loop_count = 0; //Add on 191031
                                model.Vreg1_Infinite_Count = 0; //Add on 191031

                                //Set2
                                {
                                    double OC_Skip_Special_Process_Start_Lv = Convert.ToDouble(textBox_OC_Skip_Special_Process_Start_Lv.Text);
                                    double OC_Skip_Special_Process_Last_Lv = Convert.ToDouble(textBox_OC_Skip_Special_Process_Last_Lv.Text);
                                    if (checkBox_Dual_Mode_Gamma_Copy_Set1_to_Set2.Checked)
                                    {
                                        DP173_form_Dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(model.band, Gamma_Set.Set2);
                                        DP173_Dual_Mode_Get_Param(model.gray, Gamma_Set.Set2, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table   
                                        if (checkBox_Set2_OC_Skip_If_XY_and_deltaL_Are_within_Specs.Checked && model.Target.double_Lv <= OC_Skip_Special_Process_Start_Lv && model.Target.double_Lv >= OC_Skip_Special_Process_Last_Lv)
                                        {
                                            int Green_Offset = Convert.ToInt32(textBox_OC_Skip_Special_Process_G_Offset.Text);
                                            f1.GB_Status_AppendText_Nextline("OC_Skip_Special_Process_Start_Lv >= Target.double_Lv >= OC_Skip_Special_Process_Last_Lv : " + OC_Skip_Special_Process_Start_Lv.ToString() + ">=" + model.Target.double_Lv.ToString() + ">=" + OC_Skip_Special_Process_Last_Lv.ToString(), Color.Green);
                                            f1.GB_Status_AppendText_Nextline("Green_Offset " + Green_Offset.ToString() + " has been applied", Color.Green);

                                            DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set1_to_Set2(model.band, model.gray, Green_Offset); //Copy Gamma (Set1 to Set2)
                                        }
                                        else
                                        {
                                            DP173_form_Dual_engineer.Dual_Mode_Gamma_Copy_Set1_to_Set2(model.band, model.gray); //Copy Gamma (Set1 to Set2)
                                        }
                                    }

                                    DP173_form_Dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(model.band, Gamma_Set.Set2);
                                    DP173_Dual_Get_All_Band_Gray_Gamma(Gamma_Set.Set2, model.All_band_gray_Gamma); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet

                                    if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                    {
                                        if (checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked) model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set1);
                                        else model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Gamma_Set.Set2); //Read Vreg1 Value (Gamma_Set 2's Vreg1 Value = Gamma_Set 1's Vreg1 Value);
                                        DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Gamma_Set.Set2); //Add on 190702 (Condition 1 꺼를 읽은거기 때문에 먼저 Condition2 꺼에 초기 Vreg1 세팅 필요)
                                        model.Initial_Vreg1 = model.Vreg1; //Add on 190603
                                        model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1; //Add on 190603
                                    }


                                    if (Optic_Compensation_Stop) break;
                                    DP173_Dual_Mode_Get_Param(model.gray, Gamma_Set.Set2, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table   
                                    f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

                                    if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && model.Vreg1_loop_count == 0
                                        && checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false)
                                    {
                                        DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, 0, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Gamma_Set.Set2);
                                        model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                    }

                                    Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set2, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                    Thread.Sleep(20);
                                    DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count
                                        , model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                    model.loop_count = 0;
                                    model.Infinite_Count = 0;
                                    DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set2); //Add on 190614
                                    f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                    Application.DoEvents();
                                    Optic_Compensation_Succeed = false;
                                    model.Within_Spec_Limit = false;

                                    ///-----------Added On 203020-----------
                                    bool Skip_Set2_OC = Is_Dual_Mode_Set2_OC_Skip(model, OC_Skip_Special_Process_Last_Lv);
                                    if (Skip_Set2_OC)
                                    {
                                        Dual_Set2_Band_Green_Offset_1(model, OC_Skip_Special_Process_Last_Lv);
                                        DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                        DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Gamma_Set.Set2); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                        Application.DoEvents();

                                        initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set2);
                                    }
                                    
                                    while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false && Skip_Set2_OC == false)
                                    {
                                        //if (Target.double_Lv < Skip_Lv)//Deleted on 200221
                                        if (Current_Gray_GB_Skip) //Added on 200221
                                        {
                                            if (model.band >= 1)
                                            {
                                                DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B, Gamma_Set.Set2);
                                                model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                            }
                                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set2, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

                                            initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set2);
                                           
                                            model.Measure.Set_Value(0, 0, 0);
                                            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X", Gamma_Set.Set2); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                            f1.GB_Status_AppendText_Nextline("B" + model.band.ToString() + "/G" + model.gray.ToString()
                                                + " Compensation Skip (Target Lv : " + model.Target.double_Lv.ToString() + ") < " + model.Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                            Optic_Compensation_Succeed = true;
                                            break;
                                        }
                                        //Vreg1 + Sub-Compensation (Change Gamma Value)
                                        if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked
                                                 && (checkBox_Dual_Set23456_Vreg1_Copy_from_Set1_and_Vreg1_Comp_Skip.Checked == false))
                                        {
                                            Vreg1_Infinite_Loop_Check(model.Vreg1_loop_count, model);

                                            if (model.Vreg1_loop_count < model.loop_count_max)
                                            {
                                                f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + model.Vreg1_loop_count.ToString(), Color.Blue);
                                                model.Prev_Vreg1 = model.Vreg1;
                                                model.Prev_Gamma.Equal_Value(model.Gamma);

                                                model.Vreg1_Compensation();

                                                f1.Showing_Diff_and_Current_Vreg1_and_Gamma(model.Prev_Gamma, model.Gamma, model.Prev_Vreg1, model.Vreg1);

                                                if (Math.Abs(model.Vreg1 - model.Prev_Vreg1) >= 1) model.Vreg1_Need_To_Be_Updated = true;
                                                else model.Vreg1_Need_To_Be_Updated = false;


                                                if (model.Vreg1_Need_To_Be_Updated)
                                                {
                                                    DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Gamma_Set.Set2);
                                                }
                                            }
                                            model.Vreg1_loop_count++;
                                            model.loop_count++;
                                            if (model.Vreg1_Infinite_Count >= 3)
                                            {
                                                Extension_Applied = "O";
                                                f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                            }
                                            else
                                                Extension_Applied = "X";
                                        }
                                        else
                                        {
                                            model.Vreg1_Need_To_Be_Updated = false;

                                            model.Prev_Gamma.Equal_Value(model.Gamma);
                                            Infinite_Loop_Check(model.loop_count, model);

                                            model.Sub_Compensation();

                                            //Engineering Mode
                                            f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);
                                            model.loop_count++;

                                            if (model.Infinite_Count >= 3)
                                            {
                                                Extension_Applied = "O";
                                                f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                            }
                                            else
                                                Extension_Applied = "X";
                                        }
                                        if (model.Vreg1_Need_To_Be_Updated == false)
                                        {
                                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Gamma_Set.Set2, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                        }

                                        if (model.Within_Spec_Limit)
                                        {
                                            initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Gamma_Set.Set2);

                                            //------Added on 200423-----------
                                            Dual_Set2_Band_Green_Offset_1(model, OC_Skip_Special_Process_Last_Lv);
                                            //--------------------------------

                                            Optic_Compensation_Succeed = true;
                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            break;
                                        }

                                        if (model.Gamma_Out_Of_Register_Limit)
                                        {
                                            if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                                Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set2); //Added on 200519
                                            else
                                                Optic_Compensation_Succeed = false;

                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");

                                            if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                            break;
                                        }

                                        textBox_loop_count.Text = (model.loop_count).ToString();

                                        if (model.loop_count == model.loop_count_max)
                                        {
                                            if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                                Optic_Compensation_Succeed = Apply_Dual_Triple_Mode_Upper_Band_Gray(model, Gamma_Set.Set2); //Added on 200519
                                            else
                                                Optic_Compensation_Succeed = false;

                                            textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                            System.Windows.Forms.MessageBox.Show("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Loop Count Over");

                                            if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                            break;
                                        }

                                        DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                                        DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Gamma_Set.Set2); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                                        Application.DoEvents();
                                    }
                                    f1.GB_ProgressBar_PerformStep();
                                }//Set2 End
                            }//Set2 End too
                        }
                        if (checkBox_Only_255G.Checked) model.gray = 8;
                    }//Gray Loop End
                    if (model.Is_AOD_Band()) f1.AOD_Off();
                }
            }//Band Loop End

            //---Set3----
            if (checkBox_Dual_Mode_Gamma_Copy_Set1_to_Set3.Checked && Optic_Compensation_Stop == false) Copy_All_Band_Gamma_From_Set1_To_Set3();
            if (checkBox_Dual_Mode_Set3_OC_Skip.Checked == false && Optic_Compensation_Stop == false) Set3_Optic_Compensation(Set3_Skip_Band, model.oc_mode, initial_RGBVreg1_cal);//Single Set3 Optic Compensation

            //---Set4----
            if (checkBox_Dual_Mode_Gamma_Copy_Set2_to_Set4.Checked && Optic_Compensation_Stop == false) Copy_All_Band_Gamma_From_Set2_To_Set4();
            if (checkBox_Dual_Mode_Set4_OC_Skip.Checked == false && Optic_Compensation_Stop == false) Set4_Optic_Compensation(Set4_Skip_Band, model.oc_mode, initial_RGBVreg1_cal);//Single Set4 Optic Compensation

            //---Set5----
            if (checkBox_Dual_Mode_Set5_OC_Skip.Checked == false && Optic_Compensation_Stop == false) Dual_Set5_Optic_Compensation(Set6_Skip_Band);//Single Set6 Optic Compensation

            //---Set6----
            if (checkBox_Dual_Mode_Gamma_Copy_Set5_to_Set6.Checked && Optic_Compensation_Stop == false) Copy_All_Band_Gamma_From_Set5_To_Set6();
            if (checkBox_Dual_Mode_Set6_OC_Skip.Checked == false && Optic_Compensation_Stop == false) Set6_Optic_Compensation(Set6_Skip_Band, model.oc_mode, initial_RGBVreg1_cal);//Single Set6 Optic Compensation


            f1.OC_Timer_Stop();
            DP173_DBV_Setting(1);  //DBV Setting    

            f1.PTN_update(255, 255, 255);

            System.Windows.Forms.MessageBox.Show("Optic Compensation Finished !");

            //--After Dual Compensation Finished--
            DP173_form_Dual_engineer.Dual_RadioButton_All_Enable(true);
            DP173_form_Dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(false);

            /*
            initial_RGBVreg1_cal.Show_Calculated_Vdata();
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set1);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set2);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set3);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set4);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set5);
            initial_RGBVreg1_cal.Show_Calculated_Vdata(Gamma_Set.Set6);
             */
        }


        private bool Apply_Dual_Triple_Mode_Upper_Band_Gray(DP173_or_Elgin model, Gamma_Set Set)
        {
            bool Optic_Compensation_Succeed;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Apply_Dual_Triple_Mode_Upper_Band_Gray Applied", Color.Blue);
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];

            double Max_Target_Lv = Convert.ToDouble(textBox_OC_Fail_Prevension_Target_Max_Lv.Text);
            if (model.Target.double_Lv < Max_Target_Lv)
            {
                DP173_form_Dual_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Gamma.int_R, ref model.Gamma.int_G, ref model.Gamma.int_B, Set);
                Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                Thread.Sleep(20);
                DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Set); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                Optic_Compensation_Succeed = true;
            }
            else
            {
                Optic_Compensation_Succeed = false;
            }
            return Optic_Compensation_Succeed;
        }












        private void Send_Set_Gamma_And_Measure_And_Update_GridView(RGB[,] All_band_gray_Gamma, DP173_or_Elgin model,Gamma_Set Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
            DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied, Set); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
            Application.DoEvents();
        }



        private bool Get_Is_Skip_Set2_OC(int band, int gray, DP173_or_Elgin model)
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            bool Skip_Set2_OC = false;
            bool X_Spec_In = false;
            bool Y_Spec_In = false;
            bool Delta_L_Spec_In = false;
            bool Delta_UV_Spec_In = false;

            XYLv Set1_Measured = DP173_form_Dual_engineer.Get_Set_Measured_Values(Gamma_Set.Set1, band, gray);
            XYLv Set2_Measured = DP173_form_Dual_engineer.Get_Set_Measured_Values(Gamma_Set.Set2, band, gray);
            f1.GB_Status_AppendText_Nextline("Set1 Measured B" + band.ToString() + "/" + gray.ToString() + " X/Y/LV : " + Set1_Measured.X + "/" + Set1_Measured.Y + "/" + Set1_Measured.Lv, Color.Blue);
            f1.GB_Status_AppendText_Nextline("Set2 Measured B" + band.ToString() + "/" + gray.ToString() + " X/Y/LV : " + Set2_Measured.X + "/" + Set2_Measured.Y + "/" + Set2_Measured.Lv, Color.Blue);
            double Diff_Delta_L_Spec = Set2_Diff_Delta_L_Spec[band, gray];
            double UV_Distance_Limit = Set2_Diff_Delta_UV_Spec[band, gray];

            if (radioButton_OC_Skip_XY_Dual_Mode.Checked)
            {
                X_Spec_In = Compare_X(Set1_Measured, Set2_Measured, model);
                Y_Spec_In = Compare_Y(Set1_Measured, Set2_Measured, model);
            }
            else if (radioButton_OC_Skip_UV_Dual_Mode.Checked)
            {
                Delta_UV_Spec_In = Compare_Delta_UV(Set1_Measured, Set2_Measured, UV_Distance_Limit);
            }
            Delta_L_Spec_In = Compare_Delta_L(Set1_Measured, Set2_Measured, Diff_Delta_L_Spec);

            if (X_Spec_In && Y_Spec_In && Delta_L_Spec_In)
            {
                Skip_Set2_OC = true;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_OC(XY) = true", Color.Green);
            }
            else if (Delta_UV_Spec_In && Delta_L_Spec_In)
            {
                Skip_Set2_OC = true;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_OC(UV) = true", Color.Green);
            }
            else
            {
                Skip_Set2_OC = false;
                f1.GB_Status_AppendText_Nextline("Skip_Set2_OC = false", Color.Red);
            }
            return Skip_Set2_OC;
        }


        private int Get_Set2_Band_Gray_Green_Gamma(int band, int gray)
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            return Convert.ToInt32(DP173_form_Dual_engineer.dataGridView_OC_param_Set2.Rows[(band * 8) + (gray + 2)].Cells[2].Value);
        }

        private int Get_Set3_Band_Gray_Green_Gamma(int band, int gray)
        {
            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            return Convert.ToInt32(DP173_form_Dual_engineer.dataGridView_OC_param_Set3.Rows[(band * 8) + (gray + 2)].Cells[2].Value);
        }




        //Single
        private void DP173_Update_Engineering_Sheet(RGB Gamma, XYLv Measure, int band, int gray, int loop_count, string Extension_Applied)
        {
            DP173_Single_Engineering_Mornitoring DP173_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            DP173_form_engineer.Set_OC_Param_DP173(gray, Gamma.int_R, Gamma.int_G, Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, loop_count, Extension_Applied);
            DP173_form_engineer.Updata_Sub_To_Main_GridView(band, gray);
        }
        private void DP173_Single_Mode_Set1_HBM_Gamma_Update(RGB[] Gamma)
        {
            //Conditon = true : Condition 1
            //Conditon = false : Condition 2
            DP173_Single_Engineering_Mornitoring DP173_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            DP173_form_engineer.Set1_HBM_Update_Gamma(Gamma);
            for (int gray = 0; gray < 8; gray++) DP173_form_engineer.Updata_Sub_To_Main_GridView(0, gray);
        }

        //Dual & Triple
        private void DP173_Dual_Mode_Update_Engineering_Sheet(RGB Gamma, XYLv Measure, int band, int gray, int loop_count, string Extension_Applied, Gamma_Set Set)
        {
            //Conditon = true : Condition 1
            //Conditon = false : Condition 2
            DP173_Dual_Engineering_Mornitoring Dual_Mode_form_Engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            Dual_Mode_form_Engineer.Set_OC_Param_DP173(gray, Gamma.int_R, Gamma.int_G, Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, loop_count, Extension_Applied, Set);
            Dual_Mode_form_Engineer.Updata_Sub_To_Main_GridView(band, gray, Set);
        }
        private void DP173_Dual_Mode_Set1_HBM_Gamma_Update(RGB[] Gamma)
        {
            //Conditon = true : Condition 1
            //Conditon = false : Condition 2
            DP173_Dual_Engineering_Mornitoring Dual_Mode_form_Engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            Dual_Mode_form_Engineer.Set1_HBM_Update_Gamma(Gamma);
            for (int gray = 0; gray < 8; gray++) Dual_Mode_form_Engineer.Updata_Sub_To_Main_GridView(0, gray, Gamma_Set.Set1);
        }
        private void DP173_Dual_Mode_Set2_HBM_Gamma_Update(RGB[] Gamma)
        {
            //Conditon = true : Condition 1
            //Conditon = false : Condition 2
            DP173_Dual_Engineering_Mornitoring Dual_Mode_form_Engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            Dual_Mode_form_Engineer.Set2_HBM_Update_Gamma(Gamma);
            for (int gray = 0; gray < 8; gray++) Dual_Mode_form_Engineer.Updata_Sub_To_Main_GridView(0, gray, Gamma_Set.Set2);
        }

        private void ProgressBar_Max_Step_Setting(int step, bool AOD_Skip = false)
        {
            int ProgressBar_max = 0;

            //if (checkBox_VREF2_AM0_Compensation.Checked) ProgressBar_max += step;

            //How many BSQH Points are checked ? (graypoints = 10)
            if (checkBox_Band0.Checked) ProgressBar_max += (8 * step);
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

            if (AOD_Skip == false)
            {
                if (checkBox_AOD0.Checked) ProgressBar_max += (8 * step);
                if (checkBox_AOD1.Checked) ProgressBar_max += (8 * step);
                if (checkBox_AOD2.Checked) ProgressBar_max += (8 * step);
            }

            //OTP Auto Write checked 
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Set_GB_ProgressBar_Maximum(ProgressBar_max);
            f1.Set_GB_ProgressBar_Step(step);

        }

        private void DP173_Get_All_Band_Gray_Gamma(RGB[,] All_band_gray_Gamma)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Get All Band/Gray Gamma from OC_Param", Color.Blue);

            DP173_Single_Engineering_Mornitoring DP173_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            DP173_form_engineer.DP173_Get_All_Band_Gray_Gamma(All_band_gray_Gamma);
        }

        private void DP173_Get_Param(int gray, ref RGB Gamma, ref XYLv Target, ref XYLv Limit, ref XYLv Extension)
        {
            DP173_Single_Engineering_Mornitoring Single_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            Single_form_engineer.Get_OC_Param_DP173(gray, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B
                , ref Target.double_X, ref Target.double_Y, ref Target.double_Lv, ref Limit.double_X, ref Limit.double_Y, ref Limit.double_Lv, ref Extension.double_X, ref Extension.double_Y);
        }

        private RGB DP173_Get_Gamma(int gray)
        {
            DP173_Single_Engineering_Mornitoring Single_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            return Single_form_engineer.DP173_Get_Gamma(gray);
        }

        public void DP173_Pattern_Setting(Gamma_Set Set, int gray, int band, OC_Single_Dual_Triple mode)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Gray = 255;

            //--Get Gray--
            if (checkBox_Special_Gray_Compensation.Checked)
            {
                string Band_Gray = string.Empty;
                if (mode == OC_Single_Dual_Triple.Single) //Single
                {
                    DP173_Single_Engineering_Mornitoring DP173_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
                    Band_Gray = DP173_form_engineer.Get_BX_GXXX_By_Gray_DP173(gray);
                }
                else if (mode == OC_Single_Dual_Triple.Dual) //Dual
                {
                    DP173_Dual_Engineering_Mornitoring DP173_form_dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
                    Band_Gray = DP173_form_dual_engineer.Dual_Get_BX_GXXX_By_Gray_DP173(gray, Set);
                }

                if (band == 10) Gray = Convert.ToInt32(Band_Gray.Remove(0, 5));//B10_G255 --> 255
                else Gray = Convert.ToInt32(Band_Gray.Remove(0, 4)); //ex) B0_G255 --> 255 , A2_G91 --> 91 , A0_G4 --> 4

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

            //--Display Gray Pattern--
            if (band == 11 || band == 12 || band == 13) //AOD Mode Pattern
            {
                if (radioButton_AOD_PTN_10per.Checked)
                {
                    f1.Image_Crosstalk(f1.current_model.get_AOD_X(), f1.current_model.get_AOD_Y(), 0, 0, 0, Gray, Gray, Gray);
                    f1.GB_Status_AppendText_Nextline("AOD Gray" + Gray.ToString() + " Setting", System.Drawing.Color.Black);
                }
                else if (radioButton_AOD_PTN_100per.Checked)
                {
                    f1.PTN_update(Gray, Gray, Gray);
                    f1.GB_Status_AppendText_Nextline("(AOD PTN 100% Mode has been selected)Gray" + Gray.ToString() + " Setting", System.Drawing.Color.Black);
                }
            }
            else //Normal Mode Pattern
            {
                f1.PTN_update(Gray, Gray, Gray);
                f1.GB_Status_AppendText_Nextline("Gray" + Gray.ToString() + " Setting", System.Drawing.Color.Black);
            }
        }


        public void Update_and_Send_All_Band_Gray_Gamma_Change_G1_Only(RGB[,] All_band_gray_Gamma, RGB Current_Gamma, int Current_Band, Gamma_Set Set
            , string R_AM1, string R_AM0, string G_AM1, string G_AM0, string B_AM1, string B_AM0)
        {

            if (Current_Band >= 11)//AOD
            {
                R_AM1 = "00";
                R_AM0 = "00";

                G_AM1 = "00";
                G_AM0 = "00";

                B_AM1 = "00";
                B_AM0 = "00";
            }


            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            RGB[] Gamma_9th_data = new RGB[9]; //Data[8] (+G1)
            RGB[] Gamma_8ea_data = new RGB[9]; //Data[7:0]< G255 (+G1)

            string[] Hex_Param = new string[40];

            //G255 ~ G4
            for (int gray = 0; gray < 8; gray++)
            {
                Gamma_9th_data[gray].Set_Value(All_band_gray_Gamma[Current_Band, gray].int_R >> 8, All_band_gray_Gamma[Current_Band, gray].int_G >> 8, All_band_gray_Gamma[Current_Band, gray].int_B >> 8);
                Gamma_8ea_data[gray].Set_Value(All_band_gray_Gamma[Current_Band, gray].int_R & 0xFF, All_band_gray_Gamma[Current_Band, gray].int_G & 0xFF, All_band_gray_Gamma[Current_Band, gray].int_B & 0xFF);
            }

            //G1 (Setting As Current Gamma)
            Gamma_9th_data[8].Set_Value(Current_Gamma.int_R >> 8, Current_Gamma.int_G >> 8, Current_Gamma.int_B >> 8);
            Gamma_8ea_data[8].Set_Value(Current_Gamma.int_R & 0xFF, Current_Gamma.int_G & 0xFF, Current_Gamma.int_B & 0xFF);

            Hex_Param[0] = ((Gamma_9th_data[0].int_R << 4) + (Gamma_9th_data[0].int_G << 2) + (Gamma_9th_data[0].int_B)).ToString("X2");//G255 RGB 9th bit

            Hex_Param[1] = ((Gamma_9th_data[1].int_R << 7) + (Gamma_9th_data[2].int_R << 6)
                + (Gamma_9th_data[3].int_R << 5) + (Gamma_9th_data[4].int_R << 4)
                + (Gamma_9th_data[5].int_R << 3) + (Gamma_9th_data[6].int_R << 2)
                + (Gamma_9th_data[7].int_R << 1) + Gamma_9th_data[8].int_R).ToString("X2");//GXX < G255 R 9th
            Hex_Param[2] = "00";
            Hex_Param[3] = Gamma_8ea_data[0].int_R.ToString("X2"); //G255[7:0]
            Hex_Param[4] = R_AM1;
            Hex_Param[5] = R_AM0;
            Hex_Param[6] = Gamma_8ea_data[1].int_R.ToString("X2"); //G191[7:0]
            Hex_Param[7] = Gamma_8ea_data[2].int_R.ToString("X2"); //G127[7:0]
            Hex_Param[8] = Gamma_8ea_data[3].int_R.ToString("X2"); //G63[7:0]
            Hex_Param[9] = Gamma_8ea_data[4].int_R.ToString("X2"); //G31[7:0]
            Hex_Param[10] = Gamma_8ea_data[5].int_R.ToString("X2"); //G15[7:0]
            Hex_Param[11] = Gamma_8ea_data[6].int_R.ToString("X2"); //G7[7:0]
            Hex_Param[12] = Gamma_8ea_data[7].int_R.ToString("X2");//G4[7:0]
            Hex_Param[13] = Gamma_8ea_data[8].int_R.ToString("X2");//G1[7:0]


            Hex_Param[14] = ((Gamma_9th_data[1].int_G << 7) + (Gamma_9th_data[2].int_G << 6)
                + (Gamma_9th_data[3].int_G << 5) + (Gamma_9th_data[4].int_G << 4)
                + (Gamma_9th_data[5].int_G << 3) + (Gamma_9th_data[6].int_G << 2)
                + (Gamma_9th_data[7].int_G << 1) + Gamma_9th_data[8].int_G).ToString("X2"); //GXX < G255 G 9th
            Hex_Param[15] = "00";
            Hex_Param[16] = Gamma_8ea_data[0].int_G.ToString("X2"); //G255[7:0]
            Hex_Param[17] = G_AM1;
            Hex_Param[18] = G_AM0;
            Hex_Param[19] = Gamma_8ea_data[1].int_G.ToString("X2"); //G191[7:0]
            Hex_Param[20] = Gamma_8ea_data[2].int_G.ToString("X2"); //G127[7:0]
            Hex_Param[21] = Gamma_8ea_data[3].int_G.ToString("X2"); //G63[7:0]
            Hex_Param[22] = Gamma_8ea_data[4].int_G.ToString("X2"); //G31[7:0]
            Hex_Param[23] = Gamma_8ea_data[5].int_G.ToString("X2"); //G15[7:0]
            Hex_Param[24] = Gamma_8ea_data[6].int_G.ToString("X2"); //G7[7:0]
            Hex_Param[25] = Gamma_8ea_data[7].int_G.ToString("X2");//G4[7:0]
            Hex_Param[26] = Gamma_8ea_data[8].int_G.ToString("X2");//G1[7:0]


            Hex_Param[27] = ((Gamma_9th_data[1].int_B << 7) + (Gamma_9th_data[2].int_B << 6)
                + (Gamma_9th_data[3].int_B << 5) + (Gamma_9th_data[4].int_B << 4)
                + (Gamma_9th_data[5].int_B << 3) + (Gamma_9th_data[6].int_B << 2)
                + (Gamma_9th_data[7].int_B << 1) + Gamma_9th_data[8].int_B).ToString("X2"); //GXX < G255 B 9th
            Hex_Param[28] = "00";
            Hex_Param[29] = Gamma_8ea_data[0].int_B.ToString("X2"); //G255[7:0]
            Hex_Param[30] = B_AM1;
            Hex_Param[31] = B_AM0;
            Hex_Param[32] = Gamma_8ea_data[1].int_B.ToString("X2"); //G191[7:0]
            Hex_Param[33] = Gamma_8ea_data[2].int_B.ToString("X2"); //G127[7:0]
            Hex_Param[34] = Gamma_8ea_data[3].int_B.ToString("X2"); //G63[7:0]
            Hex_Param[35] = Gamma_8ea_data[4].int_B.ToString("X2"); //G31[7:0]
            Hex_Param[36] = Gamma_8ea_data[5].int_B.ToString("X2"); //G15[7:0]
            Hex_Param[37] = Gamma_8ea_data[6].int_B.ToString("X2"); //G7[7:0]
            Hex_Param[38] = Gamma_8ea_data[7].int_B.ToString("X2");//G4[7:0]
            Hex_Param[39] = Gamma_8ea_data[8].int_B.ToString("X2");//G1[7:0]

            int Offset = 0;
            if (Set == Gamma_Set.Set1 && Current_Band < 11) Offset = 0;//Set1 Normal
            else if (Set == Gamma_Set.Set2 && Current_Band < 11) Offset = 40;//Set2 Normal
            else if (Set == Gamma_Set.Set3 && Current_Band < 11) Offset = 80;//Set3 Normal
            else if (Set == Gamma_Set.Set4 && Current_Band < 11) Offset = 120;//Set4 Normal
            else if (Set == Gamma_Set.Set5 && Current_Band < 11) Offset = 160;//Set5 Normal
            else if (Set == Gamma_Set.Set6 && Current_Band < 11) Offset = 200;//Set6 Normal
            else if (Current_Band == 11) Offset = 10;//AOD0
            else if (Current_Band == 12) Offset = 50;//AOD1
            else if (Current_Band == 13) Offset = 90;//AOD2
            else
            {
                System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                Optic_Compensation_Stop = true;
                return;
            }

            string Band_Register = DP173.Get_Gamma_Register_Hex_String(Current_Band).Remove(0, 2);
            f1.DP173_Long_Packet_CMD_Send(Offset, Hex_Param.Length, Band_Register, Hex_Param);
        }



        private void Update_and_Send_All_Band_Gray_Gamma_GR1_Disalbe(RGB[,] All_band_gray_Gamma, RGB Current_Gamma, int Current_Band, int Current_Gray, Gamma_Set Set
            , string R_AM1, string R_AM0, string G_AM1, string G_AM0, string B_AM1, string B_AM0)
        {
            if (Current_Band >= 11)//AOD
            {
                R_AM1 = "00";
                R_AM0 = "00";

                G_AM1 = "00";
                G_AM0 = "00";

                B_AM1 = "00";
                B_AM0 = "00";
            }

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Update Gamma table as current Gamma
            All_band_gray_Gamma[Current_Band, Current_Gray].Equal_Value(Current_Gamma);

            RGB[] Gamma_9th_data = new RGB[8]; //Data[8]
            RGB[] Gamma_8ea_data = new RGB[8]; //Data[7:0]

            string[] Hex_Param = new string[40];

            //G255 ~ G4
            for (int gray = 0; gray < 8; gray++)
            {
                Gamma_9th_data[gray].Set_Value(All_band_gray_Gamma[Current_Band, gray].int_R >> 8, All_band_gray_Gamma[Current_Band, gray].int_G >> 8, All_band_gray_Gamma[Current_Band, gray].int_B >> 8);
                Gamma_8ea_data[gray].Set_Value(All_band_gray_Gamma[Current_Band, gray].int_R & 0xFF, All_band_gray_Gamma[Current_Band, gray].int_G & 0xFF, All_band_gray_Gamma[Current_Band, gray].int_B & 0xFF);
            }

            Hex_Param[0] = ((Gamma_9th_data[0].int_R << 4) + (Gamma_9th_data[0].int_G << 2) + (Gamma_9th_data[0].int_B)).ToString("X2");//G255 RGB 9th bit

            Hex_Param[1] = ((Gamma_9th_data[1].int_R << 7) + (Gamma_9th_data[2].int_R << 6)
                + (Gamma_9th_data[3].int_R << 5) + (Gamma_9th_data[4].int_R << 4)
                + (Gamma_9th_data[5].int_R << 3) + (Gamma_9th_data[6].int_R << 2)
                + 2 + Gamma_9th_data[7].int_R).ToString("X2");//GXX < G255 R 9th
            Hex_Param[2] = "00";
            Hex_Param[3] = Gamma_8ea_data[0].int_R.ToString("X2"); //G255[7:0]
            Hex_Param[4] = R_AM1;
            Hex_Param[5] = R_AM0;
            Hex_Param[6] = Gamma_8ea_data[1].int_R.ToString("X2"); //G191[7:0]
            Hex_Param[7] = Gamma_8ea_data[2].int_R.ToString("X2"); //G127[7:0]
            Hex_Param[8] = Gamma_8ea_data[3].int_R.ToString("X2"); //G63[7:0]
            Hex_Param[9] = Gamma_8ea_data[4].int_R.ToString("X2"); //G31[7:0]
            Hex_Param[10] = Gamma_8ea_data[5].int_R.ToString("X2"); //G15[7:0]
            Hex_Param[11] = Gamma_8ea_data[6].int_R.ToString("X2"); //G7[7:0]
            Hex_Param[12] = "90";
            Hex_Param[13] = Gamma_8ea_data[7].int_R.ToString("X2");//G1[7:0]


            Hex_Param[14] = ((Gamma_9th_data[1].int_G << 7) + (Gamma_9th_data[2].int_G << 6)
                + (Gamma_9th_data[3].int_G << 5) + (Gamma_9th_data[4].int_G << 4)
                + (Gamma_9th_data[5].int_G << 3) + (Gamma_9th_data[6].int_G << 2)
                + 2 + Gamma_9th_data[7].int_G).ToString("X2"); //GXX < G255 G 9th
            Hex_Param[15] = "00";
            Hex_Param[16] = Gamma_8ea_data[0].int_G.ToString("X2"); //G255[7:0]
            Hex_Param[17] = G_AM1;
            Hex_Param[18] = G_AM0;
            Hex_Param[19] = Gamma_8ea_data[1].int_G.ToString("X2"); //G191[7:0]
            Hex_Param[20] = Gamma_8ea_data[2].int_G.ToString("X2"); //G127[7:0]
            Hex_Param[21] = Gamma_8ea_data[3].int_G.ToString("X2"); //G63[7:0]
            Hex_Param[22] = Gamma_8ea_data[4].int_G.ToString("X2"); //G31[7:0]
            Hex_Param[23] = Gamma_8ea_data[5].int_G.ToString("X2"); //G15[7:0]
            Hex_Param[24] = Gamma_8ea_data[6].int_G.ToString("X2"); //G7[7:0]
            Hex_Param[25] = "90";
            Hex_Param[26] = Gamma_8ea_data[7].int_G.ToString("X2");//G1[7:0]


            Hex_Param[27] = ((Gamma_9th_data[1].int_B << 7) + (Gamma_9th_data[2].int_B << 6)
                + (Gamma_9th_data[3].int_B << 5) + (Gamma_9th_data[4].int_B << 4)
                + (Gamma_9th_data[5].int_B << 3) + (Gamma_9th_data[6].int_B << 2)
                + 2 + Gamma_9th_data[7].int_B).ToString("X2"); //GXX < G255 B 9th
            Hex_Param[28] = "00";
            Hex_Param[29] = Gamma_8ea_data[0].int_B.ToString("X2"); //G255[7:0]
            Hex_Param[30] = B_AM1;
            Hex_Param[31] = B_AM0;
            Hex_Param[32] = Gamma_8ea_data[1].int_B.ToString("X2"); //G191[7:0]
            Hex_Param[33] = Gamma_8ea_data[2].int_B.ToString("X2"); //G127[7:0]
            Hex_Param[34] = Gamma_8ea_data[3].int_B.ToString("X2"); //G63[7:0]
            Hex_Param[35] = Gamma_8ea_data[4].int_B.ToString("X2"); //G31[7:0]
            Hex_Param[36] = Gamma_8ea_data[5].int_B.ToString("X2"); //G15[7:0]
            Hex_Param[37] = Gamma_8ea_data[6].int_B.ToString("X2"); //G7[7:0]
            Hex_Param[38] = "90";//G4[7:0]
            Hex_Param[39] = Gamma_8ea_data[7].int_B.ToString("X2");//G1[7:0]


            int Offset = 0;
            if (Set == Gamma_Set.Set1 && Current_Band < 11) Offset = 0;//Set1 Normal
            else if (Set == Gamma_Set.Set2 && Current_Band < 11) Offset = 40;//Set2 Normal
            else if (Set == Gamma_Set.Set3 && Current_Band < 11) Offset = 80;//Set3 Normal
            else if (Set == Gamma_Set.Set4 && Current_Band < 11) Offset = 120;//Set4 Normal
            else if (Set == Gamma_Set.Set5 && Current_Band < 11) Offset = 160;//Set5 Normal
            else if (Set == Gamma_Set.Set6 && Current_Band < 11) Offset = 200;//Set6 Normal
            else if (Current_Band == 11) Offset = 10;//AOD0
            else if (Current_Band == 12) Offset = 50;//AOD1
            else if (Current_Band == 13) Offset = 90;//AOD2
            else
            {
                System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                Optic_Compensation_Stop = true;
                return;
            }
            string Band_Register = DP173.Get_Gamma_Register_Hex_String(Current_Band).Remove(0, 2);
            f1.DP173_Long_Packet_CMD_Send(Offset, Hex_Param.Length, Band_Register, Hex_Param);
        }

        private void Update_and_Send_All_Band_Gray_Gamma_GR1_Disalbe_GR1_Calculation(RGB[,] All_band_gray_Gamma, RGB Current_Gamma, int Current_Band, int Current_Gray, Gamma_Set Set
            , string R_AM1, string R_AM0, string G_AM1, string G_AM0, string B_AM1, string B_AM0)
        {
            if (Current_Band >= 11)//AOD
            {
                R_AM1 = "00";
                R_AM0 = "00";

                G_AM1 = "00";
                G_AM0 = "00";

                B_AM1 = "00";
                B_AM0 = "00";
            }


            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            RGB_Double[] Calculated_Vdata = new RGB_Double[8]; //for Dual
            int R_AM1_Dec = Convert.ToInt32(R_AM1, 16);
            int G_AM1_Dec = Convert.ToInt32(G_AM1, 16);
            int B_AM1_Dec = Convert.ToInt32(B_AM1, 16);


            int Dec_Vreg1 = DP173_Get_Normal_Initial_Vreg1(Current_Band, Set);//Current_Band Vreg1
            double Calculated_Vreg1_Voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Dec_Vreg1, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
            for (int gray = 0; gray < 8; gray++)
            {
                if (gray == 0)
                {
                    Calculated_Vdata[gray].double_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, All_band_gray_Gamma[Current_Band, gray].int_R);
                    Calculated_Vdata[gray].double_G = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, All_band_gray_Gamma[Current_Band, gray].int_G);
                    Calculated_Vdata[gray].double_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, All_band_gray_Gamma[Current_Band, gray].int_B);
                }
                else
                {
                    Calculated_Vdata[gray].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, R_AM1_Dec, Calculated_Vdata[(gray - 1)].double_R, All_band_gray_Gamma[Current_Band, gray].int_R, gray);
                    Calculated_Vdata[gray].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, G_AM1_Dec, Calculated_Vdata[(gray - 1)].double_G, All_band_gray_Gamma[Current_Band, gray].int_G, gray);
                    Calculated_Vdata[gray].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, B_AM1_Dec, Calculated_Vdata[(gray - 1)].double_B, All_band_gray_Gamma[Current_Band, gray].int_B, gray);
                }
                Calculated_Vdata[gray].Update_String_From_Double();
            }

            RGB_Double Calculated_GR1_Vdata = new RGB_Double();

            Calculated_GR1_Vdata.double_R = (Calculated_Vdata[6].double_R + Calculated_Vdata[7].double_R) / 2.0;
            Calculated_GR1_Vdata.double_G = (Calculated_Vdata[6].double_G + Calculated_Vdata[7].double_G) / 2.0;
            Calculated_GR1_Vdata.double_B = (Calculated_Vdata[6].double_B + Calculated_Vdata[7].double_B) / 2.0;
            Calculated_GR1_Vdata.Update_String_From_Double();

            RGB GR1_Gamma = new RGB();
            GR1_Gamma.int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, R_AM1_Dec, Calculated_Vdata[6].double_R, Calculated_GR1_Vdata.double_R, 7);
            GR1_Gamma.int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, G_AM1_Dec, Calculated_Vdata[6].double_G, Calculated_GR1_Vdata.double_G, 7);
            GR1_Gamma.int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Calculated_Vreg1_Voltage, B_AM1_Dec, Calculated_Vdata[6].double_B, Calculated_GR1_Vdata.double_B, 7);
            GR1_Gamma.String_Update_From_int();

            //Update Gamma table as current Gamma
            All_band_gray_Gamma[Current_Band, Current_Gray].Equal_Value(Current_Gamma);

            RGB[] Gamma_9th_data = new RGB[8]; //Data[8]
            RGB[] Gamma_8ea_data = new RGB[8]; //Data[7:0]
            RGB Gamma_GR1_9th_data = new RGB();
            RGB Gamma_GR1_8ea_data = new RGB();

            string[] Hex_Param = new string[40];

            //G255 ~ G4
            for (int gray = 0; gray < 8; gray++)
            {
                Gamma_9th_data[gray].Set_Value(All_band_gray_Gamma[Current_Band, gray].int_R >> 8, All_band_gray_Gamma[Current_Band, gray].int_G >> 8, All_band_gray_Gamma[Current_Band, gray].int_B >> 8);
                Gamma_8ea_data[gray].Set_Value(All_band_gray_Gamma[Current_Band, gray].int_R & 0xFF, All_band_gray_Gamma[Current_Band, gray].int_G & 0xFF, All_band_gray_Gamma[Current_Band, gray].int_B & 0xFF);
            }
            Gamma_GR1_9th_data.Set_Value(GR1_Gamma.int_R >> 8, GR1_Gamma.int_G >> 8, GR1_Gamma.int_B >> 8);
            Gamma_GR1_8ea_data.Set_Value(GR1_Gamma.int_R & 0xFF, GR1_Gamma.int_G & 0xFF, GR1_Gamma.int_B & 0xFF);

            Hex_Param[0] = ((Gamma_9th_data[0].int_R << 4) + (Gamma_9th_data[0].int_G << 2) + (Gamma_9th_data[0].int_B)).ToString("X2");//G255 RGB 9th bit

            Hex_Param[1] = ((Gamma_9th_data[1].int_R << 7) + (Gamma_9th_data[2].int_R << 6)
                + (Gamma_9th_data[3].int_R << 5) + (Gamma_9th_data[4].int_R << 4)
                + (Gamma_9th_data[5].int_R << 3) + (Gamma_9th_data[6].int_R << 2)
                + (Gamma_GR1_9th_data.int_R << 1) + Gamma_9th_data[7].int_R).ToString("X2");//GXX < G255 R 9th
            Hex_Param[2] = "00";
            Hex_Param[3] = Gamma_8ea_data[0].int_R.ToString("X2"); //G255[7:0]
            Hex_Param[4] = R_AM1;
            Hex_Param[5] = R_AM0;
            Hex_Param[6] = Gamma_8ea_data[1].int_R.ToString("X2"); //G191[7:0]
            Hex_Param[7] = Gamma_8ea_data[2].int_R.ToString("X2"); //G127[7:0]
            Hex_Param[8] = Gamma_8ea_data[3].int_R.ToString("X2"); //G63[7:0]
            Hex_Param[9] = Gamma_8ea_data[4].int_R.ToString("X2"); //G31[7:0]
            Hex_Param[10] = Gamma_8ea_data[5].int_R.ToString("X2"); //G15[7:0]
            Hex_Param[11] = Gamma_8ea_data[6].int_R.ToString("X2"); //G7[7:0]
            Hex_Param[12] = Gamma_GR1_8ea_data.int_R.ToString("X2");
            Hex_Param[13] = Gamma_8ea_data[7].int_R.ToString("X2");//G1[7:0]


            Hex_Param[14] = ((Gamma_9th_data[1].int_G << 7) + (Gamma_9th_data[2].int_G << 6)
                + (Gamma_9th_data[3].int_G << 5) + (Gamma_9th_data[4].int_G << 4)
                + (Gamma_9th_data[5].int_G << 3) + (Gamma_9th_data[6].int_G << 2)
                + (Gamma_GR1_9th_data.int_G << 1) + Gamma_9th_data[7].int_G).ToString("X2"); //GXX < G255 G 9th
            Hex_Param[15] = "00";
            Hex_Param[16] = Gamma_8ea_data[0].int_G.ToString("X2"); //G255[7:0]
            Hex_Param[17] = G_AM1;
            Hex_Param[18] = G_AM0;
            Hex_Param[19] = Gamma_8ea_data[1].int_G.ToString("X2"); //G191[7:0]
            Hex_Param[20] = Gamma_8ea_data[2].int_G.ToString("X2"); //G127[7:0]
            Hex_Param[21] = Gamma_8ea_data[3].int_G.ToString("X2"); //G63[7:0]
            Hex_Param[22] = Gamma_8ea_data[4].int_G.ToString("X2"); //G31[7:0]
            Hex_Param[23] = Gamma_8ea_data[5].int_G.ToString("X2"); //G15[7:0]
            Hex_Param[24] = Gamma_8ea_data[6].int_G.ToString("X2"); //G7[7:0]
            Hex_Param[25] = Gamma_GR1_8ea_data.int_G.ToString("X2");
            Hex_Param[26] = Gamma_8ea_data[7].int_G.ToString("X2");//G1[7:0]


            Hex_Param[27] = ((Gamma_9th_data[1].int_B << 7) + (Gamma_9th_data[2].int_B << 6)
                + (Gamma_9th_data[3].int_B << 5) + (Gamma_9th_data[4].int_B << 4)
                + (Gamma_9th_data[5].int_B << 3) + (Gamma_9th_data[6].int_B << 2)
                + (Gamma_GR1_9th_data.int_B << 1) + Gamma_9th_data[7].int_B).ToString("X2"); //GXX < G255 B 9th
            Hex_Param[28] = "00";
            Hex_Param[29] = Gamma_8ea_data[0].int_B.ToString("X2"); //G255[7:0]
            Hex_Param[30] = B_AM1;
            Hex_Param[31] = B_AM0;
            Hex_Param[32] = Gamma_8ea_data[1].int_B.ToString("X2"); //G191[7:0]
            Hex_Param[33] = Gamma_8ea_data[2].int_B.ToString("X2"); //G127[7:0]
            Hex_Param[34] = Gamma_8ea_data[3].int_B.ToString("X2"); //G63[7:0]
            Hex_Param[35] = Gamma_8ea_data[4].int_B.ToString("X2"); //G31[7:0]
            Hex_Param[36] = Gamma_8ea_data[5].int_B.ToString("X2"); //G15[7:0]
            Hex_Param[37] = Gamma_8ea_data[6].int_B.ToString("X2"); //G7[7:0]
            Hex_Param[38] = Gamma_GR1_8ea_data.int_B.ToString("X2");
            Hex_Param[39] = Gamma_8ea_data[7].int_B.ToString("X2");//G1[7:0]


            int Offset = 0;
            if (Set == Gamma_Set.Set1 && Current_Band < 11) Offset = 0;//Set1 Normal
            else if (Set == Gamma_Set.Set2 && Current_Band < 11) Offset = 40;//Set2 Normal
            else if (Set == Gamma_Set.Set3 && Current_Band < 11) Offset = 80;//Set3 Normal
            else if (Set == Gamma_Set.Set4 && Current_Band < 11) Offset = 120;//Set4 Normal
            else if (Set == Gamma_Set.Set5 && Current_Band < 11) Offset = 160;//Set5 Normal
            else if (Set == Gamma_Set.Set6 && Current_Band < 11) Offset = 200;//Set6 Normal
            else if (Current_Band == 11) Offset = 10;//AOD0
            else if (Current_Band == 12) Offset = 50;//AOD1
            else if (Current_Band == 13) Offset = 90;//AOD2
            else
            {
                System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                Optic_Compensation_Stop = true;
                return;
            }

            string Band_Register = DP173.Get_Gamma_Register_Hex_String(Current_Band).Remove(0, 2);
            f1.DP173_Long_Packet_CMD_Send(Offset, Hex_Param.Length, Band_Register, Hex_Param);
        }

        public void Update_and_Send_All_Band_Gray_Gamma(RGB[,] All_band_gray_Gamma, RGB Current_Gamma, int Current_Band, int Current_Gray, Gamma_Set Set
            , string R_AM1, string R_AM0, string G_AM1, string G_AM0, string B_AM1, string B_AM0)
        {
            if (radioButton_GR1_EN_False.Checked) Update_and_Send_All_Band_Gray_Gamma_GR1_Disalbe(All_band_gray_Gamma, Current_Gamma, Current_Band, Current_Gray, Set, R_AM1, R_AM0, G_AM1, G_AM0, B_AM1, B_AM0);
            else if (radioButton_GR1_EN_False_GR1_Calculation.Checked) Update_and_Send_All_Band_Gray_Gamma_GR1_Disalbe_GR1_Calculation(All_band_gray_Gamma, Current_Gamma, Current_Band, Current_Gray, Set, R_AM1, R_AM0, G_AM1, G_AM0, B_AM1, B_AM0);
        }




        private void Infinite_Loop_Check(int loop_count, DP173_or_Elgin model)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (loop_count == 0) model.Temp_Gamma[0].Equal_Value(model.Gamma);
            else if (loop_count == 1) model.Temp_Gamma[1].Equal_Value(model.Gamma);
            else if (loop_count == 2) model.Temp_Gamma[2].Equal_Value(model.Gamma);
            else if (loop_count == 3) model.Temp_Gamma[3].Equal_Value(model.Gamma);
            else if (loop_count == 4) model.Temp_Gamma[4].Equal_Value(model.Gamma);//Added On 200218
            else if (loop_count == 5) model.Temp_Gamma[5].Equal_Value(model.Gamma);//Added On 200218
            else
            {
                model.Temp.Equal_Value(model.Temp_Gamma[1]);
                model.Temp_Gamma[0].Equal_Value(model.Temp);

                model.Temp.Equal_Value(model.Temp_Gamma[2]);
                model.Temp_Gamma[1].Equal_Value(model.Temp);

                model.Temp.Equal_Value(model.Temp_Gamma[3]);//Added On 200218
                model.Temp_Gamma[2].Equal_Value(model.Temp);//Added On 200218

                model.Temp.Equal_Value(model.Temp_Gamma[4]);//Added On 200218
                model.Temp_Gamma[3].Equal_Value(model.Temp);//Added On 200218

                model.Temp.Equal_Value(model.Temp_Gamma[5]);//Added On 200218
                model.Temp_Gamma[4].Equal_Value(model.Temp);//Added On 200218

                model.Temp_Gamma[5].Equal_Value(model.Gamma);//Added On 200218

                model.Diif_Gamma[0] = model.Temp_Gamma[1] - model.Temp_Gamma[0];
                model.Diif_Gamma[1] = model.Temp_Gamma[2] - model.Temp_Gamma[1];
                model.Diif_Gamma[2] = model.Temp_Gamma[3] - model.Temp_Gamma[2];
                model.Diif_Gamma[3] = model.Temp_Gamma[4] - model.Temp_Gamma[3];
                model.Diif_Gamma[4] = model.Temp_Gamma[5] - model.Temp_Gamma[4];
                //Ver5
                if ((model.Diif_Gamma[2].R == model.Diif_Gamma[4].R)
                    && (model.Diif_Gamma[2].G == model.Diif_Gamma[4].G)
                    && (model.Diif_Gamma[2].B == model.Diif_Gamma[4].B)
                    && ((model.Diif_Gamma[2].R != model.Diif_Gamma[3].R) || (model.Diif_Gamma[2].G != model.Diif_Gamma[3].G) || (model.Diif_Gamma[2].B != model.Diif_Gamma[3].B))
                    && (((model.Diif_Gamma[3].int_R >= 0) && (model.Diif_Gamma[4].int_R < 0))//Added On 200218
                        || ((model.Diif_Gamma[3].int_R < 0) && (model.Diif_Gamma[4].int_R >= 0))//Added On 200218
                        || ((model.Diif_Gamma[3].int_G >= 0) && (model.Diif_Gamma[4].int_G < 0))//Added On 200218
                        || ((model.Diif_Gamma[3].int_G < 0) && (model.Diif_Gamma[4].int_G >= 0))//Added On 200218
                        || ((model.Diif_Gamma[3].int_B >= 0) && (model.Diif_Gamma[4].int_B < 0))//Added On 200218
                        || ((model.Diif_Gamma[3].int_B < 0) && (model.Diif_Gamma[4].int_B >= 0))))//Added On 200218
                {
                    f1.GB_Status_AppendText_Nextline("Infinite Loop Case 1", Color.Purple);
                    model.Infinite = true;
                    model.Infinite_Count++;
                }
                else if (((model.Temp_Gamma[0].R == model.Temp_Gamma[3].R) && (model.Temp_Gamma[0].G == model.Temp_Gamma[3].G) && (model.Temp_Gamma[0].B == model.Temp_Gamma[3].B))
                    && ((model.Temp_Gamma[1].R == model.Temp_Gamma[4].R) && (model.Temp_Gamma[1].G == model.Temp_Gamma[4].G) && (model.Temp_Gamma[1].B == model.Temp_Gamma[4].B))
                    && ((model.Temp_Gamma[2].R == model.Temp_Gamma[5].R) && (model.Temp_Gamma[2].G == model.Temp_Gamma[5].G) && (model.Temp_Gamma[2].B == model.Temp_Gamma[5].B)))
                {
                    f1.GB_Status_AppendText_Nextline("Infinite Loop Case 2", Color.Blue);
                    model.Infinite = true;
                    model.Infinite_Count++;
                }
                else
                {
                    model.Infinite = false;
                }

                if (model.Infinite) f1.GB_Status_AppendText_Nextline("Infinite : " + model.Infinite.ToString(), Color.Red);
                else f1.GB_Status_AppendText_Nextline("Infinite : " + model.Infinite.ToString(), Color.Green);

                if (model.Infinite_Count >= 3)
                    f1.GB_Status_AppendText_Nextline("Infinite_Count = " + model.Infinite_Count.ToString(), Color.Red);
                else
                    f1.GB_Status_AppendText_Nextline("Infinite_Count = " + model.Infinite_Count.ToString(), Color.Green);
            }
        }



        private void DP173_Band_Gray255_Compensation(int band, RGB[,] All_band_gray_Gamma, Gamma_Set Set, DP173_or_Elgin model)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Single_Engineering_Mornitoring DP173_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            int gray = 0;

            DP173_DBV_Setting(band);
            DP173_Pattern_Setting(Set, gray, band, OC_Single_Dual_Triple.Single);
            Thread.Sleep(300); //Pattern 안정화 Time

            DP173_form_engineer.Band_Radiobuttion_Select(band);

            Optic_Compensation_Succeed = false;
            Optic_Compensation_Stop = false;
            int loop_count = 0;
            model.Infinite_Count = 0;
            model.Infinite = false;

            DP173_Get_Param(gray, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); ; //Get (First) Gray255 Gamma,Target,Limit From OC-Param-Table 
            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

            Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, model.Gamma, band, gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

            Thread.Sleep(20);

            DP173_Update_Engineering_Sheet(model.Gamma, model.Measure, band, gray, loop_count, "X");
            f1.GB_Status_AppendText_Nextline("HBM/Gray255 Compensation Start", Color.Green);

            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
            {
                f1.CA_Measure_button_Perform_Click(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv); //Get (First)Measure

                Infinite_Loop_Check(loop_count, model);
                model.Prev_Gamma.Equal_Value(model.Gamma);

                model.Sub_Compensation();

                f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);

                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, model.Gamma, band, gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                Thread.Sleep(20);

                if (model.Infinite_Count >= 3)
                {
                    Extension_Applied = "O";
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                }
                else
                    Extension_Applied = "X";

                DP173_Update_Engineering_Sheet(model.Gamma, model.Measure, band, gray, loop_count, Extension_Applied);

                if (model.Within_Spec_Limit)
                {
                    Optic_Compensation_Succeed = true;
                    break;
                }

                if (model.Gamma_Out_Of_Register_Limit)
                {
                    Optic_Compensation_Stop = true;
                    System.Windows.Forms.MessageBox.Show("Red/Vreg1/Blue is out of Limit");
                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                    break;
                }
                textBox_loop_count.Text = (++loop_count).ToString();

                if (loop_count == 300)
                {
                    Optic_Compensation_Succeed = false;
                    System.Windows.Forms.MessageBox.Show("HBM Gray255" + "Loop Count Over");
                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                    break;
                }
            }
            f1.GB_Status_AppendText_Nextline("HBM / Gray255 Compensation Finish", Color.Green);
        }

        private void DP173_Dual_Band_Gray255_Compensation(int band, RGB[,] All_band_gray_Gamma, Gamma_Set Set, DP173_or_Elgin model)
        {
            int gray = 0;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Dual_Engineering_Mornitoring DP173_form_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            DP173_DBV_Setting(band);//DBV Setting
            DP173_Pattern_Setting(Gamma_Set.Set1, gray, band, OC_Single_Dual_Triple.Dual);//Gray255 Pattern Setting
            Thread.Sleep(300); //Pattern 안정화 Time
            DP173_form_engineer.Band_Radiobuttion_Select(band, Set);//Select Band

            Optic_Compensation_Succeed = false;
            Optic_Compensation_Stop = false;
            model.loop_count = 0;
            model.Infinite_Count = 0;
            model.Infinite = false;

            DP173_Dual_Mode_Get_Param(gray, Set, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension);

            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

            Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, model.Gamma, band, gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

            Thread.Sleep(20);

            DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, band, gray, model.loop_count, "X", Set);
            f1.GB_Status_AppendText_Nextline("HBM/Gray255 Compensation Start", Color.Green);

            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
            {
                f1.CA_Measure_button_Perform_Click(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv); //Get (First)Measure

                Infinite_Loop_Check(model.loop_count, model);
                model.Prev_Gamma.Equal_Value(model.Gamma);

                model.Sub_Compensation();

                f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);

                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, model.Gamma, band, gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                Thread.Sleep(20);

                if (model.Infinite_Count >= 3)
                {
                    Extension_Applied = "O";
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                }
                else
                    Extension_Applied = "X";

                DP173_Dual_Mode_Update_Engineering_Sheet(model.Gamma, model.Measure, band, gray, model.loop_count, Extension_Applied, Set);

                if (model.Within_Spec_Limit)
                {
                    Optic_Compensation_Succeed = true;
                    break;
                }

                if (model.Gamma_Out_Of_Register_Limit)
                {
                    Optic_Compensation_Stop = true;
                    System.Windows.Forms.MessageBox.Show("Red/Vreg1/Blue is out of Limit");
                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                    break;
                }
                textBox_loop_count.Text = (++model.loop_count).ToString();

                if (model.loop_count == 300)
                {
                    Optic_Compensation_Succeed = false;
                    System.Windows.Forms.MessageBox.Show("Gray255" + "Loop Count Over");
                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                    break;
                }
            }
            f1.GB_Status_AppendText_Nextline("Gray255 Compensation Finish", Color.Green);
        }


        public int DP173_Get_Normal_Initial_Vreg1(int band, Gamma_Set Set)
        {
            //AOD0
            if (band == 11)
            {
                return Convert.ToInt32(textBox_Vreg1_A0.Text);
            }
            //AOD1
            else if (band == 12)
            {
                return Convert.ToInt32(textBox_Vreg1_A1.Text);
            }
            //AOD2
            else if (band == 13)
            {
                return Convert.ToInt32(textBox_Vreg1_A2.Text);
            }
            //Set 1(Normal)
            else if (Set == Gamma_Set.Set1)
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt32(textBox_Vreg1_B0_1.Text);
                    case 1:
                        return Convert.ToInt32(textBox_Vreg1_B1_1.Text);
                    case 2:
                        return Convert.ToInt32(textBox_Vreg1_B2_1.Text);
                    case 3:
                        return Convert.ToInt32(textBox_Vreg1_B3_1.Text);
                    case 4:
                        return Convert.ToInt32(textBox_Vreg1_B4_1.Text);
                    case 5:
                        return Convert.ToInt32(textBox_Vreg1_B5_1.Text);
                    case 6:
                        return Convert.ToInt32(textBox_Vreg1_B6_1.Text);
                    case 7:
                        return Convert.ToInt32(textBox_Vreg1_B7_1.Text);
                    case 8:
                        return Convert.ToInt32(textBox_Vreg1_B8_1.Text);
                    case 9:
                        return Convert.ToInt32(textBox_Vreg1_B9_1.Text);
                    case 10:
                        return Convert.ToInt32(textBox_Vreg1_B10_1.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            //Set 2(Normal)
            else if (Set == Gamma_Set.Set2)
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt32(textBox_Vreg1_B0_2.Text);
                    case 1:
                        return Convert.ToInt32(textBox_Vreg1_B1_2.Text);
                    case 2:
                        return Convert.ToInt32(textBox_Vreg1_B2_2.Text);
                    case 3:
                        return Convert.ToInt32(textBox_Vreg1_B3_2.Text);
                    case 4:
                        return Convert.ToInt32(textBox_Vreg1_B4_2.Text);
                    case 5:
                        return Convert.ToInt32(textBox_Vreg1_B5_2.Text);
                    case 6:
                        return Convert.ToInt32(textBox_Vreg1_B6_2.Text);
                    case 7:
                        return Convert.ToInt32(textBox_Vreg1_B7_2.Text);
                    case 8:
                        return Convert.ToInt32(textBox_Vreg1_B8_2.Text);
                    case 9:
                        return Convert.ToInt32(textBox_Vreg1_B9_2.Text);
                    case 10:
                        return Convert.ToInt32(textBox_Vreg1_B10_2.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            //Set 3(Normal)
            else if (Set == Gamma_Set.Set3)
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt32(textBox_Vreg1_B0_3.Text);
                    case 1:
                        return Convert.ToInt32(textBox_Vreg1_B1_3.Text);
                    case 2:
                        return Convert.ToInt32(textBox_Vreg1_B2_3.Text);
                    case 3:
                        return Convert.ToInt32(textBox_Vreg1_B3_3.Text);
                    case 4:
                        return Convert.ToInt32(textBox_Vreg1_B4_3.Text);
                    case 5:
                        return Convert.ToInt32(textBox_Vreg1_B5_3.Text);
                    case 6:
                        return Convert.ToInt32(textBox_Vreg1_B6_3.Text);
                    case 7:
                        return Convert.ToInt32(textBox_Vreg1_B7_3.Text);
                    case 8:
                        return Convert.ToInt32(textBox_Vreg1_B8_3.Text);
                    case 9:
                        return Convert.ToInt32(textBox_Vreg1_B9_3.Text);
                    case 10:
                        return Convert.ToInt32(textBox_Vreg1_B10_3.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            //Set 4(Normal)
            else if (Set == Gamma_Set.Set4)
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt32(textBox_Vreg1_B0_4.Text);
                    case 1:
                        return Convert.ToInt32(textBox_Vreg1_B1_4.Text);
                    case 2:
                        return Convert.ToInt32(textBox_Vreg1_B2_4.Text);
                    case 3:
                        return Convert.ToInt32(textBox_Vreg1_B3_4.Text);
                    case 4:
                        return Convert.ToInt32(textBox_Vreg1_B4_4.Text);
                    case 5:
                        return Convert.ToInt32(textBox_Vreg1_B5_4.Text);
                    case 6:
                        return Convert.ToInt32(textBox_Vreg1_B6_4.Text);
                    case 7:
                        return Convert.ToInt32(textBox_Vreg1_B7_4.Text);
                    case 8:
                        return Convert.ToInt32(textBox_Vreg1_B8_4.Text);
                    case 9:
                        return Convert.ToInt32(textBox_Vreg1_B9_4.Text);
                    case 10:
                        return Convert.ToInt32(textBox_Vreg1_B10_4.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            //Set 5(Normal)
            else if (Set == Gamma_Set.Set5)
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt32(textBox_Vreg1_B0_5.Text);
                    case 1:
                        return Convert.ToInt32(textBox_Vreg1_B1_5.Text);
                    case 2:
                        return Convert.ToInt32(textBox_Vreg1_B2_5.Text);
                    case 3:
                        return Convert.ToInt32(textBox_Vreg1_B3_5.Text);
                    case 4:
                        return Convert.ToInt32(textBox_Vreg1_B4_5.Text);
                    case 5:
                        return Convert.ToInt32(textBox_Vreg1_B5_5.Text);
                    case 6:
                        return Convert.ToInt32(textBox_Vreg1_B6_5.Text);
                    case 7:
                        return Convert.ToInt32(textBox_Vreg1_B7_5.Text);
                    case 8:
                        return Convert.ToInt32(textBox_Vreg1_B8_5.Text);
                    case 9:
                        return Convert.ToInt32(textBox_Vreg1_B9_5.Text);
                    case 10:
                        return Convert.ToInt32(textBox_Vreg1_B10_5.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            //Set 6(Normal)
            else if (Set == Gamma_Set.Set6)
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt32(textBox_Vreg1_B0_6.Text);
                    case 1:
                        return Convert.ToInt32(textBox_Vreg1_B1_6.Text);
                    case 2:
                        return Convert.ToInt32(textBox_Vreg1_B2_6.Text);
                    case 3:
                        return Convert.ToInt32(textBox_Vreg1_B3_6.Text);
                    case 4:
                        return Convert.ToInt32(textBox_Vreg1_B4_6.Text);
                    case 5:
                        return Convert.ToInt32(textBox_Vreg1_B5_6.Text);
                    case 6:
                        return Convert.ToInt32(textBox_Vreg1_B6_6.Text);
                    case 7:
                        return Convert.ToInt32(textBox_Vreg1_B7_6.Text);
                    case 8:
                        return Convert.ToInt32(textBox_Vreg1_B8_6.Text);
                    case 9:
                        return Convert.ToInt32(textBox_Vreg1_B9_6.Text);
                    case 10:
                        return Convert.ToInt32(textBox_Vreg1_B10_6.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary (band>13 or band<0)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3 or 4");
                Optic_Compensation_Stop = true;
                return 0;
            }
        }

        private void Vreg1_Infinite_Loop_Check(int Vreg1_loop_count, DP173_or_Elgin model)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (Vreg1_loop_count == 0) model.Vreg1_Temp_Gamma[0].Equal_Value(model.Gamma);
            else if (Vreg1_loop_count == 1)
            {
                model.Vreg1_Value[0] = model.Vreg1;
                model.Vreg1_Temp_Gamma[1].Equal_Value(model.Gamma);
            }
            else if (Vreg1_loop_count == 2)
            {
                model.Vreg1_Value[1] = model.Vreg1;
                model.Vreg1_Temp_Gamma[2].Equal_Value(model.Gamma);
            }
            else if (Vreg1_loop_count == 3)
            {
                model.Vreg1_Value[2] = model.Vreg1;
                model.Vreg1_Temp_Gamma[3].Equal_Value(model.Gamma);
            }
            else
            {

                model.Vreg1_Value_Temp = model.Vreg1_Value[1];
                model.Vreg1_Value[0] = model.Vreg1_Value_Temp;
                model.Vreg1_Value_Temp = model.Vreg1_Value[2];
                model.Vreg1_Value[1] = model.Vreg1_Value_Temp;
                model.Vreg1_Value[2] = model.Vreg1;

                model.Vreg1_Temp.Equal_Value(model.Vreg1_Temp_Gamma[1]);
                model.Vreg1_Temp_Gamma[0].Equal_Value(model.Vreg1_Temp);
                model.Vreg1_Temp.Equal_Value(model.Vreg1_Temp_Gamma[2]);
                model.Vreg1_Temp_Gamma[1].Equal_Value(model.Vreg1_Temp);
                model.Vreg1_Temp.Equal_Value(model.Vreg1_Temp_Gamma[3]);
                model.Vreg1_Temp_Gamma[2].Equal_Value(model.Vreg1_Temp);
                model.Vreg1_Temp_Gamma[3].Equal_Value(model.Gamma);

                model.Vreg1_Diif_Gamma[0] = model.Vreg1_Temp_Gamma[1] - model.Vreg1_Temp_Gamma[0];
                model.Vreg1_Diif_Gamma[1] = model.Vreg1_Temp_Gamma[2] - model.Vreg1_Temp_Gamma[1];
                model.Vreg1_Diif_Gamma[2] = model.Vreg1_Temp_Gamma[3] - model.Vreg1_Temp_Gamma[2];

                //Ver5
                if ((model.Vreg1_Value[2] == model.Vreg1_Value[1] && model.Vreg1_Value[1] == model.Vreg1_Value[0]) &&
                    ((model.Vreg1_Diif_Gamma[0].R == model.Vreg1_Diif_Gamma[2].R && model.Vreg1_Diif_Gamma[0].B == model.Vreg1_Diif_Gamma[2].B) && (model.Vreg1_Diif_Gamma[0].R != model.Vreg1_Diif_Gamma[1].R || model.Vreg1_Diif_Gamma[0].B != model.Vreg1_Diif_Gamma[1].B)))
                {
                    model.Vreg1_Infinite = true;
                    model.Vreg1_Infinite_Count++;
                }

                else model.Vreg1_Infinite = false;

                if (model.Vreg1_Infinite) f1.GB_Status_AppendText_Nextline("Vreg1_Infinite : " + model.Vreg1_Infinite.ToString(), Color.Red);
                else f1.GB_Status_AppendText_Nextline("Vreg1_Infinite : " + model.Vreg1_Infinite.ToString(), Color.Green);

                if (model.Vreg1_Infinite_Count >= 3)
                    f1.GB_Status_AppendText_Nextline("Vreg1_Infinite_Count = " + model.Vreg1_Infinite_Count.ToString(), Color.Red);
                else
                    f1.GB_Status_AppendText_Nextline("Vreg1_Infinite_Count = " + model.Vreg1_Infinite_Count.ToString(), Color.Green);
            }
        }


        private void DP173_Update_and_Send_Vreg1_and_Textbox_Update(DP173_or_Elgin model, Gamma_Set Set)
        {
            DP173_Update_and_Send_Vreg1(model, Set);
            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
            DP173_Update_Vreg1_TextBox(model, Set);
        }


        private void DP173_Update_and_Send_Vreg1(DP173_or_Elgin model, Gamma_Set Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string Total = model.Vreg1.ToString("X3");

            string[] B1_Vreg1_Gamma;
            int Offset = 0;
            if (Set == Gamma_Set.Set1)
            {
                Offset = 43;
                B1_Vreg1_Gamma = B1_Vreg1_Gamma_Set1;
            }
            else if (Set == Gamma_Set.Set2)
            {
                Offset = 60;
                B1_Vreg1_Gamma = B1_Vreg1_Gamma_Set2;
            }
            else if (Set == Gamma_Set.Set3)
            {
                Offset = 77;
                B1_Vreg1_Gamma = B1_Vreg1_Gamma_Set3;
            }
            else if (Set == Gamma_Set.Set4)
            {
                Offset = 94;
                B1_Vreg1_Gamma = B1_Vreg1_Gamma_Set4;
            }
            else if (Set == Gamma_Set.Set5)
            {
                Offset = 111;
                B1_Vreg1_Gamma = B1_Vreg1_Gamma_Set5;
            }
            else if (Set == Gamma_Set.Set6)
            {
                Offset = 128;
                B1_Vreg1_Gamma = B1_Vreg1_Gamma_Set6;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
                Optic_Compensation_Stop = true;
                return;
            }

            if (model.band % 2 == 0) B1_Vreg1_Gamma[model.band / 2] = Total[0].ToString() + (Convert.ToInt32(B1_Vreg1_Gamma[model.band / 2], 16) & 0x0F).ToString("X");
            else B1_Vreg1_Gamma[model.band / 2] = ((Convert.ToInt32(B1_Vreg1_Gamma[model.band / 2], 16) & 0xF0) >> 4).ToString("X") + Total[0].ToString();

            B1_Vreg1_Gamma[6 + model.band] = Total[1].ToString() + Total[2].ToString();
            f1.DP173_Long_Packet_CMD_Send(Offset, 17, "B1", B1_Vreg1_Gamma);
            f1.GB_Status_AppendText_Nextline("Vreg1 Is Applied", Color.Black, true);
        }


        private void DP173_Update_Vreg1_TextBox(DP173_or_Elgin model, Gamma_Set Set)
        {
            if (Set == Gamma_Set.Set1)
            {
                if (model.band == 0)
                {
                    textBox_Vreg1_B0_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B0_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B0_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 1)
                {
                    textBox_Vreg1_B1_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B1_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B1_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 2)
                {
                    textBox_Vreg1_B2_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B2_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B2_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 3)
                {
                    textBox_Vreg1_B3_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B3_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B3_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 4)
                {
                    textBox_Vreg1_B4_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B4_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B4_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 5)
                {
                    textBox_Vreg1_B5_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B5_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B5_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 6)
                {
                    textBox_Vreg1_B6_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B6_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B6_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 7)
                {
                    textBox_Vreg1_B7_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B7_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B7_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 8)
                {
                    textBox_Vreg1_B8_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B8_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B8_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 9)
                {
                    textBox_Vreg1_B9_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B9_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B9_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 10)
                {
                    textBox_Vreg1_B10_1.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B10_1_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B10_1.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else
                {
                    //Do nothing 
                }
            }
            else if (Set == Gamma_Set.Set2)
            {
                if (model.band == 0)
                {
                    textBox_Vreg1_B0_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B0_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B0_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 1)
                {
                    textBox_Vreg1_B1_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B1_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B1_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 2)
                {
                    textBox_Vreg1_B2_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B2_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B2_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 3)
                {
                    textBox_Vreg1_B3_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B3_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B3_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 4)
                {
                    textBox_Vreg1_B4_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B4_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B4_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 5)
                {
                    textBox_Vreg1_B5_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B5_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B5_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 6)
                {
                    textBox_Vreg1_B6_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B6_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B6_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 7)
                {
                    textBox_Vreg1_B7_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B7_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B7_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 8)
                {
                    textBox_Vreg1_B8_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B8_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B8_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 9)
                {
                    textBox_Vreg1_B9_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B9_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B9_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 10)
                {
                    textBox_Vreg1_B10_2.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B10_2_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B10_2.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else
                {
                    //Do nothing 
                }
            }
            else if (Set == Gamma_Set.Set3)
            {
                if (model.band == 0)
                {
                    textBox_Vreg1_B0_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B0_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B0_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 1)
                {
                    textBox_Vreg1_B1_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B1_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B1_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 2)
                {
                    textBox_Vreg1_B2_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B2_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B2_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 3)
                {
                    textBox_Vreg1_B3_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B3_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B3_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 4)
                {
                    textBox_Vreg1_B4_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B4_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B4_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 5)
                {
                    textBox_Vreg1_B5_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B5_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B5_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 6)
                {
                    textBox_Vreg1_B6_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B6_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B6_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 7)
                {
                    textBox_Vreg1_B7_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B7_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B7_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 8)
                {
                    textBox_Vreg1_B8_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B8_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B8_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 9)
                {
                    textBox_Vreg1_B9_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B9_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B9_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 10)
                {
                    textBox_Vreg1_B10_3.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B10_3_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B10_3.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
            }
            else if (Set == Gamma_Set.Set4)
            {
                if (model.band == 0)
                {
                    textBox_Vreg1_B0_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B0_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B0_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 1)
                {
                    textBox_Vreg1_B1_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B1_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B1_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 2)
                {
                    textBox_Vreg1_B2_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B2_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B2_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 3)
                {
                    textBox_Vreg1_B3_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B3_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B3_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 4)
                {
                    textBox_Vreg1_B4_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B4_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B4_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 5)
                {
                    textBox_Vreg1_B5_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B5_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B5_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 6)
                {
                    textBox_Vreg1_B6_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B6_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B6_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 7)
                {
                    textBox_Vreg1_B7_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B7_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B7_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 8)
                {
                    textBox_Vreg1_B8_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B8_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B8_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 9)
                {
                    textBox_Vreg1_B9_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B9_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B9_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
                else if (model.band == 10)
                {
                    textBox_Vreg1_B10_4.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                    textBox_Vreg1_B10_4_volt.Text = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Convert.ToInt32(textBox_Vreg1_B10_4.Text), Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1).ToString();
                }
            }
            else if (Set == Gamma_Set.Set5)
            {
                if (model.band == 0)
                {
                    textBox_Vreg1_B0_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 1)
                {
                    textBox_Vreg1_B1_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 2)
                {
                    textBox_Vreg1_B2_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 3)
                {
                    textBox_Vreg1_B3_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 4)
                {
                    textBox_Vreg1_B4_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 5)
                {
                    textBox_Vreg1_B5_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 6)
                {
                    textBox_Vreg1_B6_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 7)
                {
                    textBox_Vreg1_B7_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 8)
                {
                    textBox_Vreg1_B8_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 9)
                {
                    textBox_Vreg1_B9_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 10)
                {
                    textBox_Vreg1_B10_5.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
            }


            else if (Set == Gamma_Set.Set6)
            {
                if (model.band == 0)
                {
                    textBox_Vreg1_B0_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 1)
                {
                    textBox_Vreg1_B1_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 2)
                {
                    textBox_Vreg1_B2_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 3)
                {
                    textBox_Vreg1_B3_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 4)
                {
                    textBox_Vreg1_B4_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 5)
                {
                    textBox_Vreg1_B5_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 6)
                {
                    textBox_Vreg1_B6_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 7)
                {
                    textBox_Vreg1_B7_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 8)
                {
                    textBox_Vreg1_B8_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 9)
                {
                    textBox_Vreg1_B9_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
                else if (model.band == 10)
                {
                    textBox_Vreg1_B10_6.Text = model.Vreg1.ToString(); //Read Vreg1 Value
                }
            }
            else
            {
                //Do nothing 
            }
            Application.DoEvents();
        }

        private void SET_hex_ELVSS(int Band, double ELVSS, string[] hex_ELVSS)
        {
            int Current_Dec = Convert.ToInt32(30 + (10 * (ELVSS + 3.1)));
            hex_ELVSS[Band] = Current_Dec.ToString("X2");
        }

        private void SET_hex_VINIT2(int Band, double VINIT2, string[] hex_VINIT2, int VINIT2_SEL)
        {
            int Current_Dec = 0;
            if (VINIT2_SEL == 1) Current_Dec = Convert.ToInt32(2 + (VINIT2 * 10));
            else Current_Dec = Convert.ToInt32(2 - (VINIT2 * 10));
            hex_VINIT2[Band] = Current_Dec.ToString("X2");
        }


        private void DP173_VREF1_Compensation(double Set1_HBM_RGB_Min_White)
        {
            double VREF1_Margin = Convert.ToDouble(textBox_REF1_Margin.Text);
            double New_VREF1_Voltage = (Set1_HBM_RGB_Min_White - VREF1_Margin);

            if (New_VREF1_Voltage > 5.25) System.Windows.Forms.MessageBox.Show("VREF1 Upper Overflow NG");
            else if (New_VREF1_Voltage < 0.25) System.Windows.Forms.MessageBox.Show("VREF1 Lower Overflow NG");

            int Dec_VREG1_REF1 = Convert.ToInt32((New_VREF1_Voltage - 0.2) / 0.04);
            string Hex_VREG1_REF1 = (Dec_VREG1_REF1 & 0x7F).ToString("X2");
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP173_One_Param_CMD_Send(27, "B1", Hex_VREG1_REF1);
        }

        private void DP173_ELVSS_Compensation(int band, double First_ELVSS, double Last_ELVSS)
        {
            int delay = Convert.ToInt32(textBox_ELVSS_CMD_Delay.Text);

            if (Optic_Compensation_Stop)
            {

            }
            else
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                Gamma_Set Set = Gamma_Set.Set1;
                bool ELVSS_Find_Finish = false;
                double ELVSS = 0; //Input & Output (ref)
                double First_Slope = 0; //Input & Output (ref)
                double ELVSS_Margin = Convert.ToDouble(textBox_ELVSS_Margin.Text);
                double Slope_Margin = Convert.ToDouble(textBox_Slope_Margin.Text);
                XYLv First_Measure = new XYLv();
                XYLv Measure = new XYLv();

                string[] hex_ELVSS = new string[11]; for (int i = 0; i < 11; i++) hex_ELVSS[i] = "00";

                //Get ELVSS(double)
                for (ELVSS = First_ELVSS; ELVSS < Last_ELVSS; ELVSS += 0.1)
                {
                    if (ELVSS == First_ELVSS)
                    {
                        SET_hex_ELVSS(band, ELVSS, hex_ELVSS);
                        Send_ELVSS_Setting(Set, hex_ELVSS);
                        Update_ELVSS_Textbox(Set, band, ELVSS, hex_ELVSS[band]);
                        Thread.Sleep(delay); f1.GB_Status_AppendText_Nextline("Applied Delay(ms):" + delay.ToString(), Color.Blue);
                        f1.CA_Measure_For_ELVSS(ELVSS.ToString(), ref First_Measure.double_X, ref First_Measure.double_Y, ref First_Measure.double_Lv);
                    }
                    else
                    {
                        SET_hex_ELVSS(band, ELVSS, hex_ELVSS);
                        Send_ELVSS_Setting(Set, hex_ELVSS);
                        Update_ELVSS_Textbox(Set, band, ELVSS, hex_ELVSS[band]);
                        Thread.Sleep(delay); f1.GB_Status_AppendText_Nextline("Applied Delay(ms):" + delay.ToString(), Color.Blue);
                        f1.CA_Measure_For_ELVSS(ELVSS.ToString(), ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);

                        //added on 200330
                        double ELVSS_Max = Convert.ToDouble(textBox_ELVSS_Max.Text);
                        double ELVSS_Min = Convert.ToDouble(textBox_ELVSS_Min.Text);

                        Imported_my_cpp_dll.ELVSS_Compensation_For_DP173(ELVSS_Min, ELVSS_Max, ref ELVSS_Find_Finish, First_ELVSS, ref ELVSS, ref First_Slope, ELVSS_Margin, Slope_Margin, First_Measure.double_X, First_Measure.double_Y
                            , First_Measure.double_Lv, Measure.double_X, Measure.double_Y, Measure.double_Lv);

                        if (ELVSS_Find_Finish)
                        {
                            f1.GB_Status_AppendText_Nextline("ELVSS Find Finish", Color.Black);
                            break;
                        }
                    }
                }
                //Set and send ELVSS_Set1 
                SET_hex_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set1.Text), hex_ELVSS);
                SET_hex_ELVSS(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set1.Text), hex_ELVSS);

                Send_ELVSS_Setting(Gamma_Set.Set1, hex_ELVSS);
                if (checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Checked) Send_ELVSS_Setting(Gamma_Set.Set5, hex_ELVSS);

                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_ELVSS = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_ELVSS[i] = (Convert.ToInt32(hex_ELVSS[i], 16) - 5).ToString("X2");

                    Send_Lowtemperature_ELVSS_Setting(Gamma_Set.Set1, Low_Temp_hex_ELVSS);
                    if (checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Checked) Send_Lowtemperature_ELVSS_Setting(Gamma_Set.Set5, Low_Temp_hex_ELVSS);
                }

                //Set and send ELVSS_Set2 
                SET_hex_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set2.Text), hex_ELVSS);
                SET_hex_ELVSS(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set2.Text), hex_ELVSS);
                Send_ELVSS_Setting(Gamma_Set.Set2, hex_ELVSS);
                if (checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Checked) Send_ELVSS_Setting(Gamma_Set.Set6, hex_ELVSS);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_ELVSS = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_ELVSS[i] = (Convert.ToInt32(hex_ELVSS[i], 16) - 5).ToString("X2");

                    Send_Lowtemperature_ELVSS_Setting(Gamma_Set.Set2, Low_Temp_hex_ELVSS);
                    if (checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Checked) Send_Lowtemperature_ELVSS_Setting(Gamma_Set.Set6, Low_Temp_hex_ELVSS);
                }

                //Set and send ELVSS_Set3
                SET_hex_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set3.Text), hex_ELVSS);
                SET_hex_ELVSS(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set3.Text), hex_ELVSS);
                Send_ELVSS_Setting(Gamma_Set.Set3, hex_ELVSS);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_ELVSS = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_ELVSS[i] = (Convert.ToInt32(hex_ELVSS[i], 16) - 5).ToString("X2");

                    Send_Lowtemperature_ELVSS_Setting(Gamma_Set.Set3, Low_Temp_hex_ELVSS);
                }

                //Set and send ELVSS_Set4
                SET_hex_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set4.Text), hex_ELVSS);
                SET_hex_ELVSS(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set4.Text), hex_ELVSS);
                Send_ELVSS_Setting(Gamma_Set.Set4, hex_ELVSS);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_ELVSS = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_ELVSS[i] = (Convert.ToInt32(hex_ELVSS[i], 16) - 5).ToString("X2");

                    Send_Lowtemperature_ELVSS_Setting(Gamma_Set.Set4, Low_Temp_hex_ELVSS);
                }

                //Set and send ELVSS_Set5
                SET_hex_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set5.Text), hex_ELVSS);
                SET_hex_ELVSS(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set5.Text), hex_ELVSS);
                Send_ELVSS_Setting(Gamma_Set.Set5, hex_ELVSS);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_ELVSS = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_ELVSS[i] = (Convert.ToInt32(hex_ELVSS[i], 16) - 5).ToString("X2");

                    Send_Lowtemperature_ELVSS_Setting(Gamma_Set.Set5, Low_Temp_hex_ELVSS);
                }

                //Set and send ELVSS_Set6
                SET_hex_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set6.Text), hex_ELVSS);
                SET_hex_ELVSS(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set6.Text), hex_ELVSS);
                Send_ELVSS_Setting(Gamma_Set.Set6, hex_ELVSS);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_ELVSS = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_ELVSS[i] = (Convert.ToInt32(hex_ELVSS[i], 16) - 5).ToString("X2");

                    Send_Lowtemperature_ELVSS_Setting(Gamma_Set.Set6, Low_Temp_hex_ELVSS);
                }


                string[] hex_VINIT2 = new string[11];
                //Set and send VINIT2_Set1
                SET_hex_VINIT2(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B0_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B1_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B2_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B3_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B4_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B5_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B6_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B7_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B8_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B9_Set1.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set1.Text) + Convert.ToDouble(Vinit_Offset_B10_Set1.Text), hex_VINIT2, 0);
                Send_Vinit2_Setting(Gamma_Set.Set1, hex_VINIT2);
                if (checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Checked) Send_Vinit2_Setting(Gamma_Set.Set5, hex_VINIT2);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_VINIT2 = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_VINIT2[i] = (Convert.ToInt32(hex_VINIT2[i], 16) + 5).ToString("X2");

                    Send_Lowtemperature_Vinit2_Setting(Gamma_Set.Set1, Low_Temp_hex_VINIT2);
                    if (checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Checked) Send_Lowtemperature_Vinit2_Setting(Gamma_Set.Set5, Low_Temp_hex_VINIT2);
                }

                //Set and send VINIT2_Set2
                SET_hex_VINIT2(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B0_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B1_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B2_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B3_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B4_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B5_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B6_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B7_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B8_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B9_Set2.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set2.Text) + Convert.ToDouble(Vinit_Offset_B10_Set2.Text), hex_VINIT2, 0);
                Send_Vinit2_Setting(Gamma_Set.Set2, hex_VINIT2);
                if (checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Checked) Send_Vinit2_Setting(Gamma_Set.Set6, hex_VINIT2);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_VINIT2 = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_VINIT2[i] = (Convert.ToInt32(hex_VINIT2[i], 16) + 5).ToString("X2");

                    Send_Lowtemperature_Vinit2_Setting(Gamma_Set.Set2, Low_Temp_hex_VINIT2);
                    if (checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Checked) Send_Lowtemperature_Vinit2_Setting(Gamma_Set.Set6, Low_Temp_hex_VINIT2);
                }

                //Set and send VINIT2_Set3
                SET_hex_VINIT2(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B0_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B1_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B2_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B3_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B4_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B5_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B6_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B7_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B8_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B9_Set3.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set3.Text) + Convert.ToDouble(Vinit_Offset_B10_Set3.Text), hex_VINIT2, 0);
                Send_Vinit2_Setting(Gamma_Set.Set3, hex_VINIT2);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_VINIT2 = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_VINIT2[i] = (Convert.ToInt32(hex_VINIT2[i], 16) + 5).ToString("X2");

                    Send_Lowtemperature_Vinit2_Setting(Gamma_Set.Set3, Low_Temp_hex_VINIT2);
                }

                //Set and send VINIT2_Set4
                SET_hex_VINIT2(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B0_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B1_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B2_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B3_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B4_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B5_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B6_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B7_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B8_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B9_Set4.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set4.Text) + Convert.ToDouble(Vinit_Offset_B10_Set4.Text), hex_VINIT2, 0);
                Send_Vinit2_Setting(Gamma_Set.Set4, hex_VINIT2);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_VINIT2 = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_VINIT2[i] = (Convert.ToInt32(hex_VINIT2[i], 16) + 5).ToString("X2");

                    Send_Lowtemperature_Vinit2_Setting(Gamma_Set.Set4, Low_Temp_hex_VINIT2);
                }

                //Set and send VINIT2_Set5
                SET_hex_VINIT2(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B0_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B1_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B2_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B3_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B4_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B5_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B6_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B7_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B8_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B9_Set5.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set5.Text) + Convert.ToDouble(Vinit_Offset_B10_Set5.Text), hex_VINIT2, 0);
                Send_Vinit2_Setting(Gamma_Set.Set5, hex_VINIT2);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_VINIT2 = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_VINIT2[i] = (Convert.ToInt32(hex_VINIT2[i], 16) + 5).ToString("X2");

                    Send_Lowtemperature_Vinit2_Setting(Gamma_Set.Set5, Low_Temp_hex_VINIT2);
                }

                //Set and send VINIT2_Set6
                SET_hex_VINIT2(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B0_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B1_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B2_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B3_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B4_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B5_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B6_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B7_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B8_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(9, ELVSS + Convert.ToDouble(ELVSS_B9_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B9_Set6.Text), hex_VINIT2, 0);
                SET_hex_VINIT2(10, ELVSS + Convert.ToDouble(ELVSS_B10_Offset_Set6.Text) + Convert.ToDouble(Vinit_Offset_B10_Set6.Text), hex_VINIT2, 0);
                Send_Vinit2_Setting(Gamma_Set.Set6, hex_VINIT2);
                if (checkBox_ELVSS_VINIT2_Low_Temperature.Checked)
                {
                    string[] Low_Temp_hex_VINIT2 = new string[11];
                    for (int i = 0; i < 11; i++) Low_Temp_hex_VINIT2[i] = (Convert.ToInt32(hex_VINIT2[i], 16) + 5).ToString("X2");

                    Send_Lowtemperature_Vinit2_Setting(Gamma_Set.Set6, Low_Temp_hex_VINIT2);
                }
            }
            button_Read_ELVSS_Vinit.PerformClick();
        }




        public void Groupbox35_Show()
        {
            this.groupBox35.Show();
        }

        public void Groupbox35_Hide()
        {
            this.groupBox35.Hide();
        }

        private bool Is_AM1_HBM_OC(int band, bool AM1_OC, bool AM1_OC_Finished)
        {
            if (band == 1 && AM1_OC && AM1_OC_Finished == false)
                return true;
            else
                return false;
        }

        private RGB Get_AM1_Dec(double Voltage_VREG1_REF2047, double Vreg1_voltage, RGB_Double AM1_Voltage)
        {
            RGB AM1_Dec = new RGB();
            AM1_Dec.int_R = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Voltage.double_R);
            AM1_Dec.int_G = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Voltage.double_G);
            AM1_Dec.int_B = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Voltage.double_B);
            return AM1_Dec;
        }

        private RGB_Double Get_AM0_Voltage(string R_AM0_Hex, string G_AM0_Hex, string B_AM0_Hex)
        {
            RGB_Double AM0_Voltage = new RGB_Double();
            int Dec_AM_R = Convert.ToInt32(R_AM0_Hex, 16); if (Dec_AM_R > 127) Dec_AM_R = 127;
            int Dec_AM_G = Convert.ToInt32(G_AM0_Hex, 16); if (Dec_AM_G > 127) Dec_AM_G = 127;
            int Dec_AM_B = Convert.ToInt32(B_AM0_Hex, 16); if (Dec_AM_B > 127) Dec_AM_B = 127;
            AM0_Voltage.double_R = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_AM_R);
            AM0_Voltage.double_G = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_AM_G);
            AM0_Voltage.double_B = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_AM_B);
            return AM0_Voltage;
        }

        bool Is_AM1_Within_Voltage_Range(RGB_Double AM1_Voltage, RGB_Double AM0_Voltage)
        {
            if ((AM1_Voltage.double_R < AM0_Voltage.double_R)
                && (AM1_Voltage.double_G < AM0_Voltage.double_G)
                && (AM1_Voltage.double_B < AM0_Voltage.double_B))
                return true;
            else
            {
                System.Windows.Forms.MessageBox.Show("At lease One AM1 > AM0 , Out of Range!!");
                return false;
            }
        }


        RGB_Double Get_GR0_Voltage(string[] Hex_Param, int[] Dec_Param, double Vreg1_voltage, RGB[] Selected_Band_gray_Gamma, RGB_Double[] Selected_Band_gray_Gamma_Voltage_Dll)
        {
            //R 
            Selected_Band_gray_Gamma[0].int_R = ((Dec_Param[0] & 0x10) << 4) + Dec_Param[3];//AM2
            Selected_Band_gray_Gamma[1].int_R = ((Dec_Param[1] & 0x80) << 1) + Dec_Param[6];//GR7
            Selected_Band_gray_Gamma[2].int_R = ((Dec_Param[1] & 0x40) << 2) + Dec_Param[7];//GR6
            Selected_Band_gray_Gamma[3].int_R = ((Dec_Param[1] & 0x20) << 3) + Dec_Param[8];//GR5
            Selected_Band_gray_Gamma[4].int_R = ((Dec_Param[1] & 0x10) << 4) + Dec_Param[9];//GR4
            Selected_Band_gray_Gamma[5].int_R = ((Dec_Param[1] & 0x08) << 5) + Dec_Param[10];//GR3
            Selected_Band_gray_Gamma[6].int_R = ((Dec_Param[1] & 0x04) << 6) + Dec_Param[11];//GR2
            Selected_Band_gray_Gamma[7].int_R = ((Dec_Param[1] & 0x02) << 7) + Dec_Param[12];//GR1
            Selected_Band_gray_Gamma[8].int_R = ((Dec_Param[1] & 0x01) << 8) + Dec_Param[13];//GR0

            //G
            Selected_Band_gray_Gamma[0].int_G = ((Dec_Param[0] & 0x04) << 6) + Dec_Param[16];//AM2
            Selected_Band_gray_Gamma[1].int_G = ((Dec_Param[14] & 0x80) << 1) + Dec_Param[19];//GR7
            Selected_Band_gray_Gamma[2].int_G = ((Dec_Param[14] & 0x40) << 2) + Dec_Param[20];//GR6
            Selected_Band_gray_Gamma[3].int_G = ((Dec_Param[14] & 0x20) << 3) + Dec_Param[21];//GR5
            Selected_Band_gray_Gamma[4].int_G = ((Dec_Param[14] & 0x10) << 4) + Dec_Param[22];//GR4
            Selected_Band_gray_Gamma[5].int_G = ((Dec_Param[14] & 0x08) << 5) + Dec_Param[23];//GR3
            Selected_Band_gray_Gamma[6].int_G = ((Dec_Param[14] & 0x04) << 6) + Dec_Param[24];//GR2
            Selected_Band_gray_Gamma[7].int_G = ((Dec_Param[14] & 0x02) << 7) + Dec_Param[25];//GR1
            Selected_Band_gray_Gamma[8].int_G = ((Dec_Param[14] & 0x01) << 8) + Dec_Param[26];//GR0

            //B
            Selected_Band_gray_Gamma[0].int_B = ((Dec_Param[0] & 0x01) << 8) + Dec_Param[29]; //AM2
            Selected_Band_gray_Gamma[1].int_B = ((Dec_Param[27] & 0x80) << 1) + Dec_Param[32];//GR7
            Selected_Band_gray_Gamma[2].int_B = ((Dec_Param[27] & 0x40) << 2) + Dec_Param[33];//GR6
            Selected_Band_gray_Gamma[3].int_B = ((Dec_Param[27] & 0x20) << 3) + Dec_Param[34];//GR5
            Selected_Band_gray_Gamma[4].int_B = ((Dec_Param[27] & 0x10) << 4) + Dec_Param[35];//GR4
            Selected_Band_gray_Gamma[5].int_B = ((Dec_Param[27] & 0x08) << 5) + Dec_Param[36];//GR3
            Selected_Band_gray_Gamma[6].int_B = ((Dec_Param[27] & 0x04) << 6) + Dec_Param[37];//GR2
            Selected_Band_gray_Gamma[7].int_B = ((Dec_Param[27] & 0x02) << 7) + Dec_Param[38];//GR1
            Selected_Band_gray_Gamma[8].int_B = ((Dec_Param[27] & 0x01) << 8) + Dec_Param[39];//GR0


            //G255 (AM2)
            Selected_Band_gray_Gamma_Voltage_Dll[0].double_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_R);
            Selected_Band_gray_Gamma_Voltage_Dll[0].double_G = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_G);
            Selected_Band_gray_Gamma_Voltage_Dll[0].double_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_B);

            //G191,G127(GR7,GR6)
            Selected_Band_gray_Gamma_Voltage_Dll[1].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[0].double_R, Selected_Band_gray_Gamma[1].int_R, 1);
            Selected_Band_gray_Gamma_Voltage_Dll[1].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[0].double_G, Selected_Band_gray_Gamma[1].int_G, 1);
            Selected_Band_gray_Gamma_Voltage_Dll[1].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[0].double_B, Selected_Band_gray_Gamma[1].int_B, 1);

            Selected_Band_gray_Gamma_Voltage_Dll[2].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[1].double_R, Selected_Band_gray_Gamma[2].int_R, 2);
            Selected_Band_gray_Gamma_Voltage_Dll[2].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[1].double_G, Selected_Band_gray_Gamma[2].int_G, 2);
            Selected_Band_gray_Gamma_Voltage_Dll[2].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[1].double_B, Selected_Band_gray_Gamma[2].int_B, 2);

            //G63,G31,G15,G7,G1(GR5,GR4,GR3,GR2,GR0)
            Selected_Band_gray_Gamma_Voltage_Dll[3].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[2].double_R, Selected_Band_gray_Gamma[3].int_R, 3);
            Selected_Band_gray_Gamma_Voltage_Dll[3].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[2].double_G, Selected_Band_gray_Gamma[3].int_G, 3);
            Selected_Band_gray_Gamma_Voltage_Dll[3].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[2].double_B, Selected_Band_gray_Gamma[3].int_B, 3);

            Selected_Band_gray_Gamma_Voltage_Dll[4].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[3].double_R, Selected_Band_gray_Gamma[4].int_R, 4);
            Selected_Band_gray_Gamma_Voltage_Dll[4].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[3].double_G, Selected_Band_gray_Gamma[4].int_G, 4);
            Selected_Band_gray_Gamma_Voltage_Dll[4].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[3].double_B, Selected_Band_gray_Gamma[4].int_B, 4);

            Selected_Band_gray_Gamma_Voltage_Dll[5].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[4].double_R, Selected_Band_gray_Gamma[5].int_R, 5);
            Selected_Band_gray_Gamma_Voltage_Dll[5].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[4].double_G, Selected_Band_gray_Gamma[5].int_G, 5);
            Selected_Band_gray_Gamma_Voltage_Dll[5].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[4].double_B, Selected_Band_gray_Gamma[5].int_B, 5);

            //GR2
            Selected_Band_gray_Gamma_Voltage_Dll[6].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[5].double_R, Selected_Band_gray_Gamma[6].int_R, 6);
            Selected_Band_gray_Gamma_Voltage_Dll[6].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[5].double_G, Selected_Band_gray_Gamma[6].int_G, 6);
            Selected_Band_gray_Gamma_Voltage_Dll[6].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[5].double_B, Selected_Band_gray_Gamma[6].int_B, 6);

            //GR1 
            Selected_Band_gray_Gamma_Voltage_Dll[7].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[6].double_R, Selected_Band_gray_Gamma[7].int_R, 7);
            Selected_Band_gray_Gamma_Voltage_Dll[7].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[6].double_G, Selected_Band_gray_Gamma[7].int_G, 7);
            Selected_Band_gray_Gamma_Voltage_Dll[7].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[6].double_B, Selected_Band_gray_Gamma[7].int_B, 7);

            //GR0 
            Selected_Band_gray_Gamma_Voltage_Dll[8].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[6].double_R, Selected_Band_gray_Gamma[8].int_R, 7);
            Selected_Band_gray_Gamma_Voltage_Dll[8].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[6].double_G, Selected_Band_gray_Gamma[8].int_G, 7);
            Selected_Band_gray_Gamma_Voltage_Dll[8].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[6].double_B, Selected_Band_gray_Gamma[8].int_B, 7);

            return Selected_Band_gray_Gamma_Voltage_Dll[8];
        }

        private RGB[] Get_Gamma_Form_Voltage(RGB AM1_Dec, RGB_Double[] Selected_Band_gray_Gamma_Voltage_Dll, double Vreg1_voltage)
        {
            RGB[] Selected_Band_gray_Gamma = new RGB[9];
            //R 
            Selected_Band_gray_Gamma[0].int_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma_Voltage_Dll[0].double_R);//AM2
            Selected_Band_gray_Gamma[1].int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_R, Selected_Band_gray_Gamma_Voltage_Dll[0].double_R, Selected_Band_gray_Gamma_Voltage_Dll[1].double_R, 1);//GR7
            Selected_Band_gray_Gamma[2].int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_R, Selected_Band_gray_Gamma_Voltage_Dll[1].double_R, Selected_Band_gray_Gamma_Voltage_Dll[2].double_R, 2);//GR6
            Selected_Band_gray_Gamma[3].int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_R, Selected_Band_gray_Gamma_Voltage_Dll[2].double_R, Selected_Band_gray_Gamma_Voltage_Dll[3].double_R, 3);//GR5
            Selected_Band_gray_Gamma[4].int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_R, Selected_Band_gray_Gamma_Voltage_Dll[3].double_R, Selected_Band_gray_Gamma_Voltage_Dll[4].double_R, 4);//GR4
            Selected_Band_gray_Gamma[5].int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_R, Selected_Band_gray_Gamma_Voltage_Dll[4].double_R, Selected_Band_gray_Gamma_Voltage_Dll[5].double_R, 5);//GR3
            Selected_Band_gray_Gamma[6].int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_R, Selected_Band_gray_Gamma_Voltage_Dll[5].double_R, Selected_Band_gray_Gamma_Voltage_Dll[6].double_R, 6);//GR2
            Selected_Band_gray_Gamma[7].int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_R, Selected_Band_gray_Gamma_Voltage_Dll[6].double_R, Selected_Band_gray_Gamma_Voltage_Dll[7].double_R, 7);//GR1
            Selected_Band_gray_Gamma[8].int_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_R, Selected_Band_gray_Gamma_Voltage_Dll[6].double_R, Selected_Band_gray_Gamma_Voltage_Dll[8].double_R, 7);//GR0

            //G
            Selected_Band_gray_Gamma[0].int_G = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma_Voltage_Dll[0].double_G);//AM2
            Selected_Band_gray_Gamma[1].int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_G, Selected_Band_gray_Gamma_Voltage_Dll[0].double_G, Selected_Band_gray_Gamma_Voltage_Dll[1].double_G, 1);//GR7
            Selected_Band_gray_Gamma[2].int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_G, Selected_Band_gray_Gamma_Voltage_Dll[1].double_G, Selected_Band_gray_Gamma_Voltage_Dll[2].double_G, 2);//GR6
            Selected_Band_gray_Gamma[3].int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_G, Selected_Band_gray_Gamma_Voltage_Dll[2].double_G, Selected_Band_gray_Gamma_Voltage_Dll[3].double_G, 3);//GR5
            Selected_Band_gray_Gamma[4].int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_G, Selected_Band_gray_Gamma_Voltage_Dll[3].double_G, Selected_Band_gray_Gamma_Voltage_Dll[4].double_G, 4);//GR4
            Selected_Band_gray_Gamma[5].int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_G, Selected_Band_gray_Gamma_Voltage_Dll[4].double_G, Selected_Band_gray_Gamma_Voltage_Dll[5].double_G, 5);//GR3
            Selected_Band_gray_Gamma[6].int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_G, Selected_Band_gray_Gamma_Voltage_Dll[5].double_G, Selected_Band_gray_Gamma_Voltage_Dll[6].double_G, 6);//GR2
            Selected_Band_gray_Gamma[7].int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_G, Selected_Band_gray_Gamma_Voltage_Dll[6].double_G, Selected_Band_gray_Gamma_Voltage_Dll[7].double_G, 7);//GR1
            Selected_Band_gray_Gamma[8].int_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_G, Selected_Band_gray_Gamma_Voltage_Dll[6].double_G, Selected_Band_gray_Gamma_Voltage_Dll[8].double_G, 7);//GR0

            //B
            Selected_Band_gray_Gamma[0].int_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma_Voltage_Dll[0].double_B);//AM2
            Selected_Band_gray_Gamma[1].int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_B, Selected_Band_gray_Gamma_Voltage_Dll[0].double_B, Selected_Band_gray_Gamma_Voltage_Dll[1].double_B, 1);//GR7
            Selected_Band_gray_Gamma[2].int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_B, Selected_Band_gray_Gamma_Voltage_Dll[1].double_B, Selected_Band_gray_Gamma_Voltage_Dll[2].double_B, 2);//GR6
            Selected_Band_gray_Gamma[3].int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_B, Selected_Band_gray_Gamma_Voltage_Dll[2].double_B, Selected_Band_gray_Gamma_Voltage_Dll[3].double_B, 3);//GR5
            Selected_Band_gray_Gamma[4].int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_B, Selected_Band_gray_Gamma_Voltage_Dll[3].double_B, Selected_Band_gray_Gamma_Voltage_Dll[4].double_B, 4);//GR4
            Selected_Band_gray_Gamma[5].int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_B, Selected_Band_gray_Gamma_Voltage_Dll[4].double_B, Selected_Band_gray_Gamma_Voltage_Dll[5].double_B, 5);//GR3
            Selected_Band_gray_Gamma[6].int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_B, Selected_Band_gray_Gamma_Voltage_Dll[5].double_B, Selected_Band_gray_Gamma_Voltage_Dll[6].double_B, 6);//GR2
            Selected_Band_gray_Gamma[7].int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_B, Selected_Band_gray_Gamma_Voltage_Dll[6].double_B, Selected_Band_gray_Gamma_Voltage_Dll[7].double_B, 7);//GR
            Selected_Band_gray_Gamma[8].int_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Dec.int_B, Selected_Band_gray_Gamma_Voltage_Dll[6].double_B, Selected_Band_gray_Gamma_Voltage_Dll[8].double_B, 7);//GR0

            return Selected_Band_gray_Gamma;
        }



        private bool Set1_HBM_AM1_Compensation(ref string R_AM1_Hex, ref string G_AM1_Hex, ref string B_AM1_Hex, OC_Single_Dual_Triple oc_mode, Gamma_Set Set = Gamma_Set.Set1)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Band = 0;
            string Band_Address = DP173.Get_Gamma_Register_Hex_String(Band).Remove(0, 2); //"XX"
            int Offset = 0;
            if (Set == Gamma_Set.Set1) Offset = 0;//Set1 Normal
            else if (Set == Gamma_Set.Set2) Offset = 40;//Set2 Normal
            else if (Set == Gamma_Set.Set3) Offset = 80;//Set3 Normal
            else if (Set == Gamma_Set.Set4) Offset = 120;//Set4 Normal
            else if (Set == Gamma_Set.Set5) Offset = 160;//Set5 Normal
            else if (Set == Gamma_Set.Set6) Offset = 200;//Set6 Normal

            string[] Hex_Param = new string[40];
            int[] Dec_Param = new int[40];
            f1.MX_OTP_Read(Offset, 40, Band_Address);
            for (int i = 0; i < 40; i++)
            {
                Hex_Param[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Dec_Param[i] = Convert.ToInt32(Hex_Param[i], 16);
            }
            int Dec_Vreg1 = DP173_Get_Normal_Initial_Vreg1(Band, Set);
            Vreg1_voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Dec_Vreg1, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);

            RGB[] Selected_Band_gray_Gamma = new RGB[9]; //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)
            RGB_Double[] Selected_Band_gray_Gamma_Voltage_Dll = new RGB_Double[9]; //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)
            string R_AM0_Hex = Hex_Param[5];
            string G_AM0_Hex = Hex_Param[18];
            string B_AM0_Hex = Hex_Param[31];
            RGB_Double AM0_Voltage = Get_AM0_Voltage(R_AM0_Hex, G_AM0_Hex, B_AM0_Hex);
            RGB_Double GR0_Voltage = Get_GR0_Voltage(Hex_Param, Dec_Param, Vreg1_voltage, Selected_Band_gray_Gamma, Selected_Band_gray_Gamma_Voltage_Dll);

            double AM1_Margin_R = Convert.ToDouble(textBox_AM1_Margin_R.Text);
            double AM1_Margin_G = Convert.ToDouble(textBox_AM1_Margin_G.Text);
            double AM1_Margin_B = Convert.ToDouble(textBox_AM1_Margin_B.Text);

            RGB_Double AM1_Voltage = new RGB_Double();
            AM1_Voltage.double_R = (GR0_Voltage.double_R + AM1_Margin_R);
            AM1_Voltage.double_G = (GR0_Voltage.double_G + AM1_Margin_G);
            AM1_Voltage.double_B = (GR0_Voltage.double_B + AM1_Margin_B);


            bool AM1_OC_NG = false;
            if (Is_AM1_Within_Voltage_Range(AM1_Voltage, AM0_Voltage))
            {
                //Get AM1 R/G/B
                RGB AM1_Dec = Get_AM1_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Voltage);
                R_AM1_Hex = AM1_Dec.int_R.ToString("X2");
                G_AM1_Hex = AM1_Dec.int_G.ToString("X2");
                B_AM1_Hex = AM1_Dec.int_B.ToString("X2");
                Selected_Band_gray_Gamma = Get_Gamma_Form_Voltage(AM1_Dec, Selected_Band_gray_Gamma_Voltage_Dll, Vreg1_voltage);

                Hex_Param = Get_RGB_Hex_Param(Selected_Band_gray_Gamma, R_AM0_Hex, G_AM0_Hex, B_AM0_Hex, R_AM1_Hex, G_AM1_Hex, B_AM1_Hex);
                f1.DP173_Long_Packet_CMD_Send(Offset, Hex_Param.Length, Band_Address, Hex_Param);
                Application.DoEvents();

                if (oc_mode == OC_Single_Dual_Triple.Single)
                {
                    DP173_Single_Mode_Set1_HBM_Gamma_Update(Selected_Band_gray_Gamma);
                }
                else if (oc_mode == OC_Single_Dual_Triple.Dual)
                {
                    DP173_Dual_Mode_Set1_HBM_Gamma_Update(Selected_Band_gray_Gamma);
                }

                Application.DoEvents();
                AM1_OC_NG = false;
            }
            else
            {
                AM1_OC_NG = true;
            }
            return AM1_OC_NG;
        }


        private void Single_Mode_Initialize()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Single_Engineering_Mornitoring.getInstance().Show();

            DP173_Single_Engineering_Mornitoring DP173_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            DP173_form_engineer.button_Read_OC_Param_From_Excel_File_Perform_Click();//Added On 200317
            DP173_form_engineer.Engineering_Mode_DataGridview_ReadOnly(true);
            DP173_form_engineer.GridView_Measure_Applied_Loop_Area_Data_Clear();
            DP173_form_engineer.Gamma_Vreg1_Diff_Clear();
            DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB();
            DP173_form_engineer.RadioButton_All_Enable(false);

            Set_Condition_Mipi_Script_Change(Gamma_Set.Set1);//Apply Set1 Before ELVSS/Black Compensation Start

            DP173_form_engineer.Band_Radiobuttion_Select(0);
            DP173_form_engineer.Force_Triger_Band_Set_CheckedChanged_Function(0);
            Application.DoEvents();

            //Initialize
            Optic_Compensation_Stop = false;

            //CA channel Selection
            f1.objMemory.ChannelNO = Convert.ToInt32(f1.textBox_ch.Text);
            f1.CA_Channel_trackBar.Value = Convert.ToInt32(f1.textBox_ch.Text);

            f1.OC_Timer_Start();

            //ProgressBar-related
            int step = 1;
            f1.Set_GB_ProgressBar_Value(0); //Progressbar Value's initializing
            ProgressBar_Max_Step_Setting(step); //Set Progressbar's Step and Max-Value

            //Global Value Initializing
            Optic_Compensation_Succeed = false;

            if (checkBox_Read_DBV_Values.Checked) button_Read_DP173_DBV_Setting.PerformClick();
        }

        private void Single_ELVSS_VREF1_Compensation(DP173_or_Elgin model, RGB[,] All_band_gray_Gamma)
        {
            button_Gamma_Set1_Apply.PerformClick();
            //ELVSS OC
            if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false)
            {
                if (radioButton_ELVSS_Start_From_Band0_First_ELVSS_60.Checked)
                {
                    int start_band = 0;
                    double First_ELVSS = -6.0;
                    double Last_ELVSS = -2.0;
                    DP173_Band_Gray255_Compensation(start_band, All_band_gray_Gamma, Gamma_Set.Set1, model);
                    DP173_ELVSS_Compensation(start_band, First_ELVSS, Last_ELVSS);
                }
                else if (radioButton_ELVSS_Start_From_Band1_First_ELVSS_45.Checked)
                {
                    int start_band = 1;
                    double First_ELVSS = -4.5;
                    double Last_ELVSS = -2.0;
                    DP173_Band_Gray255_Compensation(start_band, All_band_gray_Gamma, Gamma_Set.Set1, model);
                    DP173_ELVSS_Compensation(start_band, First_ELVSS, Last_ELVSS);
                }
            }

            //VREF1 OC
            if (checkBox_VREF1_Comp.Checked && Optic_Compensation_Stop == false)
            {
                int band = 0;
                int gray = 0;
                DP173_Band_Gray255_Compensation(band, All_band_gray_Gamma, Gamma_Set.Set1, model);

                RGB HBM_White_Gamma = new RGB();
                HBM_White_Gamma.Equal_Value(All_band_gray_Gamma[band, gray]);
                double Set1_HBM_RGB_Min_White = Get_Set1_HBM_RGB_Min_White(HBM_White_Gamma);
                DP173_VREF1_Compensation(Set1_HBM_RGB_Min_White);//Set Compensated VREF1   
            }
        }

        private Gamma_Set Get_Single_Mode_Set_and_Apply()
        {
            Gamma_Set Set = new Gamma_Set();
            //---Set1,2,3,4,5,6 Conditon Apply---
            if (radioButton_Single_Mode_Set1.Checked)
            {
                Set = Gamma_Set.Set1;
                Set_Condition_Mipi_Script_Change(Set);
                button_Gamma_Set1_Apply.PerformClick();
            }
            else if (radioButton_Single_Mode_Set2.Checked)
            {
                Set = Gamma_Set.Set2;
                Set_Condition_Mipi_Script_Change(Set);
                button_Gamma_Set2_Apply.PerformClick();
            }
            else if (radioButton_Single_Mode_Set3.Checked)
            {
                Set = Gamma_Set.Set3;
                Set_Condition_Mipi_Script_Change(Set);
                button_Gamma_Set3_Apply.PerformClick();
            }
            else if (radioButton_Single_Mode_Set4.Checked)
            {
                Set = Gamma_Set.Set4;
                Set_Condition_Mipi_Script_Change(Set);
                button_Gamma_Set4_Apply.PerformClick();
            }
            else if (radioButton_Single_Mode_Set5.Checked)
            {
                Set = Gamma_Set.Set5;
                Set_Condition_Mipi_Script_Change(Set);
                button_Gamma_Set5_Apply.PerformClick();
            }
            else if (radioButton_Single_Mode_Set6.Checked)
            {
                Set = Gamma_Set.Set6;
                Set_Condition_Mipi_Script_Change(Set);
                button_Gamma_Set6_Apply.PerformClick();
            }
            return Set;
        }

        private void Single_Calculate_Init_RGBVreg1_And_Apply_And_Measure(DP173_or_Elgin model, bool[] Applied_Band, Gamma_Set Set, Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal)
        {

        }

        private bool Single_Mode_IRC_G255_Verify(DP173_or_Elgin model,Gamma_Set Set)
        {
            bool continue_to_next_gray_loop;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
            DP173_Pattern_Setting(Set, model.gray, model.band, OC_Single_Dual_Triple.Single);//Pattern Setting
            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
            Thread.Sleep(300); //Pattern 안정화 Time
            DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
            DP173_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X"); Application.DoEvents();

            XYLv Limit_For_IRC_Verify_G255 = model.Limit * model.G255_IRC_OC_Verify_Limit_Ratio;
            XYLv Diff = (model.Measure - model.Target).Change_To_Absolute_Values();

            f1.GB_Status_AppendText_Nextline("Diff X/Y/Lv : " + Diff.X + "/" + Diff.Y + "/" + Diff.Lv, Color.Blue);
            f1.GB_Status_AppendText_Nextline("Limit_For_IRC_Verify_G255 X/Y/Lv : " + Limit_For_IRC_Verify_G255.X + "/" + Limit_For_IRC_Verify_G255.Y + "/" + Limit_For_IRC_Verify_G255.Lv, Color.Blue);
            if ((Diff.double_X < Limit_For_IRC_Verify_G255.double_X)
                && (Diff.double_Y < Limit_For_IRC_Verify_G255.double_Y)
                && (Diff.double_Lv < Limit_For_IRC_Verify_G255.double_Lv))
            {
                //model.IRC_G255_Verify_OK = true;
                f1.GB_Status_AppendText_Nextline("IRC_G255_Verify : OK", Color.Green);
                model.gray = 1;
                continue_to_next_gray_loop = true;
            }
            else
            {
                //model.IRC_G255_Verify_OK = false;
                f1.GB_Status_AppendText_Nextline("IRC_G255_Verify : NG", Color.Red);
                continue_to_next_gray_loop = false;
            }
            
            return continue_to_next_gray_loop;
        }


        void Single_Mode_If_Within_Spec_Limit(DP173_or_Elgin model, Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal, Gamma_Set Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Set);

            if (model.gray == 1 && checkBox_G255_IRC_Verify_OC_Apply.Checked && model.band <= DP173_or_Elgin.Max_Normal_Band)
            {
                if (model.IRC_G255_OC_Try_Count > model.IRC_G255_OC_Try_Max_Count)
                {
                    Optic_Compensation_Succeed = false;
                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                    System.Windows.Forms.MessageBox.Show("IRC_G255_Verify_NG(IRC_G255_OC_Try_Count > IRC_G255_OC_Try_Max_Count)");
                    if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                }
                else
                {
                    model.IRC_G255_OC_Try_Count++;
                    model.gray = -1;
                    f1.GB_Status_AppendText_Nextline("IRC_G255_OC_Try_Count : " + model.IRC_G255_OC_Try_Count.ToString(), Color.Blue);
                }
            }
            else
            {
                Optic_Compensation_Succeed = true;
                textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
            }
        }
                               



        private void Single_Mode_Optic_compensation()
        {
            Single_Mode_Initialize();

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_Single_Engineering_Mornitoring DP173_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            DP173_or_Elgin model = new DP173_or_Elgin(OC_Single_Dual_Triple.Single);
            Initial_RGBVreg1_Calculation_EA9154 initial_RGBVreg1_cal = new Initial_RGBVreg1_Calculation_EA9154();

            //All_band_gray_Gamma Update
            DP173_Get_All_Band_Gray_Gamma(model.All_band_gray_Gamma); //Get All_band_gray_Gamma[12,8]

            //Single ELVSS Compensation
            Single_ELVSS_VREF1_Compensation(model, model.All_band_gray_Gamma);

            // Black Compensation
            Black_Compensation(model);
            button_Vreg1_Read.PerformClick();//Update vreg1 and REF_voltage(global variables)
            button_Read_AM0_VREF2_HBM_Set1_Only.PerformClick();//Added On 200219 (VREF1/VREF2047/AM0 Read)

            if (checkBox_Apply_FX_Previous_Band_Vreg1.Checked || checkBox_Apply_FX_3points_RGB.Checked)
            {
                int Last_Band = Convert.ToInt32(numericUpDown2.Value);
                for (int b = 0; b <= Last_Band; b++) initial_RGBVreg1_cal.Selected_Band[b] = true;
                if (Last_Band < 10) for (int b = (Last_Band + 1); b <= 10; b++) initial_RGBVreg1_cal.Selected_Band[b] = false;
            }

            if (Optic_Compensation_Stop) return;
            Gamma_Set Set = Get_Single_Mode_Set_and_Apply();//This must come after ELVSS OC (Because ELVSS OC is proccessed in Set1 condition)
            initial_RGBVreg1_cal.Set_Calculated_RGBVreg1_Vdata_Pointer(Set);

            bool AM1_OC = radioButton_HBM_AM1_OC.Checked;
            bool AM1_OC_Finished = false;
            for (model.band = 0; model.band < 14 && Optic_Compensation_Stop == false; model.band++)
            {
                if (Is_AM1_HBM_OC(model.band, AM1_OC, AM1_OC_Finished))
                {
                    Optic_Compensation_Stop = Set1_HBM_AM1_Compensation(ref model.R_AM1_Hex, ref model.G_AM1_Hex, ref model.B_AM1_Hex, OC_Single_Dual_Triple.Single, Set);
                    model.Update_AM1_Dec_From_AM1_Hex();
                    AM1_OC_Finished = true;
                    model.band = -1;
                    continue;
                }

                f1.GB_Status_AppendText_Nextline("Band" + (model.band).ToString(), Color.Green);
                if (Optic_Compensation_Stop) break;
                model.Gamma_Out_Of_Register_Limit = false;

                if (Band_BSQH_Selection(ref model.band)) //If this band is not selected , move on to the next band
                {
                    DP173_form_engineer.Band_Radiobuttion_Select(model.band);//Select Band
                    DP173_DBV_Setting(model.band);  //DBV Setting

                    if (model.Is_AOD_Band()) //AOD1,2,3
                    {
                        DP173_Pattern_Setting(Gamma_Set.Set1, 0, model.band, OC_Single_Dual_Triple.Single);//Pattern Setting
                        Thread.Sleep(300);
                        f1.AOD_On();
                        DP173_DBV_Setting(model.band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                    }

                    double Before_Calculated_Init_Vreg1 = 0;
                    bool Gray255_Calculated = false;

                    if (checkBox_Vreg1_Compensation.Checked && (model.band < 11))
                    {
                        model.Vreg1_loop_count = 0; //Vreg1 loop countR
                        model.Vreg1_Infinite_Count = 0;
                        model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                        Before_Calculated_Init_Vreg1 = model.Vreg1;

                        if ((checkBox_Apply_FX_Previous_Band_Vreg1.Checked) && (model.band >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]))
                        {
                            Gray255_Calculated = true;

                            double band_Target_Lv = Convert.ToDouble(DP173_form_engineer.dataGridView_OC_param.Rows[(model.band * 8) + (0 + 2)].Cells[9].Value);
                            int[] Previous_Band_Gamma_Red = new int[8];
                            int[] Previous_Band_Gamma_Green = new int[8];
                            int[] Previous_Band_Gamma_Blue = new int[8];
                            double[] Previous_Band_Target_Lv = new double[8];
                            for (int i = 0; i < 8; i++)
                            {
                                Previous_Band_Gamma_Red[i] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[1].Value);
                                Previous_Band_Gamma_Green[i] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[2].Value);
                                Previous_Band_Gamma_Blue[i] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[3].Value);
                                if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[6].Value);
                                else if (this.radioButton_Previous_Target_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[9].Value);
                            }

                            int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1(model.band - 1, Set);//Prev_Band Vreg1

                            //C++ Dll Initial Calculate R/Vreg1/B Verify OK (= C# Result)
                            Imported_my_cpp_dll.DP173_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.Vreg1, ref model.Vreg1_First_Gamma_Red, ref model.Vreg1_First_Gamma_Blue, initial_RGBVreg1_cal.Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, model.band, band_Target_Lv, Previous_Band_Vreg1_Dec
    , Previous_Band_Target_Lv, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                            f1.GB_Status_AppendText_Nextline("(1)After(Dll C++,Precision : 0.001) Vreg1_First_Gamma_Red/Vreg1/Vreg1_First_Gamma_Blue : " + model.Vreg1_First_Gamma_Red.ToString() + "/" + model.Vreg1.ToString() + "/" + model.Vreg1_First_Gamma_Blue.ToString(), Color.Red);

                            //Set Calculated Vreg1_dec
                            DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);
                        }
                        else
                        {
                            Gray255_Calculated = false;
                        }

                        if (Gray255_Calculated)
                        {
                            model.Initial_Vreg1 = model.Vreg1;
                            model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1;
                            DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(model.band, model.Initial_Vreg1, true);
                            DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(model.band, model.Diff_Vreg1, true);
                        }
                        else
                        {
                            f1.GB_Status_AppendText_Nextline("(Initial Vreg1 is not applied) Vreg1 : " + model.Vreg1.ToString(), Color.Red);
                            model.Initial_Vreg1 = model.Vreg1;
                            model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1;
                            DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(model.band, model.Initial_Vreg1);
                            DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(model.band, model.Diff_Vreg1);
                        }
                    }

                    else if (model.band < 11)
                    {
                        if ((checkBox_Apply_FX_Previous_Band_Vreg1.Checked) && (model.band >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]))
                        {
                            model.Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);//Current_Band Vreg1
                            Gray255_Calculated = true;

                            double band_Target_Lv = Convert.ToDouble(DP173_form_engineer.dataGridView_OC_param.Rows[(model.band * 8) + (0 + 2)].Cells[9].Value);
                            int[] Previous_Band_Gamma_Red = new int[8];
                            int[] Previous_Band_Gamma_Green = new int[8];
                            int[] Previous_Band_Gamma_Blue = new int[8];
                            double[] Previous_Band_Target_Lv = new double[8];
                            for (int i = 0; i < 8; i++)
                            {
                                Previous_Band_Gamma_Red[i] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[1].Value);
                                Previous_Band_Gamma_Green[i] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[2].Value);
                                Previous_Band_Gamma_Blue[i] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[3].Value);
                                if (radioButton_Previous_Measure_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[6].Value);
                                else if (this.radioButton_Previous_Target_Lv.Checked) Previous_Band_Target_Lv[i] = Convert.ToDouble(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (i + 2)].Cells[9].Value);
                            }

                            int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1(model.band - 1, Set);//Prev_Band Vreg1

                            Imported_my_cpp_dll.DP173_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(model.Vreg1, model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.G255_First_Gamma_Red, ref model.G255_First_Gamma_Green, ref model.G255_First_Gamma_Blue, initial_RGBVreg1_cal.Selected_Band, Previous_Band_Gamma_Red, Previous_Band_Gamma_Green, Previous_Band_Gamma_Blue, model.band, band_Target_Lv, Previous_Band_Vreg1_Dec
                                                                                              , Previous_Band_Target_Lv, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                            f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ (Gray255) After Gamma_R/G/B : " + model.G255_First_Gamma_Red.ToString() + "/" + model.G255_First_Gamma_Green.ToString() + "/" + model.G255_First_Gamma_Blue.ToString(), Color.Blue);

                            //Copy "Previous Band Gamma to Current Band Gamma" and Set "All_band_gray_Gamma"
                            DP173_form_engineer.Copy_Previous_Band_Gamma(model.band);
                            System.Windows.Forms.Application.DoEvents();
                            DP173_form_engineer.Get_Band_Gray_Gamma(model.All_band_gray_Gamma, model.band);

                            //Set Calculated Vreg1_dec
                            DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);

                            model.Initial_Vreg1 = model.Vreg1;
                            model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1;
                            DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(model.band, model.Initial_Vreg1);
                            DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(model.band, model.Diff_Vreg1);
                        }
                        else
                        {
                            Gray255_Calculated = false;
                        }
                    }

                    if (checkBox_G255_IRC_Verify_OC_Apply.Checked) model.IRC_G255_Verify_OC_Vars_Init();
                    for (model.gray = 0; model.gray < 8; model.gray++)
                    {
                        if (Optic_Compensation_Stop) break;

                        DP173_Get_Param(model.gray, ref model.Gamma, ref model.Target, ref model.Limit, ref model.Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table  

                        if (checkBox_G255_IRC_Verify_OC_Apply.Checked && (model.gray == 0) && (model.IRC_G255_OC_Try_Count > 0) && model.band <= DP173_or_Elgin.Max_Normal_Band) if (Single_Mode_IRC_G255_Verify(model, Set)) continue;

                        bool Calculated = false;
                        if ((model.Target.double_Lv > Convert.ToDouble(textBox_Fast_OC_RGB_Skip_Target.Text)) && (checkBox_Apply_FX_3points_RGB.Checked) && (model.band >= 1) && (model.band < 11) && (model.gray >= 1) && (initial_RGBVreg1_cal.Selected_Band[model.band]))
                        {
                            Calculated = true;

                            f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C# (All Band , 3points, Combine) Before Gamma_R/G/B : " + model.Gamma.int_R.ToString() + "/" + model.Gamma.int_G.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);

                            int Previous_Band_G255_Green_Gamma = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[((model.band - 1) * 8) + (0 + 2)].Cells[2].Value);
                            int Previous_Band_Vreg1_Dec = DP173_Get_Normal_Initial_Vreg1((model.band - 1), Set);
                            int Current_Band_Dec_Vreg1 = DP173_Get_Normal_Initial_Vreg1(model.band, Set);
                            double Prvious_Gray_Gamma_R_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_R;
                            double Prvious_Gray_Gamma_G_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_G;
                            double Prvious_Gray_Gamma_B_Voltage = initial_RGBVreg1_cal.Calculated_Vdata[model.band, (model.gray - 1)].double_B;

                            SJH_Matrix M = new SJH_Matrix();

                            int[] Band_Gray_Gamma_Red = new int[(model.band * 8)]; //Previous Bands
                            int[] Band_Gray_Gamma_Green = new int[(model.band * 8)];
                            int[] Band_Gray_Gamma_Blue = new int[(model.band * 8)];
                            double[] Band_Gray_Target_Lv = new double[(model.band * 8)];
                            int[] Band_Vreg1_Dec = new int[model.band + 1];//Previous Bands + Current Band
                            Band_Vreg1_Dec[model.band] = DP173_Get_Normal_Initial_Vreg1(model.band, Set);
                            //-----------Added On 200312---------------------
                            int second_band_end = Convert.ToInt32(numericUpDown2.Value);
                            if (model.band <= second_band_end) //(band > first_band_end && band <= second_band_end), ex)B4,B5,B6,..,B10 (it means : min(second_band_start) > first_band_end)
                            {
                                for (int b = 0; b < model.band; b++)
                                {
                                    for (int g = 0; g < 8; g++)
                                    {
                                        
                                            Band_Gray_Gamma_Red[(b * 8) + g] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[1].Value);
                                            Band_Gray_Gamma_Green[(b * 8) + g] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[2].Value);
                                            Band_Gray_Gamma_Blue[(b * 8) + g] = Convert.ToInt32(DP173_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[3].Value);
                                            if (radioButton_Previous_Measure_Lv.Checked) Band_Gray_Target_Lv[(b * 8) + g] = Convert.ToDouble(DP173_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[6].Value);
                                            else if (this.radioButton_Previous_Target_Lv.Checked) Band_Gray_Target_Lv[(b * 8) + g] = Convert.ToDouble(DP173_form_engineer.dataGridView_OC_param.Rows[(b * 8) + (g + 2)].Cells[9].Value);
                                        
                                    }
                                    Band_Vreg1_Dec[b] = DP173_Get_Normal_Initial_Vreg1(b, Set);
                                }
                            }
                            //----------------------------------------------
                            double Fx_3points_Combine_LV_1 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_1.Text);
                            double Fx_3points_Combine_LV_2 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_2.Text);
                            double Fx_3points_Combine_LV_3 = Convert.ToDouble(textBox_Fx_3points_Lv_Combine_3.Text);
                            double Fx_3points_Combine_Lv_Distance = Convert.ToDouble(textBox_Fx_3points_Lv_Distance_Combine.Text);

                            double Combine_Lv_Ratio = Convert.ToDouble(textBox_Fast_OC_3Points_Ver2_LV_Combine_Ratio.Text);

                            Imported_my_cpp_dll.Get_Initial_Gamma_Fx_3points_Combine_Points_2(Combine_Lv_Ratio, model.R_AM1_Dec, model.G_AM1_Dec, model.B_AM1_Dec, ref model.Gamma.int_R, ref model.Gamma.int_G, ref model.Gamma.int_B, initial_RGBVreg1_cal.Selected_Band, Band_Gray_Gamma_Red, Band_Gray_Gamma_Green, Band_Gray_Gamma_Blue, Band_Gray_Target_Lv, Current_Band_Dec_Vreg1,
                                    Band_Vreg1_Dec, model.band, model.gray, model.Target.double_Lv, Prvious_Gray_Gamma_R_Voltage, Prvious_Gray_Gamma_G_Voltage, Prvious_Gray_Gamma_B_Voltage,
                                    Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1
                                    , Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance);
                            f1.GB_Status_AppendText_Nextline("Get First RGB From Dll C++ Ver2 (All Band , 3points, Combine) After Gamma_R/G/B : " + model.Gamma.int_R.ToString() + "/" + model.Gamma.int_G.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);

                        }
                        else if (checkBox_Vreg1_Compensation.Checked == false && Gray255_Calculated == true && model.gray == 0)
                        {
                            Calculated = true;
                            model.Gamma.int_R = model.G255_First_Gamma_Red;
                            model.Gamma.int_G = model.G255_First_Gamma_Green;
                            model.Gamma.int_B = model.G255_First_Gamma_Blue;
                        }
                        else
                        {
                            Calculated = false;
                        }

                        //HBM의 Gray255꺼는 IRC 보상 안하면 그냥 받음
                        if (model.Target.double_Lv >= model.Skip_Lv)
                        {
                            if (Calculated || (Gray255_Calculated && model.gray == 0))
                            {
                                model.Cal_Gamma_Init.Equal_Value(model.Gamma);
                                DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_RGB(model.band, model.gray, model.Cal_Gamma_Init, true);
                                f1.GB_Status_AppendText_Nextline(model.Cal_Gamma_Init.R, Color.Red);
                            }
                            else
                            {
                                model.Gamma_Init.Equal_Value(model.Gamma);
                                DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_RGB(model.band, model.gray, model.Gamma_Init);
                                f1.GB_Status_AppendText_Nextline(model.Gamma_Init.R, Color.Blue);
                            }
                        }

                        //Added On 200227 (HBM의 Gray255는 Copy가 안되었는데? .. 확인필요)
                        if (checkBox_Copy_Apply_Band_From_Upper_To_Lower.Checked && (model.band > 0) && (model.band < 11) && (model.gray == 0) && initial_RGBVreg1_cal.Selected_Band[model.band])
                        {
                            DP173_form_engineer.Copy_Previous_Band_Gamma(model.band); Application.DoEvents();
                            DP173_form_engineer.Get_Band_Gray_Gamma(model.All_band_gray_Gamma, model.band); Application.DoEvents();
                            Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                        }

                        if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + model.Limit.double_X.ToString() + "/" + model.Limit.double_Y.ToString() + "/" + model.Limit.double_Lv.ToString(), Color.Red);

                        if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked && model.Vreg1_loop_count == 0)
                        {
                            DP173_form_engineer.Get_Gamma_Only_DP173(model.band - 1, 0, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B);
                            model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);

                            if (checkBox_Apply_FX_Previous_Band_Vreg1.Checked && initial_RGBVreg1_cal.Selected_Band[model.band] == true)
                            {
                                f1.GB_Status_AppendText_Nextline("(1)Before R/Vreg1/B : " + model.Gamma.int_R.ToString() + "/" + Before_Calculated_Init_Vreg1.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Blue);
                                model.Gamma.int_R = model.Vreg1_First_Gamma_Red;
                                model.Gamma.int_B = model.Vreg1_First_Gamma_Blue;
                                if (Gray255_Calculated)
                                {
                                    model.Cal_Gamma_Init.Equal_Value(model.Gamma);
                                    DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_RGB(model.band, model.gray, model.Cal_Gamma_Init, true);
                                }
                                f1.GB_Status_AppendText_Nextline("(1)After R/Vreg1/B : " + model.Gamma.int_R.ToString() + "/" + model.Vreg1.ToString() + "/" + model.Gamma.int_B.ToString(), Color.Red);
                            }
                        }

                        DP173_Pattern_Setting(Set, model.gray, model.band, OC_Single_Dual_Triple.Single);//Pattern Setting
                        Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                        Thread.Sleep(300); //Pattern 안정화 Time
                        DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                        f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                        DP173_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X"); Application.DoEvents();
                        
                        model.loop_count = 0;
                        model.Infinite_Count = 0;
                        

                        Optic_Compensation_Succeed = false;
                        model.Within_Spec_Limit = false;

                        while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                        {
                            if (model.Target.double_Lv < model.Skip_Lv)
                            {
                                if (model.band >= 1)
                                {
                                    DP173_form_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Prev_Band_Gray255_Gamma.int_R, ref model.Prev_Band_Gray255_Gamma.int_G, ref model.Prev_Band_Gray255_Gamma.int_B);
                                    model.Gamma.Equal_Value(model.Prev_Band_Gray255_Gamma);
                                }

                                Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);

                                ///----Update R/G/B/Vreg1 Voltage---- (All_band_gray_Gamma as the base)
                                initial_RGBVreg1_cal.Update_Calculated_Vdata(model, Set);

                                //-----------------------------------
                                model.Measure.Set_Value(0, 0, 0);
                                DP173_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                f1.GB_Status_AppendText_Nextline("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Compensation Skip (Target Lv : " + model.Target.double_Lv.ToString() + ") < " + model.Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                Optic_Compensation_Succeed = true;
                                break;
                            }
                            //Vreg1 + Sub-Compensation (Change Gamma Value)
                            if (model.Is_Normal_Band() && model.gray == 0 && checkBox_Vreg1_Compensation.Checked)
                            {
                                Vreg1_Infinite_Loop_Check(model.Vreg1_loop_count, model);
                                if (model.Vreg1_loop_count < model.loop_count_max)
                                {
                                    //f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                    model.Prev_Vreg1 = model.Vreg1;
                                    model.Prev_Gamma.Equal_Value(model.Gamma);

                                    model.Vreg1_Compensation();

                                    f1.Showing_Diff_and_Current_Vreg1_and_Gamma(model.Prev_Gamma, model.Gamma, model.Prev_Vreg1, model.Vreg1);

                                    if (Math.Abs(model.Vreg1 - model.Prev_Vreg1) >= 1) model.Vreg1_Need_To_Be_Updated = true;
                                    else model.Vreg1_Need_To_Be_Updated = false;

                                    if (model.Vreg1_Need_To_Be_Updated)
                                    {
                                        DP173_Update_and_Send_Vreg1_and_Textbox_Update(model, Set);

                                        model.Diff_Vreg1 = model.Vreg1 - model.Initial_Vreg1;
                                        if (Gray255_Calculated) DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(model.band, model.Diff_Vreg1, true);
                                        else DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(model.band, model.Diff_Vreg1);
                                    }
                                }
                                model.Vreg1_loop_count++;
                                model.loop_count++;
                                if (model.Vreg1_Infinite_Count >= 3)
                                {
                                    Extension_Applied = "O";
                                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                }
                                else Extension_Applied = "X";
                            }
                            else
                            {
                                model.Vreg1_Need_To_Be_Updated = false;

                                model.Prev_Gamma.Equal_Value(model.Gamma);
                                Infinite_Loop_Check(model.loop_count, model);

                                model.Sub_Compensation();

                                //Engineering Mode
                                f1.Showing_Diff_and_Current_Gamma(model.Prev_Gamma, model.Gamma);
                                model.loop_count++;

                                if (model.Infinite_Count >= 3)
                                {
                                    Extension_Applied = "O";
                                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + model.Extension.double_X.ToString() + "," + model.Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                }
                                else Extension_Applied = "X";

                            }

                            if (model.Vreg1_Need_To_Be_Updated == false)
                            {
                                Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                                Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)

                                if (Calculated || (Gray255_Calculated && model.gray == 0))
                                {
                                    int DIff_R = model.Gamma.int_R - model.Cal_Gamma_Init.int_R;
                                    int DIff_G = model.Gamma.int_G - model.Cal_Gamma_Init.int_G;
                                    int DIff_B = model.Gamma.int_B - model.Cal_Gamma_Init.int_B;
                                    DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(model.band, model.gray, DIff_R, DIff_G, DIff_B, true);
                                }
                                else
                                {
                                    int DIff_R = model.Gamma.int_R - model.Gamma_Init.int_R;
                                    int DIff_G = model.Gamma.int_G - model.Gamma_Init.int_G;
                                    int DIff_B = model.Gamma.int_B - model.Gamma_Init.int_B;
                                    DP173_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(model.band, model.gray, DIff_R, DIff_G, DIff_B);
                                }
                            }

                            if (model.Within_Spec_Limit)
                            {
                                Single_Mode_If_Within_Spec_Limit(model, initial_RGBVreg1_cal, Set);
                                break;
                            }

                            if (model.Gamma_Out_Of_Register_Limit)
                            {
                                if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                    Optic_Compensation_Succeed = Apply_Single_Mode_Upper_Band_Gray(model, Set); //Added on 200519
                                else
                                    Optic_Compensation_Succeed = false;

                                textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");

                                if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                break;
                            }
                            textBox_loop_count.Text = (model.loop_count).ToString();

                            if (model.loop_count == model.loop_count_max)
                            {
                                if (checkBox_OC_Fail_Prevension.Checked && model.Is_Normal_Band())
                                    Optic_Compensation_Succeed = Apply_Single_Mode_Upper_Band_Gray(model, Set); //Added on 200519
                                else
                                    Optic_Compensation_Succeed = false;

                                textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                System.Windows.Forms.MessageBox.Show("B" + model.band.ToString() + "/G" + model.gray.ToString() + " Loop Count Over");

                                if (this.checkBox_Continue_After_Fail.Checked == false) Optic_Compensation_Stop = true;
                                break;
                            }
                            DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                            if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Measured Lv : " + model.Measure.double_Lv.ToString(), Color.Black);
                            DP173_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(model.Target, model.Measure, model.Limit, model.Extension);
                            Application.DoEvents();
                        }
                        f1.GB_ProgressBar_PerformStep();
                        if (checkBox_Only_255G.Checked)
                            model.gray = 8;
                    }
                    if (model.Is_AOD_Band()) //AOD1,2,3
                        f1.AOD_Off();
                }
            }//Band Loop End
      
            DP173_form_engineer.Engineering_Mode_DataGridview_ReadOnly(false);
            f1.OC_Timer_Stop();

            DP173_DBV_Setting(1);  //DBV Setting    
            f1.PTN_update(255, 255, 255);
            //---------------------------------------------
            if (Optic_Compensation_Succeed) System.Windows.Forms.MessageBox.Show("Optic Compensation Finish !");
            DP173_form_engineer.RadioButton_All_Enable(true);

            if(f1.radioButton_Debug_Status_Mode.Checked) initial_RGBVreg1_cal.Show_Calculated_Vdata();
            if (f1.radioButton_Debug_Status_Mode.Checked) initial_RGBVreg1_cal.Show_Calculated_Vdata(Set);
        }

        private bool Apply_Single_Mode_Upper_Band_Gray(DP173_or_Elgin model, Gamma_Set Set)
        {
            bool Optic_Compensation_Succeed;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Apply_Single_Mode_Upper_Band_Gray Applied", Color.Blue);

            DP173_Single_Engineering_Mornitoring DP173_form_engineer = (DP173_Single_Engineering_Mornitoring)Application.OpenForms["DP173_Single_Engineering_Mornitoring"];
            double Max_Target_Lv = Convert.ToDouble(textBox_OC_Fail_Prevension_Target_Max_Lv.Text);
            if (model.Target.double_Lv < Max_Target_Lv)
            {
                DP173_form_engineer.Get_Gamma_Only_DP173(model.band - 1, model.gray, ref model.Gamma.int_R, ref model.Gamma.int_G, ref model.Gamma.int_B);
                Update_and_Send_All_Band_Gray_Gamma(model.All_band_gray_Gamma, model.Gamma, model.band, model.gray, Set, model.R_AM1_Hex, model.R_AM0_Hex, model.G_AM1_Hex, model.G_AM0_Hex, model.B_AM1_Hex, model.B_AM0_Hex);
                Thread.Sleep(20);
                DP173_Measure_Average(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv, ref model.total_average_loop_count, model.Target.double_X, model.Target.double_Y, model.Target.double_Lv, model.Limit.double_X, model.Limit.double_Y, model.Limit.double_Lv, model.Extension.double_X, model.Extension.double_Y);
                DP173_Update_Engineering_Sheet(model.Gamma, model.Measure, model.band, model.gray, model.loop_count, Extension_Applied);

                Optic_Compensation_Succeed = true;
            }
            else
            {
                Optic_Compensation_Succeed = false;
            }
            return Optic_Compensation_Succeed;
        }

        private bool Band_BSQH_Selection(ref int band)
        {
            if (checkBox_Band0.Checked == false && band == 0)
                return false;
            else if (checkBox_Band1.Checked == false && band == 1)
                return false;
            else if (checkBox_Band2.Checked == false && band == 2)
                return false;
            else if (checkBox_Band3.Checked == false && band == 3)
                return false;
            else if (checkBox_Band4.Checked == false && band == 4)
                return false;
            else if (checkBox_Band5.Checked == false && band == 5)
                return false;
            else if (checkBox_Band6.Checked == false && band == 6)
                return false;
            else if (checkBox_Band7.Checked == false && band == 7)
                return false;
            else if (checkBox_Band8.Checked == false && band == 8)
                return false;
            else if (checkBox_Band9.Checked == false && band == 9)
                return false;
            else if (checkBox_Band10.Checked == false && band == 10)
                return false;
            else if (checkBox_AOD0.Checked == false && band == 11)
                return false;
            else if (checkBox_AOD1.Checked == false && band == 12)
                return false;
            else if (checkBox_AOD2.Checked == false && band == 13)
                return false;
            else
                return true;
        }





        private void Clear_AM0_Read_Params()
        {
            textBox_REF2047.Text = "";
            textBox_AM0_Resolution.Text = "";
            textBox_R_AM0.Text = "";
            textBox_G_AM0.Text = "";
            textBox_B_AM0.Text = "";
        }

        private bool REF2047_Compensation(double REF2047_Margin, double REF2047_Resolution, double REF2047_Limit_Lv, DP173_or_Elgin model)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("=REF2047 Compensation Start", Color.Black);

            int REF_2047_Max = 127;
            int Dec_REF2047 = REF_2047_Max; //REF2047(127 max) = 7v , REF2047(0 min) = 5.76v
            f1.DP150_One_Param_CMD_Send(21, "B1", Dec_REF2047.ToString("X2"));

            while (Dec_REF2047 > 0)
            {
                f1.GB_Status_AppendText_Nextline("Dec_REF2047 : " + Dec_REF2047.ToString(), Color.Blue);

                if (Optic_Compensation_Stop) return true;//Optic_Compensation_Stop = true;

                if (Dec_REF2047 == 0)
                {
                    Dec_REF2047 += Convert.ToInt16(REF2047_Margin / REF2047_Resolution);
                    f1.DP150_One_Param_CMD_Send(21, "B1", Dec_REF2047.ToString("X2"));
                    f1.GB_Status_AppendText_Nextline("Black(REF2047) Compensation OK (Case 1)", Color.Green);
                    return true;
                }
                else
                {
                    Thread.Sleep(20);//Add on 190820
                    f1.CA_Measure_button_Perform_Click(ref model.Measure.double_X, ref model.Measure.double_Y, ref model.Measure.double_Lv);
                    f1.GB_Status_AppendText_Nextline("Measure.double_Lv / Vreg1_REF2047_Limit_Lv : " + model.Measure.double_Lv.ToString() + "/" + REF2047_Limit_Lv.ToString(), Color.Black);

                    if (model.Measure.double_Lv < REF2047_Limit_Lv)
                    {
                        Dec_REF2047--;
                        f1.DP150_One_Param_CMD_Send(21, "B1", Dec_REF2047.ToString("X2"));
                        continue;
                    }
                    else
                    {
                        Dec_REF2047 += Convert.ToInt16(REF2047_Margin / REF2047_Resolution);
                        if (Dec_REF2047 > REF_2047_Max)
                        {
                            f1.GB_Status_AppendText_Nextline("Black(REF2047) Compensation Fail (Black Margin Is Not Enough)", Color.Red);
                            return false;
                        }
                        else
                        {
                            f1.DP150_One_Param_CMD_Send(21, "B1", Dec_REF2047.ToString("X2"));
                            f1.GB_Status_AppendText_Nextline("Black(REF2047) Compensation OK (Case 2)", Color.Green);
                            return true;
                        }
                    }
                }
            }
            return true;
        }

        private bool AM0_Compensation(double Margin_R, double Margin_G, double Margin_B)
        {
            //Get Original Voltage_VREG1_REF2047
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(21, 7, "B1");
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)
            int Dec_VREG1_REF2047 = (Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F);
            double Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v
            double AM0_Resolution = Imported_my_cpp_dll.Get_DP173_EA9154_AM0_Resolution(Hex_VREG1_REF1, Hex_VREG1_REF2047);

            int AM0_Max = 127;
            int New_Dec_AM_R = Convert.ToInt32(Margin_R / AM0_Resolution);
            int New_Dec_AM_G = Convert.ToInt32(Margin_G / AM0_Resolution);
            int New_Dec_AM_B = Convert.ToInt32(Margin_B / AM0_Resolution);

            bool AM0_OC_Ok = true;
            if (New_Dec_AM_R > AM0_Max)
            {
                f1.GB_Status_AppendText_Nextline("Red Black Margin NG, New_Dec_AM_R : " + New_Dec_AM_R.ToString() + "(>127)", Color.Red);
                AM0_OC_Ok = false;
            }
            if (New_Dec_AM_G > AM0_Max)
            {
                f1.GB_Status_AppendText_Nextline("Red Black Margin NG, New_Dec_AM_G : " + New_Dec_AM_G.ToString() + "(>127)", Color.Red);
                AM0_OC_Ok = false;
            }
            if (New_Dec_AM_B > AM0_Max)
            {
                f1.GB_Status_AppendText_Nextline("Red Black Margin NG, New_Dec_AM_B : " + New_Dec_AM_B.ToString() + "(>127)", Color.Red);
                AM0_OC_Ok = false;
            }

            if (AM0_OC_Ok)
            {
                string New_HBM_AM0_Hex_R = (New_Dec_AM_R & 0x7F).ToString("X2");
                string New_HBM_AM0_Hex_G = (New_Dec_AM_G & 0x7F).ToString("X2");
                string New_HBM_AM0_Hex_B = (New_Dec_AM_B & 0x7F).ToString("X2");
                Set_AM0(New_HBM_AM0_Hex_R, New_HBM_AM0_Hex_G, New_HBM_AM0_Hex_B);
            }

            return AM0_OC_Ok;
        }


        private bool Sub_Black_Compensation(DP173_or_Elgin model)
        {
            Clear_AM0_Read_Params();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP173_DBV_Setting(0);//DBV = Band0
            f1.PTN_update(0, 0, 0);//Black Pattern
            Set_AM0("00", "00", "00");//Set AM0 = 0x00
            Thread.Sleep(300);

            //REF2047 Compensation
            double REF2047_Margin = Convert.ToDouble(textBox_REF2047_Margin.Text);
            double REF2047_Resolution = 0.04;
            double REF2047_Limit_Lv = Convert.ToDouble(textBox_Black_Limit_Lv.Text);
            bool Black_OC_OK = REF2047_Compensation(REF2047_Margin, REF2047_Resolution, REF2047_Limit_Lv, model);

            //AM0 Compensation
            double Margin_R = Convert.ToDouble(textBox_AM0_R_Margin.Text);
            double Margin_G = Convert.ToDouble(textBox_AM0_G_Margin.Text);
            double Margin_B = Convert.ToDouble(textBox_AM0_B_Margin.Text);
            if (Black_OC_OK) Black_OC_OK = AM0_Compensation(Margin_R, Margin_G, Margin_B);
            return Black_OC_OK;
        }

        private void Set_AM0(string New_HBM_AM0_Hex_R, string New_HBM_AM0_Hex_G, string New_HBM_AM0_Hex_B)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP173_One_Param_CMD_Send(5, "B3", New_HBM_AM0_Hex_R);
            f1.DP173_One_Param_CMD_Send(18, "B3", New_HBM_AM0_Hex_G);
            f1.DP173_One_Param_CMD_Send(31, "B3", New_HBM_AM0_Hex_B);
        }

        private void Band_Set_AM0(int Band, string New_HBM_AM0_Hex_R, string New_HBM_AM0_Hex_G, string New_HBM_AM0_Hex_B)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string Band_Address = DP173.Get_Gamma_Register_Hex_String(Band).Remove(0, 2); //"XX"
            f1.DP173_One_Param_CMD_Send(5, Band_Address, New_HBM_AM0_Hex_R);
            f1.DP173_One_Param_CMD_Send(18, Band_Address, New_HBM_AM0_Hex_G);
            f1.DP173_One_Param_CMD_Send(31, Band_Address, New_HBM_AM0_Hex_B);
        }

        private void button_Read_AM0_VREF2_Click(object sender, EventArgs e)
        {
            //Show_Band_Set_RGB_AM0_Voltage(int band, int set, double Vreg1_voltage, double VGEG1_REF2047)
            button_Vreg1_Read.PerformClick();
            double VREG1_REF2047_Voltage = Convert.ToDouble(VREG1_REF2047.Text);
            //Set1 (Normal)
            Gamma_Set Set = Gamma_Set.Set1;
            Show_Band_Set_RGB_AM0_Voltage(0, Set, Convert.ToDouble(textBox_Vreg1_B0_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(1, Set, Convert.ToDouble(textBox_Vreg1_B1_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(2, Set, Convert.ToDouble(textBox_Vreg1_B2_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(3, Set, Convert.ToDouble(textBox_Vreg1_B3_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(4, Set, Convert.ToDouble(textBox_Vreg1_B4_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(5, Set, Convert.ToDouble(textBox_Vreg1_B5_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(6, Set, Convert.ToDouble(textBox_Vreg1_B6_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(7, Set, Convert.ToDouble(textBox_Vreg1_B7_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(8, Set, Convert.ToDouble(textBox_Vreg1_B8_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(9, Set, Convert.ToDouble(textBox_Vreg1_B9_1_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(10, Set, Convert.ToDouble(textBox_Vreg1_B10_1_volt.Text), VREG1_REF2047_Voltage);

            //Set2 (Normal)
            Set = Gamma_Set.Set2;
            Show_Band_Set_RGB_AM0_Voltage(0, Set, Convert.ToDouble(textBox_Vreg1_B0_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(1, Set, Convert.ToDouble(textBox_Vreg1_B1_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(2, Set, Convert.ToDouble(textBox_Vreg1_B2_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(3, Set, Convert.ToDouble(textBox_Vreg1_B3_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(4, Set, Convert.ToDouble(textBox_Vreg1_B4_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(5, Set, Convert.ToDouble(textBox_Vreg1_B5_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(6, Set, Convert.ToDouble(textBox_Vreg1_B6_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(7, Set, Convert.ToDouble(textBox_Vreg1_B7_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(8, Set, Convert.ToDouble(textBox_Vreg1_B8_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(9, Set, Convert.ToDouble(textBox_Vreg1_B9_2_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(10, Set, Convert.ToDouble(textBox_Vreg1_B10_2_volt.Text), VREG1_REF2047_Voltage);

            //Set3 (Normal)
            Set = Gamma_Set.Set3;
            Show_Band_Set_RGB_AM0_Voltage(0, Set, Convert.ToDouble(textBox_Vreg1_B0_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(1, Set, Convert.ToDouble(textBox_Vreg1_B1_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(2, Set, Convert.ToDouble(textBox_Vreg1_B2_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(3, Set, Convert.ToDouble(textBox_Vreg1_B3_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(4, Set, Convert.ToDouble(textBox_Vreg1_B4_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(5, Set, Convert.ToDouble(textBox_Vreg1_B5_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(6, Set, Convert.ToDouble(textBox_Vreg1_B6_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(7, Set, Convert.ToDouble(textBox_Vreg1_B7_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(8, Set, Convert.ToDouble(textBox_Vreg1_B8_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(9, Set, Convert.ToDouble(textBox_Vreg1_B9_3_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(10, Set, Convert.ToDouble(textBox_Vreg1_B10_3_volt.Text), VREG1_REF2047_Voltage);

            //Set4 (Normal)
            Set = Gamma_Set.Set4;
            Show_Band_Set_RGB_AM0_Voltage(0, Set, Convert.ToDouble(textBox_Vreg1_B0_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(1, Set, Convert.ToDouble(textBox_Vreg1_B1_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(2, Set, Convert.ToDouble(textBox_Vreg1_B2_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(3, Set, Convert.ToDouble(textBox_Vreg1_B3_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(4, Set, Convert.ToDouble(textBox_Vreg1_B4_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(5, Set, Convert.ToDouble(textBox_Vreg1_B5_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(6, Set, Convert.ToDouble(textBox_Vreg1_B6_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(7, Set, Convert.ToDouble(textBox_Vreg1_B7_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(8, Set, Convert.ToDouble(textBox_Vreg1_B8_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(9, Set, Convert.ToDouble(textBox_Vreg1_B9_4_volt.Text), VREG1_REF2047_Voltage);
            Show_Band_Set_RGB_AM0_Voltage(10, Set, Convert.ToDouble(textBox_Vreg1_B10_4_volt.Text), VREG1_REF2047_Voltage);
        }



        private void Get_SET1_HBM_AM1_Hex(ref string HBM_AM1_Hex_R, ref string HBM_AM1_Hex_G, ref string HBM_AM1_Hex_B)
        {
            //Get Set1 HBM
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(0, 40, "B3"); Thread.Sleep(50);
            string[] Hex = new string[40];
            for (int i = 0; i < 40; i++) Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            //HBM_AM0_Hex_R = f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
            //HBM_AM0_Hex_G = f1.dataGridView1.Rows[18].Cells[1].Value.ToString();
            //HBM_AM0_Hex_B = f1.dataGridView1.Rows[31].Cells[1].Value.ToString();
            HBM_AM1_Hex_R = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();
            HBM_AM1_Hex_G = f1.dataGridView1.Rows[17].Cells[1].Value.ToString();
            HBM_AM1_Hex_B = f1.dataGridView1.Rows[30].Cells[1].Value.ToString();
        }


        private void Get_SET1_AM0_AM1_Hex(string Band_Address, int Offset, ref string HBM_AM0_Hex_R, ref string HBM_AM0_Hex_G, ref string HBM_AM0_Hex_B, ref string HBM_AM1_Hex_R, ref string HBM_AM1_Hex_G, ref string HBM_AM1_Hex_B)
        {
            //Get Set1 HBM
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(Offset, 40, Band_Address); Thread.Sleep(50);
            string[] Hex = new string[40];
            for (int i = 0; i < 40; i++) Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            HBM_AM0_Hex_R = f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
            HBM_AM0_Hex_G = f1.dataGridView1.Rows[18].Cells[1].Value.ToString();
            HBM_AM0_Hex_B = f1.dataGridView1.Rows[31].Cells[1].Value.ToString();
            HBM_AM1_Hex_R = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();
            HBM_AM1_Hex_G = f1.dataGridView1.Rows[17].Cells[1].Value.ToString();
            HBM_AM1_Hex_B = f1.dataGridView1.Rows[30].Cells[1].Value.ToString();





        }

        private void button_Read_AM0_VREF2_HBM_Set1_Only_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(21, 7, "B1"); ;
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)

            int Dec_VREG1_REF1 = Convert.ToInt32(Hex_VREG1_REF1, 16) & 0x7F;
            int Dec_VREG1_REF2047 = Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F;

            double Voltage_VREG1_REF1 = 0.2 + (0.04 * Dec_VREG1_REF1); //Min 0.2v , Max 5.28
            double Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v
            double AM0_Resolution = (Voltage_VREG1_REF2047 - Voltage_VREG1_REF1) / 700;

            textBox_REF2047.Text = Voltage_VREG1_REF2047.ToString();
            textBox_AM0_Resolution.Text = AM0_Resolution.ToString();

            double Dll_AM0_Resolution = Imported_my_cpp_dll.Get_DP173_EA9154_AM0_Resolution(Hex_VREG1_REF1, Hex_VREG1_REF2047);

            f1.GB_Status_AppendText_Nextline("AM0_Resolution / Dll_AM0_Resolution : " + AM0_Resolution.ToString() + "/" + Dll_AM0_Resolution.ToString(), Color.Blue);

            //Set1 HBM
            f1.MX_OTP_Read(0, 40, "B3");
            string[] Hex = new string[40];
            for (int i = 0; i < 40; i++) Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            string HBM_AM0_Hex_R = f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
            string HBM_AM0_Hex_G = f1.dataGridView1.Rows[18].Cells[1].Value.ToString();
            string HBM_AM0_Hex_B = f1.dataGridView1.Rows[31].Cells[1].Value.ToString();
            int Dec_AM_R = Convert.ToInt32(HBM_AM0_Hex_R, 16); if (Dec_AM_R > 127) Dec_AM_R = 127;
            int Dec_AM_G = Convert.ToInt32(HBM_AM0_Hex_G, 16); if (Dec_AM_G > 127) Dec_AM_G = 127;
            int Dec_AM_B = Convert.ToInt32(HBM_AM0_Hex_B, 16); if (Dec_AM_B > 127) Dec_AM_B = 127;
            double Voltage_AM0_R = Voltage_VREG1_REF2047 - AM0_Resolution * Dec_AM_R;
            double Voltage_AM0_G = Voltage_VREG1_REF2047 - AM0_Resolution * Dec_AM_G;
            double Voltage_AM0_B = Voltage_VREG1_REF2047 - AM0_Resolution * Dec_AM_B;
            textBox_R_AM0.Text = Voltage_AM0_R.ToString();
            textBox_G_AM0.Text = Voltage_AM0_G.ToString();
            textBox_B_AM0.Text = Voltage_AM0_B.ToString();

            f1.GB_Status_AppendText_Nextline("Read)Voltage_VREG1_REF2047 : " + Voltage_VREG1_REF2047.ToString(), Color.Black);
            f1.GB_Status_AppendText_Nextline("Read)AM0_Resolution : " + AM0_Resolution.ToString(), Color.Black);
            f1.GB_Status_AppendText_Nextline("Read)Voltage_AM0_R : " + Voltage_AM0_R.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Read)Voltage_AM0_G : " + Voltage_AM0_G.ToString(), Color.Green);
            f1.GB_Status_AppendText_Nextline("Read)Voltage_AM0_B : " + Voltage_AM0_B.ToString(), Color.Blue);
        }

        private void button_B0_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B0_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band0 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B1_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B1_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band1 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B2_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B2_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band2 DBV Setting", System.Drawing.Color.Black);
        }


        public void button_B3_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B3_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band3 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B4_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B4_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band4 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B5_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B5_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band5 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B6_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B6_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band6 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B7_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B7_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band7 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B8_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B8_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band8 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B9_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B9_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band9 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B10_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B10_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band10 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_A0_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_AOD0_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("AOD0 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_A1_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_AOD1_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("AOD1 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_A2_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_AOD2_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("AOD2 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_R_AM0_Test_Click(object sender, EventArgs e)
        {
            Optic_Compensation_Stop = false;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting("7FF");//HBM
            f1.IPC_Quick_Send("image.black");//Black Pattern

            f1.MX_OTP_Read(21, 7, "B1");
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)

            int Dec_VREG1_REF1 = Convert.ToInt32(Hex_VREG1_REF1, 16) & 0x7F;
            int Dec_VREG1_REF2047 = Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F;

            double Voltage_VREG1_REF1 = 0.2 + (0.04 * Dec_VREG1_REF1); //Min 0.2v , Max 5.28
            double Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v
            double AM0_Resolution = (Voltage_VREG1_REF2047 - Voltage_VREG1_REF1) / 700;

            f1.MX_OTP_Read(0, 40, "B3");
            string[] B3_Hex = new string[40];
            for (int i = 0; i < 40; i++) B3_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            double Voltage_AM0_R = 0;
            for (int Dec_AM0_R = 0; Dec_AM0_R < 128; Dec_AM0_R++)
            {
                if (Optic_Compensation_Stop) break;

                B3_Hex[5] = Dec_AM0_R.ToString("X2");//R
                B3_Hex[18] = "00";//G
                B3_Hex[31] = "00";//B
                f1.Long_Packet_CMD_Send(40, "B3", B3_Hex);
                Thread.Sleep(50);
                f1.Measure_Indicate_Gray(Dec_AM0_R);

                Voltage_AM0_R = Voltage_VREG1_REF2047 - AM0_Resolution * Dec_AM0_R;
                f1.GB_Status_AppendText_Nextline("Voltage_AM0_R/Dec_AM0_R : " + Voltage_AM0_R.ToString("0.0000") + "/(" + Dec_AM0_R.ToString() + "/0/0)", Color.Red);
            }
        }

        private void button_G_AM0_Test_Click(object sender, EventArgs e)
        {
            Optic_Compensation_Stop = false;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting("7FF");//HBM
            f1.IPC_Quick_Send("image.black");//Black Pattern

            f1.MX_OTP_Read(21, 7, "B1");
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)

            int Dec_VREG1_REF1 = Convert.ToInt32(Hex_VREG1_REF1, 16) & 0x7F;
            int Dec_VREG1_REF2047 = Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F;

            double Voltage_VREG1_REF1 = 0.2 + (0.04 * Dec_VREG1_REF1); //Min 0.2v , Max 5.28
            double Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v
            double AM0_Resolution = (Voltage_VREG1_REF2047 - Voltage_VREG1_REF1) / 700;

            f1.MX_OTP_Read(0, 40, "B3");
            string[] B3_Hex = new string[40];
            for (int i = 0; i < 40; i++) B3_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            double Voltage_AM0_G = 0;
            for (int Dec_AM0_G = 0; Dec_AM0_G < 128; Dec_AM0_G++)
            {
                if (Optic_Compensation_Stop) break;

                B3_Hex[5] = "00";//R
                B3_Hex[18] = Dec_AM0_G.ToString("X2");//G
                B3_Hex[31] = "00";//B
                f1.Long_Packet_CMD_Send(40, "B3", B3_Hex);
                Thread.Sleep(50);
                f1.Measure_Indicate_Gray(Dec_AM0_G);

                Voltage_AM0_G = Voltage_VREG1_REF2047 - AM0_Resolution * Dec_AM0_G;
                f1.GB_Status_AppendText_Nextline("Voltage_AM0_G/(Dec_AM0_R/Dec_AM0_G/Dec_AM0_B) : " + Voltage_AM0_G.ToString("0.0000") + "/(0/" + Dec_AM0_G.ToString() + "/0)", Color.Green);
            }
        }

        private void button_B_AM0_Test_Click(object sender, EventArgs e)
        {
            Optic_Compensation_Stop = false;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting("7FF");//HBM
            f1.IPC_Quick_Send("image.black");//Black Pattern

            f1.MX_OTP_Read(21, 7, "B1");
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)

            int Dec_VREG1_REF1 = Convert.ToInt32(Hex_VREG1_REF1, 16) & 0x7F;
            int Dec_VREG1_REF2047 = Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F;

            double Voltage_VREG1_REF1 = 0.2 + (0.04 * Dec_VREG1_REF1); //Min 0.2v , Max 5.28
            double Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v
            double AM0_Resolution = (Voltage_VREG1_REF2047 - Voltage_VREG1_REF1) / 700;

            f1.MX_OTP_Read(0, 40, "B3");
            string[] B3_Hex = new string[40];
            for (int i = 0; i < 40; i++) B3_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            double Voltage_AM0_B = 0;
            for (int Dec_AM0_B = 0; Dec_AM0_B < 128; Dec_AM0_B++)
            {
                if (Optic_Compensation_Stop) break;

                B3_Hex[5] = "00";//R
                B3_Hex[18] = "00";//G
                B3_Hex[31] = Dec_AM0_B.ToString("X2");//B
                f1.Long_Packet_CMD_Send(40, "B3", B3_Hex);
                Thread.Sleep(50);
                f1.Measure_Indicate_Gray(Dec_AM0_B);

                Voltage_AM0_B = Voltage_VREG1_REF2047 - AM0_Resolution * Dec_AM0_B;
                f1.GB_Status_AppendText_Nextline("Voltage_AM0_B/(Dec_AM0_R/Dec_AM0_G/Dec_AM0_B) : " + Voltage_AM0_B.ToString("0.0000") + "(0/0/" + Dec_AM0_B.ToString() + ")", Color.Blue);
            }

        }

        private void button_BSQH_Stop_Click(object sender, EventArgs e)
        {
            Optic_Compensation_Stop = true;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.OC_Timer_Stop();
        }

        private void button_Read_DP173_DBV_Setting_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                f1.MX_OTP_Read(0, 15, "B1");
                Thread.Sleep(200);

                textBox_B0_DBV_Setting.Text = "7FF";
                textBox_B1_DBV_Setting.Text = f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
                textBox_B2_DBV_Setting.Text = f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString();
                textBox_B3_DBV_Setting.Text = f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString();
                textBox_B4_DBV_Setting.Text = f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString();
                textBox_B5_DBV_Setting.Text = f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString();
                textBox_B6_DBV_Setting.Text = f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString();
                textBox_B7_DBV_Setting.Text = f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString();
                textBox_B8_DBV_Setting.Text = f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString();
                textBox_B9_DBV_Setting.Text = f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString();
                textBox_B10_DBV_Setting.Text = f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString();

                //Read AOD1~3 DBV
                f1.MX_OTP_Read(0, 8, "B2");
                Thread.Sleep(200);
                textBox_AOD0_DBV_Setting.Text = "7FF";
                textBox_AOD1_DBV_Setting.Text = f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[4].Cells[1].Value.ToString();
                textBox_AOD2_DBV_Setting.Text = f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
                f1.GB_Status_AppendText_Nextline("DP173 DBV value was loaded from register", System.Drawing.Color.Black);

                Application.DoEvents();
            }
            catch
            {
                System.Windows.MessageBox.Show("DBV Value Read fail");
            }
        }

        private void groupBox35_Enter(object sender, EventArgs e)
        {

        }

        public bool Get_IS_G2G_On()
        {
            if (radioButton_G2G_On.Checked) return true;
            else return false;
        }

        private void button_Dll_Info_Click(object sender, EventArgs e)
        {
            IntPtr ptr = Imported_my_cpp_dll.Get_Dll_Information();
            string Message = Marshal.PtrToStringAnsi(ptr);
            System.Windows.MessageBox.Show(Message);
        }


        private void Set_Condition_Mipi_Script_Change(Gamma_Set Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string temp_Mipi_Data_String = string.Empty;
            int count_mipi_cmd = 0;
            int count_one_mipi_cmd_length = 0;
            bool Flag = false;

            TextBox temp_textBox = new TextBox();
            TextBox textBox_Show_Compared_Mipi_Data = new TextBox();
            if (Set == Gamma_Set.Set1)
            {
                temp_textBox = textBox_Mipi_Script_Set1;
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set1;
            }
            else if (Set == Gamma_Set.Set2)
            {
                temp_textBox = textBox_Mipi_Script_Set2;
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set2;
            }
            else if (Set == Gamma_Set.Set3)
            {
                temp_textBox = textBox_Mipi_Script_Set3;
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set3;
            }
            else if (Set == Gamma_Set.Set4)
            {
                temp_textBox = textBox_Mipi_Script_Set4;
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set4;
            }
            else if (Set == Gamma_Set.Set5)
            {
                temp_textBox = textBox_Mipi_Script_Set5;
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set5;
            }
            else if (Set == Gamma_Set.Set6)
            {
                temp_textBox = textBox_Mipi_Script_Set6;
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set6;
            }

            textBox_Show_Compared_Mipi_Data.Clear();

            //Delete others except for Mipi CMDs and Write on the 2nd Textbox
            for (int i = 0; i < temp_textBox.Lines.Length; i++)
            {
                if (temp_textBox.Lines[i].Length >= 20) // mipi.write 0xXX 0xXX <-- 20ea Character
                {
                    if (temp_textBox.Lines[i].Substring(0, 10) == "mipi.write")
                    {
                        count_mipi_cmd++;

                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 10; k < temp_textBox.Lines[i].Length; k++)
                        {
                            if (temp_textBox.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && temp_textBox.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 10 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else
                    {
                        // It's not a "mipi.write" of "delay" command , do nothing 
                    }
                }

                //Delay
                else if (temp_textBox.Lines[i].Length >= 5
                    && temp_textBox.Lines[i].Substring(0, 5) != "     ")
                {
                    if (temp_textBox.Lines[i].Substring(0, 5) == "delay")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < temp_textBox.Lines[i].Length; k++)
                        {
                            if (temp_textBox.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && temp_textBox.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }

                    else if (temp_textBox.Lines[i].Substring(0, 5) == "image")
                    {
                        count_mipi_cmd++;
                        count_one_mipi_cmd_length = 0;
                        Flag = false;
                        for (int k = 5; k < temp_textBox.Lines[i].Length; k++)
                        {
                            if (temp_textBox.Lines[i][k] != '#') //주석이 없으면
                            {
                                count_one_mipi_cmd_length++;
                            }
                            else //<-- 주석이 나타나면 그만 Count
                            {
                                Flag = true;
                                break;
                            }
                        }
                        if (Flag && temp_textBox.Lines[i][count_one_mipi_cmd_length] != ' ')
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else if (Flag == false)
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length) + " ";
                        }
                        else
                        {
                            temp_Mipi_Data_String = temp_textBox.Lines[i].Substring(0, 5 + count_one_mipi_cmd_length);
                        }

                        temp_Mipi_Data_String = System.Text.RegularExpressions.Regex.Replace(temp_Mipi_Data_String, @"\s+", " ");
                        textBox_Show_Compared_Mipi_Data.Text += temp_Mipi_Data_String + "\r\n";
                    }

                }
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void Set_Condition_Mipi_Script_Send(Gamma_Set Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            TextBox textBox_Show_Compared_Mipi_Data = new TextBox();
            if (Set == Gamma_Set.Set1)
            {
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set1;
                f1.GB_Status_AppendText_Nextline("Set1 Applied", Status_Color_Set1);
            }
            else if (Set == Gamma_Set.Set2)
            {
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set2;
                f1.GB_Status_AppendText_Nextline("Set2 Applied", Status_Color_Set2);
            }
            else if (Set == Gamma_Set.Set3)
            {
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set3;
                f1.GB_Status_AppendText_Nextline("Set3 Applied", Status_Color_Set3);
            }
            else if (Set == Gamma_Set.Set4)
            {
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set4;
                f1.GB_Status_AppendText_Nextline("Set4 Applied", Status_Color_Set4);
            }
            else if (Set == Gamma_Set.Set5)
            {
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set5;
                f1.GB_Status_AppendText_Nextline("Set5 Applied", Status_Color_Set5);
            }
            else if (Set == Gamma_Set.Set6)
            {
                textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set6;
                f1.GB_Status_AppendText_Nextline("Set6 Applied", Status_Color_Set6);
            }

            //Send "mipi.write" of "delay" command
            for (int i = 0; i < textBox_Show_Compared_Mipi_Data.Lines.Length - 1; i++)
            {
                System.Windows.Forms.Application.DoEvents();

                if (textBox_Show_Compared_Mipi_Data.Lines[i].Length >= 10
                    && textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 10) == "mipi.write")
                {
                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data.Lines[i]);
                }
                else if (textBox_Show_Compared_Mipi_Data.Lines[i].Length >= 5 && (
                    textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5) == "delay"
                    || textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5) == "image"))
                {
                    f1.IPC_Quick_Send(textBox_Show_Compared_Mipi_Data.Lines[i]);
                }
                else
                {
                    // It's not a "mipi.write" command , do nothing 
                }
            }
        }




        private void All_Band_Checkbox_Status(bool Checked)
        {
            checkBox_Band0.Checked = Checked;
            checkBox_Band1.Checked = Checked;
            checkBox_Band2.Checked = Checked;
            checkBox_Band3.Checked = Checked;
            checkBox_Band4.Checked = Checked;
            checkBox_Band5.Checked = Checked;
            checkBox_Band6.Checked = Checked;
            checkBox_Band7.Checked = Checked;
            checkBox_Band8.Checked = Checked;
            checkBox_Band9.Checked = Checked;
            checkBox_Band10.Checked = Checked;
            checkBox_AOD0.Checked = Checked;
            checkBox_AOD1.Checked = Checked;
            checkBox_AOD2.Checked = Checked;
        }

        private void button_Select_All_Band_Click(object sender, EventArgs e)
        {
            All_Band_Checkbox_Status(true);
        }

        private void button_Deselect_All_Band_Click(object sender, EventArgs e)
        {
            All_Band_Checkbox_Status(false);
        }

        private void checkBox_Ave_Measure_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_Dual_Mode_Copy_Mea_To_Target_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox_Dual_Mode_Set3_OC_Skip_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Dual_Mode_Set3_OC_Skip.Checked)
            {
                groupBox5.Visible = false;
            }
            else
            {
                groupBox5.Visible = true;
            }
        }

        private void checkBox_Dual_Mode_Set4_OC_Skip_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView_RGB_Vdata_Write_KeyDown(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (e.KeyData == (Keys.Control | Keys.V)) f1.PasteInData(ref this.dataGridView_RGB_Vdata_Write);
        }



        public void RGB_Vdata_Grid_Initalize()
        {
            for (int k = 0; k < 7; k++)
            {
                if (dataGridView_RGB_Vdata_Read.ColumnCount < 7) dataGridView_RGB_Vdata_Read.Columns.Add("", "");
                if (dataGridView_RGB_Vdata_Write.ColumnCount < 4) dataGridView_RGB_Vdata_Write.Columns.Add("", "");
            }

            dataGridView_RGB_Vdata_Read.Rows.Add("-", "GM_R", "GM_G", "GM_B", "R(v)", "G(v)", "B(v)");
            dataGridView_RGB_Vdata_Read.Rows.Add("G255", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G191", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G127", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G63", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G31", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G15", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G7", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G4", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G1", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("AM1", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("AM0", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("-", "Dec", "-", "-", "Volt", "-", "-");
            dataGridView_RGB_Vdata_Read.Rows.Add("REF2047", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("REF1635", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("REF1227", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("REF815", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("REF407", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("REF63", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("REF1", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("Vreg1", "", "", "", "", "", "");

            dataGridView_RGB_Vdata_Write.Rows.Add("-", "GM_R", "GM_G", "GM_B");
            dataGridView_RGB_Vdata_Write.Rows.Add("G255", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G191", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G127", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G63", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G31", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G15", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G7", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G4", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G1", "", "", "");

            RGB_Vdata_Grid_Tema_Change();

            foreach (DataGridViewColumn column in this.dataGridView_RGB_Vdata_Write.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
            foreach (DataGridViewColumn column in this.dataGridView_RGB_Vdata_Read.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }







        public void RGB_Vdata_Grid_Tema_Change()
        {
            dataGridView_RGB_Vdata_Write.Columns[0].Width = 40;
            dataGridView_RGB_Vdata_Read.Columns[0].Width = 40;

            for (int i = 1; i <= 3; i++) //Gamma R,G,B
            {
                dataGridView_RGB_Vdata_Write.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_RGB_Vdata_Write.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_RGB_Vdata_Write.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_RGB_Vdata_Write.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_RGB_Vdata_Write.Columns[i].Width = 55;

                dataGridView_RGB_Vdata_Read.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_RGB_Vdata_Read.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_RGB_Vdata_Read.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_RGB_Vdata_Read.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_RGB_Vdata_Read.Columns[i].Width = 55;
            }

            for (int i = 4; i <= 6; i++) //Measure X,Y,Lv
            {
                dataGridView_RGB_Vdata_Read.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dataGridView_RGB_Vdata_Read.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_RGB_Vdata_Read.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_RGB_Vdata_Read.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_RGB_Vdata_Read.Columns[i].Width = 55;

            }
        }

        private string[] Get_RGB_Hex_Param(RGB[] Selected_Band_gray_Gamma, string R_AM0_Hex, string G_AM0_Hex, string B_AM0_Hex, string R_AM1_Hex, string G_AM1_Hex, string B_AM1_Hex)
        {
            RGB[] Gamma_9th_data = new RGB[9]; //G255
            RGB[] Gamma_8ea_data = new RGB[9]; //GXXX < G255
            string[] Hex_Param = new string[40];
            for (int gray = 0; gray < 9; gray++)
            {
                Gamma_9th_data[gray].Set_Value(Selected_Band_gray_Gamma[gray].int_R >> 8, Selected_Band_gray_Gamma[gray].int_G >> 8, Selected_Band_gray_Gamma[gray].int_B >> 8);
                Gamma_8ea_data[gray].Set_Value(Selected_Band_gray_Gamma[gray].int_R & 0xFF, Selected_Band_gray_Gamma[gray].int_G & 0xFF, Selected_Band_gray_Gamma[gray].int_B & 0xFF);
            }

            Hex_Param[0] = ((Gamma_9th_data[0].int_R << 4) + (Gamma_9th_data[0].int_G << 2) + (Gamma_9th_data[0].int_B)).ToString("X2");//G255 RGB 9th bit

            Hex_Param[1] = ((Gamma_9th_data[1].int_R << 7) + (Gamma_9th_data[2].int_R << 6)
                + (Gamma_9th_data[3].int_R << 5) + (Gamma_9th_data[4].int_R << 4)
                + (Gamma_9th_data[5].int_R << 3) + (Gamma_9th_data[6].int_R << 2)
                + (Gamma_9th_data[7].int_R << 1) + Gamma_9th_data[8].int_R).ToString("X2");//GXX < G255 R 9th

            Hex_Param[2] = "00";
            Hex_Param[3] = Gamma_8ea_data[0].int_R.ToString("X2"); //G255[7:0]
            Hex_Param[4] = R_AM1_Hex;
            Hex_Param[5] = R_AM0_Hex;
            Hex_Param[6] = Gamma_8ea_data[1].int_R.ToString("X2"); //G191[7:0]
            Hex_Param[7] = Gamma_8ea_data[2].int_R.ToString("X2"); //G127[7:0]
            Hex_Param[8] = Gamma_8ea_data[3].int_R.ToString("X2"); //G63[7:0]
            Hex_Param[9] = Gamma_8ea_data[4].int_R.ToString("X2"); //G31[7:0]
            Hex_Param[10] = Gamma_8ea_data[5].int_R.ToString("X2"); //G15[7:0]
            Hex_Param[11] = Gamma_8ea_data[6].int_R.ToString("X2"); //G7[7:0]
            Hex_Param[12] = Gamma_8ea_data[7].int_R.ToString("X2");//G4[7:0]
            Hex_Param[13] = Gamma_8ea_data[8].int_R.ToString("X2");//G1[7:0]



            Hex_Param[14] = ((Gamma_9th_data[1].int_G << 7) + (Gamma_9th_data[2].int_G << 6)
                + (Gamma_9th_data[3].int_G << 5) + (Gamma_9th_data[4].int_G << 4)
                + (Gamma_9th_data[5].int_G << 3) + (Gamma_9th_data[6].int_G << 2)
                + (Gamma_9th_data[7].int_G << 1) + Gamma_9th_data[8].int_G).ToString("X2"); //GXX < G255 G 9th
            Hex_Param[15] = "00";
            Hex_Param[16] = Gamma_8ea_data[0].int_G.ToString("X2"); //G255[7:0]
            Hex_Param[17] = G_AM1_Hex;
            Hex_Param[18] = G_AM0_Hex;
            Hex_Param[19] = Gamma_8ea_data[1].int_G.ToString("X2"); //G191[7:0]
            Hex_Param[20] = Gamma_8ea_data[2].int_G.ToString("X2"); //G127[7:0]
            Hex_Param[21] = Gamma_8ea_data[3].int_G.ToString("X2"); //G63[7:0]
            Hex_Param[22] = Gamma_8ea_data[4].int_G.ToString("X2"); //G31[7:0]
            Hex_Param[23] = Gamma_8ea_data[5].int_G.ToString("X2"); //G15[7:0]
            Hex_Param[24] = Gamma_8ea_data[6].int_G.ToString("X2"); //G7[7:0]
            Hex_Param[25] = Gamma_8ea_data[7].int_G.ToString("X2");//G4[7:0]
            Hex_Param[26] = Gamma_8ea_data[8].int_G.ToString("X2");//G1[7:0]


            Hex_Param[27] = ((Gamma_9th_data[1].int_B << 7) + (Gamma_9th_data[2].int_B << 6)
                + (Gamma_9th_data[3].int_B << 5) + (Gamma_9th_data[4].int_B << 4)
                + (Gamma_9th_data[5].int_B << 3) + (Gamma_9th_data[6].int_B << 2)
                + (Gamma_9th_data[7].int_B << 1) + Gamma_9th_data[8].int_B).ToString("X2"); //GXX < G255 B 9th
            Hex_Param[28] = "00";
            Hex_Param[29] = Gamma_8ea_data[0].int_B.ToString("X2"); //G255[7:0]
            Hex_Param[30] = B_AM1_Hex;
            Hex_Param[31] = B_AM0_Hex;
            Hex_Param[32] = Gamma_8ea_data[1].int_B.ToString("X2"); //G191[7:0]
            Hex_Param[33] = Gamma_8ea_data[2].int_B.ToString("X2"); //G127[7:0]
            Hex_Param[34] = Gamma_8ea_data[3].int_B.ToString("X2"); //G63[7:0]
            Hex_Param[35] = Gamma_8ea_data[4].int_B.ToString("X2"); //G31[7:0]
            Hex_Param[36] = Gamma_8ea_data[5].int_B.ToString("X2"); //G15[7:0]
            Hex_Param[37] = Gamma_8ea_data[6].int_B.ToString("X2"); //G7[7:0]
            Hex_Param[38] = Gamma_8ea_data[7].int_B.ToString("X2");//G4[7:0]
            Hex_Param[39] = Gamma_8ea_data[8].int_B.ToString("X2");//G1[7:0]

            return Hex_Param;
        }

        private void button_RGB_Vdata_Write_Click(object sender, EventArgs e)
        {
            while (true)
            {
                if (dataGridView_RGB_Vdata_Write.Rows.Count > 11) dataGridView_RGB_Vdata_Write.Rows.RemoveAt(dataGridView_RGB_Vdata_Write.Rows.Count - 2);
                else break;
            }

            for (int k = 0; k < 4; k++) dataGridView_RGB_Vdata_Write.Rows[dataGridView_RGB_Vdata_Write.Rows.Count - 1].Cells[k].Value = "";

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            string Band_Address = DP173.Get_Gamma_Register_Hex_String(Band).Remove(0, 2); //"XX"
            Gamma_Set Set = Get_Selected_Set_Of_RGB_Vdata_Read_Write();
            f1.GB_Status_AppendText_Nextline("Set" + Set.ToString() + "/Band" + Band.ToString() + " RGB Writing Start", Color.Red);
            Application.DoEvents();

            int Offset = 0;
            if (Set == Gamma_Set.Set1 && Band < 11) Offset = 0;//Set1 Normal
            else if (Set == Gamma_Set.Set2 && Band < 11) Offset = 40;//Set2 Normal
            else if (Set == Gamma_Set.Set3 && Band < 11) Offset = 80;//Set3 Normal
            else if (Set == Gamma_Set.Set4 && Band < 11) Offset = 120;//Set4 Normal
            else if (Set == Gamma_Set.Set5 && Band < 11) Offset = 160;//Set5 Normal
            else if (Set == Gamma_Set.Set6 && Band < 11) Offset = 200;//Set6 Normal
            else { MessageBox.Show("Band > 11, which is out of options"); }

            //Get AM0 R/G/B
            string R_AM0_Hex = string.Empty;
            string G_AM0_Hex = string.Empty;
            string B_AM0_Hex = string.Empty;

            //Get AM1 R/G/B
            string R_AM1_Hex = string.Empty;
            string G_AM1_Hex = string.Empty;
            string B_AM1_Hex = string.Empty;

            //f1.DP173_Long_Packet_CMD_Send(Offset, Hex_Param.Length, Band_Address, Hex_Param);

            Get_SET1_AM0_AM1_Hex(Band_Address, Offset, ref R_AM0_Hex, ref G_AM0_Hex, ref B_AM0_Hex, ref R_AM1_Hex, ref G_AM1_Hex, ref B_AM1_Hex);


            RGB[] Selected_Band_gray_Gamma = new RGB[9]; //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)
            for (int gray = 0; gray < 9; gray++)
            {
                Selected_Band_gray_Gamma[gray].int_R = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[gray + 1].Cells[1].Value);//R
                Selected_Band_gray_Gamma[gray].int_G = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[gray + 1].Cells[2].Value);//G
                Selected_Band_gray_Gamma[gray].int_B = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[gray + 1].Cells[3].Value);//B
            }

            string[] Hex_Param = Get_RGB_Hex_Param(Selected_Band_gray_Gamma, R_AM0_Hex, G_AM0_Hex, B_AM0_Hex, R_AM1_Hex, G_AM1_Hex, B_AM1_Hex);

            f1.DP173_Long_Packet_CMD_Send(Offset, Hex_Param.Length, Band_Address, Hex_Param);
            f1.GB_Status_AppendText_Nextline("Set" + Set.ToString() + "/Band" + Band.ToString() + " RGB Writing Finished", Color.Red);
        }

        private int Get_Selected_Band_Of_RGB_Vdata_Read_Write()
        {
            if (radioButton_band0.Checked) return 0;
            else if (radioButton_band1.Checked) return 1;
            else if (radioButton_band2.Checked) return 2;
            else if (radioButton_band3.Checked) return 3;
            else if (radioButton_band4.Checked) return 4;
            else if (radioButton_band5.Checked) return 5;
            else if (radioButton_band6.Checked) return 6;
            else if (radioButton_band7.Checked) return 7;
            else if (radioButton_band8.Checked) return 8;
            else if (radioButton_band9.Checked) return 9;
            else if (radioButton_band10.Checked) return 10;
            else
            {
                MessageBox.Show("Impossible to get band");
                return 999;
            }
        }

        private Gamma_Set Get_Selected_Set_Of_RGB_Vdata_Read_Write()
        {
            if (radioButton_Set1.Checked) return Gamma_Set.Set1;
            else if (radioButton_Set2.Checked) return Gamma_Set.Set2;
            else if (radioButton_Set3.Checked) return Gamma_Set.Set3;
            else if (radioButton_Set4.Checked) return Gamma_Set.Set4;
            else if (radioButton_Set5.Checked) return Gamma_Set.Set5;
            else if (radioButton_Set6.Checked) return Gamma_Set.Set6;
            else
            {
                MessageBox.Show("Impossible to get Set");
                return Gamma_Set.Set6;
            }
        }



        private void Apply_Read_Result_to_OC_Param(int Band, Gamma_Set Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string Band_Address = DP173.Get_Gamma_Register_Hex_String(Band).Remove(0, 2); //"XX"
            int Offset = 0;
            if (Set == Gamma_Set.Set1 && Band < 11) Offset = 0;//Set1 Normal
            else if (Set == Gamma_Set.Set2 && Band < 11) Offset = 40;//Set2 Normal
            else if (Set == Gamma_Set.Set3 && Band < 11) Offset = 80;//Set3 Normal
            else if (Set == Gamma_Set.Set4 && Band < 11) Offset = 120;//Set4 Normal
            f1.MX_OTP_Read(Offset, 40, Band_Address);

            string[] Hex_Param = new string[40];
            int[] Dec_Param = new int[40];
            for (int i = 0; i < 40; i++)
            {
                Hex_Param[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Dec_Param[i] = Convert.ToInt32(Hex_Param[i], 16);
            }

            RGB[] Selected_Band_gray_Gamma = new RGB[9]; //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)
            //R 
            Selected_Band_gray_Gamma[0].int_R = ((Dec_Param[0] & 0x10) << 4) + Dec_Param[3];//AM2
            Selected_Band_gray_Gamma[1].int_R = ((Dec_Param[1] & 0x80) << 1) + Dec_Param[6];//GR7
            Selected_Band_gray_Gamma[2].int_R = ((Dec_Param[1] & 0x40) << 2) + Dec_Param[7];//GR6
            Selected_Band_gray_Gamma[3].int_R = ((Dec_Param[1] & 0x20) << 3) + Dec_Param[8];//GR5
            Selected_Band_gray_Gamma[4].int_R = ((Dec_Param[1] & 0x10) << 4) + Dec_Param[9];//GR4
            Selected_Band_gray_Gamma[5].int_R = ((Dec_Param[1] & 0x08) << 5) + Dec_Param[10];//GR3
            Selected_Band_gray_Gamma[6].int_R = ((Dec_Param[1] & 0x04) << 6) + Dec_Param[11];//GR2
            Selected_Band_gray_Gamma[7].int_R = ((Dec_Param[1] & 0x02) << 7) + Dec_Param[12];//GR1
            Selected_Band_gray_Gamma[8].int_R = ((Dec_Param[1] & 0x01) << 8) + Dec_Param[13];//GR0

            //G
            Selected_Band_gray_Gamma[0].int_G = ((Dec_Param[0] & 0x04) << 6) + Dec_Param[16];//AM2
            Selected_Band_gray_Gamma[1].int_G = ((Dec_Param[14] & 0x80) << 1) + Dec_Param[19];//GR7
            Selected_Band_gray_Gamma[2].int_G = ((Dec_Param[14] & 0x40) << 2) + Dec_Param[20];//GR6
            Selected_Band_gray_Gamma[3].int_G = ((Dec_Param[14] & 0x20) << 3) + Dec_Param[21];//GR5
            Selected_Band_gray_Gamma[4].int_G = ((Dec_Param[14] & 0x10) << 4) + Dec_Param[22];//GR4
            Selected_Band_gray_Gamma[5].int_G = ((Dec_Param[14] & 0x08) << 5) + Dec_Param[23];//GR3
            Selected_Band_gray_Gamma[6].int_G = ((Dec_Param[14] & 0x04) << 6) + Dec_Param[24];//GR2
            Selected_Band_gray_Gamma[7].int_G = ((Dec_Param[14] & 0x02) << 7) + Dec_Param[25];//GR1
            Selected_Band_gray_Gamma[8].int_G = ((Dec_Param[14] & 0x01) << 8) + Dec_Param[26];//GR0

            //B
            Selected_Band_gray_Gamma[0].int_B = ((Dec_Param[0] & 0x01) << 8) + Dec_Param[29]; //AM2
            Selected_Band_gray_Gamma[1].int_B = ((Dec_Param[27] & 0x80) << 1) + Dec_Param[32];//GR7
            Selected_Band_gray_Gamma[2].int_B = ((Dec_Param[27] & 0x40) << 2) + Dec_Param[33];//GR6
            Selected_Band_gray_Gamma[3].int_B = ((Dec_Param[27] & 0x20) << 3) + Dec_Param[34];//GR5
            Selected_Band_gray_Gamma[4].int_B = ((Dec_Param[27] & 0x10) << 4) + Dec_Param[35];//GR4
            Selected_Band_gray_Gamma[5].int_B = ((Dec_Param[27] & 0x08) << 5) + Dec_Param[36];//GR3
            Selected_Band_gray_Gamma[6].int_B = ((Dec_Param[27] & 0x04) << 6) + Dec_Param[37];//GR2
            Selected_Band_gray_Gamma[7].int_B = ((Dec_Param[27] & 0x02) << 7) + Dec_Param[38];//GR1
            Selected_Band_gray_Gamma[8].int_B = ((Dec_Param[27] & 0x01) << 8) + Dec_Param[39];//GR0

            DP173_Dual_Engineering_Mornitoring DP173_form_Dual_engineer = (DP173_Dual_Engineering_Mornitoring)Application.OpenForms["DP173_Dual_Engineering_Mornitoring"];
            int Prev_Offset_Row = 8 * (Band - 1);
            int Offset_Row = 8 * Band;

            DataGridView dataGridView_OC_param = DP173_form_Dual_engineer.Get_OC_Param_DataGridView(Set);

            for (int i = 0; i <= 8; i++)
            {
                dataGridView_OC_param.Rows[i + 2 + Offset_Row].Cells[1].Value = Selected_Band_gray_Gamma[i].int_R;
                dataGridView_OC_param.Rows[i + 2 + Offset_Row].Cells[2].Value = Selected_Band_gray_Gamma[i].int_G;
                dataGridView_OC_param.Rows[i + 2 + Offset_Row].Cells[3].Value = Selected_Band_gray_Gamma[i].int_B;
            }
        }

        private void Read_and_Update_Band_Set1234_OC_Param(int band)
        {
            Apply_Read_Result_to_OC_Param(band, Gamma_Set.Set1);
            Apply_Read_Result_to_OC_Param(band, Gamma_Set.Set2);
            Apply_Read_Result_to_OC_Param(band, Gamma_Set.Set3);
            Apply_Read_Result_to_OC_Param(band, Gamma_Set.Set4);
        }


        private void button_RGB_Vdata_Read_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            string Band_Address = DP173.Get_Gamma_Register_Hex_String(Band).Remove(0, 2); //"XX"
            Gamma_Set Set = Get_Selected_Set_Of_RGB_Vdata_Read_Write();
            f1.GB_Status_AppendText_Nextline("Set" + Set.ToString() + "/Band" + Band.ToString() + " RGB Read Start", Color.Blue);
            for (int i = 1; i < dataGridView_RGB_Vdata_Read.Rows.Count; i++)
            {
                if (i == 12) continue;

                dataGridView_RGB_Vdata_Read.Rows[i].Cells[1].Value = "";//R
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[2].Value = "";//G
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[3].Value = "";//B
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[4].Value = "";//R_Voltage
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[5].Value = "";//G_Voltage
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[6].Value = "";//B_Voltage
            }


            Application.DoEvents();

            int Offset = 0;
            if (Set == Gamma_Set.Set1 && Band < 11) Offset = 0;//Set1 Normal
            else if (Set == Gamma_Set.Set2 && Band < 11) Offset = 40;//Set2 Normal
            else if (Set == Gamma_Set.Set3 && Band < 11) Offset = 80;//Set3 Normal
            else if (Set == Gamma_Set.Set4 && Band < 11) Offset = 120;//Set4 Normal
            else if (Set == Gamma_Set.Set5 && Band < 11) Offset = 160;//Set5 Normal
            else if (Set == Gamma_Set.Set6 && Band < 11) Offset = 200;//Set6 Normal

            else { MessageBox.Show("Band > 11, which is out of options"); }

            string[] Hex_Param = new string[40];
            int[] Dec_Param = new int[40];
            f1.MX_OTP_Read(Offset, 40, Band_Address);
            for (int i = 0; i < 40; i++)
            {
                Hex_Param[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Dec_Param[i] = Convert.ToInt32(Hex_Param[i], 16);
            }

            //--------Added on 200324--------
            RGB AM0_Hex = new RGB();
            AM0_Hex.R = f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
            AM0_Hex.G = f1.dataGridView1.Rows[18].Cells[1].Value.ToString();
            AM0_Hex.B = f1.dataGridView1.Rows[31].Cells[1].Value.ToString();
            AM0_Hex.int_R = Convert.ToInt32(AM0_Hex.R, 16);
            AM0_Hex.int_G = Convert.ToInt32(AM0_Hex.G, 16);
            AM0_Hex.int_B = Convert.ToInt32(AM0_Hex.B, 16);

            RGB AM1_Hex = new RGB();
            AM1_Hex.R = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();
            AM1_Hex.G = f1.dataGridView1.Rows[17].Cells[1].Value.ToString();
            AM1_Hex.B = f1.dataGridView1.Rows[30].Cells[1].Value.ToString();
            AM1_Hex.int_R = Convert.ToInt32(AM1_Hex.R, 16);
            AM1_Hex.int_G = Convert.ToInt32(AM1_Hex.G, 16);
            AM1_Hex.int_B = Convert.ToInt32(AM1_Hex.B, 16);
            //------------------------------


            //HBM_AM1_Hex_R = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();
            //HBM_AM1_Hex_G = f1.dataGridView1.Rows[17].Cells[1].Value.ToString();
            //HBM_AM1_Hex_B = f1.dataGridView1.Rows[30].Cells[1].Value.ToString();

            //----RGB "9"th bit----- 
            //R : 0x10 = 10000(b) (R AM2)
            //G : 0x04 = 100(b) (G AM2)
            //B : 0x01 = 1(b) (B AM2)
            //0x80 = 1000 0000 (GR7)
            //0x40 = 0100 0000 (GR6)
            //0x20 = 0010 0000 (GR5)
            //0x10 = 0001 0000 (GR4)
            //0x08 = 0000 1000 (GR3)
            //0x04 = 0000 0100 (GR2)
            //0x02 = 0000 0010 (GR1)
            //0x01 = 0000 0001 (GR0)
            //----------------------
            RGB[] Selected_Band_gray_Gamma = new RGB[9]; //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)
            //R 
            Selected_Band_gray_Gamma[0].int_R = ((Dec_Param[0] & 0x10) << 4) + Dec_Param[3];//AM2
            Selected_Band_gray_Gamma[1].int_R = ((Dec_Param[1] & 0x80) << 1) + Dec_Param[6];//GR7
            Selected_Band_gray_Gamma[2].int_R = ((Dec_Param[1] & 0x40) << 2) + Dec_Param[7];//GR6
            Selected_Band_gray_Gamma[3].int_R = ((Dec_Param[1] & 0x20) << 3) + Dec_Param[8];//GR5
            Selected_Band_gray_Gamma[4].int_R = ((Dec_Param[1] & 0x10) << 4) + Dec_Param[9];//GR4
            Selected_Band_gray_Gamma[5].int_R = ((Dec_Param[1] & 0x08) << 5) + Dec_Param[10];//GR3
            Selected_Band_gray_Gamma[6].int_R = ((Dec_Param[1] & 0x04) << 6) + Dec_Param[11];//GR2
            Selected_Band_gray_Gamma[7].int_R = ((Dec_Param[1] & 0x02) << 7) + Dec_Param[12];//GR1
            Selected_Band_gray_Gamma[8].int_R = ((Dec_Param[1] & 0x01) << 8) + Dec_Param[13];//GR0

            //G
            Selected_Band_gray_Gamma[0].int_G = ((Dec_Param[0] & 0x04) << 6) + Dec_Param[16];//AM2
            Selected_Band_gray_Gamma[1].int_G = ((Dec_Param[14] & 0x80) << 1) + Dec_Param[19];//GR7
            Selected_Band_gray_Gamma[2].int_G = ((Dec_Param[14] & 0x40) << 2) + Dec_Param[20];//GR6
            Selected_Band_gray_Gamma[3].int_G = ((Dec_Param[14] & 0x20) << 3) + Dec_Param[21];//GR5
            Selected_Band_gray_Gamma[4].int_G = ((Dec_Param[14] & 0x10) << 4) + Dec_Param[22];//GR4
            Selected_Band_gray_Gamma[5].int_G = ((Dec_Param[14] & 0x08) << 5) + Dec_Param[23];//GR3
            Selected_Band_gray_Gamma[6].int_G = ((Dec_Param[14] & 0x04) << 6) + Dec_Param[24];//GR2
            Selected_Band_gray_Gamma[7].int_G = ((Dec_Param[14] & 0x02) << 7) + Dec_Param[25];//GR1
            Selected_Band_gray_Gamma[8].int_G = ((Dec_Param[14] & 0x01) << 8) + Dec_Param[26];//GR0

            //B
            Selected_Band_gray_Gamma[0].int_B = ((Dec_Param[0] & 0x01) << 8) + Dec_Param[29]; //AM2
            Selected_Band_gray_Gamma[1].int_B = ((Dec_Param[27] & 0x80) << 1) + Dec_Param[32];//GR7
            Selected_Band_gray_Gamma[2].int_B = ((Dec_Param[27] & 0x40) << 2) + Dec_Param[33];//GR6
            Selected_Band_gray_Gamma[3].int_B = ((Dec_Param[27] & 0x20) << 3) + Dec_Param[34];//GR5
            Selected_Band_gray_Gamma[4].int_B = ((Dec_Param[27] & 0x10) << 4) + Dec_Param[35];//GR4
            Selected_Band_gray_Gamma[5].int_B = ((Dec_Param[27] & 0x08) << 5) + Dec_Param[36];//GR3
            Selected_Band_gray_Gamma[6].int_B = ((Dec_Param[27] & 0x04) << 6) + Dec_Param[37];//GR2
            Selected_Band_gray_Gamma[7].int_B = ((Dec_Param[27] & 0x02) << 7) + Dec_Param[38];//GR1
            Selected_Band_gray_Gamma[8].int_B = ((Dec_Param[27] & 0x01) << 8) + Dec_Param[39];//GR0
            for (int gray = 0; gray < 9; gray++)
            {
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[1].Value = Selected_Band_gray_Gamma[gray].int_R.ToString();//R
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[2].Value = Selected_Band_gray_Gamma[gray].int_G.ToString();//G
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[3].Value = Selected_Band_gray_Gamma[gray].int_B.ToString();//B

            }

            //VREG1_REF1~2047 
            f1.MX_OTP_Read(21, 7, "B1"); ;
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF63 = f1.dataGridView1.Rows[5].Cells[1].Value.ToString(); //[5:0] (6bit)
            string Hex_VREG1_REF407 = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF815 = f1.dataGridView1.Rows[3].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF1227 = f1.dataGridView1.Rows[2].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF1635 = f1.dataGridView1.Rows[1].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)

            int Dec_VREG1_REF1 = Convert.ToInt32(Hex_VREG1_REF1, 16) & 0x7F;
            int Dec_VREG1_REF63 = Convert.ToInt32(Hex_VREG1_REF63, 16) & 0x3F;
            int Dec_VREG1_REF407 = Convert.ToInt32(Hex_VREG1_REF407, 16) & 0x3F;
            int Dec_VREG1_REF815 = Convert.ToInt32(Hex_VREG1_REF815, 16) & 0x3F;
            int Dec_VREG1_REF1227 = Convert.ToInt32(Hex_VREG1_REF1227, 16) & 0x3F;
            int Dec_VREG1_REF1635 = Convert.ToInt32(Hex_VREG1_REF1635, 16) & 0x3F;
            int Dec_VREG1_REF2047 = Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F;

            Voltage_VREG1_REF1 = 0.2 + (0.04 * Dec_VREG1_REF1); //Min 0.2v , Max 5.28
            Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v
            Voltage_VREG1_REF1635 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (222.5 + (0.5 * Dec_VREG1_REF1635)) / 254.0);
            Voltage_VREG1_REF1227 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (206.5 + (0.5 * Dec_VREG1_REF1227)) / 254.0);
            Voltage_VREG1_REF815 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (182.5 + (0.5 * Dec_VREG1_REF815)) / 254.0);
            Voltage_VREG1_REF407 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (154.5 + (0.5 * Dec_VREG1_REF407)) / 254.0);
            Voltage_VREG1_REF63 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (62.5 + (0.5 * Dec_VREG1_REF63)) / 254.0);

            int Dec_Vreg1 = Get_Normal_Vreg1(Set, Band);

            Vreg1_voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Dec_Vreg1, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);

            f1.GB_Status_AppendText_Nextline("Dec_Vreg1/Vreg1_voltage : " + Dec_Vreg1.ToString() + "/" + Vreg1_voltage.ToString(), Color.Blue);

            //Dll Verify
            RGB_Double[] Selected_Band_gray_Gamma_Voltage_Dll = new RGB_Double[9]; //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)
            //G255 (AM2)
            Selected_Band_gray_Gamma_Voltage_Dll[0].double_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_R);
            Selected_Band_gray_Gamma_Voltage_Dll[0].double_G = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_G);
            Selected_Band_gray_Gamma_Voltage_Dll[0].double_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_B);

            //G191,G127(GR7,GR6)
            Selected_Band_gray_Gamma_Voltage_Dll[1].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[0].double_R, Selected_Band_gray_Gamma[1].int_R, 1);
            Selected_Band_gray_Gamma_Voltage_Dll[1].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[0].double_G, Selected_Band_gray_Gamma[1].int_G, 1);
            Selected_Band_gray_Gamma_Voltage_Dll[1].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[0].double_B, Selected_Band_gray_Gamma[1].int_B, 1);

            Selected_Band_gray_Gamma_Voltage_Dll[2].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[1].double_R, Selected_Band_gray_Gamma[2].int_R, 2);
            Selected_Band_gray_Gamma_Voltage_Dll[2].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[1].double_G, Selected_Band_gray_Gamma[2].int_G, 2);
            Selected_Band_gray_Gamma_Voltage_Dll[2].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[1].double_B, Selected_Band_gray_Gamma[2].int_B, 2);

            //G63,G31,G15,G7,G4(GR5,GR4,GR3,GR2,GR1)
            Selected_Band_gray_Gamma_Voltage_Dll[3].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[2].double_R, Selected_Band_gray_Gamma[3].int_R, 3);
            Selected_Band_gray_Gamma_Voltage_Dll[3].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[2].double_G, Selected_Band_gray_Gamma[3].int_G, 3);
            Selected_Band_gray_Gamma_Voltage_Dll[3].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[2].double_B, Selected_Band_gray_Gamma[3].int_B, 3);

            Selected_Band_gray_Gamma_Voltage_Dll[4].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[3].double_R, Selected_Band_gray_Gamma[4].int_R, 4);
            Selected_Band_gray_Gamma_Voltage_Dll[4].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[3].double_G, Selected_Band_gray_Gamma[4].int_G, 4);
            Selected_Band_gray_Gamma_Voltage_Dll[4].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[3].double_B, Selected_Band_gray_Gamma[4].int_B, 4);

            Selected_Band_gray_Gamma_Voltage_Dll[5].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[4].double_R, Selected_Band_gray_Gamma[5].int_R, 5);
            Selected_Band_gray_Gamma_Voltage_Dll[5].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[4].double_G, Selected_Band_gray_Gamma[5].int_G, 5);
            Selected_Band_gray_Gamma_Voltage_Dll[5].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[4].double_B, Selected_Band_gray_Gamma[5].int_B, 5);

            Selected_Band_gray_Gamma_Voltage_Dll[6].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[5].double_R, Selected_Band_gray_Gamma[6].int_R, 6);
            Selected_Band_gray_Gamma_Voltage_Dll[6].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[5].double_G, Selected_Band_gray_Gamma[6].int_G, 6);
            Selected_Band_gray_Gamma_Voltage_Dll[6].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[5].double_B, Selected_Band_gray_Gamma[6].int_B, 6);

            Selected_Band_gray_Gamma_Voltage_Dll[7].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[6].double_R, Selected_Band_gray_Gamma[7].int_R, 7);
            Selected_Band_gray_Gamma_Voltage_Dll[7].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[6].double_G, Selected_Band_gray_Gamma[7].int_G, 7);
            Selected_Band_gray_Gamma_Voltage_Dll[7].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[6].double_B, Selected_Band_gray_Gamma[7].int_B, 7);

            //G1(GR0) 
            Selected_Band_gray_Gamma_Voltage_Dll[8].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[4], Selected_Band_gray_Gamma_Voltage_Dll[6].double_R, Selected_Band_gray_Gamma[8].int_R, 7);
            Selected_Band_gray_Gamma_Voltage_Dll[8].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[17], Selected_Band_gray_Gamma_Voltage_Dll[6].double_G, Selected_Band_gray_Gamma[8].int_G, 7);
            Selected_Band_gray_Gamma_Voltage_Dll[8].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_Param[30], Selected_Band_gray_Gamma_Voltage_Dll[6].double_B, Selected_Band_gray_Gamma[8].int_B, 7);

            for (int gray = 0; gray < 9; gray++)
            {
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[4].Value = Selected_Band_gray_Gamma_Voltage_Dll[gray].double_R.ToString();//R
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[5].Value = Selected_Band_gray_Gamma_Voltage_Dll[gray].double_G.ToString();//G
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[6].Value = Selected_Band_gray_Gamma_Voltage_Dll[gray].double_B.ToString();//B
            }

            //--------Added on 200324--------
            RGB_Double AM1_Vdata = new RGB_Double();
            AM1_Vdata.double_R = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Hex.int_R);
            AM1_Vdata.double_G = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Hex.int_G);
            AM1_Vdata.double_B = Imported_my_cpp_dll.DP173_Get_AM1_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, AM1_Hex.int_B);

            //AM1
            dataGridView_RGB_Vdata_Read.Rows[10].Cells[1].Value = AM1_Hex.int_R.ToString();//R
            dataGridView_RGB_Vdata_Read.Rows[10].Cells[2].Value = AM1_Hex.int_G.ToString();//G
            dataGridView_RGB_Vdata_Read.Rows[10].Cells[3].Value = AM1_Hex.int_B.ToString();//B
            dataGridView_RGB_Vdata_Read.Rows[10].Cells[4].Value = AM1_Vdata.double_R.ToString();//R
            dataGridView_RGB_Vdata_Read.Rows[10].Cells[5].Value = AM1_Vdata.double_G.ToString();//G
            dataGridView_RGB_Vdata_Read.Rows[10].Cells[6].Value = AM1_Vdata.double_B.ToString();//B

            RGB_Double AM0_Vdata = new RGB_Double();
            AM0_Vdata.double_R = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, AM0_Hex.int_R);
            AM0_Vdata.double_G = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, AM0_Hex.int_G);
            AM0_Vdata.double_B = Imported_my_cpp_dll.DP173_Get_AM0_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, AM0_Hex.int_B);

            //AM0
            dataGridView_RGB_Vdata_Read.Rows[11].Cells[1].Value = AM0_Hex.int_R.ToString();//R
            dataGridView_RGB_Vdata_Read.Rows[11].Cells[2].Value = AM0_Hex.int_G.ToString();//G
            dataGridView_RGB_Vdata_Read.Rows[11].Cells[3].Value = AM0_Hex.int_B.ToString();//B
            dataGridView_RGB_Vdata_Read.Rows[11].Cells[4].Value = AM0_Vdata.double_R.ToString();//R
            dataGridView_RGB_Vdata_Read.Rows[11].Cells[5].Value = AM0_Vdata.double_G.ToString();//G
            dataGridView_RGB_Vdata_Read.Rows[11].Cells[6].Value = AM0_Vdata.double_B.ToString();//B
            //-------------------------------

            //Vreg1 REF
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[1].Value = Dec_VREG1_REF2047.ToString();
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[4].Value = Voltage_VREG1_REF2047.ToString();

            dataGridView_RGB_Vdata_Read.Rows[14].Cells[1].Value = Dec_VREG1_REF1635.ToString();
            dataGridView_RGB_Vdata_Read.Rows[14].Cells[4].Value = Voltage_VREG1_REF1635.ToString();

            dataGridView_RGB_Vdata_Read.Rows[15].Cells[1].Value = Dec_VREG1_REF1227.ToString();
            dataGridView_RGB_Vdata_Read.Rows[15].Cells[4].Value = Voltage_VREG1_REF1227.ToString();

            dataGridView_RGB_Vdata_Read.Rows[16].Cells[1].Value = Dec_VREG1_REF815.ToString();
            dataGridView_RGB_Vdata_Read.Rows[16].Cells[4].Value = Voltage_VREG1_REF815.ToString();

            dataGridView_RGB_Vdata_Read.Rows[17].Cells[1].Value = Dec_VREG1_REF407.ToString();
            dataGridView_RGB_Vdata_Read.Rows[17].Cells[4].Value = Voltage_VREG1_REF407.ToString();

            dataGridView_RGB_Vdata_Read.Rows[18].Cells[1].Value = Dec_VREG1_REF63.ToString();
            dataGridView_RGB_Vdata_Read.Rows[18].Cells[4].Value = Voltage_VREG1_REF63.ToString();

            dataGridView_RGB_Vdata_Read.Rows[19].Cells[1].Value = Dec_VREG1_REF1.ToString();
            dataGridView_RGB_Vdata_Read.Rows[19].Cells[4].Value = Voltage_VREG1_REF1.ToString();

            //Vreg1
            dataGridView_RGB_Vdata_Read.Rows[20].Cells[1].Value = Dec_Vreg1.ToString();
            dataGridView_RGB_Vdata_Read.Rows[20].Cells[4].Value = Vreg1_voltage.ToString();

            f1.GB_Status_AppendText_Nextline("Set" + Set.ToString() + "/Band" + Band.ToString() + " RGB Read Finished", Color.Blue);
        }

        private double Get_Set1_HBM_RGB_Min_White(RGB HBM_White_Gamma)
        {
            //Dll Verify
            RGB_Double Set1_HBM_RGB_White = new RGB_Double(); //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)
            //G255 (AM2)
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(21, 7, "B1"); ;
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)
            int Dec_VREG1_REF1 = Convert.ToInt32(Hex_VREG1_REF1, 16) & 0x7F;
            int Dec_VREG1_REF2047 = Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F;

            double Voltage_VREG1_REF1 = 0.2 + (0.04 * Dec_VREG1_REF1); //Min 0.2v , Max 5.28
            double Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v

            Set1_HBM_RGB_White.double_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Voltage_VREG1_REF1, HBM_White_Gamma.int_R);
            Set1_HBM_RGB_White.double_G = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Voltage_VREG1_REF1, HBM_White_Gamma.int_G);
            Set1_HBM_RGB_White.double_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Voltage_VREG1_REF1, HBM_White_Gamma.int_B);

            double Set1_HBM_RGB_Min_White = Set1_HBM_RGB_White.double_R;
            if (Set1_HBM_RGB_Min_White > Set1_HBM_RGB_White.double_G) Set1_HBM_RGB_Min_White = Set1_HBM_RGB_White.double_G;
            if (Set1_HBM_RGB_Min_White > Set1_HBM_RGB_White.double_B) Set1_HBM_RGB_Min_White = Set1_HBM_RGB_White.double_B;

            return Set1_HBM_RGB_Min_White;
        }







        private void button_ELVSS_Margin_Test_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            button_Read_DP173_DBV_Setting.PerformClick();


            Gamma_Set Set = Gamma_Set.Set1;
            if (radioButton_ELVSS_Test_Set1.Checked)
            {
                Set = Gamma_Set.Set1;
                button_Gamma_Set1_Apply.PerformClick();
            }

            int Band = Convert.ToInt32(textBox_ELVSS_Band_Select.Text);
            if (Band >= 10)
            {
                Band = 10;
                textBox_ELVSS_Band_Select.Text = "10";
            }
            else if (Band < 0)
            {
                Band = 0;
                textBox_ELVSS_Band_Select.Text = "0";
            }
            Optic_Compensation_Stop = false;
            DP173_DBV_Setting(Band);//DBV Setting
            //W
            f1.dataGridView2.Rows.Add("W", "-", "-", "-");
            f1.PTN_update(255, 255, 255);
            Thread.Sleep(300);
            ELVSS_Margin_Test(Set, Band);
            //R
            f1.dataGridView2.Rows.Add("R", "-", "-", "-");
            f1.PTN_update(255, 0, 0);
            Thread.Sleep(300);
            ELVSS_Margin_Test(Set, Band);
            //G
            f1.dataGridView2.Rows.Add("G", "-", "-", "-");
            f1.PTN_update(0, 255, 0);
            Thread.Sleep(300);
            ELVSS_Margin_Test(Set, Band);
            //B
            f1.dataGridView2.Rows.Add("B", "-", "-", "-");
            f1.PTN_update(0, 0, 255);
            Thread.Sleep(300);
            ELVSS_Margin_Test(Set, Band);
            f1.dataGridView2.Rows.Add("-", "-", "-", "-");

            /*
            for (int band = 0; band <= 9; band++)
            {
                f1.dataGridView2.Rows.Add("B" + band.ToString(), "-", "-", "-");
                DP173_DBV_Setting(band);//DBV Setting
                //W
                f1.dataGridView2.Rows.Add("W", "-", "-", "-");
                f1.PTN_update(255, 255, 255);
                Thread.Sleep(300);
                ELVSS_Margin_Test(Set, band);
                //R
                f1.dataGridView2.Rows.Add("R", "-", "-", "-");
                f1.PTN_update(255, 0, 0);
                Thread.Sleep(300);
                ELVSS_Margin_Test(Set, band);
                //G
                f1.dataGridView2.Rows.Add("G", "-", "-", "-");
                f1.PTN_update(0, 255, 0);
                Thread.Sleep(300);
                ELVSS_Margin_Test(Set, band);
                //B
                f1.dataGridView2.Rows.Add("B", "-", "-", "-");
                f1.PTN_update(0, 0, 255);
                Thread.Sleep(300);
                ELVSS_Margin_Test(Set, band);
                f1.dataGridView2.Rows.Add("-", "-", "-", "-");
            }*/
        }

        private void ELVSS_Margin_Test(Gamma_Set Set, int band)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (Set == Gamma_Set.Set1) f1.MX_OTP_Read(129, 11, "E0"); //OK
            else if (Set == Gamma_Set.Set2) f1.MX_OTP_Read(140, 11, "E0"); //OK
            else if (Set == Gamma_Set.Set3) f1.MX_OTP_Read(151, 11, "E0"); //OK
            else if (Set == Gamma_Set.Set4) f1.MX_OTP_Read(162, 11, "E0"); //Ok
            else if (Set == Gamma_Set.Set5) f1.MX_OTP_Read(173, 11, "E0"); //OK
            else if (Set == Gamma_Set.Set6) f1.MX_OTP_Read(184, 11, "E0"); //Ok
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
                return;
            }

            string[] hex_ELVSS = new string[11];
            double[] dec_ELVSS = new double[11];
            double[] ELVSS = new double[11];
            for (int i = 0; i < 11; i++)
            {
                hex_ELVSS[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS[i] = Convert.ToDouble(Convert.ToInt32(hex_ELVSS[i], 16));
                ELVSS[i] = ((dec_ELVSS[i] - 30) / 10.0) - 3.1;
            }

            int delay = Convert.ToInt32(textBox_ELVSS_CMD_Delay.Text);

            // 3Ah = -6.0v , 2h = -2.0v
            for (int Current_Dec = 1; Current_Dec <= 41; Current_Dec++)
            {
                if (Optic_Compensation_Stop) break;

                ELVSS[band] = ((Current_Dec - 30) / 10.0) - 3.1;
                hex_ELVSS[band] = Current_Dec.ToString("X2");
                Send_ELVSS_Setting(Set, hex_ELVSS);
                Update_ELVSS_Textbox(Set, band, ELVSS[band], hex_ELVSS[band]);
                Thread.Sleep(delay); f1.GB_Status_AppendText_Nextline("Applied Delay : " + delay.ToString(), Color.Blue);
                if (checkBox_ELVSS_CA_Measure.Checked)
                {
                    f1.CA_Measure_For_ELVSS(ELVSS[band].ToString());
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("ELVSS : " + ELVSS[band].ToString(), Color.Blue);
                }
            }
        }

        private void Update_ELVSS_Textbox(Gamma_Set Set, int band, double ELVSS, string hex_ELVSS)
        {
            if (band >= 11)
            {
                if (band == 11) textBox_ELVSS_A0.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                else if (band == 12) textBox_ELVSS_A1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                else if (band == 13) textBox_ELVSS_A2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
            }
            else if (Set == Gamma_Set.Set1)
            {
                switch (band)
                {
                    case 0:
                        textBox_ELVSS_B0_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 1:
                        textBox_ELVSS_B1_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 2:
                        textBox_ELVSS_B2_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 3:
                        textBox_ELVSS_B3_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 4:
                        textBox_ELVSS_B4_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 5:
                        textBox_ELVSS_B5_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 6:
                        textBox_ELVSS_B6_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 7:
                        textBox_ELVSS_B7_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 8:
                        textBox_ELVSS_B8_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 9:
                        textBox_ELVSS_B9_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 10:
                        textBox_ELVSS_B10_Set1.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set2)
            {
                switch (band)
                {
                    case 0:
                        textBox_ELVSS_B0_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 1:
                        textBox_ELVSS_B1_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 2:
                        textBox_ELVSS_B2_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 3:
                        textBox_ELVSS_B3_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 4:
                        textBox_ELVSS_B4_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 5:
                        textBox_ELVSS_B5_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 6:
                        textBox_ELVSS_B6_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 7:
                        textBox_ELVSS_B7_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 8:
                        textBox_ELVSS_B8_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 9:
                        textBox_ELVSS_B9_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 10:
                        textBox_ELVSS_B10_Set2.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set3)
            {
                switch (band)
                {
                    case 0:
                        textBox_ELVSS_B0_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 1:
                        textBox_ELVSS_B1_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 2:
                        textBox_ELVSS_B2_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 3:
                        textBox_ELVSS_B3_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 4:
                        textBox_ELVSS_B4_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 5:
                        textBox_ELVSS_B5_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 6:
                        textBox_ELVSS_B6_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 7:
                        textBox_ELVSS_B7_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 8:
                        textBox_ELVSS_B8_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 9:
                        textBox_ELVSS_B9_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 10:
                        textBox_ELVSS_B10_Set3.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set4)
            {
                switch (band)
                {
                    case 0:
                        textBox_ELVSS_B0_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 1:
                        textBox_ELVSS_B1_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 2:
                        textBox_ELVSS_B2_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 3:
                        textBox_ELVSS_B3_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 4:
                        textBox_ELVSS_B4_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 5:
                        textBox_ELVSS_B5_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 6:
                        textBox_ELVSS_B6_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 7:
                        textBox_ELVSS_B7_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 8:
                        textBox_ELVSS_B8_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 9:
                        textBox_ELVSS_B9_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 10:
                        textBox_ELVSS_B10_Set4.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set5)
            {
                switch (band)
                {
                    case 0:
                        textBox_ELVSS_B0_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 1:
                        textBox_ELVSS_B1_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 2:
                        textBox_ELVSS_B2_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 3:
                        textBox_ELVSS_B3_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 4:
                        textBox_ELVSS_B4_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 5:
                        textBox_ELVSS_B5_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 6:
                        textBox_ELVSS_B6_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 7:
                        textBox_ELVSS_B7_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 8:
                        textBox_ELVSS_B8_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 9:
                        textBox_ELVSS_B9_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 10:
                        textBox_ELVSS_B10_Set5.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set6)
            {
                switch (band)
                {
                    case 0:
                        textBox_ELVSS_B0_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 1:
                        textBox_ELVSS_B1_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 2:
                        textBox_ELVSS_B2_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 3:
                        textBox_ELVSS_B3_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 4:
                        textBox_ELVSS_B4_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 5:
                        textBox_ELVSS_B5_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 6:
                        textBox_ELVSS_B6_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 7:
                        textBox_ELVSS_B7_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 8:
                        textBox_ELVSS_B8_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 9:
                        textBox_ELVSS_B9_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    case 10:
                        textBox_ELVSS_B10_Set6.Text = ELVSS.ToString() + "v (" + hex_ELVSS + "h)";
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                        break;
                }
            }


            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3 or 4");
            }
        }


        private void button_Flash_Write_Click(object sender, EventArgs e)
        {
            bool Flash_Checksum_OK = true;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (Flash_Checksum_OK)
            {
                Enable_Flash_Erase_Write();
                int Flash_Erase_Write_Delay = 200;
                Flash_Erase_And_Write(Flash_Erase_Write_Delay);
            }
            System.Windows.Forms.MessageBox.Show("Flash Erase & Write Process Finished");
        }

        private void Enable_Flash_Erase_Write()
        {
            //Enable Flash Erase & Write
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP173_One_Param_CMD_Send(4, "E7", "AA");//Unlock CMOTP protection (Flash Writng Enabled)
            f1.DP173_One_Param_CMD_Send(6, "E7", "01");//Set Flash Write Clock as 1ms 
        }

        private void Flash_Erase_And_Write(int Flash_Erase_Write_Delay)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            //CMOTP
            f1.DP173_One_Param_CMD_Send(7, "E7", "1F");//Wrate Data to Flash
            Thread.Sleep(Flash_Erase_Write_Delay);
            Flash_Fail_Verify_Check("CMOTP");

            //GMOTP
            f1.DP173_One_Param_CMD_Send(8, "E7", "1F");//Wrate Data to Flash
            Thread.Sleep(Flash_Erase_Write_Delay);
            Flash_Fail_Verify_Check("GMOTP");

            //LGOTP
            f1.DP173_One_Param_CMD_Send(9, "E7", "1F");//Wrate Data to Flash
            Thread.Sleep(Flash_Erase_Write_Delay);
            Flash_Fail_Verify_Check("LGOTP");
        }

        private void Flash_Fail_Verify_Check(string Show_Memory_Block)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(2, 1, "DD");
            string Flash_Status_Check = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            if (Flash_Status_Check == "01")
            {
                MessageBox.Show(Show_Memory_Block + " Writing is not finished yet(Flash Busy). it needs to be set more writing time");
                f1.GB_Status_AppendText_Nextline(Show_Memory_Block + " Writing is not finished yet(Flash Busy). it needs to be set more writing time", Color.Black);
            }
            else if (Flash_Status_Check == "80")
            {
                MessageBox.Show(Show_Memory_Block + " OC Writing Failed.");
                f1.GB_Status_AppendText_Nextline(Show_Memory_Block + " Writing Failed(OK)", Color.Red);
            }
            else if (Flash_Status_Check == "00")
            {
                f1.GB_Status_AppendText_Nextline(Show_Memory_Block + " Flash OC Writing OK.", Color.Green);
            }
            else
            {
                MessageBox.Show("Unknown Status, DD(P3) = " + Flash_Status_Check);
            }
        }




        private void button_Flash_CRC_Check_Click(object sender, EventArgs e)
        {
            int Calculate_Checksum_Delay = 30;
            uint Total_Dec_Checksum_CMOTP = 0;
            uint Total_Dec_Checksum_GMOTP = 0;
            uint Total_Dec_Checksum_LGOTP = 0;
            Flash_CRC_Check(Calculate_Checksum_Delay, ref Total_Dec_Checksum_CMOTP, ref Total_Dec_Checksum_GMOTP, ref Total_Dec_Checksum_LGOTP);
        }

        private int Convert_Binary_String_To_Int(string binary)
        {
            return Convert.ToInt32("binary", 2);
        }


        private string calculate_twos_complement(string Binary_Sum)
        {
            int i, carry = 1;
            char[] one_comp = Binary_Sum.ToCharArray();
            char[] two_comp = Binary_Sum.ToCharArray();
            for (i = 0; i < Binary_Sum.Length; i++)
            {
                if (Binary_Sum[i] == '0')
                {
                    one_comp[i] = '1';
                }
                else if (Binary_Sum[i] == '1')
                {
                    one_comp[i] = '0';
                }
            }
            for (i = Binary_Sum.Length - 1; i >= 0; i--)
            {
                if (one_comp[i] == '1' && carry == 1)
                {
                    two_comp[i] = '0';
                }
                else if (one_comp[i] == '0' && carry == 1)
                {
                    two_comp[i] = '1';
                    carry = 0;
                }
                else //if carry = 0
                {
                    two_comp[i] = one_comp[i];
                }
            }
            return new string(two_comp);
        }






        private void Flash_CRC_Check(int Calculate_Checksum_Delay, ref uint Total_Dec_Checksum_CMOTP, ref uint Total_Dec_Checksum_GMOTP, ref uint Total_Dec_Checksum_LGOTP)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP173_One_Param_CMD_Send(7, "E7", "00");
            f1.DP173_One_Param_CMD_Send(7, "E7", "80");//CMOTP Pre-load Function Start (Read CMOTP Group Register and Calculate Checksum)
            Thread.Sleep(Calculate_Checksum_Delay);
            f1.MX_OTP_Read(5, 4, "DD"); //Read Hex_CheckSum and Show
            string[] Hex_Checksum_CMOTP = new string[4];
            string Total_Hex_Checksum_CMOTP = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                Hex_Checksum_CMOTP[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Total_Hex_Checksum_CMOTP += Hex_Checksum_CMOTP[i];
            }

            f1.DP173_One_Param_CMD_Send(8, "E7", "00");
            f1.DP173_One_Param_CMD_Send(8, "E7", "80");//CMOTP Pre-load Function Start (Read CMOTP Group Register and Calculate Checksum)
            Thread.Sleep(Calculate_Checksum_Delay);
            f1.MX_OTP_Read(9, 4, "DD"); //Read Hex_CheckSum and Show
            string[] Hex_Checksum_GMOTP = new string[4];
            string Total_Hex_Checksum_GMOTP = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                Hex_Checksum_GMOTP[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Total_Hex_Checksum_GMOTP += Hex_Checksum_GMOTP[i];
            }

            f1.DP173_One_Param_CMD_Send(9, "E7", "00");
            f1.DP173_One_Param_CMD_Send(9, "E7", "80");//CMOTP Pre-load Function Start (Read CMOTP Group Register and Calculate Checksum)
            Thread.Sleep(Calculate_Checksum_Delay);
            f1.MX_OTP_Read(13, 4, "DD"); //Read Hex_CheckSum and Show
            string[] Hex_Checksum_LGOTP = new string[4];
            string Total_Hex_Checksum_LGOTP = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                Hex_Checksum_LGOTP[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Total_Hex_Checksum_LGOTP += Hex_Checksum_LGOTP[i];
            }

            for (int i = 0; i < 4; i++) f1.GB_Status_AppendText_Nextline("Hex_Checksum_CMOTP[" + i.ToString() + "] : " + Hex_Checksum_CMOTP[i], Color.Red);
            for (int i = 0; i < 4; i++) f1.GB_Status_AppendText_Nextline("Hex_Checksum_GMOTP[" + i.ToString() + "] : " + Hex_Checksum_GMOTP[i], Color.Green);
            for (int i = 0; i < 4; i++) f1.GB_Status_AppendText_Nextline("Hex_Checksum_LGOTP[" + i.ToString() + "] : " + Hex_Checksum_LGOTP[i], Color.Blue);


            Total_Dec_Checksum_CMOTP = Convert.ToUInt32(Total_Hex_Checksum_CMOTP, 16); //uint = UInt32 --> [0 ~ 4294967295(FFFFFFFF)]
            Total_Dec_Checksum_GMOTP = Convert.ToUInt32(Total_Hex_Checksum_GMOTP, 16);
            Total_Dec_Checksum_LGOTP = Convert.ToUInt32(Total_Hex_Checksum_LGOTP, 16);
            f1.GB_Status_AppendText_Nextline("Total_Dec_Checksum_CMOTP : " + Total_Dec_Checksum_CMOTP.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("Total_Dec_Checksum_GMOTP : " + Total_Dec_Checksum_GMOTP.ToString(), Color.Green);
            f1.GB_Status_AppendText_Nextline("Total_Dec_Checksum_LGOTP : " + Total_Dec_Checksum_LGOTP.ToString(), Color.Blue);
        }


        private void button_Test_Click(object sender, EventArgs e)
        {


            //Excel Test
            /*
            Excel_Control excel_ctrl = new Excel_Control(Directory.GetCurrentDirectory() + "\\Optic_Measurement\\Auto_Measured_GCS_BCS_4.xlsx", "Sample_1");  //5 seconds
            for (int i = 34; i < 34 + 255; i++) excel_ctrl.UpdateExcelData(i, 3, i * 2);  //12 seconds (255ea Data)
            excel_ctrl.Save_Data_to_Excel_File(); //5 seconds
            */

            /*
            int band = 0;
            bool A = true;
            for (band = 0; band < 14 && Optic_Compensation_Stop == false; band++)
            {
                if (A)
                {
                    A = false;
                    band--;
                    continue;
                }
            }*/
        }

        void update_RGB_Read_Grid(RGB[,] All_band_gray_Gamma, int band)
        {
            for (int gray = 0; gray < 8; gray++)
            {
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[1].Value = All_band_gray_Gamma[band, gray].int_R.ToString();//R
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[2].Value = All_band_gray_Gamma[band, gray].int_G.ToString();//G
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[3].Value = All_band_gray_Gamma[band, gray].int_B.ToString();//B
            }
        }

        void update_RGB_Read_Grid_G1(RGB Gamma)
        {
            dataGridView_RGB_Vdata_Read.Rows[9].Cells[1].Value = Gamma.int_R.ToString();//R
            dataGridView_RGB_Vdata_Read.Rows[9].Cells[2].Value = Gamma.int_G.ToString();//G
            dataGridView_RGB_Vdata_Read.Rows[9].Cells[3].Value = Gamma.int_B.ToString();//B   
        }







        private void button_VREF1_Test_Click(object sender, EventArgs e)
        {

        }

        private void button_AM0_Write_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Offset = 0;//Set 1
            for (int band = 0; band <= 10; band++)
            {
                string Band_Register = DP173.Get_Gamma_Register_Hex_String(band).Remove(0, 2);
                f1.DP173_One_Param_CMD_Send(Offset + 5, Band_Register, "00");
                f1.DP173_One_Param_CMD_Send(Offset + 18, Band_Register, "00");
                f1.DP173_One_Param_CMD_Send(Offset + 31, Band_Register, "00");
            }

            Offset = 40;//Set 2
            for (int band = 0; band <= 10; band++)
            {
                string Band_Register = DP173.Get_Gamma_Register_Hex_String(band).Remove(0, 2);
                f1.DP173_One_Param_CMD_Send(Offset + 5, Band_Register, "00");
                f1.DP173_One_Param_CMD_Send(Offset + 18, Band_Register, "00");
                f1.DP173_One_Param_CMD_Send(Offset + 31, Band_Register, "00");
            }

            Offset = 80;//Set 3
            for (int band = 0; band <= 10; band++)
            {
                string Band_Register = DP173.Get_Gamma_Register_Hex_String(band).Remove(0, 2);
                f1.DP173_One_Param_CMD_Send(Offset + 5, Band_Register, "00");
                f1.DP173_One_Param_CMD_Send(Offset + 18, Band_Register, "00");
                f1.DP173_One_Param_CMD_Send(Offset + 31, Band_Register, "00");
            }

            Offset = 120;//Set 4
            for (int band = 0; band <= 10; band++)
            {
                string Band_Register = DP173.Get_Gamma_Register_Hex_String(band).Remove(0, 2);
                f1.DP173_One_Param_CMD_Send(Offset + 5, Band_Register, "00");
                f1.DP173_One_Param_CMD_Send(Offset + 18, Band_Register, "00");
                f1.DP173_One_Param_CMD_Send(Offset + 31, Band_Register, "00");
            }
        }

        private void groupBox17_Enter(object sender, EventArgs e)
        {

        }

        private void button_POCB_On_G2G_On_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xCF 0xF5");//updated on 200317
            f1.GB_Status_AppendText_Nextline("POCB On + G2G On", Color.Blue);
        }

        private void button_POCB_On_G2G_Off_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xCF 0xF4");//updated on 200317
            f1.GB_Status_AppendText_Nextline("POCB On + G2G Off", Color.DarkBlue);
        }

        private void button_POCB_Off_G2G_On_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xCF 0x81");//updated on 200317
            f1.GB_Status_AppendText_Nextline("POCB Off + G2G On", Color.Red);
        }

        private void button_POCB_Off_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xCF 0x00");//updated on 200317
            f1.GB_Status_AppendText_Nextline("POCB Off + G2G Off", Color.DarkRed);
        }

 
        private void button_Res_Test_Gray255_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.dataGridView2_Rows_Add("G255");
            int Selected_Gray = 0;
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            f1.PTN_update(255, 255, 255); Thread.Sleep(500);
            Offset_Measure(Selected_Gray, Offset);
        }

        private void Offset_Measure(int Selected_Gray, int Offset)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Red
            f1.dataGridView2_Rows_Add("Red");
            Write_RGB(Selected_Gray, 0, 0, 0);
            f1.CA_Measure_For_ELVSS("0");
            Write_RGB(Selected_Gray, Offset, 0, 0);
            f1.CA_Measure_For_ELVSS("+" + textBox_Resolution_Test_Offset.Text);
            Write_RGB(Selected_Gray, -Offset * 2, 0, 0);
            f1.CA_Measure_For_ELVSS("-" + textBox_Resolution_Test_Offset.Text);

            //Green
            f1.dataGridView2_Rows_Add("Green");
            Write_RGB(Selected_Gray, Offset, 0, 0);
            f1.CA_Measure_For_ELVSS("0");
            Write_RGB(Selected_Gray, 0, Offset, 0);
            f1.CA_Measure_For_ELVSS("+" + textBox_Resolution_Test_Offset.Text);
            Write_RGB(Selected_Gray, 0, -Offset * 2, 0);
            f1.CA_Measure_For_ELVSS("-" + textBox_Resolution_Test_Offset.Text);

            //Blue
            f1.dataGridView2_Rows_Add("Blue");
            Write_RGB(Selected_Gray, 0, Offset, 0);
            f1.CA_Measure_For_ELVSS("0");
            Write_RGB(Selected_Gray, 0, 0, Offset);
            f1.CA_Measure_For_ELVSS("+" + textBox_Resolution_Test_Offset.Text);
            Write_RGB(Selected_Gray, 0, 0, -Offset * 2);
            f1.CA_Measure_For_ELVSS("-" + textBox_Resolution_Test_Offset.Text);
            Write_RGB(Selected_Gray, 0, 0, Offset);
        }

        private void Write_RGB(int Selected_Gray, int R_Offset, int G_Offset, int B_Offset)
        {
            while (true)
            {
                if (dataGridView_RGB_Vdata_Write.Rows.Count > 11) dataGridView_RGB_Vdata_Write.Rows.RemoveAt(dataGridView_RGB_Vdata_Write.Rows.Count - 2);
                else break;
            }

            for (int k = 0; k < 4; k++) dataGridView_RGB_Vdata_Write.Rows[dataGridView_RGB_Vdata_Write.Rows.Count - 1].Cells[k].Value = "";

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            string Band_Address = DP173.Get_Gamma_Register_Hex_String(Band).Remove(0, 2); //"XX"
            Gamma_Set Set = Get_Selected_Set_Of_RGB_Vdata_Read_Write();
            Application.DoEvents();

            DP173_or_Elgin Model = new DP173_or_Elgin(OC_Single_Dual_Triple.Single);

            RGB[] Gamma_9th_data = new RGB[9]; //G255
            RGB[] Gamma_8ea_data = new RGB[9]; //GXXX < G255
            RGB[] Selected_Band_gray_Gamma = new RGB[9]; //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)
            for (int gray = 0; gray < 9; gray++)
            {
                Selected_Band_gray_Gamma[gray].int_R = Convert.ToInt32(dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[1].Value);//R
                Selected_Band_gray_Gamma[gray].int_G = Convert.ToInt32(dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[2].Value);//G
                Selected_Band_gray_Gamma[gray].int_B = Convert.ToInt32(dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[3].Value);//B
            }

            //----Added----
            Selected_Band_gray_Gamma[Selected_Gray].int_R += R_Offset;
            Selected_Band_gray_Gamma[Selected_Gray].int_G += G_Offset;
            Selected_Band_gray_Gamma[Selected_Gray].int_B += B_Offset;
            dataGridView_RGB_Vdata_Read.Rows[Selected_Gray + 1].Cells[1].Value = Selected_Band_gray_Gamma[Selected_Gray].int_R;
            dataGridView_RGB_Vdata_Read.Rows[Selected_Gray + 1].Cells[2].Value = Selected_Band_gray_Gamma[Selected_Gray].int_G;
            dataGridView_RGB_Vdata_Read.Rows[Selected_Gray + 1].Cells[3].Value = Selected_Band_gray_Gamma[Selected_Gray].int_B;
            //-------------

            string[] Hex_Param = new string[40];
            for (int gray = 0; gray < 9; gray++)
            {
                Gamma_9th_data[gray].Set_Value(Selected_Band_gray_Gamma[gray].int_R >> 8, Selected_Band_gray_Gamma[gray].int_G >> 8, Selected_Band_gray_Gamma[gray].int_B >> 8);
                Gamma_8ea_data[gray].Set_Value(Selected_Band_gray_Gamma[gray].int_R & 0xFF, Selected_Band_gray_Gamma[gray].int_G & 0xFF, Selected_Band_gray_Gamma[gray].int_B & 0xFF);
            }

            Hex_Param[0] = ((Gamma_9th_data[0].int_R << 4) + (Gamma_9th_data[0].int_G << 2) + (Gamma_9th_data[0].int_B)).ToString("X2");//G255 RGB 9th bit

            Hex_Param[1] = ((Gamma_9th_data[1].int_R << 7) + (Gamma_9th_data[2].int_R << 6)
                + (Gamma_9th_data[3].int_R << 5) + (Gamma_9th_data[4].int_R << 4)
                + (Gamma_9th_data[5].int_R << 3) + (Gamma_9th_data[6].int_R << 2)
                + (Gamma_9th_data[7].int_R << 1) + Gamma_9th_data[8].int_R).ToString("X2");//GXX < G255 R 9th

            Hex_Param[2] = "00";
            Hex_Param[3] = Gamma_8ea_data[0].int_R.ToString("X2"); //G255[7:0]
            Hex_Param[4] = Model.R_AM1_Hex;
            Hex_Param[5] = Model.R_AM0_Hex;
            Hex_Param[6] = Gamma_8ea_data[1].int_R.ToString("X2"); //G191[7:0]
            Hex_Param[7] = Gamma_8ea_data[2].int_R.ToString("X2"); //G127[7:0]
            Hex_Param[8] = Gamma_8ea_data[3].int_R.ToString("X2"); //G63[7:0]
            Hex_Param[9] = Gamma_8ea_data[4].int_R.ToString("X2"); //G31[7:0]
            Hex_Param[10] = Gamma_8ea_data[5].int_R.ToString("X2"); //G15[7:0]
            Hex_Param[11] = Gamma_8ea_data[6].int_R.ToString("X2"); //G7[7:0]
            Hex_Param[12] = Gamma_8ea_data[7].int_R.ToString("X2");//G4[7:0]
            Hex_Param[13] = Gamma_8ea_data[8].int_R.ToString("X2");//G1[7:0]



            Hex_Param[14] = ((Gamma_9th_data[1].int_G << 7) + (Gamma_9th_data[2].int_G << 6)
                + (Gamma_9th_data[3].int_G << 5) + (Gamma_9th_data[4].int_G << 4)
                + (Gamma_9th_data[5].int_G << 3) + (Gamma_9th_data[6].int_G << 2)
                + (Gamma_9th_data[7].int_G << 1) + Gamma_9th_data[8].int_G).ToString("X2"); //GXX < G255 G 9th
            Hex_Param[15] = "00";
            Hex_Param[16] = Gamma_8ea_data[0].int_G.ToString("X2"); //G255[7:0]
            Hex_Param[17] = Model.G_AM1_Hex;
            Hex_Param[18] = Model.G_AM0_Hex;
            Hex_Param[19] = Gamma_8ea_data[1].int_G.ToString("X2"); //G191[7:0]
            Hex_Param[20] = Gamma_8ea_data[2].int_G.ToString("X2"); //G127[7:0]
            Hex_Param[21] = Gamma_8ea_data[3].int_G.ToString("X2"); //G63[7:0]
            Hex_Param[22] = Gamma_8ea_data[4].int_G.ToString("X2"); //G31[7:0]
            Hex_Param[23] = Gamma_8ea_data[5].int_G.ToString("X2"); //G15[7:0]
            Hex_Param[24] = Gamma_8ea_data[6].int_G.ToString("X2"); //G7[7:0]
            Hex_Param[25] = Gamma_8ea_data[7].int_G.ToString("X2");//G4[7:0]
            Hex_Param[26] = Gamma_8ea_data[8].int_G.ToString("X2");//G1[7:0]


            Hex_Param[27] = ((Gamma_9th_data[1].int_B << 7) + (Gamma_9th_data[2].int_B << 6)
                + (Gamma_9th_data[3].int_B << 5) + (Gamma_9th_data[4].int_B << 4)
                + (Gamma_9th_data[5].int_B << 3) + (Gamma_9th_data[6].int_B << 2)
                + (Gamma_9th_data[7].int_B << 1) + Gamma_9th_data[8].int_B).ToString("X2"); //GXX < G255 B 9th
            Hex_Param[28] = "00";
            Hex_Param[29] = Gamma_8ea_data[0].int_B.ToString("X2"); //G255[7:0]
            Hex_Param[30] = Model.B_AM1_Hex;
            Hex_Param[31] = Model.B_AM0_Hex;
            Hex_Param[32] = Gamma_8ea_data[1].int_B.ToString("X2"); //G191[7:0]
            Hex_Param[33] = Gamma_8ea_data[2].int_B.ToString("X2"); //G127[7:0]
            Hex_Param[34] = Gamma_8ea_data[3].int_B.ToString("X2"); //G63[7:0]
            Hex_Param[35] = Gamma_8ea_data[4].int_B.ToString("X2"); //G31[7:0]
            Hex_Param[36] = Gamma_8ea_data[5].int_B.ToString("X2"); //G15[7:0]
            Hex_Param[37] = Gamma_8ea_data[6].int_B.ToString("X2"); //G7[7:0]
            Hex_Param[38] = Gamma_8ea_data[7].int_B.ToString("X2");//G4[7:0]
            Hex_Param[39] = Gamma_8ea_data[8].int_B.ToString("X2");//G1[7:0]

            int Offset = 0;
            if (Set == Gamma_Set.Set1 && Band < 11) Offset = 0;//Set1 Normal
            else if (Set == Gamma_Set.Set2 && Band < 11) Offset = 40;//Set2 Normal
            else if (Set == Gamma_Set.Set3 && Band < 11) Offset = 80;//Set3 Normal
            else if (Set == Gamma_Set.Set4 && Band < 11) Offset = 120;//Set4 Normal
            else if (Set == Gamma_Set.Set5 && Band < 11) Offset = 160;//Set5 Normal
            else if (Set == Gamma_Set.Set6 && Band < 11) Offset = 200;//Set6 Normal
            else { MessageBox.Show("Band > 11, which is out of options"); }

            f1.DP173_Long_Packet_CMD_Send(Offset, Hex_Param.Length, Band_Address, Hex_Param);

            //Show Vdata
            RGB_Double[] Selected_Band_gray_Gamma_Voltage = new RGB_Double[9]; //(9ea Gray-points-> 255/191/127/63/31/15/7/4/1)

            //G255 (AM2)
            Selected_Band_gray_Gamma_Voltage[0].double_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_R);
            Selected_Band_gray_Gamma_Voltage[0].double_G = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_G);
            Selected_Band_gray_Gamma_Voltage[0].double_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Selected_Band_gray_Gamma[0].int_B);

            //G191,G127(GR7,GR6)
            Selected_Band_gray_Gamma_Voltage[1].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.R_AM1_Dec, Selected_Band_gray_Gamma_Voltage[0].double_R, Selected_Band_gray_Gamma[1].int_R, 1);
            Selected_Band_gray_Gamma_Voltage[1].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.G_AM1_Dec, Selected_Band_gray_Gamma_Voltage[0].double_G, Selected_Band_gray_Gamma[1].int_G, 1);
            Selected_Band_gray_Gamma_Voltage[1].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.B_AM1_Dec, Selected_Band_gray_Gamma_Voltage[0].double_B, Selected_Band_gray_Gamma[1].int_B, 1);

            Selected_Band_gray_Gamma_Voltage[2].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.R_AM1_Dec, Selected_Band_gray_Gamma_Voltage[1].double_R, Selected_Band_gray_Gamma[2].int_R, 2);
            Selected_Band_gray_Gamma_Voltage[2].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.G_AM1_Dec, Selected_Band_gray_Gamma_Voltage[1].double_G, Selected_Band_gray_Gamma[2].int_G, 2);
            Selected_Band_gray_Gamma_Voltage[2].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.B_AM1_Dec, Selected_Band_gray_Gamma_Voltage[1].double_B, Selected_Band_gray_Gamma[2].int_B, 2);

            //G63,G31,G15,G7,G4(GR5,GR4,GR3,GR2,GR1)
            Selected_Band_gray_Gamma_Voltage[3].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.R_AM1_Dec, Selected_Band_gray_Gamma_Voltage[2].double_R, Selected_Band_gray_Gamma[3].int_R, 3);
            Selected_Band_gray_Gamma_Voltage[3].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.G_AM1_Dec, Selected_Band_gray_Gamma_Voltage[2].double_G, Selected_Band_gray_Gamma[3].int_G, 3);
            Selected_Band_gray_Gamma_Voltage[3].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.B_AM1_Dec, Selected_Band_gray_Gamma_Voltage[2].double_B, Selected_Band_gray_Gamma[3].int_B, 3);

            Selected_Band_gray_Gamma_Voltage[4].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.R_AM1_Dec, Selected_Band_gray_Gamma_Voltage[3].double_R, Selected_Band_gray_Gamma[4].int_R, 4);
            Selected_Band_gray_Gamma_Voltage[4].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.G_AM1_Dec, Selected_Band_gray_Gamma_Voltage[3].double_G, Selected_Band_gray_Gamma[4].int_G, 4);
            Selected_Band_gray_Gamma_Voltage[4].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.B_AM1_Dec, Selected_Band_gray_Gamma_Voltage[3].double_B, Selected_Band_gray_Gamma[4].int_B, 4);

            Selected_Band_gray_Gamma_Voltage[5].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.R_AM1_Dec, Selected_Band_gray_Gamma_Voltage[4].double_R, Selected_Band_gray_Gamma[5].int_R, 5);
            Selected_Band_gray_Gamma_Voltage[5].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.G_AM1_Dec, Selected_Band_gray_Gamma_Voltage[4].double_G, Selected_Band_gray_Gamma[5].int_G, 5);
            Selected_Band_gray_Gamma_Voltage[5].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.B_AM1_Dec, Selected_Band_gray_Gamma_Voltage[4].double_B, Selected_Band_gray_Gamma[5].int_B, 5);

            Selected_Band_gray_Gamma_Voltage[6].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.R_AM1_Dec, Selected_Band_gray_Gamma_Voltage[5].double_R, Selected_Band_gray_Gamma[6].int_R, 6);
            Selected_Band_gray_Gamma_Voltage[6].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.G_AM1_Dec, Selected_Band_gray_Gamma_Voltage[5].double_G, Selected_Band_gray_Gamma[6].int_G, 6);
            Selected_Band_gray_Gamma_Voltage[6].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.B_AM1_Dec, Selected_Band_gray_Gamma_Voltage[5].double_B, Selected_Band_gray_Gamma[6].int_B, 6);

            Selected_Band_gray_Gamma_Voltage[7].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.R_AM1_Dec, Selected_Band_gray_Gamma_Voltage[6].double_R, Selected_Band_gray_Gamma[7].int_R, 7);
            Selected_Band_gray_Gamma_Voltage[7].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.G_AM1_Dec, Selected_Band_gray_Gamma_Voltage[6].double_G, Selected_Band_gray_Gamma[7].int_G, 7);
            Selected_Band_gray_Gamma_Voltage[7].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.B_AM1_Dec, Selected_Band_gray_Gamma_Voltage[6].double_B, Selected_Band_gray_Gamma[7].int_B, 7);

            //G1(GR0) 
            Selected_Band_gray_Gamma_Voltage[8].double_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.R_AM1_Dec, Selected_Band_gray_Gamma_Voltage[6].double_R, Selected_Band_gray_Gamma[8].int_R, 7);
            Selected_Band_gray_Gamma_Voltage[8].double_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.G_AM1_Dec, Selected_Band_gray_Gamma_Voltage[6].double_G, Selected_Band_gray_Gamma[8].int_G, 7);
            Selected_Band_gray_Gamma_Voltage[8].double_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Model.B_AM1_Dec, Selected_Band_gray_Gamma_Voltage[6].double_B, Selected_Band_gray_Gamma[8].int_B, 7);

            for (int gray = 0; gray < 9; gray++)
            {
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[4].Value = Selected_Band_gray_Gamma_Voltage[gray].double_R.ToString();//R
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[5].Value = Selected_Band_gray_Gamma_Voltage[gray].double_G.ToString();//G
                dataGridView_RGB_Vdata_Read.Rows[gray + 1].Cells[6].Value = Selected_Band_gray_Gamma_Voltage[gray].double_B.ToString();//B
            }
        }

        private void button_Res_Test_Gray191_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.dataGridView2_Rows_Add("G191");
            int Selected_Gray = 1;
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            f1.PTN_update(191, 191, 191); Thread.Sleep(500);
            Offset_Measure(Selected_Gray, Offset);
        }

        private void button_Res_Test_Gray127_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.dataGridView2_Rows_Add("127");
            int Selected_Gray = 2;
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            f1.PTN_update(127, 127, 127); Thread.Sleep(500);
            Offset_Measure(Selected_Gray, Offset);
        }

        private void button_Res_Test_Gray63_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.dataGridView2_Rows_Add("G63");
            int Selected_Gray = 3;
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            f1.PTN_update(63, 63, 63); Thread.Sleep(500);
            Offset_Measure(Selected_Gray, Offset);
        }

        private void button_Res_Test_Gray31_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.dataGridView2_Rows_Add("G31");
            int Selected_Gray = 4;
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            f1.PTN_update(31, 31, 31); Thread.Sleep(500);
            Offset_Measure(Selected_Gray, Offset);
        }

        private void button_Res_Test_Gray15_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.dataGridView2_Rows_Add("G15");
            int Selected_Gray = 5;
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            f1.PTN_update(15, 15, 15); Thread.Sleep(500);
            Offset_Measure(Selected_Gray, Offset);
        }

        private void button_Res_Test_Gray7_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.dataGridView2_Rows_Add("G7");
            int Selected_Gray = 6;
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            f1.PTN_update(7, 7, 7); Thread.Sleep(500);
            Offset_Measure(Selected_Gray, Offset);
        }

        private void button_Res_Test_Gray4_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.dataGridView2_Rows_Add("G4");
            int Selected_Gray = 7;
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047
            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            f1.PTN_update(4, 4, 4); Thread.Sleep(500);
            Offset_Measure(Selected_Gray, Offset);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button_Read_DP173_DBV_Setting.PerformClick();

            Optic_Compensation_Stop = false;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Offset = Convert.ToInt32(textBox_Resolution_Test_Offset.Text);
            button_RGB_Vdata_Read.PerformClick();//Get AM1_RGB, Vreg1_voltage, Voltage_VREG1_REF2047

            int Band = Get_Selected_Band_Of_RGB_Vdata_Read_Write();
            DP173_DBV_Setting(Band); Thread.Sleep(50);

            if (Optic_Compensation_Stop) return;
            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + "/G255", Color.Blue);
            f1.PTN_update(255, 255, 255); Thread.Sleep(500); f1.dataGridView2_Rows_Add("G255"); Offset_Measure(0, Offset);

            if (Optic_Compensation_Stop) return;
            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + "/G191", Color.Blue);
            f1.PTN_update(191, 191, 191); Thread.Sleep(500); f1.dataGridView2_Rows_Add("G191"); Offset_Measure(1, Offset);

            if (Optic_Compensation_Stop) return;
            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + "/G127", Color.Blue);
            f1.PTN_update(127, 127, 127); Thread.Sleep(500); f1.dataGridView2_Rows_Add("G127"); Offset_Measure(2, Offset);

            if (Optic_Compensation_Stop) return;
            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + "/G63", Color.Blue);
            f1.PTN_update(63, 63, 63); Thread.Sleep(500); f1.dataGridView2_Rows_Add("G63"); Offset_Measure(3, Offset);

            if (Optic_Compensation_Stop) return;
            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + "/G31", Color.Blue);
            f1.PTN_update(31, 31, 31); Thread.Sleep(500); f1.dataGridView2_Rows_Add("G31"); Offset_Measure(4, Offset);

            if (Optic_Compensation_Stop) return;
            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + "/G15", Color.Blue);
            f1.PTN_update(15, 15, 15); Thread.Sleep(500); f1.dataGridView2_Rows_Add("G15"); Offset_Measure(5, Offset);

            if (Optic_Compensation_Stop) return;
            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + "/G7", Color.Blue);
            f1.PTN_update(7, 7, 7); Thread.Sleep(500); f1.dataGridView2_Rows_Add("G7"); Offset_Measure(6, Offset);

            if (Optic_Compensation_Stop) return;
            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + "/G4", Color.Blue);
            f1.PTN_update(4, 4, 4); Thread.Sleep(500); f1.dataGridView2_Rows_Add("G4"); Offset_Measure(7, Offset);

            System.Windows.MessageBox.Show("All Gray Measure Finished");
        }

        //---------------------Dll Verify-----------------
        double[] Get_Previous_Band_Gamma_Voltage(int Dec_AM1, int Previous_Band_Vreg1_Dec, int[] Previous_Band_Gamma, double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1)
        {
            double[] Previous_Band_Gamma_Voltage = new double[8];
            double Previous_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Previous_Band_Vreg1_Dec, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);

            for (int gray = 0; gray < 8; gray++)
            {
                if (gray == 0) Previous_Band_Gamma_Voltage[gray] = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma[gray]);
                else Previous_Band_Gamma_Voltage[gray] = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Dec_AM1, Previous_Band_Gamma_Voltage[gray - 1], Previous_Band_Gamma[gray], gray);
            }
            return Previous_Band_Gamma_Voltage;
        }

        /*
        double DP173_Get_AM1_RGB_Voltage(double Voltage_VREG1_REF2047, double Vreg1_voltage, int Dec_AM1)
        {
            //AM1_RGB_Voltage = (REF2047_voltage + (Vreg1_voltage - REF2047_voltage) * (((AM1*2) + 2)/700); 
            return Voltage_VREG1_REF2047 + (Vreg1_voltage - Voltage_VREG1_REF2047) * (((Dec_AM1 * 2) + 8) / 700.0);//R_Voltage
        }
        */

        private void C_Sharp_DP173_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(int Vreg1, int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Target_Lv, double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1)
        {
            if (band >= 1 && Selected_Band[band] == true)
            {
                double[] Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_R, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                double[] Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_G, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                double[] Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_B, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);

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
                //Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
                //f1.GB_Status_AppendText_Nextline("C_G[0] , C_G[7] = " + C_G[0].ToString() + " , " + C_G[7].ToString(), Color.Blue);//Just For Debug, it can be deleted later (191113)
                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;

                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
                double Previous_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);

                double Actual_Previous_Vdata_Red = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);

                //Red
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0) Calculated_Vdata_Red = Vdata;
                }

                //Green
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
                }

                //Blue
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0) Calculated_Vdata_Blue = Vdata;
                }


                double Vreg1_voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Vreg1, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);//Verify OK (200205)

                //Got the Vreg1 
                //Need to get Gamma_R/B
                Gamma_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_G = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Green);
                Gamma_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Blue);

                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                f1.GB_Status_AppendText_Nextline("(2)After(C#,Precision : 0.001) Calculated_Vdata_Red/Calculated_Vdata_Green/Calculated_Vdata_Blue : " + Calculated_Vdata_Red.ToString() + "/" + Calculated_Vdata_Green.ToString() + "/" + Calculated_Vdata_Blue.ToString(), Color.Red);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }



        private void C_Sharp_DP173_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, ref int Vreg1_Dec_Init, ref int Gamma_R, ref int Gamma_B, bool[] Selected_Band, int[] Previous_Band_Gamma_Red, int[] Previous_Band_Gamma_Green, int[] Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double[] Previous_Band_Target_Lv, double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1)
        {
            if (band >= 1 && Selected_Band[band] == true)
            {
                double[] Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_R, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                double[] Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_G, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                double[] Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_B, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);

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
                //Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
                //f1.GB_Status_AppendText_Nextline("C_G[0] , C_G[7] = " + C_G[0].ToString() + " , " + C_G[7].ToString(), Color.Blue);//Just For Debug, it can be deleted later (191113)
                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Red = 0;
                double Calculated_Vdata_Green = 0;
                double Calculated_Vdata_Blue = 0;

                double Calculated_Target_Lv = 0;
                int iteration;

                //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
                double Previous_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);

                double Actual_Previous_Vdata_Red = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
                double Actual_Previous_Vdata_Green = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
                double Actual_Previous_Vdata_Blue = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);

                //Red
                for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0) Calculated_Vdata_Red = Vdata;
                }

                //Green
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
                }

                //Blue
                for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0) Calculated_Vdata_Blue = Vdata;
                }

                double Vreg1_voltage = Voltage_VREG1_REF2047 + ((Calculated_Vdata_Green - Voltage_VREG1_REF2047) * (700.0 / (Previous_Band_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                Vreg1_Dec_Init = Imported_my_cpp_dll.DP173_Get_Vreg1_Dec(Vreg1_voltage, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);

                //Got the Vreg1 
                //Need to get Gamma_R/B
                Gamma_R = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Red);
                Gamma_B = Imported_my_cpp_dll.DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Blue);

                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                f1.GB_Status_AppendText_Nextline("(2)After(C#,Precision : 0.001) Calculated_Vdata_Red/Calculated_Vdata_Green/Calculated_Vdata_Blue : " + Calculated_Vdata_Red.ToString() + "/" + Calculated_Vdata_Green.ToString() + "/" + Calculated_Vdata_Blue.ToString(), Color.Red);
            }
            else //Band0 + Other not selected Bands's
            {
                //Do nothing
            }
        }

        private void checkBox_ELVSS_Comp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ELVSS_Comp.Checked)
            {
                checkBox_ELVSS_VINIT2_Low_Temperature.Checked = true;
                checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Checked = true;
                checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Checked = true;
                checkBox_ELVSS_VINIT2_Low_Temperature.Enabled = true;
                checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Enabled = true;
                checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Enabled = true;
            }
            else
            {
                checkBox_ELVSS_VINIT2_Low_Temperature.Checked = false;
                checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Checked = false;
                checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Checked = false;
                checkBox_ELVSS_VINIT2_Low_Temperature.Enabled = false;
                checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Enabled = false;
                checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Enabled = false;
            }
        }
        private void button_Gamma_Set1_Apply_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Set_Condition_Mipi_Script_Send(Gamma_Set.Set1);
            int delay = Convert.ToInt32(textBox_delay_After_Set1.Text);
            Thread.Sleep(delay);
            f1.GB_Status_AppendText_Nextline("Set1 Thread delay " + delay.ToString() + " was applied", Status_Color_Set1);
        }

        private void button_Gamma_Set2_Apply_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Set_Condition_Mipi_Script_Send(Gamma_Set.Set2);
            int delay = Convert.ToInt32(textBox_delay_After_Set2.Text);
            Thread.Sleep(delay);
            f1.GB_Status_AppendText_Nextline("Set2 Thread delay " + delay.ToString() + " was applied", Status_Color_Set2);
        }

        private void button_Gamma_Set3_Apply_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Set_Condition_Mipi_Script_Send(Gamma_Set.Set3);
            int delay = Convert.ToInt32(textBox_delay_After_Set3.Text);
            Thread.Sleep(delay);
            f1.GB_Status_AppendText_Nextline("Set3 Thread delay " + delay.ToString() + " was applied", Status_Color_Set3);
        }

        private void button_Gamma_Set4_Apply_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Set_Condition_Mipi_Script_Send(Gamma_Set.Set4);
            int delay = Convert.ToInt32(textBox_delay_After_Set4.Text);
            Thread.Sleep(delay);
            f1.GB_Status_AppendText_Nextline("Set4 Thread delay " + delay.ToString() + " was applied", Status_Color_Set4);
        }

        private void button_Gamma_Set5_Apply_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Set_Condition_Mipi_Script_Send(Gamma_Set.Set5);
            int delay = Convert.ToInt32(textBox_delay_After_Set5.Text);
            Thread.Sleep(delay);
            f1.GB_Status_AppendText_Nextline("Set5 Thread delay " + delay.ToString() + " was applied", Status_Color_Set5);
        }

        private void button_Gamma_Set6_Apply_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Set_Condition_Mipi_Script_Send(Gamma_Set.Set6);
            int delay = Convert.ToInt32(textBox_delay_After_Set6.Text);
            Thread.Sleep(delay);
            f1.GB_Status_AppendText_Nextline("Set5 Thread delay " + delay.ToString() + " was applied", Status_Color_Set6);
        }


        private void Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_All_Band_3points_Combine_Points(int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[] Band_Gray_Gamma_Red, int[] Band_Gray_Gamma_Green, int[] Band_Gray_Gamma_Blue, double[] Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1
            , double Fx_3points_Combine_LV_1, double Fx_3points_Combine_LV_2, double Fx_3points_Combine_LV_3, double Fx_3points_Combine_Lv_Distance)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            SJH_Matrix M = new SJH_Matrix();

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                double[][] Band_Gray_Gamma_Red_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Green_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Blue_Voltage = M.MatrixCreate(band, 8);


                for (int i = 0; i < band; i++)
                {
                    int[] Previous_Band_Gamma_R = new int[8];
                    int[] Previous_Band_Gamma_G = new int[8];
                    int[] Previous_Band_Gamma_B = new int[8];

                    for (int g = 0; g < 8; g++)
                    {
                        Previous_Band_Gamma_R[g] = Band_Gray_Gamma_Red[(8 * i) + g];
                        Previous_Band_Gamma_G[g] = Band_Gray_Gamma_Green[(8 * i) + g];
                        Previous_Band_Gamma_B[g] = Band_Gray_Gamma_Blue[(8 * i) + g];
                    }

                    //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
                    Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_R, Band_Vreg1_Dec[i], Previous_Band_Gamma_R, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                    Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_G, Band_Vreg1_Dec[i], Previous_Band_Gamma_G, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                    Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_B, Band_Vreg1_Dec[i], Previous_Band_Gamma_B, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
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
                        Target_Lv_Points[(8 * b) + g] = Band_Gray_Target_Lv[(8 * b) + g];
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

                double Diff_Lv = 0;
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
                        if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_1)
                        {
                            Temp_R_1.Add(Gamma_Red_Voltage_Points[(8 * b) + g]);
                            Temp_G_1.Add(Gamma_Green_Voltage_Points[(8 * b) + g]);
                            Temp_B_1.Add(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                            Temp_Target_Lv_1.Add(Target_Lv_Points[(8 * b) + g]);
                        }
                        // A <= X < B (Region 2)
                        else if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_2)
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
                            f1.GB_Status_AppendText_Nextline("Diff_Lv < Fx_3points_Combine_Lv_Distance", Color.Blue, true);
                            Gamma_Red_Voltage_Points_Rearrange.Add(Gamma_Red_Voltage_Points[(8 * b) + g]);
                            Gamma_Green_Voltage_Points_Rearrange.Add(Gamma_Green_Voltage_Points[(8 * b) + g]);
                            Gamma_Blue_Voltage_Points_Rearrange.Add(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                            Target_Lv_Points_Rearrange.Add(Target_Lv_Points[(8 * b) + g]);

                            if (((8 * b) + g) < ((8 * (band - 1)) + (8 - 1))) //if it's not the last point
                            {
                                Diff_Lv = Math.Abs(Target_Lv_Points[(8 * b) + g] - Target_Lv_Points[(8 * b) + (g + 1)]);
                                f1.GB_Status_AppendText_Nextline("Diff_Lv/Fx_3points_Combine_Lv_Distance : " + Diff_Lv.ToString() + "/" + Fx_3points_Combine_Lv_Distance.ToString(), Color.Blue, true);
                                if (Diff_Lv < Fx_3points_Combine_Lv_Distance)
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

                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
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

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Band_Vreg1_Dec[band], Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                Gamma_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_R, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata, gray);
                Gamma_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_G, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata, gray);
                Gamma_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_B, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata, gray);
            }
        }


        private void Get_First_Gamma_Fx_HBM_Mode_C_Sharp_Dll_Test_All_Band_3points_Combine_Points_2(double Combine_Lv_Ratio, int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, bool[] Selected_Band, int[] Band_Gray_Gamma_Red, int[] Band_Gray_Gamma_Green, int[] Band_Gray_Gamma_Blue, double[] Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int[] Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1
            , double Fx_3points_Combine_LV_1, double Fx_3points_Combine_LV_2, double Fx_3points_Combine_LV_3, double Fx_3points_Combine_Lv_Distance)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            SJH_Matrix M = new SJH_Matrix();

            if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
            {
                double[][] Band_Gray_Gamma_Red_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Green_Voltage = M.MatrixCreate(band, 8);
                double[][] Band_Gray_Gamma_Blue_Voltage = M.MatrixCreate(band, 8);


                for (int i = 0; i < band; i++)
                {
                    int[] Previous_Band_Gamma_R = new int[8];
                    int[] Previous_Band_Gamma_G = new int[8];
                    int[] Previous_Band_Gamma_B = new int[8];

                    for (int g = 0; g < 8; g++)
                    {
                        Previous_Band_Gamma_R[g] = Band_Gray_Gamma_Red[(8 * i) + g];
                        Previous_Band_Gamma_G[g] = Band_Gray_Gamma_Green[(8 * i) + g];
                        Previous_Band_Gamma_B[g] = Band_Gray_Gamma_Blue[(8 * i) + g];
                    }

                    //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
                    Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_R, Band_Vreg1_Dec[i], Previous_Band_Gamma_R, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                    Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_G, Band_Vreg1_Dec[i], Previous_Band_Gamma_G, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                    Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_B, Band_Vreg1_Dec[i], Previous_Band_Gamma_B, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
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
                        Target_Lv_Points[(8 * b) + g] = Band_Gray_Target_Lv[(8 * b) + g];
                    }
                }

                Array.Sort<double>(Gamma_Red_Voltage_Points);
                Array.Sort<double>(Gamma_Green_Voltage_Points);
                Array.Sort<double>(Gamma_Blue_Voltage_Points);
                Array.Sort<double>(Target_Lv_Points);
                Array.Reverse(Target_Lv_Points);


                //-----------------Added On 200311-------------------
                List<double> Combinded_Gamma_Red_Voltage_Points = new List<double>();
                List<double> Combinded_Gamma_Green_Voltage_Points = new List<double>();
                List<double> Combinded_Gamma_Blue_Voltage_Points = new List<double>();
                List<double> Combinded_Target_Lv_Points = new List<double>();

                for (int b = 0; b < band; b++)
                {
                    for (int g = 0; g < 8; g++)
                    {
                        if (b == 0 && g == 0)//First Point
                        {
                            f1.GB_Status_AppendText_Nextline("-----------Apply This Point !---------", Color.Blue);
                            Combinded_Gamma_Red_Voltage_Points.Add(Gamma_Red_Voltage_Points[(8 * b) + g]);
                            Combinded_Gamma_Green_Voltage_Points.Add(Gamma_Green_Voltage_Points[(8 * b) + g]);
                            Combinded_Gamma_Blue_Voltage_Points.Add(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                            Combinded_Target_Lv_Points.Add(Target_Lv_Points[(8 * b) + g]);
                        }
                        else
                        {
                            double Abs_Diff_Lv_Between_Two_Points = Math.Abs(Combinded_Target_Lv_Points[Combinded_Target_Lv_Points.Count() - 1] - Target_Lv_Points[(8 * b) + g]);
                            double Threshold_Lv = (Target_Lv_Points[(8 * b) + g] * Combine_Lv_Ratio);


                            if (Abs_Diff_Lv_Between_Two_Points > Threshold_Lv)
                            {
                                f1.GB_Status_AppendText_Nextline("-----------Apply This Point !---------", Color.Blue);
                                Combinded_Gamma_Red_Voltage_Points.Add(Gamma_Red_Voltage_Points[(8 * b) + g]);
                                Combinded_Gamma_Green_Voltage_Points.Add(Gamma_Green_Voltage_Points[(8 * b) + g]);
                                Combinded_Gamma_Blue_Voltage_Points.Add(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                                Combinded_Target_Lv_Points.Add(Target_Lv_Points[(8 * b) + g]);
                            }
                            else
                            {
                                f1.GB_Status_AppendText_Nextline("-----------Skip This Point !---------", Color.Red);
                                f1.GB_Status_AppendText_Nextline("Combinded_Target_Lv_Points[Combinded_Target_Lv_Points.Count() - 1] : " + Combinded_Target_Lv_Points[Combinded_Target_Lv_Points.Count() - 1].ToString(), Color.DarkGreen);
                                f1.GB_Status_AppendText_Nextline("Target_Lv_Points[(8 * b) + g] : " + Target_Lv_Points[(8 * b) + g].ToString(), Color.DarkGreen);
                                f1.GB_Status_AppendText_Nextline("Abs_Diff_Lv_Between_Two_Points : " + Abs_Diff_Lv_Between_Two_Points.ToString(), Color.DarkGreen);
                                f1.GB_Status_AppendText_Nextline("Threshold_Lv : " + Threshold_Lv.ToString(), Color.DarkGreen);
                            }
                        }
                    }
                }
                //----------------------------------------------------


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

                for (int points = 0; points < Combinded_Target_Lv_Points.Count(); points++)
                {
                    //Lv
                    // X < A (Region 1)
                    if (Combinded_Target_Lv_Points[points] < Fx_3points_Combine_LV_1)
                    {
                        Temp_R_1.Add(Combinded_Gamma_Red_Voltage_Points[points]);
                        Temp_G_1.Add(Combinded_Gamma_Green_Voltage_Points[points]);
                        Temp_B_1.Add(Combinded_Gamma_Blue_Voltage_Points[points]);
                        Temp_Target_Lv_1.Add(Combinded_Target_Lv_Points[points]);
                    }
                    // A <= X < B (Region 2)
                    else if (Combinded_Target_Lv_Points[points] < Fx_3points_Combine_LV_2)
                    {
                        Temp_R_2.Add(Combinded_Gamma_Red_Voltage_Points[points]);
                        Temp_G_2.Add(Combinded_Gamma_Green_Voltage_Points[points]);
                        Temp_B_2.Add(Combinded_Gamma_Blue_Voltage_Points[points]);
                        Temp_Target_Lv_2.Add(Combinded_Target_Lv_Points[points]);
                    }
                    // B <= X < C (Region 3)
                    else if (Combinded_Target_Lv_Points[points] < Fx_3points_Combine_LV_3)
                    {
                        Temp_R_3.Add(Combinded_Gamma_Red_Voltage_Points[points]);
                        Temp_G_3.Add(Combinded_Gamma_Green_Voltage_Points[points]);
                        Temp_B_3.Add(Combinded_Gamma_Blue_Voltage_Points[points]);
                        Temp_Target_Lv_3.Add(Combinded_Target_Lv_Points[points]);

                    }
                    // C <= X (Region 4)
                    else
                    {
                        Gamma_Red_Voltage_Points_Rearrange.Add(Combinded_Gamma_Red_Voltage_Points[points]);
                        Gamma_Green_Voltage_Points_Rearrange.Add(Combinded_Gamma_Green_Voltage_Points[points]);
                        Gamma_Blue_Voltage_Points_Rearrange.Add(Combinded_Gamma_Blue_Voltage_Points[points]);
                        Target_Lv_Points_Rearrange.Add(Combinded_Target_Lv_Points[points]);
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

                for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
                }

                for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
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

                for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
                }

                //Get Gamma_R/G/B From Calculated Voltage_R/G/B
                double Current_Band_Vreg1_Voltage = Imported_my_cpp_dll.DP173_Get_Vreg1_Voltage(Band_Vreg1_Dec[band], Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
                Gamma_R = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_R, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata, gray);
                Gamma_G = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_G, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata, gray);
                Gamma_B = Imported_my_cpp_dll.DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_B, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata, gray);
            }
        }

        private void radioButton_Dual_Mode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_Single_Mode_Set2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ELVSS_B2_Offset_Set2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_Save_Setting_Click(object sender, EventArgs e)
        {
            //------Get Setting Here------
            EA9154_Preferences up = new EA9154_Preferences();

            //Set1 ELVSS Offset
            up.ELVSS_Offset_Set1[0] = ELVSS_B0_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[1] = ELVSS_B1_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[2] = ELVSS_B2_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[3] = ELVSS_B3_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[4] = ELVSS_B4_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[5] = ELVSS_B5_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[6] = ELVSS_B6_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[7] = ELVSS_B7_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[8] = ELVSS_B8_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[9] = ELVSS_B9_Offset_Set1.Text;
            up.ELVSS_Offset_Set1[10] = ELVSS_B10_Offset_Set1.Text;

            //Set2 ELVSS Offset
            up.ELVSS_Offset_Set2[0] = ELVSS_B0_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[1] = ELVSS_B1_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[2] = ELVSS_B2_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[3] = ELVSS_B3_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[4] = ELVSS_B4_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[5] = ELVSS_B5_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[6] = ELVSS_B6_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[7] = ELVSS_B7_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[8] = ELVSS_B8_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[9] = ELVSS_B9_Offset_Set2.Text;
            up.ELVSS_Offset_Set2[10] = ELVSS_B10_Offset_Set2.Text;

            //Set3 ELVSS Offset
            up.ELVSS_Offset_Set3[0] = ELVSS_B0_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[1] = ELVSS_B1_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[2] = ELVSS_B2_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[3] = ELVSS_B3_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[4] = ELVSS_B4_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[5] = ELVSS_B5_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[6] = ELVSS_B6_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[7] = ELVSS_B7_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[8] = ELVSS_B8_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[9] = ELVSS_B9_Offset_Set3.Text;
            up.ELVSS_Offset_Set3[10] = ELVSS_B10_Offset_Set3.Text;

            //Set4 ELVSS Offset
            up.ELVSS_Offset_Set4[0] = ELVSS_B0_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[1] = ELVSS_B1_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[2] = ELVSS_B2_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[3] = ELVSS_B3_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[4] = ELVSS_B4_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[5] = ELVSS_B5_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[6] = ELVSS_B6_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[7] = ELVSS_B7_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[8] = ELVSS_B8_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[9] = ELVSS_B9_Offset_Set4.Text;
            up.ELVSS_Offset_Set4[10] = ELVSS_B10_Offset_Set4.Text;

            //Set5 ELVSS Offset
            up.ELVSS_Offset_Set5[0] = ELVSS_B0_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[1] = ELVSS_B1_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[2] = ELVSS_B2_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[3] = ELVSS_B3_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[4] = ELVSS_B4_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[5] = ELVSS_B5_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[6] = ELVSS_B6_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[7] = ELVSS_B7_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[8] = ELVSS_B8_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[9] = ELVSS_B9_Offset_Set5.Text;
            up.ELVSS_Offset_Set5[10] = ELVSS_B10_Offset_Set5.Text;

            //Set6 ELVSS Offset
            up.ELVSS_Offset_Set6[0] = ELVSS_B0_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[1] = ELVSS_B1_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[2] = ELVSS_B2_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[3] = ELVSS_B3_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[4] = ELVSS_B4_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[5] = ELVSS_B5_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[6] = ELVSS_B6_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[7] = ELVSS_B7_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[8] = ELVSS_B8_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[9] = ELVSS_B9_Offset_Set6.Text;
            up.ELVSS_Offset_Set6[10] = ELVSS_B10_Offset_Set6.Text;

            //Set1 Vinit2 Offset
            up.Vinit_Offset_Set1[0] = Vinit_Offset_B0_Set1.Text;
            up.Vinit_Offset_Set1[1] = Vinit_Offset_B1_Set1.Text;
            up.Vinit_Offset_Set1[2] = Vinit_Offset_B2_Set1.Text;
            up.Vinit_Offset_Set1[3] = Vinit_Offset_B3_Set1.Text;
            up.Vinit_Offset_Set1[4] = Vinit_Offset_B4_Set1.Text;
            up.Vinit_Offset_Set1[5] = Vinit_Offset_B5_Set1.Text;
            up.Vinit_Offset_Set1[6] = Vinit_Offset_B6_Set1.Text;
            up.Vinit_Offset_Set1[7] = Vinit_Offset_B7_Set1.Text;
            up.Vinit_Offset_Set1[8] = Vinit_Offset_B8_Set1.Text;
            up.Vinit_Offset_Set1[9] = Vinit_Offset_B9_Set1.Text;
            up.Vinit_Offset_Set1[10] = Vinit_Offset_B10_Set1.Text;

            //Set2 Vinit2 Offset
            up.Vinit_Offset_Set2[0] = Vinit_Offset_B0_Set2.Text;
            up.Vinit_Offset_Set2[1] = Vinit_Offset_B1_Set2.Text;
            up.Vinit_Offset_Set2[2] = Vinit_Offset_B2_Set2.Text;
            up.Vinit_Offset_Set2[3] = Vinit_Offset_B3_Set2.Text;
            up.Vinit_Offset_Set2[4] = Vinit_Offset_B4_Set2.Text;
            up.Vinit_Offset_Set2[5] = Vinit_Offset_B5_Set2.Text;
            up.Vinit_Offset_Set2[6] = Vinit_Offset_B6_Set2.Text;
            up.Vinit_Offset_Set2[7] = Vinit_Offset_B7_Set2.Text;
            up.Vinit_Offset_Set2[8] = Vinit_Offset_B8_Set2.Text;
            up.Vinit_Offset_Set2[9] = Vinit_Offset_B9_Set2.Text;
            up.Vinit_Offset_Set2[10] = Vinit_Offset_B10_Set2.Text;

            //Set3 Vinit2 Offset
            up.Vinit_Offset_Set3[0] = Vinit_Offset_B0_Set3.Text;
            up.Vinit_Offset_Set3[1] = Vinit_Offset_B1_Set3.Text;
            up.Vinit_Offset_Set3[2] = Vinit_Offset_B2_Set3.Text;
            up.Vinit_Offset_Set3[3] = Vinit_Offset_B3_Set3.Text;
            up.Vinit_Offset_Set3[4] = Vinit_Offset_B4_Set3.Text;
            up.Vinit_Offset_Set3[5] = Vinit_Offset_B5_Set3.Text;
            up.Vinit_Offset_Set3[6] = Vinit_Offset_B6_Set3.Text;
            up.Vinit_Offset_Set3[7] = Vinit_Offset_B7_Set3.Text;
            up.Vinit_Offset_Set3[8] = Vinit_Offset_B8_Set3.Text;
            up.Vinit_Offset_Set3[9] = Vinit_Offset_B9_Set3.Text;
            up.Vinit_Offset_Set3[10] = Vinit_Offset_B10_Set3.Text;

            //Set4 Vinit2 Offset
            up.Vinit_Offset_Set4[0] = Vinit_Offset_B0_Set4.Text;
            up.Vinit_Offset_Set4[1] = Vinit_Offset_B1_Set4.Text;
            up.Vinit_Offset_Set4[2] = Vinit_Offset_B2_Set4.Text;
            up.Vinit_Offset_Set4[3] = Vinit_Offset_B3_Set4.Text;
            up.Vinit_Offset_Set4[4] = Vinit_Offset_B4_Set4.Text;
            up.Vinit_Offset_Set4[5] = Vinit_Offset_B5_Set4.Text;
            up.Vinit_Offset_Set4[6] = Vinit_Offset_B6_Set4.Text;
            up.Vinit_Offset_Set4[7] = Vinit_Offset_B7_Set4.Text;
            up.Vinit_Offset_Set4[8] = Vinit_Offset_B8_Set4.Text;
            up.Vinit_Offset_Set4[9] = Vinit_Offset_B9_Set4.Text;
            up.Vinit_Offset_Set4[10] = Vinit_Offset_B10_Set4.Text;

            //Set5 Vinit2 Offset
            up.Vinit_Offset_Set5[0] = Vinit_Offset_B0_Set5.Text;
            up.Vinit_Offset_Set5[1] = Vinit_Offset_B1_Set5.Text;
            up.Vinit_Offset_Set5[2] = Vinit_Offset_B2_Set5.Text;
            up.Vinit_Offset_Set5[3] = Vinit_Offset_B3_Set5.Text;
            up.Vinit_Offset_Set5[4] = Vinit_Offset_B4_Set5.Text;
            up.Vinit_Offset_Set5[5] = Vinit_Offset_B5_Set5.Text;
            up.Vinit_Offset_Set5[6] = Vinit_Offset_B6_Set5.Text;
            up.Vinit_Offset_Set5[7] = Vinit_Offset_B7_Set5.Text;
            up.Vinit_Offset_Set5[8] = Vinit_Offset_B8_Set5.Text;
            up.Vinit_Offset_Set5[9] = Vinit_Offset_B9_Set5.Text;
            up.Vinit_Offset_Set5[10] = Vinit_Offset_B10_Set5.Text;

            //Set6 Vinit2 Offset
            up.Vinit_Offset_Set6[0] = Vinit_Offset_B0_Set6.Text;
            up.Vinit_Offset_Set6[1] = Vinit_Offset_B1_Set6.Text;
            up.Vinit_Offset_Set6[2] = Vinit_Offset_B2_Set6.Text;
            up.Vinit_Offset_Set6[3] = Vinit_Offset_B3_Set6.Text;
            up.Vinit_Offset_Set6[4] = Vinit_Offset_B4_Set6.Text;
            up.Vinit_Offset_Set6[5] = Vinit_Offset_B5_Set6.Text;
            up.Vinit_Offset_Set6[6] = Vinit_Offset_B6_Set6.Text;
            up.Vinit_Offset_Set6[7] = Vinit_Offset_B7_Set6.Text;
            up.Vinit_Offset_Set6[8] = Vinit_Offset_B8_Set6.Text;
            up.Vinit_Offset_Set6[9] = Vinit_Offset_B9_Set6.Text;
            up.Vinit_Offset_Set6[10] = Vinit_Offset_B10_Set6.Text;

            up.checkBox_ELVSS_Comp = checkBox_ELVSS_Comp.Checked;
            up.checkBox_ELVSS_VINIT2_Low_Temperature = checkBox_ELVSS_VINIT2_Low_Temperature.Checked;
            up.checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5 = checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Checked;
            up.checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6 = checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Checked;

            up.textBox_ELVSS_Margin = textBox_ELVSS_Margin.Text;
            up.textBox_ELVSS_CMD_Delay = textBox_ELVSS_CMD_Delay.Text;
            up.textBox_Slope_Margin = textBox_Slope_Margin.Text;
            up.textBox_ELVSS_Min = textBox_ELVSS_Min.Text;
            up.textBox_ELVSS_Max = textBox_ELVSS_Max.Text;

            up.radioButton_ELVSS_Start_From_Band0_First_ELVSS_60 = radioButton_ELVSS_Start_From_Band0_First_ELVSS_60.Checked;
            up.radioButton_ELVSS_Start_From_Band1_First_ELVSS_45 = radioButton_ELVSS_Start_From_Band1_First_ELVSS_45.Checked;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string Path = string.Empty;

            if (f1.current_model.Get_Current_Model_Name() == Model_Name.Elgin)
                Path = Directory.GetCurrentDirectory() + "\\Elgin\\Prefs.xml";
            else if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP173)
                Path = Directory.GetCurrentDirectory() + "\\DP173\\Prefs.xml";

            StreamWriter myWriter = new StreamWriter(Path);
            mySerializer.Serialize(myWriter, up);
            System.Windows.Forms.MessageBox.Show("Settings have been saved");
            myWriter.Close();
        }

        public void Load_Setting_Perform_Click()
        {
            button_Load_Setting.PerformClick();

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Model_Name currunt_model = f1.current_model.Get_Current_Model_Name();
            if (currunt_model == Model_Name.Elgin) Elgin_UI_Setting_For_Dual_Compensation();
            else if (currunt_model == Model_Name.DP173) DP173_UI_Setting_For_Dual_Compensation();
        }

        private void button_Load_Setting_Click(object sender, EventArgs e)
        {
            //------Set Setting Here------
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string Path = string.Empty;

            if (f1.current_model.Get_Current_Model_Name() == Model_Name.Elgin)
                Path = Directory.GetCurrentDirectory() + "\\Elgin\\Prefs.xml";
            else if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP173)
                Path = Directory.GetCurrentDirectory() + "\\DP173\\Prefs.xml";

            FileStream myFileStream = new FileStream(Path, FileMode.Open);//Used For Loading Setting
            EA9154_Preferences up = (EA9154_Preferences)mySerializer.Deserialize(myFileStream);

            //Set1 ELVSS Offset
            ELVSS_B0_Offset_Set1.Text = up.ELVSS_Offset_Set1[0];
            ELVSS_B1_Offset_Set1.Text = up.ELVSS_Offset_Set1[1];
            ELVSS_B2_Offset_Set1.Text = up.ELVSS_Offset_Set1[2];
            ELVSS_B3_Offset_Set1.Text = up.ELVSS_Offset_Set1[3];
            ELVSS_B4_Offset_Set1.Text = up.ELVSS_Offset_Set1[4];
            ELVSS_B5_Offset_Set1.Text = up.ELVSS_Offset_Set1[5];
            ELVSS_B6_Offset_Set1.Text = up.ELVSS_Offset_Set1[6];
            ELVSS_B7_Offset_Set1.Text = up.ELVSS_Offset_Set1[7];
            ELVSS_B8_Offset_Set1.Text = up.ELVSS_Offset_Set1[8];
            ELVSS_B9_Offset_Set1.Text = up.ELVSS_Offset_Set1[9];
            ELVSS_B10_Offset_Set1.Text = up.ELVSS_Offset_Set1[10];

            //Set2 ELVSS Offset
            ELVSS_B0_Offset_Set2.Text = up.ELVSS_Offset_Set2[0];
            ELVSS_B1_Offset_Set2.Text = up.ELVSS_Offset_Set2[1];
            ELVSS_B2_Offset_Set2.Text = up.ELVSS_Offset_Set2[2];
            ELVSS_B3_Offset_Set2.Text = up.ELVSS_Offset_Set2[3];
            ELVSS_B4_Offset_Set2.Text = up.ELVSS_Offset_Set2[4];
            ELVSS_B5_Offset_Set2.Text = up.ELVSS_Offset_Set2[5];
            ELVSS_B6_Offset_Set2.Text = up.ELVSS_Offset_Set2[6];
            ELVSS_B7_Offset_Set2.Text = up.ELVSS_Offset_Set2[7];
            ELVSS_B8_Offset_Set2.Text = up.ELVSS_Offset_Set2[8];
            ELVSS_B9_Offset_Set2.Text = up.ELVSS_Offset_Set2[9];
            ELVSS_B10_Offset_Set2.Text = up.ELVSS_Offset_Set2[10];

            //Set3 ELVSS Offset
            ELVSS_B0_Offset_Set3.Text = up.ELVSS_Offset_Set3[0];
            ELVSS_B1_Offset_Set3.Text = up.ELVSS_Offset_Set3[1];
            ELVSS_B2_Offset_Set3.Text = up.ELVSS_Offset_Set3[2];
            ELVSS_B3_Offset_Set3.Text = up.ELVSS_Offset_Set3[3];
            ELVSS_B4_Offset_Set3.Text = up.ELVSS_Offset_Set3[4];
            ELVSS_B5_Offset_Set3.Text = up.ELVSS_Offset_Set3[5];
            ELVSS_B6_Offset_Set3.Text = up.ELVSS_Offset_Set3[6];
            ELVSS_B7_Offset_Set3.Text = up.ELVSS_Offset_Set3[7];
            ELVSS_B8_Offset_Set3.Text = up.ELVSS_Offset_Set3[8];
            ELVSS_B9_Offset_Set3.Text = up.ELVSS_Offset_Set3[9];
            ELVSS_B10_Offset_Set3.Text = up.ELVSS_Offset_Set3[10];

            //Set4 ELVSS Offset
            ELVSS_B0_Offset_Set4.Text = up.ELVSS_Offset_Set4[0];
            ELVSS_B1_Offset_Set4.Text = up.ELVSS_Offset_Set4[1];
            ELVSS_B2_Offset_Set4.Text = up.ELVSS_Offset_Set4[2];
            ELVSS_B3_Offset_Set4.Text = up.ELVSS_Offset_Set4[3];
            ELVSS_B4_Offset_Set4.Text = up.ELVSS_Offset_Set4[4];
            ELVSS_B5_Offset_Set4.Text = up.ELVSS_Offset_Set4[5];
            ELVSS_B6_Offset_Set4.Text = up.ELVSS_Offset_Set4[6];
            ELVSS_B7_Offset_Set4.Text = up.ELVSS_Offset_Set4[7];
            ELVSS_B8_Offset_Set4.Text = up.ELVSS_Offset_Set4[8];
            ELVSS_B9_Offset_Set4.Text = up.ELVSS_Offset_Set4[9];
            ELVSS_B10_Offset_Set4.Text = up.ELVSS_Offset_Set4[10];

            //Set5 ELVSS Offset
            ELVSS_B0_Offset_Set5.Text = up.ELVSS_Offset_Set5[0];
            ELVSS_B1_Offset_Set5.Text = up.ELVSS_Offset_Set5[1];
            ELVSS_B2_Offset_Set5.Text = up.ELVSS_Offset_Set5[2];
            ELVSS_B3_Offset_Set5.Text = up.ELVSS_Offset_Set5[3];
            ELVSS_B4_Offset_Set5.Text = up.ELVSS_Offset_Set5[4];
            ELVSS_B5_Offset_Set5.Text = up.ELVSS_Offset_Set5[5];
            ELVSS_B6_Offset_Set5.Text = up.ELVSS_Offset_Set5[6];
            ELVSS_B7_Offset_Set5.Text = up.ELVSS_Offset_Set5[7];
            ELVSS_B8_Offset_Set5.Text = up.ELVSS_Offset_Set5[8];
            ELVSS_B9_Offset_Set5.Text = up.ELVSS_Offset_Set5[9];
            ELVSS_B10_Offset_Set5.Text = up.ELVSS_Offset_Set5[10];

            //Set6 ELVSS Offset
            ELVSS_B0_Offset_Set6.Text = up.ELVSS_Offset_Set6[0];
            ELVSS_B1_Offset_Set6.Text = up.ELVSS_Offset_Set6[1];
            ELVSS_B2_Offset_Set6.Text = up.ELVSS_Offset_Set6[2];
            ELVSS_B3_Offset_Set6.Text = up.ELVSS_Offset_Set6[3];
            ELVSS_B4_Offset_Set6.Text = up.ELVSS_Offset_Set6[4];
            ELVSS_B5_Offset_Set6.Text = up.ELVSS_Offset_Set6[5];
            ELVSS_B6_Offset_Set6.Text = up.ELVSS_Offset_Set6[6];
            ELVSS_B7_Offset_Set6.Text = up.ELVSS_Offset_Set6[7];
            ELVSS_B8_Offset_Set6.Text = up.ELVSS_Offset_Set6[8];
            ELVSS_B9_Offset_Set6.Text = up.ELVSS_Offset_Set6[9];
            ELVSS_B10_Offset_Set6.Text = up.ELVSS_Offset_Set6[10];

            //Set1 Vinit2 Offset
            Vinit_Offset_B0_Set1.Text = up.Vinit_Offset_Set1[0];
            Vinit_Offset_B1_Set1.Text = up.Vinit_Offset_Set1[1];
            Vinit_Offset_B2_Set1.Text = up.Vinit_Offset_Set1[2];
            Vinit_Offset_B3_Set1.Text = up.Vinit_Offset_Set1[3];
            Vinit_Offset_B4_Set1.Text = up.Vinit_Offset_Set1[4];
            Vinit_Offset_B5_Set1.Text = up.Vinit_Offset_Set1[5];
            Vinit_Offset_B6_Set1.Text = up.Vinit_Offset_Set1[6];
            Vinit_Offset_B7_Set1.Text = up.Vinit_Offset_Set1[7];
            Vinit_Offset_B8_Set1.Text = up.Vinit_Offset_Set1[8];
            Vinit_Offset_B9_Set1.Text = up.Vinit_Offset_Set1[9];
            Vinit_Offset_B10_Set1.Text = up.Vinit_Offset_Set1[10];

            //Set2 Vinit2 Offset
            Vinit_Offset_B0_Set2.Text = up.Vinit_Offset_Set2[0];
            Vinit_Offset_B1_Set2.Text = up.Vinit_Offset_Set2[1];
            Vinit_Offset_B2_Set2.Text = up.Vinit_Offset_Set2[2];
            Vinit_Offset_B3_Set2.Text = up.Vinit_Offset_Set2[3];
            Vinit_Offset_B4_Set2.Text = up.Vinit_Offset_Set2[4];
            Vinit_Offset_B5_Set2.Text = up.Vinit_Offset_Set2[5];
            Vinit_Offset_B6_Set2.Text = up.Vinit_Offset_Set2[6];
            Vinit_Offset_B7_Set2.Text = up.Vinit_Offset_Set2[7];
            Vinit_Offset_B8_Set2.Text = up.Vinit_Offset_Set2[8];
            Vinit_Offset_B9_Set2.Text = up.Vinit_Offset_Set2[9];
            Vinit_Offset_B10_Set2.Text = up.Vinit_Offset_Set2[10];

            //Set3 Vinit2 Offset
            Vinit_Offset_B0_Set3.Text = up.Vinit_Offset_Set3[0];
            Vinit_Offset_B1_Set3.Text = up.Vinit_Offset_Set3[1];
            Vinit_Offset_B2_Set3.Text = up.Vinit_Offset_Set3[2];
            Vinit_Offset_B3_Set3.Text = up.Vinit_Offset_Set3[3];
            Vinit_Offset_B4_Set3.Text = up.Vinit_Offset_Set3[4];
            Vinit_Offset_B5_Set3.Text = up.Vinit_Offset_Set3[5];
            Vinit_Offset_B6_Set3.Text = up.Vinit_Offset_Set3[6];
            Vinit_Offset_B7_Set3.Text = up.Vinit_Offset_Set3[7];
            Vinit_Offset_B8_Set3.Text = up.Vinit_Offset_Set3[8];
            Vinit_Offset_B9_Set3.Text = up.Vinit_Offset_Set3[9];
            Vinit_Offset_B10_Set3.Text = up.Vinit_Offset_Set3[10];

            //Set4 Vinit2 Offset
            Vinit_Offset_B0_Set4.Text = up.Vinit_Offset_Set4[0];
            Vinit_Offset_B1_Set4.Text = up.Vinit_Offset_Set4[1];
            Vinit_Offset_B2_Set4.Text = up.Vinit_Offset_Set4[2];
            Vinit_Offset_B3_Set4.Text = up.Vinit_Offset_Set4[3];
            Vinit_Offset_B4_Set4.Text = up.Vinit_Offset_Set4[4];
            Vinit_Offset_B5_Set4.Text = up.Vinit_Offset_Set4[5];
            Vinit_Offset_B6_Set4.Text = up.Vinit_Offset_Set4[6];
            Vinit_Offset_B7_Set4.Text = up.Vinit_Offset_Set4[7];
            Vinit_Offset_B8_Set4.Text = up.Vinit_Offset_Set4[8];
            Vinit_Offset_B9_Set4.Text = up.Vinit_Offset_Set4[9];
            Vinit_Offset_B10_Set4.Text = up.Vinit_Offset_Set4[10];

            //Set5 Vinit2 Offset
            Vinit_Offset_B0_Set5.Text = up.Vinit_Offset_Set5[0];
            Vinit_Offset_B1_Set5.Text = up.Vinit_Offset_Set5[1];
            Vinit_Offset_B2_Set5.Text = up.Vinit_Offset_Set5[2];
            Vinit_Offset_B3_Set5.Text = up.Vinit_Offset_Set5[3];
            Vinit_Offset_B4_Set5.Text = up.Vinit_Offset_Set5[4];
            Vinit_Offset_B5_Set5.Text = up.Vinit_Offset_Set5[5];
            Vinit_Offset_B6_Set5.Text = up.Vinit_Offset_Set5[6];
            Vinit_Offset_B7_Set5.Text = up.Vinit_Offset_Set5[7];
            Vinit_Offset_B8_Set5.Text = up.Vinit_Offset_Set5[8];
            Vinit_Offset_B9_Set5.Text = up.Vinit_Offset_Set5[9];
            Vinit_Offset_B10_Set5.Text = up.Vinit_Offset_Set5[10];

            //Set6 Vinit2 Offset
            Vinit_Offset_B0_Set6.Text = up.Vinit_Offset_Set6[0];
            Vinit_Offset_B1_Set6.Text = up.Vinit_Offset_Set6[1];
            Vinit_Offset_B2_Set6.Text = up.Vinit_Offset_Set6[2];
            Vinit_Offset_B3_Set6.Text = up.Vinit_Offset_Set6[3];
            Vinit_Offset_B4_Set6.Text = up.Vinit_Offset_Set6[4];
            Vinit_Offset_B5_Set6.Text = up.Vinit_Offset_Set6[5];
            Vinit_Offset_B6_Set6.Text = up.Vinit_Offset_Set6[6];
            Vinit_Offset_B7_Set6.Text = up.Vinit_Offset_Set6[7];
            Vinit_Offset_B8_Set6.Text = up.Vinit_Offset_Set6[8];
            Vinit_Offset_B9_Set6.Text = up.Vinit_Offset_Set6[9];
            Vinit_Offset_B10_Set6.Text = up.Vinit_Offset_Set6[10];

            checkBox_ELVSS_Comp.Checked = up.checkBox_ELVSS_Comp;
            checkBox_ELVSS_VINIT2_Low_Temperature.Checked = up.checkBox_ELVSS_VINIT2_Low_Temperature;
            checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5.Checked = up.checkBox_ELVSS_Vinit2_Copy_From_Set1_to_Set5;
            checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6.Checked = up.checkBox_ELVSS_Vinit2_Copy_From_Set2_to_Set6;

            textBox_ELVSS_Margin.Text = up.textBox_ELVSS_Margin;
            textBox_ELVSS_CMD_Delay.Text = up.textBox_ELVSS_CMD_Delay;
            textBox_Slope_Margin.Text = up.textBox_Slope_Margin;
            textBox_ELVSS_Min.Text = up.textBox_ELVSS_Min;
            textBox_ELVSS_Max.Text = up.textBox_ELVSS_Max;

            radioButton_ELVSS_Start_From_Band0_First_ELVSS_60.Checked = up.radioButton_ELVSS_Start_From_Band0_First_ELVSS_60;
            radioButton_ELVSS_Start_From_Band1_First_ELVSS_45.Checked = up.radioButton_ELVSS_Start_From_Band1_First_ELVSS_45;

            myFileStream.Close();
            System.Windows.Forms.MessageBox.Show("Settings have been Loaded");
        }

        private void radioButton_band1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox28_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton_AM0_0x00_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_AM0_0x00.Checked)
            {
                groupBox17.Hide();
            }
        }

        private void radioButton_Black_Compensation_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Black_Compensation.Checked)
            {
                groupBox17.Show();
            }
        }

        private void radioButton_Single_Mode_Set4_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void Update_REF_Voltages()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(21, 7, "B1"); ;
            string Hex_VREG1_REF1 = f1.dataGridView1.Rows[6].Cells[1].Value.ToString(); //White [6:0] (7bit)
            string Hex_VREG1_REF63 = f1.dataGridView1.Rows[5].Cells[1].Value.ToString(); //[5:0] (6bit)
            string Hex_VREG1_REF407 = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF815 = f1.dataGridView1.Rows[3].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF1227 = f1.dataGridView1.Rows[2].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF1635 = f1.dataGridView1.Rows[1].Cells[1].Value.ToString();//[5:0] (6bit)
            string Hex_VREG1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString(); //Black[6:0] (7bit)

            int Dec_VREG1_REF1 = Convert.ToInt32(Hex_VREG1_REF1, 16) & 0x7F;
            int Dec_VREG1_REF63 = Convert.ToInt32(Hex_VREG1_REF63, 16) & 0x3F;
            int Dec_VREG1_REF407 = Convert.ToInt32(Hex_VREG1_REF407, 16) & 0x3F;
            int Dec_VREG1_REF815 = Convert.ToInt32(Hex_VREG1_REF815, 16) & 0x3F;
            int Dec_VREG1_REF1227 = Convert.ToInt32(Hex_VREG1_REF1227, 16) & 0x3F;
            int Dec_VREG1_REF1635 = Convert.ToInt32(Hex_VREG1_REF1635, 16) & 0x3F;
            int Dec_VREG1_REF2047 = Convert.ToInt32(Hex_VREG1_REF2047, 16) & 0x7F;

            Imported_my_cpp_dll.Get_REF_Voltages(Dec_VREG1_REF2047, Dec_VREG1_REF1635, Dec_VREG1_REF1227, Dec_VREG1_REF815, Dec_VREG1_REF407, Dec_VREG1_REF63, Dec_VREG1_REF1, ref Voltage_VREG1_REF2047, ref  Voltage_VREG1_REF1635, ref  Voltage_VREG1_REF1227, ref  Voltage_VREG1_REF815, ref  Voltage_VREG1_REF407, ref  Voltage_VREG1_REF63, ref  Voltage_VREG1_REF1);
        }



        private void DP173_AOI_IRC_On()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x39 0xC0 0x00 0x09 0x00 0x00 0x09");//updated on 200317
        }



        private uint Get_LGOTP_CRC()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x09");
            f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x00");
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x09");
            f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x80");
            Thread.Sleep(30);

            //Add DDh's 14~17 parameters to Total_Hex_Checksum_LGOTP
            f1.MX_OTP_Read(13, 4, "DD"); //Read DDh's 14~17 parameters
            string[] Hex_Checksum_LGOTP = new string[4];
            string Total_Hex_Checksum_LGOTP = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                Hex_Checksum_LGOTP[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Total_Hex_Checksum_LGOTP += Hex_Checksum_LGOTP[i];
            }

            for (int i = 0; i < 4; i++) f1.GB_Status_AppendText_Nextline("Hex_Checksum_LGOTP[" + i.ToString() + "] : " + Hex_Checksum_LGOTP[i], Color.Blue);

            //Convert Total_Hex_Checksum_LGOTP into Decimal Value(Total_Dec_Checksum_LGOTP)
            uint Total_Dec_Checksum_LGOTP = Convert.ToUInt32(Total_Hex_Checksum_LGOTP, 16);
            f1.GB_Status_AppendText_Nextline("Total_Dec_Checksum_LGOTP : " + Total_Dec_Checksum_LGOTP.ToString(), Color.Blue);

            return Total_Dec_Checksum_LGOTP;
        }


        private void LGOTP_Flash_Erase_And_Write()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            //-----LGOTP Write-----
            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x04");
            f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0xAA");

            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x06");
            f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x01");

            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x09");
            f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x1F");
            Thread.Sleep(200);

            //----LGOTP Verify----
            f1.MX_OTP_Read(2, 1, "DD");//Read DDh's 3rd parameter
            string Flash_Status_Check = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            if (Flash_Status_Check == "00")
                f1.GB_Status_AppendText_Nextline("LGOTP Flash OC Writing OK.", Color.Green);
            else
                f1.GB_Status_AppendText_Nextline("LGOTP Flash OC Writing NG", Color.Red);
        }




        private void button_IRC_On_Flash_Write_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            uint Before_Total_Dec_Checksum_LGOTP = Get_LGOTP_CRC();

            f1.IPC_Quick_Send("mipi.write 0x39 0xC0 0x00 0x09 0x00 0x00 0x09");//IRC On (C0h)
            LGOTP_Flash_Erase_And_Write();
            Restart();
            uint After_Total_Dec_Checksum_LGOTP = Get_LGOTP_CRC();

            if (After_Total_Dec_Checksum_LGOTP == Before_Total_Dec_Checksum_LGOTP)
            {
                f1.GB_Status_AppendText_Nextline("LGOTP Flash OC Writing OK.", Color.Green);//CRC OK
            }
            else
            {
                f1.GB_Status_AppendText_Nextline("LGOTP Flash OC Writing OK.", Color.Green);//CRC NG
            }
        }

        private void Restart()
        {

        }

        private void radioButton_band2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_Single_Mode_Set1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_Single_Mode_Set3_CheckedChanged(object sender, EventArgs e)
        {

        }



        private void radioButton_ELVSS_Start_From_Band0_First_ELVSS_60_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton_ELVSS_Start_From_Band0_First_ELVSS_45_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox_Special_Gray_Compensation_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_ELVSS_Test_Set2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_GR1_EN_False_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_Triple_Gamma_Copy_Set1_to_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Triple_Gamma_Copy_Set1_to_Set3.Checked)
            {
                if (checkBox_Triple_Gamma_Copy_Set1_to_Set3.Checked)
                    checkBox_Triple_Gamma_Copy_Set2_to_Set3.Checked = false;
            }
        }

        private void checkBox_Triple_Gamma_Copy_Set2_to_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Triple_Gamma_Copy_Set2_to_Set3.Checked)
            {
                if (checkBox_Triple_Gamma_Copy_Set2_to_Set3.Checked)
                    checkBox_Triple_Gamma_Copy_Set1_to_Set3.Checked = false;
            }
        }

        private void DP173_Measure_Average(ref double Measured_X, ref double Measured_Y, ref double Measured_Lv, ref int total_average_loop_count
            , double Target_X, double Target_Y, double Target_Lv, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y)
        {
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
                f1().isMsr = true;
                f1().CA_Measure_button.Enabled = false;
                f1().objCa.Measure();
                //Diff_X = Math.Abs(Target_X - objProbe.sx);
                //Diff_Y = Math.Abs(Target_Y - objProbe.sy);
                //Diff_Lv = Math.Abs(Target_Lv - objProbe.Lv);
                Diff_X = Math.Abs(Target_X - f1().objCa.OutputProbes.get_ItemOfNumber(1).sx);
                Diff_Y = Math.Abs(Target_Y - f1().objCa.OutputProbes.get_ItemOfNumber(1).sy);
                Diff_Lv = Math.Abs(Target_Lv - f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv);

                Tolerance_X = Limit_X + Extension_X;
                Tolerance_Y = Limit_Y + Extension_Y;
                Tolerance_Lv = Limit_Lv;

                if (checkBox_Ave_Measure.Checked && f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(textBox_Ave_Lv_Limit.Text)
                    && ((Tolerance_X * Ratio) > Diff_X) && ((Tolerance_Y * Ratio) > Diff_Y) && ((Tolerance_Lv * Ratio) > Diff_Lv))
                {
                    f1().GB_Status_AppendText_Nextline(f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + textBox_Ave_Lv_Limit.Text, Color.Blue);
                    f1().GB_Status_AppendText_Nextline("Tolerance_X * Ratio : " + (Tolerance_X * Ratio).ToString() + " / Diff_X : " + Diff_X.ToString(), Color.Blue);
                    f1().GB_Status_AppendText_Nextline("Tolerance_Y * Ratio : " + (Tolerance_Y * Ratio).ToString() + " / Diff_X : " + Diff_Y.ToString(), Color.Blue);
                    f1().GB_Status_AppendText_Nextline("Tolerance_Lv * Ratio : " + (Tolerance_Lv * Ratio).ToString() + " / Diff_X : " + Diff_Lv.ToString(), Color.Blue);

                    for (int a = 0; a < 5; a++)
                    {
                        f1().objCa.Measure();
                        Measure[a].X = f1().objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Measure[a].Y = f1().objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Measure[a].Lv = f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

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
                    f1().GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                    f1().GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                    f1().GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                    f1().GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                    f1().GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                    f1().X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                    f1().Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                    f1().Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                    textBox_Total_Average_Meas_Count.Text = (++total_average_loop_count).ToString();
                }

                else
                {
                    f1().X_Value_display.Text = f1().objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    f1().Y_Value_display.Text = f1().objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    f1().Lv_Value_display.Text = f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                }

                Measured_X = Convert.ToDouble(f1().X_Value_display.Text);
                Measured_Y = Convert.ToDouble(f1().Y_Value_display.Text);
                Measured_Lv = Convert.ToDouble(f1().Lv_Value_display.Text);

                //Data Grid setting//////////////////////
                f1().dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                f1().dataGridView2.Rows.Add("-", f1().X_Value_display.Text, f1().Y_Value_display.Text, f1().Lv_Value_display.Text);
                f1().dataGridView2.FirstDisplayedScrollingRowIndex = f1().dataGridView2.RowCount - 1;

                System.Windows.Forms.Application.DoEvents();
                f1().CA_Measure_button.Enabled = true;
            }
            catch (Exception er)
            {
                f1().DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }



    }
}
