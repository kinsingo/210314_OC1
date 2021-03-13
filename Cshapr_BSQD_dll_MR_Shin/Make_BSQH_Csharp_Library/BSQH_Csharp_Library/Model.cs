using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSQH_Csharp_Library
{
    

    public enum Gamma_Set
    {
        Set1 = 0,
        Set2 = 1,
        Set3 = 2,
        Set4 = 3,
        Set5 = 4,
        Set6 = 5,
        SetNull,
    }

    public enum OC_Mode
    {
        Mode1,
        Mode2,
        Mode3,
        Mode4,
        Mode5,
        Mode6,
        ModeNull,
    }

    public interface Model
    {
        //-------Write_Related_CMDs-------------
        //ELVSS & Vinit2
        bool IsBandGammaReversed(RGB_Double[] Band_Gamma_Voltage);
        byte[][] Get_ELVSS_CMD(Gamma_Set Set, double[] ELVSS_Voltage);
        byte[][] Get_ELVSS_0x00_CMD(Gamma_Set Set);
        byte[][] Get_Cold_ELVSS_CMD(Gamma_Set Set, double[] Cold_ELVSS_Voltage);
        byte[][] Get_Cold_ELVSS_0x00_CMD(Gamma_Set Set);
        byte[][] Get_Vinit2_CMD(Gamma_Set Set, double[] Vinit2_Voltage);
        byte[][] Get_Vinit2_0x00_CMD(Gamma_Set Set);
        byte[][] Get_Cold_Vinit2_CMD(Gamma_Set Set, double[] Cold_Vinit2_Voltage);
        byte[][] Get_Cold_Vinit2_0x00_CMD(Gamma_Set Set);

        //Vreg1
        byte[][] Get_Vreg1_CMD(Gamma_Set Set, int[] Dec_Normal_Vreg1);
        byte[][] Get_Vreg1_0x00_CMD(Gamma_Set Set);

        //REF0 & REF4095
        byte[][] Get_REF4095_REF0_CMD(byte REF4095, byte REF0);
        byte[][] Get_REF4095_REF0_0x00_CMD();

        //AM2&GR,AM1,AM0 
        byte[][] Get_Gamma_AM1_AM0_CMD(Gamma_Set Set, int band, int[] Band_Set_Gamma_Red, int[] Band_Set_Gamma_Green, int[] Band_Set_Gamma_Blue, int AM1_Red, int AM1_Green, int AM1_Blue, int AM0_Red, int AM0_Green, int AM0_Blue);
        byte[][] Get_Gamma_AM1_AM0_CMD(Gamma_Set Set, int band, RGB[] Band_Set_Gamma, RGB AM1, RGB AM0);
        byte[][] Get_Gamma_AM1_AM0_0x00_CMD(Gamma_Set Set, int band);


        //-------Read_Related_CMDs-------------
        //Get Reading DBV CMDs
        byte[] Get_Normal_Read_DBV_CMD();
        byte[] Get_AOD_Read_DBV_CMD();
       
        //Get Reading ELVSS & Vinit2 CMDs
        byte[] Get_Read_ELVSS_CMD(Gamma_Set Set);
        byte[] Get_Read_Cold_ELVSS_CMD(Gamma_Set Set);
        byte[] Get_Read_Vinit2_CMD(Gamma_Set Set);
        byte[] Get_Read_Cold_Vinit2_CMD(Gamma_Set Set);

        //Get Reading Vreg1 CMDs
        byte[] Get_Normal_Read_Vreg1_CMD(Gamma_Set Set);
        byte[] Get_Normal_Read_REF0_REF4095_CMD();

        //AM2&GR,AM1,AM0 
        byte[] Get_Read_Gamma_AM1_AM0_CMD(Gamma_Set Set,int band);

        //Get DBV 
        int Get_Normal_DBV(byte[] parameters,int normal_band_index);
        int Get_AOD_DBV(byte[] parameters, int AOD_band_index);

        //Get ELVSS&Vinit2 Volategs
        byte Get_Byte_ELVSS(byte[] parameters, int normal_band_index);
        byte Get_Byte_Cold_ELVSS(byte[] parameters, int normal_band_index);
        byte Get_Byte_Vinit2(byte[] parameters, int normal_band_index);
        byte Get_Byte_Cold_Vinit2(byte[] parameters, int normal_band_index);

        //Get Vreg1s
        int Get_Normal_Vreg1s(byte[] parameters, int normal_band_index);
        byte Get_Normal_REF0(byte[] parameters);
        byte Get_Normal_REF4095(byte[] parameters);

        //AM2&GR,AM1,AM0 
        RGB Get_Gamma(int gray, byte[] parameters);
        RGB Get_AM1(byte[] parameters);
        RGB Get_AM0(byte[] parameters);
    }
}
