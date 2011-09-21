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

        IFractalGenerator fractal;

        #endregion

        #region constructors
        
        public MainWindow()
        {
            InitializeComponent();
            this.fractal = new MandelbrotGenerator();
            this.generateFractal();
        }

        #endregion

        private void generateFractal()
        {
            DateTime start = DateTime.Now;
            this.mandelImage.Image = fractal.generate(
                500,
                500,
                this.centreXTextBox.Double,
                this.centreYTextBox.Double,
                this.scaleTextBox.Double,
                Int32.Parse(maxIterationsTextBox.Text)
            );
            timerLabel.Text = (DateTime.Now - start).TotalMilliseconds.ToString();
        }

        #region mouse functions

        private void generateMandelbrotClick(object sender, EventArgs e)
        {
            generateFractal();
        }

        private void setImageCentre(object sender, MouseEventArgs e)
        {
            double scale = this.scaleTextBox.Double;
            centreXTextBox.Double += (e.X - 250) * scale;
            centreYTextBox.Double += (e.Y - 250) * scale;
            this.generateFractal();
        }

        private void setImageFocus(object sender, EventArgs e)
        {
            mandelImage.Focus(); //A control must have focus before it can fire in respons to the mouse wheel.
        }

        private void setImageZoom(object sender, MouseEventArgs e)
        {
            int zoom = e.Delta;
            if (zoom > 0)
                scaleTextBox.Double /= (1 + zoom / 120.0);
            else
                scaleTextBox.Double *= (1 - zoom / 120.0);
            this.generateFractal();
        }
        #endregion
    }
}
