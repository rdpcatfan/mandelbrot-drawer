using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Mandelbrot
{
    /* Stores some information about an image in order to make it easier to
     * pass to and from functions.
     * 
     * Immutable.
     */
    class ImageInfo
    {
        public int pxSize { get; private set; }
        public int pySize { get; private set;  }
        public double rxCentre { get; private set; }
        public double ryCentre { get; private set; }
        public double rScale { get; private set; }

        public ImageInfo(int pxSize, int pySize, double rxCentre, double ryCentre, double rScale)
        {
            this.pxSize = pxSize;
            this.pySize = pySize;
            this.rxCentre = rxCentre;
            this.ryCentre = ryCentre;
            this.rScale = rScale;
            Debug.Assert(rScale > 0, "Scale invalid.");
        }
    }
}
