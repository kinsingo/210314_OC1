using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using References
using SectionLib;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using BSQH_Csharp_Library;

namespace PNC_Csharp
{
    public partial class DBV_Accuracy : Form
    {
        bool GCS_BCS_Stop = false;
        Point Previouse_Pos;

        private static DBV_Accuracy Instance;
        public static DBV_Accuracy getInstance()
        {
            if (Instance == null)
                Instance = new DBV_Accuracy();

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

        private DBV_Accuracy()
        {
            InitializeComponent();
        }

        private Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }


        private void DBV_Accuracy_Load(object sender, EventArgs e)
        {
            BackColor = f1().current_model.Get_Back_Ground_Color();
            DBV_trackbar.Maximum = f1().current_model.get_DBV_Max();
            textBox_BCS_Max.Text = f1().current_model.get_DBV_Max().ToString("X3");
            textBox_X_Max.Text = f1().current_model.get_DBV_Max().ToString();

            //Measure X,Y.Lv
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.Columns.Add("Gray", "Gray");
            dataGridView2.Columns.Add("x", "x");
            dataGridView2.Columns.Add("y", "y");
            dataGridView2.Columns.Add("Lv", "Lv");
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
            dataGridView2.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;


            //-----Chart Related-----
            comboBox_Chart_Type.SelectedItem = "Point";
            if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
            chart1.Series["Channel1"].ChartType = SeriesChartType.Point;

            //Inialize Chart previous position as (x,y) = (0,0)
            Previouse_Pos = new Point(0, 0);

            //Set AxisX
            chart1.ChartAreas[0].AxisX.Title = "TBD(DBV or Gray)";
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;

            //Set AxisY
            chart1.ChartAreas[0].AxisY.Title = "Lv Difference(%)";
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            //Set Chart Background Color and Gradient Style
            chart1.BackGradientStyle = GradientStyle.HorizontalCenter;

            //Clear Series Default Datas
            chart1.Series.Clear();
            //-----------------------
        }


        private void CA_Measure_button_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.CA_connection_button_Perform_Click();// Measure 시에 Remote off 일수도 있으니 on 으로 구동 

            //Auto size (columns)
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView2.Columns[0].HeaderText = "-";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

            try
            {
                f1.isMsr = true;

                CA_Measure_button.Enabled = false;

                f1.objCa.Measure();

                label6.Text = "x";
                label7.Text = "y";
                label8.Text = "Lv";
                //X_Value_display.Text = objProbe.sx.ToString("0.0000");
                //Y_Value_display.Text = objProbe.sy.ToString("0.0000");
                //Lv_Value_display.Text = objProbe.Lv.ToString("0.0000");
                X_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                Y_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                Lv_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");

                System.Windows.Forms.Application.DoEvents();

                CA_Measure_button.Enabled = true;
                
                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add("-", X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            catch (Exception er)
            {
                f1.DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        public void Set_Chart1_BackColor(Color color)
        {
            chart1.BackColor = color;
        }

        private void Grid_Clear_button_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null; // reset (unbind the datasource)
            dataGridView2.Rows.Clear();
        }

        private void button_GCS_BCS_Stop_Click(object sender, EventArgs e)
        {
            GCS_BCS_Stop = true;
        }

        private void button_GCS_Measure_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            GCS_BCS_Stop = false;

            f1.GB_Status_AppendText_Nextline("Gray GCS Measuring Started", System.Drawing.Color.Black);
            Grid_Clear_button_Click(sender, e);

            int GCS_Max = Convert.ToUInt16(textBox_GCS_Max.Text);
            int GCS_Min = Convert.ToUInt16(textBox_GCS_Min.Text);

            if (GCS_Max > 255 || GCS_Max < 1)
            {
                GCS_Max = 255;
                textBox_GCS_Max.Text = "255";
            }

            if (GCS_Min < 0 || GCS_Min > 254)
            {
                GCS_Min = 0;
                textBox_GCS_Min.Text = "0";
            }

            if (GCS_Min >= GCS_Max)
            {
                GCS_Max = 255;
                textBox_GCS_Max.Text = "255";
                GCS_Min = 0;
                textBox_GCS_Min.Text = "0";
            }

            //----chart inializing for GCS measurement---
            chart1.Series.Clear();
            if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
            chart1.Series["Channel1"].Points.Clear();
            chart1.Series["Channel1"].ChartType = SeriesChartType.Point;
            chart1.Series["Channel1"].Color = Color.Blue;
            chart1.ChartAreas[0].AxisX.Title = "Gray";
            chart1.ChartAreas[0].AxisX.Maximum = GCS_Max;
            chart1.ChartAreas[0].AxisX.Minimum = GCS_Min;
            if (radioButton_Chart_Point_Marker_Size_1.Checked) chart1.Series["Channel1"].MarkerSize = 1;
            else if (radioButton_Chart_Point_Marker_Size_2.Checked) chart1.Series["Channel1"].MarkerSize = 2;
            else if (radioButton_Chart_Point_Marker_Size_4.Checked) chart1.Series["Channel1"].MarkerSize = 4;
            else if (radioButton_Chart_Point_Marker_Size_8.Checked) chart1.Series["Channel1"].MarkerSize = 8;
            button_Chart_Type_Change.PerformClick();
            if (radioButton_Y_Min_Max_Manual_Setting.Checked) radioButton_Y_Min_Max_Manual_Setting_CheckedChanged(sender, e);
            else if (radioButton_Y_Min_Max_Auto_Setting.Checked)radioButton_Y_Min_Max_Auto_Setting_CheckedChanged(sender, e);
            //-------------------------------------------


            progressBar_GB.Maximum = GCS_Max - GCS_Min;
            progressBar_GB.Value = GCS_Min - GCS_Min;

            dataGridView2.Columns[0].HeaderText = "Gray";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";

            for (int j = 4; j < dataGridView2.ColumnCount; j++)
                dataGridView2.Columns[j].HeaderText = string.Empty;

            int miliseconds = Convert.ToInt32(GCS_BCS_Delay_Textbox.Text);
            int gray;

            int Step_Value;

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

            if (max_to_min_rb.Checked)
            {
                int count = 0;
                for (int i = GCS_Max; i > GCS_Min - Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i - Step_Value;
                    gray = i + Step_Value;

                    if (gray > GCS_Max)
                        gray = GCS_Max;
                    else if (gray < GCS_Min)
                        gray = GCS_Min;

                    progressBar_GB.Value = GCS_Max - gray;

                    f1.Set_GCS(gray, gray, gray);
                    Thread.Sleep(miliseconds);


                    double Prev_Lv = Convert.ToDouble(Lv_Value_display.Text);

                    if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, gray.ToString());   
                    else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked)Average_Measure_5_Min_Max_Lv_Delete(gray.ToString());                   
                    else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked)Get_Max_Measure(2,gray.ToString());                   
                    else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, gray.ToString());

                    if (count != 0)//Skip the first step
                    {
                        double After_Lv = Convert.ToDouble(Lv_Value_display.Text);
                        //double Diff_Lv = After_Lv - Prev_Lv; (min to max Measure Mode)
                        double Diff_Lv = Prev_Lv - After_Lv; //(max to min Measure Mode)
                        double Diff_Lv_Per = ((Prev_Lv - After_Lv) / After_Lv)*100;
                        //f1.GB_Status_AppendText_Nextline("Diff = After_Lv - Prev_Lv : " + Diff_Lv.ToString() + " = " + After_Lv.ToString() + " - " + prev_Lv.ToString(), Color.Blue);
                        f1.GB_Status_AppendText_Nextline("Diff = Prev_Lv - After_Lv : " + Diff_Lv.ToString() + " = " + Prev_Lv.ToString() + " - " + After_Lv.ToString(), Color.Blue);
                        f1.GB_Status_AppendText_Nextline("Diff(%) = ((Prev_Lv - After_Lv)/After_Lv)*100 : " + Diff_Lv_Per.ToString() + "(%)", Color.Blue);
                        chart1.Series["Channel1"].Points.AddXY(gray, Diff_Lv_Per);
                    }
                    count++;
                }
            }
            else if (min_to_max_rb.Checked)
            {
                int count = 0;
                for (int i = GCS_Min; i < GCS_Max + Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i + Step_Value;
                    gray = i - Step_Value;


                    if (gray > GCS_Max)
                        gray = GCS_Max;
                    else if (gray < GCS_Min)
                        gray = GCS_Min;

                    progressBar_GB.Value = gray - GCS_Min;

                    f1.Set_GCS(gray, gray, gray);
                    Thread.Sleep(miliseconds);

                    double Prev_Lv = Convert.ToDouble(Lv_Value_display.Text);

                    if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked) Average_Measure_5_Min_Max_Lv_Delete(gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked) Get_Max_Measure(2, gray.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, gray.ToString());

                    if (count != 0)//Skip the first step
                    {
                        double After_Lv = Convert.ToDouble(Lv_Value_display.Text);
                        double Diff_Lv = After_Lv - Prev_Lv; //(min to max Measure Mode)
                        double Diff_Lv_Per = ((After_Lv - Prev_Lv) / Prev_Lv) * 100;
                        //double Diff_Lv = Prev_Lv - After_Lv; //(max to min Measure Mode)
                        f1.GB_Status_AppendText_Nextline("Diff = After_Lv - Prev_Lv : " + Diff_Lv.ToString() + " = " + After_Lv.ToString() + " - " + Prev_Lv.ToString(), Color.Blue);
                        f1.GB_Status_AppendText_Nextline("Diff(%) = ((After_Lv - Prev_Lv)/Prev_Lv)*100 : " + Diff_Lv_Per.ToString() + "(%)", Color.Blue);
                        //f1.GB_Status_AppendText_Nextline("Diff = Prev_Lv - After_Lv : " + Diff_Lv.ToString() + " = " + Prev_Lv.ToString() + " - " + After_Lv.ToString(), Color.Blue);
                        chart1.Series["Channel1"].Points.AddXY(gray, Diff_Lv_Per);
                    }
                    count++;
                }
            }
            f1.GB_Status_AppendText_Nextline("Gray GCS Measuring Finished", System.Drawing.Color.Black);
        }




        private void Average_Measure_5_Min_Max_Lv_Delete(string DBV_Or_Gray_String = "-")
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
            XYLv[] Measure = new XYLv[5];
            try
            {
                f1.isMsr = true;
                CA_Measure_button.Enabled = false;

                int Max_Index = 0; double Max_Value = 0;
                int Min_Index = 0; double Min_Value = 2000;

                for (int a = 0; a < 5; a++)
                {
                    f1.objCa.Measure();
                    Measure[a].X = f1.objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Measure[a].Y = f1.objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Measure[a].Lv = f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                    Measure[a].Double_Update_From_String();
                    f1.GB_Status_AppendText_Nextline(a.ToString() + ")Measured X/Y/Lv = " + Measure[a].X + "/" + Measure[a].Y + "/" + Measure[a].Lv, Color.Green);

                    if (Measure[a].double_Lv < Min_Value)
                    {
                        Min_Value = Measure[a].double_Lv;
                        Min_Index = a;
                    }

                    if (Measure[a].double_Lv > Max_Value)
                    {
                        Max_Value = Measure[a].double_Lv;
                        Max_Index = a;
                    }

                }

                int count = 0;
                XYLv Sum_Measure = new XYLv();
                XYLv Ave_Measure = new XYLv();
                Sum_Measure.Set_Value(0, 0, 0);
                Ave_Measure.Set_Value(0, 0, 0);

                for (int a = 0; a < 5; a++)
                {
                    if (a == Max_Index || a == Min_Index)
                    {
                    }
                    else
                    {
                        Sum_Measure.double_X += Measure[a].double_X;
                        Sum_Measure.double_Y += Measure[a].double_Y;
                        Sum_Measure.double_Lv += Measure[a].double_Lv;
                        count++;
                    }
                }

                Ave_Measure.double_X = Math.Round((Sum_Measure.double_X / count), 4);
                Ave_Measure.double_Y = Math.Round((Sum_Measure.double_Y / count), 4);
                Ave_Measure.double_Lv = Math.Round((Sum_Measure.double_Lv / count), 4);
                Ave_Measure.String_Update_From_Double();

                f1.GB_Status_AppendText_Nextline("Max_Lv/Min_Lv Index = " + Max_Index.ToString() + "/" + Min_Index.ToString(), Color.Black);
                f1.GB_Status_AppendText_Nextline("Min Lv/Ave Lv/Max Lv = " + Measure[Min_Index].double_Lv.ToString() + "/" + Ave_Measure.Lv + "/" + Measure[Max_Index].double_Lv.ToString(), Color.Black);
                f1.GB_Status_AppendText_Nextline("count = " + count.ToString() + ",Ave_X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Blue);

                X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');

                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add(DBV_Or_Gray_String, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;

                System.Windows.Forms.Application.DoEvents();
                CA_Measure_button.Enabled = true;
            }
            catch (Exception er)
            {
                f1.DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void Average_Measure(int Measure_times = 1,string GCS_Or_BCS_String = "-")
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            XYLv[] Measure = new XYLv[Measure_times];
            try
            {
                f1.isMsr = true;
                CA_Measure_button.Enabled = false;
                if (Measure_times > 1)
                {
                    XYLv Sum_Measure = new XYLv();
                    XYLv Ave_Measure = new XYLv();
                    Sum_Measure.Set_Value(0, 0, 0);
                    Ave_Measure.Set_Value(0, 0, 0);

                    for (int a = 0; a < Measure_times; a++)
                    {
                        f1.objCa.Measure();
                        Measure[a].X = f1.objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Measure[a].Y = f1.objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Measure[a].Lv = f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                        Measure[a].Double_Update_From_String();
                        Sum_Measure.double_X += Measure[a].double_X;
                        Sum_Measure.double_Y += Measure[a].double_Y;
                        Sum_Measure.double_Lv += Measure[a].double_Lv;
                        f1.GB_Status_AppendText_Nextline(a.ToString() + ")Measured X/Y/Lv = " + Measure[a].X + "/" + Measure[a].Y + "/" + Measure[a].Lv, Color.Green);
                    }

                    Ave_Measure.double_X = Math.Round((Sum_Measure.double_X / Measure_times), 4);
                    Ave_Measure.double_Y = Math.Round((Sum_Measure.double_Y / Measure_times), 4);
                    Ave_Measure.double_Lv = Math.Round((Sum_Measure.double_Lv / Measure_times), 4);
                    Ave_Measure.String_Update_From_Double();


                    f1.GB_Status_AppendText_Nextline("Ave X/Y/Lv = " + Ave_Measure.X + "/" + Ave_Measure.Y + "/" + Ave_Measure.Lv, Color.Blue);

                    X_Value_display.Text = Ave_Measure.X.PadRight(6, '0');
                    Y_Value_display.Text = Ave_Measure.Y.PadRight(6, '0');
                    Lv_Value_display.Text = Ave_Measure.Lv.PadRight(6, '0');
                }
                else if (Measure_times == 1)
                {
                    f1.objCa.Measure();
                    X_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                }
                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add(GCS_Or_BCS_String, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;

                System.Windows.Forms.Application.DoEvents();
                CA_Measure_button.Enabled = true;
            }
            catch (Exception er)
            {
                f1.DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void button_BCS_Measure_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int BCS_Max = Convert.ToInt32(textBox_BCS_Max.Text, 16);
            int BCS_Min = Convert.ToInt32(textBox_BCS_Min.Text, 16);

            if (BCS_Max > 4095 || BCS_Max < 1)
            {
                textBox_BCS_Max.Text = "FFF";
                BCS_Max = 4095;
            }

            if (BCS_Min < 0 || BCS_Min > 4094)
            {
                textBox_BCS_Min.Text = "0";
                BCS_Min = 0;
            }

            if (BCS_Min >= BCS_Max)
            {
                textBox_BCS_Max.Text = "FFF";
                BCS_Max = 4095;
                textBox_BCS_Min.Text = "0";
                BCS_Min = 0;
            }

            //----chart inializing for BCS measurement---
            chart1.Series.Clear();
            if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
            chart1.Series["Channel1"].Points.Clear();
            chart1.Series["Channel1"].ChartType = SeriesChartType.Point;
            chart1.Series["Channel1"].Color = Color.DarkGreen;
            chart1.ChartAreas[0].AxisX.Title = "DBV";
            chart1.ChartAreas[0].AxisX.Maximum = BCS_Max;
            chart1.ChartAreas[0].AxisX.Minimum = BCS_Min;
            if (radioButton_Chart_Point_Marker_Size_1.Checked)chart1.Series["Channel1"].MarkerSize = 1;
            else if (radioButton_Chart_Point_Marker_Size_2.Checked) chart1.Series["Channel1"].MarkerSize = 2;
            else if (radioButton_Chart_Point_Marker_Size_4.Checked) chart1.Series["Channel1"].MarkerSize = 4;
            else if (radioButton_Chart_Point_Marker_Size_8.Checked) chart1.Series["Channel1"].MarkerSize = 8;
            button_Chart_Type_Change.PerformClick();
            //X Range
            if (radioButton_X_Min_Max_Manual_Setting.Checked) radioButton_X_Min_Max_Manual_Setting_CheckedChanged(sender, e);
            else if (radioButton_X_Min_Max_Auto_Setting.Checked) radioButton_X_Min_Max_Auto_Setting_CheckedChanged(sender, e);
            //Y Range
            if (radioButton_Y_Min_Max_Manual_Setting.Checked) radioButton_Y_Min_Max_Manual_Setting_CheckedChanged(sender, e);
            else if (radioButton_Y_Min_Max_Auto_Setting.Checked) radioButton_Y_Min_Max_Auto_Setting_CheckedChanged(sender, e);
            //-------------------------------------------


            //----DBV Accuracy related----
            double DBV_Accuracy_Lv_Threshold = Convert.ToDouble(textBox_DBV_Accuracy_Lv_Threshold.Text);

            double DBV_Accuracy_Lv_Diff_Low_Limit = Convert.ToDouble(textBox_DBV_Accuracy_Lv_Diff_Low_Limit.Text);
            double Low_Max_Lv_Diff_Per = 0;
            double Low_Max_Lv_Diff_Per_Correspond_DBV = 0;

            double DBV_Accuracy_Lv_Diff_High_Limit = Convert.ToDouble(textBox_DBV_Accuracy_Lv_Diff_High_Limit.Text);
            double High_Max_Lv_Diff_Per = 0;
            double High_Max_Lv_Diff_Per_Correspond_DBV = 0;

            StripLine stripline = new StripLine();
            stripline.Interval = 0;
            stripline.StripWidth = 0.02;
            stripline.BackColor = Color.DarkBlue;
            stripline.IntervalOffset = DBV_Accuracy_Lv_Diff_Low_Limit;
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline);

            StripLine stripline2 = new StripLine();
            stripline2.Interval = 0;
            stripline2.StripWidth = 0.02;
            stripline2.BackColor = Color.Blue;
            stripline2.IntervalOffset = DBV_Accuracy_Lv_Diff_High_Limit;
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline2);

            StripLine stripline3 = new StripLine();
            stripline3.Interval = 0;
            stripline3.StripWidth = 0.02;
            stripline3.BackColor = Color.DarkBlue;
            stripline3.IntervalOffset = (-DBV_Accuracy_Lv_Diff_Low_Limit);
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline3);

            StripLine stripline4 = new StripLine();
            stripline4.Interval = 0;
            stripline4.StripWidth = 0.02;
            stripline4.BackColor = Color.Blue;
            stripline4.IntervalOffset = (-DBV_Accuracy_Lv_Diff_High_Limit);
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline4);
            //----------------------------

            GCS_BCS_Stop = false;

            f1.GB_Status_AppendText_Nextline("BCS Measuring Started", System.Drawing.Color.DarkGray);
            Grid_Clear_button_Click(sender, e);

            this.progressBar_GB.Maximum = BCS_Max - BCS_Min;
            progressBar_GB.Value = BCS_Min - BCS_Min;

            dataGridView2.Columns[0].HeaderText = "DBV";
            dataGridView2.Columns[1].HeaderText = "X";
            dataGridView2.Columns[2].HeaderText = "Y";
            dataGridView2.Columns[3].HeaderText = "Lv";
            for (int j = 4; j < dataGridView2.ColumnCount; j++)
                dataGridView2.Columns[j].HeaderText = string.Empty;

            int miliseconds = Convert.ToInt32(GCS_BCS_Delay_Textbox.Text);
            int dbv;
            int Step_Value;

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
            if (max_to_min_rb.Checked)
            {
                int count = 0;
                for (int i = BCS_Max; i >= BCS_Min - Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i - Step_Value;
                    dbv = i + Step_Value;

                    if (dbv > BCS_Max)
                        dbv = BCS_Max;
                    else if (dbv < BCS_Min)
                        dbv = BCS_Min;

                    progressBar_GB.Value = BCS_Max - dbv;

                    f1.Set_BCS(dbv);
                    DBV_textbox.Text = dbv.ToString("X");
                    DBV_trackbar.Value = dbv;

                    Thread.Sleep(miliseconds);
                    
                    double Prev_Lv = Convert.ToDouble(Lv_Value_display.Text);

                    if (radioButton_Ave_BCS_GCS_1.Checked)   Average_Measure(1, dbv.ToString());                   
                    else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, dbv.ToString());                    
                    else if (radioButton_Ave_BCS_GCS_5.Checked)Average_Measure(5, dbv.ToString());                    
                    else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked)Average_Measure_5_Min_Max_Lv_Delete(dbv.ToString());                    
                    else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked)Get_Max_Measure(2, dbv.ToString());                    
                    else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, dbv.ToString());
                    if (count != 0)//Skip the first step
                    {
                        double After_Lv = Convert.ToDouble(Lv_Value_display.Text);
                        //double Diff_Lv = After_Lv - Prev_Lv; (min to max Measure Mode)
                        double Diff_Lv = Prev_Lv - After_Lv; //(max to min Measure Mode)
                        double Diff_Lv_Per = ((Prev_Lv - After_Lv) / After_Lv) * 100;
                        //f1.GB_Status_AppendText_Nextline("Diff = After_Lv - Prev_Lv : " + Diff_Lv.ToString() + " = " + After_Lv.ToString() + " - " + prev_Lv.ToString(), Color.Blue);
                        f1.GB_Status_AppendText_Nextline("Diff = Prev_Lv - After_Lv : " + Diff_Lv.ToString() + " = " + Prev_Lv.ToString() + " - " + After_Lv.ToString(), Color.Blue);
                        f1.GB_Status_AppendText_Nextline("Diff(%) = ((Prev_Lv - After_Lv)/After_Lv)*100 : " + Diff_Lv_Per.ToString() + "(%)", Color.Blue);

                        //----Get Low or High Max_Lv_Diff and corresponding DBV-----
                        if (count == 1)
                        {
                            Low_Max_Lv_Diff_Per = Diff_Lv_Per;
                            Low_Max_Lv_Diff_Per_Correspond_DBV = dbv;
                            High_Max_Lv_Diff_Per = Diff_Lv_Per;
                            High_Max_Lv_Diff_Per_Correspond_DBV = dbv;
                        }
                        else
                        {
                            //Low Diff Spec
                            if (After_Lv < DBV_Accuracy_Lv_Threshold)
                            {
                                if (Math.Abs(Diff_Lv_Per) > Math.Abs(Low_Max_Lv_Diff_Per))
                                {
                                    Low_Max_Lv_Diff_Per = Diff_Lv_Per;
                                    Low_Max_Lv_Diff_Per_Correspond_DBV = dbv;
                                    textBox_Low_Max_Lv_Diff_Per.Text = Low_Max_Lv_Diff_Per.ToString();
                                    textBox_Low_Max_Lv_Diff_Per_Correspond_DBV.Text = Low_Max_Lv_Diff_Per_Correspond_DBV.ToString();
                                }
                            }
                            //High Diff Spec
                            else
                            {
                                if (Math.Abs(Diff_Lv_Per) > Math.Abs(High_Max_Lv_Diff_Per))
                                {
                                    High_Max_Lv_Diff_Per = Diff_Lv_Per;
                                    High_Max_Lv_Diff_Per_Correspond_DBV = dbv;
                                    textBox_High_Max_Lv_Diff_Per.Text = High_Max_Lv_Diff_Per.ToString();
                                    textBox_High_Max_Lv_Diff_Per_Correspond_DBV.Text = High_Max_Lv_Diff_Per_Correspond_DBV.ToString();
                                }
                            }
                        }
                        //--------------Drawing Points On Chart Related--------------------------
                        chart1.Series["Channel1"].Points.AddXY(dbv, Diff_Lv_Per); //Add Point to chart
                        if (After_Lv < DBV_Accuracy_Lv_Threshold)//Low
                        {
                            if (Math.Abs(Diff_Lv_Per) > DBV_Accuracy_Lv_Diff_Low_Limit) chart1.Series[0].Points[count - 1].Color = Color.FromArgb(255, 0, 0);
                            else chart1.Series[0].Points[count - 1].Color = Color.FromArgb(0, 100, 0);
                        }
                        else//High
                        {
                            if (Math.Abs(Diff_Lv_Per) > DBV_Accuracy_Lv_Diff_High_Limit) chart1.Series[0].Points[count - 1].Color = Color.FromArgb(255, 0, 255);
                            else chart1.Series[0].Points[count - 1].Color = Color.FromArgb(0, 180, 0);
                        }
                        //-----------------------------------------------------------------------
                    }
                    count++;
                    if (dbv == 1)
                    {
                        break;
                    }
                }
            }
            else if (min_to_max_rb.Checked)
            {
                int count = 0;
                for (int i = BCS_Min; i < BCS_Max + Step_Value; )
                {
                    if (GCS_BCS_Stop)
                        break;

                    i = i + Step_Value;
                    dbv = i - Step_Value;

                    if (dbv > BCS_Max)
                        dbv = BCS_Max;
                    else if (dbv < BCS_Min)
                        dbv = BCS_Min;

                    progressBar_GB.Value = dbv - BCS_Min;

                    f1.Set_BCS(dbv);
                    DBV_textbox.Text = dbv.ToString("X");
                    DBV_trackbar.Value = dbv;

                    Thread.Sleep(miliseconds);

                    double Prev_Lv = Convert.ToDouble(Lv_Value_display.Text);

                    if (radioButton_Ave_BCS_GCS_1.Checked) Average_Measure(1, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_3.Checked) Average_Measure(3, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5.Checked) Average_Measure(5, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Mix_Max_Lv_Delete.Checked) Average_Measure_5_Min_Max_Lv_Delete(dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_2_Get_Max_Lv.Checked) Get_Max_Measure(2, dbv.ToString());
                    else if (radioButton_Ave_BCS_GCS_5_Get_Max_Lv.Checked) Get_Max_Measure(5, dbv.ToString());
                    if (count != 0)//Skip the first step
                    {
                        double After_Lv = Convert.ToDouble(Lv_Value_display.Text);
                        double Diff_Lv = After_Lv - Prev_Lv; //(min to max Measure Mode)
                        double Diff_Lv_Per = ((After_Lv - Prev_Lv) / Prev_Lv) * 100;
                        //double Diff_Lv = Prev_Lv - After_Lv; //(max to min Measure Mode)
                        f1.GB_Status_AppendText_Nextline("Diff = After_Lv - Prev_Lv : " + Diff_Lv.ToString() + " = " + After_Lv.ToString() + " - " + Prev_Lv.ToString(), Color.Blue);
                        f1.GB_Status_AppendText_Nextline("Diff(%) = ((After_Lv - Prev_Lv)/Prev_Lv)*100 : " + Diff_Lv_Per.ToString() + "(%)", Color.Blue);
                        //f1.GB_Status_AppendText_Nextline("Diff = Prev_Lv - After_Lv : " + Diff_Lv.ToString() + " = " + Prev_Lv.ToString() + " - " + After_Lv.ToString(), Color.Blue);
                        //----Get Low or High Max_Lv_Diff and corresponding DBV-----
                        if (count == 1)
                        {
                            Low_Max_Lv_Diff_Per = Diff_Lv_Per;
                            Low_Max_Lv_Diff_Per_Correspond_DBV = dbv;
                            High_Max_Lv_Diff_Per = Diff_Lv_Per;
                            High_Max_Lv_Diff_Per_Correspond_DBV = dbv;
                        }
                        else
                        {
                            //Low Diff Spec
                            if (After_Lv < DBV_Accuracy_Lv_Threshold)
                            {
                                if (Math.Abs(Diff_Lv_Per) > Math.Abs(Low_Max_Lv_Diff_Per))
                                {
                                    Low_Max_Lv_Diff_Per = Diff_Lv_Per;
                                    Low_Max_Lv_Diff_Per_Correspond_DBV = dbv;
                                    textBox_Low_Max_Lv_Diff_Per.Text = Low_Max_Lv_Diff_Per.ToString();
                                    textBox_Low_Max_Lv_Diff_Per_Correspond_DBV.Text = Low_Max_Lv_Diff_Per_Correspond_DBV.ToString();
                                }
                            }
                            //High Diff Spec
                            else
                            {
                                if (Math.Abs(Diff_Lv_Per) > Math.Abs(High_Max_Lv_Diff_Per))
                                {
                                    High_Max_Lv_Diff_Per = Diff_Lv_Per;
                                    High_Max_Lv_Diff_Per_Correspond_DBV = dbv;
                                    textBox_High_Max_Lv_Diff_Per.Text = High_Max_Lv_Diff_Per.ToString();
                                    textBox_High_Max_Lv_Diff_Per_Correspond_DBV.Text = High_Max_Lv_Diff_Per_Correspond_DBV.ToString();
                                }
                            }
                        }
                        //--------------Drawing Points On Chart Related--------------------------
                        chart1.Series["Channel1"].Points.AddXY(dbv, Diff_Lv_Per); //Add Point to chart
                        if (After_Lv < DBV_Accuracy_Lv_Threshold)//Low
                        {
                            if (Math.Abs(Diff_Lv_Per) > DBV_Accuracy_Lv_Diff_Low_Limit) chart1.Series[0].Points[count - 1].Color = Color.FromArgb(255, 0, 0);
                            else chart1.Series[0].Points[count - 1].Color = Color.FromArgb(0, 100, 0);
                        }
                        else//High
                        {
                            if (Math.Abs(Diff_Lv_Per) > DBV_Accuracy_Lv_Diff_High_Limit) chart1.Series[0].Points[count - 1].Color = Color.FromArgb(255, 0, 255);
                            else chart1.Series[0].Points[count - 1].Color = Color.FromArgb(0, 180, 0);
                        }
                        //-----------------------------------------------------------------------
                    }
                    count++;

                    if (dbv == BCS_Max)
                    {
                        break;
                    }
                }
            }
            f1.GB_Status_AppendText_Nextline("BCS Measuring Finished", System.Drawing.Color.DarkGray);
        }

     
        private void DBV_trackbar_ValueChanged(object sender, EventArgs e)
        {
           
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            //This "dbv" is the point ! 
            int dbv = DBV_trackbar.Value;
            DBV_textbox.Text = dbv.ToString("X"); //dex to hex (as a string form)

            if (Convert.ToInt32(DBV_textbox.Text, 16) > DBV_trackbar.Maximum)
                DBV_textbox.Text = DBV_trackbar.Maximum.ToString("X3"); ; //"FFF";
            if (Convert.ToInt32(DBV_textbox.Text, 16) < 0)
                DBV_textbox.Text = "0";

            DBV_trackbar.Value = Convert.ToInt32(DBV_textbox.Text, 16);

            string DBV1, DBV2;
            if (dbv <= 255) //255 = FF
            {
                DBV1 = "00";
                DBV2 = dbv.ToString("X").PadLeft(2, '0');//dex to hex (as a string form)
            }
            else if (dbv <= 511) //511 = 1FF
            {
                DBV1 = "01";
                DBV2 = (dbv - 256).ToString("X"); //256 = 100
            }
            else if (dbv <= 767) //767 = 2FF
            {
                DBV1 = "02";
                DBV2 = (dbv - 512).ToString("X"); //512 = 200
            }
            else if (dbv <= 1023) //767 = 3FF
            {
                DBV1 = "03";
                DBV2 = (dbv - 768).ToString("X"); //768 = 300
            }
            else if (dbv <= 1279) //1279 = 4FF
            {
                DBV1 = "04";
                DBV2 = (dbv - 1024).ToString("X"); //1024 = 400
            }
            else if (dbv <= 1535)
            {
                DBV1 = "05";
                DBV2 = (dbv - 1280).ToString("X"); //1280 = 500
            }
            else if (dbv <= 1791)
            {
                DBV1 = "06";
                DBV2 = (dbv - 1536).ToString("X"); //1536 = 600

            }
            else if (dbv <= 2047)
            {
                DBV1 = "07";
                DBV2 = (dbv - 1792).ToString("X"); //1792 = 700
            }
            else if (dbv <= 2303)
            {
                DBV1 = "08";
                DBV2 = (dbv - 2048).ToString("X"); //2048 = 800
            }
            else if (dbv <= 2559)
            {
                DBV1 = "09";
                DBV2 = (dbv - 2304).ToString("X"); //2304 = 900
            }
            else if (dbv <= 2815)
            {
                DBV1 = "0A";
                DBV2 = (dbv - 2560).ToString("X"); //2560 = A00
            }
            else if (dbv <= 3071)
            {
                DBV1 = "0B";
                DBV2 = (dbv - 2816).ToString("X"); //2816 = B00
            }
            else if (dbv <= 3327)
            {
                DBV1 = "0C";
                DBV2 = (dbv - 3072).ToString("X"); //3072 = C00
            }
            else if (dbv <= 3583)
            {
                DBV1 = "0D";
                DBV2 = (dbv - 3328).ToString("X"); //3328 = D00
            }
            else if (dbv <= 3839)
            {
                DBV1 = "0E";
                DBV2 = (dbv - 3584).ToString("X"); //3584 = E00
            }
            else //(dbv <= 4095)
            {
                DBV1 = "0F";
                DBV2 = (dbv - 3840).ToString("X"); //3840 = F00
            }

            if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP086 || f1.current_model.Get_Current_Model_Name() == Model_Name.DP116)
            {
                
                f1.IPC_Quick_Send("mipi.write 0x15 0xFF 0x10");
                First_Model_Option_Form.getInstance().Current_Page_Address = "0x10";
            }
            f1.IPC_Quick_Send("mipi.write 0x39 0x51 0x" + DBV1 + " 0x" + DBV2);
        }

        private void Get_Max_Measure(int Measure_times = 2,string GCS_Or_BCS_String = "-")
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            XYLv[] Measure = new XYLv[Measure_times];
            int Max_Index = 0; double Max_Value = 0;

            try
            {
                f1.isMsr = true;
                CA_Measure_button.Enabled = false;
                if (Measure_times > 1)
                {
                    for (int a = 0; a < Measure_times; a++)
                    {
                        f1.objCa.Measure();
                        Measure[a].X = f1.objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                        Measure[a].Y = f1.objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                        Measure[a].Lv = f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                        Measure[a].Double_Update_From_String();

                        if (Measure[a].double_Lv > Max_Value)
                        {
                            Max_Value = Measure[a].double_Lv;
                            Max_Index = a;
                        }

                        f1.GB_Status_AppendText_Nextline(a.ToString() + ")Measured X/Y/Lv = " + Measure[a].X + "/" + Measure[a].Y + "/" + Measure[a].Lv, Color.Green);
                    }
                    f1.GB_Status_AppendText_Nextline("Max_Lv Index = " + Max_Index.ToString(), Color.Black);
                    f1.GB_Status_AppendText_Nextline("Max X/Y/Lv = " + Measure[Max_Index].X + "/" + Measure[Max_Index].Y + "/" + Measure[Max_Index].Lv, Color.Blue);

                    X_Value_display.Text = Measure[Max_Index].X.PadRight(6, '0');
                    Y_Value_display.Text = Measure[Max_Index].Y.PadRight(6, '0');
                    Lv_Value_display.Text = Measure[Max_Index].Lv.PadRight(6, '0');
                }
                else if (Measure_times == 1)
                {
                    f1.objCa.Measure();
                    X_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).sx.ToString("0.0000");
                    Y_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).sy.ToString("0.0000");
                    Lv_Value_display.Text = f1.objCa.OutputProbes.get_ItemOfNumber(1).Lv.ToString("0.0000");
                }
                //Data Grid setting//////////////////////
                dataGridView2.DataSource = null; // reset (unbind the datasource)

                //Add data to Datagridview
                dataGridView2.Rows.Add(GCS_Or_BCS_String, X_Value_display.Text, Y_Value_display.Text, Lv_Value_display.Text);
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;

                System.Windows.Forms.Application.DoEvents();
                CA_Measure_button.Enabled = true;
            }
            catch (Exception er)
            {
                f1.DisplayError(er);
                System.Windows.Forms.Application.Exit();
            }
        }




        private void change_DBV_button_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (Convert.ToInt32(DBV_textbox.Text, 16) > DBV_trackbar.Maximum)
                DBV_textbox.Text = DBV_trackbar.Maximum.ToString("X3");
            if (Convert.ToInt32(DBV_textbox.Text, 16) < 0)
                DBV_textbox.Text = "0";

            //This "dbv" is the point !
            int dbv = Convert.ToInt32(DBV_textbox.Text, 16);

            DBV_trackbar.Value = Convert.ToInt32(DBV_textbox.Text, 16);

            string DBV1, DBV2;
            if (dbv <= 255) //255 = FF
            {
                DBV1 = "00";
                DBV2 = dbv.ToString("X").PadLeft(2, '0');//dex to hex (as a string form)
            }
            else if (dbv <= 511) //511 = 1FF
            {
                DBV1 = "01";
                DBV2 = (dbv - 256).ToString("X"); //256 = 100
            }
            else if (dbv <= 767) //767 = 2FF
            {
                DBV1 = "02";
                DBV2 = (dbv - 512).ToString("X"); //512 = 200
            }
            else if (dbv <= 1023) //767 = 3FF
            {
                DBV1 = "03";
                DBV2 = (dbv - 768).ToString("X"); //768 = 300
            }
            else if (dbv <= 1279) //1279 = 4FF
            {
                DBV1 = "04";
                DBV2 = (dbv - 1024).ToString("X"); //1024 = 400
            }
            else if (dbv <= 1535)
            {
                DBV1 = "05";
                DBV2 = (dbv - 1280).ToString("X"); //1280 = 500
            }
            else if (dbv <= 1791)
            {
                DBV1 = "06";
                DBV2 = (dbv - 1536).ToString("X"); //1536 = 600

            }
            else if (dbv <= 2047)
            {
                DBV1 = "07";
                DBV2 = (dbv - 1792).ToString("X"); //1792 = 700
            }
            else if (dbv <= 2303)
            {
                DBV1 = "08";
                DBV2 = (dbv - 2048).ToString("X"); //2048 = 800
            }
            else if (dbv <= 2559)
            {
                DBV1 = "09";
                DBV2 = (dbv - 2304).ToString("X"); //2304 = 900
            }
            else if (dbv <= 2815)
            {
                DBV1 = "0A";
                DBV2 = (dbv - 2560).ToString("X"); //2560 = A00
            }
            else if (dbv <= 3071)
            {
                DBV1 = "0B";
                DBV2 = (dbv - 2816).ToString("X"); //2816 = B00
            }
            else if (dbv <= 3327)
            {
                DBV1 = "0C";
                DBV2 = (dbv - 3072).ToString("X"); //3072 = C00
            }
            else if (dbv <= 3583)
            {
                DBV1 = "0D";
                DBV2 = (dbv - 3328).ToString("X"); //3328 = D00
            }
            else if (dbv <= 3839)
            {
                DBV1 = "0E";
                DBV2 = (dbv - 3584).ToString("X"); //3584 = E00
            }
            else //(dbv <= 4095)
            {
                DBV1 = "0F";
                DBV2 = (dbv - 3840).ToString("X"); //3840 = F00
            }

            if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP086 || f1.current_model.Get_Current_Model_Name() == Model_Name.DP116)
            {
                f1.IPC_Quick_Send("mipi.write 0x15 0xFF 0x10");
                First_Model_Option_Form.getInstance().Current_Page_Address = "0x10";
            }
            f1.IPC_Quick_Send("mipi.write 0x39 0x51 0x" + DBV1 + " 0x" + DBV2);
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            //This gives the corresponding X and Y coordinates of the mouse point.
            Point pos = e.Location;
            if (Previouse_Pos.X == pos.X && Previouse_Pos.Y == pos.Y) return;

            toolTip1.RemoveAll();
            var results = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    double xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                    double yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);
                    xVal = Math.Round(xVal, 5);
                    yVal = Math.Round(yVal, 5);
                    toolTip1.Show("(X,Y):(" + xVal.ToString() + "," + yVal.ToString() + ")", chart1, pos.X, pos.Y);
                }
            }
            Previouse_Pos.X = pos.X;
            Previouse_Pos.Y = pos.Y;
        }

        private void button_Chart_Type_Change_Click(object sender, EventArgs e)
        {
            if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");

            if (comboBox_Chart_Type.SelectedItem.ToString() == "Point") chart1.Series["Channel1"].ChartType = SeriesChartType.Point;
            else if (comboBox_Chart_Type.SelectedItem.ToString() == "FastPoint") chart1.Series["Channel1"].ChartType = SeriesChartType.FastPoint;
            else if (comboBox_Chart_Type.SelectedItem.ToString() == "Line") chart1.Series["Channel1"].ChartType = SeriesChartType.Line;
            else if (comboBox_Chart_Type.SelectedItem.ToString() == "StepLine") chart1.Series["Channel1"].ChartType = SeriesChartType.StepLine;
            else if (comboBox_Chart_Type.SelectedItem.ToString() == "FastLine") chart1.Series["Channel1"].ChartType = SeriesChartType.FastLine;
            else if (comboBox_Chart_Type.SelectedItem.ToString() == "Column") chart1.Series["Channel1"].ChartType = SeriesChartType.Column;
            else if (comboBox_Chart_Type.SelectedItem.ToString() == "StackedColumn") chart1.Series["Channel1"].ChartType = SeriesChartType.StackedColumn;  
        }

        private void radioButton_Chart_Point_Marker_Size_1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Chart_Point_Marker_Size_1.Checked)
            {
                if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
                chart1.Series["Channel1"].MarkerSize = 1;
            }
        }

        private void radioButton_Chart_Point_Marker_Size_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Chart_Point_Marker_Size_2.Checked)
            {
                if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
                chart1.Series["Channel1"].MarkerSize = 2;
            }
        }

        private void radioButton_Chart_Point_Marker_Size_4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Chart_Point_Marker_Size_4.Checked)
            {
                if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
                chart1.Series["Channel1"].MarkerSize = 4;
            }
        }

        private void radioButton_Chart_Point_Marker_Size_8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Chart_Point_Marker_Size_8.Checked)
            {
                if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
                chart1.Series["Channel1"].MarkerSize = 8;
            }
        }

        private void radioButton_Y_Min_Max_Manual_Setting_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Y_Min_Max_Manual_Setting.Checked)
            {
                if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
                chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox_Y_Max.Text);
                chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox_Y_Min.Text);
            }
        }

        private void radioButton_Y_Min_Max_Auto_Setting_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Y_Min_Max_Auto_Setting.Checked)
            {
                if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
                chart1.ChartAreas[0].AxisY.Maximum = Double.NaN;
                chart1.ChartAreas[0].AxisY.Minimum = Double.NaN;
                chart1.ChartAreas[0].RecalculateAxesScale();
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton_X_Min_Max_Manual_Setting_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_X_Min_Max_Manual_Setting.Checked)
            {
                if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
                chart1.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox_X_Max.Text);
                chart1.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox_X_Min.Text);
            }
        }

        private void radioButton_X_Min_Max_Auto_Setting_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_X_Min_Max_Auto_Setting.Checked)
            {
                if (chart1.Series.IsUniqueName("Channel1")) chart1.Series.Add("Channel1");
                chart1.ChartAreas[0].AxisX.Maximum = Double.NaN;
                chart1.ChartAreas[0].AxisX.Minimum = Double.NaN;
                chart1.ChartAreas[0].RecalculateAxesScale();
            }
        }

        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
