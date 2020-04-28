using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class Inventory : MonoBehaviour, IEnumerable
    {
        public string inventoryName;

        protected List<ItemObject> items;
        protected Character owner;

        public ItemObject this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {

        }

        public abstract bool PlaceItem(ItemObject newItem);
        public abstract void Interact(int slotIndex);
        public abstract void MoveItem(Inventory targetInventory, int startingIndex, int targetIndex);

        public bool HasItem(ItemObject item)
        {
            return items.Contains(item);
        }

        public Inventory SetOwner(Character _owner)
        {
            owner = _owner;
            return this;
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }

        public abstract void OnSaveInitiated();
    }
}
