using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Mandelbrot
{
    abstract class FractalGenerator : IFractalGenerator
    {
        #region member vars
        protected const int Infinity = 9001;
        protected const int InfinityPlusOne = 9002;
        #endregion

        public Image generate(int width, int height, double centreX, double centreY, double scale, int iterations)
        {
            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            BitmapData bmd = newImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                newImage.PixelFormat
            );

            double x, y;
            unsafe
            {
                Int32* currentpixel = (Int32*)bmd.Scan0.ToPointer();
                for (int row = 0; row < 500; row++)
                {
                    for (int col = 0; col < 500; col++)
                    {
                        x = centreX + (col - width / 2) * scale;
                        y = centreY + (row - height / 2) * scale;
                        int iterationCount = this.checkConvergence(iterations, x, y);
                        *currentpixel = this.getColour(iterationCount, iterations);
                        currentpixel += 1;
                    }
                }
            }
            newImage.UnlockBits(bmd);
            return newImage;
        }

        protected abstract int checkConvergence(int maxIterations, double x, double y);

        protected abstract Int32 getColour(int iterationCount, int maxIterations);
    }
}
