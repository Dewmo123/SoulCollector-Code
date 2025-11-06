using System;
using System.Collections.Generic;
using Inventory;

namespace UI.Inventory
{
    public interface IInventoryModel
    {
        List<InventoryItem> Items { get; }
        event Action<InventoryItem> OnItemChanged;
        event Action OnInventoryChanged;
    }
}