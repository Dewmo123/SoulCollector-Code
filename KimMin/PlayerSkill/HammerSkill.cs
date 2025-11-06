using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Combat;
using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.StatSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Work.Common.Core;
using Work.Events;
using Work.Sound;
using Work.System;

namespace Work.PlayerSkill
{
    public class HammerSkill : Skill
    {
        [SerializeField] private DamageCaster damageCaster; 
        [SerializeField] private PoolItemSO hammerEffect;
        [SerializeField] private Vector2 offset;
        [SerializeField] private SoundSO hammerSound;
        private readonly PlaySFXEvent playSFXEvent = SoundEventChannel.PlaySFXEvent;

        [Inject] private PoolManagerMono _poolManager;
        [Inject] private EnemyStorage _enemyStorage;
        
        public override void InitSkill(Entity owner)
        {
            base.InitSkill(owner);
            damageCaster.InitCaster(owner);
        }

        public async override void UseSkill()
        {
            var enemy = _enemyStorage.GetStrongestEnemy();
            if (enemy == null) return;
            
            GameEventBus.RaiseEvent(playSFXEvent.Initializer(hammerSound));
            var hammer = _poolManager.Pop<PoolingEffect>(hammerEffect);
            Vector2 playPos = (Vector2)enemy.transform.position + offset;
            hammer.PlayVFX(playPos, Quaternion.identity);
            await Awaitable.WaitForSecondsAsync(0.5f);
            
            damageCaster.CastDamage(enemy.transform.position, Damage, false);
        }

        public override void LevelUp()
        {
            Level++;
        }
    }
}