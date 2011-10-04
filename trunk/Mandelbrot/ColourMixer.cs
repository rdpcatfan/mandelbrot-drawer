using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mandelbrot
{
    class ColourMixer : Form
    {
        public enum BaseColour {
            red,
            green,
            blue
        }

        #region member variables
        private IDictionary<BaseColour, NumericUpDown> counters;
        private PictureBox preview;

        private readonly Size labelSize = new Size(40, 18);
        private readonly Size counterSize = new Size(60, 18);
        private const int pxExternalPadding = 6;
        private const int pyExternalPadding = 6;
        private const int pxInternalPadding = 10;
        private const int pyInternalPadding = 6;
        #endregion

        #region properties
        private byte this[BaseColour b]
        {
            get
            {
                return (byte)counters[b].Value;
            }
            set
            {
                counters[b].Value = value;
            }
        }

        public Color MixedColour
        {
            get
            {
                return Color.FromArgb(this[BaseColour.red], this[BaseColour.green], this[BaseColour.blue]);
            }
            set
            {
                this[BaseColour.red] = value.R;
                this[BaseColour.green] = value.G;
                this[BaseColour.blue] = value.B;
            }
        }
        #endregion

        #region events
        public event EventHandler ColourChanged;
        #endregion

        public ColourMixer(Color c)
        {
            InitializeComponents();
            this.MixedColour = c;
        }

        private void InitializeComponents()
        {

            counters = new Dictionary<BaseColour, NumericUpDown>();
            IDictionary<BaseColour, Tuple<string, NumericUpDown>> colours = new Dictionary<BaseColour, Tuple<string, NumericUpDown>>();
            colours[BaseColour.red] = new Tuple<string, NumericUpDown>("Rood:", new NumericUpDown());
            colours[BaseColour.green] = new Tuple<string, NumericUpDown>("Groen:", new NumericUpDown());
            colours[BaseColour.blue] = new Tuple<string, NumericUpDown>("Blauw:", new NumericUpDown());

            int currentHeight = pyExternalPadding;
            int pyStep = Math.Max(labelSize.Height, counterSize.Height) + pyInternalPadding;
            int pxCounterBegin = labelSize.Width + pxInternalPadding + pxExternalPadding;
            foreach (BaseColour b in colours.Keys)
            {
                Tuple<string, NumericUpDown> t = colours[b];
                Label tempLabel = new Label();
                tempLabel.Location = new Point(pxExternalPadding, currentHeight);
                tempLabel.Size = labelSize;
                tempLabel.Text = t.Item1;
                this.Controls.Add(tempLabel);

                NumericUpDown tempCounter = new NumericUpDown();
                tempCounter.Location = new Point(pxCounterBegin, currentHeight);
                tempCounter.Size = counterSize;
                tempCounter.Minimum = 0;
                tempCounter.Maximum = 0xFF;
                tempCounter.ValueChanged += (object o, EventArgs e) =>
                {
                    if (ColourChanged != null)
                        ColourChanged(this, EventArgs.Empty);
                    preview.Invalidate();
                };
                this.counters[b] = tempCounter;
                tempCounter.Value = this[b];
                this.Controls.Add(tempCounter);

                currentHeight += pyStep;
            }

            preview = new PictureBox();
            preview.Location = new Point(pxCounterBegin + counterSize.Width + pxInternalPadding, pyExternalPadding);
            preview.Size = new Size(3 * pyStep - pxInternalPadding, 3 * pyStep - pyInternalPadding); // A square box
            preview.Paint += (object o, PaintEventArgs pea) =>
            {
                using (Brush br = new SolidBrush(this.MixedColour))
                {
                    pea.Graphics.FillRectangle(br, this.preview.ClientRectangle);
                };
            };

            this.Size = new Size(pxCounterBegin + counterSize.Width + pxInternalPadding + 3 * pyStep + 2 * pxExternalPadding, currentHeight + pyStep + 2 * pyExternalPadding);
            this.Text = "Kleur Kiezer";
            this.Controls.Add(preview);
        }
    }
}
