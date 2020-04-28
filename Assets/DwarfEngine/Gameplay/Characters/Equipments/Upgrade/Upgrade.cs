using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class Upgrade : Equipment
    {
        protected override void EquipLogic()
        {
            ApplyUpgrade();
        }

        protected override void UnequipLogic()
        {
            RemoveUpgrade();
        }

        /// <summary>
        /// Apply the upgrade when it is equipped.
        /// </summary>
        public abstract void ApplyUpgrade();

        /// <summary>
        /// Remove the upgrade when it is unequipped.
        /// </summary>
        public abstract void RemoveUpgrade();
    }
}
