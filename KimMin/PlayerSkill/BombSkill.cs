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
    public class BombSkill : Skill
    {
        [SerializeField] private PoolItemSO bombPool;
        [SerializeField] private PoolItemSO bombEffectPool;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private SoundSO sfxSound;
        private readonly PlaySFXEvent playSFXEvent = SoundEventChannel.PlaySFXEvent;
        
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private EnemyStorage _enemyStorage;

        private Bomb _bomb;
        public override void InitSkill(Entity owner)
        {
            base.InitSkill(owner);
            damageCaster.InitCaster(owner);
        }

        public override void UseSkill()
        {
            var enemy = _enemyStorage.GetStrongestEnemy();
            if (enemy == null) return;
            
            _bomb = _poolManager.Pop<Bomb>(bombPool);
            Vector3 pos = transform.position;
            pos.y += 0.2f;
            _bomb.transform.position = pos;
            _bomb.Init(enemy.transform);
            _bomb.SubscribeStatus(HandleBombExplode);
            GameEventBus.RaiseEvent(playSFXEvent.Initializer(sfxSound));
        }

        public async void HandleBombExplode(Vector2 bombPos)
        {
            var effect = _poolManager.Pop<PoolingEffect>(bombEffectPool);
            effect.PlayVFX(bombPos, Quaternion.identity);
            _bomb.UnsubscribeStatus(HandleBombExplode);
            damageCaster.transform.position = bombPos;
            damageCaster.CastDamage(Damage, false);
            GameEventBus.RaiseEvent(CameraEventChannel.CameraImpulseEvent);

            await Awaitable.WaitForSecondsAsync(2f);
            _poolManager.Push(effect);
        }

        public override void LevelUp()
        {
            Level++;
        }
    }
}