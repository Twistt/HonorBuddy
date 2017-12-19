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
using Eclipse;
using Styx;
using Eclipse.Models;
using Styx.CommonBot.Frames;
using CommonBehaviors.Actions;
using System;
using System.Windows.Media;
using Eclipse.Bots.QuestBot;
using System.Windows.Forms;
using Eclipse.MultiBot.Core;

namespace Eclipse
{
	 public class EclipseQuestBot: BotBase
    {
        #region Overrides of BotBase
        public override bool RequiresProfile { get { return false; } }
        public override string Name
        {
            get { return "Eclipse - QuestBot Ver 0.1"; }
        }
        private EclipseConfigForm _gui;
        public override Form ConfigurationForm
        {
            get
            {
                if (_gui == null || _gui.IsDisposed) _gui = new EclipseConfigForm();
                return _gui;
            }
        }
        private Composite _root;
        public override Composite Root
        {
            get
            {
                return _root ?? (_root =
                    new PrioritySelector(
                        CreateQuestBehavior()
                        )
                    );
            }
        }

        public static  Composite CreateQuestBehavior()
        {
            return new PrioritySelector(
                EC.LootBehavior,
                EC.GetQuestsBehavior,
                new Decorator(r => EC.ActiveQuestOrder == null, new Action(a => EC.getNextQuestOrder())),
                new Decorator(r => EC.ActiveQuestOrder != null, DoQuestOrder())
            );
        }
        public static Composite DoQuestOrder()
        {
            return new PrioritySelector(
                //new Decorator(d => EC.ActiveQuestOrder == null, new Action(a => TreeRoot.StatusText = "Its null... wtf")),
                //new Decorator(d => EC.ActiveQuestOrder != null, new Action(a => TreeRoot.StatusText = string.Format("Active Object is {0} POI is {1}",EC.ActiveQuestOrder.objectiveType.ToString(), EC.ActiveQuestOrder.MobId))),
                //Turn in logic for a quest that is complete
                new Decorator(r => EC.ActiveQuestOrder.status == QuestStatus.Complete, 
                    new Action(a => EC.getNextQuestOrder())),
                new Decorator(r => EC.ActiveQuestOrder.type == QuestOrder.QOType.PickUp, EC.CreateQuestPickupBehavior),
                new Decorator(r => EC.ActiveQuestOrder.type == QuestOrder.QOType.TurnIn, EC.TurninBehavior),
                new Decorator(r => EC.ActiveQuestOrder.type == QuestOrder.QOType.MoveTo, EC.MoveToBehavior),
                new Decorator(r => EC.ActiveQuestOrder.type == QuestOrder.QOType.Objective, 
                    new PrioritySelector(
                        new Decorator(r => EC.ActiveQuestOrder.objectiveType == QuestObjective.QuestType.KillMob, EC.CreatHuntingBehavior),
                        new Decorator(r => EC.ActiveQuestOrder.objectiveType == QuestObjective.QuestType.CollectItem && EC.GetNearestItem(EC.ActiveQuestOrder), EC.CollectItemsFromGroundBehavior),
                        new Decorator(r => EC.ActiveQuestOrder.objectiveType == QuestObjective.QuestType.KillAndLootItem && EC.TargetClosestQuestMob(uint.Parse(EC.ActiveQuestOrder.MobId)), EC.KillAndLootItemBehavior),
                        new Decorator(r => EC.ActiveQuestOrder.objectiveType == QuestObjective.QuestType.ClickNpc, EC.ClickNPCBehavior),
                        new Decorator(r => EC.ActiveQuestOrder.objectiveType == QuestObjective.QuestType.HangOutWithNPC, EC.HangOutWithNPCBehavior),
                        new Decorator(r => EC.ActiveQuestOrder.objectiveType == QuestObjective.QuestType.UseItemOnMob, EC.UseItemOnMobBehavior),
                        new Decorator(r => EC.ActiveQuestOrder.objectiveType == QuestObjective.QuestType.CastSpellOnMob, EC.CastSpellOnMob)
                        //new DecoratorContinue(d => EC.ActiveQuestOrder.status != null, new Action(a => TreeRoot.StatusText = EC.ActiveQuestOrder.objectiveType.ToString() + EC.ActiveQuestOrder.MobId))
                        ))
                //this will be the reflection bit that finds classes with names and invokes them.
               //new Decorator(r => EC.ActiveQuestOrder.type == QuestOrder.QOType.CustomBehavior && EC.ActiveQuestOrder.BehaviorName != null, EC.getProperty(EC.ActiveQuestOrder.BahaviorClassName, EC.ActiveQuestOrder.BehaviorName))
               
            );
        }
        public override void Initialize()
        {
            BotEvents.Player.OnMapChanged += Player_OnMapChanged;
        }
        private void Player_OnMapChanged(BotEvents.Player.MapChangedEventArgs args)
        {
            _root = null;
        }
        public override void Pulse()
        {

        }
        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All & ~(PulseFlags.Looting | PulseFlags.CharacterManager); }
        }

        private bool _oldLogoutForInactivity;
        public override void Start()
        {
            _oldLogoutForInactivity = GlobalSettings.Instance.LogoutForInactivity;
            GlobalSettings.Instance.LogoutForInactivity = false;
            EC.Me = StyxWoW.Me;
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
            UIHooks.AttachQuestEvents();
        }
        public override void Stop()
        {
            GlobalSettings.Instance.LogoutForInactivity = _oldLogoutForInactivity;
            UIHooks.DetatchQuestEvents();
        }

        #endregion
    }

    
}
