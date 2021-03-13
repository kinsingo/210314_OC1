using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;//Port 190530
using BSQH_Csharp_Library;
using CASDK2;


namespace PNC_Csharp.CA_Multi_Channels
{
    class Multi_CA410_Control : Base_Multi_CA_Control, I_CA410_Control, I_CA_Control
    {
        // CA-410 관련 변수
        int ca_and_probe_count; 
        CASDK2Ca200 objCa200;
        CASDK2Cas objCas;
        CASDK2Ca[] objCa;
        CASDK2Probes[] objProbes;
        CASDK2OutputProbes[] objOutputProbes;
        CASDK2Probe[] objProbe;
        CASDK2Memory[] objMemory;
        bool[] Is_CA_Connected;
        CASDK2DeviceData[] pDeviceData;

        DataGridView dataGridView_CA1_5;
        DataGridView dataGridView_CA6_10;
        TextBox textBox_ch_W;

        
        public Multi_CA410_Control(TextBox _textBox_ch_W, DataGridView _dataGridView_CA1_5, DataGridView _dataGridView_CA6_10)
        {
            ca_and_probe_count = 0;

            textBox_ch_W = _textBox_ch_W;
            dataGridView_CA1_5 = _dataGridView_CA1_5;
            dataGridView_CA6_10 = _dataGridView_CA6_10;
        }

        public bool[,] Get_Connected_Probe_Channels()
        {
            const int max_port_num = 2;
            const int max_port_probe_num = 5;

            bool[,] Connected_Channels = new bool[max_port_num,max_port_probe_num];
            for (int port = 0; port < max_port_num; port++)
                for (int probe = 0; probe < max_port_probe_num; probe++)
                    Connected_Channels[port, probe] = false;

            for (int i = 0;i< ca_and_probe_count;i++)
            {
                if(i < max_port_probe_num)
                    Connected_Channels[0, i] = true;
                else
                    Connected_Channels[1, i - max_port_probe_num] = true;
            }

            return Connected_Channels;
        }

        public bool connect_CA()
        {
            Get_All_Serial_Port();

            //Create CA Object
            objCa = new CASDK2Ca[ca_and_probe_count];
            objProbes = new CASDK2Probes[ca_and_probe_count];
            objOutputProbes = new CASDK2OutputProbes[ca_and_probe_count];
            objProbe = new CASDK2Probe[ca_and_probe_count];
            objMemory = new CASDK2Memory[ca_and_probe_count];
            Is_CA_Connected = new bool[ca_and_probe_count];
            objCa200 = new CASDK2Ca200();

            //int ca_port;
            for (int ca = 0; ca < ca_and_probe_count; ca++)
                Is_CA_Connected[ca] = GetErrorMessage(objCa200.SetConfiguration(ca + 1, "1", pDeviceData[ca].lPortNo, 38400, 0));

            bool CA_connected = true;

            objCa200.get_Cas(ref objCas);
            for (int channel = 0; channel < ca_and_probe_count; channel++)
            {
                if (Is_CA_Connected[channel])
                {
                    CA_connected = GetErrorMessage(objCas.get_Item(channel + 1, ref objCa[channel])); if (CA_connected == false) break;
                    CA_connected = GetErrorMessage(objCa[channel].get_Probes(ref objProbes[channel])); if (CA_connected == false) break;
                    CA_connected = GetErrorMessage(objCa[channel].get_OutputProbes(ref objOutputProbes[channel])); if (CA_connected == false) break;
                    CA_connected = GetErrorMessage(objCa[channel].get_Memory(ref objMemory[channel])); if (CA_connected == false) break;
                    CA_connected = GetErrorMessage(objProbes[channel].get_Item(1, ref objProbe[channel])); if (CA_connected == false) break;
                    CA_connected = GetErrorMessage(objOutputProbes[channel].AddAll()); if (CA_connected == false) break;
                    CA_connected = GetErrorMessage(objOutputProbes[channel].get_Item(1, ref objProbe[channel])); if (CA_connected == false) break;

                    if (channel < 5)
                        dataGridView_CA1_5.Rows[0].Cells[channel].Style.ForeColor = Color.Green;
                    else
                        dataGridView_CA6_10.Rows[0].Cells[channel].Style.ForeColor = Color.Green;
                }
                else
                {
                    if (channel < 5)
                        dataGridView_CA1_5.Rows[0].Cells[channel].Style.ForeColor = Color.Red;
                    else
                        dataGridView_CA6_10.Rows[0].Cells[channel].Style.ForeColor = Color.Red;
                }
            }

            if (CA_connected)
                this.CA_Setting_After_CA_Connect();

            return CA_connected;
        }

        public XYLv[] Get_Multi_MeasuredData()
        {
            XYLv[] measurement = new XYLv[ca_and_probe_count];

            for (int ca = 0; ca < ca_and_probe_count; ca++)
                GetErrorMessage(objCa[ca].put_DisplayMode(0));     //Set Lvxy mode

            GetErrorMessage(objCas.SendMsr());         //Measure
            GetErrorMessage(objCas.ReceiveMsr());      //Get results

            for (int ca = 0; ca < ca_and_probe_count; ca++)
            {
                // Get measurement data
                GetErrorMessage(objProbe[ca].get_Lv(ref measurement[ca].double_Lv));
                GetErrorMessage(objProbe[ca].get_sx(ref measurement[ca].double_X));
                GetErrorMessage(objProbe[ca].get_sy(ref measurement[ca].double_Y));
            }
            return measurement;
        }

        protected void Get_All_Serial_Port()
        {
            ca_and_probe_count = 0;
            string[] ports = SerialPort.GetPortNames();
            ca_and_probe_count = ports.Length;
            pDeviceData = new CASDK2DeviceData[ca_and_probe_count];
            CASDK2.CASDK2Discovery.SearchAllUSBDevices(ref pDeviceData);

            for (int ca = 0; ca < ca_and_probe_count; ca++)
            {
                // CH1~5와 CH6~10 grid 분리 & CA serial number 끝 4자리 표시
                dataGridView_CA1_5.Rows[0].DefaultCellStyle.Font = new Font("굴림", 9);
                dataGridView_CA6_10.Rows[0].DefaultCellStyle.Font = new Font("굴림", 9);
                if (ca_and_probe_count < 5) dataGridView_CA1_5.Rows[0].Cells[ca].Value = pDeviceData[ca].strSerialNo.Substring(4, pDeviceData[ca].strSerialNo.Length - 4);
                else dataGridView_CA6_10.Rows[0].Cells[ca - 5].Value = pDeviceData[ca].strSerialNo.Substring(4, pDeviceData[ca].strSerialNo.Length - 4);
            }
        }

        private void CA_Setting_After_CA_Connect()
        {
            int freqmode = 0;   // SyncMode : NTSC 0 / PAL 1 / EXT 2 / INT 4
            double freq = 60.0; //frequency = 60.0Hz
            int speed = 1;      //Measurement speed : FAST
            int Lvmode = 1;     //Lv : cd/m2

            for (int ca = 0; ca < ca_and_probe_count; ca++)
            {
                GetErrorMessage(objCa[ca].CalZero());                      //Zero-Calibration
                GetErrorMessage(objCa[ca].put_DisplayProbe("P1"));         //Set display probe to P1
                GetErrorMessage(objCa[ca].put_SyncMode(freqmode, freq));   //Set sync mode and frequency
                GetErrorMessage(objCa[ca].put_AveragingMode(speed));       //Set measurement speed
                GetErrorMessage(objCa[ca].put_BrightnessUnit(Lvmode));     //Set Brightness unit
                GetErrorMessage(objMemory[ca].put_ChannelNO(Convert.ToInt32(textBox_ch_W.Text)));
            }
        }

        public void Zero_Cal()
        {
            for (int ca = 0; ca < ca_and_probe_count; ca++)
                GetErrorMessage(objCa[ca].CalZero());
            
        }
        public void Set_SyncMode(int freqmode, double freq = 60.0)
        {
            for (int ca = 0; ca < ca_and_probe_count; ca++)
                GetErrorMessage(objCa[ca].put_SyncMode(freqmode, freq));

        }
        public void Set_MeasruemnetMode(int MeasuremntMode)
        {
            for (int ca = 0; ca < ca_and_probe_count; ca++)
                GetErrorMessage(objCa[ca].put_AveragingMode(MeasuremntMode));
        }

        public void Set_White_Channel(int ca_ch)
        {
            for (int ca = 0; ca < ca_and_probe_count; ca++)
                GetErrorMessage(objMemory[ca].put_ChannelNO(ca_ch));
        }

        public int Get_Probe_Count()
        {
            return ca_and_probe_count;
        }
    }
}
