using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.PlayerEquipments
{
    [Serializable]
    public struct UpgradeInfo
    {
        public int needAmount;
        public int maxLevel;
    }
    [CreateAssetMenu(fileName = "SkillUpgradeData", menuName = "SO/Skill/Upgrade")]
    public class UpgradeSO : ScriptableObject
    {
        public List<UpgradeInfo> upgradeInfos;
    }
}
