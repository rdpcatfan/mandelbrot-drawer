using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Mandelbrot
{
    class MandelbrotGenerator : FractalGenerator
    {
        protected override int checkConvergence(int maxIterations, double x, double y)
        {
            int iterationCount = 0;
            double a = x;
            double b = y;
            double tempA; // Declared here for possible efficiency reasons
            if (x > 2 || y > 2 || a * a + b * b > 4)
                return 0;
            if (x < 0.02533 && x * x + y * y < 0.3)
                return Infinity;

            for (int i = 1; i < maxIterations; i++)
            {
                tempA = a * a - b * b + x;
                b = 2 * a * b + y;
                a = tempA;
                iterationCount++;
                if (a * a + b * b > 4)
                    return iterationCount;
            }

            return Infinity;
        }

        protected override Int32 getColour(int iterationCount, int maxIterations)
        {
            if (iterationCount == Infinity)
                return 0;
            return (int)Math.Round(iterationCount * 255.0 / maxIterations) << 16;
        }
    }
}
