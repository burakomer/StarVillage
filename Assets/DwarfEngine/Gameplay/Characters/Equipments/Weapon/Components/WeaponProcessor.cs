using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public enum AttackTime { Immediate, ButtonRelease, Both }

    [RequireComponent(typeof(Weapon))]
    [DisallowMultipleComponent]
    public abstract class WeaponProcessor : WeaponComponent
    {
        public AttackTime attackTime;

        public void ProcessAttack(IEnumerator AttackMethod, Action StopMethod, bool calledOnStop, Action ConsumeMethod = null)
        {
            if ((attackTime == AttackTime.Immediate && !calledOnStop)
                || (attackTime == AttackTime.ButtonRelease && calledOnStop)
                    || (attackTime == AttackTime.Both))
            {
                StartCoroutine(AttackLogic(AttackMethod, StopMethod, ConsumeMethod));
            }
        }

        protected abstract IEnumerator AttackLogic(IEnumerator AttackMethod, Action StopMethod, Action ConsumeMethod = null);
    }
}
