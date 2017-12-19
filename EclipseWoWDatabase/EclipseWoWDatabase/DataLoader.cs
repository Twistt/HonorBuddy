using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.EclipsePlugins.Models;
using Eclipse.WoWDatabase.Models;
using GatheringLegion.Models;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.WoWDatabase
{
    public static class DataLoader
    {
        public static bool loadNPCs()
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  NPC;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var npc = (NPC)ORM.convertDataRowtoObject(new NPC(), row, "");
                    Core.NPCs.Add(npc);

                }
                return true;
            }
            else return false;
        }
        public static bool loadZones()
        {
            if (DAL.getTableStructure("Zones") == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(new Zone(), "Zones"));
            }

            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Zones;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var zone = (Zone)ORM.convertDataRowtoObject(new Zone(), row, "");
                    Core.Zones.Add(zone);

                }
                return true;
            }
            else return false;
        }
        public static bool loadObjects()
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Objects;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var obj = (EclipseObject)ORM.convertDataRowtoObject(new EclipseObject(), row, "");
                    Core.Objects.Add(obj);

                }
                return true;
            }
            else return false;
        }
        public static bool loadMobs()
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Mob;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var Mob = (Mob)ORM.convertDataRowtoObject(new Mob(), row, "");
                    Core.MOBs.Add(Mob);

                }
                return true;
            }
            else return false;
        }
        public static bool loadQuests()
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Quests;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var quest = (Quest)ORM.convertDataRowtoObject(new Quest(), row, "");
                    Core.Quests.Add(quest);

                }
                return true;
            }
            else return false;
        }

        public static bool loadLocations()
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Locations;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var location = (Location)ORM.convertDataRowtoObject(new Location(), row, "");
                    Core.Locations.Add(location);

                }
                return true;
            }
            else return false;
        }
        public static bool loadFavorites()
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Favorites;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var location = (Location)ORM.convertDataRowtoObject(new Location(), row, "");
                    Core.FavoritePlaces.Add(location);

                }
                return true;
            }
            else return false;
        }
    }
}
