using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Feedbacks
{
    public class BlinkFeedback : Feedback
    {
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private float delaySeconds;
        [SerializeField] private float blinkValue;

        private readonly int _blinkShaderParam = Shader.PropertyToID("_BlinkValue");
        private Material _material;
        private bool _isFinished;

        private void Awake()
        {
            _material = targetRenderer.material;
        }


        public override async void CreateFeedback()
        {
            _material.SetFloat(_blinkShaderParam, blinkValue);
            _isFinished = false;
            await UniTask.WaitForSeconds(delaySeconds);
            if (_isFinished == false)
                FinishFeedback();
        }

        public override void FinishFeedback()
        {


            _isFinished = true;
            _material.SetFloat(_blinkShaderParam, 0);
        }
    }
}
