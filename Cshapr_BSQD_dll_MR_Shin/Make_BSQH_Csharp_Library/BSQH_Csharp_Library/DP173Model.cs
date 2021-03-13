using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSQH_Csharp_Library
{
    public class DP173Model : Model
    {
        private static DP173Model instance;
        private DP173Model() { }
        public static DP173Model getInstance()
        {
            if (instance == null)
                instance = new DP173Model();
            return instance;
        }


        public byte[][] Get_ELVSS_CMD(Gamma_Set Set, double[] ELVSS_Voltage)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_ELVSS_0x00_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Cold_ELVSS_CMD(Gamma_Set Set, double[] Cold_ELVSS_Voltage)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Cold_ELVSS_0x00_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Vinit2_CMD(Gamma_Set Set, double[] Vinit2_Voltage)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Vinit2_0x00_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Cold_Vinit2_CMD(Gamma_Set Set, double[] Cold_Vinit2_Voltage)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Cold_Vinit2_0x00_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Vreg1_CMD(Gamma_Set Set, int[] Dec_Normal_Vreg1)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Vreg1_0x00_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_REF4095_REF0_CMD(byte REF4095, byte REF0)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_REF4095_REF0_0x00_CMD()
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Gamma_AM1_AM0_CMD(Gamma_Set Set, int band, int[] Band_Set_Gamma_Red, int[] Band_Set_Gamma_Green, int[] Band_Set_Gamma_Blue, int AM1_Red, int AM1_Green, int AM1_Blue, int AM0_Red, int AM0_Green, int AM0_Blue)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Gamma_AM1_AM0_CMD(Gamma_Set Set, int band, RGB[] Band_Set_Gamma, RGB AM1, RGB AM0)
        {
            throw new NotImplementedException();
        }

        public byte[][] Get_Gamma_AM1_AM0_0x00_CMD(Gamma_Set Set, int band)
        {
            throw new NotImplementedException();
        }

        public byte[] Get_Normal_Read_DBV_CMD()
        {
            throw new NotImplementedException();
        }

        public byte[] Get_AOD_Read_DBV_CMD()
        {
            throw new NotImplementedException();
        }

        public byte[] Get_Read_ELVSS_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[] Get_Read_Cold_ELVSS_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[] Get_Read_Vinit2_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[] Get_Read_Cold_Vinit2_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[] Get_Normal_Read_Vreg1_CMD(Gamma_Set Set)
        {
            throw new NotImplementedException();
        }

        public byte[] Get_Normal_Read_REF0_REF4095_CMD()
        {
            throw new NotImplementedException();
        }

        public byte[] Get_Read_Gamma_AM1_AM0_CMD(Gamma_Set Set, int band)
        {
            throw new NotImplementedException();
        }

        public int Get_Normal_DBV(byte[] parameters, int normal_band_index)
        {
            throw new NotImplementedException();
        }

        public int Get_AOD_DBV(byte[] parameters, int AOD_band_index)
        {
            throw new NotImplementedException();
        }

        public byte Get_Byte_ELVSS(byte[] parameters, int normal_band_index)
        {
            throw new NotImplementedException();
        }

        public byte Get_Byte_Cold_ELVSS(byte[] parameters, int normal_band_index)
        {
            throw new NotImplementedException();
        }

        public byte Get_Byte_Vinit2(byte[] parameters, int normal_band_index)
        {
            throw new NotImplementedException();
        }

        public byte Get_Byte_Cold_Vinit2(byte[] parameters, int normal_band_index)
        {
            throw new NotImplementedException();
        }

        public int Get_Normal_Vreg1s(byte[] parameters, int normal_band_index)
        {
            throw new NotImplementedException();
        }

        public int Get_Normal_REF0(byte[] parameters)
        {
            throw new NotImplementedException();
        }

        public byte Get_Normal_REF4095(byte[] parameters)
        {
            throw new NotImplementedException();
        }

        public byte Get_Gamma(int gray, byte[] parameters)
        {
            throw new NotImplementedException();
        }

        public RGB Get_AM1(byte[] parameters)
        {
            throw new NotImplementedException();
        }

        public RGB Get_AM0(byte[] parameters)
        {
            throw new NotImplementedException();
        }


        byte Model.Get_Normal_REF0(byte[] parameters)
        {
            throw new NotImplementedException();
        }

        RGB Model.Get_Gamma(int gray, byte[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool IsBandGammaReversed(RGB_Double[] Band_Gamma_Voltage)
        {
            throw new NotImplementedException();
        }
    }
}
