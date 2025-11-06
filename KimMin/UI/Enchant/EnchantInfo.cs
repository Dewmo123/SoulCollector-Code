using Inventory;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Work.UI.Enchant
{
    public class EnchantInfo : MonoBehaviour, IUIElement<ItemDataSO, int>
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI levelText;
        
        public void EnableFor(ItemDataSO item, int level)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = item.itemIcon;
            levelText.text = $"Level.{level}";
        }

        public void Disable()
        {
            icon.gameObject.SetActive(false);
            levelText.text = string.Empty;
        }
    }
}