using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Mandelbrot
{
    /* Class for the storage of information on how one image maps into another
     * image.
     * 
     * There are several important things to know about the images.  An image
     * may be clearer (here, 1s represent the mapped-from image and 2s the
     * mapped-to image;  the borders are to be considered part of each image.
     *  
     *      beginning of area that overlaps
     *      |     end of area that overlaps
     *      |     |
     *      v     v
     *      +--------------+
     *      |11111111111111|
     *      |11111111111111|
     * +----------+11111111| <-- beginning of area that overlaps
     * |2222222222|11111111|
     * |2222222222|11111111|
     * |2222222222|--------+ <-- end of area that overlaps
     * |2222222222|
     * +----------+
     *        
     * As two images:
     * beginning of area that overlaps
     * |     end of area that overlaps
     * |     |
     * v     v
     * +--------------+
     * |11111111111111|
     * |11111111111111|
     * |11111111111111| <-- beginning of area that overlaps
     * |11111111111111| <-- end of area that overlaps
     * |11111111111111|
     * +--------------+
     * 
     *      beginning of area that overlaps
     *      |     end of area that overlaps
     *      |     |
     *      v     v
     * +----------+  <-- beginning of area that overlaps
     * |2222222222|
     * |2222222222|
     * |2222222222|  <-- end of area that overlaps
     * |2222222222|
     * +----------+
     * 
     * As these images show, there are six things to be stored:
     * 0. Horizontal beginning of the area of the first image that overlaps
     * 1. Vertical beginning of the area of the first image that overlaps
     * 2. Horizontal beginning of the area of the second image that overlaps
     * 3. Vertical beginning of the area of the first image that overlaps
     * 4. Horizontal size of the area of the overlapping area
     * 5. Vertical size of the area of the overlapping area
     * 
     * Note that points 0 and 1 are in the coordinates of the first image, while
     * 2 and 3 are in the coordinates of the second image.
     */
    class ImageCombination
    {
        public readonly int pxSize;
        public readonly int pySize;
        public readonly int pxBeginInSource;
        public readonly int pyBeginInSource;
        public readonly int pxBeginInDestination;
        public readonly int pyBeginInDestination;
        
        // Overlap in the second rectangle.
        public Rectangle OverlapInSecond
        {
            get
            {
                return new Rectangle(pxBeginInDestination, pyBeginInDestination, pxSize, pySize);
            }
        }

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
    }
}
