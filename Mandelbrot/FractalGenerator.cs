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
        
        // Temporary variable for storing the Pallete.  Should probably be
        // replaced, so that the pallete can be passed in as a parameter.
        protected ColourPalette colourPalette;

        private ImageInfo oldInfo;

        /* If a piece of the old image has been copied, the value of the
         * beginning of this piece in the new image is stored here and the
         * size of the piece is stored here. Otherwise, this is equal to
         * AbsentIgnoredArea.
         */
        private Rectangle ignoredArea;

        // Constant for specifying a non-existant ignored area.
        private static Rectangle AbsentIgnoredArea = new Rectangle(-1, -1, 0, 0);

        // Stores the previous image.
        private Bitmap oldImage;
        #endregion

        #region constructors

        /* Initialise the fractal generator.  Ensure that there's no way that
         * it may seem like copying from the previous image would be a good
         * idea (there is no previous image).
         */
        public FractalGenerator()
        {
            this.oldInfo = new ImageInfo();
            this.ignoredArea = new Rectangle(-1, -1, 0, 0);
            //this.colourPalette = new ColourPalette(Color.Red, Color.FromArgb(0, 0xFF, 0), Color.Blue, Color.Black);
            //this.colourPalette = new ColourPalette(Color.DeepSkyBlue, Color.GhostWhite, Color.Crimson, Color.ForestGreen);
            //this.colourPalette = new ColourPalette(Color.MidnightBlue, Color.ForestGreen, Color.Firebrick, Color.SandyBrown);
            this.colourPalette = new ColourPalette(Color.White, Color.Red, Color.Green, Color.Blue);
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


        #endregion

        #region public functions

        /* Generate and return an image that corresponds to the passed-in info.
         * Test for escape up to iMax iterations.
         */
        public Image generate(
            ImageInfo info,
            int iMax
        )
        {
            Bitmap newImage = new Bitmap(info.pxSize, info.pySize, PixelFormat.Format32bppRgb);
            BitmapData bmd = newImage.LockBits(
                new Rectangle(0, 0, info.pxSize, info.pySize),
                ImageLockMode.WriteOnly,
                newImage.PixelFormat
            );

            if (info.rxCentre + info.rScale == info.rxCentre) // Precision error
                throw new Exception("Precision limit exceeded.");

            if (this.oldInfo.rScale == info.rScale)
                this.moveImage(bmd, info.pOffset(this.oldInfo)); // This sets the ignored area.
            else
                this.ignoredArea = new Rectangle(-1, -1, 0, 0);

            IList<PartInfo> parts = this.makePartsFromWhole(info, bmd.Scan0, iMax);
            Parallel.ForEach(parts, generatePart);
            newImage.UnlockBits(bmd);
            this.oldInfo = info;
            this.oldImage = newImage;
            return newImage;
        }

        #endregion

        #region private functions

        /* Split the image surface into several parts, with each part
         * containing all the necessary information to render it. 
         *
         * Parameters:
         *   wholeImage - information about the entire image
         *   begin      - pointer to first pixel of image
         *   iMax       - maximum number of iterations to try for
         */
        IList<PartInfo> makePartsFromWhole(ImageInfo wholeImage, IntPtr begin, int iMax)
        {
            // List of parts that will be returned.
            List<PartInfo> parts = new List<PartInfo>();
            parts.AddRange(makePartsFromSection(wholeImage, new Rectangle(0, 0, wholeImage.pxCentre, wholeImage.pySize), begin, iMax));
            parts.AddRange(makePartsFromSection(wholeImage, new Rectangle(wholeImage.pxCentre, 0, wholeImage.pxCentre, wholeImage.pySize), begin, iMax));
            return parts;
        }

        private List<PartInfo> makePartsFromSection(ImageInfo wholeImage, Rectangle workArea, IntPtr begin, int iMax)
        {
            List<PartInfo> parts = new List<PartInfo>();

            // If possible, pieces will be of this size.
            // TODO:  If almost all pixels are distributed, slightly more than
            // this should be given to a part.
            // Note:  The p2 prefix stands for `pixel squared', and is the result
            // of the product of two pixel counts.
            const int p2PreferredBlockSize = 50000;

            // Number of pixels that still need to be distributed.
            int p2Free = workArea.Width * workArea.Height;

            // Line up to which all pixels have been distributed, starting from
            // the area currently being partitioned.
            int pyCurrentPosition = 0;
            while (p2Free != 0)
            {
                PartInfo nextPart = new PartInfo(wholeImage.pxSize, wholeImage.rScale, iMax);
                nextPart.pxPartSize = workArea.Width;
                nextPart.rxStart = wholeImage.rxValue(workArea.X);
                nextPart.ryStart = wholeImage.ryValue(workArea.Y + pyCurrentPosition);
                unsafe
                {
                    Int32* rawData = (Int32*)begin.ToPointer();
                    rawData += wholeImage.pxSize * (workArea.Y + pyCurrentPosition) + workArea.X;
                    nextPart.imageData = new IntPtr(rawData);
                }
                if (p2PreferredBlockSize < p2Free) // Lots of free space left
                {
                    // Figure out how many lines make up p2PreferredBlockSize and 
                    // grab that much, making sure to note that you took it.
                    int pyClaimed = p2PreferredBlockSize / workArea.Width;
                    p2Free -= pyClaimed * workArea.Width;
                    pyCurrentPosition += pyClaimed;
                    nextPart.pyPartSize = pyClaimed;
                }
                else // Little free space left
                {
                    // Just take it all.
                    p2Free = 0;
                    nextPart.pyPartSize = workArea.Height - pyCurrentPosition;
                    pyCurrentPosition = wholeImage.pySize; // Just in case the algorithm changes later.
                }
                parts.Add(nextPart);
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

        // Move the existing image by pShift pixels.
        private void moveImage(BitmapData newImageData, Size pShift)
        {
            int pxNewSize = oldImage.Width - Math.Abs(pShift.Width);
            int pyNewSize = oldImage.Height - Math.Abs(pShift.Height);
            // The following few lines are generic code that should be split out.
            int pxSourceBegin = pShift.Width < 0 ? 0 : pShift.Width;
            int pxDestBegin = pShift.Width < 0 ? -pShift.Width : 0;
            int pySourceBegin = pShift.Height < 0 ? 0 : pShift.Height;
            int pyDestBegin = pShift.Height > 0 ? 0 : -pShift.Height;
            int pxOldSize = this.oldImage.Width;
            int pyOldSize = this.oldImage.Height;

            BitmapData oldImageData = this.oldImage.LockBits(
                new Rectangle(0, 0, pxOldSize, pyOldSize),
                ImageLockMode.ReadOnly,
                oldImage.PixelFormat
            );

            int pxToSkip = pxOldSize - pxNewSize;
            
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
                    src += pxToSkip;
                    dest += pxToSkip;
                }
            }
            this.oldImage.UnlockBits(oldImageData);
        }

        /* Given the coordinates of a point and the number of iterations
         * necessary to reach it, return the colour that that pixel should
         * be given.
         *
         * It might be reasonable to make this a static, non-virtual function
         * and use a List for storing the colours instead. -- Anton
         */
        protected Int32 getColour(ConvergenceCheckResult res)
        {
            if (res.iCount == iInfinity)
                return 0; // black

            double v = res.iCount - Math.Log(0.5 * Math.Log(res.rxPoint * res.rxPoint + res.ryPoint * res.ryPoint, 1E100), 2);
            int colourInt1 = (int)v & 0x1FF;
            return colourPalette[colourInt1];
        }

        #endregion
    }
}
