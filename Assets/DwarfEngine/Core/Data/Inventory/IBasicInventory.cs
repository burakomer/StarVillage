using System.Collections.Generic;

namespace DwarfEngine
{
    public interface IBasicInventory
    {
        List<Inventory> connectedInventories { get; }

        void Connect(Inventory secondaryInventory);
    }
}
