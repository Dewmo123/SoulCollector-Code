using Inventory;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;
using Work.Core;
using Work.Item;
using Work.UI.Popup;

namespace Work.UI.Enchant
{
    public class EnchantItemSlotUI : SlotUI
    {
        [SerializeField] private InventoryItemPopup popup;

        private readonly UIPopupEvent popupEvent = UIEventChannel.UIPopupEvent;
        private ItemDataSO _item;
        private readonly EnchantSelectEvent _selectEvent = ItemEventChannel.EnchantSelectEvent;
        protected override void Awake()
        {
            base.Awake();
            button.onClick.AddListener(HandleClick);
        }
        private void HandleClick()
        {
            GameEventBus.RaiseEvent(popupEvent.Initializer(popup, _item, true, HandleAccept));
        }

        private void HandleAccept(ItemDataSO item)
        {
            GameEventBus.RaiseEvent(_selectEvent.Initializer(item as EquipableItemSO));
        }

        public override void EnableFor(InventoryItem item)
        {
            base.EnableFor(item);
            _item = item.data;
        }

        private void OnDisable() => Disable();

        public override void Disable()
        {
        }
    }
}