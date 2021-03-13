////using References
using BSQH_Csharp_Library;
using PNC_Csharp.Measurement_10ch;
using PNC_Csharp.Measurement_QA;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PNC_Csharp.CA_Multi_Channels
{
    public partial class CA_Multi_Channel_Form : Form
    {
        //RichTextbox Variables
        int Status_Line_Num;
        const int max_Status_line = 10000;

        Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        const int max_port_num = 2;
        const int max_port_probe_num = 5;
        I_CA_Control ca_ctrl_obj;
        I_CA310_Control ca310_ctrl_obj;
        I_CA410_Control ca410_ctrl_obj;
        bool Is_CA_connected;

        public CA_Multi_Channel_Form()
        {
            InitializeComponent();
        }


        private static CA_Multi_Channel_Form Instance;
        public static CA_Multi_Channel_Form getInstance()
        {
            if (Instance == null)
                Instance = new CA_Multi_Channel_Form();

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

        public I_CA_Control Get_ca_ctrl_obj()
        {
            return ca_ctrl_obj;
        }

        public bool Check_Is_CA_Connected()
        {
            return Is_CA_connected;
        }

        private void Button_Click_Enable(bool Able)
        {
            btn_CA_Connect.Enabled = Able;
            CA_zero_cal_button.Enabled = Able;
            CA_Measure_Button.Enabled = Able;
            button_clear.Enabled = Able;
        }

        private void CA_Multi_Channel_Form_Load(object sender, EventArgs e)
        {
            Status_Line_Num = 0;
            Initialize_Before_CA_Connected();
            Show_Selected_Model(); 
            GridView_Control gridView_Control = new GridView_Control(dataGridView_CA_Measure, dataGridView_CA1_5, dataGridView_CA6_10);
            gridView_Control.Initalize_GridView();

            Display_Messages_For_Users();
        }


        private void Display_Messages_For_Users()
        {
            RichTextBox_GB_Status.SelectionColor = Color.Red; 
            RichTextBox_GB_Status.AppendText("\nCA 연결전) 하기 CA설정을 잘 확인후 측정 진행 부탁드립니다. " + "\r\n");
            RichTextBox_GB_Status.SelectionColor = Color.Black; 
            RichTextBox_GB_Status.AppendText("1) CA310/CA410 둘중 어떤것을 연결할지 선택 후 CA Connect Button 클릭. " + "\r\n");
            Application.DoEvents();

            RichTextBox_GB_Status.SelectionColor = Color.Red; 
            RichTextBox_GB_Status.AppendText("\nCA 연결후) 하기 CA설정을 잘 확인후 측정 진행 부탁드립니다. (변경하는 설정은 CA에 바로 반영됨) " + "\r\n");
            RichTextBox_GB_Status.SelectionColor = Color.Black; 
            RichTextBox_GB_Status.AppendText("2) Port Channel Control : 연결된 사용 가능한 Channel의 CA가 Check된 형태로 보여지며" + "\r\n");
            RichTextBox_GB_Status.AppendText("2) Port Channel Control : 측정을 원하지 않는 CA는 Uncheck로 사용불가 상태로 설정 가능합니다." + "\r\n");
            RichTextBox_GB_Status.AppendText("3) CA Channel Setting : 반드시 White Calibration이 진행된 Channel로 설정 부탁드립니다.  " + "\r\n");
            RichTextBox_GB_Status.AppendText("4) CA Measure Mode : Auto/Fast/Slow. (Auto 모드를 추천드립니다.) " + "\r\n");
            RichTextBox_GB_Status.AppendText("5) CA Sync Mode : NTSC/Pal/Ext. (Ext모드가 가능하지 않으면 NTSC 추천드립니다.) " + "\r\n");
            Application.DoEvents();

            RichTextBox_GB_Status.SelectionColor = Color.Red;
            RichTextBox_GB_Status.AppendText("\nCA310의 경우 USB Type으로 동작하지만, CA410의 경우Port Type으로 동작합니다." + "\r\n");
            RichTextBox_GB_Status.AppendText("CA410을 사용하시는 경우, CA410이외 컴퓨터에 연결된 다른 Port는 연결을 해제해 주어야 합니다. (장치관리자 포트(COM) 확인)" + "\r\n");
            RichTextBox_GB_Status.SelectionColor = Color.Black;
            Application.DoEvents();

            RichTextBox_GB_Status.SelectionColor = Color.Red;
            RichTextBox_GB_Status.AppendText("\n측정하기전 Model Selection으로 측정하고자 하는 모델을 선택해 주세요" + "\r\n");
            RichTextBox_GB_Status.SelectionColor = Color.Black; 
            RichTextBox_GB_Status.AppendText("1) AOD 및 Delta E4의 경우 선택된 모델 사이즈에 맞는 네모 패턴을 띄움 \r\n");
            RichTextBox_GB_Status.AppendText("2) AOD On/Off 또한 선택된 모델에 상응하는 명령어로 적용됨 \r\n");
            Application.DoEvents();
        }

        private void Initialize_Before_CA_Connected()
        {
            Is_CA_connected = false;
            groupBox_Channel_Setting_Mode.Hide();
            groupBox_Measuremode.Hide();
            groupBox_Syncmode.Hide();
            radioButton_CA310.Enabled = true;
            radioButton_CA410.Enabled = true;
        }

        private void Initialize_After_CA_Connected()
        {
            Is_CA_connected = true;
            groupBox_Channel_Setting_Mode.Show();
            groupBox_Measuremode.Show();
            groupBox_Syncmode.Show();
            radioButton_CA310.Enabled = false;
            radioButton_CA410.Enabled = false;
        }

        private void btn_CA_Connect_Click(object sender, EventArgs e)
        {
            Button_Click_Enable(false);

            if (Is_CA_connected == false)
                Is_CA_connected = Connect_CA();

            if (Is_CA_connected)
            {
                Update_checkBox_CA310_Port_Channel_Checked();
                Initialize_After_CA_Connected();
                f1().Hide();
                Optic_Measurement_Form.getInstance().Hide();
                Optic_Measurement_10ch.getInstance().Hide();
                f1().Is_1ch_CA_Mode = false;
                btn_CA_Connect.Hide();

                CA_CyncMode_Update();
                CA_MeasurementMode_Update();
                CA_White_Channel_Update();
            }

            Button_Click_Enable(true);
        }

        private bool CA310_connect()
        {
            Multi_CA310_Control ca_obj = new Multi_CA310_Control(textBox_ch, dataGridView_CA1_5, dataGridView_CA6_10);
            ca_ctrl_obj = ca_obj;
            ca310_ctrl_obj = ca_obj;

            return ca_ctrl_obj.connect_CA();
        }


        private bool Connect_CA()
        {
            if (radioButton_CA410.Checked)
                return CA410_connect();
            else
                return CA310_connect();
        }

        private bool CA410_connect()
        {
            Multi_CA410_Control ca_obj = new Multi_CA410_Control(textBox_ch, dataGridView_CA1_5, dataGridView_CA6_10);
            ca_ctrl_obj = ca_obj;
            ca410_ctrl_obj = ca_obj;

            return ca_ctrl_obj.connect_CA();
        }


        private void Update_checkBox_CA310_Port_Channel_Checked()
        {
            bool[,] Is_Connected_Probe_Channels = ca_ctrl_obj.Get_Connected_Probe_Channels();

            for (int port = 0; port < 2; port++)
                for (int probe = 0; probe < 5; probe++)
                    checkBox_CA310_Port_Channel_Checked(port, probe, IsConnected: Is_Connected_Probe_Channels[port, probe]);
        }

        private void checkBox_CA310_Port_Channel_Checked(int port, int probe, bool IsConnected)
        {
            CheckBox checkBox_CA310_PortX_ChX = null;

            if (port == 0)
            {
                if (probe == 0) checkBox_CA310_PortX_ChX = checkBox_CA310_Port1_Ch1;
                else if (probe == 1) checkBox_CA310_PortX_ChX = checkBox_CA310_Port1_Ch2;
                else if (probe == 2) checkBox_CA310_PortX_ChX = checkBox_CA310_Port1_Ch3;
                else if (probe == 3) checkBox_CA310_PortX_ChX = checkBox_CA310_Port1_Ch4;
                else if (probe == 4) checkBox_CA310_PortX_ChX = checkBox_CA310_Port1_Ch5;
            }
            else if (port == 1)
            {
                if (probe == 0) checkBox_CA310_PortX_ChX = checkBox_CA310_Port2_Ch1;
                else if (probe == 1) checkBox_CA310_PortX_ChX = checkBox_CA310_Port2_Ch2;
                else if (probe == 2) checkBox_CA310_PortX_ChX = checkBox_CA310_Port2_Ch3;
                else if (probe == 3) checkBox_CA310_PortX_ChX = checkBox_CA310_Port2_Ch4;
                else if (probe == 4) checkBox_CA310_PortX_ChX = checkBox_CA310_Port2_Ch5;
            }

            checkBox_CA310_PortX_ChX.Checked = IsConnected;
            checkBox_CA310_PortX_ChX.Enabled = IsConnected;
        }

        public bool[] Get_Is_CA_connected_channels()
        {
            bool[] CA_connected_channels = new bool[10];

            CA_connected_channels[0] = checkBox_CA310_Port1_Ch1.Checked;
            CA_connected_channels[1] = checkBox_CA310_Port1_Ch2.Checked;
            CA_connected_channels[2] = checkBox_CA310_Port1_Ch3.Checked;
            CA_connected_channels[3] = checkBox_CA310_Port1_Ch4.Checked;
            CA_connected_channels[4] = checkBox_CA310_Port1_Ch5.Checked;

            CA_connected_channels[5] = checkBox_CA310_Port2_Ch1.Checked;
            CA_connected_channels[6] = checkBox_CA310_Port2_Ch2.Checked;
            CA_connected_channels[7] = checkBox_CA310_Port2_Ch3.Checked;
            CA_connected_channels[8] = checkBox_CA310_Port2_Ch4.Checked;
            CA_connected_channels[9] = checkBox_CA310_Port2_Ch5.Checked;

            return CA_connected_channels;
        }

        public void Update_checkBox_MultiCAChannel_Enabled(bool Able)
        {
            checkBox_CA310_Port1_Ch1.Enabled = Able;
            checkBox_CA310_Port1_Ch2.Enabled = Able;
            checkBox_CA310_Port1_Ch3.Enabled = Able;
            checkBox_CA310_Port1_Ch4.Enabled = Able;
            checkBox_CA310_Port1_Ch5.Enabled = Able;

            checkBox_CA310_Port2_Ch1.Enabled = Able;
            checkBox_CA310_Port2_Ch2.Enabled = Able;
            checkBox_CA310_Port2_Ch3.Enabled = Able;
            checkBox_CA310_Port2_Ch4.Enabled = Able;
            checkBox_CA310_Port2_Ch5.Enabled = Able;
        }




        private void CA_zero_cal_button_Click(object sender, EventArgs e)
        {
            ca_ctrl_obj.Zero_Cal();
        }

        private void CA_Measure_button_Click(object sender, EventArgs e)
        {
            Sub_Measure_and_Get_MeasuredGridviewData();
        }


        public XYLv[] Sub_Measure_and_Get_MeasuredData()
        {
            if (Is_CA_connected == false)
                throw new Exception("Connect CA first");

            XYLv[] measurement = ca_ctrl_obj.Get_Multi_MeasuredData();

            bool[] Is_Connected_Channels = Get_Is_CA_connected_channels();

            for (int ch = 0; ch < measurement.Length; ch++)
                if (Is_Connected_Channels[ch] == false)
                    measurement[ch].double_X = measurement[ch].double_Y = measurement[ch].double_Lv = 0;
         
            return measurement;
        }

        private XYLv[] Sub_Measure_and_Get_MeasuredGridviewData()
        {
            XYLv[] Gridview_Data = new XYLv[10];

            if (Is_CA_connected)
            {
                XYLv[] measurement = ca_ctrl_obj.Get_Multi_MeasuredData();

                bool[] Is_Connected_Channels = Get_Is_CA_connected_channels();

                for (int ch = 0; ch < measurement.Length; ch++)
                {
                    if (Is_Connected_Channels[ch])
                    {
                        Gridview_Data[ch] = measurement[ch];
                        dataGridView_CA_Measure.Rows.Add("ch" + (ch + 1), measurement[ch].double_X.ToString("0.0000"), measurement[ch].double_Y.ToString("0.0000"), measurement[ch].double_Lv.ToString("0.0000"));
                    }
                    else
                    {
                        Gridview_Data[ch].double_X = Gridview_Data[ch].double_Y = Gridview_Data[ch].double_Lv = 0;
                        dataGridView_CA_Measure.Rows.Add("ch" + (ch + 1).ToString().ToString(), "-", "-", "-");
                    }
                }
            }
            else
            {
                MessageBox.Show("Connect CA first");
            }

            return Gridview_Data;
        }

        public DataGridView Get_dataGridView_CA_Measure()
        {
            return dataGridView_CA_Measure;
        }



        private void CA_CyncMode_Update()
        {
            int syncmode = Get_SyncMode();
            ca_ctrl_obj.Set_SyncMode(syncmode, freq: 60);
            f1().GB_Status_AppendText_Nextline("CA SyncMode(NTSC 0 / PAL 1 / EXT 2 / INT 4) Updated : " + syncmode, Color.Purple);
        }

        private void CA_MeasurementMode_Update()
        {
            int MeasurementMode = Get_MeasurementMode();
            ca_ctrl_obj.Set_MeasruemnetMode(MeasurementMode);
            f1().GB_Status_AppendText_Nextline("CA MeasurementMode(Slow 0/ Fast 1/ Auto 2) Updated : " + MeasurementMode, Color.Purple);
        }


        private int Get_SyncMode()
        {
            // SyncMode : NTSC 0 / PAL 1 / EXT 2 / INT 4
            int syncmode;
            if (radioButton_NTSC.Checked) syncmode = 0;
            else if (radioButton_PAL.Checked) syncmode = 1;
            else if (radioButton_EXT.Checked) syncmode = 2;
            else
                throw new Exception("this Get_SyncMode is not processed");

            return syncmode;
        }

        private int Get_MeasurementMode()
        {
            int MeasurementMode;      //Measurement speed : FAST

            if (this.radioButton_CA_Measure_Auto.Checked) MeasurementMode = 2; // Auto
            else if (this.radioButton_CA_Measure_Fast.Checked) MeasurementMode = 1; // Fast
            else if (this.radioButton_CA_Measure_Slow.Checked) MeasurementMode = 0; // Slow
            else
                throw new Exception("this Get_MeasurementMode is not processed");

            return MeasurementMode;
        }


        private void radioButton_CA_Measure_Auto_CheckedChanged(object sender, EventArgs e)
        {
            CA_MeasurementMode_Update();
        }
        private void radioButton_CA_Measure_Fast_CheckedChanged(object sender, EventArgs e)
        {
            CA_MeasurementMode_Update();
        }
        private void radioButton_CA_Measure_Slow_CheckedChanged(object sender, EventArgs e)
        {
            CA_MeasurementMode_Update();
        }



        private void radioButton_NTSC_CheckedChanged(object sender, EventArgs e)
        {
            CA_CyncMode_Update();
        }
        private void radioButton_PAL_CheckedChanged(object sender, EventArgs e)
        {
            CA_CyncMode_Update();
        }
        private void radioButton_EXT_CheckedChanged(object sender, EventArgs e)
        {
            CA_CyncMode_Update();
        }




        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            CA_White_Channel_Update();
        }




        private void CA_White_Channel_Update()
        {
            int ca_ch = trackBar2.Value;
            textBox_ch.Text = ca_ch.ToString();
            ca_ctrl_obj.Set_White_Channel(ca_ch);
            f1().GB_Status_AppendText_Nextline("CA White Channel Updated : " + ca_ch + "ch", Color.Purple);
        }
      

        private void button_Hide_Click(object sender, EventArgs e)
        {
            if (Is_CA_connected)
            {
                f1().Exit_Whole_Program();
            }
            else
            {
                this.Visible = false;
                f1().Enabled = true;
            }
        }

        private void version1ForQAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Is_CA_connected)
            {
                Optic_Measurement_Form.getInstance().Visible = false;
                Optic_Measurement_Form.getInstance().Show();
                Optic_Measurement_Form.getInstance().Set_IChannel(new Multi_Channel());
            }
            else
            {
                MessageBox.Show("Please Connect Multi-CA First");
            }

           
        }

        private void version2ForGAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Is_CA_connected)
            {
                Optic_Measurement_10ch.getInstance().Visible = false;
                Optic_Measurement_10ch.getInstance().Show();
                Optic_Measurement_10ch.getInstance().UpdateModelInfo();
            }
            else
            {
                MessageBox.Show("Please Connect Multi-CA First");
            }
        }

        private void Show_Selected_Model()
        {
            this.Text = "CA_Multi_Channel_Form (Selected Model : " + f1().current_model.Get_Current_Model_Name() + ")";
        }

        private void dP086ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1().model_dp086_update();
            Show_Selected_Model();
        }

        private void dP116ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1().model_dp116_update();
            Show_Selected_Model();
        }

        private void dP150ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1().model_dp150_update();
            Show_Selected_Model();
        }

        private void metaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1().model_meta_update();
            Show_Selected_Model();
        }

        private void dP173ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1().model_dp173_update();
            Show_Selected_Model();
        }

        private void elginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1().model_elgin_update();
            Show_Selected_Model();
        }

        private void dP213ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1().model_dp213_update();
            Show_Selected_Model();
        }

        public void GB_Status_AppendText_Nextline(string text, System.Drawing.Color color)
        {
            if (Status_Line_Num > max_Status_line)
            {
                Status_Line_Num = 0;

                RichTextBox_GB_Status.Clear();
                Thread.Sleep(2);
            }

            Status_Line_Num++;

            //Color (Text 색 바꾸고,AppendText)
            RichTextBox_GB_Status.SelectionColor = color;
            RichTextBox_GB_Status.AppendText(text + "\r\n");
            //Black (Text 색 원복 as ForeColor)
            RichTextBox_GB_Status.SelectionColor = RichTextBox_GB_Status.ForeColor;//System.Drawing.Color.Black;
                                                                                   //Scroll to the end Without Focus
            RichTextBox_GB_Status.SelectionStart = RichTextBox_GB_Status.Text.Length;
            RichTextBox_GB_Status.ScrollToCaret();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            dataGridView_CA_Measure.Rows.Clear();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
