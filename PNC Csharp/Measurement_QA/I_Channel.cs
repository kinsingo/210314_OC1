using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PNC_Csharp.Measurement_QA
{
    public interface I_Channel
    {
        void Measure_and_Update_Datagridview(DataGridView datagridview, int gray_or_dbv, bool IsCalculateDeltaE, AvgMeasMode avg_meas_mode);

        void Calculate_Delta_E3_From_x_y_Lv(bool Is_Min_to_Max_E3,int gray_end_Point, int Addtional_DeltaE_Rows, DataGridView datagridview);

        void Calculate_Delta_E2_From_x_y_Lv(bool Is_Min_to_Max_E2, int dbv_end_Point, int dbv_max_point, int Step_Value, DataGridView datagridview);

        void Calculate_Delta_E4_From_x_y_Lv(ref int dgv_startindex,int max_index, DataGridView datagridview);

        bool IsMultiChannel();

        bool IsCAConnected();
    }
}
