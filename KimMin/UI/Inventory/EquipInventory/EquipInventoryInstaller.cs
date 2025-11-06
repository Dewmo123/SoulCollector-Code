using DewmoLib.Dependencies;
using Scripts.Entities;
using Scripts.PlayerEquipments;
using Scripts.PlayerEquipments.PartnerSystem;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.Players;
using System;
using System.Collections.Generic;
using UnityEngine;
using Work.Common.Core;
using Work.Core;

namespace UI.Inventory.EquipInventory
{
    public class EquipInventoryInstaller : MonoBehaviour
    {
        [SerializeField] private InventoryOptionButton[] optionButtons;
        [SerializeField] private EquipInventoryView inventoryView;
        [Inject] private Player _player;
        private static readonly Dictionary<ItemType, Type> _managerTypes = new()
        {
            {ItemType.Buddy,typeof(PartnerManager) }
            ,{ItemType.Skill,typeof(PlayerSkillManager)}
        };
        
        private InventoryOptionButton _prevButton;
        private readonly TogglePopupEnable _togglePopupEvent = UIEventChannel.TogglePopupEnable;
        
        private void Start()
        {


            foreach (var option in optionButtons)
            {
                var buttons = option.EquipButtons;
                Type type = _managerTypes[option.ItemType];
                Debug.Assert(type != null, nameof(type) + "is null");
                IEquipManager manager = _player.GetCompo(type) as IEquipManager;
                var inventoryButtons = new EquipInventoryButtons(buttons, manager);
                inventoryButtons.OnEquipButtonClicked += inventoryView.GetEquipButton;
            }

            foreach (var button in optionButtons)
            {
                button.Button.onClick.AddListener(() => HandleButtonClick(button));
            }
        }

        private void HandleButtonClick(InventoryOptionButton button)
        {
            _prevButton?.SetActiveView(false);
            _prevButton?.Parent.SetActive(false);
            button.SetActiveView(true);
            button.Parent.SetActive(true);
            _prevButton = button;
            GameEventBus.RaiseEvent(_togglePopupEvent.Initializer(true));
        }
    }
}