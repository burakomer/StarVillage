using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DwarfEngine
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler
    {
        public ItemAsset Item => item;
        
        [Header("Slot")]
        [SerializeField] private ItemAsset item;

        [Header("Graphics")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image selectionImage;
        public Sprite deselectedSprite;
        public Sprite selectedSprite;
        
        public void SetItem(ItemAsset newItem)
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

        public ItemAsset ReplaceItem(ItemAsset newItem)
        {
            ItemAsset oldItem = item;
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
