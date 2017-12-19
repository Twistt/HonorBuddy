using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatheringLegion.Models
{
    public class Zone
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        public uint ParentZone { get; set; }
        public bool GatherHere { get; set; }
    }
}
