using System;
using System.Collections.Generic;
using UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using Work.UI.Core;

namespace Work.Min.Code.UI
{
    [DefaultExecutionOrder(-15)]
    public class UIEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public Dictionary<EUIEvent, Action<PointerEventData>> EventHandler { get; private set; }

        private void Awake()
        {
            EventHandler = new();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(EventHandler.TryGetValue(EUIEvent.PointerEnter, out var evt))
                evt?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(EventHandler.TryGetValue(EUIEvent.PointerExit, out var evt))
                evt?.Invoke(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(EventHandler.TryGetValue(EUIEvent.PointerClick, out var evt))
                evt?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(EventHandler.TryGetValue(EUIEvent.PointerDown, out var evt))
                evt?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(EventHandler.TryGetValue(EUIEvent.PointerUp, out var evt))
                evt?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(EventHandler.TryGetValue(EUIEvent.Drag, out var evt))
                evt?.Invoke(eventData);
        }
        
        public void BindUIEvent(GameObject go, Action<PointerEventData> action, EUIEvent type = EUIEvent.PointerClick)
        {
            UIEventHandler evt = UIUtility.GetOrAddComponent<UIEventHandler>(go);

            if (evt.EventHandler.ContainsKey(type))
            {
                evt.EventHandler[type] -= action;
                evt.EventHandler[type] += action;
            }
            else if (!evt.EventHandler.ContainsKey(type))
                evt.EventHandler[type] = action;
        }
    }
}