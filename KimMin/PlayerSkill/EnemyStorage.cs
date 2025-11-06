using System.Collections.Generic;
using System.Linq;
using DewmoLib.Dependencies;
using Scripts.Combat;
using Scripts.Enemies;
using UnityEngine;

namespace Work.PlayerSkill
{
    [Provide]
    public class EnemyStorage : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private float radius;
        [SerializeField] private ContactFilter2D whatIsEnemy;
        
        private const int MaxCount = 16;
        private Collider2D[] _result = new Collider2D[MaxCount];

        public Enemy[] GetAllEnemies()
        {
            int count = Physics2D.OverlapCircle(transform.position, radius, whatIsEnemy, _result);
            List<Enemy> enemies = new();
            for (int i = 0; i < count; i++)
            {
                enemies.Add(_result[i].GetComponent<Enemy>());
            }
            
            return enemies.ToArray();
        }
        
        public Enemy GetNearestEnemy()
        {
            return GetAllEnemies()
                .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
                .FirstOrDefault();
        }

        public Enemy GetStrongestEnemy()
        {
            return GetAllEnemies()
                .OrderByDescending(e => e.GetCompo<EntityHealth>().CurrentHealth)
                .FirstOrDefault();
        }

        public Enemy GetWeakestEnemy()
        {
            return GetAllEnemies()
                .OrderBy(e => e.GetCompo<EntityHealth>().CurrentHealth)
                .FirstOrDefault();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}