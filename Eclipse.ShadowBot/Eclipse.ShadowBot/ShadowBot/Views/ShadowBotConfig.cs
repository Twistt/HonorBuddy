using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.Comms;
using Eclipse.ShadowBot.Data;
using Styx;
using Styx.CommonBot;
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

namespace Eclipse.ShadowBot
{
    public partial class ShadowBotConfig : Form
    {
        private bool ShowAdvanced = false;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public ShadowBotConfig()
        {
            InitializeComponent();
            timer.Interval = 500;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSettings();
            EclipseShadowBot.Client = new WoWClient();
            EclipseShadowBot.Client.ServerIP = tbServerIP.Text;
            EclipseShadowBot.Client.Connect();
        }
        private void SaveSettings()
        {
            EclipseShadowBot.settings = new ShadowBotSettings();
            if (Styx.CommonBot.TreeRoot.IsRunning)
            {
                if (EclipseShadowBot.Me.Role != WoWPartyMember.GroupRole.Healer && checkboxHealBotMode.Checked) MessageBox.Show("You have chosen the heal only mode - but your group role is not set to healer. Your bot will not heal or dps if you dont set your role.");

                EclipseShadowBot.AssistLeader = boolAssistLeader.Checked;
                EclipseShadowBot.PickUpQuests = boolGetQuests.Checked;
                EclipseShadowBot.FollowDistance = int.Parse(tbFollowDistance.Text);
                EclipseShadowBot.HealBotMode = checkboxHealBotMode.Checked;
                EclipseShadowBot.LootMobs = boolLootMobs.Checked;
                EclipseShadowBot.SkinMobs = boolSkinMobs.Checked;
                EclipseShadowBot.FollowName = tbFollowName.Text;
                EclipseShadowBot.FollowByName = boolFollowByName.Checked;

                EclipseShadowBot.settings.LootMobs = EclipseShadowBot.LootMobs;
                EclipseShadowBot.settings.AssistLeader = EclipseShadowBot.AssistLeader;
                EclipseShadowBot.settings.PickUpQuests = EclipseShadowBot.PickUpQuests;
                EclipseShadowBot.settings.FollowDistance = EclipseShadowBot.FollowDistance;
                EclipseShadowBot.settings.HealBotMode = EclipseShadowBot.HealBotMode;
                EclipseShadowBot.settings.SkinMobs = EclipseShadowBot.SkinMobs;
                EclipseShadowBot.settings.CharacterName = EclipseShadowBot.Me.Name;
                EclipseShadowBot.settings.FollowByName = boolFollowByName.Checked;
                EclipseShadowBot.settings.FollowName = EclipseShadowBot.FollowName;
                EclipseShadowBot.FreeBagSlots = int.Parse(tbFreeBagSlots.Text);

                if (boolFollowByName.Checked == false)
                {
                    EclipseShadowBot.Leader = (WoWPlayer)EclipseShadowBot.Me.CurrentTarget;
                    lblTarget.Text = EclipseShadowBot.Leader.Name;
                }
                else
                {
                    EclipseShadowBot.Leader = (WoWPlayer)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Player && n.Name == tbFollowName.Text).FirstOrDefault();
                    if (EclipseShadowBot.Leader == null)
                    {
                        TreeRoot.StatusText = "Waiting for leader to get in range.";
                        lblTarget.Text = tbFollowName.Text;
                    }

                }
                EclipseShadowBot.settings.SaveOrCreate();
                try
                {

                    //This starts the server by returning a port number it will be called on.
                    //CommsCommon.cc.DoWork(new WowMessage() { Type = "LetsBeFriends", Name = StyxWoW.Me.Name });
                    timer.Start();
                }

                catch (Exception err)
                {
                    EC.Log("Error..... " + err.StackTrace);
                }
            }
            else MessageBox.Show("The bot must be running in order to choose a leader.");
        }
        private void ShadowBotConfig_Load(object sender, EventArgs e)
        {
            EC.FindDB();
            DataTable dt = null;
            if (StyxWoW.Me != null) dt = DAL.LoadSL3Data(string.Format("Select * from ShadowBotSettings where CharacterName = '{0}'", StyxWoW.Me.Name));
            if (dt != null){
                if (dt.Rows.Count > 0)
                {
                    EC.Log("Loading Settings...");
                    EclipseShadowBot.settings = (ShadowBotSettings)ORM.convertDataRowtoObject(new ShadowBotSettings(), dt.Rows[0]);
                    boolAssistLeader.Checked = EclipseShadowBot.settings.AssistLeader;
                    boolLootMobs.Checked = EclipseShadowBot.settings.LootMobs;
                    boolGetQuests.Checked = EclipseShadowBot.settings.PickUpQuests;
                    checkboxHealBotMode.Checked = EclipseShadowBot.settings.HealBotMode;
                    tbFollowDistance.Text = EclipseShadowBot.settings.FollowDistance.ToString();
                    boolSkinMobs.Checked = EclipseShadowBot.settings.SkinMobs;
                    boolLootMobs.Checked = EclipseShadowBot.settings.LootMobs;
                    boolFollowByName.Checked = EclipseShadowBot.settings.FollowByName;
                    tbFollowName.Text = EclipseShadowBot.settings.FollowName;
                    EC.Log("Finished loading settings...");
                }
            }
            timer.Tick += timer_Tick;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EclipseShadowBot.LeaderMode = true;
            EclipseShadowBot.Server = new WoWServer();
            try
            {
                EclipseShadowBot.Server.Start();
                timer.Start();
                WoWServer.Log("Starting server...");
                WoWServer.UpdateUI = true;
            }
            catch (Exception err)
            {
                EC.Log("Error..... " + err.StackTrace);
            }
            listBox1.DataSource = WoWServer.Clients;
            listBox1.DisplayMember = "Display";
        }
        void timer_Tick(object sender, EventArgs e)
        {
            WoWServer.UpdateUI = true;
            if (EclipseShadowBot.LeaderMode && WoWServer.UpdateUI)
            {
                listBox1.DataSource = null;
                listBox1.DataSource = WoWServer.Clients;
                listBox1.DisplayMember = "Display";

                lbServerLog.DataSource = null;
                lbServerLog.DataSource = WoWServer.RunningLog.OrderByDescending(d=>d).ToList();

                cbClients.DataSource = null;
                cbClients.DataSource = WoWServer.Clients;
                cbClients.DisplayMember = "Display";
                WoWServer.UpdateUI = false;
            }
            if (!EclipseShadowBot.LeaderMode && WoWServer.UpdateUI)
            {
                lbMessages.DataSource = null;
                lbMessages.DataSource = EclipseShadowBot.Client.Messages;
                lbMessages.DisplayMember = "Type";
                WoWServer.UpdateUI = false;
            }
        }
        private void boolFollowByName_CheckedChanged(object sender, EventArgs e)
        {
            if (boolFollowByName.Checked) tbFollowName.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (StyxWoW.Me != null) EclipseShadowBot.Server.MessageClients(new WowMessage() { Type = cbClientMessages.Text, X = StyxWoW.Me.Location.X, Y = StyxWoW.Me.Location.Y, Z = StyxWoW.Me.Location.Z  });
            else { EclipseShadowBot.Server.MessageClients(new WowMessage() { Type = cbClientMessages.Text, X = 1, Y = 2, Z = 3 }); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var client = (WoWClient)cbClients.SelectedItem;
            if (client != null)
            {
                try
                {
                    EclipseShadowBot.Server.SendMessage(new WowMessage() { Type = cbClientMessages.Text }, client);
                }
                catch (Exception err)
                {
                    WoWServer.Clients.Remove(client.client);
                }
            }

        }

        private void btnShowAdvanced_Click(object sender, EventArgs e)
        {
            if (ShowAdvanced)
            {
                gbServerSettings.Hide();
            }
            if (!ShowAdvanced) gbServerSettings.Show();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void ShadowBotConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            WoWServer.RunServer = false;
            foreach (var c in WoWServer.Clients)
            {
                try
                {
                    c.Close();
                }
                catch (Exception) { }
            }
            if (EclipseShadowBot.Client != null) EclipseShadowBot.Client.client.Close();
        }
    }
}
