using Scripts.Entities;
using System;

namespace Scripts.Enemies
{
    public class EnemyAnimator : EntityAnimator,IChangeableCompo<EnemySO>
    {
        private Enemy _enemy;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _enemy = entity as Enemy;
        }
        public void Change(EnemySO before,EnemySO information)
        {
            Animator.runtimeAnimatorController = information.animator;
        }

    }
}
