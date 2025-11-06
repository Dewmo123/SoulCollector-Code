using System;
using DewmoLib.Dependencies;
using UI.Base;
using UI.Core;
using UnityEngine;
using Work.Common.Core;

namespace Work.Core
{
    public enum InventoryPopupType
    {
        Normal,
        Equip,
        Enchant
    }
    
    public class UIEventManager : MonoBehaviour
    {
        [Inject] private UIManager _uiManager;

        private void Start()
        {
            GameEventBus.AddListener<UIPopupEvent>(HandlePopupEvent);
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<UIPopupEvent>(HandlePopupEvent);
        }
        private void HandlePopupEvent(UIPopupEvent evt)
        {
            _uiManager.ShowPopup(evt.popup, evt.data, evt.hasOption, evt.callback);
        }
    }
}