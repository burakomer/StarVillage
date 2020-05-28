using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(Weapon))]
    [DisallowMultipleComponent]
    public class WeaponCharge : WeaponComponent
    {
        public Stat chargeTime;
        public bool cancellable;
        public bool prematureAttack;

        protected float chargeTimer;
        protected Coroutine chargeCoroutine;

        public IEnumerator StartCharging()
        {
            chargeCoroutine = StartCoroutine(_StartCharging());
            yield return chargeCoroutine;
        }

        public IEnumerator _StartCharging()
        {
            if (chargeTime.FloatValue > Time.deltaTime)
            {
                chargeTimer = 0;

                do
                {
                    chargeTimer += Time.deltaTime;
                    if (chargeTimer >= chargeTime.FloatValue)
                    {
                        chargeTimer = chargeTime.FloatValue;
                        break;
                    }
                    yield return null;
                } while (chargeTimer < chargeTime.FloatValue);
            }

            chargeCoroutine = null;
        }

        /// <summary>
        /// Cancels charging. If already charged, returns false. 
        /// If prematureAttack is set to true, weapon is fired.
        /// </summary>
        /// /// <param name="StopWeapon">Method to call to completely cancel the weapon in case premature attack is false.</param>
        public bool CancelCharging(Action StopWeapon)
        {
            if (chargeCoroutine == null)
            {
                return false;
            }

            if (!cancellable)
            {
                return true; // Returns true to prevent cancellation since it breaks the StopEquipment logic.
            }

            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
            
            if ((chargeTimer < chargeTime.FloatValue) && prematureAttack)
            {
                weapon.ProcessAttack();
            }
            else
            {
                StopWeapon();
            }
            return true;
        }
    }
}
