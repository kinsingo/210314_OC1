namespace TestAlgorithmForm
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Adjust_Yscale = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton_B = new System.Windows.Forms.RadioButton();
            this.radioButton_G = new System.Windows.Forms.RadioButton();
            this.radioButton_R = new System.Windows.Forms.RadioButton();
            this.textBox_Y_Scale_Max = new System.Windows.Forms.TextBox();
            this.radioButton_DataSet6 = new System.Windows.Forms.RadioButton();
            this.textBox_Y_Scale_Min = new System.Windows.Forms.TextBox();
            this.radioButton_DataSet5 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton_DataSet4 = new System.Windows.Forms.RadioButton();
            this.radioButton_DataSet3 = new System.Windows.Forms.RadioButton();
            this.radioButton_DataSet2 = new System.Windows.Forms.RadioButton();
            this.radioButton_DataSet1 = new System.Windows.Forms.RadioButton();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_Csharp_Monotonic_Cubic_Spline_Interpolation = new System.Windows.Forms.Button();
            this.button_Csharp_Polynomial_Interpolation = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_CPP_Monotonic_Cubic_Spline_Interpolation = new System.Windows.Forms.Button();
            this.button_CPP_Polynomial_Interpolation = new System.Windows.Forms.Button();
            this.button_Chart_Clear = new System.Windows.Forms.Button();
            this.button_display_point_data = new System.Windows.Forms.Button();
            this.radioButton_Gray_to_Voltage = new System.Windows.Forms.RadioButton();
            this.radioButton_Voltage_to_Gray = new System.Windows.Forms.RadioButton();
            this.radioButton_Many_Points = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_Many_Points);
            this.groupBox1.Controls.Add(this.button_Adjust_Yscale);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.textBox_Y_Scale_Max);
            this.groupBox1.Controls.Add(this.radioButton_DataSet6);
            this.groupBox1.Controls.Add(this.textBox_Y_Scale_Min);
            this.groupBox1.Controls.Add(this.radioButton_DataSet5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButton_DataSet4);
            this.groupBox1.Controls.Add(this.radioButton_DataSet3);
            this.groupBox1.Controls.Add(this.radioButton_DataSet2);
            this.groupBox1.Controls.Add(this.radioButton_DataSet1);
            this.groupBox1.Location = new System.Drawing.Point(1254, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 290);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DataSet";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // button_Adjust_Yscale
            // 
            this.button_Adjust_Yscale.Location = new System.Drawing.Point(81, 256);
            this.button_Adjust_Yscale.Name = "button_Adjust_Yscale";
            this.button_Adjust_Yscale.Size = new System.Drawing.Size(127, 27);
            this.button_Adjust_Yscale.TabIndex = 9;
            this.button_Adjust_Yscale.Text = "Adjust Y Scale";
            this.button_Adjust_Yscale.UseVisualStyleBackColor = true;
            this.button_Adjust_Yscale.Click += new System.EventHandler(this.button_Adjust_Yscale_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton_B);
            this.groupBox4.Controls.Add(this.radioButton_G);
            this.groupBox4.Controls.Add(this.radioButton_R);
            this.groupBox4.Location = new System.Drawing.Point(7, 196);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(68, 88);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "DataSet";
            // 
            // radioButton_B
            // 
            this.radioButton_B.AutoSize = true;
            this.radioButton_B.Location = new System.Drawing.Point(6, 57);
            this.radioButton_B.Name = "radioButton_B";
            this.radioButton_B.Size = new System.Drawing.Size(32, 17);
            this.radioButton_B.TabIndex = 2;
            this.radioButton_B.Text = "B";
            this.radioButton_B.UseVisualStyleBackColor = true;
            // 
            // radioButton_G
            // 
            this.radioButton_G.AutoSize = true;
            this.radioButton_G.Location = new System.Drawing.Point(6, 38);
            this.radioButton_G.Name = "radioButton_G";
            this.radioButton_G.Size = new System.Drawing.Size(33, 17);
            this.radioButton_G.TabIndex = 1;
            this.radioButton_G.Text = "G";
            this.radioButton_G.UseVisualStyleBackColor = true;
            // 
            // radioButton_R
            // 
            this.radioButton_R.AutoSize = true;
            this.radioButton_R.Checked = true;
            this.radioButton_R.Location = new System.Drawing.Point(6, 19);
            this.radioButton_R.Name = "radioButton_R";
            this.radioButton_R.Size = new System.Drawing.Size(33, 17);
            this.radioButton_R.TabIndex = 0;
            this.radioButton_R.TabStop = true;
            this.radioButton_R.Text = "R";
            this.radioButton_R.UseVisualStyleBackColor = true;
            // 
            // textBox_Y_Scale_Max
            // 
            this.textBox_Y_Scale_Max.Location = new System.Drawing.Point(170, 230);
            this.textBox_Y_Scale_Max.Name = "textBox_Y_Scale_Max";
            this.textBox_Y_Scale_Max.Size = new System.Drawing.Size(38, 20);
            this.textBox_Y_Scale_Max.TabIndex = 8;
            this.textBox_Y_Scale_Max.Text = "6.5";
            this.textBox_Y_Scale_Max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radioButton_DataSet6
            // 
            this.radioButton_DataSet6.AutoSize = true;
            this.radioButton_DataSet6.Location = new System.Drawing.Point(6, 114);
            this.radioButton_DataSet6.Name = "radioButton_DataSet6";
            this.radioButton_DataSet6.Size = new System.Drawing.Size(177, 17);
            this.radioButton_DataSet6.TabIndex = 5;
            this.radioButton_DataSet6.Text = "DataSet6 (DP213 Set2 Band10)";
            this.radioButton_DataSet6.UseVisualStyleBackColor = true;
            // 
            // textBox_Y_Scale_Min
            // 
            this.textBox_Y_Scale_Min.Location = new System.Drawing.Point(129, 230);
            this.textBox_Y_Scale_Min.Name = "textBox_Y_Scale_Min";
            this.textBox_Y_Scale_Min.Size = new System.Drawing.Size(39, 20);
            this.textBox_Y_Scale_Min.TabIndex = 7;
            this.textBox_Y_Scale_Min.Text = "3.5";
            this.textBox_Y_Scale_Min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radioButton_DataSet5
            // 
            this.radioButton_DataSet5.AutoSize = true;
            this.radioButton_DataSet5.Location = new System.Drawing.Point(6, 95);
            this.radioButton_DataSet5.Name = "radioButton_DataSet5";
            this.radioButton_DataSet5.Size = new System.Drawing.Size(171, 17);
            this.radioButton_DataSet5.TabIndex = 4;
            this.radioButton_DataSet5.Text = "DataSet5 (DP213 Set2 Band8)";
            this.radioButton_DataSet5.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Y Scale : ";
            // 
            // radioButton_DataSet4
            // 
            this.radioButton_DataSet4.AutoSize = true;
            this.radioButton_DataSet4.Location = new System.Drawing.Point(6, 76);
            this.radioButton_DataSet4.Name = "radioButton_DataSet4";
            this.radioButton_DataSet4.Size = new System.Drawing.Size(171, 17);
            this.radioButton_DataSet4.TabIndex = 3;
            this.radioButton_DataSet4.Text = "DataSet4 (DP213 Set2 Band6)";
            this.radioButton_DataSet4.UseVisualStyleBackColor = true;
            // 
            // radioButton_DataSet3
            // 
            this.radioButton_DataSet3.AutoSize = true;
            this.radioButton_DataSet3.Location = new System.Drawing.Point(6, 57);
            this.radioButton_DataSet3.Name = "radioButton_DataSet3";
            this.radioButton_DataSet3.Size = new System.Drawing.Size(171, 17);
            this.radioButton_DataSet3.TabIndex = 2;
            this.radioButton_DataSet3.Text = "DataSet3 (DP213 Set2 Band4)";
            this.radioButton_DataSet3.UseVisualStyleBackColor = true;
            // 
            // radioButton_DataSet2
            // 
            this.radioButton_DataSet2.AutoSize = true;
            this.radioButton_DataSet2.Location = new System.Drawing.Point(6, 38);
            this.radioButton_DataSet2.Name = "radioButton_DataSet2";
            this.radioButton_DataSet2.Size = new System.Drawing.Size(171, 17);
            this.radioButton_DataSet2.TabIndex = 1;
            this.radioButton_DataSet2.Text = "DataSet2 (DP213 Set2 Band2)";
            this.radioButton_DataSet2.UseVisualStyleBackColor = true;
            // 
            // radioButton_DataSet1
            // 
            this.radioButton_DataSet1.AutoSize = true;
            this.radioButton_DataSet1.Checked = true;
            this.radioButton_DataSet1.Location = new System.Drawing.Point(6, 19);
            this.radioButton_DataSet1.Name = "radioButton_DataSet1";
            this.radioButton_DataSet1.Size = new System.Drawing.Size(171, 17);
            this.radioButton_DataSet1.TabIndex = 0;
            this.radioButton_DataSet1.TabStop = true;
            this.radioButton_DataSet1.Text = "DataSet1 (DP213 Set2 Band0)";
            this.radioButton_DataSet1.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 12);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1236, 837);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_Csharp_Monotonic_Cubic_Spline_Interpolation);
            this.groupBox2.Controls.Add(this.button_Csharp_Polynomial_Interpolation);
            this.groupBox2.Location = new System.Drawing.Point(1254, 463);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 144);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "C# Implementation ";
            // 
            // button_Csharp_Monotonic_Cubic_Spline_Interpolation
            // 
            this.button_Csharp_Monotonic_Cubic_Spline_Interpolation.Location = new System.Drawing.Point(6, 78);
            this.button_Csharp_Monotonic_Cubic_Spline_Interpolation.Name = "button_Csharp_Monotonic_Cubic_Spline_Interpolation";
            this.button_Csharp_Monotonic_Cubic_Spline_Interpolation.Size = new System.Drawing.Size(205, 53);
            this.button_Csharp_Monotonic_Cubic_Spline_Interpolation.TabIndex = 2;
            this.button_Csharp_Monotonic_Cubic_Spline_Interpolation.Text = "Monotonic Cubic Spline Interpolation";
            this.button_Csharp_Monotonic_Cubic_Spline_Interpolation.UseVisualStyleBackColor = true;
            this.button_Csharp_Monotonic_Cubic_Spline_Interpolation.Click += new System.EventHandler(this.button_Csharp_Monotonic_Cubic_Spline_Interpolation_Click);
            // 
            // button_Csharp_Polynomial_Interpolation
            // 
            this.button_Csharp_Polynomial_Interpolation.Location = new System.Drawing.Point(6, 19);
            this.button_Csharp_Polynomial_Interpolation.Name = "button_Csharp_Polynomial_Interpolation";
            this.button_Csharp_Polynomial_Interpolation.Size = new System.Drawing.Size(205, 53);
            this.button_Csharp_Polynomial_Interpolation.TabIndex = 0;
            this.button_Csharp_Polynomial_Interpolation.Text = "N-Polynomial Interpolation";
            this.button_Csharp_Polynomial_Interpolation.UseVisualStyleBackColor = true;
            this.button_Csharp_Polynomial_Interpolation.Click += new System.EventHandler(this.button_Csharp_Polynomial_Interpolation_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_CPP_Monotonic_Cubic_Spline_Interpolation);
            this.groupBox3.Controls.Add(this.button_CPP_Polynomial_Interpolation);
            this.groupBox3.Location = new System.Drawing.Point(1254, 658);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(218, 191);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "C++ Implementation";
            // 
            // button_CPP_Monotonic_Cubic_Spline_Interpolation
            // 
            this.button_CPP_Monotonic_Cubic_Spline_Interpolation.Location = new System.Drawing.Point(7, 78);
            this.button_CPP_Monotonic_Cubic_Spline_Interpolation.Name = "button_CPP_Monotonic_Cubic_Spline_Interpolation";
            this.button_CPP_Monotonic_Cubic_Spline_Interpolation.Size = new System.Drawing.Size(205, 53);
            this.button_CPP_Monotonic_Cubic_Spline_Interpolation.TabIndex = 2;
            this.button_CPP_Monotonic_Cubic_Spline_Interpolation.Text = "Monotonic Cubic Spline Interpolation";
            this.button_CPP_Monotonic_Cubic_Spline_Interpolation.UseVisualStyleBackColor = true;
            this.button_CPP_Monotonic_Cubic_Spline_Interpolation.Click += new System.EventHandler(this.button_CPP_Monotonic_Cubic_Spline_Interpolation_Click);
            // 
            // button_CPP_Polynomial_Interpolation
            // 
            this.button_CPP_Polynomial_Interpolation.Location = new System.Drawing.Point(6, 19);
            this.button_CPP_Polynomial_Interpolation.Name = "button_CPP_Polynomial_Interpolation";
            this.button_CPP_Polynomial_Interpolation.Size = new System.Drawing.Size(205, 53);
            this.button_CPP_Polynomial_Interpolation.TabIndex = 0;
            this.button_CPP_Polynomial_Interpolation.Text = "N-Polynomial Interpolation";
            this.button_CPP_Polynomial_Interpolation.UseVisualStyleBackColor = true;
            this.button_CPP_Polynomial_Interpolation.Click += new System.EventHandler(this.button_CPP_Polynomial_Interpolation_Click);
            // 
            // button_Chart_Clear
            // 
            this.button_Chart_Clear.Location = new System.Drawing.Point(1254, 308);
            this.button_Chart_Clear.Name = "button_Chart_Clear";
            this.button_Chart_Clear.Size = new System.Drawing.Size(108, 43);
            this.button_Chart_Clear.TabIndex = 4;
            this.button_Chart_Clear.Text = "Chart Clear";
            this.button_Chart_Clear.UseVisualStyleBackColor = true;
            this.button_Chart_Clear.Click += new System.EventHandler(this.button_Chart_Clear_Click);
            // 
            // button_display_point_data
            // 
            this.button_display_point_data.Location = new System.Drawing.Point(1364, 308);
            this.button_display_point_data.Name = "button_display_point_data";
            this.button_display_point_data.Size = new System.Drawing.Size(108, 43);
            this.button_display_point_data.TabIndex = 5;
            this.button_display_point_data.Text = "Display Point Data";
            this.button_display_point_data.UseVisualStyleBackColor = true;
            this.button_display_point_data.Click += new System.EventHandler(this.button_display_point_data_Click);
            // 
            // radioButton_Gray_to_Voltage
            // 
            this.radioButton_Gray_to_Voltage.AutoSize = true;
            this.radioButton_Gray_to_Voltage.Checked = true;
            this.radioButton_Gray_to_Voltage.Location = new System.Drawing.Point(1260, 421);
            this.radioButton_Gray_to_Voltage.Name = "radioButton_Gray_to_Voltage";
            this.radioButton_Gray_to_Voltage.Size = new System.Drawing.Size(98, 17);
            this.radioButton_Gray_to_Voltage.TabIndex = 3;
            this.radioButton_Gray_to_Voltage.TabStop = true;
            this.radioButton_Gray_to_Voltage.Text = "Gray to Voltage";
            this.radioButton_Gray_to_Voltage.UseVisualStyleBackColor = true;
            // 
            // radioButton_Voltage_to_Gray
            // 
            this.radioButton_Voltage_to_Gray.AutoSize = true;
            this.radioButton_Voltage_to_Gray.Location = new System.Drawing.Point(1260, 440);
            this.radioButton_Voltage_to_Gray.Name = "radioButton_Voltage_to_Gray";
            this.radioButton_Voltage_to_Gray.Size = new System.Drawing.Size(98, 17);
            this.radioButton_Voltage_to_Gray.TabIndex = 4;
            this.radioButton_Voltage_to_Gray.Text = "Voltage to Gray";
            this.radioButton_Voltage_to_Gray.UseVisualStyleBackColor = true;
            // 
            // radioButton_Many_Points
            // 
            this.radioButton_Many_Points.AutoSize = true;
            this.radioButton_Many_Points.Location = new System.Drawing.Point(6, 134);
            this.radioButton_Many_Points.Name = "radioButton_Many_Points";
            this.radioButton_Many_Points.Size = new System.Drawing.Size(83, 17);
            this.radioButton_Many_Points.TabIndex = 10;
            this.radioButton_Many_Points.Text = "Many Points";
            this.radioButton_Many_Points.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1484, 861);
            this.Controls.Add(this.radioButton_Voltage_to_Gray);
            this.Controls.Add(this.button_display_point_data);
            this.Controls.Add(this.radioButton_Gray_to_Voltage);
            this.Controls.Add(this.button_Chart_Clear);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_Csharp_Monotonic_Cubic_Spline_Interpolation;
        private System.Windows.Forms.Button button_Csharp_Polynomial_Interpolation;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_CPP_Monotonic_Cubic_Spline_Interpolation;
        private System.Windows.Forms.Button button_CPP_Polynomial_Interpolation;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_Chart_Clear;
        public System.Windows.Forms.RadioButton radioButton_DataSet5;
        public System.Windows.Forms.RadioButton radioButton_DataSet4;
        public System.Windows.Forms.RadioButton radioButton_DataSet3;
        public System.Windows.Forms.RadioButton radioButton_DataSet2;
        public System.Windows.Forms.RadioButton radioButton_DataSet1;
        public System.Windows.Forms.RadioButton radioButton_DataSet6;
        public System.Windows.Forms.RadioButton radioButton_B;
        public System.Windows.Forms.RadioButton radioButton_G;
        public System.Windows.Forms.RadioButton radioButton_R;
        private System.Windows.Forms.Button button_display_point_data;
        private System.Windows.Forms.RadioButton radioButton_Gray_to_Voltage;
        private System.Windows.Forms.RadioButton radioButton_Voltage_to_Gray;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Y_Scale_Min;
        private System.Windows.Forms.TextBox textBox_Y_Scale_Max;
        private System.Windows.Forms.Button button_Adjust_Yscale;
        public System.Windows.Forms.RadioButton radioButton_Many_Points;
    }
}

