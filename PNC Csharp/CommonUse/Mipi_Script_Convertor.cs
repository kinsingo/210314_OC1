using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PNC_Csharp
{
    public partial class Mipi_Script_Convertor : Form
    {
        Mipi_Script mipi_script_controller;


        private static Mipi_Script_Convertor Instance;
        public static Mipi_Script_Convertor getInstance()
        {
            if (Instance == null)
                Instance = new Mipi_Script_Convertor();

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
        private Mipi_Script_Convertor()
        {
            InitializeComponent();
        }
        private void Mipi_Script_Convertor_Load(object sender, EventArgs e)
        {
            mipi_script_controller = new Mipi_Script_Related();
        }

       
        private void Button_Convert_Script_For_Doowon_FI_Machine_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            textBox_Show_Compared_Mipi_Data.Clear();
            richTextBox_Mipi_Finally_Converted.Clear();
            f1.GB_Status_AppendText_Nextline("Script Converting Start", Color.Black);
            Application.DoEvents();
            
            //Step1 (1st to 2nd (Transform))
            mipi_script_controller.Transfrom_Mipi_Cript(textBox_Mipi_Data_To_Be_Compared, textBox_Show_Compared_Mipi_Data, true);

            //Step2 (2nd to 3rd (Final Transform))
            mipi_script_controller.Convert_mipi_Script_For_OC_or_FI_Machine(textBox_Show_Compared_Mipi_Data, richTextBox_Mipi_Finally_Converted,Vendor.Doowon);
        }

        private void Button_Convert_Script_For_Guil_OC_Machine_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            textBox_Show_Compared_Mipi_Data.Clear();
            richTextBox_Mipi_Finally_Converted.Clear();
            f1.GB_Status_AppendText_Nextline("Script Converting Start", Color.Black);
            Application.DoEvents();

            //Step1 (1st to 2nd (Transform))
            mipi_script_controller.Transfrom_Mipi_Cript(textBox_Mipi_Data_To_Be_Compared, textBox_Show_Compared_Mipi_Data, true);

            //Step2 (2nd to 3rd (Final Transform))
            mipi_script_controller.Convert_mipi_Script_For_OC_or_FI_Machine(textBox_Show_Compared_Mipi_Data, richTextBox_Mipi_Finally_Converted, Vendor.Guil);
        }

        private void button_textBox_Mipi_Data_To_Be_Compared_Clear_Click(object sender, EventArgs e)
        {
            textBox_Mipi_Data_To_Be_Compared.Clear();
        }

        private void Hide_button_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        
    }
}
