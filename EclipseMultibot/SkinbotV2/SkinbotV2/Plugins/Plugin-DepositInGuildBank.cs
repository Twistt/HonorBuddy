using Eclipse.MultiBot.Plugins;
using Styx;
using Styx.Common;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.MultiBot
{
    public class Plugin_DepositInGuildBank: HBPlugin
    {
        //This is the name of the plugin that will be in the dropdown on the HB plugins list
        public override string Name { get { return "Donate This Crap"; } }
        //This is the name of hte author of the plugin that will also appear in the HB info for the pluin
        public override string Author { get { return "Twist"; } }
        //This is a controlled version that the HB store uses (and helps you too) to make sure that only the right version that works with a specific HB version is used.
        public override Version Version { get { return new Version(1, 0, 0, 0); } }
        //This tells HB that you want control of the config button to show your OWN config form (defined below).
        public override bool WantButton { get { return true; } }
        // This text will appear ON The button itself (the button will change sizes to accomadate all the text).
        public override string ButtonText { get { return "Configure Donations"; } }
        // Styx.Me is all the Local player information. So using just "Me" is a shorthand form of "StyxWoW.Me" so that oyu dont ahve to put the whole thing in every time you can just write "Me". 
        //      Since Me is a STATIC variable its tied directly to the HB memory reader and will always be up to date- 
        //      since we have made a STATIC reference here "Me" is always the same as the HB version of hte variable and therefore will always be up to date.
        private static LocalPlayer Me = StyxWoW.Me;
        //This is an instance reference to your config form - it starts out as nothing (null) so if you try to use it before you have assigned it it will error (object null reference exception)
        private DonationConfiguration mainForm;

        //This is the EXACT same name as the class - therefore making it the Constructor. When HB loads the plugins to make the plugin list this is ALWAYS run (so you shouldnt do alot of stuff in here).
        public Plugin_DepositInGuildBank()
        {
            // this sets the value of the above config form variable to an actual instance of the form (hence the new).
            mainForm = new DonationConfiguration();
            Logging.Write("Loaded " + Name + " v" + Version.ToString() + " by " + Author);
        }

        public override void Pulse()
        {
            if (!StyxWoW.IsInGame)
            {
                return;
            }

            /* PULSE ACTION HERE */
           //if (mainForm != null) mainForm.Pulse();
        }
        
        
        public override void OnButtonPress()
        {
            //if (mainForm == null || mainForm.IsDisposed)                 mainForm = new ProfileTypeSelector();
            try
            {
                mainForm.Show();
                mainForm.Activate();
            }
            catch (ArgumentOutOfRangeException ee)
            {

            }
        }
    }
}
