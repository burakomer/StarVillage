using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class TopdownController2D : MonoBehaviour
	{
		private const float MAX_JUMP_HEIGHT = 128f;

		public Transform body;
		public Transform shadow;
		[Space]

		public float bodyZ;
		public float groundZ;

		public float jumpHeight = 4;
		public float timeToJumpApex = .4f;
		public float elasticity;

		private float verticalSpeed;

		private float jumpVelocity;
		private float gravity;

		private bool isBouncing;
		[SerializeField] private bool jumpInitiated;

		private Health _health;
		private Rigidbody2D _rigidbody;

		private void Start()
		{
			_health = GetComponent<Health>();
			_rigidbody = GetComponent<Rigidbody2D>();

			if (_health != null)
			{
				_health.OnDamage += Push;
			}

			gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
			jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		}

		private void OnDestroy()
		{
			if (_health != null)
			{
				_health.OnDamage -= Push;
			}
		}

		private void FixedUpdate()
		{
			if (bodyZ > 0) verticalSpeed += gravity * Time.fixedDeltaTime;

			bodyZ += verticalSpeed;

			if (bodyZ < 0)
			{
				// Simulate a bounce (to avoid negative Z)
				bodyZ = -bodyZ;

				// If the object was falling down, reduce the speed:
				if (verticalSpeed < 0) verticalSpeed = -verticalSpeed * 0.6f / elasticity;
				
				// If the speed is now below the threshold, snap the object to the ground
				if (verticalSpeed < 1.8)
				{
					bodyZ = 0;
					verticalSpeed = 0;
					isBouncing = false;
				}
			}

			body.localPosition = bodyZ.ToVector3(0, 1, 0);

			if (isBouncing)
			{
				shadow.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.1f, bodyZ / MAX_JUMP_HEIGHT);
			}
		}

		private void Bounce()
		{
			verticalSpeed = jumpVelocity;
			isBouncing = true;
		}

		public void Push(Vector2 direction)
		{
			_rigidbody.AddForce(direction * 2000f);
			Bounce();
		}
	}
}