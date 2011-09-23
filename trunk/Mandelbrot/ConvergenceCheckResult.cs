using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot
{
    struct ConvergenceCheckResult
    {
        public int iterations;
        public double x;
        public double y;

        public ConvergenceCheckResult(int it, double x, double y)
        {
            this.iterations = it;
            this.x = x;
            this.y = y;
        }
    }
}
