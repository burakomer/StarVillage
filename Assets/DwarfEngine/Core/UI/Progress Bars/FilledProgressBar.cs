using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfEngine
{
    /// <summary>
    /// A UI element that displays the normalized graphical representation of a progress.
    /// </summary>
    public class FilledProgressBar : ProgressBarBase
    {
        public bool hideAtFull;

        [SerializeField] private Image frontBar;
        [SerializeField] private Image delayedBar;
        [SerializeField] private Image background;

        private const float DAMAGE_BAR_DELAY = 0.5f;
        private const float FADE_AMOUNT_EACH_TICK = 2.5f;

        private float damageBarDelayTimer;
        private bool delayedBarDamageInProgress = false;

        private void OnEnable()
        {
            if (hideAtFull)
            {
                frontBar.color = new Color(1, 1, 1, 0);
                delayedBar.color = new Color(1, 1, 1, 0);
                background.color = new Color(1, 1, 1, 0);
            }
        }

        protected virtual void OnBarDamage(float valueNormalized)
        {
            if (frontBar.color.a < 0.05f)
            {
                frontBar.color = new Color(1, 1, 1, 1);
                delayedBar.color = new Color(1, 1, 1, 1);
                background.color = new Color(1, 1, 1, 1);
            }

            frontBar.fillAmount = valueNormalized;
            damageBarDelayTimer = DAMAGE_BAR_DELAY;
            
            if (delayedBar != null)
            {
                if (!delayedBarDamageInProgress)
                {
                    StartCoroutine(DamageDelayedBar());
                }
            }

            if (hideAtFull && frontBar.fillAmount <= 0)
            {
                frontBar.color = new Color(1, 1, 1, 0);
                delayedBar.color = new Color(1, 1, 1, 0);
                background.color = new Color(1, 1, 1, 0);
            }
        }

        protected virtual void OnBarHeal(float valueNormalized)
        {
            frontBar.fillAmount = valueNormalized;
            delayedBar.fillAmount = valueNormalized;
        }

        protected virtual IEnumerator DamageDelayedBar()
        {
            delayedBarDamageInProgress = true;
            while (damageBarDelayTimer > 0)
            {
                damageBarDelayTimer -= Time.deltaTime;
                yield return null;
            }

            while (delayedBar.fillAmount > frontBar.fillAmount)
            {
                delayedBar.fillAmount -= FADE_AMOUNT_EACH_TICK * Time.deltaTime;
                yield return null;
            }

            delayedBar.fillAmount = frontBar.fillAmount;

            delayedBarDamageInProgress = false;
        }

        public override void OnNext(float value)
        {
            if (delayedBar.fillAmount > value)
            {
                OnBarDamage(value);
            }
            else if (delayedBar.fillAmount < value)
            {
                OnBarHeal(value);
            }
        }

        public static FilledProgressBar Create(GameObject obj, IProgressSource source)
        {
            RectTransform barCanvas = Instantiate(GameAssets.Instance.GenericHealthBar.GetComponent<RectTransform>(), obj.transform);
            barCanvas.localPosition = source.barOffset;
            FilledProgressBar newBar = barCanvas.GetComponentInChildren<FilledProgressBar>();
            newBar.barName = source.targetBar;
            return newBar;
        }
    }
}