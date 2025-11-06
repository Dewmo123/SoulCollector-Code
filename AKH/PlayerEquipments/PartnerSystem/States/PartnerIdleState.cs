using Scripts.Entities;

namespace Scripts.PlayerEquipments.PartnerSystem.States
{
    public class PartnerIdleState : PartnerState
    {
        public PartnerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Update()
        {
            base.Update();
            if (_attackCompo.CanAttack())
                _partner.ChangeState("ATTACK");
        }
    }
}
