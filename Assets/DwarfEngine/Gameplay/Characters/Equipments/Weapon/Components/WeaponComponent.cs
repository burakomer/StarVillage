using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(Weapon))]
    public class WeaponComponent : MonoBehaviour
    {
        protected Weapon weapon;

        private void Start()
        {
            weapon = GetComponent<Weapon>();

            Init();
        }

        protected virtual void Init()
        {

        }
    }
}
