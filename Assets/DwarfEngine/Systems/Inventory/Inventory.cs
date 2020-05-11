using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class Inventory : MonoBehaviour, IEnumerable
    {
        public string inventoryName;
        public int size;

        protected List<ItemAsset> items;
        protected Character owner;

        public ItemAsset this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        #region Unity Methods
        private void Awake()
        {
            items = new List<ItemAsset>(size);

            for (int i = 0; i < size; i++)
            {
                items.Add(null);
            }

            GameManager.Instance.OnSaveInitiated += OnSaveInitiated;
            GameManager.Instance.OnLoadInitiated += OnLoadInitiated;

            PreInit();
        }

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnSaveInitiated -= OnSaveInitiated;
            GameManager.Instance.OnLoadInitiated -= OnLoadInitiated;
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Called in Awake.
        /// </summary>
        protected virtual void PreInit()
        {

        }

        /// <summary>
        /// Called in Start.
        /// </summary>
        protected virtual void Init()
        {

        }

        public virtual void OnSaveInitiated()
        {
            InventoryData data = new InventoryData(items);

            SaveManager.Save(data, SaveKeyContainer.InventoryData + inventoryName);
        }

        public virtual void OnLoadInitiated()
        {
            if (SaveManager.SaveExists(SaveKeyContainer.InventoryData + inventoryName))
            {
                InventoryData data = SaveManager.Load<InventoryData>(SaveKeyContainer.InventoryData + inventoryName);
                items = InventoryManager.Instance.itemDatabase.GetItemsFromData(data.itemIds);
            }
        } 
        #endregion
        
        #region Abstract Methods
        public abstract bool PlaceItem(ItemAsset newItem);
        public abstract void Interact(int slotIndex);
        public abstract void MoveItem(Inventory targetInventory, int startingIndex, int targetIndex); 
        #endregion

        public bool HasItem(ItemAsset item)
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
    }
}
