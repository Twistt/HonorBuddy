using Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwistedCombatRoutines.Data
{
    public class EclipseCombatRoutine
    {
        public int Id { get; set; }
        public string RoutineName { get; set; }
        public String WowClass { get; set; }
        public String WowSpec { get; set; }
        public bool IsPvP { get; set; }
        public bool IsPvE { get; set; }
        public List<CombatBehavior> TCombatBehaviors = new List<CombatBehavior>();
        public List<CombatBehavior> THealingBehaviors = new List<CombatBehavior>();
        public List<CombatBehavior> TPullBehaviors = new List<CombatBehavior>();
        public List<CombatBehavior> TRestBehaviors = new List<CombatBehavior>();

    }
}
