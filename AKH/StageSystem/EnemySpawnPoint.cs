using UnityEngine;

namespace Scripts.StageSystem
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField] private float height;
        [SerializeField] private Transform ground;
        private Vector2 _range;

        private void Update()
        {
            Vector2 middlePos = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height / 2));
            float halfWidth = middlePos.x - Camera.main.transform.position.x;
            middlePos.y = ground.position.y;
            middlePos.x += halfWidth;
            transform.position = middlePos;
            _range = new(halfWidth*2, height);
        }
        public Vector2 RandomPointInRange()
        {
            float halfWidth = _range.x / 2;
            float halfHeight = _range.y / 2;
            Vector2 newPos = new()
            {
                x = Random.Range(-halfWidth, halfWidth),
                y = Random.Range(-halfHeight, halfHeight)
            };
            return (Vector2)transform.position + newPos;
        }
        public Vector2 GetMiddlePos()
            => transform.position;
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, _range);
        }
#endif
    }
}
