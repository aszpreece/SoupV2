
namespace SoupForm.Forms
{
    partial class BrainEdit
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.inputComboBox = new System.Windows.Forms.ComboBox();
            this.addNewInputButton = new System.Windows.Forms.Button();
            this.deleteInputButton = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.outputComboBox = new System.Windows.Forms.ComboBox();
            this.addNewOuputButton = new System.Windows.Forms.Button();
            this.deleteSelectedOutput = new System.Windows.Forms.Button();
            this.inputList = new System.Windows.Forms.ListView();
            this.outputList = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.inputList);
            this.panel1.Controls.Add(this.splitter2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 325);
            this.panel1.TabIndex = 0;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 222);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(172, 3);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.inputComboBox);
            this.panel3.Controls.Add(this.addNewInputButton);
            this.panel3.Controls.Add(this.deleteInputButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 225);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(172, 100);
            this.panel3.TabIndex = 0;
            // 
            // inputComboBox
            // 
            this.inputComboBox.FormattingEnabled = true;
            this.inputComboBox.Location = new System.Drawing.Point(11, 35);
            this.inputComboBox.Name = "inputComboBox";
            this.inputComboBox.Size = new System.Drawing.Size(152, 23);
            this.inputComboBox.TabIndex = 2;
            // 
            // addNewInputButton
            // 
            this.addNewInputButton.Location = new System.Drawing.Point(11, 64);
            this.addNewInputButton.Name = "addNewInputButton";
            this.addNewInputButton.Size = new System.Drawing.Size(152, 23);
            this.addNewInputButton.TabIndex = 1;
            this.addNewInputButton.Text = "Add New Input";
            this.addNewInputButton.UseVisualStyleBackColor = true;
            this.addNewInputButton.Click += new System.EventHandler(this.addNewInputButton_Click);
            // 
            // deleteInputButton
            // 
            this.deleteInputButton.Location = new System.Drawing.Point(11, 6);
            this.deleteInputButton.Name = "deleteInputButton";
            this.deleteInputButton.Size = new System.Drawing.Size(152, 23);
            this.deleteInputButton.TabIndex = 0;
            this.deleteInputButton.Text = "Delete Selected Input";
            this.deleteInputButton.UseVisualStyleBackColor = true;
            this.deleteInputButton.Click += new System.EventHandler(this.deleteInputButton_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(172, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 325);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.outputList);
            this.panel2.Controls.Add(this.splitter3);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(175, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(193, 325);
            this.panel2.TabIndex = 2;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter3.Location = new System.Drawing.Point(0, 222);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(193, 3);
            this.splitter3.TabIndex = 1;
            this.splitter3.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.outputComboBox);
            this.panel4.Controls.Add(this.addNewOuputButton);
            this.panel4.Controls.Add(this.deleteSelectedOutput);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 225);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(193, 100);
            this.panel4.TabIndex = 0;
            // 
            // outputComboBox
            // 
            this.outputComboBox.FormattingEnabled = true;
            this.outputComboBox.Location = new System.Drawing.Point(23, 35);
            this.outputComboBox.Name = "outputComboBox";
            this.outputComboBox.Size = new System.Drawing.Size(152, 23);
            this.outputComboBox.TabIndex = 3;
            // 
            // addNewOuputButton
            // 
            this.addNewOuputButton.Location = new System.Drawing.Point(23, 64);
            this.addNewOuputButton.Name = "addNewOuputButton";
            this.addNewOuputButton.Size = new System.Drawing.Size(152, 23);
            this.addNewOuputButton.TabIndex = 2;
            this.addNewOuputButton.Text = "Add New Output";
            this.addNewOuputButton.UseVisualStyleBackColor = true;
            this.addNewOuputButton.Click += new System.EventHandler(this.addNewOuputButton_Click);
            // 
            // deleteSelectedOutput
            // 
            this.deleteSelectedOutput.Location = new System.Drawing.Point(23, 6);
            this.deleteSelectedOutput.Name = "deleteSelectedOutput";
            this.deleteSelectedOutput.Size = new System.Drawing.Size(152, 23);
            this.deleteSelectedOutput.TabIndex = 1;
            this.deleteSelectedOutput.Text = "Delete Selected Output";
            this.deleteSelectedOutput.UseVisualStyleBackColor = true;
            this.deleteSelectedOutput.Click += new System.EventHandler(this.deleteSelectedOutput_Click);
            // 
            // inputList
            // 
            this.inputList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputList.HideSelection = false;
            this.inputList.Location = new System.Drawing.Point(0, 0);
            this.inputList.Name = "inputList";
            this.inputList.Size = new System.Drawing.Size(172, 222);
            this.inputList.TabIndex = 2;
            this.inputList.UseCompatibleStateImageBehavior = false;
            // 
            // outputList
            // 
            this.outputList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputList.HideSelection = false;
            this.outputList.Location = new System.Drawing.Point(0, 0);
            this.outputList.Name = "outputList";
            this.outputList.Size = new System.Drawing.Size(193, 222);
            this.outputList.TabIndex = 2;
            this.outputList.UseCompatibleStateImageBehavior = false;
            // 
            // BrainEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "BrainEdit";
            this.Size = new System.Drawing.Size(368, 325);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button addNewInputButton;
        private System.Windows.Forms.Button deleteInputButton;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button addNewOuputButton;
        private System.Windows.Forms.Button deleteSelectedOutput;
        private System.Windows.Forms.ComboBox inputComboBox;
        private System.Windows.Forms.ComboBox outputComboBox;
        private System.Windows.Forms.ListView inputList;
        private System.Windows.Forms.ListView outputList;
    }
}
