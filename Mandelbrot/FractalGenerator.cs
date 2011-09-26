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
        protected const int iInfinity = 9001;

		// An even larger value that may be used for testing purposes.
        protected const int iInfinityPlusOne = 9002;

		// The x coordinate of the centre in the previously generated image.
		// Used to allow the old image to be copied when possible.
        private double rxOldCentre;

		// The y coordinate of the centre in the previously generated image.
		// Used to allow the old image to be copied when possible.
        private double ryOldCentre;

		// The rScale of the previously generated image.  If this is the same
		// in the next image, and the location of the image isn't too far off,
		// a chunk of the old data can be copied.
        private double rOldScale;

		// If a piece of the old image has been copied, the x value of the
		// beginning of this piece in the new image is stored here.
		// Otherwise, this is equal to -1.
        private int pxIgnoredBegin;

		// If a piece of the old image has been copied, the y value of the
		// beginning of this piece in the new image is stored here.
		// Otherwise, this is equal to -1.
        private int pyIgnoredBegin;

		// If a piece of the old image has been copied, the width of the
		// piece is stored here.  Otherwise, this is equal to 0.
        private int pxIgnoredSize;

		// If a piece of the old image has been copied, the height of the
		// piece is stored here.  Otherwise, this is equal to 0.
        private int pyIgnoredSize;

		// Stores the previous image.  Does not 
        private Bitmap oldImage;
        #endregion

        #region constructors

        /* Initialise the fractal generator.  Ensure that there's no way that
         * it may seem like copying from the previous image would be a good
         * idea (there is no previous image).
         */
        public FractalGenerator()
        {
            this.rxOldCentre = this.ryOldCentre = 0;
            // NaN == x is false for all x, so the rScale always seems
            // incorrect.
            this.rOldScale = Double.NaN;
            this.pxIgnoredBegin = this.pyIgnoredBegin = -1;
            this.pxIgnoredSize = this.pyIgnoredSize = 0;
        }

        #endregion

        #region abstract functions

		/* Check whether point (x, y) escapes in no more than iMax
		 * iterations.  Return the coordinates reached when it escapes, and
		 * the number of iteration taken to do so.  If the point does not
		 * escape, return iInfinity and the coordinates at the last iteration
		 * performed.
		 */
        protected abstract ConvergenceCheckResult checkConvergence(double rxPoint, double ryPoint, int maxIterations);

		/* Given the coordinates of a point and the number of iterations
		 * necessary to reach it, return the colour that that pixel should
		 * be given.
		 *
		 * It might be reasonable to make this a static, non-virtual function
		 * and use a List for storing the colours instead. -- Anton
		 */
        protected abstract Int32 getColour(ConvergenceCheckResult res);

        #endregion

        #region public functions

        // Generate and return an image of size pxSize x pySize, centred on
		// point (rxCentre, ryCentre), with the step size between each pixel
		// being rScale.  Test for escape up to iMax iterations.
        public Image generate(
			int pxSize,
			int pySize,
			double rxCentre,
			double ryCentre,
			double rScale,
			int iMax
		)
        {
            Bitmap newImage = new Bitmap(pxSize, pySize, PixelFormat.Format32bppRgb);
            BitmapData bmd = newImage.LockBits(
                new Rectangle(0, 0, pxSize, pySize),
                ImageLockMode.WriteOnly,
                newImage.PixelFormat
            );

            if (this.rOldScale == rScale)
            {
                int pxShift = (int)((rxCentre - this.rxOldCentre) / this.rOldScale);
                int pyShift = (int)((this.ryOldCentre - ryCentre) / this.rOldScale);
                this.moveImage(bmd, pxShift, pyShift);
            }
			else
			{
				this.pxIgnoredBegin = this.pyIgnoredBegin = -1;
				this.pxIgnoredSize = this.pyIgnoredSize = 0;
			}
            IList<PartInfo> parts = this.makeParts(pxSize, pySize, rxCentre, ryCentre, rScale, bmd.Scan0, iMax);
            Parallel.ForEach(parts, generatePart);
            newImage.UnlockBits(bmd);
            this.rOldScale = rScale;
            this.rxOldCentre = rxCentre;
            this.ryOldCentre = ryCentre;
            this.oldImage = newImage;
            return newImage;
        }

        #endregion

        #region private functions

        /* Split the image surface into several parts, with each part
		 * containing all the necessary information to render it. 
		 *
		 * TODO:  Make this function respect xIgnoreBegin and co.
		 *
		 * WARNING:  This method is under (re-)construction!  Currently
         * throws OutOfMemoryException.
         * 
		 *
		 * Parameters:
		 *   pxSize   - width of the image
		 *   pySize  - height of the image
		 *   rxCentre - x coordinate of the point in the centre of the image
		 *   ryCentre - y coordinate of the point in the centre of the image
		 *   rScale   - difference between coordinates of pixels
		 *   begin   - pointer to first pixel of image
		 *   iMax - Maximum number of iterations to try for
		 */
        IList<PartInfo> makeParts(int pxSize, int pySize, double rxCentre, double ryCentre, double rScale, IntPtr begin, int iMax)
        {
			// If possible, pieces will be of this size.
			// TODO:  If almost all pixels are distributed, slightly more than
			// this should be given to a part.
            // Note:  The p2 prefix stands for `pixel squared', and is the result
            // of the product of two pixel counts.
            const int p2PreferredBlockSize = 50000;

			// List of parts that will be returned.
            IList<PartInfo> parts = new List<PartInfo>();

			// Number of pixels that still need to be distributed.
            int p2Free = pxSize * pySize;

			// Line up to which all pixels have been distributed.
            int pyCurrentPosition = 0;
            while (p2Free != 0)
            {
                PartInfo info = new PartInfo(pxSize, rScale, iMax);
                if (pyCurrentPosition < this.pyIgnoredBegin && pyCurrentPosition >= (this.pyIgnoredBegin + this.pyIgnoredSize))
                {   // If pyCurrentPosition is NOT in ignored area
                    info.pxPartSize = pxSize;
                    info.rxStart = rxCentre - (pxSize / 2) * rScale;
                    info.ryStart = ryCentre + (pySize / 2 - pyCurrentPosition) * rScale;
                    unsafe
                    {
                        Int32* rawData = (Int32*)begin.ToPointer();
                        rawData += pxSize * pyCurrentPosition;
                        info.imageData = new IntPtr(rawData);
                    }
                    if (p2PreferredBlockSize < p2Free)
                    {
                        int pyClaimed = p2PreferredBlockSize / pxSize;
                        p2Free -= pyClaimed * pxSize;
                        pyCurrentPosition += pyClaimed;
                        info.pyPartSize = pyClaimed;
                    }
                    else
                    {
                        p2Free = 0;
                        info.pyPartSize = pySize - pyCurrentPosition;
                        pyCurrentPosition = pySize;
                    }
                }
                parts.Add(info);
            }
            return parts;
        }

		// Given information on the part to be generated, generate it.
        private unsafe void generatePart(PartInfo info)
        {
            Int32* currentPixel = (Int32*)info.imageData.ToPointer();
            double rxCurrent = info.rxStart;
            double ryCurrent = info.ryStart;
            int skippedSpace = info.pxImageSize - info.pxPartSize;
            for (int pyCounter = 0; pyCounter < info.pyPartSize; ++pyCounter)
            {
                for (int pxCounter = 0; pxCounter < info.pxPartSize; ++pxCounter)
                {
                    ConvergenceCheckResult res = this.checkConvergence(rxCurrent, ryCurrent, info.iMax);
                    *currentPixel = this.getColour(res);
                    currentPixel += 1;
                    rxCurrent += info.scale;
                }
                currentPixel += skippedSpace;
                rxCurrent = info.rxStart;
                ryCurrent -= info.scale;
            }
        }

		// Move the existing image pxShift to the right and pyShift up.
		//
		// Note:  rewriting this to be pxShift to the right and pyShift down
		// would make it easier.
        private void moveImage(BitmapData newImageData, int xShift, int yShift)
        {
            int pxNewSize = oldImage.Width - Math.Abs(xShift);
            int pyNewSize = oldImage.Height - Math.Abs(yShift);
            int pxSourceBegin = xShift < 0 ? 0 : xShift;
            int pxDestBegin = xShift < 0 ? -xShift : 0;
            int pySourceBegin = yShift < 0 ? 0 : yShift;
            int pyDestBegin = yShift > 0 ? 0 : -yShift;
            int pxOldSize = this.oldImage.Width;
            int pyOldSize = this.oldImage.Height;

            BitmapData oldImageData = this.oldImage.LockBits(
                new Rectangle(0, 0, pxOldSize, pyOldSize),
                ImageLockMode.ReadOnly,
                oldImage.PixelFormat
            );

			int toSkip = pxOldSize - pxNewSize;
            
            unsafe {
                Int32* src = (Int32*)oldImageData.Scan0.ToPointer();
                src += pxSourceBegin + pySourceBegin * pxOldSize;
                Int32* dest = (Int32*)newImageData.Scan0.ToPointer();
                dest += pxDestBegin + pyDestBegin * pxOldSize;
                for (int pyCounter = 0; pyCounter < pyNewSize; ++pyCounter)
                {
					Int32* end = dest + pxNewSize;
					while (dest < end)
						*dest++ = *src++;
                    src += toSkip;
                    dest += toSkip;
                }
            }
            this.oldImage.UnlockBits(oldImageData);
        }

        #endregion
    }
}
