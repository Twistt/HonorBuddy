using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.WoWDatabase
{
    public class TargetChangedEvents : EventArgs
    {
        public WoWUnit Target { get; private set; }

        public TargetChangedEvents(WoWUnit Target)
        {
            Core.Target = Target;
        }
    }
}
