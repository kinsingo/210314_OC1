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
	int count = 0 ;

	if(abs(Diff->X) > Limit->X * 165 && abs(Diff->Y) > Limit->Y * 165) XY_weight = 55;
	else if(abs(Diff->X) > Limit->X * 150 && abs(Diff->Y) > Limit->Y * 150) XY_weight = 50;
	else if(abs(Diff->X) > Limit->X * 135 && abs(Diff->Y) > Limit->Y * 135) XY_weight = 45;
	else if(abs(Diff->X) > Limit->X * 120 && abs(Diff->Y) > Limit->Y * 120) XY_weight = 40;
	else if(abs(Diff->X) > Limit->X * 105 && abs(Diff->Y) > Limit->Y * 105) XY_weight = 35;
	else if(abs(Diff->X) > Limit->X * 90 && abs(Diff->Y) > Limit->Y * 90) XY_weight = 30;
	else if(abs(Diff->X) > Limit->X * 75 && abs(Diff->Y) > Limit->Y * 75) XY_weight = 25;
	else if(abs(Diff->X) > Limit->X * 60 && abs(Diff->Y) > Limit->Y * 60) XY_weight = 20;
	else if(abs(Diff->X) > Limit->X * 45 && abs(Diff->Y) > Limit->Y * 45) XY_weight = 15;
	else if(abs(Diff->X) > Limit->X * 30 && abs(Diff->Y) > Limit->Y * 30) XY_weight = 10;
	else if(abs(Diff->X) > Limit->X * 27 && abs(Diff->Y) > Limit->Y * 27) XY_weight = 9;
	else if(abs(Diff->X) > Limit->X * 24 && abs(Diff->Y) > Limit->Y * 24) XY_weight = 8;
	else if(abs(Diff->X) > Limit->X * 21 && abs(Diff->Y) > Limit->Y * 21) XY_weight = 7;
	else if(abs(Diff->X) > Limit->X * 18 && abs(Diff->Y) > Limit->Y * 18) XY_weight = 6;
	else if(abs(Diff->X) > Limit->X * 15 && abs(Diff->Y) > Limit->Y * 15) XY_weight = 5;
	else if(abs(Diff->X) > Limit->X * 12 && abs(Diff->Y) > Limit->Y * 12) XY_weight = 4;
	else if(abs(Diff->X) > Limit->X * 9 && abs(Diff->Y) > Limit->Y * 9) XY_weight = 3;
	else if(abs(Diff->X) > Limit->X * 6 && abs(Diff->Y) > Limit->Y * 6) XY_weight = 2;
	else XY_weight = 1; 

	if((Gamma->R > Gamma_Register_Limit-15) || (Gamma->B > Gamma_Register_Limit-15) || (Gamma->R < 15) || (Gamma->B < 15) || (loop_count > 30)) XY_weight = 1;

	 //----- For Vreg1 Max = 2047 ------
	if((abs(Diff->Lv) > (Limit->Lv * 50)))Lv_weight = 50;
	else if((abs(Diff->Lv) > (Limit->Lv * 40)))Lv_weight = 40;
	else if((abs(Diff->Lv) > (Limit->Lv * 30)))Lv_weight = 30;
	else if((abs(Diff->Lv) > (Limit->Lv * 20)))Lv_weight = 20;
	else if((abs(Diff->Lv) > (Limit->Lv * 15)))Lv_weight = 15;
	else if((abs(Diff->Lv) > (Limit->Lv * 10)))Lv_weight = 10;
	else if((abs(Diff->Lv) > (Limit->Lv * 9)))Lv_weight = 9;
	else if((abs(Diff->Lv) > (Limit->Lv * 8)))Lv_weight = 8;
	else if((abs(Diff->Lv) > (Limit->Lv * 7)))Lv_weight = 7;
	else if((abs(Diff->Lv) > (Limit->Lv * 6)))Lv_weight = 6;
	else if((abs(Diff->Lv) > (Limit->Lv * 5)))Lv_weight = 5;
	else if((abs(Diff->Lv) > (Limit->Lv * 4)))Lv_weight = 4;
	else if((abs(Diff->Lv) > (Limit->Lv * 3)))Lv_weight = 3;
	//----- For Vreg1 Max = 2047 ------

	if(Vreg1_Register_Limit > 2050) Lv_weight *= 2;//for Vreg1_Register_Limit = 4095 (X > 2050)
	else if(Vreg1_Register_Limit > 1030) Lv_weight *= 1;//for Vreg1_Register_Limit = 2047 (2050 > X > 1030)
	else if(Vreg1_Register_Limit > 520) Lv_weight = (int)(Lv_weight * 0.5);//for Vreg1_Register_Limit = 1023 (1030 > X > 520)
	
	if(((Vreg1 > (Vreg1_Register_Limit-30))&&(Vreg1 < (30)))|| (loop_count > 30)) Lv_weight = 1;
	if(Lv_weight < 1) Lv_weight = 1;
	if(XY_weight < 1) XY_weight = 1;
}

void Sub_Compensation_Weight::Set_XY_Weight_and_LV_Weight_For_Compensation()
{
	if(abs(Diff->X) > Limit->X * 165 && abs(Diff->Y) > Limit->Y * 165) XY_weight = 55;
	else if(abs(Diff->X) > Limit->X * 150 && abs(Diff->Y) > Limit->Y * 150) XY_weight = 50;
	else if(abs(Diff->X) > Limit->X * 135 && abs(Diff->Y) > Limit->Y * 135) XY_weight = 45;
	else if(abs(Diff->X) > Limit->X * 120 && abs(Diff->Y) > Limit->Y * 120) XY_weight = 40;
	else if(abs(Diff->X) > Limit->X * 105 && abs(Diff->Y) > Limit->Y * 105) XY_weight = 35;
	else if(abs(Diff->X) > Limit->X * 90 && abs(Diff->Y) > Limit->Y * 90) XY_weight = 30;
	else if(abs(Diff->X) > Limit->X * 75 && abs(Diff->Y) > Limit->Y * 75) XY_weight = 25;
	else if(abs(Diff->X) > Limit->X * 60 && abs(Diff->Y) > Limit->Y * 60) XY_weight = 20;
	else if(abs(Diff->X) > Limit->X * 45 && abs(Diff->Y) > Limit->Y * 45) XY_weight = 15;
	else if(abs(Diff->X) > Limit->X * 30 && abs(Diff->Y) > Limit->Y * 30) XY_weight = 10;
	else if(abs(Diff->X) > Limit->X * 27 && abs(Diff->Y) > Limit->Y * 27) XY_weight = 9;
	else if(abs(Diff->X) > Limit->X * 24 && abs(Diff->Y) > Limit->Y * 24) XY_weight = 8;
	else if(abs(Diff->X) > Limit->X * 21 && abs(Diff->Y) > Limit->Y * 21) XY_weight = 7;
	else if(abs(Diff->X) > Limit->X * 18 && abs(Diff->Y) > Limit->Y * 18) XY_weight = 6;
	else if(abs(Diff->X) > Limit->X * 15 && abs(Diff->Y) > Limit->Y * 15) XY_weight = 5;
	else if(abs(Diff->X) > Limit->X * 12 && abs(Diff->Y) > Limit->Y * 12) XY_weight = 4;
	else if(abs(Diff->X) > Limit->X * 9 && abs(Diff->Y) > Limit->Y * 9) XY_weight = 3;
	else if(abs(Diff->X) > Limit->X * 6 && abs(Diff->Y) > Limit->Y * 6) XY_weight = 2;
	else XY_weight = 1; 
	
	if(abs(Diff->Lv) > (Limit->Lv * 240))Lv_weight = 80;
	else if(abs(Diff->Lv) > (Limit->Lv * 180))Lv_weight = 60;
	else if(abs(Diff->Lv) > (Limit->Lv * 165))Lv_weight = 55;
	else if(abs(Diff->Lv) > (Limit->Lv * 150))Lv_weight = 50;
	else if(abs(Diff->Lv) > (Limit->Lv * 135))Lv_weight = 45;
	else if(abs(Diff->Lv) > (Limit->Lv * 120))Lv_weight = 40;
	else if(abs(Diff->Lv) > (Limit->Lv * 105))Lv_weight = 35;
	else if(abs(Diff->Lv) > (Limit->Lv * 90))Lv_weight = 30;
	else if(abs(Diff->Lv) > (Limit->Lv * 75))Lv_weight = 25;
	else if(abs(Diff->Lv) > (Limit->Lv * 60))Lv_weight = 20;
	else if(abs(Diff->Lv) > (Limit->Lv * 45))Lv_weight = 15;
	else if(abs(Diff->Lv) > (Limit->Lv * 30))Lv_weight = 10;
	else if(abs(Diff->Lv) > (Limit->Lv * 27))Lv_weight = 9;
	else if(abs(Diff->Lv) > (Limit->Lv * 24))Lv_weight = 8;
	else if(abs(Diff->Lv) > (Limit->Lv * 21))Lv_weight = 7;
	else if(abs(Diff->Lv) > (Limit->Lv * 18))Lv_weight = 6;
	else if(abs(Diff->Lv) > (Limit->Lv * 15))Lv_weight = 5;
	else if(abs(Diff->Lv) > (Limit->Lv * 12))Lv_weight = 4;
	else if(abs(Diff->Lv) > (Limit->Lv * 9))Lv_weight = 3;
	else if(abs(Diff->Lv) > (Limit->Lv * 6))Lv_weight = 2;
	else Lv_weight = 1;

	if(Target->Lv <= 0.1 && Target->Lv >= 0.06)
	{ 
		Lv_weight = (int)((double)Lv_weight * 1.5);
		XY_weight = (int)((double)XY_weight * 1.5);
	}
	else if(Target->Lv < 0.06 && Target->Lv >= 0.03)
	{
		Lv_weight = (int)((double)Lv_weight * 2);
		XY_weight = (int)((double)XY_weight * 2);
	}
	else if(Target->Lv < 0.03)
	{
		Lv_weight = (int)((double)Lv_weight * 2.5);
		XY_weight = (int)((double)XY_weight * 2.5);
	}

    if((Gamma->R > Gamma_Register_Limit-15) || (Gamma->G > Gamma_Register_Limit-15) || (Gamma->B > Gamma_Register_Limit-15)
    || (Gamma->R < 15) || (Gamma->G < 15) || (Gamma->B < 15) || (Gamma_Register_Limit < 200) || (loop_count > 30))
	{
		XY_weight = 1;
		Lv_weight = 1;
	}

	if((abs(Diff->Lv) < (Limit->Lv * 2))&&(abs(Diff->X) < (Limit->X * 2))&&(abs(Diff->Y) < (Limit->Y * 2)))
	{
		XY_weight = 1; //Added on 200220
		Lv_weight = 1; //Added on 200220
	}

	if(Infinite_Count > 0)
	{
		XY_weight = 1; //Added on 200220
		Lv_weight = 1; //Added on 200220
	}

	if(XY_weight < 1) XY_weight = 1;
	if(Lv_weight < 1) Lv_weight = 1;
}