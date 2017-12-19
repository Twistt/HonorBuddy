﻿using Eclipse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Eclipse.MultiBot.Data;
using ArachnidCreations;
using System.Data;
using ArachnidCreations.DevTools;
using Eclipse;

namespace Eclipse
{
    public class EclipseProfile
    {
        #region StaticProfileData
        public static List<EclipseMob> AvoidMobs = new List<EclipseMob>();
        public static List<Blackspot> BlackSpots = new List<Blackspot>();
        public static List<QuestOverride> QuestOverrides = new List<QuestOverride>();
        public static List<QuestOrder> QuestOrders = new List<QuestOrder>();
        public static List<EclipseVendor> Vendors = new List<EclipseVendor>();
        public static List<MailBox> MailBoxes = new List<MailBox>();
        public static List<CustomBehavior> CustomBehaviors = new List<CustomBehavior>();
        public static List<QuestOrderLogic> LogicBlocks = new List<QuestOrderLogic>();
        public static string ProfileLoadErrors = string.Empty;
        public static string Filename = "";
        #endregion
        
        #region InstanceProfileData
        public string Name { get; set; }
        public string MinLevel { get; set; }
        public string MaxLevel { get; set; }
        public bool SellGrey { get; set; }
        public bool SellWhite { get; set; }
        public bool SellGreen { get; set; }
        public bool SellBlue { get; set; }
        public bool SellPurple { get; set; }
        public bool MailGrey { get; set; }
        public bool MailWhite { get; set; }
        public bool MailGreen { get; set; }
        public bool MailBlue { get; set; }
        public bool MailPurple { get; set; }
        public string MinFreeBagSlots { get; set; }
        public string MinDurability { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }
        public string FileName = "";
        #endregion
        
        private XDocument doc = new XDocument();     
        public EclipseProfile(string filePath = "")
        {
            PosX = 0.ToString();
            PosY = 0.ToString();
            PosZ = 0.ToString();

            MinLevel = 0.ToString();
            MaxLevel = 0.ToString();

            Name = string.Empty;
            QuestOverrides = new List<QuestOverride>();
            BlackSpots = new List<Blackspot>();
            AvoidMobs = new List<EclipseMob>();
            QuestOrders = new List<QuestOrder>();
            Vendors = new List<EclipseVendor>();
            MailBoxes = new List<MailBox>();
            CustomBehaviors = new List<CustomBehavior>();
            LogicBlocks = new List<QuestOrderLogic>();

            if (filePath != string.Empty && File.Exists(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                doc = XDocument.Load(filePath);
                FileName = file.Name;
                GetSettings();
                GetBlackspots();
                GetAvoids();
                GetQuestOverrides();
                GetQuestOrder();
                GetVendors();
                GetMailBoxes();
            }
        }
        public bool UpdateNodebyName(string name, string value)
        {
            doc.Element("HBProfile").SetElementValue(name, value);
            return true;
        }
        public void Save()
        {
            DAL.DBFile = @"C:\Users\Twist\Documents\Honorbuddy\Bots\SkinbotV2\SkinbotV2\Data\EclipseWoWDB.edb";
            foreach (var q in QuestOrders){
                DataTable result = DAL.LoadSL3Data(string.Format("Select * from questorders where questid = '{0}'", q.QuestId));
                if (result != null) 
                {
                    if (result.Rows.Count > 0) //This one already exists.
                    {
                        EC.Log("Quest already in database - Updating");
                        QuestOrder qo = (QuestOrder)ORM.convertDataRowtoObject(new QuestOrder(),result.Rows[0]);
                        if (qo.TurnInId != null && qo.TurnInId != string.Empty) q.TurnInId = qo.TurnInId;
                        if (qo.GiverId != null && qo.GiverId != string.Empty) q.GiverId = qo.GiverId;
                        if (qo.KillCount != null) q.KillCount = qo.KillCount;
                        if (qo.MobId != null && qo.MobId != string.Empty) q.MobId = qo.MobId;
                        if (qo.CollectCount != null) q.CollectCount = qo.CollectCount;
                        if (qo.ItemId != null && qo.ItemId != string.Empty) q.ItemId = qo.ItemId;
                        EC.Log("Quest already in database");
                        var sql = ORM.Update(q, "questorders","", DAL.getTableStructure("questorders"));
                        DAL.ExecuteSL3Query(sql);
                    }
                    else
                    {
                        var sql = ORM.Insert(q, "questorders", "", false, DAL.getTableStructure("questorders"));
                        DAL.ExecuteSL3Query(sql);
                    }
                }
            }
            foreach (var v in Vendors)
            {
                var sql = ORM.Insert(v, "Vendors", "", false, DAL.getTableStructure("questorders"));
                DAL.ExecuteSL3Query(sql);
            }
            var mailboxstructure = DAL.getTableStructure("MailBoxes");
            foreach (var v in MailBoxes)
            {
                var sql = ORM.Insert(v, "MailBoxes", "", false, mailboxstructure);
                DAL.ExecuteSL3Query(sql);
            }
        }
        
        #region ParseProfileData
        private void GetSettings()
        {
            if (doc.Element("HBProfile").Descendants("Name").FirstOrDefault() != null) Name = doc.Element("HBProfile").Descendants("Name").FirstOrDefault().Value;
            if (doc.Element("HBProfile").Descendants("MinLevel").FirstOrDefault() != null) MinLevel = doc.Element("HBProfile").Descendants("MinLevel").FirstOrDefault().Value;
            if (doc.Element("HBProfile").Descendants("MaxLevel").FirstOrDefault() != null) MaxLevel = doc.Element("HBProfile").Descendants("MaxLevel").FirstOrDefault().Value;
            if (doc.Element("HBProfile").Descendants("MinFreeBagSlots").FirstOrDefault() != null) MinFreeBagSlots = doc.Element("HBProfile").Descendants("MinFreeBagSlots").FirstOrDefault().Value;
            if (doc.Element("HBProfile").Descendants("MinDurability").FirstOrDefault() != null) MinDurability = doc.Element("HBProfile").Descendants("MinDurability").FirstOrDefault().Value;

            if (doc.Element("HBProfile").Descendants("SellGrey").FirstOrDefault() != null) SellGrey = bool.Parse(doc.Element("HBProfile").Descendants("SellGrey").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("SellWhite").FirstOrDefault() != null) SellWhite = bool.Parse(doc.Element("HBProfile").Descendants("SellWhite").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("SellGreen").FirstOrDefault() != null) SellGreen = bool.Parse(doc.Element("HBProfile").Descendants("SellGreen").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("SellBlue").FirstOrDefault() != null) SellBlue = bool.Parse(doc.Element("HBProfile").Descendants("SellBlue").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("SellPurple").FirstOrDefault() != null) SellPurple = bool.Parse(doc.Element("HBProfile").Descendants("SellPurple").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailGrey").FirstOrDefault() != null) MailGrey = bool.Parse(doc.Element("HBProfile").Descendants("MailGrey").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailWhite").FirstOrDefault() != null) MailWhite = bool.Parse(doc.Element("HBProfile").Descendants("MailWhite").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailGreen").FirstOrDefault() != null) MailGreen = bool.Parse(doc.Element("HBProfile").Descendants("MailGreen").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailBlue").FirstOrDefault() != null) MailBlue = bool.Parse(doc.Element("HBProfile").Descendants("MailBlue").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailPurple").FirstOrDefault() != null) MailPurple = bool.Parse(doc.Element("HBProfile").Descendants("MailPurple").FirstOrDefault().Value);
        }
        private void GetMailBoxes()
        {
            if (doc.Element("HBProfile").Element("Mailboxes") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("Mailboxes").FirstOrDefault().DescendantNodes();
                if (nodeTree != null)
                {
                    var icount = 0;
                    foreach (XElement ele in nodeTree.Where(i => i.NodeType != System.Xml.XmlNodeType.Comment))
                    {
                        MailBox mb = new MailBox { X = ele.Attribute("X").Value, Y = ele.Attribute("Y").Value, Z = ele.Attribute("Z").Value};
                        if (ele.Attribute("Name") != null) mb.Name = ele.Attribute("Name").Value;
                        else mb.Name = "MailBox" + icount;
                        MailBoxes.Add(mb);
                    }
                }
            }
        }
        public void GetBlackspots()
        {
            if (doc.Element("HBProfile").Element("Blackspots") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("Blackspots").FirstOrDefault().DescendantNodes();
                if (nodeTree != null)
                {
                    var icount = 0;
                    foreach (XElement ele in nodeTree.Where(i => i.NodeType != System.Xml.XmlNodeType.Comment))
                    {
                        Blackspot bs = new Blackspot { X = ele.Attribute("X").Value, Y = ele.Attribute("Y").Value, Z = ele.Attribute("Z").Value, Radius = ele.Attribute("Radius").Value };
                        if (ele.Attribute("Name") != null) bs.Name = ele.Attribute("Name").Value;
                        else bs.Name = "BlackSpot" + icount;
                        BlackSpots.Add(bs);
                    }
                }
            }
        }
        public void GetVendors()
        {
            if (doc.Element("HBProfile").Element("Vendors") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("Vendors").FirstOrDefault().DescendantNodes();
                if (nodeTree != null)
                {
                    foreach (XElement ele in nodeTree.Where(i => i.NodeType != System.Xml.XmlNodeType.Comment))
                    {
                        //EclipseVendor vendor = new EclipseVendor { X = ele.Attribute("X").Value.Replace(",", "."), Y = ele.Attribute("Y").Value.Replace(",", "."), Z = ele.Attribute("Z").Value.Replace(",", "."), Type = ele.Attribute("Type").Value, Name = ele.Attribute("Name").Value };
                       // Vendors.Add(vendor);
                    }
                }
            }
        }
        public void GetQuestOverrides()
        {
            if (doc.Element("HBProfile").Element("Quest") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("Quest").ToList();
                if (nodeTree != null)
                {
                    foreach (XElement ele in nodeTree)
                    {
                        QuestOverride questOR = new QuestOverride { Id=ele.Attribute("Id").Value };
                        if (ele.Attribute("Name") != null) questOR.Name = ele.Attribute("Name").Value;
                        if (ele.Attribute("NName") != null) questOR.Name = ele.Attribute("NName").Value;
                        


                        if (ele.Descendants("Objectives") != null)
                        {
                            var obj = ele.Descendants("Objective").FirstOrDefault();
                            if (obj == null) continue;
                            var type = "";
                            QuestObjective.QuestType qtype = QuestObjective.QuestType.CollectItem;
                            if (obj.Attribute("Type").Value != null) type = obj.Attribute("Type").Value;
                            if (type == "CollectItem") qtype = QuestObjective.QuestType.CollectItem;
                            if (type == "KillMob") qtype = QuestObjective.QuestType.KillMob;
                            


                            if (qtype == QuestObjective.QuestType.CollectItem)
                            {
                                QuestORItem qo = new QuestORItem { Type = qtype };

                                if (obj.Attribute("ObjectId") != null) qo.ItemId = obj.Attribute("ObjectId").Value;
                                if (obj.Attribute("ItemId") != null) qo.ItemId = obj.Attribute("ItemId").Value;
                                if (obj.Attribute("MobId") != null) qo.ItemId = obj.Attribute("MobId").Value;
                                if (obj.Attribute("CollectCount") != null) qo.CollectCount = obj.Attribute("CollectCount").Value;
                                if (obj.Attribute("CollectionCount") != null) qo.CollectCount = obj.Attribute("CollectionCount").Value;
                                qo.HotSpots.AddRange(getHotSpots(obj).ToList());
                                qo.CollectFrom.AddRange(getCollectFroms(obj));
                                questOR.Objectives.Add(qo);
                            }
                            if (qtype == QuestObjective.QuestType.KillMob)
                            {
                                
                                QuestORMob qo = new QuestORMob { Type = qtype };
                                
                                //This is because of inconsistency in the profiles.
                                if (obj.Attribute("MobId") == null && obj.Attribute("ItemId") != null) qo.MobId = obj.Attribute("ItemId").Value;
                                if (obj.Attribute("MobId") != null) qo.KillCount = obj.Attribute("MobId").Value;

                                if (obj.Attribute("KillCount") == null && obj.Attribute("CollectCount") != null) qo.KillCount = obj.Attribute("CollectCount").Value;
                                if (obj.Attribute("KillCount") != null) qo.KillCount = obj.Attribute("KillCount").Value;
                                getHotSpots(obj);
                                qo.HotSpots.AddRange(getHotSpots(obj));
                                qo.CollectFrom.AddRange(getCollectFroms(obj));
                                questOR.Objectives.Add(qo);
                            }
                        }
                        QuestOverrides.Add(questOR);
                    }
                }
            }
        }
        private IEnumerable<Eclipse.MultiBot.Data.HotSpot> getHotSpots(XElement obj)
        {
            var ele = obj.Descendants().ToList();
            List<Eclipse.MultiBot.Data.HotSpot> spots = new List<Eclipse.MultiBot.Data.HotSpot>();
            if (ele != null)
            {
                int iCount = 0;
                
                foreach (var spot in ele.Where(e=>e.Name == "Hotspot").ToList())
                {
                    iCount++;
                    Eclipse.MultiBot.Data.HotSpot hs = new Eclipse.MultiBot.Data.HotSpot ();
                    if (spot.Attribute("X") != null) hs.X = float.Parse(spot.Attribute("X").Value);
                    if (spot.Attribute("Y") != null) hs.Y =  float.Parse(spot.Attribute("Y").Value);
                    if (spot.Attribute("Z") != null) hs.Z =  float.Parse(spot.Attribute("Z").Value);
                    if (spot.Attribute("x") != null) hs.X =  float.Parse(spot.Attribute("x").Value);
                    if (spot.Attribute("y") != null) hs.Y =  float.Parse(spot.Attribute("y").Value);
                    if (spot.Attribute("z") != null) hs.Z = float.Parse(spot.Attribute("z").Value);
                    if (spot.Attribute("Name") == null) hs.Name = "Spot" + iCount;
                    else hs.Name = spot.Attribute("Name").Value;
                    spots.Add(hs);
                }
            }
            return spots;            
        }
        private IEnumerable<EclipseGeneric> getCollectFroms(XElement obj)
        {
            var ele = obj.Descendants().ToList();
            List<EclipseGeneric> collect = new List<EclipseGeneric>();
            if (ele != null)
            {
                foreach (var item in ele.Where(e => e.Name == "GameObject" || e.Name == "Mob").ToList())
                {
                    if (item.Name == "GameObject")
                    {
                        EclipseObject eo = new EclipseObject ();
                        if (item.Attribute("Name") != null) eo.Name = item.Attribute("Name").Value;
                        if (item.Attribute("ID") != null) eo.Id = item.Attribute("ID").Value;
                        if (item.Attribute("Id") != null) eo.Id = item.Attribute("Id").Value;
                        collect.Add(eo);
                    }
                    if (item.Name == "Mob")
                    {
                        EclipseMob eo = new EclipseMob { };
                        if (item.Attribute("Id") != null) eo.Id = item.Attribute("Id").Value;
                        if (item.Attribute("Entry") != null) eo.Id = item.Attribute("Entry").Value;
                        if (item.Attribute("Name") != null) eo.Name = item.Attribute("Name").Value;
                        collect.Add(eo);
                    }
                }
            }
            return collect;
        }
        public void GetAvoids(){
            if (doc.Element("HBProfile").Element("AvoidMobs") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("AvoidMobs").FirstOrDefault().DescendantNodes();
                if (nodeTree != null)
                {
                    foreach (XElement ele in nodeTree.Where(i => i.NodeType != System.Xml.XmlNodeType.Comment))
                    {
                        EclipseMob mob = new EclipseMob { Name = ele.FirstAttribute.Value, Entry = ele.LastAttribute.Value };
                        AvoidMobs.Add(mob);
                    }
                }
            }
        }
        public void GetQuestOrder()
        {

            var questorderItems = doc.Element("HBProfile").Element("QuestOrder");
            if (questorderItems != null)
            {
                if (questorderItems.HasElements) GetQuestOrderDescendants(questorderItems);
            }
        }
        public void GetQuestOrderDescendants(XElement root){
            var desc = root.Elements().ToList();
            foreach (var n in desc)
            {
                var i  = (XElement)n;
                if (i.Name == "PickUp")
                {
                    QuestOrder qo = new QuestOrder { QuestName = i.Attribute("QuestName").Value, QuestId = uint.Parse(i.Attribute("QuestId").Value), GiverId = i.Attribute("GiverId").Value, type = QuestOrder.QOType.PickUp };
                    if (i.Attribute("GiverName") != null) qo.GiverName = i.Attribute("GiverName").Value;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "UseItem")
                {
                    QuestOrder qo = new QuestOrder { QuestId = uint.Parse(i.Attribute("QuestId").Value), ItemId = i.Attribute("ItemId").Value,  X = double.Parse(i.Attribute("X").Value), Y = double.Parse(i.Attribute("Y").Value), Z = double.Parse(i.Attribute("Z").Value), type = QuestOrder.QOType.UseItem };
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (i.Attribute("QuestId") != null) qo.QuestName = i.Attribute("QuestId").Value;
                    if (i.Attribute("NumOfTimes") != null) qo.QuestName = i.Attribute("NumOfTimes").Value;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "RunTo")
                {
                    QuestOrder qo = new QuestOrder { type = QuestOrder.QOType.RunTo };
                    if (i.Attribute("QuestId") != null) qo.QuestName = i.Attribute("QuestId").Value;
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (i.Attribute("DestName") != null) qo.QuestName = i.Attribute("DestName").Value;
                    if (i.Attribute("X") != null) qo.X = double.Parse(i.Attribute("X").Value);
                    if (i.Attribute("Y") != null) qo.Y = double.Parse(i.Attribute("Y").Value);
                    if (i.Attribute("Z") != null) qo.Z = double.Parse(i.Attribute("Z").Value);
                    if (i.Attribute("x") != null) qo.X = double.Parse(i.Attribute("x").Value);
                    if (i.Attribute("y") != null) qo.Y = double.Parse(i.Attribute("y").Value);
                    if (i.Attribute("z") != null) qo.Z = double.Parse(i.Attribute("z").Value);
                    QuestOrders.Add(qo);
                }
                if (i.Name == "FlyTo")
                {
                    QuestOrder qo = new QuestOrder { QuestName = i.Attribute("QuestName").Value, QuestId = uint.Parse(i.Attribute("QuestId").Value), X = double.Parse(i.Attribute("X").Value), Y = double.Parse(i.Attribute("Y").Value), Z = double.Parse(i.Attribute("Z").Value), type = QuestOrder.QOType.RunTo };
                    QuestOrders.Add(qo);
                }
                if (i.Name == "CustomBehavior")
                {
                    CustomBehavior cb = new CustomBehavior { type = QuestOrder.QOType.CustomBehavior };
                    foreach (var att in i.Attributes().ToList())
                    {
                        if (att.Name != "File") cb.attributes.Add(att.Name.ToString(), att.Value.ToString());
                        else cb.file = att.Value.ToString();
                    }
                    CustomBehaviors.Add(cb);
                    QuestOrders.Add(cb);
                }
                if (i.Name == "Objective")
                {
                    var val = i.Attribute("Type").Value;
                    if (val == "Collect") val = "CollectItem";
                    if (val == "CollectItem") val = "CollectItem";
                    if (val == "UseItem" || val == "UseObject") continue;
                    QuestObjective.QuestType objectiveType = QuestObjective.QuestType.CollectItem;
                    Enum.TryParse(val, true, out objectiveType);
                    QuestOrder qo = new QuestOrder { objectiveType = objectiveType, type = QuestOrder.QOType.Objective };
                    if (i.Attribute("Questid") != null) qo.QuestId = uint.Parse(i.Attribute("Questid").Value);
                    if (i.Attribute("QuestId") != null) qo.QuestId = uint.Parse(i.Attribute("QuestId").Value);
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (objectiveType == QuestObjective.QuestType.CollectItem)
                    {
                        qo.ItemId = i.Attribute("ItemId").Value;
                        qo.CollectCount = int.Parse(i.Attribute("CollectCount").Value);
                    }
                    if (objectiveType == QuestObjective.QuestType.KillMob)
                    {
                        qo.MobId = i.Attribute("MobId").Value;
                        qo.KillCount = int.Parse(i.Attribute("KillCount").Value);
                    }
                    QuestOrders.Add(qo);
                }
                if (i.Name == "TurnIn")
                {
                    QuestOrder qo = new QuestOrder { type = QuestOrder.QOType.TurnIn };
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (i.Attribute("QuestId") != null) qo.QuestId = uint.Parse(i.Attribute("QuestId").Value);
                    if (i.Attribute("TurnInId") != null) qo.TurnInId = i.Attribute("TurnInId").Value;
                    if (i.Attribute("TurnInId") == null) ProfileLoadErrors += string.Format("Missing TurnInId: {0} \r\n", i.Value);
                    if (i.Attribute("TurnInId") != null && i.Attribute("TurnInId").Value == "") ProfileLoadErrors += string.Format("Missing TurnInId: {0} \r\n", i.Value);
                    if (i.Attribute("TurnInName") != null) qo.TurnInName = i.Attribute("TurnInName").Value;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "If" || i.Name == "While" || i.Name == "ElseIf")
                {
                    QuestOrderLogic qo = new QuestOrderLogic { type = QuestOrder.QOType.LogicBlock, LogicType = i.Name.ToString(), StartTag = true };
                    if (i.Attribute("Condition") != null) qo.Condition = i.Attribute("Condition").Value;
                    QuestOrders.Add(qo);
                    if (i.HasElements) GetQuestOrderDescendants(i);
                    qo = new QuestOrderLogic { type = QuestOrder.QOType.LogicBlock, LogicType = i.Name.ToString(), StartTag = false };
                    if (i.Attribute("Condition") != null) qo.Condition = i.Attribute("Condition").Value;
                    QuestOrders.Add(qo);
                    EclipseProfile.LogicBlocks.Add(qo);
                }

            }
        }
        #endregion

    }
}
