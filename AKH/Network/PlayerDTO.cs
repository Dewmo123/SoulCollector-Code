using System;
using System.Collections.Generic;

namespace Scripts.Network
{
    public enum StatType
    {
        None = 0,
        AttackPower,
        AttackSpeed,
        Health,
        CriticalChance,
        CriticalDamage,
        Hps
    }
    public enum GoodsType
    {
        None = 0,
        Gold,
        Crystal,
        ReinforceStone,
        DungeonKey
    }
    public class PlayerDTO
    {
        public string Id { get; set; }

        public ChapterDTO Chapter { get; set; }

        public Dictionary<StatType, int> Stats { get; set; }
        public Dictionary<GoodsType, int> Goods { get; set; }
        public Dictionary<string, SkillDTO> Skills { get; set; }
        public Dictionary<string, PartnerDTO> Partners { get; set; }
        public string[] SkillEquips { get; set; }
        public string[] PartnerEquips { get; set; }

    }
    public class StatDTO
    {
        public StatType StatType { get; set; }
        public int Level { get; set; }
    }
    public class GoodsDTO
    {
        public GoodsType GoodsType { get; set; }
        public int Amount { get; set; }
    }
    public class SkillDTO
    {
        public string SkillName { get; set; }
        public int Level { get; set; }
        public int Upgrade { get; set; }
        public int Amount { get; set; }
    }
    public class ChapterDTO
    {
        public int Chapter { get; set; }
        public int Stage { get; set; }
        public int EnemyCount { get; set; }
    }
    public class EnemyDeadDTO
    {
        public int EnemyCount { get; set; }
    }
    public class PartnerDTO
    {
        public string PartnerName { get; set; }
        public int Level { get; set; }
        public int Upgrade { get; set; }
        public int Amount { get; set; }
    }
    public class SkillEquipDTO
    {
        public int Idx { get; set; }
        public string SkillName { get; set; }
    }
    public class SkillAmountDTO
    {
        public string SkillName { get; set; }
        public int Amount { get; set; }
    }
    public class PartnerAmountDTO
    {
        public string PartnerName { get; set; }
        public int Amount { get; set; }
    }
    public class PartnerEquipDTO
    {
        public int Idx { get; set; }
        public string PartnerName { get; set; }
    }
    public class ChangeChapterDTO
    {
        public int Chapter { get; set; }
    }
    public class ChangeStageDTO
    {
        public int Stage { get; set; }
    }
    public class LevelUpSkillDTO
    {
        public string SkillName { get; set; }
        public int Level { get; set; }

    }
    public class LevelUpPartnerDTO
    {
        public string PartnerName { get; set; }
        public int Level { get; set; }

    }
    public class SetAmountAndUpgradeDTO
    {
        public string PartnerName { get; set; }
        public int Amount { get; set; }
        public int Upgrade { get; set; }
    }
}
