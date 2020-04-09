using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DwarfEngine
{
    public enum NavigationType
    {
        Push,
        Pop,
        Peek,
        PeekPop
    }

    /// <summary>
    /// Caches panels for easy navigation.
    /// </summary>
    public class NavigationManager : MonoBehaviour
    {
        public static NavigationManager Instance;
        
        [Tooltip("Beginning panel.")] public Panel homePanel;

        [Header("Tweening")]
        public bool tween;
        [ConditionalHide("tween", true)] public float tweenScale;
        [ConditionalHide("tween", true)] public float tweenSpeed;
        [ConditionalHide("tween", true)] public LeanTweenType easeType;

        private Panel currentPanel;
        private List<Panel> panelsInScene;
        private List<(Panel key, Panel previous)> panelStack; // Singly Linked List

        private void Awake()
        {
            this.SetSingleton(ref Instance);
        }

        private void Start()
        {
            panelsInScene = new List<Panel>();
            foreach(Panel panel in FindObjectsOfType<Panel>().Where(panel => panel.useNavigationManager == true))
            {
                panelsInScene.Add(panel);
            }

            panelStack = new List<(Panel, Panel)>();

            // Initialize panels
            foreach (Panel panel in panelsInScene)
            {
                if (tween)
                {
                    panel.ShowPanel(false, Vector3.one * tweenScale, 0, easeType);
                }
            }

            Push(homePanel.name);
        }

        /// <summary>
        /// Add a panel to the end of the stack.
        /// </summary>
        /// <param name="panelName">Name of the panel to add.</param>
        public void Push(string panelName)
        {
            Panel nextPanel = panelsInScene.Find(panel => panel.name == panelName);
            if (nextPanel == homePanel)
            {
                panelStack.Clear();
            }
            else
            {
                panelStack.Add((nextPanel, currentPanel));
            }

            ChangeCurrentPanel(nextPanel);
        }

        /// <summary>
        /// Remove the current panel from the stack.
        /// </summary>
        public void Pop()
        {
            Panel previousPanel = panelStack[panelStack.Count - 1].previous;
            
            if (previousPanel != null)
            {
                panelStack.RemoveAt(panelStack.Count - 1);
                ChangeCurrentPanel(previousPanel);
            }
            else
            {
                Debug.Log("Can't pop anymore.");
            }
        }

        /// <summary>
        /// Display an independent panel.
        /// </summary>
        /// <param name="panelName">Name of the panel to show.</param>
        /// <param name="mustAct">Disable interaction on the current panel.</param>
        public void Peek(string panelName, bool mustAct)
        {
            Panel peekPanel = panelsInScene.Find(panel => panel.name == panelName);

            if (mustAct)
            {
                currentPanel.SetInteractable(false);
            }

            if (tween)
            {
                peekPanel.ShowPanel(true, Vector3.one, tweenSpeed, easeType);
            }
            else
            {
                peekPanel.ShowPanel(true);
            }
        }

        /// <summary>
        /// Remove an independent panel;
        /// </summary>
        /// /// <param name="panelName">Name of the panel to remove.</param>
        /// <param name="mustAct">Enable interaction on the current panel.</param>
        public void Pop(string panelName, bool mustAct)
        {
            Panel peekPanel = panelsInScene.Find(panel => panel.name == panelName);

            if (mustAct)
            {
                currentPanel.SetInteractable(true);
            }

            if (tween)
            {
                peekPanel.ShowPanel(false, Vector3.one * tweenScale, tweenSpeed, easeType);
            }
            else
            {
                peekPanel.ShowPanel(false);
            }
        }

        private void ChangeCurrentPanel(Panel newPanel)
        {
            if (currentPanel != null)
            {
                if (tween)
                {
                    currentPanel.ShowPanel(false, Vector3.one * tweenScale, tweenSpeed, easeType);
                }
                else
                {
                    currentPanel.ShowPanel(false);
                }
            }
            currentPanel = newPanel;

            if (tween)
            {
                currentPanel.ShowPanel(true, Vector3.one, tweenSpeed, easeType);
            }
            else
            {
                currentPanel.ShowPanel(true);
            }
        }
    }
}