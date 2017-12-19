using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Action = Styx.TreeSharp.Action;
using Sequence = Styx.TreeSharp.Sequence;
using Styx.WoWInternals;

using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using System;
using Eclipse.WoWDatabase;
using Eclipse.WoWDatabase.Models;
using Styx.WoWInternals.WoWObjects;
using Bots.Professionbuddy.BehaviorTree;
using Styx.CommonBot;
namespace Eclipse.Bots.SkinBot
{
    public class EclipseSkinBot : BotBase
    {
        #region Overrides
        private static LocalPlayer Me;
        private static WoWPlayer Leader;
        private EclipseConfigForm _gui;
        private bool _isRunning;
        private bool _isInit = false;
        private Composite _root;
        private static Location loc;

        public override string Name
        {
            get { return "Eclipse - SkinBot"; }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override Form ConfigurationForm
        {
            get
            {
                if (_gui == null) _gui = new EclipseConfigForm();
                return _gui;
            }
        }

        public override void Start()
        {
            if (Me == null) Me = StyxWoW.Me;

        }

        public override void Stop()
        {
            _isRunning = false;
            EC.log("Stop Called");
            base.Stop();
        }

        public override void Pulse()
        {
            Core.Pulse();
        }

        public override void Initialize()
        {
            try
            {
                if (!_isInit)
                {
                    Core.Initialize();
                }
            }
            catch (Exception ex)
            {
                Logging.Write(Colors.Red, ex.ToString());
            }
        }
        public override Styx.TreeSharp.Composite Root
        {
            get
            {
                return _root ?? (_root =
                    new PrioritySelector(
                        CreateCombatBehavior(),
                        CreateWaitBehavior(),
                        CreateFollowBehavior(),
                        CreatePatrolBehavior()

                        //, //if there is no-one to follow this node will return false and NOT run the next node.
                    //CreateQuestBehavior()
                    ));
            }
        }
        #endregion

        #region Waiting Behavior
        private Composite CreateWaitBehavior()
        {
            return new PrioritySelector(
                // Wait on transport
                new Decorator(ret => Me.IsOnTransport,
                    new Sequence(
                        new Action(ret => TreeRoot.StatusText = "Flying on transport"),
                        new WaitContinue(5, ret => Me.IsOnTransport, new ActionAlwaysSucceed()),
                            new ActionAlwaysFail() // if we are still on transport after 5 seconds wait again
                    )
                ),
                // Wait on group members to catch  up
                new Decorator(ret => EC.PartyMode && !GroupAssembled(40),
                    new Sequence(
                        new Action(ret => TreeRoot.StatusText = "Waiting on Party Members"),
                        new WaitContinue(5, ret => !GroupAssembled(EC.PartyDistance), new ActionAlwaysSucceed()),
                        new ActionAlwaysFail()
                    )
                )

            );
        }

        #endregion

        #region Professions
        private static void TargetClosestSkinnableMob()
        {
            var mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n)).OrderBy(m => m.Distance).ToList();
            if (mobs.Count > 0)
            {
                foreach (WoWUnit mob in mobs)
                {
                    if (mob.CanSkin && mob.Skinnable)
                    {
                        EC.log(string.Format("Targeting dead skinnable mob {0}", mob.Name));
                        mob.Target();
                    }

                    var m = Core.MOBs.Where(cm => cm.isSkinnable && cm.Entry == mob.Entry).FirstOrDefault();
                    if (m != null)
                    {
                        EC.log(string.Format("Found mob that is a known skinnable: {0}", m.Name));
                        mob.Target();
                    }
                }
            }

            EC.log("No skinnable mob found.");

        }
        private static void TargetClosestLootableMob()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n) && ((WoWUnit)n).Lootable).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                mob.Target();
            }
            EC.log("Did not find a lootable mob.");
        }
        #endregion

        #region Patrol Behavior
        private static Composite CreatePatrolBehavior()
        {
            //new Decorator(ret=> SpellManager.Cast("Skinning"),
            //new Decorator(ret => RoutineManager.Current.PullBehavior != null, RoutineManager.Current.CombatBehavior),
            //var mob =  ClosestSkinnableMob();
            // Navigator.FlightorLandSystem.MoveTo(new WoWPoint(loc.X, loc.Y, loc.Z))
            //int _castAttempts = 0;
            return new PrioritySelector(
                new Decorator(ret => EC.FarmMode && !Me.IsActuallyInCombat,
                    new PrioritySelector(
                        new Decorator(ret => Me.CurrentTarget == null, new Action(r => TargetClosestSkinnableMob())),
                        new Decorator(ret => Me.CurrentTarget == null, new Action(r => TargetClosestLootableMob())),
                        new Decorator(ret => Me.CurrentTarget == null, new Action(r => GetNextSkinningLocation())),
                        new Decorator(ret => Me.CurrentTarget != null,
                            SkinningRoutine()
                        ))                    
                )
            );

        }

        private static void LogNothing()
        {
            EC.log("No mobs and around and no location to pat to - we are gonna do... NOTHING!");
        }
        public static Composite SkinningRoutine()
        {
            return new PrioritySelector(
                new Decorator(ret => Me.CurrentTarget.IsAlive,
                    new PrioritySelector(
                        new Decorator(ret => Me.CurrentTarget.Distance <= 20 && RoutineManager.Current.PullBehavior != null, RoutineManager.Current.CombatBehavior),
                        new Decorator(ret => Me.CurrentTarget.Distance > 20 && RoutineManager.Current.PullBehavior != null,
                            new Sequence(
                                new Action(r => EC.log("MoveToUnit")),
                                new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to kill it and wear it's skin.", Me.CurrentTarget.Name)),
                                new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                    new Action(r => Navigator.FlightorLandSystem.MoveTo(Me.CurrentTarget.Location))),
                                new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                    new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                                new ActionAlwaysSucceed())
                        )
                    )),
                new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.TaggedByMe,
                    new Sequence(
                        new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} for some epic skinning action.", Me.CurrentTarget.Name)),
                                new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                    new Action(r => Navigator.FlightorLandSystem.MoveTo(Me.CurrentTarget.Location))),
                                new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                    new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                                    new ActionAlwaysSucceed()
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
            );
        }
        public static void GetNextSkinningLocation()
        {
            EC.log("Looking for known hotspots.");
            var _loc = Core.Locations.Where(l=>
                l.Zone == Me.ZoneId 
                && !Core.RecentlyVisitedLocations.Contains(l))
                    .OrderBy(d=> 
                        Core.Distance(
                            new float[3]{d.X, d.Y, d.Z},
                            new float[3]{Me.X, Me.Y, Me.Z} ))
                        .FirstOrDefault();
            if (new float[3] { loc.X, loc.Y, loc.Z } == new float[3] { Me.X, Me.Y, Me.Z })
            {
                Core.RecentlyVisitedLocations.Add(_loc);
                GetNextSkinningLocation();
            }
            if (_loc != null) loc = _loc;
            else
            {
                EC.log("No more saved locations to visit.");
                Core.RecentlyVisitedLocations.Clear();
                loc = null;
            }


        }
        private static bool canIKillForSkin(WoWUnit woWUnit)
        {
            var skinnable = false;
            var t = woWUnit;
            if (t.IsDead && t.Skinnable) return true;
            var mob = Core.MOBs.Where(n => n.Entry == t.Entry).FirstOrDefault();
            if (mob != null)
            {
                if (isSkinnable(t) && t.Level > 1) skinnable = true;
            }
            return skinnable;
        }
        private static bool isSkinnable(WoWUnit woWUnit)
        {
            var t = woWUnit;
            Me = StyxWoW.Me;
            if (Me.CurrentTarget != null
                && t.IsDead
                && !t.Lootable
                && t.Skinnable
                && t.CanSkin
                && !Me.IsCasting
                && !Me.IsChanneling
                && !Me.Combat
                && !Me.Looting
                && t.Distance <= Me.InteractRange
                && t.TaggedByMe
            )
            return true;
            else return false;
        }
        #endregion

        #region Combat Behavior

        private static bool NeedPull(object context)
        {
            var target = StyxWoW.Me.CurrentTarget;

            if (target == null)
                return false;

            if (!target.InLineOfSight)
                return false;

            if (target.Distance > Targeting.PullDistance)
                return false;

            return true;
        }

        private static Composite CreateCombatBehavior()
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

        #region Follow Behavior

        private static WoWUnit _followMe;
        private static bool _isInitialized;
        private static WoWUnit FollowMe
        {
            get
            {
                if (!_isInitialized && _followMe != null)
                    _followMe.OnInvalidate += new ObjectInvalidateDelegate(_followMe_OnInvalidate);

                if (_followMe == null || !_followMe.IsValid)
                {
                    if (StyxWoW.Me.IsInInstance)
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            string role = Lua.GetReturnVal<string>(string.Format("return UnitGroupRolesAssigned('party{0}')", i), 0);
                            if (role == "TANK")
                                _followMe = ObjectManager.GetObjectByGuid<WoWPlayer>(StyxWoW.Me.GetPartyMemberGuid(i - 1));
                        }
                    }
                    else
                    {
                        _followMe = RaFHelper.Leader ?? StyxWoW.Me.PartyMembers.FirstOrDefault();
                    }
                    RaFHelper.SetLeader(_followMe.Guid);
                }

                if (_followMe.Guid != RaFHelper.Leader.Guid)
                    _followMe = RaFHelper.Leader;


                if (_followMe == null)
                    Logging.Write("Could not find suitable unit to follow!");

                return _followMe;
            }
        }

        static void _followMe_OnInvalidate()
        {
            _followMe = null;
        }

        private static Composite CreateFollowBehavior()
        {
            return new PrioritySelector(
                WhoDoIFollow(),
                new Decorator(ret => StyxWoW.Me.GroupInfo.IsInParty && (EC.FollowTarget != null && EC.FollowTarget.Distance > 10 || EC.FollowTarget != null && !EC.FollowTarget.InLineOfSight),
                    new Action(ret => Navigator.MoveTo(EC.FollowTarget.Location))
                )

            );
        }
        private static Composite WhoDoIFollow()
        {
            return new PrioritySelector(
                new Decorator(ii => Me.IsInInstance && Me.GroupInfo.PartySize > 1,
                    new PrioritySelector(
                        new Decorator(ctx => !Me.IsGroupLeader,
                            new Sequence(
                                new Action(ret => EC.FollowTarget = Me.GroupInfo.GroupLeader)
                            )
                        ),
                        new Decorator(ctx => Me.IsGroupLeader,
                            new Action(ret => EC.FollowTarget = null))

                    )
                )
            );

            //if (Me.IsInInstance && !Me.IsGroupLeader && Me.GroupInfo.PartySize > 1)
            //{
            //    return Me.GroupInfo.GroupLeader;
            //}
            //else if (Me.IsInInstance && Me.IsGroupLeader) return null;
        }
        #endregion

        #region Group Behaviors
        private bool GroupAssembled(float Distance)
        {
            var _assembled = true;
            if (Me.GroupInfo.PartySize > 1 && Me.IsGroupLeader)
            {
                foreach (var dude in Me.GroupInfo.PartyMembers.ToList())
                {
                    var pm = dude.ToPlayer();
                    if (!dude.IsOnline || !pm.IsAlive || pm.Distance > Distance) return false;
                }
            }
            return _assembled;
        }

        #endregion
    }
}
