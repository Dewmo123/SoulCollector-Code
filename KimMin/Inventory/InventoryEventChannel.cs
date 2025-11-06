using Scripts.PlayerEquipments.SkillSystem;
using System;
using UI.Inventory;
using Work.Common.Core;
using Work.Item;

namespace Inventory
{
    public static class InventoryEventChannel
    {
        public static SelectInventoryItemEvent SelectInventoryItemEvent = new();
        public static AddInventoryEvent AddInventoryEvent = new();
        public static AddBuddyEvent AddBuddyEvent = new();
        public static EquipSkillEvent EquipSkillEvent = new();
        public static AddSkillAmountEvent AddSkillAmountEvent = new();
        public static AddPartnerAmountEvent AddPartnerAmountEvent = new();
        public static LevelUpSkillEvent LevelUpSkillEvent = new();
        public static LevelUpPartnerEvent LevelUpPartnerEvent = new();
    }
    public class LevelUpSkillEvent : GameEvent
    {
        public EquipableItemSO targetItem;
        public Action<ItemDataSO,bool> callback;
        public LevelUpSkillEvent Init(EquipableItemSO targetItem,Action<ItemDataSO,bool>callback)
        {
            this.targetItem = targetItem;
            this.callback = callback;
            return this;
        }
    }
    public class LevelUpPartnerEvent : GameEvent
    {
        public EquipableItemSO targetItem;
        public Action<ItemDataSO,bool> callback;
        public LevelUpPartnerEvent Init(EquipableItemSO targetItem,Action<ItemDataSO,bool>callback)
        {
            this.targetItem = targetItem;
            this.callback = callback;
            return this;
        }
    }
    public class SelectInventoryItemEvent : GameEvent
    {
        public InventorySlotUI item;

        public SelectInventoryItemEvent Initializer(InventorySlotUI item)
        {
            this.item = item;
            return this;
        }
    }
    public class AddInventoryEvent : GameEvent
    {
        public ItemDataSO item;


        public AddInventoryEvent Initializer(ItemDataSO item)
        {
            this.item = item;
            return this;
        }
    }
    public class AddBuddyEvent : GameEvent
    {
        public int index;
        public BuddySO data;

        public AddBuddyEvent Initializer(BuddySO data, int index)
        {
            this.index = index;
            this.data = data;
            return this;
        }
    }
    public class EquipSkillEvent : GameEvent
    {
        public int index;
        public SkillDataSO data;

        public EquipSkillEvent Initializer(SkillDataSO data, int index)
        {
            this.index = index;
            this.data = data;
            return this;
        }
    }
    public class AddSkillAmountEvent : GameEvent
    {
        public SkillDataSO skill;
        public int amount;
        public AddSkillAmountEvent Init(SkillDataSO skill,int amount)
        {
            this.amount = amount;
            this.skill = skill;
            return this;
        }
    }
    public class AddPartnerAmountEvent : GameEvent
    {
        public BuddySO partner;
        public int amount;
        public AddPartnerAmountEvent Init(BuddySO partner, int amount)
        {
            this.amount = amount;
            this.partner = partner;
            return this;
        }
    }
}