
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Network;
using UnityEngine;

namespace Scripts.Players.Storages
{
    public class PlayerSkillStorage
    {
        private WebClient _webClient;
        public Dictionary<string, SkillDTO> Skills { get; private set; }
        public string[] SkillEquips { get; private set; }

        public void Initialize(WebClient webClient, Dictionary<string, SkillDTO> skills, string[] skillEquips)
        {
            _webClient = webClient;
            Skills = skills;
            SkillEquips = skillEquips;
        }

        public async Task<bool> AddSkillAmount(string skillName, int amount)
        {
            SkillAmountDTO dto = new()
            {
                SkillName = skillName,
                Amount = amount
            };
            bool success = await _webClient.SendPostRequest("player/skill/add-amount", dto);
            if (success)
            {
                if (!Skills.ContainsKey(skillName))
                {
                    Skills.Add(skillName, new() { SkillName = skillName,Amount = amount });
                }
                else
                {
                    Skills[skillName].Amount += amount;
                }
            }
            return success;
        }

        public async Task<bool> EquipSkill(int idx, string skillName)
        {
            SkillEquipDTO dto = new()
            {
                Idx = idx,
                SkillName = skillName
            };
            bool success = await _webClient.SendPostRequest<SkillEquipDTO>("player/skill/equip", dto);
            if (success)
                SkillEquips[idx] = skillName;
            return success;
        }

        public async Task<bool> LevelUpSkill(string skillName, int level)
        {
            LevelUpSkillDTO dto = new()
            {
                SkillName = skillName,
                Level = level
            };
            bool success = await _webClient.SendPostRequest("player/skill/level-up", dto);
            if (success)
                Skills[skillName].Level += level;
            return success;
        }
        public async Task<bool> SetAmountAndUpgrade(string partnerName, int amount, int upgrade)
        {
            SetAmountAndUpgradeDTO dto = new()
            {
                Amount = amount,
                PartnerName = partnerName,
                Upgrade = upgrade
            };
            bool success = await _webClient.SendPostRequest("player/skill/set-upgrade-and-amount", dto);
            if (success)
            {
                Skills[partnerName].Amount = amount;
                Skills[partnerName].Upgrade = upgrade;
            }
            return success;
        }
    }
}
