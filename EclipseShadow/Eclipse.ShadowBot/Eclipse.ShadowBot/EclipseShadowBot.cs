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
using Styx.CommonBot.Coroutines;
using System.IO;
using ArachnidCreations;
using System.Data;
using Eclipse.ShadowBot.Data;
using Eclipse.Comms;

namespace Eclipse.ShadowBot
{
    public class EclipseShadowBot : BotBase
    {

        public static LocalPlayer Me;
        public static WoWPlayer Leader;
        public static bool LootMobs = false;
        public static bool AssistLeader = false;
        public static bool IgnoreAttackers = false;
        public static bool PickUpQuests = false;
        public static int FollowDistance = 8;
        public static bool HealBotMode = false;
        public static bool SkinMobs = false;
        public static string FollowName { get; set; }
        public static bool FollowByName { get; set; }
        public static bool LeaderInRange = false;
        public static ShadowBotSettings settings = null;
        public static bool LeaderMode = false;
        public static bool ShouldBeMounted = false;
        public static WoWPoint navLoc;
        public static bool NavMode = false;
        private ShadowBotConfig _gui;
        private Composite _root;
        public List<Mount.MountWrapper> mounts = new List<Mount.MountWrapper>();
        #region Overrides
        public override string Name
        {
            get { return "Eclipse - ShadowBot 1.0"; }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override Form ConfigurationForm
        {
            get
            {
                if (_gui == null || _gui.IsDisposed) _gui = new ShadowBotConfig();
                return _gui;
            }
        }

        public override void Start()
        {
            if (Me == null) Me = StyxWoW.Me;
            if (EC.DataPath == string.Empty) EC.FindDB();
            if (EC.DataPath != string.Empty)
            {
                EC.LoadSettings();
            }
            Mount.OnMountUp += Mount_OnMountUp;
        }

        public override void Stop()
        {
            EC.Log("Stop Called");
            ServerCommon.StopServer();
            if (CommsCommon.cc != null) CommsCommon.cc.StopServer();
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

            }
            catch (Exception ex)
            {
                Logging.Write(Colors.Red, ex.ToString());
            }
        }
        void Mount_OnMountUp(object sender, MountUpEventArgs e)
        {
            EC.Log("Mounting");
        }
        #endregion

        #region Behaviors
        public override Composite Root
        {
            get
            {
                return _root ?? (_root =
                    new PrioritySelector(
                        new Decorator(r=> NavMode && navLoc.Distance(Me.Location) > Me.InteractRange, new Action(a=>Flightor.MoveTo(navLoc))),
                        new Decorator(r => NavMode && navLoc.Distance(Me.Location) <= Me.InteractRange, new Action(a => NavMode = false)),
                        new Decorator (r=> !NavMode, 
                            new PrioritySelector(
                                new Decorator(r=> !Me.Mounted && Leader.Mounted && Mount.CanMount(), MountBehavior),
                                new Decorator(r=> !Me.Mounted && ShouldBeMounted && Mount.CanMount(), MountBehavior),
                                new Decorator(r => StyxWoW.Me.IsDead || !StyxWoW.Me.IsAlive, EC.CreateDeadBehavior),
                                new Decorator(r => HealBotMode, EC.CreateHealBehavior()),
                                new Decorator(r => Leader == null && FollowByName, 
                                    new Decorator(r=> FindLeader(), 
                                        //this so that the runstatus doesnt return too soon
                                        new PrioritySelector())),
                                new Decorator(r => Leader != null && Me.IsAlive,
                                    new PrioritySelector(
                                        new Decorator(r=>Leader.IsDead && Me.Combat, CreateCombatBehavior()),
                                        new Decorator(r=> Leader.IsDead && !Me.Combat, new Action(a=>Leader = Me.PartyMembers.Where(p=>p.IsAlive).OrderBy(d=>d.Distance).FirstOrDefault())),
                                        new Decorator(r => LootMobs && !Me.BagsFull, new Decorator(r => EC.TargetClosestLootableMob(), EC.CreateLootingBehavior)),
                                        new Decorator(r => Me.FreeBagSlots <= 15 && !Me.Combat && EC.FindVendor(), EC.CreateVendorBehavior),
                                        new Decorator(r => Leader.Distance > FollowDistance,new Action(r => Flightor.MoveTo(Leader.Location))),
                                        new Decorator(r => SkinMobs, new Decorator(r => EC.TargetClosestSkinnableMob(), EC.CreateSkinningBehavior)),
                                        new Decorator(r => PickUpQuests, new Decorator(r => EC.GetQuestGiver(), EC.CreateQuestBehavior)),
                                        new Decorator(r => AssistLeader,  new Decorator(r => !HealBotMode && Leader.Combat && Leader.CurrentTarget != null,
                                            new Sequence(
                                                new Action(a => Leader.CurrentTarget.Target()), CreateCombatBehavior())
                                    )))
                                )
                            )
                        )
                    )
                );
            }
        }

        private Composite MountBehavior
        {
            get{
                return new Sequence(
                    new Action(a => ShouldBeMounted = false), //This is FIRST so that if for some reason mounting fails it doesnt keep trying forever
                    new Action(a => EC.Log("Mounting Up!")),
                    new Action(a => Mount.SummonMount(Mount.FlyingMounts.FirstOrDefault().CreatureSpellId)),
                    //new Action(a=> Mount.GetMountSpell().Cast()),
                    new Action(a => EC.Log("Done Mounting and ready to go!")));
            }
        }
        #endregion

        private bool FindLeader()
        {
            Leader = (WoWPlayer)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Player && n.Name == FollowName).FirstOrDefault();
            if (Leader == null)
            {
                TreeRoot.StatusText = "Waiting for leader to get in range";
                return false;
            }
            else
            {
                EC.Log("Leader Now in range!");
                return true;
            }
        }

        #region Combat Behavior
        public static Composite CreateCombatBehavior()
        {
            return new PrioritySelector(
                new Decorator(r => Me.CurrentTarget == null, new Action(a => Leader.Target())),
                new Decorator(ret => !StyxWoW.Me.Combat,
                            new PrioritySelector(
                        RoutineManager.Current.PreCombatBuffBehavior)),
                new Decorator(ret => Leader.Combat,
                    new LockSelector(
                        RoutineManager.Current.HealBehavior,
                        new Decorator(ret => StyxWoW.Me.GotTarget && !StyxWoW.Me.CurrentTarget.IsFriendly && !StyxWoW.Me.CurrentTarget.IsDead,
                            new PrioritySelector(
                                RoutineManager.Current.CombatBuffBehavior,
                                RoutineManager.Current.CombatBehavior))))
            );

        }
        #endregion

        #region Nested type: LockSelector
        //Taken from raidbot that ships with HB
        private class LockSelector : PrioritySelector
        {
            public LockSelector(params Composite[] children): base(children)
            {
            }

            public override RunStatus Tick(object context)
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    return base.Tick(context);
                }
            }
        }

        #endregion
    }
}
