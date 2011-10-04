using System;
using System.Diagnostics;
using System.Drawing;

namespace Mandelbrot
{
    /// <summary>
    /// Stores all information necessary to generate an image.
    /// </summary>
    /// <remarks>Instances of this class are immutable.</remarks>
    class ImageInfo
    {
        #region member variables
        /// <summary>
        /// Horizontal size of image in pixels.
        /// </summary>
        public readonly int pxSize;
        
        /// <summary>
        /// Vertical size of image in pixels.
        /// </summary>
        public readonly int pySize;
        
        /// <summary>
        /// X-value of the point at the centre of the image.
        /// </summary>
        public readonly double rxCentre;
        
        /// <summary>
        /// Y-value of the point at the centre of the image.
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

        #region properties
        /// <summary>
        /// X-coordinate of top-left point.
        /// </summary>
        public double rxBegin
        {
            get { return this.rxCentre - this.pxCentre * this.rScale; }
        }

        /// <summary>
        /// Y-coordinate of top-left point.
        /// </summary>
        public double ryBegin
        {
            get { return this.ryCentre + this.pyCentre * this.rScale; }
        }

        /// <summary>
        /// X-coordinate of bottom-right point.
        /// </summary>
        public double rxEnd
        {
            get { return this.rxCentre + this.pxCentre * this.rScale; }
        }

        /// <summary>
        /// Y-coordianet of bottom-right point.
        /// </summary>
        public double ryEnd
        {
            get { return this.ryCentre - this.pyCentre * this.rScale; }
        }

        /// <summary>
        /// Size of the image.
        /// </summary>
        public Size pSize
        {
            get { return new Size(this.pxSize, this.pySize); }
        }

        /// <summary>
        /// Coordinates of the centre point.
        /// </summary>
        public Point pCentre
        {
            get { return new Point(this.pxCentre, this.pyCentre); }
        }

        /// <summary>
        /// X-coordinate of the centre pixel.
        /// </summary>
        public int pxCentre
        {
            get { return this.pSize.Width / 2; }
        }

        /// <summary>
        /// Y-coordinate of the centre pixel.
        /// </summary>
        public int pyCentre
        {
            get { return this.pSize.Height / 2; }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Constructor for creating ImageInfo of non-existant images.
        /// </summary>
        public ImageInfo()
        {
            this.pxSize = this.pySize = this.iMax = 0;
            this.rxCentre = this.ryCentre = this.rScale = Double.NaN;
            // NaN == x is false for all x, so the rScale always seems
            // incorrect.
            this.palette = null; // Should never be used to generate anything, anyway.
        }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        /// <param name="pxSize">Horizontal size of image</param>
        /// <param name="pySize">Vertical size of image</param>
        /// <param name="rxCentre">Horizontal size of fractal</param>
        /// <param name="ryCentre">Vertical size of fractal</param>
        /// <param name="rScale">Scale of the fractal</param>
        /// <param name="iMax">Maximum number of iterations to try for</param>
        /// <param name="palette">Palette to use</param>
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

        /// <summary>
        /// Convenience method for constructing from a Size.
        /// </summary>
        public ImageInfo(Size pSize, double rxCentre, double ryCentre, double rScale, int iMax, ColourPalette palette) :
            this(pSize.Width, pSize.Height, rxCentre, ryCentre, rScale, iMax, palette)
        { }
        #endregion

        #region public methods
        /// <summary>
        /// Returns the X-value of the point at the given position.
        /// </summary>
        public double rxValue(int pxPosition)
        {
            return this.rxCentre + this.rScale * (pxPosition - this.pxCentre);
        }

        /// <summary>
        /// Returns the Y-value of the point at the given position.
        /// </summary>
        public double ryValue(int pyPosition)
        {
            return this.ryCentre - this.rScale * (pyPosition - this.pyCentre);
        }

        /// <summary>
        /// Returns the X-coordinate of the point with the given X-value.
        /// </summary>
        public int pxCoordinateOf(double rxPoint)
        {
            return (int)((rxPoint - rxBegin) / rScale);
        }

        /// <summary>
        /// Returns the Y-coordinate of the point with the given Y-value.
        /// </summary>
        public int pyCoordinateOf(double ryPoint)
        {
            return (int)(-(ryPoint - ryBegin) / rScale);
        }

        /// <summary>
        /// Returns the coordinates of the point with the given values.
        /// </summary>
        public Point pCoordinatesOf(double rxPoint, double ryPoint)
        {
            return new Point(pxCoordinateOf(rxPoint), pyCoordinateOf(ryPoint));
        }
        #endregion
    }
}
