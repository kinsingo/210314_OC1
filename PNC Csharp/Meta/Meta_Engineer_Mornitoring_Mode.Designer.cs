namespace PNC_Csharp
{
    partial class Meta_Engineer_Mornitoring_Mode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button_Hide = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_Diff_Hide = new System.Windows.Forms.RadioButton();
            this.radioButton_Diff_Show = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_Limit_Lv_Ratio_150 = new System.Windows.Forms.RadioButton();
            this.radioButton_Limit_Lv_Ratio_60 = new System.Windows.Forms.RadioButton();
            this.radioButton_Limit_Lv_Ratio_80 = new System.Windows.Forms.RadioButton();
            this.radioButton_Limit_Lv_Ratio_100 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_Limit_Ratio_150 = new System.Windows.Forms.RadioButton();
            this.radioButton_Limit_Ratio_60 = new System.Windows.Forms.RadioButton();
            this.radioButton_Limit_Ratio_80 = new System.Windows.Forms.RadioButton();
            this.radioButton_Limit_Ratio_100 = new System.Windows.Forms.RadioButton();
            this.button_Read_OC_Param_From_Excel_File = new System.Windows.Forms.Button();
            this.groupBox_Band_Selection = new System.Windows.Forms.GroupBox();
            this.radiobutton_Band9 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band8 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band7 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band6 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band5 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band4 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band3 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band2 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band1 = new System.Windows.Forms.RadioButton();
            this.radiobutton_Band0 = new System.Windows.Forms.RadioButton();
            this.dataGridView_Band_OC_Viewer = new System.Windows.Forms.DataGridView();
            this.dataGridView_OC_param = new System.Windows.Forms.DataGridView();
            this.dataGridView_Gamma_Vreg1_Diff = new System.Windows.Forms.DataGridView();
            this.dataGridView_RGB_Vdata = new System.Windows.Forms.DataGridView();
            this.button_Calculate_Data_Voltage = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox_Get_HBM_Equation_By_Dll = new System.Windows.Forms.CheckBox();
            this.checkBox_Get_RGB_Equation_By_Dll = new System.Windows.Forms.CheckBox();
            this.checkBox_Get_RGB_Equation = new System.Windows.Forms.CheckBox();
            this.checkBox_Get_All_Equation = new System.Windows.Forms.CheckBox();
            this.checkBox_Get_HBM_Equation = new System.Windows.Forms.CheckBox();
            this.button_Test = new System.Windows.Forms.Button();
            this.button_Real_RGBVreg1_and_Get_Real_Vdata = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox_Band_Selection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Band_OC_Viewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_OC_param)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Gamma_Vreg1_Diff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_RGB_Vdata)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Hide
            // 
            this.button_Hide.Location = new System.Drawing.Point(890, 777);
            this.button_Hide.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Hide.Name = "button_Hide";
            this.button_Hide.Size = new System.Drawing.Size(108, 28);
            this.button_Hide.TabIndex = 2;
            this.button_Hide.Text = "Hide";
            this.button_Hide.UseVisualStyleBackColor = true;
            this.button_Hide.Click += new System.EventHandler(this.button_Hide_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_Diff_Hide);
            this.groupBox3.Controls.Add(this.radioButton_Diff_Show);
            this.groupBox3.Location = new System.Drawing.Point(890, 210);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox3.Size = new System.Drawing.Size(107, 51);
            this.groupBox3.TabIndex = 283;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "R/G/B/Vreg1 Diff From Init Values";
            // 
            // radioButton_Diff_Hide
            // 
            this.radioButton_Diff_Hide.AutoSize = true;
            this.radioButton_Diff_Hide.Location = new System.Drawing.Point(58, 28);
            this.radioButton_Diff_Hide.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Diff_Hide.Name = "radioButton_Diff_Hide";
            this.radioButton_Diff_Hide.Size = new System.Drawing.Size(47, 17);
            this.radioButton_Diff_Hide.TabIndex = 12;
            this.radioButton_Diff_Hide.Text = "Hide";
            this.radioButton_Diff_Hide.UseVisualStyleBackColor = true;
            this.radioButton_Diff_Hide.CheckedChanged += new System.EventHandler(this.radioButton_Diff_Hide_CheckedChanged);
            // 
            // radioButton_Diff_Show
            // 
            this.radioButton_Diff_Show.AutoSize = true;
            this.radioButton_Diff_Show.Checked = true;
            this.radioButton_Diff_Show.Location = new System.Drawing.Point(6, 28);
            this.radioButton_Diff_Show.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Diff_Show.Name = "radioButton_Diff_Show";
            this.radioButton_Diff_Show.Size = new System.Drawing.Size(52, 17);
            this.radioButton_Diff_Show.TabIndex = 12;
            this.radioButton_Diff_Show.TabStop = true;
            this.radioButton_Diff_Show.Text = "Show";
            this.radioButton_Diff_Show.UseVisualStyleBackColor = true;
            this.radioButton_Diff_Show.CheckedChanged += new System.EventHandler(this.radioButton_Diff_Show_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_Limit_Lv_Ratio_150);
            this.groupBox2.Controls.Add(this.radioButton_Limit_Lv_Ratio_60);
            this.groupBox2.Controls.Add(this.radioButton_Limit_Lv_Ratio_80);
            this.groupBox2.Controls.Add(this.radioButton_Limit_Lv_Ratio_100);
            this.groupBox2.Location = new System.Drawing.Point(890, 106);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Size = new System.Drawing.Size(108, 102);
            this.groupBox2.TabIndex = 282;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Lv Limit Selec";
            // 
            // radioButton_Limit_Lv_Ratio_150
            // 
            this.radioButton_Limit_Lv_Ratio_150.AutoSize = true;
            this.radioButton_Limit_Lv_Ratio_150.Location = new System.Drawing.Point(6, 22);
            this.radioButton_Limit_Lv_Ratio_150.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Limit_Lv_Ratio_150.Name = "radioButton_Limit_Lv_Ratio_150";
            this.radioButton_Limit_Lv_Ratio_150.Size = new System.Drawing.Size(89, 17);
            this.radioButton_Limit_Lv_Ratio_150.TabIndex = 15;
            this.radioButton_Limit_Lv_Ratio_150.Text = "Limit(Lv) * 1.5";
            this.radioButton_Limit_Lv_Ratio_150.UseVisualStyleBackColor = true;
            this.radioButton_Limit_Lv_Ratio_150.CheckedChanged += new System.EventHandler(this.radioButton_Limit_Lv_Ratio_150_CheckedChanged);
            // 
            // radioButton_Limit_Lv_Ratio_60
            // 
            this.radioButton_Limit_Lv_Ratio_60.AutoSize = true;
            this.radioButton_Limit_Lv_Ratio_60.Location = new System.Drawing.Point(6, 80);
            this.radioButton_Limit_Lv_Ratio_60.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Limit_Lv_Ratio_60.Name = "radioButton_Limit_Lv_Ratio_60";
            this.radioButton_Limit_Lv_Ratio_60.Size = new System.Drawing.Size(89, 17);
            this.radioButton_Limit_Lv_Ratio_60.TabIndex = 14;
            this.radioButton_Limit_Lv_Ratio_60.Text = "Limit(Lv) * 0.6";
            this.radioButton_Limit_Lv_Ratio_60.UseVisualStyleBackColor = true;
            this.radioButton_Limit_Lv_Ratio_60.CheckedChanged += new System.EventHandler(this.radioButton_Limit_Lv_Ratio_60_CheckedChanged);
            // 
            // radioButton_Limit_Lv_Ratio_80
            // 
            this.radioButton_Limit_Lv_Ratio_80.AutoSize = true;
            this.radioButton_Limit_Lv_Ratio_80.Location = new System.Drawing.Point(6, 60);
            this.radioButton_Limit_Lv_Ratio_80.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Limit_Lv_Ratio_80.Name = "radioButton_Limit_Lv_Ratio_80";
            this.radioButton_Limit_Lv_Ratio_80.Size = new System.Drawing.Size(89, 17);
            this.radioButton_Limit_Lv_Ratio_80.TabIndex = 13;
            this.radioButton_Limit_Lv_Ratio_80.Text = "Limit(Lv) * 0.8";
            this.radioButton_Limit_Lv_Ratio_80.UseVisualStyleBackColor = true;
            this.radioButton_Limit_Lv_Ratio_80.CheckedChanged += new System.EventHandler(this.radioButton_Limit_Lv_Ratio_80_CheckedChanged);
            // 
            // radioButton_Limit_Lv_Ratio_100
            // 
            this.radioButton_Limit_Lv_Ratio_100.AutoSize = true;
            this.radioButton_Limit_Lv_Ratio_100.Checked = true;
            this.radioButton_Limit_Lv_Ratio_100.Location = new System.Drawing.Point(6, 41);
            this.radioButton_Limit_Lv_Ratio_100.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Limit_Lv_Ratio_100.Name = "radioButton_Limit_Lv_Ratio_100";
            this.radioButton_Limit_Lv_Ratio_100.Size = new System.Drawing.Size(80, 17);
            this.radioButton_Limit_Lv_Ratio_100.TabIndex = 12;
            this.radioButton_Limit_Lv_Ratio_100.TabStop = true;
            this.radioButton_Limit_Lv_Ratio_100.Text = "Limit(Lv) * 1";
            this.radioButton_Limit_Lv_Ratio_100.UseVisualStyleBackColor = true;
            this.radioButton_Limit_Lv_Ratio_100.CheckedChanged += new System.EventHandler(this.radioButton_Limit_Lv_Ratio_100_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_Limit_Ratio_150);
            this.groupBox1.Controls.Add(this.radioButton_Limit_Ratio_60);
            this.groupBox1.Controls.Add(this.radioButton_Limit_Ratio_80);
            this.groupBox1.Controls.Add(this.radioButton_Limit_Ratio_100);
            this.groupBox1.Location = new System.Drawing.Point(890, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(108, 102);
            this.groupBox1.TabIndex = 281;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "xy Limit Selec";
            // 
            // radioButton_Limit_Ratio_150
            // 
            this.radioButton_Limit_Ratio_150.AutoSize = true;
            this.radioButton_Limit_Ratio_150.Location = new System.Drawing.Point(6, 22);
            this.radioButton_Limit_Ratio_150.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Limit_Ratio_150.Name = "radioButton_Limit_Ratio_150";
            this.radioButton_Limit_Ratio_150.Size = new System.Drawing.Size(87, 17);
            this.radioButton_Limit_Ratio_150.TabIndex = 15;
            this.radioButton_Limit_Ratio_150.Text = "Limit(xy) * 1.5";
            this.radioButton_Limit_Ratio_150.UseVisualStyleBackColor = true;
            this.radioButton_Limit_Ratio_150.CheckedChanged += new System.EventHandler(this.radioButton_Limit_Ratio_150_CheckedChanged);
            // 
            // radioButton_Limit_Ratio_60
            // 
            this.radioButton_Limit_Ratio_60.AutoSize = true;
            this.radioButton_Limit_Ratio_60.Location = new System.Drawing.Point(6, 80);
            this.radioButton_Limit_Ratio_60.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Limit_Ratio_60.Name = "radioButton_Limit_Ratio_60";
            this.radioButton_Limit_Ratio_60.Size = new System.Drawing.Size(87, 17);
            this.radioButton_Limit_Ratio_60.TabIndex = 14;
            this.radioButton_Limit_Ratio_60.Text = "Limit(xy) * 0.6";
            this.radioButton_Limit_Ratio_60.UseVisualStyleBackColor = true;
            this.radioButton_Limit_Ratio_60.CheckedChanged += new System.EventHandler(this.radioButton_Limit_Ratio_60_CheckedChanged);
            // 
            // radioButton_Limit_Ratio_80
            // 
            this.radioButton_Limit_Ratio_80.AutoSize = true;
            this.radioButton_Limit_Ratio_80.Location = new System.Drawing.Point(6, 60);
            this.radioButton_Limit_Ratio_80.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Limit_Ratio_80.Name = "radioButton_Limit_Ratio_80";
            this.radioButton_Limit_Ratio_80.Size = new System.Drawing.Size(87, 17);
            this.radioButton_Limit_Ratio_80.TabIndex = 13;
            this.radioButton_Limit_Ratio_80.Text = "Limit(xy) * 0.8";
            this.radioButton_Limit_Ratio_80.UseVisualStyleBackColor = true;
            this.radioButton_Limit_Ratio_80.CheckedChanged += new System.EventHandler(this.radioButton_Limit_Ratio_80_CheckedChanged);
            // 
            // radioButton_Limit_Ratio_100
            // 
            this.radioButton_Limit_Ratio_100.AutoSize = true;
            this.radioButton_Limit_Ratio_100.Checked = true;
            this.radioButton_Limit_Ratio_100.Location = new System.Drawing.Point(6, 41);
            this.radioButton_Limit_Ratio_100.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Limit_Ratio_100.Name = "radioButton_Limit_Ratio_100";
            this.radioButton_Limit_Ratio_100.Size = new System.Drawing.Size(78, 17);
            this.radioButton_Limit_Ratio_100.TabIndex = 12;
            this.radioButton_Limit_Ratio_100.TabStop = true;
            this.radioButton_Limit_Ratio_100.Text = "Limit(xy) * 1";
            this.radioButton_Limit_Ratio_100.UseVisualStyleBackColor = true;
            this.radioButton_Limit_Ratio_100.CheckedChanged += new System.EventHandler(this.radioButton_Limit_Ratio_100_CheckedChanged);
            // 
            // button_Read_OC_Param_From_Excel_File
            // 
            this.button_Read_OC_Param_From_Excel_File.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_Read_OC_Param_From_Excel_File.ForeColor = System.Drawing.Color.White;
            this.button_Read_OC_Param_From_Excel_File.Location = new System.Drawing.Point(890, 520);
            this.button_Read_OC_Param_From_Excel_File.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Read_OC_Param_From_Excel_File.Name = "button_Read_OC_Param_From_Excel_File";
            this.button_Read_OC_Param_From_Excel_File.Size = new System.Drawing.Size(108, 36);
            this.button_Read_OC_Param_From_Excel_File.TabIndex = 280;
            this.button_Read_OC_Param_From_Excel_File.Text = "Read OC Param From csv File";
            this.button_Read_OC_Param_From_Excel_File.UseVisualStyleBackColor = false;
            this.button_Read_OC_Param_From_Excel_File.Click += new System.EventHandler(this.button_Read_OC_Param_From_Excel_File_Click);
            // 
            // groupBox_Band_Selection
            // 
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band9);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band8);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band7);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band6);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band5);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band4);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band3);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band2);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band1);
            this.groupBox_Band_Selection.Controls.Add(this.radiobutton_Band0);
            this.groupBox_Band_Selection.Location = new System.Drawing.Point(890, 268);
            this.groupBox_Band_Selection.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox_Band_Selection.Name = "groupBox_Band_Selection";
            this.groupBox_Band_Selection.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox_Band_Selection.Size = new System.Drawing.Size(108, 249);
            this.groupBox_Band_Selection.TabIndex = 279;
            this.groupBox_Band_Selection.TabStop = false;
            this.groupBox_Band_Selection.Text = "Band Sel";
            // 
            // radiobutton_Band9
            // 
            this.radiobutton_Band9.AutoSize = true;
            this.radiobutton_Band9.Location = new System.Drawing.Point(13, 227);
            this.radiobutton_Band9.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band9.Name = "radiobutton_Band9";
            this.radiobutton_Band9.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band9.TabIndex = 9;
            this.radiobutton_Band9.Text = "Band 9";
            this.radiobutton_Band9.UseVisualStyleBackColor = true;
            this.radiobutton_Band9.CheckedChanged += new System.EventHandler(this.radiobutton_Band9_CheckedChanged);
            // 
            // radiobutton_Band8
            // 
            this.radiobutton_Band8.AutoSize = true;
            this.radiobutton_Band8.Location = new System.Drawing.Point(13, 204);
            this.radiobutton_Band8.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band8.Name = "radiobutton_Band8";
            this.radiobutton_Band8.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band8.TabIndex = 8;
            this.radiobutton_Band8.Text = "Band 8";
            this.radiobutton_Band8.UseVisualStyleBackColor = true;
            this.radiobutton_Band8.CheckedChanged += new System.EventHandler(this.radiobutton_Band8_CheckedChanged);
            // 
            // radiobutton_Band7
            // 
            this.radiobutton_Band7.AutoSize = true;
            this.radiobutton_Band7.Location = new System.Drawing.Point(13, 180);
            this.radiobutton_Band7.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band7.Name = "radiobutton_Band7";
            this.radiobutton_Band7.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band7.TabIndex = 7;
            this.radiobutton_Band7.Text = "Band 7";
            this.radiobutton_Band7.UseVisualStyleBackColor = true;
            this.radiobutton_Band7.CheckedChanged += new System.EventHandler(this.radiobutton_Band7_CheckedChanged);
            // 
            // radiobutton_Band6
            // 
            this.radiobutton_Band6.AutoSize = true;
            this.radiobutton_Band6.Location = new System.Drawing.Point(13, 157);
            this.radiobutton_Band6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band6.Name = "radiobutton_Band6";
            this.radiobutton_Band6.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band6.TabIndex = 6;
            this.radiobutton_Band6.Text = "Band 6";
            this.radiobutton_Band6.UseVisualStyleBackColor = true;
            this.radiobutton_Band6.CheckedChanged += new System.EventHandler(this.radiobutton_Band6_CheckedChanged);
            // 
            // radiobutton_Band5
            // 
            this.radiobutton_Band5.AutoSize = true;
            this.radiobutton_Band5.Location = new System.Drawing.Point(13, 134);
            this.radiobutton_Band5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band5.Name = "radiobutton_Band5";
            this.radiobutton_Band5.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band5.TabIndex = 5;
            this.radiobutton_Band5.Text = "Band 5";
            this.radiobutton_Band5.UseVisualStyleBackColor = true;
            this.radiobutton_Band5.CheckedChanged += new System.EventHandler(this.radiobutton_Band5_CheckedChanged);
            // 
            // radiobutton_Band4
            // 
            this.radiobutton_Band4.AutoSize = true;
            this.radiobutton_Band4.Location = new System.Drawing.Point(13, 110);
            this.radiobutton_Band4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band4.Name = "radiobutton_Band4";
            this.radiobutton_Band4.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band4.TabIndex = 4;
            this.radiobutton_Band4.Text = "Band 4";
            this.radiobutton_Band4.UseVisualStyleBackColor = true;
            this.radiobutton_Band4.CheckedChanged += new System.EventHandler(this.radiobutton_Band4_CheckedChanged);
            // 
            // radiobutton_Band3
            // 
            this.radiobutton_Band3.AutoSize = true;
            this.radiobutton_Band3.Location = new System.Drawing.Point(13, 86);
            this.radiobutton_Band3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band3.Name = "radiobutton_Band3";
            this.radiobutton_Band3.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band3.TabIndex = 3;
            this.radiobutton_Band3.Text = "Band 3";
            this.radiobutton_Band3.UseVisualStyleBackColor = true;
            this.radiobutton_Band3.CheckedChanged += new System.EventHandler(this.radiobutton_Band3_CheckedChanged);
            // 
            // radiobutton_Band2
            // 
            this.radiobutton_Band2.AutoSize = true;
            this.radiobutton_Band2.Location = new System.Drawing.Point(13, 63);
            this.radiobutton_Band2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band2.Name = "radiobutton_Band2";
            this.radiobutton_Band2.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band2.TabIndex = 2;
            this.radiobutton_Band2.Text = "Band 2";
            this.radiobutton_Band2.UseVisualStyleBackColor = true;
            this.radiobutton_Band2.CheckedChanged += new System.EventHandler(this.radiobutton_Band2_CheckedChanged);
            // 
            // radiobutton_Band1
            // 
            this.radiobutton_Band1.AutoSize = true;
            this.radiobutton_Band1.Location = new System.Drawing.Point(13, 39);
            this.radiobutton_Band1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band1.Name = "radiobutton_Band1";
            this.radiobutton_Band1.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band1.TabIndex = 1;
            this.radiobutton_Band1.Text = "Band 1";
            this.radiobutton_Band1.UseVisualStyleBackColor = true;
            this.radiobutton_Band1.CheckedChanged += new System.EventHandler(this.radiobutton_Band1_CheckedChanged);
            // 
            // radiobutton_Band0
            // 
            this.radiobutton_Band0.AutoSize = true;
            this.radiobutton_Band0.Checked = true;
            this.radiobutton_Band0.Location = new System.Drawing.Point(13, 16);
            this.radiobutton_Band0.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radiobutton_Band0.Name = "radiobutton_Band0";
            this.radiobutton_Band0.Size = new System.Drawing.Size(59, 17);
            this.radiobutton_Band0.TabIndex = 0;
            this.radiobutton_Band0.TabStop = true;
            this.radiobutton_Band0.Text = "Band 0";
            this.radiobutton_Band0.UseVisualStyleBackColor = true;
            this.radiobutton_Band0.CheckedChanged += new System.EventHandler(this.radiobutton_Band0_CheckedChanged);
            // 
            // dataGridView_Band_OC_Viewer
            // 
            this.dataGridView_Band_OC_Viewer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_Band_OC_Viewer.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_Band_OC_Viewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Band_OC_Viewer.Location = new System.Drawing.Point(10, 2);
            this.dataGridView_Band_OC_Viewer.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataGridView_Band_OC_Viewer.Name = "dataGridView_Band_OC_Viewer";
            this.dataGridView_Band_OC_Viewer.ReadOnly = true;
            this.dataGridView_Band_OC_Viewer.RowTemplate.Height = 23;
            this.dataGridView_Band_OC_Viewer.Size = new System.Drawing.Size(874, 266);
            this.dataGridView_Band_OC_Viewer.TabIndex = 285;
            this.dataGridView_Band_OC_Viewer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Band_OC_Viewer_CellContentClick);
            // 
            // dataGridView_OC_param
            // 
            this.dataGridView_OC_param.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_OC_param.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_OC_param.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_OC_param.Location = new System.Drawing.Point(10, 270);
            this.dataGridView_OC_param.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataGridView_OC_param.Name = "dataGridView_OC_param";
            this.dataGridView_OC_param.RowTemplate.Height = 23;
            this.dataGridView_OC_param.Size = new System.Drawing.Size(873, 536);
            this.dataGridView_OC_param.TabIndex = 284;
            this.dataGridView_OC_param.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_OC_param_KeyDown);
            // 
            // dataGridView_Gamma_Vreg1_Diff
            // 
            this.dataGridView_Gamma_Vreg1_Diff.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_Gamma_Vreg1_Diff.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_Gamma_Vreg1_Diff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Gamma_Vreg1_Diff.Location = new System.Drawing.Point(1002, 3);
            this.dataGridView_Gamma_Vreg1_Diff.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataGridView_Gamma_Vreg1_Diff.Name = "dataGridView_Gamma_Vreg1_Diff";
            this.dataGridView_Gamma_Vreg1_Diff.ReadOnly = true;
            this.dataGridView_Gamma_Vreg1_Diff.RowTemplate.Height = 23;
            this.dataGridView_Gamma_Vreg1_Diff.Size = new System.Drawing.Size(425, 802);
            this.dataGridView_Gamma_Vreg1_Diff.TabIndex = 287;
            // 
            // dataGridView_RGB_Vdata
            // 
            this.dataGridView_RGB_Vdata.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_RGB_Vdata.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_RGB_Vdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_RGB_Vdata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_RGB_Vdata.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_RGB_Vdata.Location = new System.Drawing.Point(1431, 3);
            this.dataGridView_RGB_Vdata.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataGridView_RGB_Vdata.Name = "dataGridView_RGB_Vdata";
            this.dataGridView_RGB_Vdata.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_RGB_Vdata.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_RGB_Vdata.RowTemplate.Height = 23;
            this.dataGridView_RGB_Vdata.Size = new System.Drawing.Size(315, 714);
            this.dataGridView_RGB_Vdata.TabIndex = 288;
            // 
            // button_Calculate_Data_Voltage
            // 
            this.button_Calculate_Data_Voltage.BackColor = System.Drawing.Color.Maroon;
            this.button_Calculate_Data_Voltage.ForeColor = System.Drawing.Color.White;
            this.button_Calculate_Data_Voltage.Location = new System.Drawing.Point(5, 120);
            this.button_Calculate_Data_Voltage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Calculate_Data_Voltage.Name = "button_Calculate_Data_Voltage";
            this.button_Calculate_Data_Voltage.Size = new System.Drawing.Size(99, 32);
            this.button_Calculate_Data_Voltage.TabIndex = 289;
            this.button_Calculate_Data_Voltage.Text = "Calculate Vdata";
            this.button_Calculate_Data_Voltage.UseVisualStyleBackColor = false;
            this.button_Calculate_Data_Voltage.Click += new System.EventHandler(this.button_Calculate_Data_Voltage_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox_Get_HBM_Equation_By_Dll);
            this.groupBox4.Controls.Add(this.checkBox_Get_RGB_Equation_By_Dll);
            this.groupBox4.Controls.Add(this.checkBox_Get_RGB_Equation);
            this.groupBox4.Controls.Add(this.checkBox_Get_All_Equation);
            this.groupBox4.Controls.Add(this.checkBox_Get_HBM_Equation);
            this.groupBox4.Controls.Add(this.button_Calculate_Data_Voltage);
            this.groupBox4.Location = new System.Drawing.Point(890, 590);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(105, 158);
            this.groupBox4.TabIndex = 290;
            this.groupBox4.TabStop = false;
            // 
            // checkBox_Get_HBM_Equation_By_Dll
            // 
            this.checkBox_Get_HBM_Equation_By_Dll.AutoSize = true;
            this.checkBox_Get_HBM_Equation_By_Dll.ForeColor = System.Drawing.Color.Blue;
            this.checkBox_Get_HBM_Equation_By_Dll.Location = new System.Drawing.Point(5, 34);
            this.checkBox_Get_HBM_Equation_By_Dll.Name = "checkBox_Get_HBM_Equation_By_Dll";
            this.checkBox_Get_HBM_Equation_By_Dll.Size = new System.Drawing.Size(102, 17);
            this.checkBox_Get_HBM_Equation_By_Dll.TabIndex = 294;
            this.checkBox_Get_HBM_Equation_By_Dll.Text = "HBM F(X) By Dll";
            this.checkBox_Get_HBM_Equation_By_Dll.UseVisualStyleBackColor = true;
            // 
            // checkBox_Get_RGB_Equation_By_Dll
            // 
            this.checkBox_Get_RGB_Equation_By_Dll.AutoSize = true;
            this.checkBox_Get_RGB_Equation_By_Dll.ForeColor = System.Drawing.Color.Blue;
            this.checkBox_Get_RGB_Equation_By_Dll.Location = new System.Drawing.Point(6, 98);
            this.checkBox_Get_RGB_Equation_By_Dll.Name = "checkBox_Get_RGB_Equation_By_Dll";
            this.checkBox_Get_RGB_Equation_By_Dll.Size = new System.Drawing.Size(79, 17);
            this.checkBox_Get_RGB_Equation_By_Dll.TabIndex = 293;
            this.checkBox_Get_RGB_Equation_By_Dll.Text = "RGB By Dll";
            this.checkBox_Get_RGB_Equation_By_Dll.UseVisualStyleBackColor = true;
            // 
            // checkBox_Get_RGB_Equation
            // 
            this.checkBox_Get_RGB_Equation.AutoSize = true;
            this.checkBox_Get_RGB_Equation.Location = new System.Drawing.Point(6, 78);
            this.checkBox_Get_RGB_Equation.Name = "checkBox_Get_RGB_Equation";
            this.checkBox_Get_RGB_Equation.Size = new System.Drawing.Size(69, 17);
            this.checkBox_Get_RGB_Equation.TabIndex = 292;
            this.checkBox_Get_RGB_Equation.Text = "Get RGB";
            this.checkBox_Get_RGB_Equation.UseVisualStyleBackColor = true;
            // 
            // checkBox_Get_All_Equation
            // 
            this.checkBox_Get_All_Equation.AutoSize = true;
            this.checkBox_Get_All_Equation.Location = new System.Drawing.Point(6, 55);
            this.checkBox_Get_All_Equation.Name = "checkBox_Get_All_Equation";
            this.checkBox_Get_All_Equation.Size = new System.Drawing.Size(79, 17);
            this.checkBox_Get_All_Equation.TabIndex = 291;
            this.checkBox_Get_All_Equation.Text = "Get All F(X)";
            this.checkBox_Get_All_Equation.UseVisualStyleBackColor = true;
            this.checkBox_Get_All_Equation.CheckedChanged += new System.EventHandler(this.checkBox_Get_All_Equation_CheckedChanged);
            // 
            // checkBox_Get_HBM_Equation
            // 
            this.checkBox_Get_HBM_Equation.AutoSize = true;
            this.checkBox_Get_HBM_Equation.Location = new System.Drawing.Point(5, 14);
            this.checkBox_Get_HBM_Equation.Name = "checkBox_Get_HBM_Equation";
            this.checkBox_Get_HBM_Equation.Size = new System.Drawing.Size(92, 17);
            this.checkBox_Get_HBM_Equation.TabIndex = 290;
            this.checkBox_Get_HBM_Equation.Text = "Get HBM F(X)";
            this.checkBox_Get_HBM_Equation.UseVisualStyleBackColor = true;
            // 
            // button_Test
            // 
            this.button_Test.BackColor = System.Drawing.Color.Silver;
            this.button_Test.ForeColor = System.Drawing.Color.White;
            this.button_Test.Location = new System.Drawing.Point(890, 750);
            this.button_Test.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Test.Name = "button_Test";
            this.button_Test.Size = new System.Drawing.Size(108, 26);
            this.button_Test.TabIndex = 291;
            this.button_Test.Text = "Test";
            this.button_Test.UseVisualStyleBackColor = false;
            this.button_Test.Click += new System.EventHandler(this.button_Test_Click);
            // 
            // button_Real_RGBVreg1_and_Get_Real_Vdata
            // 
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.BackColor = System.Drawing.Color.Maroon;
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.ForeColor = System.Drawing.Color.White;
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.Location = new System.Drawing.Point(1431, 723);
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.Name = "button_Real_RGBVreg1_and_Get_Real_Vdata";
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.Size = new System.Drawing.Size(315, 82);
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.TabIndex = 293;
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.Text = "Read Real R/G/B/Vreg1 and Get Real Vdata";
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.UseVisualStyleBackColor = false;
            this.button_Real_RGBVreg1_and_Get_Real_Vdata.Click += new System.EventHandler(this.button_Real_RGBVreg1_and_Get_Real_Vdata_Click);
            // 
            // Meta_Engineer_Mornitoring_Mode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1751, 811);
            this.ControlBox = false;
            this.Controls.Add(this.button_Real_RGBVreg1_and_Get_Real_Vdata);
            this.Controls.Add(this.button_Test);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.dataGridView_RGB_Vdata);
            this.Controls.Add(this.dataGridView_Gamma_Vreg1_Diff);
            this.Controls.Add(this.dataGridView_Band_OC_Viewer);
            this.Controls.Add(this.dataGridView_OC_param);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Read_OC_Param_From_Excel_File);
            this.Controls.Add(this.groupBox_Band_Selection);
            this.Controls.Add(this.button_Hide);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Meta_Engineer_Mornitoring_Mode";
            this.Text = "Meta_Engineer_Mornitoring_Mode";
            this.Load += new System.EventHandler(this.Meta_Engineer_Mornitoring_Mode_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_Band_Selection.ResumeLayout(false);
            this.groupBox_Band_Selection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Band_OC_Viewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_OC_param)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Gamma_Vreg1_Diff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_RGB_Vdata)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Hide;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_Diff_Hide;
        private System.Windows.Forms.RadioButton radioButton_Diff_Show;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_Limit_Lv_Ratio_150;
        private System.Windows.Forms.RadioButton radioButton_Limit_Lv_Ratio_60;
        private System.Windows.Forms.RadioButton radioButton_Limit_Lv_Ratio_80;
        private System.Windows.Forms.RadioButton radioButton_Limit_Lv_Ratio_100;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_Limit_Ratio_150;
        private System.Windows.Forms.RadioButton radioButton_Limit_Ratio_60;
        private System.Windows.Forms.RadioButton radioButton_Limit_Ratio_80;
        private System.Windows.Forms.RadioButton radioButton_Limit_Ratio_100;
        private System.Windows.Forms.Button button_Read_OC_Param_From_Excel_File;
        private System.Windows.Forms.GroupBox groupBox_Band_Selection;
        private System.Windows.Forms.RadioButton radiobutton_Band9;
        private System.Windows.Forms.RadioButton radiobutton_Band8;
        private System.Windows.Forms.RadioButton radiobutton_Band7;
        private System.Windows.Forms.RadioButton radiobutton_Band6;
        private System.Windows.Forms.RadioButton radiobutton_Band5;
        private System.Windows.Forms.RadioButton radiobutton_Band4;
        private System.Windows.Forms.RadioButton radiobutton_Band3;
        private System.Windows.Forms.RadioButton radiobutton_Band2;
        private System.Windows.Forms.RadioButton radiobutton_Band1;
        private System.Windows.Forms.RadioButton radiobutton_Band0;
        public System.Windows.Forms.DataGridView dataGridView_Band_OC_Viewer;
        public System.Windows.Forms.DataGridView dataGridView_OC_param;
        public System.Windows.Forms.DataGridView dataGridView_Gamma_Vreg1_Diff;
        public System.Windows.Forms.DataGridView dataGridView_RGB_Vdata;
        private System.Windows.Forms.Button button_Calculate_Data_Voltage;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox_Get_HBM_Equation;
        private System.Windows.Forms.Button button_Test;
        private System.Windows.Forms.CheckBox checkBox_Get_All_Equation;
        private System.Windows.Forms.CheckBox checkBox_Get_RGB_Equation;
        private System.Windows.Forms.CheckBox checkBox_Get_RGB_Equation_By_Dll;
        private System.Windows.Forms.CheckBox checkBox_Get_HBM_Equation_By_Dll;
        private System.Windows.Forms.Button button_Real_RGBVreg1_and_Get_Real_Vdata;
    }
}