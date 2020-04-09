/*
 Credits to GameDevGuide channel at YouTube
 https://www.youtube.com/channel/UCR35rzd4LLomtQout93gi0w
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class PanelGroup : MonoBehaviour
    {
        public bool activateOnStart;
        public TabGroup tabGroup;
        public List<Panel> panels;

        [Header("Tweening")]
        public bool tween;
        [ConditionalHide("tween", true)] public float tweenScale;
        [ConditionalHide("tween", true)] public float tweenSpeed;
        [ConditionalHide("tween", true)] public LeanTweenType easeType;

        private int panelIndex = 0;

        private void Start()
        {
            if (panels != null)
            {
                panels = new List<Panel>();
                foreach (Transform panel in transform)
                {
                    panels.Add(panel.GetComponent<Panel>());
                }
            }
            if (activateOnStart)
            {
                ShowCurrentPanel(false);
            }
        }

        private void ShowCurrentPanel(bool tween)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (i == panelIndex)
                {
                    if (tween)
                    {
                        panels[i].ShowPanel(true, Vector3.one, tweenSpeed, easeType);
                    }
                    else
                    {
                        panels[i].ShowPanel(true);
                    }
                }
                else
                {
                    if (tween)
                    {
                        panels[i].ShowPanel(false, Vector3.one * tweenScale, tweenSpeed, easeType);
                    }
                    else
                    {
                        panels[i].ShowPanel(false);
                    }
                }
            }
        }

        public void SetPanelIndex(int index)
        {
            panelIndex = index;
            ShowCurrentPanel(tween);
        }
    }
}