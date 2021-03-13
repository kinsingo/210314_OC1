namespace PNC_Csharp
{
    partial class MIPI_CMD_Ctrl_Form
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
            this.textBox_Max_Second = new System.Windows.Forms.TextBox();
            this.textBox_Max_First = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Min_Second = new System.Windows.Forms.TextBox();
            this.textBox_Min_First = new System.Windows.Forms.TextBox();
            this.label_Direction = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_Max_to_Min = new System.Windows.Forms.RadioButton();
            this.radioButton_Min_to_Max = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_Option_2ea_Param = new System.Windows.Forms.RadioButton();
            this.radioButton_Option_1ea_Param = new System.Windows.Forms.RadioButton();
            this.button_Hide = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Delay = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Step = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Mipi_CMD = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_Which_Param2 = new System.Windows.Forms.TextBox();
            this.textBox_Which_Param1 = new System.Windows.Forms.TextBox();
            this.button_Change_Start = new System.Windows.Forms.Button();
            this.button_Change_Stop = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_count = new System.Windows.Forms.TextBox();
            this.checkBox_Measure_Between_CMD = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_Offset_Apply = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Offset = new System.Windows.Forms.TextBox();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_Max_Second
            // 
            this.textBox_Max_Second.Location = new System.Drawing.Point(488, 17);
            this.textBox_Max_Second.Name = "textBox_Max_Second";
            this.textBox_Max_Second.ReadOnly = true;
            this.textBox_Max_Second.Size = new System.Drawing.Size(33, 20);
            this.textBox_Max_Second.TabIndex = 10;
            // 
            // textBox_Max_First
            // 
            this.textBox_Max_First.Location = new System.Drawing.Point(434, 17);
            this.textBox_Max_First.Name = "textBox_Max_First";
            this.textBox_Max_First.Size = new System.Drawing.Size(33, 20);
            this.textBox_Max_First.TabIndex = 9;
            this.textBox_Max_First.Text = "0F";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(387, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Max : 0x            ,0x";
            // 
            // textBox_Min_Second
            // 
            this.textBox_Min_Second.Location = new System.Drawing.Point(324, 20);
            this.textBox_Min_Second.Name = "textBox_Min_Second";
            this.textBox_Min_Second.ReadOnly = true;
            this.textBox_Min_Second.Size = new System.Drawing.Size(33, 20);
            this.textBox_Min_Second.TabIndex = 7;
            // 
            // textBox_Min_First
            // 
            this.textBox_Min_First.Location = new System.Drawing.Point(272, 20);
            this.textBox_Min_First.Name = "textBox_Min_First";
            this.textBox_Min_First.Size = new System.Drawing.Size(33, 20);
            this.textBox_Min_First.TabIndex = 3;
            this.textBox_Min_First.Text = "01";
            // 
            // label_Direction
            // 
            this.label_Direction.AutoSize = true;
            this.label_Direction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Direction.Location = new System.Drawing.Point(360, 21);
            this.label_Direction.Name = "label_Direction";
            this.label_Direction.Size = new System.Drawing.Size(26, 20);
            this.label_Direction.TabIndex = 6;
            this.label_Direction.Text = "→";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(227, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Min : 0x            ,0x";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_Max_to_Min);
            this.groupBox3.Controls.Add(this.radioButton_Min_to_Max);
            this.groupBox3.Location = new System.Drawing.Point(117, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(93, 80);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Inc/Dec";
            // 
            // radioButton_Max_to_Min
            // 
            this.radioButton_Max_to_Min.AutoSize = true;
            this.radioButton_Max_to_Min.Location = new System.Drawing.Point(10, 48);
            this.radioButton_Max_to_Min.Name = "radioButton_Max_to_Min";
            this.radioButton_Max_to_Min.Size = new System.Drawing.Size(81, 16);
            this.radioButton_Max_to_Min.TabIndex = 1;
            this.radioButton_Max_to_Min.Text = "Max to Min";
            this.radioButton_Max_to_Min.UseVisualStyleBackColor = true;
            this.radioButton_Max_to_Min.CheckedChanged += new System.EventHandler(this.radioButton_Max_to_Min_CheckedChanged);
            // 
            // radioButton_Min_to_Max
            // 
            this.radioButton_Min_to_Max.AutoSize = true;
            this.radioButton_Min_to_Max.Checked = true;
            this.radioButton_Min_to_Max.Location = new System.Drawing.Point(10, 23);
            this.radioButton_Min_to_Max.Name = "radioButton_Min_to_Max";
            this.radioButton_Min_to_Max.Size = new System.Drawing.Size(81, 16);
            this.radioButton_Min_to_Max.TabIndex = 0;
            this.radioButton_Min_to_Max.TabStop = true;
            this.radioButton_Min_to_Max.Text = "Min to Max";
            this.radioButton_Min_to_Max.UseVisualStyleBackColor = true;
            this.radioButton_Min_to_Max.CheckedChanged += new System.EventHandler(this.radioButton_Min_to_Max_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_Option_2ea_Param);
            this.groupBox2.Controls.Add(this.radioButton_Option_1ea_Param);
            this.groupBox2.Location = new System.Drawing.Point(7, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(104, 80);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Param Option";
            // 
            // radioButton_Option_2ea_Param
            // 
            this.radioButton_Option_2ea_Param.AutoSize = true;
            this.radioButton_Option_2ea_Param.Location = new System.Drawing.Point(10, 48);
            this.radioButton_Option_2ea_Param.Name = "radioButton_Option_2ea_Param";
            this.radioButton_Option_2ea_Param.Size = new System.Drawing.Size(83, 16);
            this.radioButton_Option_2ea_Param.TabIndex = 1;
            this.radioButton_Option_2ea_Param.Text = "2ea Param";
            this.radioButton_Option_2ea_Param.UseVisualStyleBackColor = true;
            this.radioButton_Option_2ea_Param.CheckedChanged += new System.EventHandler(this.radioButton_Option_2ea_Param_CheckedChanged);
            // 
            // radioButton_Option_1ea_Param
            // 
            this.radioButton_Option_1ea_Param.AutoSize = true;
            this.radioButton_Option_1ea_Param.Checked = true;
            this.radioButton_Option_1ea_Param.Location = new System.Drawing.Point(10, 23);
            this.radioButton_Option_1ea_Param.Name = "radioButton_Option_1ea_Param";
            this.radioButton_Option_1ea_Param.Size = new System.Drawing.Size(83, 16);
            this.radioButton_Option_1ea_Param.TabIndex = 0;
            this.radioButton_Option_1ea_Param.TabStop = true;
            this.radioButton_Option_1ea_Param.Text = "1ea Param";
            this.radioButton_Option_1ea_Param.UseVisualStyleBackColor = true;
            this.radioButton_Option_1ea_Param.CheckedChanged += new System.EventHandler(this.radioButton_Option_1ea_Param_CheckedChanged);
            // 
            // button_Hide
            // 
            this.button_Hide.Location = new System.Drawing.Point(634, 104);
            this.button_Hide.Name = "button_Hide";
            this.button_Hide.Size = new System.Drawing.Size(87, 48);
            this.button_Hide.TabIndex = 1;
            this.button_Hide.Text = "Form Hide";
            this.button_Hide.UseVisualStyleBackColor = true;
            this.button_Hide.Click += new System.EventHandler(this.button_Hide_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(377, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Delay(ms) :            ";
            // 
            // textBox_Delay
            // 
            this.textBox_Delay.Location = new System.Drawing.Point(442, 133);
            this.textBox_Delay.Name = "textBox_Delay";
            this.textBox_Delay.Size = new System.Drawing.Size(37, 20);
            this.textBox_Delay.TabIndex = 14;
            this.textBox_Delay.Text = "10";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "Step : 0x";
            // 
            // textBox_Step
            // 
            this.textBox_Step.Location = new System.Drawing.Point(342, 132);
            this.textBox_Step.Name = "textBox_Step";
            this.textBox_Step.Size = new System.Drawing.Size(33, 20);
            this.textBox_Step.TabIndex = 16;
            this.textBox_Step.Text = "02";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "Standard CMD :";
            // 
            // textBox_Mipi_CMD
            // 
            this.textBox_Mipi_CMD.Location = new System.Drawing.Point(91, 104);
            this.textBox_Mipi_CMD.Name = "textBox_Mipi_CMD";
            this.textBox_Mipi_CMD.Size = new System.Drawing.Size(538, 20);
            this.textBox_Mipi_CMD.TabIndex = 18;
            this.textBox_Mipi_CMD.Text = "mipi.write 0x39 0x51 0x0C 0x54";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(237, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "Select which param to be changed :          ,";
            // 
            // textBox_Which_Param2
            // 
            this.textBox_Which_Param2.Location = new System.Drawing.Point(252, 134);
            this.textBox_Which_Param2.Name = "textBox_Which_Param2";
            this.textBox_Which_Param2.ReadOnly = true;
            this.textBox_Which_Param2.Size = new System.Drawing.Size(33, 20);
            this.textBox_Which_Param2.TabIndex = 21;
            // 
            // textBox_Which_Param1
            // 
            this.textBox_Which_Param1.Location = new System.Drawing.Point(212, 134);
            this.textBox_Which_Param1.Name = "textBox_Which_Param1";
            this.textBox_Which_Param1.Size = new System.Drawing.Size(33, 20);
            this.textBox_Which_Param1.TabIndex = 20;
            this.textBox_Which_Param1.Text = "1";
            this.textBox_Which_Param1.TextChanged += new System.EventHandler(this.textBox_Which_Param1_TextChanged);
            // 
            // button_Change_Start
            // 
            this.button_Change_Start.Location = new System.Drawing.Point(634, 38);
            this.button_Change_Start.Name = "button_Change_Start";
            this.button_Change_Start.Size = new System.Drawing.Size(87, 29);
            this.button_Change_Start.TabIndex = 2;
            this.button_Change_Start.Text = "Change Start";
            this.button_Change_Start.UseVisualStyleBackColor = true;
            this.button_Change_Start.Click += new System.EventHandler(this.button_Change_Start_Click);
            // 
            // button_Change_Stop
            // 
            this.button_Change_Stop.Location = new System.Drawing.Point(634, 69);
            this.button_Change_Stop.Name = "button_Change_Stop";
            this.button_Change_Stop.Size = new System.Drawing.Size(87, 29);
            this.button_Change_Stop.TabIndex = 22;
            this.button_Change_Stop.Text = "Stop";
            this.button_Change_Stop.UseVisualStyleBackColor = true;
            this.button_Change_Stop.Click += new System.EventHandler(this.button_Change_Stop_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(627, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Count : ";
            // 
            // textBox_count
            // 
            this.textBox_count.Location = new System.Drawing.Point(677, 6);
            this.textBox_count.Name = "textBox_count";
            this.textBox_count.ReadOnly = true;
            this.textBox_count.Size = new System.Drawing.Size(41, 20);
            this.textBox_count.TabIndex = 24;
            // 
            // checkBox_Measure_Between_CMD
            // 
            this.checkBox_Measure_Between_CMD.AutoSize = true;
            this.checkBox_Measure_Between_CMD.ForeColor = System.Drawing.Color.Blue;
            this.checkBox_Measure_Between_CMD.Location = new System.Drawing.Point(487, 132);
            this.checkBox_Measure_Between_CMD.Name = "checkBox_Measure_Between_CMD";
            this.checkBox_Measure_Between_CMD.Size = new System.Drawing.Size(137, 16);
            this.checkBox_Measure_Between_CMD.TabIndex = 26;
            this.checkBox_Measure_Between_CMD.Text = "Measure After Delay";
            this.checkBox_Measure_Between_CMD.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_Offset);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBox_Offset_Apply);
            this.groupBox1.Location = new System.Drawing.Point(219, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 52);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inc/Dec";
            // 
            // checkBox_Offset_Apply
            // 
            this.checkBox_Offset_Apply.AutoSize = true;
            this.checkBox_Offset_Apply.ForeColor = System.Drawing.Color.Blue;
            this.checkBox_Offset_Apply.Location = new System.Drawing.Point(9, 22);
            this.checkBox_Offset_Apply.Name = "checkBox_Offset_Apply";
            this.checkBox_Offset_Apply.Size = new System.Drawing.Size(92, 16);
            this.checkBox_Offset_Apply.TabIndex = 27;
            this.checkBox_Offset_Apply.Text = "Offset Apply";
            this.checkBox_Offset_Apply.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "0xB0 0x";
            // 
            // textBox_Offset
            // 
            this.textBox_Offset.Location = new System.Drawing.Point(146, 20);
            this.textBox_Offset.Name = "textBox_Offset";
            this.textBox_Offset.Size = new System.Drawing.Size(33, 20);
            this.textBox_Offset.TabIndex = 28;
            this.textBox_Offset.Text = "0F";
            // 
            // MIPI_CMD_Ctrl_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(725, 159);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox_Measure_Between_CMD);
            this.Controls.Add(this.textBox_count);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button_Change_Stop);
            this.Controls.Add(this.button_Hide);
            this.Controls.Add(this.button_Change_Start);
            this.Controls.Add(this.textBox_Which_Param2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox_Which_Param1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_Mipi_CMD);
            this.Controls.Add(this.label_Direction);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_Min_First);
            this.Controls.Add(this.textBox_Step);
            this.Controls.Add(this.textBox_Min_Second);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_Delay);
            this.Controls.Add(this.textBox_Max_First);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_Max_Second);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MIPI_CMD_Ctrl_Form";
            this.Text = "MIPI_CMD_Ctrl_Form";
            this.Load += new System.EventHandler(this.MIPI_CMD_Ctrl_Form_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Hide;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_Option_2ea_Param;
        private System.Windows.Forms.RadioButton radioButton_Option_1ea_Param;
        private System.Windows.Forms.TextBox textBox_Max_Second;
        private System.Windows.Forms.TextBox textBox_Max_First;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Min_Second;
        private System.Windows.Forms.TextBox textBox_Min_First;
        private System.Windows.Forms.Label label_Direction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_Max_to_Min;
        private System.Windows.Forms.RadioButton radioButton_Min_to_Max;
        private System.Windows.Forms.TextBox textBox_Step;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_Delay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Mipi_CMD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Which_Param2;
        private System.Windows.Forms.TextBox textBox_Which_Param1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_Change_Start;
        private System.Windows.Forms.Button button_Change_Stop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_count;
        private System.Windows.Forms.CheckBox checkBox_Measure_Between_CMD;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_Offset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_Offset_Apply;
    }
}