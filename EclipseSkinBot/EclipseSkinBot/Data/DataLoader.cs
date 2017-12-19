﻿using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.WoWDatabase.Models;
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
    }
}
