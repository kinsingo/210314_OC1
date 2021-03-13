using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices; // (Dll 사용위해 필요)
using BSQH_Csharp_Library;
using System.Windows.Data;

namespace PNC_Csharp
{
    public partial class Meta_Engineer_Mornitoring_Mode : Form
    {
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Test_checkBox_Get_HBM_Equation(int band, double[] HBM_Gamma_Voltage_G, double[] HBM_Gray_Target, double[] G255_Band_Target, int[] G255_Band_Gamma_G
    , double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_E7(double ELVDD, int dec_FV1);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_F7(double ELVDD, int dec_VCI1);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_VREG1_REF818_volt(double E7, double F7, int Dec_VREG1_REF818);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_VREG1_REF614_volt(double E7, double F7, int Dec_VREG1_REF614);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_VREG1_REF409_volt(double E7, double F7, int Dec_VREG1_REF409);
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Get_VREG1_REF205_volt(double E7, double F7, int Dec_VREG1_REF205);


        public XYLv[,] Limit = new XYLv[10, 8]; //10ea Bands , 8ea Gray-points
        private RGB[,] Gamma = new RGB[10, 8];//10ea Bands , 8ea Gray-points (Add on 191028)
        private XYLv[,] Target = new XYLv[10, 8];//Add on 191105
        private RGB_Double[,] Gamma_Voltage = new RGB_Double[10, 8];//10ea Bands , 8ea Gray-points (Add on 191028)

        Size Init_Form_Size;

        private static Meta_Engineer_Mornitoring_Mode Instance;
        public static Meta_Engineer_Mornitoring_Mode getInstance()
        {
            if (Instance == null)
                Instance = new Meta_Engineer_Mornitoring_Mode();
            return Instance;
        }
        private Meta_Engineer_Mornitoring_Mode()
        {
            InitializeComponent();
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

        public void Form_Hide()
        {
            this.Visible = false;
        }

        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Meta_Engineer_Mornitoring_Mode_Load(object sender, EventArgs e)
        {
            f1().OC_Param_load();
            BackColor = f1().current_model.Get_Back_Ground_Color();
            Init_Form_Size = this.Size;
            Engineer_Mode_Grid_Tema_Change();
            Meta_Single_Engineerig_Mornitoring_Mode_Load(sender, e);
        }

        private void radioButton_Diff_Show_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Diff_Show.Checked)
            {
                this.Size = Init_Form_Size;
            }
        }

        private void radioButton_Diff_Hide_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Diff_Hide.Checked)
            {
                this.Size = new Size(this.Size.Width - dataGridView_Gamma_Vreg1_Diff.Size.Width - 5, this.Size.Height);
            }
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
            dataGridView_RGB_Vdata.Columns[0].Width = 80;

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

                dataGridView_RGB_Vdata.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_RGB_Vdata.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_RGB_Vdata.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_RGB_Vdata.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                //dataGridView_RGB_Vdata.Columns[i].Width = 20;
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
            for (int band = 0; band < 10; band++)
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
            for (int band = 0; band < 10; band++)
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

        public void Gamma_Vdata_Clear()
        {
            for (int band = 0; band < 10; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {

                    dataGridView_RGB_Vdata.Rows[band * 8 + (gray + 2)].Cells[1].Value = string.Empty;
                    dataGridView_RGB_Vdata.Rows[band * 8 + (gray + 2)].Cells[2].Value = string.Empty;
                    dataGridView_RGB_Vdata.Rows[band * 8 + (gray + 2)].Cells[3].Value = string.Empty;
                }
            }
        }


        public void GridView_Measure_Applied_Loop_Area_Data_Clear()
        {
            for (int band = 0; band < 10; band++)
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


        private void Meta_Single_Engineerig_Mornitoring_Mode_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in this.dataGridView_OC_param.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewColumn column in this.dataGridView_Band_OC_Viewer.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //Get First Limit Values
            for (int band = 0; band < 10; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    Limit[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[10].Value);
                    Limit[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[11].Value);
                    Limit[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[12].Value);
                }
            }
            radioButton_Diff_Hide_CheckedChanged(sender, e);
        }



        public void Engineering_Mode_DataGridview_ReadOnly(bool ReadOnly)
        {
            this.dataGridView_OC_param.ReadOnly = ReadOnly;
        }


        private void Get_OC_Param_By_Gray_Meta(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
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

        public void Get_Gamma_Only_Meta(int band, int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B)
        {
            Gamma_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value.ToString());
            Gamma_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value.ToString());
            Gamma_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value.ToString());
        }

        public void Set_OC_Gamma_Meta(int band, int gray, int Gamma_R, int Gamma_G, int Gamma_B)
        {
            dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value = Gamma_R;
            dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value = Gamma_G;
            dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value = Gamma_B;
        }

        public void Set_Sub_OC_Gamma_Meta()
        {
            int band = Get_Current_Band();
            for (int gray = 0; gray < 8; gray++)
            {
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value = dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value;
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value = dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value;
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value = dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value;
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

        public void Get_OC_Param_Meta(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
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
        public string Get_BX_GXXX_By_Gray_Meta(int gray)
        {
            return dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[0].Value.ToString();
        }

        public void Meta_Get_Band_Gray_Gamma(RGB[,] Gamma,int band)
        {
            for (int gray = 0; gray < 8; gray++)
            {
                Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value.ToString());
                Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value.ToString());
                Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value.ToString());
                Gamma[band, gray].String_Update_From_int();

            }   
        }

        public void Meta_Get_All_Band_Gray_Gamma(RGB[,] Gamma)
        {
            for (int band = 0; band < 10; band++)
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

        public void Set_OC_Param_Meta(int gray, int Gamma_R, int Gamma_G, int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, int loop_count, string Extension_Applied)
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
            if (band < 10) dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (0 + 2)].Cells[4].Value = Vreg1;

        }

        public void dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(int band, int Diff_Vreg1)
        {
            if (band < 10) dataGridView_Gamma_Vreg1_Diff.Rows[band * 8 + (0 + 2)].Cells[8].Value = Diff_Vreg1;
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
            for (int band = 0; band < 10; band++)
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

        private void Set_OC_Param_By_Gray_Meta(int gray, int Gamma_R, int Gamma_G, int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double loop_count, string Extension_Applied)
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

        private void dataGridView_OC_param_KeyDown(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (e.KeyData == (Keys.Control | Keys.V)) f1.PasteInData(ref this.dataGridView_OC_param);
        }

        private void dataGridView_Band_OC_Viewer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        public double Get_Vreg1_Voltage(int Dec_Vreg1,double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt
            , double VREG1_REF205_volt, double F7)
        {
            if (Dec_Vreg1 == 0) return 6.7;//DDVDH
            else if (Dec_Vreg1 == 1) return F7;//ELVDD + VCI1_LVL[5:0]
            else if (Dec_Vreg1 == 205) return VREG1_REF205_volt;
            else if (Dec_Vreg1 == 409) return VREG1_REF409_volt;
            else if (Dec_Vreg1 == 614) return VREG1_REF614_volt;
            else if (Dec_Vreg1 == 818) return VREG1_REF818_volt;
            else if (Dec_Vreg1 == 1023) return E7;//ELVDD - FV1_LVL[5:0]
            else
            {
                if (Dec_Vreg1 < 81)
                {
                    return 0;
                }
                else if (Dec_Vreg1 < 181)
                {
                    double Offset = 331.5 + (Dec_Vreg1 - 82) * 1.5;
                    double Offset_181 = 480;
                    return F7 + (VREG1_REF205_volt - F7) * (Offset / Offset_181);
                }
                else if (Dec_Vreg1 < 205)
                {
                    double Offset = 481.2 + (Dec_Vreg1 - 182) * 1.2;
                    double Offset_205 = 508.8;
                    return F7 + (VREG1_REF205_volt - F7) * (Offset / Offset_205);
                }
                else if (Dec_Vreg1 < 409)
                {
                    double Offset = Dec_Vreg1 - 205.0;
                    double Offset_409 = 204.0; // = 409 - 205
                    return VREG1_REF205_volt + (VREG1_REF409_volt - VREG1_REF205_volt) * (Offset / Offset_409);
                }
                else if (Dec_Vreg1 < 614)
                {
                    double Offset = Dec_Vreg1 - 409;
                    double Offset_614 = 205.0; // = 614 - 409
                    return VREG1_REF409_volt + (VREG1_REF614_volt - VREG1_REF409_volt) * (Offset / Offset_614);
                }
                else if (Dec_Vreg1 < 818)
                {
                    double Offset = Dec_Vreg1 - 614;
                    double Offset_818 = 204.0; // = 818 - 614
                    return VREG1_REF614_volt + (VREG1_REF818_volt - VREG1_REF614_volt) * (Offset / Offset_818);
                }
                else if (Dec_Vreg1 < 1023)
                {
                    double Offset = Dec_Vreg1 - 818;
                    double Offset_1023 = 205.0; // = 1023 - 818
                    return VREG1_REF818_volt + (E7 - VREG1_REF818_volt) * (Offset / Offset_1023);
                }
                else
                {
                    return 999; //Will Not Happen
                }
            }
           
        }

        public int Get_Vreg1_Dec(double Vreg1_Voltage, double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt
            , double VREG1_REF205_volt, double F7)
        {
            if ((Vreg1_Voltage > (6.7 - 0.0001)) && (Vreg1_Voltage < (6.7 + 0.0001))) return 0;//DDVDH
            else if ((Vreg1_Voltage > (F7 - 0.0001)) && (Vreg1_Voltage < (F7 + 0.0001))) return 1;//ELVDD + VCI1_LVL[5:0]
            else if ((Vreg1_Voltage > (VREG1_REF205_volt - 0.0001)) && (Vreg1_Voltage < (VREG1_REF205_volt + 0.0001))) return 205;
            else if ((Vreg1_Voltage > (VREG1_REF409_volt - 0.0001)) && (Vreg1_Voltage < (VREG1_REF409_volt + 0.0001))) return 409;
            else if ((Vreg1_Voltage > (VREG1_REF614_volt - 0.0001)) && (Vreg1_Voltage < (VREG1_REF614_volt + 0.0001))) return 614;
            else if ((Vreg1_Voltage > (VREG1_REF818_volt - 0.0001)) && (Vreg1_Voltage < (VREG1_REF818_volt + 0.0001))) return 818;
            else if ((Vreg1_Voltage > (E7 - 0.0001)) && (Vreg1_Voltage < (E7 + 0.0001))) return 1023;//ELVDD - FV1_LVL[5:0]
            else
            {
                if (Vreg1_Voltage > VREG1_REF205_volt)
                {
                    return 205;

                    //double Offset = 481.2 + (Dec_Vreg1 - 182) * 1.2;        
                    //double Offset_205 = 508.8;
                    //return F7 + (VREG1_REF205_volt - F7) * (Offset / Offset_205);

                    //double Offset_205 = 508.8;
                    //double Offset = ((Vreg1_Voltage - F7) / (VREG1_REF205_volt - F7)) * Offset_205;
                    //return ((Offset - 481.2) / 1.2) + 182;//return Dec_Vreg1
                }
                else if (Vreg1_Voltage > VREG1_REF409_volt)
                {
                    //double Offset = Dec_Vreg1 - 205.0;
                    //double Offset_409 = 204.0; // = 409 - 205
                    //return VREG1_REF205_volt + (VREG1_REF409_volt - VREG1_REF205_volt) * (Offset / Offset_409);

                    double Offset_409 = 204.0;
                    double Offset = ((Vreg1_Voltage - VREG1_REF205_volt) / (VREG1_REF409_volt - VREG1_REF205_volt)) * Offset_409;
                    return (int)Offset + 205; //return Dec_Vreg1

                }
                else if (Vreg1_Voltage > VREG1_REF614_volt)
                {
                    //double Offset = Dec_Vreg1 - 409;
                    //double Offset_614 = 205.0; // = 614 - 409
                    //return VREG1_REF409_volt + (VREG1_REF614_volt - VREG1_REF409_volt) * (Offset / Offset_614);

                    double Offset_614 = 205.0; // = 614 - 409
                    double Offset = ((Vreg1_Voltage - VREG1_REF409_volt) / (VREG1_REF614_volt - VREG1_REF409_volt)) * Offset_614;
                    return (int)Offset + 409; //return Dec_Vreg1

                }
                else if (Vreg1_Voltage > VREG1_REF818_volt)
                {
                    //double Offset = Dec_Vreg1 - 614;
                    //double Offset_818 = 204.0; // = 818 - 614
                    //return VREG1_REF614_volt + (VREG1_REF818_volt - VREG1_REF614_volt) * (Offset / Offset_818);
                    double Offset_818 = 204.0;
                    double Offset = ((Vreg1_Voltage - VREG1_REF614_volt) / (VREG1_REF818_volt - VREG1_REF614_volt)) * Offset_818;
                    return (int)Offset + 614; //return Dec_Vreg1

                }
                else if (Vreg1_Voltage > (E7 + 0.0001))
                {
                    //double Offset = Dec_Vreg1 - 818;
                    //double Offset_1023 = 205.0; // = 1023 - 818
                    //return VREG1_REF818_volt + (E7 - VREG1_REF818_volt) * (Offset / Offset_1023);
                    double Offset_1023 = 205.0;
                    double Offset = ((Vreg1_Voltage - VREG1_REF818_volt) / (E7 - VREG1_REF818_volt)) * Offset_1023;
                    return (int)Offset + 818; 

                }
                else
                {
                    return 9999; //Will Not Happen
                }
            }
        }

        public double Get_Normal_Gamma_Voltage(double L, double H, int Gamma_Dec)
        {
            return L + (H - L) * ((Gamma_Dec + 1.0) / 512.0); 
        }

        public double Get_AM2_Voltage(double F7, double Vreg1_Voltage, int Gamma_Dec)
        {
            return F7 + (Vreg1_Voltage - F7) * ((Gamma_Dec + 189.0) / 700.0);
        }

        public int Get_Gamma_From_Normal_Voltage(double L, double H, double Vdata)
        {
            return Convert.ToInt16((512.0 * (Vdata - L) / (H - L))-1);
            //return L + (H - L) * ((Gamma_Dec + 1.0) / 512.0);
        }

        public int Get_Gamma_From_AM2_Voltage(double F7, double Vreg1_Voltage, double Vdata)
        {
            return Convert.ToInt16((700.0 * (Vdata - F7) / (Vreg1_Voltage - F7)) - 189);
            //return F7 + (Vreg1_Voltage - F7) * ((Gamma_Dec + 189.0) / 700);
        }


        private void button_Calculate_Data_Voltage_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Meta_Form_Show();
            Gamma_Vdata_Clear();
            f1.GB_Status_AppendText_Nextline("*Assumption : ELVDD = 4.6v , DDVDH = 6.7v , GREF_SEL = 1 , GA_INV = 1,VG4_EN =1 , AVAMODE = 0", Color.Red);

            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");

            //Get ELVDD - FV1_LVL[5:0],ELVDD + VCI1_LVL[5:0]
            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            double ELVDD = 4.6;

            string FV1 = D0_Hex[18];
            int dec_FV1 = Convert.ToInt16(FV1, 16);
            if (dec_FV1 >= 42) dec_FV1 = 42;
            double E7 = ELVDD - (0.2 + (dec_FV1 * 0.1)); //ELVDD - FV1_LVL[5:0]
            
            string VCI1 = D0_Hex[16];
            int dec_VCI1 = Convert.ToInt16(VCI1, 16);
            if (dec_VCI1 >= 42) dec_VCI1 = 42;
            double F7 = ELVDD + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]

            //Get VREG1_REF818[5:0],VREG1_REF614[5:0],VREG1_REF409[5:0],VREG1_REF205[5:0]
            //Vreg1_REF818 = F7+(E7-F7)*((222.5+0.5*HEX2DEC(VREG1_REF818_Hex))/254))
            //VREG1_REF614 = F7+(E7-F7)*((206.5+0.5*HEX2DEC(VREG1_REF614_Hex))/254))
            //VREG1_REF409 = F7+(E7-F7)*((182.5+0.5*HEX2DEC(VREG1_REF409_Hex))/254))
            //VREG1_REF205 = F7+(E7-F7)*((154.5+0.5*HEX2DEC(VREG1_REF205_Hex))/254))
            int Dec_VREG1_REF818 = Convert.ToInt16(D0_Hex[1], 16) & 0x3F;
            int Dec_VREG1_REF614 = Convert.ToInt16(D0_Hex[2], 16) & 0x3F;
            int Dec_VREG1_REF409 = Convert.ToInt16(D0_Hex[3], 16) & 0x3F;
            int Dec_VREG1_REF205 = Convert.ToInt16(D0_Hex[4], 16) & 0x3F;

            double VREG1_REF818_volt = F7+(E7-F7)*((222.5+0.5*Dec_VREG1_REF818)/254);
            double VREG1_REF614_volt = F7+(E7-F7)*((206.5+0.5*Dec_VREG1_REF614)/254);
            double VREG1_REF409_volt = F7+(E7-F7)*((182.5+0.5*Dec_VREG1_REF409)/254);
            double VREG1_REF205_volt = F7+(E7-F7)*((154.5+0.5*Dec_VREG1_REF205)/254);

            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) - FV1_LVL(v) = " + E7.ToString(), Color.Blue);       
            f1.GB_Status_AppendText_Nextline("VREG1_REF818(v) = " + VREG1_REF818_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF614(v) = " + VREG1_REF614_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF409(v) = " + VREG1_REF409_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF205(v) = " + VREG1_REF205_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) + VCI1_LVL(v) = " + F7.ToString(), Color.Blue);

            E7 = Get_E7(ELVDD, dec_FV1);
            F7 = Get_F7(ELVDD, dec_VCI1);
            VREG1_REF818_volt = Get_VREG1_REF818_volt(E7, F7, Dec_VREG1_REF818);
            VREG1_REF614_volt = Get_VREG1_REF614_volt(E7, F7, Dec_VREG1_REF614);
            VREG1_REF409_volt = Get_VREG1_REF409_volt(E7, F7, Dec_VREG1_REF409);
            VREG1_REF205_volt = Get_VREG1_REF205_volt(E7, F7, Dec_VREG1_REF205);

            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) - FV1_LVL(v) = " + E7.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("VREG1_REF818(v) = " + VREG1_REF818_volt.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("VREG1_REF614(v) = " + VREG1_REF614_volt.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("VREG1_REF409(v) = " + VREG1_REF409_volt.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("VREG1_REF205(v) = " + VREG1_REF205_volt.ToString(), Color.Red);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) + VCI1_LVL(v) = " + F7.ToString(), Color.Red);

            //Test (Verify Finished ! (Ok))
            /*for(int Vreg1 = 0;Vreg1 < 1030;Vreg1++)
            {
                double Vreg1_Voltage = Get_Vreg1_Voltage(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                f1.GB_Status_AppendText_Nextline("Vreg1_Dec / Voltage = " + Vreg1.ToString() + " / " + Vreg1_Voltage.ToString(), Color.Black);
            }*/
            
            //Get Vreg1/AM1 Voltage for each band
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            Meta_Form.button_Vreg1_Read_Click();
            int[] Dec_Vreg1 = new int[10];
            double[] Vreg1_Voltage = new double[10];
            Dec_Vreg1[0] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B0.Text);
            Dec_Vreg1[1] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B1.Text);
            Dec_Vreg1[2] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B2.Text);
            Dec_Vreg1[3] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B3.Text);
            Dec_Vreg1[4] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B4.Text);
            Dec_Vreg1[5] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B5.Text);
            Dec_Vreg1[6] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B6.Text);
            Dec_Vreg1[7] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B7.Text);
            Dec_Vreg1[8] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B8.Text);
            Dec_Vreg1[9] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B9.Text);
            double[] AM1_RGB_Voltage = new double[10];
            
            for (int band = 0; band < 10; band++)
            {
                Vreg1_Voltage[band] = Get_Vreg1_Voltage(Dec_Vreg1[band], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                AM1_RGB_Voltage[band] = F7 + (Vreg1_Voltage[band] - F7) * (8.0 / 700.0);
                f1.GB_Status_AppendText_Nextline(band.ToString() + ") Vreg1_Dec / Vreg1_Voltage / AM1_Voltage = " + Dec_Vreg1[band].ToString() + " / " + Vreg1_Voltage[band].ToString() + " / " + AM1_RGB_Voltage[band].ToString(), Color.Black);
            }
            
            //Get RGB[,] Gamma = new RGB[10, 8];//10ea Bands , 8ea Gray-points 
            //Get RGB_Double[,] Gamma_Voltage = new RGB_Double[10, 8];//10ea Bands , 8ea Gray-points
            //Set Gamma_Voltage[band,gray] , 10ea Bands , 8ea Gray-points
            RGB Reverse_Gamma = new RGB();
            int OC_row_length = dataGridView_OC_param.RowCount;
            for (int i = 2; i < OC_row_length; i++)
            {
                int band = (i - 2) / 8;
                int gray = (i - 2) % 8;
                Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param.Rows[i].Cells[1].Value);
                Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param.Rows[i].Cells[2].Value);
                Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param.Rows[i].Cells[3].Value);
                Target[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[i].Cells[9].Value);
                //Read Gamma From DataGridView Test OK
                //f1.GB_Status_AppendText_Nextline("F [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Gamma[band, gray].int_R.ToString() + "/" + Gamma[band, gray].int_G.ToString() + "/" + Gamma[band, gray].int_B.ToString(), Color.Blue);

                if (gray == 0)//G255 (AM2)
                {
                    Gamma_Voltage[band, gray].double_R = Get_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma[band, gray].int_R);
                    Gamma_Voltage[band, gray].double_G = Get_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma[band, gray].int_G);
                    Gamma_Voltage[band, gray].double_B = Get_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma[band, gray].int_B);

                    //Reverse Mode (Vdata(AM2) -> Gamma) Test OK 
                    //Reverse_Gamma.int_R = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma_Voltage[band, gray].double_R);
                    //Reverse_Gamma.int_G = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma_Voltage[band, gray].double_G);
                    //Reverse_Gamma.int_B = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Gamma_Voltage[band, gray].double_B);
                    //f1.GB_Status_AppendText_Nextline("R [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Reverse_Gamma.int_R.ToString() + "/" + Reverse_Gamma.int_G.ToString() + "/" + Reverse_Gamma.int_B.ToString(), Color.Red);
                    
                }
                else
                {
                    Gamma_Voltage[band, gray].double_R = Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_R, Gamma[band, gray].int_R);
                    Gamma_Voltage[band, gray].double_G = Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_G, Gamma[band, gray].int_G);
                    Gamma_Voltage[band, gray].double_B = Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_B, Gamma[band, gray].int_B);

                    //Reverse Mode (Vdata(Normal) -> Gamma) Test OK
                    //Reverse_Gamma.int_R = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_R, Gamma_Voltage[band, gray].double_R);
                    //Reverse_Gamma.int_G = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_G, Gamma_Voltage[band, gray].double_G);
                    //Reverse_Gamma.int_B = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_B, Gamma_Voltage[band, gray].double_B);
                    //f1.GB_Status_AppendText_Nextline("R [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Reverse_Gamma.int_R.ToString() + "/" + Reverse_Gamma.int_G.ToString() + "/" + Reverse_Gamma.int_B.ToString(), Color.Red);
                    
                }
                dataGridView_RGB_Vdata.Rows[i].Cells[1].Value = Gamma_Voltage[band, gray].double_R;
                dataGridView_RGB_Vdata.Rows[i].Cells[2].Value = Gamma_Voltage[band, gray].double_G;
                dataGridView_RGB_Vdata.Rows[i].Cells[3].Value = Gamma_Voltage[band, gray].double_B;
            }

            //Get RGB
            if (checkBox_Get_RGB_Equation.Checked)
            {
                //C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv 
                //AC = Lv
                SJH_Matrix M = new SJH_Matrix();
                RGB[,] Rev_Gamma = new RGB[10, 8];

                double[] Lv = new double[8];
                double[] Vdata_R = new double[8];
                double[] Vdata_G = new double[8];
                double[] Vdata_B = new double[8];
                double[][] A_R = M.MatrixCreate(8, 8);
                double[][] A_G = M.MatrixCreate(8, 8);
                double[][] A_B = M.MatrixCreate(8, 8);

                //double[][] inv = M.MatrixInverse(A);
                //Get A and Lv
                int count = 0;
                for (int i = 0; i <= 7; i++)
                {
                    Vdata_R[i] = Gamma_Voltage[0, i].double_R;
                    Vdata_G[i] = Gamma_Voltage[0, i].double_G;
                    Vdata_B[i] = Gamma_Voltage[0, i].double_B;
                    Lv[i] = Target[0, i].double_Lv;
                    count = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        A_R[i][count] = Math.Pow(Vdata_R[i], j);
                        A_G[i][count] = Math.Pow(Vdata_G[i], j);
                        A_B[i][count] = Math.Pow(Vdata_B[i], j);
                        count++;
                    }
                }
                //Show A and Lv
                string[] Temp_R = new string[8];
                string[] Temp_G = new string[8];
                string[] Temp_B = new string[8];
                for (int i = 0; i <= 7; i++)
                {
                    Temp_R[i] = (Lv[i].ToString() + "(nit):");
                    Temp_G[i] = (Lv[i].ToString() + "(nit):");
                    Temp_B[i] = (Lv[i].ToString() + "(nit):");
                    for (int j = 0; j <= 7; j++)
                    {
                        Temp_R[i] += (" " + A_R[i][j].ToString());
                        Temp_G[i] += (" " + A_G[i][j].ToString());
                        Temp_B[i] += (" " + A_B[i][j].ToString());
                    }
                    f1.GB_Status_AppendText_Nextline(Temp_R[i], Color.Red);
                    f1.GB_Status_AppendText_Nextline(Temp_G[i], Color.Green);
                    f1.GB_Status_AppendText_Nextline(Temp_B[i], Color.Blue);
                }


                //Get C = inv(A) * Lv
                double[][] Inv_A_R = M.MatrixCreate(8, 8);
                double[][] Inv_A_G = M.MatrixCreate(8, 8);
                double[][] Inv_A_B = M.MatrixCreate(8, 8);

                double[] C_R = new double[8];
                double[] C_G = new double[8];
                double[] C_B = new double[8];
                Inv_A_R = M.MatrixInverse(A_R);
                Inv_A_G = M.MatrixInverse(A_G);
                Inv_A_B = M.MatrixInverse(A_B);
                C_R = M.Matrix_Multiply(Inv_A_R, Lv);
                C_G = M.Matrix_Multiply(Inv_A_G, Lv);
                C_B = M.Matrix_Multiply(Inv_A_B, Lv);

                //Show C
                string temp_C_R = "R :";
                string temp_C_G = "G :";
                string temp_C_B = "B :";
                for (int i = 0; i <= 7; i++)
                {
                    temp_C_R += (" " + C_R[i].ToString());
                    temp_C_G += (" " + C_G[i].ToString());
                    temp_C_B += (" " + C_B[i].ToString());
                }
                f1.GB_Status_AppendText_Nextline(temp_C_R, Color.Red);
                f1.GB_Status_AppendText_Nextline(temp_C_G, Color.Green);
                f1.GB_Status_AppendText_Nextline(temp_C_B, Color.Blue);

                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                double Target_Lv;
                RGB_Double[,] Calculated_Vdata = new RGB_Double[10, 8];
                double Calculated_Target_Lv;
                int iteration;

                for (int gray = 0; gray < 8; gray++)
                {
                    Calculated_Vdata[0, gray].double_R = Gamma_Voltage[0, gray].double_R;
                    Calculated_Vdata[0, gray].double_G = Gamma_Voltage[0, gray].double_G;
                    Calculated_Vdata[0, gray].double_B = Gamma_Voltage[0, gray].double_B;
                }

                for (int band = 1; band < 10; band++)
                {
                    if (band == 2) break;
                    for (int gray = 0; gray < 8; gray++)
                    {
                        Target_Lv = Target[band, gray].double_Lv;
                        if (Target_Lv < 0.1) continue;

                        Calculated_Vdata[band, gray].double_R = 0;
                        Calculated_Vdata[band, gray].double_G = 0;
                        Calculated_Vdata[band, gray].double_B = 0;
                        Calculated_Target_Lv = 0;
                        //for (double Vdata = 3; Vdata <= 4; Vdata += 0.001)
                        //for (double Vdata = E7; Vdata <= F7; Vdata += 0.001)


                        for (double Vdata = Gamma_Voltage[band - 1, gray].double_R; Vdata <= F7; Vdata += 0.001)
                        {
                            if (Vdata > (F7 - 0.002))
                            {
                                Calculated_Vdata[band, gray].double_R = Vdata;
                                break;
                            }

                            Calculated_Target_Lv = 0;
                            iteration = 0;
                            for (int j = 7; j >= 0; j--)
                            {
                                Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                                //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_R[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Red);
                            }

                            //f1.GB_Status_AppendText_Nextline("R_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Red);
                            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, gray].double_R == 0)
                            {
                                Calculated_Vdata[band, gray].double_R = Vdata;
                                break;
                            }
                        }




                        for (double Vdata = Gamma_Voltage[band - 1, gray].double_G; Vdata <= F7; Vdata += 0.001)
                        {
                            if (Vdata > (F7 - 0.002))
                            {
                                Calculated_Vdata[band, gray].double_G = Vdata;
                                break;
                            }
                            Calculated_Target_Lv = 0;
                            iteration = 0;
                            for (int j = 7; j >= 0; j--)
                            {
                                Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                                //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_G[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Green);
                            }

                            //f1.GB_Status_AppendText_Nextline("G_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Green);
                            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, gray].double_G == 0)
                            {
                                Calculated_Vdata[band, gray].double_G = Vdata;
                                break;
                            }
                        }



                        for (double Vdata = Gamma_Voltage[band - 1, gray].double_B; Vdata <= F7; Vdata += 0.001)
                        {
                            if (Vdata > (F7 - 0.002))
                            {
                                Calculated_Vdata[band, gray].double_B = Vdata;
                                break;
                            }

                            Calculated_Target_Lv = 0;
                            iteration = 0;
                            for (int j = 7; j >= 0; j--)
                            {
                                Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                                //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_B[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Blue);
                            }
                            //f1.GB_Status_AppendText_Nextline("B_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Blue);
                            if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, gray].double_B == 0)
                            {
                                Calculated_Vdata[band, gray].double_B = Vdata;
                                break;
                            }
                        }
                        
                        f1.GB_Status_AppendText_Nextline("==========================================", Color.Black);
                        f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "/" + gray.ToString() + "]Calculated_Vdata_R = " + Calculated_Vdata[band, gray].double_R, Color.Red);
                        f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "/" + gray.ToString() + "]Calculated_Vdata_G = " + Calculated_Vdata[band, gray].double_G, Color.Green);
                        f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "/" + gray.ToString() + "]Calculated_Vdata_B = " + Calculated_Vdata[band, gray].double_B, Color.Blue);

                        if (gray == 0)//G255 (AM2)
                        {
                            Rev_Gamma[band, gray].int_R = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Calculated_Vdata[band, gray].double_R);
                            Rev_Gamma[band, gray].int_G = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Calculated_Vdata[band, gray].double_G);
                            Rev_Gamma[band, gray].int_B = Get_Gamma_From_AM2_Voltage(F7, Vreg1_Voltage[band], Calculated_Vdata[band, gray].double_B);
                            f1.GB_Status_AppendText_Nextline("Rev [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Rev_Gamma[band, gray].int_R.ToString() + "/" + Rev_Gamma[band, gray].int_G.ToString() + "/" + Rev_Gamma[band, gray].int_B.ToString(), Color.Red);

                        }
                        else
                        {
                            Rev_Gamma[band, gray].int_R = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_R, Calculated_Vdata[band, gray].double_R);
                            Rev_Gamma[band, gray].int_G = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_G, Calculated_Vdata[band, gray].double_G);
                            Rev_Gamma[band, gray].int_B = Get_Gamma_From_Normal_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_B, Calculated_Vdata[band, gray].double_B);
                            f1.GB_Status_AppendText_Nextline("Rev [B/G] Gamma R/G/B= [" + band.ToString() + "/" + gray.ToString() + "]" + Rev_Gamma[band, gray].int_R.ToString() + "/" + Rev_Gamma[band, gray].int_G.ToString() + "/" + Rev_Gamma[band, gray].int_B.ToString(), Color.Red);
                            f1.GB_Status_AppendText_Nextline("AM1_RGB_Voltage[band] : " + AM1_RGB_Voltage[band].ToString(), Color.Red);
                            f1.GB_Status_AppendText_Nextline("Calculated_Vdata[band, gray - 1].double_R : " + Gamma_Voltage[band, gray - 1].double_R.ToString(), Color.Red);
                            f1.GB_Status_AppendText_Nextline("Calculated_Vdata[band, gray].double_R" + Calculated_Vdata[band, gray].double_R.ToString(), Color.Red);
                        }
                    }
                }
            }



            if (checkBox_Get_HBM_Equation.Checked)
            {
                //C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv 
                //AC = Lv
                SJH_Matrix M = new SJH_Matrix();
                double[] Lv = new double[8];
                double[] Vdata_R = new double[8];
                double[] Vdata_G = new double[8];
                double[] Vdata_B = new double[8];
                double[][] A_R = M.MatrixCreate(8, 8);
                double[][] A_G = M.MatrixCreate(8, 8);
                double[][] A_B = M.MatrixCreate(8, 8);

                //double[][] inv = M.MatrixInverse(A);
                //Get A and Lv
                int count = 0;
                for (int i = 0; i <= 7; i++)
                {
                    Vdata_R[i] = Gamma_Voltage[0, i].double_R;
                    Vdata_G[i] = Gamma_Voltage[0, i].double_G;
                    Vdata_B[i] = Gamma_Voltage[0, i].double_B;
                    Lv[i] = Target[0, i].double_Lv;
                    count = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        A_R[i][count] = Math.Pow(Vdata_R[i], j);
                        A_G[i][count] = Math.Pow(Vdata_G[i], j);
                        A_B[i][count] = Math.Pow(Vdata_B[i], j);
                        count++;
                    }
                }

                //Show A and Lv
                string[] Temp_R = new string[8];
                string[] Temp_G = new string[8];
                string[] Temp_B = new string[8];
                for (int i = 0; i <= 7; i++)
                {
                    Temp_R[i] = (Lv[i].ToString() + "(nit):");
                    Temp_G[i] = (Lv[i].ToString() + "(nit):");
                    Temp_B[i] = (Lv[i].ToString() + "(nit):");
                    for (int j = 0; j <= 7; j++)
                    {
                        Temp_R[i] += (" " + A_R[i][j].ToString());
                        Temp_G[i] += (" " + A_G[i][j].ToString());
                        Temp_B[i] += (" " + A_B[i][j].ToString());
                    }
                    f1.GB_Status_AppendText_Nextline(Temp_R[i], Color.Red);
                    f1.GB_Status_AppendText_Nextline(Temp_G[i], Color.Green);
                    f1.GB_Status_AppendText_Nextline(Temp_B[i], Color.Blue);
                }


                //Get C = inv(A) * Lv
                double[][] Inv_A_R = M.MatrixCreate(8, 8);
                double[][] Inv_A_G = M.MatrixCreate(8, 8);
                double[][] Inv_A_B = M.MatrixCreate(8, 8);

                double[] C_R = new double[8];
                double[] C_G = new double[8];
                double[] C_B = new double[8];
                Inv_A_R = M.MatrixInverse(A_R);
                Inv_A_G = M.MatrixInverse(A_G);
                Inv_A_B = M.MatrixInverse(A_B);
                C_R = M.Matrix_Multiply(Inv_A_R, Lv);
                C_G = M.Matrix_Multiply(Inv_A_G, Lv);
                C_B = M.Matrix_Multiply(Inv_A_B, Lv);

                //Show C
                string temp_C_R = "R :";
                string temp_C_G = "G :";
                string temp_C_B = "B :";
                for (int i = 0; i <= 7; i++) 
                {
                    temp_C_R += (" " + C_R[i].ToString());
                    temp_C_G += (" " + C_G[i].ToString());
                    temp_C_B += (" " + C_B[i].ToString());
                }
                f1.GB_Status_AppendText_Nextline(temp_C_R, Color.Red);
                f1.GB_Status_AppendText_Nextline(temp_C_G, Color.Green);
                f1.GB_Status_AppendText_Nextline(temp_C_B, Color.Blue);

                //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                double Target_Lv;
                RGB_Double[,] Calculated_Vdata = new RGB_Double[10, 8];
                double Calculated_Target_Lv;
                int iteration;
                Calculated_Vdata[0, 0].double_R = Gamma_Voltage[0, 0].double_R;
                Calculated_Vdata[0, 0].double_G = Gamma_Voltage[0, 0].double_G;
                Calculated_Vdata[0, 0].double_B = Gamma_Voltage[0, 0].double_B;

                for (int band = 1; band < 10; band++)
                {
                    Target_Lv = Target[band, 0].double_Lv;
                    Calculated_Vdata[band, 0].double_R = 0;
                    Calculated_Vdata[band, 0].double_G = 0;
                    Calculated_Vdata[band, 0].double_B = 0; 
                    Calculated_Target_Lv = 0;
                    //for (double Vdata = 3; Vdata <= 4; Vdata += 0.001)
                    //for (double Vdata = E7; Vdata <= F7; Vdata += 0.001)
                    for (double Vdata = Calculated_Vdata[band - 1, 0].double_R; Vdata <= F7; Vdata += 0.001) 
                    {
                        Calculated_Target_Lv = 0;
                        iteration = 0;
                        for (int j = 7; j >= 0; j--)
                        {
                            Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[iteration++]);
                            //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_R[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Red);
                        }
                        
                        //f1.GB_Status_AppendText_Nextline("R_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Red);
                        if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, 0].double_R == 0) Calculated_Vdata[band, 0].double_R = Vdata;
                    }

                    for (double Vdata = Calculated_Vdata[band - 1, 0].double_G; Vdata <= F7; Vdata += 0.001)
                    {
                        Calculated_Target_Lv = 0;
                        iteration = 0;
                        for (int j = 7; j >= 0; j--)
                        {
                            Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                            //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_G[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Green);
                        }
                        
                        //f1.GB_Status_AppendText_Nextline("G_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Green);
                        if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, 0].double_G == 0) Calculated_Vdata[band, 0].double_G = Vdata;
                    }

                    for (double Vdata = Calculated_Vdata[band - 1, 0].double_B; Vdata <= F7; Vdata += 0.001)
                    {
                        Calculated_Target_Lv = 0;
                        iteration = 0;
                        for (int j = 7; j >= 0; j--)
                        {
                            Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[iteration++]);
                            //f1.GB_Status_AppendText_Nextline("Calculated_Target_Lv_B[" + iteration.ToString() + "] : " + Calculated_Target_Lv.ToString(), Color.Blue);
                        }
                        //f1.GB_Status_AppendText_Nextline("B_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Blue);
                        if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, 0].double_B == 0) Calculated_Vdata[band, 0].double_B = Vdata;    
                    }
                
                    f1.GB_Status_AppendText_Nextline("==========================================", Color.Black);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated_Vdata_R = " + Calculated_Vdata[band, 0].double_R, Color.Red);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated_Vdata_G = " + Calculated_Vdata[band, 0].double_G, Color.Green);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated_Vdata_B = " + Calculated_Vdata[band, 0].double_B, Color.Blue);

                    //Get Vreg1 By Gamma_R
                    f1.GB_Status_AppendText_Nextline("==========================================", Color.Black);
                    double Vreg1 = F7 + ((Calculated_Vdata[band, 0].double_R - F7) * (700.0 / (Gamma[(band - 1), 0].int_R + 189.0)));
                    int Vreg1_dec = Get_Vreg1_Dec(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated Vreg1(Get by Gamma R) : " + Vreg1.ToString() + "/" + Vreg1_dec.ToString(), Color.Red);

                    Vreg1 = F7 + ((Calculated_Vdata[band, 0].double_G - F7) * (700.0 / (Gamma[(band - 1), 0].int_G + 189.0)));
                    Vreg1_dec = Get_Vreg1_Dec(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated Band1 Vreg1(Get by Gamma G) : " + Vreg1.ToString() + "/" + Vreg1_dec.ToString(), Color.Green);

                    Vreg1 = F7 + ((Calculated_Vdata[band, 0].double_B - F7) * (700.0 / (Gamma[(band - 1), 0].int_B + 189.0)));
                    Vreg1_dec = Get_Vreg1_Dec(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated Band1 Vreg1(Get by Gamma B) : " + Vreg1.ToString() + "/" + Vreg1_dec.ToString(), Color.Blue);
                }
            }

            if (checkBox_Get_HBM_Equation_By_Dll.Checked)
            {

               double[] HBM_Gamma_Voltage_G = new double[8];
               double[] HBM_Gray_Target = new double[8];
                for (int i = 0; i <= 7; i++)
                {
                    HBM_Gamma_Voltage_G[i] = Gamma_Voltage[0, i].double_G;
                    HBM_Gray_Target[i] = Target[0, i].double_Lv;
                    f1.GB_Status_AppendText_Nextline("HBM_Gamma_Voltage_G : " + HBM_Gamma_Voltage_G[i].ToString(), Color.Blue);
                    f1.GB_Status_AppendText_Nextline("HBM_Gray_Target : " + HBM_Gray_Target[i].ToString(), Color.Blue);
                }
                double[] G255_Band_Target = new double[10];
                int[] G255_Band_Gamma_G = new int[10];
                for (int band = 0; band < 10; band++)
	            {
                        G255_Band_Target[band] = Target[band, 0].double_Lv;
                        G255_Band_Gamma_G[band] = Gamma[band, 0].int_G;
                        f1.GB_Status_AppendText_Nextline("G255_Band_Target : " + G255_Band_Target[band].ToString(), Color.Red);
                        f1.GB_Status_AppendText_Nextline("G255_Band_Gamma_G : " + G255_Band_Gamma_G[band].ToString(), Color.Red);
                }

                for (int band = 0; band < 10; band++)
                {
                    //int Vreg1_Dec = Test_checkBox_Get_HBM_Equation_C_Sharp_Ver(band, HBM_Gamma_Voltage_G, HBM_Gray_Target, G255_Band_Target, G255_Band_Gamma_G, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7); 
                    int Vreg1_Dec = Test_checkBox_Get_HBM_Equation(band, HBM_Gamma_Voltage_G, HBM_Gray_Target, G255_Band_Target, G255_Band_Gamma_G, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7); 
                    f1.GB_Status_AppendText_Nextline("Vreg1_dec[" + band.ToString() + "] : " + Vreg1_Dec.ToString(), Color.Blue);
                }
            }

            if (checkBox_Get_All_Equation.Checked)
            {
                SJH_Matrix M = new SJH_Matrix();
                double[][] C_R = M.MatrixCreate(9, 8); //(10 - 1)ea bands,8ea (C7,C6,..,C0)
                double[][] C_G = M.MatrixCreate(9, 8); //(10 - 1)ea bands,8ea (C7,C6,..,C0)
                double[][] C_B = M.MatrixCreate(9, 8); //(10 - 1)ea bands,8ea (C7,C6,..,C0)


                RGB_Double[,] Calculated_Vdata = new RGB_Double[10, 8];
                Calculated_Vdata[0, 0].double_R = Gamma_Voltage[0, 0].double_R;
                Calculated_Vdata[0, 0].double_G = Gamma_Voltage[0, 0].double_G;
                Calculated_Vdata[0, 0].double_B = Gamma_Voltage[0, 0].double_B;

                for (int band = 1; band < 10; band++)
                {
                    //C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv 
                    //AC = Lv

                    double[] Lv = new double[8];
                    double[] Vdata_R = new double[8];
                    double[] Vdata_G = new double[8];
                    double[] Vdata_B = new double[8];
                    double[][] A_R = M.MatrixCreate(8, 8);
                    double[][] A_G = M.MatrixCreate(8, 8);
                    double[][] A_B = M.MatrixCreate(8, 8);

                    //double[][] inv = M.MatrixInverse(A);
                    //Get A and Lv
                    int count = 0;
                    for (int i = 0; i <= 7; i++)
                    {
                        Vdata_R[i] = Gamma_Voltage[band - 1, i].double_R;
                        Vdata_G[i] = Gamma_Voltage[band - 1, i].double_G;
                        Vdata_B[i] = Gamma_Voltage[band - 1, i].double_B;

                        //Lv[i] = Convert.ToDouble(dataGridView_OC_param.Rows[i + 2].Cells[9].Value);
                        Lv[i] = Target[band - 1, i].double_Lv;

                        count = 0;
                        for (int j = 7; j >= 0; j--)
                        {
                            A_R[i][count] = Math.Pow(Vdata_R[i], j);
                            A_G[i][count] = Math.Pow(Vdata_G[i], j);
                            A_B[i][count] = Math.Pow(Vdata_B[i], j);
                            count++;
                        }
                    }
                    //Show A and Lv
                    string[] Temp_R = new string[8];
                    string[] Temp_G = new string[8];
                    string[] Temp_B = new string[8];
                    for (int i = 0; i <= 7; i++)
                    {
                        Temp_R[i] = (Lv[i].ToString() + "(nit):");
                        Temp_G[i] = (Lv[i].ToString() + "(nit):");
                        Temp_B[i] = (Lv[i].ToString() + "(nit):");
                        for (int j = 0; j <= 7; j++)
                        {
                            Temp_R[i] += (" " + A_R[i][j].ToString());
                            Temp_G[i] += (" " + A_G[i][j].ToString());
                            Temp_B[i] += (" " + A_B[i][j].ToString());
                        }
                    }

                    //Get C = inv(A) * Lv
                    double[][] Inv_A_R = M.MatrixCreate(8, 8);
                    double[][] Inv_A_G = M.MatrixCreate(8, 8);
                    double[][] Inv_A_B = M.MatrixCreate(8, 8);


                    Inv_A_R = M.MatrixInverse(A_R);
                    Inv_A_G = M.MatrixInverse(A_G);
                    Inv_A_B = M.MatrixInverse(A_B);
                    C_R[band - 1] = M.Matrix_Multiply(Inv_A_R, Lv);
                    C_G[band - 1] = M.Matrix_Multiply(Inv_A_G, Lv);
                    C_B[band - 1] = M.Matrix_Multiply(Inv_A_B, Lv);

                    //Show C
                    string temp_C_R = "R :";
                    string temp_C_G = "G :";
                    string temp_C_B = "B :";
                    for (int i = 0; i <= 7; i++)
                    {
                        temp_C_R += (" " + C_R[band - 1][i].ToString());
                        temp_C_G += (" " + C_G[band - 1][i].ToString());
                        temp_C_B += (" " + C_B[band - 1][i].ToString());
                    }
                    f1.GB_Status_AppendText_Nextline("[" + (band - 1).ToString() + "] temp_C_R : " + temp_C_R, Color.Red);
                    f1.GB_Status_AppendText_Nextline("[" + (band - 1).ToString() + "] temp_C_G : " + temp_C_G, Color.Green);
                    f1.GB_Status_AppendText_Nextline("[" + (band - 1).ToString() + "] temp_C_B : " + temp_C_B, Color.Blue);

                    //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
                    double Target_Lv;
                    double Calculated_Target_Lv = 0;
                    int iteration;



                    Target_Lv = Target[band, 0].double_Lv;
                    f1.GB_Status_AppendText_Nextline("T_Lv : " + Target_Lv.ToString(), Color.Black);

                    Calculated_Vdata[band, 0].double_R = 0;
                    Calculated_Vdata[band, 0].double_G = 0;
                    Calculated_Vdata[band, 0].double_B = 0;

                    //for (double Vdata = 3; Vdata <= 4; Vdata += 0.001)
                    for (double Vdata = Calculated_Vdata[band - 1, 0].double_R; Vdata <= F7; Vdata += 0.001)
                    {
                        Calculated_Target_Lv = 0;
                        iteration = 0;
                        for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_R[band - 1][iteration++]);
                        //f1.GB_Status_AppendText_Nextline("R_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Red);
                        if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, 0].double_R == 0) Calculated_Vdata[band, 0].double_R = Vdata;
                    }
                    for (double Vdata = Calculated_Vdata[band - 1, 0].double_G; Vdata <= F7; Vdata += 0.001)
                    {

                        Calculated_Target_Lv = 0;
                        iteration = 0;
                        for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[band - 1][iteration++]);
                        //f1.GB_Status_AppendText_Nextline("G_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Green);
                        if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, 0].double_G == 0) Calculated_Vdata[band, 0].double_G = Vdata;
                    }
                    for (double Vdata = Calculated_Vdata[band - 1, 0].double_B; Vdata <= F7; Vdata += 0.001)
                    {
                        Calculated_Target_Lv = 0;
                        iteration = 0;
                        for (int j = 7; j >= 0; j--) Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_B[band - 1][iteration++]);
                        //f1.GB_Status_AppendText_Nextline("B_Vdata = " + Vdata.ToString() + "Calculated_Target_Lv = " + Calculated_Target_Lv.ToString(), Color.Blue);
                        if ((Calculated_Target_Lv < Target_Lv) && Calculated_Vdata[band, 0].double_B == 0) Calculated_Vdata[band, 0].double_B = Vdata;
                    }
                    f1.GB_Status_AppendText_Nextline("==========================================", Color.Black);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated_Vdata_R = " + Calculated_Vdata[band, 0].double_R, Color.Red);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated_Vdata_G = " + Calculated_Vdata[band, 0].double_G, Color.Green);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated_Vdata_B = " + Calculated_Vdata[band, 0].double_B, Color.Blue);

                    //Get Vreg1 By Gamma_R
                    f1.GB_Status_AppendText_Nextline("==========================================", Color.Black);
                    double Vreg1 = F7 + ((Calculated_Vdata[band, 0].double_R - F7) * (700.0 / (Gamma[(band - 1), 0].int_R + 189.0)));
                    int Vreg1_dec = Get_Vreg1_Dec(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated Vreg1(Get by Gamma R) : " + Vreg1.ToString() + "/" + Vreg1_dec.ToString(), Color.Red);

                    Vreg1 = F7 + ((Calculated_Vdata[band, 0].double_G - F7) * (700.0 / (Gamma[(band - 1), 0].int_G + 189.0)));
                    Vreg1_dec = Get_Vreg1_Dec(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated Band1 Vreg1(Get by Gamma G) : " + Vreg1.ToString() + "/" + Vreg1_dec.ToString(), Color.Green);

                    Vreg1 = F7 + ((Calculated_Vdata[band, 0].double_B - F7) * (700.0 / (Gamma[(band - 1), 0].int_B + 189.0)));
                    Vreg1_dec = Get_Vreg1_Dec(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                    f1.GB_Status_AppendText_Nextline("[" + band.ToString() + "]Calculated Band1 Vreg1(Get by Gamma B) : " + Vreg1.ToString() + "/" + Vreg1_dec.ToString(), Color.Blue);
                }
            }
        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            SJH_Matrix M = new SJH_Matrix();
            double[][] m = new double[][] { new double[] { 0.50291, 0.379319 , 0.123123}, new double[] {0.52017,0.417991,0.2342}};
            double[][] inv = M.MatrixInverse(m);

            int row_length = m.Length;
            int column_length = m[0].Length;
            
            //printing the Original (OK)
            for (int i = 0; i < row_length; i++)
            {
                for (int j = 0; j < column_length; j++)
                {
                    //f1.GB_Status_AppendText_Nextline("Ori : " + Math.Round(m[i][j], 5).ToString(), Color.Blue);
                }
                f1.GB_Status_AppendText_Nextline("Ori : " + Math.Round(m[i][0], 5).ToString()
                    + "," + Math.Round(m[i][1], 5).ToString(), Color.Blue);
            }


            //printing the inverse (OK)
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    //f1.GB_Status_AppendText_Nextline("Inv : " + Math.Round(inv[i][j], 5).ToString(), Color.Blue);
                }
                f1.GB_Status_AppendText_Nextline("Inv : " + Math.Round(inv[i][0], 5).ToString()
    + "," + Math.Round(inv[i][1], 5).ToString(), Color.Blue);
            }

        
        }

        private void checkBox_Get_All_Equation_CheckedChanged(object sender, EventArgs e)
        {

        }

        private int Test_checkBox_Get_HBM_Equation_C_Sharp_Ver(int band, double[] HBM_Gamma_Voltage_G, double[] HBM_Gray_Target, double[] G255_Band_Target, int[] G255_Band_Gamma_G
    , double E7, double VREG1_REF818_volt, double VREG1_REF614_volt, double VREG1_REF409_volt, double VREG1_REF205_volt, double F7)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv 
            //AC = Lv
            SJH_Matrix M = new SJH_Matrix();
            double[] Lv = new double[8];
            double[] Vdata_G = new double[8];
            double[][] A_G = M.MatrixCreate(8, 8);

            //double[][] inv = M.MatrixInverse(A);
            //Get A and Lv
            int count = 0;
            for (int i = 0; i <= 7; i++)
            {
                Vdata_G[i] = HBM_Gamma_Voltage_G[i];
                Lv[i] = HBM_Gray_Target[i];
                count = 0;
                for (int j = 7; j >= 0; j--)
                {
                    A_G[i][count] = Math.Pow(Vdata_G[i], j);
                    count++;
                }
            }

            //Get C = inv(A) * Lv
            double[][] Inv_A_G = M.MatrixCreate(8, 8);
            double[] C_G = new double[8];
            Inv_A_G = M.MatrixInverse(A_G);
            C_G = M.Matrix_Multiply(Inv_A_G, Lv);

            //Show "C7*(Vdata^7) + C6*(Vdata^6) + .... + C1*Vdata + C0 = Lv"
            double Target_Lv;
            double[] G255_Calculated_Vdata_G = new double[10];
            double G255_Calculated_Target_Lv;
            int iteration;

            G255_Calculated_Vdata_G[0] = HBM_Gamma_Voltage_G[0];

            if (band == 0)
            {
                return 1023;
            }
            else if (band >= 1 && band < 10)
            {
                Target_Lv = G255_Band_Target[band];
                G255_Calculated_Vdata_G[band] = 0;
                G255_Calculated_Target_Lv = 0;
                //for (double Vdata = G255_Calculated_Vdata_G[band - 1]; Vdata <= F7; Vdata += 0.001)
                for (double Vdata = 4; Vdata <= F7; Vdata += 0.001)
                {
                    G255_Calculated_Target_Lv = 0;
                    iteration = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        G255_Calculated_Target_Lv += (Math.Pow(Vdata, j) * C_G[iteration++]);
                    }

                    if ((G255_Calculated_Target_Lv < Target_Lv) && G255_Calculated_Vdata_G[band] == 0)
                    {
                        G255_Calculated_Vdata_G[band] = Vdata;
                        f1.GB_Status_AppendText_Nextline("G255_Calculated_Vdata_G[band] : " + G255_Calculated_Vdata_G[band].ToString(), Color.Blue);
                        break;
                    }
                }
                double Vreg1 = F7 + ((G255_Calculated_Vdata_G[band] - F7) * (700.0 / (G255_Band_Gamma_G[band] + 189.0)));
                return Get_Vreg1_Dec(Vreg1, E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
            }
            else
            {
                return 0;
            }
        }


        private void Get_Gray_Gamma(RGB[,] Read_Gamma)
        {
            //RGB[,] Read_Gamma = new RGB[10, 8];//10ea Bands , 8ea Gray-points (Add on 191028)
            Meta_SW43408B Meta = Meta_SW43408B.getInstance();
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string[] Gamma_Binary_data = new string[33]; 
            string[] Gamma_Hex_data = new string[33];

            for (int band = 0; band < 10; band++)
            {
                string Current_Band_Gamma_Register = Meta.Get_Gamma_Register_Hex_String(band).Remove(0, 2);
                f1.OTP_Read(33, Current_Band_Gamma_Register);
                for (int i = 0; i < 33; i++)
                {
                    Gamma_Hex_data[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();
                    Gamma_Binary_data[i] = (Convert.ToString(Convert.ToInt32(Gamma_Hex_data[i], 16), 2)).PadLeft(8, '0');
                }
                //Gray 별 Binary Values
                //R
                Read_Gamma[band, 0].int_R = Convert.ToInt16((Gamma_Binary_data[0][0] + Gamma_Binary_data[1]), 2);
                Read_Gamma[band, 1].int_R = Convert.ToInt16((Gamma_Binary_data[2][0] + Gamma_Binary_data[10]), 2);
                Read_Gamma[band, 2].int_R = Convert.ToInt16((Gamma_Binary_data[2][1] + Gamma_Binary_data[9]), 2);
                Read_Gamma[band, 3].int_R = Convert.ToInt16((Gamma_Binary_data[2][2] + Gamma_Binary_data[8]), 2);
                Read_Gamma[band, 4].int_R = Convert.ToInt16((Gamma_Binary_data[2][3] + Gamma_Binary_data[7]), 2);
                Read_Gamma[band, 5].int_R = Convert.ToInt16((Gamma_Binary_data[2][4] + Gamma_Binary_data[6]), 2);
                Read_Gamma[band, 6].int_R = Convert.ToInt16((Gamma_Binary_data[2][5] + Gamma_Binary_data[5]), 2);
                Read_Gamma[band, 7].int_R = Convert.ToInt16((Gamma_Binary_data[2][6] + Gamma_Binary_data[4]), 2);
                //Read_Gamma[band, 8].int_R = Convert.ToInt16((Gamma_Binary_data[2][7] + Gamma_Binary_data[3]), 2);

                //G
                int Offset = 11;
                Read_Gamma[band, 0].int_G = Convert.ToInt16((Gamma_Binary_data[0 + Offset][0] + Gamma_Binary_data[1 + Offset]), 2);
                Read_Gamma[band, 1].int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][0] + Gamma_Binary_data[10 + Offset]), 2);
                Read_Gamma[band, 2].int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][1] + Gamma_Binary_data[9 + Offset]), 2);
                Read_Gamma[band, 3].int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][2] + Gamma_Binary_data[8 + Offset]), 2);
                Read_Gamma[band, 4].int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][3] + Gamma_Binary_data[7 + Offset]), 2);
                Read_Gamma[band, 5].int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][4] + Gamma_Binary_data[6 + Offset]), 2);
                Read_Gamma[band, 6].int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][5] + Gamma_Binary_data[5 + Offset]), 2);
                Read_Gamma[band, 7].int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][6] + Gamma_Binary_data[4 + Offset]), 2);
                //Read_Gamma[band, 8].int_G = Convert.ToInt16((Gamma_Binary_data[2 + Offset][7] + Gamma_Binary_data[3 + Offset]), 2);

                //B
                Offset = 22;
                Read_Gamma[band, 0].int_B = Convert.ToInt16((Gamma_Binary_data[0 + Offset][0] + Gamma_Binary_data[1 + Offset]), 2);
                Read_Gamma[band, 1].int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][0] + Gamma_Binary_data[10 + Offset]), 2);
                Read_Gamma[band, 2].int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][1] + Gamma_Binary_data[9 + Offset]), 2);
                Read_Gamma[band, 3].int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][2] + Gamma_Binary_data[8 + Offset]), 2);
                Read_Gamma[band, 4].int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][3] + Gamma_Binary_data[7 + Offset]), 2);
                Read_Gamma[band, 5].int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][4] + Gamma_Binary_data[6 + Offset]), 2);
                Read_Gamma[band, 6].int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][5] + Gamma_Binary_data[5 + Offset]), 2);
                Read_Gamma[band, 7].int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][6] + Gamma_Binary_data[4 + Offset]), 2);
                //Read_Gamma[band, 8].int_B = Convert.ToInt16((Gamma_Binary_data[2 + Offset][7] + Gamma_Binary_data[3 + Offset]), 2);

                //Update RGB String From Int
                Read_Gamma[band, 0].String_Update_From_int();
                Read_Gamma[band, 1].String_Update_From_int();
                Read_Gamma[band, 2].String_Update_From_int();
                Read_Gamma[band, 3].String_Update_From_int();
                Read_Gamma[band, 4].String_Update_From_int();
                Read_Gamma[band, 5].String_Update_From_int();
                Read_Gamma[band, 6].String_Update_From_int();
                Read_Gamma[band, 7].String_Update_From_int();
                //Read_Gamma[band, 8].String_Update_From_int();

            }
        }











        private void button_Real_RGBVreg1_and_Get_Real_Vdata_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Meta_Form_Show();
            Gamma_Vdata_Clear();
            f1.GB_Status_AppendText_Nextline("*Assumption : ELVDD = 4.6v , DDVDH = 6.7v , GREF_SEL = 1 , GA_INV = 1,VG4_EN =1 , AVAMODE = 0", Color.Red);

            f1.IPC_Quick_Send("mipi.write 0x15 0xB0 0xAC");

            //Get ELVDD - FV1_LVL[5:0],ELVDD + VCI1_LVL[5:0]
            string[] D0_Hex = new string[19];
            f1.OTP_Read(19, "D0");
            for (int i = 0; i < 19; i++) D0_Hex[i] = f1.dataGridView1.Rows[i].Cells[1].Value.ToString();

            double ELVDD = 4.6;

            string FV1 = D0_Hex[18];
            int dec_FV1 = Convert.ToInt16(FV1, 16);
            if (dec_FV1 >= 42) dec_FV1 = 42;
            double E7 = ELVDD - (0.2 + (dec_FV1 * 0.1)); //ELVDD - FV1_LVL[5:0]

            string VCI1 = D0_Hex[16];
            int dec_VCI1 = Convert.ToInt16(VCI1, 16);
            if (dec_VCI1 >= 42) dec_VCI1 = 42;
            double F7 = ELVDD + (0.2 + (dec_VCI1 * 0.1)); //ELVDD + VCI1_LVL[5:0]

            //Get VREG1_REF818[5:0],VREG1_REF614[5:0],VREG1_REF409[5:0],VREG1_REF205[5:0]
            //Vreg1_REF818 = F7+(E7-F7)*((222.5+0.5*HEX2DEC(VREG1_REF818_Hex))/254))
            //VREG1_REF614 = F7+(E7-F7)*((206.5+0.5*HEX2DEC(VREG1_REF614_Hex))/254))
            //VREG1_REF409 = F7+(E7-F7)*((182.5+0.5*HEX2DEC(VREG1_REF409_Hex))/254))
            //VREG1_REF205 = F7+(E7-F7)*((154.5+0.5*HEX2DEC(VREG1_REF205_Hex))/254))
            int Dec_VREG1_REF818 = Convert.ToInt16(D0_Hex[1], 16) & 0x3F;
            int Dec_VREG1_REF614 = Convert.ToInt16(D0_Hex[2], 16) & 0x3F;
            int Dec_VREG1_REF409 = Convert.ToInt16(D0_Hex[3], 16) & 0x3F;
            int Dec_VREG1_REF205 = Convert.ToInt16(D0_Hex[4], 16) & 0x3F;

            double VREG1_REF818_volt = F7 + (E7 - F7) * ((222.5 + 0.5 * Dec_VREG1_REF818) / 254);
            double VREG1_REF614_volt = F7 + (E7 - F7) * ((206.5 + 0.5 * Dec_VREG1_REF614) / 254);
            double VREG1_REF409_volt = F7 + (E7 - F7) * ((182.5 + 0.5 * Dec_VREG1_REF409) / 254);
            double VREG1_REF205_volt = F7 + (E7 - F7) * ((154.5 + 0.5 * Dec_VREG1_REF205) / 254);

            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) - FV1_LVL(v) = " + E7.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF818(v) = " + VREG1_REF818_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF614(v) = " + VREG1_REF614_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF409(v) = " + VREG1_REF409_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("VREG1_REF205(v) = " + VREG1_REF205_volt.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("ELVDD(4.6v) + VCI1_LVL(v) = " + F7.ToString(), Color.Blue);

            //Get Vreg1/AM1 Voltage for each band
            Meta_Model_Option_Form Meta_Form = (Meta_Model_Option_Form)Application.OpenForms["Meta_Model_Option_Form"];
            Meta_Form.button_Vreg1_Read_Click();
            int[] Dec_Vreg1 = new int[10];
            double[] Vreg1_Voltage = new double[10];
            Dec_Vreg1[0] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B0.Text);
            Dec_Vreg1[1] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B1.Text);
            Dec_Vreg1[2] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B2.Text);
            Dec_Vreg1[3] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B3.Text);
            Dec_Vreg1[4] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B4.Text);
            Dec_Vreg1[5] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B5.Text);
            Dec_Vreg1[6] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B6.Text);
            Dec_Vreg1[7] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B7.Text);
            Dec_Vreg1[8] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B8.Text);
            Dec_Vreg1[9] = Convert.ToInt16(Meta_Form.textBox_Vreg1_B9.Text);
            double[] AM1_RGB_Voltage = new double[10];

            for (int band = 0; band < 10; band++)
            {
                Vreg1_Voltage[band] = Get_Vreg1_Voltage(Dec_Vreg1[band], E7, VREG1_REF818_volt, VREG1_REF614_volt, VREG1_REF409_volt, VREG1_REF205_volt, F7);
                AM1_RGB_Voltage[band] = F7 + (Vreg1_Voltage[band] - F7) * (8.0 / 700.0);
                f1.GB_Status_AppendText_Nextline(band.ToString() + ") Vreg1_Dec / Vreg1_Voltage / AM1_Voltage = " + Dec_Vreg1[band].ToString() + " / " + Vreg1_Voltage[band].ToString() + " / " + AM1_RGB_Voltage[band].ToString(), Color.Black);
            }

            RGB[,] Read_Gamma = new RGB[10, 8];//10ea Bands , 8ea Gray-points (Add on 191028)
            Get_Gray_Gamma(Read_Gamma);

            int OC_row_length = dataGridView_OC_param.RowCount;
            for (int i = 2; i < OC_row_length; i++)
            {
                int band = (i - 2) / 8;
                int gray = (i - 2) % 8;
                if (gray == 0)//G255 (AM2)
                {
                    Gamma_Voltage[band, gray].double_R = Get_AM2_Voltage(F7, Vreg1_Voltage[band], Read_Gamma[band, gray].int_R);
                    Gamma_Voltage[band, gray].double_G = Get_AM2_Voltage(F7, Vreg1_Voltage[band], Read_Gamma[band, gray].int_G);
                    Gamma_Voltage[band, gray].double_B = Get_AM2_Voltage(F7, Vreg1_Voltage[band], Read_Gamma[band, gray].int_B);
                }
                else
                {
                    Gamma_Voltage[band, gray].double_R = Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_R, Read_Gamma[band, gray].int_R);
                    Gamma_Voltage[band, gray].double_G = Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_G, Read_Gamma[band, gray].int_G);
                    Gamma_Voltage[band, gray].double_B = Get_Normal_Gamma_Voltage(AM1_RGB_Voltage[band], Gamma_Voltage[band, gray - 1].double_B, Read_Gamma[band, gray].int_B);
                }
                dataGridView_RGB_Vdata.Rows[i].Cells[1].Value = Gamma_Voltage[band, gray].double_R;
                dataGridView_RGB_Vdata.Rows[i].Cells[2].Value = Gamma_Voltage[band, gray].double_G;
                dataGridView_RGB_Vdata.Rows[i].Cells[3].Value = Gamma_Voltage[band, gray].double_B;
            }
        }
    }
}
