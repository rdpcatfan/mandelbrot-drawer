using System;
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
                try
                {
                    if (base.Text != "")
                        return Double.Parse(base.Text.Replace(',', '.'), new CultureInfo("en-US").NumberFormat);
                    return 0.01;
                }
                catch(Exception exc)
                {
                    base.Text = (0.01).ToString();
                    throw;
                }
            }
            set
            {
                base.Text = value.ToString();
            }
        }
    }
}
