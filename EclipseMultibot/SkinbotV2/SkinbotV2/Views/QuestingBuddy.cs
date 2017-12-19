using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Xml.Linq;
using System.IO;
using Eclipse.Models;
using System.Reflection;
using Eclipse.MultiBot.Data;
using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.Bots.MultiBot;
using Styx.TreeSharp;
using Eclipse.MultiBot.Behaviors;

namespace Eclipse
{
    public partial class QuestingBuddy : Form
    {
        public static string ProfilePath = string.Empty;
        public static string MinLevel = string.Empty;
        public static string MaxLevel = string.Empty;
        private static LocalPlayer Me = StyxWoW.Me;
        EclipseProfile dt= new EclipseProfile();
        public QuestingBuddy()
        {
            InitializeComponent();
        }
        public void Pulse()
        {
            if (StyxWoW.Me != null)
            {
                dt.PosX = StyxWoW.Me.X.ToString();
                dt.PosY = StyxWoW.Me.Y.ToString();
                dt.PosZ = StyxWoW.Me.Z.ToString();
            }
            else
            {
                dt.PosX = 0.ToString();
                dt.PosY = 0.ToString();
                dt.PosZ = 0.ToString();

            }
        }
        private void EclipsePlugin_Load(object sender, EventArgs e)
        {
            pbEclipse.ImageLocation = "http://arachnidcreations.com/HonorBuddy/Image.aspx?image=Supernova-Eclipse2.jpg";
            cbQuestOrderType.DataSource = Enum.GetValues(typeof(QuestOrder.QOType));
            cbQObjectiveType.DataSource = Enum.GetValues(typeof(QuestObjective.QuestType));
            if (StyxWoW.Me != null)
            {
                try
                {
                    tbQX.DataBindings.Clear();
                    tbQY.DataBindings.Clear();
                    tbQZ.DataBindings.Clear();
                    tbVendorX.DataBindings.Clear();
                    tbVendorY.DataBindings.Clear();
                    tbVendorZ.DataBindings.Clear();
                    tbBSX.DataBindings.Clear();
                    tbBSY.DataBindings.Clear();
                    tbBSZ.DataBindings.Clear();

                    tbQX.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbQY.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbQZ.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbVendorX.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbVendorY.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbVendorZ.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbBSX.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbBSY.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbBSZ.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

                    tbQX.DataBindings.Add("Text", this.dt, "PosX");// = StyxWoW.Me.X.ToString();
                    tbQY.DataBindings.Add("Text", this.dt, "PosY");// = StyxWoW.Me.X.ToString();
                    tbQZ.DataBindings.Add("Text", this.dt, "PosZ");// = StyxWoW.Me.X.ToString();

                    tbVendorX.DataBindings.Add("Text", this.dt, "PosX");// = StyxWoW.Me.X.ToString();
                    tbVendorY.DataBindings.Add("Text", this.dt, "PosY");// = StyxWoW.Me.X.ToString();
                    tbVendorZ.DataBindings.Add("Text", this.dt, "PosZ");// = StyxWoW.Me.X.ToString();

                    tbBSX.DataBindings.Add("Text", this.dt, "PosX");// = StyxWoW.Me.X.ToString();
                    tbBSY.DataBindings.Add("Text", this.dt, "PosY");// = StyxWoW.Me.X.ToString();
                    tbBSZ.DataBindings.Add("Text", this.dt, "PosZ");// = StyxWoW.Me.X.ToString();

                    var quests = StyxWoW.Me.QuestLog.GetAllQuests();
                    List<QuestOrder> objectives = new List<QuestOrder>();
                    foreach (var q in quests)
                    {
                        objectives.AddRange(EC.QuestOrders.Where(qu => qu.QuestId == q.Id));

                    }
                    EC.Log("Finished adding QOs");
                    lbQuestOrders.DataSource = null;
                    lbQuestOrders.DataSource = objectives;
                    lbQuestOrders.DisplayMember = "DisplayName";
                    
                    EC.Log("Loaded Profile Form.");
                }
                catch (Exception err){
                    MessageBox.Show(err.Message +"|"+ err.StackTrace);
                    EC.Log(string.Format("Profile Load Error! {0}", err.ToString()));
                }
                
            }
        }

        private void loadData()
        {

            lbAvoidMobs.DataSource = null;
            lbBlackList.DataSource = null;
            lbVendors.DataSource = null;
            //lbQuests.DataSource = null;
            lbQuestOrders.DataSource = null;
            cbLogic.DataSource = null;

            lbVendors.DataSource = EclipseProfile.Vendors;
            lbVendors.DisplayMember = "Name";
            lbAvoidMobs.DataSource = EclipseProfile.AvoidMobs;
            lbAvoidMobs.DisplayMember = "Name";
            lbBlackList.DataSource = EclipseProfile.BlackSpots;
            lbBlackList.DisplayMember = "Name";
            //lbQuests.DataSource = EclipseProfile.QuestOverrides;
            //lbQuests.DisplayMember = "Name";
            lbQuestOrders.DataSource = EC.ActiveQuestQuestOrders;
            lbQuestOrders.DisplayMember = "DisplayName";
            cbLogic.DataSource = EclipseProfile.LogicBlocks;
            cbLogic.DisplayMember = "DisplayName";
            cbCustomBehaviors.DataSource = EclipseProfile.CustomBehaviors;
            cbCustomBehaviors.DisplayMember = "File";
            lbMailboxes.DataSource = EclipseProfile.MailBoxes;
            if (StyxWoW.Me != null)
            {
                lbActiveQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                lbActiveQuests.DisplayMember = "Name";
                tbQX.Text = StyxWoW.Me.X.ToString();
                tbQY.Text = StyxWoW.Me.Y.ToString();
                tbQZ.Text = StyxWoW.Me.Z.ToString();
            }
            if (ObjectManager.ObjectList != null)
            {
                lbNearbyMobs.DataSource =  ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => p.IsValid && p.DistanceSqr <= 40 * 40).ToList();
            }
        }

        private void btnAddAvoid_Click(object sender, EventArgs e)
        {
            var me = StyxWoW.Me;
            var mob = new EclipseMob { Name = me.CurrentTarget.Name, Entry= me.CurrentTarget.Entry.ToString() };
            EclipseProfile.AvoidMobs.Add(mob);
            loadData();
            EC.Log(string.Format("Added Avoid: {0}", mob.Name));

        }
        private void btnRemoveMob_Click(object sender, EventArgs e)
        {
            var avoid = (EclipseMob)lbAvoidMobs.SelectedItem;
            EclipseProfile.AvoidMobs.Remove(avoid);
            EC.Log(string.Format("Removed Avoid: {0}", avoid.Name));
            loadData();
            
        }

        private void lbActiveQuests_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (PlayerQuest)lbActiveQuests.SelectedItem;
        }
        private void lbNearbyMobs_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (WoWUnit)lbNearbyMobs.SelectedItem;
        }
        private void lbQuests_SelectedValueChanged(object sender, EventArgs e)
        {
            //propertyGrid1.SelectedObject = (QuestOverride)lbQuests.SelectedItem;
        }
        private void cbQuestOrderType_SelectedValueChanged(object sender, EventArgs e)
        {
            QuestOrder.QOType type;
            Enum.TryParse<QuestOrder.QOType>(cbQuestOrderType.SelectedValue.ToString(), out type);
            QuestObjective.QuestType qtype = QuestObjective.QuestType.CollectItem;
            if (cbQObjectiveType.SelectedValue != null) Enum.TryParse<QuestObjective.QuestType>(cbQObjectiveType.SelectedValue.ToString(), out qtype);
            if (type == QuestOrder.QOType.Objective)
            {
                cbQObjectiveType.Enabled = true;
                cbQObjectiveType.SelectedValueChanged += cbQuestOrderType_SelectedValueChanged;
                tbQGiverName.Enabled = false;
                tbQGiverId.Enabled = false;
                tbQTurninId.Enabled = false;
                tbQTurninName.Enabled = false;
                if (qtype == QuestObjective.QuestType.CollectItem)
                {
                    tbQKillCount.Enabled = false;
                    tbQMobId.Enabled = false;
                    tbQItemId.Enabled = true;
                    cbQItemBags.Enabled = true;
                    tbQCollectCount.Enabled = true;
                }
                if (qtype == QuestObjective.QuestType.KillMob)
                {
                    tbQKillCount.Enabled = true;
                    tbQMobId.Enabled = true;
                    tbQMobName.Enabled = true;
                    tbQItemId.Enabled = false;
                    tbQCollectCount.Enabled = false;
                    tbQItemId.Enabled = false;
                    tbQCollectCount.Enabled = false;
                }
                if (qtype == QuestObjective.QuestType.KillAndLootItem)
                {
                    tbQKillCount.Enabled = false;
                    tbQMobId.Enabled = true;
                    tbQMobName.Enabled = true;
                    tbQItemId.Enabled = true;
                    tbQCollectCount.Enabled = true;
                    tbQItemId.Enabled = true;
                }
                if (qtype == QuestObjective.QuestType.HangOutWithNPC)
                {
                    tbQKillCount.Enabled = false;
                    tbQMobId.Enabled = true;
                    tbQMobName.Enabled = true;
                    tbQItemId.Enabled = false;
                    cbQItemBags.Enabled = false;
                    tbQCollectCount.Enabled = true;
                }
            }
            else if (type == QuestOrder.QOType.UseItem)
            {
                tbQGiverName.Enabled = false;
                tbQGiverId.Enabled = false;
                tbQTurninId.Enabled = false;
                tbQTurninName.Enabled = false;
                tbQMobId.Enabled = true;
                tbQMobName.Enabled = true;
                tbQItemId.Enabled = true;
                cbQItemBags.Enabled = true;
                tbQCollectCount.Enabled = false;
                lblQKillCount.Text = "NumOfTimes";
                tbQKillCount.Enabled = true;
            }
            else if (type == QuestOrder.QOType.PickUp)
            {
                tbQGiverName.Enabled = true;
                tbQGiverId.Enabled = true;
                tbQTurninId.Enabled = false;
                tbQTurninName.Enabled = false;
                tbQItemId.Enabled = false;
                tbQCollectCount.Enabled = false;
                tbQKillCount.Enabled = false;
                tbQMobId.Enabled = false;
                cbQObjectiveType.Enabled = false;
                cbQItemBags.Enabled = false;
                tbQMobName.Enabled = false;
            }
            else if (type == QuestOrder.QOType.TurnIn)
            {
                tbQGiverName.Enabled = false;
                tbQGiverId.Enabled = false;
                tbQTurninId.Enabled = true;
                tbQTurninName.Enabled = true;
                tbQItemId.Enabled = false;
                tbQCollectCount.Enabled = false;
                tbQKillCount.Enabled = false;
                tbQMobId.Enabled = false;
                cbQItemBags.Enabled = false;
                tbQMobName.Enabled = false;
            }
        }
        private void lbQuestOrders_SelectedValueChanged(object sender, EventArgs e)
        {
            var qo = (QuestOrder)lbQuestOrders.SelectedItem;
            //if (qo.type == QuestOrder.QOType.CustomBehavior) qo = (CustomBehavior)qo;
            propertyGrid1.SelectedObject = qo;
            if (qo != null)
            {
                tbQMobId.Text = qo.MobId;
                tbQCollectCount.Text = qo.CollectCount.ToString();
                tbQGiverId.Text = qo.GiverId;
                tbQGiverName.Text = qo.GiverName;
                tbQItemId.Text = qo.ItemId;

                tbSpellId.Text = qo.SpellId.ToString();
                cbSpellName.Text = qo.SpellName;

                tbQKillCount.Text = qo.NumOfTimes;
                cbQItemBags.Text = qo.ItemName;
                tbQKillCount.Text = qo.KillCount.ToString();
                tbQMobName.Text = qo.MobName;
                if (cbQObjectiveType.DataSource != null) cbQObjectiveType.SelectedItem = qo.objectiveType;
                tbQId.Text = qo.QuestId.ToString();
                tbQName.Text = qo.QuestName;
                tbQTurninId.Text = qo.TurnInId;
                tbQTurninName.Text = qo.TurnInName;
                if (cbQuestOrderType.DataSource != null) cbQuestOrderType.SelectedItem = qo.type;
                tbQX.Text = qo.X.ToString();
                tbQY.Text = qo.Y.ToString();
                tbQZ.Text = qo.Z.ToString();
            }

        }
        private void getFromTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget == null)
                {
                    ObjectManager.Update();

                }
                if (StyxWoW.Me.GotTarget)
                {
                    // Try to cast the sender to a ToolStripItem
                    ToolStripItem menuItem = sender as ToolStripItem;
                    if (menuItem != null)
                    {
                        // Retrieve the ContextMenuStrip that owns this ToolStripItem
                        ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                        if (owner != null)
                        {
                            // Get the control that is displaying this context menu
                            Control sourceControl = owner.SourceControl;
                            sourceControl.Text = StyxWoW.Me.CurrentTarget.Name;
                        }
                    }
                }
            }
        }
        private void getFromSelectedQuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbActiveQuests.SelectedItem != null)
            {
                var Quest = (PlayerQuest)lbActiveQuests.SelectedItem;
                tbQId.Text = Quest.Id.ToString();
                tbQName.Text = Quest.Name;
            }
        }
        void np_FormClosed(object sender, FormClosedEventArgs e)
        {
            loadData();
        }

        private void btnAddQuestOrder_Click(object sender, EventArgs e)
        {
            EC.Log(DAL.DBFile);
            var qoType = (QuestOrder.QOType)Enum.Parse(typeof(QuestOrder.QOType), cbQuestOrderType.SelectedValue.ToString());
            var objType = (QuestObjective.QuestType)Enum.Parse(typeof(QuestObjective.QuestType), cbQObjectiveType.SelectedValue.ToString());
            if (qoType == QuestOrder.QOType.Objective && objType == QuestObjective.QuestType.CollectItem && tbQCollectCount.Text.Length == 0)
            {
                MessageBox.Show("You must have a CollectCount for a Collect Item Objective.");
                return;
            }
                QuestOrder qo = new QuestOrder();
                if (tbSpellId.Text != null) qo.SpellId = uint.Parse(tbSpellId.Text);
                qo.SpellName = cbSpellName.Text;
                qo.MobId = tbQMobId.Text;
                if (tbQCollectCount.Text != "") qo.CollectCount = int.Parse(tbQCollectCount.Text);
                else qo.CollectCount = 0;
                qo.GiverId = tbQGiverId.Text;
                qo.GiverName = tbQGiverName.Text;
                qo.ItemId = tbQItemId.Text;
                qo.NumOfTimes = tbQKillCount.Text;
                qo.ItemName = cbQItemBags.Text;
                if (tbQKillCount.Text != "") qo.KillCount = int.Parse(tbQKillCount.Text);
                else qo.KillCount = 0;
                qo.MobName = tbQMobName.Text;
                qo.objectiveType = (QuestObjective.QuestType)Enum.Parse(typeof(QuestObjective.QuestType), cbQObjectiveType.SelectedValue.ToString());
                qo.QuestId = uint.Parse(tbQId.Text);
                qo.QuestName = tbQName.Text;
                qo.TurnInId = tbQTurninId.Text;
                qo.TurnInName = tbQTurninName.Text;
                qo.type = (QuestOrder.QOType)Enum.Parse(typeof(QuestOrder.QOType), cbQuestOrderType.SelectedValue.ToString());
                qo.X = double.Parse(tbQX.Text);
                qo.Y = double.Parse(tbQY.Text);
                qo.Z = double.Parse(tbQZ.Text);
                EC.QuestOrders.Add(qo);
                DAL.ExecuteSL3Query(ORM.Insert(qo, "QuestOrders","", false, DAL.getTableStructure("QuestOrders")));
                EC.Log(string.Format("Quest Order Added Type: {0}", qo.type));
                EC.ActiveQuestQuestOrders.Add(qo);

                lbQuestOrders.DataSource = null;
                lbQuestOrders.DataSource = EC.ActiveQuestQuestOrders;
                lbQuestOrders.DisplayMember = "DisplayName";
                
          }
        private void refreshQuestOrders()
        {
            lbQuestOrders.DataSource = null;
            lbQuestOrders.DataSource = EC.ActiveQuestQuestOrders;
            lbQuestOrders.DisplayMember = "DisplayName";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                lbActiveQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                lbActiveQuests.DisplayMember = "Name";
                EC.Log(string.Format("Quests Refreshed."));
            }
        }
        private void btnAddVendor_Click(object sender, EventArgs e)
        {
            EclipseVendor vendor = new EclipseVendor { Name = tbVendorName.Text, Entry = uint.Parse(tbVendorId.Text), Type = cbVendorType.Text, X = float.Parse(tbVendorX.Text), Y = float.Parse(tbVendorY.Text), Z = float.Parse(tbVendorZ.Text) };
            EclipseProfile.Vendors.Add(vendor);
            EC.Log(string.Format("Vendor Added: {0}", vendor.Name));
            loadData();
        }
        private void btnRemoveVendor_Click(object sender, EventArgs e)
        {
            var vendor = (EclipseVendor)lbVendors.SelectedItem;
            EclipseProfile.Vendors.Remove(vendor);
            EC.Log(string.Format("Quest Order Added Type: {0}", vendor.Name));
            loadData();
        }
        private void lbVendors_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (EclipseVendor)lbVendors.SelectedItem;
        }
        private void getMobFromTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget == null)
                {
                    ObjectManager.Update();

                }
                if (StyxWoW.Me.GotTarget)
                {
                    Control control = (Control)sender;
                    control.Text = StyxWoW.Me.CurrentTarget.Name;
                }
            }

        }
        private void getIdFromTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    // Try to cast the sender to a ToolStripItem
                    ToolStripItem menuItem = sender as ToolStripItem;
                    if (menuItem != null)
                    {
                        // Retrieve the ContextMenuStrip that owns this ToolStripItem
                        ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                        if (owner != null)
                        {
                            // Get the control that is displaying this context menu
                            Control sourceControl = owner.SourceControl;
                            sourceControl.Text = StyxWoW.Me.CurrentTarget.Entry.ToString();
                        }
                    }
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                tbQX.Text = StyxWoW.Me.X.ToString();
                tbQY.Text = StyxWoW.Me.Y.ToString();
                tbQZ.Text = StyxWoW.Me.Z.ToString();
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    // Try to cast the sender to a ToolStripItem
                    ToolStripItem menuItem = sender as ToolStripItem;
                    if (menuItem != null)
                    {
                        // Retrieve the ContextMenuStrip that owns this ToolStripItem
                        ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                        if (owner != null)
                        {
                            // Get the control that is displaying this context menu
                            Control sourceControl = owner.SourceControl;
                            //sourceControl.Text = 
                        }
                    }
                }
            }
        }
        private void getItemsFromBagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    // Try to cast the sender to a ToolStripItem
                    ToolStripItem menuItem = sender as ToolStripItem;
                    if (menuItem != null)
                    {
                        // Retrieve the ContextMenuStrip that owns this ToolStripItem
                        ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                        if (owner != null)
                        {
                            // Get the control that is displaying this context menu
                            Control sourceControl = owner.SourceControl;
                            cbQItemBags.DataSource = StyxWoW.Me.BagItems;
                            cbQItemBags.DisplayMember = "Name";
                            cbQItemBags.ValueMember = "Entry";
                            cbQItemBags.ValueMemberChanged += cbQItemBags_ValueMemberChanged;
                            tbQItemId.Text = cbQItemBags.SelectedValue.ToString();
                            //sourceControl.Text = 
                        }
                    }
                }
            }
        }
        void cbQItemBags_ValueMemberChanged(object sender, EventArgs e)
        {
            tbQItemId.Text = cbQItemBags.SelectedValue.ToString();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                cbQItemBags.DataSource = null;
                cbQItemBags.DataSource = StyxWoW.Me.CarriedItems.OrderBy(b=>b.Name).ToList();
                cbQItemBags.DisplayMember = "Name";
                cbQItemBags.ValueMember = "Entry";
                EC.Log(string.Format("Bag Items Refreshed."));
            }
        }
        private void btnAddQOAbove_Click(object sender, EventArgs e)
        {
            QuestOrder qo = createQuestOrder();
            var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
            var index = EclipseProfile.QuestOrders.IndexOf(selectedQO);
            EclipseProfile.QuestOrders.Insert(index, qo);
            EC.Log(string.Format("Added Quest Order Type:{0} ABOVE {1}", qo.type, selectedQO.type));
            refreshQuestOrders();
        }
        private QuestOrder createQuestOrder()
        {
            QuestOrder qo = new QuestOrder();
            qo.MobId = tbQMobId.Text;
            qo.CollectCount = int.Parse(tbQCollectCount.Text);
            qo.GiverId = tbQGiverId.Text;
            qo.GiverName = tbQGiverName.Text;
            qo.ItemId = tbQItemId.Text;
            qo.ItemName = cbQItemBags.Text;
            qo.KillCount = int.Parse(tbQKillCount.Text);
            qo.MobName = tbQMobName.Text;
            qo.objectiveType = (QuestObjective.QuestType)Enum.Parse(typeof(QuestObjective.QuestType), cbQObjectiveType.SelectedValue.ToString());
            qo.QuestId = uint.Parse(tbQId.Text);
            qo.QuestName = tbQName.Text;
            qo.TurnInId = tbQTurninId.Text;
            qo.TurnInName = tbQTurninName.Text;
            qo.type = (QuestOrder.QOType)Enum.Parse(typeof(QuestOrder.QOType), cbQuestOrderType.SelectedValue.ToString());
            qo.X = double.Parse(tbQX.Text);
            qo.Y = double.Parse(lblQY.Text);
            qo.Z = double.Parse(tbQZ.Text);
            return qo;
        }
        private void btnAddQOBelow_Click(object sender, EventArgs e)
        {
            QuestOrder qo = createQuestOrder();
            var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
            var index = EclipseProfile.QuestOrders.IndexOf(selectedQO);
            EclipseProfile.QuestOrders.Insert(index+1, qo);
            EC.Log(string.Format("Added Quest Order Type:{0} BELOW {1}", qo.type, selectedQO.type));
            refreshQuestOrders();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
            EclipseProfile.QuestOrders.Remove(selectedQO);
            lbQuestOrders.DataSource = null;
            lbQuestOrders.DataSource = EC.ActiveQuestQuestOrders;
            lbQuestOrders.DisplayMember = "DisplayName";
            EC.Log(string.Format("Removed Quest Order Type:{0} ", selectedQO.type));
        }
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
                var index = EclipseProfile.QuestOrders.IndexOf(selectedQO);
                EclipseProfile.QuestOrders.Remove(selectedQO);
                EclipseProfile.QuestOrders.Insert(index - 1, selectedQO);
                lbQuestOrders.DataSource = null;
                lbQuestOrders.DataSource = EC.ActiveQuestQuestOrders;
                lbQuestOrders.DisplayMember = "DisplayName";
                lbQuestOrders.SelectedItem = selectedQO;

            }
            catch (ArgumentOutOfRangeException err)
            {
            }
        }
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
            try
            {
                
                var index = EclipseProfile.QuestOrders.IndexOf(selectedQO);
                EclipseProfile.QuestOrders.Remove(selectedQO);
                EclipseProfile.QuestOrders.Insert(index, selectedQO);
                lbQuestOrders.DataSource = null;
                lbQuestOrders.DataSource = EC.ActiveQuestQuestOrders;
                lbQuestOrders.DisplayMember = "DisplayName";
                lbQuestOrders.SelectedItem = selectedQO;
            }
            catch (ArgumentOutOfRangeException err)
            {
            }
        }
        private void btnRefreshNearby_Click(object sender, EventArgs e)
        {
            if (ObjectManager.ObjectList != null)
            {
                lbNearbyMobs.DataSource = null;
                lbNearbyMobs.DataSource = ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => p.IsValid && p.DistanceSqr <= 40 * 40).ToList();
                EC.Log(string.Format("Getting nearby WowUnits."));
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            var qo = (QuestOrder)lbQuestOrders.SelectedItem;
            tbQMobId.Text = qo.MobId;
            tbQCollectCount.Text = qo.CollectCount.ToString();
            tbQGiverId.Text = qo.GiverId;
            tbQGiverName.Text = qo.GiverName;
            tbQItemId.Text = qo.ItemId;
            cbQItemBags.Text = qo.ItemName;
            tbQKillCount.Text = qo.KillCount.ToString();
            if (qo.MobName != null) tbQMobName.Text = qo.MobName.ToString();
            cbQObjectiveType.SelectedItem = qo.objectiveType;
            tbQId.Text=qo.QuestId.ToString();
            tbQName.Text = qo.QuestName;
            tbQTurninId.Text = qo.TurnInId;
            tbQTurninName.Text = qo.TurnInName;
            cbQuestOrderType.SelectedItem = qo.type;
            tbQX.Text = qo.X.ToString();
            lblQY.Text = qo.Y.ToString();
            tbQZ.Text = qo.Z.ToString();
        }
        private void pbRefreshCBs_Click(object sender, EventArgs e)
        {

            cbCustomBehaviors.DataSource = null;
            cbCustomBehaviors.DataSource = EclipseProfile.CustomBehaviors;
            cbCustomBehaviors.DisplayMember = "file";
            cbLogic.DataSource = null;
            cbLogic.DataSource = EclipseProfile.LogicBlocks;
            cbLogic.DisplayMember = "DisplayName";
            if (StyxWoW.Me != null)
            {
                lbActiveQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                lbActiveQuests.DisplayMember = "Name";
            }
            EC.Log(string.Format("Refreshing Custom Behaviors."));
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            loadData();
        }
        private void tbGetGiverInfo_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    EC.Log("Getting Target Information.");
                    tbQGiverId.Text = StyxWoW.Me.CurrentTarget.Entry.ToString();
                    tbQGiverName.Text = StyxWoW.Me.CurrentTarget.Name;
                    
                }
            }
        }

        private void btnGetTurninInfo_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me.CurrentTarget == null)
            {
                ObjectManager.Update();

            }
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    EC.Log("Getting Target Information.");
                    tbQTurninId.Text = StyxWoW.Me.CurrentTarget.Entry.ToString();
                    tbQTurninName.Text = StyxWoW.Me.CurrentTarget.Name;
                }
            }
        }

        private void btnGetMobInfo_Click_1(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                EC.Log("Getting Target Information.");
                tbQX.Text = StyxWoW.Me.CurrentTarget.X.ToString();
                tbQY.Text = StyxWoW.Me.CurrentTarget.Y.ToString();
                tbQZ.Text = StyxWoW.Me.CurrentTarget.Z.ToString();

            }
        }

        private void cbQItemBags_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbQItemId.Text = cbQItemBags.SelectedValue.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string target = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=GAXEB25MFRKXE";
            try
                {
                 System.Diagnostics.Process.Start(target);
                }
            catch
                ( 
                 System.ComponentModel.Win32Exception noBrowser) 
                {
                 if (noBrowser.ErrorCode==-2147467259)
                  MessageBox.Show(noBrowser.Message);
                }
            catch (System.Exception other)
                {
                  MessageBox.Show(other.Message);
                }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            EC.Log("Adding Mailbox.");
            EclipseProfile.MailBoxes.Add(new MailBox { X = dt.PosX, Y = dt.PosY, Z = dt.PosZ });
            loadData();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget == null) ObjectManager.Update();
                if (StyxWoW.Me.GotTarget)
                {
                    EC.Log("Getting Target Information.");
                    tbQMobId.Text = StyxWoW.Me.CurrentTarget.Entry.ToString();
                    tbQMobName.Text = StyxWoW.Me.CurrentTarget.Name;
                }
            }
        }
        private void btnSearchNearby_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                EC.Log("Searching Nearby Objects");
                //lbNearbyMailboxes
                var objs = ObjectManager.ObjectList.Where(n => n.Name.Contains(tbSearchNearby.Text) || n.SafeName.Contains(tbSearchNearby.Text)).ToList();
                lbNearbyGameObjects.DataSource = objs;
            }
            else
            {
                NotAttached();
            }
        }
        public void NotAttached()
        {
            MessageBox.Show("You are not attached to the game or your HB is not up to date. Press start on your HB client to update the in-game list.");
            EC.Log("You are not attached to the game or your HB is not up to date. Press start on your HB client to update the in-game list.");
        }

        private void lbNearbyMailboxes_DoubleClick(object sender, EventArgs e)
        {
            var obj = (WoWObject)lbNearbyGameObjects.SelectedItem;
            if (obj != null)
            {
                tbQMobId.Text = obj.Entry.ToString();
                tbQMobName.Text = obj.Name;
            }

        }

        private void lbActiveQuests_SelectedValueChanged_1(object sender, EventArgs e)
        {
                var Quest = (PlayerQuest)lbActiveQuests.SelectedItem;
                if (Quest != null)
                {
                    tbQId.Text = Quest.Id.ToString();
                    tbQName.Text = Quest.Name;
                }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            WoWUnit target = null;
            if (StyxWoW.Me != null) target = StyxWoW.Me.CurrentTarget;
            if (target != null)
            {
                EC.Log("Getting Target Information.");
                tbVendorId.Text = target.Entry.ToString();
                tbVendorName.Text = target.Name;
                tbVendorX.Text = target.X.ToString();
                tbVendorY.Text = target.Y.ToString();
                tbVendorZ.Text = target.Z.ToString();
            }
        }

        private void btnAddBlackspot_Click(object sender, EventArgs e)
        {
            EC.Log("Adding BlackSpot");
            Blackspot bs = new Blackspot { Name=tbBSName.Text, Radius=tbBSRadius.Text, X = tbBSX.Text, Y=tbBSY.Text, Z=tbBSZ.Text };
            EclipseProfile.BlackSpots.Add(bs);
            loadData();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {

            if (StyxWoW.Me != null)
            {
                EC.Log("Refreshing ActiveQuests.");
                lbActiveQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                lbActiveQuests.DisplayMember = "Name";
            }
        }

        private void btnRemoveQuest_Click(object sender, EventArgs e)
        {
            var qo = (QuestOrder)lbQuestOrders.SelectedItem;
            DAL.ExecuteSL3Query(string.Format("Delete from questorders where id = {0}", qo.id));
            EclipseProfile.QuestOrders.Remove(qo);
            EC.QuestOrders.Remove(qo);
            EC.ActiveQuestQuestOrders.Remove(qo);
            EC.Log(string.Format("Removing Quest Type:{0}", qo.type));
            reLoadQOs();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            reLoadQOs();

        }
        private void reLoadQOs()
        {
            var quests = StyxWoW.Me.QuestLog.GetAllQuests();
            List<QuestOrder> objectives = new List<QuestOrder>();
            foreach (var q in quests)
            {
                objectives.AddRange(EC.QuestOrders.Where(qu => qu.QuestId == q.Id));

            }
            lbQuestOrders.DataSource = null;
            lbQuestOrders.DataSource = objectives;
            lbQuestOrders.DisplayMember = "DisplayName";
        }
        private void btnRemoveBlackSpot_Click(object sender, EventArgs e)
        {
            var bs = (Blackspot)lbBlackList.SelectedItem;
            EclipseProfile.BlackSpots.Remove(bs);
            loadData();
        }
        private int count = 3;
        private string text = string.Empty;
        private void addChildren(Control control){
            
            foreach (Control cont in control.Controls)
            {
                if (cont.Text != string.Empty)
                {
                    count++;
                    text += string.Format("insert into Translations (id,language, key, value) values ({0}, 'CN', '{1}', '',{0});\r\n", count, cont.Text);
                    text += string.Format("insert into TranslationControls (id, name) values ({0}, '{1}');\r\n", count, cont.Name);
                }
                if (cont.HasChildren) addChildren(cont);
            }
            
        }

        private void btnRunSql_Click(object sender, EventArgs e)
        {
            var dt = DAL.LoadSL3Data(tbSql.Text);
            dataGridView1.DataSource = dt;
        }

        private void btnGetTables_Click(object sender, EventArgs e)
        {
            DataTable dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table';");
            lbTables.DataSource = dt;
            lbTables.DisplayMember = "Name";

        }

        private void btnGetTables_Click_1(object sender, EventArgs e)
        {
            DataTable dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table';");
            lbTables.DataSource = dt;
            lbTables.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EC.ActiveQuestOrder = (QuestOrder)lbQuestOrders.SelectedItem;
        }

        private void lbNearbyGameObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            var obj = (WoWObject)lbNearbyGameObjects.SelectedItem;
            if (obj != null)
            {
                tbQItemId.Text = obj.Entry.ToString();
                cbQItemBags.Text = obj.Name;
            }
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            getProperty("Eclipse.MultiBot.Behaviors.TestClass","testBT");
        }
        private Composite getProperty(string className, string propertyName)
        {
            Type t = Type.GetType(className);
            var instance = Activator.CreateInstance(t);
            return (Composite)instance.GetType().GetProperty(propertyName).GetValue(instance);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var spell = SpellManager.Spells.Where(s => s.Key == cbSpellName.Text).FirstOrDefault().Value;
            tbSpellId.Text = spell.Id.ToString();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                cbSpellName.DataSource = null;
                cbSpellName.DataSource = SpellManager.Spells.OrderBy(s=>s.Key).ToList();
                cbSpellName.DisplayMember = "Key";
                cbSpellName.ValueMember = "Value";
                EC.Log(string.Format("Bag Items Refreshed."));
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }
    }
}
