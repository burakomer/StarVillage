﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	public interface IEquipmentItem
	{
		string TargetInventory { get; }

		//IEnumerator Equip(CharacterEquipmentManager equipmentManager, int slotIndex);
		bool Equip(CharacterEquipmentManager equipmentManager, int slotIndex);
		void Unequip(CharacterEquipmentManager equipmentManager, int slotIndex);
	}
}