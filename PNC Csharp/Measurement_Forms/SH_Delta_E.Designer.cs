namespace PNC_Csharp
{
    partial class SH_Delta_E
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBox_Delta_E_End_Point = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Measured_DeltaE = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.Delta_E_calculation_btn = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.textBox_delay_time = new System.Windows.Forms.TextBox();
            this.Save_log_to_Excel_btn = new System.Windows.Forms.Button();
            this.progressBar_GB = new System.Windows.Forms.ProgressBar();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_E4_94ea_PTNs = new System.Windows.Forms.RadioButton();
            this.radioButton_E4_50ea_PTNs = new System.Windows.Forms.RadioButton();
            this.IR_Drop_Delta_E_btn = new System.Windows.Forms.Button();
            this.textBox_Measured_IR_Drop_Delta_E = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Clear = new System.Windows.Forms.Button();
            this.button_Hide = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Ave_Lv_Limit = new System.Windows.Forms.TextBox();
            this.checkBox_Ave_Measure = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.step_value_16 = new System.Windows.Forms.RadioButton();
            this.step_value_8 = new System.Windows.Forms.RadioButton();
            this.textBox_Delta_E2_End_Point = new System.Windows.Forms.TextBox();
            this.step_value_4 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.step_value_1 = new System.Windows.Forms.RadioButton();
            this.textBox_Measured_DeltaE2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Delta_E2_calculation_btn = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton_Max_to_Min = new System.Windows.Forms.RadioButton();
            this.radioButton_Min_to_Max = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.textBox_Delta_E_End_Point);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.textBox_Measured_DeltaE);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.Delta_E_calculation_btn);
            this.groupBox6.Location = new System.Drawing.Point(248, 19);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox6.Size = new System.Drawing.Size(262, 65);
            this.groupBox6.TabIndex = 258;
            this.groupBox6.TabStop = false;
            // 
            // textBox_Delta_E_End_Point
            // 
            this.textBox_Delta_E_End_Point.Location = new System.Drawing.Point(142, 39);
            this.textBox_Delta_E_End_Point.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Delta_E_End_Point.Name = "textBox_Delta_E_End_Point";
            this.textBox_Delta_E_End_Point.Size = new System.Drawing.Size(32, 21);
            this.textBox_Delta_E_End_Point.TabIndex = 282;
            this.textBox_Delta_E_End_Point.Text = "38";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 12);
            this.label1.TabIndex = 281;
            this.label1.Text = "Gray End Point (<255) : ";
            // 
            // textBox_Measured_DeltaE
            // 
            this.textBox_Measured_DeltaE.Location = new System.Drawing.Point(142, 13);
            this.textBox_Measured_DeltaE.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Measured_DeltaE.Name = "textBox_Measured_DeltaE";
            this.textBox_Measured_DeltaE.ReadOnly = true;
            this.textBox_Measured_DeltaE.Size = new System.Drawing.Size(32, 21);
            this.textBox_Measured_DeltaE.TabIndex = 276;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(32, 17);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(120, 12);
            this.label17.TabIndex = 275;
            this.label17.Text = "Measured Delta E3 :";
            // 
            // Delta_E_calculation_btn
            // 
            this.Delta_E_calculation_btn.BackColor = System.Drawing.Color.Navy;
            this.Delta_E_calculation_btn.ForeColor = System.Drawing.Color.White;
            this.Delta_E_calculation_btn.Location = new System.Drawing.Point(179, 13);
            this.Delta_E_calculation_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Delta_E_calculation_btn.Name = "Delta_E_calculation_btn";
            this.Delta_E_calculation_btn.Size = new System.Drawing.Size(75, 49);
            this.Delta_E_calculation_btn.TabIndex = 274;
            this.Delta_E_calculation_btn.Text = "Delta E3 calculation";
            this.Delta_E_calculation_btn.UseVisualStyleBackColor = false;
            this.Delta_E_calculation_btn.Click += new System.EventHandler(this.Delta_E_calculation_btn_Click);
            // 
            // button_Stop
            // 
            this.button_Stop.BackColor = System.Drawing.Color.Maroon;
            this.button_Stop.ForeColor = System.Drawing.Color.White;
            this.button_Stop.Location = new System.Drawing.Point(872, 104);
            this.button_Stop.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(74, 51);
            this.button_Stop.TabIndex = 280;
            this.button_Stop.Text = "Stop";
            this.button_Stop.UseVisualStyleBackColor = false;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // textBox_delay_time
            // 
            this.textBox_delay_time.Location = new System.Drawing.Point(756, 12);
            this.textBox_delay_time.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_delay_time.Name = "textBox_delay_time";
            this.textBox_delay_time.Size = new System.Drawing.Size(47, 21);
            this.textBox_delay_time.TabIndex = 278;
            this.textBox_delay_time.Text = "300";
            // 
            // Save_log_to_Excel_btn
            // 
            this.Save_log_to_Excel_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Save_log_to_Excel_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Save_log_to_Excel_btn.Location = new System.Drawing.Point(807, 6);
            this.Save_log_to_Excel_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Save_log_to_Excel_btn.Name = "Save_log_to_Excel_btn";
            this.Save_log_to_Excel_btn.Size = new System.Drawing.Size(189, 96);
            this.Save_log_to_Excel_btn.TabIndex = 273;
            this.Save_log_to_Excel_btn.Text = "Save data as excel file";
            this.Save_log_to_Excel_btn.UseVisualStyleBackColor = false;
            this.Save_log_to_Excel_btn.Click += new System.EventHandler(this.Save_log_to_Excel_btn_Click);
            // 
            // progressBar_GB
            // 
            this.progressBar_GB.Location = new System.Drawing.Point(11, 159);
            this.progressBar_GB.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.progressBar_GB.Name = "progressBar_GB";
            this.progressBar_GB.Size = new System.Drawing.Size(984, 25);
            this.progressBar_GB.TabIndex = 264;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(11, 190);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(984, 350);
            this.dataGridView1.TabIndex = 265;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_E4_94ea_PTNs);
            this.groupBox1.Controls.Add(this.radioButton_E4_50ea_PTNs);
            this.groupBox1.Controls.Add(this.IR_Drop_Delta_E_btn);
            this.groupBox1.Controls.Add(this.textBox_Measured_IR_Drop_Delta_E);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(539, 79);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(264, 74);
            this.groupBox1.TabIndex = 283;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Delta E4";
            // 
            // radioButton_E4_94ea_PTNs
            // 
            this.radioButton_E4_94ea_PTNs.AutoSize = true;
            this.radioButton_E4_94ea_PTNs.Location = new System.Drawing.Point(11, 42);
            this.radioButton_E4_94ea_PTNs.Name = "radioButton_E4_94ea_PTNs";
            this.radioButton_E4_94ea_PTNs.Size = new System.Drawing.Size(85, 16);
            this.radioButton_E4_94ea_PTNs.TabIndex = 285;
            this.radioButton_E4_94ea_PTNs.Text = "94ea PTNs";
            this.radioButton_E4_94ea_PTNs.UseVisualStyleBackColor = true;
            this.radioButton_E4_94ea_PTNs.CheckedChanged += new System.EventHandler(this.radioButton_E4_94ea_PTNs_CheckedChanged);
            // 
            // radioButton_E4_50ea_PTNs
            // 
            this.radioButton_E4_50ea_PTNs.AutoSize = true;
            this.radioButton_E4_50ea_PTNs.Checked = true;
            this.radioButton_E4_50ea_PTNs.Location = new System.Drawing.Point(11, 19);
            this.radioButton_E4_50ea_PTNs.Name = "radioButton_E4_50ea_PTNs";
            this.radioButton_E4_50ea_PTNs.Size = new System.Drawing.Size(85, 16);
            this.radioButton_E4_50ea_PTNs.TabIndex = 284;
            this.radioButton_E4_50ea_PTNs.TabStop = true;
            this.radioButton_E4_50ea_PTNs.Text = "50ea PTNs";
            this.radioButton_E4_50ea_PTNs.UseVisualStyleBackColor = true;
            this.radioButton_E4_50ea_PTNs.CheckedChanged += new System.EventHandler(this.radioButton_E4_50ea_PTNs_CheckedChanged);
            // 
            // IR_Drop_Delta_E_btn
            // 
            this.IR_Drop_Delta_E_btn.BackColor = System.Drawing.Color.Navy;
            this.IR_Drop_Delta_E_btn.ForeColor = System.Drawing.Color.White;
            this.IR_Drop_Delta_E_btn.Location = new System.Drawing.Point(136, 32);
            this.IR_Drop_Delta_E_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.IR_Drop_Delta_E_btn.Name = "IR_Drop_Delta_E_btn";
            this.IR_Drop_Delta_E_btn.Size = new System.Drawing.Size(124, 36);
            this.IR_Drop_Delta_E_btn.TabIndex = 283;
            this.IR_Drop_Delta_E_btn.Text = "IR-Drop Delta E calculation";
            this.IR_Drop_Delta_E_btn.UseVisualStyleBackColor = false;
            this.IR_Drop_Delta_E_btn.Click += new System.EventHandler(this.IRC_Drop_Delta_E_btn_Click);
            // 
            // textBox_Measured_IR_Drop_Delta_E
            // 
            this.textBox_Measured_IR_Drop_Delta_E.Location = new System.Drawing.Point(225, 10);
            this.textBox_Measured_IR_Drop_Delta_E.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Measured_IR_Drop_Delta_E.Name = "textBox_Measured_IR_Drop_Delta_E";
            this.textBox_Measured_IR_Drop_Delta_E.ReadOnly = true;
            this.textBox_Measured_IR_Drop_Delta_E.Size = new System.Drawing.Size(35, 21);
            this.textBox_Measured_IR_Drop_Delta_E.TabIndex = 276;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 12);
            this.label3.TabIndex = 275;
            this.label3.Text = "IR-Drop Delta E :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(636, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 12);
            this.label2.TabIndex = 284;
            this.label2.Text = "Delay (B/W Measure):";
            // 
            // button_Clear
            // 
            this.button_Clear.BackColor = System.Drawing.Color.Purple;
            this.button_Clear.ForeColor = System.Drawing.Color.White;
            this.button_Clear.Location = new System.Drawing.Point(807, 104);
            this.button_Clear.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(65, 51);
            this.button_Clear.TabIndex = 285;
            this.button_Clear.Text = "Clear";
            this.button_Clear.UseVisualStyleBackColor = false;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // button_Hide
            // 
            this.button_Hide.Location = new System.Drawing.Point(950, 104);
            this.button_Hide.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Hide.Name = "button_Hide";
            this.button_Hide.Size = new System.Drawing.Size(45, 51);
            this.button_Hide.TabIndex = 286;
            this.button_Hide.Text = "Hide";
            this.button_Hide.UseVisualStyleBackColor = true;
            this.button_Hide.Click += new System.EventHandler(this.button_Hide_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox_Ave_Lv_Limit);
            this.groupBox2.Controls.Add(this.checkBox_Ave_Measure);
            this.groupBox2.Location = new System.Drawing.Point(250, 89);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Size = new System.Drawing.Size(130, 55);
            this.groupBox2.TabIndex = 284;
            this.groupBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 12);
            this.label4.TabIndex = 283;
            this.label4.Text = "Limit (<Lv) : ";
            // 
            // textBox_Ave_Lv_Limit
            // 
            this.textBox_Ave_Lv_Limit.Location = new System.Drawing.Point(79, 31);
            this.textBox_Ave_Lv_Limit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Ave_Lv_Limit.Name = "textBox_Ave_Lv_Limit";
            this.textBox_Ave_Lv_Limit.Size = new System.Drawing.Size(42, 21);
            this.textBox_Ave_Lv_Limit.TabIndex = 283;
            this.textBox_Ave_Lv_Limit.Text = "0.06";
            // 
            // checkBox_Ave_Measure
            // 
            this.checkBox_Ave_Measure.AutoSize = true;
            this.checkBox_Ave_Measure.Checked = true;
            this.checkBox_Ave_Measure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Ave_Measure.Location = new System.Drawing.Point(6, 11);
            this.checkBox_Ave_Measure.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.checkBox_Ave_Measure.Name = "checkBox_Ave_Measure";
            this.checkBox_Ave_Measure.Size = new System.Drawing.Size(124, 16);
            this.checkBox_Ave_Measure.TabIndex = 0;
            this.checkBox_Ave_Measure.Text = "Average Measure";
            this.checkBox_Ave_Measure.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.step_value_16);
            this.groupBox3.Controls.Add(this.step_value_8);
            this.groupBox3.Controls.Add(this.textBox_Delta_E2_End_Point);
            this.groupBox3.Controls.Add(this.step_value_4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.step_value_1);
            this.groupBox3.Controls.Add(this.textBox_Measured_DeltaE2);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.Delta_E2_calculation_btn);
            this.groupBox3.Location = new System.Drawing.Point(5, 19);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox3.Size = new System.Drawing.Size(239, 87);
            this.groupBox3.TabIndex = 283;
            this.groupBox3.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(62, 67);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 12);
            this.label7.TabIndex = 283;
            this.label7.Text = "Step : ";
            // 
            // step_value_16
            // 
            this.step_value_16.AutoSize = true;
            this.step_value_16.Location = new System.Drawing.Point(198, 66);
            this.step_value_16.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.step_value_16.Name = "step_value_16";
            this.step_value_16.Size = new System.Drawing.Size(35, 16);
            this.step_value_16.TabIndex = 7;
            this.step_value_16.Text = "16";
            this.step_value_16.UseVisualStyleBackColor = true;
            // 
            // step_value_8
            // 
            this.step_value_8.AutoSize = true;
            this.step_value_8.Location = new System.Drawing.Point(167, 66);
            this.step_value_8.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.step_value_8.Name = "step_value_8";
            this.step_value_8.Size = new System.Drawing.Size(29, 16);
            this.step_value_8.TabIndex = 6;
            this.step_value_8.Text = "8";
            this.step_value_8.UseVisualStyleBackColor = true;
            // 
            // textBox_Delta_E2_End_Point
            // 
            this.textBox_Delta_E2_End_Point.Location = new System.Drawing.Point(114, 39);
            this.textBox_Delta_E2_End_Point.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Delta_E2_End_Point.Name = "textBox_Delta_E2_End_Point";
            this.textBox_Delta_E2_End_Point.Size = new System.Drawing.Size(32, 21);
            this.textBox_Delta_E2_End_Point.TabIndex = 282;
            this.textBox_Delta_E2_End_Point.Text = "38";
            // 
            // step_value_4
            // 
            this.step_value_4.AutoSize = true;
            this.step_value_4.Location = new System.Drawing.Point(135, 66);
            this.step_value_4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.step_value_4.Name = "step_value_4";
            this.step_value_4.Size = new System.Drawing.Size(29, 16);
            this.step_value_4.TabIndex = 5;
            this.step_value_4.Text = "4";
            this.step_value_4.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 46);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 12);
            this.label5.TabIndex = 281;
            this.label5.Text = "DBV End (< 4095) : ";
            // 
            // step_value_1
            // 
            this.step_value_1.AutoSize = true;
            this.step_value_1.Checked = true;
            this.step_value_1.Location = new System.Drawing.Point(106, 66);
            this.step_value_1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.step_value_1.Name = "step_value_1";
            this.step_value_1.Size = new System.Drawing.Size(29, 16);
            this.step_value_1.TabIndex = 4;
            this.step_value_1.TabStop = true;
            this.step_value_1.Text = "1";
            this.step_value_1.UseVisualStyleBackColor = true;
            // 
            // textBox_Measured_DeltaE2
            // 
            this.textBox_Measured_DeltaE2.Location = new System.Drawing.Point(114, 13);
            this.textBox_Measured_DeltaE2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Measured_DeltaE2.Name = "textBox_Measured_DeltaE2";
            this.textBox_Measured_DeltaE2.ReadOnly = true;
            this.textBox_Measured_DeltaE2.Size = new System.Drawing.Size(32, 21);
            this.textBox_Measured_DeltaE2.TabIndex = 276;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 12);
            this.label6.TabIndex = 275;
            this.label6.Text = " Delta E2 :";
            // 
            // Delta_E2_calculation_btn
            // 
            this.Delta_E2_calculation_btn.BackColor = System.Drawing.Color.Navy;
            this.Delta_E2_calculation_btn.ForeColor = System.Drawing.Color.White;
            this.Delta_E2_calculation_btn.Location = new System.Drawing.Point(152, 14);
            this.Delta_E2_calculation_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Delta_E2_calculation_btn.Name = "Delta_E2_calculation_btn";
            this.Delta_E2_calculation_btn.Size = new System.Drawing.Size(82, 49);
            this.Delta_E2_calculation_btn.TabIndex = 274;
            this.Delta_E2_calculation_btn.Text = "Delta E2 calculation";
            this.Delta_E2_calculation_btn.UseVisualStyleBackColor = false;
            this.Delta_E2_calculation_btn.Click += new System.EventHandler(this.Delta_E2_calculation_btn_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton_Max_to_Min);
            this.groupBox4.Controls.Add(this.radioButton_Min_to_Max);
            this.groupBox4.Location = new System.Drawing.Point(5, 105);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox4.Size = new System.Drawing.Size(239, 39);
            this.groupBox4.TabIndex = 287;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Delta E2/E3 Direction";
            // 
            // radioButton_Max_to_Min
            // 
            this.radioButton_Max_to_Min.AutoSize = true;
            this.radioButton_Max_to_Min.Location = new System.Drawing.Point(96, 16);
            this.radioButton_Max_to_Min.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Max_to_Min.Name = "radioButton_Max_to_Min";
            this.radioButton_Max_to_Min.Size = new System.Drawing.Size(87, 16);
            this.radioButton_Max_to_Min.TabIndex = 1;
            this.radioButton_Max_to_Min.Text = "Max to Min";
            this.radioButton_Max_to_Min.UseVisualStyleBackColor = true;
            // 
            // radioButton_Min_to_Max
            // 
            this.radioButton_Min_to_Max.AutoSize = true;
            this.radioButton_Min_to_Max.Checked = true;
            this.radioButton_Min_to_Max.Location = new System.Drawing.Point(11, 16);
            this.radioButton_Min_to_Max.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Min_to_Max.Name = "radioButton_Min_to_Max";
            this.radioButton_Min_to_Max.Size = new System.Drawing.Size(87, 16);
            this.radioButton_Min_to_Max.TabIndex = 0;
            this.radioButton_Min_to_Max.TabStop = true;
            this.radioButton_Min_to_Max.Text = "Min to Max";
            this.radioButton_Min_to_Max.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox3);
            this.groupBox5.Controls.Add(this.groupBox4);
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Location = new System.Drawing.Point(13, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(521, 147);
            this.groupBox5.TabIndex = 288;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Delta E2(BCS) && E3(GCS)";
            // 
            // SH_Delta_E
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(999, 552);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Hide);
            this.Controls.Add(this.button_Clear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar_GB);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox_delay_time);
            this.Controls.Add(this.button_Stop);
            this.Controls.Add(this.Save_log_to_Excel_btn);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "SH_Delta_E";
            this.Text = "SH_Delta_E";
            this.Load += new System.EventHandler(this.SH_Delta_E_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox textBox_Delta_E_End_Point;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.TextBox textBox_delay_time;
        private System.Windows.Forms.TextBox textBox_Measured_DeltaE;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button Save_log_to_Excel_btn;
        private System.Windows.Forms.Button Delta_E_calculation_btn;
        public System.Windows.Forms.ProgressBar progressBar_GB;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button IR_Drop_Delta_E_btn;
        private System.Windows.Forms.TextBox textBox_Measured_IR_Drop_Delta_E;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button button_Hide;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBox_Ave_Lv_Limit;
        public System.Windows.Forms.CheckBox checkBox_Ave_Measure;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_Delta_E2_End_Point;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_Measured_DeltaE2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Delta_E2_calculation_btn;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.RadioButton step_value_16;
        public System.Windows.Forms.RadioButton step_value_8;
        public System.Windows.Forms.RadioButton step_value_4;
        public System.Windows.Forms.RadioButton step_value_1;
        private System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.RadioButton radioButton_Max_to_Min;
        public System.Windows.Forms.RadioButton radioButton_Min_to_Max;
        private System.Windows.Forms.RadioButton radioButton_E4_94ea_PTNs;
        private System.Windows.Forms.RadioButton radioButton_E4_50ea_PTNs;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}