namespace DwarfEngine
{
    /// <summary>
    /// Secondary inventories are connected to a main storage inventory.
    /// </summary>
    public interface ISecondaryInventory
    {
        StorageInventory mainInventory { get; set; }
    }
}
