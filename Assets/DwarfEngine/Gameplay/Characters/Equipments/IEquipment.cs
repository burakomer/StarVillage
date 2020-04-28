namespace DwarfEngine
{
    public interface IEquipment
    {
        Character owner { get; set; }

        void SetOwner(Character _owner);
    }
}