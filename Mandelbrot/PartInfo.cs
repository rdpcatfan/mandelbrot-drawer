using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot
{
    struct PartInfo
    {
        public IntPtr pToTopLeft;
        public int partHeight;
        public int partWidth;
        public int imageWidth;
        public double startX;
        public double startY;
        public double scale;
        public int maxIterations;

        public PartInfo(int imageWidth, double scale, int maxIterations)
        {
            this.pToTopLeft = (IntPtr)0; // safe, will always be assigned to later
            this.partHeight = -1;
            this.partWidth = -1;
            this.imageWidth = imageWidth;
            this.startX = Double.NaN;
            this.startY = Double.NaN;
            this.scale = scale;
            this.maxIterations = maxIterations;
        }
    }
}
