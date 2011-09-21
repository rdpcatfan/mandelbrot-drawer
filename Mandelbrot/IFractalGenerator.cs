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
        Image generate(int width, int height, double centreX, double centreY, double scale, int iterations);
    }
}
