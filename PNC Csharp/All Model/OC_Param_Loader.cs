using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Windows.Forms;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public class Common_Functions : OC_Param_Loader
    {
        //For Dual
        public double[,] Set2_Diff_Delta_L_Spec;
        public double[,] Set2_Diff_Delta_UV_Spec;

        protected Form1 f1()
        {
            return (Form1)Application.OpenForms["Form1"];
        }

        private int Single_Mode_Get_Vreg1_Diff_Columns_Length()
        {
            int Vreg1_Diff_Columns_Length;
            if (f1().current_model.Get_Current_Model_Name() == Model_Name.DP173
                || f1().current_model.Get_Current_Model_Name() == Model_Name.DP213
                || f1().current_model.Get_Current_Model_Name() == Model_Name.Elgin) 
            {
                Vreg1_Diff_Columns_Length = 16;
            }
            else if (f1().current_model.Get_Current_Model_Name() == Model_Name.Meta)
            {
                Vreg1_Diff_Columns_Length = 8;
            }
            else
            {
                Vreg1_Diff_Columns_Length = 8;
            }
            return Vreg1_Diff_Columns_Length;
        }

        private void Update_Single_Mode_dataGridView_Gamma_Vreg1_Diff()
        {
            //Gamma/Vreg1 Diff
            dataGridView_Gamma_Vreg1_Diff.Rows[0].Cells[2].Value = "Init";
            dataGridView_Gamma_Vreg1_Diff.Rows[0].Cells[6].Value = "Diff";
            dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[1].Value = "R";
            dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[2].Value = "G";
            dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[3].Value = "B";
            dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[4].Value = "Vreg1";
            dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[5].Value = "R";
            dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[6].Value = "G";
            dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[7].Value = "B";
            dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[8].Value = "Vreg1";

            if (f1().current_model.Get_Current_Model_Name() == Model_Name.DP173
                || f1().current_model.Get_Current_Model_Name() == Model_Name.DP213
                || f1().current_model.Get_Current_Model_Name() == Model_Name.Elgin)
            {
                //Gamma/Vreg1 Diff
                dataGridView_Gamma_Vreg1_Diff.Rows[0].Cells[1].Value = "Ori";
                dataGridView_Gamma_Vreg1_Diff.Rows[0].Cells[5].Value = "Ori";
                
                dataGridView_Gamma_Vreg1_Diff.Rows[0].Cells[9].Value = "Cal";
                dataGridView_Gamma_Vreg1_Diff.Rows[0].Cells[10].Value = "Init";
                dataGridView_Gamma_Vreg1_Diff.Rows[0].Cells[13].Value = "Cal";
                dataGridView_Gamma_Vreg1_Diff.Rows[0].Cells[14].Value = "Diff";

                dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[9].Value = "R";
                dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[10].Value = "G";
                dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[11].Value = "B";
                dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[12].Value = "Vreg1";
                dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[13].Value = "R";
                dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[14].Value = "G";
                dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[15].Value = "B";
                dataGridView_Gamma_Vreg1_Diff.Rows[1].Cells[16].Value = "Vreg1";
            }
            else if (f1().current_model.Get_Current_Model_Name() == Model_Name.Meta)
            {
                //RGB Vdata
                dataGridView_RGB_Vdata.Rows[0].Cells[2].Value = "Vdata";
                dataGridView_RGB_Vdata.Rows[1].Cells[1].Value = "R(v)";
                dataGridView_RGB_Vdata.Rows[1].Cells[2].Value = "G(v)";
                dataGridView_RGB_Vdata.Rows[1].Cells[3].Value = "B(v)";

            }
        }

        protected void Load_Single_OC_Param(int max_gray_amount) 
        {
            int Vreg1_Diff_Columns_Length = Single_Mode_Get_Vreg1_Diff_Columns_Length();

            OC_data = File.ReadLines(Single_OC_filePath).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]

            OC_column_length = OC_data[1].GetLength(0);
            OC_row_length = OC_data.GetLength(0);

            for (int k = 0; k < OC_column_length; k++)
            {
                if (dataGridView_OC_param.ColumnCount < OC_column_length)
                {
                    dataGridView_OC_param.Columns.Add("", "");
                    dataGridView_Band_OC_Viewer.Columns.Add("", "");
                }
                if (dataGridView_Gamma_Vreg1_Diff.ColumnCount <= Vreg1_Diff_Columns_Length)
                    dataGridView_Gamma_Vreg1_Diff.Columns.Add("", ""); //190527

                if (f1().current_model.Get_Current_Model_Name() == Model_Name.Meta)
                {
                    if (dataGridView_RGB_Vdata.ColumnCount < 4)
                        dataGridView_RGB_Vdata.Columns.Add("", ""); //191025
                }

            }
              
            for (int k = 0; k < OC_row_length; k++)
            {
                if (dataGridView_OC_param.Rows.Count < OC_row_length)
                {
                    dataGridView_OC_param.Rows.Add("");
                    dataGridView_Gamma_Vreg1_Diff.Rows.Add(""); //190527
                    if (f1().current_model.Get_Current_Model_Name() == Model_Name.Meta) dataGridView_RGB_Vdata.Rows.Add(""); 
                }
                if (k < (max_gray_amount + 2))
                {
                    if (dataGridView_Band_OC_Viewer.Rows.Count < (max_gray_amount + 2))
                        dataGridView_Band_OC_Viewer.Rows.Add("");
                }
            }     

            for (int j = 0; j < OC_column_length; j++)
            {
                for (int i = 0; i < OC_row_length; i++)
                {
                    dataGridView_OC_param.Rows[i].Cells[j].Value = OC_data[i][j];

                    if (j < 4 && i > 1) dataGridView_Gamma_Vreg1_Diff.Rows[i].Cells[j].Value = OC_data[i][j];

                    if (f1().current_model.Get_Current_Model_Name() == Model_Name.Meta)
                    {
                        if (j == 0 && i > 1) dataGridView_RGB_Vdata.Rows[i].Cells[j].Value = OC_data[i][j];
                    }

                    if (i < (max_gray_amount + 2))
                    {
                        dataGridView_Band_OC_Viewer.Rows[i].Cells[j].Value = OC_data[i][j];
                    }
                }
            }
            Update_Single_Mode_dataGridView_Gamma_Vreg1_Diff();
        }

        protected void Read_Single_OC_Param_From_Excel(int max_gray_amount)
        {
            OC_data = File.ReadLines(Single_OC_filePath).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]

            OC_column_length = OC_data[1].GetLength(0);
            OC_row_length = OC_data.GetLength(0);

            for (int k = 0; k < OC_column_length; k++)
            {
                if (dataGridView_OC_param.ColumnCount < OC_column_length)
                {
                    dataGridView_OC_param.Columns.Add("", "");
                    dataGridView_Band_OC_Viewer.Columns.Add("", "");
                }
            }

            for (int k = 0; k < OC_row_length; k++)
            {
                if (dataGridView_OC_param.Rows.Count < OC_row_length)
                {
                    dataGridView_OC_param.Rows.Add("");
                }

                if (k < (max_gray_amount + 2))
                {
                    if (dataGridView_Band_OC_Viewer.Rows.Count < (max_gray_amount + 2))
                        dataGridView_Band_OC_Viewer.Rows.Add("");
                }
            }

            for (int j = 0; j < OC_column_length; j++)
            {
                for (int i = 0; i < OC_row_length; i++)
                {
                    dataGridView_OC_param.Rows[i].Cells[j].Value = OC_data[i][j];

                    if (j < 4 && i > 1) dataGridView_Gamma_Vreg1_Diff.Rows[i].Cells[j].Value = OC_data[i][j];
                    if (i < (max_gray_amount + 2)) dataGridView_Band_OC_Viewer.Rows[i].Cells[j].Value = OC_data[i][j];
                }
            }
        }
    }



    public interface OC_Param_Load
    {
        void OC_Param_load();
        void Read_OC_Param_From_Excel_File();
        void Read_OC_Param_From_Excel_For_Dual_Mode();
    }

    public abstract class OC_Param_Loader
    {
        //Single Mode DataGridView
        protected DataGridView dataGridView_OC_param;
        protected DataGridView dataGridView_Band_OC_Viewer;
        protected DataGridView dataGridView_Gamma_Vreg1_Diff;
        protected DataGridView dataGridView_RGB_Vdata;//For Meta Model
        protected string Single_OC_filePath;

        //Dual Mode
        protected DataGridView dataGridView_OC_param_Set1;
        protected DataGridView dataGridView_OC_param_Set2;
        protected DataGridView dataGridView_OC_param_Set3;
        protected DataGridView dataGridView_OC_param_Set4;
        protected DataGridView dataGridView_OC_param_Set5;
        protected DataGridView dataGridView_OC_param_Set6;
        protected DataGridView dataGridView_Band_OC_Viewer_1;
        protected DataGridView dataGridView_Band_OC_Viewer_2;
        protected DataGridView dataGridView_Set2_DeltaL_Spec;

        //OC Pameter-related Varia1bles
        protected string[][] OC_data;
        protected int OC_column_length;
        protected int OC_row_length;

        //Dual Mode OC Parameter-Related Variables
        protected string[][] OC_data_1;
        protected int OC_column_length_1;
        protected int OC_row_length_1;
        protected string[][] OC_data_2;
        protected int OC_column_length_2;
        protected int OC_row_length_2;
        protected string[][] OC_data_3;
        protected int OC_column_length_3;
        protected int OC_row_length_3;
        protected string[][] OC_data_4;
        protected int OC_column_length_4;
        protected int OC_row_length_4;
        protected string[][] OC_data_5;
        protected int OC_column_length_5;
        protected int OC_row_length_5;
        protected string[][] OC_data_6;
        protected int OC_column_length_6;
        protected int OC_row_length_6;
        protected string[][] Set2_Delta_L_Spec;
        protected int Set2_Delta_L_Spec_column_length;
        protected int Set2_Delta_L_Spec_row_length;
    }

    class DP116_or_DP086_OC_Param : Common_Functions, OC_Param_Load
    {  
        public void OC_Param_load()
        {
            //Single Mode 
            dataGridView_OC_param = Engineer_Mornitoring_Mode.getInstance().dataGridView_OC_param;
            dataGridView_Band_OC_Viewer = Engineer_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer;
            dataGridView_Gamma_Vreg1_Diff = Engineer_Mornitoring_Mode.getInstance().dataGridView_Gamma_Vreg1_Diff;
            Single_OC_filePath = f1().current_model.Get_Single_OC_Param_Address();
            Load_Single_OC_Param(DP086_or_DP116.Max_Gray_Amount);
        }

        public void Read_OC_Param_From_Excel_File()
        {
            Read_Single_OC_Param_From_Excel(DP086_or_DP116.Max_Gray_Amount);
        }

        public void Read_OC_Param_From_Excel_For_Dual_Mode()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string filePath_1 = string.Empty;
            string filePath_2 = string.Empty;
            string filePath_Gamma_Offset = string.Empty;

            f1.Get_Dual_Mode_OC_Param_File_Path(ref filePath_1, ref filePath_2, ref filePath_Gamma_Offset);

            OC_data_1 = File.ReadLines(filePath_1).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_1 = OC_data_1[1].GetLength(0);
            OC_row_length_1 = OC_data_1.GetLength(0);

            OC_data_2 = File.ReadLines(filePath_2).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_2 = OC_data_2[1].GetLength(0);
            OC_row_length_2 = OC_data_2.GetLength(0);

            OC_data_3 = File.ReadLines(filePath_Gamma_Offset).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_3 = OC_data_3[1].GetLength(0);
            OC_row_length_3 = OC_data_3.GetLength(0);

            for (int k = 0; k < 17; k++) //#Sheet 1,2,3
            {
                if (Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_1.ColumnCount < 17)
                {
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_1.Columns.Add("", "");
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_1.Columns.Add("", "");
                }

                if (Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_2.ColumnCount < 17)
                {
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_2.Columns.Add("", "");
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_2.Columns.Add("", "");
                }

                if (Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Gamma_Offset.ColumnCount < 7)
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Gamma_Offset.Columns.Add("", "");
            }


            //for (int k = 0; k < OC_row_length; k++) #Sheet 1
            for (int k = 0; k < OC_row_length_1; k++)
            {
                if (Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_1.Rows.Count < OC_row_length_1)
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_1.Rows.Add("");
                if (k < 12)
                {
                    if (Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_1.Rows.Count < 12)
                        Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_1.Rows.Add("");
                }
            }

            //for (int k = 0; k < OC_row_length; k++) #Sheet 2
            for (int k = 0; k < OC_row_length_2; k++)
            {
                if (Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_2.Rows.Count < OC_row_length_2)
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_2.Rows.Add("");
                if (k < 12)
                {
                    if (Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_2.Rows.Count < 12)
                        Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_2.Rows.Add("");
                }
            }

            //for (int k = 0; k < OC_row_length; k++) #Sheet 3
            for (int k = 0; k < OC_row_length_3; k++)
            {
                if (Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Gamma_Offset.Rows.Count < OC_row_length_3)
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Gamma_Offset.Rows.Add("");
            }



            //for (int j = 0; j < OC_column_length; j++)
            for (int j = 0; j < 17; j++) //#Sheet 1,2
            {
                for (int i = 0; i < OC_row_length_1; i++)//#Sheet 1
                {
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_1.Rows[i].Cells[j].Value = OC_data_1[i][j];
                    if (i < 12)
                    {
                        Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_1.Rows[i].Cells[j].Value = OC_data_1[i][j];
                    }
                }

                for (int i = 0; i < OC_row_length_2; i++)//#Sheet 2
                {
                    Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_OC_param_2.Rows[i].Cells[j].Value = OC_data_2[i][j];
                    if (i < 12)
                    {
                        Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_2.Rows[i].Cells[j].Value = OC_data_2[i][j];
                    }

                }

                if (j < 7)//#Sheet 3
                {
                    for (int i = 0; i < OC_row_length_3; i++)
                    {
                        Dual_Engineer_Monitoring_Mode.getInstance().dataGridView_Gamma_Offset.Rows[i].Cells[j].Value = OC_data_3[i][j];
                    }
                }
            }
        }




       
    }

    class DP150_OC_Param : Common_Functions, OC_Param_Load
    {
        public void OC_Param_load()
        {
            //Single Mode 
            dataGridView_OC_param = DP150_Single_Engineerig_Mornitoring_Mode.getInstance().dataGridView_OC_param;
            dataGridView_Band_OC_Viewer = DP150_Single_Engineerig_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer;
            dataGridView_Gamma_Vreg1_Diff = DP150_Single_Engineerig_Mornitoring_Mode.getInstance().dataGridView_Gamma_Vreg1_Diff;
            if (Second_Model_Option_Form.getInstance().Get_IS_G2G_On()) Single_OC_filePath = f1().current_model.Get_Single_OC_G2G_Param_Address();
            else Single_OC_filePath = f1().current_model.Get_Single_OC_Param_Address();
            Load_Single_OC_Param(8);





        }


        public void Read_OC_Param_From_Excel_File()
        {
            Read_Single_OC_Param_From_Excel(8);
        }



        public void Read_OC_Param_From_Excel_For_Dual_Mode()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            //string filePath_1 = "";
            //string filePath_2 = "";
            //string OC_Gamma_Diff_Init_Form = "";

            string filePath_1 = string.Empty;
            string filePath_2 = string.Empty;
            string OC_Gamma_Diff_Init_Form = string.Empty;
            string filePath_Delta_Lv_UV = string.Empty;//Delta L,UV

            f1.Get_Dual_Mode_OC_Param_File_Path_DP150(ref filePath_1, ref filePath_2, ref OC_Gamma_Diff_Init_Form,ref filePath_Delta_Lv_UV);

            OC_data_1 = File.ReadLines(filePath_1).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_1 = OC_data_1[1].GetLength(0);
            OC_row_length_1 = OC_data_1.GetLength(0);

            OC_data_2 = File.ReadLines(filePath_2).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_2 = OC_data_2[1].GetLength(0);
            OC_row_length_2 = OC_data_2.GetLength(0);


            OC_data_3 = File.ReadLines(OC_Gamma_Diff_Init_Form).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_3 = OC_data_3[1].GetLength(0);
            OC_row_length_3 = OC_data_3.GetLength(0);

            Set2_Delta_L_Spec = File.ReadLines(filePath_Delta_Lv_UV).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            Set2_Delta_L_Spec_column_length = Set2_Delta_L_Spec[1].GetLength(0);
            Set2_Delta_L_Spec_row_length = Set2_Delta_L_Spec.GetLength(0);

            //for (int k = 0; k < 17; k++) //#Sheet 1,2,3
            for (int k = 0; k < OC_column_length_1; k++)
            {
                //if (obj_Dual_engineer_mornitoring.dataGridView_OC_param_1.ColumnCount < 17)
                if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_1.ColumnCount < OC_column_length_1) //#Sheet 1
                {
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_1.Columns.Add("", "");
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_1.Columns.Add("", "");
                }

                //if (obj_Dual_engineer_mornitoring.dataGridView_OC_param_2.ColumnCount < 17)
                if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_2.ColumnCount < OC_column_length_1) //#Sheet 2
                {
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_2.Columns.Add("", "");
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_2.Columns.Add("", "");
                }

                if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Gamma_Offset.ColumnCount < OC_column_length_3) //#Sheet 3
                    if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Gamma_Offset.ColumnCount < 4)
                        DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Gamma_Offset.Columns.Add("", "");

                if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Set2_DeltaL_Spec.ColumnCount < Set2_Delta_L_Spec_column_length) //#Sheet 4
                {
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Set2_DeltaL_Spec.Columns.Add("", "");
                }
            }


            //for (int k = 0; k < OC_row_length; k++) #Sheet 1
            for (int k = 0; k < OC_row_length_1; k++)
            {
                if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_1.Rows.Count < OC_row_length_1)
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_1.Rows.Add("");
                if (k < 10)
                {
                    if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_1.Rows.Count < 10)
                        DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_1.Rows.Add("");
                }
            }

            //for (int k = 0; k < OC_row_length; k++) #Sheet 2
            for (int k = 0; k < OC_row_length_2; k++)
            {
                if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_2.Rows.Count < OC_row_length_2)
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_2.Rows.Add("");
                if (k < 10)
                {
                    if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_2.Rows.Count < 10)
                        DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_2.Rows.Add("");
                }

                if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Set2_DeltaL_Spec.Rows.Count < Set2_Delta_L_Spec_row_length) //#Sheet 4
                {
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Set2_DeltaL_Spec.Rows.Add("");
                }
            }

            for (int k = 0; k < OC_row_length_3; k++)
            {
                if (DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Gamma_Offset.Rows.Count < OC_row_length_3)
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Gamma_Offset.Rows.Add("");
            }


            //for (int j = 0; j < 17; j++) //#Sheet 1,2
            for (int j = 0; j < OC_column_length_1; j++)
            {
                for (int i = 0; i < OC_row_length_1; i++)//#Sheet 1
                {
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_1.Rows[i].Cells[j].Value = OC_data_1[i][j];
                    if (i < 10)
                    {
                        DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_1.Rows[i].Cells[j].Value = OC_data_1[i][j];
                    }
                }

                for (int i = 0; i < OC_row_length_2; i++)//#Sheet 2
                {
                    DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_OC_param_2.Rows[i].Cells[j].Value = OC_data_2[i][j];
                    if (i < 10)
                    {
                        DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer_2.Rows[i].Cells[j].Value = OC_data_2[i][j];
                    }

                }

                if (j < 4)//#Sheet 3
                {
                    for (int i = 0; i < OC_row_length_3; i++)
                    {
                        DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Gamma_Offset.Rows[i].Cells[j].Value = OC_data_3[i][j];
                    }
                }

                if (j < Set2_Delta_L_Spec_column_length)
                {
                    for (int i = 0; i < Set2_Delta_L_Spec_row_length; i++)
                    {
                        DP150_Dual_Engineering_Mornitoring_Mode.getInstance().dataGridView_Set2_DeltaL_Spec.Rows[i].Cells[j].Value = Set2_Delta_L_Spec[i][j];
                       
                        if (j == 1)
                        {
                            if (i < Second_Model_Option_Form.getInstance().OC_Mode2_Diff_Delta_L_Spec.Length)
                            {
                                int band = i / 8;
                                int gray = i % 8;
                                Second_Model_Option_Form.getInstance().OC_Mode2_Diff_Delta_L_Spec[band, gray] = Convert.ToDouble(Set2_Delta_L_Spec[i + 2][1]);
                                Second_Model_Option_Form.getInstance().OC_Mode2_Diff_Delta_UV_Spec[band, gray] = Convert.ToDouble(Set2_Delta_L_Spec[i + 2][2]);
                            }
                        }
                    }
                }
            }
        }
    }


    class DP173_or_Elgin_OC_Param : Common_Functions, OC_Param_Load
    {
        

        
        public void OC_Param_load()
        {

            
            //Single Mode 
            dataGridView_OC_param = DP173_Single_Engineering_Mornitoring.getInstance().dataGridView_OC_param;
            dataGridView_Band_OC_Viewer = DP173_Single_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer;
            dataGridView_Gamma_Vreg1_Diff = DP173_Single_Engineering_Mornitoring.getInstance().dataGridView_Gamma_Vreg1_Diff;
            if (DP173_Model_Option_Form.getInstance().Get_IS_G2G_On()) Single_OC_filePath = f1().current_model.Get_Single_OC_G2G_Param_Address();
            else Single_OC_filePath = f1().current_model.Get_Single_OC_Param_Address();
            Load_Single_OC_Param(DP173_or_Elgin.Max_Gray_Amount);
        }
        public void Read_OC_Param_From_Excel_File()
        {
            Read_Single_OC_Param_From_Excel(DP173_or_Elgin.Max_Gray_Amount);
        }

        
        public void Read_OC_Param_From_Excel_For_Dual_Mode()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string filePath_Set1 = string.Empty;//Set1
            string filePath_Set2 = string.Empty;//Set2
            string filePath_Set3 = string.Empty;//Set3
            string filePath_Set4 = string.Empty;//Set4
            string filePath_Set5 = string.Empty;//Set5
            string filePath_Set6 = string.Empty;//Set6
            string filePath_Delta_Lv_UV = string.Empty;//Delta L,UV

            f1.Get_Dual_Mode_OC_Param_File_Path(out filePath_Set1, out filePath_Set2, out filePath_Set3, out filePath_Set4, out filePath_Set5, out filePath_Set6, out filePath_Delta_Lv_UV);

            OC_data_1 = File.ReadLines(filePath_Set1).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_1 = OC_data_1[1].GetLength(0);
            OC_row_length_1 = OC_data_1.GetLength(0);

            OC_data_2 = File.ReadLines(filePath_Set2).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_2 = OC_data_2[1].GetLength(0);
            OC_row_length_2 = OC_data_2.GetLength(0);

            OC_data_3 = File.ReadLines(filePath_Set3).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_3 = OC_data_3[1].GetLength(0);
            OC_row_length_3 = OC_data_3.GetLength(0);

            OC_data_4 = File.ReadLines(filePath_Set4).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_4 = OC_data_4[1].GetLength(0);
            OC_row_length_4 = OC_data_4.GetLength(0);

            OC_data_5 = File.ReadLines(filePath_Set5).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_5 = OC_data_5[1].GetLength(0);
            OC_row_length_5 = OC_data_5.GetLength(0);

            OC_data_6 = File.ReadLines(filePath_Set6).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_6 = OC_data_6[1].GetLength(0);
            OC_row_length_6 = OC_data_6.GetLength(0);

            Set2_Delta_L_Spec = File.ReadLines(filePath_Delta_Lv_UV).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            Set2_Delta_L_Spec_column_length = Set2_Delta_L_Spec[1].GetLength(0);
            Set2_Delta_L_Spec_row_length = Set2_Delta_L_Spec.GetLength(0);

            //for (int k = 0; k < 17; k++) //#Sheet 1,2,3
            for (int k = 0; k < OC_column_length_1; k++)
            {
                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set1.ColumnCount < OC_column_length_1) //#Sheet 1
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set1.Columns.Add("", "");
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_1.Columns.Add("", "");
                }

                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set2.ColumnCount < OC_column_length_2) //#Sheet 2
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set2.Columns.Add("", "");
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_2.Columns.Add("", "");
                }

                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set3.ColumnCount < OC_column_length_3) //#Sheet 3
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set3.Columns.Add("", "");
                }

                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set4.ColumnCount < OC_column_length_4) //#Sheet 4
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set4.Columns.Add("", "");
                }

                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set5.ColumnCount < OC_column_length_5) //#Sheet 4
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set5.Columns.Add("", "");
                }

                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set6.ColumnCount < OC_column_length_6) //#Sheet 4
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set6.Columns.Add("", "");
                }

                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.ColumnCount < Set2_Delta_L_Spec_column_length) //#Sheet 4
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.Columns.Add("", "");
                }

            }

            //#Sheet 1
            for (int k = 0; k < OC_row_length_1; k++)
            {
                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set1.Rows.Count < OC_row_length_1)
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set1.Rows.Add("");
                if (k < 10)
                {
                    if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_1.Rows.Count < 10)
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_1.Rows.Add("");
                }
            }
            //#Sheet 2
            for (int k = 0; k < OC_row_length_2; k++)
            {
                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set2.Rows.Count < OC_row_length_2)
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set2.Rows.Add("");
                if (k < 10)
                {
                    if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_2.Rows.Count < 10)
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_2.Rows.Add("");
                }

                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.Rows.Count < Set2_Delta_L_Spec_row_length) //#Sheet 4
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.Rows.Add("");
                }
            }
            //#Sheet 3
            for (int k = 0; k < OC_row_length_3; k++)
            {
                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set3.Rows.Count < OC_row_length_3)
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set3.Rows.Add("");
            }
            //#Sheet 4
            for (int k = 0; k < OC_row_length_4; k++)
            {
                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set4.Rows.Count < OC_row_length_4)
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set4.Rows.Add("");
            }

            //#Sheet 5
            for (int k = 0; k < OC_row_length_5; k++)
            {
                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set5.Rows.Count < OC_row_length_5)
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set5.Rows.Add("");
            }

            //#Sheet 6
            for (int k = 0; k < OC_row_length_6; k++)
            {
                if (DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set6.Rows.Count < OC_row_length_6)
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set6.Rows.Add("");
            }

            for (int j = 0; j < OC_column_length_1; j++)
            {
                for (int i = 0; i < OC_row_length_1; i++)//#Sheet 1
                {
                    DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set1.Rows[i].Cells[j].Value = OC_data_1[i][j];
                    if (i < 10)
                    {
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_1.Rows[i].Cells[j].Value = OC_data_1[i][j];
                    }
                }

                if (j < OC_column_length_2)
                {
                    for (int i = 0; i < OC_row_length_2; i++)//#Sheet 2
                    {
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set2.Rows[i].Cells[j].Value = OC_data_2[i][j];
                        if (i < 10)
                        {
                            DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_2.Rows[i].Cells[j].Value = OC_data_2[i][j];
                        }
                    }
                }

                if (j < OC_column_length_3)
                {
                    for (int i = 0; i < OC_row_length_3; i++)//#Sheet 3
                    {
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set3.Rows[i].Cells[j].Value = OC_data_3[i][j];
                    }
                }

                if (j < OC_column_length_4)
                {
                    for (int i = 0; i < OC_row_length_4; i++)//#Sheet 4
                    {
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set4.Rows[i].Cells[j].Value = OC_data_4[i][j];
                    }
                }

                if (j < OC_column_length_5)
                {
                    for (int i = 0; i < OC_row_length_5; i++)//#Sheet 5
                    {
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set5.Rows[i].Cells[j].Value = OC_data_5[i][j];
                    }
                }

                if (j < OC_column_length_6)
                {
                    for (int i = 0; i < OC_row_length_6; i++)//#Sheet 6
                    {
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Set6.Rows[i].Cells[j].Value = OC_data_6[i][j];
                    }
                }

                if (j < Set2_Delta_L_Spec_column_length)
                {
                    for (int i = 0; i < Set2_Delta_L_Spec_row_length; i++)
                    {
                        DP173_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.Rows[i].Cells[j].Value = Set2_Delta_L_Spec[i][j];
                        if (j == 1)
                        {
                            if (i < DP173_Model_Option_Form.getInstance().Set2_Diff_Delta_L_Spec.Length)
                            {
                                int band = i / 8;
                                int gray = i % 8;
                                DP173_Model_Option_Form.getInstance().Set2_Diff_Delta_L_Spec[band, gray] = Convert.ToDouble(Set2_Delta_L_Spec[i + 2][1]);
                                DP173_Model_Option_Form.getInstance().Set2_Diff_Delta_UV_Spec[band, gray] = Convert.ToDouble(Set2_Delta_L_Spec[i + 2][2]);
                            }
                        }
                    }
                }
            }
        }
    }

    class Meta_OC_Param : Common_Functions, OC_Param_Load
    {
        public void OC_Param_load()
        {
            //Single Mode 
            dataGridView_OC_param = Meta_Engineer_Mornitoring_Mode.getInstance().dataGridView_OC_param;
            dataGridView_Band_OC_Viewer = Meta_Engineer_Mornitoring_Mode.getInstance().dataGridView_Band_OC_Viewer;
            dataGridView_Gamma_Vreg1_Diff = Meta_Engineer_Mornitoring_Mode.getInstance().dataGridView_Gamma_Vreg1_Diff;
            if (Meta_Model_Option_Form.getInstance().Get_IS_G2G_On()) Single_OC_filePath = f1().current_model.Get_Single_OC_G2G_Param_Address();
            else Single_OC_filePath = f1().current_model.Get_Single_OC_Param_Address();
            dataGridView_RGB_Vdata = Meta_Engineer_Mornitoring_Mode.getInstance().dataGridView_RGB_Vdata;//Only For Meta
            Load_Single_OC_Param(Meta.Max_Gray_Amount);
        }

        public void Read_OC_Param_From_Excel_File()
        {
            Read_Single_OC_Param_From_Excel(Meta.Max_Gray_Amount);
        }

        public void Read_OC_Param_From_Excel_For_Dual_Mode()
        {
            throw new NotImplementedException();
        }
    }
    

    class DP213_OC_Param : Common_Functions, OC_Param_Load
    {
        public void OC_Param_load()
        {
            throw new NotImplementedException();
        }

        public void Read_OC_Param_From_Excel_File()
        {
            throw new NotImplementedException();
        }
        
        public void Read_OC_Param_From_Excel_For_Dual_Mode()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string filePath_Set1 = string.Empty;//Set1
            string filePath_Set2 = string.Empty;//Set2
            string filePath_Set3 = string.Empty;//Set3
            string filePath_Set4 = string.Empty;//Set4
            string filePath_Set5 = string.Empty;//Set5
            string filePath_Set6 = string.Empty;//Set6
            string filePath_Delta_Lv_UV = string.Empty;//Delta L,UV

            f1.Get_Dual_Mode_OC_Param_File_Path(out filePath_Set1, out filePath_Set2, out filePath_Set3, out filePath_Set4, out filePath_Set5, out filePath_Set6, out filePath_Delta_Lv_UV);

            OC_data_1 = File.ReadLines(filePath_Set1).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_1 = OC_data_1[1].GetLength(0);
            OC_row_length_1 = OC_data_1.GetLength(0);

            OC_data_2 = File.ReadLines(filePath_Set2).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_2 = OC_data_2[1].GetLength(0);
            OC_row_length_2 = OC_data_2.GetLength(0);

            OC_data_3 = File.ReadLines(filePath_Set3).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_3 = OC_data_3[1].GetLength(0);
            OC_row_length_3 = OC_data_3.GetLength(0);

            OC_data_4 = File.ReadLines(filePath_Set4).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_4 = OC_data_4[1].GetLength(0);
            OC_row_length_4 = OC_data_4.GetLength(0);

            OC_data_5 = File.ReadLines(filePath_Set5).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_5 = OC_data_5[1].GetLength(0);
            OC_row_length_5 = OC_data_5.GetLength(0);

            OC_data_6 = File.ReadLines(filePath_Set6).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            OC_column_length_6 = OC_data_6[1].GetLength(0);
            OC_row_length_6 = OC_data_6.GetLength(0);

            Set2_Delta_L_Spec = File.ReadLines(filePath_Delta_Lv_UV).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
            Set2_Delta_L_Spec_column_length = Set2_Delta_L_Spec[1].GetLength(0);
            Set2_Delta_L_Spec_row_length = Set2_Delta_L_Spec.GetLength(0);

            //for (int k = 0; k < 17; k++) //#Sheet 1,2,3
            for (int k = 0; k < OC_column_length_1; k++)
            {
                
                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_1.ColumnCount < OC_column_length_1) //#Sheet 1
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_1.Columns.Add("", "");
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_1.Columns.Add("", "");
                }

                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_2.ColumnCount < OC_column_length_2) //#Sheet 2
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_2.Columns.Add("", "");
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_2.Columns.Add("", "");
                }

                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_3.ColumnCount < OC_column_length_3) //#Sheet 3
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_3.Columns.Add("", "");
                }

                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_4.ColumnCount < OC_column_length_4) //#Sheet 4
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_4.Columns.Add("", "");
                }

                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_5.ColumnCount < OC_column_length_5) //#Sheet 4
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_5.Columns.Add("", "");
                }

                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_6.ColumnCount < OC_column_length_6) //#Sheet 4
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_6.Columns.Add("", "");
                }

                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.ColumnCount < Set2_Delta_L_Spec_column_length) //#Sheet 4
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.Columns.Add("", "");
                }

            }

            //#Sheet 1
            for (int k = 0; k < OC_row_length_1; k++)
            {
                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_1.Rows.Count < OC_row_length_1)
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_1.Rows.Add("");

                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_1.Rows.Count < (DP213_Static.Max_Gray_Amount + 2))
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_1.Rows.Add("");

            }
            //#Sheet 2
            for (int k = 0; k < OC_row_length_2; k++)
            {
                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_2.Rows.Count < OC_row_length_2)
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_2.Rows.Add("");

                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_2.Rows.Count < (DP213_Static.Max_Gray_Amount + 2))
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_2.Rows.Add("");


                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.Rows.Count < Set2_Delta_L_Spec_row_length) //#Sheet 4
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.Rows.Add("");
                }
            }

            //#Sheet 3
            for (int k = 0; k < OC_row_length_3; k++)
            {
                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_3.Rows.Count < OC_row_length_3)
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_3.Rows.Add("");
            }

            //#Sheet 4
            for (int k = 0; k < OC_row_length_4; k++)
            {
                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_4.Rows.Count < OC_row_length_4)
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_4.Rows.Add("");
            }

            //#Sheet 5
            for (int k = 0; k < OC_row_length_5; k++)
            {
                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_5.Rows.Count < OC_row_length_5)
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_5.Rows.Add("");
            }

            //#Sheet 6
            for (int k = 0; k < OC_row_length_6; k++)
            {
                if (DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_6.Rows.Count < OC_row_length_6)
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_6.Rows.Add("");
            }

            for (int j = 0; j < OC_column_length_1; j++)
            {
                for (int i = 0; i < OC_row_length_1; i++)//#Sheet 1
                {
                    DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_1.Rows[i].Cells[j].Value = OC_data_1[i][j];
                    if (i < (DP213_Static.Max_Gray_Amount + 2))
                    {
                        DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_1.Rows[i].Cells[j].Value = OC_data_1[i][j];
                    }
                }

                if (j < OC_column_length_2)
                {
                    for (int i = 0; i < OC_row_length_2; i++)//#Sheet 2
                    {
                        DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_2.Rows[i].Cells[j].Value = OC_data_2[i][j];
                        if (i < (DP213_Static.Max_Gray_Amount + 2))
                        {
                            DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Band_OC_Viewer_2.Rows[i].Cells[j].Value = OC_data_2[i][j];
                        }
                    }
                }

                if (j < OC_column_length_3)
                {
                    for (int i = 0; i < OC_row_length_3; i++)//#Sheet 3
                    {
                        DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_3.Rows[i].Cells[j].Value = OC_data_3[i][j];
                    }
                }

                if (j < OC_column_length_4)
                {
                    for (int i = 0; i < OC_row_length_4; i++)//#Sheet 4
                    {
                        DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_4.Rows[i].Cells[j].Value = OC_data_4[i][j];
                    }
                }

                if (j < OC_column_length_5)
                {
                    for (int i = 0; i < OC_row_length_5; i++)//#Sheet 5
                    {
                        DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_5.Rows[i].Cells[j].Value = OC_data_5[i][j];
                    }
                }

                if (j < OC_column_length_6)
                {
                    for (int i = 0; i < OC_row_length_6; i++)//#Sheet 6
                    {
                        DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_OC_param_Mode_6.Rows[i].Cells[j].Value = OC_data_6[i][j];
                    }
                }

                if (j < Set2_Delta_L_Spec_column_length)
                {
                    for (int i = 0; i < Set2_Delta_L_Spec_row_length; i++)
                    {
                        DP213_Dual_Engineering_Mornitoring.getInstance().dataGridView_Set2_DeltaL_Spec.Rows[i].Cells[j].Value = Set2_Delta_L_Spec[i][j];
                        if (j == 1)
                        {
                            if (i < DP213_Model_Option_Form.getInstance().OC_Mode23_Diff_Delta_L_Spec.Length)
                            {
                                int band = i / DP213_Static.Max_Gray_Amount;
                                int gray = i % DP213_Static.Max_Gray_Amount;
                                DP213_Model_Option_Form.getInstance().OC_Mode23_Diff_Delta_L_Spec[band, gray] = Convert.ToDouble(Set2_Delta_L_Spec[i + 2][1]);
                                DP213_Model_Option_Form.getInstance().OC_Mode23_Diff_Delta_UV_Spec[band, gray] = Convert.ToDouble(Set2_Delta_L_Spec[i + 2][2]);
                            }
                        }
                    }
                }
            }
        }
    }

}
