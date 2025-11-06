using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Work.Item;

namespace UI.Inventory
{
    public class InventoryEquipButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image highlightEffect;
        [SerializeField] private Button selectButton;
        [SerializeField] private Sprite originIcon;

        public int Index => transform.GetSiblingIndex();
        public InventorySlotUI Slot { get; set; }
        public EquipableItemSO Item { get; set; }
        public bool ReadyToSelect { get; private set; }
        private Sequence _seq;

        private void Awake()
        {
            _seq = DOTween.Sequence();
            _seq.Append(highlightEffect.transform.DOScale(1.3f, 0.6f))
                .Join(highlightEffect.DOFade(0f, 0.6f))
                .SetLoops(-1, LoopType.Restart);

        }

        public void SubscribeSelectButton(UnityAction<int> callback)
            => selectButton.onClick.AddListener(() => callback(Index));

        public void SetStatus(bool status)
        {
            if (status)
                OnActive();
            else
            {
                OnDeactive();
                Slot?.SetStatusText(string.Empty, Color.white, false);
            }
        }

        private void OnActive()
        {
            highlightEffect.gameObject.SetActive(true);
            _seq.Restart();
            ReadyToSelect = true;
        }

        private void OnDeactive()
        {
            highlightEffect.gameObject.SetActive(false);
            ReadyToSelect = false;
        }

        public void SetEquip(EquipableItemSO equipItem = null)
        {
            if (Item != null && equipItem == null)
            {
                Item.isEquipped = false;
                Item = null;
            }
            else if (equipItem != null)
            {
                Item = equipItem;
                Item.isEquipped = true;
            }

            icon.sprite = equipItem == null ? originIcon : equipItem.itemIcon;
        }
    }
}