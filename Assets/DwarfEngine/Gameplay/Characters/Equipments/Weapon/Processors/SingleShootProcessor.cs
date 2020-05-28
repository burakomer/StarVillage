using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// The basic weapon processor. Shoots once and then stops the weapon.
    /// </summary>
    public class SingleShootProcessor : WeaponProcessor
    {
        protected override IEnumerator AttackLogic(IEnumerator AttackMethod, Action StopMethod, Func<bool> ConsumeMethod = null)
        {
            yield return StartCoroutine(AttackMethod);
            StopMethod();
        }
    }
}
