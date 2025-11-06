using System;
using Scripts.PlayerEquipments;
using Scripts.PlayerEquipments.PartnerSystem;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.Players;
using Work.Common.Core;
using Work.Core;
using Work.Item;

namespace UI.Inventory.EquipInventory
{
    public class EquipInventoryButtons
    {
        private InventoryEquipButton[] _inventoryEquipButton;
        private InventoryEquipButton _prevEquipButton;
        private readonly TogglePopupEnable _togglePopupEvent = UIEventChannel.TogglePopupEnable;
        private IEquipManager _equipManager;
        public event Action<InventoryEquipButton, bool> OnEquipButtonClicked;

        public EquipInventoryButtons(InventoryEquipButton[] inventoryEquipButtons, IEquipManager manager)
        {
            _equipManager = manager;
            foreach (var equip in _inventoryEquipButton = inventoryEquipButtons)
            {
                _equipManager.Sockets[equip.Index].OnChange += equip.SetEquip;
                _equipManager.Sockets[equip.Index].Reload();
                equip.SubscribeSelectButton(HandleSelectButton);
            }
            
            for (int i = 0; i < _inventoryEquipButton.Length; i++)
            {
                var button = _inventoryEquipButton[i];
                var socket = _equipManager.Sockets[i];
                if (socket == null) continue;

                if (socket is SkillSocket skillSock)
                {
                    var skill = skillSock.CurrentSkill;
                    if (skill != null && skill.SkillData != null)
                    {
                        button.SetEquip(skill.SkillData);
                        button.SetStatus(false);
                    }
                }
            }
        }

        private void HandleSelectButton(int idx)
        {
            var equipButton = _inventoryEquipButton[idx];

            if (_prevEquipButton == equipButton)
            {
                equipButton.SetStatus(false);
                OnEquipButtonClicked?.Invoke(equipButton, false);
                _prevEquipButton = null;
                return;
            }

            _prevEquipButton?.SetStatus(false);
            equipButton.SetStatus(true);
            OnEquipButtonClicked?.Invoke(equipButton, true);
            _prevEquipButton = equipButton;
        }
    }
}