using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using References
using SectionLib;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using System.Threading.Tasks;


using System.Data.OleDb;
using System.Globalization;
using Microsoft.VisualBasic;

namespace PNC_Csharp
{
    public partial class SH_Delta_E : Form
    {

        //Form Parameter
        public bool Availability = false;

        private static SH_Delta_E Instance;
        public static SH_Delta_E getInstance()
        {
            if (Instance == null)
                Instance = new SH_Delta_E();

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



        private SH_Delta_E()
        {
            InitializeComponent();
            //Set the datagridview's EnableHeadersVisualStyles to false to get the header cell to accept the color change
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;

            //Delta E Data Grid view initialize
            dataGridView1.Columns.Add("Gray", "Gray");
            dataGridView1.Columns.Add("x", "x");
            dataGridView1.Columns.Add("y", "y");
            dataGridView1.Columns.Add("Lv", "Lv");
            dataGridView1.Columns.Add("X", "X");
            dataGridView1.Columns.Add("Y", "Y");
            dataGridView1.Columns.Add("Z", "Z");
            dataGridView1.Columns.Add("L*", "L*");
            dataGridView1.Columns.Add("f(X/Xw)", "f(X/Xw)");
            dataGridView1.Columns.Add("f(Y/Yw)", "f(Y/Yw)");
            dataGridView1.Columns.Add("f(Z/Zw)", "f(Z/Zw)");
            dataGridView1.Columns.Add("a*", "a*");
            dataGridView1.Columns.Add("b*", "b*");
            dataGridView1.Columns.Add("Delta E*", "Delta E");

            //Auto size (columns)
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //change color for X/Y/Lv Measured area  
            dataGridView1.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dataGridView1.Columns[0].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView1.Columns[0].HeaderCell.Style.BackColor = System.Drawing.Color.Gray;
            dataGridView1.Columns[0].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
            dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;

            for (int i = 1; i <= 3; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = System.Drawing.Color.Pink;
                dataGridView1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            }

            for (int i = 4; i <= 6; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                dataGridView1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = System.Drawing.Color.Green;
                dataGridView1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            }

            for (int i = 7; i <= 10; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = System.Drawing.Color.Salmon;
                dataGridView1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            }

            for (int i = 11; i <= 12; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView1.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = System.Drawing.Color.Cyan;
                dataGridView1.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            }

            dataGridView1.Columns[13].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView1.Columns[13].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView1.Columns[13].HeaderCell.Style.BackColor = System.Drawing.Color.Coral;
            dataGridView1.Columns[13].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView1.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;

        }

        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }

        private void SH_Delta_E_Load(object sender, EventArgs e)
        {
            BackColor = f1().current_model.Get_Back_Ground_Color();
        }

        private void Delta_E_calculation_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderText = "Gray";

            Delta_E_calculation_btn.Enabled = false;
            Delta_E2_calculation_btn.Enabled = false;
            IR_Drop_Delta_E_btn.Enabled = false;
            Save_log_to_Excel_btn.Enabled = false;
            button_Clear.Enabled = false;

            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];

            int gray_end_Point = Convert.ToInt16(textBox_Delta_E_End_Point.Text);

            if (gray_end_Point >= 254)
                gray_end_Point = 254;
            else if (gray_end_Point <= 0)
                gray_end_Point = 0;
            else { }

            textBox_Delta_E_End_Point.Text = gray_end_Point.ToString();

            dataGridView1.Rows.Clear();
            Availability = true;

            //measure delay time setting
            int delay_time_between_measurement = Convert.ToInt16(textBox_delay_time.Text);

            //재 계산시 대비 , 보여지는 Delta E value 초기화
            textBox_Measured_DeltaE.Text = string.Empty;

            //progressbar setting
            progressBar_GB.Maximum = 255 - gray_end_Point; //208번 측정(48~255) + 나머지 전환과정 끝나면 1 = 49
            progressBar_GB.Step = 1;
            progressBar_GB.Value = 0;

            
            //만약 Data 가 씌여져 있다면 , Data 삭제후 Delta E 계산 진행
            //만약 rows < 200 이면 , 처음 계산한다는말 , 즉 Row Add  필요
            f1.SH_Delta_E3_Measure(gray_end_Point, delay_time_between_measurement);


            if (Availability)
            {
                // x/y/Lv 계산 완료 후 -- > X Y Z 계산
                double x;
                double y;
                double Lv;

                double X;
                double Y;
                double Z;


                for (int gray = gray_end_Point; gray <= 255; gray++)
                {
                    x = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[1].Value);
                    y = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[2].Value);
                    Lv = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[3].Value);

                    X = (x / y) * Lv;
                    Y = Lv;
                    Z = ((1 - x - y) / y) * Lv;

                    dataGridView1.Rows[gray - gray_end_Point].Cells[4].Value = X;
                    dataGridView1.Rows[gray - gray_end_Point].Cells[5].Value = Y;
                    dataGridView1.Rows[gray - gray_end_Point].Cells[6].Value = Z;
                }


                // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
                double L, FX, FY, FZ;

                double X255 = 0;
                double Y255 = 0;
                double Z255 = 0;

                if (radioButton_Min_to_Max.Checked)
                {
                    X255 = Convert.ToDouble(dataGridView1.Rows[255 - gray_end_Point].Cells[4].Value);
                    Y255 = Convert.ToDouble(dataGridView1.Rows[255 - gray_end_Point].Cells[5].Value);
                    Z255 = Convert.ToDouble(dataGridView1.Rows[255 - gray_end_Point].Cells[6].Value);
                }
                else if (radioButton_Max_to_Min.Checked)
                {
                    X255 = Convert.ToDouble(dataGridView1.Rows[0].Cells[4].Value);
                    Y255 = Convert.ToDouble(dataGridView1.Rows[0].Cells[5].Value);
                    Z255 = Convert.ToDouble(dataGridView1.Rows[0].Cells[6].Value);
                }



                //a* , b* , Delta E
                double a, b, Delta_E;
                double Max_Delta_E = 0;


                for (int gray = gray_end_Point; gray <= 255; gray++)
                {
                    X = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[4].Value);
                    Y = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[5].Value);
                    Z = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[6].Value);

                    //Calculate L*
                    if (Y / Y255 > 0.008856)
                    {
                        L = 116 * Math.Pow(Y / Y255, 0.33333333) - 16;
                    }
                    else
                    {
                        L = 903.3 * (Y / Y255);
                    }

                    //Calculate F(X/Xw)
                    if (X / X255 > 0.008856)
                    {
                        FX = Math.Pow((X / X255), 0.33333333);
                    }
                    else
                    {
                        FX = 7.787 * (X / X255) + (16 / 116.0);
                    }

                    //Calculate F(Y/Yw)
                    if (Y / Y255 > 0.008856)
                    {
                        FY = Math.Pow((Y / Y255), 0.33333333);
                    }
                    else
                    {
                        FY = 7.787 * (Y / Y255) + (16 / 116.0);
                    }

                    //Calculate F(Z/Zw)
                    if (Z / Z255 > 0.008856)
                    {
                        FZ = Math.Pow((Z / Z255), 0.33333333);
                    }
                    else
                    {
                        FZ = 7.787 * (Z / Z255) + (16 / 116.0);
                    }

                    dataGridView1.Rows[gray - gray_end_Point].Cells[7].Value = L;
                    dataGridView1.Rows[gray - gray_end_Point].Cells[8].Value = FX;
                    dataGridView1.Rows[gray - gray_end_Point].Cells[9].Value = FY;
                    dataGridView1.Rows[gray - gray_end_Point].Cells[10].Value = FZ;

                    //Calculate a* , b* , Delta E
                    a = 500 * (FX - FY);
                    b = 200 * (FY - FZ);
                    Delta_E = Math.Pow((Math.Pow(a, 2) + Math.Pow(b, 2)), 0.5);

                    dataGridView1.Rows[gray - gray_end_Point].Cells[11].Value = a; //a*
                    dataGridView1.Rows[gray - gray_end_Point].Cells[12].Value = b; //b*
                    dataGridView1.Rows[gray - gray_end_Point].Cells[13].Value = Delta_E; //Delta E

                    if (Max_Delta_E <= Delta_E)
                    {
                        Max_Delta_E = Delta_E;
                    }
                }

                textBox_Measured_DeltaE.Text = Max_Delta_E.ToString();


                //Excel 에 Data 남기기 위한 자료 추가.
                dataGridView1.Rows.Add("Delta E3"); // 한열은 띄어쓰기로
                dataGridView1.Rows.Add(textBox_Measured_DeltaE.Text);

                progressBar_GB.PerformStep();
            }
            
            

            Delta_E_calculation_btn.Enabled = true;
            Delta_E2_calculation_btn.Enabled = true;
            IR_Drop_Delta_E_btn.Enabled = true;
            Save_log_to_Excel_btn.Enabled = true;
            button_Clear.Enabled = true;
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
           //Form Parameter
           Availability = false;
        }



        private void copyAlltoClipboard()
        {
            //Data 남기기위해 Header 를 첫번째 Row 에 써줌.
            dataGridView1.Rows.Insert(0, new string[] { dataGridView1.Columns[0].HeaderText, dataGridView1.Columns[1].HeaderText, dataGridView1.Columns[2].HeaderText
            ,dataGridView1.Columns[3].HeaderText,dataGridView1.Columns[4].HeaderText,dataGridView1.Columns[5].HeaderText,dataGridView1.Columns[6].HeaderText
            ,dataGridView1.Columns[7].HeaderText,dataGridView1.Columns[8].HeaderText,dataGridView1.Columns[9].HeaderText,dataGridView1.Columns[10].HeaderText
            ,dataGridView1.Columns[11].HeaderText,dataGridView1.Columns[12].HeaderText,dataGridView1.Columns[13].HeaderText});

            dataGridView1.SelectAll();

            System.Windows.Forms.DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                System.Windows.Forms.Clipboard.SetDataObject(dataObj);
        }

        private void Save_log_to_Excel_btn_Click(object sender, EventArgs e)
        {
            progressBar_GB.Maximum = 3;
            progressBar_GB.Step = 1;
            progressBar_GB.Value = 0;

            progressBar_GB.PerformStep();


            DateTime localDate = DateTime.Now;

            Microsoft.Office.Interop.Excel.Application xlexcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

            progressBar_GB.PerformStep();
            try
            {
                copyAlltoClipboard();
                object misValue = System.Reflection.Missing.Value;
                xlexcel = new Microsoft.Office.Interop.Excel.Application();
                // xlexcel.Visible = true;
                xlWorkBook = xlexcel.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

                //Getting the location and file name of the excel to save from user. 
                System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

                saveDialog.FileName = localDate.ToString(@"yyyy.MM.dd hhtt ", new CultureInfo("en-US")) + localDate.Minute.ToString() + "min " + localDate.Second.ToString() + "sec ";

                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    xlWorkBook.SaveAs(saveDialog.FileName);
                    System.Windows.Forms.MessageBox.Show("Save was successfully conducted");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                xlexcel.Quit();
                xlWorkBook = null;
                xlWorkSheet = null;
            }

            //Data 남기기위해 첫번째 Row 에 써준 header.text 를 다시 삭제 
            dataGridView1.Rows.Remove(dataGridView1.Rows[0]);

            progressBar_GB.PerformStep();
        }


        private void Calculate_IR_Drop_Delta_E(int max_index)
        {
            if (Availability)
            {

                // x/y/Lv 계산 완료 후 -- > X Y Z 계산
                double x;
                double y;
                double Lv;

                double X;
                double Y;
                double Z;


                for (int index = 0; index <= max_index & Availability; index++)
                {
                    x = Convert.ToDouble(dataGridView1.Rows[index].Cells[1].Value);
                    y = Convert.ToDouble(dataGridView1.Rows[index].Cells[2].Value);
                    Lv = Convert.ToDouble(dataGridView1.Rows[index].Cells[3].Value);

                    X = (x / y) * Lv;
                    Y = Lv;
                    Z = ((1 - x - y) / y) * Lv;

                    dataGridView1.Rows[index].Cells[4].Value = X;
                    dataGridView1.Rows[index].Cells[5].Value = Y;
                    dataGridView1.Rows[index].Cells[6].Value = Z;
                }


                // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
                double L, FX, FY, FZ;

                double X255 = Convert.ToDouble(dataGridView1.Rows[max_index].Cells[4].Value);
                double Y255 = Convert.ToDouble(dataGridView1.Rows[max_index].Cells[5].Value);
                double Z255 = Convert.ToDouble(dataGridView1.Rows[max_index].Cells[6].Value);

                //a* , b* , Delta E
                double a, b, Delta_E;
                double Average_Delta_E = 0;
                double Total_Delta_E = 0;


                for (int index = 0; index <= max_index & Availability; index++)
                {
                    X = Convert.ToDouble(dataGridView1.Rows[index].Cells[4].Value);
                    Y = Convert.ToDouble(dataGridView1.Rows[index].Cells[5].Value);
                    Z = Convert.ToDouble(dataGridView1.Rows[index].Cells[6].Value);

                    //Calculate L*
                    if (Y / Y255 > 0.008856)
                    {
                        L = 116 * Math.Pow(Y / Y255, 0.33333333) - 16;
                    }
                    else
                    {
                        L = 903.3 * (Y / Y255);
                    }

                    //Calculate F(X/Xw)
                    if (X / X255 > 0.008856)
                    {
                        FX = Math.Pow((X / X255), 0.33333333);
                    }
                    else
                    {
                        FX = 7.787 * (X / X255) + (16 / 116);
                    }

                    //Calculate F(Y/Yw)
                    if (Y / Y255 > 0.008856)
                    {
                        FY = Math.Pow((Y / Y255), 0.33333333);
                    }
                    else
                    {
                        FY = 7.787 * (Y / Y255) + (16 / 116);
                    }

                    //Calculate F(Z/Zw)
                    if (Z / Z255 > 0.008856)
                    {
                        FZ = Math.Pow((Z / Z255), 0.33333333);
                    }
                    else
                    {
                        FZ = 7.787 * (Z / Z255) + (16 / 116);
                    }

                    dataGridView1.Rows[index].Cells[7].Value = L;
                    dataGridView1.Rows[index].Cells[8].Value = FX;
                    dataGridView1.Rows[index].Cells[9].Value = FY;
                    dataGridView1.Rows[index].Cells[10].Value = FZ;

                    //Calculate a* , b* , Delta E
                    a = 500 * (FX - FY);
                    b = 200 * (FY - FZ);


                    dataGridView1.Rows[index].Cells[11].Value = a; //a*
                    dataGridView1.Rows[index].Cells[12].Value = b; //b*

                }

                for (int index = 0; index <= max_index & Availability; index += 2)
                {
                    double L1 = Convert.ToDouble(dataGridView1.Rows[index].Cells[7].Value);
                    double L2 = Convert.ToDouble(dataGridView1.Rows[index + 1].Cells[7].Value);
                    double a1 = Convert.ToDouble(dataGridView1.Rows[index].Cells[11].Value);
                    double a2 = Convert.ToDouble(dataGridView1.Rows[index + 1].Cells[11].Value);
                    double b1 = Convert.ToDouble(dataGridView1.Rows[index].Cells[12].Value);
                    double b2 = Convert.ToDouble(dataGridView1.Rows[index + 1].Cells[12].Value);

                    double delta_L = L1 - L2;
                    double delta_a = a1 - a2;
                    double delta_b = b1 - b2;

                    Delta_E = Math.Pow((Math.Pow(delta_L, 2) + Math.Pow(delta_a, 2) + Math.Pow(delta_b, 2)), 0.5);
                    Total_Delta_E += Delta_E;

                    dataGridView1.Rows[index].Cells[13].Value = Delta_E; //Delta E

                    //f1.GB_Status_AppendText_Nextline(delta_L.ToString() + " / " + delta_a.ToString() + " / " + delta_b.ToString(), Color.Black);
                    //f1.GB_Status_AppendText_Nextline(Delta_E.ToString(), Color.Black);
                }

                Average_Delta_E = Total_Delta_E / ((max_index + 1)/2);
                textBox_Measured_IR_Drop_Delta_E.Text = Average_Delta_E.ToString();

                //Excel 에 Data 남기기 위한 자료 추가.
                dataGridView1.Rows.Add("IR Drop DE"); // 한열은 띄어쓰기로
                dataGridView1.Rows.Add(textBox_Measured_IR_Drop_Delta_E.Text);

                progressBar_GB.PerformStep();
            }
        }

        private void IRC_Drop_Delta_E_Initialize()
        {
            dataGridView1.Columns[0].HeaderText = "Index";
            Delta_E_calculation_btn.Enabled = false;
            Delta_E2_calculation_btn.Enabled = false;
            IR_Drop_Delta_E_btn.Enabled = false;
            Save_log_to_Excel_btn.Enabled = false;
            button_Clear.Enabled = false;
            dataGridView1.Rows.Clear();
            Availability = true;
            progressBar_GB.Step = 1;
            progressBar_GB.Value = 0;
            textBox_Measured_DeltaE.Text = string.Empty;

            radioButton_E4_50ea_PTNs.Enabled = false;
            radioButton_E4_94ea_PTNs.Enabled = false;
        }

        private void IRC_Drop_Delta_E_Finalize()
        {
            Delta_E_calculation_btn.Enabled = true;
            Delta_E2_calculation_btn.Enabled = true;
            IR_Drop_Delta_E_btn.Enabled = true;
            Save_log_to_Excel_btn.Enabled = true;
            button_Clear.Enabled = true;

            radioButton_E4_50ea_PTNs.Enabled = true;
            radioButton_E4_94ea_PTNs.Enabled = true;
        }

        private void IRC_Drop_Delta_E_btn_Click(object sender, EventArgs e)
        {
            IRC_Drop_Delta_E_Initialize();
            int delay_time_between_measurement = Convert.ToInt16(textBox_delay_time.Text);

            if (radioButton_E4_50ea_PTNs.Checked)
                IRC_Drop_Delta_E_For_50ea_PTNs(delay_time_between_measurement);
            else if (radioButton_E4_94ea_PTNs.Checked)
                IRC_Drop_Delta_E_For_94ea_PTNs(delay_time_between_measurement);

            IRC_Drop_Delta_E_Finalize();
        }

        private void IRC_Drop_Delta_E_For_50ea_PTNs(int delay_time_between_measurement)
        {
            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
            int max_index = 49;
            progressBar_GB.Maximum = max_index;

            f1.SH_IR_Drop_Delta_E_Measure_For_50ea_PTNs(delay_time_between_measurement);
            Calculate_IR_Drop_Delta_E(max_index);

        }

        private void IRC_Drop_Delta_E_For_94ea_PTNs(int delay_time_between_measurement)
        {
            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
            int max_index = 93;
            progressBar_GB.Maximum = max_index; 

            f1.SH_IR_Drop_Delta_E_Measure_For_94ea_PTNs(delay_time_between_measurement);
            Calculate_IR_Drop_Delta_E(max_index);
        }

        private void IRC_Drop_Delta_E_Pattern(Color Inner_color,bool Full_PTN = false)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            Color Outer_color = Color.White;

            double X = 0;
            double Y = 0;

            f1.Intialize_XY(ref X, ref Y);

            string one_side = Convert.ToInt16(Math.Sqrt(X * Y * 0.3)).ToString();

            if (Full_PTN)
            {
                f1.PTN_update(Inner_color.R, Inner_color.G, Inner_color.B);

                //Just Mornitoring
                f1.GB_Status_AppendText_Nextline("Full PTN : " + Inner_color.R.ToString() + "/" + Inner_color.G.ToString()
                    + "/" + Inner_color.B.ToString(), Color.Black);
            }
            else
            {
                f1.IPC_Quick_Send("image.crosstalk " + one_side + " " + one_side
                    + " " + Outer_color.R.ToString() + " " + Outer_color.G.ToString() + " " + Outer_color.B.ToString()
                    + " " + Inner_color.R.ToString() + " " + Inner_color.G.ToString() + " " + Inner_color.B.ToString());

                //Just Mornitoring
                f1.GB_Status_AppendText_Nextline("Small PTN : " + Inner_color.R.ToString() + "/" + Inner_color.G.ToString()
                    + "/" + Inner_color.B.ToString(), Color.Black);
            }
        }

        public void IRC_Drop_Full_and_Square_94ea_PTN_List(int index)
        {
            switch (index)
            {
                case 0:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 230, 230));
                    break;
                case 1:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 230, 230), true);
                    break;

                case 2:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(209, 209, 209));
                    break;
                case 3:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(209, 209, 209), true);
                    break;

                case 4:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 186, 186));
                    break;
                case 5:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 186, 186), true);
                    break;

                case 6:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 158, 158));
                    break;
                case 7:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 158, 158), true);
                    break;

                case 8:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 0));
                    break;
                case 9:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 0), true);
                    break;

                case 10:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66));
                    break;
                case 11:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66), true);
                    break;

                case 12:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130));
                    break;
                case 13:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130), true);
                    break;

                case 14:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156));
                    break;
                case 15:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156), true);
                    break;

                case 16:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66));
                    break;
                case 17:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66), true);
                    break;

                case 18:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176));
                    break;
                case 19:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176), true);
                    break;

                case 20:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168));
                    break;
                case 21:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168), true);
                    break;

                case 22:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41));
                    break;
                case 23:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41), true);
                    break;

                case 24:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163));
                    break;
                case 25:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163), true);
                    break;

                case 26:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97));
                    break;
                case 27:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97), true);
                    break;

                case 28:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107));
                    break;
                case 29:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107), true);
                    break;

                case 30:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64));
                    break;
                case 31:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64), true);
                    break;

                case 32:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46));
                    break;
                case 33:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46), true);
                    break;

                case 34:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150));
                    break;
                case 35:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150), true);
                    break;

                case 36:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71));
                    break;
                case 37:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71), true);
                    break;

                case 38:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59));
                    break;
                case 39:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59), true);
                    break;

                case 40:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33));
                    break;
                case 41:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33), true);
                    break;

                case 42:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145));
                    break;
                case 43:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145), true);
                    break;

                case 44:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163));
                    break;
                case 45:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163), true);
                    break;

                case 46:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0));
                    break;
                case 47:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0), true);
                    break;

                case 48:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0));
                    break;
                case 49:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0), true);
                    break;

                case 50:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255));
                    break;
                case 51:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255), true);
                    break;

                case 52:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255));
                    break;
                case 53:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255), true);
                    break;

                case 54:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255));
                    break;
                case 55:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255), true);
                    break;

                case 56:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0));
                    break;
                case 57:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0), true);
                    break;

                case 58:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(112, 64, 38));
                    break;
                case 59:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(112, 64, 38), true);
                    break;

                case 60:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(204, 138, 102));
                    break;
                case 61:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(204, 138, 102), true);
                    break;

                case 62:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 199, 153));
                    break;
                case 63:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 199, 153), true);
                    break;

                case 64:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 199, 171));
                    break;
                case 65:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 199, 171), true);
                    break;

                case 66:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(247, 171, 125));
                    break;
                case 67:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(247, 171, 125), true);
                    break;

                case 68:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(199, 140, 92));
                    break;
                case 69:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(199, 140, 92), true);
                    break;

                case 70:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(143, 92, 51));
                    break;
                case 71:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(143, 92, 51), true);
                    break;

                case 72:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(207, 150, 115));
                    break;
                case 73:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(207, 150, 115), true);
                    break;

                case 74:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(161, 87, 33));
                    break;
                case 75:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(161, 87, 33), true);
                    break;

                case 76:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(214, 133, 92));
                    break;
                case 77:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(214, 133, 92), true);
                    break;

                case 78:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(209, 138, 105));
                    break;
                case 79:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(209, 138, 105), true);
                    break;

                case 80:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(250, 153, 115));
                    break;
                case 81:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(250, 153, 115), true);
                    break;

                case 82:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(199, 143, 107));
                    break;
                case 83:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(199, 143, 107), true);
                    break;

                case 84:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(201, 140, 107));
                    break;
                case 85:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(201, 140, 107), true);
                    break;

                case 86:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(204, 143, 105));
                    break;
                case 87:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(204, 143, 105), true);
                    break;

                case 88:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(122, 74, 38));
                    break;
                case 89:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(122, 74, 38), true);
                    break;

                case 90:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 140, 94));
                    break;
                case 91:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 140, 94), true);
                    break;

                case 92:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255));
                    break;
                case 93:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255), true);
                    break;
                
                default:
                    MessageBox.Show("Index is out of limit(min : 0 , max : 93)");
                    break;
            }
        }


        public void IRC_Drop_Full_and_Square_50ea_PTN_List(int index)
        {
            switch (index)
            {
                case 0:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0));
                    break;
                case 1:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0),true);
                    break;
                case 2:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0));
                    break;
                case 3:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0),true);
                    break;
                case 4:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255));
                    break;
                case 5:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255),true);
                    break;
                case 6:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0));
                    break;
                case 7:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0),true);
                    break;
                case 8:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255));
                    break;
                case 9:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255),true);
                    break;
                case 10:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255));
                    break;
                case 11:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255),true);
                    break;
                case 12:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66));
                    break;
                case 13:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66),true);
                    break;
                case 14:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130));
                    break;
                case 15:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130),true);
                    break;
                case 16:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156));
                    break;
                case 17:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156),true);
                    break;
                case 18:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66));
                    break;
                case 19:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66),true);
                    break;
                case 20:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176));
                    break;
                case 21:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176),true);
                    break;
                case 22:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168));
                    break;
                case 23:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168),true);
                    break;
                case 24:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41));
                    break;
                case 25:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41),true);
                    break;
                case 26:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163));
                    break;
                case 27:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163),true);
                    break;
                case 28:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97));
                    break;
                case 29:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97),true);
                    break;
                case 30:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107));
                    break;
                case 31:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107),true);
                    break;
                case 32:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64));
                    break;
                case 33:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64),true);
                    break;
                case 34:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46));
                    break;
                case 35:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46),true);
                    break;
                case 36:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150));
                    break;
                case 37:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150),true);
                    break;
                case 38:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71));
                    break;
                case 39:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71),true);
                    break;
                case 40:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59));
                    break;
                case 41:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59),true);
                    break;
                case 42:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33));
                    break;
                case 43:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33),true);
                    break;
                case 44:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145));
                    break;
                case 45:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145),true);
                    break;
                case 46:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163));
                    break;
                case 47:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163),true);
                    break;
                case 48:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255));
                    break;
                case 49:
                    IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255),true);
                    break;
                default:
                    MessageBox.Show("Index is out of limit(min : 0 , max : 49)");
                    break;
            }
        }


        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void Delta_E2_calculation_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderText = "DBV";

            Delta_E_calculation_btn.Enabled = false;
            Delta_E2_calculation_btn.Enabled = false;
            IR_Drop_Delta_E_btn.Enabled = false;
            Save_log_to_Excel_btn.Enabled = false;
            button_Clear.Enabled = false;

            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];

            int Step_Value = 0;

            if (step_value_1.Checked)
                Step_Value = 1;
            else if (step_value_4.Checked)
                Step_Value = 4;
            else if (step_value_8.Checked)
                Step_Value = 8;
            else if (step_value_16.Checked)
                Step_Value = 16;
            else
            {
                Step_Value = 0;
                System.Windows.Forms.MessageBox.Show("It's impossible");
            }

            int dbv_end_Point = Convert.ToInt16(textBox_Delta_E2_End_Point.Text);

            if (dbv_end_Point >= f1.Get_DBV_TrackBar_Maximum() - 1)
                dbv_end_Point = f1.Get_DBV_TrackBar_Maximum() - 1;
            else if (dbv_end_Point <= 0)
                dbv_end_Point = 0;
            else { }

            textBox_Delta_E2_End_Point.Text = dbv_end_Point.ToString();

            dataGridView1.Rows.Clear();
            Availability = true;

            //measure delay time setting
            int delay_time_between_measurement = Convert.ToInt16(textBox_delay_time.Text);

            //재 계산시 대비 , 보여지는 Delta E value 초기화
            textBox_Measured_DeltaE.Text = string.Empty;

            //progressbar setting
            progressBar_GB.Maximum = (f1.Get_DBV_TrackBar_Maximum() - dbv_end_Point) / Step_Value; //208번 측정(48~255) + 나머지 전환과정 끝나면 1 = 49
            progressBar_GB.Step = 1;
            progressBar_GB.Value = 0;


            //만약 Data 가 씌여져 있다면 , Data 삭제후 Delta E 계산 진행
            //만약 rows < 200 이면 , 처음 계산한다는말 , 즉 Row Add  필요
            //f1.SH_Delta_E3_Measure(gray_end_point, delay_time_between_measurement);
            f1.SH_Delta_E2_Measure(dbv_end_Point, delay_time_between_measurement);


            if (Availability)
            {
                // x/y/Lv 계산 완료 후 -- > X Y Z 계산
                double x;
                double y;
                double Lv;

                double X;
                double Y;
                double Z;

                int count = 0;
                for (int i = dbv_end_Point; i < (f1.Get_DBV_TrackBar_Maximum() + Step_Value); )
                {
                    i += Step_Value;

                    if (i - Step_Value > f1.Get_DBV_TrackBar_Maximum())
                        break;

                    x = Convert.ToDouble(dataGridView1.Rows[count].Cells[1].Value);
                    y = Convert.ToDouble(dataGridView1.Rows[count].Cells[2].Value);
                    Lv = Convert.ToDouble(dataGridView1.Rows[count].Cells[3].Value);

                    X = (x / y) * Lv;
                    Y = Lv;
                    Z = ((1 - x - y) / y) * Lv;

                    dataGridView1.Rows[count].Cells[4].Value = X;
                    dataGridView1.Rows[count].Cells[5].Value = Y;
                    dataGridView1.Rows[count].Cells[6].Value = Z;

                    count++;
                }


                // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
                double L, FX, FY, FZ;
                double X4095 = 0;
                double Y4095 = 0;
                double Z4095 = 0;


                if (radioButton_Min_to_Max.Checked)
                {
                    X4095 = Convert.ToDouble(dataGridView1.Rows[count - 1].Cells[4].Value);
                    Y4095 = Convert.ToDouble(dataGridView1.Rows[count - 1].Cells[5].Value);
                    Z4095 = Convert.ToDouble(dataGridView1.Rows[count - 1].Cells[6].Value);
                }
                else if (radioButton_Max_to_Min.Checked)
                {
                    X4095 = Convert.ToDouble(dataGridView1.Rows[0].Cells[4].Value);
                    Y4095 = Convert.ToDouble(dataGridView1.Rows[0].Cells[5].Value);
                    Z4095 = Convert.ToDouble(dataGridView1.Rows[0].Cells[6].Value);
                }
                else { }

                //a* , b* , Delta E
                double a, b, Delta_E;
                double Max_Delta_E = 0;

                count = 0;
                for (int i = dbv_end_Point; i < (f1.Get_DBV_TrackBar_Maximum() + Step_Value); )
                {
                    i += Step_Value;

                    if (i - Step_Value > f1.Get_DBV_TrackBar_Maximum())
                        break;

                    X = Convert.ToDouble(dataGridView1.Rows[count].Cells[4].Value);
                    Y = Convert.ToDouble(dataGridView1.Rows[count].Cells[5].Value);
                    Z = Convert.ToDouble(dataGridView1.Rows[count].Cells[6].Value);

                    //Calculate L*
                    if (Y / Y4095 > 0.008856)
                    {
                        L = 116 * Math.Pow(Y / Y4095, 0.33333333) - 16;
                    }
                    else
                    {
                        L = 903.3 * (Y / Y4095);
                    }

                    //Calculate F(X/Xw)
                    if (X / X4095 > 0.008856)
                    {
                        FX = Math.Pow((X / X4095), 0.33333333);
                    }
                    else
                    {
                        FX = 7.787 * (X / X4095) + (16 / 116.0);
                    }

                    //Calculate F(Y/Yw)
                    if (Y / Y4095 > 0.008856)
                    {
                        FY = Math.Pow((Y / Y4095), 0.33333333);
                    }
                    else
                    {
                        FY = 7.787 * (Y / Y4095) + (16 / 116.0);
                    }

                    //Calculate F(Z/Zw)
                    if (Z / Z4095 > 0.008856)
                    {
                        FZ = Math.Pow((Z / Z4095), 0.33333333);
                    }
                    else
                    {
                        FZ = 7.787 * (Z / Z4095) + (16 / 116.0);
                    }

                    dataGridView1.Rows[count].Cells[7].Value = L;
                    dataGridView1.Rows[count].Cells[8].Value = FX;
                    dataGridView1.Rows[count].Cells[9].Value = FY;
                    dataGridView1.Rows[count].Cells[10].Value = FZ;

                    //Calculate a* , b* , Delta E
                    a = 500 * (FX - FY);
                    b = 200 * (FY - FZ);
                    Delta_E = Math.Pow((Math.Pow(a, 2) + Math.Pow(b, 2)), 0.5);

                    dataGridView1.Rows[count].Cells[11].Value = a; //a*
                    dataGridView1.Rows[count].Cells[12].Value = b; //b*
                    dataGridView1.Rows[count].Cells[13].Value = Delta_E; //Delta E

                    if (Max_Delta_E <= Delta_E)
                    {
                        Max_Delta_E = Delta_E;
                    }

                    count++;
                }

                textBox_Measured_DeltaE2.Text = Max_Delta_E.ToString();

                //Excel 에 Data 남기기 위한 자료 추가.
                dataGridView1.Rows.Add("Delta E2"); // 한열은 띄어쓰기로
                dataGridView1.Rows.Add(textBox_Measured_DeltaE2.Text);

                progressBar_GB.PerformStep();
            }

            Delta_E_calculation_btn.Enabled = true;
            Delta_E2_calculation_btn.Enabled = true;
            IR_Drop_Delta_E_btn.Enabled = true;
            Save_log_to_Excel_btn.Enabled = true;
            button_Clear.Enabled = true;
        }

        private void radioButton_E4_94ea_PTNs_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_E4_50ea_PTNs_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
