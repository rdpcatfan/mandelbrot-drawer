using System;
using System.Windows.Forms;
using System.Drawing;

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

        /// <summary>
        /// Initialize the graphical user interface.
        /// </summary>
        private void InitializeComponent()
        {
            this.fractal = new FractalControl();
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
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // fractal
            //
            this.fractal.Paint += this.updateStatusBar;
            this.fractal.Location = new Point(pxPadding, 25);
            this.fractal.Size = this.calcFractalSize();
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
            this.menuStripStartNew.Click += new System.EventHandler(this.fractal.resetImage);
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
            this.saveImageDialog.DefaultExt = "png";
            this.saveImageDialog.FileName = "MandelBrot";
            this.saveImageDialog.Filter = "Bitmap Afbeelding|*.bmp|PNG Afbeelding|*.png|JPeg Afbeelding|*.jpg|Gif Afbeelding|*.gif";
            this.saveImageDialog.Title = "Afbeelding Opslaan";
            // 
            // MainWindow
            // 
            this.AcceptButton = this.fractal.AcceptButton;
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 644);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.fractal);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(this.fractal.MinimumSize.Width + 10, this.fractal.MinimumSize.Height + 30);
            this.ResizeBegin += this.beginResize;
            this.Resize += this.considerResize;
            this.ResizeEnd += this.completeResize;
            this.Name = "MainWindow";
            this.Text = "Mandelbrot Generator";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private FractalControl fractal;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusStripTimeLabel;
        private ToolStripStatusLabel statusStripSizeLabel;
        private MenuStrip menuStrip;
        private ToolStripMenuItem menuStripStartList;
        private ToolStripMenuItem menuStripStartNew;
        private ToolStripMenuItem menuStripStartSave;
        private ToolStripMenuItem menuStripStartExit;
        private SaveFileDialog saveImageDialog;

        private ToolStripMenuItem menuStripHelpList;
        private ToolStripMenuItem menuStripHelpInformation;
        private ToolStripMenuItem menuStripHelpAbout;

    }
}

