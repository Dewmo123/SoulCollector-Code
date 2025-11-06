using Scripts.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public bool IsDead { get; set; }
        public UnityEvent OnHitEvent;
        public UnityEvent<Entity> OnDeadEvent;
        public UnityEvent OnReviveEvent;

        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            AddComponents();
            InitializeComponents();
            AfterInitialize();
        }
        protected virtual void Start()
        {
        }
        protected virtual void AfterInitialize()
        {
            _components.Values.OfType<IAfterInit>()
                .ToList().ForEach(compo => compo.AfterInit());
        }

        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IEntityComponent>().ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }

        protected virtual void InitializeComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }

        public T GetCompo<T>(bool isDerived = true) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
                return (T)component;

            if (isDerived == false) return default(T);

            Type findType = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (findType != null)
                return (T)_components[findType];

            return default(T);
        }
        public IEntityComponent GetCompo(Type type)
            => _components.GetValueOrDefault(type);
        public void DestroyThis()
        {
            Destroy(gameObject);
        }
        public void RotateToTarget(Vector3 targetPosition, bool isSmooth = false)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

            if (isSmooth)
            {
                const float smoothRotationSpeed = 15f;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * smoothRotationSpeed);
            }
            else
            {
                transform.rotation = targetRotation;
            }
        }
        public virtual void SetDead()
        {
            if (IsDead)
                return;
            IsDead = true;
        }
        public virtual void Revive()
        {
            if (!IsDead)
                return;
            IsDead = false;
            OnReviveEvent?.Invoke();
        }
    }
}
