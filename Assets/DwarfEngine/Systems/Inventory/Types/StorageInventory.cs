using System;
using System.Collections.Generic;
using System.Linq;

namespace DwarfEngine
{
    public class StorageInventory : Inventory, IStorageInventory
    {
        /// <summary>
        /// List of the connected inventories.
        /// </summary>
        public List<Inventory> connectedInventories { get; private set; }

        /// <summary>
        /// Called in Character Inventory Manager for the Main Inventory.
        /// </summary>
        /// <param name="inventoryToConnect"></param>
        public void Connect(Inventory inventoryToConnect)
        {
            if (connectedInventories == null)
            {
                connectedInventories = new List<Inventory>();
            }

            if (connectedInventories.Contains(inventoryToConnect))
            {
                return;
            }

            connectedInventories.Add(inventoryToConnect);

            if (inventoryToConnect is IStorageInventory)
            {
                (inventoryToConnect as IStorageInventory).Connect(this);
            }
            else if (inventoryToConnect is ISecondaryInventory)
            {
                (inventoryToConnect as ISecondaryInventory).mainInventory = this;
            }
        }

        /// <summary>
        /// Return a slot index that's empty. Return null if there's not any empty slot.
        /// </summary>
        /// <returns></returns>
        public int? GetEmptyIndex()
        {
            for (int i = 0; i < items.Capacity; i++)
            {
                if (items[i] == null)
                {
                    return i;
                }
            }

            return null;
        }

        public override void Interact(int slotIndex)
        {
            if (items[slotIndex] is IEquipmentItem)
            {
                Inventory targetInv = connectedInventories
                    .Find(i => i.inventoryName == (items[slotIndex] as IEquipmentItem).TargetInventory);
                targetInv.PlaceItem(items[slotIndex]);
            }
        }

        public override bool PlaceItem(ItemAsset newItem)
        {
            if (newItem is IStackableItem)
            {
                int? emptyIndex = null;
                for (int i = 0; i < items.Capacity; i++)
                {
                    if (items[i].id == newItem.id)
                    {
                        if (((items[i] as IStackableItem).count + (newItem as IStackableItem).count) <= (items[i] as IStackableItem).maxStack)
                        {
                            (items[i] as IStackableItem).count += (newItem as IStackableItem).count;
                            return true;
                        }
                    }
                    else if (items[i] == null)
                    {
                        if (emptyIndex == null)
                            emptyIndex = i;
                    }
                }
                if (emptyIndex != null)
                {
                    items[emptyIndex.Value] = newItem;
                    return true;
                }
            }
            else
            {
                for (int i = 0; i < items.Capacity; i++)
                {
                    if (items[i] == null)
                    {
                        items[i] = newItem;
                        return true;
                    }
                }
            }

            return false; // No empty slot.
        }

        public override void MoveItem(Inventory targetInventory, int startingIndex, int targetIndex)
        {
            if (targetInventory != this && !connectedInventories.Contains(targetInventory))
            {
                return;
            }

            if (targetInventory is ISecondaryInventory)
            {
                targetInventory.MoveItem(this, targetIndex, startingIndex);
            }
            else // Basic storage item moving logic
            {
                ItemAsset tempItem = targetInventory[targetIndex];
                targetInventory[targetIndex] = items[startingIndex];
                items[startingIndex] = tempItem;
            }
        }
    }
}
