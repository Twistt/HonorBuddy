using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse;
using Eclipse.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse
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
                    EC.NPCs.Add(npc);

                }
                return true;
            }
            else return false;
        }
        public static bool loadMobs()
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  MOB;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var Mob = (Mob)ORM.convertDataRowtoObject(new Mob(), row, "");
                    EC.MOBs.Add(Mob);

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
                    EC.Locations.Add(location);

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
                    EC.FavoritePlaces.Add(location);

                }
                return true;
            }
            else return false;
        }
        public static bool loadQuestorders() {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  QuestOrders;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var qo = (QuestOrder)ORM.convertDataRowtoObject(new QuestOrder(), row, "");
                    EC.QuestOrders.Add(qo);
                }
                return true;
            }
            else return false;
        }
        public static void loadVendors()
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Vendors;"));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var vendor = (EclipseVendor)ORM.convertDataRowtoObject(new EclipseVendor(), row, "");
                    EC.Vendors.Add(vendor);

                }
            }
        }
    }
}
