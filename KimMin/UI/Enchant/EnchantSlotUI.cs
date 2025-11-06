using Inventory;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Work.UI.Enchant
{
    public class EnchantSlotUI : MonoBehaviour, IUIElement<ItemDataSO>
    {
        [SerializeField] private Image icon;
        [SerializeField] private Sprite defaultSprite;
        
        public void EnableFor(ItemDataSO item)
        {
            icon.sprite = item.itemIcon;
        }

        public void Disable()
        {
        }
    }
}