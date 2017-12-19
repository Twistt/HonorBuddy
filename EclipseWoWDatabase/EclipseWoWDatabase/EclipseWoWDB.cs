using System;
using System.Text;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Common.Helpers;
using Styx.CommonBot.Inventory;
using Styx.Helpers;
using Eclipse.WoWDatabase;

namespace Eclipse
{
    class EclipseProfileBuilder : HBPlugin
    {
        public override string Name { get { return "Eclipse WoWDB Plugin"; } }
        public override string Author { get { return "Twist"; } }
        public override Version Version { get { return new Version(1, 0, 0, 1); } }

        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Manage Data"; } }

        private static LocalPlayer intMe = StyxWoW.Me;
        private EclipseDBManager mainForm;


        public EclipseProfileBuilder()
        {
            mainForm = new EclipseDBManager();
            Logging.Write("Loaded " + Name + " v" + Version.ToString() + " by " + Author);
        }

        public override void Pulse()
        {
            if (!StyxWoW.IsInGame)
            {
                return;
            }

            Core.Pulse();
        }
        public override void OnEnable()
        {
            Core.Initialize();
            base.OnEnable();
            
        }
        
        public override void OnButtonPress()
        {
            if (mainForm.IsDisposed || mainForm == null) mainForm = new EclipseDBManager();
            else mainForm.Show();
        }


        public override void OnDisable()
        {
            base.OnDisable();
        }

        
    }
}
