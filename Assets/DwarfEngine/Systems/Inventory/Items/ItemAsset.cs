using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DwarfEngine
{
	/// <summary>
	/// The foundation of the ItemAsset class.
	/// </summary>
	public abstract class ItemAsset : ScriptableObject
	{
		/// <summary>
		/// The ID of the item in the database. It is the unique identifier in the database.
		/// </summary>
		public int id;

		/// <summary>
		/// The item name that will be displayed in the game.
		/// </summary>
		public string itemName;
		[Multiline] public string itemDescription;
		public Sprite icon;
	}

	/// <summary>
	/// The base class for inventory items.
	/// </summary>
	public abstract class ItemAsset<T> : ItemAsset where T : MonoBehaviour
	{
		/// <summary>
		/// The prefab object of the item. Must be set in the inspector.
		/// </summary>
		public T itemObject;

		// For later
		//public AssetReference itemObjectReference;
		/*
		public IEnumerator GetObject(Action<GameObject> OnSuccessCallback)
		{
			var asyncOp = itemObjectReference.LoadAssetAsync<GameObject>();

			yield return asyncOp;

			if (asyncOp.Status == AsyncOperationStatus.Succeeded)
			{
				OnSuccessCallback(asyncOp.Result);
			}
		}
		*/
	}
}