using Scripts.Combat;
using Scripts.Entities;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.PlayerEquipments.SkillSystem.Skills
{
    public class ReaperSkill : Skill
    {
        [SerializeField] private DamageCaster damageCaster;
        
        public override void InitSkill(Entity owner)
        {
            base.InitSkill(owner);
            attackPowerStat = _entityStat.GetStat(attackPowerStat);
            damageCaster.InitCaster(owner);
        }
        public override void UseSkill()
        {
            damageCaster.CastDamage(Damage,false);
        }
        public override void LevelUp()
        {
            Level++;
        }
    }
}
