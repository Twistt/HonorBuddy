using Eclipse.Models;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eclipse.Bots.QuestBot.Views
{
    public partial class MobSelectionList : Form
    {
        public bool SkinMode = false;
        public MobSelectionList()
        {

            InitializeComponent();
        }

        private void btnSearchMobs_Click(object sender, EventArgs e)
        {
            lbMobs.DataSource = null;
            var list = EC.MOBs.Where(n => n.Name.ToLower().Contains(tbSearchMobs.Text.ToLower())).ToList();
            lbMobs.DataSource = list;
            lbMobs.DisplayMember = "Name";
        }

        private void MobSelectionList_Load(object sender, EventArgs e)
        {
            lbMobs.DataSource = EC.MOBs;
            lbMobs.DisplayMember = "Name";

            if (SkinMode)
            {
                lbKillList.DataSource = EC.SkinList;
            }
            else lbKillList.DataSource = EC.KillList;
            lbKillList.DisplayMember = "Name";

            lbIgnoreList.DataSource = EC.IgnoreList;
            lbIgnoreList.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbKillList.DataSource = null;
            var mob = (WoWUnit)lbAMobs.SelectedItem;
            var dbmob = EC.MOBs.Where(m => m.Entry == mob.Entry).FirstOrDefault();
            if (dbmob != null) EC.KillList.Add(dbmob);
            else
            {
                var newmob = new Mob { Entry = mob.Entry, FactionId = mob.FactionId, Level = mob.Level, Name = mob.Name, Zone = StyxWoW.Me.ZoneId };
                EC.AddUnit(mob);
                if (SkinMode)
                {
                    EC.SkinList.Add(newmob);
                    lbKillList.DataSource = EC.SkinList;
                }
                else
                {
                    EC.KillList.Add(newmob);
                    lbKillList.DataSource = EC.KillList;
                }
            }

            lbKillList.DisplayMember = "Name";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lbKillList.DataSource = null;
            var mob = (Mob)lbMobs.SelectedItem;
            if (SkinMode)
            {
                EC.SkinList.Add(mob);
                lbKillList.DataSource = EC.SkinList;
            }
            else
            {
                EC.KillList.Add(mob);
                lbKillList.DataSource = EC.KillList;
            }
            lbKillList.DisplayMember = "Name";
        }

        private void button4_Click(object sender, EventArgs e)
        {

            lbAMobs.DataSource = ObjectManager.ObjectList.Where(w => w.Type == WoWObjectType.Unit).ToList().Where(m => !((WoWUnit)m).IsFriendly).ToList();
            lbMobs.DisplayMember = "Name";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lbIgnoreList.DataSource = null;
            var mob = (Mob)lbIgnoreList.SelectedItem;
            EC.IgnoreList.Remove(mob);
            lbIgnoreList.DataSource = EC.IgnoreList;
            lbIgnoreList.DisplayMember = "Name";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lbIgnoreList.DataSource = null;
            var mob = (Mob)lbMobs.SelectedItem;
            EC.IgnoreList.Add(mob);
            lbIgnoreList.DataSource = EC.IgnoreList;
            lbIgnoreList.DisplayMember = "Name";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lbIgnoreList.DataSource = null;
            var mob = (WoWUnit)lbAMobs.SelectedItem;
            var dbmob = EC.MOBs.Where(m => m.Entry == mob.Entry).FirstOrDefault();
            if (dbmob != null) EC.IgnoreList.Add(dbmob);
            else
            {
                var newmob = new Mob { Entry = mob.Entry, FactionId = mob.FactionId, Level = mob.Level, Name = mob.Name, Zone = StyxWoW.Me.ZoneId };
                EC.AddUnit(mob);
                EC.IgnoreList.Add(newmob);
            }
            lbIgnoreList.DataSource = EC.IgnoreList;
            lbIgnoreList.DisplayMember = "Name";
        }
    }
}
