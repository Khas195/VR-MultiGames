using System;
using script.MovementScript;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.Boid_Flocking
{
	public class BoidMovement : Movement
	{
		[SerializeField] public float MinSpeed = 2f;
		[SerializeField] public float MaxSpeed = 10f;
		[SerializeField] public float Speed = 8f;

		private Rigidbody _rigidbody;
		
		private void Start()
		{
			GroundCheck();
			_rigidbody = Controller.GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			if (_rigidbody.velocity.magnitude < MinSpeed)
			{
				if (_rigidbody.velocity.magnitude <= 0.00001f)
				{			
					Move(RandomVector());
				}
				else
				{
					Move(_rigidbody.velocity);
				}
			}
		}

		private Vector3 RandomVector()
		{
			Random.InitState((int) Time.time);
			return new Vector3(Random.value, 0, Random.value);
		}

		// Use this for initialization
		public override void Move(Vector3 direction)
		{
			if (direction.magnitude > 1f)
			{
				direction.Normalize();
			}

			GroundCheck();

			if (!IsGrounded) return;

			_rigidbody.AddForce(direction * Speed, ForceMode.Impulse);

			if (_rigidbody.velocity.magnitude > MaxSpeed)
			{
				_rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, MaxSpeed);
			}
		}

		public override void Jump(float scale)
		{
			
		}
	}
}
