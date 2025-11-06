using Inventory;
using Scripts.StatSystem;
using Work.Common.Core;

namespace Work.Item
{
    public static class ItemEventChannel
    {
        public static readonly EnchantSelectEvent EnchantSelectEvent = new();
    }

    public class EnchantSelectEvent : GameEvent
    {
        public EquipableItemSO item;

        public EnchantSelectEvent Initializer(EquipableItemSO item)
        {
            this.item = item;
            return this;
        }
    }
}