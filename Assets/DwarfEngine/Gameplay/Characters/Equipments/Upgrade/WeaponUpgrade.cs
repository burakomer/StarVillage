using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class WeaponUpgrade : EquipmentUpgrade<Weapon>
    {
        public int damageFlat;
        public float damagePercentage;

        protected override void OnNewEquipment(Equipment equipment)
        {
            if (damageFlat > 0)
            {
                (equipment as Weapon).damage.ModifyStat(gameObject, damageFlat, StatModifyType.Add); 
            }

            if (damagePercentage > 0.02)
            {
                (equipment as Weapon).damage.ModifyStat(gameObject, damagePercentage, StatModifyType.Add);
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
                        (slot.equipment as Weapon).damage.ModifyStat(gameObject, damageFlat, StatModifyType.Remove);
                    }

                    if (damagePercentage > 0.02)
                    {
                        (slot.equipment as Weapon).damage.ModifyStat(gameObject, damagePercentage, StatModifyType.Remove);
                    }
                }
            }
        }
    }
}
