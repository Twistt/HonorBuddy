using ArachnidCreations;
using Eclipse.WoWDatabase.Models;
using Styx;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eclipse.WoWDatabase
{
    public partial class EclipseDBManager : Form
    {
        public EclipseDBManager()
        {
            InitializeComponent();
        }
        Timer timer = new Timer();
        int timerTicks = 0;
        private void EclipseDBManager_Load(object sender, EventArgs e)
        {
            Core.iLog("Interface Shown.");
            Core.DataPath = tbDBFile.Text;
            timer.Tick += timer_Tick;
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Start();
            //if (Core.init == false) Core.Initialize();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timerTicks++;
            tbLog.Text = Core.ilog;
            //RefreshUI();
        }

        private void btnGetTargetInfo_Click(object sender, EventArgs e)
        {
            //if (Core.Target != null) tbSql.Text = DAL.generateCreateSQL(Core.Target, "MOBs");
            tbSql.Text = DAL.generateCreateSQL(new Quest(), "Quests");

        }

        private void btnRunSql_Click(object sender, EventArgs e)
        {
            DAL dal = new DAL();
            var dt = DAL.LoadSL3Data(tbSql.Text);
            dataGridView1.DataSource = dt;
        }

        private void btnGetTables_Click(object sender, EventArgs e)
        {
            DAL dal = new DAL();
            DataTable dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table';");
            lbTables.DataSource = dt;
            lbTables.DisplayMember = "Name";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            tbDBFile.Text = ofd.FileName;
        }

        private void tbRefreshUI_Click(object sender, EventArgs e)
        {
            RefreshUI();
            
        }
        public void RefreshUI()
        {
            lbMobs.DataSource = null;
            lbMobs.DataSource = Core.MOBs;
            lbMobs.DisplayMember = "Name";
            
            lbNpcs.DataSource = null;
            lbNpcs.DataSource = Core.NPCs;
            lbNpcs.DisplayMember = "Name";

            lbQuests.DataSource = null;
            lbQuests.DataSource = Core.Quests;
            lbQuests.DisplayMember = "Name";

            lblTimer.Text = timerTicks.ToString();
            tbLog.Text = Core.ilog;
            if (StyxWoW.Me != null)
            {
             if (StyxWoW.Me.CurrentTarget != null)   propertyGrid1.SelectedObject = StyxWoW.Me.CurrentTarget;
            }
        }

        private void btnSetDistance_Click(object sender, EventArgs e)
        {
            Core.AddTargetOnly = false;
            if (cbDistance.Text == "Target") Core.AddTargetOnly = true;
            else Core.DistanceToPoll = float.Parse(cbDistance.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //var qs = StyxWoW.Me.QuestLog.GetAllQuests().ToList();
            //var q = StyxWoW.Me.QuestLog.GetQuestById(qs.FirstOrDefault().Id);
            //tbSql.Text = DAL.generateCreateSQL(q, "Quests");

            tbSql.Text =
            DAL.Insert(StyxWoW.Me.CurrentTarget, "NPC", "", false, DAL.getTableStructure("NPC"));
            //NPCs.Add(Target);
            //iLog(string.Format("Added NPC {0} ({1})", Target.Name, Target.Entry));

        }
        private void button7_Click(object sender, EventArgs e)
        {
            var props = typeof(Quest).GetProperties().ToList();
            foreach (var prop in props){
                tbSql.AppendText(prop.PropertyType.ToString() + "\r\n");
            }

        }

        private void lbTables_DoubleClick(object sender, EventArgs e)
        {
            var table = (DataRowView)lbTables.SelectedItem;
            tbSql.Text = string.Format("Select *  from {0} limit 1000", table["Name"]);

        }

        private void tbLog_TextChanged(object sender, EventArgs e)
        {
            this.AutoScroll = true;
        }
    }
}
