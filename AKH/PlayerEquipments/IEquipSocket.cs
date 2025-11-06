using Inventory;
using System;
using Work.Item;

namespace Scripts.PlayerEquipments
{
    public interface IEquipSocket
    {
        event Action<EquipableItemSO> OnChange;
        void ChangeItem(IEquipItem itemData);
        void Reload();
    }
}
