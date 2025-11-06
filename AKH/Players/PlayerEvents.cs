using Scripts.Network;
using System;
using Work.Common.Core;
using Work.Upgrade;

namespace Scripts.Players
{
    public static class PlayerEvents
    {
        public static readonly UpgradeStatEvent UpgradeStatEvent = new();
    }
    public class UpgradeStatEvent : GameEvent
    {
        public UpgradeDataSO upgradeData;
        public Action<bool> callback;
        public UpgradeStatEvent Init(UpgradeDataSO dataSO,Action<bool> callback)
        {
            this.upgradeData = dataSO;
            this.callback = callback;
            return this;
        }
    }
}
