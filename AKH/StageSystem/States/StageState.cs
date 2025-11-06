using Scripts.Entities;
using Scripts.Players;
using Scripts.Players.Storages;

namespace Scripts.StageSystem.States
{
    public abstract class StageState
    {
        protected StageManager _stageManager;
        protected BaseStageDataSO _currentStageData;

        protected int _chapter => _storage.ChapterStorage.Chapter.Chapter;
        protected int _stage => _storage.ChapterStorage.Chapter.Stage;
        protected int _stageEnemyCount => _storage.ChapterStorage.Chapter.EnemyCount;
        protected PlayerInfoStorage _storage;
        public StageState(StageManager stageManager)
        {
            _stageManager = stageManager;
            _storage = _stageManager.Storage;
        }
        public virtual void Enter(BaseStageDataSO data)
        {
            _currentStageData = data;
            _stageManager.ChangeBackground(data);
        }
        public virtual void Update() { }
        public virtual void Exit() { }
        public virtual void OnPlayerDead(Entity entity) { }
    }
}
