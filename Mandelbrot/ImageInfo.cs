using System;
using System.Drawing;
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
        #region members
        public readonly int pxSize;
        public readonly int pySize;
        public readonly double rxCentre;
        public readonly double ryCentre;
        public readonly double rScale;
        #endregion

        #region help properties
        public double rxBegin
        {
            get { return this.rxCentre - this.pxCentre * this.rScale; }
        }

        public double ryBegin
        {
            get { return this.ryCentre + this.pyCentre * this.rScale; }
        }

        public double rxEnd
        {
            get { return this.rxCentre + this.pxCentre * this.rScale; }
        }

        public double ryEnd
        {
            get { return this.ryCentre - this.pyCentre * this.rScale; }
        }

        public Size pSize
        {
            get { return new Size(this.pxSize, this.pySize); }
        }

        public Point pCentre
        {
            get { return new Point(this.pxCentre, this.pyCentre); }
        }

        public int pxCentre
        {
            get { return this.pSize.Width / 2; }
        }

        public int pyCentre
        {
            get { return this.pSize.Height / 2; }
        }
        #endregion

        public ImageInfo()
        {
            this.pxSize = this.pySize = 0;
            this.rxCentre = this.ryCentre = this.rScale = Double.NaN;
            // NaN == x is false for all x, so the rScale always seems
            // incorrect.
        }

        public ImageInfo(int pxSize, int pySize, double rxCentre, double ryCentre, double rScale)
        {
            this.pxSize = pxSize;
            this.pySize = pySize;
            this.rxCentre = rxCentre;
            this.ryCentre = ryCentre;
            this.rScale = rScale;
            Debug.Assert(rScale > 0, "Scale invalid.");
        }

        // Convenience method for constructing from a Size.
        public ImageInfo(Size pSize, double rxCentre, double ryCentre, double rScale) :
            this(pSize.Width, pSize.Height, rxCentre, ryCentre, rScale)
        {}

        public double rxValue(int pxPosition)
        {
            return this.rxCentre + this.rScale * (pxPosition - this.pxCentre);
        }

        public double ryValue(int pyPosition)
        {
            return this.ryCentre - this.rScale * (pyPosition - this.pyCentre);
        }

        public int pxCoordinateOf(double rxPoint)
        {
            return (int)((rxPoint - rxBegin) / rScale);
        }

        public int pyCoordinateOf(double ryPoint)
        {
            return (int)(-(ryPoint - ryBegin) / rScale);
        }

        public Point pCoordinatesOf(double rxPoint, double ryPoint)
        {
            return new Point(pxCoordinateOf(rxPoint), pyCoordinateOf(ryPoint));
        }
    }
}
