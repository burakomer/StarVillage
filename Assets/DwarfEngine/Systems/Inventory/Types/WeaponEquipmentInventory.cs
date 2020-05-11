namespace DwarfEngine
{
    public class WeaponEquipmentInventory : EquipmentInventory<WeaponItemAsset>
    {
        protected override void PreInit()
        {
            size = 1;
        }
    }
}
