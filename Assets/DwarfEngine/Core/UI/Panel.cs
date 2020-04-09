using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DwarfEngine
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class Panel : MonoBehaviour
    {
        public bool useNavigationManager;
        [ConditionalHide("useNavigationManager", true)] public new string name;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void ShowPanel(bool show)
        {
            SetInteractable(show);
            _canvasGroup.alpha = show ? 1 : 0;
        }

        /// <summary>
        /// Show panel with alpha and scale tweening
        /// </summary>
        /// <param name="show"></param>
        /// <param name="scale"></param>
        /// <param name="tweenSpeed"></param>
        /// <param name="easeType"></param>
        public void ShowPanel(bool show, Vector3 scale, float tweenSpeed, LeanTweenType easeType)
        {
            SetInteractable(show);

            // Tween alpha and scale
            LeanTween.cancel(gameObject);
            _canvasGroup.LeanAlpha(show ? 1 : 0, tweenSpeed).setEase(easeType);
            _rectTransform.LeanScale(scale, tweenSpeed).setEase(easeType);
        }

        public void SetInteractable(bool interactable)
        {
            _canvasGroup.blocksRaycasts = interactable;
        }
    }
}
