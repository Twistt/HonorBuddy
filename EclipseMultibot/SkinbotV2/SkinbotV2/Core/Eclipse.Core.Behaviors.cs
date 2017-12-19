using ArachnidCreations;
using ArachnidCreations.DevTools;
using CommonBehaviors.Actions;
using System;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Action = Styx.TreeSharp.Action;
using Sequence = Styx.TreeSharp.Sequence;
using Color = System.Windows.Media.Color;
using Quest = Eclipse.Models.Quest;
using Eclipse.Models;
using Styx.CommonBot.POI;
using Bots.Quest.Actions.Combat;
using Buddy.Coroutines;
using Styx.CommonBot.Coroutines;
using Eclipse.MultiBot.Core;
namespace Eclipse
{
    public partial class EC
    {

        #region Custom Composites

        #endregion
        
        #region Behaviors
        public static Composite CreateLootingBehavior
        {
            get
            {
                return new Decorator(r => Me.CurrentTarget != null,
                    new PrioritySelector(
                        new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.Lootable,
                                    new Sequence(
                                new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))
                                        )
                                    ),
                        new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.Lootable,
                            new Sequence(
                                new Action(r => Me.CurrentTarget.Interact()),
                                new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Name))
                                )
                            )
                        )
                    );
            }
        }
        public static Composite CreateSkinningBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => StyxWoW.Me.CurrentTarget != null,
                                    new PrioritySelector(
                            new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange,
                                             new Sequence(
                                    new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} for some epic skinning action.", Me.CurrentTarget.Name)),
                                       new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                           new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location)))
                                                )
                                            ),
                                        new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.CanLoot,
                                            new Sequence(
                                                new Action(r => Me.CurrentTarget.Interact()),
                                                new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Name))
                           )),
                       new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && isSkinnable(Me.CurrentTarget),
                                            new Sequence(
                                                new Action(r => Me.CurrentTarget.Interact()),
                                                new Action(r => TreeRoot.StatusText = String.Format("Skinning {0}", Me.CurrentTarget.Name))
                                        )),
                       new Decorator(r => Me.IsFlying && Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange,
                           new Sequence(
                               new Action(context => Lua.DoString("Dismount()")),
                               new Wait(4, new ActionAlwaysSucceed()))),
                       new Decorator(r => isSkinnable(Me.CurrentTarget) && !Me.IsFlying,
                                    new Sequence(
                               new Action(r => Me.CurrentTarget.Interact()),
                               new Action(r => TreeRoot.StatusText = String.Format("Skinning {0}", Me.CurrentTarget.Name)),
                               new Wait(4, new ActionAlwaysSucceed())
                                    )
                                )
                        )
               ));
            }
        }
        public static Composite CreateQuestPickupBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => ActiveQuest.Id == ActiveQuestOrder.QuestId, new Action(a=> ActiveQuestOrder.status = QuestStatus.Complete)),
                    new Decorator(ret => GetQuestGiver() && Me.CurrentTarget != null,
                       new Decorator(ret => Me.CurrentTarget.IsAlive,
                           new PrioritySelector(
                               new Decorator(ret => Me.CurrentTarget.Distance <= Me.InteractRange,
                                   new Sequence(
                                        new Action(r => Me.CurrentTarget.Interact()),
                                        new Action(r => TreeRoot.StatusText = String.Format("Getting Quest from {0}", Me.CurrentTarget.Name)),
                                        new Action(r =>ActiveQuestOrder.status = QuestStatus.Complete)
                                    )),
                               new Decorator(ret => Me.CurrentTarget.Distance > Me.InteractRange,
                                   new Sequence(
                                       new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to get a q.", Me.CurrentTarget.Name)),
                                       new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location)),
                                       new ActionAlwaysSucceed()
                                    )
                                )
                            )
                        )
                    )
                );
            }
        }
        public static Composite CreateDeadBehavior //this is commmunity contributed content I assume from  FPSWare's RAF bot. If that IS the case than THANK YOU FPSWare!
        {
            get
            {
                return new PrioritySelector(

                // Mount up - for rez sickness wait
                    //new Decorator(ret => !Me.IsDead && !Me.IsGhost && !Me.Mounted && Me.HasAura(15007) && Flightor.MountHelper.CanMount, Common.MountUpFlying()),

                // Mounted? The ascend and just wait out rez sickness
                new Decorator(ret => Me.Mounted && !Me.IsFlying && !StyxWoW.Me.MovementInfo.IsAscending,
                    new Sequence(
                        new Action(context => Log("Flying up to wait out rez sickness")),
                        new Action(context => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromSeconds(4)))
                    )),

                // Just wait out rez sickness
                new Decorator(ret => Me.IsFlying && Me.HasAura(15007), new Action(ctx => { TreeRoot.StatusText = "Waiting out rez sickness"; TreeRoot.StatusText = "Waiting out rez sickness"; return RunStatus.Success; })),

                // Release body
                new Decorator(ret => Me.IsDead && !Me.IsGhost,
                    new Sequence(
                        new Action(context => Log("We're dead! Releasing corpse")),
                        new Action(context => Lua.DoString("RepopMe()"))
                        )),

                // Try to move to our corpse - if we can
                new Decorator(ret => Me.IsGhost && (Navigator.CanNavigateFully(Me.Location, Me.CorpsePoint) || Navigator.CanNavigateFully(Me.Location, WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 10))),
                    new PrioritySelector(
                    // Move to the location of our corpse
                    // First, try to move to our corpse location exactly
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) > 15 && Navigator.CanNavigateFully(Me.Location, Me.CorpsePoint), new Action(context => Navigator.MoveTo(Me.CorpsePoint))),

                        // If that fails try to move within 10 yards of it
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) > 15 && Navigator.CanNavigateFully(Me.Location, WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 10)), new Action(context => Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 10)))),

                        // Within range of our body? Retrieve our body
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) < 15,
                            new Sequence(
                                new Action(context => Log("Recovering our body")),
                                new Action(context => Lua.DoString("RetrieveCorpse()"))
                        ))
                    ))


                );
            }
        }
        public static Composite CreateHealBehavior()
        {
            return new PrioritySelector(RoutineManager.Current.HealBehavior);
        }
        public static Composite CreateVendorBehavior
        {
            get
            {
                return new Decorator(r => Me.CurrentTarget != null,
                    new PrioritySelector(
                        new Decorator(r => !Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.IsVendor,
                            new Sequence(
                                new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))
                                )
                            ),
                        new Decorator(r => !Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.IsVendor,
                            new Sequence(
                                new Action(r => Me.CurrentTarget.Interact()),
                                new Action(r => TreeRoot.StatusText = String.Format("At Vendor {0}", Me.CurrentTarget.Name)),
                                new Action(r => SellGreys())
                                )
                            )
                        )
                    );
            }
        }
        public static Composite CreateCombatBehavior()
        {
            return new PrioritySelector(

                new Decorator(ret => !StyxWoW.Me.Combat,
                    new PrioritySelector(

            #region Rest

new PrioritySelector(
                // Use the bt
                        new Decorator(ctx => RoutineManager.Current.RestBehavior != null,
                            RoutineManager.Current.RestBehavior),

                            // new ActionDebugString("[Combat] Rest -> Old Behavior"),
                // don't use the bt
                            new Decorator(ctx => RoutineManager.Current.NeedRest,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Resting"),
                                    new Action(ret => RoutineManager.Current.Rest())))
                                    ),

            #endregion

            #region PreCombatBuffs

 new PrioritySelector(
                // new ActionDebugString("[Combat] Checking PCBBehavior"),
                // Use the bt
                            new Decorator(ctx => RoutineManager.Current.PreCombatBuffBehavior != null,
                                RoutineManager.Current.PreCombatBuffBehavior),

                            // don't use the bt
                // new ActionDebugString("[Combat] Checking PCBOld"),
                            new Decorator(
                                ctx => RoutineManager.Current.NeedPreCombatBuffs,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Applying pre-combat buffs"),
                                    new Action(ret => RoutineManager.Current.PreCombatBuff())
                                    ))),

            #endregion

            #region Pull

                // new ActionDebugString("[Combat] Pull"),
                // Don't pull, unless we've decided to pull already.
                        new Decorator(ret => BotPoi.Current.Type == PoiType.Kill,
                            new PrioritySelector(

                                // Make sure we have a valid target list.
                                new Decorator(ret => Targeting.Instance.TargetList.Count != 0,
                // Force the 'correct' POI to be our target.
                                    new Decorator(ret => BotPoi.Current.AsObject != Targeting.Instance.FirstUnit &&
                                        BotPoi.Current.Type == PoiType.Kill,
                                        new Sequence(

                                            new Action(ret => BotPoi.Current = new BotPoi(Targeting.Instance.FirstUnit, PoiType.Kill)),
                                            new Action(ret => BotPoi.Current.AsObject.ToUnit().Target())))),

                                        new Decorator(NeedPull,
                                            new PrioritySelector(
                                                new Decorator(ctx => RoutineManager.Current.PullBuffBehavior != null,
                                                    RoutineManager.Current.PullBuffBehavior),

                                                new Decorator(ctx => RoutineManager.Current.PullBehavior != null,
                                                    RoutineManager.Current.PullBehavior),

                                                    new ActionPull())))))),
            #endregion

             new Decorator(ret => StyxWoW.Me.Combat,

                    new PrioritySelector(

            #region Heal

                new PrioritySelector(
                // Use the Behavior
                            new Decorator(ctx => RoutineManager.Current.HealBehavior != null,
                                new Sequence(
                                    RoutineManager.Current.HealBehavior,
                                    new Action(delegate { return RunStatus.Success; })
                                    )),

                            // Don't use the Behavior
                            new Decorator(ctx => RoutineManager.Current.NeedHeal,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Healing"),
                                    new Action(ret => RoutineManager.Current.Heal())
                                    ))),

            #endregion

            #region Combat Buffs

            new PrioritySelector(
                // Use the Behavior
                            new Decorator(ctx => RoutineManager.Current.CombatBuffBehavior != null,
                                        new Sequence(
                                            RoutineManager.Current.CombatBuffBehavior,
                                            new Action(delegate { return RunStatus.Success; })
                                            )
                                ),

                            // Don't use the Behavior
                            new Decorator(ctx => RoutineManager.Current.NeedCombatBuffs,
                                        new Sequence(
                                            new Action(ret => TreeRoot.StatusText = "Applying Combat Buffs"),
                                            new Action(ret => RoutineManager.Current.CombatBuff())
                                            ))),

            #endregion

            #region Combat

             new PrioritySelector(
                            // Use the Behavior
                                        new Decorator(ctx => RoutineManager.Current.CombatBehavior != null,
                                            new PrioritySelector(
                                                RoutineManager.Current.CombatBehavior,
                                                new Action(delegate { return RunStatus.Success; })
                                                )),

                                        // Don't use the Behavior
                                        new Sequence(
                                            new Action(ret => TreeRoot.StatusText = "Combat"),
                                            new Action(ret => RoutineManager.Current.Combat())))

                        #endregion

            )));
        }
        #endregion

        public static Composite CreatHuntingBehavior { get {
            return new PrioritySelector(
                new Decorator(d=> ActiveQuest.IsCompleted, new Action(d=>ActiveQuestOrder.status = QuestStatus.Complete)),
                new Decorator(d=> ObjectiveComplete(), new Action(a=>ActiveQuestOrder.status = QuestStatus.Complete)),
                new Decorator(d => ActiveQuestOrder.Location != null && qoLoc != null && Distance(new float[3] { qoLoc.X, qoLoc.Y, qoLoc.Z }, new float[3] { Me.X, Me.Y, Me.Z }) >= 40 && Navigator.CanNavigateFully(Me.Location, qoLoc), GotoLoc),
                new Decorator(d => ActiveQuestOrder.Location != null && qoLoc != null && !Navigator.CanNavigateFully(Me.Location, qoLoc), 
                    new Sequence(
                        new Action(a=>EC.getMobLocation(ActiveQuestOrder))
                        )),
                new Decorator(d => ActiveQuestOrder.Location != null && qoLoc != null && Distance(new float[3] { qoLoc.X, qoLoc.Y, qoLoc.Z }, new float[3] { Me.X, Me.Y, Me.Z }) < 40 && EC.TargetClosestQuestMob(uint.Parse(EC.ActiveQuestOrder.MobId)) && EC.AddKillTarget(), RoutineManager.Current.CombatBehavior),
                new Decorator(d => ActiveQuestOrder.Location != null && qoLoc == null, new Action(a=>qoLoc = ActiveQuestOrder.Location))
                //&& EC.TargetClosestQuestMob(uint.Parse(EC.ActiveQuestOrder.MobId)), RoutineManager.Current.CombatBehavior
                );
        
        } }

        private static bool AddKillTarget()
        {
            if (Me.CurrentTarget != null && !ActiveQuestOrder.KilledMobs.Contains(Me.CurrentTarget)) EC.ActiveQuestOrder.KilledMobs.Add(Me.CurrentTarget);
            return true;
        }
        private static bool ObjectiveComplete()
        {
            if (EC.ActiveQuestOrder.KilledMobs.Where(k => k.Entry == uint.Parse(ActiveQuestOrder.MobId) && k.IsDead).Count() >= EC.ActiveQuestOrder.KillCount) return true;
            else return false;
        }

        public static Composite UseItemOnMobBehavior { get {
            return new PrioritySelector(
                new Decorator(d => ActiveQuest.IsCompleted, new Action(d => ActiveQuestOrder.status = QuestStatus.Complete)),
                
                new Decorator(d => ActiveQuestOrder.status == QuestStatus.EnRoute, 
                    new PrioritySelector( 
                        new Decorator(d=> qoLoc.Distance(Me.Location) > Me.InteractRange && Navigator.CanNavigateFully(Me.Location, qoLoc), GotoLoc),
                        new Decorator(d => qoLoc.Distance(Me.Location) > Me.InteractRange && !Navigator.CanNavigateFully(Me.Location, qoLoc), 
                            new Sequence(
                                new Action(a=>EC.Log("Cannot navigate to QO location, looking for mob directly...")),
                                new Action(a=>qoLoc = getMobLocation(ActiveQuestOrder))
                            )),
                        new Decorator(d => qoLoc.Distance(Me.Location) <= Me.InteractRange, new Action(a=> ActiveQuestOrder.status = QuestStatus.InProgress))
                    )),
                new Decorator(d=> ActiveQuestOrder.status == QuestStatus.InProgress && Me.CurrentTarget == null, new Action(a=>getLocalMobById(uint.Parse(ActiveQuestOrder.MobId)))),
                new Decorator(d => Me.CurrentTarget != null && Me.CurrentTarget.Location.Distance(Me.Location) > Me.InteractRange, GotoTargetLoc),
                new Decorator(d => Me.CurrentTarget != null && TemporaryIgnoreList.Contains(Me.CurrentTargetGuid), new Sequence(
                        new Action(a => Me.ClearTarget()),
                        new Action(a => EC.Log("We are done with this whore, whos next?"))
                    )),
                new Decorator(d => Me.CurrentTarget.Entry != ActiveQuestOrder.MobId.ToUInt32(),
                    new Sequence(
                        new Action(a => Me.ClearTarget()),
                        new Action(a => EC.Log("This not the right target - ditching this @!#$.")),
                        new Action(a=>getLocalMobById(uint.Parse(ActiveQuestOrder.MobId)))
                    )),

                new Decorator(d => Me.CurrentTarget != null && Me.CurrentTarget.Location.Distance(Me.Location) <= Me.InteractRange && !Me.IsCasting,
                    new Sequence(
                        //new Action(a=>EC.Log(string.Format("Using item on {0}", Me.CurrentTarget.Name))),
                        new Action(a => Me.Inventory.Backpack.Items.Where(i => i.Entry == ActiveQuestOrder.InventoryItemID).FirstOrDefault().Use()),
                        new Action(a=>
                            new DeferredAction(4000, "Blacklist and retarget after 2 seconds", () => { 
                                //Blacklist.Add(Me.CurrentTarget.Guid, BlacklistFlags.Interact, new TimeSpan(0,0, 20), "Already Interacted with him.");
                                TemporaryIgnoreList.Add(Me.CurrentTargetGuid);
                                var unit = getLocalMobById(uint.Parse(ActiveQuestOrder.MobId));
                                if (unit != null)
                                {
                                    unit.Target();
                                    EC.Log("found a new target..");
                                }
                                return true; 
                            })
                        ),
                        new Action(a => DeferredAction.DoAction())

                    ))
            );
        }}

        public static Composite CastSpellOnMob { get {
            return new PrioritySelector(
                    new Decorator(d => ActiveQuest.IsCompleted, new Action(d => ActiveQuestOrder.status = QuestStatus.Complete)),
                    new Decorator(d => ActiveQuestOrder.status == QuestStatus.EnRoute,
                        new PrioritySelector(
                            new Decorator(d => qoLoc.Distance(Me.Location) > Me.InteractRange && Navigator.CanNavigateFully(Me.Location, qoLoc), GotoLoc),
                            new Decorator(d => qoLoc.Distance(Me.Location) > Me.InteractRange && !Navigator.CanNavigateFully(Me.Location, qoLoc),
                                new Sequence(
                                    new Action(a => EC.Log("Cannot navigate to QO location, looking for mob directly...")),
                                    new Action(a => qoLoc = getMobLocation(ActiveQuestOrder))
                                )),
                            new Decorator(d => qoLoc.Distance(Me.Location) <= Me.InteractRange, 
                                new Sequence(
                                    new Action(a => ActiveQuestOrder.status = QuestStatus.InProgress),
                                    new Action(a => getLocalMobById(ActiveQuestOrder.MobId.ToUInt32()))
                                )
                                
                        ))),
                    new Decorator(d => ActiveQuestOrder.status == QuestStatus.InProgress && Me.CurrentTarget == null, new Action(a => getLocalMobById(ActiveQuestOrder.MobId.ToUInt32()))),
                    new Decorator(d => Me.CurrentTarget != null && Me.CurrentTarget.Location.Distance(Me.Location) > Me.InteractRange, GotoTargetLoc),
                    new Decorator(d => Me.CurrentTarget != null && TemporaryIgnoreList.Contains(Me.CurrentTargetGuid), new Sequence(
                            new Action(a => Me.ClearTarget()),
                            new Action(a => EC.Log("We are done with this whore, whos next?"))
                        )),
                    new Decorator(d => ActiveQuest.IsCompleted, new Action(d => ActiveQuestOrder.status = QuestStatus.Complete)),
                    new Decorator(d => Me.CurrentTarget == null && !Me.IsCasting, new Action(a => getLocalMobById(ActiveQuestOrder.MobId.ToUInt32()))),
                    new Decorator(d => Me.CurrentTarget != null && Me.CurrentTarget.Location.Distance(Me.Location) <= Me.InteractRange && !Me.IsCasting,
                        new Sequence(
                            new Action(a=>EC.Log(string.Format("Casting spell on {0}", Me.CurrentTarget.SafeName))),
                            new Action(a => SpellManager.CastSpellById(EC.ActiveQuestOrder.SpellId)),
                            new Decorator(d => ActiveQuest.IsCompleted, new Action(d => ActiveQuestOrder.status = QuestStatus.Complete)),
                            new Wait(4, new ActionAlwaysSucceed())

                        ))
                );
        
        } }
    }
}
