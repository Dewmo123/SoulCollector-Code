using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;

namespace Scripts.Players.States
{
    public class PlayerIdleState : PlayerCanUseSkillState
    {
        private PlayerAttackCompo _attackCompo;
        public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }
        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();
            if (_attackCompo.SetTargetWithDetect())
                _player.ChangeState("ATTACK");
        }
    }
}
