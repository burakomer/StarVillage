using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	/// <summary>
	/// The database that every object exists in.
	/// </summary>
	[CreateAssetMenu(fileName = "New Item Database", menuName = "Data/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemDatabaseEntry> catalog;

        public void SetItemIds()
        {
            // Set the id of all items
            foreach (ItemDatabaseEntry entry in catalog)
            {
                entry.item.id = entry.id;
            }
        }

        public ItemObject GetItemWithId(int id)
        {
            return catalog
                .Find(entry => entry.id == id).item;
        }

        public List<ItemObject> GetItemsFromData(int[] ids)
        {
            List<ItemObject> items = new List<ItemObject>();

            if (ids != null)
            {
                foreach (int id in ids)
                {
                    items.Add(GetItemWithId(id));
                }
            }
            return items;
        }
    }

    [System.Serializable]
    public class ItemDatabaseEntry
    {
        public int id;
        public ItemObject item;
    }
}