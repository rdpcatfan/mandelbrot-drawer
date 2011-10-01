using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot
{
    struct PartInfo
    {
        public IntPtr imageData;
        public int pyPartSize;
        public int pxPartSize;
        public int pxImageSize;
        public double rxStart;
        public double ryStart;
        public double scale;
        public int iMax;
        public ColourPalette palette;

        public PartInfo(int pxImageSize, double rScale, int iMax, ColourPalette palette)
        {
            this.imageData = (IntPtr)0; // safe, will always be assigned to later
            this.pyPartSize = -1;
            this.pxPartSize = -1;
            this.pxImageSize = pxImageSize;
            this.rxStart = Double.NaN;
            this.ryStart = Double.NaN;
            this.scale = rScale;
            this.iMax = iMax;
            this.palette = palette;
        }
    }
}
