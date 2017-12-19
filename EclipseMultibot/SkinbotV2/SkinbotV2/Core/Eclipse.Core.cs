using ArachnidCreations;
using ArachnidCreations.DevTools;
using System;
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
using System.Net;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx;
using Styx.CommonBot;
using Styx.Common;
using Styx.TreeSharp;
namespace Eclipse
{
    public partial class EC
    {

        #region Variables
        public static bool PartyMode = false;
        public static float PartyDistance = 40;
        public static WoWUnit FollowTarget = null;
        public static bool FarmMode = true;
        public static LocalPlayer Me;
        public static WoWPlayer Leader;
        public static bool _isInit = false;
        public static Location loc;
        public static WoWPoint qoLoc;
        public static WoWGuid targetGuid; //For Blacklisting things that stick around...
        public static WoWObject questItem;
        public static QuestOrder ActiveQuestOrder { get; set; }
        public static WoWUnit Target { get; set; }
        public static WoWUnit lastTarget = null;
        public static WoWPoint navLoc;
        public static string StartupPath;
        public static bool NavMode;
        public static string DataPath = "EclipseWoWDB.edb";

        public static bool AddTargetOnly = true;
        public static float DistanceToPoll { get; set; }
        public static bool init { get; set; }
        public static string ilog = string.Empty;

        public static int inventoryCount = 0;
        public static int questCount = 0;
        public static List<Quest> CurrentQuests = new List<Quest>();
        public static bool NinjaSkin = false;
        public static bool PassiveMode = false;
        public static bool ForceNav = false;
        public static Location ForceNavLocation = null;
        public static bool SkinMode = false;
        public static bool KillThese = false;
        public static bool QuestingMode = true;
        public static bool BagsFull = false;
        public static PlayerQuest ActiveQuest = null;
        public static List<QuestOrder> ActiveQuestQuestOrders = new List<QuestOrder>();
        public static uint QuestContextId = 0;
        public static bool LeaderMode;
        public static string FollowName;
        public static bool ShouldBeMounted { get; set; }
        #endregion

        #region HelperMethods
        public static double Distance(float[] loc1, float[] loc2)
        {
            return Math.Sqrt(loc1.Zip(loc2, (a, b) => (a - b) * (a - b)).Sum());
        }
        public static bool NeedPull(object context)
        {
            var target = StyxWoW.Me.CurrentTarget;

            if (target == null)
                return false;

            if (!target.InLineOfSight)
                return false;

            if (target.Distance > Targeting.PullDistance)
                return false;

            return true;
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
        public static void ProcessUnit(WoWUnit unit)
        {
            var npc = EC.NPCs.Where(n => n.Entry == unit.Entry).FirstOrDefault();
            if (npc == null) DAL.Insert(npc, "NPC");
            if (npc != null)
            {
                if (!npc.isVendor && unit.IsVendor) DAL.ExecuteSL3Query(ORM.Update(npc, "NPC"));
            }
        }
        public static bool FindNearestVendor()
        {
            var npc = EC.NPCs.Where(n => n.isVendor).OrderBy(d => Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).FirstOrDefault();
            if (npc != null)
            {
                TreeRoot.StatusText = "(FullBags) Navigating to Vendor...";
                var loc = new Location { Entry = npc.Entry, Name = npc.Name, X = npc.X, Y = npc.Y, Z = npc.Z, Zone = npc.Zone };
                ForceNav = true;
                ForceNavLocation = loc;
                return true;
            }
            else if (npc == null)
            {
                var nearbyvendors = ObjectManager.ObjectList.Where(w => w.Type == WoWObjectType.Unit).ToList().Where(m => ((WoWUnit)m).IsFriendly && ((WoWUnit)m).IsVendor).OrderBy(d => d.Distance).ToList();
                foreach (var vendor in nearbyvendors)
                {
                    ProcessUnit((WoWUnit)vendor);
                }
                if (nearbyvendors.Count > 0)
                {
                    var closestVendor = nearbyvendors.FirstOrDefault();
                    TreeRoot.StatusText = "(FullBags) Navigating to Vendor...";
                    var loc = new Location { Entry = closestVendor.Entry, Name = closestVendor.Name, X = closestVendor.X, Y = closestVendor.Y, Z = closestVendor.Z };
                    ForceNav = true;
                    ForceNavLocation = loc;
                    return true;
                }
            }
            else
            {
                EC.Log("Bags are full and there is no Vendor!");
                return false;
            }
            return false;
        }
        public static bool IsUnitBlackListed(WoWUnit mob)
        {
            if (BlackList.Where(m => m.Guid == mob.Guid).Count() > 0 || Blacklist.Contains(mob.Guid, BlacklistFlags.All))
            {
                EC.Log(string.Format("{0} with (guid:{1}) is in blacklist - ignoring", mob.Name, mob.Guid));
                return true;
            }
            else return false;
        }
        public static void AddMobToBlackList(WoWUnit mob)
        {
            EC.Log(string.Format("Adding {0} to blacklist", mob.Guid));
            BlackList.Add(mob);
        }
        public static WoWItemArmorClass GetArmorClass()
        {
            if (StyxWoW.Me.Class == WoWClass.DeathKnight) return WoWItemArmorClass.Plate;
            if (StyxWoW.Me.Class == WoWClass.Druid) return WoWItemArmorClass.Leather;
            if (StyxWoW.Me.Class == WoWClass.Paladin) return WoWItemArmorClass.Plate;
            if (StyxWoW.Me.Class == WoWClass.Shaman && StyxWoW.Me.Level < 40) return WoWItemArmorClass.Leather;
            if (StyxWoW.Me.Class == WoWClass.Shaman && StyxWoW.Me.Level >= 40) return WoWItemArmorClass.Mail;
            return WoWItemArmorClass.None;
        }
        public static bool isSkinnable(WoWUnit woWUnit)
        {
            var t = woWUnit;
            Me = StyxWoW.Me;
            if (Me.CurrentTarget != null
                && t.IsDead
                && !t.Lootable
                && t.Skinnable
                && t.CanSkin
                && !Me.IsCasting
                && !Me.IsChanneling
                && !Me.Combat
                && !Me.Looting
                && t.Distance <= Me.InteractRange
            )
                return true;
            else return false;
        }
        public static void FindDB()
        {
            //if (!File.Exists(DataPath))
            {
                var results = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.edb", SearchOption.AllDirectories).ToList();
                if (results.Count > 0)
                {
                    DataPath = results[0];
                    Log(string.Format("--------------------------------Found {0}------------------------------------", results[0]));
                    DAL.DBFile = DataPath;
                }
            }
        }
        public static void LoadSettings()
        {
            DataTable dt = DAL.LoadSL3Data("Select * from MultiBotSettings");
            //settings = (MultiBotSettings)ORM.convertDataRowtoObject(settings, dt.Rows[0]);
        }
        public static void Log(string text, LogLevel level)
        {
            Color color = Color.FromRgb(255, 0, 255);
            if (level == LogLevel.BT) Color.FromRgb(255, 0, 255); // light purp
            if (level == LogLevel.Info) Color.FromRgb(51, 51, 255); //blue
            if (level == LogLevel.Error) Color.FromRgb(255, 51, 51); //reddish-orange
            if (level == LogLevel.Error) Color.FromRgb(4, 98, 196); //teal
            Logging.Write(color, "Eclipse=>" + text);
        }
        public static void Log(string text)
        {
            Color color = Color.FromRgb(255, 0, 255);
            Logging.Write(color, "Eclipse=>" + text);
        }
        public static void SellGreys()
        {
            //throw new NotImplementedException();
        }
        public static bool GetQuestGiver()
        {
            //ToDo: Converting the Delegate 12 times in the lamda expression is probably super slow and crappy we should convert the whole list.
            var questgivers = ObjectManager.ObjectList.Where(n =>
                n.Type == WoWObjectType.Unit
            ).ToList().Where(n =>
                ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.Available
                && ((WoWUnit)n).IsFriendly
                || ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.TurnIn
                && ((WoWUnit)n).IsFriendly
                || ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.TurnInRepeatable
                && ((WoWUnit)n).IsFriendly
            ).OrderBy(m => m.Distance).ToList();


            if (questgivers.Count > 0)
            {
                var mob = (WoWUnit)questgivers.OrderBy(m => m.Distance).FirstOrDefault();
                Log("Found a quest giver: " + mob.Name);
                mob.Target();

                return true;
            }

            //Log("No q givers around.");
            return false;
        }
        public static bool FindVendor()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && ((WoWUnit)n).IsVendor && ((WoWUnit)n).IsFriendly).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                Log("Bags almost full gonna' go vendor stuff.");
                mob.Target();
                return true;
            }
            else return false;
        }
        public static bool TargetClosestLootableMob()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && ((WoWUnit)n).Lootable).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                Log("Found a lootable mob");
                mob.Target();
                qoLoc = mob.Location;
                return true;
            }
            else return false;
        }
        public static bool TargetClosestSkinnableMob()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && isSkinnable((WoWUnit)n)).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                Log("Found a skinnable mob");
                mob.Target();
                return true;
            }
            else return false;
        }
        #endregion
    }
    public enum LogLevel
    {
        Info = 0,
        BT,
        Error,
        Comms
    }
}
