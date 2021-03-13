using System;
using System.Drawing;
using System.Windows.Forms;

namespace PNC_Csharp
{
    public partial class Dual_SH_Delta_E : Form
    {

        //Form Parameter
        public bool Availability = false;

        private void Gridview_Tema_Setting(DataGridView dataGridView)
        {
            //Set the datagridview's EnableHeadersVisualStyles to false to get the header cell to accept the color change
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;

            //Delta E Data Grid view initialize
            dataGridView.Columns.Add("Gray", "Gray");
            dataGridView.Columns.Add("x", "x");
            dataGridView.Columns.Add("y", "y");
            dataGridView.Columns.Add("Lv", "Lv");
            dataGridView.Columns.Add("X", "X");
            dataGridView.Columns.Add("Y", "Y");
            dataGridView.Columns.Add("Z", "Z");
            dataGridView.Columns.Add("L*", "L*");
            dataGridView.Columns.Add("f(X/Xw)", "f(X/Xw)");
            dataGridView.Columns.Add("f(Y/Yw)", "f(Y/Yw)");
            dataGridView.Columns.Add("f(Z/Zw)", "f(Z/Zw)");
            dataGridView.Columns.Add("a*", "a*");
            dataGridView.Columns.Add("b*", "b*");
            dataGridView.Columns.Add("Delta E*", "Delta E");

            //Auto size (columns)
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //change color for X/Y/Lv Measured area  
            dataGridView.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dataGridView.Columns[0].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView.Columns[0].HeaderCell.Style.BackColor = System.Drawing.Color.Gray;
            dataGridView.Columns[0].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
            dataGridView.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;

            for (int i = 1; i <= 3; i++)
            {
                dataGridView.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridView.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView.Columns[i].HeaderCell.Style.BackColor = System.Drawing.Color.Pink;
                dataGridView.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            }

            for (int i = 4; i <= 6; i++)
            {
                dataGridView.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                dataGridView.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView.Columns[i].HeaderCell.Style.BackColor = System.Drawing.Color.Green;
                dataGridView.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            }

            for (int i = 7; i <= 10; i++)
            {
                dataGridView.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                dataGridView.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView.Columns[i].HeaderCell.Style.BackColor = System.Drawing.Color.Salmon;
                dataGridView.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            }

            for (int i = 11; i <= 12; i++)
            {
                dataGridView.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                dataGridView.Columns[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView.Columns[i].HeaderCell.Style.BackColor = System.Drawing.Color.Cyan;
                dataGridView.Columns[i].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
                dataGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
            }

            dataGridView.Columns[13].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            dataGridView.Columns[13].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView.Columns[13].HeaderCell.Style.BackColor = System.Drawing.Color.Coral;
            dataGridView.Columns[13].HeaderCell.Style.Font = new Font(this.Font, System.Drawing.FontStyle.Regular);
            dataGridView.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
        }

        private static Dual_SH_Delta_E Instance;
        public static Dual_SH_Delta_E getInstance() 
        {
            if (Instance == null)
                Instance = new Dual_SH_Delta_E();

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

        private Dual_SH_Delta_E()
        {
            InitializeComponent();
            Gridview_Tema_Setting(dataGridView1);
            Gridview_Tema_Setting(dataGridView2);
        }

        private void Dual_SH_Delta_E_Load(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
           
            Dual_Engineer_Monitoring_Mode Dual_Engineer_Mode = (Dual_Engineer_Monitoring_Mode)System.Windows.Forms.Application.OpenForms["Dual_Engineer_Monitoring_Mode"];
            DP150_Dual_Engineering_Mornitoring_Mode Dp150_Dual_Engineer_Mode = (DP150_Dual_Engineering_Mornitoring_Mode)System.Windows.Forms.Application.OpenForms["DP150_Dual_Engineering_Mornitoring_Mode"];
            
            if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP086 || f1.current_model.Get_Current_Model_Name() == Model_Name.DP116)
                this.BackColor = Dual_Engineer_Mode.BackColor;
            else if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP150)
                this.BackColor = Dp150_Dual_Engineer_Mode.BackColor;
        }
        private double Normal_Calculate_Delta_E3(DataGridView dataGridView, int gray_end_Point)
        {
            {//Condition 1
                // x/y/Lv 계산 완료 후 -- > X Y Z 계산
                double x;
                double y;
                double Lv;

                double X;
                double Y;
                double Z;


                for (int gray = gray_end_Point; gray <= 255; gray++)
                {
                    x = Convert.ToDouble(dataGridView.Rows[gray - gray_end_Point].Cells[1].Value);
                    y = Convert.ToDouble(dataGridView.Rows[gray - gray_end_Point].Cells[2].Value);
                    Lv = Convert.ToDouble(dataGridView.Rows[gray - gray_end_Point].Cells[3].Value);

                    X = (x / y) * Lv;
                    Y = Lv;
                    Z = ((1 - x - y) / y) * Lv;

                    dataGridView.Rows[gray - gray_end_Point].Cells[4].Value = X;
                    dataGridView.Rows[gray - gray_end_Point].Cells[5].Value = Y;
                    dataGridView.Rows[gray - gray_end_Point].Cells[6].Value = Z;


                }


                // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
                double L, FX, FY, FZ;

                double X255 = Convert.ToDouble(dataGridView.Rows[255 - gray_end_Point].Cells[4].Value);
                double Y255 = Convert.ToDouble(dataGridView.Rows[255 - gray_end_Point].Cells[5].Value);
                double Z255 = Convert.ToDouble(dataGridView.Rows[255 - gray_end_Point].Cells[6].Value);

                //a* , b* , Delta E
                double a, b, Delta_E;
                double Max_Delta_E = 0;


                for (int gray = gray_end_Point; gray <= 255; gray++)
                {
                    X = Convert.ToDouble(dataGridView.Rows[gray - gray_end_Point].Cells[4].Value);
                    Y = Convert.ToDouble(dataGridView.Rows[gray - gray_end_Point].Cells[5].Value);
                    Z = Convert.ToDouble(dataGridView.Rows[gray - gray_end_Point].Cells[6].Value);

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

                    dataGridView.Rows[gray - gray_end_Point].Cells[7].Value = L;
                    dataGridView.Rows[gray - gray_end_Point].Cells[8].Value = FX;
                    dataGridView.Rows[gray - gray_end_Point].Cells[9].Value = FY;
                    dataGridView.Rows[gray - gray_end_Point].Cells[10].Value = FZ;

                    //Calculate a* , b* , Delta E
                    a = 500 * (FX - FY);
                    b = 200 * (FY - FZ);
                    Delta_E = Math.Pow((Math.Pow(a, 2) + Math.Pow(b, 2)), 0.5);

                    dataGridView.Rows[gray - gray_end_Point].Cells[11].Value = a; //a*
                    dataGridView.Rows[gray - gray_end_Point].Cells[12].Value = b; //b*
                    dataGridView.Rows[gray - gray_end_Point].Cells[13].Value = Delta_E; //Delta E

                    if (Max_Delta_E <= Delta_E)
                    {
                        Max_Delta_E = Delta_E;
                    }
                }

                return Max_Delta_E;
               
            }
        }

        private void Button_Click_Enable(bool Enable)
        {
            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
            if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP086 || f1.current_model.Get_Current_Model_Name() == Model_Name.DP116)
                f1.Dual_Engineering_Mornitoring_Hide_Button_Enable(Enable);
            else if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP150) 
                f1.Dual_DP150_Engineering_Mornitoring_Hide_Button_Enable(Enable);

            Delta_E_calculation_btn.Enabled = Enable;
            IR_Drop_Delta_E_btn.Enabled = Enable;
            Delta_E2_calculation_btn.Enabled = Enable;
            button_Clear.Enabled = Enable;
            button_SH_Difference_Measure.Enabled = Enable;
        }

        private double Special_1_Calculate_Delta_E(int gray_end_Point,int max)
        {
            double[] x = new double[2];
            double[] y = new double[2];
            double[] Lv = new double[2];

            double[] X = new double[2];
            double[] Y = new double[2];
            double[] Z = new double[2];

            //a* , b* , Delta E
            double L,FX, FY, FZ,a,b,Delta_E;

            double Max_Delta_E = 0;

            for (int gray = gray_end_Point; gray <= max; gray++)
            {
                //x,y,Lv(0) to X,Y,Z (0)
                x[0] = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[1].Value);
                y[0] = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[2].Value);
                Lv[0] = Convert.ToDouble(dataGridView1.Rows[gray - gray_end_Point].Cells[3].Value);

                X[0] = (x[0] / y[0]) * Lv[0];
                Y[0] = Lv[0];
                Z[0] = ((1 - x[0] - y[0]) / y[0]) * Lv[0];

                dataGridView1.Rows[gray - gray_end_Point].Cells[4].Value = X[0];
                dataGridView1.Rows[gray - gray_end_Point].Cells[5].Value = Y[0];
                dataGridView1.Rows[gray - gray_end_Point].Cells[6].Value = Z[0];

                //x,y,Lv(1) to X,Y,Z (1)
                x[1] = Convert.ToDouble(dataGridView2.Rows[gray - gray_end_Point].Cells[1].Value);
                y[1] = Convert.ToDouble(dataGridView2.Rows[gray - gray_end_Point].Cells[2].Value);
                Lv[1] = Convert.ToDouble(dataGridView2.Rows[gray - gray_end_Point].Cells[3].Value);

                X[1] = (x[1] / y[1]) * Lv[1];
                Y[1] = Lv[1];
                Z[1] = ((1 - x[1] - y[1]) / y[1]) * Lv[1];

                dataGridView2.Rows[gray - gray_end_Point].Cells[4].Value = X[1];
                dataGridView2.Rows[gray - gray_end_Point].Cells[5].Value = Y[1];
                dataGridView2.Rows[gray - gray_end_Point].Cells[6].Value = Z[1];


                // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
                //Calculate L*
                if (Y[1] / Y[0] > 0.008856)
                {
                    L = 116 * Math.Pow(Y[1] / Y[0], 0.33333333) - 16;
                }
                else
                {
                    L = 903.3 * (Y[1] / Y[0]);
                }

                //Calculate F(X/Xw)
                if (X[1] / X[0] > 0.008856)
                {
                    FX = Math.Pow((X[1] / X[0]), 0.33333333);
                }
                else
                {
                    FX = 7.787 * (X[1] / X[0]) + (16 / 116.0);
                }

                //Calculate F(Y/Yw)
                if (Y[1] / Y[0] > 0.008856)
                {
                    FY = Math.Pow((Y[1] / Y[0]), 0.33333333);
                }
                else
                {
                    FY = 7.787 * (Y[1] / Y[0]) + (16 / 116.0);
                }

                //Calculate F(Z/Zw)
                if (Z[1] / Z[0] > 0.008856)
                {
                    FZ = Math.Pow((Z[1] / Z[0]), 0.33333333);
                }
                else
                {
                    FZ = 7.787 * (Z[1] / Z[0]) + (16 / 116.0);
                }

                dataGridView2.Rows[gray - gray_end_Point].Cells[7].Value = L;
                dataGridView2.Rows[gray - gray_end_Point].Cells[8].Value = FX;
                dataGridView2.Rows[gray - gray_end_Point].Cells[9].Value = FY;
                dataGridView2.Rows[gray - gray_end_Point].Cells[10].Value = FZ;

                //Calculate a* , b* , Delta E
                a = 500 * (FX - FY);
                b = 200 * (FY - FZ);
                Delta_E = Math.Pow((Math.Pow(a, 2) + Math.Pow(b, 2)), 0.5);

                dataGridView2.Rows[gray - gray_end_Point].Cells[11].Value = a; //a*
                dataGridView2.Rows[gray - gray_end_Point].Cells[12].Value = b; //b*
                dataGridView2.Rows[gray - gray_end_Point].Cells[13].Value = Delta_E; //Delta E

                if (Max_Delta_E <= Delta_E)
                {
                    Max_Delta_E = Delta_E;
                }
            }

            return Max_Delta_E;
        }
        
        private void Delta_E_calculation_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderText = "Gray";
            dataGridView2.Columns[0].HeaderText = "Gray";

            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
            if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP086 || f1.current_model.Get_Current_Model_Name() == Model_Name.DP116)
                f1.Dual_Engineering_Mornitoring_Show();
            else if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP150)
                f1.Dual_150_Engineering_Mornitoring_Show();

            Button_Click_Enable(false);

            int gray_end_Point = Convert.ToInt16(textBox_Delta_E_End_Point.Text);

            if (gray_end_Point >= 254)
                gray_end_Point = 254;
            else if (gray_end_Point <= 0)
                gray_end_Point = 0;
            else { }

            textBox_Delta_E_End_Point.Text = gray_end_Point.ToString();

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
            f1.Dual_SH_Delta_E3_Measure(gray_end_Point, delay_time_between_measurement);

            if (Availability)
            {

                if (radioButton_Normal_DE_Calculate.Checked)
                {
                    f1.GB_Status_AppendText_Nextline("Dual Normal Delta E3 Calculation", Color.Blue);
                    //Condition 1
                    double Max_Delta_E3 = Normal_Calculate_Delta_E3(dataGridView1, gray_end_Point);
                    textBox_Measured_DeltaE.Text = Max_Delta_E3.ToString().Substring(0, 5);
                    dataGridView1.Rows.Add("Delta E3"); // 한열은 띄어쓰기로
                    dataGridView1.Rows.Add(Max_Delta_E3.ToString());
                    //Condition 2
                    Max_Delta_E3 = Normal_Calculate_Delta_E3(dataGridView2, gray_end_Point);
                    textBox_Measured_DeltaE_2.Text = Max_Delta_E3.ToString().Substring(0, 5);
                    dataGridView2.Rows.Add("Delta E3"); // 한열은 띄어쓰기로
                    dataGridView2.Rows.Add(Max_Delta_E3.ToString());
                }
                else if (radioButton_Special_1_DE_Calculate.Checked)
                {
                    f1.GB_Status_AppendText_Nextline("Dual Delta E3 Calculation", Color.Blue);
                    double Max_Delta_E = Special_1_Calculate_Delta_E(gray_end_Point,255);
                    textBox_Measured_DeltaE.Text = string.Empty;
                    textBox_Measured_DeltaE_2.Text = Max_Delta_E.ToString().Substring(0, 5);
                    dataGridView2.Rows.Add("Delta E3(Special 1)"); // 한열은 띄어쓰기로
                    dataGridView2.Rows.Add(Max_Delta_E.ToString());
                }
                else
                {
                    //Nothing happens
                }
                //Condition 1


                //Condition 2

            }
            progressBar_GB.PerformStep();
            Button_Click_Enable(true);
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

        

        private void SH_Delta_E_Load(object sender, EventArgs e)
        {

        }

        private void IRC_Drop_Delta_E_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderText = "Index";

            Delta_E_calculation_btn.Enabled = false;
            IR_Drop_Delta_E_btn.Enabled = false;
            
            button_Clear.Enabled = false;
            ////////////////
            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];

            dataGridView1.Rows.Clear();
            Availability = true;

            //measure delay time setting
            int delay_time_between_measurement = Convert.ToInt16(textBox_delay_time.Text);

            //재 계산시 대비 , 보여지는 Delta E value 초기화
            textBox_Measured_DeltaE.Text = string.Empty;

            //progressbar setting
            progressBar_GB.Maximum = 49; //208번 측정(48~255) + 나머지 전환과정 끝나면 1 = 49
            progressBar_GB.Step = 1;
            progressBar_GB.Value = 0;

            f1.Dual_SH_IR_Drop_Delta_E_Measure(delay_time_between_measurement);

            if (Availability)
            {

                // x/y/Lv 계산 완료 후 -- > X Y Z 계산
                double x;
                double y;
                double Lv;

                double X;
                double Y;
                double Z;


                for (int index = 0; index <= 49 & Availability; index++)
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

                double X255 = Convert.ToDouble(dataGridView1.Rows[49].Cells[4].Value);
                double Y255 = Convert.ToDouble(dataGridView1.Rows[49].Cells[5].Value);
                double Z255 = Convert.ToDouble(dataGridView1.Rows[49].Cells[6].Value);

                //a* , b* , Delta E
                double a, b, Delta_E;
                double Average_Delta_E = 0;
                double Total_Delta_E = 0;


                for (int index = 0; index <= 49 & Availability; index++)
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

                for (int index = 0; index <= 49 & Availability; index += 2)
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

                Average_Delta_E = Total_Delta_E / 25;
                textBox_Measured_IR_Drop_Delta_E.Text = Average_Delta_E.ToString();

                //Excel 에 Data 남기기 위한 자료 추가.
                dataGridView1.Rows.Add("IR Drop DE"); // 한열은 띄어쓰기로
                dataGridView1.Rows.Add(textBox_Measured_IR_Drop_Delta_E.Text);

                progressBar_GB.PerformStep();
            }
            ////////////////
            Delta_E_calculation_btn.Enabled = true;
            IR_Drop_Delta_E_btn.Enabled = true;
            
            button_Clear.Enabled = true;
        }

        private void DP116_IRC_Drop_Delta_E_Pattern(Color Inner_color, bool Full_PTN = false)
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


        public void DP116_IRC_Drop_Square_PTN_List(int index)
        {
            switch (index)
            {
                case 0:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0));
                    break;
                case 1:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 0), true);
                    break;
                case 2:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0));
                    break;
                case 3:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 0), true);
                    break;
                case 4:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255));
                    break;
                case 5:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 0, 255), true);
                    break;
                case 6:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0));
                    break;
                case 7:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 0), true);
                    break;
                case 8:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255));
                    break;
                case 9:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 255, 255), true);
                    break;
                case 10:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255));
                    break;
                case 11:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 0, 255), true);
                    break;
                case 12:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66));
                    break;
                case 13:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(115, 82, 66), true);
                    break;
                case 14:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130));
                    break;
                case 15:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 150, 130), true);
                    break;
                case 16:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156));
                    break;
                case 17:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(94, 122, 156), true);
                    break;
                case 18:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66));
                    break;
                case 19:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(89, 107, 66), true);
                    break;
                case 20:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176));
                    break;
                case 21:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(130, 128, 176), true);
                    break;
                case 22:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168));
                    break;
                case 23:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(99, 189, 168), true);
                    break;
                case 24:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41));
                    break;
                case 25:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(217, 120, 41), true);
                    break;
                case 26:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163));
                    break;
                case 27:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(74, 92, 163), true);
                    break;
                case 28:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97));
                    break;
                case 29:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(194, 84, 97), true);
                    break;
                case 30:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107));
                    break;
                case 31:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(92, 61, 107), true);
                    break;
                case 32:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64));
                    break;
                case 33:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(158, 186, 64), true);
                    break;
                case 34:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46));
                    break;
                case 35:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(230, 161, 46), true);
                    break;
                case 36:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150));
                    break;
                case 37:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(51, 61, 150), true);
                    break;
                case 38:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71));
                    break;
                case 39:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(71, 148, 71), true);
                    break;
                case 40:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59));
                    break;
                case 41:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(176, 48, 59), true);
                    break;
                case 42:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33));
                    break;
                case 43:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(237, 199, 33), true);
                    break;
                case 44:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145));
                    break;
                case 45:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(186, 84, 145), true);
                    break;
                case 46:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163));
                    break;
                case 47:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(0, 133, 163), true);
                    break;
                case 48:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255));
                    break;
                case 49:
                    DP116_IRC_Drop_Delta_E_Pattern(Color.FromArgb(255, 255, 255), true);
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
            dataGridView2.Rows.Clear();
        }
        
        

        private double Normal_IR_Drop_Denta_E_Calculation(DataGridView datagridview)
        {
            {//Condition1
                // x/y/Lv 계산 완료 후 -- > X Y Z 계산
                double x;
                double y;
                double Lv;

                double X;
                double Y;
                double Z;


                for (int index = 0; index <= 49 & Availability; index++)
                {
                    x = Convert.ToDouble(datagridview.Rows[index].Cells[1].Value);
                    y = Convert.ToDouble(datagridview.Rows[index].Cells[2].Value);
                    Lv = Convert.ToDouble(datagridview.Rows[index].Cells[3].Value);

                    X = (x / y) * Lv;
                    Y = Lv;
                    Z = ((1 - x - y) / y) * Lv;

                    datagridview.Rows[index].Cells[4].Value = X;
                    datagridview.Rows[index].Cells[5].Value = Y;
                    datagridview.Rows[index].Cells[6].Value = Z;
                }


                // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
                double L, FX, FY, FZ;

                double X255 = Convert.ToDouble(datagridview.Rows[49].Cells[4].Value);
                double Y255 = Convert.ToDouble(datagridview.Rows[49].Cells[5].Value);
                double Z255 = Convert.ToDouble(datagridview.Rows[49].Cells[6].Value);

                //a* , b* , Delta E
                double a, b, Delta_E;
                double Average_Delta_E = 0;
                double Total_Delta_E = 0;


                for (int index = 0; index <= 49 & Availability; index++)
                {
                    X = Convert.ToDouble(datagridview.Rows[index].Cells[4].Value);
                    Y = Convert.ToDouble(datagridview.Rows[index].Cells[5].Value);
                    Z = Convert.ToDouble(datagridview.Rows[index].Cells[6].Value);

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

                    datagridview.Rows[index].Cells[7].Value = L;
                    datagridview.Rows[index].Cells[8].Value = FX;
                    datagridview.Rows[index].Cells[9].Value = FY;
                    datagridview.Rows[index].Cells[10].Value = FZ;

                    //Calculate a* , b* , Delta E
                    a = 500 * (FX - FY);
                    b = 200 * (FY - FZ);


                    datagridview.Rows[index].Cells[11].Value = a; //a*
                    datagridview.Rows[index].Cells[12].Value = b; //b*

                }

                for (int index = 0; index <= 49 & Availability; index += 2)
                {
                    double L1 = Convert.ToDouble(datagridview.Rows[index].Cells[7].Value);
                    double L2 = Convert.ToDouble(datagridview.Rows[index + 1].Cells[7].Value);
                    double a1 = Convert.ToDouble(datagridview.Rows[index].Cells[11].Value);
                    double a2 = Convert.ToDouble(datagridview.Rows[index + 1].Cells[11].Value);
                    double b1 = Convert.ToDouble(datagridview.Rows[index].Cells[12].Value);
                    double b2 = Convert.ToDouble(datagridview.Rows[index + 1].Cells[12].Value);

                    double delta_L = L1 - L2;
                    double delta_a = a1 - a2;
                    double delta_b = b1 - b2;

                    Delta_E = Math.Pow((Math.Pow(delta_L, 2) + Math.Pow(delta_a, 2) + Math.Pow(delta_b, 2)), 0.5);
                    Total_Delta_E += Delta_E;

                    datagridview.Rows[index].Cells[13].Value = Delta_E; //Delta E

                    //f1.GB_Status_AppendText_Nextline(delta_L.ToString() + " / " + delta_a.ToString() + " / " + delta_b.ToString(), Color.Black);
                    //f1.GB_Status_AppendText_Nextline(Delta_E.ToString(), Color.Black);
                }

                Average_Delta_E = Total_Delta_E / 25;

                return Average_Delta_E;
            }
        }

        private void IR_Drop_Delta_E_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderText = "Index";
            dataGridView2.Columns[0].HeaderText = "Index";

            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
            if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP086 || f1.current_model.Get_Current_Model_Name() == Model_Name.DP116)
                 f1.Dual_Engineering_Mornitoring_Show();
            else if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP150)
                f1.Dual_150_Engineering_Mornitoring_Show();

            this.Button_Click_Enable(false);

            Availability = true;

            //measure delay time setting
            int delay_time_between_measurement = Convert.ToInt16(textBox_delay_time.Text);

            //재 계산시 대비 , 보여지는 Delta E value 초기화
            textBox_Measured_DeltaE.Text = string.Empty;

            //progressbar setting
            progressBar_GB.Maximum = 49; //208번 측정(48~255) + 나머지 전환과정 끝나면 1 = 49
            progressBar_GB.Step = 1;
            progressBar_GB.Value = 0;

            f1.Dual_SH_IR_Drop_Delta_E_Measure(delay_time_between_measurement);

            if (Availability)
            {
                    f1.GB_Status_AppendText_Nextline("Dual Normal Delta E Calculation", Color.Blue);
                    //Conditon 1
                    double Average_Delta_E = Normal_IR_Drop_Denta_E_Calculation(dataGridView1);
                    textBox_Measured_IR_Drop_Delta_E.Text = Average_Delta_E.ToString().Substring(0, 5);
                    dataGridView1.Rows.Add("IR Drop DE"); // 한열은 띄어쓰기로
                    dataGridView1.Rows.Add(Average_Delta_E.ToString());

                    //Conditon 2
                    Average_Delta_E = Normal_IR_Drop_Denta_E_Calculation(dataGridView2);
                    textBox_Measured_IR_Drop_Delta_E_2.Text = Average_Delta_E.ToString().Substring(0, 5);
                    dataGridView2.Rows.Add("IR Drop DE"); // 한열은 띄어쓰기로
                    dataGridView2.Rows.Add(Average_Delta_E.ToString());
            }
            ////////////////
            progressBar_GB.PerformStep();
            this.Button_Click_Enable(true);
        }

        private void deltaEMeasureToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private double Normal_Delta_E2_Calculate(DataGridView datagridview,int dbv_end_Point, int Step_Value)
        {
            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];

            {//Conditon 1
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

                    x = Convert.ToDouble(datagridview.Rows[count].Cells[1].Value);
                    y = Convert.ToDouble(datagridview.Rows[count].Cells[2].Value);
                    Lv = Convert.ToDouble(datagridview.Rows[count].Cells[3].Value);

                    X = (x / y) * Lv;
                    Y = Lv;
                    Z = ((1 - x - y) / y) * Lv;

                    datagridview.Rows[count].Cells[4].Value = X;
                    datagridview.Rows[count].Cells[5].Value = Y;
                    datagridview.Rows[count].Cells[6].Value = Z;

                    count++;
                }


                // X/Y/Z 계산 완료 후 --> L* / f(X/Xw) / f(Y/Yw) / f(Z/Zw) 
                double L, FX, FY, FZ;

                double X4095 = Convert.ToDouble(datagridview.Rows[count - 1].Cells[4].Value);
                double Y4095 = Convert.ToDouble(datagridview.Rows[count - 1].Cells[5].Value);
                double Z4095 = Convert.ToDouble(datagridview.Rows[count - 1].Cells[6].Value);

                //a* , b* , Delta E
                double a, b, Delta_E;
                double Max_Delta_E = 0;

                count = 0;
                for (int i = dbv_end_Point; i < (f1.Get_DBV_TrackBar_Maximum() + Step_Value); )
                {
                    i += Step_Value;

                    if (i - Step_Value > f1.Get_DBV_TrackBar_Maximum())
                        break;

                    X = Convert.ToDouble(datagridview.Rows[count].Cells[4].Value);
                    Y = Convert.ToDouble(datagridview.Rows[count].Cells[5].Value);
                    Z = Convert.ToDouble(datagridview.Rows[count].Cells[6].Value);

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

                    datagridview.Rows[count].Cells[7].Value = L;
                    datagridview.Rows[count].Cells[8].Value = FX;
                    datagridview.Rows[count].Cells[9].Value = FY;
                    datagridview.Rows[count].Cells[10].Value = FZ;

                    //Calculate a* , b* , Delta E
                    a = 500 * (FX - FY);
                    b = 200 * (FY - FZ);
                    Delta_E = Math.Pow((Math.Pow(a, 2) + Math.Pow(b, 2)), 0.5);

                    datagridview.Rows[count].Cells[11].Value = a; //a*
                    datagridview.Rows[count].Cells[12].Value = b; //b*
                    datagridview.Rows[count].Cells[13].Value = Delta_E; //Delta E

                    if (Max_Delta_E <= Delta_E)
                    {
                        Max_Delta_E = Delta_E;
                    }

                    count++;
                }

                return Max_Delta_E;
            }
        }

        private void Delta_E2_calculation_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderText = "DBV";
            dataGridView2.Columns[0].HeaderText = "DBV";

            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
           
            if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP086 || f1.current_model.Get_Current_Model_Name() == Model_Name.DP116)
                f1.Dual_Engineering_Mornitoring_Show();
            else if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP150)
                f1.Dual_150_Engineering_Mornitoring_Show();

            Button_Click_Enable(false);

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
            progressBar_GB.Maximum = (f1.Get_DBV_TrackBar_Maximum()- dbv_end_Point) / Step_Value; //208번 측정(48~255) + 나머지 전환과정 끝나면 1 = 49
            progressBar_GB.Step = 1;
            progressBar_GB.Value = 0;


            //만약 Data 가 씌여져 있다면 , Data 삭제후 Delta E 계산 진행
            //만약 rows < 200 이면 , 처음 계산한다는말 , 즉 Row Add  필요
            //f1.SH_Delta_E3_Measure(gray_end_point, delay_time_between_measurement);
            int Times = f1.Dual_SH_Delta_E2_Measure(dbv_end_Point, delay_time_between_measurement);

            if (Availability)
            {
                if (radioButton_Normal_DE_Calculate.Checked)
                {
                    f1.GB_Status_AppendText_Nextline("Dual Normal Delta E2 Calculation", Color.Blue);
                    //Condition 1
                    double Max_Delta_E = Normal_Delta_E2_Calculate(dataGridView1, dbv_end_Point, Step_Value);
                    textBox_Measured_DeltaE2.Text = Max_Delta_E.ToString().Substring(0, 5);
                    dataGridView1.Rows.Add("Delta E2"); // 한열은 띄어쓰기로
                    dataGridView1.Rows.Add(Max_Delta_E.ToString());

                    //Condition2
                    Max_Delta_E = Normal_Delta_E2_Calculate(dataGridView2, dbv_end_Point, Step_Value);
                    textBox_Measured_DeltaE2_2.Text = Max_Delta_E.ToString().Substring(0, 5);
                    dataGridView2.Rows.Add("Delta E2"); // 한열은 띄어쓰기로
                    dataGridView2.Rows.Add(Max_Delta_E.ToString());
                }
                else if (radioButton_Special_1_DE_Calculate.Checked)
                {
                    f1.GB_Status_AppendText_Nextline("Dual Special 1 Delta E2 Calculation", Color.Blue);
                    double Max_Delta_E = Special_1_Calculate_Delta_E(0, Times - 1);
                    textBox_Measured_DeltaE2.Text = string.Empty;
                    textBox_Measured_DeltaE2_2.Text = Max_Delta_E.ToString().Substring(0, 5);
                    dataGridView2.Rows.Add("Delta E2(Special 1)"); // 한열은 띄어쓰기로
                    dataGridView2.Rows.Add(Max_Delta_E.ToString());
                }
                else
                {
                    //Nothing happens
                }
            }
            progressBar_GB.PerformStep();
            Button_Click_Enable(true);
        }

        private void button_SH_Difference_Measure_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderText = "Gray";
            dataGridView2.Columns[0].HeaderText = "Gray";

            Form1 f1 = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
            if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP086 || f1.current_model.Get_Current_Model_Name() == Model_Name.DP116)
                f1.Dual_Engineering_Mornitoring_Show();
            else if (f1.current_model.Get_Current_Model_Name() == Model_Name.DP150)
                f1.Dual_150_Engineering_Mornitoring_Show();

            Button_Click_Enable(false);

            //measure delay time setting
            int delay_time_after_pattern = Convert.ToInt16(textBox_delay_time.Text);
           
            if (radioButton_SH_Diff_Step_Normal.Checked)
            {
                f1.Dual_SH_Difference_Measure(delay_time_after_pattern);
            }
            else if(radioButton_SH_Diff_Step_4.Checked)
            {
                f1.Dual_SH_Difference_Measure_By_Step(delay_time_after_pattern, 4);
            }
            else if (radioButton_SH_Diff_Step_8.Checked)
            {
                f1.Dual_SH_Difference_Measure_By_Step(delay_time_after_pattern, 8);
            }
            else if (radioButton_SH_Diff_Step_16.Checked)
            {
                f1.Dual_SH_Difference_Measure_By_Step(delay_time_after_pattern, 16);
            }
            else
            {
                //it will not happen
            }
           
            Button_Click_Enable(true);
        }

        























    }
}
