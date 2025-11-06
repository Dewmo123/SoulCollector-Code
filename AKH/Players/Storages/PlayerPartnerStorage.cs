
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Network;

namespace Scripts.Players.Storages
{
    public class PlayerPartnerStorage
    {
        private WebClient _webClient;
        public Dictionary<string, PartnerDTO> Partners { get; private set; }
        public string[] PartnerEquips { get; private set; }

        public void Initialize(WebClient webClient, Dictionary<string, PartnerDTO> partners, string[] partnerEquips)
        {
            _webClient = webClient;
            Partners = partners;
            PartnerEquips = partnerEquips;
        }

        public async Task<bool> EquipPartner(int idx, string partnerName)
        {
            PartnerEquipDTO dto = new()
            {
                Idx = idx,
                PartnerName = partnerName
            };
            bool success = await _webClient.SendPostRequest("player/partner/equip", dto);
            if (success)
                PartnerEquips[idx] = partnerName;
            return success;
        }

        public async Task<bool> AddPartnerAmount(string partnerName, int amount)
        {
            PartnerAmountDTO dto = new()
            {
                PartnerName = partnerName,
                Amount = amount
            };
            bool success = await _webClient.SendPostRequest("player/partner/add-amount", dto);
            if (success)
            {
                if (!Partners.ContainsKey(partnerName))
                {
                    Partners.Add(partnerName, new() { PartnerName = partnerName, Amount = amount });
                }
                else
                {
                    Partners[partnerName].Amount += amount;
                }
            }
            return success;
        }

        public async Task<bool> LevelUpPartner(string partnerName, int level)
        {
            LevelUpPartnerDTO dto = new()
            {
                PartnerName = partnerName,
                Level = level
            };
            bool success = await _webClient.SendPostRequest("player/partner/level-up", dto);
            if (success)
                Partners[partnerName].Level += level;
            return success;
        }
        public async Task<bool> SetAmountAndUpgrade(string partnerName,int amount, int upgrade)
        {
            SetAmountAndUpgradeDTO dto = new()
            {
                Amount = amount,
                PartnerName = partnerName,
                Upgrade = upgrade
            };
            bool success = await _webClient.SendPostRequest("player/partner/set-upgrade-and-amount",dto);
            if (success)
            {
                Partners[partnerName].Amount = amount;
                Partners[partnerName].Upgrade= upgrade;
            }
            return success;
        } 
    }
}
