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

        #region abstract functions
        protected abstract ConvergenceCheckResult checkConvergence(int maxIterations, double x, double y);

        protected abstract Int32 getColour(ConvergenceCheckResult res);
        #endregion

        public Image generate(int width, int height, double centreX, double centreY, double scale, int maxIterations)
        {
            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            BitmapData bmd = newImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                newImage.PixelFormat
            );

            double x = centreX - (width / 2) * scale;
            double y = centreY - (height / 2) * scale;
            IList<PartInfo> parts = makeParts(width, height, scale, centreX, centreY, bmd.Scan0, maxIterations);
            foreach (PartInfo info in parts)
                generatePart(info);
            newImage.UnlockBits(bmd);
            return newImage;
        }

        IList<PartInfo> makeParts(int width, int height, double centreX, double centreY, double scale, IntPtr begin, int maxIterations)
        {
            const int preferredNumberOfPixels = 2000;
            IList<PartInfo> parts = new List<PartInfo>();
            int freePixels = width * height;
            int currentLine = 0;
            while (freePixels != 0)
            {
                PartInfo info = new PartInfo(width, scale, maxIterations);
                info.partWidth = width;
                info.startX = centreX - (width / 2) * scale;
                info.startY = centreY + (currentLine - height / 2) * scale;
                unsafe
                {
                    Int32* rawData = (Int32*)begin.ToPointer();
                    rawData += width * currentLine;
                    info.pToTopLeft = new IntPtr(rawData);
                }
                if (preferredNumberOfPixels <= freePixels)
                {
                    int lines = preferredNumberOfPixels / width;
                    freePixels -= lines * width;
                    currentLine += lines;
                    info.partHeight = lines;
                }
                else
                {
                    freePixels = 0;
                    info.partHeight = height - currentLine;
                    currentLine = height;
                }
                parts.Add(info);
            }
            return parts;
        }

        unsafe void generatePart(PartInfo info)
        {
            Int32* currentPixel = (Int32*)info.pToTopLeft.ToPointer();
            double currentX = info.startX;
            double currentY = info.startY;
            int skippedSpace = info.imageWidth - info.partWidth;
            for (int row = 0; row < info.partWidth; ++row)
            {
                for (int col = 0; col < info.partHeight; ++col)
                {
                    ConvergenceCheckResult res = this.checkConvergence(info.maxIterations, currentX, currentY);
                    *currentPixel = this.getColour(res);
                    currentPixel += 1;
                    currentX += info.scale;
                }
                currentPixel += skippedSpace;
                currentX = info.startX;
                currentY += info.scale;
            }
        }
    }
}
