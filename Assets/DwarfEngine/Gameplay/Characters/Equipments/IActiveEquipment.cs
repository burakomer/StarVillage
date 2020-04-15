using UniRx;

namespace DwarfEngine
{
    public interface IActiveEquipment
    {
        bool StartEquipment();
        void StopEquipment();
    }
}
