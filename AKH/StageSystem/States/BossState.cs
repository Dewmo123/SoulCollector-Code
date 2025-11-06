using Scripts.Entities;
using Scripts.UI;
using UnityEngine;
using Work.Common.Core;

namespace Scripts.StageSystem.States
{
    public class BossState : StageState
    {
        private Entity _boss;
        private StageSO _stageSO;
        public BossState(StageManager stageManager) : base(stageManager)
        {
        }
        public override void Enter(BaseStageDataSO data)
        {
            base.Enter(data);
            GameEventBus.RaiseEvent(StageEvents.StopEvent);
            _stageSO = data as StageSO;
            Debug.Assert(_stageSO != null, "Invalid SO");
            _stageSO.EnterBoss();
            _boss = _stageManager.SpawnEnemy(_stageSO.boss, _stageManager.spawnPoint.GetMiddlePos(), OnBossDead, true);
        }
        public override void OnPlayerDead(Entity entity)
        {
            base.OnPlayerDead(entity);
            entity.Revive();
            _stageManager.ChangeState(StageStateEnum.Common, dynamicData: _stageSO);
        }
        private async void OnBossDead(Entity enemy)
        {
            enemy.OnDeadEvent.RemoveListener(OnBossDead);
            bool success = await _storage.ChapterStorage.ChangeChapter(1);
            if (success)
                _stageManager.ChangeState(StageStateEnum.Common, _chapter.ToString());
        }
        public override void Exit()
        {
            base.Exit();
            _stageSO.ExitBoss();
            if (!_boss.IsDead)
            {
                _boss.OnDeadEvent.RemoveListener(OnBossDead);
                _boss.SetDead();
            }
        }
    }
}
