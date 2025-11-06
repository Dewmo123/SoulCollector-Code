using Scripts.Entities;
using UnityEngine;

namespace Scripts.Players.States
{
    public class PlayerAttackState : PlayerCanUseSkillState
    {
        private PlayerAttackCompo _attackCompo;
        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }
        public override void Update()
        {
            base.Update();
            if (_attackCompo.IsTargetDeadOrEmpty() && _isTriggerCall && !_attackCompo.SetTargetWithDetect())
            {
                _player.ChangeState("IDLE");
            }
            _isTriggerCall = false;
        }
    }
}
