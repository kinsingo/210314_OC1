using BSQH_Csharp_Library;
using PNC_Csharp.CA_Multi_Channels;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace PNC_Csharp.Measurement_QA
{
    class Multi_Channel : Base_Channel , I_Channel 
    {
        private bool[] Get_Is_CA_connected_channels()
        {
            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];
            return ca_multi_ch_form.Get_Is_CA_connected_channels();
        }



        public void Calculate_Delta_E4_From_x_y_Lv(ref int dgv_startindex, int max_index, DataGridView datagridview)
        {
            bool[] Is_CA_connected_channels = Get_Is_CA_connected_channels();
            int index_1 = datagridview.Rows.Add();
            int index_2 = datagridview.Rows.Add();

            for (int ch = 0; ch < 10; ch++)
            {
                if (Is_CA_connected_channels[ch])
                {
                    int DGV_Channel_Offset = Get_DGV_Channel_Offset(ch, IsCalculateDeltaE: true);
                    double Max_Delta_E4 = base.Sub_Max_DeltaE4_Calculation(dgv_startindex, max_index, datagridview, DGV_Channel_Offset);
                    datagridview.Rows[index_1].Cells[0 + DGV_Channel_Offset].Value = "Delta E4";
                    datagridview.Rows[index_2].Cells[0 + DGV_Channel_Offset].Value = Max_Delta_E4.ToString();
                }
            }

            dgv_startindex += 2;
            datagridview.FirstDisplayedScrollingRowIndex = datagridview.RowCount - 1;
        }


        public void Calculate_Delta_E2_From_x_y_Lv(bool Is_Min_to_Max_E2, int dbv_end_Point, int dbv_max_point, int Step_Value, DataGridView datagridview)
        {
            bool[] Is_CA_connected_channels = Get_Is_CA_connected_channels();
            int index_1 = datagridview.Rows.Add();
            int index_2 = datagridview.Rows.Add();

            for (int ch = 0; ch < 10; ch++)
            {
                if (Is_CA_connected_channels[ch])
                {
                    int DGV_Channel_Offset = Get_DGV_Channel_Offset(ch, IsCalculateDeltaE: true);
                    double Max_Delta_E2 = base.Sub_Max_DeltaE2_Calculation(Is_Min_to_Max_E2, dbv_end_Point, dbv_max_point, Step_Value, datagridview, DGV_Channel_Offset);
                    datagridview.Rows[index_1].Cells[0 + DGV_Channel_Offset].Value = "Delta E2";
                    datagridview.Rows[index_2].Cells[0 + DGV_Channel_Offset].Value = Max_Delta_E2.ToString();
                }
            }
            datagridview.FirstDisplayedScrollingRowIndex = datagridview.RowCount - 1;
        }

        public void Calculate_Delta_E3_From_x_y_Lv(bool Is_Min_to_Max_E3, int gray_end_Point, int Addtional_DeltaE_Rows, DataGridView datagridview)
        {
            bool[] Is_CA_connected_channels = Get_Is_CA_connected_channels();
            int index_1 = datagridview.Rows.Add();
            int index_2 = datagridview.Rows.Add();

            for (int ch = 0; ch < 10; ch++)
            {
                if (Is_CA_connected_channels[ch])
                {
                    int DGV_Channel_Offset = Get_DGV_Channel_Offset(ch, IsCalculateDeltaE : true);
                    double Max_Delta_E3 = base.Sub_Max_DeltaE3_Calculation(Is_Min_to_Max_E3, gray_end_Point, Addtional_DeltaE_Rows, datagridview, DGV_Channel_Offset);
                    datagridview.Rows[index_1].Cells[0 + DGV_Channel_Offset].Value = "Delta E3";
                    datagridview.Rows[index_2].Cells[0 + DGV_Channel_Offset].Value = Max_Delta_E3.ToString();
                }
            }

            datagridview.FirstDisplayedScrollingRowIndex = datagridview.RowCount - 1;
        }
        public bool IsMultiChannel()
        {
            return true;
        }

        private int Get_DGV_Channel_Offset(int CA_Channel,bool IsCalculateDeltaE)
        {
            int DGV_Channel_Offset;

            if (IsCalculateDeltaE)
                DGV_Channel_Offset = (5 * CA_Channel);
            else
                DGV_Channel_Offset = (4 * CA_Channel);

            return DGV_Channel_Offset;
        }

        public void Measure_and_Update_Datagridview(DataGridView datagridview, int gray_or_dbv,bool IsCalculateDeltaE, AvgMeasMode avg_meas_mode)
        {
            System.Threading.Thread.Sleep(avg_meas_mode.Get_AverageMeasure_Delay_Before_Measure_MS());

            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];
            DataGridView multich_dgv = ca_multi_ch_form.Get_dataGridView_CA_Measure();
            multich_dgv.Rows.Clear();

            int ave_amount = avg_meas_mode.Get_AverageMeasure_Amount();
            double max_apply_lv = avg_meas_mode.Get_AverageMeasure_Apply_Max_Lv();

            bool[] Is_CA_connected_channels = Get_Is_CA_connected_channels();
            
            XYLv[] OutputXYLvs = ca_multi_ch_form.Sub_Measure_and_Get_MeasuredData();

            //첫번째 채널 기준으로 10ea 모두 Avg 할지말지 정함.
            if (OutputXYLvs[0].double_Lv <= max_apply_lv && ave_amount > 1)
                Update_OutputXYLvs_By_Deleting_Min_Max_and_Averaging_Measurement(OutputXYLvs, ave_amount);


            int index = datagridview.Rows.Add();
            for (int ch = 0; ch < 10; ch++)
            {
                int DGV_Channel_Offset = Get_DGV_Channel_Offset(ch, IsCalculateDeltaE);
                if (Is_CA_connected_channels[ch])
                {
                    datagridview.Rows[index].Cells[0 + DGV_Channel_Offset].Value = gray_or_dbv;
                    datagridview.Rows[index].Cells[1 + DGV_Channel_Offset].Value = OutputXYLvs[ch].double_X;
                    datagridview.Rows[index].Cells[2 + DGV_Channel_Offset].Value = OutputXYLvs[ch].double_Y;
                    datagridview.Rows[index].Cells[3 + DGV_Channel_Offset].Value = OutputXYLvs[ch].double_Lv;

                    multich_dgv.Rows.Add("ch" + (ch + 1), OutputXYLvs[ch].double_X.ToString("0.0000"), OutputXYLvs[ch].double_Y.ToString("0.0000"), OutputXYLvs[ch].double_Lv.ToString("0.0000"));
                }
                else
                {
                    multich_dgv.Rows.Add("ch" + (ch + 1).ToString().ToString(), "-", "-", "-");
                }
            }

            datagridview.FirstDisplayedScrollingRowIndex = datagridview.RowCount - 1;
            Application.DoEvents(); 
        }


        private XYLv[] Update_OutputXYLvs_By_Deleting_Min_Max_and_Averaging_Measurement(XYLv[] firstMeasured, int ave_amount)
        {
            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];

            List<double>[] x_list = new List<double>[firstMeasured.Length];
            List<double>[] y_list = new List<double>[firstMeasured.Length];
            List<double>[] lv_list = new List<double>[firstMeasured.Length];

            for (int i = 0; i < firstMeasured.Length; i++)
            {
                x_list[i] = new List<double>();
                y_list[i] = new List<double>();
                lv_list[i] = new List<double>();

                x_list[i].Add(firstMeasured[i].double_X);
                y_list[i].Add(firstMeasured[i].double_Y);
                lv_list[i].Add(firstMeasured[i].double_Lv);
            }
          
            //firstly_measured has been added already
            for (int i = 1; i < ave_amount; i++)
            {
                XYLv[] tempXYLvs = ca_multi_ch_form.Sub_Measure_and_Get_MeasuredData();

                for (int k = 0; k < tempXYLvs.Length; k++)
                {
                    x_list[k].Add(tempXYLvs[k].double_X);
                    y_list[k].Add(tempXYLvs[k].double_Y);
                    lv_list[k].Add(tempXYLvs[k].double_Lv);
                }
            }

            XYLv[] OutputXYLvs = new XYLv[firstMeasured.Length];
            for (int i = 0; i < firstMeasured.Length; i++)
            {
                x_list[i].Sort();
                y_list[i].Sort();
                lv_list[i].Sort();
                for (int k = 0; k < x_list[i].Count; k++)
                    f1().GB_Status_AppendText_Nextline(i + ")Sorted x/y/lv : " + x_list[i][k] + "/" + y_list[i][k] + "/" + lv_list[i][k], Color.Red);

                int mid = (ave_amount - 1) / 2;
                OutputXYLvs[i] =  new XYLv(x_list[i][mid], y_list[i][mid], lv_list[i][mid]);
                f1().GB_Status_AppendText_Nextline("output x/y/Lv : " + Math.Round(OutputXYLvs[i].double_X, 4) + " / " + Math.Round(OutputXYLvs[i].double_Y, 4) + " / " + Math.Round(OutputXYLvs[i].double_Lv, 4), Color.Blue);

                /*
                int count = 0;
                double x_sum = 0;
                double y_sum = 0;
                double lv_sum = 0;

                for (int k = 1; k < x_list[i].Count - 1; k++)
                {
                    f1().GB_Status_AppendText_Nextline(i + ")Sorted x/y/lv : " + x_list[i][k] + "/" + y_list[i][k] + "/" + lv_list[i][k], Color.DarkGreen);
                    x_sum += x_list[i][k];
                    y_sum += y_list[i][k];
                    lv_sum += lv_list[i][k];
                    count++;
                }
                OutputXYLvs[i] = new XYLv((x_sum / count), (y_sum / count), (lv_sum / count));
                */
            }
            return OutputXYLvs;
        }




        public bool IsCAConnected()
        {
            CA_Multi_Channel_Form ca_multi_ch_form = (CA_Multi_Channel_Form)System.Windows.Forms.Application.OpenForms["CA_Multi_Channel_Form"];
            return ca_multi_ch_form.Check_Is_CA_Connected();
        }

    }
}

