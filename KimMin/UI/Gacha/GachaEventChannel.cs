using Inventory;
using Work.Common.Core;

namespace Work.UI.Gacha
{
    public static class GachaEventChannel
    {
        public static GachaEvent GachaEvent = new GachaEvent();
    }

    public class GachaEvent : GameEvent
    {
        public ItemDataSO item;

        public GachaEvent Initializer(ItemDataSO item)
        {
            this.item = item;
            return this;
        }
    }
}