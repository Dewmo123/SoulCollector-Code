using Scripts.UI;
using System;
using UnityEngine;
using Work.Common.Core;

namespace Work.KimMin._01_Code.UI.MainMenu
{
    public class MenuButtonEvent : GameEvent
    {
        public MenuButton button;

        public MenuButtonEvent Initializer(MenuButton button)
        {
            this.button = button;
            return this;
        }
    }

    public static class MenuButtonEventChannel { public static MenuButtonEvent MenuButtonEvent = new(); }
    
    public class MenuButtonManager : MonoBehaviour
    {
        private MenuButton _prevButton;
        private MenuButton[] _buttons;
        private void Awake()
        {
            _buttons = GetComponentsInChildren<MenuButton>();
            GameEventBus.AddListener<SetMenuButtonEnableEvent>(HandleButtonEnableEvent);
            GameEventBus.AddListener<MenuButtonEvent>(HandleChangeMenu);
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<SetMenuButtonEnableEvent>(HandleButtonEnableEvent);
            GameEventBus.RemoveListener<MenuButtonEvent>(HandleChangeMenu);
        }
        private void HandleButtonEnableEvent(SetMenuButtonEnableEvent @event)
        {
            foreach (var button in _buttons)
            {
                button.SetStatus(false);
                button.SetEnable(@event.enabled);
            }
        }
        private void HandleChangeMenu(MenuButtonEvent evt)
        {
            if (_prevButton != null && evt.button == _prevButton)
            {
                _prevButton.SetStatus(false);
                _prevButton = null;
                return;
            }
            
            _prevButton?.SetStatus(false, false);
            evt.button.SetStatus(true);
            _prevButton = evt.button;
        }
    }
}