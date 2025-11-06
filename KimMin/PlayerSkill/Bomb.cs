using System;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace Work.PlayerSkill
{
    public class Bomb : MonoBehaviour, IPoolable
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private LayerMask whatIsEnemy;
        [SerializeField] private float speed;
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        private Transform _target;
        public event Action<Vector2> OnExplode;
        
        public void SetUpPool(Pool pool) { }
        
        public void Init(Transform target) => _target = target;

        private void Update()
        {
            if (_target == null) return;
            transform.position = 
                Vector3.MoveTowards(transform.position, _target.position,
                curve.Evaluate(Time.time) * Time.deltaTime * speed);
        }

        public void ResetItem() => _target = null;

        public void SubscribeStatus(Action<Vector2> callback) => OnExplode += callback;
        public void UnsubscribeStatus(Action<Vector2> callback) => OnExplode -= callback;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & whatIsEnemy) != 0)
            {
                OnExplode?.Invoke(transform.position);
                Destroy(gameObject);
            }
        }
    }
}