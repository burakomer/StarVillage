using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DwarfEngine
{
	public enum ItemType { Default, Consumable, Equipment }

	public abstract class ItemAsset : ScriptableObject
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
	public abstract class ItemAsset<T> : ItemAsset where T : MonoBehaviour
	{
		public AssetReference itemObjectReference;
		public T itemObject;

		public IEnumerator GetObject(Action<GameObject> OnSuccessCallback)
		{
			var asyncOp = itemObjectReference.LoadAssetAsync<GameObject>();

			yield return asyncOp;

			if (asyncOp.Status == AsyncOperationStatus.Succeeded)
			{
				OnSuccessCallback(asyncOp.Result);
			}
		}
	}
}