﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.Models
{
    public class Unit
    {
        public string Name { get; set; }
        public uint Zone { get; set; }
        public uint Entry { get; set; }
        public uint FactionId { get; set; }
        public int Level { get; set; }

    }
}
