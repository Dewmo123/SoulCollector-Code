using Scripts.Entities;
using Scripts.FSM;

namespace Scripts.Enemies.States
{
    public class EnemyState : EntityState
    {
        protected Enemy _enemy;
        protected EnemyAttackCompo _attackCompo;
        public EnemyState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _enemy = entity as Enemy;
            _attackCompo = entity.GetCompo<EnemyAttackCompo>();
        }
    }
}
