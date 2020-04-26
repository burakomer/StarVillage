﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/Items/Equipment/Weapon Item")]
	public class WeaponItemData : ItemData<Weapon>, IEquipmentItem
	{
		[SerializeField] private string targetInventory;
		public string TargetInventory => targetInventory;

		public bool Equip(CharacterEquipmentManager equipmentManager, int slotIndex)
		{
			return equipmentManager.Equip(itemObject, slotIndex);
		}

		public void Unequip(CharacterEquipmentManager equipmentManager, int slotIndex)
		{
			equipmentManager.Unequip<Weapon>(slotIndex);
		}
	}
}