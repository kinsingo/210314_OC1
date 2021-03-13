using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation;
using LGD_OC_AstractPlatForm.Enums;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.DiaLog;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Factory;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll
{
    public partial class Form1 : Form
    {
        const int channel_length = 8;
        bool[] IsCAConnected = new bool[channel_length];
        bool[] IsSampleDisplayed = new bool[channel_length];
        IBusinessAPI[] Channel_API = new IBusinessAPI[channel_length];
        RichTextBox[] richTextBoxes = new RichTextBox[channel_length];
        CompensationFacade[] Channel_OC = new CompensationFacade[channel_length];
        byte[][] ReadData = new byte[channel_length][];
        double[][] MeasuredXYLv = new double[channel_length][];

        public RichTextBox[] GetRichTextBoxes()
        {
            return richTextBoxes;
        }

        public Form1()
        {
            InitializeComponent();
            for (int ch = 0; ch < channel_length; ch++)
            {
                IsCAConnected[ch] = false;
                IsSampleDisplayed[ch] = false;
                Channel_API[ch] = null;
                MeasuredXYLv[ch] = new double[3];
            }
            richTextBoxes[0] = richTextBox_ch1;
            richTextBoxes[1] = richTextBox_ch2;
            richTextBoxes[2] = richTextBox_ch3;
            richTextBoxes[3] = richTextBox_ch4;
            richTextBoxes[4] = richTextBox_ch5;
            richTextBoxes[5] = richTextBox_ch6;
            richTextBoxes[6] = richTextBox_ch7;
            richTextBoxes[7] = richTextBox_ch8;
        }

        private void button_RichTestBox_Clear_Click(object sender, EventArgs e)
        {
            richTextBox_ch1.Clear();
            richTextBox_ch2.Clear();
            richTextBox_ch3.Clear();
            richTextBox_ch4.Clear();
            richTextBox_ch5.Clear();
            richTextBox_ch6.Clear();
            richTextBox_ch7.Clear();
            richTextBox_ch8.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VendorForm.GetInstance().Show();
            this.Enabled = false;
        }

        private void button_Read_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            byte Address = Convert.ToByte(Convert.ToInt32(textBox_Read_Address.Text, 16));
            int amount = Convert.ToInt32(textBox_Read_HowMany.Text);
            int offset = Convert.ToInt32(textBox_Read_Offset.Text);
            
            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Test) IsSampleDisplayed[ch] = true;

                if (IsSampleDisplayed[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch]);
                    ReadData[ch] = Channel_API[ch].ReadData(Address, amount, offset,ch);
                }
            }
        }

        private void button_Write_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            byte Address = Convert.ToByte(Convert.ToInt32(textBox_Write_Address.Text, 16));
            
            string[] HexData = textBox_Write_MipiData.Text.Split(' ');
            byte[] parameters = new byte[HexData.Length];
            for (int i = 0; i < HexData.Length; i++)
                parameters[i] = Convert.ToByte(Convert.ToInt32(HexData[i].Substring(2), 16));

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Test) IsSampleDisplayed[ch] = true;
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD) IsSampleDisplayed[ch] = true;//for test

                if (IsSampleDisplayed[ch])
                {
                    richTextBoxes[ch].AppendText("ch : " + ch);

                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch]);
                    Channel_API[ch].WriteData(Address, parameters,ch);
                }
            }
        }

        private void button_Measure_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Test) IsCAConnected[ch] = true;

                if (IsCAConnected[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch]);
                    MeasuredXYLv[ch] = Channel_API[ch].measure_XYL(ch);
                }
            }
        }

        private void button_Compensation_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Test)
                {
                    IsCAConnected[ch] = true;
                    IsSampleDisplayed[ch] = true;
                }

                if (IsCAConnected[ch] && IsSampleDisplayed[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch]);
                    if (Channel_OC[ch] == null) Channel_OC[ch] = new CompensationFacade(Channel_API[ch]);

                    Channel_OC[ch].OpticCompensation(Model.DP213);
                }
            }
        }

        private void button_Display_Pattern_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            byte R = Convert.ToByte(textBox_R.Text);
            byte G = Convert.ToByte(textBox_G.Text);
            byte B = Convert.ToByte(textBox_B.Text);
            byte[] RGB = new byte[3] { R, G, B };

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Test) IsSampleDisplayed[ch] = true;
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD) IsSampleDisplayed[ch] = true;//for test

                if (IsSampleDisplayed[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch]);
                    Channel_API[ch].DisplayMonoPattern(RGB,ch);
                }
            }
        }

        private void button_CA_Connect_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button_display_box_pattern_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());


            byte f_red = Convert.ToByte(textBox_box_fore_R.Text);
            byte f_green = Convert.ToByte(textBox_box_fore_G.Text);
            byte f_blue = Convert.ToByte(textBox_box_fore_B.Text);
            byte[] Box_RGB = new byte[3] { f_red, f_green, f_blue };


            byte b_red = Convert.ToByte(textBox_box_back_R.Text);
            byte b_green = Convert.ToByte(textBox_box_back_G.Text);
            byte b_blue = Convert.ToByte(textBox_box_back_B.Text);
            byte[] BackGround_RGB = new byte[3] { b_red, b_green, b_blue };

            int box_left = Convert.ToInt32(textBox_Pos_BoxLeft.Text);
            int box_top = Convert.ToInt32(textBox_Pos_BoxTop.Text);
            int[] Pos_BoxLeftTop = new int[2] { box_left, box_top };

            int box_right = Convert.ToInt32(textBox_Pos_BoxRight.Text);
            int box_bottom = Convert.ToInt32(textBox_Pos_BoxBottom.Text);
            int[] Pos_BoxRightBottom = new int[2] { box_right, box_bottom };

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Test) IsSampleDisplayed[ch] = true;
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD) IsSampleDisplayed[ch] = true;//for test

                if (IsSampleDisplayed[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch]);
                    Channel_API[ch].DisplayBoxPattern(Box_RGB, BackGround_RGB, Pos_BoxLeftTop, Pos_BoxRightBottom,ch);
                }
            }

        }
    }
}
