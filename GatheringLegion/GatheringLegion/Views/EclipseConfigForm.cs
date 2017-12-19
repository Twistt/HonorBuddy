using Eclipse.Bots.GatheringLegion.Views;
using Eclipse.WoWDatabase;
using GatheringLegion.Models;
using Styx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eclipse.Bots.GatheringLegion
{
    public partial class EclipseConfigForm : Form
    {

        public EclipseConfigForm()
        {
            InitializeComponent();
            checkBox2.Checked = Core.GatherMode;
            checkBox1.Checked = Core.PassiveMode;

        }

        private void EclipseConfigForm_Load(object sender, EventArgs e)
        {
            pbEclipse.ImageLocation = "http://www.arachnidcreations.com/HonorBuddy/Skinbot.jpg";
            cbAvoidElites.Checked = EC.Settings.AvoidElites;
            cbDontFightWhileMounted.Checked = EC.Settings.DontStopToFight;
            cbGatherChests.Checked = EC.Settings.GatherChests;
            cbGatherMana.Checked = EC.Settings.GatherManaNodes;

            //foreach (var zone in Core.Zones)
            //{
            //    var parentZoneName = string.Empty;
            //    if (zone.ParentZone != 0)
            //    {
            //        var parentzone = Core.Zones.Where(z => z.Id == zone.ParentZone).FirstOrDefault();
            //        if (parentzone != null)
            //        {
            //            parentZoneName = parentzone.Name;
            //        }
            //    }

            //    var cb = new CheckBox() { Name = "cb"+zone.Name, Text= parentZoneName +"-" + zone.Name };
            //    if (EC.Settings.GatheringZones.Where(g => g.Id == zone.Id).FirstOrDefault() != null) cb.Checked = true;
            //    cb.Width = Convert.ToInt16(((Decimal)flowLayoutPanel1.Width)*0.8M);
            //    //cb.Width = cb.Parent.Width;
            //    cb.CheckedChanged += delegate {
            //        zone.GatherHere = cb.Checked;
            //        if (cb.Checked) EC.Settings.GatheringZones.Add(zone);
            //        else EC.Settings.GatheringZones.Remove(zone);
            //        EC.Settings.SaveSettings();

            //    };
            //    flowLayoutPanel1.Controls.Add(cb);
            //}


            //foreach (var zone in Core.Zones)
            //{
            //    var parentZoneName = string.Empty;
            //    if (zone.ParentZone != 0)
            //    {
            //        var parentzone = Core.Zones.Where(z => z.Id == zone.ParentZone).FirstOrDefault();
            //        if (parentzone != null)
            //        {
            //            parentZoneName = parentzone.Name;
            //        }
            //    }

            //    var cb = new CheckBox() { Name = "cb" + zone.Name, Text = parentZoneName + "-" + zone.Name };
            //    if (EC.Settings.AvoidZones.Where(g => g.Id == zone.Id).FirstOrDefault() != null) cb.Checked = true;
            //    cb.Width = Convert.ToInt16(((Decimal)flowLayoutPanel2.Width) * 0.8M);
            //    //cb.Width = cb.Parent.Width;
            //    cb.CheckedChanged += delegate {
            //        zone.GatherHere = cb.Checked;
            //        if (cb.Checked) EC.Settings.AvoidZones.Add(zone);
            //        else EC.Settings.AvoidZones.Remove(zone);
            //        EC.Settings.SaveSettings();

            //    };
            //    flowLayoutPanel2.Controls.Add(cb);
            //}
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            EclipseDataManager edb = new EclipseDataManager();
            edb.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                EC.log("!!!Setting BOT to Passive mode!!! (its not gonna do ANYTHING!)");
                Core.PassiveMode = true;
                Core.GatherMode = false;
                checkBox2.Checked = false;
            }
            else Core.PassiveMode = false;
        }


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                EC.log("!!!Setting BOT to GatheringLegion mode!!!");
                Core.GatherMode = true;
                Core.PassiveMode = false;
                checkBox1.Checked = false;
            }
            else Core.GatherMode = false;
        }



        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.thebuddyforum.com/thebuddystore/honorbuddy-store/honorbuddy-store-botbases/202085-eclipse-skin-bot-skinning-farming-travel-stack-combiner.html");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=GRHfa6O6Jxs&feature=youtu.be");
            
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/9FxP9");
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new EclipseDataManager().Show();
        }

        private void cbAvoidElites_CheckedChanged(object sender, EventArgs e)
        {
            EC.Settings.AvoidElites = cbAvoidElites.Checked;
            EC.Settings.SaveSettings();
        }

        private void cbDontFightWhileMounted_CheckedChanged(object sender, EventArgs e)
        {
            EC.Settings.DontStopToFight = cbDontFightWhileMounted.Checked;
            EC.Settings.SaveSettings();
        }

        private void cbGatherChests_CheckedChanged(object sender, EventArgs e)
        {
            EC.Settings.GatherChests = cbGatherChests.Checked;
            EC.Settings.SaveSettings();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (tbAvoidSearch.Text != string.Empty)
            {
                lbAvoids.DataSource = Core.Zones.Where(z => z.Name.ToLower().Contains(tbAvoidSearch.Text.ToLower())).ToList();
                lbAvoids.DisplayMember = "Name";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var item = (Zone)lbAvoids.SelectedItem;
            if (item != null)
            {
                EC.Settings.AvoidZones.Add(item);
                lbAvoidZones.DataSource = null;
                lbAvoidZones.DataSource = EC.Settings.AvoidZones;
                lbAvoidZones.DisplayMember = "Name";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var item = (Zone)lbAvoidZones.SelectedItem;
            if (item != null)
            {
                EC.Settings.AvoidZones.Remove(item);
                lbAvoidZones.DataSource = null;
                lbAvoidZones.DataSource = EC.Settings.AvoidZones;
                lbAvoidZones.DisplayMember = "Name";
            }
        }

        private void cbGatherMana_CheckedChanged(object sender, EventArgs e)
        {
            EC.Settings.GatherManaNodes = cbGatherMana.Checked;
            EC.Settings.SaveSettings();
        }
    }
}
