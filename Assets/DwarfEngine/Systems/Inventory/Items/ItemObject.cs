using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	public enum ItemType { Default, Consumable, Equipment }

	public abstract class ItemObject : ScriptableObject
	{
		/// <summary>
		/// The ID of the item in the database. It is the unique identifier in the database.
		/// </summary>
		public int id;

		public string itemName;
		[Multiline] public string itemDescription;
		public Sprite icon;
	}

	/// <summary>
	/// Base class for items.
	/// </summary>
	public abstract class ItemObject<T> : ItemObject where T : MonoBehaviour
	{
		public T itemObject;
	}
}