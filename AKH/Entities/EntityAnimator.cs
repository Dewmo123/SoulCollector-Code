using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Entities
{
    public class EntityAnimator : MonoBehaviour, IEntityComponent
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        private Entity _entity;
        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void SetParam(string name, bool value) => Animator.SetBool(name, value);
        public void SetParam(int hash, float value, float dampTime) => Animator.SetFloat(hash, value, dampTime, Time.deltaTime);
        public void SetParam(int hash, float value) => Animator.SetFloat(hash, value);
        public void SetParam(int hash, int value) => Animator.SetInteger(hash, value);
        public void SetParam(int hash, bool value) => Animator.SetBool(hash, value);
        public void SetParam(int hash) => Animator.SetTrigger(hash);
        public void SetLayerWeight(int layerIndex, float weight) => Animator.SetLayerWeight(layerIndex, weight);
        public void OffAnimator()
        {
            Animator.enabled = false;
        }
    }
}