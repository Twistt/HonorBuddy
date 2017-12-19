using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Levelbot.Actions.Combat;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using Sequence = Styx.TreeSharp.Sequence;
using Styx.WoWInternals;

using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Styx;
using System;
using CommonBehaviors.Actions;
using Eclipse.Models;
using Eclipse.MultiBot;
using ArachnidCreations.DevTools;
using Styx.CommonBot.Frames;
using ArachnidCreations;
using System.Reflection;
namespace Eclipse.Bots.MultiBot
{
    public class EclipseMultiBot : BotBase
    {
        #region Global Vars
        private static MultiBotSettings settings;
        public static bool PartyMode = false;
        public static float PartyDistance = 40;
        public static WoWUnit FollowTarget = null;
        public static bool FarmMode = true;
        #endregion

        #region Overrides
        private static LocalPlayer Me;
        private static WoWPlayer Leader;
        private EclipseConfigForm _gui;
        private bool _isInit = false;
        private Composite _root;
        public static Location loc;
        public static WoWPoint qoLoc;
        private static WoWGuid targetGuid; //For Blacklisting things that stick around...
        private static WoWObject questItem;
        public static WoWUnit target;
        private static int SkinningAttempts;
        public override string Name
        {
            get { return "Eclipse - MultiBot 1.1"; }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override Form ConfigurationForm
        {
            get
            {
                if (_gui == null || _gui.IsDisposed) _gui = new EclipseConfigForm();
                return _gui;
            }
        }

        public override void Start()
        {
            EC.StartupPath = Application.StartupPath;
            if (Me == null) Me = StyxWoW.Me;
            Initialize();
        }

        public override void Stop()
        {
            EC.Log("Stop Called");
            base.Stop();
        }

        public override void Pulse()
        {
            //EC.Pulse();
        }
        private static bool _isInitialized = true;
        public override void Initialize()
        {
            try
            {
                if (!EC.init)
                {
                    EC.Log("doing init.");
                    EC.FindDB();
                    DataLoader.loadQuestorders();
                    DataLoader.loadNPCs();
                    DataLoader.loadMobs();
                    //DataLoader.loadVendors();
                    EC.init = true;
                }
            }
            catch (Exception ex)
            {
                Logging.Write(Colors.Red, ex.ToString());
            }
        }
        public override Composite Root
        {
            get
            {
                return _root ?? (_root =
                    new PrioritySelector(

                        new Decorator(ret=> EC.ForceNav,  NavBehavior()),
                        new Decorator(ret => !EC.PassiveMode && !StyxWoW.Me.IsGhost, 
                            new PrioritySelector(
                                CreateCombatBehavior(),
                                CreateWaitBehavior(),
                                CreateFollowBehavior(),
                                new Decorator(ret => EC.BagsFull && EC.FindNearestVendor(), NavBehavior()),
                                new Decorator(ret => EC.SkinMode || EC.KillThese, CreatePatrolBehavior()),
                                new Decorator(ret => EC.QuestingMode, EC.CreateQuestBehavior),
                                new Decorator(ret => StyxWoW.Me.IsGhost || Me.IsDead, CreateDeadBehavior())    
                            )
                        ),
                        new Sequence(
                            LearningBehavior(),
                            new ActionAlwaysSucceed()
                        )  
                ));
            }
        }
        #endregion

        #region LearningBehavior
        private Composite LearningBehavior()
        {
            return new PrioritySelector(
                new Action(r=>Pulse()),
                new ActionAlwaysSucceed()
                );
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
                new Decorator(ret => PartyMode && !GroupAssembled(40),
                    new Sequence(
                        new Action(ret => TreeRoot.StatusText = "Waiting on Party Members"),
                        new WaitContinue(5, ret => !GroupAssembled(PartyDistance), new ActionAlwaysSucceed()),
                        new ActionAlwaysFail()
                    )
                )

            );
        }

        #endregion

        #region Professions
        private static bool TargetClosestSkinnableMob()
        {
            var mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n)).OrderBy(m => m.Distance).ToList();
            if (EC.SkinList.Count > 0) mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n) && IsUnitOnSkinList((WoWUnit)n)).OrderBy(m => m.Distance).ToList();
            else mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n)).OrderBy(m => m.Distance).ToList();
            if (mobs.Count > 0)
            {
                foreach (WoWUnit mob in mobs)
                {
                    if (mob.Guid == targetGuid && SkinningAttempts > 3)
                    {
                        EC.Log("Adding mob to blacklist.");
                        EC.AddMobToBlackList(mob);
                        continue;
                    }
                    //this could be done better - but i dont feel like it
                    var skinMob = false;
                    if (EC.NinjaSkin && !mob.TaggedByMe) skinMob = true;
                    if (EC.NinjaSkin && mob.TaggedByMe || EC.NinjaSkin && mob.TaggedByOther) skinMob = true;
                    if (!EC.NinjaSkin && !mob.TaggedByMe) skinMob = false;

                    if (skinMob && mob.CanSkin && mob.Skinnable )
                    {
                        targetGuid = mob.Guid;
                        EC.Log(string.Format("Targeting dead skinnable mob {0}", mob.Name));
                        mob.Target();
                        return true;
                    }

                    var m = EC.MOBs.Where(cm => cm.isSkinnable && cm.Entry == mob.Entry && EC.IgnoreList.Where(i => i.Entry == cm.Entry).FirstOrDefault() == null).FirstOrDefault();
                    if (m != null)
                    {
                        if (mob.Guid == targetGuid) EC.Log("We recognize thisone." + SkinningAttempts.ToString());
                        targetGuid = mob.Guid;    
                        EC.Log(string.Format("Found a mob that can be killed/skinned/looted: {0}", m.Name));
                        SkinningAttempts = 0;
                        mob.Target();
                        return true;
                    }
                }
            }

            EC.Log("No skinnable mob found.");
            return false;

        }
        public static bool IsUnitOnSkinList(WoWUnit mob)
        {

            if (EC.SkinList.Where(m => m.Name == mob.Name).Count() > 0)
            {
                return true;
            }
            else return false;
        }
        private static WoWUnit TargetClosestLootableMob()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n) && ((WoWUnit)n).Lootable).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                //EC.Log("Found a lootable mob");
                mob.Target();
                return mob;
            }
            else return null;
        }
        public static Location GetNextSkinningLocation_notWorking()
        {
            EC.Log(string.Format("There are {0} known hotspots for zone {1} ({2}) of which {3} have been recently visited.", EC.Locations.Where(l => l.Zone == Me.ZoneId).Count(), Me.ZoneText, Me.ZoneId, EC.RecentlyVisitedLocations.Count()));
            var _loc = EC.Locations.Where(l => l.Zone == Me.ZoneId).FirstOrDefault();
            EC.Log(string.Format("Have a loc {0},{1},{2},", _loc.X, _loc.Y,+loc.Z));
            if (_loc == null)
            {
                EC.Log("Found " + EC.Locations.Where(l => l.Zone == Me.ZoneId).ToList().OrderBy(d => EC.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).Count() + " viable locations");
                return null;
            }
            else
            {
                var nearby = (Me.X - loc.X) * 2 + (Me.Y - loc.Y) * 2 < 10 * 2;
                if (nearby)
                {
                    EC.Log("point is within 10 yards of me.");
                }
                if (nearby)
                {
                    EC.Log("Closest location is where we already are - adding to visited.");
                    EC.RecentlyVisitedLocations.Add(_loc);
                    return GetNextLocation();
                }
                else
                {
                    EC.Log("Patting to new loc.");
                    loc = _loc;
                    return loc;
                }
            }

        }
        #endregion

        #region Items
        public bool BagsFull() {
            if (StyxWoW.Me.Inventory.FreeSlots == 0) return true;
            else return false;
        }
        #endregion

        #region Navigation

        public static Composite CreateDeadBehavior() //this is commmunity contributed content I assume from  FPSWare's RAF bot. If that IS the case than THANK YOU FPSWare!
        {
            return new PrioritySelector( 

                // Mount up - for rez sickness wait
                //new Decorator(ret => !Me.IsDead && !Me.IsGhost && !Me.Mounted && Me.HasAura(15007) && Flightor.MountHelper.CanMount, Common.MountUpFlying()),

                // Mounted? The ascend and just wait out rez sickness
                new Decorator(ret =>  Me.Mounted && !Me.IsFlying && !StyxWoW.Me.MovementInfo.IsAscending,
                    new Sequence(
                        new Action(context => EC.Log("Flying up to wait out rez sickness")),
                        new Action(context => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromSeconds(4)))
                    )),

                // Just wait out rez sickness
                new Decorator(ret => Me.IsFlying && Me.HasAura(15007), new Action(ctx => { TreeRoot.StatusText = "Waiting out rez sickness"; TreeRoot.StatusText = "Waiting out rez sickness"; return RunStatus.Success; })),

                // Release body
                new Decorator(ret => Me.IsDead && !Me.IsGhost,
                    new Sequence(
                        new Action(context => EC.Log("We're dead! Releasing corpse")),
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
                                new Action(context => EC.Log("Recovering our body")),
                                new Action(context => Lua.DoString("RetrieveCorpse()"))
                        ))
                    ))


                );
        }
        public static Composite NavBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => EC.ForceNavLocation != null && !AtMyDestination(),
                    new Sequence(

                        new Action(r => Styx.Pathing.Flightor.MoveTo(new WoWPoint(EC.ForceNavLocation.X, EC.ForceNavLocation.Y, EC.ForceNavLocation.Z))),
                        new ActionAlwaysFail()
                        )
                    
                    ));
        }
        private bool AtCorpseLocation()
        {
            var distance = EC.Distance(new float[3] { Me.X, Me.Y, Me.Z }, new float[3] { Me.CorpsePoint.X, Me.CorpsePoint.Y, Me.CorpsePoint.Z });
            if (distance > 10)
            {
                TreeRoot.StatusText = string.Format("within {0} of my corpse.", distance);
                return false;
            }
            else
            {
                EC.Log("Corpse reached cancelling nav.");
                return true;
            }
        }
        private static bool AtMyDestination()
        {
            var distance = EC.Distance(new float[3] { Me.X, Me.Y, Me.Z }, new float[3] { EC.ForceNavLocation.X, EC.ForceNavLocation.Y, EC.ForceNavLocation.Z });
            if (distance > 5)
            {
                TreeRoot.StatusText = string.Format("within {0} of my distination.", distance);
                return false;
            }
            else
            {
                EC.Log("Destination reached cancelling nav.");
                EC.ForceNav = false;
                EC.ForceNavLocation = null;
                return true;
            }
        }
        public static Location GetNextLocation()
        {
            //ToDo: dont revisit recently visited places.
            List<Location> _locList = EC.Locations.Where(l => l.Zone == Me.ZoneId && !EC.RecentlyVisitedLocations.Contains(l)).OrderBy(d => EC.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).ToList();
            if (EC.SkinMode)
            {
                foreach (Mob mob in EC.MOBs.Where(m=>m.isSkinnable)){
                    var locs = EC.Locations.Where(l => l.Entry == mob.Entry && l.Zone == Me.ZoneId).ToList();
                    if (locs != null)
                    {
                        if (locs.Count > 0) _locList.AddRange(locs);
                    }

                }
            }
            foreach (var _loc in _locList)
            {
                var distance = EC.Distance(new float[3] { _loc.X, _loc.Y, _loc.Z }, new float[3] { Me.X, Me.Y, Me.Z });
                //if (_loc != null) EC.Log("Found a location!");
                if (distance < 40)
                {
                    EC.Log(string.Format("within {0} of a hotspot- and thers still no mobs here, so we are gonna add this to recently visited.", distance));
                    EC.RecentlyVisitedLocations.Add(_loc);
                    //GetNextSkinningLocation();
                    loc = null;
                }
                else
                {

                    TreeRoot.StatusText = string.Format("Found {0} locations ({2} visited) in {1}", EC.Locations.Where(l => l.Zone == Me.ZoneId).Count(), EC.RecentlyVisitedLocations.Count(), Me.ZoneId);
                    loc = _loc;
                    return null;
                }

            }
            if (loc == null)
            {
                EC.RecentlyVisitedLocations.Clear();
                EC.Log("No more saved locations to visit- cleared recent locations.");

                loc = null;
            }
            return loc;
        }
       
        #endregion

        #region Patrol Behavior
        private static bool TargetLootableMob()
        {
            var mobs = ObjectManager.ObjectList.Where(n =>
                n.Type == WoWObjectType.Unit
                && !EC.IsUnitBlackListed((WoWUnit)n)
                && !((WoWUnit)n).TaggedByOther
                && !((WoWUnit)n).IsFriendly
                && ((WoWUnit)n).Lootable
                && ((WoWUnit)n).IsDead
            ).OrderBy(m => m.Distance).ToList();
            if (mobs.Count > 0)
            {
                foreach (WoWUnit mob in mobs)
                {
                    //this could be done better - but i dont feel like it
                    if (mob.IsDead && mob.Lootable)
                    {
                        EC.Log(string.Format("Found a mob that can be looted: {0}", mob.Name));
                        mob.Target();
                        qoLoc = mob.Location;
                        target = mob;
                        return true;
                    }
                }
            }
            return false;

        }
        private static bool TargetClosestMob()
        {
            var mobs = ObjectManager.ObjectList.Where(n =>
                n.Type == WoWObjectType.Unit 
                && !EC.IsUnitBlackListed((WoWUnit)n) 
                && !((WoWUnit)n).TaggedByOther 
                && !((WoWUnit)n).IsFriendly
            ).OrderBy(m => m.Distance).ToList();
           if (mobs.Count > 0)
           {
               foreach (WoWUnit mob in mobs)
               {
                   //this could be done better - but i dont feel like it
                   var m = EC.KillList.Where(cm => cm.Entry == mob.Entry || mob.Lootable).FirstOrDefault();
                   if (m != null && mob.IsAlive || mob.Lootable)
                   {
                        EC.Log(string.Format("Found a mob that can be killed/looted: {0}", m.Name));
                        mob.Target();
                        return true;
                   }
               }
           }

           EC.Log("No mobs from our kill list around.");
           return false;
           
        }
        private static Composite CreatePatrolBehavior()
        {
            //new Decorator(ret=> SpellManager.Cast("Skinning"),
            return new PrioritySelector(
                new Decorator(ret => FarmMode && !Me.IsActuallyInCombat,
                    new PrioritySelector(
                        new Decorator(ret => EC.SkinMode,
                            new PrioritySelector(
                                new Decorator(ret => Me.CurrentTarget == null && !TargetClosestSkinnableMob(),
                                    new Sequence(
                                        new Action(r => GetNextLocation()),
                                        new Decorator(ret => loc != null, new Action(r => Flightor.MoveTo(new WoWPoint(loc.X, loc.Y, loc.Z)))),
                                        new ActionAlwaysSucceed()
                                    )),
                                new Decorator(ret => Me.CurrentTarget != null || TargetClosestSkinnableMob(),
                                   CreateSkinningBehavior()
                                )
                            )
                        ),
                        new Decorator(ret => EC.KillThese,
                            new PrioritySelector(
                                 new Decorator(ret => Me.CurrentTarget == null && !TargetClosestMob(),
                                    new Sequence(
                                        new Action(r => GetNextLocation()),
                                        new Decorator(ret => loc != null,
                                            new Action(r => Flightor.MoveTo(new WoWPoint(loc.X, loc.Y, loc.Z)))),
                                            new ActionAlwaysSucceed()
                                    )),
                                new Decorator(ret=> Me.CurrentTarget != null || TargetClosestMob(),
                                   CreateKillTheseBehavior()
                                )
                            )
                        )
                    )
                )
            );

        }
        private static Composite CreateSkinningBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret=> StyxWoW.Me.CurrentTarget != null,
                    new PrioritySelector(
                   new Decorator(ret => Me.CurrentTarget.IsAlive,
                       new PrioritySelector(
                           new Decorator(ret => Me.CurrentTarget.Distance <= 40 && RoutineManager.Current.PullBehavior != null, RoutineManager.Current.CombatBehavior),
                           new Decorator(ret => Me.CurrentTarget.Distance > 40 && RoutineManager.Current.PullBehavior != null,
                               new Sequence(
                                   new Action(r => EC.Log("MoveToUnit")),
                                   new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to kill it and wear it's skin.", Me.CurrentTarget.Name)),
                                   new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                       new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))),
                                   new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                       new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                                   new ActionAlwaysSucceed())
                           )
                       )),
                   new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && SanityCheck(),
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
                           new Action(r=> SkinningAttempts++),
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

        private static bool SanityCheck()
        {
            if (SkinningAttempts > 5)
            {
                EC.Log("Failed Sanity Check.");
                SkinningAttempts = 0;
                EC.AddMobToBlackList(Me.CurrentTarget);
                Me.ClearTarget();
                return false;
            }
            return true;
        }
        private static bool canIKillForSkin(WoWUnit woWUnit)
        {
            var skinnable = false;
            var t = woWUnit;
            if (t.IsDead && t.Skinnable) return true;
            var mob = EC.MOBs.Where(n => n.Entry == t.Entry).FirstOrDefault();
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
            )
                return true;
            else return false;
        }
        private static Composite CreateKillTheseBehavior()
        {
            return new PrioritySelector(
               new Decorator(ret => Me.CurrentTarget.IsAlive,
                   new PrioritySelector(
                       new Decorator(ret => Me.CurrentTarget.Distance <= 40 && RoutineManager.Current.PullBehavior != null, RoutineManager.Current.CombatBehavior),
                       new Decorator(ret => Me.CurrentTarget.Distance > 40 && RoutineManager.Current.PullBehavior != null,
                           new Sequence(
                               new Action(r => EC.Log("MoveToUnit")),
                               new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to kill it for sport.", Me.CurrentTarget.Name)),
                               new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                   new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))),
                               new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                   new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                               new ActionAlwaysSucceed())
                       )
                   )),
               new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.Lootable,
                   new Sequence(
                       new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} search for bloody corpse trinkets.", Me.CurrentTarget.Name)),
                               new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                   new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))),
                               new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                   new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                                   new ActionAlwaysSucceed()
                       )
                   ),
               new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.Lootable,
                   new Sequence(
                       new Action(r => Me.CurrentTarget.Interact()),
                       new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Name)),
                       new ActionAlwaysSucceed()
                   )),
                new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && !Me.CurrentTarget.Lootable,
                   new Sequence(
                       new Action(r => Me.ClearTarget()),
                       new ActionAlwaysSucceed()
                   ))
           );
        }
        #endregion

        #region Combat Behavior



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

                                        new Decorator(EC.NeedPull,
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

        private static Composite CreateFollowBehavior()
        {
            return new PrioritySelector(
                WhoDoIFollow(),
                new Decorator(ret => StyxWoW.Me.GroupInfo.IsInParty && (FollowTarget != null && FollowTarget.Distance > 20 || FollowTarget != null && !FollowTarget.InLineOfSight),
                    new Action(ret => Navigator.MoveTo(FollowTarget.Location))
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
                                new Action(ret => FollowTarget = Me.GroupInfo.GroupLeader)
                            )
                        ),
                        new Decorator(ctx => Me.IsGroupLeader,
                            new Action(ret => FollowTarget = null))

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

        public enum FarmType
        {
            Skinning,
            MobByLevel
        }

    }
}
