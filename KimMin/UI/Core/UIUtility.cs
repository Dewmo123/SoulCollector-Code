using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;
using Work.Core;

namespace UI.Core
{
    public static class UIUtility
    {
        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if(component == null)
                component = go.AddComponent<T>();
            
            return component;
        }

        public static void FadeUI(GameObject go, float duration, bool isFadeOut, 
            Action completeCallback = null)
        {
            CanvasGroup canvas = go.GetOrAddComponent<CanvasGroup>();
            float targetA = isFadeOut ? 0 : 1;

            canvas.DOFade(targetA, duration)
                .OnComplete(() => {
                    canvas.interactable = !isFadeOut;
                    canvas.blocksRaycasts = !isFadeOut;
                    completeCallback?.Invoke();
                }).SetUpdate(true);
        }
        
        public static void ShowUI(GameObject go, bool isShow = true)
        {
            CanvasGroup canvas = go.GetOrAddComponent<CanvasGroup>();
            canvas.alpha = isShow ? 1f : 0f;
            canvas.interactable = isShow;
            canvas.blocksRaycasts = isShow;
        }

        public static void PlayUIEffect(Sprite sprite, RectTransform start, RectTransform end, 
            float duration = 0.2f, Action completeCallback = null)
        {
            GameEventBus.RaiseEvent(UIEventChannel.PlayUIEffectEvent
                .Initializer(sprite, start, end, duration, completeCallback));
        }
        
        public static void PlayUIEffects(Sprite sprite, RectTransform start, float radius, 
            RectTransform end, int count, float duration = 0.2f, Action completeCallback = null)
        {
            GameEventBus.RaiseEvent(UIEventChannel.PlayUIEffectsEvent
                .Initializer(sprite, start, radius, end, count, duration, completeCallback));
        }
    }
}