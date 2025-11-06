using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory;
using Scripts.Network;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.Players;
using Scripts.Players.Storages;
using UnityEngine;
using Work.Common.Core;
using Work.Core;
using Work.Item;
using Random = UnityEngine.Random;

namespace Work.UI.Gacha
{
    public class GachaModel
    {
        private readonly InventoryDataList _inventory;
        private readonly List<ItemDataSO>[] _tierList = new List<ItemDataSO>[3];
        private readonly Dictionary<ItemTier, int> _chanceTable = new();
        private readonly PlayerInfoStorage _storage;
        private int _rollCount;
        private ItemType _itemType;

        private readonly GachaEvent _gachaEvent = GachaEventChannel.GachaEvent;

        public GachaModel(ItemType itemType, InventoryDataList inventory, PlayerInfoStorage storage,int init)
        {
            _rollCount = init;
            _chanceTable.Add(ItemTier.Tier1, 1 + _rollCount / 200);
            _chanceTable.Add(ItemTier.Tier2, 19 + _rollCount / 50);
            _chanceTable.Add(ItemTier.Tier3, 80 - _rollCount / 100);
            _itemType = itemType;
            var list = inventory.GetInventoryItem().Where
                (item => item.itemType == itemType && item.rollable)
                .ToList();
            
            for (int i = 0; i < _tierList.Length; i++)
            {   
                _tierList[i] = new List<ItemDataSO>();
            }

            foreach (var item in list)
            {
                _tierList[(int)item.itemTier - 1].Add(item);
            }
            
            _inventory = inventory;
            _storage = storage;
        }

        public async Task<Dictionary<ItemDataSO, int>> Roll(int count)
        {
            bool success = await _storage.GoodsStorage.ChangeGoods(GoodsType.Crystal, -count * 50);
            if (!success)
                return null;
            _rollCount += count;
            string key = $"{_itemType}{_storage.Id}";
            PlayerPrefs.SetInt(key, _rollCount);
            PlayerPrefs.Save();
            Dictionary<ItemDataSO, int> result = new();

            for (int i = 0; i < count; i++)
            {
                int randValue = Random.Range(0, 100);
                float total = 0f;

                foreach (var (tier, chance) in _chanceTable)
                {
                    total += chance;
                    
                    if (randValue <= total)
                    {
                        var list = _tierList[(int)tier - 1];
                        if(list.Count == 0) continue;
                        
                        ItemDataSO item = list[Random.Range(0, list.Count)];
                        
                        if (result.ContainsKey(item))
                            result[item]++;
                        else
                            result.Add(item, 1);
                        
                        GameEventBus.RaiseEvent(_gachaEvent.Initializer(item));
                        
                        break;
                    }
                }
            }
            switch (_itemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Skill:
                    foreach (var item in result)
                        GameEventBus.RaiseEvent(InventoryEventChannel.AddSkillAmountEvent.Init(item.Key as SkillDataSO, item.Value));
                    break;
                case ItemType.Buddy:
                    foreach (var item in result)
                        GameEventBus.RaiseEvent(InventoryEventChannel.AddPartnerAmountEvent.Init(item.Key as BuddySO, item.Value));
                    break;
            }
            _chanceTable[ItemTier.Tier1] = 1 + _rollCount / 200;
            _chanceTable[ItemTier.Tier2] =19 + _rollCount / 50;
            _chanceTable[ItemTier.Tier3] =80 - _rollCount / 100;
            return result;
        }
        private void ChangeRNG()
        {
            
        }
    }
}