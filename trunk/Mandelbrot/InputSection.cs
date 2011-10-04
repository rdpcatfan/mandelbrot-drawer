using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        IDictionary<string, ColourPalette> colours;

        private ComboBox colourChoiceBox;
        private DoubleBox centreXBox;
        private DoubleBox centreYBox;
        private DoubleBox scaleBox;
        private TextBox maxIterationsBox;
        private CheckBox ownColourBox;
        private ColourChoice ownColourChoice;

        public ColourPalette CurrentPalette;

        private static readonly Size standardLabelSize = new Size(85, 18);
        private static readonly Size standardInputSize = new Size(100, 18);
        private const int pxInternalPadding = 10;
        private const int pyInternalPadding = 8;
        private const int columns = 3;
        #endregion

        #region public properties
        public bool doublerow
        {
            get
            {
                return InputSection.isDoubleRowAt(this.Size.Width);
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

        #region constructors
        public InputSection()
        {
            this.inputs = new Dictionary<inputNames, Tuple<Control, Control>>();
            InitializeComponent();
        }

        /// <summary>
        /// Initialise the graphical user interface.
        /// </summary>
        /// <remarks>
        /// Placed in constructors as it should only be called from the constructor.
        /// </remarks>
        private void InitializeComponent()
        {
            this.colours = new Dictionary<string, ColourPalette>();
            this.colours["Default"] = new ColourPalette(Color.White, Color.Red, Color.Green, Color.Blue);
            this.colours["Forest"] = new ColourPalette(Color.MidnightBlue, Color.ForestGreen, Color.FloralWhite, Color.Gray);
            this.colours["Awful"] = new ColourPalette(Color.Chocolate, Color.Lime, Color.PeachPuff, Color.Purple);

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

            {   // colour cbox
                Label templabel = new Label();
                ComboBox tempbox = new ComboBox();
                inputs[inputNames.colourChoice] = new Tuple<Control, Control>(templabel, tempbox);
                this.colourChoiceBox = tempbox;

                templabel.Text = "Kleur:";
                templabel.Size = standardLabelSize;
                templabel.TextAlign = ContentAlignment.MiddleRight;

                tempbox.Size = standardInputSize;
                tempbox.Items.AddRange(this.colours.Keys.ToArray());
                tempbox.Text = "Default";
                tempbox.SelectedItem = "Default";
                this.CurrentPalette = this.colours["Default"];
                tempbox.SelectedValueChanged += (object o, EventArgs e) =>
                {
                    this.CurrentPalette = this.colours[this.colourSchemeName];
                    this.ownColourBox.Checked = false;
                };
                tempbox.TabIndex = tabindex++;
            }

            {   // own colour box
                CheckBox tempcbox = new CheckBox();
                ColourChoice tempchoice = new ColourChoice();
                inputs[inputNames.ownColour] = new Tuple<Control, Control>(tempcbox, tempchoice);
                this.ownColourBox = tempcbox;
                this.ownColourChoice = tempchoice;

                tempcbox.Text = "Eigen kleur:";
                tempcbox.Size = standardLabelSize;
                tempcbox.TextAlign = ContentAlignment.MiddleRight;
                tempcbox.TabIndex = tabindex++;

                tempcbox.CheckedChanged += (object o, EventArgs e) =>
                {
                    if (tempcbox.Checked)
                        this.CurrentPalette = tempchoice.Palette;
                    else
                        this.CurrentPalette = this.colours[this.colourSchemeName];
                    this.Invalidate(); // Easy way of notifying that the fractal must be redrawn
                };

                tempchoice.Size = standardInputSize;
                tempchoice.ColourChanged += (object o, EventArgs e) =>
                {
                    tempcbox.Checked = true;
                    this.CurrentPalette = tempchoice.Palette;
                    this.Invalidate(); // Easy way of notifying that the fractal must be redrawn
                };
                tempchoice.TabIndex = tabindex++;
            }

            this.Resize += makeLayout;
            this.OnResize(new EventArgs());

            foreach (Tuple<Control, Control> t in inputs.Values) // Typical example of where var is nice.
            {
                this.Controls.Add(t.Item1);
                this.Controls.Add(t.Item2);
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Return the recommended size setting for the given width.
        /// </summary>
        /// <param name="width">Width to be used.</param>
        public Size recommendedSize(int width)
        {
            int rows = (int)Math.Ceiling(inputs.Values.Count / (double)columns);
            if (isDoubleRowAt(width))
                return new Size(width, rows * (standardLabelSize.Height + standardInputSize.Height + pyInternalPadding));
            else
                return new Size(width, rows * (Math.Max(standardLabelSize.Height, standardInputSize.Height) + pyInternalPadding));
        }
        #endregion

        #region event handlers
        /// <summary>
        /// Ensure all elements are laid out correctly.
        /// </summary>
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
        #endregion

        #region static methods
        /// <summary>
        /// Check whether the layout is double-row at a given width.
        /// </summary>
        private static bool isDoubleRowAt(int width)
        {
            return columns * (standardInputSize.Width + standardLabelSize.Width) +
                (columns - 1) * pxInternalPadding > width;
        }
        #endregion
    }
}
