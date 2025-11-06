using UnityEngine;

namespace Work.Sound
{
    [CreateAssetMenu(fileName = "Sound clip", menuName = "SO/sound", order = 0)]
    public class SoundSO : ScriptableObject
    {
        public enum AudioTypes
        {
            SFX,
            MUSIC
        };

        public AudioTypes audioType;
        public AudioClip clip;
        public bool loop = false;
        public bool randomizePitch;

        [Range(0, 1f)]
        public float randomPitchModifier = 0.1f;
        [Range(0.1f, 2f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
    }
}