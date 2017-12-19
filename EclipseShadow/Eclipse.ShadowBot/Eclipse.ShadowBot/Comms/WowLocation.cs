using Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.Comms
{
    public class WowLocation
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public WoWPoint ToWowPoint{
            get {
                return new WoWPoint(X, Y, Z);
            }
        }
    }
}
