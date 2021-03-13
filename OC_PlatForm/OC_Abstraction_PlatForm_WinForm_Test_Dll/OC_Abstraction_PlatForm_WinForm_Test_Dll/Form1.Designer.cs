namespace OC_Abstraction_PlatForm_WinForm_Test_Dll
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
            this.richTextBox_ch1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_ch2 = new System.Windows.Forms.RichTextBox();
            this.button_Compensation = new System.Windows.Forms.Button();
            this.button_RichTextBoxClear = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox_Pos_BoxTop = new System.Windows.Forms.TextBox();
            this.textBox_Pos_BoxLeft = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox_box_back_B = new System.Windows.Forms.TextBox();
            this.textBox_box_back_G = new System.Windows.Forms.TextBox();
            this.textBox_box_back_R = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox_box_fore_B = new System.Windows.Forms.TextBox();
            this.textBox_box_fore_G = new System.Windows.Forms.TextBox();
            this.button_display_box_pattern = new System.Windows.Forms.Button();
            this.textBox_box_fore_R = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_B = new System.Windows.Forms.TextBox();
            this.textBox_G = new System.Windows.Forms.TextBox();
            this.button_Display_Pattern = new System.Windows.Forms.Button();
            this.textBox_R = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_Write_MipiData = new System.Windows.Forms.TextBox();
            this.textBox_Write_Address = new System.Windows.Forms.TextBox();
            this.button_Write = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_Read_HowMany = new System.Windows.Forms.TextBox();
            this.textBox_Read_Address = new System.Windows.Forms.TextBox();
            this.textBox_Read_Offset = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button_Read = new System.Windows.Forms.Button();
            this.button_MeasureXYLv = new System.Windows.Forms.Button();
            this.richTextBox_ch4 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_ch3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_ch8 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_ch7 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_ch6 = new System.Windows.Forms.RichTextBox();
            this.richTextBox_ch5 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button_CA_Connect = new System.Windows.Forms.Button();
            this.textBox_Pos_BoxBottom = new System.Windows.Forms.TextBox();
            this.textBox_Pos_BoxRight = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox_ch1
            // 
            this.richTextBox_ch1.Location = new System.Drawing.Point(12, 31);
            this.richTextBox_ch1.Name = "richTextBox_ch1";
            this.richTextBox_ch1.ReadOnly = true;
            this.richTextBox_ch1.Size = new System.Drawing.Size(225, 278);
            this.richTextBox_ch1.TabIndex = 0;
            this.richTextBox_ch1.Text = "";
            // 
            // richTextBox_ch2
            // 
            this.richTextBox_ch2.Location = new System.Drawing.Point(243, 31);
            this.richTextBox_ch2.Name = "richTextBox_ch2";
            this.richTextBox_ch2.ReadOnly = true;
            this.richTextBox_ch2.Size = new System.Drawing.Size(225, 278);
            this.richTextBox_ch2.TabIndex = 1;
            this.richTextBox_ch2.Text = "";
            // 
            // button_Compensation
            // 
            this.button_Compensation.BackColor = System.Drawing.Color.Maroon;
            this.button_Compensation.Location = new System.Drawing.Point(7, 502);
            this.button_Compensation.Name = "button_Compensation";
            this.button_Compensation.Size = new System.Drawing.Size(246, 30);
            this.button_Compensation.TabIndex = 9;
            this.button_Compensation.Text = "Call Compensation()";
            this.button_Compensation.UseVisualStyleBackColor = false;
            this.button_Compensation.Click += new System.EventHandler(this.button_Compensation_Click);
            // 
            // button_RichTextBoxClear
            // 
            this.button_RichTextBoxClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button_RichTextBoxClear.Location = new System.Drawing.Point(1044, 31);
            this.button_RichTextBoxClear.Name = "button_RichTextBoxClear";
            this.button_RichTextBoxClear.Size = new System.Drawing.Size(149, 31);
            this.button_RichTextBoxClear.TabIndex = 10;
            this.button_RichTextBoxClear.Text = "RichTextBox Claer";
            this.button_RichTextBoxClear.UseVisualStyleBackColor = false;
            this.button_RichTextBoxClear.Click += new System.EventHandler(this.button_RichTestBox_Clear_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.button_MeasureXYLv);
            this.groupBox2.Controls.Add(this.button_Compensation);
            this.groupBox2.Location = new System.Drawing.Point(935, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 538);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Check API Function";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox_Pos_BoxBottom);
            this.groupBox5.Controls.Add(this.textBox_Pos_BoxRight);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.textBox_Pos_BoxTop);
            this.groupBox5.Controls.Add(this.textBox_Pos_BoxLeft);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.textBox_box_back_B);
            this.groupBox5.Controls.Add(this.textBox_box_back_G);
            this.groupBox5.Controls.Add(this.textBox_box_back_R);
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this.textBox_box_fore_B);
            this.groupBox5.Controls.Add(this.textBox_box_fore_G);
            this.groupBox5.Controls.Add(this.button_display_box_pattern);
            this.groupBox5.Controls.Add(this.textBox_box_fore_R);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.ForeColor = System.Drawing.Color.Black;
            this.groupBox5.Location = new System.Drawing.Point(7, 268);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(245, 154);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Write";
            // 
            // textBox_Pos_BoxTop
            // 
            this.textBox_Pos_BoxTop.Location = new System.Drawing.Point(194, 70);
            this.textBox_Pos_BoxTop.Name = "textBox_Pos_BoxTop";
            this.textBox_Pos_BoxTop.Size = new System.Drawing.Size(41, 20);
            this.textBox_Pos_BoxTop.TabIndex = 31;
            this.textBox_Pos_BoxTop.Text = "300";
            // 
            // textBox_Pos_BoxLeft
            // 
            this.textBox_Pos_BoxLeft.Location = new System.Drawing.Point(148, 70);
            this.textBox_Pos_BoxLeft.Name = "textBox_Pos_BoxLeft";
            this.textBox_Pos_BoxLeft.Size = new System.Drawing.Size(41, 20);
            this.textBox_Pos_BoxLeft.TabIndex = 30;
            this.textBox_Pos_BoxLeft.Text = "300";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(48, 73);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 13);
            this.label17.TabIndex = 29;
            this.label17.Text = "Box Pos Left/Top : ";
            // 
            // textBox_box_back_B
            // 
            this.textBox_box_back_B.Location = new System.Drawing.Point(194, 44);
            this.textBox_box_back_B.Name = "textBox_box_back_B";
            this.textBox_box_back_B.Size = new System.Drawing.Size(41, 20);
            this.textBox_box_back_B.TabIndex = 28;
            this.textBox_box_back_B.Text = "0";
            // 
            // textBox_box_back_G
            // 
            this.textBox_box_back_G.Location = new System.Drawing.Point(148, 44);
            this.textBox_box_back_G.Name = "textBox_box_back_G";
            this.textBox_box_back_G.Size = new System.Drawing.Size(41, 20);
            this.textBox_box_back_G.TabIndex = 27;
            this.textBox_box_back_G.Text = "0";
            // 
            // textBox_box_back_R
            // 
            this.textBox_box_back_R.Location = new System.Drawing.Point(102, 44);
            this.textBox_box_back_R.Name = "textBox_box_back_R";
            this.textBox_box_back_R.Size = new System.Drawing.Size(41, 20);
            this.textBox_box_back_R.TabIndex = 26;
            this.textBox_box_back_R.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(23, 47);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 13);
            this.label16.TabIndex = 25;
            this.label16.Text = "Back R/G/B : ";
            // 
            // textBox_box_fore_B
            // 
            this.textBox_box_fore_B.Location = new System.Drawing.Point(194, 19);
            this.textBox_box_fore_B.Name = "textBox_box_fore_B";
            this.textBox_box_fore_B.Size = new System.Drawing.Size(41, 20);
            this.textBox_box_fore_B.TabIndex = 24;
            this.textBox_box_fore_B.Text = "255";
            // 
            // textBox_box_fore_G
            // 
            this.textBox_box_fore_G.Location = new System.Drawing.Point(148, 19);
            this.textBox_box_fore_G.Name = "textBox_box_fore_G";
            this.textBox_box_fore_G.Size = new System.Drawing.Size(41, 20);
            this.textBox_box_fore_G.TabIndex = 23;
            this.textBox_box_fore_G.Text = "255";
            // 
            // button_display_box_pattern
            // 
            this.button_display_box_pattern.BackColor = System.Drawing.Color.Maroon;
            this.button_display_box_pattern.ForeColor = System.Drawing.Color.White;
            this.button_display_box_pattern.Location = new System.Drawing.Point(4, 122);
            this.button_display_box_pattern.Name = "button_display_box_pattern";
            this.button_display_box_pattern.Size = new System.Drawing.Size(231, 27);
            this.button_display_box_pattern.TabIndex = 14;
            this.button_display_box_pattern.Text = "DisplayBoxPattern()";
            this.button_display_box_pattern.UseVisualStyleBackColor = false;
            this.button_display_box_pattern.Click += new System.EventHandler(this.button_display_box_pattern_Click);
            // 
            // textBox_box_fore_R
            // 
            this.textBox_box_fore_R.Location = new System.Drawing.Point(102, 19);
            this.textBox_box_fore_R.Name = "textBox_box_fore_R";
            this.textBox_box_fore_R.Size = new System.Drawing.Size(41, 20);
            this.textBox_box_fore_R.TabIndex = 22;
            this.textBox_box_fore_R.Text = "255";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(27, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 19;
            this.label14.Text = "Fore R/G/B : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_B);
            this.groupBox1.Controls.Add(this.textBox_G);
            this.groupBox1.Controls.Add(this.button_Display_Pattern);
            this.groupBox1.Controls.Add(this.textBox_R);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(7, 190);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 78);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Write";
            // 
            // textBox_B
            // 
            this.textBox_B.Location = new System.Drawing.Point(194, 19);
            this.textBox_B.Name = "textBox_B";
            this.textBox_B.Size = new System.Drawing.Size(41, 20);
            this.textBox_B.TabIndex = 24;
            this.textBox_B.Text = "255";
            // 
            // textBox_G
            // 
            this.textBox_G.Location = new System.Drawing.Point(148, 19);
            this.textBox_G.Name = "textBox_G";
            this.textBox_G.Size = new System.Drawing.Size(41, 20);
            this.textBox_G.TabIndex = 23;
            this.textBox_G.Text = "255";
            // 
            // button_Display_Pattern
            // 
            this.button_Display_Pattern.BackColor = System.Drawing.Color.Maroon;
            this.button_Display_Pattern.ForeColor = System.Drawing.Color.White;
            this.button_Display_Pattern.Location = new System.Drawing.Point(6, 45);
            this.button_Display_Pattern.Name = "button_Display_Pattern";
            this.button_Display_Pattern.Size = new System.Drawing.Size(231, 27);
            this.button_Display_Pattern.TabIndex = 14;
            this.button_Display_Pattern.Text = "DisplayMonoPattern()";
            this.button_Display_Pattern.UseVisualStyleBackColor = false;
            this.button_Display_Pattern.Click += new System.EventHandler(this.button_Display_Pattern_Click);
            // 
            // textBox_R
            // 
            this.textBox_R.Location = new System.Drawing.Point(102, 19);
            this.textBox_R.Name = "textBox_R";
            this.textBox_R.Size = new System.Drawing.Size(41, 20);
            this.textBox_R.TabIndex = 22;
            this.textBox_R.Text = "255";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(86, 13);
            this.label15.TabIndex = 19;
            this.label15.Text = "Pattern R/G/B : ";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox_Write_MipiData);
            this.groupBox4.Controls.Add(this.textBox_Write_Address);
            this.groupBox4.Controls.Add(this.button_Write);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.ForeColor = System.Drawing.Color.Black;
            this.groupBox4.Location = new System.Drawing.Point(7, 114);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(245, 70);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Write";
            // 
            // textBox_Write_MipiData
            // 
            this.textBox_Write_MipiData.Location = new System.Drawing.Point(68, 43);
            this.textBox_Write_MipiData.Name = "textBox_Write_MipiData";
            this.textBox_Write_MipiData.Size = new System.Drawing.Size(169, 20);
            this.textBox_Write_MipiData.TabIndex = 23;
            this.textBox_Write_MipiData.Text = "0xEA 0x47 0x39 0x7F";
            // 
            // textBox_Write_Address
            // 
            this.textBox_Write_Address.Location = new System.Drawing.Point(68, 19);
            this.textBox_Write_Address.Name = "textBox_Write_Address";
            this.textBox_Write_Address.Size = new System.Drawing.Size(87, 20);
            this.textBox_Write_Address.TabIndex = 22;
            this.textBox_Write_Address.Text = "E2";
            // 
            // button_Write
            // 
            this.button_Write.BackColor = System.Drawing.Color.Maroon;
            this.button_Write.ForeColor = System.Drawing.Color.White;
            this.button_Write.Location = new System.Drawing.Point(161, 17);
            this.button_Write.Name = "button_Write";
            this.button_Write.Size = new System.Drawing.Size(76, 22);
            this.button_Write.TabIndex = 11;
            this.button_Write.Text = "Write()";
            this.button_Write.UseVisualStyleBackColor = false;
            this.button_Write.Click += new System.EventHandler(this.button_Write_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Mipi Data : ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Address : ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox_Read_HowMany);
            this.groupBox3.Controls.Add(this.textBox_Read_Address);
            this.groupBox3.Controls.Add(this.textBox_Read_Offset);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.button_Read);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(7, 14);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(245, 100);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Read";
            // 
            // textBox_Read_HowMany
            // 
            this.textBox_Read_HowMany.Location = new System.Drawing.Point(69, 69);
            this.textBox_Read_HowMany.Name = "textBox_Read_HowMany";
            this.textBox_Read_HowMany.Size = new System.Drawing.Size(87, 20);
            this.textBox_Read_HowMany.TabIndex = 17;
            this.textBox_Read_HowMany.Text = "5";
            // 
            // textBox_Read_Address
            // 
            this.textBox_Read_Address.Location = new System.Drawing.Point(69, 45);
            this.textBox_Read_Address.Name = "textBox_Read_Address";
            this.textBox_Read_Address.Size = new System.Drawing.Size(87, 20);
            this.textBox_Read_Address.TabIndex = 16;
            this.textBox_Read_Address.Text = "E1";
            // 
            // textBox_Read_Offset
            // 
            this.textBox_Read_Offset.Location = new System.Drawing.Point(69, 19);
            this.textBox_Read_Offset.Name = "textBox_Read_Offset";
            this.textBox_Read_Offset.Size = new System.Drawing.Size(87, 20);
            this.textBox_Read_Offset.TabIndex = 15;
            this.textBox_Read_Offset.Text = "10";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "HowMany : ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Address : ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Offset : ";
            // 
            // button_Read
            // 
            this.button_Read.BackColor = System.Drawing.Color.Maroon;
            this.button_Read.ForeColor = System.Drawing.Color.White;
            this.button_Read.Location = new System.Drawing.Point(169, 14);
            this.button_Read.Name = "button_Read";
            this.button_Read.Size = new System.Drawing.Size(68, 75);
            this.button_Read.TabIndex = 11;
            this.button_Read.Text = "Read()";
            this.button_Read.UseVisualStyleBackColor = false;
            this.button_Read.Click += new System.EventHandler(this.button_Read_Click);
            // 
            // button_MeasureXYLv
            // 
            this.button_MeasureXYLv.BackColor = System.Drawing.Color.Maroon;
            this.button_MeasureXYLv.Location = new System.Drawing.Point(7, 423);
            this.button_MeasureXYLv.Name = "button_MeasureXYLv";
            this.button_MeasureXYLv.Size = new System.Drawing.Size(246, 29);
            this.button_MeasureXYLv.TabIndex = 10;
            this.button_MeasureXYLv.Text = "Measure XYLv()";
            this.button_MeasureXYLv.UseVisualStyleBackColor = false;
            this.button_MeasureXYLv.Click += new System.EventHandler(this.button_Measure_Click);
            // 
            // richTextBox_ch4
            // 
            this.richTextBox_ch4.Location = new System.Drawing.Point(704, 31);
            this.richTextBox_ch4.Name = "richTextBox_ch4";
            this.richTextBox_ch4.ReadOnly = true;
            this.richTextBox_ch4.Size = new System.Drawing.Size(225, 278);
            this.richTextBox_ch4.TabIndex = 15;
            this.richTextBox_ch4.Text = "";
            // 
            // richTextBox_ch3
            // 
            this.richTextBox_ch3.Location = new System.Drawing.Point(473, 31);
            this.richTextBox_ch3.Name = "richTextBox_ch3";
            this.richTextBox_ch3.ReadOnly = true;
            this.richTextBox_ch3.Size = new System.Drawing.Size(225, 278);
            this.richTextBox_ch3.TabIndex = 14;
            this.richTextBox_ch3.Text = "";
            // 
            // richTextBox_ch8
            // 
            this.richTextBox_ch8.Location = new System.Drawing.Point(701, 328);
            this.richTextBox_ch8.Name = "richTextBox_ch8";
            this.richTextBox_ch8.ReadOnly = true;
            this.richTextBox_ch8.Size = new System.Drawing.Size(225, 280);
            this.richTextBox_ch8.TabIndex = 28;
            this.richTextBox_ch8.Text = "";
            // 
            // richTextBox_ch7
            // 
            this.richTextBox_ch7.Location = new System.Drawing.Point(470, 328);
            this.richTextBox_ch7.Name = "richTextBox_ch7";
            this.richTextBox_ch7.ReadOnly = true;
            this.richTextBox_ch7.Size = new System.Drawing.Size(225, 280);
            this.richTextBox_ch7.TabIndex = 27;
            this.richTextBox_ch7.Text = "";
            // 
            // richTextBox_ch6
            // 
            this.richTextBox_ch6.Location = new System.Drawing.Point(239, 328);
            this.richTextBox_ch6.Name = "richTextBox_ch6";
            this.richTextBox_ch6.ReadOnly = true;
            this.richTextBox_ch6.Size = new System.Drawing.Size(225, 280);
            this.richTextBox_ch6.TabIndex = 22;
            this.richTextBox_ch6.Text = "";
            // 
            // richTextBox_ch5
            // 
            this.richTextBox_ch5.Location = new System.Drawing.Point(9, 328);
            this.richTextBox_ch5.Name = "richTextBox_ch5";
            this.richTextBox_ch5.ReadOnly = true;
            this.richTextBox_ch5.Size = new System.Drawing.Size(225, 280);
            this.richTextBox_ch5.TabIndex = 21;
            this.richTextBox_ch5.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Channel1[COM5] (Not Connected)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Channel2[COM6] (Not Connected)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(476, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Channel3[COM7] (Not Connected)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(701, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Channel4[COM8] (Not Connected)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 312);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Channel5[COM9] (Not Connected)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(244, 312);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(175, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Channel6[COM10] (Not Connected)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(473, 312);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(175, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Channel7[COM11] (Not Connected)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(700, 312);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(175, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Channel8[COM12] (Not Connected)";
            // 
            // button_CA_Connect
            // 
            this.button_CA_Connect.BackColor = System.Drawing.Color.Green;
            this.button_CA_Connect.Location = new System.Drawing.Point(935, 31);
            this.button_CA_Connect.Name = "button_CA_Connect";
            this.button_CA_Connect.Size = new System.Drawing.Size(103, 33);
            this.button_CA_Connect.TabIndex = 11;
            this.button_CA_Connect.Text = "CA Connect";
            this.button_CA_Connect.UseVisualStyleBackColor = false;
            this.button_CA_Connect.Click += new System.EventHandler(this.button_CA_Connect_Click);
            // 
            // textBox_Pos_BoxBottom
            // 
            this.textBox_Pos_BoxBottom.Location = new System.Drawing.Point(194, 96);
            this.textBox_Pos_BoxBottom.Name = "textBox_Pos_BoxBottom";
            this.textBox_Pos_BoxBottom.Size = new System.Drawing.Size(41, 20);
            this.textBox_Pos_BoxBottom.TabIndex = 34;
            this.textBox_Pos_BoxBottom.Text = "500";
            // 
            // textBox_Pos_BoxRight
            // 
            this.textBox_Pos_BoxRight.Location = new System.Drawing.Point(148, 96);
            this.textBox_Pos_BoxRight.Name = "textBox_Pos_BoxRight";
            this.textBox_Pos_BoxRight.Size = new System.Drawing.Size(41, 20);
            this.textBox_Pos_BoxRight.TabIndex = 33;
            this.textBox_Pos_BoxRight.Text = "500";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(27, 99);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(121, 13);
            this.label18.TabIndex = 32;
            this.label18.Text = "Box Pos Right/Bottom : ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1198, 614);
            this.Controls.Add(this.button_CA_Connect);
            this.Controls.Add(this.button_RichTextBoxClear);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_ch8);
            this.Controls.Add(this.richTextBox_ch7);
            this.Controls.Add(this.richTextBox_ch6);
            this.Controls.Add(this.richTextBox_ch5);
            this.Controls.Add(this.richTextBox_ch4);
            this.Controls.Add(this.richTextBox_ch3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.richTextBox_ch2);
            this.Controls.Add(this.richTextBox_ch1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_ch1;
        private System.Windows.Forms.RichTextBox richTextBox_ch2;
        private System.Windows.Forms.Button button_Compensation;
        private System.Windows.Forms.Button button_RichTextBoxClear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox richTextBox_ch4;
        private System.Windows.Forms.RichTextBox richTextBox_ch3;
        private System.Windows.Forms.RichTextBox richTextBox_ch8;
        private System.Windows.Forms.RichTextBox richTextBox_ch7;
        private System.Windows.Forms.RichTextBox richTextBox_ch6;
        private System.Windows.Forms.RichTextBox richTextBox_ch5;
        private System.Windows.Forms.Button button_MeasureXYLv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox_Write_MipiData;
        private System.Windows.Forms.TextBox textBox_Write_Address;
        private System.Windows.Forms.Button button_Write;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_Read_HowMany;
        private System.Windows.Forms.TextBox textBox_Read_Address;
        private System.Windows.Forms.TextBox textBox_Read_Offset;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_Read;
        private System.Windows.Forms.Button button_CA_Connect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_B;
        private System.Windows.Forms.TextBox textBox_G;
        private System.Windows.Forms.Button button_Display_Pattern;
        private System.Windows.Forms.TextBox textBox_R;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox_box_fore_B;
        private System.Windows.Forms.TextBox textBox_box_fore_G;
        private System.Windows.Forms.Button button_display_box_pattern;
        private System.Windows.Forms.TextBox textBox_box_fore_R;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_Pos_BoxTop;
        private System.Windows.Forms.TextBox textBox_Pos_BoxLeft;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox_box_back_B;
        private System.Windows.Forms.TextBox textBox_box_back_G;
        private System.Windows.Forms.TextBox textBox_box_back_R;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox_Pos_BoxBottom;
        private System.Windows.Forms.TextBox textBox_Pos_BoxRight;
        private System.Windows.Forms.Label label18;
    }
}

