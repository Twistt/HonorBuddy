namespace Eclipse.Bots.SkinBot.Views
{
    partial class EclipseDataManager
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
            this.lblTimer = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnSetDistance = new System.Windows.Forms.Button();
            this.cbDistance = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDBFile = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.btnGenerateInsert = new System.Windows.Forms.Button();
            this.btnGetTargetInfo = new System.Windows.Forms.Button();
            this.tbRefreshUI = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnGetTables = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRunSql = new System.Windows.Forms.Button();
            this.tbSql = new System.Windows.Forms.TextBox();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lbMobs = new System.Windows.Forms.ListBox();
            this.lbQuests = new System.Windows.Forms.ListBox();
            this.lbNpcs = new System.Windows.Forms.ListBox();
            this.lbItems = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(440, 313);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(35, 13);
            this.lblTimer.TabIndex = 42;
            this.lblTimer.Text = "label1";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(12, 52);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(215, 189);
            this.propertyGrid1.TabIndex = 40;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(622, 473);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 39;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(562, 415);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(135, 23);
            this.button6.TabIndex = 38;
            this.button6.Text = "Generate Target Insert";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // btnSetDistance
            // 
            this.btnSetDistance.Location = new System.Drawing.Point(641, 270);
            this.btnSetDistance.Name = "btnSetDistance";
            this.btnSetDistance.Size = new System.Drawing.Size(136, 23);
            this.btnSetDistance.TabIndex = 37;
            this.btnSetDistance.Text = "Set Distance To Track";
            this.btnSetDistance.UseVisualStyleBackColor = true;
            // 
            // cbDistance
            // 
            this.cbDistance.FormattingEnabled = true;
            this.cbDistance.Items.AddRange(new object[] {
            "Target",
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "40",
            "50",
            "70",
            "90",
            "100"});
            this.cbDistance.Location = new System.Drawing.Point(499, 270);
            this.cbDistance.Name = "cbDistance";
            this.cbDistance.Size = new System.Drawing.Size(136, 21);
            this.cbDistance.TabIndex = 36;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(792, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(617, 792);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(3, 16);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(611, 773);
            this.tbLog.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(327, 311);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Running For:";
            // 
            // tbDBFile
            // 
            this.tbDBFile.Location = new System.Drawing.Point(164, 12);
            this.tbDBFile.Name = "tbDBFile";
            this.tbDBFile.Size = new System.Drawing.Size(455, 20);
            this.tbDBFile.TabIndex = 34;
            this.tbDBFile.Text = "C:\\Users\\Twist\\Documents\\Honorbuddy\\EclipseWoWDB.edb";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(13, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(144, 23);
            this.button5.TabIndex = 33;
            this.button5.Text = "Load DB File";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // btnGenerateInsert
            // 
            this.btnGenerateInsert.Location = new System.Drawing.Point(561, 385);
            this.btnGenerateInsert.Name = "btnGenerateInsert";
            this.btnGenerateInsert.Size = new System.Drawing.Size(116, 23);
            this.btnGenerateInsert.TabIndex = 31;
            this.btnGenerateInsert.Text = "Generate Insert";
            this.btnGenerateInsert.UseVisualStyleBackColor = true;
            // 
            // btnGetTargetInfo
            // 
            this.btnGetTargetInfo.Location = new System.Drawing.Point(561, 356);
            this.btnGetTargetInfo.Name = "btnGetTargetInfo";
            this.btnGetTargetInfo.Size = new System.Drawing.Size(116, 23);
            this.btnGetTargetInfo.TabIndex = 32;
            this.btnGetTargetInfo.Text = "GetTargetInfo";
            this.btnGetTargetInfo.UseVisualStyleBackColor = true;
            // 
            // tbRefreshUI
            // 
            this.tbRefreshUI.Location = new System.Drawing.Point(330, 327);
            this.tbRefreshUI.Name = "tbRefreshUI";
            this.tbRefreshUI.Size = new System.Drawing.Size(225, 23);
            this.tbRefreshUI.TabIndex = 30;
            this.tbRefreshUI.Text = "Refresh UI";
            this.tbRefreshUI.UseVisualStyleBackColor = true;
            this.tbRefreshUI.Click += new System.EventHandler(this.tbRefreshUI_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(682, 218);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 29;
            this.button4.Text = "button1";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(525, 218);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 28;
            this.button3.Text = "button1";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(392, 218);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 27;
            this.button2.Text = "button1";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnGetTables
            // 
            this.btnGetTables.Location = new System.Drawing.Point(188, 255);
            this.btnGetTables.Name = "btnGetTables";
            this.btnGetTables.Size = new System.Drawing.Size(133, 23);
            this.btnGetTables.TabIndex = 26;
            this.btnGetTables.Text = "Get Tables";
            this.btnGetTables.UseVisualStyleBackColor = true;
            this.btnGetTables.Click += new System.EventHandler(this.btnGetTables_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(262, 218);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnRunSql
            // 
            this.btnRunSql.Location = new System.Drawing.Point(561, 535);
            this.btnRunSql.Name = "btnRunSql";
            this.btnRunSql.Size = new System.Drawing.Size(116, 23);
            this.btnRunSql.TabIndex = 24;
            this.btnRunSql.Text = "btnRunSql";
            this.btnRunSql.UseVisualStyleBackColor = true;
            this.btnRunSql.Click += new System.EventHandler(this.btnRunSql_Click);
            // 
            // tbSql
            // 
            this.tbSql.Location = new System.Drawing.Point(1, 356);
            this.tbSql.Multiline = true;
            this.tbSql.Name = "tbSql";
            this.tbSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSql.Size = new System.Drawing.Size(554, 202);
            this.tbSql.TabIndex = 23;
            // 
            // lbTables
            // 
            this.lbTables.FormattingEnabled = true;
            this.lbTables.Location = new System.Drawing.Point(1, 255);
            this.lbTables.Name = "lbTables";
            this.lbTables.Size = new System.Drawing.Size(181, 95);
            this.lbTables.TabIndex = 22;
            this.lbTables.DoubleClick += new System.EventHandler(this.lbTables_DoubleClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(-2, 610);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(788, 179);
            this.dataGridView1.TabIndex = 21;
            // 
            // lbMobs
            // 
            this.lbMobs.FormattingEnabled = true;
            this.lbMobs.Location = new System.Drawing.Point(233, 52);
            this.lbMobs.Name = "lbMobs";
            this.lbMobs.Size = new System.Drawing.Size(127, 160);
            this.lbMobs.TabIndex = 19;
            // 
            // lbQuests
            // 
            this.lbQuests.FormattingEnabled = true;
            this.lbQuests.Location = new System.Drawing.Point(366, 52);
            this.lbQuests.Name = "lbQuests";
            this.lbQuests.Size = new System.Drawing.Size(127, 160);
            this.lbQuests.TabIndex = 18;
            // 
            // lbNpcs
            // 
            this.lbNpcs.FormattingEnabled = true;
            this.lbNpcs.Location = new System.Drawing.Point(622, 52);
            this.lbNpcs.Name = "lbNpcs";
            this.lbNpcs.Size = new System.Drawing.Size(165, 160);
            this.lbNpcs.TabIndex = 20;
            // 
            // lbItems
            // 
            this.lbItems.FormattingEnabled = true;
            this.lbItems.Location = new System.Drawing.Point(499, 52);
            this.lbItems.Name = "lbItems";
            this.lbItems.Size = new System.Drawing.Size(119, 160);
            this.lbItems.TabIndex = 17;
            // 
            // EclipseDataManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1409, 792);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btnSetDistance);
            this.Controls.Add(this.cbDistance);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDBFile);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btnGenerateInsert);
            this.Controls.Add(this.btnGetTargetInfo);
            this.Controls.Add(this.tbRefreshUI);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnGetTables);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRunSql);
            this.Controls.Add(this.tbSql);
            this.Controls.Add(this.lbTables);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lbMobs);
            this.Controls.Add(this.lbQuests);
            this.Controls.Add(this.lbNpcs);
            this.Controls.Add(this.lbItems);
            this.Name = "EclipseDataManager";
            this.Text = "EclipseDataManager";
            this.Load += new System.EventHandler(this.EclipseDataManager_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnSetDistance;
        private System.Windows.Forms.ComboBox cbDistance;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDBFile;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnGenerateInsert;
        private System.Windows.Forms.Button btnGetTargetInfo;
        private System.Windows.Forms.Button tbRefreshUI;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnGetTables;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnRunSql;
        private System.Windows.Forms.TextBox tbSql;
        private System.Windows.Forms.ListBox lbTables;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ListBox lbMobs;
        private System.Windows.Forms.ListBox lbQuests;
        private System.Windows.Forms.ListBox lbNpcs;
        private System.Windows.Forms.ListBox lbItems;
    }
}