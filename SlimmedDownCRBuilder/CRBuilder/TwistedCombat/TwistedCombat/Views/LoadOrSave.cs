using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse;
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
using TwistedCombatRoutines.Data;

namespace TwistedCombatRoutines
{
    public partial class LoadOrSave : Form
    {
        List<EclipseCombatRoutine> ecrs = new List<EclipseCombatRoutine>();
        public LoadOrSave()
        {
            InitializeComponent();
        }

        private void btnCreateNEW_Click(object sender, EventArgs e)
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("Select * from EclipseCombatSettings where RoutineName = '{0}'", tbRoutineName.Text.Replace("'", "''")));
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("You already have a routine named " + tbRoutineName.Text);
                    return;
                }
                else
                {
                    WoWClass cclass;
                    Enum.TryParse<WoWClass>(cbClasses.Text, out cclass);

                    WoWSpec spec;
                    Enum.TryParse<WoWSpec>(cbSpecs.Text, out spec);
                    var _settings = new EclipseCombatRoutine() { WowClass = cclass.ToString(), RoutineName = tbRoutineName.Text.Replace("'", "''"), IsPvE = cbPVERoutine.Checked, IsPvP = cbPVPRoutine.Checked, WowSpec = spec.ToString() };
                    var sql = ORM.Insert(_settings, "EclipseCombatSettings", "", true, DAL.getTableStructure("EclipseCombatSettings"));
                    DataTable dtr = DAL.LoadSL3Data(sql);
                    if (dtr != null)
                    {
                        if (dtr.Rows.Count > 0)
                        {
                            var id = int.Parse(dtr.Rows[0][0].ToString());
                            _settings.Id = id;
                            EC.CR = _settings;
                        }
                    }
                }

            }
            LbRoutines.DataSource = null;
            LbRoutines.DataSource = ecrs;
            LbRoutines.DisplayMember = "RoutineName";
            new TwistedCRGui(EC.CR).Show();
        }

        private void tbAddToRoutine_Click(object sender, EventArgs e)
        {
            EclipseCombatRoutine ec = (EclipseCombatRoutine)LbRoutines.SelectedItem;
            new TwistedCRGui(ec).Show();
        }

        private void LoadOrSave_Load(object sender, EventArgs e)
        {
            Eclipse.EC.FindDB();
            DAL.CreateSL3Connection("EclipseRotations.edb");
            DataTable dt = DAL.LoadSL3Data("Select * from EclipseCombatSettings");
            foreach (DataRow row in dt.Rows)
            {
                EclipseCombatRoutine ecr = (EclipseCombatRoutine)ORM.convertDataRowtoObject(new EclipseCombatRoutine(), row);
                ecrs.Add(ecr);
            }

            DataTable dtr = DAL.LoadSL3Data("Select * from CombatBehaviors");
            foreach (DataRow row in dtr.Rows)
            {
                CombatBehavior cb = (CombatBehavior)ORM.convertDataRowtoObject(new CombatBehavior(), row);
                if (cb.RoutineId != 0 && cb.RoutineId != null){
                    if (cb.BehaviorType == BehaviourType.Healing) ecrs.Where(r=>r.Id == cb.RoutineId).FirstOrDefault().THealingBehaviors.Add(cb);
                    if (cb.BehaviorType == BehaviourType.Combat) ecrs.Where(r => r.Id == cb.RoutineId).FirstOrDefault().TCombatBehaviors.Add(cb);
                    if (cb.BehaviorType == BehaviourType.Pulling) ecrs.Where(r => r.Id == cb.RoutineId).FirstOrDefault().TPullBehaviors.Add(cb);
                    if (cb.BehaviorType == BehaviourType.Resting) ecrs.Where(r => r.Id == cb.RoutineId).FirstOrDefault().TRestBehaviors.Add(cb);
                }
            }
            LbRoutines.DataSource = ecrs;
            LbRoutines.DisplayMember = "RoutineName";

            cbClasses.DataSource = Enum.GetValues(typeof(WoWClass));
            cbSpecs.DataSource = Enum.GetValues(typeof(WoWSpec)); 
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
