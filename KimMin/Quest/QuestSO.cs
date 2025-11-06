using Scripts.Network;
using UnityEngine;
using Work.Core;

namespace Work.Quest
{
    public enum QuestType
    {
        UpgradeLevel,
        DefeatMob,
        RollItem
    }
    
    [CreateAssetMenu(fileName = "QuestSO", menuName = "SO/Quest/Quest", order = 0)]
    public class QuestSO : ScriptableObject
    {
        public QuestType questType;
        public StatType statType;
        public ItemType itemType;
        public float startValue;
    }
}