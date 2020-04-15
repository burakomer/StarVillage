using UniRx;

namespace DwarfEngine
{
    public interface IActiveEquipmentSlot
    {
        /// <summary>
        /// Call the equipments activation method here.
        /// </summary>
        void Use(IActiveEquipment equipment);
    }
}
