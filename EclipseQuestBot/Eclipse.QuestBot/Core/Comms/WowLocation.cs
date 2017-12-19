using Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace Eclipse.Comms
{
    public class WowLocation
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vector3 ToWowPoint{
            get {
                return new Vector3(X, Y, Z);
            }
        }
    }
}
