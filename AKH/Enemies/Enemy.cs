using DewmoLib.ObjectPool.RunTime;
using DewmoLib.Utiles;
using Scripts.Entities;
using Scripts.FSM;
using Scripts.Players;
using System;
using UnityEngine;

namespace Scripts.Enemies
{
    public class Enemy : Entity,IPoolable
    {
        [SerializeField] private PoolItemSO poolItem;
        [SerializeField] private StateDataSO[] states;
        public EnemySO CurrentSO { get; private set; }
        public PoolItemSO PoolItem => poolItem;
        public Entity Target { get; private set; }
        public GameObject GameObject => gameObject;

        private EntityStateMachine _stateMachine;

        private IChangeableCompo<EnemySO>[] _changeableCompos;
        private Pool _myPool;
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new(this, states);
            _changeableCompos = GetComponentsInChildren<IChangeableCompo<EnemySO>>();
        }
        public void SetUpEnemy(EnemySO newSO,Entity target,Vector3 position)
        {
            transform.position = position;
            IsDead = false;
            Target = target;
            ChangeSO(newSO);
            ChangeState("MOVE",true);
        }
        private void ChangeSO(EnemySO newSO)
        {
            foreach (var item in _changeableCompos)
                item.Change(CurrentSO, newSO);
            CurrentSO = newSO;
        }
        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
        }
        public override void SetDead()
        {
            base.SetDead();
            OnDeadEvent?.Invoke(this);
            _stateMachine.CurrentState?.Exit();
            _myPool.Push(this);
        }
        public void ChangeState(string stateName, bool forced = false) => _stateMachine.ChangeState(stateName, forced);
    }
}
