using Scripts.Combat;
using UnityEngine;

namespace Scripts.Entities
{
    public class EntityHealthBar : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Transform bar;
        private Entity _entity;
        private EntityHealth _entityHealth;
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _entityHealth = entity.GetCompo<EntityHealth>();
        }
        private void Update()
        {
            float value = _entityHealth.CurrentHealth / _entityHealth.MaxHealth;
            if (Mathf.Approximately(value, 1))
                transform.localScale = Vector3.zero;
            else
                transform.localScale = Vector3.one;
            bar.localScale = new Vector3(value, 1, 1);
        }
    }
}
