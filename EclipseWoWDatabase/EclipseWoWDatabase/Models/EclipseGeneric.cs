using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class EclipseGeneric
    {
        private uint _id = 0;
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string Name { get; set; } //not actually used by honorbuddy - just makes it easier to keep track of
        public uint Entry {
            get { return _id; }
            set { _id = value; }
        }
        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public  ObjectType OType {get;set;}
        public enum ObjectType { 
            Mob,
            Object, 
            MailBox,
            Item
        }

    }
}
