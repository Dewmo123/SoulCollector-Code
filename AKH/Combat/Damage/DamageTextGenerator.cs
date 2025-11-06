using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Environments;
using System;
using UnityEngine;
using Work.Common.Core;

namespace Scripts.Combat.Damage
{
    public class DamageTextGenerator : MonoBehaviour
    {
        [SerializeField] private PoolItemSO damageTextItem;
        [Inject] private PoolManagerMono _poolManager;
        
        private readonly Color32 criticalColor = new Color32(255, 100, 100, 255);
        private void Awake()
        {
            GameEventBus.AddListener<DamageEvent>(HandleDamage);
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<DamageEvent>(HandleDamage);
        }
        private void HandleDamage(DamageEvent @event)
        {
            var damageText = _poolManager.Pop<DamageText>(damageTextItem);
            string message = NumberFormatter.Format(@event.damage);
            Color color = @event.isCritical ? criticalColor : Color.white;
            damageText.ShowText(@event.hitPosition, message, color);
        }
    }
}
