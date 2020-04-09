/*
 Credits to GameDevGuide channel at YouTube
 https://www.youtube.com/channel/UCR35rzd4LLomtQout93gi0w
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class TabGroup : MonoBehaviour
    {
        public enum SelectionStyle
        {
            Individual,
            Focused
        }

        public List<Tab> tabs;
        public PanelGroup panelGroup;

        [Header("States")] 

        public Color idleColor;
        public Color hoverColor;
        public Color selectedColor;

        [Header("Tweening")] 
        public bool tweening;
        [ConditionalHide("tweening", true)] public SelectionStyle selectionStyle;
        [Tooltip("It must be the first child of this object.")] [ConditionalHide("tweening")] public GameObject focusObject;

        [Header("Selected")]
        [ConditionalHide("tweening", true)] public LeanTweenType onSelectTweenType;
        [ConditionalHide("tweening", true)] public float onSelectTweenTime;
        [ConditionalHide("tweening", true)] public float onSelectScale;

        [Header("Deselected")]
        [ConditionalHide("tweening", true)] public LeanTweenType onDeselectTweenType;
        [ConditionalHide("tweening", true)] public float onDeselectTweenTime;

        private Tab selectedTab;

        protected virtual IEnumerator Start()
        {
            tabs = new List<Tab>();

            foreach (Transform tab in transform)
            {
                Tab tabComponent = tab.GetComponent<Tab>();
                if (tabComponent != null)
                {
                    tabs.Add(tabComponent);
                    tabComponent.tabGroup = this;
                }
            }

            if (selectionStyle == SelectionStyle.Focused)
            {
                focusObject = transform.GetChild(0).gameObject;
                RectTransform focusObjecTransform = focusObject.GetComponent<RectTransform>();

                // Skip a frame to let the layout group finish initializing
                yield return null; 

                focusObjecTransform.localPosition = transform.GetChild(1).localPosition;
                focusObjecTransform.sizeDelta = transform.GetChild(1).GetComponent<RectTransform>().sizeDelta;
            }

            OnTabSelected(tabs[0]);
        }

        public void OnTabEnter(Tab tab)
        {
            ResetTabs();
            if (selectedTab == null || tab != selectedTab)
            {
                tab.backgroundImage.color = hoverColor;

                // Tweening
            }
        }

        public void OnTabExit(Tab tab)
        {
            ResetTabs();
        }

        public void OnTabSelected(Tab tab)
        {
            if (selectedTab != null)
            {
                selectedTab.OnTabDeselected();
                SelectionAnimation(selectedTab.rectTransform, Vector3.one, onDeselectTweenTime, onDeselectTweenType);
            }
            selectedTab = tab;
            selectedTab.OnTabSelected();
            SelectionAnimation(selectedTab.rectTransform, Vector3.one * onSelectScale, onSelectTweenTime, onSelectTweenType);

            ResetTabs();
            tab.backgroundImage.color = selectedColor;

            if (panelGroup != null)
            {
                panelGroup.SetPanelIndex(tabs.IndexOf(tab));
            }
        }

        public void ResetTabs()
        {
            foreach (Tab tab in tabs)
            {
                if (selectedTab != null && tab == selectedTab) { continue; }
                tab.backgroundImage.color = idleColor;
            }
        }

        public void SelectionAnimation(RectTransform rectTransform, Vector3 scale, float scaleTime, LeanTweenType tweenType)
        {
            if (tweening)
            {
                LeanTween.cancel(rectTransform.gameObject);

                switch (selectionStyle)
                {
                    case SelectionStyle.Individual:
                        LeanTween.scale(rectTransform, scale, scaleTime).setEase(tweenType);
                        break;
                    case SelectionStyle.Focused:
                        LeanTween.moveLocal(focusObject, rectTransform.localPosition, onSelectTweenTime).setEase(tweenType);
                        break;
                }
            }
        }
    }
}