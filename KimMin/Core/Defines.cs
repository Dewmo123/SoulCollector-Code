using System.Collections.Generic;
using Scripts.Network;
using Scripts.StatSystem;
using UnityEngine;

namespace Work.Core
{
    public enum ItemType
    {
        None = 0,
        Skill,
        Buddy,
        Goods,
        Enemy
    }
    
    public enum ItemTier
    {
        None = 0,
        Tier1,
        Tier2,
        Tier3,
    }
    
    public class Defines : MonoBehaviour
    {
        public static DefineSO DefineSO;
        public static Dictionary<StatType, StatSO> StatDict = new();
        
        [SerializeField] private DefineSO defineSO;
        [SerializeField] private StatListSO statList;

        private void Awake()
        {
            foreach (var stat in statList.statList)
            {
                StatDict.Add(stat.statType, stat);
            }
            
            DefineSO = defineSO;
        }

        public static StatSO GetStatFromType(StatType statType) => StatDict[statType];
    }
}