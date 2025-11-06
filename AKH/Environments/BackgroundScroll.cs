using Scripts.StageSystem;
using UnityEngine;
using Work.Common.Core;

namespace Scripts.Environment
{
    public class BackgroundScroll : MonoBehaviour
    {
        private float _velocity;
        private SpriteRenderer _spriteRenderer;
        private Material _backgroundMaterial;

        private float startXPos;
        private float _currentScroll;
        private float _ratio;
        private float _beforeXPosition;
        private float _currentXPos;
        private readonly int _offsetHash = Shader.PropertyToID("_Offset");

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _backgroundMaterial = _spriteRenderer.material;
            startXPos = transform.position.x;
            _currentScroll = 0;
            _ratio = 1f / _spriteRenderer.bounds.size.x;
            GameEventBus.AddListener<MoveEvent>(HandleMoveEvent);
            GameEventBus.AddListener<StopEvent>(HandleStopEvent);
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<MoveEvent>(HandleMoveEvent);
            GameEventBus.RemoveListener<StopEvent>(HandleStopEvent);
        }
        private void HandleStopEvent(StopEvent @event)
            => _velocity = 0;

        private void HandleMoveEvent(MoveEvent @event)
            => _velocity = @event.velocity;

        private void Start()
        {
            _beforeXPosition = startXPos;
            _currentXPos = startXPos;
        }
        private void Update()
        {
            _currentXPos += (_velocity * Time.deltaTime);
            float delta = _currentXPos - _beforeXPosition;

            _beforeXPosition = _currentXPos;
            _currentScroll += delta * _ratio;
            _backgroundMaterial.SetVector(_offsetHash, new Vector2(_currentScroll, 0));
        }
    }
}