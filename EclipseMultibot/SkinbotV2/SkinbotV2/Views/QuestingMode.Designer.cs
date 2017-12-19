namespace Eclipse.MultiBot.Views
{
    partial class QuestingMode
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
            this.btnListNearbyQuestNPCs = new System.Windows.Forms.Button();
            this.lbNPCsWithQuests = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblQuestStatus = new System.Windows.Forms.Label();
            this.lblTarget = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // btnListNearbyQuestNPCs
            // 
            this.btnListNearbyQuestNPCs.Location = new System.Drawing.Point(265, 143);
            this.btnListNearbyQuestNPCs.Name = "btnListNearbyQuestNPCs";
            this.btnListNearbyQuestNPCs.Size = new System.Drawing.Size(174, 23);
            this.btnListNearbyQuestNPCs.TabIndex = 0;
            this.btnListNearbyQuestNPCs.Text = "List Nearby NPCs with Quests";
            this.btnListNearbyQuestNPCs.UseVisualStyleBackColor = true;
            this.btnListNearbyQuestNPCs.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbNPCsWithQuests
            // 
            this.lbNPCsWithQuests.FormattingEnabled = true;
            this.lbNPCsWithQuests.Location = new System.Drawing.Point(265, 169);
            this.lbNPCsWithQuests.Name = "lbNPCsWithQuests";
            this.lbNPCsWithQuests.Size = new System.Drawing.Size(174, 82);
            this.lbNPCsWithQuests.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 169);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(245, 82);
            this.listBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current Quests";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(232, 280);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(148, 225);
            this.listBox2.TabIndex = 7;
            this.listBox2.SelectedValueChanged += new System.EventHandler(this.listBox2_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(229, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Objectives";
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(12, 280);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(214, 225);
            this.listBox3.TabIndex = 10;
            this.listBox3.SelectedIndexChanged += new System.EventHandler(this.listBox3_SelectedIndexChanged);
            this.listBox3.SelectedValueChanged += new System.EventHandler(this.listBox3_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "WoW Quests";
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(782, 306);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(205, 196);
            this.tbLog.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(782, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Log";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(540, 280);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(236, 225);
            this.propertyGrid1.TabIndex = 13;
            // 
            // listBox4
            // 
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Location = new System.Drawing.Point(386, 280);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(148, 225);
            this.listBox4.TabIndex = 7;
            this.listBox4.SelectedValueChanged += new System.EventHandler(this.listBox4_SelectedValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(383, 264);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "All Quests";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(529, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(236, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Show Current Objective";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // lblQuestStatus
            // 
            this.lblQuestStatus.AutoSize = true;
            this.lblQuestStatus.Location = new System.Drawing.Point(537, 264);
            this.lblQuestStatus.Name = "lblQuestStatus";
            this.lblQuestStatus.Size = new System.Drawing.Size(35, 13);
            this.lblQuestStatus.TabIndex = 15;
            this.lblQuestStatus.Text = "label2";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(537, 238);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(35, 13);
            this.lblTarget.TabIndex = 15;
            this.lblTarget.Text = "label2";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(537, 216);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(35, 13);
            this.lblLocation.TabIndex = 15;
            this.lblLocation.Text = "label2";
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(822, 13);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(202, 287);
            this.propertyGrid2.TabIndex = 16;
            // 
            // QuestingMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 514);
            this.Controls.Add(this.propertyGrid2);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.lblQuestStatus);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox4);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lbNPCsWithQuests);
            this.Controls.Add(this.btnListNearbyQuestNPCs);
            this.Name = "QuestingMode";
            this.Text = "Questing Mode Configuration";
            this.Load += new System.EventHandler(this.QuestingMode_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnListNearbyQuestNPCs;
        private System.Windows.Forms.ListBox lbNPCsWithQuests;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblQuestStatus;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
    }
}