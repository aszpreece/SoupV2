
namespace SoupForm.Forms
{
    partial class SimulationTabContent
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "",
            "",
            ""}, -1);
            this.eventView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.eventTypeHeader = new System.Windows.Forms.ColumnHeader();
            this.eventLocationHeader = new System.Windows.Forms.ColumnHeader();
            this.eventInfoHeader = new System.Windows.Forms.ColumnHeader();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.soupGraphicsControl = new SoupForm.Controls.SoupGraphicControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // eventView
            // 
            this.eventView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.eventTypeHeader,
            this.eventLocationHeader,
            this.eventInfoHeader});
            this.eventView.Dock = System.Windows.Forms.DockStyle.Left;
            this.eventView.FullRowSelect = true;
            this.eventView.GridLines = true;
            this.eventView.HideSelection = false;
            this.eventView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.eventView.Location = new System.Drawing.Point(0, 0);
            this.eventView.MultiSelect = false;
            this.eventView.Name = "eventView";
            this.eventView.Size = new System.Drawing.Size(121, 377);
            this.eventView.TabIndex = 2;
            this.eventView.UseCompatibleStateImageBehavior = false;
            this.eventView.DoubleClick += new System.EventHandler(this.eventView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.DisplayIndex = 3;
            this.columnHeader1.Text = "Tick";
            // 
            // eventTypeHeader
            // 
            this.eventTypeHeader.DisplayIndex = 0;
            this.eventTypeHeader.Text = "Event";
            // 
            // eventLocationHeader
            // 
            this.eventLocationHeader.DisplayIndex = 1;
            this.eventLocationHeader.Text = "Location";
            // 
            // eventInfoHeader
            // 
            this.eventInfoHeader.DisplayIndex = 2;
            this.eventInfoHeader.Text = "Info";
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(0, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 480);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 380);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(590, 100);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(3, 377);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(590, 3);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitter3);
            this.panel1.Controls.Add(this.soupGraphicsControl);
            this.panel1.Controls.Add(this.eventView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(590, 377);
            this.panel1.TabIndex = 9;
            // 
            // splitter3
            // 
            this.splitter3.Location = new System.Drawing.Point(121, 0);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(3, 377);
            this.splitter3.TabIndex = 7;
            this.splitter3.TabStop = false;
            // 
            // soupGraphicsControl
            // 
            this.soupGraphicsControl.CurrentSimulation = null;
            this.soupGraphicsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soupGraphicsControl.Location = new System.Drawing.Point(121, 0);
            this.soupGraphicsControl.MouseHoverUpdatesOnly = false;
            this.soupGraphicsControl.Name = "soupGraphicsControl";
            this.soupGraphicsControl.ShouldUpdate = true;
            this.soupGraphicsControl.Size = new System.Drawing.Size(469, 377);
            this.soupGraphicsControl.TabIndex = 6;
            this.soupGraphicsControl.Text = "soupGraphicsControl";
            this.soupGraphicsControl.OnMouseWheelUpwards += new MonoGame.Forms.Controls.GraphicsDeviceControl.MouseWheelUpwardsEvent(this.soupGraphicsControl_OnMouseWheelUpwards);
            this.soupGraphicsControl.OnMouseWheelDownwards += new MonoGame.Forms.Controls.GraphicsDeviceControl.MouseWheelDownwardsEvent(this.soupGraphicsControl_OnMouseWheelDownwards);
            this.soupGraphicsControl.Click += new System.EventHandler(this.soupGraphicsControl_Click);
            this.soupGraphicsControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.soupGraphicsControl_MouseDown);
            this.soupGraphicsControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.soupGraphicsControl_MouseMove);
            this.soupGraphicsControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.soupGraphicsControl_MouseUp);
            // 
            // SimulationTabContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.splitter2);
            this.Name = "SimulationTabContent";
            this.Size = new System.Drawing.Size(593, 480);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView eventView;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ColumnHeader eventTypeHeader;
        private System.Windows.Forms.ColumnHeader eventLocationHeader;
        private System.Windows.Forms.ColumnHeader eventInfoHeader;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter3;
        private SoupForm.Controls.SoupGraphicControl soupGraphicsControl;
    }
}
