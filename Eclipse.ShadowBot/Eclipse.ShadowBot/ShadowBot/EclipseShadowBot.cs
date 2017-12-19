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
        public static int FreeBagSlots = 15;
        public static WoWServer Server { get; set; }
        public static WoWClient Client { get; set; }
        private ShadowBotConfig _gui;
        private Composite _root;
        public List<Mount.MountWrapper> mounts = new List<Mount.MountWrapper>();
        #region Overrides
        public override string Name
        {
            get { return "Eclipse - ShadowBot 1.3"; }
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
            if (EC.Me == null) EC.Me = StyxWoW.Me;
            if (EC.DataPath == string.Empty) EC.FindDB();
            if (EC.DataPath != string.Empty)
            {
                EC.LoadSettings();
            }
            Mount.OnMountUp += Mount_OnMountUp;
            UIHooks.AttachUIEvents();
        }

        public override void Stop()
        {
            EC.Log("Stop Called");
            WoWServer.RunServer = false; //This is redundant - but leaving the thread running is worse.
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
                        new Decorator(r => NavMode && navLoc.Distance(Me.Location) > Me.InteractRange, new Action(a => Flightor.MoveTo(navLoc))),
                        new Decorator(r => NavMode && navLoc.Distance(Me.Location) <= Me.InteractRange, new Action(a => NavMode = false)),
                        new Decorator(r => !NavMode,
                            new PrioritySelector(
                                new Decorator(r => StyxWoW.Me.IsDead || !StyxWoW.Me.IsAlive, EC.CreateDeadBehavior),
                                //new Decorator(r => !FindLeader(), new Action(a => CommsCommon.cc.SendMessage(new WowMessage() { Type="GetChar", Name=FollowName, Port= CommsCommon.cc.PortNumber }))),
                                new Decorator(r => Leader == null && FollowByName,
                                    new Decorator(r => FindLeader(),
                    //this so that the runstatus doesnt return too soon
                                        new PrioritySelector())),
                                new Decorator(r => Leader != null && Me.IsAlive,
                                    new PrioritySelector(
                                        new Decorator(r => Leader.Distance > FollowDistance, new Action(r => Flightor.MoveTo(Leader.Location))),
                                        new Decorator(r => HealBotMode && MountCheck(), EC.CreateHealBehavior()),
                                        new Decorator(r => !Me.Mounted && Leader.Mounted && Mount.CanMount() && !Me.IsCasting, MountBehavior),
                                        new Decorator(r => !Me.Mounted && ShouldBeMounted && Mount.CanMount() && !Me.IsCasting, MountBehavior),
                                        new Decorator(r => Leader.IsDead && Me.Combat && MountCheck(), CreateCombatBehavior()),
                                        new Decorator(r => Leader.IsDead && !Me.Combat, new Action(a => Leader = Me.PartyMembers.Where(p => p.IsAlive).OrderBy(d => d.Distance).FirstOrDefault())),
                                        new Decorator(r => LootMobs && !Me.BagsFull, new Decorator(r => EC.TargetClosestLootableMob(), EC.CreateLootingBehavior)),
                                        new Decorator(r => Me.FreeBagSlots <= FreeBagSlots && FreeBagSlots !=0 && !Me.Combat && EC.FindVendor(), EC.CreateVendorBehavior),
                                        new Decorator(r => SkinMobs, new Decorator(r => EC.TargetClosestSkinnableMob(), EC.CreateSkinningBehavior)),
                                        new Decorator(r => PickUpQuests, 
                                            new Decorator(r => EC.GetQuestGiver(), EC.CreateQuestBehavior)),
                                        new Decorator(r => AssistLeader, 
                                            new Decorator(r => !HealBotMode && Leader.Combat && Leader.CurrentTarget != null && MountCheck(),
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
                    new Decorator(d=> Leader.IsFlying, new Action(a =>  Mount.SummonMount(Styx.Helpers.CharacterSettings.Instance.GroundMountSpellId))),
                    new Decorator(d=> !Leader.IsFlying, new Action(a => Mount.SummonMount(Styx.Helpers.CharacterSettings.Instance.GroundMountSpellId))),
                    //new Action(a=> Mount.GetMountSpell().Cast()),
                    new WaitContinue(4, new Action(a => EC.Log("Done Mounting and ready to go!")))
                    );
            }
        }
        #endregion
        private bool MountCheck()
        {
            if (!Leader.Mounted && Me.Mounted)
            {
                Flightor.MountHelper.Dismount();
            }
            return true;
        }
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
