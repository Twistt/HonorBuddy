namespace Eclipse.WoWDatabase
{
    partial class EclipseDBManager
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
            this.lbItems = new System.Windows.Forms.ListBox();
            this.lbNpcs = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lbQuests = new System.Windows.Forms.ListBox();
            this.lbMobs = new System.Windows.Forms.ListBox();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.tbSql = new System.Windows.Forms.TextBox();
            this.btnRunSql = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnGetTargetInfo = new System.Windows.Forms.Button();
            this.btnGenerateInsert = new System.Windows.Forms.Button();
            this.btnGetTables = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.tbDBFile = new System.Windows.Forms.TextBox();
            this.tbRefreshUI = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbDistance = new System.Windows.Forms.ComboBox();
            this.btnSetDistance = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbItems
            // 
            this.lbItems.FormattingEnabled = true;
            this.lbItems.Location = new System.Drawing.Point(500, 56);
            this.lbItems.Name = "lbItems";
            this.lbItems.Size = new System.Drawing.Size(119, 160);
            this.lbItems.TabIndex = 0;
            // 
            // lbNpcs
            // 
            this.lbNpcs.FormattingEnabled = true;
            this.lbNpcs.Location = new System.Drawing.Point(623, 56);
            this.lbNpcs.Name = "lbNpcs";
            this.lbNpcs.Size = new System.Drawing.Size(165, 160);
            this.lbNpcs.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 568);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(788, 179);
            this.dataGridView1.TabIndex = 1;
            // 
            // lbQuests
            // 
            this.lbQuests.FormattingEnabled = true;
            this.lbQuests.Location = new System.Drawing.Point(367, 56);
            this.lbQuests.Name = "lbQuests";
            this.lbQuests.Size = new System.Drawing.Size(127, 160);
            this.lbQuests.TabIndex = 0;
            // 
            // lbMobs
            // 
            this.lbMobs.FormattingEnabled = true;
            this.lbMobs.Location = new System.Drawing.Point(234, 56);
            this.lbMobs.Name = "lbMobs";
            this.lbMobs.Size = new System.Drawing.Size(127, 160);
            this.lbMobs.TabIndex = 0;
            // 
            // lbTables
            // 
            this.lbTables.FormattingEnabled = true;
            this.lbTables.Location = new System.Drawing.Point(2, 259);
            this.lbTables.Name = "lbTables";
            this.lbTables.Size = new System.Drawing.Size(181, 95);
            this.lbTables.TabIndex = 2;
            this.lbTables.DoubleClick += new System.EventHandler(this.lbTables_DoubleClick);
            // 
            // tbSql
            // 
            this.tbSql.Location = new System.Drawing.Point(2, 360);
            this.tbSql.Multiline = true;
            this.tbSql.Name = "tbSql";
            this.tbSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSql.Size = new System.Drawing.Size(554, 202);
            this.tbSql.TabIndex = 3;
            // 
            // btnRunSql
            // 
            this.btnRunSql.Location = new System.Drawing.Point(562, 539);
            this.btnRunSql.Name = "btnRunSql";
            this.btnRunSql.Size = new System.Drawing.Size(116, 23);
            this.btnRunSql.TabIndex = 4;
            this.btnRunSql.Text = "btnRunSql";
            this.btnRunSql.UseVisualStyleBackColor = true;
            this.btnRunSql.Click += new System.EventHandler(this.btnRunSql_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(263, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(393, 222);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "button1";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(526, 222);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button1";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(683, 222);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "button1";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btnGetTargetInfo
            // 
            this.btnGetTargetInfo.Location = new System.Drawing.Point(562, 360);
            this.btnGetTargetInfo.Name = "btnGetTargetInfo";
            this.btnGetTargetInfo.Size = new System.Drawing.Size(116, 23);
            this.btnGetTargetInfo.TabIndex = 7;
            this.btnGetTargetInfo.Text = "GetTargetInfo";
            this.btnGetTargetInfo.UseVisualStyleBackColor = true;
            this.btnGetTargetInfo.Click += new System.EventHandler(this.btnGetTargetInfo_Click);
            // 
            // btnGenerateInsert
            // 
            this.btnGenerateInsert.Location = new System.Drawing.Point(562, 389);
            this.btnGenerateInsert.Name = "btnGenerateInsert";
            this.btnGenerateInsert.Size = new System.Drawing.Size(116, 23);
            this.btnGenerateInsert.TabIndex = 7;
            this.btnGenerateInsert.Text = "Generate Insert";
            this.btnGenerateInsert.UseVisualStyleBackColor = true;
            // 
            // btnGetTables
            // 
            this.btnGetTables.Location = new System.Drawing.Point(189, 259);
            this.btnGetTables.Name = "btnGetTables";
            this.btnGetTables.Size = new System.Drawing.Size(133, 23);
            this.btnGetTables.TabIndex = 5;
            this.btnGetTables.Text = "Get Tables";
            this.btnGetTables.UseVisualStyleBackColor = true;
            this.btnGetTables.Click += new System.EventHandler(this.btnGetTables_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(13, 22);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(144, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = "Load DB File";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // tbDBFile
            // 
            this.tbDBFile.Location = new System.Drawing.Point(164, 22);
            this.tbDBFile.Name = "tbDBFile";
            this.tbDBFile.Size = new System.Drawing.Size(455, 20);
            this.tbDBFile.TabIndex = 9;
            this.tbDBFile.Text = "C:\\Users\\Twist\\Downloads\\Honorbuddy 3.0.16306.861\\EclipseWoWDB.edb";
            // 
            // tbRefreshUI
            // 
            this.tbRefreshUI.Location = new System.Drawing.Point(331, 331);
            this.tbRefreshUI.Name = "tbRefreshUI";
            this.tbRefreshUI.Size = new System.Drawing.Size(225, 23);
            this.tbRefreshUI.TabIndex = 5;
            this.tbRefreshUI.Text = "Refresh UI";
            this.tbRefreshUI.UseVisualStyleBackColor = true;
            this.tbRefreshUI.Click += new System.EventHandler(this.tbRefreshUI_Click);
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(3, 16);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(611, 728);
            this.tbLog.TabIndex = 3;
            this.tbLog.TextChanged += new System.EventHandler(this.tbLog_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(794, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(617, 747);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
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
            this.cbDistance.Location = new System.Drawing.Point(500, 274);
            this.cbDistance.Name = "cbDistance";
            this.cbDistance.Size = new System.Drawing.Size(136, 21);
            this.cbDistance.TabIndex = 11;
            // 
            // btnSetDistance
            // 
            this.btnSetDistance.Location = new System.Drawing.Point(642, 274);
            this.btnSetDistance.Name = "btnSetDistance";
            this.btnSetDistance.Size = new System.Drawing.Size(136, 23);
            this.btnSetDistance.TabIndex = 12;
            this.btnSetDistance.Text = "Set Distance To Track";
            this.btnSetDistance.UseVisualStyleBackColor = true;
            this.btnSetDistance.Click += new System.EventHandler(this.btnSetDistance_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(563, 419);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(135, 23);
            this.button6.TabIndex = 13;
            this.button6.Text = "Generate Target Insert";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(623, 477);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 14;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(13, 56);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(215, 189);
            this.propertyGrid1.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(328, 315);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Running For:";
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(441, 317);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(35, 13);
            this.lblTimer.TabIndex = 16;
            this.lblTimer.Text = "label1";
            // 
            // EclipseDBManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1411, 747);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btnSetDistance);
            this.Controls.Add(this.cbDistance);
            this.Controls.Add(this.groupBox1);
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
            this.Name = "EclipseDBManager";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.EclipseDBManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbItems;
        private System.Windows.Forms.ListBox lbNpcs;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ListBox lbQuests;
        private System.Windows.Forms.ListBox lbMobs;
        private System.Windows.Forms.ListBox lbTables;
        private System.Windows.Forms.TextBox tbSql;
        private System.Windows.Forms.Button btnRunSql;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnGetTargetInfo;
        private System.Windows.Forms.Button btnGenerateInsert;
        private System.Windows.Forms.Button btnGetTables;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox tbDBFile;
        private System.Windows.Forms.Button tbRefreshUI;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbDistance;
        private System.Windows.Forms.Button btnSetDistance;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTimer;
    }
}

