using System;
using System.Collections.Generic;
using Inventory;
using Work.Upgrade;

namespace UI.Inventory
{
    public interface IInventoryView
    {
        void UpdateInventoryUI(List<InventoryItem> items);
        void UpdateItemUI(InventoryItem items);
    }
}