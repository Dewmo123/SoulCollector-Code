using Work.Common.Core;

namespace Work.Events
{
    public static class CameraEventChannel
    {
        public static CameraImpulseEvent CameraImpulseEvent = new();
    }

    public class CameraImpulseEvent : GameEvent
    {
        public float impulse = 0.05f;

        public CameraImpulseEvent Initalizer(float impulse = 0.05f)
        {
            this.impulse = impulse;
            return this;
        }
    }
}