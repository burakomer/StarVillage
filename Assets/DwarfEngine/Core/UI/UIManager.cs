using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DwarfEngine
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public Camera UICamera;
        public Canvas canvas;

        public List<ProgressBar> bars;

        public event Action<string, float> OnBarDamage;
        public event Action<string, float> OnBarHeal;

        private void Awake()
        {
            this.SetSingleton(ref Instance);

            UICamera = GetComponent<Camera>();
            canvas = GetComponentInChildren<Canvas>();

            bars = FindObjectsOfType<ProgressBar>().ToList();
        }

        #region Progress Bar Controls

        public void SetProgressBar(GameObject gObj, IProgressSource source, IObservable<float> value)
        {
            ProgressBar bar = bars.Find(b => b.name == source.targetBar) ?? ProgressBarFactory.CreateBar(gObj, source);
            value.Subscribe(bar);
        }

        public void BarDamage(string name, float damageNormalized)
        {
            OnBarDamage?.Invoke(name, damageNormalized);
        }

        public void BarHeal(string name, float healNormalized)
        {
            OnBarHeal?.Invoke(name, healNormalized);
        }
        
        #endregion
    }
}