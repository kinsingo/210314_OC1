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
#include "Optic_Compensation.h"


//Sub_Compensation
RGB_Compensation::RGB_Compensation(int Gamma_Register_Limit,int Infinite_Loop,int XY_weight,int Lv_weight,Double_XYLv* Target,Double_XYLv* Measure, Double_XYLv* Limit,Int_RGB* Gamma)
{
	//std::cout<<"RGB_Compensation(~~~~~~~~~~~~~~~)"<<std::endl;
	this->Gamma_Register_Limit = Gamma_Register_Limit;
	this->Case = rand()%5; // 0,1,2,3,4 Random
	this->Infinite_Loop = Infinite_Loop;
	this->XY_weight = XY_weight;
	this->Lv_weight = Lv_weight;
	this->Diff = new Double_XYLv(Measure->X - Target->X,Measure->Y - Target->Y,Measure->Lv - Target->Lv);
	this->Target = new Double_XYLv(Target->X,Target->Y,Target->Lv);
	this->Measure = new Double_XYLv(Measure->X,Measure->Y,Measure->Lv);
	this->Limit = new Double_XYLv(Limit->X,Limit->Y,Limit->Lv);
	this->Gamma = new Int_RGB(Gamma->R,Gamma->G,Gamma->B);

	//Default
	this->cal_Gamma = new Int_RGB(0,0,0);
	this->Gamma_Out_Of_Register_Limit = false;
	this->Within_Spec_Limit = false;
}


RGB_Compensation::~RGB_Compensation()
{
	//std::cout<<"~RGB_Compensation()"<<std::endl;
	delete Measure;
	delete Target;
	delete Gamma;
	delete cal_Gamma; 
	delete Diff;
	delete Limit;
}

RGB_Compensation::RGB_Compensation()
{
	//std::cout<<"RGB_Compensation()"<<std::endl;
}

int RGB_Compensation::Get_Calculated_Gamma_R()
{
	Gamma->R += cal_Gamma->R;
	if(Gamma->R < 0)Gamma->R = 0;
	if(Gamma->R > Gamma_Register_Limit)Gamma->R = Gamma_Register_Limit;
	return Gamma->R;
}
int RGB_Compensation::Get_Calculated_Gamma_G()
{
	Gamma->G += cal_Gamma->G;
	if(Gamma->G < 0)Gamma->G = 0;
	if(Gamma->G > Gamma_Register_Limit)Gamma->G = Gamma_Register_Limit;
	return Gamma->G;
}
int RGB_Compensation::Get_Calculated_Gamma_B()
{
	Gamma->B += cal_Gamma->B;
	if(Gamma->B < 0)Gamma->B = 0;
    if(Gamma->B > Gamma_Register_Limit)Gamma->B = Gamma_Register_Limit;
	return Gamma->B;
}

bool RGB_Compensation::Get_Gamma_Out_Of_Register_Limit()
{
	return this->Gamma_Out_Of_Register_Limit;
}
bool RGB_Compensation::Get_Within_Spec_Limit()
{
	return this->Within_Spec_Limit;
}

void RGB_Compensation::Red_Gamma_Plus_Or_Minus(bool Plus)
{
	if(Plus)
	{
		if(Gamma->R == Gamma_Register_Limit) Gamma_Out_Of_Register_Limit = true;
		else cal_Gamma->R += XY_weight;
	}
	else //Minus
	{
		if(Gamma->R == 0) Gamma_Out_Of_Register_Limit = true;
		else cal_Gamma->R -= XY_weight;
	}
}

void RGB_Compensation::Green_Gamma_Plus_Or_Minus(bool Plus)
{
	if(Plus)
	{
		if(Gamma->G == Gamma_Register_Limit) Gamma_Out_Of_Register_Limit = true;
		else cal_Gamma->G += XY_weight;
	}
	else //Minus
	{
		if(Gamma->G == 0) Gamma_Out_Of_Register_Limit = true;
		else cal_Gamma->G -= XY_weight;
	}
}
void RGB_Compensation::Blue_Gamma_Plus_Or_Minus(bool Plus)
{
	if(Plus)
	{
		if(Gamma->B == Gamma_Register_Limit) Gamma_Out_Of_Register_Limit = true;
		else cal_Gamma->B += XY_weight;
	}
	else //Minus
	{
		if(Gamma->B == 0) Gamma_Out_Of_Register_Limit = true;
		else cal_Gamma->B -= XY_weight;
	}
}


bool RGB_Compensate_State::Is_Any_RGB_Reach_Boundary_Condition()
{
	return (Gamma->R == Gamma_Register_Limit || Gamma->R == 0
		|| Gamma->G == Gamma_Register_Limit || Gamma->G == 0
		|| Gamma->B == Gamma_Register_Limit || Gamma->B == 0);
}

bool RGB_Compensate_State::Is_Calculated_RGB_All_Zero()
{
	return (cal_Gamma->R == 0 && cal_Gamma->G  == 0 && cal_Gamma->B  == 0);
}


//Fast_Sub_Compensation
void RGB_Compensate_State::Fast_RGB_Compensation() 
{
	//std::cout<<"Fast_RGB_Compensation() Algorithm"<<std::endl;

	    Within_Spec_Limit = false;
		Gamma_Out_Of_Register_Limit = false;
		//====== X Spec Out ======
		// Target.X - Measure.X > Limit.X (X need to be increased)
		if( - Diff->X > Limit->X)
		{
			if(Infinite_Loop == false)
			{
				if(Gamma->R == Gamma_Register_Limit)
				{
					Blue_Gamma_Plus_Or_Minus(false);//B--
				}
				else
				{
					Red_Gamma_Plus_Or_Minus(true);//R++
					Blue_Gamma_Plus_Or_Minus(false);//B--
				}
			}
			else
			{
				Red_Gamma_Plus_Or_Minus(true);//R++
			}
		}
		// Target.X - Measure.X < -Limit_X (X need to be Decreased)
		else if(Diff->X > Limit->X) 
		{
			if(Infinite_Loop == false)
			{
				if(Gamma->R == 0)
				{
					Blue_Gamma_Plus_Or_Minus(true);//B++
				}
				else
				{
					Red_Gamma_Plus_Or_Minus(false);//R--
					if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
				}
			}
			else
			{
				Red_Gamma_Plus_Or_Minus(false);//R--
			}
		}
		// -Limit.X < Target.X - Measure.X < Limit.X
		else
		{
			// Gamma R,G,B remain same
		}

		//====== Y Spec Out ======
		// Measured.Y - Target.Y > Limit_Y (Y need to be decreased)
		if(Diff->Y > Limit->Y)
		{
			if(Infinite_Loop == false)
			{
				if(Gamma->G == 0)
				{
					Blue_Gamma_Plus_Or_Minus(true);//B++
				}
				else
				{
					Green_Gamma_Plus_Or_Minus(false);//G--
					if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
				}
			}
			else
			{
				Green_Gamma_Plus_Or_Minus(false);//G--
			}
		}
		// Target.Y - Measured.Y > Limit_Y (Y need to be Increased)
		else if(- Diff->Y > Limit->Y)
		{
			if(Infinite_Loop == false)
			{
				if(Gamma->G == Gamma_Register_Limit)
				{
					Blue_Gamma_Plus_Or_Minus(false);//B--
				}
				else
				{
					Green_Gamma_Plus_Or_Minus(true);//G++
					Blue_Gamma_Plus_Or_Minus(false);//B--
				}
			}
			else
			{
				Green_Gamma_Plus_Or_Minus(true);//G++
			}
		}
		// -Limit_Y < Measured.Y - Target.Y < Limit_Y 
		else
		{
			// Gamma R,G,B remain same
		}

		// if(Lv Spec Out)
		if(abs(Diff->Lv) > Limit->Lv)
		{
			//Measured.Lv > Target.Lv (Lv need to be Decreased)
		    if(Diff->Lv > 0)
			{
				if(Infinite_Loop == false)
			    {
					Red_Gamma_Plus_Or_Minus(false);//R--
					Green_Gamma_Plus_Or_Minus(false);//G--
					Blue_Gamma_Plus_Or_Minus(false);//B--
				}
				else
				{
					switch(Case)
					{
					  case 0:
						  Red_Gamma_Plus_Or_Minus(false);//R--
						  Green_Gamma_Plus_Or_Minus(false);//G--
						  Blue_Gamma_Plus_Or_Minus(false);//B--
						  break;
					  case 1:
						  Red_Gamma_Plus_Or_Minus(false);//R--
						  Blue_Gamma_Plus_Or_Minus(false);//B--
						  break;
					  case 2:
						  Green_Gamma_Plus_Or_Minus(false);//G--
						  break;
					  case 3:
						  Red_Gamma_Plus_Or_Minus(false);//R--
						  Green_Gamma_Plus_Or_Minus(false);//G--
						  break;
					  default:
						  Green_Gamma_Plus_Or_Minus(false);//G--
						  Blue_Gamma_Plus_Or_Minus(false);//B--
						  break;
					}
				}

			}
			//Measured.Lv < Target.Lv (Lv need to be Increased)
			else
			{
				if(Infinite_Loop == false)
			    {
					Red_Gamma_Plus_Or_Minus(true);//R++
					Green_Gamma_Plus_Or_Minus(true);//G++
					if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
				}
				else
				{
					switch(Case)
					{
					  case 0:
				          Red_Gamma_Plus_Or_Minus(true);//R++
					      Green_Gamma_Plus_Or_Minus(true);//G++
						  if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
						  break;
					  case 1:
				          Red_Gamma_Plus_Or_Minus(true);//R++
						  if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
						  break;
					  case 2:
				          Green_Gamma_Plus_Or_Minus(true);//G++
					      break;
					  case 3:
						  Red_Gamma_Plus_Or_Minus(true);//R++
					      Green_Gamma_Plus_Or_Minus(true);//G++
					      break;
					  default:
				          Green_Gamma_Plus_Or_Minus(true);//G++
						  if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
						  break;
					}
				}
			}
		}
		// abs(Diff_Lv) < Limit_Lv (Spec In)
		else
		{
			// Gamma R,G,B remain same
		}
}

//Precise_Sub_Compensation
void RGB_Compensate_State::Precise_RGB_Compensation() 
{
	//std::cout<<"Precise_RGB_Compensation() Algorithm"<<std::endl;

    bool ok_x = false;
	bool ok_y = false;
	bool ok_xy = false;
	bool ok_Lv = false;

	if(abs(Diff->Lv) < Limit->Lv) ok_Lv = true;
	if(abs(Diff->X) < Limit->X) ok_x = true;
	if(abs(Diff->Y) < Limit->Y) ok_y = true;
	if(ok_x == true && ok_y == true) ok_xy = true;

	if(ok_xy == true && ok_Lv == false)    //����ǥ spec in, �ֵ� spec out
	{
		//Measured.Lv > Target.Lv (Lv need to be Decreased)
		if(Diff->Lv > 0)
		{
			if(Infinite_Loop == false)
			{
				Red_Gamma_Plus_Or_Minus(false);//R--
				Green_Gamma_Plus_Or_Minus(false);//G--
				Blue_Gamma_Plus_Or_Minus(false);//B--
			}
			else
			{
				switch(Case)
				{
					case 0:
			        	Red_Gamma_Plus_Or_Minus(false);//R--
			        	Green_Gamma_Plus_Or_Minus(false);//G--
			        	Blue_Gamma_Plus_Or_Minus(false);//B--
						break;
					case 1:
			        	Red_Gamma_Plus_Or_Minus(false);//R--
			        	Blue_Gamma_Plus_Or_Minus(false);//B--
						break;
					case 2:
			        	Green_Gamma_Plus_Or_Minus(false);//G--
						break;
					case 3:
			        	Red_Gamma_Plus_Or_Minus(false);//R--
			        	Green_Gamma_Plus_Or_Minus(false);//G--
						break;
					default:
			        	Green_Gamma_Plus_Or_Minus(false);//G--
			        	Blue_Gamma_Plus_Or_Minus(false);//B--
						break;
				}
			}
		}
		//Measured.Lv < Target.Lv (Lv need to be Increased)
		else
		{
			if(Infinite_Loop == false)
			{
				Red_Gamma_Plus_Or_Minus(true);//R++
				Green_Gamma_Plus_Or_Minus(true);//G++
				if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
			}
			else
			{
				switch(Case)
				{
					case 0:
				        Red_Gamma_Plus_Or_Minus(true);//R++
				        Green_Gamma_Plus_Or_Minus(true);//G++
						if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
						break;
					case 1:
				        Red_Gamma_Plus_Or_Minus(true);//R++
						if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
						break;
					case 2:
				        Green_Gamma_Plus_Or_Minus(true);//G++
						break;
					case 3:
				        Red_Gamma_Plus_Or_Minus(true);//R++
				        Green_Gamma_Plus_Or_Minus(true);//G++
				        break;
					default:
				        Green_Gamma_Plus_Or_Minus(true);//G++
						if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
						break;
				}
			}
		}
	}
	//��� 1 : ����ǥ spec out, �ֵ� : spec in
	//��� 2 : ����ǥ spec out, �ֵ� : spec out, ���1 & ���2 ��� �ϴ� ����ǥ compensation
	else		
	{			
			if(ok_x == false && ok_y == false)
			{
				if(Diff->X < 0 && Diff->Y < 0) // Mx < Tx , My < Ty
				{
					if(Diff->Lv < 0)	//x, y, Lv ��� ���� case (M_lv < T_lv) 
					{
						if(Infinite_Loop == false)
			            {
							if(Gamma->R == Gamma_Register_Limit)	
							{
								Blue_Gamma_Plus_Or_Minus(false);//B--
							}
							else							    
							{
								Red_Gamma_Plus_Or_Minus(true);//R++
							}

							if(Gamma->G == Gamma_Register_Limit)	
							{
								Blue_Gamma_Plus_Or_Minus(false);//B--
							}
							else							   
							{
								Green_Gamma_Plus_Or_Minus(true);//G++
							}
						}
						else
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}

					}
					else			//x, y ����, Lv ���� case
					{
						if(Gamma->B <= 0)		
						{
							Red_Gamma_Plus_Or_Minus(true);//R++
							Green_Gamma_Plus_Or_Minus(true);//G++
						}
						else							
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}
					}
				}
				else if(Diff->X > 0 && Diff->Y > 0) //Mx > Tx , My > Ty
				{
					if(Diff->Lv < 0)	//x, y ����, Lv ���� case (M_lv < T_lv) 
					{
						if (Gamma->B < (Gamma_Register_Limit * 0.95))
						{
							Blue_Gamma_Plus_Or_Minus(true);//B++
						}
						else
						{
							Red_Gamma_Plus_Or_Minus(false);//R--
							Green_Gamma_Plus_Or_Minus(false);//G--
						}
					}
					else	//x, y, Lv ��� ���� case
					{
		                if(Infinite_Loop == false)
			            {
							if(Gamma->R == 0)		
							{
								Blue_Gamma_Plus_Or_Minus(true);//B++
							}
							else					
							{
								Red_Gamma_Plus_Or_Minus(false);//R--
							}

							if(Gamma->G == 0)		
							{
								Blue_Gamma_Plus_Or_Minus(true);//B++
							}
							else					
							{
								Green_Gamma_Plus_Or_Minus(false);//G--
							}
						}
						else
						{
							Blue_Gamma_Plus_Or_Minus(true);//B++
						}
					}
				}
				else	//x ���� && y ����,(�Ǵ�) x ���� && y ���� case
				{
					if(Diff->X < 0)	//x ���� (Mx < Tx)
					{
						if(Infinite_Loop == false)
			            {
							if(Gamma->R == Gamma_Register_Limit) 
							{
								Blue_Gamma_Plus_Or_Minus(false);//B--
							}
							else								
							{
								Red_Gamma_Plus_Or_Minus(true);//R++
							}
						}
						else
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}
					}
					else			//x ����  (Mx >= Tx)
					{
				        if(Infinite_Loop == false)
			            {
							if(Gamma->R == 0)			
							{
								Blue_Gamma_Plus_Or_Minus(true);//B++
							}
							else						
							{
								Red_Gamma_Plus_Or_Minus(false);//R--
							}
					    }
						else
						{
								Blue_Gamma_Plus_Or_Minus(true);//B++
						}
					}
					if(Diff->Y < 0)	//y ���� (My < Ty)
					{
				        if(Infinite_Loop == false)
			            {						
							if(Gamma->G == Gamma_Register_Limit)	
							{
								Blue_Gamma_Plus_Or_Minus(false);//B--
							}
							else							
							{
								Green_Gamma_Plus_Or_Minus(true);//G++
							}
						}
						else
						{
								Blue_Gamma_Plus_Or_Minus(false);//B--
						}
					}
					else			//y ����(My >= Ty)
					{
						if(Infinite_Loop == false)
			            {
							if(Gamma->G == 0)			
							{
								Blue_Gamma_Plus_Or_Minus(true);//B++
							}
							else						
							{
								Green_Gamma_Plus_Or_Minus(false);//G--
							}
						}
						else
						{
								Blue_Gamma_Plus_Or_Minus(true);//B++
						}
					}
				}
			}
			else if(ok_x == false && ok_y == true)
			{
				if(Diff->X < 0)	//x ���� (Mx < Tx)
				{
					if(Infinite_Loop == false)
			        {
						if(Gamma->R == Gamma_Register_Limit)	
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}
						else								
						{
							Red_Gamma_Plus_Or_Minus(true);//R++
						}
					}
					else
					{
							Blue_Gamma_Plus_Or_Minus(false);//B--
					}
				}
				else			//x ���� (Mx >= Tx)
				{
					if(Infinite_Loop == false)
			        {
						if(Gamma->R == 0)			
						{
							Blue_Gamma_Plus_Or_Minus(true);//B++
						}
						else						
						{
							Red_Gamma_Plus_Or_Minus(false);//R--
						}
					}
					else
					{
							Blue_Gamma_Plus_Or_Minus(true);//B++
					}
				}
			}
			else
			{
				if(Diff->Y < 0)	//y ���� (My < Ty)
				{
					if(Infinite_Loop == false)
			        {
						if(Gamma->G == Gamma_Register_Limit)	
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}
						else     							
						{
							Green_Gamma_Plus_Or_Minus(true);//G++
						}
					}
					else
					{
							Blue_Gamma_Plus_Or_Minus(false);//B--
					}
				}

				else			//y ���� (My >= Ty)
				{
					if(Infinite_Loop == false)
			        {
						if(Gamma->G == 0)			
						{
							Blue_Gamma_Plus_Or_Minus(true);//B++
						}
						else						
						{
							Green_Gamma_Plus_Or_Minus(false);//G--
						}
					}
					else
					{
							Blue_Gamma_Plus_Or_Minus(true);//B++
					}
				}
			}
		}
}

void RGB_Compensate_State::Precise_RGB_Compensation_2() 
{
	//std::cout<<"Precise_RGB_Compensation_2() Algorithm"<<std::endl;

    bool ok_x = false;
	bool ok_y = false;
	bool ok_xy = false;
	bool ok_Lv = false;

	//Diff = Measured - Target
	if(abs(Diff->Lv) < Limit->Lv) ok_Lv = true;
	if(abs(Diff->X) < Limit->X) ok_x = true;
	if(abs(Diff->Y) < Limit->Y) ok_y = true;
	if(ok_x == true && ok_y == true) ok_xy = true;

	if(ok_xy == true && ok_Lv == false)    //����ǥ spec in, �ֵ� spec out
	{
		//Measured.Lv > Target.Lv (Up)
		if(Diff->Lv > 0)
		{
				//Measured_X < Target_X (Left)
				if(Diff->X < 0)
				{
					//X : Out of 1/2 Limit
					if(abs(Diff->X) > (Limit->X/2.0))
					{
						//Measured_Y < Target_Y (Down) 
					    if(Diff->Y < 0)
						{
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Left,Out of 1/2)/Y(Down,Out of 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Blue_Gamma_Plus_Or_Minus(false);//B--
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R--
									Green_Gamma_Plus_Or_Minus(false);//G--
									Blue_Gamma_Plus_Or_Minus(false);//B--
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Left,Out of 1/2)/Y(Down,Within 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Blue_Gamma_Plus_Or_Minus(false);//B--
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
							}
						}
						//Measured_Y > Target_Y (Up)
					    else
					    {
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Left,Out of 1/2)/Y(Up,Out of 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
								else 
								{
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Left,Out of 1/2)/Y(Up,Within 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
								else 
								{
									Green_Gamma_Plus_Or_Minus(false); //G --
								}
							}
						}
					}
					//X : Within 1/2 Limit
					else
					{
						//Measured_Y < Target_Y (Down)
					    if(Diff->Y < 0)
						{
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Left,Within 1/2)/Y(Down,Out of 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R--
									Green_Gamma_Plus_Or_Minus(false);//G --
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Left,Within 1/2)/Y(Down,Within 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
								else 
								{
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
							}
						}
						//Measured_Y > Target_Y (Up)
					    else
					    {
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Left,Within 1/2)/Y(Up,Out of 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Left,Within 1/2)/Y(Up,Within 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
							}
						}
					}
					
				}
				//Measured_X >= Target_X (Right)
				else
				{
					//X : Out of 1/2 Limit
					if(abs(Diff->X) > (Limit->X/2.0))
					{
						//Measured_Y < Target_Y (Down)
					    if(Diff->Y < 0)
						{
							if (Infinite_Loop == false)
							{
								Red_Gamma_Plus_Or_Minus(false);//R --
							}
							else
							{
								Red_Gamma_Plus_Or_Minus(false);//R --
								Green_Gamma_Plus_Or_Minus(false);//G --
								Blue_Gamma_Plus_Or_Minus(false);//B --
							}
						}

						//Measured_Y > Target_Y (Up)
					    else
					    {
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Right,Out of 1/2)/Y(Up,Out of 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Right,Out of 1/2)/Y(Up,Within 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
							}
						}
					}
					//X : Within 1/2 Limit
					else
					{
						//Measured_Y < Target_Y (Down)
					    if(Diff->Y < 0)
						{
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Right,Within 1/2)/Y(Down,Out of 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Right,Within 1/2)/Y(Down,Within 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
									Blue_Gamma_Plus_Or_Minus(false);//B --

								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
								}
							}
						}
						//Measured_Y > Target_Y (Up)
					    else
					    {
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Right,Within 1/2)/Y(Up,Out of 1/2)/Lv(Up)
								if(Infinite_Loop == false)
								{
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(false);//G --
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								Red_Gamma_Plus_Or_Minus(false);//R --
								Green_Gamma_Plus_Or_Minus(false);//G --
								Blue_Gamma_Plus_Or_Minus(false);//B --
							}
						}
					}
				}
		}
		//Measured.Lv < Target.Lv (Lv need to be Increased)
		else
		{
				//Measured_X < Target_X (Left)
				if(Diff->X < 0)
				{
					//X : Out of 1/2 Limit
					if(abs(Diff->X) > (Limit->X/2.0))
					{
						//Measured_Y < Target_Y (Down) 
					    if(Diff->Y < 0)
						{
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Left,Out of 1/2)/Y(Down,Out of 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									Blue_Gamma_Plus_Or_Minus(false);//B --
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Left,Out of 1/2)/Y(Down,Within 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
								}
							}
						}
						//Measured_Y > Target_Y (Up)
					    else
					    {
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Left,Out of 1/2)/Y(Up,Out of 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Left,Out of 1/2)/Y(Up,Within 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
							}
						}
					}
					//X : Within 1/2 Limit
					else
					{
						//Measured_Y < Target_Y (Down)
					    if(Diff->Y < 0)
						{
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Left,Within 1/2)/Y(Down,Out of 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Green_Gamma_Plus_Or_Minus(true);//G ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Left,Within 1/2)/Y(Down,Within 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
								}
							}
						}
						//Measured_Y > Target_Y (Up)
					    else
					    {
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Left,Within 1/2)/Y(Up,Out of 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
								}
							}
							//Y : Within 1/2 Limit
							else
							{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								
							}
						}
					}
				}
				//Measured_X >= Target_X (Right)
				else
				{
					//X : Out of 1/2 Limit
					if(abs(Diff->X) > (Limit->X/2.0))
					{
						//Measured_Y < Target_Y (Down)
					    if(Diff->Y < 0)
						{
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Right,Out of 1/2)/Y(Down,Out of 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Green_Gamma_Plus_Or_Minus(true);//G ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Right,Out of 1/2)/Y(Down,Within 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(false);////R --
									Green_Gamma_Plus_Or_Minus(true);//G ++
									
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R --
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
							}
						}
						//Measured_Y > Target_Y (Up)
					    else
					    {
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Right,Out of 1/2)/Y(Up,Out of 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Right,Out of 1/2)/Y(Up,Within 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Blue_Gamma_Plus_Or_Minus(true);//B++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
							}
						}
					}
					//X : Within 1/2 Limit
					else
					{
						//Measured_Y < Target_Y (Down)
					    if(Diff->Y < 0)
						{
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Right,Within 1/2)/Y(Down,Out of 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Green_Gamma_Plus_Or_Minus(true);//G++
									
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R++
									Green_Gamma_Plus_Or_Minus(true);//G++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B++
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Right,Within 1/2)/Y(Down,Within 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(false);//R--
									Green_Gamma_Plus_Or_Minus(true);//G++
									
								}
							}
						}
						//Measured_Y > Target_Y (Up)
					    else
					    {
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Right,Within 1/2)/Y(Up,Out of 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
								else 
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
							}
							//Y : Within 1/2 Limit
							else
							{
								//X(Right,Within 1/2)/Y(Up,Within 1/2)/Lv(Down)
								if(Infinite_Loop == false)
								{
									Red_Gamma_Plus_Or_Minus(true);//R ++
									Green_Gamma_Plus_Or_Minus(true);//G ++
									if (Gamma->B < (Gamma_Register_Limit * 0.95)) Blue_Gamma_Plus_Or_Minus(true);//B ++
								}
								else 
								{
							        Blue_Gamma_Plus_Or_Minus(true);//B++
							        
								}
							}
						}
					}
				}
		}
	}
	//��� 1 : ����ǥ spec out, �ֵ� : spec in
	//��� 2 : ����ǥ spec out, �ֵ� : spec out, ���1 & ���2 ��� �ϴ� ����ǥ compensation
	else		
	{			
			if(ok_x == false && ok_y == false)
			{
				if(Diff->X < 0 && Diff->Y < 0) // Mx < Tx , My < Ty
				{
					if(Diff->Lv < 0)	//x, y, Lv ��� ���� case (M_lv < T_lv) 
					{
						if(Infinite_Loop == false)
			            {
							if(Gamma->R == Gamma_Register_Limit)	
							{
								Blue_Gamma_Plus_Or_Minus(false);//B--
							}
							else							    
							{
								Red_Gamma_Plus_Or_Minus(true);//R++
							}

							if(Gamma->G == Gamma_Register_Limit)	
							{
								Blue_Gamma_Plus_Or_Minus(false);//B--
							}
							else							   
							{
								Green_Gamma_Plus_Or_Minus(true);//G++
							}
						}
						else
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}

					}
					else
					{
						if(Gamma->B <= 0)		
						{
							Red_Gamma_Plus_Or_Minus(true);//R++
							Green_Gamma_Plus_Or_Minus(true);//G++
						}
						else							
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}
					}
				}
				else if(Diff->X > 0 && Diff->Y > 0) //Mx > Tx , My > Ty
				{
					if(Diff->Lv < 0)
					{
						if(Gamma->B == Gamma_Register_Limit)	
						{
							Red_Gamma_Plus_Or_Minus(false);//R--
							Green_Gamma_Plus_Or_Minus(false);//G--
						}
						else							
						{
							Blue_Gamma_Plus_Or_Minus(true);//B++
						}
					}
					else	//x, y, Lv ��� ���� case
					{
		                if(Infinite_Loop == false)
			            {
							if(Gamma->R == 0)		
							{
								Blue_Gamma_Plus_Or_Minus(true);//B++
							}
							else					
							{
								Red_Gamma_Plus_Or_Minus(false);//R--
							}

							if(Gamma->G == 0)		
							{
								Blue_Gamma_Plus_Or_Minus(true);//B++
							}
							else					
							{
								Green_Gamma_Plus_Or_Minus(false);//G--
							}
						}
						else
						{
							Blue_Gamma_Plus_Or_Minus(true);//B++
						}
					}
				}
				else	//x ���� && y ����,(�Ǵ�) x ���� && y ���� case
				{
					if(Diff->X < 0)	//x ���� (Mx < Tx)
					{
						if(Infinite_Loop == false)
			            {
							if(Gamma->R == Gamma_Register_Limit) 
							{
								Blue_Gamma_Plus_Or_Minus(false);//B--
							}
							else								
							{
								Red_Gamma_Plus_Or_Minus(true);//R++
							}
						}
						else
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}
					}
					else			//x ����  (Mx >= Tx)
					{
				        if(Infinite_Loop == false)
			            {
							if(Gamma->R == 0)			
							{
								Blue_Gamma_Plus_Or_Minus(true);//B++
							}
							else						
							{
								Red_Gamma_Plus_Or_Minus(false);//R--
							}
					    }
						else
						{
								Blue_Gamma_Plus_Or_Minus(true);//B++
						}
					}
					if(Diff->Y < 0)	//y ���� (My < Ty)
					{
				        if(Infinite_Loop == false)
			            {						
							if(Gamma->G == Gamma_Register_Limit)	
							{
								Blue_Gamma_Plus_Or_Minus(false);//B--
							}
							else							
							{
								Green_Gamma_Plus_Or_Minus(true);//G++
							}
						}
						else
						{
								Blue_Gamma_Plus_Or_Minus(false);//B--
						}
					}
					else			//y ����(My >= Ty)
					{
						if(Infinite_Loop == false)
			            {
							if(Gamma->G == 0)			
							{
								Blue_Gamma_Plus_Or_Minus(true);//B++
							}
							else						
							{
								Green_Gamma_Plus_Or_Minus(false);//G--
							}
						}
						else
						{
								Blue_Gamma_Plus_Or_Minus(true);//B++
						}
					}
				}
			}
			else if(ok_x == false && ok_y == true)
			{
				if(Diff->X < 0)	//x ���� (Mx < Tx)
				{
					if(Infinite_Loop == false)
			        {
						if(Gamma->R == Gamma_Register_Limit)	
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}
						else								
						{
							Red_Gamma_Plus_Or_Minus(true);//R++
						}
					}
					else
					{
							Blue_Gamma_Plus_Or_Minus(false);//B--
					}
				}
				else			//x ���� (Mx >= Tx)
				{
					if(Infinite_Loop == false)
			        {
						if(Gamma->R == 0)			
						{
							Blue_Gamma_Plus_Or_Minus(true);//B++
						}
						else						
						{
							Red_Gamma_Plus_Or_Minus(false);//R--
						}
					}
					else
					{
							Blue_Gamma_Plus_Or_Minus(true);//B++
					}
				}
			}
			else
			{
				if(Diff->Y < 0)	//y ���� (My < Ty)
				{
					if(Infinite_Loop == false)
			        {
						if(Gamma->G == Gamma_Register_Limit)	
						{
							Blue_Gamma_Plus_Or_Minus(false);//B--
						}
						else     							
						{
							Green_Gamma_Plus_Or_Minus(true);//G++
						}
					}
					else
					{
							Blue_Gamma_Plus_Or_Minus(false);//B--
					}
				}

				else			//y ���� (My >= Ty)
				{
					if(Infinite_Loop == false)
			        {
						if(Gamma->G == 0)			
						{
							Blue_Gamma_Plus_Or_Minus(true);//B++
						}
						else						
						{
							Green_Gamma_Plus_Or_Minus(false);//G--
						}
					}
					else
					{
							Blue_Gamma_Plus_Or_Minus(true);//B++
					}
				}
			}
		}
}
bool RGB_Compensate_State::Get_Is_Lv_Priority(int Lv_Priority_Min,int Lv_Priority_Max,Double_XYLv LVPriority_Limit)
{
	bool Lv_Priority;

	if((Gamma->R >= Lv_Priority_Max) || (Gamma->G >= Lv_Priority_Max) || (Gamma->B >= Lv_Priority_Max))Lv_Priority = false; //True Condition 1
	else if((Gamma->R <= Lv_Priority_Min) || (Gamma->G <= Lv_Priority_Min) || (Gamma->B <= Lv_Priority_Min))Lv_Priority = false;
	else if((abs(Diff->X) < LVPriority_Limit.X)&&(abs(Diff->Y) < LVPriority_Limit.Y)&&(abs(Diff->Lv) > LVPriority_Limit.Lv))Lv_Priority = true;//True Condition 1
	else if(Measure->Lv < (Target->Lv * 0.5))Lv_Priority = true;//True Condition 2
	else if(Measure->Lv > (Target->Lv * 2))Lv_Priority = true;//True Condition 3
	else Lv_Priority = false;

	return Lv_Priority;
}

void RGB_Compensate_State::Lv_Priority_Compensation(int Lv_Priority_Min,int Lv_Priority_Max)
{
	if(Measure->Lv < Target->Lv)
	{
		Gamma->R += Lv_weight; if(Gamma->R >= Lv_Priority_Max)Gamma->R = Lv_Priority_Max;
		Gamma->G += Lv_weight; if(Gamma->G >= Lv_Priority_Max)Gamma->G = Lv_Priority_Max;
		Gamma->B += Lv_weight; if(Gamma->B >= Lv_Priority_Max)Gamma->B = Lv_Priority_Max;
	}
	else
	{
		Gamma->R -= Lv_weight; if(Gamma->R <= Lv_Priority_Min)Gamma->R = Lv_Priority_Min;
		Gamma->G -= Lv_weight; if(Gamma->G <= Lv_Priority_Min)Gamma->G = Lv_Priority_Min;
		Gamma->B -= Lv_weight; if(Gamma->B <= Lv_Priority_Min)Gamma->B = Lv_Priority_Min;
	}
}


void RGB_Compensate_State::Full_Compensation(int loop_count,int& Infinite_Count)
{
	if(abs(Diff->X) < Limit->X && abs(Diff->Y) < Limit->Y && abs(Diff->Lv) < Limit->Lv)
	{
		Within_Spec_Limit = true;
	}
	else 
	{
		Double_XYLv LVPriority_Limit(0.02,0.02,(Limit->Lv * 5));//After on 200622
		int Lv_Priority_Max = (int)(Gamma_Register_Limit * 0.95);
		int Lv_Priority_Min = (int)(Gamma_Register_Limit * 0.05);
		bool Lv_Priority = Get_Is_Lv_Priority(Lv_Priority_Min,Lv_Priority_Max,LVPriority_Limit);

		if(Lv_Priority)//After 200622
		{
			Lv_Priority_Compensation(Lv_Priority_Min,Lv_Priority_Max);
			if(Infinite_Count >= 3)Infinite_Count = 0;//Added on 200622
		}
		else
		{
			if(Infinite_Count == 0 && loop_count < 15)//After 200622
			{
				Fast_RGB_Compensation();
			}
			else
			{
				if(Target->Lv > 0.1) Precise_RGB_Compensation_2();
				else Precise_RGB_Compensation();
			}
		}

		if(Is_Calculated_RGB_All_Zero() && Is_Any_RGB_Reach_Boundary_Condition()) 
			Gamma_Out_Of_Register_Limit = true; //Added on 200519
	}
}

void RGB_Compensate_State::Init_Algorithm_Applied_Full_Compensation(int loop_count, int& Infinite_Count)
{
	if (abs(Diff->X) < Limit->X && abs(Diff->Y) < Limit->Y && abs(Diff->Lv) < Limit->Lv)
	{
		Within_Spec_Limit = true;
	}
	else
	{
		if (Target->Lv > 0.1) Precise_RGB_Compensation_2();
		else Precise_RGB_Compensation();
		
		if (Is_Calculated_RGB_All_Zero() && Is_Any_RGB_Reach_Boundary_Condition()) Gamma_Out_Of_Register_Limit = true; //Added on 200519
	}
}





RGB_Compensate_State::RGB_Compensate_State(int Gamma_Register_Limit,int Infinite_Loop,int XY_weight,int Lv_weight,Double_XYLv* Target,Double_XYLv* Measure, Double_XYLv* Limit,Int_RGB* Gamma)
	:RGB_Compensation(Gamma_Register_Limit,Infinite_Loop,XY_weight,Lv_weight,Target,Measure,Limit,Gamma)
{
	//std::cout<<"RGB_Compensate_State(~~~~)"<<std::endl;
}

RGB_Compensate_State::~RGB_Compensate_State()
{
	//std::cout<<"~RGB_Compensate_State()"<<std::endl;	
}


int Vreg1_Compensate_State::Get_Calculated_Vreg1()
{
	Vreg1 += cal_vreg1;
	if(Vreg1 < 0) Vreg1 = 0;
	if(Vreg1 > Vreg1_Register_Limit)Vreg1 = Vreg1_Register_Limit;
	return Vreg1;
}


//Vreg1_Compensation_Related
void Vreg1_Compensate_State::Vreg1_Compensation()
{
	//If(Diff < Limit) , Spec In
	if(abs(Diff->X) < Limit->X && abs(Diff->Y) < Limit->Y && abs(Diff->Lv) < Limit->Lv)
	{
		Within_Spec_Limit = true;
	}
	// Spec Out
	//double Diff_X = Measure_X - Target_X;
	else 
	{
		if(Gamma->R == 0 || Vreg1 == 0 || Gamma->B == 0
		|| Gamma->R == Gamma_Register_Limit || Vreg1 == Vreg1_Register_Limit || Gamma->B == Gamma_Register_Limit)
		{
			Gamma_Out_Of_Register_Limit = true;
		}
		else
		{
			if((abs(Diff->Lv) > (Limit->Lv * 3))||((abs(Diff->Lv) > Limit->Lv)&&(Infinite_Loop)))//0.05%
			{
				if(Measure->Lv> Target->Lv) //������(Lv)�� Target ��� ū ���
				{
					if(Infinite_Loop)cal_vreg1 -= 1;
					else cal_vreg1 -= Lv_weight;
				}
				else if(Measure->Lv < Target->Lv) //������(Lv)�� Target ��� ���� ���
				{
					if(Infinite_Loop)cal_vreg1 += 1;
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
				if(abs(Diff->X) > Limit->X && abs(Diff->Y) > Limit->Y && abs(Diff->Lv) > Limit->Lv)
				{
					if((Measure->X > Target->X) &&  (Measure->Y > Target->Y) && (Measure->Lv > Target->Lv))// x--/y-/Lv- (R-/B+) : Case 1
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B += 1;
							else cal_Gamma->R -= 1;
						}
						else 
						{
							cal_Gamma->R -= XY_weight; cal_Gamma->B += XY_weight;
						}
					}
					else if((Measure->X > Target->X) &&  (Measure->Y > Target->Y)&& (Measure->Lv < Target->Lv))//x-/y-/Lv+ (B+) : Case 3
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B += 1;
							else cal_Gamma->B += 2;
						}
						else 
					    {
							cal_Gamma->B += XY_weight;
						}
					}
					else if((Measure->X < Target->X) && (Measure->Y > Target->Y)) // x+/y- (R+/B+) : Case 19,21 
					{
						if(Infinite_Loop)
						{
						   if(Case == 0)cal_Gamma->B += 1;
						   else cal_Gamma->R += 1;
						}
						else 
						{
						   cal_Gamma->R += XY_weight; cal_Gamma->B += XY_weight;
						}
					}
					else if((Measure->X > Target->X) &&  (Measure->Y < Target->Y) ) // x-/y+ (R-/B-) : Case 7,9
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B -= 1;
							else cal_Gamma->R -= 1;
						}
						else 
						{
							cal_Gamma->R -= XY_weight; cal_Gamma->B -= XY_weight;
						}
					}
					else if((Measure->X < Target->X) &&  (Measure->Y < Target->Y) && (Measure->Lv > Target->Lv))//x+/y+/Lv- (B-) : Case 25
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B -= 1;
							else cal_Gamma->B -= 2;
						}
						else
						{
							cal_Gamma->B -= XY_weight;
						}
					}
					else if((Measure->X < Target->X) &&  (Measure->Y < Target->Y) && (Measure->Lv < Target->Lv))//x+/y+/Lv+ (R+/B-) : Case 27
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B -= 1;
							else cal_Gamma->R += 1;
						}
						else
						{
							cal_Gamma->R += XY_weight; cal_Gamma->B -= XY_weight;
						}
					}
					else
					{
						//Nothing Should Happen
					}
				}
				else if(abs(Diff->X) > Limit->X && abs(Diff->Y) > Limit->Y && abs(Diff->Lv) < Limit->Lv)//X,Y ���� �
				{
					if((Measure->X > Target->X) &&  (Measure->Y > Target->Y)) // x-/y- (B+) : Case 2
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B += 1;
							else cal_Gamma->B += 2;
						}
						else
						{
							cal_Gamma->B += XY_weight;
						}
					}
					else if((Measure->X < Target->X) &&  (Measure->Y > Target->Y)) //x+/y- (R+/B+) : Case 20
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B += 1;
							else cal_Gamma->R += 1;
						}
						else
						{
							cal_Gamma->R += XY_weight; cal_Gamma->B += XY_weight;
						}
					}
					else if((Measure->X > Target->X) &&  (Measure->Y < Target->Y)) //x-/y+ (R-/B-) : Case 8
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B -= 1;
							else cal_Gamma->R -= 1;
						}
						else
						{
							cal_Gamma->R -= XY_weight; cal_Gamma->B -= XY_weight;
						}
						
					}
					else if((Measure->X < Target->X) &&  (Measure->Y < Target->Y)) // x+/y+ (B-) : Case 26
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B -= 1;
							else cal_Gamma->B -= 2;
						}
						else
						{
							cal_Gamma->B -= XY_weight;
						}
					}
					else
					{
						//Nothing Should Happen
					}
				}
				else if(abs(Diff->X) > Limit->X && abs(Diff->Y) < Limit->Y && abs(Diff->Lv) > Limit->Lv)//X,Lv ���� �
				{
					if((Measure->X > Target->X)) // x- (R-) : Case 4,6
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->R -= 1;
							else cal_Gamma->R -= 2;
						}
						else
						{
							cal_Gamma->R -= XY_weight;
						}
						
					}
					else if((Measure->X < Target->X))// x+ (R+) : Case 22,24
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->R += 1;
							else cal_Gamma->R += 2;
						}
						else
						{
							cal_Gamma->R += XY_weight;
						}
					}
					else
					{
						//Nothing Should Happen
					}
				}
				else if(abs(Diff->X) < Limit->X && abs(Diff->Y) > Limit->Y && abs(Diff->Lv) > Limit->Lv)//Y,Lv ���� �
				{
					if((Measure->Y > Target->Y)) // y- (R+/B+) : Case 10,12
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B += 1;
							else cal_Gamma->R += 1;
						}
						else
						{
							cal_Gamma->R += XY_weight; cal_Gamma->B += XY_weight;
						}
					}
					else if((Measure->Y < Target->Y)) // y+ (R-/B-) : Case 16,18
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B -= 1;
							else cal_Gamma->R -= 1;
						}
						else
						{
							cal_Gamma->R -= XY_weight; cal_Gamma->B -= XY_weight;
						}
					}
					else
					{
						//Nothing Should Happen
					}
				}
				else if(abs(Diff->X) > Limit->X && abs(Diff->Y) < Limit->Y && abs(Diff->Lv) < Limit->Lv)// X�� ���� � 
				{
					if((Measure->X > Target->X)) // x- (R-) : Case 5
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->R -= 1;
							else cal_Gamma->R -= 2;
						}
						else
						{
							cal_Gamma->R -= XY_weight;
						}
					}
					else if((Measure->X < Target->X))// x+ (R+) : Case 23
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->R += 1;
							else cal_Gamma->R += 2;
						}
						else
						{
							cal_Gamma->R += XY_weight;
						}
					}
					else
					{
						//Nothing Should Happen
					}
				}
				else if(abs(Diff->X) < Limit->X && abs(Diff->Y) > Limit->Y && abs(Diff->Lv) < Limit->Lv)// Y�� ���� � 
				{
					if((Measure->Y > Target->Y)) // y- (B+) : Case 11
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B += 1;
							else cal_Gamma->B += 2;
						}
						else
						{
							cal_Gamma->B += XY_weight;
						}
					}
					else if((Measure->Y < Target->Y)) // y+ (R-/B-) : Case 17
					{
						if(Infinite_Loop)
						{
							if(Case == 0)cal_Gamma->B -= 1;
							else cal_Gamma->R -= 1;
						}
						else
						{
							cal_Gamma->R -= XY_weight; cal_Gamma->B -= XY_weight;
						}
					}
					else
					{
						//Nothing Should Happen
					}
				}
				else if(abs(Diff->X) < Limit->X && abs(Diff->Y) < Limit->Y && abs(Diff->Lv) > Limit->Lv)// Lv�� ���� � 
				{
					if((abs(Diff->Lv) > (Limit->Lv * 2)) && (Lv_weight < 2))Lv_weight = 2; 
					else Lv_weight = 1; 

					if(Measure->Lv> Target->Lv) //Lv- (Vreg-) : Case 13
					{
						cal_vreg1 -= Lv_weight;	
					}
					else if(Measure->Lv < Target->Lv) //Lv+ (Vreg+) : Case 15
					{
						cal_vreg1 += Lv_weight;
					}
					else 
					{
						//Nothing Should Happen
					}
				}
				else // X,Y,Lv ��� Spec In (�̰Ŵ� ������ �̹� ó���� !)
				{
					//Nothing Should Happen
				}
			}
		}
	}
}

Vreg1_Compensate_State::Vreg1_Compensate_State(int Vreg1,int Vreg1_Register_Limit,int Gamma_Register_Limit,int Infinite_Loop,int XY_weight,int Lv_weight,Double_XYLv* Target,Double_XYLv* Measure, Double_XYLv* Limit,Int_RGB* Gamma)
	:RGB_Compensation(Gamma_Register_Limit,Infinite_Loop,XY_weight,Lv_weight,Target,Measure,Limit,Gamma)
{
	//std::cout<<"Vreg1_Compensate_State(~~~~~)"<<std::endl;	
	this->Vreg1 = Vreg1;
	this->Vreg1_Register_Limit = Vreg1_Register_Limit;
	this->cal_vreg1 = 0;
}
Vreg1_Compensate_State::~Vreg1_Compensate_State()
{
	//std::cout<<"~Vreg1_Compensate_State()"<<std::endl;	
}