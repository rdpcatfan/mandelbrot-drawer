using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot
{
    struct ConvergenceCheckResult
    {
        public int iCount;
        public double rxPoint;
        public double ryPoint;

        public ConvergenceCheckResult(int iCount, double rxPoint, double ryPoint)
        {
            this.iCount = iCount;
            this.rxPoint = rxPoint;
            this.ryPoint = ryPoint;
        }
    }
}
