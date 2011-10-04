using System;

namespace Mandelbrot
{
    /// <summary>
    /// An instance of this class should contain all information necessary to
    /// generate a part of the image.
    /// 
    /// This struct is intended to be used with the FractalGenerator class:
    /// therefore, there has been very little work put into making it safe
    /// and easy-to-use.
    /// </summary>
    struct PartInfo
    {
        #region member variables
        public IntPtr imageData;
        public int pyPartSize;
        public int pxPartSize;
        public int pxImageSize;
        public double rxStart;
        public double ryStart;
        public double scale;
        public int iMax;
        public ColourPalette palette;
        #endregion

        #region constructors
        /// <summary>
        /// Constructor, accepting all variables that can be known as soon as the part is created.
        /// </summary>
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
        #endregion
    }
}
