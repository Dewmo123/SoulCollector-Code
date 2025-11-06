using Scripts.Combat;
using Scripts.Enemies;
using UnityEngine;

namespace AKH.Scripts.Enemies
{
    public class EnemyHealth : EntityHealth, IChangeableCompo<EnemySO>
    {
        public void Change(EnemySO before, EnemySO current)
        {
            before?.UnSubscribeStat(hpStat,HandleMaxHpChange);
            maxHealth = current.SubscribeStat(hpStat,HandleMaxHpChange,10);
            currentHealth = maxHealth;
        }
    }
}
