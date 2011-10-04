using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mandelbrot
{
    class ColourBox : PictureBox
    {
        #region member variables
        private Color _colour;
        #endregion

        #region properties
        /// <summary>
        /// Colour currently selected and displayed.
        /// </summary>
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

        #region events
        /// <summary>
        /// Triggered when the colour is changed.
        /// </summary>
        public event EventHandler ColourChanged;
        #endregion

        #region constructors
        /// <summary>
        /// Constuctor.
        /// </summary>
        /// <param name="c">Colour to initialise to.</param>
        public ColourBox(Color c)
        {
            this._colour = c;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Paint += (object o, PaintEventArgs e) =>
            {
                using (Brush br = new SolidBrush(this.Colour))
                {
                    e.Graphics.FillRectangle(br, this.ClientRectangle);
                }
            };
            this.Click += makeColourChoiceDialog;
        }
        #endregion

        #region event handlers
        /// <summary>
        /// Create a dialog for choosing the colour.
        /// </summary>
        private void makeColourChoiceDialog(object o, EventArgs ea)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = true;
            dialog.ShowHelp = true;
            dialog.Color = this.Colour;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.Colour = dialog.Color;
                if (this.ColourChanged != null)
                    ColourChanged(this, EventArgs.Empty);
            }
            // In other cases, do nothing
        }
        #endregion
    }
}
