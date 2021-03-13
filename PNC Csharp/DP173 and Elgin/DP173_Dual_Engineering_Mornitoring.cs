using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public partial class DP173_Dual_Engineering_Mornitoring : Form
    {
        Color Color_Set1 = Color.FromArgb(255, 150, 150);
        Color Color_Set2 = Color.FromArgb(255, 200, 150);
        Color Color_Set3 = Color.FromArgb(175, 175, 255);
        Color Color_Set4 = Color.FromArgb(150, 200, 255);
        Color Color_Set5 = Color.FromArgb(200, 255, 200);
        Color Color_Set6 = Color.FromArgb(50, 255, 200);

        private XYLv[,] Limit_Condition1 = new XYLv[14, 8]; //14ea Bands , 8ea Gray-points
        private XYLv[,] Limit_Condition2 = new XYLv[14, 8]; //14ea Bands , 8ea Gray-points
        private XYLv[,] Limit_Condition3 = new XYLv[14, 8]; //14ea Bands , 8ea Gray-points
        private XYLv[,] Limit_Condition4 = new XYLv[14, 8]; //14ea Bands , 8ea Gray-points
        private XYLv[,] Limit_Condition5 = new XYLv[14, 8]; //14ea Bands , 8ea Gray-points
        private XYLv[,] Limit_Condition6 = new XYLv[14, 8]; //14ea Bands , 8ea Gray-points
        
        public RGB[,] All_band_gray_Gamma = new RGB[14, 8]; //14ea Bands , 8ea Gray-points

        private static DP173_Dual_Engineering_Mornitoring Instance;
        public static DP173_Dual_Engineering_Mornitoring getInstance()
        {
            if (Instance == null)
                Instance = new DP173_Dual_Engineering_Mornitoring();
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
        private DP173_Dual_Engineering_Mornitoring()
        {
            InitializeComponent();
        }


        private void DP173_Dual_Engineering_Mornitoring_Load(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            BackColor = f1.current_model.Get_Back_Ground_Color();
            button_Read_OC_Param_From_Excel_File.PerformClick();

            Engineer_Mode_Mornitor_View_Set_Tema_Change(Gamma_Set.Set1);
            Engineer_Mode_Mornitor_View_Set_Tema_Change(Gamma_Set.Set2);
            Engineer_Mode_Grid_Tema_Change();
            OC_DataGridView_Not_Sortable();

            //Get First Limit Values
            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    Limit_Condition1[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[10].Value);
                    Limit_Condition1[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[11].Value);
                    Limit_Condition1[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[12].Value);

                    Limit_Condition2[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[10].Value);
                    Limit_Condition2[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[11].Value);
                    Limit_Condition2[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[12].Value);

                    Limit_Condition3[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[10].Value);
                    Limit_Condition3[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[11].Value);
                    Limit_Condition3[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[12].Value);

                    Limit_Condition4[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[10].Value);
                    Limit_Condition4[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[11].Value);
                    Limit_Condition4[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[12].Value);

                    Limit_Condition5[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[10].Value);
                    Limit_Condition5[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[11].Value);
                    Limit_Condition5[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[12].Value);

                    Limit_Condition6[band, gray].double_X = Convert.ToDouble(dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[10].Value);
                    Limit_Condition6[band, gray].double_Y = Convert.ToDouble(dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[11].Value);
                    Limit_Condition6[band, gray].double_Lv = Convert.ToDouble(dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[12].Value);

                }
            }
        }
        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Copy_Data_Grid_View(int Offset_Row, Gamma_Set Set)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Set);
            DataGridView dataGridView_OC_param_Set = Get_OC_Param_DataGridView(Set);

            Engineer_Mode_Mornitor_View_Set_Tema_Change(Set);

            for (int j = 0; j < dataGridView_Band_OC_Viewer.ColumnCount; j++)
                for (int i = 2; i < dataGridView_Band_OC_Viewer.RowCount; i++)
                    dataGridView_Band_OC_Viewer.Rows[i].Cells[j].Value = dataGridView_OC_param_Set.Rows[i + Offset_Row].Cells[j].Value;
        }

        /// <summary>
        /// Set1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radiobutton_Band0_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 0;
            if (radiobutton_Band0_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band1_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 1;
            if (radiobutton_Band1_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band2_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 2;
            if (radiobutton_Band2_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band3_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 3;
            if (radiobutton_Band3_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band4_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 4;
            if (radiobutton_Band4_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band5_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 5;
            if (radiobutton_Band5_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band6_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 6;
            if (radiobutton_Band6_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band7_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 7;
            if (radiobutton_Band7_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band8_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 8;
            if (radiobutton_Band8_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band9_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 9;
            if (radiobutton_Band9_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_Band10_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 10;
            if (radiobutton_Band10_Set1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_AOD0_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 11;
            if (radiobutton_AOD0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_AOD1_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 12;
            if (radiobutton_AOD1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }

        private void radiobutton_AOD2_Set1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 13;
            if (radiobutton_AOD2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set1);
            }
        }


        
        /// <summary>
        /// Set2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radiobutton_Band0_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 0;
            if (radiobutton_Band0_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band1_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 1;
            if (radiobutton_Band1_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band2_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 2;
            if (radiobutton_Band2_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band3_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 3;
            if (radiobutton_Band3_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band4_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 4;
            if (radiobutton_Band4_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band5_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 5;
            if (radiobutton_Band5_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band6_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 6;
            if (radiobutton_Band6_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band7_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 7;
            if (radiobutton_Band7_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band8_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 8;
            if (radiobutton_Band8_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band9_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 9;
            if (radiobutton_Band9_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        private void radiobutton_Band10_Set2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 10;
            if (radiobutton_Band10_Set2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set2);
            }
        }

        /// <summary>
        /// Set3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public void Force_Triger_Band_Set_CheckedChanged_Function(int band,Gamma_Set Set)
        {
            int Offset_Row = 8 * band;
            Copy_Data_Grid_View(Offset_Row, Set);
            Application.DoEvents();
        }

        private void radiobutton_Band0_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 0;
            if (radiobutton_Band0_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band1_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 1;
            if (radiobutton_Band1_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band2_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 2;
            if (radiobutton_Band2_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band3_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 3;
            if (radiobutton_Band3_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band4_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 4;
            if (radiobutton_Band4_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band5_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 5;
            if (radiobutton_Band5_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band6_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 6;
            if (radiobutton_Band6_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band7_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 7;
            if (radiobutton_Band7_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band8_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 8;
            if (radiobutton_Band8_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band9_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 9;
            if (radiobutton_Band9_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }

        private void radiobutton_Band10_Set3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 10;
            if (radiobutton_Band10_Set3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set3);
            }
        }
        /// <summary>
        /// Set4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void radiobutton_Band0_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 0;
            if (radiobutton_Band0_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band1_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 1;
            if (radiobutton_Band1_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band2_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 2;
            if (radiobutton_Band2_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band3_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 3;
            if (radiobutton_Band3_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band4_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 4;
            if (radiobutton_Band4_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band5_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 5;
            if (radiobutton_Band5_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band6_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 6;
            if (radiobutton_Band6_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band7_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 7;
            if (radiobutton_Band7_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band8_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 8;
            if (radiobutton_Band8_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band9_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 9;
            if (radiobutton_Band9_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        private void radiobutton_Band10_Set4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 10;
            if (radiobutton_Band10_Set4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set4);
            }
        }

        

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        public void DP173_Get_All_Band_Gray_Gamma(RGB[,] Gamma, Gamma_Set Set)
        {
            DataGridView dataGridView_OC_param_Set = Get_OC_Param_DataGridView(Set);

            for (int band = 0; band < 14; band++)
            {
                if (band >= 11 && (Set != Gamma_Set.Set1)) break;//Set2 ~ Set6

                for (int gray = 0; gray < 8; gray++)
                {
                    Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_Set.Rows[band * 8 + (gray + 2)].Cells[1].Value.ToString());
                    Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_Set.Rows[band * 8 + (gray + 2)].Cells[2].Value.ToString());
                    Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_Set.Rows[band * 8 + (gray + 2)].Cells[3].Value.ToString());
                    Gamma[band, gray].String_Update_From_int();

                }
            }
        }


        public void Dual_RadioButton_All_Enable(bool enable)
        {
            groupBox_Band_Selection_Set1.Enabled = enable;
            groupBox_Band_Selection_Set2.Enabled = enable;
            groupBox_Band_Selection_Set3.Enabled = enable;
            groupBox_Band_Selection_Set4.Enabled = enable;
            groupBox_Band_Selection_Set5.Enabled = enable;
            groupBox_Band_Selection_Set6.Enabled = enable;
        }


        public void Engineer_Mode_Mornitor_View_Set_Tema_Change(Gamma_Set Set)
        {
            for (int i = 4; i <= 6; i++) //Measure X,Y,Lv
            {
                switch (Set)
                {
                    case Gamma_Set.Set1:
                        dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = Color_Set1;
                        break;
                    case Gamma_Set.Set2:
                        dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = Color_Set2;
                        break;
                    case Gamma_Set.Set3:
                        dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = Color_Set3;
                        break;
                    case Gamma_Set.Set4:
                        dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = Color_Set4;
                        break;
                    case Gamma_Set.Set5:
                        dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = Color_Set5;
                        break;
                    case Gamma_Set.Set6:
                        dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = Color_Set6;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
                        break;
                }
            }
        }

        private void Engineer_Mode_Grid_Tema_Change()
        {
            dataGridView_Band_OC_Viewer_1.Columns[0].Width = 80;
            dataGridView_Band_OC_Viewer_2.Columns[0].Width = 80;
            dataGridView_OC_param_Set1.Columns[0].Width = 80;
            dataGridView_OC_param_Set2.Columns[0].Width = 80;
            dataGridView_OC_param_Set3.Columns[0].Width = 80;
            dataGridView_OC_param_Set4.Columns[0].Width = 80;
            dataGridView_OC_param_Set5.Columns[0].Width = 80;
            dataGridView_OC_param_Set6.Columns[0].Width = 80;

            for (int i = 1; i <= 3; i++) //Gamma R,G,B
            {
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_1.Columns[i].Width = 40;

                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_2.Columns[i].Width = 40;

                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set1.Columns[i].Width = 40;

                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set2.Columns[i].Width = 40;

                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set3.Columns[i].Width = 40;

                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set4.Columns[i].Width = 40;

                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set5.Columns[i].Width = 40;

                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set6.Columns[i].Width = 40;
            }

            for (int i = 4; i <= 6; i++) //Measure X,Y,Lv
            {
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_1.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_2.Columns[i].Width = 55;

                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.BackColor = Color_Set1;
                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set1.Columns[i].Width = 55;

                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.BackColor = Color_Set2;
                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set2.Columns[i].Width = 55;

                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.BackColor = Color_Set3;
                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set3.Columns[i].Width = 55;

                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.BackColor = Color_Set4;
                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set4.Columns[i].Width = 55;

                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.BackColor = Color_Set5;
                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set5.Columns[i].Width = 55;

                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.BackColor = Color_Set6;
                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set6.Columns[i].Width = 55;
            }

            for (int i = 7; i <= 9; i++) //Target X,Y,Lv
            {
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_1.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_2.Columns[i].Width = 55;

                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set1.Columns[i].Width = 55;

                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set2.Columns[i].Width = 55;

                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set3.Columns[i].Width = 55;

                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set4.Columns[i].Width = 55;

                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set5.Columns[i].Width = 55;

                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set6.Columns[i].Width = 55;
            }

            for (int i = 10; i <= 12; i++) //Limit X,Y,Lv
            {
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_1.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_2.Columns[i].Width = 55;

                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set1.Columns[i].Width = 55;

                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set2.Columns[i].Width = 55;

                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set3.Columns[i].Width = 55;

                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set4.Columns[i].Width = 55;

                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set5.Columns[i].Width = 55;

                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set6.Columns[i].Width = 55;
            }

            for (int i = 13; i <= 15; i++) //Extension X,Y,Applied
            {
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_1.Columns[i].Width = 55;

                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_Band_OC_Viewer_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_Band_OC_Viewer_2.Columns[i].Width = 55;

                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Set1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set1.Columns[i].Width = 55;

                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Set2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set2.Columns[i].Width = 55;

                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Set3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set3.Columns[i].Width = 55;

                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Set4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set4.Columns[i].Width = 55;

                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Set5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set5.Columns[i].Width = 55;

                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Set6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Set6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Set6.Columns[i].Width = 55;
            }

            //Loop
            dataGridView_Band_OC_Viewer_1.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_Band_OC_Viewer_1.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_Band_OC_Viewer_1.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_Band_OC_Viewer_1.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_Band_OC_Viewer_1.Columns[16].Width = 40;

            dataGridView_Band_OC_Viewer_2.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_Band_OC_Viewer_2.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_Band_OC_Viewer_2.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_Band_OC_Viewer_2.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_Band_OC_Viewer_2.Columns[16].Width = 40;

            dataGridView_OC_param_Set1.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Set1.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Set1.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Set1.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Set1.Columns[16].Width = 40;

            dataGridView_OC_param_Set2.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Set2.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Set2.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Set2.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Set2.Columns[16].Width = 40;

            dataGridView_OC_param_Set3.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Set3.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Set3.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Set3.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Set3.Columns[16].Width = 40;

            dataGridView_OC_param_Set4.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Set4.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Set4.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Set4.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Set4.Columns[16].Width = 40;

            dataGridView_OC_param_Set5.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Set5.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Set5.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Set5.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Set5.Columns[16].Width = 40;

            dataGridView_OC_param_Set6.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Set6.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Set6.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Set6.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Set6.Columns[16].Width = 40;
        }



        private void OC_DataGridView_Not_Sortable()
        {
            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Set1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Set2.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Set3.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Set4.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Set5.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Set6.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_Band_OC_Viewer_1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_Band_OC_Viewer_2.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }


        public void Band_Radiobuttion_Select(int band, Gamma_Set Set)
        {
            if (band == 11)
            {
                radiobutton_AOD0.Checked = true;
            }
            else if (band == 12)
            {
                radiobutton_AOD1.Checked = true;
            }
            else if (band == 13)
            {
                radiobutton_AOD2.Checked = true;
            }
            else if (Set == Gamma_Set.Set1)
            {
                switch (band)
                {
                    case 0:
                        radiobutton_Band0_Set1.Checked = true;
                        break;
                    case 1:
                        radiobutton_Band1_Set1.Checked = true;
                        break;
                    case 2:
                        radiobutton_Band2_Set1.Checked = true;
                        break;
                    case 3:
                        radiobutton_Band3_Set1.Checked = true;
                        break;
                    case 4:
                        radiobutton_Band4_Set1.Checked = true;
                        break;
                    case 5:
                        radiobutton_Band5_Set1.Checked = true;
                        break;
                    case 6:
                        radiobutton_Band6_Set1.Checked = true;
                        break;
                    case 7:
                        radiobutton_Band7_Set1.Checked = true;
                        break;
                    case 8:
                        radiobutton_Band8_Set1.Checked = true;
                        break;
                    case 9:
                        radiobutton_Band9_Set1.Checked = true;
                        break;
                    case 10:
                        radiobutton_Band10_Set1.Checked = true;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Set1 Band Out of Boundary (band>13 or band<0)");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set2)
            {
                switch (band)
                {
                    case 0:
                        radiobutton_Band0_Set2.Checked = true;
                        break;
                    case 1:
                        radiobutton_Band1_Set2.Checked = true;
                        break;
                    case 2:
                        radiobutton_Band2_Set2.Checked = true;
                        break;
                    case 3:
                        radiobutton_Band3_Set2.Checked = true;
                        break;
                    case 4:
                        radiobutton_Band4_Set2.Checked = true;
                        break;
                    case 5:
                        radiobutton_Band5_Set2.Checked = true;
                        break;
                    case 6:
                        radiobutton_Band6_Set2.Checked = true;
                        break;
                    case 7:
                        radiobutton_Band7_Set2.Checked = true;
                        break;
                    case 8:
                        radiobutton_Band8_Set2.Checked = true;
                        break;
                    case 9:
                        radiobutton_Band9_Set2.Checked = true;
                        break;
                    case 10:
                        radiobutton_Band10_Set2.Checked = true;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Set2 Band Out of Boundary (band>13 or band<0)");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set3)
            {
                switch (band)
                {
                    case 0:
                        radiobutton_Band0_Set3.Checked = true;
                        break;
                    case 1:
                        radiobutton_Band1_Set3.Checked = true;
                        break;
                    case 2:
                        radiobutton_Band2_Set3.Checked = true;
                        break;
                    case 3:
                        radiobutton_Band3_Set3.Checked = true;
                        break;
                    case 4:
                        radiobutton_Band4_Set3.Checked = true;
                        break;
                    case 5:
                        radiobutton_Band5_Set3.Checked = true;
                        break;
                    case 6:
                        radiobutton_Band6_Set3.Checked = true;
                        break;
                    case 7:
                        radiobutton_Band7_Set3.Checked = true;
                        break;
                    case 8:
                        radiobutton_Band8_Set3.Checked = true;
                        break;
                    case 9:
                        radiobutton_Band9_Set3.Checked = true;
                        break;
                    case 10:
                        radiobutton_Band10_Set3.Checked = true;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Set3 Band Out of Boundary (band>13 or band<0)");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set4)
            {
                switch (band)
                {
                    case 0:
                        radiobutton_Band0_Set4.Checked = true;
                        break;
                    case 1:
                        radiobutton_Band1_Set4.Checked = true;
                        break;
                    case 2:
                        radiobutton_Band2_Set4.Checked = true;
                        break;
                    case 3:
                        radiobutton_Band3_Set4.Checked = true;
                        break;
                    case 4:
                        radiobutton_Band4_Set4.Checked = true;
                        break;
                    case 5:
                        radiobutton_Band5_Set4.Checked = true;
                        break;
                    case 6:
                        radiobutton_Band6_Set4.Checked = true;
                        break;
                    case 7:
                        radiobutton_Band7_Set4.Checked = true;
                        break;
                    case 8:
                        radiobutton_Band8_Set4.Checked = true;
                        break;
                    case 9:
                        radiobutton_Band9_Set4.Checked = true;
                        break;
                    case 10:
                        radiobutton_Band10_Set4.Checked = true;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Set4 Band Out of Boundary (band>13 or band<0)");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set5)
            {
                switch (band)
                {
                    case 0:
                        radiobutton_Band0_Set5.Checked = true;
                        break;
                    case 1:
                        radiobutton_Band1_Set5.Checked = true;
                        break;
                    case 2:
                        radiobutton_Band2_Set5.Checked = true;
                        break;
                    case 3:
                        radiobutton_Band3_Set5.Checked = true;
                        break;
                    case 4:
                        radiobutton_Band4_Set5.Checked = true;
                        break;
                    case 5:
                        radiobutton_Band5_Set5.Checked = true;
                        break;
                    case 6:
                        radiobutton_Band6_Set5.Checked = true;
                        break;
                    case 7:
                        radiobutton_Band7_Set5.Checked = true;
                        break;
                    case 8:
                        radiobutton_Band8_Set5.Checked = true;
                        break;
                    case 9:
                        radiobutton_Band9_Set5.Checked = true;
                        break;
                    case 10:
                        radiobutton_Band10_Set5.Checked = true;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Set5 Band Out of Boundary (band>13 or band<0)");
                        break;
                }
            }
            else if (Set == Gamma_Set.Set6)
            {
                switch (band)
                {
                    case 0:
                        radiobutton_Band0_Set6.Checked = true;
                        break;
                    case 1:
                        radiobutton_Band1_Set6.Checked = true;
                        break;
                    case 2:
                        radiobutton_Band2_Set6.Checked = true;
                        break;
                    case 3:
                        radiobutton_Band3_Set6.Checked = true;
                        break;
                    case 4:
                        radiobutton_Band4_Set6.Checked = true;
                        break;
                    case 5:
                        radiobutton_Band5_Set6.Checked = true;
                        break;
                    case 6:
                        radiobutton_Band6_Set6.Checked = true;
                        break;
                    case 7:
                        radiobutton_Band7_Set6.Checked = true;
                        break;
                    case 8:
                        radiobutton_Band8_Set6.Checked = true;
                        break;
                    case 9:
                        radiobutton_Band9_Set6.Checked = true;
                        break;
                    case 10:
                        radiobutton_Band10_Set6.Checked = true;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Set6 Band Out of Boundary (band>13 or band<0)");
                        break;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
            }
        }


        public void Dual_Mode_GridView_Measure_Applied_Loop_Area_Data_Clear()
        {
            for (int band = 0; band < 14; band++)
            {
                for (int gray = 0; gray < 8; gray++)
                {
                    //Measure Mornitor 1/2 Clear
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[4].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[5].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[6].Value = string.Empty;

                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[4].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[5].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[6].Value = string.Empty;

                    //Extension Loopcount Mornitor 1/2 Clear
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[15].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[16].Value = string.Empty;

                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[15].Value = string.Empty;
                    dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[16].Value = string.Empty;

                    //Measure Set 1/2/3/4 Clear
                    dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value = string.Empty;
                    
                    dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[6].Value = string.Empty;
                    
                    dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[6].Value = string.Empty;
                    
                    dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[6].Value = string.Empty;

                    dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[6].Value = string.Empty;

                    dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[4].Value = string.Empty;
                    dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[5].Value = string.Empty;
                    dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[6].Value = string.Empty;

                    //Extension Loopcount Set 1/2/3/4 Clear
                    dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[16].Value = string.Empty;

                    dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[16].Value = string.Empty;

                    dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[16].Value = string.Empty;

                    dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[16].Value = string.Empty;

                    dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[16].Value = string.Empty;

                    dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[15].Value = string.Empty;
                    dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[16].Value = string.Empty;
                }
            }
        }

        public void Dual_Engineering_Mode_DataGridview_ReadOnly(bool ReadOnly)
        {
            this.dataGridView_OC_param_Set1.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_Set2.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_Set3.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_Set4.ReadOnly = ReadOnly;
        }

        public void Get_OC_Param_DP173(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
, ref double Limit_Y, ref double Limit_Lv, ref double Extension_X, ref double Extension_Y, Gamma_Set Set)
        {
            //Get Param according to gray
            Get_OC_Param_By_Gray_DP173(gray, ref Gamma_R, ref Gamma_G, ref Gamma_B,
                ref Target_X, ref Target_Y, ref Target_Lv, ref Limit_X, ref Limit_Y, ref Limit_Lv, ref Extension_X, ref Extension_Y, Set);
        }

        public void Get_Gamma_Only_DP173(int band, int gray, RGB Gamma, Gamma_Set Set)
        {
            Get_Gamma_Only_DP173(band, gray, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Set);
            Gamma.String_Update_From_int();
        }

        public void Updata_Sub_To_Main_GridView(int band, int gray, Gamma_Set Set)
        {
            if (Set == Gamma_Set.Set1 || Set == Gamma_Set.Set3 || Set == Gamma_Set.Set5)
            {
                int Offset_Row = 8 * band;
                for (int j = 0; j < dataGridView_Band_OC_Viewer_1.ColumnCount; j++)
                {
                    if (Set == Gamma_Set.Set1) dataGridView_OC_param_Set1.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[j].Value;
                    else if (Set == Gamma_Set.Set3) dataGridView_OC_param_Set3.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[j].Value;
                    else if (Set == Gamma_Set.Set5) dataGridView_OC_param_Set5.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[j].Value;
                }
            }
            else if (Set == Gamma_Set.Set2 || Set == Gamma_Set.Set4 || Set == Gamma_Set.Set6)
            {
                int Offset_Row = 8 * band;
                for (int j = 0; j < dataGridView_Band_OC_Viewer_2.ColumnCount; j++)
                {
                    if (Set == Gamma_Set.Set2) dataGridView_OC_param_Set2.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[j].Value;
                    else if (Set == Gamma_Set.Set4) dataGridView_OC_param_Set4.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[j].Value;
                    else if (Set == Gamma_Set.Set6) dataGridView_OC_param_Set6.Rows[gray + 2 + Offset_Row].Cells[j].Value = dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[j].Value;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
            }
        }


        public void Dual_Update_Viewer_Sheet_form_OC_Sheet(int band, Gamma_Set Set)
        {
            int Offset_Row = 8 * band;
            Copy_Data_Grid_View(Offset_Row, Set); 
        }



        public void Dual_Mode_Gamma_Copy_Set1_to_Set4(int band, int gray, int Green_Offset = 0)
        {
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[2].Value) + Green_Offset;
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Set2_to_Set5(int band, int gray, int Green_Offset = 0)
        {
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[2].Value) + Green_Offset;
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Set3_to_Set6(int band, int gray, int Green_Offset = 0)
        {
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[2].Value) + Green_Offset;
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Set1_to_Set2(int band, int gray, int Green_Offset = 0)
        {
            dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[2].Value) + Green_Offset;
            dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Set1_to_Set3(int band, int gray, int Green_Offset = 0)
        {
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[2].Value) + Green_Offset;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Set2_to_Set4(int band, int gray)
        {
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[2].Value);
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Set5_to_Set6(int band, int gray)
        {
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[2].Value);
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Set2_to_Set3(int band, int gray)
        {
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[2].Value);
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[3].Value);
        }


        public void Dual_Copy_Set1_Measure_To_Set3_Measure(int band, int gray)
        {
            //Copy Set1 to Set3
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set2_Measure_To_Set4_Measure(int band, int gray)
        {
            //Copy Set2 to Set4
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set5_Measure_To_Set6_Measure(int band, int gray)
        {
            //Copy Set5 to Set6
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set1_Measure_To_Set4_Measure(int band, int gray)
        {
            //Copy Set5 to Set6
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set2_Measure_To_Set5_Measure(int band, int gray)
        {
            //Copy Set5 to Set6
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set3_Measure_To_Set6_Measure(int band, int gray)
        {
            //Copy Set5 to Set6
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }


        public XYLv Get_Set_Measured_Values(Gamma_Set Set,int band,int gray)
        {
            XYLv temp = new XYLv();

            if(Set == Gamma_Set.Set1)
            {
                temp.double_X = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value);
                temp.double_Y = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value);
                temp.double_Lv = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value);
            }
            else if(Set == Gamma_Set.Set2)
            {
                temp.double_X = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[4].Value);
                temp.double_Y = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[5].Value);
                temp.double_Lv = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[6].Value);
            }
            else if(Set == Gamma_Set.Set3)
            {
                temp.double_X = Convert.ToDouble(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[4].Value);
                temp.double_Y = Convert.ToDouble(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[5].Value);
                temp.double_Lv = Convert.ToDouble(dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[6].Value);
            }
            else if (Set == Gamma_Set.Set4)
            {
                temp.double_X = Convert.ToDouble(dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[4].Value);
                temp.double_Y = Convert.ToDouble(dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[5].Value);
                temp.double_Lv = Convert.ToDouble(dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[6].Value);
            }
            temp.String_Update_From_Double();
            return temp;
        }


        public void Dual_Copy_Set1_Measure_To_Set23456Target(int band, int gray)
        {
            Dual_Copy_Set1_Measure_To_Set2Target(band, gray);
            Dual_Copy_Set1_Measure_To_Set3Target(band, gray);
            Dual_Copy_Set1_Measure_To_Set4Target(band, gray);
            Dual_Copy_Set1_Measure_To_Set5Target(band, gray);
            Dual_Copy_Set1_Measure_To_Set6Target(band, gray);
        }

        public void Dual_Copy_Set1_Measure_To_Set2Target(int band, int gray)
        {
            //Copy Set1 to Set2
            dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[7].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[8].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[9].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set1_Measure_To_Set3Target(int band, int gray)
        {
            //Copy Set1 to Set3
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[7].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[8].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[9].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set1_Measure_To_Set4Target(int band, int gray)
        {
            //Copy Set1 to Set4
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[7].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[8].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set4.Rows[band * 8 + (gray + 2)].Cells[9].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set1_Measure_To_Set5Target(int band, int gray)
        {
            //Copy Set1 to Set5
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[7].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[8].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set5.Rows[band * 8 + (gray + 2)].Cells[9].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set1_Measure_To_Set6Target(int band, int gray)
        {
            //Copy Set1 to Set6
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[7].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[8].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set6.Rows[band * 8 + (gray + 2)].Cells[9].Value = dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set2_Measure_To_Set3Target(int band, int gray)
        {
            //Copy Set2 to Set3
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[7].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[8].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[9].Value = dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Set1_Set2_Ave_Measure_To_Set3Target(int band, int gray)
        {
            XYLv Set1_M_XYLv = new XYLv();
            XYLv Set2_M_XYLv = new XYLv();
            XYLv Set3_T_XYLv = new XYLv();

            Set1_M_XYLv.double_X = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[4].Value);
            Set1_M_XYLv.double_Y = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[5].Value);
            Set1_M_XYLv.double_Lv = Convert.ToDouble(dataGridView_OC_param_Set1.Rows[band * 8 + (gray + 2)].Cells[6].Value);

            Set2_M_XYLv.double_X = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[4].Value);
            Set2_M_XYLv.double_Y = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[5].Value);
            Set2_M_XYLv.double_Lv = Convert.ToDouble(dataGridView_OC_param_Set2.Rows[band * 8 + (gray + 2)].Cells[6].Value);

            Set3_T_XYLv.double_X = ((Set1_M_XYLv.double_X + Set2_M_XYLv.double_X)/2.0);
            Set3_T_XYLv.double_Y = ((Set1_M_XYLv.double_Y + Set2_M_XYLv.double_Y)/2.0);
            Set3_T_XYLv.double_Lv = ((Set1_M_XYLv.double_Lv + Set2_M_XYLv.double_Lv)/2.0);

            //Copy Set2 to Set3
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[7].Value = Set3_T_XYLv.double_X;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[8].Value = Set3_T_XYLv.double_Y;
            dataGridView_OC_param_Set3.Rows[band * 8 + (gray + 2)].Cells[9].Value = Set3_T_XYLv.double_Lv;
        }



        public DataGridView Get_Viewer_DataGridView(Gamma_Set Set)
        {
            if (Set == Gamma_Set.Set1 || Set == Gamma_Set.Set3 || Set == Gamma_Set.Set5)
            {
                return dataGridView_Band_OC_Viewer_1;
            }
            else if (Set == Gamma_Set.Set2 || Set == Gamma_Set.Set4 || Set == Gamma_Set.Set6)
            {
                return dataGridView_Band_OC_Viewer_2;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
                return null;
            }
        }

        public DataGridView Get_OC_Param_DataGridView(Gamma_Set Set)
        {
            if (Set == Gamma_Set.Set1)
            {
                return dataGridView_OC_param_Set1;
            }
            else if (Set == Gamma_Set.Set2)
            {
                return dataGridView_OC_param_Set2;
            }
            else if (Set == Gamma_Set.Set3)
            {
                return dataGridView_OC_param_Set3;
            }
            else if (Set == Gamma_Set.Set4)
            {
                return dataGridView_OC_param_Set4;
            }
            else if (Set == Gamma_Set.Set5)
            {
                return dataGridView_OC_param_Set5;
            }
            else if (Set == Gamma_Set.Set6)
            {
                return dataGridView_OC_param_Set6;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
                return null;
            }
        }



        public void Set_OC_Param_DP173(int gray, int Gamma_R, int Gamma_G, int Gamma_B,
    double Measure_X, double Measure_Y, double Measure_Lv, int loop_count, string Extension_Applied, Gamma_Set Set)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Set);

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value = Gamma_R;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value = Gamma_G;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value = Gamma_B;

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[4].Value = Measure_X;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[5].Value = Measure_Y;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[6].Value = Measure_Lv;

            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[15].Value = Extension_Applied;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[16].Value = loop_count;
        }

        public void Get_Gamma_Only_DP173(int band, int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, Gamma_Set Set)
        {
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Set);
            Gamma_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value.ToString());
            Gamma_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value.ToString());
            Gamma_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value.ToString());
        }

        private void Get_OC_Param_By_Gray_DP173(int gray, ref int Gamma_R, ref int Gamma_G, ref int Gamma_B, ref double Target_X, ref double Target_Y, ref double Target_Lv, ref double Limit_X
, ref double Limit_Y, ref double Limit_Lv, ref double Extension_X, ref double Extension_Y, Gamma_Set Set)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Set);

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






        public void Get_Band_Gray_Gamma(RGB[,] Gamma, int band, Gamma_Set Set)
        {
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Set);
            for (int gray = 0; gray < 8; gray++)
            {
                Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[1].Value.ToString());
                Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[2].Value.ToString());
                Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * 8 + (gray + 2)].Cells[3].Value.ToString());
                Gamma[band, gray].String_Update_From_int();
            }
        }


        public void Copy_Previous_Band_Gamma(int band,Gamma_Set Set)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Set);
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Set);

            int Prev_Offset_Row = 8 * (band - 1);
            int Offset_Row = 8 * band;
            for (int j = 1; j <= 3; j++)
            {
                for (int i = 2; i < dataGridView_Band_OC_Viewer.RowCount; i++)
                {
                    dataGridView_OC_param.Rows[i + Offset_Row].Cells[j].Value = dataGridView_OC_param.Rows[i + Prev_Offset_Row].Cells[j].Value;
                    dataGridView_Band_OC_Viewer.Rows[i].Cells[j].Value = dataGridView_OC_param.Rows[i + Offset_Row].Cells[j].Value;
                }
            }
        }






        //Dual
        public string Dual_Get_BX_GXXX_By_Gray_DP173(int gray, Gamma_Set Set)
        {
            if (Set == Gamma_Set.Set1 || Set == Gamma_Set.Set3 || Set == Gamma_Set.Set5) return dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[0].Value.ToString();
            else return dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[0].Value.ToString();
            
        }

        public void Set1_HBM_Update_Gamma(RGB[] Gamma)
        {
            for (int gray = 0; gray <= 6; gray++)
            {
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[1].Value = Gamma[gray].int_R;
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[2].Value = Gamma[gray].int_G;
                dataGridView_Band_OC_Viewer_1.Rows[gray + 2].Cells[3].Value = Gamma[gray].int_B;
            }
            dataGridView_Band_OC_Viewer_1.Rows[7 + 2].Cells[1].Value = Gamma[8].int_R;
            dataGridView_Band_OC_Viewer_1.Rows[7 + 2].Cells[2].Value = Gamma[8].int_G;
            dataGridView_Band_OC_Viewer_1.Rows[7 + 2].Cells[3].Value = Gamma[8].int_B;

        }
        public void Set2_HBM_Update_Gamma(RGB[] Gamma)
        {
            for (int gray = 0; gray <= 6; gray++)
            {
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[1].Value = Gamma[gray].int_R;
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[2].Value = Gamma[gray].int_G;
                dataGridView_Band_OC_Viewer_2.Rows[gray + 2].Cells[3].Value = Gamma[gray].int_B;
            }
            dataGridView_Band_OC_Viewer_2.Rows[7 + 2].Cells[1].Value = Gamma[8].int_R;
            dataGridView_Band_OC_Viewer_2.Rows[7 + 2].Cells[2].Value = Gamma[8].int_G;
            dataGridView_Band_OC_Viewer_2.Rows[7 + 2].Cells[3].Value = Gamma[8].int_B;
        }




        public void button_Read_OC_Param_From_Excel_File_Perform_Click()
        {
            button_Read_OC_Param_From_Excel_File.PerformClick();
        }

        private void button_Read_OC_Param_From_Excel_File_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.current_model.Read_OC_Param_From_Excel_For_Dual_Mode();

            if (f1.current_model.Get_Current_Model_Name() == Model_Name.Elgin) this.Text = "Elgin Dual Engineering Form";
            else this.Text = "DP173 Dual Engineering Form";

            Band_Radiobuttion_Select(0, Gamma_Set.Set1); //Select Set1 Band as 0
            Force_Triger_Band_Set_CheckedChanged_Function(0, Gamma_Set.Set1);

            Band_Radiobuttion_Select(0, Gamma_Set.Set2); //Select Set2 Band as 0
            Force_Triger_Band_Set_CheckedChanged_Function(0, Gamma_Set.Set2);

            Band_Radiobuttion_Select(0, Gamma_Set.Set3); //Select Set3 Band as 0
            Band_Radiobuttion_Select(0, Gamma_Set.Set4); //Select Set4 Band as 0
            Band_Radiobuttion_Select(0, Gamma_Set.Set5); //Select Set3 Band as 0
            Band_Radiobuttion_Select(0, Gamma_Set.Set6); //Select Set4 Band as 0
        }

        private void dataGridView_OC_param_Set1_KeyDown(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (e.KeyData == (Keys.Control | Keys.V)) f1.PasteInData(ref this.dataGridView_OC_param_Set1);
        }

        private void dataGridView_OC_param_Set2_KeyDown(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (e.KeyData == (Keys.Control | Keys.V)) f1.PasteInData(ref this.dataGridView_OC_param_Set2);
        }

        private void dataGridView_OC_param_Set3_KeyDown(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (e.KeyData == (Keys.Control | Keys.V)) f1.PasteInData(ref this.dataGridView_OC_param_Set3);
        }

        private void dataGridView_OC_param_Set4_KeyDown(object sender, KeyEventArgs e)
        {
            //When "ctrl + v" is pressed, it will paste data from clipboard data
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (e.KeyData == (Keys.Control | Keys.V)) f1.PasteInData(ref this.dataGridView_OC_param_Set4);
        }


        private XYLv[,] Get_Limit_Condition(Gamma_Set Set)
        {
             if (Set == Gamma_Set.Set1)
            {
                return Limit_Condition1;
            }
            else if (Set == Gamma_Set.Set2)
            {
                return Limit_Condition2;
            }
            else if (Set == Gamma_Set.Set3)
            {
                return Limit_Condition3;
            }
            else if (Set == Gamma_Set.Set4)
            {
                return Limit_Condition4;
            }
            else if (Set == Gamma_Set.Set5)
            {
                return Limit_Condition5;
            }
            else if (Set == Gamma_Set.Set6)
            {
                return Limit_Condition6;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
                return null;
            }
        }


        private void Limit_Update(double Ratio, bool XY, Gamma_Set Set)
        {
            DataGridView datagridview = Get_OC_Param_DataGridView(Set);
            XYLv[,] Limit_Condition = Get_Limit_Condition(Set);

            for (int band = 0; band < 14; band++)
            {
                if ((Set != Gamma_Set.Set1) && (band > 10)) break;
                for (int gray = 0; gray < 8; gray++)
                {
                    if (XY)
                    {
                        //X
                        if ((Limit_Condition[band, gray].double_X * Ratio) >= 0.99) datagridview.Rows[band * 8 + (gray + 2)].Cells[10].Value = 1;
                        else datagridview.Rows[band * 8 + (gray + 2)].Cells[10].Value = Limit_Condition[band, gray].double_X * Ratio;

                        //Y
                        if ((Limit_Condition[band, gray].double_Y * Ratio) >= 0.99) datagridview.Rows[band * 8 + (gray + 2)].Cells[11].Value = 1;
                        else datagridview.Rows[band * 8 + (gray + 2)].Cells[11].Value = Limit_Condition[band, gray].double_Y * Ratio;
                    }
                    else 
                    {
                        //Lv
                        datagridview.Rows[band * 8 + (gray + 2)].Cells[12].Value = Limit_Condition1[band, gray].double_Lv * Ratio;
                    }
                }
            }
            //update main_Grid_view from sub_Grid_View
            int Current_Band = Get_Current_Band(Set);
            int Offset_Row = 8 * Current_Band;
            Copy_Data_Grid_View(Offset_Row, Set); 
        }

        private int Get_Current_Band(Gamma_Set Set)
        {
            int band = 0;

            if (Set == Gamma_Set.Set1)
            {
                if (radiobutton_Band0_Set1.Checked)
                    band = 0;
                else if (radiobutton_Band1_Set1.Checked)
                    band = 1;
                else if (radiobutton_Band2_Set1.Checked)
                    band = 2;
                else if (radiobutton_Band3_Set1.Checked)
                    band = 3;
                else if (radiobutton_Band4_Set1.Checked)
                    band = 4;
                else if (radiobutton_Band5_Set1.Checked)
                    band = 5;
                else if (radiobutton_Band6_Set1.Checked)
                    band = 6;
                else if (radiobutton_Band7_Set1.Checked)
                    band = 7;
                else if (radiobutton_Band8_Set1.Checked)
                    band = 8;
                else if (radiobutton_Band9_Set1.Checked)
                    band = 9;
                else if (radiobutton_Band10_Set1.Checked)
                    band = 10;
                else if (radiobutton_AOD0.Checked)
                    band = 11;
                else if (radiobutton_AOD1.Checked)
                    band = 12;
                else if (radiobutton_AOD2.Checked)
                    band = 13;
                else
                {
                    //Do nothing 
                }

            }
            else if (Set == Gamma_Set.Set2)
            {
                if (radiobutton_Band0_Set2.Checked)
                    band = 0;
                else if (radiobutton_Band1_Set2.Checked)
                    band = 1;
                else if (radiobutton_Band2_Set2.Checked)
                    band = 2;
                else if (radiobutton_Band3_Set2.Checked)
                    band = 3;
                else if (radiobutton_Band4_Set2.Checked)
                    band = 4;
                else if (radiobutton_Band5_Set2.Checked)
                    band = 5;
                else if (radiobutton_Band6_Set2.Checked)
                    band = 6;
                else if (radiobutton_Band7_Set2.Checked)
                    band = 7;
                else if (radiobutton_Band8_Set2.Checked)
                    band = 8;
                else if (radiobutton_Band9_Set2.Checked)
                    band = 9;
                else if (radiobutton_Band10_Set2.Checked)
                    band = 10;
                else
                {
                    //Do nothing 
                }
            }
            else if (Set == Gamma_Set.Set3)
            {
                if (radiobutton_Band0_Set3.Checked)
                    band = 0;
                else if (radiobutton_Band1_Set3.Checked)
                    band = 1;
                else if (radiobutton_Band2_Set3.Checked)
                    band = 2;
                else if (radiobutton_Band3_Set3.Checked)
                    band = 3;
                else if (radiobutton_Band4_Set3.Checked)
                    band = 4;
                else if (radiobutton_Band5_Set3.Checked)
                    band = 5;
                else if (radiobutton_Band6_Set3.Checked)
                    band = 6;
                else if (radiobutton_Band7_Set3.Checked)
                    band = 7;
                else if (radiobutton_Band8_Set3.Checked)
                    band = 8;
                else if (radiobutton_Band9_Set3.Checked)
                    band = 9;
                else if (radiobutton_Band10_Set3.Checked)
                    band = 10;
                else
                {
                    //Do nothing 
                }
            }
            else if (Set == Gamma_Set.Set4)
            {
                if (radiobutton_Band0_Set4.Checked)
                    band = 0;
                else if (radiobutton_Band1_Set4.Checked)
                    band = 1;
                else if (radiobutton_Band2_Set4.Checked)
                    band = 2;
                else if (radiobutton_Band3_Set4.Checked)
                    band = 3;
                else if (radiobutton_Band4_Set4.Checked)
                    band = 4;
                else if (radiobutton_Band5_Set4.Checked)
                    band = 5;
                else if (radiobutton_Band6_Set4.Checked)
                    band = 6;
                else if (radiobutton_Band7_Set4.Checked)
                    band = 7;
                else if (radiobutton_Band8_Set4.Checked)
                    band = 8;
                else if (radiobutton_Band9_Set4.Checked)
                    band = 9;
                else if (radiobutton_Band10_Set4.Checked)
                    band = 10;
                else
                {
                    //Do nothing 
                }
            }
            else if (Set == Gamma_Set.Set5)
            {
                if (radiobutton_Band0_Set5.Checked)
                    band = 0;
                else if (radiobutton_Band1_Set5.Checked)
                    band = 1;
                else if (radiobutton_Band2_Set5.Checked)
                    band = 2;
                else if (radiobutton_Band3_Set5.Checked)
                    band = 3;
                else if (radiobutton_Band4_Set5.Checked)
                    band = 4;
                else if (radiobutton_Band5_Set5.Checked)
                    band = 5;
                else if (radiobutton_Band6_Set5.Checked)
                    band = 6;
                else if (radiobutton_Band7_Set5.Checked)
                    band = 7;
                else if (radiobutton_Band8_Set5.Checked)
                    band = 8;
                else if (radiobutton_Band9_Set5.Checked)
                    band = 9;
                else if (radiobutton_Band10_Set5.Checked)
                    band = 10;
                else
                {
                    //Do nothing 
                }
            }
            else if (Set == Gamma_Set.Set6)
            {
                if (radiobutton_Band0_Set6.Checked)
                    band = 0;
                else if (radiobutton_Band1_Set6.Checked)
                    band = 1;
                else if (radiobutton_Band2_Set6.Checked)
                    band = 2;
                else if (radiobutton_Band3_Set6.Checked)
                    band = 3;
                else if (radiobutton_Band4_Set6.Checked)
                    band = 4;
                else if (radiobutton_Band5_Set6.Checked)
                    band = 5;
                else if (radiobutton_Band6_Set6.Checked)
                    band = 6;
                else if (radiobutton_Band7_Set6.Checked)
                    band = 7;
                else if (radiobutton_Band8_Set6.Checked)
                    band = 8;
                else if (radiobutton_Band9_Set6.Checked)
                    band = 9;
                else if (radiobutton_Band10_Set6.Checked)
                    band = 10;
                else
                {
                    //Do nothing 
                }
            }

            else
            {
                System.Windows.Forms.MessageBox.Show("Set Should be 1,2,3,4,5 or 6");
            }
            return band;
        }

        /// <summary>
        /// Set1 XY Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void radioButton_Limit_Ratio_150_Set1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150_Set1.Checked)
            {
                Limit_Update(1.5, true, Gamma_Set.Set1);
            }
        }

        private void radioButton_Limit_Ratio_100_Set1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100_Set1.Checked)
            {
                Limit_Update(1.0, true, Gamma_Set.Set1);
            }
        }

        private void radioButton_Limit_Ratio_80_Set1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80_Set1.Checked)
            {
                Limit_Update(0.8, true, Gamma_Set.Set1);
            }
        }

        private void radioButton_Limit_Ratio_60_Set1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60_Set1.Checked)
            {
                Limit_Update(0.6, true, Gamma_Set.Set1);
            }
        }

        /// <summary>
        /// Set2 XY Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Ratio_150_Set2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150_Set2.Checked)
            {
                Limit_Update(1.5, true, Gamma_Set.Set2);
            }
        }

        private void radioButton_Limit_Ratio_100_Set2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100_Set2.Checked)
            {
                Limit_Update(1.0, true, Gamma_Set.Set2);
            }
        }

        private void radioButton_Limit_Ratio_80_Set2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80_Set2.Checked)
            {
                Limit_Update(0.8, true, Gamma_Set.Set2);
            }
        }

        private void radioButton_Limit_Ratio_60_Set2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60_Set2.Checked)
            {
                Limit_Update(0.6, true, Gamma_Set.Set2);
            }
        }
        /// <summary>
        /// Set3 XY Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Ratio_150_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150_Set3.Checked)
            {
                Limit_Update(1.5, true, Gamma_Set.Set3);
            }
        }

        private void radioButton_Limit_Ratio_100_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100_Set3.Checked)
            {
                Limit_Update(1.0, true, Gamma_Set.Set3);
            }
        }

        private void radioButton_Limit_Ratio_80_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80_Set3.Checked)
            {
                Limit_Update(0.8, true, Gamma_Set.Set3);
            }
        }

        private void radioButton_Limit_Ratio_60_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60_Set3.Checked)
            {
                Limit_Update(0.6, true, Gamma_Set.Set3);
            }
        }
        /// <summary>
        /// Set4 XY Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Ratio_150_Set4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150_Set4.Checked)
            {
                Limit_Update(1.5, true, Gamma_Set.Set4);
            }
        }

        private void radioButton_Limit_Ratio_100_Set4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100_Set4.Checked)
            {
                Limit_Update(1.0, true, Gamma_Set.Set4);
            }
        }

        private void radioButton_Limit_Ratio_80_Set4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80_Set4.Checked)
            {
                Limit_Update(0.8, true, Gamma_Set.Set4);
            }
        }

        private void radioButton_Limit_Ratio_60_Set4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60_Set4.Checked)
            {
                Limit_Update(0.6, true, Gamma_Set.Set4);
            }
        }

        /// <summary>
        /// Set5 XY Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Ratio_150_Set5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150_Set5.Checked)
            {
                Limit_Update(1.5, true, Gamma_Set.Set5);
            }
        }

        private void radioButton_Limit_Ratio_100_Set5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100_Set5.Checked)
            {
                Limit_Update(1.0, true, Gamma_Set.Set5);
            }
        }

        private void radioButton_Limit_Ratio_80_Set5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80_Set5.Checked)
            {
                Limit_Update(0.8, true, Gamma_Set.Set5);
            }
        }

        private void radioButton_Limit_Ratio_60_Set5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60_Set5.Checked)
            {
                Limit_Update(0.6, true, Gamma_Set.Set5);
            }
        }

        /// <summary>
        /// Set6 XY Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Ratio_150_Set6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_150_Set6.Checked)
            {
                Limit_Update(1.5, true, Gamma_Set.Set6);
            }
        }

        private void radioButton_Limit_Ratio_100_Set6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_100_Set6.Checked)
            {
                Limit_Update(1.0, true, Gamma_Set.Set6);
            }
        }

        private void radioButton_Limit_Ratio_80_Set6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_80_Set6.Checked)
            {
                Limit_Update(0.8, true, Gamma_Set.Set6);
            }
        }

        private void radioButton_Limit_Ratio_60_Set6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Ratio_60_Set6.Checked)
            {
                Limit_Update(0.6, true, Gamma_Set.Set6);
            }
        }

        /// <summary>
        /// Set1 Lv Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Lv_Ratio_150_Set1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150_Set1.Checked)
            {
                Limit_Update(1.5, false, Gamma_Set.Set1);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_Set1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100_Set1.Checked)
            {
                Limit_Update(1.0, false, Gamma_Set.Set1);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_Set1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80_Set1.Checked)
            {
                Limit_Update(0.8, false, Gamma_Set.Set1);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_Set1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60_Set1.Checked)
            {
                Limit_Update(0.6, false, Gamma_Set.Set1);
            }
        }

        /// <summary>
        /// Set2 Lv Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Lv_Ratio_150_Set2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150_Set2.Checked)
            {
                Limit_Update(1.5, false, Gamma_Set.Set2);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_Set2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100_Set2.Checked)
            {
                Limit_Update(1.0, false, Gamma_Set.Set2);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_Set2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80_Set2.Checked)
            {
                Limit_Update(0.8, false, Gamma_Set.Set2);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_Set2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60_Set2.Checked)
            {
                Limit_Update(0.6, false, Gamma_Set.Set2);
            }
        }

        /// <summary>
        /// Set3 Lv Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Lv_Ratio_150_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150_Set3.Checked)
            {
                Limit_Update(1.5, false, Gamma_Set.Set3);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100_Set3.Checked)
            {
                Limit_Update(1.0, false, Gamma_Set.Set3);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80_Set3.Checked)
            {
                Limit_Update(0.8, false, Gamma_Set.Set3);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_Set3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60_Set3.Checked)
            {
                Limit_Update(0.6, false, Gamma_Set.Set3);
            }
        }

        /// <summary>
        /// Set4 Lv Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Lv_Ratio_150_Set4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150_Set4.Checked)
            {
                Limit_Update(1.5, false, Gamma_Set.Set4);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_Set4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100_Set4.Checked)
            {
                Limit_Update(1.0, false, Gamma_Set.Set4);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_Set4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80_Set4.Checked)
            {
                Limit_Update(0.8, false, Gamma_Set.Set4);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_Set4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60_Set4.Checked)
            {
                Limit_Update(0.6, false, Gamma_Set.Set4);
            }
        }

        /// <summary>
        /// Set5 Lv Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Lv_Ratio_150_Set5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150_Set5.Checked)
            {
                Limit_Update(1.5, false, Gamma_Set.Set5);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_Set5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100_Set5.Checked)
            {
                Limit_Update(1.0, false, Gamma_Set.Set5);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_Set5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80_Set5.Checked)
            {
                Limit_Update(0.8, false, Gamma_Set.Set5);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_Set5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60_Set5.Checked)
            {
                Limit_Update(0.6, false, Gamma_Set.Set5);
            }
        }

        /// <summary>
        /// Set6 Lv Limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Limit_Lv_Ratio_150_Set6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_150_Set6.Checked)
            {
                Limit_Update(1.5, false, Gamma_Set.Set6);
            }
        }

        private void radioButton_Limit_Lv_Ratio_100_Set6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_100_Set6.Checked)
            {
                Limit_Update(1.0, false, Gamma_Set.Set6);
            }
        }

        private void radioButton_Limit_Lv_Ratio_80_Set6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_80_Set6.Checked)
            {
                Limit_Update(0.8, false, Gamma_Set.Set6);
            }
        }

        private void radioButton_Limit_Lv_Ratio_60_Set6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Limit_Lv_Ratio_60_Set6.Checked)
            {
                Limit_Update(0.6, false, Gamma_Set.Set6);
            }
        }

    

        /// <summary>
        /// Set5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void radiobutton_Band0_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 0;
            if (radiobutton_Band0_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band1_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 1;
            if (radiobutton_Band1_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band2_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 2;
            if (radiobutton_Band2_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band3_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 3;
            if (radiobutton_Band3_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band4_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 4;
            if (radiobutton_Band4_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band5_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 5;
            if (radiobutton_Band5_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band6_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 6;
            if (radiobutton_Band6_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band7_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 7;
            if (radiobutton_Band7_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band8_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 8;
            if (radiobutton_Band8_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band9_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 9;
            if (radiobutton_Band9_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        private void radiobutton_Band10_Set5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 10;
            if (radiobutton_Band10_Set5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set5);
            }
        }

        /// <summary>
        /// Set6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void radiobutton_Band0_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 0;
            if (radiobutton_Band0_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band1_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 1;
            if (radiobutton_Band1_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band2_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 2;
            if (radiobutton_Band2_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band3_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 3;
            if (radiobutton_Band3_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band4_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 4;
            if (radiobutton_Band4_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band5_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 5;
            if (radiobutton_Band5_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band6_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 6;
            if (radiobutton_Band6_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band7_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 7;
            if (radiobutton_Band7_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band8_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 8;
            if (radiobutton_Band8_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band9_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 9;
            if (radiobutton_Band9_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }

        private void radiobutton_Band10_Set6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = 8 * 10;
            if (radiobutton_Band10_Set6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, Gamma_Set.Set6);
            }
        }
    }
}
