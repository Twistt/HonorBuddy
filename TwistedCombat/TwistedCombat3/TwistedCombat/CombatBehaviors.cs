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
        public int id { get; set; }
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

        public double HealthPercentage { get; set; }

        public bool RequireMeleeRange { get; set; }
        public double CastRange { get; set; }

        public bool SpellIsTrinket { get; set; }
        public int TrinketId { get; set; }
        public string TrinketName { get; set; }

        public int SpellId { get; set; }
        public string SpellName { get; set; }

        //these are set up as ints to reflect the enums to serialize
        public int target { get; set; }
        public int aura { get; set; }
        public int healthOperator { get; set; }

        public Operator HealthOperator
        {
            get { return (Operator)healthOperator; }
            set
            {
                healthOperator = Convert.ToInt32(value);
            }
        }

        public TargetType Target {
            get { return (TargetType)target; }
            set {
                target = Convert.ToInt32(value);
            }
        }
        public AuraTarget Aura
        {
            get { return (AuraTarget)aura; }
            set
            {
                aura = Convert.ToInt32(value);
            }
        }
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
        public bool UseTanksTarget { get; set; }
        public DateTime LastCastTime = DateTime.Now;

    }
    public enum AuraTarget
    {
        None=0,
        KeepAuraOnMe,
        KeepAuraOnPartyMembers,
        KeepAuraOnHealer,
        KeepAuraOnTank,
        DontCastIfTargetHasAuara,
        ApplyAuraToAttackingMobs
    }
    public enum BehaviourType { 
        Healing=0,
        Pulling,
        Combat, 
        Buffing,
        Resting
    }
    public enum Operator
    {
        EQ=0,
        LT,
        GT
    }
    public enum TargetType
    {
        None=0,
        LowestHealthPartyMember,
        Tank,
        Me,
        Mob,
        Healer,
        Pet, 
        Add
    }
}
