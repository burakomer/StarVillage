using System;
using UnityEngine;

namespace DwarfEngine
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public event Action<string, float> OnBarDamage;
        public event Action<string, float> OnBarHeal;

        private void Awake()
        {
            this.SetSingleton(ref Instance);
        }

        #region Progress Bar Controls
        public void BarDamage(string name, float damageNormalized)
        {
            OnBarDamage?.Invoke(name, damageNormalized);
        }

        public void BarHeal(string name, float healNormalized)
        {
            OnBarHeal?.Invoke(name, healNormalized);
        }

        public void CreateBar(GameObject obj, string barName, Vector3 offset)
        {
            RectTransform barCanvas = Instantiate(GameAssets.Instance.GenericHealthBar.GetComponent<RectTransform>(), obj.transform);
            barCanvas.localPosition = new Vector3(0f, 0f, 0f) + offset;

            barCanvas.GetComponentInChildren<ProgressBar>().name = barName;
        }
        #endregion

    }
}