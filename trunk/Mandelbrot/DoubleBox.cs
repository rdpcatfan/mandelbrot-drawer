﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Mandelbrot
{
    class DoubleBox : TextBox
    {
        public double Double
        {
            get
            {
                if (base.Text != "")
                    return Double.Parse(base.Text.Replace(',', '.'), new CultureInfo("en-US").NumberFormat);
                return 0D;
            }
            set
            {
                base.Text = value.ToString();
            }
        }
    }
}
