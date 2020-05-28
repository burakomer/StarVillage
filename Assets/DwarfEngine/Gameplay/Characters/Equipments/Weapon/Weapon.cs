using System.Collections;
using UnityEngine;

namespace DwarfEngine
{
    public enum WeaponState { Idle, Requested, Started, Shooting, Stopping, Recharging }

    [RequireComponent(typeof(WeaponAim))]
    [DisallowMultipleComponent]
    public abstract class Weapon : Equipment, IActiveEquipment
    {
        /// <summary>
        /// The object that houses the renderer of the weapon.
        /// </summary>
        [Header("Graphics")]
        public GameObject weaponModel;

        /// <summary>
        /// SpriteRenderer of the weapon.
        /// </summary>
        [SerializeField] protected SpriteRenderer _renderer;

        [Header("Settings")]
        public Stat damage;
        public Stat rechargeTime;

        /// <summary>
        /// LayerMask that specifies what layers will this weapon hit. Must be set in inspector.
        /// </summary>
        public LayerMask hitMask;

        [Header("Animator Parameters")] // Self-explanatory
        public string weaponIdleAnim;
        public string weaponPrepareAttackAnim;
        public string weaponAttackAnim;

        /// <summary>
        /// Current state of the weapon.
        /// </summary>
        protected WeaponState state;

        /// <summary>
        /// The cooldown object of the weapon that is used for recharging.
        /// </summary>
        protected Cooldown cooldown;

        /// <summary>
        /// This stays true till the weapon processor with attackTime set to ButtonRelease stops executing an attack.
        /// </summary>
        public bool buttonReleased;

        #region Components
        [HideInInspector] public WeaponAim aim;
        [HideInInspector] public WeaponCharge charge;
        [HideInInspector] public WeaponResource resource;
        [HideInInspector] public WeaponProcessor processor;
        #endregion

        #region Logic

        #region Initialization and Update
        private void Start()
        {
            // Initialize the cooldown object.
            cooldown = new Cooldown(rechargeTime.FloatValue, () => SetState(WeaponState.Idle));
            
            // Set the initial state to Idle.
            state = WeaponState.Idle;

            // Fill the component references.
            aim = GetComponent<WeaponAim>();
            charge = GetComponent<WeaponCharge>();
            resource = GetComponent<WeaponResource>();
            processor = GetComponent<WeaponProcessor>();

            //_renderer = weaponModel.GetComponent<SpriteRenderer>();

            //Init();
        }

        /// <summary>
        /// Used to update the cooldown.
        /// </summary>
        private void Update()
        {
            cooldown.Update();
        }
        #endregion

        #region Active Equipment

        /// <summary>
        /// Called when the player releases the attack button. If the weapon is not started, it is not called.
        /// </summary>
        public void StopEquipment()
        {
            SetState(WeaponState.Stopping);
        }

        /// <summary>
        /// Requests to start the weapon from scratch. Works only if the current state is Idle.
        /// </summary>
        public bool StartEquipment()
        {
            return SetState(WeaponState.Requested);
        }

        /// <summary>
        /// If the start request goes through, the weapon is started.
        /// </summary>
        public IEnumerator StartWeapon()
        {
            PrepareAttack();

            // If charge component exists, start charging and wait till it completes.
            if (charge != null)
            {
                yield return StartCoroutine(charge.StartCharging()); 
            }

            ProcessAttack(); // Proceed to attack.
        }

        /// <summary>
        /// Sets the state to Shooting (tries to fire the weapon).
        /// </summary>
        public void ProcessAttack()
        {
            SetState(WeaponState.Shooting);
        }

        /// <summary>
        /// Completely stops the weapon.
        /// </summary>
        /// <param name="skipCooldown">If set to true, cooldown is skipped. Defaults to false.</param>
        public virtual void StopWeapon(bool skipCooldown = false)
        {
            SetState(skipCooldown ? WeaponState.Idle : WeaponState.Recharging);
            buttonReleased = false;

            OnStopWeapon();
        }
        #endregion

        #region State Machine
        /// <summary>
        /// Used to set the state.
        /// Each state has its own condition for switching. If the condition is not met, the state is not switched.
        /// </summary>
        public bool SetState(WeaponState newState)
        {
            bool stateChanged = false;
            if (state == WeaponState.Idle)
            {
                if (newState == WeaponState.Requested)
                {
                    state = newState;
                    stateChanged = true;
                }
            }
            else if (state == WeaponState.Requested)
            {
                if (newState == WeaponState.Idle || newState == WeaponState.Started || newState == WeaponState.Stopping)
                {
                    state = newState;
                    stateChanged = true;
                }
            }
            else if (state == WeaponState.Started)
            {
                if (newState == WeaponState.Idle || newState == WeaponState.Shooting || newState == WeaponState.Stopping)
                {
                    state = newState;
                    stateChanged = true;
                }
            }
            else if (state == WeaponState.Shooting)
            {
                if (newState == WeaponState.Stopping || newState == WeaponState.Recharging)
                {
                    state = newState;
                    stateChanged = true;
                }
            }
            else if (state == WeaponState.Stopping)
            {
                if (newState == WeaponState.Idle || newState == WeaponState.Recharging || newState == WeaponState.Shooting)
                {
                    state = newState;
                    stateChanged = true;
                }
            }
            else if (state == WeaponState.Recharging)
            {
                if (newState == WeaponState.Idle)
                {
                    state = newState;
                    stateChanged = true;
                }
            }

            if (stateChanged)
            {
                ProcessState();
            }

            return stateChanged;
        }

        /// <summary>
        /// If SetState was successful, the new state is processed here.
        /// </summary>
        private void ProcessState()
        {
            switch (state)
            {
                case WeaponState.Idle:
                    break;
                case WeaponState.Requested:
                    if (resource != null)
                    {
                        if (!resource.CheckResource())
                        {
                            // Display message
                            SetState(WeaponState.Idle); // Cancel completely.
                            break;
                        }
                    }
                    SetState(WeaponState.Started);
                    break;
                case WeaponState.Started:
                    StartCoroutine(StartWeapon());
                    break;
                case WeaponState.Shooting:
                    if (resource != null)
                    {
                        if (!resource.Consume())
                        {
                            // Display message
                            SetState(WeaponState.Idle);
                            break;
                        }
                    }

                    if (processor != null)
                    {
                        // If resource component exists, pass in a function that checks the resource.
                        if (resource != null)
                        {
                            processor.ProcessAttack(Attack(), () => StopWeapon(), buttonReleased, () =>
                            {
                                if (!resource.Consume())
                                {
                                    // Display message
                                    SetState(WeaponState.Recharging);
                                    return false;
                                }
                                return true;
                            });
                        }
                        else
                        {
                            processor.ProcessAttack(Attack(), () => StopWeapon(), buttonReleased);
                        }
                    }
                    else // Regular weapon processing. Never use this, always add a processor to your weapon.
                    {
                        StartCoroutine(Attack());
                        StopWeapon();
                    }
                    break;
                case WeaponState.Stopping:
                    buttonReleased = true; // For various functions.

                    OnStopEquipment();

                    // Stops if the coroutine was running.
                    StopCoroutine(StartWeapon());

                    // If charge component exists, try to cancel the charging.
                    if (charge != null)
                    {
                        if (charge.CancelCharging(() => StopWeapon(true))) // Try to cancel. If successful, return out.
                        {
                            break;
                        }
                    }

                    if (processor != null)
                    {
                        SetState(WeaponState.Shooting); // If a processor exists, process again. Uses the "stopped" variable to check when it is executed.
                        break;
                    }

                    StopWeapon(); // If this is reached, weapon is stopped completely and the cooldown will start.
                    break;
                case WeaponState.Recharging:
                    cooldown.Start();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Overridables
        /// <summary>
        /// Implement the attack logic here.
        /// </summary>
        protected abstract IEnumerator Attack();

        /// <summary>
        /// Called when the weapon is started, before proceeding to process the attack. 
        /// Do any weapon-specific preparations here.
        /// </summary>
        protected virtual void PrepareAttack()
        {

        }

        /// <summary>
        /// Implement logic here for when the StopEquipment is called.
        /// </summary>
        protected virtual void OnStopEquipment()
        {

        }

        /// <summary>
        /// Implement logic here for when the weapon is fully stopped.
        /// </summary>
        protected virtual void OnStopWeapon()
        {

        }
        #endregion

        #endregion
    }
}
