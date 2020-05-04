using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class SingleShotProcessor : WeaponProcessor
    {
        protected override IEnumerator AttackLogic(IEnumerator AttackMethod, Action StopMethod, Action ConsumeMethod = null)
        {
            yield return StartCoroutine(AttackMethod);
            StopMethod();
        }
    }
}
