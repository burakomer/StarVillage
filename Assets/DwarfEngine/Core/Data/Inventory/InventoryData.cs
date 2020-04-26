using System.Collections.Generic;

namespace DwarfEngine
{
    /// <summary>
    /// Used to convert inventory data to serializable object.
    /// </summary>
    [System.Serializable]
    public class InventoryData
    {
        public int[] itemIds;

        public InventoryData(ItemData[] items)
        {
            if (items.Length != 0)
            {
                int[] ids = new int[items.Length];

                for (int i = 0; i < ids.Length; i++)
                {
                    ids[i] = items[i].id;
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
