using Scripts.Entities;
using UnityEngine;

namespace Scripts.Enemies.States
{
    public class EnemyMoveState : EnemyState
    {
        private EntityMovement _entityMovement;
        public EnemyMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _entityMovement = entity.GetCompo<EntityMovement>();
        }
        public override void Update()
        {
            base.Update();
            float xEnd = Camera.main.ScreenToWorldPoint(new(-1, 0)).x;
            if (_enemy.transform.position.x <= xEnd)
                _enemy.SetDead();
            else if (_attackCompo.CheckTargetInRange())
            {
                _enemy.ChangeState("IDLE");
                return;
            }
            _entityMovement.SetMoveDirection(_enemy.transform.right);
        }
    }
}
