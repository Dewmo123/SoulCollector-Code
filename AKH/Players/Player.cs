
using DewmoLib.Dependencies;
using Scripts.Entities;
using Scripts.FSM;
using Scripts.StageSystem;
using UnityEngine;
using Work.Common.Core;

namespace Scripts.Players
{
    public class Player : Entity, IDependencyProvider
    {
        [SerializeField] private StateDataSO[] states;
        private EntityStateMachine _stateMachine;
        [Provide]
        public Player Provide() => this;
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new(this, states);
            GameEventBus.AddListener<MoveEvent>(HandleMoveEvent);
            GameEventBus.AddListener<StopEvent>(HandleStopEvent);
            ChangeState("IDLE");
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<MoveEvent>(HandleMoveEvent);
            GameEventBus.RemoveListener<StopEvent>(HandleStopEvent);
        }
        private void HandleStopEvent(StopEvent @event)
            => ChangeState("IDLE");
        private void HandleMoveEvent(MoveEvent @event)
        {
            Revive();
            ChangeState("MOVE");
        }
        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        public override void SetDead()
        {
            base.SetDead();
            ChangeState("DEAD");
        }
        public void ChangeState(string stateName, bool forced = false) => _stateMachine.ChangeState(stateName, forced);
    }
}
