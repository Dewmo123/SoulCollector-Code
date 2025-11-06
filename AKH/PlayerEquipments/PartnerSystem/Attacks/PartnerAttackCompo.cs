using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Entities;
using Scripts.Players;
using UnityEngine;
using Work.PlayerSkill;

namespace Scripts.PlayerEquipments.PartnerSystem.Attacks
{
    public class PartnerAttackCompo : PlayerAttackCompo
    {
        [SerializeField] private float _attackDelay;
        [SerializeField] private ProjectileSkill projectileSkill;
        public EnemyStorage EnemyStorage { get; set; }
        public PoolManagerMono PoolManager { get; set; }
        private Partner _partner => _entity as Partner;
        private float _lastAttackTime;
        public override void Attack()
        {
            projectileSkill.UseSkill(_partner.owner.GetCompo<EntityStat>(),_entity.transform, EnemyStorage, PoolManager,_partner.Level);
        }
        
        public bool CanAttack() 
            => SetTargetWithDetect() && !(Time.time < _lastAttackTime + _attackDelay || IsTargetDeadOrEmpty());
        
        public void AttackEnd()
        {
            _lastAttackTime = Time.time;
        }
    }
}
