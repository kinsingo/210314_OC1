using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public partial class DP150_Single_Engineerig_Mornitoring_Mode : Form
    {
        public XYLv[,] Limit = new XYLv[14, 9]; //14ea Bands , 9ea Gray-points

        public RGB[,] All_band_gray_Gamma = new RGB[14, 8]; //14ea Bands , 8ea Gray-points


        private static DP150_Single_Engineerig_Mornitoring_Mode Instance;
        public static DP150_Single_Engineerig_Mornitoring_Mode getInstance()
        {
            if (Instance == null)
                Instance = new DP150_Single_Engineerig_Mornitoring_Mode();

            return Instance;
        }

        public static bool IsIstanceNull()
        {
            if (Instance == null)
                return true;
            else
                return false;
        }

        public static void DeleteInstance()
        {
            Instance = null;
        }

        private DP150_Single_Engineerig_Mornitoring_Mode()
        {
            InitializeComponent();
        }

        private void DP150_Single_Engineerig_Mornitoring_Mode_Load(object sender, EventArgs e)
        {
            f1().OC_Param_load();
            BackColor = f1().current_model.Get_Back_Ground_Color();
            Engineer_Mode_Grid_Tema_Change();

            foreach (DataGridViewColumn column in this.dataGridView_OC_param.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewColumn column in this.dataGridView_Band_OC_Viewer.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //Get First Limit Values
            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    Limit[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[10].Value);
                    Limit[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[11].Value);
                    Limit[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[12].Value);
                }
            }
        }


        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button_Save_OC_Param_To_Excel_File_Click(object sender, EventArgs e)
        {

        }

      
        public void Band_Radiobuttion_Select(int band)
        {
                switch (band)
                {
                    case 0:
                        radiobutton_Band0.Checked = true;
                        break;
                    case 1:
                        radiobutton_Band1.Checked = true;
                        break;
                    case 2:
                        radiobutton_Band2.Checked = true;
                        break;
                    case 3:
                        radiobutton_Band3.Checked = true;
                        break;
                    case 4:
                        radiobutton_Band4.Checked = true;
                        break;
                    case 5:
                        radiobutton_Band5.Checked = true;
                        break;
                    case 6:
                        radiobutton_Band6.Checked = true;
                        break;
                    case 7:
                        radiobutton_Band7.Checked = true;
                        break;
                    case 8:
                        radiobutton_Band8.Checked = true;
                        break;
                    case 9:
                        radiobutton_Band9.Checked = true;
                        break;
                    case 10:
                        radiobutton_Band10.Checked = true;
                        break;
                    case 11:
                        radiobutton_AOD0.Checked = true;
                        break;
                    case 12:
                        radiobutton_AOD1.Checked = true;
                        break;
                    case 13:
                        radiobutton_AOD2.Checked = true;
                        break;
                    default:
                        break;
                }
        }

        private int Get_Current_Band()
        {
            if (radiobutton_Band0.Checked)
                return 0;
            else if (radiobutton_Band1.Checked)
                return 1;
            else if (radiobutton_Band2.Checked)
                return 2;
            else if (radiobutton_Band3.Checked)
                return 3;
            else if (radiobutton_Band4.Checked)
                return 4;
            else if (radiobutton_Band5.Checked)
                return 5;
            else if (radiobutton_Band6.Checked)
                return 6;
            else if (radiobutton_Band7.Checked)
                return 7;
            else if (radiobutton_Band8.Checked)
                return 8;
            else if (radiobutton_Band9.Checked)
                return 9;
            else if (radiobutton_Band10.Checked)
                return 10;
            else if (radiobutton_AOD0.Checked)
                return 11;
            else if (radiobutton_AOD1.Checked)
                return 12;
            else if (radiobutton_AOD2.Checked)
                return 13;
            else
                return 0;
        }

        private void Copy_Data_Grid_View(int Offset_Row)
        {
            for (int j = 0; j < dataGridView_Band_OC_Viewer.ColumnCount; j++)
            {
                for (int i = 2; i < dataGridView_Band_OC_Viewer.RowCount; i++)
                {
                    dataGridView_Band_OC_Viewer.Rows[i].Cells[j].Value = dataGridView_OC_param.Rows[i + Offset_Row].Cells[j].Value;
                }
            }
        }

        private Form1 f1()
        {
            return (Form1)Application.OpenForms["Form1"];
        }

        private void Engineer_Mode_Grid_Tema_Change()
        {
            dataGridView_OC_param.Columns[0].Width = 80;
            dataGridView_Band_OC_Viewer.Columns[0].Width = 80;
            dataGridView_Gamma_Vreg1_Diff.Columns[0].Width = 80;

            for (int i = 1; i <= 3; i++) //Gamma R,G,B
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 40;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 40;
            }

            for (int i = 1; i <= 4; i++)
            {
                dataGridView_Gamma_Vreg1_Diff.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_Gamma_Vreg1_Diff.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Gamma_Vreg1_Diff.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Gamma_Vreg1_Diff.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Gamma_Vreg1_Diff.Columns[i].Width = 40;
            }
            for (int i = 5; i <= 8; i++)
            {
                dataGridView_Gamma_Vreg1_Diff.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                dataGridView_Gamma_Vreg1_Diff.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Gamma_Vreg1_Diff.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Gamma_Vreg1_Diff.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Gamma_Vreg1_Diff.Columns[i].Width = 40;
            }


            for (int i = 4; i <= 6; i++) //Measure X,Y,Lv
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 55;
            }

            for (int i = 7; i <= 9; i++) //Target X,Y,Lv
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 55;
            }

            for (int i = 10; i <= 12; i++) //Limit X,Y,Lv
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 55;
            }

            for (int i = 13; i <= 15; i++) //Extension X,Y,Applied
            {
                dataGridView_OC_param.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_Band_OC_Viewer.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer.Columns[i].Width = 55;
            }

            //Loop
            dataGridView_OC_param.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param.Columns[16].Width = 40;

            dataGridView_Band_OC_Viewer.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_Band_OC_Viewer.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_Band_OC_Viewer.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_Band_OC_Viewer.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_Band_OC_Viewer.Columns[16].Width = 40;
        }

        public void dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB()
        {
            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[1].Value = dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[2].Value = dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[3].Value = dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value;
                }
            }
        }

        public void Gamma_Vreg1_Diff_Clear()
        {
            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[6].Value = string.Empty;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[7].Value = string.Empty;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[8].Value = string.Empty;
                }
            }
        }

        public void Copy_Previous_Band_Gamma(int band)
        {
            int Prev_Offset_Row = 8 * (band - 1);
            int Offset_Row = 8 * band;
            for (int j = 1; j <= 3; j++)
            {
                for (int i = 2; i < dataGridView_Band_OC_Viewer.RowCount; i++)
                {
                    dataGridView_OC_param.Rows[i + Offset_Row].Cells[j].Value = dataGridView_OC_param.Rows[i + Prev_Offset_Row].Cells[j].Value;
                }
            }
        }

        public void Get_Band_Gray_Gamma(RGB[,] Gamma, int band)
        {
            for (int gray = 0; gray < 8; gray++)
            {
                Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value.ToString());
                Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value.ToString());
                Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value.ToString());
                Gamma[band, gray].String_Update_From_int();

            }
        }



        public void GridView_Measure_Applied_Loop_Area_Data_Clear()
        {
            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[4].Value = string.Empty;
                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[5].Value = string.Empty;
                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[6].Value = string.Empty;

                    dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[6].Value = string.Empty;

                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[15].Value = string.Empty;
                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[16].Value = string.Empty;

                    dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[16].Value = string.Empty;
                }
            }
        }

        public void Engineering_Mode_DataGridview_ReadOnly(bool ReadOnly)
        {
            this.dataGridView_OC_param.ReadOnly = ReadOnly;
        }


        private void Get_OC_Param_By_Gray_DP150(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
            , ref double Limit_Y, ref double Limit_Lv, ref double Extension_X, ref double Extension_Y)
        {
            Gamma_R = Convert.ToInt16(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value.ToString());
            Gamma_G = Convert.ToInt16(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value.ToString());
            Gamma_B = Convert.ToInt16(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value.ToString());

            Target_X = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[7].Value.ToString());
            Target_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[8].Value.ToString());
            Target_Lv = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[9].Value.ToString());

            Limit_X = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[10].Value.ToString());
            Limit_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[11].Value.ToString());
            Limit_Lv = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[12].Value.ToString());

            Extension_X = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[13].Value.ToString());
            Extension_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[14].Value.ToString());
        }

        public void Get_Gamma_Only_DP150(int band, int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B)
        {
            Gamma_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value.ToString());
            Gamma_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value.ToString());
            Gamma_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value.ToString());
        }

        public void Set_OC_Gamma_DP150(int band, int gray, int Gamma_R, int Gamma_G, int Gamma_B)
        {
            dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value = Gamma_R;
            dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value = Gamma_G;
            dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value = Gamma_B;
        }

        public void Set_Sub_OC_Gamma_DP150()
        {
            int band = Get_Current_Band();
            for (int gray = 0; gray < 12; gray++)
            {
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value = dataGridView_OC_param.Rows[band * 9 + (gray + 2)].Cells[1].Value;
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value = dataGridView_OC_param.Rows[band * 9 + (gray + 2)].Cells[2].Value;
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value = dataGridView_OC_param.Rows[band * 9 + (gray + 2)].Cells[3].Value;
            }
        }

        public void Updata_Sub_To_Main_GridView(int band, int gray)
        {
            int Offset_Row = 8 * band;
            for (int j = 0; j < dataGridView_Band_OC_Viewer.ColumnCount; j++)
            {
                dataGridView_OC_param.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[j].Value;
            }
        }

        public void Get_OC_Param_DP116(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
            , ref double Limit_Y, ref double Limit_Lv, ref double Extension_X, ref double Extension_Y)
        {
            Gamma_R = Convert.ToInt16(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value.ToString());
            Gamma_G = Convert.ToInt16(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value.ToString());
            Gamma_B = Convert.ToInt16(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value.ToString());

            Target_X = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[7].Value.ToString());
            Target_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[8].Value.ToString());
            Target_Lv = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[9].Value.ToString());

            Limit_X = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[10].Value.ToString());
            Limit_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[11].Value.ToString());
            Limit_Lv = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[12].Value.ToString());

            Extension_X = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[13].Value.ToString());
            Extension_Y = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[14].Value.ToString());
        }

        //Single
        public string Get_BX_GXXX_By_Gray_DP116(int gray)
        {
            return dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[0].Value.ToString();
        }

        public void DP150_Get_All_Band_Gray_Gamma(RGB[,] Gamma)
        {
            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value.ToString());
                    Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value.ToString());
                    Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value.ToString());
                    Gamma[band, gray].String_Update_From_int();

                }
            }
        }


        public void Set_OC_Param_DP150(int gray, int Gamma_R, int Gamma_G, int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, int loop_count, string Extension_Applied)
        {
            //Set Param according to gray
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value = Gamma_R;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value = Gamma_G;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value = Gamma_B;

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[4].Value = Measure_X;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[5].Value = Measure_Y;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[6].Value = Measure_Lv;

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[15].Value = Extension_Applied;

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[16].Value = loop_count;
        }

        public void dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(int band, int Vreg1)
        {
            if (band < 8) dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (0 + 2)].Cells[4].Value = Vreg1;

        }

        public void dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(int band, int Diff_Vreg1)
        {
            if (band < 8) dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (0 + 2)].Cells[8].Value = Diff_Vreg1;
        }

        public void dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(int band, int gray, int Diff_R, int Diff_G, int Diff_B)
        {
            dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[5].Value = Diff_R;
            dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[6].Value = Diff_G;
            dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (gray + 2)].Cells[7].Value = Diff_B;
        }

        private void button_Read_OC_Param_From_Excel_File_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.current_model.Read_OC_Param_From_Excel_File();
            Band_Radiobuttion_Select(0);//Select Band 0
        }

        private void Limit_Update(double Ratio, bool XY)
        {
            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    if (XY)
                    {
                        if ((Limit[band, gray].double_X * Ratio) >= 0.99 || Limit[band, gray].double_X >= 0.99) dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[10].Value = 1;
                        else dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[10].Value = Limit[band, gray].double_X * Ratio;

                        if ((Limit[band, gray].double_Y * Ratio) >= 0.99 || Limit[band, gray].double_Y >= 0.99) dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[11].Value = 1;
                        else dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[11].Value = Limit[band, gray].double_Y * Ratio;
                    }
                    else //Lv
                    {
                        dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[12].Value = Limit[band, gray].double_Lv * Ratio;
                    }
                }
            }

            //update main_Grid_view from sub_Grid_View
            int Current_Band = this.Get_Current_Band();
            int Offset_Row = 8 * Current_Band;
            Copy_Data_Grid_View(Offset_Row);
        }





        private void Set_OC_Param_By_Gray_DP150(int gray, int Gamma_R, int Gamma_G, int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double loop_count, string Extension_Applied)
        {
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value = Gamma_R;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value = Gamma_G;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value = Gamma_B;

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[4].Value = Measure_X;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[5].Value = Measure_Y;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[6].Value = Measure_Lv;

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[15].Value = Extension_Applied;

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[16].Value = loop_count;
        }


        private void radiobutton_Band0_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 0;
            if (radiobutton_Band0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 1;
            if (radiobutton_Band1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 2;
            if (radiobutton_Band2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 3;
            if (radiobutton_Band3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 4;
            if (radiobutton_Band4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 5;
            if (radiobutton_Band5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 6;
            if (radiobutton_Band6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band7_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 7;
            if (radiobutton_Band7.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band8_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 8;
            if (radiobutton_Band8.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band9_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 9;
            if (radiobutton_Band9.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band10_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 10;
            if (radiobutton_Band10.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_AOD0_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 11;
            if (radiobutton_AOD0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_AOD1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 12;
            if (radiobutton_AOD1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_AOD2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 13;
            if (radiobutton_AOD2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        public void RadioButton_All_Enable(bool enable)
        {
            radiobutton_Band0.Enabled = enable;
            radiobutton_Band1.Enabled = enable;
            radiobutton_Band2.Enabled = enable;
            radiobutton_Band3.Enabled = enable;
            radiobutton_Band4.Enabled = enable;
            radiobutton_Band5.Enabled = enable;
            radiobutton_Band6.Enabled = enable;
            radiobutton_Band7.Enabled = enable;
            radiobutton_Band8.Enabled = enable;
            radiobutton_Band9.Enabled = enable;
            radiobutton_Band10.Enabled = enable;

            radiobutton_AOD0.Enabled = enable;
            radiobutton_AOD1.Enabled = enable;
            radiobutton_AOD2.Enabled = enable;
        }

        private void radioButton_Limit_Ratio_150_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150.Checked)
            {
                Limit_Update(1.5, true);
            }
        }


        private void radioButton_Limit_Ratio_100_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100.Checked)
            {
                Limit_Update(1, true);
            }
        }

        private void radioButton_Limit_Ratio_80_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80.Checked)
            {
                Limit_Update(0.8, true);
            }
        }

        private void radioButton_Limit_Ratio_60_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60.Checked)
            {
                Limit_Update(0.6, true);
            }
        }

        private void radioButton_Limit_Lv_Ratio_150_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150.Checked)
            {
                Limit_Update(1.5, false);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100.Checked)
            {
                Limit_Update(1, false);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80.Checked)
            {
                Limit_Update(0.8, false);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60.Checked)
            {
                Limit_Update(0.6, false);
            }
        }

        private void dataGridView_OC_param_KeyDown_1(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (e.KeyData == (Keys.Control | Keys.V)) f1.PasteInData(ref this.dataGridView_OC_param);
        }

        private void button_Gamma_Read_Condition1_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Second_Model_Option_Form.getInstance().Show();
            System.Windows.Forms.Application.DoEvents();
            //Gamma
            f1.GB_Status_AppendText_Nextline("#Gamma Read(Condition1) Start", Color.Blue);
            Single_Gamma_Read();
            f1.GB_Status_AppendText_Nextline("#Gamma Read(Condition1) End", Color.Blue);

            //Vreg1(Read and Display)
            f1.GB_Status_AppendText_Nextline("#Vreg1(Condition1) Start", Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xB0 0x2B #Para Offset : 44", Color.Black);
            f1.MX_OTP_Read(43, 17, "B1");
            Thread.Sleep(200);
            string Vreg1_CMD = "mipi.write 0x39 0xB1";
            for (int i = 0; i < 17; i++) Vreg1_CMD += (" 0x" + f1.dataGridView1.Rows[i].Cells[1].Value.ToString());
            f1.GB_Status_AppendText_Nextline(Vreg1_CMD + " #VREG1 Set1 Band0~10 for Normal", Color.Black);
            f1.GB_Status_AppendText_Nextline("#Vreg1(Condition1) End", Color.Blue);

            //Black(Read and Display)
            f1.GB_Status_AppendText_Nextline("#Black(REF2047) Start", Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xB0 0x15 #Para Offset : 22", Color.Black);
            f1.MX_OTP_Read(21, 1, "B1");
            string REF2047 = "mipi.write 0x39 0xB1 0x" + f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            f1.GB_Status_AppendText_Nextline(REF2047 + " #REF2047(Black)", Color.Black);
            f1.GB_Status_AppendText_Nextline("#Black(REF2047) End", Color.Blue);
        }

        private void Single_Gamma_Read()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP150_EA9152 DP150 = DP150_EA9152.getInstance();
            for (int band = 0; band < 14; band++)
            {
                string Mipi_CMD = "mipi.write 0x39 " + DP150.Get_Gamma_Register_Hex_String(band); //"mipi.write 0x39 0xXX"
                string Band_Address = DP150.Get_Gamma_Register_Hex_String(band).Remove(0, 2); //"XX"

                if (band == 11) f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0x0A #AOD1", Color.Black); //AOD1
                else if (band == 12) f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0x32 #AOD2", Color.Black);//AOD2
                else if (band == 13) f1.IPC_Quick_Send_And_Show("mipi.write 0x15 0xB0 0x5A #AOD3", Color.Black);//AOD3

                f1.OTP_Read(40, Band_Address);
                for (int i = 0; i < 40; i++) Mipi_CMD += (" 0x" + f1.dataGridView1.Rows[i].Cells[1].Value.ToString());
                f1.IPC_Quick_Send_And_Show(Mipi_CMD, Color.Black);
            }

        }


        private void button_Gamma_Down_Condition_1_Single_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Second_Model_Option_Form.getInstance().Show();
            System.Windows.Forms.Application.DoEvents();
            
            //Gamma (Read and Send and Display)
            f1.GB_Status_AppendText_Nextline("#Gamma Down(Condition1) Start", Color.Blue);
            Single_Gamma_Down();
            f1.GB_Status_AppendText_Nextline("#Gamma Down(Condition1) End", Color.Blue);

            //Vreg1 (Read and Display)
            f1.GB_Status_AppendText_Nextline("#Vreg1(Condition1) Start", Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xB0 0x2B #Para Offset : 44", Color.Black);
            f1.MX_OTP_Read(43, 17, "B1");
            Thread.Sleep(200);
            string Vreg1_CMD = "mipi.write 0x39 0xB1";
            for (int i = 0; i < 17; i++) Vreg1_CMD += (" 0x" + f1.dataGridView1.Rows[i].Cells[1].Value.ToString());
            f1.GB_Status_AppendText_Nextline(Vreg1_CMD + " #VREG1 Set1 Band0~10 for Normal", Color.Black);
            f1.GB_Status_AppendText_Nextline("#Vreg1(Condition1) End", Color.Blue);

            //Black (Read and Display)
            f1.GB_Status_AppendText_Nextline("#Black(REF2047) Start", Color.Blue);
            f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xB0 0x15 #Para Offset : 22", Color.Black);
            f1.MX_OTP_Read(21, 1, "B1");
            string REF2047 = "mipi.write 0x39 0xB1 0x" + f1.dataGridView1.Rows[0].Cells[1].Value.ToString();
            f1.GB_Status_AppendText_Nextline(REF2047 + " #REF2047(Black)", Color.Black);
            f1.GB_Status_AppendText_Nextline("#Black(REF2047) End", Color.Blue);
        }

        private void Single_Gamma_Down()
        {
            Second_Model_Option_Form Second_Model = (Second_Model_Option_Form)Application.OpenForms["Second_Model_Option_Form"];
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            DP150_Get_All_Band_Gray_Gamma(All_band_gray_Gamma);

            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    if (band == 11) f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xB0 0x0A #AOD1", Color.Black); //AOD1
                    else if (band == 12) f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xB0 0x32 #AOD2", Color.Black);//AOD2
                    else if (band == 13) f1.GB_Status_AppendText_Nextline("mipi.write 0x15 0xB0 0x5A #AOD3", Color.Black);//AOD3

                    string temp = Second_Model.Update_and_Send_All_Band_Gray_Gamma(All_band_gray_Gamma, All_band_gray_Gamma[band, gray], band, gray, true, "00", "00", "00", "00", "00", "00");
                    f1.GB_Status_AppendText_Nextline(temp, Color.Black);
                }
            }
        }







    }
}
