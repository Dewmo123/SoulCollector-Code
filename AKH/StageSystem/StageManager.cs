using AYellowpaper.SerializedCollections;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Enemies;
using Scripts.Entities;
using Scripts.Players;
using Scripts.Players.Storages;
using Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Work.Common.Core;
using Work.Core;
using Work.Dungeon;

namespace Scripts.StageSystem
{

    public class StageManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer backgroundRenderer;
        [SerializeField] private SpriteRenderer groundRenderer;
        [SerializeField] private PoolItemSO enemyItem;
        [SerializeField] private GameDB _gameDBAsset;
        public EnemySpawnPoint spawnPoint;

        private StageStateMachine _stateMachine;
        private BaseStageDataSO _currentStage;
        private readonly ShowBossPopup _bossPopup = UIEventChannel.ShowBossPopup;

        [Inject] public PlayerInfoStorage Storage;
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private Player _player;
        private void Awake()
        {
            _player.OnDeadEvent.AddListener(HandlePlayerDead);
            GameEventBus.AddListener<EnterDungeonEvent>(HandleEnterDungeon);
            GameEventBus.AddListener<EnterBossEvent>(HandleEnterBoss);
            _stateMachine = new(this);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<EnterDungeonEvent>(HandleEnterDungeon);
            GameEventBus.RemoveListener<EnterBossEvent>(HandleEnterBoss);
        }

        private void Start()
        {
            ChangeState(StageStateEnum.Common, Storage.ChapterStorage.Chapter.Chapter.ToString()); // Assuming a default common stage ID
        }
        private void HandleEnterDungeon(EnterDungeonEvent @event)
        {
            if (_player.IsDead)
                return;
            ChangeState(StageStateEnum.Dungeon, @event.dungeonName);
        }
        private void HandleEnterBoss(EnterBossEvent @event)
        {
            _stateMachine.ChangeState(StageStateEnum.Boss, _currentStage);
        }
        private void HandlePlayerDead(Entity entity)
        {
            _stateMachine.CurrentState?.OnPlayerDead(entity);
        }

        public Entity SpawnEnemy(EnemySO enemySO,Vector3 position,UnityAction<Entity> deadCallback, bool isBoss = false)
        {
            if (isBoss) GameEventBus.RaiseEvent(_bossPopup.Initializer(enemySO));
            
            var enemy = _poolManager.Pop<Enemy>(enemyItem);
            enemy.SetUpEnemy(enemySO, _player, position);
            enemy.OnDeadEvent.AddListener(deadCallback);
            return enemy;
        }
        public void ChangeBackground(BaseStageDataSO stageData)
        {
            backgroundRenderer.sprite = stageData.backgroundImage;
            groundRenderer.sprite = stageData.groundImage;
        }
        private void Update()
        {
            _stateMachine?.UpdateState();
        }
        public void ChangeState(StageStateEnum newState, string stageDataId = null, BaseStageDataSO dynamicData = null)
        {
            BaseStageDataSO dataToPass = dynamicData;

            if (dataToPass == null && !string.IsNullOrEmpty(stageDataId))
            {
                if (newState == StageStateEnum.Common || newState == StageStateEnum.Boss)
                {
                    dataToPass = _gameDBAsset.GetStageData<StageSO>(stageDataId);
                }
                else if (newState == StageStateEnum.Dungeon)
                {
                    dataToPass = _gameDBAsset.GetStageData<DungeonSO>(stageDataId);
                }
                // Add more conditions for other stage types
            }

            if (dataToPass == null)
            {
                Debug.LogError($"StageManager: Failed to get stage data for state {newState} with ID {stageDataId}");
                return;
            }
            _currentStage = dataToPass;
            _stateMachine.ChangeState(newState, dataToPass);
        }
    }
}
