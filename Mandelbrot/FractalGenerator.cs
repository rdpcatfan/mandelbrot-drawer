using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace Mandelbrot
{
    abstract class FractalGenerator : IFractalGenerator
    {
        /* General notes:
         *
         * - Rationale for lack of x and y symmetry
         *
         *   When drawing graphs, mathematicians generally draw the point (0,1)
         *   somewhat to the right and above the (0,0) point.  The C# Bitmap
         *   class, however, has the (0,0) point at the top-left of the screen,
         *   and the (1,1) point is to the right and *below* it.  This means
         *   that if the x and y axis were to be handled the same way, the
         *   (1,1) point of the Mandelbrot Set would be below and to the right
         *   of the (0,0) point:  in other words, the set would be drawn
         *   mirrored along the x axis.
         *
         *   Seeing as the Mandelbrot Set is mathematical entity, it would be
		 *   most rude to hang it up side down.  The cost of this, however,
		 *   is that the formula used for calculating screen coordinates is
		 *   often different for x and y.  The exact consequences of this are
		 *   explained when the issue is encountered.
		 *
		 *
		 * - ints vs doubles
		 *
		 *   When generating the image, there are two kinds of values: those
		 *   of the Image (the width, height, etc) and those of the fractal
		 *   (coordinates, etc).  In order to avoid mistakes, the following
		 *   convention should be used:
		 *
		 *   Unless mentioned otherwise:
		 *    p - All values that refer to the image, and are therefore a
		 *        count of pixels, should be prefixed with a p.
		 *    r - All values that refer to the fractal, and are therefore
		 *        supposed to bereal numbers, should be prefixed with an r.
		 *    i - All values that refer to a number of iterations should
		 *        be prefixed with an i.
		 *
		 *   These prefixes are used because despite int and double being
		 *   different types, an assignment like `rxCentre = pySize;' is not
		 *   a compile error, and this notation makes errors much more
		 *   obvious -- variables with different prefixes may never be
		 *   assigned to each other.
		 *
		 *   A more thorough reasoning for the prefixes can be found here:
		 *   http://www.joelonsoftware.com/articles/Wrong.html
		 *
		 *
		 * - Width and Height
		 *
		 *   All values that represent a horizontal distance, whether they
		 *   refer to the image or the fractal, should be prefixed with an
		 *   `x', and all values that represent a vertical distance should
		 *   be prefixed with a `y'.  This prefix goes after the `p' or 'r'
		 *   mentioned above.  A value with an `i' prefix may not also have
		 *   an `x' or `y' prefix, as it makes no sense.
		 *
         */

        #region member vars
		// A suitably large value that should be returned when a point does
		// not converge.
        protected const int Infinity = 9001;

		// An even larger value that may be used for testing purposes.
        protected const int InfinityPlusOne = 9002;

		// The x coordinate of the centre in the previously generated image.
		// Used to allow the old image to be copied when possible.
        private double oldCentreX;

		// The y coordinate of the centre in the previously generated image.
		// Used to allow the old image to be copied when possible.
        private double oldCentreY;

		// The scale of the previously generated image.  If this is the same
		// in the next image, and the location of the image isn't too far off,
		// a chunk of the old data can be copied.
        private double oldScale;

		// If a piece of the old image has been copied, the x value of the
		// beginning of this piece in the new image is stored here.
		// Otherwise, this is equal to -1.
        private int xIgnoredBegin;

		// If a piece of the old image has been copied, the y value of the
		// beginning of this piece in the new image is stored here.
		// Otherwise, this is equal to -1.
        private int yIgnoredBegin;

		// If a piece of the old image has been copied, the width of the
		// piece is stored here.  Otherwise, this is equal to 0.
        private int xIgnoredSize;

		// If a piece of the old image has been copied, the height of the
		// piece is stored here.  Otherwise, this is equal to 0.
        private int yIgnoredSize;

		// Stores the previous image.  Does not 
        private Bitmap oldImage;
        #endregion

        #region abstract functions

		/* Check whether point (x, y) escapes in no more than maxIterations
		 * iterations.  Return the coordinates reached when it escapes, and
		 * the number of iteration taken to do so.  If the point does not
		 * escape, return Infinity and the coordinates at the last iteration
		 * performed.
		 */
        protected abstract ConvergenceCheckResult checkConvergence(
			int maxIterations,
			double x,
			double y
		);

		/* Given the coordinates of a point and the number of iterations
		 * necessary to reach it, return the colour that that pixel should
		 * be given.
		 *
		 * It might be reasonable to make this a static, non-virtual function
		 * and use a List for storing the colours instead. -- Anton
		 */
        protected abstract Int32 getColour(ConvergenceCheckResult res);

        #endregion

		// Initialise the fractal generator.  Ensure that there's no way that
		// it may seem like copying from the previous image would be a good
		// idea (there is no previous image).
        public FractalGenerator()
        {
            this.oldCentreX = this.oldCentreY = 0;
			// NaN == x is false for all x, so the scale always seems
			// incorrect.
            this.oldScale = Double.NaN;  
			this.xIgnoredBegin = this.yIgnoredBegin = -1;
			this.xIgnoredSize = this.yIgnoredSize = 0;
        }

		// Generate and return an image of size width x height, centred on
		// point (centreX, centreY), with the step size between each pixel
		// being scale.  Test for escape up to maxIterations iterations.
        public Image generate(
			int width,
			int height,
			double centreX,
			double centreY,
			double scale,
			int maxIterations
		)
        {
            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            BitmapData bmd = newImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                newImage.PixelFormat
            );

            if (this.oldScale == scale)
            {
                int xShift = (int)((centreX - this.oldCentreX) / this.oldScale);
                int yShift = (int)((this.oldCentreY - centreY) / this.oldScale);
                this.moveImage(bmd, xShift, yShift);
            }
			else
			{
				this.xIgnoredBegin = this.yIgnoredBegin = -1;
				this.xIgnoredSize = this.yIgnoredSize = 0;
			}
            IList<PartInfo> parts = this.makeParts(width, height, centreX, centreY, scale, bmd.Scan0, maxIterations);
            Parallel.ForEach(parts, generatePart);
            newImage.UnlockBits(bmd);
            this.oldScale = scale;
            this.oldCentreX = centreX;
            this.oldCentreY = centreY;
            this.oldImage = newImage;
            return newImage;
        }

		/* Split the image surface into several parts, with each part
		 * containing all the necessary information to render it. 
		 *
		 * TODO:  Make this function respect xIgnoreBegin and co.
		 *
		 * WARNING:  This method is under (re-)construction!
		 *
		 * Parameters:
		 *   width   - width of the image
		 *   height  - height of the image
		 *   centreX - x coordinate of the point in the centre of the image
		 *   centreY - y coordinate of the point in the centre of the image
		 *   scale   - difference between coordinates of pixels
		 *   begin   - pointer to first pixel of image
		 *   maxIterations - Maximum number of iterations to try for
		 */
        IList<PartInfo> makeParts(int width, int height, double centreX, double centreY, double scale, IntPtr begin, int maxIterations)
        {
			// If possible, pieces will be of this size.
			// TODO:  If almost all pixels are distributed, slightly more than
			// this should be given to a part.
            const int preferredNumberOfPixels = 50000;

			// List of parts that will be returned.
            IList<PartInfo> parts = new List<PartInfo>();

			// Number of pixels that still need to be distributed.
            int freePixels = width * height;

			// Line up to which all pixels have been distributed.
            int currentLine = 0;
            while (freePixels != 0)
            {
                PartInfo info = new PartInfo(width, scale, maxIterations);
                if (currentLine < this.yIgnoredBegin && currentLine >= (this.yIgnoredBegin + this.yIgnoredSize))
                {   // If currentLine is NOT in ignored area
                    info.partWidth = width;
                    info.startX = centreX - (width / 2) * scale;
                    info.startY = centreY + (height / 2 - currentLine) * scale;
                    unsafe
                    {
                        Int32* rawData = (Int32*)begin.ToPointer();
                        rawData += width * currentLine;
                        info.pToTopLeft = new IntPtr(rawData);
                    }
                    if (preferredNumberOfPixels < freePixels)
                    {
                        int lines = preferredNumberOfPixels / width;
                        freePixels -= lines * width;
                        currentLine += lines;
                        info.partHeight = lines;
                    }
                    else
                    {
                        freePixels = 0;
                        info.partHeight = height - currentLine;
                        currentLine = height;
                    }
                }
                parts.Add(info);
            }
            return parts;
        }

		// Given information on the part to be generated, generate it.
        unsafe void generatePart(PartInfo info)
        {
            Int32* currentPixel = (Int32*)info.pToTopLeft.ToPointer();
            double currentX = info.startX;
            double currentY = info.startY;
            int skippedSpace = info.imageWidth - info.partWidth;
            for (int row = 0; row < info.partHeight; ++row)
            {
                for (int col = 0; col < info.partWidth; ++col)
                {
                    ConvergenceCheckResult res = this.checkConvergence(info.maxIterations, currentX, currentY);
                    *currentPixel = this.getColour(res);
                    currentPixel += 1;
                    currentX += info.scale;
                }
                currentPixel += skippedSpace;
                currentX = info.startX;
                currentY -= info.scale;
            }
        }

		// Move the existing image xShift to the right and yShift up.
		//
		// Note:  rewriting this to be xShift to the right and yShift down
		// would make it easier.
        void moveImage(BitmapData newImageData, int xShift, int yShift)
        {
            int xSize = oldImage.Width - Math.Abs(xShift);
            int ySize = oldImage.Height - Math.Abs(yShift);
            int xSourceBegin = xShift < 0 ? 0 : xShift;
            int xDestBegin = xShift < 0 ? -xShift : 0;
            int ySourceBegin = yShift < 0 ? 0 : yShift;
            int yDestBegin = yShift > 0 ? 0 : -yShift;
            int width = this.oldImage.Width;
            int height = this.oldImage.Height;

            BitmapData oldImageData = this.oldImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                oldImage.PixelFormat
            );

			int toSkip = width - xSize;
            
            unsafe {
                Int32* src = (Int32*)oldImageData.Scan0.ToPointer();
                src += xSourceBegin + ySourceBegin * width;
                Int32* dest = (Int32*)newImageData.Scan0.ToPointer();
                dest += xDestBegin + yDestBegin * width;
                for (int i = 0; i < ySize; ++i)
                {
					Int32* end = dest + xSize;
					while (dest < end)
						*dest++ = *src++;
                    src += toSkip;
                    dest += toSkip;
                }
            }
            this.oldImage.UnlockBits(oldImageData);
        }
    }
}
