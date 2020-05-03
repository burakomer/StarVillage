using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        public ItemDatabase itemDatabase;
        public string mainInventoryName;

        private Inventory[] inventories;

        private void Awake()
        {
            this.SetSingleton(ref Instance);

            inventories = FindObjectsOfType<Inventory>();
            for (int i = 0; i < inventories.Length; i++)
            {
                inventories[i].SetOwner(LevelManager.Instance.player);

                if (inventories[i].inventoryName == mainInventoryName)
                {
                    foreach (Inventory inventory in inventories)
                    {
                        if(inventory == inventories[i])
                        {
                            continue;
                        }
                        else
                        {
                            (inventories[i] as IBasicInventory).Connect(inventory);
                        }
                    }
                }
            }
        }
    }
}
