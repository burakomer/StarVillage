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
		}

		protected override void Shoot()
		{
			GameObject projectile = _pooler.GetPooledObject();

			if (projectile != null)
			{
				projectile.transform.position = transform.position;
				projectile.transform.Rotate(Vector3.forward, angle);
				projectile.gameObject.SetActive(true);
			}
		}
	}
}
