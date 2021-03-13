using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//Histogram
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace PNC_Csharp
{
    interface BMP
    {
        void V_LbyL_Magenta_Green(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Pseudo(int Resolution_X, int Resolution_Y, int WRGB_Gray, int Background_Gray, bool Save_Image = true);
        void G63_Border(int Resolution_X, int Resolution_Y, int Top_Bottom_Lines, int Left_Right_Lines, bool Save_Image = true);
        void V_LbyL_Magenta_Green_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Pattern_40_Percent(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Show_Mura_Detect_Pattern(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Gray0_to_Gray7(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Cross_Talk(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Five_Color_RYGCB_Pattern(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void H_LByL(int Resolution_X, int Resolution_Y, Color FirstColor, Color SecondColor, int firstLineLength,int SecondLineLength, bool Save_Image = true);
        void V_LByL(int Resolution_X, int Resolution_Y, Color FirstColor, Color SecondColor, int firstLineLength, int SecondLineLength, bool Save_Image = true);
        void RGB_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Color_Bar(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void V_WRGB_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void H_WRGB_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void One_Dot_Pattern(int Resolution_X, int Resolution_Y, Color FirstColor, Color SecondColor, int Dot_Size, bool Save_Image = true);
        void SH_All_IR_Drop_Pattern(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Test_Pattern(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void White_Horizentol_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Cinema(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Mosaic(int Resolution_X, int Resolution_Y, bool Save_Image = true);
        void Set_Default_Black_Image();

        void MyGradation(int Resolution_X, int Resolution_Y, bool Save_Image = true);
    }




    class BMP_Maker : BMP
    {
        Bitmap default_image;

        public BMP_Maker()
        {
            default_image = new Bitmap(100, 100);
            Initialize_Default_Imgae();
        }

        private void Initialize_Default_Imgae()
        {
            for (int i = 0; i < default_image.Height; i++)
                for (int j = 0; j < default_image.Width; j++)
                    default_image.SetPixel(j, i, Color.White);
        }
        public void Set_Default_Black_Image()
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = default_image;
        }
        public void Cinema(int Resolution_X, int Resolution_Y,bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];

            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Height = img.Height;
                int Width = img.Width;

                int White_Height = Convert.ToInt16((Width / 9.0) * 16.0);
                int Black_Height = Convert.ToInt16((Height - White_Height) / 2.0);


                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = Height;

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (i < Black_Height)
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        else if (i < (Black_Height + White_Height))
                        {
                            myRgbColor = Color.FromArgb(255, 255, 255);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;

                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Cinema.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Cinema.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Cinema.bmp NG", Color.Red);
            }
        }

        public void Mosaic(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);
                int Height = img.Height;
                int Each_Height = (img.Height / 2);

                int Width = img.Width;
                Color Fore_RgbColor = Color.FromArgb(255, 255, 255);
                Color Back_RgbColor = Color.FromArgb(0, 0, 0);
                bmp_image_processing_form.progressBar1.Maximum = Height;

                for (int i = 0; i < Each_Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (j % 2 == 0)
                        {
                            if (i % 2 == 0) img.SetPixel(j, i, Fore_RgbColor);
                            else img.SetPixel(j, i, Back_RgbColor);
                        }
                        else
                        {
                            if (i % 2 == 0) img.SetPixel(j, i, Back_RgbColor);
                            else img.SetPixel(j, i, Fore_RgbColor);
                        }
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }

                for (int i = Each_Height; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (j % 4 < 2)
                        {
                            if (i % 4 < 2) img.SetPixel(j, i, Fore_RgbColor);
                            else img.SetPixel(j, i, Back_RgbColor);
                        }
                        else
                        {
                            if (i % 4 < 2) img.SetPixel(j, i, Back_RgbColor);
                            else img.SetPixel(j, i, Fore_RgbColor);
                        }
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }

                bmp_image_processing_form.progressBar1.Value = 0;
                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Mosaic.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Mosaic.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Mosaic.bmp NG", Color.Red);
            }
        }

        public void White_Horizentol_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                //W/R/G/B Gradation
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                double r = 255.0 / img.Width;

                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = img.Width;
                for (int i = 0; i < img.Width; i++)
                {
                    int Gray = Convert.ToInt16(i * r);
                    for (int j = 0; j < img.Height; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, Gray, Gray);
                        img.SetPixel(i, j, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\W_H_Gradation.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making W_H_Gradation.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making W_H_Gradation.bmp NG", Color.Red);
            }
        }


        public void Test_Pattern(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);
                int Height = img.Height;
                int Width = img.Width;
                Color myRgbColor;

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (j < (Width / 3) || j > (2 * Width / 3))
                        {
                            myRgbColor = Color.FromArgb(255, 255, 255);
                        }
                        else
                        {
                            if (i % 10 < 5) myRgbColor = Color.FromArgb(255, 255, 255);
                            else myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                }
                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Test_Pattern.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Test_Pattern OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Test_Pattern NG", Color.Red);
            }
        }

        public void SH_All_IR_Drop_Pattern(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            bmp_image_processing_form.progressBar1.Maximum = 24;
            bmp_image_processing_form.progressBar1.Value = 0;

            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 255, 0, 0, "R", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 0, 255, 0, "G", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 0, 0, 255, "B", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 255, 255, 0, "Y", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 0, 255, 255, "C", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 255, 0, 255, "M", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 115, 82, 66, "C1", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 194, 150, 130, "C2", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 94, 122, 156, "C3", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 89, 107, 66, "C4", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 130, 128, 176, "C5", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 99, 189, 168, "C6", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 217, 120, 41, "C7", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 74, 92, 163, "C8", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 194, 84, 97, "C9", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 92, 61, 107, "C10", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 158, 186, 64, "C11", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 230, 161, 46, "C12", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 51, 61, 150, "C13", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 71, 148, 71, "C14", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 176, 48, 59, "C15", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 237, 199, 33, "C16", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 186, 84, 145, "C17", Save_Image); bmp_image_processing_form.progressBar1.Value++;
            IR_Drop_Detect_Pattern_Maker(Resolution_X, Resolution_Y, 0, 133, 163, "C18", Save_Image); bmp_image_processing_form.progressBar1.Value++;

            bmp_image_processing_form.progressBar1.Value = 0;
        }


        public void IR_Drop_Detect_Pattern_Maker(int Resolution_X, int Resolution_Y, byte Red, byte Green, byte Blue, string BMP_File_Name, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                // IR Drop 
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);
                int Center_X = img.Width / 2;
                int Center_Y = img.Height / 2;
                //double R = 520; //Area's 30% Circle
                double R = Math.Sqrt((0.3 * Resolution_X * Resolution_Y) / Math.PI);

                int Height = img.Height;
                int Width = img.Width;
                Color myRgbColor;

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if ((Math.Pow((i - Center_Y), 2) + Math.Pow((j - Center_X), 2)) < Math.Pow(R, 2))
                        {
                            myRgbColor = Color.FromArgb(Red, Green, Blue);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(255, 255, 255);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                }

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\" + "IR_Drop_Pattern_" + BMP_File_Name + ".bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making IR_Drop_Pattern_" + BMP_File_Name + " OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making IR_Drop_Pattern_" + BMP_File_Name + " NG", Color.Red);
            }
        }





        public void One_Dot_Pattern(int Resolution_X, int Resolution_Y, Color FirstColor, Color SecondColor, int Dot_Size, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Height = img.Height;
                int Width = img.Width;

                int length = 2 * Dot_Size;

                int TempHeight = 0;
                int TempWidth = 0;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    if (i % length == 0)
                        TempHeight = i;

                    for (int j = 0; j < Width; j++)
                    {
                        if (j % length == 0)
                            TempWidth = j;

                        if (i < TempHeight + Dot_Size)
                        {
                            if (j < TempWidth + Dot_Size)
                                img.SetPixel(j, i, FirstColor);
                            else
                                img.SetPixel(j, i, SecondColor);
                        }
                        else
                        {
                            if (j < TempWidth + Dot_Size)
                                img.SetPixel(j, i, SecondColor);
                            else
                                img.SetPixel(j, i, FirstColor);
                        }
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\One_Dot_Pattern.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making One_Dot_Pattern.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making One_Dot_Pattern.bmp NG", Color.Red);
            }
        }


        public void H_WRGB_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                //W/R/G/B Gradation
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                double r = 255.0 / img.Width;
                int Each_Height = img.Height / 4; //(1440/4)

                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = img.Width;
                for (int i = 0; i < img.Width; i++)
                {
                    int Gray = Convert.ToInt16(i * r);
                    for (int j = 0; j < Each_Height; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, 0, 0);
                        img.SetPixel(i, j, myRgbColor);
                    }
                    for (int j = Each_Height; j < Each_Height * 2; j++)
                    {
                        myRgbColor = Color.FromArgb(0, Gray, 0);
                        img.SetPixel(i, j, myRgbColor);
                    }
                    for (int j = Each_Height * 2; j < Each_Height * 3; j++)
                    {
                        myRgbColor = Color.FromArgb(0, 0, Gray);
                        img.SetPixel(i, j, myRgbColor);
                    }
                    for (int j = Each_Height * 3; j < img.Height; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, Gray, Gray);
                        img.SetPixel(i, j, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\H_WRGB_Gradation.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making H_WRGB_Gradation.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making H_WRGB_Gradation.bmp NG", Color.Red);
            }
        }

        public void V_WRGB_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                //W/R/G/B Gradation
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                double r = 255.0 / img.Height;
                int Each_Width = img.Width / 4; //(1440/4)
                int Width = img.Width;
                int Height = img.Height;

                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < img.Height; i++)
                {
                    int Gray = Convert.ToInt16(i * r);
                    for (int j = 0; j < Each_Width; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, 0, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    for (int j = Each_Width; j < Each_Width * 2; j++)
                    {
                        myRgbColor = Color.FromArgb(0, Gray, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    for (int j = Each_Width * 2; j < Each_Width * 3; j++)
                    {
                        myRgbColor = Color.FromArgb(0, 0, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    for (int j = Each_Width * 3; j < Each_Width * 4; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, Gray, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\WRGB_Gradation.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making WRGB_Gradation.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making WRGB_Gradation.bmp NG", Color.Red);
            }
        }

        public void Color_Bar(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                //DP116 Color Bar
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Height = img.Height;
                int Each_Width = img.Width / 8;
                Color myRgbColor;
                double r = 255.0 / Height;

                int Gray = 0;
                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    Gray = Convert.ToInt16((i) * r);
                    for (int j = 0; j < Each_Width; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, Gray, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }

                    for (int j = Each_Width; j < Each_Width * 2; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, Gray, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }

                    for (int j = Each_Width * 2; j < Each_Width * 3; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, 0, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }

                    for (int j = Each_Width * 3; j < Each_Width * 4; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, 0, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }

                    for (int j = Each_Width * 4; j < Each_Width * 5; j++)
                    {
                        myRgbColor = Color.FromArgb(0, Gray, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }

                    for (int j = Each_Width * 5; j < Each_Width * 6; j++)
                    {
                        myRgbColor = Color.FromArgb(0, Gray, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }

                    for (int j = Each_Width * 6; j < Each_Width * 7; j++)
                    {
                        myRgbColor = Color.FromArgb(0, 0, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }

                    for (int j = Each_Width * 7; j < Each_Width * 8; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, Gray, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Color_Bar.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Color_Bar.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Color_Bar.bmp NG", Color.Red);
            }
        }


        public void RGB_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                //DP116 RGB Gradation
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Each_Height = img.Height / 3;
                int Width = img.Width;
                int Height = img.Height;
                Color myRgbColor;
                double r = 255.0 / Each_Height;

                int Gray = 0;
                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Each_Height; i++)
                {
                    Gray = Convert.ToInt16((i) * r);
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, 0, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }

                for (int i = Each_Height; i < Each_Height * 2; i++)
                {
                    Gray = Convert.ToInt16((i - Each_Height) * r);
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(0, Gray, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }

                for (int i = Each_Height * 2; i < Each_Height * 3; i++)
                {
                    Gray = Convert.ToInt16((i - Each_Height * 2) * r);
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(0, 0, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\RGB_Gradation.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making RGB_Gradation.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making RGB_Gradation.bmp NG", Color.Red);
            }
        }



        public void V_LByL(int Resolution_X, int Resolution_Y, Color FirstColor, Color SecondColor, int firstLineLength, int SecondLineLength, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Height = img.Height;
                int Width = img.Width;
                
                int Length = firstLineLength + SecondLineLength;
                int TempWidth = 0;

                bmp_image_processing_form.progressBar1.Maximum = Width;
                for (int j = 0; j < Width; j++)
                {
                    if (j % Length == 0)
                        TempWidth = j;

                    for (int i = 0; i < Height; i++)
                    {
                        if (j < TempWidth + firstLineLength)
                            img.SetPixel(j, i, FirstColor);
                        else
                            img.SetPixel(j, i, SecondColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = j;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\V_LByL.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making V_LByL.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making V_LByL.bmp NG", Color.Red);
            }
        }


        public void H_LByL(int Resolution_X, int Resolution_Y, Color FirstColor, Color SecondColor, int firstLineLength, int SecondLineLength, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Height = img.Height;
                int Width = img.Width;
                
                int Length = firstLineLength + SecondLineLength;
                int TempHeight = 0;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    if (i % Length == 0)
                        TempHeight = i;

                    for (int j = 0; j < Width; j++)
                    {
                        if (i < TempHeight + firstLineLength)
                            img.SetPixel(j, i, FirstColor);
                        else
                            img.SetPixel(j, i, SecondColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\H_LByL.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making H_LByL.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making H_LByL.bmp NG", Color.Red);
            }
        }



        public void Five_Color_RYGCB_Pattern(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Height = img.Height;
                int Width = img.Width;

                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (i < (Height / 5) * 1)
                        {
                            myRgbColor = Color.FromArgb(255, 0, 0);
                        }
                        else if (i < (Height / 5) * 2)
                        {
                            myRgbColor = Color.FromArgb(255, 255, 0);
                        }
                        else if (i < (Height / 5) * 3)
                        {
                            myRgbColor = Color.FromArgb(0, 255, 0);
                        }
                        else if (i < (Height / 5) * 4)
                        {
                            myRgbColor = Color.FromArgb(0, 255, 255);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(0, 0, 255);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Five_Color_RYGCB_Pattern.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Five_Color_RYGCB_Pattern.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Five_Color_RYGCB_Pattern.bmp NG", Color.Red);
            }
        }

        public void Cross_Talk(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Each_Height = img.Height / 3;
                int Each_Width = img.Width / 3;

                if ((Each_Height * 3) != img.Height)
                {
                    System.Windows.Forms.MessageBox.Show("Y should be multiple of 3");
                    throw new Exception();
                }
                if ((Each_Width * 3) != img.Width)
                {
                    System.Windows.Forms.MessageBox.Show("X should be multiple of 3");
                    throw new Exception();
                }

                int Width = img.Width;
                int Height = img.Height;

                Color myRgbColor;
                double r = 255.0 / Each_Height;

                int Gray = 0;
                bmp_image_processing_form.progressBar1.Maximum = img.Height;
                for (int i = 0; i < Each_Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(255, 255, 255);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }

                for (int i = Each_Height; i < Each_Height * 2; i++)
                {
                    Gray = Convert.ToInt16((i - Each_Height) * r);
                    for (int j = 0; j < Width; j++)
                    {
                        if (j < Each_Width)
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        else if (j < (Each_Width * 2))
                        {
                            myRgbColor = Color.FromArgb(255, 255, 255);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }


                for (int i = Each_Height * 2; i < Each_Height * 3; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(255, 255, 255);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Cross_Talk.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Cross_Talk.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Cross_Talk.bmp NG", Color.Red);
            }
        }

        public void Gray0_to_Gray7(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);
                int Height = img.Height;
                int Width = img.Width;
                int per_height = Height / 8;

                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (i < per_height)
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        else if (i < per_height * 2)
                        {
                            myRgbColor = Color.FromArgb(1, 1, 1);
                        }
                        else if (i < per_height * 3)
                        {
                            myRgbColor = Color.FromArgb(2, 2, 2);
                        }
                        else if (i < per_height * 4)
                        {
                            myRgbColor = Color.FromArgb(3, 3, 3);
                        }
                        else if (i < per_height * 5)
                        {
                            myRgbColor = Color.FromArgb(4, 4, 4);
                        }
                        else if (i < per_height * 6)
                        {
                            myRgbColor = Color.FromArgb(5, 5, 5);
                        }
                        else if (i < per_height * 7)
                        {
                            myRgbColor = Color.FromArgb(6, 6, 6);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(7, 7, 7);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Gray0_to_Gray7.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Gray0_to_Gray7.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Gray0_to_Gray7.bmp NG", Color.Red);
            }
        }

        public void Show_Mura_Detect_Pattern(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);
                int Height = img.Height;
                int Width = img.Width;

                int Each_Height = img.Height / 3;
                if ((Each_Height * 3) != img.Height)
                {
                    System.Windows.Forms.MessageBox.Show("Y should be multiple of 3");
                    throw new Exception();
                }

                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (i < Height * (1 / 3.0))
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        else if (i < Height * (2 / 3.0))
                        {
                            myRgbColor = Color.FromArgb(16, 16, 16);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Show_Mura_Detect_Pattern_1.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Show_Mura_Detect_Pattern_1.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();

                Each_Height = img.Height / 2;
                if ((Each_Height * 2) != img.Height)
                {
                    System.Windows.Forms.MessageBox.Show("Y should be multiple of 2");
                    throw new Exception();
                }

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (i < Height * (1 / 2.0))
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(16, 16, 16);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Show_Mura_Detect_Pattern_2.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Show_Mura_Detect_Pattern_2.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Show_Mura_Detect_Pattern_1/2.bmp NG", Color.Red);
            }
        }

        public void Pattern_40_Percent(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Height = img.Height;
                int Width = img.Width;

                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (i <= (Height * 0.3))
                        {
                            myRgbColor = Color.FromArgb(255, 0, 0);
                        }
                        else if (i <= (Height * 0.4))
                        {
                            myRgbColor = Color.FromArgb(255, 255, 0);
                        }
                        else if (i <= (Height * 0.6))
                        {
                            myRgbColor = Color.FromArgb(0, 255, 0);
                        }
                        else if (i <= (Height * 0.7))
                        {
                            myRgbColor = Color.FromArgb(0, 255, 255);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(0, 0, 255);
                        }
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\Pattern_40_Percent.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Pattern_40_Percent.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making Pattern_40_Percent.bmp NG", Color.Red);
            }
        }

        public void V_LbyL_Magenta_Green_Gradation(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Each_Height = img.Height / 3;
                if ((Each_Height * 3) != img.Height)
                {
                    System.Windows.Forms.MessageBox.Show("Y should be multiple of 3");
                    throw new Exception();
                }

                int Width = img.Width;
                int Height = img.Height;
                Color myRgbColor;
                double r = 255.0 / Each_Height;
                int Gray = 0;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Each_Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (j % 2 == 0)
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                            img.SetPixel(j, i, myRgbColor);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(255, 255, 255);
                            img.SetPixel(j, i, myRgbColor);
                        }
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }

                for (int i = Each_Height; i < Each_Height * 2; i++)
                {
                    Gray = Convert.ToInt16((i - Each_Height) * r);
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(Gray, 0, Gray);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }

                for (int i = Each_Height * 2; i < Each_Height * 3; i++)
                {
                    Gray = Convert.ToInt16((i - (Each_Height * 2)) * r);
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(0, Gray, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\V_LbyL_Magenta_Green_Gradation.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making V_LbyL_Magenta_Green_Gradation.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making V_LbyL_Magenta_Green_Gradation.bmp NG", Color.Red);
            }
        }


        public void V_LbyL_Magenta_Green(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Each_Height = img.Height / 3;
                if ((Each_Height * 3) != img.Height)
                {
                    System.Windows.Forms.MessageBox.Show("Y should be multiple of 3");
                    throw new Exception();
                }

                int Width = img.Width;
                int Height = img.Height;
                Color myRgbColor;
                double r = 255.0 / Each_Height;


                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Each_Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (j % 2 == 0)
                        {
                            myRgbColor = Color.FromArgb(0, 0, 0);
                            img.SetPixel(j, i, myRgbColor);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(255, 255, 255);
                            img.SetPixel(j, i, myRgbColor);
                        }
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }


                for (int i = Each_Height; i < Each_Height * 2; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(255, 0, 255);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }



                for (int i = Each_Height * 2; i < Each_Height * 3; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        myRgbColor = Color.FromArgb(0, 255, 0);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\V_LbyL_Magenta_Green.bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making V_LbyL_Magenta_Green.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making V_LbyL_Magenta_Green.bmp NG", Color.Red);
            }
        }


        public void G63_Border(int Resolution_X, int Resolution_Y, int Top_Bottom_Lines, int Left_Right_Lines, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            string BMP_Name = "G63_Border_TB_Lines_" + Top_Bottom_Lines.ToString() + "_LR_Lines_" + Left_Right_Lines.ToString();

            try
            {
                //G63_One_Border (DP116 1176x2400)
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);

                int Height = img.Height;
                int Width = img.Width;
                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (i < Top_Bottom_Lines || j < Left_Right_Lines || i >= (Height - Top_Bottom_Lines) || j >= (Width - Left_Right_Lines))
                        {
                            myRgbColor = Color.FromArgb(255, 255, 255);
                            img.SetPixel(j, i, myRgbColor);
                        }
                        else
                        {
                            myRgbColor = Color.FromArgb(63, 63, 63);
                            img.SetPixel(j, i, myRgbColor);
                        }
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\" + BMP_Name + ".bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making " + BMP_Name + ".bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making " + BMP_Name + ".bmp NG", Color.Red);
            }
        }



        public void Pseudo(int Resolution_X, int Resolution_Y, int WRGB_Gray, int Background_Gray, bool Save_Image = true)
        {
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            string BMP_Name = "Pseudo_WRGB_Gray" + WRGB_Gray.ToString() + "_Background_Gray" + Background_Gray.ToString();

            try
            {
                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);
                int Height = img.Height;
                int Each_Height = (img.Height / 2);

                int Width = img.Width;
                int Each_Width = (img.Width / 5);

                Color White = Color.FromArgb(WRGB_Gray, WRGB_Gray, WRGB_Gray);
                Color Red = Color.FromArgb(WRGB_Gray, 0, 0);
                Color Green = Color.FromArgb(0, WRGB_Gray, 0);
                Color Blue = Color.FromArgb(0, 0, WRGB_Gray);
                Color Back_RgbColor = Color.FromArgb(Background_Gray, Background_Gray, Background_Gray);

                bmp_image_processing_form.progressBar1.Maximum = Height;

                for (int i = 0; i < Height; i++)
                {
                    if (i == Each_Height)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            if (j == Each_Width) img.SetPixel(j, i, White);
                            else if (j == (Each_Width * 2)) img.SetPixel(j, i, Red);
                            else if (j == (Each_Width * 3)) img.SetPixel(j, i, Green);
                            else if (j == (Each_Width * 4)) img.SetPixel(j, i, Blue);
                            else img.SetPixel(j, i, Back_RgbColor);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < Width; j++) img.SetPixel(j, i, Back_RgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;
                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\" + BMP_Name + ".bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making " + BMP_Name + ".bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch
            {
                if (Save_Image) bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making " + BMP_Name + ".bmp NG", Color.Red);
            }
        }

        public void MyGradation(int Resolution_X, int Resolution_Y, bool Save_Image = true)
        {
            
            BMP_Image_Processing_Form bmp_image_processing_form = (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
            
            try
            {
                int StartGray = Convert.ToInt32(bmp_image_processing_form.textBox_MyGradation_Start_Gray.Text);
                int EndGray = Convert.ToInt32(bmp_image_processing_form.textBox_MyGradation_End_Gray.Text);
                if (StartGray > EndGray)
                    throw new Exception("StartGray should be lesser than EndGray");

                Bitmap img = new Bitmap(Resolution_X, Resolution_Y);
                int Height = img.Height;
                int Width = img.Width;
                double IncrementGray =  (EndGray - StartGray) / (double)Height;

                Color myRgbColor;

                bmp_image_processing_form.progressBar1.Maximum = Height;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        int gray =  StartGray + Convert.ToInt32(IncrementGray * i);
                        myRgbColor = Color.FromArgb(gray, gray, gray);
                        img.SetPixel(j, i, myRgbColor);
                    }
                    bmp_image_processing_form.progressBar1.Value = i;
                }
                bmp_image_processing_form.progressBar1.Value = 0;

                bmp_image_processing_form.pictureBox_To_be_created_BMP.Image = img;
                if (Save_Image)
                {
                    bmp_image_processing_form.pictureBox_To_be_created_BMP.Image.Save(System.IO.Directory.GetCurrentDirectory() + "\\Produced_BMPs" + "\\MyGradation_from_G" + StartGray.ToString() + "_to_G" + EndGray.ToString() + ".bmp");
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making MyGradation.bmp OK", Color.Green);
                }
                System.Windows.Forms.Application.DoEvents();
            }
            catch(Exception ex)
            {
                bmp_image_processing_form.BMP_Status_AppendText_Nextline(ex.Message, Color.Red);

                if (Save_Image) 
                    bmp_image_processing_form.BMP_Status_AppendText_Nextline("Making MyGradation.bmp NG", Color.Red);
            }
        }
    }
}
