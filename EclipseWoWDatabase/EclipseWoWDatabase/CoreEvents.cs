using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.WoWDatabase
{
    //First we have to define a delegate that acts as a signature for the
    //function that is ultimately called when the event is triggered.
    //You will notice that the second parameter is of MyEventArgs type.
    //This object will contain information about the triggered event.
    public delegate void TargetChangedHandler(object source, EclipseEventArgs e);

    //This is a class which describes the event to the class that recieves it.
    //An EventArgs class must always derive from System.EventArgs.
    public class EclipseEventArgs : EventArgs
    {
        private WoWUnit Target;
        public EclipseEventArgs(WoWUnit target)
        {
            Target = target;
        }
        public WoWUnit GetTarget()
        {
            return Target;
        }
    }
}
