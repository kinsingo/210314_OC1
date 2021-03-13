
#include "stdafx.h"
#include "BSQH_MR_SHIN_Algorithm.h"


using namespace std;
typedef unsigned char BYTE;
SJH_Matrix M;

extern "C" __declspec (dllexport ) char* Get_Dll_Information()
{
	//----History----
	return  "GA Dll updated on 2020-04-02";
}

//bool Vreg1_Infinite_Loop : Color Coordinate Infinite Loop Check (R/B) 
extern "C" __declspec (dllexport ) 
void Vreg1_Compensation(int loop_count,bool Vreg1_Infinite_Loop,int Vreg1_Infinite_Count,int &Gamma_R,int &Vreg1,int &Gamma_B,double Measure_X,double Measure_Y,double Measure_Lv,double Target_X,double Target_Y,double Target_Lv
,double Limit_X,double Limit_Y,double Limit_Lv,double Extension_X,double Extension_Y , int Gamma_Register_Limit , int Vreg1_Register_Limit ,bool &Gamma_Or_Vreg1_Out_Of_Register_Limit,bool &Within_Spec_Limit)
{
	//Added for Rev8 : Weight Changed
	New_Vreg1_Compensation_Rev8(loop_count,Vreg1_Infinite_Loop,Vreg1_Infinite_Count,Gamma_R,Vreg1,Gamma_B,Measure_X,Measure_Y,Measure_Lv,Target_X,Target_Y,Target_Lv,Limit_X,Limit_Y,Limit_Lv,Extension_X,Extension_Y,Gamma_Register_Limit,Vreg1_Register_Limit,Gamma_Or_Vreg1_Out_Of_Register_Limit,Within_Spec_Limit);
}

void New_Vreg1_Compensation_Rev8(int loop_count,bool Vreg1_Infinite_Loop,int Vreg1_Infinite_Count,
	int &Gamma_R,int &Vreg1,int &Gamma_B,double Measure_X,double Measure_Y,double Measure_Lv,double Target_X,double Target_Y,double Target_Lv
,double Limit_X,double Limit_Y,double Limit_Lv,double Extension_X,double Extension_Y 
, int Gamma_Register_Limit , int Vreg1_Register_Limit ,bool &Gamma_Or_Vreg1_Out_Of_Register_Limit,bool &Within_Spec_Limit)
{
	if(Vreg1_Infinite_Count >= 3)
	{
		Limit_X += Extension_X;
		Limit_Y += Extension_Y;
	}
	int Case = rand()%2; // 0,1 Random

	Double_XYLv* Diff = new Double_XYLv(Measure_X - Target_X,Measure_Y - Target_Y,Measure_Lv - Target_Lv);
	Double_XYLv* Limit = new Double_XYLv(Limit_X,Limit_Y,Limit_Lv);
	Double_XYLv* Target = new Double_XYLv(Target_X,Target_Y,Target_Lv);
	Int_RGB* Gamma = new Int_RGB(Gamma_R,0,Gamma_B);
	OC_Weight* obj_weight = (OC_Weight *)new Vreg1_Compensation_Weight(Diff,Limit,Target,Gamma,Gamma_Register_Limit,Vreg1_Infinite_Loop,loop_count,Vreg1_Register_Limit,Vreg1);
	
	obj_weight->Set_XY_Weight_and_LV_Weight_For_Compensation();
	int weight = obj_weight->Get_XY_Weight();
	int Lv_weight = obj_weight->Get_Lv_Weight();


	// Gamma = Gamma + Cal_Gamma;
	// Vreg1 = Vreg1 + Cal_Vreg1;
	int cal_R = 0;int cal_vreg1 = 0;int cal_B = 0;

	Within_Spec_Limit = false;
    
	//If(Diff < Limit) , Spec In
	if(abs(Diff->X) < Limit_X && abs(Diff->Y) < Limit_Y && abs(Diff->Lv) < Limit_Lv)
	{
		Within_Spec_Limit = true;
	}
	// Spec Out
	//double Diff_X = Measure_X - Target_X;
	else 
	{
		if(Gamma_R == 0 || Vreg1 == 0 || Gamma_B == 0
		|| Gamma_R == Gamma_Register_Limit || Vreg1 == Vreg1_Register_Limit || Gamma_B == Gamma_Register_Limit)
		{
			Gamma_Or_Vreg1_Out_Of_Register_Limit = true;
		}
		else
		{
			if((abs(Diff->Lv) > (Limit_Lv * 3))||((abs(Diff->Lv) > Limit_Lv)&&(Vreg1_Infinite_Loop)))//0.05%
			{
				

				if(Measure_Lv> Target_Lv) //������(Lv)�� Target ��� ū ���
				{
					if(Vreg1_Infinite_Loop)cal_vreg1 -= 1;
					else cal_vreg1 -= Lv_weight;
				}
				else if(Measure_Lv < Target_Lv) //������(Lv)�� Target ��� ���� ���
				{
					if(Vreg1_Infinite_Loop)cal_vreg1 += 1;
					else cal_vreg1 += Lv_weight;
				}
				else //This Can't happen, use this just to Debug the Program
				{
					cal_vreg1 = Vreg1_Register_Limit;
				}
			}
			else //27 cases
			{	
				// X,Y,Lv ��� ���� � (Case 1,3,19,21,7,9,25,27) 8ea cases
				if(abs(Diff->X) > Limit_X && abs(Diff->Y) > Limit_Y && abs(Diff->Lv) > Limit_Lv)
				{
					if((Measure_X > Target_X) &&  (Measure_Y > Target_Y) && (Measure_Lv > Target_Lv))// x--/y-/Lv- (R-/B+) : Case 1
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B += 1;
							else cal_R -= 1;
						}
						else 
						{
							cal_R -= weight; cal_B += weight;
						}
					}
					else if((Measure_X > Target_X) &&  (Measure_Y > Target_Y)&& (Measure_Lv < Target_Lv))//x-/y-/Lv+ (B+) : Case 3
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B += 1;
							else cal_B += 2;
						}
						else 
					    {
							cal_B += weight;
						}
					}
					else if((Measure_X < Target_X) && (Measure_Y > Target_Y)) // x+/y- (R+/B+) : Case 19,21 
					{
						if(Vreg1_Infinite_Loop)
						{
						   if(Case == 0)cal_B += 1;
						   else cal_R += 1;
						}
						else 
						{
						   cal_R += weight; cal_B += weight;
						}
					}
					else if((Measure_X > Target_X) &&  (Measure_Y < Target_Y) ) // x-/y+ (R-/B-) : Case 7,9
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B -= 1;
							else cal_R -= 1;
						}
						else 
						{
							cal_R -= weight; cal_B -= weight;
						}
					}
					else if((Measure_X < Target_X) &&  (Measure_Y < Target_Y) && (Measure_Lv > Target_Lv))//x+/y+/Lv- (B-) : Case 25
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B -= 1;
							else cal_B -= 2;
						}
						else
						{
							cal_B -= weight;
						}
					}
					else if((Measure_X < Target_X) &&  (Measure_Y < Target_Y) && (Measure_Lv < Target_Lv))//x+/y+/Lv+ (R+/B-) : Case 27
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B -= 1;
							else cal_R += 1;
						}
						else
						{
							cal_R += weight; cal_B -= weight;
						}
					}
					else
					{
						//Nothing Should Happen
						cal_R -= 1000; 
						cal_B -= 1000;
						cal_vreg1 -= 100; 
					}
				}
				else if(abs(Diff->X) > Limit_X && abs(Diff->Y) > Limit_Y && abs(Diff->Lv) < Limit_Lv)//X,Y ���� �
				{
					if((Measure_X > Target_X) &&  (Measure_Y > Target_Y)) // x-/y- (B+) : Case 2
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B += 1;
							else cal_B += 2;
						}
						else
						{
							cal_B += weight;
						}
					}
					else if((Measure_X < Target_X) &&  (Measure_Y > Target_Y)) //x+/y- (R+/B+) : Case 20
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B += 1;
							else cal_R += 1;
						}
						else
						{
							cal_R += weight; cal_B += weight;
						}
					}
					else if((Measure_X > Target_X) &&  (Measure_Y < Target_Y)) //x-/y+ (R-/B-) : Case 8
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B -= 1;
							else cal_R -= 1;
						}
						else
						{
							cal_R -= weight; cal_B -= weight;
						}
						
					}
					else if((Measure_X < Target_X) &&  (Measure_Y < Target_Y)) // x+/y+ (B-) : Case 26
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B -= 1;
							else cal_B -= 2;
						}
						else
						{
							cal_B -= weight;
						}
					}
					else
					{
						//Nothing Should Happen
						cal_R -= 1000; 
						cal_B -= 1000;
						cal_vreg1 -= 200; 
					}
				}
				else if(abs(Diff->X) > Limit_X && abs(Diff->Y) < Limit_Y && abs(Diff->Lv) > Limit_Lv)//X,Lv ���� �
				{
					if((Measure_X > Target_X)) // x- (R-) : Case 4,6
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_R -= 1;
							else cal_R -= 2;
						}
						else
						{
							cal_R -= weight;
						}
						
					}
					else if((Measure_X < Target_X))// x+ (R+) : Case 22,24
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_R += 1;
							else cal_R += 2;
						}
						else
						{
							cal_R += weight;
						}
					}
					else
					{
						//Nothing Should Happen
						cal_R -= 1000; 
						cal_B -= 1000;
						cal_vreg1 -= 300;
					}
				}
				else if(abs(Diff->X) < Limit_X && abs(Diff->Y) > Limit_Y && abs(Diff->Lv) > Limit_Lv)//Y,Lv ���� �
				{
					if((Measure_Y > Target_Y)) // y- (R+/B+) : Case 10,12
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B += 1;
							else cal_R += 1;
						}
						else
						{
							cal_R += weight; cal_B += weight;
						}
					}
					else if((Measure_Y < Target_Y)) // y+ (R-/B-) : Case 16,18
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B -= 1;
							else cal_R -= 1;
						}
						else
						{
							cal_R -= weight; cal_B -= weight;
						}
					}
					else
					{
						//Nothing Should Happen
						cal_R -= 1000; 
						cal_B -= 1000;
						cal_vreg1 -= 400;
					}
				}
				else if(abs(Diff->X) > Limit_X && abs(Diff->Y) < Limit_Y && abs(Diff->Lv) < Limit_Lv)// X�� ���� � 
				{
					if((Measure_X > Target_X)) // x- (R-) : Case 5
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_R -= 1;
							else cal_R -= 2;
						}
						else
						{
							cal_R -= weight;
						}
					}
					else if((Measure_X < Target_X))// x+ (R+) : Case 23
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_R += 1;
							else cal_R += 2;
						}
						else
						{
							cal_R += weight;
						}
					}
					else
					{
						//Nothing Should Happen
						cal_R -= 1000; 
						cal_B -= 1000;
						cal_vreg1 -= 500;
					}
				}
				else if(abs(Diff->X) < Limit_X && abs(Diff->Y) > Limit_Y && abs(Diff->Lv) < Limit_Lv)// Y�� ���� � 
				{
					if((Measure_Y > Target_Y)) // y- (B+) : Case 11
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B += 1;
							else cal_B += 2;
						}
						else
						{
							cal_B += weight;
						}
					}
					else if((Measure_Y < Target_Y)) // y+ (R-/B-) : Case 17
					{
						if(Vreg1_Infinite_Loop)
						{
							if(Case == 0)cal_B -= 1;
							else cal_R -= 1;
						}
						else
						{
							cal_R -= weight; cal_B -= weight;
						}
					}
					else
					{
						//Nothing Should Happen
						cal_R -= 1000; 
						cal_B -= 1000;
						cal_vreg1 -= 600;
					}
				}
				else if(abs(Diff->X) < Limit_X && abs(Diff->Y) < Limit_Y && abs(Diff->Lv) > Limit_Lv)// Lv�� ���� � 
				{
					if((abs(Diff->Lv) > (Limit_Lv * 2)) && (Lv_weight < 2))Lv_weight = 2; 
					else Lv_weight = 1; 

					if(Measure_Lv> Target_Lv) //Lv- (Vreg-) : Case 13
					{
						cal_vreg1 -= Lv_weight;	
					}
					else if(Measure_Lv < Target_Lv) //Lv+ (Vreg+) : Case 15
					{
						cal_vreg1 += Lv_weight;
					}
					else 
					{
						//Nothing Should Happen
						cal_R -= 1000; 
						cal_B -= 1000;
						cal_vreg1 -= 700;
					}
				}
				else // X,Y,Lv ��� Spec In (�̰Ŵ� ������ �̹� ó���� !)
				{
					//Nothing Should Happen
					cal_R -= 1000; 
					cal_B -= 1000;
					cal_vreg1 -= 1000; 
				}
			}

			Gamma_R += cal_R;
			Vreg1 += cal_vreg1;
			Gamma_B += cal_B;

			if(Gamma_R < 0)
				Gamma_R = 0;
			if(Gamma_R > Gamma_Register_Limit)
				Gamma_R = Gamma_Register_Limit;

			if(Vreg1 < 0)
				Vreg1 = 0;
			if(Vreg1 > Vreg1_Register_Limit)
				Vreg1 = Vreg1_Register_Limit;

			if(Gamma_B < 0)
				Gamma_B = 0;
			if(Gamma_B > Gamma_Register_Limit)
				Gamma_B = Gamma_Register_Limit;
		}
	}
	delete Diff;
	delete Limit;
	delete Target;
	delete Gamma;
	delete obj_weight;
}

extern "C" __declspec (dllexport ) void Sub_Compensation(int loop_count,bool Infinite_Loop,int Infinite_Count,int &Gamma_R,int &Gamma_G,int &Gamma_B,double Measure_X,double Measure_Y,double Measure_Lv,double Target_X,double Target_Y,double Target_Lv
,double Limit_X,double Limit_Y,double Limit_Lv,double Extension_X,double Extension_Y, int Gamma_Register_Limit,bool &Gamma_Out_Of_Register_Limit,bool &Within_Spec_Limit)
{
	if(Infinite_Count >= 3)
	{
		Limit_X += Extension_X;
		Limit_Y += Extension_Y;
	}
	
	//Initalize
	Double_XYLv* Diff = new Double_XYLv(Measure_X - Target_X,Measure_Y - Target_Y,Measure_Lv - Target_Lv);
	Double_XYLv* Measure = new Double_XYLv(Measure_X,Measure_Y,Measure_Lv);
	Double_XYLv* Limit = new Double_XYLv(Limit_X,Limit_Y,Limit_Lv);
	Double_XYLv* Target = new Double_XYLv(Target_X,Target_Y,Target_Lv);
	Int_RGB* Gamma = new Int_RGB(Gamma_R,Gamma_G,Gamma_B);
	OC_Weight* obj_weight = (OC_Weight *)new Sub_Compensation_Weight(Diff,Limit,Target,Gamma,Gamma_Register_Limit,Infinite_Count, loop_count);

	//Get Weight and Set Statet and Compensate 
	obj_weight->Set_XY_Weight_and_LV_Weight_For_Compensation();
	RGB_Compensate_State compensation_state(Gamma_Register_Limit, Infinite_Loop, obj_weight->Get_XY_Weight(), obj_weight->Get_Lv_Weight(), Target, Measure, Limit, Gamma);
	compensation_state.Full_Compensation(loop_count,Infinite_Count);

	//(Reference Variables) Get Gamma , Within_Spec_Limit , Gamma_Out_Of_Register_Limit 
	Gamma_R = compensation_state.Get_Calculated_Gamma_R();
	Gamma_G = compensation_state.Get_Calculated_Gamma_G();
	Gamma_B = compensation_state.Get_Calculated_Gamma_B();
	Within_Spec_Limit = compensation_state.Get_Within_Spec_Limit();//Ref value
	Gamma_Out_Of_Register_Limit = compensation_state.Get_Gamma_Out_Of_Register_Limit();//Ref value
	
	//Delete
	delete Diff;
	delete Limit;
	delete Target;
	delete Measure;
	delete Gamma;
	delete obj_weight;
}

extern "C" __declspec (dllexport ) void ELVSS_Compensation(bool& ELVSS_Find_Finish,double First_ELVSS,double& ELVSS,double& Vinit,double& First_Slope,double Vinit_Margin,double ELVSS_Margin
	,double Slope_Margin,double First_Measure_X,double First_Measure_Y,double First_Measure_Lv,double Measure_X,double Measure_Y,double Measure_Lv)
{
	ELVSS_Find_Finish = false;
	double Cal_Y = Slope_Margin;
	double Diff = Measure_Y - First_Measure_Y;

	if(Diff < Cal_Y)
	{
	    //Do nothing	
	}
	//Double value can't precisely become exact -3.5 (ex) 3.4999999997)
	//So need to be changed as an int to compare by using "="
	else 
	{
		ELVSS_Find_Finish = true;
	    Vinit = ELVSS - Vinit_Margin;
		ELVSS = ELVSS - ELVSS_Margin;	
	}
}

extern "C" __declspec (dllexport ) void ELVSS_Compensation_For_DP173(double ELVSS_Min,double ELVSS_Max,bool& ELVSS_Find_Finish,double First_ELVSS,double& ELVSS,double& First_Slope,double ELVSS_Margin
	,double Slope_Margin,double First_Measure_X,double First_Measure_Y,double First_Measure_Lv,double Measure_X,double Measure_Y,double Measure_Lv)
{
	ELVSS_Find_Finish = false;
	double Cal_Y = Slope_Margin;
	double Diff = Measure_Y - First_Measure_Y;

	if(Diff < Cal_Y)
	{
		ELVSS_Find_Finish = false;
	}
	else 
	{
		ELVSS_Find_Finish = true;
		ELVSS = ELVSS - ELVSS_Margin;

		if(ELVSS_Max < ELVSS)ELVSS = ELVSS_Max;
		else if(ELVSS < ELVSS_Min)ELVSS = ELVSS_Min;
	}
}



void Gamma_Plus_Or_Minus(bool Plus,int& cal_Gamma,int Gamma,int weight,int Gamma_Register_Limit,bool &Gamma_Out_Of_Register_Limit)
{
	if(Plus)
	{
		if(Gamma == Gamma_Register_Limit) Gamma_Out_Of_Register_Limit = true;
		else cal_Gamma += weight;
	}
	else //Minus
	{
		if(Gamma == 0) Gamma_Out_Of_Register_Limit = true;
		else cal_Gamma -= weight;
	}
}



//190528 Add
int Vreg1_Initial_Values(int Index_Count,int* Vreg1)
{
	sort(Vreg1,Vreg1+Index_Count);
	int MedianValue_Vreg1 = 0;

	if (Index_Count%2 == 0)
	{
		// count is even, need to get the middle two elements, add them together, then divide by 2  
        int middleElement1 = Vreg1[(Index_Count / 2) - 1]; 
        int middleElement2 = Vreg1[(Index_Count / 2)];
		MedianValue_Vreg1 = (middleElement1 + middleElement2) / 2;  
	}
	else
	{
		 // count is odd, simply get the middle element. 
		 MedianValue_Vreg1 = Vreg1[(Index_Count / 2)]; 
	}
	return MedianValue_Vreg1;
}


//190528 Add
void RGB_Gamma_Initial_Values(int Index_Count,int Min_Value,int Max_Value,int* Gamma_R, int* Gamma_G, int* Gamma_B,int& Out_R,int& Out_G,int& Out_B)
{
	sort(Gamma_R,Gamma_R+Index_Count);
	sort(Gamma_G,Gamma_G+Index_Count);
	sort(Gamma_B,Gamma_B+Index_Count);
	
	int MedianValue_R = 0;
	int MedianValue_G = 0;
    int MedianValue_B = 0;

	if (Index_Count%2 == 0)
	{
		// count is even, need to get the middle two elements, add them together, then divide by 2  
        int middleElement1 = Gamma_R[(Index_Count / 2) - 1]; 
        int middleElement2 = Gamma_R[(Index_Count / 2)]; 
        MedianValue_R = (middleElement1 + middleElement2) / 2;  

		middleElement1 = Gamma_G[(Index_Count / 2) - 1]; 
        middleElement2 = Gamma_G[(Index_Count / 2)]; 
		MedianValue_G = (middleElement1 + middleElement2) / 2;  

		middleElement1 = Gamma_B[(Index_Count / 2) - 1]; 
        middleElement2 = Gamma_B[(Index_Count / 2)]; 
		MedianValue_B = (middleElement1 + middleElement2) / 2;  
	}
	else
	{
		 // count is odd, simply get the middle element. 
		 MedianValue_R = Gamma_R[(Index_Count / 2)]; 
		 MedianValue_G = Gamma_G[(Index_Count / 2)]; 
		 MedianValue_B = Gamma_B[(Index_Count / 2)]; 
	}

	Out_R = MedianValue_R;
	Out_G = MedianValue_G;
	Out_B = MedianValue_B;

	if(Out_R > Max_Value) Out_R = Max_Value;
	if(Out_R < Min_Value) Out_R = Min_Value; 
	
	if(Out_G > Max_Value) Out_G = Max_Value;
	if(Out_G < Min_Value) Out_G = Min_Value; 
	
	if(Out_B > Max_Value) Out_B = Max_Value;
	if(Out_B < Min_Value) Out_B = Min_Value; 
}




extern "C" __declspec (dllexport ) double Get_DP173_EA9154_AM0_Resolution(const char* Hex_VREG1_REF1,const char* Hex_VREG1_REF2047)
{
    int Dec_VREG1_REF1 = (int)strtol(Hex_VREG1_REF1,NULL,16) & 0x7F;
    int Dec_VREG1_REF2047 = (int)strtol(Hex_VREG1_REF2047,NULL,16) & 0x7F;
       
    double Voltage_VREG1_REF1 = 0.2 + (0.04 * Dec_VREG1_REF1); //Min 0.2v , Max 5.28
    double Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v
    double AM0_Resolution = (Voltage_VREG1_REF2047 - Voltage_VREG1_REF1) / 700;

	return AM0_Resolution;
}


//191016
// Input : string FV1_LVL[5:0] , string VCI1_LVL[5:0]
// Output : AM0_Resolution
extern "C" __declspec (dllexport ) double Get_Meta_SW43417_AM0_Resolution(const char* FV1,const char* VCI1)
{
  double ELVDD = 4.6;
  
  int dec_FV1 = (int)strtol(FV1,NULL,16);
  if (dec_FV1 >= 42) dec_FV1 = 42;
  double K12 = ELVDD - (0.2 + (dec_FV1 * 0.1)); //ELVDD - FV1_LVL[5:0]

  int dec_VCI1 = (int)strtol(VCI1,NULL,16);
  if (dec_VCI1 >= 42) dec_VCI1 = 42;
  double F12 = ELVDD + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]

  return ((F12 - K12) / 700);
}

//190926
// Mipi_Wx = "0x73 0xXX [0.2XXX ~ 0.3XXX]";
// return = "0xXX"
extern "C" __declspec (dllexport ) void Meta_WRGB_Mipi_Transfer(double Measured_Wx,double Measured_Wy,double Measured_Rx,double Measured_Ry,double Measured_Gx,double Measured_Gy,double Measured_Bx,double Measured_By
	,double Target_Wx,double Target_Wy,double Target_Rx,double Target_Ry,double Target_Gx,double Target_Gy,double Target_Bx,double Target_By
	,char** Mipi_Wx,char** Mipi_Wy,char** Mipi_Rx,char** Mipi_Ry,char** Mipi_Gx,char** Mipi_Gy,char** Mipi_Bx,char** Mipi_By)
{
	//W (char* = copy(char* type,const char*))
	*Mipi_Wx = Meta_WRGB_Mipi_Transfer_Sub(Measured_Wx,Target_Wx,1);	 
	*Mipi_Wy = Meta_WRGB_Mipi_Transfer_Sub(Measured_Wy,Target_Wy,2); 
	
	//R
	*Mipi_Rx = Meta_WRGB_Mipi_Transfer_Sub(Measured_Rx,Target_Rx,3);
	*Mipi_Ry = Meta_WRGB_Mipi_Transfer_Sub(Measured_Ry,Target_Ry,4);
	//G
	*Mipi_Gx = Meta_WRGB_Mipi_Transfer_Sub(Measured_Gx,Target_Gx,5);
	*Mipi_Gy = Meta_WRGB_Mipi_Transfer_Sub(Measured_Gy,Target_Gy,6);
	//B
	*Mipi_Bx = Meta_WRGB_Mipi_Transfer_Sub(Measured_Bx,Target_Bx,7);
	*Mipi_By = Meta_WRGB_Mipi_Transfer_Sub(Measured_By,Target_By,8);
}

char* Meta_WRGB_Mipi_Transfer_Sub(double Measured,double Target,int Color)
{
	int First_Min = (int)((Target - 0.064) * 10000);
	int First_Max = First_Min + (int)(0.0004 * 10000);

	long long Min,Max;
	string Range_Min,Range_Max;
	int Dex_Register;

	int floored_Measured = (int)(Measured * 10000); //0.123456... --> 1234
		
    for(int i=0;i<255;i++)
	{
		Min = (First_Min +(i*5));
		Max = (Min + 4);
        if(Min <= floored_Measured && floored_Measured <= Max)
		{
			Dex_Register = i;
			Range_Min = to_string(Min);
			Range_Max = to_string(Max);
		}
	}
	Range_Min.insert(Range_Min.begin(),4 - Range_Min.size(),'0');
	Range_Max.insert(Range_Max.begin(),4 - Range_Max.size(),'0');

	// int(dec) to string(hex)
	stringstream ss; 
	ss << hex << Dex_Register; 
	string result;
	switch(Color)
	{
	    case 1:result = "0x73 ";break;
		case 2:result = "0x74 ";break;
        case 3:result = "0x76 ";break;
		case 4:result = "0x77 ";break;
	    case 5:result = "0x78 ";break;
	    case 6:result = "0x79 ";break;
        case 7:result = "0x7B ";break;
	    case 8:result = "0x7C ";break;
		default: break;
	}
	result.append("0x" + ss.str()); 
	result.append(" [0." + Range_Min + ",0." + Range_Max + "]");

	return strcpy((char*)malloc(result.length()+1), result.c_str());
}

