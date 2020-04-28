using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class EquipmentUpgrade<T> : Upgrade where T : Equipment
    {
        protected CharacterEquipmentSlot[] slots;

        protected override void EquipLogic()
        {
            owner.equipmentManager.OnEquip += OnNewEquipment;

            slots = owner.equipmentManager.GetSlots<T>();

            foreach (CharacterEquipmentSlot slot in slots)
            {
                if (slot.equipment != null)
                {
                    OnNewEquipment(slot.equipment);
                }
            }

            base.EquipLogic();
        }

        protected abstract void OnNewEquipment(Equipment equipment);

        protected override void UnequipLogic()
        {
            owner.equipmentManager.OnEquip -= OnNewEquipment;
            base.UnequipLogic();
        }

        public override void ApplyUpgrade()
        {

        }
    }
}
