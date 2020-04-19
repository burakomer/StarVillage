using UnityEngine;
using UniRx;
using System.Collections;

namespace DwarfEngine
{
    public enum WeaponState { Idle, Requested, Started, Shooting, Stopping, Recharging }

    [RequireComponent(typeof(WeaponAim))]
    public abstract class Weapon : Equipment, IActiveEquipment
    {
        [Header("Graphics")]
        public SpriteRenderer weaponModel;

        [Header("Settings")]
        public int damage;
        public float rechargeTime;
        public LayerMask hitMask;

        //[Header("Animator Parameters")]

        protected WeaponState state;
        protected Cooldown cooldown;
        protected bool stopped;

        #region Components
        protected WeaponAim aim;
        protected WeaponCharge charge;
        protected WeaponResource resource;
        protected WeaponProcessor processor;
        #endregion

        #region Logic
        #region Initialization and Update
        private void Start()
        {
            cooldown = new Cooldown(rechargeTime, () => SetState(WeaponState.Idle));
            state = WeaponState.Idle;

            aim = GetComponent<WeaponAim>();
            charge = GetComponent<WeaponCharge>();
            resource = GetComponent<WeaponResource>();
            processor = GetComponent<WeaponProcessor>();

            Init();
        }

        private void Update()
        {
            cooldown.Update();
        }

        protected virtual void Init()
        {

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

        #region Equipment
        public override void EquipLogic()
        {

        }

        public override void UnequipLogic()
        {

        }

        public override void SetOwner(Character _owner)
        {
            base.SetOwner(_owner);
        }
        #endregion 
        #endregion
    }
}
