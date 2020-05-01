using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public enum AttackTime { Immediate, ButtonRelease }

    [RequireComponent(typeof(Weapon))]
    [DisallowMultipleComponent]
    public abstract class WeaponProcessor : WeaponComponent
    {
        public AttackTime attackTime;

        public void ProcessAttack(Action AttackMethod, Action StopMethod, bool calledOnStop, Action ConsumeMethod = null)
        {
            if ((attackTime == AttackTime.Immediate && !calledOnStop)
                || (attackTime == AttackTime.ButtonRelease && calledOnStop))
            {
                AttackLogic(AttackMethod, StopMethod, ConsumeMethod);
            }
        }

        protected abstract void AttackLogic(Action AttackMethod, Action StopMethod, Action ConsumeMethod = null);
    }
}
