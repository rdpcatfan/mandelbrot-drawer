using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mandelbrot
{
    class ColourPalette : List<Color>
    {
        public ColourPalette()
        {
        }

        public ColourPalette(Color startColor, Color endColor)
        {
            int r, g, b;
            int deltaR = endColor.R - startColor.R;
            int deltaG = endColor.G - startColor.G;
            int deltaB = endColor.B - startColor.B;
            for (int i = 0; i < 1000; i++)
            {
                r = startColor.R + (int)((0.5 * Math.Cos(Math.PI * (1 + 2.0 * i / 100)) + 0.5) * deltaR);
                g = startColor.G + (int)((0.5 * Math.Cos(Math.PI * (1 + 2.0 * i / 100)) + 0.5) * deltaG);
                b = startColor.B + (int)((0.5 * Math.Cos(Math.PI * (1 + 2.0 * i / 100)) + 0.5) * deltaB);
                this.Add(Color.FromArgb(r, g, b));
            }
        }

        public ColourPalette(List<Color> colorList)
        {
            int r, g, b;
            int loopIterations = 1000 / colorList.Count;
            for (int a = 0; a < colorList.Count; a++)
            {
                Color currentColor = colorList[a % colorList.Count];
                Color nextColor = colorList[(a + 1) % colorList.Count];
                int deltaR = nextColor.R - currentColor.R;
                int deltaG = nextColor.G - currentColor.G;
                int deltaB = nextColor.B - currentColor.B;

                for (int i = 0; i < loopIterations; i++)
                {
                    r = currentColor.R + (int)((0.5 * Math.Cos(Math.PI * i / loopIterations) + 0.5) * deltaR);
                    g = currentColor.G + (int)((0.5 * Math.Cos(Math.PI * i / loopIterations) + 0.5) * deltaG);
                    b = currentColor.B + (int)((0.5 * Math.Cos(Math.PI * i / loopIterations) + 0.5) * deltaB);
                    this.Add(Color.FromArgb(r, g, b));
                }
            }
        }
    }
}
