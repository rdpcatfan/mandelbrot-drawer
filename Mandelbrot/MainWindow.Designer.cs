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
            this.mandelImage = new System.Windows.Forms.PictureBox();
            this.centreXLabel = new System.Windows.Forms.Label();
            this.centreXTextBox = new Mandelbrot.DoubleBox();
            this.centreYTextBox = new Mandelbrot.DoubleBox();
            this.centreYLabel = new System.Windows.Forms.Label();
            this.scaleTextBox = new Mandelbrot.DoubleBox();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.maxIterationsTextBox = new System.Windows.Forms.TextBox();
            this.maxIterationsLabel = new System.Windows.Forms.Label();
            this.timerLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mandelImage)).BeginInit();
            this.SuspendLayout();
            // 
            // generateImageButton
            // 
            this.generateImageButton.Location = new System.Drawing.Point(516, 554);
            this.generateImageButton.Name = "generateImageButton";
            this.generateImageButton.Size = new System.Drawing.Size(75, 23);
            this.generateImageButton.TabIndex = 0;
            this.generateImageButton.Text = "Start";
            this.generateImageButton.UseVisualStyleBackColor = true;
            this.generateImageButton.Click += new System.EventHandler(this.generateMandelbrotClick);
            // 
            // mandelImage
            // 
            this.mandelImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mandelImage.Location = new System.Drawing.Point(10, 77);
            this.mandelImage.Name = "mandelImage";
            this.mandelImage.Size = new System.Drawing.Size(500, 500);
            this.mandelImage.TabIndex = 1;
            this.mandelImage.TabStop = false;
            this.mandelImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.setImageCentre);
            this.mandelImage.MouseEnter += new System.EventHandler(this.setImageFocus);
            this.mandelImage.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.setImageZoom);
            // 
            // centreXLabel
            // 
            this.centreXLabel.AutoSize = true;
            this.centreXLabel.Location = new System.Drawing.Point(12, 19);
            this.centreXLabel.Name = "centreXLabel";
            this.centreXLabel.Size = new System.Drawing.Size(54, 13);
            this.centreXLabel.TabIndex = 2;
            this.centreXLabel.Text = "midden X:";
            // 
            // centreXTextBox
            // 
            this.centreXTextBox.Double = 0D;
            this.centreXTextBox.Location = new System.Drawing.Point(72, 16);
            this.centreXTextBox.Name = "centreXTextBox";
            this.centreXTextBox.Size = new System.Drawing.Size(100, 20);
            this.centreXTextBox.TabIndex = 3;
            this.centreXTextBox.Text = "0.0";
            // 
            // centreYTextBox
            // 
            this.centreYTextBox.Double = 0D;
            this.centreYTextBox.Location = new System.Drawing.Point(72, 47);
            this.centreYTextBox.Name = "centreYTextBox";
            this.centreYTextBox.Size = new System.Drawing.Size(100, 20);
            this.centreYTextBox.TabIndex = 5;
            this.centreYTextBox.Text = "0.0";
            // 
            // centreYLabel
            // 
            this.centreYLabel.AutoSize = true;
            this.centreYLabel.Location = new System.Drawing.Point(12, 50);
            this.centreYLabel.Name = "centreYLabel";
            this.centreYLabel.Size = new System.Drawing.Size(54, 13);
            this.centreYLabel.TabIndex = 4;
            this.centreYLabel.Text = "midden Y:";
            // 
            // scaleTextBox
            // 
            this.scaleTextBox.Double = 0.01D;
            this.scaleTextBox.Location = new System.Drawing.Point(255, 16);
            this.scaleTextBox.Name = "scaleTextBox";
            this.scaleTextBox.Size = new System.Drawing.Size(100, 20);
            this.scaleTextBox.TabIndex = 7;
            this.scaleTextBox.Text = "0.01";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(203, 19);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(41, 13);
            this.scaleLabel.TabIndex = 6;
            this.scaleLabel.Text = "schaal:";
            // 
            // maxIterationsTextBox
            // 
            this.maxIterationsTextBox.Location = new System.Drawing.Point(255, 47);
            this.maxIterationsTextBox.Name = "maxIterationsTextBox";
            this.maxIterationsTextBox.Size = new System.Drawing.Size(100, 20);
            this.maxIterationsTextBox.TabIndex = 9;
            this.maxIterationsTextBox.Text = "500";
            // 
            // maxIterationsLabel
            // 
            this.maxIterationsLabel.AutoSize = true;
            this.maxIterationsLabel.Location = new System.Drawing.Point(203, 50);
            this.maxIterationsLabel.Name = "maxIterationsLabel";
            this.maxIterationsLabel.Size = new System.Drawing.Size(46, 13);
            this.maxIterationsLabel.TabIndex = 8;
            this.maxIterationsLabel.Text = "iteraties:";
            // 
            // timerLabel
            // 
            this.timerLabel.AutoSize = true;
            this.timerLabel.Location = new System.Drawing.Point(372, 15);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(77, 13);
            this.timerLabel.TabIndex = 10;
            this.timerLabel.Text = "Timer info here";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 589);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.maxIterationsTextBox);
            this.Controls.Add(this.maxIterationsLabel);
            this.Controls.Add(this.scaleTextBox);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.centreYTextBox);
            this.Controls.Add(this.centreYLabel);
            this.Controls.Add(this.centreXTextBox);
            this.Controls.Add(this.centreXLabel);
            this.Controls.Add(this.mandelImage);
            this.Controls.Add(this.generateImageButton);
            this.Name = "MainWindow";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.mandelImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button generateImageButton;
        private System.Windows.Forms.PictureBox mandelImage;
        private System.Windows.Forms.Label centreXLabel;
        private DoubleBox centreXTextBox;
        private DoubleBox centreYTextBox;
        private System.Windows.Forms.Label centreYLabel;
        private DoubleBox scaleTextBox;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.TextBox maxIterationsTextBox;
        private System.Windows.Forms.Label maxIterationsLabel;
        private System.Windows.Forms.Label timerLabel;

    }
}

