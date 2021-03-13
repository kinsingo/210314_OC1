using BSQH_Csharp_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices; // (Dll 사용위해 필요)

namespace TestAlgorithmForm
{

    public partial class Form1 : Form
    {
        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateMonotoneCubicSpline(int mX_size, int mY_size, double[] mX, double[] mY);

        [DllImport("BSQH_dll_MR_Shin.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double interpolate(int mX_size, int mY_size, double[] mX, double[] mY, double[] mM, double x);

        //X : Gray
        //Y : Vdata

        Point Previouse_Pos;
        int pointIndex;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChartInitializing();
        }

        private void ChartInitializing()
        {
            chart1.Series.Clear();

            chart1.ChartAreas[0].AxisX.Title = "Gray";
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(30,100, 100, 100);
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Interval = 5;

            chart1.ChartAreas[0].AxisY.Title = "Vdata";
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(30, 100, 100, 100);
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chart1.ChartAreas[0].AxisY.Maximum = 6.5;
            chart1.ChartAreas[0].AxisY.Minimum = 3.5;
            chart1.ChartAreas[0].AxisY.Interval = 0.1;

            if (chart1.Series.IsUniqueName("Vdata_Gray_Points")) chart1.Series.Add("Vdata_Gray_Points");
            chart1.Series["Vdata_Gray_Points"].Points.Clear();
            chart1.Series["Vdata_Gray_Points"].ChartType = SeriesChartType.Point;
            chart1.Series["Vdata_Gray_Points"].Color = Color.Black;
            chart1.Series["Vdata_Gray_Points"].MarkerSize = 4;

            Previouse_Pos = new Point(0, 0);
            pointIndex = 0;
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

        private void button_Csharp_Polynomial_Interpolation_Click(object sender, EventArgs e)
        {
            Poly_Interpolation poly_Interpolation = new Poly_Interpolation();

            GrayVdataDictionary.getInstance().Updata_Vdata_Gray_DataSet();
            int[] grays = new int[DP213_Static.Max_Gray_Amount];
            double[] voltages = new double[DP213_Static.Max_Gray_Amount];

            int count = 0;
            double max_voltage = Double.MinValue;
            double min_voltage = Double.MaxValue;
            foreach (KeyValuePair<int, double> keyValue in GrayVdataDictionary.getInstance().keyValuePairs)
            {
                grays[count] = keyValue.Key;
                voltages[count] = keyValue.Value;
                

                if (max_voltage < voltages[count])
                    max_voltage = voltages[count];

                if(min_voltage > voltages[count])
                    min_voltage = voltages[count];

                count++;
            }
            if(radioButton_Voltage_to_Gray.Checked)
            {
                poly_Interpolation.Update_Function_Param(grays, voltages);
                for (double voltage = min_voltage; voltage <= max_voltage; voltage += 0.001)
                {
                    int gray = poly_Interpolation.Polynorminal_Fuction(voltage);
                    chart1.Series["Vdata_Gray_Points"].Points.AddXY(gray, voltage);
                    chart1.Series["Vdata_Gray_Points"].Points[pointIndex++].Color = Color.FromArgb(150,150,0);
                }
            }
            else if(radioButton_Gray_to_Voltage.Checked)
            {
                poly_Interpolation.Update_Function_Param2(grays, voltages);
                for (int gray = 0; gray <= 255; gray++)
                {
                    double voltage = poly_Interpolation.Polynorminal_Fuction2(gray);
                    chart1.Series["Vdata_Gray_Points"].Points.AddXY(gray, voltage);
                    chart1.Series["Vdata_Gray_Points"].Points[pointIndex++].Color = Color.FromArgb(0, 150, 150);
                }
            }
        }


        private void button_Csharp_Monotonic_Cubic_Spline_Interpolation_Click(object sender, EventArgs e)
        {
            GrayVdataDictionary.getInstance().Updata_Vdata_Gray_DataSet();
            double[] grays = new double[DP213_Static.Max_Gray_Amount];
            double[] voltages = new double[DP213_Static.Max_Gray_Amount];

            int count = 0;
            double max_voltage = Double.MinValue;
            double min_voltage = Double.MaxValue;
            foreach (KeyValuePair<int, double> keyValue in GrayVdataDictionary.getInstance().keyValuePairs)
            {
                grays[count] = keyValue.Key;
                voltages[count] = keyValue.Value;

                if (max_voltage < voltages[count])
                    max_voltage = voltages[count];

                if (min_voltage > voltages[count])
                    min_voltage = voltages[count];

                count++;
            }
            if (radioButton_Voltage_to_Gray.Checked)
            {
                SplineInterpolator mono_cubic = SplineInterpolator.createMonotoneCubicSpline(voltages, grays);
                for (double voltage = min_voltage; voltage <= max_voltage; voltage += 0.001)
                {
                    double gray = mono_cubic.interpolate(voltage);
                    chart1.Series["Vdata_Gray_Points"].Points.AddXY(gray, voltage);
                    chart1.Series["Vdata_Gray_Points"].Points[pointIndex++].Color = Color.FromArgb(150, 150, 0);
                }

            }
            else if (radioButton_Gray_to_Voltage.Checked)
            {
                Array.Reverse(grays);
                Array.Reverse(voltages);
                SplineInterpolator mono_cubic = SplineInterpolator.createMonotoneCubicSpline(grays, voltages);

                for (int gray = 0; gray <= 255; gray++)
                {
                    double voltage = mono_cubic.interpolate(gray);
                    chart1.Series["Vdata_Gray_Points"].Points.AddXY(gray, voltage);
                    chart1.Series["Vdata_Gray_Points"].Points[pointIndex++].Color = Color.FromArgb(0, 150, 150);
                }
            }
        }

        private void button_CPP_Polynomial_Interpolation_Click(object sender, EventArgs e)
        {
         
        }

        private void button_CPP_Monotonic_Cubic_Spline_Interpolation_Click(object sender, EventArgs e)
        {
            GrayVdataDictionary.getInstance().Updata_Vdata_Gray_DataSet();
            double[] grays = new double[DP213_Static.Max_Gray_Amount];
            double[] voltages = new double[DP213_Static.Max_Gray_Amount];

            int count = 0;
            double max_voltage = Double.MinValue;
            double min_voltage = Double.MaxValue;
            foreach (KeyValuePair<int, double> keyValue in GrayVdataDictionary.getInstance().keyValuePairs)
            {
                grays[count] = keyValue.Key;
                voltages[count] = keyValue.Value;

                if (max_voltage < voltages[count])
                    max_voltage = voltages[count];

                if (min_voltage > voltages[count])
                    min_voltage = voltages[count];

                count++;
            }
            if (radioButton_Voltage_to_Gray.Checked)
            {
                IntPtr temp = CreateMonotoneCubicSpline(voltages.Length, grays.Length, voltages, grays);
                double[] mM = new double[voltages.Length];
                Marshal.Copy(temp, mM, 0, mM.Length);

                for (double voltage = min_voltage; voltage <= max_voltage; voltage += 0.001)
                {
                    double gray = interpolate(voltages.Length, grays.Length, voltages, grays, mM, voltage);
                    chart1.Series["Vdata_Gray_Points"].Points.AddXY(gray, voltage);
                    chart1.Series["Vdata_Gray_Points"].Points[pointIndex++].Color = Color.FromArgb(100, 100, 200);
                }

            }
            else if (radioButton_Gray_to_Voltage.Checked)
            {
                Array.Reverse(grays);
                Array.Reverse(voltages);

                IntPtr temp = CreateMonotoneCubicSpline(grays.Length, voltages.Length, grays, voltages);
                double[] mM = new double[grays.Length];
                Marshal.Copy(temp, mM, 0, mM.Length);

                for (int gray = 0; gray <= 255; gray++)
                {
                    double voltage = interpolate(grays.Length, voltages.Length, grays, voltages, mM, gray);
                    chart1.Series["Vdata_Gray_Points"].Points.AddXY(gray, voltage);
                    chart1.Series["Vdata_Gray_Points"].Points[pointIndex++].Color = Color.FromArgb(200, 100, 100);
                }
            }
        }

        private void button_Chart_Clear_Click(object sender, EventArgs e)
        {
            pointIndex = 0;
            chart1.Series["Vdata_Gray_Points"].Points.Clear();
        }

        private void button_display_point_data_Click(object sender, EventArgs e)
        {
            GrayVdataDictionary.getInstance().Updata_Vdata_Gray_DataSet();
            
            foreach (KeyValuePair<int, double> keyValue in GrayVdataDictionary.getInstance().keyValuePairs)
            {
                chart1.Series["Vdata_Gray_Points"].Points.AddXY(keyValue.Key, keyValue.Value);
                chart1.Series["Vdata_Gray_Points"].Points[pointIndex++].Color = GrayVdataDictionary.getInstance().Get_Color();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button_Adjust_Yscale_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox_Y_Scale_Max.Text);
            chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox_Y_Scale_Min.Text);
        }
    }
}
