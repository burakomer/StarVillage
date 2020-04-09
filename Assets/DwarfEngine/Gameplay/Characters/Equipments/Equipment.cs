using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class Equipment : MonoBehaviour
    {
        public Character owner;

        public void SetOwner(Character _owner)
        {
            owner = _owner;
        }
    }
}
