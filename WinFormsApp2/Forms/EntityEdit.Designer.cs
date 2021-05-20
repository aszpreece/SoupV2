
namespace SoupForm.Forms
{
    partial class EntityEdit
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
            this.tagTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.addComponentButton = new System.Windows.Forms.Button();
            this.deleteComponentButton = new System.Windows.Forms.Button();
            this.trySetTagButton = new System.Windows.Forms.Button();
            this.tryDeleteEntityButton = new System.Windows.Forms.Button();
            this.addChildEntityButton = new System.Windows.Forms.Button();
            this.addChildFromFile = new System.Windows.Forms.Button();
            this.addChildFromBuiltin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tagTextBox
            // 
            this.tagTextBox.Location = new System.Drawing.Point(44, 4);
            this.tagTextBox.Name = "tagTextBox";
            this.tagTextBox.Size = new System.Drawing.Size(131, 23);
            this.tagTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tag:";
            // 
            // addComponentButton
            // 
            this.addComponentButton.Location = new System.Drawing.Point(0, 33);
            this.addComponentButton.Name = "addComponentButton";
            this.addComponentButton.Size = new System.Drawing.Size(175, 23);
            this.addComponentButton.TabIndex = 2;
            this.addComponentButton.Text = "Add Component";
            this.addComponentButton.UseVisualStyleBackColor = true;
            this.addComponentButton.Click += new System.EventHandler(this.addComponentButton_Click);
            // 
            // deleteComponentButton
            // 
            this.deleteComponentButton.Location = new System.Drawing.Point(181, 33);
            this.deleteComponentButton.Name = "deleteComponentButton";
            this.deleteComponentButton.Size = new System.Drawing.Size(175, 23);
            this.deleteComponentButton.TabIndex = 3;
            this.deleteComponentButton.Text = "Delete Component";
            this.deleteComponentButton.UseVisualStyleBackColor = true;
            this.deleteComponentButton.Click += new System.EventHandler(this.deleteComponentButton_Click_1);
            // 
            // trySetTagButton
            // 
            this.trySetTagButton.Location = new System.Drawing.Point(181, 4);
            this.trySetTagButton.Name = "trySetTagButton";
            this.trySetTagButton.Size = new System.Drawing.Size(175, 23);
            this.trySetTagButton.TabIndex = 4;
            this.trySetTagButton.Text = "Set Tag";
            this.trySetTagButton.UseVisualStyleBackColor = true;
            this.trySetTagButton.Click += new System.EventHandler(this.trySetTagButton_Click);
            // 
            // tryDeleteEntityButton
            // 
            this.tryDeleteEntityButton.Location = new System.Drawing.Point(181, 62);
            this.tryDeleteEntityButton.Name = "tryDeleteEntityButton";
            this.tryDeleteEntityButton.Size = new System.Drawing.Size(175, 23);
            this.tryDeleteEntityButton.TabIndex = 5;
            this.tryDeleteEntityButton.Text = "Delete Entity";
            this.tryDeleteEntityButton.UseVisualStyleBackColor = true;
            this.tryDeleteEntityButton.Click += new System.EventHandler(this.tryDeleteEntityButton_Click);
            // 
            // addChildEntityButton
            // 
            this.addChildEntityButton.Location = new System.Drawing.Point(0, 62);
            this.addChildEntityButton.Name = "addChildEntityButton";
            this.addChildEntityButton.Size = new System.Drawing.Size(175, 23);
            this.addChildEntityButton.TabIndex = 6;
            this.addChildEntityButton.Text = "Add Child Entity";
            this.addChildEntityButton.UseVisualStyleBackColor = true;
            this.addChildEntityButton.Click += new System.EventHandler(this.addChildEntityButton_Click);
            // 
            // addChildFromFile
            // 
            this.addChildFromFile.Location = new System.Drawing.Point(0, 91);
            this.addChildFromFile.Name = "addChildFromFile";
            this.addChildFromFile.Size = new System.Drawing.Size(175, 23);
            this.addChildFromFile.TabIndex = 7;
            this.addChildFromFile.Text = "Add Child Entity From File";
            this.addChildFromFile.UseVisualStyleBackColor = true;
            // 
            // addChildFromBuiltin
            // 
            this.addChildFromBuiltin.Location = new System.Drawing.Point(181, 91);
            this.addChildFromBuiltin.Name = "addChildFromBuiltin";
            this.addChildFromBuiltin.Size = new System.Drawing.Size(175, 23);
            this.addChildFromBuiltin.TabIndex = 8;
            this.addChildFromBuiltin.Text = "Add Child Entity From Builtin";
            this.addChildFromBuiltin.UseVisualStyleBackColor = true;
            // 
            // EntityEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addChildFromBuiltin);
            this.Controls.Add(this.addChildFromFile);
            this.Controls.Add(this.addChildEntityButton);
            this.Controls.Add(this.tryDeleteEntityButton);
            this.Controls.Add(this.trySetTagButton);
            this.Controls.Add(this.deleteComponentButton);
            this.Controls.Add(this.addComponentButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tagTextBox);
            this.Name = "EntityEdit";
            this.Size = new System.Drawing.Size(553, 140);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tagTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addComponentButton;
        private System.Windows.Forms.Button deleteComponentButton;
        private System.Windows.Forms.Button trySetTagButton;
        private System.Windows.Forms.Button tryDeleteEntityButton;
        private System.Windows.Forms.Button addChildEntityButton;
        private System.Windows.Forms.Button addChildFromFile;
        private System.Windows.Forms.Button addChildFromBuiltin;
    }
}
