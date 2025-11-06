using Scripts.Feedbacks;
using Unity.Cinemachine;
using UnityEngine;

namespace Work.Feedbacks
{
    public class CameraImpulseFeedback : Feedback
    {
        [SerializeField] private float impulseForce = 0.6f;
        [SerializeField] private CinemachineImpulseSource impulseSource;

        public override void CreateFeedback()
        {
            impulseSource.GenerateImpulse(impulseForce);
        }
    }
}