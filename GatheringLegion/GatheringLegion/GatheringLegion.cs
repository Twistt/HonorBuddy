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
using Styx;
using System;
using CommonBehaviors.Actions;
using Eclipse.WoWDatabase;
using Eclipse.WoWDatabase.Models;
using System.Numerics;
using System.Windows.Media;
using ArachnidCreations;
using ArachnidCreations.DevTools;

namespace Eclipse.Bots.GatheringLegion
{
    public class EclipseGatheringLegion : BotBase
    {
        #region Overrides
        private static LocalPlayer Me;
        private static WoWPlayer Leader;
        private EclipseConfigForm _gui;
        private bool _isRunning;
        private bool _isInit = false;
        private Composite _root;
        private static Location loc;
        private static WoWGameObject wgo = null;
        private static Location NodeLocation = null;
        private static WoWGuid targetGuid = new WoWGuid();
        private static DateTime TargetTime = DateTime.Now;
        private static TimeSpan Delay_WowClientLagTime { get { return (TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150)); } }
        private static TimeSpan PostInteractDelay { get { return (TimeSpan.FromMilliseconds(1500));
    }
}
        public override string Name
        {
            get { return "Eclipse - GatheringLegion Ver 0.2"; }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override Form ConfigurationForm
        {
            get
            {
                if (!_isRunning)
                {
                    MessageBox.Show("You must start this bot to use live configuration.");
                    return null;
                }
                else return new EclipseConfigForm();
            }
        }

        public override void Start()
        {
            if (Me == null) Me = StyxWoW.Me;
            if (!Core.init) Core.Initialize();
            _isRunning = true;
        }

        public override void Stop()
        {
            _isRunning = false;
            EC.log("Stop Called");
            base.Stop();
        }

        public override void Pulse()
        {
            //Core.Pulse();
        }

        public override void Initialize()
        {
            try
            {
                if (!_isInit)
                {
                    if (!Core.init)
                        Core.Initialize();
                }
            }
            catch (Exception ex)
            {
                Logging.Write(Color.FromRgb(255,0,80), ex.ToString());
            }
        }
        public override Composite Root
        {
            get
            {
                return _root ?? (_root =
                    new PrioritySelector(

                        new Decorator(ret => Core.ForceNav && !Me.IsDead, NavBehavior()),
                        new Decorator(ret => StyxWoW.Me.IsGhost || Me.IsDead, CreateDeadBehavior()),
                        new Decorator(ret => !Core.PassiveMode && !StyxWoW.Me.IsGhost,
                            new PrioritySelector(

                                CreateFoxFlowerBehavior(),
                                new Decorator(ret => (EC.TimersUpdated.AddMilliseconds(200) < DateTime.Now) && UpdateTimers(), new PrioritySelector()),
                                new Decorator(ret => !EC.Settings.DontStopToFight, CreateCombatBehavior()),
                                new Decorator(ret => EC.Settings.DontStopToFight && !Me.Mounted, CreateCombatBehavior()),
                                new Decorator(ret => EC.Settings.DontStopToFight && Me.Mounted && !Me.IsMoving, CreateCombatBehavior()),
                                 
                                CreateLootingBehavior(),
                                CreateWaitBehavior(),
                                CreateFollowBehavior(),
                                new Decorator(ret => Core.BagsFull && FindNearestVendor(), NavBehavior()),
                                new Decorator(ret => Core.GatherMode, CreatePatrolBehavior()),
                                new Decorator(d => EC.Settings.GatherChests && CheckForChests(), CreateTreasureChestBehavior())
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
                new Action(r => Core.Pulse()),
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
                        new WaitContinue(Delay_WowClientLagTime, ret => Me.IsOnTransport, new ActionAlwaysSucceed()),
                            new ActionAlwaysFail() // if we are still on transport after 5 seconds wait again
                    )
                ),
                // Wait on group members to catch  up
                new Decorator(ret => EC.PartyMode && !GroupAssembled(40),
                    new Sequence(
                        new Action(ret => TreeRoot.StatusText = "Waiting on Party Members"),
                        new WaitContinue(Delay_WowClientLagTime, ret => !GroupAssembled(EC.PartyDistance), new ActionAlwaysSucceed()),
                        new ActionAlwaysFail()
                    )
                )

            );
        }

        #endregion

        #region Professions
        private static bool GetNearestMiningNode()
        {
            var units = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().OrderBy(m => m.Distance).ToList();
            var objects = ObjectManager.GetObjectsOfTypeFast<WoWGameObject>().OrderBy(m => m.Distance).ToList();
            if (objects.Count == 0) EC.log(string.Format("There are no objects nearby"));
            foreach (var obj in objects)
            {
                if (Core.RecentlyHarvested.Contains(obj.Guid)) continue;
                var o = (WoWGameObject)obj;
                if (o != null && o.SubType == WoWGameObjectType.GatheringNode)
                {
                    SpellFindResults herbspellid = null;
                    SpellManager.FindSpell("Herb Gathering", out herbspellid);

                    SpellFindResults miningspell = null;
                    SpellManager.FindSpell("Mining Skills", out miningspell);

                    if (o.Name.Contains("Mana") && !EC.Settings.GatherManaNodes || (WoWCurrency.GetCurrencyById(1155).TotalMax - WoWCurrency.GetCurrencyById(1155).Amount < 5) ) continue;
                    if (o.IsHerb && herbspellid == null) continue;
                    if (o.IsMineral && miningspell == null) continue;
                    
                    if (EC.Settings.AvoidElites)
                    {
                        var closetomobs = units.Where(m => m.Elite && Vector3.Distance(o.Location, m.Location) < 20).ToList();
                        if (closetomobs.Count > 0)
                        {
                            Core.log(string.Format("Skipping {0} because it is too close to elite mobs", o.Name));
                            Core.RecentlyHarvested.Add(o.Guid);
                            continue;
                        }
                    }

                    if (!CanNavigate(o))
                    {
                        //ToDo: probably need to blacklist this...
                        EC.log(string.Format("Cant find a path to ", obj.Name));
                        continue;
                    }
                    EC.log(string.Format("Found a mining node", obj.Name));
                    wgo = o;
                    var loc = new Location { Entry = o.Entry, X = o.X, Y = o.Y, Z = o.Z, Zone = StyxWoW.Me.ZoneId, Name=obj.Name };
                    NodeLocation = loc;
                    var savedloc = Core.Locations.Where(l => l.Z == loc.Z && loc.Y == l.Y && loc.Z == l.Z && loc.Entry == l.Entry && loc.Zone == l.Zone).FirstOrDefault();
                    if (savedloc == null)
                    {
                        //ToDo: see if these are close to another hotspot (with 20 yards) and dont add them - probably best on a new thread...
                        Core.log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, o.Name));
                        DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                        Core.Locations.Add(loc);
                    }
                }
                if (wgo != null) return true;
            }
            return false;
        }
        private static bool TargetClosestSkinnableMob()
        {

            var mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n)).OrderBy(m => m.Distance).ToList();
            if (mobs.Count > 0)
            {
                foreach (WoWUnit mob in mobs)
                {
                    //this could be done better - but i dont feel like it
                    var skinMob = false;
                    if (Core.NinjaSkin && !mob.TaggedByMe) skinMob = true;
                    if (Core.NinjaSkin && mob.TaggedByMe || Core.NinjaSkin && mob.TaggedByOther) skinMob = true;
                    if (!Core.NinjaSkin && !mob.TaggedByMe) skinMob = false;

                    if (skinMob && mob.CanSkin && mob.Skinnable)
                    {
                        EC.log(string.Format("Targeting dead skinnable mob {0}", mob.Name));
                        mob.Target();
                        return true;
                    }

                    var m = Core.MOBs.Where(cm => cm.isSkinnable && cm.Entry == mob.Entry && Core.IgnoreList.Where(i => i.Entry == cm.Entry).FirstOrDefault() == null && !EC.IsUnitBlackListed(mob)).FirstOrDefault();

                    if (m != null)
                    {
                        EC.log(string.Format("Found a mob that can be killed/skinned/looted: {0}", m.Name));
                        mob.Target();
                        if (mob.Guid != targetGuid)
                        {
                            targetGuid = mob.Guid;
                            TargetTime = DateTime.Now;
                        }
                        if (mob.Guid == targetGuid && mob.HealthPercent == 100 && (DateTime.Now - TargetTime).Seconds > 30)
                        {
                            EC.BlackList.Add(mob);
                            Me.ClearTarget();
                        }


                        return true;
                    }
                }
            }

            EC.log("No skinnable mob found.");
            return false;

        }
        private static bool TargetClosestLootableMob()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && ((WoWUnit)n).IsDead &&((WoWUnit)n).Lootable && !EC.IsUnitBlackListed((WoWUnit)n) && ((WoWUnit)n).Lootable).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                TreeRoot.StatusText = ("Found a lootable mob");
                mob.Target();
                return true;
            }
            else return false;
        }
        public static Location GetNextSkinningLocation_notWorking()
        {
            EC.log(string.Format("There are {0} known hotspots for zone {1} ({2}) of which {3} have been recently visited.", Core.Locations.Where(l => l.Zone == Me.ZoneId).Count(), Me.ZoneText, Me.ZoneId, Core.RecentlyVisitedLocations.Count()));
            var _loc = Core.Locations.Where(l => l.Zone == Me.ZoneId).FirstOrDefault();
            EC.log(string.Format("Have a loc {0},{1},{2},", _loc.X, _loc.Y, +loc.Z));
            if (_loc == null)
            {
                EC.log("Found " + Core.Locations.Where(l => l.Zone == Me.ZoneId).ToList().OrderBy(d => Core.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).Count() + " viable locations");
                return null;
            }
            else
            {
                var nearby = (Me.X - loc.X) * 2 + (Me.Y - loc.Y) * 2 < 10 * 2;
                if (nearby)
                {
                    EC.log("point is within 10 yards of me.");
                }
                if (nearby)
                {
                    EC.log("Closest location is where we already are - adding to visited.");
                    Core.RecentlyVisitedLocations.Add(_loc);
                    return GetNextLocation();
                }
                else
                {
                    EC.log("Patting to new loc.");
                    loc = _loc;
                    return loc;
                }
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
        private static bool CheckForFoxFlowers()
        {
            var objects = ObjectManager.GetObjectsOfType<WoWAreaTrigger>(false, false).OrderBy(d => d.Distance);
            foreach (var obj in objects)
            {
                if (Core.RecentlyHarvested.Contains(obj.Guid)) continue;
                if (obj.Entry == 9756)
                {
                    var o = obj;
                    EC.log(string.Format("Found a foxflower", obj.Name));
                    var loc = new Location { Entry = o.Entry, X = o.X, Y = o.Y, Z = o.Z, Zone = StyxWoW.Me.ZoneId };
                    NodeLocation = loc;
                    return true;
                }

            }
            return false;
        }
        private static bool CheckForChests()
        {
            var units = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().OrderBy(m => m.Distance).ToList();
            var objects = ObjectManager.GetObjectsOfTypeFast<WoWGameObject>().OrderBy(m => m.Distance).ToList();
            foreach (var obj in objects)
            {
                if (Core.RecentlyHarvested.Contains(obj.Guid)) continue;
                var o = (WoWGameObject)obj;
                if (o.SubType == WoWGameObjectType.Chest)
                {
                    if (EC.Settings.AvoidElites)
                    {
                        var closetomobs = units.Where(m => m.Elite && Vector3.Distance(o.Location, m.Location) < 10).ToList();
                        if (closetomobs.Count > 0)
                        {
                            EC.log(string.Format("Skipping {0} because it is too close to elite mobs", o.Name));
                            Core.RecentlyHarvested.Add(o.Guid);
                            continue;
                        }
                    }
                    EC.log(string.Format("Found a treasure chest", obj.Name));
                    wgo = o;
                    var loc = new Location { Entry = o.Entry, X = o.X, Y = o.Y, Z = o.Z, Zone = StyxWoW.Me.ZoneId };
                    NodeLocation = loc;
                    var savedloc = Core.Locations.Where(l => l.Z == loc.Z && loc.Y == l.Y && loc.Z == l.Z && loc.Entry == l.Entry && loc.Zone == l.Zone).FirstOrDefault();
                    if (savedloc == null)
                    {
                        //ToDo: see if these are close to another hotspot (with 20 yards) and dont add them - probably best on a new thread...
                        Core.log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, o.Name));
                        DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                        Core.Locations.Add(loc);
                    }
                }
                if (wgo != null) return true;
            }
            return false;
        }
        #endregion

        #region Items
        public bool BagsFull()
        {
            if (StyxWoW.Me.Inventory.FreeSlots == 0) return true;
            else return false;
        }
        #endregion

        #region Navigation
        //private Composite CreateFindCorpseBehavior()
        //{
        //    return new PrioritySelector(
        //        new Decorator(ret => !AtCorpseLocation(),
        //            new Sequence(
        //                //new Action(r=> TreeRoot.StatusText = "Moving to corpse."),
        //                new Action(r=> Navigator.FlightorLandSystem.MoveTo(Navigator.GeneratePath(Me.Location, Me.CorpsePoint).FirstOrDefault()) ),
        //                //new Action(r => Navigator.FlightorLandSystem.MoveTo(Me.CorpsePoint)),
        //                new ActionAlwaysFail()
        //                )

        //            ));
        //}
        public static Composite CreateDeadBehavior() //this is commmunity contributed content I assume from  FPSWare's RAF bot. If that IS the case than THANK YOU FPSWare!
        {
            return new PrioritySelector(

                // Mount up - for rez sickness wait
                //new Decorator(ret => !Me.IsDead && !Me.IsGhost && !Me.Mounted && Me.HasAura(15007) && Flightor.MountHelper.CanMount, Common.MountUpFlying()),

                // Mounted? The ascend and just wait out rez sickness
                new Decorator(ret => Me.Mounted && !Me.IsFlying && !StyxWoW.Me.MovementInfo.IsAscending,
                    new Sequence(
                        new Action(context => EC.log("Flying up to wait out rez sickness")),
                        new Action(context => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromSeconds(4)))
                    )),

                // Just wait out rez sickness
                new Decorator(ret => Me.IsFlying && Me.HasAura(15007), new Action(ctx => { TreeRoot.StatusText = "Waiting out rez sickness"; TreeRoot.StatusText = "Waiting out rez sickness"; return RunStatus.Success; })),

                // Release body
                new Decorator(ret => Me.IsDead && !Me.IsGhost,
                    new Sequence(
                        new Action(context => EC.log("We're dead! Releasing corpse")),
                        new Action(context => Lua.DoString("RepopMe()"))
                        )),

                // Try to move to our corpse - if we can
                new Decorator(ret => Me.IsGhost,
                    new PrioritySelector(
                        //new Decorator(ret=> !AtCorpseLocation(), new Action(r=>TreeRoot.StatusText = string.Format("Doing Corpse Walk..")) ),
                        // Move to the location of our corpse
                        // First, try to move to our corpse location exactly
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) > 15, new Action(context => Navigator.MoveTo(Me.CorpsePoint))),

                        // If that fails try to move within 10 yards of it
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) > 15, new Action(context => Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 10)))),

                        // Within range of our body? Retrieve our body
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) < 15,
                            new Sequence(
                                new Action(context => EC.log("Recovering our body")),
                                new Action(context => Lua.DoString("RetrieveCorpse()"))
                        ))
                    ))


                );
        }
        public static Composite NavBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => Core.ForceNavLocation != null && !AtMyDestination(),
                    new Sequence(

                        new Action(r => Navigator.MoveTo(new WoWPoint(Core.ForceNavLocation.X, Core.ForceNavLocation.Y, Core.ForceNavLocation.Z))),
                        new ActionAlwaysFail()
                        )

                    ));
        }
        private static bool AtCorpseLocation()
        {
            var distance = Core.Distance(new float[3] { Me.X, Me.Y, Me.Z }, new float[3] { Me.CorpsePoint.X, Me.CorpsePoint.Y, Me.CorpsePoint.Z });
            if (distance > 10)
            {
                TreeRoot.StatusText = string.Format("within {0} of my corpse.", distance);
                return false;
            }
            else
            {
                EC.log("Corpse reached cancelling nav.");
                return true;
            }
        }
        private static bool AtMyDestination()
        {
            var distance = Core.Distance(new float[3] { Me.X, Me.Y, Me.Z }, new float[3] { Core.ForceNavLocation.X, Core.ForceNavLocation.Y, Core.ForceNavLocation.Z });
            if (distance > 10)
            {
                TreeRoot.StatusText = string.Format("within {0} of my distination.", distance);
                return false;
            }
            else
            {
                EC.log("Destination reached cancelling nav.");
                Core.ForceNav = false;
                Core.ForceNavLocation = null;
                return true;
            }
        }
        public static Location GetNextLocation()
        {
            //ToDo: dont revisit recently visited places.
            var _locList = Core.Locations.Where(l => l.Zone == Me.ZoneId && !Core.RecentlyVisitedLocations.Contains(l)).OrderBy(d => Core.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).Where(d => Core.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z }) > 100);
            var skinnablemobs = Core.MOBs.Where(m => m.isSkinnable).ToList();
            foreach (var _loc in _locList)
            {
                //if (EC.Settings.GatheringZones.Where(z => z.Id == _loc.Zone).FirstOrDefault() == null) continue;
                if (EC.Settings.AvoidZones.Where(z => z.Id == _loc.Zone).FirstOrDefault() != null) continue;
                if (Core.GatherMode)
                {
                    var mobtype = skinnablemobs.Where(e => e.Entry == _loc.Entry).FirstOrDefault();
                    if (mobtype != null)
                    {
                        if (!mobtype.isSkinnable) continue;
                    }
                    //else continue;
                }
                var distance = Core.Distance(new float[3] { _loc.X, _loc.Y, _loc.Z }, new float[3] { Me.X, Me.Y, Me.Z });
                //if (_loc != null) EC.log("Found a location!");
                if (distance < 200)
                {
                    EC.log(string.Format("within {0} of a hotspot so we are gonna add this to recently visited.", distance));
                    Core.RecentlyVisitedLocations.Add(_loc);
                    //GetNextSkinningLocation();
                    loc = null;
                }
                else
                {

                    TreeRoot.StatusText = string.Format("Found {0} locations ({2} visited) in {1}", Core.Locations.Where(l => l.Zone == Me.ZoneId).Count(), Core.RecentlyVisitedLocations.Count(), Me.ZoneId);
                    if (CanNavigate(_loc)) loc = _loc;
                    else
                    {
                        Core.RecentlyVisitedLocations.Add(_loc);
                        Core.log("Cant navigate to next location - skipping");
                    }
                    return loc;
                }

            }
            if (loc == null)
            {
                Core.RecentlyVisitedLocations.Clear();
                EC.log("No more saved locations to visit- cleared recent locations.");

                loc = null;
            }
            return loc;
        }
        private static bool FindNearestVendor()
        {
            var npc = Core.NPCs.Where(n => n.isVendor).OrderBy(d => Core.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).FirstOrDefault();
            if (npc != null)
            {
                TreeRoot.StatusText = "(FullBags) Navigating to Vendor...";
                var loc = new Location { Entry = npc.Entry, Name = npc.Name, X = npc.X, Y = npc.Y, Z = npc.Z, Zone = npc.Zone };
                Core.ForceNav = true;
                Core.ForceNavLocation = loc;
                return true;
            }
            else
            {
                EC.log("Bags are full and there is no Vendor!");
                return false;
            }

        }
        #endregion

        #region Patrol Behavior
        private Composite CreateFoxFlowerBehavior()
        {
            return new PrioritySelector(
                new Decorator(d => CheckForFoxFlowers(),
                    new Sequence(
                        new Action(a => Navigator.MoveTo(NodeLocation.V3()))
                    ))
                );

        }
        private Composite CreateTreasureChestBehavior()
        {
            return new PrioritySelector(
                new Decorator(r => NodeLocation != null && Vector3.Distance(Me.Location, NodeLocation.V3()) > Me.InteractRange,
                    new Sequence(
                        new Action(r => TreeRoot.StatusText = String.Format("Moving to {0}", wgo.Name)),
                        new Action(r => Navigator.MoveTo(NodeLocation.V3())))),
                new Decorator(r => NodeLocation != null && Vector3.Distance(Me.Location, NodeLocation.V3()) < Me.InteractRange,
                   new Sequence(
                        new Action(context => Lua.DoString("Dismount()")),
                        new WaitContinue(Delay_WowClientLagTime, new ActionAlwaysSucceed()),
                        new Action(r => TreeRoot.StatusText = String.Format("Collecting {0}", wgo.Name)),
                        new Action(r => Navigator.PlayerMover.MoveStop()),
                        new Action(r => wgo.Interact()),
                        new WaitContinue(TimeSpan.FromSeconds(2), ret => false, new ActionAlwaysSucceed()),
                        new Action(r => wgo.Interact()),
                        new WaitContinue(TimeSpan.FromSeconds(3), ret => false, new ActionAlwaysSucceed()),
                        new Decorator(d => NodeCleared(), new Action(r => TreeRoot.StatusText = String.Format("Chest successfully looted!")))

                        ))
            );
        }
        private static Composite CreateLootingBehavior()
        {
            return new PrioritySelector(
                new Decorator(d => TargetClosestLootableMob(), 
                    new PrioritySelector(
                        new Decorator(r => Me.CurrentTarget != null && Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.CanLoot,
                            new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                        new Decorator(r => Me.CurrentTarget != null && Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.CanLoot,
                           new Sequence(
                                new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Name)),
                                new WaitContinue(Delay_WowClientLagTime, new Action(r => Me.CurrentTarget.Interact()))
                           ))
                    ))
                );
        }
        private static Composite CreatePatrolBehavior()
        {
            //new Decorator(ret=> SpellManager.Cast("Skinning"),
            return new PrioritySelector(
                new Decorator(ret => EC.FarmMode && !Me.IsActuallyInCombat,
                    new PrioritySelector(
                        new Decorator(ret => Core.GatherMode,
                            new PrioritySelector(
                                new Decorator(r=> GetNearestMiningNode(), CreateMiningBehavior()),
                                new Decorator(r => !GetNearestMiningNode(),
                                    new Sequence(
                                        new Action(r => GetNextLocation()),
                                        new Decorator(ret => loc != null && !Me.IsMoving, new Action(r => Flightor.MoveTo(new Vector3(loc.X, loc.Y, loc.Z)))),
                                        new ActionAlwaysSucceed()
                                    )),
                                new Decorator(ret => SpellManager.CanCast(393) && EC.Settings.SkinThings, 
                                    new Decorator(d=>  Me.CurrentTarget == null && !TargetClosestSkinnableMob(),
                                        new Sequence(
                                            new Action(r => GetNextLocation()),
                                            new Decorator(ret => loc != null, new Action(r => Navigator.MoveTo(new Vector3(loc.X, loc.Y, loc.Z)))),
                                            new ActionAlwaysSucceed()
                                        )))
                            )
                        )
                    )
                )
            );

        }
        private static Composite CreateMiningBehavior()
        {
            return new PrioritySelector(
                new Decorator(r => NodeLocation != null && Vector3.Distance(Me.Location, NodeLocation.V3()) > Me.InteractRange && wgo != null,
                    new Sequence(
                        new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} node", wgo.Name)),
                        new Action(r => Navigator.MoveTo(NodeLocation.V3())))),
                new Decorator(r => NodeLocation != null && Vector3.Distance(Me.Location, NodeLocation.V3()) < Me.InteractRange && wgo !=null,
                   new Sequence(
                        new Action(context => Lua.DoString("Dismount()")),
                        new WaitContinue(Delay_WowClientLagTime, new ActionAlwaysSucceed()),
                        new Action(r => TreeRoot.StatusText = String.Format("Mining {0} node", wgo.Name)),
                        new Action(r => Navigator.PlayerMover.MoveStop()),
                        new Decorator(d=>wgo != null, new Action(r => wgo.Interact())),
                        new WaitContinue(TimeSpan.FromSeconds(2), ret => false, new ActionAlwaysSucceed()),
                        new Decorator(d => wgo != null, new Action(r => wgo.Interact())),
                        new WaitContinue(TimeSpan.FromSeconds(3), ret => false, new ActionAlwaysSucceed()),
                        new Decorator(d => NodeCleared(), new Action(r => TreeRoot.StatusText = String.Format("Node successfully gathered!")))
                      
                        ))
            );
        }
        private static Composite CreateSkinningBehavior()
        {
            return new PrioritySelector(
               new Decorator(ret => Me.CurrentTarget != null && Me.CurrentTarget.IsAlive,
                   new PrioritySelector(
                       new Decorator(ret => Me.CurrentTarget.Distance <= 40 && RoutineManager.Current.PullBehavior != null, RoutineManager.Current.CombatBehavior),
                       new Decorator(ret => Me.CurrentTarget.Distance > 40 && RoutineManager.Current.PullBehavior != null,
                           new Sequence(
                               new Decorator(ret => (DateTime.Now - TargetTime).Seconds > 5 && Me.CurrentTarget.Distance > 10, new Action(a => TargetClosestSkinnableMob())),
                               //new Action(r => EC.log(string.Format("MoveToUnit for {0} seconds", (DateTime.Now - TargetTime).Seconds))),
                               new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to kill it and wear it's skin for {1} seconds.", Me.CurrentTarget.Name, (DateTime.Now - TargetTime).Seconds)),
                               new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location)),
                               new ActionAlwaysSucceed())
                       )
                   )),
               new Decorator(r => Me.CurrentTarget != null && Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange, 
                   new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
               new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.CanLoot,
                   new Sequence(
                        new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Name)),
                        new WaitContinue(Delay_WowClientLagTime, new Action(r => Me.CurrentTarget.Interact()))
                   )),
               new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && isSkinnable(Me.CurrentTarget),
                   new Sequence(
                       new Action(r => TreeRoot.StatusText = String.Format("Skinning {0}", Me.CurrentTarget.Name)),
                       new WaitContinue(Delay_WowClientLagTime, new Action(r => Me.CurrentTarget.Interact()))
                   )),
               new Decorator(r => Me.IsFlying && Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange,
                   new Sequence(
                       new Action(context => Lua.DoString("Dismount()")),
                       new WaitContinue(Delay_WowClientLagTime, new ActionAlwaysSucceed()))),
               new Decorator(r => isSkinnable(Me.CurrentTarget) && !Me.IsFlying,
                   new Sequence(
                       new Action(r => TreeRoot.StatusText = String.Format("Skinning {0}", Me.CurrentTarget.Name)),
                       new WaitContinue(new TimeSpan(0, 0, 2), new Action(r => Me.CurrentTarget.Interact()))

                   )
               )
           );
        }
        #endregion

        #region Helpers (misc)
        public static bool CanNavigate(WoWGameObject target)
        {
            var results = Navigator.LookupPathInfo(target);
            if (results.Navigability == PathNavigability.Navigable) return true;
            else return false;
        }
        public static bool CanNavigate(Location target)
        {
            var result = Flightor.MoveTo(target.V3());
            if (result == MoveResult.PathGenerationFailed) return false;
            else return true;
        }
        public static bool NodeCleared()
        {
            EC.log("Completed harvesting.");
            Core.RecentlyHarvested.Add(wgo.Guid);
            wgo = null;
            NodeLocation = null;
            //Shut up i know this is stupid im sorry. Please forgive me.
            return true;
        }
        public static bool UpdateTimers()
        {

            return true;
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
                                            new Action(ret => BotPoi.Current.AsObject.ToUnit().Target()))))

                                                    //,new Decorator(NeedPull,
                                                    //    new PrioritySelector(
                                                    //        new Decorator(ctx => RoutineManager.Current.PullBuffBehavior != null,
                                                    //            RoutineManager.Current.PullBuffBehavior),

                                                    //        new Decorator(ctx => RoutineManager.Current.PullBehavior != null,
                                                    //            RoutineManager.Current.PullBehavior), n>))

                                                    )))),
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
                new Decorator(ret => StyxWoW.Me.GroupInfo.IsInParty && (EC.FollowTarget != null && EC.FollowTarget.Distance > 20 || EC.FollowTarget != null && !EC.FollowTarget.InLineOfSight),
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
