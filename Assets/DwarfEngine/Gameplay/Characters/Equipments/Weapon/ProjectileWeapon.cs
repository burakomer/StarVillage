using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	public enum ShootType 
	{ 
		Immediate, 
		ButtonRelease // TODO : Doesn't work with Rake
	}

	public abstract class ProjectileWeapon : Weapon
	{
		[Header("Projectile Weapon")]
		public ShootType shootType;
		public float amountToShoot;
		public float projectileSpeed;
		public float maxDistance;
		public Vector2 fireOffset;

		protected GameObject currentProjectile;

		private ObjectPooler _pooler;

		protected override void Init()
		{
			base.Init();
			_pooler = GetComponent<ObjectPooler>();
			_pooler.Initialization(false, owner.gameObject.layer);
			_pooler.UpdatePool += OnUpdatePool;

			foreach (GameObject obj in _pooler.pooledObjects)
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
			currentProjectile = _pooler.GetPooledObject();

			if (currentProjectile == null)
			{
				return;
			}

			//currentProjectile.transform.position = transform.position + fireOffset.ToVector3();
			//currentProjectile.transform.Rotate(Vector3.forward, angle);
			currentProjectile.transform.SetParent(weaponModel.transform, false);
			currentProjectile.gameObject.SetActive(true);
		}

		protected override void Shoot()
		{
			if (currentProjectile != null)
			{
				if (shootType == ShootType.Immediate)
				{
					ShootProjectile();
				}
			}
		}

		protected override void Aim(Vector2 direction)
		{
			base.Aim(direction);

			if (currentProjectile != null)
			{
				currentProjectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
				//if (angle >= 90 || angle <= -90)
				//{
				//	currentProjectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
				//	//weaponModel.flipX = true;
				//}
				//else
				//{
				//	currentProjectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
				//	//weaponModel.flipX = false;
				//}
			}
		}

		protected override void OnStopEquipment()
		{
			base.OnStopEquipment();

			if (shootType == ShootType.ButtonRelease)
			{
				ShootProjectile();
			}
		}

		private void ShootProjectile()
		{
			_pooler.SetParentToContainer(currentProjectile.transform);
			currentProjectile.GetComponent<Projectile>().Shoot();
			currentProjectile = null;
		}
	}
}
