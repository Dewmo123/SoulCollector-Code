using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.Players;
using UnityEngine;

namespace Work.PlayerSkill
{
    public class ProjectileSkill : MonoBehaviour
    {
        [SerializeField] private PoolItemSO projectilePool;

        public void UseSkill(EntityStat _entityStat,Transform start, EnemyStorage enemyStorage, PoolManagerMono poolManager,int level)
        {
            var enemy = enemyStorage.GetNearestEnemy();
            if (enemy == null) return;
            var projectile = poolManager.Pop<CurveProjectile>(projectilePool);
            projectile.Level= level;
            projectile.InitProjectile(_entityStat, start.transform.position, enemy.transform.position);
        }
    }
}