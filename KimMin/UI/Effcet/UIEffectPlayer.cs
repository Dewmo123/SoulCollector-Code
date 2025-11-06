using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;
using Work.Core;
using Random = UnityEngine.Random;

namespace Wor.UI.Effcet
{
    public class UIEffectPlayer : MonoBehaviour
    {
        [SerializeField] private Image imagePrefab;
        [SerializeField] private Transform root;
        private List<Image> _imagePool = new();
        private void Awake()
        {
            GameEventBus.AddListener<PlayUIEffectEvent>(HandlePlayUIEffect);
            GameEventBus.AddListener<PlayUIEffectsEvent>(HandlePlayUIEffects);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<PlayUIEffectEvent>(HandlePlayUIEffect);
            GameEventBus.RemoveListener<PlayUIEffectsEvent>(HandlePlayUIEffects);
        }

        private void HandlePlayUIEffect(PlayUIEffectEvent evt)
        {
            if(evt.sprite == null) return;
            var target = GetImagePool().First();
            
            target.gameObject.SetActive(true);
            target.sprite = evt.sprite;
            target.rectTransform.position = evt.start.position;
            target.transform.localScale = Vector3.one * 1.5f;
            
            target.rectTransform.DOMove(evt.end.position, evt.duration).SetEase(Ease.InBack)
                .OnComplete(() => { evt.callback?.Invoke(); 
                    target.gameObject.SetActive(false); });
        }

        private async void HandlePlayUIEffects(PlayUIEffectsEvent evt)
        {
            if(evt.sprite == null) return;
            var imageList = GetImagePool(evt.count);

            for (int i = 0; i < evt.count; i++)
            {
                var target = imageList[i];
                Vector2 startPos = (Vector2)evt.start.position + Random.insideUnitCircle * evt.radius;
                target.gameObject.SetActive(true);
                target.sprite = evt.sprite;
                target.rectTransform.position = startPos;
                
                target.rectTransform.DOMove(evt.end.position, evt.duration).SetEase(Ease.InBack)
                    .OnComplete(() => { target.gameObject.SetActive(false); });
                await Awaitable.WaitForSecondsAsync(0.1f);
            }
            
            evt.callback?.Invoke();
        }

        private Image[] GetImagePool(int count = 1)
        {
            if (_imagePool.Count < count)
            {
                for (int i = _imagePool.Count; i < count; i++)
                {
                    _imagePool.Add(Instantiate(imagePrefab, root));
                }
            }
            
            for (int i = 0; i < _imagePool.Count; i++)
            {
                _imagePool[i].gameObject.SetActive(false);
            }
                
            return _imagePool.Take(count).ToArray();
        }
    }
}