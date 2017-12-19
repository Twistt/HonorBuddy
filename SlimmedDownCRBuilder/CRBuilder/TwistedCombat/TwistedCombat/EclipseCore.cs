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
using System.Windows.Forms;
using System.Windows.Media;
using Action = Styx.TreeSharp.Action;
using Sequence = Styx.TreeSharp.Sequence;
using Color = System.Windows.Media.Color;
using TwistedCombatRoutines.Data;
using TwistedCombatRoutines;
namespace Eclipse
{

    public static class EC
    {
        public static EclipseCombatRoutine CR { get; set; }
        #region HelperMethods
        public static WoWItemArmorClass GetArmorClass()
        {
            if (StyxWoW.Me.Class == WoWClass.DeathKnight) return WoWItemArmorClass.Plate;
            if (StyxWoW.Me.Class == WoWClass.Druid) return WoWItemArmorClass.Leather;
            if (StyxWoW.Me.Class == WoWClass.Paladin) return WoWItemArmorClass.Plate;
            if (StyxWoW.Me.Class == WoWClass.Shaman && StyxWoW.Me.Level < 40) return WoWItemArmorClass.Leather;
            if (StyxWoW.Me.Class == WoWClass.Shaman && StyxWoW.Me.Level >= 40) return WoWItemArmorClass.Mail;
            return WoWItemArmorClass.None;
        }
       
        internal static void FindDB()
        {
            var path = Application.StartupPath;
            if (!File.Exists(DAL.DBFile))
            {
                var results = Directory.GetFiles(path, "TwistedCombat.edb", SearchOption.AllDirectories).ToList();
                if (results.Count > 0)
                {
                    
                    Log(string.Format("--------------------------------Found {0}------------------------------------", results[0]));
                    DAL.DBFile = results[0];
                }
                else
                {
                    Log(string.Format("---------------Existing Settings file not Found - Creating new one {0}---------------", "TwistedCombat.edb"));
                    DAL.CreateFile("TwistedCombat.edb");
                    DAL.CreateSL3Connection("TwistedCombat.edb");
                    DAL.ExecuteSL3Query(@"create table  EclipseCombatSettings 
                    (id integer primary key autoincrement,WowClass varchar(255), WowSpec varchar(255), RoutineName varchar(255), IsPvp bool, IsPvE bool)");
                    DAL.ExecuteSL3Query(@"CREATE TABLE CombatBehaviors (
                        id integer primary key autoincrement,
                        RoutineId int,
                        [IsAura] [bit],
                        [Display] [text],
                        [AuraName] [text],
                        [AuraId] [int],
                        [CastAtHealthPercentage] [bit],
                        [HealthPercentage] [Real]
                        [RequireMeleeRange] [bit],
                        [SpellIsTrinket] [bit],
                        [TrinketId] [int],
                        [TrinketName] [text],
                        [SpellId] [int],
                        [SpellName] [text],
                        [DontCastIfTargetHasAura] [bit],
                        [HasTargetingRules] [bit],
                        [IsItem] [bit],
                        [IsSpell] [bit],
                        [IsInterrupt] [bit],
                        [RequirePet] [bit],
                        [UseInGroups] [bit],
                        [DontUseInGroups] [bit],
                        [PetIsDead] [bit],
                        [HasNoPet] [bit],
                        [IsIncapacitated] [bit],
                        [Order] [int]
                        
                        )");
                    DAL.DBFile = "TwistedCombat.edb";
                }
            }
        }
        public static void LoadSettings()
        {
            DataTable dt = DAL.LoadSL3Data("Select * from ShadowbotSettings");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    EC.settings = new EclipseCombatRoutine();
                    EC.settings = (EclipseCombatRoutine)ORM.convertDataRowtoObject(EC.settings, dt.Rows[0]);
                }
            }
            else
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL("ShadowbotSettings",""));
            }
        }
        public static void Log(string text, LogLevel level)
        {
            Color color = Color.FromRgb(51, 51, 255);
            if (level == LogLevel.BT) Color.FromRgb(255, 0, 255); // light purp
            if (level == LogLevel.Info) Color.FromRgb(51, 51, 255); //blue
            if (level == LogLevel.Error) Color.FromRgb(255, 51, 51); //blue
            Logging.Write(color, "ECR=>" + text);
        }
        public static void Log(string text)
        {
            Color color = Color.FromRgb(51, 51, 255);
            Logging.Write(color, "ECR=>" + text);
        }
        public static void SellGreys()
        {
            //throw new NotImplementedException();
        }
        #endregion

        public static bool SellingAtVendor { get; set; }

        public static EclipseCombatRoutine settings { get; set; }
    }
    public enum LogLevel
    {
        Info = 0,
        BT,
        Error
    }
}
