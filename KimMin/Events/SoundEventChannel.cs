using Work.Common.Core;
using Work.Sound;

namespace Work.Events
{
    public static class SoundEventChannel
    {
        public static PlaySFXEvent PlaySFXEvent = new PlaySFXEvent();
        public static StopSoundEvent StopSoundEvent = new StopSoundEvent();
    }

    public class PlaySFXEvent : GameEvent
    {
        public SoundSO clip;
        public int channel;

        public PlaySFXEvent Initializer(SoundSO clip, int channel = 0)
        {
            this.clip = clip;
            this.channel = channel;
            return this;
        }
    }

    public class StopSoundEvent : GameEvent
    {
        public int channel;

        public StopSoundEvent Initializer(int channel)
        {
            this.channel = channel;
            return this;
        }
    }
}