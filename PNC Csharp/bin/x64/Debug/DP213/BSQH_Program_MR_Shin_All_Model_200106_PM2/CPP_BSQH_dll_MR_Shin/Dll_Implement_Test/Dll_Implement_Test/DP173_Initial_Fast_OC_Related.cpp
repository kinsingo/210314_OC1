
#include "stdafx.h"
#include "DP173_Initial_Fast_OC_Related.h"
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

double* Get_Previous_Band_Gamma_Voltage(int Dec_AM1,int Previous_Band_Vreg1_Dec, int* Previous_Band_Gamma, double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1)
{
	    double* Previous_Band_Gamma_Voltage = new double[8];
        double Previous_Band_Vreg1_Voltage = DP173_Get_Vreg1_Voltage(Previous_Band_Vreg1_Dec,Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);
        
        for (int gray = 0; gray < 8; gray++)
        {
            if (gray == 0) Previous_Band_Gamma_Voltage[gray] = DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma[gray]);
            else Previous_Band_Gamma_Voltage[gray] = DP173_Get_GR_Gamma_Voltage(Voltage_VREG1_REF2047,Previous_Band_Vreg1_Voltage,Dec_AM1,Previous_Band_Gamma_Voltage[gray - 1], Previous_Band_Gamma[gray],gray);
        }                                                       
        return Previous_Band_Gamma_Voltage;
}

//Calculate and Get Initial R/G/B (Gray255)
extern "C" __declspec (dllexport )void DP173_Gray255_Get_Intial_R_G_B_Using_Previous_Band_Method(int Vreg1,int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B,int& Gamma_R,int& Gamma_G,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Target_Lv, double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1)
{
	if (band >= 1 && Selected_Band[band] == true)
    {
		SJH_Matrix *M = new SJH_Matrix();

        double* Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_R,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);
        double* Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_G,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);
        double* Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_B,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);
        
        double** A_R = M->MatrixCreate(8, 8);
        double** A_G = M->MatrixCreate(8, 8);
        double** A_B = M->MatrixCreate(8, 8);

        //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
        int count = 0;
        for (int i = 0; i <= 7; i++)
        {
            count = 0;
            for (int j = 7; j >= 0; j--)
            {
                A_R[i][count] = pow(Previous_Band_Gamma_Red_Voltage[i], j);
                A_G[i][count] = pow(Previous_Band_Gamma_Green_Voltage[i], j);
                A_B[i][count] = pow(Previous_Band_Gamma_Blue_Voltage[i], j);
                count++;
            }
        }

        //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
        double** Inv_A_R = M->MatrixCreate(8, 8);
        double** Inv_A_G = M->MatrixCreate(8, 8);
        double** Inv_A_B = M->MatrixCreate(8, 8);
        double* C_R = new double[8];
        double* C_G = new double[8];
        double* C_B = new double[8];
        Inv_A_R = M->MatrixInverse(A_R,8,8);
        Inv_A_G = M->MatrixInverse(A_G,8,8);
        Inv_A_B = M->MatrixInverse(A_B,8,8);
        C_R = M->Matrix_Multiply(Inv_A_R,8,8, Previous_Band_Target_Lv);
        C_G = M->Matrix_Multiply(Inv_A_G,8,8, Previous_Band_Target_Lv);
        C_B = M->Matrix_Multiply(Inv_A_B,8,8, Previous_Band_Target_Lv);

        //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
        //Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
        //f1.GB_Status_AppendText_Nextline("C_G[0] , C_G[7] = " + C_G[0].ToString() + " , " + C_G[7].ToString(), Color.Blue);//Just For Debug, it can be deleted later (191113)
        double Target_Lv = band_Target_Lv;
        double Calculated_Vdata_Red = 0;
        double Calculated_Vdata_Green = 0;
        double Calculated_Vdata_Blue = 0;
                
        double Calculated_Target_Lv = 0;
        int iteration;

        //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
        //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
        int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
        double Previous_Band_Vreg1_Voltage = DP173_Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);

        double Actual_Previous_Vdata_Red = DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
        double Actual_Previous_Vdata_Green = DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
        double Actual_Previous_Vdata_Blue = DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);
                
        //Red
        for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0) Calculated_Vdata_Red = Vdata;
        }

        //Green
        for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
        }

        //Blue
        for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0) Calculated_Vdata_Blue = Vdata;
        }

		double Vreg1_voltage = DP173_Get_Vreg1_Voltage(Vreg1, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);//Verify OK (200205)
                         
        //Got the Vreg1 
        //Need to get Gamma_R/B
	    Gamma_R = DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Red);
		Gamma_G = DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Green);
        Gamma_B = DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Blue);

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


//Calculate to get R/Vreg1/B
extern "C" __declspec (dllexport )void DP173_Gray255_Get_Intial_R_Vreg1_B_Using_Previous_Band_Method(int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B,int& Vreg1_Dec_Init,int& Gamma_R,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Target_Lv, double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1)
{
	if (band >= 1 && Selected_Band[band] == true)
    {
		SJH_Matrix *M = new SJH_Matrix();

        double* Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_R,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);
        double* Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_G,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);
        double* Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Voltage(Dec_AM1_B,Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);
        
        double** A_R = M->MatrixCreate(8, 8);
        double** A_G = M->MatrixCreate(8, 8);
        double** A_B = M->MatrixCreate(8, 8);

        //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
        int count = 0;
        for (int i = 0; i <= 7; i++)
        {
            count = 0;
            for (int j = 7; j >= 0; j--)
            {
                A_R[i][count] = pow(Previous_Band_Gamma_Red_Voltage[i], j);
                A_G[i][count] = pow(Previous_Band_Gamma_Green_Voltage[i], j);
                A_B[i][count] = pow(Previous_Band_Gamma_Blue_Voltage[i], j);
                count++;
            }
        }

        //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
        double** Inv_A_R = M->MatrixCreate(8, 8);
        double** Inv_A_G = M->MatrixCreate(8, 8);
        double** Inv_A_B = M->MatrixCreate(8, 8);
        double* C_R = new double[8];
        double* C_G = new double[8];
        double* C_B = new double[8];
        Inv_A_R = M->MatrixInverse(A_R,8,8);
        Inv_A_G = M->MatrixInverse(A_G,8,8);
        Inv_A_B = M->MatrixInverse(A_B,8,8);
        C_R = M->Matrix_Multiply(Inv_A_R,8,8, Previous_Band_Target_Lv);
        C_G = M->Matrix_Multiply(Inv_A_G,8,8, Previous_Band_Target_Lv);
        C_B = M->Matrix_Multiply(Inv_A_B,8,8, Previous_Band_Target_Lv);

        //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
        //Form1 f1 = (Form1)Application.OpenForms["Form1"];//Just For Debug , it can be deleted later (191113)
        //f1.GB_Status_AppendText_Nextline("C_G[0] , C_G[7] = " + C_G[0].ToString() + " , " + C_G[7].ToString(), Color.Blue);//Just For Debug, it can be deleted later (191113)
        double Target_Lv = band_Target_Lv;
        double Calculated_Vdata_Red = 0;
        double Calculated_Vdata_Green = 0;
        double Calculated_Vdata_Blue = 0;
                
        double Calculated_Target_Lv = 0;
        int iteration;

        //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
        //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
        int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
        double Previous_Band_Vreg1_Voltage = DP173_Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);

        double Actual_Previous_Vdata_Red = DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
        double Actual_Previous_Vdata_Green = DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
        double Actual_Previous_Vdata_Blue = DP173_Get_AM2_Gamma_Voltage(Voltage_VREG1_REF2047, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);
                
        //Red
        for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0) Calculated_Vdata_Red = Vdata;
        }

        //Green
        for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
        }

        //Blue
        for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0) Calculated_Vdata_Blue = Vdata;
        }

        double Vreg1_voltage = Voltage_VREG1_REF2047 + ((Calculated_Vdata_Green - Voltage_VREG1_REF2047) * (700.0 / (Previous_Band_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
        Vreg1_Dec_Init = DP173_Get_Vreg1_Dec(Vreg1_voltage,Voltage_VREG1_REF2047,Voltage_VREG1_REF1635,Voltage_VREG1_REF1227,Voltage_VREG1_REF815,Voltage_VREG1_REF407,Voltage_VREG1_REF63,Voltage_VREG1_REF1);
                         
        //Got the Vreg1 
        //Need to get Gamma_R/B
	    Gamma_R = DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Red);
        Gamma_B = DP173_Get_AM2_Gamma_Dec(Voltage_VREG1_REF2047, Vreg1_voltage, Calculated_Vdata_Blue);

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
extern "C" __declspec (dllexport )  void Get_Initial_Gamma_Fx_3points_Combine_Points(int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Band_Gray_Gamma_Red, int* Band_Gray_Gamma_Green, int* Band_Gray_Gamma_Blue, double* Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int* Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1
            , double Fx_3points_Combine_LV_1, double Fx_3points_Combine_LV_2, double Fx_3points_Combine_LV_3, double Fx_3points_Combine_Lv_Distance)
{
    if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
    {
		SJH_Matrix *M = new SJH_Matrix();

        double** Band_Gray_Gamma_Red_Voltage = M->MatrixCreate(band, 8);
        double** Band_Gray_Gamma_Green_Voltage = M->MatrixCreate(band, 8);
        double** Band_Gray_Gamma_Blue_Voltage = M->MatrixCreate(band, 8);

        for (int i = 0; i < band; i++)
        {
			int* Previous_Band_Gamma_R = new int[8];
			int* Previous_Band_Gamma_G = new int[8];
			int* Previous_Band_Gamma_B = new int[8];
			
			for(int g = 0;g <8; g++)
			{
				Previous_Band_Gamma_R[g] = Band_Gray_Gamma_Red[(8 * i) + g];
				Previous_Band_Gamma_G[g] = Band_Gray_Gamma_Green[(8 * i) + g];
				Previous_Band_Gamma_B[g] = Band_Gray_Gamma_Blue[(8 * i) + g];
			}
            //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
            Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_R, Band_Vreg1_Dec[i], Previous_Band_Gamma_R, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
            Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_G, Band_Vreg1_Dec[i], Previous_Band_Gamma_G, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
            Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_B, Band_Vreg1_Dec[i], Previous_Band_Gamma_B, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
            
			delete[] Previous_Band_Gamma_R; 
			delete[] Previous_Band_Gamma_G;
			delete[] Previous_Band_Gamma_B;
		}

                
        //Need to...
        //Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance
        int Points_Num = band * 8;
        double* Gamma_Red_Voltage_Points = new double[Points_Num];
        double* Gamma_Green_Voltage_Points = new double[Points_Num];
        double* Gamma_Blue_Voltage_Points = new double[Points_Num];
        double* Target_Lv_Points = new double[Points_Num];

        for (int b = 0; b < band; b++)
        {
            for (int g = 0; g < 8; g++)
            {
                Gamma_Red_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Red_Voltage[b][g];
                Gamma_Green_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Green_Voltage[b][g];
                Gamma_Blue_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Blue_Voltage[b][g];
                Target_Lv_Points[(8 * b) + g] = Band_Gray_Target_Lv[(8 * b) + g];
            }
        }
		std::sort(Gamma_Red_Voltage_Points, Gamma_Red_Voltage_Points + Points_Num);
		std::sort(Gamma_Green_Voltage_Points, Gamma_Green_Voltage_Points + Points_Num);
		std::sort(Gamma_Blue_Voltage_Points, Gamma_Blue_Voltage_Points + Points_Num);
		std::sort(Target_Lv_Points, Target_Lv_Points + Points_Num, desc);
				
        std::vector<double> Gamma_Red_Voltage_Points_Rearrange;
        std::vector<double> Temp_R_1;
        std::vector<double> Temp_R_2;
		std::vector<double> Temp_R_3;

        std::vector<double> Gamma_Green_Voltage_Points_Rearrange;
        std::vector<double> Temp_G_1;
        std::vector<double> Temp_G_2;
        std::vector<double> Temp_G_3;

        std::vector<double> Gamma_Blue_Voltage_Points_Rearrange;
        std::vector<double> Temp_B_1;
        std::vector<double> Temp_B_2;
        std::vector<double> Temp_B_3;

        std::vector<double> Target_Lv_Points_Rearrange;
        std::vector<double> Temp_Target_Lv_1;
        std::vector<double> Temp_Target_Lv_2;
        std::vector<double> Temp_Target_Lv_3;

        double Diff_Lv = 0;
        bool flag = false;
        for (int b = 0; b < band; b++)
        {
            for (int g = 0; g < 8; g++)
            {
                if (flag)
                {
                    g++;
                    flag = false;
                }
                //Lv
                // X < A (Region 1)
                if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_1)
                {
                    Temp_R_1.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Temp_G_1.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Temp_B_1.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Temp_Target_Lv_1.push_back(Target_Lv_Points[(8 * b) + g]);
                }
                // A <= X < B (Region 2)
                else if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_2)
                {
                    Temp_R_2.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Temp_G_2.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Temp_B_2.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Temp_Target_Lv_2.push_back(Target_Lv_Points[(8 * b) + g]);
                }
                // B <= X < C (Region 3)
                else if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_3)
                {
                    Temp_R_3.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Temp_G_3.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Temp_B_3.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Temp_Target_Lv_3.push_back(Target_Lv_Points[(8 * b) + g]);

                }
                // C <= X (Region 4)
                else
                {
                    Gamma_Red_Voltage_Points_Rearrange.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Gamma_Green_Voltage_Points_Rearrange.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Gamma_Blue_Voltage_Points_Rearrange.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Target_Lv_Points_Rearrange.push_back(Target_Lv_Points[(8 * b) + g]);

                    if (((8 * b) + g) < ((8 * (band - 1)) + (8 - 1))) //if it's not the last point
                    {
                        Diff_Lv = abs(Target_Lv_Points[(8 * b) + g] - Target_Lv_Points[(8 * b) + (g + 1)]);
                        if (Diff_Lv < Fx_3points_Combine_Lv_Distance)
                        {
                            if (g == 7)
                            {
                                flag = true;
                            }
                            else
                            {
                                g++;
                                flag = false;
                            }
                        }
                    }
                }
            }
        }
        if (Temp_Target_Lv_3.size() != 0)
        {
			double R_Sum = 0;
			double G_Sum = 0;
			double B_Sum = 0;
			double Lv_Target_Sum = 0;
			for(int i=0;i<Temp_Target_Lv_3.size();i++)
			{
				R_Sum += Temp_R_3[i];
				G_Sum += Temp_G_3[i];
				B_Sum += Temp_B_3[i];
			    Lv_Target_Sum += Temp_Target_Lv_3[i];
			}
            Gamma_Red_Voltage_Points_Rearrange.push_back((double)(R_Sum / Temp_Target_Lv_3.size()));
            Gamma_Green_Voltage_Points_Rearrange.push_back((double)(G_Sum / Temp_Target_Lv_3.size()));
            Gamma_Blue_Voltage_Points_Rearrange.push_back((double)(B_Sum / Temp_Target_Lv_3.size()));
            Target_Lv_Points_Rearrange.push_back((double)(Lv_Target_Sum / Temp_Target_Lv_3.size()));
        }
        if (Temp_Target_Lv_2.size() != 0)
        {
			double R_Sum = 0;
			double G_Sum = 0;
			double B_Sum = 0;
			double Lv_Target_Sum = 0;
			for(int i=0;i<Temp_Target_Lv_2.size();i++)
			{
				R_Sum += Temp_R_2[i];
				G_Sum += Temp_G_2[i];
				B_Sum += Temp_B_2[i];
			    Lv_Target_Sum += Temp_Target_Lv_2[i];
			}

            Gamma_Red_Voltage_Points_Rearrange.push_back((double)(R_Sum / Temp_Target_Lv_2.size()));
            Gamma_Green_Voltage_Points_Rearrange.push_back((double)(G_Sum / Temp_Target_Lv_2.size()));
            Gamma_Blue_Voltage_Points_Rearrange.push_back((double)(B_Sum / Temp_Target_Lv_2.size()));
            Target_Lv_Points_Rearrange.push_back((double)(Lv_Target_Sum / Temp_Target_Lv_2.size()));
        }

        if (Temp_Target_Lv_1.size() != 0)
        {
			double R_Sum = 0;
			double G_Sum = 0;
			double B_Sum = 0;
			double Lv_Target_Sum = 0;
			for(int i=0;i<Temp_Target_Lv_1.size();i++)
			{
				R_Sum += Temp_R_1[i];
				G_Sum += Temp_G_1[i];
				B_Sum += Temp_B_1[i];
				Lv_Target_Sum += Temp_Target_Lv_1[i];
			}

            Gamma_Red_Voltage_Points_Rearrange.push_back((double)(R_Sum / Temp_Target_Lv_1.size()));
            Gamma_Green_Voltage_Points_Rearrange.push_back((double)(G_Sum / Temp_Target_Lv_1.size()));
            Gamma_Blue_Voltage_Points_Rearrange.push_back((double)(B_Sum / Temp_Target_Lv_1.size()));
            Target_Lv_Points_Rearrange.push_back((double)(Lv_Target_Sum / Temp_Target_Lv_1.size()));
        }

        //int Formula_Num = Points_Num - 2; //((band * 8) - 2 = Points_Num - 2) 
        int Formula_Num = Target_Lv_Points_Rearrange.size() - 2; //Formula_Num = Points_Num - 2;
                
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
                Three_points_Target_Lv[k][i] = Target_Lv_Points_Rearrange[i + k];
                count = 0;
                //Get "A"
                for (int j = 2; j >= 0; j--)
                {
                    Temp_A_R[i][count] = pow(Gamma_Red_Voltage_Points_Rearrange[i + k], j);
                    Temp_A_G[i][count] = pow(Gamma_Green_Voltage_Points_Rearrange[i + k], j);
                    Temp_A_B[i][count] = pow(Gamma_Blue_Voltage_Points_Rearrange[i + k], j);
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

        for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
        }

        for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0){Calculated_G_Vdata = Vdata; break;}
        }

        for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
        }

        //Get Gamma_R/G/B From Calculated Voltage_R/G/B
        double Current_Band_Vreg1_Voltage = DP173_Get_Vreg1_Voltage(Band_Vreg1_Dec[band], Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
        Gamma_R = DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_R, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata, gray);
        Gamma_G = DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_G, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata, gray);
        Gamma_B = DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_B, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata, gray);
	
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
}



//Calculate to get R/G/B
extern "C" __declspec (dllexport )  void Get_Initial_Gamma_Fx_3points_Combine_Points_2(double Combine_Lv_Ratio,int Dec_AM1_R, int Dec_AM1_G, int Dec_AM1_B, int& Gamma_R, int& Gamma_G, int& Gamma_B, bool* Selected_Band, int* Band_Gray_Gamma_Red, int* Band_Gray_Gamma_Green, int* Band_Gray_Gamma_Blue, double* Band_Gray_Target_Lv, int Current_Band_Dec_Vreg1,
            int* Band_Vreg1_Dec, int band, int gray, double Target_Lv, double Prvious_Gray_Gamma_R_Voltage, double Prvious_Gray_Gamma_G_Voltage, double Prvious_Gray_Gamma_B_Voltage,
            double Voltage_VREG1_REF2047, double Voltage_VREG1_REF1635, double Voltage_VREG1_REF1227, double Voltage_VREG1_REF815, double Voltage_VREG1_REF407, double Voltage_VREG1_REF63, double Voltage_VREG1_REF1
            , double Fx_3points_Combine_LV_1, double Fx_3points_Combine_LV_2, double Fx_3points_Combine_LV_3, double Fx_3points_Combine_Lv_Distance)
{
    if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
    {
		SJH_Matrix *M = new SJH_Matrix();

        double** Band_Gray_Gamma_Red_Voltage = M->MatrixCreate(band, 8);
        double** Band_Gray_Gamma_Green_Voltage = M->MatrixCreate(band, 8);
        double** Band_Gray_Gamma_Blue_Voltage = M->MatrixCreate(band, 8);

        for (int i = 0; i < band; i++)
        {
			int* Previous_Band_Gamma_R = new int[8];
			int* Previous_Band_Gamma_G = new int[8];
			int* Previous_Band_Gamma_B = new int[8];
			
			for(int g = 0;g <8; g++)
			{
				Previous_Band_Gamma_R[g] = Band_Gray_Gamma_Red[(8 * i) + g];
				Previous_Band_Gamma_G[g] = Band_Gray_Gamma_Green[(8 * i) + g];
				Previous_Band_Gamma_B[g] = Band_Gray_Gamma_Blue[(8 * i) + g];
			}
            //double[][] Band_Gray_Gamma_Red_Voltage <- (int[] Band_Vreg1_Dec , int[][] Band_Gray_Gamma_Red)
            Band_Gray_Gamma_Red_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_R, Band_Vreg1_Dec[i], Previous_Band_Gamma_R, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
            Band_Gray_Gamma_Green_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_G, Band_Vreg1_Dec[i], Previous_Band_Gamma_G, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
            Band_Gray_Gamma_Blue_Voltage[i] = Get_Previous_Band_Gamma_Voltage(Dec_AM1_B, Band_Vreg1_Dec[i], Previous_Band_Gamma_B, Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
            
			delete[] Previous_Band_Gamma_R; 
			delete[] Previous_Band_Gamma_G;
			delete[] Previous_Band_Gamma_B;
		}

                
        //Need to...
        //Fx_3points_Combine_LV_1, Fx_3points_Combine_LV_2, Fx_3points_Combine_LV_3, Fx_3points_Combine_Lv_Distance
        int Points_Num = band * 8;
        double* Gamma_Red_Voltage_Points = new double[Points_Num];
        double* Gamma_Green_Voltage_Points = new double[Points_Num];
        double* Gamma_Blue_Voltage_Points = new double[Points_Num];
        double* Target_Lv_Points = new double[Points_Num];

        for (int b = 0; b < band; b++)
        {
            for (int g = 0; g < 8; g++)
            {
                Gamma_Red_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Red_Voltage[b][g];
                Gamma_Green_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Green_Voltage[b][g];
                Gamma_Blue_Voltage_Points[(8 * b) + g] = Band_Gray_Gamma_Blue_Voltage[b][g];
                Target_Lv_Points[(8 * b) + g] = Band_Gray_Target_Lv[(8 * b) + g];
            }
        }
		std::sort(Gamma_Red_Voltage_Points, Gamma_Red_Voltage_Points + Points_Num);
		std::sort(Gamma_Green_Voltage_Points, Gamma_Green_Voltage_Points + Points_Num);
		std::sort(Gamma_Blue_Voltage_Points, Gamma_Blue_Voltage_Points + Points_Num);
		std::sort(Target_Lv_Points, Target_Lv_Points + Points_Num, desc);
				
		//-----------------Added On 200311-------------------
		std::vector<double> Combinded_Gamma_Red_Voltage_Points;
        std::vector<double> Combinded_Gamma_Green_Voltage_Points;
        std::vector<double> Combinded_Gamma_Blue_Voltage_Points;
		std::vector<double> Combinded_Target_Lv_Points;

		for (int b = 0; b < band; b++)
        {
            for (int g = 0; g < 8; g++)
            {
                if(b == 0 && g == 0)//First Point
				{
					Combinded_Gamma_Red_Voltage_Points.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Combinded_Gamma_Green_Voltage_Points.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Combinded_Gamma_Blue_Voltage_Points.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Combinded_Target_Lv_Points.push_back(Target_Lv_Points[(8 * b) + g]);
				}
				else
				{
					double Abs_Diff_Lv_Between_Two_Points = abs(Combinded_Target_Lv_Points[Combinded_Target_Lv_Points.size() - 1] - Target_Lv_Points[(8 * b) + g]);
					double Threshold_Lv = (Target_Lv_Points[(8 * b) + g] * Combine_Lv_Ratio);

					if(Abs_Diff_Lv_Between_Two_Points > Threshold_Lv)
					{
						Combinded_Gamma_Red_Voltage_Points.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
						Combinded_Gamma_Green_Voltage_Points.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
						Combinded_Gamma_Blue_Voltage_Points.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
						Combinded_Target_Lv_Points.push_back(Target_Lv_Points[(8 * b) + g]);
					}
				}
            }
        }
		//----------------------------------------------------
        std::vector<double> Gamma_Red_Voltage_Points_Rearrange;
        std::vector<double> Temp_R_1;
        std::vector<double> Temp_R_2;
		std::vector<double> Temp_R_3;

        std::vector<double> Gamma_Green_Voltage_Points_Rearrange;
        std::vector<double> Temp_G_1;
        std::vector<double> Temp_G_2;
        std::vector<double> Temp_G_3;

        std::vector<double> Gamma_Blue_Voltage_Points_Rearrange;
        std::vector<double> Temp_B_1;
        std::vector<double> Temp_B_2;
        std::vector<double> Temp_B_3;

        std::vector<double> Target_Lv_Points_Rearrange;
        std::vector<double> Temp_Target_Lv_1;
        std::vector<double> Temp_Target_Lv_2;
        std::vector<double> Temp_Target_Lv_3;

        for (int points = 0; points < Combinded_Target_Lv_Points.size(); points++)
		{
			//Lv
			// X < A (Region 1)
			if (Combinded_Target_Lv_Points[points] < Fx_3points_Combine_LV_1)
			{
				Temp_R_1.push_back(Combinded_Gamma_Red_Voltage_Points[points]);
				Temp_G_1.push_back(Combinded_Gamma_Green_Voltage_Points[points]);
				Temp_B_1.push_back(Combinded_Gamma_Blue_Voltage_Points[points]);
				Temp_Target_Lv_1.push_back(Combinded_Target_Lv_Points[points]);
			}
			// A <= X < B (Region 2)
			else if (Combinded_Target_Lv_Points[points] < Fx_3points_Combine_LV_2)
			{
				Temp_R_2.push_back(Combinded_Gamma_Red_Voltage_Points[points]);
				Temp_G_2.push_back(Combinded_Gamma_Green_Voltage_Points[points]);
				Temp_B_2.push_back(Combinded_Gamma_Blue_Voltage_Points[points]);
				Temp_Target_Lv_2.push_back(Combinded_Target_Lv_Points[points]);
			}
			// B <= X < C (Region 3)
			else if (Combinded_Target_Lv_Points[points] < Fx_3points_Combine_LV_3)
			{
				Temp_R_3.push_back(Combinded_Gamma_Red_Voltage_Points[points]);
				Temp_G_3.push_back(Combinded_Gamma_Green_Voltage_Points[points]);
				Temp_B_3.push_back(Combinded_Gamma_Blue_Voltage_Points[points]);
				Temp_Target_Lv_3.push_back(Combinded_Target_Lv_Points[points]);

			}
			// C <= X (Region 4)
			else
			{
				Gamma_Red_Voltage_Points_Rearrange.push_back(Combinded_Gamma_Red_Voltage_Points[points]);
				Gamma_Green_Voltage_Points_Rearrange.push_back(Combinded_Gamma_Green_Voltage_Points[points]);
				Gamma_Blue_Voltage_Points_Rearrange.push_back(Combinded_Gamma_Blue_Voltage_Points[points]);
				Target_Lv_Points_Rearrange.push_back(Combinded_Target_Lv_Points[points]);
			}
		}


        


		/*
		double Diff_Lv = 0;
        bool flag = false;
        for (int b = 0; b < band; b++)
        {
            for (int g = 0; g < 8; g++)
            {
                if (flag)
                {
                    g++;
                    flag = false;
                }
                //Lv
                // X < A (Region 1)
                if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_1)
                {
                    Temp_R_1.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Temp_G_1.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Temp_B_1.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Temp_Target_Lv_1.push_back(Target_Lv_Points[(8 * b) + g]);
                }
                // A <= X < B (Region 2)
                else if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_2)
                {
                    Temp_R_2.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Temp_G_2.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Temp_B_2.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Temp_Target_Lv_2.push_back(Target_Lv_Points[(8 * b) + g]);
                }
                // B <= X < C (Region 3)
                else if (Target_Lv_Points[(8 * b) + g] < Fx_3points_Combine_LV_3)
                {
                    Temp_R_3.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Temp_G_3.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Temp_B_3.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Temp_Target_Lv_3.push_back(Target_Lv_Points[(8 * b) + g]);

                }
                // C <= X (Region 4)
                else
                {
                    Gamma_Red_Voltage_Points_Rearrange.push_back(Gamma_Red_Voltage_Points[(8 * b) + g]);
                    Gamma_Green_Voltage_Points_Rearrange.push_back(Gamma_Green_Voltage_Points[(8 * b) + g]);
                    Gamma_Blue_Voltage_Points_Rearrange.push_back(Gamma_Blue_Voltage_Points[(8 * b) + g]);
                    Target_Lv_Points_Rearrange.push_back(Target_Lv_Points[(8 * b) + g]);

                    if (((8 * b) + g) < ((8 * (band - 1)) + (8 - 1))) //if it's not the last point
                    {
                        Diff_Lv = abs(Target_Lv_Points[(8 * b) + g] - Target_Lv_Points[(8 * b) + (g + 1)]);
                        if (Diff_Lv < Fx_3points_Combine_Lv_Distance)
                        {
                            if (g == 7)
                            {
                                flag = true;
                            }
                            else
                            {
                                g++;
                                flag = false;
                            }
                        }
                    }
                }
            }
        }
		*/

        if (Temp_Target_Lv_3.size() != 0)
        {
			double R_Sum = 0;
			double G_Sum = 0;
			double B_Sum = 0;
			double Lv_Target_Sum = 0;
			for(int i=0;i<Temp_Target_Lv_3.size();i++)
			{
				R_Sum += Temp_R_3[i];
				G_Sum += Temp_G_3[i];
				B_Sum += Temp_B_3[i];
			    Lv_Target_Sum += Temp_Target_Lv_3[i];
			}
            Gamma_Red_Voltage_Points_Rearrange.push_back((double)(R_Sum / Temp_Target_Lv_3.size()));
            Gamma_Green_Voltage_Points_Rearrange.push_back((double)(G_Sum / Temp_Target_Lv_3.size()));
            Gamma_Blue_Voltage_Points_Rearrange.push_back((double)(B_Sum / Temp_Target_Lv_3.size()));
            Target_Lv_Points_Rearrange.push_back((double)(Lv_Target_Sum / Temp_Target_Lv_3.size()));
        }
        if (Temp_Target_Lv_2.size() != 0)
        {
			double R_Sum = 0;
			double G_Sum = 0;
			double B_Sum = 0;
			double Lv_Target_Sum = 0;
			for(int i=0;i<Temp_Target_Lv_2.size();i++)
			{
				R_Sum += Temp_R_2[i];
				G_Sum += Temp_G_2[i];
				B_Sum += Temp_B_2[i];
			    Lv_Target_Sum += Temp_Target_Lv_2[i];
			}

            Gamma_Red_Voltage_Points_Rearrange.push_back((double)(R_Sum / Temp_Target_Lv_2.size()));
            Gamma_Green_Voltage_Points_Rearrange.push_back((double)(G_Sum / Temp_Target_Lv_2.size()));
            Gamma_Blue_Voltage_Points_Rearrange.push_back((double)(B_Sum / Temp_Target_Lv_2.size()));
            Target_Lv_Points_Rearrange.push_back((double)(Lv_Target_Sum / Temp_Target_Lv_2.size()));
        }

        if (Temp_Target_Lv_1.size() != 0)
        {
			double R_Sum = 0;
			double G_Sum = 0;
			double B_Sum = 0;
			double Lv_Target_Sum = 0;
			for(int i=0;i<Temp_Target_Lv_1.size();i++)
			{
				R_Sum += Temp_R_1[i];
				G_Sum += Temp_G_1[i];
				B_Sum += Temp_B_1[i];
				Lv_Target_Sum += Temp_Target_Lv_1[i];
			}

            Gamma_Red_Voltage_Points_Rearrange.push_back((double)(R_Sum / Temp_Target_Lv_1.size()));
            Gamma_Green_Voltage_Points_Rearrange.push_back((double)(G_Sum / Temp_Target_Lv_1.size()));
            Gamma_Blue_Voltage_Points_Rearrange.push_back((double)(B_Sum / Temp_Target_Lv_1.size()));
            Target_Lv_Points_Rearrange.push_back((double)(Lv_Target_Sum / Temp_Target_Lv_1.size()));
        }
		

        //int Formula_Num = Points_Num - 2; //((band * 8) - 2 = Points_Num - 2) 
        int Formula_Num = Target_Lv_Points_Rearrange.size() - 2; //Formula_Num = Points_Num - 2;
                
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
                Three_points_Target_Lv[k][i] = Target_Lv_Points_Rearrange[i + k];
                count = 0;
                //Get "A"
                for (int j = 2; j >= 0; j--)
                {
                    Temp_A_R[i][count] = pow(Gamma_Red_Voltage_Points_Rearrange[i + k], j);
                    Temp_A_G[i][count] = pow(Gamma_Green_Voltage_Points_Rearrange[i + k], j);
                    Temp_A_B[i][count] = pow(Gamma_Blue_Voltage_Points_Rearrange[i + k], j);
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

        for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
        }

        for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0){Calculated_G_Vdata = Vdata; break;}
        }

        for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= Voltage_VREG1_REF2047; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 2; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
        }

        //Get Gamma_R/G/B From Calculated Voltage_R/G/B
        double Current_Band_Vreg1_Voltage = DP173_Get_Vreg1_Voltage(Band_Vreg1_Dec[band], Voltage_VREG1_REF2047, Voltage_VREG1_REF1635, Voltage_VREG1_REF1227, Voltage_VREG1_REF815, Voltage_VREG1_REF407, Voltage_VREG1_REF63, Voltage_VREG1_REF1);
        Gamma_R = DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_R, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata, gray);
        Gamma_G = DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_G, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata, gray);
        Gamma_B = DP173_Get_GR_Gamma_Dec(Voltage_VREG1_REF2047, Current_Band_Vreg1_Voltage, Dec_AM1_B, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata, gray);
	
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
}


//Get REF Voltages
extern "C" __declspec (dllexport ) void Get_REF_Voltages(int Dec_VREG1_REF2047,int Dec_VREG1_REF1635,int Dec_VREG1_REF1227,int Dec_VREG1_REF815,int Dec_VREG1_REF407,int Dec_VREG1_REF63,int Dec_VREG1_REF1
	,double& Voltage_VREG1_REF2047,double& Voltage_VREG1_REF1635,double& Voltage_VREG1_REF1227,double& Voltage_VREG1_REF815,double& Voltage_VREG1_REF407,double& Voltage_VREG1_REF63,double& Voltage_VREG1_REF1)
{
	  Voltage_VREG1_REF1 = 0.2 + (0.04 * Dec_VREG1_REF1); //Min 0.2v , Max 5.28
      Voltage_VREG1_REF2047 = 1.92 + (0.04 * Dec_VREG1_REF2047); //Min 1.92v , Max 7v
      Voltage_VREG1_REF1635 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (222.5 + (0.5 * Dec_VREG1_REF1635)) / 254.0);
      Voltage_VREG1_REF1227 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (206.5 + (0.5 * Dec_VREG1_REF1227)) / 254.0);
      Voltage_VREG1_REF815 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (182.5 + (0.5 * Dec_VREG1_REF815)) / 254.0);
      Voltage_VREG1_REF407 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (154.5 + (0.5 * Dec_VREG1_REF407)) / 254.0);
      Voltage_VREG1_REF63 = Voltage_VREG1_REF2047 + ((Voltage_VREG1_REF1 - Voltage_VREG1_REF2047) * (62.5 + (0.5 * Dec_VREG1_REF63)) / 254.0);
}



//Get Vreg1/RGB Voltage From Gamma
extern "C" __declspec (dllexport )double DP173_Get_Vreg1_Voltage(int Dec_Vreg1,double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1)
{
    double Va = Voltage_VREG1_REF63 - Voltage_VREG1_REF2047;
    double Vb = Voltage_VREG1_REF407 - Voltage_VREG1_REF63;
    double Vc = Voltage_VREG1_REF815 - Voltage_VREG1_REF407;
    double Vd = Voltage_VREG1_REF1227 - Voltage_VREG1_REF815;
    double Ve = Voltage_VREG1_REF1635 - Voltage_VREG1_REF1227;
    double Vf = Voltage_VREG1_REF1 - Voltage_VREG1_REF1635;
            
    if (Dec_Vreg1 == 0)
    {
        return 0;
    } 
    else if (Dec_Vreg1 == 1)
    {
        return Voltage_VREG1_REF2047;
    }
    else if (Dec_Vreg1 < 63)
    {
        return Voltage_VREG1_REF2047 + Va * ((Dec_Vreg1 - 1.0) / 62.0);
    }
    else if (Dec_Vreg1 == 63)
    {
        return Voltage_VREG1_REF63;
    }
    else if (Dec_Vreg1 < 407)
    {
        return Voltage_VREG1_REF63 + Vb * ((Dec_Vreg1 - 63.0) / 344.0);
    }
    else if (Dec_Vreg1 == 407)
    {
        return Voltage_VREG1_REF407;
    }
    else if (Dec_Vreg1 < 815)
    {
        return Voltage_VREG1_REF407 + Vc * ((Dec_Vreg1 - 407.0) / 408.0);
    }
    else if (Dec_Vreg1 == 815)
    {
        return Voltage_VREG1_REF815;
    }
    else if (Dec_Vreg1 < 1227)
    {
        return Voltage_VREG1_REF815 + Vd * ((Dec_Vreg1 - 815.0) / 412.0);
    }
    else if (Dec_Vreg1 == 1227)
    {
        return Voltage_VREG1_REF1227;
    }
    else if (Dec_Vreg1 < 1635)
    {
        return Voltage_VREG1_REF1227 + Ve * ((Dec_Vreg1 - 1227.0) / 408.0);
    }
    else if (Dec_Vreg1 == 1635)
    {
        return Voltage_VREG1_REF1635;
    }
    else if (Dec_Vreg1 < 2047)
    {
        return Voltage_VREG1_REF1635 + Vf * ((Dec_Vreg1 - 1635.0) / 412.0);
    }
    else if (Dec_Vreg1 == 2047)
    {
        return Voltage_VREG1_REF1;
    }
    else 
    {
        return 0;
    }
}

extern "C" __declspec (dllexport ) double DP173_Get_GR_Gamma_Voltage(double Voltage_VREG1_REF2047,double Vreg1_voltage,int Dec_AM1, double Prev_GR_Gamma_Voltage, int Gamma_Dec,int gray)
{
	double AM1_RGB_Voltage = DP173_Get_AM1_RGB_Voltage(Voltage_VREG1_REF2047,Vreg1_voltage,Dec_AM1); 

	//G191,G127(GR7,GR6)
	if((gray == 1) || (gray == 2))return AM1_RGB_Voltage + (Prev_GR_Gamma_Voltage - AM1_RGB_Voltage) * ((Gamma_Dec + 101) / 612.0);
	//G63,G31,G15,G7,G4(GR5,GR4,GR3,GR2,GR1)
	else return AM1_RGB_Voltage + (Prev_GR_Gamma_Voltage - AM1_RGB_Voltage) * ((Gamma_Dec + 1) / 512.0);
}
extern "C" __declspec (dllexport ) double DP173_Get_AM2_Gamma_Voltage(double Voltage_VREG1_REF2047, double Vreg1_voltage, int Gamma_Dec)
{
	//G255 (AM2)    
	return Voltage_VREG1_REF2047 + (Vreg1_voltage - Voltage_VREG1_REF2047) * ((Gamma_Dec + 189) / 700.0);
}

extern "C" __declspec (dllexport )double DP173_Get_AM1_RGB_Voltage(double Voltage_VREG1_REF2047,double Vreg1_voltage,int Dec_AM1)
{
	return Voltage_VREG1_REF2047 + (Vreg1_voltage - Voltage_VREG1_REF2047) * (((Dec_AM1*2) + 8) / 700.0);//R_Voltage
}

extern "C" __declspec (dllexport )double DP173_Get_AM0_RGB_Voltage(double Voltage_VREG1_REF2047,double Vreg1_voltage,int Dec_AM0)
{
	return Voltage_VREG1_REF2047 + (Vreg1_voltage - Voltage_VREG1_REF2047) * (Dec_AM0 / 700.0);//R_Voltage
}

extern "C" __declspec (dllexport ) int DP173_Get_AM1_RGB_Dec(double Voltage_VREG1_REF2047,double Vreg1_voltage,double AM1_Voltage)
{
	return (int)((((AM1_Voltage - Voltage_VREG1_REF2047)/(Vreg1_voltage - Voltage_VREG1_REF2047)) * 700.0) - 8)/2.0;
}
extern "C" __declspec (dllexport ) int DP173_Get_AM0_RGB_Dec(double Voltage_VREG1_REF2047,double Vreg1_voltage,double AM0_Voltage)
{
	return (int)((AM0_Voltage - Voltage_VREG1_REF2047)/(Vreg1_voltage - Voltage_VREG1_REF2047)) * 700.0;
}


//Get Vreg1/R/G/B_Dec From Voltage
extern "C" __declspec (dllexport ) int DP173_Get_Vreg1_Dec(double Vreg1_Voltage,double Voltage_VREG1_REF2047,double Voltage_VREG1_REF1635,double Voltage_VREG1_REF1227,double Voltage_VREG1_REF815,double Voltage_VREG1_REF407,double Voltage_VREG1_REF63,double Voltage_VREG1_REF1)
{
	double Va = Voltage_VREG1_REF63 - Voltage_VREG1_REF2047;
    double Vb = Voltage_VREG1_REF407 - Voltage_VREG1_REF63;
    double Vc = Voltage_VREG1_REF815 - Voltage_VREG1_REF407;
    double Vd = Voltage_VREG1_REF1227 - Voltage_VREG1_REF815;
    double Ve = Voltage_VREG1_REF1635 - Voltage_VREG1_REF1227;
    double Vf = Voltage_VREG1_REF1 - Voltage_VREG1_REF1635;
         
	if (Vreg1_Voltage < 0.0001) return 0;
    else if ((Vreg1_Voltage > (Voltage_VREG1_REF2047 - 0.00001)) && (Vreg1_Voltage < (Voltage_VREG1_REF2047 + 0.00001))) return 1; // X == Voltage_VREG1_REF2047
    else if ((Vreg1_Voltage > (Voltage_VREG1_REF63 - 0.00001)) && (Vreg1_Voltage < (Voltage_VREG1_REF63 + 0.00001))) return 63;// X == Voltage_VREG1_REF63
	else if ((Vreg1_Voltage > (Voltage_VREG1_REF407 - 0.00001)) && (Vreg1_Voltage < (Voltage_VREG1_REF407 + 0.00001))) return 407;// X == Voltage_VREG1_REF407
	else if ((Vreg1_Voltage > (Voltage_VREG1_REF815 - 0.00001)) && (Vreg1_Voltage < (Voltage_VREG1_REF815 + 0.00001))) return 815;// X == Voltage_VREG1_REF815
	else if ((Vreg1_Voltage > (Voltage_VREG1_REF1227 - 0.00001)) && (Vreg1_Voltage < (Voltage_VREG1_REF1227 + 0.00001))) return 1227;// X == Voltage_VREG1_REF1227
    else if ((Vreg1_Voltage > (Voltage_VREG1_REF1635 - 0.00001)) && (Vreg1_Voltage < (Voltage_VREG1_REF1635 + 0.00001))) return 1635;// X == Voltage_VREG1_REF1635
    else if ((Vreg1_Voltage > (Voltage_VREG1_REF1 - 0.00001)) && (Vreg1_Voltage < (Voltage_VREG1_REF1 + 0.00001))) return 2047;// X == Voltage_VREG1_REF1
	else if(Vreg1_Voltage > Voltage_VREG1_REF63)// Voltage_VREG1_REF2047 > X > Voltage_VREG1_REF63
	{
		// Vreg1_Voltage = Voltage_VREG1_REF2047 + Va * ((Dec_Vreg1 - 1.0) / 62.0);
		// V = A + B * (X - C) / D 
		// X =(V - A)*D/B + C
		return (int)(((Vreg1_Voltage - Voltage_VREG1_REF2047)*(62.0)/(Va)) + (1.0) + 0.5);
	}
	else if(Vreg1_Voltage > Voltage_VREG1_REF407)// Voltage_VREG1_REF63 > X > Voltage_VREG1_REF407
	{
		//  Vreg1_Voltage = Voltage_VREG1_REF63 + Vb * ((Dec_Vreg1 - 63.0) / 344.0);
		return (int)(((Vreg1_Voltage - Voltage_VREG1_REF63)*(344.0)/(Vb)) + (63.0)+ 0.5);
	}
	else if(Vreg1_Voltage > Voltage_VREG1_REF815)// Voltage_VREG1_REF407 > X > Voltage_VREG1_REF815
	{
		//  Vreg1_Voltage = Voltage_VREG1_REF407 + Vc * ((Dec_Vreg1 - 407.0) / 408.0);
		return (int)(((Vreg1_Voltage - Voltage_VREG1_REF407)*(408.0)/(Vc)) + (407.0)+ 0.5);
	}
	else if(Vreg1_Voltage > Voltage_VREG1_REF1227)// Voltage_VREG1_REF815 > X > Voltage_VREG1_REF1227
	{
		// Vreg1_Voltage = Voltage_VREG1_REF815 + Vd * ((Dec_Vreg1 - 815.0) / 412.0);
		return (int)(((Vreg1_Voltage - Voltage_VREG1_REF815)*(412.0)/(Vd)) + (815.0)+ 0.5);
	}
	else if(Vreg1_Voltage > Voltage_VREG1_REF1635)// Voltage_VREG1_REF1227 > X > Voltage_VREG1_REF1635
	{
		// Vreg1_Voltage = Voltage_VREG1_REF1227 + Ve * ((Dec_Vreg1 - 1227.0) / 408.0);
		return (int)(((Vreg1_Voltage - Voltage_VREG1_REF1227)*(408.0)/(Ve)) + (1227.0)+ 0.5);
	}
	else if(Vreg1_Voltage > Voltage_VREG1_REF1)// Voltage_VREG1_REF1635 > X > Voltage_VREG1_REF1
	{
		// Vreg1_Voltage = Voltage_VREG1_REF1635 + Vf * ((Dec_Vreg1 - 1635.0) / 412.0);
		return (int)(((Vreg1_Voltage - Voltage_VREG1_REF1635)*(412.0)/(Vf)) + (1635.0)+ 0.5);
	}
	else 
	{
		//impossible (X > REF2047 or X < REF1)
		return 9999999;
	}
}

extern "C" __declspec (dllexport ) int DP173_Get_GR_Gamma_Dec(double Voltage_VREG1_REF2047,double Vreg1_voltage,int Dec_AM1 , double Prev_GR_Gamma_Voltage, double Gamma_Voltage ,int gray)
{
	double AM1_RGB_Voltage = DP173_Get_AM1_RGB_Voltage(Voltage_VREG1_REF2047, Vreg1_voltage, Dec_AM1); 

	//G191,G127(GR7,GR6)
	//Gamma_Voltage = AM1_RGB_Voltage + (Prev_GR_Gamma_Voltage - AM1_RGB_Voltage) * ((Gamma_Dec + 101) / 612.0);
	//Gamma_Dec = ((Gamma_Voltage-AM1_RGB_Voltage)/(Prev_GR_Gamma_Voltage-AM1_RGB_Voltage)* (612.0)) - 101;
	if((gray == 1) || (gray == 2))return (int)((((Gamma_Voltage-AM1_RGB_Voltage)/(Prev_GR_Gamma_Voltage-AM1_RGB_Voltage)* (612.0)) - 101) + 0.5);

	//G63,G31,G15,G7,G4(GR5,GR4,GR3,GR2,GR1)
	//Gamma_Voltage = AM1_RGB_Voltage + (Prev_GR_Gamma_Voltage - AM1_RGB_Voltage) * ((Gamma_Dec + 1) / 512.0);
	//Gamma_Dec = ((Gamma_Voltage-AM1_RGB_Voltage)/(Prev_GR_Gamma_Voltage-AM1_RGB_Voltage)* (512.0)) - 1;
	else return (int)((((Gamma_Voltage-AM1_RGB_Voltage)/(Prev_GR_Gamma_Voltage-AM1_RGB_Voltage)* (512.0)) - 1) + 0.5);
}

extern "C" __declspec (dllexport ) int DP173_Get_AM2_Gamma_Dec(double Voltage_VREG1_REF2047, double Vreg1_voltage, double AM2_Gamma_voltage)
{
	//G255 (AM2)    
	//AM2_Gamma_voltage = Voltage_VREG1_REF2047 + (Vreg1_voltage - Voltage_VREG1_REF2047) * ((Gamma_Dec + 189) / 700.0);
	//Gamma_Dec = (AM2_Gamma_voltage - Voltage_VREG1_REF2047)/(Vreg1_voltage - Voltage_VREG1_REF2047)* 700.0 -  189;
	return (int)((((AM2_Gamma_voltage - Voltage_VREG1_REF2047)/(Vreg1_voltage - Voltage_VREG1_REF2047)* 700.0) -  189) + 0.5);
}

bool desc(double a, double b){ return a > b; }