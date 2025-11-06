using Inventory;
using Scripts.StatSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Enemies
{
    [CreateAssetMenu(fileName = "Enemies/EnemySO",menuName = "SO/EnemySO",order = 101)]
    public class EnemySO : ItemDataSO
    {
        public AnimatorOverrideController animator;
        public float detectRange;
        public float attackDelay;
        public Vector2 shadowSize;
        [SerializeField] private StatOverride[] statOverrides;
        private Dictionary<string, StatSO> stats;
        private void OnEnable()
        {
            stats = statOverrides.ToDictionary(so => so.StatName, so => so.CreateStat());
        }
        public StatSO GetStat(StatSO stat)
        {
            Debug.Assert(stat != null, "Finding Stat cannot be null");
            return stats.GetValueOrDefault(stat.statName);
        }
        public float SubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler, float defaultValue)
        {
            StatSO target = GetStat(stat);
            if (target == null) return defaultValue;
            target.OnValueChanged += handler;
            return target.Value;
        }
        public void UnSubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler)
        {
            StatSO target = GetStat(stat);
            if (target == null) return;
            target.OnValueChanged -= handler;
        }
    }
}
