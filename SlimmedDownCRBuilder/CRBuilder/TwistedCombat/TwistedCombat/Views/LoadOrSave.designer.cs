namespace TwistedCombatRoutines
{
    partial class LoadOrSave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadOrSave));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbLoadRoutine = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCreateNEW = new System.Windows.Forms.Button();
            this.LbRoutines = new System.Windows.Forms.ListBox();
            this.cbSpecs = new System.Windows.Forms.ComboBox();
            this.cbClasses = new System.Windows.Forms.ComboBox();
            this.cbPVERoutine = new System.Windows.Forms.CheckBox();
            this.cbPVPRoutine = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbRoutineName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(424, 185);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // tbLoadRoutine
            // 
            this.tbLoadRoutine.BackColor = System.Drawing.Color.Black;
            this.tbLoadRoutine.ForeColor = System.Drawing.Color.White;
            this.tbLoadRoutine.Location = new System.Drawing.Point(12, 314);
            this.tbLoadRoutine.Name = "tbLoadRoutine";
            this.tbLoadRoutine.Size = new System.Drawing.Size(191, 23);
            this.tbLoadRoutine.TabIndex = 11;
            this.tbLoadRoutine.Text = "Load Routine";
            this.tbLoadRoutine.UseVisualStyleBackColor = false;
            this.tbLoadRoutine.Click += new System.EventHandler(this.tbAddToRoutine_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(12, 343);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Import Purchased Routine";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnCreateNEW
            // 
            this.btnCreateNEW.BackColor = System.Drawing.Color.Black;
            this.btnCreateNEW.ForeColor = System.Drawing.Color.White;
            this.btnCreateNEW.Location = new System.Drawing.Point(209, 314);
            this.btnCreateNEW.Name = "btnCreateNEW";
            this.btnCreateNEW.Size = new System.Drawing.Size(208, 23);
            this.btnCreateNEW.TabIndex = 11;
            this.btnCreateNEW.Text = "Create New Routine";
            this.btnCreateNEW.UseVisualStyleBackColor = false;
            this.btnCreateNEW.Click += new System.EventHandler(this.btnCreateNEW_Click);
            // 
            // LbRoutines
            // 
            this.LbRoutines.FormattingEnabled = true;
            this.LbRoutines.Location = new System.Drawing.Point(12, 213);
            this.LbRoutines.Name = "LbRoutines";
            this.LbRoutines.Size = new System.Drawing.Size(191, 95);
            this.LbRoutines.TabIndex = 12;
            // 
            // cbSpecs
            // 
            this.cbSpecs.FormattingEnabled = true;
            this.cbSpecs.Location = new System.Drawing.Point(297, 264);
            this.cbSpecs.Name = "cbSpecs";
            this.cbSpecs.Size = new System.Drawing.Size(120, 21);
            this.cbSpecs.TabIndex = 19;
            // 
            // cbClasses
            // 
            this.cbClasses.FormattingEnabled = true;
            this.cbClasses.Location = new System.Drawing.Point(297, 240);
            this.cbClasses.Name = "cbClasses";
            this.cbClasses.Size = new System.Drawing.Size(120, 21);
            this.cbClasses.TabIndex = 20;
            // 
            // cbPVERoutine
            // 
            this.cbPVERoutine.AutoSize = true;
            this.cbPVERoutine.ForeColor = System.Drawing.Color.White;
            this.cbPVERoutine.Location = new System.Drawing.Point(213, 296);
            this.cbPVERoutine.Name = "cbPVERoutine";
            this.cbPVERoutine.Size = new System.Drawing.Size(81, 17);
            this.cbPVERoutine.TabIndex = 17;
            this.cbPVERoutine.Text = "Use In PVE";
            this.cbPVERoutine.UseVisualStyleBackColor = true;
            // 
            // cbPVPRoutine
            // 
            this.cbPVPRoutine.AutoSize = true;
            this.cbPVPRoutine.ForeColor = System.Drawing.Color.White;
            this.cbPVPRoutine.Location = new System.Drawing.Point(297, 296);
            this.cbPVPRoutine.Name = "cbPVPRoutine";
            this.cbPVPRoutine.Size = new System.Drawing.Size(81, 17);
            this.cbPVPRoutine.TabIndex = 18;
            this.cbPVPRoutine.Text = "Use In PVP";
            this.cbPVPRoutine.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(210, 267);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Character Spec";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(210, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Character Class";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(209, 195);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Rotation Name";
            // 
            // tbRoutineName
            // 
            this.tbRoutineName.Location = new System.Drawing.Point(209, 214);
            this.tbRoutineName.Name = "tbRoutineName";
            this.tbRoutineName.Size = new System.Drawing.Size(208, 20);
            this.tbRoutineName.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Previously Created Routines";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Black;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(12, 372);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(191, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Export Routine";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Black;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(12, 401);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(191, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Export Routine For Resale";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // LoadOrSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(424, 436);
            this.Controls.Add(this.cbSpecs);
            this.Controls.Add(this.cbClasses);
            this.Controls.Add(this.cbPVERoutine);
            this.Controls.Add(this.cbPVPRoutine);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbRoutineName);
            this.Controls.Add(this.LbRoutines);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCreateNEW);
            this.Controls.Add(this.tbLoadRoutine);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LoadOrSave";
            this.Text = "LoadOrSave";
            this.Load += new System.EventHandler(this.LoadOrSave_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button tbLoadRoutine;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCreateNEW;
        private System.Windows.Forms.ListBox LbRoutines;
        private System.Windows.Forms.ComboBox cbSpecs;
        private System.Windows.Forms.ComboBox cbClasses;
        private System.Windows.Forms.CheckBox cbPVERoutine;
        private System.Windows.Forms.CheckBox cbPVPRoutine;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbRoutineName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}