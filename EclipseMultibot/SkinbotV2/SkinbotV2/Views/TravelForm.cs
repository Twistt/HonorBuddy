using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.Models;
using Styx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eclipse.Bots.MultiBot.Views
{
    public partial class TravelForm : Form
    {
        public TravelForm()
        {
            InitializeComponent();
        }

        private void TravelForm_Load(object sender, EventArgs e)
        {
            lbFavoritePlaces.DataSource = EC.FavoritePlaces;
            lbFavoritePlaces.DisplayMember = "Name";

            lbNpcLocations.DataSource = EC.NPCs;
            lbNpcLocations.DisplayMember = "Name";

            lbMobs.DataSource = EC.MOBs;
            lbMobs.DisplayMember = "Name";
        }

        private void lbNpcLocations_SelectedValueChanged(object sender, EventArgs e)
        {
            var npc = (NPC)lbNpcLocations.SelectedItem;
            pictureBox1.ImageLocation = EC.WebRequest("http://services.acsoft.us/api/Scraper/GetWowHeadImage/?type=npc&id=" + npc.Entry).Replace("\"", "");

        }

        private void btnAddFavorite_Click(object sender, EventArgs e)
        {
            var loc = new Location { Zone = StyxWoW.Me.ZoneId, X = StyxWoW.Me.X, Y=StyxWoW.Me.Y, Z=StyxWoW.Me.Z, Name=tbFavName.Text };
            DAL.ExecuteSL3Query(ORM.Insert(loc, "Favorites", "", false, DAL.getTableStructure("Favorites")));
            EC.FavoritePlaces.Add(loc);
            EC.Log("Added new favorite location.");
            lbFavoritePlaces.DataSource = null;
            lbFavoritePlaces.DataSource = EC.FavoritePlaces;
            lbFavoritePlaces.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbNpcLocations.DataSource = null;
            lbNpcLocations.Update();
            var list = EC.NPCs.Where(n => n.Name.ToLower().Contains(tbSearchNPC.Text.ToLower())).ToList();
            lbNpcLocations.DataSource = list;
            lbNpcLocations.DisplayMember = "Name";
            lbNpcLocations.Update();

        }

        private void btnSearchFavs_Click(object sender, EventArgs e)
        {
            lbFavoritePlaces.DataSource = null;
            var list = EC.FavoritePlaces.Where(n => n.Name.ToLower().Contains(tbSearchFavs.Text.ToLower())).ToList();
            lbFavoritePlaces.DataSource = list;
            lbFavoritePlaces.DisplayMember = "Name";
        }

        private void btnSearchMobs_Click(object sender, EventArgs e)
        {
            
            lbMobs.DataSource = null;
            var list = EC.MOBs.Where(n => n.Name.ToLower().Contains(tbSearchMobs.Text.ToLower())).ToList();
            lbMobs.DataSource = list;
            lbMobs.DisplayMember = "Name";
        }
        private void btnGOTOFav_Click(object sender, EventArgs e)
        {

            var loc = (Location)lbFavoritePlaces.SelectedItem;
            EC.Log(string.Format("Setting Nav point to {0},{1}, {2}", loc.X, loc.Y, loc.Z));
            EC.ForceNav = true;
            EC.ForceNavLocation = loc;

        }
        private void btnGOTOMob(object sender, EventArgs e)
        {
            var mob = (Mob)lbMobs.SelectedItem;
            var loc = EC.Locations.Where(l => l.Entry == mob.Entry).OrderBy(d => EC.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me.Z })).FirstOrDefault();
            EC.Log(string.Format("Setting Nav point to {0},{1}, {2}", loc.X, loc.Y, loc.Z));
            EC.ForceNav = true;
            EC.ForceNavLocation = loc;
        }
        private void btnGOTONpc_Click(object sender, EventArgs e)
        {
            var npc = (NPC)lbNpcLocations.SelectedItem;
            var loc = new Location { Entry = npc.Entry, X = npc.X, Y = npc.Y, Z = npc.Z };
            EC.Log(string.Format("Setting Nav point to {0},{1}, {2}", loc.X, loc.Y, loc.Z));
            EC.ForceNav = true;
            EC.ForceNavLocation = loc;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            EC.Log("Clearing Nav Point.");
            EC.ForceNav = false;
            EC.ForceNavLocation = null;
        }
    }
}
