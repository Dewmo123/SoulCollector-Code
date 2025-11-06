using System;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.Players;
using Scripts.StatSystem;
using UnityEngine;

namespace Work.PlayerSkill
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        [SerializeField] protected StatSO attackPowerStat;
        [SerializeField] protected SkillDamageData damageData;

        [SerializeField] protected float speed;
        [SerializeField] protected float rotationSpeed;
        [SerializeField] protected float startAngle = 45f; 
        public GameObject GameObject => gameObject;
        public int Level { get; set; }

        private Rigidbody2D _rigidbody;
        protected Vector3 _start, end, _point, _target;
        protected float t, duration;
        private Pool _pool;

        protected float Damage;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            MoveProjectile();
        }

        public virtual void InitProjectile(EntityStat entityStat, Vector3 start, Vector3 target)
        {
            Damage = (entityStat.GetStat(attackPowerStat).Value *
                      (1 + damageData.damageIncreasePerLevel * Level + damageData.baseDamageMult))
                     + damageData.baseDamage;
            
            _start = start;
            _target = target;
        }

        protected virtual void MoveProjectile()
        {
            transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed);
        }

        protected void OnHit()
        {
            _pool.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }
        public void ResetItem() { }
    }
}