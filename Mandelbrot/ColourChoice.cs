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
        private IList<PictureBox> colours;

        private int NumberOfColours;
        private int pxPadding = 6;
        #endregion

        public ColourChoice()
        {
            int pxCurrent = 0;
            for (int i = 0; i < NumberOfColours; ++i)
            {
                PictureBox tempbox = new PictureBox();
                
                tempbox.Size = new Size(this.Size.Height, this.Size.Height);
                tempbox.Location = new Point(pxCurrent, 0);
                pxCurrent += this.Size.Width + pxPadding;

                Graphics gr = tempbox.CreateGraphics();
                gr.FillRectangle(Brushes.White, tempbox.ClientRectangle);

                tempbox.Click += (object o, EventArgs e) =>
                {
                };
                
                this.Controls.Add(tempbox);
            }
        }
    }
}
