using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class CharacterUpgradeSlot : CharacterEquipmentSlot<Upgrade>
    {
        protected override void Init()
        {
            base.Init();

            if (Equipment != null)
            {
                Equipment = Instantiate(Equipment, _character.transform);
                Equipment.SetOwner(_character);
            }
        }

        public override Equipment Equip(Upgrade newEquipment)
        {
            Unequip();

            Equipment = Instantiate(newEquipment, _character.transform);
            return Equipment;
        }

        public override void Unequip()
        {
            if (Equipment != null)
            {
                Destroy(Equipment.gameObject);
                Equipment = null;
            }
        }
    }
}
