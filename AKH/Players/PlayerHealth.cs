using Scripts.Combat;
using Scripts.Entities;
using Scripts.StatSystem;
using System;
using UnityEngine;

namespace Scripts.Players
{
    public class PlayerHealth : EntityHealth, IAfterInit
    {
        [SerializeField] private StatSO hpsStat;
        protected EntityStat _statCompo;
        private float hps;
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInit()
        {
            hps = _statCompo.SubscribeStat(hpsStat, HandleHpsChanged,0);
            maxHealth = _statCompo.SubscribeStat(hpStat, HandleMaxHpChange, 10);
            currentHealth = maxHealth;
        }
        private void Update()
        {
            if (IsDead)
                return;
            currentHealth = Mathf.Clamp(currentHealth + hps * Time.deltaTime, 0, maxHealth);
        }
        private void HandleHpsChanged(StatSO stat, float currentValue, float prevValue)
            => hps = currentValue;

        private void OnDestroy()
        {
            hps = _statCompo.SubscribeStat(hpsStat, HandleHpsChanged,0);
            _statCompo.UnSubscribeStat(hpStat, HandleMaxHpChange);
        }
    }
}
