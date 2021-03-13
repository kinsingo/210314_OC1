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
    public partial class Second_Model_Option_Form : Form
    {
        //Set2 Diff(Between Set1 and Set2)Delta L Spec
        public double[,] OC_Mode2_Diff_Delta_L_Spec = new double[11, 8];//for Dual (HBM + Normal : 11ea bands)
        public double[,] OC_Mode2_Diff_Delta_UV_Spec = new double[11, 8];//for Dual (HBM + Normal : 11ea bands)

        public RGB[,] All_band_gray_Gamma = new RGB[14, 8]; //14ea Bands , 8ea Gray-points
        bool Optic_Compensation_Stop = false;
        bool Optic_Compensation_Succeed = false;

        RGB Gamma;
        XYLv Measure;
        XYLv Target;
        XYLv Limit;
        XYLv Extension;
        RGB Prev_Gamma;
        RGB Diff_Gamma;

        //Vreg1 Related
        string[] B1_Vreg1_Gamma_Set1 = new string[17];
        string[] B1_Vreg1_Gamma_Set2 = new string[17];
        string[] B1_Vreg1_Gamma_Set3 = new string[17];

        bool Vreg1_Need_To_Be_Updated = false;
        int Vreg1;
        int Prev_Vreg1;
        int Diff_Vreg1;
        
        //RGB Infinite_Loop_Detect
        bool Infinite = false;
        int Infinite_Count;
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
        const int Vreg1_Register_Limit = 2047;
        bool Gamma_Out_Of_Register_Limit = false;
        bool Within_Spec_Limit = false;

        //RGB Gamma Boolen
        bool[] RGB_Need_To_Change = new bool[3];

        //First size
        int Form_height;
        int Form_width;

        //Single or Dual
        bool Single_Or_Dual_Single_Is_True;

        //
        string R_AM1_Hex;
        string G_AM1_Hex;
        string B_AM1_Hex;

        private static Second_Model_Option_Form Instance;
        public static Second_Model_Option_Form getInstance()
        {
            if (Instance == null)
                Instance = new Second_Model_Option_Form();

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
        private Second_Model_Option_Form()
        {
            InitializeComponent();
        }

        private void Second_Model_Option_Form_Load(object sender, EventArgs e)
        {
            BackColor = f1().current_model.Get_Back_Ground_Color();
        }

        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
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

       
        public void DP150_B0_DBV_Send()
        {
            button_B1_DBV_Send.PerformClick();
        }

        public void DP150_B1_DBV_Send()
        {
            button_B2_DBV_Send.PerformClick();
        }

        public void DP150_B2_DBV_Send()
        {
            button_B3_DBV_Send.PerformClick();
        }

        public void DP150_B3_DBV_Send()
        {
            button_B4_DBV_Send.PerformClick();
        }

        public void DP150_B4_DBV_Send()
        {
            button_B5_DBV_Send.PerformClick();
        }

        public void DP150_B5_DBV_Send()
        {
            button_B6_DBV_Send.PerformClick();
        }

        public void DP150_B6_DBV_Send()
        {
            button_B7_DBV_Send.PerformClick();
        }

        public void DP150_B7_DBV_Send()
        {
            button_B8_DBV_Send.PerformClick();
        }

        public void DP150_B8_DBV_Send()
        {
            button_B9_DBV_Send.PerformClick();
        }

        public void DP150_B9_DBV_Send()
        {
            button_B10_DBV_Send.PerformClick();
        }

        public void DP150_B10_DBV_Send()
        {
            button_B11_DBV_Send.PerformClick();
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

        private void button_B11_DBV_Send_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.DBV_Setting(textBox_B11_DBV_Setting.Text);
            f1.GB_Status_AppendText_Nextline("Band10 DBV Setting", System.Drawing.Color.Black);
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

        public void button_Read_DP150_DBV_Setting_Perform_Click()
        {
            button_Read_DP116_DBV_Setting.PerformClick();
        }

        public void button_Read_DP150_DBV()
        {
            button_Read_DP116_DBV_Setting.PerformClick();
        }

        private void button_Read_DP116_DBV_Setting_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                f1.MX_OTP_Read(0,15, "B1");
                Thread.Sleep(200);
                
                textBox_B1_DBV_Setting.Text = "7FF";
                textBox_B2_DBV_Setting.Text = f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
                textBox_B3_DBV_Setting.Text = f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString();
                textBox_B4_DBV_Setting.Text = f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString();
                textBox_B5_DBV_Setting.Text = f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString();
                textBox_B6_DBV_Setting.Text = f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString();
                textBox_B7_DBV_Setting.Text = f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString();
                textBox_B8_DBV_Setting.Text = f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString();
                textBox_B9_DBV_Setting.Text = f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString();
                textBox_B10_DBV_Setting.Text = f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString();
                textBox_B11_DBV_Setting.Text = f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString();
                
                //Read AOD1~3 DBV
                f1.MX_OTP_Read(0,8, "B2");
                Thread.Sleep(200);
                textBox_AOD1_DBV_Setting.Text = "7FF";
                textBox_AOD2_DBV_Setting.Text = f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[4].Cells[1].Value.ToString();
                textBox_AOD3_DBV_Setting.Text = f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[5].Cells[1].Value.ToString();
                f1.GB_Status_AppendText_Nextline("DP150 DBV value was loaded from register", System.Drawing.Color.Black);
                
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
            textBox_Vreg1_B10.Text = string.Empty;


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
            textBox_Vreg1_B9_2.Text = string.Empty;
            textBox_Vreg1_B10_2.Text = string.Empty;

            textBox_Vreg1_B0_3.Text = string.Empty;
            textBox_Vreg1_B1_3.Text = string.Empty;
            textBox_Vreg1_B2_3.Text = string.Empty;
            textBox_Vreg1_B3_3.Text = string.Empty;
            textBox_Vreg1_B4_3.Text = string.Empty;
            textBox_Vreg1_B5_3.Text = string.Empty;
            textBox_Vreg1_B6_3.Text = string.Empty;
            textBox_Vreg1_B7_3.Text = string.Empty;
            textBox_Vreg1_B8_3.Text = string.Empty;
            textBox_Vreg1_B9_3.Text = string.Empty;
            textBox_Vreg1_B10_3.Text = string.Empty;
        }
        private int DP150_Get_Normal_Initial_Vreg1_Set123(int band, int Gamma_Set)
        {
            if (Gamma_Set == 1)
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
                    case 10:
                        return Convert.ToInt16(textBox_Vreg1_B10.Text);
                    case 11:
                        return Convert.ToInt16(textBox_Vreg1_A0.Text);
                    case 12:
                        return Convert.ToInt16(textBox_Vreg1_A1.Text);
                    case 13:
                        return Convert.ToInt16(textBox_Vreg1_A2.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary(Not a Normal Band)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            else if (Gamma_Set == 2)
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt16(textBox_Vreg1_B0_2.Text);
                    case 1:
                        return Convert.ToInt16(textBox_Vreg1_B1_2.Text);
                    case 2:
                        return Convert.ToInt16(textBox_Vreg1_B2_2.Text);
                    case 3:
                        return Convert.ToInt16(textBox_Vreg1_B3_2.Text);
                    case 4:
                        return Convert.ToInt16(textBox_Vreg1_B4_2.Text);
                    case 5:
                        return Convert.ToInt16(textBox_Vreg1_B5_2.Text);
                    case 6:
                        return Convert.ToInt16(textBox_Vreg1_B6_2.Text);
                    case 7:
                        return Convert.ToInt16(textBox_Vreg1_B7_2.Text);
                    case 8:
                        return Convert.ToInt16(textBox_Vreg1_B8_2.Text);
                    case 9:
                        return Convert.ToInt16(textBox_Vreg1_B9_2.Text);
                    case 10:
                        return Convert.ToInt16(textBox_Vreg1_B10_2.Text);
                    case 11:
                        return Convert.ToInt16(textBox_Vreg1_A0.Text);
                    case 12:
                        return Convert.ToInt16(textBox_Vreg1_A1.Text);
                    case 13:
                        return Convert.ToInt16(textBox_Vreg1_A2.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary(Not a Normal Band)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            else if (Gamma_Set == 3)
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt16(textBox_Vreg1_B0_3.Text);
                    case 1:
                        return Convert.ToInt16(textBox_Vreg1_B1_3.Text);
                    case 2:
                        return Convert.ToInt16(textBox_Vreg1_B2_3.Text);
                    case 3:
                        return Convert.ToInt16(textBox_Vreg1_B3_3.Text);
                    case 4:
                        return Convert.ToInt16(textBox_Vreg1_B4_3.Text);
                    case 5:
                        return Convert.ToInt16(textBox_Vreg1_B5_3.Text);
                    case 6:
                        return Convert.ToInt16(textBox_Vreg1_B6_3.Text);
                    case 7:
                        return Convert.ToInt16(textBox_Vreg1_B7_3.Text);
                    case 8:
                        return Convert.ToInt16(textBox_Vreg1_B8_3.Text);
                    case 9:
                        return Convert.ToInt16(textBox_Vreg1_B9_3.Text);
                    case 10:
                        return Convert.ToInt16(textBox_Vreg1_B10_3.Text);
                    case 11:
                        return Convert.ToInt16(textBox_Vreg1_A0.Text);
                    case 12:
                        return Convert.ToInt16(textBox_Vreg1_A1.Text);
                    case 13:
                        return Convert.ToInt16(textBox_Vreg1_A2.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary(Not a Normal Band)");
                        Optic_Compensation_Stop = true;
                        return 0;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Gamma Set should be 1,2 or 3");
                return 0;
            }

        }

        private int DP150_Get_Normal_Initial_Vreg1(int band,bool Gamma_Set1)
        {
            if(Gamma_Set1)
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
                    case 10:
                        return Convert.ToInt16(textBox_Vreg1_B10.Text);
                    case 11:
                        return Convert.ToInt16(textBox_Vreg1_A0.Text);
                    case 12:
                        return Convert.ToInt16(textBox_Vreg1_A1.Text);
                     case 13:
                        return Convert.ToInt16(textBox_Vreg1_A2.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary(Not a Normal Band)");
                        Optic_Compensation_Stop = true;
                        return 0;
                 } 
            }
            else
            {
                switch (band)
                {
                    case 0:
                        return Convert.ToInt16(textBox_Vreg1_B0_2.Text);
                    case 1:
                        return Convert.ToInt16(textBox_Vreg1_B1_2.Text);
                    case 2:
                        return Convert.ToInt16(textBox_Vreg1_B2_2.Text);
                    case 3:
                        return Convert.ToInt16(textBox_Vreg1_B3_2.Text);
                    case 4:
                        return Convert.ToInt16(textBox_Vreg1_B4_2.Text);
                    case 5:
                        return Convert.ToInt16(textBox_Vreg1_B5_2.Text);
                    case 6:
                        return Convert.ToInt16(textBox_Vreg1_B6_2.Text);
                    case 7:
                        return Convert.ToInt16(textBox_Vreg1_B7_2.Text);
                    case 8:
                        return Convert.ToInt16(textBox_Vreg1_B8_2.Text);
                    case 9:
                        return Convert.ToInt16(textBox_Vreg1_B9_2.Text);
                    case 10:
                        return Convert.ToInt16(textBox_Vreg1_B10_2.Text);
                    case 11:
                        return Convert.ToInt16(textBox_Vreg1_A0.Text); 
                    case 12:
                        return Convert.ToInt16(textBox_Vreg1_A1.Text);
                    case 13:
                        return Convert.ToInt16(textBox_Vreg1_A2.Text);
                    default:
                        System.Windows.Forms.MessageBox.Show("Band is out of boundary(Not a Normal Band)");
                        Optic_Compensation_Stop = true;
                        return 0;
                } 
            }
        }


        private void button_Vreg1_Read_Click(object sender, EventArgs e)
        {
            TextBox_Vreg1_Clear();
            Application.DoEvents();

            //string[] B1_Vreg1_Gamma_Set1 = new string[15];
            //string[] B1_Vreg1_Gamma_Set2 = new string[15];

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(43,17,"B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set1[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0.Text = Convert.ToInt16((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B1.Text = Convert.ToInt16((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B2.Text = Convert.ToInt16((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B3.Text = Convert.ToInt16((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B4.Text = Convert.ToInt16((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B5.Text = Convert.ToInt16((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B6.Text = Convert.ToInt16((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B7.Text = Convert.ToInt16((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B8.Text = Convert.ToInt16((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B9.Text = Convert.ToInt16((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10.Text = Convert.ToInt16((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();

            f1.MX_OTP_Read(60, 17, "B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set2[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B1_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B2_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B3_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B4_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B5_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B6_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B7_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B8_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_B9_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10_2.Text = Convert.ToInt16((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();
 
            f1.MX_OTP_Read(77, 17, "B1");
            Thread.Sleep(200);
            for (int i = 0; i < 17; i++) B1_Vreg1_Gamma_Set3[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBox_Vreg1_B0_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B1_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[7].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B2_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[8].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B3_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[9].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B4_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[10].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B5_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[2].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[11].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B6_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[12].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B7_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[3].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[13].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B8_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[14].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B9_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[4].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[15].Cells[1].Value.ToString()), 16).ToString();
            textBox_Vreg1_B10_3.Text = Convert.ToInt16((f1.dataGridView1.Rows[5].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[16].Cells[1].Value.ToString()), 16).ToString();

            f1.MX_OTP_Read(2, 7, "B2");
            Thread.Sleep(100);
            textBox_Vreg1_A0.Text = Convert.ToInt16((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(1, 1) + f1.dataGridView1.Rows[4].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_A1.Text = Convert.ToInt16((f1.dataGridView1.Rows[0].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[5].Cells[1].Value.ToString()),16).ToString();
            textBox_Vreg1_A2.Text = Convert.ToInt16((f1.dataGridView1.Rows[1].Cells[1].Value.ToString().Remove(0, 1) + f1.dataGridView1.Rows[6].Cells[1].Value.ToString()),16).ToString();
        }

        private void textBox_Vreg1_B5_2_TextChanged(object sender, EventArgs e)
        {

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
            checkBox_Band13.Checked = true;
            checkBox_Band14.Checked = true;
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
            checkBox_Band13.Checked = false;
            checkBox_Band14.Checked = false;
        }

        private void button_Read_ELVSS_Vinit_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //Normal ELVSS Read
            f1.MX_OTP_Read(150, 11, "E0");
            string[] hex_ELVSS = new string[11];
            double[] dec_ELVSS = new double[11];
            double[] ELVSS = new double[11];
            for (int i = 0; i < 11; i++)
            {
                hex_ELVSS[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS[i] = Convert.ToDouble(Convert.ToInt16(hex_ELVSS[i], 16));
                ELVSS[i] = ((dec_ELVSS[i] - 30) / 10.0) - 3.5;
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
            textBox_ELVSS_B10.Text = ELVSS[10].ToString() + "v (" + hex_ELVSS[10] + "h)";
            
            //AOD ELVSS Read
            f1.MX_OTP_Read(194, 3, "E0");
            hex_ELVSS = new string[3];
            dec_ELVSS = new double[3];
            ELVSS = new double[3];
            for (int i = 0; i < 3; i++)
            {
                hex_ELVSS[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_ELVSS[i] = Convert.ToDouble(Convert.ToInt16(hex_ELVSS[i], 16));
                ELVSS[i] = ((dec_ELVSS[i] - 30) / 10.0) - 3.5;
            }
            textBox_ELVSS_A0.Text = ELVSS[0].ToString() + "v (" + hex_ELVSS[0] + "h)";
            textBox_ELVSS_A1.Text = ELVSS[1].ToString() + "v (" + hex_ELVSS[1] + "h)";
            textBox_ELVSS_A2.Text = ELVSS[2].ToString() + "v (" + hex_ELVSS[2] + "h)";

            //Vinit Read
            f1.MX_OTP_Read(73, 12, "E4");
            string[] hex_Vinit = new string[12];
            double[] dec_Vinit = new double[12];
            double[] Vinit = new double[12];
            for (int i = 0; i < 12; i++)
            {
                hex_Vinit[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                dec_Vinit[i] = Convert.ToDouble(Convert.ToInt16(hex_Vinit[i], 16));
                Vinit[i] = - ((dec_Vinit[i] - 30) / 10.0) - 2.8;
            }
            textBox_Vinit_AOD.Text = Vinit[0].ToString() + "v (" + hex_Vinit[0] + "h)";
            textBox_Vinit_B0.Text = Vinit[1].ToString() + "v (" + hex_Vinit[1] + "h)";
            textBox_Vinit_B1.Text = Vinit[2].ToString() + "v (" + hex_Vinit[2] + "h)";
            textBox_Vinit_B2.Text = Vinit[3].ToString() + "v (" + hex_Vinit[3] + "h)";
            textBox_Vinit_B3.Text = Vinit[4].ToString() + "v (" + hex_Vinit[4] + "h)";
            textBox_Vinit_B4.Text = Vinit[5].ToString() + "v (" + hex_Vinit[5] + "h)";
            textBox_Vinit_B5.Text = Vinit[6].ToString() + "v (" + hex_Vinit[6] + "h)";
            textBox_Vinit_B6.Text = Vinit[7].ToString() + "v (" + hex_Vinit[7] + "h)";
            textBox_Vinit_B7.Text = Vinit[8].ToString() + "v (" + hex_Vinit[8] + "h)";
            textBox_Vinit_B8.Text = Vinit[9].ToString() + "v (" + hex_Vinit[9] + "h)";
            textBox_Vinit_B9.Text = Vinit[10].ToString() + "v (" + hex_Vinit[10] + "h)";
            textBox_Vinit_B10.Text = Vinit[11].ToString() + "v (" + hex_Vinit[11] + "h)";
        }

        private void Textbox_ELVSS_Clear()
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
            textBox_ELVSS_B9.Text = string.Empty;
            textBox_ELVSS_B10.Text = string.Empty;

            textBox_ELVSS_A0.Text = string.Empty;
            textBox_ELVSS_A1.Text = string.Empty;
            textBox_ELVSS_A2.Text = string.Empty;
        }

        public void DP150_DBV_Setting(int band)
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
                case 10:
                    button_B11_DBV_Send.PerformClick();
                    break;
                case 11://AOD3
                    button_A1_DBV_Send.PerformClick();
                    break;
                case 12://AOD3
                    button_A2_DBV_Send.PerformClick();
                    break;
                case 13://AOD3
                    button_A3_DBV_Send.PerformClick();
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                    break;
            }
            Thread.Sleep(100);
        }




        private void DP150_SET_ELVSS(int Band, double ELVSS, string[] TI_ELVSS, string[] STMP03_ELVSS)
        {
            int Current_Dec = Convert.ToInt16(10 * (ELVSS + 3.5) + 30);
            TI_ELVSS[Band] = Current_Dec.ToString("X2");

            Current_Dec -= 4;
            STMP03_ELVSS[Band] = Current_Dec.ToString("X2");
        }

        private void DP150_ELVSS_Compensation()
        {
            DP150_DBV_Setting(0);//HBM DBV Setting
            
            DP150_Pattern_Setting(0, 0, Single_Or_Dual_Single_Is_True);//HBM Gray255 Pattern Setting
            
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
                string[] DP150_Vinit = new string[9];

                for (int i = 0; i < 9; i++) TI_ELVSS[i] = STMP03_ELVSS[i] = "00";

                for (ELVSS = First_ELVSS; ELVSS < -1.4; ELVSS += 0.1)
                {
                    if (ELVSS == First_ELVSS)
                    {
                        DP150_SET_ELVSS(0, First_ELVSS, TI_ELVSS, STMP03_ELVSS);
                        f1.DP150_Long_Packet_CMD_Send(150, 9, "E0", TI_ELVSS);//Set 3 (TI)
                        Thread.Sleep(20);
                        f1.CA_Measure_button_Perform_Click(ref First_Measure.double_X, ref First_Measure.double_Y, ref First_Measure.double_Lv);
                    }
                    else
                    {
                        DP150_SET_ELVSS(0, ELVSS, TI_ELVSS, STMP03_ELVSS);
                        f1.DP150_Long_Packet_CMD_Send(150, 9, "E0", TI_ELVSS);//Set 3 (TI)
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
                

                //ELVSS 적용
                f1.GB_Status_AppendText_Nextline("HBM ELVSS : " + ELVSS.ToString(), Color.Black);

                DP150_SET_ELVSS(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                DP150_SET_ELVSS(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                DP150_SET_ELVSS(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                DP150_SET_ELVSS(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                DP150_SET_ELVSS(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                DP150_SET_ELVSS(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                DP150_SET_ELVSS(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                DP150_SET_ELVSS(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset.Text), TI_ELVSS, STMP03_ELVSS);
                DP150_SET_ELVSS(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset.Text), TI_ELVSS, STMP03_ELVSS);

                //Vinit 적용
                DP150_Normal_Vinit_Setting(0, ELVSS + Convert.ToDouble(ELVSS_B0_Offset.Text) + Convert.ToDouble(Vinit_Offset_B0.Text), DP150_Vinit);
                DP150_Normal_Vinit_Setting(1, ELVSS + Convert.ToDouble(ELVSS_B1_Offset.Text) + Convert.ToDouble(Vinit_Offset_B1.Text), DP150_Vinit);
                DP150_Normal_Vinit_Setting(2, ELVSS + Convert.ToDouble(ELVSS_B2_Offset.Text) + Convert.ToDouble(Vinit_Offset_B2.Text), DP150_Vinit);
                DP150_Normal_Vinit_Setting(3, ELVSS + Convert.ToDouble(ELVSS_B3_Offset.Text) + Convert.ToDouble(Vinit_Offset_B3.Text), DP150_Vinit);
                DP150_Normal_Vinit_Setting(4, ELVSS + Convert.ToDouble(ELVSS_B4_Offset.Text) + Convert.ToDouble(Vinit_Offset_B4.Text), DP150_Vinit);
                DP150_Normal_Vinit_Setting(5, ELVSS + Convert.ToDouble(ELVSS_B5_Offset.Text) + Convert.ToDouble(Vinit_Offset_B5.Text), DP150_Vinit);
                DP150_Normal_Vinit_Setting(6, ELVSS + Convert.ToDouble(ELVSS_B6_Offset.Text) + Convert.ToDouble(Vinit_Offset_B6.Text), DP150_Vinit);
                DP150_Normal_Vinit_Setting(7, ELVSS + Convert.ToDouble(ELVSS_B7_Offset.Text) + Convert.ToDouble(Vinit_Offset_B7.Text), DP150_Vinit);
                DP150_Normal_Vinit_Setting(8, ELVSS + Convert.ToDouble(ELVSS_B8_Offset.Text) + Convert.ToDouble(Vinit_Offset_B8.Text), DP150_Vinit);

                f1.DP150_Long_Packet_CMD_Send(128, 9, "E0", STMP03_ELVSS);//ELVSS Set 1 (STMP)
                f1.DP150_Long_Packet_CMD_Send(139, 9, "E0", STMP03_ELVSS);//ELVSS Set 2 (STMP)
                f1.DP150_Long_Packet_CMD_Send(150, 9, "E0", TI_ELVSS);//ELVSS Set 3 (TI)
                f1.DP150_Long_Packet_CMD_Send(161, 9, "E0", TI_ELVSS);//ELVSS Set 4 (TI)
                f1.DP150_Long_Packet_CMD_Send(74, 9, "E4", DP150_Vinit);//Vinit Set1
                f1.DP150_Long_Packet_CMD_Send(118, 9, "E4", DP150_Vinit);//Vinit Set2
                Thread.Sleep(20);                
            }
            button_Read_ELVSS_Vinit.PerformClick();
        }

        private void Get_Param(int gray, ref RGB Gamma, ref XYLv Target, ref XYLv Limit, ref XYLv Extension)
        {
            DP150_Single_Engineerig_Mornitoring_Mode Single_form_engineer = (DP150_Single_Engineerig_Mornitoring_Mode)Application.OpenForms["DP150_Single_Engineerig_Mornitoring_Mode"];
            Single_form_engineer.Get_OC_Param_DP116(gray, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B
                , ref Target.double_X, ref Target.double_Y, ref Target.double_Lv, ref Limit.double_X, ref Limit.double_Y, ref Limit.double_Lv, ref Extension.double_X, ref Extension.double_Y);
        }

        private void Dual_Mode_Get_Param(int gray, bool Condition)
        {
            //Conditon = true : Condition 1
            //Conditon = false : Condition 2
            DP150_Dual_Engineering_Mornitoring_Mode DP150_form_dual_engineer = (DP150_Dual_Engineering_Mornitoring_Mode)Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
            DP150_form_dual_engineer.Get_OC_Param_DP150(gray, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B
                , ref Target.double_X, ref Target.double_Y, ref Target.double_Lv, ref Limit.double_X, ref Limit.double_Y, ref Limit.double_Lv, ref Extension.double_X, ref Extension.double_Y, Condition);
            Gamma.String_Update_From_int();
        }

        private bool DP150_Black_Compensation(double Vreg1_REF2047_Margin,double Vreg1_REF2047_Resolution,double Vreg1_REF2047_Limit_Lv)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("DP150 Black(Vreg1 REF2047) Compensation Start", Color.Black);
            f1.PTN_update(0, 0, 0);
            Thread.Sleep(400);

            int REF_2047_Max = 127;
            int Dec_REF2047 = REF_2047_Max; //REF2047(127 max) = 7v , REF2047(0 min) = 5.76v
            f1.DP150_One_Param_CMD_Send(21, "B1", Dec_REF2047.ToString("X2"));
           
            while (Dec_REF2047 > 0)
            {
                f1.GB_Status_AppendText_Nextline("Dec_REF2047 : " + Dec_REF2047.ToString(), Color.Blue);

                if (Optic_Compensation_Stop) return true;//Optic_Compensation_Stop = true;

                if (Dec_REF2047 == 0)
                {
                    Dec_REF2047 += Convert.ToInt16(Vreg1_REF2047_Margin / Vreg1_REF2047_Resolution);
                    f1.DP150_One_Param_CMD_Send(21, "B1", Dec_REF2047.ToString("X2"));
                    f1.GB_Status_AppendText_Nextline("Black(REF2047) Compensation OK (Case 1)", Color.Green);
                    return false;//Optic_Compensation_Stop = false;
                }
                else
                {
                    Thread.Sleep(20);//Add on 190820
                    f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv);
                    f1.GB_Status_AppendText_Nextline("Measure.double_Lv / Vreg1_REF2047_Limit_Lv : " + Measure.double_Lv.ToString() + "/" + Vreg1_REF2047_Limit_Lv.ToString(), Color.Black);

                    if (Measure.double_Lv < Vreg1_REF2047_Limit_Lv)
                    {
                        Dec_REF2047--;
                        f1.DP150_One_Param_CMD_Send(21, "B1", Dec_REF2047.ToString("X2"));
                        continue;
                    }
                    else
                    {
                        Dec_REF2047 += Convert.ToInt16(Vreg1_REF2047_Margin / Vreg1_REF2047_Resolution);
                        if (Dec_REF2047 > REF_2047_Max)
                        {
                            f1.GB_Status_AppendText_Nextline("Black(REF2047) Compensation Fail (Black Margin Is Not Enough)", Color.Red);
                            return true;//Optic_Compensation_Stop = true;
                        }
                        else
                        {
                            f1.DP150_One_Param_CMD_Send(21, "B1", Dec_REF2047.ToString("X2"));
                            f1.GB_Status_AppendText_Nextline("Black(REF2047) Compensation OK (Case 2)", Color.Green);
                            return false;//Optic_Compensation_Stop = false;
                        }
                    }
                }
            }
            return false;//Optic_Compensation_Stop = false;
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



         private void DP150_Update_and_Send_Vreg1(int Vreg1_int, int band, bool Gamma_Set1)
         {
             Form1 f1 = (Form1)Application.OpenForms["Form1"];

             string Total = Vreg1_int.ToString("X3");

             if (Gamma_Set1)
             {
                 if (band % 2 == 0) B1_Vreg1_Gamma_Set1[band / 2] = Total[0].ToString() + (Convert.ToInt16(B1_Vreg1_Gamma_Set1[band / 2], 16) & 0x0F).ToString("X");
                 else B1_Vreg1_Gamma_Set1[band / 2] = ((Convert.ToInt16(B1_Vreg1_Gamma_Set1[band / 2], 16) & 0xF0)>>4).ToString("X") + Total[0].ToString();
                 
                 B1_Vreg1_Gamma_Set1[6 + band] = Total[1].ToString() + Total[2].ToString();
                 f1.DP150_Long_Packet_CMD_Send(43,17,"B1",B1_Vreg1_Gamma_Set1);
             }
             else
             {
                 if (band % 2 == 0) B1_Vreg1_Gamma_Set2[band / 2] = Total[0].ToString() + (Convert.ToInt16(B1_Vreg1_Gamma_Set2[band / 2], 16) & 0x0F).ToString("X");
                 else B1_Vreg1_Gamma_Set2[band / 2] = ((Convert.ToInt16(B1_Vreg1_Gamma_Set2[band / 2], 16) & 0xF0)>>4).ToString("X") + Total[0].ToString();

                 B1_Vreg1_Gamma_Set2[6 + band] = Total[1].ToString() + Total[2].ToString();
                 f1.DP150_Long_Packet_CMD_Send(60,17,"B1",B1_Vreg1_Gamma_Set2);
             }

             if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Vreg1 Is Applied", Color.Black);
         }


         private void DP150_Update_and_Send_Vreg1_Set123(int Vreg1_int, int band, int Gamma_Set)
         {
             Form1 f1 = (Form1)Application.OpenForms["Form1"];

             string Total = Vreg1_int.ToString("X3");

             if (Gamma_Set == 1)
             {
                 if (band % 2 == 0) B1_Vreg1_Gamma_Set1[band / 2] = Total[0].ToString() + (Convert.ToInt16(B1_Vreg1_Gamma_Set1[band / 2], 16) & 0x0F).ToString("X");
                 else B1_Vreg1_Gamma_Set1[band / 2] = ((Convert.ToInt16(B1_Vreg1_Gamma_Set1[band / 2], 16) & 0xF0) >> 4).ToString("X") + Total[0].ToString();

                 B1_Vreg1_Gamma_Set1[6 + band] = Total[1].ToString() + Total[2].ToString();
                 f1.DP150_Long_Packet_CMD_Send(43, 17, "B1", B1_Vreg1_Gamma_Set1);
             }
             else if (Gamma_Set == 2)
             {
                 if (band % 2 == 0) B1_Vreg1_Gamma_Set2[band / 2] = Total[0].ToString() + (Convert.ToInt16(B1_Vreg1_Gamma_Set2[band / 2], 16) & 0x0F).ToString("X");
                 else B1_Vreg1_Gamma_Set2[band / 2] = ((Convert.ToInt16(B1_Vreg1_Gamma_Set2[band / 2], 16) & 0xF0) >> 4).ToString("X") + Total[0].ToString();

                 B1_Vreg1_Gamma_Set2[6 + band] = Total[1].ToString() + Total[2].ToString();
                 f1.DP150_Long_Packet_CMD_Send(60, 17, "B1", B1_Vreg1_Gamma_Set2);
             }
             else if (Gamma_Set == 3)
             {
                 if (band % 2 == 0) B1_Vreg1_Gamma_Set3[band / 2] = Total[0].ToString() + (Convert.ToInt16(B1_Vreg1_Gamma_Set3[band / 2], 16) & 0x0F).ToString("X");
                 else B1_Vreg1_Gamma_Set3[band / 2] = ((Convert.ToInt16(B1_Vreg1_Gamma_Set3[band / 2], 16) & 0xF0) >> 4).ToString("X") + Total[0].ToString();

                 B1_Vreg1_Gamma_Set3[6 + band] = Total[1].ToString() + Total[2].ToString();
                 f1.DP150_Long_Packet_CMD_Send(77, 17, "B1", B1_Vreg1_Gamma_Set3);
             }
             else
             {
                 System.Windows.Forms.MessageBox.Show("Gamma Set should be 1,2 or 3");
             }
             if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Vreg1 Is Applied", Color.Black);
         }

        private void DP150_Gamma_Set(int Gamma_Set,bool Normal)
        {
            if (Normal) Gamma_Set_60Hz(Gamma_Set);
            else Gamma_Set_90Hz(Gamma_Set);
            
        }


         private void Gamma_Set_60Hz(int Gamma_Set)
         {
             Form1 f1 = (Form1)Application.OpenForms["Form1"];
             if(Gamma_Set == 1)
             {
                 f1.IPC_Quick_Send("mipi.write 0x39 0x76 0x00");
                 //f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x8C");
                 //f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x00");
                 f1.Magnachip_B0_CMD_Sending("mipi.write 0x15 0xB0 0x8C", "mipi.write 0x39 0xE7 0x00");
             }
             else if(Gamma_Set == 2)
             {
                 f1.IPC_Quick_Send("mipi.write 0x39 0x76 0x00");
                 //f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x8C");
                 //f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x01");
                 f1.Magnachip_B0_CMD_Sending("mipi.write 0x15 0xB0 0x8C", "mipi.write 0x39 0xE7 0x01");
             }
             else if(Gamma_Set == 3)
             {
                 f1.IPC_Quick_Send("mipi.write 0x39 0x76 0x00");
                 //f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x8C");
                 //f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x02");
                 f1.Magnachip_B0_CMD_Sending("mipi.write 0x15 0xB0 0x8C", "mipi.write 0x39 0xE7 0x02");
             }
         }

         private void Gamma_Set_90Hz(int Gamma_Set)
         {
             Form1 f1 = (Form1)Application.OpenForms["Form1"];
             if(Gamma_Set == 1)
             {
                 f1.IPC_Quick_Send("mipi.write 0x39 0x76 0x01");
                 //f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x92");
                 //f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x00");
                 f1.Magnachip_B0_CMD_Sending("mipi.write 0x15 0xB0 0x92", "mipi.write 0x39 0xE7 0x00");
             }
             else if(Gamma_Set == 2)
             {
                 f1.IPC_Quick_Send("mipi.write 0x39 0x76 0x01");
                 //f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x92");
                 //f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x01");
                 f1.Magnachip_B0_CMD_Sending("mipi.write 0x15 0xB0 0x92", "mipi.write 0x39 0xE7 0x01");
             }
             else if(Gamma_Set == 3)
             {
                 f1.IPC_Quick_Send("mipi.write 0x39 0x76 0x01");
                 //f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x92");
                 //f1.IPC_Quick_Send("mipi.write 0x39 0xE7 0x02");
                 f1.Magnachip_B0_CMD_Sending("mipi.write 0x15 0xB0 0x92", "mipi.write 0x39 0xE7 0x02");
             }
         }

         private void Single_Mode_Optic_compensation_Gamma_Vreg1_Set_123()
         {
             bool Normal_60Hz = true;
             if (radioButton_Single_Mode_60hz_Set123.Checked) Normal_60Hz = true;
             else if (radioButton_Single_Mode_90hz_Set123.Checked) Normal_60Hz = false;

             //datagridview-related
             Form1 f1 = (Form1)Application.OpenForms["Form1"];
             DP150_Single_Engineerig_Mornitoring_Mode DP150_form_engineer = (DP150_Single_Engineerig_Mornitoring_Mode)Application.OpenForms["DP150_Single_Engineerig_Mornitoring_Mode"];
             DP150_form_engineer.Engineering_Mode_DataGridview_ReadOnly(true);
             DP150_form_engineer.GridView_Measure_Applied_Loop_Area_Data_Clear();
             DP150_form_engineer.Gamma_Vreg1_Diff_Clear();
             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB();
             Application.DoEvents();

             //Initialize
             Optic_Compensation_Stop = false;
             //Textbox_AM0_VREF2_Clear();
             Application.DoEvents();

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

             if (checkBox_Read_DBV_Values.Checked) button_Read_DP116_DBV_Setting.PerformClick();


             if (checkBox_VREF2_AM0_Compensation.Checked && Optic_Compensation_Stop == false)
             {
                 double Vreg1_REF2047_Margin = Convert.ToDouble(textBox_REF2047_Margin.Text);
                 double Vreg1_REF2047_Resolution = Convert.ToDouble(textBox_REF2047_Resolution.Text);
                 double Vreg1_REF2047_Limit_Lv = Convert.ToDouble(textBox_REF2047_Limit_Lv.Text);
                 Optic_Compensation_Stop = DP150_Black_Compensation(Vreg1_REF2047_Margin, Vreg1_REF2047_Resolution, Vreg1_REF2047_Limit_Lv);
             }

             Get_All_Band_Gray_Gamma(All_band_gray_Gamma); //Get All_band_gray_Gamma[12,8]

             if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false)
             {
                 Get_Param(0, ref Gamma, ref Target, ref Limit, ref Extension);
                 Gamma_Init.Equal_Value(Gamma);//190529
                 HBM_Mode_Gray255_Compensation();
                 if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false) DP150_ELVSS_Compensation();
             }


             if (Any_Band_is_Selected)
             {
                 if (checkBox_Vreg1_Compensation.Checked)
                 {
                     //Read Vreg1s to Textbox (For Initial Value)
                     //Update string[] B1_Vreg1_Gamma_Set1 = new string[15];
                     //Update string[] B1_Vreg1_Gamma_Set2 = new string[15];
                     button_Vreg1_Read.PerformClick();
                 }

                 //Set1
                 DP150_Gamma_Set(1, Normal_60Hz);
                 for (band = 0; band < 14; band++)
                 {
                     f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                     if (Optic_Compensation_Stop) break;
                     Gamma_Out_Of_Register_Limit = false;
                     if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                     {
                         if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                         {
                             DP150_Pattern_Setting(0, band, Single_Or_Dual_Single_Is_True);//Pattern Setting
                             f1.AOD_On();
                             DP150_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                         }

                         DP150_form_engineer.Band_Radiobuttion_Select(band);//Select Band
                         Thread.Sleep(300);

                         DP150_DBV_Setting(band);  //DBV Setting



                         if (checkBox_Vreg1_Compensation.Checked)
                         {
                             Vreg1_loop_count = 0; //Vreg1 loop countR
                             Vreg1_Infinite_Count = 0;
                             
                             Vreg1 = this.DP150_Get_Normal_Initial_Vreg1_Set123(band, 1);
                             Initial_Vreg1 = Vreg1;
                             Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(band, Initial_Vreg1);
                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
                         }

                         for (gray = 0; gray < 8; gray++)
                         {
                             if (Optic_Compensation_Stop) break;

                             Get_Param(gray, ref Gamma, ref Target, ref Limit, ref Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table  

                             //HBM의 Gray255꺼는 IRC 보상 안하면 그냥 받음
                             if (checkBox_ELVSS_Comp.Checked)
                             {
                                 if ((band == 0 && gray == 0) == false) //HBM
                                 {
                                     //HBM의 Gray255꺼는 IRC/ELVSS 보상 이전의 Init Gamma 받음
                                 }
                                 else
                                 {
                                     Gamma_Init.Equal_Value(Gamma);
                                 }
                             }
                             else
                             {
                                 Gamma_Init.Equal_Value(Gamma);
                             }

                             if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                             DP150_Pattern_Setting(gray, band, Single_Or_Dual_Single_Is_True);//Pattern Setting


                             if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                 || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                             {
                                 DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                 Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                             }

                             Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 1, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                             
                             Thread.Sleep(300); //Pattern 안정화 Time
                             DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
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
                                         DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                         Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                     }
                                     Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 1, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     Measure.Set_Value(0, 0, 0);
                                     Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                     f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString() + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                     Optic_Compensation_Succeed = true;
                                     break;
                                 }

                                 //Vreg1 + Sub-Compensation (Change Gamma Value)
                                 if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                     || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                 {
                                     if (Vreg1_loop_count == 0)
                                     {
                                         DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
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
                                         f1.GB_Status_AppendText_Nextline("(Gamma Set1)Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);

                                         if (Math.Abs(Diff_Vreg1) >= 1) Vreg1_Need_To_Be_Updated = true;
                                         else Vreg1_Need_To_Be_Updated = false;

                                         if (Vreg1_Need_To_Be_Updated)
                                         {
                                             //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                             DP150_Update_and_Send_Vreg1_Set123(Vreg1, band, 1);
                                             Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                             DP150_Update_Vreg1_TextBox(Vreg1, band, 1);
                                             Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
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

                                     Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                         , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                     //Engineering Mode
                                     Diff_Gamma = Gamma - Prev_Gamma;
                                     f1.GB_Status_AppendText_Nextline("(Gamma Set1)Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
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
                                     Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 1, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                     int DIff_R = Gamma.int_R - Gamma_Init.int_R;
                                     int DIff_G = Gamma.int_G - Gamma_Init.int_G;
                                     int DIff_B = Gamma.int_B - Gamma_Init.int_B;
                                     DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(band, gray, DIff_R, DIff_G, DIff_B);
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
                                 DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                 if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Measured Lv : " + Measure.double_Lv.ToString(), Color.Black);
                                 Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, Extension_Applied); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                 f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                 Application.DoEvents();
                             }
                             f1.GB_ProgressBar_PerformStep();
                             if (checkBox_Only_255G.Checked)
                                 gray = 8;
                         }
                         if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                             f1.AOD_Off();
                     }
                 }//Band Loop End

                 DP150_form_engineer.GridView_Measure_Applied_Loop_Area_Data_Clear();
                 DP150_form_engineer.Gamma_Vreg1_Diff_Clear();
                 DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB();

                 //Set2 (AOD Delete (12 - 3 = 9(band)))
                 DP150_Gamma_Set(2, Normal_60Hz);
                 for (band = 0; band < 11; band++)
                 {
                     f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                     if (Optic_Compensation_Stop) break;
                     Gamma_Out_Of_Register_Limit = false;
                     if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                     {
                         if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                         {
                             DP150_Pattern_Setting(0, band, Single_Or_Dual_Single_Is_True);//Pattern Setting
                             f1.AOD_On();
                             DP150_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                         }

                         DP150_form_engineer.Band_Radiobuttion_Select(band);//Select Band
                         Thread.Sleep(300);

                         DP150_DBV_Setting(band);  //DBV Setting



                         if (checkBox_Vreg1_Compensation.Checked)
                         {
                             Vreg1_loop_count = 0; //Vreg1 loop countR
                             Vreg1_Infinite_Count = 0;
                             Vreg1 = this.DP150_Get_Normal_Initial_Vreg1_Set123(band, 2);
                             Initial_Vreg1 = Vreg1;
                             Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(band, Initial_Vreg1);
                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
                         }

                         for (gray = 0; gray < 8; gray++)
                         {
                             if (Optic_Compensation_Stop) break;

                             Get_Param(gray, ref Gamma, ref Target, ref Limit, ref Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table  

                             //HBM의 Gray255꺼는 IRC 보상 안하면 그냥 받음
                             if (checkBox_ELVSS_Comp.Checked)
                             {
                                 if ((band == 0 && gray == 0) == false) //HBM
                                 {
                                     //HBM의 Gray255꺼는 IRC/ELVSS 보상 이전의 Init Gamma 받음
                                 }
                                 else
                                 {
                                     Gamma_Init.Equal_Value(Gamma);
                                 }
                             }
                             else
                             {
                                 Gamma_Init.Equal_Value(Gamma);
                             }

                             if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                             DP150_Pattern_Setting(gray, band, Single_Or_Dual_Single_Is_True);//Pattern Setting


                             if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                 || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                             {
                                 DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                 Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                             }

                             Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 2, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022


                             Thread.Sleep(300); //Pattern 안정화 Time
                             DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
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
                                         DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                         Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                     }

                                     Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 2, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     
                                     Measure.Set_Value(0, 0, 0);
                                     Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                     f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString() + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                     Optic_Compensation_Succeed = true;
                                     break;
                                 }

                                 //Vreg1 + Sub-Compensation (Change Gamma Value)
                                 if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                     || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                 {
                                     if (Vreg1_loop_count == 0)
                                     {
                                         DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
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
                                         f1.GB_Status_AppendText_Nextline("(Gamma Set2)Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);

                                         if (Math.Abs(Diff_Vreg1) >= 1) Vreg1_Need_To_Be_Updated = true;
                                         else Vreg1_Need_To_Be_Updated = false;

                                         if (Vreg1_Need_To_Be_Updated)
                                         {
                                             //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                             DP150_Update_and_Send_Vreg1_Set123(Vreg1, band, 2);
                                             Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                             DP150_Update_Vreg1_TextBox(Vreg1, band, 2);
                                             Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
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

                                     Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                         , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                     //Engineering Mode
                                     Diff_Gamma = Gamma - Prev_Gamma;
                                     f1.GB_Status_AppendText_Nextline("(Gamma Set2)Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
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
                                     Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 2, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022

                                     Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                     int DIff_R = Gamma.int_R - Gamma_Init.int_R;
                                     int DIff_G = Gamma.int_G - Gamma_Init.int_G;
                                     int DIff_B = Gamma.int_B - Gamma_Init.int_B;
                                     DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(band, gray, DIff_R, DIff_G, DIff_B);
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
                                 DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                 if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Measured Lv : " + Measure.double_Lv.ToString(), Color.Black);
                                 Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, Extension_Applied); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                 f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                 Application.DoEvents();
                             }
                             f1.GB_ProgressBar_PerformStep();
                             if (checkBox_Only_255G.Checked)
                                 gray = 8;
                         }
                         if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                             f1.AOD_Off();
                     }
                 }//Band Loop End

                 DP150_form_engineer.GridView_Measure_Applied_Loop_Area_Data_Clear();
                 DP150_form_engineer.Gamma_Vreg1_Diff_Clear();
                 DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB();

                 //Set3 (AOD Delete (12 - 3 = 9(band)))
                 DP150_Gamma_Set(3, Normal_60Hz);
                 for (band = 0; band < 11; band++)
                 {
                     f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                     if (Optic_Compensation_Stop) break;
                     Gamma_Out_Of_Register_Limit = false;
                     if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                     {
                         if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                         {
                             DP150_Pattern_Setting(0, band, Single_Or_Dual_Single_Is_True);//Pattern Setting
                             f1.AOD_On();
                             DP150_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                         }

                         DP150_form_engineer.Band_Radiobuttion_Select(band);//Select Band
                         Thread.Sleep(300);

                         DP150_DBV_Setting(band);  //DBV Setting



                         if (checkBox_Vreg1_Compensation.Checked)
                         {
                             Vreg1_loop_count = 0; //Vreg1 loop countR
                             Vreg1_Infinite_Count = 0;
                             Vreg1 = this.DP150_Get_Normal_Initial_Vreg1_Set123(band, 3);
                             Initial_Vreg1 = Vreg1;
                             Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(band, Initial_Vreg1);
                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
                         }

                         for (gray = 0; gray < 8; gray++)
                         {
                             if (Optic_Compensation_Stop) break;

                             Get_Param(gray, ref Gamma, ref Target, ref Limit, ref Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table  

                             //HBM의 Gray255꺼는 IRC 보상 안하면 그냥 받음
                             if (checkBox_ELVSS_Comp.Checked)
                             {
                                 if ((band == 0 && gray == 0) == false) //HBM
                                 {
                                     //HBM의 Gray255꺼는 IRC/ELVSS 보상 이전의 Init Gamma 받음
                                 }
                                 else
                                 {
                                     Gamma_Init.Equal_Value(Gamma);
                                 }
                             }
                             else
                             {
                                 Gamma_Init.Equal_Value(Gamma);
                             }

                             if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                             DP150_Pattern_Setting(gray, band, Single_Or_Dual_Single_Is_True);//Pattern Setting


                             if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                 || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                             {
                                 DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                 Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                             }

                             Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 3, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                             Thread.Sleep(300); //Pattern 안정화 Time
                             DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
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
                                         DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                         Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                     }
                                     Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 3, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     Measure.Set_Value(0, 0, 0);
                                     Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                     f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString() + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                     Optic_Compensation_Succeed = true;
                                     break;
                                 }

                                 //Vreg1 + Sub-Compensation (Change Gamma Value)
                                 if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                     || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                 {
                                     if (Vreg1_loop_count == 0)
                                     {
                                         DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                         Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                     }

                                     Vreg1_Infinite_Loop_Check(Vreg1_loop_count);

                                     if (Vreg1_loop_count < loop_count_max)
                                     {
                                         Prev_Vreg1 = Vreg1;
                                         Prev_Gamma.Equal_Value(Gamma);

                                         Imported_my_cpp_dll.Vreg1_Compensation(loop_count, Vreg1_Infinite, Vreg1_Infinite_Count, ref Gamma.int_R, ref Vreg1, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                         , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, Vreg1_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                         Diff_Vreg1 = Vreg1 - Prev_Vreg1;
                                         Diff_Gamma = Gamma - Prev_Gamma;
                                         f1.GB_Status_AppendText_Nextline("(Gamma Set3)Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);

                                         if (Math.Abs(Diff_Vreg1) >= 1) Vreg1_Need_To_Be_Updated = true;
                                         else Vreg1_Need_To_Be_Updated = false;

                                         if (Vreg1_Need_To_Be_Updated)
                                         {
                                             //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                             DP150_Update_and_Send_Vreg1_Set123(Vreg1, band, 3);
                                             Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                             DP150_Update_Vreg1_TextBox(Vreg1, band, 3);
                                             Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
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

                                     Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                         , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);

                                     //Engineering Mode
                                     Diff_Gamma = Gamma - Prev_Gamma;
                                     f1.GB_Status_AppendText_Nextline("(Gamma Set3)Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B, Color.Blue);
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
                                     Update_and_Send_All_Band_Gray_Gamma_Set123(All_band_gray_Gamma, Gamma, band, gray, 3, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                     int DIff_R = Gamma.int_R - Gamma_Init.int_R;
                                     int DIff_G = Gamma.int_G - Gamma_Init.int_G;
                                     int DIff_B = Gamma.int_B - Gamma_Init.int_B;
                                     DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(band, gray, DIff_R, DIff_G, DIff_B);
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
                                 DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                 if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Measured Lv : " + Measure.double_Lv.ToString(), Color.Black);
                                 Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, Extension_Applied); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                 f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                 Application.DoEvents();
                             }
                             f1.GB_ProgressBar_PerformStep();
                             if (checkBox_Only_255G.Checked)
                                 gray = 8;
                         }
                         if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                             f1.AOD_Off();
                     }
                 }//Band Loop End
                 DP150_form_engineer.Engineering_Mode_DataGridview_ReadOnly(false);
             }
             f1.OC_Timer_Stop();
             DP150_DBV_Setting(1);  //DBV Setting    
             DP150_Pattern_Setting(0, 1, Single_Or_Dual_Single_Is_True);
             //---------------------------------------------
             if (Optic_Compensation_Succeed) System.Windows.Forms.MessageBox.Show("Optic Compensation Finish !");
         }


         private void Single_Mode_Optic_compensation()
         {
             //datagridview-related
             Form1 f1 = (Form1)Application.OpenForms["Form1"];
             DP150_Single_Engineerig_Mornitoring_Mode DP150_form_engineer = (DP150_Single_Engineerig_Mornitoring_Mode)Application.OpenForms["DP150_Single_Engineerig_Mornitoring_Mode"];
             DP150_form_engineer.Engineering_Mode_DataGridview_ReadOnly(true);
             DP150_form_engineer.GridView_Measure_Applied_Loop_Area_Data_Clear();
             DP150_form_engineer.Gamma_Vreg1_Diff_Clear();
             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB();
             Application.DoEvents();

             //Initialize
             Optic_Compensation_Stop = false;
             //Textbox_AM0_VREF2_Clear();
             Application.DoEvents();
            
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

             if (checkBox_Read_DBV_Values.Checked) button_Read_DP116_DBV_Setting.PerformClick();


             if (checkBox_VREF2_AM0_Compensation.Checked && Optic_Compensation_Stop == false)
             {
                 double Vreg1_REF2047_Margin = Convert.ToDouble(textBox_REF2047_Margin.Text);
                 double Vreg1_REF2047_Resolution = Convert.ToDouble(textBox_REF2047_Resolution.Text);
                 double Vreg1_REF2047_Limit_Lv = Convert.ToDouble(textBox_REF2047_Limit_Lv.Text);
                 Optic_Compensation_Stop = DP150_Black_Compensation(Vreg1_REF2047_Margin, Vreg1_REF2047_Resolution, Vreg1_REF2047_Limit_Lv);
             }

             Get_All_Band_Gray_Gamma(All_band_gray_Gamma); //Get All_band_gray_Gamma[12,8]

             if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false)
             {
                 Get_Param(0, ref Gamma, ref Target, ref Limit, ref Extension);
                 Gamma_Init.Equal_Value(Gamma);//190529
                 HBM_Mode_Gray255_Compensation();
                 if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false) DP150_ELVSS_Compensation();
             }



             if (Any_Band_is_Selected)
             {
                 if (checkBox_Vreg1_Compensation.Checked)
                 {
                     //Read Vreg1s to Textbox (For Initial Value)
                     //Update string[] B1_Vreg1_Gamma_Set1 = new string[15];
                     //Update string[] B1_Vreg1_Gamma_Set2 = new string[15];
                     button_Vreg1_Read.PerformClick();
                 }

                 for (band = 0; band < 14; band++)
                 {
                     f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                     if (Optic_Compensation_Stop) break;
                     Gamma_Out_Of_Register_Limit = false;
                     if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                     {
                         if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                         {
                             DP150_Pattern_Setting(0, band, Single_Or_Dual_Single_Is_True);//Pattern Setting
                             f1.AOD_On();
                             DP150_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                         }

                         DP150_form_engineer.Band_Radiobuttion_Select(band);//Select Band
                         Thread.Sleep(300);

                         DP150_DBV_Setting(band);  //DBV Setting
                         


                         if (checkBox_Vreg1_Compensation.Checked)
                         {
                             Vreg1_loop_count = 0; //Vreg1 loop countR
                             Vreg1_Infinite_Count = 0;
                             if(radioButton_Single_Mode.Checked)
                                 Vreg1 = this.DP150_Get_Normal_Initial_Vreg1(band, true);
                             else if(radioButton_Single_Mode_90Hz.Checked)
                                 Vreg1 = this.DP150_Get_Normal_Initial_Vreg1(band, false);

                             Initial_Vreg1 = Vreg1;
                             Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(band, Initial_Vreg1);
                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
                         }

                         for (gray = 0; gray < 8; gray++)
                         {
                             if (Optic_Compensation_Stop) break;

                             Get_Param(gray, ref Gamma, ref Target, ref Limit, ref Extension); //Get (First)Gamma,Target,Limit From OC-Param-Table  
                             
                             //HBM의 Gray255꺼는 IRC 보상 안하면 그냥 받음
                             if (checkBox_ELVSS_Comp.Checked)
                             {
                                 if ((band == 0 && gray == 0) == false) //HBM
                                 {
                                     //HBM의 Gray255꺼는 IRC/ELVSS 보상 이전의 Init Gamma 받음
                                 }
                                 else
                                 {
                                     Gamma_Init.Equal_Value(Gamma);
                                 }
                             }
                             else
                             {
                                 Gamma_Init.Equal_Value(Gamma);
                             }

                             if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                             DP150_Pattern_Setting(gray, band, Single_Or_Dual_Single_Is_True);//Pattern Setting


                             if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                 || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                             {
                                 DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                 
                                 Prev_Band_Gray255_Gamma.int_G += (Convert.ToInt16(textBox_Vreg1_Gamma_G_Offset.Text));//Add on 191112
                                 
                                 Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                             }

                             if (radioButton_Single_Mode.Checked)
                             {
                                 //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, true, "00", "00", "00", "00", "00", "00");
                                 Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, true, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                             }
                             else if (radioButton_Single_Mode_90Hz.Checked)
                             {
                                 //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, false, "00", "00", "00", "00", "00", "00");
                                 Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, false, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                             }
                             Thread.Sleep(300); //Pattern 안정화 Time
                             DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
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
                                         DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);

                                         
                                         Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                     }
                                     if (radioButton_Single_Mode.Checked)
                                     {
                                         
                                         //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, true, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                         Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, true, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     }
                                     else if (radioButton_Single_Mode_90Hz.Checked)
                                     {
                                         //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, false, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                         Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, false, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     }
                                         Measure.Set_Value(0, 0, 0);
                                     Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, "X"); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                     f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString() + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                     Optic_Compensation_Succeed = true;
                                     break;
                                 }

                                 //Vreg1 + Sub-Compensation (Change Gamma Value)
                                 if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                     || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                 {
                                     if (Vreg1_loop_count == 0)
                                     {
                                         DP150_form_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B);
                                         Prev_Band_Gray255_Gamma.int_G += (Convert.ToInt16(textBox_Vreg1_Gamma_G_Offset.Text));//Add on 191112
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
                                         f1.GB_Status_AppendText_Nextline("Diff Red/Vreg1/Blue = " + Diff_Gamma.R + "/" + Diff_Vreg1.ToString() + "/" + Diff_Gamma.B, Color.Blue);

                                         if (Math.Abs(Diff_Vreg1) >= 1) Vreg1_Need_To_Be_Updated = true;
                                         else Vreg1_Need_To_Be_Updated = false;

                                         if (Vreg1_Need_To_Be_Updated)
                                         {
                                             //f1.GB_Status_AppendText_Nextline("Vreg1 Setting", Color.Red);
                                             if(radioButton_Single_Mode.Checked)
                                                 DP150_Update_and_Send_Vreg1(Vreg1, band, true);
                                             else if(radioButton_Single_Mode_90Hz.Checked)
                                                 DP150_Update_and_Send_Vreg1(Vreg1, band, false);
                                             Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                             DP150_Update_Vreg1_TextBox(Vreg1, band, true);
                                             Diff_Vreg1 = Vreg1 - Initial_Vreg1;
                                             DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(band, Diff_Vreg1);
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
                                     else Extension_Applied = "X";

                                 }

                                 if (Vreg1_Need_To_Be_Updated == false)
                                 {
                                     //f1.GB_Status_AppendText_Nextline("Gamma Setting", Color.Blue);
                                     if (radioButton_Single_Mode.Checked)
                                     {
                                         //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, true, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                         Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, true, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     }
                                     else if (radioButton_Single_Mode_90Hz.Checked)
                                     {
                                         //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, false, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                         Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, false, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                     }
                                     Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                     int DIff_R = Gamma.int_R - Gamma_Init.int_R;
                                     int DIff_G = Gamma.int_G - Gamma_Init.int_G;
                                     int DIff_B = Gamma.int_B - Gamma_Init.int_B;
                                     DP150_form_engineer.dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(band, gray, DIff_R, DIff_G, DIff_B);
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
                                 DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                 if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Measured Lv : " + Measure.double_Lv.ToString(), Color.Black);
                                 Update_Engineering_Sheet(Gamma, Measure, band, gray, loop_count, Extension_Applied); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                 f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension); 
                                 Application.DoEvents();
                             }
                             f1.GB_ProgressBar_PerformStep();
                             if (checkBox_Only_255G.Checked)
                                 gray = 8;
                         }
                         if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                            f1.AOD_Off();
                     }
                 }
                 DP150_form_engineer.Engineering_Mode_DataGridview_ReadOnly(false);
             }
             f1.OC_Timer_Stop();
             DP150_DBV_Setting(1);  //DBV Setting    
             DP150_Pattern_Setting(0, 1, Single_Or_Dual_Single_Is_True);
             //---------------------------------------------
             if (Optic_Compensation_Succeed) System.Windows.Forms.MessageBox.Show("Optic Compensation Finish !");
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
            if (checkBox_Band11.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band12.Checked) ProgressBar_max += (8 * step);
            if (checkBox_Band13.Checked) ProgressBar_max += (8 * step);

            bool If_Any_Band_Is_Selected;
            if (ProgressBar_max == 0)
                If_Any_Band_Is_Selected = false;
            else
                If_Any_Band_Is_Selected = true;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Set_GB_ProgressBar_Maximum(ProgressBar_max);
            f1.Set_GB_ProgressBar_Step(step);

            return If_Any_Band_Is_Selected;
        }

        public void DP150_Pattern_Setting(int gray, int band, bool Single_Or_Dual_Single_Is_True)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int Gray = 255;
            if (checkBox_Special_Gray_Compensation.Checked)
            {
                string Band_Gray = string.Empty;
                if (Single_Or_Dual_Single_Is_True) //Single
                {
                    DP150_Single_Engineerig_Mornitoring_Mode DP150_form_engineer = (DP150_Single_Engineerig_Mornitoring_Mode)Application.OpenForms["DP150_Single_Engineerig_Mornitoring_Mode"];
                    Band_Gray = DP150_form_engineer.Get_BX_GXXX_By_Gray_DP116(gray);
                }
                else //Dual
                {
                    DP150_Dual_Engineering_Mornitoring_Mode DP150_form_dual_engineer = (DP150_Dual_Engineering_Mornitoring_Mode)Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
                    Band_Gray = DP150_form_dual_engineer.Dual_Get_BX_GXXX_By_Gray_DP116(gray);
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
            if (band == 11 || band == 12 || band == 13) //AOD Mode Pattern
            {
                f1.IPC_Quick_Send("image.crosstalk " + f1.current_model.get_AOD_X() + " " + f1.current_model.get_AOD_Y() + " 0 0 0 " + Gray.ToString() + " " + Gray.ToString() + " " + Gray.ToString());
            }
            else //Normal Mode Pattern
            {
                if (checkBox_OC_IRC_Box_PTN.Checked)
                {
                    f1.DP150_IRC_Box_PTN_update(Gray, Gray, Gray, true);
                    f1.GB_Status_AppendText_Nextline("IRC Gray" + Gray.ToString() + " Box(up) Pattern Setting", System.Drawing.Color.Black);
                }
                else if (checkBox_OC_APL_Change.Checked)
                {
                    double APL = Convert.ToDouble(numericUpDown_APL.Value) * 0.01;
                    f1.APL_PTN_update(Gray, Gray, Gray, APL);
                    f1.GB_Status_AppendText_Nextline("Gray" + Gray.ToString() + " Setting , APL(%) : " + APL.ToString() + " Apply", System.Drawing.Color.Black);   
                }
                else
                {
                    if ((checkBox_OC_APL_Change_HBM_Only.Checked && band == 0) || (checkBox_OC_APL_Change_Normal.Checked && band == 1))
                    {
                            string G = Gray.ToString();
                            f1.IPC_Quick_Send("image.crosstalk 563 562 0 0 0 " + G + " " + G + " " + G);
                            f1.GB_Status_AppendText_Nextline("HBM APL(10%)Gray" + Gray.ToString() + " Setting", System.Drawing.Color.Black);
                    }
                    else
                    {
                        if (checkBox_G127_Before_Main_Pattern.Checked)
                        {
                            f1.PTN_update(127, 127, 127);
                            f1.GB_Status_AppendText_Nextline("Gray 127 Setting", System.Drawing.Color.Black);
                            Thread.Sleep(300);
                        }
                        f1.PTN_update(Gray, Gray, Gray);
                        f1.GB_Status_AppendText_Nextline("Gray" + Gray.ToString() + " Setting", System.Drawing.Color.Black);
                    }
                }
            }
        }


        public string Update_and_Send_All_Band_Gray_Gamma_Set123(RGB[,] All_band_gray_Gamma, RGB Current_Gamma, int Current_Band, int Current_Gray, int Gamma_Set
            , string R_AM1_SET1, string R_AM0_SET1, string G_AM1_SET1, string G_AM0_SET1, string B_AM1_SET1, string B_AM0_SET1)
        {
            //Gamma_Set = true --> Gamma Set 1
            //Gamma_Set = False --> Gamma Set 2

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP150_EA9152 DP150 = DP150_EA9152.getInstance();
            //Update Gamma table as current Gamma
            All_band_gray_Gamma[Current_Band, Current_Gray].Equal_Value(Current_Gamma);

            RGB[] Gamma_9th_data = new RGB[8]; //G255
            RGB[] Gamma_8ea_data = new RGB[8]; //GXXX < G255

            string[] Hex_Param = new string[40];

            string MIPI_CMD = "mipi.write 0x39 ";
            MIPI_CMD += DP150.Get_Gamma_Register_Hex_String(Current_Band);

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
            Hex_Param[4] = R_AM1_SET1;
            Hex_Param[5] = R_AM0_SET1;
            Hex_Param[6] = Gamma_8ea_data[1].int_R.ToString("X2"); //G191[7:0]
            Hex_Param[7] = Gamma_8ea_data[2].int_R.ToString("X2"); //G127[7:0]
            Hex_Param[8] = Gamma_8ea_data[3].int_R.ToString("X2"); //G63[7:0]
            Hex_Param[9] = Gamma_8ea_data[4].int_R.ToString("X2"); //G31[7:0]
            Hex_Param[10] = Gamma_8ea_data[5].int_R.ToString("X2"); //G15[7:0]
            Hex_Param[11] = Gamma_8ea_data[6].int_R.ToString("X2"); //G7[7:0]
            Hex_Param[12] = "90";//G4[7:0]
            Hex_Param[13] = Gamma_8ea_data[7].int_R.ToString("X2");//G1[7:0]



            Hex_Param[14] = ((Gamma_9th_data[1].int_G << 7) + (Gamma_9th_data[2].int_G << 6)
                + (Gamma_9th_data[3].int_G << 5) + (Gamma_9th_data[4].int_G << 4)
                + (Gamma_9th_data[5].int_G << 3) + (Gamma_9th_data[6].int_G << 2)
                + 2 + Gamma_9th_data[7].int_G).ToString("X2"); //GXX < G255 G 9th
            Hex_Param[15] = "00";
            Hex_Param[16] = Gamma_8ea_data[0].int_G.ToString("X2"); //G255[7:0]
            Hex_Param[17] = G_AM1_SET1;
            Hex_Param[18] = G_AM0_SET1;
            Hex_Param[19] = Gamma_8ea_data[1].int_G.ToString("X2"); //G191[7:0]
            Hex_Param[20] = Gamma_8ea_data[2].int_G.ToString("X2"); //G127[7:0]
            Hex_Param[21] = Gamma_8ea_data[3].int_G.ToString("X2"); //G63[7:0]
            Hex_Param[22] = Gamma_8ea_data[4].int_G.ToString("X2"); //G31[7:0]
            Hex_Param[23] = Gamma_8ea_data[5].int_G.ToString("X2"); //G15[7:0]
            Hex_Param[24] = Gamma_8ea_data[6].int_G.ToString("X2"); //G7[7:0]
            Hex_Param[25] = "90";//G4[7:0]
            Hex_Param[26] = Gamma_8ea_data[7].int_G.ToString("X2");//G1[7:0]



            Hex_Param[27] = ((Gamma_9th_data[1].int_B << 7) + (Gamma_9th_data[2].int_B << 6)
                + (Gamma_9th_data[3].int_B << 5) + (Gamma_9th_data[4].int_B << 4)
                + (Gamma_9th_data[5].int_B << 3) + (Gamma_9th_data[6].int_B << 2)
                + 2 + Gamma_9th_data[7].int_B).ToString("X2"); //GXX < G255 B 9th
            Hex_Param[28] = "00";
            Hex_Param[29] = Gamma_8ea_data[0].int_B.ToString("X2"); //G255[7:0]
            Hex_Param[30] = G_AM1_SET1;
            Hex_Param[31] = G_AM0_SET1;
            Hex_Param[32] = Gamma_8ea_data[1].int_B.ToString("X2"); //G191[7:0]
            Hex_Param[33] = Gamma_8ea_data[2].int_B.ToString("X2"); //G127[7:0]
            Hex_Param[34] = Gamma_8ea_data[3].int_B.ToString("X2"); //G63[7:0]
            Hex_Param[35] = Gamma_8ea_data[4].int_B.ToString("X2"); //G31[7:0]
            Hex_Param[36] = Gamma_8ea_data[5].int_B.ToString("X2"); //G15[7:0]
            Hex_Param[37] = Gamma_8ea_data[6].int_B.ToString("X2"); //G7[7:0]
            Hex_Param[38] = "90";//G4[7:0]
            Hex_Param[39] = Gamma_8ea_data[7].int_B.ToString("X2");//G1[7:0]

            for (int i = 0; i < 40; i++) MIPI_CMD += (" 0x" + Hex_Param[i]);

            /*
            if (Gamma_Set == 2 && Current_Band < 9) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x28"); //GammaSet2 For Normal
            else if (Gamma_Set == 3 && Current_Band < 9) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x50"); //GammaSet3 For Normal
            else if (Current_Band == 9) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x0A"); //AOD1
            else if (Current_Band == 10) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x32");//AOD2
            else if (Current_Band == 11) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x5A");//AOD3
            
            f1.IPC_Quick_Send(MIPI_CMD);
            */
            string Temp = string.Empty;
            if (Gamma_Set == 2 && Current_Band < 11)
            {
                Temp = "mipi.write 0x15 0xB0 0x28"; //GammaSet2 For Normal
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else if (Gamma_Set == 3 && Current_Band < 11)
            {
                Temp = "mipi.write 0x15 0xB0 0x50"; //GammaSet3 For Normal
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else if (Current_Band == 11)
            {
                Temp = "mipi.write 0x15 0xB0 0x0A"; //AOD1
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else if (Current_Band == 11)
            {
                Temp = "mipi.write 0x15 0xB0 0x32";//AOD2
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else if (Current_Band == 11)
            {
                Temp = "mipi.write 0x15 0xB0 0x5A";//AOD3
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else
            {
                f1.IPC_Quick_Send(MIPI_CMD);
            }
            return MIPI_CMD;
        }

        public string Update_and_Send_All_Band_Gray_Gamma(RGB[,] All_band_gray_Gamma, RGB Current_Gamma, int Current_Band, int Current_Gray, bool Gamma_Set1
            ,string R_AM1_SET1,string R_AM0_SET1,string G_AM1_SET1,string G_AM0_SET1,string B_AM1_SET1,string B_AM0_SET1)
        {
            //Gamma_Set = true --> Gamma Set 1
            //Gamma_Set = False --> Gamma Set 2

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP150_EA9152 DP150 = DP150_EA9152.getInstance();
            //Update Gamma table as current Gamma
            All_band_gray_Gamma[Current_Band, Current_Gray].Equal_Value(Current_Gamma);

            RGB[] Gamma_9th_data = new RGB[8]; //G255
            RGB[] Gamma_8ea_data = new RGB[8]; //GXXX < G255

            string[] Hex_Param = new string[40];
            
            string MIPI_CMD = "mipi.write 0x39 ";
            MIPI_CMD += DP150.Get_Gamma_Register_Hex_String(Current_Band);

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
            Hex_Param[4] = R_AM1_SET1;
            Hex_Param[5] = R_AM0_SET1;
            Hex_Param[6] = Gamma_8ea_data[1].int_R.ToString("X2"); //G191[7:0]
            Hex_Param[7] = Gamma_8ea_data[2].int_R.ToString("X2"); //G127[7:0]
            Hex_Param[8] = Gamma_8ea_data[3].int_R.ToString("X2"); //G63[7:0]
            Hex_Param[9] = Gamma_8ea_data[4].int_R.ToString("X2"); //G31[7:0]
            Hex_Param[10] = Gamma_8ea_data[5].int_R.ToString("X2"); //G15[7:0]
            Hex_Param[11] = Gamma_8ea_data[6].int_R.ToString("X2"); //G7[7:0]
            Hex_Param[12] = "90";//G4[7:0]
            Hex_Param[13] = Gamma_8ea_data[7].int_R.ToString("X2");//G1[7:0]



            Hex_Param[14] = ((Gamma_9th_data[1].int_G << 7) + (Gamma_9th_data[2].int_G << 6)
                + (Gamma_9th_data[3].int_G << 5) + (Gamma_9th_data[4].int_G << 4)
                + (Gamma_9th_data[5].int_G << 3) + (Gamma_9th_data[6].int_G << 2)
                + 2 + Gamma_9th_data[7].int_G).ToString("X2"); //GXX < G255 G 9th
            Hex_Param[15] = "00";
            Hex_Param[16] = Gamma_8ea_data[0].int_G.ToString("X2"); //G255[7:0]
            Hex_Param[17] = G_AM1_SET1;
            Hex_Param[18] = G_AM0_SET1;
            Hex_Param[19] = Gamma_8ea_data[1].int_G.ToString("X2"); //G191[7:0]
            Hex_Param[20] = Gamma_8ea_data[2].int_G.ToString("X2"); //G127[7:0]
            Hex_Param[21] = Gamma_8ea_data[3].int_G.ToString("X2"); //G63[7:0]
            Hex_Param[22] = Gamma_8ea_data[4].int_G.ToString("X2"); //G31[7:0]
            Hex_Param[23] = Gamma_8ea_data[5].int_G.ToString("X2"); //G15[7:0]
            Hex_Param[24] = Gamma_8ea_data[6].int_G.ToString("X2"); //G7[7:0]
            Hex_Param[25] = "90";//G4[7:0]
            Hex_Param[26] = Gamma_8ea_data[7].int_G.ToString("X2");//G1[7:0]



            Hex_Param[27] = ((Gamma_9th_data[1].int_B << 7) + (Gamma_9th_data[2].int_B << 6)
                + (Gamma_9th_data[3].int_B << 5) + (Gamma_9th_data[4].int_B << 4)
                + (Gamma_9th_data[5].int_B << 3) + (Gamma_9th_data[6].int_B << 2)
                + 2 + Gamma_9th_data[7].int_B).ToString("X2"); //GXX < G255 B 9th
            Hex_Param[28] = "00";
            Hex_Param[29] = Gamma_8ea_data[0].int_B.ToString("X2"); //G255[7:0]
            Hex_Param[30] = G_AM1_SET1;
            Hex_Param[31] = G_AM0_SET1;
            Hex_Param[32] = Gamma_8ea_data[1].int_B.ToString("X2"); //G191[7:0]
            Hex_Param[33] = Gamma_8ea_data[2].int_B.ToString("X2"); //G127[7:0]
            Hex_Param[34] = Gamma_8ea_data[3].int_B.ToString("X2"); //G63[7:0]
            Hex_Param[35] = Gamma_8ea_data[4].int_B.ToString("X2"); //G31[7:0]
            Hex_Param[36] = Gamma_8ea_data[5].int_B.ToString("X2"); //G15[7:0]
            Hex_Param[37] = Gamma_8ea_data[6].int_B.ToString("X2"); //G7[7:0]
            Hex_Param[38] = "90";//G4[7:0]
            Hex_Param[39] = Gamma_8ea_data[7].int_B.ToString("X2");//G1[7:0]

            for (int i = 0; i < 40; i++) MIPI_CMD += (" 0x" + Hex_Param[i]);

            /*
            if (Gamma_Set1 == false && Current_Band < 9) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x28"); //GammaSet2 For Normal
            else if (Current_Band == 9) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x0A"); //AOD1
            else if (Current_Band == 10) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x32");//AOD2
            else if (Current_Band == 11) f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0x5A");//AOD3
              //AOD1
            f1.IPC_Quick_Send(MIPI_CMD);
            */
            string Temp = string.Empty;
            if (Gamma_Set1 == false && Current_Band < 11)
            {
                Temp = "mipi.write 0x15 0xB0 0x28"; //GammaSet2 For Normal
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else if (Current_Band == 11)
            {
                Temp = "mipi.write 0x15 0xB0 0x0A"; //AOD1
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else if (Current_Band == 12)
            {
                Temp = "mipi.write 0x15 0xB0 0x32";//AOD2
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else if (Current_Band == 13)
            {
                Temp = "mipi.write 0x15 0xB0 0x5A";//AOD3
                f1.Magnachip_B0_CMD_Sending(Temp, MIPI_CMD);
            }
            else
            {
                f1.IPC_Quick_Send(MIPI_CMD);
            }
            return MIPI_CMD;
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


        private void Update_Engineering_Sheet(RGB Gamma,XYLv Measure,int band, int gray, int loop_count, string Extension_Applied)
        {
            DP150_Single_Engineerig_Mornitoring_Mode DP150_form_engineer = (DP150_Single_Engineerig_Mornitoring_Mode)Application.OpenForms["DP150_Single_Engineerig_Mornitoring_Mode"];
            DP150_form_engineer.Set_OC_Param_DP150(gray, Gamma.int_R, Gamma.int_G, Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, loop_count, Extension_Applied);
            DP150_form_engineer.Updata_Sub_To_Main_GridView(band, gray);
        }


        private void HBM_Mode_Gray255_Compensation()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //f1.Engineering_Mode_Show();
            DP150_Single_Engineerig_Mornitoring_Mode DP150_form_engineer = (DP150_Single_Engineerig_Mornitoring_Mode)Application.OpenForms["DP150_Single_Engineerig_Mornitoring_Mode"];

            DP150_DBV_Setting(0);//HBM DBV Setting
            DP150_Pattern_Setting(0, 0, Single_Or_Dual_Single_Is_True);//HBM Gray255 Pattern Setting
            Thread.Sleep(300); //Pattern 안정화 Time

            DP150_form_engineer.Band_Radiobuttion_Select(0);//Select HBM

            Optic_Compensation_Succeed = false;
            Optic_Compensation_Stop = false;
            int loop_count = 0;
            Infinite_Count = 0;
            Infinite = false;



            Get_Param(0, ref Gamma, ref Target, ref Limit, ref Extension); ; //Get (First) Gray255 Gamma,Target,Limit From OC-Param-Table 
            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);
            if (radioButton_Single_Mode.Checked)
            {
                //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0, true, "00", "00", "00", "00", "00", "00");
                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0, true, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
            }
            else if (radioButton_Single_Mode_90Hz.Checked)
            {
                //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0, false, "00", "00", "00", "00", "00", "00");
                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0, false, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
            }
            Thread.Sleep(20);
            Update_Engineering_Sheet(Gamma, Measure, 0, 0, loop_count, "X");

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

                if (radioButton_Single_Mode.Checked)
                {
                    //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0, true, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values for HBM/Gray255
                    Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0, true, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                }
                else if (radioButton_Single_Mode_90Hz.Checked)
                {
                    //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0, false, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values for HBM/Gray255
                    Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, 0, 0, false, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                }
                Thread.Sleep(20);

                if (Infinite_Count >= 3)
                {
                    Extension_Applied = "O";
                    if (f1.radioButton_Debug_Status_Mode.Checked) f1.GB_Status_AppendText_Nextline("Extension(" + Extension.double_X.ToString() + "," + Extension.double_Y.ToString() + ") Are Applied", Color.Blue);
                }
                else
                    Extension_Applied = "X";

                Update_Engineering_Sheet(Gamma, Measure, 0, 0, loop_count, Extension_Applied);

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
        

        private void Get_All_Band_Gray_Gamma(RGB[,] All_band_gray_Gamma)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Get All Band/Gray Gamma from OC_Param", Color.Blue);

            DP150_Single_Engineerig_Mornitoring_Mode DP150_form_engineer = (DP150_Single_Engineerig_Mornitoring_Mode)Application.OpenForms["DP150_Single_Engineerig_Mornitoring_Mode"];
            DP150_form_engineer.DP150_Get_All_Band_Gray_Gamma(All_band_gray_Gamma);
        }


        private void Dual_Get_All_Band_Gray_Gamma(bool Condition)
        {
            DP150_Dual_Engineering_Mornitoring_Mode DP150_form_Dual_engineer = (DP150_Dual_Engineering_Mornitoring_Mode)Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
            DP150_form_Dual_engineer.DP150_Get_All_Band_Gray_Gamma(All_band_gray_Gamma, Condition);
        }


        private void Dual_Mode_Optic_Compensation()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP150_Dual_Engineering_Mornitoring_Mode DP150_form_Dual_engineer = (DP150_Dual_Engineering_Mornitoring_Mode)Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
            DP150_form_Dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(true);
            DP150_form_Dual_engineer.Dual_Mode_GridView_Measure_Applied_Loop_Area_Data_Clear();
            DP150_form_Dual_engineer.Clear_Dual_Mode_Cal_Gamma_Diff();

            Optic_Compensation_Stop = false;

            //CA channel Selection
            f1.objMemory.ChannelNO = Convert.ToInt32(f1.textBox_ch.Text);
            f1.CA_Channel_trackBar.Value = Convert.ToInt32(f1.textBox_ch.Text);

            //Timer Start
            f1.OC_Timer_Start();


            //dll-related variables
            Gamma_Out_Of_Register_Limit = false;
            Within_Spec_Limit = false;

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
            bool Any_Band_is_Selected = ProgressBar_Max_Step_Setting(step * 2); //Set Progressbar's Step and Max-Value

            if (checkBox_Send_Manual_Code.Checked)
            {
                f1.PNC_Manual_Button_Click();
                Thread.Sleep(3000); //Manual Code 안정화 Time
            }

            if (checkBox_Read_DBV_Values.Checked) button_Read_DP116_DBV_Setting.PerformClick();

            if (checkBox_VREF2_AM0_Compensation.Checked && Optic_Compensation_Stop == false)
            {
                double Vreg1_REF2047_Margin = Convert.ToDouble(textBox_REF2047_Margin.Text);
                double Vreg1_REF2047_Resolution = Convert.ToDouble(textBox_REF2047_Resolution.Text);
                double Vreg1_REF2047_Limit_Lv = Convert.ToDouble(textBox_REF2047_Limit_Lv.Text);
                Optic_Compensation_Stop = DP150_Black_Compensation(Vreg1_REF2047_Margin, Vreg1_REF2047_Resolution, Vreg1_REF2047_Limit_Lv);
            }

            //Dual_Get_All_Band_Gray_Gamma(); //Get All_band_gray_Gamma[12,8]

            if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false)
            {
                Get_Param(0, ref Gamma, ref Target, ref Limit, ref Extension);
                Gamma_Init.Equal_Value(Gamma);//190529
                HBM_Mode_Gray255_Compensation();
                if (checkBox_ELVSS_Comp.Checked && Optic_Compensation_Stop == false) DP150_ELVSS_Compensation();
            }

            bool Condition = true; //Condition 1

            if (Any_Band_is_Selected)
            {
                if (checkBox_Vreg1_Compensation.Checked)
                {
                    //Read Vreg1s to Textbox (For Initial Value)
                    //Update string[] B1_Vreg1_Gamma_Set1 = new string[15];
                    //Update string[] B1_Vreg1_Gamma_Set2 = new string[15];
                    button_Vreg1_Read.PerformClick();
                }
            }
            for (band = 0; band < 14; band++)
            {
                f1.GB_Status_AppendText_Nextline("Band" + (band).ToString(), Color.Green);
                if (Optic_Compensation_Stop) break;
                Gamma_Out_Of_Register_Limit = false;
                if (Band_BSQH_Selection(ref band)) //If this band is not selected , move on to the next band
                {
                    if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                    {
                        DP150_Pattern_Setting(0, band, Single_Or_Dual_Single_Is_True);
                        f1.AOD_On();
                        DP150_DBV_Setting(band);  //DBV Setting (AOD 에는 2번 DBV날리게 되도록 하기위함)
                    }

                    Vreg1_loop_count = 0; //Add on 190820
                    Vreg1_Infinite_Count = 0; //Add on 190820

                    DP150_form_Dual_engineer.Band_Radiobuttion_Select(band, true);
                    DP150_form_Dual_engineer.Band_Radiobuttion_Select(band, false);
                    Thread.Sleep(300);

                    DP150_DBV_Setting(band);

                    for (gray = 0; gray < 8; gray++)
                    {

                        if (band == 2 && gray == 0)
                        {
                            Thread.Sleep(300);
                        }

                        DP150_Pattern_Setting(gray, band, Single_Or_Dual_Single_Is_True);

                        bool Condition_1_Skip = false;
                        bool Condition_2_Skip = false;

                        DP150_DBV_Setting(band);  //DBV Setting

                        Condition = true; //Condition 1

                        DP150_form_Dual_engineer.Dual_Script_Apply(Condition); //Condition1

                        if (Condition_1_Skip == false)
                        {
                            Condition = true; //Condition 1
                            Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet

                            if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                            {
                                Vreg1 = DP150_Get_Normal_Initial_Vreg1(band, Condition); //Read Vreg1 Value
                                DP150_Update_and_Send_Vreg1(Vreg1, band, Condition);
                                Thread.Sleep(20);
                                DP150_Update_Vreg1_TextBox(Vreg1, band, true);
                                Initial_Vreg1 = Vreg1; //Add on 190603
                                Diff_Vreg1 = Vreg1 - Initial_Vreg1; //Add on 190603
                            }

                            if (Optic_Compensation_Stop) break;

                            Dual_Mode_Get_Param(gray, Condition); //Get (First)Gamma,Target,Limit From OC-Param-Table
                            f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                            if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                    || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                            {
                                DP150_form_Dual_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                            }

                            RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;

                            //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, "00", "00", "00", "00", "00", "00");
                            Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022

                            Thread.Sleep(300); //Pattern 안정화 Time

                            DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
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
                                        DP150_form_Dual_engineer.Get_Gamma_Only_DP150(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                        Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                    }
                                    RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                    //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                    Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022

                                    Measure.Set_Value(0, 0, 0);
                                    Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                        + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                    Optic_Compensation_Succeed = true;
                                    break;
                                }

                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                        || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                {
                                    if (Vreg1_loop_count == 0)
                                    {
                                        DP150_form_Dual_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
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

                                        
                                        DP150_Update_and_Send_Vreg1(Vreg1, band, true);
                                        DP150_Update_and_Send_Vreg1(Vreg1, band, false); ;
                                        Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                        DP150_Update_Vreg1_TextBox(Vreg1, band, true);
                                        DP150_Update_Vreg1_TextBox(Vreg1, band, false);
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
                                    //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                    Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
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



                                DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                if (checkBox_Dual_Mode_Copy_Mea_To_Target.Checked) DP150_form_Dual_engineer.Dual_Copy_C1Measure_To_C2Target(band, gray);
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();
                            }

                            f1.GB_ProgressBar_PerformStep();
                            ///////////////////////////Condition 1 Over
                        }






                        if (band == 0 || band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                               || band == 6 || band == 7 || band == 8 || band == 9 || band == 10)
                        {
                            ///////////////////////////Condition 2 Start
                            //form_dual_engineer.Band_Radiobuttion_Select(band, Condition);//Select Band (Delete on 190605)
                            //DP116_DBV_Setting(band);  //DBV Setting
                            Condition = false; //Condition 2

                            DP150_form_Dual_engineer.Dual_Script_Apply(Condition);//Condition 2

                            if (Condition_2_Skip == false)
                            {
                                Condition = false; //Condition 2
                          
                                if (checkBox_Dual_Mode_Gamma_Copy.Checked)
                                    DP150_form_Dual_engineer.Dual_Mode_Gamma_Copy(band, gray, Target.double_Lv); //Offset 이 아니라 Copy 임 (Left to Right)

                                Vreg1_loop_count = 0; //Add on 191031
                                Vreg1_Infinite_Count = 0; //Add on 191031

                                DP150_form_Dual_engineer.Dual_Cal_Diff_R_L_Gamma();

                                DP150_form_Dual_engineer.Dual_Update_Viewer_Sheet_form_OC_Sheet(band, Condition);
                                Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet


                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                   || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked)
                                {
                                    Vreg1 = DP150_Get_Normal_Initial_Vreg1(band, Condition); //Read Vreg1 Value (Gamma_Set 2's Vreg1 Value = Gamma_Set 1's Vreg1 Value);
                                    DP150_Update_and_Send_Vreg1(Vreg1, band, Condition); //Add on 190702 (Condition 1 꺼를 읽은거기 때문에 먼저 Condition2 꺼에 초기 Vreg1 세팅 필요)
                                    Thread.Sleep(20);
                                    DP150_Update_Vreg1_TextBox(Vreg1, band, false);
                                    Initial_Vreg1 = Vreg1; //Add on 190603
                                    Diff_Vreg1 = Vreg1 - Initial_Vreg1; //Add on 190603
                                }

                                if (Optic_Compensation_Stop) break;
                                Dual_Mode_Get_Param(gray, Condition); //Get (First)Gamma,Target,Limit From OC-Param-Table   
                                f1.GB_Status_AppendText_Nextline("Limit x/y/Lv = " + Limit.double_X.ToString() + "/" + Limit.double_Y.ToString() + "/" + Limit.double_Lv.ToString(), Color.Red);

                                if ((band == 1 || band == 2 || band == 3 || band == 4 || band == 5
                                   || band == 6 || band == 7 || band == 8 || band == 9 || band == 10) && gray == 0 && checkBox_Vreg1_Compensation.Checked && Vreg1_loop_count == 0)
                                {
                                    DP150_form_Dual_engineer.Get_Gamma_Only_DP150(band - 1, 0, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                    Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                }

                                RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022

                                DP150_form_Dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                                //Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); Delete on 190614
                                Thread.Sleep(100);
                                //f1.CA_Measure_button_Perform_Click(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv); //Get (First)Measure
                                DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count
                                    , Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                loop_count = 0;
                                Infinite_Count = 0;
                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); //Add on 190614
                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                Application.DoEvents();

                                Optic_Compensation_Succeed = false;
                                Within_Spec_Limit = false;


                                DP150_uvL_Skip dp150_uvl_skip = new DP150_uvL_Skip(band,gray);
                                if (dp150_uvl_skip.Is_Mode2_OC_Skip_Within_UVL())
                                    continue;


                                while (Optic_Compensation_Succeed == false && Optic_Compensation_Stop == false)
                                {
                                    if (Target.double_Lv < Skip_Lv)
                                    {
                                        if (band >= 1)
                                        {
                                            DP150_form_Dual_engineer.Get_Gamma_Only_DP150(band - 1, gray, ref Prev_Band_Gray255_Gamma.int_R, ref Prev_Band_Gray255_Gamma.int_G, ref Prev_Band_Gray255_Gamma.int_B, Condition);
                                            Gamma.Equal_Value(Prev_Band_Gray255_Gamma);
                                        }
                                        RGB_Need_To_Change[0] = true; RGB_Need_To_Change[1] = true; RGB_Need_To_Change[2] = true;
                                        //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                        Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                        Measure.Set_Value(0, 0, 0);
                                        Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, "X", Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                        f1.GB_Status_AppendText_Nextline("B" + band.ToString() + "/G" + gray.ToString()
                                            + " Compensation Skip (Target Lv : " + Target.double_Lv.ToString() + ") < " + Skip_Lv.ToString(), System.Drawing.Color.Blue);
                                        Optic_Compensation_Succeed = true;
                                        break;
                                    }

                                    Vreg1_Need_To_Be_Updated = false;

                                    Prev_Gamma.Equal_Value(Gamma);
                                    Infinite_Loop_Check(loop_count);
                                    if (checkBox_CCT_Comp.Checked)
                                    {
                                        f1.GB_Status_AppendText_Nextline("CCT Comp Applied", Color.Black);
                                        Imported_my_cpp_dll.CCT_Sub_Compensation(loop_count, Infinite, Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);
                                    }
                                    else
                                    {
                                        Imported_my_cpp_dll.Sub_Compensation(loop_count, Infinite, ref Infinite_Count, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv
                                            , Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y, Gamma_Register_Limit, ref Gamma_Out_Of_Register_Limit, ref Within_Spec_Limit);
                                    }
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
                                    // }

                                    if (Vreg1_Need_To_Be_Updated == false)
                                    {
                                        if (Math.Abs(Diff_Gamma.int_R) > 0) RGB_Need_To_Change[0] = true;
                                        else RGB_Need_To_Change[0] = false;
                                        if (Math.Abs(Diff_Gamma.int_G) > 0) RGB_Need_To_Change[1] = true;
                                        else RGB_Need_To_Change[1] = false;
                                        if (Math.Abs(Diff_Gamma.int_B) > 0) RGB_Need_To_Change[2] = true;
                                        else RGB_Need_To_Change[2] = false;
                                        //Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, "00", "00", "00", "00", "00", "00"); //Setting Gamma Values
                                        Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                        DP150_form_Dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                                        Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                    }

                                    if (Within_Spec_Limit)
                                    {
                                        if (Target.double_Lv < Convert.ToDouble(textBox_OC_Mode1_Green_Offset_Min_Lv.Text) && band > 0)
                                        {
                                            Dual_Get_All_Band_Gray_Gamma(Condition);
                                            if ((Gamma.int_G - All_band_gray_Gamma[band - 1, gray].int_G) == 1)
                                            {
                                                Gamma.int_G++;
                                            
                                                if (Gamma.int_G > 511)
                                                {
                                                    Optic_Compensation_Stop = true;
                                                    System.Windows.Forms.MessageBox.Show("Gamma/Vreg1 is out of Limit");
                                                    break;
                                                }

                                                Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, Gamma, band, gray, Condition, R_AM1_Hex, "00", G_AM1_Hex, "00", B_AM1_Hex, "00"); //Add on 191022
                                                DP150_form_Dual_engineer.Dual_Cal_Diff_R_L_Gamma();
                                                Thread.Sleep(20); //To Verify Successfully this cannot deleted ! (Stable time for Gamma Setting)
                                                DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                                loop_count--;
                                                Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                                f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                                Application.DoEvents();
                                            }
                                        }

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

                                    DP150_Measure_Average(ref Measure.double_X, ref Measure.double_Y, ref Measure.double_Lv, ref total_average_loop_count, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, Extension.double_X, Extension.double_Y);
                                    Dual_Mode_Update_Engineering_Sheet(band, gray, loop_count, Extension_Applied, Condition); // Current-Gamma,Measure (+ update engineering mode 2nd-sheet from 1st-sheet)
                                    f1.Set_And_2D_Drawing_Target_Measure_Limit_XYLv(Target, Measure, Limit, Extension);
                                    Application.DoEvents();
                                }
                                f1.GB_ProgressBar_PerformStep();
                            }
                        }
                    }
                    if (band == 11 || band == 12 || band == 13) //AOD1,2,3
                        f1.AOD_Off();
                    // Dual_Get_All_Band_Gray_Gamma(Condition); //update "All_band_gray_Gamma[10,12]" from the engineering mode 2nd-sheet
                }
            }
            DP150_form_Dual_engineer.Dual_Cal_Diff_R_L_Gamma();
            f1.OC_Timer_Stop();

            DP150_DBV_Setting(1);  //DBV Setting    
            DP150_Pattern_Setting(0, 1, Single_Or_Dual_Single_Is_True);
            if (Optic_Compensation_Succeed) System.Windows.Forms.MessageBox.Show("Optic Compensation Finish !");
            DP150_form_Dual_engineer.Dual_Engineering_Mode_DataGridview_ReadOnly(false);
        }


  

        private void DP150_Normal_Vinit_Setting(int Band, double Vinit, string[] DP150_Vinit)
        {
            int Current_Dec = Convert.ToInt16(30 - 10 * (Vinit + 2.8));
            DP150_Vinit[Band] = Current_Dec.ToString("X2");
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
            else if (checkBox_Band13.Checked == false && band == 12)
                return false;
            else if (checkBox_Band14.Checked == false && band == 13)
                return false;
            else
                return true;
        }

        private void DP150_Update_Vreg1_TextBox(int Vreg1_int, int band, int Gamma_Set)
        {
            if (Gamma_Set == 1)
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
                else if (band == 10)
                    textBox_Vreg1_B10.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else
                {
                    //Do nothing 
                }
            }
            else if (Gamma_Set == 2)
            {
                if (band == 0)
                    textBox_Vreg1_B0_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 1)
                    textBox_Vreg1_B1_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 2)
                    textBox_Vreg1_B2_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 3)
                    textBox_Vreg1_B3_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 4)
                    textBox_Vreg1_B4_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 5)
                    textBox_Vreg1_B5_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 6)
                    textBox_Vreg1_B6_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 7)
                    textBox_Vreg1_B7_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 8)
                    textBox_Vreg1_B8_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 9)
                    textBox_Vreg1_B9_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 10)
                    textBox_Vreg1_B10_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else
                {
                    //Do nothing 
                }
            }
            else if (Gamma_Set == 3)
            {
                if (band == 0)
                    textBox_Vreg1_B0_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 1)
                    textBox_Vreg1_B1_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 2)
                    textBox_Vreg1_B2_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 3)
                    textBox_Vreg1_B3_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 4)
                    textBox_Vreg1_B4_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 5)
                    textBox_Vreg1_B5_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 6)
                    textBox_Vreg1_B6_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 7)
                    textBox_Vreg1_B7_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 8)
                    textBox_Vreg1_B8_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 9)
                    textBox_Vreg1_B9_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 10)
                    textBox_Vreg1_B10_3.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else
                {
                    //Do nothing 
                }
            }
            else
            {
                //Do nothing 
            }

            Application.DoEvents();
        }


        private void DP150_Update_Vreg1_TextBox(int Vreg1_int, int band, bool Gamma_Set1)
        {
            if (Gamma_Set1)
            {
                if (band == 0)
                    textBox_Vreg1_B0.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 1)
                    textBox_Vreg1_B1.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 2)
                    textBox_Vreg1_B2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 3)
                    textBox_Vreg1_B3.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 4)
                    textBox_Vreg1_B4.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 5)
                    textBox_Vreg1_B5.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 6)
                    textBox_Vreg1_B6.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 7)
                    textBox_Vreg1_B7.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 8)
                    textBox_Vreg1_B8.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 9)
                    textBox_Vreg1_B9.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 10)
                    textBox_Vreg1_B10.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else
                {
                    //Do nothing 
                }
            }
            else
            {
                if (band == 0)
                    textBox_Vreg1_B0_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 1)
                    textBox_Vreg1_B1_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 2)
                    textBox_Vreg1_B2_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 3)
                    textBox_Vreg1_B3_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 4)
                    textBox_Vreg1_B4_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 5)
                    textBox_Vreg1_B5_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 6)
                    textBox_Vreg1_B6_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 7)
                    textBox_Vreg1_B7_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 8)
                    textBox_Vreg1_B8_2.Text = Vreg1_int.ToString() ; //Read Vreg1 Value
                else if (band == 9)
                    textBox_Vreg1_B9_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else if (band == 10)
                    textBox_Vreg1_B10_2.Text = Vreg1_int.ToString(); //Read Vreg1 Value
                else
                {
                    //Do nothing 
                }
            }
            Application.DoEvents();
        }

        public void Groupbox35_Hide()
        {
            this.groupBox35.Hide();
        }

        public void Groupbox35_Show()
        {
            this.groupBox35.Show();
        }

        private void Optic_compensation_Start_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            TextBox_Vreg1_Clear();
            Textbox_ELVSS_Clear();
            textBox_Total_Average_Meas_Count.Text = "0";

            R_AM1_Hex = textBox_R_AM1_Hex.Text;
            G_AM1_Hex = textBox_G_AM1_Hex.Text;
            B_AM1_Hex = textBox_B_AM1_Hex.Text;
            
            if (radioButton_Single_Mode.Checked||radioButton_Single_Mode_90Hz.Checked
                || radioButton_Single_Mode_60hz_Set123.Checked || radioButton_Single_Mode_90hz_Set123.Checked) //Single Mode
            {
                Single_Or_Dual_Single_Is_True = true;
                DP150_Single_Engineerig_Mornitoring_Mode.getInstance().Show();
                DP150_Single_Engineerig_Mornitoring_Mode DP150_form_engineer = (DP150_Single_Engineerig_Mornitoring_Mode)Application.OpenForms["DP150_Single_Engineerig_Mornitoring_Mode"];
                DP150_form_engineer.Band_Radiobuttion_Select(0);
                DP150_form_engineer.RadioButton_All_Enable(false);
                
                if (radioButton_Single_Mode_90Hz.Checked || radioButton_Single_Mode_90hz_Set123.Checked)
                    f1.IPC_Quick_Send("mipi.write 0x39 0x76 0x01");
                else if (radioButton_Single_Mode.Checked || radioButton_Single_Mode_60hz_Set123.Checked)
                    f1.IPC_Quick_Send("mipi.write 0x39 0x76 0x00");

                if (radioButton_Single_Mode_60hz_Set123.Checked || radioButton_Single_Mode_90hz_Set123.Checked) Single_Mode_Optic_compensation_Gamma_Vreg1_Set_123();
                else Single_Mode_Optic_compensation();
                
                DP150_form_engineer.RadioButton_All_Enable(true);
            }

            else //Dual Mode
            {
                Single_Or_Dual_Single_Is_True = false; //Dual
                DP150_Dual_Engineering_Mornitoring_Mode.getInstance().Show();
                DP150_Dual_Engineering_Mornitoring_Mode DP150_form_Dual_engineer = (DP150_Dual_Engineering_Mornitoring_Mode)Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
                DP150_form_Dual_engineer.Band_Radiobuttion_Select(0, true); //Select Band as 0 (Condition 1)
                DP150_form_Dual_engineer.Band_Radiobuttion_Select(0, false); //Select Band as 0 (Condition 2)

                DP150_form_Dual_engineer.Dual_RadioButton_All_Enable(false);
                             
                Dual_Mode_Optic_Compensation();

                DP150_form_Dual_engineer.Dual_RadioButton_All_Enable(true);
            }
        }

        private void button_Dll_Info_Click(object sender, EventArgs e)
        {
            IntPtr ptr = Imported_my_cpp_dll.Get_Dll_Information();
            string Message = Marshal.PtrToStringAnsi(ptr);
            System.Windows.MessageBox.Show(Message);
        }

        private void button_BSQH_Stop_Click(object sender, EventArgs e)
        {
            Optic_Compensation_Stop = true;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.OC_Timer_Stop();
        }

        private void button_Read_REF2047_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.MX_OTP_Read(21, 1, "B1");
            string Vreg1_REF2047 = f1.dataGridView1.Rows[0].Cells[1].Value.ToString();

            int REF2047 = Convert.ToInt16(Vreg1_REF2047, 16) & 0x7F;
            Vreg1_REF2047 = REF2047.ToString("X2");
            double REF2047_voltage = 1.92 + (REF2047 * 0.04);

            this.textBox_REF2047.Text = REF2047_voltage.ToString() + "v" + " (" + Vreg1_REF2047 + "h)";          
        }

        private void checkBox_Read_DBV_Values_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button_Vreg1_Read.PerformClick();

            int Test_Vreg1_int = 555;
            int Test_Band = 0;
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Test_Vreg1_int/Test_Band : " + Test_Vreg1_int.ToString() + "/" + Test_Band.ToString(), Color.Blue);
            DP150_Update_and_Send_Vreg1(Test_Vreg1_int, Test_Band, true);

            Test_Vreg1_int = 1111;
            Test_Band = 0;
            f1.GB_Status_AppendText_Nextline("Test_Vreg1_int/Test_Band : " + Test_Vreg1_int.ToString() + "/" + Test_Band.ToString(), Color.Blue);
            DP150_Update_and_Send_Vreg1(Test_Vreg1_int, Test_Band, true);

            Test_Vreg1_int = 2222;
            Test_Band = 0;
            f1.GB_Status_AppendText_Nextline("Test_Vreg1_int/Test_Band : " + Test_Vreg1_int.ToString() + "/" + Test_Band.ToString(), Color.Blue);
            DP150_Update_and_Send_Vreg1(Test_Vreg1_int, Test_Band, true);

            Test_Vreg1_int = 555;
            Test_Band = 1;
            f1.GB_Status_AppendText_Nextline("Test_Vreg1_int/Test_Band : " + Test_Vreg1_int.ToString() + "/" + Test_Band.ToString(), Color.Blue);
            DP150_Update_and_Send_Vreg1(Test_Vreg1_int, Test_Band, true);

            Test_Vreg1_int = 1111;
            Test_Band = 1;
            f1.GB_Status_AppendText_Nextline("Test_Vreg1_int/Test_Band : " + Test_Vreg1_int.ToString() + "/" + Test_Band.ToString(), Color.Blue);
            DP150_Update_and_Send_Vreg1(Test_Vreg1_int, Test_Band, true);

            Test_Vreg1_int = 2222;
            Test_Band = 1;
            f1.GB_Status_AppendText_Nextline("Test_Vreg1_int/Test_Band : " + Test_Vreg1_int.ToString() + "/" + Test_Band.ToString(), Color.Blue);
            DP150_Update_and_Send_Vreg1(Test_Vreg1_int, Test_Band, true); 
        }

        private void Dual_Mode_Update_Engineering_Sheet(int band, int gray, int loop_count, string Extension_Applied, bool Condition)
        {
            //Conditon = true : Condition 1
            //Conditon = false : Condition 2
            DP150_Dual_Engineering_Mornitoring_Mode Dual_Mode_form_Engineer = (DP150_Dual_Engineering_Mornitoring_Mode)Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
            Dual_Mode_form_Engineer.Set_OC_Param_DP150(gray, Gamma.int_R, Gamma.int_G, Gamma.int_B, Measure.double_X, Measure.double_Y, Measure.double_Lv, loop_count, Extension_Applied, Condition);
            Dual_Mode_form_Engineer.Updata_Sub_To_Main_GridView(band, gray, Condition);
        }

        private void Gamma_read_btn_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button_IRC_Box_Pattern_Measure_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            for (int i = 256; i >= 0; )
            {
                int Gray = i;
                if (Gray > 255) Gray = 255;

                if (radioButton_White_Back_Ground_Up_Box.Checked)f1.DP150_IRC_Box_PTN_update(Gray, Gray, Gray, true);
                else f1.DP150_IRC_Box_PTN_update(Gray, Gray, Gray, false);
                Thread.Sleep(300);
                f1.Measure_Indicate_Gray(Gray);
                i -= 16;
            }
        }

        private void checkBox_OC_APL_Change_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_OC_APL_Change.Checked)
            {
              
                checkBox_OC_IRC_Box_PTN.Checked = false;
                checkBox_OC_APL_Change_HBM_Only.Checked = false;
                checkBox_G127_Before_Main_Pattern.Checked = false;
                checkBox_OC_APL_Change_Normal.Checked = false;
            }
        }

        private void checkBox_OC_IRC_Box_PTN_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_OC_IRC_Box_PTN.Checked)
            {
                //checkBox_OC_IRC_Box_PTN.Checked = false;
                checkBox_OC_APL_Change.Checked = false;
                checkBox_OC_APL_Change_HBM_Only.Checked = false;
                checkBox_G127_Before_Main_Pattern.Checked = false;
                checkBox_OC_APL_Change_Normal.Checked = false;
            }
        }

        private void button_Gamma_Set_1_Click(object sender, EventArgs e)
        {
            DP150_Gamma_Set(1, true);
        }

        private void button_Gamma_Set_2_Click(object sender, EventArgs e)
        {
            DP150_Gamma_Set(2, true);
        }

        private void button_Gamma_Set_3_Click(object sender, EventArgs e)
        {
            DP150_Gamma_Set(3, true);
        }

        private void button_Gamma_Set_90Hz_1_Click(object sender, EventArgs e)
        {
            DP150_Gamma_Set(1, false);
        }

        private void button_Gamma_Set_90Hz_2_Click(object sender, EventArgs e)
        {
            DP150_Gamma_Set(2, false);
        }

        private void button_Gamma_Set_90Hz_3_Click(object sender, EventArgs e)
        {
            DP150_Gamma_Set(3, false);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox15_Enter(object sender, EventArgs e)
        {

        }

        private void button_GCS_Selected_Band_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_OC_APL_Change_HBM_Only_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_OC_APL_Change_HBM_Only.Checked)
            {
                checkBox_OC_IRC_Box_PTN.Checked = false;
                checkBox_OC_APL_Change.Checked = false;
                checkBox_G127_Before_Main_Pattern.Checked = false;
            }
        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox_G127_Before_Main_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_G127_Before_Main_Pattern.Checked)
            {
                checkBox_OC_IRC_Box_PTN.Checked = false;
                checkBox_OC_APL_Change.Checked = false;
                checkBox_OC_APL_Change_HBM_Only.Checked = false;
                //checkBox_G127_Before_Main_Pattern.Checked = false;
                checkBox_OC_APL_Change_Normal.Checked = false;
            }
        }

        private void checkBox_OC_APL_Change_Normal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_OC_APL_Change_Normal.Checked)
            {
                checkBox_OC_IRC_Box_PTN.Checked = false;
                checkBox_OC_APL_Change.Checked = false;
                //checkBox_OC_APL_Change_HBM_Only.Checked = false;
                checkBox_G127_Before_Main_Pattern.Checked = false;
            }
        }

        public void DP150_Measure_Average(ref double Measured_X, ref double Measured_Y, ref double Measured_Lv, ref int total_average_loop_count
            , double Target_X, double Target_Y, double Target_Lv, double Limit_X, double Limit_Y, double Limit_Lv, double Extension_X, double Extension_Y)
        {
            Second_Model_Option_Form form_Second_Model = (Second_Model_Option_Form)System.Windows.Forms.Application.OpenForms["Second_Model_Option_Form"];
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
            if (form_Second_Model.checkBox_Ave_Apply_Ratio.Checked)
            {
                Ratio = Convert.ToDouble(form_Second_Model.textBox_Ave_Ratio.Text);
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

                if (form_Second_Model.checkBox_Ave_Measure.Checked && f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_Second_Model.textBox_Ave_Lv_Limit.Text)
                    && ((Tolerance_X * Ratio) > Diff_X) && ((Tolerance_Y * Ratio) > Diff_Y) && ((Tolerance_Lv * Ratio) > Diff_Lv))
                {
                    f1().GB_Status_AppendText_Nextline(f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_Second_Model.textBox_Ave_Lv_Limit.Text, Color.Blue);
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

                    form_Second_Model.textBox_Total_Average_Meas_Count.Text = (++total_average_loop_count).ToString();
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
