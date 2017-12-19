using ArachnidCreations;
using ArachnidCreations.DevTools;
using CommonBehaviors.Actions;
using Eclipse.ShadowBot.Data;
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
using Eclipse.Comms;
namespace Eclipse.ShadowBot
{

    public static class EC
    {

        #region Global Vars
        public static string DataPath = string.Empty;
        public static LocalPlayer Me = null;
        #endregion

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
        internal static void FindDB()
        {
            var path = Application.StartupPath;
            if (!File.Exists(DataPath))
            {
                var results = Directory.GetFiles(path, "*.edb", SearchOption.AllDirectories).ToList().Where(r => r.Contains("EclipseSettings")).ToList();
                if (results.Count > 0)
                {
                    
                    Log(string.Format("--------------------------------Found {0}------------------------------------", results[0]));
                    DAL.DBFile = results[0];
                }
                else
                {
                    Log(string.Format("---------------Existing Settings file not Found - Creating new one {0}---------------", "EclipseSettings.edb"));
                    DAL.CreateSL3Connection("EclipseSettings.edb");
                    var _CreateSQL = DAL.generateCreateSQL(new ShadowBotSettings(), "ShadowbotSettings");
                    DAL.ExecuteSL3Query(_CreateSQL);
                    DAL.DBFile = "EclipseSettings.edb";
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
                    EclipseShadowBot.settings = new ShadowBotSettings();
                    EclipseShadowBot.settings = (ShadowBotSettings)ORM.convertDataRowtoObject(EclipseShadowBot.settings, dt.Rows[0]);
                }
            }
            else
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL("ShadowbotSettings",""));
            }
        }
        public static void Log(string text, LogLevel level)
        {
            Color color = Color.FromRgb(255, 0, 255);
            if (level == LogLevel.BT) Color.FromRgb(255, 0, 255); // light purp
            if (level == LogLevel.Info) Color.FromRgb(51, 51, 255); //blue
            if (level == LogLevel.Error) Color.FromRgb(255, 51, 51); //blue
            Logging.Write(color, "Eclipse=>" + text);
        }
        public static void Log(string text)
        {
            Color color = Color.FromRgb(255, 0, 255);
            Logging.Write(color, "Eclipse=>" + text);
        }
        public static List<string> Logs = new List<string>();
        public static void InternalLog(string log)
        {
            EC.Logs.Add(DateTime.Now + "-" + log);
        }
        public static void SellGreys()
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region Behavior Helpers
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
            if (StyxWoW.Me.CurrentTarget != null && mob != null)
            {
                if (StyxWoW.Me.CurrentTarget.Entry == mob.Entry) return true;
            }
            if (mob != null)
            {
                TreeRoot.StatusText = ("Bags almost full gonna' go vendor stuff.");
                mob.Target();
                EclipseShadowBot.navLoc = mob.Location;
                EclipseShadowBot.NavMode = true;
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

        #region Behaviors
        public static Composite CreateLootingBehavior
        {
            get
            {
                return new Decorator(r => Me.CurrentTarget != null && !Me.BagsFull,
                    new PrioritySelector(
                        new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.Lootable,
                                    new Sequence(
                                new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))
                                        )
                                    ),
                        new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.Lootable,
                            new Sequence(
                                new Action(r => Me.CurrentTarget.Interact()),
                                new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Entry))
                                )
                            )
                        )
                    );
            }
        }
        public static Composite CreateSkinningBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => StyxWoW.Me.CurrentTarget != null,
                                    new PrioritySelector(
                            new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange,
                                             new Sequence(
                                    new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} for some epic skinning action.", Me.CurrentTarget.Entry)),
                                       new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                           new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location)))
                                                )
                                            ),
                                        new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.CanLoot,
                                            new Sequence(
                                                new Action(r => Me.CurrentTarget.Interact()),
                                                new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Entry))
                           )),
                       new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && isSkinnable(Me.CurrentTarget),
                                            new Sequence(
                                                new Action(r => Me.CurrentTarget.Interact()),
                                                new Action(r => TreeRoot.StatusText = String.Format("Skinning {0}", Me.CurrentTarget.Entry))
                                        )),
                       new Decorator(r => Me.IsFlying && Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange,
                           new Sequence(
                               new Action(context => Lua.DoString("Dismount()")),
                               new Wait(4, new ActionAlwaysSucceed()))),
                       new Decorator(r => isSkinnable(Me.CurrentTarget) && !Me.IsFlying,
                                    new Sequence(
                               new Action(r => Me.CurrentTarget.Interact()),
                               new Action(r => TreeRoot.StatusText = String.Format("Skinning {0}", Me.CurrentTarget.Entry)),
                               new Wait(4, new ActionAlwaysSucceed())
                                    )
                                )
                        )
               ));
            }
        }
        public static Composite CreateQuestBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => GetQuestGiver() && Me.CurrentTarget != null,
                       new Decorator(ret => Me.CurrentTarget.IsAlive,
                           new PrioritySelector(
                               new Decorator(ret => Me.CurrentTarget.Distance <= Me.InteractRange,
                                   new Sequence(
                                        new Action(r => Me.CurrentTarget.Interact()),
                                        new Action(r => TreeRoot.StatusText = String.Format("Getting Quest from {0}", Me.CurrentTarget.Entry))
                                    )),
                               new Decorator(ret => Me.CurrentTarget.Distance > Me.InteractRange,
                                   new Sequence(
                                       new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to get a q.", Me.CurrentTarget.Entry)),
                                       new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location)),
                                       new ActionAlwaysSucceed()
                                    )
                                )
                            )
                        )
                    )
                );
            }
        }
        public static Composite CreateDeadBehavior //this is commmunity contributed content I assume from  FPSWare's RAF bot. If that IS the case than THANK YOU FPSWare!
        {
            get
            {
                return new PrioritySelector(

                // Mount up - for rez sickness wait
                    //new Decorator(ret => !Me.IsDead && !Me.IsGhost && !Me.Mounted && Me.HasAura(15007) && Flightor.MountHelper.CanMount, Common.MountUpFlying()),

                // Mounted? The ascend and just wait out rez sickness
                new Decorator(ret => Me.Mounted && !Me.IsFlying && !StyxWoW.Me.MovementInfo.IsAscending,
                    new Sequence(
                        new Action(context => Log("Flying up to wait out rez sickness")),
                        new Action(context => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromSeconds(4)))
                    )),

                // Just wait out rez sickness
                new Decorator(ret => Me.IsFlying && Me.HasAura(15007), new Action(ctx => { TreeRoot.StatusText = "Waiting out rez sickness"; TreeRoot.StatusText = "Waiting out rez sickness"; return RunStatus.Success; })),

                // Release body
                new Decorator(ret => Me.IsDead && !Me.IsGhost,
                    new Sequence(
                        new Action(context => Log("We're dead! Releasing corpse")),
                        new Action(context => Lua.DoString("RepopMe()"))
                        )),

                // Try to move to our corpse - if we can
                new Decorator(ret => Me.IsGhost,
                    new PrioritySelector(
                    // Move to the location of our corpse
                    // First, try to move to our corpse location exactly
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) > 15 , new Action(context => Navigator.MoveTo(Me.CorpsePoint))),

                        // If that fails try to move within 10 yards of it
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) > 15 , new Action(context => Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 10)))),

                        // Within range of our body? Retrieve our body
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) < 15,
                            new Sequence(
                                new Action(context => Log("Recovering our body")),
                                new Action(context => Lua.DoString("RetrieveCorpse()"))
                        ))
                    ))


                );
            }
        }
        public static Composite CreateHealBehavior()
        {
            return new PrioritySelector(RoutineManager.Current.HealBehavior);
        }
        public static Composite CreateVendorBehavior
        {
            get
            {
                return new Decorator(r => Me.CurrentTarget != null,
                    new PrioritySelector(
                        new Decorator(r => !Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.IsVendor,
                            new Sequence(
                                new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))
                                )
                            ),
                        new Decorator(r => !Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.IsVendor,
                            new Sequence(
                                new Action(r => Me.CurrentTarget.Interact()),
                                new Action(r => TreeRoot.StatusText = String.Format("At Vendor {0}", Me.CurrentTarget.Entry)),
                                new Action(r => SellingAtVendor = true)
                                )
                            )
                        )
                    );
            }
        }
        #endregion

        public static bool SellingAtVendor { get; set; }
    }
    public enum LogLevel
    {
        Info = 0,
        BT,
        Error
    }
}
