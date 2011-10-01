namespace Mandelbrot
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.generateImageButton = new System.Windows.Forms.Button();
            this.mandelImageContainer = new System.Windows.Forms.PictureBox();
            this.centreXLabel = new System.Windows.Forms.Label();
            this.centreYLabel = new System.Windows.Forms.Label();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.maxIterationsTextBox = new System.Windows.Forms.TextBox();
            this.maxIterationsLabel = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusStripSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.scaleTextBox = new Mandelbrot.DoubleBox();
            this.centreYTextBox = new Mandelbrot.DoubleBox();
            this.centreXTextBox = new Mandelbrot.DoubleBox();
            ((System.ComponentModel.ISupportInitialize)(this.mandelImageContainer)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // generateImageButton
            // 
            this.generateImageButton.Location = new System.Drawing.Point(401, 49);
            this.generateImageButton.Name = "generateImageButton";
            this.generateImageButton.Size = new System.Drawing.Size(75, 23);
            this.generateImageButton.TabIndex = 0;
            this.generateImageButton.Text = "Start";
            this.generateImageButton.UseVisualStyleBackColor = true;
            this.generateImageButton.Click += new System.EventHandler(this.generateMandelbrotClick);
            // 
            // mandelImageContainer
            // 
            this.mandelImageContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mandelImageContainer.Location = new System.Drawing.Point(20, 80);
            this.mandelImageContainer.Name = "mandelImageContainer";
            this.mandelImageContainer.Size = new System.Drawing.Size(500, 500);
            this.mandelImageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mandelImageContainer.TabIndex = 1;
            this.mandelImageContainer.TabStop = false;
            this.mandelImageContainer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.setImageCentre);
            this.mandelImageContainer.MouseEnter += new System.EventHandler(this.setImageFocus);
            this.mandelImageContainer.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.setImageZoom);
            // 
            // centreXLabel
            // 
            this.centreXLabel.AutoSize = true;
            this.centreXLabel.Location = new System.Drawing.Point(21, 24);
            this.centreXLabel.Name = "centreXLabel";
            this.centreXLabel.Size = new System.Drawing.Size(54, 13);
            this.centreXLabel.TabIndex = 2;
            this.centreXLabel.Text = "midden X:";
            // 
            // centreYLabel
            // 
            this.centreYLabel.AutoSize = true;
            this.centreYLabel.Location = new System.Drawing.Point(21, 55);
            this.centreYLabel.Name = "centreYLabel";
            this.centreYLabel.Size = new System.Drawing.Size(54, 13);
            this.centreYLabel.TabIndex = 4;
            this.centreYLabel.Text = "midden Y:";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(212, 24);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(41, 13);
            this.scaleLabel.TabIndex = 6;
            this.scaleLabel.Text = "schaal:";
            // 
            // maxIterationsTextBox
            // 
            this.maxIterationsTextBox.Location = new System.Drawing.Point(264, 52);
            this.maxIterationsTextBox.Name = "maxIterationsTextBox";
            this.maxIterationsTextBox.Size = new System.Drawing.Size(100, 20);
            this.maxIterationsTextBox.TabIndex = 9;
            this.maxIterationsTextBox.Text = "500";
            // 
            // maxIterationsLabel
            // 
            this.maxIterationsLabel.AutoSize = true;
            this.maxIterationsLabel.Location = new System.Drawing.Point(212, 55);
            this.maxIterationsLabel.Name = "maxIterationsLabel";
            this.maxIterationsLabel.Size = new System.Drawing.Size(46, 13);
            this.maxIterationsLabel.TabIndex = 8;
            this.maxIterationsLabel.Text = "iteraties:";
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripSizeLabel,
            this.statusStripTimeLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 600);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip.Size = new System.Drawing.Size(540, 20);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 11;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusStripSizeLabel
            // 
            this.statusStripSizeLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusStripSizeLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusStripSizeLabel.Name = "statusStripSizeLabel";
            this.statusStripSizeLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.statusStripSizeLabel.Size = new System.Drawing.Size(97, 15);
            this.statusStripSizeLabel.Text = "Size:  500 x 500";
            // 
            // statusStripTimeLabel
            // 
            this.statusStripTimeLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusStripTimeLabel.Name = "statusStripTimeLabel";
            this.statusStripTimeLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.statusStripTimeLabel.Size = new System.Drawing.Size(110, 15);
            this.statusStripTimeLabel.Text = "Generated in: ms";
            // 
            // scaleTextBox
            // 
            this.scaleTextBox.Double = 0.01D;
            this.scaleTextBox.Location = new System.Drawing.Point(264, 21);
            this.scaleTextBox.Name = "scaleTextBox";
            this.scaleTextBox.Size = new System.Drawing.Size(100, 20);
            this.scaleTextBox.TabIndex = 7;
            this.scaleTextBox.Text = "0.01";
            // 
            // centreYTextBox
            // 
            this.centreYTextBox.Double = 0D;
            this.centreYTextBox.Location = new System.Drawing.Point(81, 52);
            this.centreYTextBox.Name = "centreYTextBox";
            this.centreYTextBox.Size = new System.Drawing.Size(100, 20);
            this.centreYTextBox.TabIndex = 5;
            this.centreYTextBox.Text = "0.0";
            // 
            // centreXTextBox
            // 
            this.centreXTextBox.Double = 0D;
            this.centreXTextBox.Location = new System.Drawing.Point(81, 21);
            this.centreXTextBox.Name = "centreXTextBox";
            this.centreXTextBox.Size = new System.Drawing.Size(100, 20);
            this.centreXTextBox.TabIndex = 3;
            this.centreXTextBox.Text = "0.0";
            // 
            // MainWindow
            // 
            this.AcceptButton = this.generateImageButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 620);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.maxIterationsTextBox);
            this.Controls.Add(this.maxIterationsLabel);
            this.Controls.Add(this.scaleTextBox);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.centreYTextBox);
            this.Controls.Add(this.centreYLabel);
            this.Controls.Add(this.centreXTextBox);
            this.Controls.Add(this.centreXLabel);
            this.Controls.Add(this.mandelImageContainer);
            this.Controls.Add(this.generateImageButton);
            this.MinimumSize = new System.Drawing.Size(556, 658);
            this.Name = "MainWindow";
            this.Text = "Mandelbrot Generator";
            this.ResizeBegin += new System.EventHandler(this.setResizeFlag);
            this.ResizeEnd += new System.EventHandler(this.resizeImageContainer);
            this.Resize += new System.EventHandler(this.tryResizeImageContainer);
            ((System.ComponentModel.ISupportInitialize)(this.mandelImageContainer)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button generateImageButton;
        private System.Windows.Forms.PictureBox mandelImageContainer;
        private System.Windows.Forms.Label centreXLabel;
        private Mandelbrot.DoubleBox centreXTextBox;
        private Mandelbrot.DoubleBox centreYTextBox;
        private System.Windows.Forms.Label centreYLabel;
        private Mandelbrot.DoubleBox scaleTextBox;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.TextBox maxIterationsTextBox;
        private System.Windows.Forms.Label maxIterationsLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusStripTimeLabel;
        private System.Windows.Forms.ToolStripStatusLabel statusStripSizeLabel;

    }
}

