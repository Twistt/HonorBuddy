using Eclipse.Bots.QuestBot.Views;
using Eclipse.MultiBot.Views;
using Styx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eclipse.MultiBot;
using ArachnidCreations.DevTools;
using Eclipse.Models;
using ArachnidCreations;
namespace Eclipse.Bots.QuestBot
{
    public partial class EclipseConfigForm : Form
    {

        public EclipseConfigForm()
        {
            InitializeComponent();
        }

        private void EclipseConfigForm_Load(object sender, EventArgs e)
        {
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            EclipseDataManager edb = new EclipseDataManager();
            edb.Show();
        }

        private void btnTravel_Click(object sender, EventArgs e)
        {
            TravelForm tf = new TravelForm();
            tf.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MobSelectionList qm = new MobSelectionList();
            qm.Show();
        }

        private void chQuestMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chQuestMode.Checked)
            {
                EC.Log("!!!Setting BOT to 'Questing' mode!!!");
                EC.QuestingMode = true;
                UIHooks.AttachQuestEvents();
            }
            else {
                EC.QuestingMode = false;
                UIHooks.DetatchQuestEvents();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MobSelectionList qm = new MobSelectionList();
            qm.SkinMode = true;
            qm.Show();
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            EC.BagsFull = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            QuestingMode qm = new QuestingMode();
            qm.Show();
        }

        private void btnQuestManager_Click(object sender, EventArgs e)
        {
            QuestingBuddy qb = new QuestingBuddy();
            qb.Show();
        }
    }
}
