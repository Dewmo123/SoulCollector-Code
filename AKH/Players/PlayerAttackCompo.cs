using Scripts.Combat;
using Scripts.Combat.Damage;
using Scripts.Entities;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.Players
{
    public class PlayerAttackCompo : EntityAttackCompo,IAfterInit
    {
        [SerializeField] private bool useCritical;
        [SerializeField] private StatSO criticalChanceStat;
        [SerializeField] private StatSO criticalDamageStat;
        [SerializeField] private ContactFilter2D enemyLayer;
        protected EntityStat _entityStat;
        private Collider2D[] _colliders = new Collider2D[15];

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _entityStat = entity.GetCompo<EntityStat>();
        }
        public void AfterInit()
        {
            attackPowerStat = _entityStat.GetStat(attackPowerStat);
            if (useCritical)
            {
                criticalChanceStat = _entityStat.GetStat(criticalChanceStat);
                criticalDamageStat = _entityStat.GetStat(criticalDamageStat);
            }
        }
        public override void Attack()
        {
            if (_target == null)
                return;
            if (useCritical && Random.value <= criticalChanceStat.Value)
            {
                float damage = attackPowerStat.Value * criticalDamageStat.Value;
                _target.Hit(damage, true);
            }
            else
            {
                _target.Hit(attackPowerStat.Value, false);
            }
        }

        public bool SetTargetWithDetect()
        {
            Transform closestOne = null;
            int cnt = Physics2D.OverlapCircle(transform.position, _detectRange, enemyLayer, _colliders);

            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < cnt; i++)
            {
                if (_colliders[i].TryGetComponent(out IDamageable enemy))
                {
                    if (enemy.IsDead) continue;
                    float distanceToEnemy = Vector2.Distance(transform.position, _colliders[i].transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        _target = enemy;
                        closestDistance = distanceToEnemy;
                        closestOne = _colliders[i].transform;
                    }
                }
            }
            return closestOne;
        }
    }
}
