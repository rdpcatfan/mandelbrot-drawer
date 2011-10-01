using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Mandelbrot
{
    /// <summary>
    /// Stores all information necessary to generate an image.
    /// </summary>
    /// <remarks>Instances of this class are immutable.</remarks>
    class ImageInfo
    {
        #region members
        /// <summary>
        /// Horizontal size of image in pixels.
        /// </summary>
        public readonly int pxSize;
        /// <summary>
        /// Vertical size of image in pixels.
        /// </summary>
        public readonly int pySize;
        /// <summary>
        /// X-coordinate of the point at the centre of the image, a real number.
        /// </summary>
        public readonly double rxCentre;
        /// <summary>
        /// Y-coordinate of the point at the centre of the image, a real number.
        /// </summary>
        public readonly double ryCentre;
        /// <summary>
        /// Difference in coordinates between two adjacent pixels.
        /// </summary>
        public readonly double rScale;
        /// <summary>
        /// Maximum number of iterations to attempt to check for convergence.
        /// </summary>
        public readonly int iMax;
        /// <summary>
        /// Colour palette to construct the image from.
        /// </summary>
        public readonly ColourPalette palette;
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
            this.pxSize = this.pySize = this.iMax = 0;
            this.rxCentre = this.ryCentre = this.rScale = Double.NaN;
            // NaN == x is false for all x, so the rScale always seems
            // incorrect.
            this.palette = null; // Should never be used to generate anything, anyway.
        }

        public ImageInfo(int pxSize, int pySize, double rxCentre, double ryCentre, double rScale, int iMax, ColourPalette palette)
        {
            this.pxSize = pxSize;
            this.pySize = pySize;
            this.rxCentre = rxCentre;
            this.ryCentre = ryCentre;
            this.rScale = rScale;
            this.iMax = iMax;
            this.palette = palette;
            Debug.Assert(rScale > 0, "Scale invalid.");
        }

        // Convenience method for constructing from a Size.
        public ImageInfo(Size pSize, double rxCentre, double ryCentre, double rScale, int iMax, ColourPalette palette) :
            this(pSize.Width, pSize.Height, rxCentre, ryCentre, rScale, iMax, palette)
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
