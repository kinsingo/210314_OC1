using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using References
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


using System.Data.OleDb;
using System.Globalization;
using Microsoft.VisualBasic;


//Histogram
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using BSQH_Csharp_Library;
using System.Security.Cryptography;

namespace PNC_Csharp
{
    public partial class BMP_Image_Processing_Form : Form
    {
        int Picturebox_Resolution_X = 96;
        int Picturebox_Resolution_Y = 120;

        string BMP_File_Name;
        Bitmap Original_img;
        BMP bmp_maker = new BMP_Maker();
        ImageProcessing img_process = new ImageProcessing();

        public int[] Histogram_R = new int[256];
        int Max_R = 1; //At first it must be (>0)

        public int[] Histogram_G = new int[256];
        int Max_G = 1; //At first it must be (>0)

        public int[] Histogram_B = new int[256];
        int Max_B = 1; //At first it must be (>0)

        int Biggest_Max_RGB = 1;
        int Histo_Origin_Y_Scale = 0;

        private Label[] GrayLabels_R = new Label[9];
        private Label[] GrayLabels_G = new Label[9];
        private Label[] GrayLabels_B = new Label[9];

        private static BMP_Image_Processing_Form Instance;
        public static BMP_Image_Processing_Form getInstance()
        {
            if (Instance == null)
                Instance = new BMP_Image_Processing_Form();

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
        private BMP_Image_Processing_Form()
        {
            InitializeComponent();
            for (int i = 0; i <= 255; i++) dataGridView1.Rows.Add(i, 0, 0, 0);
        }

        private void BMP_Image_Processing_Form_Load(object sender, EventArgs e)
        {
            textBox_BMP_Maker_Resolution_X.Text = f1().current_model.get_X().ToString();
            textBox_BMP_Maker_Resolution_Y.Text = f1().current_model.get_Y().ToString();

            for (int i = 0; i < 9; i++)
            {
                GrayLabels_R[i] = new Label();
                GrayLabels_G[i] = new Label();
                GrayLabels_B[i] = new Label();

                GrayLabels_R[i].Parent = pictureBox_Histo_R;
                GrayLabels_G[i].Parent = pictureBox_Histo_G;
                GrayLabels_B[i].Parent = pictureBox_Histo_B;

                GrayLabels_R[i].ForeColor = Color.Black;
                GrayLabels_G[i].ForeColor = Color.Black;
                GrayLabels_B[i].ForeColor = Color.Black;

                GrayLabels_R[i].BackColor = Color.Transparent;
                GrayLabels_G[i].BackColor = Color.Transparent;
                GrayLabels_B[i].BackColor = Color.Transparent;

   
                GrayLabels_R[i].AutoSize = true;
                GrayLabels_G[i].AutoSize = true;
                GrayLabels_B[i].AutoSize = true;

                int gray = i * 30;
                GrayLabels_R[i].Text = "|" + gray.ToString();
                GrayLabels_G[i].Text = "|" + gray.ToString();
                GrayLabels_B[i].Text = "|" + gray.ToString();

                GrayLabels_R[i].Location = new Point(gray - 2, 0);
                GrayLabels_G[i].Location = new Point(gray - 2, 0);
                GrayLabels_B[i].Location = new Point(gray - 2, 0);

            }            
        }

        Form1 f1()
        {
            return (Form1)Application.OpenForms["Form1"];
        }


        private void Exit_btn_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void SaveAsCSV(FastBitmap fastBitmap_img)
        {
            using (StreamWriter outfile = new StreamWriter(Directory.GetCurrentDirectory() + "\\Fast_RGB_Pixel_CSV_Data.csv"))
            {
                unsafe
                {
                    byte* row = (byte*)fastBitmap_img.Scan0;
                    byte* bb = row;
                    string content = string.Empty;
                    for (int yy = 0; yy < fastBitmap_img.Height; yy++)
                    {
                        bb = row;
                        content = string.Empty;
                        for (int xx = 0; xx < fastBitmap_img.Width; xx++)
                        {
                            // *(row + 0) is B (Blue ) component of the pixel
                            // *(row + 1) is G (Green) component of the pixel
                            // *(row + 2) is R (Red  ) component of the pixel
                            // *(row + 3) is A (Alpha) component of the pixel ( for 32bpp )
                            content += ((*(bb + 2)).ToString() + "," + (*(bb + 1)).ToString() + "," + (*(bb + 0)).ToString() + ",");
                            bb += fastBitmap_img.PixelSize;
                        }
                        outfile.WriteLine(content);
                        row += fastBitmap_img.Stride;
                    }
                }
            }
            System.Windows.Forms.MessageBox.Show("BMP Data was saved as 'Fast_RGB_Pixel_CSV_Data.csv'");
        }

        private void Fast_Image_load_btn_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image != null)
            {
                for (int i = 0; i <= 255; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = 0;
                    dataGridView1.Rows[i].Cells[2].Value = 0;
                    dataGridView1.Rows[i].Cells[3].Value = 0;
                }
                R_sum.Text = G_sum.Text = B_sum.Text = "0";
                R_ratio.Text = G_ratio.Text = B_ratio.Text = "0%";
                textBox_R_APL.Text = textBox_G_APL.Text = textBox_B_APL.Text = "0%";
                dataGridView_Pixel_RGB_Display.Columns.Clear();
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    BMP_File_Name = dlg.FileName;

                    Set_pictureBox_Loaded_BMP_Image(new Bitmap(dlg.FileName));
                    Original_img = new Bitmap(dlg.FileName);
                    FastBitmap fastBitmap_img = new FastBitmap(Original_img);

                    //Save file as CSV
                    if (checkBox_Save_RGB_Data_As_CSV.Checked)
                        SaveAsCSV(fastBitmap_img);

                    fastBitmap_img.Dispose();
                }
                else
                {
                    //Do Nothing (It takes place when click "cancle")
                }

            }

        }


        private void Initialize_for_Get_Array_and_Histogram()
        {
            for (int k = 0; k <= 255; k++)
            {
                dataGridView1.Rows[k].Cells[1].Value = 0;
                dataGridView1.Rows[k].Cells[2].Value = 0;
                dataGridView1.Rows[k].Cells[3].Value = 0;

                Histogram_R[k] = 0;
                Histogram_G[k] = 0;
                Histogram_B[k] = 0;
            }
            Max_R = 1; Max_G = 1; Max_B = 1; Biggest_Max_RGB = 1;
            pictureBox_Histo_R.Refresh();
            pictureBox_Histo_G.Refresh();
            pictureBox_Histo_B.Refresh();
            System.Windows.Forms.Application.DoEvents();
        }


        private void Get_Array_btn_Click(object sender, EventArgs e)
        {
            // *(bb + 0) is B (Blue ) component of the pixel
            // *(bb + 1) is G (Green) component of the pixel
            // *(bb + 2) is R (Red  ) component of the pixel
            if (pictureBox_Loaded_BMP.Image == null)
            {
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            }
            else
            {
                Initialize_for_Get_Array_and_Histogram();
                FastBitmap fastBitmap_img = new FastBitmap((Bitmap)pictureBox_Loaded_BMP.Image);
                progressBar1.Maximum = fastBitmap_img.Height;
                unsafe
                {
                    byte* row = (byte*)fastBitmap_img.Scan0;
                    byte* bb = row;
                    for (int yy = 0; yy < fastBitmap_img.Height; yy++)
                    {
                        bb = row;
                        for (int xx = 0; xx < fastBitmap_img.Width; xx++)
                        {
                            for (int k = 0; k <= 255; k++)
                            {
                                if ((*(bb + 2)) == k) // R++
                                {
                                    int value = (int)dataGridView1.Rows[k].Cells[1].Value;
                                    dataGridView1.Rows[k].Cells[1].Value = value + 1;

                                    Histogram_R[k] = value + 1;
                                    if (Histogram_R[k] > Max_R) Max_R = Histogram_R[k];

                                }
                                if ((*(bb + 1)) == k) // G++
                                {
                                    int value = (int)dataGridView1.Rows[k].Cells[2].Value;
                                    dataGridView1.Rows[k].Cells[2].Value = value + 1;

                                    Histogram_G[k] = value + 1;
                                    if (Histogram_G[k] > Max_G) Max_G = Histogram_G[k];
                                }

                                if ((*(bb + 0)) == k) // B++
                                {
                                    int value = (int)dataGridView1.Rows[k].Cells[3].Value;
                                    dataGridView1.Rows[k].Cells[3].Value = value + 1;

                                    Histogram_B[k] = value + 1;
                                    if (Histogram_B[k] > Max_B) Max_B = Histogram_B[k];
                                }
                            }
                            bb += fastBitmap_img.PixelSize;
                        }
                        progressBar1.Value = yy;
                        row += fastBitmap_img.Stride;
                    }
                }
                if (Biggest_Max_RGB < Max_R) Biggest_Max_RGB = Max_R;
                if (Biggest_Max_RGB < Max_G) Biggest_Max_RGB = Max_G;
                if (Biggest_Max_RGB < Max_B) Biggest_Max_RGB = Max_B;

                Histo_Origin_Y_Scale = Biggest_Max_RGB;

                pictureBox_Histo_R.Refresh();
                pictureBox_Histo_G.Refresh();
                pictureBox_Histo_B.Refresh();

                progressBar1.Value = 0;

                fastBitmap_img.Dispose();
            }
        }


        private void Cal_RGB_ratio_btn_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
            {
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            }
            else
            {
                FastBitmap fastBitmap_img = new FastBitmap((Bitmap)pictureBox_Loaded_BMP.Image);
                R_sum.Text = G_sum.Text = B_sum.Text = "0";
                R_ratio.Text = G_ratio.Text = B_ratio.Text = "0%";
                textBox_R_APL.Text = textBox_G_APL.Text = textBox_B_APL.Text = "0%";

                System.Windows.Forms.Application.DoEvents();

                progressBar1.Maximum = fastBitmap_img.Height;

                double[] Sum = new double[] { 0, 0, 0 };

                unsafe
                {
                    byte* row = (byte*)fastBitmap_img.Scan0;
                    byte* bb = row;
                    for (int yy = 0; yy < fastBitmap_img.Height; yy++)
                    {
                        bb = row;
                        for (int xx = 0; xx < fastBitmap_img.Width; xx++)
                        {
                            Sum[0] += (*(bb + 2)); //R
                            Sum[1] += (*(bb + 1)); //G
                            Sum[2] += (*(bb + 0)); //B

                            bb += fastBitmap_img.PixelSize;
                        }
                        progressBar1.Value = yy;
                        row += fastBitmap_img.Stride;
                    }
                }

                R_sum.Text = Sum[0].ToString();
                G_sum.Text = Sum[1].ToString();
                B_sum.Text = Sum[2].ToString();

                double Total = (Sum[0] + Sum[1] + Sum[2]) / 100;
                R_ratio.Text = Math.Round(Math.Abs(Sum[0] / Total), 1).ToString() + "%";
                G_ratio.Text = Math.Round(Math.Abs(Sum[1] / Total), 1).ToString() + "%";
                B_ratio.Text = Math.Round(Math.Abs(Sum[2] / Total), 1).ToString() + "%";

                double max_RGB = 255.0 * fastBitmap_img.Width * fastBitmap_img.Height;
                textBox_R_APL.Text = Math.Round((Sum[0] * 100 / max_RGB), 1).ToString() + "%";
                textBox_G_APL.Text = Math.Round((Sum[1] * 100 / max_RGB), 1).ToString() + "%";
                textBox_B_APL.Text = Math.Round((Sum[2] * 100 / max_RGB), 1).ToString() + "%";

                progressBar1.Value = 0;
                fastBitmap_img.Dispose();
            }
        }

        private void button_Show_Origin_Image_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
            {
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            }
            else
            {
                pictureBox_Loaded_BMP.Image = Original_img;
                dataGridView_Pixel_RGB_Display.Columns.Clear();
            }
        }

        private void button_Show_RGB_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
            {
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            }
            else
            {
                Bitmap Changed_img = new Bitmap(pictureBox_Loaded_BMP.Image);
                Bitmap Current_img = new Bitmap(pictureBox_Loaded_BMP.Image);
                FastBitmap fastBitmap_img = new FastBitmap((Bitmap)pictureBox_Loaded_BMP.Image);

                //Original 대비 바뀌는 부분
                // X,Y 에 대한 Boundary Processing (상하좌우 10개의 Pixel Data 보여줄거임)
                int Show_Data_Pixels = 0;
                if (radioButton_RGB_Data_1.Checked) Show_Data_Pixels = 1;
                else if (radioButton_RGB_Data_2.Checked) Show_Data_Pixels = 2;
                else if (radioButton_RGB_Data_3.Checked) Show_Data_Pixels = 3;
                else if (radioButton_RGB_Data_4.Checked) Show_Data_Pixels = 4;

                int x = Convert.ToInt32(textBox_X.Text);
                if (x > fastBitmap_img.Width - Show_Data_Pixels && fastBitmap_img.Width > Show_Data_Pixels * 2)
                    x = fastBitmap_img.Width - Show_Data_Pixels - 1;
                else if (x < Show_Data_Pixels && fastBitmap_img.Width > Show_Data_Pixels * 2)
                    x = Show_Data_Pixels;
                else if (fastBitmap_img.Width <= Show_Data_Pixels * 2)
                    System.Windows.Forms.MessageBox.Show("Image Width is lower than 20 pixels , Error");
                textBox_X.Text = x.ToString();

                int y = Convert.ToInt32(textBox_Y.Text);
                if (y > fastBitmap_img.Height - Show_Data_Pixels && fastBitmap_img.Height > Show_Data_Pixels * 2) y = fastBitmap_img.Height - Show_Data_Pixels - 1;

                else if (y < Show_Data_Pixels && fastBitmap_img.Height > Show_Data_Pixels * 2) y = Show_Data_Pixels;

                else if (fastBitmap_img.Height <= Show_Data_Pixels * 2) System.Windows.Forms.MessageBox.Show("Image Height is lower than 20 pixels , Error");

                textBox_Y.Text = y.ToString();

                //Show DataGridView
                dataGridView_Pixel_RGB_Display.RowCount = (Show_Data_Pixels * 2 + 1);
                dataGridView_Pixel_RGB_Display.ColumnCount = (Show_Data_Pixels * 2 + 1) * 3;
                for (int i = 0; i < dataGridView_Pixel_RGB_Display.ColumnCount; i = i + 3)
                {
                    dataGridView_Pixel_RGB_Display.Columns[i].HeaderText = "R";
                    dataGridView_Pixel_RGB_Display.Columns[i + 1].HeaderText = "G";
                    dataGridView_Pixel_RGB_Display.Columns[i + 2].HeaderText = "B";
                    dataGridView_Pixel_RGB_Display.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                    dataGridView_Pixel_RGB_Display.Columns[i + 1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    dataGridView_Pixel_RGB_Display.Columns[i + 2].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                }


                dataGridView_Pixel_RGB_Display.DefaultCellStyle.Font = new Font("Arial", 12, System.Drawing.FontStyle.Regular);
                Color myRgbColor = Color.FromArgb(255, 255, 0);
                int Grid_Center_X = 0;
                int Grid_Center_Y = 0;

                //fb.Scan0 + yy * fb.Stride + xx * fb.PixelSize
                unsafe
                {
                    byte* row = (byte*)fastBitmap_img.Scan0 + ((y - Show_Data_Pixels) * fastBitmap_img.Stride) + ((x - Show_Data_Pixels) * fastBitmap_img.PixelSize);
                    byte* bb = row;

                    for (int i = 0; i < dataGridView_Pixel_RGB_Display.RowCount; i++) //세로
                    {
                        bb = row;
                        for (int j = 0; j < dataGridView_Pixel_RGB_Display.ColumnCount; j = j + 3) //가로 (RGB)
                        {
                            dataGridView_Pixel_RGB_Display.Rows[i].Cells[j].Value = (*(bb + 2)); //R
                            dataGridView_Pixel_RGB_Display.Rows[i].Cells[j + 1].Value = (*(bb + 1)); //G
                            dataGridView_Pixel_RGB_Display.Rows[i].Cells[j + 2].Value = (*(bb + 0)); //B

                            if (i == Show_Data_Pixels && j == Show_Data_Pixels * 3)
                            {
                                Grid_Center_Y = i;
                                Grid_Center_X = j;
                            }

                            myRgbColor = Color.FromArgb(255 - (*(bb + 2)), 255 - (*(bb + 1)), 255 - (*(bb + 0)));
                            Changed_img.SetPixel(j / 3 + (x - Show_Data_Pixels), i + (y - Show_Data_Pixels), myRgbColor);

                            bb += fastBitmap_img.PixelSize;
                        }
                        row += fastBitmap_img.Stride;
                    }
                }

                DataGridViewCellStyle style1 = new DataGridViewCellStyle();
                style1.Font = new Font("Arial", 12, System.Drawing.FontStyle.Bold);

                DataGridViewCellStyle style2 = new DataGridViewCellStyle();
                style2.Font = new Font("Arial", 12, System.Drawing.FontStyle.Regular);

                fastBitmap_img.Dispose();

                for (int i = 0; i < 3; i++)
                {
                    //중앙 글꼴 Bold + Search Point 이미지 색상변경  
                    pictureBox_Loaded_BMP.Image = Changed_img;
                    dataGridView_Pixel_RGB_Display.Rows[Grid_Center_Y].Cells[Grid_Center_X].Style.ApplyStyle(style1);
                    dataGridView_Pixel_RGB_Display.Rows[Grid_Center_Y].Cells[Grid_Center_X + 1].Style.ApplyStyle(style1);
                    dataGridView_Pixel_RGB_Display.Rows[Grid_Center_Y].Cells[Grid_Center_X + 2].Style.ApplyStyle(style1);
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(100);
                    //중앙 글꼴 원복 + 이미지 원복
                    pictureBox_Loaded_BMP.Image = Current_img;
                    dataGridView_Pixel_RGB_Display.Rows[Grid_Center_Y].Cells[Grid_Center_X].Style.ApplyStyle(style2);
                    dataGridView_Pixel_RGB_Display.Rows[Grid_Center_Y].Cells[Grid_Center_X + 1].Style.ApplyStyle(style2);
                    dataGridView_Pixel_RGB_Display.Rows[Grid_Center_Y].Cells[Grid_Center_X + 2].Style.ApplyStyle(style2);
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(100);
                }


            }
        }

        private void pictureBox_Histo_R_Resize(object sender, EventArgs e)
        {
            pictureBox_Histo_R.Refresh();
        }

        private void pictureBox_Histo_G_Resize(object sender, EventArgs e)
        {
            pictureBox_Histo_G.Refresh();
        }

        private void pictureBox_Histo_B_Resize(object sender, EventArgs e)
        {
            pictureBox_Histo_B.Refresh();
        }

       

        private void pictureBox_Histo_R_Paint(object sender, PaintEventArgs e)
        {
            DrawHistogram(e.Graphics, pictureBox_Histo_R.BackColor, Histogram_R,
               pictureBox_Histo_R.ClientSize.Width, pictureBox_Histo_R.ClientSize.Height, Color.Red);
        }

        private void pictureBox_Histo_G_Paint(object sender, PaintEventArgs e)
        {
            DrawHistogram(e.Graphics, pictureBox_Histo_G.BackColor, Histogram_G,
               pictureBox_Histo_G.ClientSize.Width, pictureBox_Histo_G.ClientSize.Height, Color.Green);
        }

        private void pictureBox_Histo_B_Paint(object sender, PaintEventArgs e)
        {
            DrawHistogram(e.Graphics, pictureBox_Histo_B.BackColor, Histogram_B,
               pictureBox_Histo_B.ClientSize.Width, pictureBox_Histo_B.ClientSize.Height, Color.Blue);
        }

        // Draw a histogram.
        private void DrawHistogram(Graphics gr, Color back_color, int[] values, int width, int height, Color color)
        {

            gr.Clear(back_color);

            // Make a transformation to the PictureBox.
            RectangleF data_bounds = new RectangleF(0, 0, values.Length, Biggest_Max_RGB * 11 / 10);
            PointF[] points =
            {
                new PointF(0, height),
                new PointF(width, height),
                new PointF(0, 0)
            };
            Matrix transformation = new Matrix(data_bounds, points);
            gr.Transform = transformation;

            // Draw the histogram.
            using (Pen thin_pen = new Pen(color, 0))
            {
                for (int i = 0; i < values.Length; i++)
                {
                    RectangleF rect = new RectangleF(i, 0, 1, values[i]);
                    Brush the_brush = new SolidBrush(color);

                    gr.FillRectangle(the_brush, rect);
                    gr.DrawRectangle(thin_pen, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }

            gr.ResetTransform();
            gr.DrawRectangle(Pens.Black, 0, 0, width - 1, height - 1);

           
        }
        public void BMP_Status_AppendText_Nextline(string text, System.Drawing.Color color)
        {
            //Color (Text 색 바꾸고,AppendText)
            RichTextBox_BMP_Status.SelectionColor = color;
            RichTextBox_BMP_Status.AppendText(text + "\r\n");
            //Black (Text 색 원복 as ForeColor)
            RichTextBox_BMP_Status.SelectionColor = RichTextBox_BMP_Status.ForeColor;//System.Drawing.Color.Black;
            //Scroll to the end Without Focus
            RichTextBox_BMP_Status.SelectionStart = RichTextBox_BMP_Status.Text.Length;
            RichTextBox_BMP_Status.ScrollToCaret();
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_test_Click(object sender, EventArgs e)
        {
            //Test Code Here
            //System.Windows.Forms.MessageBox.Show(Math.Sqrt((0.3 * 1176 * 2400) / Math.PI).ToString());//520

            //Clear Status
            this.RichTextBox_BMP_Status.Clear();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        delegate void My_Delegate(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        private void button_Make_Multiple_Images_Click(object sender, EventArgs e)
        {
            int Resolution_X = 0;
            int Resolution_Y = 0;
            try
            {
                Resolution_X = Convert.ToInt32(textBox_BMP_Maker_Resolution_X.Text);
                Resolution_Y = Convert.ToInt32(textBox_BMP_Maker_Resolution_Y.Text);
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("X,Y Should Be Integer Values");
                return;
            }

            My_Delegate delegate_funcs = null;
            if (checkBox_Cross_Talk.Checked) delegate_funcs += bmp_maker.Cross_Talk;
            if (checkBox_Gray0_to_Gray7.Checked) delegate_funcs += bmp_maker.Gray0_to_Gray7;
            if (checkBox_Mura_Detect_Pattern.Checked) delegate_funcs += bmp_maker.Show_Mura_Detect_Pattern;
            if (checkBox_Pattern_40_Percent.Checked) delegate_funcs += bmp_maker.Pattern_40_Percent;
            if (checkBox_V_LbyL_Magenta_Green.Checked) delegate_funcs += bmp_maker.V_LbyL_Magenta_Green;
            if (checkBox_V_LbyL_Magenta_Green_Gradation.Checked) delegate_funcs += bmp_maker.V_LbyL_Magenta_Green_Gradation;
            if (checkBox_Five_Color_RYGCB_Pattern.Checked) delegate_funcs += bmp_maker.Five_Color_RYGCB_Pattern;
            if (checkBox_RGB_Gradation.Checked) delegate_funcs += bmp_maker.RGB_Gradation;
            if (checkBox_Color_Bar.Checked) delegate_funcs += bmp_maker.Color_Bar;
            if (checkBox_V_WRGB_Gradation.Checked) delegate_funcs += bmp_maker.V_WRGB_Gradation;
            if (checkBox_H_WRGB_Gradation.Checked) delegate_funcs += bmp_maker.H_WRGB_Gradation;
            if (checkBox_SH_All_IR_Drop_Pattern.Checked) delegate_funcs += bmp_maker.SH_All_IR_Drop_Pattern;
            if (checkBox_W_H_Gradation.Checked) delegate_funcs += bmp_maker.White_Horizentol_Gradation;
            if (checkBox_Mosaic.Checked) delegate_funcs += bmp_maker.Mosaic;
            if (checkBox_Cinema.Checked) delegate_funcs += bmp_maker.Cinema;
            if (checkBox_MyGradation.Checked) delegate_funcs += bmp_maker.MyGradation;
            
            if(delegate_funcs != null)
               delegate_funcs.Invoke(Resolution_X, Resolution_Y);

            if (checkBox_G63_Border.Checked)
            {
                int Top_Bottom_Lines = Convert.ToInt32(textBox_G63_Border_Top_Bottom_Line.Text);
                int Left_Right_Lines = Convert.ToInt32(textBox_G63_Border_Left_Right_Line.Text);
                bmp_maker.G63_Border(Resolution_X, Resolution_Y, Top_Bottom_Lines, Left_Right_Lines);
            }

            if (checkBox_Pseudo.Checked)
            {
                int Pseudo_Background_Gray = textBox_Pseudo_Background_Gray.Text.Dec_string_To_Dec_int();
                int Box_Pseudo_WRGB_Gray = textBox_Pseudo_WRGB_Gray.Text.Dec_string_To_Dec_int();
                bmp_maker.Pseudo(Resolution_X, Resolution_Y, Box_Pseudo_WRGB_Gray, Pseudo_Background_Gray);
            }

            if (checkBox_Dot_by_Dot_Pattern.Checked || checkBox_H_LByL.Checked || checkBox_V_LByL.Checked)
            {
                int First_Dot_or_Line_R = Convert.ToInt32(textBox_1st_Dot_or_Line_R.Text);
                int First_Dot_or_Line_G = Convert.ToInt32(textBox_1st_Dot_or_Line_G.Text);
                int First_Dot_or_Line_B = Convert.ToInt32(textBox_1st_Dot_or_Line_B.Text);

                int Second_Dot_or_Line_R = Convert.ToInt32(textBox_2nd_Dot_or_Line_R.Text);
                int Second_Dot_or_Line_G = Convert.ToInt32(textBox_2nd_Dot_or_Line_G.Text);
                int Second_Dot_or_Line_B = Convert.ToInt32(textBox_2nd_Dot_or_Line_B.Text);

                Color FirstColor = Color.FromArgb(First_Dot_or_Line_R, First_Dot_or_Line_G, First_Dot_or_Line_B);
                Color SecondColor = Color.FromArgb(Second_Dot_or_Line_R, Second_Dot_or_Line_G, Second_Dot_or_Line_B);

                if (checkBox_Dot_by_Dot_Pattern.Checked)
                {
                    int Dot_Size = Convert.ToInt32(textBox_Dot_Size.Text);
                    bmp_maker.One_Dot_Pattern(Resolution_X, Resolution_Y, FirstColor, SecondColor, Dot_Size);
                }

                if(checkBox_H_LByL.Checked)
                {
                    int firstLineLength = Convert.ToInt32(textBox_1st_Line_Num.Text);
                    int SecondLineLength = Convert.ToInt32(textBox_2nd_Line_Num.Text);
                    bmp_maker.H_LByL(Resolution_X, Resolution_Y, FirstColor, SecondColor, firstLineLength, SecondLineLength);
                    
                }

                if(checkBox_V_LByL.Checked)
                {
                    int firstLineLength = Convert.ToInt32(textBox_1st_Line_Num.Text);
                    int SecondLineLength = Convert.ToInt32(textBox_2nd_Line_Num.Text);
                    bmp_maker.V_LByL(Resolution_X, Resolution_Y, FirstColor, SecondColor, firstLineLength, SecondLineLength);
                }

            }

           



        }

        private void checkBox_Cross_Talk_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Cross_Talk.Checked) bmp_maker.Cross_Talk(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_Gray0_to_Gray7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Gray0_to_Gray7.Checked) bmp_maker.Gray0_to_Gray7(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_Mura_Detect_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Mura_Detect_Pattern.Checked) bmp_maker.Show_Mura_Detect_Pattern(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_Pattern_40_Percent_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Pattern_40_Percent.Checked) bmp_maker.Pattern_40_Percent(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_V_LbyL_Magenta_Green_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_V_LbyL_Magenta_Green.Checked) bmp_maker.V_LbyL_Magenta_Green(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_V_LbyL_Magenta_Green_Gradation_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_V_LbyL_Magenta_Green_Gradation.Checked) bmp_maker.V_LbyL_Magenta_Green_Gradation(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_Five_Color_RYGCB_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Five_Color_RYGCB_Pattern.Checked) bmp_maker.Five_Color_RYGCB_Pattern(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_H_LByL_CheckedChanged(object sender, EventArgs e)
        {
            int First_Dot_R = Convert.ToInt32(textBox_1st_Dot_or_Line_R.Text);
            int First_Dot_G = Convert.ToInt32(textBox_1st_Dot_or_Line_G.Text);
            int First_Dot_B = Convert.ToInt32(textBox_1st_Dot_or_Line_B.Text);
            int Second_Dot_R = Convert.ToInt32(textBox_2nd_Dot_or_Line_R.Text);
            int Second_Dot_G = Convert.ToInt32(textBox_2nd_Dot_or_Line_G.Text);
            int Second_Dot_B = Convert.ToInt32(textBox_2nd_Dot_or_Line_B.Text);

            Color FirstColor = Color.FromArgb(First_Dot_R, First_Dot_G, First_Dot_B);
            Color SecondColor = Color.FromArgb(Second_Dot_R, Second_Dot_G, Second_Dot_B);

            if (checkBox_H_LByL.Checked) bmp_maker.H_LByL(Picturebox_Resolution_X, Picturebox_Resolution_Y, FirstColor, SecondColor, 4, 4, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_V_LByL_CheckedChanged(object sender, EventArgs e)
        {
            int First_Dot_R = Convert.ToInt32(textBox_1st_Dot_or_Line_R.Text);
            int First_Dot_G = Convert.ToInt32(textBox_1st_Dot_or_Line_G.Text);
            int First_Dot_B = Convert.ToInt32(textBox_1st_Dot_or_Line_B.Text);
            int Second_Dot_R = Convert.ToInt32(textBox_2nd_Dot_or_Line_R.Text);
            int Second_Dot_G = Convert.ToInt32(textBox_2nd_Dot_or_Line_G.Text);
            int Second_Dot_B = Convert.ToInt32(textBox_2nd_Dot_or_Line_B.Text);

            Color FirstColor = Color.FromArgb(First_Dot_R, First_Dot_G, First_Dot_B);
            Color SecondColor = Color.FromArgb(Second_Dot_R, Second_Dot_G, Second_Dot_B);

            if (checkBox_V_LByL.Checked) bmp_maker.V_LByL(Picturebox_Resolution_X, Picturebox_Resolution_Y, FirstColor, SecondColor, 4, 4, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_RGB_Gradation_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_RGB_Gradation.Checked) bmp_maker.RGB_Gradation(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_Color_Bar_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Color_Bar.Checked) bmp_maker.Color_Bar(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_V_WRGB_Gradation_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_V_WRGB_Gradation.Checked) bmp_maker.V_WRGB_Gradation(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_H_WRGB_Gradation_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_H_WRGB_Gradation.Checked) bmp_maker.H_WRGB_Gradation(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();

        }

        private void checkBox_SH_All_IR_Drop_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SH_All_IR_Drop_Pattern.Checked) bmp_maker.SH_All_IR_Drop_Pattern(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_W_H_Gradation_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_W_H_Gradation.Checked) bmp_maker.White_Horizentol_Gradation(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_Mosaic_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Mosaic.Checked) bmp_maker.Mosaic(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_Cinema_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Cinema.Checked) bmp_maker.Cinema(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_G63_Border_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_G63_Border.Checked)
            {
                int Top_Bottom_Lines = Convert.ToInt32(textBox_G63_Border_Top_Bottom_Line.Text);
                int Left_Right_Lines = Convert.ToInt32(textBox_G63_Border_Left_Right_Line.Text);
                bmp_maker.G63_Border(Picturebox_Resolution_X, Picturebox_Resolution_Y, Top_Bottom_Lines, Left_Right_Lines, false);
            }
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_Pseudo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Pseudo.Checked)
            {
                int Pseudo_Background_Gray = textBox_Pseudo_Background_Gray.Text.Dec_string_To_Dec_int();
                int Box_Pseudo_WRGB_Gray = textBox_Pseudo_WRGB_Gray.Text.Dec_string_To_Dec_int();
                bmp_maker.Pseudo(Picturebox_Resolution_X, Picturebox_Resolution_Y, Box_Pseudo_WRGB_Gray, Pseudo_Background_Gray, false);
            }
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_One_Dot_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Dot_by_Dot_Pattern.Checked)
            {
                int First_Dot_R = Convert.ToInt32(textBox_1st_Dot_or_Line_R.Text);
                int First_Dot_G = Convert.ToInt32(textBox_1st_Dot_or_Line_G.Text);
                int First_Dot_B = Convert.ToInt32(textBox_1st_Dot_or_Line_B.Text);
                int Second_Dot_R = Convert.ToInt32(textBox_2nd_Dot_or_Line_R.Text);
                int Second_Dot_G = Convert.ToInt32(textBox_2nd_Dot_or_Line_G.Text);
                int Second_Dot_B = Convert.ToInt32(textBox_2nd_Dot_or_Line_B.Text);

                Color FirstColor = Color.FromArgb(First_Dot_R, First_Dot_G, First_Dot_B);
                Color SecondColor = Color.FromArgb(Second_Dot_R, Second_Dot_G, Second_Dot_B);

                bmp_maker.One_Dot_Pattern(Picturebox_Resolution_X, Picturebox_Resolution_Y, FirstColor, SecondColor, 12, false);
            }
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }

        private void checkBox_MyGradation_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_MyGradation.Checked) bmp_maker.MyGradation(Picturebox_Resolution_X, Picturebox_Resolution_Y, false);
            else bmp_maker.Set_Default_Black_Image();
            Application.DoEvents();
        }



        private void button_Clamp_Max_Gray_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.Clamp_Max_Gray(img);
            }

        }



        private void button_Back_To_Original_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(this.Original_img); 
        }

        private void button_Gamma_Correction_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                double Gamma = Convert.ToDouble(textBox_Gamma.Text);
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.Gamma_Encoding(img, Gamma);
            }

        }

        private void button_Gamma_Decoding_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                double Gamma = Convert.ToDouble(textBox_Gamma.Text);
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.Gamma_Decoding(img, Gamma);
            }

        }

        private void button_Change_Gray_Resolution_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                int grayResolutionBits = Convert.ToInt32(numericUpDown_gray_resolution_bits.Value);
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.Gray_Rosolution_Bit_Change(img, grayResolutionBits);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                label_PSNR.Text = "PSNR : " + img_process.Get_PSNR(Original_img, img).ToString();
                label_Resolution.Text = "Resolution (X,Y) : (" + img.Width + "," + img.Height + ")";
            }
        }

        private void button_RGB_to_Gray_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.RGB_to_Gray(img);
            }
        }

        private void button_Save_Current_Image_Click(object sender, EventArgs e)
        {
            pictureBox_Loaded_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Processed_Image.bmp");
        }

        private void button_3x3_Ave_Filter_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                double[][] Weight = new double[3][];
                Weight[0] = new double[3] { 1, 1, 1 };
                Weight[1] = new double[3] { 1, 1, 1 };
                Weight[2] = new double[3] { 1, 1, 1 };
                img_process.Filter_With_Weight(img, Weight);
            }
        }

        private void button_5x5_Ave_Filter_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                double[][] Weight = new double[5][];
                Weight[0] = new double[5] { 1, 1, 1, 1, 1 };
                Weight[1] = new double[5] { 1, 1, 1, 1, 1 };
                Weight[2] = new double[5] { 1, 1, 1, 1, 1 };
                Weight[3] = new double[5] { 1, 1, 1, 1, 1 };
                Weight[4] = new double[5] { 1, 1, 1, 1, 1 };
                img_process.Filter_With_Weight(img, Weight);
            }
        }

        private void button_histogram_equalization_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.histogram_equalization(img);
            }
        }

        private void button_Bit_Inversion_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.BitInversion(img);
            }
        }

        private void button_3x3_Meadian_Filter_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.Median_Filter_3x3(img);
            }
        }

        private void button_5x5_Meadian_Filter_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.Median_Filter_5x5(img);
            }
        }

        private void button_Laplace_Sharpness_Filter_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);

                double[][] Weight = new double[3][];
                Weight[0] = new double[3] { -0.25, -0.25, -0.25 };
                Weight[1] = new double[3] { -0.25, 3, -0.25 };
                Weight[2] = new double[3] { -0.25, -0.25, -0.25 };

                img_process.Filter_With_Weight(img, Weight);
            }
        }

        private void button_HighBoost_Filter_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                double SharpWeight = Convert.ToDouble(textBox_SharpNessWeight.Text);
                button_5x5_Ave_Filter.PerformClick();
                Bitmap AverageImg = new Bitmap(pictureBox_Loaded_BMP.Image);
               
                img_process.HighBoost(img, AverageImg, SharpWeight);
            }
        }

        private void button_1st_BMP_Load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    BMP_File_Name = dlg.FileName;

                    pictureBox_1st_BMP.Image = new Bitmap(dlg.FileName);
                    Original_img = new Bitmap(dlg.FileName);
                    FastBitmap fastBitmap_img = new FastBitmap(Original_img);

                    //Save file as CSV
                    if (checkBox_Save_RGB_Data_As_CSV.Checked)
                        SaveAsCSV(fastBitmap_img);

                    fastBitmap_img.Dispose();
                }
            }
        }

        private void button_2nd_BMP_Load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    BMP_File_Name = dlg.FileName;

                    pictureBox_2nd_BMP.Image = new Bitmap(dlg.FileName);
                    Original_img = new Bitmap(dlg.FileName);
                    FastBitmap fastBitmap_img = new FastBitmap(Original_img);

                    //Save file as CSV
                    if (checkBox_Save_RGB_Data_As_CSV.Checked)
                        SaveAsCSV(fastBitmap_img);

                    fastBitmap_img.Dispose();
                }
            }
        }

        private void button_Get_1st_2nd_PSNR_Click(object sender, EventArgs e)
        {
            if (pictureBox_1st_BMP.Image == null || pictureBox_2nd_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else if (pictureBox_1st_BMP.Image.Width != pictureBox_2nd_BMP.Image.Width
                || pictureBox_1st_BMP.Image.Height != pictureBox_2nd_BMP.Image.Height)
            {
                System.Windows.Forms.MessageBox.Show("the Two Images should have same resoltion(width and height)");
            }
            else
            {
                Bitmap img1 = new Bitmap(pictureBox_1st_BMP.Image);
                Bitmap img2 = new Bitmap(pictureBox_2nd_BMP.Image);
                label_1st_2nd_PSNR.Text = "PSNR : " + img_process.Get_PSNR(img1, img2).ToString();
                label_1st_2nd_Resolution.Text = "Resolution (X,Y) : (" + img1.Width + "," + img1.Height + ")";
            }
        }

        private void button_Image_Extraction_Background_As_Black_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                byte StartGray_R = Convert.ToByte(textBox_Image_Extraction_Start_Gray_R.Text);
                byte EndGray_R = Convert.ToByte(textBox_Image_Extraction_End_Gray_R.Text);

                byte StartGray_G = Convert.ToByte(textBox_Image_Extraction_Start_Gray_G.Text);
                byte EndGray_G = Convert.ToByte(textBox_Image_Extraction_End_Gray_G.Text);

                byte StartGray_B = Convert.ToByte(textBox_Image_Extraction_Start_Gray_B.Text);
                byte EndGray_B = Convert.ToByte(textBox_Image_Extraction_End_Gray_B.Text);

                if ((StartGray_R < EndGray_R && StartGray_R >= 0 && EndGray_R <= 255)
                    && (StartGray_G < EndGray_G && StartGray_G >= 0 && EndGray_G <= 255)
                    && (StartGray_B < EndGray_B && StartGray_B >= 0 && EndGray_B <= 255))
                {
                    Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                    img_process.ImageExtractionBankGroudBlack(StartGray_R, EndGray_R, StartGray_G, EndGray_G, StartGray_B, EndGray_B, img);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Invalis StartGray and EndGray");
                }
            }
        }

        private void button_Image_Extraction_Foreground_As_White_Click(object sender, EventArgs e)
        {

            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                byte StartGray_R = Convert.ToByte(textBox_Image_Extraction_Start_Gray_R.Text);
                byte EndGray_R = Convert.ToByte(textBox_Image_Extraction_End_Gray_R.Text);

                byte StartGray_G = Convert.ToByte(textBox_Image_Extraction_Start_Gray_G.Text);
                byte EndGray_G = Convert.ToByte(textBox_Image_Extraction_End_Gray_G.Text);

                byte StartGray_B = Convert.ToByte(textBox_Image_Extraction_Start_Gray_B.Text);
                byte EndGray_B = Convert.ToByte(textBox_Image_Extraction_End_Gray_B.Text);

                if ((StartGray_R < EndGray_R && StartGray_R >= 0 && EndGray_R <= 255)
                    && (StartGray_G < EndGray_G && StartGray_G >= 0 && EndGray_G <= 255)
                    && (StartGray_B < EndGray_B && StartGray_B >= 0 && EndGray_B <= 255))
                {
                    Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                    img_process.ImageExtractionForeGroudWhite(StartGray_R, EndGray_R, StartGray_G, EndGray_G, StartGray_B, EndGray_B, img);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Invalis StartGray and EndGray");
                }
            }
        }

        private void button_RGB_to_CYMK_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.RGB_to_CYMK(img);

            }
        }

        private void button_RGB_to_CYM_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.RGB_to_CYM(img);

            }
        }

        public void Set_pictureBox_Loaded_BMP_Image(Image image)
        {
            if(image != null)
            {
                pictureBox_Loaded_BMP.Image = image;
                label_Resolution.Text = "Resolution (X,Y) : (" + image.Width + "," + image.Height + ")";
            }  
        }

        private void pictureBox_CYM_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_CYM.Image);
            
        }
        private void pictureBox_Cyan_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_Cyan.Image);
        }

        private void pictureBox_Magenta_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_Magenta.Image);
        }

        private void pictureBox_Yellow_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_Yellow.Image);
        }

        private void pictureBox_Black_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_Black.Image);
        }

        private void pictureBox_1st_BMP_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_1st_BMP.Image);
        }

        private void pictureBox_2nd_BMP_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_2nd_BMP.Image);
        }

        private void pictureBox_To_be_created_BMP_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_To_be_created_BMP.Image);
        }

        private void pictureBox_Histo_G_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox_Histo_R_Click(object sender, EventArgs e)
        {
         
        }
        
        private void pictureBox_Histo_B_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_CIE_XY_Selected_Area_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_CIE_XY_Selected_Area.Image);
        }

        private void pictureBox_CIE_XY_Selected_Area_Ave_Color_Click(object sender, EventArgs e)
        {
            Set_pictureBox_Loaded_BMP_Image(pictureBox_CIE_XY_Selected_Area_Ave_Color.Image);
        }

        private void button_Get_Average_ColorCoordinate_xy_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {

                int from_row = Convert.ToInt32(textBox_xy_From_Row.Text);
                int from_col = Convert.ToInt32(textBox_xy_From_Col.Text);

                int to_row = Convert.ToInt32(textBox_xy_To_Row.Text);
                int to_col = Convert.ToInt32(textBox_xy_To_Col.Text);

                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);

                if (from_row < to_row && from_col < to_col 
                    && 0 < from_row && to_row < img.Height
                     && 0 < from_col && to_col < img.Width)
                {
                    Color AveColor = img_process.Get_Color_Coordinate_XY(img,from_row, to_row, from_col, to_col);
                    label_Ave_RGB.Text = "Ave R/G/B : " + AveColor.R + "/" + AveColor.G + "/" + AveColor.B;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("The From(Row,Col) or To(Row,Col) is Invalid Input");
                }
            }
        }

        private void button_Black_or_White_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                byte gray_threshold = Convert.ToByte(textBox_Bi_Value.Text);
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.BiModel_Black_White(img, gray_threshold);
            }
        }

        private void button_White_or_Black_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                byte gray_threshold = Convert.ToByte(textBox_Bi_Value.Text);
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.BiModel_White_Black(img, gray_threshold);

            }
        }

        private void button_Create_Dot_Noise_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                int Dot_R = Convert.ToInt32(textBox_Dot_R.Text);
                int Dot_G = Convert.ToInt32(textBox_Dot_G.Text);
                int Dot_B = Convert.ToInt32(textBox_Dot_B.Text);
                Color DotColor = Color.FromArgb(Dot_R, Dot_G, Dot_B);

                int Dot_Num = Convert.ToInt32(textBox_Dot_Num.Text);



                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                img_process.Dot_Noise_Creation(img, DotColor, Dot_Num);

            }
        }


        

        private void button_Edge_Detection_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);

                //----------1------
                if (checkBox_Horizontal_Edge_Detection.Checked
                    && checkBox_Vertical_Edge_Detection.Checked == false
                    && checkBox_45degree_Edge_Detection.Checked == false
                    && checkBox_225degree_Edge_Detection.Checked == false)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Horizontal_Detection_Weight());
                }
                else if (checkBox_Horizontal_Edge_Detection.Checked == false
                   && checkBox_Vertical_Edge_Detection.Checked
                   && checkBox_45degree_Edge_Detection.Checked == false
                   && checkBox_225degree_Edge_Detection.Checked == false)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Vertical_Detection_Weight());
                }
                else if (checkBox_Horizontal_Edge_Detection.Checked == false
                   && checkBox_Vertical_Edge_Detection.Checked == false
                   && checkBox_45degree_Edge_Detection.Checked
                   && checkBox_225degree_Edge_Detection.Checked == false)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_45degree_Detection_Weight());
                }
                else if (checkBox_Horizontal_Edge_Detection.Checked == false
                   && checkBox_Vertical_Edge_Detection.Checked == false
                   && checkBox_45degree_Edge_Detection.Checked == false
                   && checkBox_225degree_Edge_Detection.Checked)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_225degree_Detection_Weight());
                }

                //----------2------
                else if (checkBox_Horizontal_Edge_Detection.Checked
                   && checkBox_Vertical_Edge_Detection.Checked
                   && checkBox_45degree_Edge_Detection.Checked == false
                   && checkBox_225degree_Edge_Detection.Checked == false)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Horizontal_Detection_Weight(), EdgeFilter.Edge_Vertical_Detection_Weight()); 
                }
                else if (checkBox_Horizontal_Edge_Detection.Checked
                   && checkBox_Vertical_Edge_Detection.Checked == false
                   && checkBox_45degree_Edge_Detection.Checked 
                   && checkBox_225degree_Edge_Detection.Checked == false)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Horizontal_Detection_Weight(), EdgeFilter.Edge_45degree_Detection_Weight());
                }
                else if (checkBox_Horizontal_Edge_Detection.Checked
                   && checkBox_Vertical_Edge_Detection.Checked == false
                   && checkBox_45degree_Edge_Detection.Checked == false
                   && checkBox_225degree_Edge_Detection.Checked )
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Horizontal_Detection_Weight(), EdgeFilter.Edge_225degree_Detection_Weight());
                }
                else if (checkBox_Horizontal_Edge_Detection.Checked == false
                   && checkBox_Vertical_Edge_Detection.Checked 
                   && checkBox_45degree_Edge_Detection.Checked 
                   && checkBox_225degree_Edge_Detection.Checked == false)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Vertical_Detection_Weight(), EdgeFilter.Edge_45degree_Detection_Weight());
                }

                else if (checkBox_Horizontal_Edge_Detection.Checked == false
                   && checkBox_Vertical_Edge_Detection.Checked
                   && checkBox_45degree_Edge_Detection.Checked == false
                   && checkBox_225degree_Edge_Detection.Checked )
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Vertical_Detection_Weight(), EdgeFilter.Edge_225degree_Detection_Weight());
                }

                else if (checkBox_Horizontal_Edge_Detection.Checked == false
                   && checkBox_Vertical_Edge_Detection.Checked == false
                   && checkBox_45degree_Edge_Detection.Checked 
                   && checkBox_225degree_Edge_Detection.Checked)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_45degree_Detection_Weight(), EdgeFilter.Edge_225degree_Detection_Weight());
                }

                //----------3------
                else if (checkBox_Horizontal_Edge_Detection.Checked == false
                   && checkBox_Vertical_Edge_Detection.Checked
                   && checkBox_45degree_Edge_Detection.Checked 
                   && checkBox_225degree_Edge_Detection.Checked)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Vertical_Detection_Weight(), EdgeFilter.Edge_45degree_Detection_Weight(), EdgeFilter.Edge_225degree_Detection_Weight());
                }

                else if (checkBox_Horizontal_Edge_Detection.Checked 
                   && checkBox_Vertical_Edge_Detection.Checked == false
                   && checkBox_45degree_Edge_Detection.Checked
                   && checkBox_225degree_Edge_Detection.Checked)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Horizontal_Detection_Weight(), EdgeFilter.Edge_45degree_Detection_Weight(), EdgeFilter.Edge_225degree_Detection_Weight());
                }

                else if (checkBox_Horizontal_Edge_Detection.Checked
                   && checkBox_Vertical_Edge_Detection.Checked
                   && checkBox_45degree_Edge_Detection.Checked == false
                   && checkBox_225degree_Edge_Detection.Checked)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Horizontal_Detection_Weight(), EdgeFilter.Edge_Vertical_Detection_Weight(), EdgeFilter.Edge_225degree_Detection_Weight());
                }

                else if (checkBox_Horizontal_Edge_Detection.Checked
                   && checkBox_Vertical_Edge_Detection.Checked
                   && checkBox_45degree_Edge_Detection.Checked
                   && checkBox_225degree_Edge_Detection.Checked == false)
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Horizontal_Detection_Weight(), EdgeFilter.Edge_Vertical_Detection_Weight(), EdgeFilter.Edge_45degree_Detection_Weight());
                }

                //----------4------
                else if (checkBox_Horizontal_Edge_Detection.Checked
                   && checkBox_Vertical_Edge_Detection.Checked
                   && checkBox_45degree_Edge_Detection.Checked
                   && checkBox_225degree_Edge_Detection.Checked )
                {
                    img_process.Filter_With_Weight(img, EdgeFilter.Edge_Horizontal_Detection_Weight(), EdgeFilter.Edge_Vertical_Detection_Weight(), EdgeFilter.Edge_45degree_Detection_Weight(), EdgeFilter.Edge_225degree_Detection_Weight());
                }





            }
        }

        private void button_Dot_Detection_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                double[][] Weight = new double[3][];
                Weight[0] = new double[3] { -1, -1, -1 };
                Weight[1] = new double[3] { -1, 8, -1 };
                Weight[2] = new double[3] { -1, -1, -1 };
                img_process.Filter_With_Weight(img, Weight);
            }
        }

        private void button_Resize_Image_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                int Resized_Width = Convert.ToInt32(textBox_resized_width.Text);
                int Resized_Height = Convert.ToInt32(textBox_resized_height.Text);

                if(radioButton_Resize_Nearest_Interpolation.Checked)
                {
                    Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Nearest_Interpolation(img, Resized_Width, Resized_Height)); 
                }
                else if(radioButton_Resize_Bilinear_Interpolation.Checked)
                {
                    Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Bilinear_Interpolation(img, Resized_Width, Resized_Height));
                }
            }
        }


        private void button_Change_Image_Alpha_Value_Click(object sender, EventArgs e)
        {

            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                int Alpha = Convert.ToInt32(textBox_Image_Alpha_Value.Text);
                if (Alpha > 255)
                {
                    Alpha = 255;
                    textBox_Image_Alpha_Value.Text = Alpha.ToString();
                }
                img_process.TransPalencyChange(img, Convert.ToByte(Alpha));

            }
        }

        private void button_Erosion_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else if (Convert.ToInt32(textBox_Kernel_Length.Text) % 2 == 0 || Convert.ToInt32(textBox_Kernel_Length.Text) == 1)
                System.Windows.Forms.MessageBox.Show("Kernel Size should be odd number and bigger than 1");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                int Length = Convert.ToInt32(textBox_Kernel_Length.Text);

                if(radioButton_Morphological_Square_Kernel.Checked)
                    img_process.Dilation_or_Erosion_With_Square_Kernel(img, Length, Morphological_Operation.Erosion);
                else if(radioButton_Morphological_Circle_Kernel.Checked)
                    img_process.Dilation_or_Erosion_With_Circle_Kernel(img, Length, Morphological_Operation.Erosion);

            }
        }

        private void button_Dilation_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else if(Convert.ToInt32(textBox_Kernel_Length.Text) % 2 == 0 || Convert.ToInt32(textBox_Kernel_Length.Text) == 1)
                System.Windows.Forms.MessageBox.Show("Kernel Size should be odd number and bigger than 1");
            else
            {
                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                int Length = Convert.ToInt32(textBox_Kernel_Length.Text);

                if (radioButton_Morphological_Square_Kernel.Checked)
                    img_process.Dilation_or_Erosion_With_Square_Kernel(img, Length, Morphological_Operation.Dilation);
                else if (radioButton_Morphological_Circle_Kernel.Checked)
                    img_process.Dilation_or_Erosion_With_Circle_Kernel(img, Length, Morphological_Operation.Dilation);

            }
        }

        private void trackBar_Histo_Y_Scale_ValueChanged(object sender, EventArgs e)
        {
            if(Histo_Origin_Y_Scale != 0)
            {
                Biggest_Max_RGB = Histo_Origin_Y_Scale / trackBar_Histo_Y_Scale.Value;
                pictureBox_Histo_R.Refresh();
                pictureBox_Histo_G.Refresh();
                pictureBox_Histo_B.Refresh();
            }
                
        }

        private void pictureBox_Loaded_BMP_MouseHover(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
        }

        private void pictureBox_Loaded_BMP_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image != null)
            {
                toolTip1.RemoveAll();
                int X = Convert.ToInt32(e.X / Convert.ToDouble(pictureBox_Loaded_BMP.Width) * Convert.ToDouble(pictureBox_Loaded_BMP.Image.Width));
                int Y = Convert.ToInt32(e.Y / Convert.ToDouble(pictureBox_Loaded_BMP.Height) * Convert.ToDouble(pictureBox_Loaded_BMP.Image.Height));
                toolTip1.Show("(X,Y): " + "(" + X + "," + Y, pictureBox_Loaded_BMP, e.X, e.Y);
            }
        }

        private void button_Resize_Image_Without_Change_Specific_Area_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                int from_col = Convert.ToInt32(textBox_Resize_Without_From_x.Text);
                int from_row = Convert.ToInt32(textBox_Resize_Without_From_y.Text);

                int to_col = Convert.ToInt32(textBox_Resize_Without_To_x.Text);
                int to_row = Convert.ToInt32(textBox_Resize_Without_To_y.Text);

                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                if (from_row < to_row && from_col < to_col
                    && 0 < from_row && to_row < img.Height
                    && 0 < from_col && to_col < img.Width)
                {
                    int Resized_Width = Convert.ToInt32(textBox_resized_width.Text);
                    int Resized_Height = Convert.ToInt32(textBox_resized_height.Text);

                    if (radioButton_Resize_Nearest_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Nearest_Interpolation_Without_Change_Specific_Area(img, Resized_Width, Resized_Height, from_col, to_col, from_row, to_row));
                    }
                    else if (radioButton_Resize_Bilinear_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Bilinear_Interpolation_Without_Change_Specific_Area(img, Resized_Width, Resized_Height, from_col, to_col, from_row, to_row));
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("The From(Row,Col) or To(Row,Col) is Invalid Input");
                }
            }
        }

        private void button_Only_Resize_Left_to_X_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                int X = Convert.ToInt32(textBox_Resize_Position.Text);

                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                if (X < img.Width)
                {
                    int Resized_Width = Convert.ToInt32(textBox_resized_width.Text);
                    int Resized_Height = Convert.ToInt32(textBox_resized_height.Text);

                    if (radioButton_Resize_Nearest_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Nearest_Interpolation_Only_Left_Area(img, X, Resized_Width));
                    }
                    else if (radioButton_Resize_Bilinear_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Bilinear_Interpolation_Only_Left_Area(img, X, Resized_Width));
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("X is out of Original Image");
                }
            }
        }

        private void button_Only_Resize_X_to_Right_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                int X = Convert.ToInt32(textBox_Resize_Position.Text);

                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                if (X < img.Width)
                {
                    int Resized_Width = Convert.ToInt32(textBox_resized_width.Text);
                    int Resized_Height = Convert.ToInt32(textBox_resized_height.Text);

                    if (radioButton_Resize_Nearest_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Nearest_Interpolation_Only_Right_Area(img, X, Resized_Width));
                    }
                    else if (radioButton_Resize_Bilinear_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Bilinear_Interpolation_Only_Right_Area(img, X, Resized_Width));
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("X is out of Original Image");
                }
            }
        }

        private void button_Only_Resize_Top_to_X_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                int Y = Convert.ToInt32(textBox_Resize_Position.Text);

                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                if (Y < img.Height)
                {
                    int Resized_Width = Convert.ToInt32(textBox_resized_width.Text);
                    int Resized_Height = Convert.ToInt32(textBox_resized_height.Text);

                    if (radioButton_Resize_Nearest_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Nearest_Interpolation_Only_Top_Area(img, Y, Resized_Height));
                    }
                    else if (radioButton_Resize_Bilinear_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Bilinear_Interpolation_Only_Top_Area(img, Y, Resized_Height));
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("X is out of Original Image");
                }
            }
        }

        private void button_Only_Resize_X_to_Bottom_Click(object sender, EventArgs e)
        {
            if (pictureBox_Loaded_BMP.Image == null)
                System.Windows.Forms.MessageBox.Show("Please Load BMP First");
            else
            {
                int Y = Convert.ToInt32(textBox_Resize_Position.Text);

                Bitmap img = new Bitmap(pictureBox_Loaded_BMP.Image);
                if (Y < img.Height)
                {
                    int Resized_Width = Convert.ToInt32(textBox_resized_width.Text);
                    int Resized_Height = Convert.ToInt32(textBox_resized_height.Text);

                    if (radioButton_Resize_Nearest_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Nearest_Interpolation_Only_Bottom_Area(img, Y, Resized_Height));
                    }
                    else if (radioButton_Resize_Bilinear_Interpolation.Checked)
                    {
                        Set_pictureBox_Loaded_BMP_Image(img_process.Resize_Bilinear_Interpolation_Only_Bottom_Area(img, Y, Resized_Height));
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("X is out of Original Image");
                }
            }
        }
    }
}
