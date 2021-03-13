using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSQH_Csharp_Library
{
    public class ELVSS_Compensation
    {
        private bool _Is_ELVSS_Found;
        public ELVSS_Compensation()
        {
            _Is_ELVSS_Found = false;
        }

        //------------------------------
        public bool Is_ELVSS_Found()
        {
            return _Is_ELVSS_Found;
        }

        public int Get_ELVSSArrayLength()
        {
            return 6; //5 + 1
        }

        public double Get_First_ELVSS_Voltage()
        {
            //return -6.0;
            return -5.0;
        }

        public double Get_Last_ELVSS_Voltage()
        {
            return -0.8;
        }

        public double ELVSS_Minimum_Step()
        {
            return 0.1;
        }

       public void Is_ELVSS_Min_Max_Volatge_Valid(double ELVSS_Voltage_Max, double ELVSS_Voltage_Min)
        {
            double minimum = Get_First_ELVSS_Voltage() + ELVSS_Minimum_Step() * Get_ELVSSArrayLength();

            if (ELVSS_Voltage_Min < minimum)
                throw new Exception("ELVSS_Voltage_Min(" + ELVSS_Voltage_Min + ") should be bigger than " + minimum);

            double maximum = Get_Last_ELVSS_Voltage() - ELVSS_Minimum_Step() * Get_ELVSSArrayLength();
            if (ELVSS_Voltage_Max > maximum)
                throw new Exception("ELVSS_Voltage_Max(" + ELVSS_Voltage_Max + ") should be smaller than " + maximum);

            if(ELVSS_Voltage_Max - ELVSS_Voltage_Min < ELVSS_Minimum_Step())
                throw new Exception("The Gap Between ELVSS_Voltage_Max and ELVSS_Voltage_Min should be bigger than " + ELVSS_Minimum_Step());
        }


        public XYLv Get_Average_XYLv_After_Remove_Min_Max(XYLv[] xylvs)
        {
            if (xylvs.Length < 3)
                throw new Exception("Input Array Length Should be equal to or bigger than 3");

            List<double> X_List = new List<double>();
            List<double> Y_List = new List<double>();
            List<double> Lv_List = new List<double>();

            foreach (XYLv ele in xylvs)
            {
                X_List.Add(ele.double_X);
                Y_List.Add(ele.double_Y);
                Lv_List.Add(ele.double_Lv);
            }

            X_List.Sort();
            Y_List.Sort();
            Lv_List.Sort();

            double X_sum = 0;
            double Y_sum = 0;
            double Lv_sum = 0;
            int count = 0;
            for (int i = 1; i < xylvs.Length - 1; i++)
            {
                X_sum += X_List[i];
                Y_sum += Y_List[i];
                Lv_sum += Lv_List[i];
                count++;
            }
            return new XYLv(X_sum / count, Y_sum / count, Lv_sum / count);
        }

        //return ELVSS, 
        public double FindELVSS(double[] First_Five_Lv, double[] ELVSS, double[] Lv)
        {
            double threashold = Get_Threshold(First_Five_Lv);

            if (ELVSS.Length != Get_ELVSSArrayLength() || Lv.Length != Get_ELVSSArrayLength())
                throw new Exception("Input_Array(ELVSS & Lv) Length should be equal to " + Get_ELVSSArrayLength());

            Update_Is_ELVSS_Found(Lv,  threashold);

            return ELVSS[1];//6개 ELVSS값 중에 LV_Difference(5ea) 기준으로 첫번째 꺼니까 index는 0이 아닌 1반환
        }

        private void Update_Is_ELVSS_Found(double[] Lv, double threashold)
        {
            _Is_ELVSS_Found = true;
            for (int i = 0; i < Get_ELVSSArrayLength() - 1; i++)
            {
                double Lv_Difference = Lv[i] - Lv[i + 1];

                if (Lv_Difference < threashold)
                {
                    _Is_ELVSS_Found = false;
                    break;
                }
            }
        }

        //Receive [-6.0v , -5.6v] 5ea Lv, and get Lv(-5.9v) - Lv(-5.7v);
        private double Get_Threshold(double[] Frist_Five_Lv)
        {
            if (Frist_Five_Lv.Length != 5)
                throw new Exception("Input_Array(Frist_Five_Lv) Length should be equal to 5");

            return (Frist_Five_Lv[1] - Frist_Five_Lv[3]);
        }





    }
}
