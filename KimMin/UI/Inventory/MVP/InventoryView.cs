using System.Collections.Generic;
using System.Linq;
using Inventory;
using UI.Base;
using UnityEngine;

namespace UI.Inventory
{
    
    public enum InventoryViewType
    {
        All,
        SelectMode,
    }

    public enum InventoryTierFilter
    {
        None,
        Equal,
        Greater,
        Less,
    }
    
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        [SerializeField] private Transform root;
        [SerializeField] private SlotUI itemUI;
        
        private List<SlotUI> _uiContainer = new();

        public void UpdateInventoryUI(List<InventoryItem> items)
        {
            while (_uiContainer.Count < items.Count)
            {
                var newSlot = Instantiate(itemUI, root);
                _uiContainer.Add(newSlot);
            }
            
            DisableAll();

            for (int i = 0; i < items.Count; i++)
            {
                _uiContainer[i].EnableFor(items[i]);
            }
        }
        
        protected void DisableAll() => _uiContainer.ForEach(slot => slot.Disable());
        public void UpdateItemUI(InventoryItem item) => itemUI.EnableFor(item);
    }
}