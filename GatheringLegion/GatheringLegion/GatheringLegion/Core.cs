using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.EclipsePlugins.Models;
using Eclipse.WoWDatabase.Models;
using GatheringLegion.Models;
using Styx;
using Styx.Common;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;

namespace Eclipse.WoWDatabase
{
    public class Core
    {
        #region Variables
        public static string DataPath = "EclipseWoWDB.edb";
        public static LocalPlayer Me { get; set; }
        public static WoWUnit Target { get; set; }
        public static bool AddTargetOnly = true;
        public static float DistanceToPoll { get; set; }
        public static bool init { get; private set; }
        public static string ilog = string.Empty;
        private static WoWUnit lastTarget = null;
        private static int inventoryCount = 0;
        private static int questCount = 0;
        
        public static bool NinjaSkin = false;
        public static bool PassiveMode = true;
        public static bool ForceNav = false;
        public static Location ForceNavLocation = null;
        public static bool GatherMode = false;
        public static bool KillThese = false;
        public static bool BagsFull = false;
        public static List<Styx.WoWInternals.WoWGuid> RecentlyHarvested = new List<Styx.WoWInternals.WoWGuid>();
        #endregion

        #region Caches
        public static List<Quest> Quests = new List<Quest>();
        public static List<NPC> NPCs = new List<NPC>();
        public static List<Mob> MOBs = new List<Mob>();
        public static List<Location> Locations = new List<Location>();
        public static List<Faction> Factions = new List<Faction>();
        public static List<Location> RecentlyVisitedLocations = new List<Location>();
        public static List<Location> FavoritePlaces = new List<Location>();
        public static List<Mob> KillList = new List<Mob>();
        public static List<Mob> SkinList = new List<Mob>();
        public static List<Mob> IgnoreList = new List<Mob>();
        public static List<Location> KillLocations = new List<Location>();
        public static List<Zone> Zones = new List<Zone>();
        public static List<EclipseObject> Objects = new List<EclipseObject>();
        private static List<Quest> CurrentQuests = new List<Quest>();
        #endregion

        #region Cache Helpers
        internal static void AddNpc(WoWUnit Target)
        {
            if (NPCs.Where(m => m.Entry == Target.Entry).Count() == 0)
            {
                var npc = new NPC { Name = Target.Name};
                npc.isVendor = Target.IsVendor;    
                npc.Entry = Target.Entry;
                npc.Zone = StyxWoW.Me.ZoneId;
                npc.FactionId = Target.FactionId;
                npc.isQuestGiver = Target.IsQuestGiver;
                npc.X = Target.X;
                npc.Y = Target.Y;
                npc.Z=Target.Z;
                npc.Level=Target.Level;
                DAL.ExecuteSL3Query(DAL.Insert(npc, "NPC", "", false, DAL.getTableStructure("NPC")));
                NPCs.Add(npc);
                iLog(string.Format("Added NPC {0} ({1})", npc.Name, npc.Entry));
            }
        }
        internal static void AddMob(WoWUnit Target)
        {
            var _mob = MOBs.Where(m => m.Entry == Target.Entry).FirstOrDefault();
            if (_mob == null && Target != null)
            {
                try
                {
                    var mob = new Mob { Name = Target.Name};
                    mob.isElite = Target.Classification == WoWUnitClassificationType.Elite;
                    mob.isRare = Target.Classification == WoWUnitClassificationType.Rare;
                    mob.Entry =Target.Entry;
                    mob.Zone = StyxWoW.Me.ZoneId;
                    if (Target.FactionId != 0) mob.FactionId = Target.FactionId;
                    mob.isSkinnable = Target.Skinnable;
                    mob.Level = Target.Level;

                    var sql = DAL.Insert(mob, "MOB", "", false, DAL.getTableStructure("MOB"));
                    DAL.ExecuteSL3Query(sql);
                    MOBs.Add(mob);
                    iLog(string.Format("Added Mob {0} ({1})", mob.Name, mob.Entry));
                }
                catch (Exception err)
                {
                    iLog(string.Format("Could not DB mob {0} ({1}) probably a serialization thing...\r\n{2}", Target.Name, Target.Entry, err.ToString()));
                }
            }
            else
            {
                log("Mob is already in the database.");
                if (Target.Skinnable || Target.CanSkin)
                {
                    if (!_mob.isSkinnable)
                    {
                        log("Mob is not marked skinnable in the db but it is - updating.");
                        if (GatherMode && !KillList.Contains(_mob))
                        {
                            log("Since we are in skinmode and this mob is skinnable we are adding it to the Kill List");
                            KillList.Add(_mob);
                        }
                        _mob.isSkinnable = true;
                        MOBs.Where(m => m.Entry == Target.Entry).FirstOrDefault().isSkinnable = true; //make sure its updated in teh list not just this instance.
                        UpdateMob(_mob);
                    }
                }
            }
        }
        internal static void UpdateMob(Mob mob)
        {
            var sql = ORM.Update(mob, "MOB", "", DAL.getTableStructure("MOB"));
            DAL.ExecuteSL3Query(sql);
        }
        internal static void AddUnit(WoWUnit unit)
        {
            if (!unit.IsPlayer)
            {
                if (unit.IsFriendly) Core.AddNpc(unit);
                else Core.AddMob(unit);
            }
        }
        #endregion

        #region  Core Methods
        internal static void Initialize()
        {
            iLog("Running init...");
            FindDB();

            EC.Settings.LoadSettings();

            DataTable dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table' and Name='Quests';");
            if (dt == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(new Quest(), "Quests"));
            }

            dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table' and Name='MOB';");
            if (dt == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(new Mob(), "MOB"));
            }

            dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table' and Name='NPC';");
            if (dt == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(new Mob(), "NPC"));
            }

            dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table' and Name='Locations';");
            if (dt == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(Core.Target, "Locations"));
            }

            DataLoader.loadMobs();
            DataLoader.loadNPCs();
            DataLoader.loadQuests();
            DataLoader.loadLocations();
            DataLoader.loadFavorites();
            DataLoader.loadObjects();
            DataLoader.loadZones();
            init = true;
            iLog("Finished init.");
        }
        internal static void FindDB()
        {
            var path = Application.StartupPath;
            if (!File.Exists(DataPath))
            {
                var results = Directory.GetFiles(path, "*.edb", SearchOption.AllDirectories).ToList();
                if (results.Count > 0)
                {
                    DataPath = results[0];
                    log(string.Format("--------------------------------Found {0}------------------------------------", results[0]));
                }
            }
        }
        public static bool Pulse()
        {
            if (init == true)
            {
                try
                {
                    if (Me == null) Me = StyxWoW.Me;
                    if (Zones.Where(z=>z.Id == StyxWoW.Me.ZoneId).FirstOrDefault() == null)
                    {
                        

                        var newzone = new Zone { Id= StyxWoW.Me.ZoneId, Name= StyxWoW.Me.ZoneText };
                        //DAL.ExecuteSL3Query(ORM.generateCreateSQL(newzone, "Zones"));
                        DAL.ExecuteSL3Query(ORM.Insert(newzone, "Zones", "", false, DAL.getTableStructure("Zones")));
                        Zones.Add(newzone);
                    }
                    if (Zones.Where(z => z.Id == StyxWoW.Me.SubZoneId).FirstOrDefault() == null)
                    {
                        var newzone = new Zone { Id = StyxWoW.Me.SubZoneId, Name = StyxWoW.Me.SubZoneText, ParentZone= StyxWoW.Me.ZoneId };
                        //DAL.ExecuteSL3Query(ORM.generateCreateSQL(newzone, "Zones"));
                        DAL.ExecuteSL3Query(ORM.Insert(newzone, "Zones", "", false, DAL.getTableStructure("Zones")));
                        Zones.Add(newzone);
                    }
                    
                    //ProccessQuests();
                    //InventoryChangeCheck();
                    var rareunits = Styx.WoWInternals.ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(n => n.Classification == WoWUnitClassificationType.Rare).OrderBy(m => m.Distance).ToList();
                    foreach (var obj in rareunits)
                    {
                        var _mob = MOBs.Where(m => m.Entry == obj.Entry).FirstOrDefault();
                        if (_mob == null)
                        {
                            Core.AddMob(obj);
                            EC.log(string.Format("Found a new rare mob ", obj.Name));
                        }
                    }
                    var objects = Styx.WoWInternals.ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.GameObject).OrderBy(m => m.Distance).ToList();
                    foreach (var obj in objects)
                    {

                        var o = (WoWGameObject)obj;
                        if (o.SubType == WoWGameObjectType.GatheringNode)
                        {
                            if (Core.Objects.Where(c=>c.Id == o.Entry).Count() == 0)
                            {
                                var newobj = new EclipseObject { Entry=o.Entry, Name=o.Name, OType= EclipseGeneric.ObjectType.Object, Id=o.Entry, X=o.X, Y=o.Y, Z=o.Z };
                                Objects.Add(newobj);
                                DAL.ExecuteSL3Query(ORM.Insert(o, "Objects", "", false, DAL.getTableStructure("Objects")));
                                EC.log(string.Format("Found a new type of gathering node", obj.Name));
                            }

                            var loc = new Location { Entry = o.Entry, X = o.X, Y = o.Y, Z = o.Z, Zone = StyxWoW.Me.ZoneId };
                            var savedloc = Core.Locations.Where(l => l.Z == loc.Z && loc.Y == l.Y && loc.Z == l.Z && loc.Entry == l.Entry && loc.Zone == l.Zone).FirstOrDefault();
                            if (savedloc == null)
                            {
                                //ToDo: see if these are close to another hotspot (with 20 yards) and dont add them - probably best on a new thread...
                                Core.log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, o.Name));
                                DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                                Core.Locations.Add(loc);
                            }
                        }
                        if (o.SubType == WoWGameObjectType.Chest)
                        {
                            if (Core.Objects.Where(c => c.Id == o.Entry).Count() == 0)
                            {
                                var newobj = new EclipseObject { Entry = o.Entry, Name = o.Name, OType = EclipseGeneric.ObjectType.Object, Id = o.Entry, X = o.X, Y = o.Y, Z = o.Z };
                                Objects.Add(newobj);
                                DAL.ExecuteSL3Query(ORM.Insert(o, "Objects", "", false, DAL.getTableStructure("Objects")));
                                EC.log(string.Format("Found a new chest location", obj.Name));
                            }

                            var loc = new Location { Entry = o.Entry, X = o.X, Y = o.Y, Z = o.Z, Zone = StyxWoW.Me.ZoneId };
                            var savedloc = Core.Locations.Where(l => l.Z == loc.Z && loc.Y == l.Y && loc.Z == l.Z && loc.Entry == l.Entry && loc.Zone == l.Zone).FirstOrDefault();
                            if (savedloc == null)
                            {
                                //ToDo: see if these are close to another hotspot (with 20 yards) and dont add them - probably best on a new thread...
                                Core.log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, o.Name));
                                DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                                Core.Locations.Add(loc);
                            }
                        }

                    }

                    if (StyxWoW.Me.CurrentTarget != null)
                    {
                        if (lastTarget != StyxWoW.Me.CurrentTarget || Target == null)
                        {
                            Target = StyxWoW.Me.CurrentTarget;
                            iLog(string.Format("Target changed to {0} ({1})", Target.Name, Target.Entry));
                            ProccessUnits();
                            ProccessFactions();
                            lastTarget = StyxWoW.Me.CurrentTarget;
                        }
                    }
                    else lastTarget = null;
                }
                catch (Exception err)
                {
                    log("Error at root level: " + err.ToString());
                    return false;
                }
            }
            else
            {
                log("Not Initialized - Calling now:");
                Initialize();
            }
            return true;
        }
        #endregion

        #region HandlerMethods
        public static void ProccessUnits()
        {

            if (AddTargetOnly == true) AddUnit(Target);
            else AoeDataMine(DistanceToPoll);

            if (!Target.IsPlayer)
            {
                var loc = new Location { Entry = Target.Entry, X = Target.X, Y = Target.Y, Z = Target.Z, Zone = StyxWoW.Me.ZoneId };
                var savedloc = Locations.Where(l => l.Z == loc.Z && loc.Y == l.Y && loc.Z == l.Z && loc.Entry == l.Entry && loc.Zone == l.Zone).FirstOrDefault();
                if (savedloc == null)
                {
                    //ToDo: see if these are close to another hotspot (with 20 yards) and dont add them - probably best on a new thread...
                    log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, Target.Name));
                    DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                    Locations.Add(loc);
                }

                var mob = MOBs.Where(e => e.Entry == Target.Entry).FirstOrDefault();
                var npc = NPCs.Where(e => e.Entry == Target.Entry).FirstOrDefault();
                if (npc != null)
                {
                    var needsupdate = false;
                    if (npc.Level == 0) needsupdate = true;
                    if (!npc.isVendor && Target.IsVendor) needsupdate = true;

                    if (needsupdate)
                    {
                        Core.log("Updating npc.");
                        NPCs.Remove(npc);
                        DAL.ExecuteSL3Query(ORM.Update(npc, "NPC", "", DAL.getTableStructure("NPC")));
                        npc.Level = Target.Level;
                        npc.isVendor = Target.IsVendor;
                        NPCs.Add(npc);
                    }
                }
                if (mob != null)
                {
                    if (mob.Level == 0)
                    {
                        Core.UpdateMob(mob);
                        MOBs.Where(e => e.Entry == Target.Entry).FirstOrDefault().Level = Target.Level;
                    }
                    if (Target.IsDead && !Target.Lootable && Target.Skinnable)
                    {
                        iLog("Found skinnable mob - updating mob entry.");
                        MOBs.Remove(mob);
                        mob.isSkinnable = true;
                        DAL.ExecuteSL3Query(ORM.Update(mob, "MOB", "", DAL.getTableStructure("MOB")));
                        MOBs.Add(mob);
                    }
                }
            }
        }
        public static void ProccessQuests()
        {
            var quests = StyxWoW.Me.QuestLog.GetAllQuests().ToList();
            var temp = quests.FirstOrDefault();
            temp.Objectives.FirstOrDefault();
            var qc = quests.Count;
            if (qc > CurrentQuests.Count) HandleNewQuest(quests);
            if (qc < CurrentQuests.Count) HandleOldQuest(quests);
        }
        public static void HandleOldQuest(List<Styx.WoWInternals.PlayerQuest> quests)
        {
            Me = StyxWoW.Me;
            Quest questToRemove = null;
            foreach (var q in CurrentQuests) //Look through quests you are currently on
            {
                questToRemove = q;
                var hbquest = quests.Where(qu => qu.Id == q.Id).FirstOrDefault();
                if (hbquest == null) //see which quest is no longer in your questlog 
                {
                    if (Me.CurrentTarget != null)
                    {
                        if (Me.CurrentTarget.IsFriendly && !Me.CurrentTarget.IsPlayer) q.TurnInTo = Me.CurrentTarget.Entry;
                        var masterQuest = Quests.Where(mq => mq.Id == q.Id).FirstOrDefault(); //get this quest from the master list
                        if (masterQuest != null) //unless its not in the master list.
                        {
                            if (masterQuest.TurnInTo != q.TurnInTo) //if it has somethign else for the turn in (0 or null) 
                            {
                                iLog(string.Format("Added New Quest TurnIn {0} ({1}) Recieved From: {2}", q.Name, q.Id, q.TurnInTo)); //log the cahnge
                                Quests.Remove(masterQuest); //remove it from teh master list.
                                masterQuest.TurnInTo = q.TurnInTo;
                                Quests.Add(masterQuest); //add this quest to the master list since the turn into has been updated
                                if (masterQuest.Id != 0) DAL.ExecuteSL3Query(ORM.Update(masterQuest, "Quests", "", DAL.getTableStructure("Quests"))); //update teh quest in teh db
                                else iLog("Master quest has no id?");
                                iLog(string.Format("Quest {0} ({1}) has been removed from log (Completion)", q.Name, q.Id));
                            }
                        }
                    }
                    else
                    {
                        iLog(string.Format("Quest {0} ({1}) has been removed from log (Abandoned or AutoCompleted)", q.Name, q.Id));
                    }
                    break;
                }
            }
            CurrentQuests.Remove(questToRemove);
        }
        public static void HandleNewQuest(List<Styx.WoWInternals.PlayerQuest> quests)
        {
                iLog("New quest in log...");
                foreach (var q in quests)
                {
                    var quest = new Quest { Description = q.Description, Id = q.Id, IsDaily = q.IsDaily, IsShareable = q.IsShareable, IsWeekly = q.IsWeekly, Level = q.Level, Name = q.Name, ObjectiveText = q.ObjectiveText, RequiredLevel = q.RequiredLevel };
                    if (!CurrentQuests.Contains(quest))
                    {
                        if (questCount > quests.Count && StyxWoW.Me.CurrentTarget == null) log("Abandoned Quest.");
                        if (StyxWoW.Me.CurrentTarget != null)
                            if (StyxWoW.Me.CurrentTarget.IsQuestGiver && questCount < quests.Count) quest.RecievedFrom = StyxWoW.Me.CurrentTarget.Entry;
                        CurrentQuests.Add(quest);
                        iLog(string.Format("Added quest to current quests: {0} ({1}) Recieved From: {2}", quest.Name, quest.Id, quest.RecievedFrom));

                        if (!Quests.Contains(quest))
                        {
                            DAL.ExecuteSL3Query(DAL.Insert(quest, "Quests", "", false, DAL.getTableStructure("Quests")));
                            Quests.Add(quest);
                            iLog(string.Format("Added New Quest {0} ({1}) Recieved From: {2}", quest.Name, quest.Id, quest.RecievedFrom));
                        }
                        break;
                    }
                }
        }
        public static void ProccessFactions()
        {
            var faction = Factions.Where(f => f.FactionId == StyxWoW.Me.CurrentTarget.FactionId && f.Zone == StyxWoW.Me.ZoneId).FirstOrDefault();
            if (faction == null)
            {
                var skinnable = 0;
                if (StyxWoW.Me.CurrentTarget.CanSkin || StyxWoW.Me.CurrentTarget.Skinnable) skinnable = 1;
                DAL.ExecuteSL3Query(String.Format("INSERT OR IGNORE INTO factions (factionid, name, isskinnable, zone) VALUES ({0}, '{1}', '{2}', {3});", Target.FactionId, Target.Faction, skinnable, StyxWoW.Me.ZoneId));
                var f = new Faction { IsSkinnable = StyxWoW.Me.CurrentTarget.Skinnable, Zone = StyxWoW.Me.ZoneId };
                f.FactionId = StyxWoW.Me.CurrentTarget.FactionId;
                if (StyxWoW.Me.CurrentTarget.Faction != null) f.Name = StyxWoW.Me.CurrentTarget.Faction.Name;
                Factions.Add(f);
            }
            else if (faction != null && !faction.IsSkinnable && StyxWoW.Me.CurrentTarget.Skinnable)
            {
                iLog(string.Format("updated faction {0} as skinnable.", Target.FactionId));
                DAL.ExecuteSL3Query(string.Format("update factions set isskinnable = 1 where factionid = '{0}' and zone = '{1}'", Target.FactionId, StyxWoW.Me.ZoneId));
            }


        }
        public static void AoeDataMine(float distance)
        {
            if (Styx.WoWInternals.ObjectManager.ObjectList != null)
            {
                var units = Styx.WoWInternals.ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => p.IsValid && p.DistanceSqr <= distance * distance).ToList();
                Core.iLog(string.Format("Getting nearby WowUnits."));
                foreach (var unit in units)
                {
                    AddUnit(unit);
                }
            }
        }
        #endregion

        #region Event Hooks
        //ToDo: Have these methods raise events.
        public static void InventoryChangeCheck()
        {
            try
            {
                //Core.log(StyxWoW.Me.Inventory.FreeSlots.ToString());
                if (StyxWoW.Me.Inventory.FreeSlots < 20)
                {
                    Core.BagsFull = true;
                }
                var count = 0;
                foreach (var item in StyxWoW.Me.BagItems)
                {
                    count += (int)item.StackCount;
                }
                if (count != inventoryCount)
                {
                    inventoryCount = count;
                    Core.log("Detected change in inventory.");
                    //return true;
                }
            }
            catch (Exception err)
            {
                log(err.ToString());
            }
            //return false;
        }
        #endregion

        #region Helper Methods
        public static void log(string text)
        {
            Logging.Write(Color.FromRgb(144, 0, 255), "Eclipse=>" + text);
        }
        public static void iLog(string text)
        {
            ilog += (string.Format("Eclipse | {0:MM-dd-yy hh:mm:ss} => {1} \r\n", DateTime.Now, text));
            log(text);
        }
        public static double Distance(float[] loc1, float[] loc2)
        {
            return Math.Sqrt(loc1.Zip(loc2, (a, b) => (a - b) * (a - b)).Sum());
        }
        public static string WebRequest(string url)
        {
            HttpWebRequest webRequest = null;
            string responseData = string.Empty;

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.Timeout = 200000;
            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;
        }
        public static string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = string.Empty;
            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }
        #endregion
    }
}
