using Styx;
using Styx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.Comms
{
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public WoWPoint Location { get; set; }
        public int ZoneId { get; set; }
        public int Port { get; set; }
        public string IP { get; set; }

    }
}
