using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	public abstract class ItemData
	{
		/// <summary>
		/// The ID of the item in the database. It is the unique identifier in the database.
		/// </summary>
		public int id { get; set; }
		public string itemName;
		[Multiline] public string itemDescription;
		public Sprite icon;
	}

	/// <summary>
	/// Base class for items.
	/// </summary>
	public abstract class ItemData<T> : ItemData where T : MonoBehaviour
	{
		public T itemObject;
	}
}