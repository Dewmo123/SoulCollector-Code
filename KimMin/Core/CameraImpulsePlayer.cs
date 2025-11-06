using System;
using Unity.Cinemachine;
using UnityEngine;
using Work.Common.Core;
using Work.Events;

namespace Work.Core
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class CameraImpulsePlayer : MonoBehaviour
    {
        private CinemachineImpulseSource _impulseSource;
        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
            GameEventBus.AddListener<CameraImpulseEvent>(HandleCameraImpulse);
        }

        private void HandleCameraImpulse(CameraImpulseEvent evt)
        {
            _impulseSource.GenerateImpulse(evt.impulse);
        }

        [ContextMenu("Test Impulse")]
        private void TestImpulse()
        {
            _impulseSource.GenerateImpulse(0.5f);
        }
    }
}