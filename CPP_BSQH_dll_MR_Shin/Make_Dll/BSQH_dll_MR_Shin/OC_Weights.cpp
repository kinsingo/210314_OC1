#include "stdafx.h"
#include "OC_Weights.h"

OC_Weight::OC_Weight(Double_XYLv* Diff,Double_XYLv* Limit,Double_XYLv* Target,Int_RGB* Gamma,int Gamma_Register_Limit,int Infinite_Count,int loop_count)
{
	//std::cout<<"OC_Weight()"<<std::endl;
	this->Diff = new Double_XYLv(Diff->X,Diff->Y,Diff->Lv);
	this->Limit = new Double_XYLv(Limit->X,Limit->Y,Limit->Lv);
	this->Target = new Double_XYLv(Target->X,Target->Y,Target->Lv);
	this->Gamma = new Int_RGB(Gamma->R,Gamma->G,Gamma->B);
	
	this->Gamma_Register_Limit = Gamma_Register_Limit;
	this->Infinite_Count = Infinite_Count;
	this->loop_count = loop_count;
}

OC_Weight::~OC_Weight()
{
	//std::cout<<"~OC_Weight()"<<std::endl;
	delete this->Diff; 
	delete this->Limit;
	delete this->Gamma;
	delete this->Target;
}

int OC_Weight::Get_XY_Weight()
{
	return XY_weight;
}
	
int OC_Weight::Get_Lv_Weight()
{
	return Lv_weight;
}

Vreg1_Compensation_Weight::Vreg1_Compensation_Weight(Double_XYLv* Diff,Double_XYLv* Limit,Double_XYLv* Target,Int_RGB* Gamma,int Gamma_Register_Limit,int Infinite_Count,int loop_count,int Vreg1_Register_Limit,int Vreg1)
	:OC_Weight(Diff,Limit,Target,Gamma,Gamma_Register_Limit,Infinite_Count,loop_count)
{
	this->Vreg1_Register_Limit = Vreg1_Register_Limit;
	this->Vreg1 = Vreg1;
}


Sub_Compensation_Weight::Sub_Compensation_Weight(Double_XYLv* Diff,Double_XYLv* Limit,Double_XYLv* Target,Int_RGB* Gamma,int Gamma_Register_Limit,int Infinite_Count,int loop_count)
	:OC_Weight(Diff,Limit,Target,Gamma,Gamma_Register_Limit,Infinite_Count,loop_count)
{

}



void Vreg1_Compensation_Weight::Set_XY_Weight_and_LV_Weight_For_Compensation()
{
	int Diff_XY_Times = min((int)(abs(Diff->X) / Limit->X), (int)(abs(Diff->Y) / Limit->Y));
	XY_weight = Diff_XY_Times / 5;


	int Diff_Lv_Times = (int)(abs(Diff->Lv) / Limit->Lv);
	Lv_weight = Diff_Lv_Times;

	if(Vreg1_Register_Limit > 1030) Lv_weight *= 1;//for Vreg1_Register_Limit = 2047 (2050 > X > 1030)
	else if(Vreg1_Register_Limit > 520) Lv_weight = (int)(Lv_weight * 0.5);//for Vreg1_Register_Limit = 1023 (1030 > X > 520)
	
	if ((abs(Diff->Lv) < (Limit->Lv * 2)) && (abs(Diff->X) < (Limit->X * 2)) && (abs(Diff->Y) < (Limit->Y * 2)))
	{
		XY_weight = 1;
		Lv_weight = 1;
	}

	if ((Gamma->R > Gamma_Register_Limit - 15) || (Gamma->B > Gamma_Register_Limit - 15) || (Gamma->R < 15) || (Gamma->B < 15) || (loop_count > 30)) XY_weight = 1;
	if (XY_weight < 1) XY_weight = 1;
	if(((Vreg1 > (Vreg1_Register_Limit-30))&&(Vreg1 < (30)))|| (loop_count > 30)) Lv_weight = 1;
	if(Lv_weight < 1) Lv_weight = 1;
}

void Sub_Compensation_Weight::Set_XY_Weight_and_LV_Weight_For_Compensation()
{
	int Diff_XY_Times = min((int)(abs(Diff->X) / Limit->X), (int)(abs(Diff->Y) / Limit->Y));
	XY_weight = Diff_XY_Times / 5;

	int Diff_Lv_Times = (int)(abs(Diff->Lv) / Limit->Lv);
	Lv_weight = Diff_Lv_Times / 5;

    if((Gamma->R > Gamma_Register_Limit-15) || (Gamma->G > Gamma_Register_Limit-15) || (Gamma->B > Gamma_Register_Limit-15)
    || (Gamma->R < 15) || (Gamma->G < 15) || (Gamma->B < 15) || (Gamma_Register_Limit < 200))
	{
		XY_weight = 1;
		Lv_weight = 1;
	}

	if((abs(Diff->Lv) < (Limit->Lv * 2))&&(abs(Diff->X) < (Limit->X * 2))&&(abs(Diff->Y) < (Limit->Y * 2)))
	{
		XY_weight = 1;
		Lv_weight = 1;
	}

	if(XY_weight < 1) XY_weight = 1;
	if(Lv_weight < 1) Lv_weight = 1;

    //Added this condition to prevent power shutdown by reaching too high luminance	(ex)1000nit) 210113
	if (Target->Lv > 500)
	{
		if (XY_weight > 3) XY_weight = 3;
		if (Lv_weight > 3) Lv_weight = 3;
	}
}



