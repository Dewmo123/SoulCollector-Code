using UI.Inventory;
using UnityEngine;
using Work.Core;

namespace Inventory
{
    [CreateAssetMenu(fileName = "InventoryFilter", menuName = "SO/Inventory/Filter", order = 0)]
    public class InventoryFilterSO : ScriptableObject
    {
        public ItemType itemType;
        public InventoryViewType inventoryType;
        public InventoryTierFilter tierFilter;
        public ItemTier tier;
        public bool rollable;
        public bool enchantable;
    }
}