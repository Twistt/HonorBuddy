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
using JSONSharp;
namespace TwistedCombatRoutines
{
    public class TwistedCombatRoutine : CombatRoutine
    {
        public TwistedCombatRoutine()
        {
            EC.Log("Constructor Fired");
        }
        public int id { get; set; }
        public int RoutineId { get; set; }
        public override string Name { get { return "All in One Combat - Twisted Combat Routines"; } }
        public override WoWClass Class
        {
            get
            {
                return StyxWoW.Me.Class;
            }
        }
        public override CapabilityFlags SupportedCapabilities
        {
            get
            {
                return CapabilityFlags.All;
            }
        }
        public override bool WantButton { get { return true; } }
        public override void OnButtonPress()
        {
            var settingsForm = new LoadAndSave();
            settingsForm.Show();
        }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override Composite CombatBehavior { get { return new ActionRunCoroutine(ctx => CombatRotationBehavior()); } }
        public override Composite PreCombatBuffBehavior { get { return new ActionRunCoroutine(ctx => PreCombatBuffBehaviors()); } }
        public override Composite CombatBuffBehavior { get { return new ActionRunCoroutine(ctx => CombatBuffBehaviors()); } }
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
            if (EC.CR != null)
            {
                foreach (CombatBehavior cb in EC.CR.THealingBehaviors)
                {
                    if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                    {
                        await DoSpellLogic(cb);
                    }
                }
                return true;
            }
            return false;
        }
        public static async Task<bool> PreCombatBuffBehaviors()
        {
            if (EC.CR != null)
            {
                foreach (CombatBehavior cb in EC.CR.TCombatBehaviors)
                {
                    if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                    {
                        await DoSpellLogic(cb);
                    }
                }
                return true;
            }
            return false;
        }
        public static async Task<bool> CombatBuffBehaviors()
        {
            if (EC.CR != null)
            {
                foreach (CombatBehavior cb in EC.CR.TCombatBehaviors)
                {
                    if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                    {
                        await DoSpellLogic(cb);
                    }
                }
                return true;
            }
            return false;
        }
        public static async Task<bool> PullBehaviors()
        {
            if (EC.CR != null)
            {
                foreach (CombatBehavior cb in EC.CR.TPullBehaviors)
                {
                    if (SpellManager.HasSpell(cb.SpellId) && SpellManager.CanCast(cb.SpellId))
                    {
                        await DoSpellLogic(cb);
                    }
                }
                return true;
            }
            return false;
        }
        public static async Task<bool> DoSpellLogic(CombatBehavior cb)
        {
            if (cb.LastCastTime.AddSeconds(2) > DateTime.Now) return false;
            EC.Log("Ok lets cast a spell! I call it " + cb.SpellName);
            
            /*
             * if this spell relies on my health - we can preclude it right away since we always know what it is
             * this equals less work, less cycles, and less log spam.
             */
            if (cb.Target == TargetType.Me && cb.CastAtHealthPercentage && cb.HealthOperator == Operator.LT)
            {
                //this will only cast if health is UNDER a certain amount anyway so if our health is more, ignore it.
                if (cb.HealthPercentage > Me.HealthPercent)  return false;
            }
            if (cb.Target == TargetType.Me && cb.CastAtHealthPercentage && cb.HealthOperator == Operator.GT)
            {
                //this will only cast if health is OVER a certain amount anyway so if our health is less, ignore it.
                if (cb.HealthPercentage < Me.HealthPercent) return false;
            }

            if (cb.Target == TargetType.Me && cb.CastAtHealthPercentage && cb.HealthPercentage > Me.HealthPercent) return false;

            WoWUnit target = null;
            //Target the right person
            if (cb.IsAura)
            {
                EC.Log("Targeting player for aura");
                if (cb.Aura == AuraTarget.KeepAuraOnHealer)
                {
                    var res = GetHealer();
                    if (res != null) target = res.ToPlayer();
                    else
                    {
                        EC.Log("there is no Healer to cast this on, skipping rotation");
                        return false;
                    }
                }
                if (cb.Aura == AuraTarget.KeepAuraOnMe) target = Me;
                if (cb.Aura == AuraTarget.KeepAuraOnPartyMembers) target = GetMemberWithoutAura(cb.AuraName);
                if (cb.Aura == AuraTarget.KeepAuraOnTank)
                {
                    var res = GetTank();
                    if (res != null) target = res.ToPlayer();
                    else
                    {
                        EC.Log("there is no Tank to cast this on, skipping rotation");
                        return false;
                    }
                }
                if (cb.Aura == AuraTarget.ApplyAuraToAttackingMobs) target = TargetMobWithoutAura(cb.AuraName);
            }
            if (cb.HasTargetingRules)
            {
                EC.Log("Targeting something for spell");
                if (cb.Target == TargetType.Healer)
                {
                    var res = GetHealer();
                    if (res != null) target = res.ToPlayer();
                    else
                    {
                        EC.Log("there is no Healer to cast this on, skipping rotation");
                        return false;
                    }
                }
                if (cb.Target == TargetType.Tank)
                {
                    var res = GetTank();
                    if (res != null) target = res.ToPlayer();
                    else
                    {
                        EC.Log("there is no Tank to cast this on, skipping rotation");
                        return false;
                    }
                }
                if (cb.Target == TargetType.LowestHealthPartyMember)
                {
                    target = GetLowestHealthPartyMember();
                    if (target == null || !Me.GroupInfo.IsInParty)
                    {
                        EC.Log("we are not in a group skipping this rotation");
                        return false;
                    }
                }
                if (cb.Target == TargetType.Add)
                {
                    target = GetAdd();
                    if (target == null)
                    {
                        EC.Log("no 'adds' detected, skipping rotation.");
                        return false;
                    }
                }
                if (cb.Target == TargetType.Mob)
                {
                    target = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(m => m.IsTargetingMeOrPet && m.IsHostile).OrderBy(d => d.HealthPercent).FirstOrDefault();
                    if (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInRaid) target = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(m => m.IsTargetingMyPartyMember && m.IsHostile || Me.IsTargetingMyRaidMember && m.IsHostile).OrderBy(d => d.Distance).FirstOrDefault();
                    if (cb.UseTanksTarget && !cb.DontUseInGroups)
                    {
                        EC.Log("Targeting the TANK's target.");
                        //variable declaration inside teh game loop is bad but lets just get this done.
                        var tt = GetTank();
                        if (tt != null)
                        {
                            var ct = tt.ToPlayer().CurrentTarget;
                            if (ct != null && ct.IsHostile) target = ct;
                        }
                        else
                        {
                            EC.Log("Cant find the tank or his target so we are just going to find the nearest hostile.");
                            target = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(m => m.IsTargetingMeOrPet && m.IsHostile).OrderBy(d => d.HealthPercent).FirstOrDefault();
                        }
                    }
                    EC.Log("targeting MOB.." + target.Name);
                }

                if (cb.Target == TargetType.Me)
                {
                    target = Me;
                }
                if (Me.Pet != null)
                {
                    if (cb.TargetPet) target = Me.Pet;
                }

                if (target != null) target.Target();
                else
                {
                    EC.Log("Found no appropriate target for this spell..." +cb.ToJSON());
                }
            }

            if (target != null && Me.CurrentTarget != null)
            {
                EC.Log("Targeting " + target.SafeName);
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

                if (!_Cast) EC.Log(cb.SpellName + ": not casting yet - conditions aren't right.");
                if (_Cast && !target.IsMe && !Me.IsFacing(target.Location)) target.Face();
                
                //this is so we dont spam the same spell over and over and over again.
                cb.LastCastTime = DateTime.Now;

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
                EC.Log("No Target." + cb.ToJSON() + "\r\n");
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
                ).FirstOrDefault() != null) return true;
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
            foreach (WoWUnit mob in ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsTargetingMeOrPet))
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
            return Me.PartyMembers.Where(p => p.HealthPercent != 100).OrderByDescending(p => p.HealthPercent).FirstOrDefault();
        }

        private static WoWUnit GetAdd()
        {
            return (WoWUnit)ObjectManager.ObjectList.Where(o => o.Type == WoWObjectType.Unit).Where(o => ((WoWUnit)o).IsTargetingMyPartyMember).OrderBy(o => ((WoWUnit)o).HealthPercent).FirstOrDefault();
        }
        #endregion
    }
}
