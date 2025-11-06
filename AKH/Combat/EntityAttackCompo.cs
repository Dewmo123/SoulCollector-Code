using Scripts.Combat.Damage;
using Scripts.Entities;
using Scripts.StatSystem;
using UnityEngine;

namespace Scripts.Combat
{
    public abstract class EntityAttackCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] protected StatSO attackPowerStat;
        [SerializeField] protected float _detectRange;
        protected IDamageable _target;
        protected Entity _entity;
        private EntityAnimatorTrigger _animatorTrigger;

        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();
            _animatorTrigger.OnDamageCastTrigger += Attack;
        }
        private void OnDestroy()
        {
            _animatorTrigger.OnDamageCastTrigger -= Attack;
        }
        public abstract void Attack();

        public bool IsTargetDeadOrEmpty()
        {
            if (_target == null)
                return true;
            if (_target.IsDead)
            {
                _target = null;
                return true;
            }
            return false;
        }
#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectRange);
        }
#endif
    }
}
