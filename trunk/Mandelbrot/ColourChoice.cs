using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot
{
    class ColourChoice : UserControl
    {
        #region member variables
        IList<ColourBox> boxes;

        private int NumberOfColours = 4;
        private int pxPadding = 6;
        #endregion

        #region events
        public event EventHandler ColourChanged;
        #endregion

        public ColourPalette Palette
        {
            get
            {
                return new ColourPalette(boxes[0].Colour, boxes[1].Colour, boxes[2].Colour, boxes[3].Colour);
            }
        }

        public ColourChoice()
        {
            Color[] initialcolours = {Color.White, Color.Blue, Color.Red, Color.Black };
            this.boxes = new List<ColourBox>();
            for (int i = 0; i < NumberOfColours; ++i)
            {
                ColourBox cbox = new ColourBox(initialcolours[i]);

                // Position will be done by the resize method

                cbox.ColourChanged += (object o, EventArgs e) =>
                {
                    if (this.ColourChanged != null)
                        this.ColourChanged(this, EventArgs.Empty);
                };

                this.boxes.Add(cbox);
                this.Controls.Add(cbox);
            }
            this.Resize += this.handleResize;
        }

        private void handleResize(object o, EventArgs e)
        {
            int pxCurrent = 0;
            foreach (ColourBox box in this.boxes)
            {
                box.Size = new Size(this.Size.Height, this.Size.Height);
                box.Location = new Point(pxCurrent, 0);
                pxCurrent += this.Size.Height + pxPadding;
            }
        }
    }
}
