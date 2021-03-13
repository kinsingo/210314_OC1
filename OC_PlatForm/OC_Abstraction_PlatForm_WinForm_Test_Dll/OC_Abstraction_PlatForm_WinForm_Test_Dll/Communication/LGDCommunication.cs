using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;

//PNC
using System.IO.MemoryMappedFiles;
using System.Threading;


namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication
{
    class LGDCommunication : ICommunication
    {
        RichTextBox richtextbox;
        //PNC parameters
        bool bIPC_Open = false;
        MemoryMappedViewAccessor m_hMemoryAccessor = null;
        MemoryMappedFile m_hMemoryMapped = null;
        EventWaitHandle evt = null;
        int cnt_Send = 0;

        public LGDCommunication(RichTextBox _richtextbox)
        {
            richtextbox = _richtextbox;
        }


        public void DisplayBoxPattern(byte[] Box_RGB, byte[] Background_RGB, int[] Pos_BoxLeftTop, int[] Pos_BoxRightBottom, int channel_num)
        {
            int boxWidth = Pos_BoxRightBottom[0] - Pos_BoxLeftTop[0];
            int boxHeight = Pos_BoxRightBottom[1] - Pos_BoxLeftTop[1];

            string box_image = string.Format("image.crosstalk {0} {1} {2} {3} {4} {5} {6} {7}", boxWidth, boxHeight, Background_RGB[0], Background_RGB[1], Background_RGB[2], Box_RGB[0], Box_RGB[1], Box_RGB[2]);
            IPC_Quick_Send(box_image);
        }


        public void DisplayMonoPattern(byte[] RGB, int channel_num)
        {
            IPC_Quick_Send(String.Format("image.mono {0} {1} {2}", RGB[0], RGB[1], RGB[2]));
        }


        public byte[] ReadData(byte address, int amount, int offset, int channel_num)
        {
            throw new NotImplementedException();
        }


        public void WriteData(byte address, byte[] parameters, int channel_num)
        {
            StringBuilder mipiCMD = new StringBuilder("mipi.write");
            if (parameters.Length == 0)
                mipiCMD.Append(" 0x").Append(address.ToString("X2"));
            else if (parameters.Length == 1)
                mipiCMD.Append(" 0x15");
            else
                mipiCMD.Append(" 0x39");

            foreach (byte papam in parameters)
                mipiCMD.Append(" 0x").Append(papam.ToString("X2"));

            IPC_Quick_Send(mipiCMD.ToString());
        }

        private void IPC_Open()
        {
            try
            {
                m_hMemoryMapped = MemoryMappedFile.OpenExisting("PNC_DIKI_IPC", MemoryMappedFileRights.ReadWrite);
                if (m_hMemoryMapped == null)
                {
                    System.Windows.Forms.MessageBox.Show("Memory Mapping Error");
                    bIPC_Open = false;
                }

                m_hMemoryAccessor = m_hMemoryMapped.CreateViewAccessor();
                if (m_hMemoryAccessor == null)
                {
                    MessageBox.Show("Memory Access Error");
                    bIPC_Open = false;
                }

                evt = new EventWaitHandle(false, EventResetMode.ManualReset, "PNC_DIKI_IPC_READ");
                bIPC_Open = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("IPC Open Error !! : " + e.Message);
                bIPC_Open = false;
            }
        }

        private void IPC_Quick_Send(string Ipc_str)
        {
            if(bIPC_Open == false)
                IPC_Open();

            if(bIPC_Open)
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
                Thread.Sleep(2);

                // --------------------------------------------------------------------------
                // command send
                m_hMemoryAccessor.WriteArray<byte>(0, WriteByte, 0, WriteByte.Length);
                cnt_Send++;
                if (cnt_Send == 10)
                    cnt_Send = 0;

                Thread.Sleep(10);

                byte[] bReadData = new byte[1024];

                //To make sure during iteration these params will not be changed
                const int PNC_ACK_Sleep_ms = 20;
                const int PNC_ACK_Loop_Max_local = 10;

                for (int Ack = 0; Ack < PNC_ACK_Loop_Max_local; Ack++)
                {
                    Thread.Sleep(PNC_ACK_Sleep_ms);
                    m_hMemoryAccessor.ReadArray<byte>(0, bReadData, 0, bReadData.Length);
                    if (bReadData[0] == 'r')
                    {
                        break;
                    }
                    else if (bReadData[0] == 'm')
                    {
                        string tta = Encoding.Default.GetString(bReadData) + "\r\n";
                        richtextbox.AppendText(tta);
                        break;
                    }
                }
                evt.Reset();
            }
        }
    }
}
