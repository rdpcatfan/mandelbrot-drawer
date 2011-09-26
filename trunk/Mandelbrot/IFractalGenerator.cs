using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Mandelbrot
{
    interface IFractalGenerator
    {
        Image generate(int pxSize, int pySize, double rxCentre, double ryCentre, double rScale, int iMax);
    }
}
