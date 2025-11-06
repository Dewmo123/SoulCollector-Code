using System;

namespace Inventory
{
    [Serializable]
    public class InventoryItem
    {
        public ItemDataSO data;
        public int stackSize;
        
        public delegate void ValueChangeHandler(InventoryItem item, int prev, int next); 
        public event ValueChangeHandler OnValueChange;
        
        
        public InventoryItem(ItemDataSO newItemData, int count = 1)
        {
            data = newItemData;
            stackSize = count;
        }

        public InventoryItem(InventoryItem item)
        {
            data = item.data;
            stackSize = item.stackSize;
        }
        
        public void AddStack(int count)
        {
            int prevStack = stackSize;
            stackSize += count;
            
            OnValueChange?.Invoke(this, prevStack, stackSize);
        }

        public void RemoveStack(int count = 1)
        {
            int prevStack = stackSize;
            stackSize -= count;
            OnValueChange?.Invoke(this, prevStack, stackSize);
        }
    }
}