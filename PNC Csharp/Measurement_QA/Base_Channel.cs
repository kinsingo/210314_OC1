using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using BSQH_Csharp_Library;

namespace PNC_Csharp.Measurement_QA
{
    class Base_Channel
    {
        protected Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        protected double Sub_Max_DeltaE4_Calculation(int dgv_start_index,int max_index, DataGridView datagridview, int DGV_Channel_Offset)
        {
            //X Y Z
            double[] XArr = new double[max_index + 1];
            double[] YArr = new double[max_index + 1];
            double[] ZArr = new double[max_index + 1];

            //L a* b*
            double[] LArr = new double[max_index + 1];
            double[] aArr = new double[max_index + 1];
            double[] bArr = new double[max_index + 1];

            for (int index = 0; index <= max_index; index++)
            {
                double x = Convert.ToDouble(datagridview.Rows[index + dgv_start_index].Cells[1 + DGV_Channel_Offset].Value);
                double y = Convert.ToDouble(datagridview.Rows[index + dgv_start_index].Cells[2 + DGV_Channel_Offset].Value);
                double Lv = Convert.ToDouble(datagridview.Rows[index + dgv_start_index].Cells[3 + DGV_Channel_Offset].Value);

                XArr[index] = (x / y) * Lv;
                YArr[index] = Lv;
                ZArr[index] = ((1 - x - y) / y) * Lv;
            }


            // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
            double X255 = XArr[max_index];
            double Y255 = YArr[max_index];
            double Z255 = ZArr[max_index];

            //a* , b* , Delta E
            double Total_Delta_E = 0;


            for (int index = 0; index <= max_index; index++)
            {
                double X = XArr[index];
                double Y = YArr[index];
                double Z = ZArr[index];
                double L, FX, FY, FZ;

                //Calculate L*
                if (Y / Y255 > 0.008856)
                {
                    L = 116 * Math.Pow(Y / Y255, 0.33333333) - 16;
                }
                else
                {
                    L = 903.3 * (Y / Y255);
                }

                //Calculate F(X/Xw)
                if (X / X255 > 0.008856)
                {
                    FX = Math.Pow((X / X255), 0.33333333);
                }
                else
                {
                    FX = 7.787 * (X / X255) + (16 / 116);
                }

                //Calculate F(Y/Yw)
                if (Y / Y255 > 0.008856)
                {
                    FY = Math.Pow((Y / Y255), 0.33333333);
                }
                else
                {
                    FY = 7.787 * (Y / Y255) + (16 / 116);
                }

                //Calculate F(Z/Zw)
                if (Z / Z255 > 0.008856)
                {
                    FZ = Math.Pow((Z / Z255), 0.33333333);
                }
                else
                {
                    FZ = 7.787 * (Z / Z255) + (16 / 116);
                }

                //Calculate L a* , b*
                LArr[index] = L;
                aArr[index] = 500 * (FX - FY); //a*
                bArr[index] = 200 * (FY - FZ); //b*
            }

            int count = 0;
            for (int index = 0; index < max_index; index += 2)
            {
                count++;

                double L1 = LArr[index]; 
                double L2 = LArr[index + 1];

                double a1 = aArr[index];
                double a2 = aArr[index + 1];

                double b1 = bArr[index];
                double b2 = bArr[index + 1];

                double delta_L = L1 - L2;
                double delta_a = a1 - a2;
                double delta_b = b1 - b2;

                double Delta_E = Math.Pow((Math.Pow(delta_L, 2) + Math.Pow(delta_a, 2) + Math.Pow(delta_b, 2)), 0.5);
                Total_Delta_E += Delta_E;

                datagridview.Rows[index + dgv_start_index].Cells[4 + DGV_Channel_Offset].Value = Delta_E; //Delta E
            }

            return (Total_Delta_E / count);
        }




        protected double Sub_Max_DeltaE3_Calculation(bool Is_Min_to_Max_E3, int gray_end_Point, int Addtional_DeltaE_Rows, DataGridView datagridview, int DGV_Channel_Offset)
        {
            // x/y/Lv 계산 완료 후 -- > X Y Z 계산
            double x;
            double y;
            double Lv;

            double[] X_Array = new double[256];//0~255 (maximum 256ea) 
            double[] Y_Array = new double[256];//0~255 (maximum 256ea)
            double[] Z_Array = new double[256];//0~255 (maximum 256ea)

            int Offset = (Addtional_DeltaE_Rows) - (255 - gray_end_Point + 1);

            for (int gray = (gray_end_Point + Offset); gray <= (255 + Offset); gray++)
            {
                f1().GB_Status_AppendText_Nextline("[gray - gray_end_Point] : " + (gray - gray_end_Point).ToString(), Color.Blue, true);
                x = Convert.ToDouble(datagridview.Rows[gray - gray_end_Point].Cells[1 + DGV_Channel_Offset].Value);
                y = Convert.ToDouble(datagridview.Rows[gray - gray_end_Point].Cells[2 + DGV_Channel_Offset].Value);
                Lv = Convert.ToDouble(datagridview.Rows[gray - gray_end_Point].Cells[3 + DGV_Channel_Offset].Value);

                f1().GB_Status_AppendText_Nextline("[gray - (gray_end_Point+Offset)] : " + (gray - (gray_end_Point + Offset)).ToString(), Color.Blue, true);
                X_Array[gray - (gray_end_Point + Offset)] = (x / y) * Lv;
                Y_Array[gray - (gray_end_Point + Offset)] = Lv;
                Z_Array[gray - (gray_end_Point + Offset)] = ((1 - x - y) / y) * Lv;
            }


            // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
            double L, FX, FY, FZ;

            double X255 = 0;
            double Y255 = 0;
            double Z255 = 0;

            if (Is_Min_to_Max_E3)
            {
                f1().GB_Status_AppendText_Nextline("[255 - gray_end_Point] : " + (255 - gray_end_Point).ToString(), Color.Red, true);
                X255 = X_Array[255 - gray_end_Point];
                Y255 = Y_Array[255 - gray_end_Point];
                Z255 = Z_Array[255 - gray_end_Point];
            }
            //else if (radioButton_Max_to_Min_E3.Checked)
            else
            {
                X255 = X_Array[0];
                Y255 = Y_Array[0];
                Z255 = Z_Array[0];
            }

            //a* , b* , Delta E
            double a, b, Delta_E;
            double Max_Delta_E = 0;
            double X;
            double Y;
            double Z;

            for (int gray = (gray_end_Point + Offset); gray <= (255 + Offset); gray++)
            {
                f1().GB_Status_AppendText_Nextline("[gray - (gray_end_Point+Offset)] : " + (gray - (gray_end_Point + Offset)).ToString(), Color.Red, true);
                X = X_Array[gray - (gray_end_Point + Offset)];
                Y = Y_Array[gray - (gray_end_Point + Offset)];
                Z = Z_Array[gray - (gray_end_Point + Offset)];

                //Calculate L*
                if (Y / Y255 > 0.008856) L = 116 * Math.Pow(Y / Y255, 0.33333333) - 16;
                else L = 903.3 * (Y / Y255);

                //Calculate F(X/Xw)
                if (X / X255 > 0.008856) FX = Math.Pow((X / X255), 0.33333333);
                else FX = 7.787 * (X / X255) + (16 / 116.0);

                //Calculate F(Y/Yw)
                if (Y / Y255 > 0.008856) FY = Math.Pow((Y / Y255), 0.33333333);
                else FY = 7.787 * (Y / Y255) + (16 / 116.0);

                //Calculate F(Z/Zw)
                if (Z / Z255 > 0.008856) FZ = Math.Pow((Z / Z255), 0.33333333);
                else FZ = 7.787 * (Z / Z255) + (16 / 116.0);

                //Calculate a* , b* , Delta E
                a = 500 * (FX - FY);
                b = 200 * (FY - FZ);
                Delta_E = Math.Pow((Math.Pow(a, 2) + Math.Pow(b, 2)), 0.5);

                datagridview.Rows[gray - gray_end_Point].Cells[4 + DGV_Channel_Offset].Value = Delta_E; //Delta E

                if (Max_Delta_E <= Delta_E) Max_Delta_E = Delta_E;
            }


            return Max_Delta_E;
        }


        protected double Sub_Max_DeltaE2_Calculation(bool Is_Min_to_Max_E2, int dbv_end_Point, int dbv_max_point, int Step_Value, DataGridView datagridview, int DGV_Channel_Offset)
        {
            // x/y/Lv 계산 완료 후 -- > X Y Z 계산
            double x;
            double y;
            double Lv;

            double[] X_Array = new double[dbv_max_point + 1];//Maximum DBV_MAX !
            double[] Y_Array = new double[dbv_max_point + 1];//Maximum DBV_MAX !
            double[] Z_Array = new double[dbv_max_point + 1];//Maximum DBV_MAX !

            int count = 0;
            for (int dbv = dbv_end_Point; dbv < (dbv_max_point + Step_Value);)
            {
                x = Convert.ToDouble(datagridview.Rows[count].Cells[1 + DGV_Channel_Offset].Value);
                y = Convert.ToDouble(datagridview.Rows[count].Cells[2 + DGV_Channel_Offset].Value);
                Lv = Convert.ToDouble(datagridview.Rows[count].Cells[3 + DGV_Channel_Offset].Value);

                X_Array[count] = (x / y) * Lv;
                Y_Array[count] = Lv;
                Z_Array[count] = ((1 - x - y) / y) * Lv;

                f1().GB_Status_AppendText_Nextline("[dbv_end_Point]/dbv_max_point : " + (dbv_end_Point).ToString() + "/" + dbv_max_point.ToString(), Color.Blue, true);
                f1().GB_Status_AppendText_Nextline("[count]X_Array/Y_Array/Z_Array : [" + (count).ToString() + "]"
                    + X_Array[count].ToString() + "/" + Y_Array[count].ToString() + "/" + Z_Array[count].ToString(), Color.Blue, true);

                count++;

                dbv += Step_Value;
                if (dbv > dbv_max_point) break;
            }

            // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
            double L, FX, FY, FZ;
            double X4095 = 0;
            double Y4095 = 0;
            double Z4095 = 0;


            if (Is_Min_to_Max_E2)
            {
                X4095 = X_Array[count - 1];
                Y4095 = Y_Array[count - 1];
                Z4095 = Z_Array[count - 1];
            }
            //else if (radioButton_Max_to_Min_E2.Checked)
            else
            {
                X4095 = X_Array[0];
                Y4095 = Y_Array[0];
                Z4095 = Z_Array[0];
            }

            f1().GB_Status_AppendText_Nextline("X4095/Y4095/Z4095 : " + X4095.ToString() + "/" + Y4095.ToString() + "/" + Z4095.ToString(), Color.Red, true);

            //a* , b* , Delta E
            double a, b, Delta_E2;
            double Max_Delta_E2 = 0;
            double X;
            double Y;
            double Z;

            count = 0;
            for (int dbv = dbv_end_Point; dbv < (dbv_max_point + Step_Value);)
            {
                X = X_Array[count];
                Y = Y_Array[count];
                Z = Z_Array[count];

                //Calculate L*
                if (Y / Y4095 > 0.008856)
                {
                    L = 116 * Math.Pow(Y / Y4095, 0.33333333) - 16;
                }
                else
                {
                    L = 903.3 * (Y / Y4095);
                }

                //Calculate F(X/Xw)
                if (X / X4095 > 0.008856)
                {
                    FX = Math.Pow((X / X4095), 0.33333333);
                }
                else
                {
                    FX = 7.787 * (X / X4095) + (16 / 116.0);
                }

                //Calculate F(Y/Yw)
                if (Y / Y4095 > 0.008856)
                {
                    FY = Math.Pow((Y / Y4095), 0.33333333);
                }
                else
                {
                    FY = 7.787 * (Y / Y4095) + (16 / 116.0);
                }

                //Calculate F(Z/Zw)
                if (Z / Z4095 > 0.008856)
                {
                    FZ = Math.Pow((Z / Z4095), 0.33333333);
                }
                else
                {
                    FZ = 7.787 * (Z / Z4095) + (16 / 116.0);
                }
                //Calculate a* , b* , Delta E
                a = 500 * (FX - FY);
                b = 200 * (FY - FZ);
                Delta_E2 = Math.Pow((Math.Pow(a, 2) + Math.Pow(b, 2)), 0.5);

                datagridview.Rows[count].Cells[4 + DGV_Channel_Offset].Value = Delta_E2; //Delta E
                if (Max_Delta_E2 <= Delta_E2) Max_Delta_E2 = Delta_E2;


                count++;

                dbv += Step_Value;
                if (dbv > dbv_max_point) break;
            }

            return Max_Delta_E2;
        }
    }
}
