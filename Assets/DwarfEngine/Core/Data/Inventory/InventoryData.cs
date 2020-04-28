using System.Collections.Generic;

namespace DwarfEngine
{
    /// <summary>
    /// Used to convert inventory data to serializable object.
    /// </summary>
    [System.Serializable]
    public class InventoryData
    {
        public int[] itemIds { get; private set; }

        public InventoryData(List<ItemObject> items)
        {
            if (items.Capacity != 0)
            {
                int[] ids = new int[items.Capacity];

                for (int i = 0; i < ids.Length; i++)
                {
                    if (items[i] == null)
                    {
                        ids[i] = -1; // If null item, assign -1 to mark it empty.
                    }
                    else
                    {
                        ids[i] = items[i].Id;
                    }
                }

                itemIds = ids;
            }
            else
            {
                itemIds = new int[0];
            }
        }
    }
}
