using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Work.Common.Core;

namespace Work.KimMin._01_Code.UI.MainMenu
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private float ySize;
        [SerializeField] private RectTransform targetRect;

        //private bool _isOpened = false;
        private readonly MenuButtonEvent _menuButtonEvent = MenuButtonEventChannel.MenuButtonEvent;

        private void Awake()
        {
            button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            GameEventBus.RaiseEvent(_menuButtonEvent.Initializer(this));
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
        public void SetEnable(bool enabled) => button.interactable = enabled;
        public void SetStatus(bool isActive, bool hasTween = true)
        {
            float duration = hasTween ? 0.2f : 0f;

            if (isActive)
            {
                targetRect.gameObject.SetActive(true);
                targetRect.anchoredPosition = new Vector2(0, -ySize);
                targetRect.DOKill();
                targetRect.DOAnchorPosY(0, duration).SetEase(Ease.OutCubic);
            }
            else
            {
                targetRect.DOAnchorPosY(-ySize, duration).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    targetRect.gameObject.SetActive(false);
                });
            }
        }
    }
}