namespace PNC_Csharp
{
    partial class BMP_Image_Processing_Form
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
            this.components = new System.ComponentModel.Container();
            this.Exit_btn = new System.Windows.Forms.Button();
            this.pictureBox_Loaded_BMP = new System.Windows.Forms.PictureBox();
            this.Fast_Image_load_btn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.radioButton_RGB_Data_4 = new System.Windows.Forms.RadioButton();
            this.radioButton_RGB_Data_3 = new System.Windows.Forms.RadioButton();
            this.radioButton_RGB_Data_2 = new System.Windows.Forms.RadioButton();
            this.radioButton_RGB_Data_1 = new System.Windows.Forms.RadioButton();
            this.button_Show_Origin_Image = new System.Windows.Forms.Button();
            this.dataGridView_Pixel_RGB_Display = new System.Windows.Forms.DataGridView();
            this.textBox_Y = new System.Windows.Forms.TextBox();
            this.textBox_X = new System.Windows.Forms.TextBox();
            this.button_Show_RGB = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.G_sum = new System.Windows.Forms.TextBox();
            this.B_sum = new System.Windows.Forms.TextBox();
            this.R_sum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.G_ratio = new System.Windows.Forms.TextBox();
            this.B_ratio = new System.Windows.Forms.TextBox();
            this.R_ratio = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Cal_RGB_ratio_btn = new System.Windows.Forms.Button();
            this.Get_Histogram_and_GrayArray_btn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_G_APL = new System.Windows.Forms.TextBox();
            this.textBox_B_APL = new System.Windows.Forms.TextBox();
            this.textBox_R_APL = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox_Save_RGB_Data_As_CSV = new System.Windows.Forms.CheckBox();
            this.pictureBox_Histo_R = new System.Windows.Forms.PictureBox();
            this.pictureBox_Histo_G = new System.Windows.Forms.PictureBox();
            this.pictureBox_Histo_B = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBox_MyGradation_End_Gray = new System.Windows.Forms.TextBox();
            this.textBox_MyGradation_Start_Gray = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.checkBox_MyGradation = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.textBox_G63_Border_Left_Right_Line = new System.Windows.Forms.TextBox();
            this.textBox_G63_Border_Top_Bottom_Line = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.checkBox_G63_Border = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.textBox_Dot_Size = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.checkBox_Dot_by_Dot_Pattern = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox32 = new System.Windows.Forms.GroupBox();
            this.textBox_2nd_Line_Num = new System.Windows.Forms.TextBox();
            this.textBox_1st_Line_Num = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.checkBox_H_LByL = new System.Windows.Forms.CheckBox();
            this.checkBox_V_LByL = new System.Windows.Forms.CheckBox();
            this.textBox_2nd_Dot_or_Line_B = new System.Windows.Forms.TextBox();
            this.textBox_2nd_Dot_or_Line_G = new System.Windows.Forms.TextBox();
            this.textBox_2nd_Dot_or_Line_R = new System.Windows.Forms.TextBox();
            this.textBox_1st_Dot_or_Line_B = new System.Windows.Forms.TextBox();
            this.textBox_1st_Dot_or_Line_G = new System.Windows.Forms.TextBox();
            this.textBox_1st_Dot_or_Line_R = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.textBox_Pseudo_WRGB_Gray = new System.Windows.Forms.TextBox();
            this.textBox_Pseudo_Background_Gray = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.checkBox_Pseudo = new System.Windows.Forms.CheckBox();
            this.checkBox_Cinema = new System.Windows.Forms.CheckBox();
            this.checkBox_Mosaic = new System.Windows.Forms.CheckBox();
            this.checkBox_W_H_Gradation = new System.Windows.Forms.CheckBox();
            this.checkBox_SH_All_IR_Drop_Pattern = new System.Windows.Forms.CheckBox();
            this.checkBox_H_WRGB_Gradation = new System.Windows.Forms.CheckBox();
            this.checkBox_V_WRGB_Gradation = new System.Windows.Forms.CheckBox();
            this.checkBox_Color_Bar = new System.Windows.Forms.CheckBox();
            this.checkBox_RGB_Gradation = new System.Windows.Forms.CheckBox();
            this.checkBox_Five_Color_RYGCB_Pattern = new System.Windows.Forms.CheckBox();
            this.checkBox_V_LbyL_Magenta_Green_Gradation = new System.Windows.Forms.CheckBox();
            this.checkBox_V_LbyL_Magenta_Green = new System.Windows.Forms.CheckBox();
            this.checkBox_Pattern_40_Percent = new System.Windows.Forms.CheckBox();
            this.checkBox_Mura_Detect_Pattern = new System.Windows.Forms.CheckBox();
            this.checkBox_Gray0_to_Gray7 = new System.Windows.Forms.CheckBox();
            this.checkBox_Cross_Talk = new System.Windows.Forms.CheckBox();
            this.button_Make_Multiple_Images = new System.Windows.Forms.Button();
            this.RichTextBox_BMP_Status = new System.Windows.Forms.RichTextBox();
            this.button_test = new System.Windows.Forms.Button();
            this.pictureBox_To_be_created_BMP = new System.Windows.Forms.PictureBox();
            this.textBox_BMP_Maker_Resolution_Y = new System.Windows.Forms.TextBox();
            this.textBox_BMP_Maker_Resolution_X = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.button_Dilation = new System.Windows.Forms.Button();
            this.radioButton_Morphological_Square_Kernel = new System.Windows.Forms.RadioButton();
            this.radioButton_Morphological_Circle_Kernel = new System.Windows.Forms.RadioButton();
            this.textBox_Kernel_Length = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.button_Erosion = new System.Windows.Forms.Button();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.textBox_Bi_Value = new System.Windows.Forms.TextBox();
            this.button_White_or_Black = new System.Windows.Forms.Button();
            this.label37 = new System.Windows.Forms.Label();
            this.button_Black_or_White = new System.Windows.Forms.Button();
            this.textBox_Image_Extraction_End_Gray_B = new System.Windows.Forms.TextBox();
            this.textBox_Image_Extraction_Start_Gray_B = new System.Windows.Forms.TextBox();
            this.textBox_Image_Extraction_End_Gray_G = new System.Windows.Forms.TextBox();
            this.textBox_Image_Extraction_Start_Gray_G = new System.Windows.Forms.TextBox();
            this.button_Image_Extraction_Foreground_As_White = new System.Windows.Forms.Button();
            this.button_Image_Extraction_Background_As_Black = new System.Windows.Forms.Button();
            this.textBox_Image_Extraction_End_Gray_R = new System.Windows.Forms.TextBox();
            this.textBox_Image_Extraction_Start_Gray_R = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.textBox_Image_Alpha_Value = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.button_Change_Image_Alpha_Value = new System.Windows.Forms.Button();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.button_Edge_Detection = new System.Windows.Forms.Button();
            this.checkBox_225degree_Edge_Detection = new System.Windows.Forms.CheckBox();
            this.checkBox_45degree_Edge_Detection = new System.Windows.Forms.CheckBox();
            this.checkBox_Vertical_Edge_Detection = new System.Windows.Forms.CheckBox();
            this.checkBox_Horizontal_Edge_Detection = new System.Windows.Forms.CheckBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.textBox_SharpNessWeight = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.button_HighBoost_Filter = new System.Windows.Forms.Button();
            this.button_Laplace_Sharpness_Filter = new System.Windows.Forms.Button();
            this.groupBox29 = new System.Windows.Forms.GroupBox();
            this.button_Create_Dot_Noise = new System.Windows.Forms.Button();
            this.textBox_Dot_B = new System.Windows.Forms.TextBox();
            this.textBox_Dot_G = new System.Windows.Forms.TextBox();
            this.textBox_Dot_R = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.textBox_Dot_Num = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.button_Bit_Inversion = new System.Windows.Forms.Button();
            this.button_histogram_equalization = new System.Windows.Forms.Button();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.button_5x5_Meadian_Filter = new System.Windows.Forms.Button();
            this.button_3x3_Meadian_Filter = new System.Windows.Forms.Button();
            this.button_5x5_Ave_Filter = new System.Windows.Forms.Button();
            this.button_3x3_Ave_Filter = new System.Windows.Forms.Button();
            this.button_Save_Current_Image = new System.Windows.Forms.Button();
            this.button_RGB_to_Gray = new System.Windows.Forms.Button();
            this.button_Back_To_Original = new System.Windows.Forms.Button();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.button_Change_Gray_Resolution = new System.Windows.Forms.Button();
            this.numericUpDown_gray_resolution_bits = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.button_Gamma_Decoding = new System.Windows.Forms.Button();
            this.textBox_Gamma = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.button_Gamma_Encoding = new System.Windows.Forms.Button();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.textBox_Clamping_Max_Gray_B = new System.Windows.Forms.TextBox();
            this.textBox_Clamping_Max_Gray_G = new System.Windows.Forms.TextBox();
            this.textBox_Clamping_Max_Gray_R = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.button_Clamp_Max_Gray = new System.Windows.Forms.Button();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.label_Resolution = new System.Windows.Forms.Label();
            this.label_PSNR = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.label_1st_2nd_Resolution = new System.Windows.Forms.Label();
            this.label_1st_2nd_PSNR = new System.Windows.Forms.Label();
            this.button_Get_1st_2nd_PSNR = new System.Windows.Forms.Button();
            this.button_2nd_BMP_Load = new System.Windows.Forms.Button();
            this.pictureBox_2nd_BMP = new System.Windows.Forms.PictureBox();
            this.button_1st_BMP_Load = new System.Windows.Forms.Button();
            this.pictureBox_1st_BMP = new System.Windows.Forms.PictureBox();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.button_RGB_to_CYM = new System.Windows.Forms.Button();
            this.pictureBox_Black = new System.Windows.Forms.PictureBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.pictureBox_Magenta = new System.Windows.Forms.PictureBox();
            this.pictureBox_Yellow = new System.Windows.Forms.PictureBox();
            this.pictureBox_Cyan = new System.Windows.Forms.PictureBox();
            this.pictureBox_CYM = new System.Windows.Forms.PictureBox();
            this.button_RGB_to_CYMK = new System.Windows.Forms.Button();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.label_Calculated_Color_Temperature = new System.Windows.Forms.Label();
            this.label_Ave_xy = new System.Windows.Forms.Label();
            this.label_Ave_RGB = new System.Windows.Forms.Label();
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color = new System.Windows.Forms.PictureBox();
            this.pictureBox_CIE_XY_Selected_Area = new System.Windows.Forms.PictureBox();
            this.textBox_xy_To_Col = new System.Windows.Forms.TextBox();
            this.textBox_xy_To_Row = new System.Windows.Forms.TextBox();
            this.textBox_xy_From_Col = new System.Windows.Forms.TextBox();
            this.textBox_xy_From_Row = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.button_Get_Average_ColorCoordinate_xy = new System.Windows.Forms.Button();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.groupBox30 = new System.Windows.Forms.GroupBox();
            this.groupBox33 = new System.Windows.Forms.GroupBox();
            this.label48 = new System.Windows.Forms.Label();
            this.button_Only_Resize_X_to_Bottom = new System.Windows.Forms.Button();
            this.button_Only_Resize_Top_to_X = new System.Windows.Forms.Button();
            this.button_Only_Resize_X_to_Right = new System.Windows.Forms.Button();
            this.textBox_Resize_Position = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.button_Only_Resize_Left_to_X = new System.Windows.Forms.Button();
            this.textBox_Resize_Without_To_x = new System.Windows.Forms.TextBox();
            this.textBox_Resize_Without_To_y = new System.Windows.Forms.TextBox();
            this.textBox_Resize_Without_From_x = new System.Windows.Forms.TextBox();
            this.textBox_Resize_Without_From_y = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.button_Resize_Image_Without_Change_Specific_Area = new System.Windows.Forms.Button();
            this.radioButton_Resize_Bilinear_Interpolation = new System.Windows.Forms.RadioButton();
            this.radioButton_Resize_Nearest_Interpolation = new System.Windows.Forms.RadioButton();
            this.button_Resize_Image = new System.Windows.Forms.Button();
            this.textBox_resized_height = new System.Windows.Forms.TextBox();
            this.textBox_resized_width = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.trackBar_Histo_Y_Scale = new System.Windows.Forms.TrackBar();
            this.label45 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox34 = new System.Windows.Forms.GroupBox();
            this.button_Dot_Detection = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Loaded_BMP)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Pixel_RGB_Display)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Histo_R)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Histo_G)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Histo_B)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox32.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_To_be_created_BMP)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox31.SuspendLayout();
            this.groupBox25.SuspendLayout();
            this.groupBox27.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox29.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_gray_resolution_bits)).BeginInit();
            this.groupBox15.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_2nd_BMP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_1st_BMP)).BeginInit();
            this.groupBox23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Black)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Magenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Yellow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cyan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CYM)).BeginInit();
            this.groupBox24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CIE_XY_Selected_Area_Ave_Color)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CIE_XY_Selected_Area)).BeginInit();
            this.groupBox28.SuspendLayout();
            this.groupBox30.SuspendLayout();
            this.groupBox33.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Histo_Y_Scale)).BeginInit();
            this.groupBox34.SuspendLayout();
            this.SuspendLayout();
            // 
            // Exit_btn
            // 
            this.Exit_btn.BackColor = System.Drawing.Color.Black;
            this.Exit_btn.ForeColor = System.Drawing.Color.White;
            this.Exit_btn.Location = new System.Drawing.Point(4, 140);
            this.Exit_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Exit_btn.Name = "Exit_btn";
            this.Exit_btn.Size = new System.Drawing.Size(110, 25);
            this.Exit_btn.TabIndex = 4;
            this.Exit_btn.Text = "Exit";
            this.Exit_btn.UseVisualStyleBackColor = false;
            this.Exit_btn.Click += new System.EventHandler(this.Exit_btn_Click);
            // 
            // pictureBox_Loaded_BMP
            // 
            this.pictureBox_Loaded_BMP.Location = new System.Drawing.Point(119, 13);
            this.pictureBox_Loaded_BMP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_Loaded_BMP.Name = "pictureBox_Loaded_BMP";
            this.pictureBox_Loaded_BMP.Size = new System.Drawing.Size(350, 512);
            this.pictureBox_Loaded_BMP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Loaded_BMP.TabIndex = 6;
            this.pictureBox_Loaded_BMP.TabStop = false;
            this.pictureBox_Loaded_BMP.MouseHover += new System.EventHandler(this.pictureBox_Loaded_BMP_MouseHover);
            this.pictureBox_Loaded_BMP.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_Loaded_BMP_MouseUp);
            // 
            // Fast_Image_load_btn
            // 
            this.Fast_Image_load_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Fast_Image_load_btn.ForeColor = System.Drawing.Color.White;
            this.Fast_Image_load_btn.Location = new System.Drawing.Point(6, 30);
            this.Fast_Image_load_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Fast_Image_load_btn.Name = "Fast_Image_load_btn";
            this.Fast_Image_load_btn.Size = new System.Drawing.Size(100, 26);
            this.Fast_Image_load_btn.TabIndex = 7;
            this.Fast_Image_load_btn.Text = "BMP load";
            this.Fast_Image_load_btn.UseVisualStyleBackColor = false;
            this.Fast_Image_load_btn.Click += new System.EventHandler(this.Fast_Image_load_btn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Black;
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.radioButton_RGB_Data_4);
            this.groupBox3.Controls.Add(this.radioButton_RGB_Data_3);
            this.groupBox3.Controls.Add(this.radioButton_RGB_Data_2);
            this.groupBox3.Controls.Add(this.radioButton_RGB_Data_1);
            this.groupBox3.Controls.Add(this.button_Show_Origin_Image);
            this.groupBox3.Controls.Add(this.dataGridView_Pixel_RGB_Display);
            this.groupBox3.Controls.Add(this.textBox_Y);
            this.groupBox3.Controls.Add(this.textBox_X);
            this.groupBox3.Controls.Add(this.button_Show_RGB);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(5, 718);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox3.Size = new System.Drawing.Size(473, 231);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Show Pixels";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 196);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "Y : ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 176);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "X : ";
            // 
            // radioButton_RGB_Data_4
            // 
            this.radioButton_RGB_Data_4.AutoSize = true;
            this.radioButton_RGB_Data_4.Location = new System.Drawing.Point(12, 147);
            this.radioButton_RGB_Data_4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_RGB_Data_4.Name = "radioButton_RGB_Data_4";
            this.radioButton_RGB_Data_4.Size = new System.Drawing.Size(71, 16);
            this.radioButton_RGB_Data_4.TabIndex = 10;
            this.radioButton_RGB_Data_4.Text = "+4 Pixels";
            this.radioButton_RGB_Data_4.UseVisualStyleBackColor = true;
            // 
            // radioButton_RGB_Data_3
            // 
            this.radioButton_RGB_Data_3.AutoSize = true;
            this.radioButton_RGB_Data_3.Location = new System.Drawing.Point(12, 125);
            this.radioButton_RGB_Data_3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_RGB_Data_3.Name = "radioButton_RGB_Data_3";
            this.radioButton_RGB_Data_3.Size = new System.Drawing.Size(71, 16);
            this.radioButton_RGB_Data_3.TabIndex = 9;
            this.radioButton_RGB_Data_3.Text = "+3 Pixels";
            this.radioButton_RGB_Data_3.UseVisualStyleBackColor = true;
            // 
            // radioButton_RGB_Data_2
            // 
            this.radioButton_RGB_Data_2.AutoSize = true;
            this.radioButton_RGB_Data_2.Checked = true;
            this.radioButton_RGB_Data_2.Location = new System.Drawing.Point(12, 105);
            this.radioButton_RGB_Data_2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_RGB_Data_2.Name = "radioButton_RGB_Data_2";
            this.radioButton_RGB_Data_2.Size = new System.Drawing.Size(71, 16);
            this.radioButton_RGB_Data_2.TabIndex = 8;
            this.radioButton_RGB_Data_2.TabStop = true;
            this.radioButton_RGB_Data_2.Text = "+2 Pixels";
            this.radioButton_RGB_Data_2.UseVisualStyleBackColor = true;
            // 
            // radioButton_RGB_Data_1
            // 
            this.radioButton_RGB_Data_1.AutoSize = true;
            this.radioButton_RGB_Data_1.Location = new System.Drawing.Point(12, 84);
            this.radioButton_RGB_Data_1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_RGB_Data_1.Name = "radioButton_RGB_Data_1";
            this.radioButton_RGB_Data_1.Size = new System.Drawing.Size(65, 16);
            this.radioButton_RGB_Data_1.TabIndex = 6;
            this.radioButton_RGB_Data_1.Text = "+1 Pixel";
            this.radioButton_RGB_Data_1.UseVisualStyleBackColor = true;
            // 
            // button_Show_Origin_Image
            // 
            this.button_Show_Origin_Image.BackColor = System.Drawing.Color.Black;
            this.button_Show_Origin_Image.Location = new System.Drawing.Point(8, 16);
            this.button_Show_Origin_Image.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Show_Origin_Image.Name = "button_Show_Origin_Image";
            this.button_Show_Origin_Image.Size = new System.Drawing.Size(79, 32);
            this.button_Show_Origin_Image.TabIndex = 5;
            this.button_Show_Origin_Image.Text = "Origin Image";
            this.button_Show_Origin_Image.UseVisualStyleBackColor = false;
            this.button_Show_Origin_Image.Click += new System.EventHandler(this.button_Show_Origin_Image_Click);
            // 
            // dataGridView_Pixel_RGB_Display
            // 
            this.dataGridView_Pixel_RGB_Display.AllowUserToOrderColumns = true;
            this.dataGridView_Pixel_RGB_Display.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_Pixel_RGB_Display.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView_Pixel_RGB_Display.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_Pixel_RGB_Display.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Pixel_RGB_Display.GridColor = System.Drawing.Color.Black;
            this.dataGridView_Pixel_RGB_Display.Location = new System.Drawing.Point(98, 12);
            this.dataGridView_Pixel_RGB_Display.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataGridView_Pixel_RGB_Display.Name = "dataGridView_Pixel_RGB_Display";
            this.dataGridView_Pixel_RGB_Display.RowTemplate.Height = 23;
            this.dataGridView_Pixel_RGB_Display.Size = new System.Drawing.Size(367, 207);
            this.dataGridView_Pixel_RGB_Display.TabIndex = 17;
            // 
            // textBox_Y
            // 
            this.textBox_Y.Location = new System.Drawing.Point(42, 192);
            this.textBox_Y.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Y.Name = "textBox_Y";
            this.textBox_Y.Size = new System.Drawing.Size(32, 20);
            this.textBox_Y.TabIndex = 4;
            this.textBox_Y.Text = "100";
            this.textBox_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_X
            // 
            this.textBox_X.Location = new System.Drawing.Point(42, 170);
            this.textBox_X.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_X.Name = "textBox_X";
            this.textBox_X.Size = new System.Drawing.Size(32, 20);
            this.textBox_X.TabIndex = 3;
            this.textBox_X.Text = "100";
            this.textBox_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Show_RGB
            // 
            this.button_Show_RGB.BackColor = System.Drawing.Color.Black;
            this.button_Show_RGB.Location = new System.Drawing.Point(8, 48);
            this.button_Show_RGB.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Show_RGB.Name = "button_Show_RGB";
            this.button_Show_RGB.Size = new System.Drawing.Size(79, 32);
            this.button_Show_RGB.TabIndex = 0;
            this.button_Show_RGB.Text = "Search Area";
            this.button_Show_RGB.UseVisualStyleBackColor = false;
            this.button_Show_RGB.Click += new System.EventHandler(this.button_Show_RGB_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.G_sum);
            this.groupBox2.Controls.Add(this.B_sum);
            this.groupBox2.Controls.Add(this.R_sum);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(4, 308);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Size = new System.Drawing.Size(110, 106);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RGB Sum";
            // 
            // G_sum
            // 
            this.G_sum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.G_sum.Location = new System.Drawing.Point(25, 46);
            this.G_sum.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.G_sum.Name = "G_sum";
            this.G_sum.ReadOnly = true;
            this.G_sum.Size = new System.Drawing.Size(73, 20);
            this.G_sum.TabIndex = 5;
            this.G_sum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // B_sum
            // 
            this.B_sum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.B_sum.Location = new System.Drawing.Point(25, 76);
            this.B_sum.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.B_sum.Name = "B_sum";
            this.B_sum.ReadOnly = true;
            this.B_sum.Size = new System.Drawing.Size(73, 20);
            this.B_sum.TabIndex = 4;
            this.B_sum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // R_sum
            // 
            this.R_sum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.R_sum.Location = new System.Drawing.Point(25, 17);
            this.R_sum.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.R_sum.Name = "R_sum";
            this.R_sum.ReadOnly = true;
            this.R_sum.Size = new System.Drawing.Size(73, 20);
            this.R_sum.TabIndex = 3;
            this.R_sum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Cyan;
            this.label4.Location = new System.Drawing.Point(6, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "B :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Lime;
            this.label5.Location = new System.Drawing.Point(6, 49);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "G :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "R :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.G_ratio);
            this.groupBox1.Controls.Add(this.B_ratio);
            this.groupBox1.Controls.Add(this.R_ratio);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(4, 420);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(110, 106);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RGB ratio";
            // 
            // G_ratio
            // 
            this.G_ratio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.G_ratio.Location = new System.Drawing.Point(25, 46);
            this.G_ratio.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.G_ratio.Name = "G_ratio";
            this.G_ratio.ReadOnly = true;
            this.G_ratio.Size = new System.Drawing.Size(73, 20);
            this.G_ratio.TabIndex = 5;
            this.G_ratio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // B_ratio
            // 
            this.B_ratio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.B_ratio.Location = new System.Drawing.Point(25, 76);
            this.B_ratio.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.B_ratio.Name = "B_ratio";
            this.B_ratio.ReadOnly = true;
            this.B_ratio.Size = new System.Drawing.Size(73, 20);
            this.B_ratio.TabIndex = 4;
            this.B_ratio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // R_ratio
            // 
            this.R_ratio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.R_ratio.Location = new System.Drawing.Point(25, 17);
            this.R_ratio.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.R_ratio.Name = "R_ratio";
            this.R_ratio.ReadOnly = true;
            this.R_ratio.Size = new System.Drawing.Size(73, 20);
            this.R_ratio.TabIndex = 3;
            this.R_ratio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Cyan;
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "B :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "G :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "R :";
            // 
            // Cal_RGB_ratio_btn
            // 
            this.Cal_RGB_ratio_btn.BackColor = System.Drawing.Color.Black;
            this.Cal_RGB_ratio_btn.ForeColor = System.Drawing.Color.White;
            this.Cal_RGB_ratio_btn.Location = new System.Drawing.Point(4, 111);
            this.Cal_RGB_ratio_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Cal_RGB_ratio_btn.Name = "Cal_RGB_ratio_btn";
            this.Cal_RGB_ratio_btn.Size = new System.Drawing.Size(110, 25);
            this.Cal_RGB_ratio_btn.TabIndex = 13;
            this.Cal_RGB_ratio_btn.Text = "RGB ratio/APL";
            this.Cal_RGB_ratio_btn.UseVisualStyleBackColor = false;
            this.Cal_RGB_ratio_btn.Click += new System.EventHandler(this.Cal_RGB_ratio_btn_Click);
            // 
            // Get_Histogram_and_GrayArray_btn
            // 
            this.Get_Histogram_and_GrayArray_btn.BackColor = System.Drawing.Color.Black;
            this.Get_Histogram_and_GrayArray_btn.ForeColor = System.Drawing.Color.White;
            this.Get_Histogram_and_GrayArray_btn.Location = new System.Drawing.Point(4, 70);
            this.Get_Histogram_and_GrayArray_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Get_Histogram_and_GrayArray_btn.Name = "Get_Histogram_and_GrayArray_btn";
            this.Get_Histogram_and_GrayArray_btn.Size = new System.Drawing.Size(110, 38);
            this.Get_Histogram_and_GrayArray_btn.TabIndex = 12;
            this.Get_Histogram_and_GrayArray_btn.Text = "Get RGB Array && Histogram";
            this.Get_Histogram_and_GrayArray_btn.UseVisualStyleBackColor = false;
            this.Get_Histogram_and_GrayArray_btn.Click += new System.EventHandler(this.Get_Array_btn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dataGridView1.Location = new System.Drawing.Point(735, 103);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(220, 199);
            this.dataGridView1.TabIndex = 14;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Gray";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "R";
            this.Column2.Name = "Column2";
            this.Column2.Width = 50;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "G";
            this.Column3.Name = "Column3";
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "B";
            this.Column4.Name = "Column4";
            this.Column4.Width = 50;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(4, 167);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(110, 25);
            this.progressBar1.TabIndex = 18;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox_G_APL);
            this.groupBox4.Controls.Add(this.textBox_B_APL);
            this.groupBox4.Controls.Add(this.textBox_R_APL);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(4, 196);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox4.Size = new System.Drawing.Size(110, 106);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "RGB APL";
            // 
            // textBox_G_APL
            // 
            this.textBox_G_APL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox_G_APL.Location = new System.Drawing.Point(25, 46);
            this.textBox_G_APL.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_G_APL.Name = "textBox_G_APL";
            this.textBox_G_APL.ReadOnly = true;
            this.textBox_G_APL.Size = new System.Drawing.Size(73, 20);
            this.textBox_G_APL.TabIndex = 5;
            this.textBox_G_APL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_B_APL
            // 
            this.textBox_B_APL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox_B_APL.Location = new System.Drawing.Point(25, 76);
            this.textBox_B_APL.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_B_APL.Name = "textBox_B_APL";
            this.textBox_B_APL.ReadOnly = true;
            this.textBox_B_APL.Size = new System.Drawing.Size(73, 20);
            this.textBox_B_APL.TabIndex = 4;
            this.textBox_B_APL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_R_APL
            // 
            this.textBox_R_APL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox_R_APL.Location = new System.Drawing.Point(25, 17);
            this.textBox_R_APL.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_R_APL.Name = "textBox_R_APL";
            this.textBox_R_APL.ReadOnly = true;
            this.textBox_R_APL.Size = new System.Drawing.Size(73, 20);
            this.textBox_R_APL.TabIndex = 3;
            this.textBox_R_APL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Cyan;
            this.label9.Location = new System.Drawing.Point(6, 79);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "B :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Lime;
            this.label10.Location = new System.Drawing.Point(6, 49);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "G :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(6, 22);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(19, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "R :";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Black;
            this.groupBox5.Controls.Add(this.checkBox_Save_RGB_Data_As_CSV);
            this.groupBox5.Controls.Add(this.Fast_Image_load_btn);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(4, 6);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox5.Size = new System.Drawing.Size(110, 61);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            // 
            // checkBox_Save_RGB_Data_As_CSV
            // 
            this.checkBox_Save_RGB_Data_As_CSV.AutoSize = true;
            this.checkBox_Save_RGB_Data_As_CSV.Location = new System.Drawing.Point(7, 13);
            this.checkBox_Save_RGB_Data_As_CSV.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.checkBox_Save_RGB_Data_As_CSV.Name = "checkBox_Save_RGB_Data_As_CSV";
            this.checkBox_Save_RGB_Data_As_CSV.Size = new System.Drawing.Size(107, 16);
            this.checkBox_Save_RGB_Data_As_CSV.TabIndex = 8;
            this.checkBox_Save_RGB_Data_As_CSV.Text = "Save RGB data";
            this.checkBox_Save_RGB_Data_As_CSV.UseVisualStyleBackColor = true;
            // 
            // pictureBox_Histo_R
            // 
            this.pictureBox_Histo_R.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pictureBox_Histo_R.Location = new System.Drawing.Point(474, 66);
            this.pictureBox_Histo_R.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_Histo_R.Name = "pictureBox_Histo_R";
            this.pictureBox_Histo_R.Size = new System.Drawing.Size(257, 150);
            this.pictureBox_Histo_R.TabIndex = 24;
            this.pictureBox_Histo_R.TabStop = false;
            this.pictureBox_Histo_R.Click += new System.EventHandler(this.pictureBox_Histo_R_Click);
            this.pictureBox_Histo_R.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Histo_R_Paint);
            this.pictureBox_Histo_R.Resize += new System.EventHandler(this.pictureBox_Histo_R_Resize);
            // 
            // pictureBox_Histo_G
            // 
            this.pictureBox_Histo_G.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pictureBox_Histo_G.Location = new System.Drawing.Point(473, 221);
            this.pictureBox_Histo_G.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_Histo_G.Name = "pictureBox_Histo_G";
            this.pictureBox_Histo_G.Size = new System.Drawing.Size(257, 150);
            this.pictureBox_Histo_G.TabIndex = 25;
            this.pictureBox_Histo_G.TabStop = false;
            this.pictureBox_Histo_G.Click += new System.EventHandler(this.pictureBox_Histo_G_Click);
            this.pictureBox_Histo_G.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Histo_G_Paint);
            this.pictureBox_Histo_G.Resize += new System.EventHandler(this.pictureBox_Histo_G_Resize);
            // 
            // pictureBox_Histo_B
            // 
            this.pictureBox_Histo_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pictureBox_Histo_B.Location = new System.Drawing.Point(473, 375);
            this.pictureBox_Histo_B.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_Histo_B.Name = "pictureBox_Histo_B";
            this.pictureBox_Histo_B.Size = new System.Drawing.Size(257, 150);
            this.pictureBox_Histo_B.TabIndex = 26;
            this.pictureBox_Histo_B.TabStop = false;
            this.pictureBox_Histo_B.Click += new System.EventHandler(this.pictureBox_Histo_B_Click);
            this.pictureBox_Histo_B.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Histo_B_Paint);
            this.pictureBox_Histo_B.Resize += new System.EventHandler(this.pictureBox_Histo_B_Resize);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox6.Controls.Add(this.groupBox8);
            this.groupBox6.Controls.Add(this.RichTextBox_BMP_Status);
            this.groupBox6.Controls.Add(this.button_test);
            this.groupBox6.Controls.Add(this.pictureBox_To_be_created_BMP);
            this.groupBox6.Controls.Add(this.textBox_BMP_Maker_Resolution_Y);
            this.groupBox6.Controls.Add(this.textBox_BMP_Maker_Resolution_X);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(1242, 6);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox6.Size = new System.Drawing.Size(439, 943);
            this.groupBox6.TabIndex = 27;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Image Make One by One";
            this.groupBox6.Enter += new System.EventHandler(this.groupBox6_Enter);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.groupBox9);
            this.groupBox8.Controls.Add(this.groupBox12);
            this.groupBox8.Controls.Add(this.groupBox11);
            this.groupBox8.Controls.Add(this.groupBox10);
            this.groupBox8.Controls.Add(this.checkBox_Cinema);
            this.groupBox8.Controls.Add(this.checkBox_Mosaic);
            this.groupBox8.Controls.Add(this.checkBox_W_H_Gradation);
            this.groupBox8.Controls.Add(this.checkBox_SH_All_IR_Drop_Pattern);
            this.groupBox8.Controls.Add(this.checkBox_H_WRGB_Gradation);
            this.groupBox8.Controls.Add(this.checkBox_V_WRGB_Gradation);
            this.groupBox8.Controls.Add(this.checkBox_Color_Bar);
            this.groupBox8.Controls.Add(this.checkBox_RGB_Gradation);
            this.groupBox8.Controls.Add(this.checkBox_Five_Color_RYGCB_Pattern);
            this.groupBox8.Controls.Add(this.checkBox_V_LbyL_Magenta_Green_Gradation);
            this.groupBox8.Controls.Add(this.checkBox_V_LbyL_Magenta_Green);
            this.groupBox8.Controls.Add(this.checkBox_Pattern_40_Percent);
            this.groupBox8.Controls.Add(this.checkBox_Mura_Detect_Pattern);
            this.groupBox8.Controls.Add(this.checkBox_Gray0_to_Gray7);
            this.groupBox8.Controls.Add(this.checkBox_Cross_Talk);
            this.groupBox8.Controls.Add(this.button_Make_Multiple_Images);
            this.groupBox8.ForeColor = System.Drawing.Color.White;
            this.groupBox8.Location = new System.Drawing.Point(5, 153);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(429, 784);
            this.groupBox8.TabIndex = 303;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Make Several BMPs At Once";
            this.groupBox8.Enter += new System.EventHandler(this.groupBox8_Enter);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.textBox_MyGradation_End_Gray);
            this.groupBox9.Controls.Add(this.textBox_MyGradation_Start_Gray);
            this.groupBox9.Controls.Add(this.label20);
            this.groupBox9.Controls.Add(this.label21);
            this.groupBox9.Controls.Add(this.checkBox_MyGradation);
            this.groupBox9.ForeColor = System.Drawing.Color.White;
            this.groupBox9.Location = new System.Drawing.Point(5, 207);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(418, 84);
            this.groupBox9.TabIndex = 37;
            this.groupBox9.TabStop = false;
            // 
            // textBox_MyGradation_End_Gray
            // 
            this.textBox_MyGradation_End_Gray.Location = new System.Drawing.Point(71, 54);
            this.textBox_MyGradation_End_Gray.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_MyGradation_End_Gray.Name = "textBox_MyGradation_End_Gray";
            this.textBox_MyGradation_End_Gray.Size = new System.Drawing.Size(32, 20);
            this.textBox_MyGradation_End_Gray.TabIndex = 39;
            this.textBox_MyGradation_End_Gray.Text = "16";
            this.textBox_MyGradation_End_Gray.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_MyGradation_Start_Gray
            // 
            this.textBox_MyGradation_Start_Gray.Location = new System.Drawing.Point(71, 32);
            this.textBox_MyGradation_Start_Gray.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_MyGradation_Start_Gray.Name = "textBox_MyGradation_Start_Gray";
            this.textBox_MyGradation_Start_Gray.Size = new System.Drawing.Size(32, 20);
            this.textBox_MyGradation_Start_Gray.TabIndex = 38;
            this.textBox_MyGradation_Start_Gray.Text = "1";
            this.textBox_MyGradation_Start_Gray.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 58);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 12);
            this.label20.TabIndex = 37;
            this.label20.Text = "End Gray : ";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(5, 39);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(68, 12);
            this.label21.TabIndex = 35;
            this.label21.Text = "Start Gray: ";
            // 
            // checkBox_MyGradation
            // 
            this.checkBox_MyGradation.AutoSize = true;
            this.checkBox_MyGradation.Location = new System.Drawing.Point(6, 12);
            this.checkBox_MyGradation.Name = "checkBox_MyGradation";
            this.checkBox_MyGradation.Size = new System.Drawing.Size(132, 16);
            this.checkBox_MyGradation.TabIndex = 34;
            this.checkBox_MyGradation.Text = "Selected Gradation";
            this.checkBox_MyGradation.UseVisualStyleBackColor = true;
            this.checkBox_MyGradation.CheckedChanged += new System.EventHandler(this.checkBox_MyGradation_CheckedChanged);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.textBox_G63_Border_Left_Right_Line);
            this.groupBox12.Controls.Add(this.textBox_G63_Border_Top_Bottom_Line);
            this.groupBox12.Controls.Add(this.label15);
            this.groupBox12.Controls.Add(this.label14);
            this.groupBox12.Controls.Add(this.checkBox_G63_Border);
            this.groupBox12.ForeColor = System.Drawing.Color.White;
            this.groupBox12.Location = new System.Drawing.Point(5, 289);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(418, 39);
            this.groupBox12.TabIndex = 36;
            this.groupBox12.TabStop = false;
            // 
            // textBox_G63_Border_Left_Right_Line
            // 
            this.textBox_G63_Border_Left_Right_Line.Location = new System.Drawing.Point(381, 13);
            this.textBox_G63_Border_Left_Right_Line.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_G63_Border_Left_Right_Line.Name = "textBox_G63_Border_Left_Right_Line";
            this.textBox_G63_Border_Left_Right_Line.Size = new System.Drawing.Size(32, 20);
            this.textBox_G63_Border_Left_Right_Line.TabIndex = 39;
            this.textBox_G63_Border_Left_Right_Line.Text = "1";
            this.textBox_G63_Border_Left_Right_Line.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_G63_Border_Top_Bottom_Line
            // 
            this.textBox_G63_Border_Top_Bottom_Line.Location = new System.Drawing.Point(232, 13);
            this.textBox_G63_Border_Top_Bottom_Line.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_G63_Border_Top_Bottom_Line.Name = "textBox_G63_Border_Top_Bottom_Line";
            this.textBox_G63_Border_Top_Bottom_Line.Size = new System.Drawing.Size(32, 20);
            this.textBox_G63_Border_Top_Bottom_Line.TabIndex = 38;
            this.textBox_G63_Border_Top_Bottom_Line.Text = "1";
            this.textBox_G63_Border_Top_Bottom_Line.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(271, 17);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(114, 12);
            this.label15.TabIndex = 37;
            this.label15.Text = "Left/Right W Lines : ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(110, 17);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(124, 12);
            this.label14.TabIndex = 35;
            this.label14.Text = "Top/Bottom W Lines : ";
            // 
            // checkBox_G63_Border
            // 
            this.checkBox_G63_Border.AutoSize = true;
            this.checkBox_G63_Border.Location = new System.Drawing.Point(8, 14);
            this.checkBox_G63_Border.Name = "checkBox_G63_Border";
            this.checkBox_G63_Border.Size = new System.Drawing.Size(92, 16);
            this.checkBox_G63_Border.TabIndex = 34;
            this.checkBox_G63_Border.Text = "G63_Border";
            this.checkBox_G63_Border.UseVisualStyleBackColor = true;
            this.checkBox_G63_Border.CheckedChanged += new System.EventHandler(this.checkBox_G63_Border_CheckedChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.groupBox7);
            this.groupBox11.Controls.Add(this.label19);
            this.groupBox11.Controls.Add(this.groupBox32);
            this.groupBox11.Controls.Add(this.textBox_2nd_Dot_or_Line_B);
            this.groupBox11.Controls.Add(this.textBox_2nd_Dot_or_Line_G);
            this.groupBox11.Controls.Add(this.textBox_2nd_Dot_or_Line_R);
            this.groupBox11.Controls.Add(this.textBox_1st_Dot_or_Line_B);
            this.groupBox11.Controls.Add(this.textBox_1st_Dot_or_Line_G);
            this.groupBox11.Controls.Add(this.textBox_1st_Dot_or_Line_R);
            this.groupBox11.Controls.Add(this.label18);
            this.groupBox11.Location = new System.Drawing.Point(5, 362);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(418, 124);
            this.groupBox11.TabIndex = 36;
            this.groupBox11.TabStop = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.textBox_Dot_Size);
            this.groupBox7.Controls.Add(this.label13);
            this.groupBox7.Controls.Add(this.checkBox_Dot_by_Dot_Pattern);
            this.groupBox7.ForeColor = System.Drawing.Color.White;
            this.groupBox7.Location = new System.Drawing.Point(211, 13);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox7.Size = new System.Drawing.Size(202, 56);
            this.groupBox7.TabIndex = 54;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Dot Noise Generation";
            // 
            // textBox_Dot_Size
            // 
            this.textBox_Dot_Size.Location = new System.Drawing.Point(95, 30);
            this.textBox_Dot_Size.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Dot_Size.Name = "textBox_Dot_Size";
            this.textBox_Dot_Size.Size = new System.Drawing.Size(32, 20);
            this.textBox_Dot_Size.TabIndex = 41;
            this.textBox_Dot_Size.Text = "1";
            this.textBox_Dot_Size.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(33, 33);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 12);
            this.label13.TabIndex = 50;
            this.label13.Text = "Dot Size: ";
            // 
            // checkBox_Dot_by_Dot_Pattern
            // 
            this.checkBox_Dot_by_Dot_Pattern.AutoSize = true;
            this.checkBox_Dot_by_Dot_Pattern.Location = new System.Drawing.Point(8, 15);
            this.checkBox_Dot_by_Dot_Pattern.Name = "checkBox_Dot_by_Dot_Pattern";
            this.checkBox_Dot_by_Dot_Pattern.Size = new System.Drawing.Size(125, 16);
            this.checkBox_Dot_by_Dot_Pattern.TabIndex = 26;
            this.checkBox_Dot_by_Dot_Pattern.Text = "Dot by Dot Pattern";
            this.checkBox_Dot_by_Dot_Pattern.UseVisualStyleBackColor = true;
            this.checkBox_Dot_by_Dot_Pattern.CheckedChanged += new System.EventHandler(this.checkBox_One_Dot_Pattern_CheckedChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(217, 100);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(99, 12);
            this.label19.TabIndex = 49;
            this.label19.Text = "2nd Dot or Line : ";
            // 
            // groupBox32
            // 
            this.groupBox32.Controls.Add(this.textBox_2nd_Line_Num);
            this.groupBox32.Controls.Add(this.textBox_1st_Line_Num);
            this.groupBox32.Controls.Add(this.label39);
            this.groupBox32.Controls.Add(this.label38);
            this.groupBox32.Controls.Add(this.checkBox_H_LByL);
            this.groupBox32.Controls.Add(this.checkBox_V_LByL);
            this.groupBox32.ForeColor = System.Drawing.Color.White;
            this.groupBox32.Location = new System.Drawing.Point(6, 13);
            this.groupBox32.Name = "groupBox32";
            this.groupBox32.Size = new System.Drawing.Size(200, 104);
            this.groupBox32.TabIndex = 48;
            this.groupBox32.TabStop = false;
            this.groupBox32.Text = "Line by Line";
            // 
            // textBox_2nd_Line_Num
            // 
            this.textBox_2nd_Line_Num.Location = new System.Drawing.Point(91, 75);
            this.textBox_2nd_Line_Num.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_2nd_Line_Num.Name = "textBox_2nd_Line_Num";
            this.textBox_2nd_Line_Num.Size = new System.Drawing.Size(32, 20);
            this.textBox_2nd_Line_Num.TabIndex = 41;
            this.textBox_2nd_Line_Num.Text = "16";
            this.textBox_2nd_Line_Num.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_1st_Line_Num
            // 
            this.textBox_1st_Line_Num.Location = new System.Drawing.Point(91, 53);
            this.textBox_1st_Line_Num.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_1st_Line_Num.Name = "textBox_1st_Line_Num";
            this.textBox_1st_Line_Num.Size = new System.Drawing.Size(32, 20);
            this.textBox_1st_Line_Num.TabIndex = 40;
            this.textBox_1st_Line_Num.Text = "1";
            this.textBox_1st_Line_Num.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(5, 78);
            this.label39.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(87, 12);
            this.label39.TabIndex = 37;
            this.label39.Text = "2nd Line Num: ";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(9, 57);
            this.label38.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(83, 12);
            this.label38.TabIndex = 36;
            this.label38.Text = "1st Line Num: ";
            // 
            // checkBox_H_LByL
            // 
            this.checkBox_H_LByL.AutoSize = true;
            this.checkBox_H_LByL.Location = new System.Drawing.Point(7, 15);
            this.checkBox_H_LByL.Name = "checkBox_H_LByL";
            this.checkBox_H_LByL.Size = new System.Drawing.Size(115, 16);
            this.checkBox_H_LByL.TabIndex = 19;
            this.checkBox_H_LByL.Text = "Horizentol_LByL";
            this.checkBox_H_LByL.UseVisualStyleBackColor = true;
            this.checkBox_H_LByL.CheckedChanged += new System.EventHandler(this.checkBox_H_LByL_CheckedChanged);
            // 
            // checkBox_V_LByL
            // 
            this.checkBox_V_LByL.AutoSize = true;
            this.checkBox_V_LByL.Location = new System.Drawing.Point(7, 31);
            this.checkBox_V_LByL.Name = "checkBox_V_LByL";
            this.checkBox_V_LByL.Size = new System.Drawing.Size(102, 16);
            this.checkBox_V_LByL.TabIndex = 20;
            this.checkBox_V_LByL.Text = "Vertical_LByL";
            this.checkBox_V_LByL.UseVisualStyleBackColor = true;
            this.checkBox_V_LByL.CheckedChanged += new System.EventHandler(this.checkBox_V_LByL_CheckedChanged);
            // 
            // textBox_2nd_Dot_or_Line_B
            // 
            this.textBox_2nd_Dot_or_Line_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox_2nd_Dot_or_Line_B.Location = new System.Drawing.Point(381, 97);
            this.textBox_2nd_Dot_or_Line_B.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_2nd_Dot_or_Line_B.Name = "textBox_2nd_Dot_or_Line_B";
            this.textBox_2nd_Dot_or_Line_B.Size = new System.Drawing.Size(32, 20);
            this.textBox_2nd_Dot_or_Line_B.TabIndex = 44;
            this.textBox_2nd_Dot_or_Line_B.Text = "0";
            this.textBox_2nd_Dot_or_Line_B.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_2nd_Dot_or_Line_G
            // 
            this.textBox_2nd_Dot_or_Line_G.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox_2nd_Dot_or_Line_G.Location = new System.Drawing.Point(349, 97);
            this.textBox_2nd_Dot_or_Line_G.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_2nd_Dot_or_Line_G.Name = "textBox_2nd_Dot_or_Line_G";
            this.textBox_2nd_Dot_or_Line_G.Size = new System.Drawing.Size(32, 20);
            this.textBox_2nd_Dot_or_Line_G.TabIndex = 43;
            this.textBox_2nd_Dot_or_Line_G.Text = "0";
            this.textBox_2nd_Dot_or_Line_G.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_2nd_Dot_or_Line_R
            // 
            this.textBox_2nd_Dot_or_Line_R.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox_2nd_Dot_or_Line_R.Location = new System.Drawing.Point(317, 97);
            this.textBox_2nd_Dot_or_Line_R.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_2nd_Dot_or_Line_R.Name = "textBox_2nd_Dot_or_Line_R";
            this.textBox_2nd_Dot_or_Line_R.Size = new System.Drawing.Size(32, 20);
            this.textBox_2nd_Dot_or_Line_R.TabIndex = 42;
            this.textBox_2nd_Dot_or_Line_R.Text = "0";
            this.textBox_2nd_Dot_or_Line_R.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_1st_Dot_or_Line_B
            // 
            this.textBox_1st_Dot_or_Line_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox_1st_Dot_or_Line_B.Location = new System.Drawing.Point(381, 75);
            this.textBox_1st_Dot_or_Line_B.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_1st_Dot_or_Line_B.Name = "textBox_1st_Dot_or_Line_B";
            this.textBox_1st_Dot_or_Line_B.Size = new System.Drawing.Size(32, 20);
            this.textBox_1st_Dot_or_Line_B.TabIndex = 41;
            this.textBox_1st_Dot_or_Line_B.Text = "255";
            this.textBox_1st_Dot_or_Line_B.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_1st_Dot_or_Line_G
            // 
            this.textBox_1st_Dot_or_Line_G.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox_1st_Dot_or_Line_G.Location = new System.Drawing.Point(349, 75);
            this.textBox_1st_Dot_or_Line_G.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_1st_Dot_or_Line_G.Name = "textBox_1st_Dot_or_Line_G";
            this.textBox_1st_Dot_or_Line_G.Size = new System.Drawing.Size(32, 20);
            this.textBox_1st_Dot_or_Line_G.TabIndex = 40;
            this.textBox_1st_Dot_or_Line_G.Text = "255";
            this.textBox_1st_Dot_or_Line_G.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_1st_Dot_or_Line_R
            // 
            this.textBox_1st_Dot_or_Line_R.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox_1st_Dot_or_Line_R.Location = new System.Drawing.Point(317, 75);
            this.textBox_1st_Dot_or_Line_R.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_1st_Dot_or_Line_R.Name = "textBox_1st_Dot_or_Line_R";
            this.textBox_1st_Dot_or_Line_R.Size = new System.Drawing.Size(32, 20);
            this.textBox_1st_Dot_or_Line_R.TabIndex = 39;
            this.textBox_1st_Dot_or_Line_R.Text = "255";
            this.textBox_1st_Dot_or_Line_R.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(221, 80);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(95, 12);
            this.label18.TabIndex = 36;
            this.label18.Text = "1st Dot or Line : ";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.textBox_Pseudo_WRGB_Gray);
            this.groupBox10.Controls.Add(this.textBox_Pseudo_Background_Gray);
            this.groupBox10.Controls.Add(this.label17);
            this.groupBox10.Controls.Add(this.label16);
            this.groupBox10.Controls.Add(this.checkBox_Pseudo);
            this.groupBox10.Location = new System.Drawing.Point(5, 328);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(418, 34);
            this.groupBox10.TabIndex = 35;
            this.groupBox10.TabStop = false;
            // 
            // textBox_Pseudo_WRGB_Gray
            // 
            this.textBox_Pseudo_WRGB_Gray.Location = new System.Drawing.Point(381, 11);
            this.textBox_Pseudo_WRGB_Gray.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Pseudo_WRGB_Gray.Name = "textBox_Pseudo_WRGB_Gray";
            this.textBox_Pseudo_WRGB_Gray.Size = new System.Drawing.Size(32, 20);
            this.textBox_Pseudo_WRGB_Gray.TabIndex = 40;
            this.textBox_Pseudo_WRGB_Gray.Text = "255";
            this.textBox_Pseudo_WRGB_Gray.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Pseudo_Background_Gray
            // 
            this.textBox_Pseudo_Background_Gray.Location = new System.Drawing.Point(193, 9);
            this.textBox_Pseudo_Background_Gray.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Pseudo_Background_Gray.Name = "textBox_Pseudo_Background_Gray";
            this.textBox_Pseudo_Background_Gray.Size = new System.Drawing.Size(32, 20);
            this.textBox_Pseudo_Background_Gray.TabIndex = 40;
            this.textBox_Pseudo_Background_Gray.Text = "3";
            this.textBox_Pseudo_Background_Gray.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(282, 13);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 12);
            this.label17.TabIndex = 41;
            this.label17.Text = "WRGB Dot Gray :";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(85, 13);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(105, 12);
            this.label16.TabIndex = 40;
            this.label16.Text = "BackGroundGray:";
            // 
            // checkBox_Pseudo
            // 
            this.checkBox_Pseudo.AutoSize = true;
            this.checkBox_Pseudo.Location = new System.Drawing.Point(8, 11);
            this.checkBox_Pseudo.Name = "checkBox_Pseudo";
            this.checkBox_Pseudo.Size = new System.Drawing.Size(65, 16);
            this.checkBox_Pseudo.TabIndex = 40;
            this.checkBox_Pseudo.Text = "Pseudo";
            this.checkBox_Pseudo.UseVisualStyleBackColor = true;
            this.checkBox_Pseudo.CheckedChanged += new System.EventHandler(this.checkBox_Pseudo_CheckedChanged);
            // 
            // checkBox_Cinema
            // 
            this.checkBox_Cinema.AutoSize = true;
            this.checkBox_Cinema.Location = new System.Drawing.Point(229, 167);
            this.checkBox_Cinema.Name = "checkBox_Cinema";
            this.checkBox_Cinema.Size = new System.Drawing.Size(66, 16);
            this.checkBox_Cinema.TabIndex = 30;
            this.checkBox_Cinema.Text = "Cinema";
            this.checkBox_Cinema.UseVisualStyleBackColor = true;
            this.checkBox_Cinema.CheckedChanged += new System.EventHandler(this.checkBox_Cinema_CheckedChanged);
            // 
            // checkBox_Mosaic
            // 
            this.checkBox_Mosaic.AutoSize = true;
            this.checkBox_Mosaic.Location = new System.Drawing.Point(229, 150);
            this.checkBox_Mosaic.Name = "checkBox_Mosaic";
            this.checkBox_Mosaic.Size = new System.Drawing.Size(63, 16);
            this.checkBox_Mosaic.TabIndex = 29;
            this.checkBox_Mosaic.Text = "Mosaic";
            this.checkBox_Mosaic.UseVisualStyleBackColor = true;
            this.checkBox_Mosaic.CheckedChanged += new System.EventHandler(this.checkBox_Mosaic_CheckedChanged);
            // 
            // checkBox_W_H_Gradation
            // 
            this.checkBox_W_H_Gradation.AutoSize = true;
            this.checkBox_W_H_Gradation.Location = new System.Drawing.Point(229, 133);
            this.checkBox_W_H_Gradation.Name = "checkBox_W_H_Gradation";
            this.checkBox_W_H_Gradation.Size = new System.Drawing.Size(180, 16);
            this.checkBox_W_H_Gradation.TabIndex = 28;
            this.checkBox_W_H_Gradation.Text = "White_Horizentol_Gradation";
            this.checkBox_W_H_Gradation.UseVisualStyleBackColor = true;
            this.checkBox_W_H_Gradation.CheckedChanged += new System.EventHandler(this.checkBox_W_H_Gradation_CheckedChanged);
            // 
            // checkBox_SH_All_IR_Drop_Pattern
            // 
            this.checkBox_SH_All_IR_Drop_Pattern.AutoSize = true;
            this.checkBox_SH_All_IR_Drop_Pattern.Location = new System.Drawing.Point(229, 116);
            this.checkBox_SH_All_IR_Drop_Pattern.Name = "checkBox_SH_All_IR_Drop_Pattern";
            this.checkBox_SH_All_IR_Drop_Pattern.Size = new System.Drawing.Size(156, 16);
            this.checkBox_SH_All_IR_Drop_Pattern.TabIndex = 27;
            this.checkBox_SH_All_IR_Drop_Pattern.Text = "SH_All_IR_Drop_Pattern";
            this.checkBox_SH_All_IR_Drop_Pattern.UseVisualStyleBackColor = true;
            this.checkBox_SH_All_IR_Drop_Pattern.CheckedChanged += new System.EventHandler(this.checkBox_SH_All_IR_Drop_Pattern_CheckedChanged);
            // 
            // checkBox_H_WRGB_Gradation
            // 
            this.checkBox_H_WRGB_Gradation.AutoSize = true;
            this.checkBox_H_WRGB_Gradation.Location = new System.Drawing.Point(229, 99);
            this.checkBox_H_WRGB_Gradation.Name = "checkBox_H_WRGB_Gradation";
            this.checkBox_H_WRGB_Gradation.Size = new System.Drawing.Size(184, 16);
            this.checkBox_H_WRGB_Gradation.TabIndex = 25;
            this.checkBox_H_WRGB_Gradation.Text = "Horizentol_WRGB_Gradation";
            this.checkBox_H_WRGB_Gradation.UseVisualStyleBackColor = true;
            this.checkBox_H_WRGB_Gradation.CheckedChanged += new System.EventHandler(this.checkBox_H_WRGB_Gradation_CheckedChanged);
            // 
            // checkBox_V_WRGB_Gradation
            // 
            this.checkBox_V_WRGB_Gradation.AutoSize = true;
            this.checkBox_V_WRGB_Gradation.Location = new System.Drawing.Point(229, 82);
            this.checkBox_V_WRGB_Gradation.Name = "checkBox_V_WRGB_Gradation";
            this.checkBox_V_WRGB_Gradation.Size = new System.Drawing.Size(171, 16);
            this.checkBox_V_WRGB_Gradation.TabIndex = 24;
            this.checkBox_V_WRGB_Gradation.Text = "Vertical_WRGB_Gradation";
            this.checkBox_V_WRGB_Gradation.UseVisualStyleBackColor = true;
            this.checkBox_V_WRGB_Gradation.CheckedChanged += new System.EventHandler(this.checkBox_V_WRGB_Gradation_CheckedChanged);
            // 
            // checkBox_Color_Bar
            // 
            this.checkBox_Color_Bar.AutoSize = true;
            this.checkBox_Color_Bar.Location = new System.Drawing.Point(229, 65);
            this.checkBox_Color_Bar.Name = "checkBox_Color_Bar";
            this.checkBox_Color_Bar.Size = new System.Drawing.Size(80, 16);
            this.checkBox_Color_Bar.TabIndex = 23;
            this.checkBox_Color_Bar.Text = "Color_Bar";
            this.checkBox_Color_Bar.UseVisualStyleBackColor = true;
            this.checkBox_Color_Bar.CheckedChanged += new System.EventHandler(this.checkBox_Color_Bar_CheckedChanged);
            // 
            // checkBox_RGB_Gradation
            // 
            this.checkBox_RGB_Gradation.AutoSize = true;
            this.checkBox_RGB_Gradation.Location = new System.Drawing.Point(229, 48);
            this.checkBox_RGB_Gradation.Name = "checkBox_RGB_Gradation";
            this.checkBox_RGB_Gradation.Size = new System.Drawing.Size(111, 16);
            this.checkBox_RGB_Gradation.TabIndex = 22;
            this.checkBox_RGB_Gradation.Text = "RGB_Gradation";
            this.checkBox_RGB_Gradation.UseVisualStyleBackColor = true;
            this.checkBox_RGB_Gradation.CheckedChanged += new System.EventHandler(this.checkBox_RGB_Gradation_CheckedChanged);
            // 
            // checkBox_Five_Color_RYGCB_Pattern
            // 
            this.checkBox_Five_Color_RYGCB_Pattern.AutoSize = true;
            this.checkBox_Five_Color_RYGCB_Pattern.Location = new System.Drawing.Point(6, 150);
            this.checkBox_Five_Color_RYGCB_Pattern.Name = "checkBox_Five_Color_RYGCB_Pattern";
            this.checkBox_Five_Color_RYGCB_Pattern.Size = new System.Drawing.Size(176, 16);
            this.checkBox_Five_Color_RYGCB_Pattern.TabIndex = 18;
            this.checkBox_Five_Color_RYGCB_Pattern.Text = "Five_Color_RYGCB_Pattern";
            this.checkBox_Five_Color_RYGCB_Pattern.UseVisualStyleBackColor = true;
            this.checkBox_Five_Color_RYGCB_Pattern.CheckedChanged += new System.EventHandler(this.checkBox_Five_Color_RYGCB_Pattern_CheckedChanged);
            // 
            // checkBox_V_LbyL_Magenta_Green_Gradation
            // 
            this.checkBox_V_LbyL_Magenta_Green_Gradation.AutoSize = true;
            this.checkBox_V_LbyL_Magenta_Green_Gradation.Location = new System.Drawing.Point(6, 133);
            this.checkBox_V_LbyL_Magenta_Green_Gradation.Name = "checkBox_V_LbyL_Magenta_Green_Gradation";
            this.checkBox_V_LbyL_Magenta_Green_Gradation.Size = new System.Drawing.Size(222, 16);
            this.checkBox_V_LbyL_Magenta_Green_Gradation.TabIndex = 17;
            this.checkBox_V_LbyL_Magenta_Green_Gradation.Text = "V_LbyL_Magenta_Green_Gradation";
            this.checkBox_V_LbyL_Magenta_Green_Gradation.UseVisualStyleBackColor = true;
            this.checkBox_V_LbyL_Magenta_Green_Gradation.CheckedChanged += new System.EventHandler(this.checkBox_V_LbyL_Magenta_Green_Gradation_CheckedChanged);
            // 
            // checkBox_V_LbyL_Magenta_Green
            // 
            this.checkBox_V_LbyL_Magenta_Green.AutoSize = true;
            this.checkBox_V_LbyL_Magenta_Green.Location = new System.Drawing.Point(6, 116);
            this.checkBox_V_LbyL_Magenta_Green.Name = "checkBox_V_LbyL_Magenta_Green";
            this.checkBox_V_LbyL_Magenta_Green.Size = new System.Drawing.Size(160, 16);
            this.checkBox_V_LbyL_Magenta_Green.TabIndex = 16;
            this.checkBox_V_LbyL_Magenta_Green.Text = "V_LbyL_Magenta_Green";
            this.checkBox_V_LbyL_Magenta_Green.UseVisualStyleBackColor = true;
            this.checkBox_V_LbyL_Magenta_Green.CheckedChanged += new System.EventHandler(this.checkBox_V_LbyL_Magenta_Green_CheckedChanged);
            // 
            // checkBox_Pattern_40_Percent
            // 
            this.checkBox_Pattern_40_Percent.AutoSize = true;
            this.checkBox_Pattern_40_Percent.Location = new System.Drawing.Point(6, 99);
            this.checkBox_Pattern_40_Percent.Name = "checkBox_Pattern_40_Percent";
            this.checkBox_Pattern_40_Percent.Size = new System.Drawing.Size(135, 16);
            this.checkBox_Pattern_40_Percent.TabIndex = 15;
            this.checkBox_Pattern_40_Percent.Text = "Pattern_40_Percent";
            this.checkBox_Pattern_40_Percent.UseVisualStyleBackColor = true;
            this.checkBox_Pattern_40_Percent.CheckedChanged += new System.EventHandler(this.checkBox_Pattern_40_Percent_CheckedChanged);
            // 
            // checkBox_Mura_Detect_Pattern
            // 
            this.checkBox_Mura_Detect_Pattern.AutoSize = true;
            this.checkBox_Mura_Detect_Pattern.Location = new System.Drawing.Point(6, 82);
            this.checkBox_Mura_Detect_Pattern.Name = "checkBox_Mura_Detect_Pattern";
            this.checkBox_Mura_Detect_Pattern.Size = new System.Drawing.Size(142, 16);
            this.checkBox_Mura_Detect_Pattern.TabIndex = 14;
            this.checkBox_Mura_Detect_Pattern.Text = "Mura_Detect_Pattern";
            this.checkBox_Mura_Detect_Pattern.UseVisualStyleBackColor = true;
            this.checkBox_Mura_Detect_Pattern.CheckedChanged += new System.EventHandler(this.checkBox_Mura_Detect_Pattern_CheckedChanged);
            // 
            // checkBox_Gray0_to_Gray7
            // 
            this.checkBox_Gray0_to_Gray7.AutoSize = true;
            this.checkBox_Gray0_to_Gray7.Location = new System.Drawing.Point(6, 65);
            this.checkBox_Gray0_to_Gray7.Name = "checkBox_Gray0_to_Gray7";
            this.checkBox_Gray0_to_Gray7.Size = new System.Drawing.Size(115, 16);
            this.checkBox_Gray0_to_Gray7.TabIndex = 13;
            this.checkBox_Gray0_to_Gray7.Text = "Gray0_to_Gray7";
            this.checkBox_Gray0_to_Gray7.UseVisualStyleBackColor = true;
            this.checkBox_Gray0_to_Gray7.CheckedChanged += new System.EventHandler(this.checkBox_Gray0_to_Gray7_CheckedChanged);
            // 
            // checkBox_Cross_Talk
            // 
            this.checkBox_Cross_Talk.AutoSize = true;
            this.checkBox_Cross_Talk.Location = new System.Drawing.Point(6, 48);
            this.checkBox_Cross_Talk.Name = "checkBox_Cross_Talk";
            this.checkBox_Cross_Talk.Size = new System.Drawing.Size(85, 16);
            this.checkBox_Cross_Talk.TabIndex = 12;
            this.checkBox_Cross_Talk.Text = "Cross_Talk";
            this.checkBox_Cross_Talk.UseVisualStyleBackColor = true;
            this.checkBox_Cross_Talk.CheckedChanged += new System.EventHandler(this.checkBox_Cross_Talk_CheckedChanged);
            // 
            // button_Make_Multiple_Images
            // 
            this.button_Make_Multiple_Images.BackColor = System.Drawing.Color.Black;
            this.button_Make_Multiple_Images.Location = new System.Drawing.Point(5, 17);
            this.button_Make_Multiple_Images.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Make_Multiple_Images.Name = "button_Make_Multiple_Images";
            this.button_Make_Multiple_Images.Size = new System.Drawing.Size(80, 25);
            this.button_Make_Multiple_Images.TabIndex = 11;
            this.button_Make_Multiple_Images.Text = "Make BMP";
            this.button_Make_Multiple_Images.UseVisualStyleBackColor = false;
            this.button_Make_Multiple_Images.Click += new System.EventHandler(this.button_Make_Multiple_Images_Click);
            // 
            // RichTextBox_BMP_Status
            // 
            this.RichTextBox_BMP_Status.Location = new System.Drawing.Point(10, 57);
            this.RichTextBox_BMP_Status.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.RichTextBox_BMP_Status.Name = "RichTextBox_BMP_Status";
            this.RichTextBox_BMP_Status.Size = new System.Drawing.Size(312, 90);
            this.RichTextBox_BMP_Status.TabIndex = 301;
            this.RichTextBox_BMP_Status.Text = "";
            // 
            // button_test
            // 
            this.button_test.BackColor = System.Drawing.Color.Black;
            this.button_test.Location = new System.Drawing.Point(240, 22);
            this.button_test.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(82, 27);
            this.button_test.TabIndex = 29;
            this.button_test.Text = "Clear";
            this.button_test.UseVisualStyleBackColor = false;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // pictureBox_To_be_created_BMP
            // 
            this.pictureBox_To_be_created_BMP.Location = new System.Drawing.Point(326, 19);
            this.pictureBox_To_be_created_BMP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_To_be_created_BMP.Name = "pictureBox_To_be_created_BMP";
            this.pictureBox_To_be_created_BMP.Size = new System.Drawing.Size(109, 133);
            this.pictureBox_To_be_created_BMP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_To_be_created_BMP.TabIndex = 28;
            this.pictureBox_To_be_created_BMP.TabStop = false;
            this.pictureBox_To_be_created_BMP.Click += new System.EventHandler(this.pictureBox_To_be_created_BMP_Click);
            // 
            // textBox_BMP_Maker_Resolution_Y
            // 
            this.textBox_BMP_Maker_Resolution_Y.Location = new System.Drawing.Point(190, 26);
            this.textBox_BMP_Maker_Resolution_Y.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_BMP_Maker_Resolution_Y.Name = "textBox_BMP_Maker_Resolution_Y";
            this.textBox_BMP_Maker_Resolution_Y.Size = new System.Drawing.Size(45, 20);
            this.textBox_BMP_Maker_Resolution_Y.TabIndex = 13;
            this.textBox_BMP_Maker_Resolution_Y.Text = "2400";
            this.textBox_BMP_Maker_Resolution_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_BMP_Maker_Resolution_X
            // 
            this.textBox_BMP_Maker_Resolution_X.Location = new System.Drawing.Point(145, 26);
            this.textBox_BMP_Maker_Resolution_X.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_BMP_Maker_Resolution_X.Name = "textBox_BMP_Maker_Resolution_X";
            this.textBox_BMP_Maker_Resolution_X.Size = new System.Drawing.Size(42, 20);
            this.textBox_BMP_Maker_Resolution_X.TabIndex = 11;
            this.textBox_BMP_Maker_Resolution_X.Text = "1176";
            this.textBox_BMP_Maker_Resolution_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(105, 30);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 12);
            this.label12.TabIndex = 11;
            this.label12.Text = "(X,Y) :";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.groupBox26);
            this.groupBox13.Controls.Add(this.groupBox22);
            this.groupBox13.Controls.Add(this.groupBox25);
            this.groupBox13.Controls.Add(this.groupBox27);
            this.groupBox13.Controls.Add(this.groupBox19);
            this.groupBox13.Controls.Add(this.groupBox29);
            this.groupBox13.Controls.Add(this.button_Bit_Inversion);
            this.groupBox13.Controls.Add(this.button_histogram_equalization);
            this.groupBox13.Controls.Add(this.groupBox18);
            this.groupBox13.Controls.Add(this.button_Save_Current_Image);
            this.groupBox13.Controls.Add(this.button_RGB_to_Gray);
            this.groupBox13.Controls.Add(this.button_Back_To_Original);
            this.groupBox13.Controls.Add(this.groupBox16);
            this.groupBox13.Controls.Add(this.groupBox15);
            this.groupBox13.Controls.Add(this.groupBox14);
            this.groupBox13.ForeColor = System.Drawing.Color.White;
            this.groupBox13.Location = new System.Drawing.Point(960, 6);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(277, 943);
            this.groupBox13.TabIndex = 38;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Image Processing";
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.button_Dilation);
            this.groupBox26.Controls.Add(this.radioButton_Morphological_Square_Kernel);
            this.groupBox26.Controls.Add(this.radioButton_Morphological_Circle_Kernel);
            this.groupBox26.Controls.Add(this.textBox_Kernel_Length);
            this.groupBox26.Controls.Add(this.label44);
            this.groupBox26.Controls.Add(this.button_Erosion);
            this.groupBox26.ForeColor = System.Drawing.Color.White;
            this.groupBox26.Location = new System.Drawing.Point(6, 679);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(265, 62);
            this.groupBox26.TabIndex = 291;
            this.groupBox26.TabStop = false;
            this.groupBox26.Text = "Erosion / Dilation";
            // 
            // button_Dilation
            // 
            this.button_Dilation.BackColor = System.Drawing.Color.Black;
            this.button_Dilation.ForeColor = System.Drawing.Color.White;
            this.button_Dilation.Location = new System.Drawing.Point(68, 16);
            this.button_Dilation.Name = "button_Dilation";
            this.button_Dilation.Size = new System.Drawing.Size(59, 22);
            this.button_Dilation.TabIndex = 75;
            this.button_Dilation.Text = "Dilation";
            this.button_Dilation.UseVisualStyleBackColor = false;
            this.button_Dilation.Click += new System.EventHandler(this.button_Dilation_Click);
            // 
            // radioButton_Morphological_Square_Kernel
            // 
            this.radioButton_Morphological_Square_Kernel.AutoSize = true;
            this.radioButton_Morphological_Square_Kernel.Checked = true;
            this.radioButton_Morphological_Square_Kernel.Location = new System.Drawing.Point(191, 19);
            this.radioButton_Morphological_Square_Kernel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Morphological_Square_Kernel.Name = "radioButton_Morphological_Square_Kernel";
            this.radioButton_Morphological_Square_Kernel.Size = new System.Drawing.Size(63, 16);
            this.radioButton_Morphological_Square_Kernel.TabIndex = 74;
            this.radioButton_Morphological_Square_Kernel.TabStop = true;
            this.radioButton_Morphological_Square_Kernel.Text = "Square";
            this.radioButton_Morphological_Square_Kernel.UseVisualStyleBackColor = true;
            // 
            // radioButton_Morphological_Circle_Kernel
            // 
            this.radioButton_Morphological_Circle_Kernel.AutoSize = true;
            this.radioButton_Morphological_Circle_Kernel.Location = new System.Drawing.Point(134, 19);
            this.radioButton_Morphological_Circle_Kernel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Morphological_Circle_Kernel.Name = "radioButton_Morphological_Circle_Kernel";
            this.radioButton_Morphological_Circle_Kernel.Size = new System.Drawing.Size(56, 16);
            this.radioButton_Morphological_Circle_Kernel.TabIndex = 73;
            this.radioButton_Morphological_Circle_Kernel.Text = "Circle";
            this.radioButton_Morphological_Circle_Kernel.UseVisualStyleBackColor = true;
            // 
            // textBox_Kernel_Length
            // 
            this.textBox_Kernel_Length.Location = new System.Drawing.Point(213, 38);
            this.textBox_Kernel_Length.Name = "textBox_Kernel_Length";
            this.textBox_Kernel_Length.Size = new System.Drawing.Size(37, 20);
            this.textBox_Kernel_Length.TabIndex = 72;
            this.textBox_Kernel_Length.Text = "15";
            this.textBox_Kernel_Length.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(15, 41);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(198, 12);
            this.label44.TabIndex = 71;
            this.label44.Text = "Diameter or Square\'s Side Length :";
            // 
            // button_Erosion
            // 
            this.button_Erosion.BackColor = System.Drawing.Color.Black;
            this.button_Erosion.ForeColor = System.Drawing.Color.White;
            this.button_Erosion.Location = new System.Drawing.Point(7, 16);
            this.button_Erosion.Name = "button_Erosion";
            this.button_Erosion.Size = new System.Drawing.Size(59, 22);
            this.button_Erosion.TabIndex = 70;
            this.button_Erosion.Text = "Erosion";
            this.button_Erosion.UseVisualStyleBackColor = false;
            this.button_Erosion.Click += new System.EventHandler(this.button_Erosion_Click);
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.groupBox31);
            this.groupBox22.Controls.Add(this.textBox_Image_Extraction_End_Gray_B);
            this.groupBox22.Controls.Add(this.textBox_Image_Extraction_Start_Gray_B);
            this.groupBox22.Controls.Add(this.textBox_Image_Extraction_End_Gray_G);
            this.groupBox22.Controls.Add(this.textBox_Image_Extraction_Start_Gray_G);
            this.groupBox22.Controls.Add(this.button_Image_Extraction_Foreground_As_White);
            this.groupBox22.Controls.Add(this.button_Image_Extraction_Background_As_Black);
            this.groupBox22.Controls.Add(this.textBox_Image_Extraction_End_Gray_R);
            this.groupBox22.Controls.Add(this.textBox_Image_Extraction_Start_Gray_R);
            this.groupBox22.Controls.Add(this.label26);
            this.groupBox22.Controls.Add(this.label27);
            this.groupBox22.ForeColor = System.Drawing.Color.White;
            this.groupBox22.Location = new System.Drawing.Point(6, 541);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(265, 133);
            this.groupBox22.TabIndex = 46;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Image Extraction";
            // 
            // groupBox31
            // 
            this.groupBox31.Controls.Add(this.textBox_Bi_Value);
            this.groupBox31.Controls.Add(this.button_White_or_Black);
            this.groupBox31.Controls.Add(this.label37);
            this.groupBox31.Controls.Add(this.button_Black_or_White);
            this.groupBox31.ForeColor = System.Drawing.Color.White;
            this.groupBox31.Location = new System.Drawing.Point(6, 31);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Size = new System.Drawing.Size(100, 96);
            this.groupBox31.TabIndex = 3;
            this.groupBox31.TabStop = false;
            this.groupBox31.Text = "Black or White";
            // 
            // textBox_Bi_Value
            // 
            this.textBox_Bi_Value.Location = new System.Drawing.Point(60, 14);
            this.textBox_Bi_Value.Name = "textBox_Bi_Value";
            this.textBox_Bi_Value.Size = new System.Drawing.Size(37, 20);
            this.textBox_Bi_Value.TabIndex = 6;
            this.textBox_Bi_Value.Text = "100";
            this.textBox_Bi_Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_White_or_Black
            // 
            this.button_White_or_Black.BackColor = System.Drawing.Color.Black;
            this.button_White_or_Black.Location = new System.Drawing.Point(5, 66);
            this.button_White_or_Black.Name = "button_White_or_Black";
            this.button_White_or_Black.Size = new System.Drawing.Size(91, 25);
            this.button_White_or_Black.TabIndex = 3;
            this.button_White_or_Black.Text = "White / Black";
            this.button_White_or_Black.UseVisualStyleBackColor = false;
            this.button_White_or_Black.Click += new System.EventHandler(this.button_White_or_Black_Click);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 19);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(58, 12);
            this.label37.TabIndex = 2;
            this.label37.Text = "Th Gray : ";
            // 
            // button_Black_or_White
            // 
            this.button_Black_or_White.BackColor = System.Drawing.Color.Black;
            this.button_Black_or_White.Location = new System.Drawing.Point(5, 42);
            this.button_Black_or_White.Name = "button_Black_or_White";
            this.button_Black_or_White.Size = new System.Drawing.Size(91, 25);
            this.button_Black_or_White.TabIndex = 1;
            this.button_Black_or_White.Text = "Black / White";
            this.button_Black_or_White.UseVisualStyleBackColor = false;
            this.button_Black_or_White.Click += new System.EventHandler(this.button_Black_or_White_Click);
            // 
            // textBox_Image_Extraction_End_Gray_B
            // 
            this.textBox_Image_Extraction_End_Gray_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox_Image_Extraction_End_Gray_B.Location = new System.Drawing.Point(232, 106);
            this.textBox_Image_Extraction_End_Gray_B.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Image_Extraction_End_Gray_B.Name = "textBox_Image_Extraction_End_Gray_B";
            this.textBox_Image_Extraction_End_Gray_B.Size = new System.Drawing.Size(28, 20);
            this.textBox_Image_Extraction_End_Gray_B.TabIndex = 49;
            this.textBox_Image_Extraction_End_Gray_B.Text = "255";
            this.textBox_Image_Extraction_End_Gray_B.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Image_Extraction_Start_Gray_B
            // 
            this.textBox_Image_Extraction_Start_Gray_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox_Image_Extraction_Start_Gray_B.Location = new System.Drawing.Point(232, 84);
            this.textBox_Image_Extraction_Start_Gray_B.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Image_Extraction_Start_Gray_B.Name = "textBox_Image_Extraction_Start_Gray_B";
            this.textBox_Image_Extraction_Start_Gray_B.Size = new System.Drawing.Size(28, 20);
            this.textBox_Image_Extraction_Start_Gray_B.TabIndex = 48;
            this.textBox_Image_Extraction_Start_Gray_B.Text = "1";
            this.textBox_Image_Extraction_Start_Gray_B.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Image_Extraction_End_Gray_G
            // 
            this.textBox_Image_Extraction_End_Gray_G.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox_Image_Extraction_End_Gray_G.Location = new System.Drawing.Point(203, 106);
            this.textBox_Image_Extraction_End_Gray_G.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Image_Extraction_End_Gray_G.Name = "textBox_Image_Extraction_End_Gray_G";
            this.textBox_Image_Extraction_End_Gray_G.Size = new System.Drawing.Size(28, 20);
            this.textBox_Image_Extraction_End_Gray_G.TabIndex = 47;
            this.textBox_Image_Extraction_End_Gray_G.Text = "255";
            this.textBox_Image_Extraction_End_Gray_G.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Image_Extraction_Start_Gray_G
            // 
            this.textBox_Image_Extraction_Start_Gray_G.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox_Image_Extraction_Start_Gray_G.Location = new System.Drawing.Point(203, 84);
            this.textBox_Image_Extraction_Start_Gray_G.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Image_Extraction_Start_Gray_G.Name = "textBox_Image_Extraction_Start_Gray_G";
            this.textBox_Image_Extraction_Start_Gray_G.Size = new System.Drawing.Size(28, 20);
            this.textBox_Image_Extraction_Start_Gray_G.TabIndex = 46;
            this.textBox_Image_Extraction_Start_Gray_G.Text = "1";
            this.textBox_Image_Extraction_Start_Gray_G.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Image_Extraction_Foreground_As_White
            // 
            this.button_Image_Extraction_Foreground_As_White.BackColor = System.Drawing.Color.Black;
            this.button_Image_Extraction_Foreground_As_White.Location = new System.Drawing.Point(116, 41);
            this.button_Image_Extraction_Foreground_As_White.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Image_Extraction_Foreground_As_White.Name = "button_Image_Extraction_Foreground_As_White";
            this.button_Image_Extraction_Foreground_As_White.Size = new System.Drawing.Size(145, 25);
            this.button_Image_Extraction_Foreground_As_White.TabIndex = 45;
            this.button_Image_Extraction_Foreground_As_White.Text = "ForeGround As White";
            this.button_Image_Extraction_Foreground_As_White.UseVisualStyleBackColor = false;
            this.button_Image_Extraction_Foreground_As_White.Click += new System.EventHandler(this.button_Image_Extraction_Foreground_As_White_Click);
            // 
            // button_Image_Extraction_Background_As_Black
            // 
            this.button_Image_Extraction_Background_As_Black.BackColor = System.Drawing.Color.Black;
            this.button_Image_Extraction_Background_As_Black.Location = new System.Drawing.Point(116, 13);
            this.button_Image_Extraction_Background_As_Black.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Image_Extraction_Background_As_Black.Name = "button_Image_Extraction_Background_As_Black";
            this.button_Image_Extraction_Background_As_Black.Size = new System.Drawing.Size(145, 25);
            this.button_Image_Extraction_Background_As_Black.TabIndex = 44;
            this.button_Image_Extraction_Background_As_Black.Text = "BackGround As Black";
            this.button_Image_Extraction_Background_As_Black.UseVisualStyleBackColor = false;
            this.button_Image_Extraction_Background_As_Black.Click += new System.EventHandler(this.button_Image_Extraction_Background_As_Black_Click);
            // 
            // textBox_Image_Extraction_End_Gray_R
            // 
            this.textBox_Image_Extraction_End_Gray_R.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox_Image_Extraction_End_Gray_R.Location = new System.Drawing.Point(174, 106);
            this.textBox_Image_Extraction_End_Gray_R.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Image_Extraction_End_Gray_R.Name = "textBox_Image_Extraction_End_Gray_R";
            this.textBox_Image_Extraction_End_Gray_R.Size = new System.Drawing.Size(28, 20);
            this.textBox_Image_Extraction_End_Gray_R.TabIndex = 43;
            this.textBox_Image_Extraction_End_Gray_R.Text = "255";
            this.textBox_Image_Extraction_End_Gray_R.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Image_Extraction_Start_Gray_R
            // 
            this.textBox_Image_Extraction_Start_Gray_R.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox_Image_Extraction_Start_Gray_R.Location = new System.Drawing.Point(174, 84);
            this.textBox_Image_Extraction_Start_Gray_R.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Image_Extraction_Start_Gray_R.Name = "textBox_Image_Extraction_Start_Gray_R";
            this.textBox_Image_Extraction_Start_Gray_R.Size = new System.Drawing.Size(28, 20);
            this.textBox_Image_Extraction_Start_Gray_R.TabIndex = 42;
            this.textBox_Image_Extraction_Start_Gray_R.Text = "1";
            this.textBox_Image_Extraction_Start_Gray_R.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(111, 110);
            this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(65, 12);
            this.label26.TabIndex = 41;
            this.label26.Text = "End Gray : ";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(108, 91);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(68, 12);
            this.label27.TabIndex = 40;
            this.label27.Text = "Start Gray: ";
            // 
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.textBox_Image_Alpha_Value);
            this.groupBox25.Controls.Add(this.label42);
            this.groupBox25.Controls.Add(this.button_Change_Image_Alpha_Value);
            this.groupBox25.ForeColor = System.Drawing.Color.White;
            this.groupBox25.Location = new System.Drawing.Point(6, 747);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(106, 93);
            this.groupBox25.TabIndex = 290;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "Transpalency ";
            // 
            // textBox_Image_Alpha_Value
            // 
            this.textBox_Image_Alpha_Value.Location = new System.Drawing.Point(68, 20);
            this.textBox_Image_Alpha_Value.Name = "textBox_Image_Alpha_Value";
            this.textBox_Image_Alpha_Value.Size = new System.Drawing.Size(34, 20);
            this.textBox_Image_Alpha_Value.TabIndex = 72;
            this.textBox_Image_Alpha_Value.Text = "100";
            this.textBox_Image_Alpha_Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(6, 23);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(64, 12);
            this.label42.TabIndex = 71;
            this.label42.Text = "A(0~255) : ";
            // 
            // button_Change_Image_Alpha_Value
            // 
            this.button_Change_Image_Alpha_Value.BackColor = System.Drawing.Color.Black;
            this.button_Change_Image_Alpha_Value.ForeColor = System.Drawing.Color.White;
            this.button_Change_Image_Alpha_Value.Location = new System.Drawing.Point(6, 46);
            this.button_Change_Image_Alpha_Value.Name = "button_Change_Image_Alpha_Value";
            this.button_Change_Image_Alpha_Value.Size = new System.Drawing.Size(97, 40);
            this.button_Change_Image_Alpha_Value.TabIndex = 70;
            this.button_Change_Image_Alpha_Value.Text = "Change Transpalency";
            this.button_Change_Image_Alpha_Value.UseVisualStyleBackColor = false;
            this.button_Change_Image_Alpha_Value.Click += new System.EventHandler(this.button_Change_Image_Alpha_Value_Click);
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.button_Edge_Detection);
            this.groupBox27.Controls.Add(this.checkBox_225degree_Edge_Detection);
            this.groupBox27.Controls.Add(this.checkBox_45degree_Edge_Detection);
            this.groupBox27.Controls.Add(this.checkBox_Vertical_Edge_Detection);
            this.groupBox27.Controls.Add(this.checkBox_Horizontal_Edge_Detection);
            this.groupBox27.ForeColor = System.Drawing.Color.White;
            this.groupBox27.Location = new System.Drawing.Point(6, 843);
            this.groupBox27.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox27.Size = new System.Drawing.Size(265, 94);
            this.groupBox27.TabIndex = 51;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Edge Detection";
            // 
            // button_Edge_Detection
            // 
            this.button_Edge_Detection.BackColor = System.Drawing.Color.Black;
            this.button_Edge_Detection.Location = new System.Drawing.Point(5, 15);
            this.button_Edge_Detection.Name = "button_Edge_Detection";
            this.button_Edge_Detection.Size = new System.Drawing.Size(95, 68);
            this.button_Edge_Detection.TabIndex = 67;
            this.button_Edge_Detection.Text = "Edge Detection";
            this.button_Edge_Detection.UseVisualStyleBackColor = false;
            this.button_Edge_Detection.Click += new System.EventHandler(this.button_Edge_Detection_Click);
            // 
            // checkBox_225degree_Edge_Detection
            // 
            this.checkBox_225degree_Edge_Detection.AutoSize = true;
            this.checkBox_225degree_Edge_Detection.Location = new System.Drawing.Point(106, 67);
            this.checkBox_225degree_Edge_Detection.Name = "checkBox_225degree_Edge_Detection";
            this.checkBox_225degree_Edge_Detection.Size = new System.Drawing.Size(139, 16);
            this.checkBox_225degree_Edge_Detection.TabIndex = 44;
            this.checkBox_225degree_Edge_Detection.Text = "225 degree diagonal";
            this.checkBox_225degree_Edge_Detection.UseVisualStyleBackColor = true;
            // 
            // checkBox_45degree_Edge_Detection
            // 
            this.checkBox_45degree_Edge_Detection.AutoSize = true;
            this.checkBox_45degree_Edge_Detection.Location = new System.Drawing.Point(106, 48);
            this.checkBox_45degree_Edge_Detection.Name = "checkBox_45degree_Edge_Detection";
            this.checkBox_45degree_Edge_Detection.Size = new System.Drawing.Size(132, 16);
            this.checkBox_45degree_Edge_Detection.TabIndex = 43;
            this.checkBox_45degree_Edge_Detection.Text = "45 degree diagonal";
            this.checkBox_45degree_Edge_Detection.UseVisualStyleBackColor = true;
            // 
            // checkBox_Vertical_Edge_Detection
            // 
            this.checkBox_Vertical_Edge_Detection.AutoSize = true;
            this.checkBox_Vertical_Edge_Detection.Location = new System.Drawing.Point(106, 30);
            this.checkBox_Vertical_Edge_Detection.Name = "checkBox_Vertical_Edge_Detection";
            this.checkBox_Vertical_Edge_Detection.Size = new System.Drawing.Size(68, 16);
            this.checkBox_Vertical_Edge_Detection.TabIndex = 42;
            this.checkBox_Vertical_Edge_Detection.Text = "Vertical";
            this.checkBox_Vertical_Edge_Detection.UseVisualStyleBackColor = true;
            // 
            // checkBox_Horizontal_Edge_Detection
            // 
            this.checkBox_Horizontal_Edge_Detection.AutoSize = true;
            this.checkBox_Horizontal_Edge_Detection.Location = new System.Drawing.Point(106, 12);
            this.checkBox_Horizontal_Edge_Detection.Name = "checkBox_Horizontal_Edge_Detection";
            this.checkBox_Horizontal_Edge_Detection.Size = new System.Drawing.Size(81, 16);
            this.checkBox_Horizontal_Edge_Detection.TabIndex = 41;
            this.checkBox_Horizontal_Edge_Detection.Text = "Horizontal";
            this.checkBox_Horizontal_Edge_Detection.UseVisualStyleBackColor = true;
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.groupBox20);
            this.groupBox19.Controls.Add(this.button_Laplace_Sharpness_Filter);
            this.groupBox19.ForeColor = System.Drawing.Color.White;
            this.groupBox19.Location = new System.Drawing.Point(6, 411);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(265, 127);
            this.groupBox19.TabIndex = 45;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Sharpness Filter";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.textBox_SharpNessWeight);
            this.groupBox20.Controls.Add(this.label25);
            this.groupBox20.Controls.Add(this.button_HighBoost_Filter);
            this.groupBox20.ForeColor = System.Drawing.Color.White;
            this.groupBox20.Location = new System.Drawing.Point(6, 46);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(254, 77);
            this.groupBox20.TabIndex = 44;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Unsharp Mask (High Boost Filtering)";
            // 
            // textBox_SharpNessWeight
            // 
            this.textBox_SharpNessWeight.Location = new System.Drawing.Point(215, 48);
            this.textBox_SharpNessWeight.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_SharpNessWeight.Name = "textBox_SharpNessWeight";
            this.textBox_SharpNessWeight.Size = new System.Drawing.Size(32, 20);
            this.textBox_SharpNessWeight.TabIndex = 43;
            this.textBox_SharpNessWeight.Text = "1";
            this.textBox_SharpNessWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(99, 52);
            this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(112, 12);
            this.label25.TabIndex = 44;
            this.label25.Text = "SharpNess Weight: ";
            // 
            // button_HighBoost_Filter
            // 
            this.button_HighBoost_Filter.BackColor = System.Drawing.Color.Black;
            this.button_HighBoost_Filter.Location = new System.Drawing.Point(5, 19);
            this.button_HighBoost_Filter.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_HighBoost_Filter.Name = "button_HighBoost_Filter";
            this.button_HighBoost_Filter.Size = new System.Drawing.Size(244, 25);
            this.button_HighBoost_Filter.TabIndex = 42;
            this.button_HighBoost_Filter.Text = "High Boost Filter (with 5x5 Ave Filter)";
            this.button_HighBoost_Filter.UseVisualStyleBackColor = false;
            this.button_HighBoost_Filter.Click += new System.EventHandler(this.button_HighBoost_Filter_Click);
            // 
            // button_Laplace_Sharpness_Filter
            // 
            this.button_Laplace_Sharpness_Filter.BackColor = System.Drawing.Color.Black;
            this.button_Laplace_Sharpness_Filter.Location = new System.Drawing.Point(5, 17);
            this.button_Laplace_Sharpness_Filter.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Laplace_Sharpness_Filter.Name = "button_Laplace_Sharpness_Filter";
            this.button_Laplace_Sharpness_Filter.Size = new System.Drawing.Size(250, 25);
            this.button_Laplace_Sharpness_Filter.TabIndex = 41;
            this.button_Laplace_Sharpness_Filter.Text = "3x3 Laplace Sharpness Filter";
            this.button_Laplace_Sharpness_Filter.UseVisualStyleBackColor = false;
            this.button_Laplace_Sharpness_Filter.Click += new System.EventHandler(this.button_Laplace_Sharpness_Filter_Click);
            // 
            // groupBox29
            // 
            this.groupBox29.Controls.Add(this.button_Create_Dot_Noise);
            this.groupBox29.Controls.Add(this.textBox_Dot_B);
            this.groupBox29.Controls.Add(this.textBox_Dot_G);
            this.groupBox29.Controls.Add(this.textBox_Dot_R);
            this.groupBox29.Controls.Add(this.label41);
            this.groupBox29.Controls.Add(this.textBox_Dot_Num);
            this.groupBox29.Controls.Add(this.label40);
            this.groupBox29.ForeColor = System.Drawing.Color.White;
            this.groupBox29.Location = new System.Drawing.Point(116, 747);
            this.groupBox29.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox29.Name = "groupBox29";
            this.groupBox29.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox29.Size = new System.Drawing.Size(155, 93);
            this.groupBox29.TabIndex = 53;
            this.groupBox29.TabStop = false;
            this.groupBox29.Text = "Dot Noise Generation";
            // 
            // button_Create_Dot_Noise
            // 
            this.button_Create_Dot_Noise.BackColor = System.Drawing.Color.Black;
            this.button_Create_Dot_Noise.Location = new System.Drawing.Point(5, 15);
            this.button_Create_Dot_Noise.Name = "button_Create_Dot_Noise";
            this.button_Create_Dot_Noise.Size = new System.Drawing.Size(145, 25);
            this.button_Create_Dot_Noise.TabIndex = 66;
            this.button_Create_Dot_Noise.Text = "Create Dots Noise";
            this.button_Create_Dot_Noise.UseVisualStyleBackColor = false;
            this.button_Create_Dot_Noise.Click += new System.EventHandler(this.button_Create_Dot_Noise_Click);
            // 
            // textBox_Dot_B
            // 
            this.textBox_Dot_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox_Dot_B.Location = new System.Drawing.Point(123, 67);
            this.textBox_Dot_B.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Dot_B.Name = "textBox_Dot_B";
            this.textBox_Dot_B.Size = new System.Drawing.Size(28, 20);
            this.textBox_Dot_B.TabIndex = 65;
            this.textBox_Dot_B.Text = "255";
            this.textBox_Dot_B.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Dot_G
            // 
            this.textBox_Dot_G.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox_Dot_G.Location = new System.Drawing.Point(94, 67);
            this.textBox_Dot_G.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Dot_G.Name = "textBox_Dot_G";
            this.textBox_Dot_G.Size = new System.Drawing.Size(28, 20);
            this.textBox_Dot_G.TabIndex = 64;
            this.textBox_Dot_G.Text = "255";
            this.textBox_Dot_G.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Dot_R
            // 
            this.textBox_Dot_R.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox_Dot_R.Location = new System.Drawing.Point(65, 67);
            this.textBox_Dot_R.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Dot_R.Name = "textBox_Dot_R";
            this.textBox_Dot_R.Size = new System.Drawing.Size(28, 20);
            this.textBox_Dot_R.TabIndex = 63;
            this.textBox_Dot_R.Text = "255";
            this.textBox_Dot_R.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(24, 71);
            this.label41.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(44, 12);
            this.label41.TabIndex = 62;
            this.label41.Text = "Color : ";
            // 
            // textBox_Dot_Num
            // 
            this.textBox_Dot_Num.Location = new System.Drawing.Point(101, 43);
            this.textBox_Dot_Num.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Dot_Num.Name = "textBox_Dot_Num";
            this.textBox_Dot_Num.Size = new System.Drawing.Size(49, 20);
            this.textBox_Dot_Num.TabIndex = 61;
            this.textBox_Dot_Num.Text = "600";
            this.textBox_Dot_Num.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(45, 46);
            this.label40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(58, 12);
            this.label40.TabIndex = 41;
            this.label40.Text = "Dot Num :";
            // 
            // button_Bit_Inversion
            // 
            this.button_Bit_Inversion.BackColor = System.Drawing.Color.Black;
            this.button_Bit_Inversion.Location = new System.Drawing.Point(106, 291);
            this.button_Bit_Inversion.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Bit_Inversion.Name = "button_Bit_Inversion";
            this.button_Bit_Inversion.Size = new System.Drawing.Size(165, 25);
            this.button_Bit_Inversion.TabIndex = 44;
            this.button_Bit_Inversion.Text = "Image bit Inversion";
            this.button_Bit_Inversion.UseVisualStyleBackColor = false;
            this.button_Bit_Inversion.Click += new System.EventHandler(this.button_Bit_Inversion_Click);
            // 
            // button_histogram_equalization
            // 
            this.button_histogram_equalization.BackColor = System.Drawing.Color.Black;
            this.button_histogram_equalization.Location = new System.Drawing.Point(6, 291);
            this.button_histogram_equalization.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_histogram_equalization.Name = "button_histogram_equalization";
            this.button_histogram_equalization.Size = new System.Drawing.Size(96, 25);
            this.button_histogram_equalization.TabIndex = 43;
            this.button_histogram_equalization.Text = "Equalization";
            this.button_histogram_equalization.UseVisualStyleBackColor = false;
            this.button_histogram_equalization.Click += new System.EventHandler(this.button_histogram_equalization_Click);
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.button_5x5_Meadian_Filter);
            this.groupBox18.Controls.Add(this.button_3x3_Meadian_Filter);
            this.groupBox18.Controls.Add(this.button_5x5_Ave_Filter);
            this.groupBox18.Controls.Add(this.button_3x3_Ave_Filter);
            this.groupBox18.ForeColor = System.Drawing.Color.White;
            this.groupBox18.Location = new System.Drawing.Point(6, 322);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(265, 83);
            this.groupBox18.TabIndex = 43;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Smoothing Filter";
            // 
            // button_5x5_Meadian_Filter
            // 
            this.button_5x5_Meadian_Filter.BackColor = System.Drawing.Color.Black;
            this.button_5x5_Meadian_Filter.Location = new System.Drawing.Point(133, 49);
            this.button_5x5_Meadian_Filter.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_5x5_Meadian_Filter.Name = "button_5x5_Meadian_Filter";
            this.button_5x5_Meadian_Filter.Size = new System.Drawing.Size(115, 25);
            this.button_5x5_Meadian_Filter.TabIndex = 44;
            this.button_5x5_Meadian_Filter.Text = "5x5 Median Filter";
            this.button_5x5_Meadian_Filter.UseVisualStyleBackColor = false;
            this.button_5x5_Meadian_Filter.Click += new System.EventHandler(this.button_5x5_Meadian_Filter_Click);
            // 
            // button_3x3_Meadian_Filter
            // 
            this.button_3x3_Meadian_Filter.BackColor = System.Drawing.Color.Black;
            this.button_3x3_Meadian_Filter.Location = new System.Drawing.Point(133, 18);
            this.button_3x3_Meadian_Filter.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_3x3_Meadian_Filter.Name = "button_3x3_Meadian_Filter";
            this.button_3x3_Meadian_Filter.Size = new System.Drawing.Size(115, 25);
            this.button_3x3_Meadian_Filter.TabIndex = 43;
            this.button_3x3_Meadian_Filter.Text = "3x3 Median Filter";
            this.button_3x3_Meadian_Filter.UseVisualStyleBackColor = false;
            this.button_3x3_Meadian_Filter.Click += new System.EventHandler(this.button_3x3_Meadian_Filter_Click);
            // 
            // button_5x5_Ave_Filter
            // 
            this.button_5x5_Ave_Filter.BackColor = System.Drawing.Color.Black;
            this.button_5x5_Ave_Filter.Location = new System.Drawing.Point(14, 49);
            this.button_5x5_Ave_Filter.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_5x5_Ave_Filter.Name = "button_5x5_Ave_Filter";
            this.button_5x5_Ave_Filter.Size = new System.Drawing.Size(115, 25);
            this.button_5x5_Ave_Filter.TabIndex = 42;
            this.button_5x5_Ave_Filter.Text = "5x5 Average Filter";
            this.button_5x5_Ave_Filter.UseVisualStyleBackColor = false;
            this.button_5x5_Ave_Filter.Click += new System.EventHandler(this.button_5x5_Ave_Filter_Click);
            // 
            // button_3x3_Ave_Filter
            // 
            this.button_3x3_Ave_Filter.BackColor = System.Drawing.Color.Black;
            this.button_3x3_Ave_Filter.Location = new System.Drawing.Point(14, 18);
            this.button_3x3_Ave_Filter.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_3x3_Ave_Filter.Name = "button_3x3_Ave_Filter";
            this.button_3x3_Ave_Filter.Size = new System.Drawing.Size(115, 25);
            this.button_3x3_Ave_Filter.TabIndex = 41;
            this.button_3x3_Ave_Filter.Text = "3x3 Average Filter";
            this.button_3x3_Ave_Filter.UseVisualStyleBackColor = false;
            this.button_3x3_Ave_Filter.Click += new System.EventHandler(this.button_3x3_Ave_Filter_Click);
            // 
            // button_Save_Current_Image
            // 
            this.button_Save_Current_Image.BackColor = System.Drawing.Color.Black;
            this.button_Save_Current_Image.Location = new System.Drawing.Point(106, 260);
            this.button_Save_Current_Image.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Save_Current_Image.Name = "button_Save_Current_Image";
            this.button_Save_Current_Image.Size = new System.Drawing.Size(165, 25);
            this.button_Save_Current_Image.TabIndex = 42;
            this.button_Save_Current_Image.Text = "Save Current Image";
            this.button_Save_Current_Image.UseVisualStyleBackColor = false;
            this.button_Save_Current_Image.Click += new System.EventHandler(this.button_Save_Current_Image_Click);
            // 
            // button_RGB_to_Gray
            // 
            this.button_RGB_to_Gray.BackColor = System.Drawing.Color.Black;
            this.button_RGB_to_Gray.Location = new System.Drawing.Point(6, 260);
            this.button_RGB_to_Gray.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_RGB_to_Gray.Name = "button_RGB_to_Gray";
            this.button_RGB_to_Gray.Size = new System.Drawing.Size(96, 25);
            this.button_RGB_to_Gray.TabIndex = 41;
            this.button_RGB_to_Gray.Text = "RGB to Gray";
            this.button_RGB_to_Gray.UseVisualStyleBackColor = false;
            this.button_RGB_to_Gray.Click += new System.EventHandler(this.button_RGB_to_Gray_Click);
            // 
            // button_Back_To_Original
            // 
            this.button_Back_To_Original.BackColor = System.Drawing.Color.Black;
            this.button_Back_To_Original.Location = new System.Drawing.Point(11, 17);
            this.button_Back_To_Original.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Back_To_Original.Name = "button_Back_To_Original";
            this.button_Back_To_Original.Size = new System.Drawing.Size(255, 25);
            this.button_Back_To_Original.TabIndex = 40;
            this.button_Back_To_Original.Text = "Back to Originally loaded BMP";
            this.button_Back_To_Original.UseVisualStyleBackColor = false;
            this.button_Back_To_Original.Click += new System.EventHandler(this.button_Back_To_Original_Click);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.button_Change_Gray_Resolution);
            this.groupBox16.Controls.Add(this.numericUpDown_gray_resolution_bits);
            this.groupBox16.Controls.Add(this.label24);
            this.groupBox16.ForeColor = System.Drawing.Color.White;
            this.groupBox16.Location = new System.Drawing.Point(6, 211);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(265, 43);
            this.groupBox16.TabIndex = 39;
            this.groupBox16.TabStop = false;
            // 
            // button_Change_Gray_Resolution
            // 
            this.button_Change_Gray_Resolution.BackColor = System.Drawing.Color.Black;
            this.button_Change_Gray_Resolution.Location = new System.Drawing.Point(6, 12);
            this.button_Change_Gray_Resolution.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Change_Gray_Resolution.Name = "button_Change_Gray_Resolution";
            this.button_Change_Gray_Resolution.Size = new System.Drawing.Size(178, 25);
            this.button_Change_Gray_Resolution.TabIndex = 41;
            this.button_Change_Gray_Resolution.Text = "Change Gray Resolution";
            this.button_Change_Gray_Resolution.UseVisualStyleBackColor = false;
            this.button_Change_Gray_Resolution.Click += new System.EventHandler(this.button_Change_Gray_Resolution_Click);
            // 
            // numericUpDown_gray_resolution_bits
            // 
            this.numericUpDown_gray_resolution_bits.Location = new System.Drawing.Point(228, 12);
            this.numericUpDown_gray_resolution_bits.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDown_gray_resolution_bits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_gray_resolution_bits.Name = "numericUpDown_gray_resolution_bits";
            this.numericUpDown_gray_resolution_bits.Size = new System.Drawing.Size(29, 20);
            this.numericUpDown_gray_resolution_bits.TabIndex = 45;
            this.numericUpDown_gray_resolution_bits.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(188, 16);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(35, 12);
            this.label24.TabIndex = 44;
            this.label24.Text = "Bits : ";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.button_Gamma_Decoding);
            this.groupBox15.Controls.Add(this.textBox_Gamma);
            this.groupBox15.Controls.Add(this.label23);
            this.groupBox15.Controls.Add(this.button_Gamma_Encoding);
            this.groupBox15.ForeColor = System.Drawing.Color.White;
            this.groupBox15.Location = new System.Drawing.Point(6, 136);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(265, 74);
            this.groupBox15.TabIndex = 39;
            this.groupBox15.TabStop = false;
            // 
            // button_Gamma_Decoding
            // 
            this.button_Gamma_Decoding.BackColor = System.Drawing.Color.DarkRed;
            this.button_Gamma_Decoding.Location = new System.Drawing.Point(5, 42);
            this.button_Gamma_Decoding.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Gamma_Decoding.Name = "button_Gamma_Decoding";
            this.button_Gamma_Decoding.Size = new System.Drawing.Size(136, 25);
            this.button_Gamma_Decoding.TabIndex = 43;
            this.button_Gamma_Decoding.Text = "Gamma Decoding";
            this.button_Gamma_Decoding.UseVisualStyleBackColor = false;
            this.button_Gamma_Decoding.Click += new System.EventHandler(this.button_Gamma_Decoding_Click);
            // 
            // textBox_Gamma
            // 
            this.textBox_Gamma.Location = new System.Drawing.Point(221, 25);
            this.textBox_Gamma.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Gamma.Name = "textBox_Gamma";
            this.textBox_Gamma.Size = new System.Drawing.Size(32, 20);
            this.textBox_Gamma.TabIndex = 41;
            this.textBox_Gamma.Text = "0.5";
            this.textBox_Gamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(160, 28);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(57, 12);
            this.label23.TabIndex = 42;
            this.label23.Text = "Gamma : ";
            // 
            // button_Gamma_Encoding
            // 
            this.button_Gamma_Encoding.BackColor = System.Drawing.Color.Navy;
            this.button_Gamma_Encoding.Location = new System.Drawing.Point(5, 11);
            this.button_Gamma_Encoding.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Gamma_Encoding.Name = "button_Gamma_Encoding";
            this.button_Gamma_Encoding.Size = new System.Drawing.Size(136, 25);
            this.button_Gamma_Encoding.TabIndex = 41;
            this.button_Gamma_Encoding.Text = "Gamma Encoding";
            this.button_Gamma_Encoding.UseVisualStyleBackColor = false;
            this.button_Gamma_Encoding.Click += new System.EventHandler(this.button_Gamma_Correction_Click);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label29);
            this.groupBox14.Controls.Add(this.label28);
            this.groupBox14.Controls.Add(this.textBox_Clamping_Max_Gray_B);
            this.groupBox14.Controls.Add(this.textBox_Clamping_Max_Gray_G);
            this.groupBox14.Controls.Add(this.textBox_Clamping_Max_Gray_R);
            this.groupBox14.Controls.Add(this.label22);
            this.groupBox14.Controls.Add(this.button_Clamp_Max_Gray);
            this.groupBox14.ForeColor = System.Drawing.Color.White;
            this.groupBox14.Location = new System.Drawing.Point(6, 42);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(265, 88);
            this.groupBox14.TabIndex = 38;
            this.groupBox14.TabStop = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(201, 42);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(23, 12);
            this.label29.TabIndex = 44;
            this.label29.Text = "G : ";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(202, 63);
            this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(22, 12);
            this.label28.TabIndex = 43;
            this.label28.Text = "B : ";
            // 
            // textBox_Clamping_Max_Gray_B
            // 
            this.textBox_Clamping_Max_Gray_B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox_Clamping_Max_Gray_B.Location = new System.Drawing.Point(225, 60);
            this.textBox_Clamping_Max_Gray_B.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Clamping_Max_Gray_B.Name = "textBox_Clamping_Max_Gray_B";
            this.textBox_Clamping_Max_Gray_B.Size = new System.Drawing.Size(32, 20);
            this.textBox_Clamping_Max_Gray_B.TabIndex = 42;
            this.textBox_Clamping_Max_Gray_B.Text = "200";
            this.textBox_Clamping_Max_Gray_B.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Clamping_Max_Gray_G
            // 
            this.textBox_Clamping_Max_Gray_G.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox_Clamping_Max_Gray_G.Location = new System.Drawing.Point(225, 38);
            this.textBox_Clamping_Max_Gray_G.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Clamping_Max_Gray_G.Name = "textBox_Clamping_Max_Gray_G";
            this.textBox_Clamping_Max_Gray_G.Size = new System.Drawing.Size(32, 20);
            this.textBox_Clamping_Max_Gray_G.TabIndex = 41;
            this.textBox_Clamping_Max_Gray_G.Text = "200";
            this.textBox_Clamping_Max_Gray_G.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Clamping_Max_Gray_R
            // 
            this.textBox_Clamping_Max_Gray_R.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox_Clamping_Max_Gray_R.Location = new System.Drawing.Point(225, 17);
            this.textBox_Clamping_Max_Gray_R.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Clamping_Max_Gray_R.Name = "textBox_Clamping_Max_Gray_R";
            this.textBox_Clamping_Max_Gray_R.Size = new System.Drawing.Size(32, 20);
            this.textBox_Clamping_Max_Gray_R.TabIndex = 40;
            this.textBox_Clamping_Max_Gray_R.Text = "200";
            this.textBox_Clamping_Max_Gray_R.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(201, 21);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(22, 12);
            this.label22.TabIndex = 13;
            this.label22.Text = "R : ";
            // 
            // button_Clamp_Max_Gray
            // 
            this.button_Clamp_Max_Gray.BackColor = System.Drawing.Color.Black;
            this.button_Clamp_Max_Gray.Location = new System.Drawing.Point(5, 12);
            this.button_Clamp_Max_Gray.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Clamp_Max_Gray.Name = "button_Clamp_Max_Gray";
            this.button_Clamp_Max_Gray.Size = new System.Drawing.Size(177, 70);
            this.button_Clamp_Max_Gray.TabIndex = 12;
            this.button_Clamp_Max_Gray.Text = "Clamp the Max Gray (<=255)";
            this.button_Clamp_Max_Gray.UseVisualStyleBackColor = false;
            this.button_Clamp_Max_Gray.Click += new System.EventHandler(this.button_Clamp_Max_Gray_Click);
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.label_Resolution);
            this.groupBox17.Controls.Add(this.label_PSNR);
            this.groupBox17.Controls.Add(this.button1);
            this.groupBox17.ForeColor = System.Drawing.Color.White;
            this.groupBox17.Location = new System.Drawing.Point(736, 10);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(218, 87);
            this.groupBox17.TabIndex = 39;
            this.groupBox17.TabStop = false;
            // 
            // label_Resolution
            // 
            this.label_Resolution.AutoSize = true;
            this.label_Resolution.Location = new System.Drawing.Point(10, 43);
            this.label_Resolution.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Resolution.Name = "label_Resolution";
            this.label_Resolution.Size = new System.Drawing.Size(101, 12);
            this.label_Resolution.TabIndex = 16;
            this.label_Resolution.Text = "Resolution (X,Y) : ";
            // 
            // label_PSNR
            // 
            this.label_PSNR.AutoSize = true;
            this.label_PSNR.Location = new System.Drawing.Point(10, 66);
            this.label_PSNR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_PSNR.Name = "label_PSNR";
            this.label_PSNR.Size = new System.Drawing.Size(44, 12);
            this.label_PSNR.TabIndex = 13;
            this.label_PSNR.Text = "PSNR : ";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(5, 12);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 25);
            this.button1.TabIndex = 12;
            this.button1.Text = "Get BMP Info";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.label_1st_2nd_Resolution);
            this.groupBox21.Controls.Add(this.label_1st_2nd_PSNR);
            this.groupBox21.Controls.Add(this.button_Get_1st_2nd_PSNR);
            this.groupBox21.Controls.Add(this.button_2nd_BMP_Load);
            this.groupBox21.Controls.Add(this.pictureBox_2nd_BMP);
            this.groupBox21.Controls.Add(this.button_1st_BMP_Load);
            this.groupBox21.Controls.Add(this.pictureBox_1st_BMP);
            this.groupBox21.ForeColor = System.Drawing.Color.White;
            this.groupBox21.Location = new System.Drawing.Point(483, 719);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(472, 229);
            this.groupBox21.TabIndex = 40;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Compare Two Image";
            // 
            // label_1st_2nd_Resolution
            // 
            this.label_1st_2nd_Resolution.AutoSize = true;
            this.label_1st_2nd_Resolution.Location = new System.Drawing.Point(5, 73);
            this.label_1st_2nd_Resolution.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_1st_2nd_Resolution.Name = "label_1st_2nd_Resolution";
            this.label_1st_2nd_Resolution.Size = new System.Drawing.Size(101, 12);
            this.label_1st_2nd_Resolution.TabIndex = 15;
            this.label_1st_2nd_Resolution.Text = "Resolution (X,Y) : ";
            // 
            // label_1st_2nd_PSNR
            // 
            this.label_1st_2nd_PSNR.AutoSize = true;
            this.label_1st_2nd_PSNR.Location = new System.Drawing.Point(5, 52);
            this.label_1st_2nd_PSNR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_1st_2nd_PSNR.Name = "label_1st_2nd_PSNR";
            this.label_1st_2nd_PSNR.Size = new System.Drawing.Size(44, 12);
            this.label_1st_2nd_PSNR.TabIndex = 14;
            this.label_1st_2nd_PSNR.Text = "PSNR : ";
            // 
            // button_Get_1st_2nd_PSNR
            // 
            this.button_Get_1st_2nd_PSNR.BackColor = System.Drawing.Color.Black;
            this.button_Get_1st_2nd_PSNR.Location = new System.Drawing.Point(5, 20);
            this.button_Get_1st_2nd_PSNR.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Get_1st_2nd_PSNR.Name = "button_Get_1st_2nd_PSNR";
            this.button_Get_1st_2nd_PSNR.Size = new System.Drawing.Size(101, 25);
            this.button_Get_1st_2nd_PSNR.TabIndex = 13;
            this.button_Get_1st_2nd_PSNR.Text = "Get BMP Info";
            this.button_Get_1st_2nd_PSNR.UseVisualStyleBackColor = false;
            this.button_Get_1st_2nd_PSNR.Click += new System.EventHandler(this.button_Get_1st_2nd_PSNR_Click);
            // 
            // button_2nd_BMP_Load
            // 
            this.button_2nd_BMP_Load.BackColor = System.Drawing.Color.Teal;
            this.button_2nd_BMP_Load.ForeColor = System.Drawing.Color.White;
            this.button_2nd_BMP_Load.Location = new System.Drawing.Point(362, 13);
            this.button_2nd_BMP_Load.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_2nd_BMP_Load.Name = "button_2nd_BMP_Load";
            this.button_2nd_BMP_Load.Size = new System.Drawing.Size(100, 26);
            this.button_2nd_BMP_Load.TabIndex = 11;
            this.button_2nd_BMP_Load.Text = "2nd BMP load";
            this.button_2nd_BMP_Load.UseVisualStyleBackColor = false;
            this.button_2nd_BMP_Load.Click += new System.EventHandler(this.button_2nd_BMP_Load_Click);
            // 
            // pictureBox_2nd_BMP
            // 
            this.pictureBox_2nd_BMP.Location = new System.Drawing.Point(362, 45);
            this.pictureBox_2nd_BMP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_2nd_BMP.Name = "pictureBox_2nd_BMP";
            this.pictureBox_2nd_BMP.Size = new System.Drawing.Size(100, 174);
            this.pictureBox_2nd_BMP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_2nd_BMP.TabIndex = 10;
            this.pictureBox_2nd_BMP.TabStop = false;
            this.pictureBox_2nd_BMP.Click += new System.EventHandler(this.pictureBox_2nd_BMP_Click);
            // 
            // button_1st_BMP_Load
            // 
            this.button_1st_BMP_Load.BackColor = System.Drawing.Color.Purple;
            this.button_1st_BMP_Load.ForeColor = System.Drawing.Color.White;
            this.button_1st_BMP_Load.Location = new System.Drawing.Point(258, 13);
            this.button_1st_BMP_Load.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_1st_BMP_Load.Name = "button_1st_BMP_Load";
            this.button_1st_BMP_Load.Size = new System.Drawing.Size(100, 26);
            this.button_1st_BMP_Load.TabIndex = 9;
            this.button_1st_BMP_Load.Text = "1st BMP load";
            this.button_1st_BMP_Load.UseVisualStyleBackColor = false;
            this.button_1st_BMP_Load.Click += new System.EventHandler(this.button_1st_BMP_Load_Click);
            // 
            // pictureBox_1st_BMP
            // 
            this.pictureBox_1st_BMP.Location = new System.Drawing.Point(258, 45);
            this.pictureBox_1st_BMP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_1st_BMP.Name = "pictureBox_1st_BMP";
            this.pictureBox_1st_BMP.Size = new System.Drawing.Size(100, 174);
            this.pictureBox_1st_BMP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_1st_BMP.TabIndex = 7;
            this.pictureBox_1st_BMP.TabStop = false;
            this.pictureBox_1st_BMP.Click += new System.EventHandler(this.pictureBox_1st_BMP_Click);
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.button_RGB_to_CYM);
            this.groupBox23.Controls.Add(this.pictureBox_Black);
            this.groupBox23.Controls.Add(this.label34);
            this.groupBox23.Controls.Add(this.label33);
            this.groupBox23.Controls.Add(this.label32);
            this.groupBox23.Controls.Add(this.label31);
            this.groupBox23.Controls.Add(this.label30);
            this.groupBox23.Controls.Add(this.pictureBox_Magenta);
            this.groupBox23.Controls.Add(this.pictureBox_Yellow);
            this.groupBox23.Controls.Add(this.pictureBox_Cyan);
            this.groupBox23.Controls.Add(this.pictureBox_CYM);
            this.groupBox23.Controls.Add(this.button_RGB_to_CYMK);
            this.groupBox23.ForeColor = System.Drawing.Color.White;
            this.groupBox23.Location = new System.Drawing.Point(735, 302);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(220, 225);
            this.groupBox23.TabIndex = 47;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "RGB to CYM(K) Conversion";
            // 
            // button_RGB_to_CYM
            // 
            this.button_RGB_to_CYM.BackColor = System.Drawing.Color.Black;
            this.button_RGB_to_CYM.Location = new System.Drawing.Point(6, 20);
            this.button_RGB_to_CYM.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_RGB_to_CYM.Name = "button_RGB_to_CYM";
            this.button_RGB_to_CYM.Size = new System.Drawing.Size(104, 25);
            this.button_RGB_to_CYM.TabIndex = 54;
            this.button_RGB_to_CYM.Text = "RGB to CYM";
            this.button_RGB_to_CYM.UseVisualStyleBackColor = false;
            this.button_RGB_to_CYM.Click += new System.EventHandler(this.button_RGB_to_CYM_Click);
            // 
            // pictureBox_Black
            // 
            this.pictureBox_Black.Location = new System.Drawing.Point(163, 150);
            this.pictureBox_Black.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_Black.Name = "pictureBox_Black";
            this.pictureBox_Black.Size = new System.Drawing.Size(49, 66);
            this.pictureBox_Black.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Black.TabIndex = 53;
            this.pictureBox_Black.TabStop = false;
            this.pictureBox_Black.Click += new System.EventHandler(this.pictureBox_Black_Click);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(161, 134);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(52, 12);
            this.label34.TabIndex = 52;
            this.label34.Text = "K(Black)";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(161, 48);
            this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(53, 12);
            this.label33.TabIndex = 51;
            this.label33.Text = "Magenta";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(110, 134);
            this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(41, 12);
            this.label32.TabIndex = 50;
            this.label32.Text = "Yellow";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(118, 48);
            this.label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(33, 12);
            this.label31.TabIndex = 49;
            this.label31.Text = "Cyan";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(17, 48);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(79, 12);
            this.label30.TabIndex = 48;
            this.label30.Text = "CYM or CYMK";
            // 
            // pictureBox_Magenta
            // 
            this.pictureBox_Magenta.Location = new System.Drawing.Point(163, 64);
            this.pictureBox_Magenta.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_Magenta.Name = "pictureBox_Magenta";
            this.pictureBox_Magenta.Size = new System.Drawing.Size(49, 66);
            this.pictureBox_Magenta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Magenta.TabIndex = 47;
            this.pictureBox_Magenta.TabStop = false;
            this.pictureBox_Magenta.Click += new System.EventHandler(this.pictureBox_Magenta_Click);
            // 
            // pictureBox_Yellow
            // 
            this.pictureBox_Yellow.Location = new System.Drawing.Point(110, 149);
            this.pictureBox_Yellow.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_Yellow.Name = "pictureBox_Yellow";
            this.pictureBox_Yellow.Size = new System.Drawing.Size(49, 66);
            this.pictureBox_Yellow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Yellow.TabIndex = 46;
            this.pictureBox_Yellow.TabStop = false;
            this.pictureBox_Yellow.Click += new System.EventHandler(this.pictureBox_Yellow_Click);
            // 
            // pictureBox_Cyan
            // 
            this.pictureBox_Cyan.Location = new System.Drawing.Point(110, 64);
            this.pictureBox_Cyan.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_Cyan.Name = "pictureBox_Cyan";
            this.pictureBox_Cyan.Size = new System.Drawing.Size(49, 66);
            this.pictureBox_Cyan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Cyan.TabIndex = 45;
            this.pictureBox_Cyan.TabStop = false;
            this.pictureBox_Cyan.Click += new System.EventHandler(this.pictureBox_Cyan_Click);
            // 
            // pictureBox_CYM
            // 
            this.pictureBox_CYM.Location = new System.Drawing.Point(6, 64);
            this.pictureBox_CYM.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_CYM.Name = "pictureBox_CYM";
            this.pictureBox_CYM.Size = new System.Drawing.Size(100, 149);
            this.pictureBox_CYM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_CYM.TabIndex = 16;
            this.pictureBox_CYM.TabStop = false;
            this.pictureBox_CYM.Click += new System.EventHandler(this.pictureBox_CYM_Click);
            // 
            // button_RGB_to_CYMK
            // 
            this.button_RGB_to_CYMK.BackColor = System.Drawing.Color.Black;
            this.button_RGB_to_CYMK.Location = new System.Drawing.Point(113, 20);
            this.button_RGB_to_CYMK.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_RGB_to_CYMK.Name = "button_RGB_to_CYMK";
            this.button_RGB_to_CYMK.Size = new System.Drawing.Size(96, 25);
            this.button_RGB_to_CYMK.TabIndex = 44;
            this.button_RGB_to_CYMK.Text = "RGB to CYMK";
            this.button_RGB_to_CYMK.UseVisualStyleBackColor = false;
            this.button_RGB_to_CYMK.Click += new System.EventHandler(this.button_RGB_to_CYMK_Click);
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.label_Calculated_Color_Temperature);
            this.groupBox24.Controls.Add(this.label_Ave_xy);
            this.groupBox24.Controls.Add(this.label_Ave_RGB);
            this.groupBox24.Controls.Add(this.pictureBox_CIE_XY_Selected_Area_Ave_Color);
            this.groupBox24.Controls.Add(this.pictureBox_CIE_XY_Selected_Area);
            this.groupBox24.Controls.Add(this.textBox_xy_To_Col);
            this.groupBox24.Controls.Add(this.textBox_xy_To_Row);
            this.groupBox24.Controls.Add(this.textBox_xy_From_Col);
            this.groupBox24.Controls.Add(this.textBox_xy_From_Row);
            this.groupBox24.Controls.Add(this.label35);
            this.groupBox24.Controls.Add(this.label36);
            this.groupBox24.Controls.Add(this.button_Get_Average_ColorCoordinate_xy);
            this.groupBox24.ForeColor = System.Drawing.Color.White;
            this.groupBox24.Location = new System.Drawing.Point(4, 532);
            this.groupBox24.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox24.Size = new System.Drawing.Size(251, 181);
            this.groupBox24.TabIndex = 48;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "CIE XYZ (X + Y + Z = 1)";
            // 
            // label_Calculated_Color_Temperature
            // 
            this.label_Calculated_Color_Temperature.AutoSize = true;
            this.label_Calculated_Color_Temperature.Location = new System.Drawing.Point(6, 154);
            this.label_Calculated_Color_Temperature.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Calculated_Color_Temperature.Name = "label_Calculated_Color_Temperature";
            this.label_Calculated_Color_Temperature.Size = new System.Drawing.Size(40, 12);
            this.label_Calculated_Color_Temperature.TabIndex = 65;
            this.label_Calculated_Color_Temperature.Text = "CCT :  ";
            // 
            // label_Ave_xy
            // 
            this.label_Ave_xy.AutoSize = true;
            this.label_Ave_xy.Location = new System.Drawing.Point(6, 134);
            this.label_Ave_xy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Ave_xy.Name = "label_Ave_xy";
            this.label_Ave_xy.Size = new System.Drawing.Size(64, 12);
            this.label_Ave_xy.TabIndex = 64;
            this.label_Ave_xy.Text = "Ave(x, y) :  ";
            // 
            // label_Ave_RGB
            // 
            this.label_Ave_RGB.AutoSize = true;
            this.label_Ave_RGB.Location = new System.Drawing.Point(6, 114);
            this.label_Ave_RGB.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Ave_RGB.Name = "label_Ave_RGB";
            this.label_Ave_RGB.Size = new System.Drawing.Size(72, 12);
            this.label_Ave_RGB.TabIndex = 63;
            this.label_Ave_RGB.Text = "Ave R/G/B :  ";
            // 
            // pictureBox_CIE_XY_Selected_Area_Ave_Color
            // 
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color.Location = new System.Drawing.Point(176, 97);
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color.Name = "pictureBox_CIE_XY_Selected_Area_Ave_Color";
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color.Size = new System.Drawing.Size(71, 78);
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color.TabIndex = 62;
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color.TabStop = false;
            this.pictureBox_CIE_XY_Selected_Area_Ave_Color.Click += new System.EventHandler(this.pictureBox_CIE_XY_Selected_Area_Ave_Color_Click);
            // 
            // pictureBox_CIE_XY_Selected_Area
            // 
            this.pictureBox_CIE_XY_Selected_Area.Location = new System.Drawing.Point(176, 16);
            this.pictureBox_CIE_XY_Selected_Area.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox_CIE_XY_Selected_Area.Name = "pictureBox_CIE_XY_Selected_Area";
            this.pictureBox_CIE_XY_Selected_Area.Size = new System.Drawing.Size(71, 76);
            this.pictureBox_CIE_XY_Selected_Area.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_CIE_XY_Selected_Area.TabIndex = 49;
            this.pictureBox_CIE_XY_Selected_Area.TabStop = false;
            this.pictureBox_CIE_XY_Selected_Area.Click += new System.EventHandler(this.pictureBox_CIE_XY_Selected_Area_Click);
            // 
            // textBox_xy_To_Col
            // 
            this.textBox_xy_To_Col.Location = new System.Drawing.Point(74, 82);
            this.textBox_xy_To_Col.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_xy_To_Col.Name = "textBox_xy_To_Col";
            this.textBox_xy_To_Col.Size = new System.Drawing.Size(40, 20);
            this.textBox_xy_To_Col.TabIndex = 61;
            this.textBox_xy_To_Col.Text = "500";
            this.textBox_xy_To_Col.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_xy_To_Row
            // 
            this.textBox_xy_To_Row.Location = new System.Drawing.Point(118, 82);
            this.textBox_xy_To_Row.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_xy_To_Row.Name = "textBox_xy_To_Row";
            this.textBox_xy_To_Row.Size = new System.Drawing.Size(40, 20);
            this.textBox_xy_To_Row.TabIndex = 60;
            this.textBox_xy_To_Row.Text = "600";
            this.textBox_xy_To_Row.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_xy_From_Col
            // 
            this.textBox_xy_From_Col.Location = new System.Drawing.Point(74, 60);
            this.textBox_xy_From_Col.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_xy_From_Col.Name = "textBox_xy_From_Col";
            this.textBox_xy_From_Col.Size = new System.Drawing.Size(40, 20);
            this.textBox_xy_From_Col.TabIndex = 59;
            this.textBox_xy_From_Col.Text = "300";
            this.textBox_xy_From_Col.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_xy_From_Row
            // 
            this.textBox_xy_From_Row.Location = new System.Drawing.Point(118, 60);
            this.textBox_xy_From_Row.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_xy_From_Row.Name = "textBox_xy_From_Row";
            this.textBox_xy_From_Row.Size = new System.Drawing.Size(40, 20);
            this.textBox_xy_From_Row.TabIndex = 58;
            this.textBox_xy_From_Row.Text = "250";
            this.textBox_xy_From_Row.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(23, 85);
            this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(51, 12);
            this.label35.TabIndex = 57;
            this.label35.Text = "To(x,y) : ";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(8, 63);
            this.label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(66, 12);
            this.label36.TabIndex = 56;
            this.label36.Text = "From(x,y) : ";
            // 
            // button_Get_Average_ColorCoordinate_xy
            // 
            this.button_Get_Average_ColorCoordinate_xy.BackColor = System.Drawing.Color.Black;
            this.button_Get_Average_ColorCoordinate_xy.Location = new System.Drawing.Point(6, 19);
            this.button_Get_Average_ColorCoordinate_xy.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Get_Average_ColorCoordinate_xy.Name = "button_Get_Average_ColorCoordinate_xy";
            this.button_Get_Average_ColorCoordinate_xy.Size = new System.Drawing.Size(152, 35);
            this.button_Get_Average_ColorCoordinate_xy.TabIndex = 55;
            this.button_Get_Average_ColorCoordinate_xy.Text = "Get Ave Color Coordinate(x,y)";
            this.button_Get_Average_ColorCoordinate_xy.UseVisualStyleBackColor = false;
            this.button_Get_Average_ColorCoordinate_xy.Click += new System.EventHandler(this.button_Get_Average_ColorCoordinate_xy_Click);
            // 
            // groupBox28
            // 
            this.groupBox28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox28.Controls.Add(this.groupBox30);
            this.groupBox28.Controls.Add(this.radioButton_Resize_Bilinear_Interpolation);
            this.groupBox28.Controls.Add(this.radioButton_Resize_Nearest_Interpolation);
            this.groupBox28.Controls.Add(this.button_Resize_Image);
            this.groupBox28.Controls.Add(this.textBox_resized_height);
            this.groupBox28.Controls.Add(this.textBox_resized_width);
            this.groupBox28.Controls.Add(this.label43);
            this.groupBox28.ForeColor = System.Drawing.Color.White;
            this.groupBox28.Location = new System.Drawing.Point(568, 532);
            this.groupBox28.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox28.Size = new System.Drawing.Size(386, 186);
            this.groupBox28.TabIndex = 52;
            this.groupBox28.TabStop = false;
            this.groupBox28.Text = "Resoltuion Change(Nearest / Bilinear)";
            // 
            // groupBox30
            // 
            this.groupBox30.Controls.Add(this.groupBox33);
            this.groupBox30.Controls.Add(this.textBox_Resize_Without_To_x);
            this.groupBox30.Controls.Add(this.textBox_Resize_Without_To_y);
            this.groupBox30.Controls.Add(this.textBox_Resize_Without_From_x);
            this.groupBox30.Controls.Add(this.textBox_Resize_Without_From_y);
            this.groupBox30.Controls.Add(this.label46);
            this.groupBox30.Controls.Add(this.label47);
            this.groupBox30.Controls.Add(this.button_Resize_Image_Without_Change_Specific_Area);
            this.groupBox30.ForeColor = System.Drawing.Color.White;
            this.groupBox30.Location = new System.Drawing.Point(155, 15);
            this.groupBox30.Name = "groupBox30";
            this.groupBox30.Size = new System.Drawing.Size(227, 167);
            this.groupBox30.TabIndex = 291;
            this.groupBox30.TabStop = false;
            this.groupBox30.Text = "Resize Without Change Specific Area";
            // 
            // groupBox33
            // 
            this.groupBox33.Controls.Add(this.label48);
            this.groupBox33.Controls.Add(this.button_Only_Resize_X_to_Bottom);
            this.groupBox33.Controls.Add(this.button_Only_Resize_Top_to_X);
            this.groupBox33.Controls.Add(this.button_Only_Resize_X_to_Right);
            this.groupBox33.Controls.Add(this.textBox_Resize_Position);
            this.groupBox33.Controls.Add(this.label49);
            this.groupBox33.Controls.Add(this.button_Only_Resize_Left_to_X);
            this.groupBox33.ForeColor = System.Drawing.Color.White;
            this.groupBox33.Location = new System.Drawing.Point(6, 60);
            this.groupBox33.Name = "groupBox33";
            this.groupBox33.Size = new System.Drawing.Size(216, 101);
            this.groupBox33.TabIndex = 292;
            this.groupBox33.TabStop = false;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(4, 35);
            this.label48.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(19, 12);
            this.label48.TabIndex = 80;
            this.label48.Text = "K :";
            // 
            // button_Only_Resize_X_to_Bottom
            // 
            this.button_Only_Resize_X_to_Bottom.BackColor = System.Drawing.Color.Black;
            this.button_Only_Resize_X_to_Bottom.ForeColor = System.Drawing.Color.White;
            this.button_Only_Resize_X_to_Bottom.Location = new System.Drawing.Point(64, 75);
            this.button_Only_Resize_X_to_Bottom.Name = "button_Only_Resize_X_to_Bottom";
            this.button_Only_Resize_X_to_Bottom.Size = new System.Drawing.Size(147, 21);
            this.button_Only_Resize_X_to_Bottom.TabIndex = 79;
            this.button_Only_Resize_X_to_Bottom.Text = "Only Resize K to Bottom";
            this.button_Only_Resize_X_to_Bottom.UseVisualStyleBackColor = false;
            this.button_Only_Resize_X_to_Bottom.Click += new System.EventHandler(this.button_Only_Resize_X_to_Bottom_Click);
            // 
            // button_Only_Resize_Top_to_X
            // 
            this.button_Only_Resize_Top_to_X.BackColor = System.Drawing.Color.Black;
            this.button_Only_Resize_Top_to_X.ForeColor = System.Drawing.Color.White;
            this.button_Only_Resize_Top_to_X.Location = new System.Drawing.Point(64, 54);
            this.button_Only_Resize_Top_to_X.Name = "button_Only_Resize_Top_to_X";
            this.button_Only_Resize_Top_to_X.Size = new System.Drawing.Size(147, 21);
            this.button_Only_Resize_Top_to_X.TabIndex = 78;
            this.button_Only_Resize_Top_to_X.Text = "Only Resize Top to K";
            this.button_Only_Resize_Top_to_X.UseVisualStyleBackColor = false;
            this.button_Only_Resize_Top_to_X.Click += new System.EventHandler(this.button_Only_Resize_Top_to_X_Click);
            // 
            // button_Only_Resize_X_to_Right
            // 
            this.button_Only_Resize_X_to_Right.BackColor = System.Drawing.Color.Black;
            this.button_Only_Resize_X_to_Right.ForeColor = System.Drawing.Color.White;
            this.button_Only_Resize_X_to_Right.Location = new System.Drawing.Point(64, 34);
            this.button_Only_Resize_X_to_Right.Name = "button_Only_Resize_X_to_Right";
            this.button_Only_Resize_X_to_Right.Size = new System.Drawing.Size(147, 21);
            this.button_Only_Resize_X_to_Right.TabIndex = 77;
            this.button_Only_Resize_X_to_Right.Text = "Only Resize K to Right";
            this.button_Only_Resize_X_to_Right.UseVisualStyleBackColor = false;
            this.button_Only_Resize_X_to_Right.Click += new System.EventHandler(this.button_Only_Resize_X_to_Right_Click);
            // 
            // textBox_Resize_Position
            // 
            this.textBox_Resize_Position.Location = new System.Drawing.Point(25, 32);
            this.textBox_Resize_Position.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Resize_Position.Name = "textBox_Resize_Position";
            this.textBox_Resize_Position.Size = new System.Drawing.Size(36, 20);
            this.textBox_Resize_Position.TabIndex = 74;
            this.textBox_Resize_Position.Text = "300";
            this.textBox_Resize_Position.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(4, 18);
            this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(61, 12);
            this.label49.TabIndex = 71;
            this.label49.Text = "<Position>";
            // 
            // button_Only_Resize_Left_to_X
            // 
            this.button_Only_Resize_Left_to_X.BackColor = System.Drawing.Color.Black;
            this.button_Only_Resize_Left_to_X.ForeColor = System.Drawing.Color.White;
            this.button_Only_Resize_Left_to_X.Location = new System.Drawing.Point(64, 12);
            this.button_Only_Resize_Left_to_X.Name = "button_Only_Resize_Left_to_X";
            this.button_Only_Resize_Left_to_X.Size = new System.Drawing.Size(147, 21);
            this.button_Only_Resize_Left_to_X.TabIndex = 70;
            this.button_Only_Resize_Left_to_X.Text = "Only Resize Left to K";
            this.button_Only_Resize_Left_to_X.UseVisualStyleBackColor = false;
            this.button_Only_Resize_Left_to_X.Click += new System.EventHandler(this.button_Only_Resize_Left_to_X_Click);
            // 
            // textBox_Resize_Without_To_x
            // 
            this.textBox_Resize_Without_To_x.Location = new System.Drawing.Point(69, 37);
            this.textBox_Resize_Without_To_x.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Resize_Without_To_x.Name = "textBox_Resize_Without_To_x";
            this.textBox_Resize_Without_To_x.Size = new System.Drawing.Size(40, 20);
            this.textBox_Resize_Without_To_x.TabIndex = 76;
            this.textBox_Resize_Without_To_x.Text = "500";
            this.textBox_Resize_Without_To_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Resize_Without_To_y
            // 
            this.textBox_Resize_Without_To_y.Location = new System.Drawing.Point(111, 37);
            this.textBox_Resize_Without_To_y.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Resize_Without_To_y.Name = "textBox_Resize_Without_To_y";
            this.textBox_Resize_Without_To_y.Size = new System.Drawing.Size(40, 20);
            this.textBox_Resize_Without_To_y.TabIndex = 75;
            this.textBox_Resize_Without_To_y.Text = "600";
            this.textBox_Resize_Without_To_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Resize_Without_From_x
            // 
            this.textBox_Resize_Without_From_x.Location = new System.Drawing.Point(69, 16);
            this.textBox_Resize_Without_From_x.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Resize_Without_From_x.Name = "textBox_Resize_Without_From_x";
            this.textBox_Resize_Without_From_x.Size = new System.Drawing.Size(40, 20);
            this.textBox_Resize_Without_From_x.TabIndex = 74;
            this.textBox_Resize_Without_From_x.Text = "300";
            this.textBox_Resize_Without_From_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Resize_Without_From_y
            // 
            this.textBox_Resize_Without_From_y.Location = new System.Drawing.Point(111, 16);
            this.textBox_Resize_Without_From_y.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Resize_Without_From_y.Name = "textBox_Resize_Without_From_y";
            this.textBox_Resize_Without_From_y.Size = new System.Drawing.Size(40, 20);
            this.textBox_Resize_Without_From_y.TabIndex = 73;
            this.textBox_Resize_Without_From_y.Text = "250";
            this.textBox_Resize_Without_From_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(18, 40);
            this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(51, 12);
            this.label46.TabIndex = 72;
            this.label46.Text = "To(x,y) : ";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(3, 20);
            this.label47.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(66, 12);
            this.label47.TabIndex = 71;
            this.label47.Text = "From(x,y) : ";
            // 
            // button_Resize_Image_Without_Change_Specific_Area
            // 
            this.button_Resize_Image_Without_Change_Specific_Area.BackColor = System.Drawing.Color.Black;
            this.button_Resize_Image_Without_Change_Specific_Area.ForeColor = System.Drawing.Color.White;
            this.button_Resize_Image_Without_Change_Specific_Area.Location = new System.Drawing.Point(156, 16);
            this.button_Resize_Image_Without_Change_Specific_Area.Name = "button_Resize_Image_Without_Change_Specific_Area";
            this.button_Resize_Image_Without_Change_Specific_Area.Size = new System.Drawing.Size(60, 41);
            this.button_Resize_Image_Without_Change_Specific_Area.TabIndex = 70;
            this.button_Resize_Image_Without_Change_Specific_Area.Text = "Resize Image";
            this.button_Resize_Image_Without_Change_Specific_Area.UseVisualStyleBackColor = false;
            this.button_Resize_Image_Without_Change_Specific_Area.Click += new System.EventHandler(this.button_Resize_Image_Without_Change_Specific_Area_Click);
            // 
            // radioButton_Resize_Bilinear_Interpolation
            // 
            this.radioButton_Resize_Bilinear_Interpolation.AutoSize = true;
            this.radioButton_Resize_Bilinear_Interpolation.Checked = true;
            this.radioButton_Resize_Bilinear_Interpolation.Location = new System.Drawing.Point(9, 37);
            this.radioButton_Resize_Bilinear_Interpolation.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Resize_Bilinear_Interpolation.Name = "radioButton_Resize_Bilinear_Interpolation";
            this.radioButton_Resize_Bilinear_Interpolation.Size = new System.Drawing.Size(140, 16);
            this.radioButton_Resize_Bilinear_Interpolation.TabIndex = 71;
            this.radioButton_Resize_Bilinear_Interpolation.TabStop = true;
            this.radioButton_Resize_Bilinear_Interpolation.Text = "Bilinear Interpolation";
            this.radioButton_Resize_Bilinear_Interpolation.UseVisualStyleBackColor = true;
            // 
            // radioButton_Resize_Nearest_Interpolation
            // 
            this.radioButton_Resize_Nearest_Interpolation.AutoSize = true;
            this.radioButton_Resize_Nearest_Interpolation.Location = new System.Drawing.Point(9, 19);
            this.radioButton_Resize_Nearest_Interpolation.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton_Resize_Nearest_Interpolation.Name = "radioButton_Resize_Nearest_Interpolation";
            this.radioButton_Resize_Nearest_Interpolation.Size = new System.Drawing.Size(141, 16);
            this.radioButton_Resize_Nearest_Interpolation.TabIndex = 70;
            this.radioButton_Resize_Nearest_Interpolation.Text = "Nearest Interpolation";
            this.radioButton_Resize_Nearest_Interpolation.UseVisualStyleBackColor = true;
            // 
            // button_Resize_Image
            // 
            this.button_Resize_Image.BackColor = System.Drawing.Color.Black;
            this.button_Resize_Image.ForeColor = System.Drawing.Color.White;
            this.button_Resize_Image.Location = new System.Drawing.Point(9, 121);
            this.button_Resize_Image.Name = "button_Resize_Image";
            this.button_Resize_Image.Size = new System.Drawing.Size(140, 56);
            this.button_Resize_Image.TabIndex = 69;
            this.button_Resize_Image.Text = "Resize Image";
            this.button_Resize_Image.UseVisualStyleBackColor = false;
            this.button_Resize_Image.Click += new System.EventHandler(this.button_Resize_Image_Click);
            // 
            // textBox_resized_height
            // 
            this.textBox_resized_height.Location = new System.Drawing.Point(110, 97);
            this.textBox_resized_height.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_resized_height.Name = "textBox_resized_height";
            this.textBox_resized_height.Size = new System.Drawing.Size(39, 20);
            this.textBox_resized_height.TabIndex = 65;
            this.textBox_resized_height.Text = "2000";
            this.textBox_resized_height.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_resized_width
            // 
            this.textBox_resized_width.Location = new System.Drawing.Point(63, 97);
            this.textBox_resized_width.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_resized_width.Name = "textBox_resized_width";
            this.textBox_resized_width.Size = new System.Drawing.Size(43, 20);
            this.textBox_resized_width.TabIndex = 64;
            this.textBox_resized_width.Text = "1000";
            this.textBox_resized_width.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(63, 80);
            this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(86, 12);
            this.label43.TabIndex = 62;
            this.label43.Text = "(Width, Height)";
            // 
            // trackBar_Histo_Y_Scale
            // 
            this.trackBar_Histo_Y_Scale.Location = new System.Drawing.Point(590, 17);
            this.trackBar_Histo_Y_Scale.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBar_Histo_Y_Scale.Maximum = 100;
            this.trackBar_Histo_Y_Scale.Minimum = 1;
            this.trackBar_Histo_Y_Scale.Name = "trackBar_Histo_Y_Scale";
            this.trackBar_Histo_Y_Scale.Size = new System.Drawing.Size(140, 45);
            this.trackBar_Histo_Y_Scale.TabIndex = 291;
            this.trackBar_Histo_Y_Scale.Value = 1;
            this.trackBar_Histo_Y_Scale.ValueChanged += new System.EventHandler(this.trackBar_Histo_Y_Scale_ValueChanged);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.ForeColor = System.Drawing.Color.White;
            this.label45.Location = new System.Drawing.Point(482, 29);
            this.label45.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(105, 12);
            this.label45.TabIndex = 292;
            this.label45.Text = "Histogram Scale : ";
            // 
            // groupBox34
            // 
            this.groupBox34.Controls.Add(this.button_Dot_Detection);
            this.groupBox34.ForeColor = System.Drawing.Color.White;
            this.groupBox34.Location = new System.Drawing.Point(259, 531);
            this.groupBox34.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox34.Name = "groupBox34";
            this.groupBox34.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox34.Size = new System.Drawing.Size(305, 181);
            this.groupBox34.TabIndex = 66;
            this.groupBox34.TabStop = false;
            // 
            // button_Dot_Detection
            // 
            this.button_Dot_Detection.BackColor = System.Drawing.Color.Black;
            this.button_Dot_Detection.ForeColor = System.Drawing.Color.White;
            this.button_Dot_Detection.Location = new System.Drawing.Point(5, 13);
            this.button_Dot_Detection.Name = "button_Dot_Detection";
            this.button_Dot_Detection.Size = new System.Drawing.Size(99, 27);
            this.button_Dot_Detection.TabIndex = 68;
            this.button_Dot_Detection.Text = "Dot Detection";
            this.button_Dot_Detection.UseVisualStyleBackColor = false;
            this.button_Dot_Detection.Click += new System.EventHandler(this.button_Dot_Detection_Click);
            // 
            // BMP_Image_Processing_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1692, 961);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox34);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.trackBar_Histo_Y_Scale);
            this.Controls.Add(this.groupBox28);
            this.Controls.Add(this.groupBox24);
            this.Controls.Add(this.groupBox23);
            this.Controls.Add(this.groupBox21);
            this.Controls.Add(this.groupBox17);
            this.Controls.Add(this.groupBox13);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.pictureBox_Histo_B);
            this.Controls.Add(this.pictureBox_Histo_G);
            this.Controls.Add(this.pictureBox_Histo_R);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Cal_RGB_ratio_btn);
            this.Controls.Add(this.Get_Histogram_and_GrayArray_btn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pictureBox_Loaded_BMP);
            this.Controls.Add(this.Exit_btn);
            this.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "BMP_Image_Processing_Form";
            this.Text = "BMP_Image_Processing_Form";
            this.Load += new System.EventHandler(this.BMP_Image_Processing_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Loaded_BMP)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Pixel_RGB_Display)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Histo_R)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Histo_G)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Histo_B)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox32.ResumeLayout(false);
            this.groupBox32.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_To_be_created_BMP)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.groupBox31.ResumeLayout(false);
            this.groupBox31.PerformLayout();
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.groupBox29.ResumeLayout(false);
            this.groupBox29.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_gray_resolution_bits)).EndInit();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_2nd_BMP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_1st_BMP)).EndInit();
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Black)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Magenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Yellow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cyan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CYM)).EndInit();
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CIE_XY_Selected_Area_Ave_Color)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CIE_XY_Selected_Area)).EndInit();
            this.groupBox28.ResumeLayout(false);
            this.groupBox28.PerformLayout();
            this.groupBox30.ResumeLayout(false);
            this.groupBox30.PerformLayout();
            this.groupBox33.ResumeLayout(false);
            this.groupBox33.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Histo_Y_Scale)).EndInit();
            this.groupBox34.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Exit_btn;
        private System.Windows.Forms.Button Fast_Image_load_btn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_Show_Origin_Image;
        private System.Windows.Forms.TextBox textBox_Y;
        private System.Windows.Forms.TextBox textBox_X;
        private System.Windows.Forms.Button button_Show_RGB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox G_sum;
        private System.Windows.Forms.TextBox B_sum;
        private System.Windows.Forms.TextBox R_sum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox G_ratio;
        private System.Windows.Forms.TextBox B_ratio;
        private System.Windows.Forms.TextBox R_ratio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Cal_RGB_ratio_btn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridView dataGridView_Pixel_RGB_Display;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox_G_APL;
        private System.Windows.Forms.TextBox textBox_B_APL;
        private System.Windows.Forms.TextBox textBox_R_APL;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox_Save_RGB_Data_As_CSV;
        private System.Windows.Forms.RadioButton radioButton_RGB_Data_3;
        private System.Windows.Forms.RadioButton radioButton_RGB_Data_2;
        private System.Windows.Forms.RadioButton radioButton_RGB_Data_1;
        private System.Windows.Forms.RadioButton radioButton_RGB_Data_4;
        private System.Windows.Forms.PictureBox pictureBox_Histo_R;
        private System.Windows.Forms.PictureBox pictureBox_Histo_G;
        private System.Windows.Forms.PictureBox pictureBox_Histo_B;
        private System.Windows.Forms.GroupBox groupBox6;
        public System.Windows.Forms.TextBox textBox_BMP_Maker_Resolution_Y;
        public System.Windows.Forms.TextBox textBox_BMP_Maker_Resolution_X;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.PictureBox pictureBox_To_be_created_BMP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.RichTextBox RichTextBox_BMP_Status;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button button_Make_Multiple_Images;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.CheckBox checkBox_G63_Border;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.CheckBox checkBox_Cinema;
        private System.Windows.Forms.CheckBox checkBox_Mosaic;
        private System.Windows.Forms.CheckBox checkBox_W_H_Gradation;
        private System.Windows.Forms.CheckBox checkBox_SH_All_IR_Drop_Pattern;
        private System.Windows.Forms.CheckBox checkBox_Dot_by_Dot_Pattern;
        private System.Windows.Forms.CheckBox checkBox_H_WRGB_Gradation;
        private System.Windows.Forms.CheckBox checkBox_V_WRGB_Gradation;
        private System.Windows.Forms.CheckBox checkBox_Color_Bar;
        private System.Windows.Forms.CheckBox checkBox_RGB_Gradation;
        private System.Windows.Forms.CheckBox checkBox_V_LByL;
        private System.Windows.Forms.CheckBox checkBox_H_LByL;
        private System.Windows.Forms.CheckBox checkBox_Five_Color_RYGCB_Pattern;
        private System.Windows.Forms.CheckBox checkBox_V_LbyL_Magenta_Green_Gradation;
        private System.Windows.Forms.CheckBox checkBox_V_LbyL_Magenta_Green;
        private System.Windows.Forms.CheckBox checkBox_Pattern_40_Percent;
        private System.Windows.Forms.CheckBox checkBox_Mura_Detect_Pattern;
        private System.Windows.Forms.CheckBox checkBox_Gray0_to_Gray7;
        private System.Windows.Forms.CheckBox checkBox_Cross_Talk;
        private System.Windows.Forms.TextBox textBox_G63_Border_Left_Right_Line;
        private System.Windows.Forms.TextBox textBox_G63_Border_Top_Bottom_Line;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_Pseudo_WRGB_Gray;
        private System.Windows.Forms.TextBox textBox_Pseudo_Background_Gray;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox checkBox_Pseudo;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TextBox textBox_2nd_Dot_or_Line_B;
        private System.Windows.Forms.TextBox textBox_2nd_Dot_or_Line_G;
        private System.Windows.Forms.TextBox textBox_2nd_Dot_or_Line_R;
        private System.Windows.Forms.TextBox textBox_1st_Dot_or_Line_B;
        private System.Windows.Forms.TextBox textBox_1st_Dot_or_Line_G;
        private System.Windows.Forms.TextBox textBox_1st_Dot_or_Line_R;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox checkBox_MyGradation;
        public System.Windows.Forms.TextBox textBox_MyGradation_End_Gray;
        public System.Windows.Forms.TextBox textBox_MyGradation_Start_Gray;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button button_Clamp_Max_Gray;
        public System.Windows.Forms.TextBox textBox_Clamping_Max_Gray_R;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button button_Back_To_Original;
        public System.Windows.Forms.PictureBox pictureBox_Loaded_BMP;
        public System.Windows.Forms.TextBox textBox_Gamma;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button button_Gamma_Encoding;
        private System.Windows.Forms.Button button_Gamma_Decoding;
        private System.Windows.Forms.Button button_Change_Gray_Resolution;
        private System.Windows.Forms.NumericUpDown numericUpDown_gray_resolution_bits;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Label label_PSNR;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_RGB_to_Gray;
        private System.Windows.Forms.Button button_Save_Current_Image;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.Button button_3x3_Ave_Filter;
        private System.Windows.Forms.Button button_5x5_Ave_Filter;
        private System.Windows.Forms.Button button_histogram_equalization;
        public System.Windows.Forms.Button Get_Histogram_and_GrayArray_btn;
        private System.Windows.Forms.Button button_Bit_Inversion;
        private System.Windows.Forms.Button button_5x5_Meadian_Filter;
        private System.Windows.Forms.Button button_3x3_Meadian_Filter;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.Button button_HighBoost_Filter;
        private System.Windows.Forms.Button button_Laplace_Sharpness_Filter;
        private System.Windows.Forms.GroupBox groupBox20;
        public System.Windows.Forms.TextBox textBox_SharpNessWeight;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.Button button_2nd_BMP_Load;
        public System.Windows.Forms.PictureBox pictureBox_2nd_BMP;
        private System.Windows.Forms.Button button_1st_BMP_Load;
        public System.Windows.Forms.PictureBox pictureBox_1st_BMP;
        private System.Windows.Forms.Label label_1st_2nd_PSNR;
        private System.Windows.Forms.Button button_Get_1st_2nd_PSNR;
        private System.Windows.Forms.Label label_Resolution;
        private System.Windows.Forms.Label label_1st_2nd_Resolution;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.Button button_Image_Extraction_Foreground_As_White;
        private System.Windows.Forms.Button button_Image_Extraction_Background_As_Black;
        public System.Windows.Forms.TextBox textBox_Image_Extraction_End_Gray_R;
        public System.Windows.Forms.TextBox textBox_Image_Extraction_Start_Gray_R;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        public System.Windows.Forms.TextBox textBox_Image_Extraction_End_Gray_B;
        public System.Windows.Forms.TextBox textBox_Image_Extraction_Start_Gray_B;
        public System.Windows.Forms.TextBox textBox_Image_Extraction_End_Gray_G;
        public System.Windows.Forms.TextBox textBox_Image_Extraction_Start_Gray_G;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        public System.Windows.Forms.TextBox textBox_Clamping_Max_Gray_B;
        public System.Windows.Forms.TextBox textBox_Clamping_Max_Gray_G;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        public System.Windows.Forms.PictureBox pictureBox_Magenta;
        public System.Windows.Forms.PictureBox pictureBox_Yellow;
        public System.Windows.Forms.PictureBox pictureBox_Cyan;
        public System.Windows.Forms.PictureBox pictureBox_CYM;
        private System.Windows.Forms.Button button_RGB_to_CYMK;
        private System.Windows.Forms.Button button_RGB_to_CYM;
        public System.Windows.Forms.PictureBox pictureBox_Black;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.GroupBox groupBox24;
        public System.Windows.Forms.TextBox textBox_xy_To_Col;
        public System.Windows.Forms.TextBox textBox_xy_To_Row;
        public System.Windows.Forms.TextBox textBox_xy_From_Col;
        public System.Windows.Forms.TextBox textBox_xy_From_Row;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Button button_Get_Average_ColorCoordinate_xy;
        public System.Windows.Forms.PictureBox pictureBox_CIE_XY_Selected_Area_Ave_Color;
        public System.Windows.Forms.PictureBox pictureBox_CIE_XY_Selected_Area;
        private System.Windows.Forms.Label label_Ave_RGB;
        public System.Windows.Forms.Label label_Ave_xy;
        public System.Windows.Forms.Label label_Calculated_Color_Temperature;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.GroupBox groupBox29;
        private System.Windows.Forms.GroupBox groupBox31;
        private System.Windows.Forms.TextBox textBox_Bi_Value;
        private System.Windows.Forms.Button button_White_or_Black;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Button button_Black_or_White;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox32;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox7;
        public System.Windows.Forms.TextBox textBox_2nd_Line_Num;
        public System.Windows.Forms.TextBox textBox_1st_Line_Num;
        public System.Windows.Forms.TextBox textBox_Dot_Size;
        private System.Windows.Forms.Button button_Create_Dot_Noise;
        public System.Windows.Forms.TextBox textBox_Dot_B;
        public System.Windows.Forms.TextBox textBox_Dot_G;
        public System.Windows.Forms.TextBox textBox_Dot_R;
        private System.Windows.Forms.Label label41;
        public System.Windows.Forms.TextBox textBox_Dot_Num;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.Button button_Edge_Detection;
        private System.Windows.Forms.CheckBox checkBox_225degree_Edge_Detection;
        private System.Windows.Forms.CheckBox checkBox_45degree_Edge_Detection;
        private System.Windows.Forms.CheckBox checkBox_Vertical_Edge_Detection;
        private System.Windows.Forms.CheckBox checkBox_Horizontal_Edge_Detection;
        private System.Windows.Forms.RadioButton radioButton_Resize_Bilinear_Interpolation;
        private System.Windows.Forms.RadioButton radioButton_Resize_Nearest_Interpolation;
        private System.Windows.Forms.Button button_Resize_Image;
        public System.Windows.Forms.TextBox textBox_resized_height;
        public System.Windows.Forms.TextBox textBox_resized_width;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.TextBox textBox_Image_Alpha_Value;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Button button_Change_Image_Alpha_Value;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.Button button_Dilation;
        private System.Windows.Forms.RadioButton radioButton_Morphological_Square_Kernel;
        private System.Windows.Forms.RadioButton radioButton_Morphological_Circle_Kernel;
        private System.Windows.Forms.TextBox textBox_Kernel_Length;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button button_Erosion;
        private System.Windows.Forms.TrackBar trackBar_Histo_Y_Scale;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox30;
        public System.Windows.Forms.TextBox textBox_Resize_Without_To_x;
        public System.Windows.Forms.TextBox textBox_Resize_Without_To_y;
        public System.Windows.Forms.TextBox textBox_Resize_Without_From_x;
        public System.Windows.Forms.TextBox textBox_Resize_Without_From_y;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Button button_Resize_Image_Without_Change_Specific_Area;
        private System.Windows.Forms.GroupBox groupBox33;
        private System.Windows.Forms.Button button_Only_Resize_X_to_Bottom;
        private System.Windows.Forms.Button button_Only_Resize_Top_to_X;
        private System.Windows.Forms.Button button_Only_Resize_X_to_Right;
        public System.Windows.Forms.TextBox textBox_Resize_Position;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Button button_Only_Resize_Left_to_X;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.GroupBox groupBox34;
        private System.Windows.Forms.Button button_Dot_Detection;
    }
}