using Scripts.Entities;
using Scripts.StageSystem;
using Work.Common.Core;

namespace Scripts.Players.States
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
            {
                _player.OnDeadEvent?.Invoke(_player);
                _isTriggerCall = false;
            }
        }
    }
}
