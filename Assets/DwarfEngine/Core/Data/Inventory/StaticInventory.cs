using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class StaticInventory : Inventory
    {
        public int itemLimit;

        protected override void Init()
        {
            items = new List<ItemObject>(itemLimit);
        }

        public override void OnSaveInitiated()
        {
            InventoryData data = new InventoryData(items);

            // TODO : Save data to storage
        }
    }
}
