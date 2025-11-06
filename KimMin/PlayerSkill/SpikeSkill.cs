using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Combat;
using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;
using UnityEngine;
using Work.System;

namespace Work.PlayerSkill
{
    public class SpikeSkill : Skill
    {
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private PoolItemSO waveEffect;
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private EnemyStorage _enemyStorage;
        
        public override void InitSkill(Entity owner)
        {
            base.InitSkill(owner);
            damageCaster.InitCaster(owner);
        }

        public override void UseSkill()
        {
            var enemy = _enemyStorage.GetStrongestEnemy();
            if (enemy == null) return;
            
            var wave = _poolManager.Pop<PoolingEffect>(waveEffect);
            wave.transform.position = enemy.transform.position;
            damageCaster.transform.position = enemy.transform.position;
            damageCaster.CastDamage(21f, false);
        }

        public override void LevelUp()
        {
            Level++;
        }
    }
}