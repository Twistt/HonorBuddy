namespace Eclipse.ShadowBot
{
    partial class ShadowBotConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShadowBotConfig));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTarget = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.boolGetQuests = new System.Windows.Forms.CheckBox();
            this.boolAssistLeader = new System.Windows.Forms.CheckBox();
            this.tbFollowDistance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkboxHealBotMode = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbFreeBagSlots = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFollowName = new System.Windows.Forms.TextBox();
            this.boolFollowByName = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnShowAdvanced = new System.Windows.Forms.Button();
            this.boolSkinMobs = new System.Windows.Forms.CheckBox();
            this.boolLootMobs = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gbServerSettings = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbMessages = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.tbServerIP = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbServerLog = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.cbClients = new System.Windows.Forms.ComboBox();
            this.cbClientMessages = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbServerSettings.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(95, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Following";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Following:";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(129, 201);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(109, 13);
            this.lblTarget.TabIndex = 1;
            this.lblTarget.Text = "Not following anyone.";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(21, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Ignore Attackers";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // boolGetQuests
            // 
            this.boolGetQuests.AutoSize = true;
            this.boolGetQuests.Checked = true;
            this.boolGetQuests.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolGetQuests.Location = new System.Drawing.Point(21, 42);
            this.boolGetQuests.Name = "boolGetQuests";
            this.boolGetQuests.Size = new System.Drawing.Size(100, 17);
            this.boolGetQuests.TabIndex = 2;
            this.boolGetQuests.Text = "Pick Up Quests";
            this.boolGetQuests.UseVisualStyleBackColor = true;
            // 
            // boolAssistLeader
            // 
            this.boolAssistLeader.AutoSize = true;
            this.boolAssistLeader.Checked = true;
            this.boolAssistLeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolAssistLeader.Location = new System.Drawing.Point(184, 42);
            this.boolAssistLeader.Name = "boolAssistLeader";
            this.boolAssistLeader.Size = new System.Drawing.Size(89, 17);
            this.boolAssistLeader.TabIndex = 2;
            this.boolAssistLeader.Text = "Assist Leader";
            this.boolAssistLeader.UseVisualStyleBackColor = true;
            // 
            // tbFollowDistance
            // 
            this.tbFollowDistance.Location = new System.Drawing.Point(95, 57);
            this.tbFollowDistance.Name = "tbFollowDistance";
            this.tbFollowDistance.Size = new System.Drawing.Size(39, 20);
            this.tbFollowDistance.TabIndex = 3;
            this.tbFollowDistance.Text = "12";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Follow Distance";
            // 
            // checkboxHealBotMode
            // 
            this.checkboxHealBotMode.AutoSize = true;
            this.checkboxHealBotMode.Location = new System.Drawing.Point(184, 19);
            this.checkboxHealBotMode.Name = "checkboxHealBotMode";
            this.checkboxHealBotMode.Size = new System.Drawing.Size(94, 17);
            this.checkboxHealBotMode.TabIndex = 5;
            this.checkboxHealBotMode.Text = "HealBot Mode";
            this.checkboxHealBotMode.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(105, 366);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 57);
            this.button2.TabIndex = 6;
            this.button2.Text = "Set THIS bot as your LEADER!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.Cursor = System.Windows.Forms.Cursors.Help;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(328, 37);
            this.label4.TabIndex = 8;
            this.label4.Text = "This is the distance that the follower will attempt to keep the leader - this als" +
    "o dictates how far away it will go to get Quests and loot.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbFreeBagSlots);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbFollowName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblTarget);
            this.groupBox1.Controls.Add(this.boolFollowByName);
            this.groupBox1.Controls.Add(this.tbFollowDistance);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 217);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Following";
            // 
            // tbFreeBagSlots
            // 
            this.tbFreeBagSlots.Location = new System.Drawing.Point(280, 57);
            this.tbFreeBagSlots.Name = "tbFreeBagSlots";
            this.tbFreeBagSlots.Size = new System.Drawing.Size(39, 20);
            this.tbFreeBagSlots.TabIndex = 14;
            this.tbFreeBagSlots.Text = "5";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(21, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(300, 33);
            this.label5.TabIndex = 13;
            this.label5.Text = "You do not HAVE to have a leader running ShadowBot but if you do press start on t" +
    "he leader first.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Leader Name:";
            // 
            // tbFollowName
            // 
            this.tbFollowName.Location = new System.Drawing.Point(221, 98);
            this.tbFollowName.Name = "tbFollowName";
            this.tbFollowName.Size = new System.Drawing.Size(100, 20);
            this.tbFollowName.TabIndex = 11;
            // 
            // boolFollowByName
            // 
            this.boolFollowByName.AutoSize = true;
            this.boolFollowByName.Checked = true;
            this.boolFollowByName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolFollowByName.Location = new System.Drawing.Point(21, 101);
            this.boolFollowByName.Name = "boolFollowByName";
            this.boolFollowByName.Size = new System.Drawing.Size(102, 17);
            this.boolFollowByName.TabIndex = 2;
            this.boolFollowByName.Text = "Follow By Name";
            this.boolFollowByName.UseVisualStyleBackColor = true;
            this.boolFollowByName.CheckedChanged += new System.EventHandler(this.boolFollowByName_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(169, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Sell at free Bag Slots";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnShowAdvanced);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.boolSkinMobs);
            this.groupBox2.Controls.Add(this.boolLootMobs);
            this.groupBox2.Controls.Add(this.boolGetQuests);
            this.groupBox2.Controls.Add(this.boolAssistLeader);
            this.groupBox2.Controls.Add(this.checkboxHealBotMode);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 220);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(334, 132);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnShowAdvanced
            // 
            this.btnShowAdvanced.Location = new System.Drawing.Point(95, 103);
            this.btnShowAdvanced.Name = "btnShowAdvanced";
            this.btnShowAdvanced.Size = new System.Drawing.Size(143, 23);
            this.btnShowAdvanced.TabIndex = 6;
            this.btnShowAdvanced.Text = "Show Advanced Settings";
            this.btnShowAdvanced.UseVisualStyleBackColor = true;
            this.btnShowAdvanced.Click += new System.EventHandler(this.btnShowAdvanced_Click);
            // 
            // boolSkinMobs
            // 
            this.boolSkinMobs.AutoSize = true;
            this.boolSkinMobs.Checked = true;
            this.boolSkinMobs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolSkinMobs.Location = new System.Drawing.Point(184, 65);
            this.boolSkinMobs.Name = "boolSkinMobs";
            this.boolSkinMobs.Size = new System.Drawing.Size(76, 17);
            this.boolSkinMobs.TabIndex = 2;
            this.boolSkinMobs.Text = "Skin Mobs";
            this.boolSkinMobs.UseVisualStyleBackColor = true;
            // 
            // boolLootMobs
            // 
            this.boolLootMobs.AutoSize = true;
            this.boolLootMobs.Checked = true;
            this.boolLootMobs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolLootMobs.Location = new System.Drawing.Point(21, 65);
            this.boolLootMobs.Name = "boolLootMobs";
            this.boolLootMobs.Size = new System.Drawing.Size(76, 17);
            this.boolLootMobs.TabIndex = 2;
            this.boolLootMobs.Text = "Loot Mobs";
            this.boolLootMobs.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(214, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(348, 528);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gbServerSettings);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(340, 502);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Follow";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gbServerSettings
            // 
            this.gbServerSettings.Controls.Add(this.groupBox4);
            this.gbServerSettings.Controls.Add(this.label7);
            this.gbServerSettings.Controls.Add(this.label8);
            this.gbServerSettings.Controls.Add(this.tbServerPort);
            this.gbServerSettings.Controls.Add(this.tbServerIP);
            this.gbServerSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbServerSettings.Location = new System.Drawing.Point(3, 352);
            this.gbServerSettings.Name = "gbServerSettings";
            this.gbServerSettings.Size = new System.Drawing.Size(334, 137);
            this.gbServerSettings.TabIndex = 11;
            this.gbServerSettings.TabStop = false;
            this.gbServerSettings.Text = "Server Settings";
            this.gbServerSettings.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbMessages);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox4.Location = new System.Drawing.Point(3, 45);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(328, 89);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Messages From The Leader";
            // 
            // lbMessages
            // 
            this.lbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMessages.FormattingEnabled = true;
            this.lbMessages.Location = new System.Drawing.Point(3, 16);
            this.lbMessages.Name = "lbMessages";
            this.lbMessages.Size = new System.Drawing.Size(322, 70);
            this.lbMessages.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(152, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Port";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Server IP";
            // 
            // tbServerPort
            // 
            this.tbServerPort.Location = new System.Drawing.Point(184, 19);
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(41, 20);
            this.tbServerPort.TabIndex = 4;
            this.tbServerPort.Text = "8001";
            // 
            // tbServerIP
            // 
            this.tbServerIP.Location = new System.Drawing.Point(63, 19);
            this.tbServerIP.Name = "tbServerIP";
            this.tbServerIP.Size = new System.Drawing.Size(83, 20);
            this.tbServerIP.TabIndex = 6;
            this.tbServerIP.Text = "127.0.0.1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(340, 502);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Leader";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbServerLog);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(3, 201);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(334, 111);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // lbServerLog
            // 
            this.lbServerLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbServerLog.FormattingEnabled = true;
            this.lbServerLog.Location = new System.Drawing.Point(3, 16);
            this.lbServerLog.Name = "lbServerLog";
            this.lbServerLog.Size = new System.Drawing.Size(328, 92);
            this.lbServerLog.TabIndex = 7;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button4);
            this.groupBox6.Controls.Add(this.cbClients);
            this.groupBox6.Controls.Add(this.cbClientMessages);
            this.groupBox6.Controls.Add(this.button3);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(3, 114);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(334, 87);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Manage Clients";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(231, 45);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(97, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "Send to Client";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // cbClients
            // 
            this.cbClients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClients.FormattingEnabled = true;
            this.cbClients.Location = new System.Drawing.Point(4, 47);
            this.cbClients.Name = "cbClients";
            this.cbClients.Size = new System.Drawing.Size(218, 21);
            this.cbClients.TabIndex = 11;
            // 
            // cbClientMessages
            // 
            this.cbClientMessages.FormattingEnabled = true;
            this.cbClientMessages.Items.AddRange(new object[] {
            "MountUp",
            "ReadyCheck",
            "Disconnect",
            "ToMe",
            "ChangeLeader",
            "Dismount"});
            this.cbClientMessages.Location = new System.Drawing.Point(3, 19);
            this.cbClientMessages.Name = "cbClientMessages";
            this.cbClientMessages.Size = new System.Drawing.Size(196, 21);
            this.cbClientMessages.TabIndex = 10;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(205, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(123, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Send to All Clients";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.listBox1);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(334, 111);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Clients";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 16);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(328, 92);
            this.listBox1.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(215, 528);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // ShadowBotConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 528);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tabControl1);
            this.Name = "ShadowBotConfig";
            this.Text = "Eclipse - ShadowBot Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShadowBotConfig_FormClosing);
            this.Load += new System.EventHandler(this.ShadowBotConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gbServerSettings.ResumeLayout(false);
            this.gbServerSettings.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox boolGetQuests;
        private System.Windows.Forms.CheckBox boolAssistLeader;
        private System.Windows.Forms.TextBox tbFollowDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkboxHealBotMode;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox boolLootMobs;
        private System.Windows.Forms.CheckBox boolSkinMobs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFollowName;
        private System.Windows.Forms.CheckBox boolFollowByName;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox gbServerSettings;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lbMessages;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbServerPort;
        private System.Windows.Forms.TextBox tbServerIP;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbClientMessages;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox cbClients;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnShowAdvanced;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbFreeBagSlots;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lbServerLog;
    }
}

