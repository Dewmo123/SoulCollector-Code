using DewmoLib.Utiles;
using System;
using UnityEngine;
using Work.Common.Core;

namespace Scripts.Environments
{
    public class MatchRatio : MonoBehaviour
    {
        private Vector3 _screenPosition;
        private void Start()
        {
            GameEventBus.AddListener<ResolutionChangedEvent>(HandleResolutionChanged);
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<ResolutionChangedEvent>(HandleResolutionChanged);
        }

        private void HandleResolutionChanged(ResolutionChangedEvent @event)
        {

        }
    }
}
