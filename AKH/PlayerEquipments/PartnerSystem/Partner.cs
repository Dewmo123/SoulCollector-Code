using DewmoLib.ObjectPool.RunTime;
using Inventory;
using Scripts.Entities;
using Scripts.FSM;
using Scripts.PlayerEquipments.PartnerSystem.Attacks;
using Scripts.StageSystem;
using UnityEngine;
using Work.Common.Core;
using Work.Item;
using Work.PlayerSkill;

namespace Scripts.PlayerEquipments.PartnerSystem
{
    public class Partner : Entity,IEquipItem
    {
        [SerializeField] private StateDataSO[] states;
        [field:SerializeField] public BuddySO PartnerData { get; private set; }
        public int Level { get; set; }
        public EquipableItemSO ItemData => PartnerData;
        public Entity owner;
        private EntityStat _entityStat;
        public bool IsEnabled { get; set; }

        private EntityStateMachine _stateMachine;
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new(this, states);
            GameEventBus.AddListener<MoveEvent>(HandleMoveEvent);
            GameEventBus.AddListener<StopEvent>(HandleStopEvent);
        }

        public void Init(EnemyStorage storage, PoolManagerMono poolManager, Entity owner)
        {
            var attackCompo = GetCompo<PartnerAttackCompo>();
            this.owner = owner;
            _entityStat = owner.GetCompo<EntityStat>();
            attackCompo.EnemyStorage = storage;
            attackCompo.PoolManager = poolManager;
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<MoveEvent>(HandleMoveEvent);
            GameEventBus.RemoveListener<StopEvent>(HandleStopEvent);
        }
        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        private void HandleStopEvent(StopEvent @event)
        {
            ChangeState("IDLE");
        }
        private void HandleMoveEvent(MoveEvent @event)
        {
            ChangeState("MOVE");
        }
        public void ChangeState(string stateName, bool forced = false)
        {
            _stateMachine.ChangeState(stateName, forced);
        }

        public void ApplyMultiplier()
        {
            foreach (var item in PartnerData.ownStatDict)
            {
                _entityStat.AddMultiplier(item.Key, PartnerData.itemName, item.Value);
            }
        }

        public void LevelUp()
        {
            Level++;
        }
    }
}
