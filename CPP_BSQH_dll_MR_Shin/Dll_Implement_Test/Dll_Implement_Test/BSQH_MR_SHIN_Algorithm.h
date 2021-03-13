
#pragma once
#include "stdafx.h"
#include "SJH_Matrix.h"
#include "math.h"
#include <time.h>
#include <iostream>
#include <ctime>
#include <vector>
#include <algorithm>
#include <cctype>
#include <iomanip>
#include <sstream>
#include "OC_Weights.h"
#include "Optic_Compensation.h"
//#include "State.h"


using namespace std;
extern "C" __declspec (dllexport ) char* Get_Dll_Information();

//Add Extension_X/Y 
//Add Infinite_Count
//Add loop_count (19/04/20)
extern "C" __declspec (dllexport ) void Sub_Compensation(int loop_count,bool Infinite_Loop,int Infinite_Count,int &Gamma_R,
	int &Gamma_G,int &Gamma_B,double Measure_X,double Measure_Y,double Measure_Lv,double Target_X,double Target_Y,double Target_Lv
,double Limit_X,double Limit_Y,double Limit_Lv,double Extension_X,double Extension_Y, 
int Gamma_Register_Limit,bool &Gamma_Out_Of_Register_Limit,bool &Within_Spec_Limit);

//Add Vreg1_Infinite_Loop : Color Coordinate Infinite Loop Check (R/B) 
//Add Extension_X/Y 
//Add Vreg1_Infinite_Count
//Add loop_count (19/04/20)
extern "C" __declspec (dllexport ) void Vreg1_Compensation(int loop_count,bool Vreg1_Infinite_Loop,int Vreg1_Infinite_Count,
	int &Gamma_R,int &Vreg1,int &Gamma_B,double Measure_X,double Measure_Y,double Measure_Lv,double Target_X,double Target_Y,double Target_Lv
,double Limit_X,double Limit_Y,double Limit_Lv,double Extension_X,double Extension_Y 
, int Gamma_Register_Limit , int Vreg1_Register_Limit ,bool &Gamma_Or_Vreg1_Out_Of_Register_Limit,bool &Within_Spec_Limit);



extern "C" __declspec (dllexport ) void ELVSS_Compensation(bool& ELVSS_Find_Finish,double First_ELVSS,double& ELVSS,double& Vinit,double& First_Slope,double Vinit_Margin,double ELVSS_Margin
	,double Slope_Margin,double First_Measure_X,double First_Measure_Y,double First_Measure_Lv,double Measure_X,double Measure_Y,double Measure_Lv);
extern "C" __declspec (dllexport ) void ELVSS_Compensation_For_DP173(double ELVSS_Min,double ELVSS_Max,bool& ELVSS_Find_Finish,double First_ELVSS,double& ELVSS,double& First_Slope,double ELVSS_Margin
	,double Slope_Margin,double First_Measure_X,double First_Measure_Y,double First_Measure_Lv,double Measure_X,double Measure_Y,double Measure_Lv);


//190528 Add
extern "C" __declspec (dllexport ) void RGB_Gamma_Initial_Values(int Index_Count,int Min_Value,int Max_Value,int* Gamma_R, int* Gamma_G, int* Gamma_B,int& Out_R,int& Out_G,int& Out_B);
//190528 Add
extern "C" __declspec (dllexport ) int Vreg1_Initial_Values(int Index_Count,int* Vreg1);

//190926
// Mipi_Wx = "0x73 0xXX [0.2XXX ~ 0.3XXX]";
// return = "0xXX"
extern "C" __declspec (dllexport ) void Meta_WRGB_Mipi_Transfer(double Measured_Wx,double Measured_Wy,double Measured_Rx,double Measured_Ry,double Measured_Gx,double Measured_Gy,double Measured_Bx,double Measured_By
	,double Target_Wx,double Target_Wy,double Target_Rx,double Target_Ry,double Target_Gx,double Target_Gy,double Target_Bx,double Target_By
	,char** Mipi_Wx,char** Mipi_Wy,char** Mipi_Rx,char** Mipi_Ry,char** Mipi_Gx,char** Mipi_Gy,char** Mipi_Bx,char** Mipi_By);


//191016
// Input : string FV1_LVL[5:0] , string VCI1_LVL[5:0]
// Output : AM0_Resolution
extern "C" __declspec (dllexport ) double Get_Meta_SW43417_AM0_Resolution(const char* FV1,const char* VCI1);
extern "C" __declspec (dllexport ) double Get_DP173_EA9154_AM0_Resolution(const char* Hex_VREG1_REF1,const char* Hex_VREG1_REF2047);


//private function
char* Meta_WRGB_Mipi_Transfer_Sub(double Measured,double Target,int Color);

void Gamma_Plus_Or_Minus(bool Plus,int& cal_Gamma,int Gamma,int weight,int Gamma_Register_Limit,bool &Gamma_Out_Of_Register_Limit);

void New_Vreg1_Compensation_Rev8(int loop_count,bool Vreg1_Infinite_Loop,int Vreg1_Infinite_Count,
	int &Gamma_R,int &Vreg1,int &Gamma_B,double Measure_X,double Measure_Y,double Measure_Lv,double Target_X,double Target_Y,double Target_Lv
,double Limit_X,double Limit_Y,double Limit_Lv,double Extension_X,double Extension_Y 
, int Gamma_Register_Limit , int Vreg1_Register_Limit ,bool &Gamma_Or_Vreg1_Out_Of_Register_Limit,bool &Within_Spec_Limit);