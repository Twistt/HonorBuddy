using CommonBehaviors.Actions;
using Eclipse;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.Routines;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwistedCombat3.TwistedCombat.Views;

namespace TwistedCombatRoutines
{
    public class TwistedCombatRoutine : CombatRoutine
    {
        public int id { get; set; }
        public int RoutineId { get; set; }
        public override string Name { get { return "All in One Combat - Twisted Combat Routines"; } }
        public override WoWClass Class { get { 
            return StyxWoW.Me.Class; 
        } }
        public override bool WantButton { get { return true; } }
        public override void OnButtonPress()
        {
            var settingsForm = new LoadAndSave();
            settingsForm.Show();
        }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override Composite CombatBehavior { get { return new ActionRunCoroutine(ctx => CombatRotationBehavior()); } }
        public override Composite PreCombatBuffBehavior{ get{ return new ActionRunCoroutine(ctx => PreCombatBuffBehaviors()); } }
        public override Composite CombatBuffBehavior{ get { return new ActionRunCoroutine(ctx => CombatBuffBehaviors()); } }
        public override Composite HealBehavior { get { return new ActionRunCoroutine(ctx => HealingBehaviors()); } }



        public override Composite PullBehavior { get { return new ActionRunCoroutine(ctx => PullBehaviors()); } }

        public static async Task<bool> CombatRotationBehavior()
        {
            foreach (CombatBehavior cb in EC.CR.TCombatBehaviors)
            {
                CastInterrupt();
                if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                {
                    await DoSpellLogic(cb);
                }
            }
            foreach (CombatBehavior cb in EC.CR.THealingBehaviors)
            {
                CastInterrupt();
                if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                {
                    await DoSpellLogic(cb);
                }
            }

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        private static void CastInterrupt()
        {
            if (Me.CurrentTarget != null && Me.CurrentTarget.IsCastingHealingSpell)
            {
                EC.CR.TCombatBehaviors.Where(c => c.IsInterrupt && SpellManager.CanCast(c.SpellId)).ToList().ForEach(c =>
                {
                    if (Me.IsCasting) SpellManager.StopCasting();
                    SpellManager.Cast(c.SpellId);
                });
            }
        }
        public static async Task<bool> HealingBehaviors()
        {
            foreach (CombatBehavior cb in EC.CR.THealingBehaviors)
            {
                if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                {
                    await DoSpellLogic(cb);
                }
            }
            return false;
        }
        public static async Task<bool> PreCombatBuffBehaviors()
        {
            foreach (CombatBehavior cb in EC.CR.TCombatBehaviors)
            {
                if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                {
                    await DoSpellLogic(cb);
                }
            }
            return false;
        }
        public static async Task<bool> CombatBuffBehaviors()
        {
            foreach (CombatBehavior cb in EC.CR.TCombatBehaviors)
            {
                if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                {
                    await DoSpellLogic(cb);
                }
            }
            return false;
        }
        public static async Task<bool> PullBehaviors()
        {
            foreach (CombatBehavior cb in EC.CR.TPullBehaviors)
            {
                if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                {
                    await DoSpellLogic(cb);
                }
            }
            return false;
        }
        public static async Task<bool> DoSpellLogic(CombatBehavior cb)
        {
            EC.Log("Ok we actually have this spell lets do it!");
            WoWUnit target = null;
            //Target the right person
            if (cb.IsAura)
            {
                EC.Log("Targeting player for aura");
                if (cb.Aura == AuraTarget.KeepAuraOnHealer) target = GetHealer().ToPlayer();
                if (cb.Aura == AuraTarget.KeepAuraOnMe) target = Me;
                if (cb.Aura == AuraTarget.KeepAuraOnPartyMembers) target = GetMemberWithoutAura(cb.AuraName);
                if (cb.Aura == AuraTarget.KeepAuraOnTank) target = GetTank().ToPlayer();
                if (cb.Aura == AuraTarget.ApplyAuraToAttackingMobs) target = TargetMobWithoutAura(cb.AuraName);
            }
            if (cb.HasTargetingRules)
            {
                EC.Log("Targeting player for spell");
                if (cb.Target == TargetType.Healer) target = GetHealer().ToPlayer();
                if (cb.Target == TargetType.Tank) target = GetTank().ToPlayer();
                if (cb.Target == TargetType.LowestHealthPartyMember) target = GetLowestHealthPartyMember();
                if (cb.Target == TargetType.Add) target = GetAdd();
                if (cb.Target == TargetType.Mob) target = Me.CurrentTarget;
                if (Me.Pet != null)
                {
                    if (cb.TargetPet) target = Me.Pet;
                }

            }

            if (target != null)
            {
                if (cb.RequireMeleeRange && !target.IsWithinMeleeRange) Flightor.MoveTo(target.Location);
                if (cb.CastRange > target.Distance) Flightor.MoveTo(target.Location);
                if (target != Me.CurrentTarget) target.Target();
                bool _Cast = false;
                if (cb.CastAtHealthPercentage && cb.HealthOperator == Operator.LT && target.HealthPercent < cb.HealthPercentage) _Cast = true;
                if (cb.CastAtHealthPercentage && cb.HealthOperator == Operator.GT && target.HealthPercent > cb.HealthPercentage) _Cast = true;
                if (cb.CastAtHealthPercentage && cb.HealthOperator == Operator.EQ && target.HealthPercent == cb.HealthPercentage) _Cast = true;
                if (cb.IsAura)
                {
                    if (target.Auras.ContainsKey(cb.AuraName)) _Cast = false;
                    else
                    {
                        EC.Log(string.Format("{0} Does not have {1}, applying...", target.SafeName, cb.AuraName));
                        _Cast = true;
                    }
                }
                if (cb.RequirePet && Me.Pet == null) _Cast = false;
                if (cb.PetIsDead && Me.Pet.IsAlive) _Cast = false;
                if (cb.HasNoPet && Me.Pet != null) _Cast = false;
                if (cb.UseInGroups && !Me.GroupInfo.IsInParty) _Cast = false;
                if (cb.DontUseInGroups && Me.GroupInfo.IsInParty) _Cast = false;
                if (cb.IsIncapacitated && !IsIncap(target)) _Cast = false;

                if (cb.IsSpell && _Cast)
                {
                    EC.Log(string.Format("Casting {1} on {0}", target.SafeName, cb.SpellName));
                    SpellManager.CastSpellById(cb.SpellId);
                }
                if (cb.IsItem && _Cast)
                {
                    EC.Log(string.Format("Using item on {0}", target.SafeName));
                    var item = Me.Inventory.Backpack.Items.Where(i => i.Entry == cb.TrinketId).FirstOrDefault();
                    if (item != null) item.Use();
                }
            }
            else
            {
                EC.Log("No Target.");
            }
            return true;
        }

        private static bool IsIncap(WoWUnit target)
        {
            if (target.Auras.Values.Where(a =>
                a.Spell.Mechanic == WoWSpellMechanic.Asleep ||
                a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                a.Spell.Mechanic == WoWSpellMechanic.Fleeing ||
                a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                a.Spell.Mechanic == WoWSpellMechanic.Invulnerable2 ||
                a.Spell.Mechanic == WoWSpellMechanic.Incapacitated ||
                a.Spell.Mechanic == WoWSpellMechanic.Polymorphed ||
                a.Spell.Mechanic == WoWSpellMechanic.Rooted ||
                a.Spell.Mechanic == WoWSpellMechanic.Sapped ||
                a.Spell.Mechanic == WoWSpellMechanic.Shackled ||
                a.Spell.Mechanic == WoWSpellMechanic.Stunned ||
                a.Spell.Mechanic == WoWSpellMechanic.Turned
                ).FirstOrDefault() != null ) return true;
            return false;
        }

        #region Lua Event Handlers
        public static void HandleDungeonInvite(object sender, LuaEventArgs args)
        {
            Lua.DoString("AcceptProposal()");
        }

        public static void HandleResurrect(object sender, LuaEventArgs args)
        {
            float waitseconds = Lua.GetReturnVal<float>("return GetCorpseRecoveryDelay()", 0);
            Lua.DoString("AcceptResurrect()");
        }
        #endregion

        #region HelperFunctions
        public static bool PetInCombat()
        {

            if (StyxWoW.Me.GotAlivePet &&
                StyxWoW.Me.PetInCombat &&
                StyxWoW.Me.Pet.CurrentTarget.Combat)
                return true;
            else
                return false;
        }

        public static bool MeOrPartyMemberInCombat()
        {
            // PetInCombat always returns true now
            if (StyxWoW.Me.IsActuallyInCombat || PetInCombat())
            {
                //log("Adds count: {0}, me in combat: {1}, pet in combat: {2}", mbc.adds.Count, StyxWoW.Me.IsActuallyInCombat, StyxWoW.Me.PetInCombat);
                return true;
            }
            else
                return false;
        }
        private static WoWUnit TargetMobWithoutAura(string auraname)
        {
            foreach (WoWUnit mob in ObjectManager.GetObjectsOfType<WoWUnit>().Where(u=>u.IsTargetingMeOrPet))
            {
                if (!mob.ActiveAuras.ContainsKey(auraname))
                {
                    return mob;
                }
            }
            return null;
        }
        public static WoWPlayer GetMemberWithoutAura(string auraname)
        {
            foreach (WoWPlayer player in Me.PartyMembers)
            {
                if (!player.ActiveAuras.ContainsKey(auraname))
                {
                    return player;
                }
            }
            return null;
        }
        public static WoWPartyMember GetTank()
        {
            WoWPartyMember tank = Me.GroupInfo.PartyMembers.Where(p => p.Role == WoWPartyMember.GroupRole.Tank).ToList().FirstOrDefault();
            if (tank != null)
            {
                return tank;
            }
            return null;
        }
        public static WoWPartyMember GetHealer()
        {
            WoWPartyMember healer = Me.GroupInfo.PartyMembers.Where(p => p.Role == WoWPartyMember.GroupRole.Healer).ToList().FirstOrDefault();
            if (healer != null)
            {
                return healer;
            }
            return null;
        }
        public static WoWPlayer GetLowestHealthPartyMember()
        {
            return Me.PartyMembers.Where(p=>p.HealthPercent != 100).OrderByDescending(p => p.HealthPercent).FirstOrDefault();
        }

        private static WoWUnit GetAdd()
        {
            return (WoWUnit)ObjectManager.ObjectList.Where(o => o.Type == WoWObjectType.Unit).Where(o => ((WoWUnit)o).IsTargetingMyPartyMember).OrderBy(o => ((WoWUnit)o).HealthPercent).FirstOrDefault();
        }
        #endregion
    }
}
