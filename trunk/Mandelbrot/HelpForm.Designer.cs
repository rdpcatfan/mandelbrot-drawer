namespace Mandelbrot
{
    partial class HelpForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Nieuw");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Afbeelding opslaan");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Afsluiten");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Start menu", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Midden X en Y");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Schaal");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Iteraties");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Kleuren");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Invoeropties", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Klikken");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Scrollen");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Slepen");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Muis", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Mandelbrot Generator", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode9,
            treeNode13});
            this.helpTreeView = new System.Windows.Forms.TreeView();
            this.helpRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // helpTreeView
            // 
            this.helpTreeView.FullRowSelect = true;
            this.helpTreeView.Location = new System.Drawing.Point(12, 12);
            this.helpTreeView.Name = "helpTreeView";
            treeNode1.Name = "nodeReset";
            treeNode1.Text = "Nieuw";
            treeNode2.Name = "nodeSave";
            treeNode2.Text = "Afbeelding opslaan";
            treeNode3.Name = "nodeClose";
            treeNode3.Text = "Afsluiten";
            treeNode4.Name = "nodeStart";
            treeNode4.Text = "Start menu";
            treeNode5.Name = "nodeCentreXY";
            treeNode5.Text = "Midden X en Y";
            treeNode6.Name = "nodeScale";
            treeNode6.Text = "Schaal";
            treeNode7.Name = "nodeIterations";
            treeNode7.Text = "Iteraties";
            treeNode8.Name = "nodeColours";
            treeNode8.Text = "Kleuren";
            treeNode9.Name = "nodeUserInput";
            treeNode9.Text = "Invoeropties";
            treeNode10.Name = "nodeMouseClick";
            treeNode10.Text = "Klikken";
            treeNode11.Name = "nodeScroll";
            treeNode11.Text = "Scrollen";
            treeNode12.Name = "nodeDrag";
            treeNode12.Text = "Slepen";
            treeNode13.Name = "nodeMouse";
            treeNode13.Text = "Muis";
            treeNode14.Name = "Root";
            treeNode14.Text = "Mandelbrot Generator";
            this.helpTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode14});
            this.helpTreeView.PathSeparator = " - ";
            this.helpTreeView.Size = new System.Drawing.Size(182, 272);
            this.helpTreeView.TabIndex = 0;
            this.helpTreeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.showHelpInformation);
            // 
            // helpRichTextBox
            // 
            this.helpRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.helpRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.helpRichTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.helpRichTextBox.Location = new System.Drawing.Point(201, 12);
            this.helpRichTextBox.Name = "helpRichTextBox";
            this.helpRichTextBox.ReadOnly = true;
            this.helpRichTextBox.Size = new System.Drawing.Size(503, 272);
            this.helpRichTextBox.TabIndex = 1;
            this.helpRichTextBox.Text = "";
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 296);
            this.Controls.Add(this.helpRichTextBox);
            this.Controls.Add(this.helpTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "HelpForm";
            this.Text = "Mandelbrot Generator - Help";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView helpTreeView;
        private System.Windows.Forms.RichTextBox helpRichTextBox;
    }
}