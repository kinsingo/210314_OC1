#pragma once
#include "stdafx.h"
#include "Data_Structures.h"



class RGB_Compensation
{
protected:
	int Case;
	int Infinite_Loop;
	int XY_weight;
	int Lv_weight;

	Double_XYLv* Diff;//Diff_X, Diff_Y , Diff_Lv
	Double_XYLv* Limit;
	Double_XYLv* Measure;
	Double_XYLv* Target;
	int Gamma_Register_Limit;

	//Out Variable
	bool Gamma_Out_Of_Register_Limit;//Ref(Out)
	bool Within_Spec_Limit;//Ref(Out)
	Int_RGB* Gamma; //Gamma_R , Gamma_G , Gamma_B
    
	//Intenal Variables
	Int_RGB* cal_Gamma; 

protected:
	RGB_Compensation(int Gamma_Register_Limit,int Infinite_Loop,int XY_weight,int Lv_weight,Double_XYLv* Target,Double_XYLv* Measure, Double_XYLv* Limit,Int_RGB* Gamma);
	void Red_Gamma_Plus_Or_Minus(bool Plus);
	void Green_Gamma_Plus_Or_Minus(bool Plus);
	void Blue_Gamma_Plus_Or_Minus(bool Plus);
	RGB_Compensation();
	virtual ~RGB_Compensation();

public:
	int Get_Calculated_Gamma_R();
	int Get_Calculated_Gamma_G();
	int Get_Calculated_Gamma_B();
	bool Get_Gamma_Out_Of_Register_Limit();
	bool Get_Within_Spec_Limit();
};

class RGB_Compensate_State : public RGB_Compensation
{
public:
	bool Is_Any_RGB_Reach_Boundary_Condition();//Added on 200519
	bool Is_Calculated_RGB_All_Zero();//Added on 200519

	void Fast_RGB_Compensation();
	void Precise_RGB_Compensation();
	void Precise_RGB_Compensation_2();
	bool Get_Is_Lv_Priority(int Lv_Priority_Min,int Lv_Priority_Max,Double_XYLv LVPriority_Limit);
	void Lv_Priority_Compensation(int Lv_Priority_Min,int Lv_Priority_Max);
	void Full_Compensation(int loop_count,int& Infinite_Count);
	void Init_Algorithm_Applied_Full_Compensation(int loop_count, int& Infinite_Count);
	RGB_Compensate_State(int Gamma_Register_Limit,int Infinite_Loop,int XY_weight,int Lv_weight,Double_XYLv* Target,Double_XYLv* Measure, Double_XYLv* Limit,Int_RGB* Gamma);
	~RGB_Compensate_State();
};

class Vreg1_Compensate_State : public RGB_Compensation
{
private:
	int Vreg1_Register_Limit;
	int Vreg1;
	int cal_vreg1;
public:
	void Vreg1_Compensation();
	int Get_Calculated_Vreg1();
	Vreg1_Compensate_State(int Vreg1,int Vreg1_Register_Limit,int Gamma_Register_Limit,int Infinite_Loop,int XY_weight,int Lv_weight,Double_XYLv* Target,Double_XYLv* Measure, Double_XYLv* Limit,Int_RGB* Gamma);
	~Vreg1_Compensate_State();
};


