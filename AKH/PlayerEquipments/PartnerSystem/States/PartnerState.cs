using Scripts.Entities;
using Scripts.FSM;
using Scripts.PlayerEquipments.PartnerSystem.Attacks;

namespace Scripts.PlayerEquipments.PartnerSystem.States
{
    public abstract class PartnerState : EntityState
    {
        protected Partner _partner;
        protected PartnerAttackCompo _attackCompo;
        public PartnerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _partner = entity as Partner;
            _attackCompo = _partner.GetCompo<PartnerAttackCompo>();
        }
    }
}
