using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(CharacterEquipmentManager))]
    public abstract class CharacterEquipmentSlot<T> : CharacterEquipmentSlot where T : Equipment
    {
        public T Equipment
        {
            get
            {
                return equipment as T;
            }
            set
            {
                equipment = value;
            }
        }

        public abstract Equipment Equip(T newEquipment);
        public abstract void Unequip();
    }

    public abstract class CharacterEquipmentSlot : CharacterAbility
    {
        public string targetInventory;
        public Equipment equipment;
    }
}
