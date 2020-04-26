using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class StaticInventory : Inventory
    {
        public int itemLimit;

        protected override void Init()
        {
            items = new List<ItemData>(itemLimit);
        }
    }
}
