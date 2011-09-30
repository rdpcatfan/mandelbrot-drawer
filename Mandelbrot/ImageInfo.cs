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
        public int pxSize { get; private set; }
        public int pySize { get; private set; }
        public double rxCentre { get; private set; }
        public double ryCentre { get; private set; }
        public double rScale { get; private set; }
        #endregion

        #region help properties
        public double rxBegin
        { get
          { return this.rxCentre - this.pyCentre * this.rScale; }
        }

        public double ryBegin
        {
            get
            { return this.ryCentre + this.pxCentre * this.rScale; }
        }

        public Size pSize
        {
            get
            { return new Size(this.pxSize, this.pySize); }
        }

        public Point pCentre
        {
            get
            { return new Point(this.pxCentre, this.pyCentre); }
        }

        public int pxCentre
        {
            get
            { return this.pSize.Width / 2; }
        }

        public int pyCentre
        {
            get
            { return this.pSize.Height / 2; }
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

        /* Return the difference between two images (in pixels).  The scales
         * of the two images must be equal, as the comparison is meaningless
         * otherwise.
         */
        public Size pOffset(ImageInfo other)
        {
            /* Whether to assert or throw here is an interesting question.
             * Seeing as this method being called with images of different
             * scales would be a programmer error, as opposed to a user
             * error, I've decided to go for the assert option.  Could just
             * be my C++ background, though.  Both throwing and asserting
             * would give more safety in release mode, but I'm hoping input
             * validation will be done by then... -- Anton
             */
            Debug.Assert(this.rScale == other.rScale,
                "Comparison of images with different scales.");
            return new Size(
                (int)((this.rxCentre - other.rxCentre) / this.rScale),
                -(int)((this.ryCentre - other.ryCentre) / this.rScale)
            ); // Note negative for y.
        }

        public double rxValue(int pxPosition)
        {
            return this.rxCentre + this.rScale * (pxPosition - this.pxCentre);
        }

        public double ryValue(int pyPosition)
        {
            return this.ryCentre - this.rScale * (pyPosition - this.pyCentre);
        }
    }
}
