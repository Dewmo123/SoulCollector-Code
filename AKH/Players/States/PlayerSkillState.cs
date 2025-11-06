using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;

namespace Scripts.Players.States
{
    public class PlayerSkillState : PlayerState
    {
        private PlayerSkillManager _skillManager;
        private Skill _currentSkill;
        public PlayerSkillState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _skillManager = entity.GetCompo<PlayerSkillManager>();
        }
        public override void Enter()
        {
            base.Enter();
            SkillQueueInfo info = _skillManager.SkillQueue.Peek();
            if (_skillManager.SkillSockets[info.slotIndex].CurrentSkill == null)
            {
                _player.ChangeState("IDLE");
                return;
            }
            _animatorTrigger.OnCastSkillTrigger += HandleCastSkill;
            _currentSkill = info.skill;
            _skillManager.SetCooldown(info.slotIndex);
            _entityAnimator.SetParam(_currentSkill.SkillData.animhash, true);
        }

        private void HandleCastSkill()
        {
            _currentSkill?.UseSkill();
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall && _skillManager.SkillQueue.Count > 1)
                _player.ChangeState("SKILL", true);
            else if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }
        public override void Exit()
        {
            base.Exit();
            _animatorTrigger.OnCastSkillTrigger -= HandleCastSkill;
            _entityAnimator.SetParam(_currentSkill.SkillData.animName, false);
            SkillQueueInfo info = _skillManager.SkillQueue.Dequeue();
            _skillManager.RegisteredSkill.Remove(info.slotIndex);
        }
    }
}
