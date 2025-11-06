using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Combat;
using Scripts.Combat.Damage;
using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.StatSystem;
using Unity.Mathematics;
using UnityEngine;
using Work.Common.Core;
using Work.Events;
using Work.Sound;
using Work.System;

namespace Work.PlayerSkill
{
    public class ShunraiSkill : Skill
    {
        [SerializeField] private PoolItemSO lightboltPool;
        [SerializeField] private PoolItemSO explosionPool;
        [SerializeField] private DamageCaster damageCaster;
        private readonly PlaySFXEvent playSFXEvent = SoundEventChannel.PlaySFXEvent;
        [SerializeField] private SoundSO shunraiSound;
        
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private EnemyStorage _storage;

        private const float SHUNRAI_HEIGHT = 5f;

        public override void InitSkill(Entity owner)
        {
            base.InitSkill(owner);
            damageCaster.InitCaster(owner);
        }

        public async override void UseSkill()
        {
            var enemy = _storage.GetStrongestEnemy();
            if (enemy == null) return;

            var shunraiEffect = _poolManager.Pop<PoolingEffect>(lightboltPool);
            var explosionEffect = _poolManager.Pop<PoolingEffect>(explosionPool);

            Vector2 pos = enemy.transform.position;
            explosionEffect.PlayVFX(pos, quaternion.identity);
            damageCaster.transform.position = pos;
            damageCaster.CastDamage(Damage, false);
            
            pos.y = SHUNRAI_HEIGHT;
            shunraiEffect.PlayVFX(pos, quaternion.identity);
            GameEventBus.RaiseEvent(CameraEventChannel.CameraImpulseEvent);
            GameEventBus.RaiseEvent(playSFXEvent.Initializer(shunraiSound));

            await Awaitable.WaitForSecondsAsync(0.5f);
            _poolManager.Push(shunraiEffect);
            _poolManager.Push(explosionEffect);
        }

        public override void LevelUp()
        {
            Level++;
        }
    }
}