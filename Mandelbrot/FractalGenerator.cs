using System;
using System.Collections.Generic;
using System.Drawing;
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
         * - Data types
         *
         *   When generating the image, there are two kinds of values: those
         *   of the Image (the width, height, etc) and those of the fractalGenerator
         *   (coordinates, etc).  In order to avoid mistakes, the following
         *   convention should be used:
         *
         *   Unless mentioned otherwise:
         *    p - All values that refer to the image, and are therefore a
         *        count of pixels, should be prefixed with a p.
         *    r - All values that refer to the fractalGenerator, and are therefore
         *        supposed to be real numbers, should be prefixed with an r.
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
         *   refer to the image or the fractalGenerator, should be prefixed with an
         *   `x', and all values that represent a vertical distance should
         *   be prefixed with a `y'.  This prefix goes after the `p' or 'r'
         *   mentioned above.  A value with an `i' prefix may not also have
         *   an `x' or `y' prefix, as it makes no sense.
         *
         */

        #region member vars
        /// <summary>
        /// A suitably large value that should be returned when a point does
        /// not converge.
        /// </summary>
        protected const int iInfinity = 900001;

        ///<summary>
        ///Information used to generate the previous image.
        ///</summary>
        private ImageInfo oldInfo;
        
        /// <summary>
        /// Constant for specifying a non-existant ignored area.
        /// </summary>
        private static readonly Rectangle AbsentIgnoredArea = new Rectangle(0, 0, 0, 0);

        /// <summary>
        /// The last image fully generated.
        /// </summary>
        private Bitmap lastImage;

        /// <summary>
        /// Ensures nobody ever uses this class from two threads at once.
        /// 
        /// *NOBODY*
        /// </summary>
        private Semaphore busy;
        #endregion

        #region constructors
        /// <summary>
        /// Initialise the fractalGenerator generator.  Ensure that there's no way that
        /// it may seem like copying from the previous image would be a good
        /// idea (there is no previous image).
        /// </summary>
        public FractalGenerator()
        {
            this.oldInfo = new ImageInfo();
            this.busy = new Semaphore(1, 1);
        }
        #endregion

        #region abstract functions
        /// <summary>
        /// Check whether point escapes in no more than iMax iterations.  Return
        /// the coordinates reached when it escapes, and the number of iteration
        /// taken to do so.  If the point does not escape, return iInfinity and
        /// the coordinates at the last iteration performed.
        /// </summary>
        protected abstract ConvergenceCheckResult checkConvergence(double rxPoint, double ryPoint, int iMax);
        #endregion

        #region public methods
        /// <summary>
        /// Generate and return an image that corresponds to the passed info.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when the no reasonable image can be generated.  Possible reasons for this are:
        ///  - The scale is too small.
        /// </exception>
        public Image generate(ImageInfo info)
        {
            if (!this.busy.WaitOne(0))
                throw new Exception("Ridiculous internal error.");
            Bitmap newImage = new Bitmap(info.pxSize, info.pySize, PixelFormat.Format32bppRgb);
            BitmapData bmd = newImage.LockBits(
                new Rectangle(0, 0, info.pxSize, info.pySize),
                ImageLockMode.WriteOnly,
                newImage.PixelFormat
            );
            try
            {
                if (info.rScale < 1E-15) // Precision error
                    throw new Exception("Limiet schaal overschreden");

                if (info.iMax < 1)
                    throw new Exception("Aantal iteraties moet positief zijn");
            
                IList<PartInfo> parts;
            
                if (ImageCombination.CombinationPossible(this.oldInfo, info))
                {
                    ImageCombination combination = new ImageCombination(oldInfo, info); // Hm, I'd usually use var here.
                    this.moveImage(bmd, combination);
                    parts = this.makePartsFromWhole(info, combination.OverlapInSecond, bmd.Scan0);
                }
                else
                {
                    parts = this.makePartsFromWhole(info, FractalGenerator.AbsentIgnoredArea, bmd.Scan0);
                }
                Parallel.ForEach(parts, generatePart);
            }
            finally
            {
                newImage.UnlockBits(bmd);
                this.oldInfo = info;
                this.lastImage = newImage;
                this.busy.Release();
            }
            return newImage;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Split the image surface into several parts, with each part
        /// containing all the necessary information to render it. 
        /// </summary>
        ///
        /// <param name="wholeImage">Information about the entire image.</param>
        /// <param name="ignoredArea">Rectangle of area that should not be generated.</param>
        /// <param name="begin">Pointer to first pixel of image (32bppRGB).</param>
        /// <returns>Information on the parts the image has been split into.</returns>
        IList<PartInfo> makePartsFromWhole(ImageInfo wholeImage, Rectangle ignoredArea, IntPtr begin)
        {
            // List of parts that will be returned.
            List<PartInfo> parts = new List<PartInfo>();
            /* Places that need to be allocated:
             * 
             * +------+
             * |000000|
             * |11xxx2|
             * |11xxx2|
             * |333333|
             * +------+
             * 
             * The four rectangles are placed in List<Rectangle> sections.
             */
            IList<Rectangle> sections = new List<Rectangle>();
            sections.Add(new Rectangle(0, 0, wholeImage.pxSize, ignoredArea.Y));
            sections.Add(new Rectangle(0, ignoredArea.Y, ignoredArea.X, ignoredArea.Height));
            sections.Add(new Rectangle(ignoredArea.X + ignoredArea.Width, ignoredArea.Y, wholeImage.pxSize - ignoredArea.X - ignoredArea.Width, ignoredArea.Height));
            sections.Add(new Rectangle(0, ignoredArea.Y + ignoredArea.Height, wholeImage.pxSize, wholeImage.pySize - ignoredArea.Y - ignoredArea.Height));
            foreach (Rectangle rect in sections)
                parts.AddRange(makePartsFromSection(wholeImage, rect, begin));
            return parts;
        }

        /// <summary>
        /// Split a section of an image into parts, each of which can be rendered separately.
        /// </summary>
        /// <remarks>
        /// This function should be called by <see cref="makePartsFromWhole"/>, don't call
        /// from elsewhere.
        /// </remarks>
        /// 
        /// <param name="wholeImage">Information about the whole image.</param>
        /// <param name="workArea">Area to be partitioned.</param>
        /// <param name="begin">Pointer to the first pixel of the image (32bppRGB)</param>
        /// <returns>Information on the parts that the image has been split into.</returns>
        private IList<PartInfo> makePartsFromSection(ImageInfo wholeImage, Rectangle workArea, IntPtr begin)
        {
            List<PartInfo> parts = new List<PartInfo>();

            // If possible, pieces will be of this size.
            // TODO:  If almost all pixels are distributed, slightly more than
            // this should be given to a part.
            // Note:  The p2 prefix stands for `pixel squared', and is the result
            // of the product of two pixel counts (px * py, to be specific).
            const int p2PreferredBlockSize = 50000;

            // Number of pixels that still need to be distributed.
            int p2Free = workArea.Width * workArea.Height;

            // Line up to which all pixels have been distributed, starting from
            // the area currently being partitioned.
            int pyCurrentPosition = 0;
            while (p2Free != 0)
            {
                PartInfo nextPart = new PartInfo(wholeImage.pxSize, wholeImage.rScale, wholeImage.iMax, wholeImage.palette);
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

        /// <summary>
        /// Given information on a part of the image, draw that part.
        /// </summary>
        /// <param name="info">Information on a part of the image.</param>
        private unsafe void generatePart(PartInfo info)
        {
            Int32* currentPixel = (Int32*)info.imageData.ToPointer();
            double rxCurrent = info.rxStart;
            double ryCurrent = info.ryStart;
            int skippedSpace = info.pxImageSize - info.pxPartSize;
            // Loop through the pixels and set their colours
            for (int pyCounter = 0; pyCounter < info.pyPartSize; ++pyCounter)
            {
                for (int pxCounter = 0; pxCounter < info.pxPartSize; ++pxCounter)
                {
                    ConvergenceCheckResult res = this.checkConvergence(rxCurrent, ryCurrent, info.iMax);
                    *currentPixel = this.getColour(res, info.palette);
                    currentPixel += 1;
                    rxCurrent += info.scale;
                }
                currentPixel += skippedSpace;  // Necessary to get to the beginning of the next line
                rxCurrent = info.rxStart;
                ryCurrent -= info.scale;
            }
        }

        /// <summary>
        /// Copy a piece of the old image over to the new image, moving it as requested by
        /// the ImageCombination.
        /// </summary>
        /// <param name="newImageData">Data of the image currently being generated.</param>
        /// <param name="combination">A combination of the old image and new image.</param>
        private void moveImage(BitmapData newImageData, ImageCombination combination)
        {
            int pxOldSize = this.lastImage.Width;
            int pyOldSize = this.lastImage.Height;
            int pxNewSize = newImageData.Width;
            int pyNewSize = newImageData.Height;

            BitmapData oldImageData = this.lastImage.LockBits(
                new Rectangle(0, 0, pxOldSize, pyOldSize),
                ImageLockMode.ReadOnly,
                lastImage.PixelFormat
            );

            int pxToSkipOld = pxOldSize - combination.pxSize;
            int pxToSkipNew = pxNewSize - combination.pxSize;
            
            unsafe {
                Int32* src = (Int32*)oldImageData.Scan0.ToPointer();
                src += combination.pxBeginInSource + combination.pyBeginInSource * pxOldSize;
                Int32* dest = (Int32*)newImageData.Scan0.ToPointer();
                dest += combination.pxBeginInDestination + combination.pyBeginInDestination * pxNewSize;
                for (int pyCounter = 0; pyCounter < combination.pySize; ++pyCounter)
                {
                    Int32* end = dest + combination.pxSize;
                    while (dest < end)
                        *dest++ = *src++;
                    src += pxToSkipOld;
                    dest += pxToSkipNew;
                }
            }
            this.lastImage.UnlockBits(oldImageData);
        }

        /// <summary>
        /// Given the coordinates of a point and the number of iterations
        /// necessary to reach it, return the colour that that pixel should
        /// be given.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        protected Int32 getColour(ConvergenceCheckResult res, ColourPalette palette)
        {
            if (res.iCount == iInfinity)
                return 0; // black

            double v = res.iCount - Math.Log(0.5 * Math.Log(res.rxPoint * res.rxPoint + res.ryPoint * res.ryPoint, 1E100), 2);
            int colourInt1 = (int)v & 0x1FF;
            return palette[colourInt1];
        }
        #endregion
    }
}
