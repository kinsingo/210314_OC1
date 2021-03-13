#pragma once
#include "stdafx.h"
#include "math.h"
#include <time.h>
#include <iostream>
#include <ctime>
#include <vector>
#include <algorithm>
#include <cctype>
#include <iomanip>
#include <sstream>
#include "Data_Structures.h"

class OC_Weight
{
protected:
	//Get by Methods
	int XY_weight;
	int Lv_weight;

	//Set by Constructor
	Double_XYLv* Diff; 
	Double_XYLv* Limit;
	Double_XYLv* Target;
	Int_RGB* Gamma;
	int Gamma_Register_Limit;
	int Infinite_Count;
	int loop_count;

public:
	OC_Weight(Double_XYLv* Diff,Double_XYLv* Limit,Double_XYLv* Target,Int_RGB* Gamma,int Gamma_Register_Limit,int Infinite_Count,int loop_count);
	~OC_Weight();
	int Get_XY_Weight();
	int Get_Lv_Weight();
	virtual void Set_XY_Weight_and_LV_Weight_For_Compensation() = 0;//Abstract Method
};

class Vreg1_Compensation_Weight : OC_Weight
{
private:
	int Vreg1_Register_Limit;
	int Vreg1;

public:
	Vreg1_Compensation_Weight(Double_XYLv* Diff,Double_XYLv* Limit,Double_XYLv* Target,Int_RGB* Gamma,int Gamma_Register_Limit,int Infinite_Count,int loop_count,int Vreg1_Register_Limit,int Vreg1);
	void Set_XY_Weight_and_LV_Weight_For_Compensation() override;
};

class Sub_Compensation_Weight : OC_Weight
{
public:
	Sub_Compensation_Weight(Double_XYLv* Diff,Double_XYLv* Limit,Double_XYLv* Target,Int_RGB* Gamma,int Gamma_Register_Limit,int Infinite_Count,int loop_count);
	void Set_XY_Weight_and_LV_Weight_For_Compensation() override;
};

