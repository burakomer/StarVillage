﻿using UnityEngine;
using System.Collections;

namespace DwarfEngine
{
    public abstract class EquipmentInventory<T> : StaticInventory, ISpecialInventory where T : IEquipmentItem
    {
        public StorageInventory mainInventory { get; set; }

        public override bool PlaceItem(ItemData newItem)
        {
            // First, check for an empty slot and try to equip the item
            for (int i = 0; i < items.Capacity; i++)
            {
                if (items[i] == null)
                {
                    if((newItem as IEquipmentItem).Equip(owner.equipmentManager, i))
                    {
                        items[i] = newItem;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // If there's no empty slot, check for an empty index in the main inventory and try to equip the item to the first slot
            var emptyIndex = mainInventory.GetEmptyIndex();
            if (emptyIndex != null)
            {
                if ((newItem as IEquipmentItem).Equip(owner.equipmentManager, 0))
                {
                    mainInventory[emptyIndex.Value] = items[0];
                    items[0] = newItem;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Double clicking to an equipment inventory slot unequips the item.
        /// </summary>
        public override void Interact(int slotIndex)
        {
            if (mainInventory.PlaceItem(items[slotIndex])) // Try to add the equipment back to the inventory
            {
                (items[slotIndex] as IEquipmentItem).Unequip(owner.equipmentManager, slotIndex);
                items[slotIndex] = null;
            }
        }

        public override void MoveItem(Inventory targetInventory, int startingIndex, int targetIndex)
        {
            if (targetInventory != this)
            {
                if (targetInventory[targetIndex] is T)
                {
                    if ((targetInventory[targetIndex] as IEquipmentItem).Equip(owner.equipmentManager, startingIndex))
                    {
                        ItemData tempItem = targetInventory[targetIndex];
                        targetInventory[targetIndex] = items[startingIndex];
                        items[startingIndex] = tempItem;
                    }
                }
                else if (targetInventory[targetIndex] == null)
                {
                    (items[startingIndex] as IEquipmentItem).Unequip(owner.equipmentManager, startingIndex);
                    items[startingIndex] = null;
                }
            }
        }
    }
}