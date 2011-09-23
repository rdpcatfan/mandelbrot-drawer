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
        protected override ConvergenceCheckResult checkConvergence(int maxIterations, double x, double y)
        {
            int iterationCount = 0;
            double a = x;
            double b = y;
            double tempA; // Declared here for possible efficiency reasons
            if (x < 0.02533 && x * x + y * y < 0.3)
                return new ConvergenceCheckResult(Infinity, x, y);
            if (x * x + y * y > 4)
                return new ConvergenceCheckResult(Infinity, x, y); // For a nice black colour

            for (int i = 1; i < maxIterations; i++)
            {
                tempA = a * a - b * b + x;
                b = 2 * a * b + y;
                a = tempA;
                iterationCount++;
                if (a * a + b * b > 4)
                    return new ConvergenceCheckResult(iterationCount, a, b);
            }

            return new ConvergenceCheckResult(Infinity, a, b);
        }

        protected override Int32 getColour(ConvergenceCheckResult res)
        {
            if (res.iterations == Infinity)
                return 0; // black
            double v = res.iterations - Math.Log(Math.Log(Math.Sqrt(res.x * res.x + res.y * res.y)) / Math.Log(Math.Pow(10, 100)), 2);
            const int width = 3;
            const int fill = (1 << width) - 1;
            const int rest = 8 - width;
            int c = (int)Math.IEEERemainder(v, Math.Pow(2, 512));
            return ((c & ((fill+4) << (2*width))) << (3*rest)) | ((c & (fill << width)) << (2*rest)) | ((c & fill) << rest);
        }
    }
}
