using System;
using System.Drawing;
using System.Windows.Forms;
using BSQH_Csharp_Library;
using CA200SRVRLib;

namespace PNC_Csharp.CA_Multi_Channels
{
    class Multi_CA310_Control : Base_Multi_CA_Control, I_CA310_Control, I_CA_Control
    {
        // CA-310 관련 변수
        const int max_port_num = 2;
        const int max_port_probe_num = 5;

        int ca_count;
        int probe_count;
        public Ca200[] objCa200_310;
        public Ca[] objCa_310;
        public Cas[] objCas_310;
        public Probes[] objProbes_310;
        public Memory[] objMemory_310;

        TextBox textBox_ch_W;
        DataGridView dataGridView_CA1_5;
        DataGridView dataGridView_CA6_10;
        bool[,] Connected_Channels;

        public Multi_CA310_Control(TextBox _textBox_ch_W, DataGridView _dataGridView_CA1_5, DataGridView _dataGridView_CA6_10)
        {
            ca_count = 0;
            probe_count = 0;
            textBox_ch_W = _textBox_ch_W;
            dataGridView_CA1_5 = _dataGridView_CA1_5;
            dataGridView_CA6_10 = _dataGridView_CA6_10;
        }

        public bool[,] Get_Connected_Probe_Channels()
        {
            return Connected_Channels;
        }

        public bool connect_CA()
        {
            string[] ca_probe = new string[2];
            ca_probe = CA310_update_channel_and_probe_count();
            bool CA_connected;

            if (ca_count != 0)
            {
                objCa_310 = new CA200SRVRLib.Ca[ca_count];
                objCas_310 = new CA200SRVRLib.Cas[ca_count];
                objProbes_310 = new CA200SRVRLib.Probes[ca_count];
                objMemory_310 = new CA200SRVRLib.Memory[ca_count];

                try
                {
                    for (int port = 0; port < ca_count; port++)
                    {
                        objCas_310[port] = objCa200_310[port].Cas;
                        objCa_310[port] = objCas_310[port].get_ItemOfNumber(CaNumberVal : 1);
                        objMemory_310[port] = objCa_310[port].Memory;
                        objProbes_310[port] = objCa_310[port].Probes;
                        objCa_310[port].OutputProbes.AddAll();
                    }
                    CA_connected = true;

                    CA310_probe_info_update();
                }
                catch(Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    CA_connected = false;
                }
            }
            else
            {
                CA_connected = false;
                System.Windows.Forms.MessageBox.Show("Please check the number of CA-310");
            }

            if (CA_connected)
                this.CA_Setting_After_CA_Connect();

            return CA_connected;
        }



        private bool[,] Get_Updated_Connected_Channels()
        {
            string[] ConnecStringVal = new string[max_port_probe_num]
            {
                "1",
                "12",
                "123",
                "1234",
                "12345",
            };

            objCa200_310 = new Ca200[max_port_num];

            bool[,] Connected_Channels = new bool[max_port_num, max_port_probe_num];
            for(int i = 0;i< max_port_num;i++)
                for (int k = 0; k < max_port_probe_num; k++)
                   Connected_Channels[i, k] = false;
            

            for (int port = 0; port < max_port_num; port++)
            {
                objCa200_310[port] = new Ca200();
                for (int probe = max_port_probe_num - 1; probe >= 0; probe--)
                {
                    try
                    {
                        objCa200_310[port].SetConfiguration(1, ConnecStringVal[probe], PortVal: port);
                        
                        for(int i =0;i<= probe; i++)
                                Connected_Channels[port, i] = true;

                        break;
                    }
                    catch
                    {

                    }
                }
            }
            return Connected_Channels;
        }

        public XYLv[] Get_Multi_MeasuredData()
        {
            XYLv[] measurement = new XYLv[max_port_num * max_port_probe_num];

            for (int ca = 0; ca < ca_count; ca++)
            {
                objCa_310[ca].DisplayMode = 0;
                objCa_310[ca].Measure();
            }

            for(int port = 0;port < max_port_num;port++)
            {
                for (int probe = 0; probe < max_port_probe_num; probe++)
                {
                    int index = (port * max_port_probe_num) + probe;
                   
                    if (Connected_Channels[port,probe])
                    {
                        measurement[index].double_X = Convert.ToDouble(objCa_310[port].OutputProbes.get_ItemOfNumber(probe + 1).sx);
                        measurement[index].double_Y = Convert.ToDouble(objCa_310[port].OutputProbes.get_ItemOfNumber(probe + 1).sy);
                        measurement[index].double_Lv = Convert.ToDouble(objCa_310[port].OutputProbes.get_ItemOfNumber(probe + 1).Lv);
                    }
                }
            }                

            return measurement;
        }

        private void CA310_probe_info_update()
        {
            dataGridView_CA1_5.Rows[0].DefaultCellStyle.Font = new Font("굴림", 7);
            //for (int probe = 0; probe < Convert.ToInt32(textBox_CA310_no_of_probe_1.Text); probe++)
            for (int probe = 0; probe < max_port_probe_num; probe++)
            {
                if (Connected_Channels[0, probe])
                    dataGridView_CA1_5.Rows[0].Cells[probe].Value = objCa_310[0].OutputProbes.get_ItemOfNumber(probe + 1).SerialNO.Substring(2, objCa_310[0].OutputProbes.get_ItemOfNumber(probe + 1).SerialNO.Length - 2);
            }
            
            if (ca_count > 1)
            {
                dataGridView_CA6_10.Rows[0].DefaultCellStyle.Font = new Font("굴림", 7);
                //for (int probe = 0; probe < Convert.ToInt32(textBox_CA310_no_of_probe_2.Text); probe++)
                for (int probe = 0; probe < max_port_probe_num; probe++)
                {
                    if(Connected_Channels[1, probe])
                        dataGridView_CA6_10.Rows[0].Cells[probe].Value = objCa_310[1].OutputProbes.get_ItemOfNumber(probe + 1).SerialNO.Substring(2, objCa_310[1].OutputProbes.get_ItemOfNumber(probe + 1).SerialNO.Length - 2);
                }    
                    
            }
        }

        private string[] CA310_update_channel_and_probe_count()
        {
            Connected_Channels = Get_Updated_Connected_Channels();
            string[] ca_probe = new string[max_port_num];
            ca_count = 0;
            probe_count = 0;

            ca_probe[0] = string.Empty;
            ca_probe[1] = string.Empty;

            for (int port = 0; port < max_port_num; port++)
            {
                for (int probe = 0; probe < max_port_probe_num; probe++)
                {
                    if (Connected_Channels[port, probe])
                    {
                        probe_count++;
                        ca_probe[port] += (probe + 1).ToString();
                    }
                }
            }

            if (ca_probe[0] != string.Empty) ca_count++;
            if (ca_probe[1] != string.Empty) ca_count++;

            return ca_probe;
        }


        private void CA_Setting_After_CA_Connect()
        {
            int freqmode = 0;   // SyncMode : NTSC 0 / PAL 1 / EXT 2 / INT 4
            int speed = 1;      //Measurement speed : FAST
            int Lvmode = 1;     //Lv : cd/m2

            for (int ca = 0; ca < ca_count; ca++)
            {
                objCa_310[ca].AveragingMode = speed;
                objCa_310[ca].BrightnessUnit = Lvmode;
                objCa_310[ca].SyncMode = freqmode;
                objMemory_310[ca].ChannelNO = Convert.ToInt32(textBox_ch_W.Text);
            }
        }

        public void CA_RemoteMode_On()
        {
            for (int ca = 0; ca < ca_count; ca++)
                objCa_310[ca].RemoteMode = 1;
        }
        public void CA_RemoteMode_Off()
        {
            for (int ca = 0; ca < ca_count; ca++)
                objCa_310[ca].RemoteMode = 0;
        }

        public void Zero_Cal()
        {
            for (int ca = 0; ca < ca_count; ca++)
                Sub_Zero_Cal(objCa_310[ca]);
        }

        private void Sub_Zero_Cal(CA200SRVRLib.Ca objCa)
        {
            bool calzero_success = false;

            while (calzero_success == false)
            {
                try
                {
                    objCa.CalZero();
                    calzero_success = true;
                }
                catch (Exception er)
                {
                    f1().DisplayError(er);
                    if (System.Windows.Forms.MessageBox.Show("Zero Cal Error\r\nRetry?", "CalZero", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        objCa.RemoteMode = 0;
                        System.Windows.Forms.Application.Exit();
                    }
                }
            }
        }

        public void Set_SyncMode(int freqmode, double freq = 60.0)
        {
            for (int ca = 0; ca < ca_count; ca++)
                objCa_310[ca].SyncMode = freqmode;
            
        }
        public void Set_MeasruemnetMode(int MeasuremntMode)
        {
            for (int ca = 0; ca < ca_count; ca++)
                objCa_310[ca].AveragingMode = MeasuremntMode;
        }

        public void Set_White_Channel(int ca_ch)
        {
            for (int ca = 0; ca < ca_count; ca++)
                objMemory_310[ca].ChannelNO = ca_ch;
        }

        public int Get_Probe_Count()
        {
            return probe_count;
        }
    }
}
