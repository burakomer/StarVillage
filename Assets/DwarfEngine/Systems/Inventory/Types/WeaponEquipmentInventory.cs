namespace DwarfEngine
{
    public class WeaponEquipmentInventory : EquipmentInventory<WeaponItemObject>
    {
        protected override void PreInit()
        {
            size = 1;
        }
    }
}
