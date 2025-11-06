using AYellowpaper.SerializedCollections;
using DewmoLib.Dependencies;
using Scripts.Entities;
using Scripts.Network;
using Scripts.Players.Storages;
using UnityEngine;
using Work.Common.Core;
using Work.Upgrade;

namespace Scripts.Players
{
    public class PlayerUpgradeManager : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private SerializedDictionary<StatType, UpgradeDataSO> upgradeDatas;
        private Player _player;
        private EntityStat _entityStat;
        [Inject] private PlayerInfoStorage _storage;
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _entityStat = _player.GetCompo<EntityStat>();
            GameEventBus.AddListener<UpgradeStatEvent>(HandleUpgrade);
        }

        private void Start()
        {
            HandleInfoRecv();
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<UpgradeStatEvent>(HandleUpgrade);
        }
        private void HandleInfoRecv()
        {
            foreach (var item in _storage.StatStorage.Stats)
            {
                UpgradeDataSO upgradeData = upgradeDatas[item.Key];
                _entityStat.RemoveModifier(upgradeData.upgradeStat, "level");
                _entityStat.AddModifier(upgradeData.upgradeStat, "level", upgradeData.GetUpgradeValueAtLevel(item.Value));
            }
        }
        private async void HandleUpgrade(UpgradeStatEvent @event)
        {
            UpgradeDataSO upgradeData = @event.upgradeData;
            int level = _storage.StatStorage.Stats[upgradeData.statType];
            if (upgradeData.maxLevel <= level)
                return;
            bool success = await _storage.GoodsStorage.ChangeGoods(GoodsType.Gold, -upgradeData.GetNextUpgradeCost(level));
            @event.callback?.Invoke(success);
            if (success&& await _storage.StatStorage.ChangeStat(upgradeData.statType, 1))
            {
                _entityStat.RemoveModifier(upgradeData.upgradeStat, "level");
                _entityStat.AddModifier(upgradeData.upgradeStat, "level",
                    upgradeData.GetUpgradeValueAtLevel(_storage.StatStorage.Stats[upgradeData.statType]));
            }
        }
    }
}
