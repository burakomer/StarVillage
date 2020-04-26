using UnityEngine;
using System.Collections;

namespace DwarfEngine
{
    [RequireComponent(typeof(Inventory))]
    public abstract class InventoryType : MonoBehaviour
    {
        public abstract void Interact(int slotIndex);
    }
}