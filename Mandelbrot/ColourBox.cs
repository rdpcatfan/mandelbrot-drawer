using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot
{
    class ColourBox : PictureBox
    {
        #region member variables
        private Color _colour;
        #endregion

        #region properties
        public Color Colour
        {
            get
            {
                return _colour;
            }
            set
            {
                _colour = value;
                this.Invalidate();
            }
        }
        #endregion

        public ColourBox(Color c)
        {
            this._colour = c;
            this.Paint += (object o, PaintEventArgs e) =>
            {
                using (Brush br = new SolidBrush(this.Colour))
                {
                    e.Graphics.FillRectangle(br, this.ClientRectangle);
                }
            };
            this.Click += makeMixer;
        }

        #region event handlers
        private void makeMixer(object o, EventArgs ea)
        {
            ColourMixer mixer = new ColourMixer(this.Colour);
            mixer.ColourChanged += (object p, EventArgs e) => this.Colour = mixer.MixedColour;
            mixer.Show();
        }
        #endregion
    }
}
