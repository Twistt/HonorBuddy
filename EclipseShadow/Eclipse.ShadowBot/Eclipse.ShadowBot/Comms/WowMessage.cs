using Styx;
using Styx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.Comms
{
    public class WowMessage
    {
        public WowMessage() {
            timestamp = DateTime.Now;
            X = StyxWoW.Me.X;
            Y = StyxWoW.Me.Y;
            Z = StyxWoW.Me.Z;
            ZoneId = (int)StyxWoW.Me.ZoneId;
        }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int ZoneId { get; set; }
        public string data { get; set; }
        public DateTime timestamp { get; set; }
        public int Port { get; set; }
    }
}
