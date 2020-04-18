using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class RakeProcessor : WeaponProcessor
    {
        [Header("Rake Settings")]
        public float tickFrequency;

        protected override void AttackLogic(Action AttackMethod, Action StopMethod, Action ConsumeMethod = null)
        {
            
        }

        private IEnumerator Rake(Action AttackMethod, Action StopMethod)
        {
            do
            {
                AttackMethod();
                yield return new WaitForSeconds(tickFrequency);
            } while (true); // Condition
        }
    }
}
