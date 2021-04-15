namespace SoupForm.Forms
{
    partial class NewExperimentForm
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
            this.saveSettingsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveStatsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.experimentNameBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.loadSettingsButton = new System.Windows.Forms.Button();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.saveStatsCheckBox = new System.Windows.Forms.CheckBox();
            this.saveStatsButton = new System.Windows.Forms.Button();
            this.statsFileLocation = new System.Windows.Forms.Label();
            this.openSettingsDialog = new System.Windows.Forms.OpenFileDialog();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.settingPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.selectDefinitionFolderButton = new System.Windows.Forms.Button();
            this.entityDFolderLabel = new System.Windows.Forms.Label();
            this.selectEntityDFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveSettingsFileDialog
            // 
            this.saveSettingsFileDialog.DefaultExt = "soupsettings";
            // 
            // saveStatsFileDialog
            // 
            this.saveStatsFileDialog.DefaultExt = "soupstats";
            // 
            // experimentNameBox
            // 
            this.experimentNameBox.Location = new System.Drawing.Point(111, 6);
            this.experimentNameBox.Multiline = false;
            this.experimentNameBox.Name = "experimentNameBox";
            this.experimentNameBox.Size = new System.Drawing.Size(145, 21);
            this.experimentNameBox.TabIndex = 1;
            this.experimentNameBox.Text = "";
            this.experimentNameBox.TextChanged += new System.EventHandler(this.experimentNameBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Experiment Name:";
            // 
            // loadSettingsButton
            // 
            this.loadSettingsButton.Location = new System.Drawing.Point(12, 10);
            this.loadSettingsButton.Name = "loadSettingsButton";
            this.loadSettingsButton.Size = new System.Drawing.Size(102, 23);
            this.loadSettingsButton.TabIndex = 3;
            this.loadSettingsButton.Text = "Load Settings...";
            this.loadSettingsButton.UseVisualStyleBackColor = true;
            this.loadSettingsButton.Click += new System.EventHandler(this.loadSettingsButton_Click);
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveSettingsButton.Location = new System.Drawing.Point(267, 10);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(102, 23);
            this.saveSettingsButton.TabIndex = 4;
            this.saveSettingsButton.Text = "Save Settings...";
            this.saveSettingsButton.UseVisualStyleBackColor = true;
            this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
            // 
            // saveStatsCheckBox
            // 
            this.saveStatsCheckBox.AutoSize = true;
            this.saveStatsCheckBox.Location = new System.Drawing.Point(3, 74);
            this.saveStatsCheckBox.Name = "saveStatsCheckBox";
            this.saveStatsCheckBox.Size = new System.Drawing.Size(99, 19);
            this.saveStatsCheckBox.TabIndex = 5;
            this.saveStatsCheckBox.Text = "Save Statistics";
            this.saveStatsCheckBox.UseVisualStyleBackColor = true;
            this.saveStatsCheckBox.CheckedChanged += new System.EventHandler(this.saveStatsCheckBox_CheckedChanged);
            // 
            // saveStatsButton
            // 
            this.saveStatsButton.Location = new System.Drawing.Point(3, 99);
            this.saveStatsButton.Name = "saveStatsButton";
            this.saveStatsButton.Size = new System.Drawing.Size(118, 23);
            this.saveStatsButton.TabIndex = 6;
            this.saveStatsButton.Text = "Save Statistics File...";
            this.saveStatsButton.UseVisualStyleBackColor = true;
            this.saveStatsButton.Click += new System.EventHandler(this.saveStatsButton_Click_1);
            // 
            // statsFileLocation
            // 
            this.statsFileLocation.AutoSize = true;
            this.statsFileLocation.Location = new System.Drawing.Point(127, 103);
            this.statsFileLocation.Name = "statsFileLocation";
            this.statsFileLocation.Size = new System.Drawing.Size(61, 15);
            this.statsFileLocation.TabIndex = 7;
            this.statsFileLocation.Text = "[Location]";
            // 
            // openSettingsDialog
            // 
            this.openSettingsDialog.FileName = "openSettingsDialog";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.Location = new System.Drawing.Point(27, 65);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(279, 67);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.entityDFolderLabel);
            this.panel1.Controls.Add(this.selectDefinitionFolderButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.experimentNameBox);
            this.panel1.Controls.Add(this.saveStatsCheckBox);
            this.panel1.Controls.Add(this.statsFileLocation);
            this.panel1.Controls.Add(this.saveStatsButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 128);
            this.panel1.TabIndex = 10;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.loadSettingsButton);
            this.panel2.Controls.Add(this.saveSettingsButton);
            this.panel2.Controls.Add(this.cancelButton);
            this.panel2.Controls.Add(this.okButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 430);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(410, 100);
            this.panel2.TabIndex = 11;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 128);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(410, 3);
            this.splitter1.TabIndex = 12;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 427);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(410, 3);
            this.splitter2.TabIndex = 13;
            this.splitter2.TabStop = false;
            // 
            // settingPropertyGrid
            // 
            this.settingPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingPropertyGrid.Location = new System.Drawing.Point(0, 131);
            this.settingPropertyGrid.Name = "settingPropertyGrid";
            this.settingPropertyGrid.Size = new System.Drawing.Size(410, 296);
            this.settingPropertyGrid.TabIndex = 14;
            // 
            // selectDefinitionFolderButton
            // 
            this.selectDefinitionFolderButton.Location = new System.Drawing.Point(3, 33);
            this.selectDefinitionFolderButton.Name = "selectDefinitionFolderButton";
            this.selectDefinitionFolderButton.Size = new System.Drawing.Size(184, 23);
            this.selectDefinitionFolderButton.TabIndex = 8;
            this.selectDefinitionFolderButton.Text = "Select Entity Definition Folder...";
            this.selectDefinitionFolderButton.UseVisualStyleBackColor = true;
            this.selectDefinitionFolderButton.Click += new System.EventHandler(this.selectDefinitionFolderButton_Click);
            // 
            // entityDFolderLabel
            // 
            this.entityDFolderLabel.AutoSize = true;
            this.entityDFolderLabel.Location = new System.Drawing.Point(195, 37);
            this.entityDFolderLabel.Name = "entityDFolderLabel";
            this.entityDFolderLabel.Size = new System.Drawing.Size(61, 15);
            this.entityDFolderLabel.TabIndex = 9;
            this.entityDFolderLabel.Text = "[Location]";
            // 
            // NewExperimentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 530);
            this.Controls.Add(this.settingPropertyGrid);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "NewExperimentForm";
            this.Text = "New Experiment";
            this.Load += new System.EventHandler(this.NewExperiment_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveSettingsFileDialog;
        private System.Windows.Forms.SaveFileDialog saveStatsFileDialog;
        private System.Windows.Forms.RichTextBox experimentNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loadSettingsButton;
        private System.Windows.Forms.Button saveSettingsButton;
        private System.Windows.Forms.CheckBox saveStatsCheckBox;
        private System.Windows.Forms.Button saveStatsButton;
        private System.Windows.Forms.Label statsFileLocation;
        private System.Windows.Forms.OpenFileDialog openSettingsDialog;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.PropertyGrid settingPropertyGrid;
        private System.Windows.Forms.Label entityDFolderLabel;
        private System.Windows.Forms.Button selectDefinitionFolderButton;
        private System.Windows.Forms.FolderBrowserDialog selectEntityDFolderDialog;
    }
}