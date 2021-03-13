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
using System.Runtime.InteropServices;
using System.Windows.Media.Converters;
using PNC_Csharp.Practices;

namespace PNC_Csharp
{
    public enum Morphological_Operation
    {
        Dilation,
        Erosion,
    }


    class ImageProcessing
    {
        Form1 f1() { return (Form1)Application.OpenForms["Form1"]; }

        BMP_Image_Processing_Form bmp_form()
        {
            return (BMP_Image_Processing_Form)Application.OpenForms["BMP_Image_Processing_Form"];
        }




        public void Dilation_or_Erosion_With_Square_Kernel(Bitmap img, int Length, Morphological_Operation operation)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];


            int offset = Length / 2; //중심을 기준으로 처리함

            //Make New Image
            int byteOffset = 0;
            bmp_form().progressBar1.Maximum = fastBitmap_img.Height - (2 * offset);
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;

            for (int y = offset; y < fastBitmap_img.Height - offset; y++)
            {
                for (int x = offset; x < fastBitmap_img.Width - offset; x++)
                {
                    double Min_or_Max_R = 0, Min_or_Max_G = 0, Min_or_Max_B = 0;
                    byteOffset = y * fastBitmap_img.Stride + x * 4;

                    Min_or_Max_R = ImgRGB[byteOffset + 2];
                    Min_or_Max_G = ImgRGB[byteOffset + 1];
                    Min_or_Max_B = ImgRGB[byteOffset + 0];

                    for (int row = y - offset; row <= y + offset; row++)
                    {
                        for (int col = x - offset; col <= x + offset; col++)
                        {
                            if (row == y && col == x)
                                continue;

                            byteOffset = row * fastBitmap_img.Stride + col * 4;
                            if (operation == Morphological_Operation.Dilation)
                            {
                                Min_or_Max_R = Math.Max(Min_or_Max_R, ImgRGB[byteOffset + 2]);
                                Min_or_Max_G = Math.Max(Min_or_Max_G, ImgRGB[byteOffset + 1]);
                                Min_or_Max_B = Math.Max(Min_or_Max_B, ImgRGB[byteOffset + 0]);
                            }
                            else if (operation == Morphological_Operation.Erosion)
                            {
                                Min_or_Max_R = Math.Min(Min_or_Max_R, ImgRGB[byteOffset + 2]);
                                Min_or_Max_G = Math.Min(Min_or_Max_G, ImgRGB[byteOffset + 1]);
                                Min_or_Max_B = Math.Min(Min_or_Max_B, ImgRGB[byteOffset + 0]);
                            }
                        }
                    }

                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(Min_or_Max_R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(Min_or_Max_G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(Min_or_Max_B);
                }
                bmp_form().progressBar1.PerformStep();
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void Dilation_or_Erosion_With_Circle_Kernel(Bitmap img, int Length, Morphological_Operation operation)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];


            int offset = Length / 2; //중심을 기준으로 처리함

            //Make New Image
            int byteOffset = 0;
            bmp_form().progressBar1.Maximum = fastBitmap_img.Height - (2 * offset);
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;

            for (int y = offset; y < fastBitmap_img.Height - offset; y++)
            {
                for (int x = offset; x < fastBitmap_img.Width - offset; x++)
                {
                    double Min_or_Max_R = 0, Min_or_Max_G = 0, Min_or_Max_B = 0;
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    Min_or_Max_R = ImgRGB[byteOffset + 2];
                    Min_or_Max_G = ImgRGB[byteOffset + 1];
                    Min_or_Max_B = ImgRGB[byteOffset + 0];

                    for (int row = y - offset; row <= y + offset; row++)
                    {
                        for (int col = x - offset; col <= x + offset; col++)
                        {
                            if (row == y && col == x)
                                continue;

                            double Distance = Math.Sqrt(Math.Pow((row - y), 2) + Math.Pow((col - x), 2));
                            if (Distance <= offset)
                            {
                                byteOffset = row * fastBitmap_img.Stride + col * 4;
                                if (operation == Morphological_Operation.Dilation)
                                {
                                    Min_or_Max_R = Math.Max(Min_or_Max_R, ImgRGB[byteOffset + 2]);
                                    Min_or_Max_G = Math.Max(Min_or_Max_G, ImgRGB[byteOffset + 1]);
                                    Min_or_Max_B = Math.Max(Min_or_Max_B, ImgRGB[byteOffset + 0]);
                                }
                                else if (operation == Morphological_Operation.Erosion)
                                {
                                    Min_or_Max_R = Math.Min(Min_or_Max_R, ImgRGB[byteOffset + 2]);
                                    Min_or_Max_G = Math.Min(Min_or_Max_G, ImgRGB[byteOffset + 1]);
                                    Min_or_Max_B = Math.Min(Min_or_Max_B, ImgRGB[byteOffset + 0]);
                                }
                            }
                        }
                    }

                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(Min_or_Max_R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(Min_or_Max_G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(Min_or_Max_B);
                }
                bmp_form().progressBar1.PerformStep();
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }













        public void ImageExtractionBankGroudBlack(byte StartGray_R, byte EndGray_R, byte StartGray_G, byte EndGray_G, byte StartGray_B, byte EndGray_B, Bitmap img)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];


            //Make New Image
            int byteOffset = 0;
            double R = 0, G = 0, B = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;

            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    R = ImgRGB[byteOffset + 2];
                    G = ImgRGB[byteOffset + 1];
                    B = ImgRGB[byteOffset + 0];

                    if (!((R >= StartGray_R && R <= EndGray_R) && (G >= StartGray_G && G <= EndGray_G) && (B >= StartGray_B && B <= EndGray_B)))
                    {
                        R = 0;
                        G = 0;
                        B = 0;
                    }

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }


        public void ImageExtractionForeGroudWhite(byte StartGray_R, byte EndGray_R, byte StartGray_G, byte EndGray_G, byte StartGray_B, byte EndGray_B, Bitmap img)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];


            //Make New Image
            int byteOffset = 0;
            double R = 0, G = 0, B = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;

            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    R = ImgRGB[byteOffset + 2];
                    G = ImgRGB[byteOffset + 1];
                    B = ImgRGB[byteOffset + 0];

                    if ((R >= StartGray_R && R <= EndGray_R) && (G >= StartGray_G && G <= EndGray_G) && (B >= StartGray_B && B <= EndGray_B))
                    {
                        R = 255;
                        G = 255;
                        B = 255;
                    }

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void HighBoost(Bitmap Original_img, Bitmap AverageImg, double SharpWeight)
        {
            //Make New Image
            FastBitmap fastBitmap_img_origin = new FastBitmap(Original_img);
            byte[] Origin_Img_RGB = new byte[fastBitmap_img_origin.Stride * fastBitmap_img_origin.Height];
            Marshal.Copy(fastBitmap_img_origin.Scan0, Origin_Img_RGB, 0, Origin_Img_RGB.Length);

            FastBitmap fastBitmap_img_Average = new FastBitmap(AverageImg);
            byte[] Average_Img_RGB = new byte[fastBitmap_img_Average.Stride * fastBitmap_img_Average.Height];
            Marshal.Copy(fastBitmap_img_Average.Scan0, Average_Img_RGB, 0, Average_Img_RGB.Length);
            fastBitmap_img_Average.Dispose();

            byte[] ResultRGB = new byte[fastBitmap_img_origin.Stride * fastBitmap_img_origin.Height];

            double R = 0, G = 0, B = 0;
            int byteOffset = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img_origin.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img_origin.Width; x++)
                {
                    byteOffset = y * fastBitmap_img_origin.Stride + x * 4;
                    R = Origin_Img_RGB[byteOffset + 2] + SharpWeight * (Origin_Img_RGB[byteOffset + 2] - Average_Img_RGB[byteOffset + 2]);
                    G = Origin_Img_RGB[byteOffset + 1] + SharpWeight * (Origin_Img_RGB[byteOffset + 1] - Average_Img_RGB[byteOffset + 1]);
                    B = Origin_Img_RGB[byteOffset + 0] + SharpWeight * (Origin_Img_RGB[byteOffset + 0] - Average_Img_RGB[byteOffset + 0]);

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(Math.Max(Math.Min(R, 255), 0));
                    ResultRGB[byteOffset + 1] = Convert.ToByte(Math.Max(Math.Min(G, 255), 0));
                    ResultRGB[byteOffset + 0] = Convert.ToByte(Math.Max(Math.Min(B, 255), 0));
                }
                bmp_form().progressBar1.PerformStep();
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img_origin.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img_origin.Bitmap);
            fastBitmap_img_origin.Dispose();

        }


        public void TransPalencyChange(Bitmap img, byte Alpha)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            //Make New Image
            int byteOffset = 0;
            double R = 0, G = 0, B = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;

            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;

                    ResultRGB[byteOffset + 3] = Alpha;//투명도
                    ResultRGB[byteOffset + 2] = ImgRGB[byteOffset + 2];
                    ResultRGB[byteOffset + 1] = ImgRGB[byteOffset + 1];
                    ResultRGB[byteOffset + 0] = ImgRGB[byteOffset + 0];
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();

        }
        public void Filter_With_Weight(Bitmap img, double[][] Weight)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);

            if (Weight.Length != Weight[0].Length)
                throw new Exception("Weight's row and col amount should be same");
            if (Weight.Length % 2 == 0)
                throw new Exception("Weight's Length should be the odd length");

            int W_Offset = (Weight.Length - 1) / 2;
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;

            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)//Padding Process is needed
                {
                    int byteOffset = 0;
                    double R = 0;
                    double G = 0;
                    double B = 0;

                    int row = 0;
                    for (int i = y - W_Offset; i <= y + W_Offset; i++)
                    {
                        int col = 0;
                        for (int j = x - W_Offset; j <= x + W_Offset; j++)
                        {
                            int tempY = i;
                            int tempX = j;

                            if (i < 0) tempY = 0;
                            else if (i >= fastBitmap_img.Height) tempY = fastBitmap_img.Height - 1;
                            if (j < 0) tempX = 0;
                            else if (j >= fastBitmap_img.Width) tempX = fastBitmap_img.Width - 1;

                            byteOffset = tempY * fastBitmap_img.Stride + tempX * 4;
                            R += ImgRGB[byteOffset + 2] * Weight[row][col];
                            G += ImgRGB[byteOffset + 1] * Weight[row][col];
                            B += ImgRGB[byteOffset + 0] * Weight[row][col];

                            col++;
                        }
                        row++;
                    }

                    double WeightSum = Get_Weight_Sum(Weight);
                    if (WeightSum != 0)
                    {
                        R /= WeightSum;
                        G /= WeightSum;
                        B /= WeightSum;
                    }

                    if (R < 0) R = 0;
                    if (R > 255) R = 255;

                    if (G < 0) G = 0;
                    if (G > 255) G = 255;

                    if (B < 0) B = 0;
                    if (B > 255) B = 255;

                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;

            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void Filter_With_Weight(Bitmap img, double[][] Weight1, double[][] Weight2)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);

            if (Weight1.Length != Weight1[0].Length || Weight2.Length != Weight2[0].Length || Weight1.Length != Weight2.Length)
                throw new Exception("Weight's row and col amount should be same");
            if (Weight1.Length % 2 == 0)
                throw new Exception("Weight's Length should be the odd length");

            int W_Offset = (Weight1.Length - 1) / 2;
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;

            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)//Padding Process is needed
                {
                    int byteOffset = 0;
                    double[] R = new double[2] { 0, 0 };
                    double[] G = new double[2] { 0, 0 };
                    double[] B = new double[2] { 0, 0 };

                    int row = 0;
                    for (int i = y - W_Offset; i <= y + W_Offset; i++)
                    {
                        int col = 0;
                        for (int j = x - W_Offset; j <= x + W_Offset; j++)
                        {
                            int tempY = i;
                            int tempX = j;

                            if (i < 0) tempY = 0;
                            else if (i >= fastBitmap_img.Height) tempY = fastBitmap_img.Height - 1;
                            if (j < 0) tempX = 0;
                            else if (j >= fastBitmap_img.Width) tempX = fastBitmap_img.Width - 1;

                            byteOffset = tempY * fastBitmap_img.Stride + tempX * 4;
                            R[0] += ImgRGB[byteOffset + 2] * Weight1[row][col];
                            G[0] += ImgRGB[byteOffset + 1] * Weight1[row][col];
                            B[0] += ImgRGB[byteOffset + 0] * Weight1[row][col];

                            R[1] += ImgRGB[byteOffset + 2] * Weight2[row][col];
                            G[1] += ImgRGB[byteOffset + 1] * Weight2[row][col];
                            B[1] += ImgRGB[byteOffset + 0] * Weight2[row][col];

                            col++;
                        }
                        row++;
                    }

                    double[] WeightSum = new double[2] { Get_Weight_Sum(Weight1), Get_Weight_Sum(Weight2) };
                    for (int i = 0; i < WeightSum.Length; i++)
                    {
                        if (WeightSum[i] != 0)
                        {
                            R[i] /= WeightSum[i];
                            G[i] /= WeightSum[i];
                            B[i] /= WeightSum[i];
                        }
                    }

                    double red = Math.Sqrt((Math.Pow(R[0], 2) + Math.Pow(R[1], 2)) / 2);
                    double green = Math.Sqrt((Math.Pow(G[0], 2) + Math.Pow(G[1], 2)) / 2);
                    double blue = Math.Sqrt((Math.Pow(B[0], 2) + Math.Pow(B[1], 2)) / 2);



                    if (red < 0) red = 0;
                    if (red > 255) red = 255;

                    if (green < 0) green = 0;
                    if (green > 255) green = 255;

                    if (blue < 0) blue = 0;
                    if (blue > 255) blue = 255;

                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(red);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(green);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(blue);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;

            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void Filter_With_Weight(Bitmap img, double[][] Weight1, double[][] Weight2, double[][] Weight3)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);

            if (Weight1.Length != Weight1[0].Length || Weight2.Length != Weight2[0].Length
                || Weight1.Length != Weight2.Length || Weight1.Length != Weight3.Length)
                throw new Exception("Weight's row and col amount should be same");
            if (Weight1.Length % 2 == 0)
                throw new Exception("Weight's Length should be the odd length");

            int W_Offset = (Weight1.Length - 1) / 2;
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;

            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)//Padding Process is needed
                {
                    int byteOffset = 0;
                    double[] R = new double[3] { 0, 0, 0 };
                    double[] G = new double[3] { 0, 0, 0 };
                    double[] B = new double[3] { 0, 0, 0 };

                    int row = 0;
                    for (int i = y - W_Offset; i <= y + W_Offset; i++)
                    {
                        int col = 0;
                        for (int j = x - W_Offset; j <= x + W_Offset; j++)
                        {
                            int tempY = i;
                            int tempX = j;

                            if (i < 0) tempY = 0;
                            else if (i >= fastBitmap_img.Height) tempY = fastBitmap_img.Height - 1;
                            if (j < 0) tempX = 0;
                            else if (j >= fastBitmap_img.Width) tempX = fastBitmap_img.Width - 1;

                            byteOffset = tempY * fastBitmap_img.Stride + tempX * 4;
                            R[0] += ImgRGB[byteOffset + 2] * Weight1[row][col];
                            G[0] += ImgRGB[byteOffset + 1] * Weight1[row][col];
                            B[0] += ImgRGB[byteOffset + 0] * Weight1[row][col];

                            R[1] += ImgRGB[byteOffset + 2] * Weight2[row][col];
                            G[1] += ImgRGB[byteOffset + 1] * Weight2[row][col];
                            B[1] += ImgRGB[byteOffset + 0] * Weight2[row][col];

                            R[2] += ImgRGB[byteOffset + 2] * Weight3[row][col];
                            G[2] += ImgRGB[byteOffset + 1] * Weight3[row][col];
                            B[2] += ImgRGB[byteOffset + 0] * Weight3[row][col];

                            col++;
                        }
                        row++;
                    }

                    double[] WeightSum = new double[3] { Get_Weight_Sum(Weight1), Get_Weight_Sum(Weight2), Get_Weight_Sum(Weight3) };
                    for (int i = 0; i < WeightSum.Length; i++)
                    {
                        if (WeightSum[i] != 0)
                        {
                            R[i] /= WeightSum[i];
                            G[i] /= WeightSum[i];
                            B[i] /= WeightSum[i];
                        }
                    }

                    double red = Math.Sqrt((Math.Pow(R[0], 2) + Math.Pow(R[1], 2) + Math.Pow(R[2], 2)) / 3);
                    double green = Math.Sqrt((Math.Pow(G[0], 2) + Math.Pow(G[1], 2) + Math.Pow(G[2], 2)) / 3);
                    double blue = Math.Sqrt((Math.Pow(B[0], 2) + Math.Pow(B[1], 2) + Math.Pow(B[2], 2)) / 3);


                    if (red < 0) red = 0;
                    if (red > 255) red = 255;

                    if (green < 0) green = 0;
                    if (green > 255) green = 255;

                    if (blue < 0) blue = 0;
                    if (blue > 255) blue = 255;

                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(red);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(green);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(blue);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;

            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void Filter_With_Weight(Bitmap img, double[][] Weight1, double[][] Weight2, double[][] Weight3, double[][] Weight4)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);

            if (Weight1.Length != Weight1[0].Length || Weight2.Length != Weight2[0].Length
                || Weight1.Length != Weight2.Length || Weight1.Length != Weight3.Length || Weight1.Length != Weight4.Length)
                throw new Exception("Weight's row and col amount should be same");
            if (Weight1.Length % 2 == 0)
                throw new Exception("Weight's Length should be the odd length");

            int W_Offset = (Weight1.Length - 1) / 2;
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;

            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)//Padding Process is needed
                {
                    int byteOffset = 0;
                    double[] R = new double[4] { 0, 0, 0, 0 };
                    double[] G = new double[4] { 0, 0, 0, 0 };
                    double[] B = new double[4] { 0, 0, 0, 0 };

                    int row = 0;
                    for (int i = y - W_Offset; i <= y + W_Offset; i++)
                    {
                        int col = 0;
                        for (int j = x - W_Offset; j <= x + W_Offset; j++)
                        {
                            int tempY = i;
                            int tempX = j;

                            if (i < 0) tempY = 0;
                            else if (i >= fastBitmap_img.Height) tempY = fastBitmap_img.Height - 1;
                            if (j < 0) tempX = 0;
                            else if (j >= fastBitmap_img.Width) tempX = fastBitmap_img.Width - 1;

                            byteOffset = tempY * fastBitmap_img.Stride + tempX * 4;
                            R[0] += ImgRGB[byteOffset + 2] * Weight1[row][col];
                            G[0] += ImgRGB[byteOffset + 1] * Weight1[row][col];
                            B[0] += ImgRGB[byteOffset + 0] * Weight1[row][col];

                            R[1] += ImgRGB[byteOffset + 2] * Weight2[row][col];
                            G[1] += ImgRGB[byteOffset + 1] * Weight2[row][col];
                            B[1] += ImgRGB[byteOffset + 0] * Weight2[row][col];

                            R[2] += ImgRGB[byteOffset + 2] * Weight3[row][col];
                            G[2] += ImgRGB[byteOffset + 1] * Weight3[row][col];
                            B[2] += ImgRGB[byteOffset + 0] * Weight3[row][col];

                            R[3] += ImgRGB[byteOffset + 2] * Weight4[row][col];
                            G[3] += ImgRGB[byteOffset + 1] * Weight4[row][col];
                            B[3] += ImgRGB[byteOffset + 0] * Weight4[row][col];

                            col++;
                        }
                        row++;
                    }

                    double[] WeightSum = new double[4] { Get_Weight_Sum(Weight1), Get_Weight_Sum(Weight2), Get_Weight_Sum(Weight3), Get_Weight_Sum(Weight4) };
                    for (int i = 0; i < WeightSum.Length; i++)
                    {
                        if (WeightSum[i] != 0)
                        {
                            R[i] /= WeightSum[i];
                            G[i] /= WeightSum[i];
                            B[i] /= WeightSum[i];
                        }
                    }

                    double red = Math.Sqrt((Math.Pow(R[0], 2) + Math.Pow(R[1], 2) + Math.Pow(R[2], 2) + Math.Pow(R[3], 2)) / 4);
                    double green = Math.Sqrt((Math.Pow(G[0], 2) + Math.Pow(G[1], 2) + Math.Pow(G[2], 2) + Math.Pow(G[3], 2)) / 4);
                    double blue = Math.Sqrt((Math.Pow(B[0], 2) + Math.Pow(B[1], 2) + Math.Pow(B[2], 2) + Math.Pow(B[3], 2)) / 4);


                    if (red < 0) red = 0;
                    if (red > 255) red = 255;

                    if (green < 0) green = 0;
                    if (green > 255) green = 255;

                    if (blue < 0) blue = 0;
                    if (blue > 255) blue = 255;

                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(red);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(green);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(blue);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;

            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }









        private double Get_Weight_Sum(double[][] Weight)
        {
            double sum = 0;
            for (int i = 0; i < Weight.Length; i++)
            {
                for (int j = 0; j < Weight[0].Length; j++)
                {
                    sum += Weight[i][j];
                }
            }
            return sum;
        }



        public void BitInversion(Bitmap img)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];


            //Make New Image
            int byteOffset = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(255 - ImgRGB[byteOffset + 2]);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(255 - ImgRGB[byteOffset + 1]);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(255 - ImgRGB[byteOffset + 0]);

                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }


        public void histogram_equalization(Bitmap img)
        {
            // update  int[] Histogram_R/G/B = new int[256];
            bmp_form().Get_Histogram_and_GrayArray_btn.PerformClick();
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            //Get New Histogram
            var histR = new float[256];
            var histG = new float[256];
            var histB = new float[256];

            long Sum_R = 0;
            long Sum_G = 0;
            long Sum_B = 0;

            for (int i = 0; i <= 255; i++)
            {
                Sum_R += bmp_form().Histogram_R[i];
                histR[i] = (Sum_R * 255) / (fastBitmap_img.Width * fastBitmap_img.Height);

                Sum_G += bmp_form().Histogram_R[i];
                histG[i] = (Sum_G * 255) / (fastBitmap_img.Width * fastBitmap_img.Height);

                Sum_B += bmp_form().Histogram_R[i];
                histB[i] = (Sum_B * 255) / (fastBitmap_img.Width * fastBitmap_img.Height);
            }

            double R = 0, G = 0, B = 0;
            int byteOffset = 0;
            //Make New Image
            bmp_form().progressBar1.Maximum = img.Height;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    R = histR[ImgRGB[byteOffset + 2]];
                    G = histG[ImgRGB[byteOffset + 1]];
                    B = histB[ImgRGB[byteOffset + 0]];

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }

            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void Median_Filter_5x5(Bitmap img)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    int byteOffset = 0;
                    byte[] R = new byte[25];
                    byte[] G = new byte[25];
                    byte[] B = new byte[25];
                    int count = 0;
                    for (int i = y - 2; i <= y + 2; i++)
                    {
                        for (int j = x - 2; j <= x + 2; j++)
                        {
                            int tempY = i;
                            int tempX = j;

                            if (i < 0) tempY = 0;
                            else if (i >= fastBitmap_img.Height) tempY = fastBitmap_img.Height - 1;

                            if (j < 0) tempX = 0;
                            else if (j >= fastBitmap_img.Width) tempX = fastBitmap_img.Width - 1;

                            byteOffset = tempY * fastBitmap_img.Stride + tempX * 4;
                            R[count] = ImgRGB[byteOffset + 2];
                            G[count] = ImgRGB[byteOffset + 1];
                            B[count] = ImgRGB[byteOffset + 0];
                            count++;
                        }
                    }
                    Array.Sort(R);
                    Array.Sort(G);
                    Array.Sort(B);

                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = R[12];
                    ResultRGB[byteOffset + 1] = G[12];
                    ResultRGB[byteOffset + 0] = B[12];
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }




        public void Median_Filter_3x3(Bitmap img)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    int byteOffset = 0;
                    byte[] R = new byte[9];
                    byte[] G = new byte[9];
                    byte[] B = new byte[9];
                    int count = 0;
                    for (int i = y - 1; i <= y + 1; i++)
                    {
                        for (int j = x - 1; j <= x + 1; j++)
                        {
                            int tempY = i;
                            int tempX = j;

                            if (i < 0) tempY = 0;
                            else if (i >= fastBitmap_img.Height) tempY = fastBitmap_img.Height - 1;

                            if (j < 0) tempX = 0;
                            else if (j >= fastBitmap_img.Width) tempX = fastBitmap_img.Width - 1;

                            byteOffset = tempY * fastBitmap_img.Stride + tempX * 4;
                            R[count] = ImgRGB[byteOffset + 2];
                            G[count] = ImgRGB[byteOffset + 1];
                            B[count] = ImgRGB[byteOffset + 0];
                            count++;
                        }
                    }
                    Array.Sort(R);
                    Array.Sort(G);
                    Array.Sort(B);

                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = R[4];
                    ResultRGB[byteOffset + 1] = G[4];
                    ResultRGB[byteOffset + 0] = B[4];
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }


        public void RGB_to_CYMK(Bitmap img)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);

            byte[] Result_RGB_C = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            byte[] Result_RGB_M = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            byte[] Result_RGB_Y = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            byte[] Result_RGB_K = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            byte[] Result_RGB_CYMK = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            double black = 0, cyan = 0, magenta = 0, yellow = 0;
            int byteOffset = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;

                    black = 1 - Math.Max(ImgRGB[byteOffset + 2] / 255.0, Math.Max(ImgRGB[byteOffset + 1] / 255.0, ImgRGB[byteOffset + 0] / 255.0));

                    if (black != 1.0)
                    {
                        cyan = (1.0 - (ImgRGB[byteOffset + 2] / 255.0) - black) / (1.0 - black);
                        magenta = (1.0 - (ImgRGB[byteOffset + 1] / 255.0) - black) / (1.0 - black);
                        yellow = (1.0 - (ImgRGB[byteOffset + 0] / 255.0) - black) / (1.0 - black);
                    }
                    else
                    {
                        cyan = 0;
                        magenta = 0;
                        yellow = 0;
                    }

                    //C
                    Result_RGB_C[byteOffset + 3] = 255;//투명도
                    Result_RGB_C[byteOffset + 2] = Convert.ToByte(255 * (1 - cyan));
                    Result_RGB_C[byteOffset + 1] = Convert.ToByte(255);
                    Result_RGB_C[byteOffset + 0] = Convert.ToByte(255);

                    //M
                    Result_RGB_M[byteOffset + 3] = 255;//투명도
                    Result_RGB_M[byteOffset + 2] = Convert.ToByte(255);
                    Result_RGB_M[byteOffset + 1] = Convert.ToByte(255 * (1 - magenta));
                    Result_RGB_M[byteOffset + 0] = Convert.ToByte(255);

                    //Y
                    Result_RGB_Y[byteOffset + 3] = 255;//투명도
                    Result_RGB_Y[byteOffset + 2] = Convert.ToByte(255);
                    Result_RGB_Y[byteOffset + 1] = Convert.ToByte(255);
                    Result_RGB_Y[byteOffset + 0] = Convert.ToByte(255 * (1 - yellow));

                    //K
                    Result_RGB_K[byteOffset + 3] = 255;//투명도
                    Result_RGB_K[byteOffset + 2] = Convert.ToByte(255 * (1 - black));
                    Result_RGB_K[byteOffset + 1] = Convert.ToByte(255 * (1 - black));
                    Result_RGB_K[byteOffset + 0] = Convert.ToByte(255 * (1 - black));

                    //CYMK
                    Result_RGB_CYMK[byteOffset + 3] = 255;//투명도
                    Result_RGB_CYMK[byteOffset + 2] = Convert.ToByte(cyan * 255);
                    Result_RGB_CYMK[byteOffset + 1] = Convert.ToByte(magenta * 255);
                    Result_RGB_CYMK[byteOffset + 0] = Convert.ToByte(yellow * 255);
                }

            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;

            FastBitmap fastBitmap_img_C = new FastBitmap(fastBitmap_img.Width, fastBitmap_img.Height);
            Marshal.Copy(Result_RGB_C, 0, fastBitmap_img_C.Scan0, Result_RGB_C.Length);
            bmp_form().pictureBox_Cyan.Image = fastBitmap_img_C.Bitmap;

            FastBitmap fastBitmap_img_M = new FastBitmap(fastBitmap_img.Width, fastBitmap_img.Height);
            Marshal.Copy(Result_RGB_M, 0, fastBitmap_img_M.Scan0, Result_RGB_M.Length);
            bmp_form().pictureBox_Magenta.Image = fastBitmap_img_M.Bitmap;

            FastBitmap fastBitmap_img_Y = new FastBitmap(fastBitmap_img.Width, fastBitmap_img.Height);
            Marshal.Copy(Result_RGB_Y, 0, fastBitmap_img_Y.Scan0, Result_RGB_Y.Length);
            bmp_form().pictureBox_Yellow.Image = fastBitmap_img_Y.Bitmap;

            FastBitmap fastBitmap_img_K = new FastBitmap(fastBitmap_img.Width, fastBitmap_img.Height);
            Marshal.Copy(Result_RGB_K, 0, fastBitmap_img_K.Scan0, Result_RGB_K.Length);
            bmp_form().pictureBox_Black.Image = fastBitmap_img_K.Bitmap;

            Marshal.Copy(Result_RGB_CYMK, 0, fastBitmap_img.Scan0, Result_RGB_CYMK.Length);
            bmp_form().pictureBox_CYM.Image = fastBitmap_img.Bitmap;
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);

            fastBitmap_img_C.Dispose();
            fastBitmap_img_M.Dispose();
            fastBitmap_img_Y.Dispose();
            fastBitmap_img_K.Dispose();
            fastBitmap_img.Dispose();
        }


        public void BiModel_Black_White(Bitmap img, byte gray_threshold)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            int byteOffset = 0;
            double R = 0, G = 0, B = 0, gray = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    R = ImgRGB[byteOffset + 2];
                    G = ImgRGB[byteOffset + 1];
                    B = ImgRGB[byteOffset + 0];

                    gray = 0.299 * R + 0.587 * G + 0.114 * B;

                    if (gray > gray_threshold)
                    {
                        R = 0;
                        G = 0;
                        B = 0;
                    }
                    else
                    {
                        R = 255;
                        G = 255;
                        B = 255;
                    }

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void Dot_Noise_Creation(Bitmap img, Color DotColor, int Dot_Num)
        {
            Random rand = new Random();
            HashSet<string> hs = new HashSet<string>();

            int num = 0;
            while (num < Dot_Num)
            {
                int x = rand.Next(0, img.Width);
                int y = rand.Next(0, img.Height);

                if (hs.Contains(x + " # " + y))
                    continue;

                img.SetPixel(x, y, DotColor);
                hs.Add(x + " # " + y);
                num++;
            }

            bmp_form().Set_pictureBox_Loaded_BMP_Image(img);
        }
        public void BiModel_White_Black(Bitmap img, byte gray_threshold)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            int byteOffset = 0;
            double R = 0, G = 0, B = 0, gray = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    R = ImgRGB[byteOffset + 2];
                    G = ImgRGB[byteOffset + 1];
                    B = ImgRGB[byteOffset + 0];

                    gray = 0.299 * R + 0.587 * G + 0.114 * B;

                    if (gray > gray_threshold)
                    {
                        R = 255;
                        G = 255;
                        B = 255;
                    }
                    else
                    {
                        R = 0;
                        G = 0;
                        B = 0;
                    }

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void RGB_to_CYM(Bitmap img)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);

            byte[] Result_RGB_C = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            byte[] Result_RGB_M = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            byte[] Result_RGB_Y = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            byte[] Result_RGB_K = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            byte[] Result_RGB_CYMK = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            double black = 0, cyan = 0, magenta = 0, yellow = 0;

            int byteOffset = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < fastBitmap_img.Height; y++)
            {
                for (int x = 0; x < fastBitmap_img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;

                    cyan = (1.0 - (ImgRGB[byteOffset + 2] / 255.0) - black) / (1.0 - black);
                    magenta = (1.0 - (ImgRGB[byteOffset + 1] / 255.0) - black) / (1.0 - black);
                    yellow = (1.0 - (ImgRGB[byteOffset + 0] / 255.0) - black) / (1.0 - black);

                    //C
                    Result_RGB_C[byteOffset + 3] = 255;//투명도
                    Result_RGB_C[byteOffset + 2] = Convert.ToByte(255 * (1 - cyan));
                    Result_RGB_C[byteOffset + 1] = Convert.ToByte(255);
                    Result_RGB_C[byteOffset + 0] = Convert.ToByte(255);

                    //M
                    Result_RGB_M[byteOffset + 3] = 255;//투명도
                    Result_RGB_M[byteOffset + 2] = Convert.ToByte(255);
                    Result_RGB_M[byteOffset + 1] = Convert.ToByte(255 * (1 - magenta));
                    Result_RGB_M[byteOffset + 0] = Convert.ToByte(255);

                    //Y
                    Result_RGB_Y[byteOffset + 3] = 255;//투명도
                    Result_RGB_Y[byteOffset + 2] = Convert.ToByte(255);
                    Result_RGB_Y[byteOffset + 1] = Convert.ToByte(255);
                    Result_RGB_Y[byteOffset + 0] = Convert.ToByte(255 * (1 - yellow));

                    //K
                    Result_RGB_K[byteOffset + 3] = 255;//투명도
                    Result_RGB_K[byteOffset + 2] = Convert.ToByte(255 * (1 - black));
                    Result_RGB_K[byteOffset + 1] = Convert.ToByte(255 * (1 - black));
                    Result_RGB_K[byteOffset + 0] = Convert.ToByte(255 * (1 - black));

                    //CYMK
                    Result_RGB_CYMK[byteOffset + 3] = 255;//투명도
                    Result_RGB_CYMK[byteOffset + 2] = Convert.ToByte(cyan * 255);
                    Result_RGB_CYMK[byteOffset + 1] = Convert.ToByte(magenta * 255);
                    Result_RGB_CYMK[byteOffset + 0] = Convert.ToByte(yellow * 255);
                }

            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;

            FastBitmap fastBitmap_img_C = new FastBitmap(fastBitmap_img.Width, fastBitmap_img.Height);
            Marshal.Copy(Result_RGB_C, 0, fastBitmap_img_C.Scan0, Result_RGB_C.Length);
            bmp_form().pictureBox_Cyan.Image = fastBitmap_img_C.Bitmap;

            FastBitmap fastBitmap_img_M = new FastBitmap(fastBitmap_img.Width, fastBitmap_img.Height);
            Marshal.Copy(Result_RGB_M, 0, fastBitmap_img_M.Scan0, Result_RGB_M.Length);
            bmp_form().pictureBox_Magenta.Image = fastBitmap_img_M.Bitmap;

            FastBitmap fastBitmap_img_Y = new FastBitmap(fastBitmap_img.Width, fastBitmap_img.Height);
            Marshal.Copy(Result_RGB_Y, 0, fastBitmap_img_Y.Scan0, Result_RGB_Y.Length);
            bmp_form().pictureBox_Yellow.Image = fastBitmap_img_Y.Bitmap;

            FastBitmap fastBitmap_img_K = new FastBitmap(fastBitmap_img.Width, fastBitmap_img.Height);
            Marshal.Copy(Result_RGB_K, 0, fastBitmap_img_K.Scan0, Result_RGB_K.Length);
            bmp_form().pictureBox_Black.Image = fastBitmap_img_K.Bitmap;

            Marshal.Copy(Result_RGB_CYMK, 0, fastBitmap_img.Scan0, Result_RGB_CYMK.Length);
            bmp_form().pictureBox_CYM.Image = fastBitmap_img.Bitmap;
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);

            fastBitmap_img_C.Dispose();
            fastBitmap_img_M.Dispose();
            fastBitmap_img_Y.Dispose();
            fastBitmap_img_K.Dispose();
            fastBitmap_img.Dispose();
        }

        public void RGB_to_Gray(Bitmap img)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];


            double gray = 0;
            int byteOffset = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    //G(R,G,B) = 0.299*R + 0.587*G + 0.114*B
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    gray = 0.299 * ImgRGB[byteOffset + 2] + 0.587 * ImgRGB[byteOffset + 1] + 0.114 * ImgRGB[byteOffset + 0];

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(gray);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(gray);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(gray);
                }

            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public double Get_PSNR(Bitmap OriginalImg, Bitmap NoiseImg)
        {
            double MSE = Get_MSE(OriginalImg, NoiseImg);
            return 20 * Math.Log10(255) - 10 * Math.Log10(MSE);
        }

        private double Get_MSE(Bitmap OriginalImg, Bitmap NoiseImg)
        {
            double ErrorSum = 0;

            FastBitmap Origin_img = new FastBitmap(OriginalImg);
            byte[] Origin_RGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, Origin_RGB, 0, Origin_RGB.Length);
            Origin_img.Dispose();

            FastBitmap Noise_img = new FastBitmap(NoiseImg);
            byte[] Noise_RGB = new byte[Noise_img.Stride * Noise_img.Height];
            Marshal.Copy(Noise_img.Scan0, Noise_RGB, 0, Noise_RGB.Length);
            Noise_img.Dispose();

            int byteOffset = 0;
            for (int y = 0; y < OriginalImg.Height; y++)
            {
                for (int x = 0; x < OriginalImg.Width; x++)
                {
                    byteOffset = y * Origin_img.Stride + x * 4;

                    ErrorSum += Math.Pow((Origin_RGB[byteOffset + 2] - Noise_RGB[byteOffset + 2]), 2);
                    ErrorSum += Math.Pow((Origin_RGB[byteOffset + 1] - Noise_RGB[byteOffset + 1]), 2);
                    ErrorSum += Math.Pow((Origin_RGB[byteOffset + 0] - Noise_RGB[byteOffset + 0]), 2);
                }
            }
            return (ErrorSum / (3 * OriginalImg.Height * OriginalImg.Width));
        }

        public void Gray_Rosolution_Bit_Change(Bitmap img, int grayResolutionBits)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            int factor = Convert.ToInt32(Math.Pow(2, (8 - grayResolutionBits)));
            double R = 0, G = 0, B = 0;
            int byteOffset = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    R = (ImgRGB[byteOffset + 2] / factor) * factor;
                    G = (ImgRGB[byteOffset + 1] / factor) * factor;
                    B = (ImgRGB[byteOffset + 0] / factor) * factor;

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();

        }


        public void Gamma_Encoding(Bitmap img, double Gamma)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

            double R = 0, G = 0, B = 0;
            int byteOffset = 0;
            int max_gray = 255;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    //encoded = ((original / 255) ^ (1 / gamma)) * 255
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    R = Math.Pow(Convert.ToDouble(ImgRGB[byteOffset + 2]) / max_gray, (Gamma)) * max_gray;
                    G = Math.Pow(Convert.ToDouble(ImgRGB[byteOffset + 1]) / max_gray, (Gamma)) * max_gray;
                    B = Math.Pow(Convert.ToDouble(ImgRGB[byteOffset + 0]) / max_gray, (Gamma)) * max_gray;

                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public void Gamma_Decoding(Bitmap img, double Gamma)
        {
            FastBitmap fastBitmap_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
            Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];


            double R = 0, G = 0, B = 0;
            int byteOffset = 0;
            int max_gray = 255;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;
            //----
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {

                    //original = ((encoded / 255) ^ gamma) * 255
                    byteOffset = y * fastBitmap_img.Stride + x * 4;
                    R = Math.Pow(Convert.ToDouble(ImgRGB[byteOffset + 2]) / max_gray, (1 / Gamma)) * max_gray;
                    G = Math.Pow(Convert.ToDouble(ImgRGB[byteOffset + 1]) / max_gray, (1 / Gamma)) * max_gray;
                    B = Math.Pow(Convert.ToDouble(ImgRGB[byteOffset + 0]) / max_gray, (1 / Gamma)) * max_gray;


                    ResultRGB[byteOffset + 3] = 255;//투명도
                    ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                    ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                    ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                }
            }
            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);
            fastBitmap_img.Dispose();
        }

        public Color Get_Color_Coordinate_XY(Bitmap img, int from_row, int to_row, int from_col, int to_col)
        {
            Bitmap SelectedAreaImg = new Bitmap(to_col - from_col + 1, to_row - from_row + 1);
            Bitmap SelectedAreaAveColor = new Bitmap(to_col - from_col + 1, to_row - from_row + 1);
            Bitmap TempImg = new Bitmap(img);

            double Sum_R = 0;
            double Sum_G = 0;
            double Sum_B = 0;
            int count = 0;
            for (int y = from_row; y < to_row; y++)
            {
                for (int x = from_col; x < to_col; x++)
                {
                    Color myRgbColor = img.GetPixel(x, y);

                    int tempX = x - from_col;
                    int tempY = y - from_row;
                    SelectedAreaImg.SetPixel(tempX, tempY, myRgbColor);
                    TempImg.SetPixel(x, y, Color.FromArgb(255 - myRgbColor.R, 255 - myRgbColor.G, 255 - myRgbColor.B));

                    Sum_R += myRgbColor.R;
                    Sum_G += myRgbColor.G;
                    Sum_B += myRgbColor.B;
                    count++;
                }
            }

            Color Average_Color = Color.FromArgb(Convert.ToInt32(Sum_R / count), Convert.ToInt32(Sum_G / count), Convert.ToInt32(Sum_B / count));
            for (int y = from_row; y < to_row; y++)
            {
                for (int x = from_col; x < to_col; x++)
                {
                    int tempX = x - from_col;
                    int tempY = y - from_row;
                    SelectedAreaAveColor.SetPixel(tempX, tempY, Average_Color);
                }
            }

            double X = 0.431 * Average_Color.R + 0.342 * Average_Color.G + 0.178 * Average_Color.B;
            double Y = 0.222 * Average_Color.R + 0.707 * Average_Color.G + 0.071 * Average_Color.B;
            double Z = 0.020 * Average_Color.R + 0.130 * Average_Color.G + 0.939 * Average_Color.B;
            double Sum = X + Y + Z;

            double coordinate_x = X / Sum;
            double coordinate_y = Y / Sum;
            //double coordinate_z = Z / Sum; (coordinate_x + coordinate_Y = 1 - coordinate_z)

            bmp_form().label_Ave_xy.Text = "Ave(x, y) : (" + Math.Round(coordinate_x, 4) + ", " + Math.Round(coordinate_y, 4) + ")";

            double n = (coordinate_x - 0.3320) / (0.1858 - coordinate_y);
            double CCT = 437 * Math.Pow(n, 3) + 3601 * Math.Pow(n, 2) + 6861 * n + 5517;
            bmp_form().label_Calculated_Color_Temperature.Text = "CCT : " + Math.Round(CCT) + "K";

            bmp_form().pictureBox_CIE_XY_Selected_Area.Image = SelectedAreaImg;
            bmp_form().pictureBox_CIE_XY_Selected_Area_Ave_Color.Image = SelectedAreaAveColor;

            bmp_form().Set_pictureBox_Loaded_BMP_Image(TempImg);
            Application.DoEvents();
            Thread.Sleep(1000);
            bmp_form().Set_pictureBox_Loaded_BMP_Image(img);

            return Average_Color;
        }



        public void Clamp_Max_Gray(Bitmap img)
        {

            if (Convert.ToInt32(bmp_form().textBox_Clamping_Max_Gray_R.Text) <= 0)
                System.Windows.Forms.MessageBox.Show("Max Gray should be bigger than 0");

            else
            {
                byte max_gray_R = Convert.ToByte(bmp_form().textBox_Clamping_Max_Gray_R.Text);
                byte max_gray_G = Convert.ToByte(bmp_form().textBox_Clamping_Max_Gray_G.Text);
                byte max_gray_B = Convert.ToByte(bmp_form().textBox_Clamping_Max_Gray_B.Text);
                if (max_gray_R > 255)
                {
                    max_gray_R = 255;
                    bmp_form().textBox_Clamping_Max_Gray_R.Text = max_gray_R.ToString();
                }

                if (max_gray_G > 255)
                {
                    max_gray_G = 255;
                    bmp_form().textBox_Clamping_Max_Gray_G.Text = max_gray_G.ToString();
                }

                if (max_gray_B > 255)
                {
                    max_gray_B = 255;
                    bmp_form().textBox_Clamping_Max_Gray_B.Text = max_gray_B.ToString();
                }




                FastBitmap fastBitmap_img = new FastBitmap(img);
                byte[] ImgRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];
                Marshal.Copy(fastBitmap_img.Scan0, ImgRGB, 0, ImgRGB.Length);
                byte[] ResultRGB = new byte[fastBitmap_img.Stride * fastBitmap_img.Height];

                int byteOffset = 0;
                double R = 0, G = 0, B = 0;
                bmp_form().progressBar1.Maximum = 10;
                bmp_form().progressBar1.Value = 0;
                bmp_form().progressBar1.Step = 1;
                //----
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {

                        byteOffset = y * fastBitmap_img.Stride + x * 4;

                        if (ImgRGB[byteOffset + 2] > max_gray_R) R = max_gray_R;
                        else R = ImgRGB[byteOffset + 2];
                        if (ImgRGB[byteOffset + 1] > max_gray_G) G = max_gray_G;
                        else G = ImgRGB[byteOffset + 1];
                        if (ImgRGB[byteOffset + 0] > max_gray_B) B = max_gray_B;
                        else B = ImgRGB[byteOffset + 0];

                        ResultRGB[byteOffset + 3] = 255;//투명도
                        ResultRGB[byteOffset + 2] = Convert.ToByte(R);
                        ResultRGB[byteOffset + 1] = Convert.ToByte(G);
                        ResultRGB[byteOffset + 0] = Convert.ToByte(B);
                    }
                }
                bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
                Marshal.Copy(ResultRGB, 0, fastBitmap_img.Scan0, ResultRGB.Length);
                bmp_form().Set_pictureBox_Loaded_BMP_Image(fastBitmap_img.Bitmap);

                fastBitmap_img.Dispose();
            }

        }


        public Bitmap Resize_Nearest_Interpolation_Without_Change_Specific_Area(Bitmap img, int Resized_Width, int Resized_Height, int from_col, int to_col, int from_row, int to_row)
        {
            Bitmap horizontal_processed_img = Resize_Nearest_Interpolation_Without_Changea_Horizontal_Specific_Area(img, Resized_Width, from_col, to_col);
            return Resize_Nearest_Interpolation_Without_Changea_Vertial_Specific_Area(horizontal_processed_img, Resized_Height, from_row, to_row);
        }

        private Bitmap Resize_Nearest_Interpolation_Without_Changea_Horizontal_Specific_Area(Bitmap img, int Resized_Width, int from_col, int to_col)
        {
            //Origin Image
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            //Horizontal Changed
            int Original_Left_Width = from_col;
            int Middle_Width = (to_col - from_col + 1);
            int Original_Right_Width = Origin_img.Width - Original_Left_Width - Middle_Width;
            double Ratio = (Resized_Width - Middle_Width) / Convert.ToDouble(Original_Left_Width + Original_Right_Width);

            int Resized_Left_Width = (int)(Original_Left_Width * Ratio);
            int Resized_Right_Width = (int)(Original_Right_Width * Ratio);

            while (Resized_Left_Width + Middle_Width + Resized_Right_Width != Resized_Width)
                Resized_Left_Width++;

            //Left + Middle + Right
            FastBitmap Resized_img = new FastBitmap(Resized_Width, Origin_img.Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);



            //Left Right
            for (int y = 0; y < Origin_img.Height; y++)//Original Height
            {
                for (int x = 0; x < Resized_Left_Width + Middle_Width + Resized_Right_Width; x++)
                {
                    int Origin_Nearest_x = 0;
                    if (x < Resized_Left_Width)
                    {
                        Origin_Nearest_x = Convert.ToInt32((Convert.ToDouble(x) / Resized_Left_Width) * Original_Left_Width);

                    }
                    else if (x < Resized_Left_Width + Middle_Width)
                    {
                        Origin_Nearest_x = (x - Resized_Left_Width) + Original_Left_Width;

                    }
                    else //if (x < Resized_Left_Width + middle + Resized_Right_Width)
                    {
                        Origin_Nearest_x = Convert.ToInt32((Convert.ToDouble(x - (Resized_Left_Width + Middle_Width)) / Resized_Right_Width) * Original_Right_Width);
                        Origin_Nearest_x += (Original_Left_Width + Middle_Width);
                    }

                    if (Origin_Nearest_x > Origin_img.Width - 1) Origin_Nearest_x = Origin_img.Width - 1;

                    int Origin_byteOffset = y * Origin_img.Stride + Origin_Nearest_x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }

        private Bitmap Resize_Nearest_Interpolation_Without_Changea_Vertial_Specific_Area(Bitmap img, int Resized_Height, int from_row, int to_row)
        {
            //Origin Image
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            //Horizontal Changed
            int Original_Top_Height = from_row;
            int Middle_Height = (to_row - from_row + 1);
            int Original_Bottom_Height = Origin_img.Height - Original_Top_Height - Middle_Height;

            double Ratio = (Resized_Height - Middle_Height) / Convert.ToDouble(Original_Top_Height + Original_Bottom_Height);

            int Resized_Top_Height = (int)(Original_Top_Height * Ratio);
            int Resized_Bottom_Height = (int)(Original_Bottom_Height * Ratio);

            while (Resized_Top_Height + Middle_Height + Resized_Bottom_Height != Resized_Height)
                Resized_Top_Height++;

            //Left + Middle + Right
            FastBitmap Resized_img = new FastBitmap(Origin_img.Width, Resized_Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);

            //Left Right
            for (int x = 0; x < Origin_img.Width; x++)//Original Height
            {
                for (int y = 0; y < Resized_Top_Height + Middle_Height + Resized_Bottom_Height; y++)
                {
                    int Origin_Nearest_y = 0;
                    if (y < Resized_Top_Height)
                    {
                        Origin_Nearest_y = Convert.ToInt32((Convert.ToDouble(y) / Resized_Top_Height) * Original_Top_Height);

                    }
                    else if (y < Resized_Top_Height + Middle_Height)
                    {
                        Origin_Nearest_y = (y - Resized_Top_Height) + Original_Top_Height;

                    }
                    else //if (x < Resized_Left_Width + middle + Resized_Right_Width)
                    {
                        Origin_Nearest_y = Convert.ToInt32((Convert.ToDouble(y - (Resized_Top_Height + Middle_Height)) / Resized_Bottom_Height) * Original_Bottom_Height);
                        Origin_Nearest_y += (Original_Top_Height + Middle_Height);
                    }

                    if (Origin_Nearest_y > Origin_img.Height - 1) Origin_Nearest_y = Origin_img.Height - 1;

                    int Origin_byteOffset = Origin_Nearest_y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }





        public Bitmap Resize_Bilinear_Interpolation_Without_Change_Specific_Area(Bitmap img, int Resized_Width, int Resized_Height, int from_col, int to_col, int from_row, int to_row)
        {
            Bitmap horizontal_processed_img = Resize_Bilinear_Interpolation_Without_Change_Horizontal_Specific_Area(img, Resized_Width, from_col, to_col);
            return Resize_Bilinear_Interpolation_Without_Changea_Vertial_Specific_Area(horizontal_processed_img, Resized_Height, from_row, to_row);
        }

        Bitmap Resize_Bilinear_Interpolation_Without_Change_Horizontal_Specific_Area(Bitmap img, int Resized_Width, int from_col, int to_col)
        {
            //Origin Image
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            //Horizontal Changed
            int Original_Left_Width = from_col;
            int middle = (to_col - from_col + 1);
            int Original_Right_Width = Origin_img.Width - Original_Left_Width - middle;
            double Ratio = (Resized_Width - middle) / Convert.ToDouble(Original_Left_Width + Original_Right_Width);

            int Resized_Left_Width = (int)(Original_Left_Width * Ratio);
            int Resized_Right_Width = (int)(Original_Right_Width * Ratio);

            while (Resized_Left_Width + middle + Resized_Right_Width != Resized_Width)
                Resized_Left_Width++;

            //Left + Middle + Right
            FastBitmap Resized_img = new FastBitmap(Resized_Width, Origin_img.Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);


            //Left Right
            for (int y = 0; y < Origin_img.Height; y++)//Original Height
            {
                for (int x = 0; x < Resized_Left_Width + middle + Resized_Right_Width; x++)
                {
                    if (x < Resized_Left_Width)
                    {
                        double TempX = (Convert.ToDouble(x) / Resized_Left_Width) * Original_Left_Width;
                        double TempY = (double)y;
                        Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);

                    }
                    else if (x < Resized_Left_Width + middle)
                    {
                        int Origin_Nearest_x = (x - Resized_Left_Width) + Original_Left_Width;
                        if (Origin_Nearest_x > Origin_img.Width - 1) Origin_Nearest_x = Origin_img.Width - 1;
                        int Origin_byteOffset = y * Origin_img.Stride + Origin_Nearest_x * 4;
                        int Resized_byteOffset = y * Resized_img.Stride + x * 4;
                        ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                        ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                        ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                        ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B

                    }
                    else //if (x < Resized_Left_Width + middle + Resized_Right_Width)
                    {
                        double TempX = (Convert.ToDouble(x - (Resized_Left_Width + middle)) / Resized_Right_Width) * Original_Right_Width;
                        TempX += (Original_Left_Width + middle);
                        double TempY = (double)y;
                        Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);
                    }
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }




        private Bitmap Resize_Bilinear_Interpolation_Without_Changea_Vertial_Specific_Area(Bitmap img, int Resized_Height, int from_row, int to_row)
        {
            //Origin Image
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            //Horizontal Changed
            int Original_Top_Height = from_row;
            int Middle_Height = (to_row - from_row + 1);
            int Original_Bottom_Height = Origin_img.Height - Original_Top_Height - Middle_Height;

            double Ratio = (Resized_Height - Middle_Height) / Convert.ToDouble(Original_Top_Height + Original_Bottom_Height);

            int Resized_Top_Height = (int)(Original_Top_Height * Ratio);
            int Resized_Bottom_Height = (int)(Original_Bottom_Height * Ratio);

            while (Resized_Top_Height + Middle_Height + Resized_Bottom_Height != Resized_Height)
                Resized_Top_Height++;

            //Left + Middle + Right
            FastBitmap Resized_img = new FastBitmap(Origin_img.Width, Resized_Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);

            for (int x = 0; x < Origin_img.Width; x++)//Original Height
            {
                for (int y = 0; y < Resized_Top_Height + Middle_Height + Resized_Bottom_Height; y++)
                {
                    if (y < Resized_Top_Height)
                    {
                        double TempX = (double)x;
                        double TempY = (Convert.ToDouble(y) / Resized_Top_Height) * Original_Top_Height;
                        Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);

                    }

                    else if (y < Resized_Top_Height + Middle_Height)
                    {
                        int Origin_Nearest_y = (y - Resized_Top_Height) + Original_Top_Height;
                        if (Origin_Nearest_y > Origin_img.Height - 1) Origin_Nearest_y = Origin_img.Height - 1;

                        int Origin_byteOffset = Origin_Nearest_y * Origin_img.Stride + x * 4;
                        int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                        ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                        ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                        ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                        ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                    }

                    else //if (x < Resized_Left_Width + middle + Resized_Right_Width)
                    {
                        double TempX = (double)x;
                        double TempY = (Convert.ToDouble(y - (Resized_Top_Height + Middle_Height)) / Resized_Bottom_Height) * Original_Bottom_Height;
                        TempY += (Original_Top_Height + Middle_Height);
                        Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);
                    }
                }

            }
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }


        public Bitmap Resize_Nearest_Interpolation_Only_Left_Area(Bitmap img, int X, int Resized_Width)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            int Origin_Left_Width = X;
            int Right_Width = Origin_img.Width - Origin_Left_Width;
            int Resized_Left_Width = Resized_Width - Right_Width;

            FastBitmap Resized_img = new FastBitmap(Resized_Width, Origin_img.Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);


            for (int y = 0; y < Resized_img.Height; y++)
            {
                for (int x = 0; x < Resized_Left_Width; x++)
                {
                    int Origin_Nearest_x = Convert.ToInt32((Convert.ToDouble(x) / Resized_Left_Width) * Origin_Left_Width);
                    if (Origin_Nearest_x > Origin_Left_Width - 1) Origin_Nearest_x = Origin_Left_Width - 1;

                    int Origin_byteOffset = y * Origin_img.Stride + Origin_Nearest_x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }

                
                for(int x = Resized_Left_Width; x< Resized_Width;x++)
                {
                    int Origin_Nearest_x = (x - Resized_Left_Width) + Origin_Left_Width;
                    if (Origin_Nearest_x > Origin_img.Width - 1) Origin_Nearest_x = Origin_img.Width - 1;

                    int Origin_byteOffset = y * Origin_img.Stride + Origin_Nearest_x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;

        }

        public Bitmap Resize_Nearest_Interpolation_Only_Right_Area(Bitmap img, int X, int Resized_Width)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            int Left_Width = X;

            int Original_Right_Width = Origin_img.Width - Left_Width;
            int Resized_Right_Width = Resized_Width - Left_Width;

            FastBitmap Resized_img = new FastBitmap(Resized_Width, Origin_img.Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);


            for (int y = 0; y < Resized_img.Height; y++)
            {
                for (int x = 0; x < Left_Width; x++)
                {
                    int Origin_byteOffset = y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }


                for (int x = Left_Width; x < Resized_Width; x++)
                {
                    int Origin_Nearest_x = Convert.ToInt32((Convert.ToDouble(x - Left_Width) / Resized_Right_Width) * Original_Right_Width);
                    Origin_Nearest_x += Left_Width;
                    if (Origin_Nearest_x > Origin_img.Width - 1) Origin_Nearest_x = Origin_img.Width - 1;

                    int Origin_byteOffset = y * Origin_img.Stride + Origin_Nearest_x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }

        public Bitmap Resize_Nearest_Interpolation_Only_Top_Area(Bitmap img, int Y, int Resized_Height)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            int Origin_Top_Height = Y;
            int Bottom_Height = Origin_img.Height - Y;
            int Resized_Top_Height = Resized_Height - Bottom_Height;
            
            FastBitmap Resized_img = new FastBitmap(Origin_img.Width,Resized_Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);


            for (int x = 0; x < Resized_img.Width; x++)
            {
                for (int y = 0; y < Resized_Top_Height; y++)
                {
                    int Origin_Nearest_y = Convert.ToInt32((Convert.ToDouble(y) / Resized_Top_Height) * Origin_Top_Height);
                    
                    if (Origin_Nearest_y > Origin_img.Height - 1) Origin_Nearest_y = Origin_img.Height - 1;

                    int Origin_byteOffset = Origin_Nearest_y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }


                for (int y = Resized_Top_Height; y < Resized_Height; y++)
                {
                    int Origin_Nearest_y = (y - Resized_Top_Height) + Origin_Top_Height;
                    if (Origin_Nearest_y > Origin_img.Height - 1) Origin_Nearest_y = Origin_img.Height - 1;

                    int Origin_byteOffset = Origin_Nearest_y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }

        public Bitmap Resize_Nearest_Interpolation_Only_Bottom_Area(Bitmap img, int Y,int Resized_Height)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            int Top_Height = Y;
            int Original_Bottom_Height = Origin_img.Height - Top_Height;
            int Resized_Bottom_Height = Resized_Height - Top_Height;

            FastBitmap Resized_img = new FastBitmap(Origin_img.Width, Resized_Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);


            for (int x = 0; x < Resized_img.Width; x++)
            {
                for (int y = 0; y < Top_Height; y++)
                {
                    int Origin_byteOffset = y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }


                for (int y = Top_Height; y < Resized_Height; y++)
                {
                    int Origin_Nearest_y = Convert.ToInt32((Convert.ToDouble(y - Top_Height) / Resized_Bottom_Height) * Original_Bottom_Height);
                    Origin_Nearest_y += Top_Height;
                    if (Origin_Nearest_y > Origin_img.Height - 1) Origin_Nearest_y = Origin_img.Height - 1;

                    int Origin_byteOffset = Origin_Nearest_y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }



        public Bitmap Resize_Nearest_Interpolation(Bitmap img, int Resized_Width, int Resized_Height)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            FastBitmap Resized_img = new FastBitmap(Resized_Width, Resized_Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);

            double R = 0, G = 0, B = 0;
            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;

            for (int y = 0; y < Resized_img.Height; y++)
            {
                for (int x = 0; x < Resized_img.Width; x++)
                {
                    int Origin_Nearest_x = Convert.ToInt32((Convert.ToDouble(x) / Resized_img.Width) * Origin_img.Width);
                    if (Origin_Nearest_x > Origin_img.Width - 1) Origin_Nearest_x = Origin_img.Width - 1;

                    int Origin_Nearest_y = Convert.ToInt32((Convert.ToDouble(y) / Resized_img.Height) * Origin_img.Height);
                    if (Origin_Nearest_y > Origin_img.Height - 1) Origin_Nearest_y = Origin_img.Height - 1;

                    int Origin_byteOffset = Origin_Nearest_y * Origin_img.Stride + Origin_Nearest_x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();

            return Resized_img.Bitmap;
        }


        public Bitmap Resize_Bilinear_Interpolation_Only_Left_Area(Bitmap img, int X, int Resized_Width)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            int Origin_Left_Width = X;
            int Right_Width = Origin_img.Width - Origin_Left_Width;
            int Resized_Left_Width = Resized_Width - Right_Width;

            FastBitmap Resized_img = new FastBitmap(Resized_Width, Origin_img.Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);


            for (int y = 0; y < Resized_img.Height; y++)
            {
                for (int x = 0; x < Resized_Left_Width; x++)
                {
                    double TempX = (Convert.ToDouble(x) / Resized_Left_Width) * Origin_Left_Width;
                    double TempY = (double)y;

                    Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);
                }


                for (int x = Resized_Left_Width; x < Resized_Width; x++)
                {
                    int Origin_Nearest_x = (x - Resized_Left_Width) + Origin_Left_Width;
                    if (Origin_Nearest_x > Origin_img.Width - 1) Origin_Nearest_x = Origin_img.Width - 1;

                    int Origin_byteOffset = y * Origin_img.Stride + Origin_Nearest_x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }

        public Bitmap Resize_Bilinear_Interpolation_Only_Right_Area(Bitmap img, int X, int Resized_Width)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            int Left_Width = X;

            int Original_Right_Width = Origin_img.Width - Left_Width;
            int Resized_Right_Width = Resized_Width - Left_Width;

            FastBitmap Resized_img = new FastBitmap(Resized_Width, Origin_img.Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);


            for (int y = 0; y < Resized_img.Height; y++)
            {
                for (int x = 0; x < Left_Width; x++)
                {
                    int Origin_byteOffset = y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }

                for (int x = Left_Width; x < Resized_Width; x++)
                {
                    double TempX = (Convert.ToDouble(x - Left_Width) / Resized_Right_Width) * Original_Right_Width;
                    TempX += Left_Width;
                    double TempY = (double)y;

                    Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }

        public Bitmap Resize_Bilinear_Interpolation_Only_Top_Area(Bitmap img, int Y, int Resized_Height)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            int Origin_Top_Height = Y;
            int Bottom_Height = Origin_img.Height - Y;
            int Resized_Top_Height = Resized_Height - Bottom_Height;

            FastBitmap Resized_img = new FastBitmap(Origin_img.Width, Resized_Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);

            for (int x = 0; x < Resized_img.Width; x++)
            {
                for (int y = 0; y < Resized_Top_Height; y++)
                { 
                    double TempX = (double)x;
                    double TempY = (Convert.ToDouble(y) / Resized_Top_Height) * Origin_Top_Height;

                    Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);
                }


                for (int y = Resized_Top_Height; y < Resized_Height; y++)
                {
                    int Origin_Nearest_y = (y - Resized_Top_Height) + Origin_Top_Height;
                    if (Origin_Nearest_y > Origin_img.Height - 1) Origin_Nearest_y = Origin_img.Height - 1;

                    int Origin_byteOffset = Origin_Nearest_y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }

       
        public Bitmap Resize_Bilinear_Interpolation_Only_Bottom_Area(Bitmap img, int Y, int Resized_Height)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            int Top_Height = Y;
            int Original_Bottom_Height = Origin_img.Height - Top_Height;
            int Resized_Bottom_Height = Resized_Height - Top_Height;

            FastBitmap Resized_img = new FastBitmap(Origin_img.Width, Resized_Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);


            for (int x = 0; x < Resized_img.Width; x++)
            {
                for (int y = 0; y < Top_Height; y++)
                {
                    int Origin_byteOffset = y * Origin_img.Stride + x * 4;
                    int Resized_byteOffset = y * Resized_img.Stride + x * 4;

                    ResultRGB[Resized_byteOffset + 3] = 255;//투명도
                    ResultRGB[Resized_byteOffset + 2] = ImgRGB[Origin_byteOffset + 2];//R
                    ResultRGB[Resized_byteOffset + 1] = ImgRGB[Origin_byteOffset + 1];//G
                    ResultRGB[Resized_byteOffset + 0] = ImgRGB[Origin_byteOffset + 0];//B
                }


                for (int y = Top_Height; y < Resized_Height; y++)
                {
                    double TempX = (double)x;
                    double TempY = (Convert.ToDouble(y - Top_Height) / Resized_Bottom_Height) * Original_Bottom_Height;
                    TempY += Top_Height;

                    Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();
            return Resized_img.Bitmap;
        }

        public Bitmap Resize_Bilinear_Interpolation(Bitmap img, int Resized_Width, int Resized_Height)
        {
            FastBitmap Origin_img = new FastBitmap(img);
            byte[] ImgRGB = new byte[Origin_img.Stride * Origin_img.Height];
            Marshal.Copy(Origin_img.Scan0, ImgRGB, 0, ImgRGB.Length);
            Origin_img.Dispose();

            FastBitmap Resized_img = new FastBitmap(Resized_Width, Resized_Height);
            byte[] ResultRGB = new byte[Resized_img.Stride * Resized_img.Height];
            Marshal.Copy(Resized_img.Scan0, ResultRGB, 0, ResultRGB.Length);

            bmp_form().progressBar1.Maximum = 10;
            bmp_form().progressBar1.Value = 0;
            bmp_form().progressBar1.Step = 1;

            for (int y = 0; y < Resized_img.Height; y++)
            {
                for (int x = 0; x < Resized_img.Width; x++)
                {
                    double TempX = (Convert.ToDouble(x) / Resized_img.Width) * Origin_img.Width;
                    double TempY = (Convert.ToDouble(y) / Resized_img.Height) * Origin_img.Height;
                    Bilinear_Interpolation(Origin_img, TempX, TempY, ImgRGB, Resized_img, x, y, ResultRGB);
                }
            }

            bmp_form().progressBar1.Value = bmp_form().progressBar1.Maximum;
            Marshal.Copy(ResultRGB, 0, Resized_img.Scan0, ResultRGB.Length);
            Resized_img.Dispose();

            return Resized_img.Bitmap;
        }
        private void Bilinear_Interpolation(FastBitmap Origin_img, double TempX, double TempY, byte[] ImgRGB, FastBitmap Resized_img, int x, int y, byte[] ResultRGB)
        {
            int Origin_x1 = (int)(TempX);
            int Origin_x2 = Origin_x1 + 1;
            int Origin_y1 = (int)(TempY);
            int Origin_y2 = Origin_y1 + 1;

            if (Origin_x2 > Origin_img.Width - 1)
            {
                Origin_x1 = Origin_img.Width - 2;
                Origin_x2 = Origin_img.Width - 1;
                TempX = (Origin_x1 + Origin_x2) / 2;
            }

            if (Origin_y2 > Origin_img.Height - 1)
            {
                Origin_y1 = Origin_img.Height - 2;
                Origin_y2 = Origin_img.Height - 1;
                TempY = (Origin_y1 + Origin_y2) / 2;
            }

            int Origin_Q11_Offset = Origin_y1 * Origin_img.Stride + Origin_x1 * 4;
            int Origin_Q12_Offset = Origin_y2 * Origin_img.Stride + Origin_x1 * 4;
            int Origin_Q21_Offset = Origin_y1 * Origin_img.Stride + Origin_x2 * 4;
            int Origin_Q22_Offset = Origin_y2 * Origin_img.Stride + Origin_x2 * 4;

            //Red
            double BiLinearArea = Convert.ToDouble((Origin_x2 - Origin_x1) * (Origin_y2 - Origin_y1));

            double R = ((ImgRGB[Origin_Q11_Offset + 2] / BiLinearArea) * ((Origin_x2 - TempX) * (Origin_y2 - TempY)))
                + ((ImgRGB[Origin_Q21_Offset + 2] / BiLinearArea) * ((TempX - Origin_x1) * (Origin_y2 - TempY)))
                + ((ImgRGB[Origin_Q12_Offset + 2] / BiLinearArea) * ((Origin_x2 - TempX) * (TempY - Origin_y1)))
                + ((ImgRGB[Origin_Q22_Offset + 2] / BiLinearArea) * ((TempX - Origin_x1) * (TempY - Origin_y1)));

            double G = ((ImgRGB[Origin_Q11_Offset + 1] / BiLinearArea) * ((Origin_x2 - TempX) * (Origin_y2 - TempY)))
           + ((ImgRGB[Origin_Q21_Offset + 1] / BiLinearArea) * ((TempX - Origin_x1) * (Origin_y2 - TempY)))
           + ((ImgRGB[Origin_Q12_Offset + 1] / BiLinearArea) * ((Origin_x2 - TempX) * (TempY - Origin_y1)))
           + ((ImgRGB[Origin_Q22_Offset + 1] / BiLinearArea) * ((TempX - Origin_x1) * (TempY - Origin_y1)));

            double B = ((ImgRGB[Origin_Q11_Offset + 0] / BiLinearArea) * ((Origin_x2 - TempX) * (Origin_y2 - TempY)))
           + ((ImgRGB[Origin_Q21_Offset + 0] / BiLinearArea) * ((TempX - Origin_x1) * (Origin_y2 - TempY)))
           + ((ImgRGB[Origin_Q12_Offset + 0] / BiLinearArea) * ((Origin_x2 - TempX) * (TempY - Origin_y1)))
           + ((ImgRGB[Origin_Q22_Offset + 0] / BiLinearArea) * ((TempX - Origin_x1) * (TempY - Origin_y1)));

            if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;

            int Resized_byteOffset = y * Resized_img.Stride + x * 4;
            ResultRGB[Resized_byteOffset + 3] = 255;//투명도
            ResultRGB[Resized_byteOffset + 2] = Convert.ToByte(R);
            ResultRGB[Resized_byteOffset + 1] = Convert.ToByte(G);
            ResultRGB[Resized_byteOffset + 0] = Convert.ToByte(B);
        }


    }
}
