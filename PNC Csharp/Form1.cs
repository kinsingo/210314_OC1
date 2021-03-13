using BSQH_Csharp_Library;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
//using References
using System.IO.MemoryMappedFiles;
using System.IO.Ports;//Port 190530
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using PNC_Csharp.Measurement_10ch;
using PNC_Csharp.CA_Multi_Channels;
using PNC_Csharp.Measurement_QA;

namespace PNC_Csharp
{
    public partial class Form1 : Form
    {
        //Global stringbuilder
        StringBuilder sb;
        public void SB_Append(string str)
        {
            sb.Append(str);
        }
        public void SB_Show()
        {
            GB_Status_AppendText_Nextline(sb.ToString(),Color.Purple);
        }
        public void SB_Clear()
        {
            sb.Clear();
        }

        //1ch mode
        //10ch mode
        public bool Is_1ch_CA_Mode;

        //Drawing Related
        Point Previouse_Pos;
        int GB_Lines = 0;

        //Real Total Timer
        int Start_Time_Hour = DateTime.Now.Hour;
        int Start_Time_Minute = DateTime.Now.Minute;
        int Start_Time_Second = DateTime.Now.Second;

        //PNC ACK Processing Speed
        int PNC_ACK_Sleep;
        int PNC_ACK_Loop_Max;
        
        //PNC parameters
        bool bIPC_Open = false;
        MemoryMappedFile m_hMemoryMapped = null;
        MemoryMappedViewAccessor m_hMemoryAccessor = null;
        EventWaitHandle evt = null;
        int cnt_Send = 0;
       
        //CA parameters
        public CA200SRVRLib.Ca200 objCa200;
        public CA200SRVRLib.Ca objCa;
        public CA200SRVRLib.Cas objCas;
        public CA200SRVRLib.Probe objProbe;
        public Boolean isMsr;
        public bool Is_Single_CA_is_connected = false;
        //long vbObjectError = -2147221504;
        public CA200SRVRLib.Memory objMemory; //171213 추가사항
        public CA200SRVRLib.IProbeInfo objProbeInfo; //171213 추가사항
        
        //OTP Write Verify Related Params
        string DP116_CRC_Check_CMD2_P0_4Ah;
        string DP116_CRC_Check_CMD2_P0_4Bh;
        string DP116_CRC_Check_CMD2_P0_4Ch;
        string DP116_CRC_Check_CMD2_P0_4Dh;
        int Write_Before_OTP_Zone_A;
        int Write_Before_OTP_Zone_B;
        int Write_Before_OTP_Zone_C;

        //Model 별 함수 접근가능하도록 Class 객체생성 (page_selection ... 등 함수포함)
        public Selected_Model current_model;

        //File 함수 접근 가능하도록 객체생성 , Manual/Auto Path 는 Textbox.Text 에 저장함
        public string AOD_In_Path;
        public string AOD_Out_Path;
        public string VR_In_Path;
        public string VR_Out_Path;
        public string Turn_Off_Path;

        bool GCS_BCS_Stop = false;
        bool Change_N_Measure_Stop = false;

        bool CA_First_Connection_Status = false;

        int GB_time_elapsed;
        bool bool_GB_stop = false;
        void GB_t_Tick(object sender, EventArgs e)
        {
            if (bool_GB_stop)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                return;
            }
            ++GB_time_elapsed;

            ((System.Windows.Forms.Label)((System.Windows.Forms.Timer)sender).Tag).Text = (GB_time_elapsed / 60).ToString().PadLeft(2, '0')
                + ":" + (GB_time_elapsed % 60).ToString().PadLeft(2, '0');
        }

        int Total_time_elapsed;
        bool bool_Total_stop = false;
        void Total_t_Tick(object sender, EventArgs e)
        {
            if (bool_Total_stop)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                return;
            }

            ++Total_time_elapsed;
            
            ((System.Windows.Forms.Label)((System.Windows.Forms.Timer)sender).Tag).Text = (Total_time_elapsed / 60).ToString().PadLeft(2, '0')
                + ":" + (Total_time_elapsed % 60).ToString().PadLeft(2, '0');
            
            Real_Total_Time_Update();
        }

        //GB & Total START
        public void OC_Timer_Start()
        {
            //Sub_OC_Timer_START();
            Sub_Total_Timer_START();
            Application.DoEvents();
        }

        public void OC_Timer_Stop()
        {
            //Sub_OC_Timer_STOP();
            Sub_Total_Timer_Stop();
            Application.DoEvents();
        }

        private void Sub_OC_Timer_START()
        {
            GB_time_elapsed = 0;
            bool_GB_stop = false;
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tag = GB_Time_label;
        }

        private void Sub_Total_Timer_START()
        {
            Total_time_elapsed = 0;
            bool_Total_stop = false;
            timer2.Enabled = true;
            timer2.Interval = 1000;
            timer2.Tag = Total_Time_label;
            Real_Total_Time_START();
        }

        private void Real_Total_Time_START()
        {
            Start_Time_Hour = DateTime.Now.Hour;
            Start_Time_Minute = DateTime.Now.Minute;
            Start_Time_Second = DateTime.Now.Second;
            Real_Total_Time_label.Text = "00:00";
        }

        private void Real_Total_Time_Update()
        {
            int Current_Time_Hour = DateTime.Now.Hour;
            int Current_Time_Minute = DateTime.Now.Minute;
            int Current_Time_Second = DateTime.Now.Second;

            int Diff_Hour = Current_Time_Hour - Start_Time_Hour;
            int Diff_Minute = Current_Time_Minute - Start_Time_Minute;
            int Diff_Second = Current_Time_Second - Start_Time_Second;

            int Total_Seconds = Diff_Hour * 3600 + Diff_Minute * 60 + Diff_Second;
          
            Real_Total_Time_label.Text = (Total_Seconds / 60).ToString().PadLeft(2, '0')
                 + ":" + (Total_Seconds % 60).ToString().PadLeft(2, '0');
        }
        
        //GB & Total STOP//
        private void Sub_OC_Timer_STOP()
        {
            GB_time_elapsed = 0;
            bool_GB_stop = true;
        }
        private void Sub_Total_Timer_Stop()
        {
            Total_time_elapsed = 0;
            bool_Total_stop = true;
        }

        //Form Parameter
        public Form1()
        {
            InitializeComponent();
            this.current_model = Selected_Model.getInstance();
            try
            {
                /////GB Timer Setting////// timer1 is used for GB Time
                bool_GB_stop = true;
                timer1.Enabled = true;
                timer1.Interval = 1000;
                timer1.Tag = GB_Time_label;
                timer1.Tick += new EventHandler(GB_t_Tick);

                //////Total Timer Setting//// timer2 is used for Total Time
                bool_Total_stop = true;
                timer2.Enabled = true;
                timer2.Interval = 1000;
                timer2.Tag = Total_Time_label;
                timer2.Tick += new EventHandler(Total_t_Tick);
        

                //initial setting for components
                CA_Measure_button.Enabled = true;
                CA_zero_cal_button.Enabled = true;
               
                // PNC related functions and parameters
                IPC_Open();

                //Measure X,Y.Lv
                dataGridView2.EnableHeadersVisualStyles = false;
                dataGridView2.Columns.Add("Gray", "Gray");
                dataGridView2.Columns.Add("x", "x");
                dataGridView2.Columns.Add("y", "y");
                dataGridView2.Columns.Add("Lv", "Lv");
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
                dataGridView2.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

                //Create Object (190530)
                objCa200 = new CA200SRVRLib.Ca200();
              
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();

            }
        }



        // IPC Open (PNC connection)
        private void IPC_Open()
        {
            try
            {
                m_hMemoryMapped = MemoryMappedFile.OpenExisting("PNC_DIKI_IPC", MemoryMappedFileRights.ReadWrite);
                if (m_hMemoryMapped == null)
                {
                    System.Windows.Forms.MessageBox.Show("error");
                    bIPC_Open = false;
                }

                m_hMemoryAccessor = m_hMemoryMapped.CreateViewAccessor();
                if (m_hMemoryAccessor == null)
                {
                    System.Windows.Forms.MessageBox.Show("error");
                    bIPC_Open = false;
                }

                evt = new EventWaitHandle(false, EventResetMode.ManualReset, "PNC_DIKI_IPC_READ");
                bIPC_Open = true;
                
                
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Windows.Forms.MessageBox.Show("IPC Open Error !!");
                bIPC_Open = false;
            }

            if (bIPC_Open)
            {
                //System.Windows.Forms.MessageBox.Show("Diki is successfully connected to the PC");
                GB_Status_AppendText_Nextline("Diki is successfully connected to the PC", System.Drawing.Color.Green);

                PNC_connect.ForeColor = System.Drawing.Color.Green;
                label_PNC_Connection_Status.Text = "Diki connection Status : OK";
                label_PNC_Connection_Status.ForeColor = System.Drawing.Color.Green;
                groupBox5.Show();
                groupBox4.Show();
                groupBox1.Show();
                groupBox13.Show();
                groupBox3.Show();
                groupBox18.Show();
                groupBox9.Show();
                groupBox20.Show();
                groupBox_1st_Model_OTP_Check.Show();
                groupBox_2nd_Model_OTP_Check.Show();
                

                PNC_connect.Visible = false;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Diki hasn't not connected to the PC");
                PNC_connect.ForeColor = System.Drawing.Color.Red;
                label_PNC_Connection_Status.Text = "Diki connection Status : NG";
                label_PNC_Connection_Status.ForeColor = System.Drawing.Color.Red;
                groupBox5.Hide();
                groupBox4.Hide();
                groupBox1.Hide();
                groupBox13.Hide();
                groupBox3.Hide();
                groupBox18.Hide();
                groupBox9.Hide();
                groupBox20.Hide();
                groupBox_1st_Model_OTP_Check.Hide();
                groupBox_2nd_Model_OTP_Check.Hide();
                

                PNC_connect.Visible = true;
            }


        }

        // 3.  IPC mipi.read
        public void btn_Mipi_Read_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            String[] sTxData = new string[] {
		        ("mipi.write 0x37 0x0C 0x00"),
		        ("mipi.read 0x06 0xE2")
	            };
            foreach (string s in sTxData)
            {
                IPC_Quick_Send(s);
            }
        }


        public string IPC_Send_Readquicker(string Ipc_str)
        {
            //string tta="";
            string tta = string.Empty;
            
            byte[] WriteByte;
            string Ipc_tx = "s" + cnt_Send + Ipc_str;
            WriteByte = ASCIIEncoding.ASCII.GetBytes(Ipc_tx);
            evt.Set();

            // --------------------------------------------------------------------------
            // 1024 byte dummy send..
            byte[] DummyWrite;
            DummyWrite = new byte[1024];
            for (int i = 0; i < 1024; i++) DummyWrite[i] = 0;
            m_hMemoryAccessor.WriteArray<byte>(0, DummyWrite, 0, DummyWrite.Length);
            System.Threading.Thread.Sleep(2);

            // --------------------------------------------------------------------------
            // command send
            m_hMemoryAccessor.WriteArray<byte>(0, WriteByte, 0, WriteByte.Length);
            cnt_Send++;
            if (cnt_Send == 10)
            {
                cnt_Send = 0;
            }
            System.Threading.Thread.Sleep(10);

            byte[] bReadData;
            bReadData = new byte[1024];
            for (int i = 0; i < 50; i++)
            {
                System.Threading.Thread.Sleep(100);
                m_hMemoryAccessor.ReadArray<byte>(0, bReadData, 0, bReadData.Length);
                if (bReadData[0] == 'r')
                {
                    break;
                }
                else if (bReadData[0] == 'm')
                {
                    tta = Encoding.Default.GetString(bReadData) + "\r\n";
                    break;
                }
            }
            evt.Reset();
            return tta;
        }

        public void IPC_Quick_Send_And_Show(string Ipc_str,Color color)
        {
            IPC_Quick_Send(Ipc_str);
            GB_Status_AppendText_Nextline(Ipc_str, color);
        }

        public void IPC_Quick_Send(string Ipc_str)
        {
            byte[] WriteByte;
            string Ipc_tx = "s" + cnt_Send + Ipc_str;
            WriteByte = ASCIIEncoding.ASCII.GetBytes(Ipc_tx);
            evt.Set();

            // --------------------------------------------------------------------------
            // 1024 byte dummy send..
            byte[] DummyWrite;
            DummyWrite = new byte[1024];
            m_hMemoryAccessor.WriteArray<byte>(0, DummyWrite, 0, DummyWrite.Length);
            System.Threading.Thread.Sleep(2);
            
            // --------------------------------------------------------------------------
            // command send
            m_hMemoryAccessor.WriteArray<byte>(0, WriteByte, 0, WriteByte.Length);
            cnt_Send++;
            if (cnt_Send == 10)
            {
                cnt_Send = 0;
            }
            System.Threading.Thread.Sleep(10);

            byte[] bReadData;
            bReadData = new byte[1024];
            
            //To make sure during iteration these params will not be changed
            int PNC_ACK_Sleep_ms = PNC_ACK_Sleep;
            int PNC_ACK_Loop_Max_local = PNC_ACK_Loop_Max;

            for (int Ack = 0; Ack < PNC_ACK_Loop_Max_local; Ack++)
            {
                if (checkBox_Show_PNC_Communication_Status.Checked)
                    GB_Status_AppendText_Nextline("AckCount/IntervalTime(ms) : " + (Ack + 1).ToString() + "/" + PNC_ACK_Sleep_ms.ToString(), Color.DarkRed);

                System.Threading.Thread.Sleep(PNC_ACK_Sleep_ms);

                m_hMemoryAccessor.ReadArray<byte>(0, bReadData, 0, bReadData.Length);
                if (bReadData[0] == 'r')
                {
                    if (bReadData[1] == '9')//Add on 191024 (Data Broken, neet to resend)
                    {
                        GB_Status_AppendText_Nextline("#== (Data Broken)Manual Mipi Data Resend ==", Color.Blue);
                    }     
                    break;
                }
                else if (bReadData[0] == 'm')
                {
                    string tta = Encoding.Default.GetString(bReadData) + "\r\n";
                    textBox2_cmd.AppendText(tta);
                    break;
                }
            }
            evt.Reset();
        }

        

       

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            objCa.RemoteMode = 0;
            objCa200 = null;
            objCa = null;
            objProbe = null;

            if (m_hMemoryMapped != null)
            {
                m_hMemoryMapped.Dispose();
                m_hMemoryAccessor.Dispose();
            }
        }

        private void PNC_connect_Click(object sender, EventArgs e)
        {
            IPC_Open();
        }

        public void Set_GCS(int R,int G,int B)
        {   
            //IPC_Quick_Send("image.mono" + " " + Gray + " " + Gray + " " + Gray);
            if (trackBar_APL.Value == trackBar_APL.Maximum)
            {
                IPC_Quick_Send("image.mono" + " " + R.ToString() + " " + G.ToString() + " " + B.ToString());
            }
            else
            {
                double APL = trackBar_APL.Value * 0.1;
                double X = current_model.get_X();
                double Y = current_model.get_Y();

                int x1, y1;
                double APL_SQRT = Math.Sqrt(APL);
                x1 = Convert.ToInt16((X * APL_SQRT));
                y1 = Convert.ToInt16((Y * APL_SQRT));
                Image_Crosstalk(x1, y1, 0, 0, 0, R, G, B);
            }
        }

        public void Set_BCS(int dbv)
        {
            string DBV1, DBV2;
            if (dbv <= 255) //255 = FF
            {
                DBV1 = "00";
                DBV2 = dbv.ToString("X").PadLeft(2, '0');//dex to hex (as a string form)
            }
            else if (dbv <= 511) //511 = 1FF
            {
                DBV1 = "01";
                DBV2 = (dbv - 256).ToString("X"); //256 = 100
            }
            else if (dbv <= 767) //767 = 2FF
            {
                DBV1 = "02";
                DBV2 = (dbv - 512).ToString("X"); //512 = 200
            }
            else if (dbv <= 1023) //767 = 3FF
            {
                DBV1 = "03";
                DBV2 = (dbv - 768).ToString("X"); //768 = 300
            }
            else if (dbv <= 1279) //1279 = 4FF
            {
                DBV1 = "04";
                DBV2 = (dbv - 1024).ToString("X"); //1024 = 400
            }
            else if (dbv <= 1535)
            {
                DBV1 = "05";
                DBV2 = (dbv - 1280).ToString("X"); //1280 = 500
            }
            else if (dbv <= 1791)
            {
                DBV1 = "06";
                DBV2 = (dbv - 1536).ToString("X"); //1536 = 600

            }
            else if (dbv <= 2047)
            {
                DBV1 = "07";
                DBV2 = (dbv - 1792).ToString("X"); //1792 = 700
            }
            else if (dbv <= 2303)
            {
                DBV1 = "08";
                DBV2 = (dbv - 2048).ToString("X"); //2048 = 800
            }
            else if (dbv <= 2559)
            {
                DBV1 = "09";
                DBV2 = (dbv - 2304).ToString("X"); //2304 = 900
            }
            else if (dbv <= 2815)
            {
                DBV1 = "0A";
                DBV2 = (dbv - 2560).ToString("X"); //2560 = A00
            }
            else if (dbv <= 3071)
            {
                DBV1 = "0B";
                DBV2 = (dbv - 2816).ToString("X"); //2816 = B00
            }
            else if (dbv <= 3327)
            {
                DBV1 = "0C";
                DBV2 = (dbv - 3072).ToString("X"); //3072 = C00
            }
            else if (dbv <= 3583)
            {
                DBV1 = "0D";
                DBV2 = (dbv - 3328).ToString("X"); //3328 = D00
            }
            else if (dbv <= 3839)
            {
                DBV1 = "0E";
                DBV2 = (dbv - 3584).ToString("X"); //3584 = E00
            }
            else //(dbv <= 4095)
            {
                DBV1 = "0F";
                DBV2 = (dbv - 3840).ToString("X"); //3840 = F00
            }

            if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
            {
                IPC_Quick_Send("mipi.write 0x15 0xFF 0x10");//Page Selection for Novatec IC UCS (DP116/DP086)
                First_Model_Option_Form.getInstance().Current_Page_Address = "0x10";
                if (radioButton_Debug_Status_Mode.Checked) GB_Status_AppendText_Nextline("After Current_Page_Address = " + First_Model_Option_Form.getInstance().Current_Page_Address, Color.Black);
            }
            IPC_Quick_Send("mipi.write 0x39 0x51 0x" + DBV1 + " 0x" + DBV2);
        }

     
    
        private void PTN_Display_Click(object sender, EventArgs e)
        {
            string[] RGB = new string[3];
            int[] rgb = new int[3];
            rgb[0] = Convert.ToInt32(Red_Textbox.Text);
            rgb[1] = Convert.ToInt32(Green_Textbox.Text);
            rgb[2] = Convert.ToInt32(Blue_Textbox.Text);

            for (int i = 0; i < 3; i++)
            {
                if (rgb[i] > 255)
                    rgb[i] = 255;
                if (rgb[i] < 0)
                    rgb[i] = 0;
            }

            Red_Textbox.Text = rgb[0].ToString();
            Green_Textbox.Text = rgb[1].ToString();
            Blue_Textbox.Text = rgb[2].ToString();

            IPC_Quick_Send("image.mono" + " " + Red_Textbox.Text + " " + Green_Textbox.Text + " " + Blue_Textbox.Text);
        }

        public void APL_PTN_update(int R, int G, int B, double APL)
        {
            double X = current_model.get_X();
            double Y = current_model.get_Y();
            
            double APL_SQRT = Math.Sqrt(APL);
            int x1 = Convert.ToInt16((X * APL_SQRT));
            int y1 = Convert.ToInt16((Y * APL_SQRT));
            Image_Crosstalk(x1, y1, 0, 0, 0, R, G, B);
        }



        public void PTN_update(int R, int G, int B)
        {
            Red_Textbox.Text = R.ToString();
            Green_Textbox.Text = G.ToString();
            Blue_Textbox.Text = B.ToString();

            IPC_Quick_Send("image.mono" + " " + Red_Textbox.Text + " " + Green_Textbox.Text + " " + Blue_Textbox.Text);
        }

        public void DP150_IRC_Box_PTN_update(int R, int G, int B,bool Up_Box)
        {

            Red_Textbox.Text = R.ToString();
            Green_Textbox.Text = G.ToString();
            Blue_Textbox.Text = B.ToString();
            //image.box2 R G B 255 255 255 440 1 800 361
            if (Up_Box) IPC_Quick_Send("image.box2" + " " + Red_Textbox.Text + " " + Green_Textbox.Text + " " + Blue_Textbox.Text
                + " " + 255 + " " + 255 + " " + 255 + " " + 440 + " " + 1 + " " + 800 + " " + 361);
            //image.box2 R G B 0 0 0 440 2280 800 2641
            else IPC_Quick_Send("image.box2" + " " + Red_Textbox.Text + " " + Green_Textbox.Text + " " + Blue_Textbox.Text
                + " " + 0 + " " + 0 + " " + 0 + " " + 440 + " " + 2280 + " " + 800 + " " + 2641);     
        }

        private void Red_Display_Click(object sender, EventArgs e)
        {
            PTN_update(255, 0, 0);
        }

        private void Green_Display_Click(object sender, EventArgs e)
        {
            PTN_update(0, 255, 0);
        }

        private void Blue_Display_Click(object sender, EventArgs e)
        {
            PTN_update(0, 0, 255);


        }

        private void G255_Display_Click(object sender, EventArgs e)
        {
            int gray = 255;
            PTN_update(gray, gray, gray);


        }

        private void G191_Display_Click(object sender, EventArgs e)
        {
            int gray = 191;
            PTN_update(gray, gray, gray);
           
        }

        private void G127_Display_Click(object sender, EventArgs e)
        {
            int gray = 127;
            PTN_update(gray, gray, gray);  
        }

        private void G63_Display_Click(object sender, EventArgs e)
        {
            int gray = 63;
            PTN_update(gray, gray, gray);
        }

        private void G31_Display_Click(object sender, EventArgs e)
        {
            int gray = 31;
            PTN_update(gray, gray, gray);
        }

        private void G15_Display_Click(object sender, EventArgs e)
        {
            int gray = 15;
            PTN_update(gray, gray, gray);
        }

        private void G7_Display_Click(object sender, EventArgs e)
        {
            int gray = 7;
            PTN_update(gray, gray, gray);
        }

        private void G1_Display_Click(object sender, EventArgs e)
        {
            int gray = 1;
            PTN_update(gray, gray, gray);
        }

        private void G0_Display_Click(object sender, EventArgs e)
        {
            int gray = 0;
            PTN_update(gray, gray, gray);
        }

        private void change_DBV_button_Click(object sender, EventArgs e)
        {
            DBV_Change();
        }

        private void DBV_trackbar_ValueChanged(object sender, EventArgs e)
        {
            DBV_textbox.Text = DBV_trackbar.Value.ToString("X"); //dex to hex (as a string form)
            DBV_Change();
        }

        private void DBV_Change()
        {
            if (Convert.ToInt32(DBV_textbox.Text, 16) > DBV_trackbar.Maximum)
                DBV_textbox.Text = DBV_trackbar.Maximum.ToString("X3"); ; //"FFF";
            if (Convert.ToInt32(DBV_textbox.Text, 16) < 0)
                DBV_textbox.Text = "0";

            int dbv = Convert.ToInt32(DBV_textbox.Text, 16);

            DBV_trackbar.Value = Convert.ToInt32(DBV_textbox.Text, 16);

            string DBV1, DBV2;
            if (dbv <= 255) //255 = FF
            {
                DBV1 = "00";
                DBV2 = dbv.ToString("X").PadLeft(2, '0');//dex to hex (as a string form)
            }
            else if (dbv <= 511) //511 = 1FF
            {
                DBV1 = "01";
                DBV2 = (dbv - 256).ToString("X"); //256 = 100
            }
            else if (dbv <= 767) //767 = 2FF
            {
                DBV1 = "02";
                DBV2 = (dbv - 512).ToString("X"); //512 = 200
            }
            else if (dbv <= 1023) //767 = 3FF
            {
                DBV1 = "03";
                DBV2 = (dbv - 768).ToString("X"); //768 = 300
            }
            else if (dbv <= 1279) //1279 = 4FF
            {
                DBV1 = "04";
                DBV2 = (dbv - 1024).ToString("X"); //1024 = 400
            }
            else if (dbv <= 1535)
            {
                DBV1 = "05";
                DBV2 = (dbv - 1280).ToString("X"); //1280 = 500
            }
            else if (dbv <= 1791)
            {
                DBV1 = "06";
                DBV2 = (dbv - 1536).ToString("X"); //1536 = 600

            }
            else if (dbv <= 2047)
            {
                DBV1 = "07";
                DBV2 = (dbv - 1792).ToString("X"); //1792 = 700
            }
            else if (dbv <= 2303)
            {
                DBV1 = "08";
                DBV2 = (dbv - 2048).ToString("X"); //2048 = 800
            }
            else if (dbv <= 2559)
            {
                DBV1 = "09";
                DBV2 = (dbv - 2304).ToString("X"); //2304 = 900
            }
            else if (dbv <= 2815)
            {
                DBV1 = "0A";
                DBV2 = (dbv - 2560).ToString("X"); //2560 = A00
            }
            else if (dbv <= 3071)
            {
                DBV1 = "0B";
                DBV2 = (dbv - 2816).ToString("X"); //2816 = B00
            }
            else if (dbv <= 3327)
            {
                DBV1 = "0C";
                DBV2 = (dbv - 3072).ToString("X"); //3072 = C00
            }
            else if (dbv <= 3583)
            {
                DBV1 = "0D";
                DBV2 = (dbv - 3328).ToString("X"); //3328 = D00
            }
            else if (dbv <= 3839)
            {
                DBV1 = "0E";
                DBV2 = (dbv - 3584).ToString("X"); //3584 = E00
            }
            else //(dbv <= 4095)
            {
                DBV1 = "0F";
                DBV2 = (dbv - 3840).ToString("X"); //3840 = F00
            }

            if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
            {
                IPC_Quick_Send("mipi.write 0x15 0xFF 0x10");
                First_Model_Option_Form.getInstance().Current_Page_Address = "0x10";
                if (radioButton_Debug_Status_Mode.Checked) GB_Status_AppendText_Nextline("After Current_Page_Address = " + First_Model_Option_Form.getInstance().Current_Page_Address, Color.Black);
            }

            IPC_Quick_Send("mipi.write 0x39 0x51 0x" + DBV1 + " 0x" + DBV2);
        }




        public void Exit_Whole_Program()
        {
            // When Exit , if CA is connected , disconnect the Ca from CPU
            if (Is_Single_CA_is_connected == true)
            {
                objCa.RemoteMode = 0;
                objCa200 = null;
                objCa = null;
                objProbe = null;
                objCas = null; //190530 Add
            }

            // When Exit , If PNC was connected to CPU , dispose the PNC memories from CPU
            if (m_hMemoryMapped != null)
            {
                m_hMemoryMapped.Dispose();
                m_hMemoryAccessor.Dispose();
            }

            System.Windows.Forms.Application.Exit();
            this.Close();
        }


        private void Exit_button_Click(object sender, EventArgs e)
        {
            Exit_Whole_Program();
        }


        //CA control
        private void objCa_ExeCalZero()
        {
            CA_Measure_button.Enabled = false;
            CA_zero_cal_button.Enabled = false;

            try
            {
                objCa.CalZero();
            }
            catch (Exception er)
            {
                DisplayError(er);
            }
            CA_Measure_button.Enabled = true;
            CA_zero_cal_button.Enabled = true;
        }

        private void CA_zero_cal_button_Click(object sender, EventArgs e)
        {
            bool calzero_success = false;

            while (calzero_success == false)
            {
                CA_zero_cal_button.Enabled = false;
                try
                {
                    objCa.CalZero();
                    calzero_success = true;
                }
                catch (Exception er)
                {
                    DisplayError(er);
                    if (System.Windows.Forms.MessageBox.Show("Zero Cal Error\r\nRetry?", "CalZero", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        objCa.RemoteMode = 0;
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
            CA_zero_cal_button.Enabled = true;
        }

        public void CA_Measure_button_Perform_Click(ref double Measured_X,ref double Measured_Y,ref double Measured_Lv)
        {
            CA_Measure_button.PerformClick();
            Measured_X = Convert.ToDouble(X_Value_display.Text);
            Measured_Y = Convert.ToDouble(Y_Value_display.Text);
            Measured_Lv = Convert.ToDouble(Lv_Value_display.Text);
        }
        private void Measure(string temp)
        {
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

            int i;
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;
                CA_zero_cal_button.Enabled = false;
                for (i = 0; i < 1; i++)
                {
                    objCa.Measure();


                    label6.Text = "x";
                    label7.Text = "y";
                    label8.Text = "Lv";
                    //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                    //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                    //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                    X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                    {
                        break;
                    }
                }

                CA_Measure_button.Enabled = true;
                CA_zero_cal_button.Enabled = true;

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)


                //Add data to Datagridview
                dataGridView2.Rows.Add(temp, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }
       

        public void CA_Measure_For_ELVSS(string ELVSS)
        {
            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.Columns[0].HeaderText = "ELVSS";
            Measure(ELVSS);
        }

        public void CA_Measure_For_AM0(string AM0)
        {
            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.Columns[0].HeaderText = "AM0";
            Measure(AM0);
        }




        public void CA_Measure_For_ELVSS(string ELVSS, ref double Measured_X, ref double Measured_Y, ref double Measured_Lv)
        {
            CA_Measure_For_ELVSS(ELVSS);
            Measured_X = Convert.ToDouble(X_Value_display.Text);
            Measured_Y = Convert.ToDouble(Y_Value_display.Text);
            Measured_Lv = Convert.ToDouble(Lv_Value_display.Text);
        }
        

        private void CA_Count_Measure(int count)
        {
            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView2.Columns[0].HeaderText = "Count";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

            int i;
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;
                CA_zero_cal_button.Enabled = false;
                for (i = 0; i < 1; i++)
                {
                    objCa.Measure();


                    label6.Text = "x";
                    label7.Text = "y";
                    label8.Text = "Lv";
                    //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                    //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                    //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                    X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                    {
                        break;
                    }
                }

                CA_Measure_button.Enabled = true;
                CA_zero_cal_button.Enabled = true;

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)


                //Add data to Datagridview
                dataGridView2.Rows.Add(count.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }


        private void CA_Image_Measure(int Image_Index)
        {
            //objCa.DisplayMode = 0; // 측정 모드는 xyLv
            //objCa.SyncMode = 0; //측정 모드는 NTSC

            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView2.Columns[0].HeaderText = "Image";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

            int i;
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;
                CA_zero_cal_button.Enabled = false;
                for (i = 0; i < 1; i++)
                {
                    objCa.Measure();


                    label6.Text = "x";
                    label7.Text = "y";
                    label8.Text = "Lv";
                    //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                    //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                    //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                    X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                    {
                        break;
                    }
                }

                CA_Measure_button.Enabled = true;
                CA_zero_cal_button.Enabled = true;

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)


                //Add data to Datagridview
                dataGridView2.Rows.Add(Image_Index.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        public void Dual_SH_IR_Drop_Delta_E_Measure(int delay_time_between_measurement)
        {
            Dual_SH_Delta_E form_sh_delta_e = (Dual_SH_Delta_E)System.Windows.Forms.Application.OpenForms["Dual_SH_Delta_E"];
            Dual_Engineer_Monitoring_Mode form_dual_mode = (Dual_Engineer_Monitoring_Mode)System.Windows.Forms.Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            DP150_Dual_Engineering_Mornitoring_Mode form_dual_DP150_mode = (DP150_Dual_Engineering_Mornitoring_Mode)System.Windows.Forms.Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];

            form_sh_delta_e.dataGridView1.Rows.Clear();
            form_sh_delta_e.dataGridView2.Rows.Clear();

            //For Average Measure Mode
            XYLv[] Measure = new XYLv[5];
            int Max_Index = 0; double Max_Value = 0;
            int Min_Index = 0; double Min_Value = 2000;

            //Index 0~49 에서 X/Y/Lv 먼저 찍음
            for (int index = 0; index <= 49 & form_sh_delta_e.Availability; index++)
            {
                form_sh_delta_e.DP116_IRC_Drop_Square_PTN_List(index);
                Thread.Sleep(delay_time_between_measurement);
                form_sh_delta_e.progressBar_GB.PerformStep();

                {//Condition 1
                    try
                    {
                        if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                            form_dual_mode.Conditon_1_Script_Apply();
                        else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                            form_dual_DP150_mode.Conditon_1_Script_Apply();
                               
                        Max_Index = 0; Min_Index = 0;
                        Max_Value = 0; Min_Value = 2000;
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }   

                        System.Windows.Forms.Application.DoEvents();
                       
                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;
                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview

                        form_sh_delta_e.dataGridView1.Rows.Add(index.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
                {//Condition 2
                    try
                    {
                        if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                            form_dual_mode.Conditon_2_Script_Apply();
                        else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                            form_dual_DP150_mode.Conditon_2_Script_Apply();

                        Max_Index = 0; Min_Index = 0;
                        Max_Value = 0; Min_Value = 2000;
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView2.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview

                        form_sh_delta_e.dataGridView2.Rows.Add(index.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView2.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView2.RowCount - 1;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
        }

        private void Sub_SH_IR_Drop_Delta_E_Measure(int index)
        {
            SH_Delta_E form_sh_delta_e = (SH_Delta_E)System.Windows.Forms.Application.OpenForms["SH_Delta_E"];
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;
                objCa.Measure();

                X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                
                System.Windows.Forms.Application.DoEvents();
                CA_Measure_button.Enabled = true;

                //Data Grid setting//////////////////////
                form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                form_sh_delta_e.dataGridView1.Rows.Add(index.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        public void SH_IR_Drop_Delta_E_Measure_For_50ea_PTNs(int delay_time_between_measurement)
        {
            SH_Delta_E form_sh_delta_e = (SH_Delta_E)System.Windows.Forms.Application.OpenForms["SH_Delta_E"];
            form_sh_delta_e.dataGridView1.Rows.Clear();

            //Index 0~49 에서 X/Y/Lv 먼저 찍음
            for (int index = 0; index <= 49 & form_sh_delta_e.Availability; index++)
            {
                form_sh_delta_e.IRC_Drop_Full_and_Square_50ea_PTN_List(index);
                Thread.Sleep(delay_time_between_measurement);
                form_sh_delta_e.progressBar_GB.PerformStep();
                Sub_SH_IR_Drop_Delta_E_Measure(index);
            }
        }

        public void SH_IR_Drop_Delta_E_Measure_For_94ea_PTNs(int delay_time_between_measurement)
        {
            SH_Delta_E form_sh_delta_e = (SH_Delta_E)System.Windows.Forms.Application.OpenForms["SH_Delta_E"];
            form_sh_delta_e.dataGridView1.Rows.Clear();

            //Index 0~93 에서 X/Y/Lv 먼저 찍음
            for (int index = 0; index <= 93 & form_sh_delta_e.Availability; index++)
            {
                form_sh_delta_e.IRC_Drop_Full_and_Square_94ea_PTN_List(index);
                Thread.Sleep(delay_time_between_measurement);
                form_sh_delta_e.progressBar_GB.PerformStep();
                Sub_SH_IR_Drop_Delta_E_Measure(index);
            }
        }

        public int Dual_SH_Delta_E2_Measure(int dbv_end_Point, int delay_time_between_measurement)
        {
            Dual_SH_Delta_E dual_form_sh_delta_e = (Dual_SH_Delta_E)System.Windows.Forms.Application.OpenForms["Dual_SH_Delta_E"];
            Dual_Engineer_Monitoring_Mode form_dual_mode = (Dual_Engineer_Monitoring_Mode)System.Windows.Forms.Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            DP150_Dual_Engineering_Mornitoring_Mode form_dual_DP150_mode = (DP150_Dual_Engineering_Mornitoring_Mode)System.Windows.Forms.Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];

            dual_form_sh_delta_e.dataGridView1.Rows.Clear();
            dual_form_sh_delta_e.dataGridView2.Rows.Clear();

            XYLv[] Measure = new XYLv[5];
            int Max_Index = 0; double Max_Value = 0;
            int Min_Index = 0; double Min_Value = 2000;

            int Step_Value = 0;

            if (dual_form_sh_delta_e.step_value_1.Checked)
                Step_Value = 1;
            else if (dual_form_sh_delta_e.step_value_4.Checked)
                Step_Value = 4;
            else if (dual_form_sh_delta_e.step_value_8.Checked)
                Step_Value = 8;
            else if (dual_form_sh_delta_e.step_value_16.Checked)
                Step_Value = 16;
            else
            {
                Step_Value = 0;
                System.Windows.Forms.MessageBox.Show("It's impossible");
            }

            int dbv = dbv_end_Point;
            int Times = 0;
            //Gray 48~255 에서 X/Y/Lv 먼저 찍음
            for (int i = dbv_end_Point; i < (Get_DBV_TrackBar_Maximum() + Step_Value) & dual_form_sh_delta_e.Availability; )
            {
                {//Condition 1
                    if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                        form_dual_mode.Conditon_1_Script_Apply();
                    else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                        form_dual_DP150_mode.Conditon_1_Script_Apply();

                    Max_Index = 0; Min_Index = 0;
                    Max_Value = 0; Min_Value = 2000;

                    i = i + Step_Value;
                    dbv = i - Step_Value;

                    if (dbv > Get_DBV_TrackBar_Maximum())
                        break;

                    Times++;

                    //PTN_update(gray, gray, gray);
                    this.Set_BCS(dbv);

                    Thread.Sleep(delay_time_between_measurement);
                    dual_form_sh_delta_e.progressBar_GB.PerformStep();
                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (dual_form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(dual_form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + dual_form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                                
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        dual_form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview

                        dual_form_sh_delta_e.dataGridView1.Rows.Add(dbv.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        dual_form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = dual_form_sh_delta_e.dataGridView1.RowCount - 1;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
                {//Condition 2
                    if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                         form_dual_mode.Conditon_2_Script_Apply();
                    else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                         form_dual_DP150_mode.Conditon_2_Script_Apply();

                    Max_Index = 0; Min_Index = 0;
                    Max_Value = 0; Min_Value = 2000;

                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (dual_form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(dual_form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + dual_form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                                
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                        }

                        else
                        {
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        dual_form_sh_delta_e.dataGridView2.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview

                        dual_form_sh_delta_e.dataGridView2.Rows.Add(dbv.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        dual_form_sh_delta_e.dataGridView2.FirstDisplayedScrollingRowIndex = dual_form_sh_delta_e.dataGridView2.RowCount - 1;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
            return Times;
        }
        

        


        public void SH_Delta_E2_Measure(int dbv_end_Point, int delay_time_between_measurement)
        {

            SH_Delta_E form_sh_delta_e = (SH_Delta_E)System.Windows.Forms.Application.OpenForms["SH_Delta_E"];
            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];

            form_sh_delta_e.dataGridView1.Rows.Clear();



            XYLv[] Measure = new XYLv[5];
            int Max_Index = 0; double Max_Value = 0;
            int Min_Index = 0; double Min_Value = 2000;


            int Step_Value = 0;

            if (form_sh_delta_e.step_value_1.Checked)
                Step_Value = 1;
            else if (form_sh_delta_e.step_value_4.Checked)
                Step_Value = 4;
            else if (form_sh_delta_e.step_value_8.Checked)
                Step_Value = 8;
            else if (form_sh_delta_e.step_value_16.Checked)
                Step_Value = 16;
            else
            {
                Step_Value = 0;
                System.Windows.Forms.MessageBox.Show("It's impossible");
            }

            int dbv = dbv_end_Point;

            if (form_sh_delta_e.radioButton_Min_to_Max.Checked)
            {
                for (int i = dbv_end_Point; i < ( f1.Get_DBV_TrackBar_Maximum() + Step_Value) & form_sh_delta_e.Availability; )
                {
                    Max_Index = 0; Min_Index = 0;
                    Max_Value = 0; Min_Value = 2000;

                    i = i + Step_Value;
                    dbv = i - Step_Value;
                    if (dbv > f1.Get_DBV_TrackBar_Maximum())
                        break;


                    //PTN_update(gray, gray, gray);
                    this.Set_BCS(dbv);

                    Thread.Sleep(delay_time_between_measurement);
                    form_sh_delta_e.progressBar_GB.PerformStep();
                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                                
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview

                        form_sh_delta_e.dataGridView1.Rows.Add(dbv.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
            else if (form_sh_delta_e.radioButton_Max_to_Min.Checked)
            {
                //for (int i = dbv_end_Point; i < (4095 + Step_Value) & form_sh_delta_e.Availability; )
                for (int i = (f1.Get_DBV_TrackBar_Maximum()); i > dbv_end_Point - Step_Value & form_sh_delta_e.Availability; )
                {
                    Max_Index = 0; Min_Index = 0;
                    Max_Value = 0; Min_Value = 2000;

                    /*
                    i = i + Step_Value;
                    dbv = i - Step_Value;
                    if (dbv > 4095)
                        break;
                    */
                    i = i - Step_Value;
                    dbv = i + Step_Value;
                    if (dbv < dbv_end_Point)
                        break;


                    //PTN_update(gray, gray, gray);
                    this.Set_BCS(dbv);

                    Thread.Sleep(delay_time_between_measurement);
                    form_sh_delta_e.progressBar_GB.PerformStep();
                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview

                        form_sh_delta_e.dataGridView1.Rows.Add(dbv.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
            else { }
        }






        public void Dual_SH_Delta_E3_Measure(int gray_end_Point, int delay_time_between_measurement)
        {
            Dual_SH_Delta_E form_sh_delta_e = (Dual_SH_Delta_E)System.Windows.Forms.Application.OpenForms["Dual_SH_Delta_E"];
            
            Dual_Engineer_Monitoring_Mode form_dual_mode = (Dual_Engineer_Monitoring_Mode)System.Windows.Forms.Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            DP150_Dual_Engineering_Mornitoring_Mode form_dual_DP150_mode = (DP150_Dual_Engineering_Mornitoring_Mode)System.Windows.Forms.Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];

            form_sh_delta_e.dataGridView1.Rows.Clear();
            form_sh_delta_e.dataGridView2.Rows.Clear();

            XYLv[] Measure = new XYLv[5];
            int Max_Index = 0; double Max_Value = 0;
            int Min_Index = 0; double Min_Value = 2000;

            //Gray 48~255 에서 X/Y/Lv 먼저 찍음
            for (int gray = gray_end_Point; gray <= 255 & form_sh_delta_e.Availability; gray++)
            {
                PTN_update(gray, gray, gray);
                Thread.Sleep(delay_time_between_measurement);
                form_sh_delta_e.progressBar_GB.PerformStep();
                try
                {
                    { //Condition 1
                        if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                            form_dual_mode.Conditon_1_Script_Apply();
                        else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                            form_dual_DP150_mode.Conditon_1_Script_Apply();


                        Max_Index = 0; Min_Index = 0;
                        Max_Value = 0; Min_Value = 2000;
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview
                        form_sh_delta_e.dataGridView1.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
                    }

                    { //Condition 2
                        if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                            form_dual_mode.Conditon_2_Script_Apply();
                        else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                            form_dual_DP150_mode.Conditon_2_Script_Apply();

                        Max_Index = 0; Min_Index = 0;
                        Max_Value = 0; Min_Value = 2000;
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                                
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView2.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview
                        form_sh_delta_e.dataGridView2.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView2.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView2.RowCount - 1;
                    }
                }
                catch (Exception er)
                {
                    DisplayError(er);
                    System.Windows.Forms.Application.Exit();
                }
            }
        }

        public XYLv Measure()
        {
            XYLv Meas = new XYLv();

            objCa.Measure();
            Meas.X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
            Meas.Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
            Meas.Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
            Meas.Double_Update_From_String();

            return Meas;
        }





        public void Dual_SH_Difference_Measure_By_Step(int delay_time_between_measurement, int step)
        {
            Dual_SH_Delta_E form_sh_delta_e = (Dual_SH_Delta_E)System.Windows.Forms.Application.OpenForms["Dual_SH_Delta_E"];

            Dual_Engineer_Monitoring_Mode form_dual_mode = (Dual_Engineer_Monitoring_Mode)System.Windows.Forms.Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            DP150_Dual_Engineering_Mornitoring_Mode form_dual_DP150_mode = (DP150_Dual_Engineering_Mornitoring_Mode)System.Windows.Forms.Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];

            form_sh_delta_e.dataGridView1.Rows.Clear();
            form_sh_delta_e.dataGridView2.Rows.Clear();

            XYLv[] Measure = new XYLv[5];
            int Max_Index = 0; double Max_Value = 0;
            int Min_Index = 0; double Min_Value = 2000;

            form_sh_delta_e.Availability = true;

            //gray255    
            bool First_Step = true;
            for(int gray = 255;gray > 0 ;)  
            {
                if (form_sh_delta_e.Availability == false) break;
                PTN_update(gray, gray, gray);
             
                Thread.Sleep(delay_time_between_measurement);
                //form_sh_delta_e.progressBar_GB.PerformStep();
                try
                {
                    { //Condition 1
                        if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                            form_dual_mode.Conditon_1_Script_Apply();
                        else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                            form_dual_DP150_mode.Conditon_1_Script_Apply();

                        Max_Index = 0; Min_Index = 0;
                        Max_Value = 0; Min_Value = 2000;
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            return;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview
                        form_sh_delta_e.dataGridView1.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
                    }

                    { //Condition 2
                        if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                            form_dual_mode.Conditon_2_Script_Apply();
                        else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                            form_dual_DP150_mode.Conditon_2_Script_Apply();

                        Max_Index = 0; Min_Index = 0;
                        Max_Value = 0; Min_Value = 2000;
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            return;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView2.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview
                        form_sh_delta_e.dataGridView2.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView2.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView2.RowCount - 1;
                    }


                }
                catch (Exception er)
                {
                    DisplayError(er);
                    System.Windows.Forms.Application.Exit();
                }

                if (First_Step)
                {
                    gray -= (step - 1);
                    First_Step = false;
                }
                else
                {
                    gray -= step;
                }
            }
        }



        public void Dual_SH_Difference_Measure(int delay_time_between_measurement)
        {
            Dual_SH_Delta_E form_sh_delta_e = (Dual_SH_Delta_E)System.Windows.Forms.Application.OpenForms["Dual_SH_Delta_E"];
            form_sh_delta_e.dataGridView1.Rows.Clear();
            form_sh_delta_e.dataGridView2.Rows.Clear();
            int gray;

            //gray255    
            gray = 255;
            Specific_Gray_Condition1_and_2_Measure(gray, delay_time_between_measurement);

            //gray240    
            gray = 240;
            Specific_Gray_Condition1_and_2_Measure(gray, delay_time_between_measurement);

            //gray127
            gray = 127;
            Specific_Gray_Condition1_and_2_Measure(gray, delay_time_between_measurement);

            //gray48    
            gray = 48;
            Specific_Gray_Condition1_and_2_Measure(gray, delay_time_between_measurement);

            //gray32    
            gray = 32;
            Specific_Gray_Condition1_and_2_Measure(gray, delay_time_between_measurement);

            //gray24   
            gray = 24;
            Specific_Gray_Condition1_and_2_Measure(gray, delay_time_between_measurement);

            //gray16   
            gray = 16;
            Specific_Gray_Condition1_and_2_Measure(gray, delay_time_between_measurement);   
        }

        private void Specific_Gray_Condition1_and_2_Measure(int gray, int delay_time_between_measurement)
        {
            Dual_SH_Delta_E form_sh_delta_e = (Dual_SH_Delta_E)System.Windows.Forms.Application.OpenForms["Dual_SH_Delta_E"];
            Dual_Engineer_Monitoring_Mode form_dual_mode = (Dual_Engineer_Monitoring_Mode)System.Windows.Forms.Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            DP150_Dual_Engineering_Mornitoring_Mode form_dual_DP150_mode = (DP150_Dual_Engineering_Mornitoring_Mode)System.Windows.Forms.Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
            XYLv[] Measure = new XYLv[5];

            PTN_update(gray, gray, gray);
            Thread.Sleep(delay_time_between_measurement);
            form_sh_delta_e.progressBar_GB.PerformStep();

            int Max_Index = 0; double Max_Value = 0;
            int Min_Index = 0; double Min_Value = 2000;

            try
            {
                { //Condition 1
                    if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                        form_dual_mode.Conditon_1_Script_Apply();
                    else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                        form_dual_DP150_mode.Conditon_1_Script_Apply();

                    Max_Index = 0; Min_Index = 0;
                    Max_Value = 0; Min_Value = 2000;
                    isMsr = true;

                    CA_Measure_button.Enabled = false;
                    objCa.Measure();

                    if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                    {
                        GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                        for (int a = 0; a < 5; a++)
                        {
                            objCa.Measure();
                            //Measure[a].X = objProbe.sx.ToString("0.0000");
                            //Measure[a].Y = objProbe.sy.ToString("0.0000");
                            //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                            Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

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
                        GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                        GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                        GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                        GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                        GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                        X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                        Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                        Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                    }

                    else
                    {
                        //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                        //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                        //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                    }

                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                        return;

                    CA_Measure_button.Enabled = true;

                    //Data Grid setting//////////////////////
                    form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                    //Add data to Datagridview
                    form_sh_delta_e.dataGridView1.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                    form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
                }

                { //Condition 2
                    if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
                        form_dual_mode.Conditon_2_Script_Apply();
                    else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                        form_dual_DP150_mode.Conditon_2_Script_Apply();

                    Max_Index = 0; Min_Index = 0;
                    Max_Value = 0; Min_Value = 2000;
                    isMsr = true;

                    CA_Measure_button.Enabled = false;
                    objCa.Measure();

                    if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                    {
                        GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                        for (int a = 0; a < 5; a++)
                        {
                            objCa.Measure();
                            //Measure[a].X = objProbe.sx.ToString("0.0000");
                            //Measure[a].Y = objProbe.sy.ToString("0.0000");
                            //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                            Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

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
                        GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                        GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                        GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                        GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                        GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                        X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                        Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                        Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                    }

                    else
                    {
                        //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                        //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                        //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                    }

                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                        return;

                    CA_Measure_button.Enabled = true;

                    //Data Grid setting//////////////////////
                    form_sh_delta_e.dataGridView2.DataSource = null; // reset (unbind the datasource)

                    //Add data to Datagridview
                    form_sh_delta_e.dataGridView2.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                    form_sh_delta_e.dataGridView2.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView2.RowCount - 1;
                }


            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }




        public void SH_Delta_E3_Measure(int gray_end_Point, int delay_time_between_measurement)
        {
            SH_Delta_E form_sh_delta_e = (SH_Delta_E)System.Windows.Forms.Application.OpenForms["SH_Delta_E"];

            form_sh_delta_e.dataGridView1.Rows.Clear();

            XYLv[] Measure = new XYLv[5];
            int Max_Index = 0; double Max_Value = 0;
            int Min_Index = 0; double Min_Value = 2000;
            

            //Gray 48~255 에서 X/Y/Lv 먼저 찍음
            if (form_sh_delta_e.radioButton_Min_to_Max.Checked)
            {
                for (int gray = gray_end_Point; gray <= 255 & form_sh_delta_e.Availability; gray++)
                {
                    Max_Index = 0; Min_Index = 0;
                    Max_Value = 0; Min_Value = 2000;

                    PTN_update(gray, gray, gray);
                    Thread.Sleep(delay_time_between_measurement);
                    form_sh_delta_e.progressBar_GB.PerformStep();
                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview

                        form_sh_delta_e.dataGridView1.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
            else if (form_sh_delta_e.radioButton_Max_to_Min.Checked)
            {
                for (int gray = 255; gray >= gray_end_Point & form_sh_delta_e.Availability; gray--)
                {
                    Max_Index = 0; Min_Index = 0;
                    Max_Value = 0; Min_Value = 2000;

                    PTN_update(gray, gray, gray);
                    Thread.Sleep(delay_time_between_measurement);
                    form_sh_delta_e.progressBar_GB.PerformStep();
                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        objCa.Measure();

                        if (form_sh_delta_e.checkBox_Ave_Measure.Checked && objCa.OutputProbes.get_ItemOfNumber(1).Lv < Convert.ToDouble(form_sh_delta_e.textBox_Ave_Lv_Limit.Text))
                        {
                            GB_Status_AppendText_Nextline(objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString() + "<" + form_sh_delta_e.textBox_Ave_Lv_Limit.Text, Color.Blue);

                            for (int a = 0; a < 5; a++)
                            {
                                objCa.Measure();
                                //Measure[a].X = objProbe.sx.ToString("0.0000");
                                //Measure[a].Y = objProbe.sy.ToString("0.0000");
                                //Measure[a].Lv = objProbe.Lv.ToString("0.0000");
                                Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                                Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                                Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
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
                            GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Min Lv = " + Measure[Min_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("Ave Lv = " + Ave_Measure.Lv, Color.Black);
                            GB_Status_AppendText_Nextline("Max Lv = " + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                            GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Black);

                            X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                            Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                            Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                        }

                        else
                        {
                            //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                            //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                            //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                            X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                            Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                            Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        }

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;

                        CA_Measure_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        form_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)

                        //Add data to Datagridview

                        form_sh_delta_e.dataGridView1.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        form_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = form_sh_delta_e.dataGridView1.RowCount - 1;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
            else { }
        }

        public void CA_Measure_BT_Click()
        {
            CA_Measure_button.PerformClick();
        }

        private void CA_Measure_60hz_90hz(int gray,string Hz = "60HZ",bool Hz_60 = true)
        {
            Dual_SH_Delta_E obj_dual_sh_delta_e = Dual_SH_Delta_E.getInstance();

            CA_RM_On_button.PerformClick();// Measure 시에 Remote off 일수도 있으니 on 으로 구동 
            
            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView2.Columns[0].HeaderText = "Hz";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;
                CA_zero_cal_button.Enabled = false;

                objCa.Measure();

                label6.Text = "x";
                label7.Text = "y";
                label8.Text = "Lv";
                X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

                System.Windows.Forms.Application.DoEvents();

                CA_Measure_button.Enabled = true;
                CA_zero_cal_button.Enabled = true;

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)
                dataGridView2.Rows.Add(Hz, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;

                //Data Grid setting//////////////////////
                if (Hz_60)
                {
                    obj_dual_sh_delta_e.dataGridView1.DataSource = null; // reset (unbind the datasource)
                    obj_dual_sh_delta_e.dataGridView1.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                    obj_dual_sh_delta_e.dataGridView1.FirstDisplayedScrollingRowIndex = obj_dual_sh_delta_e.dataGridView1.RowCount - 1;
                }
                else
                {
                    obj_dual_sh_delta_e.dataGridView2.DataSource = null; // reset (unbind the datasource)
                    obj_dual_sh_delta_e.dataGridView2.Rows.Add(gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                    obj_dual_sh_delta_e.dataGridView2.FirstDisplayedScrollingRowIndex = obj_dual_sh_delta_e.dataGridView1.RowCount - 1;
                }
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }






        private void CA_Measure_button_Click(object sender, EventArgs e)
        {
            CA_connection_button_Click(sender, e);// Measure 시에 Remote off 일수도 있으니 on 으로 구동 
            

            //objCa.DisplayMode = 0; // 측정 모드는 xyLv
            //objCa.SyncMode = 0; //측정 모드는 NTSC

            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView2.Columns[0].HeaderText = "-";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

           
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;
                CA_zero_cal_button.Enabled = false;

                objCa.Measure();

                   
                label6.Text = "x";
                label7.Text = "y";
                label8.Text = "Lv";                        
                //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");                    

                System.Windows.Forms.Application.DoEvents();


                
                CA_Measure_button.Enabled = true;
                CA_zero_cal_button.Enabled = true;
                     
                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)
  
                //Add data to Datagridview
                dataGridView2.Rows.Add("-",X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        public void DisplayError(Exception er)
        {
            String msg;
            msg = "Error from" + er.Source + "\r\n";
            msg += er.Message + "\r\n";
            System.Windows.Forms.MessageBox.Show(msg);
        }



        private void Grid_Clear_button_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null; // reset (unbind the datasource)
            dataGridView2.Rows.Clear();
        }



        private void Center_ptn_Click(object sender, EventArgs e)
        {
            IPC_Quick_Send("image.crosstalk 455 455 0 0 0 255 255 255");
        }

        
        private void CA_disconnection_btn_Click(object sender, EventArgs e)
        {
            if (Is_Single_CA_is_connected == true)
            {
                objCa.RemoteMode = 0;
                Is_Single_CA_is_connected = false;
                //CA remote : Off
                label_CA_remote_status.Text = "CA Remote : Off";
                label_CA_remote_status.ForeColor = System.Drawing.Color.Red;
                groupBox8.Hide(); //CA 측정 관련 GroupBox
                
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("The CA310 is already in the 'off' status");
            }
            CA_Remote_Status_Check();
        }

        public void CA_connection_button_Perform_Click()
        {
            CA_RM_On_button.PerformClick();
        }

        private void CA_connection_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Is_Single_CA_is_connected == false)
                {
                    objCa.RemoteMode = 1;
                    Is_Single_CA_is_connected = true;
                    //CA remote : On
                    label_CA_remote_status.Text = "CA Remote : On";
                    label_CA_remote_status.ForeColor = System.Drawing.Color.Green;
                    groupBox8.Show(); //CA 측정 관련 GroupBox    

                    objCa = objCa200.SingleCa;
                    objProbe = objCa.SingleProbe;
                    //objCa.ExeCalZero += new CA200SRVRLib._ICaEvents_ExeCalZeroEventHandler(objCa_ExeCalZero);
                    objMemory = objCa.Memory; //171213 추가사항
                    objProbeInfo = (CA200SRVRLib.IProbeInfo)objProbe; //171213 추가사항
                    CA_Channel_Update();
                }
            }
            catch (Exception er)
            {
                System.Windows.Forms.MessageBox.Show("The CA310 is not connected , plz restart this program to connect the CA310");
            }
            CA_Remote_Status_Check();
        }


        private void Mipi_Read(string addr)
        {
            string strcmd = "mipi.read 0x06 " + addr;
            IPC_Quick_Send(strcmd);
        }
        
        
        private void VPP_On()
        {
            IPC_Quick_Send("power.vpp.level 5.2");
            IPC_Quick_Send("power.on vpp");
        }

        private void VPP_Off()
        {
            IPC_Quick_Send("power.off vpp");
        }

        private void HS_off_LP_on()
        {
            IPC_Quick_Send("mipi.video.disable");
        }

        private void HS_on_LP_off()
        {
            IPC_Quick_Send("mipi.video.enable");	
        }


        public void Intialize_XY(ref double X, ref double Y)
        {
            X = current_model.get_X();
            Y = current_model.get_Y();
        }

        public void DP150_One_Param_CMD_Send(int Send_Start_Index, string address, string param)
        {
            IPC_Quick_Send("mipi.write 0x15 0xB0 0x" + Send_Start_Index.ToString("X2"));//First CMD
            IPC_Quick_Send("mipi.write 0x39 0x" + address + " 0x" + param);//Second CMD
        }

        public void DP173_One_Param_CMD_Send_and_Show(int Send_Start_Index, string address, string param)
        {
            IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0x" + Send_Start_Index.ToString("X2"), Color.Blue);//First CMD
            IPC_Quick_Send_And_Show("mipi.write 0x39 0x" + address + " 0x" + param, Color.Blue);//Second CMD  
        }

        public void DP173_One_Param_CMD_Send(int Send_Start_Index, string address, string param)
        {
            IPC_Quick_Send("mipi.write 0x15 0xB0 0x" + Send_Start_Index.ToString("X2"));//First CMD
            IPC_Quick_Send("mipi.write 0x39 0x" + address + " 0x" + param);//Second CMD   
        }

        public void DP173_Long_Packet_CMD_Send(byte[][] Output_CMD)
        {
            int Send_Start_Index = Convert.ToInt16(Output_CMD[0][0]);
            
            string address = Output_CMD[1][0].ToString("X2");

            string[] parameters = new string[Output_CMD[2].Length];
            for (int i = 0; i < Output_CMD[2].Length; i++)
                parameters[i] = Output_CMD[2][i].ToString("X2");

            DP173_Long_Packet_CMD_Send(Send_Start_Index, Output_CMD[2].Length, address, parameters);
        }

        public void DP173_Long_Packet_CMD_Send_and_Show(byte[][] Output_CMD)
        {
            int Send_Start_Index = Convert.ToInt16(Output_CMD[0][0]);

            string address = Output_CMD[1][0].ToString("X2");

            int param_amount = Output_CMD[1].Length - 1;

            string[] parameters = new string[param_amount];
            for (int i = 1; i < Output_CMD[1].Length; i++)
                parameters[i - 1] = Output_CMD[1][i].ToString("X2");

            DP173_Long_Packet_CMD_Send_and_Show(Send_Start_Index, param_amount, address, parameters);
        }

        public void DP173_Long_Packet_CMD_Send(int Send_Start_Index, int param_amount, string address, string[] parameters)
        {
            if (Send_Start_Index != 0)
            {
                if (radioButton_Debug_Status_Mode.Checked)
                    IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0x" + Send_Start_Index.ToString("X2"), Color.Red);//First CMD
                else
                    IPC_Quick_Send("mipi.write 0x15 0xB0 0x" + Send_Start_Index.ToString("X2"));//First CMD
            }
            
            string mipi_cmd = "mipi.write 0x39 0x" + address;
            for (int i = 0; i < param_amount; i++) mipi_cmd += " 0x" + parameters[i];

            if(radioButton_Debug_Status_Mode.Checked)
                IPC_Quick_Send_And_Show(mipi_cmd, Color.Blue);//Second CMD
            else 
                IPC_Quick_Send(mipi_cmd);//Second CMD
        }

        public void DP173_Long_Packet_CMD_Send_and_Show(int Send_Start_Index, int param_amount, string address, string[] parameters)
        {
            if (Send_Start_Index != 0)
                IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0x" + Send_Start_Index.ToString("X2"),Color.Blue);//First CMD
            
            string mipi_cmd = "mipi.write 0x39 0x" + address;
            for (int i = 0; i < param_amount; i++) mipi_cmd += " 0x" + parameters[i];

            IPC_Quick_Send_And_Show(mipi_cmd,Color.Blue);//Second CMD
        }


        public void DP150_Long_Packet_CMD_Send(int Send_Start_Index, int param_amount, string address, string[] parameters)
        {
            string mipi_cmd = "mipi.write 0x39 0x" + address;
            for (int i = 0; i < param_amount; i++) mipi_cmd += " 0x" + parameters[i];

            IPC_Quick_Send("mipi.write 0x15 0xB0 0x" + Send_Start_Index.ToString("X2"));//First CMD
            IPC_Quick_Send(mipi_cmd);//Second CMD
        }

        public string Long_Packet_CMD_Send(int param_amount,string address ,string[] parameters)
        {
            string mipi_cmd = "mipi.write 0x39 0x" + address;
            for (int i = 0; i < param_amount; i++)
            {
                mipi_cmd += " 0x" + parameters[i];
            }
            IPC_Quick_Send(mipi_cmd);
            return mipi_cmd;
            //GB_Status_AppendText_Nextline(mipi_cmd,Color.Blue);
        }

        // PasteInData pastes clipboard data into the grid passed to it.
        public void PasteInData(ref DataGridView dgv)
        {
            char[] rowSplitter = { '\n', '\r' };  // Cr and Lf.
            char columnSplitter = '\t';         // Tab.

            IDataObject dataInClipboard = Clipboard.GetDataObject();

            string stringInClipboard =
                dataInClipboard.GetData(DataFormats.Text).ToString();

            string[] rowsInClipboard = stringInClipboard.Split(rowSplitter,
                StringSplitOptions.RemoveEmptyEntries);

            int r = dgv.SelectedCells[0].RowIndex;
            int c = dgv.SelectedCells[0].ColumnIndex;

            if (dgv.Rows.Count < (r + rowsInClipboard.Length))
                dgv.Rows.Add(r + rowsInClipboard.Length - dgv.Rows.Count);

            // Loop through lines:

            int iRow = 0;
            while (iRow < rowsInClipboard.Length)
            {
                // Split up rows to get individual cells:

                string[] valuesInRow =
                    rowsInClipboard[iRow].Split(columnSplitter);

                // Cycle through cells.
                // Assign cell value only if within columns of grid:

                int jCol = 0;
                while (jCol < valuesInRow.Length)
                {
                    if ((dgv.ColumnCount - 1) >= (c + jCol))
                        dgv.Rows[r + iRow].Cells[c + jCol].Value =
                        valuesInRow[jCol];

                    jCol += 1;
                } // end while

                iRow += 1;
            } // end while
        }


        public void AOD_Pattern_Setting(int Gray)
        {
            IPC_Quick_Send("image.crosstalk " + current_model.get_AOD_X().ToString() + " " + current_model.get_AOD_Y().ToString() + " 0 0 0 " + Gray.ToString() + " " + Gray.ToString() + " " + Gray.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Is_1ch_CA_Mode = true;
            CA_Remote_Status_Check();
            Initialize_Before_CA_Connection();
            radioButton_PNC_ACK_Fast_CheckedChanged(sender, e);

            dP086ToolStripMenuItem_Click(sender, e);//Make the DP116 be selected as default
            
            Get_All_Serial_Port();//190530 Add

            if (CA_First_Connection_Status) Verify_Channel_WRGB_Update(sender, e);

            Previouse_Pos = new Point(0, 0);
            sb = new StringBuilder();
        }

        private void Verify_Channel_WRGB_Update(object sender, EventArgs e)
        {
            textBox_verify_W.Text = textBox_ch.Text;
            CA_Channel_Update();
            // R/G/B Channel
            if (objMemory.ChannelNO <= 96)
            {
                textBox_verify_R.Text = (objMemory.ChannelNO + 1).ToString();
                textBox_verify_G.Text = (objMemory.ChannelNO + 2).ToString();
                textBox_verify_B.Text = (objMemory.ChannelNO + 3).ToString();
            }
            else
            {
                textBox_verify_R.Text = objMemory.ChannelNO.ToString();
                textBox_verify_G.Text = objMemory.ChannelNO.ToString();
                textBox_verify_B.Text = objMemory.ChannelNO.ToString();
            }
        }


        private void CA_Remote_Status_Check()
        {
            if (label_CA_remote_status.Text == "CA Remote : On")
            {
                groupBox8.Show(); //CA 측정 관련 GroupBox
                groupBox18.Show();
                groupBox19.Show();
                groupBox21.Show();

            }
            else
            {
                groupBox8.Hide(); //CA 측정 관련 GroupBox
                groupBox18.Hide();
                groupBox19.Hide();
                groupBox21.Hide();
            }
        }

        private void Gradation_ptn_Click(object sender, EventArgs e)
        {
            Display_Gradation_Pattern();
        }

        public void Display_Gradation_Pattern()
        {
            IPC_Quick_Send("image.gradation.gray hh dec");
        }


        private void CA_Mode_Initialize()
        {
            if (label_CA_remote_status.Text == "CA Remote : On")
            {
                if (objCa.SyncMode == 0) radioButton_NTSC.Checked = true;
                else if (objCa.SyncMode == 1) radioButton_PAL.Checked = true;
                else if (objCa.SyncMode == 2) radioButton_EXT.Checked = true;
                else if (objCa.SyncMode == 3) radioButton_UNIV.Checked = true;

                if (objCa.AveragingMode == 2) radioButton_CA_Measure_Auto.Checked = true;
                else if (objCa.AveragingMode == 1) radioButton_CA_Measure_Fast.Checked = true;
                else if (objCa.AveragingMode == 0) radioButton_CA_Measure_Slow.Checked = true;
            }
        }


        private void Initialize_After_CA_Connection()
        {
            //한번 연결되면 , 연결 다시할 필요 없으므로 삭제.
            Is_1ch_CA_Mode = true;
            CA310_connect.Visible = false;
            CA410_connect.Visible = false;
            button_CA_Multichannel.Visible = false;
            CA_Multi_Channel_Form.getInstance().Visible = false;

            //연결되면 보여져야 하는것
            CA_Channel_trackBar.Visible = true;
            CA_zero_cal_button.Visible = true;
            CA_RM_On_button.Visible = true;
            CA_RM_Off_btn.Visible = true;
        }

        private void Initialize_Before_CA_Connection()
        {
            //연결 안되면 , 다시 연결 Try 필요하므로 visible 설정.
            CA310_connect.Visible = true;
            CA410_connect.Visible = true;
            button_CA_Multichannel.Visible = true;

            //연결 되기 전까지는 보여지면 안되는것
            CA_Channel_trackBar.Visible = false;
            CA_zero_cal_button.Visible = false;
            CA_RM_On_button.Visible = false;
            CA_RM_Off_btn.Visible = false;
        }

        private void CA310_connect_Click(object sender, EventArgs e)
        {
            try
            {
                GB_Status_AppendText_Nextline("CA310 Connect Start", Color.Black);
                objCa200.AutoConnect();
                objCa = objCa200.SingleCa;
                objProbe = objCa.SingleProbe;
                //objCa.ExeCalZero += new CA200SRVRLib._ICaEvents_ExeCalZeroEventHandler(objCa_ExeCalZero);

                objMemory = objCa.Memory; //171213 추가사항
                objProbeInfo = (CA200SRVRLib.IProbeInfo)objProbe; //171213 추가사항

                Is_Single_CA_is_connected = true;
                label_CA310_Status.Text = "CA connection status : OK";
                label_CA310_Status.ForeColor = System.Drawing.Color.Green;

                Initialize_After_CA_Connection();

                //CA remote : On
                label_CA_remote_status.Text = "CA Remote : On";
                label_CA_remote_status.ForeColor = System.Drawing.Color.Green;


                this.textBox_ch.Text = objMemory.ChannelNO.ToString();
                CA_Channel_Update();
                GB_Status_AppendText_Nextline("CA310 Connect Finished", Color.Black);
            }
            catch (Exception er)
            {
                DisplayError(er);
                //System.Windows.Forms.Application.Exit();
                System.Windows.Forms.MessageBox.Show("The CA310 is not connected , plz connect CA310 to the PC");
                Is_Single_CA_is_connected = false;
                label_CA310_Status.Text = "CA connection status : NG";
                label_CA310_Status.ForeColor = System.Drawing.Color.Red;

                Initialize_Before_CA_Connection();

                //CA remote : Off
                label_CA_remote_status.Text = "CA Remote : Off";
                label_CA_remote_status.ForeColor = System.Drawing.Color.Red;
            }
            CA_Remote_Status_Check();
            CA_Mode_Initialize();
        }

      

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

      
        //Menuscript Selection for DP116
        private void dP116ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model_dp116_update();
        }

        public void model_dp116_update()
        {
            current_model.Set_Model_As_DP116();
            Model_Setting();
        }


        public int Get_DBV_TrackBar_Maximum()
        {
            return DBV_trackbar.Maximum;
        }

        //Menuscript Selection for 2nd_Model(TBD)
        private void ndModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model_dp150_update();
        }

        public void model_dp150_update()
        {
            current_model.Set_Model_As_DP150();
            this.Model_Setting();
        }


        private void metaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model_meta_update();
        }

        public void model_meta_update()
        {
            current_model.Set_Model_As_Meta();
            this.Model_Setting();
        }


        private void DBV_Accuracy_Measure_Show_or_Hide()
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
                button_DBV_Accuracy_20191217.Show();
            else
                button_DBV_Accuracy_20191217.Hide();

        }

        void Change_Tema(System.Drawing.Color Color)
        {
            modelSelectionToolStripMenuItem.BackColor = Color;
            label_Model_Name.ForeColor = Color;
            menuStrip1.BackColor = Color;
            textBox2_cmd.BackColor = Color;
            label_Medel_Resolution.ForeColor = Color;
        }

        void OTP_Groupbox_Selection()
        {

            if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116)
            {
                this.groupBox_1st_Model_OTP_Check.Show();
                this.groupBox_2nd_Model_OTP_Check.Hide();   
                this.groupBox_Meta_Model_OTP_Check.Hide();
                groupBox_1st_Model_OTP_Check.Location = groupBox_Meta_Model_OTP_Check.Location;//Add on 190820
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
            {
                this.groupBox_1st_Model_OTP_Check.Hide();
                this.groupBox_2nd_Model_OTP_Check.Show();
                this.groupBox_Meta_Model_OTP_Check.Hide();
                groupBox_2nd_Model_OTP_Check.Location = groupBox_Meta_Model_OTP_Check.Location;//Add on 190820
            }
            else
            {
                this.groupBox_1st_Model_OTP_Check.Hide();
                this.groupBox_2nd_Model_OTP_Check.Hide();
                this.groupBox_Meta_Model_OTP_Check.Show();
            }
        }

        public void Get_Set123456_txt_Path(ref string filepath_1, ref string filepath_2, ref string filepath_3, ref string filepath_4
            , ref string filepath_5, ref string filepath_6)
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP173)
            {
                filepath_1 = Directory.GetCurrentDirectory() + "\\DP173\\Script_Condition_Set1.txt";
                filepath_2 = Directory.GetCurrentDirectory() + "\\DP173\\Script_Condition_Set2.txt";
                filepath_3 = Directory.GetCurrentDirectory() + "\\DP173\\Script_Condition_Set3.txt";
                filepath_4 = Directory.GetCurrentDirectory() + "\\DP173\\Script_Condition_Set4.txt";
                filepath_5 = Directory.GetCurrentDirectory() + "\\DP173\\Script_Condition_Set5.txt";
                filepath_6 = Directory.GetCurrentDirectory() + "\\DP173\\Script_Condition_Set6.txt";
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP213)
            {
                filepath_1 = Directory.GetCurrentDirectory() + "\\DP213\\Script_Condition_Set1.txt";
                filepath_2 = Directory.GetCurrentDirectory() + "\\DP213\\Script_Condition_Set2.txt";
                filepath_3 = Directory.GetCurrentDirectory() + "\\DP213\\Script_Condition_Set3.txt";
                filepath_4 = Directory.GetCurrentDirectory() + "\\DP213\\Script_Condition_Set4.txt";
                filepath_5 = Directory.GetCurrentDirectory() + "\\DP213\\Script_Condition_Set5.txt";
                filepath_6 = Directory.GetCurrentDirectory() + "\\DP213\\Script_Condition_Set6.txt";
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.Elgin)
            {
                filepath_1 = Directory.GetCurrentDirectory() + "\\Elgin\\Script_Condition_Set1.txt";
                filepath_2 = Directory.GetCurrentDirectory() + "\\Elgin\\Script_Condition_Set2.txt";
                filepath_3 = Directory.GetCurrentDirectory() + "\\Elgin\\Script_Condition_Set3.txt";
                filepath_4 = Directory.GetCurrentDirectory() + "\\Elgin\\Script_Condition_Set4.txt";
                filepath_5 = Directory.GetCurrentDirectory() + "\\Elgin\\Script_Condition_Set5.txt";
                filepath_6 = Directory.GetCurrentDirectory() + "\\Elgin\\Script_Condition_Set6.txt";
            }
        }


        public void Get_Dual_Mode_TXT_Path(ref string filepath_1,ref string filepath_2)
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP086)
            {
                filepath_1 = Directory.GetCurrentDirectory() + "\\DP086\\Dual_Mode\\Script_Condition_1.txt";
                filepath_2 = Directory.GetCurrentDirectory() + "\\DP086\\Dual_Mode\\Script_Condition_2.txt";
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP116)
            {
                filepath_1 = Directory.GetCurrentDirectory() + "\\DP116\\Dual_Mode\\Script_Condition_1.txt";
                filepath_2 = Directory.GetCurrentDirectory() + "\\DP116\\Dual_Mode\\Script_Condition_2.txt";
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
            {
                filepath_1 = Directory.GetCurrentDirectory() + "\\DP150\\Dual_Mode\\Script_Condition_1.txt";
                filepath_2 = Directory.GetCurrentDirectory() + "\\DP150\\Dual_Mode\\Script_Condition_2.txt";
            }
            else
            {
               
            }
        }


        public void Get_Dual_Mode_OC_Param_File_Path_DP150(ref string filepath_1, ref string filepath_2, ref string filePath_Gamma_Offset, ref string filePath_Delta_Lv_UV)
        {
            if (Second_Model_Option_Form.getInstance().Get_IS_G2G_On())
            {
                filepath_1 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set1);
                filepath_2 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set2);
            }
            else
            {
                filepath_1 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set1);
                filepath_2 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set2);
            }
            filePath_Gamma_Offset = Directory.GetCurrentDirectory() + "\\DP150\\Dual_Mode\\OC_Gamma_Diff_Init_Form.csv";
            filePath_Delta_Lv_UV = Directory.GetCurrentDirectory() + "\\DP150\\Dual_Mode\\Mode2_Delta_L_Diff_Spec.csv";
        }

        public void Get_Dual_Mode_OC_Param_File_Path(ref string filepath_1, ref string filepath_2, ref string filePath_Gamma_Offset)
        {
            if(current_model.Get_Current_Model_Name() == Model_Name.DP086)
            {
                filepath_1 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set1);
                filepath_2 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set2);
                filePath_Gamma_Offset = Directory.GetCurrentDirectory() + "\\DP086\\Dual_Mode\\OC_Gamma_Offset.csv";
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP116)
            {
                filepath_1 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set1);
                filepath_2 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set2);
                filePath_Gamma_Offset = Directory.GetCurrentDirectory() + "\\DP116\\Dual_Mode\\OC_Gamma_Offset.csv";
            }
        }

        public void Get_Dual_Mode_OC_Param_File_Path(out string filepath_1, out string filepath_2, out string filepath_3, out string filepath_4, out string filepath_5, out string filepath_6, out string filepath_7)
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin)
            {
                if (DP173_Model_Option_Form.getInstance().Get_IS_G2G_On())
                {
                    filepath_1 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set1);
                    filepath_2 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set2);
                    filepath_3 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set3);
                    filepath_4 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set4);
                    filepath_5 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set5);
                    filepath_6 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set6);
                    filepath_7 = current_model.Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address();
                }
                else
                {
                    filepath_1 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set1);
                    filepath_2 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set2);
                    filepath_3 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set3);
                    filepath_4 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set4);
                    filepath_5 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set5);
                    filepath_6 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set6);
                    filepath_7 = current_model.Get_Dual_Set2_Diff_Delta_L_Spec_Address();
                }
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP213)
            {
                if (DP213_Model_Option_Form.getInstance().Get_IS_G2G_On())
                {

                    filepath_1 = current_model.Get_Dual_OC_G2G_Param_Address(OC_Mode.Mode1);
                    filepath_2 = current_model.Get_Dual_OC_G2G_Param_Address(OC_Mode.Mode2);
                    filepath_3 = current_model.Get_Dual_OC_G2G_Param_Address(OC_Mode.Mode3);
                    filepath_4 = current_model.Get_Dual_OC_G2G_Param_Address(OC_Mode.Mode4);
                    filepath_5 = current_model.Get_Dual_OC_G2G_Param_Address(OC_Mode.Mode5);
                    filepath_6 = current_model.Get_Dual_OC_G2G_Param_Address(OC_Mode.Mode6);
                    filepath_7 = current_model.Get_Dual_Set2_Diff_Delta_L_G2G_Spec_Address();
                }
                else
                {
                    filepath_1 = current_model.Get_Dual_OC_Param_Address(OC_Mode.Mode1);
                    filepath_2 = current_model.Get_Dual_OC_Param_Address(OC_Mode.Mode2);
                    filepath_3 = current_model.Get_Dual_OC_Param_Address(OC_Mode.Mode3);
                    filepath_4 = current_model.Get_Dual_OC_Param_Address(OC_Mode.Mode4);
                    filepath_5 = current_model.Get_Dual_OC_Param_Address(OC_Mode.Mode5);
                    filepath_6 = current_model.Get_Dual_OC_Param_Address(OC_Mode.Mode6);
                    filepath_7 = current_model.Get_Dual_Set2_Diff_Delta_L_Spec_Address();
                }
            }
            else
            {
                filepath_1 = string.Empty;
                filepath_2 = string.Empty;
                filepath_3 = string.Empty;
                filepath_4 = string.Empty;
                filepath_5 = string.Empty;
                filepath_6 = string.Empty;
                filepath_7 = string.Empty;
            }
        }

        public void Get_Dual_Set123456_Mode_OC_Param_File_Path(ref string filepath_1, ref string filepath_2,
            ref string filepath_3, ref string filepath_4, ref string filepath_5, ref string filepath_6)
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin)
            {
                if (DP173_Model_Option_Form.getInstance().Get_IS_G2G_On())
                {
                    filepath_1 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set1);
                    filepath_2 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set2);
                    filepath_3 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set3);
                    filepath_4 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set4);
                    filepath_5 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set5);
                    filepath_6 = current_model.Get_Dual_OC_G2G_Param_Address(Gamma_Set.Set6);
                }
                else
                {
                    filepath_1 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set1);
                    filepath_2 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set2);
                    filepath_3 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set3);
                    filepath_4 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set4);
                    filepath_5 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set5);
                    filepath_6 = current_model.Get_Dual_OC_Param_Address(Gamma_Set.Set6);
                }
            }
        }

        public string Get_OC_Param_File_Path()
        {
            string filePath = string.Empty;

            if ((current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin) && DP173_Model_Option_Form.getInstance().Get_IS_G2G_On())
                filePath = current_model.Get_Single_OC_G2G_Param_Address();
            else if (current_model.Get_Current_Model_Name() == Model_Name.Meta && Meta_Model_Option_Form.getInstance().Get_IS_G2G_On())
                filePath = current_model.Get_Single_OC_G2G_Param_Address();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP150 && Second_Model_Option_Form.getInstance().Get_IS_G2G_On())
                filePath = current_model.Get_Single_OC_G2G_Param_Address();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP213 && DP213_Model_Option_Form.getInstance().Get_IS_G2G_On())
                filePath = current_model.Get_Single_OC_G2G_Param_Address();
            else
                filePath = current_model.Get_Single_OC_Param_Address();
                            
            return filePath;
        }




        public void Option_Setting_Tool_Click()
        {
            optionSettingToolStripMenuItem.PerformClick();
        }

        public Form Get_Current_Model_Option_Form()
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116) return First_Model_Option_Form.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP150) return Second_Model_Option_Form.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.Meta) return Meta_Model_Option_Form.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin) return DP173_Model_Option_Form.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP213) return DP213_Model_Option_Form.getInstance();
            else
            {
                System.Windows.Forms.MessageBox.Show("Error , the Model-Selection should have been made");
                return null;
            }
        }

        public Form Get_Current_Single_Engineering_Form()
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116) return Engineer_Mornitoring_Mode.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP150) return DP150_Single_Engineerig_Mornitoring_Mode.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.Meta) return Meta_Engineer_Mornitoring_Mode.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin) return DP173_Single_Engineering_Mornitoring.getInstance();
            else
            {
                System.Windows.Forms.MessageBox.Show("Error , the Model-Selection should have been made");
                return null;
            }
        }

        public Form Get_Current_Dual_Engineering_Form()
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116) return Dual_Engineer_Monitoring_Mode.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP150) return DP150_Dual_Engineering_Mornitoring_Mode.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.Meta)
            {
                System.Windows.Forms.MessageBox.Show("Meta Model doesn't have Dual Mode");
                return null;
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin)return DP173_Dual_Engineering_Mornitoring.getInstance();
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP213) return DP213_Dual_Engineering_Mornitoring.getInstance();
            else
            {
                System.Windows.Forms.MessageBox.Show("Error , the Model-Selection should have been made");
                return null;
            }
        }

        private void optionSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (First_Model_Option_Form.IsIstanceNull() == false) First_Model_Option_Form.getInstance().Visible = false;
            if (Second_Model_Option_Form.IsIstanceNull() == false) Second_Model_Option_Form.getInstance().Visible = false;
            if (Meta_Model_Option_Form.IsIstanceNull() == false) Meta_Model_Option_Form.getInstance().Visible = false;
            if (DP173_Model_Option_Form.IsIstanceNull() == false) DP173_Model_Option_Form.getInstance().Visible = false;
            if (DP213_Model_Option_Form.IsIstanceNull() == false) DP213_Model_Option_Form.getInstance().Visible = false;

            Form current_model_form = Get_Current_Model_Option_Form();
            current_model_form.BackColor = current_model.Get_Back_Ground_Color();
            current_model_form.Text = current_model.Get_Current_Model_Name().ToString();
            current_model_form.Show();

            if (current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin) DP173_Model_Option_Form.getInstance().Load_Setting_Perform_Click();
        }

        public string Get_Textbox_cmd2()
        {
            return textBox2_cmd.Text;
        }


        public void OTP_1_register_reader_Click(object sender, EventArgs e)
        {
            OTP_1_register_read();
        }

        //this can be used to save time
        string Quick_Read_Register(int dex_params_num,string Register)
        {
            //string Mipi_Read_Data = "";
            string Mipi_Read_Data = string.Empty;

            try
            {
                String[] sTxData = new string[] {
		        ("mipi.write 0x37 0x"+ dex_params_num.ToString("X2")),
		        ("mipi.read 0x06 0x"+ Register)
	            };

                foreach (string s in sTxData)
                {
                    //System.Windows.Forms.MessageBox.Show(s);
                    Mipi_Read_Data = IPC_Send_Readquicker(s);
                }                
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("OTP Quick Read fail !");
            }
            //System.Windows.Forms.MessageBox.Show(Mipi_Read_Data);
            return Mipi_Read_Data;
        }

        void OTP_1_register_read()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.ForeColor = System.Drawing.Color.Black;
            try
            {
                //textBox2_cmd.Text = "";
                textBox2_cmd.Text = string.Empty;

                int dex_params_num = Convert.ToInt32(Textbox_How_many.Text);
                if (dex_params_num > 512)
                {
                    dex_params_num = 512;
                    System.Windows.Forms.MessageBox.Show("Cannot read more than 512ea");
                }

                string Hex_Params_num = dex_params_num.ToString("X3");

                string[] sTxData = new string[2];
                if (dex_params_num > 255)
                {
                    sTxData[0] = "mipi.write 0x37 " + "0x" + Hex_Params_num[1] + Hex_Params_num[2] + " 0x0" + Hex_Params_num[0];
                    sTxData[1] = "mipi.read 0x06 0x"+Textbox_OTP_1_register.Text;
                }
                else
                {
                    sTxData[0] = "mipi.write 0x37 0x" + Hex_Params_num[1] + Hex_Params_num[2];
                    sTxData[1] = "mipi.read 0x06 0x"+Textbox_OTP_1_register.Text;
                }


                foreach (string s in sTxData)
                {
                    IPC_Quick_Send(s);
                }
                

                dataGridView1.DataSource = null; // reset (unbind the datasource)
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("Address", "Address");
                dataGridView1.Columns.Add("Param", "Param");
    
                char[] mipi_data = textBox2_cmd.Text.ToCharArray();
                string temp_data = "00";

                int count = 0;

                int i = 0;
                if (dex_params_num < 10) i = 17;
                else if (dex_params_num < 100) i = 18;
                else i = 19;
                   
                for (; i < mipi_data.Length; i = i + 5)
                {
                    temp_data = new string(mipi_data, i + 2, 2);
                    dataGridView1.Rows.Add("(" + Textbox_OTP_1_register.Text + ") " + (++count).ToString(), temp_data);

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

        void First_Model_OTP_Zone_RB_Visibie(bool Status,string Zone = "ALL")
        {
            if (Zone == "ZONE A")
            {
                Zone_A_RB_OTP_0.Visible = Status;
                Zone_A_RB_OTP_1.Visible = Status;
                Zone_A_RB_OTP_2.Visible = Status;
                Zone_A_RB_OTP_3.Visible = Status;
            }

            else if (Zone == "ZONE B")
            {
                Zone_B_RB_OTP_0.Visible = Status;
                Zone_B_RB_OTP_1.Visible = Status;
                Zone_B_RB_OTP_2.Visible = Status;
                Zone_B_RB_OTP_3.Visible = Status;
                Zone_B_RB_OTP_4.Visible = Status;
                Zone_B_RB_OTP_5.Visible = Status;
            }

            else if (Zone == "ZONE C")
            {
                Zone_C_RB_OTP_0.Visible = Status;
                Zone_C_RB_OTP_1.Visible = Status;
                Zone_C_RB_OTP_2.Visible = Status;
                Zone_C_RB_OTP_3.Visible = Status;
                Zone_C_RB_OTP_4.Visible = Status;
                Zone_C_RB_OTP_5.Visible = Status;
            }

            else if (Zone == "ALL")
            {
                Zone_A_RB_OTP_0.Visible = Status;
                Zone_A_RB_OTP_1.Visible = Status;
                Zone_A_RB_OTP_2.Visible = Status;
                Zone_A_RB_OTP_3.Visible = Status;

                Zone_B_RB_OTP_0.Visible = Status;
                Zone_B_RB_OTP_1.Visible = Status;
                Zone_B_RB_OTP_2.Visible = Status;
                Zone_B_RB_OTP_3.Visible = Status;
                Zone_B_RB_OTP_4.Visible = Status;
                Zone_B_RB_OTP_5.Visible = Status;

                Zone_C_RB_OTP_0.Visible = Status;
                Zone_C_RB_OTP_1.Visible = Status;
                Zone_C_RB_OTP_2.Visible = Status;
                Zone_C_RB_OTP_3.Visible = Status;
                Zone_C_RB_OTP_4.Visible = Status;
                Zone_C_RB_OTP_5.Visible = Status;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("PlZ Select Proper Zone");
            }
        }

        void First_Model_OTP_Zone_RB_Checked_False()
        {
            Zone_A_RB_OTP_0.Checked = false;
            Zone_A_RB_OTP_1.Checked = false;
            Zone_A_RB_OTP_2.Checked = false;
            Zone_A_RB_OTP_3.Checked = false;

            Zone_B_RB_OTP_0.Checked = false;
            Zone_B_RB_OTP_1.Checked = false;
            Zone_B_RB_OTP_2.Checked = false;
            Zone_B_RB_OTP_3.Checked = false;
            Zone_B_RB_OTP_4.Checked = false;
            Zone_B_RB_OTP_5.Checked = false;

            Zone_C_RB_OTP_0.Checked = false;
            Zone_C_RB_OTP_1.Checked = false;
            Zone_C_RB_OTP_2.Checked = false;
            Zone_C_RB_OTP_3.Checked = false;
            Zone_C_RB_OTP_4.Checked = false;
            Zone_C_RB_OTP_5.Checked = false;
        }


        private void OTP_Count_Verify_After_Write(ref bool OTP_Write_Result_Ok)
        {            
            GB_Status_AppendText_Nextline("== OTP Count Verify After OTP Write ==", System.Drawing.Color.Black);
            First_Model_OTP_Zone_RB_Checked_False();
            System.Windows.Forms.Application.DoEvents();

            First_Model_OTP_Zone_RB_Visibie(false);
            textBox2_cmd.Clear();

            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x37 0x01");
            IPC_Quick_Send("mipi.read 0x06 0x43");
            Thread.Sleep(100);
            char[] mipi_data = textBox2_cmd.Text.ToCharArray();
            string temp_data = new string(mipi_data, 19, 2);
            byte byte_temp_data = Convert.ToByte(temp_data, 16);
            //System.Windows.Forms.MessageBox.Show(byte_temp_data.ToString());

            int Zone_A = (byte_temp_data & 0x03);
            int Zone_B = (byte_temp_data & 0x1C) >> 2;
            int Zone_C = (byte_temp_data & 0xE0) >> 5;

            //GB_Status_AppendText_Nextline("ZONE-A = " + Zone_A.ToString(), System.Drawing.Color.DarkGray);
            //GB_Status_AppendText_Nextline("ZONE-B = " + Zone_B.ToString(), System.Drawing.Color.DarkGray);
            //GB_Status_AppendText_Nextline("ZONE-C = " + Zone_C.ToString(), System.Drawing.Color.DarkGray);

            int before_OTP_count_Zone_A = (~Write_Before_OTP_Zone_A) & 0x03;
            int before_OTP_count_Zone_B = (~Write_Before_OTP_Zone_B) & 0x07;
            int before_OTP_count_Zone_C = (~Write_Before_OTP_Zone_C) & 0x07;
            int After_OTP_count_Zone_A = (~Zone_A) & 0x03;
            int After_OTP_count_Zone_B = (~Zone_B) & 0x07;
            int After_OTP_count_Zone_C = (~Zone_C) & 0x07;

            GB_Status_AppendText_Nextline("(Before OTP Write) ZONE-A = : " + before_OTP_count_Zone_A.ToString(), System.Drawing.Color.DarkGray);
            GB_Status_AppendText_Nextline("(After OTP Write) ZONE-A = : " + After_OTP_count_Zone_A.ToString(), System.Drawing.Color.DarkGray);
            GB_Status_AppendText_Nextline("(Before OTP Write) ZONE-B = : " + before_OTP_count_Zone_B.ToString(), System.Drawing.Color.DarkGray);
            GB_Status_AppendText_Nextline("(After OTP Write) ZONE-B = : " + After_OTP_count_Zone_B.ToString(), System.Drawing.Color.DarkGray);
            GB_Status_AppendText_Nextline("(Before OTP Write) ZONE-C = : " + before_OTP_count_Zone_C.ToString(), System.Drawing.Color.DarkGray);
            GB_Status_AppendText_Nextline("(After OTP Write) ZONE-C = : " + After_OTP_count_Zone_C.ToString(), System.Drawing.Color.DarkGray);

            if (checkBox_1st_Mode_OTPl_Zone_A.Checked && before_OTP_count_Zone_A != (After_OTP_count_Zone_A - 1))
            {
//                GB_Status_AppendText_Nextline(before_OTP_count_Zone_A.ToString(),Color.Blue);
//                GB_Status_AppendText_Nextline((After_OTP_count_Zone_A - 1).ToString(), Color.Blue);
                GB_Status_AppendText_Nextline("OTP Write Count Check Fail(Zone-A)", Color.Red);
                OTP_Write_Result_Ok = false;
                return;
            }

            if (checkBox_1st_Mode_OTPl_Zone_B.Checked && before_OTP_count_Zone_B != (After_OTP_count_Zone_B - 1))
            {
//                GB_Status_AppendText_Nextline(before_OTP_count_Zone_B.ToString(), Color.Blue);
//                GB_Status_AppendText_Nextline((After_OTP_count_Zone_B - 1).ToString(), Color.Blue);
                GB_Status_AppendText_Nextline("OTP Write Count Check Fail(Zone-B)", Color.Red);
                OTP_Write_Result_Ok = false;
                return;
            }

            if (checkBox_1st_Mode_OTPl_Zone_C.Checked && before_OTP_count_Zone_C != (After_OTP_count_Zone_C - 1))
            {
//                GB_Status_AppendText_Nextline(before_OTP_count_Zone_C.ToString(), Color.Blue);
//                GB_Status_AppendText_Nextline((After_OTP_count_Zone_C - 1).ToString(), Color.Blue);
                GB_Status_AppendText_Nextline("OTP Write Count Check Fail(Zone-C)", Color.Red);
                OTP_Write_Result_Ok = false;
                return;
            }

            GB_Status_AppendText_Nextline("OTP Write Count Check Succeed", Color.Green);
        }


        private void button_1st_model_OTP_Check_Click(object sender, EventArgs e)
        {
            GB_Status_AppendText_Nextline("== OTP Checking ==", System.Drawing.Color.Black);
            First_Model_OTP_Zone_RB_Checked_False();
            System.Windows.Forms.Application.DoEvents();

            First_Model_OTP_Zone_RB_Visibie(false);
            textBox2_cmd.Clear();

            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x37 0x01");
            IPC_Quick_Send("mipi.read 0x06 0x43");
            Thread.Sleep(100);
            char[] mipi_data = textBox2_cmd.Text.ToCharArray();
            string temp_data = new string(mipi_data, 19, 2);
            byte byte_temp_data = Convert.ToByte(temp_data,16);
            //System.Windows.Forms.MessageBox.Show(byte_temp_data.ToString());

            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0, Disable All Registers

            int Zone_A = (byte_temp_data & 0x03);
            int Zone_B = (byte_temp_data & 0x1C) >> 2;
            int Zone_C = (byte_temp_data & 0xE0) >> 5;

            //GB_Status_AppendText_Nextline("ZONE-A = " + Zone_A.ToString(), System.Drawing.Color.DarkGray);
            //GB_Status_AppendText_Nextline("ZONE-B = " + Zone_B.ToString(), System.Drawing.Color.DarkGray);
            //GB_Status_AppendText_Nextline("ZONE-C = " + Zone_C.ToString(), System.Drawing.Color.DarkGray);

            Write_Before_OTP_Zone_A = Zone_A;
            Write_Before_OTP_Zone_B = Zone_B;
            Write_Before_OTP_Zone_C = Zone_C;

            //GB_Status_AppendText_Nextline(((~Zone_A) & 0x03).ToString(), Color.Black);
            //GB_Status_AppendText_Nextline(((~Zone_B) & 0x03).ToString(), Color.Black);
            //GB_Status_AppendText_Nextline(((~Zone_C) & 0x03).ToString(), Color.Black);


            if (checkBox_1st_Mode_OTPl_Zone_A.Checked)
            {
                First_Model_OTP_Zone_RB_Visibie(true, "ZONE A");
                switch (Zone_A)
                {
                    case 3:
                        Zone_A_RB_OTP_0.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-A OTP : 0", System.Drawing.Color.DarkGray);
                        break;
                    case 2:
                        Zone_A_RB_OTP_1.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-A OTP : 1", System.Drawing.Color.DarkGray);
                        break;
                    case 1:
                        Zone_A_RB_OTP_2.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-A OTP : 2", System.Drawing.Color.DarkGray);
                        break;
                    case 0:
                        Zone_A_RB_OTP_3.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-A OTP : 3", System.Drawing.Color.DarkGray);
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("ZONE-A cannot be this value : " + Zone_A.ToString());
                        break;
                }
            }

            if (checkBox_1st_Mode_OTPl_Zone_B.Checked)
            {
                First_Model_OTP_Zone_RB_Visibie(true, "ZONE B");
                switch (Zone_B)
                {
                    case 7:
                        Zone_B_RB_OTP_0.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-B OTP : 0", System.Drawing.Color.DarkGray);
                        break;
                    case 6:
                        Zone_B_RB_OTP_1.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-B OTP : 1", System.Drawing.Color.DarkGray);
                        break;
                    case 5:
                        Zone_B_RB_OTP_2.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-B OTP : 2", System.Drawing.Color.DarkGray);
                        break;
                    case 4:
                        Zone_B_RB_OTP_3.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-B OTP : 3", System.Drawing.Color.DarkGray);
                        break;
                    case 3:
                        Zone_B_RB_OTP_4.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-B OTP : 4", System.Drawing.Color.DarkGray);
                        break;
                    case 2:
                        Zone_B_RB_OTP_5.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-B OTP : 5", System.Drawing.Color.DarkGray);
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("ZONE-B cannot be this value : " + Zone_B.ToString());
                        break;
                }
            }

            if (checkBox_1st_Mode_OTPl_Zone_C.Checked)
            {
                First_Model_OTP_Zone_RB_Visibie(true, "ZONE C");
                switch (Zone_C)
                {
                    case 7:
                        Zone_C_RB_OTP_0.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-C OTP : 0", System.Drawing.Color.DarkGray);
                        break;
                    case 6:
                        Zone_C_RB_OTP_1.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-C OTP : 1", System.Drawing.Color.DarkGray);
                        break;
                    case 5:
                        Zone_C_RB_OTP_2.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-C OTP : 2", System.Drawing.Color.DarkGray);
                        break;
                    case 4:
                        Zone_C_RB_OTP_3.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-C OTP : 3", System.Drawing.Color.DarkGray);
                        break;
                    case 3:
                        Zone_C_RB_OTP_4.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-C OTP : 4", System.Drawing.Color.DarkGray);
                        break;
                    case 2:
                        Zone_C_RB_OTP_5.Checked = true;
                        GB_Status_AppendText_Nextline("ZONE-C OTP : 5", System.Drawing.Color.DarkGray);
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("ZONE-C cannot be this value : " + Zone_C.ToString());
                        break;
                }
            }
            System.Windows.Forms.Application.DoEvents();
        }

        public void DP116_CMD2_Page_Selection(int page_num, bool OTP_Load_Enable, bool FB_Enable, ref string Current_Page_Address, bool Current_Page_Initalize)
        {
            if (Current_Page_Initalize) Current_Page_Address = "Initalize(0x99)";

            DP116_NT37280 DP116_Func = DP116_NT37280.getInstance();

            string Address = "0x00";
            Address = DP116_Func.page_selection(page_num);

            if (Current_Page_Address != Address)
            {
                Current_Page_Address = Address;
                IPC_Quick_Send("mipi.write 0x15 0xFF " + Address);
                //FB = 0x00 (When Sleep-Out , this page's OTP loading is enable)
                //FB = 0x01 (When Sleep-Out , this page's OTP loading is disable)   
                if (FB_Enable)
                {
                    if (OTP_Load_Enable) IPC_Quick_Send("mipi.write 0x15 0xFB 0x00");
                    else IPC_Quick_Send("mipi.write 0x15 0xFB 0x01");
                }
                //GB_Status_AppendText_Nextline("CMD2 P" + page_num.ToString() + " was selected", Color.Black);
            }
            if (radioButton_Debug_Status_Mode.Checked) GB_Status_AppendText_Nextline("After Current_Page_Address = " + Current_Page_Address, Color.Black);
        }

        public void DP116_CMD2_Page_Selection_And_Show(int page_num, bool OTP_Load_Enable, Color color, bool FB_Enable, ref string Current_Page_Address, bool Current_Page_Initalize)
        {
            DP116_NT37280 DP116_Func = DP116_NT37280.getInstance();
            if (Current_Page_Initalize) Current_Page_Address = "Initalize(0x99)";

            string Address = "0x00";
            Address = DP116_Func.page_selection(page_num);

            if (Current_Page_Address != Address)
            {
                Current_Page_Address = Address;
                IPC_Quick_Send_And_Show("mipi.write 0x15 0xFF " + Address, color);
                //FB = 0x00 (When Sleep-Out , this page's OTP loading is enable)
                //FB = 0x01 (When Sleep-Out , this page's OTP loading is disable)       
                if (FB_Enable)
                {
                    if (OTP_Load_Enable) IPC_Quick_Send_And_Show("mipi.write 0x15 0xFB 0x00", color);
                    else IPC_Quick_Send_And_Show("mipi.write 0x15 0xFB 0x01", color);
                }
                //GB_Status_AppendText_Nextline("CMD2 P" + page_num.ToString() + " was selected", Color.Black);
            }
            if (radioButton_Debug_Status_Mode.Checked) GB_Status_AppendText_Nextline("After Current_Page_Address = " + Current_Page_Address, Color.Black);
        }


        public void DP116_CMD1_Page_Selection(bool OTP_Load_Enable, bool FB_Enable, ref string Current_Page_Address, bool Current_Page_Initalize)
        {
            if (Current_Page_Initalize) Current_Page_Address = "Initalize(0x99)";

            string Address = "0x10";
            if (Current_Page_Address != Address)
            {
                Current_Page_Address = Address;
                IPC_Quick_Send("mipi.write 0x15 0xFF 0x10");
                //IPC_Quick_Send("mipi.write 0x15 0xFB 0x01");
                //FB = 0x00 (When Sleep-Out , this page's OTP loading is enable)
                //FB = 0x01 (When Sleep-Out , this page's OTP loading is disable)
                if (FB_Enable)
                {
                    if (OTP_Load_Enable) IPC_Quick_Send("mipi.write 0x15 0xFB 0x00");
                    else IPC_Quick_Send("mipi.write 0x15 0xFB 0x01");
                }
                //GB_Status_AppendText_Nextline("CMD1 was selected", Color.Black); 
            }
            if (radioButton_Debug_Status_Mode.Checked) GB_Status_AppendText_Nextline("After Current_Page_Address = " + Current_Page_Address, Color.Black);
        }

        void ProcessBar_Perform_step_Everysecond(System.Windows.Forms.ProgressBar Bar , int times)
        {
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(1000);
                Bar.PerformStep();
            }
        }

        public void First_Model_OTP_Write_Button_Click()
        {
            button_1st_model_OTP_Write.PerformClick();
        }

        private void Check_OTP_Write_Enable(ref bool OTP_Write_Enable)
        {
            if (checkBox_1st_Mode_OTPl_Zone_A.Checked && Zone_A_RB_OTP_3.Checked)
            {
                    System.Windows.Forms.MessageBox.Show("Zone-A OTP is full , cannot write OTP");
                    GB_Status_AppendText_Nextline("Zone-A OTP is full , cannot write OTP", System.Drawing.Color.Red);
                    OTP_Write_Enable = false;
            }

            if (checkBox_1st_Mode_OTPl_Zone_B.Checked && Zone_B_RB_OTP_5.Checked)
            {
                    System.Windows.Forms.MessageBox.Show("Zone-B OTP is full , cannot write OTP");
                    GB_Status_AppendText_Nextline("Zone-B OTP is full , cannot write OTP", System.Drawing.Color.Red);
                    OTP_Write_Enable = false;                   
            }

            if (checkBox_1st_Mode_OTPl_Zone_C.Checked && Zone_C_RB_OTP_5.Checked)
            {
                    System.Windows.Forms.MessageBox.Show("Zone-C OTP is full , cannot write OTP");
                    GB_Status_AppendText_Nextline("Zone-C OTP is full , cannot write OTP", System.Drawing.Color.Red);
                    OTP_Write_Enable = false;   
            }
        }

        private bool OTP_Write(object sender, EventArgs e,double Delay_Margin)
        {
            bool OTP_Write_Enable = true;
            bool OTP_Write_Result_Ok = false;
            button_1st_model_OTP_Check_Click(sender, e); //OTP Count Check
            Check_OTP_Write_Enable(ref OTP_Write_Enable); //Check OUTs are available
            if (OTP_Write_Enable) //If OTPs are not full
            {

                GB_Status_AppendText_Nextline("OTP Writing Started", System.Drawing.Color.Green);
                System.Windows.Forms.Application.DoEvents();
                //--------OTP Write Seqeunce Start-----------//

                //Set Non-Reload OTP Command 0xFB for each initial and register setting Page
                DP116_CMD1_Page_Selection(false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
                for (int i = 0; i <= 12; i++)
                    DP116_CMD2_Page_Selection(i, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);

                //Disable Flash Auto Reload
                DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
                IPC_Quick_Send("mipi.write 0x15 0x48 0x01");
                IPC_Quick_Send("delay " + (50 * Delay_Margin).ToString());

                //Sleep Out + delay 150 + Display Off + Video Disable + Delay 50
                DP116_CMD1_Page_Selection(false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
                IPC_Quick_Send("mipi.write 0x05 0x11");//Sleep Out
                IPC_Quick_Send("delay " + (150 * Delay_Margin).ToString());
                IPC_Quick_Send("mipi.write 0x05 0x28");
                IPC_Quick_Send("mipi.video.disable");
                IPC_Quick_Send("delay " + (50 * Delay_Margin).ToString());

                
                //  (Check All Register Values are Correct by read Check-Sum(CRC))
                DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
                IPC_Quick_Send("mipi.write 0x15 0x37 0x01"); //CRC_CALCULATION[1:0] = 1, Enable All Registers
                //Read 4A,4B,4C,4D
                this.Read_DP116_Page_Quantity_Register(0, 1, "4A");
                DP116_CRC_Check_CMD2_P0_4Ah = dataGridView1.Rows[0].Cells[1].Value.ToString();

                this.Read_DP116_Quantity_Register(1, "4B");
                DP116_CRC_Check_CMD2_P0_4Bh = dataGridView1.Rows[0].Cells[1].Value.ToString();

                this.Read_DP116_Quantity_Register(1, "4C");
                DP116_CRC_Check_CMD2_P0_4Ch = dataGridView1.Rows[0].Cells[1].Value.ToString();

                this.Read_DP116_Quantity_Register(1, "4D");
                DP116_CRC_Check_CMD2_P0_4Dh = dataGridView1.Rows[0].Cells[1].Value.ToString();

                GB_Status_AppendText_Nextline(DP116_CRC_Check_CMD2_P0_4Ah, System.Drawing.Color.Green);
                GB_Status_AppendText_Nextline(DP116_CRC_Check_CMD2_P0_4Bh, System.Drawing.Color.Green);
                GB_Status_AppendText_Nextline(DP116_CRC_Check_CMD2_P0_4Ch, System.Drawing.Color.Green);
                GB_Status_AppendText_Nextline(DP116_CRC_Check_CMD2_P0_4Dh, System.Drawing.Color.Green);


                IPC_Quick_Send("delay " + (10 * Delay_Margin).ToString());
                

                //OTP Zone Selection
                //D2:OTP C Select D[1,0] , D1: OTP B Selection , D0:OTP A Selection //ex) 011 = 3h = OTP A&B Select
                DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
                string string_Select_Zone_Param = Convert.ToInt16(checkBox_1st_Mode_OTPl_Zone_C.Checked).ToString()
                                                   + Convert.ToInt16(checkBox_1st_Mode_OTPl_Zone_B.Checked).ToString()
                                                   + Convert.ToInt16(checkBox_1st_Mode_OTPl_Zone_A.Checked).ToString();
                int Int_Select_Zone_Param = Convert.ToInt16(string_Select_Zone_Param, 2);
                IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //#CRC_CALCULATION[1:0] = 0, Disable All Registers
                IPC_Quick_Send("mipi.write 0x15 0x3F 0x0" + Int_Select_Zone_Param.ToString()); //OTP Zone Selection Setting

                //Initial OTP Flow + delay 120
                DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
                IPC_Quick_Send("mipi.write 0x15 0x38 0x01");
                IPC_Quick_Send("delay " + (120 * Delay_Margin).ToString());

                //Enable OTP and signal output from GPIO[5]
                IPC_Quick_Send("mipi.write 0x15 0x39 0x10");
                IPC_Quick_Send("delay " + (10 * Delay_Margin).ToString());

                //Enable OTP Write
                IPC_Quick_Send("mipi.write 0x15 0x3D 0x01");
                IPC_Quick_Send("mipi.write 0x15 0x40 0x55");
                IPC_Quick_Send("mipi.write 0x15 0x41 0xAA");
                IPC_Quick_Send("mipi.write 0x15 0x42 0x66");
                IPC_Quick_Send("delay " + (4000 * Delay_Margin).ToString());

                //0x47 Read ~~
                this.Read_DP116_Page_Quantity_Register(0, 1, "47");
                string OTP_Write_Check = dataGridView1.Rows[0].Cells[1].Value.ToString();
                
                if (OTP_Write_Check == "01") // (CMD2_P0) 0x47 Read Result is "OPT Write Succeed"
                {
                    //Disable OTP Write
                    IPC_Quick_Send("mipi.write 0x15 0x3D 0x00");
                    IPC_Quick_Send("delay " + (150 * Delay_Margin).ToString());
                    this.GB_Status_AppendText_Nextline("OTP Write Succeed", Color.Green);
                    OTP_Write_Result_Ok = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("OTP Write Fail");
                    this.GB_Status_AppendText_Nextline("OTP Write Fail", Color.Red);
                    OTP_Write_Result_Ok = false;
                }

            }
            return OTP_Write_Result_Ok;
        }

        private void Verify_CRC_After_OTP_Write(ref bool OTP_Write_Result_Ok, double Delay_Margin)
        {
            //OTP Check After OTP Write
            OTP_Count_Verify_After_Write(ref OTP_Write_Result_Ok);

            //  (Check All Register Values are Correct by read Check-Sum(CRC))
            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x01"); //CRC_CALCULATION[1:0] = 1, Enable All Registers
            //Read 4A,4B,4C,4D
            this.Read_DP116_Page_Quantity_Register(0, 1, "4A");
            string CMD2_P0_4Ah = dataGridView1.Rows[0].Cells[1].Value.ToString();

            this.Read_DP116_Quantity_Register(1, "4B");
            string CMD2_P0_4Bh = dataGridView1.Rows[0].Cells[1].Value.ToString();
            
            this.Read_DP116_Quantity_Register(1, "4C");
            string CMD2_P0_4Ch = dataGridView1.Rows[0].Cells[1].Value.ToString();
           
            this.Read_DP116_Quantity_Register(1, "4D");
            string CMD2_P0_4Dh = dataGridView1.Rows[0].Cells[1].Value.ToString();

            if (CMD2_P0_4Ah == DP116_CRC_Check_CMD2_P0_4Ah && CMD2_P0_4Bh == DP116_CRC_Check_CMD2_P0_4Bh
                && CMD2_P0_4Ch == DP116_CRC_Check_CMD2_P0_4Ch && CMD2_P0_4Dh == DP116_CRC_Check_CMD2_P0_4Dh)
            {
                GB_Status_AppendText_Nextline("CRC Read OK", Color.Green);
                GB_Status_AppendText_Nextline(CMD2_P0_4Ah, Color.FromArgb(0, 100, 0));
                GB_Status_AppendText_Nextline(CMD2_P0_4Bh, Color.FromArgb(0, 100, 0));
                GB_Status_AppendText_Nextline(CMD2_P0_4Ch, Color.FromArgb(0, 100, 0));
                GB_Status_AppendText_Nextline(CMD2_P0_4Dh, Color.FromArgb(0, 100, 0));
            }
            else
            {
                GB_Status_AppendText_Nextline("CRC Read Fail", Color.Red);
                GB_Status_AppendText_Nextline(CMD2_P0_4Ah, Color.FromArgb(100, 0, 0));
                GB_Status_AppendText_Nextline(CMD2_P0_4Bh, Color.FromArgb(100, 0, 0));
                GB_Status_AppendText_Nextline(CMD2_P0_4Ch, Color.FromArgb(100, 0, 0));
                GB_Status_AppendText_Nextline(CMD2_P0_4Dh, Color.FromArgb(100, 0, 0));
                OTP_Write_Result_Ok = false;
            }

            IPC_Quick_Send("delay " + (10 * Delay_Margin).ToString());

            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 1, Disable All Registers
        }

        private bool OTP_Normal_Read_Check(double Delay_Margin)
        {
            GB_Status_AppendText_Nextline("========= OTP Normal Read Check =========", Color.Black);
            bool OTP_Write_Result_Ok = true; //초기에 True 설정, 문제생길경우에만 추후 False 처리 (True -> False (가능) , False -> True (불가))
            //Disable Flash Auto Reload
            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x48 0x01");
            IPC_Quick_Send("delay " + (50 * Delay_Margin).ToString());

            //Sleep Out
            DP116_CMD1_Page_Selection(false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x05 0x11");
            IPC_Quick_Send("delay " + (150 * Delay_Margin).ToString());

            //Set_CRC_Calculation
            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x01"); //CRC_CALCULATION[1:0] = 1, Enable All Registers
            IPC_Quick_Send("delay " + (10 * Delay_Margin).ToString());


            Verify_CRC_After_OTP_Write(ref OTP_Write_Result_Ok, Delay_Margin);

            return OTP_Write_Result_Ok;
        }
        private bool OTP_Initial_Margin_Read_Check(double Delay_Margin)
        {
            GB_Status_AppendText_Nextline("========= OTP Initial Margin Read Check =========", Color.Black);
            bool OTP_Write_Result_Ok = true; //초기에 True 설정, 문제생길경우에만 추후 False 처리 (True -> False (가능) , False -> True (불가))

            //Turn Off CRC_Calculation
            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0
            IPC_Quick_Send("delay " + (10 * Delay_Margin).ToString());

            IPC_Quick_Send("mipi.write 0x15 0x38 0xE0"); //Set Initial Margin Read
            IPC_Quick_Send("delay " + (2 * Delay_Margin).ToString());

            //Set VDD = 1.1V
            IPC_Quick_Send("mipi.write 0x15 0x35 0x33");
            IPC_Quick_Send("mipi.write 0x15 0x36 0x33");

            //Sleep Out
            DP116_CMD1_Page_Selection(false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x05 0x11");
            IPC_Quick_Send("delay " + (150 * Delay_Margin).ToString());

            //Set_CRC_Calculation
            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x01"); //CRC_CALCULATION[1:0] = 1, Enable All Registers
            IPC_Quick_Send("delay " + (10 * Delay_Margin).ToString());

            Verify_CRC_After_OTP_Write(ref OTP_Write_Result_Ok, Delay_Margin);

            return OTP_Write_Result_Ok;
        }
        private bool OTP_Program_Margin_Read_Check(double Delay_Margin)
        {
            GB_Status_AppendText_Nextline("========= OTP Program Margin Read Check =========", Color.Black);
            bool OTP_Write_Result_Ok = true; //초기에 True 설정, 문제생길경우에만 추후 False 처리 (True -> False (가능) , False -> True (불가))

            //Turn Off CRC_Calculation
            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0
            IPC_Quick_Send("delay " + (10 * Delay_Margin).ToString());

            IPC_Quick_Send("mipi.write 0x15 0x38 0x70"); //Set Initial Margin Read
            IPC_Quick_Send("delay " + (2 * Delay_Margin).ToString());

            //Sleep Out
            DP116_CMD1_Page_Selection(false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x05 0x11");
            IPC_Quick_Send("delay " + (150 * Delay_Margin).ToString());

            //Set_CRC_Calculation
            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x01"); //CRC_CALCULATION[1:0] = 1, Enable All Registers
            IPC_Quick_Send("delay " + (10 * Delay_Margin).ToString());

            Verify_CRC_After_OTP_Write(ref OTP_Write_Result_Ok, Delay_Margin);

            return OTP_Write_Result_Ok;
        }

        private void DP116_DDVDH_Setting_For_Customer_STMP()
        {
            GB_Status_AppendText_Nextline("=====Setting For STMP PMIC=====", Color.Blue);
            DP116_CMD2_Page_Selection_And_Show(0, false,Color.Blue, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send_And_Show("mipi.write 0x15 0x77 0x0E",Color.Blue);
            IPC_Quick_Send_And_Show("mipi.write 0x15 0x7A 0x0E", Color.Blue); // AVDD = 6.7v (190420)
            IPC_Quick_Send_And_Show("mipi.write 0x15 0x81 0x0E", Color.Blue); // AOD In AVDD = 6.7v (190819)
            IPC_Quick_Send_And_Show("mipi.write 0x15 0x82 0x0E", Color.Blue);// AOD Out AVDD = 6.7v (190420)
            IPC_Quick_Send_And_Show("mipi.write 0x15 0x83 0x0E", Color.Blue); // AOD In AVDD = 6.7v (190819)
            IPC_Quick_Send_And_Show("mipi.write 0x15 0x84 0x0E", Color.Blue); // AOD Out AVDD = 6.7v (190420)
            IPC_Quick_Send_And_Show("mipi.write 0x15 0x7E 0x77", Color.Blue);//190626 

            DP116_CMD2_Page_Selection_And_Show(12, false, Color.Blue, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send_And_Show("mipi.write 0x39 0xC4 0x90 0x08",Color.Blue); //#190626 (ELVSS change at 1 frame)
            Thread.Sleep(100);
        }

        private void button_1st_model_OTP_Write_Click(object sender, EventArgs e)
        {
            if (checkBox_DP086_DP116_OTP_Write_For_STMP.Checked) DP116_DDVDH_Setting_For_Customer_STMP(); //Temporary Ver
            
            double Delay_Margin = 1.2;
            
            if (OTP_Write(sender, e, Delay_Margin)) //OTP Write + (CMD2_P0) 0x47 Read Verify + (Auto Inital)OTP Check
            {
                // Auto Sequence 추가
                PNC_Auto_on_btn_Click(sender, e);
                Thread.Sleep(2000);
                IPC_Quick_Send("delay " + (150 * Delay_Margin).ToString());

                //--------------------CRC Skip Ver-------------------
                DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
                IPC_Quick_Send("mipi.write 0x15 0x37 0x00");
                IPC_Quick_Send("mipi.write 0x15 0x38 0x00");
                IPC_Quick_Send("mipi.write 0x15 0x35 0x00");
                IPC_Quick_Send("mipi.write 0x15 0x36 0x00");
                IPC_Quick_Send("mipi.write 0x15 0x48 0x00");
                GB_Status_AppendText_Nextline("OTP Write Succeed (Skip CRC Check) !", Color.Green);
                //---------------------------------------------------

                /*
                if (OTP_Normal_Read_Check(Delay_Margin)) //Normal Read Check 구현 아직안됨(우선 False 로 해놓음)
                {
                    if (OTP_Initial_Margin_Read_Check(Delay_Margin)) //Initial Margin Read Check 구현 아직안됨 (우선 False 로 해놓음)
                    {
                        if (OTP_Program_Margin_Read_Check(Delay_Margin)) //Program Margin Read Check 구현 아직안됨 (우선 False 로 해놓음)
                        {
                            //Turn Off Margin Read & CRC Calculation + Set VDD to Default + Enable Flash Auto Reload
                            DP116_CMD2_Page_Selection(0, false, true);
                            IPC_Quick_Send("mipi.write 0x15 0x37 0x00");
                            IPC_Quick_Send("mipi.write 0x15 0x38 0x00");
                            IPC_Quick_Send("mipi.write 0x15 0x35 0x00");
                            IPC_Quick_Send("mipi.write 0x15 0x36 0x00");
                            IPC_Quick_Send("mipi.write 0x15 0x48 0x00");

                            GB_Status_AppendText_Nextline("OTP Write Succeed !", Color.Green);
                        }
                    }
                }
                */
            }
        }

        //Manual Code On
        private void PNC_Manual_on_btn_Click(object sender, EventArgs e)
        {
            IPC_Quick_Send("ipc.file.send " + textBox_Manual_Address.Text);

            int max_line = Convert.ToInt16(textBox_GB_Status_Max_Line.Text);
            if (GB_Lines > max_line) button_Clear_Status_Text.PerformClick();
            GB_Lines++;
            RichTextBox_GB_Status.AppendText("Manual Code\r\n");  
        }

        public void PNC_Manual_Button_Click()
        {
            PNC_Manual_on_btn.PerformClick();
        }

        //Turn Off Code On
        private void PNC_turn_off_btn_Click(object sender, EventArgs e)
        {
            IPC_Quick_Send("ipc.file.send " + this.Turn_Off_Path);

            int max_line = Convert.ToInt16(textBox_GB_Status_Max_Line.Text);
            if (GB_Lines > max_line) button_Clear_Status_Text.PerformClick();
            GB_Lines++;
            RichTextBox_GB_Status.AppendText("Power Off Code\r\n");
        }

        public void AOD_On()
        {
            button_AOD_In.PerformClick();
        }
         
        public void AOD_Off()
        {
            button_AOD_Out.PerformClick();
        }

        //VR In Code On
        private void button_VR_In_Click(object sender, EventArgs e)
        {
            IPC_Quick_Send("ipc.file.send " + this.VR_In_Path);

            int max_line = Convert.ToInt16(textBox_GB_Status_Max_Line.Text);
            if (GB_Lines > max_line) button_Clear_Status_Text.PerformClick();
            GB_Lines++;
            RichTextBox_GB_Status.AppendText("VR In\r\n");
        }

        //VR Out Code On
        private void button_VR_Out_Click(object sender, EventArgs e)
        {
            IPC_Quick_Send("ipc.file.send " + this.VR_Out_Path);

            int max_line = Convert.ToInt16(textBox_GB_Status_Max_Line.Text);
            if (GB_Lines > max_line) button_Clear_Status_Text.PerformClick();
            GB_Lines++;
            RichTextBox_GB_Status.AppendText("VR Out\r\n");
        }

        public void PNC_Auto_Button_Click()
        {
            PNC_Auto_on_btn.PerformClick();
        }

        private void PNC_Auto_on_btn_Click(object sender, EventArgs e)
        {
            IPC_Quick_Send("ipc.file.send " + this.textBox_Auto_Address.Text);

            int max_line = Convert.ToInt16(textBox_GB_Status_Max_Line.Text);
            if (GB_Lines > max_line) button_Clear_Status_Text.PerformClick();
            GB_Lines++;
            RichTextBox_GB_Status.AppendText("Auto Code\r\n");
        }

        private void trackBar2_ValueChanged_1(object sender, EventArgs e)
        {
            objMemory.ChannelNO = CA_Channel_trackBar.Value;
            textBox_ch.Text = CA_Channel_trackBar.Value.ToString();
            Verify_Channel_WRGB_Update(sender, e);
        }

        private void CA_Channel_Update()
        {
            objMemory.ChannelNO = Convert.ToInt32(this.textBox_ch.Text);
            CA_Channel_trackBar.Value = Convert.ToInt32(this.textBox_ch.Text);
        }

        public void Set_GB_ProgressBar_Maximum(int max)
        {
            progressBar_GB.Maximum = max;
        }

        public void Add_GB_ProgressBar_Maximum(int Plue)
        {
            progressBar_GB.Maximum += Plue;
        }

        public void Add_one_GB_ProgressBar_Maximum()
        {
            progressBar_GB.Maximum++;
        }

        public void Set_GB_ProgressBar_Value(int Value)
        {
            progressBar_GB.Value = Value;
        }

        public void ADD_GB_ProgressBar_Value(int Value)
        {
            progressBar_GB.Value += Value;
        }

        public void Set_GB_ProgressBar_Step(int step)
        {
            progressBar_GB.Step = step;
        }   
        
        public void GB_ProgressBar_PerformStep()
        {
            progressBar_GB.PerformStep();
        }

        private void button_GCS_Measure_Click(object sender, EventArgs e)
        {
            GCS_BCS_Stop = false;

            GB_Status_AppendText_Nextline("Gray GCS Measuring Started", System.Drawing.Color.Black);
            Grid_Clear_button_Click(sender, e);

            int GCS_Max = Convert.ToUInt16(textBox_GCS_Max.Text);
            int GCS_Min = Convert.ToUInt16(textBox_GCS_Min.Text);

            if (GCS_Max > 255 || GCS_Max < 1)
            {
                GCS_Max = 255;
                textBox_GCS_Max.Text = "255";
            }

            if (GCS_Min < 0 || GCS_Min > 254)
            {
                GCS_Min = 0;
                textBox_GCS_Min.Text = "0";
            }

            if (GCS_Min >= GCS_Max)
            {
                GCS_Max = 255;
                textBox_GCS_Max.Text = "255";
                GCS_Min = 0;
                textBox_GCS_Min.Text = "0";
            }

            this.progressBar_GB.Maximum = GCS_Max - GCS_Min;
            progressBar_GB.Value = GCS_Min - GCS_Min;

            dataGridView2.Columns[0].HeaderText = "Gray";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";
                      
            for (int j = 4; j < dataGridView2.ColumnCount; j++)
                dataGridView2.Columns[j].HeaderText = string.Empty;

            int miliseconds = Convert.ToInt32(GCS_BCS_Delay_Textbox.Text);
            int gray;

            int Step_Value;

            if (step_value_1.Checked)
                Step_Value = 1;
            else if (step_value_4.Checked)
                Step_Value = 4;
            else if (step_value_8.Checked)
                Step_Value = 8;
            else if (step_value_16.Checked)
                Step_Value = 16;
            else
            {
                Step_Value = 0;
                System.Windows.Forms.MessageBox.Show("It's impossible");
            }

            if (max_to_min_rb.Checked)
            {
                for (int i = GCS_Max; i > GCS_Min - Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i - Step_Value;
                    gray = i + Step_Value;

                    if (gray > GCS_Max)
                        gray = GCS_Max;
                    else if (gray < GCS_Min)
                        gray = GCS_Min;

                    progressBar_GB.Value = GCS_Max - gray;

                    if (radioButton_Gray_W.Checked) Set_GCS(gray, gray, gray);
                    else if (radioButton_Gray_R.Checked) Set_GCS(gray, 0, 0);
                    else if (radioButton_Gray_Y.Checked) Set_GCS(gray, gray, 0);
                    else if (radioButton_Gray_G.Checked) Set_GCS(0, gray, 0);
                    else if (radioButton_Gray_C.Checked) Set_GCS(0, gray, gray);
                    else if (radioButton_Gray_B.Checked) Set_GCS(0, 0, gray);
                    else if (radioButton_Gray_M.Checked) Set_GCS(gray, 0, gray);
                    else { MessageBox.Show("this cannot happen"); }

                    Thread.Sleep(miliseconds);

                    if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked) Average_Measure_5_Min_Max_Lv_Delete(gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked) Get_Max_Measure(2, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, gray.ToString());
                }
            }
            else if (min_to_max_rb.Checked)
            {
                for (int i = GCS_Min; i < GCS_Max + Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i + Step_Value;
                    gray = i - Step_Value;


                    if (gray > GCS_Max)
                        gray = GCS_Max;
                    else if (gray < GCS_Min)
                        gray = GCS_Min;

                    progressBar_GB.Value = gray - GCS_Min;

                    if (radioButton_Gray_W.Checked) Set_GCS(gray, gray, gray);
                    else if (radioButton_Gray_R.Checked) Set_GCS(gray, 0, 0);
                    else if (radioButton_Gray_Y.Checked) Set_GCS(gray, gray, 0);
                    else if (radioButton_Gray_G.Checked) Set_GCS(0, gray, 0);
                    else if (radioButton_Gray_C.Checked) Set_GCS(0, gray, gray);
                    else if (radioButton_Gray_B.Checked) Set_GCS(0, 0, gray);
                    else if (radioButton_Gray_M.Checked) Set_GCS(gray, 0, gray);
                    else { MessageBox.Show("this cannot happen"); }

                    Thread.Sleep(miliseconds);
                    if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked) Average_Measure_5_Min_Max_Lv_Delete(gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked) Get_Max_Measure(2, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, gray.ToString());
                }
            }
            GB_Status_AppendText_Nextline("Gray GCS Measuring Finished", System.Drawing.Color.Black);
        }


        public void dataGridView2_Rows_Add(string TBD, string X = "-", string Y = "-", string Lv = "-")
        {
            dataGridView2.Rows.Add(TBD, X, Y, Lv);
        }

        private void Average_Measure_5_Min_Max_Lv_Delete(string GCS_Or_BCS_String = "-")
        {
            XYLv[] Measure = new XYLv[5];
            try
            {
                isMsr = true;
                CA_Measure_button.Enabled = false;

                int Max_Index = 0; double Max_Value = 0;
                int Min_Index = 0; double Min_Value = 2000;

                for (int a = 0; a < 5; a++)
                {
                    objCa.Measure();
                    Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                    Measure[a].Double_Update_From_String();
                    GB_Status_AppendText_Nextline(a.ToString() + ")Measured X/Y/Lv = " + Measure[a].X + "/" + Measure[a].Y + "/" + Measure[a].Lv, Color.Green);

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

                GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                GB_Status_AppendText_Nextline("Min Lv/Ave Lv/Max Lv = " + Measure[Min_Index].double_Lv.ToString() + "/" + Ave_Measure.Lv + "/" + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Blue);
                
                X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                
                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add(GCS_Or_BCS_String, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;

                System.Windows.Forms.Application.DoEvents();
                CA_Measure_button.Enabled = true;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void Get_Max_Measure(int Measure_times = 2,string GCS_Or_BCS_String = "-")
        {
            XYLv[] Measure = new XYLv[Measure_times];
            int Max_Index = 0; double Max_Value = 0;
            
            try
            {
                isMsr = true;
                CA_Measure_button.Enabled = false;
                if (Measure_times > 1)
                {
                    for (int a = 0; a < Measure_times; a++)
                    {
                        objCa.Measure();
                        Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                        Measure[a].Double_Update_From_String();

                        if (Measure[a].double_Lv > Max_Value)
                        {
                            Max_Value = Measure[a].double_Lv;
                            Max_Index = a;
                        }

                        GB_Status_AppendText_Nextline(a.ToString() + ")Measured X/Y/Lv = " + Measure[a].X + "/" + Measure[a].Y + "/" + Measure[a].Lv, Color.Green);
                    }
                    GB_Status_AppendText_Nextline("Max_Lv Index = " + Max_Index.ToString() , Color.Black);
                    GB_Status_AppendText_Nextline("Max X/Y/Lv = " + Measure[Max_Index].X + "/" + Measure[Max_Index].Y + "/" + Measure[Max_Index].Lv, Color.Blue);

                    X_Value_display.Text = Measure[Max_Index].X.PadRight(6, '0');
                    Y_Value_display.Text = Measure[Max_Index].Y.PadRight(6, '0');
                    Lv_Value_display.Text = Measure[Max_Index].Lv.PadRight(6, '0');
                }
                else if (Measure_times == 1)
                {
                    objCa.Measure();
                    X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                }
                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add(GCS_Or_BCS_String, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;

                System.Windows.Forms.Application.DoEvents();
                CA_Measure_button.Enabled = true;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }



        public void Average_Measure(int Measure_times = 1, string GCS_Or_BCS_String = "-",int Condition = 1)
        {
            XYLv[] Measure = new XYLv[Measure_times];
            try
            {
                isMsr = true;
                CA_Measure_button.Enabled = false;
                if (Measure_times > 1)
                {
                    XYLv Sum_Measure = new XYLv();
                    XYLv Ave_Measure = new XYLv();
                    Sum_Measure.Set_Value(0, 0, 0);
                    Ave_Measure.Set_Value(0, 0, 0);

                    for (int a = 0; a < Measure_times; a++)
                    {
                        objCa.Measure();
                        Measure[a].X = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Measure[a].Y = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Measure[a].Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                        Measure[a].Double_Update_From_String();
                        Sum_Measure.double_X += Measure[a].double_X;
                        Sum_Measure.double_Y += Measure[a].double_Y;
                        Sum_Measure.double_Lv += Measure[a].double_Lv;
                        //GB_Status_AppendText_Nextline(a.ToString() + ")Measured X/Y/Lv = " + Measure[a].X + "/" + Measure[a].Y + "/" + Measure[a].Lv, Color.Green);
                    }

                    Ave_Measure.double_X = Math.Round((Sum_Measure.double_X / Measure_times), 4);
                    Ave_Measure.double_Y = Math.Round((Sum_Measure.double_Y / Measure_times), 4);
                    Ave_Measure.double_Lv = Math.Round((Sum_Measure.double_Lv / Measure_times), 4);
                    Ave_Measure.String_Update_From_Double();


                    GB_Status_AppendText_Nextline("Ave X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Blue);
                    
                    X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                    Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                    Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                }
                else if (Measure_times == 1)
                {
                    objCa.Measure();
                    X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                }
                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add(GCS_Or_BCS_String, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;

                System.Windows.Forms.Application.DoEvents();
                CA_Measure_button.Enabled = true;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }



        private void BCS_Meas_Without_Delete_Gridview()
        {
            int BCS_Max = Convert.ToInt32(textBox_BCS_Max.Text, 16);
            int BCS_Min = Convert.ToInt32(textBox_BCS_Min.Text, 16);

            if (BCS_Max > 4095 || BCS_Max < 1)
            {
                textBox_BCS_Max.Text = "FFF";
                BCS_Max = 4095;
            }

            if (BCS_Min < 0 || BCS_Min > 4094)
            {
                textBox_BCS_Min.Text = "0";
                BCS_Min = 0;
            }

            if (BCS_Min >= BCS_Max)
            {
                textBox_BCS_Max.Text = "FFF";
                BCS_Max = 4095;
                textBox_BCS_Min.Text = "0";
                BCS_Min = 0;
            }

            GCS_BCS_Stop = false;

            GB_Status_AppendText_Nextline("BCS Measuring Started", System.Drawing.Color.DarkGray);
            //Grid_Clear_button_Click(sender, e);

            this.progressBar_GB.Maximum = BCS_Max - BCS_Min;
            progressBar_GB.Value = BCS_Min - BCS_Min;

            dataGridView2.Columns[0].HeaderText = "DBV";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";
            for (int j = 4; j < dataGridView2.ColumnCount; j++)
                dataGridView2.Columns[j].HeaderText = string.Empty;

            int miliseconds = Convert.ToInt32(GCS_BCS_Delay_Textbox.Text);
            int dbv;
            int Step_Value;

            if (step_value_1.Checked)
                Step_Value = 1;
            else if (step_value_4.Checked)
                Step_Value = 4;
            else if (step_value_8.Checked)
                Step_Value = 8;
            else if (step_value_16.Checked)
                Step_Value = 16;
            else
            {
                Step_Value = 0;
                System.Windows.Forms.MessageBox.Show("It's impossible");
            }
            if (max_to_min_rb.Checked)
            {
                for (int i = BCS_Max; i >= BCS_Min - Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i - Step_Value;
                    dbv = i + Step_Value;

                    if (dbv > BCS_Max)
                        dbv = BCS_Max;
                    else if (dbv < BCS_Min)
                        dbv = BCS_Min;

                    progressBar_GB.Value = BCS_Max - dbv;

                    Set_BCS(dbv);
                    DBV_textbox.Text = dbv.ToString("X");
                    DBV_trackbar.Value = dbv;

                    Thread.Sleep(miliseconds);

                    if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked) Average_Measure_5_Min_Max_Lv_Delete(dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked) Get_Max_Measure(2, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, dbv.ToString());

                    if (dbv == 1)
                    {
                        break;
                    }
                }
            }
            else if (min_to_max_rb.Checked)
            {
                for (int i = BCS_Min; i < BCS_Max + Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i + Step_Value;
                    dbv = i - Step_Value;

                    if (dbv > BCS_Max)
                        dbv = BCS_Max;
                    else if (dbv < BCS_Min)
                        dbv = BCS_Min;

                    progressBar_GB.Value = dbv - BCS_Min;

                    Set_BCS(dbv);
                    DBV_textbox.Text = dbv.ToString("X");
                    DBV_trackbar.Value = dbv;

                    Thread.Sleep(miliseconds);

                    if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked) Average_Measure_5_Min_Max_Lv_Delete(dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked) Get_Max_Measure(2, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, dbv.ToString());

                    if (dbv == BCS_Max)
                    {
                        break;
                    }
                }
            }
            GB_Status_AppendText_Nextline("BCS Measuring Finished", System.Drawing.Color.DarkGray);
        }



        private void button_BCS_Measure_Click(object sender, EventArgs e)
        {
            int BCS_Max = Convert.ToInt32(textBox_BCS_Max.Text, 16);
            int BCS_Min = Convert.ToInt32(textBox_BCS_Min.Text, 16);

            if (BCS_Max > 4095 || BCS_Max < 1)
            {
                textBox_BCS_Max.Text = "FFF";
                BCS_Max = 4095;
            }

            if (BCS_Min < 0 || BCS_Min > 4094)
            {
                textBox_BCS_Min.Text = "0";
                BCS_Min = 0;
            }

            if (BCS_Min >= BCS_Max)
            {
                textBox_BCS_Max.Text = "FFF";
                BCS_Max = 4095;
                textBox_BCS_Min.Text = "0";
                BCS_Min = 0;
            }

                GCS_BCS_Stop = false;

                GB_Status_AppendText_Nextline("BCS Measuring Started", System.Drawing.Color.DarkGray);
                Grid_Clear_button_Click(sender, e);

                this.progressBar_GB.Maximum = BCS_Max - BCS_Min;
                progressBar_GB.Value = BCS_Min - BCS_Min;

                dataGridView2.Columns[0].HeaderText = "DBV";
                dataGridView2.Columns[1].HeaderText = "X";
                dataGridView2.Columns[2].HeaderText = "Y";
                dataGridView2.Columns[3].HeaderText = "Lv";
                for (int j = 4; j < dataGridView2.ColumnCount; j++)
                    dataGridView2.Columns[j].HeaderText = string.Empty;

                int miliseconds = Convert.ToInt32(GCS_BCS_Delay_Textbox.Text);
                int dbv;
                int Step_Value;

                if (step_value_1.Checked)
                    Step_Value = 1;
                else if (step_value_4.Checked)
                    Step_Value = 4;
                else if (step_value_8.Checked)
                    Step_Value = 8;
                else if (step_value_16.Checked)
                    Step_Value = 16;
                else
                {
                    Step_Value = 0;
                    System.Windows.Forms.MessageBox.Show("It's impossible");
                }
                if (max_to_min_rb.Checked)
                {
                    for (int i = BCS_Max; i >= BCS_Min -Step_Value; )
                    {
                        if (GCS_BCS_Stop)
                            break;

                        i = i - Step_Value;
                        dbv = i + Step_Value;

                        if (dbv > BCS_Max)
                            dbv = BCS_Max;
                        else if (dbv < BCS_Min)
                            dbv = BCS_Min;

                        progressBar_GB.Value = BCS_Max - dbv;

                        Set_BCS(dbv);
                        DBV_textbox.Text = dbv.ToString("X");
                        DBV_trackbar.Value = dbv;

                        Thread.Sleep(miliseconds);

                        if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked) Average_Measure_5_Min_Max_Lv_Delete(dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked) Get_Max_Measure(2, dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, dbv.ToString());


                       


                        if (dbv == 1)
                        {
                            break;
                        }
                    }
                }
                else if (min_to_max_rb.Checked)
                {
                    for (int i = BCS_Min; i < BCS_Max + Step_Value; )
                    {
                        if (GCS_BCS_Stop)
                            break;

                        i = i + Step_Value;
                        dbv = i - Step_Value;

                        if (dbv > BCS_Max)
                            dbv = BCS_Max;
                        else if (dbv < BCS_Min)
                            dbv = BCS_Min;

                        progressBar_GB.Value = dbv - BCS_Min;

                        Set_BCS(dbv);
                        DBV_textbox.Text = dbv.ToString("X");
                        DBV_trackbar.Value = dbv;

                        Thread.Sleep(miliseconds);

                        if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked) Average_Measure_5_Min_Max_Lv_Delete(dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked) Get_Max_Measure(2, dbv.ToString());
                        else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, dbv.ToString());

                        if (dbv == BCS_Max)
                        {
                            break;
                        }
                    }
                }
                GB_Status_AppendText_Nextline("BCS Measuring Finished", System.Drawing.Color.DarkGray);
        }

        public void Select_Channel_For_Red()
        {
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_R.Text);
            GB_Status_AppendText_Nextline("Red Channel Selected (" + textBox_verify_R.Text + ")", Color.Red);
        }

        public void Select_Channel_For_Green()
        {
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_G.Text);
            GB_Status_AppendText_Nextline("Green Channel Selected (" + textBox_verify_G.Text + ")", Color.Green);
        }

        public void Select_Channel_For_Blue()
        {
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_B.Text);
            GB_Status_AppendText_Nextline("Blue Channel Selected (" + textBox_verify_B.Text + ")", Color.Blue);
        }

        public void Select_Channel_For_White()
        {
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_W.Text);
            GB_Status_AppendText_Nextline("White Channel Selected (" + textBox_verify_W.Text + ")",Color.Black);
        }

       

        private void WRGB_verify_btn_Click(object sender, EventArgs e)
        {
            GB_Status_AppendText_Nextline("== W/R/G/B/Black Verify Start ==", RichTextBox_GB_Status.ForeColor);

            // White
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_W.Text);
            PTN_update(255, 255, 255);
            Thread.Sleep(300);
            CA_Measure_button_Click(sender, e);
            GB_Status_AppendText_Nextline("White : "+"("+X_Value_display.Text +"),("+Y_Value_display.Text+"),("+Lv_Value_display.Text+")",RichTextBox_GB_Status.ForeColor);

            // Red
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_R.Text);
            PTN_update(255, 0, 0);
            Thread.Sleep(300);
            CA_Measure_button_Click(sender, e);
            GB_Status_AppendText_Nextline("Red : "+"(" + X_Value_display.Text + "),(" + Y_Value_display.Text + "),(" + Lv_Value_display.Text + ")", System.Drawing.Color.Red);


            // Green
            PTN_update(0, 255, 0);
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_G.Text);
            Thread.Sleep(300);
            CA_Measure_button_Click(sender, e);
            GB_Status_AppendText_Nextline("Green : "+"(" + X_Value_display.Text + "),(" + Y_Value_display.Text + "),(" + Lv_Value_display.Text + ")", System.Drawing.Color.Green);

            
            // Blue
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_B.Text);
            PTN_update(0, 0, 255);
            Thread.Sleep(300);
            CA_Measure_button_Click(sender, e);
            GB_Status_AppendText_Nextline("Blue : " + "(" + X_Value_display.Text + "),(" + Y_Value_display.Text + "),(" + Lv_Value_display.Text + ")", System.Drawing.Color.Blue);


            // Black
            objMemory.ChannelNO = Convert.ToInt32(textBox_verify_W.Text);
            PTN_update(0, 0, 0);
            Thread.Sleep(300);
            CA_Measure_button_Click(sender, e);
            GB_Status_AppendText_Nextline("Black : " + "(" + X_Value_display.Text + "),(" + Y_Value_display.Text + "),(" + Lv_Value_display.Text + ")", RichTextBox_GB_Status.ForeColor);
        }


        public void Show_OK_Message(string text)
        {
            GB_Status_AppendText_Nextline(text, Color.Green);
        }

        public void Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound(string text)
        {
            GB_Status_AppendText_Nextline(text, Color.Red);
            
            SoundPlayer simpleSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\wavs\\NG_Sound.wav");
            simpleSound.PlayLooping();
            if (System.Windows.Forms.MessageBox.Show(text) == DialogResult.OK)
            {
                GB_Status_AppendText_Nextline("OK Clicked", Color.Blue);
                simpleSound.Stop();
            }
        }

        

        public void GB_Status_AppendText_Nextline(string text,System.Drawing.Color color,bool Debug_Mode = false)
        {
            if (Debug_Mode == true && radioButton_Debug_Status_Mode.Checked == false)
            {
                //Do nothing
            }
            else
            {
                if(Is_1ch_CA_Mode)
                {
                    int max_line = Convert.ToInt32(textBox_GB_Status_Max_Line.Text);
                    if (GB_Lines > max_line) button_Clear_Status_Text.PerformClick();
                    GB_Lines++;

                    //Color (Text 색 바꾸고,AppendText)
                    RichTextBox_GB_Status.SelectionColor = color;
                    RichTextBox_GB_Status.AppendText(text + "\r\n");
                    //Black (Text 색 원복 as ForeColor)
                    RichTextBox_GB_Status.SelectionColor = RichTextBox_GB_Status.ForeColor;//System.Drawing.Color.Black;
                                                                                           //Scroll to the end Without Focus
                    RichTextBox_GB_Status.SelectionStart = RichTextBox_GB_Status.Text.Length;
                    RichTextBox_GB_Status.ScrollToCaret();
                }
                else
                {
                    CA_Multi_Channel_Form.getInstance().GB_Status_AppendText_Nextline(text, color);
                }
            }
        }


        public void Showing_Diff_and_Current_Vreg1_and_Gamma(RGB Prev_Gamma, RGB Gamma, int Prev_Vreg1,int Vreg1)
        {
            int Diff_Vreg1 = Vreg1 - Prev_Vreg1;
            RGB Diff_Gamma = Gamma - Prev_Gamma;
            Gamma.String_Update_From_int();
            GB_Status_AppendText_Nextline("Diff Vreg1/R/G/B = " + Diff_Vreg1.ToString() + "/" + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B + " (Vreg1/R/G/B = " + Vreg1.ToString()  + "," + Gamma.R + "," + Gamma.G + "," + Gamma.B + ")", Color.Blue);                        
        }

        public void Showing_Diff_and_Current_Gamma(RGB Prev_Gamma, RGB Gamma)
        {
            RGB Diff_Gamma = Gamma - Prev_Gamma;
            Diff_Gamma.String_Update_From_int();
            Gamma.String_Update_From_int();
            GB_Status_AppendText_Nextline("Diff_Gamma R/G/B = " + Diff_Gamma.R + "/" + Diff_Gamma.G + "/" + Diff_Gamma.B + " (Gamma R/G/B = " + Gamma.R + "," + Gamma.G + "," + Gamma.B + ")", Color.Blue);                        
        }

        private void button_GCS_R_Click(object sender, EventArgs e)
        {
            GCS_BCS_Stop = false;

            GB_Status_AppendText_Nextline("Red GCS Measuring Started", System.Drawing.Color.Red);
            Grid_Clear_button_Click(sender, e);

            objMemory.ChannelNO = Convert.ToInt32(this.textBox_verify_R.Text);
            this.progressBar_GB.Maximum = 255;
            progressBar_GB.Value = 0;

            
            dataGridView2.Columns[0].HeaderText = "Gray";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";
            
            for (int j = 4; j < dataGridView2.ColumnCount; j++)
                dataGridView2.Columns[j].HeaderText = string.Empty;

            int miliseconds = Convert.ToInt32(GCS_BCS_Delay_Textbox.Text);
            int gray;

            int Step_Value;

            if (step_value_1.Checked)
                Step_Value = 1;
            else if (step_value_4.Checked)
                Step_Value = 4;
            else if (step_value_8.Checked)
                Step_Value = 8;
            else if (step_value_16.Checked)
                Step_Value = 16;
            else
            {
                Step_Value = 0;
                System.Windows.Forms.MessageBox.Show("It's impossible");
            }

            if (max_to_min_rb.Checked)
            {
                for (int i = 255; i > -Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i - Step_Value;
                    gray = i + Step_Value;

                    if (gray > 255)
                        gray = 255;
                    else if (gray < 0)
                        gray = 0;

                    progressBar_GB.Value = 255 - gray;

                    Set_GCS(gray, 0, 0);
                    Thread.Sleep(miliseconds);

                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        CA_zero_cal_button.Enabled = false;

                        objCa.Measure();

                        //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                        //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                        //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                        {
                            break;
                        }

                        CA_Measure_button.Enabled = true;
                        CA_zero_cal_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        dataGridView2.Rows.Add(gray, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        //To scroll to bottom of DataGridView 
                        dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                        /////////////////////////////////////////
                        if (Step_Value == 1 && gray == 0)
                            break;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }

                }
            }
            else if (min_to_max_rb.Checked)
            {
                for (int i = 0; i < 255 + Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i + Step_Value;
                    gray = i - Step_Value;


                    if (gray > 255)
                        gray = 255;
                    else if (gray < 0)
                        gray = 0;

                    progressBar_GB.Value = gray;

                    Set_GCS(gray, 0, 0);
                    Thread.Sleep(miliseconds);
                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        CA_zero_cal_button.Enabled = false;

                        objCa.Measure();

                        //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                        //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                        //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                        {
                            break;
                        }

                        CA_Measure_button.Enabled = true;
                        CA_zero_cal_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        dataGridView2.Rows.Add(gray, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        //To scroll to bottom of DataGridView 
                        dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                        /////////////////////////////////////////
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }

                }
            }
            GB_Status_AppendText_Nextline("Red GCS Measuring Finished", System.Drawing.Color.Red);
        }

        private void button_GCS_G_Click(object sender, EventArgs e)
        {
            GCS_BCS_Stop = false;
            
            GB_Status_AppendText_Nextline("Green GCS Measuring Started", System.Drawing.Color.Green);
            Grid_Clear_button_Click(sender, e);

            objMemory.ChannelNO = Convert.ToInt32(this.textBox_verify_G.Text);
            this.progressBar_GB.Maximum = 255;
            progressBar_GB.Value = 0;

            dataGridView2.Columns[0].HeaderText = "Gray";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";
            for (int j = 4; j < dataGridView2.ColumnCount; j++)
                dataGridView2.Columns[j].HeaderText = string.Empty;

            int miliseconds = Convert.ToInt32(GCS_BCS_Delay_Textbox.Text);
            int gray;

            int Step_Value;

            if (step_value_1.Checked)
                Step_Value = 1;
            else if (step_value_4.Checked)
                Step_Value = 4;
            else if (step_value_8.Checked)
                Step_Value = 8;
            else if (step_value_16.Checked)
                Step_Value = 16;
            else
            {
                Step_Value = 0;
                System.Windows.Forms.MessageBox.Show("It's impossible");
            }

            if (max_to_min_rb.Checked)
            {
                for (int i = 255; i > -Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i - Step_Value;
                    gray = i + Step_Value;

                    if (gray > 255)
                        gray = 255;
                    else if (gray < 0)
                        gray = 0;

                    progressBar_GB.Value = 255 - gray;

                    Set_GCS(0, gray, 0);
                    Thread.Sleep(miliseconds);

                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        CA_zero_cal_button.Enabled = false;

                        objCa.Measure();

                        //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                        //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                        //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                        {
                            break;
                        }

                        CA_Measure_button.Enabled = true;
                        CA_zero_cal_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        dataGridView2.Rows.Add(gray, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        //To scroll to bottom of DataGridView 
                        dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                        /////////////////////////////////////////
                        if (Step_Value == 1 && gray == 0)
                            break;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }

                }
            }
            else if (min_to_max_rb.Checked)
            {
                for (int i = 0; i < 255 + Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i + Step_Value;
                    gray = i - Step_Value;


                    if (gray > 255)
                        gray = 255;
                    else if (gray < 0)
                        gray = 0;

                    progressBar_GB.Value = gray;

                    Set_GCS(0, gray, 0);
                    Thread.Sleep(miliseconds);
                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        CA_zero_cal_button.Enabled = false;

                        objCa.Measure();

                        //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                        //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                        //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                        {
                            break;
                        }

                        CA_Measure_button.Enabled = true;
                        CA_zero_cal_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        dataGridView2.Rows.Add(gray, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        //To scroll to bottom of DataGridView 
                        dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                        /////////////////////////////////////////
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }

                }
            }
            GB_Status_AppendText_Nextline("Green GCS Measuring Finished", System.Drawing.Color.Green);
        }

        private void button_GCS_B_Click(object sender, EventArgs e)
        {
            GCS_BCS_Stop = false;

            GB_Status_AppendText_Nextline("Blue GCS Measuring Started", System.Drawing.Color.Blue);
            Grid_Clear_button_Click(sender, e);

            objMemory.ChannelNO = Convert.ToInt32(this.textBox_verify_B.Text);
            this.progressBar_GB.Maximum = 255;
            progressBar_GB.Value = 0;

            dataGridView2.Columns[0].HeaderText = "Gray";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";
            for (int j = 4; j < dataGridView2.ColumnCount; j++)
                dataGridView2.Columns[j].HeaderText = string.Empty;

            int miliseconds = Convert.ToInt32(GCS_BCS_Delay_Textbox.Text);
            int gray;

            int Step_Value;

            if (step_value_1.Checked)
                Step_Value = 1;
            else if (step_value_4.Checked)
                Step_Value = 4;
            else if (step_value_8.Checked)
                Step_Value = 8;
            else if (step_value_16.Checked)
                Step_Value = 16;
            else
            {
                Step_Value = 0;
                System.Windows.Forms.MessageBox.Show("It's impossible");
            }

            if (max_to_min_rb.Checked)
            {
                for (int i = 255; i > -Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i - Step_Value;
                    gray = i + Step_Value;

                    if (gray > 255)
                        gray = 255;
                    else if (gray < 0)
                        gray = 0;

                    progressBar_GB.Value = 255 - gray;

                    Set_GCS(0, 0, gray);
                    Thread.Sleep(miliseconds);

                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        CA_zero_cal_button.Enabled = false;

                        objCa.Measure();

                        //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                        //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                        //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                        {
                            break;
                        }

                        CA_Measure_button.Enabled = true;
                        CA_zero_cal_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        dataGridView2.Rows.Add(gray, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        //To scroll to bottom of DataGridView 
                        dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                        /////////////////////////////////////////
                        if (Step_Value == 1 && gray == 0)
                            break;
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }

                }
            }
            else if (min_to_max_rb.Checked)
            {
                for (int i = 0; i < 255 + Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i + Step_Value;
                    gray = i - Step_Value;


                    if (gray > 255)
                        gray = 255;
                    else if (gray < 0)
                        gray = 0;

                    progressBar_GB.Value = gray;

                    Set_GCS(0, 0, gray);
                    Thread.Sleep(miliseconds);
                    try
                    {
                        isMsr = true;

                        CA_Measure_button.Enabled = false;
                        CA_zero_cal_button.Enabled = false;

                        objCa.Measure();

                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                        {
                            break;
                        }

                        CA_Measure_button.Enabled = true;
                        CA_zero_cal_button.Enabled = true;

                        //Data Grid setting//////////////////////
                        dataGridView2.Rows.Add(gray, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                        //To scroll to bottom of DataGridView 
                        dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                        /////////////////////////////////////////
                    }
                    catch (Exception er)
                    {
                        DisplayError(er);
                        System.Windows.Forms.Application.Exit();
                    }

                }
            }
            GB_Status_AppendText_Nextline("Blue GCS Measuring Finished", System.Drawing.Color.Blue);
        }

        private void button_GCS_W_Click(object sender, EventArgs e)
        {
            objMemory.ChannelNO = Convert.ToInt32(this.textBox_verify_W.Text);
            button_GCS_Measure_Click(sender, e);
        }

        private void testDllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CPP_Dll_Test_Form.getInstance().Visible = false;
            CPP_Dll_Test_Form.getInstance().Show();
        }

        public void MX_OTP_Read(int Read_Start_Index, int Quantity, string Register)
        {
            if (Read_Start_Index == 0)
            {
                OTP_Read(Quantity, Register);
            }
            else
            {
                IPC_Quick_Send("mipi.write 0x15 0xB0 0x" + Read_Start_Index.ToString("X2")); 
                OTP_Read(Quantity, Register);
            }
        }

        public void MX_OTP_Read(byte[] output)
        {
            int Read_Start_Index = Convert.ToInt32(output[0]);
            string Register = output[1].ToString("X2");
            int Quantity = Convert.ToInt32(output[2]);
            MX_OTP_Read(Read_Start_Index, Quantity, Register);
        }

        public void OTP_Read(int Quantity, string Register)
        {
            Textbox_How_many.Text = Quantity.ToString();
            Textbox_OTP_1_register.Text = Register;
            OTP_1_register_read();
            Thread.Sleep(50);
        }


        public void Read_DP116_Page_Quantity_Register(int Page, int Quantity, string Register)
        {
            DP116_CMD2_Page_Selection(Page, false, false, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            Textbox_How_many.Text = Quantity.ToString();
            Textbox_OTP_1_register.Text = Register;
            OTP_1_register_read();
            Thread.Sleep(50);
        }

        public void DP116_Read_Gamma_Gray()
        {
           //B0 ~ E1
           Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(12, "B0");

           String Register = "B1";

           string temp_mipi_cmd = "mipi.write 0x39 0x" + Register;
           int Int_Register = Convert.ToInt16(Register,16);
           int Max_Register = Convert.ToInt16(Register, 16) + 41;

           for (; Int_Register < Max_Register; Int_Register++)
           {
               OTP_Read(12, Int_Register.ToString("X2"));
               temp_mipi_cmd = "mipi.write 0x39 0x" + Int_Register.ToString("X2") + " ";
               temp_mipi_cmd += textBox2_cmd.Text.Substring(18, 59);
               GB_Status_AppendText_Nextline(temp_mipi_cmd, Color.Black);
           }
        }

        public void DP116_Read_Vreg1_VREF_AM0_and_Send(bool condition,bool Single,ref string Current_Page_Address)
        {
            GB_Status_AppendText_Nextline("#VREF1 , VREF2 , AM0 Read and Send", System.Drawing.Color.Blue);
            DP116_CMD2_Page_Selection_And_Show(0, false, Color.DarkGreen, false, ref Current_Page_Address, true);
            Read_DP116_Quantity_Register_to_Send(1, "AD",true);
            Read_DP116_Quantity_Register_to_Send(1, "AE", true);
            Read_DP116_Quantity_Register_to_Send(1, "AF", true);

            if (Single) //Single Mode
            {
                GB_Status_AppendText_Nextline("#Vreg1 Read and Send", System.Drawing.Color.Blue);
                DP116_CMD2_Page_Selection_And_Show(0, false, Color.DarkGreen, false, ref Current_Page_Address, true);
                Read_DP116_Quantity_Register_to_Send(16, "DD", true);
                Read_DP116_Quantity_Register_to_Send(16, "DE", true);
            }
            else //Dual Mode
            {
                if (condition)
                {
                    GB_Status_AppendText_Nextline("#Condition 1 Vreg1 Read and Send", System.Drawing.Color.Blue);
                    DP116_CMD2_Page_Selection_And_Show(0, false, Color.DarkGreen, false, ref Current_Page_Address, true);
                    Read_DP116_Quantity_Register_to_Send(16, "DD", true);
                    Read_DP116_Quantity_Register_to_Send(16, "DE", true);
                }
                else
                {
                    GB_Status_AppendText_Nextline("#Condition 2 Vreg1 Send", System.Drawing.Color.Blue);
                    DP116_CMD2_Page_Selection_And_Show(1, false, Color.DarkGreen, false, ref Current_Page_Address, true);
                    Read_DP116_Quantity_Register_to_Send(16, "DD", true);
                    Read_DP116_Quantity_Register_to_Send(16, "DE", true);
                }
            }
        }

        public void Read_DP116_Quantity_Register_to_Send(int Quantity, string Register,bool UI_Show)
        {
            OTP_Read(Quantity, Register);
            string temp_mipi_cmd = string.Empty;

            if (Quantity == 1) temp_mipi_cmd = "mipi.write 0x15 0x" + Register;
            else temp_mipi_cmd = "mipi.write 0x39 0x" + Register;
            for (int i = 0; i < Quantity; i++) temp_mipi_cmd += " 0x" + dataGridView1.Rows[i].Cells[1].Value.ToString();

            if (UI_Show)
            {
                IPC_Quick_Send_And_Show(temp_mipi_cmd, Color.Black);
            }
            else
            {
                IPC_Quick_Send(temp_mipi_cmd);
            }
            
        }

        public void Read_DP116_CMD2_Quantity_Register_for_Gamma_Read(int Quantity, string Register,string annotation = "")
        {
            OTP_Read(Quantity, Register);
            string temp_mipi_cmd = string.Empty;
            if (Quantity == 1)
            {
                temp_mipi_cmd = "mipi.write 0x15 0x" + Register;
            }
            else
            {
                temp_mipi_cmd = "mipi.write 0x39 0x" + Register;
            }

            for (int i = 0; i < Quantity; i++)
            {
                temp_mipi_cmd += " 0x" + dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            if (annotation == string.Empty)
            GB_Status_AppendText_Nextline(temp_mipi_cmd, Color.Black);
            else
            GB_Status_AppendText_Nextline(temp_mipi_cmd + " #" + annotation, Color.Black);
        }


        public void Read_DP116_Quantity_Register(int Quantity, string Register)
        {
            Textbox_How_many.Text = Quantity.ToString();
            Textbox_OTP_1_register.Text = Register;
            OTP_1_register_read();
            Thread.Sleep(50);
        }

        public void DBV_Setting(string DBV)
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP086 || current_model.Get_Current_Model_Name() == Model_Name.DP116) DP116_CMD1_Page_Selection(false, false, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x39 0x51 0x0" + DBV[0] + " 0x" + DBV[1] + DBV[2]);
        }


        
        private void trackBar_APL_ValueChanged(object sender, EventArgs e)
        {
            double APL = trackBar_APL.Value * 0.1;
            double X = current_model.get_X();
            double Y = current_model.get_Y();
            int x1 = Convert.ToInt16((X * Math.Sqrt(APL)));
            int y1 = Convert.ToInt16((Y * Math.Sqrt(APL)));

            int Gray = trackBar_Gray.Value;
            if (radioButton_Gray_W.Checked) Image_Crosstalk(x1,y1,0,0,0,Gray,Gray,Gray);
            else if (radioButton_Gray_R.Checked)Image_Crosstalk(x1,y1,0,0,0,Gray,0,0);
            else if (radioButton_Gray_Y.Checked)Image_Crosstalk(x1,y1,0,0,0,Gray,Gray,0);
            else if (radioButton_Gray_G.Checked)Image_Crosstalk(x1,y1,0,0,0,0,Gray,0);
            else if (radioButton_Gray_C.Checked)Image_Crosstalk(x1,y1,0,0,0,0,Gray,Gray);
            else if (radioButton_Gray_B.Checked)Image_Crosstalk(x1,y1,0,0,0,0,0,Gray);
            else if (radioButton_Gray_M.Checked)Image_Crosstalk(x1,y1,0,0,0,Gray,0,Gray);
        }

        public void Image_Crosstalk(int x1,int y1,int B_R, int B_G, int B_B, int F_R, int F_G, int F_B)
        {
            //IPC_Quick_Send("image.crosstalk " + x1.ToString() + " " + y1.ToString() + " 0 0 0 " + Gray.ToString() + " " + Gray.ToString() + " " + Gray.ToString());
            IPC_Quick_Send("image.crosstalk " + x1.ToString() + " " + y1.ToString() + " " + B_R.ToString() + " " + B_G.ToString() + " " + B_B.ToString()
                + " " + F_R.ToString() + " " + F_G.ToString() + " " + F_B.ToString());
        }

        private void trackBar_APL_Scroll(object sender, EventArgs e)
        {

        }

        private void Model_Change_Close_Forms()
        {
            //DP086 & DP116
            if(First_Model_Option_Form.IsIstanceNull() == false) First_Model_Option_Form.getInstance().Visible = false;
            if(Engineer_Mornitoring_Mode.IsIstanceNull() == false) Engineer_Mornitoring_Mode.getInstance().Visible = false;
            if(Dual_Engineer_Monitoring_Mode.IsIstanceNull() == false) Dual_Engineer_Monitoring_Mode.getInstance().Visible = false;

            //DP150
            if (Second_Model_Option_Form.IsIstanceNull() == false) Second_Model_Option_Form.getInstance().Visible = false;
            if (DP150_Single_Engineerig_Mornitoring_Mode.IsIstanceNull() == false) DP150_Single_Engineerig_Mornitoring_Mode.getInstance().Visible = false;
            if (DP150_Dual_Engineering_Mornitoring_Mode.IsIstanceNull() == false) DP150_Dual_Engineering_Mornitoring_Mode.getInstance().Visible = false;

            //Meta
            if (Meta_Model_Option_Form.IsIstanceNull() == false) Meta_Model_Option_Form.getInstance().Visible = false;//add on 190903
            if (Meta_Engineer_Mornitoring_Mode.IsIstanceNull() == false) Meta_Engineer_Mornitoring_Mode.getInstance().Visible = false; //add on 190903

            //DP173 (EA9154)
            if (DP173_Model_Option_Form.IsIstanceNull() == false) DP173_Model_Option_Form.getInstance().Visible = false;
            if (DP173_Single_Engineering_Mornitoring.IsIstanceNull() == false) DP173_Single_Engineering_Mornitoring.getInstance().Visible = false;
            if (DP173_Dual_Engineering_Mornitoring.IsIstanceNull() == false) DP173_Dual_Engineering_Mornitoring.getInstance().Visible = false;

            //DP213 (EA9155)
            if (DP213_Model_Option_Form.IsIstanceNull() == false) DP213_Model_Option_Form.getInstance().Visible = false;
            if (DP213_Dual_Engineering_Mornitoring.IsIstanceNull() == false) DP213_Dual_Engineering_Mornitoring.getInstance().Visible = false;

            //BMP
            if (BMP_Image_Processing_Form.IsIstanceNull() == false) BMP_Image_Processing_Form.getInstance().Visible = false;

            //Practice
            if (Practice_Coding.IsIstanceNull() == false) Practice_Coding.getInstance().Visible = false;
            if (CPP_Dll_Test_Form.IsIstanceNull() == false) CPP_Dll_Test_Form.getInstance().Visible = false;

            //Measurement
            if (Dual_SH_Delta_E.IsIstanceNull() == false) Dual_SH_Delta_E.getInstance().Visible = false;
            if (SH_Delta_E.IsIstanceNull() == false) SH_Delta_E.getInstance().Visible = false;
            if (Optic_Measurement_Form.IsIstanceNull() == false) Optic_Measurement_Form.getInstance().Visible = false;
            if (Optic_Measurement_10ch.IsIstanceNull() == false) Optic_Measurement_10ch.getInstance().Visible = false;
            if (DBV_Accuracy.IsIstanceNull() == false) DBV_Accuracy.getInstance().Visible = false;

            //CommonUse
            if (MIPI_CMD_Ctrl_Form.IsIstanceNull() == false) MIPI_CMD_Ctrl_Form.getInstance().Visible = false;
            if (Mipi_Script_Convertor.IsIstanceNull() == false) Mipi_Script_Convertor.getInstance().Visible = false;
            if (OTP_Read_and_Compare.IsIstanceNull() == false) OTP_Read_and_Compare.getInstance().Visible = false;
            if (PNC_10ch_Board_Ctrl.IsIstanceNull() == false) PNC_10ch_Board_Ctrl.getInstance().Visible = false;
        }

        private void dP086ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model_dp086_update();
        }

        public void model_dp086_update()
        {
            current_model.Set_Model_As_DP086();
            Model_Setting();
        }




        private void DeleteAllSubFormObjects()
        {
            //DP086 or DP116
            First_Model_Option_Form.DeleteInstance();
            First_Model_Option_Form.DeleteInstance();
            Engineer_Mornitoring_Mode.DeleteInstance();
            Dual_Engineer_Monitoring_Mode.DeleteInstance();

            //DP150
            Second_Model_Option_Form.DeleteInstance();
            DP150_Single_Engineerig_Mornitoring_Mode.DeleteInstance();
            DP150_Dual_Engineering_Mornitoring_Mode.DeleteInstance();

            //Meta
            Meta_Model_Option_Form.DeleteInstance();
            Meta_Engineer_Mornitoring_Mode.DeleteInstance();

            //DP173 or Elgin
            DP173_Model_Option_Form.DeleteInstance();
            DP173_Single_Engineering_Mornitoring.DeleteInstance();
            DP173_Dual_Engineering_Mornitoring.DeleteInstance();

            //DP213
            DP213_Model_Option_Form.DeleteInstance();
            DP213_Dual_Engineering_Mornitoring.DeleteInstance();

            //BMP
            BMP_Image_Processing_Form.DeleteInstance();

            //Practice
            Practice_Coding.DeleteInstance();
            CPP_Dll_Test_Form.DeleteInstance();

            //Measurement
            Dual_SH_Delta_E.DeleteInstance();
            SH_Delta_E.DeleteInstance();
            Optic_Measurement_Form.DeleteInstance();
            Optic_Measurement_10ch.DeleteInstance();
            DBV_Accuracy.DeleteInstance();

            //CommonUse
            MIPI_CMD_Ctrl_Form.DeleteInstance();
            Mipi_Script_Convertor.DeleteInstance();
            OTP_Read_and_Compare.DeleteInstance();
            PNC_10ch_Board_Ctrl.DeleteInstance();
        }




        private void Model_Setting()
        {
            ShowModelInfo();
            Model_Change_Close_Forms();
            DeleteAllSubFormObjects();
            DBV_Accuracy_Measure_Show_or_Hide();
            OTP_Groupbox_Selection();
            current_model.Get_File_Address();
            Change_Tema(current_model.Get_Back_Ground_Color());
            DBV_Max_Setting();
        }

        private void DBV_Max_Setting()
        {
            DBV_trackbar.Maximum = current_model.get_DBV_Max();
            textBox_BCS_Max.Text = current_model.get_DBV_Max().ToString("X3");
        }

        private void ShowModelInfo()
        {
            GB_Status_AppendText_Nextline(current_model.Get_Current_Model_Name().ToString() + " Model was selected", System.Drawing.Color.Black);
            this.Text = current_model.Get_Current_Model_Name().ToString() + " BSQH Program";
            label_Model_Name.Text = "Model Name : " + current_model.Get_Current_Model_Name().ToString();
            label_Medel_Resolution.Text = "Model Resolution : " + current_model.get_X().ToString() + "x" + current_model.get_Y().ToString();
        }

        private void modelSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void CRC_Check()
        {
            button_Dp116_CRC_Check.PerformClick();
        }

        private void button_Dp116_CRC_Check_Click(object sender, EventArgs e)
        {
            //Flash Load X
            IPC_Quick_Send("mipi.write 0x15 0xFF 0x20");
            First_Model_Option_Form.getInstance().Current_Page_Address = "0x20";
            if (radioButton_Debug_Status_Mode.Checked) GB_Status_AppendText_Nextline("After Current_Page_Address = " + First_Model_Option_Form.getInstance().Current_Page_Address, Color.Black);

            IPC_Quick_Send("mipi.write 0x15 0xFB 0x01");
            IPC_Quick_Send("mipi.write 0x15 0x48 0x01");

            GB_Status_AppendText_Nextline("<CRC Check>", Color.FromArgb(0, 200, 0));
            //  (Check All Register Values are Correct by read Check-Sum(CRC))
            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x01"); //CRC_CALCULATION[1:0] = 1, Enable All Registers
            Thread.Sleep(2);
            //Read 4A,4B,4C,4D
            this.Read_DP116_Page_Quantity_Register(0, 1, "4A");
            string CMD2_P0_4Ah = dataGridView1.Rows[0].Cells[1].Value.ToString();

            this.Read_DP116_Quantity_Register(1, "4B");
            string CMD2_P0_4Bh = dataGridView1.Rows[0].Cells[1].Value.ToString();

            this.Read_DP116_Quantity_Register(1, "4C");
            string CMD2_P0_4Ch = dataGridView1.Rows[0].Cells[1].Value.ToString();

            this.Read_DP116_Quantity_Register(1, "4D");
            string CMD2_P0_4Dh = dataGridView1.Rows[0].Cells[1].Value.ToString();

            GB_Status_AppendText_Nextline(CMD2_P0_4Ah, Color.FromArgb(0, 100, 0));
            GB_Status_AppendText_Nextline(CMD2_P0_4Bh, Color.FromArgb(0, 100, 0));
            GB_Status_AppendText_Nextline(CMD2_P0_4Ch, Color.FromArgb(0, 100, 0));
            GB_Status_AppendText_Nextline(CMD2_P0_4Dh, Color.FromArgb(0, 100, 0));

            DP116_CMD2_Page_Selection(0, false, true, ref First_Model_Option_Form.getInstance().Current_Page_Address, true);
            IPC_Quick_Send("mipi.write 0x15 0x37 0x00"); //CRC_CALCULATION[1:0] = 0, Disable All Registers
            Thread.Sleep(2);
        }


        private void Total_Time_label_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }



        private void button_Cooling_Test_Click(object sender, EventArgs e)
        {
            Sub_Total_Timer_START();

            for (double minute = 0; minute <= 15; minute += 0.5)
            {
                CA_Cooling_Measure(minute);
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(30000); //30 seconds
            }

            Sub_Total_Timer_Stop();
        }

        private void CA_Cooling_Measure(double Minute)
        {
            //objCa.DisplayMode = 0; // 측정 모드는 xyLv
            //objCa.SyncMode = 0; //측정 모드는 NTSC

            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView2.Columns[0].HeaderText = "Minute";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

            int i;
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;
                CA_zero_cal_button.Enabled = false;
                for (i = 0; i < 1; i++)
                {
                    objCa.Measure();


                    label6.Text = "x";
                    label7.Text = "y";
                    label8.Text = "Lv";
                    //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                    //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                    //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                    X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   

                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                    {
                        break;
                    }
                }

                CA_Measure_button.Enabled = true;
                CA_zero_cal_button.Enabled = true;

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)


                //Add data to Datagridview
                dataGridView2.Rows.Add(Minute.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);

            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void oTPReadAndCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void button_GCS_BCS_Stop_Click(object sender, EventArgs e)
        {
            GCS_BCS_Stop = true;
        }

        private void button_WRGB_GCS_BCS_Stop_Click(object sender, EventArgs e)
        {
            GCS_BCS_Stop = true;
        }

        private void button_Count_Measure_Click(object sender, EventArgs e)
        {
            Change_N_Measure_Stop = false;
            Grid_Clear_button_Click(sender, e);
            for (int i = 0; i < Convert.ToInt16(textBox_Measure_Count.Text);i++)
            {
                if (Change_N_Measure_Stop) break;

                CA_Count_Measure(i+1);
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(Convert.ToInt16(textBox_Measure_Delay.Text));
            }
        }

        private void button_Change_and_Measure_Click(object sender, EventArgs e)
        {
            int First_Image_Index = Convert.ToInt16(textbox_Pattern_1st_Index.Text);
            int Last_Image_Index = Convert.ToInt16(textbox_Pattern_2nd_Index.Text);

            Change_N_Measure_Stop = false;

            for (int i = First_Image_Index; i <= Last_Image_Index; i++)
            {
                if (Change_N_Measure_Stop) break;

                IPC_Quick_Send("image.display " + i.ToString());
                Thread.Sleep(Convert.ToInt16(textBox_Measure_Delay.Text));
                CA_Image_Measure(i);
            }
        }

        private void button_C_n_M_Stop_Click(object sender, EventArgs e)
        {
            Change_N_Measure_Stop = true;
        }

        private void sH向DeltaEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label_CA_remote_status.Text == "CA Remote : On")
            {
                SH_Delta_E.getInstance().Visible = false;
                SH_Delta_E.getInstance().Show();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Connect CA310 First");
            }
        }

       

        

        public void Measure_Indicate_Gray(double Gray)
        {
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;

                for (int i = 0; i < 1; i++)
                {
                    objCa.Measure();
                    //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                    //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                    //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                    X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                        break;
                }

                CA_Measure_button.Enabled = true;

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add(Gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        

        public void Measure_Indicate_Gray(int Gray)
        {
                try
                {
                    isMsr = true;

                    CA_Measure_button.Enabled = false;

                    for (int i = 0; i < 1; i++)
                    {
                        objCa.Measure();
                        //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                        //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                        //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                        X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");   
                        System.Windows.Forms.Application.DoEvents();

                        if (isMsr == false)
                            break;
                    }

                    CA_Measure_button.Enabled = true;

                    //Data Grid setting//////////////////////
                    dataGridView2.DataSource = null; // reset (unbind the datasource)

                    //Add data to Datagridview
                    dataGridView2.Rows.Add(Gray.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                    dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                }
                catch (Exception er)
                {
                    DisplayError(er);
                    System.Windows.Forms.Application.Exit();
                }   
        }

        public void Measure_Indicate(int Value_TB_Indicated, ref double Measured_X, ref double Measured_Y, ref double Measured_Lv)
        {
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;

                for (int i = 0; i < 1; i++)
                {
                    objCa.Measure();
                    //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                    //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                    //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                    Measured_X = objCa.OutputProbes.get_ItemOfNumber(1).sx;
                    Measured_Y = objCa.OutputProbes.get_ItemOfNumber(1).sy;
                    Measured_Lv = objCa.OutputProbes.get_ItemOfNumber(1).Lv;

                    X_Value_display.Text = Measured_X.ToString("0.0000");
                    Y_Value_display.Text = Measured_Y.ToString("0.0000");
                    Lv_Value_display.Text = Measured_Lv.ToString("0.0000");
                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                        break;
                }

                CA_Measure_button.Enabled = true;

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add(Value_TB_Indicated.ToString(), X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void opticMeasurementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void engineeringModeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void singleModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Engineer_Mornitoring_Mode.IsIstanceNull() == false) Engineer_Mornitoring_Mode.getInstance().Hide();
            if (DP150_Single_Engineerig_Mornitoring_Mode.IsIstanceNull() == false) DP150_Single_Engineerig_Mornitoring_Mode.getInstance().Hide();
            if (Meta_Engineer_Mornitoring_Mode.IsIstanceNull() == false) Meta_Engineer_Mornitoring_Mode.getInstance().Hide();
            if (DP173_Single_Engineering_Mornitoring.IsIstanceNull() == false) DP173_Single_Engineering_Mornitoring.getInstance().Hide();

            Form single_engineering = Get_Current_Single_Engineering_Form();
            if (single_engineering != null)
                single_engineering.Show();
        }

        public void OC_Param_load()
        {
            current_model.OC_Param_load();
        }

        private void dualModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Dual_Engineer_Monitoring_Mode.IsIstanceNull() == false) Dual_Engineer_Monitoring_Mode.getInstance().Hide();
            if (DP150_Dual_Engineering_Mornitoring_Mode.IsIstanceNull() == false) DP150_Dual_Engineering_Mornitoring_Mode.getInstance().Hide();
            if (DP173_Dual_Engineering_Mornitoring.IsIstanceNull() == false) DP173_Dual_Engineering_Mornitoring.getInstance().Hide();
            if (DP173_Single_Engineering_Mornitoring.IsIstanceNull() == false) DP173_Single_Engineering_Mornitoring.getInstance().Hide();
            if (DP213_Dual_Engineering_Mornitoring.IsIstanceNull() == false) DP213_Dual_Engineering_Mornitoring.getInstance().Hide();

            Form dual_engineering = Get_Current_Dual_Engineering_Form();
            if (dual_engineering != null) dual_engineering.Show();
        }

        public void Dual_Engineering_Mornitoring_Show()
        {
            Dual_Engineer_Monitoring_Mode.getInstance().Show();
        }

        public void Dual_150_Engineering_Mornitoring_Show()
        {
            DP150_Dual_Engineering_Mornitoring_Mode.getInstance().Show();
        }


        public void Dual_Engineering_Mornitoring_Hide_Button_Enable(bool enable)
        {
            Dual_Engineer_Monitoring_Mode.getInstance().button_Hide.Enabled = enable;
        }

        public void Dual_DP150_Engineering_Mornitoring_Hide_Button_Enable(bool enable)
        {
            DP150_Dual_Engineering_Mornitoring_Mode.getInstance().button_Hide.Enabled = enable;
        }


        private void button_Clear_Status_Text_Click(object sender, EventArgs e)
        {
            GB_Lines = 0;

            RichTextBox_GB_Status.Clear();
            Thread.Sleep(2);
        }

        private void Get_All_Serial_Port()
        {
            string[] ports = SerialPort.GetPortNames();
            string Connected_Ports = string.Empty;
            int count = 0;
            foreach (string port in ports)
            {
                if (count == 0)
                {
                    if (port.Length == 4) //Port ComXX
                    {
                        this.textBox_CA_Port.Text = port[3].ToString();
                    }
                    else if (port.Length == 5) //Port ComXX
                    {
                        this.textBox_CA_Port.Text = (port[3].ToString() + port[4].ToString());
                    }
                    else //Port ComXXX
                    {
                        this.textBox_CA_Port.Text = (port[3].ToString() + port[4].ToString() + port[5].ToString());
                    }
                }
                Connected_Ports += (port + " ");
                count++;
            }
            if (count == 0)
            {
                GB_Status_AppendText_Nextline("There isn't any port connected to the CPU" + Connected_Ports, Color.Red);
            }
            else if (count == 1)
            {
                GB_Status_AppendText_Nextline(count + " Port is connected to CPU : " + Connected_Ports, Color.Green);
            }
            else
            {
                GB_Status_AppendText_Nextline(count + " Ports are connected to CPU : " + Connected_Ports, Color.Green);
            } 
        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {
        }

        private void CA410_connect_Click(object sender, EventArgs e)
        {
            Get_All_Serial_Port();

            GB_Status_AppendText_Nextline("CA410 Connect Start", Color.Black);
            int ca_port = Convert.ToInt16(textBox_CA_Port.Text);
            try
            {
                //objCa200.AutoConnect();
                objCa200.SetConfiguration(1, "1", ca_port, 38400);
                objCas = objCa200.Cas;
                objCa = objCas.get_ItemOfNumber(1);
                objCa.OutputProbes.Add("P1");
                objMemory = objCa.Memory; //171213 추가사항
                objProbeInfo = (CA200SRVRLib.IProbeInfo)objProbe; //171213 추가사항

                Is_Single_CA_is_connected = true;
                label_CA310_Status.Text = "CA(410) connection status : OK";
                label_CA310_Status.ForeColor = System.Drawing.Color.Green;
                Initialize_After_CA_Connection();

                //CA remote : On
                label_CA_remote_status.Text = "CA Remote : On";
                label_CA_remote_status.ForeColor = System.Drawing.Color.Green;
                this.textBox_ch.Text = objMemory.ChannelNO.ToString();
                CA_Channel_Update();
                GB_Status_AppendText_Nextline("CA410 Zero-Cal", Color.Black);
                CA_zero_cal_button.PerformClick();
                GB_Status_AppendText_Nextline("CA410 Connect Finished", Color.Black);
            }

            catch (Exception er)
            {
                DisplayError(er);
                //System.Windows.Forms.Application.Exit();
                System.Windows.Forms.MessageBox.Show("The CA410 is not connected , plz connect CA310 to the PC");
                Is_Single_CA_is_connected = false;
                label_CA310_Status.Text = "CA connection status : NG";
                label_CA310_Status.ForeColor = System.Drawing.Color.Red;

                Initialize_Before_CA_Connection();

                //CA remote : Off
                label_CA_remote_status.Text = "CA Remote : Off";
                label_CA_remote_status.ForeColor = System.Drawing.Color.Red;
            }
            CA_Remote_Status_Check();
            CA_Mode_Initialize();
        }

        private void radioButton_Normal_Status_Mode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_Debug_Status_Mode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void trackBar_Gray_ValueChanged(object sender, EventArgs e)
        {
            trackBar_APL.Value = trackBar_APL.Maximum;

            int Gray = trackBar_Gray.Value;
            if (radioButton_Gray_W.Checked)
            {
                PTN_update(Gray, Gray, Gray);
            }
            else if (radioButton_Gray_R.Checked)
            {
                PTN_update(Gray, 0, 0);
            }
            else if (radioButton_Gray_Y.Checked)
            {
                PTN_update(Gray, Gray, 0);
            }
            else if (radioButton_Gray_G.Checked)
            {
                PTN_update(0, Gray, 0);
            }
            else if (radioButton_Gray_C.Checked)
            {
                PTN_update(0, Gray, Gray);
            }
            else if (radioButton_Gray_B.Checked)
            {
                PTN_update(0, 0, Gray);
            }
            else if (radioButton_Gray_M.Checked)
            {
                PTN_update(Gray, 0, Gray);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error ! this cannot take place");
            }
        }

        private void trackBar_Gray_Scroll(object sender, EventArgs e)
        {

        }

         

        private void radioButton_Gray_W_CheckedChanged(object sender, EventArgs e)
        {
            trackBar_Gray_ValueChanged(sender, e);
            trackBar_Gray.BackColor = System.Drawing.Color.FromArgb(120, 120, 120);
        }

        private void radioButton_Gray_R_CheckedChanged(object sender, EventArgs e)
        {
            trackBar_Gray_ValueChanged(sender, e);
            trackBar_Gray.BackColor = System.Drawing.Color.FromArgb(120, 0, 0);
        }

        private void radioButton_Gray_Y_CheckedChanged(object sender, EventArgs e)
        {
            trackBar_Gray_ValueChanged(sender, e);
            trackBar_Gray.BackColor = System.Drawing.Color.FromArgb(120, 120, 0);
        }

        private void radioButton_Gray_G_CheckedChanged(object sender, EventArgs e)
        {
            trackBar_Gray_ValueChanged(sender, e);
            trackBar_Gray.BackColor = System.Drawing.Color.FromArgb(0, 120, 0);
        }

        private void radioButton_Gray_C_CheckedChanged(object sender, EventArgs e)
        {
            trackBar_Gray_ValueChanged(sender, e);
            trackBar_Gray.BackColor = System.Drawing.Color.FromArgb(0, 120, 120);
        }

        private void radioButton_Gray_B_CheckedChanged(object sender, EventArgs e)
        {
            trackBar_Gray_ValueChanged(sender, e);
            trackBar_Gray.BackColor = System.Drawing.Color.FromArgb(0, 0, 120);
        }

        private void radioButton_Gray_M_CheckedChanged(object sender, EventArgs e)
        {
            trackBar_Gray_ValueChanged(sender, e);
            trackBar_Gray.BackColor = System.Drawing.Color.FromArgb(120, 0, 120);
        }

        public void Magnachip_B0_CMD_Sending(string A, string B)
        {
            IPC_Quick_Send(A); //First
            IPC_Quick_Send(B);//Second
        }

        private void button_DP150_Flash_Write_Click(object sender, EventArgs e)
        {
            button_DP150_Flash_Erase_Click(sender, e);
            button_DP150_Flash_Erase.Enabled = false;
            System.Windows.Forms.Application.DoEvents();
            IPC_Quick_Send_And_Show("mipi.dsi 4 800 burst continuos dsc frc",Color.Red);

            //IPC_Quick_Send("mipi.write 0x15 0xB0 0xBC");
            //IPC_Quick_Send("mipi.write 0x39 0xE7 0x02");
            Magnachip_B0_CMD_Sending("mipi.write 0x15 0xB0 0xBC", "mipi.write 0x39 0xE7 0x02");

            IPC_Quick_Send("mipi.write 0x15 0x59 0x03");

            GB_Status_AppendText_Nextline("Flash Writing Start", Color.Blue);

            //IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
            //IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x04 0x00 0x00 0x00");
            Magnachip_B0_CMD_Sending("mipi.write.hs 0x39 0xB0 0x06", "mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x04 0x00 0x00 0x00");
            Thread.Sleep(100);
            
            if (DP150_Flash_Write_Verify())
            {
                GB_Status_AppendText_Nextline("Flash(1) OK", Color.Green);
                IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");
                
                IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x02 0x00 0x00 0x00");
                Thread.Sleep(100);

                if (DP150_Flash_Write_Verify())
                {
                    GB_Status_AppendText_Nextline("Flash(2) OK", Color.Green);
                    IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                    IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");
                    
                    IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                    IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x01 0x00 0x00 0x00");
                    Thread.Sleep(100);

                    if (DP150_Flash_Write_Verify())
                    {
                        GB_Status_AppendText_Nextline("Flash(3) OK", Color.Green);
                        IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                        IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");
                        
                        IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                        IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x04 0x00 0x00");
                        Thread.Sleep(100);
                        if (DP150_Flash_Write_Verify())
                        {
                            GB_Status_AppendText_Nextline("Flash(4) OK", Color.Green);
                            IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                            IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");
                            
                            IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                            IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x02 0x00 0x00");
                            Thread.Sleep(100);
                            if (DP150_Flash_Write_Verify())
                            {
                                GB_Status_AppendText_Nextline("Flash(5) OK", Color.Green);
                                IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");
                                
                                IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x01 0x00 0x00");
                                Thread.Sleep(100);
                                if (DP150_Flash_Write_Verify())
                                {
                                    GB_Status_AppendText_Nextline("Flash(6) OK", Color.Green);
                                    IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                    IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");
                                    
                                    IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                    IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x04 0x00");
                                    Thread.Sleep(100);
                                    if (DP150_Flash_Write_Verify())
                                    {
                                        GB_Status_AppendText_Nextline("Flash(7) OK", Color.Green);
                                        IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                        IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");
                                        
                                        IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                        IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x02 0x00");
                                        Thread.Sleep(100);
                                        if (DP150_Flash_Write_Verify())
                                        {
                                            GB_Status_AppendText_Nextline("Flash(8) OK", Color.Green);
                                            IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                            IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");
                                            
                                            IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                            IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x55 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x01 0x00");
                                            Thread.Sleep(100);
                                            if (DP150_Flash_Write_Verify())
                                            {
                                                GB_Status_AppendText_Nextline("Flash(9) OK", Color.Green);
                                                IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
                                                IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");

                                                IPC_Quick_Send("mipi.write 0x15 0x59 0x02");

                                                System.Windows.Forms.MessageBox.Show("Flash Writing Is Successfully Completed");
                                            }
                                            else
                                                System.Windows.Forms.MessageBox.Show("Flash Write Fail(9)");
                                        }
                                        else
                                            System.Windows.Forms.MessageBox.Show("Flash Write Fail(8)");
                                    }
                                    else
                                        System.Windows.Forms.MessageBox.Show("Flash Write Fail(7)");
                                }
                                else
                                    System.Windows.Forms.MessageBox.Show("Flash Write Fail(6)");
                            }
                            else
                                System.Windows.Forms.MessageBox.Show("Flash Write Fail(5)");
                        }
                        else
                            System.Windows.Forms.MessageBox.Show("Flash Write Fail(4)");
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Flash Write Fail(3)");
                }
                else
                    System.Windows.Forms.MessageBox.Show("Flash Write Fail(2)");
            }
            else
                System.Windows.Forms.MessageBox.Show("Flash Write Fail(1)");   
            GB_Status_AppendText_Nextline("Flash Writing Finished", Color.Blue);
            button_DP150_Flash_Erase.Enabled = true;
            System.Windows.Forms.Application.DoEvents();
         }

        private bool DP150_Flash_Write_Verify()
        {
            if (checkBox_DP150_Flash_Write_Verify_Skip.Checked) return true;
            else
            {
                Thread.Sleep(50);
                System.Windows.Forms.Application.DoEvents();      
                this.OTP_Read(3, "DD");
                Thread.Sleep(50);
                
                string DD_Param = dataGridView1.Rows[2].Cells[1].Value.ToString();
                int DD_1st_bit = (Convert.ToInt16(DD_Param, 16) >> 7);

                if (DD_1st_bit == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        private void button_DP150_Flash_Erase_Click(object sender, EventArgs e)
        {
            GB_Status_AppendText_Nextline("Flash Erase Started", Color.Blue);
            button_DP150_Flash_Write.Enabled = false;
            System.Windows.Forms.Application.DoEvents();

            IPC_Quick_Send("mipi.dsi 4 800 burst continuos dsc frc");
            
            //IPC_Quick_Send("mipi.write 0x15 0xB0 0xBC");
            //IPC_Quick_Send("mipi.write 0x39 0xE7 0x02");
            Magnachip_B0_CMD_Sending("mipi.write 0x15 0xB0 0xBC", "mipi.write 0x39 0xE7 0x02");

            IPC_Quick_Send("mipi.write 0x15 0x59 0x03");

            //IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
            //IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0xAA 0x06 0x00 0x00 0x00 0x01");
            Magnachip_B0_CMD_Sending("mipi.write.hs 0x39 0xB0 0x06", "mipi.write.hs 0x39 0xE1 0xAA 0x06 0x00 0x00 0x00 0x01");

            //IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
            //IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00");
            Magnachip_B0_CMD_Sending("mipi.write.hs 0x39 0xB0 0x06", "mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00");

            //IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
            //IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0xAA 0x52 0x00 0x00 0x00 0x01");
            Magnachip_B0_CMD_Sending("mipi.write.hs 0x39 0xB0 0x06","mipi.write.hs 0x39 0xE1 0xAA 0x52 0x00 0x00 0x00 0x01");

            Thread.Sleep(200);

            //IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
            //IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00");
            Magnachip_B0_CMD_Sending("mipi.write.hs 0x39 0xB0 0x06", "mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00");

            //IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
            //IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0xAA 0x05 0x00 0x00 0x00 0x01");
            Magnachip_B0_CMD_Sending("mipi.write.hs 0x39 0xB0 0x06", "mipi.write.hs 0x39 0xE1 0xAA 0x05 0x00 0x00 0x00 0x01");

            Thread.Sleep(10);

            //IPC_Quick_Send("mipi.write.hs 0x39 0xB0 0x06");
            //IPC_Quick_Send("mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00");
            Magnachip_B0_CMD_Sending("mipi.write.hs 0x39 0xB0 0x06", "mipi.write.hs 0x39 0xE1 0x00 0x00 0x00 0x00 0x00 0x00");

            Thread.Sleep(10);

            IPC_Quick_Send("mipi.write 0x15 0x59 0x02");

            GB_Status_AppendText_Nextline("Flash Erase Finished", Color.Blue);
            button_DP150_Flash_Write.Enabled = true;
            System.Windows.Forms.Application.DoEvents();
        }

        private void min_to_max_rb_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void button_60Hz_Click(object sender, EventArgs e)
        {
            GB_Status_AppendText_Nextline("60Hz Set1", Color.Blue);
            //IPC_Quick_Send("mipi.write 0x39 0x7F 0x5A 0x5A");
            //IPC_Quick_Send("mipi.write 0x39 0x76 0x00");
            IPC_Quick_Send("mipi.write 0x39 0x76 0x00");
            IPC_Quick_Send("mipi.write 0x15 0xB0 0x8C");
            IPC_Quick_Send("mipi.write 0x39 0xE7 0x00");
        }

        private void button_90Hz_Click(object sender, EventArgs e)
        {
            GB_Status_AppendText_Nextline("90Hz Set2", Color.Red);
            //IPC_Quick_Send("mipi.write 0x39 0x7F 0x5A 0x5A");
            //IPC_Quick_Send("mipi.write 0x39 0x76 0x01");
            IPC_Quick_Send("mipi.write 0x39 0x76 0x01");
            IPC_Quick_Send("mipi.write 0x15 0xB0 0x92");
            IPC_Quick_Send("mipi.write 0x39 0xE7 0x01");
        }

        private void textBox_BCS_Max_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_AOD_In_Click(object sender, EventArgs e)
        {
            if(current_model.Get_Current_Model_Name() == Model_Name.DP150)
            {
                IPC_Quick_Send("mipi.write 0x39 0x7F 0x5A 0x5A");
                IPC_Quick_Send("mipi.write 0x39 0xF0 0x5A 0x5A");
                IPC_Quick_Send("mipi.write 0x39 0xF1 0x5A 0x5A");
                IPC_Quick_Send("mipi.write 0x39 0xF2 0x5A 0x5A");
                IPC_Quick_Send("mipi.write 0x05 0x28");
                
                IPC_Quick_Send("mipi.write 0x15 0xB0 0x3F");
                IPC_Quick_Send("mipi.write 0x39 0xE0 0x3C 0x00 0x00 0x51");

                IPC_Quick_Send("mipi.write 0x39 0x51 0x07 0xFF");
                IPC_Quick_Send("mipi.write 0x05 0x39");
                IPC_Quick_Send("delay 10");
                IPC_Quick_Send("mipi.write 0x05 0x29");
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin
                || current_model.Get_Current_Model_Name() == Model_Name.DP213)
            {
                IPC_Quick_Send("mipi.write 0x05 0x39");
            }
            else
            {
                IPC_Quick_Send("ipc.file.send " + this.AOD_In_Path);
            }

            int max_line = Convert.ToInt32(textBox_GB_Status_Max_Line.Text);
            if (GB_Lines > max_line) button_Clear_Status_Text.PerformClick();
            GB_Lines++;
            RichTextBox_GB_Status.AppendText("AOD In\r\n");
        }

        private void button_AOD_Out_Click(object sender, EventArgs e)
        {
            if (current_model.Get_Current_Model_Name() == Model_Name.DP150)
            {
                IPC_Quick_Send("mipi.write 0x39 0x7F 0x5A 0x5A");
                IPC_Quick_Send("mipi.write 0x05 0x28");
                IPC_Quick_Send("mipi.write 0x39 0x51 0x06 0xED");
                IPC_Quick_Send("mipi.write 0x05 0x38");
                IPC_Quick_Send("delay 10");
                IPC_Quick_Send("mipi.write 0x05 0x13");
                IPC_Quick_Send("mipi.write 0x05 0x29");
            }
            else if (current_model.Get_Current_Model_Name() == Model_Name.DP173 || current_model.Get_Current_Model_Name() == Model_Name.Elgin
                 || current_model.Get_Current_Model_Name() == Model_Name.DP213)
            {
                IPC_Quick_Send("mipi.write 0x05 0x38");
            }
            else
            {
                IPC_Quick_Send("ipc.file.send " + this.AOD_Out_Path);
            }

            int max_line = Convert.ToInt32(textBox_GB_Status_Max_Line.Text);
            if (GB_Lines > max_line) button_Clear_Status_Text.PerformClick();
            GB_Lines++;
            RichTextBox_GB_Status.AppendText("AOD Out\r\n");
        }

        private void imageAnalizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BMP_Image_Processing_Form.getInstance().Visible = false;
            BMP_Image_Processing_Form.getInstance().Show();
        }

        private void CA_Sync_Mode()
        {
            //Test Sync mode
            if (radioButton_NTSC.Checked)
            {
                if (objCa.SyncMode != 0) GB_Status_AppendText_Nextline("CA NTSC Sync Mode Is Selected", Color.Blue);
                label19.Text = "Color coordinate standard : NTSC";
                objCa.SyncMode = 0; //NTSC
            }
            else if (radioButton_PAL.Checked)
            {
                if (objCa.SyncMode != 1) GB_Status_AppendText_Nextline("CA PAL Sync Mode Is Selected", Color.Blue);
                label19.Text = "Color coordinate standard : PAL";
                objCa.SyncMode = 1;
            }
            else if (radioButton_EXT.Checked)
            {
                try
                {
                    if (objCa.SyncMode != 2) GB_Status_AppendText_Nextline("CA EXT Sync Mode Is Selected", Color.Blue);
                    label19.Text = "Color coordinate standard : EXT";
                    objCa.SyncMode = 2;     
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    System.Windows.Forms.MessageBox.Show("Please Input an external synchronization signal");
                    radioButton_NTSC.Checked = true;
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("EXT Mode Error");
                    radioButton_NTSC.Checked = true;
                }
            }
            else if (radioButton_UNIV.Checked)
            {
                if (objCa.SyncMode != 3) GB_Status_AppendText_Nextline("CA UNIV Sync Mode Is Selected", Color.Blue);
                label19.Text = "Color coordinate standard : UNIV";
                objCa.SyncMode = 3;  
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("CA Sync mode Error");
            }
        }


        private void radioButton_NTSC_CheckedChanged(object sender, EventArgs e)
        {
            CA_Sync_Mode();
        }

        private void radioButton_PAL_CheckedChanged(object sender, EventArgs e)
        {
            CA_Sync_Mode();
        }

        private void radioButton_EXT_CheckedChanged(object sender, EventArgs e)
        {
            CA_Sync_Mode();
        }

        private void radioButton_UNIV_CheckedChanged(object sender, EventArgs e)
        {
            CA_Sync_Mode();
            
        }


        private void CA_Measure_Mode()
        {
            //Test Display mode
            if (radioButton_CA_Measure_Auto.Checked)
            {
                if (objCa.AveragingMode != 2) GB_Status_AppendText_Nextline("CA Auto Measure Mode Is Selected", Color.Blue);
                objCa.AveragingMode = 2; //Auto 
            }
            else if (radioButton_CA_Measure_Fast.Checked)
            {
                if (objCa.AveragingMode != 1) GB_Status_AppendText_Nextline("CA Fast Measure Mode Is Selected", Color.Blue);
                objCa.AveragingMode = 1; //Fast         
            }
            else if (radioButton_CA_Measure_Slow.Checked)
            {
                if (objCa.AveragingMode != 0) GB_Status_AppendText_Nextline("CA Slow Measure Mode Is Selected", Color.Blue);
                objCa.AveragingMode = 0; //Slow
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("CA Measure mode Error");
            }
        }

        private void radioButton_CA_Measure_Auto_CheckedChanged(object sender, EventArgs e)
        {
            CA_Measure_Mode();
        }

        private void radioButton_CA_Measure_Fast_CheckedChanged(object sender, EventArgs e)
        {
            CA_Measure_Mode();
        }

        private void radioButton_CA_Measure_Slow_CheckedChanged(object sender, EventArgs e)
        {
            CA_Measure_Mode();
        }

        private void radioButton_PNC_ACK_Fast_CheckedChanged(object sender, EventArgs e)
        {
            PNC_ACK_Sleep = 10;
            PNC_ACK_Loop_Max = 500 * 3;
        }

        private void radioButton_PNC_ACK_Normal_CheckedChanged(object sender, EventArgs e)
        {
            PNC_ACK_Sleep = 50;
            PNC_ACK_Loop_Max = 100 * 3;
        }

        private void radioButton_PNC_ACK_Slow_CheckedChanged(object sender, EventArgs e)
        {        
            PNC_ACK_Sleep = 100;
            PNC_ACK_Loop_Max = 50 * 3;
        }

        public void Meta_Form_Show()
        {
            Meta_Model_Option_Form.getInstance().Show();
        }

       

        private void MultiBoardControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PNC_10ch_Board_Ctrl.getInstance().Visible = false;
            PNC_10ch_Board_Ctrl.getInstance().Show();
        }



        private void button_APL_Measure_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 10; i++)
            {
                trackBar_APL.Value = i;
                Thread.Sleep(300);
                CA_Measure_For_APL((i * 10).ToString() + "%");
            }
        }

        public void CA_Measure_For_APL(string APL)
        {
            //objCa.DisplayMode = 0; // 측정 모드는 xyLv
            //objCa.SyncMode = 0; //측정 모드는 NTSC

            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView2.Columns[0].HeaderText = "APL";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

            int i;
            try
            {
                isMsr = true;

                CA_Measure_button.Enabled = false;
                CA_zero_cal_button.Enabled = false;
                for (i = 0; i < 1; i++)
                {
                    objCa.Measure();


                    label6.Text = "x";
                    label7.Text = "y";
                    label8.Text = "Lv";
                    X_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

                    System.Windows.Forms.Application.DoEvents();

                    if (isMsr == false)
                    {
                        break;
                    }
                }

                CA_Measure_button.Enabled = true;
                CA_zero_cal_button.Enabled = true;

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)


                //Add data to Datagridview
                dataGridView2.Rows.Add(APL, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            catch (Exception er)
            {
                DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void X_Value_display_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Y_Value_display_TextChanged(object sender, EventArgs e)
        {

        }

        private void Lv_Value_display_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dBVAccuracyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBV_Accuracy.getInstance().Visible = false;
            DBV_Accuracy.getInstance().Show();
        }

        private void osiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button_DBV_Accuracy_20191217_Click(object sender, EventArgs e)
        {
            G255_Display.PerformClick();//White Pattern

            step_value_1.Checked = true; //1step
            max_to_min_rb.Checked = true; //max to min DBV
            GCS_BCS_Delay_Textbox.Text = "16";//delay 16ms

            //First 2nit ~ 600nit
            dataGridView2.Rows.Add("1st", "-", "-", "-");
            radioButton_Ave_BCS_GCS_1.Checked = true;
            textBox_BCS_Max.Text = "7FE";
            textBox_BCS_Min.Text = "1";
            BCS_Meas_Without_Delete_Gridview();

            //Second
            dataGridView2.Rows.Add("2nd", "-", "-", "-");
            radioButton_Ave_BCS_GCS_5.Checked = true;
            textBox_BCS_Max.Text = "1BC";
            BCS_Meas_Without_Delete_Gridview();

            //Third (Second);
            dataGridView2.Rows.Add("3rd", "-", "-", "-");
            BCS_Meas_Without_Delete_Gridview();
        }

        private void Display_Pattern_And_60Hz_90Hz_Measure(int gray,int delay)
        {
            PTN_update(gray, gray, gray);
            button_60Hz.PerformClick(); System.Windows.Forms.Application.DoEvents();
            Thread.Sleep(delay); System.Windows.Forms.Application.DoEvents();
            CA_Measure_60hz_90hz(gray, "60hz", true); System.Windows.Forms.Application.DoEvents();
            button_90Hz.PerformClick(); System.Windows.Forms.Application.DoEvents();
            Thread.Sleep(delay); System.Windows.Forms.Application.DoEvents();
            CA_Measure_60hz_90hz(gray, "90hz", false); System.Windows.Forms.Application.DoEvents();
        }

        private void Add_Info_to_GridView(string temp = "123")
        {
            Dual_SH_Delta_E obj_dual_sh_delta_e = Dual_SH_Delta_E.getInstance();
            obj_dual_sh_delta_e.dataGridView1.Rows.Add(temp, "-", "-", "-");
            obj_dual_sh_delta_e.dataGridView2.Rows.Add(temp, "-", "-", "-");
        }

        

        private void dP173ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model_dp173_update();
        }

        public void model_dp173_update()
        {
            current_model.Set_Model_As_DP173();
            current_model.OC_Param_load();
            this.Model_Setting();
        }


        private void groupBox23_Enter(object sender, EventArgs e)
        {

        }

        public void Set_And_2D_Drawing_Target_Measure_Limit_XYLv(XYLv Target, XYLv Measure, XYLv Limit, XYLv Extension)
        {
            Target.String_Update_From_Double();
            Measure.String_Update_From_Double();
            Limit.String_Update_From_Double();
            Extension.String_Update_From_Double();

            chart1.Series.Clear();

            chart1.ChartAreas[0].AxisX.Title = "X";
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.Maximum = Target.double_X + (Limit.double_X * 20);
            chart1.ChartAreas[0].AxisX.Minimum = Target.double_X - (Limit.double_X * 20);
            chart1.ChartAreas[0].AxisX.Interval = (Limit.double_X * 2);

            chart1.ChartAreas[0].AxisY.Title = "Y";
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.Maximum = Target.double_Y + (Limit.double_Y * 20);
            chart1.ChartAreas[0].AxisY.Minimum = Target.double_Y - (Limit.double_Y * 20);
            chart1.ChartAreas[0].AxisY.Interval = (Limit.double_Y * 2);
            //-------------------------

            //----chart2 inializing---
            chart2.Series.Clear();

            chart2.ChartAreas[0].AxisX.Title = "-";
            chart2.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart2.ChartAreas[0].AxisX.Maximum = 2;
            chart2.ChartAreas[0].AxisX.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Interval = 0;
            chart2.ChartAreas[0].AxisX.IntervalOffset = 1;

            chart2.ChartAreas[0].AxisY.Title = "Lv";
            chart2.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            if (Target.double_Lv > 400)
            {
                chart2.ChartAreas[0].AxisY.Maximum = Target.double_Lv + (Limit.double_Lv * 40);
                chart2.ChartAreas[0].AxisY.Minimum = Target.double_Lv - (Limit.double_Lv * 40);
                chart2.ChartAreas[0].AxisY.Interval = (Limit.double_Lv * 4);
            }
            else if (Target.double_Lv > 300)
            {
                chart2.ChartAreas[0].AxisY.Maximum = Target.double_Lv + (Limit.double_Lv * 30);
                chart2.ChartAreas[0].AxisY.Minimum = Target.double_Lv - (Limit.double_Lv * 30);
                chart2.ChartAreas[0].AxisY.Interval = (Limit.double_Lv * 3);
            }
            else if (Target.double_Lv > 200)
            {
                chart2.ChartAreas[0].AxisY.Maximum = Target.double_Lv + (Limit.double_Lv * 20);
                chart2.ChartAreas[0].AxisY.Minimum = Target.double_Lv - (Limit.double_Lv * 20);
                chart2.ChartAreas[0].AxisY.Interval = (Limit.double_Lv * 2);
            }
            else
            {
                chart2.ChartAreas[0].AxisY.Maximum = Target.double_Lv + (Limit.double_Lv * 10);
                chart2.ChartAreas[0].AxisY.Minimum = Target.double_Lv - (Limit.double_Lv * 10);
                chart2.ChartAreas[0].AxisY.Interval = (Limit.double_Lv * 1);
            }
            //-------------------------

            //----chart1 Add Point-----
            if (chart1.Series.IsUniqueName("Target")) chart1.Series.Add("Target");
            chart1.Series["Target"].Points.Clear();
            chart1.Series["Target"].ChartType = SeriesChartType.Point;
            chart1.Series["Target"].Color = Color.Blue;
            chart1.Series["Target"].MarkerSize = 4;
            chart1.Series["Target"].Points.AddXY(Target.double_X, Target.double_Y);

            if (chart1.Series.IsUniqueName("Measure")) chart1.Series.Add("Measure");
            chart1.Series["Measure"].Points.Clear();
            chart1.Series["Measure"].ChartType = SeriesChartType.Point;
            chart1.Series["Measure"].Color = Color.Black;
            chart1.Series["Measure"].MarkerSize = 4;
            chart1.Series["Measure"].Points.AddXY(Measure.double_X, Measure.double_Y);

            if ((Math.Abs(Target.double_X - Measure.double_X) < Limit.double_X) && (Math.Abs(Target.double_Y - Measure.double_Y) < Limit.double_Y)) chart1.Series["Measure"].Points[0].Color = Color.FromArgb(0, 255, 0);
            else if ((Math.Abs(Target.double_X - Measure.double_X) < (Limit.double_X + Extension.double_X)) && (Math.Abs(Target.double_Y - Measure.double_Y) < (Limit.double_Y + Extension.double_Y))) chart1.Series["Measure"].Points[0].Color = Color.FromArgb(0, 200, 0);
            else if ((Math.Abs(Target.double_X - Measure.double_X) < (Limit.double_X * 3)) && (Math.Abs(Target.double_Y - Measure.double_Y) < (Limit.double_Y * 3))) chart1.Series["Measure"].Points[0].Color = Color.FromArgb(0, 150, 0);
            else chart1.Series["Measure"].Points[0].Color = Color.FromArgb(255, 0, 0);
            //-------------------------

            //----chart2 Add Point-----
            if (chart2.Series.IsUniqueName("Target")) chart2.Series.Add("Target");
            chart2.Series["Target"].Points.Clear();
            chart2.Series["Target"].ChartType = SeriesChartType.Point;
            chart2.Series["Target"].Color = Color.Blue;
            chart2.Series["Target"].MarkerSize = 4;
            chart2.Series["Target"].Points.AddXY(1, Target.double_Lv);

            if (chart2.Series.IsUniqueName("Measure")) chart2.Series.Add("Measure");
            chart2.Series["Measure"].Points.Clear();
            chart2.Series["Measure"].ChartType = SeriesChartType.Point;
            chart2.Series["Measure"].Color = Color.Black;
            chart2.Series["Measure"].MarkerSize = 4;
            chart2.Series["Measure"].Points.AddXY(1, Measure.double_Lv);

            if ((Math.Abs(Target.double_Lv - Measure.double_Lv) < Limit.double_Lv)) chart2.Series["Measure"].Points[0].Color = Color.FromArgb(0, 255, 0);
            else if ((Math.Abs(Target.double_Lv - Measure.double_Lv) < (Limit.double_Lv * 3))) chart2.Series["Measure"].Points[0].Color = Color.FromArgb(0, 150, 0);
            else chart2.Series["Measure"].Points[0].Color = Color.FromArgb(255, 0, 0);
            //-------------------------

            //clear chart1
            chart1.ChartAreas[0].AxisX.StripLines.Clear();
            chart1.ChartAreas[0].AxisY.StripLines.Clear();
            //----chart1 Limit----
            //X

            StripLine stripline = new StripLine();
            stripline.Interval = 0;
            stripline.StripWidth = 0.0001;
            stripline.BackColor = Color.Black;
            stripline.IntervalOffset = Target.double_X + Limit.double_X;
            chart1.ChartAreas[0].AxisX.StripLines.Add(stripline);

            StripLine stripline2 = new StripLine();
            stripline2.Interval = 0;
            stripline2.StripWidth = 0.0001;
            stripline2.BackColor = Color.Black;
            stripline2.IntervalOffset = Target.double_X - Limit.double_X;
            chart1.ChartAreas[0].AxisX.StripLines.Add(stripline2);

            //Y
            StripLine stripline3 = new StripLine();
            stripline3.Interval = 0;
            stripline3.StripWidth = 0.0001;
            stripline3.BackColor = Color.Black;
            stripline3.IntervalOffset = Target.double_Y + Limit.double_Y;
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline3);

            StripLine stripline4 = new StripLine();
            stripline4.Interval = 0;
            stripline4.StripWidth = 0.0001;
            stripline4.BackColor = Color.Black;
            stripline4.IntervalOffset = Target.double_Y - Limit.double_Y;
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline4);
            //-------------------------

            //----chart1 (Limit * 2)----
            //X
            StripLine striplin5 = new StripLine();
            striplin5.Interval = 0;
            striplin5.StripWidth = 0.0001;
            striplin5.BackColor = Color.Gray;
            striplin5.IntervalOffset = Target.double_X + (Limit.double_X * 2);
            chart1.ChartAreas[0].AxisX.StripLines.Add(striplin5);

            StripLine stripline6 = new StripLine();
            stripline6.Interval = 0;
            stripline6.StripWidth = 0.0001;
            stripline6.BackColor = Color.Gray;
            stripline6.IntervalOffset = Target.double_X - (Limit.double_X * 2);
            chart1.ChartAreas[0].AxisX.StripLines.Add(stripline6);

            //Y
            StripLine stripline7 = new StripLine();
            stripline7.Interval = 0;
            stripline7.StripWidth = 0.0001;
            stripline7.BackColor = Color.Gray;
            stripline7.IntervalOffset = Target.double_Y + (Limit.double_Y * 2);
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline7);

            StripLine stripline8 = new StripLine();
            stripline8.Interval = 0;
            stripline8.StripWidth = 0.0001;
            stripline8.BackColor = Color.Gray;
            stripline8.IntervalOffset = Target.double_Y - (Limit.double_Y * 2);
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline8);
            //-------------------------

            //----chart1 (Limit * 3)----
            //X
            StripLine stripline_3 = new StripLine();
            stripline_3.Interval = 0;
            stripline_3.StripWidth = 0.0001;
            stripline_3.BackColor = Color.LightGray;
            stripline_3.IntervalOffset = (Target.double_X + (Limit.double_X * 3));
            chart1.ChartAreas[0].AxisX.StripLines.Add(stripline_3);

            StripLine stripline2_3 = new StripLine();
            stripline2_3.Interval = 0;
            stripline2_3.StripWidth = 0.0001;
            stripline2_3.BackColor = Color.LightGray;
            stripline2_3.IntervalOffset = (Target.double_X - (Limit.double_X * 3));
            chart1.ChartAreas[0].AxisX.StripLines.Add(stripline2_3);

            //Y
            StripLine stripline3_3 = new StripLine();
            stripline3_3.Interval = 0;
            stripline3_3.StripWidth = 0.0001;
            stripline3_3.BackColor = Color.LightGray;
            stripline3_3.IntervalOffset = (Target.double_Y + (Limit.double_Y * 3));
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline3_3);

            StripLine stripline4_3 = new StripLine();
            stripline4_3.Interval = 0;
            stripline4_3.StripWidth = 0.0001;
            stripline4_3.BackColor = Color.LightGray;
            stripline4_3.IntervalOffset = (Target.double_Y - (Limit.double_Y * 3));
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline4_3);
            //-------------------------

            //Clear chart2 
            chart2.ChartAreas[0].AxisY.StripLines.Clear();
            //----chart2 Limit----
            //Y
            StripLine stripline_Lv_max = new StripLine();
            stripline_Lv_max.Interval = 0;
            stripline_Lv_max.StripWidth = (Limit.double_Lv / 10000.0);
            stripline_Lv_max.BackColor = Color.Black;
            stripline_Lv_max.IntervalOffset = Target.double_Lv + Limit.double_Lv;
            chart2.ChartAreas[0].AxisY.StripLines.Add(stripline_Lv_max);

            StripLine stripline_Lv_min = new StripLine();
            stripline_Lv_min.Interval = 0;
            stripline_Lv_min.StripWidth = (Limit.double_Lv / 10000.0);
            stripline_Lv_min.BackColor = Color.Black;
            stripline_Lv_min.IntervalOffset = Target.double_Lv - Limit.double_Lv;
            chart2.ChartAreas[0].AxisY.StripLines.Add(stripline_Lv_min);
            //-------------------------


            //----chart2 (Limit * 2)----
            //Y
            StripLine stripline_Lv_max_2 = new StripLine();
            stripline_Lv_max_2.Interval = 0;
            stripline_Lv_max_2.StripWidth = (Limit.double_Lv / 10000.0);
            stripline_Lv_max_2.BackColor = Color.Gray;
            stripline_Lv_max_2.IntervalOffset = Target.double_Lv + (Limit.double_Lv * 2);
            chart2.ChartAreas[0].AxisY.StripLines.Add(stripline_Lv_max_2);

            StripLine stripline_Lv_min_2 = new StripLine();
            stripline_Lv_min_2.Interval = 0;
            stripline_Lv_min_2.StripWidth = (Limit.double_Lv / 10000.0);
            stripline_Lv_min_2.BackColor = Color.Gray;
            stripline_Lv_min_2.IntervalOffset = Target.double_Lv - (Limit.double_Lv * 2);
            chart2.ChartAreas[0].AxisY.StripLines.Add(stripline_Lv_min_2);
            //-------------------------

            //----chart2 (Limit * 3)----
            //Y
            StripLine stripline_Lv_max_3 = new StripLine();
            stripline_Lv_max_3.Interval = 0;
            stripline_Lv_max_3.StripWidth = (Limit.double_Lv / 10000.0);
            stripline_Lv_max_3.BackColor = Color.LightGray;
            stripline_Lv_max_3.IntervalOffset = Target.double_Lv + (Limit.double_Lv * 3);
            chart2.ChartAreas[0].AxisY.StripLines.Add(stripline_Lv_max_3);

            StripLine stripline_Lv_min_3 = new StripLine();
            stripline_Lv_min_3.Interval = 0;
            stripline_Lv_min_3.StripWidth = (Limit.double_Lv / 10000.0);
            stripline_Lv_min_3.BackColor = Color.LightGray;
            stripline_Lv_min_3.IntervalOffset = Target.double_Lv - (Limit.double_Lv * 3);
            chart2.ChartAreas[0].AxisY.StripLines.Add(stripline_Lv_min_3);
        }

        private void chart1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //This gives the corresponding X and Y coordinates of the mouse point.
            Point pos = e.Location;
            if (Previouse_Pos.X == pos.X && Previouse_Pos.Y == pos.Y) return;

            toolTip1.RemoveAll();
            var results = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    double xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                    double yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);
                    xVal = Math.Round(xVal, 5);
                    yVal = Math.Round(yVal, 5);
                    toolTip1.Show("(X,Y):(" + xVal.ToString() + "," + yVal.ToString() + ")", chart1, pos.X, pos.Y);
                }
            }
            Previouse_Pos.X = pos.X;
            Previouse_Pos.Y = pos.Y;
        }


        private void groupBox_1st_Model_OTP_Check_Enter(object sender, EventArgs e)
        {

        }

        private void practiceCodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Practice_Coding.getInstance().Visible = false;
            Practice_Coding.getInstance().Show();
        }

        private void elginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model_elgin_update();
        }

        public void model_elgin_update()
        {
            current_model.Set_Model_As_Elgin();
            this.Model_Setting();
        }

        private void mipiCMDCtrlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MIPI_CMD_Ctrl_Form.getInstance().Visible = false;
            MIPI_CMD_Ctrl_Form.getInstance().Show();
        }

        private void mipiReadAndCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OTP_Read_and_Compare.getInstance().Visible = false;
            OTP_Read_and_Compare.getInstance().Show();
        }

        private void mipiSciprConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mipi_Script_Convertor.getInstance().Visible = false;
            Mipi_Script_Convertor.getInstance().Show();
        }

        private void opticMeasurementToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Is_Single_CA_is_connected == true)
            {
                Optic_Measurement_Form.getInstance().Visible = false;
                Optic_Measurement_Form.getInstance().Show();
                Optic_Measurement_Form.getInstance().Set_IChannel(new Single_Channel());
            }
            else
            {
                MessageBox.Show("Please Connect Single CA First");
            }
        }


        private void dP213ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model_dp213_update();
        }

        public void model_dp213_update()
        {
            current_model.Set_Model_As_DP213();
            this.Model_Setting();
        }


        public string[] Get_Read_Hex_String_Array(int Param_Amount)
        {
            string[] String_Hex = new string[Param_Amount];
            for (int i = 0; i < Param_Amount; i++) String_Hex[i] = dataGridView1.Rows[i].Cells[1].Value.ToString();
            return String_Hex;
        }

        public byte[] Get_Read_Byte_Array(int param_amount)
        {
            string[] Hex_Read_Params = Get_Read_Hex_String_Array(param_amount);
            byte[] Byte_Read_Params = new byte[param_amount];
            for (int i = 0; i < param_amount; i++) Byte_Read_Params[i] = Convert.ToByte(Hex_Read_Params[i], 16);
            return Byte_Read_Params;
        }


        private void Show_Instance_Status(string Name, bool IsIstanceNull)
        {
            if (IsIstanceNull)
                GB_Status_AppendText_Nextline("Is " + Name + "null ? " + IsIstanceNull.ToString(), Color.Blue);
            else
                GB_Status_AppendText_Nextline("Is " + Name + "null ? " + IsIstanceNull.ToString(), Color.Red);
        }

        private void button_Show_Created_Form_Objects_Click(object sender, EventArgs e)
        {
            GB_Status_AppendText_Nextline("--DP086 or DP116--", Color.Black);
            Show_Instance_Status("First_Model_Option_Form", First_Model_Option_Form.IsIstanceNull());
            Show_Instance_Status("Engineer_Mornitoring_Mode", Engineer_Mornitoring_Mode.IsIstanceNull());
            Show_Instance_Status("Dual_Engineer_Monitoring_Mode", Dual_Engineer_Monitoring_Mode.IsIstanceNull());

            GB_Status_AppendText_Nextline("--DP150--", Color.Black);
            Show_Instance_Status("Second_Model_Option_Form", Second_Model_Option_Form.IsIstanceNull());
            Show_Instance_Status("DP150_Single_Engineerig_Mornitoring_Mode", DP150_Single_Engineerig_Mornitoring_Mode.IsIstanceNull());
            Show_Instance_Status("DP150_Dual_Engineering_Mornitoring_Mode", DP150_Dual_Engineering_Mornitoring_Mode.IsIstanceNull());

            GB_Status_AppendText_Nextline("--Meta--", Color.Black);
            Show_Instance_Status("Meta_Model_Option_Form", Meta_Model_Option_Form.IsIstanceNull());
            Show_Instance_Status("Meta_Engineer_Mornitoring_Mode", Meta_Engineer_Mornitoring_Mode.IsIstanceNull());

            GB_Status_AppendText_Nextline("--DP173 or Elgin--", Color.Black);
            Show_Instance_Status("DP173_Model_Option_Form", DP173_Model_Option_Form.IsIstanceNull());
            Show_Instance_Status("DP173_Single_Engineering_Mornitoring", DP173_Single_Engineering_Mornitoring.IsIstanceNull());
            Show_Instance_Status("DP173_Dual_Engineering_Mornitoring", DP173_Dual_Engineering_Mornitoring.IsIstanceNull());

            GB_Status_AppendText_Nextline("--DP213--", Color.Black);
            Show_Instance_Status("DP213_Model_Option_Form", DP213_Model_Option_Form.IsIstanceNull());
            Show_Instance_Status("DP213_Dual_Engineering_Mornitoring", DP213_Dual_Engineering_Mornitoring.IsIstanceNull());

            GB_Status_AppendText_Nextline("--BMP--", Color.Black);
            Show_Instance_Status("BMP_Image_Processing_Form", BMP_Image_Processing_Form.IsIstanceNull());

            GB_Status_AppendText_Nextline("--Practice--", Color.Black);
            Show_Instance_Status("Practice_Coding", Practice_Coding.IsIstanceNull());
            Show_Instance_Status("CPP_Dll_Test_Form", CPP_Dll_Test_Form.IsIstanceNull());

            GB_Status_AppendText_Nextline("--Measurement--", Color.Black);
            Show_Instance_Status("Dual_SH_Delta_E", Dual_SH_Delta_E.IsIstanceNull());
            Show_Instance_Status("SH_Delta_E", SH_Delta_E.IsIstanceNull());
            Show_Instance_Status("Optic_Measurement_Form", Optic_Measurement_Form.IsIstanceNull());
            Show_Instance_Status("CA_Multi_Channel_Form ", CA_Multi_Channel_Form.IsIstanceNull());
            Show_Instance_Status("Optic_Measurement_10ch", Optic_Measurement_10ch.IsIstanceNull());
            Show_Instance_Status("DBV_Accuracy", DBV_Accuracy.IsIstanceNull());

            GB_Status_AppendText_Nextline("--CommonUse--", Color.Black);
            Show_Instance_Status("MIPI_CMD_Ctrl_Form", MIPI_CMD_Ctrl_Form.IsIstanceNull());
            Show_Instance_Status("Mipi_Script_Convertor", Mipi_Script_Convertor.IsIstanceNull());
            Show_Instance_Status("OTP_Read_and_Compare", OTP_Read_and_Compare.IsIstanceNull());
            Show_Instance_Status("PNC_10ch_Board_Ctrl", PNC_10ch_Board_Ctrl.IsIstanceNull());
        }

        private void button_Delete_AllSubFormObjects_Click(object sender, EventArgs e)
        {
            DeleteAllSubFormObjects();
        }

        private void button_Test_AnyThing_Click(object sender, EventArgs e)
        {

        }

      

   


        private void button_CA_Multichannel_Click(object sender, EventArgs e)
        {
            CA_Multi_Channel_Form.getInstance().Visible = false;
            CA_Multi_Channel_Form.getInstance().Show();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}

