using System;
using Inventory;
using Scripts.Enemies;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;

namespace Work.Core
{
    public static class UIEventChannel
    {
        public static UIPopupEvent UIPopupEvent = new();
        public static TogglePopupEnable TogglePopupEnable = new();
        public static PlayUIEffectEvent PlayUIEffectEvent = new();
        public static PlayUIEffectsEvent PlayUIEffectsEvent = new();
        public static ShowBossPopup ShowBossPopup = new();
    }

    public class UIPopupEvent : GameEvent
    {
        public BasePopup popup;
        public ScriptableObject data;
        public bool hasOption;
        public Action<ItemDataSO> callback;

        public UIPopupEvent Initializer(BasePopup popup, ScriptableObject data, bool hasOption = false,
            Action<ItemDataSO> callback = null)
        {
            this.popup = popup;
            this.data = data;
            this.hasOption = hasOption;
            this.callback = callback;
            return this;
        }
    }

    public class TogglePopupEnable : GameEvent
    {
        public bool isActive;

        public TogglePopupEnable Initializer(bool isActive)
        {
            this.isActive = isActive;
            return this;
        }
    }
    
    public class PlayUIEffectEvent : GameEvent
    {
        public Sprite sprite;
        public RectTransform start;
        public RectTransform end;
        public float duration;
        public Action callback;

        public PlayUIEffectEvent Initializer(Sprite sprite, RectTransform start, 
            RectTransform end, float duration, Action callback = null)
        {
            this.sprite = sprite;
            this.start = start;
            this.end = end;
            this.duration = duration;
            this.callback = callback;
            return this;
        }
    }
    
    public class PlayUIEffectsEvent : GameEvent
    {
        public Sprite sprite;
        public RectTransform start;
        public RectTransform end;
        public float duration;
        public Action callback;
        public int count;
        public float radius;

        public PlayUIEffectsEvent Initializer(Sprite uiEffect, RectTransform start, 
            float radius, RectTransform end, int count, float duration, Action callback = null)
        {
            this.sprite = uiEffect;
            this.start = start;
            this.radius = radius;
            this.end = end;
            this.count = count;
            this.duration = duration;
            this.callback = callback;
            return this;
        }
    }

    public class ShowBossPopup : GameEvent
    {
        public EnemySO data;

        public ShowBossPopup Initializer(EnemySO data)
        {
            this.data = data;
            return this;
        }
    }
}