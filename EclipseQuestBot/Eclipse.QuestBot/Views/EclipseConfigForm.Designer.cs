namespace Eclipse.Bots.QuestBot
{
    partial class EclipseConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EclipseConfigForm));
            this.chQuestMode = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.btnQuestManager = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // chQuestMode
            // 
            this.chQuestMode.AutoSize = true;
            this.chQuestMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chQuestMode.ForeColor = System.Drawing.Color.DarkViolet;
            this.chQuestMode.Location = new System.Drawing.Point(299, 59);
            this.chQuestMode.Name = "chQuestMode";
            this.chQuestMode.Size = new System.Drawing.Size(90, 17);
            this.chQuestMode.TabIndex = 3;
            this.chQuestMode.Text = "QuestMode";
            this.chQuestMode.UseVisualStyleBackColor = true;
            this.chQuestMode.CheckedChanged += new System.EventHandler(this.chQuestMode_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(242, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(203, 39);
            this.label5.TabIndex = 4;
            this.label5.Text = "ZOMG Quest Mode. You can add quests in real time too. Fancy =P";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(406, 135);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(39, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Data";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnQuestManager
            // 
            this.btnQuestManager.Location = new System.Drawing.Point(248, 135);
            this.btnQuestManager.Name = "btnQuestManager";
            this.btnQuestManager.Size = new System.Drawing.Size(152, 23);
            this.btnQuestManager.TabIndex = 6;
            this.btnQuestManager.Text = "Quest Manager";
            this.btnQuestManager.UseVisualStyleBackColor = true;
            this.btnQuestManager.Click += new System.EventHandler(this.btnQuestManager_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 225);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // EclipseConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 225);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnQuestManager);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chQuestMode);
            this.Name = "EclipseConfigForm";
            this.Text = "Eclipse - QuestBot Configuration";
            this.Load += new System.EventHandler(this.EclipseConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chQuestMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnQuestManager;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

