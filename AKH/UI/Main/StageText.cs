using Scripts.StageSystem;
using System;
using TMPro;
using UnityEngine;
using Work.Common.Core;

namespace Scripts.UI.Main
{
    public class StageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private void Awake()
        {
            GameEventBus.AddListener<SetStageTextEvent>(HandleEvent);
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<SetStageTextEvent>(HandleEvent);
        }

        private void HandleEvent(SetStageTextEvent @event)
        {
            text.SetText(@event.text);
        }
    }
}
