using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace DwarfEngine
{
    public class Health : MonoBehaviour, IProgressSource
    {
        public const float INVINCIBILITY_DURATION = 0.5f;

        public IntReactiveProperty currentHealth;
        public Subject<float> currentHealthNormalized { get; private set; }
        public ReadOnlyReactiveProperty<bool> isAlive { get; protected set; }

        /// <summary>
        /// Which direction the damage came from.
        /// </summary>
        public event System.Action<Vector2> OnDamage;
        
        [Header("Events")]
        public UnityEvent OnKill;

        [Header("Properties")]
        [SerializeField] private bool invincible;
        public int maximumHealth;
        public GameObject model;

        [Header("Death")]
        public bool destroyOnDeath;
        public float timeBeforeDestroy = 5f;

        //[Header("Feedbacks")]
        //public bool displayDamagePopup;
        //private ObjectPooler _pooler;

        [Header("Health Bar")]
        public bool hasHealthBar;
        public string _targetBar;
        public Vector3 _barOffset;
        public string targetBar => _targetBar;
        public Vector3 barOffset => _barOffset;

        protected virtual void Start()
        {
            currentHealth = new IntReactiveProperty(maximumHealth); // Set current health to max health
            currentHealthNormalized = new Subject<float>();

            currentHealth
                .Select(health => (float) health / maximumHealth) // Connect normalized value to actual value
                .Subscribe(currentHealthNormalized)
                .AddTo(this);

            isAlive = currentHealth
                .Select(health => health > 0) // isAlive is true when health is bigger than 0, false otherwise
                .ToReadOnlyReactiveProperty();

            if (hasHealthBar)
            {
                UIManager.Instance.SetProgressBar(model, this, currentHealthNormalized); // Set up the health bar
                currentHealthNormalized.OnNext(currentHealth.Value); // Set health bar value for the first time 
            }

            // TODO : Resolve
            //if (displayDamagePopup)
            //{
            //    _pooler = gameObject.AddComponent<ObjectPooler>();
            //    _pooler.objectToPool = GameAssets.Instance.GenericTextPopup.gameObject;
            //    _pooler.amountToPool = 3;
            //    _pooler.expandInNeed = true;
            //    _pooler.Initialization(true);
            //}
        }

        public virtual void Heal(int healAmount)
        {
            currentHealth.Value = Mathf.Min(currentHealth.Value + healAmount, maximumHealth);
        }

        public virtual void Damage(int damageAmount, float invincibilityDuration, GameObject damageDealer)
        {
            if (invincible)
            {
                return;
            }

            if (currentHealth.Value <= 0)
            {
                return;
            }

            OnDamage?.Invoke((transform.position - damageDealer.transform.position).normalized);

            //gameObject.transform.position += Vector3.zero;
            currentHealth.Value -= damageAmount;

            // TODO : Resolve
            //if (displayDamagePopup)
            //{
            //    TextPopup popup = _pooler.GetPooledObject<TextPopup>();
            //    if (popup != null)
            //    {
            //        popup.Setup(
            //            "-" + damageAmount.ToString(),
            //            transform.position + new Vector3(0f, /* TODO : Position the damage popup */ + 1f, 0f),
            //            textProperties.color,
            //            textProperties.isCritical,
            //            textProperties.isCritical ? 1.5f : 1
            //            );
            //        popup.gameObject.SetActive(true);
            //    }
            //}

            // TODO : Resolve
            //_animator?.SetTrigger(damagedParameter);

            if (currentHealth.Value <= 0)
            {
                currentHealth.Value = 0;

                Kill();
                return;
            }

            if (invincibilityDuration > 0)
            {
                StartCoroutine(Invincibility(invincibilityDuration));
            }
        }

        /// <summary>
        /// Override this to implement different effects when the character is killed. If destroyOnDeath is set to true, this might not be needed.
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

            OnKillBeforeDestroyed();

            if (destroyOnDeath)
            {
                Destroy(gameObject, timeBeforeDestroy);
            }
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
            return (float)currentHealth.Value / maximumHealth;
        }
    }
}
