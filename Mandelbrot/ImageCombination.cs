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
        public readonly int pxBeginInFirst;
        public readonly int pyBeginInFirst;
        public readonly int pxBeginInSecond;
        public readonly int pyBeginInSecond;
        
        // Overlap in the second rectangle.
        public Rectangle OverlapInSecond
        {
            get
            {
                return new Rectangle(pxBeginInSecond, pyBeginInSecond, pxSize, pySize);
            }
        }

        public ImageCombination(ImageInfo mapFrom, ImageInfo mapTo)
        {
            // This method does not work if the size of mapFrom and mapTo are not
            // equal, it's probably best to rewrite this, but I'm not yet sure how.
            // (Also, FractalGenerator does not support that yet.)
            Debug.Assert(mapFrom.pSize == mapTo.pSize,
                "Sorry, mapping of images of different sizes is not yet supported.");
            Debug.Assert(mapFrom.rScale == mapTo.rScale,
                "Attempted to map images with different scales.");
            int pxShift = (int)((mapFrom.rxCentre - mapTo.rxCentre) / mapFrom.rScale);
            int pyShift = -(int)((mapFrom.ryCentre - mapTo.ryCentre) / mapFrom.rScale);
            if (pxShift > 0)
            {
                this.pxBeginInFirst = pxShift;
                this.pxBeginInSecond = 0;
            }
            else
            {
                this.pxBeginInFirst = 0;
                this.pxBeginInSecond = -pxShift;
            }
            if (pyShift > 0)
            {
                this.pyBeginInFirst = pyShift;
                this.pyBeginInSecond = 0;
            }
            else
            {
                this.pyBeginInFirst = 0;
                this.pyBeginInSecond = -pyShift;
            }
            this.pxSize = mapFrom.pxSize - Math.Abs(pxShift);
            this.pySize = mapFrom.pySize - Math.Abs(pyShift);
        }
    }
}
