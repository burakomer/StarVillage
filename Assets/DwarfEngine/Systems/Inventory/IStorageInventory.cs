using System.Collections.Generic;

namespace DwarfEngine
{
    public interface IStorageInventory
    {
        /// <summary>
        /// List of the connected inventories. It is for transferring items between them.
        /// </summary>
        List<Inventory> connectedInventories { get; }

        /// <summary>
        /// Connection logic.
        /// </summary>
        /// <param name="secondaryInventory"></param>
        void Connect(Inventory secondaryInventory);
    }
}
