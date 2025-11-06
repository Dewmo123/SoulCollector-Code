using Scripts.Entities;

namespace Scripts.PlayerEquipments
{
    public interface IEquipManager : IEntityComponent
    {
        IEquipSocket[] Sockets { get; set; }
    }
}