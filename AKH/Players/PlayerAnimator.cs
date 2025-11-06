using Scripts.Entities;
using Scripts.StatSystem;
using System;
using UnityEngine;

namespace Scripts.Players
{
    public class PlayerAnimator : EntityAnimator,IAfterInit
    {
        [SerializeField] private StatSO attackSpeedStat;
        private EntityStat _statCompo;
        private static int _attackSpeedHash = Animator.StringToHash("AttackSpeed");

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _statCompo = entity.GetCompo<EntityStat>();
        }
        public void AfterInit()
        {
            float val = _statCompo.SubscribeStat(attackSpeedStat, HandleAttackSpeedChanged, 1);
            SetParam(_attackSpeedHash, val);
        }

        private void HandleAttackSpeedChanged(StatSO stat, float currentValue, float prevValue)
        {
            SetParam(_attackSpeedHash, stat.DivideValue);
        }
    }
}
