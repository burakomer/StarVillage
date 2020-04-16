using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;

namespace DwarfEngine
{
    public enum AttackType { OneShot, Channeling, Rake }
    public enum WeaponState { Idle, WindUp, Shooting, Recharging }

    public abstract class Weapon : Equipment, IActiveEquipment
    {
        public Vector2 currentDirection { get; protected set; }

        [Header("Graphics")]
        public SpriteRenderer weaponModel;
        public SpriteRenderer directionIndicator;
        public bool rotateModel;
        //public ReactiveProperty<bool> shot;

        [Header("Attributes")]
        public AttackType attackType;
        public int damage;
        public float rechargeTime;
        public float windUpTime;
        public bool prematureShoot;
        public float tickFrequency;
        public LayerMask hitMask;

        [Header("Animator Parameters")]
        public string attackAnimationName;
        public int attackFrame;
        public int attackEndFrame;

        protected ReactiveProperty<WeaponState> state;
        protected Cooldown cooldown;
        protected float windUpTimer;

        protected float angle;

        private Coroutine windUpCoroutine;


        private void Start()
        {
            cooldown = new Cooldown(rechargeTime);
            state = new ReactiveProperty<WeaponState>();

            // Sets shot to true when 
            //weaponModel.currentAnimation
            //    .Where(anim => anim.animationName == attackAnimationName && anim.currentFrame == attackFrame)
            //    .Subscribe(_ =>
            //    {
            //        shot.Value = true;
            //    })
            //    .AddTo(this);
            //
            //// Stops the weapon when the specified frame is reached
            //weaponModel.currentAnimation
            //    .Where(anim => anim.animationName == attackAnimationName && anim.currentFrame == attackEndFrame)
            //    .Subscribe(_ =>
            //    {
            //        shot.Value = false;
            //        StopEquipment();
            //    })
            //    .AddTo(this);
            //
            //shot
            //    .Where(b => b)
            //    .Subscribe(_ =>
            //    {
            //        Shoot();
            //    })
            //    .AddTo(this);

            Init();
        }

        private void Update()
        {
            cooldown.Update();
        }

        protected virtual void Init()
        {

        }

        #region Active Equipment
        public void StopEquipment()
        {
            OnStopEquipment();

            if (windUpCoroutine != null)
            {
                StopCoroutine(windUpCoroutine);
                windUpCoroutine = null;
            }

            if (windUpTimer < windUpTime && prematureShoot)
            {
                windUpTimer = windUpTime;
                ProcessWeapon();
            }

            SetState(WeaponState.Recharging);

            if (attackType != AttackType.OneShot)
            {
                cooldown.Start(); 
            }
        }

        public bool StartEquipment()
        {
            if (!cooldown.isReady)
            {
                Debug.Log($"Cooldown not ready: {name}");
                return false;
            }

            windUpCoroutine = StartCoroutine(StartWindUp());
            return true;
        } 
        #endregion

        #region Wind Up
        private IEnumerator StartWindUp()
        {
            if (windUpTime > 0)
            {
                SetState(WeaponState.WindUp);
                windUpTimer = 0;

                PreWindUp();
                do
                {
                    windUpTimer += Time.deltaTime;
                    if (windUpTimer >= windUpTime)
                    {
                        windUpTimer = windUpTime;
                        break;
                    }
                    WindUpLogic(windUpTimer / windUpTime);
                    yield return null;
                } while (windUpTimer < windUpTime);
            }

            windUpCoroutine = null;
            ProcessWeapon();
        }

        protected virtual void PreWindUp()
        {

        }

        protected virtual void WindUpLogic(float windUpNormalized)
        {

        }
        #endregion

        #region Weapon Processing
        protected virtual void OnStopEquipment()
        {

        }

        /// <summary>
        /// Handle shooting and animation logic here.
        /// </summary>
        protected abstract void Shoot();

        /// <summary>
        /// Handle aim logic here.
        /// </summary>
        /// <param name="direction"></param>
        protected virtual void Aim(Vector2 direction)
        {
            angle = Vector2.SignedAngle(Vector2.right, direction);
            currentDirection = direction.normalized;

            if (directionIndicator != null)
            {
                directionIndicator.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            if (rotateModel)
            {
                if (angle >= 90 || angle <= -90)
                {
                    weaponModel.transform.rotation = Quaternion.Euler(0f, 180f, 180f - angle);
                    //weaponModel.flipX = true;
                }
                else
                {
                    weaponModel.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                    //weaponModel.flipX = false;
                }
                //fireFeedback.transform.parent.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private void ProcessWeapon()
        {
            SetState(WeaponState.Shooting);

            switch (attackType)
            {
                case AttackType.OneShot:
                    Shoot();
                    cooldown.Start();
                    //StopEquipment();
                    break;
                case AttackType.Channeling:
                    StartCoroutine(Channel());
                    break;
                case AttackType.Rake:
                    StartCoroutine(Rake());
                    break;
            }
        }

        #region Channeling
        private IEnumerator Channel()
        {
            Shoot();
            do
            {
                ChannelTick();
                yield return new WaitForSeconds(tickFrequency);
            } while (state.Value == WeaponState.Shooting);
        }

        /// <summary>
        /// Must override if attack type is Channeling
        /// </summary>
        protected virtual void ChannelTick()
        {

        }
        #endregion

        #region Rake
        private IEnumerator Rake()
        {
            do
            {
                Shoot();
                yield return new WaitForSeconds(tickFrequency);
            } while (state.Value == WeaponState.Shooting);
        }
        #endregion 

        private void SetState(WeaponState newState)
        {
            state.Value = newState;
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

            _owner.brain.inputs.look
                .Where(v => v != Vector2.zero)
                .Subscribe(v =>
                {
                    Aim(v);
                })
                .AddTo(this);
        }
        #endregion
    }
}
