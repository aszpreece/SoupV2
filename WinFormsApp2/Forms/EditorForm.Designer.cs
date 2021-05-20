
namespace SoupForm.Forms
{
    partial class EditorForm
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
            soupGraphicControl1.ShouldUpdate = false;
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.editPanel = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.soupGraphicControl1 = new SoupForm.Controls.SoupGraphicControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileButtonItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEntityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBuiltIn = new System.Windows.Forms.ToolStripMenuItem();
            this.openEntityFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveEntityFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Indent = 5;
            this.treeView1.Location = new System.Drawing.Point(0, 24);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.Size = new System.Drawing.Size(257, 552);
            this.treeView1.TabIndex = 0;
            this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
            // 
            // editPanel
            // 
            this.editPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.editPanel.Location = new System.Drawing.Point(257, 384);
            this.editPanel.Name = "editPanel";
            this.editPanel.Size = new System.Drawing.Size(712, 192);
            this.editPanel.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(257, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 360);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(260, 381);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(709, 3);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // soupGraphicControl1
            // 
            this.soupGraphicControl1.CurrentSimulation = null;
            this.soupGraphicControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soupGraphicControl1.Location = new System.Drawing.Point(260, 24);
            this.soupGraphicControl1.MouseHoverUpdatesOnly = false;
            this.soupGraphicControl1.Name = "soupGraphicControl1";
            this.soupGraphicControl1.ShouldUpdate = true;
            this.soupGraphicControl1.Size = new System.Drawing.Size(709, 357);
            this.soupGraphicControl1.TabIndex = 4;
            this.soupGraphicControl1.Text = "soupGraphicControl1";
            this.soupGraphicControl1.OnMouseWheelUpwards += new MonoGame.Forms.Controls.GraphicsDeviceControl.MouseWheelUpwardsEvent(this.soupGraphicsControl_OnMouseWheelUpwards);
            this.soupGraphicControl1.OnMouseWheelDownwards += new MonoGame.Forms.Controls.GraphicsDeviceControl.MouseWheelDownwardsEvent(this.soupGraphicsControl_OnMouseWheelDownwards);
            this.soupGraphicControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.soupGraphicsControl_MouseDown);
            this.soupGraphicControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.soupGraphicsControl_MouseMove);
            this.soupGraphicControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.soupGraphicsControl_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(260, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(709, 10);
            this.panel2.TabIndex = 5;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileButtonItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(969, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileButtonItem
            // 
            this.fileButtonItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openEntityMenuItem,
            this.saveMenuItem,
            this.openBuiltIn});
            this.fileButtonItem.Name = "fileButtonItem";
            this.fileButtonItem.Size = new System.Drawing.Size(37, 20);
            this.fileButtonItem.Text = "File";
            // 
            // openEntityMenuItem
            // 
            this.openEntityMenuItem.Name = "openEntityMenuItem";
            this.openEntityMenuItem.Size = new System.Drawing.Size(173, 22);
            this.openEntityMenuItem.Text = "Open Entity";
            this.openEntityMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(173, 22);
            this.saveMenuItem.Text = "Save Entity";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // openBuiltIn
            // 
            this.openBuiltIn.Name = "openBuiltIn";
            this.openBuiltIn.Size = new System.Drawing.Size(173, 22);
            this.openBuiltIn.Text = "Open Builtin Entity";
            // 
            // openEntityFileDialog
            // 
            this.openEntityFileDialog.FileName = "openFileDialog1";
            // 
            // saveEntityFileDialog
            // 
            this.saveEntityFileDialog.DefaultExt = "entityd";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 576);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.soupGraphicControl1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.editPanel);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel editPanel;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private SoupForm.Controls.SoupGraphicControl soupGraphicControl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileButtonItem;
        private System.Windows.Forms.ToolStripMenuItem openEntityMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.OpenFileDialog openEntityFileDialog;
        private System.Windows.Forms.SaveFileDialog saveEntityFileDialog;
        private System.Windows.Forms.ToolStripMenuItem openBuiltIn;
    }
}