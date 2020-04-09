using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfEngine
{
    public class CharacterEquipmentManager : CharacterAbility
    {
        public List<CharacterEquipmentSlot> equipmentSlots;
        public List<IActiveEquipmentSlot> activeEquipmentSlots;

        protected override void Init()
        {
            base.Init();

            foreach (CharacterEquipmentSlot slot in GetComponents<CharacterEquipmentSlot>())
            {
                equipmentSlots.Add(slot);
                if (slot is IActiveEquipmentSlot)
                {
                    activeEquipmentSlots.Add(slot as IActiveEquipmentSlot);
                }
            }
        }
    }
}
