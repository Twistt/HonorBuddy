namespace SkinbotV2.Views
{
    partial class SkinningManagement
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
            this.lbMobs = new System.Windows.Forms.ListBox();
            this.btnAddTargeted = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbMobs
            // 
            this.lbMobs.FormattingEnabled = true;
            this.lbMobs.Location = new System.Drawing.Point(12, 15);
            this.lbMobs.Name = "lbMobs";
            this.lbMobs.Size = new System.Drawing.Size(216, 199);
            this.lbMobs.TabIndex = 0;
            // 
            // btnAddTargeted
            // 
            this.btnAddTargeted.Location = new System.Drawing.Point(12, 226);
            this.btnAddTargeted.Name = "btnAddTargeted";
            this.btnAddTargeted.Size = new System.Drawing.Size(109, 23);
            this.btnAddTargeted.TabIndex = 1;
            this.btnAddTargeted.Text = "Add Targeted Mob";
            this.btnAddTargeted.UseVisualStyleBackColor = true;
            this.btnAddTargeted.Click += new System.EventHandler(this.btnAddTargeted_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Location = new System.Drawing.Point(133, 226);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(95, 23);
            this.btnRemoveSelected.TabIndex = 1;
            this.btnRemoveSelected.Text = "Remove Mob";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // SkinningManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 261);
            this.Controls.Add(this.btnRemoveSelected);
            this.Controls.Add(this.btnAddTargeted);
            this.Controls.Add(this.lbMobs);
            this.Name = "SkinningManagement";
            this.Text = "SkinningManagement";
            this.Load += new System.EventHandler(this.SkinningManagement_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbMobs;
        private System.Windows.Forms.Button btnAddTargeted;
        private System.Windows.Forms.Button btnRemoveSelected;
    }
}