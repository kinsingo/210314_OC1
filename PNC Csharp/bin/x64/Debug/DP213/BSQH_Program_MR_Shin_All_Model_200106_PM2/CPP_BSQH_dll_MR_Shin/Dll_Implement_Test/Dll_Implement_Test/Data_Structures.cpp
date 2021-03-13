#include "stdafx.h"
#include "Data_Structures.h"

//Double_XYLv
Double_XYLv::Double_XYLv(double X,double Y,double Lv)
{
	//std::cout<<"Double_XYLv(~~~~~~~~~~~~~~~)"<<std::endl;
	Set_XYLV(X,Y,Lv);
}

Double_XYLv::~Double_XYLv()
{
	//std::cout<<"~Double_XYLv()"<<std::endl;
}

void Double_XYLv::Set_XYLV(double X,double Y,double Lv)
{
	this->X = X;
	this->Y = Y;
	this->Lv = Lv;
}

//Int_RGB
Int_RGB::Int_RGB(int R,int G,int B)
{
	//std::cout<<"Int_RGB(~~~~~~~~)"<<std::endl;
	Set_RGB(R,G,B);
}
Int_RGB::~Int_RGB()
{
	//std::cout<<"~Int_RGB()"<<std::endl;
}



void Int_RGB::Set_RGB(int R,int G,int B)
{
	this->R = R;
	this->G = G;
	this->B = B;
}
