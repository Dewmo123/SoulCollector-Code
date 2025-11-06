using Scripts.Combat;
using Scripts.Combat.Damage;
using Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AKH.Scripts.Combat
{
    public class OverlapDamageCaster : DamageCaster
    {
        public enum OverlapCastType
        {
            Circle, Box
        }
        [SerializeField] protected OverlapCastType overlapCastType;
        [SerializeField] private Vector2 damageBoxSize;
        [SerializeField] private float damageRadius;

        private Collider2D[] _hitResults;

        public override void InitCaster(Entity owner)
        {
            base.InitCaster(owner);
            _hitResults = new Collider2D[maxHitCount];
        }

        public override bool CastDamage(float damage, bool isCritical)
        {

            int cnt = overlapCastType switch
            {
                OverlapCastType.Circle => Physics2D.OverlapCircle(transform.position, damageRadius, contactFilter, _hitResults),
                OverlapCastType.Box => Physics2D.OverlapBox(transform.position, damageBoxSize, 0, contactFilter, _hitResults),
                _ => 0
            };

            for (int i = 0; i < cnt; i++)
            {
                if (_hitResults[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.Hit(damage,isCritical);
                }
            }

            return cnt > 0;
        }

        public override bool CastDamage(Vector2 pos, float damage, bool isCritical)
        {
            transform.position = pos;
            return CastDamage(damage, isCritical);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.7f, 0.7f, 0, 1f);
            switch (overlapCastType)
            {
                case OverlapCastType.Circle:
                    Gizmos.DrawWireSphere(transform.position, damageRadius);
                    break;
                case OverlapCastType.Box:
                    Gizmos.DrawWireCube(transform.position, damageBoxSize);
                    break;
            }
            ;
        }
#endif
    }
}
