using Eclipse.WoWDatabase;
using Eclipse.WoWDatabase.Models;
using Styx;
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

namespace SkinbotV2.Views
{
    public partial class SkinningManagement : Form
    {
        public SkinningManagement()
        {
            InitializeComponent();
        }

        private void SkinningManagement_Load(object sender, EventArgs e)
        {
            lbMobs.DataSource = null;
            var list = Core.MOBs.Where(n => n.isSkinnable).ToList();
            lbMobs.DataSource = list;
            lbMobs.DisplayMember = "Name";
        }

        private void btnAddTargeted_Click(object sender, EventArgs e)
        {
            if (Styx.StyxWoW.Me.CurrentTarget != null)
            {
                lbMobs.DataSource = null;
                var dbmob = Core.MOBs.Where(m => m.Entry == Styx.StyxWoW.Me.CurrentTarget.Entry).FirstOrDefault();
                if (dbmob != null) dbmob.isSkinnable = true;
                else
                {
                    var mob = Styx.StyxWoW.Me.CurrentTarget;
                    Core.MOBs.Add(new Mob { Entry = mob.Entry, FactionId = mob.FactionId, Level = mob.Level, Name = mob.Name, Zone = StyxWoW.Me.ZoneId, isSkinnable = true });
                    Core.AddMob(Styx.StyxWoW.Me.CurrentTarget);

                }
                lbMobs.DataSource = Core.MOBs.Where(n => n.isSkinnable).ToList();
            }
            else
            {
                MessageBox.Show("You have no target selected");
            }
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            Core.IgnoreList.Add((Mob)lbMobs.SelectedItem);
            Core.log("This mob has been added to the ignore list.");
        }
    }
}
