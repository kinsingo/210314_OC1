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
    public partial class First_Model_Option_Form : Form 
    {
        //Log For R/G/B/Vreg1 of 10 recent samples.
        public RGB[, ,] RGBVre1_Log_data = new RGB[12, 10, 10]; //12ea Bands , 10ea Gray-points , 10 Samples 
        public RGB[, ,] RGBVre1_Log_Offset_data = new RGB[12, 10, 10]; //12ea Bands , 10ea Gray-points , 10 Samples (Add on 190612)

        public string[][] RGBVre1_Log_Read_data;
        public string[][] RGBVre1_Log_Read_Offset_data;//(Add on 190612)

        bool DP116_Or_DP150_116_Is_True;

        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        private First_Model_Option_Form()
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

        private static First_Model_Option_Form Instance;

        public static First_Model_Option_Form getInstance()
        {
            if (Instance == null)
                Instance = new First_Model_Option_Form();

            return Instance;
        }

        private void First_Model_Option_Form_Load(object sender, EventArgs e)
        {
            BackColor = f1().current_model.Get_Back_Ground_Color();
            obj_engineer_mornitoring = Engineer_Mornitoring_Mode.getInstance();
            checkBox_Ave_Measure_CheckedChanged(sender, e);
            checkBox_Copy_Final_G255_to_Others_CheckedChanged(sender, e);
            Form_height = this.Size.Height;
            Form_width = this.Size.Width;
        }


        RGB Gamma_255; //Used for Test Mode
        RGB Gamma_Gray; //Used for Test Mode
        
        //Optic Compensation related variables
        RGB Prev_Gamma;
        RGB Diff_Gamma;
        int Vreg1;
        int Prev_Vreg1;
        int Diff_Vreg1;
        RGB Gamma;
        XYLv Measure;
        XYLv Target;
        XYLv Limit;
        XYLv Extension;
        public RGB[,] All_band_gray_Gamma = new RGB[12, 10]; //12ea Bands , 10ea Gray-points
        bool Optic_Compensation_Stop = false;
        bool Optic_Compensation_Succeed = false;
        
        //Vreg1 Related
        string[] DD_Vreg1 = new string[16];
        string[] DE_Vreg1 = new string[16];

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
        const int Gamma_Register_Limit = 511;
        const int Vreg1_Register_Limit = 4095;
        bool Gamma_Out_Of_Register_Limit = false;
        bool Within_Spec_Limit = false;

        //RGB Gamma Boolen
        bool[] RGB_Need_To_Change = new bool[3];

        //Current_Page
        public string Current_Page_Address = "0xFF";

        //First size
        int Form_height;
        int Form_width;

        Engineer_Mornitoring_Mode obj_engineer_mornitoring;
        
        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }


        public void Groupbox35_Hide()
        {
            this.groupBox35.Hide();
        }

        public void Groupbox35_Show()
        {
            this.groupBox35.Show();
        }

       

        public void Read_DP116_DBV_Setting()
        {
            button_Read_DP116_DBV_Setting.PerformClick();
        }

        private void button_Read_DP116_DBV_Setting_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                f1.Read_DP116_Page_Quantity_Register(12, 16, "B0");
                Thread.Sleep(200);
                textBox_B1_DBV_Setting.Text = f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[1].Cells[1].Value.ToString();
                textBox_B2_DBV_Setting.Text = f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[3].Cells[1].Value.ToString();
                textBox_B3_DBV_Setting.Text = f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
                textBox_B4_DBV_Setting.Text = f1.dataGridView1.Rows[6].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString();
                textBox_B5_DBV_Setting.Text = f1.dataGridView1.Rows[8].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString();
                textBox_B6_DBV_Setting.Text = f1.dataGridView1.Rows[10].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString();
                textBox_B7_DBV_Setting.Text = f1.dataGridView1.Rows[12].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString();
                textBox_B8_DBV_Setting.Text = f1.dataGridView1.Rows[14].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString();

                //Read Band9 DBV
                f1.Read_DP116_Page_Quantity_Register(12, 16, "B1");
                Thread.Sleep(100);
                textBox_B9_DBV_Setting.Text = f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[1].Cells[1].Value.ToString();
                 
                //Read AOD1~3 DBV
                f1.Read_DP116_Page_Quantity_Register(12, 16, "B2");
                Thread.Sleep(200);
                textBox_AOD1_DBV_Setting.Text = f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[1].Cells[1].Value.ToString();
                textBox_AOD2_DBV_Setting.Text = f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[3].Cells[1].Value.ToString();
                textBox_AOD3_DBV_Setting.Text = f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
                f1.GB_Status_AppendText_Nextline("DP116 DBV value was loaded from register", System.Drawing.Color.Black);

                Application.DoEvents();
            }
            catch
            {
                System.Windows.MessageBox.Show("DBV Value Read fail");
            }
        }


        void Sub_Band_Gray_Read(int band, int gray,bool Gamma_Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string H_Gamma_Register = string.Empty;
            string L_Gamma_Register = string.Empty;
            string Total_Gamma_Register = string.Empty;

            int Temp = 176;//Hex = B0 , Dex = 176
            int R_Gamma_Address;
            int G_Gamma_Address;
            int B_Gamma_Address;

            //////////////////////////////// R ////////////////////////////////////
            R_Gamma_Address = Temp + (band * 3);
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(0, 12, R_Gamma_Address.ToString("X"));
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(1, 12, R_Gamma_Address.ToString("X"));
            }
            if (gray == 1 || gray == 2 || gray == 3) //GR8/GR7/GR6
            {
                H_Gamma_Register = f1.dataGridView1.Rows[8].Cells[1].Value.ToString();//MSB678 (Gray 1/6/13 High)
                L_Gamma_Register = f1.dataGridView1.Rows[12 - gray].Cells[1].Value.ToString();//Gray 1/6/13
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[3 - gray] + L_Gamma_Register;
            }
            else if (gray == 4 || gray == 5 || gray == 6) //GR5/GR4/GR3
            {
                H_Gamma_Register = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();//MSB345
                L_Gamma_Register = f1.dataGridView1.Rows[11 - gray].Cells[1].Value.ToString();//Gray 23/28/60
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[6 - gray] + L_Gamma_Register;
            }
            else if (gray == 7 || gray == 8 || gray == 9) //GR2/GR1/GR0
            {
                H_Gamma_Register = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();//MSB012
                L_Gamma_Register = f1.dataGridView1.Rows[10 - gray].Cells[1].Value.ToString();//Gray 91/134/189
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[9 - gray] + L_Gamma_Register;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select proper Gray");
            }
            //Apply
            Gamma_Gray.int_R = Convert.ToInt32(Total_Gamma_Register, 2);
            Gamma_Gray.R = Gamma_Gray.int_R.ToString();






            //////////////////////////////// G ////////////////////////////////////
            G_Gamma_Address = Temp + (band * 3 + 1);
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(0, 12, G_Gamma_Address.ToString("X"));
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(1, 12, G_Gamma_Address.ToString("X"));
            }
            if (gray == 1 || gray == 2 || gray == 3)
            {
                H_Gamma_Register = f1.dataGridView1.Rows[8].Cells[1].Value.ToString();//MSB678
                L_Gamma_Register = f1.dataGridView1.Rows[12 - gray].Cells[1].Value.ToString();//Gray 1/6/13
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[3 - gray] + L_Gamma_Register;
            }
            else if (gray == 4 || gray == 5 || gray == 6)
            {
                H_Gamma_Register = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();//MSB345
                L_Gamma_Register = f1.dataGridView1.Rows[11 - gray].Cells[1].Value.ToString();//Gray 23/28/60
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[6 - gray] + L_Gamma_Register;
            }
            else if (gray == 7 || gray == 8 || gray == 9)
            {
                H_Gamma_Register = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();//MSB012
                L_Gamma_Register = f1.dataGridView1.Rows[10 - gray].Cells[1].Value.ToString();//Gray 91/134/189
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[9 - gray] + L_Gamma_Register;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select proper Gray");
            }
            //Apply
            Gamma_Gray.int_G = Convert.ToInt32(Total_Gamma_Register, 2);
            Gamma_Gray.G = Gamma_Gray.int_G.ToString();


            //////////////////////////////// B ////////////////////////////////////
            B_Gamma_Address = Temp + (band * 3 + 2);
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(0, 12, B_Gamma_Address.ToString("X"));
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(1, 12, B_Gamma_Address.ToString("X"));
            }
            if (gray == 1 || gray == 2 || gray == 3)
            {
                H_Gamma_Register = f1.dataGridView1.Rows[8].Cells[1].Value.ToString();//MSB678
                L_Gamma_Register = f1.dataGridView1.Rows[12 - gray].Cells[1].Value.ToString();//Gray 1/6/13
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[3 - gray] + L_Gamma_Register;
            }
            else if (gray == 4 || gray == 5 || gray == 6)
            {
                H_Gamma_Register = f1.dataGridView1.Rows[4].Cells[1].Value.ToString();//MSB345
                L_Gamma_Register = f1.dataGridView1.Rows[11 - gray].Cells[1].Value.ToString();//Gray 23/28/60
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[6 - gray] + L_Gamma_Register;
            }
            else if (gray == 7 || gray == 8 || gray == 9)
            {
                H_Gamma_Register = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();//MSB012
                L_Gamma_Register = f1.dataGridView1.Rows[10 - gray].Cells[1].Value.ToString();//Gray 91/134/189
                // Binary String (xxxx xxxx)
                H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(3, '0');
                L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');
                Total_Gamma_Register = H_Gamma_Register[9 - gray] + L_Gamma_Register;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select proper Gray");
            }
            //Apply
            Gamma_Gray.int_B = Convert.ToInt32(Total_Gamma_Register, 2);
            Gamma_Gray.B = Gamma_Gray.int_B.ToString();
        }

        void Sub_255_Band_Gray_Read(int band,bool Gamma_Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string H_Gamma_Register;
            string L_Gamma_Register;
            string Total_Gamma_Register;

            ////////////////================= Red 255 =================////////////////////
            //Read EA (H_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "EA");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "F0");
            }
            if (band <= 7)// Hex String (XX) , Band 1 ~ 8
                H_Gamma_Register = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            else// Hex String (XX) , Band 9 ~ 16
                H_Gamma_Register = f1.dataGridView1.Rows[1].Cells[1].Value.ToString();

            // Binary String (xxxx xxxx)
            H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(8, '0');

            //Read EB (L_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 14, "EB");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 14, "F1");
            }
            // Hex String (XX) , Band 1
            L_Gamma_Register = f1.dataGridView1.Rows[band].Cells[1].Value.ToString();
            // Binary String (xxxx xxxx)
            L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');

            //Merge H + L
            if (band <= 7)
                Total_Gamma_Register = H_Gamma_Register[band] + L_Gamma_Register;
            else// Hex String (XX) , Band 9 ~ 16
                Total_Gamma_Register = H_Gamma_Register[band - 8] + L_Gamma_Register;

            //Apply
            this.Gamma_255.int_R = Convert.ToInt32(Total_Gamma_Register, 2);
            this.Gamma_255.R = Gamma_255.int_R.ToString();


            ////////////////================= Green 255 =================////////////////////
            //Read EA (H_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "EC");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "F2");
            }

            if (band <= 7)// Hex String (XX) , Band 1 ~ 8
                H_Gamma_Register = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            else// Hex String (XX) , Band 9 ~ 16
                H_Gamma_Register = f1.dataGridView1.Rows[1].Cells[1].Value.ToString();

            // Binary String (xxxx xxxx)
            H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(8, '0');

            //Read EB (L_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 14, "ED");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 14, "F3");
            }
            // Hex String (XX) , Band 1
            L_Gamma_Register = f1.dataGridView1.Rows[band].Cells[1].Value.ToString();
            // Binary String (xxxx xxxx)
            L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');

            //Merge H + L
            if (band <= 7)
                Total_Gamma_Register = H_Gamma_Register[band] + L_Gamma_Register;
            else// Hex String (XX) , Band 9 ~ 16
                Total_Gamma_Register = H_Gamma_Register[band - 8] + L_Gamma_Register;

            //Apply
            this.Gamma_255.int_G = Convert.ToInt32(Total_Gamma_Register, 2);
            this.Gamma_255.G = Gamma_255.int_G.ToString();



            ////////////////================= Blue 255 =================////////////////////
            //Read EA (H_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "EE");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "F4");
            }

            if (band <= 7)// Hex String (XX) , Band 1 ~ 8
                H_Gamma_Register = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            else// Hex String (XX) , Band 9 ~ 16
                H_Gamma_Register = f1.dataGridView1.Rows[1].Cells[1].Value.ToString();

            // Binary String (xxxx xxxx)
            H_Gamma_Register = Convert.ToString(Convert.ToInt32(H_Gamma_Register, 16), 2).PadLeft(8, '0');

            //Read EB (L_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 14, "EF");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 14, "F5");
            }
            // Hex String (XX) , Band 1
            L_Gamma_Register = f1.dataGridView1.Rows[band].Cells[1].Value.ToString();
            // Binary String (xxxx xxxx)
            L_Gamma_Register = Convert.ToString(Convert.ToInt32(L_Gamma_Register, 16), 2).PadLeft(8, '0');

            //Merge H + L
            if (band <= 7)
                Total_Gamma_Register = H_Gamma_Register[band] + L_Gamma_Register;
            else// Hex String (XX) , Band 9 ~ 16
                Total_Gamma_Register = H_Gamma_Register[band - 8] + L_Gamma_Register;

            //Apply
            this.Gamma_255.int_B = Convert.ToInt32(Total_Gamma_Register, 2);
            this.Gamma_255.B = Gamma_255.int_B.ToString();

        }

        private void Update_AM255_High_P1_P2(ref int P1, ref int P2, int band, int Register_Value)
        {
            int pos = 7;

            if (band <= 7)// Hex String (XX) , Band 1 ~ 8 
            {
                switch (band)
                {
                    case 0:
                        pos = 7;
                        break;
                    case 1:
                        pos = 6;
                        break;
                    case 2:
                        pos = 5;
                        break;
                    case 3:
                        pos = 4;
                        break;
                    case 4:
                        pos = 3;
                        break;
                    case 5:
                        pos = 2;
                        break;
                    case 6:
                        pos = 1;
                        break;
                    case 7:
                        pos = 0;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                        break;
                }
                P1 = (P1 & (~(1 << pos))) | ((Register_Value >> 8) << pos);
            }    //H_Gamma_Register = EA번지 , P1
            else// Hex String (XX) , Band 9 ~ 16
            {
                switch (band)
                {
                    case 8:
                        pos = 7;
                        break;
                    case 9:
                        pos = 6;
                        break;
                    case 10:
                        pos = 5;
                        break;
                    case 11://AOD1
                        pos = 4;
                        break;
                    case 12://AOD2
                        pos = 3;
                        break;
                    case 13://AOD3
                        pos = 2;
                        break;
                    case 14://VR1
                        pos = 1;
                        break;
                    case 15://VR2
                        pos = 0;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                        break;
                }
                P2 = (P2 & (~(1 << pos))) | ((Register_Value >> 8) << pos);
                //System.Windows.Forms.MessageBox.Show((((Red_AM255 >> 8) << pos)).ToString());
            }    //H_Gamma_Register = EA 번지 , P2
        }

        private void Sub_Band_Gray_Write(int band, int gray,bool Gamma_Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            const int B0h = 176;//Hex = B0 , Dex = 176
            string R_Gamma_Address = (B0h + (band * 3)).ToString("X");
            string G_Gamma_Address = (B0h + (band * 3) + 1).ToString("X");
            string B_Gamma_Address = (B0h + (band * 3) + 2).ToString("X");
            string temp_mipi_cmd = "mipi.write ~~";

            //////////////////////////////// R ////////////////////////////////////
            int Red_Gamma = Convert.ToInt16(textBox_Gamma_Write_R.Text);
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(0, 12, R_Gamma_Address);
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(1, 12, R_Gamma_Address);
            }
            string[] Params_R = new string[12];
            int[] Params_int_R = new int[12];
            int temp_R = 0;
            for (int i = 0; i < 12; i++)
            {
                Params_R[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Params_int_R[i] = Convert.ToInt16(Params_R[i], 16);
            }

            /////////////////////////////// G ////////////////////////////////////
            int Green_Gamma = Convert.ToInt16(textBox_Gamma_Write_G.Text);
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(0, 12, G_Gamma_Address);
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(1, 12, G_Gamma_Address);
            }
            string[] Params_G = new string[12];
            int[] Params_int_G = new int[12];
            int temp_G = 0;
            for (int i = 0; i < 12; i++)
            {
                Params_G[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Params_int_G[i] = Convert.ToInt16(Params_G[i], 16);
            }

            ///////////////////////////// B ////////////////////////////////////
            int Blue_Gamma = Convert.ToInt16(textBox_Gamma_Write_B.Text);
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(0, 12, B_Gamma_Address);
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(1, 12, B_Gamma_Address);
            }
            string[] Params_B = new string[12];
            int[] Params_int_B = new int[12];
            int temp_B = 0;
            for (int i = 0; i < 12; i++)
            {
                Params_B[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                Params_int_B[i] = Convert.ToInt16(Params_B[i], 16);
            }

            ///////////////////// R G B //////////////////////////
            int pos = 0;
            if (gray == 1 || gray == 2 || gray == 3) //GR8/GR7/GR6
            {
                switch (gray)
                {
                    case 1: //GR8
                        pos = 0;
                        Params_R[11] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[11] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[11] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    case 2: //GR7
                        pos = 1;
                        Params_R[10] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[10] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[10] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    case 3: //GR6
                        pos = 2;
                        Params_R[9] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[9] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[9] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    default:
                        break;
                }
                temp_R = (Red_Gamma >> 8) << pos;
                temp_G = (Green_Gamma >> 8) << pos;
                temp_B = (Blue_Gamma >> 8) << pos;

                Params_int_R[8] = (Params_int_R[8] & (~(1 << pos))) | temp_R;
                Params_int_G[8] = (Params_int_G[8] & (~(1 << pos))) | temp_G;
                Params_int_B[8] = (Params_int_B[8] & (~(1 << pos))) | temp_B;

                Params_R[8] = Params_int_R[8].ToString("X");
                Params_G[8] = Params_int_G[8].ToString("X");
                Params_B[8] = Params_int_B[8].ToString("X");

            }
            else if (gray == 4 || gray == 5 || gray == 6) //GR5/GR4/GR3
            {
                switch (gray)
                {
                    case 4: //GR5
                        pos = 0;
                        Params_R[7] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[7] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[7] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    case 5: //GR4
                        pos = 1;
                        Params_R[6] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[6] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[6] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    case 6: //GR3
                        pos = 2;
                        Params_R[5] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[5] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[5] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    default:
                        break;
                }
                temp_R = (Red_Gamma >> 8) << pos;
                temp_G = (Green_Gamma >> 8) << pos;
                temp_B = (Blue_Gamma >> 8) << pos;

                Params_int_R[4] = (Params_int_R[4] & (~(1 << pos))) | temp_R;
                Params_int_G[4] = (Params_int_G[4] & (~(1 << pos))) | temp_G;
                Params_int_B[4] = (Params_int_B[4] & (~(1 << pos))) | temp_B;

                Params_R[4] = Params_int_R[4].ToString("X");
                Params_G[4] = Params_int_G[4].ToString("X");
                Params_B[4] = Params_int_B[4].ToString("X");

            }
            else if (gray == 7 || gray == 8 || gray == 9) //GR2/GR1/GR0
            {
                switch (gray)
                {
                    case 7: //GR2
                        pos = 0;
                        Params_R[3] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[3] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[3] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    case 8: //GR1
                        pos = 1;
                        Params_R[2] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[2] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[2] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    case 9: //GR0
                        pos = 2;
                        Params_R[1] = (Red_Gamma & 0xFF).ToString("X");
                        Params_G[1] = (Green_Gamma & 0xFF).ToString("X");
                        Params_B[1] = (Blue_Gamma & 0xFF).ToString("X");
                        break;
                    default:
                        break;
                }

                temp_R = (Red_Gamma >> 8) << pos;
                temp_G = (Green_Gamma >> 8) << pos;
                temp_B = (Blue_Gamma >> 8) << pos;

                Params_int_R[0] = (Params_int_R[0] & (~(1 << pos))) | temp_R;
                Params_int_G[0] = (Params_int_G[0] & (~(1 << pos))) | temp_G;
                Params_int_B[0] = (Params_int_B[0] & (~(1 << pos))) | temp_B;

                Params_R[0] = Params_int_R[0].ToString("X");
                Params_G[0] = Params_int_G[0].ToString("X");
                Params_B[0] = Params_int_B[0].ToString("X");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select proper Gray");
            }

            if (Gamma_Set)
                f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, true);
            else
                f1.DP116_CMD2_Page_Selection(1, false, false, ref Current_Page_Address, true);


            //Apply Red Gammas
            temp_mipi_cmd = "mipi.write 0x39 0x" + R_Gamma_Address;
            for (int i = 0; i < 12; i++)
            {
                temp_mipi_cmd += " 0x" + Params_R[i];
            }
            f1.IPC_Quick_Send(temp_mipi_cmd);

            //Apply Green Gamma
            temp_mipi_cmd = "mipi.write 0x39 0x" + G_Gamma_Address;
            for (int i = 0; i < 12; i++)
            {
                temp_mipi_cmd += " 0x" + Params_G[i];
            }
            f1.IPC_Quick_Send(temp_mipi_cmd);

            //Apply Blue Gamma
            temp_mipi_cmd = "mipi.write 0x39 0x" + B_Gamma_Address;
            for (int i = 0; i < 12; i++)
            {
                temp_mipi_cmd += " 0x" + Params_B[i];
            }
            f1.IPC_Quick_Send(temp_mipi_cmd);

        }

        private void Sub_255_Band_Gray_Write(int band,bool Gamma_Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int PH1; //High Data
            int PH2; //High Data

            string Param_H1;
            string Param_H2;
            string[] Param_L = new string[16];

            string temp_mipi_cmd;

            ////////////////================= Red 255 AM255 =================////////////////////
            int Red_AM255 = Convert.ToInt16(textBox_Gamma_Write_R.Text);

            //Read EAh (H_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "EA");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "F0");
            }
            PH1 = Convert.ToInt16(f1.dataGridView1.Rows[0].Cells[1].Value.ToString(), 16);
            PH2 = Convert.ToInt16(f1.dataGridView1.Rows[1].Cells[1].Value.ToString(), 16);

            //Update EAh Param1,Param2 (High Data)
            Update_AM255_High_P1_P2(ref PH1, ref PH2, band, Red_AM255);
            Param_H1 = PH1.ToString("X").PadLeft(2, '0');
            Param_H2 = PH2.ToString("X").PadLeft(2, '0');

            //Write Data Into EAh (High Data)
            f1.DP116_CMD2_Page_Selection(11, false, false, ref Current_Page_Address, true);
            if (Gamma_Set)
            {
                f1.IPC_Quick_Send("mipi.write 0x39 0xEA 0x" + Param_H1 + " 0x" + Param_H2);
            }
            else
            {
                f1.IPC_Quick_Send("mipi.write 0x39 0xF0 0x" + Param_H1 + " 0x" + Param_H2);
            }

            //Read EB and change EB value (L_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 16, "EB");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 16, "F1");
            }
            for (int i = 0; i < 16; i++)
            {
                Param_L[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            Param_L[band] = (Red_AM255 & 0xFF).ToString("X");

            if (Gamma_Set)
            {
                temp_mipi_cmd = "mipi.write 0x39 0xEB";
            }
            else
            {
                temp_mipi_cmd = "mipi.write 0x39 0xF1";
            }
            for (int i = 0; i < 16; i++)
            {
                temp_mipi_cmd += " 0x" + Param_L[i];
            }
            f1.IPC_Quick_Send(temp_mipi_cmd);


            ////////////////================= Green 255 =================////////////////////
            int Green_AM255 = Convert.ToInt16(textBox_Gamma_Write_G.Text);

            //Read ECh (H_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "EC");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "F2");
            }
            PH1 = Convert.ToInt16(f1.dataGridView1.Rows[0].Cells[1].Value.ToString(), 16);
            PH2 = Convert.ToInt16(f1.dataGridView1.Rows[1].Cells[1].Value.ToString(), 16);

            //Update ECh Param1,Param2 (High Data)
            Update_AM255_High_P1_P2(ref PH1, ref PH2, band, Green_AM255);
            Param_H1 = PH1.ToString("X").PadLeft(2, '0');
            Param_H2 = PH2.ToString("X").PadLeft(2, '0');

            //Write Data Into ECh (High Data)
            f1.DP116_CMD2_Page_Selection(11, false, false, ref Current_Page_Address, true);
            if (Gamma_Set)
            {
                f1.IPC_Quick_Send("mipi.write 0x39 0xEC 0x" + Param_H1 + " 0x" + Param_H2);
            }
            else
            {
                f1.IPC_Quick_Send("mipi.write 0x39 0xF2 0x" + Param_H1 + " 0x" + Param_H2);
            }

            //Read ED and change ED value (L_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 16, "ED");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 16, "F3");
            }
            for (int i = 0; i < 16; i++)
            {
                Param_L[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            Param_L[band] = (Green_AM255 & 0xFF).ToString("X");
            if (Gamma_Set)
            {
                temp_mipi_cmd = "mipi.write 0x39 0xED";
            }
            else
            {
                temp_mipi_cmd = "mipi.write 0x39 0xF3";
            }
            for (int i = 0; i < 16; i++)
            {
                temp_mipi_cmd += " 0x" + Param_L[i];
            }
            f1.IPC_Quick_Send(temp_mipi_cmd);



            ////////////////================= Blue 255 =================////////////////////
            int Blue_AM255 = Convert.ToInt16(textBox_Gamma_Write_B.Text);

            //Read EEh (H_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "EE");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 2, "F4");
            }
            PH1 = Convert.ToInt16(f1.dataGridView1.Rows[0].Cells[1].Value.ToString(), 16);
            PH2 = Convert.ToInt16(f1.dataGridView1.Rows[1].Cells[1].Value.ToString(), 16);

            //Update EEh Param1,Param2 (High Data)
            Update_AM255_High_P1_P2(ref PH1, ref PH2, band, Blue_AM255);
            Param_H1 = PH1.ToString("X").PadLeft(2, '0');
            Param_H2 = PH2.ToString("X").PadLeft(2, '0');

            //Write Data Into EEh (High Data)
            f1.DP116_CMD2_Page_Selection(11, false, false, ref Current_Page_Address, true);
            if (Gamma_Set)
            {
                f1.IPC_Quick_Send("mipi.write 0x39 0xEE 0x" + Param_H1 + " 0x" + Param_H2);
            }
            else
            {
                f1.IPC_Quick_Send("mipi.write 0x39 0xF4 0x" + Param_H1 + " 0x" + Param_H2);
            }

            //Read EF and change EF value (L_Gamma_Register_255)
            if (Gamma_Set)
            {
                f1.Read_DP116_Page_Quantity_Register(11, 16, "EF");
            }
            else
            {
                f1.Read_DP116_Page_Quantity_Register(11, 16, "F5");
            }
            for (int i = 0; i < 16; i++)
            {
                Param_L[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            Param_L[band] = (Blue_AM255 & 0xFF).ToString("X");
            if (Gamma_Set)
            {
                temp_mipi_cmd = "mipi.write 0x39 0xEF";
            }
            else
            {
                temp_mipi_cmd = "mipi.write 0x39 0xF5";
            }
            for (int i = 0; i < 16; i++)
            {
                temp_mipi_cmd += " 0x" + Param_L[i];
            }
            f1.IPC_Quick_Send(temp_mipi_cmd);
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

        public void button_A1_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_AOD1_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("AOD0 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_A2_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_AOD2_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("AOD1 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_A3_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_AOD3_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("AOD2 DBV Setting", System.Drawing.Color.Black);
        }

        private void Read_Vreg1()
        {

        }

        private void Write_Vreg1(int value)
        {
            

        }

        private void Initalize_Vreg1(bool Gamma_Set1)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            for (int band = 0; band < 12; band++)
            {
                if (band >= 0 && band <= 7)
                {
                    if (Gamma_Set1)
                    {
                        f1.Read_DP116_Page_Quantity_Register(0, 16, "DD");
                    }
                    else
                    {
                        f1.Read_DP116_Page_Quantity_Register(1, 16, "DD");
                    }

                    Thread.Sleep(20);
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(20);
                    for (int i = 0; i < 16; i++)
                    {
                        DD_Vreg1[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                    }
                }
                else
                {
                    if (Gamma_Set1)
                    {
                        f1.Read_DP116_Page_Quantity_Register(0, 16, "DE");
                    }
                    else
                    {
                        f1.Read_DP116_Page_Quantity_Register(1, 16, "DE");
                    }
                    Thread.Sleep(20);
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(20);
                    for (int i = 0; i < 16; i++)
                    {
                        DE_Vreg1[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                    }
                }
            }
        }

        public int Vreg1_Read2(int band,bool Gamma_Set1,bool Need_To_Read)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string H_Vreg1_Value = string.Empty;
            string L_Vreg1_Value = string.Empty;
            string Total_Value = string.Empty;
     
            if (band >= 0 && band <= 7)
            {
                if (Need_To_Read)
                {
                    if (Gamma_Set1)
                    {
                        f1.Read_DP116_Page_Quantity_Register(0, 16, "DD");
                    }
                    else
                    {
                        f1.Read_DP116_Page_Quantity_Register(1, 16, "DD");
                    }
                }
                Thread.Sleep(20);
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(20);
                for (int i = 0; i < 16; i++)
                {
                    DD_Vreg1[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                }
                H_Vreg1_Value = f1.dataGridView1.Rows[band * 2].Cells[1].Value.ToString();
                L_Vreg1_Value = f1.dataGridView1.Rows[band * 2 + 1].Cells[1].Value.ToString();

            }
            else
            {
                if (Need_To_Read)
                {
                    if (Gamma_Set1)
                    {
                        f1.Read_DP116_Page_Quantity_Register(0, 16, "DE");
                    }
                    else
                    {
                        f1.Read_DP116_Page_Quantity_Register(1, 16, "DE");
                    }
                }
                Thread.Sleep(20);
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(20);
                for (int i = 0; i < 16; i++)
                {
                    DE_Vreg1[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                }
                H_Vreg1_Value = f1.dataGridView1.Rows[(band - 8) * 2].Cells[1].Value.ToString();
                L_Vreg1_Value = f1.dataGridView1.Rows[(band - 8) * 2 + 1].Cells[1].Value.ToString();
            }
            H_Vreg1_Value = Convert.ToString(H_Vreg1_Value).PadLeft(2, '0');
            Total_Value = H_Vreg1_Value[1] + L_Vreg1_Value;
            
            return Convert.ToInt16(Total_Value, 16);
        }

        private void Vreg1_Read(int band,bool Gamma_Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string H_Vreg1_Value = string.Empty;
            string L_Vreg1_Value = string.Empty;
            string Total_Value = string.Empty;

            
            if (band >= 0 && band <= 7)
            {
                if (Gamma_Set)
                {
                    f1.Read_DP116_Page_Quantity_Register(0, 16, "DD");
                }
                else
                {
                    f1.Read_DP116_Page_Quantity_Register(1, 16, "DD");
                }
                H_Vreg1_Value = f1.dataGridView1.Rows[band * 2].Cells[1].Value.ToString();
                L_Vreg1_Value = f1.dataGridView1.Rows[band * 2 + 1].Cells[1].Value.ToString();

            }
            else
            {
                if (Gamma_Set)
                {
                    f1.Read_DP116_Page_Quantity_Register(0, 16, "DE");
                }
                else
                {
                    f1.Read_DP116_Page_Quantity_Register(1, 16, "DE");
                }
                H_Vreg1_Value = f1.dataGridView1.Rows[(band - 8) * 2].Cells[1].Value.ToString();
                L_Vreg1_Value = f1.dataGridView1.Rows[(band - 8) * 2 + 1].Cells[1].Value.ToString();
            }

            H_Vreg1_Value = Convert.ToString(H_Vreg1_Value).PadLeft(2, '0');
            Total_Value = H_Vreg1_Value[1] + L_Vreg1_Value;

            this.textBox_Vreg1_Read.Text = Convert.ToInt16(Total_Value,16).ToString();
        }

        private void VREF2_and_AM0_Read()
        {
            string VREF2;
            string AM0;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Read_DP116_Page_Quantity_Register(0, 1, "AE"); //VREF2
            VREF2 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            VREF2 = Convert.ToString(Convert.ToInt16(VREF2,16) & 0x7F);
            textBox_VREF2_Read.Text = VREF2;
            f1.GB_Status_AppendText_Nextline("VREF2 : " + VREF2, Color.Blue);

            f1.Read_DP116_Page_Quantity_Register(0, 1, "AF"); //AM0
            AM0 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            AM0 = Convert.ToString(Convert.ToInt16(AM0, 16) & 0x7F);
            textBox_AM0_Read.Text = AM0;
            f1.GB_Status_AppendText_Nextline("AM0 : " + AM0, Color.Blue);
        }


        private void Gamma_read_btn_Click(object sender, EventArgs e)
        {
            textBox_Vreg1_Read.Text = string.Empty;
            textBox_Gamma_Read_R.Text = string.Empty;
            textBox_Gamma_Read_G.Text = string.Empty;
            textBox_Gamma_Read_B.Text = string.Empty;
            textBox_VREF2_Read.Text = string.Empty;
            textBox_AM0_Read.Text = string.Empty;
            Application.DoEvents();

            int Band_Point = Convert.ToInt16(textBox_Band_Point.Text);
            if (Band_Point >= 15)
            {
                Band_Point = 15;
                textBox_Band_Point.Text = Band_Point.ToString();
            }

            int Gray_Point = Convert.ToInt16(textBox_Gray_Point.Text);
            if (Gray_Point >= 9)
            {
                Gray_Point = 9;
                textBox_Gray_Point.Text = Gray_Point.ToString();
            }

            bool Condition = true;
            if (radioButton_Gamma_Set_1.Checked)
            {
                Condition = true;
            }
            else if (radioButton_Gamma_Set_2.Checked)
            {
                Condition = false;
            }
            else
            {
                //Do nothinng
                System.Windows.Forms.MessageBox.Show("Error");
            }


            if (Gray_Point == 0) //Gray 255
            {
                Sub_255_Band_Gray_Read(Band_Point, Condition);
                textBox_Gamma_Read_R.Text = Gamma_255.R;
                textBox_Gamma_Read_G.Text = Gamma_255.G;
                textBox_Gamma_Read_B.Text = Gamma_255.B;
                Vreg1_Read(Band_Point, Condition); //Read Vreg1 Only at Gray255
            }
            else if (Gray_Point >= 1 && Gray_Point <= 9) //GR8 (Gray_Point = 1 / 189Gray) , GR7 (Gray_Point = 2 / 134Gray) , .. , GR0 (Gray_Point = 9 / 1Gray)
            {
                Sub_Band_Gray_Read(Band_Point, Gray_Point, Condition);
                textBox_Gamma_Read_R.Text = Gamma_Gray.R;
                textBox_Gamma_Read_G.Text = Gamma_Gray.G;
                textBox_Gamma_Read_B.Text = Gamma_Gray.B;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Select Proper Gray Point");
            }
            VREF2_and_AM0_Read();
        }

        private void Vreg1_Write(int band,bool Gamma_Set)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Vreg1_int = Convert.ToInt16(textBox_Vreg1_Write.Text);
            if (Vreg1_int < 0)
            {
                Vreg1_int = 0;
            }
            else if (Vreg1_int > 4095)
            {
                Vreg1_int = 4095;
            }
            textBox_Vreg1_Write.Text = Vreg1_int.ToString();

            string[] DD_ = new string[16];
            string[] DE_ = new string[16];
            string Total = Vreg1_int.ToString("X3");
            
            if (band >= 0 && band <= 7)
            {
                if (Gamma_Set)
                {
                    f1.Read_DP116_Page_Quantity_Register(0, 16, "DD");
                }
                else
                {
                    f1.Read_DP116_Page_Quantity_Register(1, 16, "DD");
                }
                for(int i=0 ; i<16 ; i++)
                {
                    DD_[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                }
                DD_[band * 2] = Total[0].ToString();
                DD_[band * 2 + 1] = Total[1].ToString() + Total[2].ToString();
                f1.Long_Packet_CMD_Send(16, "DD", DD_);
            }
            else
            {
                if (Gamma_Set)
                {
                    f1.Read_DP116_Page_Quantity_Register(0, 16, "DE");
                }
                else
                {
                    f1.Read_DP116_Page_Quantity_Register(1, 16, "DE");
                }
                for (int i = 0; i < 16; i++)
                {
                    DE_[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                }
                DE_[(band - 8) * 2] = Total[0].ToString();
                DE_[(band - 8) * 2 + 1] = Total[1].ToString() + Total[2].ToString();
                f1.Long_Packet_CMD_Send(16, "DE", DE_);
            }
        }

        //Original_Value & ~(1 << position)) | ((bit << position) & (1 << position))
        private void Gamma_Write_btn_Click(object sender, EventArgs e)
        {
            int Band_Point = Convert.ToInt16(textBox_Band_Point.Text);
            if (Band_Point >= 15)
            {
                Band_Point = 15;
                textBox_Band_Point.Text = Band_Point.ToString();
            }

            int Gray_Point = Convert.ToInt16(textBox_Gray_Point.Text);
            if (Gray_Point >= 9)
            {
                Gray_Point = 9;
                textBox_Gray_Point.Text = Gray_Point.ToString();
            }

            Gamma.int_R = Convert.ToInt32(textBox_Gamma_Write_R.Text, 10);
            if (Gamma.int_R < 0)
            {
                Gamma.int_R = 0;
            }
            else if (Gamma.int_R > 511)
            {
                Gamma.int_R = 511;
            }
            textBox_Gamma_Write_R.Text = Gamma.int_R.ToString();

            Gamma.int_G = Convert.ToInt32(textBox_Gamma_Write_G.Text, 10);
            if (Gamma.int_G < 0)
            {
                Gamma.int_G = 0;
            }
            else if (Gamma.int_G > 511)
            {
                Gamma.int_G = 511;
            }
            textBox_Gamma_Write_G.Text = Gamma.int_G.ToString();

            Gamma.int_B = Convert.ToInt32(textBox_Gamma_Write_B.Text, 10);
            if (Gamma.int_B < 0)
            {
                Gamma.int_B = 0;
            }
            else if (Gamma.int_B > 511)
            {
                Gamma.int_B = 511;
            }
            textBox_Gamma_Write_B.Text = Gamma.int_B.ToString();

            bool Condition = true;
            if (radioButton_Gamma_Set_1.Checked)
            {
                Condition = true;
            }
            else if (radioButton_Gamma_Set_2.Checked)
            {
                Condition = false;
            }
            else
            {
                //Do nothing 
            }
            

            //Gamma R/G/B Write
            if (Gray_Point == 0)
            {
                Sub_255_Band_Gray_Write(Band_Point, Condition);
            }
            else if (Gray_Point >= 1 && Gray_Point <= 9)
            {
                Sub_Band_Gray_Write(Band_Point, Gray_Point,Condition);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Select Proper Gray Point");
            }
            //Vreg1 Write (if option has been selected)
            if(checkBox_Vreg1_Write.Checked)
            Vreg1_Write(Band_Point,Condition);
        }

        private void groupBox26_Enter(object sender, EventArgs e)
        {

        }

        private void Get_Param(int gray)
        {
            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
            form_engineer.Get_OC_Param_DP116(gray, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B
                , ref Target.double_X, ref Target.double_Y, ref Target.double_Lv, ref Limit.double_X, ref Limit.double_Y, ref Limit.double_Lv , ref Extension.double_X , ref Extension.double_Y);
        }

        private void Dual_Mode_Get_Param(int gray, bool Condition)
        {
            //Conditon = true : Condition 1
            //Conditon = false : Condition 2
            Dual_Engineer_Monitoring_Mode form_dual_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            form_dual_engineer.Get_OC_Param_DP116(gray, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B
                , ref Target.double_X, ref Target.double_Y, ref Target.double_Lv,ref Limit.double_X, ref Limit.double_Y, ref Limit.double_Lv,ref Extension.double_X, ref Extension.double_Y, Condition);
            Gamma.String_Update_From_int();
        }

        private void Update_Engineering_Sheet(int band, int gray, int loop_count, string Extension_Applied)
        {
            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
            form_engineer.Set_OC_Param_DP116(gray, Gamma.int_R, Gamma.int_G, Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, loop_count, Extension_Applied);
            form_engineer.Updata_Sub_To_Main_GridView(band, gray);
        }
        
        private void Dual_Mode_Update_Engineering_Sheet(int band, int gray, int loop_count, string Extension_Applied,bool Condition)
        {
            //Conditon = true : Condition 1
            //Conditon = false : Condition 2
            Dual_Engineer_Monitoring_Mode Dual_Mode_form_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            Dual_Mode_form_engineer.Set_OC_Param_DP116(gray, Gamma.int_R, Gamma.int_G, Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, loop_count, Extension_Applied, Condition);
            Dual_Mode_form_engineer.Updata_Sub_To_Main_GridView(band, gray,Condition);
        }

        private bool Band_Selection_Check(int band)
        {
            switch (band)
            {
                case 0:
                    if (checkBox_Band1.Checked) return true;
                    else return false;
                case 1:
                    if (checkBox_Band2.Checked) return true;
                    else return false;
                case 2:
                    if (checkBox_Band3.Checked) return true;
                    else return false;
                case 3:
                    if (checkBox_Band4.Checked) return true;
                    else return false;;
                case 4:
                    if (checkBox_Band5.Checked) return true;
                    else return false;
                case 5:
                    if (checkBox_Band6.Checked) return true;
                    else return false;
                case 6:
                    if (checkBox_Band7.Checked) return true;
                    else return false;
                case 7:
                    if (checkBox_Band8.Checked) return true;
                    else return false;
                case 8:
                    if (checkBox_Band9.Checked) return true;
                    else return false;
                case 9:
                    if (checkBox_Band10.Checked) return true;
                    else return false;
                case 10:
                    if (checkBox_Band11.Checked) return true;
                    else return false;
                case 11:
                    if (checkBox_Band12.Checked) return true;
                    else return false;
                default:
                    return false;
            }
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
            else if (checkBox_Band11.Checked == false && band == 10)
                return false;
            else if (checkBox_Band12.Checked == false && band == 11)
                return false;
            else
                return true;
        }

        private void Get_All_Band_Gray_Gamma()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Get All Band/Gray Gamma from OC_Param", Color.Blue);

            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
            form_engineer.DP116_Get_All_Band_Gray_Gamma(All_band_gray_Gamma);
        }

        private void Dual_Get_All_Band_Gray_Gamma(bool Condition)
        {
            Dual_Engineer_Monitoring_Mode Dual_form_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            Dual_form_engineer.DP116_Get_All_Band_Gray_Gamma(All_band_gray_Gamma, Condition);
        }

        private void Update_Vreg1_TextBox(int Vreg1_int, int band,bool condition)
        {
            if (condition)
            {
                if (band == 0)
                    textBox_Vreg1_B0.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 1)
                    textBox_Vreg1_B1.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 2)
                    textBox_Vreg1_B2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 3)
                    textBox_Vreg1_B3.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 4)
                    textBox_Vreg1_B4.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 5)
                    textBox_Vreg1_B5.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 6)
                    textBox_Vreg1_B6.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 7)
                    textBox_Vreg1_B7.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 8)
                    textBox_Vreg1_B8.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else
                {
                    //Do nothing 
                }
            }
            else 
            {
                if (band == 0)
                    textBox_Vreg1_B0_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 1)
                    textBox_Vreg1_B1_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 2)
                    textBox_Vreg1_B2_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 3)
                    textBox_Vreg1_B3_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 4)
                    textBox_Vreg1_B4_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 5)
                    textBox_Vreg1_B5_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 6)
                    textBox_Vreg1_B6_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 7)
                    textBox_Vreg1_B7_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else if (band == 8)
                    textBox_Vreg1_B8_2.Text = Vreg1_int.ToString() + " (" + Vreg1_int.ToString("X3") + "h)"; //Read Vreg1 Value
                else
                {
                    //Do nothing 
                }
            }
            Application.DoEvents();
        }

       

        private void Update_and_Send_Vreg1(int Vreg1_int, int band,bool Gamma_Set1)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
           
            string Total = Vreg1_int.ToString("X3");

            if (Gamma_Set1)
            {
                f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address,false);
            }
            else
            {
                f1.DP116_CMD2_Page_Selection(1, false, false, ref Current_Page_Address, false);
            }

            if (band >= 0 && band <= 7)
            {
                DD_Vreg1[band * 2] = Total[0].ToString();
                DD_Vreg1[band * 2 + 1] = Total[1].ToString() + Total[2].ToString();
                f1.Long_Packet_CMD_Send(16, "DD", DD_Vreg1);
            }
            else
            {
                DE_Vreg1[(band - 8) * 2] = Total[0].ToString();
                DE_Vreg1[(band - 8) * 2 + 1] = Total[1].ToString() + Total[2].ToString();
                f1.Long_Packet_CMD_Send(16, "DE", DE_Vreg1);
            }

            if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Vreg1 Is Applied", Color.Black);
        }

        public void Update_and_Send_All_Band_Gray_Gamma(RGB Current_Gamma, int Current_Band, int Current_Gray , bool Gamma_Set1)
        {
            //Gamma_Set = true --> Gamma Set 1
            //Gamma_Set = False --> Gamma Set 2

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Update Gamma table as current Gamma
            All_band_gray_Gamma[Current_Band, Current_Gray].Equal_Value(Current_Gamma);

            //RGB temp_mipi_cmd1 = new RGB();
            StringBuilder temp_mipi_cmd1_R = new StringBuilder();
            StringBuilder temp_mipi_cmd1_G = new StringBuilder();
            StringBuilder temp_mipi_cmd1_B = new StringBuilder();
            //RGB temp_mipi_cmd2 = new RGB();
            StringBuilder temp_mipi_cmd2_R = new StringBuilder();
            StringBuilder temp_mipi_cmd2_G = new StringBuilder();
            StringBuilder temp_mipi_cmd2_B = new StringBuilder();
            
            RGB[] Band_Gray255_9th_data = new RGB[16]; //AM256[8] setting
            RGB[] Band_8ea_Gray_data = new RGB[16]; //AM256[7:0] setting
            RGB Param_H1 = new RGB();//EA,EC,EEh 1st param
            RGB Param_H2 = new RGB();//EA,EC,EEh 2nd param

            if (Current_Gray == 0) //if Gray = 255 (Gray255 모든 Band 적용)
            {
                for (int band = 0; band < 16; band++)
                {
                    if (band == 9 || band == 10 || band == 14 || band == 15)
                    {
                        Band_Gray255_9th_data[band].Set_Value(0, 0, 0);
                        Band_8ea_Gray_data[band].Set_Value(0, 0, 0);
                    }
                    else if (band == 11 || band == 12 || band == 13)
                    {
                        Band_Gray255_9th_data[band].Set_Value(All_band_gray_Gamma[band - 2, 0].int_R >> 8
                            , All_band_gray_Gamma[band - 2, 0].int_G >> 8, All_band_gray_Gamma[band - 2, 0].int_B >> 8);

                        Band_8ea_Gray_data[band].Set_Value(All_band_gray_Gamma[band - 2, 0].int_R & 0xFF
                            , All_band_gray_Gamma[band - 2, 0].int_G & 0xFF, All_band_gray_Gamma[band - 2, 0].int_B & 0xFF);
                    }
                    else
                    {
                        Band_Gray255_9th_data[band].Set_Value(All_band_gray_Gamma[band, 0].int_R >> 8
                            , All_band_gray_Gamma[band, 0].int_G >> 8, All_band_gray_Gamma[band, 0].int_B >> 8);

                        Band_8ea_Gray_data[band].Set_Value(All_band_gray_Gamma[band, 0].int_R & 0xFF
                            , All_band_gray_Gamma[band, 0].int_G & 0xFF, All_band_gray_Gamma[band, 0].int_B & 0xFF);
                    }
                }

                //AM256[7:0] setting
                if (Gamma_Set1)
                {
                    //temp_mipi_cmd2.R = "mipi.write 0x39 0xEB";
                    //temp_mipi_cmd2.G = "mipi.write 0x39 0xED";
                    //temp_mipi_cmd2.B = "mipi.write 0x39 0xEF";
                    temp_mipi_cmd2_R.Append("mipi.write 0x39 0xEB");
                    temp_mipi_cmd2_G.Append("mipi.write 0x39 0xED");
                    temp_mipi_cmd2_B.Append("mipi.write 0x39 0xEF");

                }
                else
                {
                    //temp_mipi_cmd2.R = "mipi.write 0x39 0xF1";
                    //temp_mipi_cmd2.G = "mipi.write 0x39 0xF3";
                    //temp_mipi_cmd2.B = "mipi.write 0x39 0xF5";
                    temp_mipi_cmd2_R.Append("mipi.write 0x39 0xF1");
                    temp_mipi_cmd2_G.Append("mipi.write 0x39 0xF3");
                    temp_mipi_cmd2_B.Append("mipi.write 0x39 0xF5");
                }

                //binary format string (xxxxxxxx)
                for (int band = 0; band <= 7; band++)
                {
                    Param_H1.R += Band_Gray255_9th_data[band].R; //"xxxxxxxx" (bit) string form
                    Param_H1.G += Band_Gray255_9th_data[band].G; //"xxxxxxxx" (bit) string form
                    Param_H1.B += Band_Gray255_9th_data[band].B; //"xxxxxxxx" (bit) string form

                    //temp_mipi_cmd2.R += " 0x" + Band_8ea_Gray_data[band].int_R.ToString("X2");
                    //temp_mipi_cmd2.G += " 0x" + Band_8ea_Gray_data[band].int_G.ToString("X2");
                    //temp_mipi_cmd2.B += " 0x" + Band_8ea_Gray_data[band].int_B.ToString("X2");
                    temp_mipi_cmd2_R.Append(" 0x").Append(Band_8ea_Gray_data[band].int_R.ToString("X2"));
                    temp_mipi_cmd2_G.Append(" 0x").Append(Band_8ea_Gray_data[band].int_G.ToString("X2"));
                    temp_mipi_cmd2_B.Append(" 0x").Append(Band_8ea_Gray_data[band].int_B.ToString("X2"));
                }
                for (int band = 8; band <= 15; band++)
                {
                    Param_H2.R += Band_Gray255_9th_data[band].R; //"xxxxxxxx" (bit) string form
                    Param_H2.G += Band_Gray255_9th_data[band].G; //"xxxxxxxx" (bit) string form
                    Param_H2.B += Band_Gray255_9th_data[band].B; //"xxxxxxxx" (bit) string form

                    //temp_mipi_cmd2.R += " 0x" + Band_8ea_Gray_data[band].int_R.ToString("X2");
                    //temp_mipi_cmd2.G += " 0x" + Band_8ea_Gray_data[band].int_G.ToString("X2");
                    //temp_mipi_cmd2.B += " 0x" + Band_8ea_Gray_data[band].int_B.ToString("X2");
                    temp_mipi_cmd2_R.Append(" 0x").Append(Band_8ea_Gray_data[band].int_R.ToString("X2"));
                    temp_mipi_cmd2_G.Append(" 0x").Append(Band_8ea_Gray_data[band].int_G.ToString("X2"));
                    temp_mipi_cmd2_B.Append(" 0x").Append(Band_8ea_Gray_data[band].int_B.ToString("X2"));
                }

                //hex format string (XX)
                Param_H1.R = String.Format("{0:X2}", Convert.ToUInt64(Param_H1.R, 2));
                Param_H1.G = String.Format("{0:X2}", Convert.ToUInt64(Param_H1.G, 2));
                Param_H1.B = String.Format("{0:X2}", Convert.ToUInt64(Param_H1.B, 2));
                Param_H2.R = String.Format("{0:X2}", Convert.ToUInt64(Param_H2.R, 2));
                Param_H2.G = String.Format("{0:X2}", Convert.ToUInt64(Param_H2.G, 2));
                Param_H2.B = String.Format("{0:X2}", Convert.ToUInt64(Param_H2.B, 2));

                //AM256[8] setting
                if (Gamma_Set1)
                {
                    //temp_mipi_cmd1.R = "mipi.write 0x39 0xEA 0x" + Param_H1.R + " 0x" + Param_H2.R;
                    //temp_mipi_cmd1.G = "mipi.write 0x39 0xEC 0x" + Param_H1.G + " 0x" + Param_H2.G;
                    //temp_mipi_cmd1.B = "mipi.write 0x39 0xEE 0x" + Param_H1.B + " 0x" + Param_H2.B;
                    temp_mipi_cmd1_R.Append("mipi.write 0x39 0xEA 0x").Append(Param_H1.R).Append(" 0x").Append(Param_H2.R);
                    temp_mipi_cmd1_G.Append("mipi.write 0x39 0xEC 0x").Append(Param_H1.G).Append(" 0x").Append(Param_H2.G);
                    temp_mipi_cmd1_B.Append("mipi.write 0x39 0xEE 0x").Append(Param_H1.B).Append(" 0x").Append(Param_H2.B);

                }
                else
                {
                    //temp_mipi_cmd1.R = "mipi.write 0x39 0xF0 0x" + Param_H1.R + " 0x" + Param_H2.R;
                    //temp_mipi_cmd1.G = "mipi.write 0x39 0xF2 0x" + Param_H1.G + " 0x" + Param_H2.G;
                    //temp_mipi_cmd1.B = "mipi.write 0x39 0xF4 0x" + Param_H1.B + " 0x" + Param_H2.B;
                    temp_mipi_cmd1_R.Append("mipi.write 0x39 0xF0 0x").Append(Param_H1.R).Append(" 0x").Append(Param_H2.R);
                    temp_mipi_cmd1_G.Append("mipi.write 0x39 0xF2 0x").Append(Param_H1.G).Append(" 0x").Append(Param_H2.G);
                    temp_mipi_cmd1_B.Append("mipi.write 0x39 0xF4 0x").Append(Param_H1.B).Append(" 0x").Append(Param_H2.B);
                }

                f1.DP116_CMD2_Page_Selection(11, false, false, ref Current_Page_Address, false);


                if (RGB_Need_To_Change[0])
                {
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Red Gray 255 Gamma Applied", Color.Red);
                    
                    f1.IPC_Quick_Send(temp_mipi_cmd1_R.ToString());
                    f1.IPC_Quick_Send(temp_mipi_cmd2_R.ToString());
                }

                if (RGB_Need_To_Change[1])
                {
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Green Gray 255 Gamma Applied", Color.Green);

                    f1.IPC_Quick_Send(temp_mipi_cmd1_G.ToString());
                    f1.IPC_Quick_Send(temp_mipi_cmd2_G.ToString());
                }

                if (RGB_Need_To_Change[2])
                {
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Blue Gray 255 Gamma Applied", Color.Blue);
                    f1.IPC_Quick_Send(temp_mipi_cmd1_B.ToString());
                    f1.IPC_Quick_Send(temp_mipi_cmd2_B.ToString());
                }


            }
            else//if Gray < 255 (Current Band의 Gray255 제외 모든 Gray 적용)
            {

                RGB[] Params = new RGB[12]; //B0h's Parameter amount = 12ea
                const int B0h = 176;//Hex = B0 , Dex = 176
                string R_Gamma_Address = (B0h + (Current_Band * 3)).ToString("X2");
                string G_Gamma_Address = (B0h + (Current_Band * 3) + 1).ToString("X2");
                string B_Gamma_Address = (B0h + (Current_Band * 3) + 2).ToString("X2");
                if (Current_Band == 9 || Current_Band == 10 || Current_Band == 11) //AOD 1,2,3
                {
                    R_Gamma_Address = (B0h + ((Current_Band + 2) * 3)).ToString("X2");
                    G_Gamma_Address = (B0h + ((Current_Band + 2) * 3) + 1).ToString("X2");
                    B_Gamma_Address = (B0h + ((Current_Band + 2) * 3) + 2).ToString("X2");
                }


                RGB[] temp = new RGB[3];

                //GR8
                Params[11].R = (All_band_gray_Gamma[Current_Band, 1].int_R & 0xFF).ToString("X2");
                Params[11].G = (All_band_gray_Gamma[Current_Band, 1].int_G & 0xFF).ToString("X2");
                Params[11].B = (All_band_gray_Gamma[Current_Band, 1].int_B & 0xFF).ToString("X2");
                temp[0].Set_Value((All_band_gray_Gamma[Current_Band, 1].int_R >> 8)
                    , (All_band_gray_Gamma[Current_Band, 1].int_G >> 8), (All_band_gray_Gamma[Current_Band, 1].int_B >> 8));

                //GR7
                Params[10].R = (All_band_gray_Gamma[Current_Band, 2].int_R & 0xFF).ToString("X2");
                Params[10].G = (All_band_gray_Gamma[Current_Band, 2].int_G & 0xFF).ToString("X2");
                Params[10].B = (All_band_gray_Gamma[Current_Band, 2].int_B & 0xFF).ToString("X2");
                temp[1].Set_Value((All_band_gray_Gamma[Current_Band, 2].int_R >> 8) << 1
                    , (All_band_gray_Gamma[Current_Band, 2].int_G >> 8) << 1, (All_band_gray_Gamma[Current_Band, 2].int_B >> 8) << 1);

                //GR6
                Params[9].R = (All_band_gray_Gamma[Current_Band, 3].int_R & 0xFF).ToString("X2");
                Params[9].G = (All_band_gray_Gamma[Current_Band, 3].int_G & 0xFF).ToString("X2");
                Params[9].B = (All_band_gray_Gamma[Current_Band, 3].int_B & 0xFF).ToString("X2");
                temp[2].Set_Value((All_band_gray_Gamma[Current_Band, 3].int_R >> 8) << 2
                    , (All_band_gray_Gamma[Current_Band, 3].int_G >> 8) << 2, (All_band_gray_Gamma[Current_Band, 3].int_B >> 8) << 2);

                //GR[8]678
                Params[8].Equal_Value(temp[0] + temp[1] + temp[2]);
                Params[8].R = (Params[8].int_R).ToString("X2");
                Params[8].G = (Params[8].int_G).ToString("X2");
                Params[8].B = (Params[8].int_B).ToString("X2");


                //GR5
                Params[7].R = (All_band_gray_Gamma[Current_Band, 4].int_R & 0xFF).ToString("X2");
                Params[7].G = (All_band_gray_Gamma[Current_Band, 4].int_G & 0xFF).ToString("X2");
                Params[7].B = (All_band_gray_Gamma[Current_Band, 4].int_B & 0xFF).ToString("X2");
                temp[0].Set_Value((All_band_gray_Gamma[Current_Band, 4].int_R >> 8)
                    , (All_band_gray_Gamma[Current_Band, 4].int_G >> 8), (All_band_gray_Gamma[Current_Band, 4].int_B >> 8));

                //GR4
                Params[6].R = (All_band_gray_Gamma[Current_Band, 5].int_R & 0xFF).ToString("X2");
                Params[6].G = (All_band_gray_Gamma[Current_Band, 5].int_G & 0xFF).ToString("X2");
                Params[6].B = (All_band_gray_Gamma[Current_Band, 5].int_B & 0xFF).ToString("X2");
                temp[1].Set_Value((All_band_gray_Gamma[Current_Band, 5].int_R >> 8) << 1
                    , (All_band_gray_Gamma[Current_Band, 5].int_G >> 8) << 1, (All_band_gray_Gamma[Current_Band, 5].int_B >> 8) << 1);

                //GR3
                Params[5].R = (All_band_gray_Gamma[Current_Band, 6].int_R & 0xFF).ToString("X2");
                Params[5].G = (All_band_gray_Gamma[Current_Band, 6].int_G & 0xFF).ToString("X2");
                Params[5].B = (All_band_gray_Gamma[Current_Band, 6].int_B & 0xFF).ToString("X2");
                temp[2].Set_Value((All_band_gray_Gamma[Current_Band, 6].int_R >> 8) << 2
                    , (All_band_gray_Gamma[Current_Band, 6].int_G >> 8) << 2, (All_band_gray_Gamma[Current_Band, 6].int_B >> 8) << 2);

                //GR[8]345
                Params[4].Equal_Value(temp[0] + temp[1] + temp[2]);
                Params[4].R = (Params[4].int_R).ToString("X2");
                Params[4].G = (Params[4].int_G).ToString("X2");
                Params[4].B = (Params[4].int_B).ToString("X2");


                //GR2
                Params[3].R = (All_band_gray_Gamma[Current_Band, 7].int_R & 0xFF).ToString("X2");
                Params[3].G = (All_band_gray_Gamma[Current_Band, 7].int_G & 0xFF).ToString("X2");
                Params[3].B = (All_band_gray_Gamma[Current_Band, 7].int_B & 0xFF).ToString("X2");
                temp[0].Set_Value((All_band_gray_Gamma[Current_Band, 7].int_R >> 8)
                    , (All_band_gray_Gamma[Current_Band, 7].int_G >> 8), (All_band_gray_Gamma[Current_Band, 7].int_B >> 8));

                //GR1
                Params[2].R = (All_band_gray_Gamma[Current_Band, 8].int_R & 0xFF).ToString("X2");
                Params[2].G = (All_band_gray_Gamma[Current_Band, 8].int_G & 0xFF).ToString("X2");
                Params[2].B = (All_band_gray_Gamma[Current_Band, 8].int_B & 0xFF).ToString("X2");
                temp[1].Set_Value((All_band_gray_Gamma[Current_Band, 8].int_R >> 8) << 1
                    , (All_band_gray_Gamma[Current_Band, 8].int_G >> 8) << 1, (All_band_gray_Gamma[Current_Band, 8].int_B >> 8) << 1);

                //GR0
                Params[1].R = (All_band_gray_Gamma[Current_Band, 9].int_R & 0xFF).ToString("X2");
                Params[1].G = (All_band_gray_Gamma[Current_Band, 9].int_G & 0xFF).ToString("X2");
                Params[1].B = (All_band_gray_Gamma[Current_Band, 9].int_B & 0xFF).ToString("X2");
                temp[2].Set_Value((All_band_gray_Gamma[Current_Band, 9].int_R >> 8) << 2
                    , (All_band_gray_Gamma[Current_Band, 9].int_G >> 8) << 2, (All_band_gray_Gamma[Current_Band, 9].int_B >> 8) << 2);

                //GR[8]012
                Params[0].Equal_Value(temp[0] + temp[1] + temp[2]);
                Params[0].R = (Params[0].int_R).ToString("X2");
                Params[0].G = (Params[0].int_G).ToString("X2");
                Params[0].B = (Params[0].int_B).ToString("X2");

                if (Gamma_Set1)
                {
                    f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, false);
                }
                else
                {
                    f1.DP116_CMD2_Page_Selection(1, false, false, ref Current_Page_Address, false);
                }

                //string temp_mipi_cmd = string.Empty;
                StringBuilder temp_mipi_cmd_R = new StringBuilder();
                StringBuilder temp_mipi_cmd_G = new StringBuilder();
                StringBuilder temp_mipi_cmd_B = new StringBuilder();

                //Apply Red Gamma
                if (RGB_Need_To_Change[0])
                {
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Red Gray 189~4 Gamma Applied", Color.Red);

                    //temp_mipi_cmd = "mipi.write 0x39 0x" + R_Gamma_Address;
                    temp_mipi_cmd_R.Append("mipi.write 0x39 0x").Append(R_Gamma_Address);

                    for (int i = 0; i < 12; i++)
                    {
                        //temp_mipi_cmd += " 0x" + Params[i].R;
                        temp_mipi_cmd_R.Append(" 0x").Append(Params[i].R);
                    }
                    f1.IPC_Quick_Send(temp_mipi_cmd_R.ToString());
                }

                //Apply Green Gamma
                if (RGB_Need_To_Change[1])
                {
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Green Gray 189~4 Gamma Applied", Color.Green);

                    //temp_mipi_cmd = "mipi.write 0x39 0x" + G_Gamma_Address;
                    temp_mipi_cmd_G.Append("mipi.write 0x39 0x").Append(G_Gamma_Address);

                    for (int i = 0; i < 12; i++)
                    {
                        //temp_mipi_cmd += " 0x" + Params[i].G;
                        temp_mipi_cmd_G.Append(" 0x").Append(Params[i].G);
                    }
                    f1.IPC_Quick_Send(temp_mipi_cmd_G.ToString());
                }

                //Apply Blue Gamma
                if (RGB_Need_To_Change[2])
                {
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Blue Gray 189~4 Gamma Applied", Color.Blue);

                    //temp_mipi_cmd = "mipi.write 0x39 0x" + B_Gamma_Address;
                    temp_mipi_cmd_B.Append("mipi.write 0x39 0x").Append(B_Gamma_Address);
                    for (int i = 0; i < 12; i++)
                    {
                        //temp_mipi_cmd += " 0x" + Params[i].B;
                        temp_mipi_cmd_B.Append(" 0x").Append(Params[i].B);
                    }
                    //f1.IPC_Quick_Send(temp_mipi_cmd);
                    f1.IPC_Quick_Send(temp_mipi_cmd_B.ToString());
                }
            }
        }

        public void DP116_DBV_Setting(int band)
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
                case 9://AOD1
                    button_A1_DBV_Send.PerformClick();
                    break;
                case 10://AOD2
                    button_A2_DBV_Send.PerformClick();
                    break;
                case 11://AOD3
                    button_A3_DBV_Send.PerformClick();
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                    break;
            }
            Thread.Sleep(100);
        }

        public void Special_Mode_Pattern_Setting(int band,int gray)
        {
            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
            string Band_Gray = form_engineer.Get_BX_GXXX_By_Gray_DP116(gray);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Gray = Convert.ToInt16(Band_Gray.Remove(0, 4)); //ex) B0_G255 --> 255 , A2_G91 --> 91 , A0_G4 --> 4

            f1.GB_Status_AppendText_Nextline("Band_Gray : " + Band_Gray + " / Gray : " + Gray.ToString(), System.Drawing.Color.Red);

            if (band == 9 || band == 10 || band == 11) //AOD Mode Pattern
            {
                int x1 = f1.current_model.get_AOD_X();
                int y1 = f1.current_model.get_AOD_Y();
                f1.IPC_Quick_Send("image.crosstalk " + x1.ToString() + " " + y1.ToString() + " 0 0 0 " + Gray.ToString() + " " + Gray.ToString() + " " + Gray.ToString());
            }
            else //Normal Mode Pattern
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

        public void DP116_GrayPoints_Pattern_Setting(bool AOD,int gray)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Gray = 255;

            switch (gray)
            {
                case 0:
                    Gray = 255;
                    break;
                case 1:
                    Gray = 189;
                    break;
                case 2:
                    Gray = 134;
                    break;
                case 3:
                    Gray = 91;
                    break;
                case 4:
                    Gray = 60;
                    break;
                case 5:
                    Gray = 38;
                    break;
                case 6:
                    Gray = 23;
                    break;
                case 7:
                    Gray = 13;
                    break;
                case 8:
                    Gray = 6;
                    break;
                case 9:
                    Gray = 4;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Gray is out of boundary");
                    break;
            }

            if (AOD) //AOD Mode Pattern
            {
                int x1 = f1.current_model.get_AOD_X();
                int y1 = f1.current_model.get_AOD_Y();
                f1.Image_Crosstalk(x1,y1,0,0,0,Gray,Gray,Gray);
                f1.GB_Status_AppendText_Nextline("AOD Gray" + Gray.ToString() + " Setting", System.Drawing.Color.Black);
            }
            else //Normal Mode Pattern
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



        public void DP116_Pattern_Setting(int gray, int band,bool DP116_Or_DP150_116_Is_True)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Gray = 255;
            if (checkBox_Special_Gray_Compensation.Checked)
            {
                string Band_Gray = string.Empty;
                if (DP116_Or_DP150_116_Is_True) //DP116 
                {
                    Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
                    Band_Gray = form_engineer.Get_BX_GXXX_By_Gray_DP116(gray);
                }
                else //DP150 Load form 1st Dual View
                {
                    Dual_Engineer_Monitoring_Mode form_dual_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"]; 
                    Band_Gray = form_dual_engineer.Dual_Get_BX_GXXX_By_Gray_DP116(gray);
                }
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
                        Gray = 189;
                        break;
                    case 2:
                        Gray = 134;
                        break;
                    case 3:
                        Gray = 91;
                        break;
                    case 4:
                        Gray = 60;
                        break;
                    case 5:
                        Gray = 38;
                        break;
                    case 6:
                        Gray = 23;
                        break;
                    case 7:
                        Gray = 13;
                        break;
                    case 8:
                        Gray = 6;
                        break;
                    case 9:
                        Gray = 4;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Gray is out of boundary");
                        break;
                }
            }

            if (band == 9 || band == 10 || band == 11) //AOD Mode Pattern
            {
                int x1 = f1.current_model.get_AOD_X();
                int y1 = f1.current_model.get_AOD_Y();
                f1.Image_Crosstalk(x1, y1, 0, 0, 0, Gray, Gray, Gray);
                f1.GB_Status_AppendText_Nextline("AOD Gray" + Gray.ToString() + " Setting", System.Drawing.Color.Black);
            }
            else //Normal Mode Pattern
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

        private bool ProgressBar_Max_Step_Setting(int step)
        {
            int ProgressBar_max = 0;

            if (checkBox_VREF2_AM0_Compensation.Checked) ProgressBar_max += step;
            
            //How many BSQH Points are checked ? (graypoints = 10)
            if (checkBox_Band1.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band2.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band3.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band4.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band5.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band6.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band7.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band8.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band9.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band10.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band11.Checked) ProgressBar_max += (10 * step);
            if (checkBox_Band12.Checked) ProgressBar_max += (10 * step);

            bool If_Any_Band_Is_Selected;
            if (ProgressBar_max == 0)
                If_Any_Band_Is_Selected = false;
            else
                If_Any_Band_Is_Selected = true;


            //OTP Auto Write checked ? 
            if (checkBox_1st_Mode_OTP_AutoWirte.Checked) ProgressBar_max += (5 * step);

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Set_GB_ProgressBar_Maximum(ProgressBar_max);
            f1.Set_GB_ProgressBar_Step(step);

            return If_Any_Band_Is_Selected;
        }



        private bool VREF2_AM0_Compensation(double VREF2_Margin,double Black_Margin ,double VREF_Resolution,double VREF2_Limit_Lv)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("VREF2-AM0 Compensation Start", Color.Black);
            f1.PTN_update(0, 0, 0);
            f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address,true);//CMD2 P0


            f1.Read_DP116_Page_Quantity_Register(0, 1, "AE");
            int VREF2_opt = Convert.ToInt16(f1.dataGridView1.Rows[0].Cells[1].Value.ToString(), 16) & 0x80;

            f1.Read_DP116_Page_Quantity_Register(0, 1, "AF");
            int AM0_opt = Convert.ToInt16(f1.dataGridView1.Rows[0].Cells[1].Value.ToString(), 16) & 0x80;

            int Data_VREF2 = 0;
            int Data_AM0 = 0;
            int AM0 = 0;
            for (; AM0 < 128; AM0++)
            {
                if (Optic_Compensation_Stop) break;

                if (AM0 == 127)
                {
                    f1.GB_Status_AppendText_Nextline("VREF2 Compensation Fail (Loop Count = Loop Count Max)", Color.Red);
                    return true;
                }

                if (Data_VREF2 == 0)
                {
                    //Black Setting (Temp)
                    Data_AM0 = AM0_opt + AM0;
                    f1.IPC_Quick_Send("mipi.write 0x15 0xAF 0x" + Data_AM0.ToString("X2"));
                    Thread.Sleep(20);//Add on 190820
                    f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);

                    if (Measure.double_Lv > VREF2_Limit_Lv)
                    {
                        Data_VREF2 = (AM0 - 1) - Convert.ToInt16(VREF2_Margin / VREF_Resolution + 0.5);
                        break;
                    }
                }
            }

            if (Optic_Compensation_Stop == false)
            {
                f1.GB_Status_AppendText_Nextline("VREF2 Compensation Succeed", Color.Green);

                Data_AM0 = Data_VREF2 - Convert.ToInt16((Black_Margin - VREF2_Margin) / VREF_Resolution + 0.5);
                //Data_AM0 = (AM0 - 1) - Convert.ToInt16(Black_Margin / VREF_Resolution + 0.5);
                f1.GB_Status_AppendText_Nextline("Prev Data_AM0 : " + Data_AM0.ToString(), Color.Blue);
                f1.GB_Status_AppendText_Nextline("Prev Data_VREF2 : " + Data_VREF2.ToString(), Color.Blue);
                if (Data_AM0 <= 0)
                {
                    f1.GB_Status_AppendText_Nextline("Black Margin Fail (Data_AM0 <= 0)", Color.Red);
                    return true;
                    //Data_AM0 = 0;
                }
                Data_AM0 = AM0_opt + Data_AM0;
                f1.IPC_Quick_Send("mipi.write 0x15 0xAF 0x" + Data_AM0.ToString("X2"));
                f1.GB_Status_AppendText_Nextline("After AM0 : " + (Data_AM0 - AM0_opt).ToString(), Color.Black);
                textBox_AM0.Text = (Data_AM0 - AM0_opt).ToString();

                if (Data_VREF2 <= 0) Data_VREF2 = 0;
                Data_VREF2 = VREF2_opt + Data_VREF2;
                f1.IPC_Quick_Send("mipi.write 0x15 0xAE 0x" + Data_VREF2.ToString("X2"));
                f1.GB_Status_AppendText_Nextline("After VREF2 : " + (Data_VREF2 - VREF2_opt).ToString(), Color.Black);
                textBox_VREF2.Text = (Data_VREF2 - VREF2_opt).ToString();

                f1.GB_ProgressBar_PerformStep();
            }

            button_Read_AM0_VREF2.PerformClick();

            return false;
        }

        //private void Vreg1_Infinite_Loop_Check(ref bool Vreg1_Infinite, int Vreg1_loop_count,int Vreg1)
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


        private void Infinite_Loop_Check(int loop_count)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (loop_count == 0) Temp_Gamma[0].Equal_Value(Gamma);
            else if (loop_count == 1)Temp_Gamma[1].Equal_Value(Gamma);
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

                if (Infinite) f1.GB_Status_AppendText_Nextline("Infinite : " +  Infinite.ToString() , Color.Red);
                else f1.GB_Status_AppendText_Nextline("Infinite : " + Infinite.ToString(), Color.Green);

                if (Infinite_Count >= 3)
                    f1.GB_Status_AppendText_Nextline("Infinite_Count = " + Infinite_Count.ToString(), Color.Red);
                else
                    f1.GB_Status_AppendText_Nextline("Infinite_Count = " + Infinite_Count.ToString(), Color.Green);
            }
        }

        private void Band1_Gray255_Vreg1_Compensation(bool Condition)
        {
            int loop_count = 0;
            Optic_Compensation_Succeed = false;
            Optic_Compensation_Stop = false;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //f1.Engineering_Mode_Show();
            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];

            form_engineer.Band_Radiobuttion_Select(1);//Select Band
            DP116_DBV_Setting(1);//DBV Setting
            Get_Param(0); //Get (First)Gamma,Target,Limit From OC-Param-Table                            
            Vreg1 = Vreg1_Read2(1, Condition, true); //Read Vreg1 Value
            DP116_Pattern_Setting(0, 1, DP116_Or_DP150_116_Is_True);//Pattern Setting
            Thread.Sleep(300); //Pattern 안정화 Time
            Vreg1_Infinite = false;

            Optic_Compensation_Succeed = false;
            Optic_Compensation_Stop = false;

            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
            {
                Update_and_Send_All_Band_Gray_Gamma(Gamma, 1, 0, Condition); //Setting Gamma Values
                Update_and_Send_Vreg1(Vreg1, 1, Condition);
                Thread.Sleep(20);
                f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure

                Imported_my_cpp_dll.Vreg1_Compensation(loop_count, false, 5, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, 511, 4095, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

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
                    if (this.checkBox_Continue_After_Fail.Checked == false)
                        Optic_Compensation_Stop = true;
                    break;
                }
                Update_Engineering_Sheet(1, 0, loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
            }

            /*
            //datagridview-related
            form_engineer.Band_Radiobuttion_Select(1);//Select Band
            DP116_DBV_Setting(1);//DBV Setting
            Get_Param(0); //Get (First)Gamma,Target,Limit From OC-Param-Table                            
            Vreg1 = Vreg1_Read2(1); //Read Vreg1 Value
            DP116_Pattern_Setting(0, 1);//Pattern Setting
            Thread.Sleep(300); //Pattern 안정화 Time
            Vreg1_Infinite = false;

            //int loop_count = 0;
            //Optic_Compensation_Succeed = false;
            //Optic_Compensation_Stop = false;

            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
            {
                Update_and_Send_All_Band_Gray_Gamma(Gamma, 1, 0); //Setting Gamma Values
                Update_and_Send_Vreg1(Vreg1, 1);
                Thread.Sleep(100);
                f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure

                Vreg1_Compensation(false, 5, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, 511, 4095, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                f1.GB_Status_AppendText_Nextline("B1" + " Red/Vreg1/Blue = " + Gamma.int_R.ToString() + "," + Vreg1.ToString() + "," + Gamma.int_B.ToString(), Color.Black);

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
                    System.Windows.Forms.MessageBox.Show("Band1_Gray255_Vreg1_Compensation Loop Count Over");
                    if (this.checkBox_Continue_After_Fail.Checked == false)
                        Optic_Compensation_Stop = true;
                    break;
                }
                Update_Engineering_Sheet(1, 0, loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
            }*/
        }

        private void IRC_Compensation()
        {
            if (Optic_Compensation_Stop)
            {
            }
            else
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];

                f1.Select_Channel_For_Red();
                f1.PTN_update(255, 0, 0);//Red
                Thread.Sleep(200); //Pattern 안정화 Time
                f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                double Measure_Red_IRC = Measure.double_Lv;

                f1.Select_Channel_For_Green();
                f1.PTN_update(0, 255, 0);//Green
                Thread.Sleep(200); //Pattern 안정화 Time
                f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                double Measure_Green_IRC = Measure.double_Lv;

                f1.Select_Channel_For_Blue();
                f1.PTN_update(0, 0, 255);//Blue
                Thread.Sleep(200); //Pattern 안정화 Time
                f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                double Measure_Blue_IRC = Measure.double_Lv;

                f1.Select_Channel_For_White();//Wjite 로 채널 변경

                double Total_RGB_IRC = Measure_Red_IRC + Measure_Green_IRC + Measure_Blue_IRC;
                IRC_RGB_Sum_Textbox.Text = Total_RGB_IRC.ToString();

                double Reference_IRC = Convert.ToDouble(this.IRC_Textbox.Text);
                double Diff_RGB_IRC = Total_RGB_IRC - Reference_IRC;
                double Gain_Offset_IRC = Diff_RGB_IRC / 5.0;

                //190522 Gain_Offset_IRC Limit Setting
                if (Gain_Offset_IRC < Convert.ToDouble(textBox_Gain_Offset_Min.Text))
                {
                    Gain_Offset_IRC = Convert.ToDouble(textBox_Gain_Offset_Min.Text);
                    f1.GB_Status_AppendText_Nextline("IRC Gain Offset Min Value Applied : " + Gain_Offset_IRC.ToString(), Color.Red);
                }
                else if (Gain_Offset_IRC > Convert.ToDouble(textBox_Gain_Offset_Max.Text))
                {
                    Gain_Offset_IRC = Convert.ToDouble(textBox_Gain_Offset_Max.Text);
                    f1.GB_Status_AppendText_Nextline("IRC Gain Offset Max Value Applied : " + Gain_Offset_IRC.ToString(), Color.Red);
                }
                else
                {
                    f1.GB_Status_AppendText_Nextline("IRC Gain Offset is within Limit : " + Gain_Offset_IRC.ToString(), Color.Blue);
                }

                textBox_Calculated_IRC_Gain_Offset.Text = Gain_Offset_IRC.ToString();

                int Int_Gain_Offset_IRC = (int)Gain_Offset_IRC;

                textBox_Applied_IRC_Gain_Offset.Text = Int_Gain_Offset_IRC.ToString();

                f1.Read_DP116_Page_Quantity_Register(12, 16, "4B");
                f1.GB_Status_AppendText_Nextline("Before IRC Compensation (4Bh) : " + f1.textBox2_cmd.Text.Substring(18, 16 * 5), Color.Red);


                string D1 = (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[11].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 1)).ToString("X2");
                string D2 = (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[12].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 2)).ToString("X2");
                string D3 = (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[13].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 3)).ToString("X2");
                string D4 = (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[14].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 4)).ToString("X2");
                string D5 = (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[15].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 5)).ToString("X2");

                if (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[11].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 1) < 0) D1 = "00";
                if (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[12].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 2) < 0) D2 = "00";
                if (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[13].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 3) < 0) D3 = "00";
                if (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[14].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 4) < 0) D4 = "00";
                if (Convert.ToInt16(Convert.ToInt16(f1.dataGridView1.Rows[15].Cells[1].Value.ToString(), 16) + Int_Gain_Offset_IRC * 5) < 0) D5 = "00";


                string[] CMD_IRC = new string[16];
                for (int i = 0; i < 11; i++)
                    CMD_IRC[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                CMD_IRC[11] = D1;
                CMD_IRC[12] = D2;
                CMD_IRC[13] = D3;
                CMD_IRC[14] = D4;
                CMD_IRC[15] = D5;

                f1.Long_Packet_CMD_Send(16, "4B", CMD_IRC);
                f1.Read_DP116_Page_Quantity_Register(12, 16, "4B");
                f1.GB_Status_AppendText_Nextline("After IRC Compensation (4Bh) : " + f1.textBox2_cmd.Text.Substring(18, 16 * 5), Color.Green);

            }
        }

        private void Update_OC_Gamma_From_Log(RGB[, ,] RGBVre1_Log_data)
        {
            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
            //190528 Add
            int[] Gamma_R = new int[10]; //{ 99, 23, 44, 51, 66, 77, 88, 33, 11, 111 };
            int[] Gamma_G = new int[10]; //{ 99, 23, 44, 111, 59, 66, 77, 33, 88, 11 };
            int[] Gamma_B = new int[10]; //{ 99, 23, 44, 55, 33, 77, 88, 22, 11, 1 };
            int Out_R = 0;
            int Out_G = 0;
            int Out_B = 0;

            // form_engineer.Set_OC_Gamma_DP116(band
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    for (int sample = 0; sample < 10; sample++)
                    {
                        Gamma_R[sample] = RGBVre1_Log_data[band, gray, sample].int_R;
                        Gamma_G[sample] = RGBVre1_Log_data[band, gray, sample].int_G;
                        Gamma_B[sample] = RGBVre1_Log_data[band, gray, sample].int_B;
                    }
                    Imported_my_cpp_dll.RGB_Gamma_Initial_Values(10, 100, 500, Gamma_R, Gamma_G, Gamma_B, ref Out_R, ref Out_G, ref Out_B);
                    form_engineer.Set_OC_Gamma_DP116(band, gray, Out_R, Out_G, Out_B);
                }
            }
            form_engineer.Set_Sub_OC_Gamma_DP116();
        }



        private void Update_OC_Offset_Gamma_From_Log(RGB[, ,] RGBVre1_Log_Offset_data,int band)
        {
            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
            //190528 Add
            int[] Offset_R = new int[10]; //{ 99, 23, 44, 51, 66, 77, 88, 33, 11, 111 };
            int[] Offset_G = new int[10]; //{ 99, 23, 44, 111, 59, 66, 77, 33, 88, 11 };
            int[] Offset_B = new int[10]; //{ 99, 23, 44, 55, 33, 77, 88, 22, 11, 1 };
            int Out_Offset_R = 0;
            int Out_Offset_G = 0;
            int Out_Offset_B = 0;

            // Offset Apply to Band1~Band8
            for (int gray = 0; gray < 10; gray++)
            {
                for (int sample = 0; sample < 10; sample++)
                {
                    Offset_R[sample] = RGBVre1_Log_Offset_data[band, gray, sample].int_R;
                    Offset_G[sample] = RGBVre1_Log_Offset_data[band, gray, sample].int_G;
                    Offset_B[sample] = RGBVre1_Log_Offset_data[band, gray, sample].int_B;
                }
                Imported_my_cpp_dll.RGB_Gamma_Initial_Values(10, 0, 100, Offset_R, Offset_G, Offset_B, ref Out_Offset_R, ref Out_Offset_G, ref Out_Offset_B);
                form_engineer.Set_OC_Offset_Gamma_DP116(band, gray, Out_Offset_R, Out_Offset_G, Out_Offset_B,500,100);
            }
            form_engineer.Set_Sub_OC_Gamma_DP116();
        }
        

        private void Single_Mode_Optic_compensation()
        {
            bool Vreg1_Need_To_Be_Updated = false;
            //datagridview-related
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //f1.Engineering_Mode_Show(); //Delete on 190605
            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
            form_engineer.Engineering_Mode_DataGridview_ReadOnly(true);
            form_engineer.GridView_Measure_Applied_Loop_Area_Data_Clear();
            form_engineer.Gamma_Vreg1_Diff_Clear();
            form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB();
            Application.DoEvents();

            //Initialize
            Optic_Compensation_Stop = false;
            Textbox_ELVSS_Vinit_Clear();
            Textbox_AM0_VREF2_Clear();
            textBox_Applied_IRC_Gain_Offset.Text = string.Empty;
            Application.DoEvents();
            Initalize_Vreg1(true); //Update DD[]/DE[]

            //CA channel Selection
            f1.objMemory.ChannelNO = Convert.ToInt32(f1.textBox_ch.Text);
            f1.CA_Channel_trackBar.Value = Convert.ToInt32(f1.textBox_ch.Text);

            //Timer Start
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
            double Skip_Lv = Convert.ToDouble(textBox_Subcompensation_GB_skip_Lv.Text);
            RGB Prev_Band_Gray255_Gamma = new RGB();
            Optic_Compensation_Succeed = false;
            RGB Gamma_Init = new RGB();

            //ProgressBar-related
            int step = 1;
            f1.Set_GB_ProgressBar_Value(0); //Progressbar Value's initializing
            bool Any_Band_is_Selected = ProgressBar_Max_Step_Setting(step); //Set Progressbar's Step and Max-Value


            string Path = string.Empty;
            //----GET CSV Gamma/Vreg Log (190528) Start----
            if (checkBox_Get_Init_Gamma_Vreg1.Checked || checkBox_Save_Init_Gamma_to_Log.Checked)
            {
                if (f1.label_Model_Name.Text == "Model Name : DP086")
                    Path = Directory.GetCurrentDirectory() + "\\DP086\\DP086_Gamma_Vreg1_Log.csv";
                else
                    Path = Directory.GetCurrentDirectory() + "\\DP116\\DP116_Gamma_Vreg1_Log.csv";
                f1.GB_Status_AppendText_Nextline("Read Data From Log csv", Color.Blue);
                Read_Data_From_Log(Path, ref RGBVre1_Log_Read_data);
                f1.GB_Status_AppendText_Nextline("Update RGBVre1_Log_data[Band,Gray,Sample]", Color.Blue);
                Get_Update_RGBVre1_Log_data(RGBVre1_Log_Read_data, RGBVre1_Log_data);
            }

            if (checkBox_Get_Offset_Gamma_Vreg1.Checked || checkBox_Save_Offset_Gamma_to_Log.Checked)
            {
                if (f1.label_Model_Name.Text == "Model Name : DP086")
                    Path = Directory.GetCurrentDirectory() + "\\DP086\\DP116_Offset_Log.csv";
                else
                    Path = Directory.GetCurrentDirectory() + "\\DP116\\DP116_Offset_Log.csv";

                f1.GB_Status_AppendText_Nextline("Read Offset Data From Log csv", Color.Blue);
                Read_Data_From_Log(Path, ref RGBVre1_Log_Read_Offset_data);
                f1.GB_Status_AppendText_Nextline("Update RGBVre1_Log_Offset_data[Band,Gray,Sample]", Color.Blue);
                Get_Update_RGBVre1_Log_data(RGBVre1_Log_Read_Offset_data, RGBVre1_Log_Offset_data);
            }
            
            //---------------------------------------------
            if (checkBox_Get_Init_Gamma_Vreg1.Checked)
            {
                //Update OC from Log
                f1.GB_Status_AppendText_Nextline("Update OC Gamma(Initial Gamma) from Log", Color.Blue);
                Update_OC_Gamma_From_Log(RGBVre1_Log_data);
                form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB();
                Application.DoEvents();
            }


            if (checkBox_Send_Manual_Code.Checked)
            {
                f1.PNC_Manual_Button_Click();
                Thread.Sleep(3000); //Manual Code 안정화 Time
            }

            if (checkBox_Read_DBV_Values.Checked) button_Read_DP116_DBV_Setting.PerformClick();

            if (checkBox_VREF2_AM0_Compensation.Checked && Optic_Compensation_Stop == false)
            {
                double VREF2_Margin = Convert.ToDouble(textBox_VREF2_Margin.Text);
                double Black_Margin = Convert.ToDouble(textBox_AM0_Margin.Text);
                double VREF_Resolution = Convert.ToDouble(textBox_VREF_Resolution.Text);
                double VREF2_Limit_Lv = Convert.ToDouble(textBox_VREF2_Limit_Lv.Text);
                Optic_Compensation_Stop = VREF2_AM0_Compensation(VREF2_Margin, Black_Margin, VREF_Resolution, VREF2_Limit_Lv);
            }

            // IRC & ELVSS Compensation
            if ((checkBox_IRC_Comp.Checked || checkBox_ELVSS_Comp.Checked) && Optic_Compensation_Stop == false)
            {
                //IRC Off
                this.button_IRC_Off.PerformClick();
                Thread.Sleep(100);

                Get_Param(0); //Get (First)Gamma,Target,Limit From OC-Param-Table
                Gamma_Init.Equal_Value(Gamma);//190529
                HBM_Mode_Gray255_Compensation();
                if (checkBox_IRC_Comp.Checked && Optic_Compensation_Stop == false) IRC_Compensation();
                if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false) ELVSS_Compensation();
                //IRC On
                this.button_IRC_On.PerformClick();
            }
            
            if (Any_Band_is_Selected)
            {
                f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address,false);
                f1.IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0, Disable All Registers

                Get_All_Band_Gray_Gamma(); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                for (band = 0; band < 12; band++)
                {
                    f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                    if (Optic_Compensation_Stop) break;
                    Gamma_Out_Of_Register_Limit = false;
                    //Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
                    if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                    {
                        if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            f1.AOD_On();
                            DP116_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                        }

                        form_engineer.Band_Radiobuttion_Select(band);//Select Band
                        Thread.Sleep(300);

                        DP116_DBV_Setting(band);  //DBV Setting
                        
                        Vreg1_loop_count = 0; //Vreg1 loop countR
                        Vreg1_Infinite_Count = 0;

                        if (checkBox_Get_Init_Gamma_Vreg1.Checked)
                        {
                            f1.GB_Status_AppendText_Nextline("Log Data Init Vreg1 is applied", Color.Blue);
                            int[] Vreg1_Array = new int[10];
                            for (int sample = 0; sample < 10; sample++) Vreg1_Array[sample] = RGBVre1_Log_data[band, 0, sample].int_Vreg1;
                            Vreg1 = Imported_my_cpp_dll.Vreg1_Initial_Values(10, Vreg1_Array);
                        }
                        else if (checkBox_Get_Offset_Gamma_Vreg1.Checked)
                        {
                            if (band == 0)
                            {
                                f1.GB_Status_AppendText_Nextline("HBM (Offset Apply X)", Color.Black);
                                Vreg1 = Vreg1_Read2(band, true, true); //Read Vreg1 Value
                            }
                            else if (band == 9)
                            {
                                f1.GB_Status_AppendText_Nextline("AOD 0 (Offset Apply X)", Color.Black);
                                Vreg1 = Vreg1_Read2(band, true, true); //Read Vreg1 Value
                            }
                            else
                            {
                                int Pre_Vreg1 = Vreg1_Read2(band - 1, true, true); //Read Previous Vreg1 Value
                                f1.GB_Status_AppendText_Nextline("Log Data Offset Vreg1 is applied", Color.Blue);
                                int[] Vreg1_Offset_Array = new int[10];
                                for (int sample = 0; sample < 10; sample++) Vreg1_Offset_Array[sample] = RGBVre1_Log_Offset_data[band, 0, sample].int_Vreg1;
                                int Offset = Imported_my_cpp_dll.Vreg1_Initial_Values(10, Vreg1_Offset_Array);
                                Vreg1 = Pre_Vreg1 + Offset;
                                if (Vreg1 > 4095) Vreg1 = 4095;
                                else if (Vreg1 < 100) Vreg1 = 100;
                            }
                        }

                        else
                        {
                            f1.GB_Status_AppendText_Nextline("Normal Init Vreg1 is applied", Color.Blue);
                            Vreg1 = Vreg1_Read2(band, true, true); //Read Vreg1 Value
                            
                        }

                        Update_Vreg1_TextBox(Vreg1,band,true);
                        int Initial_Vreg1 = Vreg1;
                        int Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                        form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(band, Initial_Vreg1);
                        form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);

                        if (checkBox_Get_Offset_Gamma_Vreg1.Checked) //If Offset Mode Checked   
                        {
                            if (band == 0) f1.GB_Status_AppendText_Nextline("HBM (Offset Apply X)", Color.Red);
                            else if (band == 9) f1.GB_Status_AppendText_Nextline("AOD 0 (Offset Apply X)", Color.Red);
                            else
                            {
                                if (Band_Selection_Check(band - 1))//Apply
                                {
                                    f1.GB_Status_AppendText_Nextline("Band" + band.ToString() + " apply offset from Band" + (band - 1).ToString(), Color.Blue);
                                    Update_OC_Offset_Gamma_From_Log(RGBVre1_Log_Offset_data, band);
                                    form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Band_Init_RGB(band);
                                }
                                else
                                {
                                    f1.GB_Status_AppendText_Nextline("Band" + band.ToString() + " offset wasn't applied because the previous band was not compensated", Color.Red);
                                }
                            }
                        }

                        for (gray = 0; gray < 10; gray++)
                        {
                            if (Optic_Compensation_Stop) break;
                            Get_Param(gray); //Get (First)Gamma,Target,Limit From OC-Param-Table  

                            //HBM의 Gray255꺼는 IRC 보상 안하면 그냥 받음
                            if (checkBox_IRC_Comp.Checked == false && checkBox_ELVSS_Comp.Checked == false)
                            {
                                Gamma_Init.Equal_Value(Gamma);
                            }
                            else if (checkBox_IRC_Comp.Checked || checkBox_ELVSS_Comp.Checked)
                            {
                                if ((band == 0 && gray == 0) == false)
                                {
                                    Gamma_Init.Equal_Value(Gamma);
                                }
                                else
                                {
                                    //HBM의 Gray255꺼는 IRC/ELVSS 보상 이전의 Init Gamma 받음
                                }
                            }
                           
                            
                            if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                            DP116_Pattern_Setting(gray, band, DP116_Or_DP150_116_Is_True);//Pattern Setting
                            
                            if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                            {
                                    form_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                    Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                            }

                            RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                            Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray,true); //Setting Gamma Values
                            Thread.Sleep(300); //Pattern 안정화 Time
                            //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                            DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
                                , Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                            loop_count = 0;
                            Infinite_Count = 0;
                            Update_Engineering_Sheet(band, gray, loop_count, "X");

                            Optic_Compensation_Succeed = false;
                            Within_Spec_Limit = false;

                            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                            {
                                if (Target.double_Lv < Skip_Lv)
                                {
                                    if (band >= 1)
                                    {
                                        form_engineer.Get_Gamma_Only_DP116(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                        Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                    }
                                    RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                    Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, true); //Setting Gamma Values
                                    Measure.Set_Value(0, 0, 0);
                                    Update_Engineering_Sheet(band, gray, loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                        + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                    Optic_Compensation_Succeed = true;
                                    break;
                                }

                                //Vreg1 + Sub-Compensation (Change Gamma Value)
                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                {
                                    if (Vreg1_loop_count == 0)
                                    {
                                        form_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                        Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                    }

                                    Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                    if (Vreg1_loop_count < loop_count_max)
                                    {
                                        //f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                       
                                        
                                        Prev_Vreg1 = Vreg1;
                                        Prev_Gamma.Equal_Value(Gamma);

                                        Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                        Diff_Vreg1 = Vreg1 - Prev_Vreg1;
                                        Diff_Gamma = Gamma - Prev_Gamma;

                                        //f1.GB_Status_AppendText_Nextline("B" + band.ToString() + " Red/Vreg1/Blue = " + Gamma.int_R.ToString() + "," + Vreg1.ToString() + "," + Gamma.int_B.ToString(), Color.Black);
                                        f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);
                                        if (Math.Abs(Diff_Vreg1) >= 1)
                                            Vreg1_Need_To_Be_Updated = true;
                                        else
                                            Vreg1_Need_To_Be_Updated = false;

                                        if (Vreg1_Need_To_Be_Updated)
                                        {
                                            //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                            Update_and_Send_Vreg1(Vreg1, band, true);
                                            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                            Update_Vreg1_TextBox(Vreg1, band,true);
                                            Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                                            form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
                                        }
                                    }
                                    Vreg1_loop_count++;
                                    loop_count++;
                                    
                                    if (Vreg1_Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }
                                else
                                {
                                    Vreg1_Need_To_Be_Updated = false;

                                    Prev_Gamma.Equal_Value(Gamma);
                                    Infinite_Loop_Check(loop_count);

                                    Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                    //Engineering Mode
                                    Diff_Gamma = Gamma - Prev_Gamma;
                                    f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                                    loop_count++;

                                    if (Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }
                                if (Vreg1_Need_To_Be_Updated == false)
                                {
                                    if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                                    else RGB_Need_To_Change[0] = false;
                                    if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                                    else RGB_Need_To_Change[1] = false;
                                    if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                                    else RGB_Need_To_Change[2] = false;
                                    //f1.GB_Status_AppendText_Nextline("Gamma Setting", Color.Blue);
                                    Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, true); //Setting Gamma Values
                                    Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                    int DIff_R = Gamma.int_R - Gamma_Init.int_R;
                                    int DIff_G = Gamma.int_G - Gamma_Init.int_G;
                                    int DIff_B = Gamma.int_B - Gamma_Init.int_B;

                                    form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(band,gray, DIff_R, DIff_G, DIff_B);
                                }
                                //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure
                  
                                if (Within_Spec_Limit)
                                {
                                    if (gray == 0 && band < 9 && checkBox_Copy_Final_G255_to_Others.Checked)
                                    {
                                        form_engineer.SubGridView_Copy_Final_G255_Measured_xy_to_Other_Grays_Target();
                                        if (band <= 1 && checkBox_G189_Offset_From_G255.Checked)
                                        {
                                            form_engineer.SubGridView_G189_xy_Offset_From_G255_xy(band);
                                        }
                                    }
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

                                DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Measured Lv : " + Measure.double_Lv.ToString(), Color.Black);
                                Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();
                            }
                            f1.GB_ProgressBar_PerformStep();
                        }
                        if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            f1.AOD_Off();
                        }
                    }
                    //VREF2_and_AM0_Read();
                }
                f1.OC_Timer_Stop();//GB Timer Stop
                form_engineer.Engineering_Mode_DataGridview_ReadOnly(false);

                //if Optic Compensation is finished , then....
                if (Optic_Compensation_Stop == false && Gamma_Out_Of_Register_Limit == false)
                {
                    if (checkBox_1st_Mode_OTP_AutoWirte.Checked)
                    {
                        f1.CRC_Check();
                        f1.First_Model_OTP_Write_Button_Click();
                        f1.ADD_GB_ProgressBar_Value(5 * step);
                    }

                    if (checkBox_Save_Init_Gamma_to_Log.Checked)
                    {
                        //----Down CSV Gamma/Vreg Log (190528) Start----
                        Get_All_Band_Gray_Gamma(); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                        Get_All_Band_Vreg1(All_band_gray_Gamma); //Get update Vreg1 to All_band_gray_Gamma[band,0]
                        Shift_and_Get_New_Data(RGBVre1_Log_data, All_band_gray_Gamma);
                        string appendText = string.Empty;
                        Get_Init_String(ref appendText, RGBVre1_Log_Read_data);
                        Merge_Text(ref appendText, RGBVre1_Log_data);
                        File.WriteAllText(Path, appendText);//Clear and Write
                        f1.GB_Status_AppendText_Nextline("Update Log CSV Finished", Color.Blue);
                    }

                    if (this.checkBox_Save_Offset_Gamma_to_Log.Checked)
                    {
                        Get_All_Band_Gray_Gamma(); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                        Get_All_Band_Vreg1(All_band_gray_Gamma); //Get update Vreg1 to All_band_gray_Gamma[band,0]
                        Shift_and_Get_New_Offset_Data(RGBVre1_Log_Offset_data, All_band_gray_Gamma);
                        string appendText = string.Empty;
                        Get_Init_String(ref appendText, RGBVre1_Log_Read_Offset_data);
                        Merge_Text(ref appendText, this.RGBVre1_Log_Offset_data);
                        File.WriteAllText(Path, appendText);//Clear and Write
                        f1.GB_Status_AppendText_Nextline("Update Log CSV Finished", Color.Blue);
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



        private void Dual_Mode_3_Optic_compensation()
        {
            bool Vreg1_Need_To_Be_Updated = false; //Add on 190603

            //datagridview-related
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //f1.Dual_Engineering_Mode_Show(); //Delete on 190605
            Dual_Engineer_Monitoring_Mode form_dual_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            form_dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(true);
            form_dual_engineer.Dual_Mode_GridView_Measure_Extension_LoopCound_Area_Data_Clear();
            form_dual_engineer.Clear_Dual_Mode_Cal_Gamma_Diff();
            //Initialize
            Optic_Compensation_Stop = false;

            //CA channel Selection
            f1.objMemory.ChannelNO = Convert.ToInt32(f1.textBox_ch.Text);
            f1.CA_Channel_trackBar.Value = Convert.ToInt32(f1.textBox_ch.Text);

            //Timer Start
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
            double Skip_Lv = Convert.ToDouble(textBox_Subcompensation_GB_skip_Lv.Text);
            RGB Prev_Band_Gray255_Gamma = new RGB();
            Optic_Compensation_Succeed = false;
            int total_average_loop_count = 0;


            //ProgressBar-related
            int step = 1;
            f1.Set_GB_ProgressBar_Value(0); //Progressbar Value's initializing
            bool Any_Band_is_Selected = ProgressBar_Max_Step_Setting(step * 2); //Set Progressbar's Step and Max-Value

            if (checkBox_Send_Manual_Code.Checked)
            {
                f1.PNC_Manual_Button_Click();
                Thread.Sleep(3000); //Manual Code 안정화 Time
            }

            if (checkBox_Read_DBV_Values.Checked) button_Read_DP116_DBV_Setting.PerformClick();

            if (checkBox_VREF2_AM0_Compensation.Checked)
            {
                double VREF2_Margin = Convert.ToDouble(textBox_VREF2_Margin.Text);
                double Black_Margin = Convert.ToDouble(textBox_AM0_Margin.Text);
                double VREF_Resolution = Convert.ToDouble(textBox_VREF_Resolution.Text);
                double VREF2_Limit_Lv = Convert.ToDouble(textBox_VREF2_Limit_Lv.Text);
                VREF2_AM0_Compensation(VREF2_Margin, Black_Margin, VREF_Resolution, VREF2_Limit_Lv);
            }

            // IRC & ELVSS Compensation
            if (checkBox_IRC_Comp.Checked || checkBox_ELVSS_Comp.Checked)
            {
                //IRC Off
                this.button_IRC_Off.PerformClick();
                Thread.Sleep(100);

                Dual_Mode_HBM_Mode_Gray255_Compensation(true); // Condition 1 에서 진행
                if (checkBox_IRC_Comp.Checked) IRC_Compensation();
                if (checkBox_ELVSS_Comp.Checked) ELVSS_Compensation();
                //IRC On
                this.button_IRC_On.PerformClick();
            }

            bool Condition = true; //Condition 1

            int Initial_Vreg1; //Add on 190603
            int Diff_Vreg1; //Add on 190603

            if (Any_Band_is_Selected)
            {

                f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, false);
                f1.IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0, Disable All Registers
                for (band = 0; band < 12; band++)
                {
                    f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                    if (Optic_Compensation_Stop) break;
                    Gamma_Out_Of_Register_Limit = false;
                    //Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
                    if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                    {
                        //form_dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(band, Condition);
                        if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            f1.AOD_On();
                            DP116_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                        }

                        Vreg1_loop_count = 0; //Vreg1 loop countR
                        Vreg1_Infinite_Count = 0;

                        form_dual_engineer.Band_Radiobuttion_Select(band, true);//Select Band (Condition1) Add on 190605
                        form_dual_engineer.Band_Radiobuttion_Select(band, false);//Select Band (Condition2) Add on 190605
                        Thread.Sleep(300);

                        DP116_DBV_Setting(band);  //DBV Setting

                        for (gray = 0; gray < 10; gray++)
                        {
                            DP116_Pattern_Setting(gray, band, DP116_Or_DP150_116_Is_True);//Pattern Setting
                            ///////////////////////////Condition 1/////////////////////////////
                            bool Condition_1_Skip = false;
                            bool Condition_2_Skip = false;

                            //form_dual_engineer.Band_Radiobuttion_Select(band, Condition);//Select Band (Delete on 190605)

                            DP116_DBV_Setting(band);  //DBV Setting
                            Condition = true; //Condition 1
                            if (form_dual_engineer.radioButton_AOD_C2_Apply.Checked && band >= 9)
                            {
                                Condition_1_Skip = true;
                            }
                            else
                            {
                                form_dual_engineer.Dual_Script_Apply(Condition);//Condition 1
                            }

                            if (Condition_1_Skip == false)
                            {
                                Condition = true; //Condition 1
                                Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet

                                if (gray == 0)
                                {
                                    Vreg1 = Vreg1_Read2(band, Condition, true); //Read Vreg1 Value
                                    //Update_and_Send_Vreg1(Vreg1, band, Condition);
                                    //Thread.Sleep(50);
                                    Update_Vreg1_TextBox(Vreg1, band, true);
                                    Initial_Vreg1 = Vreg1; //Add on 190603
                                    Diff_Vreg1 = Vreg1 - Initial_Vreg1; //Add on 190603
                                }



                                if (Optic_Compensation_Stop) break;
                                
                                Dual_Mode_Get_Param(gray, Condition); //Get (First)Gamma,Target,Limit From OC-Param-Table     
                                f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);


                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                                {
                                    form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                    Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                }
                                RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                Thread.Sleep(300); //Pattern 안정화 Time
                                //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                                DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
                                    , Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                loop_count = 0;
                                Infinite_Count = 0;
                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); //Add on 190614
                                Application.DoEvents();

                                Optic_Compensation_Succeed = false;
                                Within_Spec_Limit = false;

                                while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                                {
                                    if (Target.double_Lv < Skip_Lv)
                                    {
                                        if (band >= 1)
                                        {
                                            form_dual_engineer.Get_Gamma_Only_DP116(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }
                                        RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                        Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                        Measure.Set_Value(0, 0, 0);
                                        Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                            + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                        Optic_Compensation_Succeed = true;
                                        break;
                                    }

                                    //Vreg1 + Sub-Compensation (Change Gamma Value)
                                    if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                        || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                    {
                                        if (Vreg1_loop_count == 0)
                                        {
                                            form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }

                                        Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                        if (Vreg1_loop_count < loop_count_max)
                                        {
                                            f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                            Prev_Vreg1 = Vreg1;
                                            Prev_Gamma.Equal_Value(Gamma);

                                            Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                            Diff_Vreg1 = Vreg1 - Prev_Vreg1;
                                            Diff_Gamma = Gamma - Prev_Gamma;
                                            f1.GB_Status_AppendText_Nextline("B" + band.ToString() + " Red/Vreg1/Blue = " + Gamma.int_R.ToString() + "," + Vreg1.ToString() + "," + Gamma.int_B.ToString(), Color.Black);
                                            f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);


                                            if (Math.Abs(Diff_Vreg1) >= 1)
                                                Vreg1_Need_To_Be_Updated = true;
                                            else
                                                Vreg1_Need_To_Be_Updated = false;

                                            if (Vreg1_Need_To_Be_Updated)
                                            {
                                                //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                                Update_and_Send_Vreg1(Vreg1, band, Condition);
                                                Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                                Update_Vreg1_TextBox(Vreg1, band, true);
                                            }
                                        }
                                        Vreg1_loop_count++;
                                        loop_count++;

                                        if (Vreg1_Infinite_Count >= 3)
                                        {
                                            Extension_Applied = "O";
                                            f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                        }
                                        else
                                            Extension_Applied = "X";
                                    }
                                    else
                                    {
                                        Vreg1_Need_To_Be_Updated = false;//Add on 190603

                                        Prev_Gamma.Equal_Value(Gamma);
                                        Infinite_Loop_Check(loop_count);

                                        Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                        //Engineering Mode
                                        Diff_Gamma = Gamma - Prev_Gamma;
                                        f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                                        loop_count++;

                                        if (Infinite_Count >= 3)
                                        {
                                            Extension_Applied = "O";
                                            f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                        }
                                        else
                                            Extension_Applied = "X";
                                    }


                                    if (Vreg1_Need_To_Be_Updated == false)
                                    {
                                        if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                                        else RGB_Need_To_Change[0] = false;
                                        if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                                        else RGB_Need_To_Change[1] = false;
                                        if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                                        else RGB_Need_To_Change[2] = false;
                                        Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                        Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                        //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure
                                    }


                                    if (Within_Spec_Limit)
                                    {
                                        Optic_Compensation_Succeed = true;
                                        textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                        break;
                                    }

                                    if (Gamma_Out_Of_Register_Limit)
                                    {
                                        Optic_Compensation_Stop = true;
                                        System.Windows.Forms.MessageBox.Show("Gamma/Vreg1 is out of Limit");
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

                                    DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                    Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) form_dual_engineer.Dual_Copy_C1Measure_To_C2Target(band, gray);
                                    f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                    Application.DoEvents();

                                }
                                f1.GB_ProgressBar_PerformStep();
                                ///////////////////////////Condition 1 Over
                            }
                            ///////////////////////////Condition 2 Start
                            //form_dual_engineer.Band_Radiobuttion_Select(band, Condition);//Select Band (Delete on 190605)
                            //DP116_DBV_Setting(band);  //DBV Setting
                            Condition = false; //Condition 2
                            if (form_dual_engineer.radioButton_AOD_C1_Apply.Checked && band >= 9)
                            {
                                Condition_2_Skip = true;
                            }
                            else
                            {
                                form_dual_engineer.Dual_Script_Apply(Condition);//Condition 2
                            }

                            if (Condition_2_Skip == false)
                            {
                                Condition = false; //Condition 2
                                //form_dual_engineer.Dual_All_Gamma_Copy(); //Gamma Copy form Condition 1 to 2
                                //form_dual_engineer.Band_Gray_Gamma_Copy_L_to_R(band, gray);

                                form_dual_engineer.Dual_Mode_Gamma_Offset_Apply(band, gray);
                                form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();

                                form_dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(band, Condition);
                                Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet


                                if (gray == 0)
                                {
                                    Vreg1 = Vreg1_Read2(band, true, true); //Read Vreg1 Value (Gamma_Set 2's Vreg1 Value = Gamma_Set 1's Vreg1 Value);
                                    Update_and_Send_Vreg1(Vreg1, band, Condition); //Add on 190702 (Condition 1 꺼를 읽은거기 때문에 먼저 Condition2 꺼에 초기 Vreg1 세팅 필요)
                                    Thread.Sleep(20);
                                    Update_Vreg1_TextBox(Vreg1, band, false);
                                    Initial_Vreg1 = Vreg1; //Add on 190603
                                    Diff_Vreg1 = Vreg1 - Initial_Vreg1; //Add on 190603
                                }

                                if (Optic_Compensation_Stop) break;
                                Dual_Mode_Get_Param(gray, Condition); //Get (First)Gamma,Target,Limit From OC-Param-Table                            
                                f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);


                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                   || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                                {
                                    form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                    Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                }
                                RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                                //Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); Delete on 190614
                                Thread.Sleep(100);
                                //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                                DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
                                    , Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                loop_count = 0;
                                Infinite_Count = 0;
                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); //Add on 190614
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();

                                Optic_Compensation_Succeed = false;
                                Within_Spec_Limit = false;

                                while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                                {
                                    if (Target.double_Lv < Skip_Lv)
                                    {
                                        if (band >= 1)
                                        {
                                            form_dual_engineer.Get_Gamma_Only_DP116(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }
                                        RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                        Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                        Measure.Set_Value(0, 0, 0);
                                        Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                            + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                        Optic_Compensation_Succeed = true;
                                        break;
                                    }

                                    //Vreg1 + Sub-Compensation (Change Gamma Value)
                                    if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                        || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                    {
                                        if (Vreg1_loop_count == 0)
                                        {
                                            form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }

                                        Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                        if (Vreg1_loop_count < loop_count_max)
                                        {
                                            f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                            Prev_Vreg1 = Vreg1;
                                            Prev_Gamma.Equal_Value(Gamma);

                                            Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                            Diff_Vreg1 = Vreg1 - Prev_Vreg1;
                                            Diff_Gamma = Gamma - Prev_Gamma;
                                            f1.GB_Status_AppendText_Nextline("B" + band.ToString() + " Red/Vreg1/Blue = " + Gamma.int_R.ToString() + "," + Vreg1.ToString() + "," + Gamma.int_B.ToString(), Color.Black);
                                            f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);

                                            if (Math.Abs(Diff_Vreg1) >= 1)
                                                Vreg1_Need_To_Be_Updated = true;
                                            else
                                                Vreg1_Need_To_Be_Updated = false;

                                            if (Vreg1_Need_To_Be_Updated)
                                            {
                                                //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                                Update_and_Send_Vreg1(Vreg1, band, Condition);
                                                Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                                Update_Vreg1_TextBox(Vreg1, band, false);
                                            }



                                        }
                                        Vreg1_loop_count++;
                                        loop_count++;

                                        if (Vreg1_Infinite_Count >= 3)
                                        {
                                            Extension_Applied = "O";
                                            f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                        }
                                        else
                                            Extension_Applied = "X";
                                    }
                                    else
                                    {
                                        Vreg1_Need_To_Be_Updated = false;

                                        Prev_Gamma.Equal_Value(Gamma);
                                        Infinite_Loop_Check(loop_count);

                                        Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);


                                        //Engineering Mode
                                        Diff_Gamma = Gamma - Prev_Gamma;
                                        f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                                        loop_count++;

                                        if (Infinite_Count >= 3)
                                        {
                                            Extension_Applied = "O";
                                            f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                        }
                                        else
                                            Extension_Applied = "X";
                                    }
                                    if (Vreg1_Need_To_Be_Updated == false)
                                    {
                                        if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                                        else RGB_Need_To_Change[0] = false;
                                        if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                                        else RGB_Need_To_Change[1] = false;
                                        if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                                        else RGB_Need_To_Change[2] = false;
                                        Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                        form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                                        Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                    }


                                    if (Within_Spec_Limit)
                                    {
                                        Optic_Compensation_Succeed = true;
                                        textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                        break;
                                    }

                                    if (Gamma_Out_Of_Register_Limit)
                                    {
                                        Optic_Compensation_Stop = true;
                                        System.Windows.Forms.MessageBox.Show("Gamma/Vreg1 is out of Limit");
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
                                    //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure
                                    DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                    Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                    Application.DoEvents();
                                }
                                f1.GB_ProgressBar_PerformStep();
                            }
                        }
                        if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            f1.AOD_Off();
                        }
                    }
                    // Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                }
                form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                form_dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(false);
                if (checkBox_1st_Mode_OTP_AutoWirte.Checked && Optic_Compensation_Stop == false && Gamma_Out_Of_Register_Limit == false)
                {
                    f1.CRC_Check();
                    f1.First_Model_OTP_Write_Button_Click();
                    f1.ADD_GB_ProgressBar_Value(5 * step);

                }
            }
            f1.OC_Timer_Stop();
            if (Optic_Compensation_Succeed) System.Windows.Forms.MessageBox.Show("Optic Compensation Finish !");
        }


        private void Dual_Mode_2_Optic_compensation()
        {
            bool Vreg1_Need_To_Be_Updated = false;//Add on 190603

            //datagridview-related
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //f1.Dual_Engineering_Mode_Show(); //Delete on 190605
            Dual_Engineer_Monitoring_Mode form_dual_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            form_dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(true);
            form_dual_engineer.Dual_Mode_GridView_Measure_Extension_LoopCound_Area_Data_Clear();
            form_dual_engineer.Clear_Dual_Mode_Cal_Gamma_Diff();
            //Initialize
            Optic_Compensation_Stop = false;

            //CA channel Selection
            f1.objMemory.ChannelNO = Convert.ToInt32(f1.textBox_ch.Text);
            f1.CA_Channel_trackBar.Value = Convert.ToInt32(f1.textBox_ch.Text);

            //Timer Start
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
            double Skip_Lv = Convert.ToDouble(textBox_Subcompensation_GB_skip_Lv.Text);
            RGB Prev_Band_Gray255_Gamma = new RGB();
            Optic_Compensation_Succeed = false;
            int total_average_loop_count = 0;

            //ProgressBar-related
            int step = 1;
            f1.Set_GB_ProgressBar_Value(0); //Progressbar Value's initializing
            bool Any_Band_is_Selected = ProgressBar_Max_Step_Setting(step * 2); //Set Progressbar's Step and Max-Value

            if (checkBox_Send_Manual_Code.Checked)
            {
                f1.PNC_Manual_Button_Click();
                Thread.Sleep(3000); //Manual Code 안정화 Time
            }

            if (checkBox_Read_DBV_Values.Checked) button_Read_DP116_DBV_Setting.PerformClick();

            if (checkBox_VREF2_AM0_Compensation.Checked)
            {
                double VREF2_Margin = Convert.ToDouble(textBox_VREF2_Margin.Text);
                double Black_Margin = Convert.ToDouble(textBox_AM0_Margin.Text);
                double VREF_Resolution = Convert.ToDouble(textBox_VREF_Resolution.Text);
                double VREF2_Limit_Lv = Convert.ToDouble(textBox_VREF2_Limit_Lv.Text);
                VREF2_AM0_Compensation(VREF2_Margin, Black_Margin, VREF_Resolution, VREF2_Limit_Lv);
            }

            // IRC & ELVSS Compensation
            if (checkBox_IRC_Comp.Checked || checkBox_ELVSS_Comp.Checked)
            {
                //IRC Off
                this.button_IRC_Off.PerformClick();
                Thread.Sleep(100);

                Dual_Mode_HBM_Mode_Gray255_Compensation(true); // Condition 1 에서 진행
                if (checkBox_IRC_Comp.Checked) IRC_Compensation();
                if (checkBox_ELVSS_Comp.Checked) ELVSS_Compensation();
                //IRC On
                this.button_IRC_On.PerformClick();
            }

            bool Condition = true; //Condition 1

            if (Any_Band_is_Selected)
            {

                f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, false);
                f1.IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0, Disable All Registers
                for (band = 0; band < 12; band++)
                {
                    f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                    ///////////////////////////Condition 1/////////////////////////////      
                    Condition = true; //Condition 1
                    //form_dual_engineer.Dual_Script_Apply(Condition);//Condition 1 (Delete on 190702)
                    Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet

                    if (Optic_Compensation_Stop) break;
                    Gamma_Out_Of_Register_Limit = false;
                    //Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
                    if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                    {
                        if (band < 9) //Add on 190702
                        {
                            form_dual_engineer.Dual_Script_Apply(Condition);//Condition 1
                        }
                        //form_dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(band, Condition);
                        else if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            if (form_dual_engineer.radioButton_AOD_C1_Apply.Checked)
                            {
                                form_dual_engineer.Dual_Script_Apply(true);
                            }
                            else if (form_dual_engineer.radioButton_AOD_C2_Apply.Checked)
                            {
                                //form_dual_engineer.Dual_Script_Apply(false);
                            }
                            else if (form_dual_engineer.radioButton_AOD_Both_Apply.Checked)
                            {
                                form_dual_engineer.Dual_Script_Apply(Condition);
                            }
                            else
                            {
                                //Do nothing, it will not take place
                            }
                            f1.AOD_On();
                            DP116_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                        }

                        if (form_dual_engineer.radioButton_AOD_C2_Apply.Checked && band >= 9) //AOD and C2 only checked
                        {
                            //Do nothing
                        }
                        else
                        {
                            form_dual_engineer.Band_Radiobuttion_Select(band, Condition);//Select Band
                            Thread.Sleep(300);

                            DP116_DBV_Setting(band);  //DBV Setting
                            Vreg1_loop_count = 0; //Vreg1 loop countR
                            Vreg1_Infinite_Count = 0;


                            Vreg1 = Vreg1_Read2(band, Condition, true); //Read Vreg1 Value
                            Update_Vreg1_TextBox(Vreg1, band, true);
                            int Initial_Vreg1 = Vreg1; //Add on 190603
                            int Diff_Vreg1 = Vreg1 - Initial_Vreg1;//Add on 190603

                            for (gray = 0; gray < 10; gray++)
                            {
                                if (Optic_Compensation_Stop) break;
                                DP116_Pattern_Setting(gray, band, DP116_Or_DP150_116_Is_True);//Pattern Setting
                                Dual_Mode_Get_Param(gray, Condition); //Get (First)Gamma,Target,Limit From OC-Param-Table                            
                                f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                   || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                                {
                                    form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                    Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                }

                                RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values

                                Thread.Sleep(300); //Pattern 안정화 Time
                                //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                                DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
                                    , Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                loop_count = 0;
                                Infinite_Count = 0;
                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); //Add on 190614
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();

                                Optic_Compensation_Succeed = false;
                                Within_Spec_Limit = false;

                                while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                                {
                                    if (Target.double_Lv < Skip_Lv)
                                    {
                                        if (band >= 1)
                                        {
                                            form_dual_engineer.Get_Gamma_Only_DP116(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }

                                        RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                        Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                        Measure.Set_Value(0, 0, 0);
                                        Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                            + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                        Optic_Compensation_Succeed = true;
                                        break;
                                    }

                                    //Vreg1 + Sub-Compensation (Change Gamma Value)
                                    if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                        || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                    {
                                        if (Vreg1_loop_count == 0)
                                        {
                                            form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }

                                        Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                        if (Vreg1_loop_count < loop_count_max)
                                        {
                                            f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                            Prev_Vreg1 = Vreg1;
                                            Prev_Gamma.Equal_Value(Gamma);

                                            Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                            Diff_Vreg1 = Vreg1 - Prev_Vreg1;
                                            Diff_Gamma = Gamma - Prev_Gamma;
                                            f1.GB_Status_AppendText_Nextline("B" + band.ToString() + " Red/Vreg1/Blue = " + Gamma.int_R.ToString() + "," + Vreg1.ToString() + "," + Gamma.int_B.ToString(), Color.Black);
                                            f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);


                                            if (Math.Abs(Diff_Vreg1) >= 1)
                                                Vreg1_Need_To_Be_Updated = true;
                                            else
                                                Vreg1_Need_To_Be_Updated = false;

                                            if (Vreg1_Need_To_Be_Updated)
                                            {
                                                //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                                Update_and_Send_Vreg1(Vreg1, band, Condition);
                                                Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                                Update_Vreg1_TextBox(Vreg1, band, true);
                                            }


                                        }
                                        Vreg1_loop_count++;
                                        loop_count++;

                                        if (Vreg1_Infinite_Count >= 3)
                                        {
                                            Extension_Applied = "O";
                                            f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                        }
                                        else
                                            Extension_Applied = "X";
                                    }
                                    else
                                    {
                                        Vreg1_Need_To_Be_Updated = false;//Add on 190603

                                        Prev_Gamma.Equal_Value(Gamma);
                                        Infinite_Loop_Check(loop_count);

                                        Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);


                                        //Engineering Mode
                                        Diff_Gamma = Gamma - Prev_Gamma;
                                        f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                                        loop_count++;

                                        if (Infinite_Count >= 3)
                                        {
                                            Extension_Applied = "O";
                                            f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                        }
                                        else
                                            Extension_Applied = "X";
                                    }


                                    if (Vreg1_Need_To_Be_Updated == false)
                                    {
                                        if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                                        else RGB_Need_To_Change[0] = false;
                                        if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                                        else RGB_Need_To_Change[1] = false;
                                        if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                                        else RGB_Need_To_Change[2] = false;
                                        Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                        Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                    }

                                    if (Within_Spec_Limit)
                                    {
                                        Optic_Compensation_Succeed = true;
                                        textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                        break;
                                    }

                                    if (Gamma_Out_Of_Register_Limit)
                                    {
                                        Optic_Compensation_Stop = true;
                                        System.Windows.Forms.MessageBox.Show("Gamma/Vreg1 is out of Limit");
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

                                    //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure
                                    DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                    Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) form_dual_engineer.Dual_Copy_C1Measure_To_C2Target(band, gray);
                                    f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                    Application.DoEvents();
                                }
                                f1.GB_ProgressBar_PerformStep();
                            }
                        }
                        if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            f1.AOD_Off();
                        }

                    }

                    ///////////////////////////Condition 1 Over
                    ///////////////////////////Condition 2 Start
                    Condition = false; //Condition 2
                    //form_dual_engineer.Dual_Script_Apply(Condition);//Condition 2 (Delete on 190702)
                    //form_dual_engineer.Dual_All_Gamma_Copy(); //Gamma Copy form Condition 1 to 2
                    //form_dual_engineer.Dual_Band_Gamma_Copy(band);
                    
            
                    //VREF2_and_AM0_Read();
                    if (Optic_Compensation_Stop) break;
                    Gamma_Out_Of_Register_Limit = false;
                    //Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
                    if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                    {
                        form_dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(band, Condition);
                        if (band < 9) //Add on 190702
                        {
                            form_dual_engineer.Dual_Script_Apply(Condition);//Condition 2
                        }
                        else if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            if (form_dual_engineer.radioButton_AOD_C1_Apply.Checked)
                            {
                                //form_dual_engineer.Dual_Script_Apply(true);
                            }
                            else if (form_dual_engineer.radioButton_AOD_C2_Apply.Checked)
                            {
                                form_dual_engineer.Dual_Script_Apply(false);
                            }
                            else if (form_dual_engineer.radioButton_AOD_Both_Apply.Checked)
                            {
                                form_dual_engineer.Dual_Script_Apply(Condition);
                            }
                            else
                            {
                                //Do nothing, it will not take place
                            }
                            f1.AOD_On();
                            DP116_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                        }

                        if (form_dual_engineer.radioButton_AOD_C1_Apply.Checked && band >= 9) //AOD and C2 only checked
                        {
                            //Do nothing
                        }
                        else
                        {
                            if ((band < 9 && form_dual_engineer.radioButton_AOD_C1_Apply.Checked) || form_dual_engineer.radioButton_AOD_Both_Apply.Checked || form_dual_engineer.radioButton_AOD_C2_Apply.Checked)
                            {
                                Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                                form_dual_engineer.Dual_Band_Add_Offset_Gamma(band);
                                form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                            }
                            form_dual_engineer.Band_Radiobuttion_Select(band, Condition);//Select Band
                            Thread.Sleep(300);
                            //DP116_DBV_Setting(band);  //DBV Setting
                            Vreg1_loop_count = 0; //Vreg1 loop countR
                            Vreg1_Infinite_Count = 0;

                            Vreg1 = Vreg1_Read2(band, true, true); //Read Vreg1 Value (Gamma_Set 2's Vreg1 Value = Gamma_Set 1's Vreg1 Value);
                            Update_Vreg1_TextBox(Vreg1, band, false);
                            int Initial_Vreg1 = Vreg1; //Add on 190603
                            int Diff_Vreg1 = Vreg1 - Initial_Vreg1; //Add on 190603

                            if (checkBox_Dual_Mode_1st_Gamma_Vreg1_Alppy.Checked)
                            {
                                if ((band < 9 && form_dual_engineer.radioButton_AOD_C1_Apply.Checked) || form_dual_engineer.radioButton_AOD_Both_Apply.Checked || (band < 9 &&form_dual_engineer.radioButton_AOD_C2_Apply.Checked))
                                {
                                    Dual_Mode_1st_Band_Gamma_Vreg1_Apply(band, Vreg1, Condition);
                                }
                            }

                            for (gray = 0; gray < 10; gray++)
                            {
                                if (Optic_Compensation_Stop) break;
                                DP116_Pattern_Setting(gray, band, DP116_Or_DP150_116_Is_True);//Pattern Setting
                                Dual_Mode_Get_Param(gray, Condition); //Get (First)Gamma,Target,Limit From OC-Param-Table                            
                                f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                   || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                                {
                                    form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                    Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                }
                                RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                                Thread.Sleep(300); //Pattern 안정화 Time
                                //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                                DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
                                    , Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                loop_count = 0;
                                Infinite_Count = 0;
                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); //Add on 190614
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();

                                Optic_Compensation_Succeed = false;
                                Within_Spec_Limit = false;

                                while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                                {
                                    if (Target.double_Lv < Skip_Lv)
                                    {
                                        if (band >= 1)
                                        {
                                            form_dual_engineer.Get_Gamma_Only_DP116(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }
                                        RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                        Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                        Measure.Set_Value(0, 0, 0);
                                        Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                            + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                        Optic_Compensation_Succeed = true;
                                        break;
                                    }

                                    //Vreg1 + Sub-Compensation (Change Gamma Value)
                                    if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                        || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                    {
                                        if (Vreg1_loop_count == 0)
                                        {
                                            form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }

                                        Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                        if (Vreg1_loop_count < loop_count_max)
                                        {
                                            f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                            Prev_Vreg1 = Vreg1;
                                            Prev_Gamma.Equal_Value(Gamma);

                                            Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                            Diff_Vreg1 = Vreg1 - Prev_Vreg1;
                                            Diff_Gamma = Gamma - Prev_Gamma;
                                            f1.GB_Status_AppendText_Nextline("B" + band.ToString() + " Red/Vreg1/Blue = " + Gamma.int_R.ToString() + "," + Vreg1.ToString() + "," + Gamma.int_B.ToString(), Color.Black);
                                            f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);

                                            if (Math.Abs(Diff_Vreg1) >= 1)
                                                Vreg1_Need_To_Be_Updated = true;
                                            else
                                                Vreg1_Need_To_Be_Updated = false;

                                            if (Vreg1_Need_To_Be_Updated)
                                            {
                                                //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                                Update_and_Send_Vreg1(Vreg1, band, Condition);
                                                Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                                Update_Vreg1_TextBox(Vreg1, band, false);
                                            }
                                        }
                                        Vreg1_loop_count++;
                                        loop_count++;

                                        if (Vreg1_Infinite_Count >= 3)
                                        {
                                            Extension_Applied = "O";
                                            f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                        }
                                        else
                                            Extension_Applied = "X";
                                    }
                                    else
                                    {
                                        Vreg1_Need_To_Be_Updated = false;

                                        Prev_Gamma.Equal_Value(Gamma);
                                        Infinite_Loop_Check(loop_count);

                                        Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);


                                        //Engineering Mode
                                        Diff_Gamma = Gamma - Prev_Gamma;
                                        f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                                        loop_count++;

                                        if (Infinite_Count >= 3)
                                        {
                                            Extension_Applied = "O";
                                            f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                        }
                                        else
                                            Extension_Applied = "X";
                                    }
                                    if (Vreg1_Need_To_Be_Updated == false)
                                    {
                                        if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                                        else RGB_Need_To_Change[0] = false;
                                        if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                                        else RGB_Need_To_Change[1] = false;
                                        if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                                        else RGB_Need_To_Change[2] = false;
                                        Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                        form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                                        Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                    }


                                    if (Within_Spec_Limit)
                                    {
                                        Optic_Compensation_Succeed = true;
                                        textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                        break;
                                    }

                                    if (Gamma_Out_Of_Register_Limit)
                                    {
                                        Optic_Compensation_Stop = true;
                                        System.Windows.Forms.MessageBox.Show("Gamma/Vreg1 is out of Limit");
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


                                    //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure
                                    DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                    Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                    Application.DoEvents();

                                }
                                f1.GB_ProgressBar_PerformStep();
                            }
                        }
                        if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            f1.AOD_Off();
                        }
                        ///////////////////////////////////////////////// Condition 2 Over
                    }
                   // Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                }
                form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                form_dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(false);
                if (checkBox_1st_Mode_OTP_AutoWirte.Checked && Optic_Compensation_Stop == false && Gamma_Out_Of_Register_Limit == false)
                {
                    f1.CRC_Check();
                    f1.First_Model_OTP_Write_Button_Click();
                    f1.ADD_GB_ProgressBar_Value(5 * step);
                }
            }
            f1.OC_Timer_Stop();

            if (Optic_Compensation_Succeed) System.Windows.Forms.MessageBox.Show("Optic Compensation Finish !");
        }


        private void Dual_Mode_1st_Band_Gamma_Vreg1_Apply(int band,int Vreg1,bool Condition)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Dual Mode 1st Band" + (band).ToString() + " Gamma Vreg1 Apply Started(Condition : " + Condition.ToString() + ")", Color.Blue);
            f1.IPC_Quick_Send("image.gradation.gray hh dec");
            Application.DoEvents();
    
            Update_and_Send_Vreg1(Vreg1, band, Condition);//Apply Vreg1

            for (int gray = 0; gray < 10; gray++)
            {
                Dual_Mode_Get_Param(gray, Condition); //Get Gamma
                f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);
                Thread.Sleep(10);
                Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Apply Gamma
                Thread.Sleep(20);
            }

            f1.GB_Status_AppendText_Nextline("Dual Mode 1stGamma Vreg1 Apply End", Color.Blue);
        }



        private void Dual_Mode_1_Optic_compensation()
        {
            bool Vreg1_Need_To_Be_Updated = false;

            //datagridview-related
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //f1.Dual_Engineering_Mode_Show(); //Delete on 190605
            Dual_Engineer_Monitoring_Mode form_dual_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            form_dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(true);
            form_dual_engineer.Dual_Mode_GridView_Measure_Extension_LoopCound_Area_Data_Clear();
            form_dual_engineer.Clear_Dual_Mode_Cal_Gamma_Diff();

            //Initialize
            Optic_Compensation_Stop = false;

            //CA channel Selection
            f1.objMemory.ChannelNO = Convert.ToInt32(f1.textBox_ch.Text);
            f1.CA_Channel_trackBar.Value = Convert.ToInt32(f1.textBox_ch.Text);

            //Timer Start
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
            double Skip_Lv = Convert.ToDouble(textBox_Subcompensation_GB_skip_Lv.Text);
            RGB Prev_Band_Gray255_Gamma = new RGB();
            Optic_Compensation_Succeed = false;
            int total_average_loop_count = 0;


            //ProgressBar-related
            int step = 1;
            f1.Set_GB_ProgressBar_Value(0); //Progressbar Value's initializing
            bool Any_Band_is_Selected = ProgressBar_Max_Step_Setting(step * 2); //Set Progressbar's Step and Max-Value

            if (checkBox_Send_Manual_Code.Checked)
            {
                f1.PNC_Manual_Button_Click();
                Thread.Sleep(3000); //Manual Code 안정화 Time
            }

            if (checkBox_Read_DBV_Values.Checked) button_Read_DP116_DBV_Setting.PerformClick();

            if (checkBox_VREF2_AM0_Compensation.Checked)
            {
                double VREF2_Margin = Convert.ToDouble(textBox_VREF2_Margin.Text);
                double Black_Margin = Convert.ToDouble(textBox_AM0_Margin.Text);
                double VREF_Resolution = Convert.ToDouble(textBox_VREF_Resolution.Text);
                double VREF2_Limit_Lv = Convert.ToDouble(textBox_VREF2_Limit_Lv.Text);
                VREF2_AM0_Compensation(VREF2_Margin, Black_Margin, VREF_Resolution, VREF2_Limit_Lv);
            }

            // IRC & ELVSS Compensation
            if (checkBox_IRC_Comp.Checked || checkBox_ELVSS_Comp.Checked)
            {
                //IRC Off
                this.button_IRC_Off.PerformClick();
                Thread.Sleep(100);

                Dual_Mode_HBM_Mode_Gray255_Compensation(true); // Condition 1 에서 진행
                if (checkBox_IRC_Comp.Checked) IRC_Compensation();
                if (checkBox_ELVSS_Comp.Checked) ELVSS_Compensation();
                //IRC On
                this.button_IRC_On.PerformClick();
            }



            if (Any_Band_is_Selected)
            {
                ///////////////////////////Condition 1/////////////////////////////              
                bool Condition = true; //Condition 1

                //form_dual_engineer.Dual_Script_Apply(Condition);//Condition 1 (Delete on 190702)
                f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, false);
                f1.IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0, Disable All Registers

                Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                for (band = 0; band < 12; band++)
                {
                    f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                    if (Optic_Compensation_Stop) break;
                    Gamma_Out_Of_Register_Limit = false;
                    //Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
                    if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                    {
                        if (band < 9) //Add on 190702
                        {
                            form_dual_engineer.Dual_Script_Apply(Condition);//Condition 1
                        }
                        //form_dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(band, Condition);
                        else if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            if (form_dual_engineer.radioButton_AOD_C1_Apply.Checked)
                            {
                                form_dual_engineer.Dual_Script_Apply(true);
                            }
                            else if (form_dual_engineer.radioButton_AOD_C2_Apply.Checked)
                            {
                                break;
                            }
                            else if (form_dual_engineer.radioButton_AOD_Both_Apply.Checked)
                            {
                                form_dual_engineer.Dual_Script_Apply(Condition);
                            }
                            else
                            {
                                //Do nothing, it will not take place
                            }
                            f1.AOD_On();
                            DP116_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                        }

                        form_dual_engineer.Band_Radiobuttion_Select(band, Condition);//Select Band
                        Thread.Sleep(300);

                        DP116_DBV_Setting(band);  //DBV Setting
                        Vreg1_loop_count = 0; //Vreg1 loop countR
                        Vreg1_Infinite_Count = 0;

                        Vreg1 = Vreg1_Read2(band, Condition, true); //Read Vreg1 Value
                        Update_Vreg1_TextBox(Vreg1, band, true);
                        int Initial_Vreg1 = Vreg1; //Add on 190603
                        int Diff_Vreg1 = Vreg1 - Initial_Vreg1; //Add on 190603

                        for (gray = 0; gray < 10; gray++)
                        {
                            if (Optic_Compensation_Stop) break;
                            Dual_Mode_Get_Param(gray, Condition); //Get (First)Gamma,Target,Limit From OC-Param-Table 
                            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                            DP116_Pattern_Setting(gray, band, DP116_Or_DP150_116_Is_True);//Pattern Setting

                            if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                            {
                                form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B,Condition);
                                Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                            }

                            RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                            Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                            Thread.Sleep(300); //Pattern 안정화 Time
                            //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                            DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
                                ,Target.double_X,Target.double_Y,Target.double_Lv,Limit.double_X,Limit.double_Y,Limit.double_Lv,Extension.double_X,Extension.double_Y);
                            loop_count = 0;
                            Infinite_Count = 0;
                            Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition);//Add on 190614
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                            Application.DoEvents();

                            Optic_Compensation_Succeed = false;
                            Within_Spec_Limit = false;

                            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                            {
                                if (Target.double_Lv < Skip_Lv)
                                {
                                    if (band >= 1)
                                    {
                                        form_dual_engineer.Get_Gamma_Only_DP116(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                        Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                    }
                                    RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                    Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                    Measure.Set_Value(0, 0, 0);
                                    Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                        + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                    Optic_Compensation_Succeed = true;
                                    break;
                                }

                                //Vreg1 + Sub-Compensation (Change Gamma Value)
                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                {
                                    if (Vreg1_loop_count == 0)
                                    {
                                        form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                        Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                    }

                                    Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                    if (Vreg1_loop_count < loop_count_max)
                                    {
                                        f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                        Prev_Vreg1 = Vreg1;
                                        Prev_Gamma.Equal_Value(Gamma);

                                        Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                        Diff_Vreg1 = Vreg1 - Prev_Vreg1;
                                        Diff_Gamma = Gamma - Prev_Gamma;
                                        f1.GB_Status_AppendText_Nextline("B" + band.ToString() + " Red/Vreg1/Blue = " + Gamma.int_R.ToString() + "," + Vreg1.ToString() + "," + Gamma.int_B.ToString(), Color.Black);
                                        f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);


                                        if (Math.Abs(Diff_Vreg1) >= 1)
                                            Vreg1_Need_To_Be_Updated = true;
                                        else
                                            Vreg1_Need_To_Be_Updated = false;

                                        if (Vreg1_Need_To_Be_Updated)
                                        {
                                            Update_and_Send_Vreg1(Vreg1, band, Condition);
                                            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                            Update_Vreg1_TextBox(Vreg1, band, true);
                                        }     
                                    }
                                    Vreg1_loop_count++;
                                    loop_count++;

                                    if (Vreg1_Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }
                                else
                                {
                                    Vreg1_Need_To_Be_Updated = false;

                                    Prev_Gamma.Equal_Value(Gamma);
                                    Infinite_Loop_Check(loop_count);

                                    Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);


                                    //Engineering Mode
                                    Diff_Gamma = Gamma - Prev_Gamma;
                                    f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                                    loop_count++;

                                    if (Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }

                                if (Vreg1_Need_To_Be_Updated == false)
                                {
                                    if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                                    else RGB_Need_To_Change[0] = false;
                                    if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                                    else RGB_Need_To_Change[1] = false;
                                    if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                                    else RGB_Need_To_Change[2] = false;
                                    Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                    Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                    //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure
                                }

                                if (Within_Spec_Limit)
                                {
                                    Optic_Compensation_Succeed = true;
                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    break;
                                }

                                if (Gamma_Out_Of_Register_Limit)
                                {
                                    Optic_Compensation_Stop = true;
                                    System.Windows.Forms.MessageBox.Show("Gamma/Vreg1 is out of Limit");
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

                                DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) form_dual_engineer.Dual_Copy_C1Measure_To_C2Target(band, gray);
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();
                            }
                            f1.GB_ProgressBar_PerformStep();
                        }
                        if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            f1.AOD_Off();
                        }

                    }
                    //VREF2_and_AM0_Read();
                }
                ///////////////////////////Condition 1 Over
                ///////////////////////////Condition 2 Start
                Condition = false; //Condition 2
                //form_dual_engineer.Dual_Script_Apply(Condition);//Condition 2 (Delete on 190702)
                //form_dual_engineer.Dual_All_Gamma_Copy(); //Gamma Copy form Condition 1 to 2
                form_dual_engineer.Dual_All_Add_Offset_Gamma(); //Add Gamma Offset form Condition 1 to 2
                form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();

                f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, false);
                f1.IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0, Disable All Registers

                Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                for (band = 0; band < 12; band++)
                {
                    if (Optic_Compensation_Stop) break;
                    Gamma_Out_Of_Register_Limit = false;
                    //Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
                    if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                    {
                        form_dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(band, Condition);
                        if (band < 9) //Add on 190702
                        {
                            form_dual_engineer.Dual_Script_Apply(Condition);//Condition 2
                        }
                        else if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            if (form_dual_engineer.radioButton_AOD_C1_Apply.Checked)
                            {
                                break;
                            }
                            else if (form_dual_engineer.radioButton_AOD_C2_Apply.Checked)
                            {
                                form_dual_engineer.Dual_Script_Apply(false);
                            }
                            else if (form_dual_engineer.radioButton_AOD_Both_Apply.Checked)
                            {
                                form_dual_engineer.Dual_Script_Apply(Condition);
                            }
                            else
                            {
                                //Do nothing, it will not take place
                            }
                            f1.AOD_On();
                            DP116_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                        }

                        form_dual_engineer.Band_Radiobuttion_Select(band, Condition);//Select Band
                        Thread.Sleep(300);

                        DP116_DBV_Setting(band);  //DBV Setting
                        Vreg1_loop_count = 0; //Vreg1 loop countR
                        Vreg1_Infinite_Count = 0;


                        Vreg1 = Vreg1_Read2(band, true, true); //Read Vreg1 Value (Gamma_Set 2's Vreg1 Value = Gamma_Set 1's Vreg1 Value);
                        Update_Vreg1_TextBox(Vreg1, band, false);
                        int Initial_Vreg1 = Vreg1; //Add on 190603
                        int Diff_Vreg1 = Vreg1 - Initial_Vreg1; //Add on 190603

                        if (checkBox_Dual_Mode_1st_Gamma_Vreg1_Alppy.Checked)
                            Dual_Mode_1st_Band_Gamma_Vreg1_Apply(band, Vreg1, Condition);
                             
                        for (gray = 0; gray < 10; gray++)
                        {
                            if (Optic_Compensation_Stop) break;
                            Dual_Mode_Get_Param(gray, Condition); //Get (First)Gamma,Target,Limit From OC-Param-Table                            
                            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                            DP116_Pattern_Setting(gray, band, DP116_Or_DP150_116_Is_True);//Pattern Setting
                            if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                   || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                            {
                                form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                            }

                            RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                            Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values   
                            form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();

                            Thread.Sleep(300); //Pattern 안정화 Time
                            //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                            DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
                               ,Target.double_X,Target.double_Y,Target.double_Lv,Limit.double_X,Limit.double_Y,Limit.double_Lv,Extension.double_X,Extension.double_Y);
                            loop_count = 0;
                            Infinite_Count = 0;
                            Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); //190614 Add
                            f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                            Application.DoEvents();

                            Optic_Compensation_Succeed = false;
                            Within_Spec_Limit = false;

                            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                            {
                                if (Target.double_Lv < Skip_Lv)
                                {
                                    if (band >= 1)
                                    {
                                        form_dual_engineer.Get_Gamma_Only_DP116(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                        Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                    }

                                    RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                    Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                    form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();

                                    Measure.Set_Value(0, 0, 0);
                                    Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                        + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                    Optic_Compensation_Succeed = true;
                                    break;
                                }

                                //Vreg1 + Sub-Compensation (Change Gamma Value)
                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                {
                                    if (Vreg1_loop_count == 0)
                                    {
                                        form_dual_engineer.Get_Gamma_Only_DP116(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                        Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                    }

                                    Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                    if (Vreg1_loop_count < loop_count_max)
                                    {
                                        f1.GB_Status_AppendText_Nextline("Vreg1 Loop Count : " + Vreg1_loop_count.ToString(), Color.Blue);
                                        Prev_Vreg1 = Vreg1;
                                        Prev_Gamma.Equal_Value(Gamma);

                                        Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                        Diff_Vreg1 = Vreg1 - Prev_Vreg1;
                                        Diff_Gamma = Gamma - Prev_Gamma;
                                        f1.GB_Status_AppendText_Nextline("B" + band.ToString() + " Red/Vreg1/Blue = " + Gamma.int_R.ToString() + "," + Vreg1.ToString() + "," + Gamma.int_B.ToString(), Color.Black);
                                        f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);

                                        if (Math.Abs(Diff_Vreg1) >= 1)
                                            Vreg1_Need_To_Be_Updated = true;
                                        else
                                            Vreg1_Need_To_Be_Updated = false;

                                        if (Vreg1_Need_To_Be_Updated)
                                        {
                                            //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                            Update_and_Send_Vreg1(Vreg1, band, Condition);
                                            Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                            Update_Vreg1_TextBox(Vreg1, band, false);
                                        }                  
                                    }
                                    Vreg1_loop_count++;
                                    loop_count++;

                                    if (Vreg1_Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }
                                else
                                {
                                    Vreg1_Need_To_Be_Updated = false;

                                    Prev_Gamma.Equal_Value(Gamma);
                                    Infinite_Loop_Check(loop_count);

                                    Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);


                                    //Engineering Mode
                                    Diff_Gamma = Gamma - Prev_Gamma;
                                    f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                                    loop_count++;

                                    if (Infinite_Count >= 3)
                                    {
                                        Extension_Applied = "O";
                                        f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                                    }
                                    else
                                        Extension_Applied = "X";
                                }
                                if (Vreg1_Need_To_Be_Updated == false)
                                {
                                    if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                                    else RGB_Need_To_Change[0] = false;
                                    if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                                    else RGB_Need_To_Change[1] = false;
                                    if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                                    else RGB_Need_To_Change[2] = false;
                                    Update_and_Send_All_Band_Gray_Gamma(Gamma, band, gray, Condition); //Setting Gamma Values
                                    form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                                    Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                    
                                }


                                if (Within_Spec_Limit)
                                {
                                    Optic_Compensation_Succeed = true;
                                    textBox_Bool_Finish_Status.Text = Optic_Compensation_Succeed.ToString();
                                    break;
                                }

                                if (Gamma_Out_Of_Register_Limit)
                                {
                                    Optic_Compensation_Stop = true;
                                    System.Windows.Forms.MessageBox.Show("Gamma/Vreg1 is out of Limit");
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

                                //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);//Measure
                                DP116_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();

                            }
                            f1.GB_ProgressBar_PerformStep();
                        }
                        if (band == 9 || band == 10 || band == 11) //AOD1,2,3
                        {
                            f1.AOD_Off();
                        }
                        ///////////////////////////////////////////////// Condition 2 Over
                    }
                }

                form_dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(false);
                if (checkBox_1st_Mode_OTP_AutoWirte.Checked && Optic_Compensation_Stop == false && Gamma_Out_Of_Register_Limit == false)
                {
                    f1.CRC_Check();
                    f1.First_Model_OTP_Write_Button_Click();
                    f1.ADD_GB_ProgressBar_Value(5 * step);
                }
            }
            form_dual_engineer.Dual_Cal_Diff_R_L_Gamma();
            f1.OC_Timer_Stop();

            if (Optic_Compensation_Succeed) System.Windows.Forms.MessageBox.Show("Optic Compensation Finish !");
        }


        private void Optic_compensation_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            TextBox_Vreg1_Clear();
            textBox_Total_Average_Meas_Count.Text = "0";

            if (radioButton_Single_Mode.Checked) //Single + Normal
            {
                DP116_Or_DP150_116_Is_True = true; //DP116 (Bool For Band/Gray Pattern Setting)

                Engineer_Mornitoring_Mode.getInstance().Show();
                
                Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
                form_engineer.Band_Radiobuttion_Select(0);//Select Band as 0

                form_engineer.RadioButton_All_Enable(false);
                Single_Mode_Optic_compensation();
                form_engineer.RadioButton_All_Enable(true);
            }
            else
            {
                DP116_Or_DP150_116_Is_True = false; //DP150 (Bool For Band/Gray Pattern Setting)
                Dual_Engineer_Monitoring_Mode.getInstance().Show();
                Dual_Engineer_Monitoring_Mode form_dual_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
                form_dual_engineer.Band_Radiobuttion_Select(0, true); //Select Band as 0 (Condition 1)
                form_dual_engineer.Band_Radiobuttion_Select(0, false); //Select Band as 0 (Condition 2)

                form_dual_engineer.Dual_RadioButton_All_Enable(false);
                if (radioButton_Dual_Mode_1.Checked)//Dual + Change Between Condition
                {
                    Dual_Mode_1_Optic_compensation();
                }
                else if (radioButton_Dual_Mode_2.Checked)//Dual + Change Between Band(Condition 1/2)
                {
                    Dual_Mode_2_Optic_compensation();
                }
                else if (radioButton_Dual_Mode_3.Checked)//Dual + Change Between Band/Gray(Condition 1/2)
                {
                    Dual_Mode_3_Optic_compensation();
                }
                else
                {
                    //Do nothing
                }
                form_dual_engineer.Dual_RadioButton_All_Enable(true);
            }
        }

        private void button_BSQH_Stop_Click(object sender, EventArgs e)
        {
            Optic_Compensation_Stop = true;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.OC_Timer_Stop();
        }

        private void radioButton_CMD2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_CMD2.Checked)
            {
                textBox_Page_Selection.Text = "0";
                textBox_Page_Selection.ReadOnly = false;
            }
        }

        private void radioButton_CMD1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_CMD1.Checked)
            {
                textBox_Page_Selection.Text = string.Empty;
                textBox_Page_Selection.ReadOnly = true;
            }
        }

        private void button_Page_Select_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if(radioButton_CMD1.Checked)
            {
                f1.DP116_CMD1_Page_Selection(false, false, ref Current_Page_Address,true);
            }
            else
            {
                int text_num = 0;
                
                try
                {
                    text_num = Convert.ToInt16(textBox_Page_Selection.Text);
                    if(text_num > 12)
                    {
                       text_num = 12;
                       textBox_Page_Selection.Text = "12";
                    }
                  
                    if(text_num < 0)
                    {
                       text_num = 0;
                       textBox_Page_Selection.Text = "0";
                    }

                    f1.DP116_CMD2_Page_Selection(text_num, false, false, ref Current_Page_Address, true);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Please Input Proper Page");
                }  
            }

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if (checkBox_VREF2_AM0_Compensation.Checked)
            {
                double VREF2_Margin = Convert.ToDouble(textBox_VREF2_Margin.Text);
                double Black_Margin = Convert.ToDouble(textBox_AM0_Margin.Text);
                double VREF_Resolution = Convert.ToDouble(textBox_VREF_Resolution.Text);
                double VREF2_Limit_Lv = Convert.ToDouble(textBox_VREF2_Limit_Lv.Text);
                VREF2_AM0_Compensation(VREF2_Margin, Black_Margin, VREF_Resolution, VREF2_Limit_Lv);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            IntPtr ptr = Imported_my_cpp_dll.Get_Dll_Information();
            string Message = Marshal.PtrToStringAnsi(ptr);
            System.Windows.MessageBox.Show(Message);
        }

        private void button_POCB_ON_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, true);
            f1.IPC_Quick_Send("mipi.write 0x15 0x7D 0x85");
            f1.GB_Status_AppendText_Nextline("POCB On", System.Drawing.Color.Green);
        }

        private void button_POCB_Off_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, true);
            f1.IPC_Quick_Send("mipi.write 0x15 0x7D 0xA5");
            f1.GB_Status_AppendText_Nextline("POCB Off", System.Drawing.Color.Red);
        }

        //Gamma Read
        private void button_Gamma_Down_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            f1.GB_Status_AppendText_Nextline("#--- VREF1 , VREF2 , AM0 ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.Blue, false, ref Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AD", "VREF1"); //VREF1
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AE", "VREF2"); //VREF2
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "AF", "AM0"); //AM0

            f1.GB_Status_AppendText_Nextline("#--- Vreg1 ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.Blue, false, ref Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "DD", "Vreg1(1)"); //Vreg1 +
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "DE", "Vreg2(2)"); //Vreg1 + 2

            f1.GB_Status_AppendText_Nextline("#--- IRC Setting ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.Blue, false, ref Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "4A");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(16, "4B");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(12, "4C");

            f1.GB_Status_AppendText_Nextline("#--- Gamma(Gray) ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.Blue, false, ref Current_Page_Address, true);
            f1.DP116_Read_Gamma_Gray();

            f1.GB_Status_AppendText_Nextline("#--- Gamma(White) ---", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(11, false, Color.Blue, false, ref Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "EA");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "EB");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "EC");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "ED");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(2, "EE");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(14, "EF");

            f1.GB_Status_AppendText_Nextline("#--- ELVSS  ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x2C #CMD2_P12 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection(12, false, false, ref Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(11, "BD");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(11, "BE");

            f1.GB_Status_AppendText_Nextline("#--- Vinit ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x20 #CMD2_P0 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "9F");
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "A2");

            f1.GB_Status_AppendText_Nextline("#--- ELVSS Set ---", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x10 #CMD1 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD1_Page_Selection(false, false, ref Current_Page_Address, true);
            f1.Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(1, "66");
        }

        //Gamma Down
        private void button_Gamma_Down_Real_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Engineer_Mornitoring_Mode.getInstance().Show();
            System.Windows.Forms.Application.DoEvents();
            
            //VREF1,2 / AM0 / Vreg1
            f1.DP116_Read_Vreg1_VREF_AM0_and_Send(true, true, ref Current_Page_Address);

            //IRC
            f1.GB_Status_AppendText_Nextline("#IRC Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.DarkGreen, false, ref Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(16, "4A", true);
            f1.Read_DP116_Quantity_Register_to_Send(16, "4B", true);
            f1.Read_DP116_Quantity_Register_to_Send(12, "4C", true);

            //Gamma 255
            f1.GB_Status_AppendText_Nextline("#Gamma Send from OC_DataGrid", System.Drawing.Color.Blue);
            this.Get_All_Band_Gray_Gamma();
            f1.DP116_CMD2_Page_Selection_And_Show(11, false, Color.DarkGreen, false, ref Current_Page_Address, true);
            Send_Band_Gray_Gamma(All_band_gray_Gamma, 0, 0, true);

            //Gamma Gray (<255)
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.DarkGreen, false, ref Current_Page_Address, true);
            for (int band = 0; band < 12; band++)
                Send_Band_Gray_Gamma(All_band_gray_Gamma, band, 1, true);

            //ELVSS
            f1.GB_Status_AppendText_Nextline("#ELVSS Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(12, false, Color.DarkGreen, false, ref Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(11, "BD", true);
            f1.Read_DP116_Quantity_Register_to_Send(11, "BE", true);

            //Vinit
            f1.GB_Status_AppendText_Nextline("#Vinit Read and Send", System.Drawing.Color.Blue);
            f1.DP116_CMD2_Page_Selection_And_Show(0, false, Color.DarkGreen, false, ref Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "9F", true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "A2", true);

            //ELVSS Set
            f1.GB_Status_AppendText_Nextline("#ELVSS Set Read and Send", System.Drawing.Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xFF 0x10 #CMD1 Select", System.Drawing.Color.Blue);
            f1.DP116_CMD1_Page_Selection(false, false, ref Current_Page_Address, true);
            f1.Read_DP116_Quantity_Register_to_Send(1, "66", true);
        }

        public void Send_Band_Gray_Gamma(RGB[,] All_band_gray_Gamma, int Current_Band, int Current_Gray, bool condition)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Update Gamma table as current Gamma

            //All_band_gray_Gamma[band,0] --> 각 밴드별 gray 255
            RGB temp_mipi_cmd1 = new RGB();
            RGB temp_mipi_cmd2 = new RGB();
            RGB[] Band_Gray255_9th_data = new RGB[16]; //AM256[8] setting
            RGB[] Band_8ea_Gray_data = new RGB[16]; //AM256[7:0] setting
            RGB Param_H1 = new RGB();//EA,EC,EEh 1st param
            RGB Param_H2 = new RGB();//EA,EC,EEh 2nd param

                if (Current_Gray == 0) //if Gray = 255 (Gray255 모든 Band 적용)
                {
                    for (int band = 0; band < 16; band++)
                    {
                        if (band == 9 || band == 10 || band == 14 || band == 15)
                        {
                            Band_Gray255_9th_data[band].Set_Value(0, 0, 0);
                            Band_8ea_Gray_data[band].Set_Value(0, 0, 0);
                        }
                        else if (band == 11 || band == 12 || band == 13)
                        {
                            Band_Gray255_9th_data[band].Set_Value(All_band_gray_Gamma[band - 2, 0].int_R >> 8
                                , All_band_gray_Gamma[band - 2, 0].int_G >> 8, All_band_gray_Gamma[band - 2, 0].int_B >> 8);

                            Band_8ea_Gray_data[band].Set_Value(All_band_gray_Gamma[band - 2, 0].int_R & 0xFF
                                , All_band_gray_Gamma[band - 2, 0].int_G & 0xFF, All_band_gray_Gamma[band - 2, 0].int_B & 0xFF);
                        }
                        else
                        {
                            Band_Gray255_9th_data[band].Set_Value(All_band_gray_Gamma[band, 0].int_R >> 8
                                , All_band_gray_Gamma[band, 0].int_G >> 8, All_band_gray_Gamma[band, 0].int_B >> 8);

                            Band_8ea_Gray_data[band].Set_Value(All_band_gray_Gamma[band, 0].int_R & 0xFF
                                , All_band_gray_Gamma[band, 0].int_G & 0xFF, All_band_gray_Gamma[band, 0].int_B & 0xFF);
                        }
                    }

                    //AM256[7:0] setting
                    if (condition)
                    {
                        temp_mipi_cmd2.R = "mipi.write 0x39 0xEB";
                        temp_mipi_cmd2.G = "mipi.write 0x39 0xED";
                        temp_mipi_cmd2.B = "mipi.write 0x39 0xEF";
                    }
                    else
                    {
                        temp_mipi_cmd2.R = "mipi.write 0x39 0xF1";
                        temp_mipi_cmd2.G = "mipi.write 0x39 0xF3";
                        temp_mipi_cmd2.B = "mipi.write 0x39 0xF5";
                    }
                    //binary format string (xxxxxxxx)
                    for (int band = 0; band <= 7; band++)
                    {
                        Param_H1.R += Band_Gray255_9th_data[band].R; //"xxxxxxxx" (bit) string form
                        Param_H1.G += Band_Gray255_9th_data[band].G; //"xxxxxxxx" (bit) string form
                        Param_H1.B += Band_Gray255_9th_data[band].B; //"xxxxxxxx" (bit) string form

                        temp_mipi_cmd2.R += " 0x" + Band_8ea_Gray_data[band].int_R.ToString("X2");
                        temp_mipi_cmd2.G += " 0x" + Band_8ea_Gray_data[band].int_G.ToString("X2");
                        temp_mipi_cmd2.B += " 0x" + Band_8ea_Gray_data[band].int_B.ToString("X2");
                    }
                    for (int band = 8; band <= 15; band++)
                    {
                        Param_H2.R += Band_Gray255_9th_data[band].R; //"xxxxxxxx" (bit) string form
                        Param_H2.G += Band_Gray255_9th_data[band].G; //"xxxxxxxx" (bit) string form
                        Param_H2.B += Band_Gray255_9th_data[band].B; //"xxxxxxxx" (bit) string form

                        temp_mipi_cmd2.R += " 0x" + Band_8ea_Gray_data[band].int_R.ToString("X2");
                        temp_mipi_cmd2.G += " 0x" + Band_8ea_Gray_data[band].int_G.ToString("X2");
                        temp_mipi_cmd2.B += " 0x" + Band_8ea_Gray_data[band].int_B.ToString("X2");
                    }

                    //hex format string (XX)
                    Param_H1.R = String.Format("{0:X2}", Convert.ToUInt64(Param_H1.R, 2));
                    Param_H1.G = String.Format("{0:X2}", Convert.ToUInt64(Param_H1.G, 2));
                    Param_H1.B = String.Format("{0:X2}", Convert.ToUInt64(Param_H1.B, 2));
                    Param_H2.R = String.Format("{0:X2}", Convert.ToUInt64(Param_H2.R, 2));
                    Param_H2.G = String.Format("{0:X2}", Convert.ToUInt64(Param_H2.G, 2));
                    Param_H2.B = String.Format("{0:X2}", Convert.ToUInt64(Param_H2.B, 2));

                    //AM256[8] setting
                    if (condition)
                    {
                        temp_mipi_cmd1.R = "mipi.write 0x39 0xEA 0x" + Param_H1.R + " 0x" + Param_H2.R;
                        temp_mipi_cmd1.G = "mipi.write 0x39 0xEC 0x" + Param_H1.G + " 0x" + Param_H2.G;
                        temp_mipi_cmd1.B = "mipi.write 0x39 0xEE 0x" + Param_H1.B + " 0x" + Param_H2.B;
                    }
                    else
                    {
                        temp_mipi_cmd1.R = "mipi.write 0x39 0xF0 0x" + Param_H1.R + " 0x" + Param_H2.R;
                        temp_mipi_cmd1.G = "mipi.write 0x39 0xF2 0x" + Param_H1.G + " 0x" + Param_H2.G;
                        temp_mipi_cmd1.B = "mipi.write 0x39 0xF4 0x" + Param_H1.B + " 0x" + Param_H2.B;
                    }


                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd1.R,Color.Black);
                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd1.G, Color.Black);
                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd1.B, Color.Black);
                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd2.R, Color.Black);
                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd2.G, Color.Black);
                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd2.B, Color.Black);
                }
                else//if Gray < 255 (Current Band의 Gray255 제외 모든 Gray 적용)
                {

                    RGB[] Params = new RGB[12]; //B0h's Parameter amount = 12ea
                    const int B0h = 176;//Hex = B0 , Dex = 176
                    string R_Gamma_Address = (B0h + (Current_Band * 3)).ToString("X2");
                    string G_Gamma_Address = (B0h + (Current_Band * 3) + 1).ToString("X2");
                    string B_Gamma_Address = (B0h + (Current_Band * 3) + 2).ToString("X2");
                    if (Current_Band == 9 || Current_Band == 10 || Current_Band == 11) //AOD 1,2,3
                    {
                        R_Gamma_Address = (B0h + ((Current_Band + 2) * 3)).ToString("X2");
                        G_Gamma_Address = (B0h + ((Current_Band + 2) * 3) + 1).ToString("X2");
                        B_Gamma_Address = (B0h + ((Current_Band + 2) * 3) + 2).ToString("X2");
                    }
                    RGB[] temp = new RGB[3];

                    //GR8
                    Params[11].R = (All_band_gray_Gamma[Current_Band, 1].int_R & 0xFF).ToString("X2");
                    Params[11].G = (All_band_gray_Gamma[Current_Band, 1].int_G & 0xFF).ToString("X2");
                    Params[11].B = (All_band_gray_Gamma[Current_Band, 1].int_B & 0xFF).ToString("X2");
                    temp[0].Set_Value((All_band_gray_Gamma[Current_Band, 1].int_R >> 8)
                        , (All_band_gray_Gamma[Current_Band, 1].int_G >> 8), (All_band_gray_Gamma[Current_Band, 1].int_B >> 8));

                    //GR7
                    Params[10].R = (All_band_gray_Gamma[Current_Band, 2].int_R & 0xFF).ToString("X2");
                    Params[10].G = (All_band_gray_Gamma[Current_Band, 2].int_G & 0xFF).ToString("X2");
                    Params[10].B = (All_band_gray_Gamma[Current_Band, 2].int_B & 0xFF).ToString("X2");
                    temp[1].Set_Value((All_band_gray_Gamma[Current_Band, 2].int_R >> 8) << 1
                        , (All_band_gray_Gamma[Current_Band, 2].int_G >> 8) << 1, (All_band_gray_Gamma[Current_Band, 2].int_B >> 8) << 1);

                    //GR6
                    Params[9].R = (All_band_gray_Gamma[Current_Band, 3].int_R & 0xFF).ToString("X2");
                    Params[9].G = (All_band_gray_Gamma[Current_Band, 3].int_G & 0xFF).ToString("X2");
                    Params[9].B = (All_band_gray_Gamma[Current_Band, 3].int_B & 0xFF).ToString("X2");
                    temp[2].Set_Value((All_band_gray_Gamma[Current_Band, 3].int_R >> 8) << 2
                        , (All_band_gray_Gamma[Current_Band, 3].int_G >> 8) << 2, (All_band_gray_Gamma[Current_Band, 3].int_B >> 8) << 2);

                    //GR[8]678
                    Params[8].Equal_Value(temp[0] + temp[1] + temp[2]);
                    Params[8].R = (Params[8].int_R).ToString("X2");
                    Params[8].G = (Params[8].int_G).ToString("X2");
                    Params[8].B = (Params[8].int_B).ToString("X2");


                    //GR5
                    Params[7].R = (All_band_gray_Gamma[Current_Band, 4].int_R & 0xFF).ToString("X2");
                    Params[7].G = (All_band_gray_Gamma[Current_Band, 4].int_G & 0xFF).ToString("X2");
                    Params[7].B = (All_band_gray_Gamma[Current_Band, 4].int_B & 0xFF).ToString("X2");
                    temp[0].Set_Value((All_band_gray_Gamma[Current_Band, 4].int_R >> 8)
                        , (All_band_gray_Gamma[Current_Band, 4].int_G >> 8), (All_band_gray_Gamma[Current_Band, 4].int_B >> 8));

                    //GR4
                    Params[6].R = (All_band_gray_Gamma[Current_Band, 5].int_R & 0xFF).ToString("X2");
                    Params[6].G = (All_band_gray_Gamma[Current_Band, 5].int_G & 0xFF).ToString("X2");
                    Params[6].B = (All_band_gray_Gamma[Current_Band, 5].int_B & 0xFF).ToString("X2");
                    temp[1].Set_Value((All_band_gray_Gamma[Current_Band, 5].int_R >> 8) << 1
                        , (All_band_gray_Gamma[Current_Band, 5].int_G >> 8) << 1, (All_band_gray_Gamma[Current_Band, 5].int_B >> 8) << 1);

                    //GR3
                    Params[5].R = (All_band_gray_Gamma[Current_Band, 6].int_R & 0xFF).ToString("X2");
                    Params[5].G = (All_band_gray_Gamma[Current_Band, 6].int_G & 0xFF).ToString("X2");
                    Params[5].B = (All_band_gray_Gamma[Current_Band, 6].int_B & 0xFF).ToString("X2");
                    temp[2].Set_Value((All_band_gray_Gamma[Current_Band, 6].int_R >> 8) << 2
                        , (All_band_gray_Gamma[Current_Band, 6].int_G >> 8) << 2, (All_band_gray_Gamma[Current_Band, 6].int_B >> 8) << 2);

                    //GR[8]345
                    Params[4].Equal_Value(temp[0] + temp[1] + temp[2]);
                    Params[4].R = (Params[4].int_R).ToString("X2");
                    Params[4].G = (Params[4].int_G).ToString("X2");
                    Params[4].B = (Params[4].int_B).ToString("X2");


                    //GR2
                    Params[3].R = (All_band_gray_Gamma[Current_Band, 7].int_R & 0xFF).ToString("X2");
                    Params[3].G = (All_band_gray_Gamma[Current_Band, 7].int_G & 0xFF).ToString("X2");
                    Params[3].B = (All_band_gray_Gamma[Current_Band, 7].int_B & 0xFF).ToString("X2");
                    temp[0].Set_Value((All_band_gray_Gamma[Current_Band, 7].int_R >> 8)
                        , (All_band_gray_Gamma[Current_Band, 7].int_G >> 8), (All_band_gray_Gamma[Current_Band, 7].int_B >> 8));

                    //GR1
                    Params[2].R = (All_band_gray_Gamma[Current_Band, 8].int_R & 0xFF).ToString("X2");
                    Params[2].G = (All_band_gray_Gamma[Current_Band, 8].int_G & 0xFF).ToString("X2");
                    Params[2].B = (All_band_gray_Gamma[Current_Band, 8].int_B & 0xFF).ToString("X2");
                    temp[1].Set_Value((All_band_gray_Gamma[Current_Band, 8].int_R >> 8) << 1
                        , (All_band_gray_Gamma[Current_Band, 8].int_G >> 8) << 1, (All_band_gray_Gamma[Current_Band, 8].int_B >> 8) << 1);

                    //GR0
                    Params[1].R = (All_band_gray_Gamma[Current_Band, 9].int_R & 0xFF).ToString("X2");
                    Params[1].G = (All_band_gray_Gamma[Current_Band, 9].int_G & 0xFF).ToString("X2");
                    Params[1].B = (All_band_gray_Gamma[Current_Band, 9].int_B & 0xFF).ToString("X2");
                    temp[2].Set_Value((All_band_gray_Gamma[Current_Band, 9].int_R >> 8) << 2
                        , (All_band_gray_Gamma[Current_Band, 9].int_G >> 8) << 2, (All_band_gray_Gamma[Current_Band, 9].int_B >> 8) << 2);

                    //GR[8]012
                    Params[0].Equal_Value(temp[0] + temp[1] + temp[2]);
                    Params[0].R = (Params[0].int_R).ToString("X2");
                    Params[0].G = (Params[0].int_G).ToString("X2");
                    Params[0].B = (Params[0].int_B).ToString("X2");
                    
                    //Apply Red Gamma
                    string temp_mipi_cmd = "mipi.write 0x39 0x" + R_Gamma_Address;
                    for (int i = 0; i < 12; i++)
                    {
                        temp_mipi_cmd += " 0x" + Params[i].R;
                    }
                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd,Color.Black);

                    //Apply Green Gamma
                    temp_mipi_cmd = "mipi.write 0x39 0x" + G_Gamma_Address;
                    for (int i = 0; i < 12; i++)
                    {
                        temp_mipi_cmd += " 0x" + Params[i].G;
                    }
                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd, Color.Black);

                    //Apply Blue Gamma
                    temp_mipi_cmd = "mipi.write 0x39 0x" + B_Gamma_Address;
                    for (int i = 0; i < 12; i++)
                    {
                        temp_mipi_cmd += " 0x" + Params[i].B;
                    }
                    f1.IPC_Quick_Send_And_Show(temp_mipi_cmd, Color.Black);
                }
        }

        private void button_ELVSS_Margin_Test_Click(object sender, EventArgs e)
        {
            int Band = Convert.ToInt16(textBox_ELVSS_Band_Select.Text);
            
            if (Band >= 8)
            {
                Band = 8;
                textBox_ELVSS_Band_Select.Text = "8";
            }

            if (Band < 0)
            {
                Band = 0;
                textBox_ELVSS_Band_Select.Text = "0";
            }

            DP116_DBV_Setting(Band);//DBV Setting

            ELVSS_Margin_Test(12, Band, "BE");
        }

        private void ELVSS_Margin_Test(int Quantity,int Band,string Register)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Read_DP116_Page_Quantity_Register(12,Quantity, Register);
            string[] Param = new string[Quantity];
            
            for (int i = 0; i < Quantity; i++)
            {
                Param[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            double ELVSS = 0;
            // 23h = -4.0v , 9h = -1.4v
            for (int Current_Dec = 40; Current_Dec >= 9; Current_Dec--)
            {
                ELVSS = -4 + (35 - Current_Dec) * 0.1;
                Param[Band] = Current_Dec.ToString("X2");
                f1.Long_Packet_CMD_Send(Quantity, Register, Param);
                Thread.Sleep(20);
                f1.CA_Measure_For_ELVSS(ELVSS.ToString());
            }
        }

        private void STMP03_Normal_ELVSS_Setting(int Band, string Register, double ELVSS)
        {
            int Quantity = 12;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Read_DP116_Page_Quantity_Register(12, Quantity, Register); //Read CMD2_Page12 Register 12ea Parameters
            string[] Param = new string[Quantity];

            for (int i = 0; i < Quantity; i++)
            {
                Param[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            int Current_Dec = Convert.ToInt16(35 - 10 * (ELVSS + 4)) + 4;
            Param[Band] = Current_Dec.ToString("X2");
            f1.Long_Packet_CMD_Send(Quantity, Register, Param);

            f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + " Final ELVSS : " + ELVSS.ToString(), Color.Green);
        }

        private void SET_ELVSS(int Band, double ELVSS, string[] TI_ELVSS, string[] STMP03_ELVSS)
        {
            int Current_Dec = Convert.ToInt16(35 - 10 * (ELVSS + 4));
            TI_ELVSS[Band] = Current_Dec.ToString("X2");

            Current_Dec = Convert.ToInt16(35 - 10 * (ELVSS + 4)) + 4;
            STMP03_ELVSS[Band] = Current_Dec.ToString("X2");
        }





        private void TI_Normal_ELVSS_Setting(int Band, string Register,double ELVSS)
        {
           int Quantity = 12;

           Form1 f1 = (Form1)Application.OpenForms["Form1"];
           f1.Read_DP116_Page_Quantity_Register(12, Quantity, Register); //Read CMD2_Page12 Register 12ea Parameters
           string[] Param = new string[Quantity];

           for (int i = 0; i < Quantity; i++)
           {
               Param[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
           }

           int Current_Dec = Convert.ToInt16(35 - 10 * (ELVSS + 4));
           Param[Band] = Current_Dec.ToString("X2");
           f1.Long_Packet_CMD_Send(Quantity, Register, Param);

           f1.GB_Status_AppendText_Nextline("B" + Band.ToString() + " Final ELVSS : " + ELVSS.ToString(), Color.Green); 
        }

        private void Normal_Vinit_Setting(double Vinit)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DP116_CMD2_Page_Selection(0, false, false, ref Current_Page_Address, true);

            int Current_Dec =  Convert.ToInt16( 33 - 10 * (Vinit + 3.3));
            
            String V_Init = Current_Dec.ToString("X2");
            f1.IPC_Quick_Send("mipi.write 0x15 0x9F 0x" + V_Init);
            f1.IPC_Quick_Send("mipi.write 0x15 0xA2 0x" + V_Init);

            f1.GB_Status_AppendText_Nextline("Final Vinit : " + Vinit.ToString(), Color.Green); 
        }

        private void HBM_Mode_Gray255_Compensation()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //f1.Engineering_Mode_Show();
            Engineer_Mornitoring_Mode form_engineer = (Engineer_Mornitoring_Mode)Application.OpenForms["Engineer_Mornitoring_Mode"];
           
            DP116_DBV_Setting(0);//HBM DBV Setting
            DP116_Pattern_Setting(0, 0, DP116_Or_DP150_116_Is_True);//HBM Gray255 Pattern Setting
            Thread.Sleep(300); //Pattern 안정화 Time
            form_engineer.Band_Radiobuttion_Select(0);//Select HBM
            Get_Param(0); //Get (First) Gray255 Gamma,Target,Limit From OC-Param-Table 
            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);
                                           
            Optic_Compensation_Succeed = false;
            Optic_Compensation_Stop = false;
            int loop_count = 0;
            Infinite_Count = 0;
            Infinite = false;
            
            RGB_Need_To_Change[0] = true;RGB_Need_To_Change[1] = true;RGB_Need_To_Change[2] = true;
            Update_and_Send_All_Band_Gray_Gamma(Gamma, 0, 0, true); //Setting Gamma Values for HBM/Gray255
            Thread.Sleep(20);

            f1.GB_Status_AppendText_Nextline("HBM/Gray255 Compensation Start", Color.Green);

            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
            {
                f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure

                Infinite_Loop_Check(loop_count);
                Prev_Gamma.Equal_Value(Gamma);

                Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                Diff_Gamma = Gamma - Prev_Gamma;
                f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                else RGB_Need_To_Change[0] = false;
                if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                else RGB_Need_To_Change[1] = false;
                if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                else RGB_Need_To_Change[2] = false;

                Update_and_Send_All_Band_Gray_Gamma(Gamma, 0, 0, true); //Setting Gamma Values for HBM/Gray255
                Thread.Sleep(20);

                Update_Engineering_Sheet(0, 0, loop_count, "X");

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

        private void Dual_Mode_HBM_Mode_Gray255_Compensation(bool Condition)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //f1.Engineering_Mode_Show();
            Dual_Engineer_Monitoring_Mode form_dual_engineer = (Dual_Engineer_Monitoring_Mode)Application.OpenForms["Dual_Engineer_Monitoring_Mode"];

            DP116_DBV_Setting(0);//HBM DBV Setting
            DP116_Pattern_Setting(0, 0, DP116_Or_DP150_116_Is_True);//HBM Gray255 Pattern Setting
            Thread.Sleep(300); //Pattern 안정화 Time
            form_dual_engineer.Band_Radiobuttion_Select(0, Condition);//Select HBM
            Dual_Mode_Get_Param(0, Condition); //Get (First) Gray255 Gamma,Target,Limit From OC-Param-Table                        
            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);
                            
            Optic_Compensation_Succeed = false;
            Optic_Compensation_Stop = false;
            int loop_count = 0;
            Infinite_Count = 0;
            Infinite = false;

            RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
            Update_and_Send_All_Band_Gray_Gamma(Gamma, 0, 0, Condition); //Setting Gamma Values for HBM/Gray255
            Thread.Sleep(20);

            while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
            {
                
                f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure

                Infinite_Loop_Check(loop_count);
                Prev_Gamma.Equal_Value(Gamma);

                Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                        , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                f1.GB_Status_AppendText_Nextline("HBM/Gray255" + " Red/Green/Blue = " + Gamma.int_R.ToString() + "," + Gamma.int_G.ToString() + "," + Gamma.int_B.ToString(), Color.Black);

                Diff_Gamma = Gamma - Prev_Gamma;
                f1.GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
                if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                else RGB_Need_To_Change[0] = false;
                if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                else RGB_Need_To_Change[1] = false;
                if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                else RGB_Need_To_Change[2] = false;

                Update_and_Send_All_Band_Gray_Gamma(Gamma, 0, 0, Condition); //Setting Gamma Values for HBM/Gray255
                Thread.Sleep(20);

                Dual_Mode_Update_Engineering_Sheet(0, 0, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)

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

            f1.GB_Status_AppendText_Nextline("HBM / Gray255 Compensation Finish(Dual Mode)", Color.Green);
        }

        private void ELVSS_Compensation()
        {
            DP116_DBV_Setting(0);//HBM DBV Setting
            DP116_Pattern_Setting(0, 0, DP116_Or_DP150_116_Is_True);//HBM Gray255 Pattern Setting

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

                string[] TI_ELVSS = new string[9];
                string[] STMP03_ELVSS = new string[9];
                for (int i = 0; i < 9; i++) TI_ELVSS[i] = STMP03_ELVSS[i] = "00";

                f1.DP116_CMD2_Page_Selection(12, false, false, ref Current_Page_Address, true);
                
                for (ELVSS = First_ELVSS; ELVSS < -1.4; ELVSS += 0.1)
                {
                    if (ELVSS == First_ELVSS)
                    {
                        //TI_Normal_ELVSS_Setting(0, "BD", First_ELVSS);//HBM
                        SET_ELVSS(0, First_ELVSS, TI_ELVSS, STMP03_ELVSS);
                        f1.Long_Packet_CMD_Send(9, "BE", TI_ELVSS);

                        Thread.Sleep(20);
                        f1.CA_Measure_button_Perform_Click(ref First_Measure.double_X, ref First_Measure.double_Y, ref First_Measure.double_Lv);
                    }
                    else
                    {
                        //TI_Normal_ELVSS_Setting(0, "BD", ELVSS);//HBM
                        SET_ELVSS(0, ELVSS, TI_ELVSS, STMP03_ELVSS);
                        f1.Long_Packet_CMD_Send(9, "BE", TI_ELVSS);

                        Thread.Sleep(20);
                        f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);

                        Imported_my_cpp_dll.ELVSS_Compensation(ref ELVSS_Find_Finish, First_ELVSS, ref ELVSS, ref Vinit, ref First_Slope, Vinit_Margin, ELVSS_Margin, Slope_Margin, First_Measure.double_X, First_Measure.double_Y
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


                SET_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                SET_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                SET_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                SET_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                SET_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                SET_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                SET_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                SET_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                SET_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset.Text), TI_ELVSS, STMP03_ELVSS);


                f1.DP116_CMD2_Page_Selection(12, false, false, ref Current_Page_Address, true);
                f1.Long_Packet_CMD_Send(9, "BE", TI_ELVSS);
                Thread.Sleep(20);
                f1.Long_Packet_CMD_Send(9, "BD", STMP03_ELVSS);
                Thread.Sleep(20);
            }
            button_Read_ELVSS_Vinit.PerformClick();
        }


        

        private void Apply_ELVSS()
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Read_DP116_Page_Quantity_Register(12, 16, "4B");
            f1.GB_Status_AppendText_Nextline("After IRC Compensation : " + f1.textBox2_cmd.Text.Substring(18, 16 * 5), Color.Green);
        }

        private void button_IRC_On_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("---IRC On--", Color.Green);
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xFF 0x10", Color.Green);
            Current_Page_Address = "0x10";
            if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("After Current_Page_Address = " + Current_Page_Address, Color.Black);
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0x7E 0xFC", Color.Green);
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0x80 0x03", Color.Green);
        }

        private void button_IRC_Off_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("---IRC Off--", Color.Red);
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xFF 0x10", Color.Red);
            Current_Page_Address = "0x10";
            if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("After Current_Page_Address = " + Current_Page_Address, Color.Black);
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0x7E 0xFD", Color.Red);
            f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0x80 0x02", Color.Red);
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
            checkBox_Band11.Checked = true;
            checkBox_Band12.Checked = true;
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
            checkBox_Band11.Checked = false;
            checkBox_Band12.Checked = false;
        }

        private void button_Gamma_Set_1_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("---Gamma Set 1 was selected---", Color.Blue);
            f1.IPC_Quick_Send("mipi.write 0x15 0xFF 0x10");
            Current_Page_Address = "0x10";
            if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("After Current_Page_Address = " + Current_Page_Address, Color.Black);
            f1.IPC_Quick_Send("mipi.write 0x15 0x26 0x01");
        }

        private void button_Gamma_Set_2_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("---Gamma Set 2 was selected---", Color.Red);
            f1.IPC_Quick_Send("mipi.write 0x15 0xFF 0x10");
            Current_Page_Address = "0x10";
            if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("After Current_Page_Address = " + Current_Page_Address, Color.Black);
            f1.IPC_Quick_Send("mipi.write 0x15 0x26 0x02");
        }

        private void groupBox35_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton_Gamma_Set_1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Textbox_ELVSS_Vinit_Clear()
        {
            textBox_ELVSS_B0.Text = string.Empty;
            textBox_ELVSS_B1.Text = string.Empty;
            textBox_ELVSS_B2.Text = string.Empty;
            textBox_ELVSS_B3.Text = string.Empty;
            textBox_ELVSS_B4.Text = string.Empty;
            textBox_ELVSS_B5.Text = string.Empty;
            textBox_ELVSS_B6.Text = string.Empty;
            textBox_ELVSS_B7.Text = string.Empty;
            textBox_ELVSS_B8.Text = string.Empty;

            textBox_ELVSS_A0.Text = string.Empty;
            textBox_ELVSS_A1.Text = string.Empty;
            textBox_ELVSS_A2.Text = string.Empty;

            textBox_Vinit.Text = string.Empty;
        }

        private void button_Read_ELVSS_Vinit_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Normal ELVSS Read
            f1.IPC_Quick_Send("mipi.write 0x15 0xFF 0x10");
            f1.OTP_Read(1, "66");
            string ELVSS_SET = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            if (ELVSS_SET == "00")
            {
                f1.GB_Status_AppendText_Nextline("ELVSS Set1 is selected(mipi.write 0x15 0x66 0x00)",Color.Blue);
                f1.Read_DP116_Page_Quantity_Register(12, 9, "BD");//CMD2 Page12 "BF" 9ea Param Read   
            }
            else
            {
                f1.GB_Status_AppendText_Nextline("ELVSS Set2 is selected(mipi.write 0x15 0x66 0x01)", Color.Red);
                f1.Read_DP116_Page_Quantity_Register(12, 9, "BE");//CMD2 Page12 "BF" 9ea Param Read
            }
            
            string[] hex_ELVSS = new string[9];
            double[] dec_ELVSS = new double[9];
            double[] ELVSS = new double[9];
            for (int i = 0; i < 9; i++)
            {
                hex_ELVSS[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS[i] = Convert.ToDouble(Convert.ToInt16(hex_ELVSS[i], 16));
                ELVSS[i] = ((35 - dec_ELVSS[i]) / 10.0) - 4;
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

            //AOD ELVSS Read
            f1.Read_DP116_Page_Quantity_Register(12, 4, "C1");//CMD2 Page12 "C1" 4ea Param Read
            hex_ELVSS = new string[3];
            dec_ELVSS = new double[3];
            ELVSS = new double[3];
            for (int i = 0; i < 3; i++)
            {
                hex_ELVSS[i] = f1.dataGridView1.Rows[i + 1].Cells[1].Value.ToString();
                dec_ELVSS[i] = Convert.ToDouble(Convert.ToInt16(hex_ELVSS[i], 16));
                ELVSS[i] =  - (dec_ELVSS[i] / 50.0);
            }
            textBox_ELVSS_A0.Text = ELVSS[0].ToString() + "v (" + hex_ELVSS[0] + "h)";
            textBox_ELVSS_A1.Text = ELVSS[1].ToString() + "v (" + hex_ELVSS[1] + "h)";
            textBox_ELVSS_A2.Text = ELVSS[2].ToString() + "v (" + hex_ELVSS[2] + "h)";

            //Vinit Read
            f1.Read_DP116_Page_Quantity_Register(0, 1, "9F"); //CMD2 Page0 "9F" 1ea Param Read
            string hex_Vinit = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            double dec_Vinit = Convert.ToDouble(Convert.ToInt16(hex_Vinit, 16));
            double Vinit = (- dec_Vinit / 10.0);
            textBox_Vinit.Text = Vinit.ToString() + "v (" + hex_Vinit + "h)";
        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void Textbox_AM0_VREF2_Clear()
        {
            textBox_VREF2.Text = string.Empty;
            textBox_AM0.Text = string.Empty;
        }

        private void button_Read_AM0_VREF2_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Read_DP116_Page_Quantity_Register(0, 1, "AE");
            string hex_VREF2 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            int VREF2 = Convert.ToInt16(hex_VREF2, 16) & 0x7F;
            hex_VREF2 = VREF2.ToString("X2");
            textBox_VREF2.Text = VREF2 + " (" + hex_VREF2 + "h)";

            f1.Read_DP116_Page_Quantity_Register(0, 1, "AF");
            string hex_AM0 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            int AM0 = Convert.ToInt16(hex_AM0, 16) & 0x7F;
            hex_AM0 = AM0.ToString("X2");
            textBox_AM0.Text = AM0 + " (" + hex_AM0 + "h)";
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

            textBox_Vreg1_A0.Text = string.Empty;
            textBox_Vreg1_A1.Text = string.Empty;
            textBox_Vreg1_A2.Text = string.Empty;

            textBox_Vreg1_B0_2.Text = string.Empty;
            textBox_Vreg1_B1_2.Text = string.Empty;
            textBox_Vreg1_B2_2.Text = string.Empty;
            textBox_Vreg1_B3_2.Text = string.Empty;
            textBox_Vreg1_B4_2.Text = string.Empty;
            textBox_Vreg1_B5_2.Text = string.Empty;
            textBox_Vreg1_B6_2.Text = string.Empty;
            textBox_Vreg1_B7_2.Text = string.Empty;
            textBox_Vreg1_B8_2.Text = string.Empty;

            textBox_Vreg1_A0_2.Text = string.Empty;
            textBox_Vreg1_A1_2.Text = string.Empty;
            textBox_Vreg1_A2_2.Text = string.Empty;
        }

        private void button_Vreg1_Read_Click(object sender, EventArgs e)
        {
            TextBox_Vreg1_Clear();
            Application.DoEvents();

            bool Gamma_Set1 = true;

                //Read DDh
            textBox_Vreg1_B0.Text = Vreg1_Read2(0, Gamma_Set1, true).ToString() + " (" + Vreg1_Read2(0, Gamma_Set1, false).ToString("X3") + "h)"; //Read Vreg1 Value
            textBox_Vreg1_B1.Text = Vreg1_Read2(1, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(1, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B2.Text = Vreg1_Read2(2, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(2, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B3.Text = Vreg1_Read2(3, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(3, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B4.Text = Vreg1_Read2(4, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(4, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B5.Text = Vreg1_Read2(5, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(5, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B6.Text = Vreg1_Read2(6, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(6, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B7.Text = Vreg1_Read2(7, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(7, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value

                //Read DEh
            textBox_Vreg1_B8.Text = Vreg1_Read2(8, Gamma_Set1, true).ToString() + " (" + Vreg1_Read2(8, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_A0.Text = Vreg1_Read2(11, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(11, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_A1.Text = Vreg1_Read2(12, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(12, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_A2.Text = Vreg1_Read2(13, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(13, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value

            Gamma_Set1 = false;

            //Read DDh
            textBox_Vreg1_B0_2.Text = Vreg1_Read2(0, Gamma_Set1, true).ToString() + " (" + Vreg1_Read2(0, Gamma_Set1, false).ToString("X3") + "h)"; //Read Vreg1 Value
            textBox_Vreg1_B1_2.Text = Vreg1_Read2(1, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(1, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B2_2.Text = Vreg1_Read2(2, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(2, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B3_2.Text = Vreg1_Read2(3, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(3, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B4_2.Text = Vreg1_Read2(4, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(4, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B5_2.Text = Vreg1_Read2(5, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(5, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B6_2.Text = Vreg1_Read2(6, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(6, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_B7_2.Text = Vreg1_Read2(7, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(7, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value

            //Read DEh
            textBox_Vreg1_B8_2.Text = Vreg1_Read2(8, Gamma_Set1, true).ToString() + " (" + Vreg1_Read2(8, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_A0_2.Text = Vreg1_Read2(11, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(11, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_A1_2.Text = Vreg1_Read2(12, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(12, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
            textBox_Vreg1_A2_2.Text = Vreg1_Read2(13, Gamma_Set1, false).ToString() + " (" + Vreg1_Read2(13, Gamma_Set1, false).ToString("X3") + "h)"; ; //Read Vreg1 Value
        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Engineer_Mornitoring_Mode.getInstance().Show();

            Get_All_Band_Vreg1(All_band_gray_Gamma);

            /*
            int[] Gamma_R = new int[10] { 99, 23, 44, 51 , 66, 77, 88, 33, 11, 111 };
            int[] Gamma_G = new int[10] { 99, 23, 44, 111, 59, 66, 77, 33, 88, 11  };
            int[] Gamma_B = new int[10] { 99, 23, 44, 55 , 33, 77, 88, 22, 11, 1 };
            int Out_R = 0;
            int Out_G = 0;
            int Out_B = 0;

            RGB_Gamma_Initial_Values(10,Gamma_R, Gamma_G, Gamma_B, ref Out_R, ref Out_G, ref Out_B);

            System.Windows.Forms.MessageBox.Show(Out_R.ToString() + "/" + Out_G.ToString() + "/" + Out_B.ToString());
            */
            
            /*
           string Path = "";

           if (f1.label_Model_Name.Text == "Model Name : DP086")
               Path = Directory.GetCurrentDirectory() + "\\DP086\\DP086_Gamma_Vreg1_Log.csv";
           else
               Path = Directory.GetCurrentDirectory() + "\\DP116\\DP116_Gamma_Vreg1_Log.csv";

           Read_Data_From_Log(Path,ref RGBVre1_Log_Read_data);
           Get_Update_RGBVre1_Log_data(RGBVre1_Log_Read_data, RGBVre1_Log_data);
           Get_All_Band_Gray_Gamma(); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
           Get_All_Band_Vreg1(All_band_gray_Gamma); //Get update Vreg1 to All_band_gray_Gamma[band,0]

           Update_OC_Gamma_From_Log(RGBVre1_Log_data);
           */
            
           /*
           string appendText = "";
           Get_Init_String(ref appendText, RGBVre1_Log_Read_data);
           Shift_and_Get_New_Data(RGBVre1_Log_data, All_band_gray_Gamma);
           Merge_Text(ref appendText, RGBVre1_Log_data);
           File.WriteAllText(Path, appendText);//Clear and Write*/
           
        }

        private void Get_Init_String(ref string appendText, string[][] RGBVre1_Log_Read_data)
        {
            IntPtr ptr = Imported_my_cpp_dll.Get_Dll_Information();
            string Dll_Info = Marshal.PtrToStringAnsi(ptr);

           appendText =  "-,Sample #1,,,,Sample #2,,,,Sample #3,,,,Sample #4,,,,Sample #5,,,,Sample #6,,,,Sample #7,,,,Sample #8,,,,Sample #9,,,,Sample #10,,," + Environment.NewLine;
           
            int length = RGBVre1_Log_Read_data[1].Length;
            for (int j = 0; j < length - 4; j++)
            {
                if (j == 0)
                    appendText += "-,";
                else
                    appendText += (RGBVre1_Log_Read_data[1][j + 4] + ",");
            }
            appendText += (RGBVre1_Log_Read_data[1][length - 4] + ",");
            appendText += (Dll_Info + ",");
            appendText += (RGBVre1_Log_Read_data[1][length - 2] + ",");
            appendText += (DateTime.Now.Date.ToString().Substring(0, 10) + " " +  DateTime.Now.TimeOfDay.ToString() + Environment.NewLine);

            appendText += "Band_Gray,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1,Red,Green,Blue,Vreg1" + Environment.NewLine;
        }

        private void Merge_Text(ref string Text,RGB[, ,] RGBVre1_data)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Merge CSV Text", Color.Blue);

            for (int Band = 0; Band < 12; Band++)
            {
                for (int Gray = 0; Gray < 10; Gray++)
                {
                    for (int Sample = 0; Sample < 10; Sample++)
                    {
                        if (Band <= 8)
                        {
                            if (Gray == 0 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G255,";
                            else if (Gray == 1 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G189,";
                            else if (Gray == 2 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G134,";
                            else if (Gray == 3 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G91,";
                            else if (Gray == 4 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G60,";
                            else if (Gray == 5 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G38,";
                            else if (Gray == 6 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G23,";
                            else if (Gray == 7 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G13,";
                            else if (Gray == 8 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G6,";
                            else if (Gray == 9 && Sample == 0)
                                Text += "B" + Band.ToString() + "_G4,";
                            else
                            { }
                        }
                        else
                        {
                            if (Gray == 0 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G255,";
                            else if (Gray == 1 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G189,";
                            else if (Gray == 2 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G134,";
                            else if (Gray == 3 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G91,";
                            else if (Gray == 4 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G60,";
                            else if (Gray == 5 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G38,";
                            else if (Gray == 6 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G23,";
                            else if (Gray == 7 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G13,";
                            else if (Gray == 8 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G6,";
                            else if (Gray == 9 && Sample == 0)
                                Text += "A" + (Band - 9).ToString() + "_G4,";
                            else
                            { }
                        }
                            Text += RGBVre1_data[Band, Gray, Sample].R + "," + RGBVre1_data[Band, Gray, Sample].G + "," + RGBVre1_data[Band, Gray, Sample].B + ","
                                + RGBVre1_data[Band, Gray, Sample].Vreg1 + ",";
                    }
                    Text += Environment.NewLine;
                }
            }
        }

        private void Get_Update_RGBVre1_Log_data(string[][] Read_Data, RGB[, ,] RGBVre1_data)
        {
            //public RGB[,,] RGBVre1_Log_data = new RGB[12, 10, 10];
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
            int Sample = 0; //sample[0~9] --> 10ea
            int Sample_Term = 4;

            int Band = 0; //Band[0~11] --> 12ea  
            int Band_Term = 10;
         
            //Band0/Gray255/1st Sample
            for (Band = 0; Band < 12; Band++)
            {
                for (Sample = 0; Sample < 10; Sample++)
                {
                    //f1.GB_Status_AppendText_Nextline("Band/Sample = " + Band.ToString() + "/" + Sample.ToString(), Color.Red);
                    for (int Gray = 0; Gray < 10; Gray++)
                    {
                        RGBVre1_data[Band, Gray, Sample].R = Read_Data[(3 + Gray) + (Band_Term * Band)][1 + (Sample_Term * Sample)];
                        RGBVre1_data[Band, Gray, Sample].G = Read_Data[(3 + Gray) + (Band_Term * Band)][2 + (Sample_Term * Sample)];
                        RGBVre1_data[Band, Gray, Sample].B = Read_Data[(3 + Gray) + (Band_Term * Band)][3 + (Sample_Term * Sample)];
                        RGBVre1_data[Band, Gray, Sample].Vreg1 = Read_Data[(3 + Gray) + (Band_Term * Band)][4 + (Sample_Term * Sample)];
                        //f1.GB_Status_AppendText_Nextline(RGBVre1_Log_data[Band, Gray, Sample].R + "/" + RGBVre1_Log_data[Band, Gray, Sample].G + "/" + RGBVre1_Log_data[Band, Gray, Sample].B + "/" + RGBVre1_Log_data[Band, Gray, Sample].Vreg1, Color.Blue);
                        RGBVre1_data[Band, Gray, Sample].Int_Update_From_String();
                        
                    }
                }
            }
        }

        private void Read_Data_From_Log(string Path,ref string [][]Read_Data)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
            Read_Data = File.ReadLines(Path).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]4
        }


        private void Shift_and_Get_New_Data(RGB[, ,] RGBVre1_data, RGB[,] All_band_gray_Gamma)
        {
            //public RGB[,] All_band_gray_Gamma = new RGB[12, 10]; //12ea Bands , 10ea Gray-points
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Shift Data in and Write New Gamma/Vreg1 into CSV", Color.Blue);
            for (int Band = 0; Band < 12; Band++) //Band[0~11] --> 12ea  
            {
                for (int Sample = 0; Sample < 10; Sample++) //sample[0~9] --> 10ea
                {
                    for (int Gray = 0; Gray < 10; Gray++) //Gray[0~9] --> 10ea
                    {
                        if (Sample < 9)
                        {
                            RGBVre1_data[Band, Gray, Sample].Equal_Value(RGBVre1_data[Band, Gray, Sample + 1]);
                        }
                        else
                        {
                            RGBVre1_data[Band, Gray, Sample].Equal_Value(All_band_gray_Gamma[Band, Gray]);
                        }
                    }
                }
            }
        }

        private void Shift_and_Get_New_Offset_Data(RGB[, ,] RGBVre1_Offset_data, RGB[,] All_band_gray_Gamma)
        {
            //public RGB[,] All_band_gray_Gamma = new RGB[12, 10]; //12ea Bands , 10ea Gray-points
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Shift Offset Data in and Write New Gamma/Vreg1 into CSV", Color.Blue);
            for (int Band = 1; Band < 12; Band++) //Band[0~11] --> 12ea  
            {
                for (int Sample = 0; Sample < 10; Sample++) //sample[0~9] --> 10ea
                {
                    for (int Gray = 0; Gray < 10; Gray++) //Gray[0~9] --> 10ea
                    {
                        if (Sample < 9)
                        {
                            RGBVre1_Offset_data[Band, Gray, Sample].Equal_Value(RGBVre1_Offset_data[Band, Gray, Sample + 1]);
                        }
                        else
                        {
                            RGBVre1_Offset_data[Band, Gray, Sample].Equal_Value(All_band_gray_Gamma[Band, Gray] - All_band_gray_Gamma[Band-1, Gray]);
                        }
                    }
                }
            }
        }

        private void Get_All_Band_Vreg1(RGB[,] All_band_gray_Gamma)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Get All Vreg1", Color.Blue);

            All_band_gray_Gamma[0, 0].int_Vreg1 = Vreg1_Read2(0, true, true);
            All_band_gray_Gamma[1, 0].int_Vreg1 = Vreg1_Read2(1, true, false);
            All_band_gray_Gamma[2, 0].int_Vreg1 = Vreg1_Read2(2, true, false);
            All_band_gray_Gamma[3, 0].int_Vreg1 = Vreg1_Read2(3, true, false);
            All_band_gray_Gamma[4, 0].int_Vreg1 = Vreg1_Read2(4, true, false);
            All_band_gray_Gamma[5, 0].int_Vreg1 = Vreg1_Read2(5, true, false);
            All_band_gray_Gamma[6, 0].int_Vreg1 = Vreg1_Read2(6, true, false);
            All_band_gray_Gamma[7, 0].int_Vreg1 = Vreg1_Read2(7, true, false);

            All_band_gray_Gamma[8, 0].int_Vreg1 = Vreg1_Read2(8, true, true);
            All_band_gray_Gamma[9, 0].int_Vreg1 = Vreg1_Read2(11, true, false);
            All_band_gray_Gamma[10, 0].int_Vreg1 = Vreg1_Read2(12, true, false);
            All_band_gray_Gamma[11, 0].int_Vreg1 = Vreg1_Read2(13, true, false);
        }

        private void radioButton_More_Func_Form_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_More_Func_Form.Checked == true)
            {
                //this.Size = new Size(1319, 597);
                this.Size = new Size(Form_width, Form_height);
            }
        }

        private void radioButton_Normal_Form_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Normal_Form.Checked == true)
            {
                this.Size = new Size(Form_width * 813 / 1319, Form_height);
            }
           
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_Get_Init_Gamma_Vreg1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Get_Init_Gamma_Vreg1.Checked)
            {
                checkBox_Get_Offset_Gamma_Vreg1.Checked = false;
                checkBox_Save_Offset_Gamma_to_Log.Checked = false;
            }
        }

        private void checkBox_Save_Init_Gamma_to_Log_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Save_Init_Gamma_to_Log.Checked)
            {
                checkBox_Get_Offset_Gamma_Vreg1.Checked = false;
                checkBox_Save_Offset_Gamma_to_Log.Checked = false;
            }
        }

        private void checkBox_Get_Offset_Gamma_Vreg1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Get_Offset_Gamma_Vreg1.Checked)
            {
                checkBox_Get_Init_Gamma_Vreg1.Checked = false;
                checkBox_Save_Init_Gamma_to_Log.Checked = false;
            }
        }

        private void checkBox_Save_Offset_Gamma_to_Log_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Save_Offset_Gamma_to_Log.Checked)
            {
                checkBox_Get_Init_Gamma_Vreg1.Checked = false;
                checkBox_Save_Init_Gamma_to_Log.Checked = false;
            }
        }

        private void checkBox_Ave_Measure_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Ave_Measure.Checked)
            {
                checkBox_Ave_Apply_Ratio.Enabled = true;
            }
            else
            {
                checkBox_Ave_Apply_Ratio.Checked = false;
                checkBox_Ave_Apply_Ratio.Enabled = false;
            }
        }

        private void checkBox_Copy_Final_G255_to_Others_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Copy_Final_G255_to_Others.Checked)
            {
                checkBox_G189_Offset_From_G255.Enabled = true;
            }
            else
            {
                checkBox_G189_Offset_From_G255.Checked = false; 
                checkBox_G189_Offset_From_G255.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_Gray_Points_Measure_Click(object sender, EventArgs e)
        {
            int measure_delay = Convert.ToInt16(textBox_Gray_Points_Measure_Delay.Text);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            f1.GB_Status_AppendText_Nextline("Gray 9 Points Measure Start", Color.Blue);
            if (checkBox_Gray_Points_Measure_AOD.Checked)
            {
                f1.AOD_On();
                Thread.Sleep(measure_delay * 3);
            }
            for (int gray = 0; gray < 10; gray++)
            {
                DP116_GrayPoints_Pattern_Setting(checkBox_Gray_Points_Measure_AOD.Checked, gray);
                Thread.Sleep(measure_delay);
                f1.CA_Measure_BT_Click();
            }
            if (checkBox_Gray_Points_Measure_AOD.Checked) f1.AOD_Off();
            f1.GB_Status_AppendText_Nextline("Gray 9 Points Measure End", Color.Blue);
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }


        private void DP116_Measure_Average(ref double Measured_X, ref double Measured_Y, ref double Measured_Lv, ref int total_average_loop_count
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
