using Scripts.StageSystem;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;

namespace Work.Dungeon
{
    public class DungeonContentUI : MonoBehaviour, IUIElement<DungeonSO>
    {
        [SerializeField] private TextMeshProUGUI dungeonName;
        [SerializeField] private TextMeshProUGUI difficulty;
        [SerializeField] private TextMeshProUGUI keyRequire;
        [SerializeField] private Image keyImage;
        [SerializeField] private DungeonItemUI dungeonItem;
        [SerializeField] private Transform rewardRoot;
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private Button startButton;

        public void EnableFor(DungeonSO dungeon)
        {
            foreach (var item in dungeon.rewards)
            {
                Instantiate(dungeonItem, rewardRoot)
                    .EnableFor(item.Key.itemIcon, item.Value);
            }
            foreach (var item in dungeon.dungeonEnemies)
            {
                Instantiate(dungeonItem, enemyRoot)
                    .EnableFor(item.itemIcon, 0);
            }
            dungeonName.text = dungeon.stageName;
            difficulty.text = $"난이도 : {dungeon.dungeonDifficulty}";
            keyRequire.text = dungeon.keyRequire.ToString();
            keyImage.sprite = dungeon.key.itemIcon;
            startButton.onClick.AddListener(() => HandleStartDungeon(dungeon));
        }

        private void HandleStartDungeon(DungeonSO dungeon)
        {
            //던전 시작
            GameEventBus.RaiseEvent(StageEvents.EnterDungeonEvent.Init(dungeon.stageName));
        }

        public void Disable()
        {
            startButton.onClick.RemoveAllListeners();
        }
    }
}