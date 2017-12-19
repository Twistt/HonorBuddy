using Eclipse.Comms;
using Eclipse.Models;
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
using Color = System.Windows.Media.Color;
using JSONSharp;
using ArachnidCreations;
using ArachnidCreations.DevTools;
namespace Eclipse.Multibot
{
    public class Main
    {
        public static bool init = false;

         #region  Core Methods
        public static void Initialize()
        {
            iLog("Running init...");
            FindDB();
            DataLoader.loadQuestorders();
            DataLoader.loadMobs();
            DataLoader.loadNPCs();
            //DataLoader.loadQuests();
            DataLoader.loadLocations();
            DataLoader.loadFavorites();
            DataLoader.loadVendors();
            init = true;
            iLog("Finished init.");
        }
        public static void FindDB()
        {
            var path = Application.StartupPath;
            if (!File.Exists(EC.DataPath))
            {
                var results = Directory.GetFiles(path, "*.edb", SearchOption.AllDirectories).ToList();
                if (results.Count > 0)
                {
                    EC.DataPath = results[0];
                    log(string.Format("--------------------------------Found {0}------------------------------------", EC.DataPath));
                }
                else
                {
                    EC.DataPath = @"C:\Users\Twist\Documents\Honorbuddy\Bots\SkinbotV2\SkinbotV2\Data\EclipseWoWDB.edb";
                    log(string.Format("Didnt find ANY .edb Files using HARD CODED file instead (If your seeing this and your me you will need to change this hardcoded value)"));
                }
            }
        }
        public static bool Pulse()
        {
            if (EC.init == true)
            {
                try
                {
                    //UpdateQuests();
                    InventoryChangeCheck();
                    if (StyxWoW.Me.CurrentTarget != null)
                    {
                        if (EC.lastTarget != StyxWoW.Me.CurrentTarget || EC.Target == null)
                        {
                            EC.Target = StyxWoW.Me.CurrentTarget;
                            iLog(string.Format("Target changed to {0} ({1})", EC.Target.Name, EC.Target.Entry));
                            ProccessUnits();
                            ProccessFactions();
                            EC.lastTarget = StyxWoW.Me.CurrentTarget;
                        }
                    }
                    else EC.lastTarget = null;
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
        public static void AddRafModeServerEvents(){
            Comms.CommsCommon.AddServerCommHandler("Test", () =>
            {
                //this is to show the anonymous function style of event delegates =P
                //return ServerCommon.OK.ToJSON();
                return null;
            });
        }
        public static void AddRAFModeClientEvents()
        {
            Comms.CommsCommon.AddServerCommHandler("Test", () =>
            {
                //this is to show the anonymous function style of event delegates =P
                //return ServerCommon.OK;
                return null;
            });
        }
        private static void UpdateQuests()
        {
            var quests = StyxWoW.Me.QuestLog.GetAllQuests();
            foreach (var q in quests)
            {
                if (EC.CurrentQuests.Where(qe => qe.Id == q.Id).Count() == 0) 
                {
                    Quest quest = new Quest() { Id = q.Id, Description = q.Description, Level = q.Level, Name = q.Name, RecievedFrom = EC.QuestContextId, Money = q.RewardMoney };
                    var dt = DAL.LoadSL3Data(string.Format("select * from questorders where questid = '{0}'", quest.Id));
                    if (dt == null) EC.Log("Did not find any quest orders for this quest");
                    else
                    {
                        try
                        {
                            List<QuestOrder> questorders = ORM.convertDataTabletoObject<QuestOrder>(dt, "");
                            quest.QuestOrders = questorders;
                            EC.Log(String.Format("Loaded {0} for Quest named {1}", quest.QuestOrders.Count(), quest.Name));
                            EC.CurrentQuests.Add(quest);
                            EC.QuestContextId = 0;
                        }
                        catch (Exception err)
                        {
                            EC.Log(err.ToString());
                        }
                    }
                }
            }
        }
        #endregion

        #region HandlerMethods

        public static void ProccessUnits()
        {

            if (EC.AddTargetOnly == true) EC.AddUnit(EC.Target);
            else AoeDataMine(EC.DistanceToPoll);

            if (!EC.Target.IsPlayer)
            {
                var loc = new Location { Entry = EC.Target.Entry, X = EC.Target.X, Y = EC.Target.Y, Z = EC.Target.Z, Zone = StyxWoW.Me.ZoneId };
                var savedloc = EC.Locations.Where(l => l.Z == loc.Z && loc.Y == l.Y && loc.Z == l.Z && loc.Entry == l.Entry && loc.Zone == l.Zone).FirstOrDefault();
                if (savedloc == null)
                {
                    //ToDo: see if these are close to another hotspot (with 20 yards) and dont add them - probably best on a new thread...

                    log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, EC.Target.Name));
                    DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                    EC.Locations.Add(loc);
                }

                var mob = EC.MOBs.Where(e => e.Entry == EC.Target.Entry).FirstOrDefault();
                var npc = EC.NPCs.Where(e => e.Entry == EC.Target.Entry).FirstOrDefault();
                if (npc != null)
                {
                    var needsupdate = false;
                    if (npc.Level == 0) needsupdate = true;
                    if (!npc.isVendor && EC.Target.IsVendor) needsupdate = true;

                    if (needsupdate)
                    {
                        EC.Log("Updating npc.");
                        EC.NPCs.Remove(npc);
                        npc.Level = EC.Target.Level;
                        npc.isVendor = EC.Target.IsVendor;
                        DAL.ExecuteSL3Query(ORM.Update(npc, "NPC", "", DAL.getTableStructure("NPC")));
                        EC.NPCs.Add(npc);
                    }
                }
                if (mob != null)
                {
                    if (mob.Level == 0)
                    {
                        EC.UpdateMob(mob);
                        EC.MOBs.Where(e => e.Entry == EC.Target.Entry).FirstOrDefault().Level = EC.Target.Level;
                    }
                    if (EC.Target.IsDead && !EC.Target.Lootable && EC.Target.Skinnable)
                    {
                        iLog("Found skinnable mob - updating mob entry.");
                        EC.MOBs.Remove(mob);
                        mob.isSkinnable = true;
                        DAL.ExecuteSL3Query(ORM.Update(mob, "MOB", "", DAL.getTableStructure("MOB")));
                        EC.MOBs.Add(mob);
                    }
                }
            }
        }
        public static void ProccessFactions()
        {
            var faction = EC.Factions.Where(f => f.FactionId == StyxWoW.Me.CurrentTarget.FactionId && f.Zone == StyxWoW.Me.ZoneId).FirstOrDefault();
            if (faction == null)
            {
                var skinnable = 0;
                if (StyxWoW.Me.CurrentTarget.CanSkin || StyxWoW.Me.CurrentTarget.Skinnable) skinnable = 1;
                DAL.ExecuteSL3Query(String.Format("INSERT OR IGNORE INTO factions (factionid, name, isskinnable, zone) VALUES ({0}, '{1}', '{2}', {3});", EC.Target.FactionId, EC.Target.Faction, skinnable, StyxWoW.Me.ZoneId));
                var f = new Faction { IsSkinnable = StyxWoW.Me.CurrentTarget.Skinnable, Zone = StyxWoW.Me.ZoneId };
                f.FactionId = StyxWoW.Me.CurrentTarget.FactionId;
                if (StyxWoW.Me.CurrentTarget.Faction != null) f.Name = StyxWoW.Me.CurrentTarget.Faction.Name;
                EC.Factions.Add(f);
            }
            else if (faction != null && !faction.IsSkinnable && StyxWoW.Me.CurrentTarget.Skinnable)
            {
                iLog(string.Format("updated faction {0} as skinnable.", EC.Target.FactionId));
                DAL.ExecuteSL3Query(string.Format("update factions set isskinnable = 1 where factionid = '{0}' and zone = '{1}'", EC.Target.FactionId, StyxWoW.Me.ZoneId));
            }


        }
        public static void AoeDataMine(float distance)
        {
            if (Styx.WoWInternals.ObjectManager.ObjectList != null)
            {
                var units = Styx.WoWInternals.ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => p.IsValid && p.DistanceSqr <= distance * distance).ToList();
                Main.iLog(string.Format("Getting nearby WowUnits."));
                foreach (var unit in units)
                {
                    EC.AddUnit(unit);
                }
            }
        }
        public static List<WoWObject> GetNearbyNPCSWithQuests()
        {
            return Styx.WoWInternals.ObjectManager.ObjectList.Where(w => w.Type == WoWObjectType.Unit).ToList().Where(m => ((WoWUnit)m).IsFriendly && ((WoWUnit)m).QuestGiverStatus == QuestGiverStatus.Available).ToList();
        }
        #endregion

        #region Event Hooks
        //ToDo: Have these methods raise events.
        public static void InventoryChangeCheck()
        {
            try
            {
                //EC.Log(StyxWoW.Me.Inventory.FreeSlots.ToString());
                if (StyxWoW.Me.Inventory.FreeSlots < 16)
                {
                    EC.Log("Bags are full we should go find a vendor.");
                    EC.BagsFull = true;
                }
                var count = 0;
                foreach (var item in StyxWoW.Me.BagItems)
                {
                    count += (int)item.StackCount;
                }
                if (count != EC.inventoryCount)
                {
                    EC.inventoryCount = count;
                    EC.Log("Detected change in inventory.");
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
            EC.Log(string.Format("Eclipse | {0:MM-dd-yy hh:mm:ss} => {1} \r\n", DateTime.Now, text), LogLevel.Info);
        }

        #endregion

        public static void PreLoadQuests()
        {
            var questlog = StyxWoW.Me.QuestLog.GetAllQuests();
            EC.ActiveQuest = questlog.FirstOrDefault();
            EC.ActiveQuestQuestOrders = EC.QuestOrders.Where(q => q.QuestId == EC.ActiveQuest.Id).ToList();
        }
    }
}
