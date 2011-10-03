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
        #region member vars
        /// <summary>
        /// Amount of horizontal padding on the sides of the fractal control.
        /// </summary>
        private const int pxPadding = 15;

        /// <summary>
        /// Used for managing resize operations.
        /// </summary>
        bool resizeBeginTriggered = false;
        #endregion

        #region constructors
        /// <summary>
        /// Construct the window, generate and render the first instance of
        /// the fractalGenerator with some sane defaults.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region events
        /// <summary>
        /// beginResize is fired in response to the ResizeBegin event of the main form.
        /// ResizeBegin fires once when the border of the form is dragged. 
        /// It does not fire when the 'maximize' button of the form is clicked. 
        /// </summary>
        private void beginResize(object sender, EventArgs e)
        {
            resizeBeginTriggered = true;
        }

        /// <summary>
        /// completeResize is fired in response to the Resize event of the main form.
        /// ResizeBegin fires zero or more times when the border of the form is dragged. 
        /// It fires once when the 'maximize' button of the form is clicked.
        /// completeResize updates the current size in the status strip and 
        /// resizes the image when the 'maximize' button is clicked.
        /// </summary>
        private void considerResize(object sender, EventArgs e)
        {
            if (!resizeBeginTriggered)
                this.fractal.Size = calcFractalSize();
        }

        /// <summary>
        /// Fires once when the resize of the form has ended if a ResizeBegin event has been raised.
        /// Resets the flag and resizes the image.
        /// </summary>
        private void completeResize(object sender, EventArgs e)
        {
            resizeBeginTriggered = false;
            this.fractal.Size = calcFractalSize();
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
                        fractal.Image.Save(saveImageDialog.FileName,
                           ImageFormat.Bmp);
                        break;

                    case 2:
                        fractal.Image.Save(saveImageDialog.FileName,
                            ImageFormat.Png);
                        break;

                    case 3:
                        fractal.Image.Save(saveImageDialog.FileName,
                           ImageFormat.Jpeg);
                        break;

                    case 4:
                        fractal.Image.Save(saveImageDialog.FileName,
                           ImageFormat.Gif);
                        break;
                }
            }
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        private void exitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Open the AboutBox dialog.
        /// </summary>
        private void openAboutBox(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        /// <summary>
        /// Open the help form.
        /// </summary>
        private void openHelpForm(object sender, EventArgs e)
        {
            HelpForm help = new HelpForm();
            help.Show();
        }

        /// <summary>
        /// React to a change in the image by updating the status bar.
        /// </summary>
        private void updateStatusBar(object sender, EventArgs e)
        {
            statusStripSizeLabel.Text = "Afmetingen: " + this.fractal.Image.Width + " x " + this.fractal.Image.Height;
            this.statusStripTimeLabel.Text = "Gegenereerd in: " + this.fractal.GenerationTime.TotalMilliseconds.ToString("0") + " ms";
        }
        #endregion

        #region private methods
        /// <summary>
        /// Calculate the size necessary for the fractal control.
        /// </summary>
        private Size calcFractalSize()
        {
            return new Size(this.ClientSize.Width - pxPadding * 2, this.ClientSize.Height - 60);
        }
        #endregion
    }
}
