using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.WoWDatabase.Models
{
    public class Location
    {
        public uint Entry { get; set; }
        public uint Zone { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string Name { get; set; }

        internal Vector3 V3()
        {
            return new Vector3(X, Y, Z);
        }

        internal float Distance()
        {
            throw new NotImplementedException();
        }
    }
}
