using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse;
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
using TwistedCombatRoutines;
using TwistedCombatRoutines.Data;

namespace TwistedCombat3.TwistedCombat.Views
{
    public partial class ConfigGui : Form
    {
        public ConfigGui()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(16, 16, 16);
            listBox1.DataSource = EC.CR.THealingBehaviors;
            listBox1.DisplayMember = "Display";

            listBox2.DataSource = EC.CR.TCombatBehaviors;
            listBox2.DisplayMember = "Display";

            listBox3.DataSource = EC.CR.TRestBehaviors;
            listBox3.DisplayMember = "Display";

            listBox4.DataSource = EC.CR.TPullBehaviors;
            listBox4.DisplayMember = "Display";

            cbSpellName.DataSource = SpellManager.Spells.Values.Where(p => !p.HasAttribute(SpellAttributes.Passive)).OrderBy(o => o.LocalizedName).ToList();
            cbSpellName.DisplayMember = "LocalizedName";

            cbTargetType.DataSource = Enum.GetValues(typeof(TargetType));
            cbAuraTarget.DataSource = Enum.GetValues(typeof(AuraTarget));
            cbOperator.DataSource = Enum.GetValues(typeof(Operator));


            lblClass.Text = EC.CR.WowClass;
            lblSpec.Text = EC.CR.WowSpec;
            lblRoutineName.Text = EC.CR.RoutineName;
        }

        private void tbAddToRoutine_Click(object sender, EventArgs e)
        {
            //get listbox from active tab
            CombatBehavior cb = new CombatBehavior();
            cb.RoutineId = EC.CR.Id;
            cb.Target = (TargetType)Enum.Parse(typeof(TargetType), cbTargetType.SelectedValue.ToString());
            cb.Aura = (AuraTarget)Enum.Parse(typeof(AuraTarget), cbAuraTarget.SelectedValue.ToString());
            cb.HealthOperator = (Operator)Enum.Parse(typeof(Operator), cbOperator.SelectedValue.ToString());
            if (tbSpellId.Text.Length == 0)
            {
                MessageBox.Show("You must choose a spell to cast with the rules you have chosen...");
                return;
            }
            if (tbSpellId.Text.Length > 0 && tbItemId.Text.Length > 0)
            {
                MessageBox.Show("You must choose an item to use (such as a trinket) OR a spell - you cannot choose both.");
                return;
            }
            else
            {
                if (tbSpellId.Text.Length > 0) cb.IsSpell = true;
                if (tbItemId.Text.Length > 0) cb.IsItem = true;
            }

            if (cb.Target != TargetType.None)
            {
                cb.HasTargetingRules = true;
            }

            if (cb.Aura != AuraTarget.None)
            {
                cb.IsAura = true;
            }

            if (cb.IsAura == true)
            {
                if (tbAuraId.Text != string.Empty) cb.AuraId = int.Parse(tbAuraId.Text);
                cb.AuraName = cbAuraName.Text;
                if (cb.AuraName.Length == 0 && cb.AuraId == 0)
                {
                    MessageBox.Show("You have selected to keep an aura up but have not specified which aura to keep up.");
                    return;
                }
            }
            if (cb.IsAura && cb.HasTargetingRules)
            {
                var dialogresult = MessageBox.Show("You have selected a Target AND an aura - this is usually only used for \"Procs\" - was this intended?", "Add an Aura by Target", MessageBoxButtons.YesNo);
                if (dialogresult == System.Windows.Forms.DialogResult.No) return;
            }

            cb.CastAtHealthPercentage = cbCastAtHealth.Checked;
            cb.IsInterrupt = cbIsInterrupt.Checked;
            cb.RequirePet = cbRequirePet.Checked;
            cb.UseInGroups = cbUseInGroup.Checked;
            cb.RequirePet = cbRequirePet.Checked;
            cb.UseInGroups = cbUseInGroup.Checked;
            cb.PetIsDead = cbPetisDead.Checked;
            cb.HasNoPet = cbPetNotSummoned.Checked;
            cb.IsIncapacitated = cbIncapacitated.Checked;
            cb.UseTanksTarget = cbUseTanksTarget.Checked;
            //if (cbTarget.Text != string.Empty) cb.Target = (TargetType)Enum.Parse(typeof(TargetType), cbTarget.Text);

            if (tbPercent.Text != string.Empty) cb.HealthPercentage = double.Parse(tbPercent.Text);
            cb.RequireMeleeRange = cbMeleeRange.Checked;
            if ((tbRange.Text) != string.Empty) cb.CastRange = int.Parse(tbRange.Text);

            if (tbItemId.Text != string.Empty) cb.TrinketId = int.Parse(tbItemId.Text);
            cb.TrinketName = cbItemSpellName.Text;

            if (tbSpellId.Text != string.Empty) cb.SpellId = int.Parse(tbSpellId.Text);
            cb.SpellName = cbSpellName.Text;

            if (cb.SpellName != string.Empty)
            {
                var lb = tabControl1.SelectedTab;
                var btype = (BehaviourType)Enum.Parse(typeof(BehaviourType), lb.Text);

                if (btype == BehaviourType.Healing)
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabHealing"];
                    cb.BehaviorType = BehaviourType.Healing;
                    EC.CR.THealingBehaviors.Add(cb);
                }
                if (btype == BehaviourType.Combat)
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabCombat"];
                    cb.BehaviorType = BehaviourType.Combat;
                    EC.CR.TCombatBehaviors.Add(cb);
                }
                if (btype == BehaviourType.Resting)
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabResting"];
                    cb.BehaviorType = BehaviourType.Resting;
                    EC.CR.TRestBehaviors.Add(cb);
                }
                if (btype == BehaviourType.Pulling)
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPulling"];
                    cb.BehaviorType = BehaviourType.Pulling;
                    EC.CR.TPullBehaviors.Add(cb);
                }
            }
            else
            {
                MessageBox.Show("Ensure spell name and Selected Routine is not empty.");
            }

            var sql = ORM.Insert(cb, "CombatBehaviors", "", true, DAL.getTableStructure("CombatBehaviors"));
            DataTable dtr = DAL.LoadSL3Data(sql);
            if (dtr != null)
            {
                if (dtr.Rows.Count > 0)
                {
                    var id = int.Parse(dtr.Rows[0][0].ToString());
                    cb.id = id;
                    //settings = _settings;
                }
            }
            UpdateUI();
            MessageBox.Show("Successfully added bahavior to routine.");

        }
        public void UpdateUI()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = EC.CR.THealingBehaviors;
            listBox1.DisplayMember = "Display";

            listBox2.DataSource = null;
            listBox2.DataSource = EC.CR.TCombatBehaviors;
            listBox2.DisplayMember = "Display";

            listBox3.DataSource = null;
            listBox3.DataSource = EC.CR.TRestBehaviors;
            listBox3.DisplayMember = "Display";

            listBox4.DataSource = null;
            listBox4.DataSource = EC.CR.TPullBehaviors;
            listBox4.DisplayMember = "Display";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //get active tab so we know which one the user wants to edit (we assume all of the listboxes in all of the tabs have an active component)
            var lb = tabControl1.SelectedTab;
            //get listbox from active tab
            ListBox c = (ListBox)lb.Controls[0].Controls[0];
            if (lb != null)
            {
                var i = (CombatBehavior)c.SelectedItem;
                if (i != null)
                {
                    if (i.AuraId != 0) tbAuraId.Text = i.AuraId.ToString();
                    if (i.AuraName != null) cbAuraName.Text = i.AuraName;
                    if (i.CastAtHealthPercentage) cbCastAtHealth.Checked = true;
                    if (i.CastRange != 0) tbRange.Text = i.CastRange.ToString();
                    cbAuraTarget.SelectedItem = i.Aura;
                    cbTargetType.SelectedItem = i.Target;
                    if (i.DontUseInGroups) DontUseInGroup.Checked = true;
                    if (i.HasNoPet) cbPetNotSummoned.Checked = true;
                    cbOperator.SelectedItem = i.HealthOperator;
                    if (i.HealthPercentage != 0) tbPercent.Text = i.HealthPercentage.ToString();
                    if (i.IsInterrupt) cbIsInterrupt.Checked = true;
                    if (i.PetIsDead) cbPetisDead.Checked = true;
                    if (i.RequireMeleeRange) cbMeleeRange.Checked = true;
                    if (i.RequirePet) cbRequirePet.Checked = true;
                    if (i.SpellId != 0) tbSpellId.Text = i.SpellId.ToString();
                    if (i.SpellName != null) cbSpellName.Text = i.SpellName;
                    if (i.TrinketId != 0) tbItemId.Text = i.TrinketId.ToString();
                    if (i.TrinketName != null) cbItemSpellName.Text = i.TrinketName;
                    if (i.UseInGroups) cbUseInGroup.Checked = true;
                    if (i.IsIncapacitated) cbIncapacitated.Checked = true;
                    if (i.UseTanksTarget) cbUseTanksTarget.Checked = true;
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<WoWAura> auras = new List<WoWAura>();
            var local = ObjectManager.GetObjectsOfType<WoWUnit>();
            foreach (WoWUnit unit in local)
            {
                auras.AddRange(unit.Auras.Values);
            }
            cbAuraName.DataSource = null;
            cbAuraName.DataSource = auras;
            cbAuraName.DataSource = "Name";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //get active tab so we know which one the user wants to edit (we assume all of the listboxes in all of the tabs have an active component)
            var lb = tabControl1.SelectedTab;
            //get listbox from active tab
            ListBox c = (ListBox)lb.Controls[0].Controls[0];
            CombatBehavior cb = (CombatBehavior)c.SelectedItem;
            if (cb.BehaviorType == BehaviourType.Healing) EC.CR.THealingBehaviors.Remove(cb);
            if (cb.BehaviorType == BehaviourType.Combat) EC.CR.TCombatBehaviors.Remove(cb);
            if (cb.BehaviorType == BehaviourType.Pulling) EC.CR.TPullBehaviors.Remove(cb);
            if (cb.BehaviorType == BehaviourType.Resting) EC.CR.TRestBehaviors.Remove(cb);
            DAL.ExecuteSL3Query(string.Format("delete from CombatBehaviors where CombatBehaviors.id = '{0}'", cb.id));
            UpdateUI();
        }

        private void cbAuraName_SelectedValueChanged_1(object sender, EventArgs e)
        {
            var item = (WoWSpell)cbSpellName.SelectedItem;
            if (item != null)
            {

                tbSpellId.Text = item.Id.ToString();
            }
        }
    }
}
