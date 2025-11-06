using Scripts.Network;
using Scripts.StatSystem;
using UnityEngine;
using Work.UI;

namespace Work.Upgrade
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "SO/Upgrade/UpgradeData", order = 300)]
    public class UpgradeDataSO : ScriptableObject
    {
        public StatSO upgradeStat;
        public StatType statType;
        public float baseUpgradeValue;
        public int baseUpgradeCost;
        public int maxLevel;
        public UpgradeView upgradeView;
        public int baseCostIncrease; // n
        public int increaseInterval; // m
        public int increaseAmount; // a

        public float GetUpgradeValueAtLevel(StatSO stat,int level)
        {
            if (level <= 0)
                return 0;
            return (baseUpgradeValue * level) * (1 + stat.MultiplierValue);
        }
        public float GetUpgradeValueAtLevel(int level)
        {
            if (level <= 0)
                return 0;
            return (baseUpgradeValue * level);
        }
        public float GetValueAtLevel(StatSO stat,int level)
        {
            return stat.ModifiedValue + GetUpgradeValueAtLevel(stat,level);
        }
        public float GetNextLevelUpIncrease(StatSO stat,int level)
        {
            float nextValue = GetValueAtLevel(stat,level + 1);
            float currentValue = GetValueAtLevel(stat,level);
            return nextValue - currentValue;
        }
        public int GetNextUpgradeCost(int level)
        {
            if (level <= 0) return 0;
            if (level == 1) return baseUpgradeCost;

            int cost = baseUpgradeCost;
            for (int i = 1; i < level; i++)
            {
                int n_i = baseCostIncrease + ((i - 1) / increaseInterval) * increaseAmount;
                cost += n_i;
            }
            return cost;
        }
    }
}