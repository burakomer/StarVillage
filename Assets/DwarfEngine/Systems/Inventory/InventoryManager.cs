using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// The manager of inventories.
    /// Used as a singleton object for every scene.
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static InventoryManager Instance;

        /// <summary>
        /// The ItemDatabase object that stores the items in the game and their IDs.
        /// </summary>
        public ItemDatabase itemDatabase;

        /// <summary>
        /// Name of the main inventory. Every other inventory is connected to the main inventory.
        /// </summary>
        public string mainInventoryName;

        /// <summary>
        /// Array of existing inventories in the scene.
        /// </summary>
        private Inventory[] inventories;

        private void Awake()
        {
            this.SetSingleton(ref Instance);

            // Get all inventories in the scene.
            inventories = FindObjectsOfType<Inventory>();
            for (int i = 0; i < inventories.Length; i++)
            {
                // Every inventory in the scene is owned by the player.
                inventories[i].SetOwner(LevelManager.Instance.player);

                // If the inventory is main inventory, connect all other inventories to it.
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
                            (inventories[i] as IStorageInventory).Connect(inventory);
                        }
                    }
                }
            }
        }
    }
}
