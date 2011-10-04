namespace Mandelbrot
{
    class MandelbrotGenerator : FractalGenerator
    {
        #region overrides
        /// <summary>
        /// Check whether the mandelbrot function converges at the given point, in
        /// no more than the given number of iterations.
        /// </summary>
        /// <param name="rxPoint">X-value to check.</param>
        /// <param name="ryPoint">Y-value to check.</param>
        /// <param name="iMax">Maximum number of iterations to check for.</param>
        /// <returns>How long the point took to converge, and the final coordinates.</returns>
        protected override ConvergenceCheckResult checkConvergence(double rxPoint, double ryPoint, int iMax)
        {
            double rxTemp = rxPoint;
            double ryTemp = ryPoint;
            double tempA;
            
            // Check whether the point is within one of the big circles
            double q = (rxPoint - 0.25) * (rxPoint - 0.25) + ryPoint * ryPoint;
            if (q * (q + rxPoint - 0.25) < 0.25 * ryPoint * ryPoint)
                return new ConvergenceCheckResult(iInfinity, rxPoint, ryPoint);
            if (rxPoint * rxPoint + 2 * rxPoint + 1 + ryPoint * ryPoint < 0.0625)
                return new ConvergenceCheckResult(iInfinity, rxPoint, ryPoint);
            
            // Check whether the point is far away
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

        #endregion

    }
}
