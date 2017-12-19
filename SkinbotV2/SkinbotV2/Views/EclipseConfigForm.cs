using Eclipse.Bots.SkinBot.Views;
using Eclipse.WoWDatabase;
using SkinbotV2.Views;
using Styx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eclipse.Bots.SkinBot
{
    public partial class EclipseConfigForm : Form
    {

        public EclipseConfigForm()
        {
            InitializeComponent();
            checkBox2.Checked = Core.SkinMode;
            checkBox1.Checked = Core.PassiveMode;
        }

        private void EclipseConfigForm_Load(object sender, EventArgs e)
        {
            pbEclipse.ImageLocation = "http://www.arachnidcreations.com/HonorBuddy/Skinbot.jpg";
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
            }
            else Core.PassiveMode = false;
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
                EC.log("!!!Setting BOT to SkinBot mode!!!");
                Core.SkinMode = true;
            }
            else Core.SkinMode = false;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            MobSelectionList mbl = new MobSelectionList();
            mbl.Show();
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
            new SkinningManagement().Show();
        }
    }
}
