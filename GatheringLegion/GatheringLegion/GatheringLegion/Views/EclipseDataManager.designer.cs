namespace Eclipse.Bots.GatheringLegion.Views
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
            this.btnItems = new System.Windows.Forms.Button();
            this.btnNPCs = new System.Windows.Forms.Button();
            this.btnMobs = new System.Windows.Forms.Button();
            this.btnGetTables = new System.Windows.Forms.Button();
            this.btnLocations = new System.Windows.Forms.Button();
            this.btnRunSql = new System.Windows.Forms.Button();
            this.tbSql = new System.Windows.Forms.TextBox();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lbMobs = new System.Windows.Forms.ListBox();
            this.lbLocations = new System.Windows.Forms.ListBox();
            this.lbNpcs = new System.Windows.Forms.ListBox();
            this.lbItems = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lbObjects = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(587, 385);
            this.lblTimer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(46, 17);
            this.lblTimer.TabIndex = 42;
            this.lblTimer.Text = "label1";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(16, 64);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(287, 233);
            this.propertyGrid1.TabIndex = 40;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(829, 582);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 28);
            this.button7.TabIndex = 39;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(749, 511);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(180, 28);
            this.button6.TabIndex = 38;
            this.button6.Text = "Generate Target Insert";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // btnSetDistance
            // 
            this.btnSetDistance.Location = new System.Drawing.Point(855, 332);
            this.btnSetDistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSetDistance.Name = "btnSetDistance";
            this.btnSetDistance.Size = new System.Drawing.Size(181, 28);
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
            this.cbDistance.Location = new System.Drawing.Point(665, 332);
            this.cbDistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDistance.Name = "cbDistance";
            this.cbDistance.Size = new System.Drawing.Size(180, 24);
            this.cbDistance.TabIndex = 36;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(1236, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(643, 975);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(4, 19);
            this.tbLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(635, 952);
            this.tbLog.TabIndex = 3;
            this.tbLog.Text = "C:\\Users\\Twist\\Documents\\Honorbuddy\\Bots\\EclipseGatheringLegion\\Data\\EclipseWoWDB" +
    ".edb";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(436, 383);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "Running For:";
            // 
            // tbDBFile
            // 
            this.tbDBFile.Location = new System.Drawing.Point(219, 15);
            this.tbDBFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDBFile.Name = "tbDBFile";
            this.tbDBFile.Size = new System.Drawing.Size(605, 22);
            this.tbDBFile.TabIndex = 34;
            this.tbDBFile.Text = "C:\\Users\\Twist\\Downloads\\Honorbuddy 3.0.16306.861\\Bots\\SkinbotV2\\SkinbotV2\\Data\\E" +
    "clipseWoWDB.edb";
            this.tbDBFile.TextChanged += new System.EventHandler(this.tbDBFile_TextChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(17, 11);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(192, 28);
            this.button5.TabIndex = 33;
            this.button5.Text = "Load DB File";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // btnGenerateInsert
            // 
            this.btnGenerateInsert.Location = new System.Drawing.Point(748, 474);
            this.btnGenerateInsert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerateInsert.Name = "btnGenerateInsert";
            this.btnGenerateInsert.Size = new System.Drawing.Size(155, 28);
            this.btnGenerateInsert.TabIndex = 31;
            this.btnGenerateInsert.Text = "Generate Insert";
            this.btnGenerateInsert.UseVisualStyleBackColor = true;
            // 
            // btnGetTargetInfo
            // 
            this.btnGetTargetInfo.Location = new System.Drawing.Point(748, 438);
            this.btnGetTargetInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGetTargetInfo.Name = "btnGetTargetInfo";
            this.btnGetTargetInfo.Size = new System.Drawing.Size(155, 28);
            this.btnGetTargetInfo.TabIndex = 32;
            this.btnGetTargetInfo.Text = "GetTargetInfo";
            this.btnGetTargetInfo.UseVisualStyleBackColor = true;
            this.btnGetTargetInfo.Click += new System.EventHandler(this.btnGetTargetInfo_Click_1);
            // 
            // tbRefreshUI
            // 
            this.tbRefreshUI.Location = new System.Drawing.Point(440, 402);
            this.tbRefreshUI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbRefreshUI.Name = "tbRefreshUI";
            this.tbRefreshUI.Size = new System.Drawing.Size(300, 28);
            this.tbRefreshUI.TabIndex = 30;
            this.tbRefreshUI.Text = "Refresh UI";
            this.tbRefreshUI.UseVisualStyleBackColor = true;
            this.tbRefreshUI.Click += new System.EventHandler(this.tbRefreshUI_Click);
            // 
            // btnItems
            // 
            this.btnItems.Location = new System.Drawing.Point(696, 268);
            this.btnItems.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnItems.Name = "btnItems";
            this.btnItems.Size = new System.Drawing.Size(100, 28);
            this.btnItems.TabIndex = 29;
            this.btnItems.Text = "Bag Items";
            this.btnItems.UseVisualStyleBackColor = true;
            // 
            // btnNPCs
            // 
            this.btnNPCs.Location = new System.Drawing.Point(855, 268);
            this.btnNPCs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNPCs.Name = "btnNPCs";
            this.btnNPCs.Size = new System.Drawing.Size(100, 28);
            this.btnNPCs.TabIndex = 28;
            this.btnNPCs.Text = "NPCs";
            this.btnNPCs.UseVisualStyleBackColor = true;
            this.btnNPCs.Click += new System.EventHandler(this.btnNPCs_Click);
            // 
            // btnMobs
            // 
            this.btnMobs.Location = new System.Drawing.Point(340, 268);
            this.btnMobs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMobs.Name = "btnMobs";
            this.btnMobs.Size = new System.Drawing.Size(100, 28);
            this.btnMobs.TabIndex = 27;
            this.btnMobs.Text = "Mobs";
            this.btnMobs.UseVisualStyleBackColor = true;
            this.btnMobs.Click += new System.EventHandler(this.btnMobs_Click);
            // 
            // btnGetTables
            // 
            this.btnGetTables.Location = new System.Drawing.Point(251, 314);
            this.btnGetTables.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGetTables.Name = "btnGetTables";
            this.btnGetTables.Size = new System.Drawing.Size(177, 28);
            this.btnGetTables.TabIndex = 26;
            this.btnGetTables.Text = "Get Tables";
            this.btnGetTables.UseVisualStyleBackColor = true;
            this.btnGetTables.Click += new System.EventHandler(this.btnGetTables_Click);
            // 
            // btnLocations
            // 
            this.btnLocations.Location = new System.Drawing.Point(515, 268);
            this.btnLocations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLocations.Name = "btnLocations";
            this.btnLocations.Size = new System.Drawing.Size(100, 28);
            this.btnLocations.TabIndex = 25;
            this.btnLocations.Text = "Locations";
            this.btnLocations.UseVisualStyleBackColor = true;
            this.btnLocations.Click += new System.EventHandler(this.btnLocations_Click);
            // 
            // btnRunSql
            // 
            this.btnRunSql.Location = new System.Drawing.Point(748, 658);
            this.btnRunSql.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRunSql.Name = "btnRunSql";
            this.btnRunSql.Size = new System.Drawing.Size(155, 28);
            this.btnRunSql.TabIndex = 24;
            this.btnRunSql.Text = "btnRunSql";
            this.btnRunSql.UseVisualStyleBackColor = true;
            this.btnRunSql.Click += new System.EventHandler(this.btnRunSql_Click);
            // 
            // tbSql
            // 
            this.tbSql.Location = new System.Drawing.Point(1, 438);
            this.tbSql.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSql.Multiline = true;
            this.tbSql.Name = "tbSql";
            this.tbSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSql.Size = new System.Drawing.Size(737, 248);
            this.tbSql.TabIndex = 23;
            // 
            // lbTables
            // 
            this.lbTables.FormattingEnabled = true;
            this.lbTables.ItemHeight = 16;
            this.lbTables.Location = new System.Drawing.Point(1, 314);
            this.lbTables.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbTables.Name = "lbTables";
            this.lbTables.Size = new System.Drawing.Size(240, 116);
            this.lbTables.TabIndex = 22;
            this.lbTables.DoubleClick += new System.EventHandler(this.lbTables_DoubleClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(-3, 751);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1051, 220);
            this.dataGridView1.TabIndex = 21;
            // 
            // lbMobs
            // 
            this.lbMobs.FormattingEnabled = true;
            this.lbMobs.ItemHeight = 16;
            this.lbMobs.Location = new System.Drawing.Point(311, 64);
            this.lbMobs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbMobs.Name = "lbMobs";
            this.lbMobs.Size = new System.Drawing.Size(168, 196);
            this.lbMobs.TabIndex = 19;
            this.lbMobs.SelectedValueChanged += new System.EventHandler(this.lbMobs_SelectedValueChanged);
            // 
            // lbLocations
            // 
            this.lbLocations.FormattingEnabled = true;
            this.lbLocations.ItemHeight = 16;
            this.lbLocations.Location = new System.Drawing.Point(488, 64);
            this.lbLocations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbLocations.Name = "lbLocations";
            this.lbLocations.Size = new System.Drawing.Size(168, 196);
            this.lbLocations.TabIndex = 18;
            this.lbLocations.SelectedIndexChanged += new System.EventHandler(this.lbLocations_SelectedIndexChanged);
            this.lbLocations.SelectedValueChanged += new System.EventHandler(this.lbLocations_SelectedValueChanged);
            this.lbLocations.DoubleClick += new System.EventHandler(this.lbLocations_DoubleClick);
            // 
            // lbNpcs
            // 
            this.lbNpcs.FormattingEnabled = true;
            this.lbNpcs.ItemHeight = 16;
            this.lbNpcs.Location = new System.Drawing.Point(829, 64);
            this.lbNpcs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbNpcs.Name = "lbNpcs";
            this.lbNpcs.Size = new System.Drawing.Size(183, 196);
            this.lbNpcs.TabIndex = 20;
            this.lbNpcs.SelectedValueChanged += new System.EventHandler(this.lbNpcs_SelectedValueChanged);
            // 
            // lbItems
            // 
            this.lbItems.FormattingEnabled = true;
            this.lbItems.ItemHeight = 16;
            this.lbItems.Location = new System.Drawing.Point(665, 64);
            this.lbItems.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbItems.Name = "lbItems";
            this.lbItems.Size = new System.Drawing.Size(157, 196);
            this.lbItems.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(855, 369);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 28);
            this.button1.TabIndex = 43;
            this.button1.Text = "AOE DataMine";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbObjects
            // 
            this.lbObjects.FormattingEnabled = true;
            this.lbObjects.ItemHeight = 16;
            this.lbObjects.Location = new System.Drawing.Point(1021, 64);
            this.lbObjects.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbObjects.Name = "lbObjects";
            this.lbObjects.Size = new System.Drawing.Size(183, 196);
            this.lbObjects.TabIndex = 20;
            this.lbObjects.SelectedIndexChanged += new System.EventHandler(this.lbObjects_SelectedIndexChanged);
            this.lbObjects.SelectedValueChanged += new System.EventHandler(this.lbNpcs_SelectedValueChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1047, 268);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 28;
            this.button2.Text = "Objects";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1111, 314);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 44;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // EclipseDataManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1879, 975);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
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
            this.Controls.Add(this.btnItems);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnNPCs);
            this.Controls.Add(this.btnMobs);
            this.Controls.Add(this.btnGetTables);
            this.Controls.Add(this.btnLocations);
            this.Controls.Add(this.btnRunSql);
            this.Controls.Add(this.tbSql);
            this.Controls.Add(this.lbTables);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lbMobs);
            this.Controls.Add(this.lbLocations);
            this.Controls.Add(this.lbObjects);
            this.Controls.Add(this.lbNpcs);
            this.Controls.Add(this.lbItems);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button btnItems;
        private System.Windows.Forms.Button btnNPCs;
        private System.Windows.Forms.Button btnMobs;
        private System.Windows.Forms.Button btnGetTables;
        private System.Windows.Forms.Button btnLocations;
        private System.Windows.Forms.Button btnRunSql;
        private System.Windows.Forms.TextBox tbSql;
        private System.Windows.Forms.ListBox lbTables;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ListBox lbMobs;
        private System.Windows.Forms.ListBox lbLocations;
        private System.Windows.Forms.ListBox lbNpcs;
        private System.Windows.Forms.ListBox lbItems;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lbObjects;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}