using System;
using DG.Tweening;
using Scripts.Enemies;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;
using Work.Core;

namespace Work.UI.Popup
{
    public class BossPopup : MonoBehaviour
    {
        [SerializeField] private Image bossIcon;
        [SerializeField] private Image mask;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private GameObject content;

        private readonly Vector2 startSize = new(500f, 500f);
        private readonly Vector2 endSize  = new(0f, 500f);

        private void Awake()
        {
            GameEventBus.AddListener<ShowBossPopup>(HandleShowBossPopup);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<ShowBossPopup>(HandleShowBossPopup);
        }

        private void HandleShowBossPopup(ShowBossPopup evt) => ShowPopup(evt.data);

        private void ShowPopup(EnemySO data)
        {
            if (data == null) return;
            content.gameObject.SetActive(true);
            
            bossIcon.sprite = data.itemIcon;
            title.text = data.name;
            mask.rectTransform.sizeDelta = endSize;
            mask.rectTransform.DOSizeDelta(startSize, 1f).OnComplete(() => {
                mask.rectTransform.DOSizeDelta(endSize, 0.8f).OnComplete(() =>
                {
                    content.gameObject.SetActive(false);
                });
            });
        }   
    }
}