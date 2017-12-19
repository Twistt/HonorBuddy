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
using Styx.CommonBot.Frames;

namespace Eclipse
{
    public partial class EC
    {
        #region Questing Behaviors
        public static Composite KillAndLootItemBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(r => EC.ActiveQuest.IsCompleted, new Action(a => EC.ActiveQuestOrder.status = QuestStatus.Complete)),
                    //new Decorator(r => EC.LogAndContinue(EC.ActiveQuest.GetObjectives().FirstOrDefault().ToString()), new Action(a => EC.ActiveQuestOrder.status = QuestStatus.Complete)),
                    new Decorator(r => EC.Target.IsAlive, RoutineManager.Current.CombatBehavior),
                    new Decorator(r => EC.Target.IsDead, LootBehavior)
                    );
            }
        }
        public static Composite CollectItemsFromGroundBehavior
        {
            get
            {
                return new PrioritySelector(                    //get item off the ground
                    new Decorator(r => EC.ActiveQuestOrder.objectiveType == QuestObjective.QuestType.CollectItem,
                        new PrioritySelector(
                            new Decorator(r => qoLoc.Distance(Me.Location) > Me.InteractRange, GotoLoc),
                            new Decorator(r => qoLoc.Distance(Me.Location) <= Me.InteractRange,
                                new Sequence(
                                    new Action(a=> Navigator.PlayerMover.MoveStop()),
                                    new WaitContinue(TimeSpan.FromMilliseconds(2000), context => false, new ActionAlwaysSucceed()),
                                    new Action(a => questItem.Interact()),
                                    new Decorator(d=> ActiveQuestOrder.CollectCount == 1, new Action(a=> ActiveQuestOrder.status = QuestStatus.Complete)),
                                    new WaitContinue(TimeSpan.FromMilliseconds(2000), context => false, new ActionAlwaysSucceed())
                                    
                                         
                                )
                            )
                        )
                    )
                );
            }
        }
        public static Composite HangOutWithNPCBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(r => Target == null && TargetClosestQuestNPC(EC.ActiveQuestOrder) && qoLoc.Distance(Me.Location) > Me.InteractRange, GotoLoc),
                    new Decorator(r => qoLoc.Distance(Me.Location) > Me.InteractRange, GotoLoc),
                    new Decorator(r => qoLoc.Distance(Me.Location) <= Me.InteractRange && Target != null, new Sequence(
                        new WaitContinue(TimeSpan.FromSeconds(10), context => false, new ActionAlwaysSucceed()),
                        new Action(a => EC.Log("I touched the butt...")),
                        new Action(a => EC.ActiveQuestOrder.CurrentCollectCount++),
                        new Action(a => EC.ActiveQuestOrder.status = QuestStatus.Complete)
                    ))
                );
            }
        }
        public static Composite ClickNPCBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(r => Target == null && TargetClosestQuestNPC(EC.ActiveQuestOrder) && qoLoc.Distance(Me.Location) > Me.InteractRange, GotoLoc),
                    new Decorator(r => qoLoc.Distance(Me.Location) > Me.InteractRange, GotoLoc),
                    new Decorator(r => qoLoc.Distance(Me.Location) <= Me.InteractRange && Target != null, new Sequence(
                        new Action(a => Target.Interact()),
                        new WaitContinue(TimeSpan.FromSeconds(2), context => false, new ActionAlwaysSucceed()),
                        new Action(a => EC.Log("I touched the butt...")),
                        new Action(a => EC.ActiveQuestOrder.CurrentCollectCount++)
                    ))
                );
            }
        }
        public static Composite MoveToBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(r => EC.ActiveQuestOrder.type == QuestOrder.QOType.MoveTo && qoLoc.Distance(Me.Location) > 20, GotoLoc),
                    new Decorator(r => EC.ActiveQuestOrder.type == QuestOrder.QOType.MoveTo && qoLoc.Distance(Me.Location) <= 20, new Action(a => EC.ActiveQuestOrder.status = QuestStatus.Complete))
                    );
            }
        }
        public static Composite TurninBehavior
        {
            get
            {
                return
                    //new PrioritySelector(
                    //new Decorator(r => EC.ActiveQuestOrder.status == QuestStatus.NotStarted, SetupTurnin()),
                    new PrioritySelector(
                        //new Decorator(r => !Navigator.CanNavigateFully(Me.Location, qoLoc), new Action(a => FindTurnin(ActiveQuestOrder))),
                        new Decorator(r => Me.Location.Distance(qoLoc) > Me.InteractRange, GotoLoc),
                        new Decorator(r => Me.Location.Distance(qoLoc) <= Me.InteractRange && Me.CurrentTarget == null && LogAndContinue("WE are about to set up the turn in..."), SetupTurnin()),
                        new Decorator(r => Me.CurrentTarget == null, new Action(a => FindTurnin(ActiveQuestOrder))),
                        new Decorator(r => Me.CurrentTarget != null,
                            new PrioritySelector(
                                new Decorator(r => LogAndContinue("We are in the turn in baviours"), new PrioritySelector()),
                                new Decorator(r => qoLoc.Distance(Me.Location) > Me.InteractRange, GotoLoc),
                                new Decorator(r => qoLoc.Distance(Me.Location) <= Me.InteractRange && TargetClosestQuestMob(ActiveQuestOrder.TurnInId.ToUInt32()),
                                    new Sequence(
                                        new Action(a => EC.ActiveQuestOrder.status = QuestStatus.InProgress),
                                        new Action(a => Target.Interact()),
                                        new WaitContinue(TimeSpan.FromMilliseconds(2000), context => false, new ActionAlwaysSucceed()),
                                        new Action(a => EC.ActiveQuestOrder.status = QuestStatus.Complete),
                                        new Action(a => EC.ActiveQuest = null),
                                        new Action(a => EC.ActiveQuestOrder = null)
                                    )
                                )
                            )
                        )
                    //)
                );
            }
        }
        public static Composite GetQuestsBehavior
        {
            get
            {
                return
                    new Decorator(r => nearbyQuestGiver(),
                      new Decorator(r => Target != null,
                          new PrioritySelector(
                              new Decorator(r => Target.Distance > Me.InteractRange, GotoLoc),
                              new Decorator(r => Target.Distance <= Me.InteractRange,
                                  new Sequence(
                                      new Action(a => Target.Target()),
                                      new Action(a => Target.Interact()),
                                      new Action(a => HandleQuestDialogue()),
                                      new WaitContinue(TimeSpan.FromMilliseconds(2000), context => false, new ActionAlwaysSucceed())
                                      )
                                ),
                              new Decorator(r => GossipFrame.Instance.IsVisible, new Action(a => GossipFrame.Instance.SelectGossipOption(0)))
                          )
                      )
                  );
            }
        }
        public static Composite LootBehavior
        {
            get
            {
                return new Decorator(r => EC.TargetClosestLootableMob(),
                    new PrioritySelector(
                        new Decorator(r => qoLoc.Distance(Me.Location) > Me.InteractRange, GotoLoc),
                        new Decorator(r => qoLoc.Distance(Me.Location) <= Me.InteractRange, 
                            new Sequence(
                                new Action(a => WoWMovement.MoveStop()),
                                new WaitContinue(TimeSpan.FromMilliseconds(2000), context => false, new ActionAlwaysSucceed()),
                                new Action(a => Target.Interact()),
                                new WaitContinue(TimeSpan.FromMilliseconds(2000), context => false, new ActionAlwaysSucceed())
                                //new Action(a => Target = null)
                            )
                        )
                    )
                );
            }
        }
        #endregion

        #region Questing Helpers
        public static Composite getProperty(string className, string propertyName)
        {
            Type t = Type.GetType(className);
            var instance = Activator.CreateInstance(t);
            return (Composite)instance.GetType().GetProperty(propertyName).GetValue(instance);
        }
        public static bool IsInInventory(uint id)
        {
            if (StyxWoW.Me.Inventory.Items.Where(i => i.Entry == id).FirstOrDefault() != null) return true;
            else return false;
        }
        public static bool LogAndContinue(string msg)
        {
            EC.Log(msg);
            return true;
        }
        public static bool objectiveComplete(QuestOrder questOrder)
        {
            if (questOrder.CollectCount >= questOrder.CurrentCollectCount)
            {
                questOrder.status = QuestStatus.Complete;
                return true;
            }
            if (questOrder.KillCount >= questOrder.CurrentKillCount)
            {
                questOrder.status = QuestStatus.Complete;
                return true;
            }
            return false;
        }
        public static Composite SetupTurnin()
        {
            return new Sequence(
                new Action(a => EC.ActiveQuestOrder.status = QuestStatus.EnRoute),
                new Action(a => GetQuestNPC()),
                new Action(a => EC.Log("Turning in quest."))
            );
        }
        public static void HandleQuestDialogue()
        {
            var i = QuestFrame.Instance;
            EC.Log(string.Format("name: {0}, questid:{1}", i.FrameName, i.CurrentShownQuestId));
            if (i.IsVisible)
            {
                QuestFrame.Instance.ClickContinue();
            }
        }
        public static bool GetNearestItem(QuestOrder questOrder)
        {
            if (questOrder != null)
            {
                uint itemid = 0;
                uint.TryParse(questOrder.ItemId, out itemid);
                var objs = ObjectManager.ObjectList.Where(n => n.Entry == itemid).OrderBy(m => m.Distance).ToList();
                if (objs.Count > 0)
                {
                    questItem = objs.FirstOrDefault();
                    qoLoc = questItem.Location;
                    TreeRoot.StatusText = (string.Format("Found a quest item (id:{0}) {1} meters away", itemid, questItem.Distance));
                    questOrder.status = QuestStatus.InProgress;
                    return true;
                }
                if (objs.Count == 0 && ActiveQuestOrder.Z != 0)
                {
                    qoLoc = ActiveQuestOrder.Location;
                    questOrder.status = QuestStatus.EnRoute;
                    TreeRoot.StatusText = string.Format("Didn't find a quest item ({0}) nearby so we are gonna head to the Quest location {1} meters away.", itemid, qoLoc.Distance(Me.Location));
                    return true;
                }
            }
            EC.Log("Could not find a quest item in this area...");
            return false;
        }
        public static bool TargetClosestQuestNPC(QuestOrder qo)
        {
            var npcs = ObjectManager.ObjectList.Where(n =>
                n.Type == WoWObjectType.Unit
                && !EC.IsUnitBlackListed((WoWUnit)n)
                && ((WoWUnit)n).Entry == uint.Parse(qo.MobId)
            ).OrderBy(m => m.Distance).ToList();
            if (npcs.Count > 0)
            {
                var mob = ((WoWUnit)npcs.FirstOrDefault());
                Target = mob;
                mob.Target();
                qoLoc = mob.Location;
                TreeRoot.StatusText = (string.Format("Found a quest npc {0} meters away.", qoLoc.Distance(Me.Location)));
                return true;
            }
            if (npcs.Count == 0 && ActiveQuestOrder.Z != 0)
            {
                qoLoc = ActiveQuestOrder.Location;
                qo.status = QuestStatus.EnRoute;
                TreeRoot.StatusText = string.Format("Didn't find a npc ({0} {1}) nearby so we are gonna head to the Quest location {2} meters away.", qo.MobId, qo.MobName, qoLoc.Distance(Me.Location));
                return true;
            }
            EC.Log("Could not find a location or an noc to navigate to...");
            return false;

        }
        public static bool TargetClosestQuestMob(uint mobid)
        {
            //Me.ClearTarget();
            var mobs = ObjectManager.ObjectList.Where(n =>
                n.Type == WoWObjectType.Unit
                && !EC.IsUnitBlackListed((WoWUnit)n)
                && !((WoWUnit)n).TaggedByOther
                && ((WoWUnit)n).Entry == mobid
                && ((WoWUnit)n).IsAlive
                && !Blacklist.Contains(((WoWUnit)n), BlacklistFlags.Interact)
            ).OrderBy(m => m.Distance).ToList();
            if (mobs.Count > 0)
            {
                var mob = ((WoWUnit)mobs.FirstOrDefault());
                Target = mob;
                mob.Target();
                qoLoc = mob.Location;
                TreeRoot.StatusText = (string.Format("Found a quest mob {0} meters away.", qoLoc.Distance(Me.Location)));
                return true;
            }
            else
            {
                EC.Log("No Questmob nearby - choosing a different objective... we'll come back to this one.");
                EC.ActiveQuestOrder = EC.ActiveQuestQuestOrders.Where(o=>o != ActiveQuestOrder).OrderByDescending(q => q.Location.Distance(q.Location)).FirstOrDefault();
                return false;
            }
            return false;
        }
        public static bool getNextQuestOrder()
        {
            TreeRoot.StatusText = "Getting next QuestOrder.";
            var quests = Me.QuestLog.GetAllQuests();
            List<QuestOrder> objectives = new List<QuestOrder>();
            foreach (var q in quests)
            {
                objectives.AddRange(EC.QuestOrders.Where(qu => qu.QuestId == q.Id && qu.status != QuestStatus.Complete));
            }
            foreach (var qo in objectives)
            {
                if (qo.Location == null || qo.X == 0)
                {
                    qo.Location = GetQuestLocation(qo);
                    //if (qo.Location != null) DAL.ExecuteSL3Query(ORM.Update(qo, "QuestOrders", "id"));
                }
            }
            //just get the ones where we have turnin information for
            objectives = objectives.Where(q => q.X != 0).ToList();
            var objstoRemove = new List<QuestOrder>();
            foreach (var quest in quests)
            {
                if (quest.IsCompleted)
                {
                    EC.Log(quest.Name + " is complete.");
                    //we now need to remove the objectives that are from quests that are finished and not turned in
                    objstoRemove.AddRange(objectives.Where(o => o.QuestId == quest.Id && o.type != QuestOrder.QOType.TurnIn));
                }
                else
                {
                    //we also need to remove the turn ins for quests that are NOT completed.
                    EC.Log("Removing turn ins for " + quest.Name + " until it is complete.");
                    objstoRemove.AddRange(objectives.Where(o => o.QuestId == quest.Id && o.type == QuestOrder.QOType.TurnIn));
                    //we also need to remove the pickups for quests that we already have.
                    objstoRemove.AddRange(objectives.Where(o => o.QuestId == quest.Id && o.type == QuestOrder.QOType.PickUp));
                }
            }
            foreach (var obj in objstoRemove)
            {
                objectives.Remove(obj);
            }
            EC.Log("There are " + objectives.Count + " objectives to choose from.");
            EC.ActiveQuestQuestOrders = objectives;
            EC.ActiveQuestOrder = objectives.OrderBy(d => d.Location.Distance(Me.Location)).FirstOrDefault();

            if (EC.ActiveQuestOrder != null)
            {
                Me.ClearTarget();
                EC.Log("Changed the active objective to " + EC.ActiveQuestOrder.type.ToString());
                qoLoc = EC.ActiveQuestOrder.Location;

                    EC.ActiveQuest = quests.Where(q => q.Id == EC.ActiveQuestOrder.QuestId).First();
                    EC.ActiveQuestOrder.status = QuestStatus.EnRoute;
                    return true;

                //if (!Navigator.CanNavigateFully(Me.Location, qoLoc))
                //{
                //    qoLoc = GetQuestLocation(EC.ActiveQuestOrder);
                //    EC.ActiveQuest = quests.Where(q => q.Id == EC.ActiveQuestOrder.QuestId).First();
                //    EC.ActiveQuestOrder.status = QuestStatus.EnRoute;
                //    return true;
                //}
            }
            EC.Log("No quests where the location is known. =(");
            TreeRoot.Stop();
            return false;

        }
        public static WoWPoint GetQuestLocation(QuestOrder qo)
        {
            if (qo.type == QuestOrder.QOType.Objective)
            {
                return getItemLocation(qo);
            }
            if (qo.type == QuestOrder.QOType.TurnIn)
            {
                return FindTurnin(qo);
            }
            return qo.Location;
        }
        public static WoWPoint getItemLocation(QuestOrder aqo)
        {
            if (aqo.X == 0)
            {
                uint id = 0;
                uint.TryParse(aqo.ItemId, out id);

                if (id != 0)
                {
                    var item = ObjectManager.ObjectList.Where(o => o.Entry == id).FirstOrDefault();
                    if (item != null)
                    {
                        return item.Location;
                    }
                }
                if (aqo.ItemName != string.Empty)
                {
                    var item = ObjectManager.ObjectList.Where(o => o.Name == aqo.ItemName).FirstOrDefault();
                    if (item != null)
                    {
                        return item.Location;
                    }
                }
            }
            else
            {
                return aqo.Location;
            }
            EC.Log(string.Format("We didnt find ANY location information for this quest item {0}, {1}.", aqo.ItemName, aqo.ItemId));
            return aqo.Location;

        }
        public static  WoWPoint FindTurnin(QuestOrder aqo)
        {
            if (aqo.X == 0 )
            {
                EC.Log(string.Format("QO location is empty for turnin -> finding NPC from DB by id({0})", aqo.TurnInId));
                uint questgiver = 0;
                uint.TryParse(aqo.TurnInId, out questgiver);
                if (questgiver != 0)
                {
                    Target = getLocalMobById(questgiver);
                    if (Target != null)
                    {
                        EC.Log("Found quest giver nearby. Heading to:" + Target.Name);
                        Target.Target();
                        aqo.Location = Target.Location;
                        //ToDo: Update the DB with found location.
                        return Target.Location;
                    }
                    else
                    {
                        var npc = EC.NPCs.Where(n => n.Entry == questgiver).FirstOrDefault();
                        if (npc != null)
                        {
                            EC.Log("Found an npc for turn in by id! " + npc.Name);
                            aqo.Location = new WoWPoint(npc.X, npc.Y, npc.Z);
                            qoLoc = new WoWPoint(npc.X, npc.Y, npc.Z);
                            return new WoWPoint(npc.X, npc.Y, npc.Z);
                        }
                        if (npc == null)
                        {
                            GetQuestNPC();
                            EC.Log("Last resort...");
                            return qoLoc;
                        }
                    }
                }
                if (questgiver == 0)
                {
                    if (aqo.TurnInName != null && aqo.TurnInName != string.Empty)
                    {
                        var target = getLocalMobByName(aqo.TurnInName);
                        if (target != null)
                        {
                            EC.Log("Found an npc for turnin by name!");
                            aqo.Location = target.Location;
                            return target.Location;
                        }
                    }
                }
                EC.Log("We didnt find ANY turnin information for this quest. =(");
                return aqo.Location;
            }
            else { return aqo.Location; }

        }
        public static WoWUnit getLocalMobByName(string name)
        {
            return (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && n.Name == name).OrderBy(m => m.Distance).FirstOrDefault();
        }
        public static WoWUnit getLocalMobById(uint id)
        {
            
            var unit = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Entry == id && !TemporaryIgnoreList.Contains(n.Guid)).OrderBy(m => m.Distance).FirstOrDefault();
            if (unit != null) unit.Target();
            else EC.Log("Could not find a target");
            return unit;
        }
        public static WoWPoint getMobLocation(QuestOrder aqo)
        {
                uint id = 0;
                uint.TryParse(aqo.MobId, out id);
                var mob = EC.MOBs.Where(n => n.Entry == id).FirstOrDefault();
                if (mob == null)
                {
                    var localmob = getLocalMobById(uint.Parse(aqo.MobId));
                    if (localmob != null)
                    {
                        EC.Log("Found a mob nearby (by id) thats part of the objective.");
                        Target = localmob;
                        qoLoc = localmob.Location;
                        localmob.Target();
                        EC.ActiveQuestOrder.Location = localmob.Location;
                        EC.ActiveQuestOrder.status = QuestStatus.EnRoute;
                        return localmob.Location;
                    }
                    if (localmob == null && aqo.MobName != string.Empty)
                    {
                        localmob = getLocalMobByName(aqo.MobName);
                        if (localmob != null)
                        {
                            EC.Log("Found a mob nearby (by name) thats part of the objective.");
                            Target = localmob;
                            qoLoc = localmob.Location;
                            localmob.Target();
                            EC.ActiveQuestOrder.status = QuestStatus.EnRoute;
                            return localmob.Location;
                        }
                    }
                }
                if (mob != null)
                {
                    EC.Log("Found a mob in the database thats part of this objective.");
                    EC.ActiveQuestOrder.status = QuestStatus.EnRoute;
                    WoWUnit m = (WoWUnit)ObjectManager.ObjectList.Where(o => o.Entry == mob.Entry).FirstOrDefault();
                    if (m == null)
                    {
                        Target = m;
                        m.Target();
                        qoLoc = m.Location;
                        return qoLoc;
                    }
                    qoLoc = new WoWPoint(mob.X, mob.Y, mob.Z);
                    return new WoWPoint(mob.X, mob.Y, mob.Z);
                }
            return aqo.Location;
        }
        public static bool updateQuestOrders()
        {
            var questLog = Me.QuestLog.GetAllQuests();
            if (questLog.Count > 0)
            {
                var toRemoveList = new List<QuestOrder>();
                //Add quest orders for quests we have.
                foreach (var qu in questLog)
                {
                    //Add the quest orders to the active QOs that arent already in there.
                    EC.ActiveQuestQuestOrders.AddRange(EC.QuestOrders.Where(c => c.QuestId == qu.Id && !EC.ActiveQuestQuestOrders.Contains(c)));
                    //Remove the objectives from teh completed quests since we dont have to worry about them.
                    if (qu.IsCompleted) toRemoveList.AddRange(EC.ActiveQuestQuestOrders.Where(q => q.QuestId == qu.Id && q.type == QuestOrder.QOType.Objective).ToList());
                }
                //Remove teh quest orders for quests we no longer have.
                foreach (QuestOrder qo in EC.ActiveQuestQuestOrders)
                {
                    if (qo.type == QuestOrder.QOType.TurnIn) qo.Location = FindTurnin(qo);
                    if (qo.type == QuestOrder.QOType.Objective && qo.objectiveType == QuestObjective.QuestType.KillMob) qo.Location = FindTurnin(qo);
                    if (questLog.Where(q => q.Id == qo.QuestId).Count() == 0) toRemoveList.Add(qo);
                }
                foreach (var qo in toRemoveList) EC.ActiveQuestQuestOrders.Remove(qo);
                var activeQOs = EC.ActiveQuestQuestOrders.Where(q => q.QuestId == EC.ActiveQuest.Id);
                EC.ActiveQuestOrder = EC.ActiveQuestQuestOrders.Where(q => q.X != 0).OrderBy(q => q.Location.Distance(StyxWoW.Me.Location)).FirstOrDefault();
                EC.ActiveQuest = questLog.Where(q => q.Id == EC.ActiveQuestOrder.QuestId).FirstOrDefault();
                EC.Log("Finished Updating Quests.");
            }
            return true;
        }
        public static bool GetQuestNPC()
        {
            var questgivers = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n) && ((WoWUnit)n).IsFriendly && n.Entry == uint.Parse(EC.ActiveQuestOrder.TurnInId)).ToList().OrderBy(m => m.Distance).ToList();
            if (questgivers.Count == 0) questgivers = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit).ToList().Where(u => u.QuestGiverStatus == QuestGiverStatus.TurnIn).OrderBy(m => m.Distance).ToList();
            if (questgivers.Count == 0) questgivers = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n) && ((WoWUnit)n).IsFriendly && ((WoWUnit)n).HasQuestCursor).ToList().OrderBy(m => m.Distance).ToList();
            if (questgivers.Count == 0) questgivers = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit).ToList().Where(u=>u.QuestGiverStatus == QuestGiverStatus.TurnIn).OrderBy(m => m.Distance).ToList();
            if (questgivers.Count == 0)
            {
                var npc = NPCs.Where(n => n.Entry == uint.Parse(EC.ActiveQuestOrder.TurnInId)).FirstOrDefault();
                if (npc != null) qoLoc = new WoWPoint(npc.X, npc.Y, npc.Z);
                return true;
            }
            if (questgivers.Count > 0)
            {
                var npc = (WoWUnit)questgivers.OrderBy(m => m.Distance).FirstOrDefault();
                EC.Log("Found a quest npc: " + npc.Name);
                npc.Target();
                Target = npc;
                qoLoc = Target.Location;
                return true;
            }
            EC.Log("Could not find Quest NPC." + EC.ActiveQuestOrder.TurnInId);
            return false;
        }
        public static bool nearbyQuestGiver()
        {
            var questgivers = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n) && ((WoWUnit)n).IsFriendly).ToList().Where(n =>
                ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.Available
                //|| ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.TurnIn
                //|| ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.TurnInRepeatable
            ).OrderBy(m => m.Distance).ToList();
            if (questgivers.Count > 0)
            {
                var npc = (WoWUnit)questgivers.OrderBy(m => m.Distance).FirstOrDefault();
                EC.Log("Found a quest npc: " + npc.Name);
                npc.Target();
                Target = npc;
                qoLoc = Target.Location;
                return true;
            }
            //EC.Log("No q givers around.");
            return false;
        }
        public static Composite GotoTargetLoc
        {
            get
            {

                return new PrioritySelector(
                    new Decorator(d=>Me.CurrentTarget != null,
                        new Decorator(r => Me.CurrentTarget.Location.Distance(Me.Location) > Me.InteractRange,
                            new Sequence(
                                new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location)),
                                new Action(r => TreeRoot.StatusText = "OMW to location..." + qoLoc.ToString())
                            ))));

            }
        }
        public static Composite GotoLoc
        {
            get
            {
                
                return new PrioritySelector(
                    new Decorator(r => qoLoc.Distance(Me.Location) > Me.InteractRange,
                        new Sequence(
                            new Action(r => Flightor.MoveTo(qoLoc)),
                            new Action(r=> TreeRoot.StatusText = "OMW to location..." + qoLoc.ToString())
                        )));

            }
        }
        #endregion
    }
}
