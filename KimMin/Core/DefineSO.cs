using UnityEngine;

namespace Work.Core
{
    [CreateAssetMenu(fileName = "DefineSO", menuName = "SO/Define", order = 0)]
    public class DefineSO : ScriptableObject
    {
        [Header("Enchant")]
        public int requireEnchantLevel;
        public float expIncreaseRatio;
    }
}