using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scripts.Combat.Damage
{
    public class DamageText : MonoBehaviour, IPoolable
    {
        [SerializeField] private PoolItemSO poolItem;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private float yDelta;
        [SerializeField] private float duration;
        public PoolItemSO PoolItem => poolItem;

        public GameObject GameObject => gameObject;
        private Pool _myPool;
        public void ResetItem()
        {
        }
        public void ShowText(Vector3 position, string message,Color color)
        {
            text.color = color;
            text.SetText(message);
            transform.position = position;
            text.DOFade(0, duration).SetEase(Ease.InExpo);
            transform.DOMoveY(position.y+yDelta, duration)
                .SetEase(Ease.InExpo)
                .OnComplete(() => _myPool.Push(this));
        }
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }
    }
}
