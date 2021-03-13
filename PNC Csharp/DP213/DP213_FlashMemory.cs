using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
   public interface DP213_FlashMemory_For_User
    {
        void Read_From_Frame_and_Show();
        void Read_From_Flash_and_Show();
        void Send_EA9155_Info_and_Flash_Erase_And_Write();
        void OD_Flash_Erase_Write_and_CRC_Check();
        int Get_Dec_CMOTP();
        int Get_Dec_GammaOTP();
        int Get_Dec_LGOTP();
        int Get_Dec_IDOTP();
    }

    public class DP213_FlashMemory : DP213_forms_accessor, DP213_FlashMemory_For_User
    {
        //singleton
        static private DP213_FlashMemory instance;
        private DP213_FlashMemory() { }
        public static DP213_FlashMemory getInstance()
        {
            if (instance == null)
                instance = new DP213_FlashMemory();

            return instance;
        }

        private int Read_Dec_CMOTP;
        public int Get_Dec_CMOTP()
        {
            return Read_Dec_CMOTP;
        }

        private int Read_Dec_GammaOTP;
        public int Get_Dec_GammaOTP()
        {
            return Read_Dec_GammaOTP;
        }

        private int Read_Dec_LGOTP;
        public int Get_Dec_LGOTP()
        {
            return Read_Dec_LGOTP;
        }

        private int Read_Dec_IDOTP;
        public int Get_Dec_IDOTP()
        {
            return Read_Dec_IDOTP;
        }



        public void Send_EA9155_Info_and_Flash_Erase_And_Write()
        {
            Flash_Erase_And_Write();
        }


        private void Enable_Flash_Erase_Write()
        {
            f1().DP173_One_Param_CMD_Send_and_Show(5, "E8", "AA");//Unlock CMOTP protection (Flash Writng Enabled)
            f1().DP173_One_Param_CMD_Send_and_Show(7, "E8", "01");//Set Flash Write Clock as 1ms 
        }

        private void Flash_Erase_And_Write()
        {
            Enable_Flash_Erase_Write();
            int Flash_Erase_Write_Delay = 200;

            //CMOTP
            f1().DP173_One_Param_CMD_Send_and_Show(8, "E8", "1F");//Flash Erase&Write
            Thread.Sleep(Flash_Erase_Write_Delay);
            Flash_Fail_Verify_Check("CMOTP");

            //GMOTP
            f1().DP173_One_Param_CMD_Send_and_Show(9, "E8", "1F");//Flash Erase&Write
            Thread.Sleep(Flash_Erase_Write_Delay);
            Flash_Fail_Verify_Check("GammaOTP");

            //LGOTP
            f1().DP173_One_Param_CMD_Send_and_Show(10, "E8", "1F");//Flash Erase&Write
            Thread.Sleep(Flash_Erase_Write_Delay);
            Flash_Fail_Verify_Check("LGOTP");
        }


        private void Flash_Fail_Verify_Check(string Show_Memory_Block)
        {
            f1().MX_OTP_Read(2, 1, "DD");
            string Flash_Status_Check = f1().dataGridView1.Rows[0].Cells[1].Value.ToString();
            if (Flash_Status_Check == "01")
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound(Show_Memory_Block + " Writing is not finished yet(Flash Busy). it needs to be set more writing time");
            }
            else if (Flash_Status_Check == "80")
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound(Show_Memory_Block + " OC Writing Failed.");
            }
            else if (Flash_Status_Check == "00" || Flash_Status_Check == "02")
            {
                f1().Show_OK_Message(Show_Memory_Block + " Flash OC Writing OK");
            }
            else
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("Unknown Status, DD(P3) = " + Flash_Status_Check);
            }
        }





        private void Unlock_CMOTP_GammaOTP_LGOTP_IDOTP_Protection()
        {
            f1().DP173_One_Param_CMD_Send(5, "E8", "AA");//Unlock CM/Gamma/LG/IDOTP protection (Flash Writng Enabled)
        }

        private void Preload_For_CMOTP_GammaOTP_LGOTP_IDOTP()
        {
            for (int i = 8; i <= 11; i++)
            {
                f1().DP173_One_Param_CMD_Send(i, "E8", "00");
                f1().DP173_One_Param_CMD_Send(i, "E8", "80");
            }
        }

        private void Read_and_Show_CMOTP_GammaOTP_LGOTP_IDOTP_CRC()
        {
            f1().MX_OTP_Read(7, 8, "DD"); //Read Hex_CheckSum and Show

            string[] Hex_Checksum = new string[8];
            for (int i = 0; i < Hex_Checksum.Length; i++)
            {
                Hex_Checksum[i] = f1().dataGridView1.Rows[i].Cells[1].Value.ToString();
            }

            Read_Dec_CMOTP = Convert.ToInt32(Hex_Checksum[0] + Hex_Checksum[1], 16);
            Read_Dec_GammaOTP = Convert.ToInt32(Hex_Checksum[2] + Hex_Checksum[3], 16);
            Read_Dec_LGOTP = Convert.ToInt32(Hex_Checksum[4] + Hex_Checksum[5], 16);
            Read_Dec_IDOTP = Convert.ToInt32(Hex_Checksum[6] + Hex_Checksum[7], 16);

            f1().GB_Status_AppendText_Nextline("CMOTP CRC : " + Hex_Checksum[0] + "," + Hex_Checksum[1], Color.Black);
            f1().GB_Status_AppendText_Nextline("GammaOTP CRC : " + Hex_Checksum[2] + "," + Hex_Checksum[3], Color.Black);
            f1().GB_Status_AppendText_Nextline("LGOTP CRC : " + Hex_Checksum[4] + "," + Hex_Checksum[5], Color.Black);
            f1().GB_Status_AppendText_Nextline("IDOTP CRC : " + Hex_Checksum[6] + "," + Hex_Checksum[7], Color.Black);
        }




        private void Initializing_For_OD_ERA_Flash_Write()
        {
            f1().IPC_Quick_Send("mipi.write 0x05 0x28");
            f1().IPC_Quick_Send("mipi.video.disable");

            f1().IPC_Quick_Send("mipi.write 0x39 0x2A 0x00 0x00 0x05 0x9F");//1440 (For DP173 Panel)
            f1().IPC_Quick_Send("mipi.write 0x39 0x2B 0x00 0x00 0x0D 0x1F");//3360 (For DP173 Panel)

            f1().IPC_Quick_Send("mipi.write 0x15 0xb0 0x08");
            f1().IPC_Quick_Send("mipi.write 0x15 0xF5 0x30");
        }

        private void OD_Flash_Erase_Write_Verify()
        {
            f1().DP173_One_Param_CMD_Send(5, "E8", "BB"); //Protection_Key = "BB"
            f1().DP173_One_Param_CMD_Send(12, "E8", "08"); //Set LUT Index (ERA=0x06 / OD=0x08)

            LUT_Preload_and_LUT_Flash_Erase_Write();
            Thread.Sleep(1000);
            Flash_Fail_Verify_Check("ODOTP");
        }

        private void ERA_Flash_Erase_Write_Verify()
        {
            f1().DP173_One_Param_CMD_Send(5, "E8", "BB"); //Protection_Key = "BB"
            f1().DP173_One_Param_CMD_Send(12, "E8", "06"); //Set LUT Index (ERA=0x06 / OD=0x08)

            LUT_Preload_and_LUT_Flash_Erase_Write();
            Thread.Sleep(1000);
            Flash_Fail_Verify_Check("ERAOTP");
        }

        private void LUT_Preload_and_LUT_Flash_Erase_Write()
        {
            f1().DP173_One_Param_CMD_Send(13, "E8", "00");
            f1().DP173_One_Param_CMD_Send(13, "E8", "80"); //LUT Preload

            f1().DP173_One_Param_CMD_Send(13, "E8", "00");
            f1().DP173_One_Param_CMD_Send(13, "E8", "13"); //LUT Flash Write & Erase
        }

        private bool IS_OD_Flash_CRC_OK()
        {
            string[] OD_CRC_Hex = new string[2];
            OD_CRC_Hex[0] = dp213_form().textBox_OD_CRC_Hex_1.Text;
            OD_CRC_Hex[1] = dp213_form().textBox_OD_CRC_Hex_2.Text;

            f1().DP173_One_Param_CMD_Send(5, "E8", "11");  //Protection_Key = "11"

            f1().DP173_One_Param_CMD_Send(4, "E8", "00");
            f1().DP173_One_Param_CMD_Send(4, "E8", "18"); //LUT Index_Setting & Flash_Read (ERA = 0x16 / OD = 0x18)

            f1().MX_OTP_Read(20, 2, "DD");

            string[] Read_Hex_Checksum = new string[2];
            Read_Hex_Checksum[0] = f1().dataGridView1.Rows[0].Cells[1].Value.ToString();
            Read_Hex_Checksum[1] = f1().dataGridView1.Rows[1].Cells[1].Value.ToString();

            f1().GB_Status_AppendText_Nextline("OD_CRC_Hex[0] / OD_CRC_Hex[1] : " + OD_CRC_Hex[0] + "/" + OD_CRC_Hex[1], Color.Blue);
            f1().GB_Status_AppendText_Nextline("Read_Hex_Checksum[0] / Read_Hex_Checksum[1] : " + Read_Hex_Checksum[0] + "/" + Read_Hex_Checksum[1], Color.Blue);

            if ((OD_CRC_Hex[0] == Read_Hex_Checksum[0]) && (OD_CRC_Hex[1] == Read_Hex_Checksum[1]))
                return true;
            else
                return false;
        }


        public void Read_From_Frame_and_Show()
        {
            Unlock_CMOTP_GammaOTP_LGOTP_IDOTP_Protection();
            Preload_For_CMOTP_GammaOTP_LGOTP_IDOTP();
            Thread.Sleep(30);
            Read_and_Show_CMOTP_GammaOTP_LGOTP_IDOTP_CRC();
        }

        public void Read_From_Flash_and_Show()
        {
            Unlock_Read_After_Sleepout_Done_Protection();
            Read_For_CMOTP_GammaOTP_LGOTP_IDOTP();
            Thread.Sleep(30);
            Read_and_Show_CMOTP_GammaOTP_LGOTP_IDOTP_CRC();
        }

        private void Unlock_Read_After_Sleepout_Done_Protection()
        {
            f1().DP173_One_Param_CMD_Send(5, "E8", "11");//Unlock "Read After Sleepout Done" Protection
        }

        private void Read_For_CMOTP_GammaOTP_LGOTP_IDOTP()
        {
            f1().DP173_One_Param_CMD_Send(4, "E8", "00");
            f1().DP173_One_Param_CMD_Send(4, "E8", "11");
            f1().DP173_One_Param_CMD_Send(4, "E8", "00");
            f1().DP173_One_Param_CMD_Send(4, "E8", "12");
            f1().DP173_One_Param_CMD_Send(4, "E8", "00");
            f1().DP173_One_Param_CMD_Send(4, "E8", "13");
            f1().DP173_One_Param_CMD_Send(4, "E8", "00");
            f1().DP173_One_Param_CMD_Send(4, "E8", "14");
        }

        public void OD_Flash_Erase_Write_and_CRC_Check()
        {
            Initializing_For_OD_ERA_Flash_Write();

            dp213_form().Mipi_Script_Send(dp213_form().textBox_OD_Script);

            OD_Flash_Erase_Write_Verify();

            bool OD_CRC_OK = IS_OD_Flash_CRC_OK();

            if (OD_CRC_OK)
                f1().Show_OK_Message("OD Flash CRC Check OK");
            else
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("OD Flash CRC Check NG");
        }



    }
}
