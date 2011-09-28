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
            {// To limit scope of p.
                double q = (rxPoint - 0.25) * (rxPoint - 0.25) + ryPoint * ryPoint;
                if (q * (q + rxPoint - 0.25) < 0.25 * ryPoint * ryPoint)
                    return new ConvergenceCheckResult(iInfinity, rxPoint, ryPoint);
                if (rxPoint * rxPoint + 2 * rxPoint + 1 + ryPoint * ryPoint < 0.0625)
                    return new ConvergenceCheckResult(iInfinity, rxPoint, ryPoint);
            }
            if (rxPoint * rxPoint + ryPoint * ryPoint > 4)
                return new ConvergenceCheckResult(0, rxPoint, ryPoint);

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

            double v = res.iCount - Math.Log(0.5 * Math.Log(res.rxPoint * res.rxPoint + res.ryPoint * res.ryPoint, 1E100), 2);
            int colourInt1 = (int)v & 0x1FF;
            return colourPalette[colourInt1];
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
