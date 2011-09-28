using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mandelbrot
{
    class ColourPalette : List<Int32>
    {

        public ColourPalette(Color first, Color second, Color third, Color fourth)
        {
            this.populate(first, second, 128);
            this.populate(second, third, 128);
            this.populate(third, fourth, 128);
            this.populate(fourth, first, 128);
        }

        private void populate(Color startColor, Color endColor, int colourBands)
        {
            int r, g, b;
            int deltaR = endColor.R - startColor.R;
            int deltaG = endColor.G - startColor.G;
            int deltaB = endColor.B - startColor.B;
            for (int i = 0; i < colourBands; i++)
            {
                r = startColor.R + calcShade(i, colourBands, deltaR);
                g = startColor.G + calcShade(i, colourBands, deltaG);
                b = startColor.B + calcShade(i, colourBands, deltaB);
                this.Add(ColourPalette.int32FromRGB(r, g, b));
            }
        }

        private static int calcShade(int i, int colourBands, int deltaR)
        {
            return (int)(deltaR * (double)(i) / colourBands);
        }

        private static Int32 int32FromRGB(int r, int g, int b)
        {
            return ((r & 0xFF) << 16) | ((g & 0xFF) << 8) | (b & 0xFF);
        }
    }
}
