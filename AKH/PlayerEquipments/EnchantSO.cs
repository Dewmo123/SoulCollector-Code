using Scripts.Goods;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.PlayerEquipments
{
    [Serializable]
    public struct EnchantInfo
    {
        public GoodsSO needType;
        public int needAmount;
    }
    [CreateAssetMenu(fileName = "SkillData", menuName = "SO/Skill/Enchant")]
    public class EnchantSO : ScriptableObject
    {
        public List<EnchantInfo> enchantInfos;
    }
}
