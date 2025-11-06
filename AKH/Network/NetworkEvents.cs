using Work.Common.Core;

namespace Scripts.Network
{
    public static class NetworkEvents
    {
        public readonly static ChangeStatEvent ChangeStatEvent = new();
        public readonly static ChangeGoodsEvent ChangeGoodsEvent = new();
    }
    public class ChangeStatEvent : GameEvent
    {
        public StatType statType;
        public int prev,next;
        public ChangeStatEvent Init(StatType statType, int prev,int next)
        {
            this.statType = statType;
            this.prev = prev;
            this.next = next;
            return this;
        }
    }
    public class ChangeGoodsEvent : GameEvent
    {
        public GoodsType goodsType;
        public int prev, next;

        public ChangeGoodsEvent Init(GoodsType goodsType, int prev, int next)
        {
            this.goodsType = goodsType;
            this.prev = prev;
            this.next = next;
            return this;
        }
    }
}
