using Scripts.Entities;

namespace Scripts.PlayerEquipments.PartnerSystem.States
{
    public class PartnerAttackState : PartnerState
    {
        public PartnerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
                _partner.ChangeState("IDLE");
        }
        public override void Exit()
        {
            base.Exit();
            _attackCompo.AttackEnd();
        }
    }
}
