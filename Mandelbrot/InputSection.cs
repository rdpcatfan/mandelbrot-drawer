using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mandelbrot
{
    class InputSection : UserControl
    {
        private enum inputNames
        {
            centreX,
            centreY,
            scale,
            maxIterations,
            colourChoice,
            ownColour
        }

        #region member vars
        private IDictionary<inputNames, Tuple<Control, Control>> inputs;
        ICollection<string> validSchemeNames;

        private ComboBox colourChoiceBox;
        private DoubleBox centreXBox;
        private DoubleBox centreYBox;
        private DoubleBox scaleBox;
        private TextBox maxIterationsBox;
        private CheckBox ownColourBox;
        private ColourChoice ownColourChoice;

        private readonly Size standardLabelSize = new Size(60, 13);
        private readonly Size standardInputSize = new Size(100, 20);
        private const int pxInternalPadding = 10;
        private const int pyInternalPadding = 8;
        private const int columns = 3;
        #endregion

        #region public properties
        public bool doublerow
        {
            get
            {
                return this.isDoubleRowAt(this.Size.Width);
            }
        }

        public double rxCentre
        {
            get
            {
                return centreXBox.Double;
            }
            set
            {
                centreXBox.Double = value;
            }
        }

        public double ryCentre
        {
            get
            {
                return centreYBox.Double;
            }
            set
            {
                centreYBox.Double = value;
            }
        }

        public double rScale
        {
            get
            {
                return scaleBox.Double;
            }
            set
            {
                scaleBox.Double = value;
            }
        }

        public int iMax
        {
            get
            {
                return Int32.Parse(maxIterationsBox.Text);
            }
            set
            {
                maxIterationsBox.Text = value.ToString();
            }
        }

        public int MinimumWidth
        {
            get
            {
                return columns * Math.Max(standardLabelSize.Width, standardInputSize.Width) + (columns - 1) * pxInternalPadding;
            }
        }

        public int MinimumHeight
        {
            get
            {
                return recommendedSize(MinimumWidth).Height;
            }
        }

        public string colourSchemeName
        {
            get
            {
                return (string)colourChoiceBox.SelectedItem;
            }
            set
            {
                colourChoiceBox.SelectedItem = value;
            }
        }
        #endregion

        public InputSection(ICollection<string> colourSchemes)
        {
            this.validSchemeNames = colourSchemes;
            this.inputs = new Dictionary<inputNames, Tuple<Control, Control>>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            int tabindex = 0;

            // Using braces to limit scope.
            {   // centreX
                Label templabel = new Label();
                DoubleBox tempbox = new DoubleBox();
                inputs[inputNames.centreX] = new Tuple<Control, Control>(templabel, tempbox);
                this.centreXBox = tempbox;

                templabel.Text = "Midden X:";
                templabel.Size = standardLabelSize;
                templabel.TextAlign = ContentAlignment.MiddleRight;

                tempbox.Text = (0.0).ToString();
                tempbox.Size = standardInputSize;
                tempbox.TabIndex = tabindex++;
            }

            {   // centreY
                Label templabel = new Label();
                DoubleBox tempbox = new DoubleBox();
                inputs[inputNames.centreY] = new Tuple<Control, Control>(templabel, tempbox);
                this.centreYBox = tempbox;

                templabel.Text = "Midden Y:";
                templabel.Size = standardLabelSize;
                templabel.TextAlign = ContentAlignment.MiddleRight;

                tempbox.Text = (0.0).ToString();
                tempbox.Size = standardInputSize;
                tempbox.TabIndex = tabindex++;
            }

            {   // scale
                Label templabel = new Label();
                DoubleBox tempbox = new DoubleBox();
                inputs[inputNames.scale] = new Tuple<Control, Control>(templabel, tempbox);
                this.scaleBox = tempbox;

                templabel.Text = "Schaal:";
                templabel.Size = standardLabelSize;
                templabel.TextAlign = ContentAlignment.MiddleRight;

                tempbox.Text = (0.01).ToString();
                tempbox.Size = standardInputSize;
                tempbox.TabIndex = tabindex++;
            }

            {   // iterations
                Label templabel = new Label();
                TextBox tempbox = new TextBox();
                inputs[inputNames.maxIterations] = new Tuple<Control, Control>(templabel, tempbox);
                this.maxIterationsBox = tempbox;

                templabel.Text = "Iteraties:";
                templabel.Size = standardLabelSize;
                templabel.TextAlign = ContentAlignment.MiddleRight;

                tempbox.Text = (500).ToString();
                tempbox.Size = standardInputSize;
                tempbox.TabIndex = tabindex++;
            }

            {   // colour choice
                Label templabel = new Label();
                ComboBox tempbox = new ComboBox();
                inputs[inputNames.colourChoice] = new Tuple<Control, Control>(templabel, tempbox);
                this.colourChoiceBox = tempbox;

                templabel.Text = "Kleur:";
                templabel.Size = standardLabelSize;
                templabel.TextAlign = ContentAlignment.MiddleRight;

                tempbox.Size = standardInputSize;
                tempbox.Items.AddRange(validSchemeNames.ToArray());
                tempbox.Text = "Default";
                tempbox.SelectedItem = "Default";
                tempbox.TabIndex = tabindex++;
            }

            this.Resize += makeLayout;
            this.OnResize(new EventArgs());

            foreach (Tuple<Control, Control> t in inputs.Values) // Typical example of where var is nice.
            {
                this.Controls.Add(t.Item1);
                this.Controls.Add(t.Item2);
            }
        }

        private void makeLayout(object sender, EventArgs e)
        {
            int pxStep, pyStep, pxStart, pyStart; // Distance between inputs and starting points.
            pxStart = 0;
            pyStart = 0;
            if (doublerow)
            {
                pxStep = (this.Size.Width - (columns - 1) * pxInternalPadding) / 3;
                pyStep = pyInternalPadding + standardInputSize.Height + standardLabelSize.Height;
            }
            else
            {
                pxStep = standardInputSize.Width + standardLabelSize.Width + pxInternalPadding;
                pyStep = Math.Max(standardInputSize.Height, standardLabelSize.Height) + pyInternalPadding;
            }
            int rows = (int)Math.Ceiling(inputs.Values.Count / (double)columns);
            int currentRow = 1, pxCurrent = pxStart, pyCurrent = pyStart;
            foreach (Tuple<Control, Control> t in inputs.Values)
            {
                t.Item1.Location = new Point(pxCurrent, pyCurrent);
                if (doublerow)
                    t.Item2.Location = new Point(pxCurrent + pxStep - t.Item2.Width, pyCurrent + t.Item1.Height + 2);
                else
                    t.Item2.Location = new Point(pxCurrent + t.Item1.Width + 2, pyCurrent);
                if (currentRow % rows != 0)
                {
                    pyCurrent += pyStep;
                    currentRow += 1;
                }
                else
                {
                    pyCurrent = pyStart;
                    pxCurrent += pxStep;
                    currentRow = 1; // Not necessary, as modulo is cyclical anyway, but easier to debug.
                }
            }
        }

        private bool isDoubleRowAt(int width)
        {
            return columns * (standardInputSize.Width + standardLabelSize.Width) +
                (columns - 1) * pxInternalPadding > width;
        }

        /// <summary>
        /// Return the recommended size setting for the given width.
        /// </summary>
        /// <param name="width">Width to be used.</param>
        /// <returns></returns>
        public Size recommendedSize(int width)
        {
            int rows = (int)Math.Ceiling(inputs.Values.Count / (double)columns);
            if (isDoubleRowAt(width))
                return new Size(width, rows * (standardLabelSize.Height + standardInputSize.Height + pyInternalPadding));
            else
                return new Size(width, rows * (Math.Max(standardLabelSize.Height, standardInputSize.Height) + pyInternalPadding));
        }
    }
}
