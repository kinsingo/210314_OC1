using System.Windows.Forms;

namespace PNC_Csharp.CA_Multi_Channels
{
    class GridView_Control
    {
        DataGridView dataGridView_CA_Measure;
        DataGridView dataGridView_CA1_5;
        DataGridView dataGridView_CA6_10;

        public GridView_Control(DataGridView _dataGridView_CA_Measure, DataGridView _dataGridView_CA1_5,DataGridView _dataGridView_CA6_10)
        {
            dataGridView_CA_Measure = _dataGridView_CA_Measure;
            dataGridView_CA1_5 = _dataGridView_CA1_5;
            dataGridView_CA6_10 = _dataGridView_CA6_10;
        }

        public void Initalize_GridView()
        {
            dataGridView_CA_Measure_initial_setting();
            dataGridView_CA1_5_initial_setting();
            dataGridView_CA6_10_initial_setting();
        }

        private void dataGridView_CA_Measure_initial_setting()
        {
            dataGridView_CA_Measure.EnableHeadersVisualStyles = false;
            dataGridView_CA_Measure.ReadOnly = true;
            dataGridView_CA_Measure.Columns.Add("Channel", "Channel");
            dataGridView_CA_Measure.Columns.Add("X", "X");
            dataGridView_CA_Measure.Columns.Add("Y", "Y");
            dataGridView_CA_Measure.Columns.Add("Lv", "Lv");
            dataGridView_CA_Measure.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;


            dataGridView_CA_Measure.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_CA_Measure.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView_CA_Measure.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dataGridView_CA_Measure.Columns[0].HeaderCell.Style.BackColor = System.Drawing.Color.LightGray;

            for (int col = 1; col <= 3; col++)
            {
                dataGridView_CA_Measure.Columns[col].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_CA_Measure.Columns[col].HeaderCell.Style.BackColor = System.Drawing.Color.Cyan;
                dataGridView_CA_Measure.Columns[col].Width = 50;
            }

            foreach (DataGridViewColumn column in dataGridView_CA_Measure.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }


        private void dataGridView_CA1_5_initial_setting()
        {
            dataGridView_CA1_5.EnableHeadersVisualStyles = false;
            dataGridView_CA1_5.ReadOnly = true;
            dataGridView_CA1_5.Columns.Add("CA1", "CA1");
            dataGridView_CA1_5.Columns.Add("CA2", "CA2");
            dataGridView_CA1_5.Columns.Add("CA3", "CA3");
            dataGridView_CA1_5.Columns.Add("CA4", "CA4");
            dataGridView_CA1_5.Columns.Add("CA5", "CA5");
            dataGridView_CA1_5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_CA1_5.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView_CA1_5.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
            dataGridView_CA1_5.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            for (int i = 0; i < dataGridView_CA1_5.ColumnCount; i++)
            {
                dataGridView_CA1_5.Rows[0].Cells[i].Value = "-";
                dataGridView_CA1_5.Rows[0].Cells[i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void dataGridView_CA6_10_initial_setting()
        {
            dataGridView_CA6_10.EnableHeadersVisualStyles = false;
            dataGridView_CA6_10.ReadOnly = true;
            dataGridView_CA6_10.Columns.Add("CA6", "CA6");
            dataGridView_CA6_10.Columns.Add("CA7", "CA7");
            dataGridView_CA6_10.Columns.Add("CA8", "CA8");
            dataGridView_CA6_10.Columns.Add("CA9", "CA9");
            dataGridView_CA6_10.Columns.Add("CA10", "CA10");
            dataGridView_CA6_10.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_CA6_10.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView_CA6_10.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
            dataGridView_CA6_10.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            for (int i = 0; i < dataGridView_CA6_10.ColumnCount; i++)
            {
                dataGridView_CA6_10.Rows[0].Cells[i].Value = "-";
                dataGridView_CA6_10.Rows[0].Cells[i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
    }
}
