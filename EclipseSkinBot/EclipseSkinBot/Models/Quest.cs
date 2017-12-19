using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.WoWDatabase.Models
{
    public class Quest
    {
        public uint Id { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public bool IsDaily { get; set; }
        public bool IsWeekly { get; set; }
        public bool IsShareable { get; set; }
        public string Description { get; set; }
        public int RequiredLevel { get; set; }
        public string ObjectiveText { get; set; }

    }
}
