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
    public interface CRC_Common_Methods
    {
        void Initialize_Vriables();
        int Get_Changed_Decimal_OTP_Sum();
        void Show_Changed_Flags_Status();
    }

    public class DP213_CRC_Check : DP213_forms_accessor
    {
        DP213_FlashMemory_For_User flashmemory;
        CMOTP_OC_Vriables CM;
        GammaOTP_OC_Variables Gamma;
        int CRC_Check_Retry_Count;
        //LGOTP_OC_Variables LG;
        //IDOTP_OC_Variables ID;

        //Singleton
        private static DP213_CRC_Check instance;
        private DP213_CRC_Check(Gamma_Set OC_Mode1_GammaSet)
        {
            flashmemory = DP213_FlashMemory.getInstance();
            CM = new CMOTP_OC_Vriables();
            Gamma = new GammaOTP_OC_Variables(OC_Mode1_GammaSet);
            //LG = new LGOTP_OC_Variables();
            //ID = new IDOTP_OC_Variables();
            Initialize_Vriables();
        }

        public static DP213_CRC_Check getInstance(Gamma_Set OC_Mode1_GammaSet)
        {
            if (instance == null)
                instance = new DP213_CRC_Check(OC_Mode1_GammaSet);

            return instance;
        }

        public void Initialize_Vriables()
        {
            CM.Initialize_Vriables();
            Gamma.Initialize_Vriables();
            //LG.Initialize_Vriables();
            //ID.Initialize_Vriables();
            CRC_Check_Retry_Count = 0;
        }

        public void Show_Changed_Flags_Status()
        {
            CM.Show_Changed_Flags_Status();
            Gamma.Show_Changed_Flags_Status();
            //LG.Show_Flags_Status();
            //ID.Show_Flags_Status();
        }

        public void Show_CMOTP_GammaOTP_Result(Gamma_Set OC_Mode1_GammaSet)
        {
            int Dec_Changed_CMOTP_Sum = CM.Get_Changed_Decimal_OTP_Sum();
            int Dec_Changed_GammaOTP_Sum = Gamma.Get_Changed_Decimal_OTP_Sum();
            f1().GB_Status_AppendText_Nextline("Dec_Changed_CMOTP_Sum : " + Dec_Changed_CMOTP_Sum.ToString(), Color.DarkRed);
            f1().GB_Status_AppendText_Nextline("Dec_Changed_GammaOTP_Sum : " + Dec_Changed_GammaOTP_Sum.ToString(), Color.DarkGreen);

            flashmemory.Read_From_Frame_and_Show();
            int Dec_CMOTP_Origianl_1 = flashmemory.Get_Dec_CMOTP();
            f1().GB_Status_AppendText_Nextline("Dec_CMOTP_Origianl_1 : " + Dec_CMOTP_Origianl_1.ToString(), Color.Red);
            int Dec_GammaOTP_Origianl_1 = flashmemory.Get_Dec_GammaOTP();
            f1().GB_Status_AppendText_Nextline("Dec_GammaOTP_Origianl_1 : " + Dec_GammaOTP_Origianl_1.ToString(), Color.Green);

            CM.Send_CM_CMDs_As_0x00s();
            Gamma.Send_Gamma_CMDs_As_0x00(OC_Mode1_GammaSet);
            flashmemory.Read_From_Frame_and_Show();
            int Dec_CMOTP_0x00s = flashmemory.Get_Dec_CMOTP();
            f1().GB_Status_AppendText_Nextline("Dec_CMOTP_0x00s : " + Dec_CMOTP_0x00s.ToString(), Color.Red);
            int Dec_GammaOTP_0x00s = flashmemory.Get_Dec_GammaOTP();
            f1().GB_Status_AppendText_Nextline("Dec_GammaOTP_0x00s : " + Dec_GammaOTP_0x00s.ToString(), Color.Green);

            CM.Send_CM_CMDs_As_Original_Values();
            Gamma.Send_Gamma_CMDs_As_Original_Values(OC_Mode1_GammaSet);
            flashmemory.Read_From_Frame_and_Show();
            int Dec_CMOTP_Origianl_2 = flashmemory.Get_Dec_CMOTP();
            f1().GB_Status_AppendText_Nextline("Dec_CMOTP_Origianl_2 : " + Dec_CMOTP_Origianl_2.ToString(), Color.Red);
            int Dec_GammaOTP_Origianl_2 = flashmemory.Get_Dec_GammaOTP();
            f1().GB_Status_AppendText_Nextline("Dec_GammaOTP_Origianl_2 : " + Dec_GammaOTP_Origianl_2.ToString(), Color.Green);

            f1().GB_Status_AppendText_Nextline("(Dec_CMOTP_Origianl_2 - Dec_CMOTP_0x00s) : " + (Dec_CMOTP_Origianl_2 - Dec_CMOTP_0x00s).ToString(), Color.DarkRed);
            f1().GB_Status_AppendText_Nextline("(Dec_GammaOTP_Origianl_2 - Dec_GammaOTP_0x00s) : " + (Dec_GammaOTP_Origianl_2 - Dec_GammaOTP_0x00s).ToString(), Color.DarkGreen);

            bool Is_CRC_OK = false;
            Is_CRC_OK = Show_CMOTP_CRC_Check_Result(Dec_Changed_CMOTP_Sum, Dec_CMOTP_Origianl_1, Dec_CMOTP_0x00s, Dec_CMOTP_Origianl_2);
            Is_CRC_OK = Show_GammaOTP_CRC_Check_Result(Dec_Changed_GammaOTP_Sum, Dec_GammaOTP_Origianl_1, Dec_GammaOTP_0x00s, Dec_GammaOTP_Origianl_2);

            if (Is_CRC_OK == false && CRC_Check_Retry_Count <= 5)
            {
                CRC_Check_Retry_Count++;
                f1().GB_Status_AppendText_Nextline("CRC Check NG, Retry " + CRC_Check_Retry_Count.ToString(), Color.Red);
                Show_CMOTP_GammaOTP_Result(OC_Mode1_GammaSet);
            }
        }

        private bool Show_CMOTP_CRC_Check_Result(int Dec_Changed_CMOTP_Sum, int Dec_CMOTP_Origianl_1, int Dec_CMOTP_0x00s, int Dec_CMOTP_Origianl_2)
        {
            if (Dec_CMOTP_Origianl_2 == Dec_CMOTP_Origianl_1)
            {
                string Program_Hex_Changed_CMOTP_Sum = (Dec_Changed_CMOTP_Sum & 0xFFFF).ToString("X4");
                string Read_Hex_Changed_CMOTP_Sum = ((Dec_CMOTP_Origianl_2 - Dec_CMOTP_0x00s)&0xFFFF).ToString("X4");
                f1().GB_Status_AppendText_Nextline("Program_Hex_Changed_CMOTP_Sum / Read_Hex_Changed_CMOTP_Sum : " + Program_Hex_Changed_CMOTP_Sum + " / " + Read_Hex_Changed_CMOTP_Sum, Color.DarkRed);

                if (Program_Hex_Changed_CMOTP_Sum == Read_Hex_Changed_CMOTP_Sum)
                {
                    f1().Show_OK_Message("Frame)CMOTP CRC Check OK");
                    return true;
                }
                else
                {
                    f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("Frame)CMOTP CRC Check NG");
                    return false;
                }
            }
            else
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("Frame)CMOTP CRC Check NG");
                return false;
            }
        }


        private bool Show_GammaOTP_CRC_Check_Result(int Dec_Changed_GammaOTP_Sum, int Dec_GammaOTP_Origianl_1, int Dec_GammaOTP_0x00s, int Dec_GammaOTP_Origianl_2)
        {
            if (Dec_GammaOTP_Origianl_2 == Dec_GammaOTP_Origianl_1)
            {
                string Program_Hex_Changed_GammaOTP_Sum = (Dec_Changed_GammaOTP_Sum & 0xFFFF).ToString("X4");
                string Read_Hex_Changed_GammaOTP_Sum = ((Dec_GammaOTP_Origianl_2 - Dec_GammaOTP_0x00s) & 0xFFFF).ToString("X4");
                f1().GB_Status_AppendText_Nextline("Program_Hex_Changed_GammaOTP_Sum / Read_Hex_Changed_GammaOTP_Sum : " + Program_Hex_Changed_GammaOTP_Sum + " / " + Read_Hex_Changed_GammaOTP_Sum, Color.DarkRed);

                if (Program_Hex_Changed_GammaOTP_Sum == Read_Hex_Changed_GammaOTP_Sum)
                {
                    f1().Show_OK_Message("Frame)GammaOTP CRC Check OK");
                    return true;
                }
                else
                {
                    f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("Frame)GammaOTP CRC Check NG");
                    return false;
                }
            }
            else
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("Frame)GammaOTP CRC Check NG");
                return false;
            }
        }


        //CM
        public void Set_GammaSet_ELVSS_Hex_Param(Gamma_Set Set, string[] Hex_ELVSS_Param)
        {
            CM.Set_GammaSet_ELVSS_Hex_Param(Convert.ToInt16(Set), Hex_ELVSS_Param);
        }

        public void Set_GammaSet_Vinit2_Hex_Param(Gamma_Set Set, string[] Hex_Vinit2_Param)
        {
            CM.Set_GammaSet_Vinit2_Hex_Param(Convert.ToInt16(Set), Hex_Vinit2_Param);
        }

        public void Set_GammaSet_Cold_ELVSS_Hex_Param(Gamma_Set Set, string[] Hex_Cold_ELVSS_Param)
        {
            CM.Set_GammaSet_Cold_ELVSS_Hex_Param(Convert.ToInt16(Set), Hex_Cold_ELVSS_Param);
        }

        public void Set_GammaSet_Cold_Vinit2_Hex_Param(Gamma_Set Set, string[] Hex_Cold_Vinit2_Param)
        {
            CM.Set_GammaSet_Cold_Vinit2_Hex_Param(Convert.ToInt16(Set), Hex_Cold_Vinit2_Param);
        }

         //GM
         public void Set_GammaSet_Normal_RGB_Hex_Param(Gamma_Set Set,int band_index, string[] Hex_45ea_RGB_Params)
         {
             Gamma.Set_GammaSet_Normal_RGB_Hex_Param(Set, band_index, Hex_45ea_RGB_Params);
         }

         public void Set_AOD_RGB_Hex_Param(int band_index, string[] Hex_45ea_RGB_Params)
         {
             Gamma.Set_AOD_RGB_Hex_Param(band_index, Hex_45ea_RGB_Params);
         }

         public void Set_VREF0_VREF4095_Hex_Param(string Hex_Param_REF0, string Hex_Param_REF4095)
         {
             Gamma.Set_VREF0_VREF4095_Hex_Param(Hex_Param_REF0, Hex_Param_REF4095);
         }

         public void Set_Normal_GammaSet_Vreg1_Hex_Params(Gamma_Set Set, string[] Hex_Params)
         {
             Gamma.Set_Normal_GammaSet_Vreg1_Hex_Params(Set, Hex_Params);
         }
    }


    public class CMOTP_OC_Vriables : DP213_forms_accessor, CRC_Common_Methods
    {
        //Initialize
        public void Initialize_Vriables()
        {
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                {
                    GammaSet_ELVSS_Hex_Param[Set_index, band_index] = "00";
                    GammaSet_Vinit2_Hex_Param[Set_index, band_index] = "00";
                }
                Is_GammaSet_ELVSS_Changed[Set_index] = false;
                Is_GammaSet_Vinit2_Changed[Set_index] = false;
            }
        }

        public void Show_Changed_Flags_Status()
        {
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                Show_ELVSS_Changed_Flags_Status(Set_index);
                Show_Vinit2_Changed_Flags_Status(Set_index);
                Show_Cold_ELVSS_Changed_Flags_Status(Set_index);
                Show_Cold_Vinit2_Changed_Flags_Status(Set_index);
            }
        }

        public int Get_Changed_Decimal_OTP_Sum()
        {
            int Decimal_Sum = 0;
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                {
                    if (Is_GammaSet_ELVSS_Changed[Set_index]) Decimal_Sum += Convert.ToInt16(GammaSet_ELVSS_Hex_Param[Set_index, band_index], 16);
                    if (Is_GammaSet_Vinit2_Changed[Set_index]) Decimal_Sum += Convert.ToInt16(GammaSet_Vinit2_Hex_Param[Set_index, band_index], 16);
                    if (Is_GammaSet_Cold_ELVSS_Changed[Set_index]) Decimal_Sum += Convert.ToInt16(GammaSet_Cold_ELVSS_Hex_Param[Set_index, band_index], 16);
                    if (Is_GammaSet_Cold_Vinit2_Changed[Set_index]) Decimal_Sum += Convert.ToInt16(GammaSet_Cold_Vinit2_Hex_Param[Set_index, band_index], 16);
                }
            }
            return Decimal_Sum;
        }

        //Normal Set1~Set6 ELVSS (Band0 ~ Band11 : 12ea Bands)
        string[,] GammaSet_ELVSS_Hex_Param = new string[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        bool[] Is_GammaSet_ELVSS_Changed = new bool[DP213_Static.Max_Set_Amount];
        public void Set_GammaSet_ELVSS_Hex_Param(int Set_index, string[] Hex_ELVSS_Param)
        {
            for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                GammaSet_ELVSS_Hex_Param[Set_index, band_index] = Hex_ELVSS_Param[band_index];

            Is_GammaSet_ELVSS_Changed[Set_index] = true;
        }
        void Show_ELVSS_Changed_Flags_Status(int Set_index)
        {
            if (Is_GammaSet_ELVSS_Changed[Set_index])
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_GammaSet_ELVSS_Changed : " + Is_GammaSet_ELVSS_Changed[Set_index].ToString(), Color.Blue);
            else
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_GammaSet_ELVSS_Changed : " + Is_GammaSet_ELVSS_Changed[Set_index].ToString(), Color.Red);
        }

        //Normal Set1~Set6 ELVSS (Band0 ~ Band11 : 12ea Bands)
        string[,] GammaSet_Cold_ELVSS_Hex_Param = new string[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        bool[] Is_GammaSet_Cold_ELVSS_Changed = new bool[DP213_Static.Max_Set_Amount];
        public void Set_GammaSet_Cold_ELVSS_Hex_Param(int Set_index, string[] Hex_Cold_ELVSS_Param)
        {
            for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                GammaSet_Cold_ELVSS_Hex_Param[Set_index, band_index] = Hex_Cold_ELVSS_Param[band_index];

            Is_GammaSet_Cold_ELVSS_Changed[Set_index] = true;
        }
        void Show_Cold_ELVSS_Changed_Flags_Status(int Set_index)
        {
            if (Is_GammaSet_Cold_ELVSS_Changed[Set_index])
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_GammaSet_Cold_ELVSS_Changed : " + Is_GammaSet_Cold_ELVSS_Changed[Set_index].ToString(), Color.Blue);
            else
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_GammaSet_Cold_ELVSS_Changed : " + Is_GammaSet_Cold_ELVSS_Changed[Set_index].ToString(), Color.Red);
        }


        //Normal Set1~Set6 Vinit2 (Band0 ~ Band11 : 12ea Bands)
        string[,] GammaSet_Vinit2_Hex_Param = new string[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        bool[] Is_GammaSet_Vinit2_Changed = new bool[DP213_Static.Max_Set_Amount];
        public void Set_GammaSet_Vinit2_Hex_Param(int Set_index, string[] Hex_Vinit2_Param)
        {
            for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                GammaSet_Vinit2_Hex_Param[Set_index, band_index] = Hex_Vinit2_Param[band_index];

            Is_GammaSet_Vinit2_Changed[Set_index] = true;
        }
        void Show_Vinit2_Changed_Flags_Status(int Set_index)
        {
            if (Is_GammaSet_Vinit2_Changed[Set_index])
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_GammaSet_Vinit2_Changed : " + Is_GammaSet_Vinit2_Changed[Set_index].ToString(), Color.Blue);
            else
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_GammaSet_Vinit2_Changed : " + Is_GammaSet_Vinit2_Changed[Set_index].ToString(), Color.Red);
        }


        //Normal Set1~Set6 Vinit2 (Band0 ~ Band11 : 12ea Bands)
        string[,] GammaSet_Cold_Vinit2_Hex_Param = new string[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        bool[] Is_GammaSet_Cold_Vinit2_Changed = new bool[DP213_Static.Max_Set_Amount];
        public void Set_GammaSet_Cold_Vinit2_Hex_Param(int Set_index, string[] Hex_Cold_Vinit2_Param)
        {
            for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                GammaSet_Cold_Vinit2_Hex_Param[Set_index, band_index] = Hex_Cold_Vinit2_Param[band_index];

            Is_GammaSet_Cold_Vinit2_Changed[Set_index] = true;
        }
        void Show_Cold_Vinit2_Changed_Flags_Status(int Set_index)
        {
            if (Is_GammaSet_Cold_Vinit2_Changed[Set_index])
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_GammaSet_Cold_Vinit2_Changed : " + Is_GammaSet_Cold_Vinit2_Changed[Set_index].ToString(), Color.Blue);
            else
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_GammaSet_Cold_Vinit2_Changed : " + Is_GammaSet_Cold_Vinit2_Changed[Set_index].ToString(), Color.Red);
        }

        public void Send_CM_CMDs_As_0x00s()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                if (Is_GammaSet_ELVSS_Changed[Set_index]) cmds.Send_ELVSS_0x00s_CMD(Set);
                if (Is_GammaSet_Cold_ELVSS_Changed[Set_index]) cmds.Send_Cold_ELVSS_0x00s_CMD(Set);
                if (Is_GammaSet_Vinit2_Changed[Set_index]) cmds.Send_Vinit2_0x00s_CMD(Set);
                if (Is_GammaSet_Cold_Vinit2_Changed[Set_index]) cmds.Send_Cold_Vinit2_0x00s_CMD(Set);
            }
        }

        public void Send_CM_CMDs_As_Original_Values()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                if (Is_GammaSet_ELVSS_Changed[Set_index]) cmds.Send_ELVSS_CMD(Set);
                if (Is_GammaSet_Cold_ELVSS_Changed[Set_index]) cmds.Send_Cold_ELVSS_CMD(Set);
                if (Is_GammaSet_Vinit2_Changed[Set_index]) cmds.Send_Vinit2_CMD(Set);
                if (Is_GammaSet_Cold_Vinit2_Changed[Set_index]) cmds.Send_Cold_Vinit2_CMD(Set);
            }
        }
    }

   public class GammaOTP_OC_Variables : DP213_forms_accessor, CRC_Common_Methods
    {
        Normal_Gamma_CRC Normal_Gamma;
        AOD_Gamma_CRC AOD_Gamma;
        REF0_REF4095_NormalGammaSetVreg1 Ref_Params;

        public GammaOTP_OC_Variables(Gamma_Set AOD_Gamma_Set)
        {
            Normal_Gamma = new Normal_Gamma_CRC();
            AOD_Gamma = new AOD_Gamma_CRC();
            Ref_Params = new REF0_REF4095_NormalGammaSetVreg1();
        }

        public int Get_Changed_Decimal_OTP_Sum()
        {
            int Decimal_Sum = 0;

            Decimal_Sum += Normal_Gamma.Get_Changed_Decimal_OTP_Sum();
            Decimal_Sum += AOD_Gamma.Get_Changed_Decimal_OTP_Sum();
            Decimal_Sum += Ref_Params.Get_Changed_Decimal_OTP_Sum();

            return Decimal_Sum;
        }

        public void Initialize_Vriables()
        {
            Normal_Gamma.Initialize_Vriables();
            AOD_Gamma.Initialize_Vriables();
            Ref_Params.Initialize_Vriables();
        }

        public void Show_Changed_Flags_Status()
        {
            Normal_Gamma.Show_Changed_Flags_Status();
            AOD_Gamma.Show_Changed_Flags_Status();
            Ref_Params.Show_Changed_Flags_Status();
        }

        public void Send_Gamma_CMDs_As_0x00(Gamma_Set OC_Mode1_GammaSet)
        {
            Normal_Gamma.Send_CMDs_Normal_RGB_AM0_AM1_As_0x00();
            AOD_Gamma.Send_CMDs_AOD_RGB_AM0_AM1_As_0x00(OC_Mode1_GammaSet);
            Ref_Params.Send_Changed_CMDs_REF0_REF4095_Vreg1_As_0x00s();
        }

        public void Send_Gamma_CMDs_As_Original_Values(Gamma_Set OC_Mode1_GammaSet)
        {
            Normal_Gamma.Send_CMDs_Normal_RGB_AM0_AM1_As_Original_Values();
            AOD_Gamma.Send_CMDs_AOD_RGB_AM0_AM1_As_Original_Values(OC_Mode1_GammaSet);
            Ref_Params.Send_Changed_CMDs_REF0_REF4095_Vreg1_As_Original_Values();
        }


        public void Set_GammaSet_Normal_RGB_Hex_Param(Gamma_Set Set, int band_index, string[] Hex_45ea_RGB_Params)
        {
            Normal_Gamma.Set_GammaSet_Normal_RGB_Hex_Param(Set, band_index, Hex_45ea_RGB_Params);
        }

        public void Set_AOD_RGB_Hex_Param(int band_index, string[] Hex_45ea_RGB_Params)
        {
            AOD_Gamma.Set_AOD_RGB_Hex_Param(band_index, Hex_45ea_RGB_Params);
        }

        public void Set_VREF0_VREF4095_Hex_Param(string Hex_Param_REF0, string Hex_Param_REF4095)
        {
            Ref_Params.Set_VREF0_VREF4095_Hex_Param(Hex_Param_REF0, Hex_Param_REF4095);
        }


        public void Set_Normal_GammaSet_Vreg1_Hex_Params(Gamma_Set Set, string[] Hex_Params)
        {
            Ref_Params.Set_Normal_GammaSet_Vreg1_Hex_Params(Set, Hex_Params);
        }
    }

   public class REF0_REF4095_NormalGammaSetVreg1 : DP213_forms_accessor, CRC_Common_Methods
    {
        public void Initialize_Vriables()
        {
            Initialze_REF0_and_REF4095();
            Initial_Normal_GammaSet_Vreg1();
        }

        public int Get_Changed_Decimal_OTP_Sum()
        {
            int Decimal_Sum = 0;

            if (Is_VREF0_or_VREF4095_Changed)
            {
                Decimal_Sum += Convert.ToInt16(VREF0_Hex_Param, 16);
                Decimal_Sum += Convert.ToInt16(VREF4095_Hex_Param, 16);
            }
            
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                if (Is_Normal_GammaSet_Vreg1_Changed[Set_index])
                {
                    for (int Param_index = 0; Param_index < DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount; Param_index++)
                        Decimal_Sum += Convert.ToInt16(Normal_GammaSet_Vreg1_Hex_Params[Set_index, Param_index], 16);
                }
            }

            return Decimal_Sum;
        }

        public void Show_Changed_Flags_Status()
        {
            if (Is_VREF0_or_VREF4095_Changed)
                f1().GB_Status_AppendText_Nextline("Is_VREF0_or_VREF4095_Changed : " + Is_VREF0_or_VREF4095_Changed.ToString(), Color.Blue);
            else
                f1().GB_Status_AppendText_Nextline("Is_VREF0_or_VREF4095_Changed : " + Is_VREF0_or_VREF4095_Changed.ToString(), Color.Red);

            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                if (Is_Normal_GammaSet_Vreg1_Changed[Set_index])
                    f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_Normal_GammaSet_Vreg1_Changed : " + Is_Normal_GammaSet_Vreg1_Changed[Set_index].ToString(), Color.Blue);
                else
                    f1().GB_Status_AppendText_Nextline(Set_index.ToString() + ")Is_Normal_GammaSet_Vreg1_Changed : " + Is_Normal_GammaSet_Vreg1_Changed[Set_index].ToString(), Color.Red);
            }
        }

        public void Send_Changed_CMDs_REF0_REF4095_Vreg1_As_0x00s()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();

            if (Is_VREF0_or_VREF4095_Changed)
                cmds.Send_0x00s_VREF0_VREF4095();

            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                if (Is_Normal_GammaSet_Vreg1_Changed[Set_index])
                {
                    Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                    cmds.Send_0x00s_Vreg1(Set);
                }
            }
        }


        public void Send_Changed_CMDs_REF0_REF4095_Vreg1_As_Original_Values()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();

            if (Is_VREF0_or_VREF4095_Changed)
                cmds.Send_VREF0_VREF4095();

            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                if (Is_Normal_GammaSet_Vreg1_Changed[Set_index])
                {
                    Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                    cmds.Send_Vreg1(Set);
                }
            }
        }


        private void Initialze_REF0_and_REF4095()
        {
            VREF0_Hex_Param = "00";
            VREF4095_Hex_Param = "00";
            Is_VREF0_or_VREF4095_Changed = false;
        }


        private void Initial_Normal_GammaSet_Vreg1()
        {
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                for (int Param_index = 0; Param_index < DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount; Param_index++)
                {
                    Normal_GammaSet_Vreg1_Hex_Params[Set_index, Param_index] = "00";
                }
                Is_Normal_GammaSet_Vreg1_Changed[Set_index] = false;
            }
        }

        //REF0 and REF4095
        string VREF0_Hex_Param;
        string VREF4095_Hex_Param;
        bool Is_VREF0_or_VREF4095_Changed;
        public void Set_VREF0_VREF4095_Hex_Param(string Hex_Param_REF0, string Hex_Param_REF4095)
        {

            VREF0_Hex_Param = Hex_Param_REF0;
            VREF4095_Hex_Param = Hex_Param_REF4095;
            Is_VREF0_or_VREF4095_Changed = true;
        }

        //Vreg1
        string[,] Normal_GammaSet_Vreg1_Hex_Params = new string[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount];
        bool[] Is_Normal_GammaSet_Vreg1_Changed = new bool[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        public void Set_Normal_GammaSet_Vreg1_Hex_Params(Gamma_Set Set, string[] Hex_Params)
        {
            int Set_index = Convert.ToInt16(Set);
            
            for (int Param_index = 0; Param_index < DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount; Param_index++)
            {
                Normal_GammaSet_Vreg1_Hex_Params[Set_index, Param_index] = Hex_Params[Param_index];
            }
            Is_Normal_GammaSet_Vreg1_Changed[Set_index] = true;
        }
    }


   public class Normal_Gamma_CRC : DP213_forms_accessor, CRC_Common_Methods
    {
        private string[, ,] GammaSet_Normal_RGB_Hex_Param = new string[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.One_Band_Gamma_Parameters_Amount];
        private bool[,] Is_GammaSet_Normal_RGB_Changed = new bool[DP213_Static.Max_Set_Amount, DP213_Static.Max_HBM_and_Normal_Band_Amount];
        public void Set_GammaSet_Normal_RGB_Hex_Param(Gamma_Set Set, int band_index, string[] Hex_45ea_RGB_Params)
        {
            int Set_index = Convert.ToInt16(Set);

            for (int Param_index = 0; Param_index < DP213_Static.One_Band_Gamma_Parameters_Amount; Param_index++)
                GammaSet_Normal_RGB_Hex_Param[Set_index, band_index, Param_index] = Hex_45ea_RGB_Params[Param_index];

            Is_GammaSet_Normal_RGB_Changed[Set_index, band_index] = true;
        }


        public int Get_Changed_Decimal_OTP_Sum()
        {
            int Decimal_Sum = 0;

            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                {
                    if (Is_GammaSet_Normal_RGB_Changed[Set_index, band_index])
                    {
                        for (int param_index = 0; param_index < DP213_Static.One_Band_Gamma_Parameters_Amount; param_index++)
                            Decimal_Sum += Convert.ToInt16(GammaSet_Normal_RGB_Hex_Param[Set_index, band_index, param_index], 16);
                    }
                }
            }
            return Decimal_Sum;
        }

        public void Send_CMDs_Normal_RGB_AM0_AM1_As_0x00()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();

            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                {
                    if (Is_GammaSet_Normal_RGB_Changed[Set_index, band_index])
                        cmds.Send_Gamma_AM1_AM0_As_0x00s(Set, band_index);
                }
            }
        }

        public void Send_CMDs_Normal_RGB_AM0_AM1_As_Original_Values()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();

            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                {
                    if (Is_GammaSet_Normal_RGB_Changed[Set_index, band_index])
                        cmds.Send_Gamma_AM1_AM0(Set, band_index);
                }
            }
        }

        public void Initialize_Vriables()
        {
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                {
                    for (int param_index = 0; param_index < DP213_Static.One_Band_Gamma_Parameters_Amount; param_index++)
                    {
                        GammaSet_Normal_RGB_Hex_Param[Set_index, band_index, param_index] = "00";
                    }
                    Is_GammaSet_Normal_RGB_Changed[Set_index, band_index] = false;
                }
            }
        }

        public void Show_Changed_Flags_Status()
        {
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                {
                    Show_Flags_Status(Set_index, band_index);
                }
            }
        }

        private void Show_Flags_Status(int Set_index, int band_index)
        {
            if (Is_GammaSet_Normal_RGB_Changed[Set_index, band_index])
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + "," + band_index.ToString() + ")Is_GammaSet_Normal_RGB_Changed : " + Is_GammaSet_Normal_RGB_Changed[Set_index, band_index].ToString(), Color.Blue);
            else
                f1().GB_Status_AppendText_Nextline(Set_index.ToString() + "," + band_index.ToString() + ")Is_GammaSet_Normal_RGB_Changed : " + Is_GammaSet_Normal_RGB_Changed[Set_index, band_index].ToString(), Color.Red);
        }       
    }

  public  class AOD_Gamma_CRC : DP213_forms_accessor, CRC_Common_Methods
    {
        private string[,] AOD_RGB_Hex_Param = new string[DP213_Static.Max_AOD_Band_Amount, DP213_Static.One_Band_Gamma_Parameters_Amount];
        private bool[] Is_AOD_RGB_Changed = new bool[DP213_Static.Max_AOD_Band_Amount];
        public void Set_AOD_RGB_Hex_Param(int band_index, string[] Hex_45ea_RGB_Params)
        {
            for (int Param_index = 0; Param_index < DP213_Static.One_Band_Gamma_Parameters_Amount; Param_index++)
                AOD_RGB_Hex_Param[band_index, Param_index] = Hex_45ea_RGB_Params[Param_index];

            Is_AOD_RGB_Changed[band_index] = true;
        }

        private void Show_Flags_Status(int band_index)
        {
            if (Is_AOD_RGB_Changed[band_index])
                f1().GB_Status_AppendText_Nextline(band_index.ToString() + ")Is_AOD_RGB_Changed : " + Is_AOD_RGB_Changed[band_index].ToString(), Color.Blue);
            else
                f1().GB_Status_AppendText_Nextline(band_index.ToString() + ")Is_AOD_RGB_Changed : " + Is_AOD_RGB_Changed[band_index].ToString(), Color.Red);
        }

        public int Get_Changed_Decimal_OTP_Sum()
        {
            int Decimal_Sum = 0;

            for (int band_index = 0; band_index < DP213_Static.Max_AOD_Band_Amount; band_index++)
            {
                if (Is_AOD_RGB_Changed[band_index])
                {
                    for (int param_index = 0; param_index < DP213_Static.One_Band_Gamma_Parameters_Amount; param_index++)
                        Decimal_Sum += Convert.ToInt16(AOD_RGB_Hex_Param[band_index, param_index], 16);
                }
            }
            return Decimal_Sum;
        }

        public void Send_CMDs_AOD_RGB_AM0_AM1_As_0x00(Gamma_Set OC_Mode1_GammaSet)
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            for (int band_index = DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index < DP213_Static.Max_Band_Amount; band_index++)
            {
                if (Is_AOD_RGB_Changed[band_index - DP213_Static.Max_HBM_and_Normal_Band_Amount])
                    cmds.Send_Gamma_AM1_AM0_As_0x00s(OC_Mode1_GammaSet, band_index);
            }
        }

        public void Send_CMDs_AOD_RGB_AM0_AM1_As_Original_Values(Gamma_Set OC_Mode1_GammaSet)
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            for (int band_index = DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index < DP213_Static.Max_Band_Amount; band_index++)
            {
                if (Is_AOD_RGB_Changed[band_index - DP213_Static.Max_HBM_and_Normal_Band_Amount])
                    cmds.Send_Gamma_AM1_AM0(OC_Mode1_GammaSet, band_index);
            }
        }

        public void Initialize_Vriables()
        {
            for (int band_index = 0; band_index < DP213_Static.Max_AOD_Band_Amount; band_index++)
            {
                for (int param_index = 0; param_index < DP213_Static.One_Band_Gamma_Parameters_Amount; param_index++)
                {
                    AOD_RGB_Hex_Param[band_index, param_index] = "00";
                }
                Is_AOD_RGB_Changed[band_index] = false;
            }
        }

        public void Show_Changed_Flags_Status()
        {
            for (int band_index = 0; band_index < DP213_Static.Max_AOD_Band_Amount; band_index++)
            {
                Show_Flags_Status(band_index);
            }
        }
    }

   public class LGOTP_OC_Variables : DP213_forms_accessor, CRC_Common_Methods
    {
        public void Initialize_Vriables()
        {
            throw new NotImplementedException();
        }

        public int Get_Changed_Decimal_OTP_Sum()
        {
            throw new NotImplementedException();
        }

        public void Show_Changed_Flags_Status()
        {
            throw new NotImplementedException();
        }
    }

    public class IDOTP_OC_Variables : DP213_forms_accessor, CRC_Common_Methods
    {
        public void Initialize_Vriables()
        {
            throw new NotImplementedException();
        }

        public int Get_Changed_Decimal_OTP_Sum()
        {
            throw new NotImplementedException();
        }

        public void Show_Changed_Flags_Status()
        {
            throw new NotImplementedException();
        }
    }
}
