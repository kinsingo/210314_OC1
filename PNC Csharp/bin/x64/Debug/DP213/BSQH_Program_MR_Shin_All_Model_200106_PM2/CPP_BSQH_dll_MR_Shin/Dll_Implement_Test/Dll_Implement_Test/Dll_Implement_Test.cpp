// Dll_Implement_Test.cpp : 콘솔 응용 프로그램에 대한 진입점을 정의합니다.
//

#include "stdafx.h"
#include "BSQH_MR_SHIN_Algorithm.h"

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{

	vector<double> Gamma_Red_Voltage_Points;
	vector<double> Gamma_Green_Voltage_Points;
	vector<double> Gamma_Blue_Voltage_Points;
	vector<double> Finally_Measured_Lv;

	for (int i = 0; i < 10; i++)
	{
		Gamma_Red_Voltage_Points.push_back(5 - 0.2 * i);
		Gamma_Green_Voltage_Points.push_back(5 - 0.2 * i);
		Gamma_Blue_Voltage_Points.push_back(5 - 0.2 * i);
		Finally_Measured_Lv.push_back(-0.4 + 0.2 * i);
	}
	

	for (int i = 0; i < Finally_Measured_Lv.size(); i++)
	{
		cout << "R/G/B/Lv : " << Gamma_Red_Voltage_Points[i] << "/"
			<< Gamma_Green_Voltage_Points[i] << "/" << Gamma_Blue_Voltage_Points[i]
			<<"/"<< Finally_Measured_Lv[i]<< endl;
	}

	//0.01nit 미만은 그냥 참조 안함.(삭제)
	while (Finally_Measured_Lv[0] < 0.01)
	{
		Gamma_Red_Voltage_Points.erase(Gamma_Red_Voltage_Points.begin());
		Gamma_Green_Voltage_Points.erase(Gamma_Green_Voltage_Points.begin());
		Gamma_Blue_Voltage_Points.erase(Gamma_Blue_Voltage_Points.begin());
		Finally_Measured_Lv.erase(Finally_Measured_Lv.begin());
	}

	cout << endl;
	for (int i = 0; i < Finally_Measured_Lv.size(); i++)
	{
		cout << "R/G/B/Lv : " << Gamma_Red_Voltage_Points[i] << "/"
			<< Gamma_Green_Voltage_Points[i] << "/" << Gamma_Blue_Voltage_Points[i]
			<< "/" << Finally_Measured_Lv[i] << endl;
	}

	Gamma_Red_Voltage_Points[2] = -1;


	//역전되는 곳은 참조 안함.(삭제)
	for (int i = 0; i < Finally_Measured_Lv.size() - 1; )
	{
		if (((Gamma_Red_Voltage_Points[i + 1] - Gamma_Red_Voltage_Points[i]) >= 0)
			|| ((Gamma_Green_Voltage_Points[i + 1] - Gamma_Green_Voltage_Points[i]) >= 0)
			|| ((Gamma_Blue_Voltage_Points[i + 1] - Gamma_Blue_Voltage_Points[i]) >= 0)
			|| ((Finally_Measured_Lv[i + 1] - Finally_Measured_Lv[i]) <= 0))
		{
			Gamma_Red_Voltage_Points.erase(Gamma_Red_Voltage_Points.begin() + i);
			Gamma_Green_Voltage_Points.erase(Gamma_Green_Voltage_Points.begin() + i);
			Gamma_Blue_Voltage_Points.erase(Gamma_Blue_Voltage_Points.begin() + i);
			Finally_Measured_Lv.erase(Finally_Measured_Lv.begin() + i);
		}
		else
		{
			i++;
		}
	}

	cout << endl;
	for (int i = 0; i < Finally_Measured_Lv.size(); i++)
	{
		cout << "R/G/B/Lv : " << Gamma_Red_Voltage_Points[i] << "/"
			<< Gamma_Green_Voltage_Points[i] << "/" << Gamma_Blue_Voltage_Points[i]
			<< "/" << Finally_Measured_Lv[i] << endl;
	}
	

	Gamma_Green_Voltage_Points[4] = -1;


	//역전되는 곳은 참조 안함.(삭제)
	for (int i = 0; i < Finally_Measured_Lv.size() - 1; )
	{
		if (((Gamma_Red_Voltage_Points[i + 1] - Gamma_Red_Voltage_Points[i]) >= 0)
			|| ((Gamma_Green_Voltage_Points[i + 1] - Gamma_Green_Voltage_Points[i]) >= 0)
			|| ((Gamma_Blue_Voltage_Points[i + 1] - Gamma_Blue_Voltage_Points[i]) >= 0)
			|| ((Finally_Measured_Lv[i + 1] - Finally_Measured_Lv[i]) <= 0))
		{
			Gamma_Red_Voltage_Points.erase(Gamma_Red_Voltage_Points.begin() + i);
			Gamma_Green_Voltage_Points.erase(Gamma_Green_Voltage_Points.begin() + i);
			Gamma_Blue_Voltage_Points.erase(Gamma_Blue_Voltage_Points.begin() + i);
			Finally_Measured_Lv.erase(Finally_Measured_Lv.begin() + i);
		}
		else
		{
			i++;
		}
	}

	cout << endl;
	for (int i = 0; i < Finally_Measured_Lv.size(); i++)
	{
		cout << "R/G/B/Lv : " << Gamma_Red_Voltage_Points[i] << "/"
			<< Gamma_Green_Voltage_Points[i] << "/" << Gamma_Blue_Voltage_Points[i]
			<< "/" << Finally_Measured_Lv[i] << endl;
	}

	Gamma_Blue_Voltage_Points[2] = -1;


	//역전되는 곳은 참조 안함.(삭제)
	for (int i = 0; i < Finally_Measured_Lv.size() - 1; )
	{
		if (((Gamma_Red_Voltage_Points[i + 1] - Gamma_Red_Voltage_Points[i]) >= 0)
			|| ((Gamma_Green_Voltage_Points[i + 1] - Gamma_Green_Voltage_Points[i]) >= 0)
			|| ((Gamma_Blue_Voltage_Points[i + 1] - Gamma_Blue_Voltage_Points[i]) >= 0)
			|| ((Finally_Measured_Lv[i + 1] - Finally_Measured_Lv[i]) <= 0))
		{
			Gamma_Red_Voltage_Points.erase(Gamma_Red_Voltage_Points.begin() + i);
			Gamma_Green_Voltage_Points.erase(Gamma_Green_Voltage_Points.begin() + i);
			Gamma_Blue_Voltage_Points.erase(Gamma_Blue_Voltage_Points.begin() + i);
			Finally_Measured_Lv.erase(Finally_Measured_Lv.begin() + i);
		}
		else
		{
			i++;
		}
	}

	cout << endl;
	for (int i = 0; i < Finally_Measured_Lv.size(); i++)
	{
		cout << "R/G/B/Lv : " << Gamma_Red_Voltage_Points[i] << "/"
			<< Gamma_Green_Voltage_Points[i] << "/" << Gamma_Blue_Voltage_Points[i]
			<< "/" << Finally_Measured_Lv[i] << endl;
	}


	Finally_Measured_Lv[2] = 10;


	//역전되는 곳은 참조 안함.(삭제)
	for (int i = 0; i < Finally_Measured_Lv.size() - 1; )
	{
		if (((Gamma_Red_Voltage_Points[i + 1] - Gamma_Red_Voltage_Points[i]) >= 0)
			|| ((Gamma_Green_Voltage_Points[i + 1] - Gamma_Green_Voltage_Points[i]) >= 0)
			|| ((Gamma_Blue_Voltage_Points[i + 1] - Gamma_Blue_Voltage_Points[i]) >= 0)
			|| ((Finally_Measured_Lv[i + 1] - Finally_Measured_Lv[i]) <= 0))
		{
			Gamma_Red_Voltage_Points.erase(Gamma_Red_Voltage_Points.begin() + i);
			Gamma_Green_Voltage_Points.erase(Gamma_Green_Voltage_Points.begin() + i);
			Gamma_Blue_Voltage_Points.erase(Gamma_Blue_Voltage_Points.begin() + i);
			Finally_Measured_Lv.erase(Finally_Measured_Lv.begin() + i);
		}
		else
		{
			i++;
		}
	}

	cout << endl;
	for (int i = 0; i < Finally_Measured_Lv.size(); i++)
	{
		cout << "R/G/B/Lv : " << Gamma_Red_Voltage_Points[i] << "/"
			<< Gamma_Green_Voltage_Points[i] << "/" << Gamma_Blue_Voltage_Points[i]
			<< "/" << Finally_Measured_Lv[i] << endl;
	}


	return 0;
}



