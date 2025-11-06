using System;
using System.Collections.Generic;
using DewmoLib.Dependencies;
using Inventory;
using Scripts.Players.Storages;
using TMPro;
using UI.Base;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;
using Work.Core;
using Work.Item;

namespace Work.UI.Popup
{
    [Serializable]
    public class InventoryItemPopup : BasePopup<EquipableItemSO>
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI tier;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button selectButton;
        [SerializeField] private TextMeshProUGUI statText;
        [SerializeField] private Transform statRoot;
        [SerializeField] private Image upgradeBar;
        [SerializeField] private TextMeshProUGUI barText;
        [SerializeField] private TextMeshProUGUI upgradeText;
        [field: SerializeField] public InventoryPopupType PopupType { get; private set; }

        private List<TextMeshProUGUI> _statTexts = new();
        [Inject] PlayerInfoStorage _storage;
        public override void EnableFor(EquipableItemSO item, bool hasOption = false, Action<ItemDataSO> callback = null)
        {
            title.text = item.itemName;
            description.text = item.itemDescription;
            tier.text = item.itemTier.ToString();
            icon.sprite = item.itemIcon;
            selectButton.gameObject.SetActive(false);

            if (hasOption)
            {
                selectButton.gameObject.SetActive(true);
                selectButton.onClick.AddListener(() => HandleClickSelectButton(item, callback));
            }

            for (int i = _statTexts.Count; i < item.ownStatDict.Count; i++)
            {
                _statTexts.Add(Instantiate(statText, statRoot));
            }
            
            _statTexts.ForEach(item => item.gameObject.SetActive(false));

            int idx = 0;
            foreach (var stat in item.ownStatDict)
            {
                _statTexts[idx].gameObject.SetActive(true);
                _statTexts[idx++].text = $"{stat.Key.description} +{stat.Value * 100}%";
            }
            int currentAmount = item.itemType switch
            {
                ItemType.Buddy => _storage.PartnerStorage.Partners[item.itemName].Amount,
                ItemType.Skill => _storage.SkillStorage.Skills[item.itemName].Amount,
                _ => 0
            };
            int currentUpgrade = item.itemType switch
            {
                ItemType.Buddy => _storage.PartnerStorage.Partners[item.itemName].Upgrade,
                ItemType.Skill => _storage.SkillStorage.Skills[item.itemName].Upgrade,
                _ => 0
            };
            int needAmount = 1;
            if(item.upgradeInfos.Count > currentUpgrade)
            {
                needAmount = item.upgradeInfos[currentUpgrade].needAmount;
            }
            upgradeBar.fillAmount = (float)currentAmount / needAmount;
            barText.SetText($"{currentAmount}/{needAmount}");
            upgradeText.SetText($"{currentUpgrade} ÁßÃ¸");
            closeButton.onClick.AddListener(HandleClose);
            button.onClick.AddListener(HandleClose);
            UIUtility.ShowUI(gameObject);
        }

        private void HandleClickSelectButton(ItemDataSO item, Action<ItemDataSO> callback = null)
        {
            callback?.Invoke(item);
            selectButton.onClick.RemoveAllListeners();
            UIUtility.ShowUI(gameObject, false);
        }

        private void HandleClose()
        {
            UIUtility.ShowUI(gameObject, false);
            button.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
        }
    }
}