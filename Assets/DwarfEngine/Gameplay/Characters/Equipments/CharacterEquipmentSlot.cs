using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(CharacterEquipmentManager))]
    public abstract class CharacterEquipmentSlot: MonoBehaviour
    {
        public string targetInventory;
        public Equipment equipment;
    }
}
