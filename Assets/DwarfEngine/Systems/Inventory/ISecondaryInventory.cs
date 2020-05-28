namespace DwarfEngine
{
    /// <summary>
    /// Secondary inventories are connected to a main storage inventory.
    /// </summary>
    public interface ISecondaryInventory
    {
        /// <summary>
        /// The inventory that it is connected to.
        /// </summary>
        StorageInventory mainInventory { get; set; }
    }
}
