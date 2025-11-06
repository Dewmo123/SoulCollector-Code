using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Inventory;
using Scripts.Entities;
using Scripts.Network;
using Scripts.Players.Storages;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Work.Common.Core;
using Work.Item;
using Work.PlayerSkill;

namespace Scripts.PlayerEquipments.PartnerSystem
{
    public class PartnerManager : MonoBehaviour, IEquipManager
    {
        [SerializeField] private GameObject[] partnerPrefabs;
        [SerializeField] private InventoryDataList invenSO;
        private Dictionary<string, Partner> _partners = new();
        private Entity _entity;
        [Inject] private PlayerInfoStorage _storage;
        [Inject] private EnemyStorage _enemyStorage;
        [Inject] private PoolManagerMono _poolManager;

        public IEquipSocket[] Sockets { get; set; }

        public void Initialize(Entity entity)
        {
            Sockets = GetComponentsInChildren<IEquipSocket>();
            _entity = entity;
            for (int i = 0; i < partnerPrefabs.Length; i++)
            {
                var item = Instantiate(partnerPrefabs[i], transform).GetComponent<Partner>();
                item.name = item.PartnerData.itemName;
                item.gameObject.SetActive(false);
                item.Init(_enemyStorage, _poolManager, _entity);
                _partners.Add(item.name, item);
            }
            GameEventBus.AddListener<AddBuddyEvent>(HandleEquipBuddy);
            GameEventBus.AddListener<AddPartnerAmountEvent>(HandleAddPartnerAmountEvent);
            GameEventBus.AddListener<LevelUpPartnerEvent>(HandleLevelupPartner);
        }

        private void Start()
        {
            HandleInfoRecv();
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<LevelUpPartnerEvent>(HandleLevelupPartner);
            GameEventBus.RemoveListener<AddPartnerAmountEvent>(HandleAddPartnerAmountEvent);
            GameEventBus.RemoveListener<AddBuddyEvent>(HandleEquipBuddy);
        }
        private async void HandleInfoRecv()
        {
            foreach (var partnerDTO in _storage.PartnerStorage.Partners)
            {
                Partner partner = _partners[partnerDTO.Key];
                partner.Level = partnerDTO.Value.Level;
                await AddPartnerLogic(partner.ItemData, partnerDTO.Value.Amount);
            }
            for (int i = 0; i < Sockets.Length; i++)
            {
                string partnerName = _storage.PartnerStorage.PartnerEquips[i];
                Sockets[i].ChangeItem(partnerName == null ? null : _partners[partnerName]);
            }
        }
        private async void HandleLevelupPartner(LevelUpPartnerEvent @event)
        {
            EquipableItemSO item = @event.targetItem;
            PartnerDTO dto = _storage.PartnerStorage.Partners[item.itemName];
            int currentLevel = dto.Level;
            if(currentLevel >= item.GetMaxLevel(dto.Upgrade))
            {
                @event.callback?.Invoke(item, false);
                return;
            }
            EnchantInfo enchant = item.enchantInfo.enchantInfos[currentLevel];
            bool success = await _storage.GoodsStorage.ChangeGoods(enchant.needType.goodsType, enchant.needAmount);
            if (success)
            {
                await _storage.PartnerStorage.LevelUpPartner(item.itemName, 1);
                @event.callback?.Invoke(item, true);
            }
            else
            {
                @event.callback?.Invoke(item, false);
            }
        }
        private async void HandleAddPartnerAmountEvent(AddPartnerAmountEvent @event)
        {
            BuddySO partner = @event.partner;
            int amount = @event.amount;
            bool success = await _storage.PartnerStorage.AddPartnerAmount(partner.itemName, amount);
            if (success)
                await AddPartnerLogic(partner, amount);
        }

        private async Task AddPartnerLogic(EquipableItemSO item, int addAmount)
        {
            invenSO.AddItem(item, addAmount);
            PartnerDTO dto = _storage.PartnerStorage.Partners[item.itemName];
            int upgrade = dto.Upgrade;
            int amount = dto.Amount;
            int remain = item.Upgrade(ref upgrade, amount);
            bool success = await _storage.PartnerStorage.SetAmountAndUpgrade(item.itemName, remain, upgrade);
            if (success)
            {
                invenSO.RemoveItem(item, amount - remain);
                Partner partner = _partners[item.itemName];
                if (!partner.IsEnabled)
                {
                    partner.ApplyMultiplier();
                    partner.IsEnabled = true;
                }
            }
        }

        private void HandleEquipBuddy(AddBuddyEvent @event)
        {
            EquipPartner(@event.index, @event.data);
        }



        public async void EquipPartner(int idx, BuddySO buddySO)
        {
            string partnerName = buddySO == null ? null : buddySO.itemName;
            bool success = await _storage.PartnerStorage.EquipPartner(idx, partnerName);
            if (success)
                Sockets[idx].ChangeItem(partnerName == null ? null : _partners[partnerName]);
        }
    }
}
