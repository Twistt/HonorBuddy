using Styx.TreeSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.MultiBot.Behaviors
{
    public class TestClass
    {
        public Composite testBT
        {
            get
            {
                return new PrioritySelector();
            }
        }
    }
}
