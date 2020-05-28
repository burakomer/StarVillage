using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace DwarfEngine
{
    public class Health : MonoBehaviour, IProgressSource
    {
        /// <summary>
        /// Global duration of the invincibility in seconds.
        /// </summary>
        public const float INVINCIBILITY_DURATION = 0.5f;

        /// <summary>
        /// The current health of the object
        /// </summary>
        public IntReactiveProperty currentHealth;

        /// <summary>
        /// The observable that broadcast the normalized value of the current health to its subscribers whenever the current health changes.
        /// </summary>
        public Subject<float> currentHealthNormalized { get; private set; }

        /// <summary>
        /// True if the object is alive, false if dead.
        /// </summary>
        public ReadOnlyReactiveProperty<bool> isAlive { get; protected set; }

        /// <summary>
        /// Called when the object receives damage. Other scripts can subscribe to this to execute feedbacks. Vector2 argument is the direction of the received attack. (Negative of the attackers direction.)
        /// </summary>
        public event System.Action<Vector2> OnDamage;
        
        /// <summary>
        /// Executed when the object is killed.
        /// </summary>
        [Header("Events")]
        public UnityEvent OnKill;

        [Header("Properties")]
        [SerializeField] private bool invincible;

        /// <summary>
        /// Maximum health of the object.
        /// </summary>
        public int maximumHealth;

        /// <summary>
        /// The visual model of the object.
        /// </summary>
        public GameObject model;

        [Header("Death")]
        public bool destroyOnDeath;

        /// <summary>
        /// Time before the object is destroyed.
        /// </summary>
        public float timeBeforeDestroy = 5f;

        //[Header("Feedbacks")]
        //public bool displayDamagePopup;
        //private ObjectPooler _pooler;

        /// <summary>
        /// If checked, a health bar is assigned to the object. 
        /// If it exists in the scene with the targetBar name, that one is used. 
        /// If not, a new one is created that is attached to the object.
        /// </summary>
        [Header("Health Bar")]
        public bool hasHealthBar;
        /// <summary>
        /// Inspector field for the targetBar.
        /// </summary>
        public string _targetBar;

        /// <summary>
        /// Inspector field for the barOffset.
        /// </summary>
        public Vector3 _barOffset;
        public string targetBar => _targetBar;
        public Vector3 barOffset => _barOffset;

        private void Start()
        {
            if (currentHealth != null) // Meaning that it is not set in the inspector,
            {
                currentHealth = new IntReactiveProperty(maximumHealth); // Set current health to max health. 
            }

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

        /// <summary>
        /// Heals the object.
        /// </summary>
        /// <param name="healAmount">Amount to heal.</param>
        public virtual void Heal(int healAmount)
        {
            currentHealth.Value = Mathf.Min(currentHealth.Value + healAmount, maximumHealth);
        }

        /// <summary>
        /// Damages the object.
        /// </summary>
        /// <param name="invincibilityDuration">The duration of invincibility which prevents incoming damages.</param>
        /// <param name="damageDealer">Source of the damage (the owner of the weapon).</param>
        public virtual void Damage(int damageAmount, float invincibilityDuration, GameObject damageDealer)
        {
            // If the current health is already zero or the invincible is true, return.
            if (invincible)
            {
                return;
            }

            if (currentHealth.Value <= 0)
            {
                return;
            }

            // Substract the damage value from the current health.
            currentHealth.Value -= damageAmount;

            OnDamage?.Invoke((transform.position - damageDealer.transform.position).normalized);

            // TODO : Fix
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

            // TODO : Fix
            //_animator?.SetTrigger(damagedParameter);

            // If the current health becomes zero, execute Kill and return.
            if (currentHealth.Value <= 0)
            {
                currentHealth.Value = 0;

                Kill();
                return;
            }

            // Start invincibility if it's not zero.
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

        /// <summary>
        /// Prevents incoming damages.
        /// </summary>
        public void DisableDamage()
        {
            invincible = true;
        }

        /// <summary>
        /// Incoming damages will be received normally.
        /// </summary>
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

        [System.Obsolete("Use currentHealthNormalized")]
        public float GetHealthNormalized()
        {
            return (float)currentHealth.Value / maximumHealth;
        }
    }
}
