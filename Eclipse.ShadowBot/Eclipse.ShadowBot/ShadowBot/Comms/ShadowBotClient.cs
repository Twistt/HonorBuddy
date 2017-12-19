using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Eclipse.Comms
{
    public class ShadowBotClient
    {
        public int PortNumber { get; set; }
        public IPAddress IPAddress { get; set; }
        public List<WowMessage> Messages = new List<WowMessage>();
        public WowCharacter Character {get;set;}
        public string Display {
            get {
                WowCharacter c = null;
                if (Character == null) c = new WowCharacter() { Name = "TBD" };
                else c = Character;
                return string.Format("{0}=> ({1},{2})",c.Name, PortNumber, IPAddress.ToString());
            }
        }
    }
}
