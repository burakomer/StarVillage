using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfEngine
{
	[RequireComponent(typeof(FlexibleGridLayout))]
	public class InventoryUI : MonoBehaviour
	{
		public GameObject slotPrefab;
		[Space]

		[SerializeField] private Inventory _inventory;
		[SerializeField] private List<InventorySlot> slots;

		/// <summary>
		/// For initializing slot UI from Inspector.
		/// </summary>
		public void SetSlots()
		{
			//FlexibleGridLayout grid = GetComponent<FlexibleGridLayout>();
			slots = new List<InventorySlot>(_inventory.size);
			
			for (int i = 0; i < slots.Capacity; i++)
			{
				slots.Add(Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, transform).GetComponentInChildren<InventorySlot>());
			}
		}

		private void Start()
		{
			for (int i = 0; i < slots.Capacity; i++)
			{
				slots[i].SetItem(_inventory[i]);
			}
		}
	} 
}
