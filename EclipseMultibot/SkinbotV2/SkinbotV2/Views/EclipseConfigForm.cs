using Eclipse.Bots.MultiBot.Views;
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
namespace Eclipse.Bots.MultiBot
{
    public partial class EclipseConfigForm : Form
    {

        public EclipseConfigForm()
        {
            InitializeComponent();
        }

        private void EclipseConfigForm_Load(object sender, EventArgs e)
        {
            pbEclipse.ImageLocation = "http://hb.acsoft.us/image.aspx?image=SkinBot.jpg";
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
                EC.Log("!!!Setting BOT to Passive mode!!! (its not gonna do ANYTHING!)");
                EC.PassiveMode = true;
            }
            else EC.PassiveMode = false;
        }

        private void btnTravel_Click(object sender, EventArgs e)
        {
            TravelForm tf = new TravelForm();
            tf.Show();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                EC.Log("!!!Setting BOT to SkinBot mode!!!");
                EC.SkinMode = true;
            }
            else EC.SkinMode = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                EC.Log("!!!Setting BOT to 'KillThese' mode!!!");
                EC.KillThese = true;
            }
            else EC.KillThese = false;
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
