using Inventory;
using Scripts.Players;
using Scripts.Players.Storages;
using System;
using Work.Common.Core;
using Work.Core;
using Work.Item;
using Random = UnityEngine.Random;

namespace Work.UI.Enchant
{
    public class EnchantModel
    {
        public Action<ItemDataSO, bool> OnEnchant;
        public void TryEnchant(ItemDataSO item, float chance)
        {
            if (item is not EquipableItemSO equipable)
                return;
            if (Random.value < chance)
            {
                switch (item.itemType)
                {
                    case ItemType.Skill:
                        GameEventBus.RaiseEvent(InventoryEventChannel.LevelUpSkillEvent.Init(equipable, OnEnchant));
                        break;
                    case ItemType.Buddy:
                        GameEventBus.RaiseEvent(InventoryEventChannel.LevelUpPartnerEvent.Init(equipable, OnEnchant));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                OnEnchant?.Invoke(item, false);
            }
        }
    }
}