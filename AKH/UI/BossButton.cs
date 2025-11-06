using Scripts.StageSystem;
using System;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;

namespace Scripts.UI
{
    public class BossButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        private void Awake()
        {
            GameEventBus.AddListener<BossKeyEnableEvent>(HandleButtonEnable);
            button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<BossKeyEnableEvent>(HandleButtonEnable);
            button.onClick.RemoveListener(HandleButtonClick);
        }
        private void HandleButtonClick()
        {
            GameEventBus.RaiseEvent(StageEvents.EnterBossEvent);
            button.gameObject.SetActive(false);
        }

        private void HandleButtonEnable(BossKeyEnableEvent @event)
        {
            button.gameObject.SetActive(true);
        }
    }
}
