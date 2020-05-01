using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class WeaponUpgrade : EquipmentUpgrade<Weapon>
    {
        [Header("Weapon Upgrades")]

        public int damageFlat;
        public float damagePercentage;

        public float chargeTimeReduction;

        protected override void OnNewEquipment(Equipment equipment)
        {
            if (damageFlat > 0)
            {
                (equipment as Weapon).damage.AddModifier(gameObject, damageFlat, StatModifierType.Flat); 
            }

            if (damagePercentage > 0.02f)
            {
                (equipment as Weapon).damage.AddModifier(gameObject, damagePercentage, StatModifierType.Percentage);
            }

            if (chargeTimeReduction > 0.02f)
            {
                var charge = (equipment as Weapon).GetComponent<WeaponCharge>();
                if (charge != null) charge.chargeTime.AddModifier(gameObject, -chargeTimeReduction, StatModifierType.Percentage);
            }
        }
        
        public override void RemoveUpgrade()
        {
            foreach (CharacterEquipmentSlot slot in slots)
            {
                if (slot.equipment != null)
                {
                    if (damageFlat > 0)
                    {
                        (slot.equipment as Weapon).damage.RemoveModifier(gameObject);
                    }

                    if (damagePercentage > 0.02)
                    {
                        (slot.equipment as Weapon).damage.RemoveModifier(gameObject);
                    }

                    if (chargeTimeReduction > 0.02f)
                    {
                        var charge = (slot.equipment as Weapon).GetComponent<WeaponCharge>();
                        if (charge != null) charge.chargeTime.RemoveModifier(gameObject);
                    }
                }
            }
        }
    }
}
