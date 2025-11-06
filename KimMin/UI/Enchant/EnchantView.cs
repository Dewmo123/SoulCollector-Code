using DewmoLib.Dependencies;
using DG.Tweening;
using Inventory;
using Scripts.Players.Storages;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;
using Work.Core;
using Work.Item;

namespace Work.UI.Enchant
{
    public class EnchantView : MonoBehaviour
    {
        [SerializeField] private EnchantSlotUI slotUI;
        [SerializeField] private Button enchantButton;
        [SerializeField] private EnchantInfo prevInfo;
        [SerializeField] private EnchantInfo nextInfo;
        [SerializeField] private TextMeshProUGUI chanceText;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Image resultImage;
        [SerializeField] private TextMeshProUGUI needText;
        [SerializeField] private Image needImage;
        [Inject] private PlayerInfoStorage _storage;

        private readonly Color32 sucessColor = new(175, 255, 175, 255);
        private readonly Color32 failColor = new(255, 175, 175, 255);

        public event Action<ItemDataSO, float> OnClickEnchant;
        private EquipableItemSO _currentItem;
        private float _chance;

        public void Initialize()
        {
            GameEventBus.AddListener<EnchantSelectEvent>(HandleEnchantSelect);
            enchantButton.onClick.AddListener(HandleEnchant);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<EnchantSelectEvent>(HandleEnchantSelect);
            enchantButton.onClick.RemoveListener(HandleEnchant);
        }

        private void HandleEnchant()
        {
            if (_currentItem == null) return;
            OnClickEnchant?.Invoke(_currentItem, _chance);
        }

        private void HandleEnchantSelect(EnchantSelectEvent evt)
        {
            slotUI.EnableFor(evt.item);
            _currentItem = evt.item;
            SetNeedInfo(evt.item);
            UpdateStatus(evt.item);
        }

        private void SetNeedInfo(EquipableItemSO item)
        {
            int level = item.itemType switch
            {
                ItemType.Buddy => _storage.PartnerStorage.Partners[_currentItem.itemName].Level,
                ItemType.Skill => _storage.SkillStorage.Skills[_currentItem.itemName].Level,
                _ => 0
            };
            Scripts.PlayerEquipments.EnchantInfo info = item.enchantInfo.enchantInfos[level];
            needImage.sprite = info.needType.itemIcon;
            needText.text = info.needAmount.ToString();
        }

        private void UpdateStatus(EquipableItemSO item)
        {
            int level = 0, upgrade = 0;
            if (item.itemType == ItemType.Skill)
            {
                level = _storage.SkillStorage.Skills[item.itemName].Level;
                upgrade = _storage.SkillStorage.Skills[item.itemName].Upgrade;
            }
            else if (item.itemType == ItemType.Buddy)
            {
                level = _storage.PartnerStorage.Partners[item.itemName].Level;
                upgrade = _storage.PartnerStorage.Partners[item.itemName].Upgrade;
            }
            prevInfo.EnableFor(item, level);
            nextInfo.EnableFor(item, level + 1);
            _chance = Mathf.Clamp((100 - 5 * level) / 100f, 0.05f, 1f);
            chanceText.gameObject.SetActive(true);
            if (item.GetMaxLevel(upgrade) <= level)
            {
                chanceText.SetText("최대 레벨입니다. 중첩을 늘리세요");
                enchantButton.interactable = false;
            }
            else
            {
                chanceText.text = $"성공확률 {_chance * 100f:f2}%";
                enchantButton.interactable = true;
                SetNeedInfo(_currentItem);
            }
        }

        public void OnTryEnchant(ItemDataSO item, bool isSucess)
        {
            if (isSucess)
                UpdateStatus(item as EquipableItemSO);

            ShowResult(isSucess);
        }

        private void ShowResult(bool isSucess)
        {
            resultImage.gameObject.SetActive(true);

            resultText.color = isSucess ? sucessColor : failColor;
            resultText.text = isSucess ? "성공" : "실패";
            DOTween.To(() => new Vector2(500, 100f), size => resultImage.rectTransform.sizeDelta =
                size, new Vector2(500f, 0f), 0.5f).SetEase(Ease.InExpo).OnComplete(() =>
            {
                resultImage.gameObject.SetActive(false);
            });
        }
    }
}