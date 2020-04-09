using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DwarfEngine
{
    public class Health : MonoBehaviour
    {
        public int currentHealth { get; protected set; }
        public bool isAlive { get; protected set; }

        [Header("Properties")]
        public bool invincible;
        public int maximumHealth;
        public GameObject model;

        [Header("Death")] 
        public UnityEvent OnKill;
        public bool destroyOnDeath;
        public float timeBeforeDestroy = 5f;

        [Header("Feedbacks")]
        public bool displayDamagePopup;

        [Header("Health Bar")]
        public bool hasHealthBar;
        public string TargetBar;
        public Vector3 barOffset;

        [Header("Animator Parameters")]
        [Tooltip("Bool")] public string aliveParameter;
        [Tooltip("Trigger")] public string damagedParameter;

        protected Animator _animator;
        protected ObjectPooler _pooler;

        protected virtual void Start()
        {
            isAlive = true;

            _animator = model.GetComponent<Animator>();

            currentHealth = maximumHealth;
            
            // Text popup initialization
            if (displayDamagePopup)
            {
                _pooler = gameObject.AddComponent<ObjectPooler>();
                _pooler.objectToPool = GameAssets.Instance.GenericTextPopup.gameObject;
                _pooler.amountToPool = 3;
                _pooler.expandInNeed = true;
                _pooler.Initialization(true);
            }
        }

        /// <summary>
        /// Inflicts damage.
        /// </summary>
        /// <param name="damageAmount">Amount to substract from currentHealth</param>
        /// <param name="invincibilityDuration">Duration of invincibility.</param>
        /// <param name="textProperties">Properties of the text popup.</param>
        /// <param name="damageDealer">Object of the damage dealer.</param>
        public virtual void Damage(int damageAmount, float invincibilityDuration, 
            (bool isCritical, Color color) textProperties, GameObject damageDealer = null)
        {
            if (invincible)
            {
                return;
            }

            if (currentHealth <= 0)
            {
                return;
            }

            //gameObject.transform.position += Vector3.zero;
            currentHealth -= damageAmount;

            if (displayDamagePopup)
            {
                TextPopup popup = _pooler.GetPooledObject<TextPopup>();
                if (popup != null)
                {
                    popup.Setup(
                        "-" + damageAmount.ToString(),
                        transform.position + new Vector3(0f, /* TODO : Position the damage popup */ + 1f, 0f),
                        textProperties.color,
                        textProperties.isCritical,
                        textProperties.isCritical ? 1.5f : 1
                        );
                    popup.gameObject.SetActive(true);
                }
            }

            if (hasHealthBar) UIManager.Instance.BarDamage(TargetBar, GetHealthNormalized());

            _animator?.SetTrigger(damagedParameter);
            
            if (currentHealth <= 0)
            {
                Kill();
                return;
            }

            if (invincibilityDuration > 0)
            {
                StartCoroutine(Invincibility(invincibilityDuration));
                //if (flash) StartCoroutine(FlashModel());
            }
        }

        /// <summary>
        /// What to do after the object is killed. For example, disable the collider so it can't be interactable anymore.
        /// </summary>
        protected virtual void OnKillBeforeDestroyed()
        {
            GetComponent<Collider2D>().enabled = false;
        }

        /// <summary>
        /// Kills the character. OnKill executes the actions subscribed to it.
        /// </summary>
        public virtual void Kill()
        {
            OnKill?.Invoke();

            isAlive = false;

            OnKillBeforeDestroyed();

            _animator.SetBool(aliveParameter, isAlive);
            if (destroyOnDeath)
            {
                Destroy(gameObject, timeBeforeDestroy);
            }
        }

        /// <summary>
        /// Heals the object.
        /// </summary>
        /// <param name="healAmount">Amount to add to the currentHealth.</param>
        public virtual void Heal(int healAmount)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maximumHealth);
            if (hasHealthBar) UIManager.Instance.BarHeal(TargetBar, GetHealthNormalized());
        }

        public void DisableDamage()
        {
            invincible = true;
        }

        public void EnableDamage()
        {
            invincible = false;
        }

        /// <summary>
        /// Waits for seconds before enabling damage.
        /// </summary>
        /// <param name="duration">Seconds to wait.</param>
        public IEnumerator Invincibility(float duration)
        {
            DisableDamage();
            yield return new WaitForSeconds(duration);
            EnableDamage();
        }

        public float GetHealthNormalized()
        {
            return (float)currentHealth / maximumHealth;
        }

        //public IEnumerator FlashModel()
        //{
        //    while (invincible)
        //    {
        //        _renderer.enabled = false;
        //        yield return new WaitForSeconds(0.1f);
        //        _renderer.enabled = true;
        //        yield return new WaitForSeconds(0.1f);
        //    }
        //    _renderer.enabled = true;
        //}
    }
}
