using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Windows.Forms;
using BSQH_Csharp_Library;
using System.Drawing;

namespace PNC_Csharp
{
    public class DP213_OC_Offset : DP213_forms_accessor
    {
        private static DP213_OC_Offset instance;
        public static DP213_OC_Offset getInstance()
        {
            if (instance == null)
                instance = new DP213_OC_Offset();
            return instance;
        }

        RGB[,] AM1_Offset;
        public RGB[,] getAM1Offset()
        {
            return AM1_Offset;
        }

        int[,] Vreg1_Offset;
        public int[,] getVreg1Offset()
        {
            return Vreg1_Offset;
        }

        RGB[,] RGB_Offset_From_OCMode1_to_OCMode2;
        public RGB[,] Get_RGB_Offset_From_OCMode1_to_OCMode2() { return RGB_Offset_From_OCMode1_to_OCMode2; }

        RGB[,] RGB_Offset_From_OCMode1_to_OCMode3;
        public RGB[,] Get_RGB_Offset_From_OCMode1_to_OCMode3() { return RGB_Offset_From_OCMode1_to_OCMode3; }


        private string[][] Offset_Data;

        private DP213_OC_Offset()
        {
            AM1_Offset = new RGB[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Set_Amount];
            Vreg1_Offset = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Set_Amount];
            RGB_Offset_From_OCMode1_to_OCMode2 = new RGB[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
            RGB_Offset_From_OCMode1_to_OCMode3 = new RGB[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];

            string offset_file_path = Directory.GetCurrentDirectory() + "\\DP213\\Triple_Mode\\OC_Offset.csv";
            Offset_Data = File.ReadLines(offset_file_path).Select(x => x.Split(',')).ToArray();

            Update_Offset_Params();
        }

        private void Update_Offset_Params()
        {
            int Vreg1_row_start_offset = 1;
            int Vreg1_column_start_offset = 1;
            RGB AM1_row_start_offset = new RGB();
            AM1_row_start_offset.Set_Value(15,29,43);
            RGB AM1_column_start_offset = new RGB(Vreg1_column_start_offset);

            for(int row = 0;row < DP213_Static.Max_HBM_and_Normal_Band_Amount;row++)
            {
                for (int col = 0; col < DP213_Static.Max_Set_Amount; col++)
                {
                    Vreg1_Offset[row, col] = Convert.ToInt32(Offset_Data[Vreg1_row_start_offset + row][Vreg1_column_start_offset + col]);
                    AM1_Offset[row, col].int_R = Convert.ToInt32(Offset_Data[AM1_row_start_offset.int_R + row][AM1_column_start_offset.int_R + col]);
                    AM1_Offset[row, col].int_G = Convert.ToInt32(Offset_Data[AM1_row_start_offset.int_G + row][AM1_column_start_offset.int_G + col]);
                    AM1_Offset[row, col].int_B = Convert.ToInt32(Offset_Data[AM1_row_start_offset.int_B + row][AM1_column_start_offset.int_B + col]);
                }
            }
            //123
            int OCMode2_RGB_row_start_offset = 57;
            int OCMode3_RGB_row_start_offset = 191;
            for (int row = 0; row < (DP213_Static.Max_HBM_and_Normal_Band_Amount * DP213_Static.Max_Gray_Amount); row++)
            {
                int band = row / DP213_Static.Max_Gray_Amount;
                int gray = row % DP213_Static.Max_Gray_Amount;

                RGB_Offset_From_OCMode1_to_OCMode2[band, gray].int_R = Convert.ToInt32(Offset_Data[OCMode2_RGB_row_start_offset + row][1]);
                RGB_Offset_From_OCMode1_to_OCMode2[band, gray].int_G = Convert.ToInt32(Offset_Data[OCMode2_RGB_row_start_offset + row][2]);
                RGB_Offset_From_OCMode1_to_OCMode2[band, gray].int_B = Convert.ToInt32(Offset_Data[OCMode2_RGB_row_start_offset + row][3]);

                RGB_Offset_From_OCMode1_to_OCMode3[band, gray].int_R = Convert.ToInt32(Offset_Data[OCMode3_RGB_row_start_offset + row][1]);
                RGB_Offset_From_OCMode1_to_OCMode3[band, gray].int_G = Convert.ToInt32(Offset_Data[OCMode3_RGB_row_start_offset + row][2]);
                RGB_Offset_From_OCMode1_to_OCMode3[band, gray].int_B = Convert.ToInt32(Offset_Data[OCMode3_RGB_row_start_offset + row][3]);
            }
        }
    }
}
