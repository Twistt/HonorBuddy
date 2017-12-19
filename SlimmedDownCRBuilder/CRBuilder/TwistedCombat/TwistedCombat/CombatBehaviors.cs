using Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwistedCombatRoutines
{
    public class CombatBehavior
    {
        public int Id { get; set; }
        public int RoutineId { get; set; }
        public BehaviourType BehaviorType { get; set; }

        public bool IsAura { get; set; }
        public string Display { 
            get {
                if (SpellIsTrinket) return string.Format("Use item {0} on {1}", TrinketId, Target.ToString());
                else return string.Format("Cast Spell {0} on {1}", SpellName, Target.ToString());
            } 
        }

        public string AuraName { get; set; }
        public int AuraId { get; set; }

        public bool CastAtHealthPercentage { get; set; }
        public Operator HealthOperator { get; set; }
        public double HealthPercentage { get; set; }

        public bool RequireMeleeRange { get; set; }
        public double CastRange { get; set; }

        public bool SpellIsTrinket { get; set; }
        public int TrinketId { get; set; }
        public string TrinketName { get; set; }

        public int SpellId { get; set; }
        public string SpellName { get; set; }

        public TargetType Target { get; set; }
        public AuraTarget Aura { get; set; }

        public bool DontCastIfTargetHasAura { get; set; }
        public bool HasTargetingRules { get; set; }
        public bool IsItem { get; set; }
        public bool IsSpell { get; set; }
        public bool TargetMob { get; set; }
        public bool TargetPet { get; set; }

        public bool RequirePet { get; set; }
        public bool UseInGroups { get; set; }
        public bool DontUseInGroups { get; set; }
        public bool IsInterrupt { get; set; }
        public bool PetIsDead { get; set; }
        public bool HasNoPet { get; set; }
        public bool IsIncapacitated { get; set; }
    }
    public enum AuraTarget
    {
        None,
        KeepAuraOnMe,
        KeepAuraOnPartyMembers,
        KeepAuraOnHealer,
        KeepAuraOnTank,
        DontCastIfTargetHasAuara,
        ApplyAuraToAttackingMobs
    }
    public enum BehaviourType { 
        Healing,
        Pulling,
        Combat, 
        Buffing,
        Resting
    }
    public enum Operator
    {
        EQ,
        LT,
        GT
    }
    public enum TargetType
    {
        None,
        LowestHealthPartyMember,
        Tank,
        Me,
        Mob,
        Healer,
        Pet, 
        Add
    }
}
