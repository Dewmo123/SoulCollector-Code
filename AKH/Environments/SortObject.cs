using UnityEngine;

namespace Scripts.Environments
{
    public class SortObject : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        [SerializeField] private int _offset;
        private void Awake()
        {
            _renderer = transform.GetComponent<SpriteRenderer>();
            _renderer.sortingOrder = Mathf.RoundToInt(transform.parent.position.y * -100) + _offset;
        }
        private void Update()
        {
            _renderer.sortingOrder = Mathf.RoundToInt(transform.parent.position.y * -100) + _offset;
        }
    }
}
