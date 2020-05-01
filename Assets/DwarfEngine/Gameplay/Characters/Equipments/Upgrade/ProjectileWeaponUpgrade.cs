using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class ProjectileWeaponUpgrade : WeaponUpgrade
    {
        public float projectileSpeedPercentage;

        protected override void OnNewEquipment(Equipment equipment)
        {
            if (equipment is ProjectileWeapon)
            {
                base.OnNewEquipment(equipment); 
            }

            if (projectileSpeedPercentage > 0.02f)
            {
                (equipment as ProjectileWeapon).projectileSpeed.AddModifier(gameObject, projectileSpeedPercentage, StatModifierType.Percentage); 
            }
        }

        public override void RemoveUpgrade()
        {
            foreach (CharacterEquipmentSlot slot in slots)
            {
                if (slot.equipment != null)
                {
                    if (slot.equipment is ProjectileWeapon)
                    {
                        base.RemoveUpgrade();
                    }

                    if (projectileSpeedPercentage > 0.02f)
                    {
                        (slot.equipment as ProjectileWeapon).projectileSpeed.RemoveModifier(gameObject);
                    }
                }
            }
        }
    }
}
