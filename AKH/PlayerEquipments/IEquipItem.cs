using Inventory;
using Work.Item;

namespace Scripts.PlayerEquipments
{
    public interface IEquipItem
    {
        EquipableItemSO ItemData { get; }
        bool IsEnabled { get; set; }
        void ApplyMultiplier();
        void LevelUp();
    }
}
