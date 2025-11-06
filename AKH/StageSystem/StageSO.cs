using AYellowpaper.SerializedCollections;
using Scripts.Enemies;
using Scripts.Network;
using Scripts.StatSystem;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.StageSystem
{
    [Serializable]
    public struct DropInfo
    {
        public GoodsType type;
        public float percent;
        public int baseValue;
        public int modifierPerStage;
        public float multiplierPerStage;
        public int GetValue(int stageCount) => Mathf.FloorToInt((baseValue + modifierPerStage * stageCount) * (1 + multiplierPerStage * stageCount));
    }
    [Serializable]
    public struct StageSettingInfo
    {
        public int baseValue;
        public int modifierPerStage;
        public float multiplierPerStage;
        public int GetValue(int stageCount) => Mathf.FloorToInt((baseValue + modifierPerStage * stageCount) * (1 + multiplierPerStage * stageCount));
    }
    [Serializable]
    public struct EnemyStatIncInfo
    {
        public float baseMultiplier;
        public float modifierPerStage;
        public float multiplierPerStage;
        public float GetValue(int stage) => (baseMultiplier + modifierPerStage * stage) * (1 + multiplierPerStage * stage);
    }
    [CreateAssetMenu(fileName ="StageSystem/StageSO",menuName = "SO/StageSO")]
    public class StageSO : BaseStageDataSO
    {
        [Header("EnemySetting")]
        public int maxStageCount;
        public StageSettingInfo nextStageCount;
        public StageSettingInfo enemyCount;
        public EnemySO[] enemys;
        public EnemySO boss;
        public SerializedDictionary<StatSO, EnemyStatIncInfo> defaultMultiplier;//적 스탯 배율
        [Header("DropSetting")]
        public DropInfo[] dropInfos;
        public EnemySO GetRandomEnemy()
            => enemys[Random.Range(0, enemys.Length)];
        public void ApplyMultiplier(int stage)
        {
            foreach(var enemy in enemys)
                foreach(var item in defaultMultiplier)
                {
                    enemy.GetStat(item.Key).AddMultiplier("Stage", item.Value.GetValue(stage));
                }
        }
        public void RemoveMultiplier()
        {
            foreach (var enemy in enemys)
                foreach (var item in defaultMultiplier.Keys)
                    enemy.GetStat(item).RemoveMultiplier("Stage");
        }
        public void EnterBoss()
        {
            foreach(var item in defaultMultiplier)
            {
                boss.GetStat(item.Key).AddMultiplier("Boss", item.Value.GetValue(maxStageCount));
            }
        }
        public void ExitBoss()
        {
            foreach (var item in defaultMultiplier)
            {
                boss.GetStat(item.Key).RemoveMultiplier("Boss");
            }
        }

    }
}
