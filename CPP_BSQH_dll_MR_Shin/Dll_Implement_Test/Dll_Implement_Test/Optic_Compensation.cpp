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
					Blue_Gamma_Plus_Or_Minus(true);//B++
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
					Blue_Gamma_Plus_Or_Minus(true);//B++
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
					Blue_Gamma_Plus_Or_Minus(true);//B++
				}
				else
				{
					switch(Case)
					{
					  case 0:
				          Red_Gamma_Plus_Or_Minus(true);//R++
					      Green_Gamma_Plus_Or_Minus(true);//G++
					      Blue_Gamma_Plus_Or_Minus(true);//B++
						  break;
					  case 1:
				          Red_Gamma_Plus_Or_Minus(true);//R++
					      Blue_Gamma_Plus_Or_Minus(true);//B++
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
					      Blue_Gamma_Plus_Or_Minus(true);//B++
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
				Blue_Gamma_Plus_Or_Minus(true);//B++
			}
			else
			{
				switch(Case)
				{
					case 0:
				        Red_Gamma_Plus_Or_Minus(true);//R++
				        Green_Gamma_Plus_Or_Minus(true);//G++
				        Blue_Gamma_Plus_Or_Minus(true);//B++
						break;
					case 1:
				        Red_Gamma_Plus_Or_Minus(true);//R++
				        Blue_Gamma_Plus_Or_Minus(true);//B++
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
				        Blue_Gamma_Plus_Or_Minus(true);//B++
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
		this->XY_weight = 1;

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
							//Y : Out of 1/2 Limit
							if(abs(Diff->Y) > (Limit->Y/2.0))
							{
								//X(Right,Out of 1/2)/Y(Down,Out of 1/2)/Lv(Up)
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
								//X(Right,Out of 1/2)/Y(Down,Within 1/2)/Lv(Up)
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
								
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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
									Blue_Gamma_Plus_Or_Minus(true);//B ++
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

	if((Gamma->R >= Lv_Priority_Max) && (Gamma->G >= Lv_Priority_Max)&& (Gamma->B >= Lv_Priority_Max))Lv_Priority = false; //True Condition 1
	else if((Gamma->R <= Lv_Priority_Min) && (Gamma->G <= Lv_Priority_Min)&& (Gamma->B <= Lv_Priority_Min))Lv_Priority = false;
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


void RGB_Compensate_State::Full_Compensation(int loop_count,int Infinite_Count)
{
	if(abs(Diff->X) < Limit->X && abs(Diff->Y) < Limit->Y && abs(Diff->Lv) < Limit->Lv)
	{
		Within_Spec_Limit = true;
	}
	else 
	{
		Double_XYLv LVPriority_Limit(0.015,0.015,(Limit->Lv * 5));
		int Lv_Priority_Max = (int)(Gamma_Register_Limit * 0.95);
		int Lv_Priority_Min = (int)(Gamma_Register_Limit * 0.05);
		bool Lv_Priority = Get_Is_Lv_Priority(Lv_Priority_Min,Lv_Priority_Max,LVPriority_Limit);

		if(Lv_Priority && (loop_count < 50))//Lv Priority Compensation
		{
			Lv_Priority_Compensation(Lv_Priority_Min,Lv_Priority_Max);
		}
		else
		{
			if(loop_count > 50)//Added On 200220
			{
				if(Target->Lv > 0.1) Precise_RGB_Compensation_2();
				else Precise_RGB_Compensation();
			}
			else
			{
				if(Infinite_Count == 0)
			    {
					Fast_RGB_Compensation();
				}
				else
				{
				    if(Target->Lv > 0.1) Precise_RGB_Compensation_2();
				    else Precise_RGB_Compensation();
				}
			}
		}
	}
}



RGB_Compensate_State::RGB_Compensate_State(int Gamma_Register_Limit,int Infinite_Loop,int XY_weight,int Lv_weight,Double_XYLv* Target,Double_XYLv* Measure, Double_XYLv* Limit,Int_RGB* Gamma)
	:RGB_Compensation(Gamma_Register_Limit,Infinite_Loop,XY_weight,Lv_weight,Target,Measure,Limit,Gamma)
{
	//std::cout<<"RGB_Compensate_State()"<<std::endl;
}

RGB_Compensate_State::~RGB_Compensate_State()
{
	//std::cout<<"~RGB_Compensate_State()"<<std::endl;	
}

