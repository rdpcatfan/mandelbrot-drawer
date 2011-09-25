using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Mandelbrot
{
    public partial class MainWindow : Form
    {
        #region membervars

        /* In order to split presentation and implementation, the logic for
         * general fractal generation has been moved to the FractalGenerator
         * abstract class and the logic for the Mandelbrot Set in particular
         * has been moved to the MandelbrotGenerator class.
         *
         * Due to FractalGenerator providing a fairly specific implementation
         * of how work is divided and pixels generated, it is likely that
         * there will eventually be entirely different fractal generators. In
         * order to allow for this, the the IFractalGenerator interface was
         * introduced, which gives more implementation freedom than the
         * abstract class;  this allows, amongst other things, the Buddhabrot
         * to be generated, which the current FractalGenerator would not be
         * capable of doing.
         */
        IFractalGenerator fractal;

        #endregion

        #region constructors
        
        /* Construct the window, generate and render the first instance of
         * the fractal with some sane defaults.
         */
        public MainWindow()
        {
            InitializeComponent();
            /* Seeing as FractalGenerator is partially fractal-agnostic, it
             * would be nice if the Collatz Fractal or Julia Set could also
             * be generated.
             */
            this.fractal = new MandelbrotGenerator();
            this.generateFractal();
        }

        #endregion

        /* Generate and render the fractal.
         *
         * Note:  At the moment, this also times the process and displays
         * the time taken.  Whether this is good design is questionable,
         * but it is not significant enough of an issue to redesign.
         */
        private void generateFractal()
        {
            DateTime start = DateTime.Now;  // Poor man's timer
            this.mandelImage.Image = fractal.generate(
                500, // Image width, should be made a variable.
                500, // Image height, should be made a variable.
                this.centreXTextBox.Double,
                this.centreYTextBox.Double,
                this.scaleTextBox.Double,
                Int32.Parse(maxIterationsTextBox.Text)
            );
            // Note that the timer has a resolution of around 15 ms -- this is
            // good enough for testing, but it's probably best to get something
            // more accurate for displaying it to the user.
            //
            // Also, the precision of timerLabel varies rather greatly, there
            // should be some way to nail it to two decimals, or something
            // around that.
            DateTime elapsed = DateTime.Now - start;
            timerLabel.Text = elapsed.TotalMilliseconds.ToString();
        }

        #region mouse functions

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

        /* Event to be called on a single click on the image.
         *
         * Renaming this method is probably a good idea, and perhaps even
         * splitting the business logic off entirely -- we'll need to
         * change the centre to certain X and Y coordinates for other events
         * too, right?
         */
        private void setImageCentre(object sender, MouseEventArgs e)
        {
            /* As accessing this.scaleTextBox.Double is a fairly expensive
             * operation, we'll save the value.
             */
            double scale = this.scaleTextBox.Double;
            /* The following two lines calculate the distance between the old
             * centre and the new centre on the screen, and then convert that
             * to the difference in the value.
             *
             * Note that two 250 constants should become variables at some
             * point:  they are the width and height of the image divided by
             * two.
             *
             * Also note the difference in the calculation of Y:  the rational
             * can be found in FractalGenerator.cs.
             */
            centreXTextBox.Double += (e.X - 250) * scale;
            centreYTextBox.Double -= (e.Y - 250) * scale;
            this.generateFractal();
        }

        /* Ensures that the image always has focus if the mouse is above it
         * (or was it `not above something else'?).  This is done so that
         * the scroll functionality always works, as a control must have
         * focus before it can respond to mouse wheel events.
         */
        private void setImageFocus(object sender, EventArgs e)
        {
            this.mandelImage.Focus();
        }

        /* Modify the image maginification.  Used in response to the
         * mousewheel event, and magnifies in different proportions
         * depending on how for the user scrolls.
         */
        private void setImageZoom(object sender, MouseEventArgs e)
        {
            int zoom = e.Delta;  // Distance scrolled, in steps of 120.
            if (zoom > 0)  // Scroll in, thus decrease the scale.
                scaleTextBox.Double /= (1 + zoom / 120.0);
            else  // Scroll out, thus increase the scale.
                scaleTextBox.Double *= (1 - zoom / 120.0);
            this.generateFractal();
        }
        #endregion
    }
}
