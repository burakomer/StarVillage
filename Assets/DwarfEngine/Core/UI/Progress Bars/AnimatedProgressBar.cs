using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfEngine
{
    public class AnimatedProgressBar : ProgressBarBase
    {
        [Range(0, 1f)] public float fillAmount;

        public float FillAmount => currentFillAmount / maxDistance;

        [Header("Points")]
        public Vector2 startPoint;
        public Vector2 endPoint;

        [Header("Graphics")]
        public RectTransform barImage;

        private float maxDistance;
        private float currentFillAmount;

        private void Start()
        {
            maxDistance = Vector2.Distance(startPoint, endPoint);
        }

        public override void OnNext(float value)
        {
            barImage.localPosition = Vector2Int.FloorToInt(Vector2.Lerp(endPoint, startPoint, value)).ToVector3();
            currentFillAmount = maxDistance * value;
        }
    }
}
