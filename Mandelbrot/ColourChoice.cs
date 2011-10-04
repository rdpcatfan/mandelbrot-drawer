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

        public ColourChoice()
        {
            Color[] initialcolours = {Color.White, Color.Blue, Color.Red, Color.Black };
            this.boxes = new List<ColourBox>();
            for (int i = 0; i < NumberOfColours; ++i)
            {
                ColourBox cbox = new ColourBox(initialcolours[i]);

                // Position will be done later

                this.boxes.Add(cbox);
                this.Controls.Add(cbox);
            }
            this.handleResize(this, EventArgs.Empty);
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
