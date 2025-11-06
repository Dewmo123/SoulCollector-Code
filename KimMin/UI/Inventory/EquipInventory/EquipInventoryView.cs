using System;
using Inventory;
using UnityEngine;
using Scripts.PlayerEquipments.SkillSystem;
using Work.Common.Core;
using Work.Core;
using Work.Item;

namespace UI.Inventory.EquipInventory
{
    public class EquipInventoryView : MonoBehaviour
    {
        private readonly AddBuddyEvent _addBuddyEvent = InventoryEventChannel.AddBuddyEvent;
        private readonly EquipSkillEvent _addSkillEvent = InventoryEventChannel.EquipSkillEvent;
        private readonly TogglePopupEnable _togglePopupEvent = UIEventChannel.TogglePopupEnable;
        private readonly Color32 color = new Color32(150, 255, 150, 255);
        
        private InventoryEquipButton _prevEquipButton;


        private void Awake()
        {
            GameEventBus.AddListener<SelectInventoryItemEvent>(HandleSelectItem);
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<SelectInventoryItemEvent>(HandleSelectItem);
        }
        private void OnEnable()
        {
            _prevEquipButton?.SetStatus(false);
            _prevEquipButton = null;
        }

        public void GetEquipButton(InventoryEquipButton button, bool isActive)
        {
            if (isActive)
            {
                _prevEquipButton = button;
                return;
            }

            if (button.Slot != null && button.Slot.Item is EquipableItemSO slotItem && slotItem.isEquipped)
            {
                EquipItem(button.Slot, false);
                _prevEquipButton = null;
                return;
            }

            if (button.Item != null && button.Item.isEquipped)
            {
                var item = button.Item;
                item.isEquipped = false;
                button.SetEquip(null);
                button.SetStatus(false);

                GameEvent evt = null;
                if (item.itemType == ItemType.Skill)
                    evt = _addSkillEvent.Initializer(null, button.Index);
                else if (item.itemType == ItemType.Buddy)
                    evt = _addBuddyEvent.Initializer(null, button.Index);

                if (evt != null)
                {
                    GameEventBus.RaiseEvent(evt);
                    GameEventBus.RaiseEvent(_togglePopupEvent.Initializer(true));
                }
                _prevEquipButton = null;
                return;
            }

            button.SetStatus(false);
            _prevEquipButton = null;
        }

        private void HandleSelectItem(SelectInventoryItemEvent evt)
        {
            var item = evt.item.Item as EquipableItemSO;
            if (item == null) return;
            if (_prevEquipButton == null) return;

            _prevEquipButton.Slot = evt.item;

            if (_prevEquipButton.ReadyToSelect)
            {
                if (item.isEquipped)
                    _prevEquipButton.SetStatus(true);
                else
                    EquipItem(evt.item, true);
            }
        }

        public void EquipItem(InventorySlotUI slot, bool isEquip)
        {
            if (slot == null)
                return;
            if (slot.Item is not EquipableItemSO item)
                return;

            int idx = _prevEquipButton.Index;
            GameEvent gameEvent = null;

            _prevEquipButton.Item = item;
            slot.Item = item;
            item.isEquipped = isEquip;

            if (isEquip)
                slot.SetStatusText("장착중", color);
            else
                slot.SetStatusText("", Color.white, false);

            switch (item.itemType)
            {
                case ItemType.Buddy:
                    var buddy = isEquip ? item as BuddySO : null;
                    gameEvent = _addBuddyEvent.Initializer(buddy, idx);
                    break;
                case ItemType.Skill:
                    var skill = isEquip ? item as SkillDataSO : null;
                    gameEvent = _addSkillEvent.Initializer(skill, idx);
                    break;
            }

            if (gameEvent != null)
            {
                _prevEquipButton.SetEquip(isEquip ? item : null);
                _prevEquipButton.SetStatus(false);
                GameEventBus.RaiseEvent(gameEvent);
                GameEventBus.RaiseEvent(_togglePopupEvent.Initializer(true));
            }
        }
    }
}