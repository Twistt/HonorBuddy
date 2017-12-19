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
using System.Text;
using System.Windows.Forms;

namespace Eclipse.WoWDatabase
{
    public class Core
    {
        #region Variables
        //public static string DataPath ="C:\\Users\\Twist\\Documents\\Honorbuddy\\EclipseWoWDB.edb";
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
                var npc = new NPC { Name = Target.Name, Entry = Target.Entry, Zone = StyxWoW.Me.ZoneId, FactionId = Target.Faction.Id, isQuestGiver = Target.IsQuestGiver };
                DAL.ExecuteSL3Query(DAL.Insert(npc, "NPC", "", false, DAL.getTableStructure("NPC")));
                NPCs.Add(npc);
                iLog(string.Format("Added NPC {0} ({1})", npc.Name, npc.Entry));
            }
        }
        internal static void AddMob(WoWUnit Target)
        {
            if (MOBs.Where(m=>m.Entry == Target.Entry).Count() ==0 && Target!= null)
            {
                try
                {
                    var mob = new Mob { Name = Target.Name, Entry = Target.Entry, Zone = StyxWoW.Me.ZoneId, FactionId = Target.Faction.Id, isSkinnable = Target.Skinnable };
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
            } else
            {
                log(string.Format("-------Found file at {0}-----------", DataPath));
                
            }
        }
        internal static void Initialize()
        {
            Core.log("Running init...");
            FindDB();

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
        public static void Pulse()
        {
            
            if (init == true)
            {
                try
                {
                    if (Me == null) Me = StyxWoW.Me;
                    if (Zones.Where(z => z.Id == StyxWoW.Me.ZoneId).FirstOrDefault() == null)
                    {


                        var newzone = new Zone { Id = StyxWoW.Me.ZoneId, Name = StyxWoW.Me.ZoneText };
                        //DAL.ExecuteSL3Query(ORM.generateCreateSQL(newzone, "Zones"));
                        DAL.ExecuteSL3Query(ORM.Insert(newzone, "Zones", "", false, DAL.getTableStructure("Zones")));
                        Zones.Add(newzone);
                    }
                    if (Zones.Where(z => z.Id == StyxWoW.Me.SubZoneId).FirstOrDefault() == null)
                    {
                        var newzone = new Zone { Id = StyxWoW.Me.SubZoneId, Name = StyxWoW.Me.SubZoneText, ParentZone = StyxWoW.Me.ZoneId };
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
                            Core.log(string.Format("Found a new rare mob ", obj.Name));
                        }
                    }
                    var objects = Styx.WoWInternals.ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.GameObject).OrderBy(m => m.Distance).ToList();
                    foreach (var obj in objects)
                    {

                        var o = (WoWGameObject)obj;
                        if (o.SubType == WoWGameObjectType.GatheringNode)
                        {
                            if (Core.Objects.Where(c => c.Id == o.Entry).Count() == 0)
                            {
                                var newobj = new EclipseObject { Entry = o.Entry, Name = o.Name, OType = EclipseGeneric.ObjectType.Object, Id = o.Entry, X = o.X, Y = o.Y, Z = o.Z };
                                Objects.Add(newobj);
                                DAL.ExecuteSL3Query(ORM.Insert(o, "Objects", "", false, DAL.getTableStructure("Objects")));
                                Core.log(string.Format("Found a new type of gathering node", obj.Name));
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
                        //if (o.SubType == WoWGameObjectType.Chest)
                        //{
                        //    if (Core.Objects.Where(c => c.Id == o.Entry).Count() == 0)
                        //    {
                        //        var newobj = new EclipseObject { Entry = o.Entry, Name = o.Name, OType = EclipseGeneric.ObjectType.Object, Id = o.Entry, X = o.X, Y = o.Y, Z = o.Z };
                        //        Objects.Add(newobj);
                        //        DAL.ExecuteSL3Query(ORM.Insert(o, "Objects", "", false, DAL.getTableStructure("Objects")));
                        //        EC.log(string.Format("Found a new chest location", obj.Name));
                        //    }

                        //    var loc = new Location { Entry = o.Entry, X = o.X, Y = o.Y, Z = o.Z, Zone = StyxWoW.Me.ZoneId };
                        //    var savedloc = Core.Locations.Where(l => l.Z == loc.Z && loc.Y == l.Y && loc.Z == l.Z && loc.Entry == l.Entry && loc.Zone == l.Zone).FirstOrDefault();
                        //    if (savedloc == null)
                        //    {
                        //        //ToDo: see if these are close to another hotspot (with 20 yards) and dont add them - probably best on a new thread...
                        //        Core.log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, o.Name));
                        //        DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                        //        Core.Locations.Add(loc);
                        //    }
                        //}

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

                    ProccessQuests();
                }
                catch (Exception err){
                    iLog("Error at root level: " + err.ToString());
                }
            }
            else Initialize();

        }
        #endregion

        #region HandlerMethods
        public static void ProccessUnits()
        {
            
            if (AddTargetOnly == true) AddUnit(Target);
            else AoeDataMine(DistanceToPoll);
        }
        public static void ProccessQuests()
        {
            foreach (var q in StyxWoW.Me.QuestLog.GetAllQuests().ToList())
            {
                if (Quests.Where(qu=>qu.Id == q.Id).Count() == 0)
                {
                    
                    var quest = new Quest { Description=q.Description, Id=q.Id, IsDaily=q.IsDaily, IsShareable=q.IsShareable, IsWeekly=q.IsWeekly, Level=q.Level, Name=q.Name, ObjectiveText=q.ObjectiveText, RequiredLevel=q.RequiredLevel  };
                    DAL.ExecuteSL3Query(DAL.Insert(quest, "Quests", "", false, DAL.getTableStructure("Quests")));
                    Quests.Add(quest);
                    iLog(string.Format("Added Quest {0} ({1})", quest.Name, quest.Id));
                }
            }
        }
        public static void ProccessFactions()
        {
            var faction = Factions.Where(f => f.FactionId == Target.FactionId && f.Zone == StyxWoW.Me.ZoneId).FirstOrDefault();
            if (faction == null)
            {
                var skinnable = 0;
                if (Target.CanSkin || Target.Skinnable) skinnable = 1;
                DAL.ExecuteSL3Query(String.Format("INSERT OR IGNORE INTO factions (factionid, name, isskinnable, zone) VALUES ({0}, '{1}', '{2}', {3});", Target.FactionId, Target.Faction, skinnable, StyxWoW.Me.ZoneId));
                Factions.Add(new Faction { FactionId = Target.FactionId, IsSkinnable = Target.Skinnable, Zone = StyxWoW.Me.ZoneId, Name = Target.Faction.Name });
            }
            else if (faction != null && !faction.IsSkinnable && Target.Skinnable)
            {
                iLog(string.Format("updated faction {0} as skinnable.", Target.Faction.Id));
                DAL.ExecuteSL3Query(string.Format("update factions set isskinnable = 1 where factionid = '{0}' and zone = '{1}'", Target.Faction.Id, StyxWoW.Me.ZoneId));
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

        #region Helper Methods
        public static void log(string text)
        {
            Logging.Write("Eclipse=>" + text);
        }
        public static void iLog(string text)
        {
            ilog += (string.Format("Eclipse=> {0:MM-dd-yy hh:mm:ss} => {1} \r\n", DateTime.Now, text));
        }
        #endregion
    }
}
