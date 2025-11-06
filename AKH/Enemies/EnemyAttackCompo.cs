using Scripts.Combat;
using Scripts.Combat.Damage;
using Scripts.Entities;
using UnityEngine;

namespace Scripts.Enemies
{
    public class EnemyAttackCompo : EntityAttackCompo, IChangeableCompo<EnemySO>
    {
        private Enemy _enemy;
        private float _attackDelay;
        private float _lastAttackTime;
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _enemy = entity as Enemy;
        }
        public void Change(EnemySO before, EnemySO current)
        {
            _target = _enemy.Target.GetComponent<IDamageable>();
            _attackDelay = current.attackDelay;
            _detectRange = current.detectRange;
            attackPowerStat = current.GetStat(attackPowerStat);
        }
        public bool CheckTargetInRange()
        {
            if (_enemy.Target.IsDead)
                return false;
            float dist = Mathf.Abs(_enemy.transform.position.x - _enemy.Target.transform.position.x);
            return dist <= _detectRange;
        }
        public bool CanAttack()
            => !(Time.time < _lastAttackTime + _attackDelay);
        public void AttackEnd()
        {
            _lastAttackTime = Time.time;
        }

        public override void Attack()
        {
            _target.Hit(attackPowerStat.Value, false);
        }
    }
}
