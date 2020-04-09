using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfEngine
{
    /// <summary>
    /// A UI element that displays the normalized graphical representation of a progress.
    /// </summary>
    public class ProgressBar : MonoBehaviour
    {
        [Header("Properties")]
        public new string name;
        public bool hideAtFull;

        [SerializeField] private Image frontBar;
        [SerializeField] private Image delayedBar;
        [SerializeField] private Image background;

        private const float damageBarFadeTimerMax = 0.5f;
        private const float fadeAmount = 2.5f;
        private float damageBarFadeTimer;
        private bool damageBarInProgress = false;

        protected virtual void OnEnable()
        {
            UIManager.Instance.OnBarDamage += OnBarDamage;
            UIManager.Instance.OnBarHeal += OnBarHeal;

            if (hideAtFull)
            {
                frontBar.color = new Color(1, 1, 1, 0);
                delayedBar.color = new Color(1, 1, 1, 0);
                background.color = new Color(1, 1, 1, 0);
            }
        }

        protected virtual void OnDisable()
        {
            UIManager.Instance.OnBarDamage -= OnBarDamage;
            UIManager.Instance.OnBarHeal -= OnBarHeal;
        }

        protected virtual void OnBarDamage(string name, float damageNormalized)
        {
            if (this.name == name)
            {
                if (frontBar.color.a < 0.05f)
                {
                    frontBar.color = new Color(1, 1, 1, 1);
                    delayedBar.color = new Color(1, 1, 1, 1);
                    background.color = new Color(1, 1, 1, 1);
                }

                frontBar.fillAmount = damageNormalized;
                damageBarFadeTimer = damageBarFadeTimerMax;
                if (delayedBar != null)
                {
                    if (!damageBarInProgress)
                    {
                        StartCoroutine(DamageBarImage());
                    }
                }

                if (hideAtFull && frontBar.fillAmount <= 0)
                {
                    frontBar.color = new Color(1, 1, 1, 0);
                    delayedBar.color = new Color(1, 1, 1, 0);
                    background.color = new Color(1, 1, 1, 0);
                }
            }
        }

        protected virtual IEnumerator DamageBarImage()
        {
            damageBarInProgress = true;
            while (damageBarFadeTimer > 0)
            {
                damageBarFadeTimer -= Time.deltaTime;
                yield return null;
            }

            while (delayedBar.fillAmount > frontBar.fillAmount)
            {
                delayedBar.fillAmount -= fadeAmount * Time.deltaTime;
                yield return null;
            }

            damageBarInProgress = false;
        }

        protected virtual void OnBarHeal(string name, float healNormalized)
        {
            if (this.name == name)
            {
                frontBar.fillAmount = healNormalized;
            }
        }
    }
}