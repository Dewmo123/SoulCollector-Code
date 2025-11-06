using UnityEngine;

namespace Scripts.StageSystem
{
    public abstract class BaseStageDataSO : ScriptableObject
    {
        [Header("EnvironmentSetting")]
        public Sprite backgroundImage;
        public Sprite groundImage;
        public string stageName;
        public float velocity;
        // Add other common properties here
    }
}
