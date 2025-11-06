using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        [SerializeField] private StatSO stat;
        [SerializeField] private bool isUseOverride;
        [SerializeField] private float overrideValue;
        public string StatName => stat.statName;
        public StatOverride(StatSO stat) => this.stat = stat;

        public StatSO CreateStat()
        {
            StatSO newStat = stat.Clone() as StatSO;
            Debug.Assert(newStat != null, $"{nameof(newStat)} stat cloning failed");

            if (isUseOverride)
            {
                newStat.BaseValue = overrideValue;
            }
            return newStat;
        }
    }
}
