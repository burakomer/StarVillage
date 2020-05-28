using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public enum AttackTime { Immediate, ButtonRelease }

    [DisallowMultipleComponent]
    public abstract class WeaponProcessor : WeaponComponent
    {
        /// <summary>
        /// When the attack is executed.
        /// </summary>
        public AttackTime attackTime;

        protected bool buttonReleased;

        /// <summary>
        /// Called when the attack needs to be processed. 
        /// Executes the attack logic according to the attackTime setting.
        /// </summary>
        /// <param name="AttackMethod">The attack method that is specific to the weapon.</param>
        /// <param name="StopMethod">The stop method that must be called to stop the weapon.</param>
        /// <param name="ConsumeMethod">The consume method that checks the resource. Returns false if no resource left. Null by default.</param>
        public void ProcessAttack(IEnumerator AttackMethod, Action StopMethod, bool calledOnButtonRelease, Func<bool> ConsumeMethod = null)
        {
            buttonReleased = calledOnButtonRelease;

            if ((attackTime == AttackTime.Immediate && !calledOnButtonRelease)
                || (attackTime == AttackTime.ButtonRelease && calledOnButtonRelease))
            {
                StartCoroutine(AttackLogic(AttackMethod, StopMethod, ConsumeMethod));
            }
        }

        /// <summary>
        /// The attack logic of the processor.
        /// </summary>
        /// <param name="AttackMethod">The attack method that is specific to the weapon.</param>
        /// <param name="StopMethod">The stop method that must be called to stop the weapon.</param>
        /// <param name="ConsumeMethod">The consume method that checks the resource. Returns false if no resource left. Null by default.</param>
        protected abstract IEnumerator AttackLogic(IEnumerator AttackMethod, Action StopMethod, Func<bool> ConsumeMethod = null);
    }
}
