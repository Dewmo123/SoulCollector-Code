using AYellowpaper.SerializedCollections;
using Inventory;
using Scripts.PlayerEquipments;
using Scripts.StatSystem;
using System.Collections.Generic;

namespace Work.Item
{
    public class EquipableItemSO : ItemDataSO
    {
        public SerializedDictionary<StatSO, float> ownStatDict = new();
        public EnchantSO enchantInfo;
        public UpgradeSO upgradeInfo;
        public bool isEquipped = false;
        public List<UpgradeInfo> upgradeInfos => upgradeInfo.upgradeInfos;
        public int GetMaxLevel(int currentUpgrade)
        {
            if (upgradeInfos.Count > currentUpgrade)
                return upgradeInfos[currentUpgrade].maxLevel;
            else
                return 0;
        }
        public int Upgrade(ref int upgrade, int amount)
        {
            while (upgradeInfos.Count > upgrade && upgradeInfos[upgrade].needAmount <= amount)
            {
                amount -= upgradeInfos[upgrade].needAmount;
                upgrade++;
            }
            return amount;
        }
        private void OnEnable()
        {
            isEquipped = false;
        }
    }
}