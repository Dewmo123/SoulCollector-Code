using System;
using Inventory;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Work.Common.Core;
using Work.Core;
using Work.Item;
using Work.UI.Popup;

namespace UI.Inventory
{
    public class InventorySlotUI : SlotUI
    {
        [SerializeField] private ItemDataSO item;
        [SerializeField] private InventoryItemPopup popup;
        [SerializeField] private TextMeshProUGUI statusText;
        
        public ItemDataSO Item 
        { 
            get => item;
            set => item = value;
        }

        private readonly UIPopupEvent popupEvent = UIEventChannel.UIPopupEvent;
        private readonly SelectInventoryItemEvent _selectEvt = InventoryEventChannel.SelectInventoryItemEvent;
        
        public override void InitUI()
        {
            base.InitUI();
            button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            GameEventBus.RaiseEvent(popupEvent.Initializer(popup, Item));
            
            if (Item is EquipableItemSO)
            {
                GameEventBus.RaiseEvent(_selectEvt.Initializer(this));
            }
        }

        public override void EnableFor(InventoryItem item)
        {
            base.EnableFor(item);
            Item = item.data;
        }

        public void SetStatusText(string text, Color color, bool isActive = true)
        {
            statusText.gameObject.SetActive(isActive);
            statusText.text = text;
            statusText.color = color;
        }
    }
}