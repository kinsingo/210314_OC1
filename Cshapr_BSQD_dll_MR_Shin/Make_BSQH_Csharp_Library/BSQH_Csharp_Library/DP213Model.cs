using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices; // (Dll 사용위해 필요)

namespace BSQH_Csharp_Library
{
    public class DP213Model : Model
    {
        private static DP213Model instance;
        private DP213Model(){}
        public static DP213Model getInstance()
        {
            if (instance == null)
                instance = new DP213Model();
            return instance;
        }

        public byte[][] Get_ELVSS_CMD(Gamma_Set Set, double[] ELVSS_Voltage)
        {
            return new DP213_ELVSS(Set, ELVSS_Voltage).Get_ELVSS_CMD();
        }

        public byte[][] Get_Cold_ELVSS_CMD(Gamma_Set Set, double[] Cold_ELVSS_Voltage)
        {
            return new DP213_ELVSS(Set, Cold_ELVSS_Voltage).Get_Cold_ELVSS_CMD();
        }

        public byte[][] Get_Vinit2_CMD(Gamma_Set Set, double[] Vinit2_Voltage)
        {
            return new DP213_Vinit2(Set, Vinit2_Voltage).Get_Vinit2_CMD();
        }

        public byte[][] Get_Cold_Vinit2_CMD(Gamma_Set Set, double[] Cold_Vinit2_Voltage)
        {
            return new DP213_Vinit2(Set, Cold_Vinit2_Voltage).Get_Cold_Vinit2_CMD();
        }

        public byte[][] Get_ELVSS_0x00_CMD(Gamma_Set Set)
        {
            return new DP213_ELVSS(Set).Get_ELVSS_0x00_CMD();
        }

        public byte[][] Get_Cold_ELVSS_0x00_CMD(Gamma_Set Set)
        {
            return new DP213_ELVSS(Set).Get_Cold_ELVSS_0x00_CMD();
        }

        public byte[][] Get_Vinit2_0x00_CMD(Gamma_Set Set)
        {
            return new DP213_Vinit2(Set).Get_Vinit2_0x00_CMD();
        }

        public byte[][] Get_Cold_Vinit2_0x00_CMD(Gamma_Set Set)
        {
            return new DP213_Vinit2(Set).Get_Cold_Vinit2_0x00_CMD();
        }

        public byte[][] Get_Vreg1_CMD(Gamma_Set Set, int[] Dec_Normal_Vreg1)
        {
            return new DP213_Vreg1(Set, Dec_Normal_Vreg1).Get_Vreg1_CMD();
        }

        public byte[][] Get_Vreg1_0x00_CMD(Gamma_Set Set)
        {
            return new DP213_Vreg1(Set).Get_Vreg1_0x00_CMD();
        }

        public byte[][] Get_REF4095_REF0_CMD(byte REF4095, byte REF0)
        {
            byte Start_Index = 19;
            byte Address = Convert.ToByte("B1", 16);
            byte[] REF4095_REF0 = { REF4095, REF0};
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, REF4095_REF0);
        }

        public byte[][] Get_REF4095_REF0_0x00_CMD()
        {
            byte Start_Index = 19;
            byte Address = Convert.ToByte("B1", 16);
            byte[] REF4095_REF0 = { 0, 0 };
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, REF4095_REF0);
        }

        public byte[][] Get_Gamma_AM1_AM0_CMD(Gamma_Set Set,int band,int[] Band_Set_Gamma_Red,int[] Band_Set_Gamma_Green,int[] Band_Set_Gamma_Blue,
            int AM1_Red,int AM1_Green,int AM1_Blue,int AM0_Red,int AM0_Green,int AM0_Blue)
        {
            return new DP213_Gamma(Set, band, Band_Set_Gamma_Red, Band_Set_Gamma_Green, Band_Set_Gamma_Blue, AM1_Red, AM1_Green, AM1_Blue, AM0_Red, AM0_Green, AM0_Blue).Get_Gamma_AM1_AM0_CMD();
        }

        public byte[][] Get_Gamma_AM1_AM0_CMD(Gamma_Set Set,int band,RGB[] Band_Set_Gamma, RGB AM1, RGB AM0)
        {
            return new DP213_Gamma(Set, band, Band_Set_Gamma, AM1, AM0).Get_Gamma_AM1_AM0_CMD();
        }

        public byte[][] Get_Gamma_AM1_AM0_0x00_CMD(Gamma_Set Set, int band)
        {
            return new DP213_Gamma(Set, band).Get_Gamma_AM1_AM0_0x00_CMD();
        }

        public byte[] Get_Read_ELVSS_CMD(Gamma_Set Set)
        {
            return new DP213_ELVSS(Set).Get_Read_ELVSS_CMD();
        }

        public byte[] Get_Read_Cold_ELVSS_CMD(Gamma_Set Set)
        {
            return new DP213_ELVSS(Set).Get_Read_Cold_ELVSS_CMD();
        }

        public byte[] Get_Read_Vinit2_CMD(Gamma_Set Set)
        {
            return new DP213_Vinit2(Set).Get_Read_Vinit2_CMD();
        }

        public byte[] Get_Read_Cold_Vinit2_CMD(Gamma_Set Set)
        {
            return new DP213_Vinit2(Set).Get_Read_Cold_Vinit2_CMD();
        }

        public byte[] Get_Normal_Read_DBV_CMD()
        {
            byte Start_Index = 0;
            byte Address = Convert.ToByte("B1",16);
            byte Quantity = 17;

            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        public byte[] Get_AOD_Read_DBV_CMD()
        {
            byte Start_Index = 0;
            byte Address = Convert.ToByte("B2", 16);
            byte Quantity = 4;

            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        public byte[] Get_Normal_Read_Vreg1_CMD(Gamma_Set Set)
        {
            return new DP213_Vreg1(Set).Get_Normal_Read_Vreg1_CMD();
        }

        public byte[] Get_Normal_Read_REF0_REF4095_CMD()
        {
            byte Start_Index = 19;
            byte Address = Convert.ToByte("B1", 16);
            byte Quantity = 2;
            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        public byte[] Get_Read_Gamma_AM1_AM0_CMD(Gamma_Set Set, int band)
        {
            return new DP213_Gamma(Set, band).Get_Read_Gamma_AM1_AM0_CMD();
        }


        public int Get_Normal_DBV(byte[] papameters,int Normal_band_Index)
        {
            switch (Normal_band_Index)
            {
                case 0:
                    return 4095;
                case 1:
                    return ((papameters[0] & 0xF0) << 4) + (papameters[6]);
                case 2:
                    return ((papameters[0] & 0x0F) << 8) + (papameters[7]);
                case 3:
                    return ((papameters[1] & 0xF0) << 4) + (papameters[8]);
                case 4:
                    return ((papameters[1] & 0x0F) << 8) + (papameters[9]);
                case 5:
                    return ((papameters[2] & 0xF0) << 4) + (papameters[10]);
                case 6:
                    return ((papameters[2] & 0x0F) << 8) + (papameters[11]);
                case 7:
                    return ((papameters[3] & 0xF0) << 4) + (papameters[12]);
                case 8:
                    return ((papameters[3] & 0x0F) << 8) + (papameters[13]);
                case 9:
                    return ((papameters[4] & 0xF0) << 4) + (papameters[14]);
                case 10:
                    return ((papameters[4] & 0x0F) << 8) + (papameters[15]);
                case 11:
                    return ((papameters[5] & 0xF0) << 4) + (papameters[16]);
                default:
                    throw new Exception("Band is out of boundary");
            }
        }

        public int Get_AOD_DBV(byte[] papameters,int AOD_band_Index)
        {
            switch (AOD_band_Index)
            {
                case 0:
                    return 4095;
                case 1:
                    return((papameters[1] & 0xF0) << 4) + (papameters[2]);
                case 2:
                    return ((papameters[1] & 0x0F) << 8) + (papameters[3]);
                default:
                    throw new Exception("AOD_band_Index is out of boundary");
            }
        }

        public byte Get_Byte_ELVSS(byte[] parameters, int normal_band_index)
        {
            return parameters[normal_band_index];
        }

        public byte Get_Byte_Cold_ELVSS(byte[] parameters, int normal_band_index)
        {
            return parameters[normal_band_index];
        }

        public byte Get_Byte_Vinit2(byte[] parameters, int normal_band_index)
        {
            return (byte)((int)parameters[normal_band_index] & 0x3F);
        }

        public byte Get_Byte_Cold_Vinit2(byte[] parameters, int normal_band_index)
        {
            return (byte)((int)parameters[normal_band_index] & 0x3F);
        }

        public int Get_Normal_Vreg1s(byte[] parameters, int normal_band_index)
        {
            switch (normal_band_index)
            {
                case 0:
                    return ((parameters[0] & 0xF0) << 4) + parameters[6];
                case 1:
                    return ((parameters[0] & 0x0F) << 8) + parameters[7];
                case 2:
                   return ((parameters[1] & 0xF0) << 4) + parameters[8];
                case 3:
                    return ((parameters[1] & 0x0F) << 8) + parameters[9];
                case 4:
                     return ((parameters[2] & 0xF0) << 4) + parameters[10];
                case 5:
                    return ((parameters[2] & 0x0F) << 8) + parameters[11];
                case 6:
                    return ((parameters[3] & 0xF0) << 4) + parameters[12];
                case 7:
                     return ((parameters[3] & 0x0F) << 8) + parameters[13];
                case 8:
                    return ((parameters[4] & 0xF0) << 4) + parameters[14];
                case 9:
                    return ((parameters[4] & 0x0F) << 8) + parameters[15];
                case 10:
                    return ((parameters[5] & 0xF0) << 4) + parameters[16];
                case 11:
                    return ((parameters[5] & 0x0F) << 8) + parameters[17];
                default:
                    throw new Exception("Band is out of boundary");
            } 
        }

        public byte Get_Normal_REF0(byte[] papameters)
        {
            return (byte)(papameters[1] & 0x7F);
        }

        public byte Get_Normal_REF4095(byte[] papameters)
        {
            return (byte)(papameters[0] & 0x7F);
        }

        public RGB Get_Gamma(int gray, byte[] papameters)
        {
            int RGB_Offset = 15;
            RGB Gamma = new RGB();

            if (gray == 0)
            {
                Gamma.int_R = ((papameters[0] & 0x04) << 6) + papameters[2];
                Gamma.int_G = ((papameters[0 + RGB_Offset] & 0x04) << 6) + papameters[2 + RGB_Offset];
                Gamma.int_B = ((papameters[0 + (RGB_Offset * 2)] & 0x04) << 6) + papameters[2 + (RGB_Offset * 2)];
            }
            else if (gray == 1)
            {
                Gamma.int_R = ((papameters[0] & 0x02) << 7) + papameters[5];
                Gamma.int_G = ((papameters[0 + RGB_Offset] & 0x02) << 7) + papameters[5 + RGB_Offset];
                Gamma.int_B = ((papameters[0 + (RGB_Offset * 2)] & 0x02) << 7) + papameters[5 + (RGB_Offset * 2)];
            }
            else if (gray == 2)
            {
                Gamma.int_R = ((papameters[0] & 0x01) << 8) + papameters[6];
                Gamma.int_G = ((papameters[0 + RGB_Offset] & 0x01) << 8) + papameters[6 + RGB_Offset];
                Gamma.int_B = ((papameters[0 + (RGB_Offset * 2)] & 0x01) << 8) + papameters[6 + (RGB_Offset * 2)];
            }
            else if (gray == 3)
            {
                Gamma.int_R = ((papameters[1] & 0x80) << 1) + papameters[7];
                Gamma.int_G = ((papameters[1 + RGB_Offset] & 0x80) << 1) + papameters[7 + RGB_Offset];
                Gamma.int_B = ((papameters[1 + (RGB_Offset * 2)] & 0x80) << 1) + papameters[7 + (RGB_Offset * 2)];

            }
            else if (gray == 4)
            {
                Gamma.int_R = ((papameters[1] & 0x40) << 2) + papameters[8];
                Gamma.int_G = ((papameters[1 + RGB_Offset] & 0x40) << 2) + papameters[8 + RGB_Offset];
                Gamma.int_B = ((papameters[1 + (RGB_Offset * 2)] & 0x40) << 2) + papameters[8 + (RGB_Offset * 2)];
            }
            else if (gray == 5)
            {
                Gamma.int_R = ((papameters[1] & 0x20) << 3) + papameters[9];
                Gamma.int_G = ((papameters[1 + RGB_Offset] & 0x20) << 3) + papameters[9 + RGB_Offset];
                Gamma.int_B = ((papameters[1 + (RGB_Offset * 2)] & 0x20) << 3) + papameters[9 + (RGB_Offset * 2)];
            }
            else if (gray == 6)
            {
                Gamma.int_R = ((papameters[1] & 0x10) << 4) + papameters[10];
                Gamma.int_G = ((papameters[1 + RGB_Offset] & 0x10) << 4) + papameters[10 + RGB_Offset];
                Gamma.int_B = ((papameters[1 + (RGB_Offset * 2)] & 0x10) << 4) + papameters[10 + (RGB_Offset * 2)];
            }
            else if (gray == 7)
            {
                Gamma.int_R = ((papameters[1] & 0x08) << 5) + papameters[11];
                Gamma.int_G = ((papameters[1 + RGB_Offset] & 0x08) << 5) + papameters[11 + RGB_Offset];
                Gamma.int_B = ((papameters[1 + (RGB_Offset * 2)] & 0x08) << 5) + papameters[11 + (RGB_Offset * 2)];
            }
            else if (gray == 8)
            {
                Gamma.int_R = ((papameters[1] & 0x04) << 6) + papameters[12];
                Gamma.int_G = ((papameters[1 + RGB_Offset] & 0x04) << 6) + papameters[12 + RGB_Offset];
                Gamma.int_B = ((papameters[1 + (RGB_Offset * 2)] & 0x04) << 6) + papameters[12 + (RGB_Offset * 2)];
            }
            else if (gray == 9)
            {
                Gamma.int_R = ((papameters[1] & 0x02) << 7) + papameters[13];
                Gamma.int_G = ((papameters[1 + RGB_Offset] & 0x02) << 7) + papameters[13 + RGB_Offset];
                Gamma.int_B = ((papameters[1 + (RGB_Offset * 2)] & 0x02) << 7) + papameters[13 + (RGB_Offset * 2)];
            }
            else if (gray == 10)
            {
                Gamma.int_R = ((papameters[1] & 0x01) << 8) + papameters[14];
                Gamma.int_G = ((papameters[1 + RGB_Offset] & 0x01) << 8) + papameters[14 + RGB_Offset];
                Gamma.int_B = ((papameters[1 + (RGB_Offset * 2)] & 0x01) << 8) + papameters[14 + (RGB_Offset * 2)];
            }
            else
            {
                throw new Exception("gray index is out of boundary");
            }

            return Gamma;
        }



        public RGB Get_AM1(byte[] papameters)
        {
            int RGB_Offset = 15;

            RGB AM1 = new RGB();
            AM1.int_R = papameters[3];
            AM1.int_G = papameters[3 + RGB_Offset];
            AM1.int_B = papameters[3 + (RGB_Offset * 2)];

            return AM1;
        }

        public RGB Get_AM0(byte[] papameters)
        {
            int RGB_Offset = 15;

            RGB AM0 = new RGB();
            AM0.int_R = papameters[4];
            AM0.int_G = papameters[4 + RGB_Offset];
            AM0.int_B = papameters[4 + (RGB_Offset * 2)];

            return AM0;
        }

        public bool IsBandGammaReversed(RGB_Double[] Band_Gamma_Voltage)
        {
            for (int i = 1; i < Band_Gamma_Voltage.Length; i++)
            {
                if (IsGrayGammaReversed(Band_Gamma_Voltage[i - 1], Band_Gamma_Voltage[i]))
                    return true;
            }
            return false;
        }

        private bool IsGrayGammaReversed(RGB_Double Prev_Gamma_Voltage, RGB_Double Current_Gamma_Voltage)
        {
            if (Prev_Gamma_Voltage < Current_Gamma_Voltage)
                return false;
            else
                return true;
        }
    }



    class DP213_Gamma
    {
        Gamma_Set Set;
        RGB[] Band_Set_Gamma;
        RGB AM1;
        RGB AM0;
        int band;

        public DP213_Gamma(Gamma_Set Set,int band,int[] Band_Set_Gamma_Red,int[] Band_Set_Gamma_Green,int[] Band_Set_Gamma_Blue,
            int AM1_Red,int AM1_Green,int AM1_Blue,int AM0_Red,int AM0_Green,int AM0_Blue)
        {
            this.Set = Set;
            this.band = band;

            for(int i=0; i<DP213_Static.Max_Gray_Amount; i++)
            {
                Band_Set_Gamma[i].int_R = Band_Set_Gamma_Red[i];
                Band_Set_Gamma[i].int_G = Band_Set_Gamma_Green[i];
                Band_Set_Gamma[i].int_B = Band_Set_Gamma_Blue[i];
            }

            AM1.int_R = AM1_Red;
            AM1.int_G = AM1_Green;
            AM1.int_B = AM1_Blue;

            AM0.int_R = AM0_Red;
            AM0.int_G = AM0_Green;
            AM0.int_B = AM0_Blue;
        }

        public DP213_Gamma(Gamma_Set Set,int band,RGB[] Band_Set_Gamma, RGB AM1, RGB AM0)
        {
            this.Set = Set;
            this.band = band;
            this.Band_Set_Gamma = Band_Set_Gamma;
            this.AM1 = AM1;
            this.AM0 = AM0;
        }

        public DP213_Gamma(Gamma_Set Set, int band)
        {
            this.Set = Set;
            this.band = band;
        }

        public byte[] Get_Read_Gamma_AM1_AM0_CMD()
        {
            byte Start_Index = Convert.ToByte(DP213_Static.Get_Band_Gamma_Start_Offset(band));
            byte Address = Convert.ToByte(DP213_Static.Get_Band_Set_Gamma_Hex_Register(Set, band), 16);
            byte Quantity = 45;

            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        public byte[][] Get_Gamma_AM1_AM0_CMD()
        {
            byte Start_Index = Convert.ToByte(DP213_Static.Get_Band_Gamma_Start_Offset(band));
            byte Address = Convert.ToByte(DP213_Static.Get_Band_Set_Gamma_Hex_Register(Set, band), 16);
            byte[] Gamma_Array = Get_byte_Gamma_Params();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Gamma_Array);
        }

        public byte[][] Get_Gamma_AM1_AM0_0x00_CMD()
        {
            byte Start_Index = Convert.ToByte(DP213_Static.Get_Band_Gamma_Start_Offset(band));
            byte Address = Convert.ToByte(DP213_Static.Get_Band_Set_Gamma_Hex_Register(Set, band), 16);
            byte[] Gamma_Array = Get_byte_Gamma_0x00_Params();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Gamma_Array);
        }

        byte[] Get_byte_Gamma_0x00_Params()
        {
            byte[] Byte_Gamma_Param = new byte[45];
            for (int i = 0; i < 45; i++)
                Byte_Gamma_Param[i] = 0;
            return Byte_Gamma_Param;
        }

        byte[] Get_byte_Gamma_Params()
        {
            int[] Dec_Gamma_Param = new int[45];

            //Red(1~15)
            Dec_Gamma_Param[0] = ((Band_Set_Gamma[0].int_R & 0x100) >> 6) + ((Band_Set_Gamma[1].int_R & 0x100) >> 7) + ((Band_Set_Gamma[2].int_R & 0x100) >> 8);
            Dec_Gamma_Param[1] = ((Band_Set_Gamma[3].int_R & 0x100) >> 1) + ((Band_Set_Gamma[4].int_R & 0x100) >> 2) + ((Band_Set_Gamma[5].int_R & 0x100) >> 3)
                + ((Band_Set_Gamma[6].int_R & 0x100) >> 4) + ((Band_Set_Gamma[7].int_R & 0x100) >> 5) + ((Band_Set_Gamma[8].int_R & 0x100) >> 6)
                + ((Band_Set_Gamma[9].int_R & 0x100) >> 7) + ((Band_Set_Gamma[10].int_R & 0x100) >> 8);
            Dec_Gamma_Param[2] = (Band_Set_Gamma[0].int_R & 0xFF);
            Dec_Gamma_Param[3] = AM1.int_R;
            Dec_Gamma_Param[4] = AM0.int_R;
            Dec_Gamma_Param[5] = (Band_Set_Gamma[1].int_R & 0xFF);
            Dec_Gamma_Param[6] = (Band_Set_Gamma[2].int_R & 0xFF);
            Dec_Gamma_Param[7] = (Band_Set_Gamma[3].int_R & 0xFF);
            Dec_Gamma_Param[8] = (Band_Set_Gamma[4].int_R & 0xFF);
            Dec_Gamma_Param[9] = (Band_Set_Gamma[5].int_R & 0xFF);
            Dec_Gamma_Param[10] = (Band_Set_Gamma[6].int_R & 0xFF);
            Dec_Gamma_Param[11] = (Band_Set_Gamma[7].int_R & 0xFF);
            Dec_Gamma_Param[12] = (Band_Set_Gamma[8].int_R & 0xFF);
            Dec_Gamma_Param[13] = (Band_Set_Gamma[9].int_R & 0xFF);
            Dec_Gamma_Param[14] = (Band_Set_Gamma[10].int_R & 0xFF);

            //Green(16~30)
            Dec_Gamma_Param[15] = ((Band_Set_Gamma[0].int_G & 0x100) >> 6) + ((Band_Set_Gamma[1].int_G & 0x100) >> 7) + ((Band_Set_Gamma[2].int_G & 0x100) >> 8);
            Dec_Gamma_Param[16] = ((Band_Set_Gamma[3].int_G & 0x100) >> 1) + ((Band_Set_Gamma[4].int_G & 0x100) >> 2) + ((Band_Set_Gamma[5].int_G & 0x100) >> 3)
                + ((Band_Set_Gamma[6].int_G & 0x100) >> 4) + ((Band_Set_Gamma[7].int_G & 0x100) >> 5) + ((Band_Set_Gamma[8].int_G & 0x100) >> 6)
                + ((Band_Set_Gamma[9].int_G & 0x100) >> 7) + ((Band_Set_Gamma[10].int_G & 0x100) >> 8);
            Dec_Gamma_Param[17] = (Band_Set_Gamma[0].int_G & 0xFF);
            Dec_Gamma_Param[18] = AM1.int_G;
            Dec_Gamma_Param[19] = AM0.int_G;
            Dec_Gamma_Param[20] = (Band_Set_Gamma[1].int_G & 0xFF);
            Dec_Gamma_Param[21] = (Band_Set_Gamma[2].int_G & 0xFF);
            Dec_Gamma_Param[22] = (Band_Set_Gamma[3].int_G & 0xFF);
            Dec_Gamma_Param[23] = (Band_Set_Gamma[4].int_G & 0xFF);
            Dec_Gamma_Param[24] = (Band_Set_Gamma[5].int_G & 0xFF);
            Dec_Gamma_Param[25] = (Band_Set_Gamma[6].int_G & 0xFF);
            Dec_Gamma_Param[26] = (Band_Set_Gamma[7].int_G & 0xFF);
            Dec_Gamma_Param[27] = (Band_Set_Gamma[8].int_G & 0xFF);
            Dec_Gamma_Param[28] = (Band_Set_Gamma[9].int_G & 0xFF);
            Dec_Gamma_Param[29] = (Band_Set_Gamma[10].int_G & 0xFF);

            //Blue(31~45)
            Dec_Gamma_Param[30] = ((Band_Set_Gamma[0].int_B & 0x100) >> 6) + ((Band_Set_Gamma[1].int_B & 0x100) >> 7) + ((Band_Set_Gamma[2].int_B & 0x100) >> 8);
            Dec_Gamma_Param[31] = ((Band_Set_Gamma[3].int_B & 0x100) >> 1) + ((Band_Set_Gamma[4].int_B & 0x100) >> 2) + ((Band_Set_Gamma[5].int_B & 0x100) >> 3)
                + ((Band_Set_Gamma[6].int_B & 0x100) >> 4) + ((Band_Set_Gamma[7].int_B & 0x100) >> 5) + ((Band_Set_Gamma[8].int_B & 0x100) >> 6)
                + ((Band_Set_Gamma[9].int_B & 0x100) >> 7) + ((Band_Set_Gamma[10].int_B & 0x100) >> 8);
            Dec_Gamma_Param[32] = (Band_Set_Gamma[0].int_B & 0xFF);
            Dec_Gamma_Param[33] = AM1.int_B;
            Dec_Gamma_Param[34] = AM0.int_B;
            Dec_Gamma_Param[35] = (Band_Set_Gamma[1].int_B & 0xFF);
            Dec_Gamma_Param[36] = (Band_Set_Gamma[2].int_B & 0xFF);
            Dec_Gamma_Param[37] = (Band_Set_Gamma[3].int_B & 0xFF);
            Dec_Gamma_Param[38] = (Band_Set_Gamma[4].int_B & 0xFF);
            Dec_Gamma_Param[39] = (Band_Set_Gamma[5].int_B & 0xFF);
            Dec_Gamma_Param[40] = (Band_Set_Gamma[6].int_B & 0xFF);
            Dec_Gamma_Param[41] = (Band_Set_Gamma[7].int_B & 0xFF);
            Dec_Gamma_Param[42] = (Band_Set_Gamma[8].int_B & 0xFF);
            Dec_Gamma_Param[43] = (Band_Set_Gamma[9].int_B & 0xFF);
            Dec_Gamma_Param[44] = (Band_Set_Gamma[10].int_B & 0xFF);

            byte[] Byte_Gamma_Param = new byte[45];
            for (int i = 0; i < 45; i++)
                Byte_Gamma_Param[i] = Convert.ToByte(Dec_Gamma_Param[i]);
            return Byte_Gamma_Param;
        }


    }


    class DP213_Vreg1
    {
        Gamma_Set Set;
        int[] Band_Dec_Vreg1;

        public DP213_Vreg1(Gamma_Set Set)
        {
            this.Set = Set;
        }

        public DP213_Vreg1(Gamma_Set Set, int[] Dec_Normal_Vreg1)
        {
            this.Set = Set;
            this.Band_Dec_Vreg1 = Dec_Normal_Vreg1;
        }

        public byte[][] Get_Vreg1_CMD()
        {
            byte Start_Index = Convert.ToByte(DP213_Static.Get_Set_Vreg1_Start_Offset(Set));
            byte Address = Convert.ToByte("B1", 16);
            byte[] Vreg1_Array = Get_Set_Hex_Normal_Vreg1();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Vreg1_Array);
        }

        public byte[] Get_Normal_Read_Vreg1_CMD()
        {
            byte Start_Index = Convert.ToByte(DP213_Static.Get_Set_Vreg1_Start_Offset(Set));
            byte Address = Convert.ToByte("B1", 16);
            byte Quantity = Convert.ToByte(DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount);

            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        public byte[][] Get_Vreg1_0x00_CMD()
        {
            byte Start_Index = Convert.ToByte(DP213_Static.Get_Set_Vreg1_Start_Offset(Set));
            byte Address = Convert.ToByte("B1", 16);
            byte[] Vreg1_Array = Get_Set_Hex_Normal_Vreg1_Zeros();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Vreg1_Array);
        }

        byte[] Get_Set_Hex_Normal_Vreg1()
        {
            byte[] Vreg1_Array = new byte[DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount];

            Vreg1_Array[0] = Convert.ToByte(((Band_Dec_Vreg1[0] & 0xF00) >> 4) + ((Band_Dec_Vreg1[1] & 0xF00) >> 8));
            Vreg1_Array[1] = Convert.ToByte(((Band_Dec_Vreg1[2] & 0xF00) >> 4) + ((Band_Dec_Vreg1[3] & 0xF00) >> 8));
            Vreg1_Array[2] = Convert.ToByte(((Band_Dec_Vreg1[4] & 0xF00) >> 4) + ((Band_Dec_Vreg1[5] & 0xF00) >> 8));
            Vreg1_Array[3] = Convert.ToByte(((Band_Dec_Vreg1[6] & 0xF00) >> 4) + ((Band_Dec_Vreg1[7] & 0xF00) >> 8));
            Vreg1_Array[4] = Convert.ToByte(((Band_Dec_Vreg1[8] & 0xF00) >> 4) + ((Band_Dec_Vreg1[9] & 0xF00) >> 8));
            Vreg1_Array[5] = Convert.ToByte(((Band_Dec_Vreg1[10] & 0xF00) >> 4) + ((Band_Dec_Vreg1[11] & 0xF00) >> 8));

            Vreg1_Array[6] = Convert.ToByte(Band_Dec_Vreg1[0] & 0xFF);
            Vreg1_Array[7] = Convert.ToByte(Band_Dec_Vreg1[1] & 0xFF);
            Vreg1_Array[8] = Convert.ToByte(Band_Dec_Vreg1[2] & 0xFF);
            Vreg1_Array[9] = Convert.ToByte(Band_Dec_Vreg1[3] & 0xFF);
            Vreg1_Array[10] = Convert.ToByte(Band_Dec_Vreg1[4] & 0xFF);
            Vreg1_Array[11] = Convert.ToByte(Band_Dec_Vreg1[5] & 0xFF);
            Vreg1_Array[12] = Convert.ToByte(Band_Dec_Vreg1[6] & 0xFF);
            Vreg1_Array[13] = Convert.ToByte(Band_Dec_Vreg1[7] & 0xFF);
            Vreg1_Array[14] = Convert.ToByte(Band_Dec_Vreg1[8] & 0xFF);
            Vreg1_Array[15] = Convert.ToByte(Band_Dec_Vreg1[9] & 0xFF);
            Vreg1_Array[16] = Convert.ToByte(Band_Dec_Vreg1[10] & 0xFF);
            Vreg1_Array[17] = Convert.ToByte(Band_Dec_Vreg1[11] & 0xFF);

            return Vreg1_Array;
        }

        byte[] Get_Set_Hex_Normal_Vreg1_Zeros()
        {
            byte[] Vreg1_Array = new byte[DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount];

            for (int i = 0; i < DP213_Static.One_Normal_GammaSet_Vreg1_Parameters_Amount; i++)
                Vreg1_Array[i] = 0;
            
            return Vreg1_Array;
        }

       




    }


    class DP213_Vinit2
    {
        Gamma_Set Set;
        double[] Vinit2_Voltage;

        public DP213_Vinit2(Gamma_Set Set)
        {
            this.Set = Set;
        }

        public DP213_Vinit2(Gamma_Set Set, double[] Vinit2_Voltage)
        {
            this.Set = Set;
            this.Vinit2_Voltage = Vinit2_Voltage;
        }

        public byte[][] Get_Vinit2_CMD()
        {
            byte Start_Index = Get_Start_Index_of_Vinit2();
            byte Address = Get_Address_of_Vinit2();
            byte[] Dec_Vinit2 = Get_Dec_Vinit2();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Dec_Vinit2);
        }

        public byte[][] Get_Cold_Vinit2_CMD()
        {
            byte Start_Index = Get_Start_Index_of_Cold_Vinit2();
            byte Address = Convert.ToByte("E4", 16);
            byte[] Dec_Vinit2 = Get_Dec_Vinit2();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Dec_Vinit2);
        }

        public byte[][] Get_Vinit2_0x00_CMD()
        {
            byte Start_Index = Get_Start_Index_of_Vinit2();
            byte Address = Get_Address_of_Vinit2();
            byte[] Dec_Vinit2 = Get_Vinit2_Zeros();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Dec_Vinit2);
        }

        public byte[][] Get_Cold_Vinit2_0x00_CMD()
        {
            byte Start_Index = Get_Start_Index_of_Cold_Vinit2();
            byte Address = Convert.ToByte("E4", 16);
            byte[] Dec_Vinit2 = Get_Vinit2_Zeros();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Dec_Vinit2);
        }

        byte[] Get_Dec_Vinit2()
        {
            byte[] Dec_Vinit2 = new byte[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int i = 0; i < DP213_Static.Max_HBM_and_Normal_Band_Amount; i++)
                Dec_Vinit2[i] = Convert.ToByte(Imported_my_cpp_dll.DP213_VINI2_Voltage_to_Dec(Vinit2_Voltage[i]));

            return Dec_Vinit2;
        }

        byte[] Get_Vinit2_Zeros()
        {
            byte[] Dec_Vinit2 = new byte[DP213_Static.Max_HBM_and_Normal_Band_Amount];
           
            for (int i = 0; i < DP213_Static.Max_HBM_and_Normal_Band_Amount; i++)
                Dec_Vinit2[i] = 0;

            return Dec_Vinit2;
        }

        public byte[] Get_Read_Vinit2_CMD()
        {
            byte Quantity = 12;
            byte Start_Index = Get_Start_Index_of_Vinit2();
            byte Address = Get_Address_of_Vinit2();
            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        byte Get_Start_Index_of_Vinit2()
        {
            switch (Set)
            {
                case Gamma_Set.Set1:
                    return 88;
                case Gamma_Set.Set2:
                    return 149;
                case Gamma_Set.Set3:
                    return 210;
                case Gamma_Set.Set4:
                    return 29;
                case Gamma_Set.Set5:
                    return 90;
                case Gamma_Set.Set6:
                    return 151;
                default:
                    throw new Exception("Invalid GammaSet");
            }
        }

        byte Get_Address_of_Vinit2()
        {
            if (Set == Gamma_Set.Set1 || Set == Gamma_Set.Set2 || Set == Gamma_Set.Set3)
            {
                return Convert.ToByte("E2", 16);
            }
            else if (Set == Gamma_Set.Set4 || Set == Gamma_Set.Set5 || Set == Gamma_Set.Set6)
            {
                return Convert.ToByte("E3", 16);
            }
            else
            {
                throw new Exception("Invalid GammaSet");
            }
        }

        public byte[] Get_Read_Cold_Vinit2_CMD()
        {
            byte Quantity = 12;
            byte Start_Index = Get_Start_Index_of_Cold_Vinit2();
            byte Address = Convert.ToByte("E4", 16);
            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        private byte Get_Start_Index_of_Cold_Vinit2()
        {
            switch (Set)
            {
                case Gamma_Set.Set1:
                    return 9;
                case Gamma_Set.Set2:
                    return 33;
                case Gamma_Set.Set3:
                    return 57;
                case Gamma_Set.Set4:
                    return 81;
                case Gamma_Set.Set5:
                    return 105;
                case Gamma_Set.Set6:
                    return 129;
                default:
                    throw new Exception("Invalid GammaSet");
            }
        }

    }


    class DP213_ELVSS
    {
        Gamma_Set Set;
        double[] ELVSS_Voltage;

        public DP213_ELVSS(Gamma_Set Set)
        {
            this.Set = Set;
        }

        public DP213_ELVSS(Gamma_Set Set, double[] ELVSS_Voltage)
        {
            this.Set = Set;
            this.ELVSS_Voltage = ELVSS_Voltage;
        }

        public byte[][] Get_ELVSS_CMD()
        {
            byte Start_Index = Get_Start_Index_of_ELVSS();
            byte Address = Convert.ToByte("D0", 16);
            byte[] Dec_ELVSS = Get_Dec_ELVSS();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Dec_ELVSS);
        }

        public byte[][] Get_Cold_ELVSS_CMD()
        {
            byte Start_Index = Get_Start_Index_of_Cold_ELVSS();
            byte Address = Convert.ToByte("E0", 16);
            byte[] Dec_ELVSS = Get_Dec_ELVSS();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Dec_ELVSS);
        }

        public byte[][] Get_ELVSS_0x00_CMD()
        {
            byte Start_Index = Get_Start_Index_of_ELVSS();
            byte Address = Convert.ToByte("D0", 16);
            byte[] Dec_ELVSS = Get_ELVSS_Zeros();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Dec_ELVSS);
        }

        public byte[][] Get_Cold_ELVSS_0x00_CMD()
        {
            byte Start_Index = Get_Start_Index_of_Cold_ELVSS();
            byte Address = Convert.ToByte("E0", 16);
            byte[] Dec_ELVSS = Get_ELVSS_Zeros();
            return Static_Func.Get_WriteCMD_Output_bytes(Start_Index, Address, Dec_ELVSS);
        }

        byte[] Get_Dec_ELVSS()
        {
            byte[] Dec_ELVSS = new byte[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int i = 0; i < DP213_Static.Max_HBM_and_Normal_Band_Amount; i++)
                Dec_ELVSS[i] = Convert.ToByte(Imported_my_cpp_dll.DP213_ELVSS_Voltage_to_Dec(ELVSS_Voltage[i]));

            return Dec_ELVSS;
        }

        byte[] Get_ELVSS_Zeros()
        {
            byte[] Dec_ELVSS = new byte[DP213_Static.Max_HBM_and_Normal_Band_Amount];
            for (int i = 0; i < DP213_Static.Max_HBM_and_Normal_Band_Amount; i++)
                Dec_ELVSS[i] = 0;

            return Dec_ELVSS;
        }

        
        public byte[] Get_Read_ELVSS_CMD()
        {
            byte Quantity = 12;
            byte Start_Index = Get_Start_Index_of_ELVSS();
            byte Address = Convert.ToByte("D0", 16);
            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        private byte Get_Start_Index_of_ELVSS()
        {
            switch (Set)
            {
                case Gamma_Set.Set1:
                    return 113;
                case Gamma_Set.Set2:
                    return 125;
                case Gamma_Set.Set3:
                    return 137;
                case Gamma_Set.Set4:
                    return 149;
                case Gamma_Set.Set5:
                    return 161;
                case Gamma_Set.Set6:
                    return 173;
                default:
                    throw new Exception("Invalid GammaSet");
            }
        }

        public byte[] Get_Read_Cold_ELVSS_CMD()
        {
            byte Quantity = 12;
            byte Start_Index = Get_Start_Index_of_Cold_ELVSS();
            byte Address = Convert.ToByte("E0", 16);
            return Static_Func.Get_ReadCMD_Output_bytes(Start_Index, Address, Quantity);
        }

        private byte Get_Start_Index_of_Cold_ELVSS()
        {
            switch (Set)
            {
                case Gamma_Set.Set1:
                    return 0;
                case Gamma_Set.Set2:
                    return 12;
                case Gamma_Set.Set3:
                    return 24;
                case Gamma_Set.Set4:
                    return 36;
                case Gamma_Set.Set5:
                    return 48;
                case Gamma_Set.Set6:
                    return 60;
                default:
                    throw new Exception("Invalid GammaSet");
            }
        }
    }



    public class DP213_Static
    {
        //Static public Variables
        static readonly public int Max_Gray_Amount = 11;
        static readonly public int Max_Band_Amount = 15;
        static readonly public int Max_Normal_Band_Amount = 11;
        static readonly public int Max_HBM_and_Normal_Band_Amount = 12;
        static readonly public int Max_AOD_Band_Amount = 3;
        static readonly public int Max_Set_Amount = 6;
        static readonly public int One_Band_Gamma_Parameters_Amount = 45;
        static readonly public int One_Normal_GammaSet_Vreg1_Parameters_Amount = 18;

        static public readonly int Gamma_Register_Max = 511;//[8:0]
        static public readonly int Vreg1_Register_Max = 4095;//[11:0]
        static readonly public int REF4095_REF0_Max = 127;//[6:0]
        static readonly public int AM1_AM0_Max = 127;//[6:0]

        static public RGB[] Minimum_Distance(RGB[] rgb60, RGB[] rgb90, RGB[] rgb120, XYLv[] xylv60, XYLv[] xylv90, XYLv[] xylv120, int n)
        {
            double min = double.MaxValue;
            double[,,] data = new double[n, n, n];

            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < n; k++)
                {
                    for (int m = 0; m < n; m++)
                    {
                        data[i, k, m] = Math.Sqrt(Math.Pow(xylv60[i].double_X - xylv90[k].double_X, 2) + Math.Pow(xylv60[i].double_Y - xylv90[k].double_Y, 2))
                            + Math.Sqrt(Math.Pow(xylv90[k].double_X - xylv120[m].double_X, 2) + Math.Pow(xylv90[k].double_Y - xylv120[m].double_Y, 2))
                            + Math.Sqrt(Math.Pow(xylv60[i].double_X - xylv120[m].double_X, 2) + Math.Pow(xylv60[i].double_Y - xylv120[m].double_Y, 2));
                    }
                }
            }

            RGB[] result = new RGB[3];

            for (int i = 0; i < n; i++)
                for (int k = 0; k < n; k++)
                    for (int m = 0; m < n; m++)
                    {
                        if (min > data[i, k, m])
                        {
                            min = data[i, k, m];
                            result[0] = rgb60[i];
                            result[1] = rgb90[k];
                            result[2] = rgb120[m];
                        }
                    }
            return result;
        }


        static public bool Is_AOD0_or_HBM_Band(int band)
        {
            if (band == 0 || band == 12)
                return true;
            else
                return false;
        }

        static public bool Is_Not_AOD0_or_HBM_Band(int band)
        {
            return (!Is_AOD0_or_HBM_Band(band));
        }

        static public int Get_Pattern_Gray_by_index_gray(int gray)
        {
            switch (gray)
            {
                case 0:
                    return 255;
                case 1:
                    return 191;
                case 2:
                    return 127;
                case 3:
                    return 95;
                case 4:
                    return 63;
                case 5:
                    return 47;
                case 6:
                    return 31;
                case 7:
                    return 23;
                case 8:
                    return 15;
                case 9:
                    return 7;
                case 10:
                    return 4;
                default:
                    return 9999999;
            }
        }

        static public string Get_Band_Set_Gamma_Hex_Register(Gamma_Set Set, int band)
        {
            switch (Set)
            {
                case Gamma_Set.Set1:
                    if (band <= 3) return "B3";
                    else if (band <= 7) return "B4";
                    else if (band <= 11) return "B5";
                    else return "B2";//AOD
                case Gamma_Set.Set2:
                    if (band <= 3) return "B6";
                    else if (band <= 7) return "B7";
                    else if (band <= 11) return "B8";
                    else return "B2";//AOD
                case Gamma_Set.Set3:
                    if (band <= 3) return "B9";
                    else if (band <= 7) return "BA";
                    else if (band <= 11) return "BB";
                    else return "B2";//AOD;
                case Gamma_Set.Set4:
                    if (band <= 3) return "BC";
                    else if (band <= 7) return "BD";
                    else if (band <= 11) return "BE";
                    else return "B2";//AOD
                case Gamma_Set.Set5:
                    if (band <= 3) return "BF";
                    else if (band <= 7) return "C0";
                    else if (band <= 11) return "C1";
                    else return "B2";//AOD
                case Gamma_Set.Set6:
                    if (band <= 3) return "C2";
                    else if (band <= 7) return "C3";
                    else if (band <= 11) return "C4";
                    else return "B2";//AOD
                case Gamma_Set.SetNull:
                    return "B2";//AOD
                default:
                    throw new Exception("InValid GammaSet");
            }
        }

        static public int Get_Band_Gamma_Start_Offset(int band)
        {
            int Start_Offset;
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                int standard = (band % 4);
                if (standard == 0) Start_Offset = 0;
                else if (standard == 1) Start_Offset = 45;
                else if (standard == 2) Start_Offset = 90;
                else Start_Offset = 135;
            }
            else
            {
                if (band == 12) Start_Offset = 10;
                else if (band == 13) Start_Offset = 55;
                else Start_Offset = 100;
            }
            return Start_Offset;
        }

        static public int Get_Set_Vreg1_Start_Offset(Gamma_Set Set)
        {
            switch (Set)
            {
                case Gamma_Set.Set1:
                    return 34;
                case Gamma_Set.Set2:
                    return 52;
                case Gamma_Set.Set3:
                    return 70;
                case Gamma_Set.Set4:
                    return 88;
                case Gamma_Set.Set5:
                    return 106;
                case Gamma_Set.Set6:
                    return 124;
                default:
                    throw new Exception("Invalid GammaSet");
            }
        }

        static public Gamma_Set Convert_to_Gamma_Set_From_int(int Set_index)
        {
            switch (Set_index)
            {
                case 0:
                    return Gamma_Set.Set1;
                case 1:
                    return Gamma_Set.Set2;
                case 2:
                    return Gamma_Set.Set3;
                case 3:
                    return Gamma_Set.Set4;
                case 4:
                    return Gamma_Set.Set5;
                case 5:
                    return Gamma_Set.Set6;
                default:
                    return Gamma_Set.SetNull;
            }
        }

        static public int Convert_2byte_to_Minus_int(string Twobyte_Hex)
        {
            return (255 - Convert.ToInt16(Twobyte_Hex, 16));
        }
    }
}

    
