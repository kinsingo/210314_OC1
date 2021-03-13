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

//Calculate and Get Initial R/Vreg1/B (Gray255)
extern "C" __declspec (dllexport )void DP173_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B,int& Vreg1_Dec_Init,int& Gamma_R,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Target_Lv, double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1);

//Calculate and Get Initial R/G/B (Gray255)
extern "C" __declspec (dllexport )void DP173_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(int Vreg1,int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B,int& Gamma_R,int& Gamma_G,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Target_Lv, double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1);

//Calculate and Get Initial R/G/B (Gray191~G4)
extern "C" __declspec (dllexport )  void Get_Initial_Gamma_Fx_3points_Combine_Points(int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Band_Gray_Gamma_Red, int* Band_Gray_Gamma_Green, int* Band_Gray_Gamma_Blue, double* Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int* Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1
            , double Fx_3points_Combine_LV_1, double Fx_3points_Combine_LV_2, double Fx_3points_Combine_LV_3, double Fx_3points_Combine_Lv_Distance);

extern "C" __declspec (dllexport )  void Get_Initial_Gamma_Fx_3points_Combine_Points_2(double Combine_Lv_Ratio,int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Band_Gray_Gamma_Red, int* Band_Gray_Gamma_Green, int* Band_Gray_Gamma_Blue, double* Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int* Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1
            , double Fx_3points_Combine_LV_1, double Fx_3points_Combine_LV_2, double Fx_3points_Combine_LV_3, double Fx_3points_Combine_Lv_Distance);

//Get Vreg1/RGB Voltage From Gamma
extern "C" __declspec (dllexport ) double DP173_Get_Vreg1_Voltage(int Dec_Vreg1,double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1);//Verify OK (200205)
extern "C" __declspec (dllexport ) double DP173_Get_GR_Gamma_Voltage(double Voltage_VREG1_REF2047,double Vreg1_voltage,int Dec_AM1, double Prev_GR_Gamma_Voltage, int Gamma_Dec,int gray);//Verify OK (200205)
extern "C" __declspec (dllexport ) double DP173_Get_AM2_Gamma_Voltage(double Voltage_VREG1_REF2047, double Vreg1_Voltage, int Gamma_Dec); //Verify OK (200205)

//Get Vreg1/RGB Gamma From Voltage
extern "C" __declspec (dllexport ) int DP173_Get_Vreg1_Dec(double Vreg1_Voltage,double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1);//Verify OK (200205)
extern "C" __declspec (dllexport ) int DP173_Get_GR_Gamma_Dec(double Voltage_VREG1_REF2047,double Vreg1_voltage,int Dec_AM1 , double Prev_GR_Gamma_Voltage, double Gamma_Voltage ,int gray);//Verify OK (200205)
extern "C" __declspec (dllexport ) int DP173_Get_AM2_Gamma_Dec(double Voltage_VREG1_REF2047, double Vreg1_voltage, double AM2_Gamma_voltage);//Verify OK (200205)
        
//Get REF Voltages
extern "C" __declspec (dllexport ) void Get_REF_Voltages(int Dec_VREG1_REF2047,int Dec_VREG1_REF1635,int Dec_VREG1_REF1227,int Dec_VREG1_REF815,int Dec_VREG1_REF407,int Dec_VREG1_REF63,int Dec_VREG1_REF1
	,double& Voltage_VREG1_REF2047,double& Voltage_VREG1_REF1635,double& Voltage_VREG1_REF1227,double& Voltage_VREG1_REF815,double& Voltage_VREG1_REF407,double& Voltage_VREG1_REF63,double& Voltage_VREG1_REF1);


extern "C" __declspec (dllexport ) double DP173_Get_AM1_RGB_Voltage(double Voltage_VREG1_REF2047,double Vreg1_voltage,int Dec_AM1);           
extern "C" __declspec (dllexport ) double DP173_Get_AM0_RGB_Voltage(double Voltage_VREG1_REF2047,double Vreg1_voltage,int Dec_AM0);    

extern "C" __declspec (dllexport ) int DP173_Get_AM1_RGB_Dec(double Voltage_VREG1_REF2047,double Vreg1_voltage,double AM1_Voltage);
extern "C" __declspec (dllexport ) int DP173_Get_AM0_RGB_Dec(double Voltage_VREG1_REF2047,double Vreg1_voltage,double AM0_Voltage);

//Inner Function
double* Get_Previous_Band_Gamma_Voltage(int Dec_AM1,int Previous_Band_Vreg1_Dec, int* Previous_Band_Gamma, double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1);
bool desc(double a, double b);

