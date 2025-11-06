using Scripts.Entities;

namespace Scripts.Enemies.States
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
                _enemy.ChangeState("IDLE");
        }
        public override void Exit()
        {
            base.Exit();
            _attackCompo.AttackEnd();
        }
    }
}
