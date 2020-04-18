using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class SingleShotProcessor : WeaponProcessor
    {
        protected override void AttackLogic(Action AttackMethod, Action StopMethod, Action ConsumeMethod = null)
        {
            AttackMethod();
            StopMethod();
        }
    }
}
