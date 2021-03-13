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

class Double_XYLv
{
public:
	double X;
	double Y;
	double Lv;
	~Double_XYLv();
	Double_XYLv(double X,double Y,double Lv);
	void Set_XYLV(double X,double Y,double Lv);
};

class Int_RGB
{
public:
	int R;
	int G;
	int B;
	~Int_RGB();
	Int_RGB(int R,int G,int B);
	void Set_RGB(int R,int G,int B);
};