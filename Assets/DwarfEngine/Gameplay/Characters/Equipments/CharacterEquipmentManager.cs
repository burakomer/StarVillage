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
        [HideInInspector] public CharacterWeaponEquipmentSlot weaponSlot;

        public List<CharacterEquipmentSlot> equipmentSlots;
        public List<IActiveEquipmentSlot> activeEquipmentSlots;

        protected override void Init()
        {
            base.Init();

            equipmentSlots = new List<CharacterEquipmentSlot>();
            activeEquipmentSlots = new List<IActiveEquipmentSlot>();

            foreach (CharacterEquipmentSlot slot in GetComponents<CharacterEquipmentSlot>())
            {
                equipmentSlots.Add(slot);
                if (slot is IActiveEquipmentSlot)
                {
                    activeEquipmentSlots.Add(slot as IActiveEquipmentSlot);
                }

                if (slot is CharacterWeaponEquipmentSlot)
                {
                    weaponSlot = slot as CharacterWeaponEquipmentSlot;
                }
            }
        }
    }
}
