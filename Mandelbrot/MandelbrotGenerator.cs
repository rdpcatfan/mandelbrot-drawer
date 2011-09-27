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
        #region overriding functions

        protected override ConvergenceCheckResult checkConvergence(double rxPoint, double ryPoint, int iMax)
        {
            double rxTemp = rxPoint;
            double ryTemp = ryPoint;
            double tempA; // Declared here for possible efficiency reasons
            if (rxPoint < 0.02533 && rxPoint * rxPoint + ryPoint * ryPoint < 0.3)
                return new ConvergenceCheckResult(iInfinity, rxPoint, ryPoint);
            if (rxPoint * rxPoint + ryPoint * ryPoint > 4)
                return new ConvergenceCheckResult(iInfinity, rxPoint, ryPoint); // For a nice black colour

            for (int iCounter = 1; iCounter < iMax; iCounter++)
            {
                tempA = rxTemp * rxTemp - ryTemp * ryTemp + rxPoint;
                ryTemp = 2 * rxTemp * ryTemp + ryPoint;
                rxTemp = tempA;
                if (rxTemp * rxTemp + ryTemp * ryTemp > 4)
                    return new ConvergenceCheckResult(iCounter, rxTemp, ryTemp);
            }

            return new ConvergenceCheckResult(iInfinity, rxTemp, ryTemp);
        }
                
        protected override Int32 getColour(ConvergenceCheckResult res)
        {
            if (res.iCount == iInfinity)
                return 0; // black

            double v = res.iCount - Math.Log(0.5 * Math.Log(res.rxPoint * res.rxPoint + res.ryPoint * res.ryPoint) / Math.Log(Math.Pow(10, 100)), 2);
            int colourInt1 = (int)v % colourPalette.Count;
            int colourInt2 = (colourInt1 + 1) % colourPalette.Count;
            double colourFraction2 = v - colourInt1;
            double colourFraction1 = 1 - colourFraction2;
            byte r = (byte)(colourPalette[colourInt1].R * colourFraction1 + colourPalette[colourInt2].R * colourFraction2);
            byte g = (byte)(colourPalette[colourInt1].G * colourFraction1 + colourPalette[colourInt2].G * colourFraction2);
            byte b = (byte)(colourPalette[colourInt1].B * colourFraction1 + colourPalette[colourInt2].B * colourFraction2);
            return (r << 16) + (g << 8) + b;

            /*
            double v = res.iCount - Math.Log(Math.Log(MandelbrotGenerator.ComplexAbs(res.rxPoint, res.ryPoint)) / Math.Log(1E100), 2);
            const int width = 3;
            const int fill = (1 << width) - 1;
            const int rest = 8 - width;
            int c = (int)Math.IEEERemainder(v, Math.Pow(2, 512));
            return ((c & ((fill+4) << (2*width))) << (3*rest)) | ((c & (fill << width)) << (2*rest)) | ((c & fill) << rest);
            */
        }

        #endregion

        #region private functions

        private static double ComplexAbs(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        #endregion
    }
}
