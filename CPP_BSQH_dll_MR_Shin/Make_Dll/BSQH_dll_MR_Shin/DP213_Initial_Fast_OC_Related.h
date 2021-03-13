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
#include "SplineInterpolator.h"

//Calculating Initial RGBVreg1 Algorithm (Through Monotone-Cubic-Interpolation)
extern "C" __declspec (dllexport)void DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method_Through_MCI(double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, int& Vreg1_Dec_Init, int& Gamma_R, int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
    , double* Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0);


extern "C" __declspec (dllexport) void DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method_Through_MCI(int Vreg1, double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
    , double* Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0);


extern "C" __declspec (dllexport)  void DP213_Get_Intial_R_G_B_Using_3Points_Method_Through_MCI(double Combine_Lv_Ratio, double* Band_Voltage_AM1_R, double* Band_Voltage_AM1_G, double* Band_Voltage_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Band_Gray_Gamma_Red, int* Band_Gray_Gamma_Green, int* Band_Gray_Gamma_Blue, double* Band_Gray_Finally_Measured_Lv,
    int* Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage, double Voltage_VREF4095, double Voltage_VREF0);


//Calculating Initial RGBVreg1 Algorithm (Through Poly-Interpolation)
extern "C" __declspec (dllexport )void DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B,int& Vreg1_Dec_Init,int& Gamma_R,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095,double Voltage_VREF0);


extern "C" __declspec (dllexport ) void DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(int Vreg1,double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B,int& Gamma_R,int& Gamma_G,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095,double Voltage_VREF0);


extern "C" __declspec (dllexport )  void DP213_Get_Intial_R_G_B_Using_3Points_Method(double Combine_Lv_Ratio,double* Band_Voltage_AM1_R, double* Band_Voltage_AM1_G, double* Band_Voltage_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Band_Gray_Gamma_Red, int* Band_Gray_Gamma_Green, int* Band_Gray_Gamma_Blue, double* Band_Gray_Finally_Measured_Lv,
            int* Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,double Voltage_VREF4095, double Voltage_VREF0);



bool decrease(double a, double b);
double* Get_Previous_Band_Gamma_Voltage(double AM1_Voltage,int Previous_Band_Vreg1_Dec, int* Previous_Band_Gamma, double Voltage_VREF4095,double Voltage_VREF0);
void DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(int& Gamma_R,int& Gamma_G,int& Gamma_B,int Prev_Band_Gray255_Gamma_R,int Prev_Band_Gray255_Gamma_G,int Prev_Band_Gray255_Gamma_B);


//Get Vreg1/RGB Voltage From Gamma
extern "C" __declspec (dllexport ) double DP213_Get_Vreg1_Voltage(double Voltage_VREF4095,double Voltage_VREF0,int Dec_Vreg1);
extern "C" __declspec (dllexport ) double DP213_Get_GR_Gamma_Voltage(double AM1_Voltage, double Prev_GR_Gamma_Voltage, int Gamma_Dec, int gray);
extern "C" __declspec (dllexport ) double DP213_Get_AM2_Gamma_Voltage(double Voltage_VREF4095, double Vreg1_Voltage, int Dec_AM2); 
extern "C" __declspec (dllexport ) double DP213_Get_AM1_RGB_Voltage(double Voltage_VREF4095,double Vreg1_voltage,int Dec_AM1);           
extern "C" __declspec (dllexport ) double DP213_Get_AM0_RGB_Voltage(double Voltage_VREF4095,double Vreg1_voltage,int Dec_AM0);    

//Get Vreg1/RGB Gamma From Voltage
extern "C" __declspec (dllexport ) int DP213_Get_Vreg1_Dec(double Voltage_VREF4095,double Voltage_VREF0,double Vreg1_Voltage);
extern "C" __declspec (dllexport ) int DP213_Get_GR_Gamma_Dec(double AM1_Voltage,double Prev_GR_Gamma_Voltage,double Gamma_Voltage,int gray);

extern "C" __declspec (dllexport ) int DP213_Get_AM2_Gamma_Dec(double Voltage_VREF4095, double Vreg1_voltage, double AM2_voltage);        
extern "C" __declspec (dllexport ) int DP213_Get_AM1_RGB_Dec(double Voltage_VREF4095,double Vreg1_voltage,double AM1_Voltage);
extern "C" __declspec (dllexport ) int DP213_Get_AM0_RGB_Dec(double Voltage_VREF4095,double Vreg1_voltage,double AM0_Voltage);

//ELVSS & Vinit2
extern "C" __declspec (dllexport ) double DP213_ELVSS_Dec_to_Voltage(int ELVSS_Dec);
extern "C" __declspec (dllexport ) int DP213_ELVSS_Voltage_to_Dec(double ELVSS_Voltage);
extern "C" __declspec (dllexport ) double DP213_VINI2_Dec_to_Voltage(int VINIT2_Dec);
extern "C" __declspec (dllexport ) int DP213_VINI2_Voltage_to_Dec(double VINIT2_Voltage);

//VREF0
extern "C" __declspec (dllexport ) double DP213_VREF0_Dec_to_Voltage(int VREF0_Dec); 
extern "C" __declspec (dllexport ) int DP213_VREF0_Voltage_to_Dec(double VREF0_Voltage);

//VREF4095
extern "C" __declspec (dllexport ) double DP213_VREF4095_Dec_to_Voltage(int VREF4095_Dec);
extern "C" __declspec (dllexport ) int DP213_VREF4095_Voltage_to_Dec(double VREF4095_Voltage); 

//AM0 Resolution
extern "C" __declspec (dllexport ) double Get_DP213_EA9155_AM0_Resolution(int Dec_REF0,int Dev_REF4095);