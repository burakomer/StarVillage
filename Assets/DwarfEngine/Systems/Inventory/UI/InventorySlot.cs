using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DwarfEngine
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler
    {
        public ItemObject Item => item;
        
        [Header("Slot")]
        [SerializeField] private ItemObject item;

        [Header("Graphics")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image selectionImage;
        public Sprite deselectedSprite;
        public Sprite selectedSprite;
        
        public void SetItem(ItemObject newItem)
        {
            item = newItem;
            if (item != null)
            {
                iconImage.sprite = item.icon;
            }
            else
            {
                iconImage.sprite = deselectedSprite;
            }
        }

        public ItemObject ReplaceItem(ItemObject newItem)
        {
            ItemObject oldItem = item;
            SetItem(newItem);
            return oldItem;
        }

        #region Event Handling
        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrop(PointerEventData eventData)
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        } 
        #endregion
    }
}
