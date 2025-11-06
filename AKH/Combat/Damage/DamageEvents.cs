using UnityEngine;
using Work.Common.Core;

namespace Scripts.Combat.Damage
{
    public static class DamageEvents
    {
        public readonly static DamageEvent DamageEvent = new();
    }
    public class DamageEvent : GameEvent
    {
        public Vector3 hitPosition;
        public float damage;
        public bool isCritical;
        public DamageEvent Init(Vector3 hitPosition,float damage,bool isCritical)
        {
            this.hitPosition = hitPosition;
            this.damage = damage;
            this.isCritical = isCritical;
            return this;
        }
    }
}
