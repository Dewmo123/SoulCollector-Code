using DG.Tweening;
using Inventory;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Work.UI.Gacha
{
    public class GachaItemUI : MonoBehaviour, IUIElement<ItemDataSO, int>
    {
        [SerializeField] private GameObject holder;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Image effectImage;
        [SerializeField] private Image effectImage2;
        
        private readonly Color32[] _tierColor =
        {
            new(255, 175, 175, 255),
            new(175, 255, 155, 255),
            new(255, 255, 255, 255),
        };
        
        public void EnableFor(ItemDataSO item, int count)
        {
            ResetElements();
            var color = _tierColor[(int)item.itemTier - 1];
            
            effectImage.color = effectImage2.color = color;
            effectImage2.DOFade(0f, 0.25f).OnComplete(() => 
                { effectImage2.gameObject.SetActive(false); });
            
            effectImage.transform.DOScale(1.85f, 0.75f).SetEase(Ease.OutQuart);
            effectImage.DOFade(0f, 0.75f).SetEase(Ease.OutQuart).OnComplete(() => 
                { effectImage.gameObject.SetActive(false); });
            
            background.color = color;
            icon.sprite = item.itemIcon;
            countText.text = count.ToString();
        }

        private void ResetElements()
        {
            holder.SetActive(true);
            effectImage.gameObject.SetActive(true);
            effectImage2.gameObject.SetActive(true);
            effectImage.transform.localScale = effectImage.transform.localScale = Vector2.one;
        }

        public void Disable()
        {
            holder.transform.parent.gameObject.SetActive(false);
            holder.SetActive(false);
        }
    }
}