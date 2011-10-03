using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;

namespace Mandelbrot
{
    public partial class MainWindow : Form
    {
        #region member variables

        /// <summary>
        /// In order to split presentation and implementation, the logic for
        /// general fractal generation has been moved to the FractalGenerator
        /// abstract class and the logic for the Mandelbrot Set in particular
        /// has been moved to the MandelbrotGenerator class.
        ///
        /// Due to FractalGenerator providing a fairly specific implementation
        /// of how work is divided and pixels generated, it is likely that
        /// there will eventually be entirely different fractal generators. In
        /// order to allow for this, the the IFractalGenerator interface was
        /// introduced, which gives more implementation freedom than the
        /// abstract class;  this allows, amongst other things, the Buddhabrot
        /// to be generated, which the current FractalGenerator would not be
        /// capable of doing.
        /// </summary>
        IFractalGenerator fractal;

        /// <summary>
        /// Used for managing resize operations.
        /// </summary>
        bool resizeBeginTriggered = false;

        IDictionary<string, ColourPalette> colours;

        /// <summary>
        /// Holds the current mouse position in an image drag operation.
        /// </summary>
        Point mouseDownPosition, dragPosition;

        #endregion

        #region constructors

        /// <summary>
        /// Construct the window, generate and render the first instance of
        /// the fractal with some sane defaults.
        /// </summary>
        public MainWindow()
        {
            this.colours = new Dictionary<string, ColourPalette>();
            this.colours["Default"] = new ColourPalette(Color.White, Color.Red, Color.Green, Color.Blue);
            this.colours["Forest"] = new ColourPalette(Color.MidnightBlue, Color.ForestGreen, Color.FloralWhite, Color.Gray);
            this.colours["Awful"] = new ColourPalette(Color.Chocolate, Color.Lime, Color.PeachPuff, Color.Purple);
            InitializeComponent();
            /* Seeing as FractalGenerator is partially fractal-agnostic, it
             * would be nice if the Collatz Fractal or Julia Set could also
             * be generated.
             */
            this.fractal = new MandelbrotGenerator();
            this.generateFractal();
        }

        #endregion

        /// <summary>
        /// Generate and render the fractal.
        /// </summary>
        /// <remarks>
        /// At the moment, this also times the process and displays
        /// the time taken.  Whether this is good design is questionable,
        /// but it is not significant enough of an issue to redesign.
        /// </remarks>
        private void generateFractal()
        {
            DateTime start = DateTime.Now;  // Poor man's timer
            this.mandelImageContainer.Image = fractal.generate(
                new ImageInfo(this.mandelImageContainer.Width,
                this.mandelImageContainer.Height,
                this.centreXTextBox.Double,
                this.centreYTextBox.Double,
                this.scaleTextBox.Double,
                Int32.Parse(maxIterationsTextBox.Text),
                this.colours["Default"])
            );
            // Note that the timer has a resolution of around 15 ms -- this is
            // good enough for testing, but it's probably best to get something
            // more accurate for displaying it to the user.
            //
            // Also, the precision of timerLabel varies rather greatly, there
            // should be some way to nail it to two decimals, or something
            // around that.
            double elapsed = (DateTime.Now - start).TotalMilliseconds;
            this.statusStripTimeLabel.Text = "Gegenereerd in: " + elapsed.ToString("0") + " ms";
        }

        #region UI interaction functions

        /* Event to be triggered when the `Start' button is pressed.
         *
         * Do we really need this?  It might be better if it was regenerated
         * immediately after any of the settings are changed.  Nice for
         * testing, though, I suppose. -- Anton
         */
        private void generateMandelbrotClick(object sender, EventArgs e)
        {
            generateFractal();
        }


        /* Ensures that the image always has focus if the mouse is above it.
         * This is done so that the scroll functionality always works, 
         * as a control must have focus before it can respond to mouse wheel events.
         */
        private void setImageFocus(object sender, EventArgs e)
        {
            this.mandelImageContainer.Focus();
        }

        /* Modify the image maginification.  Used in response to the
         * mousewheel event, and magnifies in different proportions
         * depending on how for the user scrolls.
         */
        private void setImageZoom(object sender, MouseEventArgs e)
        {
            int zoom = e.Delta;  // Distance scrolled, in steps of 120.
            if (zoom > 0)  // Scroll in, thus decrease the rScale.
                scaleTextBox.Double /= (1 + zoom / 120.0);
            else  // Scroll out, thus increase the rScale.
                scaleTextBox.Double *= (1 - zoom / 120.0);
            this.generateFractal();
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
                double rScale = this.scaleTextBox.Double;
                this.centreXTextBox.Double += (dragPosition.X - e.X) * rScale;
                this.centreYTextBox.Double -= (dragPosition.Y - e.Y) * rScale;
                dragPosition = new Point(e.X, e.Y);
                generateFractal();
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
                double rScale = this.scaleTextBox.Double;
                /* The following two lines calculate the distance between the old
                 * centre and the new centre on the screen, and then convert that
                 * to the difference in the value.
                 *
                 * Note the difference in the calculation of Y:  the rational
                 * can be found in FractalGenerator.cs.
                 */
                centreXTextBox.Double += (e.X - mandelImageContainer.Size.Width / 2) * rScale;
                centreYTextBox.Double -= (e.Y - mandelImageContainer.Size.Height / 2) * rScale;
                this.generateFractal();
            }
        }

        /// <summary>
        /// setResizeFlag is fired in response to the ResizeBegin event of the main form.
        /// ResizeBegin fires once when the border of the form is dragged. 
        /// It does not fire when the 'maximize' button of the form is clicked. 
        /// </summary>
        private void setResizeFlag(object sender, EventArgs e)
        {
            resizeBeginTriggered = true;
        }

        /// <summary>
        /// tryResizeImageContainer is fired in response to the Resize event of the main form.
        /// ResizeBegin fires zero or more times when the border of the form is dragged. 
        /// It fires once when the 'maximize' button of the form is clicked.
        /// tryResizeImageContainer updates the current size in the status strip and 
        /// resizes the image when the 'maximize' button is clicked.
        /// </summary>
        private void tryResizeImageContainer(object sender, EventArgs e)
        {
            statusStripSizeLabel.Text = "Afmetingen: " + this.mandelImageContainer + " x " + this.mandelImageContainer;
            if (!resizeBeginTriggered)
                setImageContainerSize();
        }

        /// <summary>
        /// Fires once when the resize of the form has ended if a ResizeBegin event has been raised.
        /// Resets the flag and resizes the image.
        /// </summary>
        private void resizeImageContainer(object sender, EventArgs e)
        {
            resizeBeginTriggered = false;
            setImageContainerSize();
        }

        /// <summary>
        /// Resizes the mandelImageContainer, while preserving the distance to the border of the form.
        /// Updates the image to adjust to the new size of the image container.
        /// </summary>
        private void setImageContainerSize()
        {
            mandelImageContainer.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 180);
            this.generateFractal();
        }

        /// <summary>
        /// Resets the image to the default starting position, scale and iterations.
        /// </summary>
        private void resetImage(object sender, EventArgs e)
        {
            centreXTextBox.Double = 0.0;
            centreYTextBox.Double = 0.0;
            scaleTextBox.Double = 0.01;
            maxIterationsTextBox.Text = "500";
            generateFractal();
        }

        /// <summary>
        /// Opens a SaveFileDialog, allowing the current image in the mandelImageContainer to be saved as a bitmap, jpeg or gif.
        /// </summary>
        private void saveImage(object sender, EventArgs e)
        {
            saveImageDialog.ShowDialog();
            if (saveImageDialog.FileName != "")
            {
                switch (saveImageDialog.FilterIndex)
                {
                    case 1:
                        mandelImageContainer.Image.Save(saveImageDialog.FileName,
                           System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 2:
                        mandelImageContainer.Image.Save(saveImageDialog.FileName,
                           System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 3:
                        mandelImageContainer.Image.Save(saveImageDialog.FileName,
                           System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
            }
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        private void exitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Opens the AboutBox dialog.
        /// </summary>
        private void openAboutBox(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void openHelpForm(object sender, EventArgs e)
        {
            HelpForm help = new HelpForm();
            help.Show();
        }
        #endregion


    }
}
