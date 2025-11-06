using Scripts.Network;
using Scripts.Players.Storages;
using Scripts.StageSystem;
using System;
using Work.Common.Core;
using Work.Quest;
using Work.UI.Gacha;

namespace Work.UI.Quest
{
    public class QuestModel
    {
        private readonly QuestSequence _questSequence;
        private readonly PlayerInfoStorage _playerInfoStorage;
        private int index = 0;
        private int _cnt = 0;
        private QuestSO _currentQuest;

        public event Action<QuestSO> OnNewQuestStart;
        public event Action<int> OnProgressChanged;

        public QuestModel(QuestSequence questSequence, PlayerInfoStorage playerInfoStorage)
        {
            _questSequence = questSequence;
            _playerInfoStorage = playerInfoStorage;

            GameEventBus.AddListener<ChangeStatEvent>(HandleStatChanged);
            GameEventBus.AddListener<DropItemEvent>(HandleEnemyDead);
            GameEventBus.AddListener<GachaEvent>(HandleGacha);
        }

        private void HandleGacha(GachaEvent evt)
        {
            if (_currentQuest.questType != QuestType.RollItem
                || _currentQuest.itemType != evt.item.itemType) return;

            OnProgressChanged?.Invoke(_cnt++);
        }

        private void HandleEnemyDead(DropItemEvent evt)
        {
            if (_currentQuest.questType != QuestType.DefeatMob) return;
            OnProgressChanged?.Invoke(_cnt++);
        }

        private void HandleStatChanged(ChangeStatEvent evt)
        {
            if (_currentQuest.questType != QuestType.UpgradeLevel || evt.statType != _currentQuest.statType)
                return;
            _cnt += evt.next - evt.prev;
            OnProgressChanged?.Invoke(_cnt);
        }

        public async void CompleteQuest()
        {
            await _playerInfoStorage.GoodsStorage.ChangeGoods(GoodsType.Crystal, 200);
        }

        public void SetNewGetNewQuest()
        {
            var nextQuest = _questSequence.quests[index++];
            _currentQuest = nextQuest;

            if (index >= _questSequence.quests.Count)
            {
                index = 0;
            }

            OnNewQuestStart?.Invoke(nextQuest);
            OnProgressChanged?.Invoke(0);

            _cnt = 0;
        }
    }
}