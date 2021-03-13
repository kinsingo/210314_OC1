using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSQH_Csharp_Library
{
    public class InterpolationFomulaFactory
    {
        //Gray to Voltage --> Voltage = F(Gray);
        private SplineInterpolator[] PrevBand_G2V_Fomula;
        //Voltage to Gray --> Gray = F(Voltage);
        private SplineInterpolator[] PrevBand_V2G_Fomula;

        public InterpolationFomulaFactory()
        {
            PrevBand_G2V_Fomula = new SplineInterpolator[3];
            PrevBand_V2G_Fomula = new SplineInterpolator[3];
        }

        //----------Gray to Voltage --> Voltage = F(Gray)----------;
        public void CreatePrevBand_G2V_Fomula(double[] Grays, RGB_Double[] RGBvoltages)
        {
            double[] Red_Voltages = new double[RGBvoltages.Length];
            double[] Green_Voltages = new double[RGBvoltages.Length];
            double[] Blue_Voltages = new double[RGBvoltages.Length];
            double[] grays = new double[Grays.Length];

            int index = 0;
            for (int i = RGBvoltages.Length - 1; i >= 0; i--)
            {
                Red_Voltages[index] = RGBvoltages[i].double_R;
                Green_Voltages[index] = RGBvoltages[i].double_G;
                Blue_Voltages[index] = RGBvoltages[i].double_B;
                grays[index] = Grays[i];
                index++;
            }
           
            PrevBand_G2V_Fomula[0] = SplineInterpolator.createMonotoneCubicSpline(grays, Red_Voltages);
            PrevBand_G2V_Fomula[1] = SplineInterpolator.createMonotoneCubicSpline(grays, Green_Voltages);
            PrevBand_G2V_Fomula[2] = SplineInterpolator.createMonotoneCubicSpline(grays, Blue_Voltages);
        }
        public double GetPrevBand_Red_Volatge(double gray)
        {
            if (PrevBand_G2V_Fomula == null)
                throw new Exception("PrevBand_G2V_Fomula hasn't yet created");

            return PrevBand_G2V_Fomula[0].interpolate(gray);
        }

        public double GetPrevBand_Green_Volatge(double gray)
        {
            if (PrevBand_G2V_Fomula == null)
                throw new Exception("PrevBand_G2V_Fomula hasn't yet created");

            return PrevBand_G2V_Fomula[1].interpolate(gray);
        }

        public double GetPrevBand_Blue_Volatge(double gray)
        {
            if (PrevBand_G2V_Fomula == null)
                throw new Exception("PrevBand_G2V_Fomula hasn't yet created");

            return PrevBand_G2V_Fomula[2].interpolate(gray);
        }

        //------------Voltage to Gray --> Gray = F(Voltage)-----------
        public void CreatePrevBand_V2G_Fomula(RGB_Double[] RGBvoltages, double[] Grays)
        {
            double[] Red_Voltages = new double[RGBvoltages.Length];
            double[] Green_Voltages = new double[RGBvoltages.Length];
            double[] Blue_Voltages = new double[RGBvoltages.Length];
            double[] grays = new double[Grays.Length];

            for (int i = 0; i < RGBvoltages.Length; i++)
            {
                Red_Voltages[i] = RGBvoltages[i].double_R;
                Green_Voltages[i] = RGBvoltages[i].double_G;
                Blue_Voltages[i] = RGBvoltages[i].double_B;
                grays[i] = Grays[i];
            }

            PrevBand_V2G_Fomula[0] = SplineInterpolator.createMonotoneCubicSpline(Red_Voltages, grays);
            PrevBand_V2G_Fomula[1] = SplineInterpolator.createMonotoneCubicSpline(Green_Voltages, grays);
            PrevBand_V2G_Fomula[2] = SplineInterpolator.createMonotoneCubicSpline(Blue_Voltages, grays);
        }

        public double GetPrevBand_Red_gray(double voltage)
        {
            if (PrevBand_V2G_Fomula == null)
                throw new Exception("PrevBand_V2G_Fomula hasn't yet created");

            return PrevBand_V2G_Fomula[0].interpolate(voltage);
        }

        public double GetPrevBand_Green_gray(double voltage)
        {
            if (PrevBand_V2G_Fomula == null)
                throw new Exception("PrevBand_V2G_Fomula hasn't yet created");

            return PrevBand_V2G_Fomula[1].interpolate(voltage);
        }

        public double GetPrevBand_Blue_gray(double voltage)
        {
            if (PrevBand_V2G_Fomula == null)
                throw new Exception("PrevBand_V2G_Fomula hasn't yet created");

            return PrevBand_V2G_Fomula[2].interpolate(voltage);
        }
    }
}
