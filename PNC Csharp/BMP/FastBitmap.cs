using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;


namespace PNC_Csharp
{
    class FastBitmap : IDisposable
    {
        #region Public Members
        public Bitmap Bitmap;//The bitmap
        public readonly BitmapData Data;// The data of bitmap
        public readonly int PixelSize;// The pixel size
        public readonly int XX;// The left of rectangle proccesed
        public readonly int YY;// The top of rectangle proccesed
        public readonly int Width;// The width of rectangle proccesed
        public readonly int Height;// The height of rectangle proccesed
        public readonly int Stride;// the width of a single row
        public readonly IntPtr Scan0;// The first pixel location in rectangle proccesed
        #endregion

        #region private
        private int FindPixelSize()
        {
            if (Data.PixelFormat == PixelFormat.Format24bppRgb)
            {
                return 3;
            }
            if (Data.PixelFormat == PixelFormat.Format32bppArgb)
            {
                return 4;
            }
            return 4;
        }
        #endregion

        #region Constructors
        public FastBitmap(Bitmap bitmap, int xx, int yy, int width, int height)
        {
            this.Bitmap = bitmap;
            XX = xx;
            YY = yy;
            Width = width;
            Height = height;
            Data = this.Bitmap.LockBits(new Rectangle(xx, yy, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            PixelSize = FindPixelSize(); // holds the size of pixel in bytes
            Stride = Data.Stride;
            Scan0 = Data.Scan0;//holds a pointer to the location of the left top pixel of the rectangle we want to process.
        }
        public FastBitmap(Bitmap bitmap)
            : this(bitmap, 0, 0, bitmap.Width, bitmap.Height)
        {
        }

        public FastBitmap(int width,int height)
            : this(new Bitmap(width,height), 0, 0, width, height)
        {
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Bitmap.UnlockBits(Data); 
        }
        #endregion
    }
}
