/*
 Credits to GameDevGuide channel at YouTube
 https://www.youtube.com/channel/UCR35rzd4LLomtQout93gi0w
*/

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DwarfEngine
{
    [RequireComponent(typeof(Image), typeof(Canvas), typeof(GraphicRaycaster))]
    public class Tab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IDragHandler, IDropHandler
    {
        public TabGroup tabGroup;
        [HideInInspector] public Image backgroundImage;
        [HideInInspector] public RectTransform rectTransform;
        [HideInInspector] public Canvas canvas;

        [Header("Events")] 
        
        public UnityEvent TabSelected;
        public UnityEvent TabDeselected;

        private bool pointerLeft;

        private void Awake()
        {
            backgroundImage = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
            canvas.sortingOrder = 1;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
            pointerLeft = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
            pointerLeft = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!pointerLeft)
            {
                tabGroup.OnTabSelected(this);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnPointerUp(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnTabSelected()
        {
            canvas.overrideSorting = true;
            TabSelected?.Invoke();
        }

        public void OnTabDeselected()
        {
            canvas.overrideSorting = false;
            TabDeselected?.Invoke();
        }
    }
}