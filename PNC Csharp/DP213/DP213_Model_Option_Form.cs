using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices; // (Dll 사용위해 필요)
//using References
using System.Xml.Serialization;
using BSQH_Csharp_Library;


namespace PNC_Csharp
{
    public partial class DP213_Model_Option_Form : Form
    {
        //Set2 Diff(Between Set1 and Set2)Delta L Spec
        public double[,] OC_Mode23_Diff_Delta_L_Spec = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];//for Dual (HBM + Normal : 11ea bands)
        public double[,] OC_Mode23_Diff_Delta_UV_Spec = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];//for Dual (HBM + Normal : 11ea bands)

        DP213_CMDS_Write_Read_Update_Variables cmds;
        DP213_OC_Values_Storage storage;
        DP213_OC_Current_Variables_Structure vars;
        DP213_OC_Variables_Update_Algorithm_Interface vars_update;
        DP213_Script_Update script_update;
        DP213_CRC_Check crcs;
        DP213_FlashMemory_For_User flashmemory;

        private Form1 f1() { return (Form1)Application.OpenForms["Form1"]; }
        protected DP213_Dual_Engineering_Mornitoring dp213_mornitoring() { return (DP213_Dual_Engineering_Mornitoring)Application.OpenForms["DP213_Dual_Engineering_Mornitoring"]; }

        private static DP213_Model_Option_Form Instance;
        public static DP213_Model_Option_Form getInstance()
        {
            if (Instance == null)
                Instance = new DP213_Model_Option_Form();

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

        private DP213_Model_Option_Form()
        {
            InitializeComponent();
            update_setting();
        }

        private void DP213_Model_Option_Form_Load(object sender, EventArgs e)
        {
            BackColor = f1().current_model.Get_Back_Ground_Color();
            Get_Instances();
            button_Load_Setting.PerformClick();
            RGB_Vdata_Grid_Initalize();
        }

        private void Get_Instances()
        {
            cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            storage = DP213_OC_Values_Storage.getInstance();
            vars = DP213_OC_Current_Variables_Structure.getInstance();
            vars_update = DP213_OC_Variables_Update_Algorithm.getInstance();
            script_update = DP213_Script_Update.getInstance();
            crcs = DP213_CRC_Check.getInstance(Get_OC_Mode_Set(OC_Mode.Mode1));
            flashmemory = DP213_FlashMemory.getInstance();
        }


        private void RGB_Vdata_Grid_Read_Initalize()
        {
            for (int column = 0; column < 7; column++) dataGridView_RGB_Vdata_Read.Columns.Add("", "");
            dataGridView_RGB_Vdata_Read.Columns[0].Width = 40;

            dataGridView_RGB_Vdata_Read.Rows.Add("-", "GM_R", "GM_G", "GM_B", "R(v)", "G(v)", "B(v)");
            dataGridView_RGB_Vdata_Read.Rows.Add("G255", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G191", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G127", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G95", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G63", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G47", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G31", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G23", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G15", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G7", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("G1", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("AM1", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("AM0", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("Vreg1", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("VREF4095", "", "", "", "", "", "");
            dataGridView_RGB_Vdata_Read.Rows.Add("VREF0", "", "", "", "", "", "");
        }

        private void RGB_Vdata_Grid_Write_Initalize()
        {
            for (int column = 0; column < 4; column++) dataGridView_RGB_Vdata_Write.Columns.Add("", "");
            dataGridView_RGB_Vdata_Write.Columns[0].Width = 40;

            dataGridView_RGB_Vdata_Write.Rows.Add("-", "GM_R", "GM_G", "GM_B");
            dataGridView_RGB_Vdata_Write.Rows.Add("G255", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G191", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G127", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G95", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G63", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G47", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G31", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G23", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G15", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G7", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("G1", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("AM1", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("AM0", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("Vreg1", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("VREF4095", "", "", "");
            dataGridView_RGB_Vdata_Write.Rows.Add("VREF0", "", "", "");
        }

        private void RGB_Vdata_Grid_Initalize()
        {
            RGB_Vdata_Grid_Read_Initalize();
            RGB_Vdata_Grid_Read_Tema_Change();

            RGB_Vdata_Grid_Write_Initalize();
            RGB_Vdata_Grid_Write_Tema_Change();
        }

        private void RGB_Vdata_Grid_Read_Tema_Change()
        {
            dataGridView_RGB_Vdata_Read.Columns[0].Width = 40;
            for (int i = 1; i <= 3; i++) //Gamma R,G,B
            {
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
            foreach (DataGridViewColumn column in this.dataGridView_RGB_Vdata_Read.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void RGB_Vdata_Grid_Write_Tema_Change()
        {
            dataGridView_RGB_Vdata_Write.Columns[0].Width = 40;
            for (int i = 1; i <= 3; i++) //Gamma R,G,B
            {
                dataGridView_RGB_Vdata_Write.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_RGB_Vdata_Write.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_RGB_Vdata_Write.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_RGB_Vdata_Write.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_RGB_Vdata_Write.Columns[i].Width = 55;
            }
            foreach (DataGridViewColumn column in this.dataGridView_RGB_Vdata_Write.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }


  


        
        private void button_B0_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B0_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band0 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B1_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B1_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band1 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B2_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B2_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band2 DBV Setting", System.Drawing.Color.Black);
        }


        public void button_B3_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B3_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band3 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B4_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B4_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band4 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B5_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B5_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band5 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B6_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B6_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band6 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B7_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B7_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band7 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_B8_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B8_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band8 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B9_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B9_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band9 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B10_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B10_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band10 DBV Setting", System.Drawing.Color.Black);
        }

        private void button_B11_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_B11_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("Band11 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_A0_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_AOD0_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("AOD0 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_A1_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_AOD1_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("AOD1 DBV Setting", System.Drawing.Color.Black);
        }

        public void button_A2_DBV_Send_Click(object sender, EventArgs e)
        {
            f1().DBV_Setting(textBox_AOD2_DBV_Setting.Text);
            f1().GB_Status_AppendText_Nextline("AOD2 DBV Setting", System.Drawing.Color.Black);
        }
        private void Read_Normal_DBV()
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_Normal_Read_DBV_CMD();
            f1().MX_OTP_Read(output);
            Thread.Sleep(200);

            byte[] pamaeters = f1().Get_Read_Byte_Array(output[2]);            
            textBox_B0_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,0).ToString("X3");
            textBox_B1_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,1).ToString("X3");
            textBox_B2_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,2).ToString("X3");
            textBox_B3_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,3).ToString("X3");
            textBox_B4_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,4).ToString("X3");
            textBox_B5_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,5).ToString("X3");
            textBox_B6_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,6).ToString("X3");
            textBox_B7_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,7).ToString("X3");
            textBox_B8_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,8).ToString("X3");
            textBox_B9_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,9).ToString("X3");
            textBox_B10_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,10).ToString("X3");
            textBox_B11_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(pamaeters,11).ToString("X3");
          }

        private void Read_AOD_DBV()
        {
            byte[] output = ModelFactory.Get_DP213_Instance().Get_AOD_Read_DBV_CMD();
            f1().MX_OTP_Read(output);
            Thread.Sleep(200);

            byte[] pamaeters = f1().Get_Read_Byte_Array(output[2]);
            textBox_AOD0_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_AOD_DBV(pamaeters,0).ToString("X3");
            textBox_AOD1_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_AOD_DBV(pamaeters,1).ToString("X3");
            textBox_AOD2_DBV_Setting.Text = ModelFactory.Get_DP213_Instance().Get_AOD_DBV(pamaeters,2).ToString("X3");
        }

        public void DP213_DBV_Setting(int band)
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
                    button_B11_DBV_Send.PerformClick();
                    break;
                case 12:
                    button_A0_DBV_Send.PerformClick();
                    break;
                case 13:
                    button_A1_DBV_Send.PerformClick();
                    break;
                case 14:
                    button_A2_DBV_Send.PerformClick();
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Band is out of boundary");
                    break;
            }
            Thread.Sleep(100);
        }



        private void button_Read_DP213_DBV_Setting_Click(object sender, EventArgs e)
        {
            Read_DBV();
        }

        public void Read_DBV()
        {
            try
            {
                Read_Normal_DBV();
                Read_AOD_DBV();
            }
            catch
            {
                System.Windows.MessageBox.Show("DBV Value Read fail");
            }
        }

        private void button_Select_All_Band_Click(object sender, EventArgs e)
        {
            bool All_Checked = true;
            All_Band_Checkbox_Status(All_Checked);
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
            checkBox_Band11.Checked = Checked;
            checkBox_AOD0.Checked = Checked;
            checkBox_AOD1.Checked = Checked;
            checkBox_AOD2.Checked = Checked;
        }

        private void button_Deselect_All_Band_Click(object sender, EventArgs e)
        {
            bool All_Checked = false;
            All_Band_Checkbox_Status(All_Checked);
        }

        private void button_Vreg1_Read_Click(object sender, EventArgs e)
        {
            cmds.Vreg1_Text_Clear();
            cmds.Read_Dec_Vreg1_and_Save_to_Textbox();
        }

        public bool Get_IS_G2G_On()
        {
            if (radioButton_G2G_On.Checked) return true;
            else return false;
        }

        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public void Groupbox35_Hide()
        {
            this.groupBox_OC_Main_Setting.Hide();
        }


        public void Groupbox35_Show()
        {
            this.groupBox_OC_Main_Setting.Show();
        }
        
        private void button_Dll_Info_Click(object sender, EventArgs e)
        {
            IntPtr ptr = Imported_my_cpp_dll.Get_Dll_Information();
            string Message = Marshal.PtrToStringAnsi(ptr);
            System.Windows.MessageBox.Show(Message);
        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        

        


     

        public bool Band_BSQH_Selection(int band)
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
            else if (checkBox_Band11.Checked == false && band == 11)
                return false;
            else if (checkBox_AOD0.Checked == false && band == 12)
                return false;
            else if (checkBox_AOD1.Checked == false && band == 13)
                return false;
            else if (checkBox_AOD2.Checked == false && band == 14)
                return false;
            else
                return true;
        }


        public double[] DP213_Get_Pattern_Grays(OC_Mode Mode, int band)
        {
            double[] Grays = new double[DP213_Static.Max_Gray_Amount];
            for(int gray = 0;gray< DP213_Static.Max_Gray_Amount;gray++)
            {
                Grays[gray] = (double)Get_Pattern_Gray(Mode, gray, band);
            }
            return Grays;
        }



        public void DP213_Pattern_Setting(OC_Mode Mode, int gray, int band)
        {
            int Pattern_Gray = Get_Pattern_Gray(Mode, gray, band);
            Display_Pattern(band, Pattern_Gray);
        }

        private int Get_Pattern_Gray(OC_Mode Mode, int gray, int band)
        {
            int Pattern_Gray = 255;
            
            if (checkBox_Special_Gray_Compensation.Checked)
                Pattern_Gray = Get_Pattern_Gray_by_Special_OC(Mode, gray, band);
            else
                Pattern_Gray = DP213_Static.Get_Pattern_Gray_by_index_gray(gray);
            
            return Pattern_Gray;
        }

        private int Get_Pattern_Gray_by_Special_OC(OC_Mode Mode, int gray, int band)
        {
            string Band_Gray = dp213_mornitoring().Dual_Get_BX_GXXX_By_Gray(band,gray, Mode);
            int Pattern_Gray;

            if (band == 10 || band == 11) Pattern_Gray = Convert.ToInt32(Band_Gray.Remove(0, 5));//B10/11_G255 --> 255
            else Pattern_Gray = Convert.ToInt32(Band_Gray.Remove(0, 4)); //ex) B0/A0_G255 --> 255

            f1().GB_Status_AppendText_Nextline("(special OC)Band_Gray : " + Band_Gray + " / Gray : " + Pattern_Gray.ToString(), System.Drawing.Color.Red);
            return Pattern_Gray;
        }

        private void Display_Pattern(int band, int Pattern_Gray)
        {
            //AOD Mode Pattern
            if (band == 12 || band == 13 || band == 14) 
            {
                f1().Image_Crosstalk(f1().current_model.get_AOD_X(), f1().current_model.get_AOD_Y(), 0, 0, 0, Pattern_Gray, Pattern_Gray, Pattern_Gray);
                f1().GB_Status_AppendText_Nextline("AOD Gray" + Pattern_Gray.ToString() + " Setting", System.Drawing.Color.Black);
            }
            //Normal Mode Pattern
            else 
            {
                //f1().PTN_update(Pattern_Gray, Pattern_Gray, Pattern_Gray);

                int Res_X =  Convert.ToInt32(textBox_X_Resolution.Text);
                if (Res_X > 1228) Res_X = 1228;
                if (Res_X < 10) Res_X = 10;

                int Res_Y = Convert.ToInt32(textBox_Y_Resolution.Text);
                if (Res_Y > 2700) Res_Y = 2700;
                if (Res_Y < 10) Res_Y = 10;

                f1().Image_Crosstalk(Res_X, Res_Y, 0, 0,0, Pattern_Gray, Pattern_Gray, Pattern_Gray);


                f1().GB_Status_AppendText_Nextline("Gray" + Pattern_Gray.ToString() + " Setting", System.Drawing.Color.Black);
            }
        }

        public double Get_OC_Skip_Lv()
        {
            return Convert.ToDouble(textBox_Subcompensation_GB_skip_Lv.Text);
        }
       

        private NumericUpDown Get_NumericUpDown(OC_Mode Mode)
        {
            if (Mode == OC_Mode.Mode1) return numericUpDown_Set_Mode_1;
            else if (Mode == OC_Mode.Mode2) return numericUpDown_Set_Mode_2;
            else if (Mode == OC_Mode.Mode3) return numericUpDown_Set_Mode_3;
            else if (Mode == OC_Mode.Mode4) return numericUpDown_Set_Mode_4;
            else if (Mode == OC_Mode.Mode5) return numericUpDown_Set_Mode_5;
            else return numericUpDown_Set_Mode_6;
        }

        private Gamma_Set GetGammaSet(NumericUpDown numericupdown)
        {
            if (numericupdown.Value == 1)
                return Gamma_Set.Set1;
            else if (numericupdown.Value == 2)
                return Gamma_Set.Set2;
            else if (numericupdown.Value == 3)
                return Gamma_Set.Set3;
            else if (numericupdown.Value == 4)
                return Gamma_Set.Set4;
            else if (numericupdown.Value == 5)
                return Gamma_Set.Set5;
            else
                return Gamma_Set.Set6;
        }

        public Gamma_Set Get_OC_Mode_Set(OC_Mode Mode)
        {
            NumericUpDown numericUpDown_Set_Mode = Get_NumericUpDown(Mode);
            return GetGammaSet(numericUpDown_Set_Mode);
        }

        bool Is_Dec_Value_Equal(decimal A, decimal B)
        {
            if (A == B)
                return true;
            else
                return false;

        }

        bool Is_any_OCmode_having_a_Same_GammaSet()
        {
            decimal a = numericUpDown_Set_Mode_1.Value;
            decimal b = numericUpDown_Set_Mode_2.Value;
            decimal c = numericUpDown_Set_Mode_3.Value;
            decimal d = numericUpDown_Set_Mode_4.Value;
            decimal e = numericUpDown_Set_Mode_5.Value;
            decimal f = numericUpDown_Set_Mode_6.Value;

            if (a == b) return true;
            else if (a == c) return true;
            else if (a == d) return true;
            else if (a == e) return true;
            else if (a == f) return true;

            else if (b == c) return true;
            else if (b == d) return true;
            else if (b == e) return true;
            else if (b == f) return true;

            else if (c == d) return true;
            else if (c == e) return true;
            else if (c == f) return true;

            else if (d == e) return true;
            else if (d == f) return true;

            else if (e == f) return true;

            else return false;
        }

        public TargetSetting Get_Mode2_Target_Setting()
        {
            double Applied_Lv = Convert.ToDouble(textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Applied_Max_LV.Text);
            double Offset_X = Convert.ToDouble(textBox_Copy_Mode1_M_to_Mode2_Target_Offset_X.Text);
            double Offset_Y = Convert.ToDouble(textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Y.Text);

            return new TargetSetting(Applied_Lv, Offset_X, Offset_Y);
        }

        public TargetSetting Get_Mode3_Target_Setting()
        {
            double Applied_Lv = Convert.ToDouble(textBox_Mode3_Target_Offset_Applied_Max_LV.Text);
            double Offset_X = Convert.ToDouble(textBox_Mode3_Target_Offset_X.Text);
            double Offset_Y = Convert.ToDouble(textBox_Mode3_Target_Offset_Y.Text);

            return new TargetSetting(Applied_Lv, Offset_X, Offset_Y);
        }

        private void Optic_compensation_Start_Click(object sender, EventArgs e)
        {
            if (Is_any_OCmode_having_a_Same_GammaSet())
            {
                System.Windows.Forms.MessageBox.Show("every OCmode(1~6) should have a different Gamma_Set !");
            }
            else
            {
                DP213_Main_OC_Flow Main_OC = new DP213_Main_OC_Flow();
                Main_OC.Start_OC();   
            }
        }
        
        private void update_setting()
        {
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_1);
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_2);
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_3);
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_4);
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_5);
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_6);

            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_2_Skip, numericUpDown_Set_Mode_2);
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_3_Skip, numericUpDown_Set_Mode_3);
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_4_Skip, numericUpDown_Set_Mode_4);
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_5_Skip, numericUpDown_Set_Mode_5);
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_6_Skip, numericUpDown_Set_Mode_6);
        }

        private void checkBox_Mode_Skip_CheckedChanged(CheckBox Mode_Skip, NumericUpDown numericUpDown_Set_Mode)
        {
            if (Mode_Skip.Checked)
                numericUpDown_Set_Mode.Enabled = false;
            else
                numericUpDown_Set_Mode.Enabled = true;
        }

        private void checkBox_Mode_2_Skip_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_2_Skip, numericUpDown_Set_Mode_2);
        }

        private void checkBox_Mode_3_Skip_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_3_Skip, numericUpDown_Set_Mode_3);
        }

        private void checkBox_Mode_4_Skip_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_4_Skip, numericUpDown_Set_Mode_4);
        }

        private void checkBox_Mode_5_Skip_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_5_Skip, numericUpDown_Set_Mode_5);
        }

        private void checkBox_Mode_6_Skip_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_Mode_Skip_CheckedChanged(checkBox_Mode_6_Skip, numericUpDown_Set_Mode_6);
        }

        private void numericUpDown_Set_Mode_Value_Change(NumericUpDown numericUpDown_Set_Mode)
        {
            switch (Convert.ToInt16(numericUpDown_Set_Mode.Value))
            {
                case 1:
                    numericUpDown_Set_Mode.BackColor = DP213_Color_Static.Color_Set1;
                    break;
                case 2:
                    numericUpDown_Set_Mode.BackColor = DP213_Color_Static.Color_Set2;
                    break;
                case 3:
                    numericUpDown_Set_Mode.BackColor = DP213_Color_Static.Color_Set3;
                    break;
                case 4:
                    numericUpDown_Set_Mode.BackColor = DP213_Color_Static.Color_Set4;
                    break;
                case 5:
                    numericUpDown_Set_Mode.BackColor = DP213_Color_Static.Color_Set5;
                    break;
                case 6:
                    numericUpDown_Set_Mode.BackColor = DP213_Color_Static.Color_Set6;
                    break;
            }
        }

        private void numericUpDown_Set_Mode_1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_1);
        }

        private void numericUpDown_Set_Mode_2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_2);
        }

        private void numericUpDown_Set_Mode_3_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_3);
        }

        private void numericUpDown_Set_Mode_4_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_4);
        }

        private void numericUpDown_Set_Mode_5_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_5);
        }

        private void numericUpDown_Set_Mode_6_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_Set_Mode_Value_Change(numericUpDown_Set_Mode_6);
        }


         
        private Gamma_Set get_selected_Gamma_Set()
        {
            if (radioButton_Set1.Checked) return Gamma_Set.Set1;
            else if (radioButton_Set2.Checked) return Gamma_Set.Set2;
            else if (radioButton_Set3.Checked) return Gamma_Set.Set3;
            else if (radioButton_Set4.Checked) return Gamma_Set.Set4;
            else if (radioButton_Set5.Checked) return Gamma_Set.Set5;
            else if (radioButton_Set6.Checked) return Gamma_Set.Set6;
            else return Gamma_Set.SetNull;
        }

        private int get_selected_band()
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
            else if (radioButton_band11.Checked) return 11;
            else if (radioButton_band12.Checked) return 12;
            else if (radioButton_band13.Checked) return 13;
            else if (radioButton_band14.Checked) return 14;
            else return 9999;
        }

        private void Clear_Read_RGB_Vdata_Gridview()
        {
            for (int i = 1; i < dataGridView_RGB_Vdata_Read.Rows.Count; i++)
            {
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[1].Value = ""; //R
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[2].Value = ""; //G
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[3].Value = ""; //B
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[4].Value = ""; //R_Vdata
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[5].Value = ""; //G_Vdata
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[6].Value = ""; //B_Vdata
            }
            Application.DoEvents();
        }

        private void Set_RGB_Vdata_Gridview(Gamma_Set Set, int band)
        {
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                Read_HBM_Normal_Data(Set,band);
            }
            else
            {
                Read_AOD_Data(band);
            }
        }


        private void Read_AOD_Data(int band)
        {
            //Gamma
            for (int i = 1; i <= 11; i++)
            {
                int gray = (i - 1);
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[1].Value = storage.Get_All_band_gray_Gamma(Gamma_Set.Set1, band, gray).int_R;//AOD From Gamma_Set1
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[2].Value = storage.Get_All_band_gray_Gamma(Gamma_Set.Set1, band, gray).int_G;//AOD From Gamma_Set1
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[3].Value = storage.Get_All_band_gray_Gamma(Gamma_Set.Set1, band, gray).int_B;//AOD From Gamma_Set1
            }

            //AM1
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[1].Value = storage.Get_Band_Set_AM1(Gamma_Set.Set1, band).int_R;
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[2].Value = storage.Get_Band_Set_AM1(Gamma_Set.Set1, band).int_G;
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[3].Value = storage.Get_Band_Set_AM1(Gamma_Set.Set1, band).int_B;

            //AM0
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[1].Value = storage.Get_Band_Set_AM0(Gamma_Set.Set1, band).int_R;
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[2].Value = storage.Get_Band_Set_AM0(Gamma_Set.Set1, band).int_G;
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[3].Value = storage.Get_Band_Set_AM0(Gamma_Set.Set1, band).int_B;

            //Vreg1
            dataGridView_RGB_Vdata_Read.Rows[14].Cells[1].Value = storage.Get_AOD_Dec_Vreg1(band - 12);
            
            //VREF4095
            dataGridView_RGB_Vdata_Read.Rows[15].Cells[1].Value = storage.Get_AOD_Dec_VREF4095();
            
            //VREF0
            dataGridView_RGB_Vdata_Read.Rows[16].Cells[1].Value = storage.Get_AOD_Dec_VREF0();
        }

        private void Read_HBM_Normal_Data(Gamma_Set Set, int band)
        {
            //Gamma
            for (int i = 1; i <= 11; i++)
            {
                int gray = (i - 1);
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[1].Value = storage.Get_All_band_gray_Gamma(Set, band, gray).int_R;
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[2].Value = storage.Get_All_band_gray_Gamma(Set, band, gray).int_G;
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[3].Value = storage.Get_All_band_gray_Gamma(Set, band, gray).int_B;
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[4].Value = storage.Get_Voltage_All_band_gray_Gamma(Set, band, gray).double_R;
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[5].Value = storage.Get_Voltage_All_band_gray_Gamma(Set, band, gray).double_G;
                dataGridView_RGB_Vdata_Read.Rows[i].Cells[6].Value = storage.Get_Voltage_All_band_gray_Gamma(Set, band, gray).double_B;

            }

            //AM1
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[1].Value = storage.Get_Band_Set_AM1(Set, band).int_R;
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[2].Value = storage.Get_Band_Set_AM1(Set, band).int_G;
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[3].Value = storage.Get_Band_Set_AM1(Set, band).int_B;
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[4].Value = storage.Get_Band_Set_Voltage_AM1(Set, band).double_R;
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[5].Value = storage.Get_Band_Set_Voltage_AM1(Set, band).double_G;
            dataGridView_RGB_Vdata_Read.Rows[12].Cells[6].Value = storage.Get_Band_Set_Voltage_AM1(Set, band).double_B;

            //AM0
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[1].Value = storage.Get_Band_Set_AM0(Set, band).int_R;
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[2].Value = storage.Get_Band_Set_AM0(Set, band).int_G;
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[3].Value = storage.Get_Band_Set_AM0(Set, band).int_B;
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[4].Value = storage.Get_Band_Set_Voltage_AM0(Set, band).double_R;
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[5].Value = storage.Get_Band_Set_Voltage_AM0(Set, band).double_G;
            dataGridView_RGB_Vdata_Read.Rows[13].Cells[6].Value = storage.Get_Band_Set_Voltage_AM0(Set, band).double_B;

            //Vreg1
            dataGridView_RGB_Vdata_Read.Rows[14].Cells[1].Value = storage.Get_Normal_Dec_Vreg1(Set, band);
            dataGridView_RGB_Vdata_Read.Rows[14].Cells[4].Value = storage.Get_Normal_Voltage_Vreg1(Set, band);

            //VREF4095
            dataGridView_RGB_Vdata_Read.Rows[15].Cells[1].Value = storage.Get_Dec_VREF4095();
            dataGridView_RGB_Vdata_Read.Rows[15].Cells[4].Value = storage.Get_Voltage_VREF4095();

            //VREF0
            dataGridView_RGB_Vdata_Read.Rows[16].Cells[1].Value = storage.Get_Dec_VREF0();
            dataGridView_RGB_Vdata_Read.Rows[16].Cells[4].Value = storage.Get_Voltage_VREF0();
        }

        


        private void Read_and_Update_REF0_REF4095_Vreg1_AM2_GR_AM1_AM0_Data_and_Voltages(Gamma_Set selected_Set,int selected_band)
        {
            //HBM Normal
            if (selected_band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                cmds.Read_and_Update_REF0_REF4095_and_Textboxes(); //(1) Read REF0/REF4095 For Vreg1
                cmds.Read_and_Update_Set_Vreg1_and_Textboxes(selected_Set);//(2) Read Vreg1 For AM2
                cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(selected_Set, selected_band);//(3) Read AM2/GR/AM1/AM0
            }

            //AOD
            else
            {
                cmds.Read_and_Update_AOD_REF0_REF4095();
                cmds.Read_and_Update_AOD_Vreg1_and_Textboxes();
                cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(Gamma_Set.SetNull, selected_band);//AOD Read From Set1
            }
        }

        private void Update_Read_Vdata_Gridview()
        {
            Gamma_Set selected_Set = get_selected_Gamma_Set();
            int selected_band = get_selected_band();
            Clear_Read_RGB_Vdata_Gridview();
            Set_RGB_Vdata_Gridview(selected_Set, selected_band);
        }

         private void button_RGB_Vdata_Read_Click(object sender, EventArgs e)
         {
            Gamma_Set selected_Set = get_selected_Gamma_Set();
            int selected_band = get_selected_band();
            Read_and_Update_REF0_REF4095_Vreg1_AM2_GR_AM1_AM0_Data_and_Voltages(selected_Set,selected_band);
            Update_Read_Vdata_Gridview();
         }

         private void Set_and_Send_RGB_Vdata()
         {
             try
             {
                 Gamma_Set selected_Set = get_selected_Gamma_Set();
                 int selected_band = get_selected_band();

                 //AM1
                 RGB New_AM1 = new RGB();
                 New_AM1.int_R = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[12].Cells[1].Value);
                 New_AM1.int_G = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[12].Cells[2].Value);
                 New_AM1.int_B = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[12].Cells[3].Value);

                  //AM0
                 RGB New_AM0 = new RGB();
                 New_AM0.int_R = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[13].Cells[1].Value);
                 New_AM0.int_G = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[13].Cells[2].Value);
                 New_AM0.int_B = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[13].Cells[3].Value);
                 
                 //Gamma
                 for (int i = 1; i <= 11; i++)
                 {
                     int gray = (i - 1);
                     RGB New_Gamma = new RGB();
                     New_Gamma.int_R = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[i].Cells[1].Value);
                     New_Gamma.int_G = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[i].Cells[2].Value);
                     New_Gamma.int_B = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[i].Cells[3].Value);
                     cmds.Set_and_Send_RGB_CMD(selected_Set, selected_band, gray, New_Gamma, New_AM0, New_AM1);
                 }

                 
                 int Vreg1 = Convert.ToInt32(dataGridView_RGB_Vdata_Write.Rows[14].Cells[1].Value);
                 byte VREF4095 = Convert.ToByte(dataGridView_RGB_Vdata_Write.Rows[15].Cells[1].Value);
                 byte VREF0 = Convert.ToByte(dataGridView_RGB_Vdata_Write.Rows[16].Cells[1].Value);

                 //HBM+Normal
                 if (selected_band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
                 {
                     //Vreg1
                     cmds.Set_and_Send_Vreg1_and_update_Textbox(selected_Set, selected_band, Vreg1);

                     //VREF4095
                     cmds.Set_and_Send_VREF4095_and_update_Textbox(VREF4095);

                     //VREF0
                     cmds.Set_and_Send_VREF0_and_update_Textbox(VREF0);
                 }
                 //AOD
                 else
                 {
                     //Vreg1
                     cmds.Set_and_Send_AOD_Dec_Vreg1_and_update_textboxes(selected_band, Vreg1);

                     //Only Send VREF4095 & REF0
                     cmds.Set_and_Send_AOD_REF4095_REF0(VREF4095, VREF0);
                 }

                 
             }
             catch(Exception e)
             {
                 System.Windows.Forms.MessageBox.Show(e.ToString());
             }
      
         }

         private void update_variables_from_read_data()
         {
             Gamma_Set selected_Set = get_selected_Gamma_Set();
             cmds.Read_and_Update_Set_Vreg1_and_Textboxes(selected_Set);
             cmds.Read_and_Update_AOD_Vreg1_and_Textboxes();
         }

         private void button_RGB_Vdata_Write_Click(object sender, EventArgs e)
         {
             update_variables_from_read_data();
             Set_and_Send_RGB_Vdata();
         }

         private void dataGridView_RGB_Vdata_Write_KeyDown(object sender, KeyEventArgs e)
         {
             if (e.KeyData == (Keys.Control | Keys.V)) f1().PasteInData(ref this.dataGridView_RGB_Vdata_Write);
         }

         public void Mipi_Script_Send(TextBox textBox_Show_Compared_Mipi_Data)
         {
             //Send "mipi.write" of "delay" command
             for (int i = 0; i < textBox_Show_Compared_Mipi_Data.Lines.Length - 1; i++)
             {
                 System.Windows.Forms.Application.DoEvents();

                 if (textBox_Show_Compared_Mipi_Data.Lines[i].Length >= 10
                     && textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 10) == "mipi.write")
                 {
                     f1().IPC_Quick_Send(textBox_Show_Compared_Mipi_Data.Lines[i]);
                 }
                 else if (textBox_Show_Compared_Mipi_Data.Lines[i].Length >= 5 && (
                     textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5) == "delay"
                     || textBox_Show_Compared_Mipi_Data.Lines[i].Substring(0, 5) == "image"))
                 {
                     f1().IPC_Quick_Send(textBox_Show_Compared_Mipi_Data.Lines[i]);
                 }
                 else
                 {
                     // It's not a "mipi.write" command , do nothing 
                 }
             }
         }

         public void Set_Condition_Mipi_Script_Send(Gamma_Set Set)
         {
             Form1 f1 = (Form1)Application.OpenForms["Form1"];

             TextBox textBox_Show_Compared_Mipi_Data = new TextBox();
             if (Set == Gamma_Set.Set1)
             {
                 textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set1;
                 f1.GB_Status_AppendText_Nextline("Set1 Applied", DP213_Color_Static.Color_Set1);
             }
             else if (Set == Gamma_Set.Set2)
             {
                 textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set2;
                 f1.GB_Status_AppendText_Nextline("Set2 Applied", DP213_Color_Static.Color_Set2);
             }
             else if (Set == Gamma_Set.Set3)
             {
                 textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set3;
                 f1.GB_Status_AppendText_Nextline("Set3 Applied", DP213_Color_Static.Color_Set3);
             }
             else if (Set == Gamma_Set.Set4)
             {
                 textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set4;
                 f1.GB_Status_AppendText_Nextline("Set4 Applied", DP213_Color_Static.Color_Set4);
             }
             else if (Set == Gamma_Set.Set5)
             {
                 textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set5;
                 f1.GB_Status_AppendText_Nextline("Set5 Applied", DP213_Color_Static.Color_Set5);
             }
             else if (Set == Gamma_Set.Set6)
             {
                 textBox_Show_Compared_Mipi_Data = textBox_Show_Compared_Mipi_Data_Set6;
                 f1.GB_Status_AppendText_Nextline("Set6 Applied", DP213_Color_Static.Color_Set6);
             }

             //Send "mipi.write" of "delay" command
             Mipi_Script_Send(textBox_Show_Compared_Mipi_Data);
         }

         private void button_Gamma_Set1_Apply_Click(object sender, EventArgs e)
         {
             Set_Condition_Mipi_Script_Send(Gamma_Set.Set1);
         }

         private void button_Gamma_Set2_Apply_Click(object sender, EventArgs e)
         {
             Set_Condition_Mipi_Script_Send(Gamma_Set.Set2);
         }

         private void button_Gamma_Set3_Apply_Click(object sender, EventArgs e)
         {
             Set_Condition_Mipi_Script_Send(Gamma_Set.Set3);
         }

         private void button_Gamma_Set4_Apply_Click(object sender, EventArgs e)
         {
             Set_Condition_Mipi_Script_Send(Gamma_Set.Set4);
         }

         private void button_Gamma_Set5_Apply_Click(object sender, EventArgs e)
         {
             Set_Condition_Mipi_Script_Send(Gamma_Set.Set5);
         }

         private void button_Gamma_Set6_Apply_Click(object sender, EventArgs e)
         {
             Set_Condition_Mipi_Script_Send(Gamma_Set.Set6);
         }

         private void button_Flash_Write_Click(object sender, EventArgs e)
         {
             flashmemory.Send_EA9155_Info_and_Flash_Erase_And_Write();
         }

         private void Read_E3_P225_P226_Click(object sender, EventArgs e)
         {

         }

         private void button_BSQH_Stop_Click_1(object sender, EventArgs e)
         {
             vars.Optic_Compensation_Stop = true;
         }

         private void radioButton_band12_CheckedChanged(object sender, EventArgs e)
         {
             if (radioButton_band12.Checked)
                 groupBox_Vdata_Read_Write_Set_Selection.Visible = false;
             else
                 groupBox_Vdata_Read_Write_Set_Selection.Visible = true;
         }

         private void radioButton_band13_CheckedChanged(object sender, EventArgs e)
         {
             if (radioButton_band13.Checked)
                 groupBox_Vdata_Read_Write_Set_Selection.Visible = false;
             else
                 groupBox_Vdata_Read_Write_Set_Selection.Visible = true;
         }

         private void radioButton_band14_CheckedChanged(object sender, EventArgs e)
         {
             if (radioButton_band14.Checked)
                 groupBox_Vdata_Read_Write_Set_Selection.Visible = false;
             else
                 groupBox_Vdata_Read_Write_Set_Selection.Visible = true;
         }


         private void button_OD_Flash_Erase_Write_and_CRC_Check_Click(object sender, EventArgs e)
         {
             flashmemory.OD_Flash_Erase_Write_and_CRC_Check();
         }


        private void button_Frame_CRC_Check_Click(object sender, EventArgs e)
        {
            f1().GB_Status_AppendText_Nextline("---Loading From Flash Start---", Color.Blue);
            flashmemory.Read_From_Frame_and_Show();
            f1().GB_Status_AppendText_Nextline("---Loading From Flash Finished---", Color.Blue);
        }

        private void button_Flash_CRC_Check_Click(object sender, EventArgs e)
        {
            f1().GB_Status_AppendText_Nextline("---Loading From Flash Start---", Color.Red);
            flashmemory.Read_From_Flash_and_Show();
            f1().GB_Status_AppendText_Nextline("---Loading From Flash Finished---", Color.Red);
        }


        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void button_Save_Setting_Click(object sender, EventArgs e)
        {
            EA9155_Preferences To_Be_Serialized_Data_Structure = Get_Updated_EA9155_Preferences();
            Save_EA9155_Preferences(To_Be_Serialized_Data_Structure);
        }

        private void Save_EA9155_Preferences(EA9155_Preferences To_Be_Serialized_Data_Structure)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(EA9155_Preferences));
            SaveFileDialog saveFileDialog1 = Get_XML_Filter_saveFileDialog1();

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Serialize_EA9155_Preferences_Object(saveFileDialog1.FileName, mySerializer, To_Be_Serialized_Data_Structure);
                System.Windows.Forms.MessageBox.Show("Setting has been saved");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing has benn Saved");
            }
        }

        private SaveFileDialog Get_XML_Filter_saveFileDialog1()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\DP213";
            saveFileDialog1.Filter = "Default Extension (*.xml)|*.xml";
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.AddExtension = true;
            return saveFileDialog1;
        }

        private void Serialize_EA9155_Preferences_Object(string FileName,XmlSerializer mySerializer, EA9155_Preferences To_Be_Serialized_Data_Structure)
        {
            StreamWriter myWriter = new StreamWriter(FileName);
            mySerializer.Serialize(myWriter, To_Be_Serialized_Data_Structure);
            myWriter.Close();
        }

        private EA9155_Preferences Get_Updated_EA9155_Preferences()
        {
            EA9155_Preferences up = new EA9155_Preferences();

            //GB_Ctrl
            up.checkBox_Band[0] = checkBox_Band0.Checked;
            up.checkBox_Band[1] = checkBox_Band1.Checked;
            up.checkBox_Band[2] = checkBox_Band2.Checked;
            up.checkBox_Band[3] = checkBox_Band3.Checked;
            up.checkBox_Band[4] = checkBox_Band4.Checked;
            up.checkBox_Band[5] = checkBox_Band5.Checked;
            up.checkBox_Band[6] = checkBox_Band6.Checked;
            up.checkBox_Band[7] = checkBox_Band7.Checked;
            up.checkBox_Band[8] = checkBox_Band8.Checked;
            up.checkBox_Band[9] = checkBox_Band9.Checked;
            up.checkBox_Band[10] = checkBox_Band10.Checked;
            up.checkBox_Band[11] = checkBox_Band11.Checked;

            up.checkBox_AOD[0] = checkBox_AOD0.Checked;
            up.checkBox_AOD[1] = checkBox_AOD1.Checked;
            up.checkBox_AOD[2] = checkBox_AOD2.Checked;

            up.checkBox_Read_DBV_Values = checkBox_Read_DBV_Values.Checked;
            up.checkBox_Special_Gray_Compensation = checkBox_Special_Gray_Compensation.Checked;
            up.checkBox_Only_255G = checkBox_Only_255G.Checked;
            up.radioButton_G2G_On = radioButton_G2G_On.Checked;
            up.radioButton_G2G_Off = radioButton_G2G_Off.Checked;
            up.textBox_Max_Loop = textBox_Max_Loop.Text;
            up.textBox_Subcompensation_GB_skip_Lv = textBox_Subcompensation_GB_skip_Lv.Text;

            up.radioButton_Mode23456_Gray255_RGB_OC = radioButton_Mode23456_Gray255_RGB_OC.Checked;
            up.radioButton_Mode23456_Gray255_RVreg1B_OC = radioButton_Mode23456_Gray255_RVreg1B_OC.Checked;

            //Limit Ratio Selection
            up.radioButton_Limit_Apply_Ratio1 = radioButton_Limit_Apply_Ratio1.Checked;
            up.radioButton_Limit_Apply_Ratio2 = radioButton_Limit_Apply_Ratio2.Checked;
            up.radioButton_Limit_Apply_Ratio3 = radioButton_Limit_Apply_Ratio3.Checked;

            //Set_Mode_Selection
            up.numericUpDown_Set_Mode_1 = numericUpDown_Set_Mode_1.Value;
            up.numericUpDown_Set_Mode_2 = numericUpDown_Set_Mode_2.Value;
            up.numericUpDown_Set_Mode_3 = numericUpDown_Set_Mode_3.Value;
            up.numericUpDown_Set_Mode_4 = numericUpDown_Set_Mode_4.Value;
            up.numericUpDown_Set_Mode_5 = numericUpDown_Set_Mode_5.Value;
            up.numericUpDown_Set_Mode_6 = numericUpDown_Set_Mode_6.Value;
            up.checkBox_Mode_2_Skip = checkBox_Mode_2_Skip.Checked;
            up.checkBox_Mode_3_Skip = checkBox_Mode_3_Skip.Checked;
            up.checkBox_Mode_4_Skip = checkBox_Mode_4_Skip.Checked;
            up.checkBox_Mode_5_Skip = checkBox_Mode_5_Skip.Checked;
            up.checkBox_Mode_6_Skip = checkBox_Mode_6_Skip.Checked;


            //ELVSS_Offset
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = ELVSS_B0_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = ELVSS_B1_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = ELVSS_B2_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = ELVSS_B3_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = ELVSS_B4_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = ELVSS_B5_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = ELVSS_B6_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = ELVSS_B7_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = ELVSS_B8_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = ELVSS_B9_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = ELVSS_B10_Offset_Set1.Text;
            up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = ELVSS_B11_Offset_Set1.Text;

            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = ELVSS_B0_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = ELVSS_B1_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = ELVSS_B2_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = ELVSS_B3_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = ELVSS_B4_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = ELVSS_B5_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = ELVSS_B6_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = ELVSS_B7_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = ELVSS_B8_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = ELVSS_B9_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = ELVSS_B10_Offset_Set2.Text;
            up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = ELVSS_B11_Offset_Set2.Text;

            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = ELVSS_B0_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = ELVSS_B1_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = ELVSS_B2_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = ELVSS_B3_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = ELVSS_B4_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = ELVSS_B5_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = ELVSS_B6_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = ELVSS_B7_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = ELVSS_B8_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = ELVSS_B9_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = ELVSS_B10_Offset_Set3.Text;
            up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = ELVSS_B11_Offset_Set3.Text;

            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = ELVSS_B0_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = ELVSS_B1_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = ELVSS_B2_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = ELVSS_B3_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = ELVSS_B4_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = ELVSS_B5_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = ELVSS_B6_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = ELVSS_B7_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = ELVSS_B8_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = ELVSS_B9_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = ELVSS_B10_Offset_Set4.Text;
            up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = ELVSS_B11_Offset_Set4.Text;

            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = ELVSS_B0_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = ELVSS_B1_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = ELVSS_B2_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = ELVSS_B3_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = ELVSS_B4_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = ELVSS_B5_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = ELVSS_B6_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = ELVSS_B7_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = ELVSS_B8_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = ELVSS_B9_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = ELVSS_B10_Offset_Set5.Text;
            up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = ELVSS_B11_Offset_Set5.Text;

            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = ELVSS_B0_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = ELVSS_B1_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = ELVSS_B2_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = ELVSS_B3_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = ELVSS_B4_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = ELVSS_B5_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = ELVSS_B6_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = ELVSS_B7_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = ELVSS_B8_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = ELVSS_B9_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = ELVSS_B10_Offset_Set6.Text;
            up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = ELVSS_B11_Offset_Set6.Text;

            //Vinit2_Offset
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = Vinit_Offset_B0_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = Vinit_Offset_B1_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = Vinit_Offset_B2_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = Vinit_Offset_B3_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = Vinit_Offset_B4_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = Vinit_Offset_B5_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = Vinit_Offset_B6_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = Vinit_Offset_B7_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = Vinit_Offset_B8_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = Vinit_Offset_B9_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = Vinit_Offset_B10_Set1.Text;
            up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = Vinit_Offset_B11_Set1.Text;

            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = Vinit_Offset_B0_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = Vinit_Offset_B1_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = Vinit_Offset_B2_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = Vinit_Offset_B3_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = Vinit_Offset_B4_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = Vinit_Offset_B5_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = Vinit_Offset_B6_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = Vinit_Offset_B7_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = Vinit_Offset_B8_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = Vinit_Offset_B9_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = Vinit_Offset_B10_Set2.Text;
            up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = Vinit_Offset_B11_Set2.Text;

            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = Vinit_Offset_B0_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = Vinit_Offset_B1_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = Vinit_Offset_B2_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = Vinit_Offset_B3_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = Vinit_Offset_B4_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = Vinit_Offset_B5_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = Vinit_Offset_B6_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = Vinit_Offset_B7_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = Vinit_Offset_B8_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = Vinit_Offset_B9_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = Vinit_Offset_B10_Set3.Text;
            up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = Vinit_Offset_B11_Set3.Text;

            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = Vinit_Offset_B0_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = Vinit_Offset_B1_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = Vinit_Offset_B2_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = Vinit_Offset_B3_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = Vinit_Offset_B4_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = Vinit_Offset_B5_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = Vinit_Offset_B6_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = Vinit_Offset_B7_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = Vinit_Offset_B8_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = Vinit_Offset_B9_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = Vinit_Offset_B10_Set4.Text;
            up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = Vinit_Offset_B11_Set4.Text;

            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = Vinit_Offset_B0_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = Vinit_Offset_B1_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = Vinit_Offset_B2_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = Vinit_Offset_B3_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = Vinit_Offset_B4_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = Vinit_Offset_B5_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = Vinit_Offset_B6_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = Vinit_Offset_B7_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = Vinit_Offset_B8_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = Vinit_Offset_B9_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = Vinit_Offset_B10_Set5.Text;
            up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = Vinit_Offset_B11_Set5.Text;

            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0] = Vinit_Offset_B0_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1] = Vinit_Offset_B1_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2] = Vinit_Offset_B2_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3] = Vinit_Offset_B3_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4] = Vinit_Offset_B4_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5] = Vinit_Offset_B5_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6] = Vinit_Offset_B6_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7] = Vinit_Offset_B7_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8] = Vinit_Offset_B8_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9] = Vinit_Offset_B9_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10] = Vinit_Offset_B10_Set6.Text;
            up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11] = Vinit_Offset_B11_Set6.Text;

            //ELVSS OC
            up.checkBox_ELVSS_and_Vinit2_Comp = checkBox_ELVSS_and_Vinit2_Comp.Checked;
            up.textBox_ELVSS_CMD_Delay = textBox_ELVSS_CMD_Delay.Text;
            up.textBox_ELVSS_Min_Before_Add_Offset = textBox_ELVSS_Min_Before_Add_Offset.Text;
            up.textBox_ELVSS_Max_Before_Add_Offset = textBox_ELVSS_Max_Before_Add_Offset.Text;

            //Vreg1 OC
            up.checkBox_Mode1_Vreg1_Compensation = checkBox_Mode1_Vreg1_Compensation.Checked;

            //AM1 OC
            up.radioButton_AM1_0x00 = radioButton_AM1_0x00.Checked;
            up.radioButton_AM1_Original_Value = radioButton_AM1_Original_Value.Checked;
            up.radioButton_AM1_Comp = radioButton_AM1_Comp.Checked;
            up.textBox_AM1_Margin_R = textBox_AM1_Margin_R.Text;
            up.textBox_AM1_Margin_G = textBox_AM1_Margin_G.Text;
            up.textBox_AM1_Margin_B = textBox_AM1_Margin_B.Text;

            //Target Setting
            up.Checkbox_Copy_Mode1_Measure_to_Mode2_Target = Checkbox_Copy_Mode1_Measure_to_Mode2_Target.Checked;
            up.textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Applied_Max_LV = textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Applied_Max_LV.Text;
            up.textBox_Copy_Mode1_M_to_Mode2_Target_Offset_X = textBox_Copy_Mode1_M_to_Mode2_Target_Offset_X.Text;
            up.textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Y = textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Y.Text; 

            up.checkBox_Copy_Mode12_Ave_Measure_to_Mode3_Target = checkBox_Copy_Mode12_Ave_Measure_to_Mode3_Target.Checked;
            up.checkBox_Copy_Mode2_Measure_to_Mode3_Target = checkBox_Copy_Mode2_Measure_to_Mode3_Target.Checked;
            up.textBox_Mode3_Target_Offset_Applied_Max_LV = textBox_Mode3_Target_Offset_Applied_Max_LV.Text;
            up.textBox_Mode3_Target_Offset_X = textBox_Mode3_Target_Offset_X.Text;
            up.textBox_Mode3_Target_Offset_Y = textBox_Mode3_Target_Offset_Y.Text;


            //Triple Mode Related
            up.checkBox_Set23_OC_Skip_If_UV_and_deltaL_Are_within_Specs = checkBox_Set23_OC_Skip_If_UV_and_deltaL_Are_within_Specs.Checked;
            up.checkBox_Copy_Mode1_Vreg1_to_Mode23456 = checkBox_Copy_Mode1_Vreg1_to_Mode23456.Checked;
            up.checkBox_Copy_Mode1_Gamma_to_Mode23 = checkBox_Copy_Mode1_Gamma_to_Mode23.Checked;
            up.checkBox_Copy_Mode1_Gamma_to_Mode4 = checkBox_Copy_Mode1_Gamma_to_Mode4.Checked;
            up.checkBox_Copy_Mode2_Gamma_to_Mode5 = checkBox_Copy_Mode2_Gamma_to_Mode5.Checked;
            up.checkBox_Copy_Mode3_Gamma_to_Mode6 = checkBox_Copy_Mode3_Gamma_to_Mode6.Checked;
            

            //OD ERA DGGM CRC 
            up.textBox_OD_CRC_Hex_1 = textBox_OD_CRC_Hex_1.Text;
            up.textBox_OD_CRC_Hex_2 = textBox_OD_CRC_Hex_2.Text;

            //Black OC
            up.radioButton_AM0_0x00 = radioButton_AM0_0x00.Checked;
            up.radioButton_AM0_Original_Value = radioButton_AM0_Original_Value.Checked;
            up.radioButton_Black_Compensation = radioButton_Black_Compensation.Checked;
            up.textBox_AM0_R_Margin = textBox_AM0_R_Margin.Text;
            up.textBox_AM0_G_Margin = textBox_AM0_G_Margin.Text;
            up.textBox_AM0_B_Margin = textBox_AM0_B_Margin.Text;
            up.textBox_REF4095_Margin = textBox_REF4095_Margin.Text;
            up.textBox_Black_Limit_Lv = textBox_Black_Limit_Lv.Text;
            
            //REF0
            up.checkBox_VREF0_Comp = checkBox_VREF0_Comp.Checked;
            up.textBox_REF0_Margin = textBox_REF0_Margin.Text;

            up.numericUpDown_Set456_Skip_Max_Band = numericUpDown_Set456_Skip_Max_Band.Value;

            //Initial RGBVreg1 Finding Algorithm
            up.checkBox_Initial_RVreg1B_or_RGB_Algorithm_Apply = checkBox_Initial_RVreg1B_or_RGB_Algorithm_Apply.Checked;
            up.textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Upper_Limit = textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Upper_Limit.Text;
            up.textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Lower_Limit = textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Lower_Limit.Text;
            up.textBox_Initial_RGB_Algorithm_Lv_Combine_Ratio = textBox_Initial_RGB_Algorithm_Lv_Combine_Ratio.Text;
            up.radioButton_MCI_and_3Points = radioButton_MCI_and_3Points.Checked;
            up.radioButton_LUT_MCI = radioButton_LUT_MCI.Checked;

            //UVL Check
            up.checkBox_OC_Mode23_UVL_Check = checkBox_OC_Mode23_UVL_Check.Checked;

            //Min Distance
            up.radioButton_Min_Distance_N_Points_Measurements = radioButton_Min_Distance_N_Points_Measurements.Checked;
            up.textbox_min_distance_n_points = textbox_min_distance_n_points.Text;

            //Differece Offset
            up.radioButton_Difference_Offset = radioButton_Difference_Offset.Checked;
            up.textBox_Delta_L_Limit = textBox_Delta_L_Limit.Text;
            up.radioButton_None = radioButton_None.Checked;

            up.radioButton_AM0_AM1_REF0_ELVSS_OCMode_1 = radioButton_AM0_AM1_REF0_ELVSS_OCMode_1.Checked;
            up.radioButton_AM0_AM1_REF0_ELVSS_OCMode_2 = radioButton_AM0_AM1_REF0_ELVSS_OCMode_2.Checked;
            up.radioButton_AM0_AM1_REF0_ELVSS_OCMode_3 = radioButton_AM0_AM1_REF0_ELVSS_OCMode_3.Checked;

            up.checkBox_Apply_OC_Mode1n3_LV_Lowerthan_OC_Mode2_LV = checkBox_Apply_OC_Mode1n3_LV_Lowerthan_OC_Mode2_LV.Checked;

            up.checkBox_OC_File_Refresh = checkBox_OC_File_Refresh.Checked;

            //Gray95/47/23 OC_Skip (Apply Interpolation)
            up.checkBox_Apply_Interpolation_Gray95 = checkBox_Apply_Interpolation_Gray95.Checked;
            up.checkBox_Apply_Interpolation_Gray47 = checkBox_Apply_Interpolation_Gray47.Checked;
            up.checkBox_Apply_Interpolation_Gray23 = checkBox_Apply_Interpolation_Gray23.Checked;
            up.checkBox_Apply_Interpolation_OC_Mode123 = checkBox_Apply_Interpolation_OC_Mode123.Checked;
            up.checkBox_Apply_Interpolation_OC_Mode456 = checkBox_Apply_Interpolation_OC_Mode456.Checked;
            up.checkBox_Apply_Interpolation_OC_Mode_AOD = checkBox_Apply_Interpolation_OC_Mode_AOD.Checked;
            up.textBox_Interpolation_ToBeApplied_LV_OCMode123 = textBox_Interpolation_ToBeApplied_LV_OCMode123.Text;
            up.textBox_Interpolation_ToBeApplied_LV_OCMode456 = textBox_Interpolation_ToBeApplied_LV_OCMode456.Text;
            up.textBox_Interpolation_ToBeApplied_LV_AOD = textBox_Interpolation_ToBeApplied_LV_AOD.Text;
            up.checkBox_B0G255_Skip_If_Compensated = checkBox_B0G255_Skip_If_Compensated.Checked;


            return up;
        }


  

        private void button_Load_Setting_Click(object sender, EventArgs e)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(EA9155_Preferences));
            OpenFileDialog openFileDialog1 = Get_XML_Filter_openFileDialog1();
           
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                Deserialize_EA9155_Preferences_Object_and_Update_Settings(mySerializer, openFileDialog1.FileName);
            
            else
                System.Windows.Forms.MessageBox.Show("Nothing has been Loaded");
            
        }

        private void Deserialize_EA9155_Preferences_Object_and_Update_Settings(XmlSerializer mySerializer, string Filename)
        {
            FileStream myFileStream = new FileStream(Filename, FileMode.Open);
            EA9155_Preferences up = (EA9155_Preferences)mySerializer.Deserialize(myFileStream);
            Update_Setting(up);
            myFileStream.Close();
            System.Windows.Forms.MessageBox.Show("Settings have been Loaded");
        }


        private OpenFileDialog Get_XML_Filter_openFileDialog1()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\DP213";
            openFileDialog1.Filter = "Default Extension (*.xml)|*.xml";
            openFileDialog1.DefaultExt = "xml";
            openFileDialog1.AddExtension = true;

            return openFileDialog1;
        }

        private void Update_Setting(EA9155_Preferences up)
        {
            checkBox_Band0.Checked = up.checkBox_Band[0];
            checkBox_Band1.Checked = up.checkBox_Band[1];
            checkBox_Band2.Checked = up.checkBox_Band[2];
            checkBox_Band3.Checked = up.checkBox_Band[3];
            checkBox_Band4.Checked = up.checkBox_Band[4];
            checkBox_Band5.Checked = up.checkBox_Band[5];
            checkBox_Band6.Checked = up.checkBox_Band[6];
            checkBox_Band7.Checked = up.checkBox_Band[7];
            checkBox_Band8.Checked = up.checkBox_Band[8];
            checkBox_Band9.Checked = up.checkBox_Band[9];
            checkBox_Band10.Checked = up.checkBox_Band[10];
            checkBox_Band11.Checked = up.checkBox_Band[11];

            checkBox_AOD0.Checked = up.checkBox_AOD[0];
            checkBox_AOD1.Checked = up.checkBox_AOD[1];
            checkBox_AOD2.Checked = up.checkBox_AOD[2];

            checkBox_Read_DBV_Values.Checked = up.checkBox_Read_DBV_Values;
            checkBox_Special_Gray_Compensation.Checked = up.checkBox_Special_Gray_Compensation;
            checkBox_Only_255G.Checked = up.checkBox_Only_255G;
            radioButton_G2G_On.Checked = up.radioButton_G2G_On;
            radioButton_G2G_Off.Checked = up.radioButton_G2G_Off;
            textBox_Max_Loop.Text = up.textBox_Max_Loop;
            textBox_Subcompensation_GB_skip_Lv.Text = up.textBox_Subcompensation_GB_skip_Lv;

            radioButton_Mode23456_Gray255_RGB_OC.Checked = up.radioButton_Mode23456_Gray255_RGB_OC;
            radioButton_Mode23456_Gray255_RVreg1B_OC.Checked = up.radioButton_Mode23456_Gray255_RVreg1B_OC;

            //Limit Ratio Selection
            radioButton_Limit_Apply_Ratio1.Checked = up.radioButton_Limit_Apply_Ratio1;
            radioButton_Limit_Apply_Ratio2.Checked = up.radioButton_Limit_Apply_Ratio2;
            radioButton_Limit_Apply_Ratio3.Checked = up.radioButton_Limit_Apply_Ratio3;

            //Set_Mode_Selection
            numericUpDown_Set_Mode_1.Value = up.numericUpDown_Set_Mode_1;
            numericUpDown_Set_Mode_2.Value = up.numericUpDown_Set_Mode_2;
            numericUpDown_Set_Mode_3.Value = up.numericUpDown_Set_Mode_3;
            numericUpDown_Set_Mode_4.Value = up.numericUpDown_Set_Mode_4;
            numericUpDown_Set_Mode_5.Value = up.numericUpDown_Set_Mode_5;
            numericUpDown_Set_Mode_6.Value = up.numericUpDown_Set_Mode_6;
            checkBox_Mode_2_Skip.Checked = up.checkBox_Mode_2_Skip;
            checkBox_Mode_3_Skip.Checked = up.checkBox_Mode_3_Skip;
            checkBox_Mode_4_Skip.Checked = up.checkBox_Mode_4_Skip;
            checkBox_Mode_5_Skip.Checked = up.checkBox_Mode_5_Skip;
            checkBox_Mode_6_Skip.Checked = up.checkBox_Mode_6_Skip;

            //ELVSS_Offset
            ELVSS_B0_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            ELVSS_B1_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            ELVSS_B2_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            ELVSS_B3_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            ELVSS_B4_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            ELVSS_B5_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            ELVSS_B6_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            ELVSS_B7_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            ELVSS_B8_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            ELVSS_B9_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            ELVSS_B10_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            ELVSS_B11_Offset_Set1.Text = up.ELVSS_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            ELVSS_B0_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            ELVSS_B1_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            ELVSS_B2_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            ELVSS_B3_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            ELVSS_B4_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            ELVSS_B5_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            ELVSS_B6_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            ELVSS_B7_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            ELVSS_B8_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            ELVSS_B9_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            ELVSS_B10_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            ELVSS_B11_Offset_Set2.Text = up.ELVSS_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            ELVSS_B0_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            ELVSS_B1_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            ELVSS_B2_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            ELVSS_B3_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            ELVSS_B4_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            ELVSS_B5_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            ELVSS_B6_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            ELVSS_B7_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            ELVSS_B8_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            ELVSS_B9_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            ELVSS_B10_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            ELVSS_B11_Offset_Set3.Text = up.ELVSS_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            ELVSS_B0_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            ELVSS_B1_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            ELVSS_B2_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            ELVSS_B3_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            ELVSS_B4_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            ELVSS_B5_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            ELVSS_B6_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            ELVSS_B7_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            ELVSS_B8_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            ELVSS_B9_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            ELVSS_B10_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            ELVSS_B11_Offset_Set4.Text = up.ELVSS_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            ELVSS_B0_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            ELVSS_B1_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            ELVSS_B2_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            ELVSS_B3_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            ELVSS_B4_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            ELVSS_B5_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            ELVSS_B6_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            ELVSS_B7_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            ELVSS_B8_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            ELVSS_B9_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            ELVSS_B10_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            ELVSS_B11_Offset_Set5.Text = up.ELVSS_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            ELVSS_B0_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            ELVSS_B1_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            ELVSS_B2_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            ELVSS_B3_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            ELVSS_B4_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            ELVSS_B5_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            ELVSS_B6_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            ELVSS_B7_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            ELVSS_B8_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            ELVSS_B9_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            ELVSS_B10_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            ELVSS_B11_Offset_Set6.Text = up.ELVSS_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            //Vinit2_Offset
            Vinit_Offset_B0_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            Vinit_Offset_B1_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            Vinit_Offset_B2_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            Vinit_Offset_B3_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            Vinit_Offset_B4_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            Vinit_Offset_B5_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            Vinit_Offset_B6_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            Vinit_Offset_B7_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            Vinit_Offset_B8_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            Vinit_Offset_B9_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            Vinit_Offset_B10_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            Vinit_Offset_B11_Set1.Text = up.Vinit2_Offset[0 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            Vinit_Offset_B0_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            Vinit_Offset_B1_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            Vinit_Offset_B2_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            Vinit_Offset_B3_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            Vinit_Offset_B4_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            Vinit_Offset_B5_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            Vinit_Offset_B6_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            Vinit_Offset_B7_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            Vinit_Offset_B8_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            Vinit_Offset_B9_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            Vinit_Offset_B10_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            Vinit_Offset_B11_Set2.Text = up.Vinit2_Offset[1 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            Vinit_Offset_B0_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            Vinit_Offset_B1_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            Vinit_Offset_B2_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            Vinit_Offset_B3_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            Vinit_Offset_B4_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            Vinit_Offset_B5_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            Vinit_Offset_B6_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            Vinit_Offset_B7_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            Vinit_Offset_B8_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            Vinit_Offset_B9_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            Vinit_Offset_B10_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            Vinit_Offset_B11_Set3.Text = up.Vinit2_Offset[2 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            Vinit_Offset_B0_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            Vinit_Offset_B1_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            Vinit_Offset_B2_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            Vinit_Offset_B3_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            Vinit_Offset_B4_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            Vinit_Offset_B5_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            Vinit_Offset_B6_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            Vinit_Offset_B7_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            Vinit_Offset_B8_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            Vinit_Offset_B9_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            Vinit_Offset_B10_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            Vinit_Offset_B11_Set4.Text = up.Vinit2_Offset[3 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            Vinit_Offset_B0_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            Vinit_Offset_B1_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            Vinit_Offset_B2_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            Vinit_Offset_B3_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            Vinit_Offset_B4_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            Vinit_Offset_B5_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            Vinit_Offset_B6_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            Vinit_Offset_B7_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            Vinit_Offset_B8_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            Vinit_Offset_B9_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            Vinit_Offset_B10_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            Vinit_Offset_B11_Set5.Text = up.Vinit2_Offset[4 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            Vinit_Offset_B0_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 0];
            Vinit_Offset_B1_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 1];
            Vinit_Offset_B2_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 2];
            Vinit_Offset_B3_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 3];
            Vinit_Offset_B4_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 4];
            Vinit_Offset_B5_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 5];
            Vinit_Offset_B6_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 6];
            Vinit_Offset_B7_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 7];
            Vinit_Offset_B8_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 8];
            Vinit_Offset_B9_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 9];
            Vinit_Offset_B10_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 10];
            Vinit_Offset_B11_Set6.Text = up.Vinit2_Offset[5 * DP213_Static.Max_HBM_and_Normal_Band_Amount + 11];

            //ELVSS OC
            checkBox_ELVSS_and_Vinit2_Comp.Checked = up.checkBox_ELVSS_and_Vinit2_Comp;
            textBox_ELVSS_CMD_Delay.Text = up.textBox_ELVSS_CMD_Delay;
            textBox_ELVSS_Min_Before_Add_Offset.Text = up.textBox_ELVSS_Min_Before_Add_Offset;
            textBox_ELVSS_Max_Before_Add_Offset.Text = up.textBox_ELVSS_Max_Before_Add_Offset;

            //Vreg1 OC
            checkBox_Mode1_Vreg1_Compensation.Checked = up.checkBox_Mode1_Vreg1_Compensation;

            //AM1 OC
            radioButton_AM1_0x00.Checked = up.radioButton_AM1_0x00;
            radioButton_AM1_Original_Value.Checked = up.radioButton_AM1_Original_Value;
            radioButton_AM1_Comp.Checked = up.radioButton_AM1_Comp;
            textBox_AM1_Margin_R.Text = up.textBox_AM1_Margin_R;
            textBox_AM1_Margin_G.Text = up.textBox_AM1_Margin_G;
            textBox_AM1_Margin_B.Text = up.textBox_AM1_Margin_B;

            //Target Setting
            Checkbox_Copy_Mode1_Measure_to_Mode2_Target.Checked = up.Checkbox_Copy_Mode1_Measure_to_Mode2_Target;
            textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Applied_Max_LV.Text = up.textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Applied_Max_LV;
            textBox_Copy_Mode1_M_to_Mode2_Target_Offset_X.Text = up.textBox_Copy_Mode1_M_to_Mode2_Target_Offset_X;
            textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Y.Text = up.textBox_Copy_Mode1_M_to_Mode2_Target_Offset_Y;

            checkBox_Copy_Mode12_Ave_Measure_to_Mode3_Target.Checked = up.checkBox_Copy_Mode12_Ave_Measure_to_Mode3_Target;
            checkBox_Copy_Mode2_Measure_to_Mode3_Target.Checked = up.checkBox_Copy_Mode2_Measure_to_Mode3_Target;
            textBox_Mode3_Target_Offset_Applied_Max_LV.Text = up.textBox_Mode3_Target_Offset_Applied_Max_LV;
            textBox_Mode3_Target_Offset_X.Text = up.textBox_Mode3_Target_Offset_X;
            textBox_Mode3_Target_Offset_Y.Text = up.textBox_Mode3_Target_Offset_Y;

            //Triple Mode Related
            checkBox_Set23_OC_Skip_If_UV_and_deltaL_Are_within_Specs.Checked = up.checkBox_Set23_OC_Skip_If_UV_and_deltaL_Are_within_Specs;
            checkBox_Copy_Mode1_Vreg1_to_Mode23456.Checked = up.checkBox_Copy_Mode1_Vreg1_to_Mode23456;
            checkBox_Copy_Mode1_Gamma_to_Mode23.Checked = up.checkBox_Copy_Mode1_Gamma_to_Mode23;
            checkBox_Copy_Mode1_Gamma_to_Mode4.Checked = up.checkBox_Copy_Mode1_Gamma_to_Mode4;
            checkBox_Copy_Mode2_Gamma_to_Mode5.Checked = up.checkBox_Copy_Mode2_Gamma_to_Mode5;
            checkBox_Copy_Mode3_Gamma_to_Mode6.Checked = up.checkBox_Copy_Mode3_Gamma_to_Mode6;

            //OD ERA DGGM CRC 
            textBox_OD_CRC_Hex_1.Text = up.textBox_OD_CRC_Hex_1;
            textBox_OD_CRC_Hex_2.Text = up.textBox_OD_CRC_Hex_2;


            //Black OC
            radioButton_AM0_0x00.Checked = up.radioButton_AM0_0x00;
            radioButton_AM0_Original_Value.Checked = up.radioButton_AM0_Original_Value;
            radioButton_Black_Compensation.Checked = up.radioButton_Black_Compensation;
            textBox_AM0_R_Margin.Text = up.textBox_AM0_R_Margin;
            textBox_AM0_G_Margin.Text = up.textBox_AM0_G_Margin;
            textBox_AM0_B_Margin.Text = up.textBox_AM0_B_Margin;
            textBox_REF4095_Margin.Text = up.textBox_REF4095_Margin;
            textBox_Black_Limit_Lv.Text = up.textBox_Black_Limit_Lv;
            
            //REF0
            checkBox_VREF0_Comp.Checked = up.checkBox_VREF0_Comp;
            textBox_REF0_Margin.Text = up.textBox_REF0_Margin;
            numericUpDown_Set456_Skip_Max_Band.Value = up.numericUpDown_Set456_Skip_Max_Band;

            //Initial RGBVreg1 Finding Algorithm
            checkBox_Initial_RVreg1B_or_RGB_Algorithm_Apply.Checked = up.checkBox_Initial_RVreg1B_or_RGB_Algorithm_Apply;
            textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Upper_Limit.Text = up.textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Upper_Limit;
            textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Lower_Limit.Text = up.textBox_Initial_OC_RGB_Algorithm_Skip_Target_Lv_Lower_Limit;
            textBox_Initial_RGB_Algorithm_Lv_Combine_Ratio.Text = up.textBox_Initial_RGB_Algorithm_Lv_Combine_Ratio;
            radioButton_MCI_and_3Points.Checked = up.radioButton_MCI_and_3Points;
            radioButton_LUT_MCI.Checked = up.radioButton_LUT_MCI;

            //UVL
            checkBox_OC_Mode23_UVL_Check.Checked = up.checkBox_OC_Mode23_UVL_Check;

            //Min Distance
            radioButton_Min_Distance_N_Points_Measurements.Checked = up.radioButton_Min_Distance_N_Points_Measurements;
            textbox_min_distance_n_points.Text = up.textbox_min_distance_n_points;

            //Differece Offset
            radioButton_Difference_Offset.Checked = up.radioButton_Difference_Offset;
            textBox_Delta_L_Limit.Text = up.textBox_Delta_L_Limit;
            radioButton_None.Checked = up.radioButton_None;

            radioButton_AM0_AM1_REF0_ELVSS_OCMode_1.Checked = up.radioButton_AM0_AM1_REF0_ELVSS_OCMode_1;
            radioButton_AM0_AM1_REF0_ELVSS_OCMode_2.Checked = up.radioButton_AM0_AM1_REF0_ELVSS_OCMode_2;
            radioButton_AM0_AM1_REF0_ELVSS_OCMode_3.Checked = up.radioButton_AM0_AM1_REF0_ELVSS_OCMode_3;

            checkBox_Apply_OC_Mode1n3_LV_Lowerthan_OC_Mode2_LV.Checked = up.checkBox_Apply_OC_Mode1n3_LV_Lowerthan_OC_Mode2_LV;

            checkBox_OC_File_Refresh.Checked = up.checkBox_OC_File_Refresh;

            //Gray95/47/23 OC_Skip (Apply Interpolation)
            checkBox_Apply_Interpolation_Gray95.Checked = up.checkBox_Apply_Interpolation_Gray95;
            checkBox_Apply_Interpolation_Gray47.Checked = up.checkBox_Apply_Interpolation_Gray47;
            checkBox_Apply_Interpolation_Gray23.Checked = up.checkBox_Apply_Interpolation_Gray23;
            checkBox_Apply_Interpolation_OC_Mode123.Checked = up.checkBox_Apply_Interpolation_OC_Mode123;
            checkBox_Apply_Interpolation_OC_Mode456.Checked = up.checkBox_Apply_Interpolation_OC_Mode456;
            checkBox_Apply_Interpolation_OC_Mode_AOD.Checked = up.checkBox_Apply_Interpolation_OC_Mode_AOD;
            textBox_Interpolation_ToBeApplied_LV_OCMode123.Text = up.textBox_Interpolation_ToBeApplied_LV_OCMode123;
            textBox_Interpolation_ToBeApplied_LV_OCMode456.Text = up.textBox_Interpolation_ToBeApplied_LV_OCMode456;
            textBox_Interpolation_ToBeApplied_LV_AOD.Text = up.textBox_Interpolation_ToBeApplied_LV_AOD;
            checkBox_B0G255_Skip_If_Compensated.Checked = up.checkBox_B0G255_Skip_If_Compensated;
        }


        public int Get_Set456_Skip_Max_Band()
        {
            return Convert.ToInt32(numericUpDown_Set456_Skip_Max_Band.Value);
        }

        private int Get_Bist_Red()
        {
            int Gray = Convert.ToInt32(Bist_Red_Textbox.Text);

            if (Gray > 255)
                Gray = 255;

            if (Gray < 0)
                Gray = 0;

            Bist_Red_Textbox.Text = Gray.ToString();
            return Gray;
        }

        private int Get_Bist_Green()
        {
            int Gray = Convert.ToInt32(Bist_Green_Textbox.Text);

            if (Gray > 255)
                Gray = 255;

            if (Gray < 0)
                Gray = 0;

            Bist_Green_Textbox.Text = Gray.ToString();
            return Gray;
        }

        private int Get_Bist_Blue()
        {
            int Gray = Convert.ToInt32(Bist_Blue_Textbox.Text);

            if (Gray > 255)
                Gray = 255;

            if (Gray < 0)
                Gray = 0;

            Bist_Blue_Textbox.Text = Gray.ToString();
            return Gray;
        }

        private RGB Get_Updated_color_gray()
        {
            RGB color_gray = new RGB();
            color_gray.int_R = Get_Bist_Red();
            color_gray.int_G = Get_Bist_Green();
            color_gray.int_B = Get_Bist_Blue();
            return color_gray;
        }

        private void button_Bist_On_Click(object sender, EventArgs e)
        {
            RGB color_gray = Get_Updated_color_gray();

            string[] Params = new string[5];
            Params[0] = "03";
            Params[1] = "00";
            Params[2] = color_gray.int_R.ToString("X2");
            Params[3] = color_gray.int_G.ToString("X2");
            Params[4] = color_gray.int_B.ToString("X2");

            f1().Long_Packet_CMD_Send(5, "EF", Params);
        }

        private void button_Bist_Off_Click(object sender, EventArgs e)
        {
            f1().IPC_Quick_Send("mipi.write 0x15 0xEF 0x00");
        }


        private void groupBox15_Enter(object sender, EventArgs e)
        {

        }

        private void button_Read_ELVSS_and_Vinit2_Click(object sender, EventArgs e)
        {
            cmds.ELVSS_Vinit2_Text_Clear();
            if (radioButton_Cold_ELVSS_Vinit2.Checked)
            {
                cmds.Read_Cold_ELVSS_and_Vinit2_Voltage_and_Save_to_Textbox();
            }
            else if (radioButton_Normal_ELVSS_Vinit2.Checked)
            {
                cmds.Read_ELVSS_and_Vinit2_Voltage_and_Save_to_Textbox();
            }
        }

        private void button_ELVSS_Margin_Test_Click(object sender, EventArgs e)
        {
            DP213_ELVSS_Margin_Test A = new DP213_ELVSS_Margin_Test();
            A.ELVSS_Margin_Test_Start();
        }


        private void button_Send_ELVSS_Click(object sender, EventArgs e)
        {
            DP213_ELVSS_Margin_Test A = new DP213_ELVSS_Margin_Test();

            double ELVSS_Voltage = Convert.ToDouble(textBox_ELVSS_Voltage_For_Sending.Text);
            int band = Convert.ToInt32(textBox_ELVSS_Band_For_Sending.Text);
            A.Set_and_Send_ELVSS_CMD(ELVSS_Voltage, Gamma_Set.Set1, band);
            A.Set_and_Send_ELVSS_CMD(ELVSS_Voltage, Gamma_Set.Set2, band);
            A.Set_and_Send_ELVSS_CMD(ELVSS_Voltage, Gamma_Set.Set3, band);
        }


        public int Get_Margin_Tested_ELVSS_Band()
        {
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
            return Band;
        }

        public void ELVSS_Margin_Test_Initialize()
        {
            radioButton_Normal_ELVSS_Vinit2.Checked = true;
            button_Read_DP213_DBV_Setting.PerformClick();
            button_Read_ELVSS_and_Vinit2.PerformClick();
            button_Gamma_Set1_Apply.PerformClick();
            Application.DoEvents();

        }

        private void label97_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_Copy_Mode12_Ave_Measure_to_Mode3_Target_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Copy_Mode12_Ave_Measure_to_Mode3_Target.Checked && checkBox_Copy_Mode2_Measure_to_Mode3_Target.Checked)
                checkBox_Copy_Mode2_Measure_to_Mode3_Target.Checked = false;
        }

        private void checkBox_Copy_Mode1_Measure_to_Mode3_Target_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Copy_Mode2_Measure_to_Mode3_Target.Checked && checkBox_Copy_Mode12_Ave_Measure_to_Mode3_Target.Checked)
                checkBox_Copy_Mode12_Ave_Measure_to_Mode3_Target.Checked = false;
        }

        private void groupBox22_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

