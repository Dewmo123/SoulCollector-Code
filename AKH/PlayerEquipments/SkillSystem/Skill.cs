using System;
using Inventory;
using Scripts.Entities;
using Scripts.StatSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Work.Item;

namespace Scripts.PlayerEquipments.SkillSystem
{
    [Serializable]
    public class SkillDamageData
    {
        public float baseDamage;
        [FormerlySerializedAs("baseDamageMultiplier")] public float baseDamageMult;
        public float damageIncreasePerLevel;
    }
    
    public abstract class Skill : MonoBehaviour,IEquipItem
    {
        [field: SerializeField] public SkillDataSO SkillData { get; private set; }
        public bool IsEnabled { get; set; } = false;
        public int Level { get; set; } // 레벨 Set 해줄때 업그레이드 넣어줘야함
        public EquipableItemSO ItemData => SkillData;
        
        [Header("Damage")]
        [SerializeField] protected StatSO attackPowerStat;
        [SerializeField] protected SkillDamageData damageData;
        protected float Damage => 
            (_entityStat.GetStat(attackPowerStat).Value *
             (1 + damageData.damageIncreasePerLevel * Level + damageData.baseDamageMult))
            + damageData.baseDamage;
        
        protected Entity _owner;
        protected EntityStat _entityStat;
        protected EntityAnimatorTrigger _animatorTrigger;

        public virtual void InitSkill(Entity owner)
        {
            _owner = owner;
            _entityStat = owner.GetCompo<EntityStat>();
            _animatorTrigger = owner.GetCompo<EntityAnimatorTrigger>();
        }
        public virtual void ApplyMultiplier()
        {
            foreach(var item in SkillData.ownStatDict)
            {
                _entityStat.AddMultiplier(item.Key, SkillData.itemName, item.Value);
            }
        }
        public abstract void UseSkill();
        public abstract void LevelUp();
    }
}
