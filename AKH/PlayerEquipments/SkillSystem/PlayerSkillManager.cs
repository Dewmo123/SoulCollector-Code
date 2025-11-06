using DewmoLib.Dependencies;
using Inventory;
using Scripts.Entities;
using Scripts.Network;
using Scripts.PlayerEquipments.PartnerSystem;
using Scripts.Players;
using Scripts.Players.Storages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Work.Common.Core;
using Work.Item;

namespace Scripts.PlayerEquipments.SkillSystem
{
    public struct SkillQueueInfo
    {
        public Skill skill;
        public int slotIndex;
    }
    public class PlayerSkillManager : MonoBehaviour, IEquipManager
    {
        [SerializeField] private InventoryDataList invenSO;
        public SkillSocket[] SkillSockets => Sockets as SkillSocket[];

        public Queue<SkillQueueInfo> SkillQueue { get; private set; } = new();
        public IEquipSocket[] Sockets { get; set; } = new SkillSocket[6];
        public bool IsAuto { get; private set; } = true;

        public HashSet<int> RegisteredSkill { get; set; } = new();
        private Dictionary<string, Skill> _skills;
        private Player _player;
        [Inject] PlayerInfoStorage _storage;
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _skills = new();
            Skill[] skills = GetComponentsInChildren<Skill>();
            foreach (var item in skills)
            {
                item.InitSkill(_player);
                _skills.Add(item.ItemData.itemName, item);
            }
            for (int i = 0; i < SkillSockets.Length; i++)
                SkillSockets[i] = new();
            GameEventBus.AddListener<EquipSkillEvent>(HandleEquipEvent);
            GameEventBus.AddListener<AddSkillAmountEvent>(HandleAddSkillAmountEvent);
            GameEventBus.AddListener<LevelUpSkillEvent>(HandleLevelupSkill);
        }

        private void Start()
        {
            HandleInfoRecv();
        }
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<EquipSkillEvent>(HandleEquipEvent);
            GameEventBus.RemoveListener<AddSkillAmountEvent>(HandleAddSkillAmountEvent);
            GameEventBus.RemoveListener<LevelUpSkillEvent>(HandleLevelupSkill);
        }
        private async void HandleInfoRecv()
        {
            foreach (var item in _storage.SkillStorage.Skills)
            {
                var skill = _skills[item.Key];
                skill.Level = item.Value.Level;
                await AddSkillLogic(skill.ItemData, item.Value.Amount);
            }
            for (int i = 0; i < Sockets.Length; i++)
            {
                string skillName = _storage.SkillStorage.SkillEquips[i];
                SkillSockets[i].ChangeItem(skillName == null ? null : _skills[skillName]);
            }
        }
        private async void HandleLevelupSkill(LevelUpSkillEvent @event)
        {
            EquipableItemSO item = @event.targetItem;
            Skill target = _skills[item.itemName];
            int currentLevel = _storage.SkillStorage.Skills[item.itemName].Level;
            EnchantInfo enchant = item.enchantInfo.enchantInfos[currentLevel];
            bool success = await _storage.GoodsStorage.ChangeGoods(enchant.needType.goodsType, -enchant.needAmount);
            if (success)
            {
                await _storage.SkillStorage.LevelUpSkill(item.itemName, 1);
                target.LevelUp();
                @event.callback?.Invoke(item, true);
            }
            else
            {
                @event.callback?.Invoke(item, false);
            }
        }
        private async void HandleAddSkillAmountEvent(AddSkillAmountEvent @event)
        {
            EquipableItemSO item = @event.skill;
            int amount = @event.amount;
            bool success = await _storage.SkillStorage.AddSkillAmount(item.itemName, amount);
            if (success)
            {
                await AddSkillLogic(item, amount);
            }
        }
        private async Task AddSkillLogic(EquipableItemSO item, int addAmount)
        {
            invenSO.AddItem(item, addAmount);
            SkillDTO dto = _storage.SkillStorage.Skills[item.itemName];
            int upgrade = dto.Upgrade;
            int amount = dto.Amount;
            int remain = item.Upgrade(ref upgrade, amount);
            bool success = await _storage.SkillStorage.SetAmountAndUpgrade(item.itemName, remain, upgrade);
            if (success)
            {
                invenSO.RemoveItem(item, amount - remain);
                Skill skill = _skills[item.itemName];
                if (!skill.IsEnabled)
                {
                    skill.ApplyMultiplier();
                    skill.IsEnabled = true;
                }
            }
        }
        private void HandleEquipEvent(EquipSkillEvent @event)
        {
            EquipSkill(@event.index, @event.data);
        }



        private void Update()
        {
            for (int idx = 0; idx < SkillSockets.Length; idx++)
            {
                SkillSocket item = SkillSockets[idx];
                item?.UpdateSocket();
            }
            AutoRegisterSkill();
        }
        private void AutoRegisterSkill()
        {
            if (!IsAuto)
                return;
            for (int idx = 0; idx < SkillSockets.Length; idx++)
            {
                RegisterSkill(idx);
            }
        }
        public async void EquipSkill(int idx, SkillDataSO skillData)
        {
            string skillName = skillData == null ? null : skillData.itemName;
            bool success = await _storage.SkillStorage.EquipSkill(idx, skillName);
            if (success)
                SkillSockets[idx].ChangeItem(skillName == null ? null : _skills[skillName]);
        }
        public void RegisterSkill(int idx)
        {
            if (SkillSockets[idx].CanUseSkill() && !_player.IsDead && !RegisteredSkill.Contains(idx))
            {
                RegisteredSkill.Add(idx);
                SkillQueueInfo info = new()
                {
                    skill = SkillSockets[idx].CurrentSkill,
                    slotIndex = idx
                };
                SkillQueue.Enqueue(info);
            }
        }
        public void SetCooldown(int idx) => SkillSockets[idx].SetCooldown();
    }
}
