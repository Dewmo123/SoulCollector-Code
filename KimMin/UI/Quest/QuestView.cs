using System;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;
using Work.Core;
using Work.Quest;

namespace Work.UI.Quest
{
    public class QuestView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;
        [SerializeField] private Button questButton;
        [SerializeField] private RectTransform currencyRect;
        [SerializeField] private Sprite moneySprite;

        private float _target;
        private bool _isComplete;
        public event Action OnGetNewQuest;

        private void Awake()
        {
            questButton.onClick.AddListener(HandleQuestButtonClick);
        }

        private void OnDestroy()
        {
            questButton.onClick.RemoveListener(HandleQuestButtonClick);
        }

        private void HandleQuestButtonClick()
        {
            if (_isComplete)
            {
                UIUtility.PlayUIEffects(moneySprite, questButton.image.rectTransform, 75f,
                    currencyRect, 8, 0.5f);
                _isComplete = false;
                OnGetNewQuest?.Invoke();
            }
        }

        public void SetNewQuest(QuestSO quest)
        {
            string titleText = quest.questType switch
            {
                QuestType.DefeatMob => $"몹 {quest.startValue}마리 처지",
                QuestType.RollItem => $"{quest.itemType} {quest.startValue}번 뽑기",
                QuestType.UpgradeLevel => $"{Defines.GetStatFromType(quest.statType).description} " +
                                          $"{quest.startValue}번 강화",
                _ =>""
            };

            title.text = titleText;
            _target = quest.startValue;
        }

        public void UpdateQuest(int level)
        {
            if (level >= _target)
            {
                _isComplete = true;
                progress.text = "완료!";
                return;
            }
            
            progress.text = $"({level}/{_target})";
        }
    }
}