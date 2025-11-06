using Scripts.Combat.Damage;
using Scripts.Entities;
using UnityEngine;
using Work.Core;

namespace Work.PlayerSkill
{
    public class CurveProjectile : Projectile
    {
        private bool _isMoving = false;
        
        public override void InitProjectile(EntityStat entityStat, Vector3 start, Vector3 target)
        {
            base.InitProjectile(entityStat, start, target);
            float distance = Vector3.Distance(start, target);
            float angle = Utility.GetAngleFromPos(start, target);
            
            duration = distance / speed;
            _point = Utility.GetNewPoint(start, angle, distance * 0.3f);
            t = 0f;
            
            _isMoving = true;
        }
        
        protected override void MoveProjectile()
        {
            base.MoveProjectile();
            if (!_isMoving) return;
            
            end = _target;
            t += Time.deltaTime / duration;
            transform.position = Utility.QuaternionCurve(_start, _point, end, t);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                _isMoving = false;
                damageable.Hit(Damage, false);
            }
            
            OnHit();
        }
    }
}