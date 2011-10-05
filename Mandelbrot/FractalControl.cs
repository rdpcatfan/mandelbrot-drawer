using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace Mandelbrot
{
    /// <summary>
    /// Control for manipulating and displaying the image of the Mandelbrot
    /// Set.
    /// </summary>
    class FractalControl : UserControl
    {
        #region member variables
        private InputSection input;
        private Button generateImageButton;
        private PictureBox mandelImageContainer;

        /// <summary>
        /// Keeps track of a button.  May be used by form classes to make the
        /// user interface more convenient.
        /// </summary>
        public IButtonControl AcceptButton { get; private set; }

        private const int pyButtonSize = 25;
        private const int pyInternalPadding = 5;

        /// <summary>
        /// In order to split presentation and implementation, the logic for
        /// general fractalGenerator generation has been moved to the FractalGenerator
        /// abstract class and the logic for the Mandelbrot Set in particular
        /// has been moved to the MandelbrotGenerator class.
        ///
        /// Due to FractalGenerator providing a fairly specific implementation
        /// of how work is divided and pixels generated, it is likely that
        /// there will eventually be entirely different fractalGenerator generators. In
        /// order to allow for this, the the IFractalGenerator interface was
        /// introduced, which gives more implementation freedom than the
        /// abstract class;  this allows, amongst other things, the Buddhabrot
        /// to be generated, which the current FractalGenerator would not be
        /// capable of doing.
        /// </summary>
        IFractalGenerator fractalGenerator;

        /// <summary>
        /// Holds the current mouse position in an image drag operation.
        /// </summary>
        Point mouseDownPosition, dragPosition;

        /// <summary>
        /// Locks when the image is being generated
        /// </summary>
        private Semaphore generating;

        /// <summary>
        /// Locks when the image sizes are being accessed.
        /// </summary>
        private Semaphore imageSizeLock;
        #endregion

        #region properties
        /// <summary>
        /// Horizontal size of the image.
        /// </summary>
        public int pxImage
        {
            get
            {
                this.imageSizeLock.WaitOne();
                int temp = mandelImageContainer.Image.Width;
                this.imageSizeLock.Release();
                return temp;
            }
        }

        /// <summary>
        /// Vertical size of the image.
        /// </summary>
        public int pyImage
        {
            get
            {
                this.imageSizeLock.WaitOne();
                int temp = mandelImageContainer.Image.Width;
                this.imageSizeLock.Release();
                return temp;
            }
        }

        /// <summary>
        /// Duration it took to generate the last image.
        /// </summary>
        public TimeSpan GenerationTime { get; private set; }

        /// <summary>
        /// The mandelbrot image itself.
        /// </summary>
        public Image Image
        {
            get
            {
                return mandelImageContainer.Image;
            }
            private set
            {
                mandelImageContainer.Image = value;
            }
        }
        #endregion

        #region events
        public event EventHandler FinishedDrawing;
        #endregion

        #region constructors
        /// <summary>
        /// Construct and initialise a fractal control.
        /// </summary>
        public FractalControl()
        {
            this.generating = new Semaphore(1, 1);
            this.imageSizeLock = new Semaphore(1, 1);
            this.fractalGenerator = new MandelbrotGenerator();
            this.InitializeComponents();
        }

        /// <summary>
        /// Initialize the graphical user interface.
        /// </summary>
        /// <remarks>
        /// Listed as a constructor because it should only ever be called when constructing.
        /// </remarks>
        private void InitializeComponents()
        {
            this.input = new InputSection();
            this.generateImageButton = new Button();
            this.mandelImageContainer = new PictureBox();

            // input
            this.input.Location = new Point(0, 0);
            this.input.Size = this.input.recommendedSize(this.ClientSize.Width);

            // generateImageButton
            this.generateImageButton.Text = "Start";
            this.generateImageButton.Location = new Point(0, this.input.Size.Height + pyInternalPadding);
            this.generateImageButton.Size = new Size(this.Size.Width, pyButtonSize);
            this.generateImageButton.Click += this.requestGenerateFractal;
            this.AcceptButton = this.generateImageButton;

            // mandelImageContainer
            this.mandelImageContainer.Location = new Point(0, this.generateImageButton.Location.Y + pyButtonSize + pyInternalPadding);
            this.mandelImageContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mandelImageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mandelImageContainer.MouseDown += this.dragImageStart;
            this.mandelImageContainer.MouseEnter += this.setImageFocus;
            this.mandelImageContainer.MouseMove += this.dragImage;
            this.mandelImageContainer.MouseUp += this.dragImageEnd;
            this.mandelImageContainer.MouseWheel += this.setImageZoom;
            this.mandelImageContainer.Image = new Bitmap(1, 1); // Temporary image to avoid race condition.

            this.input.Invalidated += this.requestGenerateFractal;
            this.Resize += this.sizeChanged;

            this.Controls.Add(this.input);
            this.Controls.Add(this.generateImageButton);
            this.Controls.Add(this.mandelImageContainer);
            this.MinimumSize = new Size(this.input.MinimumWidth, this.input.MinimumHeight + pyButtonSize + 2 * pyInternalPadding + 100);
        }
        #endregion


        private void generateFractal()
        {
            DateTime start = DateTime.Now;  // Poor man's timer
            try
            {
                this.mandelImageContainer.Image = fractalGenerator.generate(
                    new ImageInfo(this.mandelImageContainer.Width,
                    this.mandelImageContainer.Height,
                    this.input.rxCentre,
                    this.input.ryCentre,
                    this.input.rScale,
                    this.input.iMax,
                    this.input.CurrentPalette)
                );
                if (FinishedDrawing != null)
                    FinishedDrawing(this, EventArgs.Empty);
            }
            catch (Exception exc)
            {
                ImageAttributes imageAttributes = new ImageAttributes();
                Image image = this.mandelImageContainer.Image;
                Graphics gr = this.mandelImageContainer.CreateGraphics();

                float[][] colorMatrixElements = {   new float[]{0.1f, 0.1f, 0.1f,  0,  0},
                                                    new float[]{0.1f, 0.1f, 0.1f,  0,  0},
                                                    new float[]{0.1f, 0.1f, 0.1f,  0,  0},
                                                    new float[]{  0 ,   0 ,   0 ,  1,  0},
                                                    new float[]{  0 ,   0 ,   0 ,  0,  1}
                                                };
                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                Rectangle containerSize = this.mandelImageContainer.ClientRectangle;
                Rectangle imageSize = new Rectangle(new Point((containerSize.Width - image.Width) / 2, (containerSize.Height - image.Height) / 2), image.Size);

                gr.DrawImage(image,
                   imageSize,
                   0, 0,
                   imageSize.Width,
                   imageSize.Height,
                   GraphicsUnit.Pixel,
                   imageAttributes);

                string errorText = "Er is een probleem opgetreden:\r\n\r\n" + exc.Message;
                gr.DrawString(errorText, new Font("Arial", 16), Brushes.White, containerSize.Width / 2 - 160, containerSize.Height / 2 - 40);
            }
            finally
            {
                // Note that the timer has a resolution of around 15 ms -- this is
                // good enough for testing, but it's probably best to get something
                // more accurate for displaying it to the user.
                //
                // Also, the precision of timerLabel varies rather greatly, there
                // should be some way to nail it to two decimals, or something
                // around that.
                this.GenerationTime = DateTime.Now - start;
            }
        }

        #region event handlers
        /// <summary>
        /// Generate and render the fractalGenerator.
        /// </summary>
        /// <remarks>
        /// At the moment, this also times the process and displays
        /// the time taken.  Whether this is good design is questionable,
        /// but it is not significant enough of an issue to redesign.
        /// </remarks>
        private void requestGenerateFractal(object o = null, EventArgs e = null)
        {
            if (!generating.WaitOne(0))
                return; // Don't do anything if we're already busy.
            try
            {
                Thread worker = new Thread(new ThreadStart(generateFractal));
                worker.Start();
            }
            finally
            {
                generating.Release();
            }
        }

        /// <summary>
        /// Return the image to the original position.
        /// </summary>
        /// <remarks>
        /// Does not reset the colour scheme.
        /// </remarks>
        public void resetImage(object sender, EventArgs e)
        {
            this.input.rxCentre = 0.0;
            this.input.ryCentre = 0.0;
            this.input.rScale = 0.01;
            this.input.iMax = 500;
            this.requestGenerateFractal();
        }

        /// <summary>
        /// Ensure that the image always has focus if the mouse is above it.
        /// This must be done for the scroll functionality to be available, as a
        /// control must have focus before it can respond to mouse wheel events.
        /// </summary>
        private void setImageFocus(object sender, EventArgs e)
        {
            if (!generating.WaitOne(0)) // Don't want to interfere.
                return;
            try
            {
                this.mandelImageContainer.Focus();
            }
            finally
            {
                generating.Release();
            }
        }

        /// <summary>
        /// Modify the image magnification.  Triggered in response to the
        /// mousewheel event.  The change in magnification increases as the
        /// user scrolls further.
        /// </summary>
        private void setImageZoom(object sender, MouseEventArgs e)
        {
            int zoom = e.Delta;  // Distance scrolled, in steps of 120.
            if (zoom > 0)  // Scroll in, thus decrease the rScale.
                this.input.rScale /= (1 + zoom / 120.0);
            else  // Scroll out, thus increase the rScale.
                this.input.rScale *= (1 - zoom / 120.0);
            this.requestGenerateFractal();
        }

        /// <summary>
        /// Sets variables based on the position of the MouseDownEvent, 
        /// which will be used in dragImage and dragImageEnd.
        /// </summary>
        private void dragImageStart(object sender, MouseEventArgs e)
        {
            mouseDownPosition = new Point(e.X, e.Y);
            dragPosition = new Point(e.X, e.Y);
        }

        /// <summary>
        /// Repositions the image relative to its current position when 
        /// the image is dragged. 
        /// </summary>
        private void dragImage(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double rScale = this.input.rScale;
                this.input.rxCentre += (dragPosition.X - e.X) * rScale;
                this.input.ryCentre -= (dragPosition.Y - e.Y) * rScale;
                dragPosition = new Point(e.X, e.Y);
                this.requestGenerateFractal();
            }
        }

        /// <summary>
        /// If the image was not moved through ImageDrag, the image will be centred 
        /// on the position of the mouse click.
        /// </summary>
        private void dragImageEnd(object sender, MouseEventArgs e)
        {
            Point mouseUpPosition = new Point(e.X, e.Y);
            if (mouseDownPosition == mouseUpPosition)
            {
                /* As accessing this.scaleTextBox.Double is a fairly expensive
                 * operation, we'll save the value.
                 */
                double rScale = this.input.rScale;
                /* The following two lines calculate the distance between the old
                 * centre and the new centre on the screen, and then convert that
                 * to the difference in the value.
                 *
                 * Note the difference in the calculation of Y:  the rational
                 * can be found in FractalGenerator.cs.
                 */
                this.input.rxCentre += (e.X - mandelImageContainer.Size.Width / 2) * rScale;
                this.input.ryCentre -= (e.Y - mandelImageContainer.Size.Height / 2) * rScale;
                this.requestGenerateFractal();
            }
        }

        /// <summary>
        /// Resizes the mandelImageContainer, while preserving the distance to the border of the form.
        /// Updates the image to adjust to the new size of the image container.
        /// </summary>
        private void sizeChanged(object sender, EventArgs e)
        {
            this.input.Size = this.input.recommendedSize(this.ClientSize.Width);
            this.generateImageButton.Location = new Point(0, this.input.Size.Height + pyInternalPadding);
            this.generateImageButton.Size = new Size(this.ClientSize.Width, pyButtonSize);
            this.mandelImageContainer.Location = new Point(0, this.generateImageButton.Location.Y + pyButtonSize + pyInternalPadding);
            this.mandelImageContainer.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - this.input.Size.Height - pyButtonSize - pyInternalPadding * 2);
            this.requestGenerateFractal();
        }
        #endregion
    }
}
