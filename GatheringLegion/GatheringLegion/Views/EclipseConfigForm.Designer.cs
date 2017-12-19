namespace Eclipse.Bots.GatheringLegion
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
            this.pbEclipse = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbAvoidElites = new System.Windows.Forms.CheckBox();
            this.cbDontFightWhileMounted = new System.Windows.Forms.CheckBox();
            this.cbGatherChests = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tbAvoidSearch = new System.Windows.Forms.TextBox();
            this.lbAvoidZones = new System.Windows.Forms.ListBox();
            this.lbAvoids = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.cbGatherMana = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbEclipse)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbEclipse
            // 
            this.pbEclipse.Location = new System.Drawing.Point(0, 0);
            this.pbEclipse.Name = "pbEclipse";
            this.pbEclipse.Size = new System.Drawing.Size(358, 153);
            this.pbEclipse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEclipse.TabIndex = 6;
            this.pbEclipse.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.DarkViolet;
            this.checkBox1.Location = new System.Drawing.Point(188, 159);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(107, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Training Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(186, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 50);
            this.label1.TabIndex = 4;
            this.label1.Text = "LEARNING MODE ONLY - This is how you Train the Artificial Intelligence (primarily" +
    " for skinning). ";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.ForeColor = System.Drawing.Color.DarkViolet;
            this.checkBox2.Location = new System.Drawing.Point(10, 161);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(163, 17);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "AI Mode (recommended)";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 63);
            this.label2.TabIndex = 4;
            this.label2.Text = "the bot will run around and gather in an area - it will remember all noes it has " +
    "seen. And re-find them in a random order.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(19, 272);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(81, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Support Thread";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(130, 272);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(72, 13);
            this.linkLabel2.TabIndex = 5;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Video Tutorial";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(236, 272);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(83, 13);
            this.linkLabel3.TabIndex = 5;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Discord Support";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 246);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(204, 278);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "How";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.flowLayoutPanel3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(198, 272);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gathering Settings";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoScroll = true;
            this.flowLayoutPanel3.Controls.Add(this.cbAvoidElites);
            this.flowLayoutPanel3.Controls.Add(this.cbDontFightWhileMounted);
            this.flowLayoutPanel3.Controls.Add(this.cbGatherChests);
            this.flowLayoutPanel3.Controls.Add(this.cbGatherMana);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(2, 15);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(194, 255);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // cbAvoidElites
            // 
            this.cbAvoidElites.AutoSize = true;
            this.cbAvoidElites.Location = new System.Drawing.Point(3, 3);
            this.cbAvoidElites.Name = "cbAvoidElites";
            this.cbAvoidElites.Size = new System.Drawing.Size(81, 17);
            this.cbAvoidElites.TabIndex = 0;
            this.cbAvoidElites.Text = "Avoid Elites";
            this.cbAvoidElites.UseVisualStyleBackColor = true;
            this.cbAvoidElites.CheckedChanged += new System.EventHandler(this.cbAvoidElites_CheckedChanged);
            // 
            // cbDontFightWhileMounted
            // 
            this.cbDontFightWhileMounted.AutoSize = true;
            this.cbDontFightWhileMounted.Location = new System.Drawing.Point(3, 26);
            this.cbDontFightWhileMounted.Name = "cbDontFightWhileMounted";
            this.cbDontFightWhileMounted.Size = new System.Drawing.Size(183, 17);
            this.cbDontFightWhileMounted.TabIndex = 1;
            this.cbDontFightWhileMounted.Text = "Dont stop to fight when mounted.";
            this.cbDontFightWhileMounted.UseVisualStyleBackColor = true;
            this.cbDontFightWhileMounted.CheckedChanged += new System.EventHandler(this.cbDontFightWhileMounted_CheckedChanged);
            // 
            // cbGatherChests
            // 
            this.cbGatherChests.AutoSize = true;
            this.cbGatherChests.Location = new System.Drawing.Point(3, 49);
            this.cbGatherChests.Name = "cbGatherChests";
            this.cbGatherChests.Size = new System.Drawing.Size(93, 17);
            this.cbGatherChests.TabIndex = 2;
            this.cbGatherChests.Text = "Gather Chests";
            this.cbGatherChests.UseVisualStyleBackColor = true;
            this.cbGatherChests.CheckedChanged += new System.EventHandler(this.cbGatherChests_CheckedChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(204, 278);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Where";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.tbAvoidSearch);
            this.groupBox2.Controls.Add(this.lbAvoidZones);
            this.groupBox2.Controls.Add(this.lbAvoids);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(2, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(200, 274);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Avoid these Areas";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(8, 246);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(189, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "Remove Selected From Avoid List";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(5, 118);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(189, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Add Selected To Avoid List";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(145, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Search";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tbAvoidSearch
            // 
            this.tbAvoidSearch.Location = new System.Drawing.Point(6, 19);
            this.tbAvoidSearch.Name = "tbAvoidSearch";
            this.tbAvoidSearch.Size = new System.Drawing.Size(133, 20);
            this.tbAvoidSearch.TabIndex = 1;
            this.tbAvoidSearch.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lbAvoidZones
            // 
            this.lbAvoidZones.FormattingEnabled = true;
            this.lbAvoidZones.Location = new System.Drawing.Point(7, 150);
            this.lbAvoidZones.Name = "lbAvoidZones";
            this.lbAvoidZones.Size = new System.Drawing.Size(190, 95);
            this.lbAvoidZones.TabIndex = 0;
            // 
            // lbAvoids
            // 
            this.lbAvoids.FormattingEnabled = true;
            this.lbAvoids.Location = new System.Drawing.Point(5, 45);
            this.lbAvoids.Name = "lbAvoids";
            this.lbAvoids.Size = new System.Drawing.Size(190, 69);
            this.lbAvoids.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(366, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(212, 304);
            this.tabControl1.TabIndex = 8;
            // 
            // cbGatherMana
            // 
            this.cbGatherMana.AutoSize = true;
            this.cbGatherMana.Location = new System.Drawing.Point(3, 72);
            this.cbGatherMana.Name = "cbGatherMana";
            this.cbGatherMana.Size = new System.Drawing.Size(122, 17);
            this.cbGatherMana.TabIndex = 3;
            this.cbGatherMana.Text = "Gather Mana Nodes";
            this.cbGatherMana.UseVisualStyleBackColor = true;
            this.cbGatherMana.CheckedChanged += new System.EventHandler(this.cbGatherMana_CheckedChanged);
            // 
            // EclipseConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 304);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.pbEclipse);
            this.Name = "EclipseConfigForm";
            this.Text = "Eclipse - GatheringLegion Configuration";
            this.Load += new System.EventHandler(this.EclipseConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbEclipse)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbEclipse;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.CheckBox cbAvoidElites;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox cbDontFightWhileMounted;
        private System.Windows.Forms.CheckBox cbGatherChests;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbAvoidSearch;
        private System.Windows.Forms.ListBox lbAvoids;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox lbAvoidZones;
        private System.Windows.Forms.CheckBox cbGatherMana;
    }
}

