namespace DwarfEngine
{
    /// <summary>
    /// Special inventories require special logic to work with.
    /// </summary>
    public interface ISpecialInventory
    {
        StorageInventory mainInventory { get; set; }
    }
}
