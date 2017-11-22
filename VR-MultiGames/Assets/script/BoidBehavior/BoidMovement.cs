using System;
using script.MovementScript;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.BoidBehavior
{
	public class BoidMovement : Movement
	{
		[Header("Setting")]

		[SerializeField] 
		private bool _useForce;

		[SerializeField]
		private bool _isRotateToVelocity = true;
		
		[SerializeField]
		private float _jumpForce = 5f;

		[Tooltip("Scale the rotation speed to look at velocity direction")] 
		[SerializeField]
		private float _rotationSyncScale = 5;

		public float RotationSyncScale
		{
			get { return _rotationSyncScale; }
		}

		// Use this for initialization
		public override void Move(Vector3 direction)
		{
			MoveToDirection(direction);

			if (_isRotateToVelocity && Controller.Rigidbody.velocity.sqrMagnitude > 1)
			{
				RotateToDirection(Controller.Rigidbody.velocity);
			}
		}

		private void MoveToDirection(Vector3 direction)
		{
			if (_useForce)
			{
				Controller.Rigidbody.AddForce(direction, ForceMode.Impulse);
			}
			else
			{
				Controller.Rigidbody.velocity += direction;
			}
		}

		private void RotateToDirection(Vector3 direction)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 
				Time.deltaTime * _rotationSyncScale);
		}

		public override void Jump(float scale)
		{
			if (!Controller.Rigidbody.useGravity) return;
			
			if (_useForce)
			{
				Controller.Rigidbody.AddForce(Vector3.up * _jumpForce * scale, ForceMode.Impulse);
			}
			else
			{
				Controller.Rigidbody.velocity += Vector3.up * _jumpForce * scale; 
			}
		}
	}
}
