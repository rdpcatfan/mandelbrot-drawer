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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuStripStartList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripStartNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripStartSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripStartExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripHelpList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripHelpInformation = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.colourChoiceLabel = new System.Windows.Forms.Label();
            this.colourChoiceComboBox = new System.Windows.Forms.ComboBox();
            this.ownColourCheckBox = new System.Windows.Forms.CheckBox();
            this.ownColourBox1 = new System.Windows.Forms.PictureBox();
            this.ownColourBox2 = new System.Windows.Forms.PictureBox();
            this.ownColourBox3 = new System.Windows.Forms.PictureBox();
            this.ownColourBox4 = new System.Windows.Forms.PictureBox();
            this.scaleTextBox = new Mandelbrot.DoubleBox();
            this.centreYTextBox = new Mandelbrot.DoubleBox();
            this.centreXTextBox = new Mandelbrot.DoubleBox();
            ((System.ComponentModel.ISupportInitialize)(this.mandelImageContainer)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ownColourBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ownColourBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ownColourBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ownColourBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // generateImageButton
            // 
            this.generateImageButton.Location = new System.Drawing.Point(20, 112);
            this.generateImageButton.Name = "generateImageButton";
            this.generateImageButton.Size = new System.Drawing.Size(500, 27);
            this.generateImageButton.TabIndex = 0;
            this.generateImageButton.Text = "Start";
            this.generateImageButton.UseVisualStyleBackColor = true;
            this.generateImageButton.Click += new System.EventHandler(this.generateMandelbrotClick);
            // 
            // mandelImageContainer
            // 
            this.mandelImageContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mandelImageContainer.Location = new System.Drawing.Point(20, 145);
            this.mandelImageContainer.Name = "mandelImageContainer";
            this.mandelImageContainer.Size = new System.Drawing.Size(500, 459);
            this.mandelImageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mandelImageContainer.TabIndex = 1;
            this.mandelImageContainer.TabStop = false;
            this.mandelImageContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragImageStart);
            this.mandelImageContainer.MouseEnter += new System.EventHandler(this.setImageFocus);
            this.mandelImageContainer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dragImage);
            this.mandelImageContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragImageEnd);
            this.mandelImageContainer.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.setImageZoom);
            // 
            // centreXLabel
            // 
            this.centreXLabel.AutoSize = true;
            this.centreXLabel.Location = new System.Drawing.Point(12, 30);
            this.centreXLabel.Name = "centreXLabel";
            this.centreXLabel.Size = new System.Drawing.Size(55, 13);
            this.centreXLabel.TabIndex = 2;
            this.centreXLabel.Text = "Midden X:";
            // 
            // centreYLabel
            // 
            this.centreYLabel.AutoSize = true;
            this.centreYLabel.Location = new System.Drawing.Point(12, 71);
            this.centreYLabel.Name = "centreYLabel";
            this.centreYLabel.Size = new System.Drawing.Size(55, 13);
            this.centreYLabel.TabIndex = 4;
            this.centreYLabel.Text = "Midden Y:";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(159, 30);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(43, 13);
            this.scaleLabel.TabIndex = 6;
            this.scaleLabel.Text = "Schaal:";
            // 
            // maxIterationsTextBox
            // 
            this.maxIterationsTextBox.Location = new System.Drawing.Point(184, 86);
            this.maxIterationsTextBox.Name = "maxIterationsTextBox";
            this.maxIterationsTextBox.Size = new System.Drawing.Size(115, 20);
            this.maxIterationsTextBox.TabIndex = 9;
            this.maxIterationsTextBox.Text = "500";
            // 
            // maxIterationsLabel
            // 
            this.maxIterationsLabel.AutoSize = true;
            this.maxIterationsLabel.Location = new System.Drawing.Point(159, 71);
            this.maxIterationsLabel.Name = "maxIterationsLabel";
            this.maxIterationsLabel.Size = new System.Drawing.Size(47, 13);
            this.maxIterationsLabel.TabIndex = 8;
            this.maxIterationsLabel.Text = "Iteraties:";
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripSizeLabel,
            this.statusStripTimeLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 624);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip.Size = new System.Drawing.Size(548, 20);
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
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripStartList,
            this.menuStripHelpList});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(548, 23);
            this.menuStrip.TabIndex = 12;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuStripStartList
            // 
            this.menuStripStartList.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuStripStartList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripStartNew,
            this.menuStripStartSave,
            this.menuStripStartExit});
            this.menuStripStartList.Name = "menuStripStartList";
            this.menuStripStartList.Size = new System.Drawing.Size(43, 19);
            this.menuStripStartList.Text = "Start";
            // 
            // menuStripStartNew
            // 
            this.menuStripStartNew.Name = "menuStripStartNew";
            this.menuStripStartNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuStripStartNew.Size = new System.Drawing.Size(218, 22);
            this.menuStripStartNew.Text = "Nieuw";
            this.menuStripStartNew.Click += new System.EventHandler(this.resetImage);
            // 
            // menuStripStartSave
            // 
            this.menuStripStartSave.Name = "menuStripStartSave";
            this.menuStripStartSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuStripStartSave.Size = new System.Drawing.Size(218, 22);
            this.menuStripStartSave.Text = "Afbeelding Opslaan";
            this.menuStripStartSave.Click += new System.EventHandler(this.saveImage);
            // 
            // menuStripStartExit
            // 
            this.menuStripStartExit.Name = "menuStripStartExit";
            this.menuStripStartExit.Size = new System.Drawing.Size(218, 22);
            this.menuStripStartExit.Text = "Afsluiten";
            this.menuStripStartExit.Click += new System.EventHandler(this.exitApplication);
            // 
            // menuStripHelpList
            // 
            this.menuStripHelpList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripHelpInformation,
            this.menuStripHelpAbout});
            this.menuStripHelpList.Name = "menuStripHelpList";
            this.menuStripHelpList.Size = new System.Drawing.Size(44, 19);
            this.menuStripHelpList.Text = "Help";
            // 
            // menuStripHelpInformation
            // 
            this.menuStripHelpInformation.Name = "menuStripHelpInformation";
            this.menuStripHelpInformation.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.menuStripHelpInformation.Size = new System.Drawing.Size(238, 22);
            this.menuStripHelpInformation.Text = "Mandelbrot Generator Help";
            this.menuStripHelpInformation.Click += new System.EventHandler(this.openHelpForm);
            // 
            // menuStripHelpAbout
            // 
            this.menuStripHelpAbout.Name = "menuStripHelpAbout";
            this.menuStripHelpAbout.Size = new System.Drawing.Size(238, 22);
            this.menuStripHelpAbout.Text = "Over Mandelbrot Generator";
            this.menuStripHelpAbout.Click += new System.EventHandler(this.openAboutBox);
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.DefaultExt = "bmp";
            this.saveImageDialog.FileName = "MandelBrot";
            this.saveImageDialog.Filter = "Bitmap Afbeelding|*.bmp|JPeg Afbeelding|*.jpg|Gif Afbeelding|*.gif";
            this.saveImageDialog.Title = "Afbeelding Opslaan";
            // 
            // colourChoiceLabel
            // 
            this.colourChoiceLabel.AutoSize = true;
            this.colourChoiceLabel.Location = new System.Drawing.Point(307, 30);
            this.colourChoiceLabel.Name = "colourChoiceLabel";
            this.colourChoiceLabel.Size = new System.Drawing.Size(66, 13);
            this.colourChoiceLabel.TabIndex = 13;
            this.colourChoiceLabel.Text = "Kleur keuze:";
            // 
            // colourChoiceComboBox
            // 
            this.colourChoiceComboBox.FormattingEnabled = true;
            this.colourChoiceComboBox.Location = new System.Drawing.Point(329, 43);
            this.colourChoiceComboBox.Name = "colourChoiceComboBox";
            this.colourChoiceComboBox.Size = new System.Drawing.Size(178, 21);
            this.colourChoiceComboBox.TabIndex = 14;
            // 
            // ownColourCheckBox
            // 
            this.ownColourCheckBox.AutoSize = true;
            this.ownColourCheckBox.Location = new System.Drawing.Point(310, 70);
            this.ownColourCheckBox.Name = "ownColourCheckBox";
            this.ownColourCheckBox.Size = new System.Drawing.Size(129, 17);
            this.ownColourCheckBox.TabIndex = 15;
            this.ownColourCheckBox.Text = "Eigen kleur gebruiken";
            this.ownColourCheckBox.UseVisualStyleBackColor = true;
            // 
            // ownColourBox1
            // 
            this.ownColourBox1.Location = new System.Drawing.Point(352, 86);
            this.ownColourBox1.Name = "ownColourBox1";
            this.ownColourBox1.Size = new System.Drawing.Size(21, 20);
            this.ownColourBox1.TabIndex = 16;
            this.ownColourBox1.TabStop = false;
            // 
            // ownColourBox2
            // 
            this.ownColourBox2.Location = new System.Drawing.Point(389, 86);
            this.ownColourBox2.Name = "ownColourBox2";
            this.ownColourBox2.Size = new System.Drawing.Size(21, 20);
            this.ownColourBox2.TabIndex = 17;
            this.ownColourBox2.TabStop = false;
            // 
            // ownColourBox3
            // 
            this.ownColourBox3.Location = new System.Drawing.Point(430, 86);
            this.ownColourBox3.Name = "ownColourBox3";
            this.ownColourBox3.Size = new System.Drawing.Size(21, 20);
            this.ownColourBox3.TabIndex = 18;
            this.ownColourBox3.TabStop = false;
            // 
            // ownColourBox4
            // 
            this.ownColourBox4.Location = new System.Drawing.Point(472, 86);
            this.ownColourBox4.Name = "ownColourBox4";
            this.ownColourBox4.Size = new System.Drawing.Size(21, 20);
            this.ownColourBox4.TabIndex = 19;
            this.ownColourBox4.TabStop = false;
            // 
            // scaleTextBox
            // 
            this.scaleTextBox.Double = 0.01D;
            this.scaleTextBox.Location = new System.Drawing.Point(184, 44);
            this.scaleTextBox.Name = "scaleTextBox";
            this.scaleTextBox.Size = new System.Drawing.Size(115, 20);
            this.scaleTextBox.TabIndex = 7;
            this.scaleTextBox.Text = "0.01";
            // 
            // centreYTextBox
            // 
            this.centreYTextBox.Double = 0D;
            this.centreYTextBox.Location = new System.Drawing.Point(39, 86);
            this.centreYTextBox.Name = "centreYTextBox";
            this.centreYTextBox.Size = new System.Drawing.Size(114, 20);
            this.centreYTextBox.TabIndex = 5;
            this.centreYTextBox.Text = "0.0";
            // 
            // centreXTextBox
            // 
            this.centreXTextBox.Double = 0D;
            this.centreXTextBox.Location = new System.Drawing.Point(39, 44);
            this.centreXTextBox.Name = "centreXTextBox";
            this.centreXTextBox.Size = new System.Drawing.Size(114, 20);
            this.centreXTextBox.TabIndex = 3;
            this.centreXTextBox.Text = "0.0";
            // 
            // MainWindow
            // 
            this.AcceptButton = this.generateImageButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 644);
            this.Controls.Add(this.ownColourBox4);
            this.Controls.Add(this.ownColourBox3);
            this.Controls.Add(this.ownColourBox2);
            this.Controls.Add(this.ownColourBox1);
            this.Controls.Add(this.ownColourCheckBox);
            this.Controls.Add(this.colourChoiceComboBox);
            this.Controls.Add(this.colourChoiceLabel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
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
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(556, 678);
            this.Name = "MainWindow";
            this.Text = "Mandelbrot Generator";
            this.ResizeBegin += new System.EventHandler(this.setResizeFlag);
            this.ResizeEnd += new System.EventHandler(this.resizeImageContainer);
            this.Resize += new System.EventHandler(this.tryResizeImageContainer);
            ((System.ComponentModel.ISupportInitialize)(this.mandelImageContainer)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ownColourBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ownColourBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ownColourBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ownColourBox4)).EndInit();
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
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuStripStartList;
        private System.Windows.Forms.ToolStripMenuItem menuStripStartNew;
        private System.Windows.Forms.ToolStripMenuItem menuStripStartSave;
        private System.Windows.Forms.ToolStripMenuItem menuStripStartExit;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.Label colourChoiceLabel;
        private System.Windows.Forms.ComboBox colourChoiceComboBox;
        private System.Windows.Forms.CheckBox ownColourCheckBox;
        private System.Windows.Forms.PictureBox ownColourBox1;
        private System.Windows.Forms.PictureBox ownColourBox2;
        private System.Windows.Forms.PictureBox ownColourBox3;
        private System.Windows.Forms.PictureBox ownColourBox4;
        private System.Windows.Forms.ToolStripMenuItem menuStripHelpList;
        private System.Windows.Forms.ToolStripMenuItem menuStripHelpInformation;
        private System.Windows.Forms.ToolStripMenuItem menuStripHelpAbout;

    }
}

