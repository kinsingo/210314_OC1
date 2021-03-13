using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNC_Csharp.Measurement_10ch
{
    public class Multi_CH_Measurement_Preferences
    {
        //Info
        public string Saved_Date;

        public string Set_Change_Delay;

        public bool[] check_SET = new bool[6];
        public string[] SEQ_SET = new string[6];
        public string[] textBox_Script_SET = new string[6];

        public bool[] check_GCS_DBV = new bool[20];
        public string[] GCS_DBV = new string[20];

        public string GCS_Delay;
        public string GCS_min_gray;
        public string GCS_max_gray;
        public string GCS_step;

        public bool GCS_Min_to_Max;
        public bool GCS_Max_to_Min;

        public bool[] check_BCS_Gray = new bool[20];
        public string[] BCS_Gray = new string[20];

        public bool BCS_Range1;
        public bool BCS_Range2;
        public bool BCS_Range3;

        public string BCS_Delay;
        public string BCS_Range1_min_DBV;
        public string BCS_Range1_max_DBV;
        public string BCS_Range1_step;
        public string BCS_Range2_min_DBV;
        public string BCS_Range2_max_DBV;
        public string BCS_Range2_step;
        public string BCS_Range3_min_DBV;
        public string BCS_Range3_max_DBV;
        public string BCS_Range3_step;

        public bool BCS_Min_to_Max;
        public bool BCS_Max_to_Min;

        public bool[] check_Gamma_Crush = new bool[10];
        public string[] Gamma_Crush_DBV = new string[10];
        public string[] Gamma_Crush_Gray = new string[10];

        public string Gamma_Crush_PTN_Delay;
        public string Gamma_Crush_DBV_Delay;

        public bool checkBox_Gamma_Crush_W;
        public bool checkBox_Gamma_Crush_R;
        public bool checkBox_Gamma_Crush_G;
        public bool checkBox_Gamma_Crush_B;

        public bool[] check_AOD_GCS_DBV = new bool[3];
        public string[] AOD_GCS_DBV = new string[3];

        public string AOD_GCS_Delay;
        public string AOD_CODE_Delay;
        public string AOD_GCS_min_gray;
        public string AOD_GCS_max_gray;
        public string AOD_GCS_step;

        public bool AOD_GCS_Min_to_Max;
        public bool AOD_GCS_Max_to_Min;

        public string IR_Drop_DeltaE_DBV;
        public string IR_Drop_DeltaE_Delay;
        public string IR_Drop_DeltaE_Set;

        public string textBox_Aging_Time;
        public string textBox_Aging_DBV;

        public bool check_GCS_Measure;
        public bool check_BCS_Measure;
        public bool check_AOD_GCS_Measure;
        public bool check_IR_Drop_DeltaE_Measure;
        public bool check_Gamma_Crush_Measure;
    }
}
