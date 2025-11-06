using Cysharp.Threading.Tasks;
using Scripts.Enemies;
using Scripts.Entities;
using Scripts.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Work.Common.Core;

namespace Scripts.StageSystem.States
{
    public class CommonState : StageState
    {
        private bool _isPlayerDead, _isChanged;
        private HashSet<Entity> _enemys;
        private StageSO _stageSO;
        public CommonState(StageManager stageManager) : base(stageManager)
        {
            _enemys = new();
        }
        public override void Enter(BaseStageDataSO data)
        {
            base.Enter(data);
            _isChanged = false;
            _stageSO = data as StageSO;
            Debug.Assert(_stageSO != null, "Invalid SO");
            _stageManager.ChangeBackground(_stageSO);
            CheckNextStage();
            SetProgressUI();
        }
        private void SpawnEnemys()
        {
            for (int i = 0; i < _stageSO.enemyCount.GetValue(_stage); i++)
            {
                EnemySO enemySO = _stageSO.GetRandomEnemy();
                Entity enemy = _stageManager.SpawnEnemy(enemySO, _stageManager.spawnPoint.RandomPointInRange(), HandleEnemyDead);
                _enemys.Add(enemy);
            }
        }
        private async void StartStage(int stage)
        {
            bool success = true;
            success = await _storage.ChapterStorage.ChangeStage(stage);
            RestartStage();
        }
        private async void RestartStage()
        {
            _stageSO.RemoveMultiplier();
            _stageSO.ApplyMultiplier(_stage);
            SetProgressUI();
            string stageText = $"{_stageSO.stageName} {_chapter} - {_stage}";
            GameEventBus.RaiseEvent(UIEvents.SetStageTextEvent.Init(stageText));
            GameEventBus.RaiseEvent(StageEvents.MoveEvent.Init(_stageSO.velocity));
            await Awaitable.WaitForSecondsAsync(3f);
            if (!_isPlayerDead && !_isChanged)
            {
                GameEventBus.RaiseEvent(StageEvents.StopEvent);
                SpawnEnemys();
            }
        }
        private void SetProgressUI()
        {
            string text = $"{_stageEnemyCount} / {_stageSO.nextStageCount.GetValue(_stage)}";
            GameEventBus.RaiseEvent(UIEvents.SetStageProgressEvent.Init(text, _stageEnemyCount, _stageSO.nextStageCount.GetValue(_stage)));
        }
        private async void HandleEnemyDead(Entity entity)
        {
            _enemys.Remove(entity);
            entity.OnDeadEvent.RemoveListener(HandleEnemyDead);
            bool isLast = _enemys.Count == 0;
            if (!_isPlayerDead)
            {
                await EnemyDeadTask();
                if (isLast)
                    CheckNextStage();
            }
        }

        private async Task EnemyDeadTask()
        {
            foreach (var item in _stageSO.dropInfos)
            {
                if (Random.value < item.percent)
                    await _storage.GoodsStorage.ChangeGoods(item.type, item.GetValue(_stage));
            }
            await _storage.ChapterStorage.EnemyDead(1);
            SetProgressUI();
            GameEventBus.RaiseEvent(StageEvents.DropItemEvent);
        }

        private void CheckNextStage()
        {
            if (_stageEnemyCount >= _stageSO.nextStageCount.GetValue(_stage))
            {
                if (_stage == _stageSO.maxStageCount)
                {
                    GameEventBus.RaiseEvent(StageEvents.BossKeyEnableEvent);
                    RestartStage();
                }
                else
                    StartStage(1);
            }
            else
                RestartStage();
        }
        public override void Exit()
        {
            base.Exit();
            _isChanged = true;
            foreach (var item in _enemys)
            {
                item.OnDeadEvent.RemoveListener(HandleEnemyDead);
                item.SetDead();
            }
            _enemys.Clear();
            _stageSO?.RemoveMultiplier();
        }
        public override async void OnPlayerDead(Entity entity)
        {
            base.OnPlayerDead(entity);
            _isPlayerDead = true;
            await UniTask.WaitUntil(() => _enemys.Count == 0);
            entity.Revive();
            _isPlayerDead = false;
            RestartStage();
        }
    }
}
