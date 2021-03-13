#include "stdafx.h"
#include "DP213_Initial_Fast_OC_Related.h"
#include "math.h"
#include <time.h>
#include <iostream>
#include <list>
#include <ctime>
#include <vector>
#include <algorithm>
#include <cctype>
#include <iomanip>
#include <sstream>

using namespace std;

namespace DP213_Static
{
	static const int Max_Gray_Amount = 11;
}

//Calculating Initial RGBVreg1 Algorithm (Through Monotone-Cubic-Interpolation)
extern "C" __declspec (dllexport)void DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method_Through_MCI(double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, int& Vreg1_Dec_Init, int& Gamma_R, int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
    , double* Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0)
{
    if (band >= 1 && Selected_Band[band] == true)
    {
        double* Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_R, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREF4095, Voltage_VREF0);
        double* Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_G, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREF4095, Voltage_VREF0);
        double* Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_B, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREF4095, Voltage_VREF0);

        std::sort(Previous_Band_Gamma_Red_Voltage, Previous_Band_Gamma_Red_Voltage + DP213_Static::Max_Gray_Amount, decrease);
        std::sort(Previous_Band_Gamma_Green_Voltage, Previous_Band_Gamma_Green_Voltage + DP213_Static::Max_Gray_Amount, decrease);
        std::sort(Previous_Band_Gamma_Blue_Voltage, Previous_Band_Gamma_Blue_Voltage + DP213_Static::Max_Gray_Amount, decrease);
        std::sort(Previous_Band_Finally_Measured_Lv, Previous_Band_Finally_Measured_Lv + DP213_Static::Max_Gray_Amount);

        vector<double> Gamma_Red_Voltage_Points;
        vector<double> Gamma_Green_Voltage_Points;
        vector<double> Gamma_Blue_Voltage_Points;
        vector<double> Finally_Measured_Lv;

            for (int g = 0; g < DP213_Static::Max_Gray_Amount; g++)
            {
                Gamma_Red_Voltage_Points.push_back(Previous_Band_Gamma_Red_Voltage[g]);
                Gamma_Green_Voltage_Points.push_back(Previous_Band_Gamma_Green_Voltage[g]);
                Gamma_Blue_Voltage_Points.push_back(Previous_Band_Gamma_Blue_Voltage[g]);
                Finally_Measured_Lv.push_back(Previous_Band_Finally_Measured_Lv[g]);
            }

        //0.01nit 미만은 그냥 참조 안함.(삭제)
        while (Finally_Measured_Lv[0] < 0.01)
        {
            Gamma_Red_Voltage_Points.erase(Gamma_Red_Voltage_Points.begin());
            Gamma_Green_Voltage_Points.erase(Gamma_Green_Voltage_Points.begin());
            Gamma_Blue_Voltage_Points.erase(Gamma_Blue_Voltage_Points.begin());
            Finally_Measured_Lv.erase(Finally_Measured_Lv.begin());
        }

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

        double* mM_R = CreateMonotoneCubicSpline(Gamma_Red_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Red_Voltage_Points[0]);
        double* mM_G = CreateMonotoneCubicSpline(Gamma_Green_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Green_Voltage_Points[0]);
        double* mM_B = CreateMonotoneCubicSpline(Gamma_Blue_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Blue_Voltage_Points[0]);

        double Calculated_Vdata_Red = interpolate(Gamma_Red_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Red_Voltage_Points[0], mM_R, band_Target_Lv);
        double Calculated_Vdata_Green = interpolate(Gamma_Green_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Green_Voltage_Points[0], mM_G, band_Target_Lv);
        double Calculated_Vdata_Blue = interpolate(Gamma_Blue_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Blue_Voltage_Points[0], mM_B, band_Target_Lv);
        double Vreg1_voltage = Voltage_VREF4095 + ((Calculated_Vdata_Green - Voltage_VREF4095) * (900.0 / (Previous_Band_Gamma_Green[0] + 389.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
      
        Vreg1_Dec_Init = DP213_Get_Vreg1_Dec(Voltage_VREF4095, Voltage_VREF0, Vreg1_voltage);
        Gamma_R = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Red);
        Gamma_B = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Blue);

        DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(Gamma_R, Previous_Band_Gamma_Green[0], Gamma_B, Previous_Band_Gamma_Red[0], Previous_Band_Gamma_Green[0], Previous_Band_Gamma_Blue[0]);

        delete[] Previous_Band_Gamma_Red_Voltage;
        delete[] Previous_Band_Gamma_Green_Voltage;
        delete[] Previous_Band_Gamma_Blue_Voltage;
        delete[] mM_R;
        delete[] mM_G;
        delete[] mM_B;
    }
    else //Band0 + Other not selected Bands's
    {
        //Do nothing
    }
}


extern "C" __declspec (dllexport) void DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method_Through_MCI(int Vreg1, double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
    , double* Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095, double Voltage_VREF0)
{
    if (band >= 1 && Selected_Band[band] == true)
    {
        double* Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_R, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREF4095, Voltage_VREF0);
        double* Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_G, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREF4095, Voltage_VREF0);
        double* Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_B, Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREF4095, Voltage_VREF0);

        std::sort(Previous_Band_Gamma_Red_Voltage, Previous_Band_Gamma_Red_Voltage + DP213_Static::Max_Gray_Amount, decrease);
        std::sort(Previous_Band_Gamma_Green_Voltage, Previous_Band_Gamma_Green_Voltage + DP213_Static::Max_Gray_Amount, decrease);
        std::sort(Previous_Band_Gamma_Blue_Voltage, Previous_Band_Gamma_Blue_Voltage + DP213_Static::Max_Gray_Amount, decrease);
        std::sort(Previous_Band_Finally_Measured_Lv, Previous_Band_Finally_Measured_Lv + DP213_Static::Max_Gray_Amount);

        vector<double> Gamma_Red_Voltage_Points;
        vector<double> Gamma_Green_Voltage_Points;
        vector<double> Gamma_Blue_Voltage_Points;
        vector<double> Finally_Measured_Lv;


        for (int g = 0; g < DP213_Static::Max_Gray_Amount; g++)
        {
            Gamma_Red_Voltage_Points.push_back(Previous_Band_Gamma_Red_Voltage[g]);
            Gamma_Green_Voltage_Points.push_back(Previous_Band_Gamma_Green_Voltage[g]);
            Gamma_Blue_Voltage_Points.push_back(Previous_Band_Gamma_Blue_Voltage[g]);
            Finally_Measured_Lv.push_back(Previous_Band_Finally_Measured_Lv[g]);
        }

        //0.01nit 미만은 그냥 참조 안함.(삭제)
        while (Finally_Measured_Lv[0] < 0.01)
        {
            Gamma_Red_Voltage_Points.erase(Gamma_Red_Voltage_Points.begin());
            Gamma_Green_Voltage_Points.erase(Gamma_Green_Voltage_Points.begin());
            Gamma_Blue_Voltage_Points.erase(Gamma_Blue_Voltage_Points.begin());
            Finally_Measured_Lv.erase(Finally_Measured_Lv.begin());
        }

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

        double* mM_R = CreateMonotoneCubicSpline(Gamma_Red_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Red_Voltage_Points[0]);
        double* mM_G = CreateMonotoneCubicSpline(Gamma_Green_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Green_Voltage_Points[0]);
        double* mM_B = CreateMonotoneCubicSpline(Gamma_Blue_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Blue_Voltage_Points[0]);

        double Calculated_Vdata_Red = interpolate(Gamma_Red_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Red_Voltage_Points[0], mM_R, band_Target_Lv);
        double Calculated_Vdata_Green = interpolate(Gamma_Green_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Green_Voltage_Points[0], mM_G, band_Target_Lv);
        double Calculated_Vdata_Blue = interpolate(Gamma_Blue_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Blue_Voltage_Points[0], mM_B, band_Target_Lv);
        double Vreg1_voltage = DP213_Get_Vreg1_Voltage(Voltage_VREF4095, Voltage_VREF0, Vreg1);

        //Got the Vreg1 
        //Need to get Gamma_R/B
        Gamma_R = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Red);
        Gamma_G = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Green);
        Gamma_B = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Blue);

        DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(Gamma_R, Gamma_G, Gamma_B, Previous_Band_Gamma_Red[0], Previous_Band_Gamma_Green[0], Previous_Band_Gamma_Blue[0]);

        delete[] Previous_Band_Gamma_Red_Voltage;
        delete[] Previous_Band_Gamma_Green_Voltage;
        delete[] Previous_Band_Gamma_Blue_Voltage;
        delete[] mM_R;
        delete[] mM_G;
        delete[] mM_B;
    }
    else //Band0 + Other not selected Bands's
    {
        //Do nothing
    }
}


extern "C" __declspec (dllexport)  void DP213_Get_Intial_R_G_B_Using_3Points_Method_Through_MCI(double Combine_Lv_Ratio, double* Band_Voltage_AM1_R, double* Band_Voltage_AM1_G, double* Band_Voltage_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Band_Gray_Gamma_Red, int* Band_Gray_Gamma_Green, int* Band_Gray_Gamma_Blue, double* Band_Gray_Finally_Measured_Lv,
    int* Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage, double Voltage_VREF4095, double Voltage_VREF0)
{
    if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
    {
        SJH_Matrix* M = new SJH_Matrix();
        double** Band_Gray_Gamma_Red_Voltage = M->MatrixCreate(band, DP213_Static::Max_Gray_Amount);
        double** Band_Gray_Gamma_Green_Voltage = M->MatrixCreate(band, DP213_Static::Max_Gray_Amount);
        double** Band_Gray_Gamma_Blue_Voltage = M->MatrixCreate(band, DP213_Static::Max_Gray_Amount);

        for (int i = 0; i < band; i++)
        {
            int* Previous_Band_Gamma_R = new int[DP213_Static::Max_Gray_Amount];
            int* Previous_Band_Gamma_G = new int[DP213_Static::Max_Gray_Amount];
            int* Previous_Band_Gamma_B = new int[DP213_Static::Max_Gray_Amount];

            for (int g = 0; g < DP213_Static::Max_Gray_Amount; g++)
            {
                Previous_Band_Gamma_R[g] = Band_Gray_Gamma_Red[(DP213_Static::Max_Gray_Amount * i) + g];
                Previous_Band_Gamma_G[g] = Band_Gray_Gamma_Green[(DP213_Static::Max_Gray_Amount * i) + g];
                Previous_Band_Gamma_B[g] = Band_Gray_Gamma_Blue[(DP213_Static::Max_Gray_Amount * i) + g];
            }
            //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
            Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_R[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_R, Voltage_VREF4095, Voltage_VREF0);
            Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_G[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_G, Voltage_VREF4095, Voltage_VREF0);
            Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_B[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_B, Voltage_VREF4095, Voltage_VREF0);


            delete[] Previous_Band_Gamma_R;
            delete[] Previous_Band_Gamma_G;
            delete[] Previous_Band_Gamma_B;
        }


        //Need to...
        //Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance 
        vector<double> Gamma_Red_Voltage_Points;
        vector<double> Gamma_Green_Voltage_Points;
        vector<double> Gamma_Blue_Voltage_Points;
        vector<double> Finally_Measured_Lv;
        for (int b = 0; b < band; b++)
        {
            for (int g = 0; g < DP213_Static::Max_Gray_Amount; g++)
            {
                Gamma_Red_Voltage_Points.push_back(Band_Gray_Gamma_Red_Voltage[b][g]);
                Gamma_Green_Voltage_Points.push_back(Band_Gray_Gamma_Green_Voltage[b][g]);
                Gamma_Blue_Voltage_Points.push_back(Band_Gray_Gamma_Blue_Voltage[b][g]);
                Finally_Measured_Lv.push_back(Band_Gray_Finally_Measured_Lv[(DP213_Static::Max_Gray_Amount * b) + g]);
            }
        }

        sort(Gamma_Red_Voltage_Points.begin(), Gamma_Red_Voltage_Points.end(), decrease);
        sort(Gamma_Green_Voltage_Points.begin(), Gamma_Green_Voltage_Points.end(), decrease);
        sort(Gamma_Blue_Voltage_Points.begin(), Gamma_Blue_Voltage_Points.end(), decrease);
        sort(Finally_Measured_Lv.begin(), Finally_Measured_Lv.end());

        //0.1nit 미만은 그냥 참조 안함.(삭제)
        while (Finally_Measured_Lv[0] < 0.1)
        {
            Gamma_Red_Voltage_Points.erase(Gamma_Red_Voltage_Points.begin());
            Gamma_Green_Voltage_Points.erase(Gamma_Green_Voltage_Points.begin());
            Gamma_Blue_Voltage_Points.erase(Gamma_Blue_Voltage_Points.begin());
            Finally_Measured_Lv.erase(Finally_Measured_Lv.begin());
        }


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

        double* mM_R = CreateMonotoneCubicSpline(Gamma_Red_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Red_Voltage_Points[0]);
        double* mM_G = CreateMonotoneCubicSpline(Gamma_Green_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Green_Voltage_Points[0]);
        double* mM_B = CreateMonotoneCubicSpline(Gamma_Blue_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Blue_Voltage_Points[0]);

        double Calculated_Vdata_Red = interpolate(Gamma_Red_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Red_Voltage_Points[0], mM_R, Target_Lv);
        double Calculated_Vdata_Green = interpolate(Gamma_Green_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Green_Voltage_Points[0], mM_G, Target_Lv);
        double Calculated_Vdata_Blue = interpolate(Gamma_Blue_Voltage_Points.size(), Finally_Measured_Lv.size(), &Finally_Measured_Lv[0], &Gamma_Blue_Voltage_Points[0], mM_B, Target_Lv);
      
        //Get Gamma_R/G/B From Calculated Voltage_R/G/B
        double Current_Band_Vreg1_Voltage = DP213_Get_Vreg1_Voltage(Voltage_VREF4095, Voltage_VREF0, Band_Vreg1_Dec[band]);
        Gamma_R = DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_R[band], Prvious_Gray_Gamma_R_Voltage, Calculated_Vdata_Red, gray);
        Gamma_G = DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_G[band], Prvious_Gray_Gamma_G_Voltage, Calculated_Vdata_Green, gray);
        Gamma_B = DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_B[band], Prvious_Gray_Gamma_B_Voltage, Calculated_Vdata_Blue, gray);

        int Prev_Band_Current_Gray_Gamma_R = Band_Gray_Gamma_Red[(DP213_Static::Max_Gray_Amount * (band - 1)) + gray];
        int Prev_Band_Current_Gray_Gamma_G = Band_Gray_Gamma_Green[(DP213_Static::Max_Gray_Amount * (band - 1)) + gray];
        int Prev_Band_Current_Gray_Gamma_B = Band_Gray_Gamma_Blue[(DP213_Static::Max_Gray_Amount * (band - 1)) + gray];
        DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(Gamma_R, Gamma_G, Gamma_B, Prev_Band_Current_Gray_Gamma_R, Prev_Band_Current_Gray_Gamma_G, Prev_Band_Current_Gray_Gamma_B);

        delete M;
        delete[] Band_Gray_Gamma_Red_Voltage;
        delete[] Band_Gray_Gamma_Green_Voltage;
        delete[] Band_Gray_Gamma_Blue_Voltage;
        delete[] mM_R;
        delete[] mM_G;
        delete[] mM_B;
    }
    else //Band0 + Other not selected Bands's
    {

    }
}











//Calculate to get R/Vreg1/B
extern "C" __declspec (dllexport )void DP213_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B,int& Vreg1_Dec_Init,int& Gamma_R,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095,double Voltage_VREF0)
{
	if (band >= 1 && Selected_Band[band] == true)
    {
		SJH_Matrix *M = new SJH_Matrix();
		double* Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_R,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREF4095,Voltage_VREF0);
        double* Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_G,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREF4095,Voltage_VREF0);
        double* Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_B,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREF4095,Voltage_VREF0);
        
		double** A_R = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double** A_G = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double** A_B = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);

        //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
        int count = 0;
        for (int i = 0; i <= (DP213_Static::Max_Gray_Amount - 1); i++)
        {
            count = 0;
            for (int j = (DP213_Static::Max_Gray_Amount - 1); j >= 0; j--)
            {
                A_R[i][count] = pow(Previous_Band_Gamma_Red_Voltage[i], j);
                A_G[i][count] = pow(Previous_Band_Gamma_Green_Voltage[i], j);
                A_B[i][count] = pow(Previous_Band_Gamma_Blue_Voltage[i], j);
                count++;
            }
        }

        //Get C[DP213_Static.Max_Gray_Amount] = inv(A)[DP213_Static.Max_Gray_Amount,DP213_Static.Max_Gray_Amount] * Previous_Band_Target_Lv[DP213_Static.Max_Gray_Amount]
        double** Inv_A_R = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double** Inv_A_G = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double** Inv_A_B = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double* C_R = new double[DP213_Static::Max_Gray_Amount];
        double* C_G = new double[DP213_Static::Max_Gray_Amount];
        double* C_B = new double[DP213_Static::Max_Gray_Amount];
        Inv_A_R = M->MatrixInverse(A_R,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount);
        Inv_A_G = M->MatrixInverse(A_G,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount);
        Inv_A_B = M->MatrixInverse(A_B,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount);
        C_R = M->Matrix_Multiply(Inv_A_R,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount, Previous_Band_Finally_Measured_Lv);
        C_G = M->Matrix_Multiply(Inv_A_G,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount, Previous_Band_Finally_Measured_Lv);
        C_B = M->Matrix_Multiply(Inv_A_B,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount, Previous_Band_Finally_Measured_Lv);

        //Show "C10*(Vdata^10) + C9*(Vdata^9) + C8*(Vdata^8) + C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
        double Target_Lv = band_Target_Lv;
        double Calculated_Vdata_Red = 0;
        double Calculated_Vdata_Green = 0;
        double Calculated_Vdata_Blue = 0;
                
        double Calculated_Target_Lv = 0;
        int iteration = 0;

        //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
        //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
        double Previous_Band_Vreg1_Voltage = DP213_Get_Vreg1_Voltage(Voltage_VREF4095,Voltage_VREF0,Previous_Band_Vreg1_Dec);
        double Actual_Previous_Vdata_Red = DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
        double Actual_Previous_Vdata_Green = DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
        double Actual_Previous_Vdata_Blue = DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);
                
        //Red
        for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            
			for (int j = (DP213_Static::Max_Gray_Amount - 1); j >= 0; j--) 
				Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
            
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0) 
			{
					Calculated_Vdata_Red = Vdata;
					break;
			}
        }

        //Green
        for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            
			for (int j = (DP213_Static::Max_Gray_Amount - 1); j >= 0; j--) 
				Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) 
			{
					Calculated_Vdata_Green = Vdata;
					break;
			}
        }

        //Blue
        for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
           
			for (int j = (DP213_Static::Max_Gray_Amount - 1); j >= 0; j--) 
				Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0)
			{
					Calculated_Vdata_Blue = Vdata;
					break;
			}
        }


        double Vreg1_voltage = Voltage_VREF4095 + ((Calculated_Vdata_Green - Voltage_VREF4095) * (900.0 / (Previous_Band_Gamma_Green[0] + 389.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
        Vreg1_Dec_Init = DP213_Get_Vreg1_Dec(Voltage_VREF4095,Voltage_VREF0,Vreg1_voltage);
		
		//Got the Vreg1 
        //Need to get Gamma_R/B
	    Gamma_R = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Red);
        Gamma_B = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Blue);
		

		DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(Gamma_R,Previous_Band_Gamma_Green[0],Gamma_B,Previous_Band_Gamma_Red[0],Previous_Band_Gamma_Green[0],Previous_Band_Gamma_Blue[0]);

		delete[] Previous_Band_Gamma_Red_Voltage;
        delete[] Previous_Band_Gamma_Green_Voltage ;
		delete[] Previous_Band_Gamma_Blue_Voltage ;
		delete M;
		delete[] A_R ;
		delete[] A_G ;
		delete[] A_B ;
        delete[] Inv_A_R;
        delete[] Inv_A_G;
        delete[] Inv_A_B;
        delete[] C_R;
        delete[] C_G;
        delete[] C_B;
    }
    else //Band0 + Other not selected Bands's
    {
        //Do nothing
    }
}

//Calculate and Get Initial R/G/B (Gray255)
extern "C" __declspec (dllexport ) void DP213_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(int Vreg1,double Prev_Band_Voltage_AM1_R, double Prev_Band_Voltage_AM1_G, double Prev_Band_Voltage_AM1_B,int& Gamma_R,int& Gamma_G,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Finally_Measured_Lv, double Voltage_VREF4095,double Voltage_VREF0)
{
	if (band >= 1 && Selected_Band[band] == true)
    {
		SJH_Matrix *M = new SJH_Matrix();

		double* Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_R,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREF4095,Voltage_VREF0);
        double* Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_G,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREF4095,Voltage_VREF0);
        double* Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Prev_Band_Voltage_AM1_B,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREF4095,Voltage_VREF0);
        
		double** A_R = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double** A_G = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double** A_B = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);

        //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
        int count = 0;
        for (int i = 0; i <= (DP213_Static::Max_Gray_Amount - 1); i++)
        {
            count = 0;
            for (int j = (DP213_Static::Max_Gray_Amount - 1); j >= 0; j--)
            {
                A_R[i][count] = pow(Previous_Band_Gamma_Red_Voltage[i], j);
                A_G[i][count] = pow(Previous_Band_Gamma_Green_Voltage[i], j);
                A_B[i][count] = pow(Previous_Band_Gamma_Blue_Voltage[i], j);
                count++;
            }
        }

        //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
        double** Inv_A_R = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double** Inv_A_G = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double** Inv_A_B = M->MatrixCreate(DP213_Static::Max_Gray_Amount, DP213_Static::Max_Gray_Amount);
        double* C_R = new double[DP213_Static::Max_Gray_Amount];
        double* C_G = new double[DP213_Static::Max_Gray_Amount];
        double* C_B = new double[DP213_Static::Max_Gray_Amount];
        Inv_A_R = M->MatrixInverse(A_R,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount);
        Inv_A_G = M->MatrixInverse(A_G,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount);
        Inv_A_B = M->MatrixInverse(A_B,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount);
        C_R = M->Matrix_Multiply(Inv_A_R,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount, Previous_Band_Finally_Measured_Lv);
        C_G = M->Matrix_Multiply(Inv_A_G,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount, Previous_Band_Finally_Measured_Lv);
        C_B = M->Matrix_Multiply(Inv_A_B,DP213_Static::Max_Gray_Amount,DP213_Static::Max_Gray_Amount, Previous_Band_Finally_Measured_Lv);

        //Show "C10*(Vdata^10) + C9*(Vdata^9) + C8*(Vdata^8) + C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
        double Target_Lv = band_Target_Lv;
        double Calculated_Vdata_Red = 0;
        double Calculated_Vdata_Green = 0;
        double Calculated_Vdata_Blue = 0;
                
        double Calculated_Target_Lv = 0;
        int iteration = 0;

        //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
        //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
        double Previous_Band_Vreg1_Voltage = DP213_Get_Vreg1_Voltage(Voltage_VREF4095,Voltage_VREF0,Previous_Band_Vreg1_Dec);
        double Actual_Previous_Vdata_Red = DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
        double Actual_Previous_Vdata_Green = DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
        double Actual_Previous_Vdata_Blue = DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);
        
        //Red
        for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
         
			for (int j = (DP213_Static::Max_Gray_Amount - 1); j >= 0; j--) 
				Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
        
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0) 
			{
				Calculated_Vdata_Red = Vdata;
				break;
			}
        }

        //Green
        for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            
			for (int j = (DP213_Static::Max_Gray_Amount - 1); j >= 0; j--) 
				Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) 
			{
				Calculated_Vdata_Green = Vdata;
				break;
			}
        }

        //Blue
        for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;

            for (int j = (DP213_Static::Max_Gray_Amount - 1); j >= 0; j--) 
				Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0) 
			{
					Calculated_Vdata_Blue = Vdata;
					break;
			}
        }

		double Vreg1_voltage = DP213_Get_Vreg1_Voltage(Voltage_VREF4095,Voltage_VREF0,Vreg1);
        //Got the Vreg1 
        //Need to get Gamma_R/B
	    Gamma_R = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Red);
		Gamma_G = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Green);
        Gamma_B = DP213_Get_AM2_Gamma_Dec(Voltage_VREF4095, Vreg1_voltage, Calculated_Vdata_Blue);

		DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(Gamma_R,Gamma_G,Gamma_B,Previous_Band_Gamma_Red[0],Previous_Band_Gamma_Green[0],Previous_Band_Gamma_Blue[0]);

		delete[] Previous_Band_Gamma_Red_Voltage;
        delete[] Previous_Band_Gamma_Green_Voltage ;
		delete[] Previous_Band_Gamma_Blue_Voltage ;
		delete M;
		delete[] A_R ;
		delete[] A_G ;
		delete[] A_B ;
        delete[] Inv_A_R;
        delete[] Inv_A_G;
        delete[] Inv_A_B;
        delete[] C_R;
        delete[] C_G;
        delete[] C_B;
    }
    else //Band0 + Other not selected Bands's
    {
        //Do nothing
    }
}

//Calculate to get R/G/B
extern "C" __declspec (dllexport )  void DP213_Get_Intial_R_G_B_Using_3Points_Method(double Combine_Lv_Ratio,double* Band_Voltage_AM1_R, double* Band_Voltage_AM1_G, double* Band_Voltage_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Band_Gray_Gamma_Red, int* Band_Gray_Gamma_Green, int* Band_Gray_Gamma_Blue, double* Band_Gray_Finally_Measured_Lv,
            int* Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,double Voltage_VREF4095, double Voltage_VREF0)
{
	if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
    {
		SJH_Matrix *M = new SJH_Matrix();

		double** Band_Gray_Gamma_Red_Voltage = M->MatrixCreate(band, DP213_Static::Max_Gray_Amount);
        double** Band_Gray_Gamma_Green_Voltage = M->MatrixCreate(band, DP213_Static::Max_Gray_Amount);
        double** Band_Gray_Gamma_Blue_Voltage = M->MatrixCreate(band, DP213_Static::Max_Gray_Amount);

        for (int i = 0; i < band; i++)
        {
			int* Previous_Band_Gamma_R = new int[DP213_Static::Max_Gray_Amount];
			int* Previous_Band_Gamma_G = new int[DP213_Static::Max_Gray_Amount];
			int* Previous_Band_Gamma_B = new int[DP213_Static::Max_Gray_Amount];
			
			for(int g = 0;g <DP213_Static::Max_Gray_Amount; g++)
			{
				Previous_Band_Gamma_R[g] = Band_Gray_Gamma_Red[(DP213_Static::Max_Gray_Amount * i) + g];
				Previous_Band_Gamma_G[g] = Band_Gray_Gamma_Green[(DP213_Static::Max_Gray_Amount * i) + g];
				Previous_Band_Gamma_B[g] = Band_Gray_Gamma_Blue[(DP213_Static::Max_Gray_Amount * i) + g];
			}
            //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
            Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_R[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_R, Voltage_VREF4095, Voltage_VREF0);
            Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_G[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_G, Voltage_VREF4095, Voltage_VREF0);
            Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Band_Voltage_AM1_B[i], Band_Vreg1_Dec[i], Previous_Band_Gamma_B, Voltage_VREF4095, Voltage_VREF0);


			delete[] Previous_Band_Gamma_R; 
			delete[] Previous_Band_Gamma_G;
			delete[] Previous_Band_Gamma_B;
		}

                
        //Need to...
        //Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance
		int Points_Num = band * DP213_Static::Max_Gray_Amount ;
        double* Gamma_Red_Voltage_Points = new double[Points_Num];
        double* Gamma_Green_Voltage_Points = new double[Points_Num];
        double* Gamma_Blue_Voltage_Points = new double[Points_Num];
        double* Target_Lv_Points = new double[Points_Num];

        for (int b = 0; b < band; b++)
        {
            for (int g = 0; g < DP213_Static::Max_Gray_Amount; g++)
            {
                Gamma_Red_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g] = Band_Gray_Gamma_Red_Voltage[b][g];
                Gamma_Green_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g] = Band_Gray_Gamma_Green_Voltage[b][g];
                Gamma_Blue_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g] = Band_Gray_Gamma_Blue_Voltage[b][g];
                Target_Lv_Points[(DP213_Static::Max_Gray_Amount * b) + g] = Band_Gray_Finally_Measured_Lv[(DP213_Static::Max_Gray_Amount * b) + g];
            }
        }
		std::sort(Gamma_Red_Voltage_Points, Gamma_Red_Voltage_Points + Points_Num);
		std::sort(Gamma_Green_Voltage_Points, Gamma_Green_Voltage_Points + Points_Num);
		std::sort(Gamma_Blue_Voltage_Points, Gamma_Blue_Voltage_Points + Points_Num);
		std::sort(Target_Lv_Points, Target_Lv_Points + Points_Num, decrease);
				
		//-----------------Added On 200311-------------------
		std::vector<double> Combinded_Gamma_Red_Voltage_Points;
        std::vector<double> Combinded_Gamma_Green_Voltage_Points;
        std::vector<double> Combinded_Gamma_Blue_Voltage_Points;
		std::vector<double> Combinded_Target_Lv_Points;

		for (int b = 0; b < band; b++)
        {
            for (int g = 0; g < DP213_Static::Max_Gray_Amount; g++)
            {
                if(b == 0 && g == 0)//First Point
				{
					Combinded_Gamma_Red_Voltage_Points.push_back(Gamma_Red_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
                    Combinded_Gamma_Green_Voltage_Points.push_back(Gamma_Green_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
                    Combinded_Gamma_Blue_Voltage_Points.push_back(Gamma_Blue_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
                    Combinded_Target_Lv_Points.push_back(Target_Lv_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
				}
				else
				{
					double Abs_Diff_Lv_Between_Two_Points = abs(Combinded_Target_Lv_Points[Combinded_Target_Lv_Points.size() - 1] - Target_Lv_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
					double Threshold_Lv = (Target_Lv_Points[(DP213_Static::Max_Gray_Amount * b) + g] * Combine_Lv_Ratio);

					if(Abs_Diff_Lv_Between_Two_Points > Threshold_Lv)
					{
						Combinded_Gamma_Red_Voltage_Points.push_back(Gamma_Red_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
						Combinded_Gamma_Green_Voltage_Points.push_back(Gamma_Green_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
						Combinded_Gamma_Blue_Voltage_Points.push_back(Gamma_Blue_Voltage_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
						Combinded_Target_Lv_Points.push_back(Target_Lv_Points[(DP213_Static::Max_Gray_Amount * b) + g]);
					}
				}
            }
        }
		//----------------------------------------------------
        //int Formula_Num = Points_Num - 2; //((band * 8) - 2 = Points_Num - 2) 
        int Formula_Num = Combinded_Target_Lv_Points.size() - 2; //Formula_Num = Points_Num - 2;
                
        double** Three_points_Gamma_Red_Voltage = M->MatrixCreate(Formula_Num, 3);
        double** Three_points_Gamma_Green_Voltage = M->MatrixCreate(Formula_Num, 3);
        double** Three_points_Gamma_Blue_Voltage = M->MatrixCreate(Formula_Num, 3);
        double** Three_points_Target_Lv = M->MatrixCreate(Formula_Num, 3);
        double** Three_points_C_R = M->MatrixCreate(Formula_Num, 3);
        double** Three_points_C_G = M->MatrixCreate(Formula_Num, 3);
        double** Three_points_C_B = M->MatrixCreate(Formula_Num, 3);

        double** Temp_A_R = M->MatrixCreate(3, 3);
        double** Temp_A_G = M->MatrixCreate(3, 3);
        double** Temp_A_B = M->MatrixCreate(3, 3);
        double** Temp_Inv_A_R = M->MatrixCreate(3, 3);
        double** Temp_Inv_A_G = M->MatrixCreate(3, 3);
        double** Temp_Inv_A_B = M->MatrixCreate(3, 3);

        double* C_R = new double[3];
        double* C_G = new double[3];
        double* C_B = new double[3];

        int count = 0;
        for (int k = 0; k < Formula_Num; k++)
        {
            for (int i = 0; i <= 2; i++)
            {
                //Get "Three_points_Target_Lv"
                Three_points_Target_Lv[k][i] = Combinded_Target_Lv_Points[i + k];
                count = 0;
                //Get "A"
                for (int j = 2; j >= 0; j--)
                {
                    Temp_A_R[i][count] = pow(Combinded_Gamma_Red_Voltage_Points[i + k], j);
                    Temp_A_G[i][count] = pow(Combinded_Gamma_Green_Voltage_Points[i + k], j);
                    Temp_A_B[i][count] = pow(Combinded_Gamma_Blue_Voltage_Points[i + k], j);
                    count++;
                }
            }

            //Get Inv_A (by using "A")
            Temp_Inv_A_R = M->MatrixInverse(Temp_A_R,3,3);
            Temp_Inv_A_G = M->MatrixInverse(Temp_A_G,3,3);
            Temp_Inv_A_B = M->MatrixInverse(Temp_A_B,3,3);

            //Get C (by using "Inv_A" , "Three_points_Target_Lv")
            Three_points_C_R[k] = M->Matrix_Multiply(Temp_Inv_A_R,3,3,Three_points_Target_Lv[k]);
            Three_points_C_G[k] = M->Matrix_Multiply(Temp_Inv_A_G,3,3,Three_points_Target_Lv[k]);
            Three_points_C_B[k] = M->Matrix_Multiply(Temp_Inv_A_B,3,3,Three_points_Target_Lv[k]);

            //Get C (Target belongs to [Three_points_Target_Lv[k][2],Three_points_Target_Lv[k][0]])
            if (Three_points_Target_Lv[k][2] < Target_Lv && Target_Lv < Three_points_Target_Lv[k][0])
            {
                for (int i = 0; i <= 2; i++)
                {
                    C_R[i] = Three_points_C_R[k][i];
                    C_G[i] = Three_points_C_G[k][i];
                    C_B[i] = Three_points_C_B[k][i];
                }
                break; //Break for k loop
            }
        }

        //Get Calculated Voltage_R/G/B
        double Calculated_Target_Lv;
        int iteration;
        double Calculated_R_Vdata = 0;
        double Calculated_G_Vdata = 0;
        double Calculated_B_Vdata = 0;

        for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;

            for (int j = 2; j >= 0; j--) 
				Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
            
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) 
			{ 
				Calculated_R_Vdata = Vdata; 
				break; 
			}
        }

        for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;

            for (int j = 2; j >= 0; j--) 
				Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
           
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0)
			{
				Calculated_G_Vdata = Vdata;
				break;
			}
        }

        for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= Voltage_VREF4095; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            
			for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            
			if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0)
			{
				Calculated_B_Vdata = Vdata; 
				break; 
			}
        }

        //Get Gamma_R/G/B From Calculated Voltage_R/G/B
        double Current_Band_Vreg1_Voltage = DP213_Get_Vreg1_Voltage(Voltage_VREF4095,Voltage_VREF0,Band_Vreg1_Dec[band]);
        Gamma_R = DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_R[band], Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata, gray);
        Gamma_G = DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_G[band], Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata, gray);
        Gamma_B = DP213_Get_GR_Gamma_Dec(Band_Voltage_AM1_B[band], Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata, gray);
	
		int Prev_Band_Current_Gray_Gamma_R = Band_Gray_Gamma_Red[(DP213_Static::Max_Gray_Amount * (band - 1)) + gray];
	    int Prev_Band_Current_Gray_Gamma_G = Band_Gray_Gamma_Green[(DP213_Static::Max_Gray_Amount * (band - 1)) + gray];
	    int Prev_Band_Current_Gray_Gamma_B = Band_Gray_Gamma_Blue[(DP213_Static::Max_Gray_Amount * (band - 1)) + gray];
        DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(Gamma_R,Gamma_G,Gamma_B,Prev_Band_Current_Gray_Gamma_R,Prev_Band_Current_Gray_Gamma_G,Prev_Band_Current_Gray_Gamma_B);

		delete M;
        delete[] Band_Gray_Gamma_Red_Voltage;
		delete[] Band_Gray_Gamma_Green_Voltage;
		delete[] Band_Gray_Gamma_Blue_Voltage;
		delete[] Gamma_Red_Voltage_Points;
		delete[] Gamma_Green_Voltage_Points; 
		delete[] Gamma_Blue_Voltage_Points;
		delete[] Target_Lv_Points;

		delete[] Three_points_Gamma_Red_Voltage;
		delete[] Three_points_Gamma_Green_Voltage;
		delete[] Three_points_Gamma_Blue_Voltage;
		delete[] Three_points_Target_Lv;
		delete[] Three_points_C_R;
		delete[] Three_points_C_G;
		delete[] Three_points_C_B;	

		delete[] Temp_A_R;
		delete[] Temp_A_G;
		delete[] Temp_A_B;
		delete[] Temp_Inv_A_R; 
		delete[] Temp_Inv_A_G;
		delete[] Temp_Inv_A_B;
		delete[] C_R;
		delete[] C_G;
		delete[] C_B;
	}
	else //Band0 + Other not selected Bands's
	{

	}
}

bool decrease(double a, double b){ return a > b; }

double* Get_Previous_Band_Gamma_Voltage(double Prev_Band_AM1_Voltage,int Previous_Band_Vreg1_Dec, int* Previous_Band_Gamma, double Voltage_VREF4095,double Voltage_VREF0)
{
	    double* Previous_Band_Gamma_Voltage = new double[DP213_Static::Max_Gray_Amount];
        double Previous_Band_Vreg1_Voltage = DP213_Get_Vreg1_Voltage(Voltage_VREF4095, Voltage_VREF0, Previous_Band_Vreg1_Dec);
        
		for (int gray = 0; gray < DP213_Static::Max_Gray_Amount; gray++)
        {
            if (gray == 0) 
				Previous_Band_Gamma_Voltage[gray] = DP213_Get_AM2_Gamma_Voltage(Voltage_VREF4095, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma[gray]);
            else if (gray == 1 || gray == 2 || gray == 3 || gray == 5 || gray == 7 || gray == 9 || gray == 10)
				Previous_Band_Gamma_Voltage[gray] = DP213_Get_GR_Gamma_Voltage(Prev_Band_AM1_Voltage, Previous_Band_Gamma_Voltage[gray - 1], Previous_Band_Gamma[gray],gray);
			else if (gray == 4 || gray == 6 || gray == 8)
				Previous_Band_Gamma_Voltage[gray] = DP213_Get_GR_Gamma_Voltage(Prev_Band_AM1_Voltage, Previous_Band_Gamma_Voltage[gray - 2], Previous_Band_Gamma[gray],gray);
        }                                                       
        return Previous_Band_Gamma_Voltage;
}


void DP213_If_One_Of_RGB_Near_Register_Min_Max_Set_Gamma_As_Prev_Band_Gamma(int& Gamma_R,int& Gamma_G,int& Gamma_B,int Prev_Band_Gray255_Gamma_R,int Prev_Band_Gray255_Gamma_G,int Prev_Band_Gray255_Gamma_B)
{
	if(Gamma_R >= 500 || Gamma_R <= 10 || Gamma_G >= 500 || Gamma_G <= 10 || Gamma_B >= 500 || Gamma_B <= 10)
	{
		Gamma_R = Prev_Band_Gray255_Gamma_R;
		Gamma_G = Prev_Band_Gray255_Gamma_G;
		Gamma_B = Prev_Band_Gray255_Gamma_B;
	}
}

int Convert_Toint32(double A)
{
	return (int)(A + 0.5);
}

//Get Vreg1/RGB Voltage From Gamma
extern "C" __declspec (dllexport ) double DP213_Get_Vreg1_Voltage(double Voltage_VREF4095,double Voltage_VREF0,int Dec_Vreg1)
{
	return Voltage_VREF4095 + (Voltage_VREF0 - Voltage_VREF4095) * (Dec_Vreg1/4095.0);
}

extern "C" __declspec (dllexport ) double DP213_Get_AM2_Gamma_Voltage(double Voltage_VREF4095, double Vreg1_voltage, int Dec_AM2)
{
	return Voltage_VREF4095 + (Vreg1_voltage - Voltage_VREF4095) * ((Dec_AM2 + 389) / 900.0);
}

extern "C" __declspec (dllexport ) double DP213_Get_AM1_RGB_Voltage(double Voltage_VREF4095,double Vreg1_voltage,int Dec_AM1)
{
	return Voltage_VREF4095 + (Vreg1_voltage - Voltage_VREF4095) * (((Dec_AM1 * 2) + 8) / 900.0);
}

extern "C" __declspec (dllexport ) double DP213_Get_AM0_RGB_Voltage(double Voltage_VREF4095,double Vreg1_voltage,int Dec_AM0)
{
	return Voltage_VREF4095 + (Vreg1_voltage - Voltage_VREF4095) * ((Dec_AM0* 2) / 900.0);
}

extern "C" __declspec (dllexport ) double DP213_Get_GR_Gamma_Voltage(double AM1_Voltage, double Prev_GR_Gamma_Voltage, int Gamma_Dec, int gray)
{
	//G191,G127(GR9,GR8)
	if((gray == 1) || (gray == 2))return AM1_Voltage + (Prev_GR_Gamma_Voltage - AM1_Voltage) * ((Gamma_Dec + 101) / 612.0);
	//G95,G63,G47,G31,G23,G15,G7,G1 (GR7,GR6,GR5,GR4,GR3,GR2,GR1,GR0)
	else return AM1_Voltage + (Prev_GR_Gamma_Voltage - AM1_Voltage) * ((Gamma_Dec + 1) / 512.0);

}

//Get Vreg1/RGB Gamma From Voltage
extern "C" __declspec (dllexport ) int DP213_Get_Vreg1_Dec(double Voltage_VREF4095,double Voltage_VREF0,double Vreg1_voltage)
{
	//Vreg1_Voltage = Voltage_VREF4095 + (Voltage_VREF0 - Voltage_VREF4095) * (Dec_Vreg1/4095.0);
	return Convert_Toint32(4095*((Vreg1_voltage - Voltage_VREF4095)/(Voltage_VREF0 - Voltage_VREF4095)));
}

extern "C" __declspec (dllexport ) int DP213_Get_AM2_Gamma_Dec(double Voltage_VREF4095, double Vreg1_voltage, double AM2_voltage)
{
	//AM2_voltage = Voltage_VREF4095 + (Vreg1_Voltage - Voltage_VREF4095) * ((Dec_AM2 + 389) / 900.0);
	return Convert_Toint32(900*(AM2_voltage - Voltage_VREF4095)/(Vreg1_voltage - Voltage_VREF4095) - 389);
}
extern "C" __declspec (dllexport ) int DP213_Get_AM1_RGB_Dec(double Voltage_VREF4095,double Vreg1_voltage,double AM1_Voltage)
{
	//AM1_Voltage = Voltage_VREF4095 + (Vreg1_voltage - Voltage_VREF4095) * (((Dec_AM1 * 2) + 8) / 900.0);
	return Convert_Toint32(((900.0 * ((AM1_Voltage - Voltage_VREF4095)/(Vreg1_voltage - Voltage_VREF4095))) - 8)/2.0);
}
extern "C" __declspec (dllexport ) int DP213_Get_AM0_RGB_Dec(double Voltage_VREF4095,double Vreg1_voltage,double AM0_Voltage)
{
	//AM0_Voltage = Voltage_VREF4095 + (Vreg1_voltage - Voltage_VREF4095) * ((Dec_AM0* 2) / 900.0);
	return Convert_Toint32((900.0*((AM0_Voltage - Voltage_VREF4095)/(Vreg1_voltage - Voltage_VREF4095)))/2.0);
}

extern "C" __declspec (dllexport ) int DP213_Get_GR_Gamma_Dec(double AM1_Voltage,double Prev_GR_Gamma_Voltage,double Gamma_Voltage,int gray)
{
	if((gray == 1) || (gray == 2))return Convert_Toint32(((Gamma_Voltage-AM1_Voltage)/(Prev_GR_Gamma_Voltage-AM1_Voltage)* (612.0)) - 101);
	else return Convert_Toint32(((Gamma_Voltage-AM1_Voltage)/(Prev_GR_Gamma_Voltage-AM1_Voltage)* (512.0)) - 1);
}

//ELVSS & Vinit2
extern "C" __declspec (dllexport ) double DP213_ELVSS_Dec_to_Voltage(int ELVSS_Dec) 
{
	return ((ELVSS_Dec - 30) / 10.0) - 3.1; 
}
extern "C" __declspec (dllexport ) int DP213_ELVSS_Voltage_to_Dec(double ELVSS_Voltage)
{ 
	return Convert_Toint32(10.0 * (ELVSS_Voltage + 3.1) + 30); 
}
extern "C" __declspec (dllexport ) double DP213_VINI2_Dec_to_Voltage(int VINIT2_Dec)
{ 
	return ((2 - VINIT2_Dec) / 10.0); 
}
extern "C" __declspec (dllexport ) int DP213_VINI2_Voltage_to_Dec(double VINIT2_Voltage) 
{ 
	return Convert_Toint32(2 - (10.0 * VINIT2_Voltage)); 
}

//VREF0
extern "C" __declspec (dllexport ) double DP213_VREF0_Dec_to_Voltage(int VREF0_Dec)
{ 
	return 0.2 + (VREF0_Dec * 0.04); 
}
extern "C" __declspec (dllexport ) int DP213_VREF0_Voltage_to_Dec(double VREF0_Voltage)
{ 
	return Convert_Toint32((VREF0_Voltage - 0.2) / 0.04); 
}

//VREF4095
extern "C" __declspec (dllexport ) double DP213_VREF4095_Dec_to_Voltage(int VREF4095_Dec)
{ 
	return 1.92 + (VREF4095_Dec * 0.04);
}
extern "C" __declspec (dllexport ) int DP213_VREF4095_Voltage_to_Dec(double VREF4095_Voltage)
{ 
	return Convert_Toint32((VREF4095_Voltage - 1.92) / 0.04);
}


//AM0 Resolution
extern "C" __declspec (dllexport ) double Get_DP213_EA9155_AM0_Resolution(int Dec_REF0,int Dev_REF4095)
{
	double Voltage_REF0 = DP213_VREF0_Dec_to_Voltage(Dec_REF0); 
	double Voltage_REF4095 = DP213_VREF4095_Dec_to_Voltage(Dev_REF4095);

	return  (Voltage_REF4095 - Voltage_REF0)/(900.0 / 2.0);
}