using System;
using System.Diagnostics;
using System.Drawing;

namespace Mandelbrot
{
    /// <summary>
    /// Class for the storage of information on how one image maps into another
    /// image.
    ///
    /// There are several important things to know about the images.  An image
    /// may be clearer (here, 1s represent the mapped-from image and 2s the
    /// mapped-to image;  the borders are to be considered part of each image.
    ///
    /// <code>
    ///      beginning of area that overlaps
    ///      |     end of area that overlaps
    ///      |     |
    ///      v     v
    ///      +--------------+
    ///      |11111111111111|
    ///      |11111111111111|
    /// +----------+11111111| c-- beginning of area that overlaps
    /// |2222222222|11111111|
    /// |2222222222|11111111|
    /// |2222222222|--------+ c-- end of area that overlaps
    /// |2222222222|
    /// +----------+
    /// </code>
    /// 
    /// As two images:
    /// <code>
    /// beginning of area that overlaps
    /// |     end of area that overlaps
    /// |     |
    /// v     v
    /// +--------------+
    /// |11111111111111|
    /// |11111111111111|
    /// |11111111111111| c-- beginning of area that overlaps
    /// |11111111111111| c-- end of area that overlaps
    /// |11111111111111|
    /// +--------------+
    ///
    ///      beginning of area that overlaps
    ///      |     end of area that overlaps
    ///      |     |
    ///      v     v
    /// +----------+  c-- beginning of area that overlaps
    /// |2222222222|
    /// |2222222222|
    /// |2222222222|  c-- end of area that overlaps
    /// |2222222222|
    /// +----------+
    /// </code>
    ///
    /// As these images show, there are six things to be stored:
    /// 0. Horizontal beginning of the area of the first image that overlaps
    /// 1. Vertical beginning of the area of the first image that overlaps
    /// 2. Horizontal beginning of the area of the second image that overlaps
    /// 3. Vertical beginning of the area of the first image that overlaps
    /// 4. Horizontal size of the area of the overlapping area
    /// 5. Vertical size of the area of the overlapping area
    /// 
    /// Note that points 0 and 1 are in the coordinate system of the first image,
    /// while 2 and 3 are in the coordinate system of the second image.
    /// </summary>
    /// <remarks>
    /// This class uses the terms `source', `original', and `mapFrom', as well as
    /// the terms `destination', `target', and `mapTo'.  From each of the three,
    /// one should be selected and stuck to.
    /// </remarks>
    class ImageCombination
    {
        #region member variables
        /// <summary>
        /// The horizontal size of the overlap in pixels.
        /// </summary>
        public readonly int pxSize;

        /// <summary>
        /// The vertical size of the overlap in pixels.
        /// </summary>
        public readonly int pySize;

        /// <summary>
        /// The leftmost point of the overlapping area in the source image.
        /// </summary>
        public readonly int pxBeginInSource;

        /// <summary>
        /// The topmost point of the overlapping area in the source image.
        /// </summary>
        public readonly int pyBeginInSource;

        /// <summary>
        /// The leftmost point of the overlapping area in the destination image.
        /// </summary>
        public readonly int pxBeginInDestination;

        /// <summary>
        /// The beginning x-coordinate of the overlapping area in the source image.
        /// </summary>
        public readonly int pyBeginInDestination;
        #endregion

        #region properties
        /// <summary>
        /// Return a rectangle representing the overlap in the first image.
        /// </summary>
        public Rectangle OverlapInSecond
        {
            get
            {
                return new Rectangle(pxBeginInDestination, pyBeginInDestination, pxSize, pySize);
            }
        }
        #endregion

        #region costructors
        /// <summary>
        /// Calculate the overlapping area of two images, in terms of the coordinates
        /// of each image and keep track of it.  The two images are treated equally,
        /// but the image to be copied from should be passed first.
        /// </summary>
        /// <param name="mapFrom">Original image.</param>
        /// <param name="mapTo">Target image.</param>
        public ImageCombination(ImageInfo mapFrom, ImageInfo mapTo)
        {
            Debug.Assert(mapFrom.rScale == mapTo.rScale,
                "Attempted to map images with different scales.");

            Point centreInMapTo = mapTo.pCoordinatesOf(mapFrom.rxCentre, mapFrom.ryCentre);
            Point centreInMapFrom = mapFrom.pCentre;

            // The following four lines construct a description of the rectangle to be copied,
            // relative to a centre in each image.
            int pxToBegin = Math.Min(centreInMapFrom.X, centreInMapTo.X);
            int pyToBegin = Math.Min(centreInMapFrom.Y, centreInMapTo.Y);
            int pxToEnd = Math.Min(mapFrom.pxSize - centreInMapFrom.X, mapTo.pxSize - centreInMapTo.X);
            int pyToEnd = Math.Min(mapFrom.pySize - centreInMapFrom.Y, mapTo.pySize - centreInMapTo.Y);
            
            // Now let's translate that rectangle into coordinates of each image.
            this.pxBeginInSource = centreInMapFrom.X - pxToBegin;
            this.pxBeginInDestination = centreInMapTo.X - pxToBegin;
            this.pyBeginInSource = centreInMapFrom.Y - pyToBegin;
            this.pyBeginInDestination = centreInMapTo.Y - pyToBegin;
            this.pxSize = pxToBegin + pxToEnd;
            this.pySize = pyToBegin + pyToEnd;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Check whether it's possible to map the first image to the second one.
        /// </summary>
        /// <param name="mapFrom">Original image.</param>
        /// <param name="mapTo">Target image.</param>
        /// <returns>True if the images are compatible.</returns>
        static public bool CombinationPossible(ImageInfo mapFrom, ImageInfo mapTo)
        {
            return
                mapFrom.rScale == mapTo.rScale &&
                mapFrom.rxCentre != mapTo.rxCentre &&
                mapFrom.ryCentre != mapTo.ryCentre &&
                mapFrom.iMax == mapTo.iMax &&
                mapFrom.palette == mapTo.palette;
        }
        #endregion
    }
}
