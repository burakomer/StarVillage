using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	public abstract class ProjectileWeapon : Weapon
	{
		[Header("Projectile Weapon")]
		public float amountToShoot;
		public float projectileSpeed;
		public float maxDistance;
		public Vector2 fireOffset;

		protected GameObject currentProjectile;

		protected ObjectPooler pooler;

		protected override void Init()
		{
			base.Init();
			pooler = GetComponent<ObjectPooler>();
			pooler.Initialization(false, owner.gameObject.layer);
			pooler.UpdatePool += OnUpdatePool;

			foreach (GameObject obj in pooler.pooledObjects)
			{
				OnUpdatePool(obj);
				//p.transform.localPosition += Owner._characterWeaponUser.weaponHolder.transform.localPosition;
			}
		}

		private void OnUpdatePool(GameObject obj)
		{
			Projectile p = obj.GetComponent<Projectile>();
			p.damage = damage;
			p.speed = projectileSpeed;
			p.fireOffset = owner.transform.localPosition;
			p.maxDistance = maxDistance;
			p.hitMask = hitMask;
			p.fireOffset = fireOffset;
		}

		/// <summary>
		/// Call this in Shoot or PreWindUp.
		/// </summary>
		protected void SetupProjectile()
		{
			currentProjectile = pooler.GetPooledObject();

			if (currentProjectile == null)
			{
				return;
			}

			//currentProjectile.transform.position = transform.position + fireOffset.ToVector3();
			//currentProjectile.transform.Rotate(Vector3.forward, angle);
			currentProjectile.transform.SetParent(weaponModel.transform, false);
			currentProjectile.gameObject.SetActive(true);
		}

		protected override void Attack()
		{
			if (currentProjectile != null)
			{
				ShootProjectile();
			}
		}

		//protected override void Aim(Vector2 direction)
		//{
		//	if (currentProjectile != null)
		//	{
		//		if (angle >= 90 || angle <= -90)
		//		{
		//			currentProjectile.transform.rotation = Quaternion.Euler(0f, 180f, angle);
		//		}
		//		else
		//		{
		//			currentProjectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
		//		}
		//	}
		//}

		private void ShootProjectile()
		{
			pooler.SetParentToContainer(currentProjectile.transform);
			currentProjectile.GetComponent<Projectile>().Shoot();
			currentProjectile = null;
		}
	}
}
