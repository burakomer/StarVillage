﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	/// <summary>
	/// ItemAsset for equipments.
	/// </summary>
	[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Data/Items/Equipment/Weapon Item")]
	public class WeaponItemAsset : ItemAsset<Weapon>, IEquipmentItem
	{
		/// <summary>
		/// Target inventory name of the weapon.
		/// </summary>
		[SerializeField] private string targetInventory;
		public string TargetInventory => targetInventory;

		public bool Equip(CharacterEquipmentManager equipmentManager, int slotIndex)
		{
			//yield return GetObject(itemObj => 
			//{
			//	equipSuccessful = equipmentManager.Equip(itemObj.GetComponent<Weapon>(), slotIndex);
			//});

			return equipmentManager.Equip(itemObject, slotIndex);
		}

		public void Unequip(CharacterEquipmentManager equipmentManager, int slotIndex)
		{
			equipmentManager.Unequip<Weapon>(slotIndex);
		}
	}
}