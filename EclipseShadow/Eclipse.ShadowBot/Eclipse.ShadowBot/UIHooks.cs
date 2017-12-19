using Eclipse.Comms;
using Styx;
using Styx.CommonBot.CharacterManagement;
using Styx.CommonBot.Frames;
using Styx.CommonBot.Inventory;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.ShadowBot
{

    public static class UIHooks
    {
        public static uint QuestContextId = 0;
        #region Attach and Detach LUA Events
        // Parts of This class were jacked from FPSWare's abandoned RAF2 Botbase - some of it was used as a reference - ALL of it needed to be fixed to work with the new HB. 
        // Some of FPSWare's code was also jacked from isntance buddy before it became part of HB's built in bots.
        public static void AttachUIEvents()
        {
            EC.Log("Attaching quest events.");
            Lua.Events.AttachEvent("QUEST_GREETING", OnQuestGreeting);
            Lua.Events.AttachEvent("QUEST_DETAIL", OnQuestDetail);
            Lua.Events.AttachEvent("GOSSIP_SHOW", OnGossipShow);
            Lua.Events.AttachEvent("QUEST_COMPLETE", OnQuestComplete);
            Lua.Events.AttachEvent("QUEST_PROGRESS", OnQuestProgress);

            Lua.Events.AttachEvent("PARTY_INVITE_REQUEST", OnPartyInvite);
            Lua.Events.AttachEvent("LFG_ROLE_CHECK_SHOW", OnLfgRoleCheck);
            Lua.Events.AttachEvent("LFG_PROPOSAL_SHOW", OnLfgProposalShow);
            Lua.Events.AttachEvent("RESURRECT_REQUEST", OnRecurrectionHandler);
            Lua.Events.AttachEvent("READY_CHECK", OnReadyCheckHandler);
        }
        public static void DetatchUIEvents()
        {
            EC.Log("Deattaching quest events.");
            Lua.Events.DetachEvent("QUEST_GREETING", OnQuestGreeting);
            Lua.Events.DetachEvent("QUEST_DETAIL", OnQuestDetail);
            Lua.Events.DetachEvent("GOSSIP_SHOW", OnGossipShow);
            Lua.Events.DetachEvent("QUEST_COMPLETE", OnQuestComplete);
            Lua.Events.DetachEvent("QUEST_PROGRESS", OnQuestProgress);
        }
        #endregion
        
        #region Lua Event Handlers
        public static void OnQuestProgress(object obj, LuaEventArgs args)
        {
            EC.Log("Turning in a quest ...");
            Lua.DoString("RunMacroText(\"/click QuestFrameCompleteButton\")");
        }
        public static void OnQuestDetail(object obj, LuaEventArgs args)
        {
            var i = QuestFrame.Instance.CurrentShownQuest;
            QuestContextId = StyxWoW.Me.CurrentTarget.Entry;
            QuestFrame.Instance.AcceptQuest();
            EC.Log("Accepted a Quest..." + i.Name);
        }
        public static void OnQuestGreeting(object obj, LuaEventArgs args)
        {
            int activeQuests = QuestFrame.Instance.AvailableQuests.Count;
            EC.Log("Accepting a quest ...");
            QuestFrame.Instance.SelectAvailableQuest(0);


            // Get a list of active quests from the currently open window
            List<GossipQuestEntry> gossipQuests = QuestFrame.Instance.IsVisible ? QuestFrame.Instance.ActiveQuests : GossipFrame.Instance.ActiveQuests;

            // Check if the q is complete against our q logs
            GossipQuestEntry quest = (from entry in gossipQuests
                                      let pQuest = StyxWoW.Me.QuestLog.GetQuestById((uint)entry.Id)
                                      where pQuest != null && pQuest.IsCompleted
                                      select entry).FirstOrDefault();

            // We have a valid q we can turn in to this q giver!
            if (quest == null) return;

            // DO IT BITCH!
            QuestFrame.Instance.SelectActiveGossipQuest(quest.Index);
            EC.Log("====== it appeared to work, right?");
        }
        public static Styx.TreeSharp.Action ClickRepair()
        {
            return new Styx.TreeSharp.Action(context => Lua.DoString("RepairAllItems(1)"));
        }
        public static Styx.TreeSharp.Action ClickGuildRepair()
        {
            return new Styx.TreeSharp.Action(context => Lua.DoString("RepairAllItems()"));
        }
        public static void OnGossipShow(object obj, LuaEventArgs args)
        {
            EC.Log("===== OnGossipShow");


            // Get a list of active quests from the currently open window
            List<GossipQuestEntry> gossipQuests = QuestFrame.Instance.IsVisible ? QuestFrame.Instance.ActiveQuests : GossipFrame.Instance.ActiveQuests;

            // Check if the q is complete against our q logs
            GossipQuestEntry quest = (from entry in gossipQuests
                                      let pQuest = StyxWoW.Me.QuestLog.GetQuestById((uint)entry.Id)
                                      where pQuest != null && pQuest.IsCompleted
                                      select entry).FirstOrDefault();

            // We have a valid q we can turn in to this q giver!
            if (quest != null)
            {
                // DO IT BITCH!
                GossipFrame.Instance.SelectActiveGossipQuest(quest.Index);
            }

            EC.Log("Picking up a q");

            if (QuestFrame.Instance.IsVisible)
            {
                if (QuestFrame.Instance.AvailableQuests.Count > 0) QuestFrame.Instance.SelectAvailableQuest(0);
            }
            else if (GossipFrame.Instance.IsVisible)
            {
                if (GossipFrame.Instance.AvailableQuests.Count > 0) GossipFrame.Instance.SelectAvailableQuest(0);
            }

        }
        public static void OnQuestComplete(object obj, LuaEventArgs args)
        {
            EC.Log("===== OnQuestComplete");

            int countOfQuestRewards = Lua.GetReturnVal<int>("return GetNumQuestChoices()", 0);
            EC.Log("countOfQuestRewards: " + countOfQuestRewards);

            // More than 1 reward to choose from
            if (countOfQuestRewards > 1)
            {
                float bestOverallItemScore = Single.MinValue;
                int bestOverallItemChoice = -1;
                WeightSet classWeightSet = CharacterManager.CurrentClassProfile.WeightSet;
                var EquippedItems = StyxWoW.Me.Inventory.Equipped.Items;
                string bestQuestReward = string.Empty;
                // Enumerate q rewards to find the best one
                for (int i = 1; i <= countOfQuestRewards; i++)
                {
                    string itemLink = Lua.GetReturnVal<string>("return GetQuestItemLink(\"choice\", " + i.ToString(CultureInfo.InvariantCulture) + ")", 0);
                    string[] splitted = itemLink.Split(':');

                    uint itemId;
                    if (String.IsNullOrEmpty(itemLink) || (splitted.Length == 0 || splitted.Length < 2) || (!UInt32.TryParse(splitted[1], out itemId) || itemId == 0))
                    {
                        EC.Log("Parsing ItemLink for q item failed!");
                        EC.Log(string.Format("ItemLink: {0}", itemLink));
                        continue;
                    }
                    else
                    {
                        EC.Log("itemId is good:" + itemId);
                    }

                    ItemInfo choiceItemInfo = ItemInfo.FromId(itemId);
                    if (choiceItemInfo == null)
                    {
                        EC.Log("Retrieving item info for reward item failed");
                        EC.Log(String.Format("Item Id:{0} ItemLink:{1}", itemId, itemLink));
                        continue;
                    }
                    else
                    {
                        EC.Log("choiceItemInfo is good:" + choiceItemInfo.Name);
                    }

                    // Score of the item.
                    float choiceItemScore = classWeightSet.EvaluateItem(choiceItemInfo,ItemStats.FromLink(itemLink),true);

                    // Score the equipped item if any. otherwise 0
                    float bestEquipItemScore = Single.MinValue;

                    // The best slot
                    InventorySlot bestSlot = InventorySlot.None;
                    List<InventorySlot> inventorySlots = InventoryManager.GetInventorySlotsByEquipSlot(choiceItemInfo.EquipSlot);

                    foreach (InventorySlot slot in inventorySlots)
                    {
                        var newslot = choiceItemInfo.EquipSlot;
                        EC.Log("foreach inventoryslots");
                        WoWItem equipped = EquippedItems[(int)slot];
                        if (equipped == null) continue;

                        bestEquipItemScore = classWeightSet.EvaluateItem(equipped, true);
                        bestSlot = slot;
                    }

                    if (bestEquipItemScore > Single.MinValue)
                    {
                        EC.Log(string.Format("Equipped Item {0} scored {1}", EquippedItems[(int)bestSlot].Name, bestEquipItemScore.ToString(CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        EC.Log("bestEquipItemScore < Single.MinValue");
                    }


                    // If this our armor type?
                    bool goodArmor = choiceItemInfo.ItemClass == WoWItemClass.Armor && (choiceItemInfo.ArmorClass == EC.GetArmorClass());// || miscArmorType.Contains(choiceItemInfo.InventoryType));
                    EC.Log("goodArmor: " + goodArmor);

                    // Now we assign the overall score of this item
                    // 1. Is it a higher score than our equipped item?
                    // 2. Is this armor type good for us?
                    if (choiceItemScore > bestEquipItemScore && bestSlot != InventorySlot.None && (goodArmor && (choiceItemScore - bestEquipItemScore) > bestOverallItemScore))
                    {
                        bestOverallItemScore = (choiceItemScore - bestEquipItemScore);
                        bestOverallItemChoice = i;
                        EC.Log("choiceItemScore and other stuff SUCCEEDED");
                        bestQuestReward = choiceItemInfo.Name;

                    }
                    else
                    {
                        EC.Log("choiceItemScore and other stuff failed");
                    }
                    
                }
               
                EC.Log("finished enumerating q rewards...");

                // We have not found a replacement item, so choose the most value item for vendoring
                if (bestOverallItemScore == Single.MinValue)
                {
                    EC.Log("No upgrades available, choosing most valuable item for vendoring");
                    ItemInfo highestItemInfo = null;

                    for (int i = 1; i <= countOfQuestRewards; i++)
                    {
                        string itemLink = Lua.GetReturnVal<string>("return GetQuestItemLink(\"choice\", " + i.ToString(CultureInfo.InvariantCulture) + ")", 0);
                        string[] splitted = itemLink.Split(':');

                        uint itemId;
                        if (String.IsNullOrEmpty(itemLink) || (splitted.Length == 0 || splitted.Length < 2) || (!UInt32.TryParse(splitted[1], out itemId) || itemId == 0))
                        {
                            EC.Log("Parsing ItemLink failed!");
                            EC.Log(string.Format("ItemLink: {0}", itemLink));
                            continue;
                        }

                        ItemInfo choiceItemInfo = ItemInfo.FromId(itemId);
                        if (choiceItemInfo == null)
                        {
                            EC.Log("Retrieving item info failed!");
                            EC.Log(string.Format("Item Id:{0} ItemLink:{1}", itemId, itemLink));
                            continue;
                        }

                        if (highestItemInfo == null || highestItemInfo.SellPrice > choiceItemInfo.SellPrice)
                        {
                            highestItemInfo = choiceItemInfo;
                            bestOverallItemChoice = i;
                        }
                    }
                }

                // Select the best q reward
                //if (bestOverallItemChoice > 0)
                if (bestOverallItemChoice > 0)
                {
                    EC.Log("bestOverallItemChoice > 0 and it is: " + bestOverallItemChoice);
                    //EC.Log("QuestFrame.Instance.IsVisible: " + QuestFrame.Instance.IsVisible);
                    QuestFrame.Instance.SelectQuestReward(bestOverallItemChoice - 1);
                }
                else
                {
                    EC.Log("Inside OnQuestComplete. We failed to choose a q reward, either for upgrade or vendoring.");
                    QuestFrame.Instance.SelectQuestReward(0);
                }

            }
            QuestFrame.Instance.SelectQuestReward(0);
            QuestFrame.Instance.CompleteQuest();

        }
        #endregion
        private static void OnPartyInvite(object sender, LuaEventArgs args)
        {
            string invitedBy = args.Args[0].ToString();
            CommsCommon.Log(string.Format("Accepting group invite from {0}", invitedBy));
            Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\")");
        }


        private static void OnReadyCheckHandler(object sender, LuaEventArgs args)
        {
            CommsCommon.Log("Ready check has been initiated by " + args.Args[0]);
            Lua.DoString("RunMacroText(\"/click ReadyCheckFrameYesButton\")");
        }

        private static void OnRecurrectionHandler(object sender, LuaEventArgs args)
        {
            CommsCommon.Log("Someone wants to rez you...");
            Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\")");
        }

        public static void OnLfgProposalShow(object obj, LuaEventArgs args)
        {
            //Log.Info("OnLfgProposalShow .... ");
            CommsCommon.Log("The que popped!");
            Lua.DoString("AcceptProposal()");
        }

        public static void OnLfgRoleCheck(object obj, LuaEventArgs args)
        {

            CommsCommon.Log("Accepting role popup");
            Lua.DoString("CompleteLFGRoleCheck(true)");

        }
    }
}
