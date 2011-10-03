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

        private CheckBox ownColourBox;
        private ComboBox colourChoiceBox;
        private DoubleBox centreXBox;
        private DoubleBox centreYBox;
        private DoubleBox scaleBox;
        private TextBox maxIterationsBox;

        private readonly Size standardLabelSize = new Size(60, 13);
        private readonly Size standardInputSize = new Size(100, 20);
        private const int pxInternalPadding = 10;
        private const int pyInternalPadding = 5;
        private const int columns = 3;
        #endregion

        #region public properties
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

        public override Size MinimumSize
        {
            get
            {
                int pxMinimum = columns * Math.Max(standardInputSize.Width, standardLabelSize.Width) + (columns - 1) * pxInternalPadding;
                return new Size(pxMinimum, 100);
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

            this.Resize += layoutInputs;
            this.OnResize(new EventArgs());

            foreach (Tuple<Control, Control> t in inputs.Values) // Typical example of where var is nice.
            {
                this.Controls.Add(t.Item1);
                this.Controls.Add(t.Item2);
            }
        }

        private void layoutInputs(object sender, EventArgs e)
        {
            // True if the window is rather narrow
            bool doublerow =
                columns * (standardInputSize.Width + standardLabelSize.Width) +
                (columns - 1) * pxInternalPadding > this.Size.Width;
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
                    t.Item2.Location = new Point(pxCurrent + pxStep - t.Item2.Width, pyCurrent + t.Item1.Height);
                else
                    t.Item2.Location = new Point(pxCurrent + t.Item1.Width, pyCurrent);
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
    }
}
