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
            EquipLogic();
        }

        private void OnDestroy()
        {
            UnequipLogic();
        }

        /// <summary>
        /// Called after the owner is set.
        /// </summary>
        protected abstract void EquipLogic();

        /// <summary>
        /// Called when destroyed.
        /// </summary>
        protected abstract void UnequipLogic();
    }
}
