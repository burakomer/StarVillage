using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// A weapon processor that works well with machine gun type weapons.
    /// </summary>
    public class RakeProcessor : WeaponProcessor
    {
        [Header("Rake Settings")]
        public float tickFrequency;

        /// <summary>
        /// Must be set to higher than 0 if the attackTime is set to ButtonRelease. 
        /// Or else it won't attack more than once.
        /// </summary>
        public int shootAmount;

        /// <summary>
        /// How much shoot left.
        /// </summary>
        private int shootsLeft;

        protected override IEnumerator AttackLogic(IEnumerator AttackMethod, Action StopMethod, Func<bool> ConsumeMethod = null)
        {
            switch (attackTime)
            {
                case AttackTime.Immediate:
                    if (ConsumeMethod != null)
                    {
                        if (shootAmount > 0)
                        {
                            shootsLeft = shootAmount;
                            do
                            {
                                yield return StartCoroutine(AttackMethod);
                                yield return new WaitForSeconds(tickFrequency);
                            } while (ConsumeMethod() && shootsLeft > 0 && !buttonReleased);
                            StopMethod();
                        } 
                        else
                        {
                            do
                            {
                                yield return StartCoroutine(AttackMethod);
                                shootsLeft--;
                                yield return new WaitForSeconds(tickFrequency);
                            } while (ConsumeMethod() && !buttonReleased);
                            StopMethod();
                        }
                    }
                    else
                    {
                        if (shootAmount > 0)
                        {
                            do
                            {
                                yield return StartCoroutine(AttackMethod);
                                shootsLeft--;
                                yield return new WaitForSeconds(tickFrequency);
                            } while (shootsLeft > 0 && !buttonReleased);
                            StopMethod();
                        }
                        else
                        {
                            do
                            {
                                yield return StartCoroutine(AttackMethod);
                                shootsLeft--;
                                yield return new WaitForSeconds(tickFrequency);
                            } while (!buttonReleased);
                            StopMethod();
                        }
                    }
                    break;
                case AttackTime.ButtonRelease:
                    Debug.LogError("RakeProcessor process logic on ButtonRelease not implemented yet!");
                    StopMethod();
                    break;
                default:
                    Debug.LogError("<color=red><b>FIX ME!</b></color>");
                    StopMethod();
                    break;
            }
        }
    }
}
