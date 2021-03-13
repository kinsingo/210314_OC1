#pragma once
#include "stdafx.h"
#include "SJH_Matrix.h"
#include <iostream>
#include <fstream>
#include <iterator>
#include <algorithm>
#include <sstream>
#include <stdio.h>
#include <time.h>
//----------------------Meta Initials Fast OC Related-------------------

//Fast Initial Related (public , Ok)
extern "C" __declspec (dllexport ) int Test_checkBox_Get_HBM_Equation(int band,double* HBM_Gamma_Voltage_G,double* HBM_Gray_Target,double* G255_Band_Target,int* G255_Band_Gamma_G
	,double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt , double VREG1_REF205_volt, double F7);
//Fast Initial Related (private , Ok)
int Meta_Get_Vreg1_Dec(double Vreg1_Voltage, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt , double VREG1_REF205_volt, double F7);

//Fast Initial Related (public , Ok) // Get --> E7/F7/VREG1_REF818_volt/VREG1_REF614_volt/VREG1_REF409_volt/VREG1_REF205_volt
extern "C" __declspec (dllexport )double Get_E7 (double ELVDD,int dec_FV1);
extern "C" __declspec (dllexport )double Get_F7 (double ELVDD,int dec_VCI1);
extern "C" __declspec (dllexport )double Get_VREG1_REF818_volt (double E7,double F7,int Dec_VREG1_REF818);
extern "C" __declspec (dllexport )double Get_VREG1_REF614_volt (double E7,double F7,int Dec_VREG1_REF614);
extern "C" __declspec (dllexport )double Get_VREG1_REF409_volt (double E7,double F7,int Dec_VREG1_REF409);
extern "C" __declspec (dllexport )double Get_VREG1_REF205_volt (double E7,double F7,int Dec_VREG1_REF205);
           
//Fast Initial Related (private , Ok)
extern "C" __declspec (dllexport ) double Meta_Get_Normal_Gamma_Voltage(double L, double H, int Gamma_Dec);
extern "C" __declspec (dllexport ) double Meta_Get_AM2_Voltage(double F7, double Vreg1_Voltage, int Gamma_Dec);
extern "C" __declspec (dllexport ) int Meta_Get_Gamma_From_Normal_Voltage(double L, double H, double Vdata);
extern "C" __declspec (dllexport ) int Meta_Get_Gamma_From_AM2_Voltage(double F7, double Vreg1_Voltage, double Vdata);

//Fast Initial Related (public , Ok)
extern "C" __declspec (dllexport )double Meta_Get_Vreg1_Voltage(int Dec_Vreg1,double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);
        
//Fast Initial Related (public , Ok)
double* Get_HBM_Gamma_Green_Voltage (int* HBM_Gamma_Green,double E7,double VREG1_REF818_volt,double VREG1_REF614_volt,double VREG1_REF409_volt,double VREG1_REF205_volt,double F7);
extern "C" __declspec (dllexport )int Dll_Meta_Get_Normal_Initial_Vreg1_Fx_HBM(bool* Selected_Band,int* HBM_Gamma_Green,int Vreg1_Dec_Init,int band,double band_Target_Lv,int Previous_Band_G255_Green_Gamma,int Previous_Band_Vreg1_Dec,double* HBM_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt
            , double VREG1_REF205_volt, double F7);

//Fast Initial Related (public Ok)
double* Get_Previous_Band_Gamma_Green_Voltage(int Previous_Band_Vreg1_Dec, int* Previous_Band_Gamma_Green, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);
extern "C" __declspec (dllexport )int Dll_Meta_Get_Normal_Initial_Vreg1_Fx_Previous_Band(bool* Selected_Band, int* Previous_Band_Gamma_Green, int Vreg1_Dec_Init, int band, double band_Target_Lv, int Previous_Band_G255_Green_Gamma, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);
extern "C" __declspec (dllexport )void Dll_Meta_Get_Normal_Initial_Vreg1_R_B_Fx_Previous_Band(int& Vreg1_Dec_Init,int& Gamma_R,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);


//Fast Initial Related (public Ok)
//Output : Gamma_R/G/B (Call by reference)
extern "C" __declspec (dllexport )void Dll_Meta_Get_First_Gamma_Fx_HBM(int& Gamma_R,int& Gamma_G,int& Gamma_B, bool* Selected_Band, int* HBM_Gamma_R, int* HBM_Gamma_G, int* HBM_Gamma_B, double* HBM_Target_Lv,
            int Current_Band_Dec_Vreg1, int band, int gray, double Target_Lv,double Prvious_Gray_Gamma_R_Voltage,double Prvious_Gray_Gamma_G_Voltage,double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);