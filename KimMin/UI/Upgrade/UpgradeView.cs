using Scripts.Environments;
using Scripts.StatSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.Upgrade;

namespace Work.UI
{
    public interface IUpgradeView
    {
        void SetUpUI(UpgradeDataSO data);
        void UpdateUI(UpgradeDataSO data, StatSO stat, int level);
        public event Action OnUpgradeClicked;
    }

    public class UpgradeView : MonoBehaviour, IUpgradeView
    {
        public event Action OnUpgradeClicked;

        [SerializeField] private StatSO stat;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI statIncresaseText;
        [SerializeField] private TextMeshProUGUI statText;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        private UpgradeDataSO _data;

        public void SetUpUI(UpgradeDataSO data)
        {
            _data = data;
            titleText.text = data.upgradeStat.description;
            icon.sprite = data.upgradeStat.Icon;
            button.onClick.AddListener(HandleUpgradeClicked);
        }

        private void HandleUpgradeClicked()
        {
            OnUpgradeClicked?.Invoke();
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        public void UpdateUI(UpgradeDataSO data, StatSO stat, int level)
        {
            levelText.text = $"({level}/{data.maxLevel})";
            priceText.text = $"{NumberFormatter.Format(data.GetNextUpgradeCost(level))}";
            if (stat.IsPercent)
            {
                statText.text = string.Format("{0:F2}%", stat.Value * 100);
                statIncresaseText.text = string.Format("+{0:F2}%", data.GetNextLevelUpIncrease(stat, level) * 100f);
            }
            else
            {
                statText.text = string.Format("{0:F2}", NumberFormatter.Format(stat.Value, 2));
                statIncresaseText.text = string.Format("+{0:F2}", data.GetNextLevelUpIncrease(stat, level));
            }
        }
    }
}