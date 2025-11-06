using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;

namespace Scripts.Players.States
{
    public abstract class PlayerCanUseSkillState : PlayerState
    {
        private PlayerSkillManager _skillManager;
        protected PlayerCanUseSkillState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _skillManager = entity.GetCompo<PlayerSkillManager>();
        }
        public override void Update()
        {
            base.Update();
            if (_skillManager.SkillQueue.Count > 0)
                _player.ChangeState("SKILL");
        }
    }
}
