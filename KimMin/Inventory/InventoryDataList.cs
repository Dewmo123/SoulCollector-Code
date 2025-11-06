using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Work.Core;

namespace Inventory
{
    [Serializable]
    public struct ItemDataList
    {
        public List<ItemData> items;

        public ItemDataList(List<ItemData> items)
        {
            this.items = items;
        }
    }
    [Serializable]
    public struct ItemData
    {
        public int id;
        public int count;

        public ItemData(InventoryItem item)
        {
            id = item.data.ItemID;
            count = item.stackSize;
        }
    }
    
    [CreateAssetMenu(fileName = "Inventory", menuName = "SO/Inventory/Inventory", order = 0)]
    public class InventoryDataList : ScriptableObject
    {
        public event Action OnInventoryUpdated;
        public event Action<InventoryItem> OnItemAdded;
        [SerializeField] private ItemDataSO[] allItems;
        [SerializeField] private List<InventoryItem> _items = new List<InventoryItem>();
        //private readonly string _saveKey = "InventoryData";

        private void OnEnable()
        {
            ResetPlayerPrefs();
        }
        [ContextMenu("ResetPlayerPrefs")]
        public void ResetPlayerPrefs()
        {
            _items = new List<InventoryItem>();
        }
        
        public InventoryItem GetItemFromID(int id)
        {
            foreach (var item in _items)
            {
                if (item.data.ItemID == id)
                {
                    return item;
                }
            } 
            return null;
        }
        
        public int GetItemCount(ItemDataSO itemData)
        {
            var items = GetInventoryItem(itemData);
            if (items == null) return 0;
            return items.stackSize;
        }
        private InventoryItem GetInventoryItem(ItemDataSO item) =>
            _items.Find(i => i.data.ItemID == item.ItemID);
        
        public void AddItem(ItemDataSO itemData, int count = 1, bool notify = true)
        {
            var items = GetInventoryItem(itemData);
            if (items == null)
            {
                CreateNewInventoryItem(itemData, count);
            }
            else
            {
               items.AddStack(count);
            }
            
            _items = _items.OrderBy(item => item.data.itemName).ToList();
            if(notify)
                OnInventoryUpdated?.Invoke();
        }

        public void RemoveItem(ItemDataSO itemData, int count = 1)
        {
            var items = GetInventoryItem(itemData);
            if (items == null) return;

            items.RemoveStack(count);
            if (items.stackSize <= 0)
                _items.Remove(items);
            _items = _items.OrderBy(item => item.data.itemName).ToList();
            OnInventoryUpdated?.Invoke();
        }

        public List<InventoryItem> GetItemFrom(ItemType itemType, ItemTier itemTier = ItemTier.None)
        {
            List<InventoryItem> list = _items.Where(item => item.data.itemType == itemType).ToList();
            if (itemTier != ItemTier.None)
            {
                list = list.Where(item => item.data.itemTier == itemTier).ToList();
                return list;
            }
            return list;
        }

        private void CreateNewInventoryItem(ItemDataSO itemData, int count)
        {
            InventoryItem newItem = new InventoryItem(itemData, count);
            _items.Add(newItem);
            OnItemAdded?.Invoke(newItem);
        }

        public List<ItemDataSO> GetInventoryItem() => allItems.ToList();
        public List<InventoryItem> GetInventory() => _items;
    }
}