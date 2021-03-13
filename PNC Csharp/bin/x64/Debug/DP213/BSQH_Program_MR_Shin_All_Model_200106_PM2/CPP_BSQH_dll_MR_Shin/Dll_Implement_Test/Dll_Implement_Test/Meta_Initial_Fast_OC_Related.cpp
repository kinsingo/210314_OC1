
#include "stdafx.h"
#include "Meta_Initial_Fast_OC_Related.h"
#include "math.h"
#include <time.h>
#include <iostream>
#include <ctime>
#include <vector>
#include <algorithm>
#include <cctype>
#include <iomanip>
#include <sstream>

using namespace std;

//Output : Gamma_R/G/B (Call by reference)
extern "C" __declspec (dllexport )void Dll_Meta_Get_First_Gamma_Fx_HBM(int& Gamma_R,int& Gamma_G,int& Gamma_B, bool* Selected_Band, int* HBM_Gamma_R, int* HBM_Gamma_G, int* HBM_Gamma_B, double* HBM_Target_Lv,
            int Current_Band_Dec_Vreg1, int band, int gray, double Target_Lv,double Prvious_Gray_Gamma_R_Voltage,double Prvious_Gray_Gamma_G_Voltage,double Prvious_Gray_Gamma_B_Voltage,
            double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
{
	if (band >= 1 && Selected_Band[band] == true && gray >= 1)//gray255(gray = 0)use vreg1 compensation (skip this option)
    {
		SJH_Matrix *M = new SJH_Matrix();

        double* HBM_Gamma_R_Voltage = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_R, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        double* HBM_Gamma_G_Voltage = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_G, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        double* HBM_Gamma_B_Voltage = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_B, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        double** A_R = M->MatrixCreate(8, 8);
        double** A_G = M->MatrixCreate(8, 8);
        double** A_B = M->MatrixCreate(8, 8);
        double** Inv_A_R = M->MatrixCreate(8, 8);
        double** Inv_A_G = M->MatrixCreate(8, 8);
        double** Inv_A_B = M->MatrixCreate(8, 8);
        double* C_R = new double[8];
        double* C_G = new double[8];
        double* C_B = new double[8];

        //Get A[i][count] = HBM_Gamma_Green_Voltage[i]^j
        int count = 0;
        for (int i = 0; i <= 7; i++)
        {
            count = 0;
            for (int j = 7; j >= 0; j--)
            {
                A_R[i][count] = pow(HBM_Gamma_R_Voltage[i], j);
                A_G[i][count] = pow(HBM_Gamma_G_Voltage[i], j);
                A_B[i][count] = pow(HBM_Gamma_B_Voltage[i], j);
                count++;
            }
        }

        //Get C[8] = inv(A)[8,8] * HBM_Target_Lv[8]
        Inv_A_R = M->MatrixInverse(A_R,8,8);
        Inv_A_G = M->MatrixInverse(A_G,8,8);
        Inv_A_B = M->MatrixInverse(A_B,8,8);
        C_R = M->Matrix_Multiply(Inv_A_R,8,8,HBM_Target_Lv);
        C_G = M->Matrix_Multiply(Inv_A_G,8,8,HBM_Target_Lv);
        C_B = M->Matrix_Multiply(Inv_A_B,8,8,HBM_Target_Lv);

        //Get Calculated Voltage_R/G/B
        double Calculated_Target_Lv;
        int iteration;
        double Calculated_R_Vdata = 0;
        double Calculated_G_Vdata = 0;
        double Calculated_B_Vdata = 0;
        for (double Vdata = Prvious_Gray_Gamma_R_Voltage; Vdata <= F7; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_R_Vdata == 0) { Calculated_R_Vdata = Vdata; break; }
        }

        for (double Vdata = Prvious_Gray_Gamma_G_Voltage; Vdata <= F7; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_G_Vdata == 0) { Calculated_G_Vdata = Vdata; break; }
        }

        for (double Vdata = Prvious_Gray_Gamma_B_Voltage; Vdata <= F7; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_B_Vdata == 0) { Calculated_B_Vdata = Vdata; break; }
        }

        //Get Gamma_R/G/B From Calculated Voltage_R/G/B
        double Current_Band_Vreg1_Voltage = Meta_Get_Vreg1_Voltage(Current_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        double Current_Band_AM1_RGB_Voltage = F7 + (Current_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);
        Gamma_R = Meta_Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_R_Voltage, Calculated_R_Vdata);
        Gamma_G = Meta_Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_G_Voltage, Calculated_G_Vdata);
        Gamma_B = Meta_Get_Gamma_From_Normal_Voltage(Current_Band_AM1_RGB_Voltage, Prvious_Gray_Gamma_B_Voltage, Calculated_B_Vdata);

	    delete M;
        delete[] HBM_Gamma_R_Voltage;// = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_R, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        delete[] HBM_Gamma_G_Voltage;// = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_G, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        delete[] HBM_Gamma_B_Voltage;// = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_B, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        delete[] A_R;// = M->MatrixCreate(8, 8);
        delete[] A_G;// = M->MatrixCreate(8, 8);
        delete[] A_B;// = M->MatrixCreate(8, 8);
        delete[] Inv_A_R;// = M->MatrixCreate(8, 8);
        delete[] Inv_A_G;// = M->MatrixCreate(8, 8);
        delete[] Inv_A_B;// = M->MatrixCreate(8, 8);
        delete[] C_R;// = new double[8];
        delete[] C_G;// = new double[8];
        delete[] C_B;// = new double[8];
    }
}






//Fast Initial Related (OK)
extern "C" __declspec (dllexport ) int Test_checkBox_Get_HBM_Equation(int band,double* HBM_Gamma_Voltage_G,double* HBM_Gray_Target,double* G255_Band_Target,int* G255_Band_Gamma_G
	,double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt , double VREG1_REF205_volt, double F7)
{
   //C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv 
   //AC = Lv
   SJH_Matrix* m = new SJH_Matrix();
   double* Lv = new double[8];
   double* Vdata_G = new double[8];
   double** A_G = m->MatrixCreate(8, 8);

   //double[][] inv = M.MatrixInverse(A);
   //Get A and Lv
    int count = 0;
    for (int i = 0; i <= 7; i++)
    {
        Vdata_G[i] = HBM_Gamma_Voltage_G[i];
        Lv[i] = HBM_Gray_Target[i];
        count = 0;
		for (int j = 7; j >= 0; j--)
        {
            A_G[i][count] = pow(Vdata_G[i], j);
		    count++;
        }	
    }

	//Get C = inv(A) * Lv
    double** Inv_A_G = m->MatrixCreate(8, 8);
    double* C_G = new double[8];
    Inv_A_G = m->MatrixInverse(A_G,8,8);
    C_G = m->Matrix_Multiply(Inv_A_G,8,8,Lv);

	//Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
    double Target_Lv;
    double* G255_Calculated_Vdata_G = new double[10];
    double G255_Calculated_Target_Lv;
    int iteration;

    G255_Calculated_Vdata_G[0] = HBM_Gamma_Voltage_G[0];

    cout<<"3"<<endl;

	int Return_Value = 0;

	if(band == 0)
    {
		Return_Value = 1023;
	}
	else if(band >= 1 && band <10)
	{
		Target_Lv = G255_Band_Target[band];
        G255_Calculated_Vdata_G[band] = 0;
        G255_Calculated_Target_Lv = 0;
        for (double Vdata = 4; Vdata <= F7; Vdata += 0.001)
        {
            G255_Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--)
            {
                G255_Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            }
            if ((G255_Calculated_Target_Lv < Target_Lv) && G255_Calculated_Vdata_G[band] == 0) 
			{ 
					G255_Calculated_Vdata_G[band] = Vdata;
					break;
			}			
		} 
		double Vreg1 = F7 + ((G255_Calculated_Vdata_G[band] - F7) * (700.0 / (G255_Band_Gamma_G[band] + 189.0)));;
		Return_Value = Meta_Get_Vreg1_Dec(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7); 
	}
	else 
    {
		Return_Value = 0;
	}

   delete[] Lv;
   delete[] Vdata_G;
   delete[] A_G;
   delete[] Inv_A_G;
   delete[] C_G;
   delete[] G255_Calculated_Vdata_G;
   delete m; //add on 191113

   return Return_Value;
}

//(OK)
int Meta_Get_Vreg1_Dec(double Vreg1_Voltage, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt , double VREG1_REF205_volt, double F7)
{
        if ((Vreg1_Voltage > (6.7 - 0.0001)) && (Vreg1_Voltage < (6.7 + 0.0001))) return 0;//DDVDH
        else if ((Vreg1_Voltage > (F7 - 0.0001)) && (Vreg1_Voltage < (F7 + 0.0001))) return 1;//ELVDD + VCI1_LVL[5:0]
        else if ((Vreg1_Voltage > (VREG1_REF205_volt - 0.0001)) && (Vreg1_Voltage < (VREG1_REF205_volt + 0.0001))) return 205;
        else if ((Vreg1_Voltage > (VREG1_REF409_volt - 0.0001)) && (Vreg1_Voltage < (VREG1_REF409_volt + 0.0001))) return 409;
        else if ((Vreg1_Voltage > (VREG1_REF614_volt - 0.0001)) && (Vreg1_Voltage < (VREG1_REF614_volt + 0.0001))) return 614;
        else if ((Vreg1_Voltage > (VREG1_REF818_volt - 0.0001)) && (Vreg1_Voltage < (VREG1_REF818_volt + 0.0001))) return 818;
        else if ((Vreg1_Voltage > (E7 - 0.0001)) && (Vreg1_Voltage < (E7 + 0.0001))) return 1023;//ELVDD - FV1_LVL[5:0]
        else
        {
            if (Vreg1_Voltage > VREG1_REF205_volt)
            {
                return 205;
            }
            else if (Vreg1_Voltage > VREG1_REF409_volt)
            {
                double Offset_409 = 204.0;
                double Offset = ((Vreg1_Voltage - VREG1_REF205_volt) / (VREG1_REF409_volt - VREG1_REF205_volt)) * Offset_409;
                return (int)Offset + 205; //return Dec_Vreg1

            }
            else if (Vreg1_Voltage > VREG1_REF614_volt)
            {
                double Offset_614 = 205.0; // = 614 - 409
                double Offset = ((Vreg1_Voltage - VREG1_REF409_volt) / (VREG1_REF614_volt - VREG1_REF409_volt)) * Offset_614;
                return (int)Offset + 409; //return Dec_Vreg1

            }
            else if (Vreg1_Voltage > VREG1_REF818_volt)
            {
                double Offset_818 = 204.0;
                double Offset = ((Vreg1_Voltage - VREG1_REF614_volt) / (VREG1_REF818_volt - VREG1_REF614_volt)) * Offset_818;
                return (int)Offset + 614; //return Dec_Vreg1

            }
            else if (Vreg1_Voltage > (E7 + 0.0001))
            {
                double Offset_1023 = 205.0;
                double Offset = ((Vreg1_Voltage - VREG1_REF818_volt) / (E7 - VREG1_REF818_volt)) * Offset_1023;
                return (int)Offset + 818; 

            }
            else
            {
                return 9999; //Will Not Happen
            }
        }
}

//Fast Initial Related (private , OK)
extern "C" __declspec (dllexport )
double Meta_Get_Normal_Gamma_Voltage(double L, double H, int Gamma_Dec)
{
	return L + (H - L) * ((Gamma_Dec + 1.0) / 512.0); 
}
//OK
extern "C" __declspec (dllexport )
double Meta_Get_AM2_Voltage(double F7, double Vreg1_Voltage, int Gamma_Dec)
{
	return F7 + (Vreg1_Voltage - F7) * ((Gamma_Dec + 189.0) / 700.0);

}
//OK
extern "C" __declspec (dllexport )
int Meta_Get_Gamma_From_Normal_Voltage(double L, double H, double Vdata)
{
	return (int)(((512.0 * (Vdata - L) / (H - L))-1) + 0.5);
}
//OK
extern "C" __declspec (dllexport )
int Meta_Get_Gamma_From_AM2_Voltage(double F7, double Vreg1_Voltage, double Vdata)
{
	return (int)(((700.0 * (Vdata - F7) / (Vreg1_Voltage - F7)) - 189)+0.5);
}

//Fast Initial Related (public) // Get --> E7/F7/VREG1_REF818_volt/VREG1_REF614_volt/VREG1_REF409_volt/VREG1_REF205_volt
//OK
extern "C" __declspec (dllexport )double Get_E7 (double ELVDD,int dec_FV1)
{
	if (dec_FV1 >= 42) dec_FV1 = 42;
	return ELVDD - (0.2 + (dec_FV1 * 0.1)); //ELVDD - FV1_LVL[5:0]
}
//OK
extern "C" __declspec (dllexport )double Get_F7 (double ELVDD,int dec_VCI1)
{
	if (dec_VCI1 >= 42) dec_VCI1 = 42;
    return ELVDD + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]
}
//OK
extern "C" __declspec (dllexport )double Get_VREG1_REF818_volt (double E7,double F7,int Dec_VREG1_REF818)
{
    return F7+(E7-F7)*((222.5+0.5*Dec_VREG1_REF818)/254);
}
//OK
extern "C" __declspec (dllexport )double Get_VREG1_REF614_volt (double E7,double F7,int Dec_VREG1_REF614)
{
	return F7+(E7-F7)*((206.5+0.5*Dec_VREG1_REF614)/254);
}
//OK
extern "C" __declspec (dllexport )double Get_VREG1_REF409_volt (double E7,double F7,int Dec_VREG1_REF409)
{
	return F7+(E7-F7)*((182.5+0.5*Dec_VREG1_REF409)/254);
}
//OK
extern "C" __declspec (dllexport )double Get_VREG1_REF205_volt (double E7,double F7,int Dec_VREG1_REF205)
{
	return F7+(E7-F7)*((154.5+0.5*Dec_VREG1_REF205)/254);
}








//Fast Initial Related (public) // Get --> HBM_Gamma_Green_Voltage[]
//int* HBM_Gamma_Green = new int[8];
//double* HBM_Target_Lv = new double[8];
//double* HBM_Gamma_Green = new double[8];
double* Get_HBM_Gamma_Green_Voltage (int* HBM_Gamma_Green,double E7,double VREG1_REF818_volt,double VREG1_REF614_volt,double VREG1_REF409_volt,double VREG1_REF205_volt,double F7)
{
	double* HBM_Gamma_Green_Voltage = new double[8];

	int HBM_Dec_Vreg1 = 1023;
    double HBM_Vreg1_Voltage = Meta_Get_Vreg1_Voltage(HBM_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
    double HBM_AM1_RGB_Voltage = F7 + (HBM_Vreg1_Voltage - F7) * (8.0 / 700.0);

    for (int gray = 0; gray < 8; gray++)
    {
        if (gray == 0)HBM_Gamma_Green_Voltage[gray] = Meta_Get_AM2_Voltage(F7, HBM_Vreg1_Voltage, HBM_Gamma_Green[gray]);
        else HBM_Gamma_Green_Voltage[gray] = Meta_Get_Normal_Gamma_Voltage(HBM_AM1_RGB_Voltage, HBM_Gamma_Green_Voltage[gray - 1], HBM_Gamma_Green[gray]);
    }

	return HBM_Gamma_Green_Voltage;
}

//Fast Initial Related (public) --> "Fx" means "C" (Not yet defined) // Get --> Fx_Green[]
extern "C" __declspec (dllexport )double* Get_Fx_Green (int* HBM_Gamma_Green,double* HBM_Gray_Target,double* HBM_Gamma_Green_Voltage);


//Final
//1) bool* Selected_Band --> 10ea Param Array (For Each Band)
extern "C" __declspec (dllexport )
int Dll_Meta_Get_Normal_Initial_Vreg1_Fx_HBM(bool* Selected_Band,int* HBM_Gamma_Green,int Vreg1_Dec_Init,int band,double band_Target_Lv,int Previous_Band_G255_Green_Gamma,int Previous_Band_Vreg1_Dec,double* HBM_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt
            , double VREG1_REF205_volt, double F7)
{
	if (band >= 1 && Selected_Band[band] == true)
	{
		        double* HBM_Gamma_Green_Voltage = Get_HBM_Gamma_Green_Voltage(HBM_Gamma_Green,E7,VREG1_REF818_volt,VREG1_REF614_volt,VREG1_REF409_volt,VREG1_REF205_volt,F7);		 
		        SJH_Matrix* M = new SJH_Matrix();
                double** A_G = M->MatrixCreate(8, 8);

                //Get A[i][count] = HBM_Gamma_Green_Voltage[i]^j
                int count = 0;
                for (int i = 0; i <= 7; i++)
				{
                    count = 0;
                    for (int j = 7; j >= 0; j--)
					{
                        A_G[i][count++] = pow(HBM_Gamma_Green_Voltage[i], j);
                    }
                }

				//Get C[8] = inv(A)[8,8] * HBM_Target_Lv[8]
                double** Inv_A_G = M->MatrixCreate(8, 8);
                double* C_G = new double[8];
                Inv_A_G = M->MatrixInverse(A_G,8,8);
                C_G = M->Matrix_Multiply(Inv_A_G,8,8,HBM_Target_Lv);

				
				//Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                double Target_Lv = band_Target_Lv;
                double Calculated_Vdata_Green = 0;
                double Calculated_Target_Lv = 0;
                int iteration;

				//Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
                //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
                int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
                double Previous_Band_Vreg1_Voltage = Meta_Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                double Actual_Previous_Vdata_Green = Meta_Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_G255_Green_Gamma);

                //Green
                for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
                {
                    Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
                    if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
                }

                double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Previous_Band_G255_Green_Gamma + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
                int Vreg1_Dec = Meta_Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7); 
                
				delete[] HBM_Gamma_Green_Voltage;
				delete M;
				delete[] A_G;
				delete[] Inv_A_G;
				delete[] C_G;
				
				return Vreg1_Dec;
	}
	else //Band0 + Other not selected Bands's
	{
		return Vreg1_Dec_Init;
	}
}


extern "C" __declspec (dllexport )void Dll_Meta_Get_Normal_Initial_Vreg1_R_B_Fx_Previous_Band(int& Vreg1_Dec_Init,int& Gamma_R,int& Gamma_B, bool* Selected_Band, int* Previous_Band_Gamma_Red, int* Previous_Band_Gamma_Green, int* Previous_Band_Gamma_Blue, int band, double band_Target_Lv, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
{
	if (band >= 1 && Selected_Band[band] == true)
    {
        double* Previous_Band_Gamma_Red_Voltage = Get_Previous_Band_Gamma_Green_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Red, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        double* Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Green_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        double* Previous_Band_Gamma_Blue_Voltage = Get_Previous_Band_Gamma_Green_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Blue, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                
        SJH_Matrix *M = new SJH_Matrix();
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
        double Previous_Band_Vreg1_Voltage = Meta_Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);

        double Actual_Previous_Vdata_Red = Meta_Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Red[0]);
        double Actual_Previous_Vdata_Green = Meta_Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[0]);
        double Actual_Previous_Vdata_Blue = Meta_Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Blue[0]);
                
        //Red
        for (double Vdata = Actual_Previous_Vdata_Red; Vdata <= F7; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_R[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Red == 0) Calculated_Vdata_Red = Vdata;
        }

        //Green
        for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
        }

        //Blue
        for (double Vdata = Actual_Previous_Vdata_Blue; Vdata <= F7; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_B[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Blue == 0) Calculated_Vdata_Blue = Vdata;
        }

        double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Previous_Band_Gamma_Green[0] + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
        Vreg1_Dec_Init = Meta_Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                
        //Got the Vreg1 
        //Need to get Gamma_R/B
        double Current_Band_AM1_RGB_Voltage = F7 + (Vreg1_voltage - F7) * (8.0 / 700.0);
        Gamma_R = Meta_Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Red);
        Gamma_B = Meta_Get_Gamma_From_AM2_Voltage(F7, Vreg1_voltage, Calculated_Vdata_Blue);

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






extern "C" __declspec (dllexport )
double Meta_Get_Vreg1_Voltage(int Dec_Vreg1,double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
{
    if (Dec_Vreg1 == 0) return 6.7;//DDVDH
    else if (Dec_Vreg1 == 1) return F7;//ELVDD + VCI1_LVL[5:0]
    else if (Dec_Vreg1 == 205) return VREG1_REF205_volt;
    else if (Dec_Vreg1 == 409) return VREG1_REF409_volt;
    else if (Dec_Vreg1 == 614) return VREG1_REF614_volt;
    else if (Dec_Vreg1 == 818) return VREG1_REF818_volt;
    else if (Dec_Vreg1 == 1023) return E7;//ELVDD - FV1_LVL[5:0]
    else
    {
        if (Dec_Vreg1 < 81)
        {
            return 0;
        }
        else if (Dec_Vreg1 < 181)
        {
            double Offset = 331.5 + (Dec_Vreg1 - 82) * 1.5;
            double Offset_181 = 480;
            return F7 + (VREG1_REF205_volt - F7) * (Offset / Offset_181);
        }
        else if (Dec_Vreg1 < 205)
        {
            double Offset = 481.2 + (Dec_Vreg1 - 182) * 1.2;
            double Offset_205 = 508.8;
            return F7 + (VREG1_REF205_volt - F7) * (Offset / Offset_205);
        }
        else if (Dec_Vreg1 < 409)
        {
            double Offset = Dec_Vreg1 - 205.0;
            double Offset_409 = 204.0; // = 409 - 205
            return VREG1_REF205_volt + (VREG1_REF409_volt - VREG1_REF205_volt) * (Offset / Offset_409);
        }
        else if (Dec_Vreg1 < 614)
        {
            double Offset = Dec_Vreg1 - 409;
            double Offset_614 = 205.0; // = 614 - 409
            return VREG1_REF409_volt + (VREG1_REF614_volt - VREG1_REF409_volt) * (Offset / Offset_614);
        }
        else if (Dec_Vreg1 < 818)
        {
            double Offset = Dec_Vreg1 - 614;
            double Offset_818 = 204.0; // = 818 - 614
            return VREG1_REF614_volt + (VREG1_REF818_volt - VREG1_REF614_volt) * (Offset / Offset_818);
        }
        else if (Dec_Vreg1 < 1023)
        {
            double Offset = Dec_Vreg1 - 818;
            double Offset_1023 = 205.0; // = 1023 - 818
            return VREG1_REF818_volt + (E7 - VREG1_REF818_volt) * (Offset / Offset_1023);
        }
        else
        {
            return 999; //Will Not Happen
        }
    }
           
}

double* Get_Previous_Band_Gamma_Green_Voltage(int Previous_Band_Vreg1_Dec, int* Previous_Band_Gamma_Green, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
{
	    double* Previous_Band_Gamma_Green_Voltage = new double[8];

        double Previous_Band_Vreg1_Voltage = Meta_Get_Vreg1_Voltage(Previous_Band_Vreg1_Dec, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        double Previous_Band_AM1_RGB_Voltage = F7 + (Previous_Band_Vreg1_Voltage - F7) * (8.0 / 700.0);

        for (int gray = 0; gray < 8; gray++)
        {
            if (gray == 0) Previous_Band_Gamma_Green_Voltage[gray] = Meta_Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_Gamma_Green[gray]);
            else Previous_Band_Gamma_Green_Voltage[gray] = Meta_Get_Normal_Gamma_Voltage(Previous_Band_AM1_RGB_Voltage, Previous_Band_Gamma_Green_Voltage[gray - 1], Previous_Band_Gamma_Green[gray]);
        }
        return Previous_Band_Gamma_Green_Voltage;
}

extern "C" __declspec (dllexport)
int Dll_Meta_Get_Normal_Initial_Vreg1_Fx_Previous_Band(bool* Selected_Band, int* Previous_Band_Gamma_Green, int Vreg1_Dec_Init, int band, double band_Target_Lv, int Previous_Band_G255_Green_Gamma, int Previous_Band_Vreg1_Dec
            , double* Previous_Band_Target_Lv, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
{
	if (band >= 1 && Selected_Band[band] == true)
    {
        double* Previous_Band_Gamma_Green_Voltage = Get_Previous_Band_Gamma_Green_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        SJH_Matrix* M = new SJH_Matrix();
        double** A_G = M->MatrixCreate(8, 8);

        //Get A[i][count] = Previous_Band_Gamma_Green_Voltage[i]^j
        int count = 0;
        for (int i = 0; i <= 7; i++)
        {
            count = 0;
            for (int j = 7; j >= 0; j--)
            {
                A_G[i][count++] = pow(Previous_Band_Gamma_Green_Voltage[i], j);
            }
        }

        //Get C[8] = inv(A)[8,8] * Previous_Band_Target_Lv[8]
        double** Inv_A_G = M->MatrixCreate(8, 8);
        double* C_G = new double[8];
        Inv_A_G = M->MatrixInverse(A_G,8,8);
        C_G = M->Matrix_Multiply(Inv_A_G,8,8,Previous_Band_Target_Lv);

        //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
        double Target_Lv = band_Target_Lv;
        double Calculated_Vdata_Green = 0;
        double Calculated_Target_Lv = 0;
        int iteration;

        //Get Vreg1 From Gamma_Green (Or the "Actual_Previous_Vdata_Green" can directly get from the datagridview_voltage_table
        //Didn't get data directly from the datagridview_voltage_table, because it's gonna be easier to code in cpp dll
        int Previous_Band_Dec_Vreg1 = Previous_Band_Vreg1_Dec;
        double Previous_Band_Vreg1_Voltage = Meta_Get_Vreg1_Voltage(Previous_Band_Dec_Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        double Actual_Previous_Vdata_Green = Meta_Get_AM2_Voltage(F7, Previous_Band_Vreg1_Voltage, Previous_Band_G255_Green_Gamma);

        //Green
        for (double Vdata = Actual_Previous_Vdata_Green; Vdata <= F7; Vdata += 0.001)
        {
            Calculated_Target_Lv = 0;
            iteration = 0;
            for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (pow(Vdata, j) * C_G[iteration++]);
            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata_Green == 0) Calculated_Vdata_Green = Vdata;
        }

        double Vreg1_voltage = F7 + ((Calculated_Vdata_Green - F7) * (700.0 / (Previous_Band_G255_Green_Gamma + 189.0))); //just use HBM_Gamma[0], because other normal band copy G255 R/G/B From HBM
        int Vreg1_Dec = Meta_Get_Vreg1_Dec(Vreg1_voltage, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        
		delete[] Previous_Band_Gamma_Green_Voltage;// = Get_Previous_Band_Gamma_Green_Voltage(Previous_Band_Vreg1_Dec, Previous_Band_Gamma_Green, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
        delete M;// = new SJH_Matrix();
        delete[] A_G;// = M->MatrixCreate(8, 8);
		delete[] Inv_A_G;// = M->MatrixCreate(8, 8);
        delete[] C_G;// = new double[8];
		
		return Vreg1_Dec;
    }
    else //Band0 + Other not selected Bands's
    {
        return Vreg1_Dec_Init;
    }
}
