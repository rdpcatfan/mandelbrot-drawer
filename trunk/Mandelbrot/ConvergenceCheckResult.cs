namespace Mandelbrot
{
    /// <summary>
    /// Convenience struct for passing results of calculations.
    /// </summary>
    struct ConvergenceCheckResult
    {
        #region member variables
        public int iCount;
        public double rxPoint;
        public double ryPoint;
        #endregion

        #region constructors
        public ConvergenceCheckResult(int iCount, double rxPoint, double ryPoint)
        {
            this.iCount = iCount;
            this.rxPoint = rxPoint;
            this.ryPoint = ryPoint;
        }
        #endregion
    }
}
