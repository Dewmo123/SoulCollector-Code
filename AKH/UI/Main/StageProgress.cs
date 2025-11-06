using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;

namespace Scripts.UI.Main
{
    public class StageProgress : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image bar;
        private void Awake()
        {
            GameEventBus.AddListener<SetStageProgressEvent>(HandleEvent);
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<SetStageProgressEvent>(HandleEvent);
        }
        private void HandleEvent(SetStageProgressEvent @event)
        {
            bar.fillAmount = @event.current / @event.total;
            text.SetText(@event.text);
        }
    }
}
