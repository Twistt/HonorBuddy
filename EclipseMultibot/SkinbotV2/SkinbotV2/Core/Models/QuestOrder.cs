using Styx;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.Models
{
    public class QuestOrder
    {
        public int id { get; set; }
        public QuestOrder Parent { get; set; }
        private string _giverName = string.Empty;
        private string _turnInName = string.Empty;
        public string QuestName { get; set; }
        public string GiverName
        {
            get
            {
                if (_giverName == string.Empty) return "Unknown";
                else return _giverName;
            }
            set { _giverName = value; }
        }
        public string TurnInName
        {
            get
            {
                if (_turnInName == string.Empty) return "Unknown";
                else return _turnInName;
            }
            set { _turnInName = value; }
        }
        public string GiverId { get; set; }
        public string BehaviorName { get; set; }
        public string BahaviorClassName { get; set; }
        public uint QuestId { get; set; }
        public QuestObjective.QuestType objectiveType { get; set; }
        public QOType type { get; set; }
        public enum QOType
        {
            None = 0,
            PickUp,
            Objective,
            TurnIn,
            UseItem,
            CustomBehavior,
            LogicBlock,
            RunTo,
            FlyTo,
            MoveTo
        }
        public WoWClass classRestriction { get; set; }
        public string QuestObjectiveType
        {
            get
            {
                if (objectiveType != null) return objectiveType.ToString();
                else return string.Empty;
            }
            set
            {
                objectiveType = (QuestObjective.QuestType)Enum.Parse(typeof(QuestObjective.QuestType), value);
            }
        }
        public uint SpellId { get; set; }
        public string SpellName { get; set; }
        public string ZoneName { get; set; }
        public uint InventoryItemID { get; set; }
        public string Type
        {
            get
            {
                if (type != null) return type.ToString();
                else return string.Empty;
            }
            set
            {
                type = (QOType)Enum.Parse(typeof(QOType), value);
            }
        }
        public string ClassRestriction
        {
            get
            {
                if (classRestriction != null) return classRestriction.ToString();
                else return string.Empty;
            }
            set
            {
                classRestriction = (WoWClass)Enum.Parse(typeof(WoWClass), value);
            }
        }
        public string MobId { get; set; }
        public string ItemId { get; set; }
        public int KillCount { get; set; }
        public int CollectCount { get; set; }
        public string TurnInId { get; set; }

        public string Description { get; set; }
        public string DisplayName
        {
            get
            {
                if (QuestName != null) return string.Format("{0} => {1}", type.ToString(), QuestName);
                else if (type == QOType.CustomBehavior) return string.Format("{0} => {1}", type.ToString(), ((CustomBehavior)this).file);
                else if (type == QOType.RunTo) return string.Format("{0} => {1},{2},{3}", type.ToString(), X, Y, Z);
                else if (type == QOType.LogicBlock)
                {
                    var qol = (QuestOrderLogic)this;
                    var tag = "";
                    if (qol.StartTag) tag = "(Start)";
                    else tag = "(End)";
                    if (Description == string.Empty || Description == null) return string.Format("{0}=>{2}{1}", qol.LogicType, qol.Condition, tag);
                    else return string.Format("{0}=>{2}{1}", qol.LogicType, qol.Description, tag);
                }
                else if (type == QOType.PickUp || type == QOType.TurnIn) return string.Format("{0} => {2}({1})", type.ToString(), QuestId, QuestName);
                else if (type == QOType.Objective) return string.Format("{0} => {2}({1})", type.ToString(), QuestId, QuestName);
                else return string.Format("{0} => {1}", type.ToString(), objectiveType);
            }
            set { }
        }

        public string ItemName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public string MobName { get; set; }
        public string NumOfTimes { get; set; }
        public int KilledOrCollected = 0;


        public WoWPoint Location
        {
            get { return new WoWPoint(X, Y, Z); }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }
        public int CurrentCollectCount = 0;
        public int CurrentKillCount = 0;
        public QuestStatus status = 0;
        public bool HasPrerequisite
        {
            get
            {
                if (QuestPrerequisite == 0 || QuestPrerequisite == null) return false;
                else return true;
            }
            set { }
        }
        public uint QuestPrerequisite { get; set; }
        public List<WoWUnit> KilledMobs = new List<WoWUnit>();
    }
    public enum QuestStatus
    {
        None = 0,
        NotStarted,
        Complete,
        InProgress,
        EnRoute
    }

}
