using Scripts.Entities;
using Scripts.FSM;

namespace Scripts.Players.States
{
    public abstract class PlayerState : EntityState
    {
        protected Player _player;
        public PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
        }
    }
}
