using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public partial class Engineer_Mornitoring_Mode : Form
    {
        public XYLv[,] Limit = new XYLv[12, 10]; //12ea Bands , 10ea Gray-points


        private static Engineer_Mornitoring_Mode Instance;
        public static Engineer_Mornitoring_Mode getInstance()
        {
            if (Instance == null)
                Instance = new Engineer_Mornitoring_Mode();

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

        private Engineer_Mornitoring_Mode()
        {
            InitializeComponent();
        }

        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        private void Engineer_Mornitoring_Mode_Load(object sender, EventArgs e)
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
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    Limit[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[10].Value);
                    Limit[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[11].Value);
                    Limit[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[12].Value);
                }
            }
            radioButton_Diff_Hide_CheckedChanged(sender, e);
        }

        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Engineer_Mode_Grid_Tema_Change()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
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
                    radiobutton_AOD0.Checked = true;
                    break;
                case 10:
                    radiobutton_AOD1.Checked = true;
                    break;
                case 11:
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
            else if (radiobutton_AOD0.Checked)
                return 9;
            else if (radiobutton_AOD1.Checked)
                return 10;
            else if (radiobutton_AOD2.Checked)
                return 11;
            else
                return 0;
        }

        


        private void radiobutton_Band1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 0;
            if (radiobutton_Band0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }


        private void radiobutton_Band2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 1;
            if (radiobutton_Band1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 2;
            if (radiobutton_Band2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 3;
            if (radiobutton_Band3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 4;
            if (radiobutton_Band4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 5;
            if (radiobutton_Band5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band7_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 6;
            if (radiobutton_Band6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band8_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 7;
            if (radiobutton_Band7.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band9_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 8;
            if (radiobutton_Band8.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band10_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 9;
            if (radiobutton_AOD0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band11_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 10;
            if (radiobutton_AOD1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row);
            }
        }

        private void radiobutton_Band12_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 10 * 11;
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

            radiobutton_AOD0.Enabled = enable;
            radiobutton_AOD1.Enabled = enable;
            radiobutton_AOD2.Enabled = enable;
        }

      


        public void Engineering_Mode_DataGridview_ReadOnly(bool ReadOnly)
        {
            this.dataGridView_OC_param.ReadOnly = ReadOnly;
        }

        public void SubGridView_G189_xy_Offset_From_G255_xy(int band)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("G189 Offset from G255 Measured xy", Color.Blue);

            First_Model_Option_Form first_model = (First_Model_Option_Form)Application.OpenForms["First_Model_Option_Form"];

            double Measured_G255_x = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[2].Cells[4].Value);
            double Measured_G255_y = Convert.ToDouble(dataGridView_Band_OC_Viewer.Rows[2].Cells[5].Value);
            double Offset_x = 0;
            double Offset_y = 0; 

            if (band == 0)
            {
                Offset_x = Convert.ToDouble(first_model.textBox_B0_Gray189_x_Offset.Text);
                Offset_y = Convert.ToDouble(first_model.textBox_B0_Gray189_y_Offset.Text);
            }
            else if (band == 1)
            {
                Offset_x = Convert.ToDouble(first_model.textBox_B1_Gray189_x_Offset.Text);
                Offset_y = Convert.ToDouble(first_model.textBox_B1_Gray189_y_Offset.Text);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Shouldn't happen (G189 Offset) , band should be band0 or band1");
            }
            dataGridView_Band_OC_Viewer.Rows[3].Cells[7].Value = Measured_G255_x + Offset_x; //G189 Target x
            dataGridView_Band_OC_Viewer.Rows[3].Cells[8].Value = Measured_G255_y + Offset_y; //G189 Target y
        }

        public void SubGridView_Copy_Final_G255_Measured_xy_to_Other_Grays_Target()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("(Sub DataGridView) Copy Final G255 Measured xy to Other Grays Target xy", Color.Blue);

            var Measured_G255_x = dataGridView_Band_OC_Viewer.Rows[2].Cells[4].Value;
            var Measured_G255_y = dataGridView_Band_OC_Viewer.Rows[2].Cells[5].Value;
            
            for(int i=3 ; i<=11 ; i++)
            {
                dataGridView_Band_OC_Viewer.Rows[i].Cells[7].Value = Measured_G255_x;
                dataGridView_Band_OC_Viewer.Rows[i].Cells[8].Value = Measured_G255_y;
            }
        }

        public void GridView_Measure_Applied_Loop_Area_Data_Clear()
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[4].Value = string.Empty;
                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[5].Value = string.Empty;
                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[6].Value = string.Empty;

                    dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[6].Value = string.Empty;

                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[15].Value = string.Empty;
                    dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[16].Value = string.Empty;

                    dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[16].Value = string.Empty;
                }
            }

        }




        public string Get_BX_GXXX_By_Gray_DP116(int gray)
        {
            return dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[0].Value.ToString();
        }


        private void Get_OC_Param_By_Gray_DP116(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
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




        private void Set_OC_Param_By_Gray_DP116(int gray, int Gamma_R, int Gamma_G, int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv, double loop_count,string Extension_Applied)
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
        

        void Show_Result(ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
            , ref double Limit_Y, ref double Limit_Lv)
        {
            System.Windows.Forms.MessageBox.Show("Gamma R = " + Gamma_R.ToString());
            System.Windows.Forms.MessageBox.Show("Gamma G = " + Gamma_G.ToString());
            System.Windows.Forms.MessageBox.Show("Gamma B = " + Gamma_B.ToString());

            System.Windows.Forms.MessageBox.Show("Target X = " + Target_X.ToString());
            System.Windows.Forms.MessageBox.Show("Target Y = " + Target_Y.ToString());
            System.Windows.Forms.MessageBox.Show("Target Lv = " + Target_Lv.ToString());

            System.Windows.Forms.MessageBox.Show("Limit X = " + Limit_X.ToString());
            System.Windows.Forms.MessageBox.Show("Limit Y = " + Limit_Y.ToString());
            System.Windows.Forms.MessageBox.Show("Limit Lv = " + Limit_Lv.ToString());
        }

        public void Get_OC_Param_DP116(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
            , ref double Limit_Y, ref double Limit_Lv, ref double Extension_X, ref double Extension_Y)
        {
            //Radio Button ctrl by band
            //Band_Radiobuttion_Select(band); 
            
            //Get Param according to gray
            Get_OC_Param_By_Gray_DP116(gray, ref Gamma_R, ref Gamma_G, ref Gamma_B, ref Target_X, ref Target_Y, ref Target_Lv, ref Limit_X, ref Limit_Y, ref Limit_Lv, ref Extension_X, ref Extension_Y);

            //Test Mode
            //Show_Result(ref Gamma_R, ref Gamma_G, ref Gamma_B, ref Target_X, ref Target_Y, ref Target_Lv, ref Limit_X, ref Limit_Y, ref Limit_Lv);
        }

        public void Get_Gamma_Only_DP116(int band,int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B)
        {
            Gamma_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[1].Value.ToString());
            Gamma_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[2].Value.ToString());
            Gamma_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[3].Value.ToString());
        }

        public void DP116_Get_All_Band_Gray_Gamma(RGB[,] Gamma)
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[1].Value.ToString());
                    Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[2].Value.ToString());
                    Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[3].Value.ToString());
                    Gamma[band, gray].String_Update_From_int();
                    
                }
            }
        }

        public void Set_OC_Offset_Gamma_DP116(int band, int gray, int Offset_R, int Offset_G, int Offset_B,int Max,int Min)
        {
            int R = (Convert.ToInt16(dataGridView_OC_param.Rows[(band - 1) * 10 + (gray + 2)].Cells[1].Value) + Offset_R);
            int G = (Convert.ToInt16(dataGridView_OC_param.Rows[(band - 1) * 10 + (gray + 2)].Cells[2].Value) + Offset_G);
            int B = (Convert.ToInt16(dataGridView_OC_param.Rows[(band - 1) * 10 + (gray + 2)].Cells[3].Value) + Offset_B);

            if(R > Max) R = Max;
            else if(R < Min) R = Min;

            if(G > Max) G = Max;
            else if(G < Min) G = Min;

            if(B > Max) B = Max;
            else if(B < Min) B = Min;

            dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[1].Value = R;
            dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[2].Value = G;
            dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[3].Value = B;
        }


        public void Set_OC_Gamma_DP116(int band, int gray, int Gamma_R, int Gamma_G, int Gamma_B)
        {
            dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[1].Value = Gamma_R;
            dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[2].Value = Gamma_G;
            dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[3].Value = Gamma_B;
        }

        public void Set_Sub_OC_Gamma_DP116()
        {
            int band = Get_Current_Band();
            for (int gray = 0; gray < 10; gray++)
            {
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[1].Value;
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[2].Value;
                dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[3].Value;
            }
        }

        public void Set_OC_Param_DP116(int gray, int Gamma_R, int Gamma_G, int Gamma_B, double Measure_X, double Measure_Y, double Measure_Lv,int loop_count,string Extension_Applied)
        {
            //Set Param according to gray
            Set_OC_Param_By_Gray_DP116(gray, Gamma_R, Gamma_G, Gamma_B, Measure_X, Measure_Y, Measure_Lv, loop_count,Extension_Applied);
        }

        public void Updata_Sub_To_Main_GridView(int band,int gray)
        {
            int Offset_Row = 10 * band;
            for (int j = 0; j < dataGridView_Band_OC_Viewer.ColumnCount; j++)
            {
                    dataGridView_OC_param.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[j].Value;
            }
        }

        private void button_Read_OC_Param_From_Excel_File_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.current_model.Read_OC_Param_From_Excel_File();
            Band_Radiobuttion_Select(0);//Select Band 0
        }

        private void button_Save_OC_Param_To_Excel_File_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string filePath = f1.Get_OC_Param_File_Path();
            SaveToCSV(this.dataGridView_OC_param, filePath);
        }

        private void SaveToCSV(DataGridView DGV, string CSV_Name = "Output.csv")
        {
            string filename = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = CSV_Name;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show("Data will be exported and you will be notified when it is ready.");
                if (File.Exists(filename))
                {
                    try
                    {
                        File.Delete(filename);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                    }
                }
                int columnCount = DGV.ColumnCount;
                string[] output = new string[DGV.RowCount];
                for (int i = 0; i < DGV.RowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        output[i] += DGV.Rows[i].Cells[j].Value.ToString() + ",";
                    }
                }
                System.IO.File.WriteAllLines(sfd.FileName, output, System.Text.Encoding.UTF8);
                MessageBox.Show("Your file was generated and its ready for use.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PasteInData(ref this.dataGridView_OC_param);
        }


        // PasteInData pastes clipboard data into the grid passed to it.
        private void PasteInData(ref DataGridView dgv)
        {
            char[] rowSplitter = { '\n', '\r' };  // Cr and Lf.
            char columnSplitter = '\t';         // Tab.

            IDataObject dataInClipboard = Clipboard.GetDataObject();

            string stringInClipboard =
                dataInClipboard.GetData(DataFormats.Text).ToString();

            string[] rowsInClipboard = stringInClipboard.Split(rowSplitter,
                StringSplitOptions.RemoveEmptyEntries);

            int r = dgv.SelectedCells[0].RowIndex;
            int c = dgv.SelectedCells[0].ColumnIndex;

            if (dgv.Rows.Count < (r + rowsInClipboard.Length))
                dgv.Rows.Add(r + rowsInClipboard.Length - dgv.Rows.Count);

            // Loop through lines:

            int iRow = 0;
            while (iRow < rowsInClipboard.Length)
            {
                // Split up rows to get individual cells:

                string[] valuesInRow =
                    rowsInClipboard[iRow].Split(columnSplitter);

                // Cycle through cells.
                // Assign cell value only if within columns of grid:

                int jCol = 0;
                while (jCol < valuesInRow.Length)
                {
                    if ((dgv.ColumnCount - 1) >= (c + jCol))
                        dgv.Rows[r + iRow].Cells[c + jCol].Value =
                        valuesInRow[jCol];

                    jCol += 1;
                } // end while

                iRow += 1;
            } // end while
        }

 
        private void dataGridView_OC_param_KeyDown(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            if (e.KeyData == (Keys.Control | Keys.V)) PasteInData(ref this.dataGridView_OC_param);
        }


        private void Limit_Update(double Ratio,bool XY)
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    if (XY)
                    {
                        if ((Limit[band, gray].double_X * Ratio) >= 0.99 || Limit[band, gray].double_X >= 0.99) dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[10].Value = 1;
                        else dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[10].Value = Limit[band, gray].double_X * Ratio;

                        if ((Limit[band, gray].double_Y * Ratio) >= 0.99 || Limit[band, gray].double_Y >= 0.99) dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[11].Value = 1;
                        else dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[11].Value = Limit[band, gray].double_Y * Ratio;
                    }
                    else //Lv
                    {
                        dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[12].Value = Limit[band, gray].double_Lv * Ratio;
                    }
                }
            }

            //update main_Grid_view from sub_Grid_View
            int Current_Band = this.Get_Current_Band();
            int Offset_Row = 10 * Current_Band;
                Copy_Data_Grid_View(Offset_Row);
        }

        private void radioButton_Limit_Ratio_100_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100.Checked)
            {
                Limit_Update(1,true);
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

        private void radioButton_Limit_Ratio_150_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150.Checked)
            {
                Limit_Update(1.5, true);
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

        private void radioButton_Diff_Show_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Diff_Show.Checked)
            {
                this.Size = new Size(1554, 723);
            }
        }

        private void radioButton_Diff_Hide_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Diff_Hide.Checked)
            {
                this.Size = new Size(1109, 723);
            }
        }
        public void dataGridView_Gamma_Vreg1_Diff_Set_Init_Vreg1(int band,int Vreg1)
        {
            if (band < 8) dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (0 + 2)].Cells[4].Value = Vreg1;
            
        }

        public void dataGridView_Gamma_Vreg1_Diff_Set_Diff_Vreg1(int band, int Diff_Vreg1)
        {
            if (band < 8) dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (0 + 2)].Cells[8].Value = Diff_Vreg1;
        }
        public void dataGridView_Gamma_Vreg1_Diff_Set_Diff_RGB(int band,int gray, int Diff_R, int Diff_G, int Diff_B)
        {
            dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[5].Value = Diff_R;
            dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[6].Value = Diff_G;
            dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[7].Value = Diff_B;
        }

        public void dataGridView_Gamma_Vreg1_Diff_Set_Band_Init_RGB(int band)
        {
            for (int gray = 0; gray < 10; gray++)
            {
                dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[1].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[1].Value;
                dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[2].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[2].Value;
                dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[3].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[3].Value;
            }
        }

        public void dataGridView_Gamma_Vreg1_Diff_Set_All_Init_RGB()
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[1].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[1].Value;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[2].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[2].Value;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[3].Value = dataGridView_OC_param.Rows[band * 10 + (gray + 2)].Cells[3].Value;
                }
            }   
        }

        public void Gamma_Vreg1_Diff_Clear()
        {
            for (int band = 0; band < 12; band++)
            {
                for (int gray = 0; gray < 10; gray++)
                {
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[6].Value = string.Empty;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[7].Value = string.Empty;
                    dataGridView_Gamma_Vreg1_Diff.Rows[band * 10 + (gray + 2)].Cells[8].Value = string.Empty;
                }
            }

        }

        private void dataGridView_Band_OC_Viewer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    


     

    }
}
