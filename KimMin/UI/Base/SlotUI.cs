using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public class SlotUI : BaseUI, IUIElement<InventoryItem>
    {
        [SerializeField] protected TextMeshProUGUI countText;
        [SerializeField] protected Image icon;
        [SerializeField] protected Button button;
        public virtual void EnableFor(InventoryItem item)
        {
            if (countText != null)
                countText.text = item.stackSize.ToString();
            
            icon.sprite = item.data.itemIcon;
        }

        protected virtual void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        public virtual void Disable() { }
    }
}