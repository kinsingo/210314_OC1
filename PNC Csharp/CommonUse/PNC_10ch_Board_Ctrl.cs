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
    public partial class PNC_10ch_Board_Ctrl : Form
    {
        int PNC_ch1, PNC_ch2, PNC_ch3, PNC_ch4, PNC_ch5, PNC_ch6, PNC_ch7, PNC_ch8, PNC_ch9, PNC_ch10;
        string Binary_10ch = "";
        string Hex_10ch = "";

        private static PNC_10ch_Board_Ctrl Instance;
        public static PNC_10ch_Board_Ctrl getInstance()
        {
            if (Instance == null)
                Instance = new PNC_10ch_Board_Ctrl();

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

        private PNC_10ch_Board_Ctrl()
        {
            InitializeComponent();
        }

        private void PNC_10ch_Board_Ctrl_Load(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }



        private void checkBox_ch1_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void checkBox_ch2_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        } 

        private void checkBox_ch3_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void checkBox_ch4_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void checkBox_ch5_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void checkBox_ch6_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void checkBox_ch7_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void checkBox_ch8_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void checkBox_ch9_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }

        private void checkBox_ch10_CheckedChanged(object sender, EventArgs e)
        {
            Check_Box_Update_For_PNC_Ch_Control();
        }


        private void Check_Box_Update_For_PNC_Ch_Control()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (checkBox_ch1.Checked)
                PNC_ch1 = 1;
            else
                PNC_ch1 = 0;

            if (checkBox_ch2.Checked)
                PNC_ch2 = 1;
            else
                PNC_ch2 = 0;

            if (checkBox_ch3.Checked)
                PNC_ch3 = 1;
            else
                PNC_ch3 = 0;

            if (checkBox_ch4.Checked)
                PNC_ch4 = 1;
            else
                PNC_ch4 = 0;

            if (checkBox_ch5.Checked)
                PNC_ch5 = 1;
            else
                PNC_ch5 = 0;

            if (checkBox_ch6.Checked)
                PNC_ch6 = 1;
            else
                PNC_ch6 = 0;

            if (checkBox_ch7.Checked)
                PNC_ch7 = 1;
            else
                PNC_ch7 = 0;

            if (checkBox_ch8.Checked)
                PNC_ch8 = 1;
            else
                PNC_ch8 = 0;

            if (checkBox_ch9.Checked)
                PNC_ch9 = 1;
            else
                PNC_ch9 = 0;

            if (checkBox_ch10.Checked)
                PNC_ch10 = 1;
            else
                PNC_ch10 = 0;



            //Binary_10ch = PNC_ch1.ToString() + PNC_ch2.ToString() + PNC_ch3.ToString() + PNC_ch4.ToString() + PNC_ch5.ToString()
            //               + PNC_ch6.ToString() + PNC_ch7.ToString() + PNC_ch8.ToString() + PNC_ch9.ToString() + PNC_ch10.ToString();
            Binary_10ch = PNC_ch10.ToString() + PNC_ch9.ToString() + PNC_ch8.ToString() + PNC_ch7.ToString() + PNC_ch6.ToString()
                           + PNC_ch5.ToString() + PNC_ch4.ToString() + PNC_ch3.ToString() + PNC_ch2.ToString() + PNC_ch1.ToString();

            Hex_10ch = Convert.ToInt32(Binary_10ch, 2).ToString("X");

            //System.Windows.Forms.MessageBox.Show(Hex_10ch + "(hex) = " + Binary_10ch + "(binary)");
            f1.IPC_Quick_Send("mipi.nvd.wchannel 0x0" + Hex_10ch);
        }

        private void otp_1_read(DataGridView DGview)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DGview.Columns.Clear();
            DGview.ForeColor = System.Drawing.Color.Black;
            try
            {
                //textBox2_cmd.Text = "";
                f1.textBox2_cmd.Text = string.Empty;

                int dex_params_num = Convert.ToInt32(Textbox_How_many.Text);

                if (dex_params_num > 120)
                {
                    dex_params_num = 120;
                    System.Windows.Forms.MessageBox.Show("Cannot read more than 120 ea");
                }

                String[] sTxData = new string[] {
		        ("mipi.write 0x37 0x"+ dex_params_num.ToString("X")),
                ("mipi.read 0x06 0x"+Textbox_OTP_1_register.Text)
	            };

                foreach (string s in sTxData)
                {
                    //System.Windows.Forms.MessageBox.Show(s);
                    f1.IPC_Quick_Send(s);
                }


                DGview.DataSource = null; // reset (unbind the datasource)
                DGview.Columns.Clear();

                DGview.Columns.Add("Address", "Address");
                DGview.Columns.Add("Param", "Param");

                char[] mipi_data = f1.textBox2_cmd.Text.ToCharArray();
                string temp_data = "00";

                int count = 0;

                int i = 0;
                if (dex_params_num < 10)
                    i = 17;
                else
                    i = 18;

                for (; i < mipi_data.Length; i = i + 5)
                {
                    temp_data = new string(mipi_data, i + 2, 2);
                    DGview.Rows.Add("(" + Textbox_OTP_1_register.Text + ") " + (++count).ToString(), temp_data);

                    if (count == dex_params_num)
                        break;
                }
                //Textbox_How_many.Text = count.ToString();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("OTP Read fail !\nPlease check the Sample or System-connection status");
            }
            System.Windows.Forms.Application.DoEvents();
        }
        
        private void OTP_1_register_reader_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            dataGridView3.Columns.Clear();
            dataGridView4.Columns.Clear();
            dataGridView5.Columns.Clear();
            dataGridView6.Columns.Clear();
            dataGridView7.Columns.Clear();
            dataGridView8.Columns.Clear();
            dataGridView9.Columns.Clear();
            dataGridView10.Columns.Clear();

            if(PNC_ch1 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 0");
                otp_1_read(dataGridView1);
            }
            
            if (PNC_ch2 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 1");
                otp_1_read(dataGridView2);
            }

            if (PNC_ch3 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 2");
                otp_1_read(dataGridView3);
            }

            if (PNC_ch4 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 3");
                otp_1_read(dataGridView4);
            }

            if (PNC_ch5 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 4");
                otp_1_read(dataGridView5);
            }

            if (PNC_ch6 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 5");
                otp_1_read(dataGridView6);
            }

            if (PNC_ch7 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 6");
                otp_1_read(dataGridView7);
            }

            if (PNC_ch8 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 7");
                otp_1_read(dataGridView8);

            }
            if (PNC_ch9 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 8");
                otp_1_read(dataGridView9);
            }
            if (PNC_ch10 == 1)
            {
                f1.IPC_Quick_Send("mipi.nvd.rchannel 9");
                otp_1_read(dataGridView10);
            }
        }

        private void button_All_Ch_Select_Click(object sender, EventArgs e)
        {
            All_Ch_Select(true);
        }

        private void button_All_Ch_Deselect_Click(object sender, EventArgs e)
        {
            All_Ch_Select(false);
        }

        private void All_Ch_Select(bool All_Select)
        {
            checkBox_ch1.Checked = All_Select;
            checkBox_ch2.Checked = All_Select;
            checkBox_ch3.Checked = All_Select;
            checkBox_ch4.Checked = All_Select;
            checkBox_ch5.Checked = All_Select;
            checkBox_ch6.Checked = All_Select;
            checkBox_ch7.Checked = All_Select;
            checkBox_ch8.Checked = All_Select;
            checkBox_ch9.Checked = All_Select;
            checkBox_ch10.Checked = All_Select;
        }
    }
}
