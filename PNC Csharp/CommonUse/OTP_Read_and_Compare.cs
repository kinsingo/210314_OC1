using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PNC_Csharp
{
    public partial class OTP_Read_and_Compare : Form
    {
        bool OTP_Read_And_Compare_Stop;
        Mipi_Script mipi_script_controller;
        D_IC d_ic_model;

        private static OTP_Read_and_Compare Instance;
        public static OTP_Read_and_Compare getInstance()
        {
            if (Instance == null)
                Instance = new OTP_Read_and_Compare();

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

        private OTP_Read_and_Compare()
        {
            InitializeComponent();
        }

        private void OTP_Read_and_Compare_Load(object sender, EventArgs e)
        {
            mipi_script_controller = new Mipi_Script_Related();
        }
        private void Hide_button_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Button_OTP_Compare_Click(object sender, EventArgs e)
        {
            OTP_Read_And_Compare_Stop = false;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            textBox_Show_Compared_Mipi_Data.Clear();
            richTextBox_Mipi_Finally_Compared.Clear();
            f1.GB_Status_AppendText_Nextline("OTP Read and Compare Start", Color.Black);
            Application.DoEvents();

            //Step1 (1st to 2nd (Transform))
            mipi_script_controller.Transfrom_Mipi_Cript(textBox_Mipi_Data_To_Be_Compared, textBox_Show_Compared_Mipi_Data, false);

            //Step2 (2nd to 3rd (Read and Compare))
            mipi_script_controller.Read_and_Compare(ref OTP_Read_And_Compare_Stop, textBox_Show_Compared_Mipi_Data, richTextBox_Mipi_Finally_Compared, D_IC.Normal);
        }

        private void SetrichTextBox_Mipi_Finally_Compared(String A , Color color)
        {
            richTextBox_Mipi_Finally_Compared.SelectionColor = color;
            richTextBox_Mipi_Finally_Compared.AppendText(A + "\r\n");            
            richTextBox_Mipi_Finally_Compared.SelectionStart = richTextBox_Mipi_Finally_Compared.Text.Length;
            richTextBox_Mipi_Finally_Compared.ScrollToCaret();
        }

        private void Button_DP086_OTP_Compare_Click(object sender, EventArgs e)
        {
            OTP_Read_And_Compare_Stop = false;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            textBox_Show_Compared_Mipi_Data.Clear();
            richTextBox_Mipi_Finally_Compared.Clear();
            f1.GB_Status_AppendText_Nextline("OTP Read and Compare Start", Color.Black);
            Application.DoEvents();

            //Step1 (1st to 2nd (Transform))
            mipi_script_controller.Transfrom_Mipi_Cript(textBox_Mipi_Data_To_Be_Compared, textBox_Show_Compared_Mipi_Data,false);

            //Step2 (2nd to 3rd (Read and Compare))
            mipi_script_controller.Read_and_Compare(ref OTP_Read_And_Compare_Stop, textBox_Show_Compared_Mipi_Data, richTextBox_Mipi_Finally_Compared, D_IC.Novatec);
        
        }

        private void button_textBox_Mipi_Data_To_Be_Compared_Clear_Click(object sender, EventArgs e)
        {
            textBox_Mipi_Data_To_Be_Compared.Clear();
        }



        

        private void Button_DP173_OTP_Compare_Click(object sender, EventArgs e)
        {
            OTP_Read_And_Compare_Stop = false;
            
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            textBox_Show_Compared_Mipi_Data.Clear();
            richTextBox_Mipi_Finally_Compared.Clear();
            f1.GB_Status_AppendText_Nextline("OTP Read and Compare Start", Color.Black);
            Application.DoEvents();

            //Step1 (1st to 2nd (Transform))
            mipi_script_controller.Transfrom_Mipi_Cript(textBox_Mipi_Data_To_Be_Compared, textBox_Show_Compared_Mipi_Data, false);

            //Step2 (2nd to 3rd (Read and Compare))
            mipi_script_controller.Read_and_Compare(ref OTP_Read_And_Compare_Stop, textBox_Show_Compared_Mipi_Data, richTextBox_Mipi_Finally_Compared, D_IC.Magna);
        }

        private void Button_Meta_OTP_Compare_Click(object sender, EventArgs e)
        {
            OTP_Read_And_Compare_Stop = false;

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            textBox_Show_Compared_Mipi_Data.Clear();
            richTextBox_Mipi_Finally_Compared.Clear();
            f1.GB_Status_AppendText_Nextline("OTP Read and Compare Start", Color.Black);
            Application.DoEvents();

            //Step1 (1st to 2nd (Transform))
            mipi_script_controller.Transfrom_Mipi_Cript(textBox_Mipi_Data_To_Be_Compared, textBox_Show_Compared_Mipi_Data, false);

            //Step2 (2nd to 3rd (Read and Compare))
            mipi_script_controller.Read_and_Compare(ref OTP_Read_And_Compare_Stop, textBox_Show_Compared_Mipi_Data, richTextBox_Mipi_Finally_Compared, D_IC.SIW);
        }

        private void button_OTP_Read_And_Compare_Stop_Click(object sender, EventArgs e)
        {
            OTP_Read_And_Compare_Stop = true;
        }
    }
}
