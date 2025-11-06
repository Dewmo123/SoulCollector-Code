using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DewmoLib.Dependencies;
using Inventory;
using UI.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using Work.Common.Core;
using Work.Core;
using Work.Min.Code.UI;
using Work.UI.Core;

namespace UI.Core
{
    public class UIManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private MessagePopup basePopupPrefab;
        [SerializeField] private List<BasePopup> popupPrefabs;
        [SerializeField] private SerializedDictionary<ScriptableObject, BasePopup> earlyPopups;
        private Dictionary<Type, IPopup> _popups = new();

        [Provide]
        public UIManager Provide() => this;
        
        private MessagePopup _popupInstance;
        private readonly Stack<BaseUI> _popupStack = new();
        private bool _disableMode = false;
        
        public static UIManager Instance { get; private set; }
        
        private void Start()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            
            foreach (var popup in popupPrefabs)
            {
                var popupType = popup.GetType();
                _popups[popupType] = popup as IPopup;
            }
            foreach(var item in earlyPopups)
            {
                Type type = item.Key.GetType();
                UIUtility.ShowUI(item.Value.gameObject, false);
                _popups[type] = item.Value as IPopup;
            }
            
            GameEventBus.AddListener<TogglePopupEnable>(HandleTogglePopup);
        }

        private void OnDisable()
        {
            GameEventBus.RemoveListener<TogglePopupEnable>(HandleTogglePopup);
        }

        private void HandleTogglePopup(TogglePopupEnable evt) => _disableMode = !evt.isActive;

        public MessagePopup ShowMessagePopup(string message, bool hasCancel = false,
            Action onAccept = null, Action onCancel = null)
        {
            if (_disableMode) return null;

            if (_popupInstance == null)
                _popupInstance = Instantiate(basePopupPrefab, rootCanvas.transform);
            
            _popupInstance.name = $"{basePopupPrefab.name}(prefab)";
            _popupInstance.InitUI();
            _popupInstance.ShowPopup(message, hasCancel, onAccept, onCancel);
            _popupStack.Push(_popupInstance);
            return _popupInstance;
        }
        
        public IPopup ShowPopup(BasePopup  popupPrefab, ScriptableObject data, bool hasOption = false, 
            Action<ItemDataSO> callback = null)
        {
            if (_disableMode) return null;
            
            if (!_popups.TryGetValue(data.GetType(), out var popup))
            {
                var newPopup = Instantiate(popupPrefab, rootCanvas.transform);
                _popups[data.GetType()] = newPopup as IPopup;
                popup = newPopup as IPopup;
            }
            
            popup?.EnableFor(data, hasOption, callback);
            return popup;
        }
        
        public void BindEvent(GameObject go, Action<PointerEventData> action, EUIEvent type)
        {
            UIEventHandler evt = UIUtility.GetOrAddComponent<UIEventHandler>(go);
            evt.BindUIEvent(go, action, type);
        }
    }
}