using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DwarfEngine
{
    public class CharacterEquipmentManager : CharacterAbility
    {
        public event Action<Equipment> OnEquip;
        public event Action<Equipment> OnUnequip;

        public List<IActiveEquipmentSlot> activeEquipmentSlots;
        private Dictionary<Type, CharacterEquipmentSlot[]> equipmentSlots;

        protected override void Init()
        {
            base.Init();

            equipmentSlots = new Dictionary<Type, CharacterEquipmentSlot[]>();
            activeEquipmentSlots = new List<IActiveEquipmentSlot>();

            foreach (CharacterEquipmentSlot slot in GetComponents<CharacterEquipmentSlot>())
            {
                //var attribute = slot.GetType()
                //    .GetCustomAttributes(typeof(EquipmentSlotAttribute), true)
                //    .FirstOrDefault() as EquipmentSlotAttribute;

                var equipmentType = slot.GetType().BaseType.GetGenericArguments().FirstOrDefault();

                if (equipmentSlots.ContainsKey(equipmentType))
                {
                    equipmentSlots[equipmentType].Append(slot);
                }
                else
                {
                    equipmentSlots.Add(equipmentType, new CharacterEquipmentSlot[] { slot });
                }

                if (slot is IActiveEquipmentSlot)
                {
                    activeEquipmentSlots.Add(slot as IActiveEquipmentSlot);
                }
            }
        }

        public CharacterEquipmentSlot[] GetSlots<TEq>() where TEq : Equipment
        {
            return equipmentSlots[typeof(TEq)];
        }

        public CharacterEquipmentSlot GetSlot<TEq>(int slotIndex) where TEq : Equipment
        {
            return equipmentSlots[typeof(TEq)][slotIndex];
        }

        public TSlot GetSlot<TSlot, TEq>(int slotIndex) where TSlot : CharacterEquipmentSlot where TEq : Equipment
        {
            return equipmentSlots[typeof(TEq)][slotIndex] as TSlot;
        }

        public bool Equip<T>(T newEquipment, int slotIndex = 0) where T : Equipment
        {
            (equipmentSlots[typeof(T)][slotIndex] as CharacterEquipmentSlot<T>)
                .Equip(newEquipment) // TODO: Equip logic (level check, class check etc.)
                .SetOwner(_character);

            OnEquip?.Invoke(newEquipment);

            return true; 
        }

        public void Unequip<T>(int slotIndex = 0) where T : Equipment
        {
            (equipmentSlots[typeof(T)][slotIndex] as CharacterEquipmentSlot<T>)
                .Unequip();
        }
    }
}
