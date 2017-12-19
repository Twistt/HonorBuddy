using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.Bots.QuestBot;
using Eclipse.Models;
using Styx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Eclipse.MultiBot.Views
{
    public partial class QuestingMode : Form
    {
        EclipseProfile dt = new EclipseProfile();
        public QuestingMode()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //lbNPCsWithQuests.DataSource = Eclipse.Multibot.Main.GetNearbyNPCSWithQuests();
            //lbNPCsWithQuests.DisplayMember = "name";

        }
        private void btnImportProfile_Click(object sender, EventArgs e)
        {
            //try
            {
                //ToDo: Renable before publish!
                //OpenFileDialog ofd = new OpenFileDialog();
                //ofd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                //if (tbFileName.Text.Length == 0)
                //{
                //    ofd.ShowDialog();
                //    tbFileName.Text = ofd.FileName;
                //}
                var dirs = Directory.GetFiles(@"C:\Users\Twist\Documents\Honorbuddy\Default Profiles\Cava\Scripts\", "*.xml");
                foreach (var dir in dirs)
                {
                    dt = new EclipseProfile(dir);
                    EC.Log("Loaded Profile Data.");
                    dt.Save();
                }
            }
        }

        private void QuestingMode_Load(object sender, EventArgs e)
        {
            listBox3.DataSource = EC.ActiveQuestQuestOrders;
            listBox3.DisplayMember = "QuestName";
            if (StyxWoW.Me != null) listBox1.DataSource = StyxWoW.Me.QuestLog.GetAllQuests().ToList();
            listBox1.DisplayMember = "name";

            listBox4.DataSource = EC.QuestOrders;
            listBox4.DisplayMember = "QuestName";
            var quests = StyxWoW.Me.QuestLog.GetAllQuests();
            List<QuestOrder> objectives = new List<QuestOrder>();
            foreach (var q in quests)
            {
                objectives.AddRange(EC.QuestOrders.Where(qu => qu.QuestId == q.Id));

            }
            listBox2.DataSource = objectives;
            listBox2.DisplayMember = "DisplayName";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dt.Save();
        }

        private void listBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            QuestOrder QO = (QuestOrder)listBox3.SelectedItem;
            if (QO != null)
            {
                propertyGrid1.SelectedObject = QO;
                tbLog.AppendText(String.Format("Showing Quest: {0}, {1} \r\n", QO.QuestName, QO.QuestId));
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Quests;"));

            foreach (DataRow row in dt.Rows)
            {
                var quest = (Quest)ORM.convertDataRowtoObject(new Quest(), row, "");
                EC.Quests.Add(quest);

            }
            foreach (var q in Styx.StyxWoW.Me.QuestLog.GetAllQuests().ToList())
            {
                Quest quest = new Quest { Description = q.Description, Id = q.Id, IsDaily = q.IsDaily, IsShareable = q.IsShareable, IsWeekly = q.IsWeekly, Level = q.Level, Name = q.Name, ObjectiveText = q.ObjectiveText, RequiredLevel = q.RequiredLevel };

                var result2 = DAL.LoadSL3Data(string.Format("select * from Quests where Id = '{0}'", q.Id));
                if (result2 == null)
                {
                    DAL.ExecuteSL3Query(DAL.Insert(quest, "Quests", "", false, DAL.getTableStructure("Quests")));
                    EC.Log("Added Quest to DataBase.");
                }
                else
                {
                    if (result2.Rows.Count == 0)
                    {
                        DAL.ExecuteSL3Query(DAL.Insert(quest, "Quests", "", false, DAL.getTableStructure("Quests")));
                        EC.Log("Added Quest to DataBase.");
                    }
                }
                var result = DAL.LoadSL3Data(string.Format("select * from questorders where questid = '{0}'", q.Id));
                if (result != null)
                {
                    if (result.Rows.Count > 0)
                    {
                        var qos = ORM.convertDataTabletoObject<QuestOrder>(result);
                        quest.QuestOrders.AddRange(qos);
                        EC.Log(string.Format("Loaded quest orders for {0}", q.Id));
                    }
                    else
                    {
                        EC.Log(q.ObjectiveText);
                    }
                }

                EC.CurrentQuests.Add(quest);
            }
            listBox3.DataSource = EC.CurrentQuests;
            listBox3.DisplayMember = "name";
            if (StyxWoW.Me != null) listBox1.DataSource = StyxWoW.Me.QuestLog.GetAllQuests().ToList();
            listBox1.DisplayMember = "name";

        }

        private void listBox4_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = EC.ActiveQuestOrder;
            lblQuestStatus.Text = EC.ActiveQuestOrder.status.ToString();
            if (EC.Target != null) lblTarget.Text = EC.Target.Name;
            if (EC.qoLoc != null) lblLocation.Text = EC.qoLoc.ToString();
            //propertyGrid2.SelectedObject = EC.Me.QuestLog.GetQuest(0);

            var objective = EC.ActiveQuest.GetObjectives()[0]; // or whatever to find the correct objective /
            Styx.WoWInternals.WoWDescriptorQuest dynamicData;
            if (EC.ActiveQuest.GetData(out dynamicData))
            {
                uint objectivesCompleted = dynamicData.ObjectivesDone[objective.Index];
            }
            propertyGrid2.SelectedObject = dynamicData;
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            QuestOrder QO = (QuestOrder)listBox2.SelectedItem;
            if (QO != null)
            {
                propertyGrid1.SelectedObject = QO;
                tbLog.AppendText(String.Format("Showing Quest: {0}, {1} \r\n", QO.QuestName, QO.QuestId));
            }
        }


    }
}

