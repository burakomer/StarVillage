using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class Equipment : MonoBehaviour, IEquipment
    {
        public Character owner { get; set; }

        public void SetOwner(Character _owner)
        {
            owner = _owner;
        }

        public abstract void EquipLogic();
        public abstract void UnequipLogic();
    }
}
