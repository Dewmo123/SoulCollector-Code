using Scripts.Entities;
using Scripts.Players;
using Scripts.StatSystem;
using System;
using Work.Common.Core;
using Work.Upgrade;

namespace Work.UI
{
    public interface IUpgradeModel
    {
        event Action<UpgradeDataSO,StatSO, int> OnUpdateStat;
        void Upgrade();
    }

    public class UpgradeModel : IUpgradeModel,IDisposable
    {
        private readonly UpgradeDataSO _upgradeData;
        public event Action<UpgradeDataSO,StatSO, int> OnUpdateStat;
        public int CurrentLevel { get; private set; } = 0;
        private EntityStat _statCompo;
        public UpgradeModel(UpgradeDataSO upgradeData, int firstLevel,EntityStat statCompo)
        {
            _upgradeData = upgradeData;
            CurrentLevel = firstLevel;
            _statCompo = statCompo;
            _statCompo.SubscribeStat(upgradeData.upgradeStat, HandleStatChanged, 0);
        }
        public void HandleStatChanged(StatSO stat, float currentValue, float prevValue)
        {
            OnUpdateStat?.Invoke(_upgradeData,stat, CurrentLevel);
        }

        public void Upgrade()
        {
            //재화 처리는 나중에
            GameEventBus.RaiseEvent(PlayerEvents.UpgradeStatEvent.Init(_upgradeData, SuccessUpgrade));
        }

        private void SuccessUpgrade(bool success)
        {
            if (success)
            {
                CurrentLevel++;
            }
        }

        public void Dispose()
        {
            _statCompo.UnSubscribeStat(_upgradeData.upgradeStat, HandleStatChanged);
        }
    }
}