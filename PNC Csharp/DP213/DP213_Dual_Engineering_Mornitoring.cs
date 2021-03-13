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
    public partial class DP213_Dual_Engineering_Mornitoring : Form
    {
        DP213_OC_Values_Storage storage = DP213_OC_Values_Storage.getInstance();
        DP213_OC_Current_Variables_Structure vars = DP213_OC_Current_Variables_Structure.getInstance();
        
        private Form1 f1() { return (Form1)Application.OpenForms["Form1"]; }
        private DP213_Model_Option_Form dp213_form() { return (DP213_Model_Option_Form)Application.OpenForms["DP213_Model_Option_Form"]; }
        private readonly int max_band_amount = DP213_Static.Max_Band_Amount;
        private readonly int max_gray_amount = DP213_Static.Max_Gray_Amount;


        private static DP213_Dual_Engineering_Mornitoring Instance;
        public static DP213_Dual_Engineering_Mornitoring getInstance()
        {
            if (Instance == null)
                Instance = new DP213_Dual_Engineering_Mornitoring();

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

        private DP213_Dual_Engineering_Mornitoring()
        {
            InitializeComponent();
        }

        private void DP213_Dual_Engineering_Mornitoring_Load(object sender, EventArgs e)
        {
            BackColor = f1().current_model.Get_Back_Ground_Color();
            button_Read_OC_Param_From_Excel_File.PerformClick();
            Engineer_Mode_Grid_Tema_Change();
            OC_DataGridView_Not_Sortable();
        }

        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public DataGridView Get_Viewer_DataGridView(OC_Mode Mode)
        {
            if (Mode == OC_Mode.Mode1 || Mode == OC_Mode.Mode3 || Mode == OC_Mode.Mode5)
            {
                return dataGridView_Band_OC_Viewer_1;
            }
            else if (Mode == OC_Mode.Mode2 || Mode == OC_Mode.Mode4 || Mode == OC_Mode.Mode6)
            {
                return dataGridView_Band_OC_Viewer_2;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("OC Mode Should be 1,2,3,4,5 or 6");
                return null;
            }
        }

        public DataGridView Get_OC_Param_DataGridView(OC_Mode Mode)
        {
            if (Mode == OC_Mode.Mode1)
            {
                return dataGridView_OC_param_Mode_1;
            }
            else if (Mode == OC_Mode.Mode2)
            {
                return dataGridView_OC_param_Mode_2;
            }
            else if (Mode == OC_Mode.Mode3)
            {
                return dataGridView_OC_param_Mode_3;
            }
            else if (Mode == OC_Mode.Mode4)
            {
                return dataGridView_OC_param_Mode_4;
            }
            else if (Mode == OC_Mode.Mode5)
            {
                return dataGridView_OC_param_Mode_5;
            }
            else if (Mode == OC_Mode.Mode6)
            {
                return dataGridView_OC_param_Mode_6;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("OC Mode Should be 1,2,3,4,5 or 6");
                return null;
            }
        }


        private void Copy_Data_Grid_View(int Offset_Row, OC_Mode Mode)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Mode);
            DataGridView dataGridView_OC_param_Set = Get_OC_Param_DataGridView(Mode);

            Engineer_Mode_Mornitor_View_OC_Mode_Tema_Change(Mode);

            for (int j = 0; j < dataGridView_Band_OC_Viewer.ColumnCount; j++)
                for (int i = 2; i < dataGridView_Band_OC_Viewer.RowCount; i++)
                    dataGridView_Band_OC_Viewer.Rows[i].Cells[j].Value = dataGridView_OC_param_Set.Rows[i + Offset_Row].Cells[j].Value;
        }
        /// <summary>
        /// Set1 & AOD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radiobutton_Band0_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 0;
            if (radiobutton_Band0_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band1_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 1;
            if (radiobutton_Band1_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band2_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 2;
            if (radiobutton_Band2_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band3_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 3;
            if (radiobutton_Band3_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band4_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 4;
            if (radiobutton_Band4_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band5_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 5;
            if (radiobutton_Band5_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band6_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 6;
            if (radiobutton_Band6_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band7_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 7;
            if (radiobutton_Band7_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band8_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 8;
            if (radiobutton_Band8_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band9_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 9;
            if (radiobutton_Band9_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band10_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 10;
            if (radiobutton_Band10_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_Band11_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 11;
            if (radiobutton_Band11_Mode_1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_AOD0_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 12;
            if (radiobutton_AOD0.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_AOD1_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 13;
            if (radiobutton_AOD1.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        private void radiobutton_AOD2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 14;
            if (radiobutton_AOD2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode1);
            }
        }

        /// <summary>
        /// Set2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void radiobutton_Band0_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 0;
            if (radiobutton_Band0_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band1_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 1;
            if (radiobutton_Band1_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band2_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 2;
            if (radiobutton_Band2_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band3_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 3;
            if (radiobutton_Band3_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band4_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 4;
            if (radiobutton_Band4_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band5_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 5;
            if (radiobutton_Band5_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band6_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 6;
            if (radiobutton_Band6_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band7_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 7;
            if (radiobutton_Band7_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band8_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 8;
            if (radiobutton_Band8_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band9_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 9;
            if (radiobutton_Band9_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band10_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 10;
            if (radiobutton_Band10_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        private void radiobutton_Band11_Mode2_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 11;
            if (radiobutton_Band11_Mode_2.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode2);
            }
        }

        /// <summary>
        /// Set3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void Force_Triger_Band_Mode_CheckedChanged_Function(int band, OC_Mode Mode)
        {
            if (band >= max_band_amount) MessageBox.Show("Band is out of boundary (band >= 15)");
            else if (band < 0) MessageBox.Show("Band is out of boundary (band < 0)");

            int Offset_Row = max_gray_amount * band;
            Copy_Data_Grid_View(Offset_Row, Mode);
            Application.DoEvents();
        }


        private void radiobutton_Band0_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 0;
            if (radiobutton_Band0_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band1_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 1;
            if (radiobutton_Band1_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band2_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 2;
            if (radiobutton_Band2_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band3_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 3;
            if (radiobutton_Band3_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band4_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 4;
            if (radiobutton_Band4_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band5_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 5;
            if (radiobutton_Band5_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band6_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 6;
            if (radiobutton_Band6_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band7_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 7;
            if (radiobutton_Band7_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band8_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 8;
            if (radiobutton_Band8_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band9_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 9;
            if (radiobutton_Band9_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band10_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 10;
            if (radiobutton_Band10_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }

        private void radiobutton_Band11_Mode3_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 11;
            if (radiobutton_Band11_Mode_3.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode3);
            }
        }


        /// <summary>
        /// Set4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radiobutton_Band0_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 0;
            if (radiobutton_Band0_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band1_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 1;
            if (radiobutton_Band1_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band2_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 2;
            if (radiobutton_Band2_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band3_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 3;
            if (radiobutton_Band3_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band4_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 4;
            if (radiobutton_Band4_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band5_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 5;
            if (radiobutton_Band5_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band6_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 6;
            if (radiobutton_Band6_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band7_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 7;
            if (radiobutton_Band7_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band8_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 8;
            if (radiobutton_Band8_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band9_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 9;
            if (radiobutton_Band9_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band10_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 10;
            if (radiobutton_Band10_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }

        private void radiobutton_Band11_Mode4_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 11;
            if (radiobutton_Band11_Mode_4.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode4);
            }
        }
        /// <summary>
        /// Set5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void radiobutton_Band0_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 0;
            if (radiobutton_Band0_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band1_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 1;
            if (radiobutton_Band1_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band2_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 2;
            if (radiobutton_Band2_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band3_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 3;
            if (radiobutton_Band3_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band4_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 4;
            if (radiobutton_Band4_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band5_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 5;
            if (radiobutton_Band5_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band6_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 6;
            if (radiobutton_Band6_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band7_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 7;
            if (radiobutton_Band7_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band8_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 8;
            if (radiobutton_Band8_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band9_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 9;
            if (radiobutton_Band9_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band10_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 10;
            if (radiobutton_Band10_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }

        private void radiobutton_Band11_Mode5_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 11;
            if (radiobutton_Band11_Mode_5.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode5);
            }
        }
        /// <summary>
        /// Set6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void radiobutton_Band0_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 0;
            if (radiobutton_Band0_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band1_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 1;
            if (radiobutton_Band1_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band2_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 2;
            if (radiobutton_Band2_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band3_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 3;
            if (radiobutton_Band3_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band4_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 4;
            if (radiobutton_Band4_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band5_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 5;
            if (radiobutton_Band5_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band6_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 6;
            if (radiobutton_Band6_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band7_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 7;
            if (radiobutton_Band7_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band8_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 8;
            if (radiobutton_Band8_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band9_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 9;
            if (radiobutton_Band9_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band10_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 10;
            if (radiobutton_Band10_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        private void radiobutton_Band11_Mode6_CheckedChanged(object sender, EventArgs e)
        {
            int Offset_Row = max_gray_amount * 11;
            if (radiobutton_Band11_Mode_6.Checked)
            {
                Copy_Data_Grid_View(Offset_Row, OC_Mode.Mode6);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Gamma"></param>
        /// <param name="Set"></param>
        /// 

        private RGB[, ,] Get_Read_From_Gridviews_All_Band_Gray_Gamma()
        {
            RGB[, ,] Temp_All_band_gray_Gamma = new RGB[DP213_Static.Max_Set_Amount, DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
            for (int band = 0; band < max_band_amount; band++)
            {
                for (int gray = 0; gray < max_gray_amount; gray++)
                {
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set1), band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set1), band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set1), band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set1), band, gray].String_Update_From_int();

                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set2), band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set2), band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set2), band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set2), band, gray].String_Update_From_int();

                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set3), band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set3), band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set3), band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set3), band, gray].String_Update_From_int();

                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set4), band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set4), band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set4), band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set4), band, gray].String_Update_From_int();

                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set5), band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set5), band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set5), band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set5), band, gray].String_Update_From_int();

                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set6), band, gray].int_R = Convert.ToInt16(dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set6), band, gray].int_G = Convert.ToInt16(dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set6), band, gray].int_B = Convert.ToInt16(dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());
                    Temp_All_band_gray_Gamma[Convert.ToInt16(Gamma_Set.Set6), band, gray].String_Update_From_int();
                }
            }
            return Temp_All_band_gray_Gamma;
        }

        public void DP213_Update_All_Band_Gray_Gamma()
        {
            storage.Set_All_band_gray_Gamma(Get_Read_From_Gridviews_All_Band_Gray_Gamma());
        }

        public void Dual_RadioButton_All_Enable(bool enable)
        {
            groupBox_Band_Selection_Mode_1.Enabled = enable;
            groupBox_Band_Selection_Mode_2.Enabled = enable;
            groupBox_Band_Selection_Mode_3.Enabled = enable;
            groupBox_Band_Selection_Mode_4.Enabled = enable;
            groupBox_Band_Selection_Mode_5.Enabled = enable;
            groupBox_Band_Selection_Mode_6.Enabled = enable;
        }

        public void Engineer_Mode_Mornitor_View_OC_Mode_Tema_Change(OC_Mode Mode)
        {
            for (int i = 4; i <= 6; i++) //Measure X,Y,Lv
            {
                switch (Mode)
                {
                    case OC_Mode.Mode1:
                        dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.BackColor;
                        break;
                    case OC_Mode.Mode2:
                        dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.BackColor;
                        break;
                    case OC_Mode.Mode3:
                        dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.BackColor;
                        break;
                    case OC_Mode.Mode4:
                        dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.BackColor;
                        break;
                    case OC_Mode.Mode5:
                        dataGridView_Band_OC_Viewer_1.Columns[i].DefaultCellStyle.BackColor = dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.BackColor;
                        break;
                    case OC_Mode.Mode6:
                        dataGridView_Band_OC_Viewer_2.Columns[i].DefaultCellStyle.BackColor = dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.BackColor;
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("OC_Mode Should be 1,2,3,4,5 or 6");
                        break;
                }
            }
        }

        private void Engineer_Mode_Grid_Tema_Change()
        {
            dataGridView_Band_OC_Viewer_1.Columns[0].Width = 80;
            dataGridView_Band_OC_Viewer_2.Columns[0].Width = 80;
            dataGridView_OC_param_Mode_1.Columns[0].Width = 80;
            dataGridView_OC_param_Mode_2.Columns[0].Width = 80;
            dataGridView_OC_param_Mode_3.Columns[0].Width = 80;
            dataGridView_OC_param_Mode_4.Columns[0].Width = 80;
            dataGridView_OC_param_Mode_5.Columns[0].Width = 80;
            dataGridView_OC_param_Mode_6.Columns[0].Width = 80;

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

                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_1.Columns[i].Width = 40;

                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_2.Columns[i].Width = 40;

                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_3.Columns[i].Width = 40;

                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_4.Columns[i].Width = 40;

                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_5.Columns[i].Width = 40;

                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_6.Columns[i].Width = 40;
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

                //dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.BackColor = DP213_Static.Color_Set1;
                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_1.Columns[i].Width = 55;

                //dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.BackColor = DP213_Static.Color_Set2;
                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_2.Columns[i].Width = 55;

                //dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.BackColor = DP213_Static.Color_Set3;
                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_3.Columns[i].Width = 55;

                //dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.BackColor = DP213_Static.Color_Set4;
                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_4.Columns[i].Width = 55;

                //dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.BackColor = DP213_Static.Color_Set5;
                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_5.Columns[i].Width = 55;

                //dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.BackColor = DP213_Static.Color_Set6;
                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_6.Columns[i].Width = 55;
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

                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_1.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_2.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_3.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_4.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_5.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_6.Columns[i].Width = 55;
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

                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_1.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_2.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_3.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_4.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_5.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_6.Columns[i].Width = 55;
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

                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Mode_1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_1.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Mode_2.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_2.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_2.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Mode_3.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_3.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_3.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Mode_4.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_4.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_4.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Mode_5.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_5.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_5.Columns[i].Width = 55;

                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
                dataGridView_OC_param_Mode_6.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView_OC_param_Mode_6.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                dataGridView_OC_param_Mode_6.Columns[i].Width = 55;
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

            dataGridView_OC_param_Mode_1.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Mode_1.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Mode_1.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Mode_1.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Mode_1.Columns[16].Width = 40;

            dataGridView_OC_param_Mode_2.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Mode_2.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Mode_2.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Mode_2.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Mode_2.Columns[16].Width = 40;

            dataGridView_OC_param_Mode_3.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Mode_3.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Mode_3.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Mode_3.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Mode_3.Columns[16].Width = 40;

            dataGridView_OC_param_Mode_4.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Mode_4.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Mode_4.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Mode_4.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Mode_4.Columns[16].Width = 40;

            dataGridView_OC_param_Mode_5.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Mode_5.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Mode_5.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Mode_5.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Mode_5.Columns[16].Width = 40;

            dataGridView_OC_param_Mode_6.Columns[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView_OC_param_Mode_6.Columns[16].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView_OC_param_Mode_6.Columns[16].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView_OC_param_Mode_6.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridView_OC_param_Mode_6.Columns[16].Width = 40;
        }

        private void OC_DataGridView_Not_Sortable()
        {
            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Mode_1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Mode_2.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Mode_3.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Mode_4.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Mode_5.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_OC_param_Mode_6.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_Band_OC_Viewer_1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in this.dataGridView_Band_OC_Viewer_2.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void Mode_1_Band_Radiobuttion_Select(int band)
        {
            switch (band)
            {
                case 0:
                    radiobutton_Band0_Mode_1.Checked = true;
                    break;
                case 1:
                    radiobutton_Band1_Mode_1.Checked = true;
                    break;
                case 2:
                    radiobutton_Band2_Mode_1.Checked = true;
                    break;
                case 3:
                    radiobutton_Band3_Mode_1.Checked = true;
                    break;
                case 4:
                    radiobutton_Band4_Mode_1.Checked = true;
                    break;
                case 5:
                    radiobutton_Band5_Mode_1.Checked = true;
                    break;
                case 6:
                    radiobutton_Band6_Mode_1.Checked = true;
                    break;
                case 7:
                    radiobutton_Band7_Mode_1.Checked = true;
                    break;
                case 8:
                    radiobutton_Band8_Mode_1.Checked = true;
                    break;
                case 9:
                    radiobutton_Band9_Mode_1.Checked = true;
                    break;
                case 10:
                    radiobutton_Band10_Mode_1.Checked = true;
                    break;
                case 11:
                    radiobutton_Band11_Mode_1.Checked = true;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Set1 Band Out of Boundary (band>13 or band<0)");
                    break;
            }
        }

        private void Mode_2_Band_Radiobuttion_Select(int band)
        {
            switch (band)
            {
                case 0:
                    radiobutton_Band0_Mode_2.Checked = true;
                    break;
                case 1:
                    radiobutton_Band1_Mode_2.Checked = true;
                    break;
                case 2:
                    radiobutton_Band2_Mode_2.Checked = true;
                    break;
                case 3:
                    radiobutton_Band3_Mode_2.Checked = true;
                    break;
                case 4:
                    radiobutton_Band4_Mode_2.Checked = true;
                    break;
                case 5:
                    radiobutton_Band5_Mode_2.Checked = true;
                    break;
                case 6:
                    radiobutton_Band6_Mode_2.Checked = true;
                    break;
                case 7:
                    radiobutton_Band7_Mode_2.Checked = true;
                    break;
                case 8:
                    radiobutton_Band8_Mode_2.Checked = true;
                    break;
                case 9:
                    radiobutton_Band9_Mode_2.Checked = true;
                    break;
                case 10:
                    radiobutton_Band10_Mode_2.Checked = true;
                    break;
                case 11:
                    radiobutton_Band11_Mode_2.Checked = true;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Set2 Band Out of Boundary (band>13 or band<0)");
                    break;
            }
        }

        private void Mode_3_Band_Radiobuttion_Select(int band)
        {
            switch (band)
            {
                case 0:
                    radiobutton_Band0_Mode_3.Checked = true;
                    break;
                case 1:
                    radiobutton_Band1_Mode_3.Checked = true;
                    break;
                case 2:
                    radiobutton_Band2_Mode_3.Checked = true;
                    break;
                case 3:
                    radiobutton_Band3_Mode_3.Checked = true;
                    break;
                case 4:
                    radiobutton_Band4_Mode_3.Checked = true;
                    break;
                case 5:
                    radiobutton_Band5_Mode_3.Checked = true;
                    break;
                case 6:
                    radiobutton_Band6_Mode_3.Checked = true;
                    break;
                case 7:
                    radiobutton_Band7_Mode_3.Checked = true;
                    break;
                case 8:
                    radiobutton_Band8_Mode_3.Checked = true;
                    break;
                case 9:
                    radiobutton_Band9_Mode_3.Checked = true;
                    break;
                case 10:
                    radiobutton_Band10_Mode_3.Checked = true;
                    break;
                case 11:
                    radiobutton_Band11_Mode_3.Checked = true;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Set3 Band Out of Boundary (band>13 or band<0)");
                    break;
            }
        }

        private void Mode_4_Band_Radiobuttion_Select(int band)
        {
            switch (band)
            {
                case 0:
                    radiobutton_Band0_Mode_4.Checked = true;
                    break;
                case 1:
                    radiobutton_Band1_Mode_4.Checked = true;
                    break;
                case 2:
                    radiobutton_Band2_Mode_4.Checked = true;
                    break;
                case 3:
                    radiobutton_Band3_Mode_4.Checked = true;
                    break;
                case 4:
                    radiobutton_Band4_Mode_4.Checked = true;
                    break;
                case 5:
                    radiobutton_Band5_Mode_4.Checked = true;
                    break;
                case 6:
                    radiobutton_Band6_Mode_4.Checked = true;
                    break;
                case 7:
                    radiobutton_Band7_Mode_4.Checked = true;
                    break;
                case 8:
                    radiobutton_Band8_Mode_4.Checked = true;
                    break;
                case 9:
                    radiobutton_Band9_Mode_4.Checked = true;
                    break;
                case 10:
                    radiobutton_Band10_Mode_4.Checked = true;
                    break;
                case 11:
                    radiobutton_Band11_Mode_4.Checked = true;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Set4 Band Out of Boundary (band>13 or band<0)");
                    break;
            }
        }

         private void Mode_5_Band_Radiobuttion_Select(int band)
         {
             switch (band)
             {
                 case 0:
                     radiobutton_Band0_Mode_5.Checked = true;
                     break;
                 case 1:
                     radiobutton_Band1_Mode_5.Checked = true;
                     break;
                 case 2:
                     radiobutton_Band2_Mode_5.Checked = true;
                     break;
                 case 3:
                     radiobutton_Band3_Mode_5.Checked = true;
                     break;
                 case 4:
                     radiobutton_Band4_Mode_5.Checked = true;
                     break;
                 case 5:
                     radiobutton_Band5_Mode_5.Checked = true;
                     break;
                 case 6:
                     radiobutton_Band6_Mode_5.Checked = true;
                     break;
                 case 7:
                     radiobutton_Band7_Mode_5.Checked = true;
                     break;
                 case 8:
                     radiobutton_Band8_Mode_5.Checked = true;
                     break;
                 case 9:
                     radiobutton_Band9_Mode_5.Checked = true;
                     break;
                 case 10:
                     radiobutton_Band10_Mode_5.Checked = true;
                     break;
                 case 11:
                     radiobutton_Band11_Mode_5.Checked = true;
                     break;
                 default:
                     System.Windows.Forms.MessageBox.Show("Set5 Band Out of Boundary (band>13 or band<0)");
                     break;
             }
         }

         private void Mode_6_Band_Radiobuttion_Select(int band)
         {
             switch (band)
             {
                 case 0:
                     radiobutton_Band0_Mode_6.Checked = true;
                     break;
                 case 1:
                     radiobutton_Band1_Mode_6.Checked = true;
                     break;
                 case 2:
                     radiobutton_Band2_Mode_6.Checked = true;
                     break;
                 case 3:
                     radiobutton_Band3_Mode_6.Checked = true;
                     break;
                 case 4:
                     radiobutton_Band4_Mode_6.Checked = true;
                     break;
                 case 5:
                     radiobutton_Band5_Mode_6.Checked = true;
                     break;
                 case 6:
                     radiobutton_Band6_Mode_6.Checked = true;
                     break;
                 case 7:
                     radiobutton_Band7_Mode_6.Checked = true;
                     break;
                 case 8:
                     radiobutton_Band8_Mode_6.Checked = true;
                     break;
                 case 9:
                     radiobutton_Band9_Mode_6.Checked = true;
                     break;
                 case 10:
                     radiobutton_Band10_Mode_6.Checked = true;
                     break;
                 case 11:
                     radiobutton_Band11_Mode_6.Checked = true;
                     break;
                 default:
                     System.Windows.Forms.MessageBox.Show("Mode6 Band Out of Boundary (band>11 or band<0)");
                     break;
             }
         }

         public void Band_Radiobuttion_Select(int band, OC_Mode Mode)
        {
            if (band == 12)
                radiobutton_AOD0.Checked = true;
            else if (band == 13)
                radiobutton_AOD1.Checked = true;
            else if (band == 14)
                radiobutton_AOD2.Checked = true;
            else if (Mode == OC_Mode.Mode1)
                Mode_1_Band_Radiobuttion_Select(band);
            else if (Mode == OC_Mode.Mode2)
                Mode_2_Band_Radiobuttion_Select(band);
            else if (Mode == OC_Mode.Mode3)
                Mode_3_Band_Radiobuttion_Select(band);
            else if (Mode == OC_Mode.Mode4)
                Mode_4_Band_Radiobuttion_Select(band);
            else if (Mode == OC_Mode.Mode5)
                Mode_5_Band_Radiobuttion_Select(band);
            else if (Mode == OC_Mode.Mode6)
                Mode_6_Band_Radiobuttion_Select(band);
            else
                System.Windows.Forms.MessageBox.Show("OC_Mode Should be 1,2,3,4,5 or 6");

            Force_Triger_Band_Mode_CheckedChanged_Function(band, Mode);
            
        }


        private void Dual_Mode_OC_Viewer12_Measure_Applied_Loop_Area_Data_Clear(int gray)
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
        }

        private void Dual_Mode_OC_Param_123456_Measure_Applied_Loop_Area_Data_Clear(int band,int gray)
        {
            //Measure Set 1/2/3/4 Clear
            dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = string.Empty;
            dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = string.Empty;
            dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = string.Empty;

            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = string.Empty;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = string.Empty;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = string.Empty;

            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = string.Empty;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = string.Empty;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = string.Empty;

            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = string.Empty;
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = string.Empty;
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = string.Empty;

            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = string.Empty;
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = string.Empty;
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = string.Empty;

            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = string.Empty;
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = string.Empty;
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = string.Empty;

            //Extension Loopcount Set 1/2/3/4 Clear
            dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[15].Value = string.Empty;
            dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[16].Value = string.Empty;

            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[15].Value = string.Empty;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[16].Value = string.Empty;

            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[15].Value = string.Empty;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[16].Value = string.Empty;

            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[15].Value = string.Empty;
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[16].Value = string.Empty;

            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[15].Value = string.Empty;
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[16].Value = string.Empty;

            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[15].Value = string.Empty;
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[16].Value = string.Empty;
        }

        public void Dual_Mode_GridView_Measure_Loopcount_ExtentionApplied_Clear()
        {
            for (int band = 0; band < max_band_amount; band++)
            {
                for (int gray = 0; gray < max_gray_amount; gray++)
                {
                    Dual_Mode_OC_Viewer12_Measure_Applied_Loop_Area_Data_Clear(gray);
                    Dual_Mode_OC_Param_123456_Measure_Applied_Loop_Area_Data_Clear(band, gray);
                }
            }
        }

        public void Dual_Mode_GridView_Measure_Loopcount_ExtentionApplied_Clear_Without_AOD_Area()
        {
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                for (int gray = 0; gray < max_gray_amount; gray++)
                {
                    Dual_Mode_OC_Viewer12_Measure_Applied_Loop_Area_Data_Clear(gray);
                    Dual_Mode_OC_Param_123456_Measure_Applied_Loop_Area_Data_Clear(band, gray);
                }
            }
        }

        public void Dual_Engineering_Mode_DataGridview_ReadOnly(bool ReadOnly)
        {
            this.dataGridView_OC_param_Mode_1.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_Mode_2.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_Mode_3.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_Mode_4.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_Mode_5.ReadOnly = ReadOnly;
            this.dataGridView_OC_param_Mode_6.ReadOnly = ReadOnly;
        }


        public void Get_Gamma_Target_Limit_Extention_and_Update_Set_All_band_gray_Gamma(int band, int gray, Gamma_Set Set, OC_Mode Mode)
        {
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);

            vars.Gamma.int_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
            vars.Gamma.int_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
            vars.Gamma.int_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());
            storage.Set_All_band_gray_Gamma(Set, band, gray, vars.Gamma);

            vars.Target.double_X = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[7].Value.ToString());
            vars.Target.double_Y = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[8].Value.ToString());
            vars.Target.double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[9].Value.ToString());

            vars.Limit.double_X = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[10].Value.ToString());
            vars.Limit.double_Y = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[11].Value.ToString());
            vars.Limit.double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[12].Value.ToString());

            vars.Extension.double_X = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[13].Value.ToString());
            vars.Extension.double_Y = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[14].Value.ToString());
        }


        public void Update_Target_Lv(int band, int gray, OC_Mode Mode)
        {
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);
            vars.Target.double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[9].Value.ToString());
        }

        public void Get_Gamma_Only_DP213(int band, int gray, RGB Gamma, OC_Mode Mode)
        {
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);
            Gamma.int_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
            Gamma.int_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
            Gamma.int_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());

            Gamma.String_Update_From_int();
        }

        public void Dual_Update_Viewer_Sheet_form_OC_Sheet(int band, OC_Mode Mode)
        {
            int Offset_Row = max_gray_amount * band;
            Copy_Data_Grid_View(Offset_Row, Mode);
        }






        public void Dual_Mode_Gamma_Copy_Mode2_to_Mode5(int band, int gray)
        {
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value);
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Mode3_to_Mode6(int band, int gray)
        {
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value);
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Mode1_to_Mode2(int band, int gray)
        {
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value);
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Mode1_to_Mode2_and_Apply_Offset (int band, int gray, RGB Offset)
        {
            int R = Convert.ToInt32(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value) + Offset.int_R;
            int G = Convert.ToInt32(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value) + Offset.int_G;
            int B = Convert.ToInt32(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value) + Offset.int_B;
           
            if (R > DP213_Static.Gamma_Register_Max || G > DP213_Static.Gamma_Register_Max || B > DP213_Static.Gamma_Register_Max)
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("OC_Mode2 Gamma has reached max reigister(511), OC Fail");
                vars.Optic_Compensation_Stop = true;
            }
            if (R < 0 || G < 0 || B < 0)
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("OC_Mode2 Gamma has reached min reigister(0), OC Fail");
                vars.Optic_Compensation_Stop = true;
            }

            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = R;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = G;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = B;
        }

        public void Dual_Mode_Gamma_Copy_Mode1_to_Mode3(int band, int gray)
        {
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value);
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Mode_Gamma_Copy_Mode1_to_Mode3_and_Apply_Offset(int band, int gray, RGB Offset)
        {
            int R = Convert.ToInt32(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value) + Offset.int_R;
            int G = Convert.ToInt32(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value) + Offset.int_G;
            int B = Convert.ToInt32(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value) + Offset.int_B;
            if (R > DP213_Static.Gamma_Register_Max || G > DP213_Static.Gamma_Register_Max || B > DP213_Static.Gamma_Register_Max)
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("OC_Mode3 Gamma has reached max reigister(511), OC Fail");
                vars.Optic_Compensation_Stop = true;
            }
            if (R < 0 || G < 0 || B < 0)
            {
                f1().Show_NG_Message_and_pop_up_Messagebox_With_NG_Sound("OC_Mode3 Gamma has reached min reigister(0), OC Fail");
                vars.Optic_Compensation_Stop = true;
            }

            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = R;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = G;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = B;
        }

        public void Dual_Mode_Gamma_Copy_Mode1_to_Mode4(int band, int gray)
        {
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value);
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value);
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = Convert.ToInt16(dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value);
        }

        public void Dual_Copy_Mode1_Measure_To_Mode4_Measure(int band, int gray)
        {
            //Copy Set5 to Set6
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Mode_4.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Mode2_Measure_To_Mode5_Measure(int band, int gray)
        {
            //Copy Set5 to Set6
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Mode_5.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Mode3_Measure_To_Mode6_Measure(int band, int gray)
        {
            //Copy Set5 to Set6
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Mode_6.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Mode1_Measure_To_Mode2Target(int band, int gray)
        {
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[7].Value = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[8].Value = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[9].Value = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Mode1_Measure_To_Mode2Target(int band, int gray,double Offset_X,double Offset_Y)
        {
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[7].Value = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value + Offset_X;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[8].Value = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value + Offset_Y;
            dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[9].Value = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;
        }


        public void Dual_Copy_Mode2_Measure_To_Mode3Target(int band, int gray)
        {
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[7].Value = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[8].Value = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[9].Value = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Mode2_Measure_To_Mode3Target(int band, int gray, double Offset_X, double Offset_Y)
        {
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[7].Value = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value + Offset_X;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[8].Value = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value + Offset_Y;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[9].Value = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;
        }

        public void Dual_Copy_Mode12_Ave_Measure_To_Mode3Target(int band, int gray)
        {
            double Mode1_X = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            double Mode1_Y = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            double Mode1_Lv = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;

            double Mode2_X = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            double Mode2_Y = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            double Mode2_Lv = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;

            //Copy Mode1/2 Ave to Set3
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[7].Value = (Mode1_X + Mode2_X) / 2.0;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[8].Value = (Mode1_Y + Mode2_Y) / 2.0;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[9].Value = (Mode1_Lv + Mode2_Lv) / 2.0;
        }

        public void Dual_Copy_Mode12_Ave_Measure_To_Mode3Target(int band, int gray, double Offset_X, double Offset_Y)
        {
            double Mode1_X = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            double Mode1_Y = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            double Mode1_Lv = (double)dataGridView_OC_param_Mode_1.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;

            double Mode2_X = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value;
            double Mode2_Y = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value;
            double Mode2_Lv = (double)dataGridView_OC_param_Mode_2.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value;

            //Copy Mode1/2 Ave to Set3
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[7].Value = (Mode1_X + Mode2_X) / 2.0 + Offset_X;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[8].Value = (Mode1_Y + Mode2_Y) / 2.0 + Offset_Y;
            dataGridView_OC_param_Mode_3.Rows[band * max_gray_amount + (gray + 2)].Cells[9].Value = (Mode1_Lv + Mode2_Lv) / 2.0;
        }

        public void Update_Gamma_Measure_Loopcount_ExtensionApplied_to_OC_Param_and_OC_Viewer(int gray, int band, OC_Mode Mode)
        {
            Set_OC_Param_and_OC_Viewer_Gamma(gray, band, Mode);
            Set_OC_Param_and_OC_Viewer_Measured_Values(gray, band, Mode);
            Set_OC_Param_and_OC_Viewer_LoopCount_Extension_Applied(gray, band, Mode);
        }

        public void Set_OC_Param_and_OC_Viewer_Gamma(int gray, int band, OC_Mode Mode)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Mode);
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = vars.Gamma.int_R;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = vars.Gamma.int_G;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = vars.Gamma.int_B;
        }


        public void Update_OC_Viewer_Gamma(int gray, int band, OC_Mode Mode,RGB Gamma)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Mode);
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[1].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value = Gamma.int_R;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[2].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value = Gamma.int_G;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[3].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value = Gamma.int_B;
        }



        public void Set_OC_Param_and_OC_Viewer_Measured_Values(int gray, int band, OC_Mode Mode)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Mode);
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[4].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[4].Value = vars.Measure.double_X;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[5].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[5].Value = vars.Measure.double_Y;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[6].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[6].Value = vars.Measure.double_Lv;
        }

        public void Set_OC_Param_and_OC_Viewer_LoopCount_Extension_Applied(int gray, int band, OC_Mode Mode)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Mode);
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[15].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[15].Value = vars.Extension_Applied;
            dataGridView_Band_OC_Viewer.Rows[gray + 2].Cells[16].Value = dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[16].Value = vars.loop_count;
        }


        public void Get_Band_Gray_Gamma(RGB[,] Gamma, int band, OC_Mode Mode)
        {
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);
            for (int gray = 0; gray < max_gray_amount; gray++)
            {
                Gamma[band, gray].int_R = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[1].Value.ToString());
                Gamma[band, gray].int_G = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[2].Value.ToString());
                Gamma[band, gray].int_B = Convert.ToInt16(dataGridView_OC_param.Rows[band * max_gray_amount + (gray + 2)].Cells[3].Value.ToString());
                Gamma[band, gray].String_Update_From_int();
            }
        }


        public void Copy_Previous_Band_Gamma(int band, OC_Mode Mode)
        {
            DataGridView dataGridView_Band_OC_Viewer = Get_Viewer_DataGridView(Mode);
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);

            int Prev_Offset_Row = max_gray_amount * (band - 1);
            int Offset_Row = max_gray_amount * band;
            for (int j = 1; j <= 3; j++)
            {
                for (int i = 2; i < dataGridView_Band_OC_Viewer.RowCount; i++)
                {
                    dataGridView_OC_param.Rows[i + Offset_Row].Cells[j].Value = dataGridView_OC_param.Rows[i + Prev_Offset_Row].Cells[j].Value;
                    dataGridView_Band_OC_Viewer.Rows[i].Cells[j].Value = dataGridView_OC_param.Rows[i + Offset_Row].Cells[j].Value;
                }
            }
        }


        public XYLv Get_Mode_Measured_Values(int band, int gray, OC_Mode Mode)
        {

            XYLv temp = new XYLv();
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);

            temp.double_X = Convert.ToDouble(dataGridView_OC_param.Rows[(band * max_gray_amount) + (gray + 2)].Cells[4].Value);
            temp.double_Y = Convert.ToDouble(dataGridView_OC_param.Rows[(band * max_gray_amount) + (gray + 2)].Cells[5].Value);
            temp.double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[(band * max_gray_amount) + (gray + 2)].Cells[6].Value);
            temp.String_Update_From_Double();

            return temp;
        }

        public XYLv Get_Mode_Target_Values(int band, int gray, OC_Mode Mode)
        {

            XYLv temp = new XYLv();
            DataGridView dataGridView_OC_param = Get_OC_Param_DataGridView(Mode);

            temp.double_X = Convert.ToDouble(dataGridView_OC_param.Rows[(band * max_gray_amount) + (gray + 2)].Cells[7].Value);
            temp.double_Y = Convert.ToDouble(dataGridView_OC_param.Rows[(band * max_gray_amount) + (gray + 2)].Cells[8].Value);
            temp.double_Lv = Convert.ToDouble(dataGridView_OC_param.Rows[(band * max_gray_amount) + (gray + 2)].Cells[9].Value);
            temp.String_Update_From_Double();

            return temp;
        }


        private void Set_groupBox_Band_Selection_Mode_BackColor(OC_Mode Mode,Color Backcolor)
        {
            if (Mode == OC_Mode.Mode1)
                groupBox_Band_Selection_Mode_1.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode2)
                groupBox_Band_Selection_Mode_2.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode3)
                groupBox_Band_Selection_Mode_3.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode4)
                groupBox_Band_Selection_Mode_4.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode5)
                groupBox_Band_Selection_Mode_5.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode6)
                groupBox_Band_Selection_Mode_6.BackColor = Backcolor;
            else
            {
                System.Windows.Forms.MessageBox.Show("OC Mode Should be 1,2,3,4,5 or 6");
                groupBox_Band_Selection_Mode_1.BackColor = Backcolor;
            }
        }

        private void Set_dataGridView_OC_param_Mode_XYLv_Columns_Backcolor(OC_Mode Mode, int Column_index, Color Backcolor)
        {
            if (Mode == OC_Mode.Mode1)
                dataGridView_OC_param_Mode_1.Columns[Column_index].DefaultCellStyle.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode2)
                dataGridView_OC_param_Mode_2.Columns[Column_index].DefaultCellStyle.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode3)
                dataGridView_OC_param_Mode_3.Columns[Column_index].DefaultCellStyle.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode4)
                dataGridView_OC_param_Mode_4.Columns[Column_index].DefaultCellStyle.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode5)
                dataGridView_OC_param_Mode_5.Columns[Column_index].DefaultCellStyle.BackColor = Backcolor;
            else if (Mode == OC_Mode.Mode6)
                dataGridView_OC_param_Mode_6.Columns[Column_index].DefaultCellStyle.BackColor = Backcolor;
            else
            {
                System.Windows.Forms.MessageBox.Show("OC Mode Should be 1,2,3,4,5 or 6");
                dataGridView_OC_param_Mode_1.Columns[Column_index].DefaultCellStyle.BackColor = Backcolor;
            }
        }

        public void Set_Mode_Background_Color(OC_Mode Mode,Color Backcolor)
        {
            Set_groupBox_Band_Selection_Mode_BackColor(Mode, Backcolor);
            
            for (int Column_index = 4; Column_index <= 6; Column_index++) //Measure X,Y,Lv
               Set_dataGridView_OC_param_Mode_XYLv_Columns_Backcolor(Mode,Column_index,Backcolor);

            Engineer_Mode_Mornitor_View_OC_Mode_Tema_Change(OC_Mode.Mode1);
            Engineer_Mode_Mornitor_View_OC_Mode_Tema_Change(OC_Mode.Mode2);
        }

        //Dual
        public string Dual_Get_BX_GXXX_By_Gray(int band,int gray, OC_Mode Mode)
        {
            DataGridView OC_datagridview = Get_OC_Param_DataGridView(Mode);
            return OC_datagridview.Rows[band * max_gray_amount + (gray + 2)].Cells[0].Value.ToString();
        }
        







        public void button_Read_OC_Param_From_Excel_File_Perform_Click()
        {
            button_Read_OC_Param_From_Excel_File.PerformClick();
        }


        public void OC_File_Refresh()
        {
            f1().current_model.Read_OC_Param_From_Excel_For_Dual_Mode();
            this.Text = "DP213 Dual Engineering Form";

            Band_Radiobuttion_Select(0, OC_Mode.Mode3);
            Band_Radiobuttion_Select(0, OC_Mode.Mode4);
            Band_Radiobuttion_Select(0, OC_Mode.Mode5);
            Band_Radiobuttion_Select(0, OC_Mode.Mode6);

            Band_Radiobuttion_Select(0, OC_Mode.Mode1); //Select Set1 Band as 0
            Force_Triger_Band_Mode_CheckedChanged_Function(0, OC_Mode.Mode1);

            Band_Radiobuttion_Select(0, OC_Mode.Mode2); //Select Set2 Band as 0
            Force_Triger_Band_Mode_CheckedChanged_Function(0, OC_Mode.Mode2);

            Application.DoEvents();
        }
        private void button_Read_OC_Param_From_Excel_File_Click(object sender, EventArgs e)
        {
            OC_File_Refresh();
        }

        private int Get_Mode1_Current_Band()
        {
            int band = 0;

            if (radiobutton_Band0_Mode_1.Checked)
                band = 0;
            else if (radiobutton_Band1_Mode_1.Checked)
                band = 1;
            else if (radiobutton_Band2_Mode_1.Checked)
                band = 2;
            else if (radiobutton_Band3_Mode_1.Checked)
                band = 3;
            else if (radiobutton_Band4_Mode_1.Checked)
                band = 4;
            else if (radiobutton_Band5_Mode_1.Checked)
                band = 5;
            else if (radiobutton_Band6_Mode_1.Checked)
                band = 6;
            else if (radiobutton_Band7_Mode_1.Checked)
                band = 7;
            else if (radiobutton_Band8_Mode_1.Checked)
                band = 8;
            else if (radiobutton_Band9_Mode_1.Checked)
                band = 9;
            else if (radiobutton_Band10_Mode_1.Checked)
                band = 10;
            else if (radiobutton_Band11_Mode_1.Checked)
                band = 11;
            else if (radiobutton_AOD0.Checked)
                band = 12;
            else if (radiobutton_AOD1.Checked)
                band = 13;
            else if (radiobutton_AOD2.Checked)
                band = 14;

            return band;
        }


        private int Get_Mode2_Current_Band()
        {
            int band = 0;

            if (radiobutton_Band0_Mode_2.Checked)
                band = 0;
            else if (radiobutton_Band1_Mode_2.Checked)
                band = 1;
            else if (radiobutton_Band2_Mode_2.Checked)
                band = 2;
            else if (radiobutton_Band3_Mode_2.Checked)
                band = 3;
            else if (radiobutton_Band4_Mode_2.Checked)
                band = 4;
            else if (radiobutton_Band5_Mode_2.Checked)
                band = 5;
            else if (radiobutton_Band6_Mode_2.Checked)
                band = 6;
            else if (radiobutton_Band7_Mode_2.Checked)
                band = 7;
            else if (radiobutton_Band8_Mode_2.Checked)
                band = 8;
            else if (radiobutton_Band9_Mode_2.Checked)
                band = 9;
            else if (radiobutton_Band10_Mode_2.Checked)
                band = 10;
            else if (radiobutton_Band11_Mode_2.Checked)
                band = 11;

            return band;
        }

        private int Get_Mode3_Current_Band()
        {
            int band = 0;

            if (radiobutton_Band0_Mode_3.Checked)
                band = 0;
            else if (radiobutton_Band1_Mode_3.Checked)
                band = 1;
            else if (radiobutton_Band2_Mode_3.Checked)
                band = 2;
            else if (radiobutton_Band3_Mode_3.Checked)
                band = 3;
            else if (radiobutton_Band4_Mode_3.Checked)
                band = 4;
            else if (radiobutton_Band5_Mode_3.Checked)
                band = 5;
            else if (radiobutton_Band6_Mode_3.Checked)
                band = 6;
            else if (radiobutton_Band7_Mode_3.Checked)
                band = 7;
            else if (radiobutton_Band8_Mode_3.Checked)
                band = 8;
            else if (radiobutton_Band9_Mode_3.Checked)
                band = 9;
            else if (radiobutton_Band10_Mode_3.Checked)
                band = 10;
            else if (radiobutton_Band11_Mode_3.Checked)
                band = 11;

            return band;
        }

        private int Get_Mode4_Current_Band()
        {
            int band = 0;

            if (radiobutton_Band0_Mode_4.Checked)
                band = 0;
            else if (radiobutton_Band1_Mode_4.Checked)
                band = 1;
            else if (radiobutton_Band2_Mode_4.Checked)
                band = 2;
            else if (radiobutton_Band3_Mode_4.Checked)
                band = 3;
            else if (radiobutton_Band4_Mode_4.Checked)
                band = 4;
            else if (radiobutton_Band5_Mode_4.Checked)
                band = 5;
            else if (radiobutton_Band6_Mode_4.Checked)
                band = 6;
            else if (radiobutton_Band7_Mode_4.Checked)
                band = 7;
            else if (radiobutton_Band8_Mode_4.Checked)
                band = 8;
            else if (radiobutton_Band9_Mode_4.Checked)
                band = 9;
            else if (radiobutton_Band10_Mode_4.Checked)
                band = 10;
            else if (radiobutton_Band11_Mode_4.Checked)
                band = 11;

            return band;
        }

        private int Get_Mode5_Current_Band()
        {
            int band = 0;

            if (radiobutton_Band0_Mode_5.Checked)
                band = 0;
            else if (radiobutton_Band1_Mode_5.Checked)
                band = 1;
            else if (radiobutton_Band2_Mode_5.Checked)
                band = 2;
            else if (radiobutton_Band3_Mode_5.Checked)
                band = 3;
            else if (radiobutton_Band4_Mode_5.Checked)
                band = 4;
            else if (radiobutton_Band5_Mode_5.Checked)
                band = 5;
            else if (radiobutton_Band6_Mode_5.Checked)
                band = 6;
            else if (radiobutton_Band7_Mode_5.Checked)
                band = 7;
            else if (radiobutton_Band8_Mode_5.Checked)
                band = 8;
            else if (radiobutton_Band9_Mode_5.Checked)
                band = 9;
            else if (radiobutton_Band10_Mode_5.Checked)
                band = 10;
            else if (radiobutton_Band11_Mode_5.Checked)
                band = 11;

            return band;
        }

        private int Get_Mode6_Current_Band()
        {
            int band = 0;

            if (radiobutton_Band0_Mode_6.Checked)
                band = 0;
            else if (radiobutton_Band1_Mode_6.Checked)
                band = 1;
            else if (radiobutton_Band2_Mode_6.Checked)
                band = 2;
            else if (radiobutton_Band3_Mode_6.Checked)
                band = 3;
            else if (radiobutton_Band4_Mode_6.Checked)
                band = 4;
            else if (radiobutton_Band5_Mode_6.Checked)
                band = 5;
            else if (radiobutton_Band6_Mode_6.Checked)
                band = 6;
            else if (radiobutton_Band7_Mode_6.Checked)
                band = 7;
            else if (radiobutton_Band8_Mode_6.Checked)
                band = 8;
            else if (radiobutton_Band9_Mode_6.Checked)
                band = 9;
            else if (radiobutton_Band10_Mode_6.Checked)
                band = 10;
            else if (radiobutton_Band11_Mode_6.Checked)
                band = 11;

            return band;
        }

        private int Get_Current_Band(OC_Mode Mode)
        {
            int band = 0;

            if (Mode == OC_Mode.Mode1)
                band = Get_Mode1_Current_Band();
            else if (Mode == OC_Mode.Mode2)
                band = Get_Mode2_Current_Band();
            else if (Mode == OC_Mode.Mode3)
                band = Get_Mode3_Current_Band();
            else if (Mode == OC_Mode.Mode4)
                band = Get_Mode4_Current_Band();
            else if (Mode == OC_Mode.Mode5)
                band = Get_Mode5_Current_Band();
            else if (Mode == OC_Mode.Mode6)
                band = Get_Mode6_Current_Band();
            else
                System.Windows.Forms.MessageBox.Show("OC Mode Should be 1,2,3,4,5 or 6");

            return band;
        }

        public void Set_OC_Params_For_OCMode123456(int rows_index, int columns_index, string New_value)
        {
            dataGridView_OC_param_Mode_1.Rows[rows_index].Cells[columns_index].Value = New_value;
            dataGridView_OC_param_Mode_2.Rows[rows_index].Cells[columns_index].Value = New_value;
            dataGridView_OC_param_Mode_3.Rows[rows_index].Cells[columns_index].Value = New_value;
            dataGridView_OC_param_Mode_4.Rows[rows_index].Cells[columns_index].Value = New_value;
            dataGridView_OC_param_Mode_5.Rows[rows_index].Cells[columns_index].Value = New_value;
            dataGridView_OC_param_Mode_6.Rows[rows_index].Cells[columns_index].Value = New_value;
        }

        public void Set_OC_Params_For_Selected_OCMode(OC_Mode Mode, int rows_index, int columns_index, string New_value)
        {
            DataGridView dataGridView_OC_param_Mode = Get_OC_Param_DataGridView(Mode);
            dataGridView_OC_param_Mode.Rows[rows_index].Cells[columns_index].Value = New_value;
        }

        private void button_Read_RGB_and_Show_Vdatas_Click(object sender, EventArgs e)
        {
            if (dp213_form() == null)
            {
                MessageBox.Show("Please Click OC Option first");
            }
            else
            {
                Show_DP213_Dual_Engineer_Read_Datas Show_DP213_Read_Data = new Show_DP213_Dual_Engineer_Read_Datas();
                Show_DP213_Read_Data.Read_And_Update_Gridview();
                Read_and_Update_AOD();
                Send_All_OC_Read_CMDs_and_Show();
            }
        }

        private void Read_and_Update_AOD()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(dp213_form().Get_OC_Mode_Set(OC_Mode.Mode1),DP213_Static.Max_HBM_and_Normal_Band_Amount);
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(dp213_form().Get_OC_Mode_Set(OC_Mode.Mode1), (DP213_Static.Max_HBM_and_Normal_Band_Amount + 1));
            cmds.Read_and_Update_Set_Band_AM1_AM0_Gamma(dp213_form().Get_OC_Mode_Set(OC_Mode.Mode1), (DP213_Static.Max_HBM_and_Normal_Band_Amount + 2));
        }

        private void Send_All_OC_Read_CMDs_and_Show()
        {
            bool Original_Debug_Status = f1().radioButton_Debug_Status_Mode.Checked;
            f1().radioButton_Debug_Status_Mode.Checked = true;


            Send_All_OC_Read_CMDs();

            f1().radioButton_Debug_Status_Mode.Checked = Original_Debug_Status;
        }

        private void Send_All_OC_Read_CMDs()
        {
            Send_All_ELVSS_Vinit2_Normal_Cold();
            Send_All_REF0_REF4095_Vreg1();
            Send_All_Normal_Gamma();
            Send_All_AOD_Gamma();
        }

        private void Send_All_ELVSS_Vinit2_Normal_Cold()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            f1().GB_Status_AppendText_Nextline("#Send_All_ELVSS_Vinit2_Normal_Cold", Color.Purple);
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                cmds.Send_ELVSS_CMD(Set);
                cmds.Send_Cold_ELVSS_CMD(Set);
                cmds.Send_Vinit2_CMD(Set);
                cmds.Send_Cold_Vinit2_CMD(Set);
            }
        }

        private void Send_All_REF0_REF4095_Vreg1()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            f1().GB_Status_AppendText_Nextline("#Send_All_REF0_REF4095_Vreg1", Color.Purple);
            cmds.Send_VREF0_VREF4095();
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                cmds.Send_Vreg1(Set);
            }
        }

        private void Send_All_AOD_Gamma()
        {
            

            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            f1().GB_Status_AppendText_Nextline("#Send_All_AOD_Gamma", Color.Purple);
            for (int band_index = DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index < DP213_Static.Max_Band_Amount; band_index++)
                cmds.Send_Gamma_AM1_AM0(dp213_form().Get_OC_Mode_Set(OC_Mode.Mode1), band_index);
        }

        private void Send_All_Normal_Gamma()
        {
            DP213_CMDS_Write_Read_Update_Variables cmds = DP213_CMDS_Write_Read_Update_Variables.getInstance();
            f1().GB_Status_AppendText_Nextline("#Send_All_Normal_Gamma", Color.Purple);
            for (int Set_index = 0; Set_index < DP213_Static.Max_Set_Amount; Set_index++)
            {
                Gamma_Set Set = DP213_Static.Convert_to_Gamma_Set_From_int(Set_index);
                for (int band_index = 0; band_index < DP213_Static.Max_HBM_and_Normal_Band_Amount; band_index++)
                    cmds.Send_Gamma_AM1_AM0(Set, band_index);
            }
        }




        public void Set_ProgressBar_Max_and_Initialize_Value(int max)
        {
            progressBar_Read_Data.Step = 1;
            progressBar_Read_Data.Maximum = max;
            progressBar_Read_Data.Value = 0;
        }

        public void Set_ProgressBar_Value(int value)
        {
            progressBar_Read_Data.Value = value;
        }


        private void DP213_Dual_Engineering_Mornitoring_Shown(object sender, EventArgs e)
        {

        }


        


        

    }
}
