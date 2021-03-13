using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;//for CultureInfo
using System.IO;//for Dicrectory class
using BSQH_Csharp_Library;


namespace BSQH_Csharp_Library
{
    public class Init_Gray_LUT
    {
        string OC_Mode1_Gray_LUT_Path;
        string OC_Mode4_Gray_LUT_Path;
        string OC_Mode5_Gray_LUT_Path;
        string OC_Mode6_Gray_LUT_Path;
        
        //OC_Mode1/4/5/6 --> 4ea 
        RGB_Double[,] OC_Mode1_Matched_Grays; 
        RGB_Double[,] OC_Mode4_Matched_Grays;
        RGB_Double[,] OC_Mode5_Matched_Grays; 
        RGB_Double[,] OC_Mode6_Matched_Grays;
        RGB_Double[,] OC_Mode1_LUT_RGB;
        RGB_Double[,] OC_Mode4_LUT_RGB;
        RGB_Double[,] OC_Mode5_LUT_RGB;
        RGB_Double[,] OC_Mode6_LUT_RGB;

        public Init_Gray_LUT(string LUT_Path1, string LUT_Path2, string LUT_Path3, string LUT_Path4) 
        {
            OC_Mode1_Gray_LUT_Path = LUT_Path1;
            OC_Mode4_Gray_LUT_Path = LUT_Path2;
            OC_Mode5_Gray_LUT_Path = LUT_Path3;
            OC_Mode6_Gray_LUT_Path = LUT_Path4;

            OC_Mode1_Matched_Grays = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
            OC_Mode4_Matched_Grays = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
            OC_Mode5_Matched_Grays = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
            OC_Mode6_Matched_Grays = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];

            OC_Mode1_LUT_RGB = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
            OC_Mode4_LUT_RGB = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
            OC_Mode5_LUT_RGB = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
            OC_Mode6_LUT_RGB = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];

            Update_LUT_Grays();
        }

        public void Update_Matched_Grays(int band, RGB_Double[] RGBvoltages, OC_Mode Mode, InterpolationFomulaFactory interpolationFomulaFactory)
        {
            if (band == 0)
                throw new Exception("Should not Update band0's Matched_Grays");
    
            RGB_Double[,] Matched_Grays = Get_Matched_Grays(Mode);

            for (int i = 0; i < DP213_Static.Max_Gray_Amount; i++)
            {
                Matched_Grays[band, i].double_R = interpolationFomulaFactory.GetPrevBand_Red_gray(RGBvoltages[i].double_R);
                Matched_Grays[band, i].double_G = interpolationFomulaFactory.GetPrevBand_Green_gray(RGBvoltages[i].double_G);
                Matched_Grays[band, i].double_B = interpolationFomulaFactory.GetPrevBand_Blue_gray(RGBvoltages[i].double_B);
            }

        }

        public RGB_Double Get_LUT_RGB(int band,int gray,OC_Mode mode)
        {
            if (band == 0)
                throw new Exception("Should not Get band0's Matched_Grays");
            

            if (mode == OC_Mode.Mode1) return OC_Mode1_LUT_RGB[band, gray];
            if (mode == OC_Mode.Mode4) return OC_Mode4_LUT_RGB[band, gray];
            if (mode == OC_Mode.Mode5) return OC_Mode5_LUT_RGB[band, gray];
            if (mode == OC_Mode.Mode6) return OC_Mode6_LUT_RGB[band, gray];

            throw new Exception("OC_Mode should be 1,4,5 or 6");
        }

        private void Update_LUT_Grays()
        {
            StreamReader SReader1 = new StreamReader(OC_Mode1_Gray_LUT_Path);
            StreamReader SReader4 = new StreamReader(OC_Mode4_Gray_LUT_Path);
            StreamReader SReader5 = new StreamReader(OC_Mode5_Gray_LUT_Path);
            StreamReader SReader6 = new StreamReader(OC_Mode6_Gray_LUT_Path);

            //header Data should be removed in this way
            String[] OC_Mode1_Vals = SReader1.ReadLine().Split(',');
            String[] OC_Mode4_Vals = SReader4.ReadLine().Split(',');
            String[] OC_Mode5_Vals = SReader5.ReadLine().Split(',');
            String[] OC_Mode6_Vals = SReader6.ReadLine().Split(',');

            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                {
                    OC_Mode1_Vals = SReader1.ReadLine().Split(',');
                    OC_Mode4_Vals = SReader4.ReadLine().Split(',');
                    OC_Mode5_Vals = SReader5.ReadLine().Split(',');
                    OC_Mode6_Vals = SReader6.ReadLine().Split(',');

                    Sub_Update_LUT_RGB(band, gray, OC_Mode1_Vals, OC_Mode1_LUT_RGB);
                    Sub_Update_LUT_RGB(band, gray, OC_Mode4_Vals, OC_Mode4_LUT_RGB);
                    Sub_Update_LUT_RGB(band, gray, OC_Mode5_Vals, OC_Mode5_LUT_RGB);
                    Sub_Update_LUT_RGB(band, gray, OC_Mode6_Vals, OC_Mode6_LUT_RGB);
                }
            }

            SReader1.Close();
            SReader4.Close();
            SReader5.Close();
            SReader6.Close();
        }

        private void Sub_Update_LUT_RGB(int band, int gray, String[] OC_Mode_Vals, RGB_Double[,] OC_Mode_LUT_RGB)
        {
            int count = 0;
            double R_Sum = 0;
            double G_Sum = 0;
            double B_Sum = 0;

            double min_R = Double.MaxValue;
            double min_G = Double.MaxValue;
            double min_B = Double.MaxValue;

            double max_R = Double.MinValue;
            double max_G = Double.MinValue;
            double max_B = Double.MinValue;

            for (int i = 4; i < OC_Mode_Vals.Length; i += 4)
            {
                double R = Convert.ToDouble(OC_Mode_Vals[i + 1]);
                double G = Convert.ToDouble(OC_Mode_Vals[i + 2]);
                double B = Convert.ToDouble(OC_Mode_Vals[i + 3]);

                if (min_R > R) min_R = R;
                if (min_G > G) min_G = G;
                if (min_B > B) min_B = B;

                if (max_R < R) max_R = R;
                if (max_G < G) max_G = G;
                if (max_B < B) max_B = B;

                R_Sum += R;
                G_Sum += G;
                B_Sum += B;
                count++;
            }

            OC_Mode_LUT_RGB[band, gray].double_R = (R_Sum - min_R - max_R) / (count - 2);
            OC_Mode_LUT_RGB[band, gray].double_G = (G_Sum - min_G - max_G) / (count - 2);
            OC_Mode_LUT_RGB[band, gray].double_B = (B_Sum - min_B - max_B) / (count - 2);
        }

        public void Save_Updated_Matched_Grays()
        {
            StringBuilder Main_sb1 = new StringBuilder();
            StringBuilder Main_sb4 = new StringBuilder();
            StringBuilder Main_sb5 = new StringBuilder();
            StringBuilder Main_sb6 = new StringBuilder();
            
            StringBuilder SUB_sb1 = new StringBuilder();
            StringBuilder SUB_sb4 = new StringBuilder();
            StringBuilder SUB_sb5 = new StringBuilder();
            StringBuilder SUB_sb6 = new StringBuilder();

            StreamReader SReader1 = new StreamReader(OC_Mode1_Gray_LUT_Path);
            StreamReader SReader4 = new StreamReader(OC_Mode4_Gray_LUT_Path);
            StreamReader SReader5 = new StreamReader(OC_Mode5_Gray_LUT_Path);
            StreamReader SReader6 = new StreamReader(OC_Mode6_Gray_LUT_Path);
            
            Main_sb1.AppendLine(SReader1.ReadLine());
            Main_sb4.AppendLine(SReader4.ReadLine());
            Main_sb5.AppendLine(SReader5.ReadLine());
            Main_sb6.AppendLine(SReader6.ReadLine());
        
            for (int band = 0;band < DP213_Static.Max_HBM_and_Normal_Band_Amount;band++)
            {
                for (int gray = 0;gray < DP213_Static.Max_Gray_Amount; gray++)
                {
                    String[] OC_Mode1_Vals = SReader1.ReadLine().Split(',');
                    String[] OC_Mode4_Vals = SReader4.ReadLine().Split(',');
                    String[] OC_Mode5_Vals = SReader5.ReadLine().Split(',');
                    String[] OC_Mode6_Vals = SReader6.ReadLine().Split(',');

                    SUB_sb1.Clear();
                    SUB_sb4.Clear();
                    SUB_sb5.Clear();
                    SUB_sb6.Clear();

                    SUB_sb1.Append(OC_Mode1_Vals[0]).Append(",").Append(OC_Mode1_Matched_Grays[band, gray].double_R).Append(",").Append(OC_Mode1_Matched_Grays[band, gray].double_G).Append(",").Append(OC_Mode1_Matched_Grays[band, gray].double_B);
                    SUB_sb4.Append(OC_Mode4_Vals[0]).Append(",").Append(OC_Mode4_Matched_Grays[band, gray].double_R).Append(",").Append(OC_Mode4_Matched_Grays[band, gray].double_G).Append(",").Append(OC_Mode4_Matched_Grays[band, gray].double_B);
                    SUB_sb5.Append(OC_Mode5_Vals[0]).Append(",").Append(OC_Mode5_Matched_Grays[band, gray].double_R).Append(",").Append(OC_Mode5_Matched_Grays[band, gray].double_G).Append(",").Append(OC_Mode5_Matched_Grays[band, gray].double_B);
                    SUB_sb6.Append(OC_Mode6_Vals[0]).Append(",").Append(OC_Mode6_Matched_Grays[band, gray].double_R).Append(",").Append(OC_Mode6_Matched_Grays[band, gray].double_G).Append(",").Append(OC_Mode6_Matched_Grays[band, gray].double_B);
                    for(int i = 0; i < OC_Mode1_Vals.Length - 4;i++)
                    {
                        SUB_sb1.Append(",").Append(OC_Mode1_Vals[i]);
                        SUB_sb4.Append(",").Append(OC_Mode4_Vals[i]);
                        SUB_sb5.Append(",").Append(OC_Mode5_Vals[i]);
                        SUB_sb6.Append(",").Append(OC_Mode6_Vals[i]);
                    }

                    Main_sb1.AppendLine(SUB_sb1.ToString());
                    Main_sb4.AppendLine(SUB_sb4.ToString());
                    Main_sb5.AppendLine(SUB_sb5.ToString());
                    Main_sb6.AppendLine(SUB_sb6.ToString());
                }
            }

            //Must Close Before WriteAllText (because they access same csv File)
            SReader1.Close();
            SReader4.Close();
            SReader5.Close();
            SReader6.Close();

            File.WriteAllText(OC_Mode1_Gray_LUT_Path, Main_sb1.ToString());
            File.WriteAllText(OC_Mode4_Gray_LUT_Path, Main_sb4.ToString());
            File.WriteAllText(OC_Mode5_Gray_LUT_Path, Main_sb5.ToString());
            File.WriteAllText(OC_Mode6_Gray_LUT_Path, Main_sb6.ToString());
        }



        public RGB_Double[,] Get_Matched_Grays(OC_Mode Mode)
        {
            if (Mode == OC_Mode.Mode1) return OC_Mode1_Matched_Grays;
            if (Mode == OC_Mode.Mode4) return OC_Mode4_Matched_Grays;
            if (Mode == OC_Mode.Mode5) return OC_Mode5_Matched_Grays;
            if (Mode == OC_Mode.Mode6) return OC_Mode6_Matched_Grays;

            throw new Exception("OC_Mode Should be 1,4,5 or 6");
        }


    }
}
