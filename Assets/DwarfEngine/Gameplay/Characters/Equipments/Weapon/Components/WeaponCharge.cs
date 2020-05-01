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
        public bool prematureAttack;

        protected float chargeTimer;
        protected Coroutine chargeCoroutine;

        public IEnumerator StartCharging()
        {
            Debug.Log(chargeTime.FloatValue);
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
        /// </summary>
        /// /// <param name="StopWeapon">Method to call to completely cancel the weapon in case premature attack is false.</param>
        /// <returns>True if cancel was successful.</returns>
        public bool CancelCharging(Action StopWeapon)
        {
            if (chargeCoroutine == null)
            {
                return false;
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
