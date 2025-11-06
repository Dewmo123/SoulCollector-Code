using Scripts.Enemies;
using UnityEngine;

namespace Scripts.Entities
{
    public class EntityMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Rigidbody2D rigid;
        [SerializeField] private float speed;
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        public void SetMoveDirection(Vector2 direction)
        {
            rigid.linearVelocity = direction.normalized * speed;
        }
        public void StopImmediately()
        {
            rigid.linearVelocity = Vector2.zero;
        }
    }
}
