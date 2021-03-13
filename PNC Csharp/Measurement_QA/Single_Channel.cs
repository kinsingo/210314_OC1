using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using BSQH_Csharp_Library;
using System.Collections.Generic;

namespace PNC_Csharp.Measurement_QA
{
    class Single_Channel : Base_Channel,I_Channel
    {
        public void Calculate_Delta_E4_From_x_y_Lv(ref int dgv_startindex, int max_index, DataGridView datagridview)
        {
            f1().GB_Status_AppendText_Nextline("dgv_startindex before E4 calculation : " + dgv_startindex, Color.Blue);

            int DGV_Channel_Offset = 0;
            double Max_Delta_E4 = base.Sub_Max_DeltaE4_Calculation(dgv_startindex, max_index, datagridview, DGV_Channel_Offset);
            

            int index = datagridview.Rows.Add();
            datagridview.Rows[index].Cells[0 + DGV_Channel_Offset].Value = "Delta E4";

            index = datagridview.Rows.Add();
            datagridview.Rows[index].Cells[0 + DGV_Channel_Offset].Value = Max_Delta_E4.ToString();

            dgv_startindex += 2;

            datagridview.FirstDisplayedScrollingRowIndex = datagridview.RowCount - 1;
        }

        public void Calculate_Delta_E3_From_x_y_Lv(bool Is_Min_to_Max_E3,int gray_end_Point, int Addtional_DeltaE_Rows, DataGridView datagridview)
        {
            int DGV_Channel_Offset = 0;
            double Max_Delta_E3 = base.Sub_Max_DeltaE3_Calculation(Is_Min_to_Max_E3,  gray_end_Point,  Addtional_DeltaE_Rows,  datagridview, DGV_Channel_Offset);

            int index = datagridview.Rows.Add();
            datagridview.Rows[index].Cells[0 + DGV_Channel_Offset].Value = "Delta E3";

            index = datagridview.Rows.Add();
            datagridview.Rows[index].Cells[0 + DGV_Channel_Offset].Value = Max_Delta_E3.ToString();

            datagridview.FirstDisplayedScrollingRowIndex = datagridview.RowCount - 1;
        }

        public void Calculate_Delta_E2_From_x_y_Lv(bool Is_Min_to_Max_E2, int dbv_end_Point, int dbv_max_point, int Step_Value, DataGridView datagridview)
        {
            int DGV_Channel_Offset = 0;
            double Max_Delta_E2 = base.Sub_Max_DeltaE2_Calculation(Is_Min_to_Max_E2, dbv_end_Point, dbv_max_point, Step_Value, datagridview, DGV_Channel_Offset);

            int index = datagridview.Rows.Add();
            datagridview.Rows[index].Cells[0 + DGV_Channel_Offset].Value = "Delta E2";

            index = datagridview.Rows.Add();
            datagridview.Rows[index].Cells[0 + DGV_Channel_Offset].Value = Max_Delta_E2.ToString();

            datagridview.FirstDisplayedScrollingRowIndex = datagridview.RowCount - 1;
        }


        public void Measure_and_Update_Datagridview(DataGridView datagridview,int gray_or_dbv, bool IsCalculateDeltaE, AvgMeasMode avg_meas_mode)
        {
            System.Threading.Thread.Sleep(avg_meas_mode.Get_AverageMeasure_Delay_Before_Measure_MS());

            int ave_amount = avg_meas_mode.Get_AverageMeasure_Amount();
            double max_apply_lv = avg_meas_mode.Get_AverageMeasure_Apply_Max_Lv();

            
            f1().objCa.Measure();
            XYLv output_xylv = new XYLv(f1().objCa.OutputProbes.get_ItemOfNumber(1).sx,f1().objCa.OutputProbes.get_ItemOfNumber(1).sy,f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv);

            if (output_xylv.double_Lv <= max_apply_lv && ave_amount > 1)
                output_xylv = Get_Delete_Min_Max_and_Averaged_Measurement(output_xylv, ave_amount);
            
            f1().GB_Status_AppendText_Nextline("output x/y/Lv : " + Math.Round(output_xylv.double_X, 4) + " / " + Math.Round(output_xylv.double_Y, 4) + " / " + Math.Round(output_xylv.double_Lv, 4), Color.Blue);

            //Data Grid setting//////////////////////
            datagridview.DataSource = null; // reset (unbind the datasource)
            datagridview.Rows.Add(gray_or_dbv.ToString(), Math.Round(output_xylv.double_X,4), Math.Round(output_xylv.double_Y,4),Math.Round(output_xylv.double_Lv,4));
            datagridview.FirstDisplayedScrollingRowIndex = datagridview.RowCount - 1;
            Application.DoEvents();
        }



        private XYLv Get_Delete_Min_Max_and_Averaged_Measurement(XYLv firstly_measured, int ave_amount)
        {
            List<double> x_list = new List<double>();
            List<double> y_list = new List<double>();
            List<double> lv_list = new List<double>();
            x_list.Add(firstly_measured.double_X);
            y_list.Add(firstly_measured.double_Y);
            lv_list.Add(firstly_measured.double_Lv);

            //firstly_measured has been added already
            for (int i = 1; i < ave_amount; i++)
            {
                f1().objCa.Measure();
                x_list.Add(f1().objCa.OutputProbes.get_ItemOfNumber(1).sx);
                y_list.Add(f1().objCa.OutputProbes.get_ItemOfNumber(1).sy);
                lv_list.Add(f1().objCa.OutputProbes.get_ItemOfNumber(1).Lv);
            }

            x_list.Sort();
            y_list.Sort();
            lv_list.Sort();

            for (int i = 0; i < x_list.Count; i++)
                f1().GB_Status_AppendText_Nextline("Sorted x/y/lv : " + x_list[i] + "/" + y_list[i] + "/" + lv_list[i], Color.Red);


            int mid = (ave_amount - 1) / 2;

            return new XYLv(x_list[mid], y_list[mid], lv_list[mid]);


            /*
            //first & last will not calculated.
            int count = 0;
            double x_sum = 0;
            double y_sum = 0;
            double lv_sum = 0;

            for (int i = 1; i < x_list.Count - 1; i++)
            {
                f1().GB_Status_AppendText_Nextline("Sorted x/y/lv : " + x_list[i] + "/" + y_list[i] + "/" + lv_list[i], Color.DarkGreen);
                x_sum += x_list[i];
                y_sum += y_list[i];
                lv_sum += lv_list[i];
                count++;
            }

            return new XYLv((x_sum / count), (y_sum / count), (lv_sum / count));
            */
        }

        public bool IsMultiChannel()
        {
            return false;
        }

        public bool IsCAConnected()
        {
            return f1().Is_Single_CA_is_connected;
        }

        
    }
}
