using Scripts.Combat.Damage;
using Scripts.Entities;
using Scripts.StatSystem;
using System;
using UnityEngine;
using UnityEngine.Events;
using Work.Common.Core;

namespace Scripts.Combat
{

    public class EntityHealth : MonoBehaviour, IDamageable, IEntityComponent
    {
        protected Entity _entity;
        [SerializeField] protected StatSO hpStat;
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float currentHealth;
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public bool IsDead => _entity.IsDead;
        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
            entity.OnReviveEvent.AddListener(HandleRevive);
        }

        

        protected virtual void HandleMaxHpChange(StatSO stat, float currentValue, float prevValue)
        {
            float changed = currentValue - prevValue;
            maxHealth = currentValue;
        }

        public void Hit(float damage, bool isCritical)
        {
            if (_entity.IsDead)
                return;
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            GameEventBus.RaiseEvent(DamageEvents.DamageEvent.Init(_entity.transform.position + Vector3.up/2, damage, isCritical));
            _entity.OnHitEvent?.Invoke();
            if (currentHealth <= 0)
            {
                _entity.SetDead();
            }
        }
        private void HandleRevive()
        {
            currentHealth = maxHealth;
        }
    }
}
