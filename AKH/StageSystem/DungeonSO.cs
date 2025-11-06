using AYellowpaper.SerializedCollections;
using Inventory;
using Scripts.Enemies;
using Scripts.Goods;
using Scripts.StatSystem;
using UI.Base;
using UnityEngine;

namespace Scripts.StageSystem
{
    [CreateAssetMenu(fileName = "StageSystem/DungeonSO", menuName = "SO/DungeonSO")]
    public class DungeonSO : BaseStageDataSO
    {
        [Header("DungeonSetting")]
        public int dungeonDifficulty;
        public EnemySO[] dungeonEnemies;
        public GoodsSO key;
        public int keyRequire;
        public SerializedDictionary<ItemDataSO, int> rewards;
        public SerializedDictionary<StatSO, float> defaultMultiplier;//¿˚ Ω∫≈» πË¿≤
        public BasePopup resultPopupPrefab;
        // Add other dungeon-specific properties here
        public void ApplyMultiplier()
        {
            foreach (var enemy in dungeonEnemies)
                foreach (var item in defaultMultiplier)
                {
                    enemy.GetStat(item.Key).AddMultiplier("Stage", item.Value);
                }
        }
        public void RemoveMultiplier()
        {
            foreach (var enemy in dungeonEnemies)
                foreach (var item in defaultMultiplier.Keys)
                    enemy.GetStat(item).RemoveMultiplier("Stage");
        }
    }
}
