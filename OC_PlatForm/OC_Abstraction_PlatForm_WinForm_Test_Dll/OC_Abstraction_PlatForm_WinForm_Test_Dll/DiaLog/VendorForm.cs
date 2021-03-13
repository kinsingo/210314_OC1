using System;
using System.Windows.Forms;
using System.Drawing;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.DiaLog
{
    public partial class VendorForm : Form
    {
        WhichVendor vendor;

        private static VendorForm instance = null;
        public static VendorForm GetInstance()
        {
            if (instance == null)
                instance = new VendorForm();
            return instance;
        }


        public WhichVendor GetVendor()
        {
            return vendor;
        }

        public int GetChannelLength()
        {
            if (vendor == WhichVendor.LGD)
                return 1;
            else
                return 8;
        }
        protected VendorForm()
        {
            InitializeComponent();
        }

        private void Gooil_Click(object sender, EventArgs e)
        {
            vendor = WhichVendor.Gooil;
            this.Hide();

            Form f1 = (Form)Application.OpenForms["Form1"];
            f1.Enabled = true;
        }

        private void button_LGD_Click(object sender, EventArgs e)
        {
            vendor = WhichVendor.LGD;
            this.Hide();

            var f1 = (Form1)Application.OpenForms["Form1"];
            f1.Enabled = true;

            var richtextboxes = f1.GetRichTextBoxes();
            richtextboxes[0].BackColor = Color.White;
            for (int i = 1; i < richtextboxes.Length; i++)
                richtextboxes[i].BackColor = Color.Black;
        }

        private void button_TestMode_VirtualVendor_Click(object sender, EventArgs e)
        {
            vendor = WhichVendor.Test;
            this.Hide();

            Form f1 = (Form)Application.OpenForms["Form1"];
            f1.Enabled = true;
        }
    }
}
