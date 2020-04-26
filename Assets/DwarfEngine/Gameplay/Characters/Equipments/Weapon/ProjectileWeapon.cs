﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	public abstract class ProjectileWeapon : Weapon
	{
		[Header("Projectile Weapon")]
		public Projectile projectile;
		public float amountToShoot;
		public float projectileSpeed;
		public float maxDistance;
		public Vector2 fireOffset;

		[Header("Object Pooler")]
		public int amountToPool;
		public bool expandInNeed;

		protected Projectile currentProjectile;
		protected ObjectPooler<Projectile> pooler;

		protected override void Init()
		{
			base.Init();
			pooler = new ObjectPooler<Projectile>(projectile, amountToPool, expandInNeed);
			pooler.Initialize(OnUpdatePool, owner.gameObject.layer);

			foreach (Projectile obj in pooler)
			{
				OnUpdatePool(obj);
			}
		}

		private void OnUpdatePool(Projectile p)
		{
			p.damage = damage;
			p.speed = projectileSpeed;
			p.fireOffset = owner.transform.localPosition;
			p.maxDistance = maxDistance;
			p.hitMask = hitMask;
			p.fireOffset = fireOffset;
		}

		/// <summary>
		/// Call this in Shoot or PreCharge.
		/// </summary>
		protected void SetupProjectile()
		{
			currentProjectile = pooler.Get();

			if (currentProjectile == null)
			{
				return;
			}

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

		private void ShootProjectile()
		{
			pooler.SetParentToContainer(currentProjectile.transform);
			
			currentProjectile.transform.localScale = Vector3.one;
			currentProjectile.Shoot();
			currentProjectile = null;
		}

		private void OnDestroy()
		{
			pooler.Destroy();
		}
	}
}
