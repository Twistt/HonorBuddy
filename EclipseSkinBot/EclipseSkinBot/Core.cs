using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.WoWDatabase.Models;
using Styx;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Color = System.Windows.Media.Color;
namespace Eclipse.WoWDatabase
{
    public class Core
    {
        #region Variables
        public static string DataPath ="EclipseWoWDB.edb";
        public static LocalPlayer Me { get; set; }
        public static WoWUnit Target { get; set; }
        public static bool AddTargetOnly = true;
        public static float DistanceToPoll { get; set; }
        public static bool init { get; private set; }
        public static string ilog = string.Empty;
        private static WoWUnit lastTarget = null;
        #endregion

        #region Caches
        public static List<Quest> Quests = new List<Quest>();
        public static List<NPC> NPCs = new List<NPC>();
        public static List<Mob> MOBs = new List<Mob>();
        public static List<Location> Locations = new List<Location>();
        public static List<Faction> Factions = new List<Faction>();
        public static List<Location> RecentlyVisitedLocations = new List<Location>();
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
            var _mob = MOBs.Where(m => m.Entry == Target.Entry).FirstOrDefault();
            if (_mob == null && Target != null)
            {
                try
                {
                    var mob = new Mob { Name = StyxWoW.Me.CurrentTarget.Name };
                    mob.Entry = StyxWoW.Me.CurrentTarget.Entry;
                    mob.Zone = StyxWoW.Me.ZoneId;
                    if (StyxWoW.Me.CurrentTarget.FactionId != 0) mob.FactionId = StyxWoW.Me.CurrentTarget.FactionId;
                    mob.isSkinnable = StyxWoW.Me.CurrentTarget.Skinnable;
                    mob.Level = StyxWoW.Me.CurrentTarget.Level;

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
                if (Target.Skinnable || Target.CanSkin )
                {
                    if (!_mob.isSkinnable)
                    {
                        log("Mob is not marked skinnable in the db but it is - updating.");
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
            init = true;
            iLog("Finished init.");
        }
        internal static void FindDB(){
            var path = Application.StartupPath;
            if (!File.Exists(DataPath))
            {
                var results = Directory.GetFiles(path, "*.edb", SearchOption.AllDirectories).ToList();
                if (results.Count > 0){
                    DataPath = results[0];
                    log(string.Format("--------------------------------Found {0}------------------------------------", results[0]));
                }
            }


        }
        public static void Pulse()
        {

            if (init == true)
            {
                try
                {
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
                catch (Exception err)
                {
                    log("Error at root level: " + err.ToString());
                }
            }
            else
            {
                log("Not Initialized - Calling now:");
                Initialize();
                
            }

        }
        #endregion

        #region HandlerMethods
        public static void ProccessUnits()
        {
            
            if (AddTargetOnly == true) AddUnit(Target);
            else AoeDataMine(DistanceToPoll);

            var loc = new Location { Entry=Target.Entry, X=Target.X, Y=Target.Y, Z=Target.Z, Zone = StyxWoW.Me.ZoneId };
            if (!Locations.Contains(loc))
            {
                log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, Target.Name));
                DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                Locations.Add(loc);
            }

            var mob = MOBs.Where(e => e.Entry == Target.Entry).FirstOrDefault();
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

        #region Helper Methods
        public static void log(string text)
        {
            Logging.Write(Color.FromRgb(144,0,255),"Eclipse=>" + text);
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
        #endregion
    }
}
