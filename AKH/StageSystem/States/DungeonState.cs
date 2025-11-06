using Cysharp.Threading.Tasks;
using Inventory;
using Scripts.Enemies;
using Scripts.Entities;
using Scripts.Goods;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.StageSystem;
using Scripts.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Work.Common.Core;
using Work.Core;
using Work.Item; // For Debug.LogError

namespace Scripts.StageSystem.States
{
    public class DungeonState : StageState
    {
        private DungeonSO _dungeonSO;
        private int _bossCount;
        public DungeonState(StageManager stageManager) : base(stageManager)
        {
        }
        public override void Enter(BaseStageDataSO data)
        {
            base.Enter(data);
            _bossCount = 0;
            _dungeonSO = data as DungeonSO;
            Debug.Assert(_dungeonSO != null, "Invalid SO");
            _dungeonSO.ApplyMultiplier();
            GameEventBus.RaiseEvent(StageEvents.StopEvent);
            GameEventBus.RaiseEvent(UIEvents.SetMenuButtonEnableEvent.Init(false));
            GameEventBus.RaiseEvent(UIEvents.SetStageTextEvent.Init(_dungeonSO.stageName));
            SetProgressUI();
            SpawnBoss(_dungeonSO.dungeonEnemies[_bossCount]);
            // Dungeon-specific entry logic here
        }
        public override void Exit()
        {
            base.Exit();
            _dungeonSO.RemoveMultiplier();
            GameEventBus.RaiseEvent(UIEvents.SetMenuButtonEnableEvent.Init(true));
        }
        private void SpawnBoss(EnemySO enemySO)
        {
            _stageManager.SpawnEnemy(enemySO, _stageManager.spawnPoint.GetMiddlePos(), HandleEnemyDead);
        }

        private void SetProgressUI()
        {
            string text = $"{_bossCount} / {_dungeonSO.dungeonEnemies.Length}";
            GameEventBus.RaiseEvent(UIEvents.SetStageProgressEvent.Init(text, _bossCount, _dungeonSO.dungeonEnemies.Length));
        }

        private async void HandleEnemyDead(Entity boss)
        {
            boss.OnDeadEvent.RemoveListener(HandleEnemyDead);
            _bossCount++;
            SetProgressUI();
            if (_dungeonSO.dungeonEnemies.Length == _bossCount)
            {
                await UniTask.WaitForSeconds(0.5f);
                var evt = UIEventChannel.UIPopupEvent.Initializer(_dungeonSO.resultPopupPrefab,_dungeonSO,true,HandlePopupEvent);
                GameEventBus.RaiseEvent(evt);
                GameEventBus.RaiseEvent(StageEvents.MoveEvent);
            }
            else
            {
                SpawnBoss(_dungeonSO.dungeonEnemies[_bossCount]);
            }
        }
        public override void OnPlayerDead(Entity entity)
        {
            base.OnPlayerDead(entity);
            var evt = UIEventChannel.UIPopupEvent.Initializer(_dungeonSO.resultPopupPrefab, _dungeonSO, false, HandlePopupEvent);
            GameEventBus.RaiseEvent(evt);
        }
        private async void HandlePopupEvent(ItemDataSO sO)
        {
            foreach(var item in _dungeonSO.rewards)
            {
                if(item.Key is SkillDataSO skill)
                {
                    GameEventBus.RaiseEvent(InventoryEventChannel.AddSkillAmountEvent.Init(skill, item.Value));
                }
                else if(item.Key is BuddySO buddy)
                {
                    GameEventBus.RaiseEvent(InventoryEventChannel.AddBuddyEvent.Initializer(buddy, item.Value));
                }
                else if( item.Key is GoodsSO goods)
                {
                    await _storage.GoodsStorage.ChangeGoods(goods.goodsType, item.Value);
                }
            }
            _stageManager.ChangeState(StageStateEnum.Common, _chapter.ToString());
        }
    }
}
