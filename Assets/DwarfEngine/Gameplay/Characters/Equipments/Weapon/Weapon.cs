using System.Collections;
using UnityEngine;

namespace DwarfEngine
{
    public enum WeaponState { Idle, Requested, Started, Shooting, Stopping, Recharging }

    [RequireComponent(typeof(WeaponAim))]
    [DisallowMultipleComponent]
    public abstract class Weapon : Equipment, IActiveEquipment
    {
        [Header("Graphics")]
        public GameObject weaponModel;
        [SerializeField] protected SpriteRenderer _renderer;

        [Header("Settings")]
        public Stat damage;
        public Stat rechargeTime;
        public LayerMask hitMask;

        [Header("Animator Parameters")]
        public string weaponAnimName;

        protected WeaponState state;
        protected Cooldown cooldown;
        protected bool stopped;


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
            cooldown = new Cooldown(rechargeTime.FloatValue, () => SetState(WeaponState.Idle));
            state = WeaponState.Idle;

            aim = GetComponent<WeaponAim>();
            charge = GetComponent<WeaponCharge>();
            resource = GetComponent<WeaponResource>();
            processor = GetComponent<WeaponProcessor>();

            //_renderer = weaponModel.GetComponent<SpriteRenderer>();

            //Init();
        }

        private void Update()
        {
            cooldown.Update();
        }
        #endregion

        #region Active Equipment
        public void StopEquipment()
        {
            SetState(WeaponState.Stopping);
        }

        public bool StartEquipment()
        {
            return SetState(WeaponState.Requested); // Request to start the weapon from scratch. Works only if the current state is Idle.
        }

        public IEnumerator StartWeapon()
        {
            PrepareAttack(); // Implemented in subclasses of this.

            if (charge != null)
            {
                yield return StartCoroutine(charge.StartCharging()); // If charge exists, start charging and wait till it completes.
            }

            ProcessAttack(); // Proceed to attack.
        }

        public void ProcessAttack()
        {
            SetState(WeaponState.Shooting);
        }

        public virtual void StopWeapon(bool skipCooldown = false)
        {
            SetState(skipCooldown ? WeaponState.Idle : WeaponState.Recharging);
            stopped = false;

            OnStopWeapon();
        }
        #endregion

        #region State Machine
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
                        if (resource != null)
                        {
                            processor.ProcessAttack(Attack, () => StopWeapon(), stopped, () =>
                            {
                                if (!resource.Consume())
                                {
                                    // Display message
                                    SetState(WeaponState.Recharging);
                                    return;
                                }
                            });
                        }
                        else
                        {
                            processor.ProcessAttack(Attack, () => StopWeapon(), stopped);
                        }
                    }
                    else // Regular weapon processing
                    {
                        Attack();
                        StopWeapon();
                    }
                    break;
                case WeaponState.Stopping:
                    stopped = true; // For various functions.

                    OnStopEquipment(); // Implemented in subclasses of this.

                    StopCoroutine(StartWeapon()); // Stops if it's running.

                    if (charge != null)
                    {
                        if (charge.CancelCharging(() => StopWeapon(true))) // Try to cancel. If successful, return out.
                        {
                            break;
                        }
                    }

                    if (processor != null)
                    {
                        SetState(WeaponState.Shooting); // If a processor exists, process again. Uses stopped.
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
        protected virtual void PrepareAttack()
        {

        }

        /// <summary>
        /// Handle shooting and animation logic here.
        /// </summary>
        protected abstract void Attack();

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
