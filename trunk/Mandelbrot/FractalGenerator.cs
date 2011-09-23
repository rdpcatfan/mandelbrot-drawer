using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace Mandelbrot
{
    abstract class FractalGenerator : IFractalGenerator
    {
        #region member vars
        protected const int Infinity = 9001;
        protected const int InfinityPlusOne = 9002;
        private double oldCentreX;
        private double oldCentreY;
        private double oldScale;
        private int xIgnoredBegin;
        private int yIgnoredBegin;
        private int xIgnoredSize;
        private int yIgnoredSize;
        private Bitmap oldImage;
        #endregion

        #region abstract functions
        protected abstract ConvergenceCheckResult checkConvergence(int maxIterations, double x, double y);

        protected abstract Int32 getColour(ConvergenceCheckResult res);
        #endregion

        public FractalGenerator()
        {
            this.oldCentreX = this.oldCentreY = 0;
            this.oldScale = Double.NaN;
            this.xIgnoredBegin = this.yIgnoredBegin = this.xIgnoredSize = this.yIgnoredSize = 0;
        }

        public Image generate(int width, int height, double centreX, double centreY, double scale, int maxIterations)
        {
            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            BitmapData bmd = newImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                newImage.PixelFormat
            );

            if (this.oldScale == scale)
            {
                int xShift = (int)((centreX - this.oldCentreX) / this.oldScale);
                int yShift = (int)((this.oldCentreY - centreY) / this.oldScale);
                moveImage(bmd, xShift, yShift);
            }
            IList<PartInfo> parts = makeParts(width, height, centreX, centreY, scale, bmd.Scan0, maxIterations);
            Parallel.ForEach(parts, generatePart);
            newImage.UnlockBits(bmd);
            this.oldScale = scale;
            this.oldCentreX = centreX;
            this.oldCentreY = centreY;
            this.oldImage = newImage;
            return newImage;
        }

        IList<PartInfo> makeParts(int width, int height, double centreX, double centreY, double scale, IntPtr begin, int maxIterations)
        {
            const int preferredNumberOfPixels = 10000;
            IList<PartInfo> parts = new List<PartInfo>();
            int freePixels = width * height;
            int currentLine = 0;
            while (freePixels != 0)
            {
                PartInfo info = new PartInfo(width, scale, maxIterations);
                if (currentLine < this.yIgnoredBegin && currentLine >= (this.yIgnoredBegin + this.yIgnoredSize))
                {   // If currentLine is NOT in ignored area
                    info.partWidth = width;
                    info.startX = centreX - (width / 2) * scale;
                    info.startY = centreY + (height / 2 - currentLine) * scale;
                    unsafe
                    {
                        Int32* rawData = (Int32*)begin.ToPointer();
                        rawData += width * currentLine;
                        info.pToTopLeft = new IntPtr(rawData);
                    }
                    if (preferredNumberOfPixels < freePixels)
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
            for (int row = 0; row < info.partHeight; ++row)
            {
                for (int col = 0; col < info.partWidth; ++col)
                {
                    ConvergenceCheckResult res = this.checkConvergence(info.maxIterations, currentX, currentY);
                    *currentPixel = this.getColour(res);
                    currentPixel += 1;
                    currentX += info.scale;
                }
                currentPixel += skippedSpace;
                currentX = info.startX;
                currentY -= info.scale;
            }
        }

        void moveImage(BitmapData newImageData, int xShift, int yShift)
        {
            int xSize, ySize, xSourceBegin, xDestBegin, ySourceBegin, yDestBegin;
            xSize = oldImage.Width - Math.Abs(xShift);
            ySize = oldImage.Height - Math.Abs(yShift);
            xSourceBegin = xShift < 0 ? 0 : xShift;
            xDestBegin = xShift < 0 ? -xShift : 0;
            ySourceBegin = yShift < 0 ? 0 : yShift;
            yDestBegin = yShift > 0 ? 0 : -yShift;
            int width = this.oldImage.Width;
            int height = this.oldImage.Height;
            BitmapData oldImageData = this.oldImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                oldImage.PixelFormat
            );
            
            unsafe {
                Int32* src = (Int32*)oldImageData.Scan0.ToPointer();
                src += xSourceBegin + ySourceBegin * width;
                Int32* dest = (Int32*)newImageData.Scan0.ToPointer();
                dest += xDestBegin + yDestBegin * width;
                for (int i = 0; i < ySize; ++i)
                {
                    for (int j = 0; j < xSize; ++j)
                        dest[j] = src[j];
                    src += width;
                    dest += width;
                }
            }
            this.oldImage.UnlockBits(oldImageData);
        }
    }
}
