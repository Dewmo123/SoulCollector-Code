using System.Collections.Generic;
using UnityEngine;

namespace Scripts.StatSystem
{
    [CreateAssetMenu(fileName = "StatList", menuName = "SO/Stat/StatList", order = 0)]
    public class StatListSO : ScriptableObject
    {
        public List<StatSO> statList;
    }
}