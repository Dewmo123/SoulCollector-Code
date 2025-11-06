using Scripts.Entities;

namespace Scripts.Enemies.States
{
    public class EnemyIdleState : EnemyState
    {
        EntityMovement _movement;
        public EnemyIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = _entity.GetCompo<EntityMovement>();
        }
        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
        }
        public override void Update()
        {
            base.Update();
            if (!_attackCompo.CheckTargetInRange())
                _enemy.ChangeState("MOVE");
            else if (_attackCompo.CanAttack())
                _enemy.ChangeState("ATTACK");
        }
    }
}
