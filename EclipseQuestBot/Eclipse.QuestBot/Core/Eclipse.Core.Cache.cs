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

namespace Eclipse
{
    public partial class EC
    {
	    #region Caches
        public static List<Quest> Quests = new List<Quest>();
        public static List<QuestOrder> QuestOrders = new List<QuestOrder>();
        public static List<Styx.WoWInternals.PlayerQuest> IncompleteableQuests = new List<Styx.WoWInternals.PlayerQuest>();
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
        public static List<uint> NewQuests = new List<uint>();
        public static List<EclipseVendor> Vendors = new List<EclipseVendor>();
        public static List<WoWUnit> BlackList = new List<WoWUnit>();
        public static List<WoWGuid> TemporaryIgnoreList = new List<WoWGuid>();

        #endregion
        #region Cache Helpers
        public static void AddNpc(WoWUnit Target)
        {
            if (EC.NPCs.Where(m => m.Entry == EC.Target.Entry).Count() == 0)
            {
                var npc = new NPC { Name = EC.Target.Name };
                npc.isVendor = EC.Target.IsVendor;
                npc.Entry = EC.Target.Entry;
                npc.Zone = StyxWoW.Me.ZoneId;
                npc.FactionId = EC.Target.FactionId;
                npc.isQuestGiver = EC.Target.IsQuestGiver;
                npc.X = EC.Target.X;
                npc.Y = EC.Target.Y;
                npc.Z = EC.Target.Z;
                npc.Level = EC.Target.Level;
                DAL.ExecuteSL3Query(DAL.Insert(npc, "NPC", "", false, DAL.getTableStructure("NPC")));
                EC.NPCs.Add(npc);
                EC.Log(string.Format("Added NPC {0} ({1})", npc.Name, npc.Entry), LogLevel.Info);
            }
        }
        public static void AddMob(WoWUnit Target)
        {
            var _mob = EC.MOBs.Where(m => m.Entry == EC.Target.Entry).FirstOrDefault();
            if (_mob == null && EC.Target != null)
            {
                try
                {
                    var mob = new Mob { Name = EC.Target.Name };
                    mob.Entry = EC.Target.Entry;
                    mob.Zone = StyxWoW.Me.ZoneId;
                    if (Target.FactionId != 0) mob.FactionId = EC.Target.FactionId;
                    mob.isSkinnable = EC.Target.Skinnable;
                    mob.Level = EC.Target.Level;

                    var sql = DAL.Insert(mob, "MOB", "", false, DAL.getTableStructure("MOB"));
                    DAL.ExecuteSL3Query(sql);
                    EC.MOBs.Add(mob);
                    Log(string.Format("Added Mob {0} ({1})", mob.Name, mob.Entry));
                }
                catch (Exception err)
                {
                    Log(string.Format("Could not DB mob {0} ({1}) probably a serialization thing...\r\n{2}", EC.Target.Name, EC.Target.Entry, err.ToString()));
                }
            }
            else
            {
                Log("Mob is already in the database.");
                if (Target.Skinnable || EC.Target.CanSkin)
                {
                    if (!_mob.isSkinnable)
                    {
                        Log("Mob is not marked skinnable in the db but it is - updating.");
                        if (EC.SkinMode && !EC.KillList.Contains(_mob))
                        {
                            Log("Since we are in skinmode and this mob is skinnable we are adding it to the Kill List");
                            EC.KillList.Add(_mob);
                        }
                        _mob.isSkinnable = true;
                        EC.MOBs.Where(m => m.Entry == EC.Target.Entry).FirstOrDefault().isSkinnable = true; //make sure its updated in teh list not just this instance.
                        UpdateMob(_mob);
                    }
                }
            }
        }
        public static void UpdateMob(Mob mob)
        {
            var sql = ORM.Update(mob, "MOB", "", DAL.getTableStructure("MOB"));
            DAL.ExecuteSL3Query(sql);
        }
        public static void AddUnit(WoWUnit unit)
        {
            if (!unit.IsPlayer)
            {
                if (unit.IsFriendly) AddNpc(unit);
                else AddMob(unit);
            }
        }
        #endregion
    }
}
