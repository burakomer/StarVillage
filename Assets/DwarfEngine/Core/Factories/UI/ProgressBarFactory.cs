using UnityEngine;

namespace DwarfEngine
{
    public static class ProgressBarFactory
    {
        public static ProgressBar CreateBar(GameObject obj, IProgressSource source)
        {
            RectTransform barCanvas = Object.Instantiate(GameAssets.Instance.GenericHealthBar.GetComponent<RectTransform>(), obj.transform);
            barCanvas.localPosition = new Vector3(0f, 0f, 0f) + source.barOffset;
            ProgressBar newBar = barCanvas.GetComponentInChildren<ProgressBar>();
            newBar.barName = source.targetBar;
            return newBar;
        }
    }
}
