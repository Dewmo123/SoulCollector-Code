    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Inventory;
    using UnityEngine;
    using Work.Core;

    namespace UI.Inventory
    {
        public class InventoryModel : IInventoryModel
        {
            private readonly InventoryDataList _dataList;
            private InventoryFilterSO _filter;
            private List<InventoryItem> _cachedItems = new();

            public List<InventoryItem> Items => _cachedItems;
            public event Action<InventoryItem> OnItemChanged;
            public event Action OnInventoryChanged;

            public InventoryModel(InventoryDataList items, InventoryFilterSO filter)
            {
                _dataList = items;
                _filter = filter;
                _dataList.OnInventoryUpdated += OnDataUpdated;
                _dataList.OnItemAdded += OnItemAdded;
                
                UpdateCache();
            }
            
            public void SetCondition(InventoryFilterSO newCondition)
            {
                _filter = newCondition;
                UpdateCache();
                OnInventoryChanged?.Invoke();
            }

            private void UpdateCache()
            {
                var list = _dataList.GetInventory();
                _cachedItems = list.Where(CheckCondition).ToList();
            }
            
            private void OnDataUpdated()
            {
                UpdateCache();
                OnInventoryChanged?.Invoke();
            }

            private void OnItemAdded(InventoryItem item)
            {
                Debug.Assert(_filter != null, $"filter is not assigned!");

                if (CheckCondition(item))
                    _cachedItems.Add(item);

                OnItemChanged?.Invoke(item);
            }
            
            private bool CheckCondition(InventoryItem item)
            {
                if (_filter.inventoryType == InventoryViewType.SelectMode &&
                    item.data.itemType != _filter.itemType) return false;
                
                if (_filter.rollable && !item.data.rollable) return false;
                if (_filter.enchantable && !item.data.enchantable) return false;
                
                if (_filter.tierFilter == InventoryTierFilter.Equal
                    && _filter.tier != item.data.itemTier) return false;
                if (_filter.tierFilter == InventoryTierFilter.Greater
                    && _filter.tier > item.data.itemTier) return false;
                if (_filter.tierFilter == InventoryTierFilter.Less
                    && _filter.tier < item.data.itemTier) return false;

                return true;
            }

            public void AddItem(ItemDataSO item, int count = 1)
            {
                _dataList.AddItem(item, count);
            }

            public void RemoveItem(ItemDataSO item, int count = 1)
            {
                _dataList.RemoveItem(item, count);
            }
        }
    }