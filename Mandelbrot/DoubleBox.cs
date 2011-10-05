using System;
using System.Globalization;
using System.Windows.Forms;

namespace Mandelbrot
{
    /// <summary>
    /// Convenience class for a textbox that should have doubles
    /// entered into it.
    /// </summary>
    class DoubleBox : TextBox
    {
        #region properties
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
                catch
                {
                    base.Invoke((Action)(() => base.Text = (0.01).ToString()));
                    return 0.01;
                }
            }
            set
            {
                base.Text = value.ToString();
            }
        }
        #endregion
    }
}
