using Eclipse.Bots.SkinBot.Views;
using Eclipse.WoWDatabase;
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
        }

        private void EclipseConfigForm_Load(object sender, EventArgs e)
        {

        }

        private void btnData_Click(object sender, EventArgs e)
        {
            EclipseDataManager edb = new EclipseDataManager();
            edb.Show();
        }
    }
}
